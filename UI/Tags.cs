using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using ReaderInterface;
using Util;
using System.Text;
using DeviceRFID;
using System.Collections.Generic;



namespace UI
{
	/// <summary>
	/// Summary description for Tags.
	/// </summary>
	public class Tags : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblTagID;
		private System.Windows.Forms.TextBox tbTagID;
		private System.Windows.Forms.GroupBox gbTags;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.ComboBox cbStatus;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Tag currentTag = null;
		private CultureInfo culture;
		ResourceManager rm;

		IReaderInterface readerInterface;
        private DeviceRFID.RFIDDevice accessControlDevice = null;
		ApplUserTO logInUser;

		private System.Windows.Forms.Label lblEmployee;
		DebugLog log;
		private string status = "";

		string messageTagSave1 = "";
		string messageTagSave2 = "";
		string messageTagSave3 = "";
		string messageTagSave4 = "";
		private System.Windows.Forms.Button btnFromReader;
		private System.Windows.Forms.Button btnCancel;
		string messageTagNew1 = "";
        private DateTimePicker dtpValidTo;
        private Label lblValidTo;
        private DateTimePicker dtpIssued;
        private Label lblIssued;
        private CheckBox chbValidToUndefined;
        private CheckBox chbIssuedUndefined;
        private Label lblEmployeeID;
        private GroupBox gbActaAndroid;
        private Label lblTypeAndroid;
        private TextBox tbTagIDAndroid;
        private Label lblTagAndroid;
        private ComboBox cbTypeAndroid;
        private TextBox tbDescAndroid;
        private Label lblDescAndroid;
        private ComboBox cbStatusAndroid;
        private Label lblStatusAndroid;

        static int desktopReaderCOMPort = 0;

