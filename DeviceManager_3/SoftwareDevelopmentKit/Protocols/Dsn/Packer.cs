using System;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using SoftwareDevelopmentKit.Tcp.Client;

namespace SoftwareDevelopmentKit.Protocols.Dsn
{
    public class Packer
    {
        // ------- Данные класса -----------

        private TcpClient client = null;

        private StringBuilder input = null;
        private StringBuilder output = null;

        private Timer timer = null;
        private Mutex mutex = null;

        // ------- События --------

        private bool working = false;
        public event PacketEventHandler OnPacket;

        // ------- Конструктор --------------

        public Packer(TcpClient Client)
        {
            client = Client;
            client.OnReceive += new ReceiveEventHandler(client_OnReceive);

            input = new StringBuilder();
            output = new StringBuilder();

            timer = new Timer(new TimerCallback(TranslaterFunction), null, Timeout.Infinite, 10);
            mutex = new Mutex(false);
        }

        // ------ Обработка данных -------

        void client_OnReceive(object sender, byte[] data)
        {
            lock (input)
            {
                if (!working)
                {
                    working = true;
                    timer.Change(0, 100);
                }
                input.Append(Encoding.ASCII.GetString(data));
            }
        }

        // -------- транслятор -----------

        private void TranslaterFunction(object state)
        {
            bool flag = false;
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    flag = true;
                    lock (input)
                    {
                        output.Append(input.ToString());
                        input.Remove(0, input.Length);
                    }

                    Regex regex = new Regex("@%[0-9A-Fa-f]*[\\$]", RegexOptions.IgnoreCase);
                    if (regex.IsMatch(output.ToString()))
                    {
                        MatchCollection colection = regex.Matches(output.ToString());
                        foreach (Match match in colection)
                        {
                            if (OnPacket != null) OnPacket(match.Value);
                            output.Replace(match.Value, string.Empty);
                        }
                    }
                    flag = false;
                    mutex.ReleaseMutex();
                }
            }
            finally
            {
                if (flag) mutex.ReleaseMutex();
            }
        }

        // -------- Делегаты ---------

        private delegate void Translater();
        public delegate void PacketEventHandler(string packet);
    }
}