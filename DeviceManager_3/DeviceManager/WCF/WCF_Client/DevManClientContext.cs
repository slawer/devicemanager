using System;
using System.Threading;
using System.Collections.Generic;

using WCF;

namespace WCF.WCF_Client
{
    /// <summary>
    /// Хранит текущие настройки пользователя DeviceManager
    /// </summary>
    public class DevManClientContext
    {
        private Handle handle = null;                       // дескриптор пользователя

        private Role role = Role.Common;                    // роль выполняемая системой
        private UserMode mode = UserMode.Active;            // режим работы с сервером

        private ReaderWriterLockSlim slim = null;           // синхронизатор
        
        /// <summary>
        /// Возникает когда данные необходимо синхронизовать с DeviceManager
        /// </summary>
        public event EventHandler onUpdate;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public DevManClientContext()
        {
            slim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        /// <summary>
        /// Определяет идентификатор пользователя
        /// </summary>
        public Handle Handle
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return handle;
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
                        handle = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Определяет роль которую выполняет пользователь системы
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
                return WCF.Role.Default;
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
        /// определяет режим в котором работает пользователь системы
        /// </summary>
        public UserMode Mode
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return mode;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }
                return UserMode.Default;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        mode = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Синхронизовать данные с DeviceManager
        /// </summary>
        internal void Update()
        {
            if (onUpdate != null)
            {
                onUpdate(this, EventArgs.Empty);
            }
        }
    }
}