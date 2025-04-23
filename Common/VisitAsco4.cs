using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;
using DataAccess;
using Util;

namespace Common
{
    public class VisitAsco4
    {
         private int _visitID = -1;
        private int _integerValue1= -1;
        private int _integerValue2= -1;
        private int _integerValue3= -1;
        private int _integerValue4= -1;
        private int _integerValue5= -1;
        private int _integerValue6 = -1;
        private int _integerValue7 = -1;
        private int _integerValue8 = -1;
        private int _integerValue9 = -1;
        private int _integerValue10 = -1;
        private DateTime _datetimeValue1 = new DateTime();
        private DateTime _datetimeValue2 = new DateTime();
        private DateTime _datetimeValue3 = new DateTime();
        private DateTime _datetimeValue4 = new DateTime();
        private DateTime _datetimeValue5 = new DateTime();
        private DateTime _datetimeValue6 = new DateTime();
        private DateTime _datetimeValue7 = new DateTime();
        private DateTime _datetimeValue8 = new DateTime();
        private DateTime _datetimeValue9 = new DateTime();
        private DateTime _datetimeValue10 = new DateTime();
        private string _nVarcharValue1 = "";
        private string _nVarcharValue2 = "";
        private string _nVarcharValue3 = "";
        private string _nVarcharValue4 = "";
        private string _nVarcharValue5 = "";
        private string _nVarcharValue6 = "";
        private string _nVarcharValue7 = "";
        private string _nVarcharValue8 = "";
        private string _nVarcharValue9 = "";
        private string _nVarcharValue10 = "";

        public string NVarcharValue10
        {
            get { return _nVarcharValue10; }
            set { _nVarcharValue10 = value; }
        }

        public string NVarcharValue9
        {
            get { return _nVarcharValue9; }
            set { _nVarcharValue9 = value; }
        }

        /// <summary>
        /// Local for ZIN.
        /// </summary>
        public string NVarcharValue8
        {
            get { return _nVarcharValue8; }
            set { _nVarcharValue8 = value; }
        }
        /// <summary>
        /// Registration number for ZIN.
        /// </summary>
        public string NVarcharValue7
        {
            get { return _nVarcharValue7; }
            set { _nVarcharValue7 = value; }
        }
        /// <summary>
        /// Type of vehicle for ZIN.
        /// </summary>
        public string NVarcharValue6
        {
            get { return _nVarcharValue6; }
            set { _nVarcharValue6 = value; }
        }
        /// <summary>
        /// Number of work order for ZIN.
        /// </summary>
        public string NVarcharValue5
        {
            get { return _nVarcharValue5; }
            set { _nVarcharValue5 = value; }
        }
        /// <summary>
        /// State of issue for ZIN.
        /// </summary>
        public string NVarcharValue4
        {
            get { return _nVarcharValue4; }
            set { _nVarcharValue4 = value; }
        }

        /// <summary>
        /// Place of issue for ZIN.
        /// </summary>
        public string NVarcharValue3
        {
            get { return _nVarcharValue3; }
            set { _nVarcharValue3 = value; }
        }
        /// <summary>
        /// Document name for ZIN.
        /// </summary>
        public string NVarcharValue2
        {
            get { return _nVarcharValue2; }
            set { _nVarcharValue2 = value; }
        }
        /// <summary>
        /// Visitor for ZIN.
        /// </summary>
        public string NVarcharValue1
        {
            get { return _nVarcharValue1; }
            set { _nVarcharValue1 = value; }
        }

        public DateTime DatetimeValue10
        {
            get { return _datetimeValue10; }
            set { _datetimeValue10 = value; }
        }

        public DateTime DatetimeValue9
        {
            get { return _datetimeValue9; }
            set { _datetimeValue9 = value; }
        }

        public DateTime DatetimeValue8
        {
            get { return _datetimeValue8; }
            set { _datetimeValue8 = value; }
        }

        public DateTime DatetimeValue7
        {
            get { return _datetimeValue7; }
            set { _datetimeValue7 = value; }
        }

        public DateTime DatetimeValue6
        {
            get { return _datetimeValue6; }
            set { _datetimeValue6 = value; }
        }


