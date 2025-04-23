using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using TransferObjects;
using System.Globalization;

namespace DataAccess
{
    public class MSSQLSyncCostCenterHistDAO : SyncCostCenterHistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MSSQLSyncCostCenterHistDAO()
		{
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLSyncCostCenterHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(SyncCostCenterTO syncCC, bool doCommit)
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
                sbInsert.Append("INSERT INTO sync_cost_centers_hist ");
                sbInsert.Append("(rec_id, cost_center_code, cost_center_company_code, cost_center_description, valid_from, created_by, created_time, result, remark, created_time_hist) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + syncCC.RecID + ",");
                sbInsert.Append(" N'" + syncCC.Code + "',");
                if (syncCC.CompanyCode != "")
                {
                    sbInsert.Append(" N'" + syncCC.CompanyCode + "',");
                }
                else
                {
                    sbInsert.Append(" NULL,");
                }
                if (syncCC.Desc != "")
                {
                    sbInsert.Append(" N'" + syncCC.Desc + "',");
                }
                else
                {
                    sbInsert.Append(" NULL,");
                }
                sbInsert.Append(" '" + syncCC.ValidFrom.ToString(dateTimeformat) + "',");
                sbInsert.Append(" N'" + syncCC.CreatedBy + "',");
                sbInsert.Append(" '" + syncCC.CreatedTime.ToString(dateTimeformat) + "',");
                sbInsert.Append(" '" + syncCC.Result.ToString().Trim() + "',");
                if (syncCC.Remark != "")
                {
                    sbInsert.Append(" N'" + syncCC.Remark + "',");
                }
                else
                {
                    sbInsert.Append(" NULL,");
                }
                sbInsert.Append(" GETDATE()) ");

                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();

                throw ex;
            }

            return rowsAffected > 0;
        }

        public List<SyncCostCenterTO> getCC(DateTime from, DateTime to, string code, string company, int result)
        {
            DataSet dataSet = new DataSet();
            SyncCostCenterTO ccTO = new SyncCostCenterTO();
            List<SyncCostCenterTO> ccList = new List<SyncCostCenterTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM sync_cost_centers_hist ");

                if (!from.Equals(new DateTime()) || !to.Equals(new DateTime()) || !code.Trim().Equals("") || !company.Trim().Equals("") || result != -1)
                {
                    sb.Append("WHERE ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("created_time_hist >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (!to.Equals(new DateTime()))
                        sb.Append("created_time_hist < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    if (!code.Trim().Equals(""))
                        sb.Append("cost_center_code = N'" + code.Trim() + "' AND ");

                    if (!company.Trim().Equals(""))
                        sb.Append("cost_center_company_code = N'" + company.Trim() + "' AND ");

                    if (result != -1)
                        sb.Append("result = '" + result.ToString().Trim() + "' AND ");

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                    select = sb.ToString();

                select = select + "ORDER BY created_time_hist ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "CC");
                DataTable table = dataSet.Tables["CC"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        ccTO = new SyncCostCenterTO();
                        
                        if (!row["cost_center_code"].Equals(DBNull.Value))
                        {
                            ccTO.Code = row["cost_center_code"].ToString().Trim();
                        }
                        if (!row["cost_center_company_code"].Equals(DBNull.Value))
                        {
                            ccTO.CompanyCode = row["cost_center_company_code"].ToString().Trim();
                        }
                        if (!row["cost_center_description"].Equals(DBNull.Value))
                        {
                            ccTO.Desc = row["cost_center_description"].ToString().Trim();
                        }                        
                        if (!row["valid_from"].Equals(DBNull.Value))
                        {
                            ccTO.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            ccTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            ccTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["created_time_hist"].Equals(DBNull.Value))
                        {
                            ccTO.CreatedTimeHist = DateTime.Parse(row["created_time_hist"].ToString().Trim());
                        }
                        if (!row["result"].Equals(DBNull.Value))
                        {
                            ccTO.Result = int.Parse(row["result"].ToString().Trim());
                        }
                        if (!row["remark"].Equals(DBNull.Value))
                        {
                            ccTO.Remark = row["remark"].ToString().Trim();
                        }

                        ccList.Add(ccTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ccList;
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
            _sqlTrans = (SqlTransaction)trans;
        }
    }
}
