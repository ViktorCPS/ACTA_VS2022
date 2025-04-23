using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;


namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLTagDAO.
	/// </summary>
	public class MySQLTagDAO : TagDAO
	{
		MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

		public MySQLTagDAO()
		{
			conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLTagDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as MySqlConnection;
        }

        public int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO tags ");
				sbInsert.Append("(tag_id, owner_id, status, description, issued, valid_to, created_by, created_time) ");
				sbInsert.Append("VALUES (");
				
				if (tagID == 0)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + tagID.ToString().Trim() + "', ");
				}
				if (ownerID < 0)
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("'" + ownerID.ToString().Trim() + "', ");
				}
				if (status.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + status.Trim() + "', ");
				}
				if (description.Trim().Equals(""))
				{
					sbInsert.Append("null, ");
				}
				else
				{
					sbInsert.Append("N'" + description.Trim() + "', ");
				}
                if (issued == new DateTime())
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + issued.ToString(dateTimeformat).Trim() + "', ");
                }
                if (validTO == new DateTime())
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + validTO.ToString(dateTimeformat).Trim() + "', ");
                }
				sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', current_timestamp) ");
				
				MySqlCommand cmd = new MySqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();	
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw new Exception(ex.Message);
			}

			return rowsAffected;
		}

        public int searchTagsCount(int emplID, string status, string wUnits, DateTime from, DateTime to, string tagID)
        {
            DataSet dataSet = new DataSet();

            int count = 0;
            string select = "";

            try
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(t.record_id) count_rec ");
                sb.Append("FROM tags t, employees empl ");
                sb.Append("WHERE t.owner_id = empl.employee_id AND ");

                if ((emplID != -1) || (!status.Trim().Equals("")) ||
                    (!from.Equals(new DateTime(0))) || (!to.Equals(new DateTime(0))) ||
                    (!tagID.Equals("")))
                {
                    if (emplID != -1)
                    {
                        //sb.Append("UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("t.owner_id = '" + emplID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (!status.Trim().Equals(""))
                    {
                        sb.Append("t.status IN (" + status.Trim().ToUpper() + ") AND ");
                    }
                    if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime(0)))
                    {
                        sb.Append("((t.created_time >= CONVERT('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("t.created_time < CONVERT('" + to.AddDays(1).ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(t.modified_time >= CONVERT('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("t.modified_time < CONVERT('" + to.AddDays(1).ToString("yyyy-MM-dd") + "', datetime))) AND ");
                    }
                    if (!tagID.Equals(""))
                    {
                        //sb.Append("UPPER(ps.pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("t.tag_id = '" + tagID.ToString().Trim().ToUpper() + "' AND ");
                    }
                }

                select = sb.ToString();

                select += " empl.working_unit_id IN (" + wUnits + ")";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Tag");
                DataTable table = dataSet.Tables["Tag"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_rec"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return count;
        }

        public List<TagTO> searchTags(int emplID, string status, string wuString, DateTime from, DateTime to, string tagID)
        {
            DataSet dataSet = new DataSet();
            TagTO tag = new TagTO();
            List<TagTO> tagsList = new List<TagTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT t.*,empl.first_name,empl.last_name,wu.name FROM tags t,employees empl, working_units wu ");

                sb.Append("WHERE t.owner_id = empl.employee_id ");
                sb.Append("AND empl.working_unit_id = wu.working_unit_id AND ");

                if ((emplID != -1) || (!status.Trim().Equals("")) ||
                    (!from.Equals(new DateTime(0))) || (!to.Equals(new DateTime(0))) ||
                    (!tagID.Equals("")))
                {
                    if (emplID != -1)
                    {
                        //sb.Append("UPPER(ps.employee_id) LIKE '" + employeeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("t.owner_id = '" + emplID.ToString().Trim().ToUpper() + "' AND ");
                    }
                    if (!status.Trim().Equals(""))
                    {
                        sb.Append("t.status IN (" + status.Trim().ToUpper() + ") AND ");
                    }
                    if (!from.Equals(new DateTime(0)) && !to.Equals(new DateTime(0)))
                    {
                        sb.Append("((t.created_time >= CONVERT('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("t.created_time < CONVERT('" + to.AddDays(1).ToString("yyyy-MM-dd") + "', datetime)) OR ");
                        sb.Append("(t.modified_time >= CONVERT('" + from.ToString("yyyy-MM-dd") + "', datetime) AND ");
                        sb.Append("t.modified_time < CONVERT('" + to.AddDays(1).ToString("yyyy-MM-dd") + "', datetime))) AND ");
                    }
                    if (!tagID.Equals(""))
                    {
                        //sb.Append("UPPER(ps.pass_type_id) LIKE '" + passTypeID.ToString().Trim().ToUpper() + "' AND ");
                        sb.Append("t.tag_id = '" + tagID.ToString().Trim().ToUpper() + "' AND ");
                    }
                }

                select = sb.ToString();

                select += " empl.working_unit_id IN (" + wuString+ ")";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Tag");
                DataTable table = dataSet.Tables["Tag"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        tag = new TagTO();
                        tag.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
                        if (row["tag_id"] != DBNull.Value)
                        {
                            tag.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
                        }
                        if (row["owner_id"] != DBNull.Value)
                        {
                            tag.OwnerID = Int32.Parse(row["owner_id"].ToString().Trim());
                        }
                        if (row["status"] != DBNull.Value)
                        {
                            tag.Status = row["status"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            tag.Description = row["description"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            tag.EmployeeName = row["last_name"].ToString().Trim()+" ";
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            tag.EmployeeName += row["first_name"].ToString().Trim();
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            tag.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            tag.Issued = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["modified_by"] != DBNull.Value)
                        {
                            tag.ModifiedBy = row["modified_by"].ToString().Trim();
                        }
                        if (row["modified_time"] != DBNull.Value)
                        {
                            tag.ModifiedTime = DateTime.Parse(row["modified_time"].ToString().Trim());
                        }
                        if (row["name"] != DBNull.Value)
                        {
                            tag.WorkingUnit = row["name"].ToString().Trim();
                        }

                        tagsList.Add(tag);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return tagsList;
        }

        public int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string createdBy)
        {
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO tags ");
                sbInsert.Append("(tag_id, owner_id, status, description, issued, valid_to, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (tagID == 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + tagID.ToString().Trim() + "', ");
                }
                if (ownerID < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ownerID.ToString().Trim() + "', ");
                }
                if (status.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + status.Trim() + "', ");
                }
                if (description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + description.Trim() + "', ");
                }
                if (issued == new DateTime())
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + issued.ToString(dateTimeformat).Trim() + "', ");
                }
                if (validTO == new DateTime())
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + validTO.ToString(dateTimeformat).Trim() + "', ");
                }
                sbInsert.Append("N'" + createdBy.Trim() + "', current_timestamp) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return rowsAffected;
        }

        public int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string createdBy, bool doCommit)
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
                sbInsert.Append("INSERT INTO tags ");
                sbInsert.Append("(tag_id, owner_id, status, description, issued, valid_to, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (tagID == 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + tagID.ToString().Trim() + "', ");
                }
                if (ownerID < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ownerID.ToString().Trim() + "', ");
                }
                if (status.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + status.Trim() + "', ");
                }
                if (description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + description.Trim() + "', ");
                }
                if (issued == new DateTime())
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + issued.ToString(dateTimeformat).Trim() + "', ");
                }
                if (validTO == new DateTime())
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + validTO.ToString(dateTimeformat).Trim() + "', ");
                }
                sbInsert.Append("N'" + createdBy.Trim() + "', current_timestamp) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, SqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if(doCommit)
                SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if(doCommit)
                SqlTrans.Rollback();

                throw new Exception(ex.Message);
            }

            return rowsAffected;
        }

		public bool delete(int recordID)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM tags WHERE record_id = " + recordID.ToString().Trim());
				
				MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public TagTO find(int recordID)
		{
			DataSet dataSet = new DataSet();
			TagTO tag = new TagTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand( "SELECT * FROM tags WHERE record_id = '" + recordID.ToString().Trim() + "'", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Tag");
				DataTable table = dataSet.Tables["Tag"];

				if (table.Rows.Count == 1)
				{
					tag = new TagTO();
					tag.RecordID = Int32.Parse(table.Rows[0]["record_id"].ToString().Trim());
					if (table.Rows[0]["tag_id"] != DBNull.Value)
					{
						tag.TagID = UInt32.Parse(table.Rows[0]["tag_id"].ToString().Trim());
					}
					if (table.Rows[0]["owner_id"] != DBNull.Value)
					{
						tag.OwnerID = Int32.Parse(table.Rows[0]["owner_id"].ToString().Trim());
					}
					if (table.Rows[0]["status"] != DBNull.Value)
					{
						tag.Status = table.Rows[0]["status"].ToString().Trim();
					}
					if (table.Rows[0]["description"] != DBNull.Value)
					{
						tag.Description = table.Rows[0]["description"].ToString().Trim();
					}
                    if (table.Rows[0]["issued"] != DBNull.Value)
                    {
                        tag.Issued = DateTime.Parse(table.Rows[0]["issued"].ToString().Trim());
                    }
                    if (table.Rows[0]["valid_to"] != DBNull.Value)
                    {
                        tag.ValidTO = DateTime.Parse(table.Rows[0]["valid_to"].ToString().Trim());
                    }
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return tag;
		}

		public TagTO findActive(int ownerID)
		{
			DataSet dataSet = new DataSet();
			TagTO tag = new TagTO();
			try
			{
				string select = "SELECT * FROM tags WHERE owner_id = '" + ownerID.ToString().Trim() + "' "
					+ "AND (UPPER(status) = N'" + Constants.statusActive.ToUpper()
					+ "' OR UPPER(status) = N'" + Constants.statusBlocked.ToUpper() + "')";
				MySqlCommand cmd;

                if (this.SqlTrans != null)
                {
                    cmd = new MySqlCommand(select, conn, this.SqlTrans);
                }
                else
                {
                    cmd = new MySqlCommand(select, conn);
                }
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Tag");
				DataTable table = dataSet.Tables["Tag"];

				if (table.Rows.Count == 1)
				{
					tag = new TagTO();
					tag.RecordID = Int32.Parse(table.Rows[0]["record_id"].ToString().Trim());
					if (table.Rows[0]["tag_id"] != DBNull.Value)
					{
						tag.TagID = UInt32.Parse(table.Rows[0]["tag_id"].ToString().Trim());
					}
					if (table.Rows[0]["owner_id"] != DBNull.Value)
					{
						tag.OwnerID = Int32.Parse(table.Rows[0]["owner_id"].ToString().Trim());
					}
					if (table.Rows[0]["status"] != DBNull.Value)
					{
						tag.Status = table.Rows[0]["status"].ToString().Trim();
					}
					if (table.Rows[0]["description"] != DBNull.Value)
					{
						tag.Description = table.Rows[0]["description"].ToString().Trim();
					}
                    if (table.Rows[0]["issued"] != DBNull.Value)
                    {
                        tag.Issued = DateTime.Parse(table.Rows[0]["issued"].ToString().Trim());
                    }
                    if (table.Rows[0]["valid_to"] != DBNull.Value)
                    {
                        tag.ValidTO = DateTime.Parse(table.Rows[0]["valid_to"].ToString().Trim());
                    }
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception: " + ex.Message);
			}

			return tag;
		}

		public bool update(int recordID, ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, bool doCommit)
		{
			bool isUpdated = false;
            MySqlTransaction sqlTrans = null;

            if (doCommit)
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
				sbUpdate.Append("UPDATE tags SET ");
				if (tagID > 0)
				{
					sbUpdate.Append("tag_id = '" + tagID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("tag_id = null, ");
				}
				if (ownerID >= 0)
				{
					sbUpdate.Append("owner_id = '" + ownerID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("owner_id = null, ");
				}
				if (!status.Trim().Equals(""))
				{
					sbUpdate.Append("status = N'" + status.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("status = null, ");
				}
				if (!description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}
                if (issued != new DateTime())
                {
                    sbUpdate.Append("issued = '" + issued.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("issued = null, ");
                }
                if (validTO != new DateTime())
                {
                    sbUpdate.Append("valid_to = '" + validTO.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("valid_to = null, ");
                }
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = current_timestamp ");
				sbUpdate.Append("WHERE record_id = '" + recordID.ToString().Trim() + "'");
				
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
                if (doCommit)
                {
                    sqlTrans.Rollback();
                }
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

        public bool update(int recordID, ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string user, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = null;

            if (doCommit)
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
                sbUpdate.Append("UPDATE tags SET ");
                if (tagID > 0)
                {
                    sbUpdate.Append("tag_id = '" + tagID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("tag_id = null, ");
                }
                if (ownerID >= 0)
                {
                    sbUpdate.Append("owner_id = '" + ownerID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("owner_id = null, ");
                }
                if (!status.Trim().Equals(""))
                {
                    sbUpdate.Append("status = N'" + status.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("status = null, ");
                }
                if (!description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (issued != new DateTime())
                {
                    sbUpdate.Append("issued = '" + issued.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("issued = null, ");
                }
                if (validTO != new DateTime())
                {
                    sbUpdate.Append("valid_to = '" + validTO.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("valid_to = null, ");
                }
                sbUpdate.Append("modified_by = N'" + user + "', ");
                sbUpdate.Append("modified_time = current_timestamp ");
                sbUpdate.Append("WHERE record_id = '" + recordID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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
                    sqlTrans.Rollback();
                }

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }


		public List<TagTO> getTags(TagTO t)
		{
			DataSet dataSet = new DataSet();
			TagTO tag = new TagTO();
			List<TagTO> tagsList = new List<TagTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM tags ");

                if ((t.RecordID != -1) || (t.TagID != 0) ||
                        (t.OwnerID != -1) || (!t.Status.Trim().Equals("")) ||
                        (!t.Description.Trim().Equals("")))
				{
					sb.Append(" WHERE ");
					
					if (t.RecordID != -1)
					{
						sb.Append(" UPPER(record_id) LIKE '" + t.RecordID.ToString().Trim() + "' AND");
					}
					if (t.TagID != 0)
					{
						sb.Append(" UPPER(tag_id) LIKE '" + t.TagID.ToString().Trim() + "' AND");
					}
					if (t.OwnerID != -1)
					{
						sb.Append(" UPPER(owner_id) LIKE '" + t.OwnerID.ToString().Trim() + "' AND");
					}
					if (!t.Status.Trim().Equals(""))
					{
						sb.Append(" UPPER(status) LIKE N'" + t.Status.Trim().ToUpper() + "' AND");
					}
					if (!t.Description.Trim().Equals(""))
					{
						sb.Append(" UPPER(description) LIKE N'" + t.Description.Trim().ToUpper() + "' AND");
					}

					select = sb.ToString(0, sb.ToString().Length - 3);
				}
				else
				{
					select = sb.ToString();
				}

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Tag");
				DataTable table = dataSet.Tables["Tag"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						tag = new TagTO();
						tag.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
						if (row["tag_id"] != DBNull.Value)
						{
							tag.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
						}
						if (row["owner_id"] != DBNull.Value)
						{
							tag.OwnerID = Int32.Parse(row["owner_id"].ToString().Trim());
						}
						if (row["status"] != DBNull.Value)
						{
							tag.Status = row["status"].ToString().Trim();
						}
						if (row["description"] != DBNull.Value)
						{
							tag.Description = row["description"].ToString().Trim();
						}
                        if (row["issued"] != DBNull.Value)
                        {
                            tag.Issued = DateTime.Parse(row["issued"].ToString().Trim());
                        }
                        if (row["valid_to"] != DBNull.Value)
                        {
                            tag.ValidTO = DateTime.Parse(row["valid_to"].ToString().Trim());
                        }

						tagsList.Add(tag);
					}
				}
			}
			catch(Exception ex)
			{				
				throw new Exception("Exception: " + ex.Message);
			}

			return tagsList;
		}

       
		public Dictionary<ulong, TagTO> getActiveTags()
		{
			DataSet dataSet = new DataSet();
			TagTO tag = new TagTO();
			Dictionary<ulong, TagTO> tagsList = new Dictionary<ulong,TagTO>();

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM tags ");

				sb.Append(" WHERE ");
				sb.Append(" UPPER(status) LIKE N'" + Constants.statusActive.ToUpper() + "' ");
				sb.Append(" ORDER BY tag_id ");

				MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Tag");
				DataTable table = dataSet.Tables["Tag"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						tag = new TagTO();
						tag.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
						if (row["tag_id"] != DBNull.Value)
						{
							tag.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
						}
						if (row["owner_id"] != DBNull.Value)
						{
							tag.OwnerID = Int32.Parse(row["owner_id"].ToString().Trim());
						}
						if (row["status"] != DBNull.Value)
						{
							tag.Status = row["status"].ToString().Trim();
						}
						if (row["description"] != DBNull.Value)
						{
							tag.Description = row["description"].ToString().Trim();
						}
                        if (row["issued"] != DBNull.Value)
                        {
                            tag.Issued = DateTime.Parse(row["issued"].ToString().Trim());
                        }
                        if (row["valid_to"] != DBNull.Value)
                        {
                            tag.ValidTO = DateTime.Parse(row["valid_to"].ToString().Trim());
                        }
                        if(!tagsList.ContainsKey(tag.TagID))
						tagsList.Add(tag.TagID, tag);
					}
				}
			}
			catch(Exception ex)
			{				
				throw new Exception("Exception: " + ex.Message);
			}

			return tagsList;
		}

		public Dictionary<ulong, TagTO> getActiveTagsWithAccessGroup()
		{
			DataSet dataSet = new DataSet();
			TagTO tag = new TagTO();
			Dictionary<ulong, TagTO> tagsList = new Dictionary<ulong,TagTO>();

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT tags.*, employees.access_group_id FROM tags, employees ");

				sb.Append(" WHERE ");
				sb.Append(" UPPER(tags.status) LIKE N'" + Constants.statusActive.ToUpper() + "' ");
				sb.Append(" AND tags.owner_id = employees.employee_id ");
				sb.Append(" ORDER BY tags.tag_id ");				

				//MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn );
                MySqlCommand cmd;
                if (this.SqlTrans == null)
                    cmd = new MySqlCommand(sb.ToString(), conn);
                else
                    cmd = new MySqlCommand(sb.ToString(), conn, this.SqlTrans);
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Tag");
				DataTable table = dataSet.Tables["Tag"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						tag = new TagTO();
						tag.RecordID = Int32.Parse(row["record_id"].ToString().Trim());
						if (row["tag_id"] != DBNull.Value)
						{
							tag.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
						}
						if (row["owner_id"] != DBNull.Value)
						{
							tag.OwnerID = Int32.Parse(row["owner_id"].ToString().Trim());
						}
						if (row["status"] != DBNull.Value)
						{
							tag.Status = row["status"].ToString().Trim();
						}
						if (row["description"] != DBNull.Value)
						{
							tag.Description = row["description"].ToString().Trim();
						}
						if (row["access_group_id"] != DBNull.Value)
						{
							tag.AccessGroupID = Int32.Parse(row["access_group_id"].ToString().Trim());
						}
                        if (row["issued"] != DBNull.Value)
                        {
                            tag.Issued = DateTime.Parse(row["issued"].ToString().Trim());
                        }
                        if (row["valid_to"] != DBNull.Value)
                        {
                            tag.ValidTO = DateTime.Parse(row["valid_to"].ToString().Trim());
                        }

						tagsList.Add(tag.TagID, tag);
					}
				}
			}
			catch(Exception ex)
			{				
				throw new Exception("Exception: " + ex.Message);
			}

			return tagsList;
		}

		public List<TagTO> getInactiveTags(string wUnits, DateTime from, DateTime to)
		{
			DataSet dataSet = new DataSet();
			TagTO tag = new TagTO();
			List<TagTO> tagsList = new List<TagTO>();

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT t.tag_id, t.status, t.owner_id, e.last_name, e.first_name, t.modified_by, t.modified_time ");
				sb.Append("FROM tags t, employees e ");
				sb.Append("WHERE UPPER(t.status) in (N'" + Constants.statusLost.ToUpper() + "', N'" + Constants.statusDamaged.ToUpper() + "', N'" + Constants.statusReturned.ToUpper() + "', N'" + Constants.statusRetired.ToUpper() + "') ");
				sb.Append("AND DATE_FORMAT(t.modified_time,'%m/%d/%Y') >= '" + from.ToString("MM/dd/yyy") + "' ");
				sb.Append("AND DATE_FORMAT(t.modified_time,'%m/%d/%Y') <= '" + to.ToString("MM/dd/yyy") + "' ");
				sb.Append("AND t.owner_id = e.employee_id ");
				if (!wUnits.Equals(""))
				{
					sb.Append("AND e.working_unit_id IN (" + wUnits + ") ");
				}
				sb.Append("ORDER BY t.modified_time, t.status, e.last_name, e.first_name");

				MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "Tag");
				DataTable table = dataSet.Tables["Tag"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						tag = new TagTO();
						if (row["tag_id"] != DBNull.Value)
						{
							tag.TagID = UInt32.Parse(row["tag_id"].ToString().Trim());
						}
						if (row["status"] != DBNull.Value)
						{
							tag.Status = row["status"].ToString().Trim();
						}
						if (row["last_name"] != DBNull.Value)
						{
							tag.EmployeeName = row["last_name"].ToString().Trim();
						}
						if (row["first_name"] != DBNull.Value)
						{
							tag.EmployeeName += " " + row["first_name"].ToString().Trim();
						}
						if (row["modified_by"] != DBNull.Value)
						{
							tag.ModifiedBy = row["modified_by"].ToString().Trim();
						}
						if (row["modified_time"] != DBNull.Value)
						{
							tag.ModifiedTime = DateTime.Parse( row["modified_time"].ToString());
						}
						if (row["owner_id"] != DBNull.Value)
						{
							tag.OwnerID = (int) row["owner_id"];
						}
						tagsList.Add(tag);
					}
				}
			}
			catch(Exception ex)
			{				
				throw new Exception("Exception: " + ex.Message);
			}

			return tagsList;
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

		public void serialize(List<TagTO> TagTOList)
		{
			try
			{
				//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLTagsFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLTagsFile;
				this.serialize(TagTOList, filename);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void serialize(List<TagTO> TagTOList, String filename)
		{
			try
			{
				Stream stream = File.Open(filename, FileMode.Create);
				TagTO[] tagTOArray = (TagTO[]) TagTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
				bformatter.Serialize(stream, tagTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        public void serialize(List<TagTO> TagTOList, Stream stream)
        {
            try
            {
                TagTO[] tagTOArray = (TagTO[])TagTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
                bformatter.Serialize(stream, tagTOArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public List<TagTO> deserialize(string filePath)
		{
            List<TagTO> tagList = new List<TagTO>();

			try
			{
				if (File.Exists(filePath))
				{
					Stream stream = File.Open(filePath, FileMode.Open);
					XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
					TagTO[] deserialized;

					try
					{
						deserialized = (TagTO[]) bformatter.Deserialize(stream);
						ArrayList tags = ArrayList.Adapter(deserialized);

                        foreach (TagTO tag in tags)
                        {
                            tagList.Add(tag);
                        }
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

			return tagList;
		}

        public List<TagTO> deserialize(Stream stream)
        {
            List<TagTO> tagList = new List<TagTO>();

            try
            {
                XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
                TagTO[] deserialized;

                try
                {
                    deserialized = (TagTO[])bformatter.Deserialize(stream);
                    ArrayList tags = ArrayList.Adapter(deserialized);
                    foreach (TagTO tag in tags)
                    {
                        tagList.Add(tag);
                    }
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

            return tagList;
        }
	}
}
