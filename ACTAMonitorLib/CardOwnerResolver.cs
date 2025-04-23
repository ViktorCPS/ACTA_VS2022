using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Common;
using Util;
using TransferObjects;
using ACTAConfigManipulation;

namespace ACTAMonitorLib
{
	/// <summary>
	/// CardOwnerResolver class is responsible for running a thread that resolves a card owner based on the card serial number.
	/// Then it notifies the main thread to show information about the card owner on UI form and places the information
	/// in a queue for further processing by CardOwnerLocationUpdater to update current location of the card ownern in database.
	/// It is also responsible for reloading of employees and tags from the database (including employees photos from file system
    /// or database).
	/// </summary>
	public class CardOwnerResolver : IDisposable 
	{
		private AutoResetEvent autoResetEvent = new AutoResetEvent (false);
		private Thread worker;
		private object locker = new object();
		private Queue cardSerialNumbers = new Queue();
		private CardOwnerLocationUpdater cardOwnerLocationUpdater;

		private NotificationController notificationController;

		private DebugLog log = null;

		private List<EmployeeTO> employees = new List<EmployeeTO>();
		private List<TagTO> tags = new List<TagTO>();

		public CardOwnerResolver() 
		{
			//log = new DebugLog(Constants.logFilePath + System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id).ProcessName + "\\Log.txt");
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");

			notificationController = NotificationController.GetInstance();

			worker = new Thread(new ThreadStart(Work));
			worker.Name = "CardOwnerResolver";
			string  updateEmployeeLocation = ConfigurationManager.AppSettings["UpdateEmployeeLocation"];

            if (updateEmployeeLocation == null)
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                ResourceManager rm = new ResourceManager("ACTAMonitorLib.GateResource", typeof(Monitor).Assembly);

                MessageBox.Show(rm.GetString("noVisitorsParameters", culture));

                ConfigAdd conf = new ConfigAdd(rm.GetString("Visitors", culture));

                conf.ShowDialog();

                updateEmployeeLocation = ConfigurationManager.AppSettings["UpdateEmployeeLocation"];
            }

			if (updateEmployeeLocation != null && updateEmployeeLocation.ToUpper().Equals("YES")) 
			{
                cardOwnerLocationUpdater = new CardOwnerLocationUpdater();
			}
			worker.Start();
		}

        public void SetEmployeesAndTags(List<EmployeeTO> employees, List<TagTO> tags)
        {
            lock (locker)
            {
                this.employees = employees;
                this.tags = tags;
            }
        }

		public void EnqueueTask (CardOwner cardOwner) 
		{
			lock (locker) 
			{
				cardSerialNumbers.Enqueue (cardOwner);
			}
			autoResetEvent.Set();
		}

		public void Dispose() 
		{
			EnqueueTask (null); // Signal the consumer to exit.
			if (worker != null) 
			{
				worker.Join(); // Wait for the consumer's thread to finish.
			}
			autoResetEvent.Close(); // Release any OS resources.

			if (cardOwnerLocationUpdater != null) 
			{
				cardOwnerLocationUpdater.Dispose();
			}
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
						if (cardSerialNumbers.Count > 0) 
						{
							cardOwner = (CardOwner)cardSerialNumbers.Dequeue();
							if (cardOwner == null) return;
						}
					}

					if (cardOwner != null) 
					{
                        Reader r = new Reader();
                        r.RdrTO = cardOwner.reader;
						Console.WriteLine ("Resolver > reader: " + r.GetReaderAddress().ToString("X8") + 
							", card serial number: " + cardOwner.cardSerialNumber.ToString());

						int emplID = -1;
						uint tagID = cardOwner.cardSerialNumber;
						EmployeeTO currentEmployee = null;

                        lock (locker)
                        {
                            foreach (TagTO tag in this.tags)
                            {
                                if (tag.TagID.Equals(tagID) &&
                                    (tag.Status == Constants.statusActive || tag.Status == Constants.statusBlocked))
                                {
                                    emplID = tag.OwnerID;
                                    break;
                                }
                            }

                            if (!emplID.Equals(-1))
                            {
                                foreach (EmployeeTO empl in this.employees)
                                {
                                    if (empl.EmployeeID == emplID)
                                    {
                                        currentEmployee = empl;
                                        break;
                                    }
                                }
                            }
                        }

						if (currentEmployee == null) 
						{
							cardOwner.employee = new EmployeeTO();
							cardOwner.employee.FirstName = "NEPOZNATA KARTICA!";
						}
						else 
						{
							cardOwner.employee = currentEmployee;
							if (cardOwnerLocationUpdater != null) 
							{
								cardOwnerLocationUpdater.EnqueueTask(cardOwner);
							}
						}

						notificationController.CardOwnerObserved(cardOwner);
					}
					else 
					{
						autoResetEvent.WaitOne(); // No more cardSerialNumbers - wait for a signal
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
