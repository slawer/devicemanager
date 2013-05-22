using System;

namespace DeviceManager
{
    /// <summary>
    /// Определяет роль пакета
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Ответ от устройства
        /// </summary>
        Slave,

        /// <summary>
        /// Запрос устройству
        /// </summary>
        Master,

        /// <summary>
        /// Тип запроса не определен
        /// </summary>
        Default
    }
}