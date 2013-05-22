using System;
using System.Threading;

using System.Xml;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class Saver : IComponent
    {
        protected Application app = null;       // контекст в котором работает подсистема
        protected State state = State.Default;  // текущее состояние конвертера

        protected File file;                    // осуществляет работу с файлом на диске

        protected DateTime last_time;           // хранит предшествующее время
        protected TimeSpan timeout;             // интервал времени по истечении которого обновить метку время/смещение

        protected Mutex t_mutex;                // синхронизатор
        protected Timer timer = null;           // инициирует процедурур сохранения данных в файл

        protected int period = 1000;            // частота сохранения данных в файл
        protected long isconverted = 0;         // показывает отработал или нет конвертер

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
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Saver()
        {
            file = new File();
            file.OnErrorLoad += new EventHandler(file_OnErrorLoad);

            last_time = DateTime.MinValue;
            timeout = new TimeSpan(0, 0, 0, 0, (int)File.DEF_SPERIOD);

            t_mutex = new Mutex();
            timer = new Timer(TimerCallback, null, Timeout.Infinite, period);

            p_mutex = new Mutex();
            parameters = new System.Collections.Generic.List<Parameter>();
        }

        /// <summary>
        /// Не удалось загрузить файл
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void file_OnErrorLoad(object sender, EventArgs e)
        {
            if (OnError != null)
            {
                OnError(this, new ErrorArgs("Не удалось загрузить файл", ErrorType.NotFatal));
            }
        }

        /// <summary>
        /// Возвращяет текущее состояни конвертера
        /// </summary>
        public State State
        {
            get { return state; }
        }

        /// <summary>
        /// Возвращяет файл с которым осуществляется работа
        /// </summary>
        public File File
        {
            get { return file; }
        }

        /// <summary>
        /// Указывает выполнил конвертер работу или нет
        /// </summary>
        protected bool IsConverted
        {
            get { return (Interlocked.Read(ref isconverted) == 1); }
        }

        /// <summary>
        /// Запустить службу
        /// </summary>
        public void Run()
        {
            if (state == State.Default || state == State.Stopped)
            {
                state = State.Running;
                timer.Change(0, period);

                if (OnStart != null)
                {
                    OnStart(this, null);
                }
            }
        }

        /// <summary>
        /// Остановить службу
        /// </summary>
        public void Stop()
        {
            if (state == State.Running)
            {
                state = State.Stopped;
                timer.Change(Timeout.Infinite, period);

                if (OnStop != null)
                {
                    OnStop(this, null);
                }
            }
        }

        /// <summary>
        /// Выполнить инициализацию компонента
        /// </summary>
        public void Initialize()
        {
            app = Application.CreateInstance();
            if (app != null)
            {
            }
            else
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Подсистеме, осуществляющей сохранение данный в файл," +
                        " не удалось выполнить инициализацию!", ErrorType.Fatal));
                }
        }

        /// <summary>
        /// Конвертер выполнил свою работу
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        public void ConverterCompletedEventHandler(object sender, EventArgs e)
        {
            Interlocked.Exchange(ref isconverted, 1);
        }
    }
}