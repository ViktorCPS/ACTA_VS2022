using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;


namespace Common
{
	/// <summary>
	/// Summary description for Readers.
	/// </summary>
	public class Reader
	{
		protected DAOFactory daoFactory = null;
		protected ReaderDAO readDao = null;
        
		DebugLog log;

        ReaderTO rTO = new ReaderTO();

        public ReaderTO RdrTO
        {
            get { return rTO; }
            set { rTO = value; }
        }
        
		public Reader()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			readDao = daoFactory.getReaderDAO(null);
		}

        public Reader(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            readDao = daoFactory.getReaderDAO(dbConnection);
        }

		public Reader(int readerId)
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			readDao = daoFactory.getReaderDAO(null);
			this.RdrTO.ReaderID = readerId;
		}

        public Reader(int readerId, string description, int A0GateID, int A1GateID, int downloadInterval, DateTime downloadStartTime)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.RdrTO.ReaderID = readerId;
            this.RdrTO.Description = description;
            this.RdrTO.A0GateID = A0GateID;
            this.RdrTO.A1GateID = A1GateID;
            this.RdrTO.DownloadInterval = downloadInterval;
            this.RdrTO.DownloadStartTime = downloadStartTime;
        }


        public int Save()
		{
			int inserted;

			try
			{
				inserted = readDao.insert(this.RdrTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}
        public Dictionary<int, ReaderTO> SearchDictionary()
        {
            try
            {
                return readDao.getReadersDictionary(this.RdrTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public List<ReaderTO> Search()
		{
            try
			{
				return readDao.getReaders(this.RdrTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.Search(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public List<ReaderTO> Search(string a0GateID)
		{		
			try
			{
				string[] gatesArray = a0GateID.Split(',');
                
				return readDao.getReaders(gatesArray);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.Search(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public List<ReaderTO> SearchAll()
        {
            try
            {               
                return readDao.getAllReaders();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.SearchAll(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<ReaderTO> SearchOnAntenna0()
        {
            try
            {
                return readDao.getReadersOnAntenna0();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.SearchOnAntenna0(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<ReaderTO> SearchLastReadTime()
		{
			try
			{
				return readDao.getReadersLastReadTime();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.SearchLastReadTime(): " + ex.Message + "\n");
				throw ex;
			}
		}

		public DateTime SearchAllReadersLastReadTime()
		{
			DateTime allReadersTime = new DateTime(0);
			try
			{
				 allReadersTime = readDao.getAllReadersLastReadTime();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.SearchAllReadersLastReadTime(): " + ex.Message + "\n");
				throw ex;
			}

			return allReadersTime;
		}

        public DateTime SearchLastLogUsed(int readerID, string direction)
        {
            try
            {
                return readDao.getLastLogUsed(readerID, direction);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.SearchLastLogUsed(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public List<ReaderTO> searchForIDs(string readerIds)
        {
            try
            {
                return readDao.searchForIDs(readerIds);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = readDao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                readDao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                readDao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return readDao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                readDao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
		public bool Update(ReaderTO rTO)
		{
			bool isUpdated = false;

			try
			{
				isUpdated = readDao.update(rTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.Update(): " + ex.Message + "\n");
				throw ex;
			}

			return isUpdated;
		}

		public bool Delete(int readerID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = readDao.delete(readerID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.Delete(): " + ex.Message + "\n");
				throw ex;

			}
			return isDeleted;
		}

		public LocationTO getLocation(int locId)
		{
			try
			{
				return new Location().Find(locId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.getLocation(): " + ex.Message + "\n");
				throw ex;
			}
		}

		public ReaderTO find(int readerID)
		{
			ReaderTO rTo = new ReaderTO();
			try
			{
				rTo = readDao.find(readerID.ToString());
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.find(): " + ex.Message + "\n");
				throw ex;
			}
			return rTo;
		}

		public int FindMAXReaderID()
		{
			int readerID = 0;

			try
			{
				readerID = readDao.findMAXReaderID();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.FindMAXReaderID(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return readerID;
		}

        public List<ReaderTO> getReadersOnLocation(int locationId)
		{
            List<ReaderTO> readerList = new List<ReaderTO>();
			
			try
			{
				// A0 primary Location
                this.RdrTO = new ReaderTO();
				this.RdrTO.A0LocID = locationId;
				readerList = this.Search();

				// A0 Secondary Location
                this.RdrTO = new ReaderTO();
                this.RdrTO.A0LocID = 0;
				this.RdrTO.A0SecLocID = locationId;
				readerList.AddRange(this.Search());

				// A1 Primary Location
                this.RdrTO = new ReaderTO();
                this.RdrTO.A0SecLocID = 0;
                this.RdrTO.A1LocID = locationId;
				readerList.AddRange(this.Search());

				// A1 Secondary Location
                this.RdrTO = new ReaderTO();
                this.RdrTO.A1LocID = 0;
                this.RdrTO.A1SecLocID = locationId;
				readerList.AddRange(this.Search());
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.getReadersOnLocation(): " + ex.Message + "\n");
				throw ex;
			}

			return readerList;
		}

		
		/// <summary>
		/// Send list of Reader objects to serialization.
		/// </summary>
		/// <param name="ReadersTOList">List of ReaderTO</param>
		private void CacheData(List<ReaderTO> ReadersTOList)
		{
			try
			{
				readDao.serialize(ReadersTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		public void CacheDataSearch(string readerID, string description, string connType, string connAddress,
			string a0GateID, string a0LocID, string a0Direction, string a0SecLocID, string a0SecDir, string a0IsCounter,
			string a1GateID, string a1LocID, string a1Direction, string a1SecLocID, string a1SecDir, string a1IsCounter,
			string techType)
		{
            List<ReaderTO> readerTOList = new List<ReaderTO>();

			try
			{
				string[] gatesArray = a0GateID.Split(',');                
				
				readerTOList = readDao.getReaders(gatesArray);
				this.CacheData(readerTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.CacheDataSearch(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		/// <summary>
		/// Cache all of the Readers from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				string[] gatesArray = new string[0]; 
				this.CacheData(readDao.getReaders(gatesArray));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.CacheAllData(): " + ex.Message + "\n");
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
				readDao = daoFactory.getReaderDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}

		public int GetReaderAddress()
		{
			int address = -1;

			try
			{
				if (this.RdrTO.ConnectionType.Equals(Constants.ConnTypeIP))
				{
					// Convert string to int
					try
					{
						IPAddress ip = IPAddress.Parse(this.RdrTO.ConnectionAddress.Trim());
						address = ip.GetAddressBytes()[3] + (ip.GetAddressBytes()[2] << 8) +
							(ip.GetAddressBytes()[1] << 16) + (ip.GetAddressBytes()[0] << 24);
					}
					catch(Exception exIP)
					{
						throw exIP;
					}
				}
				else if (this.RdrTO.ConnectionType.Equals(Constants.ConnTypeSerial))
				{
					address = Convert.ToInt32(this.RdrTO.ConnectionAddress);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Exception in: " + this.ToString() + 
					".GetReaderAddres() : " + ex.Message);  
			}

			return address;
		}

		public string GetDefaultTechnology()
		{
			string technlogy = "";
			Reader tempReader = new Reader();

			try
			{
				List<ReaderTO> readers = tempReader.Search();
				if (readers.Count > 0)
				{
					technlogy = readers[0].TechType;
				}
				else
				{
					technlogy = Constants.DefaultTechType;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " " + this.ToString() + 
					".GetDefaultTechnology(): " + ex.Message + "\n");
			}
			return technlogy;
		}


        public List<ReaderTO> getReaders(int locID, int gateID)
        {
            try
            {                
                return readDao.getReaders(locID,gateID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.getReaders(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<ReaderTO> getReadersForMap(int mapID)
        {           
            try
            {
                return readDao.getReadersForMap(mapID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.getReadersForMap(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update(ReaderTO readerTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = readDao.update(readerTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.Update(): " + ex.Message + "\n");
                throw ex;
            }

            return isUpdated;
        }

        public int Save(bool doCommit)
        {
            int inserted;

            try
            {
                inserted = readDao.insert(this.RdrTO,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

       
    }
}

