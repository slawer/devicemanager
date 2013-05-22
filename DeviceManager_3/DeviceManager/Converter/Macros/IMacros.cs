using System;
using System.Xml;

namespace DeviceManager
{
    /// <summary>
    /// Определяет интерфейс формулы вычисления параметра конвейера
    /// </summary>
    public interface IMacros
    {
        /// <summary>
        /// Возвращяет тип формулы
        /// </summary>
        FormulaType Type { get; }

        /// <summary>
        /// Определяет список параметров макроса
        /// </summary>
        Argument[] Args { get; }

        /// <summary>
        /// Определяет значение макроса
        /// </summary>
        float Value { get; set; }

        /// <summary>
        /// Вычислить параметр 
        /// </summary>
        /// <param name="signals">Значения поступившие с датчиков</param>
        /// <param name="results">Значения являющиеся конечными данными</param>
        /// <returns>Вычисленное значение</returns>
        float Calculate(Float[] signals, Float[] results);

        /// <summary>
        /// Сбросить состояние макроcа в начальное состояние (по умолчанию)
        /// </summary>
        void Reset();

        /// <summary>
        /// Описание формулы
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Описание аргументов формулы
        /// </summary>
        string Arguments { get; }

        /// <summary>
        /// Текстовое описание формулы (сложение, вычитание и т.п..)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Получить XmlNode макроса для сохранения начтроек конвертора
        /// </summary>
        /// <param name="document">Документ в который осуществляется сохранение настроек</param>
        /// <returns>XmlNode макроса</returns>
        XmlNode CreateXmlNode(XmlDocument document);

        /// <summary>
        /// Инициализировать формулу из сохраненного раннее узла Xml
        /// </summary>
        /// <param name="node">Узел на основе которого выполнить инициализацию макроса</param>
        void InstanceMacrosFromXmlNode(XmlNode node);
    }
}