using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for Gate.
	/// </summary>
	public class Gate
	{
		DAOFactory daoFactory = null;
		GateDAO gateDAO = null;

		DebugLog log;

        GateTO gTO = new GateTO();

		public GateTO GTO
		{
            get { return gTO; }
			set{ gTO = value; }
		}
        
		public Gate()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			gateDAO = daoFactory.getGateDAO(null);
		}
        public Gate(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            gateDAO = daoFactory.getGateDAO(dbConnection);
        }

		public Gate(int gateID, string name, string description, DateTime downloadStartTime,
		int downloadInterval, int downloadEraseCounter)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			gateDAO = daoFactory.getGateDAO(null);

			this.GTO.GateID = gateID;
			this.GTO.Name = name;
			this.GTO.Description = description;
			this.GTO.DownloadStartTime = downloadStartTime;
			this.GTO.DownloadInterval = downloadInterval;
			this.GTO.DownloadEraseCounter = downloadEraseCounter;
		}

		public int Save(string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter)
		{
			int inserted;
			try
			{
				inserted = gateDAO.insert(name, description, downloadStartTime, downloadInterval, downloadEraseCounter);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public List<GateTO> Search()
		{
			try
			{				
				return gateDAO.getGates(this.GTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public List<GateTO> SearchGetGateTAProfile()
		{
			try
			{				
				return gateDAO.getGatesGetGateTAProfile();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.SearchGetGateTAProfile(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public GateTO FindGetAccessProfile(string gateID)
		{
			try
			{
				return gateDAO.findGetAccessProfile(gateID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.FindGetAccessProfile(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool UpdateGateTAProfile(string  gateID, string gateTAProfileID, bool doCommit)
		{
			bool isUpdated = false;

			try
			{
				isUpdated = gateDAO.updateGateTAProfile(gateID, gateTAProfileID, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.UpdateGateTAProfile(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;		
		}

		public List<GateTO> SearchWithGateTAProfile(string gateTimeaccessProfileId)
		{
			try
			{
				return gateDAO.getGatesWithGateTAProfile(gateTimeaccessProfileId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.SearchWithGateTAProfile(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(int gateID, string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter)
		{
			bool isUpdated;

			try
			{
				isUpdated = gateDAO.update(gateID, name, description, downloadStartTime, downloadInterval, downloadEraseCounter);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(int gateID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = gateDAO.delete(gateID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = gateDAO.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				gateDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				gateDAO.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.RollbackTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return gateDAO.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				gateDAO.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public GateTO Find(int gateID)
		{
			GateTO gateTO = new GateTO();

			try
			{
				gateTO = gateDAO.find(gateID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return gateTO;
		}

        public List<GateTO> SerchForLocation(int locationID)
        {        
            try
            {
                return gateDAO.getGatesForLocation(locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Gate.SerchForLocation(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<GateTO> SerchForLocationEnabled(int locationID)
        {
            try
            {
                return gateDAO.getGatesForLocationEnabled(locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Gate.SerchForLocation(): " + ex.Message + "\n");
                throw ex;
            }
        }
        
		public void Clear()
		{
            this.GTO = new GateTO();
		}

		
		/// <summary>
		/// Send list of GateTO objects to serialization.
		/// </summary>
		/// <param name="locatioTOList">List of LocationTO</param>
		private void CacheData(List<GateTO> gateTOList)
		{
			try
			{
				gateDAO.serialize(gateTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}		
		
		public void CacheData()
		{
			List<GateTO> gateTOList = new List<GateTO>();

			try
			{
				gateTOList = gateDAO.getGates(this.GTO);
				this.CacheData(gateTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}		
		
		/// <summary>
		/// Cache all of the Gates from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(gateDAO.getGates(new GateTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.CacheAllData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/// <summary>
		/// Change DAO, start to use XML data source
		/// </summary>
		public void SwitchToXMLDAO()
		{
			try
			{
                daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
				gateDAO = daoFactory.getGateDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gate.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public List<GateTO> getGatesForMap(int mapID)
        {
            try
            {
                return gateDAO.getGatesForMap(mapID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Gate.getReadersForMap(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public int Save(string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter,bool doCommit)
        {
            int inserted;
            try
            {
                inserted = gateDAO.insert(name, description, downloadStartTime, downloadInterval, downloadEraseCounter,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Gate.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }
    }
}
