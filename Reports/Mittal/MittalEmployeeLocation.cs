using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;

using TransferObjects;
using Common;
using Util;

using System.Resources;
using System.Globalization;

namespace Reports.Mittal
{
	/// <summary>
	/// Summary description for MittalEmployeeLocation.
	/// </summary>
	public class MittalEmployeeLocation : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbStatistic;
		private System.Windows.Forms.Label lblInside;
		private System.Windows.Forms.Label lblMittal;
		private System.Windows.Forms.Label lblOthers;
		private System.Windows.Forms.Label lblTotal;
		private System.Windows.Forms.TextBox tbMittalIN;
		private System.Windows.Forms.TextBox tbOthersIN;
		private System.Windows.Forms.TextBox tbTotalIN;
		private System.Windows.Forms.Label lblOutside;
		private System.Windows.Forms.TextBox tbTotalOUT;
		private System.Windows.Forms.TextBox tbOthersOUT;
		private System.Windows.Forms.TextBox tbMittalOUT;
		private System.Windows.Forms.Label lblTotalStaff;
		private System.Windows.Forms.TextBox tbTotalStaff;
		private System.Windows.Forms.ListView lvStaff;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnDetails;
		private System.Windows.Forms.Label lblPresence;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ApplUserTO logInUser;
		ResourceManager rm;				
		private CultureInfo culture;
		DebugLog log;

		private ListViewItemComparer _comp;

		// List View indexes
		const int LocationIndex = 0;
		const int MittalIndex = 1;
		const int OthersIndex = 2;
		const int TotalIndex = 3;

