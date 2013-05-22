using System;
using System.Xml;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Определяет интерфейс, который должна реализовывать каждая подсистема
    /// </summary>
    interface IComponent
    {
        /// <summary>
        /// Генерируется при возникновении ошибки. Добавить в интерфейс
        /// </summary>
        event ApplicationErrorHandler OnError;

        /// <summary>
        /// Возникает при запуске подсистемы
        /// </summary>
        event EventHandler OnStart;

        /// <summary>
        /// Возникает при остановке подсистемы
        /// </summary>
        event EventHandler OnStop;

        /// <summary>
        /// Запустить службу
        /// </summary>
        void Run();

        /// <summary>
        /// Остановить службу
        /// </summary>
        void Stop();

        /// <summary>
        /// Выполнить инициализацию компонента
        /// </summary>
        void Initialize();

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="document">Документ в который выполнить сохранение настроек</param>
        void Save(XmlDocument document);

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="root">Узел в котором находятся настройки подсистемы</param>
        void Load(XmlNode root);
    }
}