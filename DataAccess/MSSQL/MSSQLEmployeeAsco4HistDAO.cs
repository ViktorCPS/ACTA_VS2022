using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
   public class MSSQLEmployeeAsco4HistDAO:EmployeeAsco4HistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLEmployeeAsco4HistDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
        }

        public MSSQLEmployeeAsco4HistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
        }

        public bool insert(int emplID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
           DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5
           , string string6, string string7, string string8, string string9, string string10, string createdBy, DateTime createdTime, bool doCommit)
        {
            bool isInserted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }


            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employees_asco4_hist ");
                sbInsert.Append("(employee_id,integer_value_1, integer_value_2, integer_value_3, integer_value_4, integer_value_5,integer_value_6, integer_value_7, integer_value_8, integer_value_9, integer_value_10,");
                sbInsert.Append("datetime_value_1,datetime_value_2,datetime_value_3,datetime_value_4,datetime_value_5,datetime_value_6,datetime_value_7,datetime_value_8,datetime_value_9,datetime_value_10,");
                sbInsert.Append("nvarchar_value_1,nvarchar_value_2,nvarchar_value_3,nvarchar_value_4,nvarchar_value_5,nvarchar_value_6,nvarchar_value_7,nvarchar_value_8,nvarchar_value_9,nvarchar_value_10, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(emplID + ", ");
                if (int1 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int1 + ", ");
                }
                if (int2 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int2 + ", ");
                }
                if (int3 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int3 + ", ");
                }
                if (int4 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int4 + ", ");
                }
                if (int5 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int5 + ", ");
                }
                if (int6 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int6 + ", ");
                }
                if (int7 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int7 + ", ");
                }
                if (int8 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int8 + ", ");
                }
                if (int9 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int9 + ", ");
                }
                if (int10 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int10 + ", ");
                }
                if (dateTime1.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime2.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime3.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime4.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime5.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime6.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime7.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime8.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime9.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime10.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (string1.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string1 + "', ");
                }
                if (string2.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string2 + "', ");
                }
                if (string3.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string3 + "', ");
                }
                if (string4.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string4 + "', ");
                }
                if (string5.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string5 + "', ");
                }
                if (string6.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string6 + "', ");
                }
                if (string7.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string7 + "', ");
                }
                if (string8.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string8 + "', ");
                }
                if (string9.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string9 + "', ");
                }
                if (string10.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string10 + "', ");
                }
                sbInsert.Append("N'" + createdBy + "', '"+createdTime.ToString("yyy-MM-dd HH:mm:ss")+"', "); 
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()); ");


                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
                if (rowsAffected > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }

                throw ex;
            }

            return isInserted;
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
            _sqlTrans = (SqlTransaction)trans;
        }
    }
}
