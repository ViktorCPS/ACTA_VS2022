using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text;
using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for MySQLEmployeeAbsenceDAO.
	/// </summary>
	public class MySQLEmployeeAbsenceDAO : EmployeeAbsenceDAO
	{		
		MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MySQLEmployeeAbsenceDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLEmployeeAbsenceDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public int insert(int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used, DateTime vacationYear, string description)
		{
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			int rowsAffected = 0;

			try
			{
				StringBuilder sbInsert = new StringBuilder();
				sbInsert.Append("INSERT INTO employee_absences ");
                sbInsert.Append("(employee_id, pass_type_id, date_start, date_end, used, vac_year,description, created_by, created_time) ");
				sbInsert.Append("VALUES (");

				if (employeeID != -1)
				{
					sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (passTypeID != -1)
				{
					sbInsert.Append("'" + passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!dateStart.Equals(new DateTime()))
				{
					sbInsert.Append("'" + dateStart.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (!dateEnd.Equals(new DateTime()))
				{
					sbInsert.Append("'" + dateEnd.ToString(dateTimeformat).Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
				if (used != -1)
				{
					sbInsert.Append("'" + used.ToString().Trim() + "', ");
				}
				else
				{
					sbInsert.Append("NULL, ");
				}
                if (!vacationYear.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + vacationYear.ToString(dateTimeformat) + "', ");
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
				
				sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");
					
				MySqlCommand cmd = new MySqlCommand( sbInsert.ToString(), conn, sqlTrans );
				rowsAffected = cmd.ExecuteNonQuery();
				sqlTrans.Commit();
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw ex;
			}

			return rowsAffected;
		}

        public int insert(int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used, DateTime vacationYear, bool doCommit)
        {
            MySqlTransaction sqlTrans = null;
            if (doCommit)
            {
                sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }
            else
            {
                sqlTrans = this.SqlTrans;
            }
            int rowsAffected = 0;

            try
            {
                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO employee_absences ");
                sbInsert.Append("(employee_id, pass_type_id, date_start, date_end, used, vac_year, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                if (employeeID != -1)
                {
                    sbInsert.Append("'" + employeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (passTypeID != -1)
                {
                    sbInsert.Append("'" + passTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!dateStart.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + dateStart.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!dateEnd.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + dateEnd.ToString(dateTimeformat).Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (used != -1)
                {
                    sbInsert.Append("'" + used.ToString().Trim() + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }
                if (!vacationYear.Equals(new DateTime()))
                {
                    sbInsert.Append("'" + vacationYear.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbInsert.Append("NULL, ");
                }

                sbInsert.Append(" N'" + DAOController.GetLogInUser().Trim() + "', NOW()) ");

                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqlTrans);
                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqlTrans.Commit();
                }
            }
            catch (MySqlException sqlEx)
            {
                if (doCommit)

                    sqlTrans.Rollback();
                else
                    sqlTrans.Rollback();

                if (sqlEx.Number == 1062)
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
                if (doCommit)
                    sqlTrans.Rollback();
                else
                    sqlTrans.Rollback();
                throw ex;
            }

            return rowsAffected;
        }


		public bool update(int recID, int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used)
		{
			bool isUpdated = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE employee_absences SET ");

				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = '" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = NULL, ");
				}
				if (passTypeID != -1)
				{
					sbUpdate.Append("pass_type_id = '" +  passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("pass_type_id = NULL, ");
				}
				if (!dateStart.Equals(new DateTime()))
				{
					sbUpdate.Append("date_start = '" +  dateStart.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_start = NULL, ");
				}
				if (!dateEnd.Equals(new DateTime()))
				{
					sbUpdate.Append("date_end = '" +  dateEnd.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_end = NULL, ");
				}
				if (used != -1)
				{
					sbUpdate.Append("used = '" + used.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("used = NULL, ");
				}
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE rec_id = '" + recID.ToString().Trim() + "'");

				MySqlCommand cmd = new MySqlCommand( sbUpdate.ToString(), conn, sqlTrans );
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;
				}

				sqlTrans.Commit();
				
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool update(int recID, int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used, bool doCommit)
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
				sbUpdate.Append("UPDATE employee_absences SET ");

				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = '" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = NULL, ");
				}
				if (passTypeID != -1)
				{
					sbUpdate.Append("pass_type_id = '" +  passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("pass_type_id = NULL, ");
				}
				if (!dateStart.Equals(new DateTime()))
				{
					sbUpdate.Append("date_start = '" +  dateStart.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_start = NULL, ");
				}
				if (!dateEnd.Equals(new DateTime()))
				{
					sbUpdate.Append("date_end = '" +  dateEnd.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_end = NULL, ");
				}
				if (used != -1)
				{
					sbUpdate.Append("used = '" + used.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("used = NULL, ");
				}
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE rec_id = '" + recID.ToString().Trim() + "'");

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

		public bool updateEAdeleteIOP(int recID, int employeeID, int passTypeIDOld, int passTypeID, DateTime dateStartOld,
			DateTime dateEndOld, DateTime dateStart, DateTime dateEnd, int used, DAOFactory factory, IDbTransaction trans, string description)
		{
			MySQLIOPairDAO iopDAO = (MySQLIOPairDAO) factory.getIOPairDAO(null);
			
			bool isUpdated = true;

            List<IOPairTO> ioPairs = new List<IOPairTO>();
            IOPairTO pair = new IOPairTO();
            pair.EmployeeID = employeeID;
            if (trans != null)
            {
                ioPairs = iopDAO.getIOPairs(pair, dateStartOld, dateEndOld.AddDays(1), "", -1, trans);
            }
            else
            {
                ioPairs = iopDAO.getIOPairs(pair, dateStartOld, dateEndOld.AddDays(1), "", -1);
            }

            if (trans != null)
            {
                this.SqlTrans = (MySqlTransaction)trans;
            }
            else
            {
                this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }

			iopDAO.SqlTrans = this.SqlTrans;

			try
			{
				// update Employee Absence
				StringBuilder sbUpdate = new StringBuilder();
				sbUpdate.Append("UPDATE employee_absences SET ");

				if (employeeID != -1)
				{
					sbUpdate.Append("employee_id = '" + employeeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("employee_id = NULL, ");
				}
				if (passTypeID != -1)
				{
					sbUpdate.Append("pass_type_id = '" +  passTypeID.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("pass_type_id = NULL, ");
				}
				if (!dateStart.Equals(new DateTime()))
				{
					sbUpdate.Append("date_start = '" +  dateStart.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_start = NULL, ");
				}
				if (!dateEnd.Equals(new DateTime()))
				{
					sbUpdate.Append("date_end = '" +  dateEnd.ToString(dateTimeformat) + "', ");
				}
				else
				{
					sbUpdate.Append("date_end = NULL, ");
				}
				if (used != -1)
				{
					sbUpdate.Append("used = '" +  used.ToString().Trim() + "', ");
				}
				else
				{
					sbUpdate.Append("used = NULL, ");
				}
                if (!description.Equals(""))
                {
                    sbUpdate.Append("description = N'" + description.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("description = NULL, ");
                }
				sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
				sbUpdate.Append("modified_time = NOW() ");
				sbUpdate.Append("WHERE rec_id = '" + recID.ToString().Trim() + "'");

				MySqlCommand cmd = new MySqlCommand( sbUpdate.ToString(), conn, SqlTrans );
				int res = cmd.ExecuteNonQuery();
				if(res > 0)
				{
					isUpdated = true;
				}

				List<IOPairTO> ioPairAbsences = new List<IOPairTO>();
				foreach (IOPairTO iop in ioPairs)
				{
					if (iop.PassTypeID == passTypeIDOld)
					{
						ioPairAbsences.Add(iop);
					}
				}

				foreach (IOPairTO iopTO in ioPairAbsences)
				{
                    if (iopTO.IOPairDate.Date.Equals(dateEndOld.AddDays(1).Date))
                    {
                        if (iopTO.StartTime.Hour == 0 && iopTO.StartTime.Minute == 0
                        && iopTO.EndTime.Hour == 7 && iopTO.EndTime.Minute == 0)
                        {
                            isUpdated = iopDAO.delete(iopTO.IOPairID, false) && isUpdated;
                        }
                        
                        continue;
                    }

					isUpdated = iopDAO.delete(iopTO.IOPairID, false) && isUpdated;
				}

                if (trans == null)
                {
                    if (isUpdated)
                    {
                        this.SqlTrans.Commit();
                    }
                    else
                    {
                        this.SqlTrans.Rollback();
                    }
                }
			}
			catch(Exception ex)
			{
                if (trans == null)
                {
                    this.SqlTrans.Rollback();
                }
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}
        public bool updateEAdeleteIOP(int recID, int employeeID, int passTypeIDOld, int passTypeID, DateTime dateStartOld,
        DateTime dateEndOld, DateTime dateStart, DateTime dateEnd, int used, DAOFactory factory, IDbTransaction trans)
        {
            MySQLIOPairDAO iopDAO = (MySQLIOPairDAO)factory.getIOPairDAO(conn);

            bool isUpdated = true;

            List<IOPairTO> ioPairs = new List<IOPairTO>();
            IOPairTO pair = new IOPairTO();
            pair.EmployeeID = employeeID;
            if (trans != null)
            {
                ioPairs = iopDAO.getIOPairs(pair, dateStartOld, dateEndOld.AddDays(1), "", -1, trans);
            }
            else
            {
                ioPairs = iopDAO.getIOPairs(pair, dateStartOld, dateEndOld.AddDays(1), "", -1);
            }

            if (trans != null)
            {
                this.SqlTrans = (MySqlTransaction)trans;
            }
            else
            {
                this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            }

            iopDAO.SqlTrans = this.SqlTrans;

            try
            {
                // update Employee Absence
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE employee_absences SET ");

                if (employeeID != -1)
                {
                    sbUpdate.Append("employee_id = '" + employeeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("employee_id = NULL, ");
                }
                if (passTypeID != -1)
                {
                    sbUpdate.Append("pass_type_id = '" + passTypeID.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("pass_type_id = NULL, ");
                }
                if (!dateStart.Equals(new DateTime()))
                {
                    sbUpdate.Append("date_start = '" + dateStart.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("date_start = NULL, ");
                }
                if (!dateEnd.Equals(new DateTime()))
                {
                    sbUpdate.Append("date_end = '" + dateEnd.ToString(dateTimeformat) + "', ");
                }
                else
                {
                    sbUpdate.Append("date_end = NULL, ");
                }
                if (used != -1)
                {
                    sbUpdate.Append("used = '" + used.ToString().Trim() + "', ");
                }
                else
                {
                    sbUpdate.Append("used = NULL, ");
                }
               
                sbUpdate.Append("modified_by = N'" + DAOController.GetLogInUser().Trim() + "', ");
                sbUpdate.Append("modified_time = NOW() ");
                sbUpdate.Append("WHERE rec_id = '" + recID.ToString().Trim() + "'");

                MySqlCommand cmd = new MySqlCommand(sbUpdate.ToString(), conn, SqlTrans);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isUpdated = true;
                }

                List<IOPairTO> ioPairAbsences = new List<IOPairTO>();
                foreach (IOPairTO iop in ioPairs)
                {
                    if (iop.PassTypeID == passTypeIDOld)
                    {
                        ioPairAbsences.Add(iop);
                    }
                }

                foreach (IOPairTO iopTO in ioPairAbsences)
                {
                    if (iopTO.IOPairDate.Date.Equals(dateEndOld.AddDays(1).Date))
                    {
                        if (iopTO.StartTime.Hour == 0 && iopTO.StartTime.Minute == 0
                        && iopTO.EndTime.Hour == 7 && iopTO.EndTime.Minute == 0)
                        {
                            isUpdated = iopDAO.delete(iopTO.IOPairID, false) && isUpdated;
                        }

                        continue;
                    }

                    isUpdated = iopDAO.delete(iopTO.IOPairID, false) && isUpdated;
                }

                if (trans == null)
                {
                    if (isUpdated)
                    {
                        this.SqlTrans.Commit();
                    }
                    else
                    {
                        this.SqlTrans.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                if (trans == null)
                {
                    this.SqlTrans.Rollback();
                }
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

		public bool delete(int recID)
		{
			bool isDeleted = false;
			MySqlTransaction sqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);

			try
			{
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM employee_absences WHERE rec_id = '" + recID.ToString() + "'");
				
				MySqlCommand cmd = new MySqlCommand( sbDelete.ToString(), conn, sqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}
				sqlTrans.Commit();
			}
			catch(Exception ex)
			{
				sqlTrans.Rollback();
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool deleteEAdeleteIOP(int recID, int employeeID, int passTypeID, DateTime dateStart, 
			DateTime dateEnd, DAOFactory factory)
		{
			MySQLIOPairDAO iopDAO = (MySQLIOPairDAO) factory.getIOPairDAO(conn);
			
			bool isDeleted = true;

            IOPairTO pair = new IOPairTO();
            pair.EmployeeID = employeeID;
			List<IOPairTO> ioPairs = iopDAO.getIOPairs(pair, dateStart, dateEnd.AddDays(1), "", -1);

            this.SqlTrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
			iopDAO.SqlTrans = this.SqlTrans;

			try
			{
				// delete Employee Absence
				StringBuilder sbDelete = new StringBuilder();
				sbDelete.Append("DELETE FROM employee_absences WHERE rec_id = '" + recID.ToString() + "'");
				
				MySqlCommand cmd = new MySqlCommand( sbDelete.ToString(), conn, this.SqlTrans);
				int res = cmd.ExecuteNonQuery();
				if (res != 0)
				{
					isDeleted = true;
				}

				List<IOPairTO> ioPairAbsences = new List<IOPairTO>();
				foreach (IOPairTO iop in ioPairs)
				{
					if (iop.PassTypeID == passTypeID)
					{
						ioPairAbsences.Add(iop);
					}
				}

				foreach (IOPairTO iopTO in ioPairAbsences)
				{
                    if (iopTO.IOPairDate.Date.Equals(dateEnd.AddDays(1).Date))
                    {
                        if (iopTO.StartTime.Hour == 0 && iopTO.StartTime.Minute == 0
                        && iopTO.EndTime.Hour == 7 && iopTO.EndTime.Minute == 0)
                        {
                            isDeleted = iopDAO.delete(iopTO.IOPairID, false) && isDeleted;
                        }
                        
                        continue;
                    }

					isDeleted = iopDAO.delete(iopTO.IOPairID, false) && isDeleted;
				}

                if (isDeleted)
                {
                    this.SqlTrans.Commit();
                }
                else
                {
                    this.SqlTrans.Rollback();
                }
			}
			catch(Exception ex)
			{
                this.SqlTrans.Rollback();
               
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public EmployeeAbsenceTO find(int recID)
		{
			DataSet dataSet = new DataSet();
			EmployeeAbsenceTO emplAbsence = new EmployeeAbsenceTO();
			try
			{
				MySqlCommand cmd = new MySqlCommand( "SELECT * FROM employee_absences WHERE rec_id = '" + recID.ToString().Trim() + "'", conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeAbsence");
				DataTable table = dataSet.Tables["EmployeeAbsence"];

				if (table.Rows.Count == 1)
				{
					emplAbsence = new EmployeeAbsenceTO();
					emplAbsence.RecID = Int32.Parse(table.Rows[0]["rec_id"].ToString().Trim());
					
					if (table.Rows[0]["employee_id"] != DBNull.Value)
					{
						emplAbsence.EmployeeID = Int32.Parse(table.Rows[0]["employee_id"].ToString().Trim());
					}
					if (table.Rows[0]["pass_type_id"] != DBNull.Value)
					{
						emplAbsence.PassTypeID = Int32.Parse(table.Rows[0]["pass_type_id"].ToString().Trim());
					}
					if (table.Rows[0]["date_start"] != DBNull.Value)
					{
						emplAbsence.DateStart = DateTime.Parse(table.Rows[0]["date_start"].ToString());
					}
					if (table.Rows[0]["date_end"] != DBNull.Value)
					{
						emplAbsence.DateEnd = DateTime.Parse(table.Rows[0]["date_end"].ToString());
					}
					if (table.Rows[0]["used"] != DBNull.Value)
					{
						emplAbsence.Used = Int32.Parse(table.Rows[0]["used"].ToString().Trim());
					}
                    if (table.Rows[0]["description"] != DBNull.Value)
                    {
                        emplAbsence.Description = table.Rows[0]["description"].ToString().Trim();
                    }
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return emplAbsence;
		}

        // Same as method getEmployeeAbsences(string  employeeID, string passTypeID, DateTime dateStart, DateTime dateEnd, string used, string wUnits, IDBTransaction trans)
		// command is made without transaction
        public List<EmployeeAbsenceTO> getEmployeeAbsences(EmployeeAbsenceTO eaTO, string wUnits)
		{
			DataSet dataSet = new DataSet();
			EmployeeAbsenceTO emplAbsence = new EmployeeAbsenceTO();
			List<EmployeeAbsenceTO> emplAbsenceList = new List<EmployeeAbsenceTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT ea.*, e.last_name e_last_name, e.first_name e_first_name, pt.description pt_description FROM employee_absences ea, employees e, pass_types pt ");
				sb.Append("WHERE ea.employee_id = e.employee_id AND ea.pass_type_id = pt.pass_type_id AND");

                if ((eaTO.EmployeeID != -1) || (eaTO.PassTypeID != -1) ||
                    (!eaTO.DateStart.Date.Equals(new DateTime().Date)) || (!eaTO.DateEnd.Date.Equals(new DateTime().Date))
                    || eaTO.Used != -1)
				{	
					if (eaTO.EmployeeID != -1)
					{
						//sb.Append(" UPPER(ea.employee_id) LIKE '" + employeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.employee_id = '" + eaTO.EmployeeID.ToString().Trim() + "' AND");
					}
                    if (eaTO.PassTypeID != -1)
					{
						//sb.Append(" UPPER(ea.pass_type_id) LIKE '" + passTypeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.pass_type_id = '" + eaTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) || (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
					{
						/*sb.Append(" ((ea.date_start >= '" + dateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
						sb.Append(" ea.date_start <= '" + dateEnd.Date.ToString(dateTimeformat).Trim() + "') OR");
						sb.Append(" (ea.date_end >= '" + dateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
						sb.Append(" ea.date_end <= '" + dateEnd.Date.ToString(dateTimeformat).Trim() + "')) AND");*/

                        /* 2008-03-14
                         * allow one of this dates to be null */
                        if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) && (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
                        {
                            sb.Append(" ((ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "') OR");
                            sb.Append(" (ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "')) AND");
                        }
                        else if (!eaTO.DateStart.Date.Equals(new DateTime().Date))
                        {
                            sb.Append(" ((ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "') OR");
                            sb.Append(" (ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "')) AND");
                        }
                        else if (!eaTO.DateEnd.Date.Equals(new DateTime().Date))
                        {
                            sb.Append(" (ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "') AND");
                        }
					}
                    if (eaTO.Used != -1)
					{
						//sb.Append(" UPPER(ea.used) LIKE '" + used.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.used = '" + eaTO.Used.ToString().Trim() + "' AND");
					}
				}

				select = sb.ToString(0, sb.ToString().Length - 3);

				if (!wUnits.Trim().Equals(""))
				{
					select += " AND e.working_unit_id IN (" + wUnits + ")";
				}

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeAbsence");
				DataTable table = dataSet.Tables["EmployeeAbsence"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplAbsence = new EmployeeAbsenceTO();
						emplAbsence.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
					
						if (row["employee_id"] != DBNull.Value)
						{
							emplAbsence.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							emplAbsence.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["date_start"] != DBNull.Value)
						{
							emplAbsence.DateStart = DateTime.Parse(row["date_start"].ToString());
						}
						if (row["date_end"] != DBNull.Value)
						{
							emplAbsence.DateEnd = DateTime.Parse(row["date_end"].ToString());
						}
						if (row["used"] != DBNull.Value)
						{
							emplAbsence.Used = Int32.Parse(row["used"].ToString().Trim());
						}
                        if (row["created_time"] != DBNull.Value)
                        {
                            emplAbsence.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
						if (row["e_last_name"] != DBNull.Value)
						{
							emplAbsence.EmployeeName = row["e_last_name"].ToString().Trim();
						}
						if (row["e_first_name"] != DBNull.Value)
						{
							emplAbsence.EmployeeName += " " + row["e_first_name"].ToString().Trim();
						}
						if (row["pt_description"] != DBNull.Value)
						{
							emplAbsence.PassType = row["pt_description"].ToString().Trim();
						}

						emplAbsenceList.Add(emplAbsence);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			
			return emplAbsenceList;
		}

        public List<EmployeeAbsenceTO> getEmployeeAbsences(EmployeeAbsenceTO eaTO)
        {
            DataSet dataSet = new DataSet();
            EmployeeAbsenceTO emplAbsence = new EmployeeAbsenceTO();
            List<EmployeeAbsenceTO> emplAbsenceList = new List<EmployeeAbsenceTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ea.*, e.last_name e_last_name, e.first_name e_first_name, pt.description pt_description FROM employee_absences ea, employees e, pass_types pt ");
                sb.Append("WHERE ea.employee_id = e.employee_id AND ea.pass_type_id = pt.pass_type_id AND");

                if ((eaTO.EmployeeID != -1) || (eaTO.PassTypeID != -1) ||
                    (!eaTO.DateStart.Date.Equals(new DateTime().Date)) || (!eaTO.DateEnd.Date.Equals(new DateTime().Date))
                    || eaTO.Used != -1)
                {
                    if (eaTO.EmployeeID != -1)
                    {
                        //sb.Append(" UPPER(ea.employee_id) LIKE '" + employeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.employee_id = '" + eaTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (eaTO.PassTypeID != -1)
                    {
                        //sb.Append(" UPPER(ea.pass_type_id) LIKE '" + passTypeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.pass_type_id = '" + eaTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) || (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
                    {
                        /*sb.Append(" ((ea.date_start >= '" + dateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.date_start <= '" + dateEnd.Date.ToString(dateTimeformat).Trim() + "') OR");
                        sb.Append(" (ea.date_end >= '" + dateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.date_end <= '" + dateEnd.Date.ToString(dateTimeformat).Trim() + "')) AND");*/

                        /* 2008-03-14
                         * allow one of this dates to be null */
                        if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) && (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
                        {
                            sb.Append(" ((ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "') OR");
                            sb.Append(" (ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "')) AND");
                        }
                        else if (!eaTO.DateStart.Date.Equals(new DateTime().Date))
                        {
                            sb.Append(" ((ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "') OR");
                            sb.Append(" (ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "')) AND");
                        }
                        else if (!eaTO.DateEnd.Date.Equals(new DateTime().Date))
                        {
                            sb.Append(" (ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "') AND");
                        }
                    }
                    if (eaTO.Used != -1)
                    {
                        //sb.Append(" UPPER(ea.used) LIKE '" + used.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.used = '" + eaTO.Used.ToString().Trim() + "' AND");
                    }
                }

                select = sb.ToString(0, sb.ToString().Length - 3);

              

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeAbsence");
                DataTable table = dataSet.Tables["EmployeeAbsence"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplAbsence = new EmployeeAbsenceTO();
                        emplAbsence.RecID = Int32.Parse(row["rec_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            emplAbsence.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            emplAbsence.DateStart = DateTime.Parse(row["date_start"].ToString());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            emplAbsence.DateEnd = DateTime.Parse(row["date_end"].ToString());
                        }
                        if (row["used"] != DBNull.Value)
                        {
                            emplAbsence.Used = Int32.Parse(row["used"].ToString().Trim());
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            emplAbsence.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["e_last_name"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeName = row["e_last_name"].ToString().Trim();
                        }
                        if (row["e_first_name"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeName += " " + row["e_first_name"].ToString().Trim();
                        }
                        if (row["pt_description"] != DBNull.Value)
                        {
                            emplAbsence.PassType = row["pt_description"].ToString().Trim();
                        }

                        emplAbsenceList.Add(emplAbsence);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplAbsenceList;
        }

        // Same as method getEmployeeAbsences(string  employeeID, string passTypeID, DateTime dateStart, DateTime dateEnd, string used, string wUnits)
        // command is made with transaction
        public List<EmployeeAbsenceTO> getEmployeeAbsences(EmployeeAbsenceTO eaTO, string wUnits, IDbTransaction trans)
        {
            DataSet dataSet = new DataSet();
            EmployeeAbsenceTO emplAbsence = new EmployeeAbsenceTO();
            List<EmployeeAbsenceTO> emplAbsenceList = new List<EmployeeAbsenceTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ea.*, e.last_name e_last_name, e.first_name e_first_name, pt.description pt_description FROM employee_absences ea, employees e, pass_types pt ");
                sb.Append("WHERE ea.employee_id = e.employee_id AND ea.pass_type_id = pt.pass_type_id AND");

                if ((eaTO.EmployeeID != -1) || (eaTO.PassTypeID != -1) ||
                    (!eaTO.DateStart.Date.Equals(new DateTime().Date)) || (!eaTO.DateEnd.Date.Equals(new DateTime().Date))
                    || eaTO.Used != -1)
                {
                    if (eaTO.EmployeeID != -1)
                    {
                        //sb.Append(" UPPER(ea.employee_id) LIKE '" + employeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.employee_id = '" + eaTO.EmployeeID.ToString().Trim() + "' AND");
                    }
                    if (eaTO.PassTypeID != -1)
                    {
                        //sb.Append(" UPPER(ea.pass_type_id) LIKE '" + passTypeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.pass_type_id = '" + eaTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) || (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
                    {
                        /*sb.Append(" ((ea.date_start >= '" + dateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.date_start <= '" + dateEnd.Date.ToString(dateTimeformat).Trim() + "') OR");
                        sb.Append(" (ea.date_end >= '" + dateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.date_end <= '" + dateEnd.Date.ToString(dateTimeformat).Trim() + "')) AND");*/

                        /* 2008-03-14
                         * allow one of this dates to be null */
                        if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) && (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
                        {
                            sb.Append(" ((ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "') OR");
                            sb.Append(" (ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "')) AND");
                        }
                        else if (!eaTO.DateStart.Date.Equals(new DateTime().Date))
                        {
                            sb.Append(" ((ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                            sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "') OR");
                            sb.Append(" (ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "')) AND");
                        }
                        else if (!eaTO.DateEnd.Date.Equals(new DateTime().Date))
                        {
                            sb.Append(" (ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "') AND");
                        }
                    }
                    if (eaTO.Used != -1)
                    {
                        //sb.Append(" UPPER(ea.used) LIKE '" + used.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.used = '" + eaTO.Used.ToString().Trim() + "' AND");
                    }
                }

                select = sb.ToString(0, sb.ToString().Length - 3);

                if (!wUnits.Trim().Equals(""))
                {
                    select += " AND e.working_unit_id IN (" + wUnits + ")";
                }

                MySqlCommand cmd = new MySqlCommand(select, conn, (MySqlTransaction) trans);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeAbsence");
                DataTable table = dataSet.Tables["EmployeeAbsence"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplAbsence = new EmployeeAbsenceTO();
                        emplAbsence.RecID = Int32.Parse(row["rec_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            emplAbsence.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            emplAbsence.DateStart = DateTime.Parse(row["date_start"].ToString());
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            emplAbsence.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            emplAbsence.DateEnd = DateTime.Parse(row["date_end"].ToString());
                        }
                        if (row["used"] != DBNull.Value)
                        {
                            emplAbsence.Used = Int32.Parse(row["used"].ToString().Trim());
                        }
                        if (row["e_last_name"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeName = row["e_last_name"].ToString().Trim();
                        }
                        if (row["e_first_name"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeName += " " + row["e_first_name"].ToString().Trim();
                        }
                        if (row["pt_description"] != DBNull.Value)
                        {
                            emplAbsence.PassType = row["pt_description"].ToString().Trim();
                        }

                        emplAbsenceList.Add(emplAbsence);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplAbsenceList;
        }

		//Added this function because Search algorithm is changed for Absences form, 
		//and original Search function is used in IOPair, for insertWholeDayAbsences
		public List<EmployeeAbsenceTO> getEmployeeAbsencesForAbsences(EmployeeAbsenceTO eaTO, string wUnits)
		{
			DataSet dataSet = new DataSet();
			EmployeeAbsenceTO emplAbsence = new EmployeeAbsenceTO();
			List<EmployeeAbsenceTO> emplAbsenceList = new List<EmployeeAbsenceTO>();
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT ea.*, e.last_name e_last_name, e.first_name e_first_name, pt.description pt_description FROM employee_absences ea, employees e, pass_types pt ");
				sb.Append("WHERE ea.employee_id = e.employee_id AND ea.pass_type_id = pt.pass_type_id AND");

                if ((eaTO.EmployeeID != -1) || (eaTO.PassTypeID != -1) ||
                    (!eaTO.DateStart.Date.Equals(new DateTime().Date)) || (!eaTO.DateEnd.Date.Equals(new DateTime().Date))
                    || eaTO.Used != -1)
				{	
					if (eaTO.EmployeeID != -1)
					{
						//sb.Append(" UPPER(ea.employee_id) LIKE '" + employeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.employee_id = '" + eaTO.EmployeeID.ToString().Trim() + "' AND");
					}
					if (eaTO.PassTypeID != -1)
					{
						//sb.Append(" UPPER(ea.pass_type_id) LIKE '" + passTypeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.pass_type_id = '" + eaTO.PassTypeID.ToString().Trim() + "' AND");
					}
                    if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) && (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
					{
                        sb.Append(" ((ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "') OR");
                        sb.Append(" (ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "')) AND");
					}
                    if (eaTO.Used != -1)
					{
						//sb.Append(" UPPER(ea.used) LIKE '" + used.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.used = '" + eaTO.Used.ToString().Trim() + "' AND");
					}
				}

				select = sb.ToString(0, sb.ToString().Length - 3);

				if (!wUnits.Trim().Equals(""))
				{
					select += " AND e.working_unit_id IN (" + wUnits + ")";
				}

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeAbsence");
				DataTable table = dataSet.Tables["EmployeeAbsence"];

				if (table.Rows.Count > 0)
				{
					foreach(DataRow row in table.Rows)
					{
						emplAbsence = new EmployeeAbsenceTO();
						emplAbsence.RecID = Int32.Parse(row["rec_id"].ToString().Trim());
					
						if (row["employee_id"] != DBNull.Value)
						{
							emplAbsence.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
						}
						if (row["pass_type_id"] != DBNull.Value)
						{
							emplAbsence.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
						}
						if (row["date_start"] != DBNull.Value)
						{
							emplAbsence.DateStart = DateTime.Parse(row["date_start"].ToString());
						}
						if (row["date_end"] != DBNull.Value)
						{
							emplAbsence.DateEnd = DateTime.Parse(row["date_end"].ToString());
						}
                        if (row["created_time"] != DBNull.Value)
                        {
                            emplAbsence.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
						if (row["used"] != DBNull.Value)
						{
							emplAbsence.Used = Int32.Parse(row["used"].ToString().Trim());
						}
						if (row["e_last_name"] != DBNull.Value)
						{
							emplAbsence.EmployeeName = row["e_last_name"].ToString().Trim();
						}
						if (row["e_first_name"] != DBNull.Value)
						{
							emplAbsence.EmployeeName += " " + row["e_first_name"].ToString().Trim();
						}
						if (row["pt_description"] != DBNull.Value)
						{
							emplAbsence.PassType = row["pt_description"].ToString().Trim();
						}
                        if (row["description"] != DBNull.Value)
                        {
                            emplAbsence.Description = row["description"].ToString().Trim();
                        }
                        if (row["vac_year"] != DBNull.Value)
                        {
                            emplAbsence.VacationYear = DateTime.Parse(row["vac_year"].ToString());
                        }

						emplAbsenceList.Add(emplAbsence);
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			
			return emplAbsenceList;
		}

		public int getExistingEmployeeAbsences(EmployeeAbsenceTO eaTO, string wUnits)
		{
			DataSet dataSet = new DataSet();
			int count = 0;
			string select = "";

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT COUNT(*) count FROM employee_absences ea, employees e ");
				sb.Append("WHERE ea.employee_id = e.employee_id AND ");
                sb.Append("ea.employee_id = " + eaTO.EmployeeID.ToString().Trim() + " AND ");
                sb.Append("ea.rec_id <> " + eaTO.RecID.ToString().Trim() + " AND ");
				/*sb.Append("((ea.date_start >= '" + dateStart.Date.ToString(dateTimeformat).Trim() + "' AND ");
				sb.Append("ea.date_start <= '" + dateEnd.Date.ToString(dateTimeformat).Trim() + "') OR ");
				sb.Append("(ea.date_end >= '" + dateStart.Date.ToString(dateTimeformat).Trim() + "' AND ");
				sb.Append("ea.date_end <= '" + dateEnd.Date.ToString(dateTimeformat).Trim() + "')) AND");*/
                if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) && (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
                {
                    sb.Append(" ((ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                    sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "') OR");
                    sb.Append(" (ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                    sb.Append(" ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "')) AND");
                }

				select = sb.ToString(0, sb.ToString().Length - 3);

				if (!wUnits.Trim().Equals(""))
				{
					select += "AND e.working_unit_id IN (" + wUnits + ")";
				}

				MySqlCommand cmd = new MySqlCommand(select, conn );
				MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

				sqlDataAdapter.Fill(dataSet, "EmployeeAbsence");
				DataTable table = dataSet.Tables["EmployeeAbsence"];

				if (table.Rows.Count > 0)
				{
					if (table.Rows[0]["count"] != DBNull.Value)
					{
						count = Int32.Parse(table.Rows[0]["count"].ToString().Trim());
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			
			return count;
		}
        public List<EmployeeAbsenceTO> getEmployeeAbsencesForVacEvid(EmployeeAbsenceTO eaTO, string employees, string wUnits, DateTime relatedYearFrom, DateTime relatedYearTo)
        {
            DataSet dataSet = new DataSet();
            EmployeeAbsenceTO emplAbsence = new EmployeeAbsenceTO();
            List<EmployeeAbsenceTO> emplAbsenceList = new List<EmployeeAbsenceTO>();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ea.*, e.last_name e_last_name, e.first_name e_first_name, pt.description pt_description FROM employee_absences ea, employees e, pass_types pt ");
                sb.Append("WHERE ea.employee_id = e.employee_id AND ea.pass_type_id = pt.pass_type_id AND");

                if ((!employees.Trim().Equals("")) || (eaTO.PassTypeID != -1) ||
                    (!eaTO.DateStart.Date.Equals(new DateTime().Date)) || (!eaTO.DateEnd.Date.Equals(new DateTime().Date))
                    || eaTO.Used != -1)
                {
                    if (!employees.Trim().Equals(""))
                    {
                        //sb.Append(" UPPER(ea.employee_id) LIKE '" + employeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.employee_id IN (" + employees.Trim() + ") AND");
                    }
                    if (eaTO.PassTypeID != -1)
                    {
                        //sb.Append(" UPPER(ea.pass_type_id) LIKE '" + passTypeID.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.pass_type_id = '" + eaTO.PassTypeID.ToString().Trim() + "' AND");
                    }
                    if ((!eaTO.DateStart.Date.Equals(new DateTime().Date)) && (!eaTO.DateEnd.Date.Equals(new DateTime().Date)))
                    {
                        sb.Append(" ((ea.date_start >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.date_start <= '" + eaTO.DateEnd.Date.ToString(dateTimeformat).Trim() + "') OR");
                        sb.Append(" (ea.date_start < '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.date_end >= '" + eaTO.DateStart.Date.ToString(dateTimeformat).Trim() + "')) AND");
                    }
                    if ((!relatedYearFrom.Date.Equals(new DateTime().Date)) && (!relatedYearTo.Date.Equals(new DateTime().Date)))
                    {
                        sb.Append(" (ea.vac_year >= '" + relatedYearFrom.Date.ToString(dateTimeformat).Trim() + "' AND");
                        sb.Append(" ea.vac_year <= '" + relatedYearTo.Date.ToString(dateTimeformat).Trim() + "') AND");
                    }
                    if (eaTO.Used != -1)
                    {
                        //sb.Append(" UPPER(ea.used) LIKE '" + used.Trim().ToUpper() + "' AND");
                        sb.Append(" ea.used = '" + eaTO.Used.ToString().Trim() + "' AND");
                    }
                }

                select = sb.ToString(0, sb.ToString().Length - 3);

                if (!wUnits.Trim().Equals(""))
                {
                    select += " AND e.working_unit_id IN (" + wUnits + ")";
                }

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "EmployeeAbsence");
                DataTable table = dataSet.Tables["EmployeeAbsence"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        emplAbsence = new EmployeeAbsenceTO();
                        emplAbsence.RecID = Int32.Parse(row["rec_id"].ToString().Trim());

                        if (row["employee_id"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["pass_type_id"] != DBNull.Value)
                        {
                            emplAbsence.PassTypeID = Int32.Parse(row["pass_type_id"].ToString().Trim());
                        }
                        if (row["date_start"] != DBNull.Value)
                        {
                            emplAbsence.DateStart = DateTime.Parse(row["date_start"].ToString());
                        }
                        if (row["date_end"] != DBNull.Value)
                        {
                            emplAbsence.DateEnd = DateTime.Parse(row["date_end"].ToString());
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            emplAbsence.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                        if (row["used"] != DBNull.Value)
                        {
                            emplAbsence.Used = Int32.Parse(row["used"].ToString().Trim());
                        }
                        if (row["created_by"] != DBNull.Value)
                        {
                            emplAbsence.CreatedBy = row["created_by"].ToString().Trim();
                        }
                        if (row["e_last_name"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeName = row["e_last_name"].ToString().Trim();
                        }
                        if (row["e_first_name"] != DBNull.Value)
                        {
                            emplAbsence.EmployeeName += " " + row["e_first_name"].ToString().Trim();
                        }
                        if (row["pt_description"] != DBNull.Value)
                        {
                            emplAbsence.PassType = row["pt_description"].ToString().Trim();
                        }
                        if (row["vac_year"] != DBNull.Value)
                        {
                            emplAbsence.VacationYear = DateTime.Parse(row["vac_year"].ToString());
                        }

                        emplAbsenceList.Add(emplAbsence);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return emplAbsenceList;
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
