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

using Util;
using Common;
using TransferObjects;
using ACTAConfigManipulation;

namespace UI
{
	/// <summary>
	/// Summary description for VisitorsView.
	/// </summary>
	public class VisitorsView : System.Windows.Forms.Form
    {
		private System.Windows.Forms.ListView lvVisitorsView;
        private Button btnClose;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private CultureInfo culture;
        DebugLog log;
        ResourceManager rm;

        private ListViewItemComparer _comp;

		// List View indexes
		const int EmployeeIDIndex = 0;
		const int FirstNameIndex = 1;
		const int LastNameIndex = 2;
		const int DateStartIndex = 3;

		public VisitorsView()
		{
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                this.CenterToScreen();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.GateResource", typeof(VisitorsView).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorsView.VisitorsView(): " + ex.Message + "\n");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisitorsView));
            this.lvVisitorsView = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvVisitorsView
            // 
            this.lvVisitorsView.FullRowSelect = true;
            this.lvVisitorsView.GridLines = true;
            this.lvVisitorsView.HideSelection = false;
            this.lvVisitorsView.Location = new System.Drawing.Point(28, 29);
            this.lvVisitorsView.Name = "lvVisitorsView";
            this.lvVisitorsView.Size = new System.Drawing.Size(656, 260);
            this.lvVisitorsView.TabIndex = 2;
            this.lvVisitorsView.UseCompatibleStateImageBehavior = false;
            this.lvVisitorsView.View = System.Windows.Forms.View.Details;
            this.lvVisitorsView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvVisitorsView_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(609, 323);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // VisitorsView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(712, 351);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvVisitorsView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(720, 385);
            this.MinimumSize = new System.Drawing.Size(720, 385);
            this.Name = "VisitorsView";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "VisitorsView";
            this.Load += new System.EventHandler(this.VisitorsView_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VisitorsView_KeyUp);
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
                    case VisitorsView.EmployeeIDIndex:
                    case VisitorsView.FirstNameIndex:
                    case VisitorsView.LastNameIndex:
                    {
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    }
                    case VisitorsView.DateStartIndex:
					{
                        DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                        DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

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

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("titleCurrentVisitors", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);

                // list view initialization
                lvVisitorsView.BeginUpdate();
                lvVisitorsView.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvVisitorsView.Width - 4) / 4, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrFirstName", culture), (lvVisitorsView.Width - 4) / 4, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrLastName", culture), (lvVisitorsView.Width - 4) / 4, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrDateStart", culture), (lvVisitorsView.Width - 4) / 4, HorizontalAlignment.Left);
                lvVisitorsView.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorsView.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void populateListView(ArrayList visitsList)
        {
            try
            {
                lvVisitorsView.BeginUpdate();
                lvVisitorsView.Items.Clear();

                if (visitsList.Count > 0)
                {
                    foreach (VisitTO visit in visitsList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = visit.EmployeeLastName.ToString() + " " + visit.EmployeeFirstName.ToString();
                        item.SubItems.Add(visit.FirstName.Trim());
                        item.SubItems.Add(visit.LastName.Trim());
                        item.SubItems.Add(visit.DateStart.ToString("dd.MM.yyyy   HH:mm").Trim());

                        lvVisitorsView.Items.Add(item);
                    }
                }

                lvVisitorsView.EndUpdate();
                lvVisitorsView.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorsView.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void VisitorsView_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string visitorCode = ConfigurationManager.AppSettings["VisitorsCode"];

                if (visitorCode == null)
                {
                    MessageBox.Show(rm.GetString("noVisitorsParameters", culture));

                    ConfigAdd conf = new ConfigAdd(rm.GetString("Visitors", culture));

                    conf.ShowDialog();

                    visitorCode = ConfigurationManager.AppSettings["VisitorsCode"];
                }

                if (visitorCode == null)
                {
                    this.Close();
                }
                else
                {
                    WorkingUnit workingunit = new WorkingUnit();
                    workingunit.WUTO.WorkingUnitID = int.Parse(visitorCode.Trim());
                    List<WorkingUnitTO> wUnitsVisitors = workingunit.Search();
                    wUnitsVisitors = workingunit.FindAllChildren(wUnitsVisitors);

                    string wUnits = "";
                    foreach (WorkingUnitTO wuV in wUnitsVisitors)
                    {
                        wUnits += wuV.WorkingUnitID.ToString().Trim() + ",";
                    }
                    if (wUnits.Length > 0)
                    {
                        wUnits = wUnits.Substring(0, wUnits.Length - 1);
                    }

                    Visit visit = new Visit();
                    ArrayList visits = visit.getCurrentVisits(wUnits);
                    populateListView(visits);

                    _comp = new ListViewItemComparer(lvVisitorsView);
                    lvVisitorsView.ListViewItemSorter = _comp;
                    lvVisitorsView.Sorting = SortOrder.Ascending;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorsView.VisitorsView_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvVisitorsView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvVisitorsView.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvVisitorsView.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvVisitorsView.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvVisitorsView.Sorting = SortOrder.Ascending;
                }
                lvVisitorsView.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorsView.lvVisitorsView_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvVisitorsView_ColumnClick():" + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorsView.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void VisitorsView_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " VisitorsView.VisitorsView_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
