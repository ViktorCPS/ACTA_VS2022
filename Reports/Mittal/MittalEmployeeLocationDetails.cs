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

namespace Reports.Mittal
{
	/// <summary>
	/// Summary description for MittalEmployeeLocationDetails.
	/// </summary>
	public class MittalEmployeeLocationDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ComboBox cbLocation;
		private System.Windows.Forms.Label lblTotal;
		private System.Windows.Forms.Label lblOthers;
		private System.Windows.Forms.Label lblMittal;
		private System.Windows.Forms.TabPage tabPageMittal;
		private System.Windows.Forms.TabPage tabPageOthers;
		private System.Windows.Forms.GroupBox gbStatisticforLocation;
		private System.Windows.Forms.TextBox tbTotal;
		private System.Windows.Forms.TextBox tbOthers;
		private System.Windows.Forms.TextBox tbMittal;
		private System.Windows.Forms.ListView lvStaffMittal;
		private System.Windows.Forms.ListView lvStaffOthers;
		private System.Windows.Forms.GroupBox gbLocation;
		private System.Windows.Forms.Label lblLocation;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ApplUserTO logInUser;
		ResourceManager rm;			
		private CultureInfo culture;
		DebugLog log;

		private ListViewItemComparer _comp1;
		private ListViewItemComparer _comp2;

		// List View indexes
		const int EmployeeIndex = 0;
		const int WorkingUnitIndex = 1;
		const int PassTypeIndex = 2;
		const int EventTimeIndex = 3;

		string wuString = "";
		string hdrLocationID = "";

		public MittalEmployeeLocationDetails(string locationID)
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("Reports.ReportResource",typeof(MittalEmployeeLocationDetails).Assembly);				
				setLanguage();

