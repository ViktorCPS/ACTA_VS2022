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
    public class MSSQLApplUserCategoryXPassTypeDAO : ApplUserCategoryXPassTypeDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MSSQLApplUserCategoryXPassTypeDAO()
		{
			conn = MSSQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}

        public MSSQLApplUserCategoryXPassTypeDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }


        public int insert(ApplUserCategoryXPassTypeTO catXptTO, bool doCommit)
        {
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO appl_users_categories_x_pass_types (appl_users_category_id, pass_type_id, purpose, created_by, created_time) VALUES (");
                sbInsert.Append("'" + catXptTO.CategoryID.ToString().Trim() + "', ");
                sbInsert.Append("'" + catXptTO.PassTypeID.ToString().Trim() + "', ");
                sbInsert.Append("'" + catXptTO.Purpose.Trim() + "', ");
                
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
        public bool delete(int passTypeID, bool doCommit)
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
                sbDelete.Append("DELETE FROM appl_users_categories_x_pass_types WHERE " );
                sbDelete.Append(" pass_type_id = " + passTypeID.ToString().Trim() );

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
        public bool delete(int categoryID, int ptID, string purpose)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM appl_users_categories_x_pass_types WHERE appl_users_category_id = '" + categoryID.ToString().Trim() + "' ");
                sbDelete.Append("AND pass_type_id = '" + ptID.ToString().Trim() + "' AND UPPER(purpose) = '" + purpose.Trim().ToUpper() + "'");

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

        public Dictionary<int, List<int>> getUserCategoriesXPassTypesDictionary(ApplUserCategoryXPassTypeTO catXptTO)
        {
            DataSet dataSet = new DataSet();
            ApplUserCategoryXPassTypeTO userCatPtTO = new ApplUserCategoryXPassTypeTO();
            Dictionary<int, List<int>> categoryPtList = new Dictionary<int, List<int>>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_categories_x_pass_types ");
                if ((catXptTO.CategoryID != -1) || (catXptTO.PassTypeID != -1) || (!catXptTO.Purpose.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (catXptTO.CategoryID != -1)
                    {
                        sb.Append(" appl_users_category_id = '" + catXptTO.CategoryID.ToString().Trim() + "' AND");
                    }
                    if (catXptTO.PassTypeID != -1)
                    {
                        sb.Append(" pass_type_id = '" + catXptTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!catXptTO.Purpose.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(purpose) = N'" + catXptTO.Purpose.ToUpper().Trim() + "' AND");
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
                        userCatPtTO = new ApplUserCategoryXPassTypeTO();
                        userCatPtTO.CategoryID = Int32.Parse(row["appl_users_category_id"].ToString().Trim());

                        if (!row["pass_type_id"].Equals(DBNull.Value))
                        {
                            userCatPtTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (!row["purpose"].Equals(DBNull.Value))
                        {
                            userCatPtTO.Purpose = row["purpose"].ToString().Trim();
                        }
                        if (!categoryPtList.ContainsKey(userCatPtTO.PassTypeID))
                            categoryPtList.Add(userCatPtTO.PassTypeID, new List<int>());
                        categoryPtList[userCatPtTO.PassTypeID].Add(userCatPtTO.CategoryID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return categoryPtList;
        }

        public List<ApplUserCategoryXPassTypeTO> getUserCategoriesXPassTypes(ApplUserCategoryXPassTypeTO catXptTO)
        {
            DataSet dataSet = new DataSet();
            ApplUserCategoryXPassTypeTO userCatPtTO = new ApplUserCategoryXPassTypeTO();
            List<ApplUserCategoryXPassTypeTO> categoryPtList = new List<ApplUserCategoryXPassTypeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM appl_users_categories_x_pass_types ");
                if ((catXptTO.CategoryID != -1) || (catXptTO.PassTypeID != -1) || (!catXptTO.Purpose.Trim().Equals("")))
                {
                    sb.Append(" WHERE");
                    if (catXptTO.CategoryID != -1)
                    {
                        sb.Append(" appl_users_category_id = '" + catXptTO.CategoryID.ToString().Trim() + "' AND");
                    }
                    if (catXptTO.PassTypeID != -1)
                    {
                        sb.Append(" pass_type_id = '" + catXptTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!catXptTO.Purpose.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(purpose) = N'" + catXptTO.Purpose.ToUpper().Trim() + "' AND");
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
                        userCatPtTO = new ApplUserCategoryXPassTypeTO();
                        userCatPtTO.CategoryID = Int32.Parse(row["appl_users_category_id"].ToString().Trim());

                        if (!row["pass_type_id"].Equals(DBNull.Value))
                        {
                            userCatPtTO.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (!row["purpose"].Equals(DBNull.Value))
                        {
                            userCatPtTO.Purpose = row["purpose"].ToString().Trim();
                        }

                        categoryPtList.Add(userCatPtTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return categoryPtList;
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
