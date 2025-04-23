using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

using TransferObjects;
using Util;

namespace DataAccess
{
    public class MSSQLEmployeeTypeVisibilityDAO : EmployeeTypeVisibilityDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }
        public MSSQLEmployeeTypeVisibilityDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public MSSQLEmployeeTypeVisibilityDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            DAOController.GetInstance();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        public Dictionary<int, List<int>> getCompanyVisibleTypes(int catID)
        {
            DataSet dataSet = new DataSet();
            EmployeeTypeVisibilityTO typeVisTO = new EmployeeTypeVisibilityTO();
            Dictionary<int, List<int>> companyTypes = new Dictionary<int,List<int>>();
            string select = "";

            try
            {
                if (catID == -1)
                    return companyTypes;

                select = "SELECT * FROM employee_types_visibility WHERE value = '" + Constants.yesInt.ToString().Trim() + "'";
                select += " AND appl_users_category_id = '" + catID + "'";

                SqlCommand cmd = new SqlCommand(select, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeTypes");
                DataTable table = dataSet.Tables["EmployeeTypes"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        typeVisTO = new EmployeeTypeVisibilityTO();
                        typeVisTO.EmployeeTypeID = Int32.Parse(row["employee_type_id"].ToString().Trim());
                        typeVisTO.WorkingUnitID = Int32.Parse(row["working_unit_id"].ToString().Trim());

                        if (!companyTypes.ContainsKey(typeVisTO.WorkingUnitID))
                            companyTypes.Add(typeVisTO.WorkingUnitID, new List<int>());

                        if (!companyTypes[typeVisTO.WorkingUnitID].Contains(typeVisTO.EmployeeTypeID))
                            companyTypes[typeVisTO.WorkingUnitID].Add(typeVisTO.EmployeeTypeID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return companyTypes;
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
