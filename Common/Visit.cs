using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;
using System.Collections.Generic;

namespace Common
{
	/// <summary>
	/// Summary description for Visit.
	/// </summary>
	public class Visit
	{
		private int _visitID;
		private int _employeeID;
		private string _firstName;
		private string _lastName;
		private string _visitorJMBG;
		private string _visitorID;
		private DateTime _dateStart;
		private DateTime _dateEnd;
		private int _visitedPerson;
		private int _visitedWorkingUnit;
		private string _visitDescr;
		private int _locationID;
		private string _remarks;
        private string _employeeFirstName;
        private string _employeeLastName;
        private string _visitedFirstName;
        private string _visitedLastName;
        private string _wuName;
        private string _company;

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }
		
		private ArrayList readerList = new ArrayList();
		private Hashtable tagTOListTest = new Hashtable();
		private ArrayList passTypeList = new ArrayList();

		DAOFactory daoFactory = null;
		VisitDAO vdao = null;

		DebugLog debug;

		public int VisitID
		{
			get{ return _visitID; }
			set{ _visitID = value; }
		}

		public int EmployeeID
		{
			get{ return _employeeID; }
			set{ _employeeID = value; }
		}

		public string FirstName
		{
			get{ return _firstName; }
			set{ _firstName = value; }
		}

		public string LastName
		{
			get{ return _lastName; }
			set{ _lastName = value; }
		}

		public string VisitorJMBG
		{
			get{ return _visitorJMBG; }
			set{ _visitorJMBG = value; }
		}

		public string VisitorID
		{
			get{ return _visitorID; }
			set{ _visitorID = value; }
		}

		public DateTime DateStart
		{
			get{ return _dateStart; }
			set{ _dateStart = value; }
		}

		public DateTime DateEnd
		{
			get{ return _dateEnd; }
			set{ _dateEnd = value; }
		}

		public int VisitedPerson
		{ 
			get { return _visitedPerson; }
			set { _visitedPerson = value; }
		}

		public int VisitedWorkingUnit
		{
			get{ return _visitedWorkingUnit; }
			set{ _visitedWorkingUnit = value; }
		}

		public string VisitDescr
		{
			get{ return _visitDescr; }
			set{ _visitDescr = value; }
		}

		public int LocationID
		{
			get{ return _locationID; }
			set{ _locationID = value; }
		}

		public string Remarks
		{
			get{ return _remarks; }
			set{ _remarks = value; }
		}

        public string EmployeeFirstName
        {
            get { return _employeeFirstName; }
            set { _employeeFirstName = value; }
        }

        public string EmployeeLastName
        {
            get { return _employeeLastName; }
            set { _employeeLastName = value; }
        }

        public string VisitedFirstName
        {
            get { return _visitedFirstName; }
            set { _visitedFirstName = value; }
        }

        public string VisitedLastName
        {
            get { return _visitedLastName; }
            set { _visitedLastName = value; }
        }

        public string WUName
        {
            get { return _wuName; }
            set { _wuName = value; }
        }

		public Visit()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			vdao = daoFactory.getVisitDAO(null);

