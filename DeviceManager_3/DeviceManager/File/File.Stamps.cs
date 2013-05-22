using System;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class File
    {
        protected Stamp[] stamps = null;        // метки время/смещение для файла

        /// <summary>
        /// Возвращяет метки время/смещение для файла
        /// </summary>
        public Stamp[] Stamps
        {
            get { return stamps; }
        }
    }
}