using System;
using System.Xml;
using System.Threading;

namespace DeviceManager
{
    /// <summary>
    /// Суммирование (сумма всех значений параметра)
    /// </summary>
    public class Accumulation : IMacros
    {
        /// <summary>
        /// имя корневого узла настроек макроса
        /// </summary>
        protected const string rootName = "macros";

        /// <summary>
        /// имя узла в котором хранится позиция канала из которого брать значение
        /// </summary>
        protected const string indexName = "index";

        /// <summary>
        /// имя узла в котором сохраняется описание макроса
        /// </summary>
        protected const string descriptionName = "desc";

        /// <summary>
        /// имя узла в который сериализуется аргумент
        /// </summary>
        protected const string argumentName = "argument";

        protected float current = 0.0f;                 // результат работы функции
        protected Argument[] arguments = null;          // параметры функции

        protected ReaderWriterLock locker = null;       // синхронизатор
        protected string description;                   // описание формулы

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Accumulation()
        {
            locker = new ReaderWriterLock();

            arguments = new Argument[2];
            for (int index = 0; index < 2; index++)
            {
                arguments[index] = new Argument();
            }
        }

        /// <summary>
        /// Возвращяет тип формулы
        /// </summary>
        public FormulaType Type 
        {
            get { return FormulaType.Accumulation; }
        }

        /// <summary>
        /// Определяет список параметров макроса
        /// </summary>
        public Argument[] Args 
        {
            get { return arguments; }
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
                        float x = (arguments[0].Source == DataSource.Signals) ? GetValue(arguments[0].Index, signals) : GetValue(arguments[0].Index, results);
                        try
                        {
                            if (float.IsNaN(x) == false)
                            {
                                current += x;
                            }
                            else
                                current = 0.0f;
                        }
                        catch
                        {
                            current = 0.0f;
                        }

                        return current;
                    }
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch { }
            return float.NaN;
        }

        /// <summary>
        /// Сбросить состояние макроcа в начальное состояние (по умолчанию)
        /// </summary>
        public void Reset()
        {
            try
            {
                locker.AcquireWriterLock(100);
                try
                {
                    current = 0.0f;
                }
                finally
                {
                    locker.ReleaseWriterLock();
                }
            }
            catch { }
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
                    return string.Format("Суммирование значений(Пар.№{0})", arguments[0].Index);
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
            get { return "Суммирование (сумма всех значений параметра)"; }
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

                XmlNode arg_1 = arguments[0].CreateNode(document);
                XmlNode arg_2 = arguments[1].CreateNode(document);

                XmlNode descNode = document.CreateElement(descriptionName);

                descNode.InnerText = description;

                root.AppendChild(arg_1);
                root.AppendChild(arg_2);

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
                if (arguments != null)
                {
                    if (arguments[0] != null && arguments[1] != null)
                    {
                        XmlNode[] argsNode = FindArguments(node);
                        if (argsNode != null)
                        {
                            if (argsNode[0] != null && argsNode[1] != null)
                            {
                                arguments[0].InstanceFromXml(argsNode[0]);
                                arguments[1].InstanceFromXml(argsNode[1]);

                                InitDescription(node);
                            }
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
        protected XmlNode[] FindArguments(XmlNode node)
        {
            try
            {
                if (node != null)
                {
                    if (node.HasChildNodes)
                    {
                        int index = 0;
                        XmlNode[] args = new XmlNode[2];

                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Name == argumentName)
                            {
                                args[index++] = child;
                                if (index >= 2)
                                {
                                    return args;
                                }
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