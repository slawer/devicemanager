using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class File
    {
        protected long countSavedBlocks;        // количество сохраненых блоков информации
        protected long countSavedLabels;        // количество сохраненых меток время/смещение        

        protected long stam_index;              // индекс метки время/смещение
        protected long block_offset;            // смещение по которому выполнять запись блока данных

        /// <summary>
        /// Выполнить инициализацию смещений
        /// </summary>
        private void InitializeOffsets()
        {
            if (Stamps != null && Stamps.Length > 0)
            {
                List<Stamp> stamps = new List<Stamp>();
                foreach (var stam in Stamps)
                {
                    if (stam.DateTime > 0)
                    {
                        if (stam.F_Offset == stam.S_Offset)
                        {
                            stamps.Add(stam);
                        }
                    }
                }

                if (stamps.Count > 0)
                {
                    int index = 0;
                    Stamp max = stamps[0];

                    for (int i = 1; i < stamps.Count; i++)
                    {
                        if (stamps[i].DateTime >= max.DateTime)
                        {
                            index = i;
                            max = stamps[i];

                            max.index = i;
                        }
                    }

                    stam_index = index;
                    block_offset = max.F_Offset;
                    
                    CalculateBlockOffset();
                }
                else
                {
                    stam_index = 0;
                    block_offset = DataOffset;
                }
            }
        }

        /// <summary>
        /// Вычислить смещение блока данных
        /// </summary>
        private void CalculateBlockOffset()
        {
            long total_block_offset = DataAreaSize + DataOffset;
            if (block_offset < DataOffset) block_offset = DataOffset;

            if (block_offset >= total_block_offset)
            {
                block_offset = DataOffset;
            }
        }

        /// <summary>
        /// Сохранить блок данных в файл
        /// </summary>
        /// <param name="data">Блок сохраняемых данных</param>
        public void WriteBlock(byte[] data)
        {
            try
            {
                if (file != null)
                {
                    CalculateBlockOffset();
                    file.Seek(block_offset, System.IO.SeekOrigin.Begin);

                    file.Write(data, 0, data.Length);
                    file.Flush();

                    block_offset += BLOCK_SIZE;
                    Interlocked.Increment(ref countSavedBlocks);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Сохранить блок данных и добавить метку время/смещение
        /// </summary>
        /// <param name="data">Сохраняемый блок данных</param>
        public void WriteBlockWithStamp(byte[] data, double now)
        {
            long stam_offset = -1;
            long block_off_saved = -1;

            try
            {
                CalculateBlockOffset();
                if (stam_index >= File.COUNT_STAMPS)
                {
                    stam_index = 0;
                }

                stam_offset = (long)(stam_index * STAM_SIZE) + StampsOffset;         // смещение в файле метки время/смещение
                
                stam_index = stam_index + 1;
                file.Seek(stam_offset, System.IO.SeekOrigin.Begin);                     // установливаем позицию для записи метки

                Writer.Write(now);
                
                Writer.Write(block_offset);
                Writer.Write(block_offset + 1);

                Writer.Flush();

                block_off_saved = block_offset;                
                
                file.Seek(block_offset, System.IO.SeekOrigin.Begin);
                file.Write(data, 0, data.Length);
                
                file.Seek(stam_offset + 16, System.IO.SeekOrigin.Begin);
                Writer.Write(block_off_saved);

                block_offset += 512;

                file.Flush();

                Interlocked.Increment(ref countSavedBlocks);
                Interlocked.Increment(ref countSavedLabels);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Количестыо сохраненых блоков данных
        /// </summary>
        public long CountSavedBlocks
        {
            get { return Interlocked.Read(ref countSavedBlocks); }
            set { Interlocked.Exchange(ref countSavedBlocks, 0); }
        }

        /// <summary>
        /// Количество сохраненых меток время/смещение
        /// </summary>
        public long CountSavedLabels
        {
            get { return Interlocked.Read(ref countSavedLabels); }
            set { Interlocked.Exchange(ref countSavedLabels, 0); }
        }
    }
}