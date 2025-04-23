using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Common;
using Util;
using TransferObjects;
using ReaderRemoteDataAccess;

namespace ReaderManagement
{
	/// <summary>
	/// Process log data: 
	/// XML files - > log_tmp, 
	/// log_tmp - > log, 
	/// log -> passes, 
	/// passes - > io_pairs
	/// </summary>
	public class DataProcessingManager
	{
		private Thread processManagerThread;
		private static DataProcessingManager managerInstance;
		private bool isProcessing = false;
		private int PROCESSINGSLEEPINTERVAL = -1;
		private static string FILESUFIX = "mmss";
        public static bool processAllTags = false;
        public static bool recordsToProcess = false;
        public bool doProcess = false;
        object locker = new object();
        System.Timers.Timer timer;
        Dictionary<int, EmployeeTO> specialEmpl = new Dictionary<int,EmployeeTO>();
        Dictionary<int, EmployeeTO> extraOrdinaryEmpl = new Dictionary<int, EmployeeTO>();
        Dictionary<int, PassTypeTO> allTypes = new Dictionary<int, PassTypeTO>();
        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> insertedPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
        private Dictionary<int, ReaderTO> readerList = new Dictionary<int, ReaderTO>(); //tamara27.4.2018.
        private List<String> gatesForProcess = Constants.GetInfoFromGates;//tamara27.4.2018.


		// Debug
		DebugLog debug;
		// Controller instance
		public NotificationController Controller;

        // time to check and process medical check visits
        private DateTime nextMedicalCheckTime = Constants.MedicalCheckStartTime;
        private long medicalCheckTimeout = Constants.MedicalCheckTimeout;  // in min

		public bool IsProcessing
		{
			get { return isProcessing; }
		}

        public bool DoProcess
        {
            get
            {
                lock (locker)
                {
                    return doProcess;
                }
            }
            set
            {
                lock (locker)
                {
                    doProcess = value;
                }
            }
        }

		protected DataProcessingManager()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			//PROCESSINGSLEEPINTERVAL = Convert.ToInt32(ConfigurationManager.AppSettings["PROCESSINGSLEEPINTERVAL"]);
            PROCESSINGSLEEPINTERVAL = Convert.ToInt32(Constants.PROCESSINGSLEEPINTERVAL.ToString());
			InitializeObserverClient();

            // get licence and check if all tags should be processed
            if (Common.Misc.getLicenceModuls(null).Contains((int)Constants.Moduls.ProcessAllTags))
            {
                processAllTags = true;
            }
            else
            {
                processAllTags = false;
            }
            // get licence and check if how many logs should be inserted at one time
            if (Common.Misc.getLicenceModuls(null).Contains((int)Constants.Moduls.RecordsToProcess))
            {
                recordsToProcess = true;
            }
            else
            {
                recordsToProcess = false;
            }

            timer = new System.Timers.Timer(900000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

		public static DataProcessingManager GetInstance()
		{
			try
			{
				if (managerInstance == null)
				{
					managerInstance = new DataProcessingManager();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return managerInstance;
		}

		public bool StartLogProcessing()
		{
			bool started = false;

			try
			{
				if (!this.isProcessing)
				{

                    // set next medical check time
                    while (nextMedicalCheckTime < DateTime.Now)
                        nextMedicalCheckTime = nextMedicalCheckTime.AddMinutes(medicalCheckTimeout);

					this.isProcessing = true;
					processManagerThread = new Thread(new ThreadStart(Processing));
					processManagerThread.Start();
					Controller.DataProcessingStateChanged("Thread: processing started");
					System.Console.WriteLine("++++ Processing Log files Started at:  " + DateTime.Now.ToString("hh:mm:ss") + "\n");
					debug.writeLog("++++ Processing Log files Started at:  " + DateTime.Now.ToString("hh:mm:ss") + "\n");  
					started = true;
				}
				else
				{
					System.Console.WriteLine("*** Processing Log files Started already !!! \n");
					debug.writeLog("*** Processing Log files Started already !!! \n");  
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".StartLogProcessing() : " + ex.StackTrace + "\n");  
			}

			return started;
		}

		public bool StopLogProcessing()
		{
			bool stopped = false;

			try
			{
                timer.Enabled = false;
				this.isProcessing = false;
				if (processManagerThread.Join(new TimeSpan(1,0,0)))
				{
					Controller.DataProcessingStateChanged("Thread: processing finished");
					System.Console.WriteLine("Processing thread finished \n");
				}
				else 
				{
					Controller.DataProcessingStateChanged("Thread: processing aborted");
					System.Console.WriteLine("Processing thread aborted \n");
					processManagerThread.Abort();
				}
				System.Console.WriteLine("---- Processing Log files Stopped at:  " + DateTime.Now.ToString("hh:mm:ss") + "\n");
				debug.writeLog("---- Processing Log files Stopped at:  " + DateTime.Now.ToString("hh:mm:ss") + "\n"); 
				stopped = true;

			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".StopLogProcessing() : " + ex.StackTrace + "\n");  
				stopped = false;
			}

			return stopped;
		}

		private bool unprocessedFilesFound()
		{
			bool found = false;

			try
			{
				if (Directory.GetFiles(Constants.unprocessed).Length != 0)
				{
					found = true;
				}
			}
			catch(DirectoryNotFoundException dex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".unprocessedFilesFound() : " + dex.Message +
					"Can't reach unprocessed directory");  
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".unprocessedFilesFound() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace);
				throw ex;
			}

			return found;
		}

        private bool unprocessedMobileFilesFound()
        {
            bool found = false;

            try
            {
                if (Directory.Exists(Constants.unprocessedMobile) && Directory.GetFiles(Constants.unprocessedMobile).Length != 0)
                {
                    found = true;
                }
            }
            catch (DirectoryNotFoundException dex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".unprocessedMobileFilesFound() : " + dex.Message +
                    "Can't reach unprocessed directory");
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".unprocessedMobileFilesFound() : Message: " + ex.Message + "/n StackTrace:" + ex.StackTrace);
                throw ex;
            }

            return found;
        }

