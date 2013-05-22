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
    public partial class SelectForm : Form
    {
        private Application app;

        public SelectForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        /// <summary>
        /// Список выбранных параметров
        /// </summary>
        public Parameter[] Parameters
        {
            get
            {
                if (listViewParameters.Items.Count > 0)
                {
                    List<Parameter> parameters = new List<Parameter>();
                    foreach (ListViewItem item in listViewParameters.Items)
                    {
                        if (item.Tag != null && item.Checked)
                        {
                            if (item.Tag is Formula)
                            {
                                Formula frm = item.Tag as Formula;
                                Parameter parameter = new Parameter();

                                parameter.Position = frm.Position;
                                parameter.Description = frm.Macros.Description;

                                parameters.Add(parameter);
                            }
                        }
                    }

                    return parameters.ToArray();
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectForm_Load(object sender, EventArgs e)
        {
            if (app != null && app.Converter != null)
            {
                Formula[] formuls = app.Converter.Formuls;
                if (formuls != null)
                {
                    foreach (Formula formula in formuls)
                    {
                        InsertParameter(formula);
                    }
                }
            }
        }

        /// <summary>
        /// Добавить параметр в список
        /// </summary>
        /// <param name="parameter">Добавляемый параметр</param>
        private void InsertParameter(Formula parameter)
        {
            ListViewItem item = new ListViewItem();

            ListViewItem.ListViewSubItem num = new ListViewItem.ListViewSubItem(item, parameter.Position.ToString());
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, parameter.Macros.Description);

            item.Tag = parameter;

            item.SubItems.Add(num);
            item.SubItems.Add(desc);

            listViewParameters.Items.Add(item);
        }

    }
}