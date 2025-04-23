using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLPassTypeLimitDAO : PassTypeLimitDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string datetimeFormat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLPassTypeLimitDAO()
		{
			conn = MySQLDAOFactory.getConnection();            
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            datetimeFormat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLPassTypeLimitDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            datetimeFormat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(PassTypeLimitTO ptLimitTO)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO pass_type_limits ");
                sbInsert.Append("(pass_type_limit_id, limit_type, limit_value, limit_measure_unit, limit_period, limit_start_date,name, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + ptLimitTO.PtLimitID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + ptLimitTO.Type.Trim() + "', ");
                sbInsert.Append("'" + ptLimitTO.Value.ToString().Trim() + "', ");
                sbInsert.Append("N'" + ptLimitTO.MeasureUnit.Trim() + "', ");
                sbInsert.Append("'" + ptLimitTO.Period.ToString().Trim() + "', ");
                sbInsert.Append("'" + ptLimitTO.StartDate.ToString(datetimeFormat) + "', ");
                sbInsert.Append("N'" + ptLimitTO.Name.Trim() + "', ");
                                
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

        public bool delete(int limitID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM pass_type_limits WHERE pass_type_limit_id = '" + limitID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;                
            }

            return isDeleted;
        }

        public bool update(PassTypeLimitTO ptLimitTO)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE pass_type_limits SET ");
                if (!ptLimitTO.Type.Trim().Equals(""))
                {
                    sbUpdate.Append("limit_type = N'" + ptLimitTO.Type.Trim() + "', ");
                }
                if (!ptLimitTO.Name.Trim().Equals(""))
                {
                    sbUpdate.Append("name = N'" + ptLimitTO.Name.Trim() + "', ");
                }
                if (ptLimitTO.Value != -1)
                {
                    sbUpdate.Append("limit_value = '" + ptLimitTO.Value.ToString().Trim() + "', ");
                }
                if (!ptLimitTO.MeasureUnit.Trim().Equals(""))
                {
                    sbUpdate.Append("limit_measure_unit = N'" + ptLimitTO.MeasureUnit.Trim() + "', ");
                }
                if (ptLimitTO.Period != -1)
                {
                    sbUpdate.Append("limit_period = '" + ptLimitTO.Period.ToString().Trim() + "', ");
                }
                if (!ptLimitTO.StartDate.Equals(new DateTime()))
                {
                    sbUpdate.Append("limit_start_date = '" + ptLimitTO.StartDate.ToString(datetimeFormat) + "', ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE pass_type_limit_id = '" + ptLimitTO.PtLimitID.ToString().Trim() + "'");

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
                throw ex;
            }

            return isUpdated;
        }

        public PassTypeLimitTO find(int limitID)
        {
            DataSet dataSet = new DataSet();
            PassTypeLimitTO limitTO = new PassTypeLimitTO();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM pass_type_limits WHERE pass_type_limit_id = '" + limitID.ToString().Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Types");
                DataTable table = dataSet.Tables["Types"];

                if (table.Rows.Count == 1)
                {
                    limitTO = new PassTypeLimitTO();

                        limitTO.PtLimitID = Int32.Parse(table.Rows[0]["pass_type_limit_id"].ToString().Trim());

                        if (!table.Rows[0]["limit_type"].Equals(DBNull.Value))
                        {
                            limitTO.Type = table.Rows[0]["limit_type"].ToString().Trim();
                        }
                        if (!table.Rows[0]["name"].Equals(DBNull.Value))
                        {
                            limitTO.Name = table.Rows[0]["name"].ToString().Trim();
                        }
                        if (!table.Rows[0]["limit_value"].Equals(DBNull.Value))
                        {
                            limitTO.Value = Int32.Parse(table.Rows[0]["limit_value"].ToString().Trim());
                        }
                        if (!table.Rows[0]["limit_measure_unit"].Equals(DBNull.Value))
                        {
                            limitTO.MeasureUnit = table.Rows[0]["limit_measure_unit"].ToString().Trim();
                        }
                        if (!table.Rows[0]["limit_period"].Equals(DBNull.Value))
                        {
                            limitTO.Period = Int32.Parse(table.Rows[0]["limit_period"].ToString().Trim());
                        }
                        if (!table.Rows[0]["limit_start_date"].Equals(DBNull.Value))
                        {
                            limitTO.StartDate = DateTime.Parse(table.Rows[0]["limit_start_date"].ToString().Trim());
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return limitTO;
        }
        public Dictionary<int, PassTypeLimitTO> getPassTypeLimitsDictionary(PassTypeLimitTO ptLimitTO)
        {
            DataSet dataSet = new DataSet();
            PassTypeLimitTO limitTO = new PassTypeLimitTO();
            Dictionary<int, PassTypeLimitTO> limitsList = new Dictionary<int, PassTypeLimitTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_type_limits ");
                if ((ptLimitTO.PtLimitID != -1) || (!ptLimitTO.Type.Trim().Equals("")) || (!ptLimitTO.Name.Trim().Equals("")) || (ptLimitTO.Value != -1) || (!ptLimitTO.MeasureUnit.Trim().Equals(""))
                    || (ptLimitTO.Period != -1) || (!ptLimitTO.StartDate.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");
                    if (ptLimitTO.PtLimitID != -1)
                    {
                        sb.Append(" pass_type_limit_id = '" + ptLimitTO.PtLimitID.ToString().Trim() + "' AND");
                    }
                    if (!ptLimitTO.Type.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(limit_type) = N'" + ptLimitTO.Type.ToUpper().Trim() + "' AND");
                    }
                    if (!ptLimitTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" name = N'" + ptLimitTO.Name.ToUpper().Trim() + "' AND");
                    }
                    if (ptLimitTO.Value != -1)
                    {
                        sb.Append(" limit_value = '" + ptLimitTO.Value.ToString().Trim() + "' AND");
                    }
                    if (!ptLimitTO.MeasureUnit.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(limit_measure_unit) = N'" + ptLimitTO.MeasureUnit.ToUpper().Trim() + "' AND");
                    }
                    if (ptLimitTO.Period != -1)
                    {
                        sb.Append(" limit_period = '" + ptLimitTO.Period.ToString().Trim() + "' AND");
                    }
                    if (!ptLimitTO.StartDate.Equals(new DateTime()))
                    {
                        sb.Append(" limit_start_date = '" + ptLimitTO.StartDate.ToString(datetimeFormat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY limit_type ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Types");
                DataTable table = dataSet.Tables["Types"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        limitTO = new PassTypeLimitTO();

                        limitTO.PtLimitID = Int32.Parse(row["pass_type_limit_id"].ToString().Trim());

                        if (!row["limit_type"].Equals(DBNull.Value))
                        {
                            limitTO.Type = row["limit_type"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            limitTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["limit_value"].Equals(DBNull.Value))
                        {
                            limitTO.Value = Int32.Parse(row["limit_value"].ToString().Trim());
                        }
                        if (!row["limit_measure_unit"].Equals(DBNull.Value))
                        {
                            limitTO.MeasureUnit = row["limit_measure_unit"].ToString().Trim();
                        }
                        if (!row["limit_period"].Equals(DBNull.Value))
                        {
                            limitTO.Period = Int32.Parse(row["limit_period"].ToString().Trim());
                        }
                        if (!row["limit_start_date"].Equals(DBNull.Value))
                        {
                            limitTO.StartDate = DateTime.Parse(row["limit_start_date"].ToString().Trim());
                        }
                        if(!limitsList.ContainsKey(limitTO.PtLimitID))
                        limitsList.Add(limitTO.PtLimitID,limitTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return limitsList;
        }
        public List<PassTypeLimitTO> getPassTypeLimits(PassTypeLimitTO ptLimitTO)
        {
            DataSet dataSet = new DataSet();
            PassTypeLimitTO limitTO = new PassTypeLimitTO();
            List<PassTypeLimitTO> limitsList = new List<PassTypeLimitTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_type_limits ");
                if ((ptLimitTO.PtLimitID != -1) || (!ptLimitTO.Type.Trim().Equals("")) || (!ptLimitTO.Name.Trim().Equals("")) || (ptLimitTO.Value != -1) || (!ptLimitTO.MeasureUnit.Trim().Equals(""))
                    || (ptLimitTO.Period != -1) || (!ptLimitTO.StartDate.Equals(new DateTime())))
                {
                    sb.Append(" WHERE");
                    if (ptLimitTO.PtLimitID != -1)
                    {
                        sb.Append(" pass_type_limit_id = '" + ptLimitTO.PtLimitID.ToString().Trim() + "' AND");
                    }
                    if (!ptLimitTO.Type.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(limit_type) = N'" + ptLimitTO.Type.ToUpper().Trim() + "' AND");
                    }
                    if (!ptLimitTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" name = N'" + ptLimitTO.Name.ToUpper().Trim() + "' AND");
                    }
                    if (ptLimitTO.Value != -1)
                    {
                        sb.Append(" limit_value = '" + ptLimitTO.Value.ToString().Trim() + "' AND");
                    }
                    if (!ptLimitTO.MeasureUnit.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(limit_measure_unit) = N'" + ptLimitTO.MeasureUnit.ToUpper().Trim() + "' AND");
                    }
                    if (ptLimitTO.Period != -1)
                    {
                        sb.Append(" limit_period = '" + ptLimitTO.Period.ToString().Trim() + "' AND");
                    }
                    if (!ptLimitTO.StartDate.Equals(new DateTime()))
                    {
                        sb.Append(" limit_start_date = '" + ptLimitTO.StartDate.ToString(datetimeFormat) + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY limit_type ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Types");
                DataTable table = dataSet.Tables["Types"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        limitTO = new PassTypeLimitTO();

                        limitTO.PtLimitID = Int32.Parse(row["pass_type_limit_id"].ToString().Trim());

                        if (!row["limit_type"].Equals(DBNull.Value))
                        {
                            limitTO.Type = row["limit_type"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            limitTO.Name = row["name"].ToString().Trim();
                        }
                        if (!row["limit_value"].Equals(DBNull.Value))
                        {
                            limitTO.Value = Int32.Parse(row["limit_value"].ToString().Trim());
                        }
                        if (!row["limit_measure_unit"].Equals(DBNull.Value))
                        {
                            limitTO.MeasureUnit = row["limit_measure_unit"].ToString().Trim();
                        }
                        if (!row["limit_period"].Equals(DBNull.Value))
                        {
                            limitTO.Period = Int32.Parse(row["limit_period"].ToString().Trim());
                        }
                        if (!row["limit_start_date"].Equals(DBNull.Value))
                        {
                            limitTO.StartDate = DateTime.Parse(row["limit_start_date"].ToString().Trim());
                        }

                        limitsList.Add(limitTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return limitsList;
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
                throw ex;
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
