using System;
using System.Xml;
using System.Threading;

namespace DeviceManager
{
    /// <summary>
    /// Реализует выполнение сценария
    /// </summary>
    public partial class Script : IMacros
    {
        protected float current = 0.0f;                 // результат работы функции

        protected string description;                   // описание формулы
        protected ReaderWriterLockSlim _slim = null;    // синхронизатор

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Script()
        {
            script = null;
            createdScript = false;

            assembly = new System.Collections.Generic.List<string>();
            _slim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        /// <summary>
        /// Возвращяет тип формулы
        /// </summary>
        public FormulaType Type
        {
            get { return FormulaType.Script; }
        }

        /// <summary>
        /// Определяет список параметров макроса
        /// </summary>
        public Argument[] Args
        {
            get { return null; }
        }

        /// <summary>
        /// Определяет значение макроса
        /// </summary>
        public float Value
        {
            get { return current; }
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
            if (_slim.TryEnterReadLock(100))
            {
                try
                {
                    if (createdScript == false)
                    {
                        if (script == null) script = InstanceScript(_script, _namespace, _classname, assembly.ToArray());
                        if (script != null)
                        {
                            current = script.Run(signals, results);
                            return current;
                        }

                        createdScript = true;
                    }
                }
                finally
                {
                    _slim.ExitReadLock();
                }
            }
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
            get { return string.Empty; }
        }

        /// <summary>
        /// Текстовое описание формулы (сложение, вычитание и т.п..)
        /// </summary>
        public string Name
        {
            get
            {
                return "Сценарий";
            }
        }

        // -------------------- сохранение / загрузка --------------------

        /// <summary>
        /// имя корневого узла настроек макроса
        /// </summary>
        protected const string rootName = "macros";

        /// <summary>
        /// имя узла в котором сохраняется описание макроса
        /// </summary>
        protected const string descriptionName = "desc";

        /// <summary>
        /// Получить XmlNode макроса для сохранения начтроек конвертора
        /// </summary>
        /// <param name="document">Документ в который осуществляется сохранение настроек</param>
        /// <returns>XmlNode макроса</returns>
        public XmlNode CreateXmlNode(XmlDocument document)
        {
            try
            {
                if (_slim.TryEnterReadLock(100))
                {
                    try
                    {
                        XmlNode root = document.CreateElement(rootName);
                        XmlNode descNode = document.CreateElement(descriptionName);

                        XmlNode _scriptNode = document.CreateElement("_script");
                        XmlNode _namespaceNode = document.CreateElement("_namespace");

                        XmlNode _classnameNode = document.CreateElement("_classname");
                        XmlNode assemblyNode = document.CreateElement("assembly");

                        descNode.InnerText = description;

                        _scriptNode.InnerText = _script;
                        _namespaceNode.InnerText = _namespace;

                        _classnameNode.InnerText = _classname;
                        foreach (String item in assembly)
                        {
                            XmlNode itemNode = document.CreateElement("assemlyItem");
                            if (itemNode != null)
                            {
                                itemNode.InnerText = item;
                                assemblyNode.AppendChild(itemNode);
                            }
                        }

                        root.AppendChild(descNode);
                        root.AppendChild(_scriptNode);
                        root.AppendChild(_namespaceNode);
                        root.AppendChild(_classnameNode);
                        root.AppendChild(assemblyNode);
                        
                        return root;
                    }
                    finally
                    {
                        _slim.ExitReadLock();
                    }
                }
                return null;
            }
            catch { }
            return null;
        }


        /// <summary>
        /// Инициализировать формулу из сохраненного раннее узла Xml
        /// </summary>
        /// <param name="node">Узел на основе которого выполнить инициализацию макроса</param>
        public void InstanceMacrosFromXmlNode(XmlNode node)
        {
            if (_slim.TryEnterWriteLock(300))
            {
                try
                {
                    if (node != null)
                    {
                        if (node.HasChildNodes)
                        {
                            foreach (XmlNode child in node.ChildNodes)
                            {
                                switch (child.Name)
                                {
                                    case descriptionName:

                                        try
                                        {
                                            description = child.InnerText;
                                        }
                                        catch { }
                                        break;

                                    case "_script":

                                        try
                                        {
                                            _script = child.InnerText;
                                        }
                                        catch { }
                                        break;

                                    case "_namespace":

                                        try
                                        {
                                            _namespace = child.InnerText;
                                        }
                                        catch { }
                                        break;

                                    case "_classname":

                                        try
                                        {
                                            _classname = child.InnerText;
                                        }
                                        catch { }
                                        break;

                                    case "assembly":

                                        try
                                        {
                                            LoadAssemblies(child);
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
                finally
                {
                    _slim.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Загрузить сборки из узла
        /// </summary>
        /// <param name="root">Узел в котором находятся сборки</param>
        protected void LoadAssemblies(XmlNode root)
        {
            if (root != null && root.HasChildNodes)
            {
                foreach (XmlNode child in root.ChildNodes)
                {
                    if (child.Name == "assemlyItem")
                    {
                        assembly.Add(child.InnerText);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Определяет интерфейс класса выполняющего сценарий
    /// </summary>
    public interface IScript
    {
        float Run(Float[] signals, Float[] results);
    }
}