using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for Addresses.
	/// </summary>
	public class Addresses : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox tbFaxNum3;
		private System.Windows.Forms.Label lblFaxNum3;
		private System.Windows.Forms.TextBox tbFaxNum2;
		private System.Windows.Forms.Label lblFaxNum2;
		private System.Windows.Forms.TextBox tbFaxNum1;
		private System.Windows.Forms.Label lblFaxNum1;
		private System.Windows.Forms.TextBox tbTelNum3;
		private System.Windows.Forms.Label lblTelNum3;
		private System.Windows.Forms.TextBox tbTelNum2;
		private System.Windows.Forms.Label lblTelNum2;
		private System.Windows.Forms.TextBox tbTelNum1;
		private System.Windows.Forms.Label lblTelNum1;
		private System.Windows.Forms.TextBox tbPostalZipCode;
		private System.Windows.Forms.Label lblPostalZipCode;
		private System.Windows.Forms.TextBox tbState;
		private System.Windows.Forms.Label lblState;
		private System.Windows.Forms.TextBox tbCountry;
		private System.Windows.Forms.Label lblCountry;
		private System.Windows.Forms.TextBox tbCityName;
		private System.Windows.Forms.Label lblCityName;
		private System.Windows.Forms.TextBox tbAddrLine3;
		private System.Windows.Forms.Label lblAddrLine3;
		private System.Windows.Forms.TextBox tbAddrLine2;
		private System.Windows.Forms.Label lblAddrLine2;
		private System.Windows.Forms.TextBox tbAddrLine1;
		private System.Windows.Forms.Label lblAddrLine1;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbAddressID;
		private System.Windows.Forms.Label lblAddressID;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		Address currentAddress = null;

		private CultureInfo culture;
		ResourceManager rm;
		
		DebugLog log;

		ApplUserTO logInUser;

		string messageAddrSave1 = "";
		string messageAddrSave2 = "";
		string messageAddrSave3 = "";
		string messageAddrSave4 = "";
		string messageAddrUpd1 = "";

		public Addresses(int addressID, string name)
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				currentAddress = new Address();

				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("UI.Resource",typeof(Addresses).Assembly);

				if (addressID <= 0)
				{
					// Add Form
					tbName.Text = name;
					btnUpdate.Visible = false;
					lblAddressID.Visible = false;
					tbAddressID.Visible = false;
				}
				else
				{
					// Update Form
					currentAddress.receiveTransferObject(currentAddress.Find(addressID));
					populateUpdateForm(currentAddress);
					btnSave.Visible = false;
					tbAddressID.Enabled = false;
				}

				setLanguage();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Addresses.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbFaxNum3 = new System.Windows.Forms.TextBox();
            this.lblFaxNum3 = new System.Windows.Forms.Label();
            this.tbFaxNum2 = new System.Windows.Forms.TextBox();
            this.lblFaxNum2 = new System.Windows.Forms.Label();
            this.tbFaxNum1 = new System.Windows.Forms.TextBox();
            this.lblFaxNum1 = new System.Windows.Forms.Label();
            this.tbTelNum3 = new System.Windows.Forms.TextBox();
            this.lblTelNum3 = new System.Windows.Forms.Label();
            this.tbTelNum2 = new System.Windows.Forms.TextBox();
            this.lblTelNum2 = new System.Windows.Forms.Label();
            this.tbTelNum1 = new System.Windows.Forms.TextBox();
            this.lblTelNum1 = new System.Windows.Forms.Label();
            this.tbPostalZipCode = new System.Windows.Forms.TextBox();
            this.lblPostalZipCode = new System.Windows.Forms.Label();
            this.tbState = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.tbCountry = new System.Windows.Forms.TextBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.tbCityName = new System.Windows.Forms.TextBox();
            this.lblCityName = new System.Windows.Forms.Label();
            this.tbAddrLine3 = new System.Windows.Forms.TextBox();
            this.lblAddrLine3 = new System.Windows.Forms.Label();
            this.tbAddrLine2 = new System.Windows.Forms.TextBox();
            this.lblAddrLine2 = new System.Windows.Forms.Label();
            this.tbAddrLine1 = new System.Windows.Forms.TextBox();
            this.lblAddrLine1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbAddressID = new System.Windows.Forms.TextBox();
            this.lblAddressID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(432, 296);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 23);
            this.btnUpdate.TabIndex = 31;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(536, 296);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(432, 296);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbFaxNum3
            // 
            this.tbFaxNum3.Location = new System.Drawing.Point(448, 240);
            this.tbFaxNum3.MaxLength = 17;
            this.tbFaxNum3.Name = "tbFaxNum3";
            this.tbFaxNum3.Size = new System.Drawing.Size(168, 20);
            this.tbFaxNum3.TabIndex = 29;
            // 
            // lblFaxNum3
            // 
            this.lblFaxNum3.Location = new System.Drawing.Point(336, 240);
            this.lblFaxNum3.Name = "lblFaxNum3";
            this.lblFaxNum3.Size = new System.Drawing.Size(100, 23);
            this.lblFaxNum3.TabIndex = 28;
            this.lblFaxNum3.Text = "Fax Number 3:";
            this.lblFaxNum3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFaxNum2
            // 
            this.tbFaxNum2.Location = new System.Drawing.Point(448, 208);
            this.tbFaxNum2.MaxLength = 17;
            this.tbFaxNum2.Name = "tbFaxNum2";
            this.tbFaxNum2.Size = new System.Drawing.Size(168, 20);
            this.tbFaxNum2.TabIndex = 27;
            // 
            // lblFaxNum2
            // 
            this.lblFaxNum2.Location = new System.Drawing.Point(336, 208);
            this.lblFaxNum2.Name = "lblFaxNum2";
            this.lblFaxNum2.Size = new System.Drawing.Size(100, 23);
            this.lblFaxNum2.TabIndex = 26;
            this.lblFaxNum2.Text = "Fax Number 2:";
            this.lblFaxNum2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFaxNum1
            // 
            this.tbFaxNum1.Location = new System.Drawing.Point(448, 176);
            this.tbFaxNum1.MaxLength = 17;
            this.tbFaxNum1.Name = "tbFaxNum1";
            this.tbFaxNum1.Size = new System.Drawing.Size(168, 20);
            this.tbFaxNum1.TabIndex = 25;
            // 
            // lblFaxNum1
            // 
            this.lblFaxNum1.Location = new System.Drawing.Point(336, 176);
            this.lblFaxNum1.Name = "lblFaxNum1";
            this.lblFaxNum1.Size = new System.Drawing.Size(100, 23);
            this.lblFaxNum1.TabIndex = 24;
            this.lblFaxNum1.Text = "Fax Number 1:";
            this.lblFaxNum1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbTelNum3
            // 
            this.tbTelNum3.Location = new System.Drawing.Point(448, 144);
            this.tbTelNum3.MaxLength = 17;
            this.tbTelNum3.Name = "tbTelNum3";
            this.tbTelNum3.Size = new System.Drawing.Size(168, 20);
            this.tbTelNum3.TabIndex = 23;
            // 
            // lblTelNum3
            // 
            this.lblTelNum3.Location = new System.Drawing.Point(336, 144);
            this.lblTelNum3.Name = "lblTelNum3";
            this.lblTelNum3.Size = new System.Drawing.Size(100, 23);
            this.lblTelNum3.TabIndex = 22;
            this.lblTelNum3.Text = "Tel Number 3:";
            this.lblTelNum3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbTelNum2
            // 
            this.tbTelNum2.Location = new System.Drawing.Point(448, 112);
            this.tbTelNum2.MaxLength = 17;
            this.tbTelNum2.Name = "tbTelNum2";
            this.tbTelNum2.Size = new System.Drawing.Size(168, 20);
            this.tbTelNum2.TabIndex = 21;
            // 
            // lblTelNum2
            // 
            this.lblTelNum2.Location = new System.Drawing.Point(336, 112);
            this.lblTelNum2.Name = "lblTelNum2";
            this.lblTelNum2.Size = new System.Drawing.Size(100, 23);
            this.lblTelNum2.TabIndex = 20;
            this.lblTelNum2.Text = "Tel number 2:";
            this.lblTelNum2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbTelNum1
            // 
            this.tbTelNum1.Location = new System.Drawing.Point(448, 80);
            this.tbTelNum1.MaxLength = 17;
            this.tbTelNum1.Name = "tbTelNum1";
            this.tbTelNum1.Size = new System.Drawing.Size(168, 20);
            this.tbTelNum1.TabIndex = 19;
            // 
            // lblTelNum1
            // 
            this.lblTelNum1.Location = new System.Drawing.Point(336, 80);
            this.lblTelNum1.Name = "lblTelNum1";
            this.lblTelNum1.Size = new System.Drawing.Size(100, 23);
            this.lblTelNum1.TabIndex = 18;
            this.lblTelNum1.Text = "Tel Number 1:";
            this.lblTelNum1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPostalZipCode
            // 
            this.tbPostalZipCode.Location = new System.Drawing.Point(448, 48);
            this.tbPostalZipCode.MaxLength = 10;
            this.tbPostalZipCode.Name = "tbPostalZipCode";
            this.tbPostalZipCode.Size = new System.Drawing.Size(168, 20);
            this.tbPostalZipCode.TabIndex = 17;
            // 
            // lblPostalZipCode
            // 
            this.lblPostalZipCode.Location = new System.Drawing.Point(336, 48);
            this.lblPostalZipCode.Name = "lblPostalZipCode";
            this.lblPostalZipCode.Size = new System.Drawing.Size(100, 23);
            this.lblPostalZipCode.TabIndex = 16;
            this.lblPostalZipCode.Text = "Postal Zip Code:";
            this.lblPostalZipCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbState
            // 
            this.tbState.Location = new System.Drawing.Point(128, 240);
            this.tbState.MaxLength = 35;
            this.tbState.Name = "tbState";
            this.tbState.Size = new System.Drawing.Size(168, 20);
            this.tbState.TabIndex = 15;
            // 
            // lblState
            // 
            this.lblState.Location = new System.Drawing.Point(16, 240);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(100, 23);
            this.lblState.TabIndex = 14;
            this.lblState.Text = "State:";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbCountry
            // 
            this.tbCountry.Location = new System.Drawing.Point(128, 208);
            this.tbCountry.MaxLength = 35;
            this.tbCountry.Name = "tbCountry";
            this.tbCountry.Size = new System.Drawing.Size(168, 20);
            this.tbCountry.TabIndex = 13;
            // 
            // lblCountry
            // 
            this.lblCountry.Location = new System.Drawing.Point(16, 208);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(100, 23);
            this.lblCountry.TabIndex = 12;
            this.lblCountry.Text = "Country:";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbCityName
            // 
            this.tbCityName.Location = new System.Drawing.Point(128, 176);
            this.tbCityName.MaxLength = 35;
            this.tbCityName.Name = "tbCityName";
            this.tbCityName.Size = new System.Drawing.Size(168, 20);
            this.tbCityName.TabIndex = 11;
            // 
            // lblCityName
            // 
            this.lblCityName.Location = new System.Drawing.Point(16, 176);
            this.lblCityName.Name = "lblCityName";
            this.lblCityName.Size = new System.Drawing.Size(100, 23);
            this.lblCityName.TabIndex = 10;
            this.lblCityName.Text = "City Name:";
            this.lblCityName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAddrLine3
            // 
            this.tbAddrLine3.Location = new System.Drawing.Point(128, 144);
            this.tbAddrLine3.MaxLength = 35;
            this.tbAddrLine3.Name = "tbAddrLine3";
            this.tbAddrLine3.Size = new System.Drawing.Size(168, 20);
            this.tbAddrLine3.TabIndex = 9;
            // 
            // lblAddrLine3
            // 
            this.lblAddrLine3.Location = new System.Drawing.Point(16, 144);
            this.lblAddrLine3.Name = "lblAddrLine3";
            this.lblAddrLine3.Size = new System.Drawing.Size(100, 23);
            this.lblAddrLine3.TabIndex = 8;
            this.lblAddrLine3.Text = "Address Line 3:";
            this.lblAddrLine3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAddrLine2
            // 
            this.tbAddrLine2.Location = new System.Drawing.Point(128, 112);
            this.tbAddrLine2.MaxLength = 35;
            this.tbAddrLine2.Name = "tbAddrLine2";
            this.tbAddrLine2.Size = new System.Drawing.Size(168, 20);
            this.tbAddrLine2.TabIndex = 7;
            // 
            // lblAddrLine2
            // 
            this.lblAddrLine2.Location = new System.Drawing.Point(16, 112);
            this.lblAddrLine2.Name = "lblAddrLine2";
            this.lblAddrLine2.Size = new System.Drawing.Size(100, 23);
            this.lblAddrLine2.TabIndex = 6;
            this.lblAddrLine2.Text = "Address Line 2:";
            this.lblAddrLine2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAddrLine1
            // 
            this.tbAddrLine1.Location = new System.Drawing.Point(128, 80);
            this.tbAddrLine1.MaxLength = 35;
            this.tbAddrLine1.Name = "tbAddrLine1";
            this.tbAddrLine1.Size = new System.Drawing.Size(168, 20);
            this.tbAddrLine1.TabIndex = 5;
            // 
            // lblAddrLine1
            // 
            this.lblAddrLine1.Location = new System.Drawing.Point(16, 80);
            this.lblAddrLine1.Name = "lblAddrLine1";
            this.lblAddrLine1.Size = new System.Drawing.Size(100, 23);
            this.lblAddrLine1.TabIndex = 4;
            this.lblAddrLine1.Text = "Address Line 1:";
            this.lblAddrLine1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(128, 48);
            this.tbName.MaxLength = 35;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(168, 20);
            this.tbName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(16, 48);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 23);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAddressID
            // 
            this.tbAddressID.Location = new System.Drawing.Point(128, 16);
            this.tbAddressID.MaxLength = 10;
            this.tbAddressID.Name = "tbAddressID";
            this.tbAddressID.Size = new System.Drawing.Size(96, 20);
            this.tbAddressID.TabIndex = 1;
            // 
            // lblAddressID
            // 
            this.lblAddressID.Location = new System.Drawing.Point(16, 16);
            this.lblAddressID.Name = "lblAddressID";
            this.lblAddressID.Size = new System.Drawing.Size(100, 23);
            this.lblAddressID.TabIndex = 0;
            this.lblAddressID.Text = "Address ID:";
            this.lblAddressID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Addresses
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(648, 334);
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblAddressID);
            this.Controls.Add(this.tbAddressID);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbAddrLine1);
            this.Controls.Add(this.tbAddrLine2);
            this.Controls.Add(this.tbAddrLine3);
            this.Controls.Add(this.tbCityName);
            this.Controls.Add(this.tbCountry);
            this.Controls.Add(this.tbState);
            this.Controls.Add(this.tbPostalZipCode);
            this.Controls.Add(this.tbTelNum1);
            this.Controls.Add(this.tbTelNum2);
            this.Controls.Add(this.tbTelNum3);
            this.Controls.Add(this.tbFaxNum1);
            this.Controls.Add(this.tbFaxNum2);
            this.Controls.Add(this.tbFaxNum3);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblAddrLine1);
            this.Controls.Add(this.lblAddrLine2);
            this.Controls.Add(this.lblAddrLine3);
            this.Controls.Add(this.lblCityName);
            this.Controls.Add(this.lblCountry);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblPostalZipCode);
            this.Controls.Add(this.lblTelNum1);
            this.Controls.Add(this.lblTelNum2);
            this.Controls.Add(this.lblTelNum3);
            this.Controls.Add(this.lblFaxNum1);
            this.Controls.Add(this.lblFaxNum2);
            this.Controls.Add(this.lblFaxNum3);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(656, 368);
            this.MinimumSize = new System.Drawing.Size(656, 368);
            this.Name = "Addresses";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "AddressesAdd";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Addresses_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Set proper language.
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				if (currentAddress.AddressID >= 0)
				{
					this.Text = rm.GetString("updateAddress", culture);
				}
				else
				{
					this.Text = rm.GetString("addAddress", culture);
				}

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblAddressID.Text = rm.GetString("lblAddressID", culture);
				lblName.Text = rm.GetString("lblName", culture);
				lblAddrLine1.Text = rm.GetString("lblAddrLine1", culture);
				lblAddrLine2.Text = rm.GetString("lblAddrLine2", culture);
				lblAddrLine3.Text = rm.GetString("lblAddrLine3", culture);
				lblCityName.Text = rm.GetString("lblCityName", culture);
				lblCountry.Text = rm.GetString("lblCountry", culture);
				lblState.Text = rm.GetString("lblState", culture);
				lblPostalZipCode.Text = rm.GetString("lblPostalZipCode", culture);
				lblTelNum1.Text = rm.GetString("lblTelNum1", culture);
				lblTelNum2.Text = rm.GetString("lblTelNum2", culture);
				lblTelNum3.Text = rm.GetString("lblTelNum3", culture);
				lblFaxNum1.Text = rm.GetString("lblFaxNum1", culture);
				lblFaxNum2.Text = rm.GetString("lblFaxNum2", culture);
				lblFaxNum3.Text = rm.GetString("lblFaxNum3", culture);

				// message's text
				messageAddrSave1 = rm.GetString("messageAddrSave1", culture);
				messageAddrSave2 = rm.GetString("messageAddrSave2", culture);
				messageAddrSave3 = rm.GetString("messageAddrSave3", culture);
				messageAddrSave4 = rm.GetString("messageAddrSave4", culture);
				messageAddrUpd1 = rm.GetString("messageAddrUpd1", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Addresses.setLanguage(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Populate form with data of address to update.
		/// </summary>
		/// <param name="loc"></param>
		public void populateUpdateForm(Address addr)
		{
			this.tbAddressID.Text = (addr.AddressID < 0) ? "" : addr.AddressID.ToString();
			this.tbName.Text = addr.Name;
			this.tbAddrLine1.Text = addr.AddressLine1;
			this.tbAddrLine2.Text = addr.AddressLine2;
			this.tbAddrLine3.Text = addr.AddressLine3;
			this.tbCityName.Text = addr.CityName;
			this.tbCountry.Text = addr.Country;
			this.tbState.Text = addr.State;
			this.tbPostalZipCode.Text = addr.PostalZipCode;
			this.tbTelNum1.Text = addr.TelNumber1;
			this.tbTelNum2.Text = addr.TelNumber2;
			this.tbTelNum3.Text = addr.TelNumber3;
			this.tbFaxNum1.Text = addr.FaxNumber1;
			this.tbFaxNum2.Text = addr.FaxNumber2;
			this.tbFaxNum3.Text = addr.FaxNumber3;
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
                log.writeLog(DateTime.Now + " Addresses.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Save new address.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int addressID = currentAddress.Save(this.tbName.Text.Trim(), this.tbAddrLine1.Text.Trim(),
                this.tbAddrLine2.Text.Trim(), this.tbAddrLine3.Text.Trim(), this.tbCityName.Text.Trim(),
                this.tbCountry.Text.Trim(), this.tbState.Text.Trim(), this.tbPostalZipCode.Text.Trim(),
                this.tbTelNum1.Text.Trim(), this.tbTelNum2.Text.Trim(), this.tbTelNum3.Text.Trim(),
                this.tbFaxNum1.Text.Trim(), this.tbFaxNum2.Text.Trim(), this.tbFaxNum3.Text.Trim());

                currentAddress.Clear();

                MessageBox.Show(messageAddrSave3);

                if (this.Owner is EmployeeAdd)
                {
                    ((EmployeeAdd)this.Owner).setAddressID(addressID);
                }
                else if (this.Owner is LocationsAdd)
                {
                    ((LocationsAdd)this.Owner).setAddressID(addressID);
                }
                else if (this.Owner is WorkingUnitsAdd)
                {
                    ((WorkingUnitsAdd)this.Owner).setAddressID(addressID);
                }

                this.Close();
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    log.writeLog(DateTime.Now + " Addresses.btnSave_Click(): " + messageAddrSave4 + "\n");
                    MessageBox.Show(messageAddrSave4);
                }
                else
                {
                    log.writeLog(DateTime.Now + " Addresses.btnSave_Click(): " + sqlex.Message + "\n");
                    MessageBox.Show(sqlex.Message);
                }
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    log.writeLog(DateTime.Now + " Addresses.btnSave_Click(): " + messageAddrSave4 + "\n");
                    MessageBox.Show(messageAddrSave4);
                }
                else
                {
                    log.writeLog(DateTime.Now + " Addresses.btnSave_Click(): " + mysqlex.Message + "\n");
                    MessageBox.Show(mysqlex.Message);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Addresses.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Update existing address.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                currentAddress.Update(Int32.Parse(this.tbAddressID.Text.Trim()), this.tbName.Text.Trim(), this.tbAddrLine1.Text.Trim(),
                    this.tbAddrLine2.Text.Trim(), this.tbAddrLine3.Text.Trim(), this.tbCityName.Text.Trim(),
                    this.tbCountry.Text.Trim(), this.tbState.Text.Trim(), this.tbPostalZipCode.Text.Trim(),
                    this.tbTelNum1.Text.Trim(), this.tbTelNum2.Text.Trim(), this.tbTelNum3.Text.Trim(),
                    this.tbFaxNum1.Text.Trim(), this.tbFaxNum2.Text.Trim(), this.tbFaxNum3.Text.Trim());

                currentAddress.Clear();

                MessageBox.Show(messageAddrUpd1);

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Addresses.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void Addresses_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Addresses.Addresses_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
