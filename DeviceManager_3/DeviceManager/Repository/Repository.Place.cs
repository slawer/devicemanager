using System;
using System.Threading;
using System.Collections.Generic;

namespace DeviceManager
{
    /// <summary>
    /// Реализует репозитарий.
    /// Осуществляеи хранение и передачу пакетов в рамках приложения,
    /// </summary>
    public partial class Repository
    {
    }

    /// <summary>
    /// Реализует место в репозитарии
    /// </summary>
    public class Place
    {
        protected List<Packet> packets = null;                  // пакеты

        protected ReaderWriterLockSlim mutex = null;            // синхронизатор
        protected PlaceState state = PlaceState.Default;        // состояние места в репозитарии

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Place()
        {            
            packets = new List<Packet>();
            mutex = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        /// <summary>
        /// Добавить пакет в репозитарий
        /// </summary>
        /// <param name="packet">Добавляемый пакет</param>
        public void Insert(Packet packet)
        {
            if (mutex.TryEnterWriteLock(500))
            {
                try
                {
                    packets.Add(packet);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    mutex.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Удалить пакет из репозитария
        /// </summary>
        /// <param name="packet">Удаляемый пакет</param>
        public void Remove(Packet packet)
        {
            if (mutex.TryEnterWriteLock(500))
            {
                try
                {
                    packets.Remove(packet);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    mutex.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Очистить место в репозитарии
        /// </summary>
        public void Clear()
        {
            if (mutex.TryEnterWriteLock(500))
            {
                try
                {
                    packets.Clear();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    mutex.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Пакеты, находящиеся в репозитарии
        /// </summary>
        public Packet[] Packets
        {
            get
            {
                if (mutex.TryEnterReadLock(300))
                {
                    try
                    {
                        return packets.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                    finally
                    {
                        mutex.ExitReadLock();
                    }
                }
                return null;
            }
        }
    }

    /// <summary>
    /// Определяет состояния места в репозитария
    /// </summary>
    public enum PlaceState
    {
        /// <summary>
        /// Данные заблокированны
        /// </summary>
        Blocked,

        /// <summary>
        /// Данные не заблокированны
        /// </summary>
        NotBlocked,

        /// <summary>
        /// Состояние по умолчанию
        /// </summary>
        Default
    }
}