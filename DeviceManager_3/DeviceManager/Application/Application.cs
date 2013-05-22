using System;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualBasic;

using DeviceManager.DevMan;
using DeviceManager.Errors;

using SoftwareDevelopmentKit.Log;
using SoftwareDevelopmentKit.Protocols.Dsn;

using WCF;

namespace DeviceManager
{
    /// <summary>
    /// Реализует приложение DeviceManager.
    /// Является управляющим классом, предоставляет интерфейсы ...
    /// </summary>
    public partial class Application
    {
        /// <summary>
        /// Размер массивов, хранящих данные с датчиков и вычисленные значения
        /// </summary>
        protected const int size = 1024;                    // размер массивов, хранящих данные с датчиков и вычисленные значения

        protected Float[] signals = null;                   // данные с датчиков
        protected Float[] results = null;                   // вычисленные значения

        protected Journal journal = null;                   // реализует работу с журналом Windows

        // ------------------------
                
        protected TypeCRC crc;                              // тип используемой CRC
        protected ApplicationMode mode;                     // режим в котором осуществлять работу

        protected Boolean autorun = false;                  // осуществлять запуск обмена или нет
        protected ReaderWriterLock p_locker;                // синхронизирует доступ к типу CRC и режиму работы приложения

        protected IProtocol protocol = null;                // реализует работу с протоколом обмена Dsn
        
        // -----------------------

        protected Serial serial = null;                     // подсистема, осуществляющая работу с последовательным портом

        protected Stock stock;                              // осуществляет извлечение данных из пакетов
        protected Converter converter;                      // осуществляет обработку извлеченных данных

        protected DisplayUnit display;                      // осуществляет отправку данных 
        protected TcpDevManager devTcpOld;                  // реализует стандартный обмен по TCP

        protected Saver saver;                              // реализует сохранение данных в файл
        protected Service service;                          // реализует сетевую службу

        // ----- ------

        private Mutex t_mutex = null;                       // синхронизатор таймера
        protected Timer timer = null;                       // инициирует работу конвертера в режимах: пассивный, эмулирующий

        protected bool t_started;                           // показывает состояние таймера
        protected int period = 100;                         // частота работы таймера

        protected State state = State.Default;              // текущее состояние
        protected Boolean notify = false;                   // скрывать главное окно или нет

        // ------------------- -----------------------

        protected Repository repository = null;             // репозитарий пакетов

        /// <summary>
        /// Возникает когда необходимо завершить работу приложения
        /// </summary>
        public event EventHandler OnExit;

        /// <summary>
        /// Возникает когда приложение начинает работу
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// Возникает когда приложение останавливает свою работу
        /// </summary>
        public event EventHandler OnStop;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        protected Application()
        {
            signals = new Float[size];
            results = new Float[size];

            repository = new Repository();
            repository.onError += new ApplicationErrorHandler(ErrorHandler);

            for (int i = 0; i < size; i++)
            {
                signals[i] = new Float();
                results[i] = new Float();
            }

            journal = Journal.CreateInstance();
             
            p_locker = new ReaderWriterLock();

            crc = TypeCRC.Cycled;
            mode = ApplicationMode.Active;

            serial = new Serial(repository);
            serial.Secondary = new SecondaryPort(repository);

            stock = new Stock(signals);

            display = new DisplayUnit(repository);
            converter = new Converter(stock, results);

            devTcpOld = new TcpDevManager();

            t_mutex = new Mutex();
            timer = new Timer(TimerElapsed, null, Timeout.Infinite, period);

            devTcpOld.Place = repository.InstancePlace();

            saver = new Saver();
            service = new Service();
        }

        /// <summary>
        /// Определяет скрывать главную форму или нет после старта
        /// </summary>
        public Boolean isNotify
        {
            get { return notify; }
            set { notify = value; }
        }

