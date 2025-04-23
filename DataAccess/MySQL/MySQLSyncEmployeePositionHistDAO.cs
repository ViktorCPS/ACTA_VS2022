using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;
using TransferObjects;

namespace DataAccess
{
    public class MySQLSyncEmployeePositionHistDAO : SyncEmployeePositionHistDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLSyncEmployeePositionHistDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MySQLSyncEmployeePositionHistDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(SyncEmployeePositionTO syncPos, bool doCommit)
        {
            if (doCommit)
            {
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                SqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO sync_employee_positions_hist ");
                sbInsert.Append("(rec_id, company_code, position_id, position_code, status, ");
                sbInsert.Append("position_title_sr, position_title_en, desc_sr, desc_en, valid_from, result, remark, created_by, created_time, created_time_hist) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + syncPos.RecID + ",");
                if (syncPos.CompanyCode.Trim() != "")
                {
                    sbInsert.Append(" N'" + syncPos.CompanyCode + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                sbInsert.Append(" '" + syncPos.PositionID.ToString().Trim() + "',");
                if (syncPos.PositionCode.Trim() != "")
                {
                    sbInsert.Append(" N'" + syncPos.PositionCode + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.Status.Trim() != "")
                {
                    sbInsert.Append(" N'" + syncPos.Status + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.PositionTitleSR.Trim() != "")
                {
                    sbInsert.Append(" N'" + syncPos.PositionTitleSR + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.PositionTitleEN.Trim() != "")
                {
                    sbInsert.Append(" N'" + syncPos.PositionTitleEN + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.DescSR.Trim() != "")
                {
                    sbInsert.Append(" N'" + syncPos.DescSR + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.DescEN.Trim() != "")
                {
                    sbInsert.Append(" N'" + syncPos.DescEN + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }                
                if (syncPos.ValidFrom != new DateTime())
                {
                    sbInsert.Append(" '" + syncPos.ValidFrom.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.Result != -1)
                {
                    sbInsert.Append(" " + syncPos.Result + ",");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.Remark != "")
                {
                    sbInsert.Append(" N'" + syncPos.Remark + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.CreatedBy != "")
                {
                    sbInsert.Append(" N'" + syncPos.CreatedBy + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncPos.CreatedTime != new DateTime())
                {
                    sbInsert.Append(" '" + syncPos.CreatedTime.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                sbInsert.Append(" NOW()) ");

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

            return rowsAffected > 0;
        }

        public List<SyncEmployeePositionTO> getPositions(DateTime from, DateTime to, int posID, int result)
        {
            DataSet dataSet = new DataSet();
            SyncEmployeePositionTO posTO = new SyncEmployeePositionTO();
            List<SyncEmployeePositionTO> posList = new List<SyncEmployeePositionTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM sync_employee_positions_hist ");

                if (!from.Equals(new DateTime()) || !to.Equals(new DateTime()) || posID != -1 || result != -1)
                {
                    sb.Append("WHERE ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("created_time_hist >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (!to.Equals(new DateTime()))
                        sb.Append("created_time_hist < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    if (posID != -1)
                        sb.Append("position_id = '" + posID.ToString().Trim() + "' AND ");

                    if (result != -1)
                        sb.Append("result = '" + result.ToString().Trim() + "' AND ");

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                    select = sb.ToString();

                select = select + "ORDER BY created_time_hist ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "POS");
                DataTable table = dataSet.Tables["POS"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        posTO = new SyncEmployeePositionTO();
                                                
                        if (!row["company_code"].Equals(DBNull.Value))
                        {
                            posTO.CompanyCode = row["company_code"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            posTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["position_id"].Equals(DBNull.Value))
                        {
                            posTO.PositionID = int.Parse(row["position_id"].ToString().Trim());
                        }
                        if (!row["position_code"].Equals(DBNull.Value))
                        {
                            posTO.PositionCode = row["position_code"].ToString().Trim();
                        }
                        if (!row["position_title_sr"].Equals(DBNull.Value))
                        {
                            posTO.PositionTitleSR = row["position_title_sr"].ToString().Trim();
                        }
                        if (!row["position_title_en"].Equals(DBNull.Value))
                        {
                            posTO.PositionTitleEN = row["position_title_en"].ToString().Trim();
                        }
                        if (!row["desc_sr"].Equals(DBNull.Value))
                        {
                            posTO.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (!row["desc_en"].Equals(DBNull.Value))
                        {
                            posTO.DescEN = row["desc_en"].ToString().Trim();
                        }
                        if (!row["valid_from"].Equals(DBNull.Value))
                        {
                            posTO.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            posTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            posTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["created_time_hist"].Equals(DBNull.Value))
                        {
                            posTO.CreatedTimeHist = DateTime.Parse(row["created_time_hist"].ToString().Trim());
                        }
                        if (!row["result"].Equals(DBNull.Value))
                        {
                            posTO.Result = int.Parse(row["result"].ToString().Trim());
                        }
                        if (!row["remark"].Equals(DBNull.Value))
                        {
                            posTO.Remark = row["remark"].ToString().Trim();
                        }

                        posList.Add(posTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return posList;
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
