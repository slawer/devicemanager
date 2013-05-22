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
    public partial class GasesForm : Form
    {
        Application _app;
        private int tempIndex = -1;

        public GasesForm(Application app)
        {
            InitializeComponent();
            _app = app;
        }

        /// <summary>
        /// комментарий
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
        /// загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GasesForm_Load(object sender, EventArgs e)
        {
            tempIndex = Number;
            if (gases != null)
            {                
                foreach (ArgumentPair pair in gases.RealArguments)
                {
                    if (pair != null)
                    {
                        InsertPair(pair);
                    }
                }
            }
        }

        /// <summary>
        /// добавляем условие
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, EventArgs e)
        {
            AddGasesPairForm frm = new AddGasesPairForm(_app);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                ArgumentPair pair = frm.Pair;
                if (pair != null)
                {
                    InsertPair(pair);
                }
            }
        }

        /// <summary>
        /// добавить пару в список
        /// </summary>
        /// <param name="pair"></param>
        protected void InsertPair(ArgumentPair pair)
        {
            ListViewItem item = new ListViewItem((listView1.Items.Count + 1).ToString());

            ListViewItem.ListViewSubItem f = new ListViewItem.ListViewSubItem(item, GetFormulaName(pair.First.Index));
            ListViewItem.ListViewSubItem s = new ListViewItem.ListViewSubItem(item, GetFormulaName(pair.Second.Index));

            item.SubItems.Add(f);
            item.SubItems.Add(s);

            item.Tag = pair;
            listView1.Items.Add(item);
        }

        protected string GetFormulaName(int index)
        {
            foreach (Formula formula in _app.Converter.Formuls)
            {
                if (formula != null)
                {
                    if (formula.Position == index)
                    {
                        return formula.Macros.Description;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// удаляем пару
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void erase_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                ArgumentPair formula = listView1.SelectedItems[0].Tag as ArgumentPair;
                if (formula != null)
                {
                    AddGasesPairForm frm = new AddGasesPairForm(_app);
                    frm.Pair = formula;

                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        ArgumentPair pair = frm.Pair;
                        if (pair != null)
                        {
                            formula.First.Index = pair.First.Index;
                            formula.First.IsActual = pair.First.IsActual;

                            formula.Second.Index = pair.Second.Index;
                            formula.Second.IsActual = pair.Second.IsActual;

                            listView1.SelectedItems[0].SubItems[1].Text = GetFormulaName(formula.First.Index);
                            listView1.SelectedItems[0].SubItems[2].Text = GetFormulaName(formula.Second.Index);
                        }
                    }
                }
            }
        }

        protected Gases gases = null;

        public Gases Gases
        {
            get
            {
                return gases;
            }

            set
            {
                gases = value;
            }
        }

        private void accept_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show(this, "Не указаны параметры формулы", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (checkBox1.Visible && checkBox1.Checked == false)
            {
                foreach (Formula formula in _app.Converter.Formuls)
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
                foreach (Formula formula in _app.Converter.Formuls)
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

            if (gases == null)
            {
                gases = new Gases();
                foreach (ListViewItem item in listView1.Items)
                {
                    ArgumentPair pair = item.Tag as ArgumentPair;
                    if (pair != null)
                    {
                        gases.InsertArgument(pair);
                    }
                }
            }
            else
            {
                gases.Clear();
                foreach (ListViewItem item in listView1.Items)
                {
                    ArgumentPair pair = item.Tag as ArgumentPair;
                    if (pair != null)
                    {
                        gases.InsertArgument(pair);
                    }
                }
            }
        }
    }
}