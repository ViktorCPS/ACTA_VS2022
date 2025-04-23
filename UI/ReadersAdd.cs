using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Net;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Reader's update and add form
	/// </summary>
	public class ReadersAdd : System.Windows.Forms.Form
	{
		//private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnAddvance;
		private System.Windows.Forms.ComboBox cbTech;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.TextBox tbReaderID;
		private System.Windows.Forms.Label lblTech;
		private System.Windows.Forms.Label lblReaderID;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.GroupBox gbAntenna0;
		private System.Windows.Forms.ComboBox cbA0PrimLoc;
		private System.Windows.Forms.Label lblA0PrimDirect;
		private System.Windows.Forms.Label lbA0lPrimaryLoc;
		private System.Windows.Forms.ComboBox cbA0PrimDirect;
		private System.Windows.Forms.GroupBox gbAntenna1;
		private System.Windows.Forms.ComboBox cbA1PrimDirect;
		private System.Windows.Forms.ComboBox cbA1PrimLoc;
		private System.Windows.Forms.Label lblA1PrimDir;
		private System.Windows.Forms.Label lblA1PrimLoc;
		private System.Windows.Forms.GroupBox gbConn;
		private System.Windows.Forms.TextBox tbConnAddress;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.RadioButton rbSerial;
		private System.Windows.Forms.RadioButton rbIP;
		private System.Windows.Forms.Label lblConnType;
		private System.Windows.Forms.Label lblDesc;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Current Reader Data
		public ReaderTO currentReader;
		// Controller instance
		public NotificationController Controller;
		// Observer client instance
		public NotificationObserverClient observerClient;
		
		private System.Windows.Forms.Button btnCancel;
		DebugLog log;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblGate0;
		private System.Windows.Forms.ComboBox cbGate0;
		private System.Windows.Forms.Label lblGate1;
		private System.Windows.Forms.ComboBox cbGate1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox cbWorkTimeCounter0;
		private System.Windows.Forms.CheckBox cbWorkTimeCounter1;
		private System.Windows.Forms.Label label10;

		ResourceManager rm;
		CultureInfo culture;

		private System.Windows.Forms.TextBox tbIP0;
		private System.Windows.Forms.TextBox tbIP1;
		private System.Windows.Forms.TextBox tbIP2;
		private System.Windows.Forms.TextBox tbIP3;
        private System.Windows.Forms.Label lblDot0;
		private System.Windows.Forms.Label lblDot2;
		private System.Windows.Forms.GroupBox gbStatus;
		private System.Windows.Forms.RadioButton rbEnabled;
		private System.Windows.Forms.RadioButton rbDisabled;
        private RadioButton rbGSM;
        private Label lblDot1;
        private TextBox tbTel;
        private Label lblTel;
        private TextBox tbPort;
        private Label lblIP;
		ApplUserTO logInUser;

		public ReadersAdd(ReaderTO rTO)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentReader = rTO;
			logInUser = NotificationController.GetLogInUser();
			
			// Run update form
			if (rTO.ReaderID != -1)
			{				
				btnUpdate.Visible = true;
				btnSave.Visible = false;
				tbReaderID.Enabled = false;
				label3.Visible = false;
			}
			// Run Add form
			else
			{
				int readID = new Reader().FindMAXReaderID();
				readID++;
				tbReaderID.Text = readID.ToString().Trim();
				btnUpdate.Visible = false;
				btnSave.Visible = true;
				tbReaderID.Enabled = true;
				label3.Visible = true;
				rbSerial.Checked = true;

				rbEnabled.Checked = true;
			}
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Readers).Assembly);
			
			this.CenterToScreen();
			setLanguage();
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAddvance = new System.Windows.Forms.Button();
            this.cbTech = new System.Windows.Forms.ComboBox();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbReaderID = new System.Windows.Forms.TextBox();
            this.lblTech = new System.Windows.Forms.Label();
            this.lblReaderID = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbAntenna0 = new System.Windows.Forms.GroupBox();
            this.cbWorkTimeCounter0 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbGate0 = new System.Windows.Forms.ComboBox();
            this.lblGate0 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbA0PrimLoc = new System.Windows.Forms.ComboBox();
            this.lblA0PrimDirect = new System.Windows.Forms.Label();
            this.lbA0lPrimaryLoc = new System.Windows.Forms.Label();
            this.cbA0PrimDirect = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gbAntenna1 = new System.Windows.Forms.GroupBox();
            this.cbWorkTimeCounter1 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbGate1 = new System.Windows.Forms.ComboBox();
            this.lblGate1 = new System.Windows.Forms.Label();
            this.cbA1PrimDirect = new System.Windows.Forms.ComboBox();
            this.cbA1PrimLoc = new System.Windows.Forms.ComboBox();
            this.lblA1PrimDir = new System.Windows.Forms.Label();
            this.lblA1PrimLoc = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbConn = new System.Windows.Forms.GroupBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.tbTel = new System.Windows.Forms.TextBox();
            this.lblTel = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.rbGSM = new System.Windows.Forms.RadioButton();
            this.lblDot2 = new System.Windows.Forms.Label();
            this.lblDot1 = new System.Windows.Forms.Label();
            this.lblDot0 = new System.Windows.Forms.Label();
            this.tbIP2 = new System.Windows.Forms.TextBox();
            this.tbIP1 = new System.Windows.Forms.TextBox();
            this.tbIP0 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblConnType = new System.Windows.Forms.Label();
            this.rbIP = new System.Windows.Forms.RadioButton();
            this.rbSerial = new System.Windows.Forms.RadioButton();
            this.tbConnAddress = new System.Windows.Forms.TextBox();
            this.tbIP3 = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.rbDisabled = new System.Windows.Forms.RadioButton();
            this.rbEnabled = new System.Windows.Forms.RadioButton();
            this.gbAntenna0.SuspendLayout();
            this.gbAntenna1.SuspendLayout();
            this.gbConn.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(8, 344);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 27;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAddvance
            // 
            this.btnAddvance.Location = new System.Drawing.Point(329, 344);
            this.btnAddvance.Name = "btnAddvance";
            this.btnAddvance.Size = new System.Drawing.Size(168, 23);
            this.btnAddvance.TabIndex = 35;
            this.btnAddvance.Text = "Advance >>>";
            this.btnAddvance.Click += new System.EventHandler(this.btnAddvance_Click);
            // 
            // cbTech
            // 
            this.cbTech.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTech.Location = new System.Drawing.Point(96, 56);
            this.cbTech.Name = "cbTech";
            this.cbTech.Size = new System.Drawing.Size(152, 21);
            this.cbTech.TabIndex = 3;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(96, 88);
            this.tbDesc.MaxLength = 50;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(152, 20);
            this.tbDesc.TabIndex = 5;
            // 
            // tbReaderID
            // 
            this.tbReaderID.Location = new System.Drawing.Point(96, 24);
            this.tbReaderID.MaxLength = 50;
            this.tbReaderID.Name = "tbReaderID";
            this.tbReaderID.Size = new System.Drawing.Size(152, 20);
            this.tbReaderID.TabIndex = 1;
            // 
            // lblTech
            // 
            this.lblTech.Location = new System.Drawing.Point(0, 56);
            this.lblTech.Name = "lblTech";
            this.lblTech.Size = new System.Drawing.Size(88, 23);
            this.lblTech.TabIndex = 2;
            this.lblTech.Text = "Technology:";
            this.lblTech.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReaderID
            // 
            this.lblReaderID.Location = new System.Drawing.Point(0, 24);
            this.lblReaderID.Name = "lblReaderID";
            this.lblReaderID.Size = new System.Drawing.Size(88, 23);
            this.lblReaderID.TabIndex = 0;
            this.lblReaderID.Text = "Reader ID:";
            this.lblReaderID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(8, 344);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbAntenna0
            // 
            this.gbAntenna0.Controls.Add(this.cbWorkTimeCounter0);
            this.gbAntenna0.Controls.Add(this.label1);
            this.gbAntenna0.Controls.Add(this.cbGate0);
            this.gbAntenna0.Controls.Add(this.lblGate0);
            this.gbAntenna0.Controls.Add(this.label6);
            this.gbAntenna0.Controls.Add(this.cbA0PrimLoc);
            this.gbAntenna0.Controls.Add(this.lblA0PrimDirect);
            this.gbAntenna0.Controls.Add(this.lbA0lPrimaryLoc);
            this.gbAntenna0.Controls.Add(this.cbA0PrimDirect);
            this.gbAntenna0.Controls.Add(this.label5);
            this.gbAntenna0.Location = new System.Drawing.Point(329, 18);
            this.gbAntenna0.Name = "gbAntenna0";
            this.gbAntenna0.Size = new System.Drawing.Size(320, 152);
            this.gbAntenna0.TabIndex = 9;
            this.gbAntenna0.TabStop = false;
            this.gbAntenna0.Text = "Antenna 0";
            // 
            // cbWorkTimeCounter0
            // 
            this.cbWorkTimeCounter0.Checked = true;
            this.cbWorkTimeCounter0.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWorkTimeCounter0.Location = new System.Drawing.Point(128, 120);
            this.cbWorkTimeCounter0.Name = "cbWorkTimeCounter0";
            this.cbWorkTimeCounter0.Size = new System.Drawing.Size(152, 24);
            this.cbWorkTimeCounter0.TabIndex = 16;
            this.cbWorkTimeCounter0.Text = "Work Time Counter";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(296, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 39;
            this.label1.Text = "*";
            // 
            // cbGate0
            // 
            this.cbGate0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGate0.Location = new System.Drawing.Point(128, 24);
            this.cbGate0.Name = "cbGate0";
            this.cbGate0.Size = new System.Drawing.Size(160, 21);
            this.cbGate0.TabIndex = 11;
            this.cbGate0.SelectedIndexChanged += new System.EventHandler(this.cbGate0_SelectedIndexChanged);
            // 
            // lblGate0
            // 
            this.lblGate0.Location = new System.Drawing.Point(16, 24);
            this.lblGate0.Name = "lblGate0";
            this.lblGate0.Size = new System.Drawing.Size(96, 23);
            this.lblGate0.TabIndex = 10;
            this.lblGate0.Text = "Gate:";
            this.lblGate0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(296, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 16);
            this.label6.TabIndex = 41;
            this.label6.Text = "*";
            // 
            // cbA0PrimLoc
            // 
            this.cbA0PrimLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbA0PrimLoc.Location = new System.Drawing.Point(128, 56);
            this.cbA0PrimLoc.Name = "cbA0PrimLoc";
            this.cbA0PrimLoc.Size = new System.Drawing.Size(160, 21);
            this.cbA0PrimLoc.TabIndex = 13;
            this.cbA0PrimLoc.SelectedIndexChanged += new System.EventHandler(this.cbA0PrimLoc_SelectedIndexChanged);
            // 
            // lblA0PrimDirect
            // 
            this.lblA0PrimDirect.Location = new System.Drawing.Point(12, 88);
            this.lblA0PrimDirect.Name = "lblA0PrimDirect";
            this.lblA0PrimDirect.Size = new System.Drawing.Size(100, 23);
            this.lblA0PrimDirect.TabIndex = 14;
            this.lblA0PrimDirect.Text = "Direction:";
            this.lblA0PrimDirect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbA0lPrimaryLoc
            // 
            this.lbA0lPrimaryLoc.Location = new System.Drawing.Point(16, 56);
            this.lbA0lPrimaryLoc.Name = "lbA0lPrimaryLoc";
            this.lbA0lPrimaryLoc.Size = new System.Drawing.Size(96, 23);
            this.lbA0lPrimaryLoc.TabIndex = 12;
            this.lbA0lPrimaryLoc.Text = "Primary Location:";
            this.lbA0lPrimaryLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA0PrimDirect
            // 
            this.cbA0PrimDirect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbA0PrimDirect.Location = new System.Drawing.Point(128, 88);
            this.cbA0PrimDirect.Name = "cbA0PrimDirect";
            this.cbA0PrimDirect.Size = new System.Drawing.Size(160, 21);
            this.cbA0PrimDirect.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(296, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 40;
            this.label5.Text = "*";
            // 
            // gbAntenna1
            // 
            this.gbAntenna1.Controls.Add(this.cbWorkTimeCounter1);
            this.gbAntenna1.Controls.Add(this.label8);
            this.gbAntenna1.Controls.Add(this.cbGate1);
            this.gbAntenna1.Controls.Add(this.lblGate1);
            this.gbAntenna1.Controls.Add(this.cbA1PrimDirect);
            this.gbAntenna1.Controls.Add(this.cbA1PrimLoc);
            this.gbAntenna1.Controls.Add(this.lblA1PrimDir);
            this.gbAntenna1.Controls.Add(this.lblA1PrimLoc);
            this.gbAntenna1.Controls.Add(this.label4);
            this.gbAntenna1.Controls.Add(this.label7);
            this.gbAntenna1.Location = new System.Drawing.Point(329, 176);
            this.gbAntenna1.Name = "gbAntenna1";
            this.gbAntenna1.Size = new System.Drawing.Size(320, 152);
            this.gbAntenna1.TabIndex = 26;
            this.gbAntenna1.TabStop = false;
            this.gbAntenna1.Text = "Antenna 1";
            // 
            // cbWorkTimeCounter1
            // 
            this.cbWorkTimeCounter1.Checked = true;
            this.cbWorkTimeCounter1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWorkTimeCounter1.Location = new System.Drawing.Point(128, 120);
            this.cbWorkTimeCounter1.Name = "cbWorkTimeCounter1";
            this.cbWorkTimeCounter1.Size = new System.Drawing.Size(152, 24);
            this.cbWorkTimeCounter1.TabIndex = 33;
            this.cbWorkTimeCounter1.Text = "Work Time Counter";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(296, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 16);
            this.label8.TabIndex = 43;
            this.label8.Text = "*";
            // 
            // cbGate1
            // 
            this.cbGate1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGate1.Location = new System.Drawing.Point(128, 24);
            this.cbGate1.Name = "cbGate1";
            this.cbGate1.Size = new System.Drawing.Size(160, 21);
            this.cbGate1.TabIndex = 28;
            // 
            // lblGate1
            // 
            this.lblGate1.Location = new System.Drawing.Point(8, 24);
            this.lblGate1.Name = "lblGate1";
            this.lblGate1.Size = new System.Drawing.Size(100, 23);
            this.lblGate1.TabIndex = 27;
            this.lblGate1.Text = "Gate:";
            this.lblGate1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA1PrimDirect
            // 
            this.cbA1PrimDirect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbA1PrimDirect.Location = new System.Drawing.Point(128, 88);
            this.cbA1PrimDirect.Name = "cbA1PrimDirect";
            this.cbA1PrimDirect.Size = new System.Drawing.Size(160, 21);
            this.cbA1PrimDirect.TabIndex = 32;
            // 
            // cbA1PrimLoc
            // 
            this.cbA1PrimLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbA1PrimLoc.Location = new System.Drawing.Point(128, 56);
            this.cbA1PrimLoc.Name = "cbA1PrimLoc";
            this.cbA1PrimLoc.Size = new System.Drawing.Size(160, 21);
            this.cbA1PrimLoc.TabIndex = 30;
            // 
            // lblA1PrimDir
            // 
            this.lblA1PrimDir.Location = new System.Drawing.Point(8, 88);
            this.lblA1PrimDir.Name = "lblA1PrimDir";
            this.lblA1PrimDir.Size = new System.Drawing.Size(100, 23);
            this.lblA1PrimDir.TabIndex = 31;
            this.lblA1PrimDir.Text = "Direction:";
            this.lblA1PrimDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblA1PrimLoc
            // 
            this.lblA1PrimLoc.Location = new System.Drawing.Point(8, 56);
            this.lblA1PrimLoc.Name = "lblA1PrimLoc";
            this.lblA1PrimLoc.Size = new System.Drawing.Size(104, 23);
            this.lblA1PrimLoc.TabIndex = 29;
            this.lblA1PrimLoc.Text = "Primary Location:";
            this.lblA1PrimLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(296, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 44;
            this.label4.Text = "*";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(296, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 16);
            this.label7.TabIndex = 45;
            this.label7.Text = "*";
            // 
            // gbConn
            // 
            this.gbConn.Controls.Add(this.lblIP);
            this.gbConn.Controls.Add(this.tbTel);
            this.gbConn.Controls.Add(this.lblTel);
            this.gbConn.Controls.Add(this.tbPort);
            this.gbConn.Controls.Add(this.rbGSM);
            this.gbConn.Controls.Add(this.lblDot2);
            this.gbConn.Controls.Add(this.lblDot1);
            this.gbConn.Controls.Add(this.lblDot0);
            this.gbConn.Controls.Add(this.tbIP2);
            this.gbConn.Controls.Add(this.tbIP1);
            this.gbConn.Controls.Add(this.tbIP0);
            this.gbConn.Controls.Add(this.label10);
            this.gbConn.Controls.Add(this.lblPort);
            this.gbConn.Controls.Add(this.lblConnType);
            this.gbConn.Controls.Add(this.rbIP);
            this.gbConn.Controls.Add(this.rbSerial);
            this.gbConn.Controls.Add(this.tbConnAddress);
            this.gbConn.Controls.Add(this.tbIP3);
            this.gbConn.Location = new System.Drawing.Point(8, 176);
            this.gbConn.Name = "gbConn";
            this.gbConn.Size = new System.Drawing.Size(303, 152);
            this.gbConn.TabIndex = 17;
            this.gbConn.TabStop = false;
            this.gbConn.Text = "Connection";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(8, 94);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(20, 13);
            this.lblIP.TabIndex = 30;
            this.lblIP.Text = "IP:";
            this.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbTel
            // 
            this.tbTel.Location = new System.Drawing.Point(187, 93);
            this.tbTel.MaxLength = 12;
            this.tbTel.Name = "tbTel";
            this.tbTel.Size = new System.Drawing.Size(80, 20);
            this.tbTel.TabIndex = 34;
            // 
            // lblTel
            // 
            this.lblTel.AutoSize = true;
            this.lblTel.Location = new System.Drawing.Point(154, 95);
            this.lblTel.Name = "lblTel";
            this.lblTel.Size = new System.Drawing.Size(34, 13);
            this.lblTel.TabIndex = 33;
            this.lblTel.Text = "Tel: +";
            this.lblTel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(110, 93);
            this.tbPort.MaxLength = 2;
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(42, 20);
            this.tbPort.TabIndex = 32;
            // 
            // rbGSM
            // 
            this.rbGSM.AutoSize = true;
            this.rbGSM.Location = new System.Drawing.Point(213, 45);
            this.rbGSM.Name = "rbGSM";
            this.rbGSM.Size = new System.Drawing.Size(49, 17);
            this.rbGSM.TabIndex = 21;
            this.rbGSM.TabStop = true;
            this.rbGSM.Text = "GSM";
            this.rbGSM.UseVisualStyleBackColor = true;
            this.rbGSM.CheckedChanged += new System.EventHandler(this.rbGSM_CheckedChanged);
            // 
            // lblDot2
            // 
            this.lblDot2.Location = new System.Drawing.Point(225, 90);
            this.lblDot2.Name = "lblDot2";
            this.lblDot2.Size = new System.Drawing.Size(8, 23);
            this.lblDot2.TabIndex = 28;
            this.lblDot2.Text = ".";
            this.lblDot2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblDot1
            // 
            this.lblDot1.Location = new System.Drawing.Point(182, 90);
            this.lblDot1.Name = "lblDot1";
            this.lblDot1.Size = new System.Drawing.Size(8, 23);
            this.lblDot1.TabIndex = 26;
            this.lblDot1.Text = ".";
            this.lblDot1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblDot0
            // 
            this.lblDot0.Location = new System.Drawing.Point(141, 90);
            this.lblDot0.Name = "lblDot0";
            this.lblDot0.Size = new System.Drawing.Size(10, 23);
            this.lblDot0.TabIndex = 24;
            this.lblDot0.Text = ".";
            this.lblDot0.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tbIP2
            // 
            this.tbIP2.Location = new System.Drawing.Point(191, 94);
            this.tbIP2.MaxLength = 3;
            this.tbIP2.Name = "tbIP2";
            this.tbIP2.Size = new System.Drawing.Size(32, 20);
            this.tbIP2.TabIndex = 27;
            this.tbIP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbIP1
            // 
            this.tbIP1.Location = new System.Drawing.Point(150, 94);
            this.tbIP1.MaxLength = 3;
            this.tbIP1.Name = "tbIP1";
            this.tbIP1.Size = new System.Drawing.Size(32, 20);
            this.tbIP1.TabIndex = 25;
            this.tbIP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbIP0
            // 
            this.tbIP0.Location = new System.Drawing.Point(110, 94);
            this.tbIP0.MaxLength = 3;
            this.tbIP0.Name = "tbIP0";
            this.tbIP0.Size = new System.Drawing.Size(32, 20);
            this.tbIP0.TabIndex = 23;
            this.tbIP0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(273, 93);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 16);
            this.label10.TabIndex = 35;
            this.label10.Text = "*";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(8, 94);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 22;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConnType
            // 
            this.lblConnType.AutoSize = true;
            this.lblConnType.Location = new System.Drawing.Point(8, 45);
            this.lblConnType.Name = "lblConnType";
            this.lblConnType.Size = new System.Drawing.Size(91, 13);
            this.lblConnType.TabIndex = 18;
            this.lblConnType.Text = "Connection Type:";
            this.lblConnType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbIP
            // 
            this.rbIP.AutoSize = true;
            this.rbIP.Location = new System.Drawing.Point(110, 45);
            this.rbIP.Name = "rbIP";
            this.rbIP.Size = new System.Drawing.Size(35, 17);
            this.rbIP.TabIndex = 19;
            this.rbIP.Text = "IP";
            this.rbIP.CheckedChanged += new System.EventHandler(this.rbIP_CheckedChanged);
            // 
            // rbSerial
            // 
            this.rbSerial.AutoSize = true;
            this.rbSerial.Location = new System.Drawing.Point(155, 45);
            this.rbSerial.Name = "rbSerial";
            this.rbSerial.Size = new System.Drawing.Size(51, 17);
            this.rbSerial.TabIndex = 20;
            this.rbSerial.Text = "Serial";
            this.rbSerial.CheckedChanged += new System.EventHandler(this.rbSerial_CheckedChanged);
            // 
            // tbConnAddress
            // 
            this.tbConnAddress.Location = new System.Drawing.Point(110, 93);
            this.tbConnAddress.MaxLength = 15;
            this.tbConnAddress.Name = "tbConnAddress";
            this.tbConnAddress.Size = new System.Drawing.Size(152, 20);
            this.tbConnAddress.TabIndex = 31;
            // 
            // tbIP3
            // 
            this.tbIP3.Location = new System.Drawing.Point(234, 94);
            this.tbIP3.MaxLength = 3;
            this.tbIP3.Name = "tbIP3";
            this.tbIP3.Size = new System.Drawing.Size(32, 20);
            this.tbIP3.TabIndex = 29;
            this.tbIP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(0, 88);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(88, 23);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(574, 344);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(256, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 38;
            this.label3.Text = "*";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(256, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 16);
            this.label9.TabIndex = 37;
            this.label9.Text = "*";
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.rbDisabled);
            this.gbStatus.Controls.Add(this.rbEnabled);
            this.gbStatus.Location = new System.Drawing.Point(8, 112);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(303, 56);
            this.gbStatus.TabIndex = 6;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Status";
            // 
            // rbDisabled
            // 
            this.rbDisabled.Location = new System.Drawing.Point(136, 20);
            this.rbDisabled.Name = "rbDisabled";
            this.rbDisabled.Size = new System.Drawing.Size(80, 24);
            this.rbDisabled.TabIndex = 8;
            this.rbDisabled.Text = "Disabled";
            // 
            // rbEnabled
            // 
            this.rbEnabled.Checked = true;
            this.rbEnabled.Location = new System.Drawing.Point(24, 20);
            this.rbEnabled.Name = "rbEnabled";
            this.rbEnabled.Size = new System.Drawing.Size(80, 24);
            this.rbEnabled.TabIndex = 7;
            this.rbEnabled.TabStop = true;
            this.rbEnabled.Text = "Enabled";
            // 
            // ReadersAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(659, 374);
            this.ControlBox = false;
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddvance);
            this.Controls.Add(this.cbTech);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbReaderID);
            this.Controls.Add(this.lblTech);
            this.Controls.Add(this.lblReaderID);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbAntenna0);
            this.Controls.Add(this.gbAntenna1);
            this.Controls.Add(this.gbConn);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.btnUpdate);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(667, 408);
            this.MinimumSize = new System.Drawing.Size(667, 408);
            this.Name = "ReadersAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "New Reader";
            this.Load += new System.EventHandler(this.ReadersAdd_Load);
            this.Closed += new System.EventHandler(this.ReadersAdd_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ReadersAdd_KeyUp);
            this.gbAntenna0.ResumeLayout(false);
            this.gbAntenna1.ResumeLayout(false);
            this.gbConn.ResumeLayout(false);
            this.gbConn.PerformLayout();
            this.gbStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void ReadersAdd_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                InitialiseObserverClient();

                populateGateCb(cbGate0);
                populateGateCb(cbGate1);
                populateLocationCb(cbA0PrimLoc);
                populateLocationCb(cbA1PrimLoc);
                populateTechCb();
                populateDirectionCb(cbA0PrimDirect);
                populateDirectionCb(cbA1PrimDirect);

                if (currentReader.ReaderID != -1)
                {
                    fillDataFields();
                }
            }
            catch (Exception ex) {

                log.writeLog(DateTime.Now + " ReadersAdd.ReadersAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally{
            this.Cursor = Cursors.Arrow;
            }
		}

		private void populateTechCb()
		{
			try
			{
				this.cbTech.Items.AddRange(
					new object[] 
					{
						Constants.TechTypeHitag1,
						Constants.TechTypeHitag2,
						Constants.TechTypeMifare,
						Constants.TechTypeICode
					});

				//this.cbTech.Items.Insert(0, rm.GetString("all", culture));
				//this.cbTech.SelectedIndex = cbTech.FindStringExact(ConfigurationManager.AppSettings["TechTypeMifare"].ToString());			
				this.cbTech.SelectedIndex = cbTech.FindStringExact(Constants.TechTypeMifare);			
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReadersAdd.populateTechCb(): " + ex.Message + "\n");
				MessageBox.Show("Exception: " + ex.Message);
			}
		}

		private void populateLocationCb(ComboBox cb)
		{
			try
			{
				Location location = new Location();
                location.LocTO.Status = Constants.DefaultStateActive.Trim();
				List<LocationTO> locations = location.Search();

				locations.Insert(0, new LocationTO(-1, rm.GetString("all", culture), "", -1, -1, Constants.DefaultStateActive.ToString()));

				cb.DataSource = locations;
				cb.DisplayMember = "Name";
				cb.ValueMember = "LocationID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReaderAdd.populateLocationCb(...): " + ex.Message + "\n");
				MessageBox.Show("Exception: " + ex.Message);
			}
		}

		private void populateGateCb(ComboBox cb)
		{
			try
			{				
				List<GateTO> gates = new Gate().Search();

				gates.Insert(0, new GateTO(-1, rm.GetString("all", culture), "", new DateTime(0), -1, -1));

				cb.DataSource = gates;
				cb.DisplayMember = "Name";
				cb.ValueMember = "GateID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReaderAdd.populateGateCb(...): " + ex.Message + "\n");
				MessageBox.Show("Exception: " + ex.Message);
			}
		}

		private void btnAddvance_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ReaderAdvanceSettings ras = new ReaderAdvanceSettings(currentReader, (int)cbGate0.SelectedValue,
                    (int)cbA0PrimLoc.SelectedValue, cbA0PrimDirect.SelectedItem.ToString(), (int)cbGate1.SelectedValue,
                    (int)cbA1PrimLoc.SelectedValue, cbA1PrimDirect.SelectedItem.ToString());
                ras.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderAdd.btnAddvance_Click(): " + ex.Message + "\n");
                MessageBox.Show("Exception: " + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (tbReaderID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("messageReaderIDNotSet", culture));
                    return;
                }
                else
                {
                    try
                    {
                        currentReader.ReaderID = Int32.Parse(tbReaderID.Text.Trim());
                    }
                    catch
                    {
                        MessageBox.Show(rm.GetString("messageReaderIDNotNum", culture));
                        return;
                    }
                }

                //if(cbTech.SelectedIndex <= 0)
                if (cbTech.SelectedIndex < 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderTechType", culture));
                    return;
                }
                else
                {
                    currentReader.TechType = cbTech.SelectedItem.ToString();
                }

                currentReader.Description = tbDesc.Text.Trim();

                if (rbIP.Checked)
                {
                    //currentReader.ConnectionType = ConfigurationManager.AppSettings["ConnTypeIP"];
                    currentReader.ConnectionType = Constants.ConnTypeIP;
                    if (tbIP0.Text.Trim().Equals("") &&
                        tbIP1.Text.Trim().Equals("") &&
                        tbIP2.Text.Trim().Equals("") &&
                        tbIP3.Text.Trim().Equals(""))
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                    else
                    {
                        currentReader.ConnectionAddress = tbIP0.Text.Trim() + "." +
                        tbIP1.Text.Trim() + "." +
                        tbIP2.Text.Trim() + "." +
                        tbIP3.Text.Trim();
                    }
                }
                else if (rbSerial.Checked)
                {
                    //currentReader.ConnectionType = ConfigurationManager.AppSettings["ConnTypeSerial"];
                    currentReader.ConnectionType = Constants.ConnTypeSerial;

                    if (tbConnAddress.Text.Trim().Equals(""))
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                    else
                    {
                        currentReader.ConnectionAddress = tbConnAddress.Text.Trim();
                    }
                }
                else
                {
                    currentReader.ConnectionType = Constants.ConnTypeGSM;

                    if (tbPort.Text.Trim().Equals("") || tbTel.Text.Trim().Equals("") || !validateTel())
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                    else
                    {
                        currentReader.ConnectionAddress = tbPort.Text.Trim() + " " + tbTel.Text.Replace(" ", "");
                    }
                }

                currentReader.A0IsCounter = cbWorkTimeCounter0.Checked ? 1 : 0;

                if (cbGate0.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderA0Gate", culture));
                    return;
                }
                else
                {
                    currentReader.A0GateID = (int)cbGate0.SelectedValue;
                }

                if (cbA0PrimLoc.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderA0PrimLoc", culture));
                    return;
                }
                else
                {
                    currentReader.A0LocID = (int)cbA0PrimLoc.SelectedValue;
                }

                if (cbA0PrimDirect.SelectedIndex <= 0)
                {

                    MessageBox.Show(rm.GetString("messageReaderA0PrimDir", culture));
                    return;
                }
                else
                {
                    currentReader.A0Direction = cbA0PrimDirect.SelectedItem.ToString();
                }

                currentReader.A1IsCounter = cbWorkTimeCounter1.Checked ? 1 : 0;

                if (cbGate1.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderA1Gate", culture));
                    return;
                }
                else
                {
                    currentReader.A1GateID = (int)cbGate1.SelectedValue;
                }

                if (cbA1PrimLoc.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderA1PrimLoc", culture));
                    return;
                }
                else
                {
                    currentReader.A1LocID = (int)cbA1PrimLoc.SelectedValue;
                }
                if (cbA1PrimDirect.SelectedIndex <= 0)
                {

                    MessageBox.Show(rm.GetString("messageReaderA1PrimDir", culture));
                    return;
                }
                else
                {
                    currentReader.A1Direction = cbA1PrimDirect.SelectedItem.ToString();
                }

                // Check Connection Address
                if (rbIP.Checked)
                {
                    string ipString = tbIP0.Text.Trim() + "." +
                                tbIP1.Text.Trim() + "." +
                                tbIP2.Text.Trim() + "." +
                                tbIP3.Text.Trim();

                    // Check IP
                    if (this.CheckIP(ipString))
                    {
                        currentReader.ConnectionAddress = ipString;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                }
                /*else
                {
                    if (!tbConnAddress.Text.Trim().Equals(""))
                    {
                        currentReader.ConnectionAddress = tbConnAddress.Text.Trim();
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                }*/

                if (rbEnabled.Checked)
                {
                    currentReader.Status = Constants.readerStatusEnabled;
                }
                else
                {
                    currentReader.Status = Constants.readerStatusDisabled;
                }

                Reader r = new Reader();
                r.RdrTO = currentReader;
                int inserted = r.Save();

                if (inserted > 0)
                {
                    MessageBox.Show(rm.GetString("readerSaved", culture));
                    // Reload List View
                    List<ReaderTO> readerList = new Reader().Search();

                    Controller.SettingsChanged(currentReader);
                    this.Close();
                }
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    MessageBox.Show(rm.GetString("messageReaderIDexists", culture));
                }
                else
                {
                    MessageBox.Show(sqlex.Message);
                    log.writeLog(DateTime.Now + " ReaderAdd.btnSave_Click(): " + sqlex.Message + "\n");
                }
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    MessageBox.Show(rm.GetString("messageReaderIDexists", culture));
                }
                else
                {
                    MessageBox.Show(mysqlex.Message);
                    log.writeLog(DateTime.Now + " ReaderAdd.btnSave_Click(): " + mysqlex.Message + "\n");
                }
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderAdd.btnSave_Click(...): " + ex.Message + "\n");
                //throw new Exception("Exception: " + ex.Message);
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //if(cbTech.SelectedIndex <= 0)
                if (cbTech.SelectedIndex < 0)
                {
                    //MessageBox.Show("Please select valid Technology type");
                    MessageBox.Show(rm.GetString("messageReaderTechType", culture));
                    return;
                }
                else
                {
                    currentReader.TechType = cbTech.SelectedItem.ToString();
                }

                currentReader.Description = tbDesc.Text.Trim();

                if (rbIP.Checked)
                {
                    //currentReader.ConnectionType = ConfigurationManager.AppSettings["ConnTypeIP"];
                    currentReader.ConnectionType = Constants.ConnTypeIP;
                    if (tbIP0.Text.Trim().Equals("") &&
                        tbIP1.Text.Trim().Equals("") &&
                        tbIP2.Text.Trim().Equals("") &&
                        tbIP3.Text.Trim().Equals(""))
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                    else
                    {
                        currentReader.ConnectionAddress = tbIP0.Text.Trim() + "." +
                            tbIP1.Text.Trim() + "." +
                            tbIP2.Text.Trim() + "." +
                            tbIP3.Text.Trim();
                    }
                }
                else if (rbSerial.Checked)
                {
                    currentReader.ConnectionType = Constants.ConnTypeSerial;
                    if (tbConnAddress.Text.Trim().Equals(""))
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                    else
                    {
                        currentReader.ConnectionAddress = tbConnAddress.Text.Trim();
                    }
                }
                else
                {
                    currentReader.ConnectionType = Constants.ConnTypeGSM;

                    if (tbPort.Text.Trim().Equals("") || tbTel.Text.Trim().Equals("") || !validateTel())
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                    else
                    {
                        currentReader.ConnectionAddress = tbPort.Text.Trim() + " " + tbTel.Text.Replace(" ", "");
                    }
                }

                currentReader.A0IsCounter = cbWorkTimeCounter0.Checked ? 1 : 0;

                if (cbGate0.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderA0Gate", culture));
                    return;
                }
                else
                {
                    currentReader.A0GateID = (int)cbGate0.SelectedValue;
                }

                if (cbA0PrimLoc.SelectedIndex <= 0)
                {
                    //MessageBox.Show("Please select valid Primary Location for Antenna 0");
                    MessageBox.Show(rm.GetString("messageReaderA0PrimLoc", culture));
                    return;
                }
                else
                {
                    currentReader.A0LocID = (int)cbA0PrimLoc.SelectedValue;
                }

                if (cbA0PrimDirect.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderA0PrimDir", culture));
                    return;
                }
                else
                {
                    currentReader.A0Direction = cbA0PrimDirect.SelectedItem.ToString();
                }

                // Check Connection Address

                if (rbIP.Checked)
                {
                    string ipString = tbIP0.Text.Trim() + "." +
                        tbIP1.Text.Trim() + "." +
                        tbIP2.Text.Trim() + "." +
                        tbIP3.Text.Trim();

                    // Check IP
                    if (this.CheckIP(ipString))
                    {
                        currentReader.ConnectionAddress = ipString;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                }
                /*else
                {
                    if (!tbConnAddress.Text.Trim().Equals(""))
                    {
                        currentReader.ConnectionAddress = tbConnAddress.Text.Trim();
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("readerConnNotValid", culture));
                        return;
                    }
                }*/

                currentReader.A1IsCounter = cbWorkTimeCounter1.Checked ? 1 : 0;

                if (cbGate1.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderA1Gate", culture));
                    return;
                }
                else
                {
                    currentReader.A1GateID = (int)cbGate1.SelectedValue;
                }

                if (cbA1PrimLoc.SelectedIndex <= 0)
                {
                    //MessageBox.Show("Please select valid Primary Location for Antenna 1");
                    MessageBox.Show(rm.GetString("messageReaderA1PrimLoc", culture));
                    return;
                }
                else
                {
                    currentReader.A1LocID = (int)cbA1PrimLoc.SelectedValue;
                }
                if (cbA1PrimDirect.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("messageReaderA1PrimLoc", culture));
                    return;
                }
                else
                {
                    currentReader.A1Direction = cbA1PrimDirect.SelectedItem.ToString();
                }

                if (rbEnabled.Checked)
                {
                    currentReader.Status = Constants.readerStatusEnabled;
                }
                else
                {
                    currentReader.Status = Constants.readerStatusDisabled;
                }

                bool updated = new Reader().Update(currentReader);

                if (updated)
                {
                    MessageBox.Show(rm.GetString("readerUpdated", culture));
                    // Reload List View
                    List<ReaderTO> readerList = new Reader().Search();
                    Controller.SettingsChanged(currentReader);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderAdd.btnUpdate_Click(...): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		
		}

		public void InitialiseObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.SettingsChangeEvent);
		}

		private void populateDirectionCb(ComboBox cb)
		{
			try
			{
				cb.Items.Add(rm.GetString("all", culture));
				cb.Items.Add(Constants.DirectionIn);
				cb.Items.Add(Constants.DirectionOut);
				//cb.Items.Add(Constants.DirectionInOut);

				cb.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReaderAdd.populateDirectionCb(ComboBox cb): " + ex.Message + "\n");
				MessageBox.Show("Exception: " + ex.Message);
			}
		}

		public void SettingsChangeEvent(object sender, NotificationEventArgs e)
		{
			if (e.reader != null)
			{
				cbGate0.SelectedValue = e.reader.A0GateID;
				cbA0PrimLoc.SelectedValue = e.reader.A0LocID;
				cbA0PrimDirect.SelectedIndex = cbA0PrimDirect.FindStringExact(e.reader.A0Direction);

				cbGate1.SelectedValue = e.reader.A1GateID;
				cbA1PrimLoc.SelectedValue = e.reader.A1LocID;
				cbA1PrimDirect.SelectedIndex = cbA0PrimDirect.FindStringExact(e.reader.A1Direction);

				this.Invalidate();
			}
		}

		private void setLanguage()
		{
			try
			{
				if (currentReader.ReaderID == -1)
				{
					this.Text = rm.GetString("addReader", culture);
				}
				else
				{
					this.Text = rm.GetString("updateReader", culture);
				}
				gbConn.Text = rm.GetString("lblConnection", culture);
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnAddvance.Text = rm.GetString("btnAddvance", culture);
				lblReaderID.Text = rm.GetString("lblReaderID", culture);
				lblTech.Text = rm.GetString("lblTech", culture);
				lblDesc.Text = rm.GetString("lblDesc", culture);
				lblConnType.Text = rm.GetString("lblConnType", culture);
				rbSerial.Text = rm.GetString("rbSerial", culture);
				lblGate0.Text = rm.GetString("lblGate", culture);
				lbA0lPrimaryLoc.Text = rm.GetString("lblPrimaryLoc", culture);
				lblA0PrimDirect.Text = rm.GetString("lblPrimDirect", culture);
				lblGate1.Text = rm.GetString("lblGate", culture);
				lblA1PrimLoc.Text = rm.GetString("lblPrimaryLoc", culture);
				lblA1PrimDir.Text = rm.GetString("lblPrimDirect", culture);
				gbAntenna0.Text = rm.GetString("gbAntenna0", culture);
				gbAntenna1.Text = rm.GetString("gbAntenna1", culture);
				cbWorkTimeCounter0.Text = rm.GetString("cbWorkTimeCounter1", culture);
				cbWorkTimeCounter1.Text = rm.GetString("cbWorkTimeCounter1", culture);
			
				gbStatus.Text = rm.GetString("hdrStatus", culture);
				rbEnabled.Text = rm.GetString("rbEnabled", culture);
				rbDisabled.Text = rm.GetString("rbDisabled", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReaderAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show("Exception: " + ex.Message);
			}
		}

        private void fillDataFields()
        {
            tbReaderID.Text = currentReader.ReaderID.ToString();
            tbDesc.Text = currentReader.Description;

            cbTech.SelectedIndex = cbTech.FindStringExact(currentReader.TechType.ToString());

            if (currentReader.ConnectionType.Equals(Constants.ConnTypeIP))
            {
                rbIP.Checked = true;
                string[] oct = currentReader.ConnectionAddress.Split('.');
                if (oct.Length == 4)
                {
                    this.tbConnAddress.Visible = false;
                    //this.tbIP0.Visible = true;
                    this.tbIP0.Text = oct[0];

                    //this.tbIP1.Visible = true;
                    this.tbIP1.Text = oct[1];
                    //this.tbIP2.Visible = true;
                    this.tbIP2.Text = oct[2];
                    //this.tbIP3.Visible = true;
                    this.tbIP3.Text = oct[3];

                    /*this.lblDot0.Visible = true;
                    this.lblDot1.Visible = true;
                    this.lblDot2.Visible = true;*/
                }
            }
            else if (currentReader.ConnectionType.Equals(Constants.ConnTypeSerial))
            {
                rbSerial.Checked = true;
                tbConnAddress.Text = currentReader.ConnectionAddress;
                
                /*tbConnAddress.Visible = true;

                this.tbIP0.Visible = false;
                this.tbIP1.Visible = false;
                this.tbIP2.Visible = false;
                this.tbIP3.Visible = false;

                this.lblDot0.Visible = false;
                this.lblDot1.Visible = false;
                this.lblDot2.Visible = false;*/
            }
            else
            {
                rbGSM.Checked = true;
                int index = currentReader.ConnectionAddress.IndexOf(' ');
                if (index >= 0)
                {
                    tbPort.Text = currentReader.ConnectionAddress.Substring(0, index);
                    tbTel.Text = currentReader.ConnectionAddress.Substring(index + 1, currentReader.ConnectionAddress.Length - index - 1);
                }                
            }

            cbGate0.SelectedValue = currentReader.A0GateID;
            cbA0PrimLoc.SelectedValue = currentReader.A0LocID;
            cbGate1.SelectedValue = currentReader.A1GateID;
            cbA1PrimLoc.SelectedValue = currentReader.A1LocID;
            cbA0PrimDirect.SelectedIndex = cbA0PrimDirect.FindStringExact(currentReader.A0Direction);
            cbA1PrimDirect.SelectedIndex = cbA1PrimDirect.FindStringExact(currentReader.A1Direction);
            cbWorkTimeCounter0.Checked = (currentReader.A0IsCounter == 1);
            cbWorkTimeCounter1.Checked = (currentReader.A1IsCounter == 1);

            if (currentReader.Status.Equals(Constants.readerStatusEnabled))
                rbEnabled.Checked = true;
            else
                rbDisabled.Checked = true;
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
                log.writeLog(DateTime.Now + " ReadersAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void ReadersAdd_Closed(object sender, System.EventArgs e)
		{
			Controller.DettachFromNotifier(this.observerClient);
		}

		private void rbSerial_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (rbSerial.Checked)
                {
                    this.Cursor = Cursors.WaitCursor;

                    this.tbConnAddress.Visible = true;
                    this.lblPort.Visible = true;

                    this.lblIP.Visible = false;
                    this.tbIP0.Visible = false;
                    this.tbIP1.Visible = false;
                    this.tbIP2.Visible = false;
                    this.tbIP3.Visible = false;

                    this.lblDot0.Visible = false;
                    this.lblDot1.Visible = false;
                    this.lblDot2.Visible = false;

                    this.tbPort.Visible = false;
                    this.lblTel.Visible = false;
                    this.tbTel.Visible = false;

                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReadersAdd.rbSerial_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void rbIP_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbIP.Checked)
                {
                    this.tbConnAddress.Visible = false;

                    this.lblPort.Visible = false;
                    this.tbPort.Visible = false;
                    this.lblTel.Visible = false;
                    this.tbTel.Visible = false;

                    this.lblIP.Visible = true;
                    this.tbIP0.Visible = true;
                    this.tbIP1.Visible = true;
                    this.tbIP2.Visible = true;
                    this.tbIP3.Visible = true;

                    this.lblDot0.Visible = true;
                    this.lblDot1.Visible = true;
                    this.lblDot2.Visible = true;

                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReadersAdd.rbIP_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private bool CheckIP(string ipString)
		{
			bool isOK = false;
			try
			{
				// Check IP Address
				try
				{
					if ((ipString.IndexOf("255") == -1) 
						&& (!ipString.Trim().StartsWith("0.")) 
						&& (!ipString.Trim().EndsWith(".0")))
					{
						IPAddress ip = IPAddress.Parse(ipString);
						isOK = true;
					}
				}
				catch
				{
					isOK = false;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReadersAdd.CheckIP(): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
			return isOK;
		}


		private void cbA0PrimLoc_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                if (cbA0PrimLoc.SelectedValue is int)
                {
                    this.Cursor = Cursors.WaitCursor;
                    cbA1PrimLoc.SelectedValue = cbA0PrimLoc.SelectedValue;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReadersAdd.cbA0PrimLoc_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbGate0_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                if (cbGate0.SelectedValue is int)
                {
                    this.Cursor = Cursors.WaitCursor;
                    cbGate1.SelectedValue = cbGate0.SelectedValue;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReadersAdd.cbGate0_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void rbGSM_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

            if (rbGSM.Checked)
            {
                this.tbConnAddress.Visible = false;

                this.lblPort.Visible = true;
                this.tbPort.Visible = true;
                this.lblTel.Visible = true;
                this.tbTel.Visible = true;

                this.lblIP.Visible = false;
                this.tbIP0.Visible = false;
                this.tbIP1.Visible = false;
                this.tbIP2.Visible = false;
                this.tbIP3.Visible = false;

                this.lblDot0.Visible = false;
                this.lblDot1.Visible = false;
                this.lblDot2.Visible = false;

                this.Invalidate();
            }
            }catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReadersAdd.cbGate0_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool validateTel()
        {
            try
            {
                bool isTelNum = true;
                string tel = tbTel.Text.Replace(" ", "");
                for (int i = 0; i < tel.Length; i++)
                {
                    int res;
                    if (!Int32.TryParse(tel.Substring(i, 1), out res))
                    {
                        isTelNum = false;
                        break;
                    }
                }

                return isTelNum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReadersAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ReadersAdd.ReadersAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
