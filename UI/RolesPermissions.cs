using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for RolesPermissions.
	/// </summary>
	public class RolesPermissions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Label lblLegend;
		private System.Windows.Forms.Label lblRE;
        private System.Windows.Forms.Label lblD;
        private IContainer components;

        // Controller instance
        public NotificationController Controller;

        // Observer client instance
        public NotificationObserverClient observerClient;

		ApplMenuItemTO currentMenuItem = null;

		// Array of all role permissions for Menu Item
		int[] itemPermissions = null;

		// Key is Menu Item ID, value is its permissions
		Hashtable permissions = null;

		// Array of all cells for Menu Item
		ArrayList permissionCells = null;
		// Key is Menu Item ID, value is its cells
		Hashtable menuItemsRow = null;

		DebugLog log;
		ApplUserTO logInUser;

		private CultureInfo culture;
		private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.Label lblU;
		ResourceManager rm;


		public RolesPermissions(List<ApplRoleTO> roleList, List<ApplMenuItemTO> menuItemList)
		{
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                itemPermissions = new int[40];
                permissions = new Hashtable();
                permissionCells = new ArrayList();
                menuItemsRow = new Hashtable();

                currentMenuItem = new ApplMenuItemTO();

                // Get all Menu Items
                //menuItems = currentMenuItem.Search("", "", "", new string[0]);

                // Convert Permissions of all Menu Items to Arrays
                //populatePermitionsArray(menuItems);

                // remove all the menu items where SYS has no privileges
                menuItemList = purgeMenuItems(menuItemList);

                populatePermitionsArray(menuItemList);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(RolesPermissions).Assembly);
                setLanguage();
                populateHeader(roleList);
                populateRolePermissionsScreen(roleList, menuItemList);

                InitObserverClient();
            }
            catch (Exception ex)
            {
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.lblLegend = new System.Windows.Forms.Label();
            this.lblRE = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.lblU = new System.Windows.Forms.Label();
            this.lblD = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 632);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(904, 632);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Location = new System.Drawing.Point(16, 128);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(960, 488);
            this.panel.TabIndex = 6;
            // 
            // lblLegend
            // 
            this.lblLegend.Location = new System.Drawing.Point(16, 8);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(100, 16);
            this.lblLegend.TabIndex = 0;
            this.lblLegend.Text = "Legend:";
            // 
            // lblRE
            // 
            this.lblRE.Location = new System.Drawing.Point(16, 32);
            this.lblRE.Name = "lblRE";
            this.lblRE.Size = new System.Drawing.Size(616, 16);
            this.lblRE.TabIndex = 1;
            this.lblRE.Text = "R/E - Read/Execute Permission";
            // 
            // lblA
            // 
            this.lblA.Location = new System.Drawing.Point(16, 56);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(616, 16);
            this.lblA.TabIndex = 2;
            this.lblA.Text = "A - Add Permision";
            // 
            // lblU
            // 
            this.lblU.Location = new System.Drawing.Point(16, 80);
            this.lblU.Name = "lblU";
            this.lblU.Size = new System.Drawing.Size(616, 16);
            this.lblU.TabIndex = 3;
            this.lblU.Text = "U - Update Permission";
            // 
            // lblD
            // 
            this.lblD.Location = new System.Drawing.Point(16, 104);
            this.lblD.Name = "lblD";
            this.lblD.Size = new System.Drawing.Size(616, 16);
            this.lblD.TabIndex = 4;
            this.lblD.Text = "D - Delete";
            // 
            // RolesPermissions
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(984, 657);
            this.ControlBox = false;
            this.Controls.Add(this.lblD);
            this.Controls.Add(this.lblU);
            this.Controls.Add(this.lblA);
            this.Controls.Add(this.lblRE);
            this.Controls.Add(this.lblLegend);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 696);
            this.MinimumSize = new System.Drawing.Size(1000, 696);
            this.Name = "RolesPermissions";
            this.ShowInTaskbar = false;
            this.Text = "RolesPermitions";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RolesPermissions_KeyUp);
            this.ResumeLayout(false);

		}
		#endregion

        /// <summary>
        /// Init Controller and Observer Client
        /// </summary>
        private void InitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
            Controller.AttachToNotifier(observerClient);
            this.observerClient.Notification += new NotificationEventHandler(this.CellBoxCheckedChanged);
        }

		/// <summary>
		/// Set proper language and initialize List View
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("rolePermForm", culture);

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label view
				lblLegend.Text = rm.GetString("lblLegend", culture);
				lblRE.Text = rm.GetString("lblRE", culture);
				lblA.Text = rm.GetString("lblA", culture);
				lblU.Text = rm.GetString("lblU", culture);
				lblD.Text = rm.GetString("lblD", culture);
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}

		}

		private void populateHeader(List<ApplRoleTO> roleList)
		{
            try
			{
			Label hdrLabel = new Label();

			for (int i = 0; i < roleList.Count; i++)
			{
				// Draw Role Name
				hdrLabel = new Label();
				this.panel.Controls.Add(hdrLabel);
				hdrLabel.Text = roleList[i].Name;
				hdrLabel.Size = new Size(100, 25);
				hdrLabel.TextAlign = ContentAlignment.MiddleCenter;
				hdrLabel.Location = new Point(100 * (i + 1), 0);

				// Draw Legend
				hdrLabel = new Label();
				this.panel.Controls.Add(hdrLabel);
				hdrLabel.Text = rm.GetString("legendText", culture);
				hdrLabel.Size = new Size(100, 20);
				hdrLabel.TextAlign = ContentAlignment.MiddleLeft;
				hdrLabel.Location = new Point(100 * (i + 1), 25);
            }
        }
        catch (Exception ex)
        {
            log.writeLog(DateTime.Now + " RolesPermitions.populateHeader(): " + ex.Message + "\n");
            MessageBox.Show(ex.Message);
        }
		}

		private void populateRolePermissionsScreen(List<ApplRoleTO> roleList, List<ApplMenuItemTO> menuItemList)
		{
			try
			{	
				Label menuItemLabel = new Label();
				int cellPermission;

				for (int i = 0; i < menuItemList.Count; i++)
				{
					// Draw Menu Item label
					menuItemLabel = new Label();
					this.panel.Controls.Add(menuItemLabel);
                    if (!menuItemList[i].Position.Contains("_"))
                    {
                        Font font = new Font("Microsoft Sans Serif", (float)8.25, FontStyle.Bold);
                        menuItemLabel.Font = font;
                    }
					menuItemLabel.Text = menuItemList[i].Name;
					menuItemLabel.Size = new Size(100, 26);
					menuItemLabel.TextAlign = ContentAlignment.MiddleLeft;
					menuItemLabel.Location = new Point(0, 26 * i + 50);
                    menuItemLabel.MouseHover += new System.EventHandler(this.labelMouse_Hover);


					permissionCells = new ArrayList();
					// Draw Permission cells
					for (int j = 0; j < roleList.Count; j++)
					{
						RolePermissionCell cell = new RolePermissionCell(100 * (j + 1), 26 * i + 50);
						cell.RoleID = roleList[j].ApplRoleID;
						cell.MenuItemID = menuItemList[i].ApplMenuItemID;
                        cell.Position = menuItemList[i].Position;
						cellPermission = ((int[]) permissions[cell.MenuItemID])[cell.RoleID];
						cell.setPermissions(cellPermission);
						cell.setCheckBoxes();
						permissionCells.Add(cell);
						this.panel.Controls.Add(cell);
					}

					menuItemsRow.Add(menuItemList[i].ApplMenuItemID, permissionCells);					
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " RolesPermitions.populateRolePermitionsScreen(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populatePermitionsArray(List<ApplMenuItemTO> menuItems)
		{
			try
			{
				foreach (ApplMenuItemTO menuItem in menuItems)
				{
					itemPermissions = menuItem.PermissionsToArray();

					// Key is Menu Item ID, value is array of Permissions for that Menu Item
					permissions.Add(menuItem.ApplMenuItemID, itemPermissions);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " RolesPermitions.populatePermitionsArray(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private List<ApplMenuItemTO> purgeMenuItems(List<ApplMenuItemTO> menuItems)
		{
			try
			{
                List<ApplMenuItemTO> menuItemsTmp = new List<ApplMenuItemTO>(menuItems.Count);
				for (int i = 0; i < menuItems.Count; i++)
				{
					itemPermissions = menuItems[i].PermissionsToArray();
					if (itemPermissions[0] != 0) menuItemsTmp.Add(menuItems[i]);
				}

				return menuItemsTmp;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " RolesPermitions.purgeMenuItems(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " RolesPermitions.btnCancel_Click(): " + ex.Message + "\n");
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

                bool update = true;
                bool adminUpd = false;
                bool noAdminRoleUpd = false;

                foreach (string menuItemID in menuItemsRow.Keys)
                {
                    currentMenuItem = new ApplMenuItem().Find(menuItemID);
                    int[] menuItemPermissions = currentMenuItem.PermissionsToArray();
                    foreach (RolePermissionCell cell in ((ArrayList)menuItemsRow[menuItemID]))
                    {
                        cell.getCheckBoxes();

                        if (cell.RoleID > 0)
                        {
                            noAdminRoleUpd = true;
                            menuItemPermissions[cell.RoleID] = cell.getPermission();
                        }
                        else
                        {
                            if (cell.getPermission() != 15)
                            {
                                adminUpd = true;
                            }
                        }
                    }

                    currentMenuItem.ArrayToPermissions(menuItemPermissions);
                    update = new ApplMenuItem().Update(currentMenuItem)
                        && new ApplMenuItem().UpdateSamePosition(currentMenuItem) && update;

                }
                if (adminUpd)
                {
                    MessageBox.Show(rm.GetString("adminRoleNoUpd", culture));
                }
                if (update && noAdminRoleUpd)
                {
                    MessageBox.Show(rm.GetString("permissionsSaved", culture));
                    this.Close();
                }
                else if (!update)
                {
                    MessageBox.Show(rm.GetString("permissionsNotSaved", culture));
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RolesPermitions.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void labelMouse_Hover(object sender, System.EventArgs e)
        {
            try
            {
              

                Label lbl = (Label)sender;
                ToolTip toolTip = new ToolTip();
                toolTip.Show(lbl.Text, this, this.panel.Location.X + lbl.Location.X + lbl.Width / 2,
                    this.panel.Location.Y + lbl.Location.Y + lbl.Height + 25, Constants.toolTipDuration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void RolesPermissions_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " RolesPermissions.RolesPermissions_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Method that implements selecting of event which will be rised.
        /// </summary>
        public void CellBoxCheckedChanged(object sender, NotificationEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (e.roleID >= 0 && !e.position.Equals("") && !e.cellBoxChecked.Equals(""))
                {
                    int read = 1;

                    foreach (string menuItemID in menuItemsRow.Keys)
                    {
                        foreach (RolePermissionCell cell in ((ArrayList)menuItemsRow[menuItemID]))
                        {
                            if (e.roleID == cell.RoleID && e.position.StartsWith(cell.Position))
                            {
                                cell.getCheckBoxes();

                                cell.ReadExePerm = cell.ReadExePerm > 0 ? cell.ReadExePerm : read;

                                cell.setCheckBoxes();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RolesPermissions.CellBoxCheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

     

       
	}
}
