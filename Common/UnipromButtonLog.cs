using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;
using System.Collections;
using System.Data;

namespace Common
{
    public class UnipromButtonLog
    {
         private int _logID = -1;
        private int _readerID = -1;
        private int _antennaOutput = -1;
        private int _status = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int AntennaOutput
        {
            get { return _antennaOutput; }
            set { _antennaOutput = value; }
        }

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public int LogID
        {
            get { return _logID; }
            set { _logID = value; }
        }

        DAOFactory daoFactory = null;
        UnipromButtonLogDAO ubldao = null;

        DebugLog log;

        
       public UnipromButtonLog()
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           ubldao = daoFactory.getUnipromButtonLogDAO(null);

           LogID = -1;
           ReaderID = -1;
           AntennaOutput = -1;
           Status = -1;
           CreatedBy = "";
           CreatedTime = new DateTime();
       }

       public UnipromButtonLog(object dbConnection)
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           ubldao = daoFactory.getUnipromButtonLogDAO(dbConnection);

           LogID = -1;
           ReaderID = -1;
           AntennaOutput = -1;
           Status = -1;
           CreatedBy = "";
           CreatedTime = new DateTime();
       }

     
       public void receiveTransferObject(UnipromButtonLogTO unipromButtonLogTO)
       {
           this.LogID= unipromButtonLogTO.LogID;
           this.ReaderID = unipromButtonLogTO.ReaderID;
           this.AntennaOutput = unipromButtonLogTO.AntennaOutput;
           this.Status = unipromButtonLogTO.Status;
           this.CreatedBy = unipromButtonLogTO.CreatedBy;
           this.CreatedTime = unipromButtonLogTO.CreatedTime;
       }

       /// <summary>
       /// Prepare TO for DAO processing
       /// </summary>
       /// <returns></returns>
       public UnipromButtonLogTO sendTransferObject()
       {
           UnipromButtonLogTO unipromButtonTO = new UnipromButtonLogTO();

           unipromButtonTO.LogID = this.LogID;
           unipromButtonTO.ReaderID = this.ReaderID;
           unipromButtonTO.Status = this.Status;
           unipromButtonTO.AntennaOutput = this.AntennaOutput;
           unipromButtonTO.CreatedBy = this.CreatedBy;
           unipromButtonTO.CreatedTime = this.CreatedTime;

           return unipromButtonTO;
       }

       public System.Collections.ArrayList Search(int readerID)
       {
           ArrayList ubTOList = new ArrayList();
           ArrayList ubList = new ArrayList();

           try
           {
               UnipromButtonLog ubMember = new UnipromButtonLog();
               ubTOList = ubldao.search(readerID);

               foreach (UnipromButtonLogTO lto in ubTOList)
               {
                   ubMember = new UnipromButtonLog();
                   ubMember.receiveTransferObject(lto);

                   ubList.Add(ubMember);
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Lock.Search(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return ubList;
       }
       public int Save(int readerID,int antenaOutput, int status,bool doCommit)
       {
           int inserted;
           try
           {
               inserted = ubldao.insert(readerID, antenaOutput, status, doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Lock.Save(): " + ex.Message + "\n");
               throw ex;
           }

           return inserted;
       }

       public bool BeginTransaction()
       {
           bool isStarted = false;

           try
           {
               isStarted = ubldao.beginTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Employee.BeginTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return isStarted;
       }

       public void CommitTransaction()
       {
           try
           {
               ubldao.commitTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public void RollbackTransaction()
       {
           try
           {
               ubldao.rollbackTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public IDbTransaction GetTransaction()
       {
           try
           {
               return ubldao.getTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Employee.GetTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public void SetTransaction(IDbTransaction trans)
       {
           try
           {
               ubldao.setTransaction(trans);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Employee.SetTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

    }
}
