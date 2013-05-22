using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace DeviceManager
{
    public partial class ComSecondaryOptions : Form
    {
        public ComSecondaryOptions()
        {
            InitializeComponent();
        }

        private string portname = string.Empty;

        /// <summary>
        /// Используемый порт
        /// </summary>
        public string PortName
        {
            get { return comboBoxPortNames.Text; }
            set { portname = value; }
        }


        /// <summary>
        /// Определяет используется порт или нет
        /// </summary>
        public Boolean IsUsePort
        {
            get { return checkBox1.Checked; }
            set { checkBox1.Checked = value; }
        }

        private void ComOptions_Load(object sender, EventArgs e)
        {
            int ind = 0;
            int indexSelect = -1;

            foreach (string port in SerialPort.GetPortNames())
            {
                if (port == portname)
                {
                    indexSelect = ind;
                }

                ind = ind + 1;
                comboBoxPortNames.Items.Add(port);
            }

            if (indexSelect != -1)
            {
                comboBoxPortNames.SelectedIndex = indexSelect;
            }
        }

        /// <summary>
        /// проверяем не совпадает ли номер порта второстепенного с основным
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Click(object sender, EventArgs e)
        {
            Application _app = Application.CreateInstance();
            if (_app != null)
            {
                if (_app.Serial.Port != null)
                {
                    if (checkBox1.Checked)
                    {
                        if (_app.Serial.Port.PortName == comboBoxPortNames.SelectedItem.ToString())
                        {
                            MessageBox.Show(this, "Номер вспомогательно порта не должен совпадать с номером основного порта",
                                "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            DialogResult = System.Windows.Forms.DialogResult.None;
                            return;
                        }
                    }
                }
            }
        }
    }
}