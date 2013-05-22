using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DeviceManager
{
    public partial class CommList : Form
    {
        private List<Packet> packets = null;            // команды опроса
        private TypeCRC typeCRC = TypeCRC.Cycled;       // Тип используемой контрольной суммы

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="type">Тип используемой контрольной суммы</param>
        public CommList(TypeCRC type)
        {
            InitializeComponent();

            typeCRC = type;
            packets = new List<Packet>();
        }

        /// <summary>
        /// Определяет тип используемой контрольной суммы
        /// </summary>
        public TypeCRC TypeCRC
        {
            get { return typeCRC; }
            set { typeCRC = value; }
        }

        /// <summary>
        /// Определяет команды опроса
        /// </summary>
        public Packet[] Packets
        {
            get { return packets.ToArray(); }
            set { packets.AddRange(value); }
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommList_Load(object sender, EventArgs e)
        {
            if (packets != null)
            {
                foreach (var packet in packets)
                {
                    String cmd = String.Empty;
                    foreach (var item in packet.Com_Packet)
                    {
                        cmd += string.Format("{0:X2}", item);
                    }
                    InsertCommand(cmd, packet.IsActived, string.Empty, packet);
                }
            }
        }

        private void InsertCommand(string command, bool actived, string namecmd, Packet pack)
        {
            ListViewItem item = new ListViewItem();
            item.Checked = actived;

            ListViewItem.ListViewSubItem fl = new ListViewItem.ListViewSubItem(item, command.Substring(0, 2));
            ListViewItem.ListViewSubItem ladd = new ListViewItem.ListViewSubItem(item, command.Substring(2, 2));
            ListViewItem.ListViewSubItem lpak = new ListViewItem.ListViewSubItem(item, command.Substring(4, 2));
            ListViewItem.ListViewSubItem cmd = new ListViewItem.ListViewSubItem(item, command.Substring(6, 2));
            ListViewItem.ListViewSubItem adr = new ListViewItem.ListViewSubItem(item, command.Substring(8, 2));
            ListViewItem.ListViewSubItem ldata = new ListViewItem.ListViewSubItem(item, command.Substring(10, 2));
            ListViewItem.ListViewSubItem data = new ListViewItem.ListViewSubItem(item, string.Empty);
            ListViewItem.ListViewSubItem status = new ListViewItem.ListViewSubItem(item, command.Substring(12, 2));
            ListViewItem.ListViewSubItem crc = new ListViewItem.ListViewSubItem(item, command.Substring(14, 2));

            string port_name = string.Empty;
            if (pack.PortType == TypePort.Primary)
            {
                port_name = "Основной";
            }
            else
                if (pack.PortType == TypePort.Secondary)
                {
                    port_name = "Вспомогательный";
                }
                else
                {
                    port_name = "Порт не определен";
                }

            ListViewItem.ListViewSubItem name = new ListViewItem.ListViewSubItem(item, port_name);
            ListViewItem.ListViewSubItem inter = new ListViewItem.ListViewSubItem(item, 
                pack.Interval.TotalMilliseconds.ToString());

            item.SubItems.Add(fl);
            item.SubItems.Add(ladd);
            item.SubItems.Add(lpak);
            item.SubItems.Add(cmd);
            item.SubItems.Add(adr);
            item.SubItems.Add(ldata);
            item.SubItems.Add(data);
            item.SubItems.Add(status);
            item.SubItems.Add(crc);
            item.SubItems.Add(name);
            item.SubItems.Add(inter);
            
            listView1.Items.Add(item);

            item.Tag = pack;
            if (actived) item.Checked = true;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            InsertCommand ins = new InsertCommand();

            ins.typeCRC = typeCRC;
            ins.Text = "Добавление новой команды опроса";

            if (ins.ShowDialog(this) == DialogResult.OK)
            {
                Packet p = new Packet();
                p.Com_Packet = ins.ComPacket;

                p.IsActived = ins.Actived;
                p.PortType = ins.TypePort;

                p.Interval = ins.Interval;

                packets.Add(p);
                InsertCommand(ins.Commanda, ins.Actived, string.Empty, p);
            }
        }

        /// <summary>
        /// Удалить команду
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (item.Tag != null)
                {
                    if (item.Tag is Packet)
                    {
                        packets.Remove(item.Tag as Packet);
                        listView1.Items.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Редактируем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                InsertCommand ins = new InsertCommand();
                ins.typeCRC = typeCRC;

                ins.Actived = item.Checked;

                ins.DataLen = item.SubItems[6].Text;
                ins.DeviceName = item.SubItems[2].Text;

                ins.TypePort = (item.Tag as Packet).PortType;
                ins.Interval = (item.Tag as Packet).Interval;

                ins.Text = "Редактирование команды опроса";
                
                if (ins.ShowDialog(this) == DialogResult.OK)
                {
                    item.SubItems[1].Text = ins.Commanda.Substring(0, 2);
                    item.SubItems[2].Text = ins.Commanda.Substring(2, 2);
                    item.SubItems[3].Text = ins.Commanda.Substring(4, 2);
                    item.SubItems[4].Text = ins.Commanda.Substring(6, 2);
                    item.SubItems[5].Text = ins.Commanda.Substring(8, 2);
                    item.SubItems[6].Text = ins.Commanda.Substring(10, 2);
                    item.SubItems[8].Text = ins.Commanda.Substring(12, 2);
                    item.SubItems[9].Text = ins.Commanda.Substring(14, 2);

                    if (ins.TypePort == TypePort.Primary)
                    {
                        item.SubItems[10].Text = "Основной";
                    }
                    else
                        if (ins.TypePort == TypePort.Secondary)
                        {
                            item.SubItems[10].Text = "Вспомогательный";
                        }
                        else
                        {
                            item.SubItems[10].Text = "Порт не определен";
                        }

                    item.SubItems[11].Text = ins.Interval.TotalMilliseconds.ToString();
                    item.Checked = ins.Actived;

                    if (item.Tag != null)
                    {
                        if (item.Tag is Packet)
                        {
                            Packet p = item.Tag as Packet;

                            p.IsActived = ins.Actived;
                            p.Com_Packet = ins.GetResultInByte();

                            p.PortType = ins.TypePort;
                            p.Interval = ins.Interval;
                        }
                    }
                }
            }
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {            
            if (e.Item != null)
            {
                if (e.Item.Tag is Packet)
                {
                    Packet packet = e.Item.Tag as Packet;
                    if (packet != null)
                    {
                        packet.IsActived = e.Item.Checked;
                    }
                }
            }
        }
    }
}