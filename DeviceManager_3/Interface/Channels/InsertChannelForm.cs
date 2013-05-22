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
    public partial class InsertChannelForm : Form
    {
        public InsertChannelForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Номер устройства
        /// </summary>
        public int Device
        {
            get { return (int)numericUpDownDeviceNumber.Value; }
            set 
            {
                if (value >= numericUpDownDeviceNumber.Minimum && value <= numericUpDownDeviceNumber.Maximum)
                {
                    numericUpDownDeviceNumber.Value = value;
                }
                else
                    numericUpDownDeviceNumber.Value = numericUpDownDeviceNumber.Minimum;
            }
        }

        /// <summary>
        /// Смещение в пакете
        /// </summary>
        public int Offset
        {
            get { return (int)numericUpDownOffset.Value; }
            set 
            {
                if (value >= numericUpDownOffset.Minimum && value <= numericUpDownOffset.Maximum)
                {
                    numericUpDownOffset.Value = value;
                }
                else
                    numericUpDownOffset.Value = numericUpDownOffset.Minimum;
            }
        }

        /// <summary>
        /// Размер данных
        /// </summary>
        public int DataSize
        {
            get { return (int)numericUpDownDataSize.Value; }
            set 
            {
                if (value >= numericUpDownDataSize.Minimum && value <= numericUpDownDataSize.Maximum)
                {
                    numericUpDownDataSize.Value = value;
                }
                else
                    numericUpDownDataSize.Value = numericUpDownDataSize.Minimum;
            }
        }

        /// <summary>
        /// Описание канала
        /// </summary>
        public string Comment
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
    }
}