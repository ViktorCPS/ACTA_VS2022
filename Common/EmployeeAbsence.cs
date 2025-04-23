using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for EmployeeAbsence.
	/// </summary>
	public class EmployeeAbsence
	{		
		DAOFactory daoFactory = null;
		EmployeeAbsenceDAO emplabsenceDAO = null;

		DebugLog log;

        EmployeeAbsenceTO emplAbsTO = new EmployeeAbsenceTO();

		public EmployeeAbsenceTO EmplAbsTO
		{
			get{ return emplAbsTO; }
			set{ emplAbsTO = value; }
		}
        
		public EmployeeAbsence(int recID, int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.EmplAbsTO.RecID = recID;
			this.EmplAbsTO.EmployeeID = employeeID;
			this.EmplAbsTO.PassTypeID = passTypeID;
			this.EmplAbsTO.DateStart = dateStart;
			this.EmplAbsTO.DateEnd = dateEnd;
			this.EmplAbsTO.Used = used;
            this.EmplAbsTO.CreatedBy = "";
            this.EmplAbsTO.VacationYear = new DateTime();
            this.EmplAbsTO.Description = "";
            this.EmplAbsTO.CreatedTime = new DateTime();

			daoFactory = DAOFactory.getDAOFactory();
			emplabsenceDAO = daoFactory.getEmployeeAbsenceDAO(null);
		}

		public EmployeeAbsence()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			emplabsenceDAO = daoFactory.getEmployeeAbsenceDAO(null);
        }
        public EmployeeAbsence(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            emplabsenceDAO = daoFactory.getEmployeeAbsenceDAO(dbConnection);
        }

		public int Save(int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used, DateTime vacationYear, string description)
		{
			int saved = 0;
			try
			{
                if (Misc.isLockedDate(dateStart.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateStart.Date);
                    throw new Exception(exceptionString);
                }
				saved = emplabsenceDAO.insert(employeeID, passTypeID, dateStart, dateEnd, used, vacationYear,description);

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return saved;
		}

        public int Save(int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used, DateTime vacationYear, bool doCommit)
        {
            int saved = 0;
            try
            {
                if (Misc.isLockedDate(dateStart.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateStart.Date);
                    throw new Exception(exceptionString);
                }
                saved = emplabsenceDAO.insert(employeeID, passTypeID, dateStart, dateEnd, used, vacationYear, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsence.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }


		public bool Update(int recID, int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used)
		{
			bool isUpdated = false;

			try
			{
				isUpdated = emplabsenceDAO.update(recID, employeeID, passTypeID, dateStart, dateEnd, used);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool UpdateEADeleteIOP(int recID, int employeeID, int passTypeIDOld, int passTypeID, DateTime dateStartOld,
			DateTime dateEndOld, DateTime dateStart, DateTime dateEnd, int used, IDbTransaction trans, string description)
		{
			bool isUpdated = false;

			try
			{
                if (Misc.isLockedDate(dateStart.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateStart.Date);
                    throw new Exception(exceptionString);
                }
                if (Misc.isLockedDate(dateStartOld.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateStartOld.Date);
                    throw new Exception(exceptionString);
                }
				isUpdated = emplabsenceDAO.updateEAdeleteIOP(recID, employeeID, passTypeIDOld, passTypeID,
					dateStartOld, dateEndOld, dateStart, dateEnd, used, daoFactory, trans, description);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

        public bool UpdateEADeleteIOP(int recID, int employeeID, int passTypeIDOld, int passTypeID, DateTime dateStartOld,
            DateTime dateEndOld, DateTime dateStart, DateTime dateEnd, int used, IDbTransaction trans)
        {
            bool isUpdated = false;

            try
            {
                if (Misc.isLockedDate(dateStart.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateStart.Date);
                    throw new Exception(exceptionString);
                }
                if (Misc.isLockedDate(dateStartOld.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(dateStartOld.Date);
                    throw new Exception(exceptionString);
                }
                isUpdated = emplabsenceDAO.updateEAdeleteIOP(recID, employeeID, passTypeIDOld, passTypeID,
                    dateStartOld, dateEndOld, dateStart, dateEnd, used, daoFactory, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsence.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

		public bool Delete(int recID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = emplabsenceDAO.delete(recID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool DeleteEADeleteIOP(int recID)
		{
			bool isDeleted = false;

			try
			{
				EmployeeAbsenceTO emplAbsTO = Find(recID);
                if (Misc.isLockedDate(emplAbsTO.DateStart.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(emplAbsTO.DateStart.Date);
                    throw new Exception(exceptionString);
                }
				isDeleted = emplabsenceDAO.deleteEAdeleteIOP(recID, emplAbsTO.EmployeeID, emplAbsTO.PassTypeID,
					emplAbsTO.DateStart, emplAbsTO.DateEnd, daoFactory);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public EmployeeAbsenceTO Find(int recID)
		{
			try
			{
				return emplabsenceDAO.find(recID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
        // Same as Search(string  employeeID, string passTypeID, DateTime dateStart, DateTime dateEnd, string used, string wUnits, IDBTransaction trans)
		public List<EmployeeAbsenceTO> Search(string wUnits)
		{
			try
			{
                return emplabsenceDAO.getEmployeeAbsences(this.EmplAbsTO, wUnits);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        public List<EmployeeAbsenceTO> Search()
        {
            try
            {
                return emplabsenceDAO.getEmployeeAbsences(this.EmplAbsTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsence.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        // Same as Search(string  employeeID, string passTypeID, DateTime dateStart, DateTime dateEnd, string used, string wUnits)
        public List<EmployeeAbsenceTO> Search(string wUnits, IDbTransaction trans)
        {            
            try
            {
                return emplabsenceDAO.getEmployeeAbsences(this.EmplAbsTO, wUnits, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsence.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //Added this function because Search algorithm is changed for Absences form, 
		//and original Search function is used in IOPair, for insertWholeDayAbsences
        public List<EmployeeAbsenceTO> SearchForAbsences(string wUnits)
		{
			try
			{				
				return emplabsenceDAO.getEmployeeAbsencesForAbsences(this.EmplAbsTO, wUnits);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.SearchAbsences(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public int SearchExistingAbsences(string wUnits)
		{			
			try
			{
				return emplabsenceDAO.getExistingEmployeeAbsences(this.EmplAbsTO, wUnits);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAbsence.SearchExistingAbsences(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<EmployeeAbsenceTO> SearchForVacEvid(string employees, string wUnits, DateTime relatedYearFrom, DateTime relatedYearTo)
        {
            try
            {                
                return emplabsenceDAO.getEmployeeAbsencesForVacEvid(this.EmplAbsTO, employees, wUnits, relatedYearFrom,relatedYearTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAbsence.SearchForVacEvid(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
		public void Clear()
		{
			this.EmplAbsTO = new EmployeeAbsenceTO();
	    }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = emplabsenceDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeVacEdit.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                emplabsenceDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeVacEdit.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                emplabsenceDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeVacEdit.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return emplabsenceDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeVacEdit.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                emplabsenceDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeVacEdit.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
	}
}

