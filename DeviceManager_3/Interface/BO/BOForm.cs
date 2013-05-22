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
    public partial class BOForm : Form
    {
        private Application app = null;

        public BOForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        /// <summary>
        /// Добавит команду на БО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insert_Click(object sender, EventArgs e)
        {
            InsertBOForm frm = new InsertBOForm(app);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                DisplayPacket packet = frm.DisplayPacket;
                if (packet != null)
                {
                    app.Display.Insert(packet);
                    InsertToList(packet);
                }                
            }
        }

        /// <summary>
        /// Добавить пакет в список
        /// </summary>
        /// <param name="packet"></param>
        private void InsertToList(DisplayPacket packet)
        {
            ListViewItem item = new ListViewItem();

            ListViewItem.ListViewSubItem num = new ListViewItem.ListViewSubItem(item, packet.Device.ToString());
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, packet.Description);
            
            string port_name = string.Empty;
            if (packet.TypePort == TypePort.Primary)
            {
                port_name = "Основной";
            }
            else
                if (packet.TypePort == TypePort.Secondary)
                {
                    port_name = "Вспомогательный";
                }
                else
                {
                    port_name = "Не определен";
                }


            ListViewItem.ListViewSubItem port = new ListViewItem.ListViewSubItem(item, port_name);

            item.Checked = packet.IsActived;

            item.SubItems.Add(num);
            item.SubItems.Add(desc);

            item.SubItems.Add(port);

            item.Tag = packet;
            listViewBos.Items.Add(item);
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BOForm_Load(object sender, EventArgs e)
        {
            DisplayPacket[] packets = app.Display.Packets;
            if (packets != null)
            {
                foreach (DisplayPacket packet in packets)
                {
                    InsertToList(packet);
                }
            }
        }

        /// <summary>
        /// Удалить пакет для БО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_Click(object sender, EventArgs e)
        {
            if (listViewBos.SelectedItems != null)
            {
                if (listViewBos.SelectedItems.Count > 0)
                {
                    if (listViewBos.SelectedItems[0].Tag is DisplayPacket)
                    {
                        DisplayPacket removed = listViewBos.SelectedItems[0].Tag as DisplayPacket;
                        if (removed != null)
                        {
                            app.Display.Remove(removed);
                            listViewBos.Items.Remove(listViewBos.SelectedItems[0]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Редактировать пакет для БО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_Click(object sender, EventArgs e)
        {
            if (listViewBos.SelectedItems != null)
            {
                if (listViewBos.SelectedItems.Count > 0)
                {
                    if (listViewBos.SelectedItems[0].Tag != null)
                    {
                        if (listViewBos.SelectedItems[0].Tag is DisplayPacket)
                        {
                            DisplayPacket pack = listViewBos.SelectedItems[0].Tag as DisplayPacket;
                            if (pack != null)
                            {
                                InsertBOForm frm = new InsertBOForm(app);
                                frm.DisplayPacket = pack;

                                if (frm.ShowDialog(this) == DialogResult.OK)
                                {
                                    DisplayPacket packet = frm.DisplayPacket;
                                    if (packet != null)
                                    {
                                        //app.Display.Insert(packet);
                                        //InsertToList(packet);

                                        listViewBos.SelectedItems[0].SubItems[1].Text = packet.Device.ToString();
                                        listViewBos.SelectedItems[0].SubItems[2].Text = packet.Description;

                                        if (packet.TypePort == TypePort.Primary)
                                        {
                                            listViewBos.SelectedItems[0].SubItems[3].Text = "Основной";
                                        }
                                        else
                                            if (packet.TypePort == TypePort.Secondary)
                                            {
                                                listViewBos.SelectedItems[0].SubItems[3].Text = "Вспомогательный";
                                            }
                                            else
                                            {
                                                listViewBos.SelectedItems[0].SubItems[3].Text = "Не определен";
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// чекнули строку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewBos_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item != null)
            {
                if (e.Item.Tag is DisplayPacket)
                {
                    DisplayPacket packet = e.Item.Tag as DisplayPacket;
                    if (packet != null)
                    {
                        packet.IsActived = e.Item.Checked;
                    }
                }
            }
        }
    }
}