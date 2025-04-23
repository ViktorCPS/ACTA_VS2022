using System;
using System.Collections;
using System.Text;

using TransferObjects;
using Util;
using DataAccess;

namespace Common
{
    public class SecurityRoutesReader
    {
        private int _readerID;
        private string _name;
        private string _description;

        private DAOFactory daoFactory;
        private SecurityRoutesReaderDAO secRouteReaderDAO;
        DebugLog log;

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public SecurityRoutesReader()
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteReaderDAO = daoFactory.getSecurityRoutesReaderDAO(null);
			
            // Init properties
            ReaderID = -1;
            Name = "";
            Description = "";
        }

        public SecurityRoutesReader(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteReaderDAO = daoFactory.getSecurityRoutesReaderDAO(dbConnection);

            // Init properties
            ReaderID = -1;
            Name = "";
            Description = "";
        }

        public void ReceiveTransferObject(SecurityRoutesReaderTO secRouteReaderTO)
        {
            try
            {
                this.ReaderID = secRouteReaderTO.ReaderID;
                this.Name = secRouteReaderTO.Name;
                this.Description = secRouteReaderTO.Description;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReader.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public SecurityRoutesReaderTO SendTransferObject()
        {
            SecurityRoutesReaderTO secRouteReaderTO = new SecurityRoutesReaderTO();

            try
            {
                secRouteReaderTO.ReaderID = this.ReaderID;
                secRouteReaderTO.Name = this.Name;
                secRouteReaderTO.Description = this.Description;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReader.SendTransferObject(): " + ex.Message + "\n");
                throw ex;
            }

            return secRouteReaderTO;
        }

        public ArrayList Search(int readerID, string name, string desc)
        {
            // List that contins TO object
            ArrayList readersTO = new ArrayList();
            ArrayList readers = new ArrayList();

            try
            {
                readersTO = secRouteReaderDAO.getReaders(readerID, name, desc);
                SecurityRoutesReader member;

                foreach (SecurityRoutesReaderTO readerTO in readersTO)
                {
                    member = new SecurityRoutesReader();
                    member.ReceiveTransferObject(readerTO);

                    readers.Add(member);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReader.Search(): " + ex.Message + "\n");
                throw ex;
            }

            return readers;
        }

        public bool Delete(int readerID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.secRouteReaderDAO.delete(readerID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReader.Delete(): " + ex.Message + "\n");
                throw ex;
            }

            return isDeleted;
        }

        public bool Update(int readerID, string name, string desc)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = this.secRouteReaderDAO.update(readerID, name, desc);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReader.Update(): " + ex.Message + "\n");
                throw ex;
            }

            return isUpdated;
        }

        public int Save(string name, string desc)
        {
            int rowsAffected = 0;

            try
            {
                rowsAffected = this.secRouteReaderDAO.insert(name, desc);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReader.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return rowsAffected;
        }
    }
}
