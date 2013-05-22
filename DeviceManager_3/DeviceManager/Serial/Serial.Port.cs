using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

using DeviceManager.Errors;

namespace DeviceManager
{
    /// <summary>
    /// Реализует обмен с устройствами через последовательный порт
    /// </summary>
    public partial class Serial
    {
        // ---- данные для процедуры чтения/ записи ----

        private ReaderWriterLockSlim opt_slim;              // синхронизирует доступ к параметрам

        private int attemptsToRead;                         // количество попыток чтения пакета
        private int attemptsCycled;                         // количество попыток чтения данных пакета

        private int waitTimeout;                            // время между посылками пакетов в порт

        private byte[] response = null;                     // указатель на полученный пакет от устройства
        
        private Packet[] s_list = null;                     // статические пакеты для отправки
        private Packet[] d_list = null;                     // динамические пакеты для отправки

        /// <summary>
        /// Обработчик таймерного события
        /// </summary>
        /// <param name="state">Объект, содержащий информацию, используемую методом ответного 
        /// вызова или значение null.</param>
        protected virtual void TimerElapsed(object state)
        {
            bool blocked = false;
            try
            {
                if (t_mutex.WaitOne(0, false))
                {
                    blocked = true;
                    InitializeList();

                    ApplicationMode mode = app.Mode;
                    switch (mode)
                    {
                        case ApplicationMode.Active:

                            ActiveWork();
                            break;

                        case ApplicationMode.Passive:

                            foreach (var packet in d_list)
                            {
                                if (packet.IsActived)
                                {
                                    if (OnPacket != null)
                                    {
                                        OnPacket(this, new SerialEventArgs(packet));
                                    }
                                }
                            }

                            break;

                        case ApplicationMode.Emulated:

                            foreach (var packet in d_list)
                            {
                                if (packet.IsActived)
                                {
                                    if (OnPacket != null)
                                    {
                                        OnPacket(this, new SerialEventArgs(packet));
                                    }
                                }
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Serial -> TimerElapsed", ErrorType.Unknown));
                }
            }
            finally
            {
                if (blocked) t_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Метод, выполняющий обработку при получении данных из порта
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Передаваемы данные</param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                switch (e.EventType)
                {
                    case SerialData.Chars:

                        if (app.Mode == ApplicationMode.Passive)
                        {
                            byte[] resp = new byte[port.BytesToRead];
                            int readed = port.Read(resp, 0, resp.Length);

                            if (state == State.Running)
                            {
                                PassiveWork(resp);
                            }

                            port.DiscardInBuffer();
                            port.DiscardOutBuffer();
                        }
                        else
                        {
                            response = GetResponse();
                            if (answerWaiter.WaitOne(0, false))
                            {
                                if (response != null)
                                {
                                    Interlocked.Increment(ref c_received_packets);
                                    Interlocked.Add(ref c_received_bytes, response.Length);

                                    if (OnPacket != null)
                                    {
                                        Packet packet = new Packet();
                                        packet.Com_Packet = response;

                                        packet.Role = Role.Slave;
                                        packet.Content = Content.Unknown;

                                        OnPacket(this, new SerialEventArgs(packet));
                                    }
                                }
                            }
                            else
                                answerWaiter.Set();
                        }
                        break;

                    case SerialData.Eof:

                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Serial -> DataReceived", ErrorType.NotFatal));
                }
            }
        }

        /// <summary>
        /// Выполняем обработку ошибки порта
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        protected void ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            switch (e.EventType)
            {
                case SerialError.Frame:

                    Interlocked.Increment(ref SerrialErrorFrame);
                    break;

                case SerialError.Overrun:

                    Interlocked.Increment(ref SerrialErrorOverrun);
                    break;

                case SerialError.RXOver:

                    Interlocked.Increment(ref SerrialErrorRXOver);
                    break;

                case SerialError.RXParity:

                    Interlocked.Increment(ref SerrialErrorRXParity);
                    break;

                case SerialError.TXFull:

                    Interlocked.Increment(ref SerrialErrorTXFull);
                    break;

                default:

                    Interlocked.Increment(ref SerrialErrorUnknown);
                    break;
            }
        }

        /// <summary>
        /// Ожидать ответ
        /// </summary>
        /// <param name="item">Отправленный пакет</param>
        private void WaitAnswer(Packet item)
        {
            try
            {
                answerWaiter.Reset();
                if (answerWaiter.WaitOne(answerTimeout, false))    // ожидаем ответ от устройства
                {
                    // получен ответ от устройства

                    if (response != null)
                    {
                        Interlocked.Increment(ref c_received_packets);
                        Interlocked.Add(ref c_received_bytes, response.Length);

                        if (OnPacket != null)
                        {
                            Packet packet = new Packet();
                            packet.Com_Packet = response;

                            packet.Role = Role.Slave;
                            packet.Content = item.Content;

                            OnPacket(this, new SerialEventArgs(packet));
                        }
                    }
                }
                else
                {
                    // ответ от устройства не получен
                    Interlocked.Increment(ref c_lost_packets);      // количество не ответов
                }
            }
            catch (Exception ex)
            {
                // ... не смогли считать данные из порта ...

                response = null;
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Serial -> WaitAnswer", ErrorType.NotFatal));
                }
            }
        }

