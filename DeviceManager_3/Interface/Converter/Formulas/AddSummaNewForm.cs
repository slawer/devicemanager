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
    public partial class AddSummaNewForm : Form
    {
        private Application app = null;
        private Argument first, second;

        private int tempIndex = -1;
        private string comment = string.Empty;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="_app">Контекст выполнения формы</param>
        public AddSummaNewForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        /// <summary>
        /// Первый аргумент
        /// </summary>
        public Argument FirstArg
        {
            get { return first; }
            set { first = value; }
        }

        /// <summary>
        /// Второй аргумент
        /// </summary>
        public Argument SecondtArg
        {
            get { return second; }
            set { second = value; }
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
        /// Определяем параметр для сложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    InsertChannel(item.Index);
                }
            }
        }

        /// <summary>
        /// определить аргумент
        /// </summary>
        private void InsertChannel(int index)
        {
            ResultsForm res = new ResultsForm(app);
            if (index == 0)
            {
                if (first != null)
                {
                    res.Position = first.Index;
                }
            }
            else
            {
                if (second != null)
                {
                    res.Position = second.Index;
                }
            }

            if (res.ShowDialog(this) == DialogResult.OK)
            {
                Formula form = res.SelectedParameter;
                if (form != null)
                {
                    if (index == 0)
                    {
                        first = new Argument();

                        first.Index = form.Position;
                        first.Source = DataSource.Results;

                        listView1.Items[index].SubItems[0].Text = first.Index.ToString();
                    }
                    else
                    {
                        second = new Argument();

                        second.Index = form.Position;
                        second.Source = DataSource.Results;

                        listView1.Items[index].SubItems[0].Text = second.Index.ToString();
                    }

                    listView1.Items[index].SubItems[1].Text = form.Macros.Description;
                }
            }
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSummaNewForm_Load(object sender, EventArgs e)
        {
            tempIndex = Number;
            if (first != null && second != null)
            {
                InsertFirst(first);
                InsertSecond(second);
            }
        }

        /// <summary>
        /// добавить первый аргумент
        /// </summary>
        /// <param name="argument"></param>
        private void InsertFirst(Argument argument)
        {
            switch (argument.Source)
            {
                case DataSource.Results:

                    if (argument.IsActual)
                    {
                        listView1.Items[0].SubItems[0].Text = first.Index.ToString();
                        Formula f2 = GetArgument(first);

                        if (f2 != null)
                        {
                            listView1.Items[0].SubItems[1].Text = f2.Macros.Description;
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// добавить второй аргумент
        /// </summary>
        /// <param name="argument"></param>
        private void InsertSecond(Argument argument)
        {
            switch (argument.Source)
            {
                case DataSource.Results:

                    if (argument.IsActual)
                    {
                        listView1.Items[1].SubItems[0].Text = second.Index.ToString();
                        Formula f2 = GetArgument(second);

                        if (f2 != null)
                        {
                            listView1.Items[1].SubItems[1].Text = f2.Macros.Description;
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Получить формулу с указанным аргументом
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        private Formula GetArgument(Argument argument)
        {
            foreach (Formula formula in app.Converter.Formuls)
            {
                if (formula.Position == argument.Index)
                {
                    return formula;
                }
            }

            return null;
        }

        /// <summary>
        /// 
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

            if (FirstArg == null || SecondtArg == null)
            {
                MessageBox.Show(this, "Укажите операнды", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                DialogResult = DialogResult.None;
            }
        }
    }
}