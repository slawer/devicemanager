using System;
using System.Xml;
using System.Threading;
using System.Runtime.Serialization;

namespace DeviceManager
{
    /// <summary>
    /// Реализует параметр, которым оперируют подсистемы программы
    /// </summary>
    [Serializable]
    public class Parameter : ISerializable
    {
        /// <summary>
        /// узел в который сохраняется параметр
        /// </summary>
        [NonSerialized]
        protected const string rootName = "parameter";

        /// <summary>
        /// узел в котором сохраняется позиция параметра
        /// </summary>
        [NonSerialized]
        protected const string positionName = "position";

        /// <summary>
        /// узел в котором сохраняется тип параметра
        /// </summary>
        [NonSerialized]
        protected const string typeName = "type";

        /// <summary>
        /// узел в котором сохраняется номер устройства
        /// </summary>
        [NonSerialized]
        protected const string deviceName = "device";

        /// <summary>
        /// узел в котором сохраняется смещение в пакета
        /// </summary>
        [NonSerialized]
        protected const string offsetName = "offset";

        /// <summary>
        /// узел в который сохраняется размер данных
        /// </summary>
        [NonSerialized]
        protected const string sizeName = "size";

        /// <summary>
        /// узел в котором сохраняется описание параметра
        /// </summary>
        [NonSerialized]
        protected const string descName = "desc";

        /// <summary>
        /// узел в котором сохраняется идентификатор параметра
        /// </summary>
        [NonSerialized]
        protected const string identifierName = "identifier";

        /// <summary>
        /// Имя сериализуемой переменной показывающей номер устройства
        /// </summary>
        [NonSerialized]
        protected const string deviceSName = "deviceSName";

        /// <summary>
        /// Имя сериализуемой переменной показывающей смещение в пакете
        /// </summary>
        [NonSerialized]
        protected const string offsetSName = "offsetSName";

        /// <summary>
        /// Имя сериализуемой переменной показывающей размер данных
        /// </summary>
        [NonSerialized]
        protected const string sizeSName = "sizeSName";
        
        /// <summary>
        /// Имя сериализуемой переменной показывающей позиция в которой находится данный параметр
        /// </summary>
        [NonSerialized]
        protected const string positionSName = "positionSName";

        /// <summary>
        /// Имя сериализуемой переменной показывающей тип параметра
        /// </summary>
        [NonSerialized]
        protected const string typeSName = "typeSName";

        /// <summary>
        /// Имя сериализуемой переменной показывающей описание параметра
        /// </summary>
        [NonSerialized]
        protected const string descSName = "descSName";

        /// <summary>
        /// Имя сериализуемой переменной показывающей идентификатор параметра
        /// </summary>
        [NonSerialized]
        protected const string identifierSName = "identifierSName";

        /// <summary>
        /// Имя сериализуемой переменной показывающей параметр актуален или нет
        /// </summary>
        [NonSerialized]
        protected const string actualSName = "actualSName";

        protected long device;               // номер устройства
        protected long offset;               // смещение в пакете

        protected long size;                 // размер данных
        protected long position;             // позиция в которой находится данный параметр

        protected ParameterType type;       // тип параметра
       
        [NonSerialized]
        protected ReaderWriterLock locker;  // синхронизирует доступ к параметрам класса

        protected string desc;              // описание параметра
        protected Guid identifier;          // идентификатор параметра

        protected long actual;              // показывает параметр актуален или нет

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Parameter()
        {
            locker = new ReaderWriterLock();
            type = ParameterType.Default;

            actual = 1;
            identifier = Guid.NewGuid();
        }

        /// <summary>
        /// Конструктор вызываемый при десериализации объекта
        /// </summary>
        internal Parameter(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                locker = new ReaderWriterLock();

                device = info.GetInt64(deviceSName);
                offset = info.GetInt64(offsetSName);

                size = info.GetInt64(sizeSName);
                position = info.GetInt64(positionSName);

                type = (ParameterType)Enum.Parse(typeof(ParameterType), info.GetString(typeSName));
                desc = info.GetString(descSName);

                identifier = (Guid)info.GetValue(identifierSName, typeof(Guid));
                actual = info.GetInt64(actualSName);

            }
            else
                throw new ArgumentNullException("info");
        }

        /// <summary>
        /// Заполняет объект System.Runtime.Serialization.SerializationInfo данными,
        /// необходимыми для сериализации целевого объекта.
        /// </summary>
        /// <param name="info"> Объект System.Runtime.Serialization.SerializationInfo для заполнения данными</param>
        /// <param name="context">Целевое местоположение сериализации</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                info.AddValue(deviceSName, Device);
                info.AddValue(offsetSName, Offset);

                info.AddValue(sizeSName, Size);
                info.AddValue(positionSName, Position);

                info.AddValue(typeSName, Type.ToString());
                info.AddValue(descSName, Description);

                info.AddValue(identifierSName, Identifier);
                info.AddValue(actualSName, IsActual);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Определяет позицию в которой находится параметр
        /// </summary>
        public int Position
        {
            get { return (int)Interlocked.Read(ref position); }
            set { Interlocked.Exchange(ref position, value); }
        }

