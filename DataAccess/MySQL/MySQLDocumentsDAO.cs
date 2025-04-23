using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

using TransferObjects;
using Util;
using System.Data.SqlClient;

namespace DataAccess
{
	/// <summary>
	/// EmployeeDAO implementation for managing Employee's data form MySQL database. 
	/// </summary>
	public class MySQLDocumentsDAO : DocumentsDAO
	{
		MySqlConnection conn = null;

        string database = "";
		protected string dateTimeformat = "";
		MySqlTransaction _sqlTrans = null;

		public MySqlTransaction SqlTrans
		{
			get {return _sqlTrans; }
			set {_sqlTrans = value; }
		}

		public MySQLDocumentsDAO()
		{
            conn = MySQLDAOFactory.getConnection();
            database = Constants.GetDatabaseString + "_doc.actamgr.";
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
			DAOController.GetInstance();
		}

        public MySQLDocumentsDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            database = Constants.GetDatabaseString + "_doc.actamgr.";
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public int insert(int documentID, String firstName, String lastName, String documentName,
            String documentDesc, byte[] document,String extension, bool doCommit)
        {
            int rowsAffected = 0;
            MySqlTransaction sqltrans = null;
            if (doCommit)
                sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            else
                sqltrans = this.SqlTrans;


            try
            {

                StringBuilder sbInsert = new StringBuilder();
                sbInsert.Append("INSERT INTO " + database + "documents");
                sbInsert.Append("(employee_id, first_name, last_name, document_name, document_description, document, extension, created_by, created_time) ");
                sbInsert.Append("VALUES (");

                sbInsert.Append("" + documentID + ", ");
                sbInsert.Append("" + firstName + ", ");
                sbInsert.Append("" + lastName + ", ");
                sbInsert.Append("" + documentName + ", ");
                sbInsert.Append("" + documentDesc + ", ");

                sbInsert.Append("?Document, ");

                sbInsert.Append("" + extension + ", ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', NOW(), NOW()) ");
                MySqlCommand cmd = new MySqlCommand(sbInsert.ToString(), conn, sqltrans);
                cmd.Parameters.Add("?Document", MySqlDbType.MediumBlob, document.Length).Value = document;

                rowsAffected = cmd.ExecuteNonQuery();
                if (doCommit)
                {
                    sqltrans.Commit();
                }
                rowsAffected = 1;
            }
            catch (Exception ex)
            {
                if (doCommit)
                {
                    sqltrans.Rollback();
                }
                throw ex;
            }

            return rowsAffected;
        }

        public List<DocumentsTO> getDocuments(DocumentsTO docTO)
        {
            DataSet dataSet = new DataSet();
            DocumentsTO document = new DocumentsTO();
            List<DocumentsTO> documentsList = new List<DocumentsTO>();
            string select = "";
            int rowsAffected = 0;
            MySqlTransaction sqltrans = null;
            //if (doCommit)
            //{
                sqltrans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            //}
            //else
            //{
            //    sqltrans = this.SqlTrans;
     //   }


            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT d.employee_id, d.first_name, d.last_name, d.document_name, d.document_description, d.created_time FROM " + database + "documents d ");
                if ((docTO.DocumentsID != -1) || (!docTO.FirstName.Trim().Equals("")) ||
                   (!docTO.LastName.Trim().Equals("")) || (!docTO.DocName.Trim().Equals("")) ||
                   (!docTO.DocDesc.Trim().Equals("")) || (!docTO.CreatedTime.ToString().Equals("")))
                {
                    sb.Append(" AND");

                    if (docTO.DocumentsID != -1)
                    {
                        //sb.Append(" UPPER(employee_id) LIKE '" + EmployeeID.Trim().ToUpper() + "' AND");
                        sb.Append(" d.employee_id = '" + docTO.DocumentsID.ToString().Trim() + "' AND");
                    }
                    if (!docTO.FirstName.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(d.first_name) LIKE N'%" + docTO.FirstName.Trim().ToUpper() + "%' AND");
                    }
                    if (!docTO.LastName.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(e.last_name) LIKE N'%" + docTO.LastName.Trim().ToUpper() + "%' AND");
                    }
                    if (!docTO.DocName.Trim().Equals(""))
                    {
                        //sb.Append(" UPPER(working_unit_id) LIKE '" + WorkingUnitID.Trim().ToUpper() + "' AND");
                        sb.Append(" e.working_unit_id IN (" + docTO.DocName.ToString().Trim() + ") AND");
                    }
                    if (!docTO.DocDesc.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(e.status) LIKE N'%" + docTO.DocDesc.Trim().ToUpper() + "%' AND");
                    }
                    if (!docTO.CreatedTime.ToString().Equals(""))
                    {
                        sb.Append(" UPPER(e.password) LIKE N'%" + docTO.CreatedTime.ToString().ToUpper() + "%' AND");
                    }


                    select = sb.ToString(0, sb.ToString().Length - 3);
                }

                else
                {
                    select = sb.ToString();
                }

                select += " ORDER BY d.last_name, d.first_name ";

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn, sqltrans);

