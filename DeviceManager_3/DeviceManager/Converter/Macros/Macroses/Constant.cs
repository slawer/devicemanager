﻿using System;
using System.Xml;
using System.Threading;

namespace DeviceManager
{
    /// <summary>
    /// Реализует константу
    /// </summary>
    public class Constant : IMacros
    {
        /// <summary>
        /// имя корневого узла настроек макроса
        /// </summary>
        protected const string rootName = "macros";

        /// <summary>
        /// имя узла в котороый сохраняются настройки первого аргумента
        /// </summary>
        protected const string agrument_1Name = "argument1";

        /// <summary>
        /// имя узла в котороый сохраняются настройки второго аргумента
        /// </summary>
        protected const string agrument_2Name = "argument2";

        /// <summary>
        /// имя узла в котором хранится значение константы
        /// </summary>
        protected const string constantName = "value";

        /// <summary>
        /// имя узла в котором сохраняется описание макроса
        /// </summary>
        protected const string descriptionName = "desc";


        protected ReaderWriterLock locker = null;   // синхронизатор
        protected float constant = float.NaN;       // текущее значение константы

        protected string description;               // описание формулы
        
        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public Constant()
        {
            locker = new ReaderWriterLock();
        }

        /// <summary>
        /// Возвращяет тип формулы
        /// </summary>
        public FormulaType Type { get { return FormulaType.Constant; } }

        /// <summary>
        /// Определяет список параметров макроса
        /// </summary>
        public Argument[] Args { get { return null; } }

        /// <summary>
        /// Значение константы
        /// </summary>
        public float Value
        {
            get
            {
                try
                {
                    locker.AcquireReaderLock(100);
                    try
                    {
                        return constant;
                    }
                    finally
                    {
                        locker.ReleaseReaderLock();
                    }
                }
                catch
                {
                    return float.NaN;
                }
            }

            set
            {
                try
                {
                    locker.AcquireWriterLock(100);
                    try
                    {
                        constant = value;
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
                    return constant;
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch
            {
                return float.NaN;
            }
        }

        /// <summary>
        /// Сбросить состояние макроcа с начальное состояние (по умолчанию)
        /// </summary>
        public void Reset()
        {
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
                return string.Format("{0}", Calculate(null, null));
            }
        }

        /// <summary>
        /// Текстовое описание формулы (сложение, вычитание и т.п..)
        /// </summary>
        public string Name 
        {
            get { return "Константа"; }
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

                XmlNode arg_1 = document.CreateElement(agrument_1Name);
                XmlNode arg_2 = document.CreateElement(agrument_2Name);

                XmlNode val = document.CreateElement(constantName);
                XmlNode descNode = document.CreateElement(descriptionName);

                val.InnerText = constant.ToString();
                descNode.InnerText = description;

                root.AppendChild(arg_1);
                root.AppendChild(arg_2);

                root.AppendChild(val);
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
                if (node.Name == rootName)
                {
                    XmlNodeList childs = node.ChildNodes;
                    if (childs != null)
                    {
                        foreach (XmlNode child in childs)
                        {
                            switch (child.Name)
                            {
                                case constantName:

                                    try
                                    {
                                        constant = Application.ParseSingle(child.InnerText);
                                    }
                                    catch
                                    {
                                    }
                                    break;

                                case descriptionName:

                                    description = child.InnerText;
                                    break;
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
    }
}