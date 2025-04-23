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
	/// Summary description for EmployeeAccessGroups.
	/// </summary>
	public class EmployeeAccessGroups : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvAccessGroups;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnEmployeeAccessGroups;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		EmployeeGroupAccessControl currentEmployeeGroupAccessControl = null;

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
		bool deletePermission = false;		

		private ListViewItemComparer _comp;

		// List View indexes
		const int NameIndex = 0;
		const int DescriptionIndex = 1;		

		public EmployeeAccessGroups()
		{
			InitializeComponent();
			setProperties();
		}

		public EmployeeAccessGroups(bool hideButton)
		{
			InitializeComponent();
			setProperties();
			if (hideButton)
				btnEmployeeAccessGroups.Visible = false;
		}

		public void setProperties()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentEmployeeGroupAccessControl = new EmployeeGroupAccessControl();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(EmployeeAccessGroups).Assembly);
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
            this.lvAccessGroups = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEmployeeAccessGroups = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvAccessGroups
            // 
            this.lvAccessGroups.FullRowSelect = true;
            this.lvAccessGroups.GridLines = true;
            this.lvAccessGroups.HideSelection = false;
            this.lvAccessGroups.Location = new System.Drawing.Point(16, 16);
            this.lvAccessGroups.Name = "lvAccessGroups";
            this.lvAccessGroups.Size = new System.Drawing.Size(584, 328);
            this.lvAccessGroups.TabIndex = 0;
            this.lvAccessGroups.UseCompatibleStateImageBehavior = false;
            this.lvAccessGroups.View = System.Windows.Forms.View.Details;
            this.lvAccessGroups.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvAccessGroups_ColumnClick);
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
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(104, 360);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(192, 360);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEmployeeAccessGroups
            // 
            this.btnEmployeeAccessGroups.Location = new System.Drawing.Point(312, 360);
            this.btnEmployeeAccessGroups.Name = "btnEmployeeAccessGroups";
            this.btnEmployeeAccessGroups.Size = new System.Drawing.Size(168, 23);
            this.btnEmployeeAccessGroups.TabIndex = 4;
            this.btnEmployeeAccessGroups.Text = "Employee <-> Access groups";
            this.btnEmployeeAccessGroups.Click += new System.EventHandler(this.btnEmployeeAccessGroups_Click);
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
            // EmployeeAccessGroups
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(616, 390);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnEmployeeAccessGroups);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvAccessGroups);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(624, 424);
            this.MinimumSize = new System.Drawing.Size(624, 424);
            this.Name = "EmployeeAccessGroups";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "EmployeeAccessGroups";
            this.Load += new System.EventHandler(this.EmployeeAccessGroups_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeeAccessGroups_KeyUp);
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
					case EmployeeAccessGroups.NameIndex:
					case EmployeeAccessGroups.DescriptionIndex:
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
				this.Text = rm.GetString("accessGroupsForm", culture);

				// button's text
				btnAdd.Text                  = rm.GetString("btnAdd", culture);
				btnUpdate.Text               = rm.GetString("btnUpdate", culture);
				btnDelete.Text               = rm.GetString("btnDelete", culture);
				btnClose.Text                = rm.GetString("btnClose", culture);
				btnEmployeeAccessGroups.Text = rm.GetString("btnEmployeeAccessGroups", culture);				

				// list view
				lvAccessGroups.BeginUpdate();
				lvAccessGroups.Columns.Add(rm.GetString("lblName", culture), (4 * (lvAccessGroups.Width - 4)) / 10, HorizontalAlignment.Left);
				lvAccessGroups.Columns.Add(rm.GetString("lblDescription", culture), (6 * (lvAccessGroups.Width - 4)) / 10, HorizontalAlignment.Left);
				lvAccessGroups.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroups.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with Access Groups found
		/// </summary>
		/// <param name="accessGroupsList"></param>
		private void populateListView()
		{
			try
			{
				ArrayList accessGroupsList = new EmployeeGroupAccessControl().Search("");

				lvAccessGroups.BeginUpdate();
				lvAccessGroups.Items.Clear();

				if (accessGroupsList.Count > 0)
				{
					foreach(EmployeeGroupAccessControl accessGroup in accessGroupsList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = accessGroup.Name.Trim();
						item.SubItems.Add(accessGroup.Description.Trim());
						item.Tag = accessGroup.AccessGroupId;
						
						lvAccessGroups.Items.Add(item);
					}
				}

				lvAccessGroups.EndUpdate();
				lvAccessGroups.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroups.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void EmployeeAccessGroups_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvAccessGroups);
                lvAccessGroups.ListViewItemSorter = _comp;
                lvAccessGroups.Sorting = SortOrder.Ascending;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAccessGroups.EmployeeAccessGroups_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvAccessGroups_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvAccessGroups.Sorting;
				lvAccessGroups.Sorting = SortOrder.None;

				if (e.Column == _comp.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvAccessGroups.Sorting = SortOrder.Descending;
					}
					else
					{
						lvAccessGroups.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp.SortColumn = e.Column;
					lvAccessGroups.Sorting = SortOrder.Ascending;
				}
                lvAccessGroups.ListViewItemSorter = _comp;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroups.lvAccessGroups_ColumnClick(): " + ex.Message + "\n");
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
				// Open Add Form
				EmployeeAccessGroupAdd addForm = new EmployeeAccessGroupAdd(); 
				addForm.ShowDialog(this);

				populateListView();
				currentEmployeeGroupAccessControl.Clear();
				this.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroups.btnAdd_Click(): " + ex.Message + "\n");
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
				if (this.lvAccessGroups.SelectedItems.Count != 1)
				{
					MessageBox.Show(rm.GetString("selOneAccessGroup", culture));
				}
				else 
				{			
					currentEmployeeGroupAccessControl.ReceiveTransferObject(currentEmployeeGroupAccessControl.Find( this.lvAccessGroups.SelectedItems[0].Tag.ToString() ));					

					// Open Update Form
					EmployeeAccessGroupAdd addForm = new EmployeeAccessGroupAdd(currentEmployeeGroupAccessControl);
					addForm.ShowDialog(this);

					populateListView();
					currentEmployeeGroupAccessControl.Clear();
					this.Invalidate();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroups.btnUpdate_Click(): " + ex.Message + "\n");
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

				if (lvAccessGroups.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("noSelAccessGroupDel", culture));
				}
				else
				{
					DialogResult result = MessageBox.Show(rm.GetString("deleteAccessGroups", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;
						int selected = lvAccessGroups.SelectedItems.Count;
						foreach(ListViewItem item in lvAccessGroups.SelectedItems)
						{
							if (((int) item.Tag) == 0)
							{
								MessageBox.Show(item.Text + ": " + rm.GetString("defaultAccessGroup", culture));
								selected--;
							}
							else
							{
								// Find if exists Employee that belong to this Access group
								List<EmployeeTO> employees = new Employee().SearchByAccessGroup(item.Tag.ToString());
								
                                if (employees.Count > 0)
								{
									MessageBox.Show(item.Text + ": " + rm.GetString("accessGroupHasEmployees", culture));
									selected--;
								}
								else
								{
									// Find if exists Gate <-> Access group xref record
									ArrayList xrefArray = (new AccessGroupXGate()).Search(item.Tag.ToString(), "");
									if (xrefArray.Count > 0)
									{
										MessageBox.Show(item.Text + ": " + rm.GetString("accessGroupHasGateXref", culture));
										selected--;
									}
									else
									{
										isDeleted = currentEmployeeGroupAccessControl.Delete(item.Tag.ToString()) && isDeleted;
									}
								}
							}
						}

						if ((selected > 0) && isDeleted)
						{
							MessageBox.Show(rm.GetString("accessGroupDel", culture));
						}
						else if (!isDeleted)
						{
							MessageBox.Show(rm.GetString("noAccessGroupDel", culture));
						}

						populateListView();
						currentEmployeeGroupAccessControl.Clear();						
						this.Invalidate();
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroups.btnDelete_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnEmployeeAccessGroups_Click(object sender, System.EventArgs e)
		{
			try
			{

                this.Cursor = Cursors.WaitCursor;

				EmployeesXAccessGroups employeesXAccessGroupsForm = new EmployeesXAccessGroups(true);
				employeesXAccessGroupsForm.ShowDialog(this);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroups.btnEmployeeAccessGroups_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAccessGroups.btnClose_Click(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " EmployeeAccessGroups.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void EmployeeAccessGroups_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " EmployeeAccessGroups.EmployeeAccessGroups_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }				
	}
}
