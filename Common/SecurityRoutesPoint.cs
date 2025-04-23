using System;
using System.Collections;
using System.Text;

using TransferObjects;
using Util;
using DataAccess;

namespace Common
{
    public class SecurityRoutesPoint
    {
        private int _controlPointID;
        private string _tagID;
        private string _tagName;
        private string _name;
        private string _description;

        private DAOFactory daoFactory;
        private SecurityRoutesPointDAO secRoutePointDAO;
        DebugLog log;

        public int ControlPointID
        {
            get { return _controlPointID; }
            set { _controlPointID = value; }
        }

        public string TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        public string TagName
        {
            get { return _tagName; }
            set { _tagName = value; }
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

        public SecurityRoutesPoint()
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRoutePointDAO = daoFactory.getSecurityRoutesPointDAO(null);
			
            // Init properties
            ControlPointID = -1;
            TagID = "";
            TagName = "";
            Name = "";
            Description = "";
        }

        public SecurityRoutesPoint(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRoutePointDAO = daoFactory.getSecurityRoutesPointDAO(dbConnection);

            // Init properties
            ControlPointID = -1;
            TagID = "";
            TagName = "";
            Name = "";
            Description = "";
        }

        public void ReceiveTransferObject(SecurityRoutesPointTO secRoutePointTO)
        {
            try
            {
                this.ControlPointID = secRoutePointTO.ControlPointID;
                this.TagID = secRoutePointTO.TagID;
                this.TagName = secRoutePointTO.TagName;
                this.Name = secRoutePointTO.Name;
                this.Description = secRoutePointTO.Description;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPoint.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public SecurityRoutesPointTO SendTransferObject()
        {
            SecurityRoutesPointTO secRoutePointTO = new SecurityRoutesPointTO();

            try
            {
                secRoutePointTO.ControlPointID = this.ControlPointID;
                secRoutePointTO.TagID = this.TagID;
                secRoutePointTO.TagName = this.TagName;
                secRoutePointTO.Name = this.Name;
                secRoutePointTO.Description = this.Description;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPoint.SendTransferObject(): " + ex.Message + "\n");
                throw ex;
            }

            return secRoutePointTO;
        }

        public ArrayList Search(int pointID, string name, string desc, string tagID)
        {
            // List that contins TO object
            ArrayList pointsTO = new ArrayList();
            ArrayList points = new ArrayList();

            try
            {
                pointsTO = secRoutePointDAO.getPoints(pointID, name, desc, tagID);
                SecurityRoutesPoint member;

                foreach (SecurityRoutesPointTO pointTO in pointsTO)
                {
                    member = new SecurityRoutesPoint();
                    member.ReceiveTransferObject(pointTO);

                    points.Add(member);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPoint.Search(): " + ex.Message + "\n");
                throw ex;
            }

            return points;
        }

        public bool Delete(int controlPointID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.secRoutePointDAO.delete(controlPointID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPoint.Delete(): " + ex.Message + "\n");
                throw ex;
            }

            return isDeleted;
        }

        public bool Update(int controlPointID, string name, string desc, string tagID)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = this.secRoutePointDAO.update(controlPointID, name, desc, tagID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPoint.Update(): " + ex.Message + "\n");
                throw ex;
            }

            return isUpdated;
        }

        public int Save(int controlPointID, string name, string desc, string tagID)
        {
            int rowsAffected = 0;

            try
            {
                rowsAffected = this.secRoutePointDAO.insert(controlPointID, name, desc, tagID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPoint.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return rowsAffected;
        }

        public int GetMaxID()
        {
            int max = 0;
            try
            {
                max = this.secRoutePointDAO.getMaxID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPoint.Save(): " + ex.Message + "\n");
                throw ex;
            }
            return max;
        }
    }
}
