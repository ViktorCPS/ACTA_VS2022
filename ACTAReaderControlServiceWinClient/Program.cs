using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace ACTAReaderControlServiceWinClient
{
    static class Program
    {
        private static Mutex mutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string mutexName = "Local\\" +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            try
            {
                mutex = new Mutex(false, mutexName);
            }
            catch
            {
                MessageBox.Show("Failed to create a Mutex!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Wait 5 seconds if contended – in case another instance
            // of the program is in the process of shutting down.
            if (!mutex.WaitOne(TimeSpan.FromSeconds(5), false))
            {
                MessageBox.Show("ReaderControlServiceWinClient is already running!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            try
            {
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ACTAReaderControlServiceWinClient.ReaderControlMainForm());
            }
            catch
            {
            }
            finally { mutex.ReleaseMutex(); }
        }
    }
}