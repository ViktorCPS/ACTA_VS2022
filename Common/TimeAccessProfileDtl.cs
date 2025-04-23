using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for TimeAccessProfileDtl.
	/// </summary>
	public class TimeAccessProfileDtl
	{
		private int     _timeAccessProfileId = -1;
		private int     _dayOfWeek = -1;
		private string  _direction = "";
		private int     _hrs0 = -1;
		private int     _hrs1 = -1;
		private int     _hrs2 = -1;
		private int     _hrs3 = -1;
		private int     _hrs4 = -1;
		private int     _hrs5 = -1;
		private int     _hrs6 = -1;
		private int     _hrs7 = -1;
		private int     _hrs8 = -1;
		private int     _hrs9 = -1;
		private int     _hrs10 = -1;
		private int     _hrs11 = -1;
		private int     _hrs12 = -1;
		private int     _hrs13 = -1;
		private int     _hrs14 = -1;
		private int     _hrs15 = -1;
		private int     _hrs16 = -1;
		private int     _hrs17 = -1;
		private int     _hrs18 = -1;
		private int     _hrs19 = -1;
		private int     _hrs20 = -1;
		private int     _hrs21 = -1;
		private int     _hrs22 = -1;
		private int     _hrs23 = -1;

		DAOFactory daoFactory = null;
		TimeAccessProfileDtlDAO timeAccessProfileDtlDAO = null;

		DebugLog log;

		public int TimeAccessProfileId
		{
			get { return _timeAccessProfileId; }
			set { _timeAccessProfileId = value; }
		}

		public int DayOfWeek
		{
			get { return _dayOfWeek; }
			set { _dayOfWeek = value; }
		}

		public string Direction
		{
			get { return _direction; }
			set { _direction = value; }
		}

		public int Hrs0
		{
			get { return _hrs0; }
			set { _hrs0 = value; }
		}

		public int Hrs1
		{
			get { return _hrs1; }
			set { _hrs1 = value; }
		}

		public int Hrs2
		{
			get { return _hrs2; }
			set { _hrs2 = value; }
		}

		public int Hrs3
		{
			get { return _hrs3; }
			set { _hrs3 = value; }
		}

		public int Hrs4
		{
			get { return _hrs4; }
			set { _hrs4 = value; }
		}

		public int Hrs5
		{
			get { return _hrs5; }
			set { _hrs5 = value; }
		}

		public int Hrs6
		{
			get { return _hrs6; }
			set { _hrs6 = value; }
		}

		public int Hrs7
		{
			get { return _hrs7; }
			set { _hrs7 = value; }
		}

		public int Hrs8
		{
			get { return _hrs8; }
			set { _hrs8 = value; }
		}

		public int Hrs9
		{
			get { return _hrs9; }
			set { _hrs9 = value; }
		}

		public int Hrs10
		{
			get { return _hrs10; }
			set { _hrs10 = value; }
		}

		public int Hrs11
		{
			get { return _hrs11; }
			set { _hrs11 = value; }
		}

		public int Hrs12
		{
			get { return _hrs12; }
			set { _hrs12 = value; }
		}

		public int Hrs13
		{
			get { return _hrs13; }
			set { _hrs13 = value; }
		}

		public int Hrs14
		{
			get { return _hrs14; }
			set { _hrs14 = value; }
		}

		public int Hrs15
		{
			get { return _hrs15; }
			set { _hrs15 = value; }
		}

		public int Hrs16
		{
			get { return _hrs16; }
			set { _hrs16 = value; }
		}

		public int Hrs17
		{
			get { return _hrs17; }
			set { _hrs17 = value; }
		}

		public int Hrs18
		{
			get { return _hrs18; }
			set { _hrs18 = value; }
		}

		public int Hrs19
		{
			get { return _hrs19; }
			set { _hrs19 = value; }
		}

		public int Hrs20
		{
			get { return _hrs20; }
			set { _hrs20 = value; }
		}

		public int Hrs21
		{
			get { return _hrs21; }
			set { _hrs21 = value; }
		}

		public int Hrs22
		{
			get { return _hrs22; }
			set { _hrs22 = value; }
		}

		public int Hrs23
		{
			get { return _hrs23; }
			set { _hrs23 = value; }
		}

		public TimeAccessProfileDtl()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			timeAccessProfileDtlDAO = daoFactory.getTimeAccessProfileDtlDAO(null);
		}

        public TimeAccessProfileDtl(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            timeAccessProfileDtlDAO = daoFactory.getTimeAccessProfileDtlDAO(dbConnection);
        }

		public TimeAccessProfileDtl(int timeAccessProfileId, int dayOfWeek, string direction, int hrs0,
			int hrs1, int hrs2, int hrs3, int hrs4, int hrs5, int hrs6, int hrs7, int hrs8, int hrs9, 
			int hrs10, int hrs11, int hrs12, int hrs13, int hrs14, int hrs15, int hrs16, int hrs17, int hrs18, 
			int hrs19, int hrs20, int hrs21, int hrs22, int hrs23)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.TimeAccessProfileId = timeAccessProfileId;
			this.DayOfWeek           = dayOfWeek;
			this.Direction           = direction;
			this.Hrs0                = hrs0;
			this.Hrs1                = hrs1;
			this.Hrs2                = hrs2;
			this.Hrs3                = hrs3;
			this.Hrs4                = hrs4;
			this.Hrs5                = hrs5;
			this.Hrs6                = hrs6;
			this.Hrs7                = hrs7;
			this.Hrs8                = hrs8;
			this.Hrs9                = hrs9;
			this.Hrs10               = hrs10;
			this.Hrs11               = hrs11;
			this.Hrs12               = hrs12;
			this.Hrs13               = hrs13;
			this.Hrs14               = hrs14;
			this.Hrs15               = hrs15;
			this.Hrs16               = hrs16;
			this.Hrs17               = hrs17;
			this.Hrs18               = hrs18;
			this.Hrs19               = hrs19;
			this.Hrs20               = hrs20;
			this.Hrs21               = hrs21;
			this.Hrs22               = hrs22;
			this.Hrs23               = hrs23;

			daoFactory = DAOFactory.getDAOFactory();
			timeAccessProfileDtlDAO = daoFactory.getTimeAccessProfileDtlDAO(null);
		}

		public int Save(string timeAccessProfileId, string dayOfWeek, string direction, string hrs0,
			string hrs1, string hrs2, string hrs3, string hrs4, string hrs5, string hrs6, string hrs7, string hrs8, string hrs9, 
			string hrs10, string hrs11, string hrs12, string hrs13, string hrs14, string hrs15, string hrs16, string hrs17, string hrs18, 
			string hrs19, string hrs20, string hrs21, string hrs22, string hrs23, bool doCommit)
		{
			int inserted;
			try
			{
				inserted = timeAccessProfileDtlDAO.insert(timeAccessProfileId, dayOfWeek, direction, hrs0,
					hrs1, hrs2, hrs3, hrs4, hrs5, hrs6, hrs7, hrs8, hrs9, hrs10, hrs11, hrs12, hrs13, hrs14, 
					hrs15, hrs16, hrs17, hrs18, hrs19, hrs20, hrs21, hrs22, hrs23, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public bool Update(string timeAccessProfileId, string dayOfWeek, string direction, string hrs0,
			string hrs1, string hrs2, string hrs3, string hrs4, string hrs5, string hrs6, string hrs7, string hrs8, string hrs9, 
			string hrs10, string hrs11, string hrs12, string hrs13, string hrs14, string hrs15, string hrs16, string hrs17, string hrs18, 
			string hrs19, string hrs20, string hrs21, string hrs22, string hrs23, bool doCommit)
		{
			bool isUpdated;

			try
			{
				isUpdated = timeAccessProfileDtlDAO.update(timeAccessProfileId, dayOfWeek, direction, hrs0,
					hrs1, hrs2, hrs3, hrs4, hrs5, hrs6, hrs7, hrs8, hrs9, hrs10, hrs11, hrs12, hrs13, hrs14, 
					hrs15, hrs16, hrs17, hrs18, hrs19, hrs20, hrs21, hrs22, hrs23, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string timeAccessProfileId, string dayOfWeek, string direction, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = timeAccessProfileDtlDAO.delete(timeAccessProfileId, dayOfWeek, direction, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}	
	
		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = timeAccessProfileDtlDAO.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				timeAccessProfileDtlDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				timeAccessProfileDtlDAO.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return timeAccessProfileDtlDAO.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				timeAccessProfileDtlDAO.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public TimeAccessProfileDtlTO Find(string timeAccessProfileId, string dayOfWeek, string direction)
		{
			TimeAccessProfileDtlTO timeAccessProfileDtlTO = new TimeAccessProfileDtlTO();

			try
			{
				timeAccessProfileDtlTO = timeAccessProfileDtlDAO.find(timeAccessProfileId, dayOfWeek, direction);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return timeAccessProfileDtlTO;
		}

		public void ReceiveTransferObject(TimeAccessProfileDtlTO timeAccessProfileDtlTO)
		{
			this.TimeAccessProfileId = timeAccessProfileDtlTO.TimeAccessProfileId;
			this.DayOfWeek           = timeAccessProfileDtlTO.DayOfWeek;
			this.Direction           = timeAccessProfileDtlTO.Direction;
			this.Hrs0                = timeAccessProfileDtlTO.Hrs0;
			this.Hrs1                = timeAccessProfileDtlTO.Hrs1;
			this.Hrs2                = timeAccessProfileDtlTO.Hrs2;
			this.Hrs3                = timeAccessProfileDtlTO.Hrs3;
			this.Hrs4                = timeAccessProfileDtlTO.Hrs4;
			this.Hrs5                = timeAccessProfileDtlTO.Hrs5;
			this.Hrs6                = timeAccessProfileDtlTO.Hrs6;
			this.Hrs7                = timeAccessProfileDtlTO.Hrs7;
			this.Hrs8                = timeAccessProfileDtlTO.Hrs8;
			this.Hrs9                = timeAccessProfileDtlTO.Hrs9;
			this.Hrs10               = timeAccessProfileDtlTO.Hrs10;
			this.Hrs11               = timeAccessProfileDtlTO.Hrs11;
			this.Hrs12               = timeAccessProfileDtlTO.Hrs12;
			this.Hrs13               = timeAccessProfileDtlTO.Hrs13;
			this.Hrs14               = timeAccessProfileDtlTO.Hrs14;
			this.Hrs15               = timeAccessProfileDtlTO.Hrs15;
			this.Hrs16               = timeAccessProfileDtlTO.Hrs16;
			this.Hrs17               = timeAccessProfileDtlTO.Hrs17;
			this.Hrs18               = timeAccessProfileDtlTO.Hrs18;
			this.Hrs19               = timeAccessProfileDtlTO.Hrs19;
			this.Hrs20               = timeAccessProfileDtlTO.Hrs20;
			this.Hrs21               = timeAccessProfileDtlTO.Hrs21;
			this.Hrs22               = timeAccessProfileDtlTO.Hrs22;
			this.Hrs23               = timeAccessProfileDtlTO.Hrs23;
		}

		public TimeAccessProfileDtlTO SendTransferObject()
		{
			TimeAccessProfileDtlTO timeAccessProfileDtlTO = new TimeAccessProfileDtlTO();

			timeAccessProfileDtlTO.TimeAccessProfileId = this.TimeAccessProfileId;
			timeAccessProfileDtlTO.DayOfWeek           = this.DayOfWeek;
			timeAccessProfileDtlTO.Direction           = this.Direction;
			timeAccessProfileDtlTO.Hrs0                = this.Hrs0;
			timeAccessProfileDtlTO.Hrs1                = this.Hrs1;
			timeAccessProfileDtlTO.Hrs2                = this.Hrs2;
			timeAccessProfileDtlTO.Hrs3                = this.Hrs3;
			timeAccessProfileDtlTO.Hrs4                = this.Hrs4;
			timeAccessProfileDtlTO.Hrs5                = this.Hrs5;
			timeAccessProfileDtlTO.Hrs6                = this.Hrs6;
			timeAccessProfileDtlTO.Hrs7                = this.Hrs7;
			timeAccessProfileDtlTO.Hrs8                = this.Hrs8;
			timeAccessProfileDtlTO.Hrs9                = this.Hrs9;
			timeAccessProfileDtlTO.Hrs10               = this.Hrs10;
			timeAccessProfileDtlTO.Hrs11               = this.Hrs11;
			timeAccessProfileDtlTO.Hrs12               = this.Hrs12;
			timeAccessProfileDtlTO.Hrs13               = this.Hrs13;
			timeAccessProfileDtlTO.Hrs14               = this.Hrs14;
			timeAccessProfileDtlTO.Hrs15               = this.Hrs15;
			timeAccessProfileDtlTO.Hrs16               = this.Hrs16;
			timeAccessProfileDtlTO.Hrs17               = this.Hrs17;
			timeAccessProfileDtlTO.Hrs18               = this.Hrs18;
			timeAccessProfileDtlTO.Hrs19               = this.Hrs19;
			timeAccessProfileDtlTO.Hrs20               = this.Hrs20;
			timeAccessProfileDtlTO.Hrs21               = this.Hrs21;
			timeAccessProfileDtlTO.Hrs22               = this.Hrs22;
			timeAccessProfileDtlTO.Hrs23               = this.Hrs23;

			return timeAccessProfileDtlTO;
		}

		public void Clear()
		{
			this.TimeAccessProfileId = -1;
			this.DayOfWeek           = -1;
			this.Direction           = "";
			this.Hrs0                = -1;
			this.Hrs1                = -1;
			this.Hrs2                = -1;
			this.Hrs3                = -1;
			this.Hrs4                = -1;
			this.Hrs5                = -1;
			this.Hrs6                = -1;
			this.Hrs7                = -1;
			this.Hrs8                = -1;
			this.Hrs9                = -1;
			this.Hrs10               = -1;
			this.Hrs11               = -1;
			this.Hrs12               = -1;
			this.Hrs13               = -1;
			this.Hrs14               = -1;
			this.Hrs15               = -1;
			this.Hrs16               = -1;
			this.Hrs17               = -1;
			this.Hrs18               = -1;
			this.Hrs19               = -1;
			this.Hrs20               = -1;
			this.Hrs21               = -1;
			this.Hrs22               = -1;
			this.Hrs23               = -1;
		}

		public ArrayList Search(string timeAccessProfileId)
		{
			ArrayList timeAccessProfileDtlTOList = new ArrayList();
			ArrayList timeAccessProfileDtlList = new ArrayList();

			try
			{
				TimeAccessProfileDtl timeAccessProfileDtlMember = new TimeAccessProfileDtl();
				timeAccessProfileDtlTOList = timeAccessProfileDtlDAO.getTimeAccessProfileDtl(timeAccessProfileId);

				foreach(TimeAccessProfileDtlTO tapTO in timeAccessProfileDtlTOList)
				{
					timeAccessProfileDtlMember = new TimeAccessProfileDtl();
					timeAccessProfileDtlMember.ReceiveTransferObject(tapTO);
					
					timeAccessProfileDtlList.Add(timeAccessProfileDtlMember);
				}
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			//return timeAccessProfileDtlList;
			return timeAccessProfileDtlTOList; //need for serialization
		}

		private void CacheData(ArrayList timeAccessProfileDtlTOList)
		{
			try
			{
				timeAccessProfileDtlDAO.serialize(timeAccessProfileDtlTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/*
		 * public void CacheData(string description, string fromDate, string toDate)
		{
			ArrayList timeAccessProfileDtlTOList = new ArrayList();

			try
			{
				timeAccessProfileDtlTOList = timeAccessProfileDtlDAO.getTimeAccessProfileDtl(description, fromDate, toDate);
				this.CacheData(timeAccessProfileDtlTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.CacheData(): " + ex.Message + "\n");
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
				this.CacheData(timeAccessProfileDtlDAO.getTimeAccessProfileDtl("", "", ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.CacheAllData(): " + ex.Message + "\n");
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
				timeAccessProfileDtlDAO = daoFactory.getTimeAccessProfileDtlDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/// <summary>
		/// Serialize TimeAccessProfileDtlTO objects to file.
		/// </summary>
		/// <param name="timeAccessProfileDtlTOList">List of timeAccessProfileDtlTO</param>
		public void Serialize(ArrayList timeAccessProfileDtlTOList, string fileName)
		{
			try
			{
				timeAccessProfileDtlDAO.serialize(timeAccessProfileDtlTOList, fileName);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.Serialize(): " + ex.Message + "\n");
				throw ex;
			}
		}

        /// <summary>
        /// Serialize TimeAccessProfileDtlTO objects to file.
        /// </summary>
        /// <param name="timeAccessProfileDtlTOList">List of timeAccessProfileDtlTO</param>
        public void Serialize(ArrayList timeAccessProfileDtlTOList, Stream stream)
        {
            try
            {
                timeAccessProfileDtlDAO.serialize(timeAccessProfileDtlTOList, stream);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeAccessProfileDtl.Serialize(): " + ex.Message + "\n");
                throw ex;
            }
        }

		public ArrayList GetFromXMLSource(string filePath)
		{
			ArrayList newTimeAccessProfileDtl = new ArrayList();
			ArrayList newTimeAccessProfileDtlTO = new ArrayList();
			TimeAccessProfileDtl timeAccessProfileDtlMember = new TimeAccessProfileDtl();

			try
			{
				try
				{
					newTimeAccessProfileDtlTO = timeAccessProfileDtlDAO.deserialize(filePath);
				}
				catch(Exception exDes)
				{
					throw exDes;
				}

				foreach(TimeAccessProfileDtlTO timeAccessProfileDtlTO in newTimeAccessProfileDtlTO)
				{
					timeAccessProfileDtlMember = new TimeAccessProfileDtl();
					timeAccessProfileDtlMember.ReceiveTransferObject(timeAccessProfileDtlTO);

					newTimeAccessProfileDtl.Add(timeAccessProfileDtlMember);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileDtl.GetFromXMLSource(): " + ex.Message + "\n");
				throw ex;
			}
			return newTimeAccessProfileDtl;
		}

        public ArrayList GetFromXMLSource(Stream stream)
        {
            ArrayList newTimeAccessProfileDtl = new ArrayList();
            ArrayList newTimeAccessProfileDtlTO = new ArrayList();
            TimeAccessProfileDtl timeAccessProfileDtlMember = new TimeAccessProfileDtl();

            try
            {
                try
                {
                    newTimeAccessProfileDtlTO = timeAccessProfileDtlDAO.deserialize(stream);
                }
                catch (Exception exDes)
                {
                    throw exDes;
                }

                foreach (TimeAccessProfileDtlTO timeAccessProfileDtlTO in newTimeAccessProfileDtlTO)
                {
                    timeAccessProfileDtlMember = new TimeAccessProfileDtl();
                    timeAccessProfileDtlMember.ReceiveTransferObject(timeAccessProfileDtlTO);

                    newTimeAccessProfileDtl.Add(timeAccessProfileDtlMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeAccessProfileDtl.GetFromXMLSource(): " + ex.Message + "\n");
                throw ex;
            }
            return newTimeAccessProfileDtl;
        }
	}
}
