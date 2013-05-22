using System;
using System.Threading;
using System.Collections.Generic;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class Saver
    {
        protected Mutex p_mutex = null;                     // синхронизирует доступ к параметрам
        protected List<Parameter> parameters = null;        // список сохраняемых параметров

        /// <summary>
        /// Добавить параметр
        /// </summary>
        /// <param name="parameter">Добавляемый параметр</param>
        public void Insert(Parameter parameter)
        {
            bool blocked = false;
            try
            {
                if (p_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    parameters.Add(parameter);
                }
            }
            finally
            {
                if (blocked) p_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Удалить параметр
        /// </summary>
        /// <param name="parameter">Удаляемый параметр</param>
        public void Remove(Parameter parameter)
        {
            bool blocked = false;
            try
            {
                if (p_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    parameters.Remove(parameter);
                }
            }
            finally
            {
                if (blocked) p_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Очистить список параметров
        /// </summary>
        public void Clear()
        {
            bool blocked = false;
            try
            {
                if (p_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    parameters.Clear();
                }
            }
            finally
            {
                if (blocked) p_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Возвращяет список сохраняемых параметров
        /// </summary>
        public Parameter[] Parameters
        {
            get
            {
                bool blocked = false;
                try
                {
                    if (p_mutex.WaitOne(100, false))
                    {
                        blocked = true;
                        return parameters.ToArray();
                    }
                    else
                        return null;
                }
                finally
                {
                    if (blocked) p_mutex.ReleaseMutex();
                }
            }
        }
    }
}