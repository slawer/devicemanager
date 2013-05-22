using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DeviceManager
{
    public partial class ChannelsViewForm : Form
    {
        private Application app = null;             // контекст
        private Mutex mutex = null;                 // синхронизатор

        public ChannelsViewForm(Application _app)
        {
            app = _app;
            InitializeComponent();

            mutex = new Mutex();
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsViewForm_Load(object sender, EventArgs e)
        {
            Parameter[] conditions = app.Stock.Conditions;
            if (conditions != null)
            {
                int rowIndex = 0;
                foreach (Parameter condition in conditions)
                {
                    dataGridView1.Rows.Add();

                    DataGridViewCell channelNumber = dataGridView1.Rows[rowIndex].Cells[0];
                    DataGridViewCell channelDesc = dataGridView1.Rows[rowIndex].Cells[1];

                    DataGridViewCell fIteration = dataGridView1.Rows[rowIndex].Cells[2];

                    channelNumber.Value = condition.Position;
                    channelDesc.Value = condition.Description;

                    dataGridView1.Rows[rowIndex].Tag = condition;
                    rowIndex = rowIndex + 1;                    
                }

                app.Serial.OnStaticComplete += new EventHandler(Serial_OnStaticComplete);       // активный
                app.Serial.Secondary.OnComplete += new EventHandler(Serial_OnStaticComplete);   // активный

                app.Converter.OnComplete += new EventHandler(Serial_OnStaticComplete);          // пассивный
                //app.Converter.OnComplete += new EventHandler(Serial_OnStaticComplete);          // эмулирующий режим
            }
        }

        /// <summary>
        /// Завершен цикл опроса устройств
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Serial_OnStaticComplete(object sender, EventArgs e)
        {
            bool blocked = false;
            try
            {
                if (mutex.WaitOne(300, false))
                {
                    blocked = true;
                    Float[] results = app.Stock.GetSlice();
                    if (results != null)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Index < dataGridView1.Rows.Count - 1)
                            {
                                int position = Convert.ToInt32(row.Cells[0].Value);
                                if (position > -1 && position < results.Length)
                                {
                                    double val = results[position].GetCurrentValue();
                                    string s_val = string.Format("{0:r}", (double)val);

                                    row.Cells[2].Value = s_val;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
            if (blocked) mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// выгружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            app.Serial.OnStaticComplete -= Serial_OnStaticComplete;       // активный
            app.Converter.OnComplete -= Serial_OnStaticComplete;          // пассивный

            app.Converter.OnComplete -= Serial_OnStaticComplete;          // эмулирующий режим
        }
    }
}