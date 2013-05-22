using System;

namespace DeviceManager.Errors
{
    /// <summary>
    /// определяет тип ошибки
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// Информация о текущем состянии
        /// </summary>
        Information,

        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning,

        /// <summary>
        /// Ошибка не фатальная, приложение может продолжать работу
        /// </summary>
        NotFatal,

        /// <summary>
        /// Ошибка фатальная, приложение должно завершить свою работу
        /// </summary>
        Fatal,

        /// <summary>
        /// Не известная ошибка, приложение может продолжать работу
        /// </summary>
        Unknown,

        /// <summary>
        /// Тип ошибки не определен
        /// </summary>
        Default
    }

    /// <summary>
    /// Реализует класс в котором передаются параметры события об ошибке
    /// </summary>
    public class ErrorArgs : EventArgs
    {
        private String message = string.Empty;              // сообщение
        private ErrorType type = ErrorType.Default;         // тип ошибки

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public ErrorArgs()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="Notice">Сообщение</param>
        public ErrorArgs(string Notice)
        {
            message = Notice;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="Notice">Сообщение</param>
        /// <param name="TypeError">Тип ошибки</param>
        public ErrorArgs(string Notice, ErrorType TypeError)
        {
            message = Notice;
            type = TypeError;
        }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Тип ошибки
        /// </summary>
        public ErrorType ErrorType
        {
            get { return type; }
        }
    }

    /// <summary>
    /// Определяет интерфейс метода, осуществляющего обработку ошибок
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="args">Параметры события</param>
    public delegate void ApplicationErrorHandler(object sender, ErrorArgs args);
}