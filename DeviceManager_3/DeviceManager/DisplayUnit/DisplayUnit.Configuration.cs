using System;
using System.Xml;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует формирование и отправку данных на блок отображения
    /// </summary>
    public partial class DisplayUnit
    {
        /// <summary>
        /// Имя узла в котором находятся настроки подсистемы
        /// осуществляющей отправку данные на БО
        /// </summary>
        protected const string rootName = "Display_Config";

        /// <summary>
        /// имя узла в который сохраняются настройки пакета на БО
        /// </summary>
        protected const string packetName = "displaypacket";

        /// <summary>
        /// имя узла в который сохраняются настройки параметра для отправки на БО
        /// </summary>
        protected const string parameterName = "parameter";

        /// <summary>
        /// узел в котором указывается описание пакета
        /// </summary>
        protected const string commentName = "comment";

        /// <summary>
        /// узел в котором сохраняется номер устройства от имени которого отвечать
        /// </summary>
        protected const string deviceName = "device";

        /// <summary>
        /// узел в котором сохраняется состояние пакета для БО
        /// </summary>
        protected const string activedName = "actived";

        /// <summary>
        /// узел в котором сохраняется состояние отправки пакета в порт
        /// </summary>
        protected const string toPortName = "toport";

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="document">Документ в который выполнить сохранение настроек</param>
        public void Save(XmlDocument document) 
        {
            try
            {
                DisplayPacket[] packets = Packets;
                if (packets != null)
                {
                    XmlNode root = document.CreateElement(rootName);
                    foreach (DisplayPacket packet in packets)
                    {
                        XmlNode packetNode = document.CreateElement(packetName);
                        
                        XmlNode commentNode = document.CreateElement(commentName);
                        XmlNode deviceNode = document.CreateElement(deviceName);

                        XmlNode activedNode = document.CreateElement(activedName);
                        XmlNode toportNode = document.CreateElement(toPortName);

                        XmlNode portTypeNode = document.CreateElement("port_type");
                        XmlNode periodNode = document.CreateElement("period");

                        commentNode.InnerText = packet.Description;
                        deviceNode.InnerText = packet.Device.ToString();

                        activedNode.InnerText = packet.IsActived.ToString();
                        toportNode.InnerText = packet.ToPort.ToString();

                        portTypeNode.InnerText = packet.TypePort.ToString();
                        periodNode.InnerText = packet.Period.ToString();

                        packetNode.AppendChild(commentNode);
                        packetNode.AppendChild(deviceNode);

                        packetNode.AppendChild(activedNode);
                        packetNode.AppendChild(toportNode);

                        packetNode.AppendChild(portTypeNode);
                        packetNode.AppendChild(periodNode);

                        Parameter[] parameters = packet.Parameters;
                        if (parameters != null)
                        {
                            foreach (Parameter parameter in parameters)
                            {
                                XmlNode paramNode = parameter.CreateXmlNode(document);
                                if (paramNode != null)
                                {
                                    packetNode.AppendChild(paramNode);
                                }
                            }
                        }

                        root.AppendChild(packetNode);
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
        /// <param name="node">Узел в котором находятся настройки</param>
        public void Load(XmlNode root) 
        {
            try
            {
                XmlNode node = FindNode(root);
                if (node != null)
                {
                    if (node.HasChildNodes)
                    {
                        DisplayPacket packet = new DisplayPacket();
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Name == packetName)
                            {
                                InsertPacket(child);
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
        /// Добавить параметр
        /// </summary>
        /// <param name="node">Узел в котором сохранены параметры пакета</param>
        protected void InsertPacket(XmlNode node)
        {
            try
            {
                if (node.HasChildNodes)
                {
                    DisplayPacket packet = new DisplayPacket();
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        switch (child.Name)
                        {
                            case parameterName:

                                try
                                {
                                    Parameter parameter = new Parameter();
                                    parameter.InstanceFromXml(child);

                                    packet.Insert(parameter);
                                }
                                catch
                                {
                                }
                                break;

                            case commentName:

                                try
                                {
                                    packet.Description = child.InnerText;
                                }
                                catch
                                {
                                }
                                break;

                            case deviceName:

                                try
                                {
                                    packet.Device = int.Parse(child.InnerText);
                                }
                                catch
                                {
                                }
                                break;

                            case activedName:

                                try
                                {
                                    packet.IsActived = bool.Parse(child.InnerText);
                                }
                                catch { }
                                break;

                            case toPortName:

                                try
                                {
                                    packet.ToPort = bool.Parse(child.InnerText);
                                }
                                catch { }
                                break;

                            case "port_type":

                                try
                                {
                                    packet.TypePort = (TypePort)Enum.Parse(typeof(TypePort), child.InnerText);
                                }
                                catch { }
                                break;

                            case "period":

                                try
                                {
                                    packet.Period = TimeSpan.Parse(child.InnerText);
                                }
                                catch { }
                                break;

                            default:
                                break;
                        }
                    }

                    Insert(packet);
                }
            }
            catch
            {
            }
        }
    }
}