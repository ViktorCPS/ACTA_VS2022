using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TransferObjects;

namespace DataAccess
{
    public class MSSQLMedicalCheckPointDAO : MedicalCheckPointDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLMedicalCheckPointDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLMedicalCheckPointDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(MedicalCheckPointTO pointTO, bool doCommit)
        {
            int rowsAffected = 0;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO medical_chk_points ");
                sbInsert.Append("(point_id, description, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append("'" + pointTO.PointID.ToString().Trim() + "', ");
                sbInsert.Append("N'" + pointTO.Desc.Trim() + "', ");
                if (!pointTO.CreatedBy.Trim().Equals(""))
                    sbInsert.Append("N'" + pointTO.CreatedBy.Trim() + "', ");
                else
                    sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!pointTO.CreatedTime.Equals(new DateTime()))
                    sbInsert.Append("'" + pointTO.CreatedTime.ToString(dateTimeformat) + "') ");
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

        public bool update(MedicalCheckPointTO pointTO, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                if (doCommit)
                    SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE medical_chk_points SET ");
                if (!pointTO.Desc.Trim().Equals(""))
                    sbUpdate.Append("description = '" + pointTO.Desc.ToString().Trim() + "', ");
                if (!pointTO.ModifiedBy.Trim().Equals(""))
                    sbUpdate.Append("modified_by = N'" + pointTO.ModifiedBy.Trim() + "', ");
                else
                    sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                if (!pointTO.ModifiedTime.Equals(new DateTime()))
                    sbUpdate.Append("modified_time = '" + pointTO.ModifiedTime.ToString(dateTimeformat) + "' ");
                else
                    sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE point_id = '" + pointTO.PointID.ToString().Trim() + "'");

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

        public bool delete(int pointID, bool doCommit)
        {
            bool isDeleted = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM medical_chk_points WHERE point_id = '" + pointID.ToString().Trim() + "'");

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

        public List<MedicalCheckPointTO> getMedicalCheckPoints(MedicalCheckPointTO pointTO)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckPointTO point = new MedicalCheckPointTO();
            List<MedicalCheckPointTO> pointList = new List<MedicalCheckPointTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM medical_chk_points ");

                if ((pointTO.PointID != -1) || (!pointTO.Desc.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (pointTO.PointID != -1)
                    {
                        sb.Append(" point_id = '" + pointTO.PointID.ToString().Trim() + "' AND");
                    }
                    if (!pointTO.Desc.Trim().Equals(""))
                    {
                        sb.Append(" description = N'" + pointTO.Desc.Trim().ToString() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY description", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Points");
                DataTable table = dataSet.Tables["Points"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        point = new MedicalCheckPointTO();
                        if (row["point_id"] != DBNull.Value)
                        {
                            point.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            point.Desc = row["description"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            point.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            point.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            point.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            point.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        pointList.Add(point);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pointList;
        }

        public Dictionary<int, MedicalCheckPointTO> getMedicalCheckPointsDictionary(MedicalCheckPointTO pointTO)
        {
            DataSet dataSet = new DataSet();
            MedicalCheckPointTO point = new MedicalCheckPointTO();
            Dictionary<int, MedicalCheckPointTO> pointDict = new Dictionary<int, MedicalCheckPointTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM medical_chk_points ");

                if ((pointTO.PointID != -1) || (!pointTO.Desc.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

                    if (pointTO.PointID != -1)
                    {
                        sb.Append(" point_id = '" + pointTO.PointID.ToString().Trim() + "' AND");
                    }
                    if (!pointTO.Desc.Trim().Equals(""))
                    {
                        sb.Append(" description = N'" + pointTO.Desc.Trim().ToString() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select + " ORDER BY description", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Points");
                DataTable table = dataSet.Tables["Points"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        point = new MedicalCheckPointTO();
                        if (row["point_id"] != DBNull.Value)
                        {
                            point.PointID = Int32.Parse(row["point_id"].ToString().Trim());
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            point.Desc = row["description"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            point.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            point.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            point.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            point.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }

                        if (!pointDict.ContainsKey(point.PointID))
                            pointDict.Add(point.PointID, point);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return pointDict;
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
