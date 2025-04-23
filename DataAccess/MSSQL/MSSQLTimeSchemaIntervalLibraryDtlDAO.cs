using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public class MSSQLTimeSchemaIntervalLibraryDtlDAO : TimeSchemaIntervalLibraryDtlDAO
    {
        
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLTimeSchemaIntervalLibraryDtlDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public MSSQLTimeSchemaIntervalLibraryDtlDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
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

        public Dictionary<int,List<TimeSchemaIntervalLibraryDtlTO>> getTimeSchemaIntervalLibraryDtlDictionary(TimeSchemaIntervalLibraryDtlTO timeSchemaTO)
        {
            DataSet dataSet = new DataSet();
            TimeSchemaIntervalLibraryDtlTO timeSchema = new TimeSchemaIntervalLibraryDtlTO();
            Dictionary<int,List<TimeSchemaIntervalLibraryDtlTO>> timeIntervalList = new Dictionary<int,List<TimeSchemaIntervalLibraryDtlTO>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM time_schema_intervals_library_dtl");
                if (timeSchemaTO.TimeSchemaIntervalId != -1 || timeSchemaTO.StartTime != new DateTime() || timeSchemaTO.EndTime != new DateTime())
                {
                    if (timeSchemaTO.StartTime != new DateTime())
                        sb.Append(" start_time= '" + timeSchemaTO.StartTime.ToString(dateTimeformat) + "' AND");
                    if (timeSchemaTO.EndTime != new DateTime())
                        sb.Append(" end_time= '" + timeSchemaTO.EndTime.ToString(dateTimeformat) + "' AND");
                    
                    if (timeSchemaTO.TimeSchemaIntervalId != -1)
                        sb.Append(" time_schema_interval_id= '" + timeSchemaTO.TimeSchemaIntervalId + "' AND");

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }



                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "TimeSchemaIntervalLibrary");
                DataTable table = dataSet.Tables["TimeSchemaIntervalLibrary"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        timeSchema = new TimeSchemaIntervalLibraryDtlTO();
                        timeSchema.TimeSchemaIntervalId = Int32.Parse(row["time_schema_interval_id"].ToString().Trim());
                        
                        if (row["created_by"] != DBNull.Value)
                        {
                            timeSchema.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            timeSchema.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            timeSchema.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            timeSchema.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["start_time"] != DBNull.Value)
                        {
                            timeSchema.StartTime = DateTime.Parse(row["start_time"].ToString().Trim());
                        }
                        if (row["end_time"] != DBNull.Value)
                        {
                            timeSchema.EndTime = DateTime.Parse(row["end_time"].ToString().Trim());
                        }
                        if (!timeIntervalList.ContainsKey(timeSchema.TimeSchemaIntervalId))
                            timeIntervalList.Add(timeSchema.TimeSchemaIntervalId, new List<TimeSchemaIntervalLibraryDtlTO>());
                        
                        timeIntervalList[timeSchema.TimeSchemaIntervalId].Add(timeSchema);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return timeIntervalList;
        }

    }
}
