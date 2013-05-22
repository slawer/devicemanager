using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace DeviceManager
{
    public partial class AddConstantForm : Form
    {
        private Application app;
        private int temIndex = -1;

        public AddConstantForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        public float Value
        {
            get
            {
                if (Information.IsNumeric(textBoxValue.Text))
                {
                    return Application.ParseSingle(textBoxValue.Text);
                }
                else
                    return float.NaN;
            }

            set
            {
                textBoxValue.Text = value.ToString();
            }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        /// <summary>
        /// Автиматически присвоить номер канала
        /// </summary>
        public bool AutoSetNumber
        {
            get { return checkBox1.Checked; }
        }

        /// <summary>
        /// Номер присваиваемого канала
        /// </summary>
        public int Number
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        /// <summary>
        /// Проверить состояние 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accept_Click(object sender, EventArgs e)
        {
            if (checkBox1.Visible && checkBox1.Checked == false)
            {
                foreach (Formula formula in app.Converter.Formuls)
                {
                    if (formula.Position == (int)numericUpDown1.Value)
                    {
                        MessageBox.Show(this, "Номер текущей формулы занят!", "Предупреждение",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        DialogResult = DialogResult.None;
                        return;
                    }
                }
            }
            else
            {
                foreach (Formula formula in app.Converter.Formuls)
                {
                    if (formula.Position != temIndex && Number == formula.Position)
                    {
                        MessageBox.Show(this, "Номер текущей формулы занят!", "Предупреждение",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        DialogResult = DialogResult.None;
                        return;
                    }
                }
            }

            if (!Information.IsNumeric(textBoxValue.Text))
            {
                MessageBox.Show(this, "Вы ввели не допустимое значение константы", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                textBoxValue.Text = string.Empty;
                DialogResult = DialogResult.None;
            }
        }

        private void AddConstantForm_Load(object sender, EventArgs e)
        {
            temIndex = Number;
        }
    }
}