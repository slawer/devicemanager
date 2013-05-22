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
    public partial class SelectParametersBOForm : Form
    {
        private Application app = null;
        private List<Formula> frms = null;

        public SelectParametersBOForm(Application _app)
        {
            app = _app;
            frms = new List<Formula>();

            InitializeComponent();
        }


        public int DefaultParameterSize
        {
            get { return (int)numericUpDown1.Value; }
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectParametersBOForm_Load(object sender, EventArgs e)
        {
            if (app != null)
            {
                foreach (Formula item in app.Converter.Formuls)
                {
                    InsertFormula(item);
                }
            }
        }

        /// <summary>
        /// Добавить формулу
        /// </summary>
        /// <param name="formula">Добавляемая формула</param>
        private void InsertFormula(Formula formula)
        {
            ListViewItem item = new ListViewItem();

            ListViewItem.ListViewSubItem number = new ListViewItem.ListViewSubItem(item, formula.Position.ToString());
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, formula.Macros.Description);

            item.SubItems.Add(number);
            item.SubItems.Add(desc);

            item.Tag = formula;
            listViewParameters.Items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accept_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewParameters.Items)
            {
                if (item.Checked)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag is Formula)
                        {
                            frms.Add(item.Tag as Formula);
                        }
                    }
                }
            }
        }

        public List<Formula> Formuls
        {
            get { return frms; }
        }
    }
}