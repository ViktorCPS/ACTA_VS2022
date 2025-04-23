using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;
using DataAccess;
using Util;

namespace Common
{
    public class EmployeeAsco4Metadata
    {
        DAOFactory daoFactory = null;
        EmployeeAsco4MetadataDAO edao = null;

        DebugLog log;

        EmployeeAsco4MetadataTO emplAsco4MetadataTO = new EmployeeAsco4MetadataTO();

        public EmployeeAsco4MetadataTO EmplAsco4MetadataTO
        {
            get { return emplAsco4MetadataTO; }
            set { emplAsco4MetadataTO = value; }
        }

        public EmployeeAsco4Metadata()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeeAsco4MetadataDAO(null);
		}

        public EmployeeAsco4Metadata(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeAsco4MetadataDAO(dbConnection);
        }

        public Dictionary<string, string> GetMetadataValues(string lang)
        {
            try
            {
                return edao.getEmployeeAsco4MetadataValues(lang);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4Metadata.GetMetadataValues(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<string, string> GetMetadataWebValues(string lang)
        {
            try
            {
                return edao.getEmployeeAsco4MetadataWebValues(lang);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4Metadata.GetMetadataWebValues(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
