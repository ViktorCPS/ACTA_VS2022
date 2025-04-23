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

using Common;
using Util;
using TransferObjects;


namespace UI
{
	/// <summary>
	/// Summary description for GateTimeAccessProfiles.
	/// </summary>
	public class GateTimeAccessProfiles : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListView lvGateTimeAccessProfiles;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblProfile0;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		GateTimeAccessProfile currentGateTimeAccessProfile = null;

		ApplUserTO logInUser;
		ResourceManager rm;				
		private CultureInfo culture;
		DebugLog log;
		
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		private System.Windows.Forms.ComboBox cbGateTimeAccessProfiles;
		private System.Windows.Forms.Label lblProfile1;
		private System.Windows.Forms.Label lblProfile2;
		private System.Windows.Forms.Label lblProfile3;
		private System.Windows.Forms.Label lblProfile4;
		private System.Windows.Forms.Label lblProfile5;
		private System.Windows.Forms.Label lblProfile6;
		private System.Windows.Forms.Label lblProfile7;
		private System.Windows.Forms.Label lblProfile8;
		private System.Windows.Forms.Label lblProfile9;
		private System.Windows.Forms.Label lblProfile10;
		private System.Windows.Forms.Label lblProfile11;
		private System.Windows.Forms.Label lblProfile12;
		private System.Windows.Forms.Label lblProfile13;
		private System.Windows.Forms.Label lblProfile14;
		private System.Windows.Forms.Label lblProfile15;
		bool deletePermission = false;		

