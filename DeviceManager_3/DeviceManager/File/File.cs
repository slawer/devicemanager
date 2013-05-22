using System;
using System.IO;
using System.Security.Permissions;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение данных в файл
    /// </summary>
    public partial class File
    {
        private long default_file_size;         // требуемый размер файла

        private BinaryReader reader = null;     // реализует двочное чтени данных
        private BinaryWriter writer = null;     // реализует двочную запись данных

        private FileStream file = null;         // файл с которым осуществляется работа

        private bool isloaded;                  // показывает загружен или нет файл

        /// <summary>
        /// Возникает когда не удалось загрузить файл
        /// </summary>
        public event EventHandler OnErrorLoad;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public File()
        {
            stamps = new Stamp[COUNT_STAMPS];
            for (int index = 0; index < COUNT_STAMPS; index++)
            {
                stamps[index] = new Stamp();
            }

            block_size = BLOCK_SIZE;
            area_size = DATA_AREA_SIZE;

            stamp_offset = BASE_STAMPS_OFFSET;
            data_offset = BASE_DATA_OFFSET;

            p_count = 0;
            default_file_size = (long)Math.Pow(1024, 3);
        }

        /// <summary>
        /// Показывает загружен файл или нет
        /// </summary>
        public bool IsLoaded
        {
            get { return isloaded; }
        }

        /// <summary>
        /// Возвращяет имя загруженного файла
        /// </summary>
        public string URI
        {
            get 
            {
                if (file != null)
                {
                    return file.Name;
                }
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Возвращяет объект с помощью которого осуществляется двочная запись данных в файл
        /// </summary>
        protected BinaryWriter Writer
        {
            get
            {
                if (writer == null)
                {
                    writer = new BinaryWriter(file);
                }
                return writer;
            }
        }

        /// <summary>
        /// Возвращяет объект с помощью которого осуществляется двочное данных данных из файла
        /// </summary>
        protected BinaryReader Reader
        {
            get
            {
                if (reader == null)
                {
                    reader = new BinaryReader(file);
                }

                return reader;
            }
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="Uri">URI загружаемого файла</param>
        public void Load(string Uri)
        {
            try
            {
                if (System.IO.File.Exists(Uri))
                {
                    Close();
                    file = new FileStream(Uri, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

                    if (file != null && file.Length == default_file_size)
                    {   
                        block_size = Reader.ReadInt64();
                        timestamp = Reader.ReadInt64();

                        area_size = Reader.ReadInt64();
                        stamp_offset = Reader.ReadInt64();

                        data_offset = Reader.ReadInt64();
                        p_count = Reader.ReadInt64();

                        speriod = Reader.ReadInt64();
                        period = Reader.ReadInt64();

                        if (Validate())
                        {
                            file.Seek(BASE_STAMPS_OFFSET, SeekOrigin.Begin);
                            for (int i = 0; i < stamps.Length; i++)
                            {
                                double d_time = Reader.ReadDouble();
                                if (d_time > 0)
                                {
                                    DateTime time = DateTime.FromOADate(d_time);
                                    stamps[i].DateTime = time.Ticks;

                                    stamps[i].F_Offset = Reader.ReadInt64();
                                    stamps[i].S_Offset = Reader.ReadInt64();
                                }
                            }

                            InitializeOffsets();

                            try
                            {
                                FileIOPermission premission = new FileIOPermission(FileIOPermissionAccess.Read, Uri);
                                premission.Demand();
                            }
                            catch { }

                            isloaded = true;
                        }
                        else
                            GenerateErrorLoad();
                    }
                    else
                        GenerateErrorLoad();
                }
                else
                    GenerateErrorLoad();
            }
            catch
            {
                GenerateErrorLoad();
            }
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public void Close()
        {
            try
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();

                    file = null;

                    reader = null;
                    writer = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Создать новый файл
        /// </summary>
        /// <param name="Uri">URI создаваемого файла</param>
        /// <param name="pCount">Количество параметров сохраняемых в файле</param>
        public static void CreateNewFile(string Uri, int pCount)
        {
            FileStream file = null;
            try
            {
                if (Uri != string.Empty)
                {
                    if (!System.IO.File.Exists(Uri))
                    {
                        file = System.IO.File.Open(Uri, FileMode.Create);

                        long f_size = (long)Math.Pow(1024, 3);
                        file.SetLength(f_size);

                        Mark(file, pCount);                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }

        /// <summary>
        /// Разметить файл
        /// </summary>
        private static void Mark(FileStream file, int pCount)
        {
            BinaryWriter writer = new BinaryWriter(file);

            long block_size = BLOCK_SIZE;
            long labels = COUNT_STAMPS;

            long data_size = DATA_AREA_SIZE;
            long l_offset = BASE_STAMPS_OFFSET;

            long d_offset = BASE_DATA_OFFSET;
            long p_count = (long)pCount;

            writer.Write(block_size);
            writer.Write(labels);

            writer.Write(data_size);
            writer.Write(l_offset);

            writer.Write(d_offset);
            writer.Write(p_count);

            writer.Write(DEF_SPERIOD);
            writer.Write(DEF_PERIOD);
        } 

        /// <summary>
        /// Проверить на корректность файл
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            if (block_size == BLOCK_SIZE && timestamp == COUNT_STAMPS &&
                stamp_offset == BASE_STAMPS_OFFSET && data_offset == BASE_DATA_OFFSET)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Сгенерировать событие "Ошибка загрузки файла"
        /// </summary>
        private void GenerateErrorLoad()
        {
            if (file != null)
            {
                file.Close();
                file.Dispose();
            }

            if (OnErrorLoad != null)
            {
                OnErrorLoad(this, EventArgs.Empty);
            }
        }
    }
}