        public DateTime DatetimeValue5
        {
            get { return _datetimeValue5; }
            set { _datetimeValue5 = value; }
        }

        public DateTime DatetimeValue4
        {
            get { return _datetimeValue4; }
            set { _datetimeValue4 = value; }
        }

        public DateTime DatetimeValue3
        {
            get { return _datetimeValue3; }
            set { _datetimeValue3 = value; }
        }
        /// <summary>
        /// Ban date for ZIN.
        /// </summary>
        public DateTime DatetimeValue2
        {
            get { return _datetimeValue2; }
            set { _datetimeValue2 = value; }
        }
        /// <summary>
        /// Privacy statement for ZIN.
        /// </summary>
        public DateTime DatetimeValue1
        {
            get { return _datetimeValue1; }
            set { _datetimeValue1 = value; }
        }

        public int IntegerValue10
        {
            get { return _integerValue10; }
            set { _integerValue10 = value; }
        }

        public int IntegerValue9
        {
            get { return _integerValue9; }
            set { _integerValue9 = value; }
        }

        public int IntegerValue8
        {
            get { return _integerValue8; }
            set { _integerValue8 = value; }
        }
       
        public int IntegerValue7
        {
            get { return _integerValue7; }
            set { _integerValue7 = value; }
        }
        
        public int IntegerValue6
        {
            get { return _integerValue6; }
            set { _integerValue6 = value; }
        }


        public int IntegerValue5
        {
            get { return _integerValue5; }
            set { _integerValue5 = value; }
        }

        public int IntegerValue4
        {
            get { return _integerValue4; }
            set { _integerValue4 = value; }
        }

        public int IntegerValue3
        {
            get { return _integerValue3; }
            set { _integerValue3 = value; }
        }
       
        public int IntegerValue2
        {
            get { return _integerValue2; }
            set { _integerValue2 = value; }
        }
        /// <summary>
        /// Ban active(0-yes/1-no) for ZIN.
        /// </summary>
        public int IntegerValue1
        {
            get { return _integerValue1; }
            set { _integerValue1 = value; }
        }

        public int VisitID
        {
            get { return _visitID; }
            set { _visitID = value; }
        }

        DAOFactory daoFactory = null;
        VisitAsco4DAO vdao = null;

        DebugLog log;


        public VisitAsco4()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            vdao = daoFactory.getVisitAsco4DAO(null);

            this.VisitID = -1;
            this.IntegerValue1 = -1;
            this.IntegerValue2 = -1;
            this.IntegerValue3 = -1;
            this.IntegerValue4 = -1;
            this.IntegerValue5 = -1;
            this.IntegerValue6 = -1;
            this.IntegerValue7 = -1;
            this.IntegerValue8 = -1;
            this.IntegerValue9 = -1;
            this.IntegerValue10 = -1;
            this.DatetimeValue1 = new DateTime();
            this.DatetimeValue2 = new DateTime();
            this.DatetimeValue3 = new DateTime();
            this.DatetimeValue4 = new DateTime();
            this.DatetimeValue5 = new DateTime();
            this.DatetimeValue6 = new DateTime();
            this.DatetimeValue7 = new DateTime();
            this.DatetimeValue8 = new DateTime();
            this.DatetimeValue9 = new DateTime();
            this.DatetimeValue10 = new DateTime();
            this.NVarcharValue1 = "";
            this.NVarcharValue2 = "";
            this.NVarcharValue3 = "";
            this.NVarcharValue4 = "";
            this.NVarcharValue5 = "";
            this.NVarcharValue6 = "";
            this.NVarcharValue7 = "";
            this.NVarcharValue8 = "";
            this.NVarcharValue9 = "";
            this.NVarcharValue10 = "";
        }

