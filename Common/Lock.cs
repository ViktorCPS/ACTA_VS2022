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
   public class Lock
    {
       private int _lockID;
        private DateTime _lockDate;
        private string _type;
        private string _comment;
        private string _createdBy;
        private DateTime _createdTime;
        DAOFactory daoFactory = null;
        LockDAO ldao = null;

        DebugLog log;

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
       
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public DateTime LockDate
        {
            get { return _lockDate; }
            set { _lockDate = value; }
        }

        public int LockID
        {
            get { return _lockID; }
            set { _lockID = value; }
        }
       public Lock()
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           ldao = daoFactory.getLockDAO(null);

           LockID = -1;
           LockDate = new DateTime();
           Comment = "";
           Type = "";
           CreatedBy = "";
           CreatedTime = new DateTime();
       }

       public Lock(object dbConnection)
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           ldao = daoFactory.getLockDAO(dbConnection);

           LockID = -1;
           LockDate = new DateTime();
           Comment = "";
           Type = "";
           CreatedBy = "";
           CreatedTime = new DateTime();
       }

       public Lock(int lockID, DateTime  lockDate, string comment, string type, string createdBy,
			DateTime createdTime)
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            ldao = daoFactory.getLockDAO(null);

            LockID = -lockID;
            LockDate = lockDate;
            Comment = comment;
            Type = type;
            CreatedBy = createdBy;
            CreatedTime = createdTime;
		}
       public void receiveTransferObject(LockTO lockTO)
       {
           this.LockID = lockTO.LockID;
           this.LockDate = lockTO.LockDate;
           this.Type = lockTO.Type;
           this.Comment = lockTO.Comment;
           this.CreatedBy = lockTO.CreatedBy;
           this.CreatedTime = lockTO.CreatedTime;
       }

       /// <summary>
       /// Prepare TO for DAO processing
       /// </summary>
       /// <returns></returns>
       public LockTO sendTransferObject()
       {
           LockTO lockTO = new LockTO();

           lockTO.LockID = this.LockID;
           lockTO.LockDate = this.LockDate;
           lockTO.Type = this.Type;
           lockTO.Comment = this.Comment;
           lockTO.CreatedBy = this.CreatedBy;
           lockTO.CreatedTime = this.CreatedTime;

           return lockTO;
       }

       public System.Collections.ArrayList Search()
       {
           ArrayList lockTOList = new ArrayList();
           ArrayList lockList = new ArrayList();

           try
           {
               Lock lockMember = new Lock();
               lockTOList = ldao.search();

               foreach (LockTO lto in lockTOList)
               {
                   lockMember = new Lock();
                   lockMember.receiveTransferObject(lto);

                   lockList.Add(lockMember);
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Lock.Search(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return lockList;
       }
       public int Save(DateTime LockDate, string type, string Comment,bool doCommit)
       {
           int inserted;
           try
           {
               inserted = ldao.insert(LockDate, type, Comment, doCommit);
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
               isStarted = ldao.beginTransaction();
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
               ldao.commitTransaction();
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
               ldao.rollbackTransaction();
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
               return ldao.getTransaction();
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
               ldao.setTransaction(trans);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Employee.SetTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

   }
}
