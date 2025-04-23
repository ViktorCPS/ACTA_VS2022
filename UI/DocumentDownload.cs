using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;
using System.Resources;
using System.Globalization;
using System.Collections.Generic;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Form for adding new and updating existing Employee data 
	/// </summary>
	public class DocumentDownload : System.Windows.Forms.Form
	{

        private int _documentID = -1;
        private byte[] _document = null;
        private string _FirstName = "";
        private string _LastName = "";
        private DateTime _modifiedTime = new DateTime(0);
        private string _documentName = "";
        private string _documentDesc = "";
        private string _extension = "";
        private System.Windows.Forms.TextBox tbDocumentName;
        private System.Windows.Forms.Label lblDocName;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button btnTag;

		private CultureInfo culture;
		// Current Employee
		private DocumentsTO currentDoc = null;
		DebugLog log;
		ResourceManager rm;
		ApplUserTO logInUser;

		private Hashtable types;

		private List<WorkingUnitTO> wUnits;
        private Dictionary<int, WorkingUnitTO> wUnitsDic = new Dictionary<int, WorkingUnitTO>();
		private string wuString = "";

        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private List<OrganizationalUnitTO> oUnitsList = new List<OrganizationalUnitTO>();
        private string ouString = "";

		// Controller instance
		public NotificationController Controller;
		// Observer client instance
		public NotificationObserverClient observerClient;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;

        private int oldGroupID = -1;

        private string wu = "";
        private string type = "";
        List<DocumentsTO> docArray;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btnTimeSchedule;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblDocument;
		private System.Windows.Forms.TextBox tbDocument;
        private System.Windows.Forms.Button btnBrowse;

		private string oldStatus = "";
        private bool timeSchemaChanged;
        private bool tagChanged;
        Documents documComm =  new Documents();
		//Indicate if calling form needs to be reload
		public bool doReloadOnBack = true;

        EmployeeImageFile eif = new EmployeeImageFile();
        Documents docs = new Documents();
        public bool useDatabaseFiles = false;
        bool readPermission = false;
        private Button btnAddData;
        Hashtable menuItemsPermissions;
        private Label lblDocDesc;
        private RichTextBox rtbDescription;
        private Label lblEmplID;
        private TextBox tbEmployeeID;
        private Label lblFirstName;
        private Label lblLastName;
        private TextBox tbFirstName;
        private TextBox tbLastName;
        bool writeDataToTag = false;
        public int DocumentID
        {
            get { return _documentID; }
            set { _documentID = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
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
        public byte[] Document
        {
            get { return _document; }
            set { _document = value; }
        }

        public String Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        

		public DocumentDownload(int documentID, string firstName, string lastName, string docName, string docDesc, byte[] document, string extension)
		{
			InitializeComponent();
			this.CenterToScreen();

			// Init Debug 
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            this.DocumentID = documentID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DocumentName = docName;
            this.DocumentDesc = docDesc;
            this.Document = document;
            this.Extension = extension;

			rm = new ResourceManager("UI.Resource",typeof(Employees).Assembly);
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			//populateStatusCombo();
			//populateWorkingGroupCombo();            
          //  cbGroup.SelectedValue = Constants.defaultWorkingGroupID;
		//	populateAccessGroupCombo();
          //  cbAccessGroup.SelectedValue = Constants.defaultAccessGroupID;
            docArray = new List<DocumentsTO>();
			currentDoc = new DocumentsTO();
			types = new Hashtable();
			logInUser = NotificationController.GetLogInUser();
            timeSchemaChanged = false;

            int databaseCount = eif.SearchCount(-1);
            if (databaseCount >= 0)
                useDatabaseFiles = true;

            List<WorkingUnitTO> wuList = new WorkingUnit().Search();

            foreach (WorkingUnitTO wuTO in wuList)
            {
                if (!wUnitsDic.ContainsKey(wuTO.WorkingUnitID))
                    wUnitsDic.Add(wuTO.WorkingUnitID, wuTO);
                else
                    wUnitsDic[wuTO.WorkingUnitID] = wuTO;
            }

			if (documentID == -1)
			{
				// Init Add Form
			//	btnUpdate.Visible = false;
				btnTag.Visible = false;
				btnTimeSchedule.Visible = false;
                btnAddData.Visible = true;
                btnSave.Visible = true;
                //cbStatus.SelectedIndex = cbStatus.FindStringExact(Constants.statusActive);
                //cbStatus.Enabled = false;
              //  cbEmployee.Enabled = false;
                tbDocumentName.Text = docName;
                rtbDescription.Text = docDesc;
                //tbLastName.Text = lastName;
                
              //  this.type = type;
			}
			else
			{
				btnSave.Visible = true;
                btnClose.Visible = true;
				// Init Update Form
			//	currentDoc =  new Documents().Find(employeeID.ToString());

				// Fill boxes with employee's data
				tbEmployeeID.Text = documentID.ToString();
				tbEmployeeID.Enabled = false;
                tbFirstName.Text = firstName.ToString();
                tbFirstName.Enabled = false;
                tbLastName.Text = lastName.ToString();
                tbLastName.Enabled = false;
                tbDocumentName.Text = docName.ToString();
                tbDocumentName.Enabled = false;
				rtbDescription.Text = docDesc.ToString();
                rtbDescription.Enabled = false;
				
				

				//cbAccessGroup.SelectedValue = currentDoc.AccessGroupID;

                //if (currentDoc.AddressID != -1)
                //{
                //    tbAddressID.Text = currentDoc.AddressID.ToString();
                //}

				if (document.Equals(null))
				{
                    
                  MessageBox.Show(rm.GetString("emplPhotoOpen", culture));
  
				}

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                setVisibility();
			}
			
			setLanguage();

			//this.cbWorkingUnitID.DropDownStyle = ComboBoxStyle.DropDownList;
			//this.cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                btnTimeSchedule.Visible = false;
		}

        private void populateWorkingUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                //cbWorkingUnitID.DataSource = wuArray;
                //cbWorkingUnitID.DisplayMember = "Name";
                //cbWorkingUnitID.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

       


        private void setLanguage()
        {
            try
            {
                if (currentDoc.DocumentsID.Equals(-1))
                {
                    this.Text = rm.GetString("addDocument", culture);
                }
                else
                {
                    this.Text = rm.GetString("updateEmployee", culture);
                }

                groupBox.Text = rm.GetString("gbDocuments", culture);

                btnSave.Text = rm.GetString("btnSave", culture);
            //    btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnTag.Text = rm.GetString("btnTag", culture);
              
                btnTimeSchedule.Text = rm.GetString("btnTimeSchedule", culture);
                btnBrowse.Text = rm.GetString("btnBrowse", culture);
                btnAddData.Text = rm.GetString("btnAddData", culture);

                lblEmplID.Text = rm.GetString("menuEmployees", culture);
                lblDocDesc.Text = rm.GetString("lblFirstName", culture);
               
            //    lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                lblDocName.Text = rm.GetString("lblDocName", culture);
                lblDocDesc.Text = rm.GetString("lblDocDesc", culture);
           //     lblInputWarning.Text = rm.GetString("lblInputWarning", culture);
             //   lblGroup.Text = rm.GetString("lblGroup", culture);
                lblDocument.Text = rm.GetString("lblDocument", culture);
             //   lblType.Text = rm.GetString("lblType", culture);
            //    lblAccessGroup.Text = rm.GetString("lblAccessGroup", culture);
           //     lblOrganizationalUnit.Text = rm.GetString("lblOrganizationalUnit", culture);
           //     lblEmployeeTypeID.Text = rm.GetString("lblEmployeeTypeID", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }
        //if operator doesn't have privilege to read AccessControl disable tag and access group chageing 
        private void setVisibility()
        {
            int permission;
            List<ApplRoleTO> currentRoles = NotificationController.GetCurrentRoles();
            string menuItemID = "";

            menuItemID = rm.GetString("menuConfiguration", culture) + "_" + rm.GetString("menuAccessControl", culture);

            foreach (ApplRoleTO role in currentRoles)
            {
                permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
            }

         //   btnTag.Enabled = cbAccessGroup.Enabled = readPermission;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        //private void clearResources()
        //{
            //if (pbPicture.Image != null)
            //{
            //    pbPicture.Image.Dispose();
            //    pbPicture.Image = null;
            //}
      //  }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tbDocumentName = new System.Windows.Forms.TextBox();
            this.lblDocName = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.btnAddData = new System.Windows.Forms.Button();
            this.btnTag = new System.Windows.Forms.Button();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.tbEmployeeID = new System.Windows.Forms.TextBox();
            this.btnTimeSchedule = new System.Windows.Forms.Button();
            this.lblEmplID = new System.Windows.Forms.Label();
            this.lblDocDesc = new System.Windows.Forms.Label();
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbDocument = new System.Windows.Forms.TextBox();
            this.lblDocument = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDocumentName
            // 
            this.tbDocumentName.Location = new System.Drawing.Point(124, 155);
            this.tbDocumentName.MaxLength = 10;
            this.tbDocumentName.Name = "tbDocumentName";
            this.tbDocumentName.Size = new System.Drawing.Size(224, 20);
            this.tbDocumentName.TabIndex = 21;
            // 
            // lblDocName
            // 
            this.lblDocName.Location = new System.Drawing.Point(20, 153);
            this.lblDocName.Name = "lblDocName";
            this.lblDocName.Size = new System.Drawing.Size(100, 23);
            this.lblDocName.TabIndex = 20;
            this.lblDocName.Text = "Document name:";
            this.lblDocName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.lblFirstName);
            this.groupBox.Controls.Add(this.lblLastName);
            this.groupBox.Controls.Add(this.tbFirstName);
            this.groupBox.Controls.Add(this.btnAddData);
            this.groupBox.Controls.Add(this.btnTag);
            this.groupBox.Controls.Add(this.tbLastName);
            this.groupBox.Controls.Add(this.tbEmployeeID);
            this.groupBox.Controls.Add(this.btnTimeSchedule);
            this.groupBox.Controls.Add(this.lblEmplID);
            this.groupBox.Controls.Add(this.lblDocDesc);
            this.groupBox.Controls.Add(this.rtbDescription);
            this.groupBox.Controls.Add(this.btnBrowse);
            this.groupBox.Controls.Add(this.tbDocument);
            this.groupBox.Controls.Add(this.lblDocument);
            this.groupBox.Controls.Add(this.tbDocumentName);
            this.groupBox.Controls.Add(this.lblDocName);
            this.groupBox.Location = new System.Drawing.Point(16, 16);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(520, 511);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Documents";
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(20, 81);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(100, 23);
            this.lblFirstName.TabIndex = 55;
            this.lblFirstName.Text = "First name:";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(22, 117);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(100, 23);
            this.lblLastName.TabIndex = 54;
            this.lblLastName.Text = "Last name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(124, 81);
            this.tbFirstName.MaxLength = 10;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(224, 20);
            this.tbFirstName.TabIndex = 53;
            // 
            // btnAddData
            // 
            this.btnAddData.Location = new System.Drawing.Point(370, 56);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(144, 23);
            this.btnAddData.TabIndex = 41;
            this.btnAddData.Text = "Additional data >>";
            this.btnAddData.Visible = false;
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(370, 215);
            this.btnTag.Name = "btnTag";
            this.btnTag.Size = new System.Drawing.Size(120, 23);
            this.btnTag.TabIndex = 40;
            this.btnTag.Text = "Tags >>>";
            this.btnTag.Visible = false;
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(124, 119);
            this.tbLastName.MaxLength = 10;
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(224, 20);
            this.tbLastName.TabIndex = 52;
            // 
            // tbEmployeeID
            // 
            this.tbEmployeeID.Location = new System.Drawing.Point(128, 43);
            this.tbEmployeeID.MaxLength = 10;
            this.tbEmployeeID.Name = "tbEmployeeID";
            this.tbEmployeeID.Size = new System.Drawing.Size(220, 20);
            this.tbEmployeeID.TabIndex = 51;
            // 
            // btnTimeSchedule
            // 
            this.btnTimeSchedule.Location = new System.Drawing.Point(370, 152);
            this.btnTimeSchedule.Name = "btnTimeSchedule";
            this.btnTimeSchedule.Size = new System.Drawing.Size(144, 23);
            this.btnTimeSchedule.TabIndex = 42;
            this.btnTimeSchedule.Text = "Time Schedule assigning";
            this.btnTimeSchedule.Visible = false;
            // 
            // lblEmplID
            // 
            this.lblEmplID.Location = new System.Drawing.Point(22, 40);
            this.lblEmplID.Name = "lblEmplID";
            this.lblEmplID.Size = new System.Drawing.Size(100, 23);
            this.lblEmplID.TabIndex = 50;
            this.lblEmplID.Text = "Employee ID:";
            this.lblEmplID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDocDesc
            // 
            this.lblDocDesc.Location = new System.Drawing.Point(-1, 199);
            this.lblDocDesc.Name = "lblDocDesc";
            this.lblDocDesc.Size = new System.Drawing.Size(123, 23);
            this.lblDocDesc.TabIndex = 43;
            this.lblDocDesc.Text = "Document description:";
            this.lblDocDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbDescription
            // 
            this.rtbDescription.Location = new System.Drawing.Point(124, 201);
            this.rtbDescription.MaxLength = 132;
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.Size = new System.Drawing.Size(224, 186);
            this.rtbDescription.TabIndex = 44;
          //  this.rtbDescription.Text = global::UI.Resource_sr.applUserXOUSaved;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(354, 430);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(144, 23);
            this.btnBrowse.TabIndex = 33;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbDocument
            // 
            this.tbDocument.Enabled = false;
            this.tbDocument.Location = new System.Drawing.Point(124, 432);
            this.tbDocument.Name = "tbDocument";
            this.tbDocument.Size = new System.Drawing.Size(224, 20);
            this.tbDocument.TabIndex = 32;
            // 
            // lblDocument
            // 
            this.lblDocument.Location = new System.Drawing.Point(16, 430);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(104, 23);
            this.lblDocument.TabIndex = 31;
            this.lblDocument.Text = "Document:";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(461, 533);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 533);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(228, 533);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DocumentDownload
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(550, 568);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(552, 550);
            this.Name = "DocumentDownload";
            this.ShowInTaskbar = false;
            this.Text = "Preuzimanje dokumenata";
            this.Load += new System.EventHandler(this.DocumentDownload_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

    
		private void btnSave_Click(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
            Documents doc = new Documents();
        //    this.progressBarDump.Value = 0;
            docArray = new Documents().Search();


            try
            {
                if (this.tbDocument.Text.Equals(""))
                    MessageBox.Show(rm.GetString("selFolder", culture));
                else
                {

                    string docString = "";
                    DocumentsTO docForSel = new DocumentsTO();
                    docForSel.DocumentsID = int.Parse(tbEmployeeID.Text.Trim());
                    docForSel.FirstName = tbFirstName.Text.ToString();
                    docForSel.LastName = tbLastName.Text.ToString();
                    docForSel.DocName = tbDocumentName.Text.ToString();
                    if (docForSel.DocumentsID == -1)
                    {
                        foreach (DocumentsTO documArray in docArray)
                        {
                            docString += documArray.DocumentsID + ", ";
                        }
                        if (docString.Length > 0)
                        {
                            docString = docString.Substring(0, docString.Length - 2);
                        }
                    }
                    else
                    {
                        docString = docForSel.DocumentsID.ToString();
                    }


                    ArrayList al = doc.SearchArray(docForSel.DocumentsID.ToString());

                    if (al.Count > 0)
                    {
                     //   this.progressBarDump.Maximum = al.Count;

                        foreach (Documents emplDocFile in al)
                        {
                            if ((tbDocumentName.Text.Trim() == emplDocFile.DocumentName)
                                             && (rtbDescription.Text.Trim() == emplDocFile.DocumentDesc)
                                             && (tbEmployeeID.Text.Trim() == emplDocFile.DocumentsID.ToString())
                                             && (tbFirstName.Text.Trim() == emplDocFile.FirstName)
                                && (tbLastName.Text.Trim() == emplDocFile.LastName)
                                             )
                            {
                                byte[] emplDoc = emplDocFile.Document;

                                MemoryStream memStream = new MemoryStream(emplDoc);

                                // Set the position to the beginning of the stream.
                                memStream.Seek(0, SeekOrigin.Begin);

                                
                                //Boris, za preuzimanje fajlova, 20170124
                                using (Stream file = File.OpenWrite(tbDocument.Text + "\\" + emplDocFile.DocumentName + "." + emplDocFile.Extension))
                                {
                                    file.Write(emplDoc, 0, emplDoc.Length);
                                }
                               
                                memStream.Close();
                                MessageBox.Show(rm.GetString("messageDownloadDocuments", culture));
                                //this.progressBarDump.Value++;
                                //this.lblProgressStatus.Text = this.progressBarDump.Value + "/" + this.progressBarDump.Maximum;
                                //this.lblProgressStatus.Refresh();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noPhotos", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeDocMaintenance.btnStart_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            //this.Cursor = Cursors.WaitCursor;
            //Documents doc = new Documents();
            //try
            //{
                

               
            //    currentDoc.DocName = tbDocumentName.Text.Trim();
            //    currentDoc.DocDesc = rtbDescription.Text.Trim();

            //    string sourceFilePath = tbDocument.Text;
            //    if (!useDatabaseFiles)
            //    {
            //        if (!tbDocument.Text.Equals(""))
            //        {
            //            currentDoc.Picture = savePhoto(sourceFilePath, currentDoc.DocumentsID);
            //        }
            //        else 
            //        {
            //            throw new Exception(rm.GetString("messageDocumentFileNotInserted", culture));
            //        }
            //    }
            //    else
            //    {
            //        if (!tbDocument.Text.Equals(""))
            //        {
            //            string newFileName = currentDoc.DocumentsID.ToString().Trim() +
            //                Path.GetExtension(sourceFilePath);

            //            currentDoc.Picture = newFileName;
            //        } //if (!tbPicture.Text.Equals(""))
            //        else
            //        {
            //            throw new Exception(rm.GetString("messageDocumentFileNotInserted", culture));
            //        }
            //    } //if (useDatabaseFiles)


            //     if (!tbDocument.Text.Equals(""))
            //            {
            //                bool imageInserted = false;
            //                FileStream FilStr = new FileStream(sourceFilePath, FileMode.Open);
            //                if (FilStr != null)
            //                {
            //                    BinaryReader BinRed = new BinaryReader(FilStr);

            //                    byte[] imgbyte = new byte[FilStr.Length + 1];

            //                    // Here you use ReadBytes method to add a byte array of the image stream.
            //                    //so the image column will hold a byte array.
            //                    imgbyte = BinRed.ReadBytes(Convert.ToInt32(BinRed.BaseStream.Length));

            //                    BinRed.Close();
            //                    FilStr.Close();
                               
            //                    ArrayList al = eif.Search(currentDoc.DocumentsID);
            //                    if (al.Count > 0)
            //                    {
            //                        imageInserted = eif.Update(currentDoc.DocumentsID, imgbyte, true);
            //                    }
            //                    else
            //                    {
            //                        int insertedCount = documComm.Save(currentDoc.DocumentsID, 
            //                            currentDoc.FirstName,currentDoc.LastName, currentDoc.DocName,
            //                            currentDoc.DocDesc, imgbyte, true);
            //                        if (insertedCount > 0)
            //                            imageInserted = true;
            //                    }
            //                } //if (FilStr != null)
            //                else
            //                {
            //                    throw new Exception(rm.GetString("messageDocumentFileNotInserted", culture));
            //                }

            //                if (!imageInserted)
            //                    MessageBox.Show(rm.GetString("photoNotSaved", culture));

            //            } //if (!tbPicture.Text.Equals(""))
            //         //if (useDatabaseFiles)
            //        DialogResult result = MessageBox.Show(rm.GetString("messageEmployeeInserted", culture), "", MessageBoxButtons.YesNo);

            //        if (result == DialogResult.Yes)
            //        {
            //          //  this.cbWorkingUnitID.Text = "";
            //          //  this.cbEmployee.Text = "";
            //            this.tbDocumentName.Text = "";
            //            this.rtbDescription.Text = "";
            //            this.tbDocument.Text = "";
            //            this.tbDocument.Text = "";
                        
            //            currentDoc = new DocumentsTO();
            //        }
            //        else
            //        {
            //            this.Close();
            //        }

            //}

            //catch (SqlException sqlex)
            //{
            //    if (doc.GetTransaction() != null)
            //    {
            //        doc.RollbackTransaction();
            //    }
            //    if (sqlex.Number == 2627)
            //    {
            //        MessageBox.Show(rm.GetString("messageEmployeeIDexists", culture));
            //    }
            //    else
            //    {
            //        MessageBox.Show(sqlex.Message);
            //        log.writeLog(DateTime.Now + " EmployeesAdd.btnSave_Click(): " + sqlex.Message + "\n");
            //    }
            //}
            //catch (MySqlException mysqlex)
            //{
            //    if (doc.GetTransaction() != null)
            //    {
            //        doc.RollbackTransaction();
            //    }
            //    if (mysqlex.Number == 1062)
            //    {
            //        MessageBox.Show(rm.GetString("messageEmployeeIDexists", culture));
            //    }
            //    else
            //    {
            //        MessageBox.Show(mysqlex.Message);
            //        log.writeLog(DateTime.Now + " EmployeesAdd.btnSave_Click(): " + mysqlex.Message + "\n");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (doc.GetTransaction() != null)
            //    {
            //        doc.RollbackTransaction();
            //    }
            //    log.writeLog(DateTime.Now + " EmployeesAdd.btnSave_Click(): " + ex.Message + "\n");
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    this.Cursor = Cursors.Arrow;
            //}
		}

        private string savePhoto(string sourceFilePath, int employeeID)
        {
            string newFileName = "";
            try
            {
                newFileName = employeeID.ToString().Trim() + Path.GetExtension(sourceFilePath);
                string newFilePath = Constants.EmployeePhotoDirectory;

                if (!Directory.Exists(newFilePath))
                {
                    Directory.CreateDirectory(newFilePath);
                }

                newFilePath += "\\" + newFileName;

                if (!sourceFilePath.Equals(newFilePath))
                {
                    File.Copy(sourceFilePath, newFilePath, true);
                }
            }
            catch (Exception ex)
            {
                newFileName = "";
                log.writeLog(DateTime.Now + " EmployeesAdd.savePhoto(): " + ex.Message + "\n");
                throw ex;
            }

            return newFileName;
        }

        private void deletePhoto(string fileName)
        {
            try
            {
                string path = Constants.EmployeePhotoDirectory;

                if (Directory.Exists(path))
                {
                    File.Delete(path + "\\" + fileName);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.deletePhoto(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public void InitialiseObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
        }

        private void DocumentDownload_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                InitialiseObserverClient();

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateWorkingUnitCombo();
                //populateEmployeeCombo(-1);
                

                

                Common.Rule rule = new Common.Rule();
                rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                rule.RuleTO.RuleType = Constants.RuleWrittingDataToTag;
                List<RuleTO> rules = rule.Search();
                if (rules.Count > 0 && rules[0].RuleValue == Constants.yesInt)
                    writeDataToTag = true;

                //string costumer = Common.Misc.getCustomer(null);
                //int cost = 0;
                //bool costum = int.TryParse(costumer, out cost);
                //if (cost == (int)Constants.Customers.FIAT)
                //{
                //    tbAddressID.Enabled = tbEmployeeID.Enabled = tbFirstName.Enabled = tbLastName.Enabled = tbPassword.Enabled = tbPicture.Enabled = cbAccessGroup.Enabled = cbEmployeeType.Enabled =
                //    cbGroup.Enabled = cbStatus.Enabled = cbType.Enabled = cbWorkingUnitID.Enabled = btnAddData.Enabled = btnAddress.Enabled = btnBrowse.Enabled =
                //    btnClear.Enabled = btnOUTree.Enabled = btnSave.Enabled = btnTag.Enabled = btnUpdate.Enabled = btnWUTree.Enabled = false;                   
                //}
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.Employees_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void cbWorkingUnitID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    //if (cbWorkingUnitID.SelectedIndex != 0)
                    //{
                    //    if (wu.WorkingUnitID == (int)cbWorkingUnitID.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                    //    {
                    //        //if (!chbHierarhicly.Checked)
                    //        //{
                    //        //    chbHierarhicly.Checked = true;
                    //            check = true;
                    //      //  }
                    //    }
                    //}
                }

                //if (cbWorkingUnitID.SelectedValue is int && !check)
                //{
                //    populateEmployeeCombo((int)cbWorkingUnitID.SelectedValue);
                //}
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                FolderBrowserDialog fbDialog = new FolderBrowserDialog();

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.tbDocument.Text = fbDialog.SelectedPath.ToString();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeDocMaintenance.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Do not reload calling form on cancel
                doReloadOnBack = false;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DocumentDownload.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DocumentDownload.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

       
	}
}
