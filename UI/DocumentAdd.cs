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
using System.Text;

namespace UI
{
	/// <summary>
	/// Form for adding new and updating existing Employee data 
	/// </summary>
	public class DocumentAdd : System.Windows.Forms.Form
	{

        private int _documentID = -1;
        private byte[] _document = null;
        private string _FirstName = "";
        private string _LastName = "";
        private DateTime _modifiedTime = new DateTime(0);
        private string _documentName = "";
        private string _documentDesc = "";
        private string _extension = "";


		private System.Windows.Forms.ComboBox cbWorkingUnitID;
        private System.Windows.Forms.TextBox tbDocumentName;
        private System.Windows.Forms.Label lblDocName;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button btnTag;
		private System.Windows.Forms.Button btnUpdate;

		private CultureInfo culture;
		// Current Employee
		private DocumentsTO currentDoc = null;
		DebugLog log;
		ResourceManager rm;
		ApplUserTO logInUser;

		private Hashtable types;
        List<DocumentsTO> docArray;
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
        List<EmployeeTO> currentEmplArray;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnTimeSchedule;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.Button btnClear;
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
        private Button btnWUTree;
        public bool useDatabaseFiles = false;
        bool readPermission = false;
        private Button btnAddData;
        Hashtable menuItemsPermissions;
        private Label lblDocDesc;
        private RichTextBox rtbDescription;
        private ComboBox cbEmployee;
        private Label lblEmployee;
        private Label lblInputWarning;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
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

        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        

		public DocumentAdd(int documentID, string firstName, string lastName, string docName, string docDesc, byte[] document, string extension)
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
			currentDoc = new DocumentsTO();
			types = new Hashtable();
			logInUser = NotificationController.GetLogInUser();
            timeSchemaChanged = false;
            docArray = new List<DocumentsTO>();
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
				btnUpdate.Visible = false;
				btnTag.Visible = false;
				btnTimeSchedule.Visible = false;
                btnAddData.Visible = false;
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
				btnSave.Visible = false;
				// Init Update Form
			//	currentDoc =  new Documents().Find(employeeID.ToString());

				// Fill boxes with employee's data
				//tbEmployeeID.Text = currentDoc.EmployeeID.ToString();
				//tbEmployeeID.Enabled = false;

				rtbDescription.Text = currentDoc.FirstName;
				//tbLastName.Text = currentDoc.LastName;
				//cbStatus.SelectedIndex = cbStatus.FindStringExact(currentDoc.Status);
				//cbGroup.SelectedValue = currentDoc.WorkingGroupID;
				tbDocumentName.Text = currentDoc.DocName;

				//cbAccessGroup.SelectedValue = currentDoc.AccessGroupID;

                //if (currentDoc.AddressID != -1)
                //{
                //    tbAddressID.Text = currentDoc.AddressID.ToString();
                //}

				if (!currentDoc.Picture.Equals(""))
				{
                    if (!useDatabaseFiles)
                    {
                        tbDocument.Text = Constants.EmployeePhotoDirectory + "\\" + currentDoc.Picture;
                        try
                        {
                            //pbPicture.Image = new Bitmap(tbPicture.Text);
                        }
                        catch (Exception ex)
                        {
                            log.writeLog(DateTime.Now + " EmployeesAdd.EmployeesAdd(): " + ex.Message + "\n");
                            MessageBox.Show(rm.GetString("emplPhotoOpen", culture));
                        }
                    }
                    else
                    {
                        //ArrayList al = eif.Search(currentDoc.DocumentsID);
                        //if (al.Count > 0)
                        //{
                        //    byte[] emplPhoto = ((EmployeeImageFile)al[0]).Picture;

                        //    MemoryStream memStream = new MemoryStream(emplPhoto);

                        //    // Set the position to the beginning of the stream.
                        //    memStream.Seek(0, SeekOrigin.Begin);

                        //    //pbPicture.Image = new Bitmap(memStream);

                        //    memStream.Close();
                        //}
                        //else
                            MessageBox.Show(rm.GetString("emplPhotoOpen", culture));
                    }
				}
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                setVisibility();
			}
			
