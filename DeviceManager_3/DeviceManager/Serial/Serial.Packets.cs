using System;
using System.Threading;

namespace DeviceManager
{
    /// <summary>
    /// Реализует обмен с устройствами через последовательный порт
    /// </summary>
    public partial class Serial
    {
        protected Boolean is_primary_done;          // обработал или нет основной порт пакеты
        protected Boolean is_secondary_done;        // обработал или нет вспомогательный порт пакеты

        protected ReaderWriterLockSlim is_slim;     // синхронизатор

        /// <summary>
        /// Пересчитать CRC пакетов
        /// </summary>
        public void ReCalculateCRC(TypeCRC typeCRC)
        {
            bool blocked = false;
            try
            {
                if (s_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    foreach (Packet packet in static_list)
                    {
                        switch (typeCRC)
                        {
                            case TypeCRC.Cycled:

                                packet.Com_Packet[packet.Com_Packet.Length - 2] = 0x00;
                                ushort crcCyc = Application.GetCRC(packet.Com_Packet, typeCRC);

                                packet.Com_Packet[packet.Com_Packet.Length - 1] = (byte)crcCyc;
                                packet.Com_Packet[packet.Com_Packet.Length - 2] = 0x00;

                                break;

                            case TypeCRC.CycledTwo:

                                packet.Com_Packet[packet.Com_Packet.Length - 2] = 0x00;
                                ushort crc = Application.GetCRC(packet.Com_Packet, typeCRC);

                                packet.Com_Packet[packet.Com_Packet.Length - 1] = (byte)crc;

                                crc >>= 8;
                                packet.Com_Packet[packet.Com_Packet.Length - 2] = (byte)crc;
                                break;

                            case TypeCRC.CRC16:

                                packet.Com_Packet[packet.Com_Packet.Length - 2] = 0x00;
                                ushort crc16 = Application.GetCRC(packet.Com_Packet, typeCRC);

                                packet.Com_Packet[packet.Com_Packet.Length - 1] = (byte)crc16;

                                crc16 >>= 8;
                                packet.Com_Packet[packet.Com_Packet.Length - 2] = (byte)crc16;
                                break;

                            default:

                                break;

                        }
                    }
                }
            }
            finally
            {
                if (blocked) s_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Добавить пакет в статический список
        /// </summary>
        /// <param name="packet">Добавляемый пакет</param>
        public void InsertPacketToStatic(Packet packet)
        {
            bool blocked = false;
            try
            {
                if (s_mutex.WaitOne(100, false))
                {
                    blocked = true;

                    packet.Role = Role.Master;
                    packet.Content = Content.Sensor;

                    static_list.Add(packet);
                }
            }
            finally
            {
                if (blocked) s_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Очистить статический список
        /// </summary>
        public void ClearStatic()
        {
            bool blocked = false;
            try
            {
                if (s_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    static_list.Clear();
                }
            }
            finally
            {
                if (blocked) s_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Удалить пакет из статического списка
        /// </summary>
        /// <param name="packet">Удаляемый пакет</param>
        public void RemovePacketFromStatic(Packet packet)
        {
            bool blocked = false;
            try
            {
                if (s_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    static_list.Remove(packet);
                }
            }
            finally
            {
                if (blocked) s_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Возвращяет массив пакетов, находящихся в статическом списке
        /// </summary>
        public Packet[] StaticPackets
        {
            get
            {
                bool blocked = false;
                try
                {
                    if (s_mutex.WaitOne(100, false))
                    {
                        blocked = true;
                        return static_list.ToArray();
                    }
                    else
                        return null;
                }
                finally
                {
                    if (blocked) s_mutex.ReleaseMutex();
                }
            }
        }
    }
}