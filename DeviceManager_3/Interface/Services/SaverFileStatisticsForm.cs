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
    public partial class SaverFileStatisticsForm : Form
    {
        private Application app = null;

        public SaverFileStatisticsForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        private void SaverFileStatisticsForm_Load(object sender, EventArgs e)
        {
            timer1_Tick(null, null);

            textBoxURI.Text = app.Saver.File.URI;

            textBoxPCount.Text = app.Saver.File.PCount.ToString();
            textBoxDataAreaSize.Text = app.Saver.File.DataAreaSize.ToString();

            textBoxDataOffset.Text = app.Saver.File.DataOffset.ToString();
            textBoxStampsCount.Text = app.Saver.File.StampsCount.ToString();

            textBoxStampsOffset.Text = app.Saver.File.StampsOffset.ToString();
            textBoxBlockSize.Text = app.Saver.File.BlockSize.ToString();

            textBoxSPeriod.Text = app.Saver.File.SPeriod.ToString();
            textBoxPeriod.Text = app.Saver.File.Period.ToString();

            timer1.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (app != null)
            {
                if (app.Saver.File.IsLoaded)
                {
                    textBoxIsLoad.Text = "Загружен";
                }
                else
                    textBoxIsLoad.Text = "Не загружен";

                textBoxLabels.Text = app.Saver.File.CountSavedLabels.ToString();
                textBoxBlocks.Text = app.Saver.File.CountSavedBlocks.ToString();
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}