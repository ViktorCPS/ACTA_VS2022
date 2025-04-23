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
    public class MSSQLApplUserCategoryDAO : ApplUserCategoryDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLApplUserCategoryDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MSSQLApplUserCategoryDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(ApplUserCategoryTO userCatTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users_categories (appl_users_category_id, description, name, created_by, created_time) VALUES (");
                sbInsert.Append("'" + userCatTO.CategoryID.ToString().Trim() + "', ");
                if (!userCatTO.Desc.Trim().Equals(""))
                    sbInsert.Append("N'" + userCatTO.Desc.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                if (!userCatTO.Name.Trim().Equals(""))
                    sbInsert.Append("N'" + userCatTO.Name.Trim() + "', ");
                else
                    sbInsert.Append("NULL, ");
                
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

        public bool delete(int categoryID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_categories WHERE appl_users_category_id = '" + categoryID.ToString().Trim() + "'");

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

        public bool update(ApplUserCategoryTO userCatTO, bool doCommit)
        {
            bool isUpdated = false;

            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE appl_users_categories SET ");
                if (!userCatTO.Desc.Trim().Equals(""))
                    sbUpdate.Append("description = N'" + userCatTO.Desc.Trim() + "', ");
                else
                    sbUpdate.Append("description = NULL, ");
                if (!userCatTO.Name.Trim().Equals(""))
                    sbUpdate.Append("name = N'" + userCatTO.Name.Trim() + "', ");
                else
                    sbUpdate.Append("name = NULL, ");
                
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = GETDATE() ");
                sbUpdate.Append("WHERE appl_users_category_id = '" + userCatTO.CategoryID.ToString().Trim() + "'");

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

        public ApplUserCategoryTO find(int categoryID)
        {
            DataSet dataSet = new DataSet();
            ApplUserCategoryTO userCatTO = new ApplUserCategoryTO();

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM appl_users_categories WHERE appl_users_category_id = '" + categoryID.ToString().Trim() + "'", conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];

                if (table.Rows.Count == 1)
                {
                    userCatTO = new ApplUserCategoryTO();
                    userCatTO.CategoryID = Int32.Parse(table.Rows[0]["appl_users_category_id"].ToString().Trim());

                    if (!table.Rows[0]["description"].Equals(DBNull.Value))
                    {
                        userCatTO.Desc = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (!table.Rows[0]["name"].Equals(DBNull.Value))
                    {
                        userCatTO.Name = table.Rows[0]["name"].ToString().Trim();
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userCatTO;
        }

        public List<ApplUserCategoryTO> getUserCategories(ApplUserCategoryTO userCatTO, bool includeMCCategories)
        {
            DataSet dataSet = new DataSet();
            ApplUserCategoryTO categoryTO = new ApplUserCategoryTO();
            List<ApplUserCategoryTO> categoryList = new List<ApplUserCategoryTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_categories ");
                if ((userCatTO.CategoryID != -1) || (!userCatTO.Desc.Trim().Equals("")) || (!userCatTO.Name.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (userCatTO.CategoryID != -1)
                    {
                        sb.Append(" appl_users_category_id = '" + userCatTO.CategoryID.ToString().Trim() + "' AND");
                    }
                    if (!userCatTO.Desc.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) = N'" + userCatTO.Desc.ToUpper().Trim() + "' AND");
                    }
                    if (!userCatTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) = N'" + userCatTO.Name.ToUpper().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY description ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];
                
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        categoryTO = new ApplUserCategoryTO();
                        categoryTO.CategoryID = Int32.Parse(row["appl_users_category_id"].ToString().Trim());

                        if (!includeMCCategories && Constants.MCCategories.Contains(categoryTO.CategoryID))
                            continue;

                        if (!row["description"].Equals(DBNull.Value))
                        {
                            categoryTO.Desc = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            categoryTO.Name = row["name"].ToString().Trim();
                        }

                        categoryList.Add(categoryTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return categoryList;
        }

        public List<ApplUserCategoryTO> getUserCategories(ApplUserCategoryTO userCatTO)
        {
            DataSet dataSet = new DataSet();
            ApplUserCategoryTO categoryTO = new ApplUserCategoryTO();
            List<ApplUserCategoryTO> categoryList = new List<ApplUserCategoryTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_categories ");
                if ((userCatTO.CategoryID != -1) || (!userCatTO.Desc.Trim().Equals("")) || (!userCatTO.Name.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (userCatTO.CategoryID != -1)
                    {
                        sb.Append(" appl_users_category_id = '" + userCatTO.CategoryID.ToString().Trim() + "' AND");
                    }
                    if (!userCatTO.Desc.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) = N'" + userCatTO.Desc.ToUpper().Trim() + "' AND");
                    }
                    if (!userCatTO.Name.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(name) = N'" + userCatTO.Name.ToUpper().Trim() + "' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY description ";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        categoryTO = new ApplUserCategoryTO();
                        categoryTO.CategoryID = Int32.Parse(row["appl_users_category_id"].ToString().Trim());

                       

                        if (!row["description"].Equals(DBNull.Value))
                        {
                            categoryTO.Desc = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            categoryTO.Name = row["name"].ToString().Trim();
                        }

                        categoryList.Add(categoryTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return categoryList;
        }

        public List<ApplUserCategoryTO> getUserCategoriesForLoginUser(string userID, bool getDefault)
        {
            DataSet dataSet = new DataSet();
            ApplUserCategoryTO categoryTO = new ApplUserCategoryTO();
            List<ApplUserCategoryTO> categoryList = new List<ApplUserCategoryTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM appl_users_categories WHERE appl_users_category_id IN "
                    + "(SELECT appl_users_category_id FROM appl_users_x_appl_users_categories WHERE user_id = '" + userID.Trim() + "' ";
                if (getDefault)
                    select += "AND default_category = '" + Constants.categoryDefault.ToString().Trim() + "' ";
                select += ") ORDER BY description";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Categories");
                DataTable table = dataSet.Tables["Categories"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        categoryTO = new ApplUserCategoryTO();
                        categoryTO.CategoryID = Int32.Parse(row["appl_users_category_id"].ToString().Trim());

                        if (!row["description"].Equals(DBNull.Value))
                        {
                            categoryTO.Desc = row["description"].ToString().Trim();
                        }
                        if (!row["name"].Equals(DBNull.Value))
                        {
                            categoryTO.Name = row["name"].ToString().Trim();
                        }

                        categoryList.Add(categoryTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return categoryList;
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

