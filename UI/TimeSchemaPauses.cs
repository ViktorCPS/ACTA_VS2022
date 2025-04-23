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
	/// Summary description for TimeSchemaPauses.
	/// </summary>
	public class TimeSchemaPauses : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListView lvPauses;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		TimeSchemaPauseTO currentTimeSchemaPause = null;

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
		const int DescriptionIndex = 0;
		const int PauseDurationIndex = 1;
		const int EarliestUseTimeIndex = 2;
		const int LatestUseTimeIndex = 3;
		const int PauseOffsetIndex = 4;
		const int ShortBreakDurationIndex = 5;

		public TimeSchemaPauses()
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				currentTimeSchemaPause = new TimeSchemaPauseTO();
				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("UI.Resource",typeof(TimeSchemaPauses).Assembly);
				setLanguage();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPauses.TimeSchemaPauses(): " + ex.Message + "\n");
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
            this.lvPauses = new System.Windows.Forms.ListView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvPauses
            // 
            this.lvPauses.FullRowSelect = true;
            this.lvPauses.GridLines = true;
            this.lvPauses.HideSelection = false;
            this.lvPauses.Location = new System.Drawing.Point(16, 16);
            this.lvPauses.Name = "lvPauses";
            this.lvPauses.Size = new System.Drawing.Size(684, 328);
            this.lvPauses.TabIndex = 1;
            this.lvPauses.UseCompatibleStateImageBehavior = false;
            this.lvPauses.View = System.Windows.Forms.View.Details;
            this.lvPauses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPauses_ColumnClick);
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
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(104, 360);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
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
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(628, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // TimeSchemaPauses
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(716, 416);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvPauses);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(624, 424);
            this.Name = "TimeSchemaPauses";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Pauses";
            this.Load += new System.EventHandler(this.TimeSchemaPauses_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TimeSchemaPauses_KeyUp);
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
					case TimeSchemaPauses.DescriptionIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case TimeSchemaPauses.PauseDurationIndex:
					case TimeSchemaPauses.EarliestUseTimeIndex:
					case TimeSchemaPauses.LatestUseTimeIndex:
					case TimeSchemaPauses.PauseOffsetIndex:
					case TimeSchemaPauses.ShortBreakDurationIndex:
					{
						int firstID = -1;
						int secondID = -1;

						if (!sub1.Text.ToString().Trim().Equals("")) 
						{
							firstID = Int32.Parse(sub1.Text.ToString().Trim());
						}

						if (!sub2.Text.ToString().Trim().Equals(""))
						{
							secondID = Int32.Parse(sub2.Text.ToString().Trim());
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
				this.Text = rm.GetString("timeSchemaPausesForm", culture);

				// button's text
				btnAdd.Text    = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnClose.Text  = rm.GetString("btnClose", culture);			

				// list view
				lvPauses.BeginUpdate();
				lvPauses.Columns.Add(rm.GetString("hdrDescripton", culture), (3 * (lvPauses.Width - 4)) / 8, HorizontalAlignment.Left);
				lvPauses.Columns.Add(rm.GetString("hdrDuration", culture), (lvPauses.Width - 4) / 8, HorizontalAlignment.Left);
				lvPauses.Columns.Add(rm.GetString("hdrEarliestTime", culture), (lvPauses.Width - 4) / 8, HorizontalAlignment.Left);
				lvPauses.Columns.Add(rm.GetString("hdrLatestTime", culture), (lvPauses.Width - 4) / 8, HorizontalAlignment.Left);
				lvPauses.Columns.Add(rm.GetString("hdrOffset", culture), (lvPauses.Width - 4) / 8, HorizontalAlignment.Left);
				lvPauses.Columns.Add(rm.GetString("hdrShortBreak", culture), (lvPauses.Width - 4) / 8, HorizontalAlignment.Left);
				lvPauses.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPauses.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateListView()
		{
			try
			{
				List<TimeSchemaPauseTO> timeSchemaPausesList = new TimeSchemaPause().Search("");

				lvPauses.BeginUpdate();
				lvPauses.Items.Clear();

				if (timeSchemaPausesList.Count > 0)
				{
					foreach(TimeSchemaPauseTO timeSchemaPause in timeSchemaPausesList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = timeSchemaPause.Description.Trim();
						item.SubItems.Add(timeSchemaPause.PauseDuration.ToString());
						item.SubItems.Add(timeSchemaPause.EarliestUseTime.ToString());
						item.SubItems.Add(timeSchemaPause.LatestUseTime.ToString());
						item.SubItems.Add(timeSchemaPause.PauseOffset.ToString());
						item.SubItems.Add(timeSchemaPause.ShortBreakDuration.ToString());
						item.Tag = timeSchemaPause;
						
						lvPauses.Items.Add(item);
					}
				}

				lvPauses.EndUpdate();
				lvPauses.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPauses.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void TimeSchemaPauses_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvPauses);
                lvPauses.ListViewItemSorter = _comp;
                lvPauses.Sorting = SortOrder.Ascending;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchemaPauses.TimeSchemaPauses_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvPauses_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvPauses.Sorting;
                lvPauses.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvPauses.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvPauses.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvPauses.Sorting = SortOrder.Ascending;
                }
                lvPauses.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchemaPauses.lvPauses_ColumnClick(): " + ex.Message + "\n");
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
                TimeSchemaPauseAdd addForm = new TimeSchemaPauseAdd();
                addForm.ShowDialog(this);

                populateListView();
                currentTimeSchemaPause = new TimeSchemaPauseTO();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchemaPauses.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            
            }
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.lvPauses.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneTimeSchedulePause", culture));
                }
                else
                {
                    currentTimeSchemaPause = (TimeSchemaPauseTO)lvPauses.SelectedItems[0].Tag;

                    // Open Update Form
                    TimeSchemaPauseAdd addForm = new TimeSchemaPauseAdd(currentTimeSchemaPause);
                    addForm.ShowDialog(this);

                    populateListView();
                    currentTimeSchemaPause = new TimeSchemaPauseTO();
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchemaPauses.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            
            }
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvPauses.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelTimeSchedulePauseDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteTimeSchedulePauses", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool isDeleted = true;
                        int selected = lvPauses.SelectedItems.Count;
                        foreach (ListViewItem item in lvPauses.SelectedItems)
                        {
                            if (((TimeSchemaPauseTO)item.Tag).PauseID == 0)
                            {
                                MessageBox.Show(item.Text + ": " + rm.GetString("defaultTimeSchedulePause", culture));
                                selected--;
                            }
                            else
                            {
                                isDeleted = new TimeSchemaPause().Delete(((TimeSchemaPauseTO)item.Tag).PauseID) && isDeleted;
                            }
                        }

                        if ((selected > 0) && isDeleted)
                        {
                            MessageBox.Show(rm.GetString("timeSchedulePauseDel", culture));
                        }
                        else if (!isDeleted)
                        {
                            MessageBox.Show(rm.GetString("noTimeSchedulePauseDel", culture));
                        }

                        populateListView();
                        currentTimeSchemaPause = new TimeSchemaPauseTO();
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchemaPauses.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " TimeSchemaPauses.btnClose_Click(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " TimeSchemaPauses.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void TimeSchemaPauses_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " TimeSchemaPauses.TimeSchemaPauses_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
