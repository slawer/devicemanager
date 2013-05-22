using System;
using System.Xml;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует вычисление параметров на основе формул
    /// </summary>
    public partial class Converter
    {
        /// <summary>
        /// Имя узла в котором сожержатся настройки конвертора
        /// </summary>
        protected const string rootName = "Converter_Config";

        /// <summary>
        /// имя узла в котором сохраняется формула
        /// </summary>
        protected const string formulaName = "formula";

        /// <summary>
        /// имя узла в котором сохрается поиция формулы
        /// </summary>
        protected const string positionName = "position";

        /// <summary>
        /// имя узла в котором сохраняется тип формулы
        /// </summary>
        protected const string formulaTypeName = "formulatype";

        /// <summary>
        /// имя узла в котором содержится макрос
        /// </summary>
        protected const string macrosName = "macros";

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="document">Документ в который выполнить сохранение настроек</param>
        public void Save(XmlDocument document)
        {
            Formula[] formuls = Formuls;
            if (formuls != null)
            {
                XmlNode root = document.CreateElement(rootName);
                foreach (Formula formula in formuls)
                {
                    XmlNode formulaNode = document.CreateElement(formulaName);

                    XmlNode positionNode = document.CreateElement(positionName);
                    XmlNode formulaTypeNode = document.CreateElement(formulaTypeName);

                    XmlNode macrosNode = formula.Macros.CreateXmlNode(document);
                    if (macrosNode != null)
                    {
                        positionNode.InnerText = formula.Position.ToString();
                        formulaTypeNode.InnerText = formula.Type.ToString();

                        formulaNode.AppendChild(positionNode);
                        formulaNode.AppendChild(formulaTypeNode);

                        formulaNode.AppendChild(macrosNode);
                        root.AppendChild(formulaNode);
                    }
                }

                document.DocumentElement.AppendChild(root);
            }
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="root">Узел в котором содержаться настройки конвертора</param>
        public void Load(XmlNode root) 
        {
            try
            {
                XmlNode node = FindNode(root);
                if (node != null)
                {
                    if (node.ChildNodes != null)
                    {
                        foreach (XmlNode formula in node.ChildNodes)
                        {
                            InsertFormula(formula);
                        }

                        SortFormuls();
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
        /// Добавить формулу
        /// </summary>
        /// <param name="formula">Узел в котором содержится информация о формуле</param>
        protected void InsertFormula(XmlNode formula)
        {
            try
            {
                FormulaType type = GetFormulaType(formula);
                switch (type)
                {
                    case FormulaType.Constant:

                        InsertConstant(formula);
                        break;

                    case FormulaType.Assignment:

                        InsertAssignment(formula);
                        break;

                    case FormulaType.Summa:

                        InsertSumma(formula);
                        break;

                    case FormulaType.Difference:

                        InsertDifference(formula);
                        break;

                    case FormulaType.Multiplication:

                        InsertMultiplication(formula);
                        break;

                    case FormulaType.Division:

                        InsertDivizion(formula);
                        break;

                    case FormulaType.Increment:

                        InsertIncrement(formula);
                        break;

                    case FormulaType.Maximum:

                        InsertMaximum(formula);
                        break;

                    case FormulaType.Minimum:

                        InsertMinimum(formula);
                        break;

                    case FormulaType.PowerOf10:

                        InsertPowerOf10(formula);
                        break;

                    case FormulaType.Accumulation:

                        InsertAccumulation(formula);
                        break;

                    case FormulaType.Media:

                        InsertMedia(formula);
                        break;

                    case FormulaType.Tranformation:

                        InsertTransformation(formula);
                        break;

                    case FormulaType.Capture:

                        InsertCapture(formula);
                        break;

                    case FormulaType.Gases:

                        InsertGases(formula);
                        break;

                    case FormulaType.Script:

                        InsertScript(formula);
                        break;

                    default:
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить константу
        /// </summary>
        /// <param name="formula">Узел содержащий формулу</param>
        protected void InsertConstant(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Constant constant = new Constant();

                frm.Position = GetPosition(formula);

                XmlNode macros = GetMacros(formula);
                if (macros != null)
                {
                    constant.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = constant;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить захват канала
        /// </summary>
        /// <param name="formula">Узел содержащий формулу</param>
        protected void InsertCapture(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Capture capture = new Capture();

                frm.Position = GetPosition(formula);

                XmlNode macros = GetMacros(formula);
                if (macros != null)
                {
                    capture.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = capture;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить команду газы
        /// </summary>
        /// <param name="formula">Узел содержащий формулу</param>
        protected void InsertGases(XmlNode formula)
        {
            Formula frm = new Formula();
            Gases capture = new Gases();

            frm.Position = GetPosition(formula);

            XmlNode macros = GetMacros(formula);
            if (macros != null)
            {
                capture.InstanceMacrosFromXmlNode(macros);

                frm.Macros = capture;
                InsertFormula(frm);
            }
        }

        /// <summary>
        /// Добавить скрипт
        /// </summary>
        /// <param name="formula">Узел содержащий формулу</param>
        protected void InsertScript(XmlNode formula)
        {
            Formula frm = new Formula();
            Script script = new Script();

            frm.Position = GetPosition(formula);
            XmlNode macros = GetMacros(formula);

            if (macros != null)
            {
                script.InstanceMacrosFromXmlNode(macros);

                frm.Macros = script;
                InsertFormula(frm);
            }
        }

        /// <summary>
        /// Добавить присваивание
        /// </summary>
        /// <param name="formula">Узел, содкржащий формулу</param>
        protected void InsertAssignment(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Assignment assignment = new Assignment();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    assignment.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = assignment;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить сумму
        /// </summary>
        /// <param name="formula">Узел, содкржащий формулу</param>
        protected void InsertSumma(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Summa summa = new Summa();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    summa.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = summa;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить разность
        /// </summary>
        /// <param name="formula">Узел, содкржащий формулу</param>
        protected void InsertDifference(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Difference difference = new Difference();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    difference.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = difference;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить умножение
        /// </summary>
        /// <param name="formula">Узел, содкржащий формулу</param>
        protected void InsertMultiplication(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Multiplication multiplication = new Multiplication();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    multiplication.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = multiplication;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить деление
        /// </summary>
        /// <param name="formula">Узел, содержащий формулу</param>
        protected void InsertDivizion(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Division divizion = new Division();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    divizion.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = divizion;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить приращение
        /// </summary>
        /// <param name="formula">Узел, содержащий формулу</param>
        protected void InsertIncrement(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Increment increment = new Increment();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    increment.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = increment;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить максимум
        /// </summary>
        /// <param name="formula">Узел, содержащий формулу</param>
        protected void InsertMaximum(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Maximum maximum = new Maximum();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    maximum.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = maximum;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить минимум
        /// </summary>
        /// <param name="formula">Узел, содержащий формулу</param>
        protected void InsertMinimum(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Minimum minimum = new Minimum();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    minimum.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = minimum;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить 10 в степени X
        /// </summary>
        /// <param name="formula">Узел, содержащий формулу</param>
        protected void InsertPowerOf10(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                PowerOf10 power = new PowerOf10();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    power.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = power;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить сумму всех значений параметра
        /// </summary>
        /// <param name="formula">Узел, содержащий формулу</param>
        protected void InsertAccumulation(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Accumulation accumulation = new Accumulation();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    accumulation.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = accumulation;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить сумму
        /// </summary>
        /// <param name="formula">Узел, содкржащий формулу</param>
        protected void InsertMedia(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Media media = new Media();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    media.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = media;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Добавить сумму
        /// </summary>
        /// <param name="formula">Узел, содкржащий формулу</param>
        protected void InsertTransformation(XmlNode formula)
        {
            try
            {
                Formula frm = new Formula();
                Transformation media = new Transformation();

                frm.Position = GetPosition(formula);
                XmlNode macros = GetMacros(formula);

                if (macros != null)
                {
                    media.InstanceMacrosFromXmlNode(macros);

                    frm.Macros = media;
                    InsertFormula(frm);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Получить узел в котором содержатся настройки макроса
        /// </summary>
        /// <param name="formula">Формула в которой искать узел с настройками макроса</param>
        /// <returns>Узел с настройками макрова, или null</returns>
        protected XmlNode GetMacros(XmlNode formula)
        {
            try
            {
                if (formula != null)
                {
                    XmlNodeList childs = formula.ChildNodes;
                    if (childs != null)
                    {
                        foreach (XmlNode child in childs)
                        {
                            if (child.Name == macrosName)
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
        /// Получить индекс формулы
        /// </summary>
        /// <param name="formula">Формула из которой извлечь индекс</param>
        /// <returns>Индекс формулы</returns>
        protected int GetPosition(XmlNode formula)
        {
            try
            {
                XmlNodeList childs = formula.ChildNodes;
                if (childs != null)
                {
                    foreach (XmlNode child in childs)
                    {
                        if (child.Name == positionName)
                        {
                            return int.Parse(child.InnerText);
                        }
                    }
                }
            }
            catch
            {
            }
            return -1;
        }

        /// <summary>
        /// Определить тип формулы
        /// </summary>
        /// <param name="formula">Узел в котором содержится формула</param>
        /// <returns>Тип формулы</returns>
        protected FormulaType GetFormulaType(XmlNode formula)
        {
            try
            {
                if (formula != null)
                {
                    XmlNodeList childs = formula.ChildNodes;
                    if (childs != null)
                    {
                        foreach (XmlNode child in childs)
                        {
                            switch (child.Name)
                            {
                                case formulaTypeName:

                                    try
                                    {
                                        return (FormulaType)Enum.Parse(typeof(FormulaType), child.InnerText);
                                    }
                                    catch
                                    {
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return FormulaType.Default;
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