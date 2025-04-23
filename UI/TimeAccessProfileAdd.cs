using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;


namespace UI
{
	/// <summary>
	/// Summary description for TimeAccessProfileAdd.
	/// </summary>
	public class TimeAccessProfileAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPageIN;
		private System.Windows.Forms.TabPage tabPageOUT;
		private System.Windows.Forms.Panel panelIN;
		private System.Windows.Forms.Panel panelOUT;
		private System.Windows.Forms.Button btnUnCheckAllIN;
		private System.Windows.Forms.Button btnCheckAllIN;
		private System.Windows.Forms.Button btnUnCheckAllOUT;
		private System.Windows.Forms.Button btnCheckAllOUT;
		private System.Windows.Forms.Label lblMonOut;
		private System.Windows.Forms.Label lblTueOut;
		private System.Windows.Forms.Label lblWedOut;
		private System.Windows.Forms.Label lblThrOut;
		private System.Windows.Forms.Label lblFriOut;
		private System.Windows.Forms.Label lblSatOut;
		private System.Windows.Forms.Label lblSunOut;
		private System.Windows.Forms.Label lblSunIn;
		private System.Windows.Forms.Label lblSatIn;
		private System.Windows.Forms.Label lblFriIn;
		private System.Windows.Forms.Label lblThrIn;
		private System.Windows.Forms.Label lblWedIn;
		private System.Windows.Forms.Label lblTueIn;
		private System.Windows.Forms.Label lblMonIn;
		private System.Windows.Forms.Label lbl0In;
		private System.Windows.Forms.Label lbl1In;
		private System.Windows.Forms.Label lbl2In;
		private System.Windows.Forms.Label lbl3In;
		private System.Windows.Forms.Label lbl4In;
		private System.Windows.Forms.Label lbl5In;
		private System.Windows.Forms.Label lbl6In;		
		private System.Windows.Forms.Label lbl7In;
		private System.Windows.Forms.Label lbl8In;
		private System.Windows.Forms.Label lbl9In;		
		private System.Windows.Forms.Label lbl10In;
		private System.Windows.Forms.Label lbl11In;
		private System.Windows.Forms.Label lbl12In;
		private System.Windows.Forms.Label lbl13In;
		private System.Windows.Forms.Label lbl14In;
		private System.Windows.Forms.Label lbl15In;
		private System.Windows.Forms.Label lbl16In;
		private System.Windows.Forms.Label lbl17In;
		private System.Windows.Forms.Label lbl18In;
		private System.Windows.Forms.Label lbl19In;
		private System.Windows.Forms.Label lbl20In;
		private System.Windows.Forms.Label lbl21In;
		private System.Windows.Forms.Label lbl22In;
		private System.Windows.Forms.Label lbl23In;
		private System.Windows.Forms.Label lbl23Out;
		private System.Windows.Forms.Label lbl22Out;
		private System.Windows.Forms.Label lbl21Out;
		private System.Windows.Forms.Label lbl20Out;
		private System.Windows.Forms.Label lbl19Out;
		private System.Windows.Forms.Label lbl18Out;
		private System.Windows.Forms.Label lbl17Out;
		private System.Windows.Forms.Label lbl16Out;
		private System.Windows.Forms.Label lbl15Out;
		private System.Windows.Forms.Label lbl14Out;
		private System.Windows.Forms.Label lbl13Out;
		private System.Windows.Forms.Label lbl12Out;
		private System.Windows.Forms.Label lbl11Out;
		private System.Windows.Forms.Label lbl10Out;
		private System.Windows.Forms.Label lbl9Out;
		private System.Windows.Forms.Label lbl8Out;
		private System.Windows.Forms.Label lbl7Out;
		private System.Windows.Forms.Label lbl6Out;
		private System.Windows.Forms.Label lbl5Out;
		private System.Windows.Forms.Label lbl4Out;
		private System.Windows.Forms.Label lbl3Out;
		private System.Windows.Forms.Label lbl2Out;
		private System.Windows.Forms.Label lbl1Out;
		private System.Windows.Forms.Label lbl0Out;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		TimeAccessProfile currentTimeAccessProfile = null;

		ApplUserTO logInUser;		
		ResourceManager rm;
		private CultureInfo culture;								
		DebugLog log;

		TimeAccessProfileCell cellMonIN;
		TimeAccessProfileCell cellTueIN;
		TimeAccessProfileCell cellWedIN;
		TimeAccessProfileCell cellThrIN;
		TimeAccessProfileCell cellFriIN;
		TimeAccessProfileCell cellSatIN;
		TimeAccessProfileCell cellSunIN;
		TimeAccessProfileCell cellMonOUT;
		TimeAccessProfileCell cellTueOUT;
		TimeAccessProfileCell cellWedOUT;
		TimeAccessProfileCell cellThrOUT;
		TimeAccessProfileCell cellFriOUT;
		TimeAccessProfileCell cellSatOUT;
		TimeAccessProfileCell cellSunOUT;

		string h0 = ""; string h1 = ""; 
		string h2 = ""; string h3 = ""; 
		string h4 = "";	string h5 = ""; 
		string h6 = ""; string h7 = ""; 
		string h8 = ""; string h9 = "";
		string h10 = ""; string h11 = ""; 
		string h12 = ""; string h13 = ""; 
		string h14 = ""; string h15 = ""; 
		string h16 = ""; string h17 = ""; 
		string h18 = ""; string h19 = "";
		string h20 = ""; string h21 = ""; 
		string h22 = ""; string h23 = "";