		/// <summary>
		/// if any file found in the "unprocessed" directory, process it.
		/// </summary>
		private void Processing()
		{
            ApplUserTO dpUser = new ApplUserTO();
                        dpUser.UserID = Constants.dataProcessingUser;
                        NotificationController.SetLogInUser(dpUser);
			const int MAX_NUMBER_OF_RETRIES = 1440;
			int numberOfRetries = 1;
			while ((numberOfRetries <= MAX_NUMBER_OF_RETRIES) && isProcessing) 
			{
				debug.writeLog(DateTime.Now.ToString("hh:mm:ss") + " A new processing is started: " + 
							   numberOfRetries.ToString() + "\n");
				try
				{
					while (isProcessing)
                    {
                       
                        //Get camera snapshots from snapshots folder and put them into database
                        Controller.DataProcessingStateChanged("Pushing snapshots");
                        DataManager dm = new DataManager();
                        dm.pushCameraSnapshotsIntoDatabase();
                        Controller.DataProcessingStateChanged("");

                        if (unprocessedFilesFound() || unprocessedMobileFilesFound() || DoProcess)
                        {
                            // Sanja 04.07.2013. check if system is closed

                            /* 
                             * REGULAR PERIODICAL closing event should prevent changes during auto closing pairs after midnight and will be set around midnight (23:55 - 00:30)
                             * DP is working normally, ACTAWeb and ACTAAdmin are blocked for changes 
                             * DEMANDED closing event - DP must finish his work, set closing event dp engine state status to FINISHED, and after that no processing until system is opened again
                             * ACTAWeb is blocked for changes as soon as system is closed, no metter what status of DP engine is
                             * ACTAAdmin is blocked for changes until DP set his status to FINISHED, after DP finshed his work, ACTAAdmin can do changes through Massive input                              
                             */

                            SystemClosingEventTO eventTO = new SystemClosingEventTO();

                            try
                            {
                                List<SystemClosingEventTO> closingList = new SystemClosingEvent().Search(DateTime.Now, DateTime.Now);

                                foreach (SystemClosingEventTO evtTO in closingList)
                                {
                                    if (evtTO.Type.Trim().ToUpper() == Constants.closingEventTypeDemanded.Trim().ToUpper())
                                        eventTO = evtTO;
                                }

                                if (eventTO.EventID != -1)
                                {
                                    // if DP engine state is set to FINISHED do not process
                                    if (eventTO.DPEngineState.Trim().ToUpper() == Constants.DPEngineState.FINISHED.ToString().Trim().ToUpper())
                                    {
                                        GC.Collect();
                                        Common.DataManager.CloseDataBaseConnection();
                                        Thread.Sleep(PROCESSINGSLEEPINTERVAL * 1000);
                                        continue;
                                    }

                                    // if DP engine state not set, set it to STARTED
                                    if (eventTO.DPEngineState.Trim() == "")
                                    {
                                        eventTO.DPEngineState = Constants.DPEngineState.STARTED.ToString().Trim();
                                        eventTO.DPEngineStateModifiedTime = DateTime.Now;

                                        SystemClosingEvent evt = new SystemClosingEvent();
                                        evt.EventTO = eventTO;

                                        evt.Update(true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                debug.writeLog(DateTime.Now.ToString("hh:mm:ss") + "Exception in checking system closing events: " + ex.Message + "\n");
                            }

                            if (!DoProcess)
                            {
                                timer.Enabled = false;
                            }

                            debug.writeBenchmarkLog(DateTime.Now + " PROCESSING STARTED!");

                            // get all special and extra ordinary employees
                            getSpecialEmployees();
                            getExtraordinaryEmployees();

                            // get all rules
                            rulesDict = new Common.Rule().SearchWUEmplTypeDictionary();

                            //get all time Schemas
                            Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = new TimeSchema().getDictionary();

                            debug.writeLog("+ ImportLog - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                            // Import reder and mobile reader log from XML File
                            ImportLogs();
                            debug.writeLog("- ImportLog - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");

                            // Za SMP postavljanje prvog IN i poslednji OUT za danasnji dan na is_wrk_hrs=1, svi ostali izmedju idu na is_wrk_hrs=0
                            if (ConfigurationManager.AppSettings["Company"].ToLower().Equals("smp"))
                            {
                                debug.writeLog("--- Postavljanje prvi IN i poslednji OUT na is_wrk_hrs=1 - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                                ProveraIN_OUT();
                                debug.writeLog("--- Postavljanje prvi IN i poslednji OUT na is_wrk_hrs=1 - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n"); 
                            }
                            // Za Delta Danube postavljanje satnica na osnovu prolazaka 
                            if (ConfigurationManager.AppSettings["Company"].ToLower().Equals("danube") || ConfigurationManager.AppSettings["Company"].ToLower().Equals("delta") || ConfigurationManager.AppSettings["Company"].ToLower().Equals("kovin"))
                            {
                                debug.writeLog("--- Postavljanje prvi IN i poslednji OUT na is_wrk_hrs=1 - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                                ProveraIN_OUT_Danube();
                                debug.writeLog("--- Postavljanje prvi IN i poslednji OUT na is_wrk_hrs=1 - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n"); 
                                debug.writeLog("--- Postavljanje satnica flexi - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                                PostavljanjeSatnica();
                                debug.writeLog("--- Postavljanje satnica flexi - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                            }


                            // Pass - > IOPairs
                            debug.writeLog("--- ClassifyPasses - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                            IOPair ioPair = new IOPair();
                            Controller.DataProcessingStateChanged("Classifying passes");
                            ioPair.ClassifyPasses(specialEmpl, extraOrdinaryEmpl, Controller, dictTimeSchema);
                            Controller.DataProcessingStateChanged("");
                            debug.writeLog("--- ClassifyPasses - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");

                            //take timeSchemasAgain it been change in ClassifyPasses
                            dictTimeSchema = new TimeSchema().getDictionary();

                            // IOPairs - > IOPairsProcessed
                            debug.writeLog("--- ProcessingIOPairs - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                            Controller.DataProcessingStateChanged("Processing IOPairs");
                            ProcessingIOPairs(dictTimeSchema);
                            debug.writeLog("--- ProcessingIOPairs - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");

                            // Auto close night overtime work
                            debug.writeLog("--- Auto close night overtime - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                            Controller.DataProcessingStateChanged("Auto close night overtime");
                            AutoCloseNightOvertime(dictTimeSchema);
                            debug.writeLog("--- Auto close night overtime - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");

                            // Change unjustified to regular work to extra ordinary employees
                            debug.writeLog("--- Change unjustified to regular work to extra ordinary employees - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                            Controller.DataProcessingStateChanged("Absence to regular work");
                            AbsenceToWork(dictTimeSchema);
                            debug.writeLog("--- Change unjustified to regular work to extra ordinary employees - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");

                            // Transfering meals
                            debug.writeLog("--- Transfering meals - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                            Controller.DataProcessingStateChanged("Transfering meals");
                            TransferingMeals();
                            debug.writeLog("--- Transfering meals - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");

                            // Filling overtime holes for SMP
                            if (ConfigurationManager.AppSettings["Company"].ToLower().Equals("smp"))
                            {
                                debug.writeLog("--- Filling overtime holes - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                                FillingOvertimeHoles();
                                debug.writeLog("--- Filling overtime holes - FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                            }

                            if (isMedicalCheckTime())
                            {
                                // check if there is medical check modul
                                if (Common.Misc.getLicenceModuls(null).Contains((int)Constants.Moduls.MedicalCheck))
                                {
                                    // Processing visits
                                    debug.writeLog("--- Processing visits - STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                                    Controller.DataProcessingStateChanged("Processing visits");
                                    ProcessingVisits();
                                    debug.writeLog("--- Processing visits- FINISHED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                                }

                                nextMedicalCheckTime = nextMedicalCheckTime.AddMinutes(medicalCheckTimeout);
                            }

                            numberOfRetries = 0;
                            Controller.DataProcessingStateChanged("Thread: processing started");

                            DoProcess = false;
                            timer.Enabled = true;
                            debug.writeBenchmarkLog(DateTime.Now + " PROCESSING FINISHED! TIMER ENABLED!");

                            try
                            {
                                if (eventTO.EventID != -1)
                                {
                                    // if DP engine state is set to STARTED, set it to FINISHED
                                    if (eventTO.DPEngineState.Trim().ToUpper() == Constants.DPEngineState.STARTED.ToString().Trim().ToUpper())
                                    {
                                        eventTO.DPEngineState = Constants.DPEngineState.FINISHED.ToString().Trim();
                                        eventTO.DPEngineStateModifiedTime = DateTime.Now;

                                        SystemClosingEvent evt = new SystemClosingEvent();
                                        evt.EventTO = eventTO;

                                        evt.Update(true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                debug.writeLog(DateTime.Now.ToString("hh:mm:ss") + "Exception in closing system: " + ex.Message + "\n");
                            }
                        
                          }
						GC.Collect();
						Common.DataManager.CloseDataBaseConnection();
						Thread.Sleep(PROCESSINGSLEEPINTERVAL * 1000);	
					}
				}
				catch(DataProcessingException dpex)
				{
					if (dpex.Number == 1)
					{
						numberOfRetries = MAX_NUMBER_OF_RETRIES;
						Controller.DataProcessingStateChanged("DataProcessingException 1: " + dpex.Message);
					}
				
				}
				catch(ThreadAbortException taex)
				{
					debug.writeLog(DateTime.Now.ToString("hh:mm:ss") + "Processing abort exception in: " + 
						this.ToString() + ".Processing() : " + taex.Message + "\n");  
				}
				catch(Exception ex)
				{
					debug.writeLog(DateTime.Now + " Exception in: " + 
						this.ToString() + ".Processing() : " + ex.StackTrace + "\n");  
				}
				finally 
				{
					if (isProcessing) 
					{
						// exception occurred
						Controller.DataProcessingStateChanged("Thread: processing restarted, retry no. " + numberOfRetries.ToString());
						Controller.DataProcessingStateChanged("");
						numberOfRetries++;
						Thread.Sleep(60000);
						Common.DataManager.CloseDataBaseConnection();
					}
				}
			}
            if (numberOfRetries > MAX_NUMBER_OF_RETRIES)
            {
                isProcessing = false;
                Controller.DataProcessingStateChanged("Thread: after " + MAX_NUMBER_OF_RETRIES.ToString() + " retries processing stopped");
            }
            else
            {
                DoProcess = false;
                timer.Enabled = true;
                debug.writeBenchmarkLog(DateTime.Now + " TIMER ENABLED!");
            }
		}

        private void ProveraIN_OUT_Danube()
        {
            List<int> emplIDs = new Employee().SearchIDs();
            List<int> idsWithPasses = new List<int>();
            DateTime date = DateTime.Now.Date;
            try
            {
                foreach (int id in emplIDs)
                {
                    List<PassTO> listaIN = new Pass().getListOfIN_OUTforDay(date, id, "IN");
                    List<PassTO> listaOUT = new Pass().getListOfIN_OUTforDay(date, id, "OUT");
                    if (listaIN.Count > 1 || listaOUT.Count > 1)
                    {
                        string passIDsIN = "";
                        foreach (PassTO passIN in listaIN)
                        {
                            if (listaIN.IndexOf(passIN) == 0)
                                continue;
                            if (passIN.IsWrkHrsCount == 0)
                                continue;
                            passIDsIN += passIN.PassID + ",";
                            if (!idsWithPasses.Contains(id))
                                idsWithPasses.Add(id);
                        }
                        if (passIDsIN.Length > 2)
                            passIDsIN = passIDsIN.Substring(0, passIDsIN.Length - 1);
                        string passIDsOUT = "";
                        foreach (PassTO passOUT in listaOUT)
                        {
                            if (listaOUT.IndexOf(passOUT) == 0)
                                continue;
                            if (passOUT.IsWrkHrsCount == 0)
                                continue;
                            passIDsOUT += passOUT.PassID + ",";
                            if (!idsWithPasses.Contains(id))
                                idsWithPasses.Add(id);
                        }
                        if (passIDsOUT.Length > 2)
                            passIDsOUT = passIDsOUT.Substring(0, passIDsOUT.Length - 1);
                        if (passIDsIN.Length > 1)
                            new Pass().updateListPassesToWrkHrs0(passIDsIN);
                        if (passIDsOUT.Length > 1)
                            new Pass().updateListPassesToWrkHrs0(passIDsOUT);
                    }
                }
                foreach (int id in idsWithPasses)
                {
                    new IOPair().deleteForDay(id, date);
                    new IOPairProcessed().Delete(id, date, true);
                }
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " +
                   this.ToString() + ".ProveraIN_OUT() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
            }
        }

        private void PostavljanjeSatnica()
        {
            EmployeesTimeSchedule ets = new EmployeesTimeSchedule();
            Pass passsche = new Pass();

            List<EmployeeTO> employees = new List<EmployeeTO>();
            List<PassTO> passes = new List<PassTO>();

            Employee emplo = new Employee();

            employees = emplo.Search();

            foreach (EmployeeTO empl in employees)
            {
                if (empl.WorkingGroupID != 20)
                    continue;
                EmployeeTimeScheduleTO ts = new EmployeeTimeScheduleTO();
                passes = passsche.getPassesForEmployeeSched(empl.EmployeeID);
                foreach (PassTO psto in passes)
                {
                    if (psto.EventTime.Date == DateTime.Today.Date && psto.Direction.Equals("IN") && psto.PairGenUsed == 0 && psto.IsWrkHrsCount==1)
                    {
                        EmployeeTimeScheduleTO etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date);
                        if (etsTO.TimeSchemaID != -1)
                            ets.DeleteFromToSchedule(empl.EmployeeID, psto.EventTime.Date, psto.EventTime.Date, "DP SERVICE", true);
                        DateTime currDate = psto.EventTime.Date;
                        etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date);
                        if (etsTO.TimeSchemaID == -1)
                        {
                            List<PassTO> listPasses = passsche.getPassesForDayForEmployee(empl.EmployeeID, currDate.AddDays(-1));
                            bool breakUnos = false;
                            foreach (PassTO item in listPasses)
                            {
                                if (item.EventTime.Hour >= 20 && item.Direction.Equals("IN") && item.IsWrkHrsCount == 1)
                                {
                                    breakUnos = true;
                                    break;
                                }
                            }
                            switch (psto.EventTime.Hour)
                            {
                                case 3:case 4:
                                    if (breakUnos)
                                        continue;
                                    ets.Save(empl.EmployeeID, psto.EventTime.Date, 1008, 0);
                                    etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(1));
                                    if (etsTO.TimeSchemaID == -1)
                                        ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(1), 0, 0);
                                    break;
                                case 5:case 6:
                                    if (breakUnos)
                                        continue;
                                    ets.Save(empl.EmployeeID, psto.EventTime.Date, 1009, 0);
                                    etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(1));
                                    if (etsTO.TimeSchemaID == -1)
                                        ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(1), 0, 0);
                                    break;
                                case 7:case 8:case 9:
                                    if (breakUnos)
                                        continue;
                                    ets.Save(empl.EmployeeID, psto.EventTime.Date, 1019, 0);
                                    etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(1));
                                    if (etsTO.TimeSchemaID == -1)
                                        ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(1), 0, 0);
                                    break;
                                case 10:case 11:case 12:
                                    if (breakUnos)
                                        continue;
                                    ets.Save(empl.EmployeeID, psto.EventTime.Date, 1010, 0);
                                    etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(1));
                                    if (etsTO.TimeSchemaID == -1)
                                        ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(1), 0, 0);
                                    break;
                                case 13:case 14:case 15:case 16:
                                    if (breakUnos)
                                        continue;
                                    ets.Save(empl.EmployeeID, psto.EventTime.Date, 1011, 0);
                                    etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(1));
                                    if (etsTO.TimeSchemaID == -1)
                                        ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(1), 0, 0);
                                    break;
                                case 18:case 19:case 20: case 21: case 22: case 23:
                                    int brNocnih = 1;
                                    foreach (PassTO item in listPasses)
                                    {
                                        if (item.EventTime.Hour >= 19)
                                        {
                                            brNocnih++;
                                            break;
                                        }
                                    }
                                    if (brNocnih == 2)
                                    {
                                        listPasses = passsche.getPassesForDayForEmployee(empl.EmployeeID, currDate.AddDays(-2));
                                        foreach (PassTO item in listPasses)
                                        {
                                            if (item.EventTime.Hour >= 19)
                                            {
                                                brNocnih++;
                                                break;
                                            }
                                        }
                                        if (brNocnih == 3)
                                        {
                                            listPasses = passsche.getPassesForDayForEmployee(empl.EmployeeID, currDate.AddDays(-3));
                                            foreach (PassTO item in listPasses)
                                            {
                                                if (item.EventTime.Hour >= 19)
                                                {
                                                    brNocnih++;
                                                    break;
                                                }
                                            }
                                            if (brNocnih == 4)
                                            {
                                                listPasses = passsche.getPassesForDayForEmployee(empl.EmployeeID, currDate.AddDays(-4));
                                                foreach (PassTO item in listPasses)
                                                {
                                                    if (item.EventTime.Hour >= 19)
                                                    {
                                                        brNocnih++;
                                                        break;
                                                    }
                                                }
                                                if (brNocnih == 5)
                                                {
                                                    listPasses = passsche.getPassesForDayForEmployee(empl.EmployeeID, currDate.AddDays(-5));
                                                    foreach (PassTO item in listPasses)
                                                    {
                                                        if (item.EventTime.Hour >= 19)
                                                        {
                                                            brNocnih++;
                                                            break;
                                                        }
                                                    }
                                                    if (brNocnih == 6)
                                                    {
                                                        listPasses = passsche.getPassesForDayForEmployee(empl.EmployeeID, currDate.AddDays(-6));
                                                        foreach (PassTO item in listPasses)
                                                        {
                                                            if (item.EventTime.Hour >= 19)
                                                            {
                                                                brNocnih++;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    bool brisanje = false;
                                    switch (brNocnih)
                                    {
                                        case 1:
                                            ets.Save(empl.EmployeeID, psto.EventTime.Date, 1012, 0);
                                            break;
                                        case 2:
                                            etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(-1));
                                            if (etsTO.TimeSchemaID != -1)
                                                ets.Update(empl.EmployeeID, psto.EventTime.Date.AddDays(-1), 1013, 0, true);
                                            else
                                            {
                                                ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(-1), 1013, 0);
                                                brisanje = true;
                                            }
                                            break;
                                        case 3:
                                            etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(-2));
                                            if (etsTO.TimeSchemaID != -1)
                                                ets.Update(empl.EmployeeID, psto.EventTime.Date.AddDays(-2), 1014, 0, true);
                                            else
                                            {
                                                ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(-2), 1014, 0);
                                                brisanje = true;
                                            }
                                            break;
                                        case 4:
                                            etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(-3));
                                            if (etsTO.TimeSchemaID != -1)
                                                ets.Update(empl.EmployeeID, psto.EventTime.Date.AddDays(-3), 1015, 0, true);
                                            else
                                            {
                                                ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(-3), 1015, 0);
                                                brisanje = true;
                                            }
                                            break;
                                        case 5:
                                            etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(-4));
                                            if (etsTO.TimeSchemaID != -1)
                                                ets.Update(empl.EmployeeID, psto.EventTime.Date.AddDays(-4), 1016, 0, true);
                                            else
                                            {
                                                ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(-4), 1016, 0);
                                                brisanje = true;
                                            }
                                            break;
                                        case 6:
                                            etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(-5));
                                            if (etsTO.TimeSchemaID != -1)
                                                ets.Update(empl.EmployeeID, psto.EventTime.Date.AddDays(-5), 1017, 0, true);
                                            else
                                            {
                                                ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(-5), 1017, 0);
                                                brisanje = true;
                                            }
                                            break;
                                        case 7:
                                            etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(-6));
                                            if (etsTO.TimeSchemaID != -1)
                                                ets.Update(empl.EmployeeID, psto.EventTime.Date.AddDays(-6), 1018, 0, true);
                                            else
                                            {
                                                ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(-6), 1018, 0);
                                                brisanje = true;
                                            }
                                            break;
                                    }
                                    DateTime date = psto.EventTime.Date.AddDays(1);
                                    DateTime toDate = psto.EventTime.Date.AddDays(-(--brNocnih));
                                    while (date > toDate)
                                    {
                                        etsTO = ets.Find(empl.EmployeeID, date);
                                        if (etsTO.TimeSchemaID != -1)
                                            ets.DeleteFromToSchedule(empl.EmployeeID, date, date, "DP SERVICE", true);
                                        date = date.AddDays(-1);
                                    }
                                    if(brisanje)
                                        while (toDate < psto.EventTime.Date)
                                        {
                                            ObrisiParove_PostaviParoveNa0(toDate, empl.EmployeeID);
                                            toDate = toDate.AddDays(1);
                                        }
                                    etsTO = ets.Find(empl.EmployeeID, psto.EventTime.Date.AddDays(2));
                                    if (etsTO.TimeSchemaID == -1)
                                        ets.Save(empl.EmployeeID, psto.EventTime.Date.AddDays(2), 0, 0);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void ObrisiParove_PostaviParoveNa0(DateTime date, int emplID)
        {
            IOPair iop = new IOPair();
            iop.deleteForAutoTimeSchema(emplID, date, true);
            IOPairProcessed iopp = new IOPairProcessed();
            iopp.deleteIoPairProcForAutoTimeSchema(emplID, date, true);
            Pass pass = new Pass();
            pass.updatePassesOnUnprocessed(emplID, date, true);
        }

        private void ProveraIN_OUT()
        {
            List<int> emplIDs = new Employee().SearchIDs();
            List<int> idsWithPasses = new List<int>();
            DateTime date = DateTime.Now.Date;
            try
            {
                foreach (int id in emplIDs)
                {
                    List<PassTO> listaIN = new Pass().getListOfIN_OUTforDay(date, id, "IN");
                    List<PassTO> listaOUT = new Pass().getListOfIN_OUTforDay(date, id, "OUT");
                    List<PassTO> listaINafter5 = new List<PassTO>();
                    if (listaIN.Count > 1 || listaOUT.Count > 1)
                    {
                        string passIDsIN = "";
                        foreach (PassTO passIN in listaIN)
                        {
                            if (passIN.EventTime.Hour < 5 && passIN.IsWrkHrsCount != 0)
                            {
                                passIDsIN += passIN.PassID + ",";
                                if (!idsWithPasses.Contains(id))
                                    idsWithPasses.Add(id);
                            }
                            else if(passIN.IsWrkHrsCount!=0)
                                listaINafter5.Add(passIN);
                        }
                        foreach (PassTO passIN in listaINafter5)
                        {
                            if (listaINafter5.IndexOf(passIN) == 0 && passIN.EventTime.Hour >= 5)
                                continue;
                            if (passIN.IsWrkHrsCount == 0)
                                continue;
                            passIDsIN += passIN.PassID + ",";
                            if (!idsWithPasses.Contains(id))
                                idsWithPasses.Add(id);
                        }
                        if (passIDsIN.Length > 2)
                            passIDsIN = passIDsIN.Substring(0, passIDsIN.Length - 1);
                        string passIDsOUT = "";
                        foreach (PassTO passOUT in listaOUT)
                        {
                            if (listaOUT.IndexOf(passOUT) == 0)
                                continue;
                            if (passOUT.IsWrkHrsCount == 0)
                                continue;
                            passIDsOUT += passOUT.PassID + ",";
                            if (!idsWithPasses.Contains(id))
                                idsWithPasses.Add(id);
                        }
                        if (passIDsOUT.Length > 2)
                            passIDsOUT = passIDsOUT.Substring(0, passIDsOUT.Length - 1);
                        if (passIDsIN.Length > 1)
                            new Pass().updateListPassesToWrkHrs0(passIDsIN);
                        if (passIDsOUT.Length > 1)
                            new Pass().updateListPassesToWrkHrs0(passIDsOUT);
                    }
                }
                foreach (int id in idsWithPasses)
                {
                    new IOPair().deleteForDay(id, date);
                    new IOPairProcessed().Delete(id, date, true);
                }
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".ProveraIN_OUT() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
            }
        }

        private void FillingOvertimeHoles()
        {
            List<int> emplIDs = new Employee().SearchIDs();
            DateTime from = DateTime.Now.Date.AddMonths(-1).AddDays(-DateTime.Now.Day + 1).Date;
            DateTime to= DateTime.Now.Date;
            try
            {
                foreach (int ID in emplIDs)
                {
                    while (from <= to)
                    {
                        List<IOPairProcessedTO> ioPairsList = new IOPairProcessed().pairsForPeriod(ID.ToString(), from, from, "-1000,88");
                        foreach (IOPairProcessedTO iopp in ioPairsList)
                        {
                            if (ioPairsList.IndexOf(iopp) == ioPairsList.Count - 1 && (new DateTime(iopp.EndTime.Year, iopp.EndTime.Month, iopp.EndTime.Day, 23, 59, 0) - iopp.EndTime).TotalMinutes <= 45 && (new DateTime(iopp.EndTime.Year, iopp.EndTime.Month, iopp.EndTime.Day, 23, 59, 0) - iopp.EndTime).TotalMinutes > 0 && iopp.EndTime.Hour == 23)
                            {
                                IOPairProcessedTO newIOpp = new IOPairProcessedTO();
                                newIOpp.EmployeeID = ID;
                                newIOpp.StartTime = iopp.EndTime;
                                newIOpp.EndTime = new DateTime(iopp.EndTime.Year, iopp.EndTime.Month, iopp.EndTime.Day, 23, 59, 0);
                                newIOpp.CreatedTime = DateTime.Now;
                                newIOpp.Alert = "0";
                                newIOpp.ConfirmationFlag = newIOpp.VerificationFlag = newIOpp.ManualCreated = 0;
                                newIOpp.PassTypeID = 88;
                                newIOpp.IsWrkHrsCounter = 1;
                                newIOpp.IOPairDate = iopp.IOPairDate;
                                newIOpp.IOPairID = 0;
                                newIOpp.LocationID = iopp.LocationID;
                                insertProcessed(null, newIOpp, 88);
                                from = from.AddDays(1);
                            }
                            if (ioPairsList.IndexOf(iopp) == ioPairsList.Count - 1 && (iopp.StartTime - new DateTime(iopp.StartTime.Year, iopp.StartTime.Month, iopp.StartTime.Day, 0, 0, 0)).TotalMinutes <= 45 && (iopp.StartTime - new DateTime(iopp.StartTime.Year, iopp.StartTime.Month, iopp.StartTime.Day, 0, 0, 0)).TotalMinutes > 0 && iopp.StartTime.Hour == 0)
                            {
                                IOPairProcessedTO newIOpp = new IOPairProcessedTO();
                                newIOpp.EmployeeID = ID;
                                newIOpp.StartTime = new DateTime(iopp.StartTime.Year, iopp.StartTime.Month, iopp.StartTime.Day, 0, 0, 0);
                                newIOpp.EndTime = iopp.StartTime;
                                newIOpp.CreatedTime = DateTime.Now;
                                newIOpp.Alert = "0";
                                newIOpp.ConfirmationFlag = newIOpp.VerificationFlag = newIOpp.ManualCreated = 0;
                                newIOpp.PassTypeID = 88;
                                newIOpp.IsWrkHrsCounter = 1;
                                newIOpp.IOPairDate = iopp.IOPairDate;
                                newIOpp.IOPairID = 0;
                                newIOpp.LocationID = iopp.LocationID;
                                insertProcessed(null, newIOpp, 88);
                                from = from.AddDays(1);
                            }
                            if (ioPairsList.IndexOf(iopp) == ioPairsList.Count - 1)
                                continue;
                            if (ioPairsList.IndexOf(iopp) == 0 && (iopp.StartTime - new DateTime(iopp.StartTime.Year, iopp.StartTime.Month, iopp.StartTime.Day, 0, 0, 0)).TotalMinutes <= 45 && (iopp.StartTime - new DateTime(iopp.StartTime.Year, iopp.StartTime.Month, iopp.StartTime.Day, 0, 0, 0)).TotalMinutes > 0)
                            {
                                IOPairProcessedTO newIOpp = new IOPairProcessedTO();
                                newIOpp.EmployeeID = ID;
                                newIOpp.StartTime = new DateTime(iopp.StartTime.Year, iopp.StartTime.Month, iopp.StartTime.Day, 0, 0, 0);
                                newIOpp.EndTime = iopp.StartTime;
                                newIOpp.CreatedTime = DateTime.Now;
                                newIOpp.Alert = "0";
                                newIOpp.ConfirmationFlag = newIOpp.VerificationFlag = newIOpp.ManualCreated = 0;
                                newIOpp.PassTypeID = 88;
                                newIOpp.IsWrkHrsCounter = 1;
                                newIOpp.IOPairDate = iopp.IOPairDate;
                                newIOpp.IOPairID = 0;
                                newIOpp.LocationID = iopp.LocationID;
                                insertProcessed(null, newIOpp, 88);
                            }
                            DateTime startTime = iopp.EndTime;
                            DateTime endTime = ioPairsList[ioPairsList.IndexOf(iopp) + 1].StartTime;
                            if ((endTime - startTime).TotalMinutes <= 45 && (endTime - startTime).TotalMinutes > 0)
                            {
                                IOPairProcessedTO newIOpp = new IOPairProcessedTO();
                                newIOpp.EmployeeID = ID;
                                newIOpp.StartTime = startTime;
                                newIOpp.EndTime = endTime;
                                newIOpp.CreatedTime = DateTime.Now;
                                newIOpp.Alert = "0";
                                newIOpp.ConfirmationFlag = newIOpp.VerificationFlag = newIOpp.ManualCreated = 0;
                                newIOpp.PassTypeID = -1000;
                                newIOpp.IsWrkHrsCounter = 1;
                                newIOpp.IOPairDate = iopp.IOPairDate;
                                newIOpp.IOPairID = 0;
                                newIOpp.LocationID = iopp.LocationID;
                                insertProcessed(null, newIOpp, -1000);
                            }
                        }
                        from = from.AddDays(1);
                    }
                    from = DateTime.Now.Date.AddMonths(-1).AddDays(-DateTime.Now.Day + 1).Date;
                }
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".FillingOvertimeHoles() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
            }
        }

        private void DeleteDuplicatesIOPairsProcessed()
        {
            //logika za brisanje duplih neopravdanih prolazaka za ceo mesec
            IOPairProcessed ioPair = new IOPairProcessed();
            bool isDeleted = ioPair.DeleteDuplicates();
        }

        private void TransferingMealsNewServer()
        {
            try
            {
                // make connection
                ReaderRemoteDAO mealDAO = ReaderRemoteDAO.getDAO();
                object MealConnection = null;

                if (mealDAO != null)
                    MealConnection = mealDAO.MakeNewDBConnection();

                if (mealDAO == null || MealConnection == null)
                {
                    debug.writeBenchmarkLog(DateTime.Now + " TransferingMeals() - Make new meal connection failed. ");
                    return; 
                }

                mealDAO.setDBConnection(MealConnection);

                try
                {
                    // transfer meals and update to transfered
                    List<OnlineMealsUsedTO> mealsToTransferList = mealDAO.GetMealsToTransfer();

                    if (mealsToTransferList.Count > 0)
                    {
                        OnlineMealsUsed meal = new OnlineMealsUsed();
                        if (meal.BeginTransaction())
                        {
                            try
                            {
                                bool transfered = true;

                                string transIDs = "";
                                foreach (OnlineMealsUsedTO mealTO in mealsToTransferList)
                                {
                                    transIDs += mealTO.TransactionID.ToString().Trim() + ",";

                                    meal.OnlineMealsUsedTO = mealTO;

                                    transfered = transfered && (meal.Save(false) > 0);

                                    if (!transfered)
                                        break;
                                }

                                if (transIDs.Length > 0)
                                    transIDs = transIDs.Substring(0, transIDs.Length - 1);

                                if (transfered)
                                    transfered = transfered && mealDAO.UpdateToTransfered(transIDs, false);

                                if (transfered)
                                    meal.CommitTransaction();
                                else
                                {
                                    if (meal.GetTransaction() != null)
                                        meal.RollbackTransaction();

                                    debug.writeBenchmarkLog(DateTime.Now + " TransferingMeals() - Transfering meals failed. ");
                                }
                            }
                            catch (Exception ex)
                            {
                                if (meal.GetTransaction() != null)
                                    meal.RollbackTransaction();

                                debug.writeBenchmarkLog(DateTime.Now + " TransferingMeals() - Transfering meals failed with exception: " + ex.Message);
                            }
                        }
                        else
                            debug.writeBenchmarkLog(DateTime.Now + " TransferingMeals() - Begin transfering transaction failed. ");
                    }

                    // delete from previous day
                    mealDAO.DeletePreviousDay(false);
                }
                catch (Exception ex)
                {                    
                    debug.writeBenchmarkLog(DateTime.Now + " TransferingMeals() - Transfering and deleting meals failed with exception: " + ex.Message);
                }

                // close conection
                mealDAO.CloseConnection(MealConnection);
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".TransferingMeals() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
            }
        }

        private void TransferingMeals()
        {
            try
            {
                OnlineMealsUsedDaily dailyMeal = new OnlineMealsUsedDaily();

                // transfer meals and update to transfered                
                List<OnlineMealsUsedTO> mealsToTransferList = dailyMeal.GetMealsToTransfer();

                List<int> emplIDsList = new List<int>();
                string emplIDs = "";
                DateTime minDate = new DateTime();
                DateTime maxDate = new DateTime();
                
                if (mealsToTransferList.Count > 0)
                {
                    foreach (OnlineMealsUsedTO mealTO in mealsToTransferList)
                    {
                        if (!emplIDsList.Contains(mealTO.EmployeeID))
                        {
                            emplIDsList.Add(mealTO.EmployeeID);
                            emplIDs += mealTO.EmployeeID.ToString().Trim() + ",";
                        }

                        if (mealTO.EventTime > maxDate)
                            maxDate = mealTO.EventTime;

                        if (mealTO.EventTime < minDate || minDate == new DateTime())
                            minDate = mealTO.EventTime;
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    List<OnlineMealsUsedTO> validationList = new List<OnlineMealsUsedTO>();

                    OnlineMealsUsed meal = new OnlineMealsUsed();

                    if (emplIDs.Trim() != "")
                        validationList = meal.Search(emplIDs, minDate, maxDate);
                    
                    if (meal.BeginTransaction())
                    {
                        try
                        {
                            bool transfered = true;

                            dailyMeal.SetTransaction(meal.GetTransaction());
                            string transIDs = "";
                            foreach (OnlineMealsUsedTO mealTO in mealsToTransferList)
                            {
                                transIDs += mealTO.TransactionID.ToString().Trim() + ",";

                                bool mealExists = false;

                                foreach (OnlineMealsUsedTO valMeal in validationList)
                                {
                                    if (mealTO.EmployeeID == valMeal.EmployeeID && mealTO.EventTime == valMeal.EventTime
                                        && mealTO.PointID == valMeal.PointID && mealTO.MealTypeID == valMeal.MealTypeID)
                                    {
                                        mealExists = true;
                                        break;
                                    }
                                }

                                if (!mealExists)
                                {
                                    meal.OnlineMealsUsedTO = mealTO;
                                    transfered = transfered && (meal.Save(false) > 0);
                                }
                                else
                                    debug.writeBenchmarkLog(DateTime.Now + " TransferingMeals() - Transfering meals failed. Meal already exists.");

                                if (!transfered)
                                    break;
                            }

                            if (transIDs.Length > 0)
                                transIDs = transIDs.Substring(0, transIDs.Length - 1);

                            if (transfered)
                                transfered = transfered && dailyMeal.UpdateToTransfered(transIDs, false);
                            
                            // delete from previous day
                            if (transfered)
                                transfered = transfered && dailyMeal.DeletePreviousDay(false);

                            if (transfered)
                                meal.CommitTransaction();
                            else
                            {
                                if (meal.GetTransaction() != null)
                                    meal.RollbackTransaction();

                                debug.writeBenchmarkLog(DateTime.Now + " TransferingMeals() - Transfering meals failed. ");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (meal.GetTransaction() != null)
                                meal.RollbackTransaction();

                            throw ex;
                        }
                    }
                    else
                        debug.writeBenchmarkLog(DateTime.Now + " TransferingMeals() - Begin transfering transaction failed. ");
                }
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".TransferingMeals() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
            }
        }

        private void AutoCloseNightOvertime(Dictionary<int, WorkTimeSchemaTO> schemas)
        {
            try
            {
                // get all opened pairs from this and previous month till today
                DateTime lastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).Date;

                // get max HRSSC cut off date
                int maxHRSSCCutOffDate = 0;
                foreach (int company in rulesDict.Keys)
                {
                    foreach (int type in rulesDict[company].Keys)
                    {
                        if (rulesDict[company][type].ContainsKey(Constants.RuleHRSSCCutOffDate) && maxHRSSCCutOffDate < rulesDict[company][type][Constants.RuleHRSSCCutOffDate].RuleValue)
                            maxHRSSCCutOffDate = rulesDict[company][type][Constants.RuleHRSSCCutOffDate].RuleValue;
                    }
                }
                
                if (maxHRSSCCutOffDate > 0 && Common.Misc.countWorkingDays(DateTime.Now.Date, null) > maxHRSSCCutOffDate)
                    lastMonth = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;

                Dictionary<int, Dictionary<DateTime, IOPairTO>> emplStartOpenPairs = new Dictionary<int, Dictionary<DateTime, IOPairTO>>();
                Dictionary<int, Dictionary<DateTime, IOPairTO>> emplEndOpenPairs = new Dictionary<int, Dictionary<DateTime, IOPairTO>>();
                string emplIDs = "";
                List<DateTime> dateList = new List<DateTime>();
                new IOPair().SearchEmplOpenPairs(lastMonth, emplStartOpenPairs, emplEndOpenPairs, ref emplIDs, dateList);
                
                if (emplIDs.Length == 0 || dateList.Count == 0)
                    return;

                // get all pass types
                Dictionary<int, PassTypeTO> ptDict = new PassType().SearchDictionary();

                // get schedules for employees
                Dictionary<int, List<EmployeeTimeScheduleTO>> employeesSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, lastMonth, DateTime.Now.Date, null);

                // get employees
                Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionary(emplIDs);

                // get asco data for employees
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);
                                
                // get all already processed pairs for employees and dates
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDatePairs = new Dictionary<int,Dictionary<DateTime,List<IOPairProcessedTO>>>();
                
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, "");

                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDatePairs.ContainsKey(pair.EmployeeID))
                        emplDatePairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                    if (!emplDatePairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDatePairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplDatePairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                IOPairProcessed processed = new IOPairProcessed();
                
                // close pair start and insert rounded processed pair if there is no processed pair overlaping and if it is overtime pair, add proccessed pair to dictionary
                foreach (int emplID in emplStartOpenPairs.Keys)
                {
                    if (emplID == 904)
                    {
                    }
                    // get employee rules
                    Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                    if (ascoDict.ContainsKey(emplID) && emplDict.ContainsKey(emplID) && rulesDict.ContainsKey(ascoDict[emplID].IntegerValue4)
                        && rulesDict[ascoDict[emplID].IntegerValue4].ContainsKey(emplDict[emplID].EmployeeTypeID))
                        emplRules = rulesDict[ascoDict[emplID].IntegerValue4][emplDict[emplID].EmployeeTypeID];

                    // if there is no overnight type, continue with processing
                    if (!emplRules.ContainsKey(Constants.RuleCompanyInitialNightOvertime))
                        continue;

                    // get employee schedules
                    List<EmployeeTimeScheduleTO> emplSchedules = new List<EmployeeTimeScheduleTO>();
                    if (employeesSchedules.ContainsKey(emplID))
                        emplSchedules = employeesSchedules[emplID];

                    foreach (DateTime date in emplStartOpenPairs[emplID].Keys)
                    {                        
                        try
                        {
                            // get day begining and day end
                            DateTime dayBegining = date.Date;
                            DateTime dayEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);

                            // get intervals for day
                            List<WorkTimeIntervalTO> dayIntervals = Common.Misc.getTimeSchemaInterval(date, emplSchedules, schemas);

                            // get time schema for day
                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (dayIntervals.Count > 0 && schemas.ContainsKey(dayIntervals[0].TimeSchemaID))
                                sch = schemas[dayIntervals[0].TimeSchemaID];

                            // is it working day
                            bool IsWorkAbsenceDay = false;
                            if (dayIntervals.Count == 0 ||
                                (dayIntervals.Count == 1 && dayIntervals[0].StartTime.Hour == 0 && dayIntervals[0].StartTime.Minute == 0))
                                IsWorkAbsenceDay = true;

                            // get day pairs
                            List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                            if (emplDatePairs.ContainsKey(emplID) && emplDatePairs[emplID].ContainsKey(date))
                                dayPairs = emplDatePairs[emplID][date];

                            // create overtime pair
                            IOPairTO pair = emplStartOpenPairs[emplID][date];
                            pair.StartTime = pair.IOPairDate.Date;
                            IOPairProcessedTO processedTO = createNightOvertimePair(pair, emplRules[Constants.RuleCompanyInitialNightOvertime].RuleValue, ptDict);
                            
                            // change end time if it is different than rounding rule for overtime (WS or out of WS)
                            int minPresenceRounding = 1;
                            if (emplRules.ContainsKey(Constants.RulePresenceRounding))
                            {
                                minPresenceRounding = emplRules[Constants.RulePresenceRounding].RuleValue;

                                if (processedTO.EndTime.TimeOfDay != new TimeSpan(23, 59, 0) && processedTO.EndTime.Minute % minPresenceRounding != 0)
                                {
                                    processedTO.EndTime = processedTO.EndTime.AddMinutes(-(processedTO.EndTime.Minute % minPresenceRounding));

                                    if (processedTO.EndTime < dayBegining)
                                        processedTO.EndTime = dayBegining;
                                }
                            }

                            if (!IsWorkAbsenceDay)
                            {
                                if (emplRules.ContainsKey(Constants.RuleOvertimeRounding))
                                    minPresenceRounding = emplRules[Constants.RuleOvertimeRounding].RuleValue;
                            }
                            else
                            {
                                if (emplRules.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                    minPresenceRounding = emplRules[Constants.RuleOvertimeRoundingOutWS].RuleValue;
                            }

                            int pairDuration = (int)processedTO.EndTime.Subtract(processedTO.StartTime).TotalMinutes;

                            if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                pairDuration++;

                            if (pairDuration % minPresenceRounding != 0)
                            {
                                processedTO.EndTime = processedTO.EndTime.AddMinutes(-(processedTO.EndTime.Subtract(processedTO.StartTime).TotalMinutes % minPresenceRounding));

                                if (processedTO.EndTime < dayBegining)
                                    processedTO.EndTime = dayBegining;
                            }

                            if (!validateOvertimePair(processedTO, emplRules, dayPairs, sch, dayIntervals, ptDict, new DateTime(), new DateTime(), IsWorkAbsenceDay)
                                || overlapInterval(processedTO, dayIntervals) || overlapPair(processedTO, dayPairs))                            
                                continue;                            

                            processed.IOPairProcessedTO = processedTO;
                            if (processed.Save(true) > 0)
                            {
                                if (!emplDatePairs.ContainsKey(emplID))
                                    emplDatePairs.Add(emplID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                                if (!emplDatePairs[emplID].ContainsKey(date))
                                    emplDatePairs[emplID].Add(date, new List<IOPairProcessedTO>());

                                emplDatePairs[emplID][date].Add(processedTO);
                            }
                        }
                        catch (Exception ex)
                        {
                            debug.writeBenchmarkLog(DateTime.Now + " Exception in: " + this.ToString() + ".AutoCloseNightOvertime() - Message: " + ex.Message
                                + "; employee: " + emplID.ToString() + "; date: " + date.ToString(Constants.dateFormat));
                        }
                    }
                }

                // close pair end and insert rounded processed pair if there is no processed pair overlaping and if it is overtime pair, add proccessed pair to dictionary                
                foreach (int emplID in emplEndOpenPairs.Keys)
                {
                    if (emplID == 904)
                    {
                    }
                    // get employee rules
                    Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                    if (ascoDict.ContainsKey(emplID) && emplDict.ContainsKey(emplID) && rulesDict.ContainsKey(ascoDict[emplID].IntegerValue4)
                        && rulesDict[ascoDict[emplID].IntegerValue4].ContainsKey(emplDict[emplID].EmployeeTypeID))
                        emplRules = rulesDict[ascoDict[emplID].IntegerValue4][emplDict[emplID].EmployeeTypeID];

                    // if there is no overnight type, continue with processing
                    if (!emplRules.ContainsKey(Constants.RuleCompanyInitialNightOvertime))
                        continue;

                    // get employee schedules
                    List<EmployeeTimeScheduleTO> emplSchedules = new List<EmployeeTimeScheduleTO>();
                    if (employeesSchedules.ContainsKey(emplID))
                        emplSchedules = employeesSchedules[emplID];

                    foreach (DateTime date in emplEndOpenPairs[emplID].Keys)
                    {
                        // if something fails, go to next day
                        try
                        {
                            // get day begining and day end
                            DateTime dayBegining = date.Date;
                            DateTime dayEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);

                            // get intervals for day
                            List<WorkTimeIntervalTO> dayIntervals = Common.Misc.getTimeSchemaInterval(date, emplSchedules, schemas);

                            // get time schema for day
                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (dayIntervals.Count > 0 && schemas.ContainsKey(dayIntervals[0].TimeSchemaID))
                                sch = schemas[dayIntervals[0].TimeSchemaID];

                            // is it working day
                            bool IsWorkAbsenceDay = false;
                            if (dayIntervals.Count == 0 ||
                                (dayIntervals.Count == 1 && dayIntervals[0].StartTime.Hour == 0 && dayIntervals[0].StartTime.Minute == 0))
                                IsWorkAbsenceDay = true;

                            // get day pairs
                            List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                            if (emplDatePairs.ContainsKey(emplID) && emplDatePairs[emplID].ContainsKey(date))
                                dayPairs = emplDatePairs[emplID][date];

                            // create overtime pair
                            IOPairTO pair = emplEndOpenPairs[emplID][date];
                            pair.EndTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, 23, 59, 0);
                            IOPairProcessedTO processedTO = createNightOvertimePair(pair, emplRules[Constants.RuleCompanyInitialNightOvertime].RuleValue, ptDict);
                            
                            // change start time if it is different than rounding rule for overtime (WS or out of WS)
                            int minPresenceRounding = 1;
                            if (emplRules.ContainsKey(Constants.RulePresenceRounding))
                            {
                                minPresenceRounding = emplRules[Constants.RulePresenceRounding].RuleValue;

                                if (processedTO.StartTime.Minute % minPresenceRounding != 0)
                                {
                                    processedTO.StartTime = processedTO.StartTime.AddMinutes(minPresenceRounding - (processedTO.StartTime.Minute % minPresenceRounding));

                                    if (processedTO.StartTime > dayEnd)
                                        processedTO.StartTime = dayEnd;
                                }
                            }

                            if (!IsWorkAbsenceDay)
                            {
                                if (emplRules.ContainsKey(Constants.RuleOvertimeRounding))
                                    minPresenceRounding = emplRules[Constants.RuleOvertimeRounding].RuleValue;
                            }
                            else
                            {
                                if (emplRules.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                    minPresenceRounding = emplRules[Constants.RuleOvertimeRoundingOutWS].RuleValue;
                            }

                            int pairDuration = (int)processedTO.EndTime.Subtract(processedTO.StartTime).TotalMinutes;
                            if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                pairDuration++;

                            if (pairDuration % minPresenceRounding != 0)
                            {
                                processedTO.StartTime = processedTO.StartTime.AddMinutes(pairDuration % minPresenceRounding);

                                if (processedTO.StartTime > dayEnd)
                                    processedTO.StartTime = dayEnd;
                            }

                            if (!validateOvertimePair(processedTO, emplRules, dayPairs, sch, dayIntervals, ptDict, new DateTime(), new DateTime(), IsWorkAbsenceDay)
                                || overlapInterval(processedTO, dayIntervals) || overlapPair(processedTO, dayPairs))
                                continue;                            

                            processed.IOPairProcessedTO = processedTO;
                            if (processed.Save(true) > 0)
                            {
                                if (!emplDatePairs.ContainsKey(emplID))
                                    emplDatePairs.Add(emplID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                                if (!emplDatePairs[emplID].ContainsKey(date))
                                    emplDatePairs[emplID].Add(date, new List<IOPairProcessedTO>());

                                emplDatePairs[emplID][date].Add(processedTO);
                            }
                        }
                        catch (Exception ex)
                        {
                            debug.writeBenchmarkLog(DateTime.Now + " Exception in: " + this.ToString() + ".AutoCloseNightOvertime() - Message: " + ex.Message
                                + "; employee: " + emplID.ToString() + "; date: " + date.ToString(Constants.dateFormat));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".AutoCloseNightOvertime() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
            }
        }

        private void AbsenceToWork(Dictionary<int, WorkTimeSchemaTO> schemas)
        {
            try
            {
                // change absence to regular work only if customer is PMC
                //string costumer = Common.Misc.getCustomer(null);
                //int cost = 0;
                //if (int.TryParse(costumer, out cost) && (cost != (int)Constants.Customers.PMC))                    
                //    return;

                // get all extra ordinary employees
                string emplIDs = "";

                if (extraOrdinaryEmpl.Count <= 0)
                    return;

                foreach (int id in extraOrdinaryEmpl.Keys)
                {
                    emplIDs += id.ToString().Trim() + ",";
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                // get all opened pairs from this and previous month till today
                DateTime lastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).Date;

                // get max HRSSC cut off date
                int maxHRSSCCutOffDate = 0;
                foreach (int company in rulesDict.Keys)
                {
                    foreach (int type in rulesDict[company].Keys)
                    {
                        if (rulesDict[company][type].ContainsKey(Constants.RuleHRSSCCutOffDate) && maxHRSSCCutOffDate < rulesDict[company][type][Constants.RuleHRSSCCutOffDate].RuleValue)
                            maxHRSSCCutOffDate = rulesDict[company][type][Constants.RuleHRSSCCutOffDate].RuleValue;
                    }
                }

                if (maxHRSSCCutOffDate > 0 && Common.Misc.countWorkingDays(DateTime.Now.Date, null) > maxHRSSCCutOffDate)
                    lastMonth = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;

                // get all absence pairs
                List<DateTime> dateList = new List<DateTime>();
                for (DateTime currDate = lastMonth.Date; currDate.Date <= DateTime.Now.Date; currDate = currDate.AddDays(1).Date)
                {
                    dateList.Add(currDate.Date);
                }

                List<IOPairProcessedTO> absencePairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, Constants.absence.ToString().Trim());

                // get all dates with absence pairs
                dateList = new List<DateTime>();
                foreach (IOPairProcessedTO pair in absencePairs)
                {
                    if (!dateList.Contains(pair.IOPairDate.Date))
                        dateList.Add(pair.IOPairDate.Date);
                }

                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, "");

                // get pairs by days
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                // get all passes
                Dictionary<int, Dictionary<DateTime, List<PassTO>>> emplPasses = new Pass().SearchPassesForEmployeesPeriod(emplIDs, lastMonth.Date, DateTime.Now.Date);

                // get all pass types
                Dictionary<int, PassTypeTO> ptDict = new PassType().SearchDictionary();

                // get schedules for employees
                Dictionary<int, List<EmployeeTimeScheduleTO>> employeesSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, lastMonth, DateTime.Now.Date, null);

                // get employees
                Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionary(emplIDs);

                // get asco data for employees
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);

                IOPairProcessed processed = new IOPairProcessed();

                // go through absence pairs, save to history and change to regular work if pair is valid
                foreach (IOPairProcessedTO pair in absencePairs)
                {
                    // get employee rules
                    Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                    if (ascoDict.ContainsKey(pair.EmployeeID) && emplDict.ContainsKey(pair.EmployeeID) && rulesDict.ContainsKey(ascoDict[pair.EmployeeID].IntegerValue4)
                        && rulesDict[ascoDict[pair.EmployeeID].IntegerValue4].ContainsKey(emplDict[pair.EmployeeID].EmployeeTypeID))
                        emplRules = rulesDict[ascoDict[pair.EmployeeID].IntegerValue4][emplDict[pair.EmployeeID].EmployeeTypeID];

                    // if there is no regular work type, continue with processing
                    if (!emplRules.ContainsKey(Constants.RuleCompanyRegularWork))
                        continue;

                    // if there is no passes for employee, continue with processing
                    if (!emplPasses.ContainsKey(pair.EmployeeID))
                        continue;

                    // get employee schedules
                    List<EmployeeTimeScheduleTO> emplSchedules = new List<EmployeeTimeScheduleTO>();
                    if (employeesSchedules.ContainsKey(pair.EmployeeID))
                        emplSchedules = employeesSchedules[pair.EmployeeID];

                    // get pairs for day
                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        dayPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date];

                    // get intervals for day
                    List<WorkTimeIntervalTO> dayIntervals = Common.Misc.getTimeSchemaInterval(pair.IOPairDate.Date, emplSchedules, schemas);
                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();

                    if (dayIntervals.Count > 0 && schemas.ContainsKey(dayIntervals[0].TimeSchemaID))
                        sch = schemas[dayIntervals[0].TimeSchemaID];
                                        
                    WorkTimeIntervalTO pairInterval = Common.Misc.getPairInterval(pair, dayPairs, sch, dayIntervals, ptDict);

                    // skip pairs from first day that belongs to already closed period
                    if (pair.IOPairDate.Date.Equals(lastMonth.Date) && pairInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        continue;

                    bool passFound = false;
                    if (emplPasses[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                    {
                        foreach (PassTO pass in emplPasses[pair.EmployeeID][pair.IOPairDate.Date])
                        {
                            if (pairInterval.EarliestArrived.TimeOfDay <= pass.EventTime.TimeOfDay && pass.EventTime.TimeOfDay <= pairInterval.LatestLeft.TimeOfDay)
                            {
                                passFound = true;
                                break;
                            }
                        }
                    }

                    if (!passFound)
                    {
                        // if interval is from third shift beggining, check passes from next day
                        if (pairInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0) && emplPasses[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date.AddDays(1)))
                        {
                            // get intervals for next day
                            List<WorkTimeIntervalTO> nextDayIntervals = Common.Misc.getTimeSchemaInterval(pair.IOPairDate.Date.AddDays(1), emplSchedules, schemas);

                            foreach (WorkTimeIntervalTO nextInterval in nextDayIntervals)
                            {
                                if (nextInterval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                    continue;

                                foreach (PassTO pass in emplPasses[pair.EmployeeID][pair.IOPairDate.Date.AddDays(1)])
                                {
                                    if (nextInterval.EarliestArrived.TimeOfDay <= pass.EventTime.TimeOfDay && pass.EventTime.TimeOfDay <= nextInterval.LatestLeft.TimeOfDay)
                                    {
                                        passFound = true;
                                        break;
                                    }
                                }

                                if (passFound)
                                    break;
                            }
                        }

                        // if interval is from third shift end, check passes from previous day
                        if (pairInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && emplPasses[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date.AddDays(-1)))
                        {
                            // get intervals for previous day
                            List<WorkTimeIntervalTO> prevDayIntervals = Common.Misc.getTimeSchemaInterval(pair.IOPairDate.Date.AddDays(-1), emplSchedules, schemas);

                            foreach (WorkTimeIntervalTO prevInterval in prevDayIntervals)
                            {
                                if (prevInterval.EndTime.TimeOfDay != new TimeSpan(23, 59, 0))
                                    continue;

                                foreach (PassTO pass in emplPasses[pair.EmployeeID][pair.IOPairDate.Date.AddDays(-1)])
                                {
                                    if (prevInterval.EarliestArrived.TimeOfDay <= pass.EventTime.TimeOfDay && pass.EventTime.TimeOfDay <= prevInterval.LatestLeft.TimeOfDay)
                                    {
                                        passFound = true;
                                        break;
                                    }
                                }

                                if (passFound)
                                    break;
                            }
                        }
                    }

                    if (passFound)
                    {
                        DateTime modTime = DateTime.Now;
                        //IOPairsProcessedHist hist = new IOPairsProcessedHist();
                        //hist.IOPairProcessedHistTO = new IOPairsProcessedHistTO(pair);
                        //hist.IOPairProcessedHistTO.Alert = Constants.alertStatus.ToString().Trim();
                        //hist.IOPairProcessedHistTO.ModifiedBy = Constants.dataProcessingUser;
                        //hist.IOPairProcessedHistTO.ModifiedTime = modTime;

                        processed.IOPairProcessedTO = new IOPairProcessedTO(pair);
                        processed.IOPairProcessedTO.PassTypeID = emplRules[Constants.RuleCompanyRegularWork].RuleValue;
                        if (ptDict.ContainsKey(processed.IOPairProcessedTO.PassTypeID))
                        {
                            processed.IOPairProcessedTO.ConfirmationFlag = ptDict[processed.IOPairProcessedTO.PassTypeID].ConfirmFlag;
                            processed.IOPairProcessedTO.VerificationFlag = ptDict[processed.IOPairProcessedTO.PassTypeID].VerificationFlag;
                        }
                        else
                        {
                            processed.IOPairProcessedTO.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                            processed.IOPairProcessedTO.VerificationFlag = (int)Constants.Verification.Verified;
                        }
                        processed.IOPairProcessedTO.CreatedBy = Constants.dataProcessingUser;
                        processed.IOPairProcessedTO.CreatedTime = modTime;

                        if (emplRules.ContainsKey(Constants.RuleMinPresence))
                        {
                            // validate duration due to minimal presence
                            int pairDuration = (int)(processed.IOPairProcessedTO.EndTime.Subtract(processed.IOPairProcessedTO.StartTime).TotalMinutes);

                            // if it is last pair from first night shift interval, add one minute
                            if (processed.IOPairProcessedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                pairDuration++;

                            if (pairDuration < emplRules[Constants.RuleMinPresence].RuleValue)
                                continue;
                        }

                        if (processed.BeginTransaction())
                        {
                            try
                            {
                                bool saved = true;
                                //hist.SetTransaction(processed.GetTransaction());
                                //saved = saved && hist.Save(false) > 0;

                                if (saved && processed.Delete(processed.IOPairProcessedTO.RecID.ToString().Trim(), false))
                                    saved = saved && processed.Save(false) > 0;

                                if (saved)
                                {
                                    processed.CommitTransaction();                                    
                                }
                                else
                                {
                                    if (processed.GetTransaction() != null)
                                        processed.RollbackTransaction();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (processed.GetTransaction() != null)
                                    processed.RollbackTransaction();

                                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " + this.ToString() + ".AbsenceToWork() - Message: " + ex.Message
                                    + "; employee: " + processed.IOPairProcessedTO.EmployeeID.ToString() + "; date: " + processed.IOPairProcessedTO.IOPairDate.Date.ToString(Constants.dateFormat));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".AbsenceToWork() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
            }
        }

        private IOPairProcessedTO createNightOvertimePair(IOPairTO pair, int ptID, Dictionary<int, PassTypeTO> ptDict)
        {
            try
            {
                IOPairProcessedTO overtimePair = new IOPairProcessedTO(pair);

                // remove seconds from start and end time
                overtimePair.StartTime = overtimePair.StartTime.AddSeconds(-overtimePair.StartTime.Second);
                overtimePair.EndTime = overtimePair.EndTime.AddSeconds(-overtimePair.EndTime.Second);
                
                overtimePair.Alert = Constants.alertStatusNoAlert.ToString();
                overtimePair.ManualCreated = (int)Constants.recordCreated.Automaticaly;
                overtimePair.CreatedBy = Constants.dataProcessingUser;
                overtimePair.PassTypeID = ptID;
                overtimePair.IOPairID = pair.IOPairID;
                if (ptDict.ContainsKey(ptID))
                {
                    overtimePair.ConfirmationFlag = ptDict[ptID].ConfirmFlag;
                    overtimePair.VerificationFlag = ptDict[ptID].VerificationFlag;                    
                }
                else
                {
                    overtimePair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                    overtimePair.VerificationFlag = (int)Constants.Verification.Verified;
                }
                
                return overtimePair;
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " + this.ToString() + ".createNightOvertimePair() - Message: " + ex.Message);
                throw ex;
            }
        }

        private MedicalCheckVisitDtlTO createVisitDtl(int id, string type)
        {
            try
            {
                MedicalCheckVisitDtlTO dtlTO = new MedicalCheckVisitDtlTO();

                dtlTO.CheckID = id;
                dtlTO.Type = type;                    
                dtlTO.CreatedBy = Constants.dataProcessingUser;

                return dtlTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MedicalCheckVisitHdrTO createVisitHdr(int emplID)
        {
            try
            {
                MedicalCheckVisitHdrTO hdrTO = new MedicalCheckVisitHdrTO();

                hdrTO.EmployeeID = emplID;
                hdrTO.FlagEmail = Constants.noInt;
                hdrTO.FlagEmailCratedTime = new DateTime();
                hdrTO.FlagChange = Constants.noInt;
                hdrTO.PointID = Constants.defaultMedicalCheckPointId;
                hdrTO.ScheduleDate = Constants.dateTimeNullValue();
                hdrTO.Status = Constants.MedicalCheckVisitStatus.WR.ToString().Trim();                
                hdrTO.CreatedBy = Constants.dataProcessingUser;

                return hdrTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessingVisits()
        {
            try
            {             
                // get active employees for whom border date for next month scheduling has been reached
                string activeEmployees = new Employee().SearchActiveIDsForScheduling();

                if (activeEmployees.Length <= 0)
                    return;

                // get asco data
                //Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(activeEmployees);

                DateTime firstDayMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
                DateTime firstDayNextMonth = firstDayMonth.Date.AddMonths(1);
                DateTime monthAfterNext = firstDayNextMonth.Date.AddMonths(1);

                Dictionary<int, MedicalCheckVisitHdrTO> emplVisits = new Dictionary<int, MedicalCheckVisitHdrTO>();
                Dictionary<int, List<EmployeeXVaccineTO>> vaccinesProcessed = new Dictionary<int, List<EmployeeXVaccineTO>>();
                Dictionary<int, List<EmployeeXRiskTO>> risksProcessed = new Dictionary<int, List<EmployeeXRiskTO>>();

                // all visits and vaccines that should happen in next month will be inserted in visits
                // all risks and vaccines for one employee are details of one visit header

                // get all vaccines that has rotation and are not processed
                List<EmployeeXVaccineTO> emplVaccines = new EmployeeXVaccine().SearchEmployeeXVaccinesNotProcessed(activeEmployees);

                // get all risks that has rotation and are not processed
                List<EmployeeXRiskTO> emplRisks = new EmployeeXRisk().SearchEmployeeXRisksNotScheduled(activeEmployees, firstDayNextMonth.Date, monthAfterNext.AddDays(-1).Date);

                // get all scheduled visits
                Dictionary<int, Dictionary<string, List<int>>> scheduledRisks = new MedicalCheckVisitDtl().SearchScheduledVisits(activeEmployees, "");

                foreach (EmployeeXVaccineTO vacTO in emplVaccines)
                {
                    DateTime nextVaccine = vacTO.DatePerformed.Date.AddMonths(vacTO.Rotation);

                    if (nextVaccine.Date >= firstDayNextMonth.Date && nextVaccine.Date < monthAfterNext.Date)
                    {
                        // if vaccine for employee already has scheduled and not performed visit, update vaccine to precessed and continue with processing
                        if (scheduledRisks.ContainsKey(vacTO.EmployeeID) && scheduledRisks[vacTO.EmployeeID].ContainsKey(Constants.VisitType.V.ToString())
                            && scheduledRisks[vacTO.EmployeeID][Constants.VisitType.V.ToString()].Contains(vacTO.VaccineID))
                        {
                            try
                            {
                                vacTO.RotationFlagUsed = Constants.yesInt;
                                vacTO.ModifiedBy = Constants.dataProcessingUser;
                                vacTO.ModifiedTime = new DateTime();
                                EmployeeXVaccine emplVac = new EmployeeXVaccine();
                                emplVac.EmplXVaccineTO = vacTO;
                                emplVac.Update(true);
                            }
                            catch (Exception ex)
                            {
                                debug.writeLog(DateTime.Now + "Exception in updating employee vaccine for employee: "
                                    + vacTO.EmployeeID.ToString() + " and vaccine: " + vacTO.VaccineID.ToString() + ". Exception: " + ex.Message);
                            }

                            continue;
                        }

                        // create visit and update vaccine to used                        
                        if (!emplVisits.ContainsKey(vacTO.EmployeeID))
                            emplVisits.Add(vacTO.EmployeeID, createVisitHdr(vacTO.EmployeeID));

                        emplVisits[vacTO.EmployeeID].VisitDetails.Add(createVisitDtl(vacTO.VaccineID, Constants.VisitType.V.ToString()));


                        if (!vaccinesProcessed.ContainsKey(vacTO.EmployeeID))
                            vaccinesProcessed.Add(vacTO.EmployeeID, new List<EmployeeXVaccineTO>());

                        vaccinesProcessed[vacTO.EmployeeID].Add(vacTO);
                    }
                }

                foreach (EmployeeXRiskTO riskTO in emplRisks)
                {
                    DateTime nextVisit = riskTO.LastDatePerformed.Date.AddMonths(riskTO.Rotation);

                    while (nextVisit.Date < firstDayNextMonth.Date)
                    {
                        nextVisit = nextVisit.Date.AddMonths(riskTO.Rotation);
                    }

                    if (nextVisit.Date >= firstDayNextMonth.Date && nextVisit.Date < monthAfterNext.Date)
                    {
                        // if risk for employee already has scheduled and not performed visit, update risk to precessed and continue with processing
                        if (scheduledRisks.ContainsKey(riskTO.EmployeeID) && scheduledRisks[riskTO.EmployeeID].ContainsKey(Constants.VisitType.R.ToString())
                            && scheduledRisks[riskTO.EmployeeID][Constants.VisitType.R.ToString()].Contains(riskTO.RiskID))
                        {
                            try
                            {
                                riskTO.LastScheduleDate = DateTime.Now;
                                riskTO.ModifiedBy = Constants.dataProcessingUser;
                                riskTO.ModifiedTime = new DateTime();
                                EmployeeXRisk emplRisk = new EmployeeXRisk();
                                emplRisk.EmplXRiskTO = riskTO;
                                emplRisk.Update(true);
                            }
                            catch (Exception ex)
                            {
                                debug.writeLog(DateTime.Now + "Exception in updating employee risk for employee: "
                                    + riskTO.EmployeeID.ToString() + " and risk: " + riskTO.RiskID.ToString() + ". Exception: " + ex.Message);
                            }

                            continue;
                        }

                        // create visit and update risk to scheduled                        
                        if (!emplVisits.ContainsKey(riskTO.EmployeeID))
                            emplVisits.Add(riskTO.EmployeeID, createVisitHdr(riskTO.EmployeeID));

                        emplVisits[riskTO.EmployeeID].VisitDetails.Add(createVisitDtl(riskTO.RiskID, Constants.VisitType.R.ToString()));


                        if (!risksProcessed.ContainsKey(riskTO.EmployeeID))
                            risksProcessed.Add(riskTO.EmployeeID, new List<EmployeeXRiskTO>());

                        risksProcessed[riskTO.EmployeeID].Add(riskTO);
                    }
                }

                //// get max rotation from employee risk
                //EmployeeXRisk risk = new EmployeeXRisk();
                //int maxRotation = risk.SearchMaxRotation();

                //// get all active employee risks for next month
                //Dictionary<uint, EmployeeXRiskTO> nextMonthRisks = risk.SearchEmployeeXRisks(activeEmployees, "", firstDayNextMonth.Date, monthAfterNext.Date);

                //// get all performed risk visits due to max rotation
                //List<MedicalCheckVisitDtlTO> visitsList = new MedicalCheckVisitDtl().SearchPerformedVisits(activeEmployees, Constants.VisitType.R.ToString(), firstDayNextMonth.Date.AddMonths(-maxRotation), new DateTime());

                //// last performed date foreach employee risk
                //Dictionary<int, Dictionary<int, DateTime>> emplRiskDate = new Dictionary<int, Dictionary<int, DateTime>>();

                //foreach (MedicalCheckVisitDtlTO dtlTO in visitsList)
                //{
                //    if (!emplRiskDate.ContainsKey(dtlTO.EmployeeID))
                //        emplRiskDate.Add(dtlTO.EmployeeID, new Dictionary<int, DateTime>());

                //    if (!emplRiskDate[dtlTO.EmployeeID].ContainsKey(dtlTO.CheckID))
                //        emplRiskDate[dtlTO.EmployeeID].Add(dtlTO.CheckID, dtlTO.DatePerformed.Date);
                //    else
                //        if (emplRiskDate[dtlTO.EmployeeID][dtlTO.CheckID].Date < dtlTO.DatePerformed.Date)
                //            emplRiskDate[dtlTO.EmployeeID][dtlTO.CheckID] = dtlTO.DatePerformed.Date;
                //}

                //foreach (uint recID in nextMonthRisks.Keys)
                //{
                //    EmployeeXRiskTO riskTO = nextMonthRisks[recID];

                //    // if risk for employee already has scheduled and not performed visit, continue with processing
                //    if (scheduledRisks.ContainsKey(riskTO.EmployeeID) && scheduledRisks[riskTO.EmployeeID].ContainsKey(Constants.VisitType.R.ToString())
                //        && scheduledRisks[riskTO.EmployeeID][Constants.VisitType.R.ToString()].Contains(riskTO.RiskID))
                //        continue;

                //    DateTime lastVisitDate = new DateTime();

                //    // get last visit for risk, or if there is no such, take hiring date as start calculation date
                //    if (emplRiskDate.ContainsKey(riskTO.EmployeeID) && emplRiskDate[riskTO.EmployeeID].ContainsKey(riskTO.RiskID))
                //        lastVisitDate = emplRiskDate[riskTO.EmployeeID][riskTO.RiskID].Date;
                //    else if (ascoDict.ContainsKey(riskTO.EmployeeID))
                //        lastVisitDate = ascoDict[riskTO.EmployeeID].DatetimeValue2.Date;

                //    DateTime nextVisit = lastVisitDate.Date.AddMonths(riskTO.Rotation);

                //    if (nextVisit.Date >= firstDayNextMonth.Date && nextVisit.Date < monthAfterNext.Date)
                //    {
                //        // create visit
                //        if (!emplVisits.ContainsKey(riskTO.EmployeeID))
                //            emplVisits.Add(riskTO.EmployeeID, createVisitHdr(riskTO.EmployeeID));

                //        emplVisits[riskTO.EmployeeID].VisitDetails.Add(createVisitDtl(riskTO.RiskID, Constants.VisitType.R.ToString()));
                //    }
                //}

                foreach (int emplID in emplVisits.Keys)
                {
                    // save one employee in transaction
                    MedicalCheckVisitHdr hdr = new MedicalCheckVisitHdr();
                    EmployeeXVaccine vac = new EmployeeXVaccine();
                    EmployeeXRisk risk = new EmployeeXRisk();
                    if (hdr.BeginTransaction())
                    {
                        try
                        {
                            bool saved = true;

                            // save visit
                            hdr.VisitHdrTO = emplVisits[emplID];
                            saved = saved && hdr.Save(false);

                            if (saved)
                            {
                                // set vaccines to processed
                                if (vaccinesProcessed.ContainsKey(emplID))
                                {
                                    vac.SetTransaction(hdr.GetTransaction());
                                    foreach (EmployeeXVaccineTO vacTO in vaccinesProcessed[emplID])
                                    {
                                        vacTO.RotationFlagUsed = Constants.yesInt;
                                        vacTO.ModifiedBy = Constants.dataProcessingUser;
                                        vacTO.ModifiedTime = new DateTime();
                                        vac.EmplXVaccineTO = vacTO;

                                        saved = saved && vac.Update(false);

                                        if (!saved)
                                            break;
                                    }
                                }
                            }

                            if (saved)
                            {
                                // set risks to scheduled
                                if (risksProcessed.ContainsKey(emplID))
                                {
                                    risk.SetTransaction(hdr.GetTransaction());
                                    foreach (EmployeeXRiskTO riskTO in risksProcessed[emplID])
                                    {
                                        riskTO.LastScheduleDate = DateTime.Now;
                                        riskTO.ModifiedBy = Constants.dataProcessingUser;
                                        riskTO.ModifiedTime = new DateTime();
                                        risk.EmplXRiskTO = riskTO;

                                        saved = saved && risk.Update(false);

                                        if (!saved)
                                            break;
                                    }
                                }
                            }

                            if (saved)
                                hdr.CommitTransaction();
                            else if (hdr.GetTransaction() != null)
                                hdr.RollbackTransaction();
                        }
                        catch (Exception ex)
                        {
                            if (hdr.GetTransaction() != null)
                                hdr.RollbackTransaction();

                            debug.writeLog(DateTime.Now + "Exception in saving employee visit for employee: " + emplID.ToString() + ". Exception: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeBenchmarkLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".ProcessingVisits() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
            }
        }

        private string manualPairCounters(IOPairProcessedTO pairToConfirm, Dictionary<int, int> emplCounters, Dictionary<int, EmployeeCounterValueTO> emplOldCounters, DateTime histCreated)
        {
            try
            {
                IOPairProcessed pair = new IOPairProcessed();
                bool saved = true;
                string error = "";

                if (pair.BeginTransaction())
                {
                    try
                    {
                        IOPairsProcessedHist hist = new IOPairsProcessedHist();
                        EmployeeCounterValue counter = new EmployeeCounterValue();
                        EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();

                        pair.IOPairProcessedTO = pairToConfirm;
                        IOPairsProcessedHistTO histTO = new IOPairsProcessedHistTO(pairToConfirm);
                        hist.IOPairProcessedHistTO = histTO;
                        hist.IOPairProcessedHistTO.Alert = Constants.alertStatus.ToString();
                        hist.IOPairProcessedHistTO.ModifiedTime = histCreated;
                        hist.SetTransaction(pair.GetTransaction());
                        hist.Save(false);
                        saved = saved && (pair.Delete(pairToConfirm.RecID.ToString(), false));

                        if (saved)
                        {
                            // update counters, updated counters insert to hist table
                            counterHist.SetTransaction(pair.GetTransaction());
                            counter.SetTransaction(pair.GetTransaction());
                            // update counters and move old counter values to hist table if updated
                            foreach (int type in emplCounters.Keys)
                            {
                                if (emplOldCounters.ContainsKey(type) && emplOldCounters[type].Value != emplCounters[type])
                                {
                                    // move to hist table
                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplOldCounters[type]);
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = type;
                                    counter.ValueTO.EmplID = pairToConfirm.EmployeeID;
                                    counter.ValueTO.Value = emplCounters[type];

                                    saved = saved && counter.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }
                        }

                        if (saved)
                        {
                            pair.CommitTransaction();
                        }
                        else
                        {
                            if (pair.GetTransaction() != null)
                                pair.RollbackTransaction();
                            debug.writeLog(DateTime.Now +
                    this.ToString() + ".manualPairCounters() : Processing manual pairs counters faild for Employee_ID = " + pairToConfirm.EmployeeID + "; Date = " + pairToConfirm.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + pairToConfirm.PassTypeID.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        if (pair.GetTransaction() != null)
                            pair.RollbackTransaction();
                        debug.writeLog(DateTime.Now + " Exception in: " +
                     this.ToString() + ".manualPairCounters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                    }
                }
                else
                    debug.writeLog(DateTime.Now +
                    this.ToString() + ".manualPairCounters() : Processing manual pairs counters faild for Employee_ID = " + pairToConfirm.EmployeeID + "; Date = " + pairToConfirm.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + pairToConfirm.PassTypeID.ToString() + "; Begin transaction faild!");

                return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessingIOPairs(Dictionary<int, WorkTimeSchemaTO> timeSchema)
        {            
            try
            {
                insertedPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                 //period for inserting holes
                DateTime startIntervalTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                DateTime endIntervalTime = DateTime.Now;

                //get all unprocessed ioPairs
                IOPair unpr = new IOPair();
                unpr.PairTO.ProcessedGenUsed = Constants.ioPairUnprocessed;
                unpr.PairTO.PassTypeID = Constants.regularWork;
                unpr.PairTO.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                List<IOPairTO> unprocessedPairsTemp = unpr.SearchClosedPairs(startIntervalTime, endIntervalTime);

                //get national and personal holidays
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(startIntervalTime, DateTime.Now, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, null, nationalTransferableHolidays);

                nationalHolidaysDays.AddRange(nationalHolidaysSundays);

                //dictionary for every day end employee with IOPairs
                Dictionary<DateTime, Dictionary<int, ArrayList>> dictEmplSchema = new Dictionary<DateTime, Dictionary<int, ArrayList>>();

                //search just active employees - prepravljeno na pretragu svih (ukoliko su prosli)
                Employee empl = new Employee();
                //22.09.2017 Miodrag / servis treba da obradi i prolaske radnika koji vise ne rade(do datuma do kog rade).
                //empl.EmplTO.Status = "ACTIVE";
                //mm
                //dictionary for all employees key is employeeID, value is EmployeeTO
                Dictionary<int, EmployeeTO> emplDict = empl.SearchDictionaryWithASCO();
                 
                WorkingUnit wu = new WorkingUnit();
                //working units in dictionary for root wu_id 
                Dictionary<int, WorkingUnitTO> WorkingUnitDictionary = wu.getWUDictionary();

                //select all pass types 
                allTypes = new PassType().SearchDictionary();

                //create employee string
                string emplString = "";
                foreach(int key in emplDict.Keys)
                {
                    EmployeeTO employee = emplDict[key];
                    //find root working unit for employee
                    if (employee.WorkingUnitID >= 0)
                        employee.WorkingUnitID = Common.Misc.getRootWorkingUnit(employee.WorkingUnitID, WorkingUnitDictionary);
                    emplString += key.ToString() + ", ";

                }
                if (emplString.Length > 0)
                    emplString = emplString.Substring(0, emplString.Length - 2);

                //get all employeesTimeSchema
                Dictionary<int, List<EmployeeTimeScheduleTO>> EmplTimeSchemas = new EmployeesTimeSchedule().SearchEmployeesSchedulesDS(emplString,startIntervalTime,endIntervalTime.AddDays(1));

                int firstDate = 0;
                int lastDate = 1;

                //dictionary of first and last date with unprocessed iopairs
                Dictionary<int, Dictionary<int, DateTime>> DateTimeDict = new Dictionary<int, Dictionary<int, DateTime>>();

                //list od datetime for each employee
                Dictionary<int, List<DateTime>> emplDateList = new Dictionary<int, List<DateTime>>();

                //list od datetime for each employee
                Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();

                //list od datetime for each employee for dayBefore
                Dictionary<int, List<DateTime>> emplDateListDayBeforeTemp = new Dictionary<int, List<DateTime>>();

                //list od datetime for each employee for dayAfter
                Dictionary<int, List<DateTime>> emplDateListDayAfterTemp = new Dictionary<int, List<DateTime>>();

                //dictionary of employees and dates for unprocessed pairs
                Dictionary<int, Dictionary<DateTime, List<IOPairTO>>> emplDateDict = new Dictionary<int, Dictionary<DateTime, List<IOPairTO>>>();
                List<IOPairTO> unprocessedPairs = new List<IOPairTO>();

                //prepare data for searching
                #region  Fill dictionaries for searching
                int workingDaysNum = Common.Misc.countWorkingDays(DateTime.Now.Date, null);
                foreach (IOPairTO unprPair in unprocessedPairsTemp)
                {
                    EmployeeTO tempEmpl = new EmployeeTO();
                    if(!emplDict.ContainsKey(unprPair.EmployeeID))
                        continue;
                    tempEmpl = emplDict[unprPair.EmployeeID];
                    DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime unprPairMonth = new DateTime(unprPair.IOPairDate.Year, unprPair.IOPairDate.Month, 1);
                    
                    // skip pairs before previous month
                    if (unprPairMonth.Date < currMonth.Date.AddMonths(-1))
                        continue;

                    if (unprPairMonth.Date < currMonth.Date && rulesDict.ContainsKey(tempEmpl.WorkingUnitID)
                        && rulesDict[tempEmpl.WorkingUnitID].ContainsKey(tempEmpl.EmployeeTypeID)
                        && rulesDict[tempEmpl.WorkingUnitID][tempEmpl.EmployeeTypeID].ContainsKey(Constants.RuleHRSSCCutOffDate)
                        && rulesDict[tempEmpl.WorkingUnitID][tempEmpl.EmployeeTypeID][Constants.RuleHRSSCCutOffDate].RuleValue < workingDaysNum)
                        continue;

                    unprocessedPairs.Add(unprPair);
                    if (!emplDateDict.ContainsKey(unprPair.EmployeeID))                    
                        emplDateDict.Add(unprPair.EmployeeID, new Dictionary<DateTime, List<IOPairTO>>());
                      
                    if (!emplDateDict[unprPair.EmployeeID].ContainsKey(unprPair.IOPairDate))
                        emplDateDict[unprPair.EmployeeID].Add(unprPair.IOPairDate, new List<IOPairTO>());
                    emplDateDict[unprPair.EmployeeID][unprPair.IOPairDate].Add(unprPair);

                    if (!emplDateList.ContainsKey(unprPair.EmployeeID))
                        emplDateList.Add(unprPair.EmployeeID, new List<DateTime>());
                    if (!emplDateList[unprPair.EmployeeID].Contains(unprPair.IOPairDate.Date))
                        emplDateList[unprPair.EmployeeID].Add(unprPair.IOPairDate.Date);

                    if (!emplDateWholeDayList.ContainsKey(unprPair.EmployeeID))
                        emplDateWholeDayList.Add(unprPair.EmployeeID, new List<DateTime>());
                    if (!emplDateWholeDayList[unprPair.EmployeeID].Contains(unprPair.IOPairDate.Date))
                        emplDateWholeDayList[unprPair.EmployeeID].Add(unprPair.IOPairDate.Date);

                    if (!DateTimeDict.ContainsKey(unprPair.EmployeeID))
                    {
                        DateTimeDict.Add(unprPair.EmployeeID, new Dictionary<int, DateTime>());
                        DateTimeDict[unprPair.EmployeeID].Add(firstDate, unprPair.IOPairDate.Date);
                        DateTimeDict[unprPair.EmployeeID].Add(lastDate, unprPair.IOPairDate.Date);
                    }
                    else
                    {
                        if (unprPair.IOPairDate.Date < DateTimeDict[unprPair.EmployeeID][firstDate])
                            DateTimeDict[unprPair.EmployeeID][firstDate] = unprPair.IOPairDate.Date;
                        else if (unprPair.IOPairDate.Date > DateTimeDict[unprPair.EmployeeID][lastDate])
                            DateTimeDict[unprPair.EmployeeID][lastDate] = unprPair.IOPairDate.Date;
                    }
                    if (unprPair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    {
                        if (!emplDateListDayAfterTemp.ContainsKey(unprPair.EmployeeID))
                            emplDateListDayAfterTemp.Add(unprPair.EmployeeID, new List<DateTime>());

                        if (!emplDateListDayAfterTemp[unprPair.EmployeeID].Contains(unprPair.IOPairDate.Date.AddDays(1)))
                            emplDateListDayAfterTemp[unprPair.EmployeeID].Add(unprPair.IOPairDate.Date.AddDays(1));
                    }
                    if (unprPair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                    {
                        if (!emplDateListDayBeforeTemp.ContainsKey(unprPair.EmployeeID))
                            emplDateListDayBeforeTemp.Add(unprPair.EmployeeID, new List<DateTime>());

                        if (!emplDateListDayBeforeTemp[unprPair.EmployeeID].Contains(unprPair.IOPairDate.Date.AddDays(-1)))
                            emplDateListDayBeforeTemp[unprPair.EmployeeID].Add(unprPair.IOPairDate.Date.AddDays(-1));
                    }
                }
                # endregion

                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> manualCreated = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                
                //list od datetime for each employee for dayBefore
                Dictionary<int, List<DateTime>> emplDateListDayBefore = new Dictionary<int, List<DateTime>>();

                //list od datetime for each employee for dayAfter
                Dictionary<int, List<DateTime>> emplDateListDayAfter = new Dictionary<int, List<DateTime>>();

                if (unprocessedPairs.Count > 0)
                {
                    // Sanja 29.11.2012. - do not take pairs for all employees, take pairs for one by one employee becouse of timeout exceeded in previous case
                    IOPairProcessed emplProcPair = new IOPairProcessed();
                    Dictionary<int, List<DateTime>> employeeDayList = new Dictionary<int, List<DateTime>>();
                    foreach (int emplID in emplDateList.Keys)
                    {
                        if (emplID == 19)
                        {
                        }
                        //debug.writeLog("++++ geting manual created pairs for employee: " + emplID.ToString().Trim() + "\n");
                        employeeDayList = new Dictionary<int, List<DateTime>>();
                        employeeDayList.Add(emplID, emplDateList[emplID]);
                        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplManualCreated = emplProcPair.getManualCreatedPairs(employeeDayList);

                        foreach (int id in emplManualCreated.Keys)
                        {
                            if (!manualCreated.ContainsKey(id))
                                manualCreated.Add(id, emplManualCreated[id]);
                        }
                    }

                    //debug.writeLog("++++ geting manual created pairs finished \n");

                    //manualy created io pairs processed
                    //manualCreated = new IOPairProcessed().getManualCreatedPairs(emplDateList);

                    //if manualy created io_pair_processed is part of night shift add day before or day after
                    foreach (int emplID in manualCreated.Keys)
                    {                       
                        foreach (DateTime date in manualCreated[emplID].Keys)
                        {
                            foreach (IOPairProcessedTO processed in manualCreated[emplID][date])
                            {      
                                //if manual created conteins pair at the end of the day
                                //process that day from 12
                                if (processed.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                {
                                    foreach (IOPairTO iop in emplDateDict[emplID][date])
                                    {
                                        if (iop.StartTime.TimeOfDay >= new TimeSpan(12, 0, 0))
                                        {
                                            if (!emplDateListDayBeforeTemp.ContainsKey(processed.EmployeeID))
                                                emplDateListDayBeforeTemp.Add(processed.EmployeeID, new List<DateTime>());

                                            if (!emplDateListDayBeforeTemp[processed.EmployeeID].Contains(processed.IOPairDate.Date))
                                                emplDateListDayBeforeTemp[processed.EmployeeID].Add(processed.IOPairDate.Date);

                                            if (!emplDateListDayAfterTemp.ContainsKey(processed.EmployeeID))
                                                emplDateListDayAfterTemp.Add(processed.EmployeeID, new List<DateTime>());

                                            if (!emplDateListDayAfterTemp[processed.EmployeeID].Contains(processed.IOPairDate.Date.AddDays(1)))
                                                emplDateListDayAfterTemp[processed.EmployeeID].Add(processed.IOPairDate.Date.AddDays(1));
                                            break;
                                        }

                                    }

                                    if (emplDateWholeDayList.ContainsKey(processed.EmployeeID) && emplDateWholeDayList[processed.EmployeeID].Contains(processed.IOPairDate.Date))
                                    {
                                        emplDateWholeDayList[processed.EmployeeID].Remove(processed.IOPairDate.Date);
                                        if (emplDateWholeDayList[processed.EmployeeID].Count <= 0)
                                            emplDateWholeDayList.Remove(processed.EmployeeID);
                                    }
                                }

                                //if manual created conteins pair at the begining of the day
                                //process that day until 12
                                if (processed.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                {
                                    foreach (IOPairTO iop in emplDateDict[emplID][date])
                                    {
                                        if (iop.StartTime.TimeOfDay <= new TimeSpan(12, 0, 0))
                                        {
                                            if (!emplDateListDayAfterTemp.ContainsKey(processed.EmployeeID))
                                                emplDateListDayAfterTemp.Add(processed.EmployeeID, new List<DateTime>());

                                            if (!emplDateListDayAfterTemp[processed.EmployeeID].Contains(processed.IOPairDate.Date))
                                                emplDateListDayAfterTemp[processed.EmployeeID].Add(processed.IOPairDate.Date);

                                            if (!emplDateListDayBeforeTemp.ContainsKey(processed.EmployeeID))
                                                emplDateListDayBeforeTemp.Add(processed.EmployeeID, new List<DateTime>());

                                            if (!emplDateListDayBeforeTemp[processed.EmployeeID].Contains(processed.IOPairDate.Date.AddDays(-1)))
                                                emplDateListDayBeforeTemp[processed.EmployeeID].Add(processed.IOPairDate.Date.AddDays(-1));
                                            if (!emplDateList.ContainsKey(emplID))
                                                emplDateList.Add(emplID, new List<DateTime>());
                                            if(!emplDateList[emplID].Contains(date.AddDays(-1)))
                                                emplDateList[emplID].Add(date.AddDays(-1));
                                            break;
                                        }
                                    }

                                    if (emplDateWholeDayList.ContainsKey(processed.EmployeeID) && emplDateWholeDayList[processed.EmployeeID].Contains(processed.IOPairDate.Date))
                                    {
                                        emplDateWholeDayList[processed.EmployeeID].Remove(processed.IOPairDate.Date);
                                        if (emplDateWholeDayList[processed.EmployeeID].Count <= 0)
                                            emplDateWholeDayList.Remove(processed.EmployeeID);
                                    }
                                }
                            }
                        }
                    }

                    foreach (int emplID in emplDateListDayBeforeTemp.Keys)
                    {
                        foreach (DateTime dateTime in emplDateListDayBeforeTemp[emplID])
                        {
                            if (!emplDateWholeDayList.ContainsKey(emplID) || !emplDateWholeDayList[emplID].Contains(dateTime))
                            {
                                if (!emplDateListDayBefore.ContainsKey(emplID))
                                    emplDateListDayBefore.Add(emplID, new List<DateTime>());

                                if (!emplDateListDayBefore[emplID].Contains(dateTime))
                                    emplDateListDayBefore[emplID].Add(dateTime);
                            }
                        }
                    }

                    foreach (int emplID in emplDateListDayAfterTemp.Keys)
                    {
                        foreach (DateTime dateTime in emplDateListDayAfterTemp[emplID])
                        {
                            if (!emplDateWholeDayList.ContainsKey(emplID) || !emplDateWholeDayList[emplID].Contains(dateTime))
                            {
                                if (!emplDateListDayAfter.ContainsKey(emplID))
                                    emplDateListDayAfter.Add(emplID, new List<DateTime>());

                                if (!emplDateListDayAfter[emplID].Contains(dateTime))
                                    emplDateListDayAfter[emplID].Add(dateTime);
                            }
                        }
                    }

                    if (emplDateListDayAfter.Count > 0 || emplDateListDayBefore.Count > 0)
                    {
                        manualCreated = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                        //manualy created io pairs processed for all days
                        if (emplDateWholeDayList.Keys.Count > 0)
                        {
                            // Sanja 22.11.2012. - do not take pairs for all employees, take pairs for one by one employee becouse of timeout exceeded in previous case
                            IOPairProcessed procPair = new IOPairProcessed();
                            Dictionary<int, List<DateTime>> emplDayList = new Dictionary<int, List<DateTime>>();
                            foreach (int emplID in emplDateWholeDayList.Keys)
                            {
                                //debug.writeLog("++++ geting manual created pairs for employee: " + emplID.ToString().Trim() + "\n");
                                emplDayList = new Dictionary<int, List<DateTime>>();
                                emplDayList.Add(emplID, emplDateWholeDayList[emplID]);
                                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplManualCreated = procPair.getManualCreatedPairs(emplDayList);

                                foreach (int id in emplManualCreated.Keys)
                                {
                                    if (!manualCreated.ContainsKey(id))
                                        manualCreated.Add(id, emplManualCreated[id]);
                                }
                            }

                            //debug.writeLog("++++ geting manual created pairs finished \n");
                        }

                        if (emplDateListDayBefore.Count > 0)
                        {
                            //manualy created io pairs processed for end of the day form 12 to 24
                            //new IOPairProcessed().getManualCreatedPairsDayBefore(emplDateListDayBefore, manualCreated);

                            // Sanja 14.09.2012. - do not take pairs for all employees, take pairs for one by one employee becouse of timeout exceeded in previous case                            
                            IOPairProcessed procPair = new IOPairProcessed();
                            Dictionary<int, List<DateTime>> emplDictDayBefore = new Dictionary<int, List<DateTime>>();
                            foreach (int emplID in emplDateListDayBefore.Keys)
                            {
                                //debug.writeLog("++++ geting manual created pairs for day before for employee: " + emplID.ToString().Trim() + "\n");
                                emplDictDayBefore = new Dictionary<int, List<DateTime>>();
                                emplDictDayBefore.Add(emplID, emplDateListDayBefore[emplID]);
                                procPair.getManualCreatedPairsDayBefore(emplDictDayBefore, manualCreated);
                            }

                            //debug.writeLog("++++ geting manual created pairs day before finished \n");
                        }

                        if (emplDateListDayAfter.Count > 0)
                        {
                            //manualy created io pairs processed for begining of the day 00 to 12
                            //new IOPairProcessed().getManualCreatedPairsDayAfter(emplDateListDayAfter, manualCreated);

                            // Sanja 14.09.2012. - do not take pairs for all employee, take pairs for one by one employee becouse of timeout exceeded in previous case
                            IOPairProcessed procPair = new IOPairProcessed();
                            Dictionary<int, List<DateTime>> emplDictDayAfter = new Dictionary<int, List<DateTime>>();
                            foreach (int emplID in emplDateListDayAfter.Keys)
                            {
                                //debug.writeLog("++++ geting manual created pairs for day after for employee: " + emplID.ToString().Trim() + "\n");
                                emplDictDayAfter = new Dictionary<int, List<DateTime>>();
                                emplDictDayAfter.Add(emplID, emplDateListDayAfter[emplID]);
                                procPair.getManualCreatedPairsDayAfter(emplDictDayAfter, manualCreated);
                            }

                            //debug.writeLog("++++ geting manual created pairs day after finished \n");
                        }
                    }
                }

                debug.writeLog("++++ Processing unprocessed io_pairs STARTED! " + DateTime.Now.ToString("hh:mm:ss") + "\n");
                Controller.DataProcessingStateChanged("Processing IOPairs - Processing unprocessed io_pairs");

                // Sanja 07.10.2013. begginigns of the shifts for particular employees and dates for flexy third shifts
                //Dictionary<int, Dictionary<DateTime, DateTime>> emplShiftBegginingsDict = new Dictionary<int, Dictionary<DateTime, DateTime>>();
                if (unprocessedPairs.Count > 0)
                {                    
                    //update all io_pairs to unprocessed
                    // Sanja 07.02.2013. update employee by employee and not all at once
                    //bool deleted = new IOPair().updateToUnprocessed(emplDateWholeDayList, emplDateListDayAfter, emplDateListDayBefore);
                    //bool deleted = true;
                    IOPair updatePair = new IOPair();
                    Dictionary<int, List<DateTime>> emplUpdateDict = new Dictionary<int, List<DateTime>>();
                    foreach (int id in emplDateWholeDayList.Keys)
                    {
                        emplUpdateDict = new Dictionary<int, List<DateTime>>();
                        emplUpdateDict.Add(id, emplDateWholeDayList[id]);
                        //tamara 29.06.2018. nova f-ja,u staroj se postavlja pair_processed_gen_used na 0 za sve prolaske,a za kontrolu prolaska treba da ostane 1
                       //updatePair.updateToUnprocessed(emplUpdateDict, new Dictionary<int, List<DateTime>>(), new Dictionary<int, List<DateTime>>());
                        updatePair.updateToUnprocessedWorkCount(emplUpdateDict, new Dictionary<int, List<DateTime>>(), new Dictionary<int, List<DateTime>>());
                    }
                    foreach (int id in emplDateListDayAfter.Keys)
                    {
                        emplUpdateDict = new Dictionary<int, List<DateTime>>();
                        emplUpdateDict.Add(id, emplDateListDayAfter[id]);
                        //tamara 29.06.2018. nova f-ja,u staroj se postavlja pair_processed_gen_used na 0 za sve prolaske,a za kontrolu prolaska treba da ostane 1
                        //updatePair.updateToUnprocessed(new Dictionary<int, List<DateTime>>(), emplUpdateDict, new Dictionary<int, List<DateTime>>());
                        updatePair.updateToUnprocessedWorkCount(new Dictionary<int, List<DateTime>>(), emplUpdateDict, new Dictionary<int, List<DateTime>>());
                    }
                    foreach (int id in emplDateListDayBefore.Keys)
                    {
                        emplUpdateDict = new Dictionary<int, List<DateTime>>();
                        emplUpdateDict.Add(id, emplDateListDayBefore[id]);
                        //tamara 29.06.2018. nova f-ja,u staroj se postavlja pair_processed_gen_used na 0 za sve prolaske,a za kontrolu prolaska treba da ostane 1
                        //updatePair.updateToUnprocessed(new Dictionary<int, List<DateTime>>(), new Dictionary<int, List<DateTime>>(), emplUpdateDict);
                        updatePair.updateToUnprocessedWorkCount(new Dictionary<int, List<DateTime>>(), new Dictionary<int, List<DateTime>>(), emplUpdateDict);
                    }                    

                    DateTime histCreated = DateTime.Now;

                    //save manual created pairs in hist table and update counters
                    IOPairsProcessedHist hist = new IOPairsProcessedHist();
                    Dictionary<int, PassTypeTO> passTypes = new Dictionary<int,PassTypeTO>();
                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int,Dictionary<int,EmployeeCounterValueTO>>();
                    //get old counter values
                    if (manualCreated.Keys.Count > 0)
                    {
                        passTypes = new PassType().SearchDictionary();
                        emplCounterValues = new EmployeeCounterValue().SearchValues(emplString);
                    }
                    foreach (int emplID in manualCreated.Keys)
                    {
                        EmployeeTO emplTO = new EmployeeTO();
                        Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                        Dictionary<int,EmployeeCounterValueTO> counterValues = new Dictionary<int,EmployeeCounterValueTO>();
                        Dictionary<int,int> counterNewValues = new Dictionary<int,int>();
                        if (emplDict.ContainsKey(emplID))
                        {
                            emplTO = emplDict[emplID];
                            if (rulesDict.ContainsKey(emplTO.WorkingUnitID) && rulesDict[emplTO.WorkingUnitID].ContainsKey(emplTO.EmployeeTypeID))
                            {
                                emplRules = rulesDict[emplTO.WorkingUnitID][emplTO.EmployeeTypeID];
                            }
                        }
                        if(emplCounterValues.ContainsKey(emplID))
                        {
                            counterValues = emplCounterValues[emplID];
                        }
                        bool pairBeforeEndWithNightShift = false;
                        bool updateCounters = true;
                        List<DateTime> weeksChanged = new List<DateTime>();
                        foreach (DateTime date in manualCreated[emplID].Keys)
                        {
                            int counterType = -1;                          

                            //for night shift do not update twice counter
                            if (pairBeforeEndWithNightShift)
                            {
                                updateCounters = false;
                                pairBeforeEndWithNightShift = false;
                            }
                            else
                            {
                                updateCounters = true;
                            }

                            foreach (IOPairProcessedTO processed in manualCreated[emplID][date])
                            {
                              
                                if (updateCounters)
                                {
                                    counterType = -1;

                                    //all counters but paid leave take from rules 
                                    foreach (string key in Constants. CounterTypesForRuleTypes.Keys)
                                    {
                                        if (emplRules.ContainsKey(key) && emplRules[key].RuleValue == processed.PassTypeID)
                                        {
                                            counterType = Constants.CounterTypesForRuleTypes[key];
                                            break;
                                        }
                                    }
                                    
                                    //paid leave take types from pass_types
                                    if (passTypes[processed.PassTypeID].LimitCompositeID != -1)
                                    {
                                        counterType = (int)Constants.EmplCounterTypes.PaidLeaveCounter;
                                    }
                                }
                                if (counterType != -1)
                                {
                                    bool is2DayShift = false;
                                    bool is2DayShiftPrevious = false;
                                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                                    WorkTimeSchemaTO workTimeSchema = null;

                                    List<EmployeeTimeScheduleTO> emplTS = new List<EmployeeTimeScheduleTO>();
                                    if (EmplTimeSchemas.ContainsKey(emplID))
                                        emplTS = EmplTimeSchemas[emplID];

                                    //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                                    //are night shift days. If day is night shift day, also take first interval of next day
                                    Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, date, ref is2DayShift,
                                        ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, timeSchema);

                                    foreach (int count in counterValues.Keys)
                                    {
                                        if (!counterNewValues.ContainsKey(count))
                                        {
                                            counterNewValues.Add(count, counterValues[count].Value);
                                        }
                                        if (count == counterType)
                                        {
                                            //if counter maesure unit is DAY decrease by one
                                            if (counterValues[count].MeasureUnit == Constants.emplCounterMesureDay)
                                            {
                                                if (counterNewValues[count] > 0)
                                                {
                                                    counterNewValues[count]--;
                                                    if (processed.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                    {
                                                        pairBeforeEndWithNightShift = true; 
                                                    }
                                                }
                                                else
                                                {
                                                    debug.writeLog(DateTime.Now + this.ToString() + ".ProcessingIOPairs() : Processing manual pairs counters faild! Counter = 0 for Employee_ID = " +
                                                        processed.EmployeeID + "; Date = " + processed.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + processed.PassTypeID.ToString());                       
                                                }
                                            }
                                            //if counter maesure unit is MINUTE
                                            else
                                            {
                                                TimeSpan pairDuration = processed.EndTime.TimeOfDay - processed.StartTime.TimeOfDay;

                                                //add one minut until midnight
                                                if(processed.EndTime.TimeOfDay == new TimeSpan(23,59,00))
                                                    pairDuration = pairDuration.Add(new TimeSpan(0,1,0));

                                                // if type is stop working done, increase counter - counter is in minutes
                                                if (emplRules.ContainsKey(Constants.RuleCompanyStopWorkingDone) && emplRules[Constants.RuleCompanyStopWorkingDone].RuleValue == processed.PassTypeID)
                                                    counterNewValues[count] += (int)pairDuration.TotalMinutes;
                                                // if type is bank hour used, increase counter - counter is in minutes
                                                else if (emplRules.ContainsKey(Constants.RuleCompanyBankHourUsed) && emplRules[Constants.RuleCompanyBankHourUsed].RuleValue == processed.PassTypeID)
                                                {
                                                    int round = 30;
                                                    if (emplRules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                                                        round = emplRules[Constants.RuleBankHoursUsedRounding].RuleValue;
                                                    counterNewValues[count] += Common.Misc.roundDuration((int)pairDuration.TotalMinutes,round,false);
                                                }
                                                else if (emplRules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed) && emplRules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue == processed.PassTypeID)
                                                {
                                                    int round = 1;
                                                    if (emplRules.ContainsKey(Constants.RuleInitialOvertimeUsedRounding))
                                                        round = emplRules[Constants.RuleInitialOvertimeUsedRounding].RuleValue;
                                                    counterNewValues[count] += Common.Misc.roundDuration((int)pairDuration.TotalMinutes, round, false);
                                                }
                                                // else decrease counter - counter is in minutes
                                                else if (pairDuration.TotalMinutes > 0) //&& counterNewValues[count] >= pairDuration.TotalMinutes)
                                                {
                                                    counterNewValues[count] = counterNewValues[count] - (int)pairDuration.TotalMinutes;
                                                    if ( counterNewValues[count] < pairDuration.TotalMinutes)
                                                        debug.writeLog(DateTime.Now + this.ToString() + ".ProcessingIOPairs() : Processing manual pairs counters faild! Counter = " + counterNewValues[count].ToString() +
                                                        " for Employee_ID = " + processed.EmployeeID + "; Date = " + processed.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + processed.PassTypeID.ToString()
                                                         + "; Start time = " + processed.StartTime.ToString("HH:mm:ss") + "; End time = " + processed.EndTime.ToString("HH:mm:ss"));
                                                }
                                                //else
                                                //{
                                                //    debug.writeLog(DateTime.Now + this.ToString() + ".ProcessingIOPairs() : Processing manual pairs counters faild! Counter = " + counterNewValues[count].ToString() +
                                                //        " for Employee_ID = " + processed.EmployeeID + "; Date = " + processed.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + processed.PassTypeID.ToString()
                                                //         + "; Start time = " + processed.StartTime.ToString("HH:mm:ss") + "; End time = " + processed.EndTime.ToString("HH:mm:ss"));

                                                //}
                                            }

                                            if (processed.StartTime.TimeOfDay != new TimeSpan(0, 0, 0)
                                                && ((emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && processed.PassTypeID == emplRules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                                || (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && processed.PassTypeID == emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)))
                                            {
                                                //if shift is more than 8 hours long and if employee has whole week annual leave 
                                                //decrease couter once more
                                                if (workTimeSchema != null && Common.Misc.is10HourShift(workTimeSchema))
                                                {
                                                    int dayNum = 0;
                                                    switch (processed.IOPairDate.DayOfWeek)
                                                    {
                                                        case DayOfWeek.Monday:
                                                            dayNum = 0;
                                                            break;
                                                        case DayOfWeek.Tuesday:
                                                            dayNum = 1;
                                                            break;
                                                        case DayOfWeek.Wednesday:
                                                            dayNum = 2;
                                                            break;
                                                        case DayOfWeek.Thursday:
                                                            dayNum = 3;
                                                            break;
                                                        case DayOfWeek.Friday:
                                                            dayNum = 4;
                                                            break;
                                                        case DayOfWeek.Saturday:
                                                            dayNum = 5;
                                                            break;
                                                        case DayOfWeek.Sunday:
                                                            dayNum = 6;
                                                            break;
                                                    }

                                                    DateTime weekBegining = processed.IOPairDate.AddDays(-dayNum).Date; // first day of current week

                                                    if (weeksChanged.Contains(weekBegining.Date))
                                                    {
                                                        break;
                                                    }
                                                    // get week annual leave pairs
                                                    int annualLeaveDays = 0;
                                                    IOPairProcessed annualPair = new IOPairProcessed();

                                                    List<IOPairProcessedTO> annualLeaveWeekPairs = annualPair.SearchWeekPairs(emplID, processed.IOPairDate, false, Common.Misc.getAnnualLeaveTypesString(emplRules), null);

                                                    foreach (IOPairProcessedTO aPair in annualLeaveWeekPairs)
                                                    {
                                                        // second night shift belongs to previous day
                                                        if (aPair.StartTime.Hour == 0 && aPair.StartTime.Minute == 0)
                                                            continue;

                                                        annualLeaveDays++;
                                                    }

                                                    // if fourts day of week is changed, subtract/add two days
                                                    if (annualLeaveDays == (Constants.fullWeek10hShift - 1))
                                                    {
                                                        counterNewValues[count]--;
                                                        weeksChanged.Add(weekBegining);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //update counters and save history
                                    manualPairCounters(processed, counterNewValues, counterValues, histCreated);
                                    foreach (int counterTypeID in counterNewValues.Keys)
                                    {
                                        foreach (int key in counterValues.Keys)
                                        {
                                            if (counterTypeID == key)
                                            {
                                                counterValues[key].Value = counterNewValues[key];
                                                break;
                                            }
                                        }
                                    }
                                }
                                // if no need to update counters just save manual created pair to hist table
                                else
                                {
                                    IOPairsProcessedHistTO histTO = new IOPairsProcessedHistTO(processed);
                                    hist.IOPairProcessedHistTO = histTO;
                                    hist.IOPairProcessedHistTO.Alert = Constants.alertStatus.ToString();
                                    hist.IOPairProcessedHistTO.ModifiedTime = histCreated;
                                    hist.Save(true);
                                }
                            }
                        }
                    }

                    //delete all processed iopairs where alert is not manualy changed
                    // Sanja 07.02.2013. delete employee by employee and not all at once
                    //deleted = new IOPairProcessed().DeleteProcessedPairs(emplDateWholeDayList, emplDateListDayAfter, emplDateListDayBefore);
                    IOPairProcessed deletePair = new IOPairProcessed();
                    Dictionary<int, List<DateTime>> emplDeleteDict = new Dictionary<int, List<DateTime>>();
                    foreach (int id in emplDateWholeDayList.Keys)
                    {
                        emplDeleteDict = new Dictionary<int, List<DateTime>>();
                        emplDeleteDict.Add(id, emplDateWholeDayList[id]);
                        deletePair.DeleteProcessedPairs(emplDeleteDict, new Dictionary<int, List<DateTime>>(), new Dictionary<int, List<DateTime>>());
                    }
                    foreach (int id in emplDateListDayAfter.Keys)
                    {
                        emplDeleteDict = new Dictionary<int, List<DateTime>>();
                        emplDeleteDict.Add(id, emplDateListDayAfter[id]);
                        deletePair.DeleteProcessedPairs(new Dictionary<int, List<DateTime>>(), emplDeleteDict, new Dictionary<int, List<DateTime>>());
                    }
                    foreach (int id in emplDateListDayBefore.Keys)
                    {
                        emplDeleteDict = new Dictionary<int, List<DateTime>>();
                        emplDeleteDict.Add(id, emplDateListDayBefore[id]);
                        deletePair.DeleteProcessedPairs(new Dictionary<int, List<DateTime>>(), new Dictionary<int, List<DateTime>>(), emplDeleteDict);
                    }                    

                    //get all ioPairs unprocessed
                    unprocessedPairs = unpr.SearchClosedPairs();
                    emplDateDict = new Dictionary<int, Dictionary<DateTime, List<IOPairTO>>>();
                    # region Update Dictionaries for searching
                    foreach (IOPairTO unprPair in unprocessedPairs)
                    {
                        if (!emplDateDict.ContainsKey(unprPair.EmployeeID))
                            emplDateDict.Add(unprPair.EmployeeID, new Dictionary<DateTime, List<IOPairTO>>());

                        if (!emplDateDict[unprPair.EmployeeID].ContainsKey(unprPair.IOPairDate))
                            emplDateDict[unprPair.EmployeeID].Add(unprPair.IOPairDate, new List<IOPairTO>());
                        emplDateDict[unprPair.EmployeeID][unprPair.IOPairDate].Add(unprPair);
                        if (!DateTimeDict.ContainsKey(unprPair.EmployeeID))
                        {
                            DateTimeDict.Add(unprPair.EmployeeID, new Dictionary<int, DateTime>());
                            DateTimeDict[unprPair.EmployeeID].Add(firstDate, unprPair.IOPairDate.Date);
                            DateTimeDict[unprPair.EmployeeID].Add(lastDate, unprPair.IOPairDate.Date);
                        }
                        else
                        {
                            if (unprPair.IOPairDate.Date < DateTimeDict[unprPair.EmployeeID][firstDate])
                                DateTimeDict[unprPair.EmployeeID][firstDate] = unprPair.IOPairDate.Date;
                            else if (unprPair.IOPairDate.Date > DateTimeDict[unprPair.EmployeeID][lastDate])
                                DateTimeDict[unprPair.EmployeeID][lastDate] = unprPair.IOPairDate.Date;
                        }
                    }
                    # endregion

                    //if no io pair unprocessed found do not process                    
                    if (unprocessedPairs.Count != 0)
                    {
                        // get new schedules if are changed
                        EmplTimeSchemas = new EmployeesTimeSchedule().SearchEmployeesSchedulesDS(emplString, startIntervalTime, endIntervalTime.AddDays(1));                        
                        processingUnprocessedIOPairs(emplDateDict, emplDict, rulesDict, EmplTimeSchemas, DateTimeDict, firstDate, lastDate, timeSchema, WorkingUnitDictionary, 
                            unprocessedPairs.Count,nationalHolidaysDays,personalHolidayDays, workingDaysNum);
                    }
                }

                debug.writeLog("---- Processing unprocessed io_pairs FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") );
               
                //Dictionary<int, List<DateTime>> emplDatesWithoutPairs = new IOPairProcessed().getDatesForEmplWithNoPairs(startIntervalTime, endIntervalTime, emplDict);
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> processedIOPairs = new IOPairProcessed().getPairsForInterval(startIntervalTime, endIntervalTime.AddDays(1));
                Dictionary<int, List<DateTime>> emplDatesWithoutPairs = new Dictionary<int, List<DateTime>>();

                DateTime startCheckingDate = endIntervalTime.Date;
                // get max HRSSC cut off date
                int maxHRSSCCutOffDate = 0;
                foreach (int company in rulesDict.Keys)
                {
                    foreach (int type in rulesDict[company].Keys)
                    {
                        if (rulesDict[company][type].ContainsKey(Constants.RuleHRSSCCutOffDate) && maxHRSSCCutOffDate < rulesDict[company][type][Constants.RuleHRSSCCutOffDate].RuleValue)
                            maxHRSSCCutOffDate = rulesDict[company][type][Constants.RuleHRSSCCutOffDate].RuleValue;
                    }
                }

                if (maxHRSSCCutOffDate > 0 && workingDaysNum > maxHRSSCCutOffDate)
                    startCheckingDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;
                // get new schedules if are changed
                EmplTimeSchemas = new EmployeesTimeSchedule().SearchEmployeesSchedulesDS(emplString, startIntervalTime, endIntervalTime.AddDays(1));
                foreach (int emplID in EmplTimeSchemas.Keys)
                {
                    //18.09.2017 Miodrag / dodao try/catch da ne puca ceo servis zbog jednog zapisa
                    try
                    {
                        int holidayPT = -1;
                        int personalHolidayPT = -1;

                        if (emplDict.ContainsKey(emplID) && rulesDict.ContainsKey(emplDict[emplID].WorkingUnitID)
                            && rulesDict[emplDict[emplID].WorkingUnitID].ContainsKey(emplDict[emplID].EmployeeTypeID))
                        {
                            if (rulesDict[emplDict[emplID].WorkingUnitID][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleHolidayPassType))
                                holidayPT = rulesDict[emplDict[emplID].WorkingUnitID][emplDict[emplID].EmployeeTypeID][Constants.RuleHolidayPassType].RuleValue;

                            if (rulesDict[emplDict[emplID].WorkingUnitID][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RulePersonalHolidayPassType))
                                personalHolidayPT = rulesDict[emplDict[emplID].WorkingUnitID][emplDict[emplID].EmployeeTypeID][Constants.RulePersonalHolidayPassType].RuleValue;
                        }

                        List<EmployeeTimeScheduleTO> emplSchList = EmplTimeSchemas[emplID];
                        DateTime currentDate = startCheckingDate.Date;
                        while (currentDate.Date <= endIntervalTime.Date)
                        {
                            if ((emplDatesWithoutPairs.ContainsKey(emplID) && emplDatesWithoutPairs[emplID].Contains(currentDate.Date))
                                || (emplDateList.ContainsKey(emplID) && emplDateList[emplID].Contains(currentDate.Date)))
                            {
                                currentDate = currentDate.AddDays(1).Date;
                                continue;
                            }

                            int schMinutes = 0;
                            int pairMinutes = 0;

                            // get scheduled hours for that day
                            bool is2DayShift = false;
                            bool is2DayShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                            WorkTimeSchemaTO workTimeSchema = null;

                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                            //are night shift days. If day is night shift day, also take first interval of next day
                            Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplSchList, currentDate.Date, ref is2DayShift,
                                ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, timeSchema);

                            if (workTimeSchema != null)
                            {
                                foreach (int num in edi.Keys)
                                {
                                    if (edi[num].StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                        continue;

                                    schMinutes += (int)edi[num].EndTime.TimeOfDay.Subtract(edi[num].StartTime.TimeOfDay).TotalMinutes;

                                    if (edi[num].EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        schMinutes++;
                                }

                                schMinutes += (int)firstIntervalNextDay.EndTime.TimeOfDay.Subtract(firstIntervalNextDay.StartTime.TimeOfDay).TotalMinutes;

                                if (firstIntervalNextDay.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                    schMinutes++;

                                // get pair minutes
                                List<IOPairProcessedTO> emplDayPairs = new List<IOPairProcessedTO>();
                                if (processedIOPairs.ContainsKey(emplID))
                                {
                                    if (processedIOPairs[emplID].ContainsKey(currentDate.Date))
                                        emplDayPairs = processedIOPairs[emplID][currentDate.Date];

                                    foreach (IOPairProcessedTO dayPair in emplDayPairs)
                                    {
                                        if (dayPair.PassTypeID == Constants.absence || dayPair.PassTypeID == holidayPT || dayPair.PassTypeID == personalHolidayPT
                                            || (allTypes.ContainsKey(dayPair.PassTypeID) && (allTypes[dayPair.PassTypeID].IsPass == Constants.passOnReader
                                            || allTypes[dayPair.PassTypeID].IsPass == Constants.wholeDayAbsence)))
                                        {
                                            foreach (int num in edi.Keys)
                                            {
                                                if (edi[num].StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                                    continue;

                                                if (dayPair.StartTime.TimeOfDay >= edi[num].EarliestArrived.TimeOfDay && dayPair.EndTime.TimeOfDay <= edi[num].LatestLeft.TimeOfDay)
                                                {
                                                    pairMinutes += (int)dayPair.EndTime.TimeOfDay.Subtract(dayPair.StartTime.TimeOfDay).TotalMinutes;

                                                    if (dayPair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                        pairMinutes++;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    if (is2DayShift && processedIOPairs[emplID].ContainsKey(currentDate.Date.AddDays(1)))
                                    {
                                        emplDayPairs = processedIOPairs[emplID][currentDate.Date.AddDays(1)];

                                        foreach (IOPairProcessedTO dayPair in emplDayPairs)
                                        {
                                            if (dayPair.PassTypeID == Constants.absence || dayPair.PassTypeID == holidayPT || dayPair.PassTypeID == personalHolidayPT
                                                || (allTypes.ContainsKey(dayPair.PassTypeID) && (allTypes[dayPair.PassTypeID].IsPass == Constants.passOnReader
                                                || allTypes[dayPair.PassTypeID].IsPass == Constants.wholeDayAbsence)))
                                            {
                                                if (dayPair.StartTime.TimeOfDay >= firstIntervalNextDay.EarliestArrived.TimeOfDay && dayPair.EndTime.TimeOfDay <= firstIntervalNextDay.LatestLeft.TimeOfDay)
                                                {
                                                    pairMinutes += (int)dayPair.EndTime.TimeOfDay.Subtract(dayPair.StartTime.TimeOfDay).TotalMinutes;

                                                    if (dayPair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                        pairMinutes++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (schMinutes > pairMinutes)
                            {
                                if (pairMinutes == 0)
                                {
                                    if (!emplDatesWithoutPairs.ContainsKey(emplID))
                                        emplDatesWithoutPairs.Add(emplID, new List<DateTime>());
                                    if (!emplDatesWithoutPairs[emplID].Contains(currentDate.Date))
                                        emplDatesWithoutPairs[emplID].Add(currentDate.Date);
                                }
                                else
                                {
                                    if (!emplDateList.ContainsKey(emplID))
                                        emplDateList.Add(emplID, new List<DateTime>());
                                    if (!emplDateList[emplID].Contains(currentDate.Date))
                                        emplDateList[emplID].Add(currentDate.Date);
                                }
                            }

                            currentDate = currentDate.AddDays(1).Date;
                        }
                    }
                    catch (Exception ex)
                    {
                        debug.writeLog(DateTime.Now + " Exception in foreach (int emplID in EmplTimeSchemas.Keys) for employee with id: " + emplID + "\n" +
                            this.ToString() + ".ProcessingIOPairs() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                    }
                    //mm
                }
                
                debug.writeLog("++++ Processing unjustified pairs and delayes STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") );
                Controller.DataProcessingStateChanged("Processing IOPairs - Processing unjustified pairs and delayes"); 
                //inserting holes and delay
                insertHolesAndDelay(emplDatesWithoutPairs, emplDict, rulesDict, startIntervalTime, EmplTimeSchemas, timeSchema, processedIOPairs, WorkingUnitDictionary, emplDateList, 
                    nationalHolidaysDays, personalHolidayDays, workingDaysNum);
                debug.writeLog("---- Processing unjustified pairs FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") );
            }//try
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".ProcessingIOPairs() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace );
            }
        }

        private void insertHolesAndDelay(Dictionary<int, List<DateTime>> emplDatesWithoutPairs, Dictionary<int, EmployeeTO> emplDict, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary,
            DateTime startIntervalTime, Dictionary<int, List<EmployeeTimeScheduleTO>> EmplTimeSchemas, Dictionary<int, WorkTimeSchemaTO> timeSchema, 
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> processedIOPairs, Dictionary<int, WorkingUnitTO> WorkingUnitDictionary, Dictionary<int, List<DateTime>> emplDateList,
            List<DateTime> nationalHolidaysDays, Dictionary<string, List<DateTime>> personalHolidayDays, int workingDaysNum)
        {
            int currentEmployeeID = -1;
            try
            {
                // check if there is location rule
                Common.Rule rule = new Common.Rule();
                rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                rule.RuleTO.RuleType = Constants.RulePairsByLocation;
                List<RuleTO> locRules = rule.Search();

                bool byLocation = locRules.Count > 0 && locRules[0].RuleValue == Constants.yesInt;
                
                int count = 0;

                //process for each employee in dictionary holes and delays
                foreach (int emplID in emplDict.Keys)
                {
                    if (emplID == 839)
                    {
                    }
                    count++;
                    currentEmployeeID = emplID;

                    Controller.DataProcessingStateChanged("Processing IOPairs - Processing unjustified pairs and delayes " + count.ToString() + "/" + emplDict.Keys.Count);

                    if ((!emplDatesWithoutPairs.ContainsKey(emplID) || (emplDatesWithoutPairs.ContainsKey(emplID) && emplDatesWithoutPairs[emplID].Count == 0))
                        && !emplDateList.ContainsKey(emplID))
                        continue;

                    //find employee 
                    EmployeeTO employee = emplDict[emplID];
                    EmployeeAsco4TO employeeAsco4 = (EmployeeAsco4TO)employee.Tag;

                    //find root working unit for employee
                    if (employee.WorkingUnitID >= 0)
                        employee.WorkingUnitID = Common.Misc.getRootWorkingUnit(employee.WorkingUnitID, WorkingUnitDictionary);

                    //get rules for specific employee
                    Dictionary<string, RuleTO> rulesForEmployee = new Dictionary<string, RuleTO>();
                    if (rulesDictionary.ContainsKey(employee.WorkingUnitID) && rulesDictionary[employee.WorkingUnitID].ContainsKey(employee.EmployeeTypeID))
                        rulesForEmployee = rulesDictionary[employee.WorkingUnitID][employee.EmployeeTypeID];

                    List<EmployeeTimeScheduleTO> emplTS = new List<EmployeeTimeScheduleTO>();
                    if (EmplTimeSchemas.ContainsKey(emplID))
                        emplTS = EmplTimeSchemas[emplID];
                    else
                        continue;

                    if (emplDateList.ContainsKey(emplID))
                    {
                        foreach (DateTime date in emplDateList[emplID])
                        {
                            if (processedIOPairs.ContainsKey(emplID))
                            {
                                Employee empl = new Employee();
                                empl.EmplTO.EmployeeID = employee.EmployeeID;

                                bool is2DayShift = false;
                                bool is2DayShiftPrevious = false;
                                WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                                WorkTimeSchemaTO workTimeSchema = null;

                                //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                                //are night shift days. If day is night shift day, also take first interval of next day
                                Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, date, ref is2DayShift,
                                    ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, timeSchema);
                                
                                if (workTimeSchema == null)
                                    continue;

                                List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                TimeSpan intervalDuration = new TimeSpan();
                                
                                if (edi != null)
                                {
                                    intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);

                                    foreach (WorkTimeIntervalTO interval in intervals)
                                    {
                                        intervalDuration += interval.EndTime - interval.StartTime;
                                    }
                                }

                                if (intervalDuration.TotalMinutes <= 0 && !(is2DayShiftPrevious && !emplDateList[emplID].Contains(date.AddDays(-1))))
                                    continue;

                                // intervals contains 'normal' intervals for day or third shift start interval from date and third shift end interval from next day
                                // get holes type for date - all interval belong to date
                                int holesType = getHoleType(workTimeSchema, nationalHolidaysDays, personalHolidayDays, rulesForEmployee, employeeAsco4, date, employee, rulesDictionary);
                                if (is2DayShift || is2DayShiftPrevious)
                                {
                                    foreach (WorkTimeIntervalTO interval in intervals)
                                    {
                                        if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0) && interval.EndTime.TimeOfDay != new TimeSpan(23, 59, 0))
                                        {
                                            DateTime startTime = interval.StartTime;
                                            DateTime endTime = interval.EndTime;

                                            if (workTimeSchema.Type == Constants.schemaTypeFlexi || workTimeSchema.Type == Constants.schemaTypeNightFlexi)
                                            {
                                                List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>(); // interval is from current day, not night interval
                                                int rounding = 1;
                                                if (rulesForEmployee.ContainsKey(Constants.RulePresenceRounding))
                                                {
                                                    rounding = rulesForEmployee[Constants.RulePresenceRounding].RuleValue;
                                                }

                                                List<int> emplList = new List<int>();
                                                emplList.Add(emplID);
                                                //List<IOPairTO> dayPairs = new IOPair().SearchAll(date.Date, date.Date, emplList);
                                                //List<IOPairTO> nextDayPairs = new IOPair().SearchAll(date.AddDays(1).Date, date.AddDays(1).Date, emplList);
                                                Common.Misc.GetEmployeeShiftStartEnd(emplID, date.Date, interval, prevDayIntervals, firstIntervalNextDay,
                                                    workTimeSchema, is2DayShift, is2DayShiftPrevious, rounding, ref startTime, ref endTime);
                                            }

                                            List<IOPairProcessedTO> list = new List<IOPairProcessedTO>();
                                            if (processedIOPairs[emplID].ContainsKey(date.Date))
                                                list = processedIOPairs[emplID][date.Date];
                                            processHolesForDayAndEmployee(emplID, date, list, new List<IOPairProcessedTO>(), new WorkTimeIntervalTO(), rulesForEmployee, startTime, endTime, holesType, byLocation);
                                        }
                                    }
                                }

                                if (!is2DayShift && !is2DayShiftPrevious)
                                {
                                    List<IOPairProcessedTO> list = new List<IOPairProcessedTO>();
                                    if (processedIOPairs[emplID].ContainsKey(date.Date))
                                        list = processedIOPairs[emplID][date.Date];

                                    foreach (WorkTimeIntervalTO interval in intervals)
                                    {
                                        if (emplID == 1)
                                        {
                                        }
                                        DateTime startTime = interval.StartTime;
                                        DateTime endTime = interval.EndTime;

                                        if (workTimeSchema.Type == Constants.schemaTypeFlexi || workTimeSchema.Type == Constants.schemaTypeNightFlexi)
                                        {
                                            List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>(); // interval is from current day, not night interval
                                            int rounding = 1;
                                            if (rulesForEmployee.ContainsKey(Constants.RulePresenceRounding))
                                            {
                                                rounding = rulesForEmployee[Constants.RulePresenceRounding].RuleValue;
                                            }

                                            //List<int> emplList = new List<int>();
                                            //emplList.Add(emplID);
                                            //List<IOPairTO> dayPairs = new IOPair().SearchAll(date.Date, date.Date, emplList);
                                            Common.Misc.GetEmployeeShiftStartEnd(emplID, date.Date, interval, prevDayIntervals, firstIntervalNextDay,
                                                workTimeSchema, is2DayShift, is2DayShiftPrevious, rounding, ref startTime, ref endTime);
                                        }

                                        processHolesForDayAndEmployee(emplID, date, list, new List<IOPairProcessedTO>(), new WorkTimeIntervalTO(), rulesForEmployee, startTime, endTime, holesType, byLocation);
                                    }
                                }
                                else if (is2DayShift)
                                {
                                    foreach (WorkTimeIntervalTO interval in intervals)
                                    {
                                        DateTime startTime = interval.StartTime;
                                        DateTime endTime = interval.EndTime;

                                        if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                        {
                                            if (workTimeSchema.Type == Constants.schemaTypeFlexi || workTimeSchema.Type == Constants.schemaTypeNightFlexi)
                                            {
                                                List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>();
                                                int rounding = 1;
                                                if (rulesForEmployee.ContainsKey(Constants.RulePresenceRounding))
                                                {
                                                    rounding = rulesForEmployee[Constants.RulePresenceRounding].RuleValue;
                                                }

                                                //List<int> emplList = new List<int>();
                                                //emplList.Add(emplID);

                                                //List<IOPairTO> dayPairs = new IOPair().SearchAll(date.Date, date.Date, emplList);
                                                //List<IOPairTO> nextDayPairs = new IOPair().SearchAll(date.AddDays(1).Date, date.AddDays(1).Date, emplList);
                                                Common.Misc.GetEmployeeShiftStartEnd(emplID, date.Date, interval, prevDayIntervals, firstIntervalNextDay,
                                                        workTimeSchema, is2DayShift, is2DayShiftPrevious, rounding, ref startTime, ref endTime);
                                            }

                                            List<IOPairProcessedTO> list = new List<IOPairProcessedTO>();
                                            if (processedIOPairs[emplID].ContainsKey(date.Date))
                                                list = processedIOPairs[emplID][date.Date];

                                            
                                                List<IOPairProcessedTO> listNextDay = new List<IOPairProcessedTO>();
                                                if (processedIOPairs[emplID].ContainsKey(date.Date.AddDays(1)))
                                                    listNextDay = processedIOPairs[emplID][date.Date.AddDays(1)];
                                                processHolesForDayAndEmployee(emplID, date, list, listNextDay, firstIntervalNextDay, rulesForEmployee, startTime, endTime, holesType, byLocation);
                                            
                                         }
                                        else
                                        {
                                            // interval is from next day
                                            if (workTimeSchema.Type == Constants.schemaTypeFlexi || workTimeSchema.Type == Constants.schemaTypeNightFlexi)
                                            {
                                                List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>();
                                                int rounding = 1;
                                                if (rulesForEmployee.ContainsKey(Constants.RulePresenceRounding))
                                                {
                                                    rounding = rulesForEmployee[Constants.RulePresenceRounding].RuleValue;
                                                }

                                                //List<int> emplList = new List<int>();
                                                //emplList.Add(emplID);

                                                foreach (WorkTimeIntervalTO dayInterval in intervals)
                                                {
                                                    if (dayInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                    {
                                                        prevDayIntervals.Add(dayInterval);
                                                        break;
                                                    }
                                                }
                                                //List<IOPairTO> dayPairs = new IOPair().SearchAll(date.AddDays(1).Date, date.AddDays(1).Date, emplList);
                                                // previous for next day interval is current day, so nightShiftPreviousDay is is2DayShift
                                                Common.Misc.GetEmployeeShiftStartEnd(emplID, date.AddDays(1).Date, interval, prevDayIntervals, firstIntervalNextDay,
                                                    workTimeSchema, is2DayShift, is2DayShift, rounding, ref startTime, ref endTime);
                                            }

                                            List<IOPairProcessedTO> list = new List<IOPairProcessedTO>();
                                            if (processedIOPairs[emplID].ContainsKey(date.AddDays(1)))
                                                list = processedIOPairs[emplID][date.AddDays(1)];
                                            processHolesForDayAndEmployee(emplID, date.AddDays(1), list, new List<IOPairProcessedTO>(), new WorkTimeIntervalTO(), rulesForEmployee, startTime, endTime, holesType, byLocation);
                                        }
                                    }
                                }

                                //when day before is two day shift
                                if (is2DayShiftPrevious)
                                {
                                    edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, date.AddDays(-1), ref is2DayShift,
                                        ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, timeSchema);

                                    if (workTimeSchema == null)
                                        continue;

                                    intervals = new List<WorkTimeIntervalTO>();
                                    intervalDuration = new TimeSpan();

                                    if (edi != null)
                                    {
                                        intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);

                                        // find start and end time of working day
                                        foreach (WorkTimeIntervalTO interval in intervals)
                                        {
                                            intervalDuration += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                        }
                                    }

                                   
                                    // get hole type for previous day
                                    holesType = getHoleType(workTimeSchema, nationalHolidaysDays, personalHolidayDays, rulesForEmployee, employeeAsco4, date.AddDays(-1), employee, rulesDictionary);

                                    if (intervalDuration.TotalMinutes > 0)
                                    {
                                        foreach (WorkTimeIntervalTO interval in intervals)
                                        {
                                            DateTime startTime = interval.StartTime;
                                            DateTime endTime = interval.EndTime;

                                            List<IOPairProcessedTO> list = new List<IOPairProcessedTO>();
                                            if (processedIOPairs[emplID].ContainsKey(date.Date))
                                                list = processedIOPairs[emplID][date.Date];

                                            if (interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                            {
                                                if (workTimeSchema.Type == Constants.schemaTypeFlexi || workTimeSchema.Type == Constants.schemaTypeNightFlexi)
                                                {
                                                    List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>();
                                                    int rounding = 1;
                                                    if (rulesForEmployee.ContainsKey(Constants.RulePresenceRounding))
                                                    {
                                                        rounding = rulesForEmployee[Constants.RulePresenceRounding].RuleValue;
                                                    }

                                                    //List<int> emplList = new List<int>();
                                                    //emplList.Add(emplID);

                                                    foreach (WorkTimeIntervalTO dayInterval in intervals)
                                                    {
                                                        if (dayInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                        {
                                                            prevDayIntervals.Add(dayInterval);
                                                            break;
                                                        }
                                                    }

                                                    //List<IOPairTO> dayPairs = new IOPair().SearchAll(date.Date, date.Date, emplList);
                                                    Common.Misc.GetEmployeeShiftStartEnd(emplID, date.Date, interval, prevDayIntervals, firstIntervalNextDay,
                                                        workTimeSchema, is2DayShift, is2DayShift, rounding, ref startTime, ref endTime);
                                                }
                                                
                                                    processHolesForDayAndEmployee(emplID, date, list, new List<IOPairProcessedTO>(), new WorkTimeIntervalTO(), rulesForEmployee, startTime, endTime, holesType, byLocation);
                                                
                                            }
                                            else
                                            {
                                                if (workTimeSchema.Type == Constants.schemaTypeFlexi || workTimeSchema.Type == Constants.schemaTypeNightFlexi)
                                                {
                                                    List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>();
                                                    int rounding = 1;
                                                    if (rulesForEmployee.ContainsKey(Constants.RulePresenceRounding))
                                                    {
                                                        rounding = rulesForEmployee[Constants.RulePresenceRounding].RuleValue;
                                                    }

                                                    //List<int> emplList = new List<int>();
                                                    //emplList.Add(emplID);

                                                    //List<IOPairTO> dayPairs = new IOPair().SearchAll(date.AddDays(-1).Date, date.AddDays(-1).Date, emplList);
                                                    //List<IOPairTO> nextDayPairs = new IOPair().SearchAll(date.Date, date.Date, emplList);
                                                    Common.Misc.GetEmployeeShiftStartEnd(emplID, date.AddDays(-1).Date, interval, prevDayIntervals, firstIntervalNextDay,
                                                        workTimeSchema, is2DayShift, is2DayShiftPrevious, rounding, ref startTime, ref endTime);
                                                }
                                               
                                                List<IOPairProcessedTO> listPrevDay = new List<IOPairProcessedTO>();
                                                if (processedIOPairs[emplID].ContainsKey(date.Date.AddDays(-1)))
                                                    listPrevDay = processedIOPairs[emplID][date.Date.AddDays(-1)];
                                                processHolesForDayAndEmployee(emplID, date.AddDays(-1), listPrevDay, list, firstIntervalNextDay, rulesForEmployee, startTime, endTime, holesType, byLocation);
                                                
                                             }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (emplDatesWithoutPairs.ContainsKey(emplID))
                    {
                        //process hole day absence
                        DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        foreach (DateTime date in emplDatesWithoutPairs[emplID])
                        {
                            DateTime dateMonth = new DateTime(date.Year, date.Month, 1);

                            if (dateMonth.Date < currMonth.AddMonths(-1).Date)
                                continue;

                            if (rulesForEmployee.ContainsKey(Constants.RuleHRSSCCutOffDate))
                            {
                                if (rulesForEmployee[Constants.RuleHRSSCCutOffDate].RuleValue < workingDaysNum && dateMonth.Date < currMonth.Date)
                                    continue;
                            }

                            Employee empl = new Employee();
                            empl.EmplTO.EmployeeID = employee.EmployeeID;

                            //calculate holes only for employee's dates
                            bool is2DayShift = false;
                            bool is2DayShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                            WorkTimeSchemaTO workTimeSchema = null;

                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                            //are night shift days. If day is night shift day, also take first interval of next day
                            Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, date, ref is2DayShift,
                                ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, timeSchema);
                            if (workTimeSchema == null)
                                continue;

                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            TimeSpan intervalDuration = new TimeSpan();

                            if (edi != null)
                            {
                                intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);

                                // find start and end time of working day
                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    intervalDuration += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                }
                            }

                            //if employee should work on current day
                            if (intervalDuration.TotalMinutes > 0)
                            {
                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    DateTime startTimeInterval = interval.StartTime;
                                    DateTime endTimeInterval = interval.EndTime;

                                    if (workTimeSchema.Type.Trim() == Constants.schemaTypeFlexi || workTimeSchema.Type == Constants.schemaTypeNightFlexi)
                                    {
                                        if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0) && interval.EndTime.TimeOfDay != new TimeSpan(23, 59, 0))
                                        {
                                            startTimeInterval = interval.EarliestArrived;
                                            TimeSpan duration = interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                            endTimeInterval = startTimeInterval.AddMinutes(duration.TotalMinutes);
                                        }
                                    }

                                    int absenceType = getHoleType(workTimeSchema, nationalHolidaysDays, personalHolidayDays, rulesForEmployee, employeeAsco4, date.Date, employee, rulesDictionary);
                                    IOPairProcessedTO processedTO = new IOPairProcessedTO();
                                    DateTime dt = date;
                                    //if second interval of the night shift io pair date is for next day
                                    if (is2DayShift && interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                        dt = date.Date.AddDays(1);
                                    processedTO.StartTime = new DateTime(dt.Year, dt.Month, dt.Day, startTimeInterval.Hour, startTimeInterval.Minute, startTimeInterval.Second);
                                    processedTO.EndTime = new DateTime(dt.Year, dt.Month, dt.Day, endTimeInterval.Hour, endTimeInterval.Minute, endTimeInterval.Second);
                                    processedTO.IOPairDate = dt.Date;
                                    processedTO.IOPairID = Constants.unjustifiedIOPairID;
                                    processedTO.EmployeeID = emplID;
                                    processedTO.LocationID = Constants.locationOut;
                                    processedTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                                    // tamara  17.7.2018.
                                    // Nenad insert correct pass type 
                                    //if (processedTO.PassTypeID != 0)
                                    //    insertProcessed(null, processedTO, processedTO.PassTypeID);
                                    //else
                                    insertProcessed(null, processedTO, absenceType);
                                }
                            }
                        }//for (DateTime date = startTime; date <= endTime; date = date.AddDays(1))
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".insertHolesAndDelay() : Message: " + ex.Message + "; Employee ID:" + currentEmployeeID.ToString() + "/n StackTrace:" + ex.StackTrace);
            }
        }

        private void processingUnprocessedIOPairs(Dictionary<int, Dictionary<DateTime, List<IOPairTO>>> emplDateDict, Dictionary<int, EmployeeTO> emplDict, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary,
            Dictionary<int, List<EmployeeTimeScheduleTO>> EmplTimeSchemas, Dictionary<int, Dictionary<int, DateTime>> DateTimeDict, int firstDate, int lastDate, Dictionary<int, WorkTimeSchemaTO> timeSchema,
             Dictionary<int, WorkingUnitTO> WorkingUnitDictionary, int unprocessedCount, List<DateTime> nationalHolidaysDays, Dictionary<string, List<DateTime>> personalHolidayDays, 
            int workingDaysNum)
        {
            int currentPairProcessing = 0;
            try
            {
                int numOfProcessed = 0;
                IOPair ioPair = new IOPair();

                // Sanja 02.10.2013. dictionary of intervals and first arrivals for specific day for flexy third shifts
                Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervalsDict = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

                //inserting io_pairs_processed
                foreach (int emplID in emplDateDict.Keys)
                {
                    if (emplID == 904)
                    {
                    }
                    try
                    {
                        
                        //find employee 
                        EmployeeTO employee = new EmployeeTO();
                        if (emplDict.ContainsKey(emplID))
                            employee = emplDict[emplID];
                        else
                        {
                            debug.writeLog(DateTime.Now.ToString() + this.ToString() + ".processingUnprocessedIOPairs() : Employee not found employee_id " + emplID);
                            continue;
                        }

                        EmployeeAsco4TO employeeAsco4 = (EmployeeAsco4TO)employee.Tag;

                        //get rules for specific employee
                        Dictionary<string, RuleTO> rulesForEmployee = new Dictionary<string, RuleTO>();
                        if (rulesDictionary.ContainsKey(employee.WorkingUnitID) && rulesDictionary[employee.WorkingUnitID].ContainsKey(employee.EmployeeTypeID))
                            rulesForEmployee = rulesDictionary[employee.WorkingUnitID][employee.EmployeeTypeID];

                        //rounding rule
                        int rounding = Constants.roundingPairMinutes;
                        if (rulesForEmployee.ContainsKey(Constants.RulePresenceRounding))
                            rounding = rulesForEmployee[Constants.RulePresenceRounding].RuleValue;

                        // shift beggining offset rule
                        int shiftBegginingOffset = 0;
                        if (rulesForEmployee.ContainsKey(Constants.RuleShiftBegginingOffset))
                            shiftBegginingOffset = rulesForEmployee[Constants.RuleShiftBegginingOffset].RuleValue;

                        // shift beggining offset rule
                        int shiftEndingOffset = 0;
                        if (rulesForEmployee.ContainsKey(Constants.RuleShiftEndingOffset))
                            shiftEndingOffset = rulesForEmployee[Constants.RuleShiftEndingOffset].RuleValue;

                        List<EmployeeTimeScheduleTO> emplTS = new List<EmployeeTimeScheduleTO>();
                        if (EmplTimeSchemas.ContainsKey(emplID))
                            emplTS = EmplTimeSchemas[emplID];

                        //MM 08.09.2017 Miodrag / Izbaceno je da data processing servis gleda dan unazad  
                        //for (DateTime date = DateTimeDict[emplID][firstDate].AddDays(-1); date <= DateTimeDict[emplID][lastDate].AddDays(1); date = date.AddDays(1))
                        for (DateTime date = DateTimeDict[emplID][firstDate]; date <= DateTimeDict[emplID][lastDate].AddDays(1); date = date.AddDays(1))
                        //mm
                        {
                            // Controller.DataProcessingStateChanged("Processing IOPairs - Processing unprocessed io_pairs "+numOfProcessed.ToString()+"/"+unprocessedCount.ToString()); 
                            //do not process if cut off date passed
                            DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            DateTime dateMonth = new DateTime(date.Year, date.Month, 1);

                            if (dateMonth.Date < currMonth.Date.AddMonths(-1))
                            {
                                if (emplDateDict.ContainsKey(emplID) && emplDateDict[emplID].ContainsKey(date))
                                    numOfProcessed += emplDateDict[emplID][date].Count;
                                continue;
                            }

                            if (rulesForEmployee.ContainsKey(Constants.RuleHRSSCCutOffDate))
                            {
                                if (rulesForEmployee[Constants.RuleHRSSCCutOffDate].RuleValue < workingDaysNum)
                                {
                                    if (dateMonth.Date < currMonth.Date)
                                    {
                                        if (emplDateDict.ContainsKey(emplID) && emplDateDict[emplID].ContainsKey(date))
                                            numOfProcessed += emplDateDict[emplID][date].Count;
                                        continue;
                                    }
                                }
                            }

                            Employee empl = new Employee();
                            empl.EmplTO.EmployeeID = employee.EmployeeID;

                            bool is2DayShift = false;
                            bool is2DayShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                            WorkTimeSchemaTO workTimeSchema = null;
                            if (emplID == 23)
                            {
                            }
                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                            //are night shift days. If day is night shift day, also take first interval of next day
                            Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, date, ref is2DayShift,
                                ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, timeSchema);

                            // Sanja 02.10.2013. save those values for flexy third shifts
                            bool nightShift = is2DayShift;
                            bool nightShiftPrevious = is2DayShiftPrevious;
                            WorkTimeIntervalTO nightShiftNextDayInterval = firstIntervalNextDay.Clone();

                            //if (!is2DayShift && !emplDateDict[emplID].ContainsKey(date))
                            //    continue;
                            if (!emplDateDict[emplID].ContainsKey(date))
                                continue;
                            
                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            TimeSpan intervalDuration = new TimeSpan();
                            if (edi != null && workTimeSchema != null)
                            {
                                is2DayShift = false;
                                is2DayShiftPrevious = false;
                                firstIntervalNextDay = null;
                                intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);

                                // Sanja 02.10.2013. dictionary of intervals for specific day for flexy third shifts                                
                                if (!emplDayIntervalsDict.ContainsKey(emplID))
                                    emplDayIntervalsDict.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                                if (!emplDayIntervalsDict[emplID].ContainsKey(date))
                                    emplDayIntervalsDict[emplID].Add(date, intervals);

                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    intervalDuration += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                }
                            }

                            //if employee should work on that day
                            if (intervalDuration.TotalMinutes > 0)
                            {                                
                                List<IOPairTO> outWS = new List<IOPairTO>();
                                outWS.AddRange(emplDateDict[emplID][date]);
                                bool isWorkingDay = false;
                                List<IOPairTO> nextDayPairs = new List<IOPairTO>();
                                List<IOPairTO> prevDayPairs = new List<IOPairTO>();
                                if (emplDateDict[emplID].ContainsKey(date.AddDays(1)))
                                    nextDayPairs = emplDateDict[emplID][date.AddDays(1)];
                                if (emplDateDict[emplID].ContainsKey(date.AddDays(-1)))
                                    prevDayPairs = emplDateDict[emplID][date.AddDays(-1)];

                                foreach (IOPairTO pair in emplDateDict[emplID][date])
                                {
                                    bool isFlexyShift = workTimeSchema.Type.Trim() == Constants.schemaTypeFlexi || workTimeSchema.Type.Trim() == Constants.schemaTypeNightFlexi;

                                    foreach (WorkTimeIntervalTO interval in intervals)
                                    {
                                        DateTime startTime = interval.StartTime;
                                        DateTime endTime = interval.EndTime;

                                        List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>(); ;
                                        if (emplDayIntervalsDict.ContainsKey(emplID) && emplDayIntervalsDict[emplID].ContainsKey(date.Date.AddDays(-1)))
                                            prevDayIntervals = emplDayIntervalsDict[emplID][date.Date.AddDays(-1)];
                                        else if (nightShiftPrevious && interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                        {
                                            // get previous days intervals
                                            prevDayIntervals = Common.Misc.getTimeSchemaInterval(date.Date.AddDays(-1), emplTS, timeSchema);
                                        }

                                        Common.Misc.GetEmployeeShiftStartEnd(emplID, date.Date, interval, prevDayIntervals, nightShiftNextDayInterval,
                                            workTimeSchema, nightShift, nightShiftPrevious, rounding, ref startTime, ref endTime);
                                        
                                        if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                        {
                                            isWorkingDay = true;
                                        }
                                        if (startTime.TimeOfDay <= pair.EndTime.TimeOfDay && endTime.TimeOfDay >= pair.StartTime.TimeOfDay)
                                        {
                                            currentPairProcessing = pair.IOPairID;
                                            IOPairProcessedTO processedTO = new IOPairProcessedTO(pair);

                                            // Sanja 30.09.2013. two rules are added (for Polimark) - num of minutes that employees is obligated to come before/after shift beggining/end
                                            // if IN/OUT is not inside shift beggining/end offset, consider it as delay/early leave
                                            // rule is just for nonflexy shifts
                                            if (!isFlexyShift)
                                            {
                                                if (shiftBegginingOffset > 0 && startTime.TimeOfDay != new TimeSpan(0, 0, 0) && processedTO.StartTime.TimeOfDay <= startTime.TimeOfDay)
                                                {
                                                    // check if pair start is in beggining offset
                                                    DateTime shiftStart = new DateTime(processedTO.StartTime.Year, processedTO.StartTime.Month, processedTO.StartTime.Day, startTime.Hour, startTime.Minute, 0);

                                                    if (processedTO.StartTime > shiftStart.AddMinutes(-shiftBegginingOffset))
                                                        processedTO.StartTime = shiftStart.AddMinutes(1);
                                                }

                                                if (shiftEndingOffset > 0 && endTime.TimeOfDay != new TimeSpan(23, 59, 0) && processedTO.EndTime.TimeOfDay >= endTime.TimeOfDay)
                                                {
                                                    // check if pair end is in ending offset
                                                    DateTime shiftEnd = new DateTime(processedTO.EndTime.Year, processedTO.EndTime.Month, processedTO.EndTime.Day, endTime.Hour, endTime.Minute, 0);

                                                    if (processedTO.EndTime < shiftEnd.AddMinutes(shiftEndingOffset))
                                                        processedTO.EndTime = shiftEnd.AddMinutes(-1);
                                                }
                                            }
                                            
                                            Common.Misc.roundPairTime(processedTO, rounding);

                                            bool presence = true;
                                            if (rulesForEmployee.ContainsKey(Constants.RuleMinPresence))
                                            {
                                                presence = Common.Misc.checkMinPresence(processedTO, nextDayPairs, prevDayPairs, rulesForEmployee[Constants.RuleMinPresence].RuleValue);
                                            }

                                            if (!presence)
                                            {
                                                pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                                ioPair.PairTO = pair;
                                                ioPair.Update(true);
                                                continue;
                                            }
                                            bool isHoliday = false;
                                            // Sanja 26.09.2012. intervals that start in midnight belongs to previous day
                                            DateTime pairDate = date.Date;
                                            if (interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && interval.EndTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                                pairDate = pairDate.AddDays(-1).Date;

                                            if (!Common.Misc.isExpatOut(rulesDictionary, employee) && workTimeSchema.Type.Trim() != Constants.schemaTypeIndustrial && (nationalHolidaysDays.Contains(pairDate.Date)
                                                || (personalHolidayDays.ContainsKey(employeeAsco4.NVarcharValue1) && personalHolidayDays[employeeAsco4.NVarcharValue1].Contains(pairDate.Date))
                                                 || (pairDate.Date.Month == employeeAsco4.DatetimeValue1.Date.Month && pairDate.Date.Day == employeeAsco4.DatetimeValue1.Date.Day)))
                                            {
                                                isHoliday = true;
                                            }

                                            int succ = processIOPair(rulesForEmployee, intervals, pair, processedTO, emplID, date, startTime, endTime, isHoliday, timeSchema);
                                            outWS.Remove(pair);
                                        }
                                    }
                                }
                                foreach (IOPairTO pair in outWS)
                                {
                                    IOPairProcessedTO processedTO = new IOPairProcessedTO(pair);
                                    currentPairProcessing = pair.IOPairID;

                                    //round io pair start time to next 5 min, end time to previous 5 min                               
                                    Common.Misc.roundPairTime(processedTO, rounding);

                                    //count io pair duration
                                    TimeSpan ioPairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;

                                    if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        ioPairDuration = ioPairDuration.Add(new TimeSpan(0, 1, 0));

                                    //if io pair is to short ignore it
                                    if (rulesForEmployee.ContainsKey(Constants.RuleMinPresence))
                                    {
                                        if (ioPairDuration.TotalMinutes < rulesForEmployee[Constants.RuleMinPresence].RuleValue)
                                        {
                                            pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                            ioPair.PairTO = pair;
                                            ioPair.Update(true);
                                            continue;
                                        }
                                    }

                                    if (isWorkingDay)
                                    {
                                        if (!rulesForEmployee.ContainsKey(Constants.RuleOvertimeMinimum)
                                         || ((rulesForEmployee.ContainsKey(Constants.RuleOvertimeMinimum) && rulesForEmployee[Constants.RuleOvertimeMinimum].RuleValue <= ioPairDuration.TotalMinutes)))
                                        {
                                            int typeID = Constants.overtimeUnjustified;
                                            if (rulesForEmployee.ContainsKey(Constants.RuleCompanyInitialOvertime))
                                                typeID = rulesForEmployee[Constants.RuleCompanyInitialOvertime].RuleValue;
                                            if (rulesForEmployee.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                                Common.Misc.roundPairEndTime(processedTO, rulesForEmployee[Constants.RuleOvertimeRounding].RuleValue, true);
                                            // Nenad insert correct pass type 
                                            if (processedTO.PassTypeID != 0)
                                                typeID = processedTO.PassTypeID;
                                            insertProcessed(pair, processedTO, typeID);
                                        }
                                        else
                                        {
                                            pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                            ioPair.PairTO = pair;
                                            ioPair.Update(true);
                                        }
                                    }
                                    else
                                    {
                                        if (!rulesForEmployee.ContainsKey(Constants.RuleMinOvertimeOutWS)
                                            || ((rulesForEmployee.ContainsKey(Constants.RuleMinOvertimeOutWS) && rulesForEmployee[Constants.RuleMinOvertimeOutWS].RuleValue <= ioPairDuration.TotalMinutes)))
                                        {
                                            if (rulesForEmployee.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                                Common.Misc.roundPairEndTime(processedTO, rulesForEmployee[Constants.RuleOvertimeRoundingOutWS].RuleValue, true);
                                            if (processedTO.StartTime != processedTO.EndTime)
                                            {
                                                int typeID = Constants.overtimeUnjustified;
                                                if (rulesForEmployee.ContainsKey(Constants.RuleCompanyInitialOvertime))
                                                    typeID = rulesForEmployee[Constants.RuleCompanyInitialOvertime].RuleValue;

                                                insertProcessed(pair, processedTO, typeID);
                                            }
                                            else
                                            {
                                                pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                                ioPair.PairTO = pair;
                                                ioPair.Update(true);
                                            }
                                        }// if (!rulesForEmployee.ContainsKey(Constants.RuleMinOvertimeOutWS)
                                        // || ((rulesForEmployee.ContainsKey(Constants.RuleMinOvertimeOutWS) && rulesForEmployee[Constants.RuleMinOvertimeOutWS].RuleValue <= ioPairDuration.Minutes)))
                                        else
                                        {
                                            pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                            ioPair.PairTO = pair;
                                            ioPair.Update(true);
                                        }
                                    }
                                }

                                numOfProcessed++;
                            }
                            //if ioPair is on day off calculate extra hours according to the rules
                            else
                            {
                                foreach (IOPairTO pair in emplDateDict[emplID][date])
                                {
                                    currentPairProcessing = pair.IOPairID;

                                    IOPairProcessedTO processedTO = new IOPairProcessedTO(pair);

                                    //round io pair start time to next 5 min, end time to previous 5 min                               
                                    Common.Misc.roundPairTime(processedTO, rounding);

                                    //count io pair duration
                                    TimeSpan ioPairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;

                                    if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        ioPairDuration = ioPairDuration.Add(new TimeSpan(0, 1, 0));

                                    //if io pair is to short ignore it
                                    if (rulesForEmployee.ContainsKey(Constants.RuleMinPresence))
                                    {
                                        if (ioPairDuration.TotalMinutes < rulesForEmployee[Constants.RuleMinPresence].RuleValue)
                                        {
                                            pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                            ioPair.PairTO = pair;
                                            ioPair.Update(true);
                                            continue;
                                        }
                                    }
                                    List<IOPairTO> pairsList = new List<IOPairTO>();
                                    List<IOPairTO> pairsNextDayList = new List<IOPairTO>();
                                    if (emplDateDict[emplID].ContainsKey(date.AddDays(-1)))
                                        pairsList = emplDateDict[emplID][date.AddDays(-1)];
                                    if (emplDateDict[emplID].ContainsKey(date.AddDays(1)))
                                        pairsNextDayList = emplDateDict[emplID][date.AddDays(1)];
                                    if (!rulesForEmployee.ContainsKey(Constants.RuleMinOvertimeOutWS)
                                        || ((rulesForEmployee.ContainsKey(Constants.RuleMinOvertimeOutWS) && rulesForEmployee[Constants.RuleMinOvertimeOutWS].RuleValue <= ioPairDuration.TotalMinutes)))
                                    {
                                        if (rulesForEmployee.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                            Common.Misc.roundPairEndTime(processedTO, rulesForEmployee[Constants.RuleOvertimeRoundingOutWS].RuleValue, pairsList, pairsNextDayList);
                                        if (processedTO.StartTime != processedTO.EndTime)
                                        {
                                            int typeID = Constants.overtimeUnjustified;
                                            if (rulesForEmployee.ContainsKey(Constants.RuleCompanyInitialOvertime))
                                                typeID = rulesForEmployee[Constants.RuleCompanyInitialOvertime].RuleValue;
                                            // tamara 17.7.2018.
                                            // Nenad insert correct pass type 
                                            if (processedTO.PassTypeID != 0)
                                                typeID = processedTO.PassTypeID;
                                            insertProcessed(pair, processedTO, typeID);
                                        }
                                        else
                                        {
                                            pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                            ioPair.PairTO = pair;
                                            ioPair.Update(true);
                                        }
                                    }// if (!rulesForEmployee.ContainsKey(Constants.RuleMinOvertimeOutWS)
                                    // || ((rulesForEmployee.ContainsKey(Constants.RuleMinOvertimeOutWS) && rulesForEmployee[Constants.RuleMinOvertimeOutWS].RuleValue <= ioPairDuration.Minutes)))
                                    else
                                    {
                                        pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                        ioPair.PairTO = pair;
                                        ioPair.Update(true);
                                    }
                                }//foreach (IOPairTO pair in emplDateDict[emplID][date])

                                //UBACEN DEO ZBOG SMP CUPRIJE, JER SU ISLI NA PAUZE NA ISTIM KONTROLERIMA KOJI SU ZA ERV I ZBOG TOGA SU OSTAJALE RUPE U PAROVIMA
                                if (emplDateDict[emplID][date].Count > 1 && ConfigurationManager.AppSettings["Company"].ToLower() == "smp")
                                {
                                    IOPairProcessed iopp = new IOPairProcessed();
                                    List<IOPairProcessedTO> parovi = iopp.pairsForPeriod(emplID.ToString(), date, date);
                                    if (parovi.Count > 1)
                                    {
                                        foreach (IOPairProcessedTO ioppTO in parovi)
                                        {
                                            if (parovi.IndexOf(ioppTO) == parovi.Count - 1)
                                            {
                                                continue;
                                            }
                                            DateTime startTime = ioppTO.EndTime;
                                            DateTime endTime = parovi[parovi.IndexOf(ioppTO) + 1].StartTime;
                                            TimeSpan razlika = endTime - startTime;
                                            if (startTime.Date == endTime.Date && razlika.TotalMinutes <= 45)
                                            {
                                                IOPairProcessedTO ioppNew = new IOPairProcessedTO();
                                                int typeID = Constants.overtimeUnjustified;
                                                if (rulesForEmployee.ContainsKey(Constants.RuleCompanyInitialOvertime))
                                                    typeID = rulesForEmployee[Constants.RuleCompanyInitialOvertime].RuleValue;
                                                ioppNew.StartTime = startTime;
                                                ioppNew.EndTime = endTime;
                                                ioppNew.EmployeeID = emplID;
                                                ioppNew.CreatedTime = DateTime.Now;
                                                ioppNew.ConfirmationFlag = ioppNew.ManualCreated = ioppNew.VerificationFlag = 0;
                                                ioppNew.Alert = "0";
                                                ioppNew.LocationID = ioppTO.LocationID;
                                                ioppNew.IsWrkHrsCounter = 1;
                                                ioppNew.IOPairDate = ioppTO.IOPairDate;
                                                ioppNew.CreatedBy = ioppTO.CreatedBy;
                                                ioppNew.IOPairID = 0;
                                                insertProcessed(null, ioppNew, typeID);
                                            }
                                            else if (startTime.Date.AddDays(1) == endTime.Date)
                                            {
                                                DateTime dt1 = new DateTime(startTime.Year, startTime.Month, startTime.Day, 23, 59, 0);
                                                TimeSpan razlika_dt1_startTime = dt1 - startTime;
                                                if (razlika_dt1_startTime.TotalMinutes > 0 && razlika_dt1_startTime.TotalMinutes <= 45)
                                                {
                                                    IOPairProcessedTO ioppNew = new IOPairProcessedTO();
                                                    int typeID = Constants.overtimeUnjustified;
                                                    if (rulesForEmployee.ContainsKey(Constants.RuleCompanyInitialOvertime))
                                                        typeID = rulesForEmployee[Constants.RuleCompanyInitialOvertime].RuleValue;
                                                    ioppNew.StartTime = startTime;
                                                    ioppNew.EndTime = dt1;
                                                    ioppNew.EmployeeID = emplID;
                                                    ioppNew.CreatedTime = DateTime.Now;
                                                    ioppNew.ConfirmationFlag = ioppNew.ManualCreated = ioppNew.VerificationFlag = 0;
                                                    ioppNew.Alert = "0";
                                                    ioppNew.LocationID = ioppTO.LocationID;
                                                    ioppNew.IsWrkHrsCounter = 1;
                                                    ioppNew.IOPairDate = ioppTO.IOPairDate;
                                                    ioppNew.CreatedBy = ioppTO.CreatedBy;
                                                    ioppNew.IOPairID = 0;
                                                    insertProcessed(null, ioppNew, typeID);
                                                }
                                            }
                                            else if (startTime.Hour < 1)
                                            {
                                                DateTime startTime1 = new DateTime(startTime.Year, startTime.Month, startTime.Day, 0, 0, 0);
                                                DateTime endTime1 = startTime;
                                                IOPairProcessedTO ioppNew = new IOPairProcessedTO();
                                                int typeID = Constants.overtimeUnjustified;
                                                if (rulesForEmployee.ContainsKey(Constants.RuleCompanyInitialOvertime))
                                                    typeID = rulesForEmployee[Constants.RuleCompanyInitialOvertime].RuleValue;
                                                ioppNew.StartTime = startTime1;
                                                ioppNew.EndTime = endTime1;
                                                ioppNew.EmployeeID = emplID;
                                                ioppNew.CreatedTime = DateTime.Now;
                                                ioppNew.ConfirmationFlag = ioppNew.ManualCreated = ioppNew.VerificationFlag = 0;
                                                ioppNew.Alert = "0";
                                                ioppNew.LocationID = ioppTO.LocationID;
                                                ioppNew.IsWrkHrsCounter = 1;
                                                ioppNew.IOPairDate = ioppTO.IOPairDate;
                                                ioppNew.CreatedBy = ioppTO.CreatedBy;
                                                ioppNew.IOPairID = 0;
                                                insertProcessed(null, ioppNew, typeID);
                                            }
                                        }
                                    }
                                }

                            }//if (edi == null) 
                        }//foreach (DateTime date in emplDateDict[emplID].Keys)
                    }
                    catch (Exception ex)
                    {
                        debug.writeLog(DateTime.Now + " Exception in: " +
                            this.ToString() + ".processingUnprocessedIOPairs(): Message: " + ex.Message + "/nStackTrace:" + ex.StackTrace + "\n Exeption throwen for io pair id = " + currentPairProcessing);
                    }
                }//foreach (int emplID in emplDateDict.Keys)
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".processingUnprocessedIOPairs(): Message: " + ex.Message + "/nStackTrace:" + ex.StackTrace + "\n Exeption throwen for io pair id = " + currentPairProcessing);
            }
        }

        private bool isNextDayIntervalPairAbsence(List<IOPairProcessedTO> nextDayPairs, WorkTimeIntervalTO nextDayInterval)
        {
            bool isAbsence = true;

            try
            {
                foreach (IOPairProcessedTO pair in nextDayPairs)
                {
                    if (pair.StartTime.TimeOfDay >= nextDayInterval.EndTime.TimeOfDay)
                        break;

                    if (pair.PassTypeID != Constants.absence)
                    {
                        isAbsence = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".isNextDayIntervalPairAbsence(): Message: " + ex.Message + "/nStackTrace:" + ex.StackTrace + "\n");
            }

            return isAbsence;
        }

        private void processHolesForDayAndEmployee(int emplID, DateTime date, List<IOPairProcessedTO> dayPairs, List<IOPairProcessedTO> nextDayPairs, WorkTimeIntervalTO nextDayInterval, 
            Dictionary<string, RuleTO> rulesForEmployee, DateTime startTime, DateTime endTime, int holesType, bool byLocation)
        {
            try
            {
                DateTime beginOfInterval = startTime;
                IOPairProcessedTO lastProcPair = new IOPairProcessedTO();
                DateTime startCopy = new DateTime();
                DateTime endCopy = new DateTime();
                int holesTypeCopy = holesType;
                startCopy = startTime;
                endCopy = endTime;
                //for first day of night shift without passes insert delay and hole
                if (dayPairs.Count == 0)
                {
                    if (endTime.TimeOfDay == new TimeSpan(23, 59, 0) && holesType == Constants.absence && !isNextDayIntervalPairAbsence(nextDayPairs, nextDayInterval))
                    {
                        DateTime delayEnd = endTime;
                        IOPairProcessedTO processedTO = new IOPairProcessedTO();
                        processedTO.StartTime = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second);
                        if (extraOrdinaryEmpl.ContainsKey(emplID))
                            delayEnd = processedTO.StartTime;
                        else if (rulesForEmployee.ContainsKey(Constants.RuleDelayMax) && rulesForEmployee[Constants.RuleDelayMax].RuleValue < (delayEnd.TimeOfDay.TotalMinutes - startTime.TimeOfDay.TotalMinutes))
                            delayEnd = processedTO.StartTime.AddMinutes(rulesForEmployee[Constants.RuleDelayMax].RuleValue);
                        processedTO.EndTime = new DateTime(date.Year, date.Month, date.Day, delayEnd.Hour, delayEnd.Minute, delayEnd.Second);
                        processedTO.IOPairDate = date.Date;
                        processedTO.IOPairID = Constants.unjustifiedIOPairID;
                        processedTO.EmployeeID = emplID;
                        processedTO.LocationID = Constants.locationOut;
                        processedTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;

                        if (!extraOrdinaryEmpl.ContainsKey(emplID) && rulesForEmployee.ContainsKey(Constants.RuleDelayRounding))
                            Common.Misc.roundPairEndTime(processedTO, rulesForEmployee[Constants.RuleDelayRounding].RuleValue, false);
                        int delay = Constants.delay;
                        if (rulesForEmployee.ContainsKey(Constants.RuleCompanyDelay))
                            delay = rulesForEmployee[Constants.RuleCompanyDelay].RuleValue;
                        if (endTime.TimeOfDay.TotalMinutes > processedTO.EndTime.TimeOfDay.TotalMinutes)
                        {
                            IOPairProcessedTO processedUnjTO = new IOPairProcessedTO();
                            processedUnjTO.StartTime = processedTO.EndTime;
                            processedUnjTO.EndTime = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, endTime.Second);
                            processedUnjTO.IOPairDate = date.Date;
                            processedUnjTO.IOPairID = Constants.unjustifiedIOPairID;
                            processedUnjTO.EmployeeID = emplID;
                            processedUnjTO.LocationID = Constants.locationOut;
                            processedUnjTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                            // tamara 17.7.2018.
                            // Nenad insert correct pass type 
                            //if (processedUnjTO.PassTypeID != 0)
                            //    insertProcessed(null, processedUnjTO, processedUnjTO.PassTypeID);
                            //else
                            insertProcessed(null, processedUnjTO, holesType);
                        }
                        //for night shift next day interval do not insert delay
                        if (processedTO.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            insertProcessed(null, processedTO, delay);
                    }
                    else
                    {
                        // insert whole interval absence (or holiday)
                        IOPairProcessedTO processedTO = new IOPairProcessedTO();
                        processedTO.StartTime = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second);
                        processedTO.EndTime = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, endTime.Second);
                        processedTO.IOPairDate = date.Date;
                        processedTO.IOPairID = Constants.unjustifiedIOPairID;
                        processedTO.EmployeeID = emplID;
                        processedTO.LocationID = Constants.locationOut;
                        processedTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                        //tamara 17.7.2018.
                        // Nenad insert correct pass type 
                        //if (processedTO.PassTypeID != 0)
                        //    insertProcessed(null, processedTO, processedTO.PassTypeID);
                        //else
                            insertProcessed(null, processedTO, holesType);
                    }
                }

                //process each pair in list
                for (int i = 0; i < dayPairs.Count; i++)
                {
                    IOPairProcessedTO procPair = dayPairs[i];
                    if (procPair.EndTime.TimeOfDay <= startTime.TimeOfDay || procPair.StartTime.TimeOfDay >= endTime.TimeOfDay)
                    {
                        //if last processed pair is before begining of the shift insert hole for that day
                        if (i == dayPairs.Count - 1 && procPair.EndTime.TimeOfDay <= startTime.TimeOfDay)
                        {
                            // if interval is begining of third shift and there is some nonabsence pair in next day interval, insert delay
                            if (endTime.TimeOfDay == new TimeSpan(23, 59, 0) && holesType == Constants.absence && !isNextDayIntervalPairAbsence(nextDayPairs, nextDayInterval))
                            {
                                DateTime delayEnd = endTime;
                                IOPairProcessedTO processedTO = new IOPairProcessedTO();
                                processedTO.StartTime = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second);
                                if (extraOrdinaryEmpl.ContainsKey(emplID))
                                    delayEnd = processedTO.StartTime;
                                else if (rulesForEmployee.ContainsKey(Constants.RuleDelayMax) && rulesForEmployee[Constants.RuleDelayMax].RuleValue < (delayEnd.TimeOfDay.TotalMinutes - startTime.TimeOfDay.TotalMinutes))
                                    delayEnd = processedTO.StartTime.AddMinutes(rulesForEmployee[Constants.RuleDelayMax].RuleValue);
                                processedTO.EndTime = new DateTime(date.Year, date.Month, date.Day, delayEnd.Hour, delayEnd.Minute, delayEnd.Second);
                                processedTO.IOPairDate = date.Date;
                                processedTO.IOPairID = Constants.unjustifiedIOPairID;
                                processedTO.EmployeeID = emplID;
                                processedTO.LocationID = Constants.locationOut;
                                processedTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;

                                if (!extraOrdinaryEmpl.ContainsKey(emplID) && rulesForEmployee.ContainsKey(Constants.RuleDelayRounding))
                                    Common.Misc.roundPairEndTime(processedTO, rulesForEmployee[Constants.RuleDelayRounding].RuleValue, false);
                                int delay = Constants.delay;
                                if (rulesForEmployee.ContainsKey(Constants.RuleCompanyDelay))
                                    delay = rulesForEmployee[Constants.RuleCompanyDelay].RuleValue;
                                if (endTime.TimeOfDay.TotalMinutes > processedTO.EndTime.TimeOfDay.TotalMinutes)
                                {
                                    IOPairProcessedTO processedUnjTO = new IOPairProcessedTO();
                                    processedUnjTO.StartTime = processedTO.EndTime;
                                    processedUnjTO.EndTime = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, endTime.Second);
                                    processedUnjTO.IOPairDate = date.Date;
                                    processedUnjTO.IOPairID = Constants.unjustifiedIOPairID;
                                    processedUnjTO.EmployeeID = emplID;
                                    processedUnjTO.LocationID = Constants.locationOut;
                                    processedUnjTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                                    // tamara 17.7.2018.
                                    // Nenad insert correct pass type 
                                    //if (processedUnjTO.PassTypeID != 0)
                                    //    insertProcessed(null, processedUnjTO, processedUnjTO.PassTypeID);
                                    //else
                                    insertProcessed(null, processedUnjTO, holesType);
                                }
                                // Nenad insert correct pass type 
                                //if (processedTO.PassTypeID != 0)
                                //    insertProcessed(null, processedTO, processedTO.PassTypeID);
                                //else
                                insertProcessed(null, processedTO, delay);
                            }
                            else
                            {
                                // insert whole interval absence (or holiday)
                                IOPairProcessedTO processedTO = new IOPairProcessedTO();
                                processedTO.StartTime = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second);
                                processedTO.EndTime = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, endTime.Second);
                                processedTO.IOPairDate = date.Date;
                                processedTO.IOPairID = Constants.unjustifiedIOPairID;
                                processedTO.EmployeeID = emplID;
                                processedTO.LocationID = Constants.locationOut;
                                processedTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                                // tamara 17.7.2018.
                                // Nenad insert correct pass type 
                                //if (processedTO.PassTypeID != 0)
                                //    insertProcessed(null, processedTO, processedTO.PassTypeID);
                                //else
                                insertProcessed(null, processedTO, holesType);
                            }
                        }
                        //if first processed pair is after end of the shift insert hole for that day
                        else if (i == 0 && procPair.StartTime.TimeOfDay >= endTime.TimeOfDay)
                        {
                            IOPairProcessedTO processedUnjTO = new IOPairProcessedTO();
                            processedUnjTO.StartTime = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second);
                            processedUnjTO.EndTime = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, endTime.Second);
                            processedUnjTO.IOPairDate = date.Date;
                            processedUnjTO.IOPairID = Constants.unjustifiedIOPairID;
                            processedUnjTO.EmployeeID = emplID;
                            processedUnjTO.LocationID = Constants.locationOut;
                            processedUnjTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                            // tamara 17.7.2018.
                            // Nenad insert correct pass type 
                            //if (processedUnjTO.PassTypeID != 0)
                            //    insertProcessed(null, processedUnjTO, processedUnjTO.PassTypeID);
                            //else
                            insertProcessed(null, processedUnjTO, holesType);
                        }

                        continue;
                    }
                    if (procPair.StartTime.TimeOfDay <= startTime.TimeOfDay && procPair.EndTime.TimeOfDay < endTime.TimeOfDay)
                    {
                        startTime = procPair.EndTime;
                        lastProcPair = new IOPairProcessedTO(procPair);
                    }
                    else if (procPair.StartTime.TimeOfDay >= startTime.TimeOfDay)
                    {
                        // if begin of interval and pair proccessed start time is after interval begin calculate delay
                        // do not calculate delay if it is holiday
                        bool locationNightShiftBeginningHole = byLocation && startTime.TimeOfDay == new TimeSpan(0, 0, 0);
                        if (startTime.TimeOfDay == beginOfInterval.TimeOfDay && (holesType == Constants.absence || locationNightShiftBeginningHole))
                        {
                            DateTime delayEnd = procPair.StartTime;
                            IOPairProcessedTO processedTO = new IOPairProcessedTO();
                            processedTO.StartTime = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second);

                            if (extraOrdinaryEmpl.ContainsKey(emplID))
                                delayEnd = processedTO.StartTime;
                            else if (rulesForEmployee.ContainsKey(Constants.RuleDelayMax) && rulesForEmployee[Constants.RuleDelayMax].RuleValue < (delayEnd.TimeOfDay.TotalMinutes - startTime.TimeOfDay.TotalMinutes))
                                delayEnd = processedTO.StartTime.AddMinutes(rulesForEmployee[Constants.RuleDelayMax].RuleValue);
                            processedTO.EndTime = new DateTime(date.Year, date.Month, date.Day, delayEnd.Hour, delayEnd.Minute, delayEnd.Second);
                            processedTO.IOPairDate = date.Date;
                            processedTO.IOPairID = Constants.unjustifiedIOPairID;
                            processedTO.EmployeeID = emplID;
                            processedTO.LocationID = Constants.locationOut;
                            processedTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;

                            if (!extraOrdinaryEmpl.ContainsKey(emplID) && rulesForEmployee.ContainsKey(Constants.RuleDelayRounding))
                                Common.Misc.roundPairEndTime(processedTO, rulesForEmployee[Constants.RuleDelayRounding].RuleValue, false);
                            int delay = Constants.delay;
                            if (rulesForEmployee.ContainsKey(Constants.RuleCompanyDelay))
                                delay = rulesForEmployee[Constants.RuleCompanyDelay].RuleValue;
                            //for night shift next day interval do not insert delay
                            if (processedTO.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                processedTO.EndTime = processedTO.StartTime;
                            //when inserting round delay if employee arrived few minutes earlier update io_pair_processed to new start time
                            if (procPair.StartTime.TimeOfDay.TotalMinutes < processedTO.EndTime.TimeOfDay.TotalMinutes)
                            {
                                procPair.StartTime = processedTO.EndTime;
                                IOPairProcessed proc = new IOPairProcessed();
                                proc.IOPairProcessedTO = procPair;
                                if (insertedPairs.ContainsKey(emplID) && insertedPairs[emplID].ContainsKey(procPair.IOPairDate.Date)) 
                                {
                                    foreach (IOPairProcessedTO ioPairSaved in insertedPairs[emplID][procPair.IOPairDate.Date])
                                    {
                                        if (ioPairSaved.EndTime == procPair.EndTime)
                                        {
                                            ioPairSaved.StartTime = procPair.StartTime;
                                            break;
                                        }
                                    }
                                }
                                proc.Update(true);
                            }
                            //when inserting round delay if employee arrived later insert hole from the end of the delay to the begin of processed pair
                            else if (procPair.StartTime.TimeOfDay.TotalMinutes > processedTO.EndTime.TimeOfDay.TotalMinutes)
                            {
                                //bool createAbsencePair = true;
                                //// if system is set to process by locations and hole is around midnight in night shift and is less then maximal hole duration allowed, 
                                //// and is between two automaticaly created regular works, prolong regular work pairs from night shift beggining and end to midnight,
                                //// instead of making hole pair
                                //if (startTime.TimeOfDay == beginOfInterval.TimeOfDay && startTime.TimeOfDay == new TimeSpan(0, 0, 0) && endTime.TimeOfDay != new TimeSpan(0, 0, 0)
                                //        && byLocation && rulesForEmployee.ContainsKey(Constants.RuleCompanyRegularWork)
                                //        && procPair.StartTime == end && procPair.PassTypeID == rulesForEmployee[Constants.RuleCompanyRegularWork].RuleValue
                                //        && procPair.ManualCreated == (int)Constants.recordCreated.Automaticaly)
                                //{
                                //    int maxAbsence = 1000;
                                //    if (rulesForEmployee.ContainsKey(Constants.RuleMaxLocationAbsence))
                                //        maxAbsence = rulesForEmployee[Constants.RuleMaxLocationAbsence].RuleValue;

                                //    int absenceDuration = (int)end.Subtract(start).TotalMinutes;
                                //    if (end.TimeOfDay == new TimeSpan(23, 59, 0))
                                //        absenceDuration++;

                                //    if (absenceDuration <= maxAbsence)
                                //    {
                                //        lastProcPair.EndTime = end;
                                //        lastProcPair.ModifiedBy = Constants.dataProcessingUser.Trim();
                                //        IOPairProcessed proc = new IOPairProcessed();
                                //        proc.IOPairProcessedTO = lastProcPair;
                                //        proc.Update(true);
                                //        createAbsence = false;
                                //    }
                                //}
                                IOPairProcessedTO processedUnjTO = new IOPairProcessedTO();
                                processedUnjTO.StartTime = processedTO.EndTime;
                                processedUnjTO.EndTime = new DateTime(date.Year, date.Month, date.Day, procPair.StartTime.Hour, procPair.StartTime.Minute, procPair.StartTime.Second);
                                processedUnjTO.IOPairDate = date.Date;
                                processedUnjTO.IOPairID = Constants.unjustifiedIOPairID;
                                processedUnjTO.EmployeeID = emplID;
                                processedUnjTO.LocationID = Constants.locationOut;
                                processedUnjTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                                // tamara 17.7.2018.
                                // Nenad insert correct pass type 
                                //if (processedUnjTO.PassTypeID != 0)
                                //    insertProcessed(null, processedUnjTO, processedUnjTO.PassTypeID);
                                //else
                                if (ConfigurationManager.AppSettings["Company"].ToLower().Contains("smp"))
                                {
                                    if ((processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay < new TimeSpan(0, 30, 0)) && ((processedTO.StartTime.TimeOfDay > startCopy.TimeOfDay && processedTO.EndTime.TimeOfDay < endCopy.TimeOfDay) || processedTO.StartTime.TimeOfDay == new TimeSpan(0, 0, 0)))
                                    {
                                        if ((processedTO.StartTime.Hour >= 0 && processedTO.EndTime.Hour < 6) || (processedTO.StartTime.Hour >= 22 && processedTO.EndTime.Hour <= 23))
                                        {
                                            holesType = rulesForEmployee[Constants.RuleNightWork].RuleValue;
                                        }
                                        else
                                        {
                                            holesType = rulesForEmployee[Constants.RuleCompanyRegularWork].RuleValue;
                                        }
                                        if (holesTypeCopy == 10)
                                        {
                                            holesType = rulesForEmployee[Constants.RuleWorkOnHolidayPassType].RuleValue;
                                            if ((processedTO.StartTime.Hour >= 0 && processedTO.EndTime.Hour < 6) || (processedTO.StartTime.Hour >= 22 && processedTO.EndTime.Hour <= 23))
                                            {
                                                holesType = rulesForEmployee[Constants.RuleHolidayPlusNightWork].RuleValue;
                                            }
                                        }
                                    } 
                                }
                                insertProcessed(null, processedUnjTO, holesType);
                                holesType = holesTypeCopy;
                            }
                            //for night shift next day interval do not insert delay
                            if (processedTO.StartTime.TimeOfDay != new TimeSpan(0, 0, 0) && (processedTO.StartTime != processedTO.EndTime))
                                insertProcessed(null, processedTO, delay);
                        }
                        //if is not begining of the shift insert holes 
                        else
                        {
                            DateTime start = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second);
                            DateTime end = new DateTime(date.Year, date.Month, date.Day, procPair.StartTime.Hour, procPair.StartTime.Minute, procPair.StartTime.Second);

                            bool createAbsence = true;                            
                            // if system is set to process by locations and hole is between two automaticaly created regular works, and is less then maximal hole duration allowed, 
                            // prolong regular work pair, instead of making hole pair
                            if (byLocation && rulesForEmployee.ContainsKey(Constants.RuleCompanyRegularWork)
                                && lastProcPair.EndTime == start && lastProcPair.PassTypeID == rulesForEmployee[Constants.RuleCompanyRegularWork].RuleValue
                                && lastProcPair.ManualCreated == (int)Constants.recordCreated.Automaticaly
                                && procPair.StartTime == end && procPair.PassTypeID == rulesForEmployee[Constants.RuleCompanyRegularWork].RuleValue
                                && procPair.ManualCreated == (int)Constants.recordCreated.Automaticaly)
                            {
                                int maxAbsence = 1000;
                                if (rulesForEmployee.ContainsKey(Constants.RuleMaxLocationAbsence))
                                    maxAbsence = rulesForEmployee[Constants.RuleMaxLocationAbsence].RuleValue;

                                int absenceDuration = (int)end.Subtract(start).TotalMinutes;
                                if (end.TimeOfDay == new TimeSpan(23, 59, 0))
                                    absenceDuration++;

                                if (absenceDuration <= maxAbsence)
                                {
                                    lastProcPair.EndTime = end;
                                    lastProcPair.ModifiedBy = Constants.dataProcessingUser.Trim();
                                    IOPairProcessed proc = new IOPairProcessed();
                                    proc.IOPairProcessedTO = lastProcPair;
                                    proc.Update(true);
                                    createAbsence = false;
                                }
                            }

                            if (createAbsence)
                            {
                                IOPairProcessedTO processedTO = new IOPairProcessedTO();
                                processedTO.StartTime = start;
                                processedTO.EndTime = end;
                                processedTO.IOPairDate = date.Date;
                                processedTO.EmployeeID = emplID;
                                processedTO.IOPairID = Constants.unjustifiedIOPairID;
                                processedTO.LocationID = Constants.locationOut;
                                processedTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                                // tamara 17.7.2018.
                                // Nenad insert correct pass type 
                                //if (processedTO.PassTypeID != 0)
                                //    insertProcessed(null, processedTO, processedTO.PassTypeID);
                                //else

                                //TESTIRANJE ZA UNOS RUPA U RADNOM VREMENU NAKON PAUZE --- VIKTOR 07.06.2024.
                                if (ConfigurationManager.AppSettings["Company"].ToLower().Contains("smp"))
                                {
                                    if ((processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay < new TimeSpan(0, 30, 0)) && ((processedTO.StartTime.TimeOfDay > startCopy.TimeOfDay && processedTO.EndTime.TimeOfDay < endCopy.TimeOfDay) || processedTO.StartTime.TimeOfDay == new TimeSpan(0, 0, 0)))
                                    {
                                        if ((processedTO.StartTime.Hour >= 0 && processedTO.EndTime.Hour < 6) || (processedTO.StartTime.Hour >= 22 && processedTO.EndTime.Hour <= 23))
                                        {
                                            holesType = rulesForEmployee[Constants.RuleNightWork].RuleValue;
                                        }
                                        else
                                        {
                                            holesType = rulesForEmployee[Constants.RuleCompanyRegularWork].RuleValue;
                                        }
                                        if (holesTypeCopy == 10)
                                        {
                                            holesType = rulesForEmployee[Constants.RuleWorkOnHolidayPassType].RuleValue;
                                            if ((processedTO.StartTime.Hour >= 0 && processedTO.EndTime.Hour < 6) || (processedTO.StartTime.Hour >= 22 && processedTO.EndTime.Hour <= 23))
                                            {
                                                holesType = rulesForEmployee[Constants.RuleHolidayPlusNightWork].RuleValue;
                                            }
                                        }
                                    } 
                                }

                                insertProcessed(null, processedTO, holesType);
                                holesType = holesTypeCopy;
                            }
                        }

                        // set start time is end of processed pair 
                        startTime = procPair.EndTime;
                        lastProcPair = new IOPairProcessedTO(procPair);
                    }
                    //if end time is before end of the shift and it is last processed pair or next pair begin after interval end insert hole from io_pair_processed end time to the end of the shift
                    if (procPair.EndTime.TimeOfDay < endTime.TimeOfDay && (i == dayPairs.Count - 1 || dayPairs[i + 1].StartTime.TimeOfDay >= endTime.TimeOfDay))
                    {
                        DateTime start = new DateTime(date.Year, date.Month, date.Day, procPair.EndTime.Hour, procPair.EndTime.Minute, procPair.EndTime.Second);
                        DateTime end = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, endTime.Second);

                        bool createAbsence = true;
                        // if system is set to process by locations and hole is between two automaticaly created regular works, and is less then maximal hole duration allowed, 
                        // prolong regular work pair, instead of making hole pair
                        if (byLocation && rulesForEmployee.ContainsKey(Constants.RuleCompanyRegularWork)
                                && lastProcPair.EndTime == start && lastProcPair.PassTypeID == rulesForEmployee[Constants.RuleCompanyRegularWork].RuleValue
                                && lastProcPair.ManualCreated == (int)Constants.recordCreated.Automaticaly
                                && procPair.StartTime == end && procPair.PassTypeID == rulesForEmployee[Constants.RuleCompanyRegularWork].RuleValue
                                && procPair.ManualCreated == (int)Constants.recordCreated.Automaticaly)
                        {
                            int maxAbsence = 1000;
                            if (rulesForEmployee.ContainsKey(Constants.RuleMaxLocationAbsence))
                                maxAbsence = rulesForEmployee[Constants.RuleMaxLocationAbsence].RuleValue;

                            int absenceDuration = (int)end.Subtract(start).TotalMinutes; 
                            if (end.TimeOfDay == new TimeSpan(23, 59, 0))
                                absenceDuration++;

                            if (absenceDuration <= maxAbsence)
                            {
                                lastProcPair.EndTime = end;
                                lastProcPair.ModifiedBy = Constants.dataProcessingUser.Trim();
                                IOPairProcessed proc = new IOPairProcessed();
                                proc.IOPairProcessedTO = lastProcPair;
                                proc.Update(true);
                                createAbsence = false;
                            }
                        }

                        if (createAbsence)
                        {
                            IOPairProcessedTO processedTO = new IOPairProcessedTO();
                            processedTO.StartTime = start;
                            processedTO.EndTime = end;
                            processedTO.IOPairDate = date.Date;
                            processedTO.EmployeeID = emplID;
                            processedTO.IOPairID = Constants.unjustifiedIOPairID;
                            processedTO.LocationID = Constants.locationOut;
                            processedTO.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                            // tamara 17.7.2018.
                            // Nenad insert correct pass type 
                            //if (processedTO.PassTypeID != 0)
                            //    insertProcessed(null, processedTO, processedTO.PassTypeID);
                            //else
                            insertProcessed(null, processedTO, holesType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".processHolesForDayAndEmployee() - Message: " + ex.Message+"; StackTrace: "+ex.StackTrace + "\n");
            }
        }

        private const int returnContinue = 3;

        private int processIOPair(Dictionary<string, RuleTO> rulesForEmployee, List<WorkTimeIntervalTO> intervals, IOPairTO pair, IOPairProcessedTO processedTO, int emplID, DateTime date, 
            DateTime startTime, DateTime endTime, bool isHoliday, Dictionary<int, WorkTimeSchemaTO> schDict)
        {
            int processedValue = 0;
            try
            {
                if (pair.EmployeeID == 148 && pair.IOPairDate == new DateTime(2024, 11, 11))
                {
                }
                IOPair ioPair = new IOPair();
                TimeSpan ioPairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;

                if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    ioPairDuration = ioPairDuration.Add(new TimeSpan(0, 1, 0));

                if (isHoliday)
                    processedTO.VerificationFlag = (int)Constants.Verification.NotVerified;
                //if whole pair is inside the shift insert regular work
                if (processedTO.StartTime.TimeOfDay >= startTime.TimeOfDay && processedTO.EndTime.TimeOfDay <= endTime.TimeOfDay)
                {
                    int typeID = Constants.regularWork;
                    if (rulesForEmployee.ContainsKey(Constants.RuleCompanyRegularWork))
                        typeID = rulesForEmployee[Constants.RuleCompanyRegularWork].RuleValue;
                    //TESTIRATI, jer nakon Pauze u nocnoj smeni nije postavljao na Nocni rad vec na Redovni -- naredne dve linije koda TESTIRATI
                    if (processedTO.StartTime.TimeOfDay >= new TimeSpan(22, 00, 0) || processedTO.EndTime.TimeOfDay <= new TimeSpan(6, 0, 0))
                        typeID = rulesForEmployee[Constants.RuleNightWork].RuleValue;
                    if (isHoliday)
                        typeID = rulesForEmployee[Constants.RuleWorkOnHolidayPassType].RuleValue;
                    if (isHoliday && (processedTO.StartTime.TimeOfDay >= new TimeSpan(22, 00, 0) || processedTO.EndTime.TimeOfDay <= new TimeSpan(6, 0, 0)))
                        typeID = rulesForEmployee[Constants.RuleHolidayPlusNightWork].RuleValue;
                    //if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0) || processedTO.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                    //    typeID = rulesForEmployee[Constants.RuleNightWork].RuleValue;
                    // tamara 17.7.2018.
                    // Nenad insert correct pass type 
                    //if (processedTO.PassTypeID != 0)
                    //    insertProcessed(null, processedTO, processedTO.PassTypeID);
                    //else
                    insertProcessed(pair, processedTO, typeID);
                }
                else
                {
                    if (processedTO.StartTime.TimeOfDay < startTime.TimeOfDay && processedTO.EndTime.TimeOfDay > startTime.TimeOfDay)
                    {
                        IOPairProcessedTO ioPairBeforeShift = new IOPairProcessedTO(processedTO);
                        ioPairBeforeShift.EndTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
                        processedValue += processIOPair(rulesForEmployee, intervals, pair, ioPairBeforeShift, emplID, date, startTime, endTime, isHoliday, schDict);
                        processedTO.StartTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, startTime.Hour, startTime.Minute, startTime.Second); ;
                        processedValue += processIOPair(rulesForEmployee, intervals, pair, processedTO, emplID, date, startTime, endTime, isHoliday, schDict);
                    }
                    else if (processedTO.StartTime.TimeOfDay < endTime.TimeOfDay && processedTO.EndTime.TimeOfDay > endTime.TimeOfDay)
                    {
                        IOPairProcessedTO ioPairAfterShift = new IOPairProcessedTO(processedTO);
                        ioPairAfterShift.StartTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
                        processedValue += processIOPair(rulesForEmployee, intervals, pair, ioPairAfterShift, emplID, date, startTime, endTime, isHoliday, schDict);
                        processedTO.EndTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
                        processedValue += processIOPair(rulesForEmployee, intervals, pair, processedTO, emplID, date, startTime, endTime, isHoliday, schDict);
                    }
                    else if (processedTO.EndTime.TimeOfDay == startTime.TimeOfDay || processedTO.StartTime.TimeOfDay == endTime.TimeOfDay)
                    {
                        if (processedTO.EndTime.TimeOfDay == startTime.TimeOfDay)
                        {
                            if (!rulesForEmployee.ContainsKey(Constants.RuleOvertimeShiftStart)
                                || ((rulesForEmployee.ContainsKey(Constants.RuleOvertimeShiftStart) && rulesForEmployee[Constants.RuleOvertimeShiftStart].RuleValue <= ioPairDuration.TotalMinutes)))
                            {
                                int typeID = Constants.overtimeUnjustified;
                                if (rulesForEmployee.ContainsKey(Constants.RuleCompanyInitialOvertime))
                                    typeID = rulesForEmployee[Constants.RuleCompanyInitialOvertime].RuleValue;

                                if (rulesForEmployee.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                    Common.Misc.roundPairStartTime(processedTO, rulesForEmployee[Constants.RuleOvertimeRounding].RuleValue, true);
                                // tamara 17.7.2018. 
                                // Nenad insert correct pass type 
                                if (processedTO.PassTypeID != 0)
                                    typeID = processedTO.PassTypeID;
                                
                                
                                if (insertProcessed(pair, processedTO, typeID))
                                    processedValue++;
                            }
                            else
                            {
                                pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                ioPair.PairTO = pair;
                                ioPair.Update(true);
                            }
                        }
                        else if (processedTO.StartTime.TimeOfDay == endTime.TimeOfDay)
                        {
                            if (!rulesForEmployee.ContainsKey(Constants.RuleOvertimeShiftEnd)
                               || ((rulesForEmployee.ContainsKey(Constants.RuleOvertimeShiftEnd) && rulesForEmployee[Constants.RuleOvertimeShiftEnd].RuleValue <= ioPairDuration.TotalMinutes)))
                            {
                                int typeID = Constants.overtimeUnjustified;
                                if (rulesForEmployee.ContainsKey(Constants.RuleCompanyInitialOvertime))
                                    typeID = rulesForEmployee[Constants.RuleCompanyInitialOvertime].RuleValue;

                                foreach (WorkTimeIntervalTO interval in intervals)
                                {
                                    DateTime intervalStart = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day,
                                            interval.StartTime.Hour, interval.StartTime.Minute, interval.StartTime.Second);
                                    DateTime intervalEnd = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day,
                                        interval.EndTime.Hour, interval.EndTime.Minute, interval.EndTime.Second);

                                    if (schDict.ContainsKey(interval.TimeSchemaID) &&
                                        (schDict[interval.TimeSchemaID].Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper())
                                        || schDict[interval.TimeSchemaID].Type.Trim().ToUpper().Equals(Constants.schemaTypeNightFlexi.Trim().ToUpper())))
                                    {
                                        intervalStart = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day,
                                            interval.EarliestArrived.Hour, interval.EarliestArrived.Minute, interval.EarliestArrived.Second);
                                        intervalEnd = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day,
                                            interval.EarliestLeft.Hour, interval.EarliestLeft.Minute, interval.EarliestLeft.Second);

                                        // Sanja 08.10.2013. - night third shifts will have more than 8 hours between EA and EL, do not considere same interval as next
                                        if (intervalStart.TimeOfDay <= endTime.TimeOfDay)
                                        {
                                            intervalStart = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, 23, 59, 0);
                                            intervalEnd = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, 23, 59, 0);
                                        }
                                    }

                                    //if (intervalStart.TimeOfDay < processedTO.EndTime.TimeOfDay && intervalEnd.TimeOfDay >= processedTO.EndTime.TimeOfDay)
                                    if (intervalStart.TimeOfDay < processedTO.EndTime.TimeOfDay && intervalEnd.TimeOfDay > processedTO.StartTime.TimeOfDay)
                                    {                                                                                
                                        IOPairProcessedTO ioPairNextInterval = new IOPairProcessedTO(processedTO);
                                        ioPairNextInterval.StartTime = intervalStart;
                                        processedValue += processIOPair(rulesForEmployee, intervals, pair, ioPairNextInterval, emplID, date, intervalStart, intervalEnd, isHoliday, schDict);
                                        processedTO.EndTime = intervalStart;
                                        break;
                                    }
                                }

                                if (rulesForEmployee.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                    Common.Misc.roundPairEndTime(processedTO, rulesForEmployee[Constants.RuleOvertimeRounding].RuleValue, true);
                                // tamara 17.7.2018.
                                // Nenad insert correct pass type 
                                if (processedTO.PassTypeID != 0)
                                    typeID = processedTO.PassTypeID;
                                if (insertProcessed(pair, processedTO, typeID))
                                    processedValue++;
                            }
                            else
                            {
                                pair.ProcessedGenUsed = Constants.ioPairProcessed;
                                ioPair.PairTO = pair;
                                ioPair.Update(true);
                            }
                        }
                    }
                }

                return processedValue;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".ProcessingIOPairs() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace + "\n Exeption throwen for io pair id = " + pair.IOPairID);

                return processedValue;
            }
        }

        private bool insertProcessed(IOPairTO pair, IOPairProcessedTO processedTO, int ioPairType)
        {
            if (processedTO.EmployeeID == 251 && processedTO.IOPairDate == new DateTime(2024, 11, 11))
            {
            }
            if (processedTO.EndTime <= processedTO.StartTime || processedTO.StartTime.Date != processedTO.EndTime.Date)
            {
                if (pair != null)
                {
                   
                    IOPair pair1 = new IOPair();
                    pair1.PairTO = pair;
                    pair1.PairTO.ProcessedGenUsed = Constants.ioPairProcessed;
                    bool succ1 = pair1.Update(true);
                    debug.writeLog(DateTime.Now +
                         this.ToString() + ".StartEndValidation() EmloyeeID: " + processedTO.EmployeeID + "; Date: " + processedTO.IOPairDate.ToString("dd.MM.yyyy") + "; StartTime: " + processedTO.StartTime.ToString("dd.MM.yyyy HH:mm:ss") +
                         "; EndTime: " + processedTO.EndTime.ToString("dd.MM.yyyy HH:mm:ss") + "; PairID: " + pair.IOPairID);
                }
                else
                    debug.writeLog(DateTime.Now +
                                         this.ToString() + ".StartEndValidation() EmloyeeID: " + processedTO.EmployeeID + "; Date: " + processedTO.IOPairDate.ToString("dd.MM.yyyy") + "; StartTime: " + processedTO.StartTime.ToString("dd.MM.yyyy HH:mm:ss") +
                                         "; EndTime: " + processedTO.EndTime.ToString("dd.MM.yyyy HH:mm:ss"));
                return false;
            }

            if (!validatePair(processedTO))
            {
                if (pair != null)
                {
                    IOPair pair1 = new IOPair();
                    pair1.PairTO = pair;
                    pair1.PairTO.ProcessedGenUsed = Constants.ioPairProcessed;
                    bool succ1 = pair1.Update(true);
                }
                return false;
            }
            IOPairProcessed processed = new IOPairProcessed();
            processed.IOPairProcessedTO = processedTO;
            if (!allTypes.ContainsKey(ioPairType))
                return false;
            PassTypeTO type = allTypes[ioPairType];
            processed.IOPairProcessedTO.ConfirmationFlag =type.ConfirmFlag;
            if (processed.IOPairProcessedTO.VerificationFlag != (int)Constants.Verification.NotVerified)
                processed.IOPairProcessedTO.VerificationFlag = type.VerificationFlag;
            processed.IOPairProcessedTO.Alert = Constants.alertStatusNoAlert.ToString();
            processed.IOPairProcessedTO.ManualCreated = Constants.noInt;
            processed.IOPairProcessedTO.PassTypeID = ioPairType;

            bool succ = processed.BeginTransaction();
            if (succ)
            {
                try
                {

                    succ = succ && processed.Save(false) > 0;
                    if (succ)
                    {
                        if (pair != null)
                        {
                            IOPair pair1 = new IOPair();
                            pair1.PairTO = pair;
                            pair1.PairTO.ProcessedGenUsed = Constants.ioPairProcessed;
                            pair1.SetTransaction(processed.GetTransaction());
                            succ = succ && pair1.Update(false);
                        }
                        if (succ)
                        {
                            processed.CommitTransaction();
                            if (!insertedPairs.ContainsKey(processedTO.EmployeeID))
                            {
                                insertedPairs.Add(processedTO.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                            }
                            if (!insertedPairs[processedTO.EmployeeID].ContainsKey(processedTO.IOPairDate.Date))
                            {
                                insertedPairs[processedTO.EmployeeID].Add(processedTO.IOPairDate.Date, new List<IOPairProcessedTO>()); 
                            }
                            insertedPairs[processedTO.EmployeeID][processedTO.IOPairDate.Date].Add(processedTO);
                        }
                        else
                        {
                            processed.RollbackTransaction();
                            if (pair != null)
                            {
                                debug.writeLog(DateTime.Now + this.ToString() + ".insertProcessed() : IOPairProcessed save failed for IOPairID: " + pair.IOPairID);
                            }
                            else
                            {

                                debug.writeLog(DateTime.Now +
                                                     this.ToString() + ".insertProcessed(): IOPairProcessed save failed for EmloyeeID: " + processedTO.EmployeeID + "; Date: " + processedTO.IOPairDate.ToString("dd.MM.yyyy") + "; StartTime: " + processedTO.StartTime.ToString("dd.MM.yyyy HH:mm:ss") +
                                                     "; EndTime: " + processedTO.EndTime.ToString("dd.MM.yyyy HH:mm:ss"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (processed.GetTransaction() != null)
                        processed.RollbackTransaction();
                    if (pair != null)
                    {
                        debug.writeLog(DateTime.Now + " Exception in: " +
                         this.ToString() + ".insertProcessed() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace + "\n IOPairProcessed save failed for IOPairID: " + pair.IOPairID);
                    }
                    else
                    {
                        debug.writeLog(DateTime.Now + " Exception in: " +
                         this.ToString() + ".insertProcessed() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace + "\n IOPairProcessed save failed for EmloyeeID: " + processedTO.EmployeeID + "; Date: " + processedTO.IOPairDate.ToString("dd.MM.yyyy") + "; StartTime: " + processedTO.StartTime.ToString("dd.MM.yyyy HH:mm:ss") +
                                                     "; EndTime: " + processedTO.EndTime.ToString("dd.MM.yyyy HH:mm:ss"));
                    }
                }
            }//if (succ)
            return succ;
        }

        private bool validatePair(IOPairProcessedTO currentTO)
        {
            bool valid = true;
            try
            {
                if (insertedPairs.ContainsKey(currentTO.EmployeeID) && insertedPairs[currentTO.EmployeeID].ContainsKey(currentTO.IOPairDate.Date))
                {
                    foreach (IOPairProcessedTO proceesed in insertedPairs[currentTO.EmployeeID][currentTO.IOPairDate.Date])
                    {
                        if ((currentTO.StartTime <= proceesed.StartTime && currentTO.EndTime > proceesed.StartTime)
                            || (currentTO.EndTime >= proceesed.EndTime && currentTO.StartTime < proceesed.EndTime)
                            || (currentTO.StartTime >= proceesed.StartTime && currentTO.EndTime <= proceesed.EndTime))
                        {
                            valid = false;
                            debug.writeLog(DateTime.Now +
                    this.ToString() + ".OverlopValidation() EmloyeeID: " + currentTO.EmployeeID + "; Date: " + currentTO.IOPairDate.ToString("dd.MM.yyyy") + "; StartTime: " + currentTO.StartTime.ToString("dd.MM.yyyy HH:mm:ss") +
                    "; EndTime: " + currentTO.EndTime.ToString("dd.MM.yyyy HH:mm:ss") + "; Overlop processed pair StartTime " + proceesed.StartTime.ToString("dd.MM.yyyy HH:mm:ss") + "; EndTime: " + proceesed.EndTime.ToString("dd.MM.yyyy HH:mm:ss"));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                 this.ToString() + ".validatePair() message: " + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            return valid;
        }

        bool isNightShiftDay(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
        {
            // see if the day is a night shift day (contains intervals starting with 00:00 and/or finishing with 23:59)
            IDictionaryEnumerator dayIntervalsEnum = dayIntervals.GetEnumerator();
            while (dayIntervalsEnum.MoveNext())
            {
                WorkTimeIntervalTO dayInterval = (WorkTimeIntervalTO)dayIntervalsEnum.Value;
                if (((dayInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0)) ||
                    (dayInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))) &&
                    (dayInterval.EndTime > dayInterval.StartTime))
                {
                    return true;
                }
            }
            return false;
        }

        private void getSpecialEmployees()
        {
            try
            {
                Employee employee = new Employee();
                employee.EmplTO.Type = Constants.emplSpecial;
                specialEmpl = employee.SearchDictionary();               
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".getSpecialEmployees() : " + ex.StackTrace + "\n");
            }
        }

        private void  getExtraordinaryEmployees()
        {            
            try
            {
                Employee employee = new Employee();
                employee.EmplTO.Type = Constants.emplExtraOrdinary;
                extraOrdinaryEmpl = employee.SearchDictionary();               
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".getSpecialEmployees() : " + ex.StackTrace + "\n");
            }
        }

		/// <summary>
		/// Get logs from XML file, deserialize it, 
		/// insert records to the log_tmp, one by one process them and push to log table
        /// Get logs from mobile XML file, deserialize it, 
        /// insert records to the log_tmp_additional_info, one by one process them and push to log table and log_additional_info table
		/// </summary>
        /// <returns>Number of new records in log_tmp and log_tmp_additional_info table</returns>
        public int ImportLogs()
        {
            // Number of records saved in log_tmp tables
            int logSaved = 0;

            try
            {
                string[] files = { };
                string[] filesMobile = { };
                Pass pass = new Pass();
                bool succeededClearTmp = false;

                try
                {
                    files = UnprocessedFiles();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                // 21.05.2014. Sanja - check if there are mobile reader XML files to process
                try
                {
                    filesMobile = UnprocessedFilesMobile();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //System.Console.WriteLine("Number of files: " + files.Length + "\n");
                debug.writeLog("Number of files: " + files.Length + "\n");
                debug.writeLog("Number of files mobile: " + filesMobile.Length + "\n");

                // create logs from reader XML files
                //LogTO loghandler = new LogTO();
                List<LogTO> newLogList = new List<LogTO>();
                List<LogTO> newLogTmpList = new List<LogTO>();
                foreach (string logFilePath in files)
                {
                    // Send notification about current file processing
                    Controller.FileProcessing(logFilePath, true);
                    //
                    // Deserialize file
                    //
                    debug.writeBenchmarkLog(" + File Deserialization : STARTED! +\n");
                    try
                    {
                        newLogList = this.GetFromXMLSource(logFilePath);
                    }
                    catch       // if the file deserialization doesn't succeed continue with processing the other ones
                    {
                        debug.writeBenchmarkLog(" + File Deserialization : FAILED! +\n");
                        continue;
                    }

                    // according to licence keep all logs or just logs where tag id is not 0

                    if (!processAllTags)
                    {
                        newLogTmpList = new List<LogTO>();
                        foreach (LogTO log in newLogList)
                        {
                            if (log.TagID > 0)
                            {
                                newLogTmpList.Add(log);
                            }
                        }

                        newLogList = newLogTmpList;
                    }

                    //09.09.2009 Natasa
                    //split list to lists of max 100 logs
                    int numOfLogs = 0;
                    int recToProcess = Constants.RecordsToProcess;
                    if (!recordsToProcess)
                        recToProcess = newLogList.Count;
                    List<LogTO> newLogTmp = new List<LogTO>();
                    List<List<LogTO>> listOfLogLists = new List<List<LogTO>>();

                    foreach (LogTO log in newLogList)
                    {
                        if (numOfLogs < recToProcess)
                        {
                            newLogTmp.Add(log);
                            numOfLogs++;
                        }
                        else
                        {
                            listOfLogLists.Add(newLogTmp);
                            newLogTmp = new List<LogTO>();
                            newLogTmp.Add(log);
                            numOfLogs = 1;
                        }
                    }

                    if (!listOfLogLists.Contains(newLogTmp))
                        listOfLogLists.Add(newLogTmp);

                    if (newLogList.Count.Equals(0))
                    {
                        // Send notification about current file processing
                        Controller.FileProcessing(logFilePath, false);

                        //
                        // Move XML file to archived
                        //
                        debug.writeBenchmarkLog(" ++++ Move file: " + logFilePath + " to archived: STARTED! ++++\n");
                        if (!this.MoveFileToArchived(logFilePath))
                        {
                            // Send notification about current file processing
                            Controller.FileProcessing(logFilePath, false);
                            continue;
                        }
                        debug.writeBenchmarkLog(" ++++ Move file: " + logFilePath + " to archived: FINISHED! ++++\n");

                        // Goto next file
                        continue;
                    }
                    debug.writeBenchmarkLog(" + File Deserialization : FINISHED! +\n");

                    //
                    // Clear log_tmp table
                    //
                    debug.writeBenchmarkLog(" ++ Clear log_tmp : STARTED! ++\n");
                    succeededClearTmp = this.ClearLogTmp();
                    if (!succeededClearTmp)
                    {
                        DataProcessingException dpEx = new DataProcessingException("", 4);
                        debug.writeLog(DateTime.Now + " " +
                            this.ToString() + ".ImportLogs() : " + dpEx.message + ". \n" + dpEx.TrackMessage(2) + " \n");
                        throw dpEx;

                        // Send notification about current file processing
                        //Controller.FileProcessing(logFilePath, false);
                        //continue;
                    }
                    debug.writeBenchmarkLog(" ++ Clear log_tmp : FINISHED! ++\n");

                    //09.09.2009 Natasa
                    //insert logs to log_tmp and log table in iterations
                    //one iteration can insert max 100 log records
                    int part = 0;
                    debug.writeBenchmarkLog(" !!!! Push data to log_tmp and log iteration : STARTED! !!!!\n");
                    bool logExeption = false;
                    foreach (List<LogTO> logPartList in listOfLogLists)
                    {
                        part++;
                        logSaved = 0;
                        debug.writeBenchmarkLog(" +++ Push data to log_tmp and log PART :" + part + " ; number of logs :" + logPartList.Count + " STARTED! +++ \n");
                        //
                        // Move data from XML file to log_tmp table
                        //
                        debug.writeBenchmarkLog(" +++ Push data to log_tmp: STARTED! +++ \n");

                        // tamara 27.4.2018 zanemarivanje gate-ova koji se ne obradjuju 
                        //proverava se da li lista svih reader-a sadrzi onaj koji je dodeljen u log-u 
                        //ako sadrzi,taj reader postaje curentReader
                        //ako gate-ovi koji su dodeljeni u config fajlu odgovaraju gate-u na kome se nalazi antena currentReader-a onda ce se obradjivati log
                        readerList = new Reader().SearchDictionary();
                        ReaderTO currentReader = new ReaderTO();


                        foreach (LogTO logMember in logPartList)
                        {
                            bool readerFound = false;

                            if (readerList.ContainsKey(logMember.ReaderID))
                            {
                                readerFound = true;
                                currentReader = readerList[logMember.ReaderID];
                            }

                            if (!readerFound)
                            {
                                continue;
                               //  logProcessingFailed(logPartList, "Record " + logTag.LogID + " Reader for this log not found. ");
                               //  return;
                            }

                            if (gatesForProcess.Contains(currentReader.A1GateID.ToString()))
                                logSaved += this.SaveToTmp(logMember);
                           
                        }

                        if ((logSaved.Equals(0)) && (logPartList.Count > 0))
                        {
                            DataProcessingException dpEx = new DataProcessingException("", 5);
                            debug.writeLog(DateTime.Now + " " +
                                this.ToString() + ".ImportLogs() : " + dpEx.message + ". \n" + dpEx.TrackMessage(2) + " \n");

                            // Send notification about current file processing
                            Controller.FileProcessing(logFilePath, false);
                            continue;
                        }
                        debug.writeBenchmarkLog(" +++ Push data to log_tmp: FINISHED! Number of records affected: "
                            + logSaved.ToString() + " +++\n");

                        //
                        // Proccess logTmp, push record to log
                        //
                        debug.writeBenchmarkLog(" ++++ Move from log_tmp to log: STARTED! ++++\n");
                        if (!this.MoveToLog())
                        {
                            // Send notification about current file processing
                            Controller.FileProcessing(logFilePath, false);
                            logExeption = true;
                            continue;
                        }
                        debug.writeBenchmarkLog(" ++++ Move from log_tmp to log: FINISHED! ++++\n");

                        //
                        // Clear log_tmp table
                        //
                        debug.writeBenchmarkLog(" +++ Clear log_tmp: STARTED! +++\n");
                        succeededClearTmp = this.ClearLogTmp();
                        if (!succeededClearTmp)
                        {
                            DataProcessingException dpEx = new DataProcessingException("", 4);
                            debug.writeLog(DateTime.Now + " " +
                                this.ToString() + ".ImportLogs() : " + dpEx.message + ". \n" + dpEx.TrackMessage(2) + " \n");
                            throw dpEx;

                            // Send notification about current file processing
                            // Controller.FileProcessing(logFilePath, false);
                            // continue;
                        }
                        debug.writeBenchmarkLog(" +++ Clear log_tmp: FINISHED! +++\n");
                    }
                    debug.writeBenchmarkLog(" !!!! Push data to log_tmp and log iteration : FINISHED! !!!! Total number of inserted logs: " + newLogList.Count + "\n");
                    if (!logExeption)
                    {
                        //
                        // Move XML file to archived
                        //
                        debug.writeBenchmarkLog(" ++++ Move file: " + logFilePath + " to archived: STARTED! ++++\n");
                        if (!this.MoveFileToArchived(logFilePath))
                        {
                            // Send notification about current file processing
                            Controller.FileProcessing(logFilePath, false);
                            continue;
                        }
                        debug.writeBenchmarkLog(" ++++ Move file: " + logFilePath + " to archived: FINISHED! ++++\n");
                    }
                    else
                    {
                        debug.writeBenchmarkLog(" Inserting Log exeption happened and file: " + logFilePath + " was not move to archived.");
                    }
                    //
                    // Create Passes from Log
                    //
                    debug.writeBenchmarkLog(" +++++ Create Passes from log: STARTED! +++++\n");
                    if (!this.PopulatePasses())
                    {
                        // Send notification about current file processing
                        Controller.FileProcessing(logFilePath, false);
                        continue;
                    }
                    debug.writeBenchmarkLog(" +++++ Create Passes from log: FINISHED! +++++\n");

                    // Send notification about current file processing
                    Controller.FileProcessing(logFilePath, false);
                }

                // 21.05.2014. Sanja - create logs with additional data from mobile reader XML files
                List<LogTmpAdditionalInfoTO> newLogMobileList = new List<LogTmpAdditionalInfoTO>();
                List<LogTmpAdditionalInfoTO> newLogMobileTmpList = new List<LogTmpAdditionalInfoTO>();
                bool succeededClearMobileTmp = false;
                foreach (string logFilePath in filesMobile)
                {
                    // Send notification about current file processing
                    Controller.FileProcessing(logFilePath, true);
                    //
                    // Deserialize file
                    //
                    debug.writeBenchmarkLog(" + File Mobile Deserialization : STARTED! +\n");
                    try
                    {
                        newLogMobileList = this.GetFromXMLSourceMobile(logFilePath);
                    }
                    catch       // if the file deserialization doesn't succeed continue with processing the other ones
                    {
                        debug.writeBenchmarkLog(" + File Mobile Deserialization : FAILED! +\n");
                        continue;
                    }

                    // according to licence keep all logs or just logs where tag id is not 0

                    if (!processAllTags)
                    {
                        newLogMobileTmpList = new List<LogTmpAdditionalInfoTO>();
                        foreach (LogTmpAdditionalInfoTO log in newLogMobileList)
                        {
                            if (log.TagID > 0)
                            {
                                newLogMobileTmpList.Add(log);
                            }
                        }

                        newLogMobileList = newLogMobileTmpList;
                    }

                    //09.09.2009 Natasa
                    //split list to lists of max 100 logs
                    int numOfLogsMobile = 0;
                    int recToProcessMobile = Constants.RecordsToProcess;
                    if (!recordsToProcess)
                        recToProcessMobile = newLogMobileList.Count;
                    List<LogTmpAdditionalInfoTO> newLogMobileTmp = new List<LogTmpAdditionalInfoTO>();
                    List<List<LogTmpAdditionalInfoTO>> listOfLogMobileLists = new List<List<LogTmpAdditionalInfoTO>>();

                    foreach (LogTmpAdditionalInfoTO log in newLogMobileList)
                    {
                        if (numOfLogsMobile < recToProcessMobile)
                        {
                            newLogMobileTmp.Add(log);
                            numOfLogsMobile++;
                        }
                        else
                        {
                            listOfLogMobileLists.Add(newLogMobileTmp);
                            newLogMobileTmp = new List<LogTmpAdditionalInfoTO>();
                            newLogMobileTmp.Add(log);
                            numOfLogsMobile = 1;
                        }
                    }

                    if (!listOfLogMobileLists.Contains(newLogMobileTmp))
                        listOfLogMobileLists.Add(newLogMobileTmp);

                    if (newLogMobileList.Count.Equals(0))
                    {
                        // Send notification about current file processing
                        Controller.FileProcessing(logFilePath, false);

                        //
                        // Move XML file to archived mobile
                        //
                        debug.writeBenchmarkLog(" ++++ Move file: " + logFilePath + " to archived mobile: STARTED! ++++\n");
                        if (!this.MoveFileToArchivedMobile(logFilePath))
                        {
                            // Send notification about current file processing
                            Controller.FileProcessing(logFilePath, false);
                            continue;
                        }
                        debug.writeBenchmarkLog(" ++++ Move file: " + logFilePath + " to archived mobile: FINISHED! ++++\n");

                        // Goto next file
                        continue;
                    }
                    debug.writeBenchmarkLog(" + File Mobile Deserialization : FINISHED! +\n");

                    //
                    // Clear log_tmp_additional_info table
                    //
                    debug.writeBenchmarkLog(" ++ Clear log_tmp_additional_info : STARTED! ++\n");
                    succeededClearMobileTmp = this.ClearLogMobileTmp();
                    if (!succeededClearMobileTmp)
                    {
                        DataProcessingException dpEx = new DataProcessingException("", 4);
                        debug.writeLog(DateTime.Now + " " +
                            this.ToString() + ".ImportLogs() : " + dpEx.message + ". \n" + dpEx.TrackMessage(2) + " \n");
                        throw dpEx;

                        // Send notification about current file processing
                        //Controller.FileProcessing(logFilePath, false);
                        //continue;
                    }
                    debug.writeBenchmarkLog(" ++ Clear log_tmp_additional_info : FINISHED! ++\n");

                    //09.09.2009 Natasa
                    //insert logs to log_tmp_additional_info and log table in iterations
                    //one iteration can insert max 100 log records
                    int partMobile = 0;
                    debug.writeBenchmarkLog(" !!!! Push data to log_tmp_additional_info and log iteration : STARTED! !!!!\n");
                    bool logMobileException = false;
                    foreach (List<LogTmpAdditionalInfoTO> logPartList in listOfLogMobileLists)
                    {
                        partMobile++;
                        logSaved = 0;
                        debug.writeBenchmarkLog(" +++ Push data to log_tmp_additional_info and log PART :" + partMobile + " ; number of logs :" + logPartList.Count + " STARTED! +++ \n");
                        //
                        // Move data from XML file to log_tmp_additional_info table
                        //
                        debug.writeBenchmarkLog(" +++ Push data to log_tmp_additional_info: STARTED! +++ \n");
                        foreach (LogTmpAdditionalInfoTO logMember in logPartList)
                        {
                            logSaved += this.SaveToMobileTmp(logMember);
                        }

                        if ((logSaved.Equals(0)) && (logPartList.Count > 0))
                        {
                            DataProcessingException dpEx = new DataProcessingException("", 5);
                            debug.writeLog(DateTime.Now + " " +
                                this.ToString() + ".ImportLogs() : " + dpEx.message + ". \n" + dpEx.TrackMessage(2) + " \n");

                            // Send notification about current file processing
                            Controller.FileProcessing(logFilePath, false);
                            continue;
                        }
                        debug.writeBenchmarkLog(" +++ Push data to log_tmp_additional_info: FINISHED! Number of records affected: "
                            + logSaved.ToString() + " +++\n");

                        //
                        // Proccess logTmpAdditionalInfo, push record to log and log additional info
                        //
                        debug.writeBenchmarkLog(" ++++ Move from log_tmp_additional_info to log and log_additional_info: STARTED! ++++\n");
                        if (!this.MoveToLogMobile())
                        {
                            // Send notification about current file processing
                            Controller.FileProcessing(logFilePath, false);
                            logMobileException = true;
                            continue;
                        }
                        debug.writeBenchmarkLog(" ++++ Move from log_tmp_additional_info to log and log_additional_info: FINISHED! ++++\n");

                        //
                        // Clear log_tmp_additional_info table
                        //
                        debug.writeBenchmarkLog(" +++ Clear log_tmp_additional_info: STARTED! +++\n");
                        succeededClearMobileTmp = this.ClearLogMobileTmp();
                        if (!succeededClearMobileTmp)
                        {
                            DataProcessingException dpEx = new DataProcessingException("", 4);
                            debug.writeLog(DateTime.Now + " " +
                                this.ToString() + ".ImportLogs() : " + dpEx.message + ". \n" + dpEx.TrackMessage(2) + " \n");
                            throw dpEx;

                            // Send notification about current file processing
                            // Controller.FileProcessing(logFilePath, false);
                            // continue;
                        }
                        debug.writeBenchmarkLog(" +++ Clear log_tmp_additional_info: FINISHED! +++\n");
                    }
                    debug.writeBenchmarkLog(" !!!! Push data to log_tmp_additional_info and log iteration : FINISHED! !!!! Total number of inserted logs: " + newLogMobileList.Count + "\n");
                    if (!logMobileException)
                    {
                        //
                        // Move XML file to archived mobile
                        //
                        debug.writeBenchmarkLog(" ++++ Move file: " + logFilePath + " to archived mobile: STARTED! ++++\n");
                        if (!this.MoveFileToArchivedMobile(logFilePath))
                        {
                            // Send notification about current file processing
                            Controller.FileProcessing(logFilePath, false);
                            continue;
                        }
                        debug.writeBenchmarkLog(" ++++ Move file: " + logFilePath + " to archived mobile: FINISHED! ++++\n");
                    }
                    else
                    {
                        debug.writeBenchmarkLog(" Inserting Log exeption happened and file: " + logFilePath + " was not move to archived mobile.");
                    }
                    //
                    // Create Passes from Log
                    //
                    debug.writeBenchmarkLog(" +++++ Create Passes from log: STARTED! +++++\n");
                    if (!this.PopulatePasses())
                    {
                        // Send notification about current file processing
                        Controller.FileProcessing(logFilePath, false);
                        continue;
                    }
                    debug.writeBenchmarkLog(" +++++ Create Passes from log: FINISHED! +++++\n");

                    // Send notification about current file processing
                    Controller.FileProcessing(logFilePath, false);
                }
            }
            catch (DataProcessingException dpEx)
            {
                if (dpEx.Number == 4) throw dpEx;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".ImportLogs() : " + ex.StackTrace + "\n");
            }

            return logSaved;
        }

		/// <summary>
		/// Delete all records from log_tmp
		/// </summary>
		/// <returns>true if succeeded, false other</returns>
		private bool ClearLogTmp()
		{
			bool succeeded = false;

			try
			{
				Log logTemp = new Log();
				logTemp.ClearLogTmp();
				succeeded = true;
			}
			catch(Exception ex)
			{
				DataProcessingException dpEx = new DataProcessingException(ex.StackTrace, 4);
				debug.writeLog(dpEx.message + dpEx.TrackMessage(2));
			}

			return succeeded;
		}

        /// <summary>
        /// Delete all records from log_tmp_additional_info
        /// </summary>
        /// <returns>true if succeeded, false other</returns>
        private bool ClearLogMobileTmp()
        {
            bool succeeded = false;

            try
            {
                Log logTemp = new Log();
                logTemp.ClearLogMobileTmp();
                succeeded = true;
            }
            catch (Exception ex)
            {
                DataProcessingException dpEx = new DataProcessingException(ex.StackTrace, 4);
                debug.writeLog(dpEx.message + dpEx.TrackMessage(2));
            }

            return succeeded;
        }

		/// <summary>
		/// Save log data from XML file to log_tmp table.
		/// </summary>
		/// <param name="logData">Log object</param>
		/// <returns>number of rows affected</returns>
		private int SaveToTmp(LogTO logData)
		{
			int rowsAffected = 0;

			try
			{
				rowsAffected = new Log().SaveToTmp(logData);
			}
			catch(Exception ex)
			{
				DataProcessingException exproc = new DataProcessingException(ex.StackTrace, 5);
				debug.writeLog(exproc.message + logData.ToString() + exproc.TrackMessage(2));
                debug.writeLog(ex.ToString());
			}

			return rowsAffected;
		}

        /// <summary>
        /// Save log data from XML file to log_tmp_additional_info table.
        /// </summary>
        /// <param name="logData">LogTmpAdditionalInfoTO object</param>
        /// <returns>number of rows affected</returns>
        private int SaveToMobileTmp(LogTmpAdditionalInfoTO logData)
        {
            int rowsAffected = 0;

            try
            {
                rowsAffected = new Log().SaveToMobileTmp(logData);
            }
            catch (Exception ex)
            {
                DataProcessingException exproc = new DataProcessingException(ex.StackTrace, 5);
                debug.writeLog(exproc.message + logData.ToString() + exproc.TrackMessage(2));
            }

            return rowsAffected;
        }

		private bool MoveToLog()
		{
			int importedRows = 0;
			bool succeeded = false; 

			try
			{
				Log currentLog = new Log();
				importedRows = currentLog.importLog();
				succeeded = true;
			}
			catch(Exception ex)
			{
				DataProcessingException dataProcEx = new DataProcessingException(ex.StackTrace, 6);
				debug.writeLog( " Exception in: " + 
					this.ToString() + ".MoveToLog() : " + dataProcEx.message + dataProcEx.TrackMessage(2));
			}

			return succeeded;
		}

        private bool MoveToLogMobile()
        {
            bool succeeded = false;

            try
            {
                Log currentLog = new Log();
                succeeded = currentLog.importLogMobile();
            }
            catch (Exception ex)
            {
                succeeded = false;
                DataProcessingException dataProcEx = new DataProcessingException(ex.StackTrace, 6);
                debug.writeLog(" Exception in: " +
                    this.ToString() + ".MoveToLogMobile() : " + dataProcEx.message + dataProcEx.TrackMessage(2));
            }

            return succeeded;
        }

		private bool MoveFileToArchived(string filepath)
		{
			string targetPath = "";
			bool isSucceeded = false;

			try
			{
				targetPath = Constants.archived
					+ Path.GetFileNameWithoutExtension(filepath) + "_Copy" + DateTime.Now.ToString(FILESUFIX)
					+ Path.GetExtension(filepath);

				File.Move(filepath, targetPath);
				isSucceeded = true;
			}
			catch(Exception ex)
			{
				DataProcessingException dataProcEx = new DataProcessingException(ex.StackTrace, 1);
				debug.writeLog( " Exception in: " + 
					this.ToString() + ".MoveFileToArchived() : " + dataProcEx.message + dataProcEx.TrackMessage(2));
			}

			return isSucceeded;
		}

        private bool MoveFileToArchivedMobile(string filepath)
        {
            string targetPath = "";
            bool isSucceeded = false;

            try
            {
                if (!Directory.Exists(Constants.archivedMobile))
                    Directory.CreateDirectory(Constants.archivedMobile);

                targetPath = Constants.archivedMobile
                    + Path.GetFileNameWithoutExtension(filepath) + "_Copy" + DateTime.Now.ToString(FILESUFIX)
                    + Path.GetExtension(filepath);

                File.Move(filepath, targetPath);
                isSucceeded = true;
            }
            catch (Exception ex)
            {
                DataProcessingException dataProcEx = new DataProcessingException(ex.StackTrace, 1);
                debug.writeLog(" Exception in: " +
                    this.ToString() + ".MoveFileToArchivedMobile() : " + dataProcEx.message + dataProcEx.TrackMessage(2));
            }

            return isSucceeded;
        }

		private bool MoveFileToTrash(string filePath)
		{
			string targetPath = "";
			bool isSucceeded = false;

			try
			{
				
				targetPath = Constants.trash 
					+ Path.GetFileNameWithoutExtension(filePath) + "_Error" + DateTime.Now.ToString(FILESUFIX)
					+ Path.GetExtension(filePath);
				
				if (Directory.Exists(Constants.trash ))
				{
					File.Move(filePath, targetPath);
					isSucceeded = true;
				}
				else
				{
					throw new DataProcessingException(Constants.trash, 1);
				}

			}
			catch(Exception ex)
			{
				DataProcessingException dataProcEx = new DataProcessingException(ex.StackTrace, 1);
				debug.writeLog( " Exception in: " + 
					this.ToString() + ".MoveFileToTrash() : " + dataProcEx.message + dataProcEx.TrackMessage(2));
			}

			return isSucceeded;
		}

        private bool MoveFileToTrashMobile(string filePath)
        {
            string targetPath = "";
            bool isSucceeded = false;

            try
            {
                if (!Directory.Exists(Constants.trashMobile))
                    Directory.CreateDirectory(Constants.trashMobile);

                targetPath = Constants.trashMobile
                    + Path.GetFileNameWithoutExtension(filePath) + "_Error" + DateTime.Now.ToString(FILESUFIX)
                    + Path.GetExtension(filePath);

                if (Directory.Exists(Constants.trash))
                {
                    File.Move(filePath, targetPath);
                    isSucceeded = true;
                }
                else
                {
                    throw new DataProcessingException(Constants.trash, 1);
                }

            }
            catch (Exception ex)
            {
                DataProcessingException dataProcEx = new DataProcessingException(ex.StackTrace, 1);
                debug.writeLog(" Exception in: " +
                    this.ToString() + ".MoveFileToTrashMobile() : " + dataProcEx.message + dataProcEx.TrackMessage(2));
            }

            return isSucceeded;
        }

		private string[] UnprocessedFiles()
		{
			string[] paths = {};

			try
			{
				paths = Directory.GetFiles(Constants.unprocessed);
			}
			catch
			{
				DataProcessingException pde = new DataProcessingException(Constants.unprocessed, 1); 
				debug.writeLog(DateTime.Now + " Exception in: "
                    + this.ToString() + ".UnprocessedFiles() : \n " 
					+ pde.message + pde.TrackMessage(1) + " \n");
				throw pde;
			}
			return paths;
		}

        private string[] UnprocessedFilesMobile()
        {
            string[] paths = { };

            try
            {
                if (Directory.Exists(Constants.unprocessedMobile))
                    paths = Directory.GetFiles(Constants.unprocessedMobile);
            }
            catch
            {
                DataProcessingException pde = new DataProcessingException(Constants.unprocessed, 1);
                debug.writeLog(DateTime.Now + " Exception in: "
                    + this.ToString() + ".UnprocessedFilesMobile() : \n "
                    + pde.message + pde.TrackMessage(1) + " \n");
                throw pde;
            }
            return paths;
        }

		private List<LogTO> GetFromXMLSource(string logFilePath)
		{
            List<LogTO> newLogList = new List<LogTO>();

			try
			{
				Log logHandler = new Log();
				newLogList = logHandler.GetFromXMLSource(logFilePath);
			}
			catch(Exception exDes)
			{
				if (exDes is DataProcessingException)
				{
					DataProcessingException dex = (DataProcessingException) exDes; 
					// Can't open XML file - continue
					if (dex.Number == 2)
					{
						debug.writeLog("File: " + logFilePath + " " + dex.message + "\n");

					}
						// Can't deserialize XML file - move to Trash and continue
					else if (dex.Number == 3)
					{
						debug.writeLog("File: " + logFilePath + " " + dex.message + "\n");
						this.MoveFileToTrash(logFilePath);
					}
				}
				else
				{
					throw exDes;
				}
			}

			return newLogList;
		}

        private List<LogTmpAdditionalInfoTO> GetFromXMLSourceMobile(string logFilePath)
        {
            List<LogTmpAdditionalInfoTO> newLogList = new List<LogTmpAdditionalInfoTO>();

            try
            {
                Log logHandler = new Log();
                newLogList = logHandler.GetFromXMLSourceMobile(logFilePath);
            }
            catch (Exception exDes)
            {
                if (exDes is DataProcessingException)
                {
                    DataProcessingException dex = (DataProcessingException)exDes;
                    // Can't open XML file - continue
                    if (dex.Number == 2)
                    {
                        debug.writeLog("File: " + logFilePath + " " + dex.message + "\n");

                    }
                    // Can't deserialize XML file - move to Trash and continue
                    else if (dex.Number == 3)
                    {
                        debug.writeLog("File: " + logFilePath + " " + dex.message + "\n");
                        this.MoveFileToTrash(logFilePath);
                    }
                }
                else
                {
                    throw exDes;
                }
            }

            return newLogList;
        }

		/// <summary>
		/// Create Passes and push records to database.
		/// </summary>
		private bool PopulatePasses()
		{
			Pass currentPasses = new Pass();
			bool succeeded = false;

			try
			{
				currentPasses.PopulatePasses(specialEmpl,extraOrdinaryEmpl);
				succeeded = true;
			}
			catch(Exception ex)
			{
				if (ex is Util.DataProcessingException)
				{
					DataProcessingException pdex = (DataProcessingException) ex;

					DataProcessingException pde = new DataProcessingException(pdex.message, 7); 
					debug.writeLog(DateTime.Now + " Exception in: " 
						+ this.ToString() + ".PopulatePasses() : \n " 
						+ pde.message + pde.TrackMessage(2) + "\n");
				}
				else
				{
					DataProcessingException pde = new DataProcessingException(ex.StackTrace, 11); 
					debug.writeLog(DateTime.Now + " Exception in: " 
						+ this.ToString() + ".PopulatePasses() : \n " 
						+ pde.message + pde.TrackMessage(2) + "\n");
				}
			}

			return succeeded;
		}

		public bool chekPrerequests()
		{
			bool isOk = false;

			try
			{
				if ((Directory.Exists(Constants.unprocessed)) 
					&& (Directory.Exists(Constants.archived)))
				{
					isOk = true;
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " 
					+ this.ToString() + ".chekPrerequests() : Message: "+ex.Message+"/n StackTrace:" + ex.StackTrace); 
			}

			return isOk;
		}

		private void InitializeObserverClient()
		{
			Controller = NotificationController.GetInstance();	
		}

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                DoProcess = true;
                timer.Enabled = false;
                debug.writeBenchmarkLog(DateTime.Now + " timer_Elapsed");
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " dataProcessing.timer_Elapsed(): " + ex.StackTrace + "\n");
            }
        }

        private int getHoleType(WorkTimeSchemaTO workTimeSchema, List<DateTime> nationalHolidaysDays, Dictionary<string, List<DateTime>> personalHolidayDays,
            Dictionary<string, RuleTO> rulesForEmployee, EmployeeAsco4TO employeeAsco4, DateTime date, EmployeeTO empl, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules)
        {
            try
            {
                int holesType = Constants.absence;

                //if (Common.Misc.isExpatOut(rules, empl))
                //    return holesType;

                if (rulesForEmployee.ContainsKey(Constants.RuleInitialAbsence))
                    holesType = rulesForEmployee[Constants.RuleInitialAbsence].RuleValue;

                // if employee doesn't have industrial shift and date is holiday instead of absence insert holiday type                                 
                if (nationalHolidaysDays.Contains(date.Date)
                || (personalHolidayDays.ContainsKey(employeeAsco4.NVarcharValue1) && personalHolidayDays[employeeAsco4.NVarcharValue1].Contains(date.Date))
                || (employeeAsco4.DatetimeValue1 != new DateTime() && employeeAsco4.NVarcharValue1 == Constants.holidayTypeIV && date.Date.Month == employeeAsco4.DatetimeValue1.Date.Month && date.Date.Day == employeeAsco4.DatetimeValue1.Date.Day))
                {
                    holesType = Constants.absence;
                    if (nationalHolidaysDays.Contains(date.Date))
                    {
                        if (workTimeSchema.Type.Trim() != Constants.schemaTypeIndustrial && rulesForEmployee.ContainsKey(Constants.RuleHolidayPassType))
                            holesType = rulesForEmployee[Constants.RuleHolidayPassType].RuleValue;
                    }
                    else
                    {
                        if (rulesForEmployee.ContainsKey(Constants.RulePersonalHolidayPassType))                        
                            holesType = rulesForEmployee[Constants.RulePersonalHolidayPassType].RuleValue;                        
                    }
                }

                return holesType;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " dataProcessing.getHoleType(): " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool isMedicalCheckTime()
        {
            try
            {
                if (DateTime.Now > nextMedicalCheckTime)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " dataProcessing.isMedicalCheckTime(): " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool overlapInterval(IOPairProcessedTO pair, List<WorkTimeIntervalTO> intervals)
        {
            try
            {
                bool ovarlap = false;
                foreach (WorkTimeIntervalTO interval in intervals)
                {
                    if (interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes > 0)
                    {
                        if ((pair.StartTime.TimeOfDay <= interval.StartTime.TimeOfDay && pair.EndTime.TimeOfDay > interval.StartTime.TimeOfDay)
                            || (pair.StartTime.TimeOfDay > interval.StartTime.TimeOfDay && pair.StartTime.TimeOfDay < interval.EndTime.TimeOfDay))
                        {
                            ovarlap = true;
                            break;
                        }
                    }
                }

                return ovarlap;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " dataProcessing.overlapInterval(): " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool overlapPair(IOPairProcessedTO pair, List<IOPairProcessedTO> pairs)
        {
            try
            {
                bool ovarlap = false;
                foreach (IOPairProcessedTO pairTO in pairs)
                {
                    if (pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes > 0)
                    {
                        if ((pair.StartTime <= pairTO.StartTime && pair.EndTime > pairTO.StartTime) || (pair.StartTime > pairTO.StartTime && pair.StartTime < pairTO.EndTime))
                        {
                            ovarlap = true;
                            break;
                        }
                    }
                }

                return ovarlap;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " dataProcessing.overlapPair(): " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool validateOvertimePair(IOPairProcessedTO pair, Dictionary<string, RuleTO> rules, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO sch,
            List<WorkTimeIntervalTO> dayIntervals, Dictionary<int, PassTypeTO> ptDict, DateTime intervalBeforeEnd, DateTime intervalAfterStart, bool IsWorkAbsenceDay)
        {
            try
            {
                if (pair.StartTime >= pair.EndTime)
                    return false;

                // validate if shift start/end overtime rules are satisfied
                int shiftStart = 1;
                int shiftEnd = 1;
                if (rules.ContainsKey(Constants.RuleOvertimeShiftStart))
                    shiftStart = rules[Constants.RuleOvertimeShiftStart].RuleValue;
                if (rules.ContainsKey(Constants.RuleOvertimeShiftEnd))
                    shiftEnd = rules[Constants.RuleOvertimeShiftEnd].RuleValue;
                IOPairProcessedTO pairToValidate = new IOPairProcessedTO(pair);

                WorkTimeIntervalTO intervalBefore = Common.Misc.getIntervalBeforePair(pairToValidate, dayPairs, dayIntervals, sch, ptDict);

                if (!intervalBeforeEnd.Equals(new DateTime()))
                    intervalBefore.EndTime = intervalBeforeEnd;

                double intervalBeforeDuration = intervalBefore.EndTime.TimeOfDay.Subtract(intervalBefore.StartTime.TimeOfDay).TotalMinutes;

                WorkTimeIntervalTO intervalAfter = Common.Misc.getIntervalAfterPair(pairToValidate, dayPairs, dayIntervals, sch, ptDict);

                if (!intervalAfterStart.Equals(new DateTime()))
                    intervalAfter.StartTime = intervalAfterStart;

                double intervalAfterDuration = intervalAfter.EndTime.TimeOfDay.Subtract(intervalAfter.StartTime.TimeOfDay).TotalMinutes;

                if (!intervalBefore.EndTime.Equals(new DateTime()) && intervalBeforeDuration > 0 && pairToValidate.EndTime.TimeOfDay.Subtract(intervalBefore.EndTime.TimeOfDay).TotalMinutes < shiftEnd)
                    return false;

                if (!intervalAfter.StartTime.Equals(new DateTime()) && intervalAfterDuration > 0 && intervalAfter.StartTime.TimeOfDay.Subtract(pairToValidate.StartTime.TimeOfDay).TotalMinutes < shiftStart)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}
