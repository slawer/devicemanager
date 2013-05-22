using System;
using System.Threading;
using System.Collections.Generic;

namespace DeviceManager
{
    /// <summary>
    /// Реализует пакет для отправки на блок отображения
    /// </summary>
    public class DisplayPacket
    {
        protected ReaderWriterLockSlim p_locker;    // синхронизирует доступ к параметрам
        protected List<Parameter> parameters;       // параметры для отправки
                
        protected int device = 0;                   // номер устройства от имено которого делать пакет
        protected ReaderWriterLockSlim f_locker;    // синхронизирует доступ к параметрам пакета для блока отображения

        protected TimeSpan period;                  // частота отправки пакета в последовательный порт
        protected DateTime lastTime;                // время последней отправки в последовательный порт

        protected Place place = null;               // место в репозитарии
        protected string description;               // описание пакета   

        protected bool actived = true;              // активна команда или нет
        protected bool toPort = true;               // отправлять пакет в порт или нет

        protected TypePort port_type;               // порт в который отправлять данный пакет

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public DisplayPacket()
        {
            p_locker = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            f_locker = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            parameters = new List<Parameter>();
            port_type = DeviceManager.TypePort.Default;
        }

        /// <summary>
        /// Определяет параметры для отправки
        /// </summary>
        public Parameter[] Parameters
        {
            get
            {
                if (p_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return parameters.ToArray();
                    }
                    finally
                    {
                        p_locker.ExitReadLock();
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Добавить параметр в список
        /// </summary>
        /// <param name="parameter">Добавляемый параметр</param>
        public void Insert(Parameter parameter)
        {
            if (p_locker.TryEnterWriteLock(300))
            {
                try
                {
                    parameters.Add(parameter);
                }
                finally
                {
                    p_locker.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Удалить параметр в список
        /// </summary>
        /// <param name="parameter">Удаляемый параметр</param>
        public void Remove(Parameter parameter)
        {
            if (p_locker.TryEnterWriteLock(300))
            {
                try
                {
                    parameters.Remove(parameter);
                }
                finally
                {
                    p_locker.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Очистить список параметров для отправки на БО
        /// </summary>
        public void Clear()
        {
            if (p_locker.TryEnterWriteLock(300))
            {
                try
                {
                    parameters.Clear();
                }
                finally
                {
                    p_locker.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Определяет номер устройства от имено которого делать пакет
        /// </summary>
        public int Device
        {
            get
            {
                if (f_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return device;
                    }
                    finally
                    {
                        f_locker.ExitReadLock();
                    }
                }

                return -1;
            }

            set
            {
                if (f_locker.TryEnterWriteLock(300))
                {
                    try
                    {
                        device = value;
                    }
                    finally
                    {
                        f_locker.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет частоту отправки пакета в последовательный порт
        /// </summary>
        public TimeSpan Period
        {
            get
            {
                if (f_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return period;
                    }
                    finally
                    {
                        f_locker.ExitReadLock();
                    }
                }

                return TimeSpan.MinValue;
            }

            set
            {
                if (f_locker.TryEnterWriteLock(300))
                {
                    try
                    {
                        period = value;
                    }
                    finally
                    {
                        f_locker.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет время последней отправки пакета в последовательный порт
        /// </summary>
        public DateTime LastTime
        {
            get
            {
                if (f_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return lastTime;
                    }
                    finally
                    {
                        f_locker.ExitReadLock();
                    }
                }

                return DateTime.MinValue;
            }

            set
            {
                if (f_locker.TryEnterWriteLock(300))
                {
                    try
                    {
                        lastTime = value;
                    }
                    finally
                    {
                        f_locker.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет описание параметра
        /// </summary>
        public string Description
        {
            get 
            {
                if (f_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return description;
                    }
                    finally
                    {
                        f_locker.ExitReadLock();
                    }
                }

                return string.Empty;
            }

            set 
            {
                if (f_locker.TryEnterWriteLock(300))
                {
                    try
                    {
                        description = value;
                    }
                    finally
                    {
                        f_locker.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Место в репозитарии
        /// </summary>
        public Place Place
        {
            get 
            {
                if (f_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return place;
                    }
                    finally
                    {
                        f_locker.ExitReadLock();
                    }
                }

                return null;            
            }

            set 
            {
                if (f_locker.TryEnterWriteLock(300))
                {
                    try
                    {
                        place = value;
                    }
                    finally
                    {
                        f_locker.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет jnghfdkznm gfrtn d gjhn bkb ytn
        /// </summary>
        public bool ToPort
        {
            get
            {
                if (f_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return toPort;
                    }
                    finally
                    {
                        f_locker.ExitReadLock();
                    }
                }

                return false;
            }

            set
            {
                if (f_locker.TryEnterWriteLock(300))
                {
                    try
                    {
                        toPort = value;
                    }
                    finally
                    {
                        f_locker.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет обрабатывать пакет или нет
        /// </summary>
        public bool IsActived
        {
            get
            {
                if (f_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return actived;
                    }
                    finally
                    {
                        f_locker.ExitReadLock();
                    }
                }

                return false;
            }

            set
            {
                if (f_locker.TryEnterWriteLock(300))
                {
                    try
                    {
                        actived = value;
                    }
                    finally
                    {
                        f_locker.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет в какой порт отправлять команду
        /// </summary>
        public TypePort TypePort
        {
            get
            {
                if (f_locker.TryEnterReadLock(100))
                {
                    try
                    {
                        return port_type;
                    }
                    finally
                    {
                        f_locker.ExitReadLock();
                    }
                }

                return DeviceManager.TypePort.Default;
            }

            set
            {
                if (f_locker.TryEnterWriteLock(300))
                {
                    try
                    {
                        port_type = value;
                    }
                    finally
                    {
                        f_locker.ExitWriteLock();
                    }
                }
            }
        }
    }
}