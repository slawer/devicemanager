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
    public partial class ConverterViewForm : Form
    {
        private Application app = null;             // контекст
        private Mutex mutex = null;                 // синхронизатор

        public ConverterViewForm(Application _app)
        {
            app = _app;
            InitializeComponent();

            mutex = new Mutex();
        }

        /// <summary>
        /// загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConverterViewForm_Load(object sender, EventArgs e)
        {
            Formula[] formuls = app.Converter.Formuls;
            if (formuls != null)
            {
                int rowIndex = 0;
                foreach (Formula formula in formuls)
                {
                    dataGridView1.Rows.Add();

                    DataGridViewCell channelNumber = dataGridView1.Rows[rowIndex].Cells[0];
                    DataGridViewCell channelDesc = dataGridView1.Rows[rowIndex].Cells[1];

                    DataGridViewCell fIteration = dataGridView1.Rows[rowIndex].Cells[2];

                    channelNumber.Value = formula.Position;
                    channelDesc.Value = formula.Macros.Description;

                    dataGridView1.Rows[rowIndex].Tag = formula;
                    rowIndex = rowIndex + 1;                    
                }

                app.Converter.OnComplete += new EventHandler(Serial_OnStaticComplete);
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
                if (mutex.WaitOne(1000, false))
                {
                    blocked = true;
                    Float[] results = app.Converter.GetResults();
                    if (results != null)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Index < dataGridView1.Rows.Count - 1)
                            {
                                int position = Convert.ToInt32(row.Cells[0].Value);
                                if (position > -1 && position < results.Length)
                                {
                                    row.Cells[2].Value = string.Format("{0:F}", (double)results[position].GetCurrentValue());
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
        private void ConverterViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            app.Converter.OnComplete -= Serial_OnStaticComplete;
        }
    }
}