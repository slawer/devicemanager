using System;
using System.Threading;
using System.Collections.Generic;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение пакетов поступивших от устройств и передачу конвертеру сохраненных пакетов
    /// </summary>
    public partial class Stock
    {
        /// <summary>
        /// Возвращяет список условий, на основе которых извлекаются данные из пакетов
        /// </summary>
        public Parameter[] Conditions
        {
            get
            {
                bool blocked = false;
                try
                {
                    if (c_mutex.WaitOne(100, false))
                    {
                        blocked = true;
                        return parameters.ToArray();
                    }
                    else
                        return null;
                }
                finally
                {
                    if (blocked) c_mutex.ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// Добавить условие
        /// </summary>
        /// <param name="condition">Добавляемое условие</param>
        public void InsertCondition(Parameter condition)
        {
            bool blocked = false;
            try
            {
                if (c_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    parameters.Add(condition);
                }
            }
            finally
            {
                if (blocked) c_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Удалить условие
        /// </summary>
        /// <param name="condition">Удаляемое условие</param>
        public void RemoveCondition(Parameter condition)
        {
            bool blocked = false;
            try
            {
                if (c_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    parameters.Remove(condition);

                    ResetParameter(condition.Position);
                }
            }
            finally
            {
                if (blocked) c_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Сбросить значение параметра
        /// </summary>
        /// <param name="position">Номер обнуляемого параметра</param>
        protected void ResetParameter(int position)
        {
            bool blocked = false;
            try
            {
                if (v_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    if (position > -1 && position < slave.Length)
                    {
                        slave[position].Value = float.NaN;
                    }
                }
            }
            finally
            {
                if (blocked) v_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Отсортировать список условий
        /// </summary>
        public void SortConditions()
        {
            bool blocked = false;
            try
            {
                if (c_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    Comparer comparer = new Comparer();

                    parameters.Sort(comparer);
                }
            }
            finally
            {
                if (blocked) c_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Очистить список условий
        /// </summary>
        public void Clear()
        {
            bool blocked = false;
            try
            {
                if (c_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    parameters.Clear();
                }
            }
            finally
            {
                if (blocked) c_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// получить свободный канал
        /// </summary>
        /// <returns></returns>
        public int GetFreeChannel()
        {
            Parameter[] parameters = Conditions;
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
        /// Реализует сравнение двух условий
        /// </summary>
        private class Comparer : IComparer<Parameter>
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
            public int Compare(Parameter x, Parameter y)
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
}