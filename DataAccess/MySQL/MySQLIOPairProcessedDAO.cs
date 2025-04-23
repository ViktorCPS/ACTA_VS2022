using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using TransferObjects;
using MySql.Data.MySqlClient;
using System.Data;
using Util;

namespace DataAccess
{
    public class MySQLIOPairProcessedDAO : IOPairProcessedDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MySQLIOPairProcessedDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLIOPairProcessedDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        //natalija 23.01.2018
        public Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getPairsForIntervalForWU(DateTime startTime, DateTime endTime, string passTypeString, string wuIDs, bool isRetired)
        {
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

            return pairsList;
        }

        public bool DeleteDuplicates(int month)
        {
            return false;
        }

        public bool delete(Dictionary<int, List<DateTime>> emplDateList)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs_processed WHERE ");

                foreach (int empl in emplDateList.Keys)
                {
                    sbDelete.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbDelete.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbDelete.Remove(sbDelete.ToString().Length - 2, 2);

                    sbDelete.Append(")) OR");
                }
                string select = sbDelete.ToString().Substring(0, sbDelete.ToString().Length - 2);
                //select += "AND NOT EXISTS (SELECT * FROM io_pairs_processed pr WHERE pr.employee_id = employee_id AND io_pair_date = pr.io_pair_date";
                //select += " AND manual_created = " + Constants.yesInt.ToString() + " AND alert != " + Constants.alertStatusAgreeToChange.ToString() + ")";
                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
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

