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
    public partial class ProtocolForm : Form
    {
        private setter se;
        private Application _app = null;

        public ProtocolForm(Application app)
        {
            InitializeComponent();
            
            _app = app;
            se = new setter(setterF);
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProtocolForm_Load(object sender, EventArgs e)
        {
            _app.Serial.OnPacket += new SerialEventHandler(Serial_OnPacket);
            _app.Serial.Secondary.OnPacket += new SerialEventHandler(Serial_OnPacketSecondary);
        }

        /// <summary>
        /// Поступил пакет из порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Serial_OnPacket(object sender, SerialEventArgs args)
        {
            try
            {
                if (args.Packet.ToPort)
                {
                    Invoke(se, args, "Primary   -> ");
                }
            }
            catch { }
        }

        /// <summary>
        /// Поступил пакет из порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Serial_OnPacketSecondary(object sender, SerialEventArgs args)
        {
            try
            {
                if (args.Packet.ToPort)
                {
                    Invoke(se, args, "Secondary -> ");
                }
            }
            catch { }
        }

        private delegate void setter(SerialEventArgs args, string type);
        private void setterF(SerialEventArgs args, string type)
        {
            if (checkBox1.Checked)
            {
                DateTime now = DateTime.Now;
                StringBuilder builder = new StringBuilder();

                builder.AppendFormat("{2}[{0}.{1:D3}]: ", now.ToLongTimeString(), now.Millisecond, type);

                if (args.Packet.Com_Packet != null)
                {
                    foreach (byte item in args.Packet.Com_Packet)
                    {
                        builder.AppendFormat("[{0:x2}] ", item);
                    }
                }

                if (listBox1.Items.Count > 1000)
                {
                    listBox1.Items.RemoveAt(0);
                }
                listBox1.Items.Add(builder.ToString());
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void ProtocolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _app.Serial.OnPacket -= Serial_OnPacket;            
        }            
    }
}