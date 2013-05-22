using System;
using System.Threading;
using System.Collections.Generic;

using System.Xml;

namespace DeviceManager
{
    /// <summary>
    /// Реализует формирование и отправку данных на блок отображения
    /// </summary>
    public partial class DisplayUnit
    {
        protected ReaderWriterLock locker;                      // синхронизирует доступ к параметрам класса
        protected List<DisplayPacket> packets;                  // пакеты для отправки в последовательный порт

        /// <summary>
        /// Возвращяет список пакетов для отправки на блоки отображения
        /// </summary>
        public DisplayPacket[] Packets
        {
            get
            {
                try
                {
                    locker.AcquireReaderLock(100);
                    try
                    {
                        return packets.ToArray();
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
                    }
                }
                catch { }
                return null;
            }
        }

        /// <summary>
        /// Добавить пакет
        /// </summary>
        /// <param name="packet">Добавляемый пакет</param>
        public void Insert(DisplayPacket packet)
        {
            try
            {
                locker.AcquireWriterLock(300);
                try
                {
                    packet.Place = repository.InstancePlace();
                    packets.Add(packet);
                }
                finally
                {
                    locker.ReleaseWriterLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Удалить пакет
        /// </summary>
        /// <param name="packet">Удаляемый пакет</param>
        public void Remove(DisplayPacket packet)
        {
            try
            {
                locker.AcquireWriterLock(300);
                try
                {
                    repository.RemovePlace(packet.Place);
                    packets.Remove(packet);
                }
                finally
                {
                    locker.ReleaseWriterLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Очистить список пакетов для отправки на БО
        /// </summary>
        public void Clear()
        {
            try
            {
                locker.AcquireWriterLock(300);
                try
                {
                    foreach (var packet in packets)
                    {
                        repository.RemovePlace(packet.Place);
                    }

                    packets.Clear();
                }
                finally
                {
                    locker.ReleaseWriterLock();
                }
            }
            catch { }
        }
    }
}