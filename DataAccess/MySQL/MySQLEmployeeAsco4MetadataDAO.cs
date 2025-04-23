using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MySQLEmployeeAsco4MetadataDAO : EmployeeAsco4MetadataDAO
    {
        MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

        public MySQLEmployeeAsco4MetadataDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DAOController.GetInstance();
		}

        public MySQLEmployeeAsco4MetadataDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DAOController.GetInstance();
        }

        public Dictionary<string, string> getEmployeeAsco4MetadataValues(string lang)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            try
            {
                DataSet dataSet = new DataSet();

                string select = "SELECT * FROM employees_asco4_metadata";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmplAsco4Metadata");
                DataTable table = dataSet.Tables["EmplAsco4Metadata"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["col"] != DBNull.Value)
                        {
                            string col = row["col"].ToString().Trim();
                            string name = "";
                            if (lang.Trim().ToUpper() == Constants.Lang_sr.Trim().ToUpper())
                            {
                                if (row["name"] != DBNull.Value)
                                    name = row["name"].ToString().Trim();
                            }
                            else
                            {
                                if (row["name_alternative"] != DBNull.Value)
                                    name = row["name_alternative"].ToString().Trim();
                            }

                            if (!metadata.ContainsKey(col))
                                metadata.Add(col, name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return metadata;
        }

        public Dictionary<string, string> getEmployeeAsco4MetadataWebValues(string lang)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            try
            {
                DataSet dataSet = new DataSet();

                string select = "SELECT * FROM employees_asco4_metadata WHERE web_visibility > 0 ORDER BY web_visibility_ord";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmplAsco4Metadata");
                DataTable table = dataSet.Tables["EmplAsco4Metadata"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["col"] != DBNull.Value)
                        {
                            string col = row["col"].ToString().Trim();
                            string name = "";
                            if (lang.Trim().ToUpper() == Constants.Lang_sr.Trim().ToUpper())
                            {
                                if (row["name"] != DBNull.Value)
                                    name = row["name"].ToString().Trim();
                            }
                            else
                            {
                                if (row["name_alternative"] != DBNull.Value)
                                    name = row["name_alternative"].ToString().Trim();
                            }

                            if (!metadata.ContainsKey(col))
                                metadata.Add(col, name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return metadata;
        }
    }
}
