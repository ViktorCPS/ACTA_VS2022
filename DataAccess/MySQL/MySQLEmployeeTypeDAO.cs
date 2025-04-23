using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace DataAccess
{
     public class MySQLEmployeeTypeDAO:EmployeeTypeDAO
    {
         MySqlConnection conn = null;
         MySqlTransaction _sqlTrans = null;
         protected string dateTimeformat = "";

         public MySqlTransaction SqlTrans
         {
             get { return _sqlTrans; }
             set { _sqlTrans = value; }
         }
         public MySQLEmployeeTypeDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

         public MySQLEmployeeTypeDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
         public bool delete(int EmplTypeID)
         {
             bool isDeleted = false;
             MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

             try
             {
                 StringBuilder sbDelete = new StringBuilder();
                 sbDelete.Append("DELETE FROM employee_types WHERE employee_type_id = '" + EmplTypeID.ToString().Trim() + "'");

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
         public int insert(EmployeeTypeTO ruleTO)
         {
             MySqlTransaction sqlTrans =  conn.BeginTransaction(IsolationLevel.RepeatableRead);            

             int rowsAffected = 0;

             try
             {
                 StringBuilder sbInsert = new StringBuilder();
                 sbInsert.Append("INSERT INTO employee_types ");
                 sbInsert.Append("(employee_type_id, working_unit_id, employee_type_name, created_by, created_time) ");
                 sbInsert.Append("VALUES (");

                 if (ruleTO.EmployeeTypeID != -1)
                 {
                     sbInsert.Append(ruleTO.EmployeeTypeID.ToString() + ", ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }

                 sbInsert.Append("'" + ruleTO.WorkingUnitID.ToString() +  "', ");

                 if (ruleTO.EmployeeTypeName != "")
                 {
                     sbInsert.Append("N'" + ruleTO.EmployeeTypeName.ToString() + "', ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }
               
                 sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

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

         public bool update(EmployeeTypeTO ruleTO)
         {
             bool isUpdated = false;
             MySqlTransaction sqlTrans = null;
           
             sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);            

             try
             {
                 StringBuilder sbUpdate = new StringBuilder();
                 sbUpdate.Append("UPDATE employee_types SET ");

                 
                 if (ruleTO.WorkingUnitID != -1)
                 {
                     sbUpdate.Append("working_unit_id = " + ruleTO.WorkingUnitID.ToString().Trim() + ", ");
                 }
                 else
                 {
                     sbUpdate.Append("working_unit_id = NULL, ");
                 }
                 if (ruleTO.EmployeeTypeName !="")
                 {
                     sbUpdate.Append("employee_type_name = N'" + ruleTO.EmployeeTypeName.ToString().Trim() + "', ");
                 }
                 else
                 {
                     sbUpdate.Append("employee_type_name = NULL, ");
                 }                
              
                 sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                 sbUpdate.Append("modified_time = NOW() ");
                 sbUpdate.Append("WHERE employee_type_id = '" + ruleTO.EmployeeTypeID.ToString().Trim() + "'");

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

         public List<EmployeeTypeTO> search(EmployeeTypeTO ruleTO)
         {
             DataSet dataSet = new DataSet();
             EmployeeTypeTO rule = new EmployeeTypeTO();
             List<EmployeeTypeTO> pairsList = new List<EmployeeTypeTO>();
             string select = "";

             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append("SELECT * FROM employee_types ");

                 if ((ruleTO.WorkingUnitID != -1) || (ruleTO.EmployeeTypeID != -1) ||
                      (ruleTO.EmployeeTypeName != ""))
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
                     if (ruleTO.EmployeeTypeName != "")
                     {
                         sb.Append(" employee_type_name = '" + ruleTO.EmployeeTypeName.ToString().Trim() + "' AND");
                     }
                     
                    
                     select = sb.ToString(0, sb.ToString().Length - 3);
                 }
                 else
                 {
                     select = sb.ToString();
                 }

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "EmployeeTypes");
                 DataTable table = dataSet.Tables["EmployeeTypes"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         rule = new EmployeeTypeTO();
                         rule.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                         rule.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                                               
                         if (row["employee_type_name"] != DBNull.Value)
                         {
                             rule.EmployeeTypeName = row["employee_type_name"].ToString().Trim();
                         }
                     

                         pairsList.Add(rule);
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return pairsList;
         }

         public Dictionary<int, Dictionary<int, string>> searchDictionary(EmployeeTypeTO typeTO)
         {
             DataSet dataSet = new DataSet();
             EmployeeTypeTO type = new EmployeeTypeTO();
             Dictionary<int, Dictionary<int, string>> types = new Dictionary<int, Dictionary<int, string>>();
             string select = "";

             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append("SELECT * FROM employee_types ");

                 if ((typeTO.WorkingUnitID != -1) || (typeTO.EmployeeTypeID != -1) ||
                      (typeTO.EmployeeTypeName != ""))
                 {
                     sb.Append(" WHERE");

                     if (typeTO.WorkingUnitID != -1)
                     {
                         sb.Append(" working_unit_id = '" + typeTO.WorkingUnitID.ToString().Trim() + "' AND");
                     }
                     if (typeTO.EmployeeTypeID != -1)
                     {
                         sb.Append(" employee_type_id = '" + typeTO.EmployeeTypeID.ToString().Trim() + "' AND");
                     }
                     if (typeTO.EmployeeTypeName != "")
                     {
                         sb.Append(" employee_type_name = '" + typeTO.EmployeeTypeName.ToString().Trim() + "' AND");
                     }

                     select = sb.ToString(0, sb.ToString().Length - 3);
                 }
                 else
                 {
                     select = sb.ToString();
                 }

                 select += " ORDER BY working_unit_id, employee_type_id, employee_type_name";

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "EmployeeTypes");
                 DataTable table = dataSet.Tables["EmployeeTypes"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         type = new EmployeeTypeTO();
                         type.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                         type.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                         if (row["employee_type_name"] != DBNull.Value)
                         {
                             type.EmployeeTypeName = row["employee_type_name"].ToString().Trim();
                         }

                         if (!types.ContainsKey(type.WorkingUnitID))
                             types.Add(type.WorkingUnitID, new Dictionary<int, string>());
                         if (!types[type.WorkingUnitID].ContainsKey(type.EmployeeTypeID))
                             types[type.WorkingUnitID].Add(type.EmployeeTypeID, type.EmployeeTypeName);
                         else
                             types[type.WorkingUnitID][type.EmployeeTypeID] = type.EmployeeTypeName;
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }

             return types;
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
    }
}
