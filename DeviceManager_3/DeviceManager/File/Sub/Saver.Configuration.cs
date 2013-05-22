using System;
using System.Xml;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class Saver
    {
        /// <summary>
        /// Имя узла в котором находятся настроки подсистемы
        /// осуществляющей сохранение данных в файл
        /// </summary>
        protected const string rootName = "Saver_Config";

        /// <summary>
        /// узел в который сохраняется URI файла
        /// </summary>
        protected const string uriName = "uri";

        /// <summary>
        /// узел в который сохраняется тайм аут
        /// </summary>
        protected const string timeoutName = "timeout";

        /// <summary>
        /// узел в котором сохранены настройки параметра
        /// </summary>
        protected const string parameterName = "parameter";

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="document">Документ в который выполнить сохранение настроек</param>
        public void Save(XmlDocument document) 
        {
            try
            {
                Parameter[] parameters = Parameters;
                if (parameters != null)
                {
                    XmlNode root = document.CreateElement(rootName);
                    XmlNode timeoutNode = document.CreateElement(timeoutName);

                    timeoutNode.InnerText = timeout.ToString();

                    root.AppendChild(timeoutNode);
                    
                    if (file != null)
                    {
                        if (file.IsLoaded)
                        {
                            XmlNode uriNode = document.CreateElement(uriName);
                            uriNode.InnerText = file.URI;

                            root.AppendChild(uriNode);
                        }
                    }

                    foreach (Parameter parameter in parameters)
                    {
                        XmlNode paramNode = parameter.CreateXmlNode(document);
                        root.AppendChild(paramNode);
                    }

                    document.DocumentElement.AppendChild(root);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="root">Узел в котором находятся настройки</param>
        public void Load(XmlNode root) 
        {
            try
            {
                string uri = string.Empty;
                XmlNode node = FindNode(root);

                if (node != null)
                {
                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            switch (child.Name)
                            {
                                case uriName:

                                    try
                                    {
                                        uri = child.InnerText;
                                    }
                                    catch
                                    {
                                    }
                                    break;

                                case timeoutName:

                                    try
                                    {
                                        timeout = TimeSpan.Parse(child.InnerText);                                        
                                    }
                                    catch
                                    {
                                    }
                                    break;

                                case parameterName:

                                    try
                                    {
                                        Parameter parameter = new Parameter();
                                        parameter.InstanceFromXml(child);

                                        Insert(parameter);
                                    }
                                    catch
                                    {
                                    }
                                    break;

                            }
                        }

                        if (uri != string.Empty)
                        {
                            file.Load(uri);
                        }
                    }
                }
            }
            catch
            {
            }
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
                            if (node.Name == rootName)
                            {
                                return node;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Stock FindNode: " + ex.Message, ErrorType.NotFatal));
                }
            }

            return null;
        }
    }
}