using System;
using System.Threading;
using System.Collections.Generic;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение пакетов поступивших от устройств и 
    /// передачу конвертеру сохраненных пакетов
    /// </summary>
    public partial class Stock : IComponent
    {
        // ---- данные класса ----

        protected Application app = null;                  // контекст в котором работает подсистема

        protected Mutex v_mutex;                           // синхронизирует доступ к извлеченным данным
        protected Mutex c_mutex;                           // синхронизирует доступ к списку условий

        protected Float[] slave;                           // извлеченные из пакета данные
        protected List<Parameter> parameters;              // условия исходя из которых извлекать данные из пакетов        
        
        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Stock(Float[] buffer)
        {
            v_mutex = new Mutex();
            c_mutex = new Mutex();

            slave = buffer;
            parameters = new List<Parameter>();
        }

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
            if (OnStart != null)
            {
                OnStart(this, null);
            }
        }

        /// <summary>
        /// Остановить службу
        /// </summary>
        public void Stop()
        {
            if (OnStop != null)
            {
                OnStop(this, null);
            }
        }

        /// <summary>
        /// Выполнить инициализацию
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
                    OnError(this, new ErrorArgs("Последовательному порту не удалось выполнить инициализацию!", ErrorType.Fatal));
                }
        }

        /// <summary>
        /// Выполняет обработку поступающего пакета
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="args">Аргументы события</param>
        public void SerialPacketHandler(object sender, SerialEventArgs args)
        {
            switch (app.Mode)
            {
                case ApplicationMode.Active:

                    ParseActive(args);
                    break;

                case ApplicationMode.Passive:

                    ParsePassive(args);
                    break;

                case ApplicationMode.Emulated:

                    ParsePassive(args);
                    break;
            }
        }

        /// <summary>
        /// Выполнить обработку пакета в активном режиме
        /// </summary>
        /// <param name="args">Аргументы события</param>
        private void ParseActive(SerialEventArgs args)
        {
            switch (args.Packet.Role)
            {
                case Role.Master:

                    break;

                case Role.Slave:

                    Parse(args.Packet);
                    break;

                case Role.Default:

                    break;

                default:

                    break;
            }   
        }

        /// <summary>
        /// Выполнить обработку пакета в пассивном режиме
        /// </summary>
        /// <param name="args">Аргументы события</param>
        private void ParsePassive(SerialEventArgs args)
        {
            byte[] packet = args.Packet.Com_Packet;
            if (packet != null)
            {
                byte ladd = packet[1];
                bool result = ((ladd & 0x80) > 0) ? true : false;

                if (result)
                {
                    ParsePassive(args.Packet);
                }               
            }
        }

        /// <summary>
        /// Получить срез данных на текущий момент времени
        /// </summary>
        /// <returns>Текущий срез данных</returns>
        public Float[] GetSlice()
        {   
            bool blocked = false;
            try
            {
                if (v_mutex.WaitOne(100, false))
                {
                    blocked = true;

                    Float[] slice = new Float[slave.Length];
                    slave.CopyTo(slice, 0);

                    return slice;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Stock -> GetSlice", ErrorType.Unknown));
                }
            }
            finally
            {
                if (blocked) v_mutex.ReleaseMutex();
            }

            return null;
        }
    }
}