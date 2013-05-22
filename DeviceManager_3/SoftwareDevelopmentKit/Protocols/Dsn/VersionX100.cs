using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SoftwareDevelopmentKit.Protocols.Dsn
{
    /// <summary>
    /// Реализует работу с протокол первой версии
    /// </summary>
    public class VersionX100 : IProtocol
    {
        private static Regex regex = null;
        private static VersionX100 protocol = null;        

        /// <summary>
        /// Инициализтрует новый экземпляр класса
        /// </summary>
        protected VersionX100()
        {
            regex = new Regex("@%[0-9a-fA-F]*[\\$]");
        }

        /// <summary>
        /// Инстанцировать класс
        /// </summary>
        /// <returns></returns>
        public static IProtocol CreateProtocol()
        {
            if (protocol == null)
            {
                protocol = new VersionX100();
            }
            return protocol as IProtocol;
        }

        // ------ реализация интерфейса -------

        /// <summary>
        /// Получить линейный адресс из пакета
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Линейный адрес</returns>
        public string GetLadd(string packet)
        {            
            if (regex.IsMatch(packet))
            {
                foreach (Match match in regex.Matches(packet))
                {
                    return match.Value.Substring(2, 2);
                }
                return string.Empty;
            }
            else
                throw new ArgumentException("Не правильный формат пакета");            
        }

        /// <summary>
        /// Выделить поле L_PACK из пакета
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Поле L_PACK</returns>
        public string GetL_Pack(string packet)
        {
            if (regex.IsMatch(packet))
            {
                foreach (Match match in regex.Matches(packet))
                {
                    return match.Value.Substring(4, 2);
                }
                return string.Empty;
            }
            else
                throw new ArgumentException("Не правильный формат пакета");
        }

        /// <summary>
        /// Выделить поле CMD
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Поле CMD</returns>
        public string GetCmd(string packet)
        {
            if (regex.IsMatch(packet))
            {
                foreach (Match match in regex.Matches(packet))
                {
                    return match.Value.Substring(6, 2);
                }
                return string.Empty;
            }
            else
                throw new ArgumentException("Не правильный формат пакета");
        }

        /// <summary>
        /// Выделить поле ADR
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Поле ADR</returns>
        public string GetAdr(string packet)
        {
            if (regex.IsMatch(packet))
            {
                foreach (Match match in regex.Matches(packet))
                {
                    return match.Value.Substring(8, 2);
                }
                return string.Empty;
            }
            else
                throw new ArgumentException("Не правильный формат пакета");
        }

        /// <summary>
        /// Выделить поле LDATA
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Поле LDATA</returns>
        public string GetLData(string packet)
        {
            if (regex.IsMatch(packet))
            {
                foreach (Match match in regex.Matches(packet))
                {
                    return match.Value.Substring(10, 2);
                }
                return string.Empty;
            }
            else
                throw new ArgumentException("Не правильный формат пакета");
        }

        /// <summary>
        /// Выделить данные из пакета
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Данные, содержащиеся в пакете</returns>
        public string GetData(string packet)
        {
            if (regex.IsMatch(packet))
            {
                string pack = string.Empty;
                foreach (Match match in regex.Matches(packet))
                {
                    pack = match.Value;
                    break;
                }

                int lpack = int.Parse(GetL_Pack(packet), NumberStyles.AllowHexSpecifier);
                int ldata = int.Parse(GetLData(packet), NumberStyles.AllowHexSpecifier);

                if (lpack == (ldata + 7))
                {
                    return pack.Substring(12, ldata * 2);
                }
                return string.Empty;
            }
            else
                throw new ArgumentException("Не правильный формат пакета");
        }

        /// <summary>
        /// Выделить поле статуса
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Статусный байт</returns>
        public string GetStatus(string packet)
        {
            if (regex.IsMatch(packet))
            {
                foreach (Match match in regex.Matches(packet))
                {
                    return match.Value.Substring(match.Value.Length - 3, 2);
                }
                return string.Empty;
            }
            else
                throw new ArgumentException("Не правильный формат пакета");
        }

        /// <summary>
        /// Определяет данный пакет для отправки устройству
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>true - если пакет предназначается устройству</returns>
        public bool IsToDevice(string packet)
        {
            byte b = (byte)int.Parse(GetLadd(packet), NumberStyles.AllowHexSpecifier);

            int lb = (byte)(b & 0x80);
            return (lb == 0);
        }

        /// <summary>
        /// Определяет данный пакет поступил от устройства
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>true - если пакет пришел от устройства</returns>
        public bool IsFromDevice(string packet)
        {
            return !IsToDevice(packet);
        }

        /// <summary>
        /// Получить номер устройства из пакета
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Номер устройства</returns>
        public int GetNumberDevice(string packet)
        {
            if (IsFromDevice(packet))
            {
                int number = int.Parse(packet.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                return (number - 128);
            }
            else
                return -1;
        }

        /// <summary>
        /// Определяет данный пакет содержит команду на чтение с устройства
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>true - да пакет содержит команду на чтение</returns>
        public bool IsRead(string packet)
        {
            string cmd = GetCmd(packet);
            byte _cp = (byte)int.Parse(cmd, NumberStyles.AllowHexSpecifier);
            return !((_cp & 2) == 0);
        }

        /// <summary>
        /// Определяет данный пакет содержит команду на запись в устройство
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>true - да пакет содержит команду на запись</returns>
        public bool IsWrite(string packet)
        {
            string cmd = GetCmd(packet);
            byte _cp = (byte)int.Parse(cmd, NumberStyles.AllowHexSpecifier);
            return !((_cp & 1) == 0);
        }

        /// <summary>
        /// Получить номер страницы
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Номер страницы</returns>
        public int PageAdress(string packet)
        {
            byte cmcd = (byte)(int.Parse(GetCmd(packet), NumberStyles.AllowHexSpecifier));
            cmcd = (byte)(cmcd & 0xE0);
            byte number = (byte)(cmcd >> 5);
            return number;
        }

        /// <summary>
        /// Формирует команду
        /// </summary>
        /// <param name="Number">Номер устройства</param>
        /// <param name="Cmd">Комадна на чтени, запись, чтени/запись</param>
        /// <param name="pNumber">Номер страницы</param>
        /// <param name="Offset">Смещенеи на странице</param>
        /// <param name="lenghtOfData">Длинна данных</param>
        /// <param name="Data">Данные</param>
        /// <returns>Сформированная команда</returns>
        public string CreateCommand(Device Number, Command Cmd, PageNumber pNumber, int Offset, 
            int lenghtOfData, string Data)
        {
            if ((Offset + lenghtOfData) > 256)
            {
                throw new ArgumentException("Invalid Offset value and lenghtOfData value. The summa must less than 255.");
            }

            if (Data != null)
            {
                if (Data != string.Empty)
                {
                    if ((Data.Length / 2) != lenghtOfData)
                    {
                        throw new ArgumentException("Data.Lenght != lenghtOfData");
                    }
                }
            }

            string ladd = GetDeviceNumber(Number);
            
            int l_pack = 7;
            if (Data != null && Data != string.Empty) l_pack += Data.Length / 2;

            byte _cmd = 0x00;
            switch (pNumber)
            {
                case PageNumber.P0: _cmd = 0x00; break;
                case PageNumber.P1: _cmd = 0x20; break;
                case PageNumber.P2: _cmd = 0x40; break;
                case PageNumber.P3: _cmd = 0x60; break;
                case PageNumber.P4: _cmd = 0x80; break;
                case PageNumber.P5: _cmd = 0xA0; break;
                case PageNumber.P6: _cmd = 0xC0; break;
                case PageNumber.P7: _cmd = 0xE0; break;
            }

            switch (Cmd)
            {
                case Command.Read: _cmd |= 0x02; break;
                case Command.Write: _cmd |= 0x01; break;
                case Command.ReadWrite: _cmd |= 0x03; break;
            }
            
            string adr = string.Format("{0:X2}", Offset);
            string ldata = string.Format("{0:X2}", lenghtOfData);

            string status = "00";


            string total = ladd + string.Format("{0:X2}", l_pack) + string.Format("{0:X2}", _cmd) +
                adr + ldata;

            if (Data != null && Data != string.Empty) total += Data;
            total += status + "$";
            return "@JOB#000#" + total;
        }

        /// <summary>
        /// Формирует команду
        /// </summary>
        /// <param name="Number">Номер устройства</param>
        /// <param name="Cmd">Комадна на чтени, запись, чтени/запись</param>
        /// <param name="pNumber">Номер страницы</param>
        /// <param name="Offset">Смещенеи на странице</param>
        /// <param name="lenghtOfData">Длинна данных</param>
        /// <param name="Data">Данные</param>
        /// <returns>Сформированная команда</returns>
        public string CreateCommand(int Number, Command Cmd, int pNumber, int Offset,
                                        int lenghtOfData, string Data)
        {
            if ((Offset + lenghtOfData) > 256)              // проверяем на корректность смещение и длинну данных обмена
            {
                // сумма смещени + длинна данных обмена не корректны
                throw new ArgumentException("Invalid Offset value and lenghtOfData value. The summa must less than 255.");  // генерируем исключение
            }

            if (Data != null)       // проверяем наличие данных
            {
                // имеются данные
                if (Data != string.Empty)   // проверяем наличие данных
                {
                    // имеются данные
                    if ((Data.Length / 2) != lenghtOfData)  // проверяем корректность длинны данных
                    {
                        // если не корректна
                        throw new ArgumentException("Data.Lenght != lenghtOfData"); // генерируем исключение
                    }
                }
            }

            string ladd = String.Format("{0:X2}", Number);                      // начинаем формировать линейный адрес

            int l_pack = 7;                                                     // устанавливаем значение поля L_PAK длинна пакета
            if (Data != null && Data != string.Empty) l_pack += Data.Length / 2;    // если имеются данные, которые необходимо включить в пакет, то корректируем длинну пакета

            byte _cmd = 0x00;                       // инициализируем поле команды
            switch (pNumber)                        // определяем номер страницы
            {
                case 0: _cmd = 0x00; break;         // нулевая страница
                case 1: _cmd = 0x20; break;         // первая страница
                case 2: _cmd = 0x40; break;         // вторая страница
                case 3: _cmd = 0x60; break;         // третья страница
                case 4: _cmd = 0x80; break;         // четвертая страница
                case 5: _cmd = 0xA0; break;         // пятая страница
                case 6: _cmd = 0xC0; break;         // шестая страница
                case 7: _cmd = 0xE0; break;         // седьмая страница
            }

            switch (Cmd)                                    // определяем тип обращения
            {
                case Command.Read: _cmd |= 0x02; break;             // на чтение
                case Command.Write: _cmd |= 0x01; break;            // на запись
                case Command.ReadWrite: _cmd |= 0x03; break;        // на чтение и запись
            }

            string adr = string.Format("{0:X2}", Offset);               // присваиваем значение полю адрес(указываем смещение по которому необходимо обратиться)
            string ldata = string.Format("{0:X2}", lenghtOfData);       // пакеум поле длинны данных обмена

            string status = "00";                           // инициализируем поле статуса


            string total = ladd + string.Format("{0:X2}", l_pack) + string.Format("{0:X2}", _cmd) +     // пакуем пакет
                adr + ldata;

            if (Data != null && Data != string.Empty) total += Data;        // если имеются данные , то привоединяем их к пакету
            total += status + "$";                                          // присоединяем статус к пакету
            return "@JOB#000#" + total;                                     // корректируем и возвращяем пакет
        }
        // ----- получить номер устройства ----

        private string GetDeviceNumber(Device device)
        {
            switch (device)
            {
                case Device.D1: return "01";
                case Device.D2: return "02";
                case Device.D3: return "03";
                case Device.D4: return "04";
                case Device.D5: return "05";
                case Device.D6: return "06";
                case Device.D7: return "07";
                case Device.D8: return "08";
                case Device.D9: return "09";
                case Device.D10: return "0A";
                case Device.D11: return "0B";
                case Device.D12: return "0C";
                case Device.D13: return "0D";
                case Device.D14: return "0E";
                case Device.D15: return "0F";
                case Device.D16: return "10";
                case Device.D17: return "11";
                case Device.D18: return "12";
                case Device.D19: return "13";
                case Device.D20: return "14";
                case Device.D21: return "15";
                case Device.D22: return "16";
                case Device.D23: return "17";
                case Device.D24: return "18";
                case Device.D25: return "19";
                case Device.D26: return "1A";
                case Device.D27: return "1B";
                case Device.D28: return "1C";
                case Device.D29: return "1D";
                case Device.D30: return "1E";
                case Device.D31: return "1F";
                case Device.D32: return "20";
                case Device.D33: return "21";
                case Device.D34: return "22";
                case Device.D35: return "23";
                case Device.D36: return "24";
                case Device.D37: return "25";
                case Device.D38: return "26";
                case Device.D39: return "27";
                case Device.D40: return "28";
                case Device.D41: return "29";
                case Device.D42: return "2A";
                case Device.D43: return "2B";
                case Device.D44: return "2C";
                case Device.D45: return "2D";
                case Device.D46: return "2E";
                case Device.D47: return "2F";
                case Device.D48: return "31";
                case Device.D49: return "32";
                case Device.D50: return "33";
                case Device.D51: return "34";
                case Device.D52: return "35";
                case Device.D53: return "36";
                case Device.D54: return "37";
                case Device.D55: return "38";
                case Device.D56: return "39";
                case Device.D57: return "3A";
                case Device.D58: return "3B";
                case Device.D59: return "3C";
                case Device.D60: return "3D";
                case Device.D61: return "3E";
                case Device.D3F: return "3F";
            }
            return string.Empty;
        }

        /// <summary>
        /// Получить данные пакета в виде байтов
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Массив байтов</returns>
        public byte[] GetDataBytes(string packet)
        {
            string data = GetData(packet);
            if (data != string.Empty && data.Length > 2)
            {
                byte[] to_ret = new byte[data.Length / 2];
                for (int i = 0; i < to_ret.Length; i++)
                {
                    to_ret[i] = byte.Parse(data.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
                }
                return to_ret;
            }
            return null;
        }
    }
}