using System;
using System.Xml;
using System.IO.Ports;
using System.Globalization;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует обмен с устройствами через последовательный порт
    /// </summary>
    public partial class Serial
    {
        /// <summary>
        /// Имя узла в котором содержатся настройки последовательного порта
        /// </summary>
        protected const string serialRootName = "Serial_Config";

        /// <summary>
        /// Имя узла в котром содержится команда опроса датчиков
        /// </summary>
        protected const string staticCommandaName = "StaticPacket";

        /// <summary>
        /// Имя узла в который сохраняется пакет для отправки в COM порт
        /// </summary>
        protected const string com_packetName = "com_packet";

        /// <summary>
        /// имя узла в который сохраняется пакет для отправки по TCP
        /// </summary>
        protected const string tcp_packetName = "tcp_packet";

        /// <summary>
        /// имя узла в который сохраняется состояние пакета (ожидать ответ или нет)
        /// </summary>
        protected const string waitName = "wait";

        /// <summary>
        /// Имя узла в который сохраняется статус пакета (активный или нет)
        /// </summary>
        protected const string activedName = "actived";

        /// <summary>
        /// имя узла в котором сохраняется тип содержимого пакета
        /// </summary>
        protected const string contentName = "content";

        /// <summary>
        /// имя узла в котором сохряется роль пакета
        /// </summary>
        protected const string roleName = "role";

        /// <summary>
        /// имя узла в котором сохраняется источник пакета
        /// </summary>
        protected const string packetSourceName = "source";

        /// <summary>
        /// имя узла в котором сохраняется число битов в байте
        /// </summary>
        protected const string DataBitsName = "databits";

        /// <summary>
        /// имя узла в котором сохрается скорость работы с портом
        /// </summary>
        protected const string BaudRateName = "baudrate";

        /// <summary>
        /// имя узвла в котром сохраняется развер буфера чтени данных из порта
        /// </summary>
        protected const string ReadBufferSizeName = "readbuffersize";

        /// <summary>
        /// имя узла в котором сохраняется размер буфера для записи в порт
        /// </summary>
        protected const string WriteBufferSizeName = "writebuffersize";

        /// <summary>
        /// имя узла в котором сохраняется таймаут чтения одного байти из порта
        /// </summary>
        protected const string ReadTimeoutName = "readtimeout";

        /// <summary>
        /// имя узла в котором сохраняется таймаут чтения данных из порта
        /// </summary>
        protected const string WriteTimeoutName = "writetimeout";

        /// <summary>
        /// имя узла в котором сохраняется четность порта
        /// </summary>
        protected const string ParityName = "parity";

        /// <summary>
        /// имя узла в котором сохраняет количество стоповых битов
        /// </summary>
        protected const string StopBitsName = "stopbits";

        /// <summary>
        /// имя узла в котором сохраняется порт с которым работать
        /// </summary>
        protected const string portNameName = "portname";

        /// <summary>
        /// имя узла в котором сохраняется частота отправки пакетов в порт
        /// </summary>
        protected const string timerPerionName = "timerperiod";

        /// <summary>
        /// имя узла в котором сохраняется время ожидания ответа на запрос
        /// </summary>
        protected const string answerTimeoutName = "answertimeout";

        /// <summary>
        /// имя узла в котором сохраняется количество попыток чтения пакета
        /// </summary>
        protected const string attemptsToReadName = "attemptstoread";

        /// <summary>
        /// имя узла в котором сохраняется количество попыток чтения данных пакета
        /// </summary>
        protected const string attemptsCycledName = "attemptscycled";

        /// <summary>
        /// имя узла в котором сохраняется время между посылками пакетов в порт
        /// </summary>
        protected const string waitTimeoutName = "waittimeout";

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="document">Документ в который выполнить сохранение настроек</param>
        public void Save(XmlDocument document)
        {
            try
            {
                XmlNode root = document.CreateElement(serialRootName);
                if (root != null)
                {
                    SavePackets(document, root);
                }

                XmlNode DataBitsNode = document.CreateElement(DataBitsName);
                XmlNode BaudRateNode = document.CreateElement(BaudRateName);

                XmlNode ReadBufferSizeNode = document.CreateElement(ReadBufferSizeName);
                XmlNode WriteBufferSizeNode = document.CreateElement(WriteBufferSizeName);

                XmlNode ReadTimeoutNode = document.CreateElement(ReadTimeoutName);
                XmlNode WriteTimeoutNode = document.CreateElement(WriteTimeoutName);

                XmlNode ParityNode = document.CreateElement(ParityName);
                XmlNode StopBitsNode = document.CreateElement(StopBitsName);

                XmlNode portNameNode = document.CreateElement(portNameName);

                if (Port != null)
                {
                    DataBitsNode.InnerText = Port.DataBits.ToString();
                    BaudRateNode.InnerText = Port.BaudRate.ToString();

                    ReadBufferSizeNode.InnerText = Port.ReadBufferSize.ToString();
                    WriteBufferSizeNode.InnerText = Port.WriteBufferSize.ToString();

                    ReadTimeoutNode.InnerText = Port.ReadTimeout.ToString();
                    WriteTimeoutNode.InnerText = Port.WriteTimeout.ToString();

                    ParityNode.InnerText = Port.Parity.ToString();
                    StopBitsNode.InnerText = Port.StopBits.ToString();

                    portNameNode.InnerText = Port.PortName;

                    root.AppendChild(DataBitsNode);
                    root.AppendChild(BaudRateNode);

                    root.AppendChild(ReadBufferSizeNode);
                    root.AppendChild(WriteBufferSizeNode);

                    root.AppendChild(ReadTimeoutNode);
                    root.AppendChild(WriteTimeoutNode);

                    root.AppendChild(ParityNode);
                    root.AppendChild(StopBitsNode);

                    root.AppendChild(portNameNode);
                }

                XmlNode timerPerionNode = document.CreateElement(timerPerionName);
                XmlNode answerTimeoutNode = document.CreateElement(answerTimeoutName);

                XmlNode attemptsToReadNode = document.CreateElement(attemptsToReadName);
                XmlNode attemptsCycledNode = document.CreateElement(attemptsCycledName);

                XmlNode waitTimeoutNode = document.CreateElement(waitTimeoutName);

                timerPerionNode.InnerText = timerPerion.ToString();
                answerTimeoutNode.InnerText = answerTimeout.ToString();

                attemptsToReadNode.InnerText = attemptsToRead.ToString();
                attemptsCycledNode.InnerText = attemptsCycled.ToString();

                waitTimeoutNode.InnerText = waitTimeout.ToString();

                root.AppendChild(timerPerionNode);
                root.AppendChild(answerTimeoutNode);

                root.AppendChild(attemptsToReadNode);
                root.AppendChild(attemptsCycledNode);

                root.AppendChild(waitTimeoutNode);

                SaveSecondary(document, root);
                document.DocumentElement.AppendChild(root);
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Serial_Save: " + ex.Message, ErrorType.NotFatal));
                }
            }
        }

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="document">Документ в который выполнить сохранение настроек</param>
        /// <param name="root">Узел в который сохранить настройки вспомогательного порта</param>
        public void SaveSecondary(XmlDocument document, XmlNode root)
        {
            try
            {
                XmlNode rootSecondary = document.CreateElement("Secondary");

                XmlNode DataBitsNode = document.CreateElement(DataBitsName);
                XmlNode BaudRateNode = document.CreateElement(BaudRateName);

                XmlNode ReadBufferSizeNode = document.CreateElement(ReadBufferSizeName);
                XmlNode WriteBufferSizeNode = document.CreateElement(WriteBufferSizeName);

                XmlNode ReadTimeoutNode = document.CreateElement(ReadTimeoutName);
                XmlNode WriteTimeoutNode = document.CreateElement(WriteTimeoutName);

                XmlNode ParityNode = document.CreateElement(ParityName);
                XmlNode StopBitsNode = document.CreateElement(StopBitsName);

                XmlNode portNameNode = document.CreateElement(portNameName);
                XmlNode isUseNode = document.CreateElement("is_use");

                if (Secondary.Port != null)
                {
                    DataBitsNode.InnerText = Secondary.Port.DataBits.ToString();
                    BaudRateNode.InnerText = Secondary.Port.BaudRate.ToString();

                    ReadBufferSizeNode.InnerText = Secondary.Port.ReadBufferSize.ToString();
                    WriteBufferSizeNode.InnerText = Secondary.Port.WriteBufferSize.ToString();

                    ReadTimeoutNode.InnerText = Secondary.Port.ReadTimeout.ToString();
                    WriteTimeoutNode.InnerText = Secondary.Port.WriteTimeout.ToString();

                    ParityNode.InnerText = Secondary.Port.Parity.ToString();
                    StopBitsNode.InnerText = Secondary.Port.StopBits.ToString();

                    portNameNode.InnerText = Secondary.Port.PortName;
                    isUseNode.InnerText = Secondary.IsActived.ToString();

                    rootSecondary.AppendChild(DataBitsNode);
                    rootSecondary.AppendChild(BaudRateNode);

                    rootSecondary.AppendChild(ReadBufferSizeNode);
                    rootSecondary.AppendChild(WriteBufferSizeNode);

                    rootSecondary.AppendChild(ReadTimeoutNode);
                    rootSecondary.AppendChild(WriteTimeoutNode);

                    rootSecondary.AppendChild(ParityNode);
                    rootSecondary.AppendChild(StopBitsNode);

                    rootSecondary.AppendChild(portNameNode);
                    rootSecondary.AppendChild(isUseNode);
                }

                root.AppendChild(rootSecondary);
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("SerialSecondary_Save: " + ex.Message, ErrorType.NotFatal));
                }
            }
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="root">Узел в котором искать настройки для загрузки</param>
        public void Load(XmlNode root) 
        {
            try
            {
                XmlNode node = FindNode(root);
                if (node != null)
                {
                    if (node.ChildNodes != null)
                    {
                        foreach (XmlNode element in node.ChildNodes)
                        {
                            switch (element.Name)
                            {
                                case staticCommandaName:

                                    LoadPacket(element);
                                    break;

                                case DataBitsName:

                                    try
                                    {
                                        port.DataBits = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        port.DataBits = 8;
                                    }
                                    break;

                                case BaudRateName:

                                    try
                                    {
                                        port.BaudRate = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        port.BaudRate = 38400;
                                    }
                                    break;

                                case ReadBufferSizeName:

                                    try
                                    {
                                        port.ReadBufferSize = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        port.ReadBufferSize = 4096;
                                    }
                                    break;

                                case WriteBufferSizeName:

                                    try
                                    {
                                        port.WriteBufferSize = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        port.WriteBufferSize = 4096;
                                    }
                                    break;

                                case ReadTimeoutName:

                                    try
                                    {
                                        port.ReadTimeout = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        port.ReadTimeout = 30;
                                    }
                                    break;

                                case WriteTimeoutName:

                                    try
                                    {
                                        port.WriteTimeout = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        port.WriteTimeout = 100;
                                    }
                                    break;

                                case ParityName:

                                    try
                                    {
                                        port.Parity = (Parity)Enum.Parse(typeof(Parity), element.InnerText);
                                    }
                                    catch
                                    {
                                        port.Parity = Parity.None;
                                    }
                                    break;

                                case StopBitsName:

                                    try
                                    {
                                        port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), element.InnerText);
                                    }
                                    catch
                                    {
                                        port.StopBits = StopBits.One;
                                    }
                                    break;

                                case portNameName:

                                    try
                                    {
                                        port.PortName = element.InnerText;
                                    }
                                    catch
                                    {
                                        port.PortName = string.Empty;
                                    }
                                    break;

                                case timerPerionName:

                                    try
                                    {
                                        timerPerion = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        timerPerion = 10;
                                    }
                                    break;

                                case answerTimeoutName:

                                    try
                                    {
                                        answerTimeout = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        answerTimeout = 100;
                                    }
                                    break;

                                case attemptsToReadName:

                                    try
                                    {
                                        attemptsToRead = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        attemptsToRead = 1;
                                    }
                                    break;

                                case attemptsCycledName:

                                    try
                                    {
                                        attemptsCycled = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        attemptsCycled = 128;
                                    }
                                    break;

                                case waitTimeoutName:

                                    try
                                    {
                                        waitTimeout = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        waitTimeout = 20;
                                    }
                                    break;

                                case "Secondary":

                                    try
                                    {
                                        LoadSecondary(element);
                                    }
                                    catch { }
                                    break;

                                default:

                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Serial Load: " + ex.Message, ErrorType.NotFatal));
                }
            }
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="root">Узел в котором искать настройки для загрузки</param>
        public void LoadSecondary(XmlNode root)
        {
            try
            {
                XmlNode node = root;
                if (node != null)
                {
                    if (node.ChildNodes != null)
                    {
                        foreach (XmlNode element in node.ChildNodes)
                        {
                            switch (element.Name)
                            {

                                case DataBitsName:

                                    try
                                    {
                                        Secondary.port.DataBits = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        Secondary.port.DataBits = 8;
                                    }
                                    break;

                                case BaudRateName:

                                    try
                                    {
                                        Secondary.port.BaudRate = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        Secondary.port.BaudRate = 38400;
                                    }
                                    break;

                                case ReadBufferSizeName:

                                    try
                                    {
                                        Secondary.port.ReadBufferSize = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        Secondary.port.ReadBufferSize = 4096;
                                    }
                                    break;

                                case WriteBufferSizeName:

                                    try
                                    {
                                        Secondary.port.WriteBufferSize = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        Secondary.port.WriteBufferSize = 4096;
                                    }
                                    break;

                                case ReadTimeoutName:

                                    try
                                    {
                                        Secondary.port.ReadTimeout = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        Secondary.port.ReadTimeout = 30;
                                    }
                                    break;

                                case WriteTimeoutName:

                                    try
                                    {
                                        Secondary.port.WriteTimeout = int.Parse(element.InnerText);
                                    }
                                    catch
                                    {
                                        Secondary.port.WriteTimeout = 100;
                                    }
                                    break;

                                case ParityName:

                                    try
                                    {
                                        Secondary.port.Parity = (Parity)Enum.Parse(typeof(Parity), element.InnerText);
                                    }
                                    catch
                                    {
                                        Secondary.port.Parity = Parity.None;
                                    }
                                    break;

                                case StopBitsName:

                                    try
                                    {
                                        Secondary.port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), element.InnerText);
                                    }
                                    catch
                                    {
                                        Secondary.port.StopBits = StopBits.One;
                                    }
                                    break;

                                case portNameName:

                                    try
                                    {
                                        Secondary.port.PortName = element.InnerText;
                                    }
                                    catch
                                    {
                                        Secondary.port.PortName = string.Empty;
                                    }
                                    break;

                                case "is_use":

                                    try
                                    {
                                        Secondary.IsActived = Boolean.Parse(element.InnerText);
                                    }
                                    catch { }
                                    break;

                                default:

                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Serial Load: " + ex.Message, ErrorType.NotFatal));
                }
            }
        }

        /// <summary>
        /// Сохранить команды опроса датчиков
        /// </summary>
        /// <param name="document">Документ в который осущесвляется запись параметров</param>
        /// <param name="root">Узел в который сохранить команды опроса</param>
        protected void SavePackets(XmlDocument document, XmlNode root)
        {
            if (root != null)
            {
                Packet[] packets = StaticPackets;
                if (packets != null)
                {
                    foreach (Packet packet in packets)
                    {
                        XmlNode packetNode = document.CreateElement(staticCommandaName);

                        XmlNode comNode = document.CreateElement(com_packetName);
                        XmlNode tcpNode = document.CreateElement(tcp_packetName);

                        XmlNode waitNode = document.CreateElement(waitName);
                        XmlNode activedNode = document.CreateElement(activedName);

                        XmlNode contentNode = document.CreateElement(contentName);
                        XmlNode roleNode = document.CreateElement(roleName);

                        XmlNode packetSourceNode = document.CreateElement(packetSourceName);
                        XmlNode packetPortNode = document.CreateElement("port_type");

                        XmlNode packetInterval = document.CreateElement("interval");

                        if (packet.Com_Packet != null)
                        {
                            string str = string.Empty;
                            foreach (var item in packet.Com_Packet)
                            {
                                str += string.Format("{0:x2}", item);
                            }

                            comNode.InnerText = str;
                        }

                        tcpNode.InnerText = packet.Tcp_Packet;
                        waitNode.InnerText = packet.Wait.ToString();

                        activedNode.InnerText = packet.IsActived.ToString();
                        contentNode.InnerText = packet.Content.ToString();

                        roleNode.InnerText = packet.Role.ToString();
                        packetSourceNode.InnerText = packet.Source.ToString();

                        packetPortNode.InnerText = packet.PortType.ToString();
                        packetInterval.InnerText = packet.Interval.ToString();

                        packetNode.AppendChild(comNode);
                        packetNode.AppendChild(tcpNode);

                        packetNode.AppendChild(waitNode);
                        packetNode.AppendChild(activedNode);

                        packetNode.AppendChild(contentNode);
                        packetNode.AppendChild(roleNode);

                        packetNode.AppendChild(packetSourceNode);

                        packetNode.AppendChild(packetPortNode);
                        packetNode.AppendChild(packetInterval);
                        
                        root.AppendChild(packetNode);
                    }
                }
            }
        }

        /// <summary>
        /// Загрузить команду опроса
        /// </summary>
        /// <param name="root">Узел в котором находятся параметры для загрузки</param>
        protected void LoadPacket(XmlNode root)
        {
            if (root != null)
            {
                if (root.Name == staticCommandaName)
                {
                    XmlNodeList childs = root.ChildNodes;
                    if (childs != null)
                    {
                        Packet packet = new Packet();
                        foreach (XmlNode child in childs)
                        {
                            switch (child.Name)
                            {
                                case com_packetName:

                                    packet.Com_Packet = GetComByte(child.InnerText);
                                    break;

                                case tcp_packetName:

                                    packet.Tcp_Packet = child.InnerText;
                                    break;

                                case waitName:

                                    try
                                    {
                                        packet.Wait = Boolean.Parse(child.InnerText);
                                    }
                                    catch
                                    {
                                        packet.Wait = true;
                                    }
                                    break;

                                case activedName:

                                    try
                                    {
                                        packet.IsActived = Boolean.Parse(child.InnerText);
                                    }
                                    catch
                                    {
                                        packet.IsActived = true;
                                    }
                                    break;

                                case contentName:

                                    try
                                    {
                                        packet.Content = (Content)Enum.Parse(typeof(Content), child.InnerText);
                                    }
                                    catch
                                    {
                                        packet.Content = Content.Default;
                                    }
                                    break;

                                case roleName:

                                    try
                                    {
                                        packet.Role = (Role)Enum.Parse(typeof(Role), child.InnerText);
                                    }
                                    catch
                                    {
                                        packet.Role = Role.Master;
                                    }
                                    break;

                                case packetSourceName:

                                    try
                                    {
                                        packet.Source = (PacketSource)Enum.Parse(typeof(PacketSource), child.InnerText);
                                    }
                                    catch
                                    {
                                        packet.Source = PacketSource.Static;
                                    }
                                    break;

                                case "port_type":

                                    try
                                    {
                                        packet.PortType = (TypePort)Enum.Parse(typeof(TypePort), child.InnerText);
                                    }
                                    catch { }
                                    break;

                                case "interval":

                                    try
                                    {
                                        packet.Interval = TimeSpan.Parse(child.InnerText);
                                    }
                                    catch { }
                                    break;
                            }
                        }

                        InsertPacketToStatic(packet);
                    }
                }
            }
        }

        /// <summary>
        /// Выделить из строки пакет для отправки порт
        /// </summary>
        /// <param name="com_packet">Строка из которой выделить пакет</param>
        /// <returns>Выделенный пакет или null в противном случае</returns>
        protected byte[] GetComByte(string com_packet)
        {
            try
            {
                int ost = com_packet.Length % 2;
                if (ost == 0)
                {
                    byte[] pack = new byte[com_packet.Length / 2];
                    for (int i = 0; i < pack.Length; i++)
                    {
                        pack[i] = byte.Parse(com_packet.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
                    }

                    return pack;
                }
            }
            catch
            {
            }
            return null;
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
                            if (node.Name == serialRootName)
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
                    OnError(this, new ErrorArgs("Serial FindNode: " + ex.Message, ErrorType.NotFatal));
                }
            }

            return null;
        }

        /// <summary>
        /// Найти узел в котром содержатся данные настроек подсистемы
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        protected XmlNode FindNodeSecondary(XmlNode root)
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
                            if (node.Name == "Secondary")
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
                    OnError(this, new ErrorArgs("SerialSecondary FindNode: " + ex.Message, ErrorType.NotFatal));
                }
            }

            return null;
        }
    }
}