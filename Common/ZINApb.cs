using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class ZINApb
    {
        private int _recID = -1;
        private int _employeeID = -1;
        private string _cardNum = "";
        private string _nVarcharValue1 = "";
        private string _nVarcharValue2 = "";
        private string _nVarcharValue3 = "";
        private string _nVarcharValue4 = "";
        private string _nVarcharValue5 = "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();

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

        public string NVarcharValue5
        {
            get { return _nVarcharValue5; }
            set { _nVarcharValue5 = value; }
        }
        /// <summary>
        /// Razlog neregularnog prolaska
        /// </summary>
        public string NVarcharValue4
        {
            get { return _nVarcharValue4; }
            set { _nVarcharValue4 = value; }
        }
        /// <summary>
        /// Smer
        /// </summary>
        public string NVarcharValue3
        {
            get { return _nVarcharValue3; }
            set { _nVarcharValue3 = value; }
        }
        
        public string NVarcharValue2
        {
            get { return _nVarcharValue2; }
            set { _nVarcharValue2 = value; }
        }
        /// <summary>
        /// Prijavnica
        /// </summary>
        public string NVarcharValue1
        {
            get { return _nVarcharValue1; }
            set { _nVarcharValue1 = value; }
        }

        public string CardNum
        {
            get { return _cardNum; }
            set { _cardNum = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        DAOFactory daoFactory = null;
        ZINApbDAO zindao = null;

        DebugLog log;

         public ZINApb()
         {
             string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
             log = new DebugLog(logFilePath);

             daoFactory = DAOFactory.getDAOFactory();
             zindao = daoFactory.getZINApbDAO(null);

         }

         public ZINApb(object dbConnection)
         {
             string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
             log = new DebugLog(logFilePath);

             daoFactory = DAOFactory.getDAOFactory();
             zindao = daoFactory.getZINApbDAO(dbConnection);

         }
        public bool save()
        {
            bool saved = false;
            try
            {
                saved = zindao.insert(this.RecID, this.EmployeeID, this.CardNum, 
                    this.NVarcharValue1, this.NVarcharValue2, this.NVarcharValue3, this.NVarcharValue4, this.NVarcharValue5);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + RecID.ToString() + "\n");
                throw ex;
            }

            return saved;
        }
        public bool update()
        {
            bool update = false;
            try
            {
                update = zindao.update(this.RecID, this.EmployeeID, this.CardNum,
                    this.NVarcharValue1, this.NVarcharValue2, this.NVarcharValue3, this.NVarcharValue4, this.NVarcharValue5);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + RecID.ToString() + "\n");
                throw ex;
            }

            return update;
        }
        public bool delete(int recID)
        {
            bool deleted = false;
            try
            {
                deleted = zindao.delete(recID);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + recID.ToString() + "\n");
                throw ex;
            }

            return deleted;
        }

         public List<string> SearchDistinct(string columnName)
        {
            List<string> docs = new List<string>();
            try
            {
                docs = zindao.getDistinct(columnName);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return docs;

        }

        public List<ZINApb> Search(DateTime fromTime, DateTime toTime,string wUnits)
        {
            List<ZINApbTO> apbTOList = new List<ZINApbTO>();
            List<ZINApb> apbList = new List<ZINApb>();

            try
            {
                ZINApb apbMember = new ZINApb();

                apbTOList = zindao.getApbsAsco(this.RecID, this.EmployeeID, this.CardNum,
                    this.NVarcharValue1, this.NVarcharValue2, this.NVarcharValue3, this.NVarcharValue4, this.NVarcharValue5, this.CreatedBy,fromTime,toTime, wUnits);

                foreach (ZINApbTO emplTO in apbTOList)
                {
                    apbMember = new ZINApb();
                    apbMember.ReceiveTransferObject(emplTO);

                    apbList.Add(apbMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SearchVisitors(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return apbList;
        }
        public ZINApbTO SendTransferObject()
        {
            ZINApbTO zinApbTO = new ZINApbTO();

            zinApbTO.RecID= this.RecID;
            zinApbTO.EmployeeID = this.EmployeeID;
            zinApbTO.CardNum = this.CardNum;
            zinApbTO.CreatedBy = this.CreatedBy;
            zinApbTO.CreatedTime = this.CreatedTime;          
            zinApbTO.NVarcharValue1 = this.NVarcharValue1;
            zinApbTO.NVarcharValue2 = this.NVarcharValue2;
            zinApbTO.NVarcharValue3 = this.NVarcharValue3;
            zinApbTO.NVarcharValue4 = this.NVarcharValue4;
            zinApbTO.NVarcharValue5 = this.NVarcharValue5;

            return zinApbTO;
        }

        public void ReceiveTransferObject(ZINApbTO zinApbTO)
        {
            this.RecID = zinApbTO.RecID;
            this.EmployeeID = zinApbTO.EmployeeID;
            this.CardNum = zinApbTO.CardNum;
            this.CreatedBy = zinApbTO.CreatedBy;
            this.CreatedTime = zinApbTO.CreatedTime;          
            this.NVarcharValue1 = zinApbTO.NVarcharValue1;
            this.NVarcharValue2 = zinApbTO.NVarcharValue2;
            this.NVarcharValue3 = zinApbTO.NVarcharValue3;
            this.NVarcharValue4 = zinApbTO.NVarcharValue4;
            this.NVarcharValue5 = zinApbTO.NVarcharValue5;
        }

       
    }
}
