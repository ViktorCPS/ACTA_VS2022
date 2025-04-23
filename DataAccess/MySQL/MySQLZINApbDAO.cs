using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public class MySQLZINApbDAO:ZINApbDAO
    {
          MySqlConnection conn = null;
		//protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MySQLZINApbDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}
        public MySQLZINApbDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
        }
        #region ZINApbDAO Members

        public bool insert(int recID, int employeeID, string cardNum, string string1, string string2, string string3, string string4, string string5)
        {
            bool isInserted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO zin_apb ");
                sbInsert.Append("(rec_id,employee_id, card_num, nvarchar_value_1,nvarchar_value_2,nvarchar_value_3,nvarchar_value_4,nvarchar_value_5,created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(recID + ", ");
                if (recID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(recID + ", ");
                }
                if (employeeID == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(employeeID + ", ");
                }
                if (!cardNum.Equals(""))
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
               
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()); ");


                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
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
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            

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
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE rec_id = " + recID);

                
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM zin_apb WHERE rec_id = " + recID);

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

        public List<string> getDistinct(string columnName)
        {
            List<string> names = new List<string>();
            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT " + columnName + " from zin_apb");
                select = sb.ToString().Trim();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

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
                sb.Append("SELECT za.* FROM zin_apb za, employee e");
                sb.Append(" WHERE za.employee_id = e.employee_id");
                if ((recID != -1)
                    || (employeeID != -1) || (!cardNum.Equals("")) || (!string1.Equals("")) || (!string2.Equals(""))
                    || (!string3.Equals("")) || (!string4.Equals("")) || (!string5.Equals("")) || (!wUnits.Equals("")) ||
                    (!createdBy.Equals("")) || ((!fromTime.Equals(new DateTime())) && (!toTime.Equals(new DateTime()))))
                {


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
                        sb.Append(" e.working_unit_id IN ( " + cardNum + " ) AND");
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
                        sb.Append(" AND (");
                        
                        sb.Append(" convert(date_format(za.created_time, '%Y-%m-%d'), datetime) >= convert('" + fromTime.ToString("yyyy-MM-dd") + "', datetime)");
                        sb.Append(" AND convert(date_format(za.created_time, '%Y-%m-%d'), datetime) <= convert('" + toTime.ToString("yyyy-MM-dd") + "', datetime)");
                       
                        sb.Append(")");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select += " ORDER BY za.rec_id ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);


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
                            apb.EmployeeID = Int32.Parse(row["integer_value_1"].ToString().Trim());
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
                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            apb.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
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
