using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Util;
using TransferObjects;

namespace DataAccess
{
    class MySQLPassHistDAO : PassHistDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySQLPassHistDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MySQLPassHistDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public int insert(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int isWrkHrsCount, int locationID,
            int pairGenUsed, int manualCreated, string remarks, string createdBy, DateTime createdTime, bool doCommit)
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
                sbInsert.Append("INSERT INTO passes_hist ");
                sbInsert.Append("(pass_id, employee_id, direction, event_time, pass_type_id, is_wrk_hrs, location_id, pair_gen_used, manual_created, remarks, created_by, created_time, modified_by, modified_time) ");
                sbInsert.Append("VALUES (");

                if (passID != -1)
                {
                    sbInsert.Append("'" + passID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (employeeID != -1)
                {
                    sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!direction.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + direction.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!eventTime.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + eventTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (passTypeID != -1)
                {
                    sbInsert.Append("'" + passTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (isWrkHrsCount != -1)
                {
                    sbInsert.Append("'" + isWrkHrsCount.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (locationID != -1)
                {
                    sbInsert.Append("'" + locationID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (pairGenUsed != -1)
                {
                    sbInsert.Append("'" + pairGenUsed.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (manualCreated != -1)
                {
                    sbInsert.Append("'" + manualCreated.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!remarks.Equals(""))
                {
                    sbInsert.Append("N'" + remarks.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!createdBy.Equals(""))
                {
                    sbInsert.Append("N'" + createdBy.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (!createdTime.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + createdTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbInsert.Append(" NOW() )");


                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
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

        public List<PassHistTO> getPassHistInterval(PassHistTO phTO, DateTime fromTime, DateTime toTime, bool eventTimeChecked, string wUnits, DateTime fromModifiedTime, DateTime toModifiedTime, bool modifiedTimeChecked)
        {
            DataSet dataSet = new DataSet();
            PassHistTO passHist = new PassHistTO();
            List<PassHistTO> passesHistList = new List<PassHistTO>();
            string select = "";

            try
            {
                TimeSpan fromSpan = new TimeSpan(0, 0, 0);
                TimeSpan toSpan = new TimeSpan(23, 59, 0);

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ph.*, empl.last_name, empl.first_name, pt.description, loc.name loc_name, wu.name wu_name ");
                sb.Append("FROM passes_hist ph, employees empl, pass_types pt, locations loc, working_units wu ");
                sb.Append("WHERE ");

                if ((phTO.EmployeeID != -1) || (!phTO.Direction.Trim().Equals("")) ||
                    ((!fromTime.Equals(new DateTime()) && !toTime.Equals(new DateTime())) && eventTimeChecked) ||
                    (phTO.PassTypeID != -1) || (phTO.LocationID != -1)
                    || (!phTO.ModifiedBy.Equals("")) || ((!fromModifiedTime.Equals(new DateTime()) && !toModifiedTime.Equals(new DateTime())) && modifiedTimeChecked))
                {
                    if (phTO.EmployeeID != -1)
                    {
                        //sb.Append(" UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ph.employee_id = '" + phTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (!phTO.Direction.Trim().Equals(""))
                    {
                        sb.Append(" RTRIM(LTRIM(UPPER(ph.direction))) LIKE N'%" + phTO.Direction.Trim().ToUpper() + "%' AND");
                    }
                    if ((!fromTime.Equals(new DateTime()) && !toTime.Equals(new DateTime())) && eventTimeChecked)
                    {
                        //sb.Append(" DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') >='" + fromTime.ToString(dateTimeformat,ci).Trim() + "' AND DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') <= '" + toTime.AddDays(1).ToString(dateTimeformat,ci).Trim() + "' AND");
                        sb.Append(" ph.event_time >= CONVERT('" + fromTime.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append(" ph.event_time < CONVERT('" + toTime.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                    }
                    if ((!fromModifiedTime.Equals(new DateTime()) && !toModifiedTime.Equals(new DateTime())) && modifiedTimeChecked)
                    {
                        //sb.Append(" DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') >='" + fromTime.ToString(dateTimeformat,ci).Trim() + "' AND DATE_FORMAT(ps.event_time,'%Y-%m-%dT%h:%m:%s') <= '" + toTime.AddDays(1).ToString(dateTimeformat,ci).Trim() + "' AND");
                        sb.Append(" ph.modified_time >= CONVERT('" + fromModifiedTime.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append(" ph.modified_time < CONVERT('" + toModifiedTime.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                    }
                    if (phTO.PassTypeID != -1)
                    {
                        //sb.Append(" UPPER(ps.pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ph.pass_type_id = '" + phTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (phTO.LocationID != -1)
                    {
                        //sb.Append(" UPPER(ps.location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" ph.location_id = '" + phTO.LocationID.ToString().Trim() + "' AND");
                    }
                    if (!phTO.ModifiedBy.Equals(""))
                    {
                        sb.Append(" ph.modified_by = N'" + phTO.ModifiedBy.Trim() + "' AND");
                    }

                    //select = sb.ToString(0, sb.ToString().Length - 3);
                }

                select = sb.ToString();

                select += " empl.employee_id = ph.employee_id AND pt.pass_type_id = ph.pass_type_id AND empl.working_unit_id = wu.working_unit_id AND "
                    + "loc.location_id = ph.location_id";
                if (!wUnits.Equals(""))
                {
                    select += " AND empl.working_unit_id IN (" + wUnits + ")";
                }

                select += " ORDER BY ph.employee_id, ph.event_time;";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassHist");
                DataTable table = dataSet.Tables["PassHist"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passHist = new PassHistTO();
                        passHist.RecID = Int32.Parse(row["rec_id"].ToString().Trim());

                        if (row["pass_id"] != DBNull.Value)
                        {
                            passHist.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            passHist.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            passHist.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            passHist.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            passHist.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            passHist.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            passHist.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            passHist.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            passHist.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            passHist.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            passHist.CreatedTime = DateTime.Parse(row["created_time"].ToString());
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                                passHist.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                                passHist.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                                passHist.PassType = row["description"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            passHist.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            passHist.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            passHist.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }
                        if (row["remarks"] != DBNull.Value)
                        {
                            passHist.Remarks = row["remarks"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            passHist.WUName = row["wu_name"].ToString().Trim();
                        }
                            passesHistList.Add(passHist);
                        }
                    }
                }
            
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passesHistList;
        }

        public int getPassesIntervalCount(PassHistTO phTO, DateTime fromTime, DateTime toTime, bool eventTimeChecked, string wUnits, DateTime fromModifiedTime, DateTime toModifiedTime, bool modifiedTimeChecked)
        {
            DataSet dataSet = new DataSet();

            int count = 0;
            string select = "";

            try
            {
                TimeSpan fromSpan = new TimeSpan(0, 0, 0);
                TimeSpan toSpan = new TimeSpan(23, 59, 0);

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(ph.rec_id) count_passHist ");
                sb.Append("FROM passes_hist ph, employees empl ");
                sb.Append("WHERE ");

                if ((phTO.EmployeeID != -1) || (!phTO.Direction.Trim().Equals("")) ||
                    ((!fromTime.Equals(new DateTime()) && !toTime.Equals(new DateTime())) && eventTimeChecked) ||
                    (phTO.PassTypeID != -1) || (phTO.LocationID != -1)
                    || (!phTO.ModifiedBy.Trim().Equals("")) || ((!fromModifiedTime.Equals(new DateTime(0)) && !toModifiedTime.Equals(new DateTime(0))) && modifiedTimeChecked))
                {
                    if (phTO.EmployeeID != -1)
                    {
                        //sb.Append("UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("ph.employee_id = '" + phTO.EmployeeID.ToString().Trim() + "' AND ");
                    }
                    if (!phTO.Direction.Trim().Equals(""))
                    {
                        sb.Append("RTRIM(LTRIM(UPPER(ph.direction))) LIKE N'%" + phTO.Direction.Trim().ToUpper() + "%' AND ");
                    }
                    if ((!fromTime.Equals(new DateTime()) && !toTime.Equals(new DateTime())) && eventTimeChecked)
                    {
                        //sb.Append("CONVERT(datetime, ps.event_time, 101) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND CONVERT(datetime,ps.event_time,101) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND ");
                        sb.Append("ph.event_time >= convert('" + fromTime.ToString("yyyy-MM-dd") + "',datetime) AND ");
                        sb.Append("ph.event_time < convert('" + toTime.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                    }
                    if ((!fromModifiedTime.Equals(new DateTime()) && !toModifiedTime.Equals(new DateTime())) && modifiedTimeChecked)
                    {
                        //sb.Append("CONVERT(datetime, ps.event_time, 101) >= '" + fromTime.ToString(dateTimeformat).Trim() + "' AND CONVERT(datetime,ps.event_time,101) <= '" + toTime.AddDays(1).ToString(dateTimeformat).Trim() + "' AND ");
                        sb.Append("ph.modified_time >= convert('" + fromModifiedTime.ToString("yyyy-MM-dd") + "',datetime) AND ");
                        sb.Append("ph.modified_time < convert('" + toModifiedTime.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                    }
                    if (phTO.PassTypeID != -1)
                    {
                        //sb.Append("UPPER(ps.pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("ph.pass_type_id = '" + phTO.PassTypeID.ToString().Trim() + "' AND ");
                    }
                    if (phTO.LocationID != -1)
                    {
                        //sb.Append("UPPER(ps.location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("ph.location_id = '" + phTO.LocationID.ToString().Trim() + "' AND ");
                    }
                    if (!phTO.ModifiedBy.Trim().Equals(""))
                    {
                        sb.Append("ph.modified_by = N'" + phTO.ModifiedBy.Trim() + "' AND ");
                    }

                }

                select = sb.ToString();

                select += "ph.employee_id = empl.employee_id AND empl.working_unit_id IN (" + wUnits + ");";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassHist");
                DataTable table = dataSet.Tables["PassHist"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_passHist"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return count;
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
            SqlTrans.Commit();
        }

        public void rollbackTransaction()
        {
            SqlTrans.Rollback();
        }

        public IDbTransaction getTransaction()
        {
            return _sqlTrans;
        }

        public void setTransaction(IDbTransaction trans)
        {
            _sqlTrans = (MySqlTransaction)trans;
        }

        public List<PassHistTO> find(int passID)
        {
            DataSet dataSet = new DataSet();
            List<PassHistTO> passHistList = new List<PassHistTO>();
            PassHistTO passHist = new PassHistTO();
            try
            {
                string select = "SELECT ph.*, empl.last_name, empl.first_name, pt.description, loc.name "
                    + "FROM passes_hist ph, employees empl, pass_types pt, locations loc "
                    + "WHERE ph.pass_id = '" + passID.ToString().Trim() + "' AND "
                    + "empl.employee_id = ph.employee_id AND pt.pass_type_id = ph.pass_type_id AND "
                    + "loc.location_id = ph.location_id";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassHist");
                DataTable table = dataSet.Tables["PassHist"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passHist = new PassHistTO();
                        passHist.RecID = Int32.Parse(row["rec_id"].ToString().Trim());

                        if (row["pass_id"] != DBNull.Value)
                        {
                            passHist.PassID = Int32.Parse(row["pass_id"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            passHist.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["direction"] != DBNull.Value)
                        {
                            passHist.Direction = row["direction"].ToString().Trim();
                        }
                        if (row["event_time"] != DBNull.Value)
                        {
                            passHist.EventTime = DateTime.Parse(row["event_time"].ToString());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            passHist.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pair_gen_used"] != DBNull.Value)
                        {
                            passHist.PairGenUsed = Int32.Parse(row["pair_gen_used"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            passHist.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            passHist.ManualyCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs"] != DBNull.Value)
                        {
                            passHist.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            passHist.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            passHist.CreatedTime = DateTime.Parse(row["created_time"].ToString());
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            passHist.EmployeeName = row["last_name"].ToString().Trim();
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            passHist.EmployeeName += " " + row["first_name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            passHist.PassType = row["description"].ToString().Trim();
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            passHist.LocationName = row["name"].ToString().Trim();
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            passHist.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            passHist.ModifiedTime = DateTime.Parse(row["modified_time"].ToString());
                        }
                        if (row["remarks"] != DBNull.Value)
                        {
                            passHist.Remarks = row["remarks"].ToString().Trim();
                        }

                        passHistList.Add(passHist);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passHistList;
        }
    }
}


