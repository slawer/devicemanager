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
    public partial class PortStatisticsForm2 : Form
    {
        private Application app = null;

        public PortStatisticsForm2(Application application)
        {
            InitializeComponent();

            app = application;
        }

        private void PortStatisticsForm2_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }

        /// <summary>
        /// выводим статистику второго порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            textBoxReceivedBytes.Text = app.Serial.Secondary.ReceivedBytes.ToString();
            textBoxReceivedPackets.Text = app.Serial.Secondary.ReceivedPackets.ToString();

            textBoxSendingBytes.Text = app.Serial.Secondary.SendBytes.ToString();
            textBoxSendingPackets.Text = app.Serial.Secondary.SendPackets.ToString();

            textBoxLostBytes.Text = app.Serial.Secondary.LostBytes.ToString();
            textBoxStupidParemeterFuckingXYI.Text = app.Serial.Secondary.LostPackets.ToString();

            textBoxErrorParity.Text = app.Serial.Secondary.SerialErrorRXParity.ToString();
            textBoxErrorFrame.Text = app.Serial.Secondary.SerialErrorFrame.ToString();

            textBoxOuterBufferOverflow.Text = app.Serial.Secondary.SerialErrorRXOver.ToString();
            textBoxOutBufferOverflow.Text = app.Serial.Secondary.SerialErrorTXFull.ToString();

            textBoxSymbolicBufferOverflow.Text = app.Serial.Secondary.SerialErrorOverrun.ToString();
        }

    }
}
