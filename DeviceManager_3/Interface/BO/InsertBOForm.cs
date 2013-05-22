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
    public partial class InsertBOForm : Form
    {
        private Application app = null;
        private DisplayPacket packet = null;

        public InsertBOForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertBOForm_Load(object sender, EventArgs e)
        {
            try
            {
                
                if ((int)packet.Period.TotalMilliseconds >= numericUpDownPeriod.Minimum
                    && (int)packet.Period.TotalMilliseconds <= numericUpDownPeriod.Maximum)
                {
                    numericUpDownPeriod.Value = (int)packet.Period.TotalMilliseconds;
                }
                else
                {
                    numericUpDownPeriod.Value = numericUpDownPeriod.Minimum;
                    packet.Period = new TimeSpan(0, 0, 0, 0, (int)numericUpDownPeriod.Minimum);
                }
            }
            catch { }

            if (packet != null)
            {
                textBoxComment.Text = packet.Description;
                numericUpDownDevice.Value = packet.Device;

                foreach (Parameter parameter in packet.Parameters)
                {
                    InsertParameter(parameter);
                }

                checkBoxToPort.Checked = packet.ToPort;

                if (packet.TypePort == TypePort.Primary)
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                    if (packet.TypePort == TypePort.Secondary)
                    {
                        comboBox1.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 2;
                    }
            }
            else
            {
                packet = new DisplayPacket();
                checkBoxToPort.Checked = packet.ToPort;
            }
        }

        /// <summary>
        /// Добавить формулу
        /// </summary>
        /// <param name="formula"></param>
        private void InsertParameter(Parameter formula)
        {
            ListViewItem item = new ListViewItem(formula.Size.ToString());
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, GetDescription(formula));

            item.SubItems.Add(desc);

            item.Tag = formula;
            listView1.Items.Add(item);
        }

        string GetDescription(Parameter formula)
        {
            foreach (var f in app.Converter.Formuls)
            {
                if (f.Position == formula.Position)
                {
                    return f.Macros.Description;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Определяет параметр для отправки на БО
        /// </summary>
        public DisplayPacket DisplayPacket
        {
            get { return packet; }
            set { packet = value; }
        }

        /// <summary>
        /// Определяет параметры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectParameters_Click(object sender, EventArgs e)
        {
            SelectParametersBOForm frm = new SelectParametersBOForm(app);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                foreach (Formula formula in frm.Formuls)
                {
                    Parameter parameter = new Parameter();

                    parameter.Size = frm.DefaultParameterSize;
                    parameter.Position = formula.Position;                    

                    InsertParameter(parameter);
                }
            }
        }

        /// <summary>
        /// добавить формулу
        /// </summary>
        /// <param name="formula">Добавляемая формула</param>
        private void InsertFormula(Formula formula)
        {
            ListViewItem item = new ListViewItem("2");
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, formula.Macros.Description);

            item.SubItems.Add(desc);
            item.Tag = formula;

            listView1.Items.Add(item);
        }

        /// <summary>
        /// удаляем параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeParameter_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem selected = listView1.SelectedItems[0];
                    if (selected != null)
                    {
                        listView1.Items.Remove(selected);
                    }
                }
            }
        }

        /// <summary>
        /// Определяет размер параметра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editParameter_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem selected = listView1.SelectedItems[0];
                    if (selected != null)
                    {
                        EditParameterForm frm = new EditParameterForm();
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {
                            selected.SubItems[0].Text = ((int)frm.numericUpDown1.Value).ToString();

                            if (selected.Tag != null)
                            {
                                if (selected.Tag is Parameter)
                                {
                                    Parameter parameter = selected.Tag as Parameter;
                                    parameter.Size = (int)frm.numericUpDown1.Value;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// формируем пакет для блока отображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accept_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                packet.Description = textBoxComment.Text;
                packet.Device = (int)numericUpDownDevice.Value;

                packet.Period = new TimeSpan(0, 0, 0, 0, (int)numericUpDownPeriod.Value);

                if (comboBox1.SelectedIndex == 0)
                {
                    packet.TypePort = TypePort.Primary;
                }
                else
                    if (comboBox1.SelectedIndex == 1)
                    {
                        packet.TypePort = TypePort.Secondary;
                    }
                    else
                    {
                        packet.TypePort = TypePort.Default;
                    }

                packet.Clear();
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item != null)
                    {
                        if (item.Tag != null)
                        {
                            if (item.Tag is Parameter)
                            {                                
                                packet.Insert(item.Tag as Parameter);
                            }
                        }
                    }
                }
            }
            else
                packet = null;
        }

        /// <summary>
        /// переместить параметр вниз по списку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toDown_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 1)
            {
                if (listView1.SelectedItems != null)
                {
                    if (listView1.SelectedItems.Count > 0)
                    {
                        ListViewItem selected = listView1.SelectedItems[0];
                        int selected_index = selected.Index;

                        if (selected_index < listView1.Items.Count - 1)
                        {
                            ListViewItem s_clone = listView1.Items[selected_index].Clone() as ListViewItem;
                            ListViewItem d_clone = listView1.Items[selected_index + 1].Clone() as ListViewItem;

                            if (s_clone != null && d_clone != null)
                            {
                                listView1.Items[selected_index] = d_clone;
                                listView1.Items[selected_index + 1] = s_clone;

                                listView1.Items[selected_index + 1].Selected = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// переместить вверх параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toUp_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 1)
            {
                if (listView1.SelectedItems != null)
                {
                    if (listView1.SelectedItems.Count > 0)
                    {
                        ListViewItem selected = listView1.SelectedItems[0];
                        int selected_index = selected.Index;

                        if (selected_index > 0)
                        {
                            ListViewItem s_clone = listView1.Items[selected_index].Clone() as ListViewItem;
                            ListViewItem d_clone = listView1.Items[selected_index - 1].Clone() as ListViewItem;

                            if (s_clone != null && d_clone != null)
                            {
                                listView1.Items[selected_index] = d_clone;
                                listView1.Items[selected_index - 1] = s_clone;
                                
                                listView1.Items[selected_index - 1].Selected = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// изменили состояние отправки пакета в порт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxToPort_CheckedChanged(object sender, EventArgs e)
        {
            packet.ToPort = checkBoxToPort.Checked;
        }
    }
}