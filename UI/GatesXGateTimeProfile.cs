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
	/// Summary description for GatesXGateTimeProfile.
	/// </summary>
	public class GatesXGateTimeProfile : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.ListView lvGates;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		GateTO currentGate = null;

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
		const int ProfileIndex = 1;
		const int DescriptionIndex = 2;

		public GatesXGateTimeProfile()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentGate = new GateTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(GatesXGateTimeProfile).Assembly);
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvGates
            // 
            this.lvGates.FullRowSelect = true;
            this.lvGates.GridLines = true;
            this.lvGates.HideSelection = false;
            this.lvGates.Location = new System.Drawing.Point(16, 16);
            this.lvGates.Name = "lvGates";
            this.lvGates.Size = new System.Drawing.Size(584, 328);
            this.lvGates.TabIndex = 1;
            this.lvGates.UseCompatibleStateImageBehavior = false;
            this.lvGates.View = System.Windows.Forms.View.Details;
            this.lvGates.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGates_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(528, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 360);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // GatesXGateTimeProfile
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(616, 390);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lvGates);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(624, 424);
            this.MinimumSize = new System.Drawing.Size(624, 424);
            this.Name = "GatesXGateTimeProfile";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Gates <-> Gate access profiles";
            this.Load += new System.EventHandler(this.GatesXGateTimeProfile_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GatesXGateTimeProfile_KeyUp);
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
					case GatesXGateTimeProfile.NameIndex:
					case GatesXGateTimeProfile.ProfileIndex:
					case GatesXGateTimeProfile.DescriptionIndex:
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
				this.Text = rm.GetString("gatesXGateTimeProfileForm", culture);

				// button's text
				btnUpdate.Text   = rm.GetString("btnUpdate", culture);
				btnClose.Text    = rm.GetString("btnClose", culture);

				// list view
				lvGates.BeginUpdate();
				lvGates.Columns.Add(rm.GetString("lblGateName", culture), (3 * (lvGates.Width - 4)) / 10, HorizontalAlignment.Left);
				lvGates.Columns.Add(rm.GetString("lblProfileName", culture), (3 * (lvGates.Width - 4)) / 10, HorizontalAlignment.Left);
				lvGates.Columns.Add(rm.GetString("lblProfileDescr", culture), (4 * (lvGates.Width - 4)) / 10, HorizontalAlignment.Left);
				lvGates.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesXGateTimeProfile.setLanguage(): " + ex.Message + "\n");
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
				List<GateTO> gatesList = new Gate().SearchGetGateTAProfile();

				lvGates.BeginUpdate();
				lvGates.Items.Clear();

				if (gatesList.Count > 0)
				{
					foreach(GateTO gate in gatesList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = gate.Name.Trim();
						// display Description field into 2 separate fields in list - profile
						// name and profile description. Delimiter is "~"
						int delimiterIndex = gate.Description.IndexOf("~");
						if (delimiterIndex > 0)
						{
							item.SubItems.Add(gate.Description.Substring(0, delimiterIndex).Trim());
							if (gate.Description.Length > delimiterIndex + 1)
								item.SubItems.Add(gate.Description.Substring(delimiterIndex + 1).Trim());
							else
								item.SubItems.Add("");
						}
						else if (delimiterIndex == 0)
						{
							item.SubItems.Add("");
							if (gate.Description.Length > 1)
								item.SubItems.Add(gate.Description.Substring(1).Trim());
							else
								item.SubItems.Add("");
						}
						else
						{
							item.SubItems.Add("");
							item.SubItems.Add("");
						}
						item.Tag = gate.GateID;
						
						lvGates.Items.Add(item);
					}
				}

				lvGates.EndUpdate();
				lvGates.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesXGateTimeProfile.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void GatesXGateTimeProfile_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvGates);
                lvGates.ListViewItemSorter = _comp;
                lvGates.Sorting = SortOrder.Ascending;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GatesXGateTimeProfile.GatesXGateTimeProfile_Load(): " + ex.Message + "\n");
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

				if (e.Column == _comp.SortColumn)
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
					_comp.SortColumn = e.Column;
					lvGates.Sorting = SortOrder.Ascending;
				}
                lvGates.ListViewItemSorter = _comp;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesXGateTimeProfile.lvGates_ColumnClick(): " + ex.Message + "\n");
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

				if (this.lvGates.SelectedItems.Count != 1)
				{
					MessageBox.Show(rm.GetString("selOneGate", culture));
				}
				else 
				{			
					currentGate = new Gate().FindGetAccessProfile(this.lvGates.SelectedItems[0].Tag.ToString());					

					// Open Update Form
					GatesXGateTimeProfileUpd addForm = new GatesXGateTimeProfileUpd(currentGate);
					addForm.ShowDialog(this);

					populateListView();
					currentGate = new GateTO();
					this.Invalidate();
				}

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesXGateTimeProfile.btnUpdate_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " GatesXGateTimeProfile.btnClose_Click(): " + ex.Message + "\n");
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

				btnUpdate.Enabled = updatePermission;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesXGateTimeProfile.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void GatesXGateTimeProfile_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " GatesXGateTimeProfile.GatesXGateTimeProfile_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }		
	}
}
