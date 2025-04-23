using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Resources;
using System.Globalization;
using System.Data;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for WorkingUnitsAdd.
	/// </summary>
	public class WorkingUnitsAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox cbParentUnitID;
		private System.Windows.Forms.Label lblParentWUID;
		private System.Windows.Forms.Label lblStar1;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.TextBox tbWorkingUnitID;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblWorkingUnitID;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		WorkingUnitTO currentWorkingUnit = null;
		ApplUsersXWUTO currentApplUsersXWU = null;
		DebugLog log;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;

		string messageWUSave1 = "";
		string messageWUSave2 = "";
		string messageWUSave3 = "";
		string messageWUSave4 = "";
		private System.Windows.Forms.Button btnAddresses;
		private System.Windows.Forms.Label lblAddressID;
		private System.Windows.Forms.TextBox tbAddress;
		string messageWUUpd3 = "";
        private Button btnWUTree;
        string messageWUUpd4 = "";

        List<WorkingUnitTO> wuArray;

		// Add Form
		public WorkingUnitsAdd()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
			
			currentWorkingUnit = new WorkingUnitTO();
			logInUser = NotificationController.GetLogInUser();

			int wuID = new WorkingUnit().FindMAXWUID();

			this.CenterToScreen();

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource", typeof(WorkingUnits).Assembly);
			
			this.cbParentUnitID.DropDownStyle = ComboBoxStyle.DropDownList;

			populateParentUnit();

			wuID++;
			tbWorkingUnitID.Text = wuID.ToString().Trim();
			btnUpdate.Visible = false;
			setLanguage();
		}

		// Update Form
		public WorkingUnitsAdd(WorkingUnitTO wu)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentWorkingUnit = new WorkingUnitTO(wu.WorkingUnitID, wu.ParentWorkingUID, wu.Description, wu.Name, wu.Status, wu.AddressID);

			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource", typeof(WorkingUnits).Assembly);
			
			this.cbParentUnitID.DropDownStyle = ComboBoxStyle.DropDownList;

			populateParentUnit();
			populateUpdateForm(wu);
			btnSave.Visible = false;
			lblStar1.Visible = false;
			tbWorkingUnitID.Enabled = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkingUnitsAdd));
            this.cbParentUnitID = new System.Windows.Forms.ComboBox();
            this.lblParentWUID = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStar1 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbWorkingUnitID = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblWorkingUnitID = new System.Windows.Forms.Label();
            this.btnAddresses = new System.Windows.Forms.Button();
            this.lblAddressID = new System.Windows.Forms.Label();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbParentUnitID
            // 
            this.cbParentUnitID.Location = new System.Drawing.Point(152, 120);
            this.cbParentUnitID.Name = "cbParentUnitID";
            this.cbParentUnitID.Size = new System.Drawing.Size(224, 21);
            this.cbParentUnitID.TabIndex = 7;
            // 
            // lblParentWUID
            // 
            this.lblParentWUID.Location = new System.Drawing.Point(8, 120);
            this.lblParentWUID.Name = "lblParentWUID";
            this.lblParentWUID.Size = new System.Drawing.Size(128, 23);
            this.lblParentWUID.TabIndex = 6;
            this.lblParentWUID.Text = "Parent Working Unit ID:";
            this.lblParentWUID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(304, 208);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStar1
            // 
            this.lblStar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar1.ForeColor = System.Drawing.Color.Red;
            this.lblStar1.Location = new System.Drawing.Point(384, 24);
            this.lblStar1.Name = "lblStar1";
            this.lblStar1.Size = new System.Drawing.Size(16, 16);
            this.lblStar1.TabIndex = 36;
            this.lblStar1.Text = "*";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(152, 208);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 12;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(48, 208);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(152, 56);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(224, 20);
            this.tbName.TabIndex = 3;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(152, 88);
            this.tbDescription.MaxLength = 50;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(224, 20);
            this.tbDescription.TabIndex = 5;
            // 
            // tbWorkingUnitID
            // 
            this.tbWorkingUnitID.Location = new System.Drawing.Point(152, 24);
            this.tbWorkingUnitID.Name = "tbWorkingUnitID";
            this.tbWorkingUnitID.Size = new System.Drawing.Size(224, 20);
            this.tbWorkingUnitID.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(16, 56);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(120, 23);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(16, 88);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(120, 23);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWorkingUnitID
            // 
            this.lblWorkingUnitID.Location = new System.Drawing.Point(16, 24);
            this.lblWorkingUnitID.Name = "lblWorkingUnitID";
            this.lblWorkingUnitID.Size = new System.Drawing.Size(120, 23);
            this.lblWorkingUnitID.TabIndex = 0;
            this.lblWorkingUnitID.Text = "Working Unit ID:";
            this.lblWorkingUnitID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAddresses
            // 
            this.btnAddresses.Location = new System.Drawing.Point(256, 160);
            this.btnAddresses.Name = "btnAddresses";
            this.btnAddresses.Size = new System.Drawing.Size(120, 23);
            this.btnAddresses.TabIndex = 10;
            this.btnAddresses.Text = "Address >>>";
            this.btnAddresses.Click += new System.EventHandler(this.btnAddresses_Click);
            // 
            // lblAddressID
            // 
            this.lblAddressID.Location = new System.Drawing.Point(40, 160);
            this.lblAddressID.Name = "lblAddressID";
            this.lblAddressID.Size = new System.Drawing.Size(100, 23);
            this.lblAddressID.TabIndex = 8;
            this.lblAddressID.Text = "Address ID:";
            this.lblAddressID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAddress
            // 
            this.tbAddress.Enabled = false;
            this.tbAddress.Location = new System.Drawing.Point(152, 160);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(64, 20);
            this.tbAddress.TabIndex = 9;
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(382, 118);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 37;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // WorkingUnitsAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(416, 241);
            this.ControlBox = false;
            this.Controls.Add(this.btnWUTree);
            this.Controls.Add(this.tbAddress);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbWorkingUnitID);
            this.Controls.Add(this.lblAddressID);
            this.Controls.Add(this.btnAddresses);
            this.Controls.Add(this.cbParentUnitID);
            this.Controls.Add(this.lblParentWUID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStar1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblWorkingUnitID);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(432, 280);
            this.MinimumSize = new System.Drawing.Size(432, 280);
            this.Name = "WorkingUnitsAdd";
            this.ShowInTaskbar = false;
            this.Text = "WorkingUnitsAdd";
            this.Load += new System.EventHandler(this.WorkingUnitsAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WorkingUnitsAdd_KeyUp);
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
				if (currentWorkingUnit.WorkingUnitID >= 0)
				{
					this.Text = rm.GetString("updateWU", culture);
				}
				else
				{
					this.Text = rm.GetString("addWU", culture);
				}

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnAddresses.Text = rm.GetString("btnAddress", culture);

				// label's text
				lblWorkingUnitID.Text = rm.GetString("lblWorkingUnitID", culture);
				lblParentWUID.Text = rm.GetString("lblParentWUID", culture);
				lblDescription.Text = rm.GetString("lblDescription", culture);
				lblName.Text = rm.GetString("lblName", culture);
				lblAddressID.Text = rm.GetString("lblAddressID", culture);

				// message's text
				messageWUSave1 = rm.GetString("messageWUSave1", culture);
				messageWUSave2 = rm.GetString("messageWUSave2", culture);
				messageWUSave3 = rm.GetString("messageWUSave3", culture);
				messageWUSave4 = rm.GetString("messageWUSave4", culture);
				messageWUUpd3 = rm.GetString("messageWUUpd3", culture);
                messageWUUpd4 = rm.GetString("messageWUUpd4", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingUnitsAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populete form with data of working unit to update
		/// </summary>
		/// <param name="loc"></param>
		public void populateUpdateForm(WorkingUnitTO wu)
		{
			this.tbWorkingUnitID.Text = (wu.WorkingUnitID < 0) ? "" : wu.WorkingUnitID.ToString();
			this.tbName.Text = wu.Name.ToString();
			this.tbDescription.Text = wu.Description.ToString();
			this.cbParentUnitID.SelectedValue = (wu.ParentWorkingUID < 0) ? 0 : wu.ParentWorkingUID;
			this.tbAddress.Text = (wu.AddressID < 0) ? "0" : wu.AddressID.ToString();
		}

		/// <summary>
		/// Populate combo box contains posible parent working units
		/// </summary>
		private void populateParentUnit()
		{
            WorkingUnit wUnit = new WorkingUnit();
            wUnit.WUTO.Status = Constants.DefaultStateActive;
            List<WorkingUnitTO> workingUnits = wUnit.Search();
            wuArray = new List<WorkingUnitTO>();
            List<int> wuIDs = new List<int>();
            if(currentWorkingUnit.WorkingUnitID != -1)
            {
                List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
                wUnits.Add(currentWorkingUnit);
                wUnits = new WorkingUnit().FindAllChildren(wUnits);
                if (wUnits.Count > 0)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if(wu.WorkingUnitID != currentWorkingUnit.WorkingUnitID)
                        wuIDs.Add(wu.WorkingUnitID);
                    }
                }
            }
			
			// Add All as a first member of combo
			wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), Constants.DefaultStateActive.ToString(), 0));

			foreach(WorkingUnitTO wuMember in workingUnits)
			{
                if (!wuIDs.Contains(wuMember.WorkingUnitID))
                {
                    if (wuMember.WorkingUnitID == 0)
                    {
                        wuArray.Insert(1, wuMember);
                    }
                    else
                    {
                        if (currentWorkingUnit.WorkingUnitID != 0)
                        {
                            if ((wuMember.ParentWorkingUID != currentWorkingUnit.WorkingUnitID) || (wuMember.WorkingUnitID == 0))
                            {
                                wuArray.Add(wuMember);
                            }
                            else
                            {
                                if (wuMember.WorkingUnitID == currentWorkingUnit.WorkingUnitID)
                                {
                                    wuArray.Add(wuMember);
                                }
                            }
                        }
                        else
                        {
                            wuArray.Add(wuMember);
                        }
                    }
                }			
			}

			cbParentUnitID.DataSource = wuArray;
			cbParentUnitID.DisplayMember = "Name";
			cbParentUnitID.ValueMember = "WorkingUnitID";
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Save new Working Unit
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (this.tbWorkingUnitID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(messageWUSave1);
                    return;
                }
                else
                {
                    try
                    {
                        if (!tbWorkingUnitID.Text.Trim().Equals(""))
                        {
                            Int32.Parse(tbWorkingUnitID.Text.Trim());
                        }
                    }
                    catch
                    {
                        MessageBox.Show(messageWUSave2);
                        return;
                    }
                }

                WorkingUnit wUnit = new WorkingUnit();
                wUnit.WUTO.Name = tbName.Text.Trim();
                List<WorkingUnitTO> wunits = wUnit.Search();

                if (wunits.Count == 0)
                {
                    currentWorkingUnit = new WorkingUnitTO();
                    currentApplUsersXWU = new ApplUsersXWUTO();
                    ApplUsersXRole admin = new ApplUsersXRole();

                    //Set currentWorkingUnit properties if all data validations were passed
                    currentWorkingUnit.WorkingUnitID = Int32.Parse(this.tbWorkingUnitID.Text.Trim());
                    int parentWU = currentWorkingUnit.WorkingUnitID;
                    if (cbParentUnitID.SelectedIndex > 0)
                    {
                        parentWU = Int32.Parse(this.cbParentUnitID.SelectedValue.ToString().Trim());
                    }
                    currentWorkingUnit.ParentWorkingUID = parentWU;
                    currentWorkingUnit.Name = tbName.Text.Trim();
                    currentWorkingUnit.Description = tbDescription.Text.Trim();
                    currentWorkingUnit.Status = Constants.DefaultStateActive.ToString();
                    if (!tbAddress.Text.Trim().Equals(""))
                    {
                        currentWorkingUnit.AddressID = Int32.Parse(tbAddress.Text.Trim());
                    }
                    else
                    {
                        currentWorkingUnit.AddressID = 0;
                    }

                    // Save currentWorkingUnit
                    WorkingUnit wu = new WorkingUnit();
                    wu.WUTO = currentWorkingUnit;
                    int inserted = wu.Save();
                    if (inserted == 1)
                    {
                        MessageBox.Show(messageWUSave4);
                        this.Cursor = Cursors.WaitCursor;
                       
                        //if user have the right to see parent wu for some purpose, give him
                        //the right to see this wu for the same purpose
                        if (cbParentUnitID.SelectedIndex > 0)
                        {
                            ApplUsersXWU auXwu = new ApplUsersXWU();
                            auXwu.AuXWUnitTO.WorkingUnitID = parentWU;
                            List<ApplUsersXWUTO> applUsersXWU = auXwu.Search();

                            foreach (ApplUsersXWUTO userWU in applUsersXWU)
                            {
                                int insertedXWU = new ApplUsersXWU().Save(userWU.UserID, currentWorkingUnit.WorkingUnitID, userWU.Purpose);
                                if (insertedXWU == 0)
                                {
                                    MessageBox.Show(rm.GetString("roleInsertingProblem", culture) + " " + userWU.Purpose +
                                            " " + rm.GetString("roleInsertingProblem1", culture) + " " + userWU.UserID);
                                }
                            }
                        }
                        else
                        {
                            //Give everybody from Admin Role rights to see this WU for all purposes
                            //Do it only if WU don't have parent wu, otherwise, it is already done in if part
                            List<ApplUserTO> AdminUserList = new List<ApplUserTO>();
                            AdminUserList = admin.FindUsersForRoleID(0);

                            List<string> purposeValues = new List<string>();
                            purposeValues.Add(Constants.ReportPurpose);
                            purposeValues.Add(Constants.PassPurpose);
                            purposeValues.Add(Constants.IOPairPurpose);
                            purposeValues.Add(Constants.LocationPurpose);
                            purposeValues.Add(Constants.PermissionPurpose);
                            purposeValues.Add(Constants.AbsencesPurpose);
                            purposeValues.Add(Constants.EmployeesPurpose);
                            purposeValues.Add(Constants.ExtraHoursPurpose);
                            purposeValues.Add(Constants.PermVerificationPurpose);
                            purposeValues.Add(Constants.RestaurantPurpose);
                            purposeValues.Add(Constants.VacationPurpose);

                            foreach (ApplUserTO user in AdminUserList)
                            {
                                foreach (string purpose in purposeValues)
                                {
                                    int insertedXWU = new ApplUsersXWU().Save(user.UserID, currentWorkingUnit.WorkingUnitID, purpose);
                                    if (insertedXWU == 0)
                                    {
                                        MessageBox.Show(rm.GetString("roleInsertingProblem", culture) + " " + purpose +
                                            " " + rm.GetString("roleInsertingProblem1", culture) + " " + user.UserID);
                                    }
                                }
                            }
                        }

                        this.Cursor = Cursors.Arrow;

                        this.Close();
                    } // if (inserted == 1)
                }
                else
                {
                    MessageBox.Show(rm.GetString("wuNameExists", culture));
                }
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    MessageBox.Show(rm.GetString("messageWUIDExists", culture));
                }
                else
                {
                    MessageBox.Show(sqlex.Message);
                    log.writeLog(DateTime.Now + " WorkingUnits.btnSave_Click(): " + sqlex.Message + "\n");
                }
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    MessageBox.Show(rm.GetString("messageWUIDExists", culture));
                }
                else
                {
                    MessageBox.Show(mysqlex.Message);
                    log.writeLog(DateTime.Now + " WorkingUnits.btnSave_Click(): " + mysqlex.Message + "\n");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Update selected Working Unit
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                string oldWUName = currentWorkingUnit.Name;
                int oldParentWUID = currentWorkingUnit.ParentWorkingUID;

                if (!oldWUName.Equals(tbName.Text.Trim()))
                {
                    WorkingUnit wUnit = new WorkingUnit();
                    wUnit.WUTO.Name = tbName.Text.Trim();
                    List<WorkingUnitTO> wunits = wUnit.Search();

                    if (wunits.Count > 0)
                    {
                        MessageBox.Show(rm.GetString("wuNameExists", culture));
                        return;
                    }
                }

                bool isUpdated = false;
                //currentWorkingUnit = new WorkingUnit();

                //Set currentWorkingUnit properties if all data validations were passed
                currentWorkingUnit.WorkingUnitID = Int32.Parse(this.tbWorkingUnitID.Text.Trim());

                if (cbParentUnitID.SelectedIndex > 0)
                {
                    currentWorkingUnit.ParentWorkingUID = Int32.Parse(this.cbParentUnitID.SelectedValue.ToString().Trim());
                }

                currentWorkingUnit.Name = tbName.Text.Trim();
                currentWorkingUnit.Description = tbDescription.Text.Trim();
                if (!tbAddress.Text.Trim().Equals(""))
                {
                    currentWorkingUnit.AddressID = Int32.Parse(tbAddress.Text.Trim());
                }
                else
                {
                    currentWorkingUnit.AddressID = 0;
                }

                currentWorkingUnit.Status = Constants.DefaultStateActive;

                WorkingUnit wu = new WorkingUnit();
                wu.WUTO = currentWorkingUnit;
                if (isUpdated = wu.Update())
                {
                    MessageBox.Show(messageWUUpd3);

                    //if new parent is different than the old parrent
                    if (oldParentWUID != currentWorkingUnit.ParentWorkingUID)
                    {
                        MessageBox.Show(messageWUUpd4);
                        this.Cursor = Cursors.WaitCursor;

                        currentApplUsersXWU = new ApplUsersXWUTO();

                        //delete all right from previous parent
                        //only if WU was not a parent to itself
                        if (oldParentWUID != currentWorkingUnit.WorkingUnitID)
                        {
                            ApplUsersXWU auXwu = new ApplUsersXWU();
                            auXwu.AuXWUnitTO.WorkingUnitID = oldParentWUID;
                            List<ApplUsersXWUTO> applUsersXWUParent = auXwu.Search();

                            auXwu.AuXWUnitTO.WorkingUnitID = currentWorkingUnit.WorkingUnitID;
                            List<ApplUsersXWUTO> applUsersXWUChild = auXwu.Search();

                            foreach (ApplUsersXWUTO userWUP in applUsersXWUParent)
                            {
                                foreach (ApplUsersXWUTO userWUC in applUsersXWUChild)
                                {
                                    if ((userWUP.UserID == userWUC.UserID) && (userWUP.Purpose == userWUC.Purpose))
                                    {
                                        bool delete = new ApplUsersXWU().Delete(userWUP.UserID, currentWorkingUnit.WorkingUnitID, userWUP.Purpose);

                                        if (!delete)
                                        {
                                            MessageBox.Show(rm.GetString("wuRightsDeletingProblem", culture) + " " + userWUP.Purpose +
                                                    " " + rm.GetString("wuRightsDeletingProblem1", culture) + " " + userWUP.UserID);
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        if (cbParentUnitID.SelectedIndex > 0)
                        {
                            //give all rights from new parent
                            if (currentWorkingUnit.ParentWorkingUID != currentWorkingUnit.WorkingUnitID)
                            {
                                ApplUsersXWU auXwu = new ApplUsersXWU();
                                auXwu.AuXWUnitTO.WorkingUnitID = currentWorkingUnit.ParentWorkingUID;
                                List<ApplUsersXWUTO> applUsersXWUParent = auXwu.Search();

                                auXwu.AuXWUnitTO.WorkingUnitID = currentWorkingUnit.WorkingUnitID;
                                List<ApplUsersXWUTO> applUsersXWUChild = auXwu.Search();

                                foreach (ApplUsersXWUTO userWUP in applUsersXWUParent)
                                {
                                    bool notExists = true;
                                    foreach (ApplUsersXWUTO userWUC in applUsersXWUChild)
                                    {
                                        if ((userWUP.UserID == userWUC.UserID) && (userWUP.Purpose == userWUC.Purpose))
                                        {
                                            notExists = false;
                                            break;
                                        }
                                    }

                                    if (notExists)
                                    {
                                        int insertedXWU = new ApplUsersXWU().Save(userWUP.UserID, currentWorkingUnit.WorkingUnitID, userWUP.Purpose);
                                        if (insertedXWU == 0)
                                        {
                                            MessageBox.Show(rm.GetString("roleInsertingProblem", culture) + " " + userWUP.Purpose +
                                                    " " + rm.GetString("roleInsertingProblem1", culture) + " " + userWUP.UserID);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //return rights for admin role users
                                ApplUsersXRole admin = new ApplUsersXRole();
                                List<ApplUserTO> AdminUserList = new List<ApplUserTO>();
                                AdminUserList = admin.FindUsersForRoleID(0);

                                List<string> purposeValues = new List<string>();
                                purposeValues.Add(Constants.ReportPurpose);
                                purposeValues.Add(Constants.PassPurpose);
                                purposeValues.Add(Constants.IOPairPurpose);
                                purposeValues.Add(Constants.LocationPurpose);
                                purposeValues.Add(Constants.PermissionPurpose);
                                purposeValues.Add(Constants.AbsencesPurpose);
                                purposeValues.Add(Constants.EmployeesPurpose);
                                purposeValues.Add(Constants.ExtraHoursPurpose);
                                purposeValues.Add(Constants.PermVerificationPurpose);
                                purposeValues.Add(Constants.RestaurantPurpose);
                                purposeValues.Add(Constants.VacationPurpose);
                                foreach (ApplUserTO user in AdminUserList)
                                {
                                    foreach (string purpose in purposeValues)
                                    {
                                        int insertedXWU = new ApplUsersXWU().Save(user.UserID, currentWorkingUnit.WorkingUnitID, purpose);
                                        if (insertedXWU == 0)
                                        {
                                            MessageBox.Show(rm.GetString("roleInsertingProblem", culture) + " " + purpose +
                                                " " + rm.GetString("roleInsertingProblem1", culture) + " " + user.UserID);
                                        }
                                    }
                                }
                            }
                        }

                        this.Cursor = Cursors.Arrow;
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void WorkingUnitsAdd_Load(object sender, System.EventArgs e)
		{
            //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
            /*tbWorkingUnitID.Enabled = false;
            tbName.Enabled = false;
            tbDescription.Enabled = false;*/
            tbAddress.Enabled = false;
            /*cbParentUnitID.Enabled = false;
            btnWUTree.Enabled = false;*/
			
		}

		private void btnAddresses_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				int addressID = tbAddress.Text.Equals("") ? 0 : Int32.Parse(tbAddress.Text.Trim());
				Addresses addrForm = new Addresses(addressID, tbName.Text);
				addrForm.ShowDialog(this);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingUnits.btnAddresses_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void setAddressID(int addressID)
		{
			this.tbAddress.Text = (addressID < 0) ? "0" : addressID.ToString();
		}

        private void WorkingUnitsAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " WorkingUnitsAdd.WorkingUnitsAdd_KeyUp(): " + ex.Message + "\n");
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
                string wuString = "";
                foreach (WorkingUnitTO wUnit in wuArray)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbParentUnitID.SelectedIndex = cbParentUnitID.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {

        }
	}
}
