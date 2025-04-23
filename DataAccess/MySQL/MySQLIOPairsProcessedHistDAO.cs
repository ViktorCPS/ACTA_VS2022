using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;

using Util;
using TransferObjects;

namespace DataAccess
{
   public class MySQLIOPairsProcessedHistDAO:IOPairsProcessedHistDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MySQLIOPairsProcessedHistDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLIOPairsProcessedHistDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public uint insert(IOPairsProcessedHistTO pairTO, bool doCommit)
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

            uint recID = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO io_pairs_processed_hist ");
                sbInsert.Append("(io_pair_id, io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, confirmation_flag,confirmed_by, confirmation_time, verification_flag, verified_by, verification_time,alert, description, created_by, created_time, modified_by,modified_time) ");
                sbInsert.Append("VALUES (");

                if (pairTO.IOPairID != -1)
                {
                    sbInsert.Append(pairTO.IOPairID.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append("'" + pairTO.IOPairDate.ToString(dateTimeformat) + "', ");

                if (pairTO.EmployeeID != -1)
                {
                    sbInsert.Append(pairTO.EmployeeID.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.LocationID != -1)
                {
                    sbInsert.Append(pairTO.LocationID.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (pairTO.IsWrkHrsCounter != -1)
                {
                    sbInsert.Append(pairTO.IsWrkHrsCounter.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (pairTO.PassTypeID != -1)
                {
                    sbInsert.Append(pairTO.PassTypeID.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.StartTime != new DateTime())
                {
                    sbInsert.Append("'" + pairTO.StartTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.EndTime != new DateTime())
                {
                    sbInsert.Append("'" + pairTO.EndTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.ManualCreated != -1)
                {
                    sbInsert.Append(pairTO.ManualCreated.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.ConfirmationFlag != -1)
                {
                    sbInsert.Append(pairTO.ConfirmationFlag.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.ConfirmedBy.Trim() != "")
                {
                    sbInsert.Append("'" + pairTO.ConfirmedBy.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.ConfirmationTime != new DateTime())
                {
                    sbInsert.Append("'" + pairTO.ConfirmationTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.VerificationFlag != -1)
                {
                    sbInsert.Append(pairTO.VerificationFlag.ToString() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.VerifiedBy.Trim() != "")
                {
                    sbInsert.Append("'" + pairTO.VerifiedBy.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.VerifiedTime != new DateTime())
                {
                    sbInsert.Append("'" + pairTO.VerifiedTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!pairTO.Alert.Trim().Equals(""))
                {
                    sbInsert.Append(pairTO.Alert.Trim() + ", ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!pairTO.Desc.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + pairTO.Desc.Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.CreatedBy.Trim() != "")
                {
                    sbInsert.Append("'" + pairTO.CreatedBy.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.CreatedTime != new DateTime())
                {
                    sbInsert.Append("'" + pairTO.CreatedTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (pairTO.ModifiedBy.Trim() != "")
                {
                    sbInsert.Append("'" + pairTO.ModifiedBy.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                if (pairTO.ModifiedTime != new DateTime())
                {
                    sbInsert.Append("'" + pairTO.ModifiedTime.ToString(dateTimeformat) + "') ");
                }
                else
                {
                    sbInsert.Append("NOW()) ");
                }
                sbInsert.Append("SELECT @@Identity AS rec_id, @@Error as error ");
                sbInsert.Append("SET NOCOUNT OFF ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "IOPairsId");
                DataTable table = dataSet.Tables["IOPairsId"];

                int error = int.Parse(((DataRow)table.Rows[0])["error"].ToString());
                if (error == 0) //OK
                {
                    recID = uint.Parse(((DataRow)table.Rows[0])["rec_id"].ToString());

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
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback();
                throw ex;
            }

            return recID;
        }

        public int insert(int emplID, string modifiedBy, DateTime modifiedTime, DateTime date, int alertStatus, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                string select = "INSERT INTO io_pairs_processed_hist (io_pair_id, io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, "
                    + "manual_created, confirmation_flag, confirmed_by, confirmation_time, verification_flag, verified_by, verification_time, alert, description, "
                    + "created_by, created_time, modified_by, modified_time) "
                    + "SELECT io_pair_id, io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, "
                    + "manual_created, confirmation_flag, confirmed_by, confirmation_time, verification_flag, verified_by, verification_time, '" + alertStatus.ToString().Trim()
                    + "', description, created_by, created_time, '" + modifiedBy.Trim() + "', '" + modifiedTime.ToString(dateTimeformat) + "' "
                    + "FROM io_pairs_processed WHERE employee_id = '" + emplID.ToString().Trim() + "' AND io_pair_date = '" + date.ToString(dateTimeformat) + "' AND start_time <= end_time";

                MySqlCommand cmd = new MySqlCommand(select, conn, SqlTrans);
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

        public int insert(string recIDs, string modifiedBy, DateTime modifiedTime, int alertStatus, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                string select = "INSERT INTO io_pairs_processed_hist (io_pair_id, io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, "
                    + "manual_created, confirmation_flag, confirmed_by, confirmation_time, verification_flag, verified_by, verification_time, alert, description, "
                    + "created_by, created_time, modified_by, modified_time) "
                    + "SELECT io_pair_id, io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, "
                    + "manual_created, confirmation_flag, confirmed_by, confirmation_time, verification_flag, verified_by, verification_time, '" + alertStatus.ToString().Trim()
                    + "', description, created_by, created_time, '" + modifiedBy.Trim() + "', '" + modifiedTime.ToString(dateTimeformat) + "' "
                    + "FROM io_pairs_processed WHERE rec_id IN (" + recIDs.Trim() + ") AND start_time <= end_time";

                MySqlCommand cmd = new MySqlCommand(select, conn, SqlTrans);
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

        public IOPairsProcessedHistTO find(uint recID)
        {
            DataSet dataSet = new DataSet();
            IOPairsProcessedHistTO pair = new IOPairsProcessedHistTO();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM io_pairs_processed_hist WHERE rec_id = '" + recID.ToString().Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pairs");
                DataTable table = dataSet.Tables["Pairs"];

                if (table.Rows.Count == 1)
                {
                    pair = new IOPairsProcessedHistTO();
                    pair.RecID = UInt32.Parse(table.Rows[0]["rec_id"].ToString().Trim());
                    if (table.Rows[0]["io_pair_id"] != DBNull.Value)
                    {
                        pair.IOPairID = Int32.Parse(table.Rows[0]["io_pair_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["io_pair_date"] != DBNull.Value)
                    {
                        pair.IOPairDate = (DateTime)table.Rows[0]["io_pair_date"];
                    }
                    if (table.Rows[0]["employee_id"] != DBNull.Value)
                    {
                        pair.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["location_id"] != DBNull.Value)
                    {
                        pair.LocationID = Int32.Parse(table.Rows[0]["location_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["is_wrk_hrs_counter"] != DBNull.Value)
                    {
                        pair.IsWrkHrsCounter = Int32.Parse(table.Rows[0]["is_wrk_hrs_counter"].ToString().Trim());
                    }
                    if (table.Rows[0]["pass_type_id"] != DBNull.Value)
                    {
                        pair.PassTypeID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["start_time"] != DBNull.Value)
                    {
                        pair.StartTime = (DateTime)table.Rows[0]["start_time"];
                    }
                    if (table.Rows[0]["end_time"] != DBNull.Value)
                    {
                        pair.EndTime = (DateTime)table.Rows[0]["end_time"];
                    }
                    if (table.Rows[0]["manual_created"] != DBNull.Value)
                    {
                        pair.ManualCreated = Int32.Parse(table.Rows[0]["manual_created"].ToString().Trim());
                    }
                    if (table.Rows[0]["confirmation_flag"] != DBNull.Value)
                    {
                        pair.ConfirmationFlag = Int32.Parse(table.Rows[0]["confirmation_flag"].ToString().Trim());
                    }
                    if (table.Rows[0]["confirmed_by"] != DBNull.Value)
                    {
                        pair.ConfirmedBy = table.Rows[0]["confirmed_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["confirmation_time"] != DBNull.Value)
                    {
                        pair.ConfirmationTime = DateTime.Parse(table.Rows[0]["confirmation_time"].ToString().Trim());
                    }
                    if (table.Rows[0]["verification_flag"] != DBNull.Value)
                    {
                        pair.VerificationFlag = Int32.Parse(table.Rows[0]["verification_flag"].ToString().Trim());
                    }
                    if (table.Rows[0]["verified_by"] != DBNull.Value)
                    {
                        pair.VerifiedBy = table.Rows[0]["verified_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["verification_time"] != DBNull.Value)
                    {
                        pair.VerifiedTime = DateTime.Parse(table.Rows[0]["verification_time"].ToString().Trim());
                    }
                    if (table.Rows[0]["alert"] != DBNull.Value)
                    {
                        pair.Alert = table.Rows[0]["alert"].ToString().Trim();
                    }
                    if (table.Rows[0]["description"] != DBNull.Value)
                    {
                        pair.Desc = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_by"] != DBNull.Value)
                    {
                        pair.CreatedBy = table.Rows[0]["created_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["created_time"] != DBNull.Value)
                    {
                        pair.CreatedTime = DateTime.Parse(table.Rows[0]["created_time"].ToString().Trim());
                    }
                    if (table.Rows[0]["modified_by"] != DBNull.Value)
                    {
                        pair.ModifiedBy = table.Rows[0]["modified_by"].ToString().Trim();
                    }
                    if (table.Rows[0]["modified_time"] != DBNull.Value)
                    {
                        pair.ModifiedTime = DateTime.Parse(table.Rows[0]["modified_time"].ToString().Trim());
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pair;
        }

        public List<IOPairsProcessedHistTO> search(IOPairsProcessedHistTO pairTO)
        {
            DataSet dataSet = new DataSet();
            IOPairsProcessedHistTO pair = new IOPairsProcessedHistTO();
            List<IOPairsProcessedHistTO> pairsList = new List<IOPairsProcessedHistTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs_processed_hist ");

                if ((pairTO.IOPairID != -1) || (!pairTO.IOPairDate.Equals(new DateTime())) || (pairTO.EmployeeID != -1) ||
                    (pairTO.LocationID != -1) || (pairTO.IsWrkHrsCounter != -1) || (pairTO.PassTypeID != -1) ||
                    (!pairTO.StartTime.Equals(new DateTime())) || (!pairTO.EndTime.Equals(new DateTime())) ||
                    (pairTO.ManualCreated != -1) || (pairTO.ConfirmationFlag != -1) || (pairTO.ConfirmedBy.Trim() != "") || (pairTO.ConfirmationTime != new DateTime())
                    || (pairTO.VerificationFlag != -1) || (pairTO.VerifiedBy.Trim() != "") || (pairTO.VerifiedTime != new DateTime())
                    || (!pairTO.Alert.Trim().Equals("")) || (!pairTO.Desc.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (pairTO.IOPairID != -1)
                    {
                        sb.Append(" io_pair_id = '" + pairTO.IOPairID.ToString().Trim() + "' AND");
                    }
                    if (!pairTO.IOPairDate.Equals(new DateTime()))
                    {
                        sb.Append(" CONVERT(VARCHAR(24), io_pair_date, 120) = '" + pairTO.IOPairDate.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (pairTO.EmployeeID != -1)
                    {
                        sb.Append(" employee_id = '" + pairTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (pairTO.LocationID != -1)
                    {
                        sb.Append(" location_id = '" + pairTO.LocationID.ToString().Trim() + "' AND");
                    }
                    if (pairTO.IsWrkHrsCounter != -1)
                    {
                        sb.Append(" is_wrk_hrs_counter = '" + pairTO.IsWrkHrsCounter.ToString().Trim() + "' AND");
                    }
                    if (pairTO.PassTypeID != -1)
                    {
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
                        sb.Append(" manual_created = '" + pairTO.ManualCreated.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.ConfirmationFlag != -1)
                    {
                        sb.Append(" confirmation_flag = '" + pairTO.ConfirmationFlag.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.ConfirmedBy.Trim() != "")
                    {
                        sb.Append(" UPPER(confirmed_by) = '" + pairTO.ConfirmedBy.Trim().ToUpper() + "' AND");
                    }
                    if (!pairTO.ConfirmationTime.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(confimration_time,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.ConfirmationTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (pairTO.VerificationFlag != -1)
                    {
                        sb.Append(" verification_flag = '" + pairTO.VerificationFlag.ToString().Trim().ToUpper() + "' AND");
                    }
                    if (pairTO.VerifiedBy.Trim() != "")
                    {
                        sb.Append(" UPPER(verified_by) = '" + pairTO.VerifiedBy.Trim().ToUpper() + "' AND");
                    }
                    if (!pairTO.VerifiedTime.Equals(new DateTime()))
                    {
                        sb.Append(" DATE_FORMAT(verification_time,GET_FORMAT(DATETIME,'ISO')) = '" + pairTO.VerifiedTime.ToString("yyy-MM-dd HH:mm:ss") + "' AND");
                    }
                    if (!pairTO.Alert.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(alert) = '" + pairTO.Alert.Trim().ToUpper() + "' AND");
                    }
                    if (!pairTO.Desc.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) = '" + pairTO.Desc.Trim().ToUpper() + "' AND");
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
                        pair = new IOPairsProcessedHistTO();
                        pair.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        if (row["io_pair_id"] != DBNull.Value)
                        {
                            pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        }
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pair.IOPairDate = (DateTime)row["io_pair_date"];
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
                            pair.IsWrkHrsCounter = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = (DateTime)row["start_time"];
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = (DateTime)row["end_time"];
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            pair.ConfirmationFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["confirmed_by"] != DBNull.Value)
                        {
                            pair.ConfirmedBy = row["confirmed_by"].ToString().Trim();
                        }
                        if (row["confirmation_time"] != DBNull.Value)
                        {
                            pair.ConfirmationTime = DateTime.Parse(row["confirmation_time"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            pair.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }
                        if (row["verified_by"] != DBNull.Value)
                        {
                            pair.VerifiedBy = row["verified_by"].ToString().Trim();
                        }
                        if (row["verification_time"] != DBNull.Value)
                        {
                            pair.VerifiedTime = DateTime.Parse(row["verification_time"].ToString().Trim());
                        }
                        if (row["alert"] != DBNull.Value)
                        {
                            pair.Alert = row["alert"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            pair.Desc = row["description"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            pair.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            pair.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
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

        public Dictionary<int, Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>> getIOPairsAllForEmpl(string employeeIDString, List<DateTime> datesList)
        {
            DataSet dataSet = new DataSet();
            IOPairsProcessedHistTO pair = new IOPairsProcessedHistTO();
            Dictionary<int, Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>> emplPairsDaySets = new Dictionary<int, Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>>();
            string select;

            try
            {
                if (!employeeIDString.Trim().Equals("") || datesList.Count > 0)
                {
                    select = "SELECT * FROM io_pairs_processed_hist WHERE start_time <= end_time";

                    if (!employeeIDString.Trim().Equals(""))
                    {
                        select += " AND employee_id IN (" + employeeIDString + ") ";
                    }

                    if (datesList.Count > 0)
                    {
                        select += " AND io_pair_date IN (";
                        foreach (DateTime currentDate in datesList)
                        {
                            select += "'" + currentDate.ToString(dateTimeformat) + "', ";
                        }

                        select = select.Substring(0, select.Length - 2);
                        select += ")";
                    }

                    select += " ORDER BY modified_time, start_time";

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                    DataTable table = dataSet.Tables["IOPairsProcessed"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pair = new IOPairsProcessedHistTO();
                            pair.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                            if (row["io_pair_id"] != DBNull.Value)
                            {
                                pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                            }
                            if (row["io_pair_date"] != DBNull.Value)
                            {
                                pair.IOPairDate = (DateTime)row["io_pair_date"];
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
                                pair.IsWrkHrsCounter = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                            }
                            if (row["pass_type_id"] != DBNull.Value)
                            {
                                pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                            }
                            if (row["start_time"] != DBNull.Value)
                            {
                                pair.StartTime = (DateTime)row["start_time"];
                            }
                            if (row["end_time"] != DBNull.Value)
                            {
                                pair.EndTime = (DateTime)row["end_time"];
                            }
                            if (row["manual_created"] != DBNull.Value)
                            {
                                pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                            }
                            if (row["confirmation_flag"] != DBNull.Value)
                            {
                                pair.ConfirmationFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                            }
                            if (row["confirmed_by"] != DBNull.Value)
                            {
                                pair.ConfirmedBy = row["confirmed_by"].ToString().Trim();
                            }
                            if (row["confirmation_time"] != DBNull.Value)
                            {
                                pair.ConfirmationTime = DateTime.Parse(row["confirmation_time"].ToString().Trim());
                            }
                            if (row["verification_flag"] != DBNull.Value)
                            {
                                pair.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                            }
                            if (row["verified_by"] != DBNull.Value)
                            {
                                pair.VerifiedBy = row["verified_by"].ToString().Trim();
                            }
                            if (row["verification_time"] != DBNull.Value)
                            {
                                pair.VerifiedTime = DateTime.Parse(row["verification_time"].ToString().Trim());
                            }
                            if (row["alert"] != DBNull.Value)
                            {
                                pair.Alert = row["alert"].ToString().Trim();
                            }
                            if (row["description"] != DBNull.Value)
                            {
                                pair.Desc = row["description"].ToString().Trim();
                            }
                            if (row["created_by"] != DBNull.Value)
                            {
                                pair.CreatedBy = row["created_by"].ToString().Trim();
                            }
                            if (row["created_time"] != DBNull.Value)
                            {
                                pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                            }
                            if (row["modified_by"] != DBNull.Value)
                            {
                                pair.ModifiedBy = row["modified_by"].ToString().Trim();
                            }
                            if (row["modified_time"] != DBNull.Value)
                            {
                                pair.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                            }                            

                            if (!emplPairsDaySets.ContainsKey(pair.EmployeeID))
                                emplPairsDaySets.Add(pair.EmployeeID, new Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>());

                            if (!emplPairsDaySets[pair.EmployeeID].ContainsKey(pair.ModifiedTime))
                                emplPairsDaySets[pair.EmployeeID].Add(pair.ModifiedTime, new Dictionary<string, List<IOPairsProcessedHistTO>>());

                            if (!emplPairsDaySets[pair.EmployeeID][pair.ModifiedTime].ContainsKey(pair.ModifiedBy))
                                emplPairsDaySets[pair.EmployeeID][pair.ModifiedTime].Add(pair.ModifiedBy, new List<IOPairsProcessedHistTO>());

                            emplPairsDaySets[pair.EmployeeID][pair.ModifiedTime][pair.ModifiedBy].Add(pair);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return emplPairsDaySets;
        }

        public Dictionary<DateTime, List<IOPairsProcessedHistTO>> getIOPairsSet(int emplID, DateTime date)
        {
            DataSet dataSet = new DataSet();
            IOPairsProcessedHistTO pair = new IOPairsProcessedHistTO();
            List<IOPairsProcessedHistTO> pairsList = new List<IOPairsProcessedHistTO>();
            Dictionary<DateTime, List<IOPairsProcessedHistTO>> pairsDic = new Dictionary<DateTime, List<IOPairsProcessedHistTO>>();

            try
            {
                string select = "SELECT iop.*, au.name modified_name FROM io_pairs_processed_hist iop, appl_users au WHERE iop.employee_id = '" + emplID.ToString().Trim() 
                    + "' AND iop.io_pair_date = '" + date.Date.ToString(dateTimeformat) + "' AND iop.modified_by = au.user_id AND iop.modified_time IN (SELECT modified_time FROM io_pairs_processed_hist WHERE employee_id = '" 
                    + emplID.ToString().Trim() + "' AND io_pair_date = '" + date.Date.ToString(dateTimeformat) + "' AND alert = '" + Constants.alertStatus.ToString().Trim()
                    + "') ORDER BY modified_time, start_time";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairsProcessedHistTO();
                        pair.RecID = UInt32.Parse(row["rec_id"].ToString().Trim());
                        if (row["io_pair_id"] != DBNull.Value)
                        {
                            pair.IOPairID = Int32.Parse(row["io_pair_id"].ToString().Trim());
                        }
                        if (row["io_pair_date"] != DBNull.Value)
                        {
                            pair.IOPairDate = (DateTime)row["io_pair_date"];
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
                            pair.IsWrkHrsCounter = Int32.Parse(row["is_wrk_hrs_counter"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            pair.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = (DateTime)row["start_time"];
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = (DateTime)row["end_time"];
                        }
                        if (row["manual_created"] != DBNull.Value)
                        {
                            pair.ManualCreated = Int32.Parse(row["manual_created"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            pair.ConfirmationFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["confirmed_by"] != DBNull.Value)
                        {
                            pair.ConfirmedBy = row["confirmed_by"].ToString().Trim();
                        }
                        if (row["confirmation_time"] != DBNull.Value)
                        {
                            pair.ConfirmationTime = DateTime.Parse(row["confirmation_time"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            pair.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }
                        if (row["verified_by"] != DBNull.Value)
                        {
                            pair.VerifiedBy = row["verified_by"].ToString().Trim();
                        }
                        if (row["verification_time"] != DBNull.Value)
                        {
                            pair.VerifiedTime = DateTime.Parse(row["verification_time"].ToString().Trim());
                        }
                        if (row["alert"] != DBNull.Value)
                        {
                            pair.Alert = row["alert"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            pair.Desc = row["description"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            pair.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            pair.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }
                        if (row["modified_name"] != DBNull.Value)
                        {
                            pair.ModifiedName = row["modified_name"].ToString().Trim();
                        }
                        
                        if (!pairsDic.ContainsKey(pair.ModifiedTime))
                            pairsDic.Add(pair.ModifiedTime, new List<IOPairsProcessedHistTO>());

                        pairsDic[pair.ModifiedTime].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pairsDic;
        }

        public bool isAlert(int emplID, DateTime date)
        {
            DataSet dataSet = new DataSet();
            bool isAlert = false;

            try
            {
                string select = "SELECT COUNT(*) num FROM io_pairs_processed_hist WHERE employee_id = '" + emplID.ToString().Trim() + "' AND io_pair_date = '"
                    + date.ToString(dateTimeformat) + "' AND alert = '" + Constants.alertStatus + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    if (table.Rows[0]["num"] != DBNull.Value)
                        isAlert = int.Parse(table.Rows[0]["num"].ToString().Trim()) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return isAlert;
        }

        public Dictionary<int, Dictionary<DateTime, int>> getAlerts(string emplIDs, DateTime from, DateTime to)
        {
            DataSet dataSet = new DataSet();
            Dictionary<int, Dictionary<DateTime, int>> alerts = new Dictionary<int, Dictionary<DateTime, int>>();

            try
            {
                if (emplIDs.Trim().Equals(""))
                    return alerts;

                string select = "SELECT employee_id, io_pair_date, COUNT(*) num FROM io_pairs_processed_hist WHERE employee_id IN (" + emplIDs.ToString().Trim() + ") ";

                if (!from.Equals(new DateTime()))
                    select += "AND io_pair_date >= '" + from.Date.ToString(dateTimeformat) + "' ";

                if (!to.Equals(new DateTime()))
                    select += "AND io_pair_date < '" + to.AddDays(1).Date.ToString(dateTimeformat) + "' ";

                select += "AND alert = '" + Constants.alertStatus + "' GROUP BY employee_id, io_pair_date ORDER BY employee_id, io_pair_date";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    int empl = -1;
                    DateTime date = new DateTime();
                    int num = 0;

                    foreach (DataRow row in table.Rows)
                    {
                        if (row["employee_id"] != DBNull.Value && row["io_pair_date"] != DBNull.Value && row["num"] != DBNull.Value)
                        {
                            if (int.TryParse(row["employee_id"].ToString(), out empl) && DateTime.TryParse(row["io_pair_date"].ToString(), out date)
                                && int.TryParse(row["num"].ToString(), out num))
                            {
                                if (!alerts.ContainsKey(empl))
                                    alerts.Add(empl, new Dictionary<DateTime, int>());

                                if (!alerts[empl].ContainsKey(date.Date))
                                    alerts[empl].Add(date.Date, num);
                                else
                                    alerts[empl][date.Date] = num;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return alerts;
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
