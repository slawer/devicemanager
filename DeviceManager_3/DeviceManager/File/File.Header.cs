using System;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class File
    {
        // ---- заголовок файла ----

        protected long block_size;      // Размер одного блока записываемой информации 
                                        // (сугубо информативный, не влияет на работу DeviceManager2). По умолчанию равен 512 байтам

        protected long timestamp;       // Количество меток время/смещение (информативно, равен 1024)
        protected long area_size;       // Размер области в которую сохраняются данные

        protected long stamp_offset;    // Смещение блока меток время/смещение
        protected long data_offset;     // Смещение блока данных

        protected long p_count;         // Количество параметров, содержащихся в файле

        protected long speriod;         // частота сохранения меток время/смещение
        protected long period;          // частота сохранения данных

        /// <summary>
        /// Возвращяет размер сохраняемого блока данных
        /// </summary>
        public long BlockSize
        {
            get { return block_size; }
        }

        /// <summary>
        /// Количество меток время/смещение
        /// </summary>
        public long StampsCount
        {
            get { return timestamp; }
        }

        /// <summary>
        /// Возвращяет размер области в которую сохраняются данные
        /// </summary>
        public long DataAreaSize
        {
            get { return area_size; }
        }

        /// <summary>
        /// Возвращяет смещение по которому находятся блоки меток время/смещение
        /// </summary>
        public long StampsOffset
        {
            get { return stamp_offset; }
        }

        /// <summary>
        /// Возвращяет смещение по которому распологается область сохранения данных
        /// </summary>
        public long DataOffset
        {
            get { return data_offset; }
        }

        /// <summary>
        /// Возвращяет количество сохраняемых параметров в файле
        /// </summary>
        public long PCount
        {
            get { return p_count; }
        }

        /// <summary>
        /// Возвращяет частоту сохранения меток время/смещение
        /// </summary>
        public long SPeriod
        {
            get { return speriod; }
        }

        /// <summary>
        /// Возвращяет частоту сохранения данных в файл
        /// </summary>
        public long Period
        {
            get { return period; }
        }
    }
}