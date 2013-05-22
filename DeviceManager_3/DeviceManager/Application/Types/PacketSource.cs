using System;

namespace DeviceManager
{
    /// <summary>
    /// Перечисляет источник пакета
    /// </summary>
    public enum PacketSource
    {
        /// <summary>
        /// Команда опроса
        /// </summary>
        Static,

        /// <summary>
        /// Не известная команда
        /// </summary>
        Dynamic,

        /// <summary>
        /// Команда не определена
        /// </summary>
        Default
    }
}