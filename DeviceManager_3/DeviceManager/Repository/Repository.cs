using System;
using System.Threading;
using System.Collections.Generic;


namespace DeviceManager
{
    /// <summary>
    /// Реализует репозитарий.
    /// Осуществляеи хранение и передачу пакетов в рамках приложения
    /// </summary>
    public partial class Repository
    {
        protected ReaderWriterLockSlim mutex = null;        // синхронизатор

        protected List<Packet> packets;                     // буфер хранения, промежуточные пакеты, извлекаемые из репозитария
        protected List<Place> places = null;                // место в репозитарии

        /// <summary>
        /// Возникает когда происходит ошибка
        /// </summary>
        public event Errors.ApplicationErrorHandler onError;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Repository()
        {
            mutex = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            places = new List<Place>();
            packets = new List<Packet>();
        }

        /// <summary>
        /// Получить место в репозитарии
        /// </summary>
        public Place InstancePlace()
        {
            if (mutex.TryEnterWriteLock(500))
            {
                try
                {
                    Place place = new Place();
                    places.Add(place);

                    return place;
                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        string message = "(репозитарий InstancePlace) + " + ex.Message;
                        onError(this, new Errors.ErrorArgs(message, Errors.ErrorType.NotFatal));
                    }
                }
                finally
                {
                    mutex.ExitWriteLock();
                }
            }
            return null;
        }

        /// <summary>
        /// Освободить место в репозитарии
        /// </summary>
        /// <param name="place">Освобождаемое место</param>
        public void RemovePlace(Place place)
        {
            if (mutex.TryEnterWriteLock(500))
            {
                try
                {
                    places.Remove(place);
                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        string message = "(репозитарий RemovePlace) + " + ex.Message;
                        onError(this, new Errors.ErrorArgs(message, Errors.ErrorType.NotFatal));
                    }
                }
                finally
                {
                    mutex.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Очистить репозитарий
        /// </summary>
        public void Clear()
        {
            if (mutex.TryEnterReadLock(300))
            {
                try
                {
                    foreach (var place in places)
                    {
                        place.Clear();
                    }
                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        string message = "(репозитарий Clear) + " + ex.Message;
                        onError(this, new Errors.ErrorArgs(message, Errors.ErrorType.NotFatal));
                    }
                }
                finally
                {
                    mutex.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Получить все пакеты, находящиеся в репозитарии
        /// </summary>
        public Packet[] Packets
        {
            get
            {
                if (mutex.TryEnterReadLock(300))
                {
                    try
                    {
                        packets.Clear();
                        foreach (var place in places)
                        {

                            packets.AddRange(place.Packets);
                            place.Clear();
                        }

                        return packets.ToArray();
                    }
                    catch (Exception ex)
                    {
                        if (onError != null)
                        {
                            string message = "(репозитарий Packets) + " + ex.Message;
                            onError(this, new Errors.ErrorArgs(message, Errors.ErrorType.NotFatal));
                        }
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
}