				hdrLocationID = locationID;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.MittalEmployeeLocationDetails(): " + ex.Message + "\n");
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMittal = new System.Windows.Forms.TabPage();
            this.lvStaffMittal = new System.Windows.Forms.ListView();
            this.tabPageOthers = new System.Windows.Forms.TabPage();
            this.lvStaffOthers = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbLocation = new System.Windows.Forms.GroupBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.gbStatisticforLocation = new System.Windows.Forms.GroupBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblOthers = new System.Windows.Forms.Label();
            this.lblMittal = new System.Windows.Forms.Label();
            this.tbTotal = new System.Windows.Forms.TextBox();
            this.tbOthers = new System.Windows.Forms.TextBox();
            this.tbMittal = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageMittal.SuspendLayout();
            this.tabPageOthers.SuspendLayout();
            this.gbLocation.SuspendLayout();
            this.gbStatisticforLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMittal);
            this.tabControl1.Controls.Add(this.tabPageOthers);
            this.tabControl1.Location = new System.Drawing.Point(8, 160);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(624, 368);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPageMittal
            // 
            this.tabPageMittal.Controls.Add(this.lvStaffMittal);
            this.tabPageMittal.Location = new System.Drawing.Point(4, 22);
            this.tabPageMittal.Name = "tabPageMittal";
            this.tabPageMittal.Size = new System.Drawing.Size(616, 342);
            this.tabPageMittal.TabIndex = 0;
            this.tabPageMittal.Text = "Mittal";
            // 
            // lvStaffMittal
            // 
            this.lvStaffMittal.FullRowSelect = true;
            this.lvStaffMittal.GridLines = true;
            this.lvStaffMittal.HideSelection = false;
            this.lvStaffMittal.Location = new System.Drawing.Point(8, 10);
            this.lvStaffMittal.MultiSelect = false;
            this.lvStaffMittal.Name = "lvStaffMittal";
            this.lvStaffMittal.Size = new System.Drawing.Size(600, 322);
            this.lvStaffMittal.TabIndex = 12;
            this.lvStaffMittal.UseCompatibleStateImageBehavior = false;
            this.lvStaffMittal.View = System.Windows.Forms.View.Details;
            this.lvStaffMittal.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvStaffMittal_ColumnClick);
            // 
            // tabPageOthers
            // 
            this.tabPageOthers.Controls.Add(this.lvStaffOthers);
            this.tabPageOthers.Location = new System.Drawing.Point(4, 22);
            this.tabPageOthers.Name = "tabPageOthers";
            this.tabPageOthers.Size = new System.Drawing.Size(616, 342);
            this.tabPageOthers.TabIndex = 1;
            this.tabPageOthers.Text = "Others";
            // 
            // lvStaffOthers
            // 
            this.lvStaffOthers.FullRowSelect = true;
            this.lvStaffOthers.GridLines = true;
            this.lvStaffOthers.HideSelection = false;
            this.lvStaffOthers.Location = new System.Drawing.Point(8, 10);
            this.lvStaffOthers.MultiSelect = false;
            this.lvStaffOthers.Name = "lvStaffOthers";
            this.lvStaffOthers.Size = new System.Drawing.Size(600, 322);
            this.lvStaffOthers.TabIndex = 13;
            this.lvStaffOthers.UseCompatibleStateImageBehavior = false;
            this.lvStaffOthers.View = System.Windows.Forms.View.Details;
            this.lvStaffOthers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvStaffOthers_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(557, 536);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbLocation
            // 
            this.gbLocation.Controls.Add(this.lblLocation);
            this.gbLocation.Controls.Add(this.cbLocation);
            this.gbLocation.Location = new System.Drawing.Point(8, 8);
            this.gbLocation.Name = "gbLocation";
            this.gbLocation.Size = new System.Drawing.Size(624, 56);
            this.gbLocation.TabIndex = 1;
            this.gbLocation.TabStop = false;
            this.gbLocation.Text = "Location";
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(8, 24);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(104, 23);
            this.lblLocation.TabIndex = 2;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(128, 24);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(256, 21);
            this.cbLocation.TabIndex = 3;
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
            // 
            // gbStatisticforLocation
            // 
            this.gbStatisticforLocation.Controls.Add(this.lblTotal);
            this.gbStatisticforLocation.Controls.Add(this.lblOthers);
            this.gbStatisticforLocation.Controls.Add(this.lblMittal);
            this.gbStatisticforLocation.Controls.Add(this.tbTotal);
            this.gbStatisticforLocation.Controls.Add(this.tbOthers);
            this.gbStatisticforLocation.Controls.Add(this.tbMittal);
            this.gbStatisticforLocation.Location = new System.Drawing.Point(8, 72);
            this.gbStatisticforLocation.Name = "gbStatisticforLocation";
            this.gbStatisticforLocation.Size = new System.Drawing.Size(624, 80);
            this.gbStatisticforLocation.TabIndex = 4;
            this.gbStatisticforLocation.TabStop = false;
            this.gbStatisticforLocation.Text = "Statistic for selected location";
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(400, 16);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(180, 23);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "Total";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOthers
            // 
            this.lblOthers.Location = new System.Drawing.Point(264, 16);
            this.lblOthers.Name = "lblOthers";
            this.lblOthers.Size = new System.Drawing.Size(120, 23);
            this.lblOthers.TabIndex = 6;
            this.lblOthers.Text = "Others";
            this.lblOthers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMittal
            // 
            this.lblMittal.Location = new System.Drawing.Point(128, 16);
            this.lblMittal.Name = "lblMittal";
            this.lblMittal.Size = new System.Drawing.Size(120, 23);
            this.lblMittal.TabIndex = 5;
            this.lblMittal.Text = "Mittal";
            this.lblMittal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTotal
            // 
            this.tbTotal.Enabled = false;
            this.tbTotal.Location = new System.Drawing.Point(400, 48);
            this.tbTotal.Name = "tbTotal";
            this.tbTotal.Size = new System.Drawing.Size(180, 20);
            this.tbTotal.TabIndex = 10;
            this.tbTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbOthers
            // 
            this.tbOthers.Enabled = false;
            this.tbOthers.Location = new System.Drawing.Point(264, 48);
            this.tbOthers.Name = "tbOthers";
            this.tbOthers.Size = new System.Drawing.Size(120, 20);
            this.tbOthers.TabIndex = 9;
            this.tbOthers.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbMittal
            // 
            this.tbMittal.Enabled = false;
            this.tbMittal.Location = new System.Drawing.Point(128, 48);
            this.tbMittal.Name = "tbMittal";
            this.tbMittal.Size = new System.Drawing.Size(120, 20);
            this.tbMittal.TabIndex = 8;
            this.tbMittal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MittalEmployeeLocationDetails
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(642, 563);
            this.ControlBox = false;
            this.Controls.Add(this.gbStatisticforLocation);
            this.Controls.Add(this.gbLocation);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(650, 590);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 590);
            this.Name = "MittalEmployeeLocationDetails";
            this.ShowInTaskbar = false;
            this.Text = "Employee location details";
            this.Load += new System.EventHandler(this.MittalEmployeeLocationDetails_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMittal.ResumeLayout(false);
            this.tabPageOthers.ResumeLayout(false);
            this.gbLocation.ResumeLayout(false);
            this.gbStatisticforLocation.ResumeLayout(false);
            this.gbStatisticforLocation.PerformLayout();
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
					case MittalEmployeeLocationDetails.EmployeeIndex:
					case MittalEmployeeLocationDetails.WorkingUnitIndex:
					case MittalEmployeeLocationDetails.PassTypeIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case MittalEmployeeLocationDetails.EventTimeIndex:
					{
						DateTime dt1 = new DateTime(1,1,1,0,0,0);
						DateTime dt2 = new DateTime(1,1,1,0,0,0);

						if (!sub1.Text.Trim().Equals("")) 
						{
                            dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy HH:mm", null);
						}

						if (!sub2.Text.Trim().Equals(""))
						{
                            dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy HH:mm", null);
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
				this.Text = rm.GetString("mittalEmployeeLocationDetailsReports", culture);

				// button's text
				btnClose.Text = rm.GetString("btnClose", culture);	
			
				// label's view
				lblLocation.Text = rm.GetString("lblLocation", culture);
				lblMittal.Text   = rm.GetString("lblMittal", culture);
				lblOthers.Text   = rm.GetString("lblOthers", culture);
				lblTotal.Text    = rm.GetString("lblTotal", culture);

				// group box text
				gbLocation.Text             = rm.GetString("location", culture);
				gbStatisticforLocation.Text = rm.GetString("gbStatisticforLocation", culture);

			    // TabPage text
				tabPageMittal.Text = rm.GetString("tabPageMittal", culture);
				tabPageOthers.Text = rm.GetString("tabPageOthers", culture);

				// list view
				lvStaffMittal.BeginUpdate();
				lvStaffMittal.Columns.Add(rm.GetString("hdrEmployee", culture), (lvStaffMittal.Width - 4) / 4, HorizontalAlignment.Left);
				lvStaffMittal.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvStaffMittal.Width - 4) / 4, HorizontalAlignment.Left);
				lvStaffMittal.Columns.Add(rm.GetString("hdrPassType", culture), (lvStaffMittal.Width - 4) / 4, HorizontalAlignment.Left);
				lvStaffMittal.Columns.Add(rm.GetString("hdrEventTime", culture), (lvStaffMittal.Width - 4) / 4, HorizontalAlignment.Left);
				lvStaffMittal.EndUpdate();

				lvStaffOthers.BeginUpdate();
				lvStaffOthers.Columns.Add(rm.GetString("hdrEmployee", culture), (lvStaffOthers.Width - 4) / 4, HorizontalAlignment.Left);
				lvStaffOthers.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvStaffOthers.Width - 4) / 4, HorizontalAlignment.Left);
				lvStaffOthers.Columns.Add(rm.GetString("hdrPassType", culture), (lvStaffOthers.Width - 4) / 4, HorizontalAlignment.Left);
				lvStaffOthers.Columns.Add(rm.GetString("hdrEventTime", culture), (lvStaffOthers.Width - 4) / 4, HorizontalAlignment.Left);
				lvStaffOthers.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Location Combo Box
		/// </summary>
		private void populateLocationCombo()
		{
			try
			{
				List<LocationTO> locArray = new List<LocationTO>();
				
				Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				locArray = loc.Search();
								
				locArray.Insert(0, new LocationTO(-1, "OUT", "", -1, 0, ""));
				locArray.Insert(0, new LocationTO(-2, rm.GetString("all", culture), rm.GetString("all", culture), 0, 0, ""));

				cbLocation.DataSource = locArray;
				cbLocation.DisplayMember = "Name";
				cbLocation.ValueMember = "LocationID";				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.populateLocationCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateStaffListView(List<EmployeeLocationTO> staffList, ListView lvName)
		{
			try
			{
				lvName.BeginUpdate();
				lvName.Items.Clear();
				
				if (staffList.Count > 0)
				{
					foreach(EmployeeLocationTO emplLoc in staffList)
					{
						ListViewItem item = new ListViewItem();						
						item.Text = emplLoc.EmployeeName.Trim();
						item.SubItems.Add(emplLoc.WUName);						
						item.SubItems.Add(emplLoc.PassType);

						if (!emplLoc.EventTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
						{
							item.SubItems.Add(emplLoc.EventTime.ToString("dd.MM.yyyy HH:mm"));
						}
						else
						{								
							item.SubItems.Add("");
						}

						item.Tag = emplLoc.EmployeeID;
											
						lvName.Items.Add(item);	
					}
				}

				lvName.EndUpdate();
				lvName.Invalidate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.populateStaffListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void MittalEmployeeLocationDetails_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp1 = new ListViewItemComparer(lvStaffMittal);
                lvStaffMittal.ListViewItemSorter = _comp1;
                lvStaffMittal.Sorting = SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvStaffOthers);
                lvStaffOthers.ListViewItemSorter = _comp2;
                lvStaffOthers.Sorting = SortOrder.Ascending;

                WorkingUnitTO visit = new WorkingUnitTO(Constants.basicVisitorCode, 0, "", "", "", 0);
                List<WorkingUnitTO> visitArray = new List<WorkingUnitTO>();
                visitArray.Add(visit);
                List<WorkingUnitTO> wUnits = new WorkingUnit().FindAllChildren(visitArray);

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }
                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateLocationCombo();
                if (!hdrLocationID.Equals(""))
                    cbLocation.SelectedValue = Int32.Parse(hdrLocationID);
                else
                    cbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.MittalEmployeeLocationDetails_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvStaffMittal_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvStaffMittal.Sorting;
                lvStaffMittal.Sorting = SortOrder.None;

                if (e.Column == _comp1.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvStaffMittal.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvStaffMittal.Sorting = SortOrder.Ascending;
                    }
                    
                }
                else
                {
                    // New Sort Order
                    _comp1.SortColumn = e.Column;
                    lvStaffMittal.Sorting = SortOrder.Ascending;
                }
                lvStaffMittal.ListViewItemSorter = _comp1;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.lvStaffMittal_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvStaffOthers_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvStaffOthers.Sorting;
				lvStaffOthers.Sorting = SortOrder.None;

				if (e.Column == _comp2.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvStaffOthers.Sorting = SortOrder.Descending;
					}
					else
					{
						lvStaffOthers.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp2.SortColumn = e.Column;
					lvStaffOthers.Sorting = SortOrder.Ascending;
				}
                lvStaffOthers.ListViewItemSorter = _comp2;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.lvStaffOthers_ColumnClick(): " + ex.Message + "\n");
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
				this.Close();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.btnClose_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void cbLocation_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{			
            this.Cursor = Cursors.WaitCursor;
            
				if (cbLocation.SelectedIndex > 0)
				{
					string locationID = cbLocation.SelectedValue.ToString();
					List<EmployeeLocationTO> currentMittalList = new EmployeeLocation().SearchMittalDet(locationID, wuString);
					List<EmployeeLocationTO> currentOtherList = new EmployeeLocation().SearchOtherDet(locationID, wuString);

					populateStaffListView(currentMittalList, lvStaffMittal);
					populateStaffListView(currentOtherList, lvStaffOthers);					

					int mittal = currentMittalList.Count;
					int other  = currentOtherList.Count;
					int total  = mittal + other;
					tbMittal.Text = mittal.ToString();					
					tbOthers.Text = other.ToString();
					tbTotal.Text  = total.ToString();
				}
				else
				{
					List<EmployeeLocationTO> empty = new List<EmployeeLocationTO>();
					populateStaffListView(empty, lvStaffMittal);
					populateStaffListView(empty, lvStaffOthers);

					tbMittal.Text = "";					
					tbOthers.Text = "";
					tbTotal.Text  = "";
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocationDetails.cbLocation_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}
	}
}
