using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLApplUserXApplUserCategoryDAO : ApplUserXApplUserCategoryDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLApplUserXApplUserCategoryDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLApplUserXApplUserCategoryDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(ApplUserXApplUserCategoryTO userXCatTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users_x_appl_users_categories (appl_users_category_id, user_id, default_category, created_by, created_time) VALUES (");
                sbInsert.Append("'" + userXCatTO.CategoryID.ToString().Trim() + "', ");
                sbInsert.Append("'" + userXCatTO.UserID.Trim() + "', ");
                sbInsert.Append("'" + userXCatTO.DefaultCategory.ToString().Trim() + "', ");

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

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

        public bool update(ApplUserXApplUserCategoryTO userXCatTO, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users_x_appl_users_categories SET ");
                if (userXCatTO.DefaultCategory != -1)
                    sbUpdate.Append("default_category = '" + userXCatTO.DefaultCategory.ToString().Trim() + "', "); ;

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE appl_users_category_id = '" + userXCatTO.CategoryID.ToString().Trim() + "' AND user_id = '" + userXCatTO.UserID.Trim() + "'");

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
                throw ex;
            }

            return isUpdated;
        }

        public bool setDefaultCategory(ApplUserXApplUserCategoryTO userXCatTO)
        {
            bool isUpdated = false;

            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();

                // set all categories to not default
                sbUpdate.Append("UPDATE appl_users_x_appl_users_categories SET ");
                sbUpdate.Append("default_category = '" + Constants.categoryNotDefault.ToString().Trim() + "', ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE user_id = '" + userXCatTO.UserID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                cmd.ExecuteNonQuery();

                sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users_x_appl_users_categories SET ");
                sbUpdate.Append("default_category = '" + Constants.categoryDefault.ToString().Trim() + "', ");
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE appl_users_category_id = '" + userXCatTO.CategoryID.ToString().Trim() + "' AND user_id = '" + userXCatTO.UserID.Trim() + "'");

                cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                cmd.ExecuteNonQuery();

                isUpdated = true;

                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw ex;
            }

            return isUpdated;
        }

        public bool delete(int categoryID, string userID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_x_appl_users_categories WHERE appl_users_category_id = '" + categoryID.ToString().Trim() + "' ");
                sbDelete.Append("AND user_id = '" + userID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("DELETE");
                throw ex;
            }

            return isDeleted;
        }

        public bool delete( string userID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_x_appl_users_categories WHERE " );
                sbDelete.Append(" user_id = '" + userID.Trim() + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                if (doCommit)
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                sqlTrans.Rollback("DELETE");
                throw ex;
            }

            return isDeleted;
        }
        public bool delete(int categoryID, string userID, bool doCommit)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = null;
            if (doCommit)
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");
            else
                sqlTrans = SqlTrans;

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_x_appl_users_categories WHERE ");
                sbDelete.Append(" user_id = '" + userID.Trim() + "'");
                sbDelete.Append(" AND appl_users_category_id = '" + categoryID + "'");

                SqlCommand cmd = new SqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                if (doCommit)
                    sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    sqlTrans.Rollback("DELETE");
                throw ex;
            }

            return isDeleted;
        }

        public Dictionary<string, List<int>> getUserCategoriesDict()
        {
            DataSet dataSet = new DataSet();
            ApplUserXApplUserCategoryTO userXCategoryTO = new ApplUserXApplUserCategoryTO();
            Dictionary<string, List<int>> userXCategoriesList = new Dictionary<string, List<int>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_x_appl_users_categories ");
               
                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        userXCategoryTO = new ApplUserXApplUserCategoryTO();
                        userXCategoryTO.CategoryID = Int32.Parse(row["appl_users_category_id"].ToString().Trim());

                        if (!row["default_category"].Equals(DBNull.Value))
                        {
                            userXCategoryTO.DefaultCategory = Int32.Parse(row["default_category"].ToString().Trim());
                        }
                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            userXCategoryTO.UserID = row["user_id"].ToString().Trim();
                        }
                        if(!userXCategoriesList.ContainsKey(userXCategoryTO.UserID))
                        userXCategoriesList.Add(userXCategoryTO.UserID,new List<int>());
                        if (!userXCategoriesList[userXCategoryTO.UserID].Contains(userXCategoryTO.CategoryID))
                            userXCategoriesList[userXCategoryTO.UserID].Add(userXCategoryTO.CategoryID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userXCategoriesList;
        }

        public List<ApplUserXApplUserCategoryTO> getUserCategories(ApplUserXApplUserCategoryTO userXCatTO, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            ApplUserXApplUserCategoryTO userXCategoryTO = new ApplUserXApplUserCategoryTO();
            List<ApplUserXApplUserCategoryTO> userXCategoriesList = new List<ApplUserXApplUserCategoryTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_x_appl_users_categories ");
                if ((userXCatTO.CategoryID != -1) || (userXCatTO.DefaultCategory != -1) || (!userXCatTO.UserID.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (userXCatTO.CategoryID != -1)
                    {
                        sb.Append(" appl_users_category_id = '" + userXCatTO.CategoryID.ToString().Trim() + "' AND");
                    }
                    if (userXCatTO.DefaultCategory != -1)
                    {
                        sb.Append(" default_category = '" + userXCatTO.DefaultCategory.ToString().Trim() + "' AND");
                    }
                    if (!userXCatTO.UserID.Trim().Equals(""))
                    {
                        sb.Append(" user_id = N'" + userXCatTO.UserID.Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd;

                if (trans != null)
                    cmd = new SqlCommand(select, conn, (SqlTransaction)trans);
                else
                    cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        userXCategoryTO = new ApplUserXApplUserCategoryTO();
                        userXCategoryTO.CategoryID = Int32.Parse(row["appl_users_category_id"].ToString().Trim());

                        if (!row["default_category"].Equals(DBNull.Value))
                        {
                            userXCategoryTO.DefaultCategory = Int32.Parse(row["default_category"].ToString().Trim());
                        }
                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            userXCategoryTO.UserID = row["user_id"].ToString().Trim();
                        }

                        userXCategoriesList.Add(userXCategoryTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userXCategoriesList;
        }

        public List<ApplUserXApplUserCategoryTO> getUserCategories(ApplUserXApplUserCategoryTO userXCatTO)
        {
            DataSet dataSet = new DataSet();
            ApplUserXApplUserCategoryTO userXCategoryTO = new ApplUserXApplUserCategoryTO();
            List<ApplUserXApplUserCategoryTO> userXCategoriesList = new List<ApplUserXApplUserCategoryTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_x_appl_users_categories ");
                if ((userXCatTO.CategoryID != -1) || (userXCatTO.DefaultCategory != -1) || (!userXCatTO.UserID.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (userXCatTO.CategoryID != -1)
                    {
                        sb.Append(" appl_users_category_id = '" + userXCatTO.CategoryID.ToString().Trim() + "' AND");
                    }
                    if (userXCatTO.DefaultCategory != -1)
                    {
                        sb.Append(" default_category = '" + userXCatTO.DefaultCategory.ToString().Trim() + "' AND");
                    }
                    if (!userXCatTO.UserID.Trim().Equals(""))
                    {
                        sb.Append(" user_id = N'" + userXCatTO.UserID.Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        userXCategoryTO = new ApplUserXApplUserCategoryTO();
                        userXCategoryTO.CategoryID = Int32.Parse(row["appl_users_category_id"].ToString().Trim());

                        if (!row["default_category"].Equals(DBNull.Value))
                        {
                            userXCategoryTO.DefaultCategory = Int32.Parse(row["default_category"].ToString().Trim());
                        }
                        if (!row["user_id"].Equals(DBNull.Value))
                        {
                            userXCategoryTO.UserID = row["user_id"].ToString().Trim();
                        }

                        userXCategoriesList.Add(userXCategoryTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userXCategoriesList;
        }

        public List<int> getSupervisors(string emplIDs)
        {
            DataSet dataSet = new DataSet();            
            List<int> supervisorsList = new List<int>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT employee_id FROM appl_users_x_appl_users_categories c, employees_asco4 a WHERE a.nvarchar_value_5 IS NOT NULL AND c.user_id = a.nvarchar_value_5 ");
                sb.Append("AND c.appl_users_category_id = " + ((int)Constants.Categories.TL).ToString().Trim() + " ");

                if (emplIDs.Trim() != "")
                    sb.Append("AND a.employee_id IN (" + emplIDs.Trim() + ")");

                select = sb.ToString();

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            supervisorsList.Add(Int32.Parse(row["employee_id"].ToString().Trim()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return supervisorsList;
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
                throw ex;
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
