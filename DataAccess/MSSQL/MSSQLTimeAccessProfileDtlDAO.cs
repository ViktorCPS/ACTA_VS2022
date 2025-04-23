using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLTimeAccessProfileDtlDAO.
	/// </summary>
	public class MSSQLTimeAccessProfileDtlDAO : TimeAccessProfileDtlDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLTimeAccessProfileDtlDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLTimeAccessProfileDtlDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
		public int insert(string timeAccessProfileId, string dayOfWeek, string direction, string hrs0,
			string hrs1, string hrs2, string hrs3, string hrs4, string hrs5, string hrs6, string hrs7, string hrs8, string hrs9, 
			string hrs10, string hrs11, string hrs12, string hrs13, string hrs14, string hrs15, string hrs16, string hrs17, string hrs18, 
			string hrs19, string hrs20, string hrs21, string hrs22, string hrs23, bool doCommit)
		{			
			SqlTransaction sqlTrans = null;

			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO timeaccess_profiles_dtl ");
				sbInsert.Append("(timeaccess_profile_id, day_of_week, direction, hrs_0, ");
				sbInsert.Append("hrs_1, hrs_2, hrs_3, hrs_4, hrs_5, hrs_6, hrs_7, hrs_8, hrs_9, hrs_10, ");
				sbInsert.Append("hrs_11, hrs_12, hrs_13, hrs_14, hrs_15, hrs_16, hrs_17, hrs_18, hrs_19, hrs_20, ");
				sbInsert.Append("hrs_21, hrs_22, hrs_23, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append(Int32.Parse(timeAccessProfileId.Trim()) + ", ");

				sbInsert.Append(Int32.Parse(dayOfWeek.Trim()) + ", ");

				sbInsert.Append("N'" + direction.Trim() + "', ");

				sbInsert.Append(Int32.Parse(hrs0.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs1.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs2.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs3.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs4.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs5.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs6.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs7.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs8.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs9.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs10.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs11.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs12.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs13.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs14.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs15.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs16.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs17.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs18.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs19.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs20.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs21.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs22.Trim()) + ", ");
				sbInsert.Append(Int32.Parse(hrs23.Trim()) + ", ");

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

				SqlCommand cmd = new SqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				if (doCommit)
				{
					sqlTrans.Commit();
				}
			}
			catch(Exception ex)
			{
				if(doCommit)
					sqlTrans.Rollback("INSERT");
				else
					sqlTrans.Rollback();				
				throw ex;
			}

			return rowsAffected;
		}		

		public bool update(string timeAccessProfileId, string dayOfWeek, string direction, string hrs0,
			string hrs1, string hrs2, string hrs3, string hrs4, string hrs5, string hrs6, string hrs7, string hrs8, string hrs9, 
			string hrs10, string hrs11, string hrs12, string hrs13, string hrs14, string hrs15, string hrs16, string hrs17, string hrs18, 
			string hrs19, string hrs20, string hrs21, string hrs22, string hrs23, bool doCommit)
		{
			bool isUpdated = false;			
			SqlTransaction sqlTrans = null;

			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE timeaccess_profiles_dtl SET ");

				sbUpdate.Append("hrs_0 = " + Int32.Parse(hrs0.Trim()) + ", ");
				sbUpdate.Append("hrs_1 = " + Int32.Parse(hrs1.Trim()) + ", ");
				sbUpdate.Append("hrs_2 = " + Int32.Parse(hrs2.Trim()) + ", ");
				sbUpdate.Append("hrs_3 = " + Int32.Parse(hrs3.Trim()) + ", ");
				sbUpdate.Append("hrs_4 = " + Int32.Parse(hrs4.Trim()) + ", ");
				sbUpdate.Append("hrs_5 = " + Int32.Parse(hrs5.Trim()) + ", ");
				sbUpdate.Append("hrs_6 = " + Int32.Parse(hrs6.Trim()) + ", ");
				sbUpdate.Append("hrs_7 = " + Int32.Parse(hrs7.Trim()) + ", ");
				sbUpdate.Append("hrs_8 = " + Int32.Parse(hrs8.Trim()) + ", ");
				sbUpdate.Append("hrs_9 = " + Int32.Parse(hrs9.Trim()) + ", ");
				sbUpdate.Append("hrs_10 = " + Int32.Parse(hrs10.Trim()) + ", ");
				sbUpdate.Append("hrs_11 = " + Int32.Parse(hrs11.Trim()) + ", ");
				sbUpdate.Append("hrs_12 = " + Int32.Parse(hrs12.Trim()) + ", ");
				sbUpdate.Append("hrs_13 = " + Int32.Parse(hrs13.Trim()) + ", ");
				sbUpdate.Append("hrs_14 = " + Int32.Parse(hrs14.Trim()) + ", ");
				sbUpdate.Append("hrs_15 = " + Int32.Parse(hrs15.Trim()) + ", ");
				sbUpdate.Append("hrs_16 = " + Int32.Parse(hrs16.Trim()) + ", ");
				sbUpdate.Append("hrs_17 = " + Int32.Parse(hrs17.Trim()) + ", ");
				sbUpdate.Append("hrs_18 = " + Int32.Parse(hrs18.Trim()) + ", ");
				sbUpdate.Append("hrs_19 = " + Int32.Parse(hrs19.Trim()) + ", ");
				sbUpdate.Append("hrs_20 = " + Int32.Parse(hrs20.Trim()) + ", ");
				sbUpdate.Append("hrs_21 = " + Int32.Parse(hrs21.Trim()) + ", ");
				sbUpdate.Append("hrs_22 = " + Int32.Parse(hrs22.Trim()) + ", ");
				sbUpdate.Append("hrs_23 = " + Int32.Parse(hrs23.Trim()) + ", ");
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE timeaccess_profile_id = " + Int32.Parse(timeAccessProfileId.Trim()));
				sbUpdate.Append(" AND day_of_week = " + Int32.Parse(dayOfWeek.Trim()));
				sbUpdate.Append(" AND UPPER(direction) = N'" + direction.ToUpper().Trim() + "' ");
				
				SqlCommand cmd = new SqlCommand( sbUpdate.ToString(), conn, sqlTrans );
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;	
				}
				if (doCommit)
				{
					sqlTrans.Commit();
				}
			}
			catch(Exception ex)
			{
				if(doCommit)
					sqlTrans.Rollback("UPDATE");
				else
					sqlTrans.Rollback();				
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool delete(string timeAccessProfileId, string dayOfWeek, string direction, bool doCommit)
		{
			bool isDeleted = false;			
			SqlTransaction sqlTrans = null;

			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM timeaccess_profiles_dtl WHERE timeaccess_profile_id = " + Int32.Parse(timeAccessProfileId.Trim()));
				sbDelete.Append(" AND day_of_week = " + Int32.Parse(dayOfWeek.Trim()));
				sbDelete.Append(" AND UPPER(direction) = N'" + direction.ToUpper().Trim() + "' ");
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}
				if (doCommit)
				{
					sqlTrans.Commit();
				}
			}
			catch(Exception ex)
			{
				if(doCommit)
					sqlTrans.Rollback("DELETE");
				else
					sqlTrans.Rollback();				
				
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

		public TimeAccessProfileDtlTO find(string timeAccessProfileId, string dayOfWeek, string direction)
		{
			DataSet dataSet = new DataSet();
			TimeAccessProfileDtlTO timeAccessProfileDtlTO = new TimeAccessProfileDtlTO();
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT * FROM timeaccess_profiles_dtl WHERE timeaccess_profile_id = " + Int32.Parse(timeAccessProfileId.Trim())
					+ " AND day_of_week = " + Int32.Parse(dayOfWeek.Trim())
					+ " AND UPPER(direction) = N'" + direction.ToUpper().Trim() + "' ", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "TimeAccessProfileDtl");
				DataTable table = dataSet.Tables["TimeAccessProfileDtl"];

				if (table.Rows.Count == 1)
				{
					timeAccessProfileDtlTO = new TimeAccessProfileDtlTO();

					if (table.Rows[0]["timeaccess_profile_id"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.TimeAccessProfileId = Int32.Parse(table.Rows[0]["timeaccess_profile_id"].ToString().Trim());
					}					

					if (!table.Rows[0]["day_of_week"].Equals(DBNull.Value))
					{
						timeAccessProfileDtlTO.DayOfWeek = Int32.Parse(table.Rows[0]["day_of_week"].ToString().Trim());
					}

					if (!table.Rows[0]["direction"].Equals(DBNull.Value))
					{
						timeAccessProfileDtlTO.Direction = table.Rows[0]["direction"].ToString().Trim();
					}

					if (table.Rows[0]["hrs_0"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs0 = Int32.Parse(table.Rows[0]["hrs_0"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_1"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs1 = Int32.Parse(table.Rows[0]["hrs_1"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_2"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs2 = Int32.Parse(table.Rows[0]["hrs_2"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_3"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs3 = Int32.Parse(table.Rows[0]["hrs_3"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_4"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs4 = Int32.Parse(table.Rows[0]["hrs_4"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_5"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs5 = Int32.Parse(table.Rows[0]["hrs_5"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_6"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs6 = Int32.Parse(table.Rows[0]["hrs_6"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_7"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs7 = Int32.Parse(table.Rows[0]["hrs_7"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_8"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs8 = Int32.Parse(table.Rows[0]["hrs_8"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_9"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs9 = Int32.Parse(table.Rows[0]["hrs_9"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_10"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs10 = Int32.Parse(table.Rows[0]["hrs_10"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_11"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs11 = Int32.Parse(table.Rows[0]["hrs_11"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_12"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs12 = Int32.Parse(table.Rows[0]["hrs_12"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_13"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs13 = Int32.Parse(table.Rows[0]["hrs_13"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_14"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs14 = Int32.Parse(table.Rows[0]["hrs_14"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_15"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs15 = Int32.Parse(table.Rows[0]["hrs_15"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_16"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs16 = Int32.Parse(table.Rows[0]["hrs_16"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_17"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs17 = Int32.Parse(table.Rows[0]["hrs_17"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_18"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs18 = Int32.Parse(table.Rows[0]["hrs_18"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_19"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs19 = Int32.Parse(table.Rows[0]["hrs_19"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_20"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs20 = Int32.Parse(table.Rows[0]["hrs_20"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_21"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs21 = Int32.Parse(table.Rows[0]["hrs_21"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_22"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs22 = Int32.Parse(table.Rows[0]["hrs_22"].ToString().Trim());
					}
					if (table.Rows[0]["hrs_23"] != DBNull.Value)
					{
						timeAccessProfileDtlTO.Hrs23 = Int32.Parse(table.Rows[0]["hrs_23"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return timeAccessProfileDtlTO;
		}
		
		public ArrayList getTimeAccessProfileDtl(string timeAccessProfileId)
		{
			DataSet dataSet = new DataSet();
			TimeAccessProfileDtlTO timeAccessProfileDtlTO = new TimeAccessProfileDtlTO();
			ArrayList timeAccessProfileDtlList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM timeaccess_profiles_dtl WHERE timeaccess_profile_id = " + Int32.Parse(timeAccessProfileId.Trim()));
				select = sb.ToString();

				//SqlCommand cmd = new SqlCommand(select, conn );
                SqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new SqlCommand(select, conn );
                else
                    cmd = new SqlCommand(select, conn, this.SqlTrans);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "TimeAccessProfileDtl");
				DataTable table = dataSet.Tables["TimeAccessProfileDtl"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						timeAccessProfileDtlTO = new TimeAccessProfileDtlTO();

						if (!row["timeaccess_profile_id"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.TimeAccessProfileId = (int)row["timeaccess_profile_id"]; //Int32.Parse(row["timeaccess_profile_id"].ToString().Trim());
						}
						if (!row["day_of_week"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.DayOfWeek = (int)row["day_of_week"]; //Int32.Parse(row["day_of_week"].ToString().Trim());
						}
						if (!row["direction"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Direction = row["direction"].ToString().Trim();
						}

						if (!row["hrs_0"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs0 = (int)row["hrs_0"]; //Int32.Parse(row["hrs_0"].ToString().Trim());
						}
						if (!row["hrs_1"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs1 = (int)row["hrs_1"]; //Int32.Parse(row["hrs_1"].ToString().Trim());
						}
						if (!row["hrs_2"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs2 = (int)row["hrs_2"]; //Int32.Parse(row["hrs_2"].ToString().Trim());
						}
						if (!row["hrs_3"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs3 = (int)row["hrs_3"]; //Int32.Parse(row["hrs_3"].ToString().Trim());
						}
						if (!row["hrs_4"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs4 = (int)row["hrs_4"]; //Int32.Parse(row["hrs_4"].ToString().Trim());
						}
						if (!row["hrs_5"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs5 = (int)row["hrs_5"]; //Int32.Parse(row["hrs_5"].ToString().Trim());
						}
						if (!row["hrs_6"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs6 = (int)row["hrs_6"]; //Int32.Parse(row["hrs_6"].ToString().Trim());
						}
						if (!row["hrs_7"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs7 = (int)row["hrs_7"]; //Int32.Parse(row["hrs_7"].ToString().Trim());
						}
						if (!row["hrs_8"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs8 = (int)row["hrs_8"]; //Int32.Parse(row["hrs_8"].ToString().Trim());
						}
						if (!row["hrs_9"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs9 = (int)row["hrs_9"]; //Int32.Parse(row["hrs_9"].ToString().Trim());
						}
						if (!row["hrs_10"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs10 = (int)row["hrs_10"]; //Int32.Parse(row["hrs_10"].ToString().Trim());
						}
						if (!row["hrs_11"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs11 = (int)row["hrs_11"]; //Int32.Parse(row["hrs_11"].ToString().Trim());
						}
						if (!row["hrs_12"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs12 = (int)row["hrs_12"]; //Int32.Parse(row["hrs_12"].ToString().Trim());
						}
						if (!row["hrs_13"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs13 = (int)row["hrs_13"]; //Int32.Parse(row["hrs_13"].ToString().Trim());
						}
						if (!row["hrs_14"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs14 = (int)row["hrs_14"]; //Int32.Parse(row["hrs_14"].ToString().Trim());
						}
						if (!row["hrs_15"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs15 = (int)row["hrs_15"]; //Int32.Parse(row["hrs_15"].ToString().Trim());
						}
						if (!row["hrs_16"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs16 = (int)row["hrs_16"]; //Int32.Parse(row["hrs_16"].ToString().Trim());
						}
						if (!row["hrs_17"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs17 = (int)row["hrs_17"]; //Int32.Parse(row["hrs_17"].ToString().Trim());
						}
						if (!row["hrs_18"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs18 = (int)row["hrs_18"]; //Int32.Parse(row["hrs_18"].ToString().Trim());
						}
						if (!row["hrs_19"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs19 = (int)row["hrs_19"]; //Int32.Parse(row["hrs_19"].ToString().Trim());
						}
						if (!row["hrs_20"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs20 = (int)row["hrs_20"]; //Int32.Parse(row["hrs_20"].ToString().Trim());
						}
						if (!row["hrs_21"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs21 = (int)row["hrs_21"]; //Int32.Parse(row["hrs_21"].ToString().Trim());
						}
						if (!row["hrs_22"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs22 = (int)row["hrs_22"]; //Int32.Parse(row["hrs_22"].ToString().Trim());
						}
						if (!row["hrs_23"].Equals(DBNull.Value))
						{
							timeAccessProfileDtlTO.Hrs23 = (int)row["hrs_23"]; //Int32.Parse(row["hrs_23"].ToString().Trim());
						}
						
						timeAccessProfileDtlList.Add(timeAccessProfileDtlTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return timeAccessProfileDtlList;
		}

		// TODO!!!
		public void serialize(ArrayList timeAccessProfileDtlTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLTimeAccessProfileDtlFile"];
				this.serialize(timeAccessProfileDtlTOList, filename);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void serialize(ArrayList timeAccessProfileDtlTOList, String filename)
		{
			try
			{
				Stream stream = File.Open(filename, FileMode.Create);
				TimeAccessProfileDtlTO[] timeAccessProfileDtlTOArray = (TimeAccessProfileDtlTO[]) timeAccessProfileDtlTOList.ToArray(typeof(TimeAccessProfileDtlTO));

				XmlSerializer bformatter = new XmlSerializer(typeof(TimeAccessProfileDtlTO[]));
				bformatter.Serialize(stream, timeAccessProfileDtlTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        // TODO!!!
        public void serialize(ArrayList timeAccessProfileDtlTOList, Stream stream)
        {
            try
            {
                TimeAccessProfileDtlTO[] timeAccessProfileDtlTOArray = (TimeAccessProfileDtlTO[])timeAccessProfileDtlTOList.ToArray(typeof(TimeAccessProfileDtlTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(TimeAccessProfileDtlTO[]));
                bformatter.Serialize(stream, timeAccessProfileDtlTOArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public ArrayList deserialize(string filePath)
		{
			ArrayList timeAccessProfileDtlList = new ArrayList();

			try
			{
				if (File.Exists(filePath))
				{
					Stream stream = File.Open(filePath, FileMode.Open);
					XmlSerializer bformatter = new XmlSerializer(typeof(TimeAccessProfileDtlTO[]));
					TimeAccessProfileDtlTO[] deserialized;

					try
					{
						deserialized = (TimeAccessProfileDtlTO[]) bformatter.Deserialize(stream);
						timeAccessProfileDtlList = ArrayList.Adapter(deserialized);
					}
					catch(Exception ex)
					{
						stream.Close();
						throw new DataProcessingException("File: " + filePath + " " + ex.Message + "\n", 3);
					}
					
					stream.Close();
				}
			}
			catch(IOException ioEx)
			{
				throw new DataProcessingException(ioEx + " File: " + filePath, 2);
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return timeAccessProfileDtlList;
		}

        public ArrayList deserialize(Stream stream)
        {
            ArrayList timeAccessProfileDtlList = new ArrayList();

            try
            {
                    XmlSerializer bformatter = new XmlSerializer(typeof(TimeAccessProfileDtlTO[]));
                    TimeAccessProfileDtlTO[] deserialized;

                    try
                    {
                        deserialized = (TimeAccessProfileDtlTO[])bformatter.Deserialize(stream);
                        timeAccessProfileDtlList = ArrayList.Adapter(deserialized);
                    }
                    catch (Exception ex)
                    {
                        stream.Close();
                        throw new DataProcessingException(ex.Message + "\n", 3);
                    }
            }
            catch (IOException ioEx)
            {
                throw new DataProcessingException(ioEx.Message, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return timeAccessProfileDtlList;
        }
	}
}
