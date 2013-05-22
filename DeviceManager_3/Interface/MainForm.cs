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
    public partial class MainForm : Form
    {
        delegate void setterStatusComPort();
        setterStatusComPort statuserPort = null;        // показывает текущее состояние порта

        private Application app = null;
        private bool hided = false;
        
        public MainForm(Application application)
        {
            InitializeComponent();
            
            app = application;
            
            app.OnStart += new EventHandler(app_OnStart);
            app.OnStop += new EventHandler(app_OnStop);

            statuserPort = new setterStatusComPort(StatuserPort);
        }

        /// <summary>
        /// Выводит состояние приложения
        /// </summary>
        private void StatuserPort()
        {
            if (app.State == State.Running)
            {
                statusLabel.Text = "Работает";                
            }
            else
            {
                statusLabel.Text = "Не работает";                
            }
        }

        /// <summary>
        /// Запущенно приложение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void app_OnStop(object sender, EventArgs e)
        {
            Invoke(statuserPort);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void app_OnStart(object sender, EventArgs e)
        {
            Invoke(statuserPort);
        }

        /// <summary>
        /// Настраиваем последовательный порт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialOpt_Click(object sender, EventArgs e)
        {
            ComOptions opt = new ComOptions();

            opt.PortName = app.Serial.Port.PortName;

            opt.SizeOfReadBuffer = app.Serial.Port.ReadBufferSize;
            opt.SizeOfWriteBuffer = app.Serial.Port.WriteBufferSize;

            opt.BaudRate = app.Serial.Port.BaudRate;
            opt.DataBits = app.Serial.Port.DataBits;

            opt.Parity = app.Serial.Port.Parity;
            opt.StopBits = app.Serial.Port.StopBits;

            if (opt.ShowDialog(this) == DialogResult.OK)
            {
                if (app.Serial.Port.IsOpen == false)
                {
                    app.Serial.Port.PortName = opt.PortName;

                    app.Serial.Port.ReadBufferSize = opt.SizeOfReadBuffer;
                    app.Serial.Port.WriteBufferSize = opt.SizeOfWriteBuffer;

                    app.Serial.Port.BaudRate = opt.BaudRate;
                    app.Serial.Port.DataBits = opt.DataBits;

                    app.Serial.Port.Parity = opt.Parity;
                    app.Serial.Port.StopBits = opt.StopBits;

                    if (app.Serial.Secondary.Port.IsOpen == false)
                    {
                        app.Serial.Secondary.Port.ReadBufferSize = opt.SizeOfReadBuffer;
                        app.Serial.Secondary.Port.WriteBufferSize = opt.SizeOfWriteBuffer;

                        app.Serial.Secondary.Port.BaudRate = opt.BaudRate;
                        app.Serial.Secondary.Port.DataBits = opt.DataBits;

                        app.Serial.Secondary.Port.Parity = opt.Parity;
                        app.Serial.Secondary.Port.StopBits = opt.StopBits;
                    }
                    else
                        MessageBox.Show(this, "Настройки COM порта не будут применены, так как порт в данный момент открыт!",
                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show(this, "Настройки COM порта не будут применены, так как порт в данный момент открыт!", 
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Настраиваем стандартное TCP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void devTcpOpt_Click(object sender, EventArgs e)
        {
            TcpOptions options = new TcpOptions();

            options.Port = app.TcpDevManager.Port;
            options.CountClients = app.TcpDevManager.CountConnections;

            options.ClientBufferSize = app.TcpDevManager.ReceiverBufferSize;

            if (options.ShowDialog(this) == DialogResult.OK)
            {
                app.TcpDevManager.Port = options.Port;
                app.TcpDevManager.CountConnections = options.CountClients;

                app.TcpDevManager.ReceiverBufferSize = options.ClientBufferSize;
            }
        }

        /// <summary>
        /// Определяет общие настройки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commOpt_Click(object sender, EventArgs e)
        {
            CommonOptions opt = new CommonOptions();

            opt.PopitokSteniaPacketa = app.Serial.AttemptsToRead;
            opt.PopitokSteniaDannih = app.Serial.AttemptsCycled;

            opt.VremaNaStenie = app.Serial.Port.ReadTimeout;
            opt.VremaNaZapis = app.Serial.Port.WriteTimeout;

            opt.PeriodPosilkiKomand = app.Serial.Period;
            opt.VremaMesdyOtpravkoiPaketov = app.Serial.WaitTimeout;

            opt.TypeCRC = app.TypeCRC;
            opt.Mode = app.Mode;

            opt.Autorun = app.Autorun;
            opt.IsNotify = app.isNotify;

            if (opt.ShowDialog(this) == DialogResult.OK)
            {
                app.Serial.AttemptsToRead = opt.PopitokSteniaPacketa;
                app.Serial.AttemptsCycled = opt.PopitokSteniaDannih;

                app.Serial.Port.ReadTimeout = opt.VremaNaStenie;
                app.Serial.Port.WriteTimeout = opt.VremaNaZapis;

                app.Serial.Period = opt.PeriodPosilkiKomand;
                app.Serial.WaitTimeout = opt.VremaMesdyOtpravkoiPaketov;

                if (app.TypeCRC != opt.TypeCRC)
                {
                    app.Serial.ReCalculateCRC(opt.TypeCRC);
                }

                app.TypeCRC = opt.TypeCRC;
                app.Mode = opt.Mode;

                app.Autorun = opt.Autorun;
                app.isNotify = opt.IsNotify;
            }
        }

        /// <summary>
        /// Запустить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void run_Click(object sender, EventArgs e)
        {
            try
            {
                if (app.State == State.Stopped || app.State == State.Default)
                {
                    if (app.Mode == ApplicationMode.Active || app.Mode == ApplicationMode.Passive)
                    {
                        if (!app.Serial.Port.IsOpen)
                        {
                            app.Serial.Port.Open();                            
                        }

                        if (app.Serial.Secondary.IsActived)
                        {
                            if (!app.Serial.Secondary.Port.IsOpen)
                            {
                                app.Serial.Secondary.Port.Open();
                            }
                        }
                    }

                    app.Run();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Остановить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_Click(object sender, EventArgs e)
        {
            try
            {
                if (app.State == State.Running)
                {
                    app.Stop();
                    if (app.Serial.Port.IsOpen)
                    {
                        app.Serial.Port.Close();
                    }

                    if (app.Serial.Secondary.Port.IsOpen)
                    {
                        app.Serial.Secondary.Port.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Рестартануть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restart_Click(object sender, EventArgs e)
        {
            app.Stop();
            System.Threading.Thread.Sleep(2000);
            app.Run();
        }

        private void cmdsList_Click(object sender, EventArgs e)
        {
            CommList cmds = new CommList(app.TypeCRC);
            cmds.Packets = app.Serial.StaticPackets;

            if (cmds.ShowDialog(this) == DialogResult.OK)
            {
                app.Serial.ClearStatic();
                if (cmds.Packets != null)
                {
                    foreach (var item in cmds.Packets)
                    {
                        app.Serial.InsertPacketToStatic(item);
                    }
                }
            }
        }

        /// <summary>
        /// Для отладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Invoke(statuserPort);            
            notifyIcon.Icon = Properties.Resources._1496_wall;            
        }

        /// <summary>
        /// Показать протокол обмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void протоколОбменаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProtocolForm frm = new ProtocolForm(app);
            frm.Show();
        }

        private void статистикаПортаToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            PortStatisticsForm frm = new PortStatisticsForm(app);
            frm.Show();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {                
                Hide();

                if (hided == false)
                {
                    notifyIcon.Visible = false;
                    notifyIcon.Visible = true;

                    hided = true;
                }
            }
        }

        /// <summary>
        /// Выводим на БО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void настройкаВыводаНаБОToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BOForm frm = new BOForm(app);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
            }
        }

        /// <summary>
        /// Завершить работу приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void статистикаTcpToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            devTcpStatisticsForm frm = new devTcpStatisticsForm(app);
            frm.Show(this);
        }

        /// <summary>
        /// О программе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAboutProgrammForm_Click(object sender, EventArgs e)
        {
            AboutBox frm = new AboutBox();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
            }
        }

        /// <summary>
        /// Показываем контактную информацию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contacts_Click(object sender, EventArgs e)
        {
            ContactsForm frm = new ContactsForm();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
            }
        }

        /// <summary>
        /// Настраиваем каналы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void channelsConfig_Click(object sender, EventArgs e)
        {
            ChannelsForm frm = new ChannelsForm(app);
            frm.Show(this);
        }

        /// <summary>
        /// Настраиваем конвертер данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConverterConfig_Click(object sender, EventArgs e)
        {
            ConverterForm frm = new ConverterForm(app);
            frm.Show(this);
        }

        /// <summary>
        /// Настраиваем сохраняльщика данных в файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveTofile_Click(object sender, EventArgs e)
        {
            SaverForm frm = new SaverForm(app);
            frm.Show(this);
        }

        /// <summary>
        /// Сохранить конфигурацию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void сохранитьТекущуюКонфигурациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (app.State == State.Running)
                {
                    MessageBox.Show(this, "Необходимо предварительно приостановить обмен!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    app.SaveConfiguration();
                    MessageBox.Show(this, "Конфигурация сохранена", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// просматриваем каналы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewChannels_Click(object sender, EventArgs e)
        {
            ChannelsViewForm frm = new ChannelsViewForm(app);
            frm.Show();
        }

        /// <summary>
        /// просматриваем данные конвертора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void converterView_Click(object sender, EventArgs e)
        {
            ConverterViewForm frm = new ConverterViewForm(app);
            frm.Show();
        }

        private void статистикаФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaverFileStatisticsForm frm = new SaverFileStatisticsForm(app);
            frm.Show(this);
        }

        /// <summary>
        /// Показать главную форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowInTaskbar = true;

            Show();
            Activate();

            WindowState = FormWindowState.Normal;
        }

        private void настройкаПортовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ComSecondaryOptions frm = new ComSecondaryOptions();

                frm.PortName = app.Serial.Secondary.Port.PortName;
                frm.IsUsePort = app.Serial.Secondary.IsActived;

                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    if (app.Serial.Secondary.Port.IsOpen == false)
                    {
                        app.Serial.Secondary.Port.PortName = frm.PortName;
                        app.Serial.Secondary.IsActived = frm.IsUsePort;
                    }
                    else
                        MessageBox.Show(this, "Настройки COM порта не будут применены, так как порт в данный момент открыт!",
                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show(this, "Во время сохранения настроек порта возникла ошибка", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PortStatisticsForm2 frm = new PortStatisticsForm2(app);
            frm.Show();
        }
    }
}