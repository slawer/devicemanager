using System;
using System.Threading;

namespace DeviceManager
{
    public partial class Serial
    {
        // ---- статистика ----

        protected long c_send_bytes = 0;                        // общее количество отправленных байт
        protected long c_send_packets = 0;                      // общее количество отправленных пакетов

        protected long c_received_bytes = 0;                    // количество полученных байт из порта
        protected long c_received_packets = 0;                  // количество полученных пакетов из порта

        protected long c_lost_bytes = 0;                        // количество лишних байтов
        protected long c_lost_packets = 0;                      // количество не ответов на пакеты на которые должны ответить

        protected long perfomance = 0;                          // индекс производительности

        protected long SerrialErrorFrame = 0;                   // общее количество ошибок кадрирования
        protected long SerrialErrorOverrun = 0;                 // общее количество ошибок переполнение буфера символов
        protected long SerrialErrorRXOver = 0;                  // общее количество ошибок переполнение входного буфера
        protected long SerrialErrorRXParity = 0;                // общее количество ошибок четности
        protected long SerrialErrorTXFull = 0;                  // общее количество ошибок переполнение выходного буфера
        protected long SerrialErrorUnknown = 0;                 // общее количество неизвестных ошибок

        /// <summary>
        /// Общее количество отправленных байт
        /// </summary>
        public long SendBytes
        {
            get { return Interlocked.Read(ref c_send_bytes); }
            set { Interlocked.Exchange(ref c_send_bytes, value); }
        }

        /// <summary>
        /// Общее количество отправленных пакетов
        /// </summary>
        public long SendPackets
        {
            get { return Interlocked.Read(ref c_send_packets); }
            set { Interlocked.Exchange(ref c_send_packets, value); }
        }

        /// <summary>
        /// Количество полученных байт из порта
        /// </summary>
        public long ReceivedBytes
        {
            get { return Interlocked.Read(ref c_received_bytes); }
            set { Interlocked.Exchange(ref c_received_bytes, value); }
        }

        /// <summary>
        /// Количество полученных пакетов из порта
        /// </summary>
        public long ReceivedPackets
        {
            get { return Interlocked.Read(ref c_received_packets); }
            set { Interlocked.Exchange(ref c_received_packets, value); }
        }

        /// <summary>
        /// Общее количество ошибок кадрирования
        /// </summary>
        public long SerialErrorFrame
        {
            get { return Interlocked.Read(ref SerrialErrorFrame); }
            set { Interlocked.Exchange(ref SerrialErrorFrame, value); }
        }

        /// <summary>
        /// Общее количество ошибок переполнение буфера символов
        /// </summary>
        public long SerialErrorOverrun
        {
            get { return Interlocked.Read(ref SerrialErrorOverrun); }
            set { Interlocked.Exchange(ref SerrialErrorOverrun, value); }
        }

        /// <summary>
        /// Общее количество ошибок переполнение входного буфера
        /// </summary>
        public long SerialErrorRXOver
        {
            get { return Interlocked.Read(ref SerrialErrorRXOver); }
            set { Interlocked.Exchange(ref SerrialErrorRXOver, value); }
        }

        /// <summary>
        /// Общее количество ошибок четности
        /// </summary>
        public long SerialErrorRXParity
        {
            get { return Interlocked.Read(ref SerrialErrorRXParity); }
            set { Interlocked.Exchange(ref SerrialErrorRXParity, value); }
        }

        /// <summary>
        /// Общее количество ошибок переполнение выходного буфера
        /// </summary>
        public long SerialErrorTXFull
        {
            get { return Interlocked.Read(ref SerrialErrorTXFull); }
            set { Interlocked.Exchange(ref SerrialErrorTXFull, value); }
        }

        /// <summary>
        /// Общее количество неизвестных ошибок
        /// </summary>
        public long SerialErrorUnknown
        {
            get { return Interlocked.Read(ref SerrialErrorUnknown); }
            set { Interlocked.Exchange(ref SerrialErrorUnknown, value); }
        }

        /// <summary>
        /// Определяет количество лишних байтов
        /// </summary>
        public long LostBytes
        {
            get { return Interlocked.Read(ref c_lost_bytes); }
            set { Interlocked.Exchange(ref c_lost_bytes, value); }
        }

        /// <summary>
        /// Определяет количество не ответов на пакеты на которые должны ответить
        /// </summary>
        public long LostPackets
        {
            get { return Interlocked.Read(ref c_lost_packets); }
            set { Interlocked.Exchange(ref c_lost_packets, value); }
        }

        /// <summary>
        /// Возвращяет индекс производительности
        /// </summary>
        protected long Perfomance
        {
            get { return Interlocked.Read(ref perfomance); }
        }
    }
}