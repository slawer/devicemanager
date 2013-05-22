using System;
using System.Threading;
using System.Collections.Generic;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class Saver
    {
        /// <summary>
        /// Выполняет сохранение данных в файл
        /// </summary>
        /// <param name="state">Не используется, равно null</param>
        private void TimerCallback(object state)
        {
            bool blocked = false;
            try
            {
                if (t_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    if (IsConverted)
                    {
                        Interlocked.Exchange(ref isconverted, 0);
                        if (file != null && file.IsLoaded)
                        {
                            DateTime now = DateTime.Now;
                            TimeSpan span = now - last_time;

                            Float[] results = app.Converter.GetResults();
                            Parameter[] parameters = Parameters;
                            
                            if (results != null && parameters != null)
                            {
                                // ---- выделить данные для сохранения ----

                                List<byte[]> blocks = GetBlocks(parameters, results, now.ToOADate());
                                if (blocks != null)
                                {                                    
                                    bool updated = false;
                                    foreach (var block in blocks)
                                    {
                                        if (span >= timeout && updated == false)
                                        {
                                            updated = true;
                                            file.WriteBlockWithStamp(block, now.ToOADate());

                                            last_time = now;
                                        }
                                        else
                                            file.WriteBlock(block);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message, ErrorType.NotFatal));
                }
            }
            finally
            {
                if (blocked) t_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Получить блоки для записи в файл
        /// </summary>
        /// <param name="parameters">Сохраняемые параметры</param>
        /// <param name="result">Параметры конвертера</param>
        /// <param name="time">Время вставляемое в блок данных</param>
        /// <returns>Список блоков для записи в файл, или null если не удалось выделить ни один блок</returns>
        private List<byte[]> GetBlocks(Parameter[] parameters, Float[] result, double time)
        {
            List<byte[]> blocks = new List<byte[]>();

            int block_count = (int)(parameters.Length / 126);
            int ostatok = (int)(parameters.Length % 126);

            if (block_count == 0 && ostatok == 0) return null;
            byte[] b_time = BitConverter.GetBytes(time);

            int position = 0;
            for (int i = 0; i < block_count; i++)
            {
                byte[] block = new byte[512];
                Array.Copy(b_time, block, b_time.Length);       // сохранили время

                for (int j = 0; j < 126; j++)
                {
                    if (position < parameters.Length)
                    {
                        if (parameters[position].IsActual)
                        {
                            float current = result[parameters[position++].Position].GetCurrentValue();
                            byte[] val = BitConverter.GetBytes(current);

                            Array.Copy(val, 0, block, (j * 4 + 8), 4);
                        }
                    }
                    else
                        return null;
                }

                blocks.Add(block);
            }

            if (ostatok > 0)
            {
                byte[] block_ost = new byte[512];
                for (int i = 0; i < ostatok; i++)
                {
                    Array.Copy(b_time, block_ost, b_time.Length);       // сохранили время
                    if (position < result.Length)
                    {
                        float current = result[parameters[position++].Position].GetCurrentValue();
                        byte[] val = BitConverter.GetBytes(current);

                        Array.Copy(val, 0, block_ost, (i * 4 + 8), 4);
                    }
                    else
                        return null;
                }

                blocks.Add(block_ost);
            }
            return blocks;
        }
    }    
}