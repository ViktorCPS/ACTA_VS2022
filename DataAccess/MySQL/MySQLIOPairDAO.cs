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
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class MySQLIOPairDAO : IOPairDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySQLIOPairDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLIOPairDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public int insert(DateTime IOPairDate, int employeeID, int locationID, int isWrkHrsCounter, int passTypeID,
            DateTime startTime, DateTime endTime,int gate_id, int manualyCreated, bool doCommit)
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
                sbInsert.Append("INSERT INTO io_pairs ");
                sbInsert.Append("(io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time,gate_id, manual_created, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (!IOPairDate.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + IOPairDate.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (employeeID != -1)
                {
                    sbInsert.Append(employeeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (locationID != -1)
                {
                    sbInsert.Append(locationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (isWrkHrsCounter != -1)
                {
                    sbInsert.Append(isWrkHrsCounter.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (passTypeID != -1)
                {
                    sbInsert.Append(passTypeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!startTime.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + startTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!endTime.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + endTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (gate_id != -1)
                {
                    sbInsert.Append(gate_id.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (manualyCreated != -1)
                {
                    sbInsert.Append(manualyCreated.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }

            catch (MySqlException sqlex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                if (sqlex.Number == 1062)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();

                DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
                throw procEx;
            }

            return rowsAffected;
        }

        public int insertWholeDayAbsence(DateTime IOPairDate, int employeeID, int locationID, int isWrkHrsCounter, int passTypeID,
            DateTime startTime, DateTime endTime, int manualyCreated)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;

            try
            {
                DataSet dataSet = new DataSet();
                String select = "SELECT * FROM io_pairs WHERE io_pair_date = '" + IOPairDate.Date.ToString(dateTimeformat) + "' "
                    + "AND employee_id = '" + employeeID.ToString() + "' AND pass_type_id = '" + passTypeID.ToString() + "'";

                MySqlCommand command = new MySqlCommand(select, conn, sqlTrans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];
                if (table.Rows.Count == 0)
                {

                    StringBuilder sbInsert = new StringBuilder();
                    sbInsert.Append("INSERT INTO io_pairs ");
                    sbInsert.Append("(io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, created_by, created_time) ");
                    sbInsert.Append("VALUES (");

                    if (!IOPairDate.Equals(new DateTime()))
                    {
                        sbInsert.Append("'" + IOPairDate.ToString(dateTimeformat) + "', ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (employeeID != -1)
                    {
                        sbInsert.Append(employeeID.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (locationID != -1)
                    {
                        sbInsert.Append(locationID.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (isWrkHrsCounter != -1)
                    {
                        sbInsert.Append(isWrkHrsCounter.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (passTypeID != -1)
                    {
                        sbInsert.Append(passTypeID.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (!startTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                    {
                        sbInsert.Append("'" + startTime.ToString(dateTimeformat) + "', ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (!endTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                    {
                        sbInsert.Append("'" + endTime.ToString(dateTimeformat) + "', ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (manualyCreated != -1)
                    {
                        sbInsert.Append(manualyCreated.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }

                    sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                    MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                    rowsAffected = cmd.ExecuteNonQuery();
                    sqlTrans.Commit();
                }
            }

            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 1062)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return rowsAffected;
        }

        /// <summary>
        /// Insert IOPair record
        /// </summary>
        /// <param name="pairTO">IOPair data</param>
        /// <param name="doCommit">If value is true - start new transaction, 
        /// false - use existing tranasction, </param>
        /// <returns>rows affected</returns>
        public int insert(IOPairTO pairTO, bool doCommit)
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
                sbInsert.Append("INSERT INTO io_pairs ");
                sbInsert.Append("(io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (!pairTO.IOPairDate.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + pairTO.IOPairDate.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.EmployeeID != -1)
                {
                    sbInsert.Append(pairTO.EmployeeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.LocationID != -1)
                {
                    sbInsert.Append(pairTO.LocationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.IsWrkHrsCount != -1)
                {
                    sbInsert.Append(pairTO.IsWrkHrsCount.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.PassTypeID != -1)
                {
                    sbInsert.Append(pairTO.PassTypeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    sbInsert.Append("'" + pairTO.StartTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    sbInsert.Append("'" + pairTO.EndTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.ManualCreated != -1)
                {
                    sbInsert.Append(pairTO.ManualCreated.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }

            }

            catch (MySqlException sqlex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                if (sqlex.Number == 1062)		// UNIQUE key constraint violation
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message + pairTO.ToString(), 12);
                    //throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message + pairTO.ToString(), 11);
                    sqlTrans.Rollback();
                    //throw procEx;
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
                //throw procEx;
            }

            return rowsAffected;
        }

        //tamara 3.5.2018.
        public int insert2(IOPairTO pairTO, bool doCommit)
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
                sbInsert.Append("INSERT INTO io_pairs ");
                sbInsert.Append("(io_pair_date,pair_processed_gen_used, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (!pairTO.IOPairDate.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + pairTO.IOPairDate.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                //
                if (pairTO.ProcessedGenUsed != -1)
                {
                    sbInsert.Append(pairTO.ProcessedGenUsed.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                //
                if (pairTO.EmployeeID != -1)
                {
                    sbInsert.Append(pairTO.EmployeeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.LocationID != -1)
                {
                    sbInsert.Append(pairTO.LocationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.IsWrkHrsCount != -1)
                {
                    sbInsert.Append(pairTO.IsWrkHrsCount.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.PassTypeID != -1)
                {
                    sbInsert.Append(pairTO.PassTypeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    sbInsert.Append("'" + pairTO.StartTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    sbInsert.Append("'" + pairTO.EndTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.ManualCreated != -1)
                {
                    sbInsert.Append(pairTO.ManualCreated.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();

                if (doCommit)
                {
                    sqlTrans.Commit();
                }

            }

            catch (MySqlException sqlex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                if (sqlex.Number == 1062)		// UNIQUE key constraint violation
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message + pairTO.ToString(), 12);
                    //throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message + pairTO.ToString(), 11);
                    sqlTrans.Rollback();
                    //throw procEx;
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
                //throw procEx;
            }

            return rowsAffected;
        }

        public int insertWholeDayAbsence(IOPairTO pairTO, bool doCommit)
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
                DataSet dataSet = new DataSet();
                String select = "SELECT * FROM io_pairs WHERE io_pair_date = '" + pairTO.IOPairDate.Date.ToString(dateTimeformat) + "' "
                    + "AND employee_id = '" + pairTO.EmployeeID.ToString() + "' AND start_time = '" + pairTO.StartTime.ToString(dateTimeformat) + "' "
                    + "AND end_time = '" + pairTO.EndTime.ToString(dateTimeformat) + "'";

                MySqlCommand command = new MySqlCommand(select, conn, sqlTrans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];
                if (table.Rows.Count == 0)
                {
                    StringBuilder sbInsert = new StringBuilder();
                    sbInsert.Append("INSERT INTO io_pairs ");
                    sbInsert.Append("(io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, created_by, created_time) ");
                    sbInsert.Append("VALUES (");

                    if (!pairTO.IOPairDate.Equals(new DateTime()))
                    {
                        sbInsert.Append("'" + pairTO.IOPairDate.ToString(dateTimeformat).Trim() + "', ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (pairTO.EmployeeID != -1)
                    {
                        sbInsert.Append(pairTO.EmployeeID.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (pairTO.LocationID != -1)
                    {
                        sbInsert.Append(pairTO.LocationID.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (pairTO.IsWrkHrsCount != -1)
                    {
                        sbInsert.Append(pairTO.IsWrkHrsCount.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (pairTO.PassTypeID != -1)
                    {
                        sbInsert.Append(pairTO.PassTypeID.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (!pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                    {
                        sbInsert.Append("'" + pairTO.StartTime.ToString(dateTimeformat).Trim() + "', ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (!pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                    {
                        sbInsert.Append("'" + pairTO.EndTime.ToString(dateTimeformat).Trim() + "', ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }
                    if (pairTO.ManualCreated != -1)
                    {
                        sbInsert.Append(pairTO.ManualCreated.ToString().Trim() + ", ");
                    }
                    else
                    {
                        sbInsert.Append("NULL, ");
                    }

                    sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                    MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                    rowsAffected = cmd.ExecuteNonQuery();
                }

                if (doCommit)
                {
                    sqlTrans.Commit();
                }

            }

            catch (MySqlException sqlex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                if (sqlex.Number == 1062)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message + pairTO.ToString(), 12);
                    //throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message + pairTO.ToString(), 11);
                    //throw procEx;
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                DataProcessingException procEx = new DataProcessingException(ex.Message, 11);
                //throw procEx;
            }

            return rowsAffected;
        }

        public int insertExtraHourPair(DateTime IOPairDate, int employeeID, int locationID, int isWrkHrsCounter,
            int passTypeID, DateTime startTime, DateTime endTime, int manualyCreated, bool doCommit)
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

            int ioPairID = -1;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                //sbInsert.Append("SET NOCOUNT ON ");

                sbInsert.Append("INSERT INTO io_pairs ");
                sbInsert.Append("(io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (!IOPairDate.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + IOPairDate.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (employeeID != -1)
                {
                    sbInsert.Append(employeeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (locationID != -1)
                {
                    sbInsert.Append(locationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (isWrkHrsCounter != -1)
                {
                    sbInsert.Append(isWrkHrsCounter.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (passTypeID != -1)
                {
                    sbInsert.Append(passTypeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!startTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    sbInsert.Append("'" + startTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!endTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    sbInsert.Append("'" + endTime.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (manualyCreated != -1)
                {
                    sbInsert.Append(manualyCreated.ToString().Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                sbInsert.Append(" N'" + Constants.extraHoursCreatedBy + "', NOW()); ");

                //sbInsert.Append("SELECT @@Identity AS io_pair_id, @@Error as error ");
                sbInsert.Append("SELECT LAST_INSERT_ID() AS io_pair_id ");
                //sbInsert.Append("SET NOCOUNT OFF ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "IOPairsId");
                DataTable table = dataSet.Tables["IOPairsId"];

                //int error = int.Parse(((DataRow) table.Rows[0])["error"].ToString());
                //if (error == 0) //OK
                //{
                ioPairID = int.Parse(((DataRow)table.Rows[0])["io_pair_id"].ToString());

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
                /*}
                else
                {
                    if (doCommit)
                    {
                        sqlTrans.Rollback("INSERT");
                    }
                }*/
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return ioPairID;
        }

        public bool delete(int IOPairID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs WHERE io_pair_id = '" + IOPairID.ToString().Trim() + "'");

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

        public bool delete(int IOPairID, bool doCommit)
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
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs WHERE io_pair_id = '" + IOPairID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }

                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool deletePairPasses(List<IOPairTO> iopairs, List<PassTO> passes, DAOFactory factory)
        {
            bool isDeleted = true;
            this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            MySQLPassDAO passDAO = (MySQLPassDAO)factory.getPassDAO(conn);
            passDAO.SqlTrans = this.SqlTrans;

            try
            {
                foreach (PassTO passTO in passes)
                {
                    isDeleted = passDAO.delete(passTO.PassID.ToString().Trim(), false) && isDeleted;
                }

                if (isDeleted)
                {
                    foreach (IOPairTO iopTO in iopairs)
                    {
                        isDeleted = delete(iopTO.IOPairID, false) && isDeleted;
                    }
                }

                if (isDeleted)
                {
                    this.SqlTrans.Commit();
                }
                else
                {
                    this.SqlTrans.Rollback();
                }
            }
            catch (Exception ex)
            {
                this.SqlTrans.Rollback();
                throw ex;
            }

            return isDeleted;
        }

        private bool delete(int employeeID, string date, bool doCommit)
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
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs WHERE employee_id = '" + employeeID.ToString().Trim() + "' ");
                sbDelete.Append("AND DATE_FORMAT(io_pair_date,'%Y-%m-%d') = '" + Convert.ToDateTime(date.Trim()).ToString("yyyy-MM-dd").Trim() + "' ");
                sbDelete.Append("AND manual_created = '" + ((int)Constants.recordCreated.Automaticaly) + "' ");
                sbDelete.Append("AND pass_type_id IN (SELECT pass_type_id FROM pass_types WHERE pass_type IN (0,1))");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
                }
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }

                isDeleted = false;

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool delete(string IOPairID)
        {
            if (!IOPairID.Equals(""))
            {
                bool isDeleted = false;
                MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                try
                {
                    StringBuilder sbDelete = new StringBuilder();
                    sbDelete.Append("DELETE FROM io_pairs WHERE io_pair_id IN (" + IOPairID.ToString().Trim() + ")");

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
            else
                return true;
        }

        public bool delete(string IOPairID, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;

            if (!IOPairID.Equals(""))
            {
                bool isDeleted = false;
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
                    StringBuilder sbDelete = new StringBuilder();
                    sbDelete.Append("DELETE FROM io_pairs WHERE io_pair_id IN (" + IOPairID.ToString().Trim() + ")");

                    MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                    int res = cmd.ExecuteNonQuery();
                    if (res != 0)
                    {
                        isDeleted = true;
                    }

                    if (doCommit)
                    {
                        sqlTrans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (doCommit)
                    {
                        sqlTrans.Rollback();
                    }
                    throw new Exception(ex.Message);
                }

                return isDeleted;
            }
            else
                return true;
        }

        public IOPairTO find(int IOPairID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            try
            {
                string select = "SELECT * FROM io_pairs WHERE io_pair_id = '" + IOPairID.ToString().Trim() + "'";
                MySqlCommand cmd;
                if (SqlTrans != null)
                    cmd = new MySqlCommand(select, conn, SqlTrans);
                else
                    cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPair");
                DataTable table = dataSet.Tables["IOPair"];

                if (table.Rows.Count == 1)
                {
                    pairTO = new IOPairTO();
                    pairTO.IOPairID = Int32.Parse(table.Rows[0]["io_pair_id"].ToString().Trim());
                    if (table.Rows[0]["io_pair_date"] != DBNull.Value)
                    {
                        pairTO.IOPairDate = DateTime.Parse(table.Rows[0]["io_pair_date"].ToString());
                    }
                    if (table.Rows[0]["employee_id"] != DBNull.Value)
                    {
                        pairTO.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["location_id"] != DBNull.Value)
                    {
                        pairTO.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["is_wrk_hrs_counter"] != DBNull.Value)
                    {
                        pairTO.IsWrkHrsCount = Int32.Parse(table.Rows[0]["is_wrk_hrs_counter"].ToString().Trim());
                    }
                    if (table.Rows[0]["pass_type_id"] != DBNull.Value)
                    {
                        pairTO.PassTypeID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["start_time"] != DBNull.Value)
                    {
                        pairTO.StartTime = DateTime.Parse(table.Rows[0]["start_time"].ToString());
                    }
                    if (table.Rows[0]["end_time"] != DBNull.Value)
                    {
                        pairTO.EndTime = DateTime.Parse(table.Rows[0]["end_time"].ToString());
                    }
                    if (table.Rows[0]["manual_created"] != DBNull.Value)
                    {
                        pairTO.ManualCreated = Int32.Parse(table.Rows[0]["manual_created"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairTO;
        }
        public bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE io_pairs SET pair_processed_gen_used = 0 WHERE ");

                foreach (int empl in emplDateList.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                string select = sbUpdate.ToString().Substring(0, sbUpdate.ToString().Length - 2);

                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();
            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE io_pairs SET pair_processed_gen_used = 0 WHERE ");

                foreach (int empl in emplDateList.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                foreach (int empl in emplDateListDayAfter.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND ");
                    sbUpdate.Append(" DATE_FORMAT(start_time,'%H:%i') <= '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                foreach (int empl in emplDateListDayBefore.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND ");
                    sbUpdate.Append(" DATE_FORMAT(start_time,'%H:%i') > '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                string select = sbUpdate.ToString().Substring(0, sbUpdate.ToString().Length - 2);

                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();
            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }


        //tamara 29.06.2018. postavlja pair_processed_gen_used na 0 ali samo za is_wrk_hrs_counter=1
        public bool updateToUnprocessedWorkCount(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE io_pairs SET pair_processed_gen_used = 0 WHERE is_wrk_hrs_counter=1 and ");

                foreach (int empl in emplDateList.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                foreach (int empl in emplDateListDayAfter.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND ");
                    sbUpdate.Append(" CONVERT(VARCHAR(5), start_time, 108) <= '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateListDayAfter[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                foreach (int empl in emplDateListDayBefore.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND ");
                    sbUpdate.Append(" CONVERT(VARCHAR(5), start_time, 108) > '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateListDayBefore[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                string select = sbUpdate.ToString().Substring(0, sbUpdate.ToString().Length - 2);

                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
                cmd.CommandTimeout = Constants.commandTimeout;
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

        public bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE io_pairs SET pair_processed_gen_used = 0 WHERE ");

                foreach (int empl in emplDateList.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                foreach (int empl in emplDateListDayAfter.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND ");
                    sbUpdate.Append(" DATE_FORMAT(start_time,'%H:%i') <= '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                foreach (int empl in emplDateListDayBefore.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND ");
                    sbUpdate.Append(" DATE_FORMAT(start_time,'%H:%i') > '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                string select = sbUpdate.ToString().Substring(0, sbUpdate.ToString().Length - 2);

                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (MySqlException sqlex)
            {
                if (doCommit)
                    sqlTrans.Rollback();

                if (sqlex.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE io_pairs SET pair_processed_gen_used = 0 WHERE ");

                foreach (int empl in emplDateList.Keys)
                {
                    sbUpdate.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbUpdate.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbUpdate.Remove(sbUpdate.ToString().Length - 2, 2);
                    sbUpdate.Append(")) OR");
                }
                string select = sbUpdate.ToString().Substring(0, sbUpdate.ToString().Length - 2);

                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();

                isUpdated = true;

                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (MySqlException sqlex)
            {
                if (doCommit)
                    sqlTrans.Rollback();

                if (sqlex.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool update(int IOPairID, DateTime IOPairDate, int employeeID, int locationID, int passTypeID, int isWrkHrsCounter,
            DateTime startTime, DateTime endTime, int manualyCreated, string createdBy)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE io_pairs SET ");

                if (!IOPairDate.Equals(new DateTime()))
                {
                    sbUpdate.Append("io_pair_date = '" + IOPairDate.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("io_pair_date = NULL, ");
                }
                if (employeeID != -1)
                {
                    sbUpdate.Append("employee_id = " + employeeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("employee_id = NULL, ");
                }
                if (locationID != -1)
                {
                    sbUpdate.Append("location_id = " + locationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("location_id = NULL, ");
                }
                if (isWrkHrsCounter != -1)
                {
                    sbUpdate.Append("is_wrk_hrs_counter = " + isWrkHrsCounter.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("is_wrk_hrs_counter = NULL, ");
                }
                if (passTypeID != -1)
                {
                    sbUpdate.Append("pass_type_id = " + passTypeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("pass_type_id = NULL, ");
                }

                if (!startTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    sbUpdate.Append("start_time = '" + startTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("start_time = NULL, ");
                }
                if (!endTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    sbUpdate.Append("end_time = '" + endTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("end_time = NULL, ");
                }
                if (manualyCreated != -1)
                {
                    sbUpdate.Append("manual_created = " + manualyCreated.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("manual_created = NULL, ");
                }
                if (createdBy.Equals(""))
                {
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("modified_by = N'" + createdBy.Trim() + "', ");
                }
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE io_pair_id = '" + IOPairID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 1062)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool updateIOPairsPasses(int employeeID, string date, DAOFactory factory)
        {
            MySQLPassDAO passDAO = (MySQLPassDAO)factory.getPassDAO(conn);
            bool isUpdated = false;
            this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            passDAO.SqlTrans = this.SqlTrans;

            try
            {
                isUpdated = delete(employeeID, date, false);

                if (isUpdated)
                {
                    isUpdated = passDAO.update(employeeID, date, false);
                }

                if (isUpdated)
                {
                    this.SqlTrans.Commit();
                }
                else
                {
                    this.SqlTrans.Rollback();
                }
            }
            catch (Exception ex)
            {
                this.SqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public bool update(IOPairTO pairTO, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = this.SqlTrans;

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE io_pairs SET ");

                if (!pairTO.IOPairDate.Equals(new DateTime()))
                {
                    sbUpdate.Append("io_pair_date = '" + pairTO.IOPairDate.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("io_pair_date = NULL, ");
                }
                if (pairTO.EmployeeID != -1)
                {
                    sbUpdate.Append("employee_id = " + pairTO.EmployeeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("employee_id = NULL, ");
                }
                if (pairTO.LocationID != -1)
                {
                    sbUpdate.Append("location_id = " + pairTO.LocationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("location_id = NULL, ");
                }
                if (pairTO.IsWrkHrsCount != -1)
                {
                    sbUpdate.Append("is_wrk_hrs_counter = " + pairTO.IsWrkHrsCount.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("is_wrk_hrs_counter = NULL, ");
                }
                if (pairTO.PassTypeID != -1)
                {
                    sbUpdate.Append("pass_type_id = " + pairTO.PassTypeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("pass_type_id = NULL, ");
                }

                if (!pairTO.StartTime.Equals(new DateTime()))
                {
                    sbUpdate.Append("start_time = '" + pairTO.StartTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("start_time = NULL, ");
                }
                if (!pairTO.EndTime.Equals(new DateTime()))
                {
                    sbUpdate.Append("end_time = '" + pairTO.EndTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("end_time = NULL, ");
                }
                if (pairTO.ManualCreated != -1)
                {
                    sbUpdate.Append("manual_created = " + pairTO.ManualCreated.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("manual_created = NULL, ");
                }
                if (pairTO.ProcessedGenUsed != -1)
                {
                    sbUpdate.Append("pair_processed_gen_used = " + pairTO.ProcessedGenUsed.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("pair_processed_gen_used = NULL, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE io_pair_id = '" + pairTO.IOPairID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public bool update(IOPairTO pairTO)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE io_pairs SET ");

                if (!pairTO.IOPairDate.Equals(new DateTime()))
                {
                    sbUpdate.Append("io_pair_date = '" + pairTO.IOPairDate.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("io_pair_date = NULL, ");
                }
                if (pairTO.EmployeeID != -1)
                {
                    sbUpdate.Append("employee_id = " + pairTO.EmployeeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("employee_id = NULL, ");
                }
                if (pairTO.LocationID != -1)
                {
                    sbUpdate.Append("location_id = " + pairTO.LocationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("location_id = NULL, ");
                }
                if (pairTO.IsWrkHrsCount != -1)
                {
                    sbUpdate.Append("is_wrk_hrs_counter = " + pairTO.IsWrkHrsCount.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("is_wrk_hrs_counter = NULL, ");
                }
                if (pairTO.PassTypeID != -1)
                {
                    sbUpdate.Append("pass_type_id = " + pairTO.PassTypeID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("pass_type_id = NULL, ");
                }

                if (!pairTO.StartTime.Equals(new DateTime()))
                {
                    sbUpdate.Append("start_time = '" + pairTO.StartTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("start_time = NULL, ");
                }
                if (!pairTO.EndTime.Equals(new DateTime()))
                {
                    sbUpdate.Append("end_time = '" + pairTO.EndTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("end_time = NULL, ");
                }
                if (pairTO.ManualCreated != -1)
                {
                    sbUpdate.Append("manual_created = " + pairTO.ManualCreated.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("manual_created = NULL, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE io_pair_id = '" + pairTO.IOPairID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 1062)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public DateTime getFirstArrivedTime(int emplID, DateTime date)
        {
            DataSet dataSet = new DataSet();
            DateTime arrived = new DateTime();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT MIN(start_time) as first_arrived FROM io_pairs ");
                sb.Append(" WHERE start_time IS NOT NULL AND end_time IS NOT NULL AND employee_id = " + emplID.ToString() + "io_pair_date = DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) = '" + date.ToString("yyy-MM-dd HH:mm:ss") + "'");

                select = sb.ToString();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    arrived = (DateTime)row["io_pair_date"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return arrived;
        }
        public List<IOPairTO> getIOPairsClosed(IOPairTO pairTO)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pair = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs ");
                sb.Append(" WHERE start_time IS NOT NULL AND end_time IS NOT NULL ");
                if ((pairTO.IOPairID != -1) || (!pairTO.IOPairDate.Equals(new DateTime())) || (pairTO.EmployeeID != -1) ||
                    (pairTO.LocationID != -1) || (pairTO.IsWrkHrsCount != -1) || (pairTO.PassTypeID != -1) ||
                    (!pairTO.StartTime.Equals(new DateTime())) || (!pairTO.EndTime.Equals(new DateTime())) ||
                    (pairTO.ManualCreated != -1) || (pairTO.ProcessedGenUsed != -1))
                {
                    sb.Append(" AND");

                    if (pairTO.IOPairID != -1)
                    {
                        //sb.Append(" UPPER(io_pair_id) LIKE '" + IOPairID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" io_pair_id = '" + pairTO.IOPairID.ToString().Trim() + "' AND");
                    }
                    if (!pairTO.IOPairDate.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.IOPairDate.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (pairTO.EmployeeID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" employee_id = '" + pairTO.EmployeeID.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.ProcessedGenUsed != -1)
                    {
                        sb.Append(" pair_processed_gen_used = '" + pairTO.ProcessedGenUsed.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.LocationID != -1)
                    {
                        //sb.Append(" UPPER(location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND");
                    }
                    if (pairTO.IsWrkHrsCount != -1)
                    {
                        //sb.Append(" UPPER(is_wrk_hrs_counter) LIKE '" + isWrkHrsCounter.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" is_wrk_hrs_counter = '" + pairTO.IsWrkHrsCount.ToString().Trim() + "' AND");
                    }
                    if (pairTO.PassTypeID != -1)
                    {
                        //sb.Append(" UPPER(pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" pass_type_id = '" + pairTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!pairTO.StartTime.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(start_time,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.StartTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (!pairTO.EndTime.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(end_time,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.EndTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (pairTO.ManualCreated != -1)
                    {
                        //sb.Append(" UPPER(manual_created) LIKE '" + manualyCreated.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" manual_created = '" + pairTO.ManualCreated.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY start_time", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairTO();
                        pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pair.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pair.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pair.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pair.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }
        public List<IOPairTO> getIOPairsClosed(IOPairTO pairTO, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pair = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs ");
                sb.Append(" WHERE start_time IS NOT NULL AND end_time IS NOT NULL ");
                if ((pairTO.IOPairID != -1) || (!pairTO.IOPairDate.Equals(new DateTime())) || (pairTO.EmployeeID != -1) ||
                    (pairTO.LocationID != -1) || (pairTO.IsWrkHrsCount != -1) || (pairTO.PassTypeID != -1) ||
                    (!pairTO.StartTime.Equals(new DateTime())) || (!pairTO.EndTime.Equals(new DateTime())) ||
                    (pairTO.ManualCreated != -1) || (pairTO.ProcessedGenUsed != -1) || (fromDate != new DateTime()) || (toDate != new DateTime()))
                {
                    sb.Append(" AND");

                    if (pairTO.IOPairID != -1)
                    {
                        //sb.Append(" UPPER(io_pair_id) LIKE '" + IOPairID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" io_pair_id = '" + pairTO.IOPairID.ToString().Trim() + "' AND");
                    }
                    if (!pairTO.IOPairDate.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.IOPairDate.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) >= '" + fromDate.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) <= '" + toDate.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (pairTO.EmployeeID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" employee_id = '" + pairTO.EmployeeID.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.ProcessedGenUsed != -1)
                    {
                        sb.Append(" pair_processed_gen_used = '" + pairTO.ProcessedGenUsed.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.LocationID != -1)
                    {
                        //sb.Append(" UPPER(location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND");
                    }
                    if (pairTO.IsWrkHrsCount != -1)
                    {
                        //sb.Append(" UPPER(is_wrk_hrs_counter) LIKE '" + isWrkHrsCounter.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" is_wrk_hrs_counter = '" + pairTO.IsWrkHrsCount.ToString().Trim() + "' AND");
                    }
                    if (pairTO.PassTypeID != -1)
                    {
                        //sb.Append(" UPPER(pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" pass_type_id = '" + pairTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!pairTO.StartTime.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(start_time,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.StartTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (!pairTO.EndTime.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(end_time,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.EndTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (pairTO.ManualCreated != -1)
                    {
                        //sb.Append(" UPPER(manual_created) LIKE '" + manualyCreated.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" manual_created = '" + pairTO.ManualCreated.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY start_time", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairTO();
                        pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pair.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pair.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pair.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pair.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }
        public List<IOPairTO> getIOPairs(IOPairTO pairTO)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pair = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs ");

                if ((pairTO.IOPairID != -1) || (!pairTO.IOPairDate.Equals(new DateTime())) || (pairTO.EmployeeID != -1) ||
                    (pairTO.LocationID != -1) || (pairTO.IsWrkHrsCount != -1) || (pairTO.PassTypeID != -1) ||
                    (!pairTO.StartTime.Equals(new DateTime())) || (!pairTO.EndTime.Equals(new DateTime())) ||
                    (pairTO.ManualCreated != -1) || (pairTO.ProcessedGenUsed != -1))
                {
                    sb.Append(" WHERE");

                    if (pairTO.IOPairID != -1)
                    {
                        //sb.Append(" UPPER(io_pair_id) LIKE '" + IOPairID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" io_pair_id = '" + pairTO.IOPairID.ToString().Trim() + "' AND");
                    }
                    if (!pairTO.IOPairDate.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.IOPairDate.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (pairTO.EmployeeID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" employee_id = '" + pairTO.EmployeeID.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.ProcessedGenUsed != -1)
                    {
                        sb.Append(" pair_processed_gen_used = '" + pairTO.ProcessedGenUsed.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.LocationID != -1)
                    {
                        //sb.Append(" UPPER(location_id) LIKE '" + locationID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND");
                    }
                    if (pairTO.IsWrkHrsCount != -1)
                    {
                        //sb.Append(" UPPER(is_wrk_hrs_counter) LIKE '" + isWrkHrsCounter.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" is_wrk_hrs_counter = '" + pairTO.IsWrkHrsCount.ToString().Trim() + "' AND");
                    }
                    if (pairTO.PassTypeID != -1)
                    {
                        //sb.Append(" UPPER(pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" pass_type_id = '" + pairTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!pairTO.StartTime.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(start_time,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.StartTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (!pairTO.EndTime.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(end_time,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.EndTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (pairTO.ManualCreated != -1)
                    {
                        //sb.Append(" UPPER(manual_created) LIKE '" + manualyCreated.ToString().Trim().ToUpper() + "' AND");
                        sb.Append(" manual_created = '" + pairTO.ManualCreated.ToString().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairTO();
                        pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pair.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pair.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pair.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pair.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public int insert(IOPairTO ioPairTO, PassTO firstPass, PassTO secondPass, DAOFactory factory)
        {
            int rowsAffected = 0;
            this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            MySQLPassDAO passDAO = (MySQLPassDAO)factory.getPassDAO(conn);

            try
            {
                // Insert into IOPair
                if (ioPairTO.IsWrkHrsCount == (int)Constants.IsWrkCount.IsNotCounter) //tamara 3.5.2018.
                {
                    rowsAffected += this.insert2(ioPairTO, false);
                }
                else
                {
                    rowsAffected += this.insert(ioPairTO, false);
                }

                // Update Passes
                passDAO.SqlTrans = this.SqlTrans;

                if (firstPass.PassID >= 0)
                {
                    rowsAffected += passDAO.update(firstPass, false) ? 1 : 0;
                }

                if (secondPass.PassID >= 0)
                {
                    rowsAffected += passDAO.update(secondPass, false) ? 1 : 0;
                }

                SqlTrans.Commit();

            }
            catch (Exception ex)
            {
                SqlTrans.Rollback();
                throw ex;
            }
            return rowsAffected;
        }

        public int getIOPairsForWUCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            int count = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) count ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (workingUnitID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" empl.working_unit_id IN (" + wUnits + ") AND");
                }

                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public List<IOPairTO> getIOPairsForWU(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description, wu.name wu_name ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (workingUnitID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" empl.working_unit_id IN (" + wUnits + ") AND");
                }

                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id ORDER BY wu.name, empl.last_name, empl.first_name, iop.io_pair_date ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["empl_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeName = row["empl_name"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["pt_description"] != DBNull.Value)
                        {
                            pairTO.PassType = row["pt_description"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pairTO.WUName = row["wu_name"].ToString().Trim();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsForWUDate(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate, int locID, int isWrkHrs)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description, wu.name wu_name ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (workingUnitID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" empl.working_unit_id IN (" + wUnits + ") AND");
                }
                if (locID != -1)
                    sb.Append(" iop.location_id = '" + locID.ToString() + "' AND");
                if (isWrkHrs != -1)
                    sb.Append(" iop.is_wrk_hrs_counter = '" + isWrkHrs.ToString() + "' AND");

                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id ORDER BY employee_id, io_pair_date, start_time ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["empl_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeName = row["empl_name"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["pt_description"] != DBNull.Value)
                        {
                            pairTO.PassType = row["pt_description"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pairTO.WUName = row["wu_name"].ToString().Trim();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public int getIOPairsNotInWUCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            int count = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) count ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (workingUnitID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" empl.working_unit_id NOT IN (" + wUnits + ") AND");
                }

                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }


        public List<IOPairTO> getIOPairsNotInWUDate(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description, wu.name wu_name ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (workingUnitID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" empl.working_unit_id NOT IN (" + wUnits + ") AND");
                }

                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id ORDER BY employee_id, io_pair_date, start_time ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["empl_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeName = row["empl_name"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["pt_description"] != DBNull.Value)
                        {
                            pairTO.PassType = row["pt_description"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pairTO.WUName = row["wu_name"].ToString().Trim();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }


        public int getIOPairsForWUWrkHrsCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            int count = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) count ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");
                sb.Append(" iop.is_wrk_hrs_counter = 1 AND");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (workingUnitID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" empl.working_unit_id IN (" + wUnits + ") AND");
                }

                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public List<IOPairTO> getIOPairsForWUWrkHrs(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description, wu.name wu_name ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");
                sb.Append(" iop.is_wrk_hrs_counter = 1 AND");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (workingUnitID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" empl.working_unit_id IN (" + wUnits + ") AND");
                }

                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id ORDER BY wu.name, empl.last_name, empl.first_name, iop.io_pair_date ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["empl_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeName = row["empl_name"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["pt_description"] != DBNull.Value)
                        {
                            pairTO.PassType = row["pt_description"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pairTO.WUName = row["wu_name"].ToString().Trim();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsForExtraHours(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT io_pair_id, io_pair_date, employee_id, start_time, end_time, manual_created");
                sb.Append(" FROM io_pairs WHERE");
                sb.Append(" is_wrk_hrs_counter = 1");
                sb.Append(" AND start_time IS NOT NULL AND end_time IS NOT NULL");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" AND io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "'");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" AND io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "'");
                }
                if (employeesList.Count > 0)
                {
                    sb.Append(" AND employee_id IN (");
                    foreach (int employeeID in employeesList)
                    {
                        sb.Append(employeeID + ", ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")");
                }
                sb.Append(" ORDER BY employee_id, io_pair_date, start_time");
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsAll(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT io_pair_id, io_pair_date, employee_id, start_time, end_time,pass_type_id");
                sb.Append(" FROM io_pairs WHERE");
                sb.Append(" start_time IS NOT NULL AND end_time IS NOT NULL AND");
                sb.Append(" is_wrk_hrs_counter = 1");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" AND io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "'");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" AND io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "'");
                }
                if (employeesList.Count > 0)
                {
                    sb.Append(" AND employee_id IN (");
                    foreach (int employeeID in employeesList)
                    {
                        sb.Append(employeeID + ", ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")");
                }
                sb.Append(" ORDER BY employee_id, io_pair_date, start_time");
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public int getIOPairsForEmplDateCount(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            string select;
            int count = 0;
            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) count ");
                    sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" AND iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" AND iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(")");
                    }
                    sb.Append("AND iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id");
                    select = sb.ToString();

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];
                    if (table.Rows.Count > 0)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public List<IOPairTO> getIOPairsForEmplDate(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description, wu.name wu_name ");
                    sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");
                    sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" AND iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" AND iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(")");
                    }
                    sb.Append("AND iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id");
                    select = sb.ToString();

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pairTO = new IOPairTO();
                            pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                            if (row["io_pair_date"] != DBNull.Value)
                            {
                                pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                            }
                            if (row["employee_id"] != DBNull.Value)
                            {
                                pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (row["location_id"] != DBNull.Value)
                            {
                                pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                            }
                            if (row["is_wrk_hrs_counter"] != DBNull.Value)
                            {
                                pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                            }
                            if (row["pass_type_id"] != DBNull.Value)
                            {
                                pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                            }
                            if (row["start_time"] != DBNull.Value)
                            {
                                pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                            }
                            if (row["end_time"] != DBNull.Value)
                            {
                                pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                            }
                            if (row["manual_created"] != DBNull.Value)
                            {
                                pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                            }
                            if (row["empl_name"] != DBNull.Value)
                            {
                                pairTO.EmployeeName = row["empl_name"].ToString().Trim();
                            }
                            if (row["empl_last_name"] != DBNull.Value)
                            {
                                pairTO.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                            }
                            if (row["loc_name"] != DBNull.Value)
                            {
                                pairTO.LocationName = row["loc_name"].ToString().Trim();
                            }
                            if (row["pt_description"] != DBNull.Value)
                            {
                                pairTO.PassType = row["pt_description"].ToString().Trim();
                            }
                            if (row["wu_name"] != DBNull.Value)
                            {
                                pairTO.WUName = row["wu_name"].ToString().Trim();
                            }

                            pairsList.Add(pairTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsForEmplDateWithOpenPairs(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID, int isWrkHrs)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description, wu.name wu_name ");
                    sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" AND iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" AND iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(")");
                    }
                    if (locationID != -1)
                    {
                        sb.Append("AND iop.location_id = '" + locationID.ToString() + "'");
                    }
                    if (isWrkHrs != -1)
                    {
                        sb.Append("AND iop.is_wrk_hrs_counter = '" + isWrkHrs.ToString() + "'");
                    }
                    sb.Append("AND iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id");
                    select = sb.ToString();

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pairTO = new IOPairTO();
                            pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                            if (row["io_pair_date"] != DBNull.Value)
                            {
                                pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                            }
                            if (row["employee_id"] != DBNull.Value)
                            {
                                pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (row["location_id"] != DBNull.Value)
                            {
                                pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                            }
                            if (row["is_wrk_hrs_counter"] != DBNull.Value)
                            {
                                pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                            }
                            if (row["pass_type_id"] != DBNull.Value)
                            {
                                pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                            }
                            if (row["start_time"] != DBNull.Value)
                            {
                                pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                            }
                            if (row["end_time"] != DBNull.Value)
                            {
                                pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                            }
                            if (row["manual_created"] != DBNull.Value)
                            {
                                pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                            }
                            if (row["empl_name"] != DBNull.Value)
                            {
                                pairTO.EmployeeName = row["empl_name"].ToString().Trim();
                            }
                            if (row["empl_last_name"] != DBNull.Value)
                            {
                                pairTO.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                            }
                            if (row["loc_name"] != DBNull.Value)
                            {
                                pairTO.LocationName = row["loc_name"].ToString().Trim();
                            }
                            if (row["pt_description"] != DBNull.Value)
                            {
                                pairTO.PassType = row["pt_description"].ToString().Trim();
                            }
                            if (row["wu_name"] != DBNull.Value)
                            {
                                pairTO.WUName = row["wu_name"].ToString().Trim();
                            }

                            pairsList.Add(pairTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsAllEmplDatePairs(string employeeIDString, List<DateTime> datesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT io_pair_id, io_pair_date, employee_id, start_time, end_time, pass_type_id");
                sb.Append(" FROM io_pairs WHERE");
                sb.Append(" is_wrk_hrs_counter = 1");
                sb.Append(" AND pass_type_id NOT IN (SELECT pass_type_id FROM pass_types WHERE pass_type = " + (int)Constants.PassType.WholeDayAbsences + ")");

                if (employeeIDString != "")
                {
                    sb.Append(" AND employee_id IN (" + employeeIDString + ")");
                }

                if (datesList.Count > 0)
                {
                    sb.Append(" AND io_pair_date IN (");
                    foreach (DateTime currentDate in datesList)
                    {
                        sb.Append("convert('" + currentDate.ToString("yyyy-MM-dd") + "', datetime), ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")");
                }
                sb.Append(" ORDER BY employee_id, io_pair_date, start_time");
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsAllForEmpl(string employeeIDString, List<DateTime> datesList, int locationID, int isWrkHrs)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*,loc.name loc_name, wu.name wu_name");
                sb.Append(" FROM io_pairs ip, locations loc, employees empl, working_units wu WHERE");

                if (employeeIDString != "")
                {
                    sb.Append(" ip.employee_id IN (" + employeeIDString + ")");
                }
                if (locationID != -1)
                {
                    sb.Append("AND ip.location_id =" + locationID.ToString() + "");
                }
                if (isWrkHrs != -1)
                {
                    sb.Append("AND ip.is_wrk_hrs_counter =" + isWrkHrs.ToString() + "");
                }
                if (datesList.Count > 0)
                {
                    sb.Append(" AND ip.io_pair_date IN (");
                    foreach (DateTime currentDate in datesList)
                    {
                        sb.Append("convert('" + currentDate.ToString("yyyy-MM-dd") + "', datetime), ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")");
                }
                sb.Append("AND ip.employee_id = empl.employee_id AND empl.working_unit_id = wu.working_unit_id AND loc.location_id = ip.location_id");
                sb.Append(" ORDER BY ip.employee_id, ip.io_pair_date, ip.start_time");
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pairTO.WUName = row["wu_name"].ToString().Trim();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsEmplTimeInterval(string employeeIDString, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT io_pair_id, io_pair_date, employee_id, start_time, end_time, pass_type_id");
                sb.Append(" FROM io_pairs WHERE");
                sb.Append(" is_wrk_hrs_counter = 1");
                sb.Append(" AND pass_type_id NOT IN (SELECT pass_type_id FROM pass_types WHERE pass_type = " + (int)Constants.PassType.WholeDayAbsences + ")");

                if (employeeIDString != "")
                {
                    sb.Append(" AND employee_id IN (" + employeeIDString + ")");
                }

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" AND io_pair_date >= convert('" + fromDate.ToString("yyyy-MM-dd") + "', datetime)");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" AND io_pair_date < convert('" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', datetime)");
                }

                sb.Append(" ORDER BY employee_id, io_pair_date, start_time");
                select = sb.ToString();

                MySqlCommand cmd;
                if (trans != null)
                {
                    cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                }
                else
                {
                    cmd = new MySqlCommand(select, conn);
                }
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }
        //Implementirano je pravilno samo za MSSQL bazu.
        public List<IOPairTO> getIOPairsEmplTimeInterval2(string employeeIDString, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT io_pair_id, io_pair_date, employee_id, start_time, end_time, pass_type_id");
                sb.Append(" FROM io_pairs WHERE");
                sb.Append(" is_wrk_hrs_counter = 1");
                sb.Append(" AND pass_type_id NOT IN (SELECT pass_type_id FROM pass_types WHERE pass_type = " + (int)Constants.PassType.WholeDayAbsences + ")");

                if (employeeIDString != "")
                {
                    sb.Append(" AND employee_id IN (" + employeeIDString + ")");
                }

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" AND io_pair_date >= convert('" + fromDate.ToString("yyyy-MM-dd") + "', datetime)");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" AND io_pair_date < convert('" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', datetime)");
                }

                sb.Append(" ORDER BY employee_id, io_pair_date, start_time");
                select = sb.ToString();

                MySqlCommand cmd;
                if (trans != null)
                {
                    cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                }
                else
                {
                    cmd = new MySqlCommand(select, conn);
                }
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }


        public int getIOPairsForLocCount(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) count");
                sb.Append(" FROM io_pairs iop, employees empl, locations loc, pass_types pt WHERE");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (locationID != -1)
                {
                    sb.Append(" iop.location_id = '" + locationID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" iop.location_id IN (" + locations + ") AND");
                }
                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id ");

                sb.Append("AND empl.working_unit_id IN (" + wUnits + ") ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                //RptDataSet rptDataSet = new RptDataSet();
                //sqlDataAdapter.Fill(rptDataSet);
                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public List<IOPairTO> getIOPairsForLoc(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description");
                sb.Append(" FROM io_pairs iop, employees empl, locations loc, pass_types pt WHERE");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (locationID != -1)
                {
                    sb.Append(" iop.location_id = '" + locationID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" iop.location_id IN (" + locations + ") AND");
                }
                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id ");

                sb.Append("AND empl.working_unit_id IN (" + wUnits + ") ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString() + "ORDER BY loc.name, empl.last_name, empl.first_name, iop.io_pair_date ", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                //RptDataSet rptDataSet = new RptDataSet();
                //sqlDataAdapter.Fill(rptDataSet);
                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["empl_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeName = row["empl_name"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["pt_description"] != DBNull.Value)
                        {
                            pairTO.PassType = row["pt_description"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString().Trim();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public DataSet getIOPairsForLocDataSet(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description");
                sb.Append(" FROM io_pairs iop, employees empl, locations loc, pass_types pt WHERE");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (locationID != -1)
                {
                    sb.Append(" iop.location_id = '" + locationID.ToString().Trim() + "' AND");
                }
                else
                {
                    sb.Append(" iop.location_id IN (" + locations + ") AND");
                }
                sb.Append(" iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id ");

                sb.Append("AND empl.working_unit_id IN (" + wUnits + ") ");

                //MySqlCommand cmd = new MySqlCommand(sb.ToString() + "ORDER BY iop.location_id, empl.last_name ", conn);
                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                //sqlDataAdapter.Fill(dataSet);
                sqlDataAdapter.Fill(dataSet, "IOPairs");
                //DataTable table = dataSet.Tables["IOPairs"];
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return dataSet;
        }

        /// <summary>
        /// Used for maintenance screen
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="workingUnitID"></param>
        /// <param name="employeeID"></param>
        /// <param name="locationID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        // Same as method getIOPairs(DateTime startDate, DateTime endDate, int workingUnitID, int employeeID, int locationID, DateTime fromDate, DateTime toDate, string wUnits, IDBTransaction trans)
        // command is made without transaction	
        public List<IOPairTO> getIOPairs(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int workingUnitID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pair = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            CultureInfo ci = CultureInfo.InvariantCulture;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, ");
                sb.Append("loc.name loc_name, wu.working_unit_id working_unit_id, wu.name wu_name, pt.description ptdesc ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, working_units wu, pass_types pt WHERE ");

                // COMPLETE
                if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))))
                {
                    sb.Append("(iop.start_time IS NOT NULL AND  ");
                    sb.Append("iop.end_time IS NOT NULL) AND ");
                }
                // INCOMPLETE
                else if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))))
                {
                    sb.Append("(iop.start_time IS NULL OR  ");
                    sb.Append("iop.end_time IS NULL) AND ");
                }
                //ALL
                else
                {

                }

                if (workingUnitID != -1)
                {
                    sb.Append("wu.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND ");
                }
                if (pairTO.EmployeeID != -1)
                {
                    sb.Append("empl.employee_id = '" + pairTO.EmployeeID.ToString().Trim() + "' AND ");
                }

                ///
                if (!fromDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,'%Y-%m-%dT%h:%m:%s') >= '" + fromDate.ToString(dateTimeformat,ci) + "' AND ");
                    sb.Append("iop.io_pair_date >= convert('" + fromDate.ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (!toDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,'%Y-%m-%dT%h:%m:%s') < '" + toDate.AddDays(1).ToString(dateTimeformat,ci) + "' AND ");
                    sb.Append("iop.io_pair_date < convert('" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (pairTO.LocationID != -1)
                {
                    sb.Append("iop.location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND ");
                }

                sb.Append("iop.employee_id = empl.employee_id AND ");
                sb.Append("iop.location_id = loc.location_id AND ");
                sb.Append("empl.working_unit_id = wu.working_unit_id AND ");
                sb.Append("iop.pass_type_id = pt.pass_type_id ");
                if (!wUnits.Equals(""))
                {
                    sb.Append("AND empl.working_unit_id IN (" + wUnits + ") ");
                }

                MySqlCommand cmd = new MySqlCommand(sb.ToString() + "ORDER BY empl.last_name ", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairTO();
                        pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pair.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pair.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pair.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pair.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["empl_name"] != DBNull.Value)
                        {
                            pair.EmployeeName = row["empl_name"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            pair.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pair.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pair.WUName = row["wu_name"].ToString().Trim();
                        }
                        if (row["ptdesc"] != DBNull.Value)
                        {
                            pair.PassType = row["ptdesc"].ToString().Trim();
                        }

                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        // Same as method getIOPairs(DateTime startDate, DateTime endDate, int workingUnitID, int employeeID, int locationID, DateTime fromDate, DateTime toDate, string wUnits)
        // command is made with transaction	
        public List<IOPairTO> getIOPairs(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int workingUnitID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pair = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            CultureInfo ci = CultureInfo.InvariantCulture;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, ");
                sb.Append("loc.name loc_name, wu.working_unit_id working_unit_id, wu.name wu_name, pt.description ptdesc ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, working_units wu, pass_types pt WHERE ");

                // COMPLETE
                if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))))
                {
                    sb.Append("(iop.start_time IS NOT NULL AND  ");
                    sb.Append("iop.end_time IS NOT NULL) AND ");
                }
                // INCOMPLETE
                else if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))))
                {
                    sb.Append("(iop.start_time IS NULL OR  ");
                    sb.Append("iop.end_time IS NULL) AND ");
                }
                //ALL
                else
                {

                }

                if (workingUnitID != -1)
                {
                    sb.Append("wu.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND ");
                }
                if (pairTO.EmployeeID != -1)
                {
                    sb.Append("empl.employee_id = '" + pairTO.EmployeeID.ToString().Trim() + "' AND ");
                }

                ///
                if (!fromDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,'%Y-%m-%dT%h:%m:%s') >= '" + fromDate.ToString(dateTimeformat,ci) + "' AND ");
                    sb.Append("iop.io_pair_date >= convert('" + fromDate.ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (!toDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,'%Y-%m-%dT%h:%m:%s') < '" + toDate.AddDays(1).ToString(dateTimeformat,ci) + "' AND ");
                    sb.Append("iop.io_pair_date < convert('" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (pairTO.LocationID != -1)
                {
                    sb.Append("iop.location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND ");
                }

                sb.Append("iop.employee_id = empl.employee_id AND ");
                sb.Append("iop.location_id = loc.location_id AND ");
                sb.Append("empl.working_unit_id = wu.working_unit_id AND ");
                sb.Append("iop.pass_type_id = pt.pass_type_id ");
                if (!wUnits.Equals(""))
                {
                    sb.Append("AND empl.working_unit_id IN (" + wUnits + ") ");
                }

                MySqlCommand cmd = new MySqlCommand(sb.ToString() + "ORDER BY empl.last_name ", conn, (MySqlTransaction)trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairTO();
                        pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pair.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pair.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pair.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pair.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["empl_name"] != DBNull.Value)
                        {
                            pair.EmployeeName = row["empl_name"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            pair.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pair.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pair.WUName = row["wu_name"].ToString().Trim();
                        }
                        if (row["ptdesc"] != DBNull.Value)
                        {
                            pair.PassType = row["ptdesc"].ToString().Trim();
                        }

                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public int getPairsCount(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int workingUnitID)
        {
            DataSet dataSet = new DataSet();
            int count = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(iop.io_pair_id) count_pair ");
                sb.Append("FROM io_pairs iop, employees empl WHERE ");

                // COMPLETE
                if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))))
                {
                    sb.Append("(iop.start_time IS NOT NULL AND ");
                    sb.Append("iop.end_time IS NOT NULL) AND ");
                }
                // INCOMPLETE
                else if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))))
                {
                    sb.Append("(iop.start_time IS NULL OR ");
                    sb.Append("iop.end_time IS NULL) AND ");
                }
                //ALL
                else
                {

                }

                if (workingUnitID != -1)
                {
                    sb.Append("empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND ");
                }
                if (pairTO.EmployeeID != -1)
                {
                    sb.Append("empl.employee_id = '" + pairTO.EmployeeID.ToString().Trim() + "' AND ");
                }

                ///
                if (!fromDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,GET_FORMAT(DATE,'ISO')) >= '" + fromDate.ToString(dateTimeformat) + "' AND ");
                    sb.Append("iop.io_pair_date >= convert('" + fromDate.ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (!toDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,GET_FORMAT(DATE,'ISO')) < '" + toDate.AddDays(1).ToString(dateTimeformat) + "' AND ");
                    sb.Append("iop.io_pair_date < convert('" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (pairTO.LocationID != -1)
                {
                    sb.Append("iop.location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND ");
                }

                sb.Append("iop.employee_id = empl.employee_id AND ");
                sb.Append("empl.working_unit_id IN (" + wUnits + ") ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_pair"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public int getIOPairsCount(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int workingUnitID)
        {
            DataSet dataSet = new DataSet();
            int count = 0;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(iop.io_pair_id) count_pair ");
                sb.Append("FROM io_pairs iop, employees empl WHERE ");

                if (pairTO.PassTypeID != -1)
                {
                    sb.Append("iop.pass_type_id = " + pairTO.PassTypeID.ToString().Trim() + " AND ");
                }

                // COMPLETE
                if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))))
                {
                    sb.Append("(iop.start_time IS NOT NULL AND ");
                    sb.Append("iop.end_time IS NOT NULL) AND ");
                }
                // INCOMPLETE
                else if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))))
                {
                    sb.Append("(iop.start_time IS NULL OR ");
                    sb.Append("iop.end_time IS NULL) AND ");
                }
                //ALL
                else
                {

                }

                if (workingUnitID != -1)
                {
                    sb.Append("empl.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND ");
                }
                if (pairTO.EmployeeID != -1)
                {
                    sb.Append("empl.employee_id = '" + pairTO.EmployeeID.ToString().Trim() + "' AND ");
                }

                ///
                if (!fromDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,GET_FORMAT(DATE,'ISO')) >= '" + fromDate.ToString(dateTimeformat) + "' AND ");
                    sb.Append("iop.io_pair_date >= convert('" + fromDate.ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (!toDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,GET_FORMAT(DATE,'ISO')) < '" + toDate.AddDays(1).ToString(dateTimeformat) + "' AND ");
                    sb.Append("iop.io_pair_date < convert('" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (pairTO.LocationID != -1)
                {
                    sb.Append("iop.location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND ");
                }

                sb.Append("iop.employee_id = empl.employee_id AND ");
                sb.Append("empl.working_unit_id IN (" + wUnits + ") ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_pair"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public int getIOPairsForEmplCount(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) count FROM io_pairs iop, locations loc WHERE");
                    sb.Append(" iop.location_id = loc.location_id AND");
                    sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (locationID != -1)
                    {
                        sb.Append(" iop.location_id = '" + locationID.ToString().Trim() + "' AND");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }

                    select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public int getIOPairsForEmplLocCount(DateTime fromDate, DateTime toDate, List<int> employeesList, string locationsID)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) count FROM io_pairs iop, locations loc WHERE");
                sb.Append(" iop.location_id = loc.location_id AND");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!locationsID.Equals(""))
                {
                    sb.Append(" iop.location_id IN (" + locationsID + ") AND");
                }
                if (employeesList.Count > 0)
                {
                    sb.Append(" iop.employee_id IN (");
                    foreach (int employeeID in employeesList)
                    {
                        sb.Append(employeeID + ", ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(") AND");
                }

                select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public List<IOPairTO> getIOPairsForEmpl(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT iop.*, loc.name loc_name FROM io_pairs iop, locations loc WHERE");
                    sb.Append(" iop.location_id = loc.location_id AND");
                    sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (locationID != -1)
                    {
                        sb.Append(" iop.location_id = '" + locationID.ToString().Trim() + "' AND");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }

                    select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                    MySqlCommand cmd = new MySqlCommand(select + "ORDER BY iop.employee_id, iop.io_pair_date, loc.name ", conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pairTO = new IOPairTO();
                            pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                            if (row["io_pair_date"] != DBNull.Value)
                            {
                                pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                            }
                            if (row["employee_id"] != DBNull.Value)
                            {
                                pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (row["location_id"] != DBNull.Value)
                            {
                                pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                            }
                            if (row["is_wrk_hrs_counter"] != DBNull.Value)
                            {
                                pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                            }
                            if (row["pass_type_id"] != DBNull.Value)
                            {
                                pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                            }
                            if (row["start_time"] != DBNull.Value)
                            {
                                pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                            }
                            if (row["end_time"] != DBNull.Value)
                            {
                                pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                            }
                            if (row["manual_created"] != DBNull.Value)
                            {
                                pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                            }

                            if (row["loc_name"] != DBNull.Value)
                            {
                                pairTO.LocationName = row["loc_name"].ToString();
                            }

                            pairsList.Add(pairTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsForEmployees(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT iop.*, loc.name loc_name FROM io_pairs iop, locations loc WHERE");
                    sb.Append(" iop.location_id = loc.location_id AND");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (locationID != -1)
                    {
                        sb.Append(" iop.location_id = '" + locationID.ToString().Trim() + "' AND");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }

                    select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                    MySqlCommand cmd = new MySqlCommand(select + "ORDER BY iop.employee_id, iop.io_pair_date, loc.name ", conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pairTO = new IOPairTO();
                            pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                            if (row["io_pair_date"] != DBNull.Value)
                            {
                                pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                            }
                            if (row["employee_id"] != DBNull.Value)
                            {
                                pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (row["location_id"] != DBNull.Value)
                            {
                                pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                            }
                            if (row["is_wrk_hrs_counter"] != DBNull.Value)
                            {
                                pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                            }
                            if (row["pass_type_id"] != DBNull.Value)
                            {
                                pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                            }
                            if (row["start_time"] != DBNull.Value)
                            {
                                pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                            }
                            if (row["end_time"] != DBNull.Value)
                            {
                                pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                            }
                            if (row["manual_created"] != DBNull.Value)
                            {
                                pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                            }

                            if (row["loc_name"] != DBNull.Value)
                            {
                                pairTO.LocationName = row["loc_name"].ToString();
                            }

                            pairsList.Add(pairTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsForEmplLoc(DateTime fromDate, DateTime toDate, List<int> employeesList, string locationsID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, loc.name loc_name FROM io_pairs iop, locations loc WHERE");
                sb.Append(" iop.location_id = loc.location_id AND");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!locationsID.Equals(""))
                {
                    sb.Append(" iop.location_id IN (" + locationsID + ") AND");
                }
                if (employeesList.Count > 0)
                {
                    sb.Append(" iop.employee_id IN (");
                    foreach (int employeeID in employeesList)
                    {
                        sb.Append(employeeID + ", ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(") AND");
                }

                select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                MySqlCommand cmd = new MySqlCommand(select + "ORDER BY iop.employee_id, iop.io_pair_date, loc.name ", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }


        public int getIOPairsForEmplWrkHrsCount(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) count FROM io_pairs iop, locations loc WHERE");
                sb.Append(" is_wrk_hrs_counter = 1 AND");
                sb.Append(" iop.location_id = loc.location_id AND");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (locationID != -1)
                {
                    sb.Append(" iop.location_id = '" + locationID.ToString().Trim() + "' AND");
                }
                if (employeesList.Count > 0)
                {
                    sb.Append(" iop.employee_id IN (");
                    foreach (int employeeID in employeesList)
                    {
                        sb.Append(employeeID + ", ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(") AND");
                }

                select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public List<IOPairTO> getIOPairsForEmplWrkHrs(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, loc.name loc_name FROM io_pairs iop, locations loc WHERE");
                sb.Append(" is_wrk_hrs_counter = 1 AND");
                sb.Append(" iop.location_id = loc.location_id AND");
                sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (locationID != -1)
                {
                    sb.Append(" iop.location_id = '" + locationID.ToString().Trim() + "' AND");
                }
                if (employeesList.Count > 0)
                {
                    sb.Append(" iop.employee_id IN (");
                    foreach (int employeeID in employeesList)
                    {
                        sb.Append(employeeID + ", ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(") AND");
                }

                select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                MySqlCommand cmd = new MySqlCommand(select + "ORDER BY iop.employee_id, iop.io_pair_date, loc.name ", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<DateTime> getDatesWithOpenPairs(DateTime fromDate, DateTime toDate, int employeeID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<DateTime> datesList = new List<DateTime>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT io_pair_date FROM io_pairs WHERE");
                sb.Append(" (start_time IS NULL OR end_time IS NULL) AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (employeeID != -1)
                {
                    sb.Append(" employee_id = " + employeeID.ToString().Trim() + " AND");
                }

                select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        datesList.Add(DateTime.Parse(row["io_pair_date"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return datesList;
        }

        public List<DateTime> getDatesWithOpenPairsWrkHrs(DateTime fromDate, DateTime toDate, int employeeID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<DateTime> datesList = new List<DateTime>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT io_pair_date FROM io_pairs WHERE");
                sb.Append(" is_wrk_hrs_counter = 1 AND");
                sb.Append(" (start_time IS NULL OR end_time IS NULL) AND");

                if (!fromDate.Equals(new DateTime()))
                {
                    sb.Append(" io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (!toDate.Equals(new DateTime()))
                {
                    sb.Append(" io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                }
                if (employeeID != -1)
                {
                    sb.Append(" employee_id = " + employeeID.ToString().Trim() + " AND");
                }

                select = sb.ToString().Substring(0, sb.ToString().Length - 3);
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        datesList.Add(DateTime.Parse(row["io_pair_date"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return datesList;
        }

        public List<IOPairTO> getIOPairsForPresence(int wuID, string wUnitsString, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*, empl.first_name first_name, empl.last_name last_name, wu.name wu_name, pt.description pass_type_desc ,loc.name loc_name ");
                sb.Append("FROM io_pairs ip, employees empl, working_units wu, pass_types pt, locations loc ");
                sb.Append("WHERE ");
                sb.Append("ip.employee_id = empl.employee_id AND ");
                sb.Append("empl.working_unit_id = wu.working_unit_id AND ");
                sb.Append("ip.pass_type_id = pt.pass_type_id AND ");
                sb.Append("ip.location_id = loc.location_id AND ");

                if (wuID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + wuID.ToString().Trim() + "' AND ");
                }
                else
                {
                    sb.Append(" empl.working_unit_id IN (" + wUnitsString + ") AND ");
                }

                if (!from.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date >= '" + from.ToString(dateTimeformat) + "' AND ");
                }
                if (!to.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date < '" + to.ToString(dateTimeformat) + "' AND ");
                }

                sb.Append("ip.start_time IS NOT NULL AND ");
                sb.Append("ip.end_time IS NOT NULL ");
                sb.Append("ORDER BY last_name, first_name ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pass_type_desc"] != DBNull.Value)
                        {
                            pairTO.PassType = row["pass_type_desc"].ToString().Trim();
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeLastName = row["last_name"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pairTO.WUName = row["wu_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString().Trim();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsForOUPresence(int wuID, string wUnitsString, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*, empl.first_name first_name, empl.last_name last_name, wu.name wu_name, pt.description pass_type_desc ,loc.name loc_name ");
                sb.Append("FROM io_pairs ip, employees empl, working_units wu, pass_types pt, locations loc ");
                sb.Append("WHERE ");
                sb.Append("ip.employee_id = empl.employee_id AND ");
                sb.Append("empl.working_unit_id = wu.working_unit_id AND ");
                sb.Append("ip.pass_type_id = pt.pass_type_id AND ");
                sb.Append("ip.location_id = loc.location_id AND ");

                if (wuID != -1)
                {
                    sb.Append(" empl.working_unit_id = '" + wuID.ToString().Trim() + "' AND ");
                }
                else
                {
                    sb.Append(" empl.working_unit_id IN (" + wUnitsString + ") AND ");
                }

                if (!from.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date >= '" + from.ToString(dateTimeformat) + "' AND ");
                }
                if (!to.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date < '" + to.ToString(dateTimeformat) + "' AND ");
                }

                sb.Append("ip.start_time IS NOT NULL AND ");
                sb.Append("ip.end_time IS NOT NULL ");
                sb.Append("ORDER BY last_name, first_name ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["pass_type_desc"] != DBNull.Value)
                        {
                            pairTO.PassType = row["pass_type_desc"].ToString().Trim();
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            pairTO.EmployeeLastName = row["last_name"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pairTO.WUName = row["wu_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString().Trim();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }
        public List<IOPairTO> getIOPairsForVisit(int visitID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.io_pair_id, iop.start_time, iop.end_time, loc.name loc_name");
                sb.Append(" FROM io_pairs iop, locations loc, visits");
                sb.Append(" WHERE iop.location_id = loc.location_id");
                sb.Append(" AND iop.employee_id = visits.employee_id");

                if (visitID != -1)
                {
                    sb.Append(" AND visits.visit_id = '" + visitID.ToString().Trim() + "'");
                }

                sb.Append(" AND");
                sb.Append(" (");
                sb.Append(" iop.start_time >= visits.date_start");
                sb.Append(" AND");
                sb.Append(" (");
                sb.Append(" iop.end_time <= visits.date_end");
                sb.Append(" OR visits.date_end IS NULL");
                sb.Append(" )");
                sb.Append(" )");

                sb.Append(" ORDER BY iop.io_pair_date, loc.name");

                select = sb.ToString();
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pairTO.LocationName = row["loc_name"].ToString();
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public int getDistinctEmployeesCount(DateTime from, DateTime to, int workingUnitID, string wUnits)
        {
            int count = 0;
            DataSet dataset = new DataSet();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT ip.employee_id employee_id, empl.first_name first_name, empl.last_name last_name, empl.working_unit_id working_unit ");
                sb.Append("FROM io_pairs ip, employees empl ");
                sb.Append("WHERE empl.employee_id = ip.employee_id AND ");

                if (!from.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date >= '" + from.ToString(dateTimeformat) + "' AND ");
                }
                if (!to.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date <= '" + to.ToString(dateTimeformat) + "' AND ");
                }
                if (workingUnitID != -1)
                {
                    sb.Append("empl.working_unit_id = " + workingUnitID + " AND ");
                }
                else
                {
                    sb.Append("empl.working_unit_id IN (" + wUnits + ") AND ");
                }


                sb.Append("ip.start_time IS NOT NULL AND ");
                sb.Append("ip.end_time IS NOT NULL ");
                sb.Append("GROUP BY ip.employee_id, empl.first_name, empl.last_name, empl.working_unit_id ");

                string select = "SELECT COUNT(*) count FROM (" + sb.ToString().Trim() + ") AS qry";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataset, "employees");
                DataTable dataTable = dataset.Tables["employees"];

                if (dataTable.Rows.Count > 0)
                {
                    count = Int32.Parse(dataTable.Rows[0]["count"].ToString().Trim());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }

        /// <summary>
        /// Return distinct employees who have worked in a given period of time.
        /// </summary>
        /// <param name="from">Start date</param>
        /// <param name="to">End date</param>
        /// <returns>ArrayList of EmployeeTO objects</returns>
        public List<EmployeeTO> getDistinctEmployees(DateTime from, DateTime to, int workingUnitID, string wUnits)
        {
            List<EmployeeTO> employeesTOList = new List<EmployeeTO>();
            DataSet dataset = new DataSet();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT ip.employee_id employee_id, empl.first_name first_name, empl.last_name last_name, empl.working_unit_id working_unit, empl.status empl_status, wu.name wu_name  ");
                sb.Append("FROM io_pairs ip, employees empl, working_units wu ");
                sb.Append("WHERE empl.employee_id = ip.employee_id AND ");
                sb.Append("wu.working_unit_id = empl.working_unit_id AND ");

                if (!from.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date >= '" + from.ToString(dateTimeformat) + "' AND ");
                }
                if (!to.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date <= '" + to.ToString(dateTimeformat) + "' AND ");
                }
                if (workingUnitID != -1)
                {
                    sb.Append("empl.working_unit_id = " + workingUnitID + " AND ");
                }
                else
                {
                    sb.Append("empl.working_unit_id IN (" + wUnits + ") AND ");
                }

                sb.Append("ip.start_time IS NOT NULL AND ");
                sb.Append("ip.end_time IS NOT NULL ");
                sb.Append("GROUP BY ip.employee_id, empl.first_name, empl.last_name, empl.working_unit_id, wu.name, empl.status ");
                sb.Append("ORDER BY  empl.last_name, empl.first_name ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataset, "employees");
                DataTable dataTable = dataset.Tables["employees"];

                EmployeeTO employeeTO = new EmployeeTO();

                foreach (DataRow row in dataTable.Rows)
                {
                    employeeTO = new EmployeeTO();

                    employeeTO.EmployeeID = Int32.Parse(row["employee_id"].ToString());
                    employeeTO.FirstName = row["first_name"].ToString();
                    employeeTO.LastName = row["last_name"].ToString();
                    employeeTO.WorkingUnitID = (int)row["working_unit"];
                    employeeTO.WorkingUnitName = row["wu_name"].ToString();
                    employeeTO.Status = row["empl_status"].ToString();
                    employeesTOList.Add(employeeTO);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return employeesTOList;
        }

        public List<EmployeeTO> getDistinctOUEmployees(DateTime from, DateTime to, int workingUnitID, string wUnits)
        {
            List<EmployeeTO> employeesTOList = new List<EmployeeTO>();
            DataSet dataset = new DataSet();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT ip.employee_id employee_id, empl.first_name first_name, empl.last_name last_name, empl.working_unit_id working_unit, empl.status empl_status, wu.name wu_name  ");
                sb.Append("FROM io_pairs ip, employees empl, working_units wu ");
                sb.Append("WHERE empl.employee_id = ip.employee_id AND ");
                sb.Append("wu.working_unit_id = empl.working_unit_id AND ");

                if (!from.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date >= '" + from.ToString(dateTimeformat) + "' AND ");
                }
                if (!to.Equals(new DateTime()))
                {
                    sb.Append("io_pair_date <= '" + to.ToString(dateTimeformat) + "' AND ");
                }
                if (workingUnitID != -1)
                {
                    sb.Append("empl.working_unit_id = " + workingUnitID + " AND ");
                }
                else
                {
                    sb.Append("empl.working_unit_id IN (" + wUnits + ") AND ");
                }

                sb.Append("ip.start_time IS NOT NULL AND ");
                sb.Append("ip.end_time IS NOT NULL ");
                sb.Append("GROUP BY ip.employee_id, empl.first_name, empl.last_name, empl.working_unit_id, wu.name, empl.status ");
                sb.Append("ORDER BY  empl.last_name, empl.first_name ");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataset, "employees");
                DataTable dataTable = dataset.Tables["employees"];

                EmployeeTO employeeTO = new EmployeeTO();

                foreach (DataRow row in dataTable.Rows)
                {
                    employeeTO = new EmployeeTO();

                    employeeTO.EmployeeID = Int32.Parse(row["employee_id"].ToString());
                    employeeTO.FirstName = row["first_name"].ToString();
                    employeeTO.LastName = row["last_name"].ToString();
                    employeeTO.WorkingUnitID = (int)row["working_unit"];
                    employeeTO.WorkingUnitName = row["wu_name"].ToString();
                    employeeTO.Status = row["empl_status"].ToString();
                    employeesTOList.Add(employeeTO);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return employeesTOList;
        }


        /// <summary>
        /// Returns list of all employees and distinct days they were worked that have opened 
        /// IO Pairs for that day.
        /// </summary>
        /// <returns>Employees</returns>
        public ArrayList getEmpoloyeesByDate(DateTime fromDate)
        {
            ArrayList employeesDate = new ArrayList();

            // list will contain two elements only:
            // first is date and second is employee_id 
            ArrayList member = new ArrayList();
            StringBuilder sb = new StringBuilder();
            DataSet dataSet = new DataSet();

            try
            {
                sb.Append("SELECT DISTINCT io_pair_date, employee_id ");
                sb.Append("FROM io_pairs ");
                sb.Append("WHERE DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) < '" + DateTime.Now.ToString(dateTimeformat) + "' ");
                if (!fromDate.Equals(new DateTime()))
                    sb.Append("AND DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) >= '" + fromDate.ToString(dateTimeformat) + "' ");
                sb.Append("AND (start_time IS NULL OR end_time IS NULL) ");
                sb.Append("ORDER BY io_pair_date, employee_id");
                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter addapter = new MySqlDataAdapter(cmd);
                addapter.Fill(dataSet, "empl");

                DataTable table = dataSet.Tables["empl"];

                foreach (DataRow row in table.Rows)
                {
                    member = new ArrayList();
                    member.Insert(0, DateTime.Parse(row["io_pair_date"].ToString()));
                    member.Insert(1, Int32.Parse(row["employee_id"].ToString()));

                    employeesDate.Add(member);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return employeesDate;
        }

        /// <summary>
        /// Returns list of all employees and distinct days they were worked that have opened 
        /// IO Pairs for today.
        /// </summary>
        /// <returns>Employees</returns>
        public ArrayList getEmpoloyeesOpenPairsToday()
        {
            ArrayList employeesDate = new ArrayList();

            // list will contain two elements only:
            // first is date and second is employee_id 
            ArrayList member = new ArrayList();
            StringBuilder sb = new StringBuilder();
            DataSet dataSet = new DataSet();

            try
            {
                sb.Append("SELECT DISTINCT io_pair_date, employee_id ");
                sb.Append("FROM io_pairs ");
                sb.Append("WHERE DATE_FORMAT(io_pair_date,GET_FORMAT(DATETIME,'ISO')) = '" + DateTime.Now.ToString(dateTimeformat) + "' ");
                sb.Append("AND (start_time IS NULL OR end_time IS NULL) ");
                sb.Append("ORDER BY io_pair_date, employee_id");
                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter addapter = new MySqlDataAdapter(cmd);
                addapter.Fill(dataSet, "empl");

                DataTable table = dataSet.Tables["empl"];

                foreach (DataRow row in table.Rows)
                {
                    member = new ArrayList();
                    member.Insert(0, DateTime.Parse(row["io_pair_date"].ToString()));
                    member.Insert(1, Int32.Parse(row["employee_id"].ToString()));

                    employeesDate.Add(member);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return employeesDate;
        }

        public void getEmplOpenPairs(DateTime fromDate, Dictionary<int, Dictionary<DateTime, IOPairTO>> emplStartOpenPairs,
            Dictionary<int, Dictionary<DateTime, IOPairTO>> emplEndOpenPairs, ref string emplIDs, List<DateTime> dateList)
        {
            StringBuilder sb = new StringBuilder();
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<int> emplList = new List<int>();

            try
            {
                sb.Append("SELECT * FROM io_pairs ");
                sb.Append("WHERE io_pair_date < '" + DateTime.Now.Date.ToString(dateTimeformat) + "' ");
                if (!fromDate.Equals(new DateTime()))
                    sb.Append("AND io_pair_date >= '" + fromDate.Date.ToString(dateTimeformat) + "' ");
                sb.Append("AND (start_time IS NULL OR end_time IS NULL) ");
                sb.Append("AND is_wrk_hrs_counter = '" + Constants.yesInt.ToString().Trim() + "' ");
                sb.Append("AND pair_processed_gen_used = '" + Constants.noInt.ToString().Trim() + "' ");
                sb.Append("ORDER BY employee_id, io_pair_date");
                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter addapter = new MySqlDataAdapter(cmd);
                addapter.Fill(dataSet, "empl");

                DataTable table = dataSet.Tables["empl"];

                foreach (DataRow row in table.Rows)
                {
                    pairTO = new IOPairTO();
                    pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                    if (row["io_pair_date"] != DBNull.Value)
                    {
                        pairTO.IOPairDate = (DateTime)row["io_pair_date"];
                    }
                    if (row["employee_id"] != DBNull.Value)
                    {
                        pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                    }
                    if (row["location_id"] != DBNull.Value)
                    {
                        pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                    }
                    if (row["is_wrk_hrs_counter"] != DBNull.Value)
                    {
                        pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                    }
                    if (row["pass_type_id"] != DBNull.Value)
                    {
                        pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                    }
                    if (row["start_time"] != DBNull.Value)
                    {
                        pairTO.StartTime = (DateTime)row["start_time"];
                    }
                    if (row["end_time"] != DBNull.Value)
                    {
                        pairTO.EndTime = (DateTime)row["end_time"];
                    }
                    if (row["manual_created"] != DBNull.Value)
                    {
                        pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                    }

                    if (pairTO.StartTime.Equals(new DateTime()))
                    {
                        if (!emplList.Contains(pairTO.EmployeeID))
                        {
                            emplList.Add(pairTO.EmployeeID);
                            emplIDs += pairTO.EmployeeID.ToString().Trim() + ",";
                        }

                        if (!dateList.Contains(pairTO.IOPairDate.Date))
                            dateList.Add(pairTO.IOPairDate.Date);

                        if (!emplStartOpenPairs.ContainsKey(pairTO.EmployeeID))
                            emplStartOpenPairs.Add(pairTO.EmployeeID, new Dictionary<DateTime, IOPairTO>());

                        if (!emplStartOpenPairs[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                            emplStartOpenPairs[pairTO.EmployeeID].Add(pairTO.IOPairDate.Date, pairTO);
                        else if (emplStartOpenPairs[pairTO.EmployeeID][pairTO.IOPairDate].EndTime > pairTO.EndTime)
                            emplStartOpenPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date] = pairTO;
                    }

                    if (pairTO.EndTime.Equals(new DateTime()))
                    {
                        if (!emplList.Contains(pairTO.EmployeeID))
                        {
                            emplList.Add(pairTO.EmployeeID);
                            emplIDs += pairTO.EmployeeID.ToString().Trim() + ",";
                        }

                        if (!dateList.Contains(pairTO.IOPairDate.Date))
                            dateList.Add(pairTO.IOPairDate.Date);

                        if (!emplEndOpenPairs.ContainsKey(pairTO.EmployeeID))
                            emplEndOpenPairs.Add(pairTO.EmployeeID, new Dictionary<DateTime, IOPairTO>());

                        if (!emplEndOpenPairs[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                            emplEndOpenPairs[pairTO.EmployeeID].Add(pairTO.IOPairDate.Date, pairTO);
                        else if (emplEndOpenPairs[pairTO.EmployeeID][pairTO.IOPairDate].StartTime < pairTO.StartTime)
                            emplEndOpenPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date] = pairTO;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> getOpenPairs(int employeeID, DateTime date)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs ");
                sb.Append("WHERE employee_id = '" + employeeID.ToString().Trim() + "' ");
                sb.Append("AND DATE_FORMAT(io_pair_date,'%m/%d/%Y') = '" + date.ToString("MM/dd/yyy") + "' ");
                sb.Append("AND (start_time IS NULL OR end_time IS NULL)");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getSpecialOutOpenPairs()
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.* FROM io_pairs iop, pass_types pt ");
                sb.Append("WHERE iop.start_time IS NOT NULL AND iop.end_time IS NULL ");
                sb.Append("AND iop.pass_type_id <> " + (int)Constants.PassType.Work + " AND iop.pass_type_id = pt.pass_type_id ");
                sb.Append("AND pt.pass_type = " + Constants.passOnReader);

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getPairsWithSpecialOut(DateTime start, DateTime end, DateTime date)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs ");
                sb.Append("WHERE start_time IS NOT NULL AND end_time IS NOT NULL ");
                sb.Append("AND DATE_FORMAT(io_pair_date,'%m/%d/%Y') = '" + date.ToString("MM/dd/yyy") + "' ");
                sb.Append("AND ((DATE_FORMAT(start_time,GET_FORMAT(TIME,'ISO')) >= '" + start.ToString("HH:mm:ss") + "' ");
                sb.Append("AND DATE_FORMAT(start_time,GET_FORMAT(TIME,'ISO'))<= '" + end.ToString("HH:mm:ss") + "') ");
                sb.Append("OR (DATE_FORMAT(end_time,GET_FORMAT(TIME,'ISO')) >= '" + start.ToString("HH:mm:ss") + "' ");
                sb.Append("AND DATE_FORMAT(end_time,GET_FORMAT(TIME,'ISO')) <= '" + end.ToString("HH:mm:ss") + "')) ");
                sb.Append("AND DATE_ADD(start_time,interval 1 SECOND) < end_time");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getPermissionPassPairs()
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs WHERE end_time = DATE_ADD(start_time,INTERVAL 1 SECOND)");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pairsList;
        }

        public bool existEmlpoyeeDatePair(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime)
        {
            bool existPair = false;

            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) FROM io_pairs ");

                if (employeeID != -1)
                {
                    sb.Append(" WHERE ");
                    sb.Append(" employee_id = " + employeeID);

                    if (!ioPairDate.Equals(new DateTime()))
                    {
                        sb.Append(" AND io_pair_date = '" + ioPairDate.ToString("yyyy-MM-dd") + "'");

                    }
                    if ((!startTime.Equals(new DateTime())) && (!endTime.Equals(new DateTime())))
                    {
                        sb.Append(" AND (");

                        sb.Append(" (");
                        sb.Append(" DATE_FORMAT(start_time,'%H:%i') <= '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" AND DATE_FORMAT(end_time,'%H:%i') > '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" DATE_FORMAT(start_time,'%H:%i') > '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" AND DATE_FORMAT(start_time,'%H:%i') < '" + endTime.ToString("HH:mm") + "'");
                        sb.Append(" )");

                        sb.Append(")");
                    }
                    /*if (!startTime.Equals(new DateTime()))
                    {
                        sb.Append(" AND DATE_FORMAT(start_time,'%H:%i') = '" + startTime.ToString("HH:mm") + "'");
                    }
                    if (!endTime.Equals(new DateTime()))
                    {
                        sb.Append(" AND DATE_FORMAT(end_time,'%H:%i') = '" + endTime.ToString("HH:mm") + "'");
                    }*/

                    select = sb.ToString();
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExistPair");
                DataTable table = dataSet.Tables["ExistPair"];

                if (table.Rows.Count > 0)
                {
                    if (Int32.Parse(table.Rows[0][0].ToString()) > 0)
                        existPair = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return existPair;
        }

        public bool existEmlpoyeeDatePair(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime, int skipIOPairID, int isWrkHrs)
        {
            bool existPair = false;

            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) FROM io_pairs ");

                if (employeeID != -1)
                {
                    sb.Append(" WHERE ");
                    sb.Append(" employee_id = " + employeeID);

                    if (!ioPairDate.Equals(new DateTime()))
                    {
                        sb.Append(" AND io_pair_date = '" + ioPairDate.ToString("yyyy-MM-dd") + "'");

                    }
                    if (skipIOPairID != -1)
                    {
                        sb.Append(" AND io_pair_id <> '" + skipIOPairID.ToString() + "'");
                    }
                    if (isWrkHrs != -1)
                    {
                        sb.Append(" AND is_wrk_hrs_counter = '" + isWrkHrs.ToString() + "'");
                    }
                    if ((!startTime.Equals(new DateTime())) && (!endTime.Equals(new DateTime())))
                    {
                        sb.Append(" AND (");

                        sb.Append(" (");
                        sb.Append(" DATE_FORMAT(start_time,'%H:%i') <= '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" AND DATE_FORMAT(end_time,'%H:%i') > '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" DATE_FORMAT(start_time,'%H:%i') > '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" AND DATE_FORMAT(start_time,'%H:%i') < '" + endTime.ToString("HH:mm") + "'");
                        sb.Append(" )");

                        sb.Append(")");
                    }
                    /*if (!startTime.Equals(new DateTime()))
                    {
                        sb.Append(" AND DATE_FORMAT(start_time,'%H:%i') = '" + startTime.ToString("HH:mm") + "'");
                    }
                    if (!endTime.Equals(new DateTime()))
                    {
                        sb.Append(" AND DATE_FORMAT(end_time,'%H:%i') = '" + endTime.ToString("HH:mm") + "'");
                    }*/

                    select = sb.ToString();
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExistPair");
                DataTable table = dataSet.Tables["ExistPair"];

                if (table.Rows.Count > 0)
                {
                    if (Int32.Parse(table.Rows[0][0].ToString()) > 0)
                        existPair = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return existPair;
        }

        public bool existEmlpoyeeDatePairNotWholeDayAbsences(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime)
        {
            bool existPair = false;

            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) FROM io_pairs ");
                sb.Append("WHERE pass_type_id IN (SELECT pass_type_id FROM pass_types WHERE pass_type IN (0,1))");

                if (employeeID != -1)
                {
                    sb.Append(" AND employee_id = " + employeeID);

                    if (!ioPairDate.Equals(new DateTime()))
                    {
                        sb.Append(" AND io_pair_date = '" + ioPairDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if ((!startTime.Equals(new DateTime())) && (!endTime.Equals(new DateTime())))
                    {
                        sb.Append(" AND (");

                        sb.Append(" (");
                        sb.Append(" DATE_FORMAT(start_time,'%H:%i') <= '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" AND DATE_FORMAT(end_time,'%H:%i') > '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" )");

                        sb.Append(" OR (");
                        sb.Append(" DATE_FORMAT(start_time,'%H:%i') > '" + startTime.ToString("HH:mm") + "'");
                        sb.Append(" AND DATE_FORMAT(start_time,'%H:%i') < '" + endTime.ToString("HH:mm") + "'");
                        sb.Append(" )");

                        sb.Append(")");
                    }

                    select = sb.ToString();
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "ExistPair");
                DataTable table = dataSet.Tables["ExistPair"];

                if (table.Rows.Count > 0)
                {
                    if (Int32.Parse(table.Rows[0][0].ToString()) > 0)
                        existPair = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return existPair;
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

        public void serialize(List<IOPairTO> ioPairTO)
        {
            //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLIOPairsFile"];
            string filename = Constants.XMLDataSourceDir + Constants.XMLIOPairsFile;

            try
            {
                Stream stream = File.Open(filename, FileMode.Create);
                //bool isOpened = true;
                IOPairTO[] ioPairArray = (IOPairTO[])ioPairTO.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(IOPairTO[]));
                bformatter.Serialize(stream, ioPairArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Serialize all of the employees found in database
        /// </summary>
        public void serialize()
        {
            try
            {
                /*
                // TODO: Not implemented yet
                //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLIOPairsFile"];
                 * string filename = Constants.XMLDataSourceDir + Constants.XMLIOPairsFile;
                Stream stream = File.Open(filename, FileMode.Create);
                ArrayList ioPairTOList = this.getIOPairsForLoc("", "", "", "", "");

                IOPairTO[] ioPairArray = (IOPairTO[]) ioPairTOList.ToArray(typeof(IOPairTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(IOPairTO[]));
                bformatter.Serialize(stream, ioPairArray);
                stream.Close();
                */
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpenPairs()
        {
            DataSet dataSet = new DataSet();

            try
            {
                string select = "SELECT io_pair_id, CAST(DATE_FORMAT(io_pair_date,'%m/%d/%Y') AS CHAR(10)) AS io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, created_by, created_time, modified_by, modified_time FROM io_pairs WHERE start_time IS NULL OR end_time IS NULL";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "OpenPairs");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dataSet;
        }

        public DataSet getClosedPairs()
        {
            DataSet dataSet = new DataSet();

            try
            {
                string select = "SELECT io_pair_id, CAST(DATE_FORMAT(io_pair_date,'%m/%d/%Y') AS CHAR(10)) AS io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, created_by, created_time, modified_by, modified_time FROM io_pairs WHERE start_time IS NOT NULL AND end_time IS NOT NULL AND modified_by = '" + Constants.AutoCloseUser + "'";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "OpenPairs");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dataSet;
        }

        public List<IOPairTO> getOpenPairsFromDataSet(int employeeID, DateTime date, DataSet dsOpenPairs)
        {
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("employee_id = " + employeeID.ToString() + " ");
                sb.Append("AND io_pair_date = '" + date.ToString("MM\\/dd\\/yyy") + "' ");
                string select = sb.ToString();

                DataTable dtOpenPairs = dsOpenPairs.Tables["OpenPairs"];
                DataRow[] rows = dtOpenPairs.Select(select);

                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.ParseExact(row["io_pair_date"].ToString(), "d", CultureInfo.InvariantCulture);
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pairsList;
        }
        public List<IOPairTO> getPairsFromDataSet(int employeeID, DateTime date, DataSet dsOpenPairs)
        {
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("employee_id = " + employeeID.ToString() + " ");
                sb.Append("AND io_pair_date = '" + date.ToString("MM\\/dd\\/yyy") + "' ");
                string select = sb.ToString();

                DataTable dtOpenPairs = dsOpenPairs.Tables["OpenPairs"];
                DataRow[] rows = dtOpenPairs.Select(select);

                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.ParseExact(row["io_pair_date"].ToString().Trim(), "d", CultureInfo.InvariantCulture);
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pairsList;
        }

        /// <summary>
        /// Returns list of all employees and distinct days they were worked
        /// but only for days that do not have pause calculated, no open pairs, 
        /// and there is at least one pair with auto_close
        /// </summary>
        /// <returns>Employees</returns>
        public ArrayList getEmployeesByDateAutoClose()
        {
            ArrayList employeesDate = new ArrayList();

            // list will contain two elements only:
            // first - employee_id and second is date 
            ArrayList member = new ArrayList();
            StringBuilder sb = new StringBuilder();
            DataSet dataSet = new DataSet();

            try
            {
                sb.Append("SELECT DISTINCT employee_id, DATE_FORMAT(io_pair_date,'%Y-%m-%d') AS workday");
                sb.Append(" FROM io_pairs");
                sb.Append(" WHERE");
                sb.Append(" NOT EXISTS (SELECT io_pair_id");
                sb.Append(" FROM io_pairs AS io3");
                sb.Append(" WHERE io3.employee_id = io_pairs.employee_id");
                sb.Append(" AND io3.io_pair_date = io_pairs.io_pair_date");
                sb.Append(" AND io3.is_wrk_hrs_counter = " + (int)Constants.IsWrkCount.IsCounter);
                sb.Append(" AND (io3.pass_type_id = " + Constants.automaticPausePassType + ")");
                sb.Append(" )");
                sb.Append(" AND NOT EXISTS (SELECT io_pair_id");
                sb.Append(" FROM io_pairs AS io1");
                sb.Append(" WHERE io1.employee_id = io_pairs.employee_id");
                sb.Append(" AND io1.io_pair_date = io_pairs.io_pair_date");
                sb.Append(" AND io1.is_wrk_hrs_counter = " + (int)Constants.IsWrkCount.IsCounter);
                sb.Append(" AND (io1.start_time IS NULL OR io1.end_time IS NULL)");
                sb.Append(" )");
                sb.Append(" AND EXISTS (SELECT io_pair_id");
                sb.Append(" FROM io_pairs AS io2");
                sb.Append(" WHERE io2.employee_id = io_pairs.employee_id");
                sb.Append(" AND io2.io_pair_date = io_pairs.io_pair_date");
                sb.Append(" AND io2.is_wrk_hrs_counter = " + (int)Constants.IsWrkCount.IsCounter);
                sb.Append(" AND (io2.modified_by = '" + Constants.AutoCloseUser + "' OR io2.modified_by = '" + Constants.AutoCloseSpecialOutUser + "')");
                sb.Append(" )");
                sb.Append(" ORDER BY employee_id, workday");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter addapter = new MySqlDataAdapter(cmd);

                addapter.Fill(dataSet, "empl");
                DataTable table = dataSet.Tables["empl"];

                foreach (DataRow row in table.Rows)
                {
                    member = new ArrayList();
                    member.Insert(0, Int32.Parse(row["employee_id"].ToString()));
                    //member.Add(DateTime.Parse(row["workday"].ToString(), new CultureInfo("en-US"), DateTimeStyles.NoCurrentDateDefault));
                    // Format of workday is mm/dd/yyy!!!!!
                    member.Insert(1, row["workday"].ToString());

                    employeesDate.Add(member);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return employeesDate;
        }

        public List<IOPairTO> getIOPairsForEmplPerm(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT iop.*, pt.description FROM io_pairs iop, pass_types pt WHERE");
                    sb.Append(" iop.pass_type_id = pt.pass_type_id AND");
                    // sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }

                    select = sb.ToString() + " is_wrk_hrs_counter = 1 ";
                    MySqlCommand cmd = new MySqlCommand(select + "ORDER BY iop.employee_id, iop.io_pair_date, iop.start_time ", conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pairTO = new IOPairTO();
                            pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                            if (row["io_pair_date"] != DBNull.Value)
                            {
                                pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                            }
                            if (row["employee_id"] != DBNull.Value)
                            {
                                pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (row["location_id"] != DBNull.Value)
                            {
                                pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                            }
                            if (row["is_wrk_hrs_counter"] != DBNull.Value)
                            {
                                pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                            }
                            if (row["pass_type_id"] != DBNull.Value)
                            {
                                pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                            }
                            if (row["start_time"] != DBNull.Value)
                            {
                                pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                            }
                            if (row["end_time"] != DBNull.Value)
                            {
                                pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                            }
                            if (row["manual_created"] != DBNull.Value)
                            {
                                pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                            }

                            if (row["description"] != DBNull.Value)
                            {
                                pairTO.PassType = row["description"].ToString();
                            }

                            pairsList.Add(pairTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public int getIOPairsForEmplPermCount(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) count FROM io_pairs iop, locations loc WHERE");
                    sb.Append(" iop.location_id = loc.location_id AND");
                    sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "' AND");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }

                    select = sb.ToString() + " is_wrk_hrs_counter = 1";
                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public int getIOPairsForDaysArrayCount(List<DateTime> days, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) count FROM io_pairs iop WHERE");
                    sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND ");

                    if (days.Count > 0)
                    {
                        sb.Append("iop.io_pair_date IN (");
                        foreach (DateTime date in days)
                        {
                            sb.Append("'" + date.ToString("yyyy-MM-dd") + "', ");
                            sb.Append("'" + date.AddDays(1).ToString("yyyy-MM-dd") + "', ");
                        }
                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }

                    select = sb.ToString() + " is_wrk_hrs_counter = 1";
                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public List<IOPairTO> getIOPairsForDaysArray(List<DateTime> days, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT iop.*, pt.description FROM io_pairs iop, pass_types pt WHERE");
                    sb.Append(" iop.pass_type_id = pt.pass_type_id AND");
                    //sb.Append(" iop.start_time IS NOT NULL AND iop.end_time IS NOT NULL AND ");

                    if (days.Count > 0)
                    {
                        sb.Append(" iop.io_pair_date IN (");
                        foreach (DateTime date in days)
                        {
                            sb.Append("'" + date.ToString("yyyy-MM-dd") + "', ");
                            sb.Append("'" + date.AddDays(1).ToString("yyyy-MM-dd") + "', ");
                        }
                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(") AND");
                    }
                    select = sb.ToString() + " is_wrk_hrs_counter = 1";
                    MySqlCommand cmd = new MySqlCommand(select + "ORDER BY iop.employee_id, iop.io_pair_date, iop.start_time ", conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pairTO = new IOPairTO();
                            pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                            if (row["io_pair_date"] != DBNull.Value)
                            {
                                pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                            }
                            if (row["employee_id"] != DBNull.Value)
                            {
                                pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (row["location_id"] != DBNull.Value)
                            {
                                pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                            }
                            if (row["is_wrk_hrs_counter"] != DBNull.Value)
                            {
                                pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                            }
                            if (row["pass_type_id"] != DBNull.Value)
                            {
                                pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                            }
                            if (row["start_time"] != DBNull.Value)
                            {
                                pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                            }
                            if (row["end_time"] != DBNull.Value)
                            {
                                pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                            }
                            if (row["manual_created"] != DBNull.Value)
                            {
                                pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                            }

                            if (row["description"] != DBNull.Value)
                            {
                                pairTO.PassType = row["description"].ToString();
                            }

                            pairsList.Add(pairTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }
        public int getIOPairsEmployeeDateCount(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            string select;
            int count = 0;
            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) count ");
                    sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" AND iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" AND iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(")");
                    }
                    sb.Append("AND iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id");
                    select = sb.ToString();

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];
                    if (table.Rows.Count > 0)
                    {
                        count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }

        public List<IOPairTO> getIOPairsEmployeeDate(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            string select;

            try
            {
                if (employeesList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, loc.name loc_name, pt.description pt_description, wu.name wu_name ");
                    sb.Append("FROM io_pairs iop, employees empl, locations loc, pass_types pt,working_units wu WHERE");

                    if (!fromDate.Equals(new DateTime()))
                    {
                        sb.Append(" iop.io_pair_date >= '" + fromDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (!toDate.Equals(new DateTime()))
                    {
                        sb.Append(" AND iop.io_pair_date <= '" + toDate.ToString("yyyy-MM-dd") + "'");
                    }
                    if (employeesList.Count > 0)
                    {
                        sb.Append(" AND iop.employee_id IN (");
                        foreach (int employeeID in employeesList)
                        {
                            sb.Append(employeeID + ", ");
                        }

                        sb.Remove(sb.ToString().Length - 2, 2);
                        sb.Append(")");
                    }
                    sb.Append("AND iop.employee_id = empl.employee_id AND iop.location_id = loc.location_id AND iop.pass_type_id = pt.pass_type_id AND empl.working_unit_id = wu.working_unit_id");
                    select = sb.ToString();

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairs");
                    DataTable table = dataSet.Tables["IOPairs"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pairTO = new IOPairTO();
                            pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                            if (row["io_pair_date"] != DBNull.Value)
                            {
                                pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                            }
                            if (row["employee_id"] != DBNull.Value)
                            {
                                pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                            }
                            if (row["location_id"] != DBNull.Value)
                            {
                                pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                            }
                            if (row["is_wrk_hrs_counter"] != DBNull.Value)
                            {
                                pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                            }
                            if (row["pass_type_id"] != DBNull.Value)
                            {
                                pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                            }
                            if (row["start_time"] != DBNull.Value)
                            {
                                pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                            }
                            if (row["end_time"] != DBNull.Value)
                            {
                                pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                            }
                            if (row["manual_created"] != DBNull.Value)
                            {
                                pairTO.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                            }
                            if (row["empl_name"] != DBNull.Value)
                            {
                                pairTO.EmployeeName = row["empl_name"].ToString().Trim();
                            }
                            if (row["empl_last_name"] != DBNull.Value)
                            {
                                pairTO.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                            }
                            if (row["loc_name"] != DBNull.Value)
                            {
                                pairTO.LocationName = row["loc_name"].ToString().Trim();
                            }
                            if (row["pt_description"] != DBNull.Value)
                            {
                                pairTO.PassType = row["pt_description"].ToString().Trim();
                            }
                            if (row["wu_name"] != DBNull.Value)
                            {
                                pairTO.WUName = row["wu_name"].ToString().Trim();
                            }

                            pairsList.Add(pairTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getIOPairsWithType(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int workingUnitID)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pair = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            CultureInfo ci = CultureInfo.InvariantCulture;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name, ");
                sb.Append("loc.name loc_name, wu.working_unit_id working_unit_id, wu.name wu_name, pt.description ptdesc ");
                sb.Append("FROM io_pairs iop, employees empl, locations loc, working_units wu, pass_types pt WHERE ");

                if (pairTO.PassTypeID != -1)
                {
                    sb.Append("iop.pass_type_id = " + pairTO.PassTypeID.ToString().Trim() + " AND ");
                }

                // COMPLETE
                if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0))))
                {
                    sb.Append("(iop.start_time IS NOT NULL AND  ");
                    sb.Append("iop.end_time IS NOT NULL) AND ");
                }
                // INCOMPLETE
                else if ((pairTO.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))) && (pairTO.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 1))))
                {
                    sb.Append("(iop.start_time IS NULL OR  ");
                    sb.Append("iop.end_time IS NULL) AND ");
                }
                //ALL
                else
                {

                }

                if (workingUnitID != -1)
                {
                    sb.Append("wu.working_unit_id = '" + workingUnitID.ToString().Trim() + "' AND ");
                }
                if (pairTO.EmployeeID != -1)
                {
                    sb.Append("empl.employee_id = '" + pairTO.EmployeeID.ToString().Trim() + "' AND ");
                }

                ///
                if (!fromDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,'%Y-%m-%dT%h:%m:%s') >= '" + fromDate.ToString(dateTimeformat,ci) + "' AND ");
                    sb.Append("iop.io_pair_date >= convert('" + fromDate.ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (!toDate.Equals(new DateTime()))
                {
                    //sb.Append("DATE_FORMAT(iop.io_pair_date,'%Y-%m-%dT%h:%m:%s') < '" + toDate.AddDays(1).ToString(dateTimeformat,ci) + "' AND ");
                    sb.Append("iop.io_pair_date < convert('" + toDate.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND ");
                }

                if (pairTO.LocationID != -1)
                {
                    sb.Append("iop.location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND ");
                }

                sb.Append("iop.employee_id = empl.employee_id AND ");
                sb.Append("iop.location_id = loc.location_id AND ");
                sb.Append("empl.working_unit_id = wu.working_unit_id AND ");
                sb.Append("iop.pass_type_id = pt.pass_type_id ");
                if (!wUnits.Equals(""))
                {
                    sb.Append("AND empl.working_unit_id IN (" + wUnits + ") ");
                }

                MySqlCommand cmd = new MySqlCommand(sb.ToString() + "ORDER BY empl.last_name ", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairTO();
                        pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pair.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pair.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["location_id"] != DBNull.Value)
                        {
                            pair.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pair.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["empl_name"] != DBNull.Value)
                        {
                            pair.EmployeeName = row["empl_name"].ToString().Trim();
                        }
                        if (row["empl_last_name"] != DBNull.Value)
                        {
                            pair.EmployeeLastName = row["empl_last_name"].ToString().Trim();
                        }
                        if (row["loc_name"] != DBNull.Value)
                        {
                            pair.LocationName = row["loc_name"].ToString().Trim();
                        }
                        if (row["wu_name"] != DBNull.Value)
                        {
                            pair.WUName = row["wu_name"].ToString().Trim();
                        }
                        if (row["ptdesc"] != DBNull.Value)
                        {
                            pair.PassType = row["ptdesc"].ToString().Trim();
                        }

                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }
        public List<IOPairTO> getNonEnteredIOPairs(int employeeID, DateTime startTime, DateTime endTime)
        {

            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT iop.*, empl.first_name empl_name, empl.last_name empl_last_name ");
                sb.Append("FROM io_pairs iop, employees empl ");
                sb.Append("WHERE (iop.start_time IS NULL OR  iop.end_time IS NULL) ");
                sb.Append("AND iop.io_pair_date >= convert('" + startTime.ToString("yyyy-MM-dd") + "', datetime) ");
                sb.Append("AND iop.io_pair_date < convert('" + endTime.AddDays(1).ToString("yyyy-MM-dd") + "', datetime) AND iop.employee_id = empl.employee_id ");
                sb.Append("AND empl.employee_id =" + employeeID.ToString());

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());

                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString());
                        }

                        if (row["location_id"] != DBNull.Value)
                        {
                            pairTO.LocationID = Int32.Parse(row["location_id"].ToString().Trim());
                        }
                        if (row["is_wrk_hrs_counter"] != DBNull.Value)
                        {
                            pairTO.IsWrkHrsCount = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString());
                        }

                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pairsList;
        }

        public List<IOPairTO> getWholeDayAbsenceIOPairs(List<int> employeesList, List<int> passTypes, int year)
        {
            DataSet dataSet = new DataSet();
            IOPairTO pairTO = new IOPairTO();
            List<IOPairTO> pairsList = new List<IOPairTO>();
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime lastDay = firstDay.AddYears(1).AddDays(-1);
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs WHERE");
                sb.Append(" start_time IS NOT NULL AND end_time IS NOT NULL");
                sb.Append(" AND io_pair_date >= '" + firstDay.ToString("yyyy-MM-dd") + "'");
                sb.Append(" AND io_pair_date <= '" + lastDay.ToString("yyyy-MM-dd") + "'");

                if (employeesList.Count > 0)
                {
                    sb.Append(" AND employee_id IN (");
                    foreach (int employeeID in employeesList)
                    {
                        sb.Append(employeeID + ", ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")");
                }

                if (passTypes.Count > 0)
                {
                    sb.Append(" AND pass_type_id IN (");
                    foreach (int ptID in passTypes)
                    {
                        sb.Append(ptID + ", ");
                    }

                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")");
                }

                sb.Append(" ORDER BY employee_id, io_pair_date, start_time");
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pairTO = new IOPairTO();
                        pairTO.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pairTO.IOPairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                        }
                        if (row["employee_id"] != DBNull.Value)
                        {
                            pairTO.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pairTO.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pairTO.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pairTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        pairsList.Add(pairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsList;
        }

        //  22.05.2019.
        public List<IOPairTO> getOpenPairsForReport(string employeeID, DateTime from, DateTime to, string orgUnitID, string workingUnitID) {
            return null;
        }

        //  23.01.2020. BOJAN
        public List<IOPairTO> getIOPairsForBreaks(DateTime from, DateTime to, string emplIDs, string ptIDs)
        {
            throw new NotImplementedException();
        }
        public void deleteForDay(int emplID, DateTime date)
        {
        }

        public void deleteForAutoTimeSchema(int emplID, DateTime date, bool doCommit)
        { }

    }
}
