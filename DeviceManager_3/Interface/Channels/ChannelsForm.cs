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
    public partial class ChannelsForm : Form
    {
        private Application app = null;

        private int lastDevice = 1; 
        private int lastOffset = 0;

        private int lastSize = 0;

        public ChannelsForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsForm_Load(object sender, EventArgs e)
        {
            foreach (var condition in app.Stock.Conditions)
            {
                InsertCondition(condition);
            }
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
            ListViewItem.ListViewSubItem size = new ListViewItem.ListViewSubItem(item, condition.Size.ToString());

            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, condition.Description);

            item.SubItems.Add(device);
            item.SubItems.Add(offset);
            item.SubItems.Add(size);
            item.SubItems.Add(desc);

            listViewChannels.Items.Add(item);
            item.Tag = condition;
        }

        /// <summary>
        /// Добавляем канал
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insert_channel_Click(object sender, EventArgs e)
        {
            InsertChannelForm ins = new InsertChannelForm();
            ins.Text = "Добавление канала";

            ins.Device = lastDevice;
            ins.Offset = lastOffset + lastSize;

            ins.DataSize = lastSize;

            if (ins.ShowDialog(this) == DialogResult.OK)
            {
                Parameter p = new Parameter();

                p.Device = ins.Device;
                p.Offset = ins.Offset;

                p.Size = ins.DataSize;
                p.Position = app.Stock.GetFreeChannel();

                p.Description = ins.Comment;

                lastDevice = p.Device;
                lastOffset = p.Offset;

                lastSize = p.Size;

                app.Stock.InsertCondition(p);
                InsertCondition(p);
            }
        }

        /// <summary>
        /// Удаляем канал
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_channel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewChannels.SelectedItems)
            {
                if (item.Tag != null)
                {
                    if (item.Tag is Parameter)
                    {
                        app.Stock.RemoveCondition(item.Tag as Parameter);
                        listViewChannels.Items.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Редактируем канал
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_channel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewChannels.SelectedItems)
            {
                if (item.Tag != null)
                {
                    if (item.Tag is Parameter)
                    {
                        Parameter parameter = item.Tag as Parameter;

                        InsertChannelForm ins = new InsertChannelForm();
                        ins.Text = "Редактирование канала";

                        ins.Device = parameter.Device;
                        ins.Offset = parameter.Offset;

                        ins.DataSize = parameter.Size;
                        ins.Comment = parameter.Description;

                        if (ins.ShowDialog(this) == DialogResult.OK)
                        {
                            parameter.Device = ins.Device;
                            parameter.Offset = ins.Offset;

                            parameter.Size = ins.DataSize;
                            parameter.Description = ins.Comment;

                            lastDevice = parameter.Device;
                            lastOffset = parameter.Offset;

                            lastSize = parameter.Size;

                            item.SubItems[1].Text = parameter.Device.ToString();

                            item.SubItems[2].Text = parameter.Offset.ToString();
                            item.SubItems[3].Text = parameter.Size.ToString();

                            item.SubItems[4].Text = parameter.Description;
                        }
                    }
                }
            }
        }
    }
}