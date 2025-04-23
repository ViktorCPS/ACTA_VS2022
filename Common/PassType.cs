using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

using DataAccess;
using TransferObjects;
using Util;
using System.Data;

namespace Common
{
	/// <summary>
	/// Summary description for PassType.
	/// </summary>
	public class PassType
	{
		DAOFactory daoFactory = null;
		PassTypeDAO passTypeDAO = null;

		DebugLog log;

        private PassTypeTO ptTO = new PassTypeTO();

		public PassTypeTO PTypeTO
		{
			get{ return ptTO; }
			set{ ptTO = value; }
		}

		public PassType()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			passTypeDAO = daoFactory.getPassTypeDAO(null);
		}

        public PassType(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            passTypeDAO = daoFactory.getPassTypeDAO(dbConnection);
        }


        public PassType(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                passTypeDAO = daoFactory.getPassTypeDAO(null);
            }
        }

		public PassType(int passTypeId, string description, int button, int isPass, string paymentCode)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			passTypeDAO = daoFactory.getPassTypeDAO(null);

            PTypeTO.PassTypeID = passTypeId;
            PTypeTO.Description = description;
            PTypeTO.Button = button;
            PTypeTO.IsPass = isPass;
            PTypeTO.PaymentCode = paymentCode;
		}

		public int Save()
		{
			int inserted;

			try
			{
				inserted = passTypeDAO.insert(this.PTypeTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}
        public int Save(bool doCommit)
        {
            int inserted;

            try
            {
                inserted = passTypeDAO.insert(this.PTypeTO,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }
        public Dictionary<int, PassTypeTO> SearchDictionary(IDbTransaction trans)
        {
            try
            {
                return passTypeDAO.getPassTypesDictionary(this.PTypeTO,trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SearchDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, PassTypeTO> SearchDictionary()
        {
            try
            {
                return passTypeDAO.getPassTypesDictionary(this.PTypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SearchDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, PassTypeTO> SearchDictionaryCodeSorted()
        {
            try
            {
                return passTypeDAO.getPassTypesDictionaryCodeSorted(this.PTypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SearchDictionaryCodeSorted(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public List<PassTypeTO> Search()
		{
			try
			{				
				return passTypeDAO.getPassTypes(this.PTypeTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public Dictionary<int, PassTypeTO> SearchForCompanyDictionary(int company, bool isAlternativeLang)
        {
            try
            {
                return passTypeDAO.getPassTypesForCompanyDictionary(company, isAlternativeLang);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SearchForCompanyDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<PassTypeTO> SearchForCompany(int company, bool isAlternativeLang)
        {
            try
            {
                return passTypeDAO.getPassTypesForCompany(company, isAlternativeLang);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SearchForCompany(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<PassTypeTO> SearchMassiveInputForCompany(int company, string ptIDs, bool isAlternativeLang)
        {
            try
            {
                return passTypeDAO.getPassTypesMassiveInputForCompany(company, ptIDs, isAlternativeLang);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SearchForCompany(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<string> SearchDistinctField(string field)
        {
            try
            {
                return passTypeDAO.getPassTypesDistinctField(field);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SearchDistinctField(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<PassTypeTO> Search(bool createDAO)
        {
           try
            {
                return passTypeDAO.getPassTypes(this.PTypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<PassTypeTO> SearchConformationTypes(int ptID, string ptIDs, bool isAltLang)
        {
            try
            {
                return passTypeDAO.getConformationTypes(ptID, ptIDs, isAltLang);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SearchConformationTypes(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public List<PassTypeTO> Search(List<int> isPass)
		{
			try
			{			
				return passTypeDAO.getPassTypes(this.PTypeTO, isPass);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        public Dictionary<int,PassTypeTO> FindByPaymentCode(string payment_code, int company)
        {
            try
            {
                return passTypeDAO.findByPaymentCode(payment_code, company);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.FindByPaymentCode(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }		
		public bool Update(int oldButton)
		{
			try
			{
				return passTypeDAO.update(this.PTypeTO, oldButton);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        public bool Update(int oldButton, bool doCommit)
        {
            try
            {
                return passTypeDAO.update(this.PTypeTO, oldButton,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public bool Delete(int passTypeID)
		{
			try
			{
				return passTypeDAO.delete(passTypeID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public bool Delete(int passTypeID, bool doCommit)
        {
            try
            {
                return passTypeDAO.delete(passTypeID,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public void Clear()
		{
			this.PTypeTO = new PassTypeTO();
		}

		public PassTypeTO Find(int passTypeID)
		{
			try
			{
				return passTypeDAO.find(passTypeID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        public Dictionary<int,PassTypeTO> Find(string passTypeID,int company)
        {
            try
            {
                return passTypeDAO.find(passTypeID,company);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public int FindMAXPassTypeID()
		{
			try
			{
				return passTypeDAO.findMAXPassTypeID();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.FindMAXPassTypeID(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Send list of PassType objects to serialization.
		/// </summary>
		/// <param name="PassTypeTOList">List of PassTypeTO</param>
		private void CacheData(List<PassTypeTO> PassTypeTOList)
		{
			try
			{
				passTypeDAO.serialize(PassTypeTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		public void CacheData()
		{
			List<PassTypeTO> passTypeTOList = new List<PassTypeTO>();

			try
			{
				passTypeTOList = passTypeDAO.getPassTypes(this.PTypeTO);
				this.CacheData(passTypeTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the PassTypes from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(passTypeDAO.getPassTypes(new PassTypeTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.CacheAllData(): " + ex.Message + "\n");
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
				passTypeDAO = daoFactory.getPassTypeDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = passTypeDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                passTypeDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                passTypeDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return passTypeDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                passTypeDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }


       
    }
}
