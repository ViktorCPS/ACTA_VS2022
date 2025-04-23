using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

using ACTAMonitorLib;
using UI;

namespace ACTAMonitor
{
	/// <summary>
	/// Summary description for Monitor.
	/// </summary>
	public class Monitor
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
				mutex = new Mutex (false, mutexName);
			}
			catch 
			{
				MessageBox.Show("Failed to create a Mutex!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			// Wait 5 seconds if contended – in case another instance
			// of the program is in the process of shutting down.
			if (!mutex.WaitOne (TimeSpan.FromSeconds (5), false)) 
			{
				MessageBox.Show("ACTAMonitor is already running!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
            try
            {
                ACTAMonitorApplicationContext context = new ACTAMonitorApplicationContext();
                if (!context.isMainFormClosed)
                {
                    Application.Run(context);
                }
            }
            catch
            {
                MessageBox.Show("ACTAMonitor has encountered an error and can't be started!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
			finally { mutex.ReleaseMutex(); }
		}
	}
}
