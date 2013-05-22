using System;

namespace SoftwareDevelopmentKit.Protocols.Dsn
{
    /// <summary>
    /// Интерфейс определяющий методы, который должен реализовать класс, наследующий данный интерфейс
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// Получить линейный адресс (поле LADD)
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Линейный адресс в виде строки(два байта в HEX формате)</returns>
        string GetLadd(string packet);

        /// <summary>
        /// Получить длинну пакета (поле L_PAK)
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Длинна пакета в виде строки(два байта в HEX формате)</returns>
        string GetL_Pack(string packet);

        /// <summary>
        /// Получить команду (поле CMD)
        /// </summary>
        /// <param name="packet"></param>
        /// <returns>Команда в виде строки(два байта в HEX формате)</returns>
        string GetCmd(string packet);

        /// <summary>
        /// Получить адресс (поле ADR)
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Адресс в виде строки(два байта в HEX формате)</returns>
        string GetAdr(string packet);

        /// <summary>
        /// Получить длинну поля данных (поле LDATA)
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Длинна поля данных в виде строки(два байта в HEX формате)</returns>
        string GetLData(string packet);

        /// <summary>
        /// Получить данных, находящиеся в пакете
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Данные в виде строки(HEX формате)</returns>
        string GetData(string packet);

        /// <summary>
        /// Получить статус
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Статус в виде строки(два байта в HEX формате)</returns>
        string GetStatus(string packet);

        /// <summary>
        /// Определяет пакет предназначен для устройства
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>true - пакет для устройства, false - нет</returns>
        bool IsToDevice(string packet);
        
        /// <summary>
        /// Определяет источником данного пакета являеься устройство
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>true - пакет поступил от устройства, false - нет</returns>
        bool IsFromDevice(string packet);   
     
        /// <summary>
        /// Получить номер устройства
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Номер устройства</returns>
        int GetNumberDevice(string packet);

        /// <summary>
        /// Определить данный пакет содержит команду на чтение
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>true - если содержит команду на чтение, false - в противном случае</returns>
        bool IsRead(string packet);
        
        /// <summary>
        /// Определить данный пакет содержит команду на запись
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>true - если содержит команду на запись, false - в противном случае</returns>
        bool IsWrite(string packet);

        /// <summary>
        /// Определяет номер страницы
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns>Номер сраницы</returns>
        int PageAdress(string packet);

        /// <summary>
        /// Генерирует пакет
        /// </summary>
        /// <param name="Number">Номер устройства</param>
        /// <param name="Cmd">Команда на чтение/запись/чтение-запись</param>
        /// <param name="pNumber">Номер страницы</param>
        /// <param name="Offset">Смещение на странице</param>
        /// <param name="lenghtOfData">Длинна данных</param>
        /// <param name="Data">Данные</param>
        /// <returns>Строка, содержащая сгенерированную команду</returns>
        string CreateCommand(Device Number, Command Cmd, PageNumber pNumber, int Offset, int lenghtOfData, string Data);

        /// <summary>
        /// Генерирует пакет
        /// </summary>
        /// <param name="Number">Номер устройства</param>
        /// <param name="Cmd">Команда на чтение/запись/чтение-запись</param>
        /// <param name="pNumber">Номер страницы</param>
        /// <param name="Offset">Смещение на странице</param>
        /// <param name="lenghtOfData">Длинна данных</param>
        /// <param name="Data">Данные</param>
        /// <returns>Строка, содержащая сгенерированную команду</returns>
        string CreateCommand(int Number, Command Cmd, int pNumber, int Offset, int lenghtOfData, string Data);

