using System;
using System.Collections;
using System.Text;

using TransferObjects;
using DataAccess;
using Util;

namespace Common
{
    public class SecurityRouteHdr
    {
        private int _securityRouteID;
        private string _name;
        private string _description;
        private string _routeType;

        public Hashtable Segments;

        private DAOFactory daoFactory;
        private SecurityRouteDAO secRouteDAO;
        DebugLog log;

        public int SecurityRouteID
        {
            get { return _securityRouteID; }
            set { _securityRouteID = value; }
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

        public string RouteType
        {
            get { return _routeType; }
            set { _routeType = value; }
        }

        public SecurityRouteHdr()
		{
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteDAO = daoFactory.getSecurityRouteDAO(null);
			
            // Init properties
            SecurityRouteID = -1;
			Name = "";
			Description = "";
            RouteType = "";
			Segments = new Hashtable();
		}

        public SecurityRouteHdr(object dbConection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            secRouteDAO = daoFactory.getSecurityRouteDAO(dbConection);

            // Init properties
            SecurityRouteID = -1;
            Name = "";
            Description = "";
            RouteType = "";
            Segments = new Hashtable();
        }

        public void ReceiveTransferObject(SecurityRouteHdrTO secRouteTO)
        {
            try
            {
                this.SecurityRouteID = secRouteTO.SecurityRouteID;
                this.Name = secRouteTO.Name;
                this.Description = secRouteTO.Description;
                this.RouteType = secRouteTO.RouteType;

                Hashtable segmentsTO = new Hashtable();
                segmentsTO = secRouteTO.Segments;
                
                SecurityRouteDtl secRouteDtl = new SecurityRouteDtl();

                foreach (int key in segmentsTO.Keys)
                {
                    secRouteDtl = new SecurityRouteDtl();
                    secRouteDtl.ReceiveTransferObject((SecurityRouteDtlTO)segmentsTO[key]);

                    this.Segments.Add(key, secRouteDtl);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteHdr.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public SecurityRouteHdrTO SendTransferObject()
        {
            SecurityRouteHdrTO secRouteTO = new SecurityRouteHdrTO();

            try
            {
                secRouteTO.SecurityRouteID = this.SecurityRouteID;
                secRouteTO.Name = this.Name;
                secRouteTO.Description = this.Description;
                secRouteTO.RouteType = this.RouteType;

                Hashtable segments = new Hashtable();
                segments = (Hashtable)this.Segments;

                SecurityRouteDtlTO secRouteDtlTO = new SecurityRouteDtlTO();

                foreach (int key in segments.Keys)
                {
                    secRouteDtlTO = ((SecurityRouteDtl)segments[key]).SendTransferObject();

                    secRouteTO.Segments.Add(key, secRouteDtlTO);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteHdr.SendTransferObject(): " + ex.Message + "\n");
                throw ex;
            }

            return secRouteTO;
        }

        public int Save()
        {
            int affectedRows;
            try
            {
                affectedRows = this.secRouteDAO.insert(this.SendTransferObject());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRouteHdr.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return affectedRows;
        }

        public ArrayList Search(string name, string desc)
        {
            // List that contins TO object
            ArrayList routesTO = new ArrayList();
            ArrayList routes = new ArrayList();

            try
            {
                routesTO = secRouteDAO.getRoutes(name, desc);
                SecurityRouteHdr member;

                foreach (SecurityRouteHdrTO routeTO in routesTO)
                {
                    member = new SecurityRouteHdr();
                    member.ReceiveTransferObject(routeTO);

                    routes.Add(member);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesHdr.Search(): " + ex.Message + "\n");
                throw ex;
            }

            return routes;
        }

        public ArrayList SearchDetailsTag(int securityRouteID)
        {
            // List that contins TO object
            ArrayList routesTO = new ArrayList();
            ArrayList routes = new ArrayList();

            try
            {
                routesTO = secRouteDAO.getRoutesDetailsTag(securityRouteID);
                SecurityRouteDtl member;

                foreach (SecurityRouteDtlTO routeTO in routesTO)
                {
                    member = new SecurityRouteDtl();
                    member.ReceiveTransferObject(routeTO);

                    routes.Add(member);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesHdr.SearchDetailsTag(): " + ex.Message + "\n");
                throw ex;
            }

            return routes;
        }

        public ArrayList SearchDetailsTerminal(int securityRouteID)
        {
            // List that contins TO object
            ArrayList routesTO = new ArrayList();
            ArrayList routes = new ArrayList();

            try
            {
                routesTO = secRouteDAO.getRoutesDetailsTerminal(securityRouteID);
                SecurityRouteDtl member;

                foreach (SecurityRouteDtlTO routeTO in routesTO)
                {
                    member = new SecurityRouteDtl();
                    member.ReceiveTransferObject(routeTO);

                    routes.Add(member);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesHdr.SearchDetailsTerminal(): " + ex.Message + "\n");
                throw ex;
            }

            return routes;
        }

        public bool Delete(int securityRouteID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.secRouteDAO.delete(securityRouteID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesHdr.Delete(): " + ex.Message + "\n");
                throw ex;
            }

            return isDeleted;
        }
    }
}
