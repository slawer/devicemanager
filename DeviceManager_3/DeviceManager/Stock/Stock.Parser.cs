using System;
using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует сохранение пакетов поступивших от устройств и передачу конвертеру сохраненных пакетов
    /// </summary>
    public partial class Stock
    {
        /// <summary>
        /// Выполнить обработку поступившего пакета
        /// </summary>
        /// <param name="packet">Пакет, который необходимо обработать</param>
        protected void Parse(Packet packet)
        {
            bool blocked = false;
            try
            {
                if (c_mutex.WaitOne(0, false))
                {
                    blocked = true;
                    if (packet.Role == Role.Slave && packet.Content == Content.Sensor)
                    {
                        foreach (Parameter parameter in parameters)
                        {
                            int device = packet.Com_Packet[1] & 0x7F;
                            if (device == parameter.Device)
                            {
                                float value = float.NaN;

                                value = GetFromDsn(packet.Com_Packet, parameter.Device, parameter.Offset,
                                    parameter.Size, parameter.IsLittleEndian);

                                SaveValue(parameter.Position, value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Stock -> Parse", ErrorType.NotFatal));
                }
            }
            finally
            {
                if (blocked) c_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Выполнить обработку поступившего пакета
        /// </summary>
        /// <param name="packet">Пакет, который необходимо обработать</param>
        protected void ParsePassive(Packet packet)
        {
            bool blocked = false;
            try
            {
                if (c_mutex.WaitOne(0, false))
                {
                    blocked = true;
                    foreach (Parameter parameter in parameters)
                    {
                        int device = packet.Com_Packet[1] & 0x7F;
                        bool isFromDevice = ((packet.Com_Packet[1] & 0x80) > 0) ? true : false;                        

                        if (isFromDevice)
                        {
                            if (device == parameter.Device)
                            {
                                float value = float.NaN;

                                value = GetFromDsn(packet.Com_Packet, parameter.Device, parameter.Offset,
                                    parameter.Size, parameter.IsLittleEndian);

                                SaveValue(parameter.Position, value);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (blocked) c_mutex.ReleaseMutex();
            }
        }
        
        /// <summary>
        /// Извлечь данные из пакета Dsn
        /// </summary>
        /// <param name="device">Устройство</param>
        /// <param name="com_packet">Пакет из которого извлечь данные</param>
        /// <param name="offset">Смещение по которому находятся данные</param>
        /// <param name="lenght">Длинна данных в пакете</param>
        /// <param name="IsLittleEndian">Порядок следования байт в пакете</param>
        /// <returns>Извлеченное значени, или float.Nan если не удалось извлечь значение из пакета</returns>
        private float GetFromDsn(byte[] com_packet, int device, int offset, int lenght, bool IsLittleEndian)
        {
            int totalLen = offset + lenght + 6;
            if (totalLen <= com_packet.Length - 2)
            {
                if (offset == 24 && lenght == 4)
                {
                    int a = 10;
                    a = a + 2;
                }
                int pos = 4 - lenght;
                byte[] val = new byte[4];

                Array.Copy(com_packet, offset + 6, val, pos, lenght);
                
                if (IsLittleEndian) Array.Reverse(val);
                return BitConverter.ToInt32(val, 0);
            }
            else
                return float.NaN;
        }

        /// <summary>
        /// Сохранить значение в массиве
        /// </summary>
        /// <param name="position">Позиция в которую сохранить значение</param>
        /// <param name="value">Значение которое сохранить</param>
        private void SaveValue(int position, float value)
        {
            bool blocked = false;
            try
            {
                if (v_mutex.WaitOne(100, false))
                {
                    blocked = true;
                    if (position < slave.Length && position > -1)
                    {
                        slave[position].Value = value;                        
                    }
                    else
                        throw new IndexOutOfRangeException("Stock -> SaveValue -> InvalidPosition");
                }
                else
                    throw new TimeoutException("Stock -> SaveValue -> Timeout Mutex Wait");
            }
            finally
            {
                if (blocked) v_mutex.ReleaseMutex();
            }
        }
    }
}