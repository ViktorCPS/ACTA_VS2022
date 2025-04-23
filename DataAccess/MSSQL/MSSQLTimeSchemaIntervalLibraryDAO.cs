using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    class MSSQLTimeSchemaIntervalLibraryDAO : TimeSchemaIntervalLibraryDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLTimeSchemaIntervalLibraryDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public MSSQLTimeSchemaIntervalLibraryDAO(SqlConnection sqlConnection)
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
        public Dictionary<int,List< TimeSchemaIntervalLibraryTO>> getTimeSchemaIntervalLibraryDictionary(TimeSchemaIntervalLibraryTO timeSchemaTO)
        {
            DataSet dataSet = new DataSet();
            TimeSchemaIntervalLibraryTO timeSchema = new TimeSchemaIntervalLibraryTO();
            Dictionary<int, List<TimeSchemaIntervalLibraryTO>> timeIntervalList = new Dictionary<int, List<TimeSchemaIntervalLibraryTO>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM time_schema_intervals_library");
                if (timeSchemaTO.TimeSchemaId != -1 || timeSchemaTO.TimeSchemaIntervalId != -1)
                { 
                   if (timeSchemaTO.TimeSchemaId != -1)
                       sb.Append(" time_schema_id= '"+timeSchemaTO.TimeSchemaId+"' AND");

                    if(timeSchemaTO.TimeSchemaIntervalId != -1)
                         sb.Append(" time_schema_interval_id= '"+timeSchemaTO.TimeSchemaIntervalId+"' AND");
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
                        timeSchema = new TimeSchemaIntervalLibraryTO();
                        timeSchema.TimeSchemaIntervalId = Int32.Parse(row["time_schema_interval_id"].ToString().Trim());
                        if (row["time_schema_id"] != DBNull.Value)
                        {
                            timeSchema.TimeSchemaId = Int32.Parse(row["time_schema_id"].ToString().Trim());
                        }
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
                        if (!timeIntervalList.ContainsKey(timeSchema.TimeSchemaId))
                            timeIntervalList.Add(timeSchema.TimeSchemaId,new List<TimeSchemaIntervalLibraryTO>());

                        timeIntervalList[timeSchema.TimeSchemaId].Add(timeSchema);
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
