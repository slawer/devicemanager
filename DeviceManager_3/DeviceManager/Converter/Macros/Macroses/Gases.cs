using System;
using System.Xml;
using System.Threading;
using System.Collections.Generic;

namespace DeviceManager
{
    /// <summary>
    /// Реализует формулу вычисления Газы (сделано для СГТ)
    /// </summary>
    public class Gases : IMacros
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

        protected Single current_value;                 // текущее значение 
        protected Argument[] arguments = null;          // параметры функции

        protected string description;                   // описание формулы
        protected List<ArgumentPair> args;              // реальные аргуметы формулы

        protected ReaderWriterLockSlim slim = null;     // синхронизатор

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Gases()
        {
            slim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            arguments = new Argument[2];
            for (int index = 0; index < 2; index++)
            {
                arguments[index] = new Argument();
            }

            args = new List<ArgumentPair>();
        }

        /// <summary>
        /// Возвращяет тип формулы
        /// </summary>
        public FormulaType Type
        {
            get
            {
                return FormulaType.Gases;
            }
        }

        /// <summary>
        /// Определяет список параметров макроса
        /// </summary>
        public Argument[] Args 
        {
            get
            {
                return arguments;
            }
        }

        /// <summary>
        /// Реальные аргументы формулы
        /// </summary>
        public ArgumentPair[] RealArguments
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return args.ToArray();
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Определяет значение макроса
        /// </summary>
        public float Value 
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return current_value;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return float.NaN;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        current_value = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Вычислить параметр 
        /// </summary>
        /// <param name="signals">Значения поступившие с датчиков</param>
        /// <param name="results">Значения являющиеся конечными данными</param>
        /// <returns>Вычисленное значение</returns>
        public float Calculate(Float[] signals, Float[] results)
        {
            if (slim.TryEnterWriteLock(300))
            {
                try
                {
                    if (args != null)
                    {
                        current_value = 0.0f;
                        foreach (ArgumentPair pair in args)
                        {
                            if (pair != null)
                            {
                                float f_val = float.NaN;
                                float s_val = float.NaN;

                                if (pair.First.Index > -1 && pair.First.Index < results.Length)
                                {
                                    f_val = results[pair.First.Index].Value;
                                }

                                if (pair.Second.Index > -1 && pair.Second.Index < results.Length)
                                {
                                    s_val = results[pair.Second.Index].Value;
                                }

                                if (float.IsNaN(f_val) == false && float.IsNaN(s_val) == false)
                                {
                                    if (f_val > s_val)
                                    {
                                        current_value = 1.0f;
                                        break;
                                    }
                                }
                            }
                        }                        
                    }
                }
                finally
                {
                    slim.ExitWriteLock();
                }

                return current_value;
            }

            return float.NaN;
        }

        /// <summary>
        /// Сбросить состояние макроcа в начальное состояние (по умолчанию)
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// Добавить аргумент
        /// </summary>
        /// <param name="argument">Добавляемый аргумент</param>
        public void InsertArgument(ArgumentPair argument)
        {
            if (slim.TryEnterWriteLock(300))
            {
                try
                {
                    args.Add(argument);
                }
                finally
                {
                    slim.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Удалить аргмуент
        /// </summary>
        /// <param name="argument">Удаляемый аргумент</param>
        public void RemoveArgument(ArgumentPair argument)
        {
            if (slim.TryEnterWriteLock(300))
            {
                try
                {
                    args.Remove(argument);
                }
                finally
                {
                    slim.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Очистить список аргументов
        /// </summary>
        public void Clear()
        {
            if (slim.TryEnterWriteLock(300))
            {
                try
                {
                    args.Clear();
                }
                finally
                {
                    slim.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Описание формулы
        /// </summary>
        public string Description 
        {
            get
            {
                if (slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return description;
                    }
                    finally
                    {
                        slim.ExitReadLock();
                    }
                }

                return string.Empty;
            }

            set
            {
                if (slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        description = value;
                    }
                    finally
                    {
                        slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Описание аргументов формулы
        /// </summary>
        public string Arguments
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Текстовое описание формулы (сложение, вычитание и т.п..)
        /// </summary>
        public string Name
        {
            get
            {
                return "Реализует команду Газы для СГТ";
            }
        }

        /// <summary>
        /// Получить XmlNode макроса для сохранения начтроек конвертора
        /// </summary>
        /// <param name="document">Документ в который осуществляется сохранение настроек</param>
        /// <returns>XmlNode макроса</returns>
        public XmlNode CreateXmlNode(XmlDocument document)
        {
            if (slim.TryEnterReadLock(100))
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

                    foreach (ArgumentPair pair in args)
                    {
                        XmlNode p_node = pair.Save(document);
                        if (p_node != null)
                        {
                            root.AppendChild(p_node);
                        }
                    }

                    return root;
                }
                finally
                {
                    slim.ExitReadLock();
                }
            }
            return null;

        }

        /// <summary>
        /// Инициализировать формулу из сохраненного раннее узла Xml
        /// </summary>
        /// <param name="node">Узел на основе которого выполнить инициализацию макроса</param>
        public void InstanceMacrosFromXmlNode(XmlNode node)
        {
            if (slim.TryEnterWriteLock(300))
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

                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            switch (child.Name)
                            {
                                case "Pair":

                                    try
                                    {
                                        ArgumentPair pair = new ArgumentPair();
                                        pair.Load(child);

                                        args.Add(pair);
                                    }
                                    catch { }
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
                finally
                {
                    slim.ExitWriteLock();
                }
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
            catch { }
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
            catch { }
        }
    }

    /// <summary>
    /// Реализует пару аргуметов
    /// </summary>
    public class ArgumentPair
    {
        protected Argument first;       // первый аргумент
        protected Argument second;      // второй аргумент

        /// <summary>
        /// инициализирует новый экземпляр класса
        /// </summary>
        public ArgumentPair()
        {
            first = new Argument();
            second = new Argument();
        }

        /// <summary>
        /// Первый аргумент
        /// </summary>
        public Argument First
        {
            get { return first; }
        }

        /// <summary>
        /// Второй аргумент
        /// </summary>
        public Argument Second
        {
            get { return second; }
        }

        /// <summary>
        /// Сохранить пару
        /// </summary>
        /// <param name="doc">Документ в который осуществляется сохранение пары</param>
        /// <returns>Узел в котором сохранена пара</returns>
        public XmlNode Save(XmlDocument doc)
        {
            try
            {
                XmlNode root = doc.CreateElement("Pair");

                XmlNode f_node = first.CreateNode(doc);
                XmlNode s_node = second.CreateNode(doc);

                root.AppendChild(f_node);
                root.AppendChild(s_node);

                return root;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Загрузить пару
        /// </summary>
        /// <param name="root">Узел в котором сохранена пара</param>
        public void Load(XmlNode root)
        {
            try
            {
                if (root != null && root.Name == "Pair")
                {
                    if (root.HasChildNodes)
                    {
                        XmlNodeList childs = root.ChildNodes;
                        if (childs != null && childs.Count >= 2)
                        {
                            first.InstanceFromXml(childs[0]);
                            second.InstanceFromXml(childs[1]);
                        }
                    }
                }
            }
            catch { }
        }
    }
}