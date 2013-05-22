using System;
using System.Threading;
using System.ServiceModel;
using System.Windows.Forms;

using WCF;

namespace DeviceManager.Runner
{
    public static class Program
    {
        /// <summary>
        /// Идентификационный номер системного мьютекса, по которому определяется запещоно или нет приложение
        /// </summary>
        private const string identifier = "44245c9b-e3da-4dc4-9be2-6a8c95663397";

        private static Mutex mutex = null;              // определяет запуск приложения
        private static bool isNotRunning = false;       // содержит значение на основе которого определяет возможность запуска программы        

        private static MainForm frm = null;             // главная форм а приложения
        private static Application application;         // основное приложение

        [STAThread]
        public static void Main(string[] Args)
        {
            try
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);


                mutex = new Mutex(true, identifier, out isNotRunning);
                if (isNotRunning)
                {
                    System.Windows.Forms.Application.EnableVisualStyles();
                    System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                    System.Windows.Forms.Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

                    application = Application.CreateInstance();
                    if (application != null)
                    {
                        application.OnExit += new EventHandler(application_OnExit);
                        application.Initialize();

                        frm = new MainForm(application);
                        frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);

                        if (application.isNotify)
                        {
                            frm.ShowInTaskbar = false;
                            frm.WindowState = FormWindowState.Minimized;
                        }

                        System.Windows.Forms.Application.Run(frm);
                    }
                }
                else
                {
                    // 13.02.2013 - убрано, так как повторный запуск стал нормальным явлением!
                    //MessageBox.Show("Приложение уже запущено", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Обработчик ошибки
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="args">Аргументы события</param>
        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            try
            {   
                Exception e = args.ExceptionObject as Exception;
                SoftwareDevelopmentKit.Log.Journal journal = SoftwareDevelopmentKit.Log.Journal.CreateInstance();
                if (e != null)
                {
                    
                    if (journal != null)
                    {
                        journal.Write(string.Format("{0} stack {1} is terminating {2}", e.Message, e.StackTrace, args.IsTerminating),
                             System.Diagnostics.EventLogEntryType.Error);
                    }
                }
                else
                {
                    journal.Write("e == null", System.Diagnostics.EventLogEntryType.Error);
                }
            }
            catch { }
        }

        /// <summary>
        /// Возникает во время завершения программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Необходимо завершить работу программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void application_OnExit(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            application.Service.close();
        }

        /// <summary>
        /// Закрываем форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}