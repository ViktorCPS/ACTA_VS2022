using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;
using TransferObjects;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    public class MySQLEmployeeLoanDAO:EmployeeLoanDAO
    {
        MySqlConnection conn = null;
         MySqlTransaction _sqlTrans = null;
         protected string dateTimeformat = "";

         public MySqlTransaction SqlTrans
         {
             get { return _sqlTrans; }
             set { _sqlTrans = value; }
         }
         public MySQLEmployeeLoanDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

         public MySQLEmployeeLoanDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
         public bool delete(int recID)
         {
             bool isDeleted = false;
             MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

             try
             {
                 StringBuilder sbDelete = new StringBuilder();
                 sbDelete.Append("DELETE FROM employee_loans WHERE rec_id = '" + recID.ToString().Trim() + "'");

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

         public int insert(EmployeeLoanTO emplLoanTO, bool doCommit)
         {
             if (doCommit)
                 SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

             int rowsAffected = 0;

             try
             {
                 StringBuilder sbInsert = new StringBuilder();
                 sbInsert.Append("INSERT INTO employee_loans ");
                 sbInsert.Append("(employee_id, working_unit_id, date_start,date_end, created_by, created_time) ");
                 sbInsert.Append("VALUES (");

                 if (emplLoanTO.EmployeeID != -1)
                 {
                     sbInsert.Append(emplLoanTO.EmployeeID.ToString() + ", ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }

                 sbInsert.Append("'" + emplLoanTO.WorkingUnitID.ToString() +  "', ");

                 if (emplLoanTO.DateStart != new DateTime())
                 {
                     sbInsert.Append("N'" + emplLoanTO.DateStart.ToString(dateTimeformat) + "', ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }
                     if (emplLoanTO.DateEnd != new DateTime())
                 {
                     sbInsert.Append("N'" + emplLoanTO.DateEnd.ToString(dateTimeformat) + "', ");
                 }
                 else
                 {
                     sbInsert.Append("NULL, ");
                 }

                     if (!emplLoanTO.CreatedBy.Trim().Equals(""))
                     {
                         sbInsert.Append("N'" + emplLoanTO.CreatedBy.Trim() + "', ");
                     }
                     else
                     {
                         sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                     }

                     sbInsert.Append("NOW()) ");

                 MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, SqlTrans);

                 rowsAffected = cmd.ExecuteNonQuery();

                 if (doCommit)
                     SqlTrans.Commit();                 
             }
             catch (Exception ex)
             {
                 if (doCommit)
                     SqlTrans.Rollback();
                 throw ex;
             }

             return rowsAffected;
         }

         public bool update(EmployeeLoanTO emplLoanTO)
         {
             bool isUpdated = false;
             MySqlTransaction sqlTrans = null;
           
             sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);            

             try
             {
                 StringBuilder sbUpdate = new StringBuilder();
                 sbUpdate.Append("UPDATE employee_loans SET ");

                 
                 if (emplLoanTO.WorkingUnitID != -1)
                 {
                     sbUpdate.Append("working_unit_id = " + emplLoanTO.WorkingUnitID.ToString().Trim() + ", ");
                 }
                 else
                 {
                     sbUpdate.Append("working_unit_id = NULL, ");
                 }
                 if (emplLoanTO.EmployeeID !=-1)
                 {
                     sbUpdate.Append("employee_id = " + emplLoanTO.EmployeeID.ToString().Trim() + ", ");
                 }
                 else
                 {
                     sbUpdate.Append("employee_id = NULL, ");
                 }                
                 if (emplLoanTO.DateStart != new DateTime())
                 {
                     sbUpdate.Append("date_start = " + emplLoanTO.DateStart.ToString(dateTimeformat).Trim() + ", ");
                 }
                 else
                 {
                     sbUpdate.Append("employee_id = NULL, ");
                 }
                 if (!emplLoanTO.ModifiedBy.Trim().Equals(""))
                 {
                     sbUpdate.Append("modified_by = N'" + emplLoanTO.ModifiedBy.Trim() + "', ");
                 }
                 else
                 {
                     sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                 }
                 sbUpdate.Append("modified_time = NOW() ");
                 sbUpdate.Append("WHERE rec_id = '" + emplLoanTO.RecID.ToString().Trim() + "'");

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

         public List<EmployeeLoanTO> search(EmployeeLoanTO ruleTO)
         {
             DataSet dataSet = new DataSet();
             EmployeeLoanTO loan = new EmployeeLoanTO();
             List<EmployeeLoanTO> loansList = new List<EmployeeLoanTO>();
             string select = "";

             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append("SELECT * FROM employee_loans ");

                 if ((ruleTO.WorkingUnitID != -1) || (ruleTO.EmployeeID != -1) ||
                      (ruleTO.DateStart != new DateTime())|| (ruleTO.DateEnd != new DateTime()))
                 {
                     sb.Append(" WHERE");

                     if (ruleTO.WorkingUnitID != -1)
                     {
                         sb.Append(" working_unit_id = '" + ruleTO.WorkingUnitID.ToString().Trim() + "' AND");
                     }
                     if (ruleTO.EmployeeID != -1)
                     {
                         sb.Append(" employee_id = '" + ruleTO.EmployeeID.ToString().Trim() + "' AND");
                     }
                     if (ruleTO.DateStart!= new DateTime())
                     {
                         sb.Append(" date_start = '" + ruleTO.DateStart.ToString(dateTimeformat).Trim() + "' AND");
                     }
                     if (ruleTO.DateEnd!= new DateTime())
                     {
                         sb.Append(" date_end = '" + ruleTO.DateEnd.ToString(dateTimeformat).Trim() + "' AND");
                     }
                    
                     select = sb.ToString(0, sb.ToString().Length - 3);
                 }
                 else
                 {
                     select = sb.ToString();
                 }

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "EmployeeLoans");
                 DataTable table = dataSet.Tables["EmployeeLoans"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         loan = new EmployeeLoanTO();
                         loan.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                         loan.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                                               
                         if (row["date_start"] != DBNull.Value)
                         {
                             loan.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                         }
                     if (row["date_end"] != DBNull.Value)
                         {
                             loan.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                         } 
                    if (row["created_by"] != DBNull.Value)
                     {
                         loan.CreatedBy = row["created_by"].ToString().Trim();
                     }

                         loansList.Add(loan);
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return loansList;
         }
         public List<EmployeeLoanTO> search(EmployeeLoanTO ruleTO, DateTime fromDate,DateTime toDate)
         {
             DataSet dataSet = new DataSet();
             EmployeeLoanTO loan = new EmployeeLoanTO();
             List<EmployeeLoanTO> loansList = new List<EmployeeLoanTO>();
             string select = "";

             try
             {
                 StringBuilder sb = new StringBuilder();
                 sb.Append("SELECT * FROM employee_loans ");

                 if ((ruleTO.WorkingUnitID != -1) || (ruleTO.EmployeeID != -1) ||
                      (ruleTO.DateStart != new DateTime()) || (ruleTO.DateEnd != new DateTime()))
                 {
                     sb.Append(" WHERE");

                     if (ruleTO.WorkingUnitID != -1)
                     {
                         sb.Append(" working_unit_id = '" + ruleTO.WorkingUnitID.ToString().Trim() + "' AND");
                     }
                     if (ruleTO.EmployeeID != -1)
                     {
                         sb.Append(" employee_id = '" + ruleTO.EmployeeID.ToString().Trim() + "' AND");
                     }
                     if (fromDate != new DateTime())
                     {
                         sb.Append(" date_start >= '" + fromDate.ToString(dateTimeformat).Trim() + "' AND");
                     }
                     if (toDate != new DateTime())
                     {
                         sb.Append(" date_end <= '" + toDate.ToString(dateTimeformat).Trim() + "' AND");
                     }

                     select = sb.ToString(0, sb.ToString().Length - 3);
                 }
                 else
                 {
                     select = sb.ToString();
                 }

                 MySqlCommand cmd = new MySqlCommand(select, conn);
                 MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                 sqlDataAdapter.Fill(dataSet, "EmployeeLoans");
                 DataTable table = dataSet.Tables["EmployeeLoans"];

                 if (table.Rows.Count > 0)
                 {
                     foreach (DataRow row in table.Rows)
                     {
                         loan = new EmployeeLoanTO();
                         loan.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                         loan.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                         if (row["date_start"] != DBNull.Value)
                         {
                             loan.DateStart = DateTime.Parse(row["date_start"].ToString().Trim());
                         }
                         if (row["date_end"] != DBNull.Value)
                         {
                             loan.DateEnd = DateTime.Parse(row["date_end"].ToString().Trim());
                         }
                         if (row["created_by"] != DBNull.Value)
                         {
                             loan.CreatedBy = row["created_by"].ToString().Trim();
                         }

                         loansList.Add(loan);
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw new Exception("Exception: " + ex.Message);
             }

             return loansList;
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

