using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLEmployeePositionDAO : EmployeePositionDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLEmployeePositionDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLEmployeePositionDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(EmployeePositionTO posTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_positions ");
                sbInsert.Append("(working_unit_id, position_id, position_code, position_title_sr, position_title_en, desc_sr, desc_en, status, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + posTO.WorkingUnitID.ToString().Trim() + "', ");                
                sbInsert.Append("'" + posTO.PositionID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + posTO.PositionCode.ToString().Trim() + "', ");
                sbInsert.Append("N'" + posTO.PositionTitleSR.ToString().Trim() + "', ");
                sbInsert.Append("N'" + posTO.PositionTitleEN.ToString().Trim() + "', ");
                if (!posTO.DescSR.Trim().Equals(""))
                    sbInsert.Append("N'" + posTO.DescSR.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!posTO.DescEN.Trim().Equals(""))
                    sbInsert.Append("N'" + posTO.DescEN.ToString().Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                sbInsert.Append("N'" + posTO.Status.ToString().Trim() + "', ");
                if (!posTO.CreatedBy.Trim().Equals(""))                
                    sbInsert.Append("N'" + posTO.CreatedBy.Trim() + "', ");                
                else                
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!posTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + posTO.CreatedTime.ToString(dateTimeformat) + "') ");
                else
                    sbInsert.Append("GETDATE()) ");

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

            return rowsAffected;
        }

        public bool update(EmployeePositionTO posTO, bool doCommit)
        {
            bool isUpdated = false;            

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_positions SET ");
                if (posTO.WorkingUnitID != -1)
                    sbUpdate.Append("working_unit_id = '" + posTO.WorkingUnitID.ToString().Trim() + "', ");                
                if (!posTO.PositionCode.Trim().Equals(""))
                    sbUpdate.Append("position_code = N'" + posTO.PositionCode.Trim() + "', ");
                if (!posTO.PositionTitleSR.Trim().Equals(""))
                    sbUpdate.Append("position_title_sr = N'" + posTO.PositionTitleSR.Trim() + "', ");
                if (!posTO.PositionTitleEN.Trim().Equals(""))
                    sbUpdate.Append("position_title_en = N'" + posTO.PositionTitleEN.Trim() + "', ");
                if (!posTO.DescSR.Trim().Equals(""))
                    sbUpdate.Append("desc_sr = N'" + posTO.DescSR.Trim() + "', ");
                else
                    sbUpdate.Append("desc_sr = NULL, ");
                if (!posTO.DescEN.Trim().Equals(""))
                    sbUpdate.Append("desc_en = N'" + posTO.DescEN.Trim() + "', ");
                else
                    sbUpdate.Append("desc_en = NULL, ");
                if (!posTO.Status.Trim().Equals(""))
                    sbUpdate.Append("status = N'" + posTO.Status.Trim() + "', ");
                if (!posTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + posTO.ModifiedBy.Trim() + "', ");                
                else                
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!posTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + posTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE position_id = '" + posTO.PositionID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool delete(int posID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM employee_positions WHERE position_id = '" + posID.ToString().Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    isDeleted = true;
                }

                if (doCommit)
                    commitTransaction();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    rollbackTransaction();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public List<EmployeePositionTO> getEmployeePositions(EmployeePositionTO posTO)
        {
            DataSet dataSet = new DataSet();
            EmployeePositionTO emplPosTO = new EmployeePositionTO();
            List<EmployeePositionTO> posList = new List<EmployeePositionTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_positions ");

                if ((posTO.WorkingUnitID != -1) || (posTO.PositionID != -1) || (!posTO.PositionCode.Trim().Equals("")) || (!posTO.PositionTitleSR.Trim().Equals(""))
                     || (!posTO.PositionTitleEN.Trim().Equals("")) || (!posTO.DescSR.Trim().Equals("")) || (!posTO.DescEN.Trim().Equals("")) || (!posTO.Status.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (posTO.WorkingUnitID != -1)
                    {
                        sb.Append(" working_unit_id = '" + posTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (posTO.PositionID != -1)
                    {
                        sb.Append(" position_id = '" + posTO.PositionID.ToString().Trim() + "' AND");
                    }
                    if (!posTO.PositionCode.Trim().Equals(""))
                    {
                        sb.Append(" position_code = N'" + posTO.PositionCode.Trim() + "' AND");
                    }
                    if (!posTO.PositionTitleSR.Trim().Equals(""))
                    {
                        sb.Append(" position_title_sr = N'" + posTO.PositionTitleSR.Trim() + "' AND");
                    }
                    if (!posTO.PositionTitleEN.Trim().Equals(""))
                    {
                        sb.Append(" position_title_en = N'" + posTO.PositionTitleEN.Trim() + "' AND");
                    }
                    if (!posTO.DescSR.Trim().Equals(""))
                    {
                        sb.Append(" desc_sr = N'" + posTO.DescSR.Trim() + "' AND");
                    }
                    if (!posTO.DescEN.Trim().Equals(""))
                    {
                        sb.Append(" desc_en = N'" + posTO.DescEN.Trim() + "' AND");
                    }
                    if (!posTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" status = N'" + posTO.Status.Trim() + "' AND");
                    }
                    
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY position_code", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeePositions");
                DataTable table = dataSet.Tables["EmployeePositions"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplPosTO = new EmployeePositionTO();
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            emplPosTO.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["position_id"] != DBNull.Value)
                        {
                            emplPosTO.PositionID = Int32.Parse(row["position_id"].ToString().Trim());
                        }
                        if (row["position_code"] != DBNull.Value)
                        {
                            emplPosTO.PositionCode = row["position_code"].ToString().Trim();
                        }
                        if (row["position_title_sr"] != DBNull.Value)
                        {
                            emplPosTO.PositionTitleSR = row["position_title_sr"].ToString().Trim();
                        }
                        if (row["position_title_en"] != DBNull.Value)
                        {
                            emplPosTO.PositionTitleEN = row["position_title_en"].ToString().Trim();
                        }
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            emplPosTO.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            emplPosTO.DescEN = row["desc_en"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            emplPosTO.Status = row["status"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            emplPosTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            emplPosTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            emplPosTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            emplPosTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }                        
                        
                        posList.Add(emplPosTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return posList;
        }

        public Dictionary<int,EmployeePositionTO> getEmployeePositionsDictionary(EmployeePositionTO posTO)
        {
            DataSet dataSet = new DataSet();
            EmployeePositionTO emplPosTO = new EmployeePositionTO();
            Dictionary<int,EmployeePositionTO> posList = new Dictionary<int,EmployeePositionTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM employee_positions ");

                if ((posTO.WorkingUnitID != -1) || (posTO.PositionID != -1) || (!posTO.PositionCode.Trim().Equals("")) || (!posTO.PositionTitleSR.Trim().Equals(""))
                     || (!posTO.PositionTitleEN.Trim().Equals("")) || (!posTO.DescSR.Trim().Equals("")) || (!posTO.DescEN.Trim().Equals("")) || (!posTO.Status.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (posTO.WorkingUnitID != -1)
                    {
                        sb.Append(" working_unit_id = '" + posTO.WorkingUnitID.ToString().Trim() + "' AND");
                    }
                    if (posTO.PositionID != -1)
                    {
                        sb.Append(" position_id = '" + posTO.PositionID.ToString().Trim() + "' AND");
                    }
                    if (!posTO.PositionCode.Trim().Equals(""))
                    {
                        sb.Append(" position_code = N'" + posTO.PositionCode.Trim() + "' AND");
                    }
                    if (!posTO.PositionTitleSR.Trim().Equals(""))
                    {
                        sb.Append(" position_title_sr = N'" + posTO.PositionTitleSR.Trim() + "' AND");
                    }
                    if (!posTO.PositionTitleEN.Trim().Equals(""))
                    {
                        sb.Append(" position_title_en = N'" + posTO.PositionTitleEN.Trim() + "' AND");
                    }
                    if (!posTO.DescSR.Trim().Equals(""))
                    {
                        sb.Append(" desc_sr = N'" + posTO.DescSR.Trim() + "' AND");
                    }
                    if (!posTO.DescEN.Trim().Equals(""))
                    {
                        sb.Append(" desc_en = N'" + posTO.DescEN.Trim() + "' AND");
                    }
                    if (!posTO.Status.Trim().Equals(""))
                    {
                        sb.Append(" status = N'" + posTO.Status.Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY position_code", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeePositions");
                DataTable table = dataSet.Tables["EmployeePositions"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplPosTO = new EmployeePositionTO();
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            emplPosTO.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["position_id"] != DBNull.Value)
                        {
                            emplPosTO.PositionID = Int32.Parse(row["position_id"].ToString().Trim());
                        }
                        if (row["position_code"] != DBNull.Value)
                        {
                            emplPosTO.PositionCode = row["position_code"].ToString().Trim();
                        }
                        if (row["position_title_sr"] != DBNull.Value)
                        {
                            emplPosTO.PositionTitleSR = row["position_title_sr"].ToString().Trim();
                        }
                        if (row["position_title_en"] != DBNull.Value)
                        {
                            emplPosTO.PositionTitleEN = row["position_title_en"].ToString().Trim();
                        }
                        if (row["desc_sr"] != DBNull.Value)
                        {
                            emplPosTO.DescSR = row["desc_sr"].ToString().Trim();
                        }
                        if (row["desc_en"] != DBNull.Value)
                        {
                            emplPosTO.DescEN = row["desc_en"].ToString().Trim();
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            emplPosTO.Status = row["status"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            emplPosTO.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            emplPosTO.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            emplPosTO.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            emplPosTO.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }
                        if(!posList.ContainsKey(emplPosTO.PositionID))
                           posList.Add(emplPosTO.PositionID,emplPosTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
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
            _sqlTrans = (SqlTransaction)trans;
        }
    }
}
