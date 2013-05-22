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
    public partial class ResultsForm : Form
    {
        private Application app = null;         // контекст в котором выполняется программа
        private int position = -1;

        public ResultsForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        /// <summary>
        /// Номер позиции выделяемого элемента
        /// </summary>
        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Определяет выбранный параметр
        /// </summary>
        public Formula SelectedParameter
        {
            get
            {
                if (listViewResults.SelectedItems != null)
                {
                    foreach (ListViewItem item in listViewResults.SelectedItems)
                    {
                        if (item.Tag != null)
                        {
                            if (item.Tag is Formula)
                            {
                                return (item.Tag as Formula);
                            }
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultsForm_Load(object sender, EventArgs e)
        {
            if (app != null)
            {
                foreach (var item in app.Converter.Formuls)
                {
                    InsertParameter(item);
                }
            }

            if (listViewResults.SelectedItems != null && listViewResults.SelectedItems.Count > 0)
            {
                listViewResults.EnsureVisible(listViewResults.SelectedItems[0].Index);
            }
        }

        /// <summary>
        /// Добавить параметр в список
        /// </summary>
        /// <param name="parameter">Добавляемый параметр</param>
        private void InsertParameter(Formula parameter)
        {
            ListViewItem item = new ListViewItem(parameter.Position.ToString());
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, parameter.Macros.Description);

            item.SubItems.Add(desc);

            listViewResults.Items.Add(item);
            item.Tag = parameter;

            if (parameter.Position == position)
            {
                item.Selected = true;
            }
        }
    }
}