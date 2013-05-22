using System;
using System.Collections.Generic;

namespace DeviceManager
{
    public partial class Converter
    {
        /// <summary>
        /// Возвращяет список формул
        /// </summary>
        public Formula[] Formuls
        {
            get
            {
                try
                {
                    f_locker.AcquireReaderLock(100);
                    try
                    {
                        return formuls.ToArray();
                    }
                    finally
                    {
                        f_locker.ReleaseReaderLock();
                    }
                }
                catch { }
                return null;
            }
        }

        /// <summary>
        /// Добавить формулу в список
        /// </summary>
        /// <param name="formula">Добавляемая формула</param>
        public void InsertFormula(Formula formula)
        {
            try
            {
                f_locker.AcquireWriterLock(300);
                try
                {
                    formuls.Add(formula);
                    formuls.Sort(new Comparer());
                }
                finally
                {
                    f_locker.ReleaseWriterLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Удалить формулу из списка
        /// </summary>
        /// <param name="formula">Удаляемая формула</param>
        public void RemoveFormula(Formula formula)
        {
            try
            {
                f_locker.AcquireWriterLock(300);
                try
                {
                    formula.IsActual = false;

                    formuls.Remove(formula);
                    ResetParameter(formula.Position);

                    formuls.Sort(new Comparer());
                }
                finally
                {
                    f_locker.ReleaseWriterLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// отсортировать формулы
        /// </summary>
        public void SortFormuls()
        {
            try
            {
                f_locker.AcquireWriterLock(300);
                try
                {
                    formuls.Sort(new Comparer());
                }
                finally
                {
                    f_locker.ReleaseWriterLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// получить свободный канал
        /// </summary>
        /// <returns></returns>
        public int GetFreeChannel()
        {
            Formula[] parameters = Formuls;
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    Array.Sort(parameters, new Comparer());
                    if (parameters[0].Position > 0)
                    {
                        return 0;
                    }
                    else
                    {
                        for (int index = 0; index < parameters.Length - 1; index++)
                        {
                            if ((parameters[index].Position + 1) != parameters[index + 1].Position)
                            {
                                return parameters[index].Position + 1;
                            }
                        }

                        int position = parameters[parameters.Length - 1].Position;
                        if (position < 1023)
                        {
                            return (position + 1);
                        }
                        else
                            return -1;
                    }
                }
                else
                    return 0;
            }
            else
                return -1;
        }

        /// <summary>
        /// Сбросить значение параметра
        /// </summary>
        /// <param name="position">Номер обнуляемого параметра</param>
        protected void ResetParameter(int position)
        {
            try
            {
                p_locker.AcquireWriterLock(300);
                try
                {
                    if (position > -1 && position < parameters.Length)
                    {
                        parameters[position].Value = float.NaN;
                    }
                }
                finally
                {
                    p_locker.ReleaseWriterLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Реализует сравнение двух условий
        /// </summary>
        private class Comparer : IComparer<Formula>
        {
            /// <summary>
            /// Сравнивает два объекта и возвращает значение, показывающее, что один объект
            /// меньше или больше другого или равен ему.
            /// </summary>
            /// <param name="x">Первый сравниваемый объект.</param>
            /// <param name="y">Второй сравниваемый объект.</param>
            /// <returns>
            /// Значение Условие Меньше нуля x меньше, чем y. Нуль x равно y. Больше нуля
            /// x больше, чем y.
            /// </returns>
            public int Compare(Formula x, Formula y)
            {
                if (x.Position > y.Position)
                {
                    return 1;
                }
                else
                    if (x.Position < y.Position)
                    {
                        return -1;
                    }

                return 0;
            }
        }
    }

    /// <summary>
    /// Интерфейс функции, обрабатывающей событие добавление формулы в конвертор
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="args">Передаваемые аргументы</param>
    public delegate void InsertFormulaEventHandler(object sender, ConverterEventArgs args);

    /// <summary>
    /// Интерфейс функции, обрабатывающей событие удаление формулы в конвертор
    /// </summary>
    /// <param name="sender">Источник события</param>
    /// <param name="args">Передаваемые аргументы</param>
    public delegate void RemoveFormulaEventHandler(object sender, ConverterEventArgs args);

    /// <summary>
    /// Реализует событие конвертора
    /// </summary>
    public class ConverterEventArgs : EventArgs
    {
        private Formula formula;                    // формула

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public ConverterEventArgs()
        {
            formula = null;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="_formula">Передаваемая формула</param>
        public ConverterEventArgs(Formula _formula)
        {
            formula = _formula;
        }

        /// <summary>
        /// Формула конвертора
        /// </summary>
        public Formula Formula
        {
            get { return formula; }
        }
    }
}