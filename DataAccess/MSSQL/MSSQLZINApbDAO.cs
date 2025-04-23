using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public class MSSQLZINApbDAO:ZINApbDAO
    {

       
        SqlConnection conn = null;
		//protected string dateTimeformat = "";
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLZINApbDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}
        public MSSQLZINApbDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
        }
        #region ZINApbDAO Members

        public bool insert(int recID, int employeeID, string cardNum, string string1, string string2, string string3, string string4, string string5)
        {
            bool isInserted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO zin_apb ");
                sbInsert.Append("(employee_id, card_num, nvarchar_value_1,nvarchar_value_2,nvarchar_value_3,nvarchar_value_4,nvarchar_value_5,created_by, created_time) ");
                sbInsert.Append("VALUES (");
               
                if (employeeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(employeeID + ", ");
                }
                if (cardNum.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'"+cardNum + "', ");
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
               
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()); ");


                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                int rowsAffected = cmd.ExecuteNonQuery();

                sqlTrans.Commit();

                if (rowsAffected > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();

                throw ex;
            }

            return isInserted;
        }

        public bool update(int recID, int employeeID, string cardNum, string string1, string string2, string string3, string string4, string string5)
        {
           bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            

            try
            {
                StringBuilder sbUpdate = new StringBuilder();

                sbUpdate.Append("UPDATE zin_apb SET ");
                if (employeeID != -1)
                {
                    sbUpdate.Append("employee_id = " + employeeID + ", ");
                }

                if (!cardNum.Equals(""))
                {
                    sbUpdate.Append("card_num = '" + cardNum + "', ");
                }               

                if (!string1.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_1 = N'" + string1 + "', ");
                }

                if (!string2.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_2 = N'" + string2 + "', ");
                }

                if (!string3.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_3 = N'" + string3 + "', ");
                }

                if (!string4.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_4 = N'" + string4 + "', ");
                }

                if (!string5.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_5 = N'" + string5 + "', ");
                }
                
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE rec_id = " + recID);

                
                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

               sqlTrans.Commit();
           }
           catch (Exception ex)
           {
               sqlTrans.Rollback();

               throw new Exception(ex.Message);
           }

           return isUpdated;

        }

        public bool delete(int recID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM zin_apb WHERE rec_id = " + recID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
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
        public List<string> getDistinct(string columnName)
        {
            List<string> names = new List<string>();
            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT "+columnName+" from zin_apb");
                select = sb.ToString().Trim();
                select += " ORDER BY " + columnName;
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "DistinctColumn");
                DataTable table = dataSet.Tables["DistinctColumn"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        string str = "";
                        if (row[columnName] != DBNull.Value)
                        {
                            str += row[columnName].ToString().Trim();
                        }
                      
                        names.Add(str);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return names;
        }
        public List<TransferObjects.ZINApbTO> getApbsAsco(int recID, int employeeID, string cardNum, string string1, string string2, string string3, string string4, string string5, string createdBy, DateTime fromTime, DateTime toTime,string wUnits)
        {
            DataSet dataSet = new DataSet();
            ZINApbTO apb = new ZINApbTO();
            List<ZINApbTO> apbsList = new List<ZINApbTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT za.*, wu.name wu_name FROM zin_apb za, employees e, working_units wu");
                sb.Append(" WHERE za.employee_id = e.employee_id AND e.working_unit_id = wu.working_unit_id");
                if ((recID != -1)
                    || (employeeID != -1) || (!cardNum.Equals("")) || (!string1.Equals("")) || (!string2.Equals(""))
                    || (!string3.Equals("")) || (!string4.Equals("")) || (!string5.Equals("")) || (!wUnits.Equals("")) ||
                    (!createdBy.Equals("")) || ((!fromTime.Equals(new DateTime())) && (!toTime.Equals(new DateTime()))))
                {

                    sb.Append(" AND");

                    if (recID != -1)
                    {
                        sb.Append(" za.rec_id = " + recID + " AND");
                    }
                    if (employeeID != -1)
                    {
                        sb.Append(" za.employee_id = " + employeeID + " AND");
                    }
                    if (!cardNum.Equals(""))
                    {
                        sb.Append(" za.card_num = '" + cardNum + "' AND");
                    }
                    if (!wUnits.Equals(""))
                    {
                        sb.Append(" e.working_unit_id IN ( " + wUnits + " ) AND");
                    }
                    if (!createdBy.Equals(""))
                    {
                        sb.Append(" za.created_by = '" + createdBy + "' AND");
                    }
                    if (!string1.Equals(""))
                    {
                        sb.Append(" za.nvarchar_value_1 = N'" + string1 + "' AND");
                    }
                    if (!string2.Equals(""))
                    {
                        sb.Append(" za.nvarchar_value_2 = N'" + string2 + "' AND");
                    }
                    if (!string3.Equals(""))
                    {
                        sb.Append(" za.nvarchar_value_3 = N'" + string3 + "' AND");
                    }
                    if (!string4.Equals(""))
                    {
                        sb.Append(" za.nvarchar_value_4 = N'" + string4 + "' AND");
                    }
                    if (!string5.Equals(""))
                    {
                        sb.Append(" za.nvarchar_value_5 = N'" + string5 + "' AND");
                    }
                    if ((!fromTime.Equals(new DateTime())) && (!toTime.Equals(new DateTime())))
                    {
                        sb.Append(" (");

                        sb.Append(" convert(datetime, convert(varchar(10), za.created_time, 111), 111) >= convert(datetime,'" + fromTime.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(" AND convert(datetime, convert(varchar(10), za.created_time, 111), 111) <= convert(datetime,'" + toTime.ToString("yyyy-MM-dd") + "', 111)");
                        sb.Append(") AND");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select += " ORDER BY za.rec_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);


                sqlDataAdapter.Fill(dataSet, "APB");
                DataTable table = dataSet.Tables["APB"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        apb = new ZINApbTO();
                        apb.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
                        if (row["employee_id"] != DBNull.Value)
                        {
                            apb.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["card_num"] != DBNull.Value)
                        {
                            apb.CardNum = row["card_num"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            apb.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            apb.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            apb.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            apb.NVarcharValue2 = row["wu_name"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value)
                        {
                            apb.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value)
                        {
                            apb.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value)
                        {
                            apb.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }

                        apbsList.Add(apb);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return apbsList;
        }

        #endregion
    }
}
