using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for ApplRoles.
	/// </summary>
	public class ApplRoles : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvRoles;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnUsersRoles;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ApplRoleTO currentRole = null;

		DebugLog log;

		// List View indexes
		const int NameIndex = 0;
		const int DescriptionIndex = 1;

		private ListViewItemComparer _comp;

		private CultureInfo culture;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnRolePermitions;
		ResourceManager rm;

		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		public ApplRoles()
		{
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentRole = new ApplRoleTO();

                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(ApplRoles).Assembly);
                setLanguage();
                populateListView();
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
            this.lvRoles = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRolePermitions = new System.Windows.Forms.Button();
            this.btnUsersRoles = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvRoles
            // 
            this.lvRoles.FullRowSelect = true;
            this.lvRoles.GridLines = true;
            this.lvRoles.HideSelection = false;
            this.lvRoles.Location = new System.Drawing.Point(16, 16);
            this.lvRoles.Name = "lvRoles";
            this.lvRoles.Size = new System.Drawing.Size(584, 328);
            this.lvRoles.TabIndex = 0;
            this.lvRoles.UseCompatibleStateImageBehavior = false;
            this.lvRoles.View = System.Windows.Forms.View.Details;
            this.lvRoles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvRoles_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 360);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(104, 360);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRolePermitions
            // 
            this.btnRolePermitions.Location = new System.Drawing.Point(224, 360);
            this.btnRolePermitions.Name = "btnRolePermitions";
            this.btnRolePermitions.Size = new System.Drawing.Size(112, 23);
            this.btnRolePermitions.TabIndex = 3;
            this.btnRolePermitions.Text = "Role Permitions";
            this.btnRolePermitions.Click += new System.EventHandler(this.btnRolePermitions_Click);
            // 
            // btnUsersRoles
            // 
            this.btnUsersRoles.Location = new System.Drawing.Point(352, 360);
            this.btnUsersRoles.Name = "btnUsersRoles";
            this.btnUsersRoles.Size = new System.Drawing.Size(128, 23);
            this.btnUsersRoles.TabIndex = 4;
            this.btnUsersRoles.Text = "User <-> Role";
            this.btnUsersRoles.Click += new System.EventHandler(this.btnUsersRoles_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(528, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ApplRoles
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(616, 390);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUsersRoles);
            this.Controls.Add(this.btnRolePermitions);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvRoles);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(624, 424);
            this.MinimumSize = new System.Drawing.Size(624, 424);
            this.Name = "ApplRoles";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "ApplRoles";
            this.Load += new System.EventHandler(this.ApplRoles_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ApplRoles_KeyUp);
            this.ResumeLayout(false);

		}
		#endregion

		#region Inner Class for sorting items in View List

		/*
		 *  Class used for sorting items in the List View 
		*/
		private class ListViewItemComparer : IComparer
		{
			private ListView _listView;

			public ListViewItemComparer(ListView lv)
			{
				_listView = lv;
			}
			public ListView ListView
			{
				get{return _listView;}
			}

			private int _sortColumn = 0;

			public int SortColumn
			{
				get { return _sortColumn; }
				set { _sortColumn = value; }
			}

			public int Compare(object a, object b)
			{
				ListViewItem item1 = (ListViewItem) a;
				ListViewItem item2 = (ListViewItem) b;

				if (ListView.Sorting == SortOrder.Descending)
				{
					ListViewItem temp = item1;
					item1 = item2;
					item2 = temp;
				}
				// Handle non Detail Cases
				return CompareItems(item1, item2);
			}

			public int CompareItems(ListViewItem item1, ListViewItem item2)
			{
				// Subitem instances
				ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
				ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

				// Return value based on sort column	
				switch (SortColumn)
				{
					case ApplRoles.NameIndex:
					case ApplRoles.DescriptionIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					default:
						throw new IndexOutOfRangeException("Unrecognized column name extension");
				}
			}
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
				this.Text = rm.GetString("roleForm", culture);

				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
				btnUsersRoles.Text = rm.GetString("btnUserRole", culture);
				btnRolePermitions.Text = rm.GetString("btnRolePermitions", culture);

				// list view
				lvRoles.BeginUpdate();
				lvRoles.Columns.Add(rm.GetString("lblName", culture), (4 * (lvRoles.Width - 10)) / 10, HorizontalAlignment.Left);
				lvRoles.Columns.Add(rm.GetString("lblDescription", culture), (6 * (lvRoles.Width - 10)) / 10, HorizontalAlignment.Left);
				lvRoles.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}

		}

		private void populateListView()
		{
			try
			{
				List<ApplRoleTO> rolesList = new ApplRole().SearchUserCreatedRoles();
				
				lvRoles.BeginUpdate();
				lvRoles.Items.Clear();

				if (rolesList.Count > 0)
				{
					foreach(ApplRoleTO role in rolesList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = role.Name.Trim();
						item.SubItems.Add(role.Description.Trim());
						item.Tag = role;
						
						lvRoles.Items.Add(item);
					}
				}

				lvRoles.EndUpdate();
				lvRoles.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " ApplRoles.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void ApplRoles_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvRoles);
                lvRoles.ListViewItemSorter = _comp;
                lvRoles.Sorting = SortOrder.Ascending;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplRoles.ApplRoles_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvRoles_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				SortOrder prevOrder = lvRoles.Sorting;
				lvRoles.Sorting = SortOrder.None;

				if (e.Column == _comp.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvRoles.Sorting = SortOrder.Descending;
					}
					else
					{
						lvRoles.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp.SortColumn = e.Column;
					lvRoles.Sorting = SortOrder.Ascending;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.lvRoles_ColumnClick(): " + ex.Message + "\n");
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

				if (lvRoles.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("noSelRoleDel", culture));
				}
				else
				{
					DialogResult result = MessageBox.Show(rm.GetString("deleteRoles", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;
						int selected = lvRoles.SelectedItems.Count;
						foreach(ListViewItem item in lvRoles.SelectedItems)
						{
							if (((ApplRoleTO)item.Tag).ApplRoleID == 0)
							{
								MessageBox.Show(rm.GetString("defaultRole", culture));
								selected--;
							}
							else
							{
								// Find if exists Users that this Role is granted to
                                ApplUsersXRole auXRole = new ApplUsersXRole();
                                auXRole.AuXRoleTO.RoleID = ((ApplRoleTO)item.Tag).ApplRoleID;
								List<ApplUsersXRoleTO> userArray = auXRole.Search();
						
								if (userArray.Count > 0)
								{
									MessageBox.Show(item.Text + ": " + rm.GetString("roleHasUsers", culture));
									selected--;
								}
								else
								{
									isDeleted = new ApplRole().UpdateOnEmptyRole(((ApplRoleTO)item.Tag).ApplRoleID) && isDeleted;
								}
							}
						}

						if ((selected > 0) && isDeleted)
						{
							MessageBox.Show(rm.GetString("rolesDel", culture));
						}
						else if (!isDeleted)
						{
							MessageBox.Show(rm.GetString("noRoleDel", culture));
						}

						populateListView();
						currentRole = new ApplRoleTO();
						this.Invalidate();
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.btnDelete_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				int applRoleID = new ApplRole().FindEmptyRole();
				if (applRoleID == 0)
				{
					MessageBox.Show(rm.GetString("allRolesUsed", culture));
				}
				else
				{
					ApplRolesAdd addForm = new ApplRolesAdd(applRoleID);
					addForm.ShowDialog(this);

					populateListView();
					currentRole = new ApplRoleTO();
					this.Invalidate();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.btnAdd_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUsersRoles_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				ApplUsersXRoles userXRoleForm = new ApplUsersXRoles();
				userXRoleForm.ShowDialog(this);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.btnUsersRoles_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnRolePermitions_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				if (lvRoles.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("selRolePerm", culture));
				}
				else
				{					
					List<ApplRoleTO> roleList = new List<ApplRoleTO>();
                    ApplMenuItem mItem = new ApplMenuItem();
                    mItem.MenuItemTO.LangCode = NotificationController.GetLanguage();
					List<ApplMenuItemTO> menuItemList = mItem.Search();

					foreach (ListViewItem item in lvRoles.SelectedItems)
					{					
						roleList.Add((ApplRoleTO)item.Tag);
					}
					
					RolesPermissions rolePermForm = new RolesPermissions(roleList, menuItemList);
					rolePermForm.ShowDialog(this);
					
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.btnRolePermitions_Click(): " + ex.Message + "\n");
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
                int privilegesPermission;
                bool privilegesReadPermission = false;

				foreach (ApplRoleTO role in currentRoles)
				{
					permission = (((int[]) menuItemsPermissions[menuItemID])[role.ApplRoleID]);
					readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
					addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
					updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
					deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);

                    string privilegesID = menuItemID.Substring(0, menuItemID.LastIndexOf('_') + 1);

                    if (logInUser.LangCode.Equals(Constants.Lang_sr))
                    {
                        privilegesID += Constants.PrivilegesSR;
                    }
                    else if (logInUser.LangCode.Equals(Constants.Lang_en))
                    {
                        privilegesID += Constants.PrivilegesEN;
                    }
                    else if (logInUser.LangCode.Equals(Constants.Lang_fi))
                    {
                        privilegesID += Constants.PrivilegesFI;
                    }

                    privilegesPermission = (((int[])menuItemsPermissions[privilegesID])[role.ApplRoleID]);
                    privilegesReadPermission = privilegesReadPermission || (((privilegesPermission / 8) % 2) == 0 ? false : true);
				}

				btnAdd.Enabled = addPermission;
				btnDelete.Enabled = deletePermission;
                btnRolePermitions.Enabled = privilegesReadPermission;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRoles.setVisibility(): " + ex.Message + "\n");
				throw ex;
			}
		}

        private void ApplRoles_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ApplRoles.ApplRoles_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