        public VisitAsco4(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            vdao = daoFactory.getVisitAsco4DAO(dbConnection);

            this.VisitID = -1;
            this.IntegerValue1 = -1;
            this.IntegerValue2 = -1;
            this.IntegerValue3 = -1;
            this.IntegerValue4 = -1;
            this.IntegerValue5 = -1;
            this.IntegerValue6 = -1;
            this.IntegerValue7 = -1;
            this.IntegerValue8 = -1;
            this.IntegerValue9 = -1;
            this.IntegerValue10 = -1;
            this.DatetimeValue1 = new DateTime();
            this.DatetimeValue2 = new DateTime();
            this.DatetimeValue3 = new DateTime();
            this.DatetimeValue4 = new DateTime();
            this.DatetimeValue5 = new DateTime();
            this.DatetimeValue6 = new DateTime();
            this.DatetimeValue7 = new DateTime();
            this.DatetimeValue8 = new DateTime();
            this.DatetimeValue9 = new DateTime();
            this.DatetimeValue10 = new DateTime();
            this.NVarcharValue1 = "";
            this.NVarcharValue2 = "";
            this.NVarcharValue3 = "";
            this.NVarcharValue4 = "";
            this.NVarcharValue5 = "";
            this.NVarcharValue6 = "";
            this.NVarcharValue7 = "";
            this.NVarcharValue8 = "";
            this.NVarcharValue9 = "";
            this.NVarcharValue10 = "";
        }

        public bool save()
        {
            bool saved = false;
            try
            {
                saved = vdao.insert(this.VisitID, this.IntegerValue1, this.IntegerValue2, this.IntegerValue3, this.IntegerValue4, this.IntegerValue5,
                    this.IntegerValue6, this.IntegerValue7, this.IntegerValue8, this.IntegerValue9, this.IntegerValue10,
                    this.DatetimeValue1, this.DatetimeValue2, this.DatetimeValue3, this.DatetimeValue4, this.DatetimeValue5, 
                    this.DatetimeValue6, this.DatetimeValue7, this.DatetimeValue8, this.DatetimeValue9, this.DatetimeValue10, 
                    this.NVarcharValue1, this.NVarcharValue2, this.NVarcharValue3, this.NVarcharValue4, this.NVarcharValue5,
                    this.NVarcharValue6, this.NVarcharValue7, this.NVarcharValue8, this.NVarcharValue9, this.NVarcharValue10);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + VisitID.ToString() + "\n");
                throw ex;
            }

