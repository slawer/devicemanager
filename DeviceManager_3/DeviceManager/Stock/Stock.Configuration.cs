using System;
using System.Xml;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение пакетов поступивших от устройств и передачу конвертеру сохраненных пакетов
    /// </summary>
    public partial class Stock
    {
        /// <summary>
        /// Имя узла в котором находятся настроки подсистемы
        /// осуществляющей извлечение данных из пакетов
        /// </summary>
        protected const string rootName = "Stock_Config";

        /// <summary>
        /// Имя узла в котором хранятся данные параметра
        /// </summary>
        protected const string channelName = "channel";

        /// <summary>
        /// Имя узла в котором сохранен номер устройтсва
        /// </summary>
        protected const string deviceName = "device";

        /// <summary>
        /// Имя узла в котором сохранено смещение в пакете
        /// </summary>
        protected const string offsetName = "offset";

        /// <summary>
        /// Имя узла в котором сохранен размер извлекаемых данных
        /// </summary>
        protected const string lenghtName = "lenght";

        /// <summary>
        /// Имя узла в котором сохранено описание параметра
        /// </summary>
        protected const string descriptionName = "decription";

        /// <summary>
        /// Имя узла в котором сохранена позиция в которую сохранять извлеченное из пакета значение
        /// </summary>
        protected const string positionName = "position";

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="document">Документ в который выполнить сохранение настроек</param>
        public void Save(XmlDocument document)
        {
            try
            {
                Parameter[] conditions = Conditions;
                if (conditions != null)
                {
                    XmlNode root = document.CreateElement(rootName);
                    foreach (var condition in conditions)
                    {
                        XmlNode channel = document.CreateElement(channelName);

                        XmlNode device = document.CreateElement(deviceName);
                        device.InnerText = condition.Device.ToString();

                        XmlNode offset = document.CreateElement(offsetName);
                        offset.InnerText = condition.Offset.ToString();

                        XmlNode lenght = document.CreateElement(lenghtName);
                        lenght.InnerText = condition.Size.ToString();

                        XmlNode desc = document.CreateElement(descriptionName);
                        desc.InnerText = condition.Description;

                        XmlNode position = document.CreateElement(positionName);
                        position.InnerText = condition.Position.ToString();

                        channel.AppendChild(device);
                        channel.AppendChild(offset);

                        channel.AppendChild(lenght);
                        channel.AppendChild(desc);

                        channel.AppendChild(position);
                        root.AppendChild(channel);                        
                    }

                    document.DocumentElement.AppendChild(root);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Stock Save: " + ex.Message, ErrorType.NotFatal));
                }
            }
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="document">документ из которого загрузить настройки</param>
        public void Load(XmlNode root)
        {
            try
            {
                XmlNode node = FindNode(root);
                if (node != null)
                {
                    if (node.ChildNodes != null)
                    {
                        foreach (XmlNode channel in node.ChildNodes)
                        {
                            if (channel != null)
                            {
                                if (channel.Name == channelName)
                                {
                                    InsertChannel(channel);
                                }
                            }
                        }

                        SortConditions();
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Stock Load: " + ex.Message, ErrorType.NotFatal));
                }
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

        /// <summary>
        /// Добавить параметр из XML узла
        /// </summary>
        /// <param name="channel">Узем из которого извлечь данные для добавлениея параметра</param>
        protected void InsertChannel(XmlNode channel)
        {
            if (channel != null)
            {
                XmlNodeList childs = channel.ChildNodes;
                if (childs != null)
                {
                    Parameter parameter = new Parameter();
                    foreach (XmlNode node in childs)
                    {
                        
                        switch (node.Name)
                        {
                            case deviceName:

                                try
                                {
                                    parameter.Device = int.Parse(node.InnerText);
                                }
                                catch 
                                {
                                    continue;
                                }
                                break;

                            case offsetName:

                                try
                                {
                                    parameter.Offset = int.Parse(node.InnerText);
                                }
                                catch
                                {
                                    continue;
                                }
                                break;

                            case lenghtName:

                                try
                                {
                                    parameter.Size = int.Parse(node.InnerText);
                                }
                                catch
                                {
                                    continue;
                                }
                                break;

                            case descriptionName:

                                parameter.Description = node.InnerText;
                                break;

                            case positionName:

                                try
                                {
                                    parameter.Position = int.Parse(node.InnerText);
                                }
                                catch
                                {
                                    continue;
                                }
                                break;

                            default:

                                break;
                        }
                    }

                    InsertCondition(parameter);
                }
            }
        }
    }
}