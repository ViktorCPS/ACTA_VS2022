using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;
using System.Data.SqlClient;

namespace DataAccess.MySQL
{
    public class MySQLLogTmpAdditionalInfoDAO : LogTmpAdditionalInfoDAO
    {

        
        MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
        
		public MySQLLogTmpAdditionalInfoDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}
        public MySQLLogTmpAdditionalInfoDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
        }


        public bool beginTransaction()
        {
            bool isStarted = false;

            try
            {
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                isStarted = true;
            }
            catch (Exception ex)
            {
                isStarted = false;
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void commitTransaction()
        {
            this.SqlTrans.Commit();
            this.SqlTrans = null;
        }

        public void rollbackTransaction()
        {
            this.SqlTrans.Rollback();
            this.SqlTrans = null;
        }

        public IDbTransaction getTransaction()
        {
            return _sqlTrans;
        }

        public void setTransaction(IDbTransaction trans)
        {
            _sqlTrans = (MySqlTransaction)trans;
        }

        #region LogTmpAdditionalInfoDAO Members


        public int insert(LogTmpAdditionalInfoTO logTmpTO)
        {
            throw new NotImplementedException();
        }

        public List<LogTmpAdditionalInfoTO> getLogs(LogTmpAdditionalInfoTO logTmpAdditionalInfoTO)
        {
            throw new NotImplementedException();
        }

        public bool update(LogTmpAdditionalInfoTO logTmpTo, bool doCommit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region LogTmpAdditionalInfoDAO Members


        public bool delete(int readerID, uint tagID, int antenna, DateTime eventTime)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region LogTmpAdditionalInfoDAO Members


        public LogTmpAdditionalInfoTO find(int readerID, uint tagID, int antenna, DateTime eventTime)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