            return saved;
        }
        public bool save(bool doCommit)
        {
            bool saved = false;
            try
            {
                saved = vdao.insert(this.VisitID, this.IntegerValue1, this.IntegerValue2, this.IntegerValue3, this.IntegerValue4, this.IntegerValue5,
                    this.IntegerValue6, this.IntegerValue7, this.IntegerValue8, this.IntegerValue9, this.IntegerValue10,
                    this.DatetimeValue1, this.DatetimeValue2, this.DatetimeValue3, this.DatetimeValue4, this.DatetimeValue5, 
                    this.DatetimeValue6, this.DatetimeValue7, this.DatetimeValue8, this.DatetimeValue9, this.DatetimeValue10, 
                    this.NVarcharValue1, this.NVarcharValue2, this.NVarcharValue3, this.NVarcharValue4, this.NVarcharValue5,
                    this.NVarcharValue6, this.NVarcharValue7, this.NVarcharValue8, this.NVarcharValue9, this.NVarcharValue10, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + VisitID.ToString() + "\n");
                throw ex;
            }

            return saved;
        }

        public bool update()
        {
            bool update = false;
            try
            {
                update = vdao.update(this.VisitID, this.IntegerValue1, this.IntegerValue2, this.IntegerValue3, this.IntegerValue4, this.IntegerValue5,
                    this.IntegerValue6, this.IntegerValue7, this.IntegerValue8, this.IntegerValue9, this.IntegerValue10,
                    this.DatetimeValue1, this.DatetimeValue2, this.DatetimeValue3, this.DatetimeValue4, this.DatetimeValue5, 
                    this.DatetimeValue6, this.DatetimeValue7, this.DatetimeValue8, this.DatetimeValue9, this.DatetimeValue10, 
                    this.NVarcharValue1, this.NVarcharValue2, this.NVarcharValue3, this.NVarcharValue4, this.NVarcharValue5,
                    this.NVarcharValue6, this.NVarcharValue7, this.NVarcharValue8, this.NVarcharValue9, this.NVarcharValue10);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + VisitID.ToString() + "\n");
                throw ex;
            }

            return update;
        }
        public bool update(bool doCommit)
        {
            bool update = false;
            try
            {
                update = vdao.update(this.VisitID, this.IntegerValue1, this.IntegerValue2, this.IntegerValue3, this.IntegerValue4, this.IntegerValue5,
                    this.IntegerValue6, this.IntegerValue7, this.IntegerValue8, this.IntegerValue9, this.IntegerValue10,
                    this.DatetimeValue1, this.DatetimeValue2, this.DatetimeValue3, this.DatetimeValue4, this.DatetimeValue5,
                    this.DatetimeValue6, this.DatetimeValue7, this.DatetimeValue8, this.DatetimeValue9, this.DatetimeValue10,
                    this.NVarcharValue1, this.NVarcharValue2, this.NVarcharValue3, this.NVarcharValue4, this.NVarcharValue5,
                    this.NVarcharValue6, this.NVarcharValue7, this.NVarcharValue8, this.NVarcharValue9, this.NVarcharValue10, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + VisitID.ToString() + "\n");
                throw ex;
            }

            return update;
        }
        public bool delete(int visitID)
        {
            bool deleted = false;
            try
            {
                deleted = vdao.delete(visitID);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + VisitID.ToString() + "\n");
                throw ex;
            }

            return deleted;
        }
        public bool delete(int visitID, bool doCommit)
        {
            bool deleted = false;
            try
            {
                deleted = vdao.delete(visitID, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Save(): " + ex.Message + VisitID.ToString() + "\n");
                throw ex;
            }

            return deleted;
        }

        public DateTime getPrivacyStatementDate(string JMBG, string docNumber)
        {
            DateTime statementDate = new DateTime(); ;
            try
            {
                statementDate = vdao.getPrivacyStatement(JMBG, docNumber);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.getPrivacyStatementDate(): " + ex.Message + VisitID.ToString() + "\n");
                throw ex;
            }

            return statementDate;
        }

        public List<VisitAsco4> Search()
        {
            List<VisitAsco4TO> visitTOList = new List<VisitAsco4TO>();
            List<VisitAsco4> visitList = new List<VisitAsco4>();

            try
            {
                VisitAsco4 visitMember = new VisitAsco4();

                visitTOList = vdao.getVisitsAsco(this.VisitID, this.IntegerValue1, this.IntegerValue2, this.IntegerValue3, this.IntegerValue4, this.IntegerValue5,
                    this.IntegerValue6, this.IntegerValue7, this.IntegerValue8, this.IntegerValue9, this.IntegerValue10,
                    this.DatetimeValue1, this.DatetimeValue2, this.DatetimeValue3, this.DatetimeValue4, this.DatetimeValue5,
                    this.DatetimeValue6, this.DatetimeValue7, this.DatetimeValue8, this.DatetimeValue9, this.DatetimeValue10,
                    this.NVarcharValue1, this.NVarcharValue2, this.NVarcharValue3, this.NVarcharValue4, this.NVarcharValue5,
                    this.NVarcharValue6, this.NVarcharValue7, this.NVarcharValue8, this.NVarcharValue9, this.NVarcharValue10);

                foreach (VisitAsco4TO emplTO in visitTOList)
                {
                    visitMember = new VisitAsco4();
                    visitMember.ReceiveTransferObject(emplTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SearchVisitors(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitList;
        }

        public List<VisitAsco4> SearchDetails(string visitIDs)
        {
            List<VisitAsco4TO> visitTOList = new List<VisitAsco4TO>();
            List<VisitAsco4> visitList = new List<VisitAsco4>();

            try
            {
                VisitAsco4 visitMember = new VisitAsco4();

                visitTOList = vdao.getVisitsDetailsAsco(visitIDs);

                foreach (VisitAsco4TO emplTO in visitTOList)
                {
                    visitMember = new VisitAsco4();
                    visitMember.ReceiveTransferObject(emplTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SearchVisitors(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitList;
        }

        public List<VisitAsco4> Search(string JMBG, string documentNumber)
        {
            List<VisitAsco4TO> visitTOList = new List<VisitAsco4TO>();
            List<VisitAsco4> visitList = new List<VisitAsco4>();

            try
            {
                VisitAsco4 visitMember = new VisitAsco4();

                visitTOList = vdao.getVisitAscoForID(JMBG, documentNumber);
                foreach (VisitAsco4TO emplTO in visitTOList)
                {
                    visitMember = new VisitAsco4();
                    visitMember.ReceiveTransferObject(emplTO);

                    visitList.Add(visitMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SearchVisitors(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitList;
        }
        public VisitAsco4TO SendTransferObject()
        {
            VisitAsco4TO visitAscoTO = new VisitAsco4TO();

            visitAscoTO.VisitID = this.VisitID;
            visitAscoTO.IntegerValue1 = this.IntegerValue1;
            visitAscoTO.IntegerValue2 = this.IntegerValue2;
            visitAscoTO.IntegerValue3 = this.IntegerValue3;
            visitAscoTO.IntegerValue4 = this.IntegerValue4;
            visitAscoTO.IntegerValue5 = this.IntegerValue5;
            visitAscoTO.IntegerValue6 = this.IntegerValue6;
            visitAscoTO.IntegerValue7 = this.IntegerValue7;
            visitAscoTO.IntegerValue8 = this.IntegerValue8;
            visitAscoTO.IntegerValue9 = this.IntegerValue9;
            visitAscoTO.IntegerValue10 = this.IntegerValue10;
            visitAscoTO.DatetimeValue1 = this.DatetimeValue1;
            visitAscoTO.DatetimeValue2 = this.DatetimeValue2;
            visitAscoTO.DatetimeValue3 = this.DatetimeValue3;
            visitAscoTO.DatetimeValue4 = this.DatetimeValue4;
            visitAscoTO.DatetimeValue5 = this.DatetimeValue5;
            visitAscoTO.DatetimeValue6 = this.DatetimeValue6;
            visitAscoTO.DatetimeValue7 = this.DatetimeValue7;
            visitAscoTO.DatetimeValue8 = this.DatetimeValue8;
            visitAscoTO.DatetimeValue9 = this.DatetimeValue9;
            visitAscoTO.DatetimeValue10 = this.DatetimeValue10;
            visitAscoTO.NVarcharValue1 = this.NVarcharValue1;
            visitAscoTO.NVarcharValue2 = this.NVarcharValue2;
            visitAscoTO.NVarcharValue3 = this.NVarcharValue3;
            visitAscoTO.NVarcharValue4 = this.NVarcharValue4;
            visitAscoTO.NVarcharValue5 = this.NVarcharValue5;
            visitAscoTO.NVarcharValue6 = this.NVarcharValue6;
            visitAscoTO.NVarcharValue7 = this.NVarcharValue7;
            visitAscoTO.NVarcharValue8 = this.NVarcharValue8;
            visitAscoTO.NVarcharValue9 = this.NVarcharValue9;
            visitAscoTO.NVarcharValue10 = this.NVarcharValue10;

            return visitAscoTO;
        }

        public void ReceiveTransferObject(VisitAsco4TO visitAscoTO)
        {
            this.VisitID = visitAscoTO.VisitID;
            this.IntegerValue1 = visitAscoTO.IntegerValue1;
            this.IntegerValue2 = visitAscoTO.IntegerValue2;
            this.IntegerValue3 = visitAscoTO.IntegerValue3;
            this.IntegerValue4 = visitAscoTO.IntegerValue4;
            this.IntegerValue5 = visitAscoTO.IntegerValue5;
            this.IntegerValue6 = visitAscoTO.IntegerValue6;
            this.IntegerValue7 = visitAscoTO.IntegerValue7;
            this.IntegerValue8 = visitAscoTO.IntegerValue8;
            this.IntegerValue9 = visitAscoTO.IntegerValue9;
            this.IntegerValue10 = visitAscoTO.IntegerValue10;
            this.DatetimeValue1 = visitAscoTO.DatetimeValue1;
            this.DatetimeValue2 = visitAscoTO.DatetimeValue2;
            this.DatetimeValue3 = visitAscoTO.DatetimeValue3;
            this.DatetimeValue4 = visitAscoTO.DatetimeValue4;
            this.DatetimeValue5 = visitAscoTO.DatetimeValue5;
            this.DatetimeValue6 = visitAscoTO.DatetimeValue6;
            this.DatetimeValue7 = visitAscoTO.DatetimeValue7;
            this.DatetimeValue8 = visitAscoTO.DatetimeValue8;
            this.DatetimeValue9 = visitAscoTO.DatetimeValue9;
            this.DatetimeValue10 = visitAscoTO.DatetimeValue10;
            this.NVarcharValue1 = visitAscoTO.NVarcharValue1;
            this.NVarcharValue2 = visitAscoTO.NVarcharValue2;
            this.NVarcharValue3 = visitAscoTO.NVarcharValue3;
            this.NVarcharValue4 = visitAscoTO.NVarcharValue4;
            this.NVarcharValue5 = visitAscoTO.NVarcharValue5;
            this.NVarcharValue6 = visitAscoTO.NVarcharValue6;
            this.NVarcharValue7 = visitAscoTO.NVarcharValue7;
            this.NVarcharValue8 = visitAscoTO.NVarcharValue8;
            this.NVarcharValue9 = visitAscoTO.NVarcharValue9;
            this.NVarcharValue10 = visitAscoTO.NVarcharValue10;
        }
        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = vdao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                vdao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                vdao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return vdao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                vdao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }


        public List<string> SearchDistinctVisitors()
        {
            List<string> visitors = new List<string>();
            try
            {
                visitors = vdao.getDistinctVisitors();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitors;
        }

        public List<string> SearchDistinct(string column)
        {
            List<string> visitors = new List<string>();
            try
            {
                visitors = vdao.getDistinct(column);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitors;
        }

        public List<string> SearchDistinctDocumentNames()
        {
            List<string> docs = new List<string>();
            try
            {
                docs = vdao.getDistinctDocumentNames();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return docs;
        }

        public int enterBan(string visitID, int banActive, DateTime banDate)
        {
            int entered = 0;
            try
            {
                entered = vdao.enterBan(visitID, banActive, banDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.enterBan(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return entered;

        }

        public List<VisitAsco4> Search(string firstName, string lastName, string jmbg, string documentNum, string company)
        {
            
                List<VisitAsco4TO> visitTOList = new List<VisitAsco4TO>();
                List<VisitAsco4> visitList = new List<VisitAsco4>();

                try
                {
                    VisitAsco4 visitMember = new VisitAsco4();

                    visitTOList = vdao.getVisitAsco(firstName,lastName,jmbg,documentNum,company);
                    foreach (VisitAsco4TO emplTO in visitTOList)
                    {
                        visitMember = new VisitAsco4();
                        visitMember.ReceiveTransferObject(emplTO);

                        visitList.Add(visitMember);
                    }
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " VisitAsco4.Search(): " + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }
                return visitList;
            

        }

        public List<Visit> SearchFirstJMBG(string firstName, string lastName, string jmbg, Dictionary<int, VisitAsco4> asco)
        {
            List<VisitTO> visitTOList = new List<VisitTO>();
            List<Visit> visitList = new List<Visit>();

            try
            {
                Visit visitMember = new Visit();
                VisitAsco4 visitAsco4Member = new VisitAsco4();
                Dictionary<int, List<VisitAsco4TO>> ascoTO = new Dictionary<int, List<VisitAsco4TO>>();
                visitTOList = vdao.getVisitAsco(firstName, lastName, jmbg,ascoTO);
                foreach (VisitTO emplTO in visitTOList)
                {
                    visitMember = new Visit();
                    visitMember.receiveTransferObject(emplTO);

                    visitList.Add(visitMember);
                }
                foreach (int i in ascoTO.Keys)
                {
                    visitAsco4Member = new VisitAsco4();
                    VisitAsco4TO vaTO = ascoTO[i][0];
                    visitAsco4Member.ReceiveTransferObject(vaTO);
                    if (!asco.ContainsKey(i))
                    {
                        asco.Add(i, visitAsco4Member);
                    }

                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitAsco4.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return visitList;
            
        }
    }
}
