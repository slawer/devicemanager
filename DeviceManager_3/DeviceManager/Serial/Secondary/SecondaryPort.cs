using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует второстепенный порт
    /// </summary>
    public partial class SecondaryPort : Serial
    {
        protected long actived = 0;                 // используется порт или нет

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public SecondaryPort(Repository repository)
            : base(repository)
        {
            t_port = TypePort.Secondary;
        }

        /// <summary>
        /// Определяет использовать вспомогательный порт или нет
        /// </summary>
        public Boolean IsActived
        {
            get
            {
                return (Interlocked.Read(ref actived) == 1);
            }

            set
            {
                if (value == true)
                {
                    Interlocked.Exchange(ref actived, 1);
                }
                else
                    Interlocked.Exchange(ref actived, 0);
            }
        }
    }
}