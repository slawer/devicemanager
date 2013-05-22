using System;
using System.Threading;

using SoftwareDevelopmentKit.Servers.Tcp;

namespace DeviceManager.DevMan
{
    public partial class TcpDevManager
    {
        /// <summary>
        /// Клиент установил соединение с сервером
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        void OnConnect(object sender, ServerEventArgs e)
        {
            TcpDevClient client = new TcpDevClient();
            
            client.Socket = e.Socket;
            client.Socket.SendTimeout = 1000;
            
            client.Share = shareClientMutex;
            client.OnPacket += new TcpDevClient.PacketEventHandler(client_OnPacket);

            bool blocked = false;
            try
            {
                if (mutex.WaitOne(100, false))
                {
                    blocked = true;
                    clients.Add(client);                 
                }
            }
            finally
            {
                if (blocked) mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Клиент разорвал соединение с серверов 
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        void OnDisconnect(object sender, ServerEventArgs e)
        {
            bool blocked = false;
            try
            {
                if (mutex.WaitOne(100, false))
                {
                    blocked = true;
                    foreach (TcpDevClient client in clients)
                    {
                        if (client.Socket.Handle == e.Socket.Handle)
                        {
                            clients.Remove(client);
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (blocked) mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Поступили данные серверу
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        void OnReceive(object sender, ServerReceiveEventArgs e)
        {
            bool blocked = false;
            try
            {
                if (mutex.WaitOne(100, false))
                {
                    blocked = true;
                    foreach (TcpDevClient client in clients)
                    {
                        if (client.Socket.Handle == e.Socket.Handle)
                        {
                            client.Insert(e.DataString);
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (blocked) mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Клиент выделил пакет из TCP
        /// </summary>
        /// <param name="packet"></param>
        void client_OnPacket(string packet)
        {
            if (OnPacket != null)
            {
                Interlocked.Increment(ref countPacketsReceive);
                OnPacket(packet);
            }
        }

        /// <summary>
        /// Отправить пакет по TCP
        /// </summary>
        /// <param name="packet">Пакет который отправить</param>
        /// <returns>Количество отправленных байт</returns>
        public int Send(string packet)
        {
            bool blocked = false;
            try
            {
                if (mutex.WaitOne(100, false))
                {
                    blocked = true;
                    int sendBytes = 0;
                    
                    bool needClear = false;
                    byte[] tcp_packet = System.Text.Encoding.ASCII.GetBytes(packet);

                    foreach (TcpDevClient client in clients)
                    {
                        if (client != null && client.Socket.Connected)
                        {
                            try
                            {
                                sendBytes = client.Socket.Send(tcp_packet);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    client.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                                }
                                catch
                                {
                                    // ...                                   
                                }

                                needClear = true;
                                client.Socket.Close();

                                Interlocked.Increment(ref countBadClients);
                            }
                        }
                        else
                            needClear = true;
                    }

                    if (needClear)
                    {
                        System.Collections.Generic.List<TcpDevClient> badCls = clients.FindAll(Predicate);
                        if (badCls != null)
                        {
                            foreach (TcpDevClient bad in badCls)
                            {
                                clients.Remove(bad);
                            }
                        }
                    }

                    if (sendBytes > 0)
                    {
                        Interlocked.Increment(ref countPacketsSend);
                        Interlocked.Add(ref sendingBytes, packet.Length);
                    }

                    return sendBytes;
                }
                else
                    throw new TimeoutException();
            }
            finally
            {
                if (blocked) mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// проверить клиента на валидность
        /// </summary>
        /// <param name="obj">Проверяемый клиент</param>
        /// <returns>Истина - если клиент правильный, в противном случае ложь</returns>
        private bool Predicate(TcpDevClient obj)
        {
            try
            {
                if (obj != null)
                {
                    if (obj.Socket != null)
                    {
                        return !(obj.Socket.Connected);
                    }
                }
            }
            catch { }
            return true;
        }
    }
}