		public Tags()
		{
			InitializeComponent();
			this.CenterToScreen();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();
			ReaderFactory.TechnologyType = new Reader().GetDefaultTechnology();
			readerInterface = ReaderFactory.GetReader;

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Tags).Assembly);

			currentTag = new Tag();
			setLanguage();
			this.cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
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
            this.lblTagID = new System.Windows.Forms.Label();
            this.tbTagID = new System.Windows.Forms.TextBox();
            this.gbTags = new System.Windows.Forms.GroupBox();
            this.chbValidToUndefined = new System.Windows.Forms.CheckBox();
            this.chbIssuedUndefined = new System.Windows.Forms.CheckBox();
            this.dtpValidTo = new System.Windows.Forms.DateTimePicker();
            this.lblValidTo = new System.Windows.Forms.Label();
            this.dtpIssued = new System.Windows.Forms.DateTimePicker();
            this.lblIssued = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gbActaAndroid = new System.Windows.Forms.GroupBox();
            this.tbDescAndroid = new System.Windows.Forms.TextBox();
            this.lblDescAndroid = new System.Windows.Forms.Label();
            this.cbStatusAndroid = new System.Windows.Forms.ComboBox();
            this.lblStatusAndroid = new System.Windows.Forms.Label();
            this.cbTypeAndroid = new System.Windows.Forms.ComboBox();
            this.lblTypeAndroid = new System.Windows.Forms.Label();
            this.tbTagIDAndroid = new System.Windows.Forms.TextBox();
            this.lblTagAndroid = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.btnFromReader = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblEmployeeID = new System.Windows.Forms.Label();
            this.gbTags.SuspendLayout();
            this.gbActaAndroid.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTagID
            // 
            this.lblTagID.Location = new System.Drawing.Point(9, 17);
            this.lblTagID.Name = "lblTagID";
            this.lblTagID.Size = new System.Drawing.Size(76, 23);
            this.lblTagID.TabIndex = 0;
            this.lblTagID.Text = "Tag ID:";
            this.lblTagID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbTagID
            // 
            this.tbTagID.Location = new System.Drawing.Point(91, 19);
            this.tbTagID.Name = "tbTagID";
            this.tbTagID.Size = new System.Drawing.Size(236, 20);
            this.tbTagID.TabIndex = 1;
            // 
            // gbTags
            // 
            this.gbTags.Controls.Add(this.chbValidToUndefined);
            this.gbTags.Controls.Add(this.chbIssuedUndefined);
            this.gbTags.Controls.Add(this.dtpValidTo);
            this.gbTags.Controls.Add(this.lblValidTo);
            this.gbTags.Controls.Add(this.dtpIssued);
            this.gbTags.Controls.Add(this.lblIssued);
            this.gbTags.Controls.Add(this.tbDescription);
            this.gbTags.Controls.Add(this.lblDescription);
            this.gbTags.Controls.Add(this.cbStatus);
            this.gbTags.Controls.Add(this.lblStatus);
            this.gbTags.Controls.Add(this.lblTagID);
            this.gbTags.Controls.Add(this.tbTagID);
            this.gbTags.Location = new System.Drawing.Point(12, 71);
            this.gbTags.Name = "gbTags";
            this.gbTags.Size = new System.Drawing.Size(341, 157);
            this.gbTags.TabIndex = 1;
            this.gbTags.TabStop = false;
            this.gbTags.Text = "Tag";
            // 
            // chbValidToUndefined
            // 
            this.chbValidToUndefined.AutoSize = true;
            this.chbValidToUndefined.Location = new System.Drawing.Point(192, 123);
            this.chbValidToUndefined.Name = "chbValidToUndefined";
            this.chbValidToUndefined.Size = new System.Drawing.Size(75, 17);
            this.chbValidToUndefined.TabIndex = 11;
            this.chbValidToUndefined.Text = "Undefined";
            this.chbValidToUndefined.UseVisualStyleBackColor = true;
            this.chbValidToUndefined.CheckedChanged += new System.EventHandler(this.chbValidToUndefined_CheckedChanged);
            // 
            // chbIssuedUndefined
            // 
            this.chbIssuedUndefined.AutoSize = true;
            this.chbIssuedUndefined.Location = new System.Drawing.Point(192, 100);
            this.chbIssuedUndefined.Name = "chbIssuedUndefined";
            this.chbIssuedUndefined.Size = new System.Drawing.Size(75, 17);
            this.chbIssuedUndefined.TabIndex = 8;
            this.chbIssuedUndefined.Text = "Undefined";
            this.chbIssuedUndefined.UseVisualStyleBackColor = true;
            this.chbIssuedUndefined.CheckedChanged += new System.EventHandler(this.chbIssuedUndefined_CheckedChanged);
            // 
            // dtpValidTo
            // 
            this.dtpValidTo.CustomFormat = "dd.MM.yyyy.";
            this.dtpValidTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpValidTo.Location = new System.Drawing.Point(91, 124);
            this.dtpValidTo.Name = "dtpValidTo";
            this.dtpValidTo.Size = new System.Drawing.Size(95, 20);
            this.dtpValidTo.TabIndex = 10;
            // 
            // lblValidTo
            // 
            this.lblValidTo.Location = new System.Drawing.Point(23, 122);
            this.lblValidTo.Name = "lblValidTo";
            this.lblValidTo.Size = new System.Drawing.Size(62, 23);
            this.lblValidTo.TabIndex = 9;
            this.lblValidTo.Text = "Valid to:";
            this.lblValidTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpIssued
            // 
            this.dtpIssued.CustomFormat = "dd.MM.yyyy.";
            this.dtpIssued.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpIssued.Location = new System.Drawing.Point(91, 98);
            this.dtpIssued.Name = "dtpIssued";
            this.dtpIssued.Size = new System.Drawing.Size(95, 20);
            this.dtpIssued.TabIndex = 7;
            // 
            // lblIssued
            // 
            this.lblIssued.Location = new System.Drawing.Point(6, 96);
            this.lblIssued.Name = "lblIssued";
            this.lblIssued.Size = new System.Drawing.Size(79, 23);
            this.lblIssued.TabIndex = 6;
            this.lblIssued.Text = "Issued:";
            this.lblIssued.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(91, 72);
            this.tbDescription.MaxLength = 50;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(236, 20);
            this.tbDescription.TabIndex = 5;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(6, 70);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(79, 23);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            this.cbStatus.Location = new System.Drawing.Point(91, 45);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(236, 21);
            this.cbStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(6, 43);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 23);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbActaAndroid
            // 
            this.gbActaAndroid.Controls.Add(this.tbDescAndroid);
            this.gbActaAndroid.Controls.Add(this.lblDescAndroid);
            this.gbActaAndroid.Controls.Add(this.cbStatusAndroid);
            this.gbActaAndroid.Controls.Add(this.lblStatusAndroid);
            this.gbActaAndroid.Controls.Add(this.cbTypeAndroid);
            this.gbActaAndroid.Controls.Add(this.lblTypeAndroid);
            this.gbActaAndroid.Controls.Add(this.tbTagIDAndroid);
            this.gbActaAndroid.Controls.Add(this.lblTagAndroid);
            this.gbActaAndroid.Location = new System.Drawing.Point(12, 71);
            this.gbActaAndroid.Name = "gbActaAndroid";
            this.gbActaAndroid.Size = new System.Drawing.Size(340, 157);
            this.gbActaAndroid.TabIndex = 12;
            this.gbActaAndroid.TabStop = false;
            this.gbActaAndroid.Text = "Tag";
            // 
            // tbDescAndroid
            // 
            this.tbDescAndroid.Location = new System.Drawing.Point(91, 116);
            this.tbDescAndroid.Name = "tbDescAndroid";
            this.tbDescAndroid.Size = new System.Drawing.Size(236, 20);
            this.tbDescAndroid.TabIndex = 7;
            // 
            // lblDescAndroid
            // 
            this.lblDescAndroid.AutoSize = true;
            this.lblDescAndroid.Location = new System.Drawing.Point(17, 119);
            this.lblDescAndroid.Name = "lblDescAndroid";
            this.lblDescAndroid.Size = new System.Drawing.Size(63, 13);
            this.lblDescAndroid.TabIndex = 6;
            this.lblDescAndroid.Text = "Description:";
            // 
            // cbStatusAndroid
            // 
            this.cbStatusAndroid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatusAndroid.FormattingEnabled = true;
            this.cbStatusAndroid.Location = new System.Drawing.Point(91, 80);
            this.cbStatusAndroid.Name = "cbStatusAndroid";
            this.cbStatusAndroid.Size = new System.Drawing.Size(236, 21);
            this.cbStatusAndroid.TabIndex = 5;
            // 
            // lblStatusAndroid
            // 
            this.lblStatusAndroid.AutoSize = true;
            this.lblStatusAndroid.Location = new System.Drawing.Point(17, 83);
            this.lblStatusAndroid.Name = "lblStatusAndroid";
            this.lblStatusAndroid.Size = new System.Drawing.Size(40, 13);
            this.lblStatusAndroid.TabIndex = 4;
            this.lblStatusAndroid.Text = "Status:";
            // 
            // cbTypeAndroid
            // 
            this.cbTypeAndroid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeAndroid.FormattingEnabled = true;
            this.cbTypeAndroid.Location = new System.Drawing.Point(91, 48);
            this.cbTypeAndroid.Name = "cbTypeAndroid";
            this.cbTypeAndroid.Size = new System.Drawing.Size(236, 21);
            this.cbTypeAndroid.TabIndex = 3;
            // 
            // lblTypeAndroid
            // 
            this.lblTypeAndroid.AutoSize = true;
            this.lblTypeAndroid.Location = new System.Drawing.Point(17, 51);
            this.lblTypeAndroid.Name = "lblTypeAndroid";
            this.lblTypeAndroid.Size = new System.Drawing.Size(34, 13);
            this.lblTypeAndroid.TabIndex = 2;
            this.lblTypeAndroid.Text = "Type:";
            // 
            // tbTagIDAndroid
            // 
            this.tbTagIDAndroid.Enabled = false;
            this.tbTagIDAndroid.Location = new System.Drawing.Point(91, 17);
            this.tbTagIDAndroid.Name = "tbTagIDAndroid";
            this.tbTagIDAndroid.Size = new System.Drawing.Size(236, 20);
            this.tbTagIDAndroid.TabIndex = 1;
            // 
            // lblTagAndroid
            // 
            this.lblTagAndroid.AutoSize = true;
            this.lblTagAndroid.Location = new System.Drawing.Point(17, 20);
            this.lblTagAndroid.Name = "lblTagAndroid";
            this.lblTagAndroid.Size = new System.Drawing.Size(40, 13);
            this.lblTagAndroid.TabIndex = 0;
            this.lblTagAndroid.Text = "TagID:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(174, 246);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 35);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(10, 32);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(306, 23);
            this.lblEmployee.TabIndex = 0;
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFromReader
            // 
            this.btnFromReader.Location = new System.Drawing.Point(12, 246);
            this.btnFromReader.Name = "btnFromReader";
            this.btnFromReader.Size = new System.Drawing.Size(100, 35);
            this.btnFromReader.TabIndex = 2;
            this.btnFromReader.Text = "Read from Reader";
            this.btnFromReader.Click += new System.EventHandler(this.btnFromReader_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(278, 246);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblEmployeeID
            // 
            this.lblEmployeeID.Location = new System.Drawing.Point(9, 9);
            this.lblEmployeeID.Name = "lblEmployeeID";
            this.lblEmployeeID.Size = new System.Drawing.Size(312, 23);
            this.lblEmployeeID.TabIndex = 5;
            this.lblEmployeeID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Tags
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(364, 289);
            this.ControlBox = false;
            this.Controls.Add(this.gbActaAndroid);
            this.Controls.Add(this.lblEmployeeID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFromReader);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbTags);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Tags";
            this.ShowInTaskbar = false;
            this.Text = "Tags";
            this.Load += new System.EventHandler(this.Tags_Load);
            this.Closed += new System.EventHandler(this.Tags_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Tags_KeyUp);
            this.gbTags.ResumeLayout(false);
            this.gbTags.PerformLayout();
            this.gbActaAndroid.ResumeLayout(false);
            this.gbActaAndroid.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Set proper language.
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// group box text
				gbTags.Text = rm.GetString("gbTags", culture);
                //Android
                gbActaAndroid.Text = rm.GetString("gbTags", culture);

                btnOK.Text = rm.GetString("btnOK", culture);
                btnFromReader.Text = rm.GetString("btnFromReader", culture);
                btnOK.Width = 100;

				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblTagID.Text = rm.GetString("lblTagID", culture);
				lblStatus.Text = rm.GetString("lblStatus", culture);
				lblDescription.Text = rm.GetString("lblDesc", culture);
                lblIssued.Text = rm.GetString("lblIssued", culture);
                lblValidTo.Text = rm.GetString("lblValidTo", culture);

                //Android
                lblTagAndroid.Text = rm.GetString("lblTagID", culture);
                lblTypeAndroid.Text = rm.GetString("lblTypeAndroid", culture);
                //lblStatusAndroid.Text = rm.GetString("lblStatus", culture);
                //lblDescAndroid.Text = rm.GetString("lblDesc", culture);

                // check box text
                chbIssuedUndefined.Text = rm.GetString("chbUndefined", culture);
                chbValidToUndefined.Text = rm.GetString("chbUndefined", culture);

				// message's text
				messageTagSave1 = rm.GetString("messageTagSave1", culture);
				messageTagSave2 = rm.GetString("messageTagSave2", culture);
				messageTagSave3 = rm.GetString("messageTagSave3", culture);
				messageTagSave4 = rm.GetString("messageTagSave4", culture);
				messageTagNew1 = rm.GetString("messageTagNew1", culture);

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tags.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate status combo box.
		/// </summary>
		private void populateStatusCombo()
		{
			ArrayList statusArray = new ArrayList();			
			statusArray.Add(rm.GetString("all", culture)); //statusArray.Add("ACTIVE/BLOCKED");
			statusArray.Add(Constants.statusLost);
			statusArray.Add(Constants.statusDamaged);
			statusArray.Add(Constants.statusReturned);

			cbStatus.DataSource = statusArray;
            //for Android
            cbStatusAndroid.DataSource = statusArray;
		}

        private void populateTypeCombo()
        {
            ArrayList typeArray = new ArrayList();
          
            typeArray.Add(Constants.typeManager);
            typeArray.Add(Constants.typeSupervisor);
            typeArray.Add(Constants.typeEmployee);

            cbTypeAndroid.DataSource = typeArray;
        }

		/// <summary>
		/// Set Employee name.
		/// </summary>
		/// <param name="name"></param>
        public void setEmployeeName(string name)
		{
			this.lblEmployee.Text = name;
		}

		/// <summary>
		/// Set Owner ID.
		/// </summary>
		/// <param name="ownerID"></param>
		public void setOwnerID(int ownerID)
		{
			currentTag.TgTO.OwnerID = ownerID;
            lblEmployeeID.Text = ownerID.ToString();
		}

		/// <summary>
		/// Set status.
		/// </summary>
		/// <param name="status"></param>
        public void setStatus(string status, bool writeDataToTag)
        {
            if (writeDataToTag)
            {
                this.status = "ANDROID" + status;
            }
            else
            {
                this.status = status;
            }
        }

        public void setTransaction(IDbTransaction trans)
        {
            currentTag.SetTransaction(trans);
        }

		// Save or update existing Tag, depending on Status sent.
		private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            UInt32 i = new UInt32();
            try
            {
                DateTime issued = new DateTime();
                DateTime validTO = new DateTime();
                if (dtpIssued.Value != Constants.dateTimeNullValue())
                    issued = dtpIssued.Value;
                if (dtpValidTo.Value != Constants.dateTimeNullValue())
                    validTO = dtpValidTo.Value;

                if (this.status.Equals("NEW") || this.status.Equals("ANDROIDNEW"))
                {
                    // Saving new Tag
                    if (this.status.Equals("NEW"))
                    {
                        if (tbTagID.Text.Trim().Equals(""))
                        {
                            this.Close();
                            return;
                        }
                        try
                        {
                            i = UInt32.Parse(tbTagID.Text.Trim());
                        }
                        //catch(Exception ex)
                        catch
                        {
                            MessageBox.Show(messageTagSave1);
                            return;
                        }
                    }
                    else if (this.status.Equals("ANDROIDNEW"))
                    {
                        if (readTagIDFromReader())
                        {
                            if (tbTagIDAndroid.Text.Trim().Equals(""))
                            {
                                this.Close();
                                return;
                            }
                            try
                            {
                                i = UInt32.Parse(tbTagIDAndroid.Text.Trim());
                            }
                            //catch(Exception ex)
                            catch
                            {
                                MessageBox.Show(messageTagSave1);
                                return;
                            }
                        }
                        else
                        {
                            this.Close();
                            return;
                        }
                    }


                    Tag tag1 = new Tag();
                    tag1.TgTO.Status = "ACTIVE";
                    tag1.TgTO.TagID = i;

                    Tag tag2 = new Tag();
                    tag2.TgTO.Status = "BLOCKED";
                    tag2.TgTO.TagID = i;

                    if (tag1.Search().Count == 0 && tag2.Search().Count == 0)
                    {
                        if (this.Owner is EmployeeAdd)
                        {
                            if (this.status.Equals("ANDROIDNEW"))
                            {
                                bool succ = currentTag.BeginTransaction();
                                if (succ)
                                {
                                    try
                                    {
                                        succ = succ && currentTag.SaveFromS(UInt32.Parse(tbTagIDAndroid.Text), Int32.Parse(currentTag.TgTO.OwnerID.ToString()), Constants.statusActive, "", issued, validTO, "", false) > 0;
                                        if (succ)
                                        {
                                            succ = writeToCard();
                                            if (succ)
                                            {
                                                MessageBox.Show(rm.GetString("successWritingToCard", culture));
                                                currentTag.CommitTransaction();
                                            }
                                            else
                                            {
                                                //not successful
                                                MessageBox.Show(rm.GetString("notSuccessWritingToCard", culture));
                                                currentTag.RollbackTransaction();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        if (currentTag.GetTransaction() != null)
                                        {
                                            //not successful
                                            MessageBox.Show(rm.GetString("notSuccessWritingToCard", culture));
                                            currentTag.RollbackTransaction();
                                        }
                                    }
                                }
                                ((EmployeeAdd)this.Owner).setHasTagChanged(succ);
                            }
                            else
                            {
                                ((EmployeeAdd)this.Owner).setHasTagChanged(currentTag.Save(UInt32.Parse(tbTagID.Text), Int32.Parse(currentTag.TgTO.OwnerID.ToString()), Constants.statusActive, "", issued, validTO) > 0);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(messageTagSave3);
                        return;
                    }
                }
                else if (this.status.Equals("BLOCKED") || this.status.Equals("ANDROIDBLOCKED"))
                {
                    // Blocking existing Active Tag
                    TagTO tag = currentTag.FindActive(currentTag.TgTO.OwnerID);
                    if (tag.RecordID >= 0)
                    {
                        if (currentTag.GetTransaction() == null)
                        {
                            currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                Int32.Parse(tag.OwnerID.ToString()),
                                Constants.statusBlocked, tbDescription.Text.Trim(), issued, validTO, true);
                        }
                        else
                        {
                            if (this.Owner is EmployeeAdd)
                            {
                                ((EmployeeAdd)this.Owner).setTagChanged(currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                    Int32.Parse(tag.OwnerID.ToString()),
                                    Constants.statusBlocked, tbDescription.Text.Trim(), issued, validTO, false));
                            }
                            else if (this.Owner is Employees)
                            {
                                ((Employees)this.Owner).setTagChanged(currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                    Int32.Parse(tag.OwnerID.ToString()),
                                    Constants.statusBlocked, tbDescription.Text.Trim(), issued, validTO, false));
                            }
                        }
                    }
                }

                else if (this.status.Equals("RETIRED") || this.status.Equals("ANDROIDRETIRED"))
                {
                    // Retiring existing Active Tag
                    TagTO tag = currentTag.FindActive(currentTag.TgTO.OwnerID);
                    if (tag.RecordID >= 0)
                    {
                        if (currentTag.GetTransaction() == null)
                        {
                            currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                Int32.Parse(tag.OwnerID.ToString()),
                                Constants.statusRetired, tbDescription.Text.Trim(), issued, validTO, true);
                        }
                        else
                        {
                            if (this.Owner is EmployeeAdd)
                            {
                                ((EmployeeAdd)this.Owner).setTagChanged(currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                    Int32.Parse(tag.OwnerID.ToString()),
                                    Constants.statusRetired, tbDescription.Text.Trim(), issued, validTO, false));
                            }
                            else if (this.Owner is Employees)
                            {
                                ((Employees)this.Owner).setTagChanged(currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                    Int32.Parse(tag.OwnerID.ToString()),
                                    Constants.statusRetired, tbDescription.Text.Trim(), issued, validTO, false));
                            }
                        }
                    }
                }
                else if (this.status.Equals("ACTIVE") || this.status.Equals("ANDROIDACTIVE"))
                {
                    // Set Blocked Tag to 'ACTIVE' status
                    TagTO tag = currentTag.FindActive(currentTag.TgTO.OwnerID);
                    if (tag.RecordID >= 0)
                    {
                        if (currentTag.GetTransaction() == null)
                        {
                            currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                Int32.Parse(tag.OwnerID.ToString()),
                                Constants.statusActive, tbDescription.Text.Trim(), issued, validTO, true);
                        }
                        else
                        {
                            if (this.Owner is EmployeeAdd)
                            {
                                ((EmployeeAdd)this.Owner).setTagChanged(currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                    Int32.Parse(tag.OwnerID.ToString()),
                                    Constants.statusActive, tbDescription.Text.Trim(), issued, validTO, false));
                            }
                            else if (this.Owner is Employees)
                            {
                                ((Employees)this.Owner).setTagChanged(currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                    Int32.Parse(tag.OwnerID.ToString()),
                                    Constants.statusActive, tbDescription.Text.Trim(), issued, validTO, false));
                            }
                        }
                    }
                }

                else if (this.status.Equals("CHANGE") || this.status.Equals("ANDROIDCHANGE"))
                {
                    // Update existing Tag
                    TagTO tag = currentTag.FindActive(currentTag.TgTO.OwnerID);
                    if (tag.RecordID >= 0)
                    {
                        if (this.status.Equals("ANDROIDCHANGE") && cbStatus.SelectedIndex == 0)
                        {
                            if (this.cbTypeAndroid.SelectedIndex > -1)
                            {

                                if (tbTagIDAndroid.Text.Equals(""))
                                {
                                    MessageBox.Show(rm.GetString("readCardFirst", culture));
                                    return;
                                }
                                bool succ = currentTag.BeginTransaction();
                                if (succ)
                                {
                                    try
                                    {
                                        succ = succ && currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tbTagIDAndroid.Text.ToString()),
                                                Int32.Parse(tag.OwnerID.ToString()),
                                                Constants.statusActive, "", issued, validTO, false);
                                        if (succ)
                                        {
                                            succ = writeToCard();
                                            if (succ)
                                            {
                                                MessageBox.Show(rm.GetString("successWritingToCard", culture));
                                                currentTag.CommitTransaction();
                                            }
                                            else
                                            {
                                                MessageBox.Show(rm.GetString("notSuccessWritingToCard", culture));
                                                currentTag.RollbackTransaction();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        if (currentTag.GetTransaction() != null)
                                        {
                                            MessageBox.Show(rm.GetString("notSuccessWritingToCard", culture));
                                            currentTag.RollbackTransaction();
                                        }
                                    }
                                }
                                ((EmployeeAdd)this.Owner).setHasTagChanged(!succ);
                            }
                        }
                        else
                        {
                            if (this.cbStatus.SelectedIndex > 0)
                            {
                                ((EmployeeAdd)this.Owner).setHasTagChanged(!currentTag.Update(Int32.Parse(tag.RecordID.ToString()), UInt32.Parse(tag.TagID.ToString()),
                                    Int32.Parse(tag.OwnerID.ToString()),
                                    cbStatus.SelectedValue.ToString(), tbDescription.Text.Trim(), issued, validTO, true));
                            }
                            else
                            {
                                ((EmployeeAdd)this.Owner).setHasTagChanged(!currentTag.Update(tag.RecordID, tag.TagID, tag.OwnerID, tag.Status.Trim(), tbDescription.Text.Trim(), issued, validTO, true));
                            }
                        }
                    }
                }
                //DialogResult dialogResult = MessageBox.Show("Close?", "Closing", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                //    this.Close();
                //}
                this.Close();
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    log.writeLog(DateTime.Now + " Tags.btnOK_Click(): " + messageTagSave2 + "\n");
                    MessageBox.Show(messageTagSave2);
                }
                else
                {
                    log.writeLog(DateTime.Now + " Tags.btnOK_Click(): " + sqlex.Message + "\n");
                    MessageBox.Show(sqlex.Message);
                }
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    log.writeLog(DateTime.Now + " Tags.btnOK_Click(): " + messageTagSave2 + "\n");
                    MessageBox.Show(messageTagSave2);
                }
                else
                {
                    log.writeLog(DateTime.Now + " Tags.btnOK_Click(): " + mysqlex.Message + "\n");
                    MessageBox.Show(mysqlex.Message);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tags.btnOK_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void Tags_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateStatusCombo();
                populateTypeCombo();
                TagTO tag = currentTag.FindActive(currentTag.TgTO.OwnerID);

                switch (status)
                {
                    case "NEW":
                        this.newLoad();
                        break;
                    case "BLOCKED":
                        this.blockedLoad();
                        if (tag.RecordID < 0)
                            tbDescription.Enabled = false;
                        break;
                    case "RETIRED":
                        this.retiredLoad();
                        if (tag.RecordID < 0)
                            tbDescription.Enabled = false;
                        break;
                    case "ACTIVE":
                        this.activeLoad();
                        if (tag.RecordID < 0)
                            tbDescription.Enabled = false;
                        break;
                    case "CHANGE":
                        if (tag.RecordID > 0)
                        {
                            this.changeLoad();
                            if (tag.TagID >= 0)
                            {
                                this.tbTagID.Text = tag.TagID.ToString();
                            }
                            else
                            {
                                this.tbTagID.Text = "";
                            }
                            this.tbDescription.Text = tag.Description;
                        }
                        else
                        {
                            this.status = "NEW";
                            this.newLoad();
                        }
                        break;
                    case "ANDROIDNEW": // nova android kartica
                        
                        btnOK.Text = rm.GetString("programCard", culture);
                        btnFromReader.Text = rm.GetString("btnReadFromCard", culture);
                        this.newLoadAndroid();
                        break;
                    case "ANDROIDCHANGE": // menjanje android kartice

                        btnOK.Text = rm.GetString("programCard", culture);
                        btnFromReader.Text = rm.GetString("btnReadFromCard", culture);
                        if (tag.RecordID > 0)
                        {
                            this.changeAndroidLoad();
                        }
                        else
                        {
                            this.status = "ANDROIDNEW";
                            this.newLoadAndroid();
                        }
                        break;
                    case "ANDROIDBLOCKED":
                        this.blockedAndroidLoad();
                        if (tag.RecordID < 0)
                        {
                            tbDescAndroid.Enabled = false;
                        }
                        break;
                    case "ANDROIDRETIRED":
                        this.retiredAndroidLoad();
                        if (tag.RecordID < 0)
                        {
                            tbDescAndroid.Enabled = false;
                        }
                        break;
                    case "ANDOROIDDACTIVE":
                        this.activeAndroidLoad();
                        if (tag.RecordID < 0)
                        {
                            tbDescAndroid.Enabled = false;
                        } 
                        break;

                }

                if (tag.Issued == new DateTime())
                {
                    chbIssuedUndefined.Checked = true;
                    chbIssuedUndefined_CheckedChanged(this, new EventArgs());
                }
                else
                {
                    chbIssuedUndefined.Checked = false;
                    chbIssuedUndefined_CheckedChanged(this, new EventArgs());
                    dtpIssued.Value = tag.Issued.Date;
                }

                if (tag.ValidTO == new DateTime())
                {
                    chbValidToUndefined.Checked = true;
                    chbValidToUndefined_CheckedChanged(this, new EventArgs());
                }
                else
                {
                    chbValidToUndefined.Checked = false;
                    chbValidToUndefined_CheckedChanged(this, new EventArgs());
                    dtpValidTo.Value = tag.ValidTO.Date;
                }

                bool datesVisible = false;
                if (status == "NEW" || status == "CHANGE")
                {
                    string costumer = Common.Misc.getCustomer(null);
                    int cost = 0;
                    if (int.TryParse(costumer, out cost) && (cost == (int)Constants.Customers.AAC))
                        datesVisible = true;
                }

                lblIssued.Visible = lblValidTo.Visible = dtpIssued.Visible = dtpValidTo.Visible = chbIssuedUndefined.Visible = chbValidToUndefined.Visible = datesVisible;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tags.Tags_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void newLoad()
		{
			this.Text = rm.GetString("newTag", culture);
			this.tbTagID.Visible = true;
			this.lblTagID.Visible = true;
			this.lblStatus.Visible = false;
			this.cbStatus.Visible = false;
			this.lblDescription.Visible = false;
			this.tbDescription.Visible = false;
			this.btnOK.Visible = true;
			this.btnFromReader.Visible = true;

            gbTags.Visible = true;
            gbActaAndroid.SendToBack();
		}

		private void blockedLoad()
		{
			this.Text = rm.GetString("blockTag", culture);
			this.tbTagID.Visible = false;
			this.lblTagID.Visible = false;
			this.lblStatus.Visible = false;
			this.cbStatus.Visible = false;
			this.lblDescription.Visible = true;
			this.tbDescription.Visible = true;
			this.btnOK.Visible = true;
			this.btnFromReader.Visible = false;

            gbTags.Visible = true;
            gbActaAndroid.SendToBack();
		}

		private void retiredLoad()
		{
			this.Text = rm.GetString("retireTag", culture);
			this.tbTagID.Visible = false;
			this.lblTagID.Visible = false;
			this.lblStatus.Visible = false;
			this.cbStatus.Visible = false;
			this.lblDescription.Visible = true;
			this.tbDescription.Visible = true;
			this.btnOK.Visible = true;
			this.btnFromReader.Visible = false;

            gbTags.Visible = true;
            gbActaAndroid.SendToBack();
		}

		private void activeLoad()
		{
			this.Text = rm.GetString("activeTag", culture);
			this.tbTagID.Visible = false;
			this.lblTagID.Visible = false;
			this.lblStatus.Visible = false;
			this.cbStatus.Visible = false;
			this.lblDescription.Visible = true;
			this.tbDescription.Visible = true;
			this.btnOK.Visible = true;
			this.btnFromReader.Visible = false;

            gbTags.Visible = true;
            gbActaAndroid.SendToBack();
		}

		private void changeLoad()
		{
			this.Text = rm.GetString("changeTag", culture);
			this.tbTagID.Visible = true;
			this.lblTagID.Visible = true;
			this.lblStatus.Visible = true;
			this.cbStatus.Visible = true;
			this.lblDescription.Visible = true;
			this.tbDescription.Visible = true;
			this.btnOK.Visible = true;
			this.btnFromReader.Visible = false;
			this.tbTagID.Enabled = false;

            gbTags.Visible = true;
            gbActaAndroid.SendToBack();
		}

        private void newLoadAndroid()
        {
            this.Text = rm.GetString("newTag", culture);
            gbTags.Visible = false;
            gbTags.SendToBack();

            btnOK.Visible = true;
            btnFromReader.Enabled = false;

            gbActaAndroid.Visible = true;
            cbTypeAndroid.SelectedIndex = 0;
            lblDescAndroid.Visible = false;
            lblStatusAndroid.Visible = false;
            cbStatusAndroid.Visible = false;
            tbDescAndroid.Visible = false;
            accessControlDevice = new RFIDDevice();
        }

        private void changeAndroidLoad()
        {
            this.Text = rm.GetString("changeTag", culture);
            gbTags.Visible = false;
            gbTags.SendToBack();

            btnOK.Visible = true;
        //    btnOK.Enabled = false;
            btnFromReader.Enabled = true;

            gbActaAndroid.Visible = true;
            this.tbTagIDAndroid.Text = "";
            cbTypeAndroid.SelectedItem = -1;
            accessControlDevice = new RFIDDevice();
        }

        private void blockedAndroidLoad()
        {
            this.Text = rm.GetString("blockTag", culture);
            this.tbTagIDAndroid.Visible = false;
            this.lblTagAndroid.Visible = false;
            this.lblStatusAndroid.Visible = false;
            this.cbStatusAndroid.Visible = false;
            this.lblTypeAndroid.Visible = false;
            this.cbTypeAndroid.Visible = false;
            this.lblDescAndroid.Visible = true;
            this.tbDescAndroid.Visible = true;
            this.btnOK.Visible = true;
            this.btnFromReader.Visible = false;

            accessControlDevice = new RFIDDevice();
        }

        private void retiredAndroidLoad()
        {
            this.Text = rm.GetString("retireTag", culture);
            this.tbTagIDAndroid.Visible = false;
            this.lblTagAndroid.Visible = false;
            this.lblStatusAndroid.Visible = false;
            this.lblTypeAndroid.Visible = false;
            this.cbTypeAndroid.Visible = false;
            this.cbStatusAndroid.Visible = false;
            this.lblDescAndroid.Visible = true;
            this.tbDescAndroid.Visible = true;
            this.btnOK.Visible = true;
            this.btnFromReader.Visible = false;

            accessControlDevice = new RFIDDevice();
        }

        private void activeAndroidLoad()
        {
            this.Text = rm.GetString("activeTag", culture);
            this.tbTagIDAndroid.Visible = false;
            this.lblTagAndroid.Visible = false;
            this.lblStatusAndroid.Visible = false;
            this.cbStatusAndroid.Visible = false;
            this.lblTypeAndroid.Visible = false;
            this.cbTypeAndroid.Visible = false;
            this.lblDescAndroid.Visible = true;
            this.tbDescAndroid.Visible = true;
            this.btnOK.Visible = true;
            this.btnFromReader.Visible = false;

            accessControlDevice = new RFIDDevice();

        }

		private void ClearForm()
		{
			this.tbTagID.Text = "";
			this.cbStatus.SelectedIndex = 0;
			this.tbDescription.Text = "";

            this.tbTagIDAndroid.Text = "";
            this.cbTypeAndroid.SelectedIndex = 0;
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                TagTO tag = currentTag.FindActive(currentTag.TgTO.OwnerID);
                if (tag.RecordID >= 0)
                {
                    MessageBox.Show(messageTagNew1);
                }
                else
                {
                    this.ClearForm();
                    this.newLoad();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tags.btnNew_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void Tags_Closed(object sender, System.EventArgs e)
		{
			if (this.status.Equals("ACTIVE") || this.status.Equals("BLOCKED") || this.status.Equals("RETIRED"))
			{
				//btnOK.PerformClick();
			}
		}

		private void btnFromReader_Click(object sender, System.EventArgs e)
		{
            if(this.status.Equals("ANDROIDCHANGE"))
            {
                IReaderInterface rf1 = ReaderFactory.GetReader;
                int port = rf1.FindDesktopReader();
                int status = 0;
                if (port != 0)
                {
                    status = accessControlDevice.Open(port, 38400);
                }
                else
                {
                    MessageBox.Show(rm.GetString("readerNotFound", culture));
                    return;
                }
                Cursor.Current = Cursors.Default;
                if (status == 0)
                {
                    MessageBox.Show(rm.GetString("portCannotOpen", culture));
                    accessControlDevice.Close();
                    return;
                }

                ulong serno = accessControlDevice.GetTagSerialNumber(0);
                if (serno == 0)
                {
                    MessageBox.Show(rm.GetString("noTag", culture));
                    accessControlDevice.Close();
                    return;
                }

                selfGeneratedPass = accessControlDevice.SGP(); //set self generated password

                status = accessControlDevice.SGPInitCard(1); // set self generated password on sector no. 1
                if (status == 0)
                {
                    //Greška u promeni lozinke!
                    return;

                }

                byte[] b0 = new byte[16]; byte[] b1 = new byte[16]; byte[] b2 = new byte[16]; byte[] b3 = new byte[16];
                int sector = 1;
                status = accessControlDevice.ReadProtectedBlock(0, 0, sector * 4 + 0, selfGeneratedPass, b0);
                status = accessControlDevice.ReadProtectedBlock(0, 0, sector * 4 + 1, selfGeneratedPass, b1);
                status = accessControlDevice.ReadProtectedBlock(0, 0, sector * 4 + 2, selfGeneratedPass, b2);
                if (status != 1)
                {
                    MessageBox.Show(rm.GetString("errorReadingSector", culture));
                    accessControlDevice.Close();
                }
                else
                {
                    accessControlDevice.Close();
                    //prvi bajt je tip kartice
                    int type = Convert.ToInt32(b0[0]);

                    //naredna 4iri bajta predstavljaju integer koji označava ID zaposlenog u sistemu u Little Endian redosledu
                    int id = (int)b0[1] + ((int)b0[2] << 8) + ((int)b0[3] << 16) + ((int)b0[4] << 24);
                    if (Convert.ToInt32(lblEmployeeID.Text) != id || Convert.ToInt32(lblEmployeeID.Text) != id)
                    {
                        MessageBox.Show(rm.GetString("wrongCard", culture));
                        return;
                    }

                    ////ime i prezime uzima 30 karaktera
                    byte[] bufferName = new byte[30];
                    int j = 5; // u prvom bloku 11 mesta je za ime, prvih 5 je bio tip kartice i ID
                    int k = 0; //  u drugom bloku svih 16 je za ime
                    int l = 0; // u trecem bloku, prva 3 polja su za ime i to daje 11+16+3=30 karaktera
                    for (int i = 0; i < 30; i++)
                    {
                        if (j < 16)
                        {
                            bufferName[i] = b0[j]; //deo iz bloka 4
                            j++;
                        }
                        else
                        {
                            if (k < 16)
                            {
                                bufferName[i] = b1[k]; // deo iz bloka 5
                                k++;
                            }
                            else
                            {
                                if (l < 4)
                                {
                                    bufferName[i] = b2[l]; //deo iz bloka 6
                                    l++;
                                }
                            }
                        }
                    }
                    string name = Encoding.UTF8.GetString(bufferName, 0, bufferName.Length);

                    //string oldName = name.Substring(0, name.IndexOf('/'));
                    if (!name.StartsWith(lblEmployee.Text))
                    {
                      //  MessageBox.Show("Kartica nije odgovarajuca");
                      //  return;
                    }
                    tbTagIDAndroid.Text = serno.ToString();
                    switch (type)
                    {
                        case 0:
                            cbTypeAndroid.Text = Constants.typeManager;
                            break;
                        case 1:
                            cbTypeAndroid.Text = Constants.typeSupervisor;
                            break;
                        case 2:
                            cbTypeAndroid.Text = Constants.typeEmployee;
                            break;
                    }
                //    btnOK.Enabled = true;

                }
            }
            else
            {
			    try
                {
                    
				    //int COMPort = Int32.Parse(ConfigurationManager.AppSettings["DesktopReaderAddress"]);
                    uint tagID = 0;
                    this.Cursor = Cursors.WaitCursor;
                    if (desktopReaderCOMPort == 0) desktopReaderCOMPort = readerInterface.FindDesktopReader();
                    this.Cursor = Cursors.Arrow;
                    if (desktopReaderCOMPort == 0)
                    {
                        MessageBox.Show(rm.GetString("noDesktopReader", culture));
                        return;
                    }
                    else
                    {
                        tagID = UInt32.Parse(readerInterface.GetTagID(desktopReaderCOMPort));
                    }
    			
				    if (tagID == 0)
				    {
					    MessageBox.Show(rm.GetString("noTagOnReader", culture));
				    }
				    else
				    {
					    this.tbTagID.Text = tagID.ToString().Trim();
				    }
			    }
			    catch (Exception ex)
			    {
				    log.writeLog(DateTime.Now + " Tags.btnFromReader_Click(): " + ex.Message + "\n");
                    MessageBox.Show(rm.GetString("noTagOnReader", culture));
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                }
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
                log.writeLog(DateTime.Now + " Tags.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void Tags_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Tags.Tags_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbIssuedUndefined_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (chbIssuedUndefined.Checked)
                    dtpIssued.Value = Constants.dateTimeNullValue();
                else
                    dtpIssued.Value = DateTime.Now.Date;

                dtpIssued.Enabled = !chbIssuedUndefined.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tags.chbIssuedUndefined_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbValidToUndefined_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (chbValidToUndefined.Checked)
                    dtpValidTo.Value = Constants.dateTimeNullValue();
                else
                    dtpValidTo.Value = DateTime.Now.Date;

                dtpValidTo.Enabled = !chbValidToUndefined.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tags.chbValidToUndefined_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool readTagIDFromReader()
        {
            IReaderInterface rf1 = ReaderFactory.GetReader;
            int port = rf1.FindDesktopReader();
            int status = 0;

            if (port != 0)
            {
                status = accessControlDevice.Open(port, 38400);
            }
            else
            {
                MessageBox.Show(rm.GetString("noReaders", culture));
                return false;
            }
            Cursor.Current = Cursors.Default;
            if (status == 0)
            {
                MessageBox.Show(rm.GetString("portCannotOpen", culture));
                accessControlDevice.Close();
                return false;
            }

            ulong serno = accessControlDevice.GetTagSerialNumber(0);
            accessControlDevice.Close();
            if (serno == 0)
            {
                MessageBox.Show(rm.GetString("noTag", culture));
                return false;
            }
            else
            {
                tbTagIDAndroid.Text = serno.ToString();
                return true;
            }
        }
        
        byte[] selfGeneratedPass = new byte[6]; // set generated password

        //pisanje po kartici
        private bool writeToCard()
        {
            IReaderInterface rf1 = ReaderFactory.GetReader;
            int port = rf1.FindDesktopReader();
            int status = 0;

            if (port != 0)
            {
                status = accessControlDevice.Open(port, 38400);
            }
            else
            {
                MessageBox.Show(rm.GetString("noReaders", culture));
                return false;
            }
            Cursor.Current = Cursors.Default;
            if (status == 0)
            {
                MessageBox.Show(rm.GetString("portCannotOpen", culture));
                accessControlDevice.Close();
                return false;
            }

            ulong serno = accessControlDevice.GetTagSerialNumber(0);
            if (serno == 0)
            {
                MessageBox.Show(rm.GetString("noTag", culture));
                accessControlDevice.Close();
                return false;
            }
            else
            {
                tbTagIDAndroid.Text = serno.ToString();
            }

            selfGeneratedPass = accessControlDevice.SGP(); //set self generated password

            status = accessControlDevice.SGPInitCard(1); // set self generated password on sector no. 1

            if (status == 0)
            {
                //Greška u promeni lozinke!
                return false;
            }

            //blokovi za pisanje
            byte[] wb0 = new byte[16]; byte[] wb1 = new byte[16]; byte[] wb2 = new byte[16];

            //tip kartice
            int typeOfCard = 0;
            switch (cbTypeAndroid.Text)
            {
                case "Manager":
                    typeOfCard = 0;
                    break;
                case "Supervisor":
                    typeOfCard = 1;
                    break;
                case "Employee":
                    typeOfCard = 2;
                    break;
            }

            byte typeOfCardByte = Convert.ToByte(typeOfCard);
            //upisuje se tip kartice (1 bajta)
            wb0[0] = typeOfCardByte;

            //id zaposlenog
            int employeeID = Convert.ToInt32(lblEmployeeID.Text);
            byte[] employeeIDBytes = Int2LittleEndian(employeeID);

            //upisuje se ID zaposlenog, 4 bajta predstavlja little endian integer
            for (int i = 1; i < 5; i++)
            {
                wb0[i] = employeeIDBytes[i - 1];
            }

            //30 bajtova je za ime i prezime, bez č,ć,ž,đ, ukoliko je >30 seče se
            string name = lblEmployee.Text;

            string fixedName = fixString(name);
            byte[] asciiNameBytes = Encoding.ASCII.GetBytes(fixedName);
            int nameLength = asciiNameBytes.Length;
            if (nameLength > 30)
            {
                name = name.Substring(0, 30);
            }

            //upisuje se ime (30 bajta)
            int k = 0;
            for (int i = 5; i < 16; i++)
            {
                if (k < nameLength)
                {
                    wb0[i] = asciiNameBytes[k];
                    k++;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (k < nameLength)
                {
                    wb1[i] = asciiNameBytes[k];
                    k++;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (k < nameLength)
                {
                    wb2[i] = asciiNameBytes[k];
                    k++;
                }
            }

            int sector = 1;
            //pisanje
            status = accessControlDevice.WriteProtectedBlock(0, 0, sector * 4 + 0, selfGeneratedPass, wb0);
            status = accessControlDevice.WriteProtectedBlock(0, 0, sector * 4 + 1, selfGeneratedPass, wb1);
            status = accessControlDevice.WriteProtectedBlock(0, 0, sector * 4 + 2, selfGeneratedPass, wb2);

            //blokovi za citanje
            byte[] rb0 = new byte[16]; byte[] rb1 = new byte[16]; byte[] rb2 = new byte[16];
            //citanje
            status = accessControlDevice.ReadProtectedBlock(0, 0, sector * 4 + 0, selfGeneratedPass, rb0);
            status = accessControlDevice.ReadProtectedBlock(0, 0, sector * 4 + 1, selfGeneratedPass, rb1);
            status = accessControlDevice.ReadProtectedBlock(0, 0, sector * 4 + 2, selfGeneratedPass, rb2);

            accessControlDevice.Close();

            if (status != 1)
            {
                return false;
            }
            else
            {
                //provera da li su isti
                for (int i = 0; i < 16; i++)
                {
                    if (wb0[i] != rb0[i] || wb1[i] != rb1[i] || wb2[i] != rb2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private byte[] Int2LittleEndian(int i)
        {
            byte[] b = new byte[4];
            b[0] = (byte)((i & (0x000000FF)) >> 0); b[1] = (byte)((i & (0x0000FF00)) >> 8);
            b[2] = (byte)((i & (0x00FF0000)) >> 16); b[3] = (byte)((i & (0xFF000000)) >> 24);
            return b;
        }

        private string fixString(string name)
        {
            string newName = name;
            newName = newName.Replace("đ", "dj");
            newName = newName.Replace("Đ", "Dj");
            newName = newName.Replace("ž", "z");
            newName = newName.Replace("Ž", "Z");
            newName = newName.Replace("ć", "c");
            newName = newName.Replace("Ć", "C");
            newName = newName.Replace("č", "c");
            newName = newName.Replace("Č", "C");
            newName = newName.Replace("š", "s");
            newName = newName.Replace("Š", "S");
            return newName;
        }
	}
}
