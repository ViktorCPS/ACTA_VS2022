
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TransferObjects;

namespace DataAccess
{
    public class MSSQLVisitAsco4DAO:VisitAsco4DAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLVisitAsco4DAO()
		{
			conn = MSSQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}
        public MSSQLVisitAsco4DAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
			DAOController.GetInstance();
        }
        public List<VisitAsco4TO> getVisitAsco(string firstName, string lastName, string jmbg, string documentNum, string company)
        {
            List<VisitAsco4TO> visits = new List<VisitAsco4TO>();
            try
            {
                DataSet dataSet = new DataSet();
                VisitAsco4TO visit = new VisitAsco4TO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM visits_asco4 va, visits v");
                sb.Append(" WHERE v.visit_id = va.visit_id");
                if (!firstName.Equals(""))
                {
                    sb.Append(" AND UPPER(v.first_name) LIKE N'"+firstName.Trim().ToUpper()+"' ");
                }
                else
                {
                    sb.Append(" AND v.first_name IS NULL");
                }
                if (!lastName.Equals(""))
                {
                    sb.Append(" AND UPPER(v.last_name) LIKE N'" + lastName.Trim().ToUpper() + "' ");
                }
                else
                {
                    sb.Append(" AND v.last_name IS NULL");
                }
                if (!jmbg.Equals(""))
                {
                    sb.Append(" AND UPPER(v.visitor_jmbg) LIKE N'" + jmbg.Trim().ToUpper() + "' ");
                }
                else
                {
                    sb.Append(" AND v.visitor_jmbg IS NULL");
                }
                if (!documentNum.Equals(""))
                {
                    sb.Append(" AND UPPER(v.visitor_id) LIKE N'" + documentNum.Trim().ToUpper() + "' ");
                }
                else
                {
                    sb.Append(" AND v.visitor_id IS NULL");
                }
                if (!company.Equals(""))
                {
                    sb.Append(" AND UPPER(v.company) LIKE N'" + company.Trim().ToUpper() + "' ");
                }
                else
                {
                    sb.Append(" AND v.company IS NULL");
                }
               
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);


                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitAsco4TO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());
                        if (row["integer_value_1"] != DBNull.Value)
                        {
                            visit.IntegerValue1 = Int32.Parse(row["integer_value_1"].ToString().Trim());
                        }
                        if (row["integer_value_2"] != DBNull.Value)
                        {
                            visit.IntegerValue2 = Int32.Parse(row["integer_value_2"].ToString().Trim());
                        }
                        if (row["integer_value_3"] != DBNull.Value)
                        {
                            visit.IntegerValue3 = Int32.Parse(row["integer_value_3"].ToString().Trim());
                        }
                        if (row["integer_value_4"] != DBNull.Value)
                        {
                            visit.IntegerValue4 = Int32.Parse(row["integer_value_4"].ToString().Trim());
                        }
                        if (row["integer_value_5"] != DBNull.Value)
                        {
                            visit.IntegerValue5 = Int32.Parse(row["integer_value_5"].ToString().Trim());
                        }
                        if (row["integer_value_6"] != DBNull.Value)
                        {
                            visit.IntegerValue6 = Int32.Parse(row["integer_value_6"].ToString().Trim());
                        }
                        if (row["integer_value_7"] != DBNull.Value)
                        {
                            visit.IntegerValue7 = Int32.Parse(row["integer_value_7"].ToString().Trim());
                        }
                        if (row["integer_value_8"] != DBNull.Value)
                        {
                            visit.IntegerValue8 = Int32.Parse(row["integer_value_8"].ToString().Trim());
                        }
                        if (row["integer_value_9"] != DBNull.Value)
                        {
                            visit.IntegerValue9 = Int32.Parse(row["integer_value_9"].ToString().Trim());
                        }
                        if (row["integer_value_10"] != DBNull.Value)
                        {
                            visit.IntegerValue10 = Int32.Parse(row["integer_value_10"].ToString().Trim());
                        }
                        if (row["datetime_value_1"] != DBNull.Value)
                        {
                            visit.DatetimeValue1 = DateTime.Parse(row["datetime_value_1"].ToString().Trim());
                        }
                        if (row["datetime_value_2"] != DBNull.Value)
                        {
                            visit.DatetimeValue2 = DateTime.Parse(row["datetime_value_2"].ToString().Trim());
                        }
                        if (row["datetime_value_3"] != DBNull.Value)
                        {
                            visit.DatetimeValue3 = DateTime.Parse(row["datetime_value_3"].ToString().Trim());
                        }
                        if (row["datetime_value_4"] != DBNull.Value)
                        {
                            visit.DatetimeValue4 = DateTime.Parse(row["datetime_value_4"].ToString().Trim());
                        }
                        if (row["datetime_value_5"] != DBNull.Value)
                        {
                            visit.DatetimeValue5 = DateTime.Parse(row["datetime_value_5"].ToString().Trim());
                        }
                        if (row["datetime_value_6"] != DBNull.Value)
                        {
                            visit.DatetimeValue6 = DateTime.Parse(row["datetime_value_6"].ToString().Trim());
                        }
                        if (row["datetime_value_7"] != DBNull.Value)
                        {
                            visit.DatetimeValue7 = DateTime.Parse(row["datetime_value_7"].ToString().Trim());
                        }
                        if (row["datetime_value_8"] != DBNull.Value)
                        {
                            visit.DatetimeValue8 = DateTime.Parse(row["datetime_value_8"].ToString().Trim());
                        }
                        if (row["datetime_value_9"] != DBNull.Value)
                        {
                            visit.DatetimeValue9 = DateTime.Parse(row["datetime_value_9"].ToString().Trim());
                        }
                        if (row["datetime_value_10"] != DBNull.Value)
                        {
                            visit.DatetimeValue10 = DateTime.Parse(row["datetime_value_10"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            visit.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            visit.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value)
                        {
                            visit.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value)
                        {
                            visit.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value)
                        {
                            visit.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }
                        if (row["nvarchar_value_6"] != DBNull.Value)
                        {
                            visit.NVarcharValue6 = row["nvarchar_value_6"].ToString().Trim();
                        }
                        if (row["nvarchar_value_7"] != DBNull.Value)
                        {
                            visit.NVarcharValue7 = row["nvarchar_value_7"].ToString().Trim();
                        }
                        if (row["nvarchar_value_8"] != DBNull.Value)
                        {
                            visit.NVarcharValue8 = row["nvarchar_value_8"].ToString().Trim();
                        }
                        if (row["nvarchar_value_9"] != DBNull.Value)
                        {
                            visit.NVarcharValue9 = row["nvarchar_value_9"].ToString().Trim();
                        }
                        if (row["nvarchar_value_10"] != DBNull.Value)
                        {
                            visit.NVarcharValue10 = row["nvarchar_value_10"].ToString().Trim();
                        }

                        visits.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visits;
        }

        public List<VisitTO> getVisitAsco(string firstName, string lastName, string jmbg, Dictionary<int, List<VisitAsco4TO>> ascoTO)
        {
            List<VisitTO> visits = new List<VisitTO>();
            try
            {
                DataSet dataSet = new DataSet();
                VisitTO visit = new VisitTO();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT v.first_name,v.last_name,v.visitor_id,v.visitor_jmbg,v.company,va.integer_value_1 AS ban,va.datetime_value_2 AS ban_date FROM visits_asco4 va, visits v");
                sb.Append(" WHERE v.visit_id = va.visit_id");

                if (!jmbg.Equals(""))
                {
                    sb.Append(" AND UPPER(v.visitor_jmbg) LIKE N'" + jmbg.Trim().ToUpper() + "' ");
                }
                else
                {
                    if (!firstName.Equals(""))
                    {
                        sb.Append(" AND UPPER(v.first_name) LIKE N'" + firstName.Trim().ToUpper() + "' ");
                    }
                    if (!lastName.Equals(""))
                    {
                        sb.Append(" AND UPPER(v.last_name) LIKE N'" + lastName.Trim().ToUpper() + "' ");
                    }
                }           
                              
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);


                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                VisitAsco4TO asco4TO = new VisitAsco4TO();

                if (table.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitTO();
                        asco4TO = new VisitAsco4TO();
                        i++;

                        visit.VisitID = i;

                        if (row["first_name"] != DBNull.Value)
                        {
                            visit.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            visit.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["visitor_jmbg"] != DBNull.Value)
                        {
                            visit.VisitorJMBG = row["visitor_jmbg"].ToString().Trim();
                        }
                        if (row["visitor_id"] != DBNull.Value)
                        {
                            visit.VisitorID= row["visitor_id"].ToString().Trim();
                        }
                        if (row["company"] != DBNull.Value)
                        {
                            visit.Company = row["company"].ToString().Trim();
                        }
                        if (row["ban"] != DBNull.Value)
                        {
                            asco4TO.IntegerValue1 = int.Parse(row["ban"].ToString().Trim());
                        }
                        if (row["ban_date"] != DBNull.Value)
                        {
                            asco4TO.DatetimeValue2 = DateTime.Parse(row["ban_date"].ToString().Trim());
                        }
                        ascoTO.Add(visit.VisitID, new List<VisitAsco4TO>());
                        ascoTO[visit.VisitID].Add(asco4TO);
                        visits.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visits;
        }

        public List<VisitAsco4TO> getVisitAscoForID(string JMBG, string documentNumber)
        {
            List<VisitAsco4TO> visits = new List<VisitAsco4TO>();
            try
            {
                DataSet dataSet = new DataSet();
                VisitAsco4TO visit = new VisitAsco4TO();
                
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM visits_asco4 va, visits v");
                sb.Append(" WHERE v.visit_id = va.visit_id");
                sb.Append(" AND (v.visitor_jmbg = '" + JMBG + "'");
                sb.Append(" OR v.visitor_id = '" + documentNumber + "' )");
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);


                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitAsco4TO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());
                        if (row["integer_value_1"] != DBNull.Value)
                        {
                            visit.IntegerValue1 = Int32.Parse(row["integer_value_1"].ToString().Trim());
                        }
                        if (row["integer_value_2"] != DBNull.Value)
                        {
                            visit.IntegerValue2 = Int32.Parse(row["integer_value_2"].ToString().Trim());
                        }
                        if (row["integer_value_3"] != DBNull.Value)
                        {
                            visit.IntegerValue3 = Int32.Parse(row["integer_value_3"].ToString().Trim());
                        }
                        if (row["integer_value_4"] != DBNull.Value)
                        {
                            visit.IntegerValue4 = Int32.Parse(row["integer_value_4"].ToString().Trim());
                        }
                        if (row["integer_value_5"] != DBNull.Value)
                        {
                            visit.IntegerValue5 = Int32.Parse(row["integer_value_5"].ToString().Trim());
                        }
                        if (row["integer_value_6"] != DBNull.Value)
                        {
                            visit.IntegerValue6 = Int32.Parse(row["integer_value_6"].ToString().Trim());
                        }
                        if (row["integer_value_7"] != DBNull.Value)
                        {
                            visit.IntegerValue7 = Int32.Parse(row["integer_value_7"].ToString().Trim());
                        }
                        if (row["integer_value_8"] != DBNull.Value)
                        {
                            visit.IntegerValue8 = Int32.Parse(row["integer_value_8"].ToString().Trim());
                        }
                        if (row["integer_value_9"] != DBNull.Value)
                        {
                            visit.IntegerValue9 = Int32.Parse(row["integer_value_9"].ToString().Trim());
                        }
                        if (row["integer_value_10"] != DBNull.Value)
                        {
                            visit.IntegerValue10 = Int32.Parse(row["integer_value_10"].ToString().Trim());
                        }
                        if (row["datetime_value_1"] != DBNull.Value)
                        {
                            visit.DatetimeValue1 = DateTime.Parse(row["datetime_value_1"].ToString().Trim());
                        }
                        if (row["datetime_value_2"] != DBNull.Value)
                        {
                            visit.DatetimeValue2 = DateTime.Parse(row["datetime_value_2"].ToString().Trim());
                        }
                        if (row["datetime_value_3"] != DBNull.Value)
                        {
                            visit.DatetimeValue3 = DateTime.Parse(row["datetime_value_3"].ToString().Trim());
                        }
                        if (row["datetime_value_4"] != DBNull.Value)
                        {
                            visit.DatetimeValue4 = DateTime.Parse(row["datetime_value_4"].ToString().Trim());
                        }
                        if (row["datetime_value_5"] != DBNull.Value)
                        {
                            visit.DatetimeValue5 = DateTime.Parse(row["datetime_value_5"].ToString().Trim());
                        }
                        if (row["datetime_value_6"] != DBNull.Value)
                        {
                            visit.DatetimeValue6 = DateTime.Parse(row["datetime_value_6"].ToString().Trim());
                        }
                        if (row["datetime_value_7"] != DBNull.Value)
                        {
                            visit.DatetimeValue7 = DateTime.Parse(row["datetime_value_7"].ToString().Trim());
                        }
                        if (row["datetime_value_8"] != DBNull.Value)
                        {
                            visit.DatetimeValue8 = DateTime.Parse(row["datetime_value_8"].ToString().Trim());
                        }
                        if (row["datetime_value_9"] != DBNull.Value)
                        {
                            visit.DatetimeValue9 = DateTime.Parse(row["datetime_value_9"].ToString().Trim());
                        }
                        if (row["datetime_value_10"] != DBNull.Value)
                        {
                            visit.DatetimeValue10 = DateTime.Parse(row["datetime_value_10"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            visit.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            visit.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value)
                        {
                            visit.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value)
                        {
                            visit.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value)
                        {
                            visit.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }
                        if (row["nvarchar_value_6"] != DBNull.Value)
                        {
                            visit.NVarcharValue6 = row["nvarchar_value_6"].ToString().Trim();
                        }
                        if (row["nvarchar_value_7"] != DBNull.Value)
                        {
                            visit.NVarcharValue7 = row["nvarchar_value_7"].ToString().Trim();
                        }
                        if (row["nvarchar_value_8"] != DBNull.Value)
                        {
                            visit.NVarcharValue8 = row["nvarchar_value_8"].ToString().Trim();
                        }
                        if (row["nvarchar_value_9"] != DBNull.Value)
                        {
                            visit.NVarcharValue9 = row["nvarchar_value_9"].ToString().Trim();
                        }
                        if (row["nvarchar_value_10"] != DBNull.Value)
                        {
                            visit.NVarcharValue10 = row["nvarchar_value_10"].ToString().Trim();
                        }

                        visits.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visits;
        }

        public DateTime getPrivacyStatement(string JMBG, string documentNumber)
        {
            DateTime statementTime = new DateTime();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT MAX(datetime_value_1) as privacy FROM visits_asco4 va, visits v");
                sb.Append(" WHERE v.visit_id = va.visit_id");
                if (!JMBG.Equals("") && !documentNumber.Equals(""))
                {
                    sb.Append(" AND (v.visitor_jmbg = '" + JMBG + "'");
                    sb.Append(" OR v.visitor_id = N'" + documentNumber + "' )");
                }
                else if (!JMBG.Equals("") )
                    sb.Append(" AND v.visitor_jmbg = '" + JMBG + "'");
                else if (!documentNumber.Equals(""))
                    sb.Append(" AND v.visitor_id = N'" + documentNumber + "' ");
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Privacy");
                DataTable table = dataSet.Tables["Privacy"];

                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    if(row["privacy"] != DBNull.Value)
                    statementTime = DateTime.Parse(row["privacy"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return statementTime;
        }
        public List<VisitAsco4TO> getVisitsAsco(int visitID, int int1, int int2, int int3, int int4, int int5, 
            int int6, int int7, int int8, int int9, int int10, 
            DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
            DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, 
            string string1, string string2, string string3, string string4, string string5, 
            string string6, string string7, string string8, string string9, string string10)
        {
            DataSet dataSet = new DataSet();
            VisitAsco4TO visit = new VisitAsco4TO();
            List<VisitAsco4TO> visitsList = new List<VisitAsco4TO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM visits_asco4");
                if ((visitID != -1)
                    || (int1 != -1) || (int2 != -1) || (int3 != -1) || (int4 != -1) || (int5 != -1)
                    || (int6 != -1) || (int7 != -1) || (int8 != -1) || (int9 != -1) || (int10 != -1)
                    || (!dateTime1.Equals(new DateTime())) || (!dateTime2.Equals(new DateTime())) || (!dateTime3.Equals(new DateTime()))
                    || (!dateTime4.Equals(new DateTime())) || (!dateTime5.Equals(new DateTime()))
                    || (!dateTime6.Equals(new DateTime())) || (!dateTime7.Equals(new DateTime())) || (!dateTime8.Equals(new DateTime()))
                    || (!dateTime9.Equals(new DateTime())) || (!dateTime10.Equals(new DateTime())) 
                    || (!string1.Equals("")) ||(!string2.Equals("")) || (!string3.Equals("")) || (!string4.Equals("")) || (!string5.Equals(""))
                     || (!string6.Equals("")) ||(!string7.Equals("")) || (!string8.Equals("")) || (!string9.Equals("")) || (!string10.Equals("")))
                {

                    sb.Append(" WHERE");

                    if (visitID != -1)
                    {
                        sb.Append(" visit_id = " + visitID + " AND");
                    }
                    if (int1 != -1)
                    {
                        sb.Append(" integer_value_1 = " + int1 + " AND");
                    }
                    if (int2 != -1)
                    {
                        sb.Append(" integer_value_2 = " + int2 + " AND");
                    }
                    if (int3 != -1)
                    {
                        sb.Append(" integer_value_3 = " + int3 + " AND");
                    }
                    if (int4 != -1)
                    {
                        sb.Append(" integer_value_4 = " + int4 + " AND");
                    }
                    if (int5 != -1)
                    {
                        sb.Append(" integer_value_5 = " + int5 + " AND");
                    }
                    if (int6 != -1)
                    {
                        sb.Append(" integer_value_6 = " + int6 + " AND");
                    }
                    if (int7 != -1)
                    {
                        sb.Append(" integer_value_7 = " + int7 + " AND");
                    }
                    if (int8 != -1)
                    {
                        sb.Append(" integer_value_8 = " + int8 + " AND");
                    }
                    if (int9 != -1)
                    {
                        sb.Append(" integer_value_9 = " + int9 + " AND");
                    }
                    if (int10 != -1)
                    {
                        sb.Append(" integer_value_10 = " + int10 + " AND");
                    }
                    if (!dateTime1.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_1 >= CONVERT(datetime,'" + dateTime1.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!dateTime2.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_2 >= CONVERT(datetime,'" + dateTime2.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!dateTime3.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_3 >= CONVERT(datetime,'" + dateTime3.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!dateTime4.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_4 >= CONVERT(datetime,'" + dateTime4.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!dateTime5.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_5 >= CONVERT(datetime,'" + dateTime5.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }

                    if (!dateTime6.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_6 >= CONVERT(datetime,'" + dateTime6.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!dateTime7.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_7 >= CONVERT(datetime,'" + dateTime7.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!dateTime8.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_8 >= CONVERT(datetime,'" + dateTime8.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!dateTime9.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_9 >= CONVERT(datetime,'" + dateTime9.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!dateTime10.Equals(new DateTime()))
                    {
                        sb.Append(" datetime_value_10 >= CONVERT(datetime,'" + dateTime10.ToString("yyyy-MM-dd") + "', 111) AND ");
                    }
                    if (!string1.Equals(""))
                    {
                        sb.Append(" nvarchar_value_1 = N'" + string1 + "' AND");
                    }
                    if (!string2.Equals(""))
                    {
                        sb.Append(" nvarchar_value_2 = N'" + string2 + "' AND");
                    }
                    if (!string3.Equals(""))
                    {
                        sb.Append(" nvarchar_value_3 = N'" + string3 + "' AND");
                    }
                    if (!string4.Equals(""))
                    {
                        sb.Append(" nvarchar_value_4 = N'" + string4 + "' AND");
                    }
                    if (!string5.Equals(""))
                    {
                        sb.Append(" nvarchar_value_5 = N'" + string5 + "' AND");
                    }
                    if (!string6.Equals(""))
                    {
                        sb.Append(" nvarchar_value_6 = N'" + string6 + "' AND");
                    }
                    if (!string7.Equals(""))
                    {
                        sb.Append(" nvarchar_value_7 = N'" + string7 + "' AND");
                    }
                    if (!string8.Equals(""))
                    {
                        sb.Append(" nvarchar_value_8 = N'" + string8 + "' AND");
                    }
                    if (!string9.Equals(""))
                    {
                        sb.Append(" nvarchar_value_9 = N'" + string9 + "' AND");
                    }
                    if (!string10.Equals(""))
                    {
                        sb.Append(" nvarchar_value_10 = N'" + string10 + "' AND");
                    }
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select += " ORDER BY visit_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitAsco4TO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());
                        if (row["integer_value_1"] != DBNull.Value)
                        {
                            visit.IntegerValue1 = Int32.Parse(row["integer_value_1"].ToString().Trim());
                        }
                        if (row["integer_value_2"] != DBNull.Value)
                        {
                            visit.IntegerValue2 = Int32.Parse(row["integer_value_2"].ToString().Trim());
                        }
                        if (row["integer_value_3"] != DBNull.Value)
                        {
                            visit.IntegerValue3 = Int32.Parse(row["integer_value_3"].ToString().Trim());
                        }
                        if (row["integer_value_4"] != DBNull.Value)
                        {
                            visit.IntegerValue4 = Int32.Parse(row["integer_value_4"].ToString().Trim());
                        }
                        if (row["integer_value_5"] != DBNull.Value)
                        {
                            visit.IntegerValue5 = Int32.Parse(row["integer_value_5"].ToString().Trim());
                        }
                        if (row["integer_value_6"] != DBNull.Value)
                        {
                            visit.IntegerValue6 = Int32.Parse(row["integer_value_6"].ToString().Trim());
                        }
                        if (row["integer_value_7"] != DBNull.Value)
                        {
                            visit.IntegerValue7 = Int32.Parse(row["integer_value_7"].ToString().Trim());
                        }
                        if (row["integer_value_8"] != DBNull.Value)
                        {
                            visit.IntegerValue8 = Int32.Parse(row["integer_value_8"].ToString().Trim());
                        }
                        if (row["integer_value_9"] != DBNull.Value)
                        {
                            visit.IntegerValue9 = Int32.Parse(row["integer_value_9"].ToString().Trim());
                        }
                        if (row["integer_value_10"] != DBNull.Value)
                        {
                            visit.IntegerValue10 = Int32.Parse(row["integer_value_10"].ToString().Trim());
                        }
                        if (row["datetime_value_1"] != DBNull.Value)
                        {
                            visit.DatetimeValue1 = DateTime.Parse(row["datetime_value_1"].ToString().Trim());
                        }
                        if (row["datetime_value_2"] != DBNull.Value)
                        {
                            visit.DatetimeValue2 = DateTime.Parse(row["datetime_value_2"].ToString().Trim());
                        }
                        if (row["datetime_value_3"] != DBNull.Value)
                        {
                            visit.DatetimeValue3 = DateTime.Parse(row["datetime_value_3"].ToString().Trim());
                        }
                        if (row["datetime_value_4"] != DBNull.Value)
                        {
                            visit.DatetimeValue4 = DateTime.Parse(row["datetime_value_4"].ToString().Trim());
                        }
                        if (row["datetime_value_5"] != DBNull.Value)
                        {
                            visit.DatetimeValue5 = DateTime.Parse(row["datetime_value_5"].ToString().Trim());
                        }
                        if (row["datetime_value_6"] != DBNull.Value)
                        {
                            visit.DatetimeValue6 = DateTime.Parse(row["datetime_value_6"].ToString().Trim());
                        }
                        if (row["datetime_value_7"] != DBNull.Value)
                        {
                            visit.DatetimeValue7 = DateTime.Parse(row["datetime_value_7"].ToString().Trim());
                        }
                        if (row["datetime_value_8"] != DBNull.Value)
                        {
                            visit.DatetimeValue8 = DateTime.Parse(row["datetime_value_8"].ToString().Trim());
                        }
                        if (row["datetime_value_9"] != DBNull.Value)
                        {
                            visit.DatetimeValue9 = DateTime.Parse(row["datetime_value_9"].ToString().Trim());
                        }
                        if (row["datetime_value_10"] != DBNull.Value)
                        {
                            visit.DatetimeValue10 = DateTime.Parse(row["datetime_value_10"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            visit.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            visit.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value)
                        {
                            visit.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value)
                        {
                            visit.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value)
                        {
                            visit.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }
                        if (row["nvarchar_value_6"] != DBNull.Value)
                        {
                            visit.NVarcharValue6 = row["nvarchar_value_6"].ToString().Trim();
                        }
                        if (row["nvarchar_value_7"] != DBNull.Value)
                        {
                            visit.NVarcharValue7 = row["nvarchar_value_7"].ToString().Trim();
                        }
                        if (row["nvarchar_value_8"] != DBNull.Value)
                        {
                            visit.NVarcharValue8 = row["nvarchar_value_8"].ToString().Trim();
                        }
                        if (row["nvarchar_value_9"] != DBNull.Value)
                        {
                            visit.NVarcharValue9 = row["nvarchar_value_9"].ToString().Trim();
                        }
                        if (row["nvarchar_value_10"] != DBNull.Value)
                        {
                            visit.NVarcharValue10 = row["nvarchar_value_10"].ToString().Trim();
                        }

                        visitsList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitsList;
        }


        public List<VisitAsco4TO> getVisitsDetailsAsco(string visitIDs)
        {
            DataSet dataSet = new DataSet();
            VisitAsco4TO visit = new VisitAsco4TO();
            List<VisitAsco4TO> visitsList = new List<VisitAsco4TO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM visits_asco4");
                if (!visitIDs.Equals(""))
                {
                    sb.Append(" WHERE");                    
                    sb.Append(" visit_id IN ( " + visitIDs + ") ");
                    
                }
                
                select = sb.ToString();
                

                select += " ORDER BY visit_id ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        visit = new VisitAsco4TO();
                        visit.VisitID = Int32.Parse(row["visit_id"].ToString().Trim());
                        if (row["integer_value_1"] != DBNull.Value)
                        {
                            visit.IntegerValue1 = Int32.Parse(row["integer_value_1"].ToString().Trim());
                        }
                        if (row["integer_value_2"] != DBNull.Value)
                        {
                            visit.IntegerValue2 = Int32.Parse(row["integer_value_2"].ToString().Trim());
                        }
                        if (row["integer_value_3"] != DBNull.Value)
                        {
                            visit.IntegerValue3 = Int32.Parse(row["integer_value_3"].ToString().Trim());
                        }
                        if (row["integer_value_4"] != DBNull.Value)
                        {
                            visit.IntegerValue4 = Int32.Parse(row["integer_value_4"].ToString().Trim());
                        }
                        if (row["integer_value_5"] != DBNull.Value)
                        {
                            visit.IntegerValue5 = Int32.Parse(row["integer_value_5"].ToString().Trim());
                        }
                        if (row["integer_value_6"] != DBNull.Value)
                        {
                            visit.IntegerValue6 = Int32.Parse(row["integer_value_6"].ToString().Trim());
                        }
                        if (row["integer_value_7"] != DBNull.Value)
                        {
                            visit.IntegerValue7 = Int32.Parse(row["integer_value_7"].ToString().Trim());
                        }
                        if (row["integer_value_8"] != DBNull.Value)
                        {
                            visit.IntegerValue8 = Int32.Parse(row["integer_value_8"].ToString().Trim());
                        }
                        if (row["integer_value_9"] != DBNull.Value)
                        {
                            visit.IntegerValue9 = Int32.Parse(row["integer_value_9"].ToString().Trim());
                        }
                        if (row["integer_value_10"] != DBNull.Value)
                        {
                            visit.IntegerValue10 = Int32.Parse(row["integer_value_10"].ToString().Trim());
                        }
                        if (row["datetime_value_1"] != DBNull.Value)
                        {
                            visit.DatetimeValue1 = DateTime.Parse(row["datetime_value_1"].ToString().Trim());
                        }
                        if (row["datetime_value_2"] != DBNull.Value)
                        {
                            visit.DatetimeValue2 = DateTime.Parse(row["datetime_value_2"].ToString().Trim());
                        }
                        if (row["datetime_value_3"] != DBNull.Value)
                        {
                            visit.DatetimeValue3 = DateTime.Parse(row["datetime_value_3"].ToString().Trim());
                        }
                        if (row["datetime_value_4"] != DBNull.Value)
                        {
                            visit.DatetimeValue4 = DateTime.Parse(row["datetime_value_4"].ToString().Trim());
                        }
                        if (row["datetime_value_5"] != DBNull.Value)
                        {
                            visit.DatetimeValue5 = DateTime.Parse(row["datetime_value_5"].ToString().Trim());
                        }
                        if (row["datetime_value_6"] != DBNull.Value)
                        {
                            visit.DatetimeValue6 = DateTime.Parse(row["datetime_value_6"].ToString().Trim());
                        }
                        if (row["datetime_value_7"] != DBNull.Value)
                        {
                            visit.DatetimeValue7 = DateTime.Parse(row["datetime_value_7"].ToString().Trim());
                        }
                        if (row["datetime_value_8"] != DBNull.Value)
                        {
                            visit.DatetimeValue8 = DateTime.Parse(row["datetime_value_8"].ToString().Trim());
                        }
                        if (row["datetime_value_9"] != DBNull.Value)
                        {
                            visit.DatetimeValue9 = DateTime.Parse(row["datetime_value_9"].ToString().Trim());
                        }
                        if (row["datetime_value_10"] != DBNull.Value)
                        {
                            visit.DatetimeValue10 = DateTime.Parse(row["datetime_value_10"].ToString().Trim());
                        }
                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            visit.NVarcharValue1 = row["nvarchar_value_1"].ToString().Trim();
                        }
                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            visit.NVarcharValue2 = row["nvarchar_value_2"].ToString().Trim();
                        }
                        if (row["nvarchar_value_3"] != DBNull.Value)
                        {
                            visit.NVarcharValue3 = row["nvarchar_value_3"].ToString().Trim();
                        }
                        if (row["nvarchar_value_4"] != DBNull.Value)
                        {
                            visit.NVarcharValue4 = row["nvarchar_value_4"].ToString().Trim();
                        }
                        if (row["nvarchar_value_5"] != DBNull.Value)
                        {
                            visit.NVarcharValue5 = row["nvarchar_value_5"].ToString().Trim();
                        }
                        if (row["nvarchar_value_6"] != DBNull.Value)
                        {
                            visit.NVarcharValue6 = row["nvarchar_value_6"].ToString().Trim();
                        }
                        if (row["nvarchar_value_7"] != DBNull.Value)
                        {
                            visit.NVarcharValue7 = row["nvarchar_value_7"].ToString().Trim();
                        }
                        if (row["nvarchar_value_8"] != DBNull.Value)
                        {
                            visit.NVarcharValue8 = row["nvarchar_value_8"].ToString().Trim();
                        }
                        if (row["nvarchar_value_9"] != DBNull.Value)
                        {
                            visit.NVarcharValue9 = row["nvarchar_value_9"].ToString().Trim();
                        }
                        if (row["nvarchar_value_10"] != DBNull.Value)
                        {
                            visit.NVarcharValue10 = row["nvarchar_value_10"].ToString().Trim();
                        }

                        visitsList.Add(visit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return visitsList;
        }


        public bool insert(int visitID, int int1, int int2, int int3, int int4, int int5, 
            int int6, int int7, int int8, int int9, int int10, 
            DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5, 
            DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, 
            string string1, string string2, string string3, string string4, string string5, 
            string string6, string string7, string string8, string string9, string string10)
        {
            bool isInserted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead,"INSERT");
           
            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO visits_asco4 ");
                sbInsert.Append("(visit_id,integer_value_1, integer_value_2, integer_value_3, integer_value_4, integer_value_5,integer_value_6, integer_value_7, integer_value_8, integer_value_9, integer_value_10,");
                sbInsert.Append("datetime_value_1,datetime_value_2,datetime_value_3,datetime_value_4,datetime_value_5,datetime_value_6,datetime_value_7,datetime_value_8,datetime_value_9,datetime_value_10,");
                sbInsert.Append("nvarchar_value_1,nvarchar_value_2,nvarchar_value_3,nvarchar_value_4,nvarchar_value_5, nvarchar_value_6,nvarchar_value_7,nvarchar_value_8,nvarchar_value_9,nvarchar_value_10,created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(visitID + ", ");
                if (int1 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int1 + ", ");
                }
                if (int2 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int2 + ", ");
                }
                if (int3 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int3 + ", ");
                }
                if (int4 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int4 + ", ");
                }
                if (int5 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int5 + ", ");
                }
                if (int6 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int6 + ", ");
                }
                if (int7 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int7 + ", ");
                }
                if (int8 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int8 + ", ");
                }
                if (int9 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int9 + ", ");
                }
                if (int10 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int10 + ", ");
                }
                if (dateTime1.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime2.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime3.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime4.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime5.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime6.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime7.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime8.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime9.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime10.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (string1.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string1 + "', ");
                }
                if (string2.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string2 + "', ");
                }
                if (string3.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string3 + "', ");
                }
                if (string4.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string4 + "', ");
                }
                if (string5.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string5 + "', ");
                }
                if (string6.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string6 + "', ");
                }
                if (string7.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string7 + "', ");
                }
                if (string8.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string8 + "', ");
                }
                if (string9.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string9 + "', ");
                }
                if (string10.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string10 + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()); ");


                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                int rowsAffected = cmd.ExecuteNonQuery();

                sqlTrans.Commit();

                if (rowsAffected > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");

                throw ex;
            }

            return isInserted;
        }

        public List<string> getDistinctVisitors()
        {
            List<string> visitors = new List<string>();
            DataSet dataSet = new DataSet();            
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT nvarchar_value_1 from visits_asco4 ORDER BY nvarchar_value_1");
                select = sb.ToString().Trim();
                 SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        if (row["nvarchar_value_1"] != DBNull.Value)
                        {
                            visitors.Add(row["nvarchar_value_1"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return visitors;
        }

        public List<string> getDistinct(string column)
        {
            List<string> visitors = new List<string>();
            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT "+column+" from visits_asco4 ORDER BY "+column);
                select = sb.ToString().Trim();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        if (row[column] != DBNull.Value)
                        {
                            visitors.Add(row[column].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return visitors;
        }

        public List<string> getDistinctDocumentNames()
        {
            List<string> docs = new List<string>();
            DataSet dataSet = new DataSet();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT nvarchar_value_2 from visits_asco4 ORDER BY nvarchar_value_2");
                select = sb.ToString().Trim();
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Visit");
                DataTable table = dataSet.Tables["Visit"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {

                        if (row["nvarchar_value_2"] != DBNull.Value)
                        {
                            docs.Add(row["nvarchar_value_2"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
            return docs;
        }

        public bool insert(int visitID, int int1, int int2, int int3, int int4, int int5,
            int int6, int int7, int int8, int int9, int int10,
            DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
            DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10,
            string string1, string string2, string string3, string string4, string string5,
            string string6, string string7, string string8, string string9, string string10, bool doCommit)
        {
            bool isInserted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead,"INSERT");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }
            

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO visits_asco4 ");
                sbInsert.Append("(visit_id,integer_value_1, integer_value_2, integer_value_3, integer_value_4, integer_value_5,integer_value_6, integer_value_7, integer_value_8, integer_value_9, integer_value_10,");
                sbInsert.Append("datetime_value_1,datetime_value_2,datetime_value_3,datetime_value_4,datetime_value_5,datetime_value_6,datetime_value_7,datetime_value_8,datetime_value_9,datetime_value_10,");
                sbInsert.Append("nvarchar_value_1,nvarchar_value_2,nvarchar_value_3,nvarchar_value_4,nvarchar_value_5, nvarchar_value_6,nvarchar_value_7,nvarchar_value_8,nvarchar_value_9,nvarchar_value_10,created_by, created_time) ");
                sbInsert.Append("VALUES (");
                sbInsert.Append(visitID + ", ");
                if (int1 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int1 + ", ");
                }
                if (int2 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int2 + ", ");
                }
                if (int3 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int3 + ", ");
                }
                if (int4 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int4 + ", ");
                }
                if (int5 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int5 + ", ");
                }
                if (int6 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int6 + ", ");
                }
                if (int7 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int7 + ", ");
                }
                if (int8 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int8 + ", ");
                }
                if (int9 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int9 + ", ");
                }
                if (int10 == -1)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append(int10 + ", ");
                }
                if (dateTime1.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime2.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime3.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime4.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime5.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime6.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime7.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime8.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime9.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (dateTime10.Equals(new DateTime()))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                if (string1.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string1 + "', ");
                }
                if (string2.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string2 + "', ");
                }
                if (string3.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string3 + "', ");
                }
                if (string4.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string4 + "', ");
                }
                if (string5.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string5 + "', ");
                }
                if (string6.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string6 + "', ");
                }
                if (string7.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string7 + "', ");
                }
                if (string8.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string8 + "', ");
                }
                if (string9.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string9 + "', ");
                }
                if (string10.Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + string10 + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()); ");


                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
                if (rowsAffected > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("INSERT");
                }

                throw ex;
            }

            return isInserted;
        }

        public bool update(int visitID, int int1, int int2, int int3, int int4, int int5,
            int int6, int int7, int int8, int int9, int int10,
            DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
            DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10,
            string string1, string string2, string string3, string string4, string string5,
            string string6, string string7, string string8, string string9, string string10, bool doCommit)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead,"UPDATE");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }

            try
            {
                StringBuilder sbUpdate = new StringBuilder();

                sbUpdate.Append("UPDATE visits_asco4 SET ");
                if (int1 != -1)
                {
                    sbUpdate.Append("integer_value_1 = " + int1 + ", ");
                }
                
                if (int2 != -1)
                {
                    sbUpdate.Append("integer_value_2 = " + int2 + ", ");
                }
                
                if (int3 != -1)
                {
                    sbUpdate.Append("integer_value_3 = " + int3 + ", ");
                }
                
                if (int4 != -1)
                {
                    sbUpdate.Append("integer_value_4 = " + int4 + ", ");
                }
                
                if (int5 != -1)
                {
                    sbUpdate.Append("integer_value_5 = " + int5 + ", ");
                }

                if (int6 != -1)
                {
                    sbUpdate.Append("integer_value_6 = " + int6 + ", ");
                }

                if (int7 != -1)
                {
                    sbUpdate.Append("integer_value_7 = " + int7 + ", ");
                }

                if (int8 != -1)
                {
                    sbUpdate.Append("integer_value_8 = " + int8 + ", ");
                }

                if (int9 != -1)
                {
                    sbUpdate.Append("integer_value_9 = " + int9 + ", ");
                }

                if (int10 != -1)
                {
                    sbUpdate.Append("integer_value_10 = " + int10 + ", ");
                }
                

                if (dateTime1 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_1 = '" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                

                if (dateTime2 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_2 = '" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                
                if (dateTime3 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_3 = '" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                
                if (dateTime4 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_4 = '" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                
                if (dateTime5 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_5 = '" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime6 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_6 = '" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }


                if (dateTime7 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_7 = '" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime8 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_8 = '" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime9 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_9 = '" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime10 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_10 = '" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                
                if (!string1.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_1 = N'" + string1 + "', ");
                }
                
                if (!string2.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_2 = N'" + string2 + "', ");
                }
                
                if (!string3.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_3 = N'" + string3 + "', ");
                }
                
                if (!string4.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_4 = N'" + string4 + "', ");
                }
                
                if (!string5.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_5 = N'" + string5 + "', ");
                }
                if (!string6.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_6 = N'" + string6 + "', ");
                }

                if (!string7.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_7 = N'" + string7 + "', ");
                }

                if (!string8.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_8 = N'" + string8 + "', ");
                }

                if (!string9.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_9 = N'" + string9 + "', ");
                }

                if (!string10.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_10 = N'" + string10 + "', ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE visit_id = " + visitID);

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                if (doCommit)
                {
                    sqlTrans.Commit();
                }

            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqlTrans.Rollback("UPDATE");
                }
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

       public  int enterBan(string visitID,int banActive, DateTime banDate)
        {
            int visitNum = 0;
            
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead,"UPDATE");


            try
            {
                StringBuilder sbUpdate = new StringBuilder();

                sbUpdate.Append("UPDATE visits_asco4 SET ");
                if (banActive != -1)
                {
                    sbUpdate.Append("integer_value_1 = " + banActive+ ", ");
                }


                if (banDate != new DateTime())
                {
                    sbUpdate.Append("datetime_value_2 = '" + banDate.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }
                               

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE visit_id IN ( " + visitID+" )");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                visitNum = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                
                sqlTrans.Rollback("UPDATE");
                
                throw new Exception(ex.Message);
            }
            return visitNum;
        }

        public bool update(int visitID, int int1, int int2, int int3, int int4, int int5,
            int int6, int int7, int int8, int int9, int int10,
            DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
            DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10,
            string string1, string string2, string string3, string string4, string string5,
            string string6, string string7, string string8, string string9, string string10)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead,"UPDATE");


            try
            {
                StringBuilder sbUpdate = new StringBuilder();

                sbUpdate.Append("UPDATE visits_asco4 SET ");
                if (int1 != -1)
                {
                    sbUpdate.Append("integer_value_1 = " + int1 + ", ");
                }

                if (int2 != -1)
                {
                    sbUpdate.Append("integer_value_2 = " + int2 + ", ");
                }

                if (int3 != -1)
                {
                    sbUpdate.Append("integer_value_3 = " + int3 + ", ");
                }

                if (int4 != -1)
                {
                    sbUpdate.Append("integer_value_4 = " + int4 + ", ");
                }

                if (int5 != -1)
                {
                    sbUpdate.Append("integer_value_5 = " + int5 + ", ");
                }

                if (int6 != -1)
                {
                    sbUpdate.Append("integer_value_6 = " + int6 + ", ");
                }

                if (int7 != -1)
                {
                    sbUpdate.Append("integer_value_7 = " + int7 + ", ");
                }

                if (int8 != -1)
                {
                    sbUpdate.Append("integer_value_8 = " + int8 + ", ");
                }

                if (int9 != -1)
                {
                    sbUpdate.Append("integer_value_9 = " + int9 + ", ");
                }

                if (int10 != -1)
                {
                    sbUpdate.Append("integer_value_10 = " + int10 + ", ");
                }


                if (dateTime1 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_1 = '" + dateTime1.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }


                if (dateTime2 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_2 = '" + dateTime2.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime3 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_3 = '" + dateTime3.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime4 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_4 = '" + dateTime4.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime5 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_5 = '" + dateTime5.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime6 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_6 = '" + dateTime6.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }


                if (dateTime7 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_7 = '" + dateTime7.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime8 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_8 = '" + dateTime8.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime9 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_9 = '" + dateTime9.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (dateTime10 != new DateTime())
                {
                    sbUpdate.Append("datetime_value_10 = '" + dateTime10.ToString("yyy-MM-dd HH:mm:ss").Trim() + "', ");
                }

                if (!string1.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_1 = N'" + string1 + "', ");
                }

                if (!string2.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_2 = N'" + string2 + "', ");
                }

                if (!string3.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_3 = N'" + string3 + "', ");
                }

                if (!string4.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_4 = N'" + string4 + "', ");
                }

                if (!string5.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_5 = N'" + string5 + "', ");
                }
                if (!string6.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_6 = N'" + string6 + "', ");
                }

                if (!string7.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_7 = N'" + string7 + "', ");
                }

                if (!string8.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_8 = N'" + string8 + "', ");
                }

                if (!string9.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_9 = N'" + string9 + "', ");
                }

                if (!string10.Equals(""))
                {
                    sbUpdate.Append("nvarchar_value_10 = N'" + string10 + "', ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE visit_id = " + visitID);

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();


            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("UPDATE");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool delete(int visitID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead,"DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM visits_asco4 WHERE visit_id = " + visitID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool delete(int visitID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead,"DELETE");
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }


            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM visits_asco4 WHERE visit_id = " + visitID);

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("DELETE");
                throw new Exception(ex.Message);
            }

            return isDeleted;
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
