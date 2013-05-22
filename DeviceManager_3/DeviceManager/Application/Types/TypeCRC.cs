using System;

namespace DeviceManager
{
    /// <summary>
    /// Перечисляет возможные типы CRC
    /// </summary>
    public enum TypeCRC
    {
        /// <summary>
        /// Простая циклическая однобайтная CRC
        /// </summary>
        Cycled,

        /// <summary>
        /// Циклическая двухбайтная CRC
        /// </summary>
        CycledTwo,

        /// <summary>
        /// Вычислять по алгоритму CRC-16 двухбайтная
        /// </summary>
        CRC16
    }
}