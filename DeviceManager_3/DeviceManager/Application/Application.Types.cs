using System;

namespace DeviceManager
{
    /// <summary>
    /// Реализует приложение DeviceManager.
    /// Является управляющим классом, предоставляет интерфейсы ...
    /// </summary>
    public partial class Application
    {
    }

    /// <summary>
    /// Представляет метод, осуществляющий обработку событий класса Serial
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="args">Аргументы события</param>
    public delegate void SerialEventHandler(object sender, SerialEventArgs args);

    /// <summary>
    /// Реализует класс, содержащий данные передаваемые при возникновении событий класса Serial
    /// </summary>
    public class SerialEventArgs : EventArgs
    {
        protected Packet packet = null;             // передаваемый пакет

        /// <summary>
        /// инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="pack">Пакет, для передачи</param>
        public SerialEventArgs(Packet pack)
        {
            packet = pack;
        }

        /// <summary>
        /// Возвращяет передаваемый пакет
        /// </summary>
        public Packet Packet
        {
            get { return packet; }
        }
    }
}