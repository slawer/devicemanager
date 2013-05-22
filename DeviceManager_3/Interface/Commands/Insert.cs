using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DeviceManager.DevMan;

namespace DeviceManager
{
    public partial class InsertCommand : Form
    {
        public InsertCommand()
        {
            InitializeComponent();
        }

        public TypeCRC typeCRC = TypeCRC.Cycled;

        public string GetResult()
        {
            byte[] packet = null;
            switch (typeCRC)
            {
                case TypeCRC.Cycled:

                    packet = Application.TranslateToUnigueFormatTcpPacket(
                                "@%" + string.Format("{0:x2}", (int)numericUpDownDevNumber.Value) + "070200" + 
                                    string.Format("{0:x2}", (int)numericUpDownDataLen.Value) + "00$", TypeCRC.Cycled);
                    break;

                case TypeCRC.CycledTwo:

                    packet = Application.TranslateToUnigueFormatTcpPacket(
                                "@%" + string.Format("{0:x2}", (int)numericUpDownDevNumber.Value) + "070200" +
                                    string.Format("{0:x2}", (int)numericUpDownDataLen.Value) + "00$", TypeCRC.CycledTwo);
                    break;

                case TypeCRC.CRC16:

                    packet = Application.TranslateToUnigueFormatTcpPacket(
                                "@%" + string.Format("{0:x2}", (int)numericUpDownDevNumber.Value) + "070200" +
                                    string.Format("{0:x2}", (int)numericUpDownDataLen.Value) + "00$", TypeCRC.CRC16);
                    break;

                default:

                    packet = Application.TranslateToUnigueFormatTcpPacket(
                                "@%" + string.Format("{0:x2}", (int)numericUpDownDevNumber.Value) + "070200" +
                                    string.Format("{0:x2}", (int)numericUpDownDataLen.Value) + "00$", TypeCRC.Cycled);
                    break;
            }

            string total = string.Empty;
            foreach (byte item in packet)
            {
                total += string.Format("{0:x2}", item) + " ";
            }
            return total; 
        }

        public byte[] GetResultInByte()
        {
            byte[] packet = null;
            switch (typeCRC)
            {
                case TypeCRC.Cycled:

                    packet = Application.TranslateToUnigueFormatTcpPacket(
                                "@%" + string.Format("{0:x2}", (int)numericUpDownDevNumber.Value) + "070200" +
                                    string.Format("{0:x2}", (int)numericUpDownDataLen.Value) + "00$", TypeCRC.Cycled);
                    break;

                case TypeCRC.CycledTwo:

                    packet = Application.TranslateToUnigueFormatTcpPacket(
                                "@%" + string.Format("{0:x2}", (int)numericUpDownDevNumber.Value) + "070200" +
                                    string.Format("{0:x2}", (int)numericUpDownDataLen.Value) + "00$", TypeCRC.CycledTwo);
                    break;

                case TypeCRC.CRC16:

                    packet = Application.TranslateToUnigueFormatTcpPacket(
                                "@%" + string.Format("{0:x2}", (int)numericUpDownDevNumber.Value) + "070200" +
                                    string.Format("{0:x2}", (int)numericUpDownDataLen.Value) + "00$", TypeCRC.CRC16);
                    break;

                default:

                    packet = Application.TranslateToUnigueFormatTcpPacket(
                                "@%" + string.Format("{0:x2}", (int)numericUpDownDevNumber.Value) + "070200" +
                                    string.Format("{0:x2}", (int)numericUpDownDataLen.Value) + "00$", TypeCRC.Cycled);
                    break;
            }
            return packet;
        }


        public string Commanda
        {
            get { return GetResult().Replace(" ", string.Empty); }
        }

        public string DeviceName
        {
            get { return string.Format("{0:x2}", (int)numericUpDownDevNumber.Value); }
            set 
            {
                try
                {
                    numericUpDownDevNumber.Value = int.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier); 
                }
                catch
                {
                    numericUpDownDevNumber.Value = numericUpDownDevNumber.Minimum;
                }                
            }
        }

        public string DataLen
        {
            get { return string.Format("{0:x2}", (int)numericUpDownDataLen.Value); }
            set 
            {
                try
                {
                    numericUpDownDataLen.Value = int.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier);
                }
                catch
                {
                    numericUpDownDataLen.Value = numericUpDownDataLen.Minimum;
                }
            }
        }

        /// <summary>
        /// Определяет тип используемого порта для отправки команды
        /// </summary>
        public TypePort TypePort
        {
            get
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    return DeviceManager.TypePort.Primary;
                }
                else
                    if (comboBox1.SelectedIndex == 1)
                    {
                        return DeviceManager.TypePort.Secondary;
                    }

                return DeviceManager.TypePort.Default;
            }

            set
            {
                switch (value)
                {
                    case DeviceManager.TypePort.Primary:

                        comboBox1.SelectedIndex = 0;
                        break;

                    case DeviceManager.TypePort.Secondary:

                        comboBox1.SelectedIndex = 1;
                        break;

                    default:

                        comboBox1.SelectedIndex = 2;
                        break;
                }
            }
        }

        public TimeSpan Interval
        {
            get { return new TimeSpan(0, 0, 0, 0, (int)numericUpDownInterval.Value); }
            set { numericUpDownInterval.Value = (decimal)value.TotalMilliseconds; }
        }

        public bool Actived
        {
            get { return checkBoxActived.Checked; }
            set { checkBoxActived.Checked = value; }
        }

        private void InsertCommand_Load(object sender, EventArgs e)
        {
            textBoxNumberHex.Text = string.Format("{0:x2}", (int)numericUpDownDevNumber.Value);
            textBoxDataLenHex.Text = string.Format("{0:x2}", (int)numericUpDownDataLen.Value);

            textBoxResult.Text = GetResult();
        }

        private void numericUpDownDevNumber_ValueChanged(object sender, EventArgs e)
        {
            textBoxNumberHex.Text = string.Format("{0:x2}", (int)numericUpDownDevNumber.Value);
            textBoxDataLenHex.Text = string.Format("{0:x2}", (int)numericUpDownDataLen.Value);

            textBoxResult.Text = GetResult();
        }

        private void numericUpDownDataLen_ValueChanged(object sender, EventArgs e)
        {
            textBoxNumberHex.Text = string.Format("{0:x2}", (int)numericUpDownDevNumber.Value);
            textBoxDataLenHex.Text = string.Format("{0:x2}", (int)numericUpDownDataLen.Value);

            textBoxResult.Text = GetResult();
        }

        public byte[] ComPacket
        {
            get { return GetResultInByte(); }
        }
    }
}