using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace LRRremote
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool newMutexCreated = false;
                string mutexName = "Local\\" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                Mutex mutex = null;
                try
                {
                    // Create a new mutex object with a unique name
                    mutex = new Mutex(false, mutexName, out newMutexCreated);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace + "\n\n" + "Application Exiting...", "Exception thrown");
                    Application.Exit();
                }

                if (newMutexCreated)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new LRRremoteMainForm());
                }
                else
                {
                    MessageBox.Show("The application is already running!", "Info");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}