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

namespace UI
{
	/// <summary>
	/// Summary description for Passes.
	/// </summary>
	public class ExitPermissionPasses : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvPasses;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private PassTO currentPass = null;

		// List View indexes
		const int PassIDIndex = 0;
		const int EmployeeIDIndex = 1;
		const int DirectionIndex = 2;
		const int EventTimeIndex = 3;
		const int PassTypeIDIndex = 4;
		const int IsWrkHrsIndex = 5;
		const int LocationIDIndex = 6;
		const int PairGenUsedIndex = 7;
		const int ManualCreatedIndex = 8;

		private ListViewItemComparer _comp;

		private CultureInfo culture;
		private ResourceManager rm;
		DebugLog log;
		ApplUserTO logInUser;

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;

		public ExitPermissionPasses(List<PassTO> passes)
		{
			InitializeComponent();
			this.CenterToScreen();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Passes).Assembly);
			setLanguage();
			currentPass = new PassTO();
			logInUser = NotificationController.GetLogInUser();

			_comp = new ListViewItemComparer(lvPasses);
			lvPasses.ListViewItemSorter = _comp;
			lvPasses.Sorting = SortOrder.Ascending;

			populateListView(passes);
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
            this.lvPasses = new System.Windows.Forms.ListView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvPasses
            // 
            this.lvPasses.FullRowSelect = true;
            this.lvPasses.GridLines = true;
            this.lvPasses.HideSelection = false;
            this.lvPasses.Location = new System.Drawing.Point(16, 16);
            this.lvPasses.MultiSelect = false;
            this.lvPasses.Name = "lvPasses";
            this.lvPasses.Size = new System.Drawing.Size(880, 216);
            this.lvPasses.TabIndex = 1;
            this.lvPasses.UseCompatibleStateImageBehavior = false;
            this.lvPasses.View = System.Windows.Forms.View.Details;
            this.lvPasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPasses_ColumnClick);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(16, 256);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(824, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ExitPermissionPasses
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(920, 286);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lvPasses);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(928, 320);
            this.MinimumSize = new System.Drawing.Size(928, 320);
            this.Name = "ExitPermissionPasses";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Passes";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExitPermissionPasses_KeyUp);
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
					case ExitPermissionPasses.PassIDIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()), Int32.Parse(sub2.Text.Trim()));
					}
					case ExitPermissionPasses.EmployeeIDIndex:
					case ExitPermissionPasses.PassTypeIDIndex:
					case ExitPermissionPasses.IsWrkHrsIndex:
					case ExitPermissionPasses.LocationIDIndex:
					case ExitPermissionPasses.PairGenUsedIndex:
					case ExitPermissionPasses.ManualCreatedIndex:
					case ExitPermissionPasses.DirectionIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case ExitPermissionPasses.EventTimeIndex:
					{
						DateTime dt1 = new DateTime(1,1,1,0,0,0);
						DateTime dt2 = new DateTime(1,1,1,0,0,0);

						if (!sub1.Text.Trim().Equals("")) 
						{
                            dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
						}

						if (!sub2.Text.Trim().Equals(""))
						{
                            dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy   HH:mm",null);
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
		/// Set proper language.
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// button's text
				btnOK.Text = rm.GetString("btnOK", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
			
				// Form name
				this.Text = rm.GetString("menuPasses", culture);
			
				// label's text
				
				// list view initialization
				lvPasses.BeginUpdate();
                lvPasses.Columns.Add(rm.GetString("hdrPassID", culture), (lvPasses.Width - 9) / 9 - 15, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvPasses.Width - 9) / 9 + 15, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrDirection", culture), (lvPasses.Width - 9) / 9 - 35, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrEventTime", culture), (lvPasses.Width - 9) / 9 + 15, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrPassTypeID", culture), (lvPasses.Width - 9) / 9, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrIsWrkHrs", culture), (lvPasses.Width - 9) / 9 + 25, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrLocation", culture), (lvPasses.Width - 9) / 9, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrPairGenUsed", culture), (lvPasses.Width - 9) / 9 - 30, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrManualCreated", culture), (lvPasses.Width - 9) / 9 + 20, HorizontalAlignment.Left);
				lvPasses.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionPasses.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with Passes found
		/// </summary>
		/// <param name="locationsList"></param>
		private void populateListView(List<PassTO> passesList)
		{
			try
			{
				lvPasses.BeginUpdate();
				lvPasses.Items.Clear();

				if (passesList.Count > 0)
				{
					foreach(PassTO pass in passesList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = pass.PassID.ToString().Trim();
						item.SubItems.Add(pass.EmployeeName.Trim());
						item.SubItems.Add(pass.Direction.Trim());
						if (!pass.EventTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
						{
							item.SubItems.Add(pass.EventTime.ToString("dd.MM.yyyy   HH:mm"));
						}
						else
						{								
							item.SubItems.Add("");
						}
						item.SubItems.Add(pass.PassType.Trim());
						if (pass.IsWrkHrsCount == (int) Constants.IsWrkCount.IsCounter)
						{
							item.SubItems.Add(rm.GetString("yes", culture));
						}
						else
						{
							item.SubItems.Add(rm.GetString("no", culture));
						}
						item.SubItems.Add(pass.LocationName.Trim());
						if (pass.PairGenUsed == (int) Constants.PairGenUsed.Used)
						{
							item.SubItems.Add(rm.GetString("yes", culture));
						}
						else
						{
							item.SubItems.Add(rm.GetString("no", culture));
						}
						if (pass.ManualyCreated == (int) Constants.recordCreated.Manualy)
						{
							item.SubItems.Add(rm.GetString("yes", culture));
						}
						else
						{
							item.SubItems.Add(rm.GetString("no", culture));
						}
					
						lvPasses.Items.Add(item);
					}
				}

				lvPasses.EndUpdate();
				_comp.SortColumn = ExitPermissionPasses.EventTimeIndex;
				lvPasses.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionPasses.populateListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void lvPasses_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvPasses.Sorting;
                lvPasses.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvPasses.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvPasses.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvPasses.Sorting = SortOrder.Ascending;
                }
                lvPasses.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionPasses.lvPasses_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionPasses.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (lvPasses.SelectedItems.Count == 1)
				{
					PassTO passTO = new Pass().Find(lvPasses.SelectedItems[0].Text.Trim());
					((ExitPermissionsAdd) this.Owner).setSelectedPass(passTO);
					this.Close();
				}
				else
				{
					MessageBox.Show(rm.GetString("selPassExitPerm", culture));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionPasses.btnOK_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void ExitPermissionPasses_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ExitPermissionPasses.ExitPermissionPasses_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