			setLanguage();

			this.cbWorkingUnitID.DropDownStyle = ComboBoxStyle.DropDownList;
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

                cbWorkingUnitID.DataSource = wuArray;
                cbWorkingUnitID.DisplayMember = "Name";
                cbWorkingUnitID.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                currentEmplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    currentEmplArray = new Employee().SearchByWU(wuString);
                }
                else
                {
                    
                    
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnitID.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }
                    
                    currentEmplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in currentEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmplArray.Insert(0, empl);
               
                cbEmployee.DataSource = currentEmplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnTag.Text = rm.GetString("btnTag", culture);
              
                btnTimeSchedule.Text = rm.GetString("btnTimeSchedule", culture);
                btnBrowse.Text = rm.GetString("btnBrowse", culture);
                btnAddData.Text = rm.GetString("btnAddData", culture);

                lblEmployee.Text = rm.GetString("menuEmployees", culture);
                lblDocDesc.Text = rm.GetString("lblFirstName", culture);
               
                lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                lblDocName.Text = rm.GetString("lblDocName", culture);
                lblDocDesc.Text = rm.GetString("lblDocDesc", culture);
                lblInputWarning.Text = rm.GetString("lblInputWarning", culture);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentAdd));
            this.cbWorkingUnitID = new System.Windows.Forms.ComboBox();
            this.tbDocumentName = new System.Windows.Forms.TextBox();
            this.lblDocName = new System.Windows.Forms.Label();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.lblInputWarning = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblDocDesc = new System.Windows.Forms.Label();
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            this.btnAddData = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbDocument = new System.Windows.Forms.TextBox();
            this.lblDocument = new System.Windows.Forms.Label();
            this.btnTimeSchedule = new System.Windows.Forms.Button();
            this.btnTag = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbWorkingUnitID
            // 
            this.cbWorkingUnitID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnitID.ItemHeight = 13;
            this.cbWorkingUnitID.Location = new System.Drawing.Point(128, 39);
            this.cbWorkingUnitID.Name = "cbWorkingUnitID";
            this.cbWorkingUnitID.Size = new System.Drawing.Size(216, 21);
            this.cbWorkingUnitID.TabIndex = 10;
            this.cbWorkingUnitID.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnitID_SelectedIndexChanged);
            // 
            // tbDocumentName
            // 
            this.tbDocumentName.Location = new System.Drawing.Point(128, 122);
            this.tbDocumentName.MaxLength = 10;
            this.tbDocumentName.Name = "tbDocumentName";
            this.tbDocumentName.Size = new System.Drawing.Size(220, 20);
            this.tbDocumentName.TabIndex = 21;
            // 
            // lblDocName
            // 
            this.lblDocName.Location = new System.Drawing.Point(22, 119);
            this.lblDocName.Name = "lblDocName";
            this.lblDocName.Size = new System.Drawing.Size(100, 23);
            this.lblDocName.TabIndex = 20;
            this.lblDocName.Text = "Document name:";
            this.lblDocName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(20, 37);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(100, 23);
            this.lblWorkingUnit.TabIndex = 9;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.lblInputWarning);
            this.groupBox.Controls.Add(this.label5);
            this.groupBox.Controls.Add(this.label4);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.cbEmployee);
            this.groupBox.Controls.Add(this.lblEmployee);
            this.groupBox.Controls.Add(this.lblDocDesc);
            this.groupBox.Controls.Add(this.rtbDescription);
            this.groupBox.Controls.Add(this.btnAddData);
            this.groupBox.Controls.Add(this.btnWUTree);
            this.groupBox.Controls.Add(this.btnBrowse);
            this.groupBox.Controls.Add(this.tbDocument);
            this.groupBox.Controls.Add(this.lblDocument);
            this.groupBox.Controls.Add(this.btnTimeSchedule);
            this.groupBox.Controls.Add(this.btnTag);
            this.groupBox.Controls.Add(this.cbWorkingUnitID);
            this.groupBox.Controls.Add(this.tbDocumentName);
            this.groupBox.Controls.Add(this.lblWorkingUnit);
            this.groupBox.Controls.Add(this.lblDocName);
            this.groupBox.Location = new System.Drawing.Point(16, 16);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(520, 511);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Documents";
            // 
            // lblInputWarning
            // 
            this.lblInputWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputWarning.Location = new System.Drawing.Point(16, 450);
            this.lblInputWarning.Name = "lblInputWarning";
            this.lblInputWarning.Size = new System.Drawing.Size(202, 42);
            this.lblInputWarning.TabIndex = 57;
            this.lblInputWarning.Text = "Field can not be empty";
            this.lblInputWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(6, 457);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 56;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(350, 367);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 55;
            this.label4.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(356, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 54;
            this.label3.Text = "*";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(356, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 53;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(356, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 52;
            this.label1.Text = "*";
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.ItemHeight = 13;
            this.cbEmployee.Location = new System.Drawing.Point(128, 81);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(216, 21);
            this.cbEmployee.TabIndex = 51;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(16, 79);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(100, 23);
            this.lblEmployee.TabIndex = 50;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDocDesc
            // 
            this.lblDocDesc.Location = new System.Drawing.Point(-1, 166);
            this.lblDocDesc.Name = "lblDocDesc";
            this.lblDocDesc.Size = new System.Drawing.Size(123, 23);
            this.lblDocDesc.TabIndex = 43;
            this.lblDocDesc.Text = "Document description:";
            this.lblDocDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbDescription
            // 
            this.rtbDescription.Location = new System.Drawing.Point(128, 168);
            this.rtbDescription.MaxLength = 132;
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.Size = new System.Drawing.Size(220, 186);
            this.rtbDescription.TabIndex = 44;
          //  this.rtbDescription.Text = global::UI.Resource_sr.applUserXOUSaved;
            // 
            // btnAddData
            // 
            this.btnAddData.Location = new System.Drawing.Point(224, 469);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(144, 23);
            this.btnAddData.TabIndex = 41;
            this.btnAddData.Text = "Additional data >>";
            this.btnAddData.Visible = false;
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(360, 37);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 12;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(370, 372);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(144, 23);
            this.btnBrowse.TabIndex = 33;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbDocument
            // 
            this.tbDocument.Enabled = false;
            this.tbDocument.Location = new System.Drawing.Point(128, 372);
            this.tbDocument.Name = "tbDocument";
            this.tbDocument.Size = new System.Drawing.Size(216, 20);
            this.tbDocument.TabIndex = 32;
            // 
            // lblDocument
            // 
            this.lblDocument.Location = new System.Drawing.Point(16, 370);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(104, 23);
            this.lblDocument.TabIndex = 31;
            this.lblDocument.Text = "Document:";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTimeSchedule
            // 
            this.btnTimeSchedule.Location = new System.Drawing.Point(370, 469);
            this.btnTimeSchedule.Name = "btnTimeSchedule";
            this.btnTimeSchedule.Size = new System.Drawing.Size(144, 23);
            this.btnTimeSchedule.TabIndex = 42;
            this.btnTimeSchedule.Text = "Time Schedule assigning";
            this.btnTimeSchedule.Visible = false;
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(224, 431);
            this.btnTag.Name = "btnTag";
            this.btnTag.Size = new System.Drawing.Size(120, 23);
            this.btnTag.TabIndex = 40;
            this.btnTag.Text = "Tags >>>";
            this.btnTag.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(445, 533);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 533);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
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
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(364, 533);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // DocumentAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(550, 568);
            this.ControlBox = false;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(552, 550);
            this.Name = "DocumentAdd";
            this.ShowInTaskbar = false;
            this.Text = "Dodavanje dokumenata";
            this.Load += new System.EventHandler(this.DocumentAdd_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

    
		private void btnSave_Click(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
            Documents doc = new Documents();
            string ext = "";
            string firstNameCheck = "";
            string lastNameCheck = "";
            try
            {
                if (cbEmployee.SelectedIndex <= 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeNotSelected", culture));
                }
                if (this.tbDocumentName.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageDocumentNameNotInserted", culture));
                }
                if (this.rtbDescription.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageDocumentDescNotInserted", culture));
                }
               
                currentEmplArray = new Employee().SearchByWU(cbWorkingUnitID.SelectedValue.ToString());
              
                foreach (EmployeeTO employee in currentEmplArray)
                {
                    if(this.cbEmployee.SelectedValue.ToString().Equals(employee.EmployeeID.ToString()))
                    {
                    currentDoc.FirstName = employee.FirstName;
                    currentDoc.LastName = employee.LastName;
                    }
                    firstNameCheck = currentDoc.FirstName;
                    lastNameCheck = currentDoc.LastName;
                }
                currentDoc.DocumentsID = Int32.Parse(this.cbEmployee.SelectedValue.ToString());

               
                currentDoc.DocName = tbDocumentName.Text.Trim();
                currentDoc.DocDesc = rtbDescription.Text.Trim();

                string sourceFilePath = tbDocument.Text;
                
                if (!useDatabaseFiles)
                {
                    if (!tbDocument.Text.Equals(""))
                    {
                        currentDoc.Picture = savePhoto(sourceFilePath, currentDoc.DocumentsID);
                    }
                    else 
                    {
                        throw new Exception(rm.GetString("messageDocumentFileNotInserted", culture));
                    }
                }
                else
                {
                    if (!tbDocument.Text.Equals(""))
                    {
                        string newFileName = currentDoc.DocumentsID.ToString().Trim() +
                            Path.GetExtension(sourceFilePath);
                        
        
                int l = sourceFilePath.IndexOf(".");
               

        if (l >0)
        {
           ext = sourceFilePath.Substring(sourceFilePath.LastIndexOf(".") + 1);
        }
       

    
                        currentDoc.Picture = Encoding.ASCII.GetBytes(newFileName);
                    } //if (!tbPicture.Text.Equals(""))
                    else
                    {
                        throw new Exception(rm.GetString("messageDocumentFileNotInserted", culture));
                    }
                } //if (useDatabaseFiles)


                 if (!tbDocument.Text.Equals(""))
                        {
                            bool imageInserted = false;
                            FileStream FilStr = new FileStream(sourceFilePath, FileMode.Open);
                            if (FilStr != null)
                            {
                                BinaryReader BinRed = new BinaryReader(FilStr);

                                byte[] imgbyte = new byte[FilStr.Length + 1];

                                // Here you use ReadBytes method to add a byte array of the image stream.
                                //so the image column will hold a byte array.
                                imgbyte = BinRed.ReadBytes(Convert.ToInt32(BinRed.BaseStream.Length));

                                BinRed.Close();
                                FilStr.Close();
                               
                              //  ArrayList al = eif.Search(currentDoc.DocumentsID);
                                //if (al.Count > 0)
                                //{
                                //    imageInserted = eif.Update(currentDoc.DocumentsID, imgbyte, true);
                                //}
                               // else
                              //  {
                                docArray = new Documents().Search();

                                foreach (DocumentsTO docs in docArray) 
                                {
                                    if ((tbDocumentName.Text.Trim() == docs.DocName)
                                             && (rtbDescription.Text.Trim() == docs.DocDesc)
                                             && firstNameCheck == docs.FirstName
                                             && lastNameCheck == docs.LastName
                                             )
                                    {
                                        throw new Exception(rm.GetString("messageDuplicateInsertForDocument", culture));
                                    }
                                }
 
                                    int insertedCount = documComm.Save(currentDoc.DocumentsID, 
                                        currentDoc.FirstName,currentDoc.LastName, currentDoc.DocName,
                                        currentDoc.DocDesc, imgbyte, ext, true);
                                    if (insertedCount > 0)
                                        imageInserted = true;
                            //    }
                            } //if (FilStr != null)
                            else
                            {
                                throw new Exception(rm.GetString("messageDocumentFileNotInserted", culture));
                            }

                            if (!imageInserted)
                                MessageBox.Show(rm.GetString("photoNotSaved", culture));

                        } //if (!tbPicture.Text.Equals(""))
                     //if (useDatabaseFiles)

                 currentDoc.Extension = ext;

                    DialogResult result = MessageBox.Show(rm.GetString("messageEmployeeInserted", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        this.cbWorkingUnitID.Text = "";
                        this.cbEmployee.Text = "";
                        this.tbDocumentName.Text = "";
                        this.rtbDescription.Text = "";
                        this.tbDocument.Text = "";
                        this.tbDocument.Text = "";
                        
                        currentDoc = new DocumentsTO();
                    }
                    else
                    {
                        this.Close();
                    }

            }

            catch (SqlException sqlex)
            {
                if (doc.GetTransaction() != null)
                {
                    doc.RollbackTransaction();
                }
                if (sqlex.Number == 2627)
                {
                    MessageBox.Show(rm.GetString("messageEmployeeIDexists", culture));
                }
                else
                {
                    MessageBox.Show(sqlex.Message);
                    log.writeLog(DateTime.Now + " EmployeesAdd.btnSave_Click(): " + sqlex.Message + "\n");
                }
            }
            catch (MySqlException mysqlex)
            {
                if (doc.GetTransaction() != null)
                {
                    doc.RollbackTransaction();
                }
                if (mysqlex.Number == 1062)
                {
                    MessageBox.Show(rm.GetString("messageEmployeeIDexists", culture));
                }
                else
                {
                    MessageBox.Show(mysqlex.Message);
                    log.writeLog(DateTime.Now + " EmployeesAdd.btnSave_Click(): " + mysqlex.Message + "\n");
                }
            }
            catch (Exception ex)
            {
                if (doc.GetTransaction() != null)
                {
                    doc.RollbackTransaction();
                }
                log.writeLog(DateTime.Now + " EmployeesAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private byte[] savePhoto(string sourceFilePath, int employeeID)
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

            return Encoding.ASCII.GetBytes(newFileName);
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

        private void DocumentAdd_Load(object sender, EventArgs e)
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
                populateEmployeeCombo(-1);
                

                

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
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWorkingUnitID.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWorkingUnitID.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            //if (!chbHierarhicly.Checked)
                            //{
                            //    chbHierarhicly.Checked = true;
                                check = true;
                          //  }
                        }
                    }
                }

                if (cbWorkingUnitID.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWorkingUnitID.SelectedValue);
                }
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

                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = rm.GetString("browsePhoto", culture);
                dlg.Filter =
                    "bmp files (*.bmp)|*.bmp"
                    + "|gif files (*.gif)|*.gif"
                    + "|jpg files (*.jpg)|*.jpg"
                    + "|jpeg files (*.jpeg)|*.jpeg"
                    + "|doc files (*.doc)|*.doc"
                    + "|docx files (*.docx)|*.docx"
                    + "|xls files (*.xls)|*.xls"
                    + "|xlsx files (*.xlsx)|*.xlsx"
                    + "|pdf files (*.pdf)|*.pdf"
                    + "|png files (*.png)|*.png";

                //Za sve  "All files (*.*)|*.*|All files (*.*)|*.*";
                dlg.FilterIndex = 9;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(dlg.FileName);
                    if (fi.Length > 10435760)
                    {
                        MessageBox.Show(rm.GetString("imgLarge", culture));
                    }
                    else
                    {
                        this.tbDocument.Text = dlg.FileName;
                        //if (pbPicture.Image != null)
                        //    pbPicture.Image.Dispose();
                        //this.pbPicture.Image = new Bitmap(dlg.FileName);
                    }
                }

                dlg.Dispose();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DocumentAdd.btnBrowse_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " DocumentAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
              
                this.tbDocumentName.Text = "";
                this.rtbDescription.Text = "";
                this.cbWorkingUnitID.SelectedIndex = 0;
                this.cbEmployee.SelectedIndex = 0;
 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DocumentAdd.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

       
	}
}
