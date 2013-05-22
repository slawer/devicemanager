using System;
using System.Threading;
using System.Collections.Generic;

using System.Xml;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует вычисление параметров на основе формул
    /// </summary>
    public partial class Converter : IComponent
    {
        // ---- константы класса ----

        /// <summary>
        /// Определяет количество параметров конвертера
        /// </summary>
        public const int ParametersCount = 1024;

        // ---- данные класса ----

        private Application app;                        // контекст в котором выполняется приложение

        protected ReaderWriterLock p_locker;            // синхронизирует доступ к параметрам
        protected ReaderWriterLock f_locker;            // синхронизирует доступ к макросам

        protected Float[] parameters = null;            // вычисленные значения параметров
        protected List<Formula> formuls = null;         // формулы по которым вычислять параметры

        protected State state;                          // текущее состояние конвертора

        protected Mutex t_mutex;                        // синхронизатор
        protected TimerCallback sync_call = null;       // синхронный вызов процедуры(по готовности)

        protected IAsyncResult async = null;            // описатель асинхронного запуска конвертера

        // ---- складыватель ----

        private Stock stock = null;                     // реализует первичную обработку пакетов

        /// <summary>
        /// Возникает когда вычислены параметра
        /// </summary>
        public event EventHandler OnComplete;

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
                    OnError(this, new ErrorArgs("Конвертору не удалось выполнить инициализацию!", ErrorType.Fatal));
                }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="stocker">Парсер пакетов</param>
        public Converter(Stock stocker, Float[] result)
        {
            parameters = result;

            p_locker = new ReaderWriterLock();
            f_locker = new ReaderWriterLock();

            formuls = new List<Formula>();            

            stock = stocker;
            state = State.Default;

            t_mutex = new Mutex();
            sync_call = new TimerCallback(TimerCallback);
        }

        /// <summary>
        /// Возвращяет текущее состояни конвертера
        /// </summary>
        public State State
        {
            get { return state; }
        }

        /// <summary>
        /// Запустить конвертер
        /// </summary>
        public void Run()
        {
            if (state == State.Default || state == State.Stopped)
            {
                state = State.Running;
                if (OnStart != null)
                {
                    OnStart(this, null);
                }
            }
        }

        /// <summary>
        /// Остановить работу конвертера
        /// </summary>
        public void Stop()
        {
            if (state == State.Running)
            {
                state = State.Stopped;
                if (OnStop != null)
                {
                    OnStop(this, null);
                }
            }
        }

        /// <summary>
        /// Получить срез данных на текущий момент времени
        /// </summary>
        /// <returns>Текущий срез данных</returns>
        public Float[] GetResults()
        {
            try
            {
                p_locker.AcquireReaderLock(300);
                try
                {
                    Float[] slice = new Float[parameters.Length];
                    parameters.CopyTo(slice, 0);

                    return slice;
                }
                finally
                {
                    p_locker.ReleaseReaderLock();
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Установить значение параметра который является захватом канала
        /// </summary>
        /// <param name="index">Номер параметра</param>
        /// <param name="value">Устанавливаемое значение</param>
        public void SetParameterValue(int index, float value)
        {
            try
            {
                f_locker.AcquireWriterLock(500);
                try
                {
                    if (index > -1)
                    {
                        foreach (Formula formula in formuls)
                        {
                            if (formula.Type == FormulaType.Capture)
                            {
                                if (formula.Position == index)
                                {
                                    formula.Macros.Value = value;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    f_locker.ReleaseWriterLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Получить значение параметра который является захватом канала
        /// </summary>
        /// <param name="index">Номер параметра</param>
        public float GetParameterValue(int index)
        {
            try
            {
                f_locker.AcquireWriterLock(500);
                try
                {
                    if (index > -1)
                    {
                        foreach (Formula formula in formuls)
                        {
                            if (formula.Type == FormulaType.Capture)
                            {
                                if (formula.Position == index)
                                {
                                    return formula.Macros.Value;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    f_locker.ReleaseWriterLock();
                }
            }
            catch { }
            return float.NaN;
        }

        /// <summary>
        /// Установить значение параметра
        /// </summary>
        /// <param name="index">Номер параметра</param>
        /// <param name="value">Устанавливаемое значение</param>
        public void SetValue(int index, float value)
        {
            try
            {
                f_locker.AcquireWriterLock(500);
                try
                {
                    if (index > -1)
                    {
                        foreach (Formula formula in formuls)
                        {
                            if (formula.Position == index)
                            {
                                formula.Macros.Value = value;
                            }
                        }
                    }
                }
                finally
                {
                    f_locker.ReleaseWriterLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Установить значение параметра
        /// </summary>
        /// <param name="index">Номер параметра</param>
        public float GetValue(int index)
        {
            try
            {
                f_locker.AcquireWriterLock(500);
                try
                {
                    if (index > -1)
                    {
                        foreach (Formula formula in formuls)
                        {
                            if (formula.Position == index)
                            {
                                return formula.Macros.Value;
                            }
                        }
                    }
                }
                finally
                {
                    f_locker.ReleaseWriterLock();
                }
            }
            catch { }
            return float.NaN;
        }

        /// <summary>
        /// Выполняет принудительный вызов процедуры обработки поступивших данных
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        public void SerialStaticEventHandler(object sender, EventArgs e)
        {
            if (async == null)
            {
                async = sync_call.BeginInvoke(null, null, null);
            }
            else
            {
                if (async.IsCompleted)
                {
                    async = sync_call.BeginInvoke(null, null, null);
                }
            }            
        }
    }
}