using System;

namespace DeviceManager
{
    /// <summary>
    /// Определяет тип параметра
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// Параметр является сигналом с датчика
        /// </summary>
        Signal,

        /// <summary>
        /// Параметр является вычисляемым
        /// </summary>
        Result,

        /// <summary>
        /// Тип параметра не определен
        /// </summary>
        Default
    }
}