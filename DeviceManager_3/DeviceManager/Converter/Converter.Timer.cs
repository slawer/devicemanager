using System;
using DeviceManager.Errors;

namespace DeviceManager
{
    public partial class Converter
    {
        /// <summary>
        /// Процедура вычисления параметров конвертера
        /// </summary>
        /// <param name="state">Равно null</param>
        public void TimerCallback(object state)
        {
            bool blocked = false;
            try
            {
                if (t_mutex.WaitOne(100, false))
                {
                    blocked = true;

                    try
                    {
                        Float[] slice = stock.GetSlice();
                        if (slice != null)
                        {
                            DoMacroses(slice);
                            if (OnComplete != null)
                            {
                                OnComplete(this, new EventArgs());
                            }
                        }
                    }
                    catch
                    {
                        throw new Exception("Converter->TimerCallback->Inner Try");
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Converter->TimerCallback", ErrorType.NotFatal));
                }
            }
            finally
            {
                if (blocked) t_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Вычислить параметры
        /// </summary>
        /// <param name="values">Данный из пакетов</param>
        private void DoMacroses(Float[] values)
        {
            try
            {
                f_locker.AcquireWriterLock(100);
                try
                {
                    try
                    {
                        p_locker.AcquireWriterLock(100);
                        try
                        {
                            foreach (Formula formula in formuls)
                            {
                                if (formula.IsActual)
                                {
                                    float result = formula.Macros.Calculate(values, parameters);
                                    if (formula.Position > -1 && formula.Position < parameters.Length)
                                    {
                                        parameters[formula.Position].Value = result;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            p_locker.ReleaseWriterLock();
                        }
                    }
                    catch { }
                }
                finally
                {
                    f_locker.ReleaseWriterLock();
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Converter->DoMacroses", ErrorType.NotFatal));
                }
            }
        }
    }
}