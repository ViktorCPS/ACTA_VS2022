using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Globalization;

namespace DataAccess
{
   public  class MySQLEmployeeVacEvidDAO : EmployeeVacEvidDAO
    {
       
		MySqlConnection conn = null;
		//protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;
       protected string dateTimeformat = "";

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MySQLEmployeeVacEvidDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

       public MySQLEmployeeVacEvidDAO(MySqlConnection mySqlConnection)
       {
           conn = mySqlConnection;
           DAOController.GetInstance();
           DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
           dateTimeformat = dateTimeFormat.SortableDateTimePattern;
       }

       public int insert(EmployeeVacEvidTO employeeVacEvidTO)
       {
           int rowsAffected = 0;
           MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
           DataSet dataSet = new DataSet();

           try
           {
               StringBuilder sbInsert = new StringBuilder();

               // Insert into header table

               sbInsert.Append("INSERT INTO employee_vac_evid (employee_id, vac_year, num_of_days, note,valid_to, created_by, created_time) ");
               sbInsert.Append("VALUES (N'" + employeeVacEvidTO.EmployeeID + "', N'" + employeeVacEvidTO.VacYear.ToString(dateTimeformat) + "', N'" + employeeVacEvidTO.NumOfDays + "', N'" + employeeVacEvidTO.Note + "', N'" + employeeVacEvidTO.ValidTo.ToString(dateTimeformat) +
                   "', N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

               MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
               cmd.ExecuteNonQuery();
               if ((employeeVacEvidTO.VacationSchedules.Count > 0))
               {
                   sbInsert.Length = 0;
                   EmployeeVacEvidScheduleTO scheduleTO = new EmployeeVacEvidScheduleTO();

                   foreach (int key in employeeVacEvidTO.VacationSchedules.Keys)
                   {
                       scheduleTO = employeeVacEvidTO.VacationSchedules[key];
                       cmd.CommandText = prepareDetailInsert(scheduleTO);
                       rowsAffected += cmd.ExecuteNonQuery();
                   }

                   sqlTrans.Commit();
               }
               else
               {
                   sqlTrans.Rollback();
               }
           }
           catch (Exception ex)
           {
               sqlTrans.Rollback();
               throw ex;
           }
           return rowsAffected;
       }

       public int insert(EmployeeVacEvidTO employeeVacEvidTO,bool doCommit)
       {
           int rowsAffected = 0;
           MySqlTransaction sqlTrans = null;
           if (doCommit)
           {
               sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
           }
           else
           {
               sqlTrans = this.SqlTrans;
           }
           DataSet dataSet = new DataSet();

           try
           {
               StringBuilder sbInsert = new StringBuilder();

               // Insert into header table

               sbInsert.Append("INSERT INTO employee_vac_evid (employee_id, vac_year, num_of_days, note,valid_to, created_by, created_time,modified_by,modified_time) ");
               sbInsert.Append("VALUES (N'" + employeeVacEvidTO.EmployeeID + "', N'" + employeeVacEvidTO.VacYear.ToString(dateTimeformat) + "', N'" + employeeVacEvidTO.NumOfDays + "', N'" + employeeVacEvidTO.Note + "', N'" + employeeVacEvidTO.ValidTo.ToString(dateTimeformat) + "', N'" + employeeVacEvidTO.CreatedBy + "', N'" + employeeVacEvidTO.CreatedTime.ToString(dateTimeformat) +
                   "', N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

               MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
               cmd.ExecuteNonQuery();
               if ((employeeVacEvidTO.VacationSchedules.Count > 0))
               {
                   sbInsert.Length = 0;
                   EmployeeVacEvidScheduleTO scheduleTO = new EmployeeVacEvidScheduleTO();

                   foreach (int key in employeeVacEvidTO.VacationSchedules.Keys)
                   {
                       scheduleTO = employeeVacEvidTO.VacationSchedules[key];
                       cmd.CommandText = prepareDetailInsert(scheduleTO);
                       rowsAffected += cmd.ExecuteNonQuery();
                   }


                   if (doCommit)
                   {
                       sqlTrans.Commit();
                   }

               }
               else
               {

                   if (doCommit)
                   {
                       sqlTrans.Rollback();
                   }

               }
           }
           catch (MySqlException sqlEx)
           {
               if (doCommit)

                   sqlTrans.Rollback();
               else
                   sqlTrans.Rollback();

               if (sqlEx.Number == 1062)
               {
                   throw new Exception(sqlEx.Number.ToString());
               }
               else
               {
                   throw new Exception(sqlEx.Message);
               }
           }
           catch (Exception ex)
           {
               if (doCommit)
                   sqlTrans.Rollback();
               else
                   sqlTrans.Rollback();
               throw ex;
           }
           return rowsAffected;
       }

       public List<EmployeeVacEvidTO> search(string employeeID, DateTime yearFrom, DateTime yearTo,int daysApproveMin, int daysApproveMax)
       {
           DataSet dataSet = new DataSet();
           EmployeeVacEvidTO vacationTO = new EmployeeVacEvidTO();
           List<EmployeeVacEvidTO> objectsList = new List<EmployeeVacEvidTO>();
           string select = "";

           try
           {
               StringBuilder sb = new StringBuilder();
               sb.Append("SELECT eve.*,e.*,wu.*, eve.modified_by mod_by, eve.modified_time mod_time FROM employee_vac_evid eve,employees e,working_units wu  ");
               sb.Append("WHERE ");
               sb.Append("eve.employee_id = e.employee_id AND ");
               sb.Append("e.working_unit_id = wu.working_unit_id ");

               if ((employeeID != "") || (!yearFrom.Equals(new DateTime()) && !yearTo.Equals(new DateTime())) || (daysApproveMax != -1) || (daysApproveMax != -1))
               {
                   if (employeeID != "")
                   {
                       sb.Append(" AND eve.employee_id IN (" + employeeID + ")");
                   }
                   if (!yearFrom.Equals(new DateTime()) && !yearTo.Equals(new DateTime()))
                   {
                       sb.Append(" AND eve.vac_year >= '" + yearFrom.ToString(dateTimeformat) + "'");
                       sb.Append(" AND eve.vac_year < '" + yearTo.AddYears(1).ToString(dateTimeformat) + "'");
                   }
                   if (daysApproveMin != -1)
                   {
                       sb.Append(" AND eve.num_of_days >= " + daysApproveMin );
                   }
                   if (daysApproveMax != -1)
                   {
                       sb.Append(" AND eve.num_of_days <= " + daysApproveMax );
                   }
                   select = sb.ToString();
               }
               else
               {
                   select = sb.ToString();
               }

               select = select + " ORDER BY e.first_name,e.last_name, eve.vac_year";

               MySqlCommand cmd = new MySqlCommand(select, conn);
               MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

               sqlDataAdapter.Fill(dataSet, "Objects");
               DataTable table = dataSet.Tables["Objects"];

               if (table.Rows.Count > 0)
               {
                   foreach (DataRow row in table.Rows)
                   {
                       vacationTO = new EmployeeVacEvidTO();

                       vacationTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                       vacationTO.VacYear = DateTime.Parse(row["vac_year"].ToString().Trim());
                       vacationTO.NumOfDays = Int32.Parse(row["num_of_days"].ToString().Trim());
                       if (!row["note"].Equals(DBNull.Value))
                       {
                           vacationTO.Note= row["note"].ToString().Trim();
                       }
                       if (!row["created_by"].Equals(DBNull.Value))
                       {
                           vacationTO.CreatedBy = row["created_by"].ToString().Trim();
                       }
                       if (!row["created_time"].Equals(DBNull.Value))
                       {
                           vacationTO.CreatedTime=DateTime.Parse(row["created_time"].ToString().Trim());
                       }
                       if (!row["first_name"].Equals(DBNull.Value))
                       {
                           vacationTO.FirstName = row["first_name"].ToString().Trim();
                       }
                       if (!row["last_name"].Equals(DBNull.Value))
                       {
                           vacationTO.LastName =row["last_name"].ToString().Trim();
                       }
                       if (!row["working_unit_id"].Equals(DBNull.Value))
                       {
                           vacationTO.WorkingUnitID = int.Parse(row["working_unit_id"].ToString().Trim());
                       }
                       if (!row["name"].Equals(DBNull.Value))
                       {
                           vacationTO.WorkingUnit =row["name"].ToString().Trim();
                       }
                       if (!row["valid_to"].Equals(DBNull.Value))
                       {
                           vacationTO.ValidTo = DateTime.Parse(row["valid_to"].ToString().Trim());
                       }
                       if (!row["modified_by"].Equals(DBNull.Value))
                       {
                           vacationTO.ModifiedBy = row["modified_by"].ToString().Trim();
                       }
                       if (!row["modified_time"].Equals(DBNull.Value))
                       {
                           vacationTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                       }
                       objectsList.Add(vacationTO);
                   }
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }

           return objectsList;
       }


       private string prepareDetailInsert(EmployeeVacEvidScheduleTO scheduleTO)
       {
           StringBuilder sbInsert = new StringBuilder();

           // Insert statement
           sbInsert.Append("INSERT INTO employee_vac_evid_schedule(");
           sbInsert.Append("employee_id, vac_year, segment_id, start_date,end_date, ");
           sbInsert.Append("created_by, created_time) ");
           sbInsert.Append("VALUES (" + scheduleTO.EmployeeID + ", '" + scheduleTO.VacYear.ToString(dateTimeformat) + "', " + scheduleTO.Segment + ", ");
           sbInsert.Append("'" + scheduleTO.StartDate.ToString(dateTimeformat) + "', '" + scheduleTO.EndDate.ToString(dateTimeformat) + "', " );
           sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW())");

           return sbInsert.ToString();
       }

       public int searchCount(string employeeID, DateTime yearFrom, DateTime yearTo, int daysApproveMin, int daysApproveMax)
       {
           int count = 0;
           string select = "";

           try
           {
               StringBuilder sb = new StringBuilder();
               sb.Append("SELECT COUNT(*) vac_evid_num FROM employee_vac_evid eve,employees e,working_units wu ");
               sb.Append("WHERE ");
               sb.Append("eve.employee_id = e.employee_id AND ");
               sb.Append("e.working_unit_id = wu.working_unit_id ");

               if ((employeeID != "") || (!yearFrom.Equals(new DateTime()) && !yearTo.Equals(new DateTime())) || (daysApproveMax != -1) || (daysApproveMax != -1))
               {
                   if (employeeID != "")
                   {
                       sb.Append(" AND eve.employee_id IN (" + employeeID + ") ");
                   }
                   if (!yearFrom.Equals(new DateTime())&&!yearTo.Equals(new DateTime()))
                   {
                       sb.Append(" AND eve.vac_year >= '" +new DateTime(yearFrom.Year,1 ,1).ToString(dateTimeformat)+"'" );
                       sb.Append(" AND eve.vac_year < '" + new DateTime(yearTo.AddYears(1).Year, 1, 1).ToString(dateTimeformat) + "'");
                   }
                   if (daysApproveMin != -1)
                   {
                       sb.Append(" AND eve.num_of_days >= " + daysApproveMin);
                   }
                   if (daysApproveMax != -1)
                   {
                       sb.Append(" AND eve.num_of_days <= " + daysApproveMax);
                   }
                   select = sb.ToString();
               }
               else
               {
                   select = sb.ToString();
               }


               MySqlCommand cmd = new MySqlCommand(select, conn);
               MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);


               DataSet dataSet = new DataSet();
               sqlDataAdapter.Fill(dataSet, "Objects");
               DataTable table = dataSet.Tables["Objects"];

               if (table.Rows.Count > 0)
               {
                   count = Int32.Parse(table.Rows[0]["vac_evid_num"].ToString().Trim());
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }

           return count;
       }

       public bool delete(int employeeID, DateTime vacYear)
       {
           bool isDeleted = false;
           MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

           try
           {
               string select = "";

               // Delete form security_routes_dtl
               select = "DELETE FROM employee_vac_evid_schedule WHERE employee_id = " + employeeID + " AND vac_year = '" + vacYear.ToString(dateTimeformat) + "' ";
               MySqlCommand cmd = new MySqlCommand(select, conn, trans);
               int affectedRows = cmd.ExecuteNonQuery();

               //Delete from security_routes_hdr
               select = "DELETE FROM employee_vac_evid WHERE employee_id = " + employeeID + " AND vac_year = '" + vacYear.ToString(dateTimeformat) + "' ";
               cmd.CommandText = select;

               affectedRows += cmd.ExecuteNonQuery();

               if (affectedRows > 0)
               {
                   isDeleted = true;
                   trans.Commit();
               }
               else
               {
                   trans.Rollback();
               }
           }
           catch (Exception ex)
           {
               trans.Rollback();
               throw ex;
           }

           return isDeleted;
       }

       public bool delete(int employeeID, DateTime vacYear,bool doCommit)
       {
           bool isDeleted = false;
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
               string select = "";

               // Delete form security_routes_dtl
               select = "DELETE FROM employee_vac_evid_schedule WHERE employee_id = " + employeeID + " AND vac_year = '" + vacYear.ToString(dateTimeformat) + "' ";
               MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
               int affectedRows = cmd.ExecuteNonQuery();

               //Delete from security_routes_hdr
               select = "DELETE FROM employee_vac_evid WHERE employee_id = " + employeeID + " AND vac_year = '" + vacYear.ToString(dateTimeformat) + "' ";
               cmd.CommandText = select;

               affectedRows += cmd.ExecuteNonQuery();

               if (affectedRows > 0)
               {
                   isDeleted = true;
                   if (doCommit)
                   {
                       sqlTrans.Commit();
                   }
               }
               else
               {
                   if (doCommit)
                   {
                       sqlTrans.Rollback();
                   }
               }
           }
           catch (MySqlException sqlEx)
           {
               if (doCommit)
                   sqlTrans.Rollback();
               else
                   sqlTrans.Rollback();

               if (sqlEx.Number == 1062)
               {
                   throw new Exception(sqlEx.Number.ToString());
               }
               else
               {
                   throw new Exception(sqlEx.Message);
               }
           }
           catch (Exception ex)
           {
               if (doCommit)
                   sqlTrans.Rollback();
               else
                   sqlTrans.Rollback();
               throw ex;
           }


           return isDeleted;
       }

       public List<EmployeeVacEvidScheduleTO> getVacationSchedules(int employeeID, DateTime vacYear)
       {
           DataSet dataSet = new DataSet();
           EmployeeVacEvidScheduleTO sheduleTO = new EmployeeVacEvidScheduleTO();
           List<EmployeeVacEvidScheduleTO> objectsList = new List<EmployeeVacEvidScheduleTO>();
           string select = "";

           try
           {
               select = "SELECT * FROM employee_vac_evid_schedule ";

               select += "WHERE employee_id = " + employeeID;


               if (!vacYear.Equals(new DateTime()))
               {
                   select += " AND vac_year = '" + vacYear.ToString(dateTimeformat) + "' ";
               }
               select += "ORDER BY employee_id, segment_id";

               MySqlCommand cmd = new MySqlCommand(select, conn);
               MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

               sqlDataAdapter.Fill(dataSet, "EmployeeVacationSchedule");
               DataTable table = dataSet.Tables["EmployeeVacationSchedule"];

               if (table.Rows.Count > 0)
               {
                   foreach (DataRow row in table.Rows)
                   {
                       sheduleTO = new EmployeeVacEvidScheduleTO();

                       sheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                       sheduleTO.VacYear = DateTime.Parse(row["vac_year"].ToString().Trim());
                       sheduleTO.Segment= Int32.Parse(row["segment_id"].ToString().Trim());

                       if (!row["start_date"].Equals(DBNull.Value))
                       {
                           sheduleTO.StartDate = DateTime.Parse(row["start_date"].ToString().Trim());
                       }
                       if (!row["end_date"].Equals(DBNull.Value))
                       {
                           sheduleTO.EndDate = DateTime.Parse(row["end_date"].ToString().Trim());
                       }
                       if (!row["status"].Equals(DBNull.Value))
                       {
                           sheduleTO.Status = row["status"].ToString().Trim();
                       }
                      
                       objectsList.Add(sheduleTO);
                   }
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }

           return objectsList;
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

       public List<EmployeeVacEvidScheduleTO> getVacationSchedules(string employeesString, DateTime yearFrom, DateTime yearTo)
       {
           DataSet dataSet = new DataSet();
           EmployeeVacEvidScheduleTO sheduleTO = new EmployeeVacEvidScheduleTO();
           List<EmployeeVacEvidScheduleTO> objectsList = new List<EmployeeVacEvidScheduleTO>();
           string select = "";

           try
           {
               select = "SELECT * FROM employee_vac_evid_schedule ";

               select += "WHERE employee_id IN (" + employeesString + ") ";


               if (!yearFrom.Equals(new DateTime()))
               {
                   select += " AND vac_year >= '" + yearFrom.ToString(dateTimeformat) + "' ";
                   select += " AND vac_year <= '" + yearTo.ToString(dateTimeformat) + "' ";
               }
               select += "ORDER BY employee_id, segment_id";

               MySqlCommand cmd = new MySqlCommand(select, conn);
               MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

               sqlDataAdapter.Fill(dataSet, "EmployeeVacationSchedule");
               DataTable table = dataSet.Tables["EmployeeVacationSchedule"];

               if (table.Rows.Count > 0)
               {
                   foreach (DataRow row in table.Rows)
                   {
                       sheduleTO = new EmployeeVacEvidScheduleTO();

                       sheduleTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                       sheduleTO.VacYear = DateTime.Parse(row["vac_year"].ToString().Trim());
                       sheduleTO.Segment = Int32.Parse(row["segment_id"].ToString().Trim());

                       if (!row["start_date"].Equals(DBNull.Value))
                       {
                           sheduleTO.StartDate = DateTime.Parse(row["start_date"].ToString().Trim());
                       }
                       if (!row["end_date"].Equals(DBNull.Value))
                       {
                           sheduleTO.EndDate = DateTime.Parse(row["end_date"].ToString().Trim());
                       }
                       if (!row["status"].Equals(DBNull.Value))
                       {
                           sheduleTO.Status = row["status"].ToString().Trim();
                       }

                       objectsList.Add(sheduleTO);
                   }
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }

           return objectsList;
       }
    }
}
