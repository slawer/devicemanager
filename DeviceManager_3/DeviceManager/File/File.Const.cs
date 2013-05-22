using System;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class File
    {
        /// <summary>
        /// Смещение по которому распологаются отметки время/смещение
        /// </summary>
        protected const long BASE_STAMPS_OFFSET = 512;

        /// <summary>
        /// Количество меток время/смещение
        /// </summary>
        protected const long COUNT_STAMPS = 61440;

        /// <summary>
        /// Смещение по которому распологаются данные
        /// </summary>
        protected const long BASE_DATA_OFFSET = (BASE_STAMPS_OFFSET + (COUNT_STAMPS * STAM_SIZE));

        /// <summary>
        /// Размер блока данных
        /// </summary>
        protected const long BLOCK_SIZE = 512;

        /// <summary>
        /// Размер метки время/смещение
        /// </summary>
        protected const long STAM_SIZE = 24;

        /// <summary>
        /// Частота сохранения данных в файл
        /// </summary>
        protected const long DEF_PERIOD = 1000;

        /// <summary>
        /// Размер области в которую сохранятся данные
        /// </summary>
        protected const long DATA_AREA_SIZE = 1072266752 - 10485760;

        /// <summary>
        /// Частота сохранения меток время/смещение
        /// </summary>
        internal const long DEF_SPERIOD = 30000;
    }
}