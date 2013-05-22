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
    public partial class CommonOptions : Form
    {
        private TypeCRC crc = TypeCRC.Cycled;
        private ApplicationMode mode = ApplicationMode.Active;

        public CommonOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Определяет запускать приложение на исполнение при старте или нет
        /// </summary>
        public Boolean Autorun
        {
            get { return checkBoxAutoStart.Checked; }
            set 
            {
                try
                {
                    checkBoxAutoStart.Checked = value;
                }
                catch
                {
                    checkBoxAutoStart.Checked = false;
                }
            }
        }

        /// <summary>
        /// Определяет сворачивать главное окно в трей или нет
        /// </summary>
        public Boolean IsNotify
        {
            get { return checkBox1.Checked; }
            set { checkBox1.Checked = value; }
        }

        /// <summary>
        /// Определяет количество попыток чтения пакета
        /// </summary>
        public int PopitokSteniaPacketa
        {
            get { return (int)numericUpDownPopitokSteniaPacketa.Value; }
            set 
            {
                try
                {
                    numericUpDownPopitokSteniaPacketa.Value = value;
                }
                catch
                {
                    numericUpDownPopitokSteniaPacketa.Value = numericUpDownPopitokSteniaPacketa.Minimum;
                }
            }
        }

        /// <summary>
        /// Определяет количество попыток чтения данных пакета
        /// </summary>
        public int PopitokSteniaDannih
        {
            get { return (int)numericUpDownPopitokSteniaDannih.Value; }
            set 
            {
                try
                {
                    numericUpDownPopitokSteniaDannih.Value = value;
                }
                catch
                {
                    numericUpDownPopitokSteniaDannih.Value = numericUpDownPopitokSteniaDannih.Minimum;
                }
            }
        }

        /// <summary>
        /// Определяет время на чтение(мс)
        /// </summary>
        public int VremaNaStenie
        {
            get { return (int)numericUpDownVremaNaStenie.Value; }
            set 
            {
                try
                {
                    numericUpDownVremaNaStenie.Value = value;
                }
                catch
                {
                    numericUpDownVremaNaStenie.Value = numericUpDownVremaNaStenie.Minimum;
                }
            }
        }

        /// <summary>
        /// Определяет время на запись(мс)
        /// </summary>
        public int VremaNaZapis
        {
            get { return (int)numericUpDownVremaNaZapis.Value; }
            set 
            {
                try
                {
                    numericUpDownVremaNaZapis.Value = value;
                }
                catch
                {
                    numericUpDownVremaNaZapis.Value = numericUpDownVremaNaZapis.Minimum;
                }
            }
        }

        /// <summary>
        /// Определяет период посылки команд(мс)
        /// </summary>
        public int PeriodPosilkiKomand
        {
            get { return (int)numericUpDownPeriodPosilkiKomand.Value; }
            set 
            {
                try
                {
                    numericUpDownPeriodPosilkiKomand.Value = value;
                }
                catch
                {
                    numericUpDownPeriodPosilkiKomand.Value = numericUpDownPeriodPosilkiKomand.Minimum;
                }
            }
        }

        /// <summary>
        /// Определяет время между отправкой пакетов
        /// </summary>
        public int VremaMesdyOtpravkoiPaketov
        {
            get { return (int)numericUpDownVremaMesdyOtpravkoiPaketov.Value; }
            set 
            {
                try
                {
                    numericUpDownVremaMesdyOtpravkoiPaketov.Value = value;
                }
                catch
                {
                    numericUpDownVremaMesdyOtpravkoiPaketov.Value = numericUpDownVremaMesdyOtpravkoiPaketov.Minimum;
                }
            }
        }

        /// <summary>
        /// Определяет тип CRC
        /// </summary>
        public TypeCRC TypeCRC
        {
            get { return crc; }
            set { crc = value; }
        }

        /// <summary>
        /// Опреедляет режим работы
        /// </summary>
        public ApplicationMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        /// <summary>
        /// Инициализировать форму
        /// </summary>
        public void InitializeGui()
        {
            switch (crc)
            {
                case global::DeviceManager.TypeCRC.Cycled:

                    radioButtonOneByte.Checked = true;
                    break;

                case global::DeviceManager.TypeCRC.CycledTwo:

                    radioButtonTwoByte.Checked = true;
                    break;

                case global::DeviceManager.TypeCRC.CRC16:

                    radioButtonCRC16.Checked = true;
                    break;
            }

            switch (mode)
            {
                case ApplicationMode.Active:

                    radioButtonActived.Checked = true;
                    break;

                case ApplicationMode.Passive:

                    radioButtonPassive.Checked = true;
                    break;

                case ApplicationMode.Emulated:

                    radioButtonEmulated.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Gjrfpfnm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommonOptions_Shown(object sender, EventArgs e)
        {
            InitializeGui();
        }

        private void radioButtonCRC16_CheckedChanged(object sender, EventArgs e)
        {
            crc = global::DeviceManager.TypeCRC.CRC16;
        }

        private void radioButtonOneByte_CheckedChanged(object sender, EventArgs e)
        {
            crc = global::DeviceManager.TypeCRC.Cycled;
        }

        private void radioButtonTwoByte_CheckedChanged(object sender, EventArgs e)
        {
            crc = global::DeviceManager.TypeCRC.CycledTwo;
        }

        private void radioButtonActived_CheckedChanged(object sender, EventArgs e)
        {
            mode = ApplicationMode.Active;
        }

        private void radioButtonPassive_CheckedChanged(object sender, EventArgs e)
        {
            mode = ApplicationMode.Passive;
        }

        private void radioButtonEmulated_CheckedChanged(object sender, EventArgs e)
        {
            mode = ApplicationMode.Emulated;
        }
    }
}