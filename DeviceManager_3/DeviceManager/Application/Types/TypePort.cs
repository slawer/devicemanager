using System;

namespace DeviceManager
{
    /// <summary>
    /// Перечисляет типы портов в системе
    /// </summary>
    public enum TypePort
    {
        /// <summary>
        /// Основной порт
        /// </summary>
        Primary,

        /// <summary>
        /// Вспомогательный порт
        /// </summary>
        Secondary,

        /// <summary>
        /// По умолчанию
        /// </summary>
        Default
    }
}