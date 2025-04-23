using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Pass.
	/// </summary>
	public class Pass
	{
		private PassTO passTO = new PassTO();

        private Dictionary<int, ReaderTO> readerList = new Dictionary<int, ReaderTO>();
		private Dictionary<ulong, TagTO> tagTOListTest = new Dictionary<ulong,TagTO>();
        private Dictionary<int, PassTypeTO> passTypeList = new Dictionary<int, PassTypeTO>();
        private List<ExitPermissionTO> exitPermissionsList = new List<ExitPermissionTO>();
		private Dictionary<ulong, LogTO> logEmployees = new Dictionary<ulong,LogTO>();
		private Dictionary<int, EmployeeLocationTO> emplLocations = new Dictionary<int,EmployeeLocationTO>();
        private Dictionary<int, EmployeeTO> extraOrdinaryEmployees = new Dictionary<int, EmployeeTO>();
        private Dictionary<int, EmployeeTO> specialEmployees = new Dictionary<int, EmployeeTO>();
       //Boris, napravljena logika za IMLEK tako da processing obradjuje samo pass-ove(kasnije i prolaske)
        //na gate-ovima definisanim u config file-u processing-a. Moguce uneti vise gate-ova. 20170112
        private List<String> gatesForProcess = Constants.GetInfoFromGates;
		DAOFactory daoFactory = null;
		PassDAO pdao = null;

		DebugLog debug;

		public PassTO PssTO
		{
			get{ return passTO; }
			set{ passTO = value; }
		}

		public Pass()
		{
			daoFactory = DAOFactory.getDAOFactory();
			pdao = daoFactory.getPassDAO(null);

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);
		}

        public Pass(object dbConnection)
        {
            daoFactory = DAOFactory.getDAOFactory();
            pdao = daoFactory.getPassDAO(dbConnection);

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);
        }

        public Pass(bool createDAO)
        {
            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                pdao = daoFactory.getPassDAO(null);
            }

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);
        }

		public Pass(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed, 
				int locationID, int manualCreated, string createdBy, DateTime createdTime, int isWrkHrsCount)
		{
			daoFactory = DAOFactory.getDAOFactory();
			pdao = daoFactory.getPassDAO(null);

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			this.PssTO.PassID = passID;
			this.PssTO.EmployeeID = employeeID;
			this.PssTO.Direction = direction;
			this.PssTO.EventTime = eventTime;
			this.PssTO.PassTypeID = passTypeID;
			this.PssTO.PairGenUsed = pairGenUsed;
			this.PssTO.LocationID = locationID;
			this.PssTO.ManualyCreated = manualCreated;
            this.PssTO.CreatedBy = createdBy;
            this.PssTO.CreatedTime = createdTime;
			this.PssTO.IsWrkHrsCount = isWrkHrsCount;
		}

        public Pass(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
                int locationID, int manualCreated, int isWrkHrsCount)
        {
            daoFactory = DAOFactory.getDAOFactory();
            pdao = daoFactory.getPassDAO(null);

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            this.PssTO.PassID = passID;
            this.PssTO.EmployeeID = employeeID;
            this.PssTO.Direction = direction;
            this.PssTO.EventTime = eventTime;
            this.PssTO.PassTypeID = passTypeID;
            this.PssTO.PairGenUsed = pairGenUsed;
            this.PssTO.LocationID = locationID;
            this.PssTO.ManualyCreated = manualCreated;
            this.PssTO.CreatedBy = "";
            this.PssTO.CreatedTime = new DateTime();
            this.PssTO.IsWrkHrsCount = isWrkHrsCount;
        }

		public int Save(int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
			int locationID, int manualCreated, int isWrkHrsCount)
		{
			int saved = 0;
			try
			{                
                if (Misc.isLockedDate(eventTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(eventTime.Date);
                    throw new Exception(exceptionString); 
                }
				// TODO: cerate TO and send to DAO
				saved = pdao.insert(employeeID, direction, eventTime, passTypeID, pairGenUsed, locationID, manualCreated, isWrkHrsCount);
			}
			catch(Exception ex)
			{
				//if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
				{
					debug.writeLog(DateTime.Now + " Pass.Save(): Record already exist! Primary Key Violation SqlException.Number" + ex.Message + "\n");
					throw new Exception(ex.Message);
				}
				else
				{
					debug.writeLog(DateTime.Now + " Pass.Save(): " + ex.Message + "\n");
					throw new Exception(ex.Message);
				}
			}

			return saved;
		}

		public int Save(List<PassTO> passTOList, ExitPermissionTO perm, string createdBy)
		{
			int saved = 0;
			try
			{
				saved = pdao.insert(passTOList, perm, createdBy, daoFactory);
			}
			catch(Exception ex)
			{
				//if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
				{
					debug.writeLog(DateTime.Now + " Pass.Save(): Record already exist! Primary Key Violation SqlException.Number" + ex.Message + "\n");
					throw new Exception(ex.Message);
				}
				else
				{
					debug.writeLog(DateTime.Now + " Pass.Save(): " + ex.Message + "\n");
					throw new Exception(ex.Message);
				}
			}

			return saved;
		}

		public int SavePassesPermission(List<PassTO> passTOList, ExitPermissionTO perm, string createdBy)
		{
			int saved = 0;
            try
            {
                if (Misc.isLockedDate(perm.StartTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(perm.StartTime.Date);
                    throw new Exception(exceptionString);
                }

                saved = pdao.insertPassesPermission(passTOList, perm, createdBy, daoFactory);
            }
            catch (Exception ex)
            {
                //if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
                {
                    debug.writeLog(DateTime.Now + " Pass.Save(): Record already exist! Primary Key Violation SqlException.Number" + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
                else
                {
                    debug.writeLog(DateTime.Now + " Pass.SavePassesPermission(): " + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
            }

			return saved;
		}

		public int Save(bool doCommit)
		{
			int saved = 0;
			try
			{
				saved = pdao.insert(this.PssTO, doCommit);
			}
			catch(Exception ex)
			{
				//if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
				{
					debug.writeLog(DateTime.Now + " Pass.Save(): Record already exist!   Primary Key Violation SqlException.Number: " + ex.Message + "\n" 
						+ "--------- Data: \n" + 
						"PassID: " + this.PssTO.PassID + ",\n " + 
						"EventTime: " + this.PssTO.EventTime.ToUniversalTime() + ", \n " + 
						"EmployeeID: " + this.PssTO.EmployeeID.ToString() + ", \n" +
						"Direction: " + this.PssTO.Direction + ", " +
						"Location ID: " + this.PssTO.LocationID.ToString() + "\n ------ \n");
				}
				else
				{
					debug.writeLog(DateTime.Now + " Pass.Save(): " + ex.Message + "\n");
					throw new Exception(ex.Message);
				}
			}

			return saved;
		}

        /// <summary>
        /// Insert record 
        /// </summary>
        /// <returns>PassID</returns>
        public int SaveGetID(bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = pdao.insertGetID(this.PssTO, doCommit);
            }
            catch (Exception ex)
            {
                //if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
                {
                    debug.writeLog(DateTime.Now + " Pass.Save(): Record already exist!   Primary Key Violation SqlException.Number: " + ex.Message + "\n"
                        + "--------- Data: \n" +
                        "PassID: " + this.PssTO.PassID + ",\n " +
                        "EventTime: " + this.PssTO.EventTime.ToUniversalTime() + ", \n " +
                        "EmployeeID: " + this.PssTO.EmployeeID.ToString() + ", \n" +
                        "Direction: " + this.PssTO.Direction + ", " +
                        "Location ID: " + this.PssTO.LocationID.ToString() + "\n ------ \n");
                }
                else
                {
                    debug.writeLog(DateTime.Now + " Pass.Save(): " + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
            }

            return saved;
        }
		
		/// <summary>
		/// Insert record and do COMMIT (Commit set to true)
		/// </summary>
		/// <returns></returns>
		public int Save()
		{
			int savedRecord = 0;

			try
			{
				savedRecord = this.Save(true);
			}
			catch(Exception ex)
			{
				//if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
				{
					debug.writeLog(DateTime.Now + " Pass.Save(): Record already exist!   Primary Key Violation SqlException.Number: " + ex.Message + "\n"
						+ "--------- Data: \n" + 
						"PassID: " + this.PssTO.PassID.ToString() + ",\n " + 
						"EventTime: " + this.PssTO.EventTime.ToUniversalTime() + ", \n " + 
						"EmployeeID: " + this.PssTO.EmployeeID.ToString() + ", \n" +
						"Direction: " + this.PssTO.Direction + ", " +
						"Location ID: " + this.PssTO.LocationID.ToString() + "\n ------ \n");
				}
				else
				{
					debug.writeLog(DateTime.Now + " Pass.Save(): " + ex.Message + "\n");
					throw new Exception(ex.Message);
				}
			}

			return savedRecord;
		}

		public bool Update(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
			int locationID, int manualCreated, int isWrkHrsCount)
		{
			bool isUpdated = false;

			try
			{
                if (Misc.isLockedDate(eventTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(eventTime.Date);
                    throw new Exception(exceptionString);
                }
				PassTO passTo = new PassTO();

				passTo.PassID = passID;
				passTo.EmployeeID = employeeID;
				passTo.Direction = direction;
				passTo.EventTime = eventTime;
				passTo.PassTypeID = passTypeID;
				passTo.PairGenUsed = pairGenUsed;
				passTo.LocationID = locationID;
				passTo.ManualyCreated = manualCreated;
				passTo.IsWrkHrsCount = isWrkHrsCount;

				isUpdated = pdao.update(passTo, true);

			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

        public bool Update(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
            int locationID, int manualCreated, int isWrkHrsCount, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (Misc.isLockedDate(eventTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(eventTime.Date);
                    throw new Exception(exceptionString);
                }
                PassTO passTo = new PassTO();

                passTo.PassID = passID;
                passTo.EmployeeID = employeeID;
                passTo.Direction = direction;
                passTo.EventTime = eventTime;
                passTo.PassTypeID = passTypeID;
                passTo.PairGenUsed = pairGenUsed;
                passTo.LocationID = locationID;
                passTo.ManualyCreated = manualCreated;
                passTo.IsWrkHrsCount = isWrkHrsCount;

                isUpdated = pdao.update(passTo, doCommit);

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        //public bool Update()
        //{
        //    bool isUpdated = false;

        //    try
        //    {
        //        if (Misc.isLockedDate(passTo.EventTime.Date))
        //        {
        //            string exceptionString = Misc.dataLockedMessage(passTo.EventTime.Date);
        //            throw new Exception(exceptionString);
        //        }

        //        isUpdated = pdao.update(this.PssTO, true);

        //    }
        //    catch(Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " Pass.Update(): " + ex.Message + "\n");
        //        throw new Exception(ex.Message);
        //    }
        //    return isUpdated;
        //}

		public bool Update()
		{
			bool isUpdated = false;

			try
			{
                if (Misc.isLockedDate(this.PssTO.EventTime))
                {
                    string exceptionString = Misc.dataLockedMessage(this.PssTO.EventTime);
                    throw new Exception(exceptionString);
                }
				isUpdated = pdao.update(this.PssTO, true);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			return isUpdated;
		}

		public List<PassTO> Search()
		{
			try
			{				
				return pdao.getPasses(this.PssTO);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public List<PassTO> SearchInterval(DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime)
		{
			try
			{				
				return pdao.getPassesInterval(this.PssTO, fromTime, toTime, wUnits, advFromTime, advToTime);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.SearchInterval(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public Dictionary<int, Dictionary<DateTime, List<PassTO>>> SearchInterval(DateTime month, string emplIDs)
        {
            try
            {
                return pdao.getPassesInterval(month, emplIDs);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SearchInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
      
        public List<PassTO> SearchInterval(DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime, bool createDAO)
        {
            try
            {
                return pdao.getPassesInterval(this.PssTO, fromTime, toTime, wUnits, advFromTime, advToTime);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SearchInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public int SearchIntervalCount(DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime)
		{
			try
			{
				return pdao.getPassesIntervalCount(this.PssTO, fromTime, toTime, wUnits, advFromTime, advToTime);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.SearchIntervalCount(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public int SearchIntervalCountForZINReport(DateTime fromTime, DateTime toTime, string employees)
        {
            try
            {
                return pdao.getPassesIntervalCountForZINReport(fromTime, toTime, employees);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SearchIntervalCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, Dictionary<DateTime, List<PassTO>>> SearchPassesForEmployeesPeriod(string employeesID, DateTime from, DateTime to)
        {
            try
            {
                return pdao.getPassesForEmployeesPeriod(employeesID, from, to);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SearchPassesForEmployeesPeriod(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<PassTO> SearchIntervalForZINReport(DateTime fromTime, DateTime toTime, string employees)
        {
            try
            {
                return pdao.getPassesIntervalForZINReport(fromTime, toTime, employees);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SearchInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<PassTO> SearchForSnapshots(string passID)
        {
            try
            {                
                return pdao.getPassesForSnapshots(passID);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SearchInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public List<PassTO> SearchList()
		{
			try
			{
				return pdao.getPassesList(this.PssTO);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public List<PassTO> SearchPassesForExitPerm(int employeeID, DateTime eventTime)
		{
			try
			{				
				return pdao.getPassesForExitPerm(employeeID, eventTime);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.SearchPassesForExitPerm(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public List<PassTO> SearchPassesForExitPerm(int employeeID, DateTime startTime, int offset)
		{
			try
			{
				return pdao.getPassesForExitPerm(employeeID, startTime, offset);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.SearchPassesForExitPerm(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<PassTO> SearchPassForPerm(int employeeID, string direction, DateTime eventTime, int isWrkHrsCount)
        {        
            try
            {
                return pdao.getPassForPerm(employeeID, direction,eventTime,isWrkHrsCount);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SearchPassesForExitPerm(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public PassTO SearchPassBeforePermissionPass(int employeeID, DateTime eventTime)
		{
			try
			{
				return pdao.getPassBeforePermissionPass(employeeID, eventTime);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.SearchPassBeforePermissionPass(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public List<PassTO> SearchPermPassesForPair(int employeeID, DateTime startTime, DateTime endTime)
		{
			try
			{
				return pdao.getPermPassesForPair(employeeID, startTime, endTime);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.SearchPermPassesForPair(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        // Find current passes values for history of change
        public List<PassTO> FindCurrentPasses(DateTime fromTime, DateTime toTime, string wUnits, string modifiedBy)
        {
            try
            {
                return pdao.getCurrentPasses(this.PssTO, fromTime, toTime, wUnits, modifiedBy);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SearchPermPassesForPair(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }//Natasa 23.04.2008.

		/// <summary>
		/// Delete Pass with given ID
		/// </summary>
		/// <param name="passId">Pass ID</param>
		/// <returns>true if suc</returns>
		public bool Delete(string passId, bool doCommit,DateTime eventTime)
		{
			bool isDeleted = false;

			try
			{              
                if (Misc.isLockedDate(eventTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(eventTime.Date);
                    throw new Exception(exceptionString);
                }
				isDeleted = pdao.delete(passId, doCommit);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = pdao.beginTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                pdao.commitTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                pdao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return pdao.getTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                pdao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public PassTO Find(string passId)
		{
			try
			{
				return pdao.find(passId);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		public void Clear()
		{
			this.PssTO = new PassTO();
		}

        //Boris, vraca sve gate-ove u bazi
        List<GateTO> gatesList = new Gate().Search();

		public void CreatePassFromLog(List<LogTO> logRecs)
		{
            List<LogTO> logUpdateRecs = new List<LogTO>();
			List<PassTO> passInsertList = new List<PassTO>();
            
			int secLocation = 0;
			string secDirection = "";

			LogTO logTag = logRecs[0];
			LogTO logButton = null;
			
			if (logRecs.Count > 1)
			{
				logButton = logRecs[1];
			}

			try
			{
				// find reader
				ReaderTO currentReader = new ReaderTO();
				bool readerFound = false;

                if (readerList.ContainsKey(logTag.ReaderID))
                {
                    readerFound = true;
                    currentReader = readerList[logTag.ReaderID];
                }
                
				if (!readerFound)
				{
					logProcessingFailed(logRecs, "Record " + logTag.LogID + " Reader for this log not found. ");
					return;
				}

				// set reader data
				if (logTag.Antenna == 0)
				{
					this.PssTO.LocationID = currentReader.A0LocID;
					this.PssTO.Direction = currentReader.A0Direction;
					this.PssTO.IsWrkHrsCount = currentReader.A0IsCounter;
                    this.PssTO.GateID = currentReader.A0GateID;
					secLocation = currentReader.A0SecLocID;
					secDirection = currentReader.A0SecDirection;
				}
				else
				{
					this.PssTO.LocationID = currentReader.A1LocID;
					this.PssTO.Direction = currentReader.A1Direction;
					this.PssTO.IsWrkHrsCount = currentReader.A1IsCounter;
                    this.PssTO.GateID = currentReader.A1GateID;
					secLocation = currentReader.A1SecLocID;
					secDirection = currentReader.A1SecDirection;
				}

				PassTypeTO pt = new PassTypeTO();
				bool ptFound = false;

				// first search for Exit Permission for particular Employee and time of pass
				ExitPermissionTO exitPermission = new ExitPermissionTO();
				bool foundExitPerm = false;
				int ptID = 0;

				TagTO tagTO = new TagTO();

				if (tagTOListTest.ContainsKey(logTag.TagID))
				{
					tagTO = tagTOListTest[logTag.TagID];
				}

				// if direction is OUT, tag is ACTIVE or BLOCKED and exist permission for that out, 
				// pass type is permission pass type, and antenna (pass) IsWrkHrsCount == 1
				if ((this.PssTO.Direction.Equals(Constants.DirectionOut)) 
					&& (tagTO.TagID >= 0) && ((tagTO.Status.Equals(Constants.statusActive.Trim())) 
							|| (tagTO.Status.Equals(Constants.statusBlocked.Trim()))) &&
                            (this.PssTO.IsWrkHrsCount == 1))      // Darko, 13.12.2007.
				{
					foreach (ExitPermissionTO exitPerm in exitPermissionsList)
					{
						if ((exitPerm.EmployeeID == tagTO.OwnerID) && (logTag.EventTime >= exitPerm.StartTime)
                            && (logTag.EventTime <= exitPerm.StartTime.AddMinutes(exitPerm.Offset)))
						{
                            ptID = exitPerm.PassTypeID;
							exitPermission = exitPerm;
							foundExitPerm = true;
							break;
						}
					}
				}

				if (foundExitPerm)
				{
					this.PssTO.PassTypeID = ptID;
				}
				// if there is no exit permision found
				else 
				{
                    bool isTheSameGate = false; //Tamara 30.1.2020.
					// if Employee is EXTRAORDINARY or SPECIAL, and direction is OUT and there is no permission,
					// pass type is official out
					if ((this.PssTO.Direction.Equals(Constants.DirectionOut)) && 
                        // 27.06.2013. Sanja - only SPECIAL employees are treated in old way
                        // for EXTRAORDINARY employees, only absence is changed to regular work at the end
						//((extraOrdinaryEmployees.ContainsKey(tagTO.OwnerID) || specialEmployees.ContainsKey(tagTO.OwnerID))))
                        specialEmployees.ContainsKey(tagTO.OwnerID))
					{
						// If log belongs to work time interval with predefined offset
						// do not sign log as special out
                       
						// Get time schema for that day and get time intervals
						Dictionary<int, WorkTimeIntervalTO> list = this.getTimeSchemaInrvals(tagTO.OwnerID, logTag.EventTime);
                        WorkTimeSchemaTO schema = this.getTimeSchema(tagTO.OwnerID, logTag.EventTime);	
						// Pass trough time intervals and check is it belongs to interval
						WorkTimeIntervalTO interval = new WorkTimeIntervalTO();

						bool isOfficial = false;
                        // do it only if IsWrkHrs is 1
                        if (this.PssTO.IsWrkHrsCount == 1)
                        {
                            foreach (int intervalIndex in list.Keys)
                            {
                                interval = list[intervalIndex];

                                // Exit has happened before regular interval ends or regular interval starts
                                // set as official out
                                //							if (((logTag.EventTime.TimeOfDay.CompareTo
                                //								(interval.EndTime.AddMinutes(-Constants.officialOutOffset).TimeOfDay) <= 0)
                                //								&& (logTag.EventTime.TimeOfDay.CompareTo
                                //								(interval.EndTime.AddMinutes(-Constants.officialOutOffset).TimeOfDay) >= 0))
                                //								||(logTag.EventTime.TimeOfDay.CompareTo
                                //								(interval.EndTime.AddMinutes(-Constants.officialOutOffset).TimeOfDay) <= 0))

                                if (schema.Type.Equals(Constants.schemaTypeFlexi))
                                {
                                    if ((logTag.EventTime.TimeOfDay.CompareTo(interval.LatestLeft.TimeOfDay) <= 0)
                                    &&
                                   (logTag.EventTime.TimeOfDay.CompareTo(interval.EarliestArrived.TimeOfDay) >= 0))
                                    {
                                        CultureInfo ci = CultureInfo.InvariantCulture;
                                        List<LogTO> logList = new Log().getLogIn(tagTO.OwnerID, logTag.EventTime.Date);
                                        List<PassTO> passList = this.getPassesForEmployeeAll(tagTO.OwnerID, logTag.EventTime.Date);
                                        DateTime shiftStart = interval.LatestArrivaed;

                                        foreach (LogTO l in logList)
                                        {
                                            if (l.EventTime.TimeOfDay < shiftStart.TimeOfDay)
                                                shiftStart = l.EventTime;
                                            if (l.EventTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                                                shiftStart = interval.EarliestArrived;
                                        }

                                        foreach (PassTO p in passList)
                                        {
                                            if (p.EventTime.TimeOfDay < shiftStart.TimeOfDay)
                                                shiftStart = p.EventTime;
                                            if (p.EventTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                                                shiftStart = interval.EarliestArrived;
                                        }
                                        TimeSpan duration = interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                        DateTime endShift = shiftStart.AddMinutes(duration.TotalMinutes);

                                        if (logTag.EventTime.TimeOfDay <= endShift.TimeOfDay)
                                        {
                                            if(passTypeList.ContainsKey(Constants.officialOut))
                                            {
                                                ptFound = true;
                                                isOfficial = true;
                                                pt = passTypeList[Constants.officialOut];
                                            }                                            
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (((interval.StartTime.TimeOfDay < interval.EndTime.TimeOfDay)
                                        &&
                                        //19.02.2010. Natasa work without offset
                                        //(logTag.EventTime.TimeOfDay.CompareTo(interval.EndTime.AddMinutes(-Constants.officialOutOffset).TimeOfDay) <= 0)
                                        (logTag.EventTime.TimeOfDay.CompareTo(interval.EndTime.TimeOfDay) < 0)
                                        &&
                                       (logTag.EventTime.TimeOfDay.CompareTo(interval.StartTime.TimeOfDay) > 0)))
                                    //05.05.2010 Natasa separte regular time schema and flexi
                                    // ||(schema.Type.Equals(Constants.schemaTypeFlexi)&&
                                    // (logTag.EventTime.TimeOfDay.CompareTo(interval.LatestLeft.TimeOfDay) <= 0)
                                    // &&
                                    //(logTag.EventTime.TimeOfDay.CompareTo(interval.EarliestArrived.TimeOfDay) >= 0)))
                                    {
                                        if (passTypeList.ContainsKey(Constants.officialOut))
                                        {
                                            ptFound = true;
                                            isOfficial = true;
                                            pt = passTypeList[Constants.officialOut];
                                        }       
                                        break;
                                    }
                                }
                            }
                        }

						if (!isOfficial)
						{
							ptFound = true;
                            if (passTypeList.ContainsKey(Constants.regularWork))
                            {
                                ptFound = true;
                                isOfficial = true;
                                pt = passTypeList[Constants.regularWork];
                            }       
							
						}
					}
                    else if (logTag.ActionCommited > Constants.actionWhenButtonPressed)
					{
						// if action_commited is greater then 127 then this is a record representing button pressed,
						// action_commited - 127 field contains information about button pressed
						// find Pass Type for button pressed
						foreach(PassTypeTO pTypeTest in passTypeList.Values)
						{
                            if (pTypeTest.Button == (logTag.ActionCommited - Constants.actionWhenButtonPressed))
							{
								pt = pTypeTest;
								ptFound = true;
								break;                                
							}
						}
					}
					else if (logButton != null)
					{
						// if button is pressed, action_commited field contains information about button pressed
						// find Pass Type for button pressed
						foreach(PassTypeTO pTypeTest in passTypeList.Values)
						{
							if (pTypeTest.Button == logButton.ActionCommited)
							{
								pt = pTypeTest;
								ptFound = true;
								break;
							}
						}

						if (!ptFound)
						{
							// if there is no button pressed, default button value is 0
							// 0 represent regular IN/OUT
							foreach(PassTypeTO pTypeTest in passTypeList.Values)
							{
								if (pTypeTest.Button == 0)
								{
									pt = pTypeTest;
									ptFound = true;
									break;
								}
							}
						}
					}
					else
					{
                        // if there is no button pressed, default button value is 0
                        // 0 represent regular IN/OUT
                        //foreach(PassTypeTO pTypeTest in passTypeList.Values)
                        //{

                        //Boris, od sada tipove prolazaka gleda prema button-u iz tabele log, VIK, IMLEK i VIP,
                        //20170208, 0,1,2 i 3 ce traziti tipove prolazaka u pass_types tabeli

                        foreach (LogTO log in logRecs)
                        {
                            //Tamara 28.1.2020.
                            int readerID = log.ReaderID;
                            int gateID = pdao.getGate(readerID);
                            ulong tagID = log.TagID;
                            int organUnitID = pdao.getOrganUnitID(tagID); //nadjem organizacionu jedinicu na oanovu tag-a
                            isTheSameGate = pdao.compareGates(gateID, organUnitID);
                            
                           
                            switch (log.Button)
                            {
                                case (0):
                                    {
                                        foreach (PassTypeTO pTypeTest in passTypeList.Values)
                                        {
                                            if (log.Button == pTypeTest.Button)
                                                pt.PassTypeID = pTypeTest.PassTypeID;
                                        }
                                        //    pt.PassTypeID = pt.Button;
                                        ptFound = true;
                                        break;
                                    }

                                case (1):
                                    {
                                        foreach (PassTypeTO pTypeTest in passTypeList.Values)
                                        {
                                            if (log.Button == pTypeTest.Button)
                                                pt.PassTypeID = pTypeTest.PassTypeID;
                                        }
                                        //        pt.PassTypeID = pt.Button;
                                        ptFound = true;
                                        break;
                                    }

                                case (2):
                                    {
                                        foreach (PassTypeTO pTypeTest in passTypeList.Values)
                                        {
                                            if (log.Button == pTypeTest.Button)
                                                pt.PassTypeID = pTypeTest.PassTypeID;
                                        }
                                        //         pt.PassTypeID = pt.Button;
                                        ptFound = true;
                                        break;
                                    }
                                case (3):
                                    {
                                        foreach (PassTypeTO pTypeTest in passTypeList.Values)
                                        {
                                            if (log.Button == pTypeTest.Button)
                                                pt.PassTypeID = pTypeTest.PassTypeID;
                                        }
                                        //         pt.PassTypeID = pt.Button;
                                        ptFound = true;
                                        break;
                                    }
                            }
                            //Tamara 5.2.2020.
                           // 11.2.2020.
                            int restaurantReaderID = new Common.Rule().SearchReaderForRestaurant(Constants.RuleReaderForRestaurant);
                           
                            if(isTheSameGate && log.ReaderID==restaurantReaderID)
                            {
                                if (log.Antenna == 0)
                                {
                                    PssTO.Direction = Constants.DirectionOut;
                                }
                                else
                                    PssTO.Direction = Constants.DirectionIn;
                            }
                        }
					}

					if (!ptFound)
					{
						int button = (logButton != null) ? logButton.ActionCommited : 0;
						logProcessingFailed(logRecs, "Record " + logTag.LogID + " PassType not found for this button " 
							+ button.ToString() + " ");
						return;
					}

					if (pt.IsPass == 0)
					{
						debug.writeLog(DateTime.Now + " Pass.CreatePassFromLog(): Record " + logTag.LogID + " regular but it does not represent pass. ");
						return;
					}
					else
					{
                        //Tamara 30.1.2020.
                        if (!isTheSameGate)
                        {
                            this.PssTO.IsWrkHrsCount = 0;
                        }
                        
						this.PssTO.PassTypeID = pt.PassTypeID;
					}
				}

				TagTO currentTag = new TagTO();

				// Check if tag exists
				if (tagTOListTest.ContainsKey(logTag.TagID))
				{
					currentTag = tagTOListTest[logTag.TagID];
				}
				else
				{
					logProcessingFailed(logRecs, "Record " + logTag.LogID + " Tag not found for tag: " + logTag.TagID);
					return;
				}

				// update Employee Location record
				if ((logEmployees.ContainsKey(logTag.TagID)) && 
					(logEmployees[logTag.TagID].LogID == logTag.LogID))
				{
					new EmployeeLocation().Update(tagTOListTest[logTag.TagID].OwnerID, logTag.ReaderID,
						logTag.Antenna, this.PssTO.PassTypeID, logTag.EventTime, -1);
				}
				
				this.PssTO.EventTime = logTag.EventTime;
				this.PssTO.ManualyCreated = 0;
                if (Misc.isLockedDate(logTag.EventTime.Date))
                {
                    this.PssTO.PairGenUsed = (int)Constants.PairGenUsed.Obsolete;
                }
                else
                {
                    this.PssTO.PairGenUsed = 0;
                }

				this.PssTO.EmployeeID = currentTag.OwnerID;
				List<PassTO> passTOList = new List<PassTO>();
				
				PassTO passTO1 = this.PssTO;

				passInsertList.Add(passTO1);

				if (secLocation >= 0)
				{
					PassTO passTO2 = this.PssTO;
					passTO2.Direction = secDirection;
					passTO2.LocationID = secLocation;

					passInsertList.Add(passTO2);
				}

				// Change PassGenUsed value to 1 on Update
				foreach(LogTO log in logRecs)
				{
					log.PassGenUsed = (int) Constants.PassGenUsed.Used;
					logUpdateRecs.Add(log);
				}



				exitPermission.Used = (int) Constants.Used.Yes;

				// insert Pass, and update Log and Exit Permission
                try
                {
                    //Boris, ako antena za reader na kome se desilo ocitavanje odgovara definisanom
                    //gate-u tada ce se par dalje obradjivati. Bilo je moguce staviti i gate za ulaznu
                    //antenu => A0GateID i porediti sa njim, ali je dovoljna samo jedna antena, posto
                    //obe antene moraju biti na istom gate-u. 20170112
                    if(gatesForProcess.Contains(currentReader.A1GateID.ToString()))
                    pdao.log2pass(passInsertList, logUpdateRecs, exitPermission, daoFactory);
                }
                catch (Exception ex)
                {
                    List<LogTO> logList = new List<LogTO>();
                    foreach(LogTO l in logUpdateRecs)
                    {                        
                        logList.Add(l);
                    }
                    logProcessingFailed(logList, ex.Message);
                }
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.CreatePassFromLog(): " + ex.Message + "\n");
				throw ex;
			}

			return;
		}

		public void PopulatePasses(Dictionary<int,EmployeeTO> specialEmpl, Dictionary<int, EmployeeTO> extraOrdinaryEmpl)
		{
            Dictionary<long, LogTO> logDict = new Dictionary<long, LogTO>();
            List<LogTO> logRecs = new List<LogTO>();
										
			System.Console.WriteLine("Entering PopulatePasses(): " + DateTime.Now);
			System.Console.WriteLine("---------------------");

			try
			{
				LogTO logTO = new LogTO();

				logTO.EventHappened = (int) Constants.EventTag.eventTagAllowed;
				logTO.PassGenUsed = 0;

				System.Console.WriteLine("PopulatePasses() Starts populating ArrayLists : " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				debug.writeBenchmarkLog(" PopulatePasses(): populating array lists from DB: STARTED! +++++\n");


				// find all logs that are not used for creating passes
				logDict = new Log().Search((int) Constants.EventTag.eventTagAllowed, (int) Constants.EventTag.eventButtonPressed, 
					(int) Constants.PassGenUsed.Unused);

				//all readers
				readerList = new Reader().SearchDictionary();
				
				// all tags, Hashtable - Key is tag_id, value is Tag
				tagTOListTest = new Tag().SearchActive();

				// all PassTypes
				passTypeList = new PassType().SearchDictionary();
				
				// all exit_permissions that are not already used
                ExitPermission permission = new ExitPermission();
                permission.PermTO.Used = (int)Constants.Used.No;
				exitPermissionsList = permission.Search(new DateTime(0), new DateTime(0), "");

                extraOrdinaryEmployees = extraOrdinaryEmpl;
                specialEmployees = specialEmpl;

				// all employee locations records
				List<EmployeeLocationTO> emplLocs = new EmployeeLocation().SearchAll();
				foreach (EmployeeLocationTO emplLoc in emplLocs)
				{
					emplLocations.Add(emplLoc.EmployeeID, emplLoc);
				}

				System.Console.WriteLine("PopulatePasses() Ends populating ArrayLists: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				debug.writeBenchmarkLog(" PopulatePasses(): populating array lists from DB: FINISHED! +++++\n");

				System.Console.WriteLine("PopulatePasses() Start populating LocEmployees: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				debug.writeBenchmarkLog(" PopulatePasses(): populating LocEmployees: STARTED! +++++\n");

				// create Hashtbale - Key is tag_id, Value is log with greatest event_time for that employee
				int key = -1;
                List<LogTO> logList = new List<LogTO>(); ;
				foreach (uint logID in logDict.Keys)
				{
                    LogTO log = logDict[logID];
                    logList.Add(log);
					if (tagTOListTest.ContainsKey(log.TagID))
					{
						key = tagTOListTest[log.TagID].OwnerID;
					}
					else
					{
						key = -1;
					}

					if ((key >= 0) && (emplLocations.ContainsKey(key)) && 
						(emplLocations[key].EventTime < log.EventTime))
					{
						if (logEmployees.ContainsKey(log.TagID))
						{
							if (logEmployees[log.TagID].EventTime < log.EventTime)
							{
								logEmployees[log.TagID] = log;
							}
						}
						else
						{
							logEmployees.Add(log.TagID, log);
						}
					}
				}
				
				System.Console.WriteLine("PopulatePasses() Ends populating LocEmployees: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				debug.writeBenchmarkLog(" PopulatePasses(): populating LocEmployees: FINISHED! +++++\n");

				int i=0; 

				LogTO logTag = new LogTO();
				LogTO logButton = new LogTO();
				
				System.Console.WriteLine("Start to execute while ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");
				debug.writeBenchmarkLog(" PopulatePasses(): processing logList: STARTED! +++++\n");
				while (i < logList.Count)
				{
					logRecs.Clear();

					// First record must be tag record
					logTag = logList[i];

					if (logTag.TagID > 0)
					{
						logRecs.Add(logTag);
						if (i+1 < logList.Count)
						{
							logButton = logList[i+1];
							
							if (logButton.TagID == 0)
							{
								// Button is pressed
								logRecs.Add(logButton);
								i += 2;
							}
							else
							{
								i++;
							}
						}
						else
						{
							i++;
						}
						
						CreatePassFromLog(logRecs);
					}
					// Goto next record, unregular situation
					else
					{
						i++;
					}
				}
				System.Console.WriteLine("Ends to execute while ...: " + DateTime.Now);
				System.Console.WriteLine("---------------------");

				debug.writeBenchmarkLog(" PopulatePasses(): processing logList: FINISHED! +++++\n");
				debug.writeBenchmarkLog(" PopulatePasses(): processing exit permissions: STARTED! +++++\n");
				// inserting IN and special OUT for exit permissions 
				//that are issued for the beginning of working day
				// all exit_permissions that are not already used
                permission = new ExitPermission();
                permission.PermTO.Used = (int)Constants.Used.No;
                exitPermissionsList = permission.Search(new DateTime(0), new DateTime(0), "");
                debug.writeBenchmarkLog(" PopulatePasses(): processing exit permissions: SEARCH ENDED! \n");
                // work with data sets instead of working with database
                //DataSet dsTimeSchedules = new EmployeesTimeSchedule().GetTimeSchedules();

                foreach (ExitPermissionTO perm in exitPermissionsList)
				{
					Employee empl = new Employee();
					empl.EmplTO.EmployeeID = perm.EmployeeID;
                    
					ArrayList schema = empl.findTimeSchema(perm.StartTime.Date);
    				// work with data set instead of working with database
                    //ArrayList schema = empl.findTimeSchemaFromDataSet(perm.StartTime.Date, dsTimeSchedules);

					// if time schema is defined for that day
					if (schema.Count > 1)
					{
						Dictionary<int, WorkTimeIntervalTO> intervals =((WorkTimeSchemaTO) schema[0]).Days[(int) schema[1]];

						foreach (int intNum in intervals.Keys)
						{
							if ((intervals[intNum].EarliestArrived.TimeOfDay <= perm.StartTime.TimeOfDay)
								&& (intervals[intNum].LatestArrivaed.TimeOfDay >= perm.StartTime.TimeOfDay))
							{
								// if permission start time is between earliest and latest arrived, 
								// insert IN and special OUT pass
								// IN pass start time is beginning of interval, OUT is for 1 sec greater
								PassTO passIN = new PassTO(-1, perm.EmployeeID, Constants.DirectionIn, perm.StartTime,
									(int) Constants.PassType.Work, (int) Constants.PairGenUsed.Unused, Constants.defaultLocID,
									(int) Constants.recordCreated.Automaticaly, (int) Constants.IsWrkCount.IsCounter);

								DateTime eventTime = perm.StartTime.AddSeconds(1);
								PassTO passOUT = new PassTO(-1, perm.EmployeeID, Constants.DirectionOut, eventTime,
									perm.PassTypeID, (int) Constants.PairGenUsed.Unused, Constants.defaultLocID,
									(int) Constants.recordCreated.Automaticaly, (int) Constants.IsWrkCount.IsCounter);
								
								List<PassTO> passTOList = new List<PassTO>();
								passTOList.Add(passIN);
								passTOList.Add(passOUT);
								perm.Used = (int) Constants.Used.Yes;
                                
                                this.Save(passTOList, perm, Constants.PermPassUser);
                                
							}
						}
					}
				}
				debug.writeBenchmarkLog(" PopulatePasses(): processing exit permissions: FINISHED! +++++\n");
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.PopulatePasses(): " + ex.Message + "\n");
				throw ex;
			}
		}

		public void logProcessingFailed(List<LogTO> failedLogs, string message)
		{
			foreach(LogTO logMember in failedLogs)
			{
				logMember.PassGenUsed = (int) Constants.PassGenUsed.Unexpected;
				debug.writeLog(DateTime.Now + " " + message);

				new Log().Update(logMember, true);
			}
		}

		public ArrayList getEmployeesByDate()
		{
			ArrayList emplDays = new ArrayList();
			try
			{
				emplDays = pdao.getEmpoloyeesByDate();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.getEmployeesByDate(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return emplDays;
		}

		public List<PassTO> getPassesForEmployee(int empoyeeID, string date)
		{			
			try
			{
				return pdao.getPassesForEmployee(empoyeeID, date);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.getEmployeesByDate(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<PassTO> getPassesForEmployeeAll(int empoyeeID, DateTime date)
        {
            try
            {
                return pdao.getPassesForEmployeeAll(empoyeeID, date);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.getPassesForEmployeeAll(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }



        public List<PassTO> getPassesForEmployeeSched(int employeeID)
        {
            try
            {
                return pdao.getPassesForEmployeeSched(employeeID);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.getPassesForEmployeeAll(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
		/// <summary>
		/// Send list of PassTO objects to serialization
		/// </summary>
		/// <param name="PassesTOList">List of PassTO</param>
		public void CacheData(List<PassTO> PassesTOList)
		{
			try
			{
				pdao.serialize(PassesTOList);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.CacheData(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

	 	public void CacheAllData()
		{
			try
			{
				pdao.serialize(pdao.getPasses(new PassTO()));
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.CacheAllData(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        		
		public void CacheData()
		{
			try
			{
				List<PassTO> PassesTOList = pdao.getPasses(this.PssTO);
				
				this.CacheData(PassesTOList);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.CacheData(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Change current DAO Factory. Switch to XML data source.
		/// </summary>
		public void SwitchToXMLDAO()
		{
			try
			{
				daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
				pdao = daoFactory.getPassDAO(null);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public List<PassTO> GetFromXMLSource(string file)
		{
			try
			{
				return pdao.deserialize(file);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.PrepareUpdateData(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Gets work intervals for given employeeID for particular day
		/// </summary>
		/// <param name="employeeID">int EmployeeID</param>
		/// <param name="date">DateTime concrete date</param>
		/// <returns>List of time schema intervals</returns>
		private Dictionary<int, WorkTimeIntervalTO> getTimeSchemaInrvals(int employeeID, DateTime date)
		{
			EmployeesTimeSchedule empltimesch = new EmployeesTimeSchedule();
			EmployeeTimeScheduleTO currentSchedule = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
			int timeScheduleIndex = -1;
			TimeSchema schema = new TimeSchema();
			// Key is date, value is start day for schema for that day
			//Dictionary<DateTime, int> startDays = new Dictionary<DateTime,int>();

			// Key is date, value is start date for schema for that day
			//Dictionary<DateTime, DateTime> startDates = new Dictionary<DateTime,DateTime>();
            Dictionary<DateTime, WorkTimeSchemaTO> schemaForDay = new Dictionary<DateTime, WorkTimeSchemaTO>();
            Dictionary<int, WorkTimeIntervalTO> schemaIntervals = new Dictionary<int, WorkTimeIntervalTO>();

			try
			{
                empltimesch.EmplTSTO.EmployeeID = employeeID;
                timeScheduleList = empltimesch.Search();

				// Get Intervals for current day
				for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
				{
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
					if (date >= timeScheduleList[scheduleIndex].Date)
						//&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
					{
						currentSchedule = timeScheduleList[scheduleIndex];
						timeScheduleIndex = scheduleIndex;
					}
				}

				if (timeScheduleIndex >= 0)
				{
                    schema.TimeSchemaTO.TimeSchemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
					List<WorkTimeSchemaTO> schemas = schema.Search();
					//startDays.Add(currentSchedule.Date, ((EmployeesTimeSchedule) timeScheduleList[timeScheduleIndex]).StartCycleDay);
					//startDates.Add(currentSchedule.Date, ((EmployeesTimeSchedule) timeScheduleList[timeScheduleIndex]).Date);
					if (schemas.Count > 0)
					{
						schemaForDay.Add(date, schemas[0]);
					}
				}
				else
				{
					return schemaIntervals;
				}

				int dayNum = 0;

				// find Time Schema and day of that Schema 

                WorkTimeSchemaTO emplSchema = new WorkTimeSchemaTO();
                                
				if (schemaForDay.ContainsKey(date))
				{
                    emplSchema = schemaForDay[date];
					//dayNum = ((int) startDays[date] + date.Day - ((DateTime) startDates[date]).Day) % emplSchema.CycleDuration;
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
					//dayNum = (currentSchedule.StartCycleDay + date.Day - currentSchedule.Date.Day) % emplSchema.CycleDuration;
                    TimeSpan ts = new TimeSpan(date.Date.Ticks - currentSchedule.Date.Date.Ticks);
                    dayNum = (currentSchedule.StartCycleDay + (int)ts.TotalDays) % emplSchema.CycleDuration;
				}

                if (emplSchema.Days.ContainsKey(dayNum))
                    schemaIntervals = emplSchema.Days[dayNum];
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Pass.getTimeSchemaInrvals(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return schemaIntervals;
		}

        /// <summary>
        /// Gets work intervals for given employeeID for particular day
        /// </summary>
        /// <param name="employeeID">int EmployeeID</param>
        /// <param name="date">DateTime concrete date</param>
        /// <returns>time schema for employee</returns>
        private WorkTimeSchemaTO getTimeSchema(int employeeID, DateTime date)
        {
            EmployeesTimeSchedule empltimesch = new EmployeesTimeSchedule();
            EmployeeTimeScheduleTO currentSchedule = new EmployeeTimeScheduleTO();
            List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
            int timeScheduleIndex = -1;
            TimeSchema schema = new TimeSchema();
            // Key is date, value is start day for schema for that day
            //Dictionary<DateTime, int> startDays = new Dictionary<DateTime,int>();

            // Key is date, value is start date for schema for that day
            //Dictionary<DateTime, DateTime> startDates = new Dictionary<DateTime, DateTime>();
            Dictionary<DateTime, WorkTimeSchemaTO> schemaForDay = new Dictionary<DateTime, WorkTimeSchemaTO>();
            Dictionary<int, WorkTimeIntervalTO> schemaIntervals = new Dictionary<int,WorkTimeIntervalTO>();
            WorkTimeSchemaTO emplSchema = new WorkTimeSchemaTO();

            try
            {
                empltimesch.EmplTSTO.EmployeeID = employeeID;
                timeScheduleList = empltimesch.Search();

                // Get Intervals for current day
                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (date >= timeScheduleList[scheduleIndex].Date)
                    //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                    {
                        currentSchedule = timeScheduleList[scheduleIndex];
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    schema.TimeSchemaTO.TimeSchemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
                    List<WorkTimeSchemaTO> schemas = schema.Search();
                    //startDays.Add(currentSchedule.Date, ((EmployeesTimeSchedule)timeScheduleList[timeScheduleIndex]).StartCycleDay);
                    //startDates.Add(currentSchedule.Date, ((EmployeesTimeSchedule)timeScheduleList[timeScheduleIndex]).Date);
                    if (schemas.Count > 0)
                    {
                        schemaForDay.Add(date, schemas[0]);
                    }
                }
                else
                {
                    return emplSchema;
                }
                                
                // find Time Schema and day of that Schema 
                if (schemaForDay.ContainsKey(date))
                    emplSchema = schemaForDay[date];
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.getTimeSchemaInrvals(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return emplSchema;
        }
        
        public void UnlockPasses(DateTime startDate, DateTime endDate,bool doCommit)
        {
            try
            {
                pdao.UnlockPasses(startDate, endDate,doCommit);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.UnlockPasses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<PassTO> getDiferencePasses(DateTime lastReadingTime)
        {
            try
            {
              return pdao.getDiferencePasses(lastReadingTime);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.getDiferencePasses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<PassTO> getListOfIN_OUTforDay(DateTime date, int emplID, string direction)
        {
            try
            {
                return pdao.getListOfIN_OUTforDay(date, emplID, direction);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.getListOfIN_OUTforDay(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public void updateListPassesToWrkHrs0(string passIDs)
        {
            try
            {
                pdao.updateListPassesToWrkHrs0(passIDs);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.updateListPassesToWrkHrs0(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<PassTO> getPassesForDayForEmployee(int emplID, DateTime day)
        {
            try
            {
                return pdao.getPassesForDayForEmployee(emplID, day);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.getPassesForDayForEmployee(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void updatePassesOnUnprocessed(int emplID, DateTime date, bool doCommit)
        {
            try
            {
                pdao.updatePassesOnUnprocessed(emplID, date, doCommit);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Pass.updatePassesOnUnprocessed(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

    }
}
