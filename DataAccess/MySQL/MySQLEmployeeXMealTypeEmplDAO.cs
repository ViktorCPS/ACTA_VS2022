using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLEmployeeXMealTypeEmplDAO:EmployeeXMealTypeEmplDAO
    {
        MySqlConnection conn = null;
		//protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MySQLEmployeeXMealTypeEmplDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}
        public MySQLEmployeeXMealTypeEmplDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
        }
        public int insert(int employeeID, int mealTypeEmplId)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_assigned ");
                sbInsert.Append("(employee_id, meals_type_empl_id, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (employeeID != -1)
                {
                    sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (mealTypeEmplId != -1)
                {
                    sbInsert.Append("'" + mealTypeEmplId.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                DataSet dataSet = new DataSet();
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();

            }
            
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(string employeeID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_x_meal_type_employees WHERE employee_id IN (" + employeeID + " )");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }



        public bool beginTransaction()
        {
            bool isStarted = false;

            try
            {
                this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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
    }
}
