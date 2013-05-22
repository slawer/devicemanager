using System;

namespace DeviceManager
{
    /// <summary>
    /// Реализует приложение DeviceManager.
    /// Является управляющим классом, предоставляет интерфейсы ...
    /// </summary>
    public partial class Application
    {
        protected static Application application = null;            // ссылка на самого себя. одиночка

        /// <summary>
        /// Получить экземпляр класса Application
        /// </summary>
        /// <returns>Экземпляр класса Application</returns>
        public static Application CreateInstance()
        {
            try
            {
                if (application == null)
                {
                    application = new Application();                    
                }
                return application;
            }
            catch
            {
                return null;
            }
        }
    }
}