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

namespace DataAccess.MySQL
{
    public class MySQLLogAdditionalInfoDAO :LogAdditionalInfoDAO
    {
        MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
        
		public MySQLLogAdditionalInfoDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}
        public MySQLLogAdditionalInfoDAO(MySqlConnection mySqlConnection)
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


        #region LogAdditionalInfoDAO Members


        public int insert(LogAdditionalInfoTO logAdditionalInfoTO)
        {
            throw new NotImplementedException();
        }

        public List<LogAdditionalInfoTO> getLogs(LogAdditionalInfoTO logAdditionalInfoTO)
        {
            throw new NotImplementedException();
        }

        public bool update(LogAdditionalInfoTO logAdditionalInfoTO, bool doCommit)
        {
            throw new NotImplementedException();
        }

        public bool delete(int logID)
        {
            throw new NotImplementedException();
        }

        public LogAdditionalInfoTO find(int logID)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
