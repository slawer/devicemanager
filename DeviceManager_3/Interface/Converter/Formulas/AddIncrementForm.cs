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
    public partial class AddIncrementForm : Form
    {
        Application app = null;
        int position = -1;

        private int tempIndex = -1;

        public AddIncrementForm(Application _app)
        {
            InitializeComponent();
            app = _app;
        }

        private void AddAssignmentForm_Load(object sender, EventArgs e)
        {
            tempIndex = Number;
            foreach (var formula in app.Converter.Formuls)
            {
                InsertFormula(formula);
            }

            if (listViewResults.SelectedItems != null && listViewResults.SelectedItems.Count > 0)
            {
                listViewResults.EnsureVisible(listViewResults.SelectedItems[0].Index);
            }
        }
        
        /// <summary>
        /// Позиция в которую сохранять
        /// </summary>
        public int Position
        {
            get { return position; }
            set { position = value; }
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
        /// Добавить условие в список
        /// </summary>
        /// <param name="condition">Добавляемое условие</param>
        private void InsertFormula(Formula parameter)
        {
            ListViewItem item = new ListViewItem(parameter.Position.ToString());
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, parameter.Macros.Description);

            item.SubItems.Add(desc);

            item.Tag = parameter;
            listViewResults.Items.Add(item);

            if (position == parameter.Position)
            {
                item.Selected = true;
            }
        }

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
                    if (formula.Position != tempIndex && Number == formula.Position)
                    {
                        MessageBox.Show(this, "Номер текущей формулы занят!", "Предупреждение",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        DialogResult = DialogResult.None;
                        return;
                    }
                }
            }

            if (listViewResults.SelectedItems != null && listViewResults.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listViewResults.SelectedItems)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag is Formula)
                        {
                            position = (item.Tag as Formula).Position;
                            DialogResult = DialogResult.OK;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "Выберите канал", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                DialogResult = DialogResult.None;
            }
        }
    }
}