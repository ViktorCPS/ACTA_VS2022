using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using TransferObjects;

namespace DataAccess
{
    public class MySQLSyncFinancialStrictureHistDAO:SyncFinancialStructureHistDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MySQLSyncFinancialStrictureHistDAO()
		{
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MySQLSyncFinancialStrictureHistDAO(MySqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(SyncFinancialStructureTO syncFS, bool doCommit)
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
                sbInsert.Append("INSERT INTO sync_financial_structure_hist ");
                sbInsert.Append("(rec_id, unit_id, unit_stringone, description, valid_from, result, remark,status, created_by, created_time, created_time_hist) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + syncFS.RecID + ",");
                sbInsert.Append(" " + syncFS.UnitID + ",");
                if (syncFS.UnitStringone != "")
                {
                    sbInsert.Append(" '" + syncFS.UnitStringone + "',");
                } 
                //if (syncFS.UnitStringoneValidFrom != new DateTime())
                //{
                //    sbInsert.Append(" '" + syncFS.UnitStringoneValidFrom.ToString(dateTimeformat) + "',");
                //}
                if (syncFS.Description != "")
                {
                    sbInsert.Append(" N'" + syncFS.Description + "',");
                }
                if (syncFS.ValidFrom != new DateTime())
                {
                    sbInsert.Append(" '" + syncFS.ValidFrom.ToString(dateTimeformat) + "',");
                }
                if (syncFS.Result != -1)
                {
                    sbInsert.Append(" " + syncFS.Result + ",");
                } 
                if (syncFS.Remark != "")
                {
                    sbInsert.Append(" N'" + syncFS.Remark + "',");
                } 
                if (syncFS.Status != "")
                {
                    sbInsert.Append(" '" + syncFS.Status + "',");
                }
                if (syncFS.CreatedBy != "")
                {
                    sbInsert.Append(" N'" + syncFS.CreatedBy + "',");
                }
                if (syncFS.CreatedTime != new DateTime())
                {
                    sbInsert.Append(" '" + syncFS.CreatedTime.ToString(dateTimeformat) + "',");
                }
                sbInsert.Append(" ', NOW()) ");

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

        public List<SyncFinancialStructureTO> getFS(DateTime from, DateTime to, int fsID, int result)
        {
            DataSet dataSet = new DataSet();
            SyncFinancialStructureTO fsTO = new SyncFinancialStructureTO();
            List<SyncFinancialStructureTO> fsList = new List<SyncFinancialStructureTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM sync_financial_structure_hist ");

                if (!from.Equals(new DateTime()) || !to.Equals(new DateTime()) || fsID != -1 || result != -1)
                {
                    sb.Append("WHERE ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("created_time_hist >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (!to.Equals(new DateTime()))
                        sb.Append("created_time_hist < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    if (fsID != -1)
                        sb.Append("unit_id = '" + fsID.ToString().Trim() + "' AND ");

                    if (result != -1)
                        sb.Append("result = '" + result.ToString().Trim() + "' AND ");

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                    select = sb.ToString();

                select = select + "ORDER BY created_time_hist ";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "FS");
                DataTable table = dataSet.Tables["FS"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        fsTO = new SyncFinancialStructureTO();

                        if (!row["unit_id"].Equals(DBNull.Value))
                        {
                            fsTO.UnitID = int.Parse(row["unit_id"].ToString().Trim());
                        }
                        if (!row["company_code"].Equals(DBNull.Value))
                        {
                            fsTO.CompanyCode = row["company_code"].ToString().Trim();
                        }
                        if (!row["unit_stringone"].Equals(DBNull.Value))
                        {
                            fsTO.UnitStringone = row["unit_stringone"].ToString().Trim();
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            fsTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            fsTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["valid_from"].Equals(DBNull.Value))
                        {
                            fsTO.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            fsTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            fsTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["created_time_hist"].Equals(DBNull.Value))
                        {
                            fsTO.CreatedTimeHist = DateTime.Parse(row["created_time_hist"].ToString().Trim());
                        }
                        if (!row["result"].Equals(DBNull.Value))
                        {
                            fsTO.Result = int.Parse(row["result"].ToString().Trim());
                        }
                        if (!row["remark"].Equals(DBNull.Value))
                        {
                            fsTO.Remark = row["remark"].ToString().Trim();
                        }

                        fsList.Add(fsTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return fsList;
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
