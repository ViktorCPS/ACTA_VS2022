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
	/// Summary description for TimeAccessProfiles.
	/// </summary>
	public class TimeAccessProfiles : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvtimeAccessProfiles;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		TimeAccessProfile currentTimeAccessProfile = null;

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

		public TimeAccessProfiles()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentTimeAccessProfile = new TimeAccessProfile();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(TimeAccessProfiles).Assembly);
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
            this.lvtimeAccessProfiles = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvtimeAccessProfiles
            // 
            this.lvtimeAccessProfiles.FullRowSelect = true;
            this.lvtimeAccessProfiles.GridLines = true;
            this.lvtimeAccessProfiles.HideSelection = false;
            this.lvtimeAccessProfiles.Location = new System.Drawing.Point(16, 16);
            this.lvtimeAccessProfiles.Name = "lvtimeAccessProfiles";
            this.lvtimeAccessProfiles.Size = new System.Drawing.Size(584, 328);
            this.lvtimeAccessProfiles.TabIndex = 1;
            this.lvtimeAccessProfiles.UseCompatibleStateImageBehavior = false;
            this.lvtimeAccessProfiles.View = System.Windows.Forms.View.Details;
            this.lvtimeAccessProfiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvtimeAccessProfiles_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 360);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(104, 360);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(192, 360);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            // TimeAccessProfiles
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(616, 390);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvtimeAccessProfiles);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(624, 424);
            this.MinimumSize = new System.Drawing.Size(624, 424);
            this.Name = "TimeAccessProfiles";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Weekly access schedules";
            this.Load += new System.EventHandler(this.TimeAccessProfiles_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TimeAccessProfiles_KeyUp);
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
					case TimeAccessProfiles.NameIndex:
					case TimeAccessProfiles.DescriptionIndex:
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
				this.Text = rm.GetString("timeAccessProfilesForm", culture);

				// button's text
				btnAdd.Text                  = rm.GetString("btnAdd", culture);
				btnUpdate.Text               = rm.GetString("btnUpdate", culture);
				btnDelete.Text               = rm.GetString("btnDelete", culture);
				btnClose.Text                = rm.GetString("btnClose", culture);

				// list view
				lvtimeAccessProfiles.BeginUpdate();
				lvtimeAccessProfiles.Columns.Add(rm.GetString("lblName", culture), (4 * (lvtimeAccessProfiles.Width - 4)) / 10, HorizontalAlignment.Left);
				lvtimeAccessProfiles.Columns.Add(rm.GetString("lblDescription", culture), (6 * (lvtimeAccessProfiles.Width - 4)) / 10, HorizontalAlignment.Left);
				lvtimeAccessProfiles.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfiles.setLanguage(): " + ex.Message + "\n");
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
				ArrayList timeAccessProfilesList = new TimeAccessProfile().Search("");

				lvtimeAccessProfiles.BeginUpdate();
				lvtimeAccessProfiles.Items.Clear();

				if (timeAccessProfilesList.Count > 0)
				{
					foreach(TimeAccessProfile timeAccessProfile in timeAccessProfilesList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = timeAccessProfile.Name.Trim();
						item.SubItems.Add(timeAccessProfile.Description.Trim());
						item.Tag = timeAccessProfile.TimeAccessProfileId;
						
						lvtimeAccessProfiles.Items.Add(item);
					}
				}

				lvtimeAccessProfiles.EndUpdate();
				lvtimeAccessProfiles.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfiles.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void TimeAccessProfiles_Load(object sender, System.EventArgs e)
		{
			// Initialize comparer object
			_comp = new ListViewItemComparer(lvtimeAccessProfiles);
			lvtimeAccessProfiles.ListViewItemSorter = _comp;
			lvtimeAccessProfiles.Sorting = SortOrder.Ascending;

			menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
			currentRoles = NotificationController.GetCurrentRoles();
			menuItemID = NotificationController.GetCurrentMenuItemID();

			setVisibility();

			populateListView();
		}

		private void lvtimeAccessProfiles_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvtimeAccessProfiles.Sorting;
                lvtimeAccessProfiles.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvtimeAccessProfiles.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvtimeAccessProfiles.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvtimeAccessProfiles.Sorting = SortOrder.Ascending;
                }
                lvtimeAccessProfiles.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeAccessProfiles.lvtimeAccessProfiles_ColumnClick(): " + ex.Message + "\n");
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
				TimeAccessProfileAdd addForm = new TimeAccessProfileAdd(); 
				addForm.ShowDialog(this);

				populateListView();
				currentTimeAccessProfile.Clear();
				this.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfiles.btnAdd_Click(): " + ex.Message + "\n");
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

				if (this.lvtimeAccessProfiles.SelectedItems.Count != 1)
				{
					MessageBox.Show(rm.GetString("selOneTimeAccessProfile", culture));
				}
				else 
				{			
					currentTimeAccessProfile.ReceiveTransferObject(currentTimeAccessProfile.Find( this.lvtimeAccessProfiles.SelectedItems[0].Tag.ToString() ));					

					// Open Update Form
					TimeAccessProfileAdd addForm = new TimeAccessProfileAdd(currentTimeAccessProfile);
					addForm.ShowDialog(this);

					populateListView();
					currentTimeAccessProfile.Clear();
					this.Invalidate();
				}

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfiles.btnUpdate_Click(): " + ex.Message + "\n");
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

				if (lvtimeAccessProfiles.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("noSelTimeAccessProfileDel", culture));
				}
				else
				{
					DialogResult result = MessageBox.Show(rm.GetString("deleteTimeAccessProfile", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;
						int selected = lvtimeAccessProfiles.SelectedItems.Count;
						foreach(ListViewItem item in lvtimeAccessProfiles.SelectedItems)
						{							
							if ((((int) item.Tag) == 0) || (((int) item.Tag) == 1))
							{
								MessageBox.Show(item.Text + ": " + rm.GetString("defaultTimeAccessProfile", culture));
								selected--;
							}
							else
							{								
								ArrayList gateTAProfiles = (new GateTimeAccessProfile()).Search("");
								if (gateTAProfiles.Count > 0)
								{
									bool inUse = false;
									int timeAcessProfileID = int.Parse(item.Tag.ToString());
									foreach (GateTimeAccessProfile gateTAProfile in gateTAProfiles)
									{
										for (int i = 0; i < 16; i++)
										{
											switch (i.ToString())
											{
												case "0":	
													if (timeAcessProfileID == gateTAProfile.GateTAProfile0)
														inUse = true;
													break;
												case "1":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile1)
														inUse = true;
													break;
												case "2":
													if (timeAcessProfileID == gateTAProfile.GateTAProfile2)
														inUse = true;
													break;
												case "3":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile3)
														inUse = true;
													break;
												case "4":
													if (timeAcessProfileID == gateTAProfile.GateTAProfile4)
														inUse = true;
													break;
												case "5":					
													if (timeAcessProfileID == gateTAProfile.GateTAProfile5)
														inUse = true;
													break;
												case "6":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile6)
														inUse = true;
													break;
												case "7":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile7)
														inUse = true;
													break;
												case "8":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile8)
														inUse = true;
													break;
												case "9":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile9)
														inUse = true;
													break;
												case "10":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile10)
														inUse = true;
													break;
												case "11":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile11)
														inUse = true;
													break;
												case "12":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile12)
														inUse = true;
													break;
												case "13":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile13)
														inUse = true;
													break;
												case "14":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile14)
														inUse = true;
													break;
												case "15":							
													if (timeAcessProfileID == gateTAProfile.GateTAProfile15)
														inUse = true;
													break;
											} //switch
											if (inUse)
												break;
										} //for (int i = 0; i < 16; i++)
										if (inUse)
											break;
									} //foreach
									if (inUse)
									{
										MessageBox.Show(item.Text + ": " + rm.GetString("timeAccessProfileHasGateTAP", culture));
										selected--;
									}
									else
									{
										isDeleted = currentTimeAccessProfile.Delete(item.Tag.ToString(), true) && isDeleted;
									}
								} //if (gateTAProfiles.Count > 0)
								else
								{
									isDeleted = currentTimeAccessProfile.Delete(item.Tag.ToString(), true) && isDeleted;
								}
							}
						} //foreach

						if ((selected > 0) && isDeleted)
						{
							MessageBox.Show(rm.GetString("timeAccessProfileDel", culture));
						}
						else if (!isDeleted)
						{
							MessageBox.Show(rm.GetString("noTimeAccessProfileDel", culture));
						}

						populateListView();
						currentTimeAccessProfile.Clear();						
						this.Invalidate();
					}//yes
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfiles.btnDelete_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " TimeAccessProfiles.btnClose_Click(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " TimeAccessProfiles.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void TimeAccessProfiles_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " TimeAccessProfiles.TimeAccessProfiles_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

	}
}