			this.VisitID = -1;
			this.EmployeeID = -1;
			this.FirstName = "";
			this.LastName = "";
			this.VisitorJMBG = "";
			this.VisitorID = "";
			this.DateStart = new DateTime();
			this.DateEnd = new DateTime();
			this.VisitedPerson = -1;
			this.VisitedWorkingUnit = -1;
			this.VisitDescr = "";
			this.LocationID = -1;
			this.Remarks = "";
            this.EmployeeFirstName = "";
            this.EmployeeLastName = "";
            this.VisitedFirstName = "";
            this.VisitedLastName = "";
            this.WUName = "";
            this.Company = "";
		}

        public Visit(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            vdao = daoFactory.getVisitDAO(dbConnection);

            this.VisitID = -1;
            this.EmployeeID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.VisitorJMBG = "";
            this.VisitorID = "";
            this.DateStart = new DateTime();
            this.DateEnd = new DateTime();
            this.VisitedPerson = -1;
            this.VisitedWorkingUnit = -1;
            this.VisitDescr = "";
            this.LocationID = -1;
            this.Remarks = "";
            this.EmployeeFirstName = "";
            this.EmployeeLastName = "";
            this.VisitedFirstName = "";
            this.VisitedLastName = "";
            this.WUName = "";
            this.Company = "";
        }

		public Visit(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID,
            string remarks, string employeeFirstName, string employeeLastName, string visitedFirstName, string visitedLastName,
            string wuName)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			vdao = daoFactory.getVisitDAO(null);

			this.VisitID = visitID;
			this.EmployeeID = employeeID;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.VisitorJMBG = visitorJMBG;
			this.VisitorID = visitorID;
			this.DateStart = dateStart;
			this.DateEnd = dateEnd;
			this.VisitedPerson = visitedPerson;
			this.VisitedWorkingUnit = visitedWorkingUnit;
			this.VisitDescr = visitDescr;
			this.LocationID = locationID;
			this.Remarks = remarks;
            this.EmployeeFirstName = employeeFirstName;
            this.EmployeeLastName = employeeLastName;
            this.VisitedFirstName = visitedFirstName;
            this.VisitedLastName = visitedLastName;
            this.WUName = wuName;
            this.Company = "";//15.12.2009 Company propertie has defalt value
		}

		public int Save(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
		{
			int saved = 0;
			try
			{
				// TODO: cerate TO and send to DAO
				saved = vdao.insert(visitID, employeeID, firstName, lastName, visitorJMBG, visitorID, 
							dateStart, dateEnd, visitedPerson, visitedWorkingUnit, visitDescr, locationID, remarks);

			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.Save(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return saved;
		}

        
        public int Save(VisitTO visitTo, bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = vdao.insert(visitTo, doCommit);
            }
            catch(Exception ex)
            {
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
                {
                    debug.writeLog(DateTime.Now + " Visit.Save(): Record already exist!   Primary Key Violation SqlException.Number" + ex.Message + "\n");
                }
                else
                {
                    debug.writeLog(DateTime.Now + " Visit.Save(): " + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
            }

            return saved;
			
        }
        

        public int Save(VisitTO visitTo)
		{
			int saved = 0;
			try
			{
				saved = vdao.insert(visitTo);
			}
			catch(Exception ex)
			{
				//if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
				{
					debug.writeLog(DateTime.Now + " Visit.Save(): Record already exist!   Primary Key Violation SqlException.Number: " + ex.Message + "\n"
						+ " ----- Data: \n " 
						+ "VisitID:" + visitTo.VisitID + ", \n" 
						+ "DateStart: " + visitTo.DateStart.ToString() + ", \n"
						+ "DateEnd: " + visitTo.DateEnd.ToString() + ", \n"
						+ "EmployeeID: " + visitTo.EmployeeID.ToString() + ", \n"
						+ "FirstName: " + visitTo.FirstName + ", \n"
						+ "LastName: " + visitTo.LastName + ", \n" 
						+ " ----------- \n");
				}
				else
				{
					debug.writeLog(DateTime.Now + " Visit.Save(): " + ex.Message + "\n");
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
				savedRecord = this.Save(this.sendTransferObject());
			}
			catch(Exception ex)
			{
				//if (ex.Message.Trim().Equals(ConfigurationManager.AppSettings["SQLExceptionPrimaryKeyViolation"]))
                if (ex.Message.Trim().Equals(Constants.SQLExceptionPrimaryKeyViolation.ToString()))
				{
					debug.writeLog(DateTime.Now + " Visit.Save(): Record already exist!   Primary Key Violation SqlException.Number: " + ex.Message + "\n"
						+ " ----- Data: \n " 
						+ "VisitID:" + this.VisitID.ToString() + ", \n" 
						+ "DateStart: " + this.DateStart.ToString() + ", \n"
						+ "DateEnd: " + this.DateEnd.ToString() + ", \n"
						+ "EmployeeID: " + this.EmployeeID.ToString() + ", \n"
						+ "FirstName: " + this.FirstName + ", \n"
						+ "LastName: " + this.LastName + ", \n" 
						+ " ----------- \n");
				}
				else
				{
					debug.writeLog(DateTime.Now + " Visit.Save(): " + ex.Message + "\n");
					throw new Exception(ex.Message);
				}
			}

			return savedRecord;

		}
        public bool updateNameForJMBG(string fristName, string lastName, string jmbg)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = vdao.update(fristName,lastName,jmbg);

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return isUpdated;
        }
		public bool Update(VisitTO visitTo)
		{
			bool isUpdated = false;

			try
			{
				isUpdated = vdao.update(visitTo);

			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			return isUpdated;
		}
        public bool UpdateVisit(VisitTO visitTo)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = vdao.updateVisit(visitTo);

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return isUpdated;
        }
        public ArrayList SearchNotCompleted(bool notCompleted)
        {
            ArrayList visitTOList = new ArrayList();
            ArrayList visitList = new ArrayList();

            try
            {
                Visit visitMember = new Visit();
                visitTOList = vdao.getVisitsNotCompleted(notCompleted);

                foreach (VisitTO visitTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(visitTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visits.SearchNotCompleted(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitList;
        }

        public DataSet SearchCompletedDS()
        {
            DataSet ds = new DataSet();
            try
            {
                
                ds = vdao.getVisitsCompletedDS();

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visits.SearchCompletedDS(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return ds;
        }

        public ArrayList SearchCompleted()
        {
            ArrayList visitTOList = new ArrayList();
            ArrayList visitList = new ArrayList();

            try
            {
                Visit visitMember = new Visit();
                visitTOList = vdao.getVisitsCompleted();

                foreach (VisitTO visitTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(visitTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return visitList;
        }

		public ArrayList Search(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
		{
			ArrayList visitTOList = new ArrayList();
			ArrayList visitList = new ArrayList();

			try
			{
				Visit visitMember = new Visit();
				visitTOList = vdao.getVisits(visitID, employeeID, firstName, lastName, visitorJMBG, visitorID, 
			                  dateStart, dateEnd, visitedPerson, visitedWorkingUnit, visitDescr, locationID, remarks);

				foreach(VisitTO visitTO in visitTOList)
				{
					visitMember = new Visit();
					visitMember.receiveTransferObject(visitTO);

					visitList.Add(visitMember);
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return visitList;
		}

        public ArrayList SearchUNIPROM(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID,
            DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
        {
            ArrayList visitTOList = new ArrayList();
            ArrayList visitList = new ArrayList();

            try
            {
                Visit visitMember = new Visit();
                visitTOList = vdao.getVisitsUNIPROM(visitID, employeeID, firstName, lastName, visitorJMBG, visitorID,
                              dateStart, dateEnd, visitedPerson, visitedWorkingUnit, visitDescr, locationID, remarks);

                foreach (VisitTO visitTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(visitTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return visitList;
        }
        public ArrayList SearchDateInterval(string employeeID, string visitedWorkingUnit, string visitedPerson, string visitorIdent,
            DateTime dateFrom, DateTime dateTo, string visitDescr, string wUnits, string state, string company, string visitor, string privacy,string visitAsco4,  Dictionary<int,VisitAsco4> asco)
        {
            ArrayList visitTOList = new ArrayList();
            ArrayList visitList = new ArrayList();
            try
            {
                Visit visitMember = new Visit();
                VisitAsco4 visitAsco4Member = new VisitAsco4();
                Dictionary<int, List<VisitAsco4TO>> ascoTO = new Dictionary<int, List<VisitAsco4TO>>();
                visitTOList = vdao.getVisitsDateInterval(employeeID, visitedWorkingUnit,
                    visitedPerson, visitorIdent, dateFrom, dateTo, visitDescr, wUnits,state,company,visitor,privacy,visitAsco4,ascoTO);

                foreach (VisitTO visitTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(visitTO);

                    visitList.Add(visitMember);
                }
                foreach (int i in ascoTO.Keys)
                {
                    visitAsco4Member = new VisitAsco4();
                    VisitAsco4TO vaTO = ascoTO[i][0];
                    visitAsco4Member.ReceiveTransferObject(vaTO);
                    if (!asco.ContainsKey(i))
                    {
                        asco.Add(i,visitAsco4Member );
                    }                   
                    
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.SearchDateInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitList;
        }

        public ArrayList SearchDateIntervalDistinct(string employeeID, string visitedWorkingUnit, string visitorID, string visitorIdent,
           DateTime dateFrom, DateTime dateTo, string visitDescr, string wUnits, string state, string company, string visitor, string privacy, string visitAsco4, Dictionary<int, VisitAsco4> asco)
        {
            ArrayList visitTOList = new ArrayList();
            ArrayList visitList = new ArrayList();
            try
            {
                Visit visitMember = new Visit();
                VisitAsco4 visitAsco4Member = new VisitAsco4();
                Dictionary<int, List<VisitAsco4TO>> ascoTO = new Dictionary<int, List<VisitAsco4TO>>();
                visitTOList = vdao.getVisitsDateIntervalDistinct(employeeID, visitedWorkingUnit,
                    visitorID, visitorIdent, dateFrom, dateTo, visitDescr, wUnits, state, company, visitor, privacy, visitAsco4, ascoTO);

                foreach (VisitTO visitTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(visitTO);

                    visitList.Add(visitMember);
                }
                foreach (int i in ascoTO.Keys)
                {
                    visitAsco4Member = new VisitAsco4();
                    VisitAsco4TO vaTO = ascoTO[i][0];
                    visitAsco4Member.ReceiveTransferObject(vaTO);
                    if (!asco.ContainsKey(i))
                    {
                        asco.Add(i, visitAsco4Member);
                    }

                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.SearchDateInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitList;
        }


        public ArrayList SearchDateInterval(string employeeID, string visitedWorkingUnit,
            string visitedPerson, string visitorIdent, DateTime dateFrom, DateTime dateTo,
            string visitDescr, string wUnits)
        {
            ArrayList visitTOList = new ArrayList();
            ArrayList visitList = new ArrayList();

            try
            {
                Visit visitMember = new Visit();
                visitTOList = vdao.getVisitsDateInterval(employeeID, visitedWorkingUnit,
                    visitedPerson, visitorIdent, dateFrom, dateTo, visitDescr, wUnits);

                foreach (VisitTO visitTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(visitTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.SearchDateInterval(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return visitList;
        }

        public int SearchDateIntervalCount(string employeeID, string visitedWorkingUnit,
            string visitedPerson, string visitorIdent, DateTime dateFrom, DateTime dateTo,
            string visitDescr, string wUnits)
        {
            int count = 0;

            try
            {
                count = vdao.getVisitsDateIntervalCount(employeeID, visitedWorkingUnit,
                    visitedPerson, visitorIdent, dateFrom, dateTo, visitDescr, wUnits);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.SearchDateIntervalCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

		/// <summary>
		/// Delete Visit with given ID
		/// </summary>
		/// <param name="passId">Pass ID</param>
		/// <returns>true if suc</returns>
		public bool Delete(string visitId)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = vdao.delete(visitId);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;

		}

		public VisitTO Find(string visitId)
		{
			VisitTO visitTO = new VisitTO();

			try
			{
				visitTO = vdao.find(visitId);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return visitTO;
		}

        public VisitTO FindMAXVisitPIN(string PIN)
        {
            VisitTO visitTO = new VisitTO();

            try
            {
                visitTO = vdao.findMAXVisitPIN(PIN);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.FindMAXVisitPIN(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return visitTO;
        }

        public VisitTO FindMAXVisitIdCard(string idCard)
        {
            VisitTO visitTO = new VisitTO();

            try
            {
                visitTO = vdao.findMAXVisitIdCard(idCard);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.FindMAXVisitIdCard(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return visitTO;
        }

		public void receiveTransferObject(VisitTO visitTo)
		{
			this.VisitID = visitTo.VisitID;
			this.EmployeeID = visitTo.EmployeeID;
			this.FirstName = visitTo.FirstName;
			this.LastName = visitTo.LastName;
			this.VisitorJMBG = visitTo.VisitorJMBG;
			this.VisitorID = visitTo.VisitorID;
			this.DateStart = visitTo.DateStart;
			this.DateEnd = visitTo.DateEnd;
			this.VisitedPerson = visitTo.VisitedPerson;
			this.VisitedWorkingUnit = visitTo.VisitedWorkingUnit;
			this.VisitDescr = visitTo.VisitDescr;
			this.LocationID = visitTo.LocationID;
			this.Remarks = visitTo.Remarks;
            this.EmployeeFirstName = visitTo.EmployeeFirstName;
            this.EmployeeLastName = visitTo.EmployeeLastName;
            this.VisitedFirstName = visitTo.VisitedFirstName;
            this.VisitedLastName = visitTo.VisitedLastName;
            this.WUName = visitTo.WUName;
            this.Company = visitTo.Company;
		}

		public VisitTO sendTransferObject()
		{
			VisitTO visitTo = new VisitTO();

			visitTo.VisitID = this.VisitID;
			visitTo.EmployeeID = this.EmployeeID;
			visitTo.FirstName = this.FirstName;
			visitTo.LastName = this.LastName;
			visitTo.VisitorJMBG = this.VisitorJMBG;
			visitTo.VisitorID = this.VisitorID;
			visitTo.DateStart = this.DateStart;
			visitTo.DateEnd = this.DateEnd;
			visitTo.VisitedPerson = this.VisitedPerson;
			visitTo.VisitedWorkingUnit = this.VisitedWorkingUnit;
			visitTo.VisitDescr = this.VisitDescr;
			visitTo.LocationID = this.LocationID;
			visitTo.Remarks = this.Remarks;
            visitTo.EmployeeFirstName = this.EmployeeFirstName;
            visitTo.EmployeeLastName = this.EmployeeLastName;
            visitTo.VisitedFirstName = this.VisitedFirstName;
            visitTo.VisitedLastName = this.VisitedLastName;
            visitTo.WUName = this.WUName;
            visitTo.Company = this.Company;

			return visitTo;
		}

		public void Clear()
		{
			this.VisitID = -1;
			this.EmployeeID = -1;
			this.FirstName = "";
			this.LastName = "";
			this.VisitorJMBG = "";
			this.VisitorID = "";
			this.DateStart = new DateTime();
			this.DateEnd = new DateTime();
			this.VisitedPerson = -1;
			this.VisitedWorkingUnit = -1;
			this.VisitDescr = "";
			this.LocationID = -1;
			this.Remarks = "";
            this.EmployeeFirstName = "";
            this.EmployeeLastName = "";
            this.VisitedFirstName = "";
            this.VisitedLastName = "";
            this.WUName = "";
		}

        public ArrayList getCurrentVisits(string wUnits)
		{
			ArrayList currVisits = new ArrayList();
			try
			{
				currVisits = vdao.getCurrentVisits(wUnits);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.getCurrentVsits(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return currVisits;
		}

        public ArrayList GetCurrentVisitsDetail(string wUnits)
        {
            ArrayList visitTOList = new ArrayList();
            ArrayList visitList = new ArrayList();

            try
            {
                Visit visitMember = new Visit();
                visitTOList = vdao.getCurrentVisitsDetail(wUnits);

                foreach (VisitTO visitTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(visitTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.GetCurrentVisitsDetail(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return visitList;
        }

        public ArrayList SearchAll(string wuString)
        {
            ArrayList visitTOList = new ArrayList();
            ArrayList visitList = new ArrayList();

            try
            {
                Visit visitMember = new Visit();
                visitTOList = vdao.getAllVisits(wuString);

                foreach (VisitTO visitTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(visitTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visit.SearchAll(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return visitList;
        }

		/// <summary>
		/// Send list of VisitTO objects to serialization
		/// </summary>
		/// <param name="VisitTOList">List of VisitTO</param>
		public void CacheData(ArrayList VisitTOList)
		{
			try
			{
				vdao.serialize(VisitTOList);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.CacheAllData(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void CacheAllDataFromDataTable(DataTable table, string path)
		{
			ArrayList VisitTOList = new ArrayList();
			try
			{ 
				
				for ( int i=0; i<table.Rows.Count; i++ )
				{
					VisitTO visit = new VisitTO();
					visit.VisitID = -1;
					visit.EmployeeID = int.Parse(table.Rows[i]["employee_id"].ToString());
					visit.FirstName = table.Rows[i]["first_name"].ToString();
					visit.LastName = table.Rows[i]["last_name"].ToString();
					visit.VisitorJMBG =  table.Rows[i]["visitor_jmbg"].ToString();
					visit.VisitorID = table.Rows[i]["visitor_id"].ToString();
					visit.DateStart = DateTime.Parse(table.Rows[i]["date_start"].ToString());
					visit.DateEnd = DateTime.Parse(table.Rows[i]["date_end"].ToString());
					visit.VisitedPerson = int.Parse(table.Rows[i]["visited_person"].ToString());
					visit.VisitedWorkingUnit = int.Parse(table.Rows[i]["visited_working_unit"].ToString());
					visit.VisitDescr = table.Rows[i]["visit_descr"].ToString();
					visit.LocationID = int.Parse(table.Rows[i]["location_id"].ToString());
					visit.Remarks = table.Rows[i]["remarks"].ToString();
					VisitTOList.Add(visit);					
				}
				vdao.serialize(VisitTOList, path);
				
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.CacheAllDataFromDataTable(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

	 	public void CacheAllData()
		{
			try
			{
				vdao.serialize(vdao.getVisits(-1, -1, "", "", "", "", new DateTime(), 
						new DateTime(), -1, -1, "", -1, ""));
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.CacheAllData(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		
		public void CacheData(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks)
		{
			ArrayList VisitsTOList = new ArrayList();

			try
			{
				VisitsTOList = vdao.getVisits(visitID, employeeID, firstName, lastName, 
					visitorJMBG, visitorID, dateStart, 
					dateEnd, visitedPerson, visitedWorkingUnit, 
					visitDescr, locationID, remarks);
				
				this.CacheData(VisitsTOList);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.CacheAllData(): " + ex.Message + "\n");
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
				vdao = daoFactory.getVisitDAO(null);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public ArrayList GetFromXMLSource(string file)
		{
			ArrayList visitTOList = new ArrayList();
			ArrayList visitList = new ArrayList();

			try
			{
				visitTOList = vdao.deserialize(file);
				Visit visitMember = new Visit();

				foreach(VisitTO visitTO in visitTOList)
				{
					visitMember = new Visit();
					visitMember.receiveTransferObject(visitTO);

					visitList.Add(visitMember);
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Visit.GetFromXMLSource(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return visitList;
		}

        public List<string> SearchDistinctCompany()
        {
            List<string> visitCompanies = new List<string>();
            try
            {
                visitCompanies = vdao.getDistinctCompany();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " VisitAsco4.SearchDistinctName(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitCompanies;
        }

         public List<string> SearchDistinctName()
        {
            List<string> visitTypes = new List<string>();
            try
            {
                visitTypes = vdao.getDistinctName();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " VisitAsco4.SearchDistinctName(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitTypes;
        }

        public System.Collections.Generic.List<string> SearchDistinctVisitType()
        {
            List<string> visitTypes = new List<string>();
            try
            {
                visitTypes = vdao.getDistinctVisitType();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " VisitAsco4.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitTypes;
        }
        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = vdao.beginTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Employee.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                vdao.commitTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                vdao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return vdao.getTransaction();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Employee.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                vdao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Visits.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }





       
    }
}