        /// <summary>
        /// Определяет тип параметра
        /// </summary>
        public ParameterType Type
        {
            get
            {
                try
                {
                    locker.AcquireReaderLock(100);
                    try
                    {
                        return type;
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
                    }
                }
                catch { }
                return ParameterType.Default;
            }

            set
            {
                try
                {
                    locker.AcquireWriterLock(100);
                    try
                    {
                        type = value;
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Определяет номер устройства
        /// </summary>
        public int Device
        {
            get { return (int)Interlocked.Read(ref device); }
            set { Interlocked.Exchange(ref device, value); }
        }

        /// <summary>
        /// Определяет смещение в пакете
        /// </summary>
        public int Offset
        {
            get { return (int)Interlocked.Read(ref offset); }
            set { Interlocked.Exchange(ref offset, value); }
        }

        /// <summary>
        /// Определяет размер данных
        /// </summary>
        public int Size
        {
            get { return (int)Interlocked.Read(ref size); }
            set { Interlocked.Exchange(ref size, value); }
        }

        /// <summary>
        /// Определяет порядок поступающих байт
        /// </summary>
        public bool IsLittleEndian
        {
            get { return BitConverter.IsLittleEndian; }
        }

        /// <summary>
        /// Определяет описание параметра
        /// </summary>
        public string Description
        {
            get
            {
                try
                {
                    locker.AcquireReaderLock(100);
                    try
                    {
                        return desc;
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
                    }
                }
                catch { }
                return string.Empty;
            }

            set
            {
                try
                {
                    locker.AcquireWriterLock(100);
                    try
                    {
                        desc = value;
                    }
                    finally
                    {
                        locker.ReleaseWriterLock();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Возвращяет идентификатор параметра
        /// </summary>
        public Guid Identifier
        {
            get { return identifier; }
        }

        /// <summary>
        /// Определяет параметр актуален или нет
        /// </summary>
        public bool IsActual
        {
            get { return (Interlocked.Read(ref actual) > 0); }
            set
            {
                if (value)
                {
                    Interlocked.Exchange(ref actual, 1);
                }
                else
                    Interlocked.Exchange(ref actual, 0);
            }
        }

        /// <summary>
        /// Сохранить параметр в узел XML
        /// </summary>
        /// <param name="document">Документ в контексте которого осуществляется работа</param>
        /// <returns>Сохраненный параметр или null</returns>
        public XmlNode CreateXmlNode(XmlDocument document)
        {
            try
            {
                XmlNode root = document.CreateElement(rootName);

                XmlNode positionNode = document.CreateElement(positionName);
                XmlNode typeNode = document.CreateElement(typeName);

                XmlNode deviceNode = document.CreateElement(deviceName);
                XmlNode offsetNode = document.CreateElement(offsetName);

                XmlNode sizeNode = document.CreateElement(sizeName);
                XmlNode descNode = document.CreateElement(descName);

                XmlNode identifierNode = document.CreateElement(identifierName);

                positionNode.InnerText = position.ToString();
                typeNode.InnerText = type.ToString();

                deviceNode.InnerText = device.ToString();
                offsetNode.InnerText = offset.ToString();

                sizeNode.InnerText = size.ToString();
                descNode.InnerText = desc;

                root.AppendChild(positionNode);
                root.AppendChild(typeNode);

                root.AppendChild(deviceNode);
                root.AppendChild(offsetNode);

                root.AppendChild(sizeNode);
                root.AppendChild(descNode);

                root.AppendChild(identifierNode);
                return root;

            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Инициализировать параметр из Xml узла
        /// </summary>
        /// <param name="node">Узел в котором содержаться настройки параметра</param>
        public void InstanceFromXml(XmlNode node)
        {
            try
            {
                if (node != null)
                {
                    if (node.Name == rootName)
                    {
                        if (node.HasChildNodes)
                        {
                            foreach (XmlNode child in node.ChildNodes)
                            {
                                switch (child.Name)
                                {
                                    case positionName:

                                        try
                                        {
                                            position = int.Parse(child.InnerText);
                                        }
                                        catch
                                        {
                                        }
                                        break;

                                    case typeName:

                                        try
                                        {
                                            type = (ParameterType)Enum.Parse(typeof(ParameterType), child.InnerText);
                                        }
                                        catch
                                        {
                                        }
                                        break;

                                    case offsetName:

                                        try
                                        {
                                            offset = long.Parse(child.InnerText);
                                        }
                                        catch
                                        {
                                        }
                                        break;

                                    case sizeName:

                                        try
                                        {
                                            size = long.Parse(child.InnerText);
                                        }
                                        catch
                                        {
                                        }
                                        break;

                                    case descName:

                                        try
                                        {
                                            desc = child.InnerText;
                                        }
                                        catch
                                        {
                                        }
                                        break;

                                    case identifierName:

                                        try
                                        {
                                            identifier = new Guid(child.InnerText);
                                        }
                                        catch { }
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
    }
}