        public bool delete(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs_processed WHERE ");

                foreach (int empl in emplDateList.Keys)
                {
                    sbDelete.Append(" ( employee_id = " + empl + " AND ");
                    sbDelete.Append(" DATE_FORMAT(start_time,'%H:%i') <= '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbDelete.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbDelete.Remove(sbDelete.ToString().Length - 2, 2);

                    sbDelete.Append(")) OR");
                }
                foreach (int empl in emplDateListDayAfter.Keys)
                {
                    sbDelete.Append(" ( employee_id = " + empl + " AND ");
                    sbDelete.Append(" DATE_FORMAT(start_time,'%H:%i') > '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbDelete.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbDelete.Remove(sbDelete.ToString().Length - 2, 2);

                    sbDelete.Append(")) OR");
                }
                string select = sbDelete.ToString().Substring(0, sbDelete.ToString().Length - 2);
                //select += "AND NOT EXISTS (SELECT * FROM io_pairs_processed pr WHERE pr.employee_id = employee_id AND io_pair_date = pr.io_pair_date";
                //select += " AND manual_created = " + Constants.yesInt.ToString() + " AND alert != " + Constants.alertStatusAgreeToChange.ToString() + ")";
                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
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

        //natalija 08112017
        public bool delete(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore, bool doCommit)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;
            try
            {
                StringBuilder sbDelete = new StringBuilder();
                //sbDelete.Append("DELETE FROM io_pairs_processed WHERE ");
                sbDelete.Append("DELETE ipp FROM io_pairs_processed ipp INNER JOIN pass_types pt ON ipp.pass_type_id = pt.pass_type_id WHERE pt.pass_type != 2 AND ");//IZMENA 08112017
                

                foreach (int empl in emplDateList.Keys)
                {
                    sbDelete.Append(" ( employee_id = " + empl + " AND ");
                    sbDelete.Append(" DATE_FORMAT(start_time,'%H:%i') <= '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbDelete.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbDelete.Remove(sbDelete.ToString().Length - 2, 2);

                    sbDelete.Append(")) OR");
                }
                foreach (int empl in emplDateListDayAfter.Keys)
                {
                    sbDelete.Append(" ( employee_id = " + empl + " AND ");
                    sbDelete.Append(" DATE_FORMAT(start_time,'%H:%i') > '12:00' AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbDelete.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbDelete.Remove(sbDelete.ToString().Length - 2, 2);

                    sbDelete.Append(")) OR");
                }
                string select = sbDelete.ToString().Substring(0, sbDelete.ToString().Length - 2);
                //select += "AND NOT EXISTS (SELECT * FROM io_pairs_processed pr WHERE pr.employee_id = employee_id AND io_pair_date = pr.io_pair_date";
                //select += " AND manual_created = " + Constants.yesInt.ToString() + " AND alert != " + Constants.alertStatusAgreeToChange.ToString() + ")";
                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
                }
                if(doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if(doCommit)
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool delete(uint RecID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs_processed WHERE rec_id = '" + RecID.ToString().Trim() + "'");

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

        public bool DeleteUjustified(Dictionary<int, List<DateTime>> emplDateList, bool doCommit)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs_processed WHERE ");

                foreach (int empl in emplDateList.Keys)
                {
                    sbDelete.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sbDelete.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sbDelete.Remove(sbDelete.ToString().Length - 2, 2);

                    sbDelete.Append(") ");
                    sbDelete.Append(" AND io_pair_date IN ( ");
                    sbDelete.Append("SELECT io_pair_date from io_pairs_processed ");
                    sbDelete.Append("WHERE employee_id =" + empl + " ");
                    sbDelete.Append("GROUP BY io_pair_date ");
                    sbDelete.Append("HAVING COUNT(rec_id) = 1 ) ");
                    sbDelete.Append("AND pass_type_id = " + Constants.absence.ToString() + " ) OR");
                }
                string select = sbDelete.ToString().Substring(0, sbDelete.ToString().Length - 2);
                //select += "AND NOT EXISTS (SELECT * FROM io_pairs_processed pr WHERE pr.employee_id = employee_id AND io_pair_date = pr.io_pair_date";
                //select += " AND manual_created = " + Constants.yesInt.ToString() + " AND alert != " + Constants.alertStatusAgreeToChange.ToString() + ")";
                MySqlCommand cmd = new MySqlCommand(select, conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                
                    isDeleted = true;
                    if (doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool delete(int emplID, DateTime date, bool doCommit)
        {
            bool isDeleted = false;
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs_processed WHERE employee_id = '" + emplID.ToString().Trim() + "' AND io_pair_date = '" + date.ToString(dateTimeformat) + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
                }

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool delete(string recIDs, bool doCommit)
        {
            bool isDeleted = false;
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM io_pairs_processed WHERE rec_id IN (" + recIDs.ToString().Trim() + ")");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
                }

                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public uint insert(IOPairProcessedTO pairTO, bool doCommit)
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
                sbInsert.Append("INSERT INTO io_pairs_processed ");
                sbInsert.Append("(io_pair_id, io_pair_date, employee_id, location_id, is_wrk_hrs_counter, pass_type_id, start_time, end_time, manual_created, confirmation_flag,confirmed_by, confirmation_time, verification_flag, verified_by, verification_time,alert, description, created_by, created_time) ");
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
                    sbInsert.Append("N'" + pairTO.ConfirmedBy.ToString().Trim() + "', ");
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
                    sbInsert.Append("N'" + pairTO.VerifiedBy.ToString().Trim() + "', ");
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
                    sbInsert.Append("'" + pairTO.Alert.Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!pairTO.Desc.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + pairTO.Desc.Trim().Replace("'", "").Replace("%", "").Replace("\r\n", "").Replace("\t", "").Replace("\"", "") + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!pairTO.CreatedBy.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + pairTO.CreatedBy.Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                }
                if (!pairTO.CreatedTime.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + pairTO.CreatedTime.ToString(dateTimeformat) + "') ");
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
      
        public bool update(IOPairProcessedTO pairTO, bool doCommit)
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
                sbUpdate.Append("UPDATE io_pairs_processed SET ");

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
                if (pairTO.IOPairID != -1)
                {
                    sbUpdate.Append("io_pair_id = " + pairTO.IOPairID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("io_pair_id = NULL, ");
                }
                if (pairTO.LocationID != -1)
                {
                    sbUpdate.Append("location_id = " + pairTO.LocationID.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("location_id = NULL, ");
                }
                if (pairTO.IsWrkHrsCounter != -1)
                {
                    sbUpdate.Append("is_wrk_hrs_counter = " + pairTO.IsWrkHrsCounter.ToString().Trim() + ", ");
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
                if (pairTO.ConfirmationFlag != -1)
                {
                    sbUpdate.Append("confirmation_flag = " + pairTO.ConfirmationFlag.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("confirmation_flag = NULL, ");
                }
                if (pairTO.ConfirmedBy.Trim() != "")
                {
                    sbUpdate.Append("confirmed_by = '" + pairTO.ConfirmedBy.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("confirmed_by = NULL, ");
                }
                if (!pairTO.ConfirmationTime.Equals(new DateTime()))
                {
                    sbUpdate.Append("confirmation_time = '" + pairTO.ConfirmationTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("confirmation_time = NULL, ");
                }
                if (pairTO.VerificationFlag != -1)
                {
                    sbUpdate.Append("verification_flag = " + pairTO.VerificationFlag.ToString().Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("verification_flag = NULL, ");
                }
                if (pairTO.VerifiedBy.Trim() != "")
                {
                    sbUpdate.Append("verified_by = '" + pairTO.VerifiedBy.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("verified_by = NULL, ");
                }
                if (!pairTO.VerifiedTime.Equals(new DateTime()))
                {
                    sbUpdate.Append("verification_time = '" + pairTO.VerifiedTime.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("verification_time = NULL, ");
                }
                if (!pairTO.Alert.Trim().Equals(""))
                {
                    sbUpdate.Append("alert = " + pairTO.Alert.Trim() + ", ");
                }
                else
                {
                    sbUpdate.Append("alert = NULL, ");
                }
                if (!pairTO.Desc.Trim().Equals(""))
                {
                    sbUpdate.Append("description = " + pairTO.Desc.Trim().Replace("'", "").Replace("%", "").Replace("\r\n", "").Replace("\t", "").Replace("\"", "") + ", ");
                }
                else
                {
                    sbUpdate.Append("description = NULL, ");
                }
                if (!pairTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + pairTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE rec_id = '" + pairTO.RecID.ToString().Trim() + "'");

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

        //natalija08112017 update celodnevna odsustva
        public bool updateManualCreatedProcessedPairs(IOPairProcessedTO processed, Dictionary<int, WorkTimeIntervalTO> workTimeInterval,Dictionary<int, WorkTimeIntervalTO> workTimeIntervalNextDay, bool is2DayShift, IDbTransaction trans)
        {
            bool isUpdated = false;
            StringBuilder sbUpdate = new StringBuilder();
            WorkTimeIntervalTO wtInterval = new WorkTimeIntervalTO();

            try
            {

                foreach (int wti in workTimeInterval.Keys)
                {
                    wtInterval = workTimeInterval[wti];
                    int interval = wtInterval.IntervalNum;
                    int dayNum = wtInterval.DayNum;

                    DateTime start = wtInterval.StartTime;
                    DateTime end = wtInterval.EndTime;


                    sbUpdate.Append("UPDATE io_pairs_processed SET ");

                    if (!processed.StartTime.Equals(new DateTime()))
                    {
                        sbUpdate.Append("start_time = '" + processed.StartTime.ToString(dateTimeformat) + "', ");
                    }
                    else
                    {
                        sbUpdate.Append("start_time = NULL, ");
                    }
                    if (!processed.EndTime.Equals(new DateTime()))
                    {
                        sbUpdate.Append("end_time = '" + processed.EndTime.ToString(dateTimeformat) + "', ");
                    }
                    else
                    {
                        sbUpdate.Append("end_time = NULL, ");
                    }
                    if (processed.CreatedBy.Equals(""))
                    {
                        sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                    }
                    else
                    {
                        sbUpdate.Append("modified_by = N'" + processed.CreatedBy.Trim() + "', ");
                    }
                    if (!processed.ModifiedBy.Trim().Equals(""))
                        sbUpdate.Append("modified_by = N'" + processed.ModifiedBy.Trim() + "', ");
                    else
                        sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");

                    sbUpdate.Append("WHERE io_pair_processed_id = '" + processed.IOPairID.ToString().Trim() + "'");

                }


                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, (MySqlTransaction)trans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }


        public bool verify(string recIDs, string verifiedBy, string ptIDs)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            
            try
            {
                if (recIDs.Length > 0)
                {
                    StringBuilder sbUpdate = new StringBuilder();
                    sbUpdate.Append("UPDATE io_pairs_processed SET verification_flag = '" + ((int)Constants.Verification.Verified).ToString().Trim());
                    sbUpdate.Append("', verified_by = N'" + verifiedBy.Trim() + "', verification_time = NOW(), ");
                    sbUpdate.Append("modified_by = N'" + verifiedBy.Trim() + "', ");
                    sbUpdate.Append("modified_time = NOW() ");
                    sbUpdate.Append("WHERE rec_id IN (" + recIDs.Trim() + ")");
                    if (ptIDs.Length > 0)
                        sbUpdate.Append(" AND pass_type_id IN (" + ptIDs.Trim() + ")");

                    MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        isUpdated = true;
                    }
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
        
        public IOPairProcessedTO find(uint recID)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM io_pairs_processed WHERE rec_id = '" + recID.ToString().Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Pairs");
                DataTable table = dataSet.Tables["Pairs"];

                if (table.Rows.Count == 1)
                {
                    pair = new IOPairProcessedTO();
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pair;
        }

        public bool verify(string emplIDs, string verifiedBy, DateTime month, string passTypes, bool validateIsVerifiedDay)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            DateTime firstDay = new DateTime(month.Year, month.Month, 1, 0, 0, 0);
            DateTime firstDayNextMonth = firstDay.AddMonths(1);

            try
            {
                if (emplIDs.Length > 0)
                {
                    StringBuilder sbUpdate = new StringBuilder();
                    sbUpdate.Append("UPDATE io_pairs_processed SET verification_flag = '" + ((int)Constants.Verification.Verified).ToString().Trim());
                    sbUpdate.Append("', verified_by = N'" + verifiedBy.Trim() + "', verification_time = NOW(), ");
                    sbUpdate.Append("modified_by = N'" + verifiedBy.Trim() + "', ");
                    sbUpdate.Append("modified_time = NOW() ");
                    sbUpdate.Append("WHERE employee_id IN (" + emplIDs.Trim() + ") AND io_pair_date >= '" + firstDay.ToString(dateTimeformat));
                    sbUpdate.Append("' AND io_pair_date < '" + firstDayNextMonth.ToString(dateTimeformat) + "' AND verification_flag = '" + ((int)Constants.Verification.NotVerified).ToString().Trim());
                    sbUpdate.Append("' AND pass_type_id IN (" + passTypes.Trim() + ")");
                    if (validateIsVerifiedDay)
                    {
                        sbUpdate.Append(" AND io_pair_date NOT IN (SELECT io_pair_date FROM io_pairs_processed WHERE (verified_by IS NOT NULL OR verification_time IS NOT NULL)");
                        sbUpdate.Append(" AND employee_id IN (" + emplIDs.Trim() + ")");
                        sbUpdate.Append(" AND io_pair_date >= '" + firstDay.ToString(dateTimeformat));
                        sbUpdate.Append("' AND io_pair_date < '" + firstDayNextMonth.ToString(dateTimeformat) + "')");
                    }

                    MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        isUpdated = true;
                    }
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

        public int getPaidLeaveDays(DateTime from, DateTime to, DateTime date, int employeeID, int ptID, int limitCompositeID, int limitOccassionalyID, int limitElemetaryID)
        {
            try
            {
                int days = 0;
                DataSet dataSet = new DataSet();

                // if at least one limit is not null
                if (limitCompositeID != -1 || limitElemetaryID != -1 || limitOccassionalyID != -1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(DISTINCT io_pair_date) days FROM io_pairs_processed iop, pass_types pt ");
                    sb.Append("WHERE iop.io_pair_date >= '" + from.ToString(dateTimeformat).Trim() + "' AND iop.io_pair_date < '" + to.ToString(dateTimeformat).Trim() + "' ");
                    sb.Append("AND iop.io_pair_date <> '" + date.ToString(dateTimeformat).Trim() + "' AND DATE_FORMAT(iop.start_time, '%H:%i:%s') <> '00:00:00' "); // second interval from night shift belongs to previous day
                    sb.Append("AND iop.pass_type_id = pt.pass_type_id AND iop.employee_id = '" + employeeID.ToString().Trim() + "' ");

                    if (limitCompositeID != -1)
                        sb.Append("AND pt.limit_composite_id = '" + limitCompositeID.ToString().Trim() + "' ");
                    if (limitOccassionalyID != -1)
                        sb.Append("AND pt.limit_occassionaly_id = '" + limitOccassionalyID.ToString().Trim() + "' ");
                    if (limitElemetaryID != -1)
                        sb.Append("AND iop.pass_type_id = '" + ptID.ToString().Trim() + "' ");

                    MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Pairs");
                    DataTable table = dataSet.Tables["Pairs"];

                    if (table.Rows.Count == 1)
                    {
                        days = Int32.Parse(table.Rows[0]["days"].ToString().Trim());
                    }
                }

                return days;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getPaidLeaveDaysOutsidePeriod(DateTime from, DateTime to, DateTime periodStart, DateTime periodEnd, int employeeID, int ptID, int limitCompositeID, int limitOccassionalyID, int limitElemetaryID)
        {
            try
            {
                int days = 0;
                DataSet dataSet = new DataSet();

                // if at least one limit is not null
                if (limitCompositeID != -1 || limitElemetaryID != -1 || limitOccassionalyID != -1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT(DISTINCT io_pair_date) days FROM io_pairs_processed iop, pass_types pt ");
                    sb.Append("WHERE iop.io_pair_date >= '" + from.ToString(dateTimeformat).Trim() + "' AND iop.io_pair_date < '" + to.ToString(dateTimeformat).Trim() + "' ");
                    sb.Append("AND (iop.io_pair_date < '" + periodStart.ToString(dateTimeformat).Trim() + "' OR iop.io_pair_date > '" + periodEnd.ToString(dateTimeformat).Trim() + "') ");
                    sb.Append("AND DATE_FORMAT(iop.start_time, '%H:%i:%s') <> '00:00:00' "); // second interval from night shift belongs to previous day
                    sb.Append("AND iop.pass_type_id = pt.pass_type_id AND iop.employee_id = '" + employeeID.ToString().Trim() + "' ");

                    if (limitCompositeID != -1)
                        sb.Append("AND pt.limit_composite_id = '" + limitCompositeID.ToString().Trim() + "' ");
                    if (limitOccassionalyID != -1)
                        sb.Append("AND pt.limit_occassionaly_id = '" + limitOccassionalyID.ToString().Trim() + "' ");
                    if (limitElemetaryID != -1)
                        sb.Append("AND iop.pass_type_id = '" + ptID.ToString().Trim() + "' ");

                    MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "Pairs");
                    DataTable table = dataSet.Tables["Pairs"];

                    if (table.Rows.Count == 1)
                    {
                        days = Int32.Parse(table.Rows[0]["days"].ToString().Trim());
                    }
                }

                return days;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getPairsForInterval(IOPairProcessedTO pairTO, DateTime startTime, DateTime endTime)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs_processed ");
                sb.Append(" WHERE io_pair_date > = '" + startTime.ToString(dateTimeformat) + "' AND io_pair_date <= '" + endTime.ToString(dateTimeformat) + "' ");
                if ((pairTO.IOPairID != -1) || (!pairTO.IOPairDate.Equals(new DateTime())) || (pairTO.EmployeeID != -1) ||
                    (pairTO.LocationID != -1) || (pairTO.IsWrkHrsCounter != -1) || (pairTO.PassTypeID != -1) ||
                    (!pairTO.StartTime.Equals(new DateTime())) || (!pairTO.EndTime.Equals(new DateTime())) ||
                    (pairTO.ManualCreated != -1) || (pairTO.ConfirmationFlag != -1) || (pairTO.ConfirmedBy.Trim() != "") || (pairTO.ConfirmationTime != new DateTime())
                    || (pairTO.VerificationFlag != -1) || (pairTO.VerifiedBy.Trim() != "") || (pairTO.VerifiedTime != new DateTime())
                    || (!pairTO.Alert.Trim().Equals("")) || (!pairTO.Desc.Trim().Equals("")))
                {
                    sb.Append(" AND");

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
                MySqlCommand cmd = new MySqlCommand(select+" ORDER BY employee_id, start_time", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }

        public Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getPairsForInterval(IOPairProcessedTO pairTO, DateTime startTime, DateTime endTime, string employeeIDs)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
            string select = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs_processed ");
                sb.Append(" WHERE io_pair_date > = '" + startTime.ToString(dateTimeformat) + "' AND io_pair_date <= '" + endTime.ToString(dateTimeformat) + "' ");
                sb.Append(" AND employee_id IN(" + employeeIDs + ") " );
                if ((pairTO.IOPairID != -1) || (!pairTO.IOPairDate.Equals(new DateTime())) || (pairTO.EmployeeID != -1) ||
                    (pairTO.LocationID != -1) || (pairTO.IsWrkHrsCounter != -1) || (pairTO.PassTypeID != -1) ||
                    (!pairTO.StartTime.Equals(new DateTime())) || (!pairTO.EndTime.Equals(new DateTime())) ||
                    (pairTO.ManualCreated != -1) || (pairTO.ConfirmationFlag != -1) || (pairTO.ConfirmedBy.Trim() != "") || (pairTO.ConfirmationTime != new DateTime())
                    || (pairTO.VerificationFlag != -1) || (pairTO.VerifiedBy.Trim() != "") || (pairTO.VerifiedTime != new DateTime())
                    || (!pairTO.Alert.Trim().Equals("")) || (!pairTO.Desc.Trim().Equals("")))
                {
                    sb.Append(" AND");

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
                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, start_time", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }

        public Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getPairsToVerifyEmplDaySet(string recIDs)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
            string select = "";
            try
            {
                if (recIDs.Trim().Equals(""))
                    return pairsList;

                select = "SELECT * FROM io_pairs_processed WHERE io_pair_date IN (SELECT io_pair_date FROM io_pairs_processed WHERE rec_id IN (" + recIDs.Trim() + "))"
                    + " AND employee_id IN (SELECT employee_id FROM io_pairs_processed WHERE rec_id IN (" + recIDs.Trim() + "))";
                
                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY employee_id, start_time", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }

        public List<IOPairProcessedTO> search(IOPairProcessedTO pairTO)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs_processed ");

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
                        sb.Append(" io_pair_date = '" + pairTO.IOPairDate.ToString(dateTimeformat) + "' AND");
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
                        sb.Append(" start_time = '" + pairTO.StartTime.ToString(dateTimeformat) + "' AND");
                    }
                    if (!pairTO.EndTime.Equals(new DateTime()))
                    {
                        sb.Append(" end_time = '" + pairTO.EndTime.ToString(dateTimeformat) + "' AND");
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
                        sb.Append(" confimration_time = '" + pairTO.ConfirmationTime.ToString(dateTimeformat) + "' AND");
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
                        sb.Append(" verification_time = '" + pairTO.VerifiedTime.ToString(dateTimeformat) + "' AND");
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
                        pair = new IOPairProcessedTO();
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
        
        public List<IOPairProcessedTO> getWeekPairs(int emplID, DateTime date, bool includeDate, string ptIDs, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();
            List<IOPairProcessedTO> weekPairs = new List<IOPairProcessedTO>();
            string select = "";

            try
            {
                if (emplID == -1 || ptIDs.Length <= 0 || date.Equals(new DateTime()))
                    return weekPairs;

                int dayNum = 0;
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        dayNum = 0;
                        break;
                    case DayOfWeek.Tuesday:
                        dayNum = 1;
                        break;
                    case DayOfWeek.Wednesday:
                        dayNum = 2;
                        break;
                    case DayOfWeek.Thursday:
                        dayNum = 3;
                        break;
                    case DayOfWeek.Friday:
                        dayNum = 4;
                        break;
                    case DayOfWeek.Saturday:
                        dayNum = 5;
                        break;
                    case DayOfWeek.Sunday:
                        dayNum = 6;
                        break;
                }

                DateTime weekBegining = date.AddDays(-dayNum).Date; // first day of current week
                DateTime weekEnd = date.AddDays(7 - dayNum).Date; // first day of next week

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM io_pairs_processed WHERE io_pair_date >= '" + weekBegining.Date.ToString(dateTimeformat) + "' AND io_pair_date <= '" + weekEnd.Date.ToString(dateTimeformat) + "' ");
                sb.Append("AND employee_id = '" + emplID.ToString().Trim() + "' ");
                sb.Append("AND pass_type_id IN (" + ptIDs.Trim() + ") ");

                select = sb.ToString();

                MySqlCommand cmd;
                if (trans != null)
                    cmd = new MySqlCommand(select + " ORDER BY io_pair_date, start_time", conn, (MySqlTransaction)trans);
                else
                    cmd = new MySqlCommand(select + " ORDER BY io_pair_date, start_time", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairs");
                DataTable table = dataSet.Tables["IOPairs"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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

                        pairsList.Add(pair);
                    }
                }

                // remove pairs from first week day that belongs to previous week
                DateTime lastEndPreviousDay = new DateTime();
                for (int i = 0; i < pairsList.Count; i++)
                {
                    if (pairsList[i].IOPairDate.Date.Equals(weekBegining.Date))
                    {
                        if ((pairsList[i].StartTime.Hour == 0 && pairsList[i].StartTime.Minute == 0) || pairsList[i].StartTime.Equals(lastEndPreviousDay))
                        {
                            lastEndPreviousDay = pairsList[i].EndTime;
                            continue;
                        }
                    }

                    if (!includeDate)
                    {
                        if (pairsList[i].IOPairDate.Date.Equals(date.Date))
                        {
                            if ((pairsList[i].StartTime.Hour == 0 && pairsList[i].StartTime.Minute == 0) || pairsList[i].StartTime.Equals(lastEndPreviousDay))
                            {
                                lastEndPreviousDay = pairsList[i].EndTime;
                                weekPairs.Add(pairsList[i]);
                                continue;
                            }
                            else
                                continue;
                        }
                        if (pairsList[i].IOPairDate.Date.Equals(date.AddDays(1).Date))
                        {
                            if ((pairsList[i].StartTime.Hour == 0 && pairsList[i].StartTime.Minute == 0) || pairsList[i].StartTime.Equals(lastEndPreviousDay))
                            {
                                lastEndPreviousDay = pairsList[i].EndTime;
                                continue;
                            }
                        }
                    }

                    if (pairsList[i].IOPairDate.Date.Equals(weekEnd.Date))
                    {
                        if ((pairsList[i].StartTime.Hour == 0 && pairsList[i].StartTime.Minute == 0) || pairsList[i].StartTime.Equals(lastEndPreviousDay))
                        {
                            lastEndPreviousDay = pairsList[i].EndTime;
                            weekPairs.Add(pairsList[i]);
                            continue;
                        }
                        else
                            continue;
                    }

                    weekPairs.Add(pairsList[i]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return weekPairs;
        }

        public int getCollectiveAnnualLeaves(int emplID, int ptID, DateTime fromDate, List<DateTime> exceptDates)
        {
            DataSet dataSet = new DataSet();            
            string select;
            int numOfDays = 0;

            try
            {
                if (emplID == -1 || ptID == -1)
                    return numOfDays;

                select = "SELECT * FROM io_pairs_processed WHERE start_time <= end_time AND employee_id = '" + emplID.ToString().Trim() + "' AND pass_type_id = '" + ptID.ToString().Trim() + "'";

                if (!fromDate.Equals(new DateTime()))
                    select += " AND io_pair_date >= '" + fromDate.Date.ToString(dateTimeformat) + "'";

                if (exceptDates.Count > 0)
                {
                    select += " AND io_pair_date NOT IN (";
                    foreach (DateTime date in exceptDates)
                    {
                        select += "'" + date.Date.ToString(dateTimeformat) + "', ";
                    }
                    select = select.Substring(0, select.Length - 2);
                    select += ")";
                }
                
                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        DateTime startTime = new DateTime();
                        if (row["start_time"] != DBNull.Value)
                        {
                            startTime = (DateTime)row["start_time"];
                        }                        

                        if (startTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            numOfDays++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return numOfDays;
        }

        public int getEarnedHours(int emplID, int ptID, DateTime fromDate, DateTime exceptDate)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            string select;
            int duration = 0;

            try
            {
                select = "SELECT * FROM io_pairs_processed WHERE start_time <= end_time AND employee_id = '" + emplID.ToString().Trim() + "' AND pass_type_id = '" + ptID.ToString().Trim()
                    + "' AND io_pair_date >= '" + fromDate.ToString(dateTimeformat) + "'";

                if (!exceptDate.Equals(new DateTime()))
                    select += " AND io_pair_date <> '" + exceptDate.Date.ToString(dateTimeformat) + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = (DateTime)row["start_time"];
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = (DateTime)row["end_time"];
                        }

                        duration += (int)pair.EndTime.TimeOfDay.Subtract(pair.StartTime.TimeOfDay).TotalMinutes;

                        if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                            duration++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return duration;
        }
                
        public int getUsedHours(int emplID, int ptID, int ptRounding, DateTime fromDate, DateTime exceptDate)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            string select;
            int duration = 0;

            try
            {
                select = "SELECT * FROM io_pairs_processed WHERE start_time <= end_time AND employee_id = '" + emplID.ToString().Trim() + "' AND pass_type_id = '" + ptID.ToString().Trim()
                    + "' AND io_pair_date >= '" + fromDate.ToString(dateTimeformat) + "'";

                if (!exceptDate.Equals(new DateTime()))
                    select += " AND io_pair_date <> '" + exceptDate.Date.ToString(dateTimeformat) + "'";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
                        if (row["start_time"] != DBNull.Value)
                        {
                            pair.StartTime = (DateTime)row["start_time"];
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            pair.EndTime = (DateTime)row["end_time"];
                        }

                        int pairDuration = (int)pair.EndTime.TimeOfDay.Subtract(pair.StartTime.TimeOfDay).TotalMinutes;

                        if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                            pairDuration++;

                        if (pairDuration % ptRounding != 0)
                            pairDuration += ptRounding - (pairDuration % ptRounding);

                        duration += pairDuration;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return duration;
        }

        public Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getManualCreatedPairs(Dictionary<int, List<DateTime>> emplDateList)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
          
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip WHERE (");
                foreach (int empl in emplDateList.Keys)
                {
                    sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")) OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2);
                //select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                select += ") AND manual_created = " + Constants.yesInt.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }
       /* public Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getManualCreatedPairs(Dictionary<int, List<DateTime>> emplDateList, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip WHERE (");
                foreach (int empl in emplDateList.Keys)
                {
                    sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")) OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2);
                //select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                select += ") AND manual_created = " + Constants.yesInt.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn,(MySqlTransaction)trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }*/
       
        //natalija08112017 procesirani parovi, bez celodnevnih odsustva
        public Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getManualCreatedPairs(Dictionary<int, List<DateTime>> emplDateList, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                //sb.Append(" FROM io_pairs_processed ip WHERE (");
                sb.Append(" FROM io_pairs_processed ip JOIN pass_types pt ON ip.pass_type_id = pt.pass_type_id WHERE (");//IZMENA
                foreach (int empl in emplDateList.Keys)
                {
                    //sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    sb.Append(" ( ip.employee_id = " + empl + " AND pt.pass_type != " + 2 + " AND ip.io_pair_date IN (");//IZMENA
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(") AND io_pair_date IN (SELECT io_pair_date FROM io_pairs_processed WHERE employee_id = " + empl + " AND manual_created = " + Constants.yesInt + ")");
                    sb.Append(") OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2) + ")";
                // select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                //select += ") AND manual_created = " + Constants.yesInt.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }

        //natalija08112017 procesirani parovi, samo celodnevna odsustva
        public Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getManualCreatedPairsWholeDayAbsence(Dictionary<int, List<DateTime>> emplDateList, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                //sb.Append(" FROM io_pairs_processed ip WHERE (");
                sb.Append(" FROM io_pairs_processed ip JOIN pass_types pt ON ip.pass_type_id = pt.pass_type_id WHERE (");//IZMENA
                foreach (int empl in emplDateList.Keys)
                {
                    //sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    sb.Append(" ( ip.employee_id = " + empl + " AND pt.pass_type = " + 2 + " AND ip.io_pair_date IN (");//IZMENA
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(") AND io_pair_date IN (SELECT io_pair_date FROM io_pairs_processed WHERE employee_id = " + empl + " AND manual_created = " + Constants.yesInt + ")");
                    sb.Append(") OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2) + ")";
                // select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                //select += ") AND manual_created = " + Constants.yesInt.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);


                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }
        

        public  List<IOPairProcessedTO> getProcessedPairs(Dictionary<int, List<DateTime>> emplDateList)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip WHERE (");
                foreach (int empl in emplDateList.Keys)
                {
                    sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")) OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2);
                //select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                select += ") " ;

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                       
                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }

        public List<IOPairProcessedTO> getProcessedPairs(Dictionary<int, List<DateTime>> emplDateList, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip WHERE (");
                foreach (int empl in emplDateList.Keys)
                {
                    sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")) OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2);
                //select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                select += ") ";

                MySqlCommand cmd = new MySqlCommand(select, conn,(MySqlTransaction) trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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

                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }

        public bool getManualCreatedPairsDayBefore(Dictionary<int, List<DateTime>> emplDateList,Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip WHERE ");
                sb.Append(" DATE_FORMAT(start_time,'%H:%i') > '12:00'");
                foreach (int empl in emplDateList.Keys)
                {
                    sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")) OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2);
                //select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                select += ") AND manual_created = " + Constants.yesInt.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        public bool getManualCreatedPairsDayBefore(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip WHERE ");
                sb.Append(" DATE_FORMAT(start_time,'%H:%i') > '12:00'");
                foreach (int empl in emplDateList.Keys)
                {
                    sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")) OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2);
                //select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                select += ") AND manual_created = " + Constants.yesInt.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public bool getManualCreatedPairsDayAfter(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip WHERE ");
                sb.Append(" DATE_FORMAT(start_time,'%H:%i') <= '12:00' AND (");
                foreach (int empl in emplDateList.Keys)
                {
                    sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")) OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2);
                //select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                select += ") AND manual_created = " + Constants.yesInt.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public DateTime getMaxDateOfPair(string employeeID, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            DateTime pairDate = new DateTime();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT MAX io_pair_date from io_pairs_processed where employee_id IN ( " + employeeID + " )");

                string select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    if(row["io_pair_date"] != DBNull.Value)
                    pairDate = DateTime.Parse(row["io_pair_date"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairDate;
        }
        public bool getManualCreatedPairsDayAfter(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsList, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip WHERE ");
                sb.Append(" DATE_FORMAT(start_time,'%H:%i') <= '12:00' AND (");
                foreach (int empl in emplDateList.Keys)
                {
                    sb.Append(" ( employee_id = " + empl + " AND io_pair_date IN (");
                    foreach (DateTime dt in emplDateList[empl])
                    {
                        sb.Append("'" + dt.ToString("yyy-MM-dd HH:mm:ss") + "', ");
                    }
                    sb.Remove(sb.ToString().Length - 2, 2);
                    sb.Append(")) OR");
                }
                string select = sb.ToString().Substring(0, sb.ToString().Length - 2);
                //select += " AND alert IN (" + Constants.alertStatusLeavePair.ToString() + ", " + Constants.alertStatusNoAlert.ToString() + ", " + Constants.alertStatus.ToString() + ")  AND manual_created = " + Constants.yesInt.ToString();
                select += ") AND manual_created = " + Constants.yesInt.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction)trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (row["created_by"] != DBNull.Value)
                        {
                            pair.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            pair.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
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
                        if (!pairsList.ContainsKey(pair.EmployeeID))
                            pairsList.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!pairsList[pair.EmployeeID].ContainsKey(pair.IOPairDate))
                            pairsList[pair.EmployeeID].Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                        pairsList[pair.EmployeeID][pair.IOPairDate].Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public void getDatesForEmplWithNoPairs(DateTime startIntervalTime, DateTime endIntervalTime, Dictionary<int, List<DateTime>> dict)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ip.*");
                sb.Append(" FROM io_pairs_processed ip ");
                sb.Append(" WHERE io_pair_date > = '" + startIntervalTime.ToString(dateTimeformat) + "' AND io_pair_date <= '" + endIntervalTime.ToString(dateTimeformat) + "' ");               
                sb.Append(" ORDER BY ip.employee_id, ip.io_pair_date, ip.start_time");
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        if (dict.ContainsKey(pair.EmployeeID))
                        {
                            if (dict[pair.EmployeeID].Contains(pair.IOPairDate))
                                dict[pair.EmployeeID].Remove(pair.IOPairDate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<IOPairProcessedTO> getIOPairsAllForEmpl(string employeeIDString, List<DateTime> datesList, string ptIDs)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();
            string select;

            try
            {
                if (!employeeIDString.Trim().Equals("") || datesList.Count > 0 || !ptIDs.Trim().Equals(""))
                {
                    select = "SELECT * FROM io_pairs_processed WHERE start_time <= end_time";

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

                    if (!ptIDs.Trim().Equals(""))
                    {
                        select += " AND pass_type_id IN (" + ptIDs + ") ";
                    }

                    select += " ORDER BY employee_id, io_pair_date, start_time";
                    
                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                    DataTable table = dataSet.Tables["IOPairsProcessed"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pair = new IOPairProcessedTO();
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
                            pairsList.Add(pair);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }

        public List<IOPairProcessedTO> getIOPairsAllForEmpl(string employeeIDString, DateTime from, DateTime to, string ptIDs)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();
            string select;

            try
            {
                select = "SELECT * FROM io_pairs_processed WHERE start_time <= end_time";

                if (!employeeIDString.Trim().Equals(""))                
                    select += " AND employee_id IN (" + employeeIDString + ") ";

                if (!from.Equals(new DateTime()))
                    select += " AND io_pair_date >= '" + from.Date.ToString(dateTimeformat) + "'";

                if (!to.Equals(new DateTime()))
                    select += " AND io_pair_date < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "'";

                if (!ptIDs.Trim().Equals(""))                
                    select += " AND pass_type_id IN (" + ptIDs + ") ";                

                select += " ORDER BY employee_id, io_pair_date, start_time";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                DataTable table = dataSet.Tables["IOPairsProcessed"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        pair = new IOPairProcessedTO();
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
                        pairsList.Add(pair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
        }

        public List<IOPairProcessedTO> getIOPairsToVerifyForEmpl(string employeeIDString, List<DateTime> datesList, string ptIDs)
        {
            DataSet dataSet = new DataSet();
            IOPairProcessedTO pair = new IOPairProcessedTO();
            List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();
            string select;

            try
            {
                if (!employeeIDString.Trim().Equals("") || datesList.Count > 0 || !ptIDs.Trim().Equals(""))
                {
                    select = "SELECT * FROM io_pairs_processed WHERE start_time <= end_time AND verification_flag = '" + ((int)Constants.Verification.NotVerified).ToString() + "'";

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

                    if (!ptIDs.Trim().Equals(""))
                    {
                        select += " AND pass_type_id IN (" + ptIDs + ")";
                    }

                    select += " ORDER BY employee_id, io_pair_date, start_time";

                    MySqlCommand cmd = new MySqlCommand(select, conn);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                    sqlDataAdapter.Fill(dataSet, "IOPairsProcessed");
                    DataTable table = dataSet.Tables["IOPairsProcessed"];

                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            pair = new IOPairProcessedTO();
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
                            pairsList.Add(pair);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pairsList;
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

        //nije odradjena mysql verzija
        public List<IOPairProcessedTO> getProcessedPairsTypesForMonthlyReports(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        //Nije implementirana MySQL verzija
        public List<IOPairProcessedTO> getProcessedPairsByPassType(DateTime from, DateTime to, string tipProlaska)
        {
            throw new NotImplementedException();
        }

        //  24.05.2019. Nije implementirana MySQL verzija
        public List<IOPairProcessedTO> getIOPairsWithManualCreatedByEmployee(string employeeIDString, DateTime from, DateTime to) 
        {
            throw new NotImplementedException();
        }
        public int BankHoursMonthly(int emplID, DateTime month)
        {
            return 0;
        }
        public int BankHoursPeriodical(int emplID, DateTime fromDate, DateTime toDate)
        {
            return 0;
        }
        public int BankHours6Months(int emplID, DateTime month)
        {
            return 0;
        }
        public int radneSubote(int emplID, DateTime month)
        {
            return 0;
        }
        public void BankHours6MonthsPay(int emplID, DateTime mesec)
        {

        }
        public List<IOPairProcessedTO> getIOPairsAllForEmpl(string employeeIDString, List<DateTime> datesList, string ptIDs, DateTime lastDate)
        {
            return new List<IOPairProcessedTO>();
        }
        public List<IOPairProcessedTO> pairsForPeriod(string emplIDs, DateTime from, DateTime to)
        {
            return new List<IOPairProcessedTO>();
        }
        public List<IOPairProcessedTO> pairsForPeriod(string emplIDs, DateTime from, DateTime to, string passType)
        {
            return new List<IOPairProcessedTO>();
        }

        public void deleteIoPairProcForAutoTimeSchema(int emplID, DateTime date, bool doCommit)
        { }

    }
}
