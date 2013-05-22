using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует второстепенный порт
    /// </summary>
    public partial class SecondaryPort
    {
        /// <summary>
        /// Обработчик таймерного события
        /// </summary>
        /// <param name="state">Не используется, равно null.</param>
        protected override void TimerElapsed(object state)
        {
            if (app.Mode != ApplicationMode.Emulated)
            {
                if (IsActived)
                {
                    base.TimerElapsed(state);
                }
            }
        }
    }
}