        /// <summary>
        /// Получить данные пакета в виде массива байтов
        /// </summary>
        /// <param name="packet">Пакет из которого извлечь данные</param>
        /// <returns>Массив байтов</returns>
        byte[] GetDataBytes(string packet);
    }

    /// <summary>
    /// Перечесление номеров устройств
    /// </summary>
    public enum Device
    {
        /// <summary>
        /// Устройство номер 1 
        /// </summary>
        D1, 
        
        /// <summary>
        /// Устройство номер 2
        /// </summary>
        D2, 
        
        /// <summary>
        /// Устройство номер 3
        /// </summary>
        D3, 
        
        /// <summary>
        /// Устройство номер 4
        /// </summary>
        D4, 
        
        /// <summary>
        /// Устройство номер 5
        /// </summary>
        
        
        D5, 
        
        /// <summary>
        /// Устройство номер 6
        /// </summary>
        D6, 
        
        /// <summary>
        /// Устройство номер 7
        /// </summary>
        D7, 
        
        /// <summary>
        /// Устройство номер 8
        /// </summary>
        D8, 
        
        /// <summary>
        /// Устройство номер 9
        /// </summary>
        D9, 
        
        /// <summary>
        /// Устройство номер 10
        /// </summary>
        D10, 
        
        /// <summary>
        /// Устройство номер 11
        /// </summary>        
        D11, 
        
        /// <summary>
        /// Устройство номер 12
        /// </summary>
        D12, 
        
        /// <summary>
        /// Устройство номер 13
        /// </summary>
        D13, 
        
        /// <summary>
        /// Устройство номер 14
        /// </summary>
        D14, 
        
        /// <summary>
        /// Устройство номер 15
        /// </summary>
        D15, 
        
        /// <summary>
        /// Устройство номер 16
        /// </summary>
        D16, 
        
        /// <summary>
        /// Устройство номер 17
        /// </summary>
        D17, 
        
        /// <summary>
        /// Устройство номер 18
        /// </summary>
        D18, 
        
        /// <summary>
        /// Устройство номер 19
        /// </summary>
        D19, 
        
        /// <summary>
        /// Устройство номер 20
        /// </summary>
        D20,
        
        /// <summary>
        /// Устройство номер 21
        /// </summary>
        D21, 
        
        /// <summary>
        /// Устройство номер 22
        /// </summary>
        D22, 
        
        /// <summary>
        /// Устройство номер 23
        /// </summary>
        D23, 
        
        /// <summary>
        /// Устройство номер 24
        /// </summary>
        D24, 
        
        /// <summary>
        /// Устройство номер 25
        /// </summary>
        D25, 
        
        /// <summary>
        /// Устройство номер 26
        /// </summary>
        D26, 
        
        /// <summary>
        /// Устройство номер 27
        /// </summary>
        D27, 
        
        /// <summary>
        /// Устройство номер 28
        /// </summary>
        D28, 
        
        /// <summary>
        /// Устройство номер 29
        /// </summary>
        D29, 
        
        /// <summary>
        /// Устройство номер 30
        /// </summary>
        D30, 
        
        /// <summary>
        /// Устройство номер 31
        /// </summary>
        D31,

        /// <summary>
        /// Устройство номер 32
        /// </summary>
        D32,

        /// <summary>
        /// Устройство номер 33
        /// </summary>
        D33,

        /// <summary>
        /// Устройство номер 34
        /// </summary>
        D34,

        /// <summary>
        /// Устройство номер 35
        /// </summary>
        D35,

        /// <summary>
        /// Устройство номер 36
        /// </summary>
        D36,

        /// <summary>
        /// Устройство номер 37
        /// </summary>
        D37,

        /// <summary>
        /// Устройство номер 38
        /// </summary>
        D38,

        /// <summary>
        /// Устройство номер 39
        /// </summary>
        D39,

        /// <summary>
        /// Устройство номер 40
        /// </summary>
        D40,

        /// <summary>
        /// Устройство номер 41
        /// </summary>
        D41,

        /// <summary>
        /// Устройство номер 42
        /// </summary>
        D42,

        /// <summary>
        /// Устройство номер 43
        /// </summary>
        D43,

        /// <summary>
        /// Устройство номер 44
        /// </summary>
        D44,

        /// <summary>
        /// Устройство номер 45
        /// </summary>
        D45,

        /// <summary>
        /// Устройство номер 46
        /// </summary>
        D46,

        /// <summary>
        /// Устройство номер 47
        /// </summary>
        D47,

        /// <summary>
        /// Устройство номер 48
        /// </summary>
        D48,

        /// <summary>
        /// Устройство номер 49
        /// </summary>
        D49,

        /// <summary>
        /// Устройство номер 50
        /// </summary>
        D50,

        /// <summary>
        /// Устройство номер 51
        /// </summary>
        D51,

        /// <summary>
        /// Устройство номер 52
        /// </summary>
        D52,

        /// <summary>
        /// Устройство номер 53
        /// </summary>
        D53,

        /// <summary>
        /// Устройство номер 54
        /// </summary>
        D54,

        /// <summary>
        /// Устройство номер 55
        /// </summary>
        D55,

        /// <summary>
        /// Устройство номер 56
        /// </summary>
        D56,

        /// <summary>
        /// Устройство номер 57
        /// </summary>
        D57,

        /// <summary>
        /// Устройство номер 58
        /// </summary>
        D58,

        /// <summary>
        /// Устройство номер 59
        /// </summary>
        D59,

        /// <summary>
        /// Устройство номер 60
        /// </summary>
        D60,

        /// <summary>
        /// Устройство номер 61
        /// </summary>
        D61,

        /// <summary>
        /// Широковещательный адресс
        /// </summary>
        D3F
    }

    /// <summary>
    /// Перечесление типов команд
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// Чтение
        /// </summary>
        Read, 
        
        /// <summary>
        /// Запись
        /// </summary>
        Write, 
        
        /// <summary>
        /// чтение-запись
        /// </summary>
        ReadWrite,

        /// <summary>
        /// Установить время
        /// </summary>
        SetTime
    }

    /// <summary>
    /// Перечесление номеров страниц
    /// </summary>
    public enum PageNumber
    {
        /// <summary>
        /// нулевая страница
        /// </summary>
        P0, 
        
        /// <summary>
        /// первая страница
        /// </summary>
        P1, 
        
        /// <summary>
        /// Вторая страница
        /// </summary>
        P2, 
        
        /// <summary>
        /// Третья страница
        /// </summary>
        P3,
        
        /// <summary>
        /// Четвертая страница
        /// </summary>
        P4, 
        
        /// <summary>
        /// Пятая страница
        /// </summary>
        P5, 
        
        /// <summary>
        /// Шестая страница
        /// </summary>
        P6, 
        
        /// <summary>
        /// Седьмая страница
        /// </summary>
        P7
    }
}