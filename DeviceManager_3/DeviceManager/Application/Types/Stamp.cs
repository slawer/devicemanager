using System;

namespace DeviceManager
{
    /// <summary>
    /// Определяет метку время/смещение для файла в который сохраняются данные
    /// </summary>
    public class Stamp
    {
        private long datetime;          // время

        private long f_offset;          // первое смещение
        private long s_offset;          // второе смещение

        public int index = 0;

        /// <summary>
        /// инициализирует новый экземпляр класса
        /// </summary>
        public Stamp()
        {
        }

        /// <summary>
        /// Определяет время для метки время/смещение
        /// </summary>
        public long DateTime
        {
            get { return datetime; }
            set { datetime = value; }
        }

        /// <summary>
        /// Определяет первое смещение на блок данных для метки
        /// </summary>
        public long F_Offset
        {
            get { return f_offset; }
            set { f_offset = value; }
        }

        /// <summary>
        /// Определяет второе (проверочное) смещение на блок данных для метки
        /// </summary>
        public long S_Offset
        {
            get { return s_offset; }
            set { s_offset = value; }
        }
    }
}