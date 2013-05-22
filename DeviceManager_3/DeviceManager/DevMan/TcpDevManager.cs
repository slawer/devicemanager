using System;
using System.Threading;
using System.Collections.Generic;

using DeviceManager;
//using SoftwareDevelopmentKit.Servers.Tcp35;
using SoftwareDevelopmentKit.Servers.Tcp;

namespace DeviceManager.DevMan
{
    /// <summary>
    /// Реализует обмен пакетами по Tcp соединению 
    /// в старом режиме, по порту протоколу Dsn и порту 56000
    /// </summary>
    public partial class TcpDevManager
    {
        // ----- Данные класса -------

        private TcpServer server;                   // основной TCP сервер

        //private Server server;                      // основной TCP сервер
        private List<TcpDevClient> clients;         // клиенты подключеннык к Tcp серверу

        private int m_port = 56000;                 // порт обмена
        private int m_numConnections = 10;          // количество одновременно подключенных клиентов
        private int m_receiveBuferSize = 512;       // размер буфера приема данных

        private Mutex mutex = null;                 // синхронизирует доступ к списку подключенных клиентов 
        private Mutex shareClientMutex;             // синхронизирует доступ к разделяемым данным для клиентов сервера

        private Place place = null;                 // место в репозитарии

        /// <summary>
        /// Возникает при получении пакета
        /// </summary>
        public event PacketEventHandler OnPacket;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public TcpDevManager()
        {
            server = new TcpServer();

            //server = new Server();
            clients = new List<TcpDevClient>();

            mutex = new Mutex(false);
            shareClientMutex = new Mutex(false);
        }

        /// <summary>
        /// Место в репозитарии
        /// </summary>
        public Place Place
        {
            get { return place; }
            set { place = value; }
        }

        /// <summary>
        /// Определяет количество одновременно подключенных клиентов
        /// </summary>
        public int CountConnections
        {
            get { return m_numConnections; }
            set { m_numConnections = value; }
        }

        /// <summary>
        /// Определяет размер буфера приема данных
        /// </summary>
        public int ReceiverBufferSize
        {
            get { return m_receiveBuferSize; }
            set { m_receiveBuferSize = value; }
        }

        /// <summary>
        /// Определяет порт обмена
        /// </summary>
        public int Port
        {
            get { return m_port; }
            set { m_port = value; }
        }

        /// <summary>
        /// Количество подключенных клиентов в данный момент
        /// </summary>
        public int CountConnected
        {
            get 
            { 
                return server.NumberOfConnectedClients; 
                //return (int)server.ConnectedSockets;
            }
        }

        /// <summary>
        /// Tcp сервер, осуществляющий работу с сокетами
        /// </summary>
        public TcpServer Server
        {
            get { return server; }
        }

        /// <summary>
        /// Запустить Tcp сервер
        /// </summary>
        public void Start()
        {
            server = new TcpServer(m_numConnections, m_receiveBuferSize);

            server.OnConnect += new ServerEventHandler(OnConnect);
            server.OnDisconnect += new ServerEventHandler(OnDisconnect);
            
            server.OnReceive += new ServerReceiveEventHandler(OnReceive);

            server.Port = Port;
            server.Start();

            /*server = new Server();

            server.Port = Port;
            server.maxAcceptedClients = m_numConnections;

            server.OnConnect += new ServerEventHandler(server_OnConnect);
            server.OnDisconnect += new ServerEventHandler(server_OnDisconnect);

            server.OnReceive += new ServerReceiveEventHandler(server_OnReceive);
            server.Run();*/
        }

        /// <summary>
        /// Подключился к серверу клиент
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_OnConnect(object sender, ServerEventArgs e)
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
        /// Разорвал с серверос соединение клиент
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_OnDisconnect(object sender, ServerEventArgs e)
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
        /// Полученны данные от клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_OnReceive(object sender, ServerReceiveEventArgs e)
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
        /// Остановить работу сервера
        /// </summary>
        public void Stop()
        {
            bool blocked = false;
            try
            {
                if (mutex.WaitOne(100, false))
                {
                    blocked = true;                 
                    server.Stop();

                    foreach (TcpDevClient client in clients)
                    {
                        client.Socket.Close();
                    }
                }
            }
            finally
            {
                if (blocked) mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Определяет интерфейс функции обрабатывающей событие OnPacket
        /// </summary>
        /// <param name="packet">Пакет</param>
        public delegate void PacketEventHandler(string packet);
    }
}