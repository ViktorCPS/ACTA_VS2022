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
	public class EmployeeAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox cbWorkingUnitID;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.TextBox tbLastName;
		private System.Windows.Forms.TextBox tbFirstName;
		private System.Windows.Forms.TextBox tbEmployeeID;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.Label lblEmployeeID;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.ComboBox cbStatus;
		private System.Windows.Forms.TextBox tbAddressID;
		private System.Windows.Forms.Label lblAddressID;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnTag;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnAddress;
		private System.Windows.Forms.Button btnUpdate;

		private CultureInfo culture;
		// Current Employee
		private EmployeeTO currentEmployee = null;
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

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblGroup;
		private System.Windows.Forms.ComboBox cbGroup;
		private System.Windows.Forms.Button btnTimeSchedule;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Label lblPicture;
		private System.Windows.Forms.TextBox tbPicture;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.PictureBox pbPicture;
		private System.Windows.Forms.ComboBox cbType;
		private System.Windows.Forms.Label lblType;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblAccessGroup;
		private System.Windows.Forms.ComboBox cbAccessGroup;
		private System.Windows.Forms.Label label8;

		private string oldStatus = "";
        private bool timeSchemaChanged;
        private bool tagChanged;

		//Indicate if calling form needs to be reload
		public bool doReloadOnBack = true;

        EmployeeImageFile eif = new EmployeeImageFile();
        private Button btnWUTree;
        public bool useDatabaseFiles = false;
        bool readPermission = false;
        private Button btnAddData;
        private Label label9;
        private ComboBox cbOrganizationalUnit;
        private Label lblOrganizationalUnit;
        private Button btnOUTree;
        private Label label10;
        private ComboBox cbEmployeeType;
        private Label lblEmployeeTypeID;
        Hashtable menuItemsPermissions;
        bool writeDataToTag = false;

        bool cbFirstChange = false;//NATALIJA08112017

		public EmployeeAdd(int employeeID, string firstName, string lastName, string wu, string type, bool hasTag)
		{
			InitializeComponent();
			this.CenterToScreen();

			// Init Debug 
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			rm = new ResourceManager("UI.Resource",typeof(Employees).Assembly);
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			populateStatusCombo();
			populateWorkingGroupCombo();            
            cbGroup.SelectedValue = Constants.defaultWorkingGroupID;
			populateAccessGroupCombo();
            cbAccessGroup.SelectedValue = Constants.defaultAccessGroupID;
			currentEmployee = new EmployeeTO();
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

			if (employeeID == -1)
			{
				// Init Add Form
				btnUpdate.Visible = false;
				btnTag.Visible = false;
				btnTimeSchedule.Visible = false;
                btnAddData.Visible = false;
				cbStatus.SelectedIndex = cbStatus.FindStringExact(Constants.statusActive);
				cbStatus.Enabled = true;
                tbFirstName.Text = firstName;
                tbLastName.Text = lastName;
                this.wu = wu;
                this.type = type;
			}
			else
			{
				btnSave.Visible = false;
				// Init Update Form
				currentEmployee =  new Employee().Find(employeeID.ToString());
				currentEmployee.HasTag = hasTag;
				oldStatus = currentEmployee.Status.Trim();
                oldGroupID = currentEmployee.WorkingGroupID;

				// Fill boxes with employee's data
				tbEmployeeID.Text = currentEmployee.EmployeeID.ToString();
				tbEmployeeID.Enabled = false;

				tbFirstName.Text = currentEmployee.FirstName;
				tbLastName.Text = currentEmployee.LastName;
				cbStatus.SelectedIndex = cbStatus.FindStringExact(currentEmployee.Status);
				cbGroup.SelectedValue = currentEmployee.WorkingGroupID;
				tbPassword.Text = currentEmployee.Password;

				cbAccessGroup.SelectedValue = currentEmployee.AccessGroupID;

				if (currentEmployee.AddressID != -1)
				{
					tbAddressID.Text = currentEmployee.AddressID.ToString();
				}

				if (!currentEmployee.Picture.Equals(""))
				{
                    if (!useDatabaseFiles)
                    {
                        tbPicture.Text = Constants.EmployeePhotoDirectory + "\\" + currentEmployee.Picture;
                        try
                        {
                            pbPicture.Image = new Bitmap(tbPicture.Text);
                        }
                        catch (Exception ex)
                        {
                            log.writeLog(DateTime.Now + " EmployeesAdd.EmployeesAdd(): " + ex.Message + "\n");
                            MessageBox.Show(rm.GetString("emplPhotoOpen", culture));
                        }
                    }
                    else
                    {
                        ArrayList al = eif.Search(currentEmployee.EmployeeID);
                        if (al.Count > 0)
                        {
                            byte[] emplPhoto = ((EmployeeImageFile)al[0]).Picture;

                            MemoryStream memStream = new MemoryStream(emplPhoto);

                            // Set the position to the beginning of the stream.
                            memStream.Seek(0, SeekOrigin.Begin);

                            pbPicture.Image = new Bitmap(memStream);

                            memStream.Close();
                        }
                        else
                            MessageBox.Show(rm.GetString("emplPhotoOpen", culture));
                    }
				}
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                setVisibility();
			}
			
			setLanguage();

			this.cbWorkingUnitID.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                btnTimeSchedule.Visible = false;
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

            btnTag.Enabled = cbAccessGroup.Enabled = readPermission;
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

        private void clearResources()
        {
            if (pbPicture.Image != null)
            {
                pbPicture.Image.Dispose();
                pbPicture.Image = null;
            }
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeAdd));
            this.cbWorkingUnitID = new System.Windows.Forms.ComboBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.tbEmployeeID = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblEmployeeID = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbEmployeeType = new System.Windows.Forms.ComboBox();
            this.lblEmployeeTypeID = new System.Windows.Forms.Label();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cbOrganizationalUnit = new System.Windows.Forms.ComboBox();
            this.lblOrganizationalUnit = new System.Windows.Forms.Label();
            this.btnAddData = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cbAccessGroup = new System.Windows.Forms.ComboBox();
            this.lblAccessGroup = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.pbPicture = new System.Windows.Forms.PictureBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbPicture = new System.Windows.Forms.TextBox();
            this.lblPicture = new System.Windows.Forms.Label();
            this.btnTimeSchedule = new System.Windows.Forms.Button();
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.lblGroup = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.tbAddressID = new System.Windows.Forms.TextBox();
            this.lblAddressID = new System.Windows.Forms.Label();
            this.btnAddress = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTag = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // cbWorkingUnitID
            // 
            this.cbWorkingUnitID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnitID.ItemHeight = 13;
            this.cbWorkingUnitID.Location = new System.Drawing.Point(128, 120);
            this.cbWorkingUnitID.Name = "cbWorkingUnitID";
            this.cbWorkingUnitID.Size = new System.Drawing.Size(216, 21);
            this.cbWorkingUnitID.TabIndex = 10;
            this.cbWorkingUnitID.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnitID_SelectedIndexChanged);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(128, 217);
            this.tbPassword.MaxLength = 10;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(216, 20);
            this.tbPassword.TabIndex = 21;
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(128, 88);
            this.tbLastName.MaxLength = 50;
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(216, 20);
            this.tbLastName.TabIndex = 7;
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(128, 56);
            this.tbFirstName.MaxLength = 50;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(216, 20);
            this.tbFirstName.TabIndex = 4;
            // 
            // tbEmployeeID
            // 
            this.tbEmployeeID.Location = new System.Drawing.Point(128, 24);
            this.tbEmployeeID.MaxLength = 10;
            this.tbEmployeeID.Name = "tbEmployeeID";
            this.tbEmployeeID.Size = new System.Drawing.Size(112, 20);
            this.tbEmployeeID.TabIndex = 1;
            this.tbEmployeeID.Leave += new System.EventHandler(this.tbEmployeeID_Leave);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(16, 217);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(100, 23);
            this.lblPassword.TabIndex = 20;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(32, 185);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(80, 23);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 120);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(100, 23);
            this.lblWorkingUnit.TabIndex = 9;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(16, 88);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(100, 23);
            this.lblLastName.TabIndex = 6;
            this.lblLastName.Text = "Last Name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(16, 56);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(100, 23);
            this.lblFirstName.TabIndex = 3;
            this.lblFirstName.Text = "First Name:";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployeeID
            // 
            this.lblEmployeeID.Location = new System.Drawing.Point(16, 24);
            this.lblEmployeeID.Name = "lblEmployeeID";
            this.lblEmployeeID.Size = new System.Drawing.Size(100, 23);
            this.lblEmployeeID.TabIndex = 0;
            this.lblEmployeeID.Text = "Employee ID:";
            this.lblEmployeeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.label10);
            this.groupBox.Controls.Add(this.cbEmployeeType);
            this.groupBox.Controls.Add(this.lblEmployeeTypeID);
            this.groupBox.Controls.Add(this.btnOUTree);
            this.groupBox.Controls.Add(this.label9);
            this.groupBox.Controls.Add(this.cbOrganizationalUnit);
            this.groupBox.Controls.Add(this.lblOrganizationalUnit);
            this.groupBox.Controls.Add(this.btnAddData);
            this.groupBox.Controls.Add(this.btnWUTree);
            this.groupBox.Controls.Add(this.label8);
            this.groupBox.Controls.Add(this.cbAccessGroup);
            this.groupBox.Controls.Add(this.lblAccessGroup);
            this.groupBox.Controls.Add(this.label7);
            this.groupBox.Controls.Add(this.cbType);
            this.groupBox.Controls.Add(this.lblType);
            this.groupBox.Controls.Add(this.pbPicture);
            this.groupBox.Controls.Add(this.btnBrowse);
            this.groupBox.Controls.Add(this.tbPicture);
            this.groupBox.Controls.Add(this.lblPicture);
            this.groupBox.Controls.Add(this.btnTimeSchedule);
            this.groupBox.Controls.Add(this.cbGroup);
            this.groupBox.Controls.Add(this.lblGroup);
            this.groupBox.Controls.Add(this.label6);
            this.groupBox.Controls.Add(this.label5);
            this.groupBox.Controls.Add(this.label4);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.cbStatus);
            this.groupBox.Controls.Add(this.tbAddressID);
            this.groupBox.Controls.Add(this.lblAddressID);
            this.groupBox.Controls.Add(this.btnAddress);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Controls.Add(this.btnTag);
            this.groupBox.Controls.Add(this.tbEmployeeID);
            this.groupBox.Controls.Add(this.tbFirstName);
            this.groupBox.Controls.Add(this.tbLastName);
            this.groupBox.Controls.Add(this.cbWorkingUnitID);
            this.groupBox.Controls.Add(this.tbPassword);
            this.groupBox.Controls.Add(this.lblEmployeeID);
            this.groupBox.Controls.Add(this.lblFirstName);
            this.groupBox.Controls.Add(this.lblLastName);
            this.groupBox.Controls.Add(this.lblWorkingUnit);
            this.groupBox.Controls.Add(this.lblStatus);
            this.groupBox.Controls.Add(this.lblPassword);
            this.groupBox.Location = new System.Drawing.Point(16, 16);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(520, 511);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Employee";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(352, 404);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 16);
            this.label10.TabIndex = 39;
            this.label10.Text = "*";
            // 
            // cbEmployeeType
            // 
            this.cbEmployeeType.Location = new System.Drawing.Point(128, 404);
            this.cbEmployeeType.Name = "cbEmployeeType";
            this.cbEmployeeType.Size = new System.Drawing.Size(216, 21);
            this.cbEmployeeType.TabIndex = 38;
            // 
            // lblEmployeeTypeID
            // 
            this.lblEmployeeTypeID.Location = new System.Drawing.Point(16, 404);
            this.lblEmployeeTypeID.Name = "lblEmployeeTypeID";
            this.lblEmployeeTypeID.Size = new System.Drawing.Size(104, 23);
            this.lblEmployeeTypeID.TabIndex = 37;
            this.lblEmployeeTypeID.Text = "Employee type:";
            this.lblEmployeeTypeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOUTree
            // 
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(372, 149);
            this.btnOUTree.Name = "btnOUTree";
            this.btnOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree.TabIndex = 16;
            this.btnOUTree.Click += new System.EventHandler(this.btnOUTree_Click);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(352, 151);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 16);
            this.label9.TabIndex = 15;
            this.label9.Text = "*";
            // 
            // cbOrganizationalUnit
            // 
            this.cbOrganizationalUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrganizationalUnit.ItemHeight = 13;
            this.cbOrganizationalUnit.Location = new System.Drawing.Point(128, 151);
            this.cbOrganizationalUnit.Name = "cbOrganizationalUnit";
            this.cbOrganizationalUnit.Size = new System.Drawing.Size(216, 21);
            this.cbOrganizationalUnit.TabIndex = 14;
            // 
            // lblOrganizationalUnit
            // 
            this.lblOrganizationalUnit.Location = new System.Drawing.Point(16, 151);
            this.lblOrganizationalUnit.Name = "lblOrganizationalUnit";
            this.lblOrganizationalUnit.Size = new System.Drawing.Size(100, 23);
            this.lblOrganizationalUnit.TabIndex = 13;
            this.lblOrganizationalUnit.Text = "Organizational unit:";
            this.lblOrganizationalUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAddData
            // 
            this.btnAddData.Location = new System.Drawing.Point(200, 469);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(144, 23);
            this.btnAddData.TabIndex = 41;
            this.btnAddData.Text = "Additional data >>";
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(372, 118);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 12;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(352, 281);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 16);
            this.label8.TabIndex = 27;
            this.label8.Text = "*";
            // 
            // cbAccessGroup
            // 
            this.cbAccessGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccessGroup.ItemHeight = 13;
            this.cbAccessGroup.Location = new System.Drawing.Point(128, 281);
            this.cbAccessGroup.Name = "cbAccessGroup";
            this.cbAccessGroup.Size = new System.Drawing.Size(216, 21);
            this.cbAccessGroup.TabIndex = 26;
            // 
            // lblAccessGroup
            // 
            this.lblAccessGroup.Location = new System.Drawing.Point(16, 281);
            this.lblAccessGroup.Name = "lblAccessGroup";
            this.lblAccessGroup.Size = new System.Drawing.Size(100, 23);
            this.lblAccessGroup.TabIndex = 25;
            this.lblAccessGroup.Text = "Access group:";
            this.lblAccessGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(352, 377);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 16);
            this.label7.TabIndex = 36;
            this.label7.Text = "*";
            // 
            // cbType
            // 
            this.cbType.Location = new System.Drawing.Point(128, 377);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(216, 21);
            this.cbType.TabIndex = 35;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(16, 377);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(104, 23);
            this.lblType.TabIndex = 34;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbPicture
            // 
            this.pbPicture.Location = new System.Drawing.Point(405, 65);
            this.pbPicture.Name = "pbPicture";
            this.pbPicture.Size = new System.Drawing.Size(90, 135);
            this.pbPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPicture.TabIndex = 35;
            this.pbPicture.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(360, 343);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(144, 23);
            this.btnBrowse.TabIndex = 33;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbPicture
            // 
            this.tbPicture.Enabled = false;
            this.tbPicture.Location = new System.Drawing.Point(128, 345);
            this.tbPicture.Name = "tbPicture";
            this.tbPicture.Size = new System.Drawing.Size(216, 20);
            this.tbPicture.TabIndex = 32;
            // 
            // lblPicture
            // 
            this.lblPicture.Location = new System.Drawing.Point(16, 345);
            this.lblPicture.Name = "lblPicture";
            this.lblPicture.Size = new System.Drawing.Size(104, 23);
            this.lblPicture.TabIndex = 31;
            this.lblPicture.Text = "Picture:";
            this.lblPicture.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTimeSchedule
            // 
            this.btnTimeSchedule.Location = new System.Drawing.Point(360, 469);
            this.btnTimeSchedule.Name = "btnTimeSchedule";
            this.btnTimeSchedule.Size = new System.Drawing.Size(144, 23);
            this.btnTimeSchedule.TabIndex = 42;
            this.btnTimeSchedule.Text = "Time Schedule assigning";
            this.btnTimeSchedule.Click += new System.EventHandler(this.btnTimeSchedule_Click);
            // 
            // cbGroup
            // 
            this.cbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroup.ItemHeight = 13;
            this.cbGroup.Location = new System.Drawing.Point(128, 249);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(216, 21);
            this.cbGroup.TabIndex = 24;
            // 
            // lblGroup
            // 
            this.lblGroup.Location = new System.Drawing.Point(16, 249);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(100, 23);
            this.lblGroup.TabIndex = 23;
            this.lblGroup.Text = "Working Group:";
            this.lblGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(352, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "*";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(352, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 19;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(352, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "*";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(352, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(352, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "*";
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.ItemHeight = 13;
            this.cbStatus.Location = new System.Drawing.Point(128, 185);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(216, 21);
            this.cbStatus.TabIndex = 18;
            // 
            // tbAddressID
            // 
            this.tbAddressID.Enabled = false;
            this.tbAddressID.Location = new System.Drawing.Point(128, 313);
            this.tbAddressID.Name = "tbAddressID";
            this.tbAddressID.ReadOnly = true;
            this.tbAddressID.Size = new System.Drawing.Size(64, 20);
            this.tbAddressID.TabIndex = 29;
            // 
            // lblAddressID
            // 
            this.lblAddressID.Location = new System.Drawing.Point(16, 313);
            this.lblAddressID.Name = "lblAddressID";
            this.lblAddressID.Size = new System.Drawing.Size(100, 23);
            this.lblAddressID.TabIndex = 28;
            this.lblAddressID.Text = "Address ID:";
            this.lblAddressID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAddress
            // 
            this.btnAddress.Location = new System.Drawing.Point(224, 313);
            this.btnAddress.Name = "btnAddress";
            this.btnAddress.Size = new System.Drawing.Size(120, 23);
            this.btnAddress.TabIndex = 30;
            this.btnAddress.Text = "Address   >>>";
            this.btnAddress.Click += new System.EventHandler(this.btnAddress_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(248, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "*";
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(224, 431);
            this.btnTag.Name = "btnTag";
            this.btnTag.Size = new System.Drawing.Size(120, 23);
            this.btnTag.TabIndex = 40;
            this.btnTag.Text = "Tags >>>";
            this.btnTag.Click += new System.EventHandler(this.btnTag_Click);
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
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
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
            // EmployeeAdd
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
            this.Name = "EmployeeAdd";
            this.ShowInTaskbar = false;
            this.Text = "New Employee";
            this.Load += new System.EventHandler(this.EmployeeAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeeAdd_KeyUp);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPicture)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void btnAddress_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int addressID = tbAddressID.Text.Equals("") ? 0 : Int32.Parse(tbAddressID.Text.Trim());
                Addresses addrForm = new Addresses(addressID, tbLastName.Text + " " + tbFirstName.Text);
                addrForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.btnAddress_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
           
		}

		private void btnTag_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				Tags tags = new Tags();
				EmployeeTO employee = new Employee().Find(tbEmployeeID.Text.Trim());
			
				if (employee.EmployeeID > 0)
				{
					if (employee.Status.Equals(Constants.statusBlocked))
					{
						MessageBox.Show(rm.GetString("emplBlockedNoTags", culture));
					}
					else
					{
						TagTO tag = new Employee().FindActive(Int32.Parse(employee.EmployeeID.ToString()));
						if (tag.RecordID >= 0)
						{
							tags.setEmployeeName(employee.FirstName + " " + employee.LastName);
							tags.setOwnerID(employee.EmployeeID);

                            tags.setStatus("CHANGE", writeDataToTag);	
                            			
							tags.ShowDialog(this);
							//MessageBox.Show(messageTagNew1);
						}
						else
						{
							DialogResult result = MessageBox.Show(rm.GetString("messageActiveTag", culture), "", MessageBoxButtons.YesNo);
							if (result == DialogResult.Yes)
							{
								tags.setEmployeeName(employee.FirstName + " " + employee.LastName);
								tags.setOwnerID(employee.EmployeeID);

                                tags.setStatus("CHANGE", writeDataToTag);

								tags.ShowDialog(this);
							}
							//this.ClearForm();
							//this.newLoad();
						}
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("tagEmplNotFound", culture));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesAdd.btnTag_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
			Employee empl = new Employee();
            try
            {
                if (this.tbEmployeeID.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageEmployeeIDNotSet", culture));
                }
                if (this.tbFirstName.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageEmployeeFirstNameNotSet", culture));
                }
                if (this.tbLastName.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageEmployeeLastNameNotSet", culture));
                }
                if (cbWorkingUnitID.SelectedIndex <= 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeWUNotSet", culture));
                }
                if (cbOrganizationalUnit.SelectedIndex <= 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeOUNotSet", culture));
                }
                if (this.cbStatus.SelectedIndex <= 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeStatusNotSet", culture));
                }
                //if (this.tbPassword.Text.Trim().Equals(""))
                //{
                //    throw new Exception(rm.GetString("messagePasswordNotSet", culture));
                //}
                try
                {
                    if (!tbEmployeeID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbEmployeeID.Text.Trim());
                    }
                }
                catch
                {
                    throw new Exception(rm.GetString("messageEmployeeIDMustBeNum", culture));
                }
                try
                {
                    if (!tbAddressID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbAddressID.Text.Trim());
                    }
                }
                catch
                {
                    throw new Exception(rm.GetString("messageEmployeeAddrMustBeNum", culture));
                }
                if (this.cbType.SelectedIndex < 0)
                {
                    throw new Exception(rm.GetString("messageTypeNotSet", culture));
                }                
                if (cbAccessGroup.SelectedIndex < 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeAGNotSet", culture));
                }

                currentEmployee.EmployeeID = Int32.Parse(tbEmployeeID.Text.Trim());
                currentEmployee.FirstName = tbFirstName.Text.Trim();
                currentEmployee.LastName = tbLastName.Text.Trim();
                currentEmployee.WorkingUnitID = Int32.Parse(this.cbWorkingUnitID.SelectedValue.ToString().Trim());
                currentEmployee.OrgUnitID = Int32.Parse(this.cbOrganizationalUnit.SelectedValue.ToString().Trim());
                currentEmployee.Status = cbStatus.SelectedValue.ToString().Trim();
                currentEmployee.Password = tbPassword.Text.Trim();
                if (cbGroup.SelectedIndex >= 0)
                {
                    currentEmployee.WorkingGroupID = (int)cbGroup.SelectedValue;
                }
                else
                {
                    currentEmployee.WorkingGroupID = Constants.defaultWorkingGroupID;
                }

                if (!tbAddressID.Text.Trim().Equals(""))
                {
                    currentEmployee.AddressID = Int32.Parse(tbAddressID.Text.Trim());
                }
                else
                {
                    currentEmployee.AddressID = 0;
                }

                string sourceFilePath = tbPicture.Text;
                if (!useDatabaseFiles)
                {
                    if (!tbPicture.Text.Equals(""))
                    {
                        currentEmployee.Picture = savePhoto(sourceFilePath, currentEmployee.EmployeeID);
                    }
                }
                else
                {
                    if (!tbPicture.Text.Equals(""))
                    {
                        string newFileName = currentEmployee.EmployeeID.ToString().Trim() +
                            Path.GetExtension(sourceFilePath);

                        currentEmployee.Picture = newFileName;
                    } //if (!tbPicture.Text.Equals(""))
                } //if (useDatabaseFiles)

                currentEmployee.Type = (string)types[cbType.SelectedItem.ToString()];
                currentEmployee.AccessGroupID = Int32.Parse(this.cbAccessGroup.SelectedValue.ToString().Trim());
                if (cbEmployeeType.SelectedValue != null && cbEmployeeType.SelectedValue is int)
                    currentEmployee.EmployeeTypeID = (int)cbEmployeeType.SelectedValue;
                
                // get all counter types
                List<EmployeeCounterTypeTO> counterTypes = new EmployeeCounterType().Search();

                bool isInserted = true;

                empl.BeginTransaction();
                empl.EmplTO = currentEmployee;
                int inserted = empl.Save(false);
                //int inserted = empl.Save(currentEmployee.EmployeeID.ToString().Trim(), currentEmployee.FirstName.Trim(),
                //    currentEmployee.LastName.Trim(), currentEmployee.WorkingUnitID.ToString().Trim(),
                //    currentEmployee.Status.Trim(), currentEmployee.Password.Trim(), currentEmployee.AddressID.ToString().Trim(),
                //    currentEmployee.Picture.Trim(), currentEmployee.WorkingGroupID.ToString().Trim(), currentEmployee.Type.Trim(),
                //    currentEmployee.AccessGroupID.ToString().Trim(), currentEmployee.OrgUnitID, false);

                isInserted = (inserted == 1) && isInserted;
                if (isInserted)
                {
                    EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule();
                    emplTimeSchedule.SetTransaction(empl.GetTransaction());

                    if (currentEmployee.WorkingGroupID >= 0)
                    {
                        DateTime today = DateTime.Now.Date;
                        List<EmployeeGroupsTimeScheduleTO> groupSchedules = new EmployeeGroupsTimeSchedule().SearchFromSchedules(
                            currentEmployee.WorkingGroupID, today, empl.GetTransaction());

                        if (groupSchedules.Count > 0)
                        {
                            List<WorkTimeSchemaTO> timeSchemas = new TimeSchema().Search(empl.GetTransaction());
                            int timeScheduleIndex = -1;
                            for (int scheduleIndex = 0; scheduleIndex < groupSchedules.Count; scheduleIndex++)
                            {
                                if (today.Date >= groupSchedules[scheduleIndex].Date)
                                {
                                    timeScheduleIndex = scheduleIndex;
                                }
                            }
                            if (timeScheduleIndex >= 0)
                            {
                                EmployeeGroupsTimeScheduleTO egts = groupSchedules[timeScheduleIndex];
                                int startDay = egts.StartCycleDay;
                                int schemaID = egts.TimeSchemaID;

                                WorkTimeSchemaTO actualTimeSchema = null;
                                foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
                                {
                                    if (currentTimeSchema.TimeSchemaID == schemaID)
                                    {
                                        actualTimeSchema = currentTimeSchema;
                                        break;
                                    }
                                }
                                if (actualTimeSchema != null)
                                {
                                    int cycleDuration = actualTimeSchema.CycleDuration;

                                    TimeSpan ts = new TimeSpan(today.Date.Ticks - egts.Date.Date.Ticks);
                                    int dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;

                                    int insert = emplTimeSchedule.Save(currentEmployee.EmployeeID, today.Date, schemaID, dayNum, "", false);
                                    isInserted = (insert > 0) && isInserted;

                                    for (int scheduleIndex = timeScheduleIndex + 1; scheduleIndex < groupSchedules.Count; scheduleIndex++)
                                    {
                                        egts = groupSchedules[scheduleIndex];

                                        insert = emplTimeSchedule.Save(currentEmployee.EmployeeID, egts.Date,
                                            egts.TimeSchemaID, egts.StartCycleDay, "", false);
                                        isInserted = (insert > 0) && isInserted;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isInserted = (emplTimeSchedule.Save(currentEmployee.EmployeeID, new DateTime(DateTime.Now.Year,
                            DateTime.Now.Month, 1), Constants.defaultSchemaID, Constants.defaultStartDay, "", false) > 0 ? true : false) && isInserted;
                        }
                    }
                    else
                    {
                        isInserted = (emplTimeSchedule.Save(currentEmployee.EmployeeID, new DateTime(DateTime.Now.Year,
                            DateTime.Now.Month, 1), Constants.defaultSchemaID, Constants.defaultStartDay, "", false) > 0 ? true : false) && isInserted;
                    }

                    if (isInserted)
                    {
                        EmployeeLocation emplLoc = new EmployeeLocation();
                        emplLoc.SetTransaction(empl.GetTransaction());
                        isInserted = (emplLoc.Save(currentEmployee.EmployeeID, -1, -1, -1, new DateTime(0), -1, false) > 0 ? true : false) && isInserted;
                    }

                    if (isInserted)
                    {
                        // insert hiring date and company
                        EmployeeAsco4 asco = new EmployeeAsco4();
                        asco.EmplAsco4TO.EmployeeID = currentEmployee.EmployeeID;
                        asco.EmplAsco4TO.IntegerValue4 = Common.Misc.getRootWorkingUnit(currentEmployee.WorkingUnitID, wUnitsDic);
                        asco.EmplAsco4TO.DatetimeValue2 = DateTime.Now.Date;
                        asco.SetTransaction(empl.GetTransaction());
                        isInserted = isInserted && asco.save(false);
                    }

                    if (isInserted)
                    {
                        EmployeeCounterValue emplCounter = new EmployeeCounterValue();
                        emplCounter.SetTransaction(empl.GetTransaction());
                        // create new counters for all counter type                        
                        foreach (EmployeeCounterTypeTO countType in counterTypes)
                        {
                            EmployeeCounterValueTO counter = new EmployeeCounterValueTO();
                            counter.EmplCounterTypeID = countType.EmplCounterTypeID;
                            counter.EmplID = currentEmployee.EmployeeID;
                            if (Constants.MeasureForCounterType.ContainsKey(countType.EmplCounterTypeID))
                                counter.MeasureUnit = Constants.MeasureForCounterType[countType.EmplCounterTypeID];
                            else
                                counter.MeasureUnit = Constants.emplCounterMesureDay;
                            counter.Value = 0;
                            
                            emplCounter.ValueTO = counter;
                            isInserted = isInserted && emplCounter.Save(false) > 0;

                            if (!isInserted)
                                break;
                        }
                    }

                    if (isInserted)
                    {
                        empl.CommitTransaction();
                    }
                    else
                    {
                        empl.RollbackTransaction();
                        MessageBox.Show(rm.GetString("messageEmployeeNotInserted", culture));
                        return;
                    }

                    Tags tags = new Tags();

                    tags.setEmployeeName(currentEmployee.FirstName + " " + currentEmployee.LastName);
                    tags.setOwnerID(currentEmployee.EmployeeID);
                    tags.setStatus("NEW", writeDataToTag);
                    tags.ShowDialog(this);

                    //currentEmployee.Clear();

                    //Bilja, 13.09.2007, not need any more. The same code is in this event, and in
                    //Save event when return from this form, so, everything was done twice
                    //this.Controller.EmpoliyeeChanged(true);

                    clearResources();

                    //need to be after clearResources(), for reading image
                    if (useDatabaseFiles)
                    {
                        if (!tbPicture.Text.Equals(""))
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

                                ArrayList al = eif.Search(currentEmployee.EmployeeID);
                                if (al.Count > 0)
                                {
                                    imageInserted = eif.Update(currentEmployee.EmployeeID, imgbyte, true);
                                }
                                else
                                {
                                    int insertedCount = eif.Save(currentEmployee.EmployeeID, imgbyte, true);
                                    if (insertedCount > 0)
                                        imageInserted = true;
                                }
                            } //if (FilStr != null)

                            if (!imageInserted)
                                MessageBox.Show(rm.GetString("photoNotSaved", culture));

                        } //if (!tbPicture.Text.Equals(""))
                    } //if (useDatabaseFiles)

                    if (!sourceFilePath.Equals(""))
                    {
                        this.pbPicture.Image = new Bitmap(sourceFilePath);
                    }

                    DialogResult result = MessageBox.Show(rm.GetString("messageEmployeeInserted", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        this.tbEmployeeID.Text = "";
                        this.tbFirstName.Text = "";
                        this.tbLastName.Text = "";
                        this.tbPassword.Text = "";
                        this.tbPicture.Text = "";
                        this.pbPicture.Image = null;
                        this.tbAddressID.Text = "";
                        this.tbEmployeeID.Focus();
                        currentEmployee = new EmployeeTO();
                    }
                    else
                    {
                        this.Close();
                    }

                } //if (isInserted)
            }
            catch (SqlException sqlex)
            {
                if (empl.GetTransaction() != null)
                {
                    empl.RollbackTransaction();
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
                if (empl.GetTransaction() != null)
                {
                    empl.RollbackTransaction();
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
                if (empl.GetTransaction() != null)
                {
                    empl.RollbackTransaction();
                }
                log.writeLog(DateTime.Now + " EmployeesAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
            Employee empl = new Employee();

            try
            {
                if (this.tbEmployeeID.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageEmployeeIDNotSet", culture));
                }
                if (this.tbFirstName.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageEmployeeFirstNameNotSet", culture));
                }
                if (this.tbLastName.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageEmployeeLastNameNotSet", culture));
                }
                if (this.cbWorkingUnitID.SelectedIndex == 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeWUNotSet", culture));
                }
                if (this.cbStatus.SelectedIndex <= 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeStatusNotSet", culture));
                }
                if (this.cbOrganizationalUnit.SelectedIndex == 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeOUNotSet", culture));
                }
                //if (this.tbPassword.Text.Trim().Equals(""))
                //{
                //    throw new Exception(rm.GetString("messagePasswordNotSet", culture));
                //}
                try
                {
                    if (!tbEmployeeID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbEmployeeID.Text.Trim());
                    }
                }
                catch
                {
                    throw new Exception(rm.GetString("messageEmployeeIDMustBeNum", culture));
                }
                if (this.cbType.SelectedIndex < 0)
                {
                    throw new Exception(rm.GetString("messageTypeNotSet", culture));
                }
                if (cbAccessGroup.SelectedIndex < 0)
                {
                    throw new Exception(rm.GetString("messageEmployeeAGNotSet", culture));
                }
                
                EmployeeHist emplHist = new EmployeeHist();
                emplHist.EmplHistTO = new EmployeeHistTO(currentEmployee);

                // get asco record
                EmployeeAsco4 asco = new EmployeeAsco4();
                EmployeeAsco4Hist ascoHist = new EmployeeAsco4Hist();
                asco.EmplAsco4TO.EmployeeID = currentEmployee.EmployeeID;
                List<EmployeeAsco4TO> ascoList = asco.Search();

                EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();

                if (ascoList.Count > 0)
                {
                    ascoTO = ascoList[0];
                    ascoHist.EmplAsco4TO = new EmployeeAsco4TO(ascoTO);
                }

                bool updateAsco = false;

                ApplUserTO applUserTOUpdate = new ApplUserTO();
                List<EmployeeTimeScheduleTO> emplTimeScheduleInsert = new List<EmployeeTimeScheduleTO>();
                List<EmployeeTimeScheduleTO> emplTimeSchedulesDel = new List<EmployeeTimeScheduleTO>();
                EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule();
                DateTime retiredDate = ascoTO.DatetimeValue3;
                bool reprocessDays = false;
                List<DateTime> dayToReprocess = new List<DateTime>();
                // if employee is retired
                if (currentEmployee.Status != Constants.statusRetired && cbStatus.SelectedValue.ToString().Trim() == Constants.statusRetired)
                {
                    // retire user if exists
                    if (ascoTO.NVarcharValue5 != "")
                    {
                        ApplUser applUser = new ApplUser();
                        applUser.UserTO.UserID = ascoTO.NVarcharValue5;
                        applUser.UserTO.Status = Constants.statusActive;
                        List<ApplUserTO> usersTO = applUser.Search();
                        if (usersTO.Count > 0)
                        {
                            applUserTOUpdate = usersTO[0];
                            applUserTOUpdate.Status = Constants.statusRetired;
                        }
                    }

                    Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = new TimeSchema().getDictionary();
                    if (retiredDate.Equals(new DateTime()))
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("noRetiredDate", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.No)
                            return;
                        else
                            retiredDate = DateTime.Now.Date;
                    }

                    //set time schedule to default and delete all io_pairs_processed   
                    // if last working day is entering third shift, set leaving third shift for first retired day and leave pairs for that day, and set default schema to second retired day
                    emplTimeSchedulesDel = emplTimeSchedule.SearchEmployeesSchedules(currentEmployee.EmployeeID.ToString(), retiredDate.Date, new DateTime());
                    bool firstRetiredDayThirdShiftLeaving = false;
                    WorkTimeSchemaTO retiredDaySchema = new WorkTimeSchemaTO();
                    // get intervals for last working day
                    Dictionary<int, List<EmployeeTimeScheduleTO>> lastWorkingSchList = emplTimeSchedule.SearchEmployeesSchedulesExactDate(currentEmployee.EmployeeID.ToString().Trim(),
                        retiredDate.Date.AddDays(-1), retiredDate.Date.AddDays(-1), null);

                    if (lastWorkingSchList.ContainsKey(currentEmployee.EmployeeID) && lastWorkingSchList[currentEmployee.EmployeeID].Count > 0)
                    {
                        List<WorkTimeIntervalTO> lastDayIntervals = Common.Misc.getTimeSchemaInterval(retiredDate.AddDays(-1), lastWorkingSchList[currentEmployee.EmployeeID], dictTimeSchema);

                        // check if last schedule has midnight ending interval                                        
                        foreach (WorkTimeIntervalTO intTO in lastDayIntervals)
                        {
                            if (intTO.EndTime.Hour == 23 && intTO.EndTime.Minute == 59 && intTO.EndTime.TimeOfDay.Subtract(intTO.StartTime.TimeOfDay).TotalMinutes > 0)
                            {
                                firstRetiredDayThirdShiftLeaving = true;
                                if (dictTimeSchema.ContainsKey(intTO.TimeSchemaID))
                                    retiredDaySchema = dictTimeSchema[intTO.TimeSchemaID];
                                break;
                            }
                        }
                    }

                    if (firstRetiredDayThirdShiftLeaving)
                    {
                        // get ending third shift day and insert that schedule into first retired day
                        // after that insert default schema
                        int startSchemaDay = -1;
                        foreach (int day in retiredDaySchema.Days.Keys)
                        {
                            if (retiredDaySchema.Days[day].Count == 1)
                            {
                                foreach (WorkTimeIntervalTO intTO in retiredDaySchema.Days[day].Values)
                                {
                                    if (intTO.StartTime.Hour == 0 && intTO.StartTime.Minute == 0 && intTO.EndTime.TimeOfDay.Subtract(intTO.StartTime.TimeOfDay).TotalMinutes > 0)
                                    {
                                        startSchemaDay = day;
                                        break;
                                    }
                                }
                            }

                            if (startSchemaDay != -1)
                                break;
                        }

                        if (retiredDaySchema.TimeSchemaID != -1 && startSchemaDay != -1)
                        {
                            EmployeeTimeScheduleTO etsRetiredInsert = new EmployeeTimeScheduleTO();
                            etsRetiredInsert.EmployeeID = currentEmployee.EmployeeID;
                            etsRetiredInsert.StartCycleDay = startSchemaDay;
                            etsRetiredInsert.Date = retiredDate.Date;
                            etsRetiredInsert.TimeSchemaID = retiredDaySchema.TimeSchemaID;
                            emplTimeScheduleInsert.Add(etsRetiredInsert);
                        }
                    }

                    EmployeeTimeScheduleTO etsInsert = new EmployeeTimeScheduleTO();
                    etsInsert.EmployeeID = currentEmployee.EmployeeID;
                    etsInsert.StartCycleDay = 0;
                    if (firstRetiredDayThirdShiftLeaving)
                        etsInsert.Date = retiredDate.Date.AddDays(1);
                    else
                        etsInsert.Date = retiredDate.Date;
                    etsInsert.TimeSchemaID = Constants.defaultSchemaID;
                    emplTimeScheduleInsert.Add(etsInsert);

                    DateTime endDate = new IOPairProcessed().getMaxDateOfPair(currentEmployee.EmployeeID.ToString(), null);
                    if (endDate == new DateTime())
                        endDate = DateTime.Now.Date;

                    List<DateTime> datesList = new List<DateTime>();
                    DateTime startReprocessDay = firstRetiredDayThirdShiftLeaving ? retiredDate.Date.AddDays(1) : retiredDate.Date;
                    for (DateTime dt = startReprocessDay; dt <= endDate; dt = dt.AddDays(1))
                    {
                        datesList.Add(dt);
                    }

                    if (endDate >= retiredDate.Date)
                    {
                        reprocessDays = true;
                        dayToReprocess = datesList;
                    }

                    if (ascoTO.EmployeeID != -1 && !ascoTO.DatetimeValue3.Date.Equals(retiredDate.Date))
                    {
                        ascoTO.DatetimeValue3 = retiredDate.Date;
                        updateAsco = true;
                    }
                }
                else if (currentEmployee.Status == Constants.statusRetired && cbStatus.SelectedValue.ToString().Trim() == Constants.statusActive)
                {
                    if (ascoTO.EmployeeID != -1)
                    {
                        ascoTO.DatetimeValue2 = DateTime.Now;
                        ascoTO.DatetimeValue3 = new DateTime();
                        updateAsco = true;
                    }
                }
                
                bool isUpdated = false;

                currentEmployee.EmployeeID = Int32.Parse(tbEmployeeID.Text.Trim());
                currentEmployee.FirstName = tbFirstName.Text.Trim();
                currentEmployee.LastName = tbLastName.Text.Trim();
                currentEmployee.WorkingUnitID = Int32.Parse(this.cbWorkingUnitID.SelectedValue.ToString().Trim());
                int company = Common.Misc.getRootWorkingUnit(currentEmployee.WorkingUnitID, wUnitsDic);
                if (ascoTO.EmployeeID != -1 && ascoTO.IntegerValue4 != company)
                {
                    ascoTO.IntegerValue4 = company;
                    updateAsco = true;
                }
                currentEmployee.OrgUnitID = Int32.Parse(this.cbOrganizationalUnit.SelectedValue.ToString().Trim());
                currentEmployee.Status = cbStatus.SelectedValue.ToString().Trim();
                currentEmployee.Password = tbPassword.Text.Trim();
                if (cbGroup.SelectedIndex >= 0)
                {
                    currentEmployee.WorkingGroupID = (int)cbGroup.SelectedValue;
                }
                else
                {
                    currentEmployee.WorkingGroupID = Constants.defaultWorkingGroupID;
                }

                if (!tbAddressID.Text.Trim().Equals(""))
                {
                    currentEmployee.AddressID = Int32.Parse(tbAddressID.Text.Trim());
                }
                else
                {
                    currentEmployee.AddressID = 0;
                }

                if (cbEmployeeType.SelectedValue != null && cbEmployeeType.SelectedValue is int)
                    currentEmployee.EmployeeTypeID = (int)cbEmployeeType.SelectedValue;

                string oldPicture = currentEmployee.Picture;
                string sourceFilePath = tbPicture.Text;
                if (!useDatabaseFiles)
                {
                    if (!tbPicture.Text.Equals(""))
                    {
                        currentEmployee.Picture = savePhoto(sourceFilePath, currentEmployee.EmployeeID);
                    }
                    else
                    {
                        currentEmployee.Picture = "";
                    }
                }
                else
                {
                    if (!tbPicture.Text.Equals(""))
                    {
                        string newFileName = currentEmployee.EmployeeID.ToString().Trim() +
                            Path.GetExtension(sourceFilePath);

                        currentEmployee.Picture = newFileName;
                    } //if (!tbPicture.Text.Equals(""))
                } //if (useDatabaseFiles)

                currentEmployee.Type = (string)types[cbType.SelectedItem.ToString()];
                currentEmployee.AccessGroupID = Int32.Parse(this.cbAccessGroup.SelectedValue.ToString().Trim());

                Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionary(currentEmployee.EmployeeID.ToString().Trim());
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(currentEmployee.EmployeeID.ToString().Trim());
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Common.Rule().SearchWUEmplTypeDictionary();
                
                if (empl.BeginTransaction())
                {
                    try
                    {
                        empl.EmplTO = currentEmployee;
                        //if (isUpdated = empl.Update(this.tbEmployeeID.Text.Trim(), this.tbFirstName.Text.Trim(),
                        //    this.tbLastName.Text.Trim(), this.cbWorkingUnitID.SelectedValue.ToString().Trim(),
                        //    this.cbStatus.SelectedValue.ToString().Trim(), this.tbPassword.Text.Trim(),
                        //    currentEmployee.AddressID.ToString().Trim(), currentEmployee.Picture.Trim(),
                        //    currentEmployee.WorkingGroupID.ToString().Trim(), currentEmployee.Type.Trim(),
                        //    currentEmployee.AccessGroupID.ToString().Trim(), currentEmployee.OrgUnitID, false))
                        if (isUpdated = empl.Update(false))
                        {
                            emplHist.SetTransaction(empl.GetTransaction());
                            emplHist.EmplHistTO.ValidTo = DateTime.Now.Date;

                            isUpdated = isUpdated && (emplHist.Save(false) > 0);

                            if (isUpdated)
                            {
                                if (oldGroupID != currentEmployee.WorkingGroupID)
                                {
                                    //NATALIJA08112017
                                    try
                                    {
                                        empl.CommitTransaction();
                                    }
                                    catch (Exception ex)
                                    {
                                        empl.RollbackTransaction();
                                    }
                                    
                                    if (!cbFirstChange)
                                    {
                                        this.Cursor = Cursors.WaitCursor;
                                        WTEmployeesTimeSchedule emplTimeSchedule1 = new WTEmployeesTimeSchedule(currentEmployee, oldGroupID);
                                        emplTimeSchedule1.ShowDialog(this);

                                    }
                                    timeSchemaChanged = true;
                                    empl.BeginTransaction();
                                    isUpdated = timeSchemaChanged && isUpdated;
                                    //n

                                    /*
                                    EmplWorkingGroupChanged emplWGroupChanged = new EmplWorkingGroupChanged(currentEmployee.WorkingGroupID, currentEmployee.EmployeeID, emplDict, 
                                        ascoDict, rulesDict, empl.GetTransaction());
                                    emplWGroupChanged.ShowDialog(this);
                                    isUpdated = this.timeSchemaChanged && isUpdated;
                                     * */
                                }

                                if (isUpdated)
                                {
                                    if (!oldStatus.Equals(currentEmployee.Status.Trim()) && currentEmployee.HasTag)
                                    {
                                        Tags tags = new Tags();

                                        if (currentEmployee.Status.Equals(Constants.statusBlocked))
                                        {
                                            tags.setStatus("BLOCKED", writeDataToTag);
                                        }
                                        else if (currentEmployee.Status.Equals(Constants.statusRetired))
                                        {
                                            tags.setStatus("RETIRED", writeDataToTag);
                                        }

                                        else if (currentEmployee.Status.Equals(Constants.statusActive))
                                        {
                                            tags.setStatus("ACTIVE", writeDataToTag);
                                        }

                                        tags.setEmployeeName(currentEmployee.FirstName + " " + currentEmployee.LastName);
                                        tags.setOwnerID(currentEmployee.EmployeeID);
                                        tags.setTransaction(empl.GetTransaction());
                                        tags.ShowDialog(this);

                                        isUpdated = tagChanged && isUpdated;
                                    }
                                }

                                if (isUpdated)
                                {
                                    // change asco if needed
                                    if (updateAsco)
                                    {
                                        ascoHist.SetTransaction(empl.GetTransaction());
                                        isUpdated = isUpdated && ascoHist.save(false);

                                        if (isUpdated)
                                        {
                                            asco.EmplAsco4TO = ascoTO;
                                            asco.SetTransaction(empl.GetTransaction());
                                            isUpdated = isUpdated && asco.update(false);
                                        }
                                    }
                                }

                                if (isUpdated)
                                {
                                    emplTimeSchedule.SetTransaction(empl.GetTransaction());
                                    // change schedule
                                    if (emplTimeSchedulesDel.Count > 0)
                                    {
                                        isUpdated = isUpdated && emplTimeSchedule.DeleteFromToSchedule(currentEmployee.EmployeeID, retiredDate.Date, new DateTime(), "", false);
                                    }

                                    if (isUpdated)
                                    {
                                        foreach (EmployeeTimeScheduleTO schIns in emplTimeScheduleInsert)
                                        {
                                            isUpdated = isUpdated && emplTimeSchedule.Save(schIns.EmployeeID, schIns.Date, schIns.TimeSchemaID, schIns.StartCycleDay, "", false) > 0;

                                            if (!isUpdated)
                                                break;
                                        }
                                    }
                                }

                                if (isUpdated)
                                {
                                    // reproces pairs if needed
                                    if (reprocessDays)
                                    {
                                        DateTime startDate = new DateTime();
                                        DateTime endDate = new DateTime();

                                        foreach (DateTime date in dayToReprocess)
                                        {
                                            if (startDate == new DateTime() || startDate > date)
                                            {
                                                startDate = date;
                                            }
                                            if (endDate == new DateTime() || endDate < date)
                                                endDate = date;
                                        }

                                        //list of datetime for each employee
                                        Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                                        emplDateWholeDayList.Add(currentEmployee.EmployeeID, dayToReprocess);

                                        if (dayToReprocess.Count > 0)
                                            isUpdated = isUpdated && Common.Misc.ReprocessPairsAndRecalculateCounters(currentEmployee.EmployeeID.ToString(), startDate.Date, endDate.Date, empl.GetTransaction(), emplDateWholeDayList, null, "");
                                    }
                                }

                                if (isUpdated)
                                {
                                    if (!applUserTOUpdate.UserID.Trim().Equals(""))
                                    {
                                        ApplUser user = new ApplUser();
                                        user.UserTO = applUserTOUpdate;
                                        user.SetTransaction(empl.GetTransaction());
                                        isUpdated = isUpdated && user.Update(false);
                                    }
                                }

                                if (isUpdated)
                                {
                                    empl.CommitTransaction();
                                    MessageBox.Show(rm.GetString("emplUpd", culture));
                                }
                                else
                                {
                                    if (empl.GetTransaction() != null)
                                        empl.RollbackTransaction();
                                    MessageBox.Show(rm.GetString("emplNotUpd", culture));
                                }

                                if (isUpdated)
                                {
                                    if (!useDatabaseFiles)
                                    {
                                        if (!oldPicture.Equals("") && currentEmployee.Picture.Equals(""))
                                        {
                                            deletePhoto(oldPicture);
                                        }
                                    }

                                    //currentEmployee.Clear();

                                    //Bilja, 13.09.2007, not need any more. The same code is in this event, and in
                                    //Update event when return from this form, so, everything was done twice
                                    //And with this, listview is reload before returning to Update event, and upd record is lost
                                    //this.Controller.EmpoliyeeChanged(true);

                                    clearResources();

                                    //need to be after clearResources(), for reading image
                                    if (useDatabaseFiles)
                                    {
                                        if (!tbPicture.Text.Equals(""))
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

                                                ArrayList al = eif.Search(currentEmployee.EmployeeID);
                                                if (al.Count > 0)
                                                {
                                                    imageInserted = eif.Update(currentEmployee.EmployeeID, imgbyte, true);
                                                }
                                                else
                                                {
                                                    int insertedCount = eif.Save(currentEmployee.EmployeeID, imgbyte, true);
                                                    if (insertedCount > 0)
                                                        imageInserted = true;
                                                }
                                            } //if (FilStr != null)

                                            if (!imageInserted)
                                                MessageBox.Show(rm.GetString("photoNotSaved", culture));

                                        } //if (!tbPicture.Text.Equals(""))
                                    } // //if (useDatabaseFiles)
                                } // if (isUpdated)

                                this.Close();
                            }
                        } //if (isUpdated)
                    }
                    catch (Exception ex)
                    {
                        if (empl.GetTransaction() != null)
                            empl.RollbackTransaction();
                        throw ex;
                    }
                }
                else
                    MessageBox.Show(rm.GetString("emplNotUpd", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void EmployeeAdd_Load(object sender, System.EventArgs e)
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

                if (currentEmployee.EmployeeID >= 0)
                {
                    cbWorkingUnitID.SelectedValue = currentEmployee.WorkingUnitID;
                }
                else
                {
                    if (!this.wu.Equals(""))
                    {
                        cbWorkingUnitID.SelectedIndex = cbWorkingUnitID.FindStringExact(this.wu);
                    }
                }

                OrganizationalUnitTO ouAll = new OrganizationalUnitTO();
                ouAll.Name = rm.GetString("all", culture);
                oUnitsList.Add(ouAll);
                foreach (int id in oUnits.Keys)
                {
                    ouString += id.ToString().Trim() + ",";                    
                    oUnitsList.Add(oUnits[id]);                    
                }

                if (ouString.Length > 0)
                    ouString = ouString.Substring(0, ouString.Length - 1);

                populateOrganizationalUnitCombo();

                if (currentEmployee.EmployeeID >= 0)
                {
                    cbOrganizationalUnit.SelectedValue = currentEmployee.OrgUnitID;
                }               

				types.Add(rm.GetString("emplOrdinary", culture), Constants.emplOrdinary);
				types.Add(rm.GetString("emplExtraOrdinary", culture), Constants.emplExtraOrdinary);
				types.Add(rm.GetString("emplSpecial", culture), Constants.emplSpecial);

				populateTypes();

                if (currentEmployee.EmployeeID >= 0)
                {
                    string emplType = "";
                    if (currentEmployee.Type.Equals(Constants.emplOrdinary))
                    {
                        emplType = rm.GetString("emplOrdinary", culture);
                    }
                    else if (currentEmployee.Type.Equals(Constants.emplExtraOrdinary))
                    {
                        emplType = rm.GetString("emplExtraOrdinary", culture);
                    }
                    else if (currentEmployee.Type.Equals(Constants.emplSpecial))
                    {
                        emplType = rm.GetString("emplSpecial", culture);
                    }
                    cbType.SelectedIndex = cbType.FindStringExact(emplType);
                }
                else
                {
                    if (!this.type.Equals(""))
                    {
                        cbType.SelectedIndex = cbType.FindStringExact(this.type);
                    }
                }

                if (currentEmployee.EmployeeTypeID != -1)
                    cbEmployeeType.SelectedValue = currentEmployee.EmployeeTypeID;

                Common.Rule rule = new Common.Rule();
                rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                rule.RuleTO.RuleType = Constants.RuleWrittingDataToTag;
                List<RuleTO> rules = rule.Search();
                if (rules.Count > 0 && rules[0].RuleValue == Constants.yesInt)
                    writeDataToTag = true;


                //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                /*
                tbEmployeeID.Enabled = false;
                tbFirstName.Enabled = false;
                tbLastName.Enabled = false;
                cbWorkingUnitID.Enabled = false;
                cbOrganizationalUnit.Enabled = false;
                cbStatus.Enabled = false;
                btnWUTree.Enabled = false;
                btnOUTree.Enabled = false;
                btnClear.Enabled = false;
                 */

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

        private void populateEmplTypeCombo(int company)
        {
            try
            {
                EmployeeType emplType = new EmployeeType();
                emplType.EmployeeTypeTO.WorkingUnitID = company;
                List<EmployeeTypeTO> typeList = new List<EmployeeTypeTO>();

                if (company != -1)
                    typeList = emplType.Search();
                                
                cbEmployeeType.DataSource = typeList;
                cbEmployeeType.DisplayMember = "EmployeeTypeName";
                cbEmployeeType.ValueMember = "EmployeeTypeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOrganizationalUnitCombo()
        {
            try
            {                
                cbOrganizationalUnit.DataSource = oUnitsList;
                cbOrganizationalUnit.DisplayMember = "Name";
                cbOrganizationalUnit.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.populateOrganizationalUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

		private void populateStatusCombo()
		{
			ArrayList statusArray = new ArrayList();
			statusArray.Add(rm.GetString("all", culture));
			statusArray.Add(Constants.statusActive);
			statusArray.Add(Constants.statusBlocked);
			statusArray.Add(Constants.statusRetired);

			cbStatus.DataSource = statusArray;
		}

		private void populateAccessGroupCombo()
		{
			try
			{
				EmployeeGroupAccessControl accessGroup = new EmployeeGroupAccessControl();
				ArrayList accessGroupArray = accessGroup.Search("");

				cbAccessGroup.DataSource    = accessGroupArray;
				cbAccessGroup.DisplayMember = "Name";
				cbAccessGroup.ValueMember   = "AccessGroupId";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAdd.populateAccessGroupCombo(): " + ex.Message + "\n");
                throw ex;
			}
		}

		private void populateTypes()
		{
			try
			{
				ArrayList array = new ArrayList();

				foreach (string type in types.Keys)
				{
					array.Add(type);
				}
				
				cbType.DataSource = array;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAdd.populateTypes(): " + ex.Message + "\n");
                throw ex;
			}
		}

        //private string parse(string forParsing)
        //{
        //    string parsedString = forParsing.Trim();
        //    if (parsedString.StartsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(1);
        //        parsedString = "%" + parsedString;
        //    }

        //    if (parsedString.EndsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(0, parsedString.Length - 1);
        //        parsedString = parsedString + "%";
        //    }

        //    return parsedString;
        //}

		public void setAddressID(int addressID)
		{
			this.tbAddressID.Text = (addressID < 0) ? "0" : addressID.ToString();
		}

		private void Employees_Closed(object sender, System.EventArgs e)
		{
            currentEmployee = new EmployeeTO();
		}

		private void setLanguage()
		{
			try
			{
				if (currentEmployee.EmployeeID.Equals(-1))
				{
					this.Text = rm.GetString("addEmployee", culture);
				}
				else
				{
					this.Text = rm.GetString("updateEmployee", culture);
				}

				groupBox.Text = rm.GetString("menuEmployees", culture);
				
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnTag.Text = rm.GetString("btnTag", culture);
				btnAddress.Text = rm.GetString("btnAddress", culture);
				btnTimeSchedule.Text = rm.GetString("btnTimeSchedule", culture);
				btnBrowse.Text = rm.GetString("btnBrowse", culture);
                btnAddData.Text = rm.GetString("btnAddData", culture);

				lblEmployeeID.Text = rm.GetString("lblEployeeID", culture);
				lblFirstName.Text = rm.GetString("lblFirstName", culture);
				lblLastName.Text = rm.GetString("lblLastName", culture);
				lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
				lblStatus.Text = rm.GetString("lblStatus", culture);
				lblPassword.Text = rm.GetString("lblPassword", culture);
				lblAddressID.Text = rm.GetString("lblAddressID", culture);
				lblGroup.Text = rm.GetString("lblGroup", culture);
				lblPicture.Text = rm.GetString("lblPicture", culture);
				lblType.Text = rm.GetString("lblType", culture);
				lblAccessGroup.Text = rm.GetString("lblAccessGroup", culture);
                lblOrganizationalUnit.Text = rm.GetString("lblOrganizationalUnit", culture);
                lblEmployeeTypeID.Text = rm.GetString("lblEmployeeTypeID", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesAdd.setLanguage(): " + ex.Message + "\n");
				throw ex;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Do not reload calling form on cancel
                doReloadOnBack = false;
                clearResources();
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
			
		}

		public void InitialiseObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
		}

		/// <summary>
		/// Populate Working Groups Combo Box
		/// </summary>
		private void populateWorkingGroupCombo()
		{
			try
			{				
				List<WorkingGroupTO> wgArray = new WorkingGroup().Search();
				//wgArray.Insert(0, new WorkingGroup(-1, rm.GetString("all", culture), rm.GetString("all", culture)));

				cbGroup.DataSource = wgArray;
				cbGroup.DisplayMember = "GroupName";
				cbGroup.ValueMember = "EmployeeGroupID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesAdd.populateWorkingGroupCombo(): " + ex.Message + "\n");
                throw ex;
			}
		}

		private void btnTimeSchedule_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
                
                WTEmployeesTimeSchedule emplTimeSchedule = new WTEmployeesTimeSchedule(currentEmployee);
				emplTimeSchedule.ShowDialog(this);

                //NATALIJA08112017
                List<EmployeeTO> lista = new Employee().Search(currentEmployee.EmployeeID.ToString());
                EmployeeTO empl = new EmployeeTO();
                empl = lista[0];
                cbGroup.SelectedValue = empl.WorkingGroupID;
                cbFirstChange = true;
                //n
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesAdd.btnTimeSchedule_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{			
			try
			{
                this.Cursor = Cursors.WaitCursor;
                this.tbFirstName.Text = "";
                this.tbLastName.Text = "";
                this.tbPassword.Text = "";
                this.tbPicture.Text = "";
                this.tbAddressID.Text = "";
                this.cbWorkingUnitID.SelectedIndex = 0;
                this.cbAccessGroup.SelectedValue = Constants.defaultAccessGroupID;
                this.cbGroup.SelectedValue = Constants.defaultWorkingGroupID;
                this.cbType.SelectedIndex = 0;
                this.pbPicture.Image = null;

                // update form
                if (currentEmployee.EmployeeID >= 0)
                {
                    this.cbStatus.SelectedIndex = 0;
                }
                else
                {
                    // add form
                    this.tbEmployeeID.Text = "";
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesAdd.btnClear_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				OpenFileDialog dlg = new OpenFileDialog();
				dlg.Title = rm.GetString("browsePhoto", culture);
				dlg.Filter = "bmp files (*.bmp)|*.bmp"
					+ "|gif files (*.gif)|*.gif"
					+ "|jpg files (*.jpg)|*.jpg"
					+ "|jpeg files (*.jpeg)|*.jpeg";
				dlg.FilterIndex = 3;
				
				if (dlg.ShowDialog() == DialogResult.OK)
				{
                    FileInfo fi = new FileInfo(dlg.FileName);
                    if (fi.Length > 1043576)
                    {
                        MessageBox.Show(rm.GetString("imgLarge", culture));
                    }
                    else
                    {
                        this.tbPicture.Text = dlg.FileName;
                        if (pbPicture.Image != null)
                            pbPicture.Image.Dispose();
                        this.pbPicture.Image = new Bitmap(dlg.FileName);
                    }
				}

				dlg.Dispose();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesAdd.btnBrowse_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
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
			catch(Exception ex)
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

        public void setTimeSchemaChanged(bool changed)
        {
            this.timeSchemaChanged = changed;
        }

        public void setTagChanged(bool changed)
        {
            this.tagChanged = changed;
        }

        public void setHasTagChanged(bool hasTag)
        {
            this.currentEmployee.HasTag = hasTag;
        }

        private void tbEmployeeID_Leave(object sender, EventArgs e)
        {
            if (!tbEmployeeID.Text.Trim().Equals("") && tbPassword.Text.Equals(""))
            {
                tbPassword.Text = tbEmployeeID.Text.Trim();
            }
        }

        private void EmployeeAdd_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAdd.EmployeeAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbWorkingUnitID.SelectedIndex = cbWorkingUnitID.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            
            try
            {
                EmployeeAsco4 addData = new EmployeeAsco4();
                addData.EmplAsco4TO.EmployeeID = currentEmployee.EmployeeID;
                List<EmployeeAsco4TO> dataList = addData.Search();

                EmployeeAsco4TO dataTO = new EmployeeAsco4TO();
                if (dataList.Count == 1)
                    dataTO = dataList[0];

                EmployeeAdditionalData addDataForm = new EmployeeAdditionalData(dataTO, currentEmployee.EmployeeID);
                addDataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAdd.btnAddData_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnOUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits("");
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbOrganizationalUnit.SelectedIndex = cbOrganizationalUnit.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAdd.btnOUTree_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWorkingUnitID_SelectedIndexChanged(object sender, EventArgs e)
        {            
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int company = -1;

                if (cbWorkingUnitID.SelectedValue != null && cbWorkingUnitID.SelectedValue is int && (int)cbWorkingUnitID.SelectedValue != -1)
                    company = Common.Misc.getRootWorkingUnit((int)cbWorkingUnitID.SelectedValue, wUnitsDic);

                populateEmplTypeCombo(company);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAdd.cbWorkingUnitID_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
