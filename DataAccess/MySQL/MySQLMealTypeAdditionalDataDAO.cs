using System;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Globalization;
using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLMealTypeAdditionalDataDAO: MealTypeAdditionalDataDAO
    {
        MySqlConnection conn = null;
		//protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;
      protected string dateTimeformat = "";
		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MySQLMealTypeAdditionalDataDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLMealTypeAdditionalDataDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public int insert(int mealTypeID, string descriptionAdd, byte[] picture)
        {
            int rowsAffected = 0;
            MySqlTransaction sqltrans = null;
            sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_type_additional_data ");
                sbInsert.Append("(meal_type_id, description_additional, picture, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (mealTypeID != -1)
                {
                    sbInsert.Append("'" + mealTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (descriptionAdd.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + descriptionAdd.Trim() + "', ");
                }

                if (picture == null)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("@Picture, ");
                }
                

                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqltrans);
                if (picture != null)
                {
                    cmd.Parameters.Add("?Picture", MySqlDbType.MediumBlob, picture.Length).Value = picture;
                }

                rowsAffected = cmd.ExecuteNonQuery();
                sqltrans.Commit();

                rowsAffected = 1;
            }
            catch (Exception ex)
            {
                sqltrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(int mealTypeID)
        {
            bool isDeleted = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_type_additional_data WHERE meal_type_id = '" + mealTypeID + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, sqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool update(int mealTypeID, string descriptionAdd, byte[] picture)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE meals_type_additional_data SET ");

                if (!descriptionAdd.Equals(""))
                {
                    sbUpdate.Append("description_additional = N'" + descriptionAdd + "', ");
                }
                else
                {
                    sbUpdate.Append("description_additional = NULL, ");
                }
                if (picture != null)
                {
                    sbUpdate.Append("picture = ?Picture, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
                sbUpdate.Append("WHERE meal_type_id = '" + mealTypeID.ToString().Trim() + "'");
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                if (picture != null)
                {
                    cmd.Parameters.Add("?Picture", MySqlDbType.MediumBlob, picture.Length).Value = picture;
                }
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 1062)
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

        public int insert(int mealTypeID, string descriptionAdd, byte[] picture, bool doCommit)
        {
            int rowsAffected = 0;
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO meals_type_additional_data ");
                sbInsert.Append("(meal_type_id, description_additional, picture, created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (mealTypeID != -1)
                {
                    sbInsert.Append("'" + mealTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                if (descriptionAdd.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + descriptionAdd.Trim() + "', ");
                }

                if (picture == null)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("@Picture, ");
                }


                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, SqlTrans);
                if (picture != null)
                {
                    cmd.Parameters.Add("?Picture", MySqlDbType.MediumBlob, picture.Length).Value = picture;
                }

                rowsAffected = cmd.ExecuteNonQuery();
                rowsAffected = 1;
                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }

        public bool delete(int mealTypeID, bool doCommit)
        {
            bool isDeleted = false;
            if (doCommit)
                SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM meals_type_additional_data WHERE meal_type_id = '" + mealTypeID + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res != 0)
                {
                    isDeleted = true;
                }
                if (doCommit)
                    SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    SqlTrans.Rollback();
                throw ex;
            }

            return isDeleted;
        }

        public bool update(int mealTypeID, string descriptionAdd, byte[] picture, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE meals_type_additional_data SET ");

                if (!descriptionAdd.Equals(""))
                {
                    sbUpdate.Append("description_additional = N'" + descriptionAdd + "', ");
                }
                else
                {
                    sbUpdate.Append("description_additional = NULL, ");
                }
                if (picture != null)
                {
                    sbUpdate.Append("picture = ?Picture, ");
                }

                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().ToString().Trim() + "', modified_time = " + " NOW() ");
                sbUpdate.Append("WHERE meal_type_id = '" + mealTypeID.ToString().Trim() + "'");
                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, sqlTrans);
                if (picture != null)
                {
                    cmd.Parameters.Add("?Picture", MySqlDbType.MediumBlob, picture.Length).Value = picture;
                }
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                sqlTrans.Commit();

            }
            catch (MySqlException sqlex)
            {
                sqlTrans.Rollback();

                if (sqlex.Number == 1062)
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
                this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
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


        public MealTypeAdditionalDataTO getAdditionalData(int mealTypeID)
        {
            DataSet dataSet = new DataSet();
            MealTypeAdditionalDataTO mealTypeAddData = new MealTypeAdditionalDataTO();
            string select;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM meals_type_additional_data WHERE meal_type_id = " + mealTypeID.ToString().Trim());
                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        mealTypeAddData = new MealTypeAdditionalDataTO();
                        mealTypeAddData.MealTypeID = Int32.Parse(row["meal_type_id"].ToString().Trim());
                        if (row["description_additional"] != DBNull.Value)
                        {
                            mealTypeAddData.DescriptionAdditional = row["description_additional"].ToString().Trim();
                        }
                        if (row["picture"] != DBNull.Value)
                        {
                            mealTypeAddData.Picture = (byte[])row["picture"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mealTypeAddData;
        }


        #region MealTypeAdditionalDataDAO Members


        public int tableExists()
        {
            int exists = 0;
            DataSet dataSet = new DataSet();
            MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("IF OBJECT_ID (N'actamgr.meals_type_additional_data', N'U') IS NOT NULL SELECT 1 AS res ELSE SELECT 0 AS res;");
                MySqlCommand cmd = new MySqlCommand(sbSelect.ToString(), conn, sqlTrans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "MealType");
                DataTable table = dataSet.Tables["MealType"];

                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];

                    if (row["res"] != DBNull.Value)
                    {
                        exists = int.Parse(row["res"].ToString().Trim());
                    }
                }
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                throw new Exception(ex.Message);
            }

            return exists;
        }

        #endregion
    }
}
