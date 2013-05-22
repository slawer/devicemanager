using System;
using System.Xml;

namespace DeviceManager
{
    /// <summary>
    /// Реализует приложение DeviceManager.
    /// Является управляющим классом, предоставляет интерфейсы ...
    /// </summary>
    public partial class Application
    {
        /// <summary>
        /// Имя файла в котором содержатся настройки программы
        /// </summary>
        protected const string config = "application.config";

        /// <summary>
        /// Корневой узел
        /// </summary>
        protected const string rootName = "DeviceManager2.0_Configuration_File";

        /// <summary>
        /// Имя узла в котором содержится тип используемой контрольной суммы
        /// </summary>
        protected const string TypeCRCName = "typeCRC";

        /// <summary>
        /// Имя узла в котором содержится режим в котором работает приложение
        /// </summary>
        protected const string ApplicationModeName = "applicationMode";

        /// <summary>
        /// Имя узла в котором содержится знчение переменной указывающей на необходимость выполнять автозапуск программы на исполнение
        /// </summary>
        protected const string AutorunName = "autorun";

        /// <summary>
        /// Имя узла в котором содержится знчение переменной указывающей на необходимость сворачивать главное окно в трей
        /// </summary>
        protected const string IsNotifyName = "notify";

        /// <summary>
        /// Корневой узел настроек TCP devMan
        /// </summary>
        protected const string tcpDevRootName = "TCP_DevMan";

        /// <summary>
        /// Количество клиентов
        /// </summary>
        protected const string countClientsName = "clientsCount";

        /// <summary>
        /// Размер буфера для одного клиента
        /// </summary>
        protected const string clientBufferSizeName = "clientBufferSizeName";

        /// <summary>
        /// Номер порта с которым работать
        /// </summary>
        protected const string portName = "portName";

