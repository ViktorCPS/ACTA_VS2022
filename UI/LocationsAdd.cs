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
using MySql.Data.MySqlClient;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for LocationsAdd.
	/// </summary>
	public class LocationsAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbAddressID;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbLocationID;
		private System.Windows.Forms.Label lblAddressID;
		private System.Windows.Forms.ComboBox cbPrentLocation;
		private System.Windows.Forms.Button btnAddress;
		private System.Windows.Forms.Label lblStar1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label lblParentLocationID;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblLocationID;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		LocationTO currentLocation = null;
		ResourceManager rm;
		ApplUserTO logInUser;

		private CultureInfo culture;
		private System.Windows.Forms.Button btnUpdate;
		
		DebugLog log;

		string messageLocSave1 = "";
		string messageLocSave2 = "";
		string messageLocSave3 = "";
		string messageLocSave4 = "";
		string messageLocSave5 = "";
		string messageLocSave6 = "";
        private Button btnLocationTree;
		string messageLocUpd3 = "";
        private TextBox tbColor;
        private Label lblColor;

        List<LocationTO> actualLocation;

		// Add Form
		public LocationsAdd()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
			
			currentLocation = new LocationTO();
			logInUser = NotificationController.GetLogInUser();

			int locID = new Location().FindMAXLocID();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(LocationsAdd).Assembly);

			setLanguage();
			this.cbPrentLocation.DropDownStyle = ComboBoxStyle.DropDownList;

			locID++;
			tbLocationID.Text = locID.ToString().Trim();
			populateParentLocationCb();
			btnUpdate.Visible = false;
		}

		// Update Form
		public LocationsAdd(LocationTO loc)
		{
			InitializeComponent();
			currentLocation = new LocationTO(loc);

			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);

			setLanguage();
			this.cbPrentLocation.DropDownStyle = ComboBoxStyle.DropDownList;

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			populateParentLocationCb();
			populateUpdateForm(loc);
			btnSave.Visible = false;
			lblStar1.Visible = false;
			tbLocationID.Enabled = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationsAdd));
            this.tbAddressID = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbLocationID = new System.Windows.Forms.TextBox();
            this.lblAddressID = new System.Windows.Forms.Label();
            this.cbPrentLocation = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddress = new System.Windows.Forms.Button();
            this.lblStar1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblParentLocationID = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblLocationID = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.tbColor = new System.Windows.Forms.TextBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbAddressID
            // 
            this.tbAddressID.Location = new System.Drawing.Point(128, 162);
            this.tbAddressID.Name = "tbAddressID";
            this.tbAddressID.ReadOnly = true;
            this.tbAddressID.Size = new System.Drawing.Size(56, 20);
            this.tbAddressID.TabIndex = 11;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(128, 98);
            this.tbDescription.MaxLength = 50;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(264, 20);
            this.tbDescription.TabIndex = 6;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(128, 66);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(264, 20);
            this.tbName.TabIndex = 4;
            // 
            // tbLocationID
            // 
            this.tbLocationID.Location = new System.Drawing.Point(128, 32);
            this.tbLocationID.MaxLength = 10;
            this.tbLocationID.Name = "tbLocationID";
            this.tbLocationID.Size = new System.Drawing.Size(96, 20);
            this.tbLocationID.TabIndex = 1;
            // 
            // lblAddressID
            // 
            this.lblAddressID.Location = new System.Drawing.Point(16, 162);
            this.lblAddressID.Name = "lblAddressID";
            this.lblAddressID.Size = new System.Drawing.Size(100, 23);
            this.lblAddressID.TabIndex = 10;
            this.lblAddressID.Text = "Address ID:";
            this.lblAddressID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPrentLocation
            // 
            this.cbPrentLocation.Location = new System.Drawing.Point(128, 130);
            this.cbPrentLocation.Name = "cbPrentLocation";
            this.cbPrentLocation.Size = new System.Drawing.Size(264, 21);
            this.cbPrentLocation.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(320, 245);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddress
            // 
            this.btnAddress.Location = new System.Drawing.Point(288, 162);
            this.btnAddress.Name = "btnAddress";
            this.btnAddress.Size = new System.Drawing.Size(104, 23);
            this.btnAddress.TabIndex = 12;
            this.btnAddress.Text = "Address >>>";
            this.btnAddress.Click += new System.EventHandler(this.btnAddress_Click);
            // 
            // lblStar1
            // 
            this.lblStar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar1.ForeColor = System.Drawing.Color.Red;
            this.lblStar1.Location = new System.Drawing.Point(232, 34);
            this.lblStar1.Name = "lblStar1";
            this.lblStar1.Size = new System.Drawing.Size(16, 16);
            this.lblStar1.TabIndex = 2;
            this.lblStar1.Text = "*";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(32, 245);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblParentLocationID
            // 
            this.lblParentLocationID.Location = new System.Drawing.Point(8, 130);
            this.lblParentLocationID.Name = "lblParentLocationID";
            this.lblParentLocationID.Size = new System.Drawing.Size(104, 23);
            this.lblParentLocationID.TabIndex = 7;
            this.lblParentLocationID.Text = "Parent Location::";
            this.lblParentLocationID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(32, 98);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(80, 23);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(40, 66);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(72, 23);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLocationID
            // 
            this.lblLocationID.Location = new System.Drawing.Point(40, 34);
            this.lblLocationID.Name = "lblLocationID";
            this.lblLocationID.Size = new System.Drawing.Size(80, 23);
            this.lblLocationID.TabIndex = 0;
            this.lblLocationID.Text = "Location ID:";
            this.lblLocationID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(32, 245);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(398, 127);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 9;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // tbColor
            // 
            this.tbColor.Location = new System.Drawing.Point(128, 195);
            this.tbColor.MaxLength = 50;
            this.tbColor.Name = "tbColor";
            this.tbColor.Size = new System.Drawing.Size(264, 20);
            this.tbColor.TabIndex = 14;
            // 
            // lblColor
            // 
            this.lblColor.Location = new System.Drawing.Point(11, 195);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(101, 23);
            this.lblColor.TabIndex = 13;
            this.lblColor.Text = "Color:";
            this.lblColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LocationsAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(434, 288);
            this.ControlBox = false;
            this.Controls.Add(this.tbColor);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.btnLocationTree);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.tbAddressID);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbLocationID);
            this.Controls.Add(this.lblAddressID);
            this.Controls.Add(this.cbPrentLocation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddress);
            this.Controls.Add(this.lblStar1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblParentLocationID);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblLocationID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "LocationsAdd";
            this.ShowInTaskbar = false;
            this.Text = "Locations";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LocationsAdd_KeyUp);
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
				if (currentLocation.LocationID >= 0)
				{
					this.Text = rm.GetString("updateLocation", culture);
				}
				else
				{
					this.Text = rm.GetString("addLocation", culture);
				}

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnAddress.Text = rm.GetString("btnAddress", culture);

				// label's text
				lblLocationID.Text = rm.GetString("lblLocationID", culture);
				lblName.Text = rm.GetString("lblName", culture);
				lblDescription.Text = rm.GetString("lblDescription", culture);
				lblParentLocationID.Text = rm.GetString("lblParentLocationID", culture);
				lblAddressID.Text = rm.GetString("lblAddressID", culture);
                lblColor.Text = rm.GetString("lblSegmentColor", culture); 

				// message's text
				messageLocSave1 = rm.GetString("messageLocSave1", culture);
				messageLocSave2 = rm.GetString("messageLocSave2", culture);
				messageLocSave3 = rm.GetString("messageLocSave3", culture);
				messageLocSave4 = rm.GetString("messageLocSave4", culture);
				messageLocSave5 = rm.GetString("messageLocSave5", culture);
				messageLocSave6 = rm.GetString("messageLocSave6", culture);
				messageLocUpd3 = rm.GetString("messageLocUpd3", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " LocationsAdd.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " LocationsAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Save new location
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
			try
            {
                if (validate())
                {
                    Location location = new Location();
                    location.LocTO = currentLocation;

                    int inserted = location.Save();

                    if (inserted == 1)
                    {
                        MessageBox.Show(messageLocSave5);
                        this.Close();
                    }
                }
			}
			catch(SqlException sqlex)
			{
				if(sqlex.Number == 2627)
				{
					log.writeLog(DateTime.Now + " Locations.btnSave_Click(): " + messageLocSave6 + "\n");
					MessageBox.Show(messageLocSave6);
				}
				else
				{
					log.writeLog(DateTime.Now + " Locations.btnSave_Click(): " + sqlex.Message + "\n");
					MessageBox.Show(sqlex.Message);
				}
			}
			catch(MySqlException mysqlex)
			{
				if(mysqlex.Number == 1062)
				{
					log.writeLog(DateTime.Now + " Locations.btnSave_Click(): " + messageLocSave6 + "\n");
					MessageBox.Show(messageLocSave6);
				}
				else
				{
					log.writeLog(DateTime.Now + " Locations.btnSave_Click(): " + mysqlex.Message + "\n");
					MessageBox.Show(mysqlex.Message);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Locations.btnSave_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Populate combo box containing posible parent locations
		/// </summary>
		private void populateParentLocationCb()
		{
			try
			{
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				List<LocationTO> parentLocArray = loc.Search();
				actualLocation = new List<LocationTO>();
                ArrayList locIDs = new ArrayList();
                if (currentLocation.LocationID != -1)
                {
                    List<LocationTO> locations = new List<LocationTO>();
                    locations.Add(currentLocation);
                    locations = new Location().FindAllChildren(locations);
                    
                    if (locations.Count > 0)
                    {
                        foreach (LocationTO locTO in locations)
                        {
                            if(locTO.LocationID != currentLocation.LocationID)
                            locIDs.Add(locTO.LocationID);
                        }
                    }
                }

				actualLocation.Insert(0, new LocationTO(-1, rm.GetString("all", culture), "", -1, -1, Constants.DefaultStateActive.ToString()));

				foreach(LocationTO locMember in parentLocArray)
				{
                    if (!locIDs.Contains(locMember.LocationID))
                    {
                        if (currentLocation.LocationID == 0)
                        {
                            if (locMember.ParentLocationID != currentLocation.LocationID)
                            {
                                actualLocation.Add(locMember);
                            }
                        }
                        else
                        {
                            actualLocation.Add(locMember);
                        }
                    }
				}

				cbPrentLocation.DataSource = actualLocation;
				cbPrentLocation.DisplayMember = "Name";
				cbPrentLocation.ValueMember = "LocationID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Locations.populateParentLocationCb(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void btnAddress_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int addressID = tbAddressID.Text.Equals("") ? 0 : Int32.Parse(tbAddressID.Text.Trim());
                Addresses addrForm = new Addresses(addressID, tbName.Text);
                addrForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnAddress_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Set text of Address ID field
		/// </summary>
		/// <param name="addressID"></param>
		public void setAddressID(int addressID)
		{
			this.tbAddressID.Text = (addressID < 0) ? "0" : addressID.ToString();
		}

		/// <summary>
		/// Populate form with data of location to update
		/// </summary>
		/// <param name="loc"></param>
		public void populateUpdateForm(LocationTO loc)
		{
			this.tbLocationID.Text = (loc.LocationID < 0) ? "" : loc.LocationID.ToString();
			this.tbName.Text = loc.Name.ToString();
			this.tbDescription.Text = loc.Description.ToString();
			this.cbPrentLocation.SelectedValue = (loc.ParentLocationID < 0) ? 0 : loc.ParentLocationID;
			this.tbAddressID.Text = (loc.AddressID < 0) ? "0" : loc.AddressID.ToString();
            this.tbColor.Text = loc.SegmentColor.Trim();
		}

		/// <summary>
		/// Update selected location
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (validate())
                {
                    Location location = new Location();
                    location.LocTO = currentLocation;

                    if (location.Update())
                    {
                        MessageBox.Show(messageLocUpd3);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void LocationsAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " LocationsAdd.LocationsAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnLocationTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LocationsTreeView locationsTreeView = new LocationsTreeView(actualLocation);
                locationsTreeView.ShowDialog();
                if (!locationsTreeView.selectedLocation.Equals(""))
                {
                    this.cbPrentLocation.SelectedIndex = cbPrentLocation.FindStringExact(locationsTreeView.selectedLocation);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LocationsAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool validate()
        {
            try
            {
                if (this.tbLocationID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(messageLocSave1);
                    tbLocationID.Focus();
                    return false;
                }

                int locID = -1;
                if (!Int32.TryParse(tbLocationID.Text.Trim(), out locID))
                    locID = -1;
                
                if (locID == -1)
                {
                    MessageBox.Show(messageLocSave2);
                    tbLocationID.Focus();
                    return false;
                }

                int addressID = 0;
                if (tbAddressID.Text.Trim() != "" && !Int32.TryParse(tbAddressID.Text.Trim(), out addressID))
                    addressID = -1;

                if (addressID == -1)
                {
                    MessageBox.Show(messageLocSave3);
                    tbAddressID.Focus();
                    return false;
                }                

                currentLocation.LocationID = locID;
                currentLocation.Name = tbName.Text.Trim();
                currentLocation.Description = tbDescription.Text.Trim();
                if (cbPrentLocation.SelectedIndex > 0)
                    currentLocation.ParentLocationID = (int)cbPrentLocation.SelectedValue;
                else
                    currentLocation.ParentLocationID = currentLocation.LocationID;
                currentLocation.AddressID = addressID;
                currentLocation.Status = Constants.statusActive.Trim();
                currentLocation.SegmentColor = tbColor.Text.Trim();

                return true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LocationsAdd.validate(): " + ex.Message + "\n");
                throw ex;
            }
        }
	}
}
