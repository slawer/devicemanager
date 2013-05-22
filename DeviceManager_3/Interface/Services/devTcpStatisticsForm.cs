using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceManager
{
    public partial class devTcpStatisticsForm : Form
    {
        private Application app = null;

        public devTcpStatisticsForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        private void devTcpStatisticsForm_Shown(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBoxReceivedBytesTCP.Text = app.TcpDevManager.TotalBytesRead.ToString();
            textBoxSendingBytesTCP.Text = app.TcpDevManager.SendingBytes.ToString();

            textBoxReceivedPacketsTCP.Text = app.TcpDevManager.PacketsReceive.ToString();
            textBoxSendingPacketsTCP.Text = app.TcpDevManager.PacketsSend.ToString();

            textBoxCountFaillClients.Text = app.TcpDevManager.CountBadCliens.ToString();
            textBoxCountConnectedClients.Text = app.TcpDevManager.CountConnected.ToString();

        }
    }
}