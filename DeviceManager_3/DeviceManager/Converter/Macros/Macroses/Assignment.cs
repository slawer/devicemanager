using System;
using System.Xml;
using System.Threading;

namespace DeviceManager
{
    /// <summary>
    /// Реализует операцию присваивания
    /// </summary>
    public class Assignment : IMacros
    {
        /// <summary>
        /// имя корневого узла настроек макроса
        /// </summary>
        protected const string rootName = "macros";

        /// <summary>
        /// имя узла в котором хранится позиция канала из которого брать значение
        /// </summary>
        protected const string arg1_indexName = "index";

        /// <summary>
        /// имя узла в котором сохраняется описание макроса
        /// </summary>
        protected const string descriptionName = "desc";

        /// <summary>
        /// имя узла в который сериализуется аргумент
        /// </summary>
        protected const string argumentName = "argument";

        protected ReaderWriterLock locker = null;       // синхронизатор
        protected Argument[] argument = null;           // аргумент функции

        protected string description;                   // описание параметра
        protected float v = float.NaN;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Assignment()
        {
            locker = new ReaderWriterLock();
            argument = new Argument[2];

            for (int i = 0; i < 2; i++)
            {
                argument[i] = new Argument();
            }
        }

        /// <summary>
        /// Возвращяет тип формулы
        /// </summary>
        public FormulaType Type 
        { 
            get { return FormulaType.Assignment; } 
        }

        /// <summary>
        /// Определяет список параметров макроса
        /// </summary>
        public Argument[] Args
        {
            get { return argument; }
        }

        /// <summary>
        /// Определяет значение макроса
        /// </summary>
        public float Value 
        {
            get { return float.NaN; }
            set { }
        }

        /// <summary>
        /// Вычислить параметр 
        /// </summary>
        /// <param name="signals">Значения поступившие с датчиков</param>
        /// <param name="results">Значения являющиеся конечными данными</param>
        /// <returns>Вычисленное значение</returns>
        public float Calculate(Float[] signals, Float[] results)
        {
            try
            {
                locker.AcquireReaderLock(100);
                try
                {
                    if (signals != null && results != null)
                    {
                        Argument arg = argument[0];
                        if (arg != null)
                        {
                            switch (arg.Source)
                            {
                                case DataSource.Signals:

                                    return GetValue(arg.Index, signals);

                                case DataSource.Results:

                                    return GetValue(arg.Index, results);

                                default:

                                    return float.NaN;
                            }
                        }
                    }
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch
            {                
            }

            return float.NaN;
        }

        /// <summary>
        /// Сбросить состояние макроcа с начальное состояние (по умолчанию)
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// Получить значени из списка
        /// </summary>
        /// <param name="index">Позиция значения в списке</param>
        /// <param name="values">Список значений</param>
        /// <returns>Значение из списка</returns>
        private float GetValue(int index, Float[] values)
        {
            if (index > -1 && index < values.Length)
            {
                return values[index].Value;
            }
            else
                return float.NaN;
        }

        /// <summary>
        /// Описание формулы
        /// </summary>
        public string Description 
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Описание аргументов формулы
        /// </summary>
        public string Arguments 
        {
            get
            {
                try
                {
                    return string.Format("Присваивание(Канал №{0})", argument[0].Index);
                }
                catch
                {
                    return "Описание аргументов недоступно!";
                }
            }
        }

        /// <summary>
        /// Текстовое описание формулы (сложение, вычитание и т.п..)
        /// </summary>
        public string Name 
        {
            get { return "Присваивание канала"; }
        }

        /// <summary>
        /// Получить XmlNode макроса для сохранения наcтроек конвертора
        /// </summary>
        /// <param name="document">Документ в который осуществляется сохранение настроек</param>
        /// <returns>XmlNode макроса</returns>
        public XmlNode CreateXmlNode(XmlDocument document)
        {
            try
            {
                XmlNode root = document.CreateElement(rootName);

                XmlNode arg_1 = argument[0].CreateNode(document);
                XmlNode descNode = document.CreateElement(descriptionName);

                descNode.InnerText = description;

                root.AppendChild(arg_1);

                root.AppendChild(descNode);
                return root;
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// Инициализировать формулу из сохраненного раннее узла Xml
        /// </summary>
        /// <param name="node">Узел на основе которого выполнить инициализацию макроса</param>
        public void InstanceMacrosFromXmlNode(XmlNode node)
        {
            try
            {
                if (argument != null)
                {
                    if (argument[0] != null)
                    {
                        XmlNode argNode = FindArgument(node);
                        if (argNode != null)
                        {
                            argument[0].InstanceFromXml(argNode);
                            InitDescription(node);
                        }
                    }
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Найти аргумент функции
        /// </summary>
        /// <param name="node">Узел в котором искать аргумент</param>
        /// <returns>Узел, содержащий настройки аргумента</returns>
        protected XmlNode FindArgument(XmlNode node)
        {
            try
            {
                if (node != null)
                {
                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Name == argumentName)
                            {
                                return child;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Извлечь описание формулы
        /// </summary>
        /// <param name="node">Узел из которого извлечь описание формулы</param>
        protected void InitDescription(XmlNode node)
        {
            try
            {
                if (node != null)
                {
                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Name == descriptionName)
                            {
                                description = child.InnerText;
                                return;
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