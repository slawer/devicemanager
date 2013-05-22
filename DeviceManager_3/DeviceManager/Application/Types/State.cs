using System;

namespace DeviceManager
{
    /// <summary>
    /// Определяет возможные состояния конкретной подсистемы
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Выполняет обмен с устройствами.
        /// </summary>
        Running,

        /// <summary>
        /// Остановлен обмен с устройствами.
        /// </summary>
        Stopped,

        /// <summary>
        /// Обмен с устройствами не осуществляется и не был запущен.
        /// </summary>
        Default
    }
}