using System;
using System.Collections;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    /// <summary>
    /// Summary description for AccessControlFile.
    /// </summary>
    public class AccessControlFile
    {
        private int _recordID = -1;
        private string _type = "";
        private int _readerID = -1;
        private int _delayed = -1;
        private string _status = "";
        private DateTime _uploadStartTime = new DateTime(0);
        private DateTime _uploadEndTime = new DateTime(0);
        private string _errorContent = "";
        private byte[] _content = null;
        private DateTime _createdTime = new DateTime(0);
        private string _createdBy = "";

        DAOFactory daoFactory = null;
        AccessControlFileDAO accessControlFileDAO = null;

        DebugLog log;

        public int RecordID
        {
            get { return _recordID; }
            set { _recordID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public int Delayed
        {
            get { return _delayed; }
            set { _delayed = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public DateTime UploadStartTime
        {
            get { return _uploadStartTime; }
            set { _uploadStartTime = value; }
        }

        public DateTime UploadEndTime
        {
            get { return _uploadEndTime; }
            set { _uploadEndTime = value; }
        }

        public string ErrorContent
        {
            get { return _errorContent; }
            set { _errorContent = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

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

        public AccessControlFile()
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            accessControlFileDAO = daoFactory.getAccessControlFileDAO(null);
		}

        public AccessControlFile(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            accessControlFileDAO = daoFactory.getAccessControlFileDAO(dbConnection);
        }

        public AccessControlFile(bool testConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (testConnection)
            {
                daoFactory = DAOFactory.getDAOFactory();
                accessControlFileDAO = daoFactory.getAccessControlFileDAO(null);
            }
            else
            {
                daoFactory = DAOFactory.getDAOFactoryWithoutTestConnection();
                accessControlFileDAO = daoFactory.getAccessControlFileDAO(null);
            }
        }

        public AccessControlFile(int recordID, string type, int readerID, int delayed,
            string status, DateTime uploadStartTime, DateTime uploadEndTime,
            string errorContent, byte[] content, DateTime createdTime, string createdBy)
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.RecordID = recordID;
            this.Type = type;
            this.ReaderID = readerID;
            this.Delayed = delayed;
            this.Status = status;
            this.UploadStartTime = uploadStartTime;
            this.UploadEndTime = uploadEndTime;
            this.ErrorContent = errorContent;
            this.Content = content;
            this.CreatedTime = createdTime;
            this.CreatedBy = createdBy;

            daoFactory = DAOFactory.getDAOFactory();
            accessControlFileDAO = daoFactory.getAccessControlFileDAO(null);
		}

        public void CloseDBConnection()
        {
            try
            {
                accessControlFileDAO.CloseDBConnection();
            }
            catch { }
        }

        public bool Save(string type, int readerID, int delayed, string status, DateTime uploadStartTime,
            DateTime uploadEndTime, byte[] content, bool doCommit)
        {
            bool inserted;
            try
            {
                inserted = accessControlFileDAO.insert(type, readerID, delayed, status, uploadStartTime,
                    uploadEndTime, content, doCommit) > 0;

                inserted = inserted && accessControlFileDAO.deleteOld(readerID, type, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        public bool Update(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID,
            string modifiedBy, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = accessControlFileDAO.update(readerID, type, oldStatus, newStatus, 
                    uploadStartTime, uploadEndTime, delay, recordID, modifiedBy, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool UpdateOthers(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID,
            string modifiedBy, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = accessControlFileDAO.updateOthers(readerID, type, oldStatus, newStatus,
                    uploadStartTime, uploadEndTime, delay, recordID, modifiedBy, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        /*public ExtraHourTO Find(int employeeID, DateTime dateEarned)
        {
            ExtraHourTO extraHourTO = new ExtraHourTO();

            try
            {
                extraHourTO = extraHourDAO.find(employeeID, dateEarned);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return extraHourTO;
        }*/

        public ArrayList Search(string type, int reader_id, int delayed, string status,
            DateTime uploadStartTime, DateTime uploadEndTime)
        {
            ArrayList accessControlFileTOList = new ArrayList();
            ArrayList accessControlFileList = new ArrayList();

            try
            {
                AccessControlFile accessControlFileMember = new AccessControlFile();
                accessControlFileTOList = accessControlFileDAO.getAccessControlFiles(type, reader_id, delayed, 
                    status, uploadStartTime, uploadEndTime);

                foreach (AccessControlFileTO acfTO in accessControlFileTOList)
                {
                    accessControlFileMember = new AccessControlFile();
                    accessControlFileMember.ReceiveTransferObject(acfTO);

                    accessControlFileList.Add(accessControlFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return accessControlFileList;
        }

        public AccessControlFileTO SearchMax(string type, int reader_id)
        {
            try
            {
                return accessControlFileDAO.getAccessControlFilesMax(type, reader_id);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.SearchMax(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchCount(string type, string status)
        {
            int count = -1;

            try
            {
                count = accessControlFileDAO.getAccessControlFilesCount(type, status);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

        public ArrayList SearchLastIssuedACFiles()
        {
            ArrayList accessControlFileTOList = new ArrayList();
            ArrayList accessControlFileList = new ArrayList();

            try
            {
                AccessControlFile accessControlFileMember = new AccessControlFile();
                accessControlFileTOList = accessControlFileDAO.getLastIssuedACFiles();

                foreach (AccessControlFileTO acfTO in accessControlFileTOList)
                {
                    accessControlFileMember = new AccessControlFile();
                    accessControlFileMember.ReceiveTransferObject(acfTO);

                    accessControlFileList.Add(accessControlFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.SearchLastIssuedACFiles(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return accessControlFileList;
        }

        public ArrayList SearchLastUploadTime()
        {
            ArrayList accessControlFileTOList = new ArrayList();
            ArrayList accessControlFileList = new ArrayList();

            try
            {
                AccessControlFile accessControlFileMember = new AccessControlFile();
                accessControlFileTOList = accessControlFileDAO.getLastUploadTime();

                foreach (AccessControlFileTO acfTO in accessControlFileTOList)
                {
                    accessControlFileMember = new AccessControlFile();
                    accessControlFileMember.ReceiveTransferObject(acfTO);

                    accessControlFileList.Add(accessControlFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.SearchLastIssuedACFiles(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return accessControlFileList;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = accessControlFileDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                accessControlFileDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                accessControlFileDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return accessControlFileDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                accessControlFileDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlFile.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void ReceiveTransferObject(AccessControlFileTO accessControlFileTO)
        {
            this.RecordID = accessControlFileTO.RecordID;
            this.Type = accessControlFileTO.Type;
            this.ReaderID = accessControlFileTO.ReaderID;
            this.Delayed = accessControlFileTO.Delayed;
            this.Status = accessControlFileTO.Status;
            this.UploadStartTime = accessControlFileTO.UploadStartTime;
            this.UploadEndTime = accessControlFileTO.UploadEndTime;
            this.ErrorContent = accessControlFileTO.ErrorContent;
            this.Content = accessControlFileTO.Content;
            this.CreatedTime = accessControlFileTO.CreatedTime;
            this.CreatedBy = accessControlFileTO.CreatedBy;
        }

        public AccessControlFileTO SendTransferObject()
        {
            AccessControlFileTO accessControlFileTO = new AccessControlFileTO();

            accessControlFileTO.RecordID = this.RecordID;
            accessControlFileTO.Type = this.Type;
            accessControlFileTO.ReaderID = this.ReaderID;
            accessControlFileTO.Delayed = this.Delayed;
            accessControlFileTO.Status = this.Status;
            accessControlFileTO.UploadStartTime = this.UploadStartTime;
            accessControlFileTO.UploadEndTime = this.UploadEndTime;
            accessControlFileTO.ErrorContent = this.ErrorContent;
            accessControlFileTO.Content = this.Content;
            accessControlFileTO.CreatedTime = this.CreatedTime;
            accessControlFileTO.CreatedBy = this.CreatedBy;

            return accessControlFileTO;
        }

        public void Clear()
        {
            this.RecordID = -1;
            this.Type = "";
            this.ReaderID = -1;
            this.Delayed = -1;
            this.Status = "";
            this.UploadStartTime = new DateTime(0);
            this.UploadEndTime = new DateTime(0);
            this.ErrorContent = "";
            this.Content = null;
            this.CreatedTime = new DateTime(0);
            this.CreatedBy = "";
        }
    }
}
