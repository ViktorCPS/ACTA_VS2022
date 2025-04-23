using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using TransferObjects;

namespace DataAccess
{
   public class MSSQLSyncResponsibilityHistDAO:SyncResponsibilityHistDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MSSQLSyncResponsibilityHistDAO()
		{
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLSyncResponsibilityHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(SyncResponsibilityTO syncFS, bool doCommit)
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
                sbInsert.Append("INSERT INTO sync_responsibility_hist ");
                sbInsert.Append("(rec_id, unit_id, responsible_person_id,  structure_type, ");
                sbInsert.Append("valid_from, valid_to, result, remark, created_by, created_time, created_time_hist) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + syncFS.RecID + ",");
                sbInsert.Append(" " + syncFS.UnitID + ",");
                if (syncFS.ResponsiblePersonID != -1)
                {
                    sbInsert.Append(" '" + syncFS.ResponsiblePersonID + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncFS.StructureType != "")
                {
                    sbInsert.Append(" N'" + syncFS.StructureType + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncFS.ValidFrom != new DateTime())
                {
                    sbInsert.Append(" N'" + syncFS.ValidFrom.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncFS.ValidTo != new DateTime())
                {
                    sbInsert.Append(" N'" + syncFS.ValidTo.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncFS.Result != -1)
                {
                    sbInsert.Append(" " + syncFS.Result + ",");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncFS.Remark != "")
                {
                    sbInsert.Append(" N'" + syncFS.Remark + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncFS.CreatedBy != "")
                {
                    sbInsert.Append(" N'" + syncFS.CreatedBy + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncFS.CreatedTime != new DateTime())
                {
                    sbInsert.Append(" '" + syncFS.CreatedTime.ToString(dateTimeformat) + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
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

        public List<SyncResponsibilityTO> getResponsibilities(DateTime from, DateTime to, int resID, int result)
        {
            DataSet dataSet = new DataSet();
            SyncResponsibilityTO resTO = new SyncResponsibilityTO();
            List<SyncResponsibilityTO> resList = new List<SyncResponsibilityTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM sync_responsibility_hist ");

                if (!from.Equals(new DateTime()) || !to.Equals(new DateTime()) || resID != -1 || result != -1)
                {
                    sb.Append("WHERE ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("created_time_hist >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (!to.Equals(new DateTime()))
                        sb.Append("created_time_hist < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    if (resID != -1)
                        sb.Append("responsible_person_id = '" + resID.ToString().Trim() + "' AND ");

                    if (result != -1)
                        sb.Append("result = '" + result.ToString().Trim() + "' AND ");

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                    select = sb.ToString();

                select = select + "ORDER BY created_time_hist ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "RES");
                DataTable table = dataSet.Tables["RES"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        resTO = new SyncResponsibilityTO();
                        
                        if (!row["unit_id"].Equals(DBNull.Value))
                        {
                            resTO.UnitID = int.Parse(row["unit_id"].ToString().Trim());
                        }
                        if (!row["responsible_person_id"].Equals(DBNull.Value))
                        {
                            resTO.ResponsiblePersonID = int.Parse(row["responsible_person_id"].ToString().Trim());
                        }
                        if (!row["structure_type"].Equals(DBNull.Value))
                        {
                            resTO.StructureType = row["structure_type"].ToString().Trim();
                        }                        
                        if (!row["valid_from"].Equals(DBNull.Value))
                        {
                            resTO.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (!row["valid_to"].Equals(DBNull.Value))
                        {
                            resTO.ValidTo = DateTime.Parse(row["valid_to"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            resTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            resTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["created_time_hist"].Equals(DBNull.Value))
                        {
                            resTO.CreatedTimeHist = DateTime.Parse(row["created_time_hist"].ToString().Trim());
                        }
                        if (!row["result"].Equals(DBNull.Value))
                        {
                            resTO.Result = int.Parse(row["result"].ToString().Trim());
                        }
                        if (!row["remark"].Equals(DBNull.Value))
                        {
                            resTO.Remark = row["remark"].ToString().Trim();
                        }

                        resList.Add(resTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return resList;
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
