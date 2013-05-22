using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace DeviceManager
{
    public partial class ComOptions : Form
    {
        public ComOptions()
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
        /// Получает или задает размер входного буфера 
        /// </summary>
        public int SizeOfReadBuffer
        {
            get { return int.Parse(comboBoxBufferRead.Text); }
            set { comboBoxBufferRead.Text = value.ToString(); }
        }

        /// <summary>
        /// Получает или задает размер выходного буфера последовательного порта
        /// </summary>
        public int SizeOfWriteBuffer
        {
            get { return int.Parse(comboBoxBufferWrite.Text); }
            set { comboBoxBufferWrite.Text = value.ToString(); }
        }

        /// <summary>
        /// Получает или задает скорость передачи для последовательного порта (в бодах).
        /// </summary>
        public int BaudRate
        {
            get { return int.Parse(comboBoxBaudRate.Text); }
            set { comboBoxBaudRate.Text = value.ToString(); }
        }

        /// <summary>
        /// Получает или задает стандартное число битов данных в байте.
        /// </summary>
        public int DataBits
        {
            get { return int.Parse(comboBoxDataBits.Text); }
            set { comboBoxDataBits.Text = value.ToString(); }
        }

        /// <summary>
        /// Получает или задает протокол контроля четности.
        /// </summary>
        public Parity Parity
        {
            get { return GetParity(comboBoxParity.Text); }
            set { comboBoxParity.SelectedIndex = GetIndexOfParity(value); }
        }

        /// <summary>
        /// Получает или задает стандартное число стоповых битов в байте.
        /// </summary>
        public StopBits StopBits
        {
            get
            {
                switch (comboBoxStopBits.SelectedIndex)
                {
                    case 0:

                        return StopBits.One;

                    case 1:

                        return StopBits.Two;
                }
                return StopBits.None;
            }

            set
            {
                switch (value)
                {
                    case StopBits.One:

                        comboBoxStopBits.SelectedIndex = 0;
                        break;

                    case StopBits.Two:

                        comboBoxStopBits.SelectedIndex = 1;
                        break;

                    default:

                        break;
                }
            }
        }

        /// <summary>
        /// Получить протокол контроля четности
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private Parity GetParity(string text)
        {
            System.IO.Ports.Parity pr = Parity.None;
            switch (text)
            {
                case "Чет":

                    pr = Parity.Even;
                    break;

                case "Нечет":

                    pr = Parity.Odd;
                    break;

                case "Нет":

                    pr = Parity.None;
                    break;

                case "Маркер":

                    pr = Parity.Mark;
                    break;

                case "Пробел":

                    pr = Parity.Space;
                    break;

                default:

                    break;
            }
            return pr;
        }

        private int GetIndexOfParity(Parity parity)
        {
            int val = -1;
            switch (parity)
            {
                case Parity.Even:

                    val = 0;
                    break;

                case Parity.Mark:

                    val = 3;
                    break;

                case Parity.None:

                    val = 2;
                    break;

                case Parity.Odd:

                    val = 1;
                    break;

                case Parity.Space:

                    val = 4;
                    break;
            }
            return val;

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
    }
}