		public MittalEmployeeLocation()
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("Reports.ReportResource",typeof(MittalEmployeeLocation).Assembly);
				setLanguage();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocation.MittalEmployeeLocation(): " + ex.Message + "\n");
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
			this.gbStatistic = new System.Windows.Forms.GroupBox();
			this.tbTotalStaff = new System.Windows.Forms.TextBox();
			this.lblTotalStaff = new System.Windows.Forms.Label();
			this.tbTotalOUT = new System.Windows.Forms.TextBox();
			this.tbOthersOUT = new System.Windows.Forms.TextBox();
			this.lblOutside = new System.Windows.Forms.Label();
			this.tbMittalOUT = new System.Windows.Forms.TextBox();
			this.lblTotal = new System.Windows.Forms.Label();
			this.lblOthers = new System.Windows.Forms.Label();
			this.lblMittal = new System.Windows.Forms.Label();
			this.tbTotalIN = new System.Windows.Forms.TextBox();
			this.tbOthersIN = new System.Windows.Forms.TextBox();
			this.lblInside = new System.Windows.Forms.Label();
			this.tbMittalIN = new System.Windows.Forms.TextBox();
			this.lvStaff = new System.Windows.Forms.ListView();
			this.lblPresence = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnDetails = new System.Windows.Forms.Button();
			this.gbStatistic.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbStatistic
			// 
			this.gbStatistic.Controls.Add(this.tbTotalStaff);
			this.gbStatistic.Controls.Add(this.lblTotalStaff);
			this.gbStatistic.Controls.Add(this.tbTotalOUT);
			this.gbStatistic.Controls.Add(this.tbOthersOUT);
			this.gbStatistic.Controls.Add(this.lblOutside);
			this.gbStatistic.Controls.Add(this.tbMittalOUT);
			this.gbStatistic.Controls.Add(this.lblTotal);
			this.gbStatistic.Controls.Add(this.lblOthers);
			this.gbStatistic.Controls.Add(this.lblMittal);
			this.gbStatistic.Controls.Add(this.tbTotalIN);
			this.gbStatistic.Controls.Add(this.tbOthersIN);
			this.gbStatistic.Controls.Add(this.lblInside);
			this.gbStatistic.Controls.Add(this.tbMittalIN);
			this.gbStatistic.Location = new System.Drawing.Point(8, 8);
			this.gbStatistic.Name = "gbStatistic";
			this.gbStatistic.Size = new System.Drawing.Size(624, 160);
			this.gbStatistic.TabIndex = 1;
			this.gbStatistic.TabStop = false;
			this.gbStatistic.Text = "Statistic";
			// 
			// tbTotalStaff
			// 
			this.tbTotalStaff.Enabled = false;
			this.tbTotalStaff.Location = new System.Drawing.Point(416, 122);
			this.tbTotalStaff.Name = "tbTotalStaff";
			this.tbTotalStaff.Size = new System.Drawing.Size(80, 20);
			this.tbTotalStaff.TabIndex = 14;
			this.tbTotalStaff.Text = "";
			this.tbTotalStaff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblTotalStaff
			// 
			this.lblTotalStaff.Location = new System.Drawing.Point(288, 122);
			this.lblTotalStaff.Name = "lblTotalStaff";
			this.lblTotalStaff.Size = new System.Drawing.Size(120, 23);
			this.lblTotalStaff.TabIndex = 13;
			this.lblTotalStaff.Text = "Total staff:";
			this.lblTotalStaff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbTotalOUT
			// 
			this.tbTotalOUT.Enabled = false;
			this.tbTotalOUT.Location = new System.Drawing.Point(416, 89);
			this.tbTotalOUT.Name = "tbTotalOUT";
			this.tbTotalOUT.Size = new System.Drawing.Size(80, 20);
			this.tbTotalOUT.TabIndex = 12;
			this.tbTotalOUT.Text = "";
			this.tbTotalOUT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tbOthersOUT
			// 
			this.tbOthersOUT.Enabled = false;
			this.tbOthersOUT.Location = new System.Drawing.Point(296, 89);
			this.tbOthersOUT.Name = "tbOthersOUT";
			this.tbOthersOUT.Size = new System.Drawing.Size(80, 20);
			this.tbOthersOUT.TabIndex = 11;
			this.tbOthersOUT.Text = "";
			this.tbOthersOUT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblOutside
			// 
			this.lblOutside.Location = new System.Drawing.Point(48, 88);
			this.lblOutside.Name = "lblOutside";
			this.lblOutside.Size = new System.Drawing.Size(120, 23);
			this.lblOutside.TabIndex = 9;
			this.lblOutside.Text = "OUTSIDE:";
			this.lblOutside.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbMittalOUT
			// 
			this.tbMittalOUT.Enabled = false;
			this.tbMittalOUT.Location = new System.Drawing.Point(176, 88);
			this.tbMittalOUT.Name = "tbMittalOUT";
			this.tbMittalOUT.Size = new System.Drawing.Size(80, 20);
			this.tbMittalOUT.TabIndex = 10;
			this.tbMittalOUT.Text = "";
			this.tbMittalOUT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblTotal
			// 
			this.lblTotal.Location = new System.Drawing.Point(416, 24);
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.Size = new System.Drawing.Size(80, 23);
			this.lblTotal.TabIndex = 4;
			this.lblTotal.Text = "Total";
			this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblOthers
			// 
			this.lblOthers.Location = new System.Drawing.Point(296, 24);
			this.lblOthers.Name = "lblOthers";
			this.lblOthers.Size = new System.Drawing.Size(80, 23);
			this.lblOthers.TabIndex = 3;
			this.lblOthers.Text = "Others";
			this.lblOthers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMittal
			// 
			this.lblMittal.Location = new System.Drawing.Point(176, 24);
			this.lblMittal.Name = "lblMittal";
			this.lblMittal.Size = new System.Drawing.Size(80, 23);
			this.lblMittal.TabIndex = 2;
			this.lblMittal.Text = "Mittal";
			this.lblMittal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tbTotalIN
			// 
			this.tbTotalIN.Enabled = false;
			this.tbTotalIN.Location = new System.Drawing.Point(416, 56);
			this.tbTotalIN.Name = "tbTotalIN";
			this.tbTotalIN.Size = new System.Drawing.Size(80, 20);
			this.tbTotalIN.TabIndex = 8;
			this.tbTotalIN.Text = "";
			this.tbTotalIN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tbOthersIN
			// 
			this.tbOthersIN.Enabled = false;
			this.tbOthersIN.Location = new System.Drawing.Point(296, 56);
			this.tbOthersIN.Name = "tbOthersIN";
			this.tbOthersIN.Size = new System.Drawing.Size(80, 20);
			this.tbOthersIN.TabIndex = 7;
			this.tbOthersIN.Text = "";
			this.tbOthersIN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblInside
			// 
			this.lblInside.Location = new System.Drawing.Point(48, 56);
			this.lblInside.Name = "lblInside";
			this.lblInside.Size = new System.Drawing.Size(120, 23);
			this.lblInside.TabIndex = 5;
			this.lblInside.Text = "INSIDE:";
			this.lblInside.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbMittalIN
			// 
			this.tbMittalIN.Enabled = false;
			this.tbMittalIN.Location = new System.Drawing.Point(176, 56);
			this.tbMittalIN.Name = "tbMittalIN";
			this.tbMittalIN.Size = new System.Drawing.Size(80, 20);
			this.tbMittalIN.TabIndex = 6;
			this.tbMittalIN.Text = "";
			this.tbMittalIN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lvStaff
			// 
			this.lvStaff.FullRowSelect = true;
			this.lvStaff.GridLines = true;
			this.lvStaff.HideSelection = false;
			this.lvStaff.Location = new System.Drawing.Point(8, 208);
			this.lvStaff.MultiSelect = false;
			this.lvStaff.Name = "lvStaff";
			this.lvStaff.Size = new System.Drawing.Size(624, 318);
			this.lvStaff.TabIndex = 16;
			this.lvStaff.View = System.Windows.Forms.View.Details;
			this.lvStaff.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvStaff_ColumnClick);
			// 
			// lblPresence
			// 
			this.lblPresence.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblPresence.Location = new System.Drawing.Point(260, 184);
			this.lblPresence.Name = "lblPresence";
			this.lblPresence.Size = new System.Drawing.Size(372, 23);
			this.lblPresence.TabIndex = 15;
			this.lblPresence.Text = "Number of staff in presence:";
			this.lblPresence.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(557, 536);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 18;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnDetails
			// 
			this.btnDetails.Location = new System.Drawing.Point(8, 536);
			this.btnDetails.Name = "btnDetails";
			this.btnDetails.TabIndex = 17;
			this.btnDetails.Text = "Details ...";
			this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
			// 
			// MittalEmployeeLocation
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(642, 563);
			this.ControlBox = false;
			this.Controls.Add(this.btnDetails);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lblPresence);
			this.Controls.Add(this.lvStaff);
			this.Controls.Add(this.gbStatistic);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(650, 590);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(650, 590);
			this.Name = "MittalEmployeeLocation";
			this.ShowInTaskbar = false;
			this.Text = "Employee location";
			this.Load += new System.EventHandler(this.MittalEmployeeLocation_Load);
			this.gbStatistic.ResumeLayout(false);
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
					case MittalEmployeeLocation.LocationIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case MittalEmployeeLocation.MittalIndex:
					case MittalEmployeeLocation.OthersIndex:
					case MittalEmployeeLocation.TotalIndex:
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
				this.Text = rm.GetString("mittalEmployeeLocationReports", culture);

				// button's text
				btnDetails.Text = rm.GetString("btnDetails", culture);
				btnClose.Text   = rm.GetString("btnClose", culture);	
			
				// label's view
				lblMittal.Text     = rm.GetString("lblMittal", culture);
				lblOthers.Text     = rm.GetString("lblOthers", culture);
				lblTotal.Text      = rm.GetString("lblTotal", culture);
				lblInside.Text     = rm.GetString("lblInside", culture);
				lblOutside.Text    = rm.GetString("lblOutside", culture);
				lblTotalStaff.Text = rm.GetString("lblTotalStaff", culture);
				lblPresence.Text   = rm.GetString("lblPresence", culture);

				// group box text
				gbStatistic.Text = rm.GetString("gbStatistic", culture);

				// list view
				lvStaff.BeginUpdate();
				lvStaff.Columns.Add(rm.GetString("lblLocation", culture), (4 * (lvStaff.Width - 4)) / 10, HorizontalAlignment.Left);
				lvStaff.Columns.Add(rm.GetString("lblMittal", culture), (2 * (lvStaff.Width - 4)) / 10, HorizontalAlignment.Left);
				lvStaff.Columns.Add(rm.GetString("lblOthers", culture), (2 * (lvStaff.Width - 4)) / 10, HorizontalAlignment.Left);
				lvStaff.Columns.Add(rm.GetString("lblTotal", culture), (2 * (lvStaff.Width - 4)) / 10, HorizontalAlignment.Left);
				lvStaff.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocation.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateListView(List<EmployeeLocationTO> staffList)
		{
			try
			{
				lvStaff.BeginUpdate();
				lvStaff.Items.Clear();

				if (staffList.Count > 0)
				{
					foreach(EmployeeLocationTO employeeLocation in staffList)
					{
						LocationTO locationTO = new Location().Find(employeeLocation.LocationID);
						ListViewItem item = new ListViewItem();						
						item.Text = locationTO.Description; 
						item.SubItems.Add(employeeLocation.EmployeeID.ToString()); //Mittal is in employee_id
						item.SubItems.Add(employeeLocation.Antenna.ToString());  //Other is in antena
						item.SubItems.Add(employeeLocation.PassTypeID.ToString()); //Total is in pass_type_id
						item.Tag = employeeLocation.LocationID;
						
						lvStaff.Items.Add(item);
					}
				}

				lvStaff.EndUpdate();
				lvStaff.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocation.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void MittalEmployeeLocation_Load(object sender, System.EventArgs e)
		{
			try
			{
            this.Cursor = Cursors.WaitCursor;
            
				// Initialize comparer object
				_comp = new ListViewItemComparer(lvStaff);
				lvStaff.ListViewItemSorter = _comp;
				lvStaff.Sorting = SortOrder.Ascending;

                WorkingUnitTO visitTO = new WorkingUnitTO(Constants.basicVisitorCode, 0, "", "", "", 0);
				List<WorkingUnitTO> visitList = new List<WorkingUnitTO>();
                visitList.Add(visitTO);
				List<WorkingUnitTO> wUnits = new WorkingUnit().FindAllChildren(visitList);
				string wuString = "";
				foreach (WorkingUnitTO wUnit in wUnits)
				{
					wuString += wUnit.WorkingUnitID.ToString().Trim() + ","; 
				}				
				if (wuString.Length > 0)
				{
					wuString = wuString.Substring(0, wuString.Length - 1);
				}

                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				List<LocationTO> locations = loc.Search();
				List<EmployeeLocationTO> mittalArray = new EmployeeLocation().SearchMittal(wuString);
				List<EmployeeLocationTO> otherArray  = new EmployeeLocation().SearchOther(wuString);

				int mittalInside  = 0;
				int otherInside   = 0;
				int totalInside   = 0;
				int mittalOutside = 0;
				int otherOutside  = 0;
				int totalOutside  = 0;
				int totalStaff    = 0;
				List<EmployeeLocationTO> staffList = new List<EmployeeLocationTO>();
				//put Mittal in employee_id
				//put Other in antena
				//put Total in pass_type_id
				foreach (LocationTO location in locations)				
				{
					EmployeeLocationTO elResult = new EmployeeLocationTO();
					elResult.LocationID = location.LocationID;
					elResult.EmployeeID = 0; 
					elResult.Antenna    = 0;
					foreach (EmployeeLocationTO elMittal in mittalArray)
					{
						if (location.LocationID == elMittal.LocationID)
						{
							elResult.EmployeeID = elMittal.ReaderID; //count(*) is in reader_id property
							break;
						}
					}
					foreach (EmployeeLocationTO elOther in otherArray)
					{
						if (location.LocationID == elOther.LocationID)
						{
							elResult.Antenna = elOther.ReaderID; //count(*) is in reader_id property
							break;
						}
					}					
					elResult.PassTypeID = elResult.EmployeeID + elResult.Antenna;
					mittalInside += elResult.EmployeeID;
					otherInside  += elResult.Antenna;

					staffList.Add(elResult);
				}	
				totalInside   = mittalInside + otherInside;
				mittalOutside = new EmployeeLocation().SearchTotalMittalOut(wuString);
				otherOutside  = new EmployeeLocation().SearchTotalOtherOut(wuString);
				totalOutside  = mittalOutside + otherOutside;
				totalStaff    = totalInside + totalOutside;

				tbMittalIN.Text   = mittalInside.ToString();
				tbOthersIN.Text   = otherInside.ToString(); 
				tbTotalIN.Text    = totalInside.ToString(); 
				tbMittalOUT.Text  = mittalOutside.ToString(); 
				tbOthersOUT.Text  = otherOutside.ToString(); 
				tbTotalOUT.Text   = totalOutside.ToString(); 
				tbTotalStaff.Text = totalStaff.ToString(); 

				populateListView(staffList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocation.MittalEmployeeLocation_Load(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvStaff_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvStaff.Sorting;
				lvStaff.Sorting = SortOrder.None;

				if (e.Column == _comp.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvStaff.Sorting = SortOrder.Descending;
					}
					else
					{
						lvStaff.Sorting = SortOrder.Ascending;
					}

				}
				else
				{
					// New Sort Order
					_comp.SortColumn = e.Column;
					lvStaff.Sorting = SortOrder.Ascending;
				}
                lvStaff.ListViewItemSorter = _comp;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocation.lvStaff_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnDetails_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				string locationID = "";
				if (lvStaff.SelectedItems.Count == 1)
				{
					locationID = lvStaff.SelectedItems[0].Tag.ToString();
				}
				Reports.Mittal.MittalEmployeeLocationDetails mittalEmployeeLocationDetails = new Reports.Mittal.MittalEmployeeLocationDetails(locationID);
				mittalEmployeeLocationDetails.ShowDialog(this);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocation.btnDetails_Click(): " + ex.Message + "\n");
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
				this.Close();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " MittalEmployeeLocation.btnCancel_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
	}
}
