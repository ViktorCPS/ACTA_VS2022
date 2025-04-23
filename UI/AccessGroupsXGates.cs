using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for AccessGroupsXGates.
	/// </summary>
	public class AccessGroupsXGates : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvGates;
		private System.Windows.Forms.ListView lvAccessGroups;
		private System.Windows.Forms.ListView lvGateTimeAccessProfiles;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Label lblGates;
		private System.Windows.Forms.Label lblAccessGroups;
		private System.Windows.Forms.Label lblGateTimeAccessProfiles;
		private System.Windows.Forms.Label lblSelected;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		AccessGroupXGate currentAccessGroupXGate = null;

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

		private ListViewItemComparer _comp1;
		private ListViewItemComparer _comp2;
		//private ListViewItemComparer _comp3;

		// List View indexes
		const int NameIndex = 0;		
		const int DescriptionIndex = 1;
		const int IDIndex = 2;

		public AccessGroupsXGates()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentAccessGroupXGate = new AccessGroupXGate();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(AccessGroupsXGates).Assembly);
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
            this.lvGates = new System.Windows.Forms.ListView();
            this.lvAccessGroups = new System.Windows.Forms.ListView();
            this.lvGateTimeAccessProfiles = new System.Windows.Forms.ListView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblGates = new System.Windows.Forms.Label();
            this.lblAccessGroups = new System.Windows.Forms.Label();
            this.lblGateTimeAccessProfiles = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvGates
            // 
            this.lvGates.FullRowSelect = true;
            this.lvGates.GridLines = true;
            this.lvGates.HideSelection = false;
            this.lvGates.Location = new System.Drawing.Point(8, 48);
            this.lvGates.MultiSelect = false;
            this.lvGates.Name = "lvGates";
            this.lvGates.Size = new System.Drawing.Size(248, 248);
            this.lvGates.TabIndex = 2;
            this.lvGates.UseCompatibleStateImageBehavior = false;
            this.lvGates.View = System.Windows.Forms.View.Details;
            this.lvGates.SelectedIndexChanged += new System.EventHandler(this.lvGates_SelectedIndexChanged);
            this.lvGates.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGates_ColumnClick);
            // 
            // lvAccessGroups
            // 
            this.lvAccessGroups.FullRowSelect = true;
            this.lvAccessGroups.GridLines = true;
            this.lvAccessGroups.HideSelection = false;
            this.lvAccessGroups.Location = new System.Drawing.Point(264, 48);
            this.lvAccessGroups.MultiSelect = false;
            this.lvAccessGroups.Name = "lvAccessGroups";
            this.lvAccessGroups.Size = new System.Drawing.Size(248, 248);
            this.lvAccessGroups.TabIndex = 3;
            this.lvAccessGroups.UseCompatibleStateImageBehavior = false;
            this.lvAccessGroups.View = System.Windows.Forms.View.Details;
            this.lvAccessGroups.SelectedIndexChanged += new System.EventHandler(this.lvAccessGroups_SelectedIndexChanged);
            this.lvAccessGroups.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvAccessGroups_ColumnClick);
            // 
            // lvGateTimeAccessProfiles
            // 
            this.lvGateTimeAccessProfiles.FullRowSelect = true;
            this.lvGateTimeAccessProfiles.GridLines = true;
            this.lvGateTimeAccessProfiles.HideSelection = false;
            this.lvGateTimeAccessProfiles.Location = new System.Drawing.Point(520, 48);
            this.lvGateTimeAccessProfiles.MultiSelect = false;
            this.lvGateTimeAccessProfiles.Name = "lvGateTimeAccessProfiles";
            this.lvGateTimeAccessProfiles.Size = new System.Drawing.Size(248, 248);
            this.lvGateTimeAccessProfiles.TabIndex = 4;
            this.lvGateTimeAccessProfiles.UseCompatibleStateImageBehavior = false;
            this.lvGateTimeAccessProfiles.View = System.Windows.Forms.View.Details;
            this.lvGateTimeAccessProfiles.SelectedIndexChanged += new System.EventHandler(this.lvGateTimeAccessProfiles_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(688, 360);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(8, 360);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(104, 360);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblGates
            // 
            this.lblGates.Location = new System.Drawing.Point(8, 16);
            this.lblGates.Name = "lblGates";
            this.lblGates.Size = new System.Drawing.Size(248, 23);
            this.lblGates.TabIndex = 25;
            this.lblGates.Text = "Gates";
            this.lblGates.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAccessGroups
            // 
            this.lblAccessGroups.Location = new System.Drawing.Point(264, 16);
            this.lblAccessGroups.Name = "lblAccessGroups";
            this.lblAccessGroups.Size = new System.Drawing.Size(248, 23);
            this.lblAccessGroups.TabIndex = 26;
            this.lblAccessGroups.Text = "Access groups";
            this.lblAccessGroups.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGateTimeAccessProfiles
            // 
            this.lblGateTimeAccessProfiles.Location = new System.Drawing.Point(520, 16);
            this.lblGateTimeAccessProfiles.Name = "lblGateTimeAccessProfiles";
            this.lblGateTimeAccessProfiles.Size = new System.Drawing.Size(248, 23);
            this.lblGateTimeAccessProfiles.TabIndex = 27;
            this.lblGateTimeAccessProfiles.Text = "Profiles on selected gate";
            this.lblGateTimeAccessProfiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSelected
            // 
            this.lblSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelected.ForeColor = System.Drawing.Color.Red;
            this.lblSelected.Location = new System.Drawing.Point(8, 320);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(760, 23);
            this.lblSelected.TabIndex = 28;
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AccessGroupsXGates
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(777, 386);
            this.ControlBox = false;
            this.Controls.Add(this.lblSelected);
            this.Controls.Add(this.lblGateTimeAccessProfiles);
            this.Controls.Add(this.lblAccessGroups);
            this.Controls.Add(this.lblGates);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lvGateTimeAccessProfiles);
            this.Controls.Add(this.lvAccessGroups);
            this.Controls.Add(this.lvGates);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(785, 420);
            this.MinimumSize = new System.Drawing.Size(785, 420);
            this.Name = "AccessGroupsXGates";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Access groups <-> Gates";
            this.Load += new System.EventHandler(this.AccessGroupsXGates_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AccessGroupsXGates_KeyUp);
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
					case AccessGroupsXGates.NameIndex:
					case AccessGroupsXGates.DescriptionIndex:					
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case AccessGroupsXGates.IDIndex:
					{
						int firstID = -1;
						int secondID = -1;

						if (!sub1.Text.Trim().Equals("")) 
						{
							firstID = Int32.Parse(sub1.Text.Trim());
						}

						if (!sub2.Text.Trim().Equals(""))
						{
							secondID = Int32.Parse(sub2.Text.Trim());
						}
						
						return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
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
				this.Text = rm.GetString("accessGroupsXGatesForm", culture);

				// button's text
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSave.Text   = rm.GetString("btnSave", culture);
				btnCancel.Text = rm.GetString("btnClose", culture);

				// label's text
				lblGates.Text                  = rm.GetString("gateForm", culture);
				lblAccessGroups.Text           = rm.GetString("accessGroupsForm", culture);
				lblGateTimeAccessProfiles.Text = rm.GetString("lblProfilesOnGate", culture);

				// list view
				lvGates.BeginUpdate();
				lvGates.Columns.Add(rm.GetString("lblName", culture), (3 * (lvGates.Width - 6)) / 10, HorizontalAlignment.Left);
				lvGates.Columns.Add(rm.GetString("lblDescription", culture), (5 * (lvGates.Width - 6)) / 10, HorizontalAlignment.Left);
				lvGates.Columns.Add(rm.GetString("lblGateID", culture), (2 * (lvGates.Width - 6)) / 10, HorizontalAlignment.Left);				
				lvGates.EndUpdate();

				lvAccessGroups.BeginUpdate();
				lvAccessGroups.Columns.Add(rm.GetString("lblName", culture), (4 * (lvAccessGroups.Width - 4)) / 10, HorizontalAlignment.Left);
				lvAccessGroups.Columns.Add(rm.GetString("lblDescription", culture), (6 * (lvAccessGroups.Width - 4)) / 10, HorizontalAlignment.Left);
				lvAccessGroups.EndUpdate();

				lvGateTimeAccessProfiles.BeginUpdate();
				lvGateTimeAccessProfiles.Columns.Add(rm.GetString("lblName", culture), (4 * (lvGateTimeAccessProfiles.Width - 4)) / 10, HorizontalAlignment.Left);
				lvGateTimeAccessProfiles.Columns.Add(rm.GetString("lblDescription", culture), (6 * (lvGateTimeAccessProfiles.Width - 4)) / 10, HorizontalAlignment.Left);
				lvGateTimeAccessProfiles.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with Gates found
		/// </summary>
		/// <param name="gatesList"></param>
		private void populateGatesListView()
		{
			try
			{
				List<GateTO> gatesList = new Gate().SearchWithGateTAProfile("");

				lvGates.BeginUpdate();
				lvGates.Items.Clear();

				if (gatesList.Count > 0)
				{
					foreach(GateTO gate in gatesList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = gate.Name.ToString().Trim();
						item.SubItems.Add(gate.Description.ToString().Trim());
						item.SubItems.Add(gate.GateID.ToString().Trim());
						item.Tag = gate.GateTimeaccessProfileID;

						lvGates.Items.Add(item);
					}
					lvGates.Items[0].Selected = true;
				}

				lvGates.EndUpdate();
				lvGates.Invalidate();				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.populateGatesListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with Access groups found
		/// </summary>
		/// <param name="accessGroupsList"></param>
		private void populateAccessGroupsListView()
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
					lvAccessGroups.Items[0].Selected = true;
				}

				lvAccessGroups.EndUpdate();
				lvAccessGroups.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.populateAccessGroupsListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with profiles found for selected gate
		/// </summary>
		/// <param name="profilesList"></param>
		private void populateGateTimeAccessProfilesListView()
		{
			try
			{		
				if (lvGates.SelectedItems.Count > 0)
				{
					TransferObjects.GateTimeAccessProfileTO gtapTO = new TransferObjects.GateTimeAccessProfileTO();
					gtapTO = new GateTimeAccessProfile().Find(lvGates.SelectedItems[0].Tag.ToString());

					if (gtapTO.GateTAProfileId != -1)
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
					
						setSelIndexlvGateTimeAccessProfiles();
					}	
					else
					{
						clearlvGateTimeAccessProfiles();
					}
				}
				else
				{
					clearlvGateTimeAccessProfiles();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.populateGateTimeAccessProfilesListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void AccessGroupsXGates_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp1 = new ListViewItemComparer(lvGates);
                lvGates.ListViewItemSorter = _comp1;
                lvGates.Sorting = SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvAccessGroups);
                lvAccessGroups.ListViewItemSorter = _comp2;
                lvAccessGroups.Sorting = SortOrder.Ascending;

                /*_comp3 = new ListViewItemComparer(lvGateTimeAccessProfiles);
                lvGateTimeAccessProfiles.ListViewItemSorter = _comp3;
                lvGateTimeAccessProfiles.Sorting = SortOrder.Ascending;*/

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                populateGatesListView(); //index change here will call populateGateTimeAccessProfilesListView();
                populateAccessGroupsListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessGroupsXGates.AccessGroupsXGates_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvGates_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				SortOrder prevOrder = lvGates.Sorting;
				lvGates.Sorting = SortOrder.None;

				if (e.Column == _comp1.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvGates.Sorting = SortOrder.Descending;
					}
					else
					{
						lvGates.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp1.SortColumn = e.Column;
					lvGates.Sorting = SortOrder.Ascending;
				}
                lvGates.ListViewItemSorter = _comp1;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.lvGates_ColumnClick(): " + ex.Message + "\n");
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

				if (e.Column == _comp2.SortColumn)
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
					_comp2.SortColumn = e.Column;
					lvAccessGroups.Sorting = SortOrder.Ascending;
				}
                lvAccessGroups.ListViewItemSorter = _comp2;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.lvAccessGroups_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		/*private void lvGateTimeAccessProfiles_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
				SortOrder prevOrder = lvGateTimeAccessProfiles.Sorting;
				lvGateTimeAccessProfiles.Sorting = SortOrder.None;

				if (e.Column == _comp3.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvGateTimeAccessProfiles.Sorting = SortOrder.Descending;
					}
					else
					{
						lvGateTimeAccessProfiles.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp3.SortColumn = e.Column;
					lvGateTimeAccessProfiles.Sorting = SortOrder.Ascending;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.lvGateTimeAccessProfiles_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}*/

		private void lvGates_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (lvGates.SelectedItems.Count > 0)
				{
					populateGateTimeAccessProfilesListView();
					//setlblSelectedText();// this will be called in populateGateTimeAccessProfilesListView(), actually, on Index_changed
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.lvGates_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void lvAccessGroups_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				if (lvAccessGroups.SelectedItems.Count > 0)
				{
					setSelIndexlvGateTimeAccessProfiles();
					setlblSelectedText();// this will be called in setSelIndexlvGateTimeAccessProfiles(), but only if index is changed
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.lvAccessGroups_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvGateTimeAccessProfiles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
               
				if (lvGateTimeAccessProfiles.SelectedItems.Count > 0)
				{
					setlblSelectedText();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.lvGateTimeAccessProfiles_SelectedIndexChanged(): " + ex.Message + "\n");
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

				if (lvGates.SelectedItems.Count <= 0 || lvAccessGroups.SelectedItems.Count <= 0
					|| lvGateTimeAccessProfiles.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("noSelPairGateAcessGroupProfileSave", culture));
				}
				else 
				{		
					TransferObjects.AccessGroupXGateTO accessGroupXGateTO = currentAccessGroupXGate.Find(
						lvAccessGroups.SelectedItems[0].Tag.ToString(), lvGates.SelectedItems[0].SubItems[2].Text);
					if (accessGroupXGateTO.GateTimeAccessProfile != -1)
					{
						//exists

						if (lvGateTimeAccessProfiles.SelectedItems[0].Tag.ToString() == "0")
						{
							//no need for entry, because 0 - always allowed

							bool isDeleted = currentAccessGroupXGate.Delete(lvAccessGroups.SelectedItems[0].Tag.ToString(), 
								lvGates.SelectedItems[0].SubItems[2].Text, true); 
							
							if (isDeleted)
							{
								MessageBox.Show(rm.GetString("pairGateAcessGroupProfileSave", culture));
							}
							else
							{
								MessageBox.Show(rm.GetString("pairGateAcessGroupProfileNotSave", culture));
							}
						}
						else
						{
							bool isUpdated = currentAccessGroupXGate.Update(lvAccessGroups.SelectedItems[0].Tag.ToString(), 
								lvGates.SelectedItems[0].SubItems[2].Text,
								lvGateTimeAccessProfiles.SelectedItems[0].Tag.ToString(), 
								lvGateTimeAccessProfiles.SelectedIndices[0].ToString(), true);

							if (isUpdated)
							{
								MessageBox.Show(rm.GetString("pairGateAcessGroupProfileSave", culture));
							}
							else
							{
								MessageBox.Show(rm.GetString("pairGateAcessGroupProfileNotSave", culture));
							}
						}
					}
					else
					{
						//not exist

						if (lvGateTimeAccessProfiles.SelectedItems[0].Tag.ToString() == "0")
						{
							//no need for entry, because 0 - always allowed
							MessageBox.Show(rm.GetString("pairGateAcessGroupProfileSave", culture));
						}
						else
						{
							int count = currentAccessGroupXGate.Save(lvAccessGroups.SelectedItems[0].Tag.ToString(), 
								lvGates.SelectedItems[0].SubItems[2].Text,
								lvGateTimeAccessProfiles.SelectedItems[0].Tag.ToString(),
								lvGateTimeAccessProfiles.SelectedIndices[0].ToString(), true); 

							if (count > 0)
							{
								MessageBox.Show(rm.GetString("pairGateAcessGroupProfileSave", culture));
							}
							else
							{
								MessageBox.Show(rm.GetString("pairGateAcessGroupProfileNotSave", culture));
							}
						}
					}					
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.btnSave_Click(): " + ex.Message + "\n");
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

				if (lvGates.SelectedItems.Count <= 0 || lvAccessGroups.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("noSelPairGateAcessGroupDel", culture));
				}
				else
				{
					DialogResult result = MessageBox.Show(rm.GetString("deletePairGateAcessGroup", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;

						TransferObjects.AccessGroupXGateTO accessGroupXGateTO = currentAccessGroupXGate.Find(
							lvAccessGroups.SelectedItems[0].Tag.ToString(), lvGates.SelectedItems[0].SubItems[2].Text);
						if (accessGroupXGateTO.GateTimeAccessProfile == -1)
						{
							//not exist, that means 0 - always allowed
							MessageBox.Show(rm.GetString("pairGateAcessGroupDel", culture));
							setDefaultIndexlvGateTimeAccessProfiles();
						}
						else
						{
							isDeleted = currentAccessGroupXGate.Delete(lvAccessGroups.SelectedItems[0].Tag.ToString(), 
								lvGates.SelectedItems[0].SubItems[2].Text, true);

							if (isDeleted)
							{
								MessageBox.Show(rm.GetString("pairGateAcessGroupDel", culture));
								//deleted, that means 0 - always allowed
								setDefaultIndexlvGateTimeAccessProfiles();
							}
							else
							{
								MessageBox.Show(rm.GetString("pairGateAcessGroupNotDel", culture));
							}
					
							this.Invalidate();
						}
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.btnDelete_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
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
            catch(Exception ex) {
                log.writeLog(DateTime.Now + " AccessGroupsXGates.btnCancel_Click(): " + ex.Message + "\n");
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

				btnSave.Enabled    = addPermission || updatePermission;
				btnDelete.Enabled  = deletePermission;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " AccessGroupsXGates.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void setSelIndexlvGateTimeAccessProfiles()
		{
			if (lvGates.SelectedItems.Count > 0 && lvAccessGroups.SelectedItems.Count > 0)
			{
				TransferObjects.AccessGroupXGateTO accessGroupXGateTO = currentAccessGroupXGate.Find(
					lvAccessGroups.SelectedItems[0].Tag.ToString(), lvGates.SelectedItems[0].SubItems[2].Text);
				if (accessGroupXGateTO.GateTimeAccessProfile != -1)
				{
					bool match = false;
					int selectedIndex = accessGroupXGateTO.ReaderAccessGroupOrdNum;
					if (selectedIndex != -1 && lvGateTimeAccessProfiles.Items[selectedIndex].Tag.ToString() == accessGroupXGateTO.GateTimeAccessProfile.ToString())
					{
						lvGateTimeAccessProfiles.Items[selectedIndex].Selected = true;
						match = true;
					}	

					//what if not exists ??????????????????????????
					if (!match)
					{
						setDefaultIndexlvGateTimeAccessProfiles();
						//deselectlvGateTimeAccessProfiles();
					}
				}
				else
				{
					//not exist, that means 0 - always allowed
					setDefaultIndexlvGateTimeAccessProfiles();
				}
			}
			else
			{
				deselectlvGateTimeAccessProfiles();				
			}
		}

		private void setlblSelectedText()
		{
			string gate       = (lvGates.SelectedItems.Count > 0 ? lvGates.SelectedItems[0].Text : "");
			string acessGroup = (lvAccessGroups.SelectedItems.Count > 0 ? lvAccessGroups.SelectedItems[0].Text : "");
			string profile    = (lvGateTimeAccessProfiles.SelectedItems.Count > 0 ? lvGateTimeAccessProfiles.SelectedItems[0].Text : "");

			lblSelected.Text = "( " + gate + ", " + acessGroup + " ) < - > " + profile;
		}	
	
		private void setDefaultIndexlvGateTimeAccessProfiles()
		{
			if (lvGateTimeAccessProfiles.Items.Count > 0)
			{
				bool match = false;
				if (lvGateTimeAccessProfiles.Items[0].Tag.ToString() == "0")
				{
					lvGateTimeAccessProfiles.Items[0].Selected = true;
					match = true;
				}	

				//what if not exists ??????????????????????????
				if (!match)
				{
					deselectlvGateTimeAccessProfiles();
				}
			}
		}

		private void clearlvGateTimeAccessProfiles()
		{
			lvGateTimeAccessProfiles.BeginUpdate();
			lvGateTimeAccessProfiles.Items.Clear();
			lvGateTimeAccessProfiles.EndUpdate();
			lvGateTimeAccessProfiles.Invalidate();			
		}

		private void deselectlvGateTimeAccessProfiles()
		{
			for (int i = 0; i < lvGateTimeAccessProfiles.SelectedItems.Count; i++)
			{
				lvGateTimeAccessProfiles.SelectedItems[i].Selected = false;
			}
		}

        private void AccessGroupsXGates_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " AccessGroupsXGates.AccessGroupsXGates_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
