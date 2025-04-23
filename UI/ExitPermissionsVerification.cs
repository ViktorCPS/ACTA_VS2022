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

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for ExitPermissionsVerification.
	/// </summary>
	public class ExitPermissionsVerification : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvExitPerm;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnVerificate;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ExitPermission currentExitPerm = null;

		ApplUserTO logInUser;
		ResourceManager rm;				
		private CultureInfo culture;
		DebugLog log;		

		private ListViewItemComparer _comp;

		// List View indexes
		const int EmployeeIndex = 0;
		const int PassTypeIndex = 1;
		const int DateTimeIndex = 2;
		const int IssuedByIndex = 3;
		const int OffsetIndex = 4;
		const int UsedIndex = 5;
		const int DescriptionIndex = 6;

		private List<WorkingUnitTO> wUnits;		
		private string wuString = "";

		public ExitPermissionsVerification()
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				currentExitPerm = new ExitPermission();
				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("UI.Resource",typeof(ExitPermissionsVerification).Assembly);
				setLanguage();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsVerification.ExitPermissionsVerification(): " + ex.Message + "\n");
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
            this.lvExitPerm = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnVerificate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvExitPerm
            // 
            this.lvExitPerm.FullRowSelect = true;
            this.lvExitPerm.GridLines = true;
            this.lvExitPerm.HideSelection = false;
            this.lvExitPerm.Location = new System.Drawing.Point(16, 16);
            this.lvExitPerm.Name = "lvExitPerm";
            this.lvExitPerm.Size = new System.Drawing.Size(584, 328);
            this.lvExitPerm.TabIndex = 1;
            this.lvExitPerm.UseCompatibleStateImageBehavior = false;
            this.lvExitPerm.View = System.Windows.Forms.View.Details;
            this.lvExitPerm.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvExitPerm_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(528, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnVerificate
            // 
            this.btnVerificate.Location = new System.Drawing.Point(16, 360);
            this.btnVerificate.Name = "btnVerificate";
            this.btnVerificate.Size = new System.Drawing.Size(208, 23);
            this.btnVerificate.TabIndex = 2;
            this.btnVerificate.Text = "Verificate selected exit permissions";
            this.btnVerificate.Click += new System.EventHandler(this.btnVerificate_Click);
            // 
            // ExitPermissionsVerification
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(616, 390);
            this.ControlBox = false;
            this.Controls.Add(this.btnVerificate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvExitPerm);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(624, 424);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(624, 424);
            this.Name = "ExitPermissionsVerification";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Exit permissions verification";
            this.Load += new System.EventHandler(this.ExitPermissionsVerification_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExitPermissionsVerification_KeyUp);
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
					case ExitPermissionsVerification.EmployeeIndex:
					case ExitPermissionsVerification.PassTypeIndex:
					case ExitPermissionsVerification.IssuedByIndex:
					case ExitPermissionsVerification.UsedIndex:
					case ExitPermissionsVerification.DescriptionIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case ExitPermissionsVerification.OffsetIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text), 
							Int32.Parse(sub2.Text));
					}
					case ExitPermissionsVerification.DateTimeIndex:
					{
						DateTime dt1 = new DateTime(1,1,1,0,0,0);
						DateTime dt2 = new DateTime(1,1,1,0,0,0);

						if (!sub1.Text.Trim().Equals("")) 
						{
                            dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
						}

						if (!sub2.Text.Trim().Equals(""))
						{
                            dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
						}
						
						return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
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
				this.Text = rm.GetString("exitPermissionsVerificationForm", culture);

				// button's text
				btnVerificate.Text = rm.GetString("btnVerificate", culture);
				btnClose.Text      = rm.GetString("btnClose", culture);

				// list view
				lvExitPerm.BeginUpdate();
				lvExitPerm.Columns.Add(rm.GetString("hdrEmployee", culture), (lvExitPerm.Width - 4)/ 8,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvExitPerm.Width - 4) / 8, 
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrPassType", culture), (lvExitPerm.Width - 4) / 8,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrDateTime", culture), (lvExitPerm.Width - 4) / 8,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrIssuedBy", culture), (lvExitPerm.Width - 4) / 8,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrOffset", culture), (lvExitPerm.Width - 4) / 8,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrStatus", culture), (lvExitPerm.Width - 4) / 8,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrDescription", culture), (lvExitPerm.Width - 4) / 8,
					HorizontalAlignment.Left);
				lvExitPerm.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsVerification.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateListView()
		{
			try
			{
                ExitPermission perm = new ExitPermission();
                perm.PermTO.Used = (int)Constants.Used.Unverified;
                List<ExitPermissionTO> ExitPermissionsList = perm.SearchVerifiedBy(new DateTime(0), new DateTime(0), wuString);

				lvExitPerm.BeginUpdate();
				lvExitPerm.Items.Clear();

				CultureInfo ci = CultureInfo.InvariantCulture;

				if (ExitPermissionsList.Count > 0)
				{
					foreach(ExitPermissionTO exitPerm in ExitPermissionsList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = exitPerm.EmployeeName.Trim();
						item.SubItems.Add(exitPerm.WorkingUnitName.Trim());
						item.SubItems.Add(exitPerm.PassTypeDesc.Trim());
						if (!exitPerm.StartTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
						{
							item.SubItems.Add(exitPerm.StartTime.ToString("dd.MM.yyyy   HH:mm", ci));
						}
						else
						{								
							item.SubItems.Add("");
						}
						item.SubItems.Add(exitPerm.UserName.ToString().Trim());
						item.SubItems.Add(exitPerm.Offset.ToString().Trim());
						if (exitPerm.Used == (int) Constants.Used.No)
						{
							item.SubItems.Add(rm.GetString("not_used", culture));
						}
						else if (exitPerm.Used == (int) Constants.Used.Unverified)
						{
							item.SubItems.Add(rm.GetString("unverified", culture));
						}
						else
						{
							item.SubItems.Add(rm.GetString("used", culture));
						}
						item.SubItems.Add(exitPerm.Description.ToString().Trim());
						item.Tag = exitPerm.PermissionID;

						lvExitPerm.Items.Add(item);
					}
				}

				lvExitPerm.EndUpdate();
				lvExitPerm.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsVerification.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void ExitPermissionsVerification_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvExitPerm);
                lvExitPerm.ListViewItemSorter = _comp;
                lvExitPerm.Sorting = SortOrder.Ascending;

                wUnits = new List<WorkingUnitTO>();
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PermVerificationPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }
                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                if (!wuString.Equals(""))
                {
                    populateListView();
                }
                else
                {
                    MessageBox.Show(rm.GetString("noPermVerPrivilege", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsVerification.ExitPermissionsVerification_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvExitPerm_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvExitPerm.Sorting;
				lvExitPerm.Sorting = SortOrder.None;

				if (e.Column == _comp.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvExitPerm.Sorting = SortOrder.Descending;
					}
					else
					{
						lvExitPerm.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp.SortColumn = e.Column;
					lvExitPerm.Sorting = SortOrder.Ascending;
				}
                lvExitPerm.ListViewItemSorter = _comp;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsVerification.lvExitPerm_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {

                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnVerificate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvExitPerm.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelExitPermVer", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("verificateExitPerm", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool isUpdated = true;
                        foreach (ListViewItem item in lvExitPerm.SelectedItems)
                        {
                            ExitPermission perm = new ExitPermission();
                            perm.PermTO.PermissionID = (int)item.Tag;
                            List<ExitPermissionTO> selectedPerm = perm.SearchVerifiedBy(new DateTime(0), new DateTime(0), "");
                            if (selectedPerm.Count == 1)
                            {
                                currentExitPerm.PermTO = selectedPerm[0];

                                currentExitPerm.PermTO.Used = (int)Constants.Used.No;
                                currentExitPerm.PermTO.VerifiedBy = logInUser.UserID;

                                isUpdated = currentExitPerm.Update(currentExitPerm.PermTO.PermissionID,
                                    currentExitPerm.PermTO.EmployeeID, currentExitPerm.PermTO.PassTypeID,
                                    currentExitPerm.PermTO.StartTime, currentExitPerm.PermTO.Offset, currentExitPerm.PermTO.Used,
                                    currentExitPerm.PermTO.Description, currentExitPerm.PermTO.VerifiedBy) && isUpdated;
                            }
                            else
                                isUpdated = false;
                        } //foreach

                        if (isUpdated)
                        {
                            MessageBox.Show(rm.GetString("exitPermVer", culture));
                            populateListView();
                            currentExitPerm.Clear();
                            //this.Invalidate();

                            //this.Close();
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noExitPermVer", culture));
                            populateListView();
                            currentExitPerm.Clear();
                            //this.Invalidate();
                        }
                    } //yes
                } //lvExitPerm.SelectedItems.Count > 0
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsVerification.btnVerificate_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ExitPermissionsVerification.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void ExitPermissionsVerification_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ExitPermissionsVerification.ExitPermissionsVerification_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
