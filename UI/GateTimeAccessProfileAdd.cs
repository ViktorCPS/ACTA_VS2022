using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for GateTimeAccessProfileAdd.
	/// </summary>
	public class GateTimeAccessProfileAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblProfile15;
		private System.Windows.Forms.Label lblProfile14;
		private System.Windows.Forms.Label lblProfile13;
		private System.Windows.Forms.Label lblProfile12;
		private System.Windows.Forms.Label lblProfile11;
		private System.Windows.Forms.Label lblProfile10;
		private System.Windows.Forms.Label lblProfile9;
		private System.Windows.Forms.Label lblProfile8;
		private System.Windows.Forms.Label lblProfile7;
		private System.Windows.Forms.Label lblProfile6;
		private System.Windows.Forms.Label lblProfile5;
		private System.Windows.Forms.Label lblProfile4;
		private System.Windows.Forms.Label lblProfile3;
		private System.Windows.Forms.Label lblProfile2;
		private System.Windows.Forms.Label lblProfile1;
		private System.Windows.Forms.Label lblProfile0;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label lblGateProfiles;
		private System.Windows.Forms.Label lblProfileDescr;
		private System.Windows.Forms.ComboBox cbProfile0;
		private System.Windows.Forms.ComboBox cbProfile1;
		private System.Windows.Forms.ComboBox cbProfile2;
		private System.Windows.Forms.ComboBox cbProfile3;
		private System.Windows.Forms.ComboBox cbProfile4;
		private System.Windows.Forms.ComboBox cbProfile5;
		private System.Windows.Forms.ComboBox cbProfile6;
		private System.Windows.Forms.ComboBox cbProfile7;
		private System.Windows.Forms.ComboBox cbProfile15;
		private System.Windows.Forms.ComboBox cbProfile13;
		private System.Windows.Forms.ComboBox cbProfile12;
		private System.Windows.Forms.ComboBox cbProfile11;
		private System.Windows.Forms.ComboBox cbProfile10;
		private System.Windows.Forms.ComboBox cbProfile9;
		private System.Windows.Forms.ComboBox cbProfile8;
		private System.Windows.Forms.ComboBox cbProfile14;
		private System.Windows.Forms.TextBox tbDesc0;
		private System.Windows.Forms.TextBox tbDesc1;
		private System.Windows.Forms.TextBox tbDesc2;
		private System.Windows.Forms.TextBox tbDesc3;
		private System.Windows.Forms.TextBox tbDesc7;
		private System.Windows.Forms.TextBox tbDesc6;
		private System.Windows.Forms.TextBox tbDesc5;
		private System.Windows.Forms.TextBox tbDesc4;
		private System.Windows.Forms.TextBox tbDesc11;
		private System.Windows.Forms.TextBox tbDesc10;
		private System.Windows.Forms.TextBox tbDesc9;
		private System.Windows.Forms.TextBox tbDesc8;
		private System.Windows.Forms.TextBox tbDesc15;
		private System.Windows.Forms.TextBox tbDesc14;
		private System.Windows.Forms.TextBox tbDesc13;
		private System.Windows.Forms.TextBox tbDesc12;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		GateTimeAccessProfile currentGateTimeAccessProfile = null;

		ApplUserTO logInUser;		
		ResourceManager rm;
		private CultureInfo culture;				
		DebugLog log;

		string profile0 = "";
		string profile1 = "";
		string profile2 = "";
		string profile3 = "";
		string profile4 = "";
		string profile5 = "";
		string profile6 = "";
		string profile7 = "";
		string profile8 = "";
		string profile9 = "";
		string profile10 = "";
		string profile11 = "";
		string profile12 = "";
		string profile13 = "";
		string profile14 = "";
		string profile15 = "";

		public GateTimeAccessProfileAdd()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentGateTimeAccessProfile = new GateTimeAccessProfile();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(GateTimeAccessProfileAdd).Assembly);
			setLanguage();

			populateComboBoxes();
			selectProfilesFromDB();
			selectProfileFromDB(cbProfile0, 0);
			selectProfileFromDB(cbProfile15, 1);
			setDescriptions();
			cbProfile0.Enabled = false;
			cbProfile15.Enabled = false;

			btnUpdate.Visible = false;
		}

		public GateTimeAccessProfileAdd(GateTimeAccessProfile gateTimeAccessProfile)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentGateTimeAccessProfile = new GateTimeAccessProfile(gateTimeAccessProfile.GateTAProfileId, gateTimeAccessProfile.Name,
				gateTimeAccessProfile.Description, gateTimeAccessProfile.GateTAProfile0, gateTimeAccessProfile.GateTAProfile1,
				gateTimeAccessProfile.GateTAProfile2, gateTimeAccessProfile.GateTAProfile3, gateTimeAccessProfile.GateTAProfile4,
				gateTimeAccessProfile.GateTAProfile5, gateTimeAccessProfile.GateTAProfile6, gateTimeAccessProfile.GateTAProfile7,
				gateTimeAccessProfile.GateTAProfile8, gateTimeAccessProfile.GateTAProfile9, gateTimeAccessProfile.GateTAProfile10,
				gateTimeAccessProfile.GateTAProfile11, gateTimeAccessProfile.GateTAProfile12, gateTimeAccessProfile.GateTAProfile13,
				gateTimeAccessProfile.GateTAProfile14, gateTimeAccessProfile.GateTAProfile15); 
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(GateTimeAccessProfileAdd).Assembly);
			setLanguage();

			tbName.Text = gateTimeAccessProfile.Name.Trim();
			tbDesc.Text = gateTimeAccessProfile.Description.Trim();

			populateComboBoxes();
			selectProfilesFromDB();
			setDescriptions();
			cbProfile0.Enabled = false;
			cbProfile15.Enabled = false;

			btnSave.Visible = false;
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
            this.label3 = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cbProfile0 = new System.Windows.Forms.ComboBox();
            this.cbProfile1 = new System.Windows.Forms.ComboBox();
            this.cbProfile2 = new System.Windows.Forms.ComboBox();
            this.cbProfile3 = new System.Windows.Forms.ComboBox();
            this.cbProfile4 = new System.Windows.Forms.ComboBox();
            this.cbProfile5 = new System.Windows.Forms.ComboBox();
            this.cbProfile6 = new System.Windows.Forms.ComboBox();
            this.cbProfile7 = new System.Windows.Forms.ComboBox();
            this.cbProfile15 = new System.Windows.Forms.ComboBox();
            this.cbProfile14 = new System.Windows.Forms.ComboBox();
            this.cbProfile13 = new System.Windows.Forms.ComboBox();
            this.cbProfile12 = new System.Windows.Forms.ComboBox();
            this.cbProfile11 = new System.Windows.Forms.ComboBox();
            this.cbProfile10 = new System.Windows.Forms.ComboBox();
            this.cbProfile9 = new System.Windows.Forms.ComboBox();
            this.cbProfile8 = new System.Windows.Forms.ComboBox();
            this.lblProfile15 = new System.Windows.Forms.Label();
            this.lblProfile14 = new System.Windows.Forms.Label();
            this.lblProfile13 = new System.Windows.Forms.Label();
            this.lblProfile12 = new System.Windows.Forms.Label();
            this.lblProfile11 = new System.Windows.Forms.Label();
            this.lblProfile10 = new System.Windows.Forms.Label();
            this.lblProfile9 = new System.Windows.Forms.Label();
            this.lblProfile8 = new System.Windows.Forms.Label();
            this.lblProfile7 = new System.Windows.Forms.Label();
            this.lblProfile6 = new System.Windows.Forms.Label();
            this.lblProfile5 = new System.Windows.Forms.Label();
            this.lblProfile4 = new System.Windows.Forms.Label();
            this.lblProfile3 = new System.Windows.Forms.Label();
            this.lblProfile2 = new System.Windows.Forms.Label();
            this.lblProfile1 = new System.Windows.Forms.Label();
            this.lblProfile0 = new System.Windows.Forms.Label();
            this.tbDesc0 = new System.Windows.Forms.TextBox();
            this.tbDesc1 = new System.Windows.Forms.TextBox();
            this.tbDesc2 = new System.Windows.Forms.TextBox();
            this.tbDesc3 = new System.Windows.Forms.TextBox();
            this.tbDesc7 = new System.Windows.Forms.TextBox();
            this.tbDesc6 = new System.Windows.Forms.TextBox();
            this.tbDesc5 = new System.Windows.Forms.TextBox();
            this.tbDesc4 = new System.Windows.Forms.TextBox();
            this.tbDesc11 = new System.Windows.Forms.TextBox();
            this.tbDesc10 = new System.Windows.Forms.TextBox();
            this.tbDesc9 = new System.Windows.Forms.TextBox();
            this.tbDesc8 = new System.Windows.Forms.TextBox();
            this.tbDesc15 = new System.Windows.Forms.TextBox();
            this.tbDesc14 = new System.Windows.Forms.TextBox();
            this.tbDesc13 = new System.Windows.Forms.TextBox();
            this.tbDesc12 = new System.Windows.Forms.TextBox();
            this.lblGateProfiles = new System.Windows.Forms.Label();
            this.lblProfileDescr = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(616, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "*";
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(152, 48);
            this.tbDesc.MaxLength = 150;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(456, 20);
            this.tbDesc.TabIndex = 20;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(152, 16);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(456, 20);
            this.tbName.TabIndex = 18;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(64, 48);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(80, 23);
            this.lblDesc.TabIndex = 19;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(64, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 23);
            this.lblName.TabIndex = 17;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbProfile0
            // 
            this.cbProfile0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile0.Location = new System.Drawing.Point(152, 104);
            this.cbProfile0.Name = "cbProfile0";
            this.cbProfile0.Size = new System.Drawing.Size(224, 21);
            this.cbProfile0.TabIndex = 22;
            this.cbProfile0.SelectedIndexChanged += new System.EventHandler(this.cbProfile0_SelectedIndexChanged);
            // 
            // cbProfile1
            // 
            this.cbProfile1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile1.Location = new System.Drawing.Point(152, 128);
            this.cbProfile1.Name = "cbProfile1";
            this.cbProfile1.Size = new System.Drawing.Size(224, 21);
            this.cbProfile1.TabIndex = 23;
            this.cbProfile1.SelectedIndexChanged += new System.EventHandler(this.cbProfile1_SelectedIndexChanged);
            // 
            // cbProfile2
            // 
            this.cbProfile2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile2.Location = new System.Drawing.Point(152, 152);
            this.cbProfile2.Name = "cbProfile2";
            this.cbProfile2.Size = new System.Drawing.Size(224, 21);
            this.cbProfile2.TabIndex = 24;
            this.cbProfile2.SelectedIndexChanged += new System.EventHandler(this.cbProfile2_SelectedIndexChanged);
            // 
            // cbProfile3
            // 
            this.cbProfile3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile3.Location = new System.Drawing.Point(152, 176);
            this.cbProfile3.Name = "cbProfile3";
            this.cbProfile3.Size = new System.Drawing.Size(224, 21);
            this.cbProfile3.TabIndex = 25;
            this.cbProfile3.SelectedIndexChanged += new System.EventHandler(this.cbProfile3_SelectedIndexChanged);
            // 
            // cbProfile4
            // 
            this.cbProfile4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile4.Location = new System.Drawing.Point(152, 200);
            this.cbProfile4.Name = "cbProfile4";
            this.cbProfile4.Size = new System.Drawing.Size(224, 21);
            this.cbProfile4.TabIndex = 26;
            this.cbProfile4.SelectedIndexChanged += new System.EventHandler(this.cbProfile4_SelectedIndexChanged);
            // 
            // cbProfile5
            // 
            this.cbProfile5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile5.Location = new System.Drawing.Point(152, 224);
            this.cbProfile5.Name = "cbProfile5";
            this.cbProfile5.Size = new System.Drawing.Size(224, 21);
            this.cbProfile5.TabIndex = 27;
            this.cbProfile5.SelectedIndexChanged += new System.EventHandler(this.cbProfile5_SelectedIndexChanged);
            // 
            // cbProfile6
            // 
            this.cbProfile6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile6.Location = new System.Drawing.Point(152, 248);
            this.cbProfile6.Name = "cbProfile6";
            this.cbProfile6.Size = new System.Drawing.Size(224, 21);
            this.cbProfile6.TabIndex = 28;
            this.cbProfile6.SelectedIndexChanged += new System.EventHandler(this.cbProfile6_SelectedIndexChanged);
            // 
            // cbProfile7
            // 
            this.cbProfile7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile7.Location = new System.Drawing.Point(152, 272);
            this.cbProfile7.Name = "cbProfile7";
            this.cbProfile7.Size = new System.Drawing.Size(224, 21);
            this.cbProfile7.TabIndex = 29;
            this.cbProfile7.SelectedIndexChanged += new System.EventHandler(this.cbProfile7_SelectedIndexChanged);
            // 
            // cbProfile15
            // 
            this.cbProfile15.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile15.Location = new System.Drawing.Point(152, 464);
            this.cbProfile15.Name = "cbProfile15";
            this.cbProfile15.Size = new System.Drawing.Size(224, 21);
            this.cbProfile15.TabIndex = 37;
            this.cbProfile15.SelectedIndexChanged += new System.EventHandler(this.cbProfile15_SelectedIndexChanged);
            // 
            // cbProfile14
            // 
            this.cbProfile14.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile14.Location = new System.Drawing.Point(152, 440);
            this.cbProfile14.Name = "cbProfile14";
            this.cbProfile14.Size = new System.Drawing.Size(224, 21);
            this.cbProfile14.TabIndex = 36;
            this.cbProfile14.SelectedIndexChanged += new System.EventHandler(this.cbProfile14_SelectedIndexChanged);
            // 
            // cbProfile13
            // 
            this.cbProfile13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile13.Location = new System.Drawing.Point(152, 416);
            this.cbProfile13.Name = "cbProfile13";
            this.cbProfile13.Size = new System.Drawing.Size(224, 21);
            this.cbProfile13.TabIndex = 35;
            this.cbProfile13.SelectedIndexChanged += new System.EventHandler(this.cbProfile13_SelectedIndexChanged);
            // 
            // cbProfile12
            // 
            this.cbProfile12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile12.Location = new System.Drawing.Point(152, 392);
            this.cbProfile12.Name = "cbProfile12";
            this.cbProfile12.Size = new System.Drawing.Size(224, 21);
            this.cbProfile12.TabIndex = 34;
            this.cbProfile12.SelectedIndexChanged += new System.EventHandler(this.cbProfile12_SelectedIndexChanged);
            // 
            // cbProfile11
            // 
            this.cbProfile11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile11.Location = new System.Drawing.Point(152, 368);
            this.cbProfile11.Name = "cbProfile11";
            this.cbProfile11.Size = new System.Drawing.Size(224, 21);
            this.cbProfile11.TabIndex = 33;
            this.cbProfile11.SelectedIndexChanged += new System.EventHandler(this.cbProfile11_SelectedIndexChanged);
            // 
            // cbProfile10
            // 
            this.cbProfile10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile10.Location = new System.Drawing.Point(152, 344);
            this.cbProfile10.Name = "cbProfile10";
            this.cbProfile10.Size = new System.Drawing.Size(224, 21);
            this.cbProfile10.TabIndex = 32;
            this.cbProfile10.SelectedIndexChanged += new System.EventHandler(this.cbProfile10_SelectedIndexChanged);
            // 
            // cbProfile9
            // 
            this.cbProfile9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile9.Location = new System.Drawing.Point(152, 320);
            this.cbProfile9.Name = "cbProfile9";
            this.cbProfile9.Size = new System.Drawing.Size(224, 21);
            this.cbProfile9.TabIndex = 31;
            this.cbProfile9.SelectedIndexChanged += new System.EventHandler(this.cbProfile9_SelectedIndexChanged);
            // 
            // cbProfile8
            // 
            this.cbProfile8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile8.Location = new System.Drawing.Point(152, 296);
            this.cbProfile8.Name = "cbProfile8";
            this.cbProfile8.Size = new System.Drawing.Size(224, 21);
            this.cbProfile8.TabIndex = 30;
            this.cbProfile8.SelectedIndexChanged += new System.EventHandler(this.cbProfile8_SelectedIndexChanged);
            // 
            // lblProfile15
            // 
            this.lblProfile15.Location = new System.Drawing.Point(24, 464);
            this.lblProfile15.Name = "lblProfile15";
            this.lblProfile15.Size = new System.Drawing.Size(120, 20);
            this.lblProfile15.TabIndex = 53;
            this.lblProfile15.Text = "Weekly schedule 15:";
            this.lblProfile15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile14
            // 
            this.lblProfile14.Location = new System.Drawing.Point(24, 440);
            this.lblProfile14.Name = "lblProfile14";
            this.lblProfile14.Size = new System.Drawing.Size(120, 20);
            this.lblProfile14.TabIndex = 52;
            this.lblProfile14.Text = "Weekly schedule 14:";
            this.lblProfile14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile13
            // 
            this.lblProfile13.Location = new System.Drawing.Point(24, 416);
            this.lblProfile13.Name = "lblProfile13";
            this.lblProfile13.Size = new System.Drawing.Size(120, 20);
            this.lblProfile13.TabIndex = 51;
            this.lblProfile13.Text = "Weekly schedule 13:";
            this.lblProfile13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile12
            // 
            this.lblProfile12.Location = new System.Drawing.Point(24, 392);
            this.lblProfile12.Name = "lblProfile12";
            this.lblProfile12.Size = new System.Drawing.Size(120, 20);
            this.lblProfile12.TabIndex = 50;
            this.lblProfile12.Text = "Weekly schedule 12:";
            this.lblProfile12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile11
            // 
            this.lblProfile11.Location = new System.Drawing.Point(24, 368);
            this.lblProfile11.Name = "lblProfile11";
            this.lblProfile11.Size = new System.Drawing.Size(120, 20);
            this.lblProfile11.TabIndex = 49;
            this.lblProfile11.Text = "Weekly schedule 11:";
            this.lblProfile11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile10
            // 
            this.lblProfile10.Location = new System.Drawing.Point(24, 344);
            this.lblProfile10.Name = "lblProfile10";
            this.lblProfile10.Size = new System.Drawing.Size(120, 20);
            this.lblProfile10.TabIndex = 48;
            this.lblProfile10.Text = "Weekly schedule 10:";
            this.lblProfile10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile9
            // 
            this.lblProfile9.Location = new System.Drawing.Point(24, 320);
            this.lblProfile9.Name = "lblProfile9";
            this.lblProfile9.Size = new System.Drawing.Size(120, 20);
            this.lblProfile9.TabIndex = 47;
            this.lblProfile9.Text = "Weekly schedule 9:";
            this.lblProfile9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile8
            // 
            this.lblProfile8.Location = new System.Drawing.Point(24, 296);
            this.lblProfile8.Name = "lblProfile8";
            this.lblProfile8.Size = new System.Drawing.Size(120, 20);
            this.lblProfile8.TabIndex = 46;
            this.lblProfile8.Text = "Weekly schedule 8:";
            this.lblProfile8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile7
            // 
            this.lblProfile7.Location = new System.Drawing.Point(24, 272);
            this.lblProfile7.Name = "lblProfile7";
            this.lblProfile7.Size = new System.Drawing.Size(120, 20);
            this.lblProfile7.TabIndex = 45;
            this.lblProfile7.Text = "Weekly schedule 7:";
            this.lblProfile7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile6
            // 
            this.lblProfile6.Location = new System.Drawing.Point(24, 248);
            this.lblProfile6.Name = "lblProfile6";
            this.lblProfile6.Size = new System.Drawing.Size(120, 20);
            this.lblProfile6.TabIndex = 44;
            this.lblProfile6.Text = "Weekly schedule 6:";
            this.lblProfile6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile5
            // 
            this.lblProfile5.Location = new System.Drawing.Point(24, 224);
            this.lblProfile5.Name = "lblProfile5";
            this.lblProfile5.Size = new System.Drawing.Size(120, 20);
            this.lblProfile5.TabIndex = 43;
            this.lblProfile5.Text = "Weekly schedule 5:";
            this.lblProfile5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile4
            // 
            this.lblProfile4.Location = new System.Drawing.Point(24, 200);
            this.lblProfile4.Name = "lblProfile4";
            this.lblProfile4.Size = new System.Drawing.Size(120, 20);
            this.lblProfile4.TabIndex = 42;
            this.lblProfile4.Text = "Weekly schedule 4:";
            this.lblProfile4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile3
            // 
            this.lblProfile3.Location = new System.Drawing.Point(24, 176);
            this.lblProfile3.Name = "lblProfile3";
            this.lblProfile3.Size = new System.Drawing.Size(120, 20);
            this.lblProfile3.TabIndex = 41;
            this.lblProfile3.Text = "Weekly schedule 3:";
            this.lblProfile3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile2
            // 
            this.lblProfile2.Location = new System.Drawing.Point(24, 152);
            this.lblProfile2.Name = "lblProfile2";
            this.lblProfile2.Size = new System.Drawing.Size(120, 20);
            this.lblProfile2.TabIndex = 40;
            this.lblProfile2.Text = "Weekly schedule 2:";
            this.lblProfile2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile1
            // 
            this.lblProfile1.Location = new System.Drawing.Point(24, 128);
            this.lblProfile1.Name = "lblProfile1";
            this.lblProfile1.Size = new System.Drawing.Size(120, 20);
            this.lblProfile1.TabIndex = 39;
            this.lblProfile1.Text = "Weekly schedule 1:";
            this.lblProfile1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile0
            // 
            this.lblProfile0.Location = new System.Drawing.Point(24, 104);
            this.lblProfile0.Name = "lblProfile0";
            this.lblProfile0.Size = new System.Drawing.Size(120, 20);
            this.lblProfile0.TabIndex = 38;
            this.lblProfile0.Text = "Weekly schedule 0:";
            this.lblProfile0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc0
            // 
            this.tbDesc0.Enabled = false;
            this.tbDesc0.Location = new System.Drawing.Point(384, 104);
            this.tbDesc0.MaxLength = 150;
            this.tbDesc0.Name = "tbDesc0";
            this.tbDesc0.Size = new System.Drawing.Size(224, 20);
            this.tbDesc0.TabIndex = 54;
            // 
            // tbDesc1
            // 
            this.tbDesc1.Enabled = false;
            this.tbDesc1.Location = new System.Drawing.Point(384, 128);
            this.tbDesc1.MaxLength = 150;
            this.tbDesc1.Name = "tbDesc1";
            this.tbDesc1.Size = new System.Drawing.Size(224, 20);
            this.tbDesc1.TabIndex = 55;
            // 
            // tbDesc2
            // 
            this.tbDesc2.Enabled = false;
            this.tbDesc2.Location = new System.Drawing.Point(384, 152);
            this.tbDesc2.MaxLength = 150;
            this.tbDesc2.Name = "tbDesc2";
            this.tbDesc2.Size = new System.Drawing.Size(224, 20);
            this.tbDesc2.TabIndex = 56;
            // 
            // tbDesc3
            // 
            this.tbDesc3.Enabled = false;
            this.tbDesc3.Location = new System.Drawing.Point(384, 176);
            this.tbDesc3.MaxLength = 150;
            this.tbDesc3.Name = "tbDesc3";
            this.tbDesc3.Size = new System.Drawing.Size(224, 20);
            this.tbDesc3.TabIndex = 57;
            // 
            // tbDesc7
            // 
            this.tbDesc7.Enabled = false;
            this.tbDesc7.Location = new System.Drawing.Point(384, 272);
            this.tbDesc7.MaxLength = 150;
            this.tbDesc7.Name = "tbDesc7";
            this.tbDesc7.Size = new System.Drawing.Size(224, 20);
            this.tbDesc7.TabIndex = 61;
            // 
            // tbDesc6
            // 
            this.tbDesc6.Enabled = false;
            this.tbDesc6.Location = new System.Drawing.Point(384, 248);
            this.tbDesc6.MaxLength = 150;
            this.tbDesc6.Name = "tbDesc6";
            this.tbDesc6.Size = new System.Drawing.Size(224, 20);
            this.tbDesc6.TabIndex = 60;
            // 
            // tbDesc5
            // 
            this.tbDesc5.Enabled = false;
            this.tbDesc5.Location = new System.Drawing.Point(384, 224);
            this.tbDesc5.MaxLength = 150;
            this.tbDesc5.Name = "tbDesc5";
            this.tbDesc5.Size = new System.Drawing.Size(224, 20);
            this.tbDesc5.TabIndex = 59;
            // 
            // tbDesc4
            // 
            this.tbDesc4.Enabled = false;
            this.tbDesc4.Location = new System.Drawing.Point(384, 200);
            this.tbDesc4.MaxLength = 150;
            this.tbDesc4.Name = "tbDesc4";
            this.tbDesc4.Size = new System.Drawing.Size(224, 20);
            this.tbDesc4.TabIndex = 58;
            // 
            // tbDesc11
            // 
            this.tbDesc11.Enabled = false;
            this.tbDesc11.Location = new System.Drawing.Point(384, 368);
            this.tbDesc11.MaxLength = 150;
            this.tbDesc11.Name = "tbDesc11";
            this.tbDesc11.Size = new System.Drawing.Size(224, 20);
            this.tbDesc11.TabIndex = 65;
            // 
            // tbDesc10
            // 
            this.tbDesc10.Enabled = false;
            this.tbDesc10.Location = new System.Drawing.Point(384, 344);
            this.tbDesc10.MaxLength = 150;
            this.tbDesc10.Name = "tbDesc10";
            this.tbDesc10.Size = new System.Drawing.Size(224, 20);
            this.tbDesc10.TabIndex = 64;
            // 
            // tbDesc9
            // 
            this.tbDesc9.Enabled = false;
            this.tbDesc9.Location = new System.Drawing.Point(384, 320);
            this.tbDesc9.MaxLength = 150;
            this.tbDesc9.Name = "tbDesc9";
            this.tbDesc9.Size = new System.Drawing.Size(224, 20);
            this.tbDesc9.TabIndex = 63;
            // 
            // tbDesc8
            // 
            this.tbDesc8.Enabled = false;
            this.tbDesc8.Location = new System.Drawing.Point(384, 296);
            this.tbDesc8.MaxLength = 150;
            this.tbDesc8.Name = "tbDesc8";
            this.tbDesc8.Size = new System.Drawing.Size(224, 20);
            this.tbDesc8.TabIndex = 62;
            // 
            // tbDesc15
            // 
            this.tbDesc15.Enabled = false;
            this.tbDesc15.Location = new System.Drawing.Point(384, 464);
            this.tbDesc15.MaxLength = 150;
            this.tbDesc15.Name = "tbDesc15";
            this.tbDesc15.Size = new System.Drawing.Size(224, 20);
            this.tbDesc15.TabIndex = 69;
            // 
            // tbDesc14
            // 
            this.tbDesc14.Enabled = false;
            this.tbDesc14.Location = new System.Drawing.Point(384, 440);
            this.tbDesc14.MaxLength = 150;
            this.tbDesc14.Name = "tbDesc14";
            this.tbDesc14.Size = new System.Drawing.Size(224, 20);
            this.tbDesc14.TabIndex = 68;
            // 
            // tbDesc13
            // 
            this.tbDesc13.Enabled = false;
            this.tbDesc13.Location = new System.Drawing.Point(384, 416);
            this.tbDesc13.MaxLength = 150;
            this.tbDesc13.Name = "tbDesc13";
            this.tbDesc13.Size = new System.Drawing.Size(224, 20);
            this.tbDesc13.TabIndex = 67;
            // 
            // tbDesc12
            // 
            this.tbDesc12.Enabled = false;
            this.tbDesc12.Location = new System.Drawing.Point(384, 392);
            this.tbDesc12.MaxLength = 150;
            this.tbDesc12.Name = "tbDesc12";
            this.tbDesc12.Size = new System.Drawing.Size(224, 20);
            this.tbDesc12.TabIndex = 66;
            // 
            // lblGateProfiles
            // 
            this.lblGateProfiles.Location = new System.Drawing.Point(152, 80);
            this.lblGateProfiles.Name = "lblGateProfiles";
            this.lblGateProfiles.Size = new System.Drawing.Size(224, 20);
            this.lblGateProfiles.TabIndex = 70;
            this.lblGateProfiles.Text = "Weekly schedules on gate:";
            this.lblGateProfiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProfileDescr
            // 
            this.lblProfileDescr.Location = new System.Drawing.Point(384, 80);
            this.lblProfileDescr.Name = "lblProfileDescr";
            this.lblProfileDescr.Size = new System.Drawing.Size(224, 20);
            this.lblProfileDescr.TabIndex = 71;
            this.lblProfileDescr.Text = "Description";
            this.lblProfileDescr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(152, 504);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 73;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(536, 504);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 74;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(152, 504);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 72;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // GateTimeAccessProfileAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(662, 536);
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblProfileDescr);
            this.Controls.Add(this.lblGateProfiles);
            this.Controls.Add(this.tbDesc15);
            this.Controls.Add(this.tbDesc14);
            this.Controls.Add(this.tbDesc13);
            this.Controls.Add(this.tbDesc12);
            this.Controls.Add(this.tbDesc11);
            this.Controls.Add(this.tbDesc10);
            this.Controls.Add(this.tbDesc9);
            this.Controls.Add(this.tbDesc8);
            this.Controls.Add(this.tbDesc7);
            this.Controls.Add(this.tbDesc6);
            this.Controls.Add(this.tbDesc5);
            this.Controls.Add(this.tbDesc4);
            this.Controls.Add(this.tbDesc3);
            this.Controls.Add(this.tbDesc2);
            this.Controls.Add(this.tbDesc1);
            this.Controls.Add(this.tbDesc0);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblProfile15);
            this.Controls.Add(this.lblProfile14);
            this.Controls.Add(this.lblProfile13);
            this.Controls.Add(this.lblProfile12);
            this.Controls.Add(this.lblProfile11);
            this.Controls.Add(this.lblProfile10);
            this.Controls.Add(this.lblProfile9);
            this.Controls.Add(this.lblProfile8);
            this.Controls.Add(this.lblProfile7);
            this.Controls.Add(this.lblProfile6);
            this.Controls.Add(this.lblProfile5);
            this.Controls.Add(this.lblProfile4);
            this.Controls.Add(this.lblProfile3);
            this.Controls.Add(this.lblProfile2);
            this.Controls.Add(this.lblProfile1);
            this.Controls.Add(this.lblProfile0);
            this.Controls.Add(this.cbProfile15);
            this.Controls.Add(this.cbProfile14);
            this.Controls.Add(this.cbProfile13);
            this.Controls.Add(this.cbProfile12);
            this.Controls.Add(this.cbProfile11);
            this.Controls.Add(this.cbProfile10);
            this.Controls.Add(this.cbProfile9);
            this.Controls.Add(this.cbProfile8);
            this.Controls.Add(this.cbProfile7);
            this.Controls.Add(this.cbProfile6);
            this.Controls.Add(this.cbProfile5);
            this.Controls.Add(this.cbProfile4);
            this.Controls.Add(this.cbProfile3);
            this.Controls.Add(this.cbProfile2);
            this.Controls.Add(this.cbProfile1);
            this.Controls.Add(this.cbProfile0);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(670, 570);
            this.MinimumSize = new System.Drawing.Size(670, 570);
            this.Name = "GateTimeAccessProfileAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Add gate access profile";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GateTimeAccessProfileAdd_KeyUp);
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
				if (!currentGateTimeAccessProfile.Name.Equals(""))
				{
					this.Text = rm.GetString("updateGateTAProfile", culture);
				}
				else
				{
					this.Text = rm.GetString("addGateTAProfile", culture);
				}
				
				// button's text
				btnSave.Text   = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblName.Text = rm.GetString("lblName", culture);
				lblDesc.Text = rm.GetString("lblDescription", culture);
				lblGateProfiles.Text = rm.GetString("lblGateProfiles", culture);
				lblProfileDescr.Text = rm.GetString("lblDescription", culture);
				lblProfile0.Text  = rm.GetString("lblProfile0", culture);
				lblProfile1.Text  = rm.GetString("lblProfile1", culture);
				lblProfile2.Text  = rm.GetString("lblProfile2", culture);
				lblProfile3.Text  = rm.GetString("lblProfile3", culture);
				lblProfile4.Text  = rm.GetString("lblProfile4", culture);
				lblProfile5.Text  = rm.GetString("lblProfile5", culture);
				lblProfile6.Text  = rm.GetString("lblProfile6", culture);
				lblProfile7.Text  = rm.GetString("lblProfile7", culture);
				lblProfile8.Text  = rm.GetString("lblProfile8", culture);
				lblProfile9.Text  = rm.GetString("lblProfile9", culture);
				lblProfile10.Text = rm.GetString("lblProfile10", culture);
				lblProfile11.Text = rm.GetString("lblProfile11", culture);
				lblProfile12.Text = rm.GetString("lblProfile12", culture);
				lblProfile13.Text = rm.GetString("lblProfile13", culture);
				lblProfile14.Text = rm.GetString("lblProfile14", culture);
				lblProfile15.Text = rm.GetString("lblProfile15", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("addGateTAProfileNameEmpty", culture));
                }
                else
                {
                    ArrayList gateTAProfiles = currentGateTimeAccessProfile.Search(tbName.Text.Trim());

                    if (gateTAProfiles.Count == 0)
                    {
                        setProfiles();
                        int inserted = currentGateTimeAccessProfile.Save(tbName.Text.Trim(), tbDesc.Text.Trim(),
                            profile0, profile1, profile2, profile3, profile4, profile5, profile6, profile7,
                            profile8, profile9, profile10, profile11, profile12, profile13, profile14, profile15);

                        if (inserted > 0)
                        {
                            currentGateTimeAccessProfile.Name = tbName.Text.Trim();
                            currentGateTimeAccessProfile.Description = tbDesc.Text.Trim();

                            MessageBox.Show(rm.GetString("gateTAProfileSaved", culture));
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("gateTAProfileNotSaved", culture));
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("gateTAProfileExists", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
             finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;	
		
				bool isUpdated = true;

				if (tbName.Text.Trim().Equals(""))
				{
					MessageBox.Show(rm.GetString("addGateTAProfileNameEmpty", culture));
				}
				else
				{
					if (!currentGateTimeAccessProfile.Name.Equals(tbName.Text.Trim()))
					{
						ArrayList gateTAProfiles = currentGateTimeAccessProfile.Search(tbName.Text.Trim());
						if (gateTAProfiles.Count != 0)
						{	
							MessageBox.Show(rm.GetString("gateTAProfileExists", culture));
							return;
						}
					}

					setProfiles();

					if (currentGateTimeAccessProfile.GateTAProfile0.ToString() != profile0
						|| currentGateTimeAccessProfile.GateTAProfile1.ToString() != profile1
						|| currentGateTimeAccessProfile.GateTAProfile2.ToString() != profile2
						|| currentGateTimeAccessProfile.GateTAProfile3.ToString() != profile3
						|| currentGateTimeAccessProfile.GateTAProfile4.ToString() != profile4
						|| currentGateTimeAccessProfile.GateTAProfile5.ToString() != profile5
						|| currentGateTimeAccessProfile.GateTAProfile6.ToString() != profile6
						|| currentGateTimeAccessProfile.GateTAProfile7.ToString() != profile7
						|| currentGateTimeAccessProfile.GateTAProfile8.ToString() != profile8
						|| currentGateTimeAccessProfile.GateTAProfile9.ToString() != profile9
						|| currentGateTimeAccessProfile.GateTAProfile10.ToString() != profile10
						|| currentGateTimeAccessProfile.GateTAProfile11.ToString() != profile11
						|| currentGateTimeAccessProfile.GateTAProfile12.ToString() != profile12
						|| currentGateTimeAccessProfile.GateTAProfile13.ToString() != profile13
						|| currentGateTimeAccessProfile.GateTAProfile14.ToString() != profile14
						|| currentGateTimeAccessProfile.GateTAProfile15.ToString() != profile15)
					{
						//some profiles are changed

						int recordsInXref = 0;
						List<GateTO> gatesWithGateProfile = new Gate().SearchWithGateTAProfile(currentGateTimeAccessProfile.GateTAProfileId.ToString());						
						if (gatesWithGateProfile.Count > 0)
						{
							foreach (GateTO gate in gatesWithGateProfile)
							{
								ArrayList gatesInXref = (new AccessGroupXGate()).Search("", gate.GateID.ToString());
								recordsInXref += gatesInXref.Count;
							}
						}

						if (recordsInXref > 0)
						{
							DialogResult proc = MessageBox.Show(rm.GetString("deleteAccessGroupXGates", culture),"", MessageBoxButtons.YesNo);
							if(proc == DialogResult.Yes)
							{
								AccessGroupXGate accessGroupXGate = new AccessGroupXGate();
								bool trans = accessGroupXGate.BeginTransaction();

								if (trans)
								{
									foreach (GateTO gate in gatesWithGateProfile)
									{
										isUpdated = accessGroupXGate.DeleteGates(gate.GateID.ToString(), false) && isUpdated;
									}									

									if (isUpdated)
									{
										currentGateTimeAccessProfile.SetTransaction(accessGroupXGate.GetTransaction());

										isUpdated = currentGateTimeAccessProfile.Update(currentGateTimeAccessProfile.GateTAProfileId.ToString(), 
											tbName.Text.Trim(), tbDesc.Text.Trim(), profile0, profile1, profile2, profile3, profile4, 
											profile5, profile6, profile7, profile8, profile9, profile10, profile11, profile12, 
											profile13, profile14, profile15, false) && isUpdated;
									}
								
									if (isUpdated)
									{		
										accessGroupXGate.CommitTransaction();

										currentGateTimeAccessProfile.Name        = tbName.Text.Trim();
										currentGateTimeAccessProfile.Description = tbDesc.Text.Trim();

										MessageBox.Show(rm.GetString("gateTAProfileUpdated", culture));
										this.Close();
									}
									else
									{
										accessGroupXGate.RollbackTransaction();

										MessageBox.Show(rm.GetString("gateTAProfileNotUpdated", culture));									
									}
								} //if (trans)
								else
								{
									MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
									return;
								}							
							} // yes
						}
						else //no records in xref table
						{
							if (isUpdated = currentGateTimeAccessProfile.Update(currentGateTimeAccessProfile.GateTAProfileId.ToString(), 
								tbName.Text.Trim(), tbDesc.Text.Trim(), profile0, profile1, profile2, profile3, profile4, 
								profile5, profile6, profile7, profile8, profile9, profile10, profile11, profile12, 
								profile13, profile14, profile15, true))
							{		
								currentGateTimeAccessProfile.Name        = tbName.Text.Trim();
								currentGateTimeAccessProfile.Description = tbDesc.Text.Trim();

								MessageBox.Show(rm.GetString("gateTAProfileUpdated", culture));
								this.Close();
							}
							else
							{
								MessageBox.Show(rm.GetString("gateTAProfileNotUpdated", culture));								
							}
						}
					}
					else if (currentGateTimeAccessProfile.Name != tbName.Text.Trim()
						|| currentGateTimeAccessProfile.Description != tbDesc.Text.Trim())
					{
						//only name or description are changed, no profile
						if (isUpdated = currentGateTimeAccessProfile.Update(currentGateTimeAccessProfile.GateTAProfileId.ToString(), 
							tbName.Text.Trim(), tbDesc.Text.Trim(), profile0, profile1, profile2, profile3, profile4, 
							profile5, profile6, profile7, profile8, profile9, profile10, profile11, profile12, 
							profile13, profile14, profile15, true))
						{		
							currentGateTimeAccessProfile.Name        = tbName.Text.Trim();
							currentGateTimeAccessProfile.Description = tbDesc.Text.Trim();

							MessageBox.Show(rm.GetString("gateTAProfileUpdated", culture));
							this.Close();
						}
						else
						{
							MessageBox.Show(rm.GetString("gateTAProfileNotUpdated", culture));								
						}
					}
					else 
					{
						//no change, do not do anything
						MessageBox.Show(rm.GetString("gateTAProfileUpdated", culture));
						this.Close();
					}																	
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void setProfiles()
		{
			try
			{
				//if (cbProfile0.SelectedIndex > 0) //0 was for *
				if (cbProfile0.SelectedIndex >= 0)
					profile0 = cbProfile0.SelectedValue.ToString();
				else
					profile0 = "";
				if (cbProfile1.SelectedIndex >= 0)
					profile1 = cbProfile1.SelectedValue.ToString();
				else
					profile1 = "";
				if (cbProfile2.SelectedIndex >= 0)
					profile2 = cbProfile2.SelectedValue.ToString();
				else
					profile2 = "";
				if (cbProfile3.SelectedIndex >= 0)
					profile3 = cbProfile3.SelectedValue.ToString();
				else
					profile3 = "";
				if (cbProfile4.SelectedIndex >= 0)
					profile4 = cbProfile4.SelectedValue.ToString();
				else
					profile4 = "";
				if (cbProfile5.SelectedIndex >= 0)
					profile5 = cbProfile5.SelectedValue.ToString();
				else
					profile5 = "";
				if (cbProfile6.SelectedIndex >= 0)
					profile6 = cbProfile6.SelectedValue.ToString();
				else
					profile6 = "";
				if (cbProfile7.SelectedIndex >= 0)
					profile7 = cbProfile7.SelectedValue.ToString();
				else
					profile7 = "";
				if (cbProfile8.SelectedIndex >= 0)
					profile8 = cbProfile8.SelectedValue.ToString();
				else
					profile8 = "";
				if (cbProfile9.SelectedIndex >= 0)
					profile9 = cbProfile9.SelectedValue.ToString();
				else
					profile9 = "";
				if (cbProfile10.SelectedIndex >= 0)
					profile10 = cbProfile10.SelectedValue.ToString();
				else
					profile10 = "";
				if (cbProfile11.SelectedIndex >= 0)
					profile11 = cbProfile11.SelectedValue.ToString();
				else
					profile11 = "";
				if (cbProfile12.SelectedIndex >= 0)
					profile12 = cbProfile12.SelectedValue.ToString();
				else
					profile12 = "";
				if (cbProfile13.SelectedIndex >= 0)
					profile13 = cbProfile13.SelectedValue.ToString();
				else
					profile13 = "";
				if (cbProfile14.SelectedIndex >= 0)
					profile14 = cbProfile14.SelectedValue.ToString();
				else
					profile14 = "";
				if (cbProfile15.SelectedIndex >= 0)
					profile15 = cbProfile15.SelectedValue.ToString();
				else
					profile15 = "";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.setProfiles(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateComboBoxes()
		{
			try
			{
				ArrayList timeAccessProfilesArray = new TimeAccessProfile().Search("");
				//timeAccessProfilesArray.Insert(0, new TimeAccessProfile(-1, "*", ""));

				populateComboBoxe(cbProfile0, timeAccessProfilesArray);				
				populateComboBoxe(cbProfile1, timeAccessProfilesArray);
				populateComboBoxe(cbProfile2, timeAccessProfilesArray);
				populateComboBoxe(cbProfile3, timeAccessProfilesArray);
				populateComboBoxe(cbProfile4, timeAccessProfilesArray);
				populateComboBoxe(cbProfile5, timeAccessProfilesArray);
				populateComboBoxe(cbProfile6, timeAccessProfilesArray);
				populateComboBoxe(cbProfile7, timeAccessProfilesArray);
				populateComboBoxe(cbProfile8, timeAccessProfilesArray);
				populateComboBoxe(cbProfile9, timeAccessProfilesArray);
				populateComboBoxe(cbProfile10, timeAccessProfilesArray);
				populateComboBoxe(cbProfile11, timeAccessProfilesArray);
				populateComboBoxe(cbProfile12, timeAccessProfilesArray);
				populateComboBoxe(cbProfile13, timeAccessProfilesArray);
				populateComboBoxe(cbProfile14, timeAccessProfilesArray);
				populateComboBoxe(cbProfile15, timeAccessProfilesArray);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.populateComboBoxes(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void populateComboBoxe(System.Windows.Forms.ComboBox cbProfile, ArrayList timeAccessProfilesArray)
        {
            try
            {
                ArrayList profileArray = new ArrayList();
                for (int i = 0; i < timeAccessProfilesArray.Count; i++)
                    profileArray.Insert(i, timeAccessProfilesArray[i]);

                cbProfile.DataSource = profileArray;
                cbProfile.DisplayMember = "Name";
                cbProfile.ValueMember = "TimeAccessProfileId";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.populateComboBoxe(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

		private void selectProfilesFromDB()
		{
			try
			{
				selectProfileFromDB(cbProfile0, currentGateTimeAccessProfile.GateTAProfile0);
				selectProfileFromDB(cbProfile1, currentGateTimeAccessProfile.GateTAProfile1);
				selectProfileFromDB(cbProfile2, currentGateTimeAccessProfile.GateTAProfile2);
				selectProfileFromDB(cbProfile3, currentGateTimeAccessProfile.GateTAProfile3);
				selectProfileFromDB(cbProfile4, currentGateTimeAccessProfile.GateTAProfile4);
				selectProfileFromDB(cbProfile5, currentGateTimeAccessProfile.GateTAProfile5);
				selectProfileFromDB(cbProfile6, currentGateTimeAccessProfile.GateTAProfile6);
				selectProfileFromDB(cbProfile7, currentGateTimeAccessProfile.GateTAProfile7);
				selectProfileFromDB(cbProfile8, currentGateTimeAccessProfile.GateTAProfile8);
				selectProfileFromDB(cbProfile9, currentGateTimeAccessProfile.GateTAProfile9);
				selectProfileFromDB(cbProfile10, currentGateTimeAccessProfile.GateTAProfile10);
				selectProfileFromDB(cbProfile11, currentGateTimeAccessProfile.GateTAProfile11);
				selectProfileFromDB(cbProfile12, currentGateTimeAccessProfile.GateTAProfile12);
				selectProfileFromDB(cbProfile13, currentGateTimeAccessProfile.GateTAProfile13);
				selectProfileFromDB(cbProfile14, currentGateTimeAccessProfile.GateTAProfile14);
				selectProfileFromDB(cbProfile15, currentGateTimeAccessProfile.GateTAProfile15);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.selectProfilesFromDB(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void selectProfileFromDB(System.Windows.Forms.ComboBox cbProfile, int profileID)
		{
			try
			{
				if (profileID != -1)
				{
					cbProfile.SelectedValue = profileID;
				}
				else
				{
					//cbProfile.SelectedIndex = 0; //index 0 was for *, value 1 is for "Always Denied"
					cbProfile.SelectedValue = 1;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.selectProfileFromDB(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void cbProfile0_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile0, tbDesc0);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile0_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile1, tbDesc1);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile1_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile2, tbDesc2);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile2_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile3, tbDesc3);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile3_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile4_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile4, tbDesc4);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile4_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile5_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile5, tbDesc5);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile5_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile6_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile6, tbDesc6);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile6_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile7_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile7, tbDesc7);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile7_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile8_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{

                this.Cursor = Cursors.WaitCursor;
				setDesc(cbProfile8, tbDesc8);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile8_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile9_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{

                this.Cursor = Cursors.WaitCursor;
				setDesc(cbProfile9, tbDesc9);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile9_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile10_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile10, tbDesc10);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile10_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile11_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile11, tbDesc11);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile11_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile12_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile12, tbDesc12);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile12_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile13_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile13, tbDesc13);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile13_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile14_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
			this.Cursor = Cursors.WaitCursor;

				setDesc(cbProfile14, tbDesc14);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile14_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbProfile15_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                setDesc(cbProfile15, tbDesc15);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.cbProfile15_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.Arrow; 
            }
		}

		private void setDescriptions()
		{
			try
			{
				setDesc(cbProfile0, tbDesc0);
				setDesc(cbProfile1, tbDesc1);
				setDesc(cbProfile2, tbDesc2);
				setDesc(cbProfile3, tbDesc3);
				setDesc(cbProfile4, tbDesc4);
				setDesc(cbProfile5, tbDesc5);
				setDesc(cbProfile6, tbDesc6);
				setDesc(cbProfile7, tbDesc7);
				setDesc(cbProfile8, tbDesc8);
				setDesc(cbProfile9, tbDesc9);
				setDesc(cbProfile10, tbDesc10);
				setDesc(cbProfile11, tbDesc11);
				setDesc(cbProfile12, tbDesc12);
				setDesc(cbProfile13, tbDesc13);
				setDesc(cbProfile14, tbDesc14);
				setDesc(cbProfile15, tbDesc15);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.setDescriptions(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void setDesc(System.Windows.Forms.ComboBox cbProfile, System.Windows.Forms.TextBox tbDesc)
		{
			if (cbProfile.Items.Count > 0 && cbProfile.SelectedIndex >= 0 && 
				cbProfile.ValueMember != "" && cbProfile.SelectedValue.ToString() != "-1")			
			{
				TransferObjects.TimeAccessProfileTO tapTO = new TransferObjects.TimeAccessProfileTO();
				tapTO = (new TimeAccessProfile()).Find(cbProfile.SelectedValue.ToString());

				tbDesc.Text = tapTO.Description.Trim();
			}
			else
				tbDesc.Text = "";
		}

        private void GateTimeAccessProfileAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " GateTimeAccessProfileAdd.GateTimeAccessProfileAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
