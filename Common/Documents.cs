using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	[Serializable()]    
	/// <summary>
	/// Documents
	/// </summary>
	public class Documents : ISerializable 
	{
		DAOFactory daoFactory = null;
		DocumentsDAO edao = null;
		
		DebugLog log;

        private int _employeeID = -1;
        private byte[] _document = null;
        private DateTime _createdTime = new DateTime(0);
        private string _documentName = "";
        private string _documentDesc = "";
        private string _firstName = "";
        private string _lastName = "";
        private string _extension = "";
        DocumentsTO emplTO = new DocumentsTO();
        
        public DocumentsTO DocTO
        {
            get { return emplTO; }
            set { emplTO = value; }
        }

        public int DocumentsID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public byte[] Document
        {
            get { return _document; }
            set { _document = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string DocumentName
        {
            get { return _documentName; }
            set { _documentName = value; }
        }
        public string DocumentDesc
        {
            get { return _documentDesc; }
            set { _documentDesc = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

		public Documents()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getDocumentsDAO(null);
		}

        public Documents(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getDocumentsDAO(null);
            }
        }

        public Documents(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getDocumentsDAO(dbConnection);
        }

        public Documents(int DocumentsID, string firstName, string lastName,  
			string docName, string docDesc, byte[] document, string extension)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getDocumentsDAO(null);

            this.DocumentsID = DocumentsID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DocumentName = docName;
            this.DocumentDesc = docDesc;
            this.Document = document;
            this.Extension = extension;
		}

		public Documents(SerializationInfo info, StreamingContext ctxt)
		{
			//Get the values from info and assign them to the appropriate properties
            this.DocTO.DocumentsID = (int)info.GetValue("DocumentsID", typeof(int));
            this.DocTO.FirstName = (String)info.GetValue("FirstName", typeof(string));
            this.DocTO.LastName = (String)info.GetValue("LastName", typeof(string));
            this.DocTO.DocName = (String)info.GetValue("DocName", typeof(string));
            this.DocTO.DocDesc = (String)info.GetValue("DocDesc", typeof(string));
         //   this.DocTO.Picture = (String)info.GetValue("Picture", typeof(string));
			

			//this.AccessGroupID = (int)info.GetValue("AccessGroupID", typeof(int));
		}
        
		//Serialization function.
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			//You can use any custom name for your name-value pair. But make sure you
			// read the values with the same name. For ex:- If you write EmpId as "DocumentsId"
			// then you should read the same with "DocumentsId"
            info.AddValue("DocumentsID", this.DocTO.DocumentsID);
            info.AddValue("FirstName", this.DocTO.FirstName);
            info.AddValue("LastName", this.DocTO.LastName);
            info.AddValue("DocName", this.DocTO.DocName);
            info.AddValue("DocDesc", this.DocTO.DocDesc);
            info.AddValue("Picture", this.DocTO.Picture);
            info.AddValue("Extension", this.DocTO.Extension);
          

			//info.AddValue("AccessGroupID", this.AccessGroupID);
		}
        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = edao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                edao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                edao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return edao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                edao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int Save(int documentID, String firstName, String lastName, String documentName, 
            String documentDesc, byte[] document, string extension, bool doCommit)
        {
            try
            {
                return edao.insert(documentID, firstName, lastName, documentName, documentDesc,
                    document, extension, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.Save(): " + ex.Message + this.DocTO.DocumentsID.ToString() + "\n");
                throw ex;
            }
        }

        public List<DocumentsTO> Search()
        {
            try
            {
                return edao.getDocuments(this.DocTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public ArrayList SearchArray(string documentsID) 
        {
            ArrayList employeeDocumentsTOList = new ArrayList();
            ArrayList employeeDocumentFileList = new ArrayList();

            try
            {
                Documents employeeDocumentsFileMember = new Documents();
                employeeDocumentsTOList = edao.getDocumentsArray(documentsID);

                foreach (DocumentsTO docTO in employeeDocumentsTOList)
                {
                    employeeDocumentsFileMember = new Documents();
                    employeeDocumentsFileMember.ReceiveTransferObject(docTO);

                    employeeDocumentFileList.Add(employeeDocumentsFileMember);
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return employeeDocumentFileList;
        }
        public void ReceiveTransferObject(DocumentsTO employeeDocumentsFileTO)
        {
            this.DocumentsID = employeeDocumentsFileTO.DocumentsID;
            this.CreatedTime = employeeDocumentsFileTO.CreatedTime;
            this.DocumentName = employeeDocumentsFileTO.DocName;
            this.DocumentDesc = employeeDocumentsFileTO.DocDesc;
            this.FirstName = employeeDocumentsFileTO.FirstName;
            this.LastName = employeeDocumentsFileTO.LastName;
            this.Document = employeeDocumentsFileTO.Picture;
            this.Extension = employeeDocumentsFileTO.Extension;
        }

        
    }
}
