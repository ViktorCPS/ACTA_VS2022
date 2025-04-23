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
    public class UnipromButton
    {
        private int _readerID = -1;
        private int _antennaOutput = -1;
        private int _status = -1;
        private string _direction = "";
        private DateTime _modifiedTime = new DateTime();

        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
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

       
        DAOFactory daoFactory = null;
        UnipromButtonDAO ubdao = null;

        DebugLog log;

        
       public UnipromButton()
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           ubdao = daoFactory.getUnipromButtonDAO(null);

           ReaderID = -1;
           AntennaOutput = -1;
           Status = -1;
           ModifiedTime = new DateTime();
           Direction = "";
       }

       public UnipromButton(object dbConnection)
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           ubdao = daoFactory.getUnipromButtonDAO(dbConnection);

           ReaderID = -1;
           AntennaOutput = -1;
           Status = -1;
           ModifiedTime = new DateTime();
           Direction = "";
       }

     
       public void receiveTransferObject(UnipromButtonTO unipromButtonTO)
       {
           this.ReaderID = unipromButtonTO.ReaderID;
           this.AntennaOutput = unipromButtonTO.AntennaOutput;
           this.ModifiedTime = unipromButtonTO.ModifiedTime;
           this.Status = unipromButtonTO.Status;
           this.Direction = unipromButtonTO.Direction;
       }

       /// <summary>
       /// Prepare TO for DAO processing
       /// </summary>
       /// <returns></returns>
       public UnipromButtonTO sendTransferObject()
       {
           UnipromButtonTO unipromButtonTO = new UnipromButtonTO();

           unipromButtonTO.ReaderID = this.ReaderID;
           unipromButtonTO.AntennaOutput = this.AntennaOutput;
           unipromButtonTO.Status = this.Status;
           unipromButtonTO.ModifiedTime = this.ModifiedTime;
           unipromButtonTO.Direction = this.Direction;

           return unipromButtonTO;
       }

       public System.Collections.ArrayList Search(int readerID, int antenna)
       {
           ArrayList ubTOList = new ArrayList();
           ArrayList ubList = new ArrayList();

           try
           {
               UnipromButton ubMember = new UnipromButton();
               ubTOList = ubdao.search(readerID,antenna);

               foreach (UnipromButtonTO lto in ubTOList)
               {
                   ubMember = new UnipromButton();
                   ubMember.receiveTransferObject(lto);

                   ubList.Add(ubMember);
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " UnipromButton.Search(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return ubList;
       }
       public int Update(int readerID,int antenaOutput, int status,bool doCommit)
       {
           int inserted;
           try
           {
               inserted = ubdao.update(readerID, antenaOutput, status, doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " UnipromButton.Update(): " + ex.Message + "\n");
               throw ex;
           }

           return inserted;
       }

       public bool BeginTransaction()
       {
           bool isStarted = false;

           try
           {
               isStarted = ubdao.beginTransaction();
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
               ubdao.commitTransaction();
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
               ubdao.rollbackTransaction();
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
               return ubdao.getTransaction();
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
               ubdao.setTransaction(trans);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Employee.SetTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }
        

      
   }
}