		public GateTimeAccessProfiles()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentGateTimeAccessProfile = new GateTimeAccessProfile();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(GateTimeAccessProfiles).Assembly);
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
            this.lvGateTimeAccessProfiles = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cbGateTimeAccessProfiles = new System.Windows.Forms.ComboBox();
            this.lblProfile0 = new System.Windows.Forms.Label();
            this.lblProfile1 = new System.Windows.Forms.Label();
            this.lblProfile2 = new System.Windows.Forms.Label();
            this.lblProfile3 = new System.Windows.Forms.Label();
            this.lblProfile7 = new System.Windows.Forms.Label();
            this.lblProfile6 = new System.Windows.Forms.Label();
            this.lblProfile5 = new System.Windows.Forms.Label();
            this.lblProfile4 = new System.Windows.Forms.Label();
            this.lblProfile15 = new System.Windows.Forms.Label();
            this.lblProfile14 = new System.Windows.Forms.Label();
            this.lblProfile13 = new System.Windows.Forms.Label();
            this.lblProfile12 = new System.Windows.Forms.Label();
            this.lblProfile11 = new System.Windows.Forms.Label();
            this.lblProfile10 = new System.Windows.Forms.Label();
            this.lblProfile9 = new System.Windows.Forms.Label();
            this.lblProfile8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvGateTimeAccessProfiles
            // 
            this.lvGateTimeAccessProfiles.FullRowSelect = true;
            this.lvGateTimeAccessProfiles.GridLines = true;
            this.lvGateTimeAccessProfiles.HideSelection = false;
            this.lvGateTimeAccessProfiles.Location = new System.Drawing.Point(144, 96);
            this.lvGateTimeAccessProfiles.Name = "lvGateTimeAccessProfiles";
            this.lvGateTimeAccessProfiles.Size = new System.Drawing.Size(496, 248);
            this.lvGateTimeAccessProfiles.TabIndex = 15;
            this.lvGateTimeAccessProfiles.UseCompatibleStateImageBehavior = false;
            this.lvGateTimeAccessProfiles.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(568, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(320, 360);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(232, 360);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 17;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(144, 360);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tbDesc
            // 
            this.tbDesc.Enabled = false;
            this.tbDesc.Location = new System.Drawing.Point(144, 48);
            this.tbDesc.MaxLength = 150;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(496, 20);
            this.tbDesc.TabIndex = 14;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(56, 48);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(80, 23);
            this.lblDesc.TabIndex = 13;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(56, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 23);
            this.lblName.TabIndex = 11;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbGateTimeAccessProfiles
            // 
            this.cbGateTimeAccessProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGateTimeAccessProfiles.Location = new System.Drawing.Point(144, 16);
            this.cbGateTimeAccessProfiles.Name = "cbGateTimeAccessProfiles";
            this.cbGateTimeAccessProfiles.Size = new System.Drawing.Size(496, 21);
            this.cbGateTimeAccessProfiles.TabIndex = 12;
            this.cbGateTimeAccessProfiles.SelectedIndexChanged += new System.EventHandler(this.cbGateTimeAccessProfiles_SelectedIndexChanged);
            // 
            // lblProfile0
            // 
            this.lblProfile0.Location = new System.Drawing.Point(16, 116);
            this.lblProfile0.Name = "lblProfile0";
            this.lblProfile0.Size = new System.Drawing.Size(120, 14);
            this.lblProfile0.TabIndex = 16;
            this.lblProfile0.Text = "Weekly schedule 0:";
            this.lblProfile0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile1
            // 
            this.lblProfile1.Location = new System.Drawing.Point(16, 130);
            this.lblProfile1.Name = "lblProfile1";
            this.lblProfile1.Size = new System.Drawing.Size(120, 14);
            this.lblProfile1.TabIndex = 17;
            this.lblProfile1.Text = "Weekly schedule 1:";
            this.lblProfile1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile2
            // 
            this.lblProfile2.Location = new System.Drawing.Point(16, 144);
            this.lblProfile2.Name = "lblProfile2";
            this.lblProfile2.Size = new System.Drawing.Size(120, 14);
            this.lblProfile2.TabIndex = 18;
            this.lblProfile2.Text = "Weekly schedule 2:";
            this.lblProfile2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile3
            // 
            this.lblProfile3.Location = new System.Drawing.Point(16, 158);
            this.lblProfile3.Name = "lblProfile3";
            this.lblProfile3.Size = new System.Drawing.Size(120, 14);
            this.lblProfile3.TabIndex = 19;
            this.lblProfile3.Text = "Weekly schedule 3:";
            this.lblProfile3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile7
            // 
            this.lblProfile7.Location = new System.Drawing.Point(16, 214);
            this.lblProfile7.Name = "lblProfile7";
            this.lblProfile7.Size = new System.Drawing.Size(120, 14);
            this.lblProfile7.TabIndex = 23;
            this.lblProfile7.Text = "Weekly schedule 7:";
            this.lblProfile7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile6
            // 
            this.lblProfile6.Location = new System.Drawing.Point(16, 200);
            this.lblProfile6.Name = "lblProfile6";
            this.lblProfile6.Size = new System.Drawing.Size(120, 14);
            this.lblProfile6.TabIndex = 22;
            this.lblProfile6.Text = "Weekly schedule 6:";
            this.lblProfile6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile5
            // 
            this.lblProfile5.Location = new System.Drawing.Point(16, 186);
            this.lblProfile5.Name = "lblProfile5";
            this.lblProfile5.Size = new System.Drawing.Size(120, 14);
            this.lblProfile5.TabIndex = 21;
            this.lblProfile5.Text = "Weekly schedule 5:";
            this.lblProfile5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile4
            // 
            this.lblProfile4.Location = new System.Drawing.Point(16, 172);
            this.lblProfile4.Name = "lblProfile4";
            this.lblProfile4.Size = new System.Drawing.Size(120, 14);
            this.lblProfile4.TabIndex = 20;
            this.lblProfile4.Text = "Weekly schedule 4:";
            this.lblProfile4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile15
            // 
            this.lblProfile15.Location = new System.Drawing.Point(16, 326);
            this.lblProfile15.Name = "lblProfile15";
            this.lblProfile15.Size = new System.Drawing.Size(120, 14);
            this.lblProfile15.TabIndex = 31;
            this.lblProfile15.Text = "Weekly schedule 15:";
            this.lblProfile15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile14
            // 
            this.lblProfile14.Location = new System.Drawing.Point(16, 312);
            this.lblProfile14.Name = "lblProfile14";
            this.lblProfile14.Size = new System.Drawing.Size(120, 14);
            this.lblProfile14.TabIndex = 30;
            this.lblProfile14.Text = "Weekly schedule 14:";
            this.lblProfile14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile13
            // 
            this.lblProfile13.Location = new System.Drawing.Point(16, 298);
            this.lblProfile13.Name = "lblProfile13";
            this.lblProfile13.Size = new System.Drawing.Size(120, 14);
            this.lblProfile13.TabIndex = 29;
            this.lblProfile13.Text = "Weekly schedule 13:";
            this.lblProfile13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile12
            // 
            this.lblProfile12.Location = new System.Drawing.Point(16, 284);
            this.lblProfile12.Name = "lblProfile12";
            this.lblProfile12.Size = new System.Drawing.Size(120, 14);
            this.lblProfile12.TabIndex = 28;
            this.lblProfile12.Text = "Weekly schedule 12:";
            this.lblProfile12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile11
            // 
            this.lblProfile11.Location = new System.Drawing.Point(16, 270);
            this.lblProfile11.Name = "lblProfile11";
            this.lblProfile11.Size = new System.Drawing.Size(120, 14);
            this.lblProfile11.TabIndex = 27;
            this.lblProfile11.Text = "Weekly schedule 11:";
            this.lblProfile11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile10
            // 
            this.lblProfile10.Location = new System.Drawing.Point(16, 256);
            this.lblProfile10.Name = "lblProfile10";
            this.lblProfile10.Size = new System.Drawing.Size(120, 14);
            this.lblProfile10.TabIndex = 26;
            this.lblProfile10.Text = "Weekly schedule 10:";
            this.lblProfile10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile9
            // 
            this.lblProfile9.Location = new System.Drawing.Point(16, 242);
            this.lblProfile9.Name = "lblProfile9";
            this.lblProfile9.Size = new System.Drawing.Size(120, 14);
            this.lblProfile9.TabIndex = 25;
            this.lblProfile9.Text = "Weekly schedule 9:";
            this.lblProfile9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfile8
            // 
            this.lblProfile8.Location = new System.Drawing.Point(16, 228);
            this.lblProfile8.Name = "lblProfile8";
            this.lblProfile8.Size = new System.Drawing.Size(120, 14);
            this.lblProfile8.TabIndex = 24;
            this.lblProfile8.Text = "Weekly schedule 8:";
            this.lblProfile8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GateTimeAccessProfiles
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(662, 390);
            this.ControlBox = false;
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
            this.Controls.Add(this.cbGateTimeAccessProfiles);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvGateTimeAccessProfiles);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(670, 424);
            this.MinimumSize = new System.Drawing.Size(670, 424);
            this.Name = "GateTimeAccessProfiles";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Gate access profiles";
            this.Load += new System.EventHandler(this.GateTimeAccessProfiles_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GateTimeAccessProfiles_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Set proper language and initialize List View
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("gateTimeAccessProfilesForm", culture);

				// button's text
				btnAdd.Text                  = rm.GetString("btnAdd", culture);
				btnUpdate.Text               = rm.GetString("btnUpdate", culture);
				btnDelete.Text               = rm.GetString("btnDelete", culture);
				btnClose.Text                = rm.GetString("btnClose", culture);

				// group box text

				// label's text
				lblName.Text      = rm.GetString("lblName", culture);
				lblDesc.Text      = rm.GetString("lblDescription", culture);
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

				// list view
				lvGateTimeAccessProfiles.BeginUpdate();
				lvGateTimeAccessProfiles.Columns.Add(rm.GetString("lblName", culture), (4 * (lvGateTimeAccessProfiles.Width - 4)) / 10, HorizontalAlignment.Left);
				lvGateTimeAccessProfiles.Columns.Add(rm.GetString("lblDescription", culture), (6 * (lvGateTimeAccessProfiles.Width - 4)) / 10, HorizontalAlignment.Left);
				lvGateTimeAccessProfiles.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfiles.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateGateTimeAccessProfilesCombo()
		{
			try
			{
				ArrayList gateTAProfileArray = new GateTimeAccessProfile().Search("");				

				cbGateTimeAccessProfiles.DataSource    = gateTAProfileArray;
				cbGateTimeAccessProfiles.DisplayMember = "Name";
				cbGateTimeAccessProfiles.ValueMember   = "GateTAProfileId";

				if (cbGateTimeAccessProfiles.Items.Count > 0)
					cbGateTimeAccessProfiles.SelectedIndex = 0;

				//cbGateTimeAccessProfiles.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfiles.populateGateTimeAccessProfilesCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with Access Groups found
		/// </summary>
		/// <param name="accessGroupsList"></param>
		private void populateListView(TransferObjects.GateTimeAccessProfileTO gtapTO)
		{
			try
			{
				lvGateTimeAccessProfiles.BeginUpdate();
				lvGateTimeAccessProfiles.Items.Clear();

				for (int i = 0; i < 16; i++)
				{
					TransferObjects.TimeAccessProfileTO tapTO = new TransferObjects.TimeAccessProfileTO();
					int profileID = -1;
					switch (i.ToString())
					{
						case "0":							
							profileID = gtapTO.GateTAProfile0;
							break;
						case "1":							
							profileID = gtapTO.GateTAProfile1;
							break;
						case "2":
							profileID = gtapTO.GateTAProfile2;
							break;
						case "3":							
							profileID = gtapTO.GateTAProfile3;
							break;
						case "4":
							profileID = gtapTO.GateTAProfile4;
							break;
						case "5":					
							profileID = gtapTO.GateTAProfile5;
							break;
						case "6":							
							profileID = gtapTO.GateTAProfile6;
							break;
						case "7":							
							profileID = gtapTO.GateTAProfile7;
							break;
						case "8":							
							profileID = gtapTO.GateTAProfile8;
							break;
						case "9":							
							profileID = gtapTO.GateTAProfile9;
							break;
						case "10":							
							profileID = gtapTO.GateTAProfile10;
							break;
						case "11":							
							profileID = gtapTO.GateTAProfile11;
							break;
						case "12":							
							profileID = gtapTO.GateTAProfile12;
							break;
						case "13":							
							profileID = gtapTO.GateTAProfile13;
							break;
						case "14":							
							profileID = gtapTO.GateTAProfile14;
							break;
						case "15":							
							profileID = gtapTO.GateTAProfile15;
							break;
					}
					if (profileID != -1)
					{
						tapTO = (new TimeAccessProfile()).Find(profileID.ToString());
					
						ListViewItem item = new ListViewItem();
						item.Text = tapTO.Name.Trim();
						item.SubItems.Add(tapTO.Description.Trim());
						item.Tag = tapTO.TimeAccessProfileId;
						
						lvGateTimeAccessProfiles.Items.Add(item);
					}
					else
					{
						ListViewItem item = new ListViewItem();
						item.Tag = -1;
						
						lvGateTimeAccessProfiles.Items.Add(item);
					}
				}

				lvGateTimeAccessProfiles.EndUpdate();
				lvGateTimeAccessProfiles.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfiles.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void GateTimeAccessProfiles_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                populateGateTimeAccessProfilesCombo();
                //to fill list view
                cbGateTimeAccessProfiles_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GateTimeAccessProfiles.GateTimeAccessProfiles_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				// Open Add Form
				GateTimeAccessProfileAdd addForm = new GateTimeAccessProfileAdd(); 
				addForm.ShowDialog(this);

				populateGateTimeAccessProfilesCombo();	
				currentGateTimeAccessProfile.Clear();
				this.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfiles.btnAdd_Click(): " + ex.Message + "\n");
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

				if (cbGateTimeAccessProfiles.Items.Count == 0 || cbGateTimeAccessProfiles.SelectedIndex < 0)
				{
					MessageBox.Show(rm.GetString("selOneGateTAProfile", culture));
				}
				else 
				{			
					currentGateTimeAccessProfile.ReceiveTransferObject(currentGateTimeAccessProfile.Find(cbGateTimeAccessProfiles.SelectedValue.ToString()));					

					// Open Update Form
					GateTimeAccessProfileAdd addForm = new GateTimeAccessProfileAdd(currentGateTimeAccessProfile);
					addForm.ShowDialog(this);

					populateGateTimeAccessProfilesCombo();	
					currentGateTimeAccessProfile.Clear();
					this.Invalidate();
				}

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfiles.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (cbGateTimeAccessProfiles.Items.Count == 0 || cbGateTimeAccessProfiles.SelectedIndex < 0)
				{
					MessageBox.Show(rm.GetString("noSelGateTAProfileDel", culture));
				}
				else
				{
					DialogResult result = MessageBox.Show(rm.GetString("deleteGateTAProfiles", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;

						if (cbGateTimeAccessProfiles.SelectedValue.ToString() == "0")
						{
							MessageBox.Show(rm.GetString("defaultGateTAProfile", culture));
							return;
						}
						else
						{
							// Find if exists Gate that use this Gate access profile
							List<GateTO> gates = new Gate().SearchWithGateTAProfile(cbGateTimeAccessProfiles.SelectedValue.ToString());
							if (gates.Count > 0)
							{
								MessageBox.Show(rm.GetString("gateTAProfileHasGates", culture));
								return;
							}
							else
							{
								isDeleted = currentGateTimeAccessProfile.Delete(cbGateTimeAccessProfiles.SelectedValue.ToString()) && isDeleted;
							}							
						}

						if (isDeleted)
						{
							MessageBox.Show(rm.GetString("gateTAProfileDel", culture));
						}
						else
						{
							MessageBox.Show(rm.GetString("noGateTAProfileDel", culture));
						}

						populateGateTimeAccessProfilesCombo();	
						currentGateTimeAccessProfile.Clear();						
						this.Invalidate();
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfiles.btnDelete_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GateTimeAccessProfiles.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void setVisibility()
		{
			try
			{
				int permission;

				foreach (ApplRoleTO role in currentRoles)
				{
					permission       = (((int[]) menuItemsPermissions[menuItemID])[role.ApplRoleID]);
					readPermission   = readPermission || (((permission / 8) % 2) == 0 ? false : true);
					addPermission    = addPermission || (((permission / 4) % 2) == 0 ? false : true);
					updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
					deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
				}

				btnAdd.Enabled    = addPermission;
				btnUpdate.Enabled = updatePermission;
				btnDelete.Enabled = deletePermission;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfiles.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void cbGateTimeAccessProfiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				if (cbGateTimeAccessProfiles.Items.Count > 0 && cbGateTimeAccessProfiles.SelectedIndex >= 0
					&& cbGateTimeAccessProfiles.ValueMember != "")
				{
					TransferObjects.GateTimeAccessProfileTO gtapTO = new TransferObjects.GateTimeAccessProfileTO();
					gtapTO = currentGateTimeAccessProfile.Find(cbGateTimeAccessProfiles.SelectedValue.ToString());

					if (gtapTO.GateTAProfileId != -1)
					{
						tbDesc.Text = gtapTO.Description.Trim();
						populateListView(gtapTO);
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GateTimeAccessProfiles.cbGateTimeAccessProfiles_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void GateTimeAccessProfiles_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " GateTimeAccessProfiles.GateTimeAccessProfiles_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
