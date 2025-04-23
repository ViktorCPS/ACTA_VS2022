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
    /// Summary description for CameraSnapshotFile.
    /// </summary>
    public class CameraSnapshotFile
    {
        private int _recordID = -1;
        private int _cameraID = -1;
        private DateTime _cameraCreatedTime = new DateTime(0);
        private DateTime _fileCreatedTime = new DateTime(0);
        private byte[] _content = null;
        private string _fileName = "";

        DAOFactory daoFactory = null;
        CameraSnapshotFileDAO cameraSnapshotFileDAO = null;

        DebugLog log;

        public int RecordID
        {
            get { return _recordID; }
            set { _recordID = value; }
        }

        public int CameraID
        {
            get { return _cameraID; }
            set { _cameraID = value; }
        }

        public DateTime CameraCreatedTime
        {
            get { return _cameraCreatedTime; }
            set { _cameraCreatedTime = value; }
        }

        public DateTime FileCreatedTime
        {
            get { return _fileCreatedTime; }
            set { _fileCreatedTime = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public CameraSnapshotFile()
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            cameraSnapshotFileDAO = daoFactory.getCameraSnapshotFileDAO(null);
		}

        public CameraSnapshotFile(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            cameraSnapshotFileDAO = daoFactory.getCameraSnapshotFileDAO(dbConnection);
        }

        public CameraSnapshotFile(int recordID, int cameraID,
            DateTime cameraCreatedTime, DateTime fileCreatedTime, byte[] content, string fileName)
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.RecordID = recordID;
            this.CameraID = cameraID;
            this.CameraCreatedTime = cameraCreatedTime;
            this.FileCreatedTime = fileCreatedTime;
            this.Content = content;
            this.FileName = fileName;

            daoFactory = DAOFactory.getDAOFactory();
            cameraSnapshotFileDAO = daoFactory.getCameraSnapshotFileDAO(null);
		}

        public int Save(string fileName, int cameraID,
            DateTime cameraCreatedTime, DateTime fileCreatedTime, byte[] content, bool doCommit)
        {
            int inserted;
            try
            {
                inserted = cameraSnapshotFileDAO.insert(fileName, cameraID, 
                    cameraCreatedTime, fileCreatedTime, content, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        public int Save(CameraSnapshotFileTO cameraFileTO, bool doCommit)
        {
            int inserted;
            try
            {
                inserted = cameraSnapshotFileDAO.insert(cameraFileTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        /*public bool Update(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID,
            string modifiedBy, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = cameraSnapshotFileDAO.update(readerID, type, oldStatus, newStatus, 
                    uploadStartTime, uploadEndTime, delay, recordID, modifiedBy, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }*/

        /*public ExtraHourTO Find(int employeeID, DateTime dateEarned)
        {
            ExtraHourTO extraHourTO = new ExtraHourTO();

            try
            {
                extraHourTO = extraHourDAO.find(employeeID, dateEarned);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return extraHourTO;
        }*/

        public ArrayList Search(int cameraID,
            DateTime cameraCreatedTime, DateTime fileCreatedTime)
        {
            ArrayList cameraSnapshotFileTOList = new ArrayList();
            ArrayList cameraSnapshotFileList = new ArrayList();

            try
            {
                CameraSnapshotFile cameraSnapshotFileMember = new CameraSnapshotFile();
                cameraSnapshotFileTOList = cameraSnapshotFileDAO.getCameraSnapshotFiles(cameraID, 
                    cameraCreatedTime, fileCreatedTime);

                foreach (CameraSnapshotFileTO csfTO in cameraSnapshotFileTOList)
                {
                    cameraSnapshotFileMember = new CameraSnapshotFile();
                    cameraSnapshotFileMember.ReceiveTransferObject(csfTO);

                    cameraSnapshotFileList.Add(cameraSnapshotFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }

        public int SearchCount(int cameraID)
        {
            int count = -1;

            try
            {
                count = cameraSnapshotFileDAO.getCameraSnapshotFilesCount(cameraID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

        public ArrayList SearchFileCreatedTime(DateTime DateFrom,DateTime DateTo)
        {
            ArrayList cameraSnapshotFileTOList = new ArrayList();
            ArrayList cameraSnapshotFileList = new ArrayList();

            try
            {
                CameraSnapshotFile cameraSnapshotFileMember = new CameraSnapshotFile();
                cameraSnapshotFileTOList = cameraSnapshotFileDAO.getCamSnapshotFilesForDates(DateFrom, DateTo);

                foreach (CameraSnapshotFileTO csfTO in cameraSnapshotFileTOList)
                {
                    cameraSnapshotFileMember = new CameraSnapshotFile();
                    cameraSnapshotFileMember.ReceiveTransferObject(csfTO);

                    cameraSnapshotFileList.Add(cameraSnapshotFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }

        public bool DeleteUntilDate(DateTime fileCreatedTime, bool doCommit)
        {
            bool deleted = false;
            try
            {
                deleted = cameraSnapshotFileDAO.DeleteUntilDate(fileCreatedTime, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return deleted;
        }

        public ArrayList SearchForPass(int passID, DateTime fromDate, DateTime toDate, string direction)
        {
            ArrayList cameraSnapshotFileTOList = new ArrayList();
            ArrayList cameraSnapshotFileList = new ArrayList();

            try
            {
                CameraSnapshotFile cameraSnapshotFileMember = new CameraSnapshotFile();
                cameraSnapshotFileTOList = cameraSnapshotFileDAO.getCSFilesForPass(passID,
                    fromDate, toDate, direction);

                foreach (CameraSnapshotFileTO csfTO in cameraSnapshotFileTOList)
                {
                    cameraSnapshotFileMember = new CameraSnapshotFile();
                    cameraSnapshotFileMember.ReceiveTransferObject(csfTO);

                    cameraSnapshotFileList.Add(cameraSnapshotFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.SearchForPass(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }

        public ArrayList SearchForPassDisplay(string recordID)
        {
            ArrayList cameraSnapshotFileTOList = new ArrayList();
            ArrayList cameraSnapshotFileList = new ArrayList();

            try
            {
                CameraSnapshotFile cameraSnapshotFileMember = new CameraSnapshotFile();
                cameraSnapshotFileTOList = cameraSnapshotFileDAO.getCSFilesForPassDisplay(recordID);

                foreach (CameraSnapshotFileTO csfTO in cameraSnapshotFileTOList)
                {
                    cameraSnapshotFileMember = new CameraSnapshotFile();
                    cameraSnapshotFileMember.ReceiveTransferObject(csfTO);

                    cameraSnapshotFileList.Add(cameraSnapshotFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.SearchForPassDisplay(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = cameraSnapshotFileDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                cameraSnapshotFileDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                cameraSnapshotFileDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return cameraSnapshotFileDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                cameraSnapshotFileDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void ReceiveTransferObject(CameraSnapshotFileTO cameraSnapshotFileTO)
        {
            this.RecordID = cameraSnapshotFileTO.RecordID;
            this.CameraID = cameraSnapshotFileTO.CameraID;
            this.CameraCreatedTime = cameraSnapshotFileTO.CameraCreatedTime;
            this.FileCreatedTime = cameraSnapshotFileTO.FileCreatedTime;
            this.Content = cameraSnapshotFileTO.Content;
            this.FileName = cameraSnapshotFileTO.FileName;
        }

        public CameraSnapshotFileTO SendTransferObject()
        {
            CameraSnapshotFileTO cameraSnapshotFileTO = new CameraSnapshotFileTO();

            cameraSnapshotFileTO.RecordID = this.RecordID;
            cameraSnapshotFileTO.CameraID = this.CameraID;
            cameraSnapshotFileTO.CameraCreatedTime = this.CameraCreatedTime;
            cameraSnapshotFileTO.FileCreatedTime = this.FileCreatedTime;
            cameraSnapshotFileTO.Content = this.Content;
            cameraSnapshotFileTO.FileName = this.FileName;

            return cameraSnapshotFileTO;
        }

        public void Clear()
        {
            this.RecordID = -1;
            this.CameraID = -1;
            this.CameraCreatedTime = new DateTime(0);
            this.FileCreatedTime = new DateTime(0);
            this.Content = null;
            this.FileName = "";
        }

        public ArrayList SearchSnapshots(string cameraID,  DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo)
        {
            ArrayList cameraSnapshotFileTOList = new ArrayList();
            ArrayList cameraSnapshotFileList = new ArrayList();

            try
            {
                CameraSnapshotFile cameraSnapshotFileMember = new CameraSnapshotFile();
                cameraSnapshotFileTOList = cameraSnapshotFileDAO.getCameraSnapshotFiles(cameraID,dateFrom,dateTo, timeFrom,timeTo);

                foreach (CameraSnapshotFileTO csfTO in cameraSnapshotFileTOList)
                {
                    cameraSnapshotFileMember = new CameraSnapshotFile();
                    cameraSnapshotFileMember.ReceiveTransferObject(csfTO);

                    cameraSnapshotFileList.Add(cameraSnapshotFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotFile.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraSnapshotFileList;
        }
    }
}
