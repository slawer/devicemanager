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
    public partial class SignalsForm : Form
    {
        private Application app = null;         // контекст в котором выполняется программа

        public SignalsForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        /// <summary>
        /// Выбранный канал устройства
        /// </summary>
        public Parameter SelectedParameter
        {
            get
            {
                if (listViewChannels.SelectedItems != null)
                {
                    foreach (ListViewItem item in listViewChannels.SelectedItems)
                    {
                        if (item != null)
                        {
                            if (item.Tag != null)
                            {
                                if (item.Tag is Parameter)
                                {
                                    return (item.Tag as Parameter);
                                }
                            }
                        }
                    }

                    return null;
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
        private void SignalsForm_Load(object sender, EventArgs e)
        {
            if (app != null)
            {
                foreach (var item in app.Stock.Conditions)
                {
                    InsertCondition(item);
                }
            }
        }

        /// <summary>
        /// Добавить параметр в список
        /// </summary>
        /// <param name="parameter">Добавляемый параметр</param>
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
        }
    }
}