        /// <summary>
        /// Получить ответ от устройства
        /// </summary>
        /// <returns>Полученный пакет от устройства</returns>
        private byte[] GetResponse()
        {
            try
            {
                byte[] resp = new byte[512];

                int Offset = 0, counter = 0;
                int BytesToRead = port.BytesToRead;

                try
                {
                    int readed = port.Read(resp, Offset, BytesToRead);
                    Offset = Offset + readed;
                }
                catch (Exception ex)
                {
                    if (OnError != null)
                    {
                        OnError(this, new ErrorArgs(ex.Message + "Serial -> GetResponse -> int readed = port.Read(resp, Offset, BytesToRead);", ErrorType.NotFatal));
                    }
                    return null;
                }

                int attCycle = AttemptsCycled;
                for (int current = 0; current < attCycle; current++)
                {
                    int packet_size = ParseDsn(resp, Offset);
                    if (packet_size > 0)
                    {
                        Array.Resize(ref resp, packet_size);
                        return resp;
                    }
                    else
                    {
                        try
                        {
                            resp[Offset] = (byte)port.ReadByte();

                            Offset++;
                            counter = 0;
                        }
                        catch (TimeoutException)
                        {
                            if (counter == AttemptsToRead) return null;
                            else counter = counter + 1;

                            continue;
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs(ex.Message + "Serial -> GetResponse", ErrorType.NotFatal));
                }
            }
            return null;
        }

        /// <summary>
        /// Выполнить инициализацию списка
        /// </summary>
        protected void InitializeList()
        {
            s_list = StaticPackets;
            if (t_port == TypePort.Primary)
            {
                d_list = repository.Packets;
                if (app.Mode != ApplicationMode.Emulated)
                {
                    if (Secondary.IsActived && d_list != null && d_list.Length > 0)
                    {
                        if (Secondary.is_slim.TryEnterWriteLock(300))
                        {
                            try
                            {
                                if (Secondary.d_list == null || d_list.Length == 0)
                                {
                                    Secondary.d_list = new Packet[d_list.Length];
                                    d_list.CopyTo(Secondary.d_list, 0);
                                }
                                else
                                {
                                    if (d_list != null && d_list.Length > 0)
                                    {
                                        int desInd = Secondary.d_list.Length;

                                        Array.Resize(ref Secondary.d_list, Secondary.d_list.Length + d_list.Length);
                                        Array.Copy(d_list, 0, Secondary.d_list, desInd, d_list.Length);
                                    }
                                }
                            }
                            finally
                            {
                                Secondary.is_slim.ExitWriteLock();
                            }
                        }
                    }
                }
                else
                {
                    if (Secondary.d_list != null)
                    {
                        Secondary.d_list = null;
                    }
                }
            }
        }

        /// <summary>
        /// Проверить нужли данный пакет отправлять в порт
        /// </summary>
        /// <param name="packet">Проверяемый пакет</param>
        /// <returns>true - если данный пакет нужно отправить в порт, false - если не нужно отправлять пакет в порт</returns>
        protected bool IsNeedToPort(Packet packet)
        {
            if (packet.Sended == false)
            {
                if (packet.PortType == t_port)
                {
                    if (packet.Role == Role.Slave)
                    {
                        return true;
                    }
                }

                if (s_list.Length > 0)
                {
                    foreach (Packet p in s_list)
                    {
                        if (p.PortType == t_port)
                        {
                            if (p.Com_Packet[1] == packet.Com_Packet[1])
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                    return true;
            }            
            return false;
        }

        /// <summary>
        /// Обработать пакеты применяя логику активного режима работы
        /// </summary>
        protected void ActiveWork()
        {
            try
            {                
                if (t_port == TypePort.Secondary && is_slim.TryEnterWriteLock(20))
                {
                    try
                    {
                        if (d_list != null)
                        {
                            if (d_list.Length > 0)
                            {
                                // первым обрабатываем димамический список
                                foreach (var packet in d_list)
                                {
                                    if (packet.IsActived && packet.ToPort)
                                    {
                                        packet.Source = PacketSource.Dynamic;
                                        if (IsNeedToPort(packet))
                                        {
                                            packet.Sended = true;
                                            Write(packet);
                                        }
                                    }
                                    else
                                        if (packet.IsActived && !packet.ToPort)
                                        {
                                            if (OnPacket != null) OnPacket(this, new SerialEventArgs(packet));
                                        }
                                }

                                if (OnDynamicComplete != null)
                                {
                                    OnDynamicComplete(this, null);
                                }
                            }
                        }
                    }
                    finally
                    {
                        d_list = null;
                        is_slim.ExitWriteLock();
                    }
                }
                else
                {
                    if (t_port == TypePort.Primary)
                    {
                        if (d_list != null)
                        {
                            if (d_list.Length > 0)
                            {
                                // первым обрабатываем димамический список
                                foreach (var packet in d_list)
                                {
                                    if (packet.IsActived && packet.ToPort)
                                    {
                                        packet.Source = PacketSource.Dynamic;

                                        if (Secondary.IsActived)
                                        {
                                            if (IsNeedToPort(packet))
                                            {
                                                packet.Sended = true;
                                                Write(packet);
                                            }
                                        }
                                        else
                                        {
                                            packet.Sended = true;
                                            Write(packet);
                                        }
                                    }
                                    else
                                        if (packet.IsActived && !packet.ToPort)
                                        {
                                            if (OnPacket != null) OnPacket(this, new SerialEventArgs(packet));
                                        }
                                }

                                if (OnDynamicComplete != null)
                                {
                                    OnDynamicComplete(this, null);
                                }
                            }
                        }
                    }
                }

                if (s_list != null)
                {
                    if (s_list.Length > 0)
                    {
                        bool written = false;
                        // вторым обрабатываем статический список
                        foreach (var packet in s_list)
                        {
                            packet.Source = PacketSource.Static;
                            if (packet.IsActived)
                            {
                                if (t_port == TypePort.Primary)
                                {   
                                    if (!Secondary.IsActived)
                                    {
                                        DateTime now = DateTime.Now;
                                        TimeSpan span = now - packet.LastTime;

                                        if (span >= packet.Interval)
                                        {
                                            Write(packet);
                                            packet.LastTime = now;

                                            written = true;
                                        }
                                    }
                                    else
                                    {
                                        if (t_port == packet.PortType)
                                        {
                                            DateTime now = DateTime.Now;
                                            TimeSpan span = now - packet.LastTime;

                                            if (span >= packet.Interval)
                                            {
                                                Write(packet);
                                                packet.LastTime = now;

                                                written = true;
                                            }
                                        }
                                    }
                                }
                                else
                                    if (t_port == packet.PortType)
                                    {
                                        DateTime now = DateTime.Now;
                                        TimeSpan span = now - packet.LastTime;

                                        if (span >= packet.Interval)
                                        {
                                            Write(packet);
                                            packet.LastTime = now;

                                            written = true;
                                        }
                                    }
                            }
                        }

                        if (written == true)
                        {
                            if (OnStaticComplete != null)
                            {
                                OnStaticComplete(this, null);
                            }
                        }
                    }
                }

                if (OnComplete != null)
                {
                    OnComplete(this, null);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, new ErrorArgs("Serial -> ActiveWork + " + ex.Message + ex.StackTrace,
                        ErrorType.NotFatal));
                }
            }
        }

        /// <summary>
        /// Обработать пакет
        /// </summary>
        /// <param name="packet">Пакет для обработки</param>
        private void Write(Packet packet)
        {
            try
            {
                port.DiscardInBuffer();                                                 // выполняем очистку
                port.DiscardOutBuffer();                                                // входного и выходного буфера

                port.Write(packet.Com_Packet, 0, packet.Com_Packet.Length);             // записываем пакет в порт
                if (OnPacket != null) OnPacket(this, new SerialEventArgs(packet));      // передаем пакет наружу

                Interlocked.Increment(ref c_send_packets);                              // увеличиваем соответствующие 
                Interlocked.Add(ref c_send_bytes, packet.Com_Packet.Length);            // счетчики

                if (packet.Wait) WaitAnswer(packet);                                    // если необходимо, ожидаем ответ на пакет                
                if (WaitTimeout >= 10)
                {
                    Thread.Sleep(WaitTimeout);
                }
            }
            catch
            {
                // ... предпологается что не смогли записать в порт пакет ...

                if (OnPortFail != null)
                {
                    OnPortFail(this, new EventArgs());
                }

                return;
            }
        }
    }
}