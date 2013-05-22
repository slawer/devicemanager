using System;

namespace SoftwareDevelopmentKit.Protocols.Dsn
{
    /// <summary>
    /// Перечесление, содержащее версии протокола
    /// </summary>
    public enum ProtocolVersion
    {
        /// <summary>
        /// Первая версия
        /// </summary>
        x100
    }

    /// <summary>
    /// Реализует работу с протоколом обмена
    /// </summary>
    public class Protocol
    {
        /// <summary>
        /// Инициализтрует новый экземпляр класса
        /// </summary>
        protected Protocol()
        {
        }

        /// <summary>
        /// Получить протокол
        /// </summary>
        /// <param name="version">версия протокола</param>
        /// <returns></returns>
        public static IProtocol GetProtocol(ProtocolVersion version)
        {
            switch (version)
            {
                case ProtocolVersion.x100:

                    return VersionX100.CreateProtocol();

                default:

                    return null;
            }
        }
    }
}