using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using TransferObjects;

namespace DataAccess
{
    public class MSSQLSyncOrganizationalStructureHistDAO:SyncOrganizationalStructureHistDAO
    {
           SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MSSQLSyncOrganizationalStructureHistDAO()
		{
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLSyncOrganizationalStructureHistDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public bool insert(SyncOrganizationalStructureTO syncFS, bool doCommit)
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
                sbInsert.Append("INSERT INTO sync_organizational_structure_hist ");
                //sbInsert.Append("(rec_id, unit_id, parent_unit_id, parent_unit_id_valid_from, description, description_valid_from,");
                //sbInsert.Append("code, code_valid_from, cost_center_stringone, cost_center_stringone_valid_from,cost_center_company_code, result, remark, created_by, created_time, created_time_hist) ");
                sbInsert.Append("(rec_id, unit_id, parent_unit_id,  description, ");
                sbInsert.Append("code, cost_center_code, cost_center_company_code, valid_from, result, remark,status, created_by, created_time, created_time_hist) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(" " + syncFS.RecID + ",");
                sbInsert.Append(" " + syncFS.UnitID + ",");
                if (syncFS.ParentUnitID != -1)
                {
                    sbInsert.Append(" '" + syncFS.ParentUnitID + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                //if (syncFS.ParentUnitIDValidFrom != new DateTime())
                //{
                //    sbInsert.Append(" '" + syncFS.ParentUnitIDValidFrom.ToString(dateTimeformat) + "',");
                //}
                //else
                //{
                //    sbInsert.Append("NULL, ");
                //}
                if (syncFS.Description != "")
                {
                    sbInsert.Append(" N'" + syncFS.Description + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                //if (syncFS.DescriptionValidFrom != new DateTime())
                //{
                //    sbInsert.Append(" '" + syncFS.DescriptionValidFrom.ToString(dateTimeformat) + "',");
                //}
                //else
                //{
                //    sbInsert.Append("NULL, ");
                //}
                if (syncFS.Code != "")
                {
                    sbInsert.Append(" N'" + syncFS.Code + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                //if (syncFS.CodeValidFrom != new DateTime())
                //{
                //    sbInsert.Append(" '" + syncFS.CodeValidFrom.ToString(dateTimeformat) + "',");
                //}
                //else
                //{
                //    sbInsert.Append("NULL, ");
                //}
                if (syncFS.CostCenterStringone != "")
                {
                    sbInsert.Append(" N'" + syncFS.CostCenterStringone + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                //if (syncFS.CostCenterStringoneValidFrom != new DateTime())
                //{
                //    sbInsert.Append(" '" + syncFS.CostCenterStringoneValidFrom.ToString(dateTimeformat) + "',");
                //}
                //else
                //{
                //    sbInsert.Append("NULL, ");
                //}
                if (syncFS.CompanyCode != "")
                {
                    sbInsert.Append(" N'" + syncFS.CompanyCode + "',");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (syncFS.ValidFrom != new DateTime())
                {
                    sbInsert.Append(" '" + syncFS.ValidFrom.ToString(dateTimeformat) + "',");
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
                if (syncFS.Status != "")
                {
                    sbInsert.Append(" N'" + syncFS.Status + "',");
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

        public List<SyncOrganizationalStructureTO> getOU(DateTime from, DateTime to, int ouID, int result)
        {
            DataSet dataSet = new DataSet();
            SyncOrganizationalStructureTO ouTO = new SyncOrganizationalStructureTO();
            List<SyncOrganizationalStructureTO> ouList = new List<SyncOrganizationalStructureTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM sync_organizational_structure_hist ");

                if (!from.Equals(new DateTime()) || !to.Equals(new DateTime()) || ouID != -1 || result != -1)
                {
                    sb.Append("WHERE ");

                    if (!from.Equals(new DateTime()))
                        sb.Append("created_time_hist >= '" + from.Date.ToString(dateTimeformat) + "' AND ");

                    if (!to.Equals(new DateTime()))
                        sb.Append("created_time_hist < '" + to.Date.AddDays(1).ToString(dateTimeformat) + "' AND ");

                    if (ouID != -1)
                        sb.Append("unit_id = '" + ouID.ToString().Trim() + "' AND ");

                    if (result != -1)
                        sb.Append("result = '" + result.ToString().Trim() + "' AND ");

                    select = sb.ToString(0, sb.ToString().Length - 4);
                }
                else
                    select = sb.ToString();

                select = select + "ORDER BY created_time_hist ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "OU");
                DataTable table = dataSet.Tables["OU"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        ouTO = new SyncOrganizationalStructureTO();

                        if (!row["unit_id"].Equals(DBNull.Value))
                        {
                            ouTO.UnitID = int.Parse(row["unit_id"].ToString().Trim());
                        }
                        if (!row["parent_unit_id"].Equals(DBNull.Value))
                        {
                            ouTO.ParentUnitID = int.Parse(row["parent_unit_id"].ToString().Trim());
                        }
                        if (!row["description"].Equals(DBNull.Value))
                        {
                            ouTO.Description = row["description"].ToString().Trim();
                        }
                        if (!row["code"].Equals(DBNull.Value))
                        {
                            ouTO.Code = row["code"].ToString().Trim();
                        }
                        if (!row["cost_center_code"].Equals(DBNull.Value))
                        {
                            ouTO.CostCenterStringone = row["cost_center_code"].ToString().Trim();
                        }
                        if (!row["cost_center_company_code"].Equals(DBNull.Value))
                        {
                            ouTO.CompanyCode = row["cost_center_company_code"].ToString().Trim();
                        }                        
                        if (!row["status"].Equals(DBNull.Value))
                        {
                            ouTO.Status = row["status"].ToString().Trim();
                        }
                        if (!row["valid_from"].Equals(DBNull.Value))
                        {
                            ouTO.ValidFrom = DateTime.Parse(row["valid_from"].ToString().Trim());
                        }
                        if (!row["created_by"].Equals(DBNull.Value))
                        {
                            ouTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (!row["created_time"].Equals(DBNull.Value))
                        {
                            ouTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (!row["created_time_hist"].Equals(DBNull.Value))
                        {
                            ouTO.CreatedTimeHist = DateTime.Parse(row["created_time_hist"].ToString().Trim());
                        }
                        if (!row["result"].Equals(DBNull.Value))
                        {
                            ouTO.Result = int.Parse(row["result"].ToString().Trim());
                        }
                        if (!row["remark"].Equals(DBNull.Value))
                        {
                            ouTO.Remark = row["remark"].ToString().Trim();
                        }

                        ouList.Add(ouTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ouList;
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
