using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Util;
using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MSSQLApplMenuItemDAO.
	/// </summary>
	public class MSSQLApplMenuItemDAO : ApplMenuItemDAO
	{
		SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MSSQLApplMenuItemDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
		}
        public MSSQLApplMenuItemDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
		// TODO!!!!
		public int insert(ApplMenuItemTO aplMenuItemTO)
		{
			int rowsAffected = 0;
			return rowsAffected;
		}

		public bool delete(string applMenuItemID)
		{
			bool isDeleted = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM appl_menu_items WHERE appl_menu_item_id = N'" + applMenuItemID.Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				isDeleted = true;
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("DELETE");
				
				throw new Exception(ex.Message);
			}

			return isDeleted;			
		}

		public bool update(ApplMenuItemTO applMenuItemTO)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE appl_menu_items SET ");
				
				/*if (!applMenuItemTO.ApplMenuItemID.Equals(""))
				{
					sbUpdate.Append("appl_menu_item_id = N'" + applMenuItemTO.ApplMenuItemID.Trim() + "', ");
				}*/
				if (!applMenuItemTO.Name.Trim().Equals(""))
				{
					sbUpdate.Append("name = N'" + applMenuItemTO.Name.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("name = null, ");
				}
				if (!applMenuItemTO.Description.Trim().Equals(""))
				{
					sbUpdate.Append("description = N'" + applMenuItemTO.Description.Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("description = null, ");
				}
                if (!applMenuItemTO.LangCode.Trim().Equals(""))
                {
                    sbUpdate.Append("lang_code = N'" + applMenuItemTO.LangCode.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("lang_code = null, ");
                }
                if (!applMenuItemTO.Position.Trim().Equals(""))
                {
                    sbUpdate.Append("position = N'" + applMenuItemTO.Position.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("position = null, ");
                }
				if (applMenuItemTO.PermitionRole0 != -1)
				{
					sbUpdate.Append("permition_role_0 = '" + applMenuItemTO.PermitionRole0.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole1 != -1)
				{
					sbUpdate.Append("permition_role_1 = '" + applMenuItemTO.PermitionRole1.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole2 != -1)
				{
					sbUpdate.Append("permition_role_2 = '" + applMenuItemTO.PermitionRole2.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole3 != -1)
				{
					sbUpdate.Append("permition_role_3 = '" + applMenuItemTO.PermitionRole3.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole4 != -1)
				{
					sbUpdate.Append("permition_role_4 = '" + applMenuItemTO.PermitionRole4.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole5 != -1)
				{
					sbUpdate.Append("permition_role_5 = '" + applMenuItemTO.PermitionRole5.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole6 != -1)
				{
					sbUpdate.Append("permition_role_6 = '" + applMenuItemTO.PermitionRole6.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole7 != -1)
				{
					sbUpdate.Append("permition_role_7 = '" + applMenuItemTO.PermitionRole7.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole8 != -1)
				{
					sbUpdate.Append("permition_role_8 = '" + applMenuItemTO.PermitionRole8.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole9 != -1)
				{
					sbUpdate.Append("permition_role_9 = '" + applMenuItemTO.PermitionRole9.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole10 != -1)
				{
					sbUpdate.Append("permition_role_10 = '" + applMenuItemTO.PermitionRole10.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole11 != -1)
				{
					sbUpdate.Append("permition_role_11 = '" + applMenuItemTO.PermitionRole11.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole12 != -1)
				{
					sbUpdate.Append("permition_role_12 = '" + applMenuItemTO.PermitionRole12.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole13 != -1)
				{
					sbUpdate.Append("permition_role_13 = '" + applMenuItemTO.PermitionRole13.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole14 != -1)
				{
					sbUpdate.Append("permition_role_14 = '" + applMenuItemTO.PermitionRole14.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole15 != -1)
				{
					sbUpdate.Append("permition_role_15 = '" + applMenuItemTO.PermitionRole15.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole16 != -1)
				{
					sbUpdate.Append("permition_role_16 = '" + applMenuItemTO.PermitionRole16.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole17 != -1)
				{
					sbUpdate.Append("permition_role_17 = '" + applMenuItemTO.PermitionRole17.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole18 != -1)
				{
					sbUpdate.Append("permition_role_18 = '" + applMenuItemTO.PermitionRole18.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole19 != -1)
				{
					sbUpdate.Append("permition_role_19 = '" + applMenuItemTO.PermitionRole19.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole20 != -1)
				{
					sbUpdate.Append("permition_role_20 = '" + applMenuItemTO.PermitionRole20.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole21 != -1)
				{
					sbUpdate.Append("permition_role_21 = '" + applMenuItemTO.PermitionRole21.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole22 != -1)
				{
					sbUpdate.Append("permition_role_22 = '" + applMenuItemTO.PermitionRole22.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole23 != -1)
				{
					sbUpdate.Append("permition_role_23 = '" + applMenuItemTO.PermitionRole23.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole24 != -1)
				{
					sbUpdate.Append("permition_role_24 = '" + applMenuItemTO.PermitionRole24.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole25 != -1)
				{
					sbUpdate.Append("permition_role_25 = '" + applMenuItemTO.PermitionRole25.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole26 != -1)
				{
					sbUpdate.Append("permition_role_26 = '" + applMenuItemTO.PermitionRole26.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole27 != -1)
				{
					sbUpdate.Append("permition_role_27 = '" + applMenuItemTO.PermitionRole27.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole28 != -1)
				{
					sbUpdate.Append("permition_role_28 = '" + applMenuItemTO.PermitionRole28.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole29 != -1)
				{
					sbUpdate.Append("permition_role_29 = '" + applMenuItemTO.PermitionRole29.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole30 != -1)
				{
					sbUpdate.Append("permition_role_30 = '" + applMenuItemTO.PermitionRole30.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole31 != -1)
				{
					sbUpdate.Append("permition_role_31 = '" + applMenuItemTO.PermitionRole31.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole32 != -1)
				{
					sbUpdate.Append("permition_role_32 = '" + applMenuItemTO.PermitionRole32.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole33 != -1)
				{
					sbUpdate.Append("permition_role_33 = '" + applMenuItemTO.PermitionRole33.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole34 != -1)
				{
					sbUpdate.Append("permition_role_34 = '" + applMenuItemTO.PermitionRole34.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole35 != -1)
				{
					sbUpdate.Append("permition_role_35 = '" + applMenuItemTO.PermitionRole35.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole36 != -1)
				{
					sbUpdate.Append("permition_role_36 = '" + applMenuItemTO.PermitionRole36.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole37 != -1)
				{
					sbUpdate.Append("permition_role_37 = '" + applMenuItemTO.PermitionRole37.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole38 != -1)
				{
					sbUpdate.Append("permition_role_38 = '" + applMenuItemTO.PermitionRole38.ToString().Trim() + "', ");
				}
				if (applMenuItemTO.PermitionRole39 != -1)
				{
					sbUpdate.Append("permition_role_39 = '" + applMenuItemTO.PermitionRole39.ToString().Trim() + "', ");
				}
								
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				sbUpdate.Append("WHERE appl_menu_item_id = N'" + applMenuItemTO.ApplMenuItemID.Trim() + "'");
				
				SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;	
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback("UPDATE");
				
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

        public bool updateSamePosition(ApplMenuItemTO applMenuItemTO)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "UPDATE");

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_menu_items SET ");

                if (applMenuItemTO.PermitionRole0 != -1)
                {
                    sbUpdate.Append("permition_role_0 = '" + applMenuItemTO.PermitionRole0.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole1 != -1)
                {
                    sbUpdate.Append("permition_role_1 = '" + applMenuItemTO.PermitionRole1.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole2 != -1)
                {
                    sbUpdate.Append("permition_role_2 = '" + applMenuItemTO.PermitionRole2.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole3 != -1)
                {
                    sbUpdate.Append("permition_role_3 = '" + applMenuItemTO.PermitionRole3.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole4 != -1)
                {
                    sbUpdate.Append("permition_role_4 = '" + applMenuItemTO.PermitionRole4.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole5 != -1)
                {
                    sbUpdate.Append("permition_role_5 = '" + applMenuItemTO.PermitionRole5.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole6 != -1)
                {
                    sbUpdate.Append("permition_role_6 = '" + applMenuItemTO.PermitionRole6.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole7 != -1)
                {
                    sbUpdate.Append("permition_role_7 = '" + applMenuItemTO.PermitionRole7.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole8 != -1)
                {
                    sbUpdate.Append("permition_role_8 = '" + applMenuItemTO.PermitionRole8.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole9 != -1)
                {
                    sbUpdate.Append("permition_role_9 = '" + applMenuItemTO.PermitionRole9.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole10 != -1)
                {
                    sbUpdate.Append("permition_role_10 = '" + applMenuItemTO.PermitionRole10.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole11 != -1)
                {
                    sbUpdate.Append("permition_role_11 = '" + applMenuItemTO.PermitionRole11.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole12 != -1)
                {
                    sbUpdate.Append("permition_role_12 = '" + applMenuItemTO.PermitionRole12.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole13 != -1)
                {
                    sbUpdate.Append("permition_role_13 = '" + applMenuItemTO.PermitionRole13.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole14 != -1)
                {
                    sbUpdate.Append("permition_role_14 = '" + applMenuItemTO.PermitionRole14.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole15 != -1)
                {
                    sbUpdate.Append("permition_role_15 = '" + applMenuItemTO.PermitionRole15.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole16 != -1)
                {
                    sbUpdate.Append("permition_role_16 = '" + applMenuItemTO.PermitionRole16.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole17 != -1)
                {
                    sbUpdate.Append("permition_role_17 = '" + applMenuItemTO.PermitionRole17.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole18 != -1)
                {
                    sbUpdate.Append("permition_role_18 = '" + applMenuItemTO.PermitionRole18.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole19 != -1)
                {
                    sbUpdate.Append("permition_role_19 = '" + applMenuItemTO.PermitionRole19.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole20 != -1)
                {
                    sbUpdate.Append("permition_role_20 = '" + applMenuItemTO.PermitionRole20.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole21 != -1)
                {
                    sbUpdate.Append("permition_role_21 = '" + applMenuItemTO.PermitionRole21.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole22 != -1)
                {
                    sbUpdate.Append("permition_role_22 = '" + applMenuItemTO.PermitionRole22.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole23 != -1)
                {
                    sbUpdate.Append("permition_role_23 = '" + applMenuItemTO.PermitionRole23.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole24 != -1)
                {
                    sbUpdate.Append("permition_role_24 = '" + applMenuItemTO.PermitionRole24.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole25 != -1)
                {
                    sbUpdate.Append("permition_role_25 = '" + applMenuItemTO.PermitionRole25.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole26 != -1)
                {
                    sbUpdate.Append("permition_role_26 = '" + applMenuItemTO.PermitionRole26.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole27 != -1)
                {
                    sbUpdate.Append("permition_role_27 = '" + applMenuItemTO.PermitionRole27.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole28 != -1)
                {
                    sbUpdate.Append("permition_role_28 = '" + applMenuItemTO.PermitionRole28.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole29 != -1)
                {
                    sbUpdate.Append("permition_role_29 = '" + applMenuItemTO.PermitionRole29.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole30 != -1)
                {
                    sbUpdate.Append("permition_role_30 = '" + applMenuItemTO.PermitionRole30.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole31 != -1)
                {
                    sbUpdate.Append("permition_role_31 = '" + applMenuItemTO.PermitionRole31.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole32 != -1)
                {
                    sbUpdate.Append("permition_role_32 = '" + applMenuItemTO.PermitionRole32.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole33 != -1)
                {
                    sbUpdate.Append("permition_role_33 = '" + applMenuItemTO.PermitionRole33.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole34 != -1)
                {
                    sbUpdate.Append("permition_role_34 = '" + applMenuItemTO.PermitionRole34.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole35 != -1)
                {
                    sbUpdate.Append("permition_role_35 = '" + applMenuItemTO.PermitionRole35.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole36 != -1)
                {
                    sbUpdate.Append("permition_role_36 = '" + applMenuItemTO.PermitionRole36.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole37 != -1)
                {
                    sbUpdate.Append("permition_role_37 = '" + applMenuItemTO.PermitionRole37.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole38 != -1)
                {
                    sbUpdate.Append("permition_role_38 = '" + applMenuItemTO.PermitionRole38.ToString().Trim() + "', ");
                }
                if (applMenuItemTO.PermitionRole39 != -1)
                {
                    sbUpdate.Append("permition_role_39 = '" + applMenuItemTO.PermitionRole39.ToString().Trim() + "', ");
                }


                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE position = N'" + applMenuItemTO.Position.Trim() + "'");

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

		public bool updateEmptyRole(int applRoleID, bool doCommit)
		{
			bool isUpdated = false;
			SqlTransaction sqlTrans = null;

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
				sbUpdate.Append("UPDATE appl_menu_items SET ");
				sbUpdate.Append("permition_role_" + applRoleID.ToString().Trim() + " = '0', ");
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = GETDATE() ");
				
				SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
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

		public ApplMenuItemTO find(string applMenuItemID)
		{
			DataSet dataSet = new DataSet();
			ApplMenuItemTO applMenuItemTO = new ApplMenuItemTO();
			try
			{
				SqlCommand cmd = new SqlCommand( "SELECT * FROM appl_menu_items WHERE appl_menu_item_id = N'" + applMenuItemID.Trim() + "'", conn );
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplMenuItems");
				DataTable table = dataSet.Tables["ApplMenuItems"];

				if (table.Rows.Count == 1)
				{
					applMenuItemTO = new ApplMenuItemTO();
					if (!table.Rows[0]["appl_menu_item_id"].Equals(DBNull.Value))
					{
						applMenuItemTO.ApplMenuItemID = table.Rows[0]["appl_menu_item_id"].ToString().Trim();
					}
					if (!table.Rows[0]["name"].Equals(DBNull.Value))
					{
						applMenuItemTO.Name = table.Rows[0]["name"].ToString().Trim();
					}
					if (!table.Rows[0]["description"].Equals(DBNull.Value))
					{
						applMenuItemTO.Description = table.Rows[0]["description"].ToString().Trim();
					}
                    if (!table.Rows[0]["lang_code"].Equals(DBNull.Value))
                    {
                        applMenuItemTO.LangCode = table.Rows[0]["lang_code"].ToString().Trim();
                    }
                    if (!table.Rows[0]["position"].Equals(DBNull.Value))
                    {
                        applMenuItemTO.Position = table.Rows[0]["position"].ToString().Trim();
                    }
					if (!table.Rows[0]["permition_role_0"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole0 = Int32.Parse(table.Rows[0]["permition_role_0"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_1"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole1 = Int32.Parse(table.Rows[0]["permition_role_1"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_2"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole2 = Int32.Parse(table.Rows[0]["permition_role_2"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_3"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole3 = Int32.Parse(table.Rows[0]["permition_role_3"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_4"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole4 = Int32.Parse(table.Rows[0]["permition_role_4"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_5"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole5 = Int32.Parse(table.Rows[0]["permition_role_5"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_6"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole6 = Int32.Parse(table.Rows[0]["permition_role_6"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_7"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole7 = Int32.Parse(table.Rows[0]["permition_role_7"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_8"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole8 = Int32.Parse(table.Rows[0]["permition_role_8"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_9"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole9 = Int32.Parse(table.Rows[0]["permition_role_9"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_10"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole10 = Int32.Parse(table.Rows[0]["permition_role_10"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_11"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole11 = Int32.Parse(table.Rows[0]["permition_role_11"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_12"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole12 = Int32.Parse(table.Rows[0]["permition_role_12"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_13"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole13 = Int32.Parse(table.Rows[0]["permition_role_13"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_14"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole14 = Int32.Parse(table.Rows[0]["permition_role_14"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_15"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole15 = Int32.Parse(table.Rows[0]["permition_role_15"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_16"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole16 = Int32.Parse(table.Rows[0]["permition_role_16"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_17"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole17 = Int32.Parse(table.Rows[0]["permition_role_17"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_18"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole18 = Int32.Parse(table.Rows[0]["permition_role_18"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_19"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole19 = Int32.Parse(table.Rows[0]["permition_role_19"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_20"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole20 = Int32.Parse(table.Rows[0]["permition_role_20"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_21"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole21 = Int32.Parse(table.Rows[0]["permition_role_21"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_22"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole22 = Int32.Parse(table.Rows[0]["permition_role_22"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_23"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole23 = Int32.Parse(table.Rows[0]["permition_role_23"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_24"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole24 = Int32.Parse(table.Rows[0]["permition_role_24"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_25"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole25 = Int32.Parse(table.Rows[0]["permition_role_25"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_26"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole26 = Int32.Parse(table.Rows[0]["permition_role_26"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_27"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole27 = Int32.Parse(table.Rows[0]["permition_role_27"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_28"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole28 = Int32.Parse(table.Rows[0]["permition_role_28"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_29"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole29 = Int32.Parse(table.Rows[0]["permition_role_29"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_30"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole30 = Int32.Parse(table.Rows[0]["permition_role_30"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_31"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole31 = Int32.Parse(table.Rows[0]["permition_role_31"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_32"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole32 = Int32.Parse(table.Rows[0]["permition_role_32"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_33"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole33 = Int32.Parse(table.Rows[0]["permition_role_33"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_34"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole34 = Int32.Parse(table.Rows[0]["permition_role_34"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_35"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole35 = Int32.Parse(table.Rows[0]["permition_role_35"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_36"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole36 = Int32.Parse(table.Rows[0]["permition_role_36"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_37"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole37 = Int32.Parse(table.Rows[0]["permition_role_37"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_38"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole38 = Int32.Parse(table.Rows[0]["permition_role_38"].ToString().Trim());
					}
					if (!table.Rows[0]["permition_role_39"].Equals(DBNull.Value))
					{
						applMenuItemTO.PermitionRole39 = Int32.Parse(table.Rows[0]["permition_role_39"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applMenuItemTO;
		}

		public List<ApplMenuItemTO> getApplMenuItems(ApplMenuItemTO menuItemTO)
		{
			DataSet dataSet = new DataSet();
			ApplMenuItemTO applMenuItemTO = new ApplMenuItemTO();
            List<ApplMenuItemTO> applMenuItemList = new List<ApplMenuItemTO>();
			string select = "";

			try
			{
				bool existPermitionRole = false;
                int[] permitionRoles = menuItemTO.PermissionsToArray();
				foreach (int permitionRole in permitionRoles)
				{
					if (permitionRole != -1)
					{
						existPermitionRole = true;
						break;
					}
				}

				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT * FROM appl_menu_items ");

                if (!menuItemTO.LangCode.Trim().Equals(""))
                {
                    sb.Append("WHERE UPPER(lang_code) LIKE N'%" + menuItemTO.LangCode.ToUpper().Trim() + "%' AND");
                }

                if (existPermitionRole && (!menuItemTO.ApplMenuItemID.Trim().Equals("")) ||
                    (!menuItemTO.Name.Trim().Equals("")) || (!menuItemTO.Description.Trim().Equals("")) ||
                    (!menuItemTO.LangCode.Trim().Equals("")) || (!menuItemTO.Position.Trim().Equals("")))
                {
                    if (!menuItemTO.ApplMenuItemID.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(appl_menu_item_id) LIKE '" + menuItemTO.ApplMenuItemID.ToUpper().Trim() + "' AND");
                    }
                    if (!menuItemTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) LIKE N'" + menuItemTO.Name.ToUpper().Trim() + "' AND");
                    }
                    if (!menuItemTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'" + menuItemTO.Description.ToUpper().Trim() + "' AND");
                    }
                    if (!menuItemTO.Position.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(position) LIKE N'" + menuItemTO.Position.ToUpper().Trim() + "' AND");
                    }
                    for (int i = 0; i < permitionRoles.Length; i++)
                    {
                        if (permitionRoles[i] != -1)
                        {
                            sb.Append(" UPPER(permition_role_" + i.ToString().Trim() + ") LIKE '" + permitionRoles[i].ToString().Trim() + "' AND");
                        }
                    }
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else if (!menuItemTO.LangCode.Equals(""))
                {
                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }
				
				select = select + "ORDER BY position, appl_menu_item_id ";

				SqlCommand cmd = new SqlCommand(select, conn);
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "ApplMenuItem");
				DataTable table = dataSet.Tables["ApplMenuItem"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						applMenuItemTO = new ApplMenuItemTO();
							
						if (!row["appl_menu_item_id"].Equals(DBNull.Value))
						{
							applMenuItemTO.ApplMenuItemID = row["appl_menu_item_id"].ToString().Trim();
						}
						if (!row["name"].Equals(DBNull.Value))
						{
							applMenuItemTO.Name = row["name"].ToString().Trim();
						}
						if (!row["description"].Equals(DBNull.Value))
						{
							applMenuItemTO.Description = row["description"].ToString().Trim();
						}
                        if (!row["lang_code"].Equals(DBNull.Value))
                        {
                            applMenuItemTO.LangCode = row["lang_code"].ToString().Trim();
                        }
                        if (!row["position"].Equals(DBNull.Value))
                        {
                            applMenuItemTO.Position = row["position"].ToString().Trim();
                        }
						if (!row["permition_role_0"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole0 = Int32.Parse(row["permition_role_0"].ToString().Trim());
						}
						if (!row["permition_role_1"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole1 = Int32.Parse(row["permition_role_1"].ToString().Trim());
						}
						if (!row["permition_role_2"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole2 = Int32.Parse(row["permition_role_2"].ToString().Trim());
						}
						if (!row["permition_role_3"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole3 = Int32.Parse(row["permition_role_3"].ToString().Trim());
						}
						if (!row["permition_role_4"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole4 = Int32.Parse(row["permition_role_4"].ToString().Trim());
						}
						if (!row["permition_role_5"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole5 = Int32.Parse(row["permition_role_5"].ToString().Trim());
						}
						if (!row["permition_role_6"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole6 = Int32.Parse(row["permition_role_6"].ToString().Trim());
						}
						if (!row["permition_role_7"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole7 = Int32.Parse(row["permition_role_7"].ToString().Trim());
						}
						if (!row["permition_role_8"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole8 = Int32.Parse(row["permition_role_8"].ToString().Trim());
						}
						if (!row["permition_role_9"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole9 = Int32.Parse(row["permition_role_9"].ToString().Trim());
						}
						if (!row["permition_role_10"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole10 = Int32.Parse(row["permition_role_10"].ToString().Trim());
						}
						if (!row["permition_role_11"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole11 = Int32.Parse(row["permition_role_11"].ToString().Trim());
						}
						if (!row["permition_role_12"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole12 = Int32.Parse(row["permition_role_12"].ToString().Trim());
						}
						if (!row["permition_role_13"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole13 = Int32.Parse(row["permition_role_13"].ToString().Trim());
						}
						if (!row["permition_role_14"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole14 = Int32.Parse(row["permition_role_14"].ToString().Trim());
						}
						if (!row["permition_role_15"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole15 = Int32.Parse(row["permition_role_15"].ToString().Trim());
						}
						if (!row["permition_role_16"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole16 = Int32.Parse(row["permition_role_16"].ToString().Trim());
						}
						if (!row["permition_role_17"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole17 = Int32.Parse(row["permition_role_17"].ToString().Trim());
						}
						if (!row["permition_role_18"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole18 = Int32.Parse(row["permition_role_18"].ToString().Trim());
						}
						if (!row["permition_role_19"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole19 = Int32.Parse(row["permition_role_19"].ToString().Trim());
						}
						if (!row["permition_role_20"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole20 = Int32.Parse(row["permition_role_20"].ToString().Trim());
						}
						if (!row["permition_role_21"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole21 = Int32.Parse(row["permition_role_21"].ToString().Trim());
						}
						if (!row["permition_role_22"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole22 = Int32.Parse(row["permition_role_22"].ToString().Trim());
						}
						if (!row["permition_role_23"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole23 = Int32.Parse(row["permition_role_23"].ToString().Trim());
						}
						if (!row["permition_role_24"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole24 = Int32.Parse(row["permition_role_24"].ToString().Trim());
						}
						if (!row["permition_role_25"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole25 = Int32.Parse(row["permition_role_25"].ToString().Trim());
						}
						if (!row["permition_role_26"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole26 = Int32.Parse(row["permition_role_26"].ToString().Trim());
						}
						if (!row["permition_role_27"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole27 = Int32.Parse(row["permition_role_27"].ToString().Trim());
						}
						if (!row["permition_role_28"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole28 = Int32.Parse(row["permition_role_28"].ToString().Trim());
						}
						if (!row["permition_role_29"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole29 = Int32.Parse(row["permition_role_29"].ToString().Trim());
						}
						if (!row["permition_role_30"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole30 = Int32.Parse(row["permition_role_30"].ToString().Trim());
						}
						if (!row["permition_role_31"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole31 = Int32.Parse(row["permition_role_31"].ToString().Trim());
						}
						if (!row["permition_role_32"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole32 = Int32.Parse(row["permition_role_32"].ToString().Trim());
						}
						if (!row["permition_role_33"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole33 = Int32.Parse(row["permition_role_33"].ToString().Trim());
						}
						if (!row["permition_role_34"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole34 = Int32.Parse(row["permition_role_34"].ToString().Trim());
						}
						if (!row["permition_role_35"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole35 = Int32.Parse(row["permition_role_35"].ToString().Trim());
						}
						if (!row["permition_role_36"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole36 = Int32.Parse(row["permition_role_36"].ToString().Trim());
						}
						if (!row["permition_role_37"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole37 = Int32.Parse(row["permition_role_37"].ToString().Trim());
						}
						if (!row["permition_role_38"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole38 = Int32.Parse(row["permition_role_38"].ToString().Trim());
						}
						if (!row["permition_role_39"].Equals(DBNull.Value))
						{
							applMenuItemTO.PermitionRole39 = Int32.Parse(row["permition_role_39"].ToString().Trim());
						}

						applMenuItemList.Add(applMenuItemTO);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return applMenuItemList;
		}

		//TODO!!!!!
		public void serialize(List<ApplMenuItemTO> applMenuItemsTOList)
		{
			try
			{
				string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLApplMenuItemFile"];
				//string filename =  + ConfigurationManager.AppSettings["XMLApplMenuItemFile"];

				Stream stream = File.Open(filename, FileMode.Create);

				ApplMenuItemTO[] applMenuItemTOArray = (ApplMenuItemTO[]) applMenuItemsTOList.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(ApplMenuItemTO[]));
				bformatter.Serialize(stream, applMenuItemTOArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}
