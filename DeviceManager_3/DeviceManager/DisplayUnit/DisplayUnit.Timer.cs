using System;
using System.Threading;

using DeviceManager.DevMan;
using DeviceManager.Errors;

using SoftwareDevelopmentKit.Protocols.Dsn;

namespace DeviceManager
{
    /// <summary>
    /// Реализует формирование и отправку данных на блок отображения
    /// </summary>
    public partial class DisplayUnit
    {
        protected IProtocol protocol = null;                    // реализует работу с протоколом обмена Dsn

        /// <summary>
        /// Инициирует процедуру отправки пакетов на блоки отображения
        /// </summary>
        /// <param name="state">Не используется</param>
        private void TimerCallback(object state)
        {
            bool blocked = false;
            try
            {
                if (t_mutex.WaitOne(0, false))
                {
                    blocked = true;
                    if (Interlocked.Read(ref counter) > 0)
                    {
                        Interlocked.Exchange(ref counter, 0);
                        DateTime now = DateTime.Now;              // текущее время

                        // ...

                        if (app != null)
                        {
                            Float[] result = app.Converter.GetResults();
                            if (result != null)
                            {
                                DisplayPacket[] packs = Packets;
                                foreach (var packet in packs)
                                {
                                    TimeSpan elapsed = now - packet.LastTime;
                                    if (elapsed >= packet.Period)
                                    {
                                        packet.LastTime = now;
                                        byte[] com = GetComPacket(packet, result);

                                        if (com != null)
                                        {
                                            Packet pack = new Packet();
                                            
                                            pack.ToPort = packet.ToPort;
                                            pack.IsActived = packet.IsActived;

                                            pack.Role = Role.Slave;
                                            pack.Content = Content.Unknown;

                                            pack.Wait = false;
                                            pack.Com_Packet = com;

                                            pack.PortType = packet.TypePort;

                                            packet.Place.Clear();
                                            packet.Place.Insert(pack);
                                        }
                                    }
                                }

                                if (OnComplete != null)
                                {
                                    OnComplete(this, null);
                                }
                            }
                        }
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "DisplayUnit ->TimerCallback", ErrorType.NotFatal));
                }
            }
            finally
            {
                if (blocked) t_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Создать пакет для отправки в последовательный порт
        /// </summary>
        /// <param name="packet">Пакет из которого сделать пакет для блока отображения</param>
        /// <param name="result">Данные конвертера</param>
        /// <returns>Пакет для отправки на блок отображения</returns>
        private byte[] GetComPacket(DisplayPacket packet, Float[] result)
        {
            Parameter[] parames = packet.Parameters;
            if (parames != null)
            {
                byte[] data = GetData(parames, result);
                if (data != null)
                {
                    int size = 8 + data.Length;
                    byte[] com_packet = new byte[size];

                    com_packet[0] = 0x7e;
                    com_packet[1] = (byte)(0x80 | packet.Device);

                    com_packet[2] = (byte)(7 + data.Length);
                    com_packet[3] = 0x02;
                    com_packet[4] = 0x00;

                    com_packet[5] = (byte)data.Length;

                    Array.Copy(data, 0, com_packet, 6, data.Length);
                    SetCRC(com_packet);

                    return com_packet;
                }
            }

            return null;
        }

        /// <summary>
        /// Получить данный, которые содержаться в пакете
        /// </summary>
        /// <param name="parames">Параметры пакета</param>
        /// <param name="result">Данные конвертера</param>
        /// <returns>Массив данных пакета или null если данные не удалось извлечь</returns>
        private byte[] GetData(Parameter[] parames, Float[] result)
        {
            int offset = 0;
            byte[] data = new byte[256];

            try
            {
                foreach (var parameter in parames)
                {
                    byte[] tmp = null;
                    float current = GetValue(parameter.Position, result);

                    if (!float.IsNaN(current))
                    {
                        tmp = BitConverter.GetBytes((int)Math.Round(current));
                    }
                    else
                        tmp = new byte[4];

                    Array.Resize(ref tmp, parameter.Size);
                    if (parameter.IsLittleEndian) Array.Reverse(tmp);

                    Array.Copy(tmp, 0, data, offset, parameter.Size);
                    offset = offset + parameter.Size;
                }

                Array.Resize(ref data, offset);
                return data;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Установить CRC для пакета
        /// </summary>
        /// <param name="com_packet">Пакет</param>
        private void SetCRC(byte[] com_packet)
        {
            ushort crc = 0x0000;
            switch (app.TypeCRC)
            {
                case TypeCRC.Cycled:

                    crc = PacketTranslater.CalculateOneByteCRC(com_packet);
                    break;

                case TypeCRC.CycledTwo:

                    crc = PacketTranslater.CalculateTwoByteCRC(com_packet);
                    break;

                case TypeCRC.CRC16:

                    crc = PacketTranslater.CalculateTwoByteCRC16(com_packet);
                    break;
            }

            byte[] t_crc = BitConverter.GetBytes(crc);
            if (BitConverter.IsLittleEndian) Array.Reverse(t_crc);

            if (app.TypeCRC == TypeCRC.Cycled)
            {
                com_packet[com_packet.Length - 2] = 0x00;
            }
            else
                com_packet[com_packet.Length - 2] = t_crc[0];

            com_packet[com_packet.Length - 1] = t_crc[1];
        }

        /// <summary>
        /// Получить значени из списка
        /// </summary>
        /// <param name="index">Позиция значения в списке</param>
        /// <param name="values">Список значений</param>
        /// <returns>Значение из списка</returns>
        private float GetValue(int index, Float[] values)
        {
            if (index > -1 && index < values.Length)
            {
                return values[index].Value;
                //return values[index].GetCurrentValue();
            }
            else
                return float.NaN;
        }
    }
}