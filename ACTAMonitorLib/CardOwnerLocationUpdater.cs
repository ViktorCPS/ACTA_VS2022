using System;
using System.Threading;
using System.Collections;

using Common;
using Util;

namespace ACTAMonitorLib
{
	/// <summary>
	/// CardOwnerLocationUpdater class is responsible for running a thread that updates current location of a card owner 
	/// in database.
	/// </summary>
	public class CardOwnerLocationUpdater : IDisposable 
	{
		private AutoResetEvent autoResetEvent = new AutoResetEvent (false);
		private Thread worker;
		private object locker = new object();
		private Queue cardOwners = new Queue();

		private NotificationController notificationController;

		private DebugLog log = null;

        // recovery parameters for updating employee location
        private static readonly int maxNumberOfRetries = 1;
        private int numberOfRetries = 0;

        /// <summary>
        /// Needed if a separate DB connection is used for updating employee location.
        /// Currently it is not used, since all updater threads uses monitor application DB connection 
        /// with updating employee location operation made thread-safe.
        /// </summary>
        //private Object DBConnection;

        public CardOwnerLocationUpdater()
        {
            //log = new DebugLog(Constants.logFilePath + System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id).ProcessName + "\\Log.txt");
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");

            notificationController = NotificationController.GetInstance();

            worker = new Thread(new ThreadStart(Work));
            worker.Name = "CardOwnerLocationUpdater";
            worker.Start();
        }

		public void EnqueueTask (CardOwner cardOwner) 
		{
			lock (locker) 
			{
				cardOwners.Enqueue (cardOwner);
			}
			autoResetEvent.Set();
		}

		public void Dispose() 
		{
			EnqueueTask (null); // Signal the consumer to exit.
			worker.Join(); // Wait for the consumer's thread to finish.
			autoResetEvent.Close(); // Release any OS resources.
		}

		private void Work() 
		{
			try 
			{
				while (true) 
				{
					CardOwner cardOwner = null;
					lock (locker) 
					{
						if (cardOwners.Count > 0) 
						{
							cardOwner = (CardOwner)cardOwners.Dequeue();
							if (cardOwner == null) return;
						}
					}

					if (cardOwner != null) 
					{
						Console.WriteLine ("LocationUpdater > card owner Id: " + cardOwner.employee.EmployeeID.ToString());

						int employeeID = cardOwner.employee.EmployeeID;
						int readerID = cardOwner.reader.ReaderID;
						int antennaNum = cardOwner.antennaNum;
						bool updated = true;
                        DateTime eventTime = cardOwner.eventTime;   // DC 1.9.2008.

						try
						{				
							//updated = (new EmployeeLocation(DBConnection)).Update(employeeID, readerID, antennaNum, 0, DateTime.Now, 0);
                            //updated = (new EmployeeLocation()).Update(employeeID, readerID, antennaNum, 0, DateTime.Now, 0);
                            updated = (new EmployeeLocation()).Update(employeeID, readerID, antennaNum, 0, eventTime, 0);   // DC 1.9.2008.
						}
						catch(Exception ex)
						{
							log.writeLog(ex);
							updated = false;
						}
                        if (updated)
                        {
                            notificationController.DataProcessingStateChanged("Card owner " + employeeID.ToString() + " location updated.");
                            numberOfRetries = 0;
                        }
                        else
                        {
                            notificationController.DataProcessingStateChanged("Error updating card owner " + employeeID.ToString() + " location!");
                            if (numberOfRetries <= maxNumberOfRetries)
                            {
                                EnqueueTask(cardOwner);
                                log.writeLog(DateTime.Now + " CardOwnerLocationUpdater: " + "Enqueue task for card owner " + employeeID.ToString() + ".\n");
                                numberOfRetries++;
                            }
                        }
					}
					else 
					{
						autoResetEvent.WaitOne(); // No more cardOwners - wait for a signal
					}
				}
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " The thread " + Thread.CurrentThread.Name + " has exited with exception: " + 
					ex.Message + "\n\n" + ex.StackTrace + "\n\n");
			}
		}
	}
}
