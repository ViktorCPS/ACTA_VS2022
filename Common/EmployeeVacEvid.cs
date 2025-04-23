using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
   public  class EmployeeVacEvid
    {
       EmployeeVacEvidTO emplVacEvidTO = new EmployeeVacEvidTO();

       public EmployeeVacEvidTO EmplVacEvidTO
       {
           get { return emplVacEvidTO; }
           set { emplVacEvidTO = value; }
       }        
       
        DAOFactory daoFactory = null;
        EmployeeVacEvidDAO edao = null;

        DebugLog log;

       public EmployeeVacEvid()
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           edao = daoFactory.getEmployeeVacEvidDAO(null);
       }

       public EmployeeVacEvid(object dbConnection)
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           edao = daoFactory.getEmployeeVacEvidDAO(dbConnection);
       }              

       public bool Remove(int employeeID,DateTime vacYear)
       {
           bool isDeleted = false;

           try
           {
               isDeleted = edao.delete(employeeID, vacYear);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " EmployeeVacEdit.Remove(): " + ex.Message + "\n");
               throw ex;
           }

           return isDeleted;
       }

       public bool Remove(int employeeID, DateTime vacYear, bool doCommit)
       {
           bool isDeleted = false;

           try
           {
               isDeleted = edao.delete(employeeID, vacYear, doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " EmployeeVacEdit.Remove(): " + ex.Message + "\n");
               throw ex;
           }

           return isDeleted;
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
                log.writeLog(DateTime.Now + " EmployeeVacEdit.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeVacEdit.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeVacEdit.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeVacEdit.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeVacEdit.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

       public List<EmployeeVacEvidTO> getVacations(string employeeID, DateTime yearFrom, DateTime yearTo, int daysApproveMin, int daysApproveMax)
       {          
           try
           {
               return edao.search(employeeID, yearFrom, yearTo, daysApproveMin, daysApproveMax);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " EmployeeVacEdit.getVacations(): " + ex.Message + "\n");
               throw ex;
           }
       }

       public List<EmployeeVacEvidScheduleTO> getVacationSchedules(int employeeID, DateTime vacYear)
       {
           try
           {
               return edao.getVacationSchedules(employeeID, vacYear);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " EmployeeVacEdit.getVacations(): " + ex.Message + "\n");
               throw ex;
           }
       }

       public int getVacationsCount(string employeeID, DateTime yearFrom,DateTime yearTO, int daysApproveMin, int daysApproveMax)
       {
           int count = 0;

           try
           {
               count = edao.searchCount(employeeID, yearFrom,yearTO, daysApproveMin, daysApproveMax);

           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " EmployeeVacEdit.getVacationsCount(): " + ex.Message + "\n");
               throw ex;
           }

           return count;
       }

       public int Save()
       {
           int affectedRows;
           try
           {
               affectedRows = edao.insert(this.EmplVacEvidTO);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + "EmployeeVacEdit.Save(): " + ex.Message + "\n");
               throw ex;
           }

           return affectedRows;
       }

       public int Save(bool doCommit)
       {
           int affectedRows;
           try
           {
               affectedRows = edao.insert(this.EmplVacEvidTO,doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + "EmployeeVacEdit.Save(): " + ex.Message + "\n");
               throw ex;
           }

           return affectedRows;
       }

       public Dictionary<int, EmployeeVacEvidScheduleTO> SearchDetails(int employeeID, DateTime year)
       {
           // List that contins TO object
           List<EmployeeVacEvidScheduleTO> emplVacTO = new List<EmployeeVacEvidScheduleTO>();
           Dictionary<int, EmployeeVacEvidScheduleTO> emplVac = new Dictionary<int,EmployeeVacEvidScheduleTO>();

           try
           {
               emplVacTO = edao.getVacationSchedules(employeeID, year);
               
               foreach (EmployeeVacEvidScheduleTO emplVacationTO in emplVacTO)
               {
                   emplVac.Add(emplVacationTO.Segment, emplVacationTO);
               }

           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MapsObjectHdr.SearchDetailsTerminal(): " + ex.Message + "\n");
               throw ex;
           }

           return emplVac;
       }

       public List<EmployeeVacEvidScheduleTO> getVacationDetails(string employeesString, DateTime yearFrom, DateTime yearTo)
       {
           try
           {
               return edao.getVacationSchedules(employeesString, yearFrom, yearTo);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MapsObjectHdr.SearchDetailsTerminal(): " + ex.Message + "\n");
               throw ex;
           }
       }
   }
}