		public TimeAccessProfileAdd()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentTimeAccessProfile = new TimeAccessProfile();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(TimeAccessProfileAdd).Assembly);
			setLanguage();

			btnUpdate.Visible = false;

			populateCheckBoxScreen();
		}

		public TimeAccessProfileAdd(TimeAccessProfile timeAccessProfile)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentTimeAccessProfile = new TimeAccessProfile(timeAccessProfile.TimeAccessProfileId, timeAccessProfile.Name, timeAccessProfile.Description);
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(TimeAccessProfileAdd).Assembly);
			setLanguage();

			tbName.Text = timeAccessProfile.Name.Trim();
			tbDesc.Text = timeAccessProfile.Description.Trim();

			btnSave.Visible = false;

			populateCheckBoxScreen();
			getDetails();			
			if ((currentTimeAccessProfile.TimeAccessProfileId == 0) || (currentTimeAccessProfile.TimeAccessProfileId == 1))
				disableAll();
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
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageIN = new System.Windows.Forms.TabPage();
            this.lbl23In = new System.Windows.Forms.Label();
            this.lbl22In = new System.Windows.Forms.Label();
            this.lbl21In = new System.Windows.Forms.Label();
            this.lbl20In = new System.Windows.Forms.Label();
            this.lbl19In = new System.Windows.Forms.Label();
            this.lbl18In = new System.Windows.Forms.Label();
            this.lbl17In = new System.Windows.Forms.Label();
            this.lbl16In = new System.Windows.Forms.Label();
            this.lbl15In = new System.Windows.Forms.Label();
            this.lbl14In = new System.Windows.Forms.Label();
            this.lbl13In = new System.Windows.Forms.Label();
            this.lbl12In = new System.Windows.Forms.Label();
            this.lbl11In = new System.Windows.Forms.Label();
            this.lbl10In = new System.Windows.Forms.Label();
            this.lbl9In = new System.Windows.Forms.Label();
            this.lbl8In = new System.Windows.Forms.Label();
            this.lbl7In = new System.Windows.Forms.Label();
            this.lbl6In = new System.Windows.Forms.Label();
            this.lbl5In = new System.Windows.Forms.Label();
            this.lbl4In = new System.Windows.Forms.Label();
            this.lbl3In = new System.Windows.Forms.Label();
            this.lbl2In = new System.Windows.Forms.Label();
            this.lbl1In = new System.Windows.Forms.Label();
            this.lbl0In = new System.Windows.Forms.Label();
            this.btnCheckAllIN = new System.Windows.Forms.Button();
            this.btnUnCheckAllIN = new System.Windows.Forms.Button();
            this.lblSunIn = new System.Windows.Forms.Label();
            this.lblSatIn = new System.Windows.Forms.Label();
            this.lblFriIn = new System.Windows.Forms.Label();
            this.lblThrIn = new System.Windows.Forms.Label();
            this.lblWedIn = new System.Windows.Forms.Label();
            this.lblTueIn = new System.Windows.Forms.Label();
            this.lblMonIn = new System.Windows.Forms.Label();
            this.panelIN = new System.Windows.Forms.Panel();
            this.tabPageOUT = new System.Windows.Forms.TabPage();
            this.lbl23Out = new System.Windows.Forms.Label();
            this.lbl22Out = new System.Windows.Forms.Label();
            this.lbl21Out = new System.Windows.Forms.Label();
            this.lbl20Out = new System.Windows.Forms.Label();
            this.lbl19Out = new System.Windows.Forms.Label();
            this.lbl18Out = new System.Windows.Forms.Label();
            this.lbl17Out = new System.Windows.Forms.Label();
            this.lbl16Out = new System.Windows.Forms.Label();
            this.lbl15Out = new System.Windows.Forms.Label();
            this.lbl14Out = new System.Windows.Forms.Label();
            this.lbl13Out = new System.Windows.Forms.Label();
            this.lbl12Out = new System.Windows.Forms.Label();
            this.lbl11Out = new System.Windows.Forms.Label();
            this.lbl10Out = new System.Windows.Forms.Label();
            this.lbl9Out = new System.Windows.Forms.Label();
            this.lbl8Out = new System.Windows.Forms.Label();
            this.lbl7Out = new System.Windows.Forms.Label();
            this.lbl6Out = new System.Windows.Forms.Label();
            this.lbl5Out = new System.Windows.Forms.Label();
            this.lbl4Out = new System.Windows.Forms.Label();
            this.lbl3Out = new System.Windows.Forms.Label();
            this.lbl2Out = new System.Windows.Forms.Label();
            this.lbl1Out = new System.Windows.Forms.Label();
            this.lbl0Out = new System.Windows.Forms.Label();
            this.btnCheckAllOUT = new System.Windows.Forms.Button();
            this.btnUnCheckAllOUT = new System.Windows.Forms.Button();
            this.lblSunOut = new System.Windows.Forms.Label();
            this.lblSatOut = new System.Windows.Forms.Label();
            this.lblFriOut = new System.Windows.Forms.Label();
            this.lblThrOut = new System.Windows.Forms.Label();
            this.lblWedOut = new System.Windows.Forms.Label();
            this.lblTueOut = new System.Windows.Forms.Label();
            this.lblMonOut = new System.Windows.Forms.Label();
            this.panelOUT = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPageIN.SuspendLayout();
            this.tabPageOUT.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(24, 384);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 24;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(288, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(616, 384);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(24, 384);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(112, 48);
            this.tbDesc.MaxLength = 150;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(168, 20);
            this.tbDesc.TabIndex = 12;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(112, 16);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(168, 20);
            this.tbName.TabIndex = 10;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(24, 48);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(80, 23);
            this.lblDesc.TabIndex = 11;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(24, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 23);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageIN);
            this.tabControl1.Controls.Add(this.tabPageOUT);
            this.tabControl1.Location = new System.Drawing.Point(24, 88);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(670, 270);
            this.tabControl1.TabIndex = 17;
            // 
            // tabPageIN
            // 
            this.tabPageIN.Controls.Add(this.lbl23In);
            this.tabPageIN.Controls.Add(this.lbl22In);
            this.tabPageIN.Controls.Add(this.lbl21In);
            this.tabPageIN.Controls.Add(this.lbl20In);
            this.tabPageIN.Controls.Add(this.lbl19In);
            this.tabPageIN.Controls.Add(this.lbl18In);
            this.tabPageIN.Controls.Add(this.lbl17In);
            this.tabPageIN.Controls.Add(this.lbl16In);
            this.tabPageIN.Controls.Add(this.lbl15In);
            this.tabPageIN.Controls.Add(this.lbl14In);
            this.tabPageIN.Controls.Add(this.lbl13In);
            this.tabPageIN.Controls.Add(this.lbl12In);
            this.tabPageIN.Controls.Add(this.lbl11In);
            this.tabPageIN.Controls.Add(this.lbl10In);
            this.tabPageIN.Controls.Add(this.lbl9In);
            this.tabPageIN.Controls.Add(this.lbl8In);
            this.tabPageIN.Controls.Add(this.lbl7In);
            this.tabPageIN.Controls.Add(this.lbl6In);
            this.tabPageIN.Controls.Add(this.lbl5In);
            this.tabPageIN.Controls.Add(this.lbl4In);
            this.tabPageIN.Controls.Add(this.lbl3In);
            this.tabPageIN.Controls.Add(this.lbl2In);
            this.tabPageIN.Controls.Add(this.lbl1In);
            this.tabPageIN.Controls.Add(this.lbl0In);
            this.tabPageIN.Controls.Add(this.btnCheckAllIN);
            this.tabPageIN.Controls.Add(this.btnUnCheckAllIN);
            this.tabPageIN.Controls.Add(this.lblSunIn);
            this.tabPageIN.Controls.Add(this.lblSatIn);
            this.tabPageIN.Controls.Add(this.lblFriIn);
            this.tabPageIN.Controls.Add(this.lblThrIn);
            this.tabPageIN.Controls.Add(this.lblWedIn);
            this.tabPageIN.Controls.Add(this.lblTueIn);
            this.tabPageIN.Controls.Add(this.lblMonIn);
            this.tabPageIN.Controls.Add(this.panelIN);
            this.tabPageIN.Location = new System.Drawing.Point(4, 22);
            this.tabPageIN.Name = "tabPageIN";
            this.tabPageIN.Size = new System.Drawing.Size(662, 244);
            this.tabPageIN.TabIndex = 0;
            this.tabPageIN.Text = "IN";
            // 
            // lbl23In
            // 
            this.lbl23In.Location = new System.Drawing.Point(614, 8);
            this.lbl23In.Name = "lbl23In";
            this.lbl23In.Size = new System.Drawing.Size(19, 20);
            this.lbl23In.TabIndex = 41;
            this.lbl23In.Text = "23";
            this.lbl23In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl22In
            // 
            this.lbl22In.Location = new System.Drawing.Point(590, 8);
            this.lbl22In.Name = "lbl22In";
            this.lbl22In.Size = new System.Drawing.Size(19, 20);
            this.lbl22In.TabIndex = 40;
            this.lbl22In.Text = "22";
            this.lbl22In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl21In
            // 
            this.lbl21In.Location = new System.Drawing.Point(566, 8);
            this.lbl21In.Name = "lbl21In";
            this.lbl21In.Size = new System.Drawing.Size(19, 20);
            this.lbl21In.TabIndex = 39;
            this.lbl21In.Text = "21";
            this.lbl21In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl20In
            // 
            this.lbl20In.Location = new System.Drawing.Point(542, 8);
            this.lbl20In.Name = "lbl20In";
            this.lbl20In.Size = new System.Drawing.Size(19, 20);
            this.lbl20In.TabIndex = 38;
            this.lbl20In.Text = "20";
            this.lbl20In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl19In
            // 
            this.lbl19In.Location = new System.Drawing.Point(518, 8);
            this.lbl19In.Name = "lbl19In";
            this.lbl19In.Size = new System.Drawing.Size(19, 20);
            this.lbl19In.TabIndex = 37;
            this.lbl19In.Text = "19";
            this.lbl19In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl18In
            // 
            this.lbl18In.Location = new System.Drawing.Point(494, 8);
            this.lbl18In.Name = "lbl18In";
            this.lbl18In.Size = new System.Drawing.Size(19, 20);
            this.lbl18In.TabIndex = 36;
            this.lbl18In.Text = "18";
            this.lbl18In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl17In
            // 
            this.lbl17In.Location = new System.Drawing.Point(470, 8);
            this.lbl17In.Name = "lbl17In";
            this.lbl17In.Size = new System.Drawing.Size(19, 20);
            this.lbl17In.TabIndex = 35;
            this.lbl17In.Text = "17";
            this.lbl17In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl16In
            // 
            this.lbl16In.Location = new System.Drawing.Point(446, 8);
            this.lbl16In.Name = "lbl16In";
            this.lbl16In.Size = new System.Drawing.Size(19, 20);
            this.lbl16In.TabIndex = 34;
            this.lbl16In.Text = "16";
            this.lbl16In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl15In
            // 
            this.lbl15In.Location = new System.Drawing.Point(422, 8);
            this.lbl15In.Name = "lbl15In";
            this.lbl15In.Size = new System.Drawing.Size(19, 20);
            this.lbl15In.TabIndex = 33;
            this.lbl15In.Text = "15";
            this.lbl15In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl14In
            // 
            this.lbl14In.Location = new System.Drawing.Point(398, 8);
            this.lbl14In.Name = "lbl14In";
            this.lbl14In.Size = new System.Drawing.Size(19, 20);
            this.lbl14In.TabIndex = 32;
            this.lbl14In.Text = "14";
            this.lbl14In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl13In
            // 
            this.lbl13In.Location = new System.Drawing.Point(374, 8);
            this.lbl13In.Name = "lbl13In";
            this.lbl13In.Size = new System.Drawing.Size(19, 20);
            this.lbl13In.TabIndex = 31;
            this.lbl13In.Text = "13";
            this.lbl13In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl12In
            // 
            this.lbl12In.Location = new System.Drawing.Point(350, 8);
            this.lbl12In.Name = "lbl12In";
            this.lbl12In.Size = new System.Drawing.Size(19, 20);
            this.lbl12In.TabIndex = 30;
            this.lbl12In.Text = "12";
            this.lbl12In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl11In
            // 
            this.lbl11In.Location = new System.Drawing.Point(326, 8);
            this.lbl11In.Name = "lbl11In";
            this.lbl11In.Size = new System.Drawing.Size(19, 20);
            this.lbl11In.TabIndex = 29;
            this.lbl11In.Text = "11";
            this.lbl11In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl10In
            // 
            this.lbl10In.Location = new System.Drawing.Point(302, 8);
            this.lbl10In.Name = "lbl10In";
            this.lbl10In.Size = new System.Drawing.Size(19, 20);
            this.lbl10In.TabIndex = 28;
            this.lbl10In.Text = "10";
            this.lbl10In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl9In
            // 
            this.lbl9In.Location = new System.Drawing.Point(278, 8);
            this.lbl9In.Name = "lbl9In";
            this.lbl9In.Size = new System.Drawing.Size(19, 20);
            this.lbl9In.TabIndex = 27;
            this.lbl9In.Text = "9";
            this.lbl9In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl8In
            // 
            this.lbl8In.Location = new System.Drawing.Point(254, 8);
            this.lbl8In.Name = "lbl8In";
            this.lbl8In.Size = new System.Drawing.Size(19, 20);
            this.lbl8In.TabIndex = 26;
            this.lbl8In.Text = "8";
            this.lbl8In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl7In
            // 
            this.lbl7In.Location = new System.Drawing.Point(230, 8);
            this.lbl7In.Name = "lbl7In";
            this.lbl7In.Size = new System.Drawing.Size(19, 20);
            this.lbl7In.TabIndex = 25;
            this.lbl7In.Text = "7";
            this.lbl7In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl6In
            // 
            this.lbl6In.Location = new System.Drawing.Point(206, 8);
            this.lbl6In.Name = "lbl6In";
            this.lbl6In.Size = new System.Drawing.Size(19, 20);
            this.lbl6In.TabIndex = 24;
            this.lbl6In.Text = "6";
            this.lbl6In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl5In
            // 
            this.lbl5In.Location = new System.Drawing.Point(182, 8);
            this.lbl5In.Name = "lbl5In";
            this.lbl5In.Size = new System.Drawing.Size(19, 20);
            this.lbl5In.TabIndex = 23;
            this.lbl5In.Text = "5";
            this.lbl5In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl4In
            // 
            this.lbl4In.Location = new System.Drawing.Point(158, 8);
            this.lbl4In.Name = "lbl4In";
            this.lbl4In.Size = new System.Drawing.Size(19, 20);
            this.lbl4In.TabIndex = 22;
            this.lbl4In.Text = "4";
            this.lbl4In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl3In
            // 
            this.lbl3In.Location = new System.Drawing.Point(134, 8);
            this.lbl3In.Name = "lbl3In";
            this.lbl3In.Size = new System.Drawing.Size(19, 20);
            this.lbl3In.TabIndex = 21;
            this.lbl3In.Text = "3";
            this.lbl3In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl2In
            // 
            this.lbl2In.Location = new System.Drawing.Point(110, 8);
            this.lbl2In.Name = "lbl2In";
            this.lbl2In.Size = new System.Drawing.Size(19, 20);
            this.lbl2In.TabIndex = 20;
            this.lbl2In.Text = "2";
            this.lbl2In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl1In
            // 
            this.lbl1In.Location = new System.Drawing.Point(86, 8);
            this.lbl1In.Name = "lbl1In";
            this.lbl1In.Size = new System.Drawing.Size(19, 20);
            this.lbl1In.TabIndex = 19;
            this.lbl1In.Text = "1";
            this.lbl1In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl0In
            // 
            this.lbl0In.Location = new System.Drawing.Point(62, 8);
            this.lbl0In.Name = "lbl0In";
            this.lbl0In.Size = new System.Drawing.Size(19, 20);
            this.lbl0In.TabIndex = 18;
            this.lbl0In.Text = "0";
            this.lbl0In.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnCheckAllIN
            // 
            this.btnCheckAllIN.Location = new System.Drawing.Point(480, 208);
            this.btnCheckAllIN.Name = "btnCheckAllIN";
            this.btnCheckAllIN.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAllIN.TabIndex = 19;
            this.btnCheckAllIN.Text = "CheckAll";
            this.btnCheckAllIN.Click += new System.EventHandler(this.btnCheckAllIN_Click);
            // 
            // btnUnCheckAllIN
            // 
            this.btnUnCheckAllIN.Location = new System.Drawing.Point(568, 208);
            this.btnUnCheckAllIN.Name = "btnUnCheckAllIN";
            this.btnUnCheckAllIN.Size = new System.Drawing.Size(75, 23);
            this.btnUnCheckAllIN.TabIndex = 20;
            this.btnUnCheckAllIN.Text = "UnCheckAll";
            this.btnUnCheckAllIN.Click += new System.EventHandler(this.btnUnCheckAllIN_Click);
            // 
            // lblSunIn
            // 
            this.lblSunIn.Location = new System.Drawing.Point(16, 164);
            this.lblSunIn.Name = "lblSunIn";
            this.lblSunIn.Size = new System.Drawing.Size(40, 22);
            this.lblSunIn.TabIndex = 14;
            this.lblSunIn.Text = "Sun:";
            this.lblSunIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSatIn
            // 
            this.lblSatIn.Location = new System.Drawing.Point(16, 142);
            this.lblSatIn.Name = "lblSatIn";
            this.lblSatIn.Size = new System.Drawing.Size(40, 22);
            this.lblSatIn.TabIndex = 13;
            this.lblSatIn.Text = "Sat:";
            this.lblSatIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFriIn
            // 
            this.lblFriIn.Location = new System.Drawing.Point(16, 120);
            this.lblFriIn.Name = "lblFriIn";
            this.lblFriIn.Size = new System.Drawing.Size(40, 22);
            this.lblFriIn.TabIndex = 12;
            this.lblFriIn.Text = "Fri:";
            this.lblFriIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblThrIn
            // 
            this.lblThrIn.Location = new System.Drawing.Point(16, 98);
            this.lblThrIn.Name = "lblThrIn";
            this.lblThrIn.Size = new System.Drawing.Size(40, 22);
            this.lblThrIn.TabIndex = 11;
            this.lblThrIn.Text = "Thr:";
            this.lblThrIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWedIn
            // 
            this.lblWedIn.Location = new System.Drawing.Point(16, 76);
            this.lblWedIn.Name = "lblWedIn";
            this.lblWedIn.Size = new System.Drawing.Size(40, 22);
            this.lblWedIn.TabIndex = 10;
            this.lblWedIn.Text = "Wed:";
            this.lblWedIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTueIn
            // 
            this.lblTueIn.Location = new System.Drawing.Point(16, 54);
            this.lblTueIn.Name = "lblTueIn";
            this.lblTueIn.Size = new System.Drawing.Size(40, 22);
            this.lblTueIn.TabIndex = 9;
            this.lblTueIn.Text = "Tue:";
            this.lblTueIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMonIn
            // 
            this.lblMonIn.Location = new System.Drawing.Point(16, 32);
            this.lblMonIn.Name = "lblMonIn";
            this.lblMonIn.Size = new System.Drawing.Size(40, 22);
            this.lblMonIn.TabIndex = 8;
            this.lblMonIn.Text = "Mon:";
            this.lblMonIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelIN
            // 
            this.panelIN.Location = new System.Drawing.Point(60, 32);
            this.panelIN.Name = "panelIN";
            this.panelIN.Size = new System.Drawing.Size(585, 160);
            this.panelIN.TabIndex = 18;
            // 
            // tabPageOUT
            // 
            this.tabPageOUT.Controls.Add(this.lbl23Out);
            this.tabPageOUT.Controls.Add(this.lbl22Out);
            this.tabPageOUT.Controls.Add(this.lbl21Out);
            this.tabPageOUT.Controls.Add(this.lbl20Out);
            this.tabPageOUT.Controls.Add(this.lbl19Out);
            this.tabPageOUT.Controls.Add(this.lbl18Out);
            this.tabPageOUT.Controls.Add(this.lbl17Out);
            this.tabPageOUT.Controls.Add(this.lbl16Out);
            this.tabPageOUT.Controls.Add(this.lbl15Out);
            this.tabPageOUT.Controls.Add(this.lbl14Out);
            this.tabPageOUT.Controls.Add(this.lbl13Out);
            this.tabPageOUT.Controls.Add(this.lbl12Out);
            this.tabPageOUT.Controls.Add(this.lbl11Out);
            this.tabPageOUT.Controls.Add(this.lbl10Out);
            this.tabPageOUT.Controls.Add(this.lbl9Out);
            this.tabPageOUT.Controls.Add(this.lbl8Out);
            this.tabPageOUT.Controls.Add(this.lbl7Out);
            this.tabPageOUT.Controls.Add(this.lbl6Out);
            this.tabPageOUT.Controls.Add(this.lbl5Out);
            this.tabPageOUT.Controls.Add(this.lbl4Out);
            this.tabPageOUT.Controls.Add(this.lbl3Out);
            this.tabPageOUT.Controls.Add(this.lbl2Out);
            this.tabPageOUT.Controls.Add(this.lbl1Out);
            this.tabPageOUT.Controls.Add(this.lbl0Out);
            this.tabPageOUT.Controls.Add(this.btnCheckAllOUT);
            this.tabPageOUT.Controls.Add(this.btnUnCheckAllOUT);
            this.tabPageOUT.Controls.Add(this.lblSunOut);
            this.tabPageOUT.Controls.Add(this.lblSatOut);
            this.tabPageOUT.Controls.Add(this.lblFriOut);
            this.tabPageOUT.Controls.Add(this.lblThrOut);
            this.tabPageOUT.Controls.Add(this.lblWedOut);
            this.tabPageOUT.Controls.Add(this.lblTueOut);
            this.tabPageOUT.Controls.Add(this.lblMonOut);
            this.tabPageOUT.Controls.Add(this.panelOUT);
            this.tabPageOUT.Location = new System.Drawing.Point(4, 22);
            this.tabPageOUT.Name = "tabPageOUT";
            this.tabPageOUT.Size = new System.Drawing.Size(662, 244);
            this.tabPageOUT.TabIndex = 1;
            this.tabPageOUT.Text = "OUT";
            // 
            // lbl23Out
            // 
            this.lbl23Out.Location = new System.Drawing.Point(614, 8);
            this.lbl23Out.Name = "lbl23Out";
            this.lbl23Out.Size = new System.Drawing.Size(19, 20);
            this.lbl23Out.TabIndex = 65;
            this.lbl23Out.Text = "23";
            this.lbl23Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl22Out
            // 
            this.lbl22Out.Location = new System.Drawing.Point(590, 8);
            this.lbl22Out.Name = "lbl22Out";
            this.lbl22Out.Size = new System.Drawing.Size(19, 20);
            this.lbl22Out.TabIndex = 64;
            this.lbl22Out.Text = "22";
            this.lbl22Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl21Out
            // 
            this.lbl21Out.Location = new System.Drawing.Point(566, 8);
            this.lbl21Out.Name = "lbl21Out";
            this.lbl21Out.Size = new System.Drawing.Size(19, 20);
            this.lbl21Out.TabIndex = 63;
            this.lbl21Out.Text = "21";
            this.lbl21Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl20Out
            // 
            this.lbl20Out.Location = new System.Drawing.Point(542, 8);
            this.lbl20Out.Name = "lbl20Out";
            this.lbl20Out.Size = new System.Drawing.Size(19, 20);
            this.lbl20Out.TabIndex = 62;
            this.lbl20Out.Text = "20";
            this.lbl20Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl19Out
            // 
            this.lbl19Out.Location = new System.Drawing.Point(518, 8);
            this.lbl19Out.Name = "lbl19Out";
            this.lbl19Out.Size = new System.Drawing.Size(19, 20);
            this.lbl19Out.TabIndex = 61;
            this.lbl19Out.Text = "19";
            this.lbl19Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl18Out
            // 
            this.lbl18Out.Location = new System.Drawing.Point(494, 8);
            this.lbl18Out.Name = "lbl18Out";
            this.lbl18Out.Size = new System.Drawing.Size(19, 20);
            this.lbl18Out.TabIndex = 60;
            this.lbl18Out.Text = "18";
            this.lbl18Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl17Out
            // 
            this.lbl17Out.Location = new System.Drawing.Point(470, 8);
            this.lbl17Out.Name = "lbl17Out";
            this.lbl17Out.Size = new System.Drawing.Size(19, 20);
            this.lbl17Out.TabIndex = 59;
            this.lbl17Out.Text = "17";
            this.lbl17Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl16Out
            // 
            this.lbl16Out.Location = new System.Drawing.Point(446, 8);
            this.lbl16Out.Name = "lbl16Out";
            this.lbl16Out.Size = new System.Drawing.Size(19, 20);
            this.lbl16Out.TabIndex = 58;
            this.lbl16Out.Text = "16";
            this.lbl16Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl15Out
            // 
            this.lbl15Out.Location = new System.Drawing.Point(422, 8);
            this.lbl15Out.Name = "lbl15Out";
            this.lbl15Out.Size = new System.Drawing.Size(19, 20);
            this.lbl15Out.TabIndex = 57;
            this.lbl15Out.Text = "15";
            this.lbl15Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl14Out
            // 
            this.lbl14Out.Location = new System.Drawing.Point(398, 8);
            this.lbl14Out.Name = "lbl14Out";
            this.lbl14Out.Size = new System.Drawing.Size(19, 20);
            this.lbl14Out.TabIndex = 56;
            this.lbl14Out.Text = "14";
            this.lbl14Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl13Out
            // 
            this.lbl13Out.Location = new System.Drawing.Point(374, 8);
            this.lbl13Out.Name = "lbl13Out";
            this.lbl13Out.Size = new System.Drawing.Size(19, 20);
            this.lbl13Out.TabIndex = 55;
            this.lbl13Out.Text = "13";
            this.lbl13Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl12Out
            // 
            this.lbl12Out.Location = new System.Drawing.Point(350, 8);
            this.lbl12Out.Name = "lbl12Out";
            this.lbl12Out.Size = new System.Drawing.Size(19, 20);
            this.lbl12Out.TabIndex = 54;
            this.lbl12Out.Text = "12";
            this.lbl12Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl11Out
            // 
            this.lbl11Out.Location = new System.Drawing.Point(326, 8);
            this.lbl11Out.Name = "lbl11Out";
            this.lbl11Out.Size = new System.Drawing.Size(19, 20);
            this.lbl11Out.TabIndex = 53;
            this.lbl11Out.Text = "11";
            this.lbl11Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl10Out
            // 
            this.lbl10Out.Location = new System.Drawing.Point(302, 8);
            this.lbl10Out.Name = "lbl10Out";
            this.lbl10Out.Size = new System.Drawing.Size(19, 20);
            this.lbl10Out.TabIndex = 52;
            this.lbl10Out.Text = "10";
            this.lbl10Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl9Out
            // 
            this.lbl9Out.Location = new System.Drawing.Point(278, 8);
            this.lbl9Out.Name = "lbl9Out";
            this.lbl9Out.Size = new System.Drawing.Size(19, 20);
            this.lbl9Out.TabIndex = 51;
            this.lbl9Out.Text = "9";
            this.lbl9Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl8Out
            // 
            this.lbl8Out.Location = new System.Drawing.Point(254, 8);
            this.lbl8Out.Name = "lbl8Out";
            this.lbl8Out.Size = new System.Drawing.Size(19, 20);
            this.lbl8Out.TabIndex = 50;
            this.lbl8Out.Text = "8";
            this.lbl8Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl7Out
            // 
            this.lbl7Out.Location = new System.Drawing.Point(230, 8);
            this.lbl7Out.Name = "lbl7Out";
            this.lbl7Out.Size = new System.Drawing.Size(19, 20);
            this.lbl7Out.TabIndex = 49;
            this.lbl7Out.Text = "7";
            this.lbl7Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl6Out
            // 
            this.lbl6Out.Location = new System.Drawing.Point(206, 8);
            this.lbl6Out.Name = "lbl6Out";
            this.lbl6Out.Size = new System.Drawing.Size(19, 20);
            this.lbl6Out.TabIndex = 48;
            this.lbl6Out.Text = "6";
            this.lbl6Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl5Out
            // 
            this.lbl5Out.Location = new System.Drawing.Point(182, 8);
            this.lbl5Out.Name = "lbl5Out";
            this.lbl5Out.Size = new System.Drawing.Size(19, 20);
            this.lbl5Out.TabIndex = 47;
            this.lbl5Out.Text = "5";
            this.lbl5Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl4Out
            // 
            this.lbl4Out.Location = new System.Drawing.Point(158, 8);
            this.lbl4Out.Name = "lbl4Out";
            this.lbl4Out.Size = new System.Drawing.Size(19, 20);
            this.lbl4Out.TabIndex = 46;
            this.lbl4Out.Text = "4";
            this.lbl4Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl3Out
            // 
            this.lbl3Out.Location = new System.Drawing.Point(134, 8);
            this.lbl3Out.Name = "lbl3Out";
            this.lbl3Out.Size = new System.Drawing.Size(19, 20);
            this.lbl3Out.TabIndex = 45;
            this.lbl3Out.Text = "3";
            this.lbl3Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl2Out
            // 
            this.lbl2Out.Location = new System.Drawing.Point(110, 8);
            this.lbl2Out.Name = "lbl2Out";
            this.lbl2Out.Size = new System.Drawing.Size(19, 20);
            this.lbl2Out.TabIndex = 44;
            this.lbl2Out.Text = "2";
            this.lbl2Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl1Out
            // 
            this.lbl1Out.Location = new System.Drawing.Point(86, 8);
            this.lbl1Out.Name = "lbl1Out";
            this.lbl1Out.Size = new System.Drawing.Size(19, 20);
            this.lbl1Out.TabIndex = 43;
            this.lbl1Out.Text = "1";
            this.lbl1Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl0Out
            // 
            this.lbl0Out.Location = new System.Drawing.Point(62, 8);
            this.lbl0Out.Name = "lbl0Out";
            this.lbl0Out.Size = new System.Drawing.Size(19, 20);
            this.lbl0Out.TabIndex = 42;
            this.lbl0Out.Text = "0";
            this.lbl0Out.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnCheckAllOUT
            // 
            this.btnCheckAllOUT.Location = new System.Drawing.Point(480, 208);
            this.btnCheckAllOUT.Name = "btnCheckAllOUT";
            this.btnCheckAllOUT.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAllOUT.TabIndex = 22;
            this.btnCheckAllOUT.Text = "CheckAll";
            this.btnCheckAllOUT.Click += new System.EventHandler(this.btnCheckAllOUT_Click);
            // 
            // btnUnCheckAllOUT
            // 
            this.btnUnCheckAllOUT.Location = new System.Drawing.Point(568, 208);
            this.btnUnCheckAllOUT.Name = "btnUnCheckAllOUT";
            this.btnUnCheckAllOUT.Size = new System.Drawing.Size(75, 23);
            this.btnUnCheckAllOUT.TabIndex = 23;
            this.btnUnCheckAllOUT.Text = "UnCheckAll";
            this.btnUnCheckAllOUT.Click += new System.EventHandler(this.btnUnCheckAllOUT_Click);
            // 
            // lblSunOut
            // 
            this.lblSunOut.Location = new System.Drawing.Point(16, 164);
            this.lblSunOut.Name = "lblSunOut";
            this.lblSunOut.Size = new System.Drawing.Size(40, 22);
            this.lblSunOut.TabIndex = 7;
            this.lblSunOut.Text = "Sun:";
            this.lblSunOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSatOut
            // 
            this.lblSatOut.Location = new System.Drawing.Point(16, 142);
            this.lblSatOut.Name = "lblSatOut";
            this.lblSatOut.Size = new System.Drawing.Size(40, 22);
            this.lblSatOut.TabIndex = 6;
            this.lblSatOut.Text = "Sat:";
            this.lblSatOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFriOut
            // 
            this.lblFriOut.Location = new System.Drawing.Point(16, 120);
            this.lblFriOut.Name = "lblFriOut";
            this.lblFriOut.Size = new System.Drawing.Size(40, 22);
            this.lblFriOut.TabIndex = 5;
            this.lblFriOut.Text = "Fri:";
            this.lblFriOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblThrOut
            // 
            this.lblThrOut.Location = new System.Drawing.Point(16, 98);
            this.lblThrOut.Name = "lblThrOut";
            this.lblThrOut.Size = new System.Drawing.Size(40, 22);
            this.lblThrOut.TabIndex = 4;
            this.lblThrOut.Text = "Thr:";
            this.lblThrOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWedOut
            // 
            this.lblWedOut.Location = new System.Drawing.Point(16, 76);
            this.lblWedOut.Name = "lblWedOut";
            this.lblWedOut.Size = new System.Drawing.Size(40, 22);
            this.lblWedOut.TabIndex = 3;
            this.lblWedOut.Text = "Wed:";
            this.lblWedOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTueOut
            // 
            this.lblTueOut.Location = new System.Drawing.Point(16, 54);
            this.lblTueOut.Name = "lblTueOut";
            this.lblTueOut.Size = new System.Drawing.Size(40, 22);
            this.lblTueOut.TabIndex = 2;
            this.lblTueOut.Text = "Tue:";
            this.lblTueOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMonOut
            // 
            this.lblMonOut.Location = new System.Drawing.Point(16, 32);
            this.lblMonOut.Name = "lblMonOut";
            this.lblMonOut.Size = new System.Drawing.Size(40, 22);
            this.lblMonOut.TabIndex = 1;
            this.lblMonOut.Text = "Mon:";
            this.lblMonOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelOUT
            // 
            this.panelOUT.Location = new System.Drawing.Point(60, 32);
            this.panelOUT.Name = "panelOUT";
            this.panelOUT.Size = new System.Drawing.Size(585, 160);
            this.panelOUT.TabIndex = 21;
            // 
            // TimeAccessProfileAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(742, 426);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(750, 460);
            this.MinimumSize = new System.Drawing.Size(750, 460);
            this.Name = "TimeAccessProfileAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Add Add weekly access schedule";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TimeAccessProfileAdd_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tabPageIN.ResumeLayout(false);
            this.tabPageOUT.ResumeLayout(false);
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
				if (!currentTimeAccessProfile.Name.Equals(""))
				{
					this.Text = rm.GetString("updateTimeAccessProfile", culture);
				}
				else
				{
					this.Text = rm.GetString("addTimeAccessProfile", culture);
				}
				
				// button's text
				btnSave.Text          = rm.GetString("btnSave", culture);
				btnUpdate.Text        = rm.GetString("btnUpdate", culture);
				btnCancel.Text        = rm.GetString("btnCancel", culture);
				btnUnCheckAllIN.Text  = rm.GetString("btnUnCheckAll", culture);
				btnCheckAllIN.Text    = rm.GetString("btnCheckAll", culture);
				btnUnCheckAllOUT.Text = rm.GetString("btnUnCheckAll", culture);
				btnCheckAllOUT.Text   = rm.GetString("btnCheckAll", culture);

				// label's text
				lblName.Text   = rm.GetString("lblName", culture);
				lblDesc.Text   = rm.GetString("lblDescription", culture);
				lblMonOut.Text = rm.GetString("lblMon", culture);
			    lblTueOut.Text = rm.GetString("lblTue", culture);
				lblWedOut.Text = rm.GetString("lblWed", culture);
				lblThrOut.Text = rm.GetString("lblThr", culture);
				lblFriOut.Text = rm.GetString("lblFri", culture);
				lblSatOut.Text = rm.GetString("lblSat", culture);
				lblSunOut.Text = rm.GetString("lblSun", culture);
				lblSunIn.Text  = rm.GetString("lblSun", culture);
				lblSatIn.Text  = rm.GetString("lblSat", culture);
			    lblFriIn.Text  = rm.GetString("lblFri", culture);
				lblThrIn.Text  = rm.GetString("lblThr", culture);
				lblWedIn.Text  = rm.GetString("lblWed", culture);
				lblTueIn.Text  = rm.GetString("lblTue", culture);
				lblMonIn.Text  = rm.GetString("lblMon", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateCheckBoxScreen()
		{
			try
			{
				cellMonIN = new TimeAccessProfileCell(0, 0);
				this.panelIN.Controls.Add(cellMonIN);
				cellTueIN = new TimeAccessProfileCell(0, 22);
				this.panelIN.Controls.Add(cellTueIN);
				cellWedIN = new TimeAccessProfileCell(0, 44);
				this.panelIN.Controls.Add(cellWedIN);
				cellThrIN = new TimeAccessProfileCell(0, 66);
				this.panelIN.Controls.Add(cellThrIN);
				cellFriIN = new TimeAccessProfileCell(0, 88);
				this.panelIN.Controls.Add(cellFriIN);
				cellSatIN = new TimeAccessProfileCell(0, 110);
				this.panelIN.Controls.Add(cellSatIN);
				cellSunIN = new TimeAccessProfileCell(0, 132);
				this.panelIN.Controls.Add(cellSunIN);
				cellMonOUT = new TimeAccessProfileCell(0, 0);
				this.panelOUT.Controls.Add(cellMonOUT);
				cellTueOUT = new TimeAccessProfileCell(0, 22);
				this.panelOUT.Controls.Add(cellTueOUT);
				cellWedOUT = new TimeAccessProfileCell(0, 44);
				this.panelOUT.Controls.Add(cellWedOUT);
				cellThrOUT = new TimeAccessProfileCell(0, 66);
				this.panelOUT.Controls.Add(cellThrOUT);
				cellFriOUT = new TimeAccessProfileCell(0, 88);
				this.panelOUT.Controls.Add(cellFriOUT);
				cellSatOUT = new TimeAccessProfileCell(0, 110);
				this.panelOUT.Controls.Add(cellSatOUT);
				cellSunOUT = new TimeAccessProfileCell(0, 132);
				this.panelOUT.Controls.Add(cellSunOUT);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileAdd.populateCheckBoxScreen(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void getDetails()
		{
			try
			{
				for (int i = 1; i < 8; i++)
				{
					string date = i.ToString();
					switch (date)
					{
						case "1": 
							setCheckBoxes(cellMonIN, date, "IN");
							setCheckBoxes(cellMonOUT, date, "OUT");
							break;
						case "2": 
							setCheckBoxes(cellTueIN, date, "IN");
							setCheckBoxes(cellTueOUT, date, "OUT");
							break;
						case "3": 
							setCheckBoxes(cellWedIN, date, "IN");
							setCheckBoxes(cellWedOUT, date, "OUT");
							break;
						case "4": 
							setCheckBoxes(cellThrIN, date, "IN");
							setCheckBoxes(cellThrOUT, date, "OUT");
							break;
						case "5": 
							setCheckBoxes(cellFriIN, date, "IN");
							setCheckBoxes(cellFriOUT, date, "OUT");
							break;
						case "6": 
							setCheckBoxes(cellSatIN, date, "IN");
							setCheckBoxes(cellSatOUT, date, "OUT");
							break;
						case "7": 
							setCheckBoxes(cellSunIN, date, "IN");
							setCheckBoxes(cellSunOUT, date, "OUT");
							break;
					}							
				}
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileAdd.getDetails(): " + ex.Message + "\n");
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
                    MessageBox.Show(rm.GetString("addTimeAccessProfileNameEmpty", culture));
                }
                else
                {
                    ArrayList timeAccessProfiles = currentTimeAccessProfile.Search(tbName.Text.Trim());

                    if (timeAccessProfiles.Count == 0)
                    {
                        bool trans = currentTimeAccessProfile.BeginTransaction();
                        if (trans)
                        {
                            int timeAccessProfileID = currentTimeAccessProfile.Save(tbName.Text.Trim(), tbDesc.Text.Trim(), false);
                            bool isInserted = (timeAccessProfileID >= 0 ? true : false); //>= because here Identity is returned, for error is -1

                            if (isInserted)
                            {
                                TimeAccessProfileDtl tapDTL = new TimeAccessProfileDtl();
                                tapDTL.SetTransaction(currentTimeAccessProfile.GetTransaction());
                                string ID = timeAccessProfileID.ToString();

                                for (int i = 1; i < 8; i++)
                                {
                                    string date = i.ToString();
                                    switch (date)
                                    {
                                        case "1":
                                            isInserted = saveDtl(cellMonIN, tapDTL, ID, date, "IN") && isInserted;
                                            isInserted = saveDtl(cellMonOUT, tapDTL, ID, date, "OUT") && isInserted;
                                            break;
                                        case "2":
                                            isInserted = saveDtl(cellTueIN, tapDTL, ID, date, "IN") && isInserted;
                                            isInserted = saveDtl(cellTueOUT, tapDTL, ID, date, "OUT") && isInserted;
                                            break;
                                        case "3":
                                            isInserted = saveDtl(cellWedIN, tapDTL, ID, date, "IN") && isInserted;
                                            isInserted = saveDtl(cellWedOUT, tapDTL, ID, date, "OUT") && isInserted;
                                            break;
                                        case "4":
                                            isInserted = saveDtl(cellThrIN, tapDTL, ID, date, "IN") && isInserted;
                                            isInserted = saveDtl(cellThrOUT, tapDTL, ID, date, "OUT") && isInserted;
                                            break;
                                        case "5":
                                            isInserted = saveDtl(cellFriIN, tapDTL, ID, date, "IN") && isInserted;
                                            isInserted = saveDtl(cellFriOUT, tapDTL, ID, date, "OUT") && isInserted;
                                            break;
                                        case "6":
                                            isInserted = saveDtl(cellSatIN, tapDTL, ID, date, "IN") && isInserted;
                                            isInserted = saveDtl(cellSatOUT, tapDTL, ID, date, "OUT") && isInserted;
                                            break;
                                        case "7":
                                            isInserted = saveDtl(cellSunIN, tapDTL, ID, date, "IN") && isInserted;
                                            isInserted = saveDtl(cellSunOUT, tapDTL, ID, date, "OUT") && isInserted;
                                            break;
                                    }
                                }
                            }

                            if (isInserted)
                            {
                                currentTimeAccessProfile.CommitTransaction();

                                currentTimeAccessProfile.Name = tbName.Text.Trim();
                                currentTimeAccessProfile.Description = tbDesc.Text.Trim();

                                MessageBox.Show(rm.GetString("timeAccessProfileSaved", culture));
                                this.Close();
                            }
                            else
                            {
                                currentTimeAccessProfile.RollbackTransaction();
                                MessageBox.Show(rm.GetString("TimeAccessProfileNotSaved", culture));
                            }
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("timeAccessProfileExists", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeAccessProfileAdd.btnSave_Click(): " + ex.Message + "\n");
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
                bool isUpdated = false;

                if (tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("addTimeAccessProfileNameEmpty", culture));
                }
                else
                {
                    if (!currentTimeAccessProfile.Name.Equals(tbName.Text.Trim()))
                    {
                        ArrayList timeAccessProfiles = currentTimeAccessProfile.Search(tbName.Text.Trim());
                        if (timeAccessProfiles.Count != 0)
                        {
                            MessageBox.Show(rm.GetString("timeAccessProfileExists", culture));
                            return;
                        }
                    }

                    bool trans = currentTimeAccessProfile.BeginTransaction();
                    if (trans)
                    {
                        isUpdated = currentTimeAccessProfile.Update(currentTimeAccessProfile.TimeAccessProfileId.ToString(), tbName.Text.Trim(), tbDesc.Text.Trim(), false);

                        if (isUpdated)
                        {
                            TimeAccessProfileDtl tapDTL = new TimeAccessProfileDtl();
                            tapDTL.SetTransaction(currentTimeAccessProfile.GetTransaction());
                            string ID = currentTimeAccessProfile.TimeAccessProfileId.ToString();

                            for (int i = 1; i < 8; i++)
                            {
                                string date = i.ToString();
                                switch (date)
                                {
                                    case "1":
                                        isUpdated = updateDtl(cellMonIN, tapDTL, ID, date, "IN") && isUpdated;
                                        isUpdated = updateDtl(cellMonOUT, tapDTL, ID, date, "OUT") && isUpdated;
                                        break;
                                    case "2":
                                        isUpdated = updateDtl(cellTueIN, tapDTL, ID, date, "IN") && isUpdated;
                                        isUpdated = updateDtl(cellTueOUT, tapDTL, ID, date, "OUT") && isUpdated;
                                        break;
                                    case "3":
                                        isUpdated = updateDtl(cellWedIN, tapDTL, ID, date, "IN") && isUpdated;
                                        isUpdated = updateDtl(cellWedOUT, tapDTL, ID, date, "OUT") && isUpdated;
                                        break;
                                    case "4":
                                        isUpdated = updateDtl(cellThrIN, tapDTL, ID, date, "IN") && isUpdated;
                                        isUpdated = updateDtl(cellThrOUT, tapDTL, ID, date, "OUT") && isUpdated;
                                        break;
                                    case "5":
                                        isUpdated = updateDtl(cellFriIN, tapDTL, ID, date, "IN") && isUpdated;
                                        isUpdated = updateDtl(cellFriOUT, tapDTL, ID, date, "OUT") && isUpdated;
                                        break;
                                    case "6":
                                        isUpdated = updateDtl(cellSatIN, tapDTL, ID, date, "IN") && isUpdated;
                                        isUpdated = updateDtl(cellSatOUT, tapDTL, ID, date, "OUT") && isUpdated;
                                        break;
                                    case "7":
                                        isUpdated = updateDtl(cellSunIN, tapDTL, ID, date, "IN") && isUpdated;
                                        isUpdated = updateDtl(cellSunOUT, tapDTL, ID, date, "OUT") && isUpdated;
                                        break;
                                }
                            }
                        }

                        if (isUpdated)
                        {
                            currentTimeAccessProfile.CommitTransaction();

                            currentTimeAccessProfile.Name = tbName.Text.Trim();
                            currentTimeAccessProfile.Description = tbDesc.Text.Trim();

                            MessageBox.Show(rm.GetString("timeAccessProfileUpdated", culture));
                            this.Close();
                        }
                        else
                        {
                            currentTimeAccessProfile.RollbackTransaction();
                            MessageBox.Show(rm.GetString("TimeAccessProfileNotUpdated", culture));
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeAccessProfileAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " TimeAccessProfileAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnCheckAllIN_Click(object sender, System.EventArgs e)
		{
			try
			{
				cellMonIN.checkAll();
				cellTueIN.checkAll();
				cellWedIN.checkAll();
				cellThrIN.checkAll();
				cellFriIN.checkAll();
				cellSatIN.checkAll();
				cellSunIN.checkAll();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileAdd.btnCheckAllIN_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnUnCheckAllIN_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cellMonIN.uncheckAll();
                cellTueIN.uncheckAll();
                cellWedIN.uncheckAll();
                cellThrIN.uncheckAll();
                cellFriIN.uncheckAll();
                cellSatIN.uncheckAll();
                cellSunIN.uncheckAll();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeAccessProfileAdd.btnUnCheckAllIN_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnCheckAllOUT_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cellMonOUT.checkAll();
                cellTueOUT.checkAll();
                cellWedOUT.checkAll();
                cellThrOUT.checkAll();
                cellFriOUT.checkAll();
                cellSatOUT.checkAll();
                cellSunOUT.checkAll();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeAccessProfileAdd.btnCheckAllOUT_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUnCheckAllOUT_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cellMonOUT.uncheckAll();
                cellTueOUT.uncheckAll();
                cellWedOUT.uncheckAll();
                cellThrOUT.uncheckAll();
                cellFriOUT.uncheckAll();
                cellSatOUT.uncheckAll();
                cellSunOUT.uncheckAll();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeAccessProfileAdd.btnUnCheckAllOUT_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void setCheckBoxes(TimeAccessProfileCell timeAccessProfileCell, string date, string direction)
		{
			TransferObjects.TimeAccessProfileDtlTO tapDtlTO = new TransferObjects.TimeAccessProfileDtlTO();

			tapDtlTO = (new TimeAccessProfileDtl()).Find(currentTimeAccessProfile.TimeAccessProfileId.ToString(), date, direction);
			timeAccessProfileCell.setCheckBoxes(tapDtlTO.Hrs0, tapDtlTO.Hrs1, tapDtlTO.Hrs2, tapDtlTO.Hrs3, tapDtlTO.Hrs4,
				tapDtlTO.Hrs5, tapDtlTO.Hrs6, tapDtlTO.Hrs7, tapDtlTO.Hrs8, tapDtlTO.Hrs9, tapDtlTO.Hrs10, 
				tapDtlTO.Hrs11, tapDtlTO.Hrs12, tapDtlTO.Hrs13, tapDtlTO.Hrs14, tapDtlTO.Hrs15, tapDtlTO.Hrs16, 
				tapDtlTO.Hrs17, tapDtlTO.Hrs18, tapDtlTO.Hrs19, tapDtlTO.Hrs20, tapDtlTO.Hrs21, tapDtlTO.Hrs22, tapDtlTO.Hrs23);
		}

		private void setStrings(TimeAccessProfileCell timeAccessProfileCell)
		{
			h0 = timeAccessProfileCell.checked0();
			h1 = timeAccessProfileCell.checked1();
			h2 = timeAccessProfileCell.checked2();
			h3 = timeAccessProfileCell.checked3();
			h4 = timeAccessProfileCell.checked4();
			h5 = timeAccessProfileCell.checked5();
			h6 = timeAccessProfileCell.checked6();
			h7 = timeAccessProfileCell.checked7();
			h8 = timeAccessProfileCell.checked8();
			h9 = timeAccessProfileCell.checked9();
			h10 = timeAccessProfileCell.checked10();
			h11 = timeAccessProfileCell.checked11();
			h12 = timeAccessProfileCell.checked12();
			h13 = timeAccessProfileCell.checked13();
			h14 = timeAccessProfileCell.checked14();
			h15 = timeAccessProfileCell.checked15();
			h16 = timeAccessProfileCell.checked16();
			h17 = timeAccessProfileCell.checked17();
			h18 = timeAccessProfileCell.checked18();
			h19 = timeAccessProfileCell.checked19();
			h20 = timeAccessProfileCell.checked20();
			h21 = timeAccessProfileCell.checked21();
			h22 = timeAccessProfileCell.checked22();
			h23 = timeAccessProfileCell.checked23();	
		}

		private bool saveDtl(TimeAccessProfileCell timeAccessProfileCell, TimeAccessProfileDtl tapDTL, string ID, string date, string direction)
		{
			setStrings(timeAccessProfileCell);
			bool isInserted = (tapDTL.Save(ID, date, direction, h0, h1, h2, h3, h4, h5, h6,
				h7, h8, h9, h10, h11, h12, h13, h14, h15, h16, h17, h18, h19,
				h20, h21, h22, h23, false) > 0) ? true : false;

			return isInserted;
		}

		private bool updateDtl(TimeAccessProfileCell timeAccessProfileCell, TimeAccessProfileDtl tapDTL, string ID, string date, string direction)
		{
			setStrings(timeAccessProfileCell);
			return tapDTL.Update(ID, date, direction, h0, h1, h2, h3, h4, h5, h6,
				h7, h8, h9, h10, h11, h12, h13, h14, h15, h16, h17, h18, h19,
				h20, h21, h22, h23, false);	
		}

		private void disableAll()
		{
			try
			{
				cellMonIN.disableAll();
				cellTueIN.disableAll();
				cellWedIN.disableAll();
				cellThrIN.disableAll();
				cellFriIN.disableAll();
				cellSatIN.disableAll();
				cellSunIN.disableAll();
				cellMonOUT.disableAll();
				cellTueOUT.disableAll();
				cellWedOUT.disableAll();
				cellThrOUT.disableAll();
				cellFriOUT.disableAll();
				cellSatOUT.disableAll();
				cellSunOUT.disableAll();

				btnUnCheckAllIN.Enabled = false;
				btnCheckAllIN.Enabled = false;
				btnUnCheckAllOUT.Enabled = false;
				btnCheckAllOUT.Enabled = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfileAdd.disableAll(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void TimeAccessProfileAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " TimeAccessProfileAdd.TimeAccessProfileAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
