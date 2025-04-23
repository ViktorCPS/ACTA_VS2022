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
    public class MSSQLMealTypeEmplDAO:MealTypeEmplDAO
    {
        SqlConnection conn = null;
		SqlTransaction _sqlTrans = null;

		public SqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}
        public MSSQLMealTypeEmplDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
        }
        public MSSQLMealTypeEmplDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
        }
        public int insert(string name, string description)
        {
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "INSERT");
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO  meals_type_empl ");
                sbInsert.Append("(name, description , created_by, created_time ) ");
                sbInsert.Append("VALUES (");
                if (!name.Equals(""))
                {
                    sbInsert.Append("N'" + name + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!description.Equals(""))
                {
                    sbInsert.Append("N'" + description + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");

                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqlTrans);

                rowsAffected = cmd.ExecuteNonQuery();
                sqlTrans.Commit();

            }
            catch (SqlException sqlEx)
            {
                sqlTrans.Rollback("INSERT");
                if (sqlEx.Number == 2627)
                {
                    throw new Exception(sqlEx.Number.ToString());
                }
                else
                {
                    throw new Exception(sqlEx.Message);
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback("INSERT");
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(string mealsTypeEmplID)
        {
            bool isDeleted = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead, "DELETE");

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_type_empl WHERE meals_type_empl_id = '" + mealsTypeEmplID + "'");

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

        public ArrayList getMealsTypeEmpl(int mealTypeEmplID, string name, string description)
        {
            DataSet dataSet = new DataSet();
            MealTypeEmplTO mealTypeEmpl = new MealTypeEmplTO();
            ArrayList mealTypeEmplList = new ArrayList();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM meals_type_empl ");
                if ((mealTypeEmplID != -1) || (!name.Equals("")) || (!description.Equals("")) )
                {
                    sb.Append("WHERE ");
                    if (mealTypeEmplID != -1)
                    {
                        sb.Append("meals_type_empl_id = '" + mealTypeEmplID.ToString().Trim() + "' AND ");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append("name = N'" + name + "' AND ");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append("description = N'" + description + "' AND ");
                    }
                    
                    select = sb.ToString(0, sb.Length - 4);
                }
                else
                {
                    select = sb.ToString();
                }

                select = select + "ORDER BY name";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealsTypeEmpl");
                DataTable table = dataSet.Tables["MealsTypeEmpl"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealTypeEmpl = new MealTypeEmplTO();
                        mealTypeEmpl.MealTypeEmplID = int.Parse(row["meals_type_empl_id"].ToString().Trim());
                        if (row["name"] != DBNull.Value)
                        {
                            mealTypeEmpl.Name = row["name"].ToString().Trim();
                        }
                        if (row["description"] != DBNull.Value)
                        {
                            mealTypeEmpl.Description = row["description"].ToString().Trim();
                        }                        
                        mealTypeEmplList.Add(mealTypeEmpl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealTypeEmplList;
        }

        public int getMealsTypeEmplCount(int mealTypeEmplID, string name, string description)
        {
            DataSet dataSet = new DataSet();
            int count = 0;
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT count(mte.meals_type_empl_id) count_mealTypeEmpl FROM meals_type_empl mte");
                if ((mealTypeEmplID != -1) || (!name.Equals("")) || (!description.Equals("")))
                {
                    sb.Append("WHERE ");
                    if (mealTypeEmplID != -1)
                    {
                        sb.Append(" meals_type_empl_id = " + mealTypeEmplID.ToString().Trim()+" AND");
                    }
                    if (!name.Equals(""))
                    {
                        sb.Append(" name = '" + name + "' AND");
                    }
                    if (!description.Equals(""))
                    {
                        sb.Append(" description = '" + description + "' AND");
                    }
                    select = sb.ToString(0, sb.Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }
                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealTypeEmpl");
                DataTable table = dataSet.Tables["MealTypeEmpl"];

                if (table.Rows.Count > 0)
                {
                    count = Int32.Parse(table.Rows[0]["count_mealTypeEmpl"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }

        public bool update(int mealTypeEmplID, string name, string description)
        {
            bool isUpdated = false;
            SqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE meals_type_empl SET ");
                if (!name.Equals(""))
                {
                    sbUpdate.Append("name = N'" + name + "', ");
                }
                else
                {
                    sbUpdate.Append("name = NULL, ");
                }
                if (!description.Equals(""))
                {
                    sbUpdate.Append("description = N'" + description + "', ");
                }
                else
                {
                    sbUpdate.Append("description = NULL, ");
                }
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " GETDATE() ");
                sbUpdate.Append("WHERE meals_type_empl_id = '" + mealTypeEmplID.ToString().Trim() + "'");
                SqlCommand cmd = new SqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (SqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 2627)
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 12);
                    throw procEx;
                }
                else
                {
                    DataProcessingException procEx = new DataProcessingException(sqlex.Message, 11);
                    throw procEx;
                }
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isUpdated;
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
