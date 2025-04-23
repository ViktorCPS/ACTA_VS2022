using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using TransferObjects;
using System.Globalization;

namespace DataAccess
{
   public  class MySQLRuleDAO:RuleDAO
    {
        MySqlConnection conn = null;
         MySqlTransaction _sqlTrans = null;
         protected string dateTimeformat = "";

         public MySqlTransaction SqlTrans
         {
             get { return _sqlTrans; }
             set { _sqlTrans = value; }
         }
         public MySQLRuleDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

         public MySQLRuleDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
         public bool delete(int RuleID)
         {
             bool isDeleted = false;
             MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

             try
             {
                 StringBuilder sbDelete = new StringBuilder();
                 sbDelete.Append("DELETE FROM rules WHERE rule_id = '" + RuleID.ToString().Trim() + "'");

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
         public int insert(RuleTO ruleTO, bool doCommit)
         {
             MySqlTransaction sqlTrans = null;

             if (doCommit)
             {
                 sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
             }
             else
             {
                 sqlTrans = this.SqlTrans;
             }

             int rowsAffected = 0;

             try
             {
                 StringBuilder sbInsert = new StringBuilder();
                 sbInsert.Append("INSERT INTO rules ");
                 sbInsert.Append("(rule_id, working_unit_id, rule_type, employee_type_id, rule_description, rule_value, rule_datetime_1, rule_datetime_2, created_by, created_time) ");
                 sbInsert.Append("VALUES (");

                 if (ruleTO.RuleID != -1)
                 {
                     sbInsert.Append(ruleTO.RuleID.ToString() + ", ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }

                 sbInsert.Append("'" + ruleTO.WorkingUnitID.ToString() +  "', ");

                 if (ruleTO.RuleType != "")
                 {
                     sbInsert.Append("N'" + ruleTO.RuleType.ToString() + "', ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }
                 if (ruleTO.EmployeeTypeID != -1)
                 {
                     sbInsert.Append(ruleTO.EmployeeTypeID.ToString() + ", ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }

                 if (ruleTO.RuleDescription != "")
                 {
                     sbInsert.Append("N'" + ruleTO.RuleDescription.ToString() + "', ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }

                 if (ruleTO.RuleValue != -1)
                 {
                     sbInsert.Append(ruleTO.RuleValue.ToString() + ", ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }
                 if (ruleTO.RuleDateTime1 != new DateTime())
                 {
                     sbInsert.Append(ruleTO.RuleDateTime1.ToString(dateTimeformat) + ", ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }
                 if (ruleTO.RuleDateTime2 != new DateTime())
                 {
                     sbInsert.Append(ruleTO.RuleDateTime2.ToString(dateTimeformat) + ", ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }
                
                 sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                 MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                 rowsAffected = cmd.ExecuteNonQuery();

                 if (doCommit)
                 {
                     sqlTrans.Commit();
                 }
             }
             catch (Exception ex)
             {
                 if(doCommit)
                 sqlTrans.Rollback();
                 throw ex;
             }

             return rowsAffected;
         }

         public bool update(RuleTO  ruleTO, bool doCommit)
         {
             bool isUpdated = false;
             MySqlTransaction sqlTrans = null;

             if (doCommit)
             {
                 sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
             }
             else
             {
                 sqlTrans = this.SqlTrans;
             }

             try
             {
                 StringBuilder sbUpdate = new StringBuilder();
                 sbUpdate.Append("UPDATE rules SET ");

                 
                 if (ruleTO.WorkingUnitID != -1)
                 {
                     sbUpdate.Append("working_unit_id = " + ruleTO.WorkingUnitID.ToString().Trim() + ", ");
                 }
                 else
                 {
                     sbUpdate.Append("working_unit_id = NULL, ");
                 }
                 if (ruleTO.EmployeeTypeID !=-1)
                 {
                     sbUpdate.Append("employee_type_id = " + ruleTO.EmployeeTypeID.ToString().Trim() + ", ");
                 }
                 else
                 {
                     sbUpdate.Append("employee_type_id = NULL, ");
                 }
                 if (ruleTO.RuleType != "")
                 {
                     sbUpdate.Append("rule_type = N'" + ruleTO.RuleType.ToString().Trim() + "', ");
                 }
                 else
                 {
                     sbUpdate.Append("rule_type = NULL, ");
                 }
                 if (ruleTO.RuleDescription != "")
                 {
                     sbUpdate.Append("rule_description = N'" + ruleTO.RuleDescription.ToString().Trim() + "', ");
                 }
                 else
                 {
                     sbUpdate.Append("rule_description = NULL, ");
                 }                 
                 if (ruleTO.RuleValue != -1)
                 {
                     sbUpdate.Append("rule_value = " + ruleTO.RuleValue.ToString().Trim() + ", ");
                 }
                 else
                 {
                     sbUpdate.Append("rule_value = NULL, ");
                 }                

                 if (!ruleTO.RuleDateTime1.Equals(new DateTime()))
                 {
                     sbUpdate.Append("rule_datetime_1 = '" + ruleTO.RuleDateTime1.ToString(dateTimeformat) + "', ");
                 }
                 else
                 {
                     sbUpdate.Append("rule_datetime_1 = NULL, ");
                 }
                 if (!ruleTO.RuleDateTime2.Equals(new DateTime()))
                 {
                     sbUpdate.Append("rule_datetime_2 = '" + ruleTO.RuleDateTime2.ToString(dateTimeformat) + "', ");
                 }
                 else
                 {
                     sbUpdate.Append("rule_datetime_2 = NULL, ");
                 }
              
                 sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                 sbUpdate.Append("modified_time = NOW() ");
                 sbUpdate.Append("WHERE rule_id = '" + ruleTO.RuleID.ToString().Trim() + "'");

                 MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                 int res = cmd.ExecuteNonQuery();
                 if (res > 0)
                 {
                     isUpdated = true;
                 }

                 if(doCommit)
                 sqlTrans.Commit();
             }            
             catch (Exception ex)
             {
                 if (doCommit)
                 sqlTrans.Rollback();
                 throw new Exception(ex.Message);
             }

             return isUpdated;
         }

         public bool update(int company, string type, int value, bool doCommit)
         {
             bool isUpdated = false;
             MySqlTransaction sqlTrans = null;

             if (doCommit)
             {
                 sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
             }
             else
             {
                 sqlTrans = this.SqlTrans;
             }

             try
             {
                 StringBuilder sbUpdate = new StringBuilder();
                 sbUpdate.Append("UPDATE rules SET rule_value = '" + value.ToString().Trim() + "', ");
                 sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                 sbUpdate.Append("modified_time = NOW() ");
                 sbUpdate.Append("WHERE working_unit_id = '" + company.ToString().Trim() + "' AND rule_type = N'" + type.Trim() + "'");

                 MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                 int res = cmd.ExecuteNonQuery();
                 if (res > 0)
                 {
                     isUpdated = true;
                 }

                 if (doCommit)
                     sqlTrans.Commit();
             }
             catch (Exception ex)
             {
                 if (doCommit)
                     sqlTrans.Rollback();
                 throw new Exception(ex.Message);
             }

             return isUpdated;
         }

         public Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> SearchWUEmplTypeDictionary()
         {
             DataSet dataSet = new DataSet();
             RuleTO rule = new RuleTO();
             Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> ruleDict = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
             string select = "";

             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append("SELECT * FROM io_pairs_processed ");

                 select = sb.ToString();

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "Rules");
                 DataTable table = dataSet.Tables["Rules"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         rule = new RuleTO();
                         rule.RuleID = Int32.Parse(row["rule_id"].ToString().Trim());
                         rule.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                         if (row["employee_type_id"] != DBNull.Value)
                         {
                             rule.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                         }
                         if (row["rule_value"] != DBNull.Value)
                         {
                             rule.RuleValue = Int32.Parse(row["rule_value"].ToString().Trim());
                         }
                         if (row["rule_type"] != DBNull.Value)
                         {
                             rule.RuleType = row["rule_type"].ToString().Trim();
                         }
                         if (row["rule_description"] != DBNull.Value)
                         {
                             rule.RuleDescription = row["rule_description"].ToString().Trim();
                         }
                         if (row["rule_datetime_1"] != DBNull.Value)
                         {
                             rule.RuleDateTime1 = (DateTime)row["rule_datetime_1"];
                         }
                         if (row["rule_datetime_2"] != DBNull.Value)
                         {
                             rule.RuleDateTime2 = (DateTime)row["rule_datetime_2"];
                         }
                         if (!ruleDict.ContainsKey(rule.WorkingUnitID))
                             ruleDict.Add(rule.WorkingUnitID, new Dictionary<int, Dictionary<string, RuleTO>>());
                         if (!ruleDict[rule.WorkingUnitID].ContainsKey(rule.EmployeeTypeID))
                             ruleDict[rule.WorkingUnitID].Add(rule.EmployeeTypeID, new Dictionary<string, RuleTO>());
                         if (!ruleDict[rule.WorkingUnitID][rule.EmployeeTypeID].ContainsKey(rule.RuleType))
                             ruleDict[rule.WorkingUnitID][rule.EmployeeTypeID].Add(rule.RuleType, rule);

                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return ruleDict;
         }
         public Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> SearchWUEmplTypeDictionary(IDbTransaction trans)
         {
             DataSet dataSet = new DataSet();
             RuleTO rule = new RuleTO();
             Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> ruleDict = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
             string select = "";

             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append("SELECT * FROM io_pairs_processed ");

                 select = sb.ToString();

                 MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "Rules");
                 DataTable table = dataSet.Tables["Rules"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         rule = new RuleTO();
                         rule.RuleID = Int32.Parse(row["rule_id"].ToString().Trim());
                         rule.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                         if (row["employee_type_id"] != DBNull.Value)
                         {
                             rule.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                         }
                         if (row["rule_value"] != DBNull.Value)
                         {
                             rule.RuleValue = Int32.Parse(row["rule_value"].ToString().Trim());
                         }
                         if (row["rule_type"] != DBNull.Value)
                         {
                             rule.RuleType = row["rule_type"].ToString().Trim();
                         }
                         if (row["rule_description"] != DBNull.Value)
                         {
                             rule.RuleDescription = row["rule_description"].ToString().Trim();
                         }
                         if (row["rule_datetime_1"] != DBNull.Value)
                         {
                             rule.RuleDateTime1 = (DateTime)row["rule_datetime_1"];
                         }
                         if (row["rule_datetime_2"] != DBNull.Value)
                         {
                             rule.RuleDateTime2 = (DateTime)row["rule_datetime_2"];
                         }
                         if (!ruleDict.ContainsKey(rule.WorkingUnitID))
                             ruleDict.Add(rule.WorkingUnitID, new Dictionary<int, Dictionary<string, RuleTO>>());
                         if (!ruleDict[rule.WorkingUnitID].ContainsKey(rule.EmployeeTypeID))
                             ruleDict[rule.WorkingUnitID].Add(rule.EmployeeTypeID, new Dictionary<string, RuleTO>());
                         if (!ruleDict[rule.WorkingUnitID][rule.EmployeeTypeID].ContainsKey(rule.RuleType))
                             ruleDict[rule.WorkingUnitID][rule.EmployeeTypeID].Add(rule.RuleType, rule);

                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return ruleDict;
         }

         public Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> getTypeAllRules(string type)
         {
             DataSet dataSet = new DataSet();
             RuleTO rule = new RuleTO();
             Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> ruleDict = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
             string select = "";

             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append("SELECT * FROM rules WHERE UPPER(rule_type) = '" + type.Trim().ToUpper() + "'");

                 select = sb.ToString();

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "Rules");
                 DataTable table = dataSet.Tables["Rules"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         rule = new RuleTO();
                         rule.RuleID = Int32.Parse(row["rule_id"].ToString().Trim());
                         rule.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                         if (row["employee_type_id"] != DBNull.Value)
                         {
                             rule.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                         }
                         if (row["rule_value"] != DBNull.Value)
                         {
                             rule.RuleValue = Int32.Parse(row["rule_value"].ToString().Trim());
                         }
                         if (row["rule_type"] != DBNull.Value)
                         {
                             rule.RuleType = row["rule_type"].ToString().Trim();
                         }
                         if (row["rule_description"] != DBNull.Value)
                         {
                             rule.RuleDescription = row["rule_description"].ToString().Trim();
                         }
                         if (row["rule_datetime_1"] != DBNull.Value)
                         {
                             rule.RuleDateTime1 = (DateTime)row["rule_datetime_1"];
                         }
                         if (row["rule_datetime_2"] != DBNull.Value)
                         {
                             rule.RuleDateTime2 = (DateTime)row["rule_datetime_2"];
                         }
                         if (!ruleDict.ContainsKey(rule.WorkingUnitID))
                             ruleDict.Add(rule.WorkingUnitID, new Dictionary<int, Dictionary<string, RuleTO>>());
                         if (!ruleDict[rule.WorkingUnitID].ContainsKey(rule.EmployeeTypeID))
                             ruleDict[rule.WorkingUnitID].Add(rule.EmployeeTypeID, new Dictionary<string, RuleTO>());
                         if (!ruleDict[rule.WorkingUnitID][rule.EmployeeTypeID].ContainsKey(rule.RuleType))
                             ruleDict[rule.WorkingUnitID][rule.EmployeeTypeID].Add(rule.RuleType, rule);

                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return ruleDict;
         }

         public List<RuleTO> search(RuleTO ruleTO)
         {
             DataSet dataSet = new DataSet();
             RuleTO rule = new RuleTO();
             List<RuleTO> ruleList = new List<RuleTO>();
             string select = "";

             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append("SELECT * FROM rules ");

                 if ((ruleTO.WorkingUnitID != -1) || (ruleTO.EmployeeTypeID != -1) ||
                     (ruleTO.RuleValue != -1) || (ruleTO.RuleType != "") || (ruleTO.RuleDescription != "") ||
                     (!ruleTO.RuleDateTime1.Equals(new DateTime())) || (!ruleTO.RuleDateTime2.Equals(new DateTime())))
                 {
                     sb.Append(" WHERE");

                     if (ruleTO.WorkingUnitID != -1)
                     {
                         sb.Append(" working_unit_id = '" + ruleTO.WorkingUnitID.ToString().Trim() + "' AND");
                     }
                     if (ruleTO.EmployeeTypeID != -1)
                     {
                         sb.Append(" employee_type_id = '" + ruleTO.EmployeeTypeID.ToString().Trim() + "' AND");
                     }
                     if (ruleTO.RuleValue != -1)
                     {
                         sb.Append(" rule_value = '" + ruleTO.RuleValue.ToString().Trim() + "' AND");
                     }
                     if (!ruleTO.RuleType.Equals(""))
                     {
                         sb.Append(" UPPER(rule_type) = '" + ruleTO.RuleType.Trim().ToUpper() + "' AND");
                     }
                     if (!ruleTO.RuleDescription.Equals(""))
                     {
                         sb.Append(" UPPER(rule_description) = '" + ruleTO.RuleDescription.Trim().ToUpper() + "' AND");
                     }
                     if (!ruleTO.RuleDateTime1.Equals(new DateTime()))
                     {
                         sb.Append(" DATE_FORMAT(rule_datetime_1,GET_FORMAT(DATETIME,'ISO')) = '" + ruleTO.RuleDateTime1.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                     }
                     if (!ruleTO.RuleDateTime2.Equals(new DateTime()))
                     {
                         sb.Append(" DATE_FORMAT(rule_datetime_2,GET_FORMAT(DATETIME,'ISO')) = '" + ruleTO.RuleDateTime2.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                     }

                     select = sb.ToString(0, sb.ToString().Length - 3);
                 }
                 else
                 {
                     select = sb.ToString();
                 }

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "Rules");
                 DataTable table = dataSet.Tables["Rules"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         rule = new RuleTO();
                         rule.RuleID = Int32.Parse(row["rule_id"].ToString().Trim());
                         rule.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                         if (row["employee_type_id"] != DBNull.Value)
                         {
                             rule.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                         }
                         if (row["rule_value"] != DBNull.Value)
                         {
                             rule.RuleValue = Int32.Parse(row["rule_value"].ToString().Trim());
                         }
                         if (row["rule_type"] != DBNull.Value)
                         {
                             rule.RuleType = row["rule_type"].ToString().Trim();
                         }
                         if (row["rule_description"] != DBNull.Value)
                         {
                             rule.RuleDescription = row["rule_description"].ToString().Trim();
                         }
                         if (row["rule_datetime_1"] != DBNull.Value)
                         {
                             rule.RuleDateTime1 = (DateTime)row["rule_datetime_1"];
                         }
                         if (row["rule_datetime_2"] != DBNull.Value)
                         {
                             rule.RuleDateTime2 = (DateTime)row["rule_datetime_2"];
                         }

                         ruleList.Add(rule);
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return ruleList;
         }

         public List<string> getRuleTypes()
         {
             DataSet dataSet = new DataSet();
             List<string> ruleList = new List<string>();
             string select = "";

             try
             {
                 select = "SELECT DISTINCT rule_type FROM rules ORDER BY rule_type";

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "Rules");
                 DataTable table = dataSet.Tables["Rules"];

                 foreach (DataRow row in table.Rows)
                 {
                     if (row["rule_type"] != DBNull.Value)
                         ruleList.Add(row["rule_type"].ToString());
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return ruleList;
         }

         public List<int> getRules(string type)
         {
             DataSet dataSet = new DataSet();
             RuleTO rule = new RuleTO();
             List<int> ruleList = new List<int>();
             string select = "";

             try
             {
                 if (type.Trim().Equals(""))
                     return ruleList;

                 select = "SELECT DISTINCT rule_value FROM rules WHERE rule_type LIKE '" + type.Trim() + "%'";

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "Rules");
                 DataTable table = dataSet.Tables["Rules"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         int value = -1;
                         if (row["rule_value"] != DBNull.Value)
                         {
                             value = Int32.Parse(row["rule_value"].ToString().Trim());
                         }

                         if (value != -1 && !ruleList.Contains(value))
                             ruleList.Add(value);
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return ruleList;
         }

         public List<int> getRulesExact(string type)
         {
             DataSet dataSet = new DataSet();
             RuleTO rule = new RuleTO();
             List<int> ruleList = new List<int>();
             string select = "";

             try
             {
                 if (type.Trim().Equals(""))
                     return ruleList;

                 select = "SELECT DISTINCT rule_value FROM rules WHERE rule_type = '" + type.Trim() + "'";

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "Rules");
                 DataTable table = dataSet.Tables["Rules"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         int value = -1;
                         if (row["rule_value"] != DBNull.Value)
                         {
                             value = Int32.Parse(row["rule_value"].ToString().Trim());
                         }

                         if (value != -1 && !ruleList.Contains(value))
                             ruleList.Add(value);
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return ruleList;
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

        public int searchReaderForRestaurant(string ruleType) 
        {
            throw new NotImplementedException();
        }
    }
}
