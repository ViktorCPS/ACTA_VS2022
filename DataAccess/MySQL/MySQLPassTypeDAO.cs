using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using TransferObjects;
using Util;

namespace DataAccess
{
    /// <summary>
    /// Summary description for MySQLPassTypeDAO.
    /// </summary>
    public class MySQLPassTypeDAO : PassTypeDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MySQLPassTypeDAO()
        {
            conn = MySQLDAOFactory.getConnection();
        }

        public MySQLPassTypeDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DAOController.GetInstance();
        }

        public int insert(PassTypeTO ptTO)
        {
            int rowsAffected = 0;
            MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                if (ptTO.Button >= 0)
                {
                    DataSet dataSet = new DataSet();
                    String select = "SELECT * FROM pass_types WHERE button = '" + ptTO.Button.ToString() + "'";

                    MySqlCommand command = new MySqlCommand(select, conn, trans);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);

                    sqlDataAdapter.Fill(dataSet, "PassType");
                    DataTable table = dataSet.Tables["PassType"];
                    if (table.Rows.Count > 0)
                    {
                        throw new Exception("Button already exists.");
                    }
                }

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO pass_types ");
                sbInsert.Append("(pass_type_id, description, button, pass_type, ");
                if (!ptTO.PaymentCode.Trim().Equals(""))
                {
                    sbInsert.Append("payment_code, ");
                }
                sbInsert.Append("created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (ptTO.PassTypeID < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.PassTypeID.ToString().Trim() + "', ");
                }
                if (ptTO.Description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + ptTO.Description.Trim() + "', ");
                }
                if (ptTO.Button < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.Button.ToString().Trim() + "', ");
                }
                if (ptTO.IsPass < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.IsPass.ToString().Trim() + "', ");
                }
                if (!ptTO.PaymentCode.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + ptTO.PaymentCode.Trim() + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, trans);
                rowsAffected = cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (MySqlException ex)
            {
                trans.Rollback();
                if (ex.Number.Equals(1062))
                {
                    throw new Exception("Pass Type with a same ID already exist.");
                }
                else
                    throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception("Exception: " + ex.Message);
            }

            return rowsAffected;
        }

        public int insert(PassTypeTO ptTO, bool doCommit)
        {
            int rowsAffected = 0;
            MySqlTransaction trans = null;
            if (doCommit)
                trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                trans = SqlTrans;

            try
            {
                if (ptTO.Button >= 0)
                {
                    DataSet dataSet = new DataSet();
                    String select = "SELECT * FROM pass_types WHERE button = '" + ptTO.Button.ToString() + "'";

                    MySqlCommand command = new MySqlCommand(select, conn, trans);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);

                    sqlDataAdapter.Fill(dataSet, "PassType");
                    DataTable table = dataSet.Tables["PassType"];
                    if (table.Rows.Count > 0)
                    {
                        throw new Exception("Button already exists.");
                    }
                }

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO pass_types ");
                sbInsert.Append("(pass_type_id, description, button, pass_type,description_alternative,segment_color, limit_composite_id,limit_occasion_id, limit_elementary_id, confirmation_flag, massive_input,manual_input_flag,verification_flag,working_unit_id,");
                if (!ptTO.PaymentCode.Trim().Equals(""))
                {
                    sbInsert.Append("payment_code, ");
                }
                sbInsert.Append("created_by, created_time) ");
                sbInsert.Append("VALUES (");
                if (ptTO.PassTypeID < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.PassTypeID.ToString().Trim() + "', ");
                }
                if (ptTO.Description.Trim().Equals(""))
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("N'" + ptTO.Description.Trim() + "', ");
                }
                if (ptTO.Button < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.Button.ToString().Trim() + "', ");
                }
                if (ptTO.IsPass < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.IsPass.ToString().Trim() + "', ");
                }
                if (!ptTO.DescAlt.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + ptTO.DescAlt.Trim() + "', ");
                }
                if (!ptTO.SegmentColor.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + ptTO.SegmentColor.Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (ptTO.LimitCompositeID < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.LimitCompositeID.ToString().Trim() + "', ");
                }
                if (ptTO.LimitElementaryID < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.LimitElementaryID.ToString().Trim() + "', ");
                }
                if (ptTO.LimitOccasionID < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.LimitOccasionID.ToString().Trim() + "', ");
                }
                if (ptTO.ConfirmFlag < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.ConfirmFlag.ToString().Trim() + "', ");
                }
                if (ptTO.MassiveInput < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.MassiveInput.ToString().Trim() + "', ");
                }
                if (ptTO.ManualInputFlag < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.ManualInputFlag.ToString().Trim() + "', ");
                }
                if (ptTO.VerificationFlag < 0)
                {
                    sbInsert.Append("null, ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.VerificationFlag.ToString().Trim() + "', ");
                }
                if (ptTO.WUID == -1)
                {
                    sbInsert.Append("'0', ");
                }
                else
                {
                    sbInsert.Append("'" + ptTO.WUID.ToString().Trim() + "', ");
                }
                if (!ptTO.PaymentCode.Trim().Equals(""))
                {
                    sbInsert.Append("N'" + ptTO.PaymentCode.Trim() + "', ");
                }
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, trans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                    trans.Commit();
            }
            catch (MySqlException ex)
            {
                if (doCommit)
                    trans.Rollback();
                if (ex.Number.Equals(1062))
                {
                    throw new Exception("Pass Type with a same ID already exist.");
                }
                else
                    throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception("Exception: " + ex.Message);
            }

            return rowsAffected;
        }

        public bool delete(int passTypeID)
        {
            bool isDeleted = false;
            MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM pass_types WHERE pass_type_id = '" + passTypeID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, trans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();

                throw new Exception("Exception: " + ex.Message);
            }

            return isDeleted;
        }

        public bool delete(int passTypeID, bool doCommit)
        {
            bool isDeleted = false;
            MySqlTransaction trans = null;

            if (doCommit)
                trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                trans = SqlTrans;

            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("DELETE FROM pass_types WHERE pass_type_id = '" + passTypeID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbDelete.ToString(), conn, trans);
                int res = cmd.ExecuteNonQuery();
                isDeleted = true;

                if (doCommit)
                    trans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    trans.Rollback();

                throw new Exception("Exception: " + ex.Message);
            }

            return isDeleted;
        }

        public PassTypeTO find(int passTypeID)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM pass_types WHERE pass_type_id = '" + passTypeID.ToString().Trim() + "'", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count == 1)
                {
                    passType = new PassTypeTO();
                    passType.PassTypeID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
                    if (table.Rows[0]["description"] != DBNull.Value)
                    {
                        passType.Description = table.Rows[0]["description"].ToString().Trim();
                    }
                    if (table.Rows[0]["button"] != DBNull.Value)
                    {
                        passType.Button = Int32.Parse(table.Rows[0]["button"].ToString().Trim());
                    }
                    if (table.Rows[0]["pass_type"] != DBNull.Value)
                    {
                        passType.IsPass = Int32.Parse(table.Rows[0]["pass_type"].ToString().Trim());
                    }
                    if (table.Rows[0]["payment_code"] != DBNull.Value)
                    {
                        passType.PaymentCode = table.Rows[0]["payment_code"].ToString().Trim();
                    }
                    if (table.Rows[0]["description_alternative"] != DBNull.Value)
                    {
                        passType.DescAlt = table.Rows[0]["description_alternative"].ToString().Trim();
                    }
                    if (table.Rows[0]["segment_color"] != DBNull.Value)
                    {
                        passType.SegmentColor = table.Rows[0]["segment_color"].ToString().Trim();
                    }
                    if (table.Rows[0]["limit_composite_id"] != DBNull.Value)
                    {
                        passType.LimitCompositeID = Int32.Parse(table.Rows[0]["limit_composite_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["limit_elementary_id"] != DBNull.Value)
                    {
                        passType.LimitElementaryID = Int32.Parse(table.Rows[0]["limit_elementary_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["limit_occasion_id"] != DBNull.Value)
                    {
                        passType.LimitOccasionID = Int32.Parse(table.Rows[0]["limit_occasion_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["working_unit_id"] != DBNull.Value)
                    {
                        passType.WUID = Int32.Parse(table.Rows[0]["working_unit_id"].ToString().Trim());
                    }
                    if (table.Rows[0]["confirmation_flag"] != DBNull.Value)
                    {
                        passType.ConfirmFlag = Int32.Parse(table.Rows[0]["confirmation_flag"].ToString().Trim());
                    }
                    if (table.Rows[0]["massive_input"] != DBNull.Value)
                    {
                        passType.MassiveInput = Int32.Parse(table.Rows[0]["massive_input"].ToString().Trim());
                    }
                    if (table.Rows[0]["manual_input_flag"] != DBNull.Value)
                    {
                        passType.ManualInputFlag = Int32.Parse(table.Rows[0]["manual_input_flag"].ToString().Trim());
                    }
                    if (table.Rows[0]["verification_flag"] != DBNull.Value)
                    {
                        passType.VerificationFlag = Int32.Parse(table.Rows[0]["verification_flag"].ToString().Trim());
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passType;
        }

        public Dictionary<int, PassTypeTO> find(string passTypeID, int company)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            Dictionary<int, PassTypeTO> listPassType = new Dictionary<int, PassTypeTO>();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM pass_types WHERE pass_type_id in (" + passTypeID.Trim() + ") and working_unit_id=" + company, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }
                        if (!listPassType.ContainsKey(passType.PassTypeID))
                            listPassType.Add(passType.PassTypeID, passType);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return listPassType;
        }

        public int findMAXPassTypeID()
        {
            DataSet dataSet = new DataSet();
            int ptID = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT MAX(pass_type_id) AS pass_type_id FROM pass_types", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassTypes");
                DataTable table = dataSet.Tables["PassTypes"];

                if (table.Rows.Count == 1 && !table.Rows[0]["pass_type_id"].Equals(DBNull.Value))
                {
                    ptID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ptID;
        }

        public bool update(PassTypeTO ptTO, int oldButton)
        {
            bool isUpdated = false;
            MySqlTransaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                if (ptTO.Button >= 0 && (oldButton != ptTO.Button))
                {
                    DataSet dataSet = new DataSet();
                    String select = "SELECT * FROM pass_types WHERE button = '" + ptTO.Button.ToString() + "'";

                    MySqlCommand command = new MySqlCommand(select, conn, trans);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);

                    sqlDataAdapter.Fill(dataSet, "PassType");
                    DataTable table = dataSet.Tables["PassType"];
                    if (table.Rows.Count > 0)
                    {
                        throw new Exception("Button already exists.");
                    }
                }

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE pass_types SET ");

                if (!ptTO.Description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + ptTO.Description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (ptTO.Button >= 0)
                {
                    sbUpdate.Append("button = '" + ptTO.Button.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("button = null, ");
                }
                if (ptTO.IsPass >= 0)
                {
                    sbUpdate.Append("pass_type = '" + ptTO.IsPass.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("pass_type = null, ");
                }
                if (ptTO.PaymentCode != "")
                {
                    sbUpdate.Append("payment_code = N'" + ptTO.PaymentCode.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("payment_code = null, ");
                }

                sbUpdate.Append("modified_by = '" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE pass_type_id = " + ptTO.PassTypeID.ToString().Trim());

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, trans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw new Exception("Exception: " + ex.Message);
            }

            return isUpdated;
        }
        public bool update(PassTypeTO ptTO, int oldButton, bool doCommit)
        {
            bool isUpdated = false;
            MySqlTransaction trans = null;
            if (doCommit)
                trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                trans = SqlTrans;

            try
            {
                if (ptTO.Button >= 0 && (oldButton != ptTO.Button))
                {
                    DataSet dataSet = new DataSet();
                    String select = "SELECT * FROM pass_types WHERE button = '" + ptTO.Button.ToString() + "'";

                    MySqlCommand command = new MySqlCommand(select, conn, trans);
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);

                    sqlDataAdapter.Fill(dataSet, "PassType");
                    DataTable table = dataSet.Tables["PassType"];
                    if (table.Rows.Count > 0)
                    {
                        throw new Exception("Button already exists.");
                    }
                }

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE pass_types SET ");

                if (!ptTO.Description.Trim().Equals(""))
                {
                    sbUpdate.Append("description = N'" + ptTO.Description.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = null, ");
                }
                if (ptTO.Button >= 0)
                {
                    sbUpdate.Append("button = '" + ptTO.Button.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("button = null, ");
                }
                if (ptTO.IsPass >= 0)
                {
                    sbUpdate.Append("pass_type = '" + ptTO.IsPass.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("pass_type = null, ");
                }
                if (!ptTO.PaymentCode.Trim().Equals(""))
                {
                    sbUpdate.Append("payment_code = N'" + ptTO.PaymentCode.Trim() + "', ");
                }
                if (!ptTO.DescAlt.Trim().Equals(""))
                {
                    sbUpdate.Append("description_alternative = N'" + ptTO.DescAlt.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description_alternative = null, ");
                }
                if (!ptTO.SegmentColor.Trim().Equals(""))
                {
                    sbUpdate.Append("segment_color = N'" + ptTO.SegmentColor.Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("segment_color = null, ");
                }
                if (ptTO.WUID != -1)
                {
                    sbUpdate.Append("working_unit_id = '" + ptTO.WUID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("working_unit_id = 0, ");
                }
                if (ptTO.LimitCompositeID >= 0)
                {
                    sbUpdate.Append("limit_composite_id = '" + ptTO.LimitCompositeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("limit_composite_id = null, ");
                }
                if (ptTO.LimitElementaryID >= 0)
                {
                    sbUpdate.Append("limit_elementary_id = '" + ptTO.LimitElementaryID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("limit_elementary_id = null, ");
                }
                if (ptTO.LimitOccasionID >= 0)
                {
                    sbUpdate.Append("limit_occasion_id = '" + ptTO.LimitOccasionID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("limit_occasion_id = null, ");
                }
                if (ptTO.ConfirmFlag >= 0)
                {
                    sbUpdate.Append("confirmation_flag = '" + ptTO.ConfirmFlag.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("confirmation_flag = null, ");
                }
                if (ptTO.MassiveInput >= 0)
                {
                    sbUpdate.Append("massive_input = '" + ptTO.MassiveInput.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("massive_input = null, ");
                }
                if (ptTO.ManualInputFlag >= 0)
                {
                    sbUpdate.Append("manual_input_flag = '" + ptTO.ManualInputFlag.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("manual_input_flag = null, ");
                }
                if (ptTO.VerificationFlag >= 0)
                {
                    sbUpdate.Append("verification_flag = '" + ptTO.VerificationFlag.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("verification_flag = null, ");
                }

                sbUpdate.Append("modified_by = '" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE pass_type_id = " + ptTO.PassTypeID.ToString().Trim());

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, trans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }
                if (doCommit)
                    trans.Commit();
            }
            catch (Exception ex)
            {
                if (doCommit)
                    trans.Rollback();
                throw new Exception("Exception: " + ex.Message);
            }

            return isUpdated;
        }

        public Dictionary<int, PassTypeTO> getPassTypesDictionary(PassTypeTO ptTO)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            Dictionary<int, PassTypeTO> passTypesList = new Dictionary<int, PassTypeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_types ");

                if ((ptTO.PassTypeID != -1) || (!ptTO.Description.Trim().Equals("")) ||
                    (ptTO.Button != -1) || (ptTO.IsPass != -1) || (ptTO.WUID != -1) || (!ptTO.PaymentCode.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (ptTO.PassTypeID != -1)
                    {
                        sb.Append(" pass_type_id = '" + ptTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + ptTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (ptTO.Button != -1)
                    {
                        sb.Append(" button = '" + ptTO.Button.ToString().Trim() + "' AND");
                    }
                    if (ptTO.IsPass != -1)
                    {
                        sb.Append(" pass_type = '" + ptTO.IsPass.ToString().Trim() + "' AND");
                    }
                    if (ptTO.WUID != -1)
                    {
                        sb.Append(" working_unit_id = '" + ptTO.WUID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.PaymentCode.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(payment_code) LIKE N'%" + ptTO.PaymentCode.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY description", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }
                        if (!passTypesList.ContainsKey(passType.PassTypeID))
                            passTypesList.Add(passType.PassTypeID, passType);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public Dictionary<int, PassTypeTO> getPassTypesDictionaryCodeSorted(PassTypeTO ptTO)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            Dictionary<int, PassTypeTO> passTypesList = new Dictionary<int, PassTypeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_types ");

                if ((ptTO.PassTypeID != -1) || (!ptTO.Description.Trim().Equals("")) ||
                    (ptTO.Button != -1) || (ptTO.IsPass != -1) || (ptTO.WUID != -1) || (!ptTO.PaymentCode.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (ptTO.PassTypeID != -1)
                    {
                        sb.Append(" pass_type_id = '" + ptTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + ptTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (ptTO.Button != -1)
                    {
                        sb.Append(" button = '" + ptTO.Button.ToString().Trim() + "' AND");
                    }
                    if (ptTO.IsPass != -1)
                    {
                        sb.Append(" pass_type = '" + ptTO.IsPass.ToString().Trim() + "' AND");
                    }
                    if (ptTO.WUID != -1)
                    {
                        sb.Append(" working_unit_id = '" + ptTO.WUID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.PaymentCode.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(payment_code) LIKE N'%" + ptTO.PaymentCode.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY payment_code", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }
                        if (!passTypesList.ContainsKey(passType.PassTypeID))
                            passTypesList.Add(passType.PassTypeID, passType);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public Dictionary<int, PassTypeTO> getPassTypesDictionary(PassTypeTO ptTO, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            Dictionary<int, PassTypeTO> passTypesList = new Dictionary<int, PassTypeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_types ");

                if ((ptTO.PassTypeID != -1) || (!ptTO.Description.Trim().Equals("")) ||
                    (ptTO.Button != -1) || (ptTO.IsPass != -1) || (ptTO.WUID != -1) || (!ptTO.PaymentCode.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (ptTO.PassTypeID != -1)
                    {
                        sb.Append(" pass_type_id = '" + ptTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + ptTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (ptTO.Button != -1)
                    {
                        sb.Append(" button = '" + ptTO.Button.ToString().Trim() + "' AND");
                    }
                    if (ptTO.IsPass != -1)
                    {
                        sb.Append(" pass_type = '" + ptTO.IsPass.ToString().Trim() + "' AND");
                    }
                    if (ptTO.WUID != -1)
                    {
                        sb.Append(" working_unit_id = '" + ptTO.WUID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.PaymentCode.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(payment_code) LIKE N'%" + ptTO.PaymentCode.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY description", conn, (MySqlTransaction)trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }
                        if (!passTypesList.ContainsKey(passType.PassTypeID))
                            passTypesList.Add(passType.PassTypeID, passType);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public Dictionary<int, PassTypeTO> getPassTypesForCompanyDictionary(int company, bool isAlternativeLang)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            Dictionary<int, PassTypeTO> passTypesList = new Dictionary<int, PassTypeTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM pass_types WHERE working_unit_id = '" + company.ToString().Trim() + "' OR pass_type_id IN ('"
                    + Constants.absence.ToString().Trim() + "', '" + Constants.overtimeUnjustified.ToString().Trim() + "') ORDER BY payment_code, ";

                if (!isAlternativeLang)
                    select += "description";
                else
                    select += "description_alternative";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }
                        if (!passTypesList.ContainsKey(passType.PassTypeID))
                            passTypesList.Add(passType.PassTypeID, passType);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public List<PassTypeTO> getConformationTypes(int ptID, string ptIDs, bool isAlternativeLang)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            List<PassTypeTO> passTypesList = new List<PassTypeTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM pass_types WHERE pass_type_id IN (SELECT confirmation_pass_type_id FROM pass_types_confirmation WHERE pass_type_id = '"
                    + ptID.ToString().Trim() + "'";

                if (!ptIDs.Trim().Equals(""))
                    select += " AND confirmation_pass_type_id IN (" + ptIDs.Trim() + ")";

                select += ") ORDER BY payment_code, ";

                if (!isAlternativeLang)
                    select += "description";
                else
                    select += "description_alternative";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }

                        passTypesList.Add(passType);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public List<PassTypeTO> getPassTypes(PassTypeTO ptTO)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            List<PassTypeTO> passTypesList = new List<PassTypeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_types ");

                if ((ptTO.PassTypeID != -1) || (!ptTO.Description.Trim().Equals("")) || (!ptTO.DescAlt.Trim().Equals("")) ||
                   (ptTO.Button != -1) || (ptTO.IsPass != -1) || (ptTO.WUID != -1) || (!ptTO.PaymentCode.Trim().Equals(""))
                   || (ptTO.MassiveInput != -1) || (ptTO.ManualInputFlag != -1) || (ptTO.ConfirmFlag != -1)
                   || (ptTO.LimitOccasionID != -1) || (ptTO.LimitElementaryID != -1) || (ptTO.LimitCompositeID != -1) || (ptTO.VerificationFlag != -1))
                {
                    sb.Append(" WHERE ");

                    if (ptTO.PassTypeID != -1)
                    {
                        sb.Append(" pass_type_id = '" + ptTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + ptTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (!ptTO.DescAlt.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description_alternative) LIKE N'%" + ptTO.DescAlt.ToUpper().Trim() + "%' AND");
                    }
                    if (ptTO.Button != -1)
                    {
                        sb.Append(" button = '" + ptTO.Button.ToString().Trim() + "' AND");
                    }
                    if (ptTO.MassiveInput != -1)
                    {
                        sb.Append(" massive_input = '" + ptTO.MassiveInput.ToString().Trim() + "' AND");
                    }
                    if (ptTO.ManualInputFlag != -1)
                    {
                        sb.Append(" manual_input_flag = '" + ptTO.ManualInputFlag.ToString().Trim() + "' AND");
                    }
                    if (ptTO.ConfirmFlag != -1)
                    {
                        sb.Append(" confirmation_flag = '" + ptTO.ConfirmFlag.ToString().Trim() + "' AND");
                    }

                    if (ptTO.LimitCompositeID != -1)
                    {
                        sb.Append(" limit_composite_id = '" + ptTO.LimitCompositeID.ToString().Trim() + "' AND");
                    }
                    if (ptTO.LimitElementaryID != -1)
                    {
                        sb.Append(" limit_elementary_id = '" + ptTO.LimitElementaryID.ToString().Trim() + "' AND");
                    }
                    if (ptTO.LimitOccasionID != -1)
                    {
                        sb.Append(" limit_occasion_id = '" + ptTO.LimitOccasionID.ToString().Trim() + "' AND");
                    }
                    if (ptTO.VerificationFlag != -1)
                    {
                        sb.Append(" verification_flag = '" + ptTO.VerificationFlag.ToString().Trim() + "' AND");
                    }
                    if (ptTO.IsPass != -1)
                    {
                        sb.Append(" pass_type = '" + ptTO.IsPass.ToString().Trim() + "' AND");
                    }
                    if (ptTO.WUID != -1)
                    {
                        sb.Append(" working_unit_id = '" + ptTO.WUID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.PaymentCode.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(payment_code) LIKE N'%" + ptTO.PaymentCode.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY description", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }


                        passTypesList.Add(passType);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public List<PassTypeTO> getPassTypesForCompany(int company, bool isAlternativeLang)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            List<PassTypeTO> passTypesList = new List<PassTypeTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM pass_types WHERE working_unit_id = '" + company.ToString().Trim() + "' OR pass_type_id IN ('"
                    + Constants.absence.ToString().Trim() + "', '" + Constants.overtimeUnjustified.ToString().Trim() + "') ORDER BY payment_code, ";
                if (!isAlternativeLang)
                    select += "description";
                else
                    select += "description_alternative";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }

                        passTypesList.Add(passType);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public List<PassTypeTO> getPassTypesMassiveInputForCompany(int company, string ptIDs, bool isAlternativeLang)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            List<PassTypeTO> passTypesList = new List<PassTypeTO>();
            string select = "";

            try
            {
                select = "SELECT * FROM pass_types WHERE (working_unit_id = '" + company.ToString().Trim() + "' OR pass_type_id IN ('"
                    + Constants.absence.ToString().Trim() + "', '" + Constants.overtimeUnjustified.ToString().Trim() + "')) AND massive_input = " + ((int)Constants.MassiveInput.Yes).ToString().Trim();

                if (ptIDs.Length > 0)
                {
                    select += " AND pass_type_id IN (" + ptIDs.Trim() + ")";
                }

                select += " ORDER BY payment_code, ";

                if (!isAlternativeLang)
                    select += "description";
                else
                    select += "description_alternative";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }

                        passTypesList.Add(passType);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }
        public Dictionary<int, PassTypeTO> findByPaymentCode(string paymentCode, int company)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            Dictionary<int, PassTypeTO> passTypesList = new Dictionary<int, PassTypeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_types ");

                if (!paymentCode.Equals("") || company != -1)
                {
                    sb.Append(" WHERE ");

                    if (!paymentCode.Equals(""))
                    {

                        string[] payment = paymentCode.Split(',');

                        string payment_code = "";
                        foreach (string p in payment)
                        {
                            payment_code += "'" + p + "'" + ",";
                        }
                        if (payment_code.Length > 0)
                        {
                            payment_code = payment_code.Remove(payment_code.LastIndexOf(","));
                        }

                        sb.Append(" payment_code in (" + payment_code + ") AND");
                    }
                    if (company != -1)
                    {
                        sb.Append(" working_unit_id=" + company + " AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY pass_type_id", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }


                        passTypesList.Add(passType.PassTypeID, passType);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }



        public List<PassTypeTO> getPassTypes(PassTypeTO ptTO, List<int> isPass)
        {
            DataSet dataSet = new DataSet();
            PassTypeTO passType = new PassTypeTO();
            List<PassTypeTO> passTypesList = new List<PassTypeTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM pass_types ");

                if ((ptTO.PassTypeID != -1) || (!ptTO.Description.Trim().Equals("")) ||
                    (ptTO.Button != -1) || (isPass.Count > 0) || (!ptTO.PaymentCode.Trim().Equals("")))
                {
                    sb.Append(" WHERE ");

                    if (ptTO.PassTypeID != -1)
                    {
                        sb.Append(" pass_type_id = '" + ptTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if (!ptTO.Description.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(description) LIKE N'%" + ptTO.Description.ToUpper().Trim() + "%' AND");
                    }
                    if (ptTO.Button != -1)
                    {
                        sb.Append(" button = '" + ptTO.Button.ToString().Trim() + "' AND");
                    }

                    if (isPass.Count > 0)
                    {
                        sb.Append(" ( ");
                        foreach (int isPassMember in isPass)
                        {
                            if (isPassMember != -1)
                            {
                                sb.Append(" pass_type = '" + isPassMember.ToString().Trim() + "' OR");
                            }
                        }

                        string temp = sb.ToString(0, sb.ToString().Length - 2);
                        sb.Remove(0, sb.Length);
                        sb.Append(temp);

                        sb.Append(") AND");
                    }
                    if (!ptTO.PaymentCode.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(payment_code) LIKE N'%" + ptTO.PaymentCode.ToUpper().Trim() + "%' AND");
                    }

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }
                else
                {
                    select = sb.ToString();
                }

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY description", conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        passType = new PassTypeTO();
                        passType.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        if (row["description"] != DBNull.Value)
                        {
                            passType.Description = row["description"].ToString().Trim();
                        }
                        if (row["button"] != DBNull.Value)
                        {
                            passType.Button = Int32.Parse(row["button"].ToString().Trim());
                        }
                        if (row["pass_type"] != DBNull.Value)
                        {
                            passType.IsPass = Int32.Parse(row["pass_type"].ToString().Trim());
                        }
                        if (row["payment_code"] != DBNull.Value)
                        {
                            passType.PaymentCode = row["payment_code"].ToString().Trim();
                        }
                        if (row["description_alternative"] != DBNull.Value)
                        {
                            passType.DescAlt = row["description_alternative"].ToString().Trim();
                        }
                        if (row["segment_color"] != DBNull.Value)
                        {
                            passType.SegmentColor = row["segment_color"].ToString().Trim();
                        }
                        if (row["limit_composite_id"] != DBNull.Value)
                        {
                            passType.LimitCompositeID = Int32.Parse(row["limit_composite_id"].ToString().Trim());
                        }
                        if (row["limit_elementary_id"] != DBNull.Value)
                        {
                            passType.LimitElementaryID = Int32.Parse(row["limit_elementary_id"].ToString().Trim());
                        }
                        if (row["limit_occasion_id"] != DBNull.Value)
                        {
                            passType.LimitOccasionID = Int32.Parse(row["limit_occasion_id"].ToString().Trim());
                        }
                        if (row["working_unit_id"] != DBNull.Value)
                        {
                            passType.WUID = Int32.Parse(row["working_unit_id"].ToString().Trim());
                        }
                        if (row["confirmation_flag"] != DBNull.Value)
                        {
                            passType.ConfirmFlag = Int32.Parse(row["confirmation_flag"].ToString().Trim());
                        }
                        if (row["massive_input"] != DBNull.Value)
                        {
                            passType.MassiveInput = Int32.Parse(row["massive_input"].ToString().Trim());
                        }
                        if (row["manual_input_flag"] != DBNull.Value)
                        {
                            passType.ManualInputFlag = Int32.Parse(row["manual_input_flag"].ToString().Trim());
                        }
                        if (row["verification_flag"] != DBNull.Value)
                        {
                            passType.VerificationFlag = Int32.Parse(row["verification_flag"].ToString().Trim());
                        }


                        passTypesList.Add(passType);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public List<string> getPassTypesDistinctField(string field)
        {
            DataSet dataSet = new DataSet();
            List<string> passTypesList = new List<string>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT " + field + " FROM pass_types ");

                select = sb.ToString();

                MySqlCommand cmd = new MySqlCommand(select + " ORDER BY " + field, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "PassType");
                DataTable table = dataSet.Tables["PassType"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (passTypesList.Contains(row[field].ToString().Trim()))
                        {
                            passTypesList.Add(row[field].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Exception: " + ex.Message);
            }

            return passTypesList;
        }

        public void serialize(List<PassTypeTO> passTypeTOList)
        {
            try
            {
                //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLPassTypesFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLPassTypesFile;
                Stream stream = File.Open(filename, FileMode.Create);

                PassTypeTO[] passTypeTOArray = (PassTypeTO[])passTypeTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(PassTypeTO[]));
                bformatter.Serialize(stream, passTypeTOArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
    }
}