        /// <summary>
        /// Сохранить настройки программы
        /// </summary>
        public void SaveConfiguration()
        {
            XmlDocument doc = null;
            try
            {
                IComponent[] components = Components;
                if (components != null)
                {
                    doc = new XmlDocument();
                    XmlElement root = doc.CreateElement(rootName);

                    doc.AppendChild(root);
                    SaveApplicationConfiguration(doc, root);

                    foreach (var component in components)
                    {
                        component.Save(doc);
                    }

                    SaveTcpConfiguration(doc);


                    string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string totalPathCfg = string.Format("{0}\\{1}", path, config);

                    doc.Save(totalPathCfg);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Загрузить настройки программы
        /// </summary>
        public void LoadConfiguration()
        {
            XmlDocument document = null;
            try
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string totalPathCfg = string.Format("{0}\\{1}", path, config);

                if (System.IO.File.Exists(totalPathCfg))
                {
                    document = new XmlDocument();

                    document.Load(totalPathCfg);
                    XmlNode root = document.FirstChild;

                    if (root != null)
                    {
                        if (root.Name == rootName)
                        {
                            LoadApplicationConfiguration(root);
                            LoadTcpDevManOldConfiguration(root);

                            IComponent[] components = Components;

                            if (components != null)
                            {
                                foreach (var component in components)
                                {
                                    component.Load(root);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (journal != null)
                    {
                        journal.Write("Не найден файл конфигурации.", System.Diagnostics.EventLogEntryType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                if (journal != null)
                {
                    journal.Write("Ошибка при загрузке конфигурации: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Возвращяет массив компонентов используемых приложением
        /// </summary>
        internal IComponent[] Components
        {
            get
            {
                IComponent[] components = new IComponent[5];

                components[0] = serial;
                components[1] = stock;

                components[2] = converter;
                components[3] = display;

                components[4] = saver;

                return components;
            }
        }

        /// <summary>
        /// Сохранить конфишурацию приложения
        /// </summary>
        /// <param name="document">Докумен в который осуществляется запись настроек приложения</param>
        /// <param name="root">Элемент в который осуществлять сохранение параметров приложения</param>
        internal void SaveApplicationConfiguration(XmlDocument document, XmlNode root)
        {
            try
            {
                if (root != null)
                {
                    XmlNode typeCRCNode = document.CreateElement(TypeCRCName);
                    XmlNode applicationModeNode = document.CreateElement(ApplicationModeName);

                    XmlNode autorunNode = document.CreateElement(AutorunName);
                    XmlNode notifyNode = document.CreateElement(IsNotifyName);

                    typeCRCNode.InnerText = TypeCRC.ToString();
                    applicationModeNode.InnerText = Mode.ToString();

                    autorunNode.InnerText = Autorun.ToString();
                    notifyNode.InnerText = isNotify.ToString();

                    root.AppendChild(typeCRCNode);
                    root.AppendChild(applicationModeNode);

                    root.AppendChild(autorunNode);
                    root.AppendChild(notifyNode);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Сохранить настройки TCP сервера старого DevMan
        /// </summary>
        /// <param name="document">Докумен в который осуществляется запись настроек приложения</param>
        internal void SaveTcpConfiguration(XmlDocument document)
        {
            try
            {
                XmlNode root = document.CreateElement(tcpDevRootName);
                XmlNode countClientsNode = document.CreateElement(countClientsName);

                XmlNode clientBufferSizeNode = document.CreateElement(clientBufferSizeName);
                XmlNode portNode = document.CreateElement(portName);

                countClientsNode.InnerText = devTcpOld.CountConnections.ToString(); ;
                clientBufferSizeNode.InnerText = devTcpOld.ReceiverBufferSize.ToString();

                portNode.InnerText = devTcpOld.Port.ToString();

                root.AppendChild(countClientsNode);
                root.AppendChild(clientBufferSizeNode);

                root.AppendChild(portNode);
                document.DocumentElement.AppendChild(root);

            }
            catch { }
        }

        /// <summary>
        /// Загрузить настройки приложения
        /// </summary>
        /// <param name="root">Узем в котором находятся настройки приложения</param>
        internal void LoadApplicationConfiguration(XmlNode root)
        {
            try
            {
                if (root != null)
                {
                    if (root.Name == rootName)
                    {
                        XmlNodeList childs = root.ChildNodes;
                        if (childs != null)
                        {
                            foreach (XmlNode node in childs)
                            {
                                switch (node.Name)
                                {
                                    case TypeCRCName:

                                        try
                                        {
                                            TypeCRC = (DeviceManager.TypeCRC)Enum.Parse(typeof(DeviceManager.TypeCRC), node.InnerText);
                                        }
                                        catch
                                        {
                                            TypeCRC = TypeCRC.Cycled;
                                        }
                                        break;

                                    case ApplicationModeName:

                                        try
                                        {
                                            Mode = (ApplicationMode)Enum.Parse(typeof(ApplicationMode), node.InnerText);
                                        }
                                        catch
                                        {
                                            Mode = ApplicationMode.Active;
                                        }
                                        break;

                                    case AutorunName:

                                        try
                                        {
                                            Autorun = Boolean.Parse(node.InnerText);
                                        }
                                        catch
                                        {
                                            Autorun = false;
                                        }
                                        break;

                                    case IsNotifyName:

                                        try
                                        {
                                            isNotify = Boolean.Parse(node.InnerText);
                                        }
                                        catch
                                        {
                                            isNotify = false;
                                        }
                                        break;

                                    default:

                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Загрузить настройки TCP DevMan
        /// </summary>
        /// <param name="root">Узем в котором находятся настройки приложения</param>
        internal void LoadTcpDevManOldConfiguration(XmlNode root)
        {
            try
            {
                if (root != null)
                {
                    XmlNode root_node = FindNode(root);
                    if (root_node != null)
                    {
                        XmlNodeList childs = root_node.ChildNodes;
                        if (childs != null)
                        {
                            foreach (XmlNode node in childs)
                            {
                                switch (node.Name)
                                {
                                    case countClientsName:

                                        try
                                        {
                                            devTcpOld.CountConnections  = int.Parse(node.InnerText);
                                        }
                                        catch
                                        {
                                            devTcpOld.CountConnections = 10;
                                        }
                                        break;

                                    case clientBufferSizeName:

                                        try
                                        {
                                            devTcpOld.ReceiverBufferSize =int.Parse(node.InnerText);
                                        }
                                        catch
                                        {
                                            devTcpOld.ReceiverBufferSize = 512;
                                        }
                                        break;

                                    case portName:

                                        try
                                        {
                                            devTcpOld.Port = int.Parse(node.InnerText);
                                        }
                                        catch
                                        {
                                            devTcpOld.Port = 56000;
                                        }
                                        break;

                                    default:

                                        break;
                                }
                            }
                        }
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// Найти узел в котром содержатся данные настроек подсистемы
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        protected XmlNode FindNode(XmlNode root)
        {
            try
            {
                if (root != null)
                {
                    XmlNodeList lists = root.ChildNodes;
                    if (root != null)
                    {
                        foreach (XmlNode node in lists)
                        {
                            if (node.Name == tcpDevRootName)
                            {
                                return node;
                            }
                        }
                    }
                }
            }
            catch { }
            return null;
        }
    }
}