using System;
using System.Collections;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for GateTimeAccessProfile.
	/// </summary>
	public class GateTimeAccessProfile
	{
		private int     _gateTAProfileId = -1;
		private string  _name = "";
		private string  _description = "";
		private int     _gateTAProfile0 = -1;
		private int     _gateTAProfile1 = -1;
		private int     _gateTAProfile2 = -1;
		private int     _gateTAProfile3 = -1;
		private int     _gateTAProfile4 = -1;
		private int     _gateTAProfile5 = -1;
		private int     _gateTAProfile6 = -1;
		private int     _gateTAProfile7 = -1;
		private int     _gateTAProfile8 = -1;
		private int     _gateTAProfile9 = -1;
		private int     _gateTAProfile10 = -1;
		private int     _gateTAProfile11 = -1;
		private int     _gateTAProfile12 = -1;
		private int     _gateTAProfile13 = -1;
		private int     _gateTAProfile14 = -1;
		private int     _gateTAProfile15 = -1;

		DAOFactory daoFactory = null;
		GateTimeAccessProfileDAO gateTimeAccessProfileDAO = null;

		DebugLog log;

		public int GateTAProfileId
		{
			get { return _gateTAProfileId; }
			set { _gateTAProfileId = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public int GateTAProfile0
		{
			get { return _gateTAProfile0; }
			set { _gateTAProfile0 = value; }
		}

		public int GateTAProfile1
		{
			get { return _gateTAProfile1; }
			set { _gateTAProfile1 = value; }
		}

		public int GateTAProfile2
		{
			get { return _gateTAProfile2; }
			set { _gateTAProfile2 = value; }
		}

		public int GateTAProfile3
		{
			get { return _gateTAProfile3; }
			set { _gateTAProfile3 = value; }
		}

		public int GateTAProfile4
		{
			get { return _gateTAProfile4; }
			set { _gateTAProfile4 = value; }
		}

		public int GateTAProfile5
		{
			get { return _gateTAProfile5; }
			set { _gateTAProfile5 = value; }
		}

		public int GateTAProfile6
		{
			get { return _gateTAProfile6; }
			set { _gateTAProfile6 = value; }
		}

		public int GateTAProfile7
		{
			get { return _gateTAProfile7; }
			set { _gateTAProfile7 = value; }
		}

		public int GateTAProfile8
		{
			get { return _gateTAProfile8; }
			set { _gateTAProfile8 = value; }
		}

		public int GateTAProfile9
		{
			get { return _gateTAProfile9; }
			set { _gateTAProfile9 = value; }
		}

		public int GateTAProfile10
		{
			get { return _gateTAProfile10; }
			set { _gateTAProfile10 = value; }
		}

		public int GateTAProfile11
		{
			get { return _gateTAProfile11; }
			set { _gateTAProfile11 = value; }
		}

		public int GateTAProfile12
		{
			get { return _gateTAProfile12; }
			set { _gateTAProfile12 = value; }
		}

		public int GateTAProfile13
		{
			get { return _gateTAProfile13; }
			set { _gateTAProfile13 = value; }
		}

		public int GateTAProfile14
		{
			get { return _gateTAProfile14; }
			set { _gateTAProfile14 = value; }
		}

		public int GateTAProfile15
		{
			get { return _gateTAProfile15; }
			set { _gateTAProfile15 = value; }
		}

		public GateTimeAccessProfile()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			gateTimeAccessProfileDAO = daoFactory.getGateTimeAccessProfileDAO(null);
		}

        public GateTimeAccessProfile(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            gateTimeAccessProfileDAO = daoFactory.getGateTimeAccessProfileDAO(dbConnection);
        }

		public GateTimeAccessProfile(int gateTAProfileId, string name, string description,
			int gateTAProfile0, int gateTAProfile1, int gateTAProfile2, int gateTAProfile3,
			int gateTAProfile4, int gateTAProfile5, int gateTAProfile6, int gateTAProfile7,
			int gateTAProfile8, int gateTAProfile9, int gateTAProfile10, int gateTAProfile11,
			int gateTAProfile12, int gateTAProfile13, int gateTAProfile14, int gateTAProfile15)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.GateTAProfileId = gateTAProfileId;
			this.Name            = name;
			this.Description     = description;
			this.GateTAProfile0  = gateTAProfile0;
			this.GateTAProfile1  = gateTAProfile1;
			this.GateTAProfile2  = gateTAProfile2;
			this.GateTAProfile3  = gateTAProfile3;
			this.GateTAProfile4  = gateTAProfile4;
			this.GateTAProfile5  = gateTAProfile5;
			this.GateTAProfile6  = gateTAProfile6;
			this.GateTAProfile7  = gateTAProfile7;
			this.GateTAProfile8  = gateTAProfile8;
			this.GateTAProfile9  = gateTAProfile9;
			this.GateTAProfile10  = gateTAProfile10;
			this.GateTAProfile11  = gateTAProfile11;
			this.GateTAProfile12  = gateTAProfile12;
			this.GateTAProfile13  = gateTAProfile13;
			this.GateTAProfile14  = gateTAProfile14;
			this.GateTAProfile15  = gateTAProfile15;

			daoFactory = DAOFactory.getDAOFactory();
			gateTimeAccessProfileDAO = daoFactory.getGateTimeAccessProfileDAO(null);
		}

		public int Save(string name, string description, string gateTAProfile0, string gateTAProfile1,
			string gateTAProfile2, string gateTAProfile3, string gateTAProfile4, string gateTAProfile5,
			string gateTAProfile6, string gateTAProfile7, string gateTAProfile8, string gateTAProfile9,
			string gateTAProfile10, string gateTAProfile11, string gateTAProfile12,
			string gateTAProfile13, string gateTAProfile14, string gateTAProfile15)
		{
			int inserted;
			try
			{
				inserted = gateTimeAccessProfileDAO.insert(name, description, gateTAProfile0, gateTAProfile1,
					gateTAProfile2, gateTAProfile3, gateTAProfile4, gateTAProfile5, gateTAProfile6,
					gateTAProfile7, gateTAProfile8, gateTAProfile9, gateTAProfile10, gateTAProfile11,
					gateTAProfile12, gateTAProfile13, gateTAProfile14, gateTAProfile15);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public bool Update(string gateTimeAccessProfileId, string name, string description, 
			string gateTAProfile0, string gateTAProfile1, string gateTAProfile2, 
			string gateTAProfile3, string gateTAProfile4, string gateTAProfile5,
			string gateTAProfile6, string gateTAProfile7, string gateTAProfile8, 
			string gateTAProfile9, string gateTAProfile10, string gateTAProfile11, 
			string gateTAProfile12, string gateTAProfile13, string gateTAProfile14, 
			string gateTAProfile15, bool doCommit)
		{
			bool isUpdated;

			try
			{
				isUpdated = gateTimeAccessProfileDAO.update(gateTimeAccessProfileId, name, description,
					gateTAProfile0, gateTAProfile1, gateTAProfile2, gateTAProfile3, gateTAProfile4, 
					gateTAProfile5, gateTAProfile6, gateTAProfile7, gateTAProfile8, gateTAProfile9,
					gateTAProfile10, gateTAProfile11, gateTAProfile12, gateTAProfile13, gateTAProfile14, 
					gateTAProfile15, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string gateTimeAccessProfileId)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = gateTimeAccessProfileDAO.delete(gateTimeAccessProfileId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = gateTimeAccessProfileDAO.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				gateTimeAccessProfileDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				gateTimeAccessProfileDAO.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.RollbackTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return gateTimeAccessProfileDAO.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				gateTimeAccessProfileDAO.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public GateTimeAccessProfileTO Find(string gateTimeAccessProfileId)
		{
			GateTimeAccessProfileTO gateTimeAccessProfileTO = new GateTimeAccessProfileTO();

			try
			{
				gateTimeAccessProfileTO = gateTimeAccessProfileDAO.find(gateTimeAccessProfileId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return gateTimeAccessProfileTO;
		}

		public void ReceiveTransferObject(GateTimeAccessProfileTO gateTimeAccessProfileTO)
		{
			this.GateTAProfileId = gateTimeAccessProfileTO.GateTAProfileId;
			this.Name            = gateTimeAccessProfileTO.Name;
			this.Description     = gateTimeAccessProfileTO.Description;
			this.GateTAProfile0  = gateTimeAccessProfileTO.GateTAProfile0;
			this.GateTAProfile1  = gateTimeAccessProfileTO.GateTAProfile1;
			this.GateTAProfile2  = gateTimeAccessProfileTO.GateTAProfile2;
			this.GateTAProfile3  = gateTimeAccessProfileTO.GateTAProfile3;
			this.GateTAProfile4  = gateTimeAccessProfileTO.GateTAProfile4;
			this.GateTAProfile5  = gateTimeAccessProfileTO.GateTAProfile5;
			this.GateTAProfile6  = gateTimeAccessProfileTO.GateTAProfile6;
			this.GateTAProfile7  = gateTimeAccessProfileTO.GateTAProfile7;
			this.GateTAProfile8  = gateTimeAccessProfileTO.GateTAProfile8;
			this.GateTAProfile9  = gateTimeAccessProfileTO.GateTAProfile9;
			this.GateTAProfile10  = gateTimeAccessProfileTO.GateTAProfile10;
			this.GateTAProfile11  = gateTimeAccessProfileTO.GateTAProfile11;
			this.GateTAProfile12  = gateTimeAccessProfileTO.GateTAProfile12;
			this.GateTAProfile13  = gateTimeAccessProfileTO.GateTAProfile13;
			this.GateTAProfile14  = gateTimeAccessProfileTO.GateTAProfile14;
			this.GateTAProfile15  = gateTimeAccessProfileTO.GateTAProfile15;
		}

		public GateTimeAccessProfileTO SendTransferObject()
		{
			GateTimeAccessProfileTO gateTimeAccessProfileTO = new GateTimeAccessProfileTO();

			gateTimeAccessProfileTO.GateTAProfileId = this.GateTAProfileId;
			gateTimeAccessProfileTO.Name            = this.Name;
			gateTimeAccessProfileTO.Description     = this.Description;
			gateTimeAccessProfileTO.GateTAProfile0  = this.GateTAProfile0;
			gateTimeAccessProfileTO.GateTAProfile1  = this.GateTAProfile1;
			gateTimeAccessProfileTO.GateTAProfile2  = this.GateTAProfile2;
			gateTimeAccessProfileTO.GateTAProfile3  = this.GateTAProfile3;
			gateTimeAccessProfileTO.GateTAProfile4  = this.GateTAProfile4;
			gateTimeAccessProfileTO.GateTAProfile5  = this.GateTAProfile5;
			gateTimeAccessProfileTO.GateTAProfile6  = this.GateTAProfile6;
			gateTimeAccessProfileTO.GateTAProfile7  = this.GateTAProfile7;
			gateTimeAccessProfileTO.GateTAProfile8  = this.GateTAProfile8;
			gateTimeAccessProfileTO.GateTAProfile9  = this.GateTAProfile9;
			gateTimeAccessProfileTO.GateTAProfile10  = this.GateTAProfile10;
			gateTimeAccessProfileTO.GateTAProfile11  = this.GateTAProfile11;
			gateTimeAccessProfileTO.GateTAProfile12  = this.GateTAProfile12;
			gateTimeAccessProfileTO.GateTAProfile13  = this.GateTAProfile13;
			gateTimeAccessProfileTO.GateTAProfile14  = this.GateTAProfile14;
			gateTimeAccessProfileTO.GateTAProfile15  = this.GateTAProfile15;

			return gateTimeAccessProfileTO;
		}

		public void Clear()
		{
			this.GateTAProfileId = -1;
			this.Name            = "";
			this.Description     = "";
			this.GateTAProfile0  = -1;
			this.GateTAProfile1  = -1;
			this.GateTAProfile2  = -1;
			this.GateTAProfile3  = -1;
			this.GateTAProfile4  = -1;
			this.GateTAProfile5  = -1;
			this.GateTAProfile6  = -1;
			this.GateTAProfile7  = -1;
			this.GateTAProfile8  = -1;
			this.GateTAProfile9  = -1;
			this.GateTAProfile10  = -1;
			this.GateTAProfile11  = -1;
			this.GateTAProfile12  = -1;
			this.GateTAProfile13  = -1;
			this.GateTAProfile14  = -1;
			this.GateTAProfile15  = -1;
		}

		public ArrayList Search(string name)
		{
			ArrayList gateTimeAccessProfileTOList = new ArrayList();
			ArrayList gateTimeAccessProfileList = new ArrayList();

			try
			{
				GateTimeAccessProfile gateTimeAccessProfileMember = new GateTimeAccessProfile();
				gateTimeAccessProfileTOList = gateTimeAccessProfileDAO.getGateTimeAccessProfile(name);

				foreach(GateTimeAccessProfileTO gtapTO in gateTimeAccessProfileTOList)
				{
					gateTimeAccessProfileMember = new GateTimeAccessProfile();
					gateTimeAccessProfileMember.ReceiveTransferObject(gtapTO);
					
					gateTimeAccessProfileList.Add(gateTimeAccessProfileMember);
				}				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return gateTimeAccessProfileList;
		}

		private void CacheData(ArrayList gateTimeAccessProfileTOList)
		{
			try
			{
				gateTimeAccessProfileDAO.serialize(gateTimeAccessProfileTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/*
		 * public void CacheData(string description, string fromDate, string toDate)
		{
			ArrayList gateTimeAccessProfileTOList = new ArrayList();

			try
			{
				gateTimeAccessProfileTOList = gateTimeAccessProfileDAO.getGateTimeAccessProfile(description, fromDate, toDate);
				this.CacheData(gateTimeAccessProfileTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the Holidays from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(gateTimeAccessProfileDAO.getGateTimeAccessProfile("", "", ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.CacheAllData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		 */

		/// <summary>
		/// Change DAO, start to use XML data source
		/// </summary>
		public void SwitchToXMLDAO()
		{
			try
			{
				daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
				gateTimeAccessProfileDAO = daoFactory.getGateTimeAccessProfileDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfile.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
