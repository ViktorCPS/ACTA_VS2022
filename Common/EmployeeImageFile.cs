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
    /// Summary description for EmployeeImageFile.
    /// </summary>
    public class EmployeeImageFile
    {
        private int _employeeID = -1;
        private byte[] _picture = null;
        private DateTime _modifiedTime = new DateTime(0);
        private string _pictureName = "";

        DAOFactory daoFactory = null;
        EmployeeImageFileDAO employeeImageFileDAO = null;

        DebugLog log;

        public int EmployeeID
		{
            get { return _employeeID; }
            set { _employeeID = value; }
		}

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }

        public string PictureName
        {
            get { return _pictureName; }
            set { _pictureName = value; }
        }

		public EmployeeImageFile()
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            employeeImageFileDAO = daoFactory.getEmployeeImageFileDAO(null);
		}

        public EmployeeImageFile(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            employeeImageFileDAO = daoFactory.getEmployeeImageFileDAO(dbConnection);
        }

        public EmployeeImageFile(int employeeID, byte[] picture, DateTime modifiedTime,
            string pictureName)
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.EmployeeID = employeeID;
            this.Picture = picture;
            this.ModifiedTime = modifiedTime;
            this.PictureName = pictureName;

            daoFactory = DAOFactory.getDAOFactory();
            employeeImageFileDAO = daoFactory.getEmployeeImageFileDAO(null);
		}

        public int Save(int employeeID, byte[] picture, bool doCommit)
        {
            int inserted;
            try
            {
                inserted = employeeImageFileDAO.insert(employeeID, picture, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        public bool Update(int employeeID, byte[] picture, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = employeeImageFileDAO.update(employeeID, picture, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Delete(int employeeID, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = employeeImageFileDAO.delete(employeeID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool DeleteAll(string employeeID, bool doCommit)
        {
            bool isDeleted = false;
            
            try
            {
                isDeleted = employeeImageFileDAO.deleteAll(employeeID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.DeleteAll(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        /*public ExtraHourTO Find(int employeeID, DateTime dateEarned)
        {
            ExtraHourTO extraHourTO = new ExtraHourTO();

            try
            {
                extraHourTO = employeeImageFileDAO.find(employeeID, dateEarned);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return extraHourTO;
        }*/

        public ArrayList Search(int employeeID)
        {
            ArrayList employeeImageFileTOList = new ArrayList();
            ArrayList employeeImageFileList = new ArrayList();

            try
            {
                EmployeeImageFile employeeImageFileMember = new EmployeeImageFile();
                employeeImageFileTOList = employeeImageFileDAO.getEmployeeImageFiles(employeeID);

                foreach (EmployeeImageFileTO eifTO in employeeImageFileTOList)
                {
                    employeeImageFileMember = new EmployeeImageFile();
                    employeeImageFileMember.ReceiveTransferObject(eifTO);

                    employeeImageFileList.Add(employeeImageFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employeeImageFileList;
        }

        public int SearchCount(int employeeID)
        {
            int count = -1;

            try
            {
                count = employeeImageFileDAO.getEmployeeImageFilesCount(employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

        public ArrayList SearchImageInfo(string[] statuses)
        {
            ArrayList employeeImageFileTOList = new ArrayList();
            ArrayList employeeImageFileList = new ArrayList();

            try
            {
                EmployeeImageFile employeeImageFileMember = new EmployeeImageFile();
                employeeImageFileTOList = employeeImageFileDAO.getEmployeeImageInfo(statuses);

                foreach (EmployeeImageFileTO eifTO in employeeImageFileTOList)
                {
                    employeeImageFileMember = new EmployeeImageFile();
                    employeeImageFileMember.ReceiveTransferObject(eifTO);

                    employeeImageFileList.Add(employeeImageFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.SearchImageInfo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employeeImageFileList;
        }

        public ArrayList SearchImageForSnapshots(string employeeID)
        {
            ArrayList employeeImageFileTOList = new ArrayList();
            ArrayList employeeImageFileList = new ArrayList();

            try
            {
                EmployeeImageFile employeeImageFileMember = new EmployeeImageFile();
                employeeImageFileTOList = employeeImageFileDAO.getEmployeeImageForSnapshots(employeeID);

                foreach (EmployeeImageFileTO eifTO in employeeImageFileTOList)
                {
                    employeeImageFileMember = new EmployeeImageFile();
                    employeeImageFileMember.ReceiveTransferObject(eifTO);

                    employeeImageFileList.Add(employeeImageFileMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.SearchImageForSnapshots(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employeeImageFileList;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = employeeImageFileDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                employeeImageFileDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                employeeImageFileDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return employeeImageFileDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                employeeImageFileDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeImageFile.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void ReceiveTransferObject(EmployeeImageFileTO employeeImageFileTO)
        {
            this.EmployeeID = employeeImageFileTO.EmployeeID;
            this.Picture = employeeImageFileTO.Picture;
            this.ModifiedTime = employeeImageFileTO.ModifiedTime;
            this.PictureName = employeeImageFileTO.PictureName;
        }

        public EmployeeImageFileTO SendTransferObject()
        {
            EmployeeImageFileTO employeeImageFileTO = new EmployeeImageFileTO();

            employeeImageFileTO.EmployeeID = this.EmployeeID;
            employeeImageFileTO.Picture = this.Picture;
            employeeImageFileTO.ModifiedTime = this.ModifiedTime;
            employeeImageFileTO.PictureName = this.PictureName;

            return employeeImageFileTO;
        }

        public void Clear()
        {
            this.EmployeeID = -1;
            this.Picture = null;
            this.ModifiedTime = new DateTime(0);
            this.PictureName = "";
        }
    }
}
