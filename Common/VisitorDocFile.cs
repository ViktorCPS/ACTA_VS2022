using System;
using System.Collections;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class VisitorDocFile
    {
        private int _visitID = -1;
        private int _docType = -1;
        private byte[] _content = null;
        private DateTime _modifiedTime = new DateTime(0);
        private string _contentName = "";
        private DateTime _createdTime = new DateTime(0);

        DAOFactory daoFactory = null;
        VisitorDocFileDAO visitorDocFileDAO = null;

        DebugLog log;

        public int VisitID
        {
            get { return _visitID; }
            set { _visitID = value; }
        }

        public int DocType
        {
            get { return _docType; }
            set { _docType = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string ContentName
        {
            get { return _contentName; }
            set { _contentName = value; }
        }

		public VisitorDocFile()
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            visitorDocFileDAO = daoFactory.getVisitorDocFileDAO(null);
		}
        public VisitorDocFile(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            visitorDocFileDAO = daoFactory.getVisitorDocFileDAO(dbConnection);
        }

        public VisitorDocFile(int visitID, int docType, byte[] content, DateTime modifiedTime, string contentName, DateTime createdTime)
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.VisitID = visitID;
            this.DocType = docType;
            this.Content = content;
            this.ModifiedTime = modifiedTime;
            this.ContentName = contentName;
            this.CreatedTime = createdTime;

            daoFactory = DAOFactory.getDAOFactory();
            visitorDocFileDAO = daoFactory.getVisitorDocFileDAO(null);
		}

        public int Save(int visitID, int docType, byte[] content, bool doCommit)
        {
            int inserted;
            try
            {
                inserted = visitorDocFileDAO.insert(visitID, docType, content, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        public VisitorDocFileTO FindVisitorDocFileByJMBG(string JMBG)
        {
            VisitorDocFileTO visitorDocFile = new VisitorDocFileTO();
            try
            {
                visitorDocFile = visitorDocFileDAO.findVisitorDocFileByJMBG(JMBG);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.FindVisitorDocFileByJMBG(): " + ex.Message + "\n");
                throw ex;
            }

            return visitorDocFile;
        }

        public VisitorDocFileTO FindVisitorDocFileByID(string ID)
        {
            VisitorDocFileTO visitorDocFile = new VisitorDocFileTO();
            try
            {
                visitorDocFile = visitorDocFileDAO.findVisitorDocFileByID(ID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.FindVisitorDocFileByID(): " + ex.Message + "\n");
                throw ex;
            }

            return visitorDocFile;
        }

        public VisitorDocFileTO FindVisitorDocFileByVisitID(string visitID)
        {
            VisitorDocFileTO visitorDocFile = new VisitorDocFileTO();
            try
            {
                visitorDocFile = visitorDocFileDAO.findVisitorDocFileByVisitID(visitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.FindVisitorDocFileByVisitID(): " + ex.Message + "\n");
                throw ex;
            }

            return visitorDocFile;
        }

        public ArrayList SearchFileCreatedTime(DateTime DateFrom, DateTime DateTo)
        {
            ArrayList visitorDocFileTOList = new ArrayList();
            ArrayList visitorDocFileList = new ArrayList();

            try
            {
                VisitorDocFile visitorDocFileMember = new VisitorDocFile();
                visitorDocFileTOList = visitorDocFileDAO.getVisitorDocFilesForDates(DateFrom, DateTo);

                foreach (VisitorDocFileTO vdfTO in visitorDocFileTOList)
                {
                    visitorDocFileMember = new VisitorDocFile();
                    visitorDocFileMember.ReceiveTransferObject(vdfTO);

                    visitorDocFileList.Add(visitorDocFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.SearchFileCreatedTime(): " + ex.Message + "\n");
                throw ex;
            }

            return visitorDocFileList;
        }

        public bool DeleteUntilDate(DateTime createdTime, bool doCommit)
        {
            bool deleted = false;
            try
            {
                deleted = visitorDocFileDAO.deleteUntilDate(createdTime, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.DeleteUntilDate(): " + ex.Message + "\n");
                throw ex;
            }
            return deleted;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = visitorDocFileDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                visitorDocFileDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                visitorDocFileDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return visitorDocFileDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.GetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                visitorDocFileDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorDocFile.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void ReceiveTransferObject(VisitorDocFileTO visitorDocFileTO)
        {
            this.VisitID = visitorDocFileTO.VisitID;
            this.DocType = visitorDocFileTO.DocType;
            this.Content = visitorDocFileTO.Content;
            this.ModifiedTime = visitorDocFileTO.ModifiedTime;
            this.ContentName = visitorDocFileTO.ContentName;
            this.CreatedTime = visitorDocFileTO.CreatedTime;
        }

        public VisitorDocFileTO SendTransferObject()
        {
            VisitorDocFileTO visitorDocFileTO = new VisitorDocFileTO();

            visitorDocFileTO.VisitID = this.VisitID;
            visitorDocFileTO.DocType = this.DocType;
            visitorDocFileTO.Content = this.Content;
            visitorDocFileTO.ModifiedTime = this.ModifiedTime;
            visitorDocFileTO.ContentName = this.ContentName;
            visitorDocFileTO.CreatedTime = this.CreatedTime;

            return visitorDocFileTO;
        }

        public void Clear()
        {
            this.VisitID = -1;
            this.DocType = -1;
            this.Content = null;
            this.ModifiedTime = new DateTime(0);
            this.ContentName = "";
            this.CreatedTime = new DateTime(0);
        }
    }
}