           //     SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                //sqlDataAdapter.Fill(dataSet, "Document");
                //DataTable table = dataSet.Tables["Document"];

                //if (table.Rows.Count > 0)
                //{
                    //foreach (DataRow row in table.Rows)
                    //{
                    //    document = new DocumentsTO();
                    //    document.DocumentsID = Int32.Parse(row["employee_id"].ToString().Trim());
                    //    if (row["first_name"] != DBNull.Value)
                    //    {
                    //        document.FirstName = row["first_name"].ToString().Trim();
                    //    }
                    //    if (row["last_name"] != DBNull.Value)
                    //    {
                    //        document.LastName = row["last_name"].ToString().Trim();
                    //    }
                    //    if (row["document_name"] != DBNull.Value)
                    //    {
                    //        document.DocName = row["document_name"].ToString().Trim();
                    //    }
                    //    if (row["document_description"] != DBNull.Value)
                    //    {
                    //        document.DocDesc = row["document_description"].ToString().Trim();
                    //    }
                    //    if (row["created_time"] != DBNull.Value)
                    //    {
                    //        document.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                    //    }

                    //    documentsList.Add(document);
                    //}
             //   }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return documentsList;
        }


        public ArrayList getDocumentsArray(string documentsID)
        {
            DataSet dataSet = new DataSet();
            DocumentsTO employeeDocumentFileTO = new DocumentsTO();
            ArrayList employeeDocumentsFileList = new ArrayList();
            string select = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT employee_id, first_name, last_name, document_name, document_description, document, extension FROM " + database + "documents ");

                if (!documentsID.Equals(""))
                {
                    sb.Append(" WHERE ");
                    sb.Append("employee_id IN (" + documentsID.ToString().Trim() + ") ");
                }
                select = sb.ToString() + " ORDER BY employee_id";

                MySqlCommand cmd = new MySqlCommand(select, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Documents");
                DataTable table = dataSet.Tables["Documents"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        employeeDocumentFileTO = new DocumentsTO();

                        if (!row["employee_id"].Equals(DBNull.Value))
                        {
                            employeeDocumentFileTO.DocumentsID = Int32.Parse(row["employee_id"].ToString().Trim());
                        }
                        if (row["first_name"] != DBNull.Value)
                        {
                            employeeDocumentFileTO.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            employeeDocumentFileTO.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["document_name"] != DBNull.Value)
                        {
                            employeeDocumentFileTO.DocName = row["document_name"].ToString().Trim();
                        }
                        if (!row["document_description"].Equals(DBNull.Value))
                        {
                            employeeDocumentFileTO.DocName = row["document_description"].ToString().Trim();
                        }
                        if (!row["document"].Equals(DBNull.Value))
                        {
                            employeeDocumentFileTO.Picture = (byte[])row["document"];
                        }
                        if (row["extension"] != DBNull.Value)
                        {
                            employeeDocumentFileTO.Extension = row["extension"].ToString().Trim();
                        }
                        employeeDocumentsFileList.Add(employeeDocumentFileTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return employeeDocumentsFileList;
        }



        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as MySqlConnection;
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
        // TODO!!!
        public void serialize(ArrayList employeeImageFileTOList)
        {
            try
            {
                string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLEmployeeImageFileFile"];
                Stream stream = File.Open(filename, FileMode.Create);

                EmployeeImageFileTO[] employeeImageFileTOArray = (EmployeeImageFileTO[])employeeImageFileTOList.ToArray(typeof(EmployeeImageFileTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(EmployeeImageFileTO[]));
                bformatter.Serialize(stream, employeeImageFileTOArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
