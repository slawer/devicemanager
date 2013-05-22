using System;
using System.Xml;
using System.Threading;
using System.ServiceModel;
using System.Collections.Generic;

using DeviceManager;
using DeviceManager.Errors;

namespace WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class Service : IComponent 
    {
        private static List<User> users = new List<User>();                     // подключенные пользователи системы
        private static ReaderWriterLock locker = new ReaderWriterLock();        // синхронизатор

        private static Application app = null;                                  // контекст в котором работает служба
        private static ServiceHost host = null;                                 // осуществляет хостинг службы

        private static State state = State.Default;                             // текущее состояние службы
        private static Place place = null;                                      // место в репозитарии

        private static Mutex t_mutex = new Mutex();                             // синхронизатор таймера
        private static Timer s_timer = new Timer(TimerCallback, null,           // таймер инициирующий
            Timeout.Infinite, 50);                                              // работу службы

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
        /// Запустить службу
        /// </summary>
        public void Run()
        {
            if (state == State.Default || state == State.Stopped)
            {
                state = State.Running;
                s_timer.Change(0, 50);

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
                s_timer.Change(Timeout.Infinite, 50);

                if (OnStop != null)
                {
                    OnStop(this, null);
                }
            }
        }

        public void close()
        {
            host.Close();
        }

        /// <summary>
        /// Выполнить инициализацию компонента
        /// </summary>
        public void Initialize()
        {
            try
            {
                app = Application.CreateInstance();
                if (app != null)
                {                    
                    app.Converter.OnComplete += new EventHandler(Converter_OnComplete);
                    if (app.Repository != null)
                    {
                        place = app.Repository.InstancePlace();
                    }

                    host = new ServiceHost(typeof(Service));
                    host.AddServiceEndpoint(typeof(IService), new NetTcpBinding(SecurityMode.None), new Uri("net.tcp://localhost:57000"));

                    host.Open();
                }
                else
                    if (OnError != null)
                    {
                        OnError(this, new ErrorArgs("Службe сетевого взаимодействия DeviceManager " + 
                            "не удалось выполнить инициализацию!", ErrorType.Fatal));
                    }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message, ErrorType.NotFatal));
                }
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
        /// Сохранить настройки
        /// </summary>
        /// <param name="document">Документ в который выполнить сохранение настроек</param>
        public void Save(XmlDocument document)
        {
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="root">Узел в котором находятся настройки подсистемы</param>
        public void Load(XmlNode root)
        {
        }

        /// <summary>
        /// Ядро службы
        /// </summary>
        /// <param name="state">Передаваемый параметр в процедуру. Не используется, всегда равен null</param>
        private static void TimerCallback(object state)
        {
            bool blocked = false;
            try
            {
                if (t_mutex.WaitOne(300, false))
                {
                    blocked = true;

                    DateTime now = DateTime.Now;
                    Float[] results = app.Converter.GetResults();

                    if (results != null)
                    {
                        Single[] slice = new float[results.Length];
                        for (int i = 0; i < results.Length; i++)
                        {
                            slice[i] = results[i].GetCurrentValue();
                        }

                        Service.SendData(now, slice);
                    }
                }
            }
            finally
            {
                if (blocked) t_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Конвертор завершил подсчет параметров
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private static void Converter_OnComplete(object sender, EventArgs e)
        {
        }
    }
}