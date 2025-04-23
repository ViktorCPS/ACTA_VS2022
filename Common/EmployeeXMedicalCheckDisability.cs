using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class EmployeeXMedicalCheckDisability
    {
        DAOFactory daoFactory = null;
        EmployeesXMedicalCheckDisabilityDAO edao = null;
		
		DebugLog log;

        EmployeeXMedicalCheckDisabilityTO emplXDisabilityTO = new EmployeeXMedicalCheckDisabilityTO();

        public EmployeeXMedicalCheckDisabilityTO EmplXDisabilityTO
        {
            get { return emplXDisabilityTO; }
            set { emplXDisabilityTO = value; }
        }
        		
		public EmployeeXMedicalCheckDisability()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeesXMedicalCheckDisabilityDAO(null);
		}

        public EmployeeXMedicalCheckDisability(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeesXMedicalCheckDisabilityDAO(dbConnection);
        }

        public EmployeeXMedicalCheckDisability(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeesXMedicalCheckDisabilityDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.EmplXDisabilityTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.EmplXDisabilityTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(string recID, bool doCommit)
        {
            try
            {
                return edao.delete(recID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeXMedicalCheckDisabilityTO> SearchEmployeeXMedicalCheckDisabilities()
        {           
            try
            {
                return edao.getEmployeeXMedicalCheckDisabilities(this.EmplXDisabilityTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.SearchEmployeeXMedicalCheckDisabilities(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<uint, EmployeeXMedicalCheckDisabilityTO> SearchEmployeeXMedicalCheckDisabilities(string emplIDs, string data, DateTime from, DateTime to)
        {
            try
            {
                return edao.getEmployeeXMedicalCheckDisabilities(emplIDs, data, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.SearchEmployeeXMedicalCheckDisabilities(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = edao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                edao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                edao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return edao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                edao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMedicalCheckDisability.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
