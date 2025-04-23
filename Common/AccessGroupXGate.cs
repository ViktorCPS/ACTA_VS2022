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
	/// Summary description for AccessGroupXGate.
	/// </summary>
	public class AccessGroupXGate
	{
		private int   _accessGroupID = -1;
		private int   _gateID = -1;
		private int   _gateTimeAccessProfile = -1;
		private int   _reader_access_group_ord_num = -1;

		DAOFactory daoFactory = null;
		AccessGroupXGateDAO accessGroupXGateDAO = null;

		DebugLog log;

		public int AccessGroupID
		{
			get { return _accessGroupID; }
			set { _accessGroupID = value; }
		}

		public int GateID
		{
			get { return _gateID; }
			set { _gateID = value; }
		}

		public int GateTimeAccessProfile
		{
			get { return _gateTimeAccessProfile; }
			set { _gateTimeAccessProfile = value; }
		}

		public int ReaderAccessGroupOrdNum
		{
			get { return _reader_access_group_ord_num; }
			set { _reader_access_group_ord_num = value; }
		}

		public AccessGroupXGate()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			accessGroupXGateDAO = daoFactory.getAccessGroupXGateDAO(null);
		}
        public AccessGroupXGate(Object dbConnection)
        {
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			accessGroupXGateDAO = daoFactory.getAccessGroupXGateDAO(dbConnection);
		}
		public AccessGroupXGate(int accessGroupID, int gateID, int gateTimeAccessProfile,
			int readerAccessGroupOrdNum)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.AccessGroupID           = accessGroupID;
			this.GateID                  = gateID;
			this.GateTimeAccessProfile   = gateTimeAccessProfile;
			this.ReaderAccessGroupOrdNum = readerAccessGroupOrdNum;

			daoFactory = DAOFactory.getDAOFactory();
			accessGroupXGateDAO = daoFactory.getAccessGroupXGateDAO(null);
		}

		public int Save(string accessGroupID, string gateID, string gateTimeAccessProfile, 
			string readerAccessGroupOrdNum, bool doCommit)
		{
			int inserted;
			try
			{
				inserted = accessGroupXGateDAO.insert(accessGroupID, gateID, gateTimeAccessProfile, readerAccessGroupOrdNum, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public bool Update(string accessGroupID, string gateID, string gateTimeAccessProfile, 
			string readerAccessGroupOrdNum, bool doCommit)
		{
			bool isUpdated;

			try
			{
				isUpdated = accessGroupXGateDAO.update(accessGroupID, gateID, gateTimeAccessProfile, readerAccessGroupOrdNum, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string accessGroupID, string gateID, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = accessGroupXGateDAO.delete(accessGroupID, gateID, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}	
	
		public bool DeleteGates(string gateID, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = accessGroupXGateDAO.deleteGates(gateID, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = accessGroupXGateDAO.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				accessGroupXGateDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				accessGroupXGateDAO.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.RollbackTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return accessGroupXGateDAO.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				accessGroupXGateDAO.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public AccessGroupXGateTO Find(string accessGroupID, string gateID)
		{
			AccessGroupXGateTO accessGroupXGateTO = new AccessGroupXGateTO();

			try
			{
				accessGroupXGateTO = accessGroupXGateDAO.find(accessGroupID, gateID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return accessGroupXGateTO;
		}

		public void ReceiveTransferObject(AccessGroupXGateTO accessGroupXGateTO)
		{
			this.AccessGroupID           = accessGroupXGateTO.AccessGroupID;
			this.GateID                  = accessGroupXGateTO.GateID;
			this.GateTimeAccessProfile   = accessGroupXGateTO.GateTimeAccessProfile;
			this.ReaderAccessGroupOrdNum = accessGroupXGateTO.ReaderAccessGroupOrdNum;
		}

		public AccessGroupXGateTO SendTransferObject()
		{
			AccessGroupXGateTO accessGroupXGateTO = new AccessGroupXGateTO();

			accessGroupXGateTO.AccessGroupID           = this.AccessGroupID;
			accessGroupXGateTO.GateID                  = this.GateID;
			accessGroupXGateTO.GateTimeAccessProfile   = this.GateTimeAccessProfile;
			accessGroupXGateTO.ReaderAccessGroupOrdNum = this.ReaderAccessGroupOrdNum;

			return accessGroupXGateTO;
		}

		public void Clear()
		{
			this.AccessGroupID           = -1;
			this.GateID                  = -1;
			this.GateTimeAccessProfile   = -1;
			this.ReaderAccessGroupOrdNum = -1;
		}

		public ArrayList Search(string accessGroupID, string gateID)
		{
			ArrayList accessGroupXGateTOList = new ArrayList();
			ArrayList accessGroupXGateList   = new ArrayList();

			try
			{
				AccessGroupXGate accessGroupXGateMember = new AccessGroupXGate();
				accessGroupXGateTOList = accessGroupXGateDAO.getAccessGroupXGate(accessGroupID, gateID);

				foreach(AccessGroupXGateTO agXgTO in accessGroupXGateTOList)
				{
					accessGroupXGateMember = new AccessGroupXGate();
					accessGroupXGateMember.ReceiveTransferObject(agXgTO);
					
					accessGroupXGateList.Add(accessGroupXGateMember);
				}
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return accessGroupXGateList;
		}

		private void CacheData(ArrayList accessGroupXGateTOList)
		{
			try
			{
				accessGroupXGateDAO.serialize(accessGroupXGateTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/*
		 * public void CacheData(string description, string fromDate, string toDate)
		{
			ArrayList accessGroupXGateTOList = new ArrayList();

			try
			{
				accessGroupXGateTOList = accessGroupXGateDAO.getAccessGroupXGate(description, fromDate, toDate);
				this.CacheData(accessGroupXGateTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.CacheData(): " + ex.Message + "\n");
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
				this.CacheData(accessGroupXGateDAO.getAccessGroupXGate("", "", ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.CacheAllData(): " + ex.Message + "\n");
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
				accessGroupXGateDAO = daoFactory.getAccessGroupXGateDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupXGate.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
