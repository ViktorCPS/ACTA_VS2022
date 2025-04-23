using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLGateTimeAccessProfileDAO.
	/// </summary>
	public class MySQLGateTimeAccessProfileDAO : GateTimeAccessProfileDAO
	{
		MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MySQLGateTimeAccessProfileDAO()
		{
			conn = MySQLDAOFactory.getConnection();
		}
        public MySQLGateTimeAccessProfileDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
        }
		public int insert(string name, string description, string gateTAProfile0, string gateTAProfile1,
			string gateTAProfile2, string gateTAProfile3, string gateTAProfile4, string gateTAProfile5,
			string gateTAProfile6, string gateTAProfile7, string gateTAProfile8, string gateTAProfile9,
			string gateTAProfile10, string gateTAProfile11, string gateTAProfile12,
			string gateTAProfile13, string gateTAProfile14, string gateTAProfile15)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();

				sbInsert.Append("INSERT INTO gate_timeaccess_profiles_hdr ");
				sbInsert.Append("(name, description, gate_timeacess_profile_0, gate_timeacess_profile_1, ");
				sbInsert.Append("gate_timeacess_profile_2, gate_timeacess_profile_3, gate_timeacess_profile_4, ");
				sbInsert.Append("gate_timeacess_profile_5, gate_timeacess_profile_6, gate_timeacess_profile_7, "); 
				sbInsert.Append("gate_timeacess_profile_8, gate_timeacess_profile_9, gate_timeacess_profile_10, ");  
				sbInsert.Append("gate_timeacess_profile_11, gate_timeacess_profile_12, gate_timeacess_profile_13, ");
				sbInsert.Append("gate_timeacess_profile_14, gate_timeacess_profile_15, ");
				sbInsert.Append("created_by, created_time) ");
				sbInsert.Append("VALUES (");

				sbInsert.Append("N'" + name.Trim() + "', ");

				sbInsert.Append("N'" + description.Trim() + "', ");

				if(!gateTAProfile0.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile0.Trim()) + ", ");
				}
				else
				{
					//sbInsert.Append("NULL, ");
					//can't be null, exception
				}
				if(!gateTAProfile1.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile1.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}				
				if(!gateTAProfile2.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile2.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile3.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile3.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile4.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile4.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile5.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile5.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile6.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile6.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile7.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile7.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile8.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile8.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile9.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile9.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile10.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile10.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile11.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile11.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile12.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile12.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile13.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile13.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile14.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile14.Trim()) + ", ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if(!gateTAProfile15.Trim().Equals(""))
				{
					sbInsert.Append(Int32.Parse(gateTAProfile15.Trim()) + ", ");
				}
				else
				{
					//sbInsert.Append("NULL, ");
					//can't be null, exception
				}

				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
				
				MySqlCommand cmd = new MySqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw ex;
			}

			return rowsAffected;
		}		

		public bool update(string gateTimeAccessProfileId, string name, string description, 
			string gateTAProfile0, string gateTAProfile1, string gateTAProfile2, 
			string gateTAProfile3, string gateTAProfile4, string gateTAProfile5,
			string gateTAProfile6, string gateTAProfile7, string gateTAProfile8, 
			string gateTAProfile9, string gateTAProfile10, string gateTAProfile11, 
			string gateTAProfile12, string gateTAProfile13, string gateTAProfile14, 
			string gateTAProfile15, bool doCommit)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = null;

			if(doCommit)
			{
				sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			}
			else
			{
				sqlTrans = this.SqlTrans;
			}

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE gate_timeaccess_profiles_hdr SET ");

				if (!name.Trim().Equals(""))
				{
					sbUpdate.Append("name = N'" + name.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("name = null, ");
				}
				
				if (!description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}

				if(!gateTAProfile0.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_0 = " + Int32.Parse(gateTAProfile0.Trim()) + ", ");
				}
				else
				{
					//sbUpdate.Append("gate_timeacess_profile_0 = null, ");
					//can't be null, exception
				}
				if(!gateTAProfile1.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_1 = " + Int32.Parse(gateTAProfile1.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_1 = null, ");
				}	
				if(!gateTAProfile2.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_2 = " + Int32.Parse(gateTAProfile2.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_2 = null, ");
				}
				if(!gateTAProfile3.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_3 = " + Int32.Parse(gateTAProfile3.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_3 = null, ");
				}
				if(!gateTAProfile4.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_4 = " + Int32.Parse(gateTAProfile4.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_4 = null, ");
				}
				if(!gateTAProfile5.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_5 = " + Int32.Parse(gateTAProfile5.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_5 = null, ");
				}
				if(!gateTAProfile6.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_6 = " + Int32.Parse(gateTAProfile6.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_6 = null, ");
				}
				if(!gateTAProfile7.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_7 = " + Int32.Parse(gateTAProfile7.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_7 = null, ");
				}
				if(!gateTAProfile8.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_8 = " + Int32.Parse(gateTAProfile8.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_8 = null, ");
				}
				if(!gateTAProfile9.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_9 = " + Int32.Parse(gateTAProfile9.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_9 = null, ");
				}
				if(!gateTAProfile10.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_10 = " + Int32.Parse(gateTAProfile10.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_10 = null, ");
				}
				if(!gateTAProfile11.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_11 = " + Int32.Parse(gateTAProfile11.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_11 = null, ");
				}	
				if(!gateTAProfile12.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_12 = " + Int32.Parse(gateTAProfile12.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_12 = null, ");
				}
				if(!gateTAProfile13.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_13 = " + Int32.Parse(gateTAProfile13.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_13 = null, ");
				}
				if(!gateTAProfile14.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_14 = " + Int32.Parse(gateTAProfile14.Trim()) + ", ");
				}
				else
				{
					sbUpdate.Append("gate_timeacess_profile_14 = null, ");
				}
				if(!gateTAProfile15.Trim().Equals(""))
				{
					sbUpdate.Append("gate_timeacess_profile_15 = " + Int32.Parse(gateTAProfile15.Trim()) + ", ");
				}
				else
				{
					//sbUpdate.Append("gate_timeacess_profile_15 = null, ");
					//can't be null, exception
				}
				
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE gate_timeaccess_profile_id = " + Int32.Parse(gateTimeAccessProfileId.Trim()));
				
				MySqlCommand cmd = new MySqlCommand( sbUpdate.ToString(), conn, sqlTrans );
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
				sqlTrans.Rollback();
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool delete(string gateTimeAccessProfileId)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM gate_timeaccess_profiles_hdr WHERE gate_timeaccess_profile_id = " + Int32.Parse(gateTimeAccessProfileId.Trim()));				
				MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
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
			_sqlTrans = (MySqlTransaction)trans;
		}

		public GateTimeAccessProfileTO find(string gateTimeAccessProfileId)
		{
			DataSet dataSet = new DataSet();
			GateTimeAccessProfileTO gateTimeAccessProfileTO = new GateTimeAccessProfileTO();
			try
			{
				//MySqlCommand cmd = new MySqlCommand("SELECT * FROM gate_timeaccess_profiles_hdr WHERE gate_timeaccess_profile_id = " + Int32.Parse(gateTimeAccessProfileId.Trim()), conn );
                MySqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new MySqlCommand("SELECT * FROM gate_timeaccess_profiles_hdr WHERE gate_timeaccess_profile_id = " + Int32.Parse(gateTimeAccessProfileId.Trim()), conn );
                else
                    cmd = new MySqlCommand("SELECT * FROM gate_timeaccess_profiles_hdr WHERE gate_timeaccess_profile_id = " + Int32.Parse(gateTimeAccessProfileId.Trim()), conn, this.SqlTrans);
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "GateTimeAccessProfile");
				DataTable table = dataSet.Tables["GateTimeAccessProfile"];

				if (table.Rows.Count == 1)
				{
					gateTimeAccessProfileTO = new GateTimeAccessProfileTO();

					if (table.Rows[0]["gate_timeaccess_profile_id"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfileId = Int32.Parse(table.Rows[0]["gate_timeaccess_profile_id"].ToString().Trim());
					}					

					if (!table.Rows[0]["name"].Equals(DBNull.Value))
					{
						gateTimeAccessProfileTO.Name = table.Rows[0]["name"].ToString().Trim();
					}

					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						gateTimeAccessProfileTO.Description = table.Rows[0]["description"].ToString().Trim();
					}

					if (table.Rows[0]["gate_timeacess_profile_0"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile0 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_0"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_1"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile1 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_1"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_2"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile2 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_2"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_3"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile3 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_3"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_4"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile4 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_4"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_5"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile5 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_5"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_6"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile6 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_6"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_7"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile7 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_7"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_8"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile8 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_8"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_9"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile9 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_9"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_10"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile10 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_10"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_11"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile11 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_11"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_12"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile12 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_12"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_13"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile13 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_13"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_14"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile14 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_14"].ToString().Trim());
					}
					if (table.Rows[0]["gate_timeacess_profile_15"] != DBNull.Value)
					{
						gateTimeAccessProfileTO.GateTAProfile15 = Int32.Parse(table.Rows[0]["gate_timeacess_profile_15"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return gateTimeAccessProfileTO;
		}

		public ArrayList getGateTimeAccessProfile(string name)
		{
			DataSet dataSet = new DataSet();
			GateTimeAccessProfileTO gateTimeAccessProfileTO = new GateTimeAccessProfileTO();
			ArrayList gateTimeAccessProfileList = new ArrayList();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM gate_timeaccess_profiles_hdr ");

				if (!name.Trim().Equals(""))
				{
					sb.Append(" WHERE ");					
					sb.Append(" UPPER(name) = N'" + name.ToUpper().Trim() + "'");

					select = sb.ToString();
				}
				else
				{
					select = sb.ToString();
				}

				select = select + " ORDER BY name ";

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "GateTimeAccessProfile");
				DataTable table = dataSet.Tables["GateTimeAccessProfile"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						gateTimeAccessProfileTO = new GateTimeAccessProfileTO();

						if (!row["gate_timeaccess_profile_id"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfileId = (int)row["gate_timeaccess_profile_id"]; //Int32.Parse(row["gate_timeaccess_profile_id"].ToString().Trim());
						}
						if (!row["name"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.Name = row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.Description = row["description"].ToString().Trim();
						}

						if (!row["gate_timeacess_profile_0"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile0 = Int32.Parse(row["gate_timeacess_profile_0"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_1"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile1 = Int32.Parse(row["gate_timeacess_profile_1"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_2"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile2 = Int32.Parse(row["gate_timeacess_profile_2"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_3"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile3 = Int32.Parse(row["gate_timeacess_profile_3"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_4"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile4 = Int32.Parse(row["gate_timeacess_profile_4"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_5"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile5 = Int32.Parse(row["gate_timeacess_profile_5"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_6"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile6 = Int32.Parse(row["gate_timeacess_profile_6"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_7"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile7 = Int32.Parse(row["gate_timeacess_profile_7"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_8"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile8 = Int32.Parse(row["gate_timeacess_profile_8"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_9"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile9 = Int32.Parse(row["gate_timeacess_profile_9"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_10"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile10 = Int32.Parse(row["gate_timeacess_profile_10"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_11"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile11 = Int32.Parse(row["gate_timeacess_profile_11"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_12"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile12 = Int32.Parse(row["gate_timeacess_profile_12"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_13"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile13 = Int32.Parse(row["gate_timeacess_profile_13"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_14"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile14 = Int32.Parse(row["gate_timeacess_profile_14"].ToString().Trim());
						}
						if (!row["gate_timeacess_profile_15"].Equals(DBNull.Value))
						{
							gateTimeAccessProfileTO.GateTAProfile15 = Int32.Parse(row["gate_timeacess_profile_15"].ToString().Trim());
						}

						gateTimeAccessProfileList.Add(gateTimeAccessProfileTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return gateTimeAccessProfileList;
		}

		// TODO!!!
		public void serialize(ArrayList gateTimeAccessProfileTOList)
		{
			try
			{
				// TODO: Not implemented yet
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLGateTimeAccessProfileFile"];
				Stream stream = File.Open(filename, FileMode.Create);

				GateTimeAccessProfileTO[] gateTimeAccessProfileTOArray = (GateTimeAccessProfileTO[]) gateTimeAccessProfileTOList.ToArray(typeof(GateTimeAccessProfileTO));

				XmlSerializer bformatter = new XmlSerializer(typeof(GateTimeAccessProfileTO[]));
				bformatter.Serialize(stream, gateTimeAccessProfileTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
