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
    public partial class PortStatisticsForm : Form
    {
        private Application app = null;

        public PortStatisticsForm(Application application)
        {
            InitializeComponent();

            app = application;
        }

        private void PortStatisticsForm_Shown(object sender, EventArgs e)
        {
            timer.Start();
        }

        /// <summary>
        /// выводим статистику
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            textBoxReceivedBytes.Text = app.Serial.ReceivedBytes.ToString();
            textBoxReceivedPackets.Text = app.Serial.ReceivedPackets.ToString();

            textBoxSendingBytes.Text = app.Serial.SendBytes.ToString();
            textBoxSendingPackets.Text = app.Serial.SendPackets.ToString();

            textBoxLostBytes.Text = app.Serial.LostBytes.ToString();
            textBoxStupidParemeterFuckingXYI.Text = app.Serial.LostPackets.ToString();

            textBoxErrorParity.Text = app.Serial.SerialErrorRXParity.ToString();
            textBoxErrorFrame.Text = app.Serial.SerialErrorFrame.ToString();

            textBoxOuterBufferOverflow.Text = app.Serial.SerialErrorRXOver.ToString();
            textBoxOutBufferOverflow.Text = app.Serial.SerialErrorTXFull.ToString();

            textBoxSymbolicBufferOverflow.Text = app.Serial.SerialErrorOverrun.ToString();
        }
    }
}