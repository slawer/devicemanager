using System;

namespace DeviceManager
{
    /// <summary>
    /// Реализует значение
    /// </summary>
    public class Float
    {
        private Single value;               // Текущее значение
        private DateTime time;              // Время когда было присвоено значение

        private TimeSpan timeout;           // интервал времени в течении которого значение устарело

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Float()
        {
            value = float.NaN;
            time = DateTime.Now;

            timeout = new TimeSpan(0, 0, 0, 3);
        }

        /// <summary>
        /// Определяет текущее значени числа
        /// </summary>
        public float Value
        {
            get
            {
                TimeSpan span = DateTime.Now - time;
                if (span > timeout)
                {
                    return float.NaN;
                }
                else
                    return value;
            }
            set
            {
                this.value = value;
                time = DateTime.Now;
            }
        }

        /// <summary>
        /// Получить значение параметра без учета времени поступления
        /// </summary>
        /// <returns>Текущее значение параметра</returns>
        public float GetCurrentValue()
        {
            return value;
        }
    }
}