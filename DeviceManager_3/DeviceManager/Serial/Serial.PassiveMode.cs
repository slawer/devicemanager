using System;
using System.Threading;
using System.Collections.Generic;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Serial
    {        
        private Translater translater = null;       // выполняет извлечение пакетов из потока байт
        private CalculaterCRC calculator = null;    // осуществляет обработку CRC пакета

        private Mutex passive_mutex = null;         // синхронизует доступ к буферам

        private List<byte> input;                   // входной (накапливающий) буфер
        private List<byte> output;                  // выходной (анализируемый) буфер

        /// <summary>
        /// Осуществляет разбор пришедших байтов
        /// </summary>
        /// <param name="data">Данные из последовательного порта</param>
        private void PassiveWork(byte[] data)
        {
            bool blocked = false;
            try
            {
                input.AddRange(data);
                if (passive_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    output.AddRange(input);
                    input.Clear();

                    InitializeCalculator();
                    translater.BeginInvoke(null, null);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Serial -> PassiveWork", ErrorType.NotFatal));
                }
            }
            finally
            {
                if (blocked) passive_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Осуществляет разбор байтов на осмысленные пакеты
        /// </summary>
        private void TranslaterFunction()
        {
            bool blocked = false;
            try
            {
                if (passive_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    while (output.Count > 0)
                    {
                        byte item = output[0];
                        switch (item)
                        {
                            case start_byte:

                                if (output.Count < (l_pak + 1)) return;
                                int size_of_packet = output[l_pak] + 1;

                                if (size_of_packet >= packet_min_size && size_of_packet <= packet_max_size)
                                {
                                    if (output.Count < size_of_packet) return;
                                    if (calculator.Calculate(0, size_of_packet, output.ToArray()))
                                    {
                                        byte[] packet = new byte[size_of_packet];
                                        
                                        output.CopyTo(0, packet, 0, size_of_packet);
                                        output.RemoveRange(0, size_of_packet);

                                        Packet pack = new Packet();

                                        pack.Role = Role.Default;
                                        pack.Content = Content.Default;

                                        pack.Source = PacketSource.Default;

                                        pack.Com_Packet = packet;                                        
                                        if (OnPacket != null)
                                        {
                                            OnPacket(this, new SerialEventArgs(pack));
                                        }
                                    }
                                    else
                                    {
                                        output.RemoveAt(0);
                                        Interlocked.Increment(ref c_lost_bytes);
                                    }
                                }
                                else
                                {
                                    output.RemoveAt(0);
                                    Interlocked.Increment(ref c_lost_bytes);
                                }
                                break;

                            default:

                                output.RemoveAt(0);
                                Interlocked.Increment(ref c_lost_bytes);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Serial -> TranslaterFunction", ErrorType.NotFatal));
                }
            }
            finally
            {
                if (blocked) passive_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// инициализировать считальщика CRC
        /// </summary>
        private void InitializeCalculator()
        {
            switch (app.TypeCRC)
            {
                case TypeCRC.Cycled:

                    if (calculator is CalculatorOneByte)
                    {
                        return;
                    }
                    else
                        calculator = new CalculatorOneByte();

                    break;

                case TypeCRC.CycledTwo:

                    if (calculator is CalculateTwoByte)
                    {
                        return;
                    }
                    else
                        calculator = new CalculateTwoByte();

                    break;

                case TypeCRC.CRC16:

                    if (calculator is CalculateCRC16)
                    {
                        return;
                    }
                    else
                        calculator = new CalculateCRC16();

                    break;
            }
        }

        /// <summary>
        /// определяет указатель на функцию
        /// </summary>
        private delegate void Translater();

        class CalculaterCRC
        {
            public TypeCRC typeCRC;
            public virtual bool Calculate(int offset, int size, byte[] buffer) { return false; }

            public CalculaterCRC()
                : base()
            {
                typeCRC = TypeCRC.Cycled;
            }
        }

        class CalculatorOneByte : CalculaterCRC
        {
            public CalculatorOneByte()
                : base()
            {
                typeCRC = TypeCRC.Cycled;
            }

            public override bool Calculate(int offset, int size, byte[] buffer)
            {
                ushort total_crc = 0x00;
                for (int index = offset + 1; index < size - 1; index++)
                {
                    total_crc += buffer[index];
                }
                byte t = (byte)total_crc;

                return (t == buffer[size - 1]);
            }
        }

        class CalculateTwoByte : CalculaterCRC
        {
            public CalculateTwoByte()
                : base()
            {
                typeCRC = TypeCRC.CycledTwo;
            }

            public override bool Calculate(int offset, int size, byte[] buffer)
            {
                uint total_crc = 0x00;
                for (int index = offset + 1; index < size - 2; index++)
                {
                    total_crc += buffer[index];
                }

                ushort short_crc = (ushort)total_crc;

                byte part_1 = buffer[size - 2];
                byte part_2 = buffer[size - 1];

                ushort crc = part_1;

                crc <<= 8;
                crc |= part_2;

                return (crc == short_crc);
            }
        }

        class CalculateCRC16 : CalculaterCRC
        {
            ushort[] Crc16Table = 
        {
            0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
            0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
            0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
            0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
            0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,
            0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
            0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
            0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
            0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
            0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
            0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
            0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
            0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
            0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
            0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
            0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
            0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
            0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
            0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
            0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
            0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
            0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
            0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
            0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
            0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
            0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
            0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
            0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
            0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
            0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
            0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
            0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040 
        };

            public CalculateCRC16()
                : base()
            {
                typeCRC = TypeCRC.CRC16;
            }

            // ----- Проверка корректности CRC пакета ------------

            public override bool Calculate(int offset, int size, byte[] buffer)
            {
                int startIndex = offset + size - 1;

                ushort totalCRC = buffer[startIndex - 1];
                totalCRC <<= 8;
                totalCRC |= buffer[startIndex];

                ushort c = GetCRC16(offset, size, buffer);

                return (c == totalCRC);
            }

            // ----- Вычисление CRC ------

            private ushort GetCRC16(int offset, int size, byte[] buffer)
            {
                ushort crc = 0xffff;
                for (int i = offset + 1; i < size - 2; i++)
                {
                    crc = (ushort)((crc >> 8) ^ Crc16Table[(crc & 0xff) ^ buffer[i]]);
                }
                return crc;
            }
        }
    }
}