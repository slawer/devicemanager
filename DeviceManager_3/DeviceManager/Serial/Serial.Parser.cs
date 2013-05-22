using System;

namespace DeviceManager
{
    /// <summary>
    /// Реализует обмен с устройствами через последовательный порт
    /// </summary>
    public partial class Serial
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

        /// <summary>
        /// Определяет неопределенное значение
        /// </summary>
        private const int invalid = -1;

        /// <summary>
        /// Определяет значение байта указывающего на начало пакета
        /// </summary>
        private const byte start_byte = 0x7e;

        /// <summary>
        /// Определяет минимально допустимую длинну пакета
        /// </summary>
        private const int packet_min_size = 0x07;

        /// <summary>
        /// Опредедляет максимальный размер пакета
        /// </summary>
        private const int packet_max_size = 0x24;

        // ----------- Смещения пакета ---------------

        /// <summary>
        /// Размер приращения при формированиии смещения
        /// </summary>
        private const int byte_size = 0x01;

        /// <summary>
        /// Смещение поля FL
        /// </summary>
        private const int fl = 0x00;

        /// <summary>
        /// Смещение поля LADD
        /// </summary>
        private const int ladd = fl + byte_size;

        /// <summary>
        /// Смещение поля l_pak
        /// </summary>
        private const int l_pak = ladd + byte_size;

        /// <summary>
        /// Смещение поля cmd
        /// </summary>
        private const int cmd = l_pak + byte_size;

        /// <summary>
        /// Смещение поля adr
        /// </summary>
        private const int adr = cmd + byte_size;

        /// <summary>
        /// Смещение поля ldata
        /// </summary>
        private const int ldata = adr + byte_size;

        /// <summary>
        /// Выполнить проверку данных на соответствие пакету в нотации ротокола Dsn
        /// </summary>
        /// <param name="response">Данные которые проверить</param>
        /// <param name="size">Размер передаваемого массива</param>
        /// <returns>В случае если данные являются корректным пакетом в нотации протокола Dsn функция вернет true, в противном случае false</returns>
        private int ParseDsn(byte[] response, int size)
        {
            if (response != null)
            {
                if (size < packet_min_size) return invalid;
                if (response[0] != start_byte) return invalid;
                if (size < (l_pak + 1)) return invalid;

                int size_of_packet = response[l_pak] + 1;
                if (size_of_packet >= packet_min_size && size_of_packet <= packet_max_size)
                {
                    if (size < size_of_packet) return invalid;
                    switch (app.TypeCRC)
                    {
                        case TypeCRC.Cycled:

                            if (CheckOneByteCRC(response, 0, size_of_packet))
                            {
                                return size_of_packet;
                            }
                            break;

                        case TypeCRC.CycledTwo:

                            if (CheckTwoByteCRC(response, 0, size_of_packet))
                            {
                                return size_of_packet;
                            }
                            break;

                        case TypeCRC.CRC16:

                            if (CheckCRC16ForDsn(response, 0, size_of_packet))
                            {
                                return size_of_packet;
                            }
                            break;
                    }
                }
            }
            return invalid;
        }

        /// <summary>
        /// Проверить CRC на корректность использую циклическую однобайтную
        /// </summary>
        /// <param name="buffer">Проверяемый пакет</param>
        /// <param name="offset">Смещение с которого осуществлять проверку</param>
        /// <param name="size">Размер пакета</param>
        /// <returns>Если CRC корректна, то функция вернет - true, в противном случае - false</returns>
        private bool CheckOneByteCRC(byte[] buffer, int offset, int size)
        {
            ushort total_crc = 0;
            for (int index = offset + 1; index < size - 1; index++)
            {
                total_crc += buffer[index];
            }
            byte t = (byte)total_crc;

            return (t == buffer[size - 1]);
        }

        /// <summary>
        /// Проверить CRC на корректность использую циклическую двухбайтную
        /// </summary>
        /// <param name="buffer">Проверяемый пакет</param>
        /// <param name="offset">Смещение с которого осуществлять проверку</param>
        /// <param name="size">Размер пакета</param>
        /// <returns>Если CRC корректна, то функция вернет - true, в противном случае - false</returns>
        private bool CheckTwoByteCRC(byte[] buffer, int offset, int size)
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

        /// <summary>
        /// /// Проверить CRC на корректность использую алгоритм CRC-16 при использовании протокола Dsn
        /// </summary>
        /// <param name="buffer">Проверяемый пакет</param>
        /// <param name="offset">Смещение с которого осуществлять проверку</param>
        /// <param name="size">Размер пакета</param>
        /// <returns>Если CRC корректна, то функция вернет - true, в противном случае - false</returns>
        private bool CheckCRC16ForDsn(byte[] buffer, int offset, int size)
        {
            int startIndex = offset + size - 1;

            ushort totalCRC = buffer[startIndex - 1];
            totalCRC <<= 8;
            totalCRC |= buffer[startIndex];

            ushort c = GetCRC16ForDsn(buffer, offset, size);
            return (c == totalCRC);
        }

        /// <summary>
        /// Получить CRC-16 при использовании протокола Dsn
        /// </summary>
        /// <param name="buffer">Пакет для вычисления CRC</param>
        /// <param name="offset">Смещение с которого следует вычислять CRC</param>
        /// <param name="size">Размер пакета</param>
        /// <returns>Вычисленное CRC</returns>
        private ushort GetCRC16ForDsn(byte[] buffer, int offset, int size)
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