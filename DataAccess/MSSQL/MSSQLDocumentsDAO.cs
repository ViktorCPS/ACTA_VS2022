using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// <summary>
    /// EmployeeDAO implementation for managing Employee's data form MSSQL database. 
    /// </summary>
    public class MSSQLDocumentsDAO : DocumentsDAO
    {
        SqlConnection conn = null;
        SqlTransaction _sqlTrans = null;
        string database = "";
        protected string dateTimeformat = "";

        public SqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MSSQLDocumentsDAO()
        {
            conn = MSSQLDAOFactory.getConnection();
            database = Constants.GetDatabaseString + "_doc.actamgr.";
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public MSSQLDocumentsDAO(SqlConnection sqlConnection)
        {
            conn = sqlConnection;
            database = Constants.GetDatabaseString + "_doc.actamgr.";
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            DAOController.GetInstance();
        }

        public void SetDBConnection(Object dbConnection)
        {
            conn = dbConnection as SqlConnection;
        }

        public int insert(int documentID, String firstName, String lastName, String documentName,
            String documentDesc, byte[] document, String extension, bool doCommit)
        {
            int rowsAffected = 0;
            SqlTransaction sqltrans = null;
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

                sbInsert.Append(documentID + ", ");
                sbInsert.Append("N'" + firstName + "', N'" + lastName + "', ");
                sbInsert.Append("N'" + documentName + "', ");
                sbInsert.Append("N'" + documentDesc + "', ");

                //sbInsert.Append(documentID.ToString().Trim() + ", ");
                //sbInsert.Append("N'" + firstName.Trim() + "', N'" + lastName.Trim() + "', ");
                //sbInsert.Append("N'" + documentName.Trim() + "', ");
                //sbInsert.Append("N'" + documentDesc.Trim() + "', ");

                sbInsert.Append("@Picture, ");
                //if (!document.Equals(""))
                //    sbInsert.Append("N'" + document + "', ");
                //else
                //    sbInsert.Append("NULL, ");
                sbInsert.Append("N'" + extension + "', ");
                sbInsert.Append("N'" + DAOController.GetLogInUser().Trim() + "', GETDATE()) ");
                SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqltrans);
                cmd.Parameters.Add("@Picture", SqlDbType.Image, document.Length).Value = document;
               
               
               // SqlCommand cmd = new SqlCommand(sbInsert.ToString(), conn, sqltrans);
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

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT d.employee_id, d.first_name, d.last_name, d.document_name, d.document_description, d.document, d.created_time FROM " + database + "documents d ");
                if ((docTO.DocumentsID != -1) || (!docTO.FirstName.Trim().Equals("")) ||
                   (!docTO.LastName.Trim().Equals("")) || (!docTO.DocName.Trim().Equals("")) || 
                   (!docTO.DocDesc.Trim().Equals("")))
                {
                    sb.Append(" WHERE");

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
                        sb.Append(" UPPER(d.last_name) LIKE N'%" + docTO.LastName.Trim().ToUpper() + "%' AND");
                    }
                    if (!docTO.DocName.Trim().Equals(""))
                    {
                        sb.Append(" UPPER(d.document_name) LIKE N'%" + docTO.DocName.Trim().ToUpper());
                    }
                    //if (!docTO.CreatedTime.ToString().Equals(""))
                    //{
                    //    sb.Append(" UPPER(e.password) LIKE N'%" + docTO.CreatedTime.ToString().ToUpper() + "%' AND");
                    //}
                    

                    select = sb.ToString(0, sb.ToString().Length - 3);
                }

                else
                {
                    select = sb.ToString();
                }

                select += " ORDER BY d.last_name, d.first_name ";

                SqlCommand cmd = new SqlCommand(select, conn);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

                sqlDataAdapter.Fill(dataSet, "Document");
                DataTable table = dataSet.Tables["Document"];

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        document = new DocumentsTO();
                        document.DocumentsID = Int32.Parse(row["employee_id"].ToString().Trim());
                        if (row["first_name"] != DBNull.Value)
                        {
                            document.FirstName = row["first_name"].ToString().Trim();
                        }
                        if (row["last_name"] != DBNull.Value)
                        {
                            document.LastName = row["last_name"].ToString().Trim();
                        }
                        if (row["document_name"] != DBNull.Value)
                        {
                            document.DocName = row["document_name"].ToString().Trim();
                        }
                        if (row["document_description"] != DBNull.Value)
                        {
                            document.DocDesc = row["document_description"].ToString().Trim();
                        }
                        if (!row["document"].Equals(DBNull.Value))
                        {
                            document.Picture = (byte[])row["document"];
                        }
                        if (row["created_time"] != DBNull.Value)
                        {
                            document.CreatedTime = DateTime.Parse(row["created_time"].ToString().Trim());
                        }
                      
                        documentsList.Add(document);
                    }
                }
            
                
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

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT employee_id, first_name, last_name, document_name, document_description, document, extension FROM " + database + "documents ");

                if (!documentsID.Equals(""))
                {
                    sb.Append(" WHERE ");
                    sb.Append("employee_id IN (" + documentsID.ToString().Trim() + ") ");
                }
                sb.Append(" ORDER BY employee_id");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

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
                            employeeDocumentFileTO.DocDesc = row["document_description"].ToString().Trim();
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
