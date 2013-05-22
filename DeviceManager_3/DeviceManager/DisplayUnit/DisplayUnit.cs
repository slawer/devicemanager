using System;
using System.Threading;
using System.Collections.Generic;

using DeviceManager.Errors;
using SoftwareDevelopmentKit.Protocols.Dsn;

namespace DeviceManager
{
    /// <summary>
    /// Реализует формирование и отправку данных на блок отображения
    /// </summary>
    public partial class DisplayUnit : IComponent
    {
        protected Mutex t_mutex = null;                 // синхронизатор таймера
        protected TimerCallback sync_call;              // синхронный вызов процедуры отправки пакетов на блоки отображения

        protected State state;                          // текущее состояние компонента

        protected long counter;                         // счетчик контроля последовательного порта
        protected Application app;                      // API системы        

        protected Repository repository;                // репизитарий
        protected IAsyncResult async = null;            // описатель асинхронного запуска отправки данных на БО

        /// <summary>
        /// <summary>
        /// Генерируется при возникновении ошибки. Добавить в интерфейс
        /// </summary>
        public event ApplicationErrorHandler OnError;

        /// <summary>
        /// Возникает при запуске подсистемы
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// Возникает при остановке подсистемы
        /// </summary>
        public event EventHandler OnStop;

        /// <summary>
        /// Возникает при завершении обработки данных и отправке их на БО
        /// </summary>
        public event EventHandler OnComplete;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public DisplayUnit(Repository reposit)
        {
            t_mutex = new Mutex();

            counter = 0;
            state = State.Default;

            locker = new ReaderWriterLock();
            packets = new List<DisplayPacket>();

            sync_call = new TimerCallback(TimerCallback);
            repository = reposit;
        }

        /// <summary>
        /// Выполнить инициализацию компонента
        /// </summary>
        public void Initialize()
        {
            app = Application.CreateInstance();
            if (app != null)
            {
                protocol = Protocol.GetProtocol(ProtocolVersion.x100);
                if (protocol == null)
                {
                    if (OnError != null)
                    {
                        OnError(this, new ErrorArgs("БО не удалось выполнить инициализацию! Не получен экземпляр протокола обмена.", ErrorType.Fatal));
                    }
                }
            }
            else
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("БО не удалось выполнить инициализацию!", ErrorType.Fatal));
                }
        }

        /// <summary>
        /// Обработать сообщение конвертера о завершении преобразования поступивших данных
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        public void OnCompleteConverter(object sender, EventArgs e)
        {
            if (async == null)
            {
                Interlocked.Increment(ref counter);
                async = sync_call.BeginInvoke(null, null, null);
            }
            else
            {
                if (async.IsCompleted)
                {
                    Interlocked.Increment(ref counter);
                    async = sync_call.BeginInvoke(null, null, null);
                }
            }
        }

        /// <summary>
        /// Запустить отправку пакетов на блоки отображения
        /// </summary>
        public void Run()
        {
            if (state == State.Default || state == State.Stopped)
            {                
                if (OnStart != null)
                {
                    OnStart(this, new EventArgs());
                }

                state = State.Running;
            }
        }

        /// <summary>
        /// Остановить отправку пакетов на блоки отображения
        /// </summary>
        public void Stop()
        {
            if (state == State.Running)
            {
                if (OnStop != null)
                {
                    OnStop(this, new EventArgs());
                }

                state = State.Stopped;
                Interlocked.Exchange(ref counter, 0);
            }
        }
    }
}