        /// <summary>
        /// Выполнить инициализацию подсистем
        /// </summary>
        public void Initialize()
        {
            serial.Initialize();
            serial.Secondary.Initialize();

            stock.Initialize();

            converter.Initialize();
            display.Initialize();

            saver.Initialize();
            service.Initialize();

            protocol = Protocol.GetProtocol(ProtocolVersion.x100);

            serial.OnStart += new EventHandler(serial_OnStart);
            serial.OnStop += new EventHandler(serial_OnStop);

            serial.OnComplete += new EventHandler(serial_OnComplete);
            serial.OnPacket += new SerialEventHandler(serial_OnPacket);

            serial.OnPortFail += new EventHandler(serial_OnPortFail);
            serial.OnError += new ApplicationErrorHandler(ErrorHandler);

            serial.OnStaticComplete += new EventHandler(serial_OnStaticComplete);
            serial.OnDynamicComplete += new EventHandler(serial_OnDynamicComplete);

            serial.OnPacket += new SerialEventHandler(stock.SerialPacketHandler);
            serial.OnStaticComplete += new EventHandler(converter.SerialStaticEventHandler);            

            // ---------------------------

            serial.Secondary.OnStart += new EventHandler(serial_OnStart);
            serial.Secondary.OnStop += new EventHandler(serial_OnStop);

            serial.Secondary.OnComplete += new EventHandler(serial_OnComplete);
            serial.Secondary.OnPacket += new SerialEventHandler(serial_OnPacket);

            serial.Secondary.OnPortFail += new EventHandler(serial_OnPortFail);
            serial.Secondary.OnError += new ApplicationErrorHandler(ErrorHandler);

            serial.Secondary.OnStaticComplete += new EventHandler(serial_OnStaticComplete);
            serial.Secondary.OnDynamicComplete += new EventHandler(serial_OnDynamicComplete);

            serial.Secondary.OnPacket += new SerialEventHandler(stock.SerialPacketHandler);
            //serial.Secondary.OnStaticComplete += new EventHandler(converter.SerialStaticEventHandler);

            // ---------------------------

            stock.OnError += new ApplicationErrorHandler(ErrorHandler);

            converter.OnStart += new EventHandler(converter_OnStart);
            converter.OnStop += new EventHandler(converter_OnStop);

            converter.OnError += new ApplicationErrorHandler(ErrorHandler);
            converter.OnComplete += new EventHandler(converter_OnComplete);

            converter.OnComplete += new EventHandler(display.OnCompleteConverter);
            converter.OnComplete += new EventHandler(saver.ConverterCompletedEventHandler);

            display.OnStart += new EventHandler(display_OnStart);
            display.OnStop += new EventHandler(display_OnStop);

            display.OnComplete += new EventHandler(display_OnComplete);
            display.OnError += new ApplicationErrorHandler(ErrorHandler);

            devTcpOld.OnPacket += new TcpDevManager.PacketEventHandler(devTcpOld_OnPacket);

            saver.OnError += new ApplicationErrorHandler(ErrorHandler);
            
            saver.OnStart += new EventHandler(saver_OnStart);
            saver.OnStop += new EventHandler(saver_OnStop);

            LoadConfiguration();
            if (Autorun)
            {
                if (mode == ApplicationMode.Active || mode == ApplicationMode.Passive)
                {
                    if (!serial.Port.IsOpen)
                    {
                        if (serial.Port.PortName != string.Empty)
                        {
                            foreach (string portName in System.IO.Ports.SerialPort.GetPortNames())
                            {
                                if (serial.Port.PortName == portName)
                                {
                                    try
                                    {
                                        serial.Port.Open();
                                        Run();
                                    }
                                    catch
                                    {
                                        return;
                                    }
                                }

                                if (serial.Secondary.Port.PortName == portName)
                                {
                                    try
                                    {
                                        serial.Secondary.Port.Open();
                                    }
                                    catch 
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    
                }
                else
                    Run();
            }
        }

        /// <summary>
        /// Инициирует работу таймера
        /// </summary>
        /// <param name="state">Не используется, равно null</param>
        protected void TimerElapsed(object state)
        {
            bool blocked = false;
            try
            {
                if (t_mutex.WaitOne(0, false))
                {
                    blocked = true;
                    converter.SerialStaticEventHandler(serial, null);
                }
            }
            finally
            {
                if (blocked) t_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Возникает когда порт запущен
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        private void serial_OnStart(object sender, EventArgs e)
        {            
        }

        /// <summary>
        /// Возникает когда порт остановлен
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        private void serial_OnStop(object sender, EventArgs e)
        {            
        }

        /// <summary>
        /// Возникает когда порт завершил отправку команд
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        private void serial_OnComplete(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Поступил пакет от устройства
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="args">Параметры события</param>
        void serial_OnPacket(object sender, SerialEventArgs args)
        {
            try
            {
                string packet = PacketTranslater.FromUnigueToTcp(args.Packet.Com_Packet);
                devTcpOld.Send(packet);
            }
            catch (Exception ex)
            {
                journal.Write(ex.Message, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Ура! Порт отвалился
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        private void serial_OnPortFail(object sender, EventArgs e)
        {
            try
            {
                if (journal != null)
                {
                    Serial port = sender as Serial;
                    if (port != null)
                    {
                        if (port.Port != null)
                        {
                            journal.Write(string.Format("Порт {0} отвалился", port.Port.PortName), EventLogEntryType.Error);
                        }
                        else
                            journal.Write("Порт отвалился", EventLogEntryType.Error);
                    }
                    else
                        journal.Write("Порт отвалился", EventLogEntryType.Error);
                }
            }
            catch
            {
                // ...
            }
        }

        /// <summary>
        /// Возникает когда завершена обработка команд опроса устройств
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        private void serial_OnStaticComplete(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Возникает когда завершена обработка вторичных команд
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        private void serial_OnDynamicComplete(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Возникает когда запускается конвертер
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        void converter_OnStart(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Возникает когда конвертер прекращяет свою работу
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        void converter_OnStop(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Возникает когда конвертер выполнил обработку данных
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        void converter_OnComplete(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Возникает когда запущен модуль реализующий отправку данных на БО
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        void display_OnStart(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Возникает когда остановлен модуль реализующий отправку данных на БО
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        void display_OnStop(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Возникает когда модуль реализующий отправку данных на БО выполнил отправку данных на БО
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргумент события</param>
        void display_OnComplete(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Поступили данные по TCP
        /// </summary>
        /// <param name="packet">поступивший по Tcp пакет</param>
        private void devTcpOld_OnPacket(string packet)
        {
            try
            {
                Packet pack = new Packet();

                pack.Com_Packet = PacketTranslater.TranslateToUnigueFormatTcpPacket(packet, crc);
                pack.Tcp_Packet = PacketTranslater.FromUnigueToTcp(pack.Com_Packet);

                pack.Wait = false;
                if (protocol.IsToDevice(pack.Tcp_Packet))
                {
                    if (protocol.IsRead(pack.Tcp_Packet))
                    {
                        pack.Wait = true;
                    }
                }
                else
                {
                    pack.Role = Role.Slave;
                    DisplayPacket[] packs = Display.Packets;

                    if (packs != null)
                    {
                        foreach (DisplayPacket _pack in packs)
                        {
                            int device = pack.Com_Packet[1] & 0x1F;
                            if (device == _pack.Device)
                            {
                                pack.PortType = _pack.TypePort;
                                break;
                            }
                        }
                    }
                }

                devTcpOld.Place.Insert(pack);
            }
            catch (Exception ex)
            {
                journal.Write(ex.Message + "Application -> devTcpOld_OnPacket", EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Сохранение в файл запущенно
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Передаваемые параметры события</param>
        protected void saver_OnStart(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Сохранение в файл остановленно
        /// </summary>
        /// <param name="sender">Тсточник события</param>
        /// <param name="e">Передаваемые параметры события</param>
        protected void saver_OnStop(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Определяет тип используемой CRC
        /// </summary>
        public TypeCRC TypeCRC
        {
            get
            {
                try
                {
                    p_locker.AcquireReaderLock(100);
                    try
                    {
                        return crc;
                    }
                    finally
                    {
                        p_locker.ReleaseReaderLock();
                    }
                }
                catch { }
                return TypeCRC.CRC16;
            }

            set
            {
                try
                {
                    p_locker.AcquireWriterLock(100);
                    try
                    {
                        crc = value;
                    }
                    finally
                    {
                        p_locker.ReleaseWriterLock();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Определяет режим в котором работает приложение
        /// </summary>
        public ApplicationMode Mode
        {
            get
            {
                try
                {
                    p_locker.AcquireReaderLock(100);
                    try
                    {
                        return mode;
                    }
                    finally
                    {
                        p_locker.ReleaseReaderLock();
                    }
                }
                catch 
                {
                    //throw new TimeoutException("Не удалось получить режим в котором работает приложение");
                }

                return ApplicationMode.Default;
            }

            set
            {
                try
                {
                    p_locker.AcquireWriterLock(100);
                    try
                    {
                        mode = value;
                        if (state == State.Running)
                        {
                            switch (mode)
                            {
                                case ApplicationMode.Active:

                                    if (t_started)
                                    {
                                        t_started = false;
                                        timer.Change(Timeout.Infinite, period);
                                    }

                                    try
                                    {
                                        if (!Serial.Port.IsOpen)
                                        {
                                            Serial.Port.Open();
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    break;

                                case ApplicationMode.Passive:


                                    if (!t_started)
                                    {
                                        t_started = true;
                                        timer.Change(0, period);
                                    }

                                    try
                                    {
                                        if (!Serial.Port.IsOpen)
                                        {
                                            Serial.Port.Open();
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    break;

                                case ApplicationMode.Emulated:

                                    if (!t_started)
                                    {
                                        t_started = true;
                                        timer.Change(0, period);
                                    }

                                    try
                                    {
                                        if (Serial.Port.IsOpen)
                                        {
                                            Serial.Port.Close();
                                        }
                                    }
                                    catch { }

                                    try
                                    {
                                        if (Serial.Secondary.Port.IsOpen)
                                        {
                                            Serial.Secondary.Port.Close();
                                        }
                                    }
                                    catch { }
                                    break;
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
        }

        /// <summary>
        /// Определяет запускать приложение после загрузки или нет
        /// </summary>
        public Boolean Autorun
        {
            get { return autorun; }
            set { autorun = value; }
        }

        /// <summary>
        /// Осуществляет обработку ошибок
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="args">Параметры события</param>
        private void ErrorHandler(object sender, ErrorArgs args)
        {
            try
            {
                if (journal != null)
                {
                    switch (args.ErrorType)
                    {
                        case ErrorType.Information:

                            journal.Write(args.Message, EventLogEntryType.Information);
                            break;

                        case ErrorType.Warning:

                            journal.Write(args.Message, EventLogEntryType.Warning);
                            break;

                        case ErrorType.NotFatal:

                            journal.Write(args.Message, EventLogEntryType.Error);
                            break;

                        case ErrorType.Fatal:

                            journal.Write(args.Message, EventLogEntryType.Error);
                            if (OnExit != null)
                            {
                                OnExit(this, new EventArgs());
                            }   
                            break;

                        case ErrorType.Unknown:

                            journal.Write(args.Message, EventLogEntryType.Error);
                            break;

                        case ErrorType.Default:

                            journal.Write(args.Message, EventLogEntryType.Information);
                            break;
                    }
                }
                else
                {
                    journal = Journal.CreateInstance();
                    if (journal != null)
                    {
                        string message = string.Format("{0}{1}{2}", "Не был создан экземпляр класса Joirnal",
                            Constants.vbCrLf, "Сообщение приложения будет сохранено как Error!");

                        journal.Write(message, EventLogEntryType.Error);
                        journal.Write(args.Message, EventLogEntryType.Error);
                    }
                }
            }
            catch
            {
                // ...
            }
        }

        /// <summary>
        /// Текущее состояние
        /// </summary>
        public State State
        {
            get { return state; }
        }

        /// <summary>
        /// Возвращяет подсистему работающую с основным портом
        /// </summary>
        public Serial Serial
        {
            get { return serial; }
        }

        /// <summary>
        /// Возвращяет конвертер данных
        /// </summary>
        public Converter Converter
        {
            get { return converter; }
        }

        /// <summary>
        /// Возвращяет объект, осуществляет первичную обработку пакетов, извлечение данных из пакетов
        /// </summary>
        public Stock Stock
        {
            get { return stock; }
        }

        /// <summary>
        /// Возвращяет объект, реализующий работу с блоками отображения
        /// </summary>
        public DisplayUnit Display
        {
            get { return display; }
        }

        /// <summary>
        /// Возвращяет старый Tcp
        /// </summary>
        public TcpDevManager TcpDevManager
        {
            get { return devTcpOld; }
        }

        /// <summary>
        /// Возвращяет подсистему, реализующую сохранение параметров в файл
        /// </summary>
        public Saver Saver
        {
            get { return saver; }
        }

        /// <summary>
        /// Возвращяет репозитарий для пакетов
        /// </summary>
        public Repository Repository
        {
            get { return repository; }
        }

        /// <summary>
        /// Возвращяет протокол обмена с устройтсвами через COM порт
        /// </summary>
        public IProtocol IProtocol
        {
            get { return protocol; }
        }

        public Service Service
        {
            get { return service; }
        }

        /// <summary>
        /// Запустить программу
        /// </summary>
        public void Run()
        {
            if (state == State.Stopped || state == State.Default)
            {
                serial.Run();
                saver.Run();

                service.Run();
                devTcpOld.Start();

                switch (Mode)
                {
                    case ApplicationMode.Active:

                        if (t_started)
                        {
                            t_started = false;
                            timer.Change(Timeout.Infinite, period);
                        }
                        break;

                    case ApplicationMode.Passive:
                    case ApplicationMode.Emulated:

                        if (!t_started)
                        {
                            t_started = true;
                            timer.Change(0, period);
                        }
                        break;
                }

                state = State.Running;
                if (OnStart != null)
                {
                    OnStart(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Остановить работу программы
        /// </summary>
        public void Stop()
        {
            if (state == State.Running)
            {
                t_started = false;
                timer.Change(Timeout.Infinite, period);

                serial.Stop();
                saver.Stop();

                service.Stop();
                devTcpOld.Stop();

                state = State.Stopped;
                if (OnStop != null)
                {
                    OnStop(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Выделить число с плавающей точкой из строки
        /// </summary>
        /// <param name="single">Строка содержащая число</param>
        /// <returns>Значение или Nan если не удалось выполнить преобразование</returns>
        public static float ParseSingle(string single)
        {
            try
            {
                string ds = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                string value = single;

                value = value.Replace(".", ds);
                value = value.Replace(",", ds);

                return float.Parse(value);
            }
            catch
            { }

            return float.NaN;
        }

        /// <summary>
        /// Выделить число с плавающей точкой из строки
        /// </summary>
        /// <param name="single">Строка содержащая число</param>
        /// <returns>Значение или Nan если не удалось выполнить преобразование</returns>
        public static double ParseDouble(string single)
        {
            try
            {
                string ds = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                string value = single;

                value = value.Replace(".", ds);
                value = value.Replace(",", ds);

                return double.Parse(value);
            }
            catch
            { }

            return double.NaN;
        }
    }
}