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
    public partial class AddAssignmentForm : Form
    {
        Application app = null;
        int position = -1;

        int tempIndex = -1;

        public AddAssignmentForm(Application _app)
        {
            InitializeComponent();
            app = _app;
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAssignmentForm_Load(object sender, EventArgs e)
        {
            tempIndex = Number;
            foreach (var condition in app.Stock.Conditions)
            {
                InsertCondition(condition);
            }

            if (listViewChannels.SelectedItems != null && listViewChannels.SelectedItems.Count > 0)
            {
                listViewChannels.EnsureVisible(listViewChannels.SelectedItems[0].Index);
            }
        }

        /// <summary>
        /// Номер обрабатываемого параметра
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
        private void InsertCondition(Parameter condition)
        {
            ListViewItem item = new ListViewItem(condition.Position.ToString());

            ListViewItem.ListViewSubItem device = new ListViewItem.ListViewSubItem(item, condition.Device.ToString());
            ListViewItem.ListViewSubItem offset = new ListViewItem.ListViewSubItem(item, condition.Offset.ToString());

            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, condition.Description);
            ListViewItem.ListViewSubItem size = new ListViewItem.ListViewSubItem(item, condition.Size.ToString());            

            item.SubItems.Add(device);
            item.SubItems.Add(offset);
                        
            item.SubItems.Add(size);
            item.SubItems.Add(desc);

            item.Tag = condition;
            listViewChannels.Items.Add(item);

            if (condition.Position == position)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// Проверка ввода
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
                    if (formula.Position != tempIndex && Number == formula.Position)
                    {
                        MessageBox.Show(this, "Номер текущей формулы занят!", "Предупреждение",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        DialogResult = DialogResult.None;
                        return;
                    }
                }
            }

            if (listViewChannels.SelectedItems != null && listViewChannels.SelectedItems.Count > 0)
            {
                if (listViewChannels.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in listViewChannels.SelectedItems)
                    {
                        if (item.Tag != null)
                        {
                            if (item.Tag is Parameter)
                            {
                                position = (item.Tag as Parameter).Position;
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
            else
            {
                MessageBox.Show(this, "Выберите канал", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                DialogResult = DialogResult.None;
            }
        }
    }
}