using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;
using MySql.Data.MySqlClient;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for ApplUsersAdd.
	/// </summary>
	public class ApplUsersAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblUserID;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbUserID;
		private System.Windows.Forms.TextBox tbConfirmPassword;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lblConfirmPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.ComboBox cbPrivilegeLvl;
		private System.Windows.Forms.TextBox tbNumOfTries;
		private System.Windows.Forms.ComboBox cbStatus;
		private System.Windows.Forms.Label lblPrivilegeLvl;
		private System.Windows.Forms.Label lblNumOfTries;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblLanguage;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ApplUserTO currentUser = null;
		ApplUserTO logInUser;
		ResourceManager rm;

		private CultureInfo culture;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbLang;
		private System.Windows.Forms.Label lblExitPermVerification;
		private System.Windows.Forms.RadioButton rbNO;
		private System.Windows.Forms.RadioButton rbYES;

        private string form = "";
        private Label lblUsingHoursAheadLimit;
        private RadioButton rbYesExtraHours;
        private RadioButton rbNoExtraHours;
        private GroupBox gbUsingAheadLimit;
        private NumericUpDown numUsingAhead;
        private Label lblHrs1;
        List<int> modulesList;
        private GroupBox gbExtraHours;
        private GroupBox gbExitPermission;
        private Label label1;
        private GroupBox gbUserCategory;
        private Button btnRemove;
        private Button btnAdd;
        private ListView lvTypeCategories;
        private ListView lvUserCategory;
		
		DebugLog log;

        private List<ApplUserCategoryTO> allCategories = new List<ApplUserCategoryTO>();
        private List<ApplUserXApplUserCategoryTO> categoryXTypeList = new List<ApplUserXApplUserCategoryTO>();
        List<int> belongCategoriesID = new List<int>();

		public ApplUsersAdd()
		{
			InitializeComponent();
			
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentUser = new ApplUserTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(ApplUsersAdd).Assembly);

			setLanguage();
			populateStatusCombo(Constants.addForm);
			populateLanguageCombo();

            this.tbNumOfTries.Text = Constants.numOfTries.ToString();
			this.tbNumOfTries.Enabled = false;
			
			btnUpdate.Visible = false;

            this.form = Constants.addForm;
            rbNoExtraHours.Checked = true;
            gbUsingAheadLimit.Visible = false;

            modulesList = Common.Misc.getLicenceModuls(null);
            setVisibility();
                       
		}

        public ApplUsersAdd(ApplUserTO user, List<int> belongList)
		{
			InitializeComponent();


            belongCategoriesID = belongList;

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentUser = new ApplUserTO(user.UserID, user.Password, user.Name, user.Description,
				user.PrivilegeLvl, user.Status, user.NumOfTries, user.LangCode);
			currentUser.ExitPermVerification = user.ExitPermVerification;
            currentUser.ExtraHoursAdvancedAmt = user.ExtraHoursAdvancedAmt;
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);

			setLanguage();

            populateStatusCombo(Constants.updateForm);			
			populateLanguageCombo();
			populateUpdateForm(currentUser);

			btnSave.Visible = false;
			label3.Visible = false;
			tbUserID.Enabled = false;
			tbConfirmPassword.Enabled = false;
            modulesList = Common.Misc.getLicenceModuls(null);
            setVisibility();
            this.form = Constants.updateForm;
            if (currentUser.ExitPermVerification == (int)Constants.PermVerification.Yes)
                rbYES.Checked = true;
            else
                rbNO.Checked = true;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblUserID = new System.Windows.Forms.Label();
            this.tbUserID = new System.Windows.Forms.TextBox();
            this.tbConfirmPassword = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cbPrivilegeLvl = new System.Windows.Forms.ComboBox();
            this.tbNumOfTries = new System.Windows.Forms.TextBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.lblPrivilegeLvl = new System.Windows.Forms.Label();
            this.lblNumOfTries = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbLang = new System.Windows.Forms.ComboBox();
            this.lblExitPermVerification = new System.Windows.Forms.Label();
            this.rbNO = new System.Windows.Forms.RadioButton();
            this.rbYES = new System.Windows.Forms.RadioButton();
            this.lblUsingHoursAheadLimit = new System.Windows.Forms.Label();
            this.rbYesExtraHours = new System.Windows.Forms.RadioButton();
            this.rbNoExtraHours = new System.Windows.Forms.RadioButton();
            this.gbUsingAheadLimit = new System.Windows.Forms.GroupBox();
            this.numUsingAhead = new System.Windows.Forms.NumericUpDown();
            this.lblHrs1 = new System.Windows.Forms.Label();
            this.gbExtraHours = new System.Windows.Forms.GroupBox();
            this.gbExitPermission = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbUserCategory = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvTypeCategories = new System.Windows.Forms.ListView();
            this.lvUserCategory = new System.Windows.Forms.ListView();
            this.gbUsingAheadLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUsingAhead)).BeginInit();
            this.gbExtraHours.SuspendLayout();
            this.gbExitPermission.SuspendLayout();
            this.gbUserCategory.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUserID
            // 
            this.lblUserID.Location = new System.Drawing.Point(17, 22);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(124, 23);
            this.lblUserID.TabIndex = 0;
            this.lblUserID.Text = "UserID:";
            this.lblUserID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbUserID
            // 
            this.tbUserID.Location = new System.Drawing.Point(147, 25);
            this.tbUserID.MaxLength = 20;
            this.tbUserID.Name = "tbUserID";
            this.tbUserID.Size = new System.Drawing.Size(256, 20);
            this.tbUserID.TabIndex = 1;
            // 
            // tbConfirmPassword
            // 
            this.tbConfirmPassword.Location = new System.Drawing.Point(147, 91);
            this.tbConfirmPassword.MaxLength = 10;
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.PasswordChar = '*';
            this.tbConfirmPassword.Size = new System.Drawing.Size(256, 20);
            this.tbConfirmPassword.TabIndex = 6;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(147, 58);
            this.tbPassword.MaxLength = 10;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(256, 20);
            this.tbPassword.TabIndex = 4;
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.Location = new System.Drawing.Point(13, 88);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(128, 23);
            this.lblConfirmPassword.TabIndex = 5;
            this.lblConfirmPassword.Text = "Confirm Password:";
            this.lblConfirmPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(25, 56);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(116, 23);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(147, 154);
            this.tbDesc.MaxLength = 150;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(256, 20);
            this.tbDesc.TabIndex = 10;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(147, 123);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(256, 20);
            this.tbName.TabIndex = 8;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(13, 152);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(128, 23);
            this.lblDescription.TabIndex = 9;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(14, 120);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(127, 23);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPrivilegeLvl
            // 
            this.cbPrivilegeLvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrivilegeLvl.Location = new System.Drawing.Point(547, 220);
            this.cbPrivilegeLvl.Name = "cbPrivilegeLvl";
            this.cbPrivilegeLvl.Size = new System.Drawing.Size(256, 21);
            this.cbPrivilegeLvl.TabIndex = 21;
            this.cbPrivilegeLvl.Visible = false;
            // 
            // tbNumOfTries
            // 
            this.tbNumOfTries.Location = new System.Drawing.Point(147, 216);
            this.tbNumOfTries.Name = "tbNumOfTries";
            this.tbNumOfTries.Size = new System.Drawing.Size(256, 20);
            this.tbNumOfTries.TabIndex = 14;
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.Location = new System.Drawing.Point(147, 184);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(256, 21);
            this.cbStatus.TabIndex = 12;
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.cbStatus_SelectedIndexChanged);
            // 
            // lblPrivilegeLvl
            // 
            this.lblPrivilegeLvl.Location = new System.Drawing.Point(412, 218);
            this.lblPrivilegeLvl.Name = "lblPrivilegeLvl";
            this.lblPrivilegeLvl.Size = new System.Drawing.Size(129, 23);
            this.lblPrivilegeLvl.TabIndex = 20;
            this.lblPrivilegeLvl.Text = "Privilege level:";
            this.lblPrivilegeLvl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPrivilegeLvl.Visible = false;
            // 
            // lblNumOfTries
            // 
            this.lblNumOfTries.Location = new System.Drawing.Point(8, 213);
            this.lblNumOfTries.Name = "lblNumOfTries";
            this.lblNumOfTries.Size = new System.Drawing.Size(133, 24);
            this.lblNumOfTries.TabIndex = 13;
            this.lblNumOfTries.Text = "Number of login tries:";
            this.lblNumOfTries.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(23, 182);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(118, 23);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLanguage
            // 
            this.lblLanguage.Location = new System.Drawing.Point(428, 181);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(113, 24);
            this.lblLanguage.TabIndex = 15;
            this.lblLanguage.Text = "Language:";
            this.lblLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 511);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 511);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 22;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(738, 510);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(409, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "*";
            // 
            // cbLang
            // 
            this.cbLang.Location = new System.Drawing.Point(547, 184);
            this.cbLang.Name = "cbLang";
            this.cbLang.Size = new System.Drawing.Size(256, 21);
            this.cbLang.TabIndex = 16;
            // 
            // lblExitPermVerification
            // 
            this.lblExitPermVerification.Location = new System.Drawing.Point(3, 20);
            this.lblExitPermVerification.Name = "lblExitPermVerification";
            this.lblExitPermVerification.Size = new System.Drawing.Size(121, 32);
            this.lblExitPermVerification.TabIndex = 17;
            this.lblExitPermVerification.Text = "Exit permissions verification:";
            this.lblExitPermVerification.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbNO
            // 
            this.rbNO.Location = new System.Drawing.Point(195, 24);
            this.rbNO.Name = "rbNO";
            this.rbNO.Size = new System.Drawing.Size(48, 24);
            this.rbNO.TabIndex = 19;
            this.rbNO.Text = "NO";
            // 
            // rbYES
            // 
            this.rbYES.Location = new System.Drawing.Point(136, 24);
            this.rbYES.Name = "rbYES";
            this.rbYES.Size = new System.Drawing.Size(48, 24);
            this.rbYES.TabIndex = 18;
            this.rbYES.Text = "YES";
            // 
            // lblUsingHoursAheadLimit
            // 
            this.lblUsingHoursAheadLimit.Location = new System.Drawing.Point(4, 26);
            this.lblUsingHoursAheadLimit.Name = "lblUsingHoursAheadLimit";
            this.lblUsingHoursAheadLimit.Size = new System.Drawing.Size(121, 23);
            this.lblUsingHoursAheadLimit.TabIndex = 24;
            this.lblUsingHoursAheadLimit.Text = "Using hours ahead:";
            this.lblUsingHoursAheadLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbYesExtraHours
            // 
            this.rbYesExtraHours.Location = new System.Drawing.Point(140, 24);
            this.rbYesExtraHours.Name = "rbYesExtraHours";
            this.rbYesExtraHours.Size = new System.Drawing.Size(48, 24);
            this.rbYesExtraHours.TabIndex = 25;
            this.rbYesExtraHours.Text = "YES";
            this.rbYesExtraHours.CheckedChanged += new System.EventHandler(this.rbYesExtraHours_CheckedChanged);
            // 
            // rbNoExtraHours
            // 
            this.rbNoExtraHours.Checked = true;
            this.rbNoExtraHours.Location = new System.Drawing.Point(194, 25);
            this.rbNoExtraHours.Name = "rbNoExtraHours";
            this.rbNoExtraHours.Size = new System.Drawing.Size(48, 24);
            this.rbNoExtraHours.TabIndex = 26;
            this.rbNoExtraHours.TabStop = true;
            this.rbNoExtraHours.Text = "NO";
            this.rbNoExtraHours.CheckedChanged += new System.EventHandler(this.rbNoExtraHours_CheckedChanged);
            // 
            // gbUsingAheadLimit
            // 
            this.gbUsingAheadLimit.Controls.Add(this.numUsingAhead);
            this.gbUsingAheadLimit.Controls.Add(this.lblHrs1);
            this.gbUsingAheadLimit.Location = new System.Drawing.Point(245, 10);
            this.gbUsingAheadLimit.Name = "gbUsingAheadLimit";
            this.gbUsingAheadLimit.Size = new System.Drawing.Size(111, 50);
            this.gbUsingAheadLimit.TabIndex = 52;
            this.gbUsingAheadLimit.TabStop = false;
            this.gbUsingAheadLimit.Text = "Using ahead limit";
            this.gbUsingAheadLimit.Visible = false;
            // 
            // numUsingAhead
            // 
            this.numUsingAhead.Location = new System.Drawing.Point(6, 18);
            this.numUsingAhead.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numUsingAhead.Name = "numUsingAhead";
            this.numUsingAhead.Size = new System.Drawing.Size(65, 20);
            this.numUsingAhead.TabIndex = 44;
            this.numUsingAhead.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numUsingAhead.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // lblHrs1
            // 
            this.lblHrs1.Location = new System.Drawing.Point(77, 16);
            this.lblHrs1.Name = "lblHrs1";
            this.lblHrs1.Size = new System.Drawing.Size(30, 23);
            this.lblHrs1.TabIndex = 45;
            this.lblHrs1.Text = "hrs";
            this.lblHrs1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbExtraHours
            // 
            this.gbExtraHours.Controls.Add(this.lblUsingHoursAheadLimit);
            this.gbExtraHours.Controls.Add(this.gbUsingAheadLimit);
            this.gbExtraHours.Controls.Add(this.rbYesExtraHours);
            this.gbExtraHours.Controls.Add(this.rbNoExtraHours);
            this.gbExtraHours.Location = new System.Drawing.Point(437, 108);
            this.gbExtraHours.Name = "gbExtraHours";
            this.gbExtraHours.Size = new System.Drawing.Size(365, 67);
            this.gbExtraHours.TabIndex = 53;
            this.gbExtraHours.TabStop = false;
            this.gbExtraHours.Text = "Extra hours";
            // 
            // gbExitPermission
            // 
            this.gbExitPermission.Controls.Add(this.lblExitPermVerification);
            this.gbExitPermission.Controls.Add(this.rbYES);
            this.gbExitPermission.Controls.Add(this.rbNO);
            this.gbExitPermission.Location = new System.Drawing.Point(437, 25);
            this.gbExitPermission.Name = "gbExitPermission";
            this.gbExitPermission.Size = new System.Drawing.Size(365, 70);
            this.gbExitPermission.TabIndex = 54;
            this.gbExitPermission.TabStop = false;
            this.gbExitPermission.Text = "Exit permissions";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(409, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 55;
            this.label1.Text = "*";
            // 
            // gbUserCategory
            // 
            this.gbUserCategory.Controls.Add(this.btnRemove);
            this.gbUserCategory.Controls.Add(this.btnAdd);
            this.gbUserCategory.Controls.Add(this.lvTypeCategories);
            this.gbUserCategory.Controls.Add(this.lvUserCategory);
            this.gbUserCategory.Location = new System.Drawing.Point(18, 266);
            this.gbUserCategory.Name = "gbUserCategory";
            this.gbUserCategory.Size = new System.Drawing.Size(784, 217);
            this.gbUserCategory.TabIndex = 56;
            this.gbUserCategory.TabStop = false;
            this.gbUserCategory.Text = "User category";
            // 
            // btnRemove
            // 
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(373, 111);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(38, 31);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "<-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(373, 74);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(38, 31);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "->";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvTypeCategories
            // 
            this.lvTypeCategories.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvTypeCategories.FullRowSelect = true;
            this.lvTypeCategories.GridLines = true;
            this.lvTypeCategories.HideSelection = false;
            this.lvTypeCategories.Location = new System.Drawing.Point(439, 19);
            this.lvTypeCategories.Name = "lvTypeCategories";
            this.lvTypeCategories.Size = new System.Drawing.Size(340, 192);
            this.lvTypeCategories.TabIndex = 41;
            this.lvTypeCategories.UseCompatibleStateImageBehavior = false;
            this.lvTypeCategories.View = System.Windows.Forms.View.Details;
            // 
            // lvUserCategory
            // 
            this.lvUserCategory.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvUserCategory.FullRowSelect = true;
            this.lvUserCategory.GridLines = true;
            this.lvUserCategory.HideSelection = false;
            this.lvUserCategory.Location = new System.Drawing.Point(6, 19);
            this.lvUserCategory.Name = "lvUserCategory";
            this.lvUserCategory.Size = new System.Drawing.Size(340, 192);
            this.lvUserCategory.TabIndex = 40;
            this.lvUserCategory.UseCompatibleStateImageBehavior = false;
            this.lvUserCategory.View = System.Windows.Forms.View.Details;
            // 
            // ApplUsersAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(818, 549);
            this.ControlBox = false;
            this.Controls.Add(this.gbUserCategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbExitPermission);
            this.Controls.Add(this.gbExtraHours);
            this.Controls.Add(this.cbLang);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.cbPrivilegeLvl);
            this.Controls.Add(this.tbNumOfTries);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.lblPrivilegeLvl);
            this.Controls.Add(this.lblNumOfTries);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.tbConfirmPassword);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.tbUserID);
            this.Controls.Add(this.lblUserID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ApplUsersAdd";
            this.ShowInTaskbar = false;
            this.Text = "ApplUsersAdd";
            this.Load += new System.EventHandler(this.ApplUsersAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ApplUsersAdd_KeyUp);
            this.gbUsingAheadLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUsingAhead)).EndInit();
            this.gbExtraHours.ResumeLayout(false);
            this.gbExitPermission.ResumeLayout(false);
            this.gbUserCategory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Set proper language
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				if (!currentUser.UserID.Equals(""))
				{
					this.Text = rm.GetString("updateUser", culture);
				}
				else
				{
					this.Text = rm.GetString("addUser", culture);
				}
                // group box's
                gbUsingAheadLimit.Text = rm.GetString("gbUsingAheadLimit", culture);
                gbExitPermission.Text = rm.GetString("gbExitPermission", culture);
                gbExtraHours.Text = rm.GetString("gbExtraHours", culture);
                gbUserCategory.Text = rm.GetString("gbUserCategory", culture);

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblUserID.Text = rm.GetString("lblUserID", culture);
				lblPassword.Text = rm.GetString("lblPassword", culture);
				lblConfirmPassword.Text = rm.GetString("lblConfirmPassword", culture);
				lblName.Text = rm.GetString("lblName", culture);
				lblDescription.Text = rm.GetString("lblDescription", culture);
				lblStatus.Text = rm.GetString("lblStatus", culture);
				lblNumOfTries.Text = rm.GetString("lblNumOfTries", culture);
				lblPrivilegeLvl.Text = rm.GetString("lblPrivilegeLvl", culture);
				lblLanguage.Text = rm.GetString("lblLanguage", culture);
				rbYES.Text = rm.GetString("yes", culture);
				rbNO.Text = rm.GetString("no", culture);
				lblExitPermVerification.Text = rm.GetString("lblExitPermVerification", culture);
                lblHrs1.Text = rm.GetString("lblHrs", culture);
                lblUsingHoursAheadLimit.Text = rm.GetString("lblUsingHoursAhead", culture);
                rbYesExtraHours.Text = rm.GetString("yes", culture);
                rbNoExtraHours.Text = rm.GetString("no", culture);


                lvUserCategory.BeginUpdate();
                lvUserCategory.Columns.Add(rm.GetString("hdrName", culture), lvUserCategory.Width / 2 - 5, HorizontalAlignment.Left);
                lvUserCategory.Columns.Add(rm.GetString("hdrDescription", culture), lvUserCategory.Width / 2 - 5, HorizontalAlignment.Left);
                lvUserCategory.EndUpdate();

                lvTypeCategories.BeginUpdate();
                lvTypeCategories.Columns.Add(rm.GetString("hdrName", culture), lvTypeCategories.Width / 3 - 5, HorizontalAlignment.Left);
                lvTypeCategories.Columns.Add(rm.GetString("hdrDescription", culture), lvTypeCategories.Width / 3 - 5, HorizontalAlignment.Left);
                lvTypeCategories.EndUpdate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateUpdateForm(ApplUserTO user)
		{
			try
			{
				this.tbUserID.Text = user.UserID.Trim();
				this.tbPassword.Text = user.Password.Trim();
                this.tbConfirmPassword.Text = user.Password.Trim();
				this.tbName.Text = user.Name.Trim();
				this.tbDesc.Text = user.Description.Trim();
				this.cbStatus.SelectedIndex = cbStatus.FindStringExact(user.Status.Trim());
				if (user.NumOfTries != -1)
				{
					this.tbNumOfTries.Text = user.NumOfTries.ToString().Trim();
				}
				else
				{
					this.tbNumOfTries.Text = "";
				}
				//this.cbPrivilegeLvl.SelectedIndex = cbStatus.FindStringExact(user.PrivilegeLvl.ToString().Trim());

				if (user.LangCode.Trim().Equals(Constants.Lang_sr))
				{
					cbLang.SelectedIndex = cbLang.FindStringExact(Constants.DisplayLangSR);
				}
				else if (user.LangCode.Trim().Equals(Constants.Lang_en))
				{
					cbLang.SelectedIndex = cbLang.FindStringExact(Constants.DisplayLangEN);
				}
                else if (user.LangCode.Trim().Equals(Constants.Lang_fi))
                {
                    cbLang.SelectedIndex = cbLang.FindStringExact(Constants.DisplayLangFI);
                }

				if (user.ExitPermVerification == (int)Constants.PermVerification.Yes)
					rbYES.Checked = true;
				else
					rbNO.Checked = true;

                if (cbStatus.SelectedItem.ToString().Equals(Constants.statusRetired))
                {
                    this.tbPassword.Enabled = false;
                    this.tbConfirmPassword.Enabled = false;
                    this.tbName.Enabled = false;
                    this.tbDesc.Enabled = false;
                    this.tbNumOfTries.Enabled = false;
                    this.cbLang.Enabled = false;
                    this.cbPrivilegeLvl.Enabled = false;
                }
                if (user.ExtraHoursAdvancedAmt > 0)
                {
                    this.rbNoExtraHours.Checked = false;
                    this.rbYesExtraHours.Checked = true;
                    numUsingAhead.Value = user.ExtraHoursAdvancedAmt;
                }
                else
                {
                    this.rbNoExtraHours.Checked = true;
                }
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateStatusCombo(string form)
		{
			try
			{
				ArrayList statusArray = new ArrayList();
				//string[] statuses = ConfigurationManager.AppSettings["UserStatus"].Split(',');
				
				/*foreach (string status in statuses)
				{
					statusArray.Add(status.Trim());
				}*/
                statusArray.Add(Constants.statusActive);
                statusArray.Add(Constants.statusDisabled);

                if (form.Equals(Constants.updateForm))
                {
                    statusArray.Add(Constants.statusRetired);
                }

				cbStatus.DataSource = statusArray;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsers.populateStatusCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateLanguageCombo()
		{
			try
			{
				cbLang.Items.Insert(0,Constants.DisplayLangSR);
				cbLang.Items.Insert(1,Constants.DisplayLangEN);
                cbLang.Items.Insert(2, Constants.DisplayLangFI);
				cbLang.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsers.populateLanguageCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				currentUser = new ApplUserTO();

				if (this.tbUserID.Text.Trim().Equals(""))
				{
					MessageBox.Show(rm.GetString("updUserNoUserID", culture));
					return;
				}
                if (this.tbPassword.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("updUserPasswordNotEmpty", culture));
                    this.tbPassword.Text = "";
                    this.tbConfirmPassword.Text = "";
                    this.tbPassword.Focus();
                    return;
                }
				if (!this.tbPassword.Text.Trim().Equals(this.tbConfirmPassword.Text.Trim()))
				{
					MessageBox.Show(rm.GetString("updUserPasswordNotMatch", culture));
					this.tbPassword.Text = "";
					this.tbConfirmPassword.Text = "";
					this.tbPassword.Focus();
					return;
				}
				try
				{
					if (!tbNumOfTries.Text.Trim().Equals(""))
					{
						Int32.Parse(tbNumOfTries.Text.Trim());
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("updUserNumOfTriesNotNum", culture));
					return;
				}
				
				currentUser.UserID = tbUserID.Text.Trim();
				if (!tbPassword.Text.Trim().Equals(""))
				{
					currentUser.Password = tbPassword.Text.Trim();
				}
				if (!tbName.Text.Trim().Equals(""))
				{
					currentUser.Name = tbName.Text.Trim();
				}
				if (!tbDesc.Text.Trim().Equals(""))
				{
					currentUser.Description = tbDesc.Text.Trim();
				}
				if (cbStatus.SelectedIndex >= 0)
				{
					currentUser.Status = cbStatus.SelectedItem.ToString();
				}
				if (!tbNumOfTries.Text.Trim().Equals(""))
				{
					currentUser.NumOfTries = Int32.Parse(tbNumOfTries.Text.Trim());
				}

				if (cbLang.SelectedIndex.Equals(0))
				{
					currentUser.LangCode = Constants.Lang_sr;
				}
				else if (cbLang.SelectedIndex.Equals(1))
				{
					currentUser.LangCode = Constants.Lang_en;
				}
                else if (cbLang.SelectedIndex.Equals(2))
                {
                    currentUser.LangCode = Constants.Lang_fi;
                }

				if (rbYES.Checked)
				{
					currentUser.ExitPermVerification = (int)Constants.PermVerification.Yes;
				}
				else
				{
					currentUser.ExitPermVerification = (int)Constants.PermVerification.No;
				}
                if (rbYesExtraHours.Checked)
                {
                    currentUser.ExtraHoursAdvancedAmt = int.Parse(numUsingAhead.Value.ToString());
                }
                else
                {
                    currentUser.ExtraHoursAdvancedAmt = 0;
                }

                ApplUser appUser = new ApplUser();
                appUser.UserTO = currentUser;
                bool trans = appUser.BeginTransaction();
                bool succ = trans;
                if (trans)
                {
                    try
                    {
                        int inserted = appUser.Save(false);
                        succ = succ && (inserted > 0);
                        if (succ) 
                        {
                            ApplUserXApplUserCategory userXCategory = new ApplUserXApplUserCategory();
                            userXCategory.SetTransaction(appUser.GetTransaction());
                            bool first = true;
                            foreach (int category in belongCategoriesID)
                            {
                                userXCategory.UserXCategoryTO.CategoryID = category;
                                userXCategory.UserXCategoryTO.UserID = appUser.UserTO.UserID;
                                if (first)
                                {
                                    userXCategory.UserXCategoryTO.DefaultCategory = 1;
                                    first = false;
                                }
                                else
                                {
                                    userXCategory.UserXCategoryTO.DefaultCategory = 0;
                                }
                                succ = succ && userXCategory.Save(false) == 1;
                            }
                            if (succ)
                            {
                                appUser.CommitTransaction();
                            }
                            else
                            {
                                appUser.RollbackTransaction();

                            }
                        }
                    }
                    catch (SqlException sqlex)
                    {
                        if (appUser.GetTransaction() != null)
                            appUser.RollbackTransaction();
                        if (sqlex.Number == 2627)
                        {
                            MessageBox.Show(rm.GetString("userExists", culture));
                        }
                        else
                        {
                            MessageBox.Show(sqlex.Message);
                            log.writeLog(DateTime.Now + " ApplUsersAdd.btnSave_Click(): " + sqlex.Message + "\n");
                        }
                    }
                }
				if (succ)
                {
					MessageBox.Show(rm.GetString("userInserted", culture));
					this.Close();				
				}
			}
			catch(SqlException sqlex)
			{
				if(sqlex.Number == 2627)
				{
					MessageBox.Show(rm.GetString("userExists", culture));
				}
				else
				{
					MessageBox.Show(sqlex.Message);
					log.writeLog(DateTime.Now + " ApplUsersAdd.btnSave_Click(): " + sqlex.Message + "\n");
				}
			}
			catch(MySqlException mysqlex)
			{
				if(mysqlex.Number == 1062)
				{
					MessageBox.Show(rm.GetString("userExists", culture));
				}
				else
				{
					MessageBox.Show(mysqlex.Message);
					log.writeLog(DateTime.Now + " ApplUsersAdd.btnSave_Click(): " + mysqlex.Message + "\n");
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsers.btnSave_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void setVisibility()
        { 
            if (!modulesList.Contains((int)Constants.Moduls.ExtraHours))
            {
                lblUsingHoursAheadLimit.Visible = false;
                rbNoExtraHours.Visible = false;
                rbYesExtraHours.Visible = false;
                gbUsingAheadLimit.Visible = false;
            }
            if (!modulesList.Contains((int)Constants.Moduls.ExitPermits))
            {
                lblExitPermVerification.Visible = false;
                rbYES.Visible = false;
                rbNO.Visible = false;
            }
           
        }

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

                if (this.tbPassword.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("updUserPasswordNotEmpty", culture));
                    this.tbPassword.Text = "";
                    this.tbConfirmPassword.Text = "";
                    this.tbPassword.Focus();
                    return;
                }
				if (this.tbConfirmPassword.Enabled && (!this.tbPassword.Text.Trim().Equals(this.tbConfirmPassword.Text.Trim())))
				{
					MessageBox.Show(rm.GetString("updUserPasswordNotMatch", culture));
					this.tbPassword.Text = "";
					this.tbConfirmPassword.Text = "";
					this.tbPassword.Focus();
					return;
				}
				try
				{
					if (!tbNumOfTries.Text.Trim().Equals(""))
					{
						Int32.Parse(tbNumOfTries.Text.Trim());
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("updUserNumOfTriesNotNum", culture));
					return;
				}
				
				currentUser.Password = tbPassword.Text.Trim();
				currentUser.Name = tbName.Text.Trim();
				currentUser.Description = tbDesc.Text.Trim();
				if (cbStatus.SelectedIndex >= 0)
				{
					currentUser.Status = cbStatus.SelectedItem.ToString();
				}
				if (!tbNumOfTries.Text.Trim().Equals(""))
				{
					currentUser.NumOfTries = Int32.Parse(tbNumOfTries.Text.Trim());
				}

				if (cbLang.SelectedIndex.Equals(0))
				{
					currentUser.LangCode = Constants.Lang_sr;
				}
				else if (cbLang.SelectedIndex.Equals(1))
				{
					currentUser.LangCode = Constants.Lang_en;
				}
                else if (cbLang.SelectedIndex.Equals(2))
                {
                    currentUser.LangCode = Constants.Lang_fi;
                }

				if (rbYES.Checked)
				{
					currentUser.ExitPermVerification = (int)Constants.PermVerification.Yes;
				}
				else
				{
					currentUser.ExitPermVerification = (int)Constants.PermVerification.No;
				}
                if (rbYesExtraHours.Checked)
                {
                    currentUser.ExtraHoursAdvancedAmt = int.Parse(numUsingAhead.Value.ToString());
                }
                else
                {
                    currentUser.ExtraHoursAdvancedAmt = 0;
                }

				//bool updated = currentUser.Update(currentUser.UserID, currentUser.Password, currentUser.Name,
				//	currentUser.Description, currentUser.PrivilegeLvl, currentUser.Status, 
				//	currentUser.NumOfTries, currentUser.LangCode);
                ApplUser appUser = new ApplUser();
                appUser.UserTO = currentUser;

                bool trans = appUser.BeginTransaction();
                bool isUpdated = trans;
                if (trans)
                {
                    try
                    {
                        isUpdated = appUser.UpdateExitPermVerification(false);
                        if (isUpdated)
                        {
                            ApplUserXApplUserCategory userCategory = new ApplUserXApplUserCategory();
                            userCategory.SetTransaction(appUser.GetTransaction());
                            isUpdated = isUpdated && userCategory.Delete(appUser.UserTO.UserID, false);
                            bool first = true;
                            foreach (int category in belongCategoriesID)
                            {
                                userCategory.UserXCategoryTO.CategoryID = category;
                                userCategory.UserXCategoryTO.UserID = appUser.UserTO.UserID;
                                if (first)
                                {
                                    userCategory.UserXCategoryTO.DefaultCategory = 1;
                                    first = false;
                                }
                                else
                                {
                                    userCategory.UserXCategoryTO.DefaultCategory = 0;
                                }
                                isUpdated = isUpdated && userCategory.Save(false) == 1;
                            }
                            if (isUpdated)
                            {
                                appUser.CommitTransaction();                                
                            }
                            else
                            {
                                appUser.RollbackTransaction();
                               
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isUpdated = false;
                        if (appUser.GetTransaction() != null)
                            appUser.RollbackTransaction();
                        log.writeLog(DateTime.Now + " ApplUsers.btnUpdate_Click(): " + ex.Message + "\n");
                        MessageBox.Show(ex.Message);
                    }
                }
				if (isUpdated)
				{
					MessageBox.Show(rm.GetString("userUpdated", culture));
					this.Close();				
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsers.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void tbPassword_TextChanged(object sender, System.EventArgs e)
		{
			tbConfirmPassword.Enabled = true;
		}

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.form.Equals(Constants.updateForm))
                {
                    if (cbStatus.SelectedItem.ToString().Equals(Constants.statusRetired))
                    {
                        this.tbPassword.Enabled = false;
                        this.tbConfirmPassword.Enabled = false;
                        this.tbName.Enabled = false;
                        this.tbDesc.Enabled = false;
                        this.tbNumOfTries.Enabled = false;
                        this.cbLang.Enabled = false;
                        this.cbPrivilegeLvl.Enabled = false;
                    }
                    else
                    {
                        this.tbPassword.Enabled = true;
                        this.tbConfirmPassword.Enabled = true;
                        this.tbName.Enabled = true;
                        this.tbDesc.Enabled = true;
                        this.tbNumOfTries.Enabled = true;
                        this.cbLang.Enabled = true;
                        this.cbPrivilegeLvl.Enabled = true;
                    }

                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " ApplUsersAdd.cbStatus_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ApplUsersAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ApplUsersAdd.ApplUsersAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

       
        private void rbYesExtraHours_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if(rbYesExtraHours.Checked)
                gbUsingAheadLimit.Visible = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.rbYesExtraHours_CheckedChanged): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbNoExtraHours_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbNoExtraHours.Checked)
                    gbUsingAheadLimit.Visible = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.rbNoExtraHours_CheckedChanged): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvUserCategory.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvUserCategory.SelectedItems)
                    {
                        ApplUserCategoryTO tt = (ApplUserCategoryTO)item.Tag;
                        if (!belongCategoriesID.Contains(tt.CategoryID))
                            belongCategoriesID.Add(tt.CategoryID);
                    }
                    populateUserCategoryListView();
                    populateTypeBelongCategorieListView();
                }
                else
                {
                    MessageBox.Show(rm.GetString("selType", culture), "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ApplUsersAdd.btnAdd_Click(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateUserCategoryListView()
        {
            try
            {
                lvUserCategory.BeginUpdate();
                lvUserCategory.Items.Clear();
                allCategories = new ApplUserCategory().Search(null);
                foreach (ApplUserCategoryTO category in allCategories)
                {
                    if (!belongCategoriesID.Contains(category.CategoryID))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = category.Name;
                        item.SubItems.Add(category.Desc);
                        item.Tag = category;
                        lvUserCategory.Items.Add(item);
                    }
                }
                lvUserCategory.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersAdd.populateUserCategoryListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populateTypeBelongCategorieListView()
        {
            try
            {
                if (allCategories.Count > 0)
                {
                    lvTypeCategories.BeginUpdate();
                    lvTypeCategories.Items.Clear();
                    foreach (int id in belongCategoriesID)
                    {
                        foreach (ApplUserCategoryTO type in allCategories)
                        {
                            if (id == type.CategoryID)
                            {
                                ListViewItem item = new ListViewItem();
                                item.Text = type.Name;
                                item.SubItems.Add(type.Desc);
                                item.Tag = type;
                                item.ToolTipText = type.Desc;
                                lvTypeCategories.Items.Add(item);
                            }
                        }
                    }
                    lvTypeCategories.EndUpdate();
                    lvTypeCategories.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ApplUsersAdd.populateTypeBelongCategorieListView(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void ApplUsersAdd_Load(object sender, EventArgs e)
        {
            try
            {
                populateUserCategoryListView();
                populateTypeBelongCategorieListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ApplUsersAdd.ApplUsersAdd_Load(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvTypeCategories.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvTypeCategories.SelectedItems)
                    {
                        ApplUserCategoryTO tt = (ApplUserCategoryTO)item.Tag;

                        if (belongCategoriesID.Contains(tt.CategoryID))
                            belongCategoriesID.Remove(tt.CategoryID);

                    }
                    populateTypeBelongCategorieListView();
                    populateUserCategoryListView();
                }
                else
                {
                    MessageBox.Show(rm.GetString("selTypeBelong", culture), "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EventAdd.btnRemove_Click(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
