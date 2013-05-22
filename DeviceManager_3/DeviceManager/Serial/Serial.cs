using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

using DeviceManager.Errors;

using System.Xml;

namespace DeviceManager
{
    /// <summary>
    /// Реализует обмен с устройствами через последовательный порт
    /// </summary>
    public partial class Serial : IComponent
    {
        // ---- данные класса ----

        protected Application app = null;                   // контекст в котором работает подсистема

        protected SerialPort port = null;                   // последовательный порт основной
        protected State state = State.Default;              // текущее состояние

        protected Mutex s_mutex;                            // синхронизирут доступ к спискам пакетов
        protected static List<Packet> static_list
            = new List<Packet>();                           // статический список

        protected Timer timer;                              // инициирует процедуру отправки пакетов в порт
        protected Mutex t_mutex = null;                     // синхронизирует процедуру таймера

        protected ManualResetEvent answerWaiter = null;     // оповещает о получении ответа на запрос
        protected Repository repository = null;             // хранилище пакетов для отправки в порт

        // ---- настроечные параметры ----

        protected int timerPerion = 10;                     // частота отправки пакетов в порт
        protected int answerTimeout = 100;                  // время ожидания ответа на запрос
                
        protected SecondaryPort s_port;                     // вспомогательный порт
        protected TypePort t_port = TypePort.Primary;       // тип порта

        /// <summary>
        /// Возникает при запуске подсистемы
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// Возникает при остановке подсистемы
        /// </summary>
        public event EventHandler OnStop;

        /// <summary>
        /// Генерируется при возникновении ошибки
        /// </summary>
        public event ApplicationErrorHandler OnError;

        /// <summary>
        /// Возникает когда последовательный порт отвалился
        /// </summary>
        public event EventHandler OnPortFail;

        /// <summary>
        /// Возникает при отправке/получении пакета в/из порт(а)
        /// </summary>
        public event SerialEventHandler OnPacket;

        /// <summary>
        /// Возникает когда завершен цикл опроса
        /// </summary>
        public event EventHandler OnComplete;

        /// <summary>
        /// Возникает когда завершен цикл опроса устройств
        /// </summary>
        public event EventHandler OnStaticComplete;

        /// <summary>
        /// Возникает когда завершен цикл опроса дополнительных команд
        /// </summary>
        public event EventHandler OnDynamicComplete;        

        /// <summary>
        /// инициализирует новый экземпляр класса
        /// </summary>
        public Serial(Repository reposit)
        {
            t_mutex = new Mutex();
            answerWaiter = new ManualResetEvent(true);

            timer = new Timer(TimerElapsed, null, Timeout.Infinite, timerPerion);

            //static_list = new List<Packet>();

            s_mutex = new Mutex();

            port = new SerialPort();
            passive_mutex = new Mutex();

            calculator = new CalculaterCRC();
            translater = new Translater(TranslaterFunction);

            input = new List<byte>();
            output = new List<byte>();

            opt_slim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            attemptsToRead = 1;
            attemptsCycled = 128;

            waitTimeout = 20;
            repository = reposit;

            is_primary_done = false;
            is_secondary_done = false;

            is_slim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        /// <summary>
        /// Возвращяет последовательный порт
        /// </summary>
        public SerialPort Port
        {
            get { return port; }
        }

        /// <summary>
        /// Возвращяет вспомогательный порт
        /// </summary>
        public SecondaryPort Secondary
        {
            get { return s_port; }
            set { s_port = value; }
        }

        /// <summary>
        /// Выполнить инициализацию
        /// </summary>
        public void Initialize()
        {
            app = Application.CreateInstance();
            if (app != null)
            {
                // ---- инициализируем порт настройками по умолчанию ----

                port.DataBits = 8;
                port.BaudRate = 38400;

                port.ReadBufferSize = 4096;
                port.WriteBufferSize = 4096;

                port.ReadTimeout = 30;
                port.WriteTimeout = 100;

                port.Parity = Parity.None;
                port.StopBits = StopBits.One;

                port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                port.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorReceived);

                // -------------------------------------------------------

                attemptsToRead = 1;
                attemptsCycled = 128;
            }
            else
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Последовательному порту не удалось выполнить инициализацию!", ErrorType.Fatal));
                }
        }

        /// <summary>
        /// Определяет количество попыток чтения пакета
        /// </summary>
        public int AttemptsToRead
        {           
            get 
            {
                if (opt_slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return attemptsToRead;
                    }
                    finally
                    {
                        opt_slim.ExitReadLock();
                    }
                }

                return 1;
            }
            
            set 
            {
                if (opt_slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        attemptsToRead = value;
                    }
                    finally
                    {
                        opt_slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет количество попыток чтения данных пакета
        /// </summary>
        public int AttemptsCycled
        {
            get 
            {
                if (opt_slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return attemptsCycled;
                    }
                    finally
                    {
                        opt_slim.ExitReadLock();
                    }
                }

                return 128;
            }

            set 
            {
                if (opt_slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        attemptsCycled = value;
                    }
                    finally
                    {
                        opt_slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет частота отправки пакетов в порт
        /// </summary>
        public int Period
        {
            get 
            {
                if (opt_slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return timerPerion;
                    }
                    finally
                    {
                        opt_slim.ExitReadLock();
                    }
                }

                return 50;
            }

            set 
            {
                if (opt_slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        timerPerion = value;
                    }
                    finally
                    {
                        opt_slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет время между отправкой пакетов в порт
        /// </summary>
        public int WaitTimeout
        {
            get
            {
                if (opt_slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return waitTimeout;
                    }
                    finally
                    {
                        opt_slim.ExitReadLock();
                    }
                }

                return 20;
            }

            set
            {
                if (opt_slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        waitTimeout = value;
                    }
                    finally
                    {
                        opt_slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Запустить обмен с устройствами
        /// </summary>
        public void Run()
        {
            if (state == State.Stopped || state == State.Default)
            {   
                timer.Change(0, timerPerion);
                if (OnStart != null)
                {
                    OnStart(this, new EventArgs());
                }

                state = State.Running;
            }

            if (!(this is SecondaryPort))
            {
                if (s_port.IsActived)
                {
                    s_port.Run();
                }
            }
        }

        /// <summary>
        /// Остановить обмен с устройствами
        /// </summary>
        public void Stop()
        {
            if (state == State.Running)
            {
                timer.Change(Timeout.Infinite, timerPerion);
                if (OnStop != null)
                {
                    OnStop(this, new EventArgs());
                }

                repository.Clear();
                state = State.Stopped;
            }

            if ((this is SecondaryPort) == false)
            {
                if (s_port.IsActived)
                {
                    s_port.Stop();
                }
            }
        }
    }
}