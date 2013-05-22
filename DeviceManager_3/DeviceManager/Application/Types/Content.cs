using System;

namespace DeviceManager
{
    /// <summary>
    /// Перечисляет возможные варианты получения значений от устройства
    /// </summary>
    public enum Content
    {
        /// <summary>
        /// Не известное содержимое
        /// </summary>
        Unknown,

        /// <summary>
        /// Значения с датчиков
        /// </summary>
        Sensor,

        /// <summary>
        /// Содержимое не определено
        /// </summary>
        Default
    }
}