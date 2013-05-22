using System;
using System.Threading;

namespace DeviceManager
{
    /// <summary>
    /// Реализует пакет обмена данными в нотации DSN
    /// </summary>
    public class Packet
    {
        private ReaderWriterLockSlim slim;  // синхронизатор
        
        private byte[] com_packet;          // пакет для отправки в порт
        private string tcp_packet;          // пакет для отправки по Tcp

        private long actived;               // выполнять команду или нет
        private long toPort;                // отправлять команду в порт или нет

        private long wait = 1;              // ожидать ответ при отправке в порт или нет

        private Role role;                  // определяет роль пакета в обмене
        private Content contentType;        // определяет содержимое пакета

        private PacketSource source;        // откуда пакет
        private TypePort port_type;         // в какой порт отправлять данный пакет

        private DateTime lastTime;          // время отправки пакета в порт
        private TimeSpan interval;          // частота отправки пакета в порт

        private bool sended = false;        // пакет был отправлен в порт или нет

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Packet()
        {
            slim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            contentType = Content.Unknown;

            actived = 1;
            toPort = 1;

            port_type = TypePort.Default;

            lastTime = DateTime.MinValue;
            interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        /// <summary>
        /// Определяет пакет для отпраки в порт
        /// </summary>
        public byte[] Com_Packet
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return com_packet;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return null;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        com_packet = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет в какой порт отправлять пакет
        /// </summary>
        public TypePort PortType
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return port_type;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return TypePort.Default;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        port_type = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет время отправки пакета в порт
        /// </summary>
        public DateTime LastTime
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return lastTime;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return DateTime.MinValue;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        lastTime = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет желаемы интевал времени по 
        /// истечении которого нужно выполнить отправку пакета в порт
        /// </summary>
        public TimeSpan Interval
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return interval;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return TimeSpan.MaxValue;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        interval = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет пакет для отправки по Tcp
        /// </summary>
        public string Tcp_Packet
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return tcp_packet;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return String.Empty;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        tcp_packet = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Указывает на необходимость ожидать ответ при отправки пакета в порт или нет
        /// </summary>
        public bool Wait
        {
            get
            {
                return (Interlocked.Read(ref wait) == 1);
            }

            set
            {
                if (value)
                {
                    Interlocked.Exchange(ref wait, 1);
                }
                else
                    Interlocked.Exchange(ref wait, 0);
            }
        }

        /// <summary>
        /// Определяет выполнять команду или нет
        /// </summary>
        public bool IsActived
        {
            get
            {
                return (Interlocked.Read(ref actived) == 1);
            }

            set
            {
                if (value)
                {
                    Interlocked.Exchange(ref actived, 1);
                }
                else
                    Interlocked.Exchange(ref actived, 0);
            }
        }


        /// <summary>
        /// Определяет выполнять команду или нет
        /// </summary>
        public bool ToPort
        {
            get
            {
                return (Interlocked.Read(ref toPort) == 1);
            }

            set
            {
                if (value)
                {
                    Interlocked.Exchange(ref toPort, 1);
                }
                else
                    Interlocked.Exchange(ref toPort, 0);
            }
        }

        /// <summary>
        /// Определяет тип значений которые содержит пакет
        /// </summary>
        public Content Content
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return contentType;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return DeviceManager.Content.Default;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        contentType = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет роль данного пакета (вопрос или ответ)
        /// </summary>
        public Role Role
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return role;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return DeviceManager.Role.Default;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        role = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет источник пакета
        /// </summary>
        public PacketSource Source
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return source;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return PacketSource.Default;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        source = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет пакет был отправлен в порт или нет
        /// </summary>
        public bool Sended
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return sended;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return false;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        sended = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }
    }
}