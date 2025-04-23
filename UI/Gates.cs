using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using Common;
using TransferObjects;

using System.Resources;
using System.Globalization;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for Gates.
	/// </summary>
	public class Gates : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbGates;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ListView lvGates;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		GateTO currentGate = null;

		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		DebugLog log;

		// List View indexes
		//const int GateIDIndex = 0;
		const int NameIndex = 0;
		const int DescriptionIndex = 1;
		const int DownloadStartTimeIndex = 2;
		const int DownloadIntervalIndex = 3;
		const int DownloadEraseCounterIndex = 4;

		private ListViewItemComparer _comp;

		private CultureInfo culture;
        private Button btnMaps;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
		ResourceManager rm;

        private Filter filter;

		public Gates()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentGate = new GateTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(Gates).Assembly);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gates));
            this.gbGates = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lvGates = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMaps = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbGates.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbGates
            // 
            this.gbGates.Controls.Add(this.btnSearch);
            this.gbGates.Controls.Add(this.tbDesc);
            this.gbGates.Controls.Add(this.lblDesc);
            this.gbGates.Controls.Add(this.tbName);
            this.gbGates.Controls.Add(this.lblName);
            this.gbGates.Location = new System.Drawing.Point(16, 16);
            this.gbGates.Name = "gbGates";
            this.gbGates.Size = new System.Drawing.Size(376, 128);
            this.gbGates.TabIndex = 0;
            this.gbGates.TabStop = false;
            this.gbGates.Tag = "FILTERABLE";
            this.gbGates.Text = "Gates";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(224, 88);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(96, 56);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(200, 20);
            this.tbDesc.TabIndex = 3;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(16, 56);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(64, 23);
            this.lblDesc.TabIndex = 2;
            this.lblDesc.Text = "Description";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(96, 24);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(200, 20);
            this.tbName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(8, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(72, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvGates
            // 
            this.lvGates.FullRowSelect = true;
            this.lvGates.GridLines = true;
            this.lvGates.HideSelection = false;
            this.lvGates.Location = new System.Drawing.Point(16, 160);
            this.lvGates.Name = "lvGates";
            this.lvGates.ShowItemToolTips = true;
            this.lvGates.Size = new System.Drawing.Size(552, 168);
            this.lvGates.TabIndex = 5;
            this.lvGates.UseCompatibleStateImageBehavior = false;
            this.lvGates.View = System.Windows.Forms.View.Details;
            this.lvGates.SelectedIndexChanged += new System.EventHandler(this.lvGates_SelectedIndexChanged);
            this.lvGates.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGates_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 344);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(104, 344);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(192, 344);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(496, 344);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMaps
            // 
            this.btnMaps.Image = ((System.Drawing.Image)(resources.GetObject("btnMaps.Image")));
            this.btnMaps.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMaps.Location = new System.Drawing.Point(344, 344);
            this.btnMaps.Name = "btnMaps";
            this.btnMaps.Size = new System.Drawing.Size(75, 23);
            this.btnMaps.TabIndex = 10;
            this.btnMaps.Text = "Maps";
            this.btnMaps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMaps.Click += new System.EventHandler(this.btnMaps_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(398, 16);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 100);
            this.gbFilter.TabIndex = 25;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // Gates
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(584, 382);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnMaps);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvGates);
            this.Controls.Add(this.gbGates);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(592, 416);
            this.MinimumSize = new System.Drawing.Size(592, 416);
            this.Name = "Gates";
            this.ShowInTaskbar = false;
            this.Text = "Gates";
            this.Load += new System.EventHandler(this.Gates_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Gates_KeyUp);
            this.gbGates.ResumeLayout(false);
            this.gbGates.PerformLayout();
            this.gbFilter.ResumeLayout(false);
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
					case Gates.NameIndex:
					case Gates.DescriptionIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case Gates.DownloadStartTimeIndex:
					{
						DateTime dt1 = new DateTime(1,1,1,0,0,0);
						DateTime dt2 = new DateTime(1,1,1,0,0,0);

						if (!sub1.Text.Trim().Equals("")) 
						{
                            dt1 = DateTime.ParseExact(sub1.Text.Trim(), "HH:mm", null);
						}

						if (!sub2.Text.Trim().Equals(""))
						{
                            dt2 = DateTime.ParseExact(sub2.Text.Trim(), "HH:mm", null);
						}
						
						return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
					}
					case Gates.DownloadIntervalIndex:
					case Gates.DownloadEraseCounterIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()), Int32.Parse(sub2.Text.Trim()));
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
				this.Text = rm.GetString("gateForm", culture);

				// group box text
				gbGates.Text = rm.GetString("gbGate", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
				
				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
                btnMaps.Text = rm.GetString("btnMaps", culture);

				// label's text
				lblName.Text = rm.GetString("lblName", culture);
				lblDesc.Text = rm.GetString("lblDescription", culture);

				// list view
				lvGates.BeginUpdate();
				lvGates.Columns.Add(rm.GetString("hdrName", culture), (lvGates.Width - 5)/ 5, HorizontalAlignment.Left);
				lvGates.Columns.Add(rm.GetString("hdrDescription", culture), (lvGates.Width - 5) / 5, HorizontalAlignment.Left);
				lvGates.Columns.Add(rm.GetString("hdrDownloadStartTime", culture), (lvGates.Width - 5) / 5, HorizontalAlignment.Left);
				lvGates.Columns.Add(rm.GetString("hdrDownloadInterval", culture), (lvGates.Width - 5) / 5, HorizontalAlignment.Left);
				lvGates.Columns.Add(rm.GetString("hdrDownloadEraseCounter", culture), (lvGates.Width - 5) / 5, HorizontalAlignment.Left);
				lvGates.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with Gates found
		/// </summary>
		/// <param name="gatesList"></param>
		public void populateListView(List<GateTO> gatesList)
		{
			try
			{
				lvGates.BeginUpdate();
				lvGates.Items.Clear();

				if (gatesList.Count > 0)
				{
					foreach(GateTO gate in gatesList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = gate.Name.ToString().Trim();
						item.SubItems.Add(gate.Description.ToString().Trim());
						item.SubItems.Add(gate.DownloadStartTime.ToString("HH:mm").Trim());
						item.SubItems.Add(gate.DownloadInterval.ToString().Trim());
						item.SubItems.Add(gate.DownloadEraseCounter.ToString().Trim());
						item.Tag = gate;
                        item.ToolTipText = "Gate ID: " + gate.GateID.ToString().Trim();

						lvGates.Items.Add(item);
					}
				}

				lvGates.EndUpdate();
				lvGates.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Prepare search strings.
		/// </summary>
		/// <param name="forParsing"></param>
		/// <returns></returns>
        //private string parse(string forParsing)
        //{
        //    string parsedString = forParsing.Trim();
        //    if (parsedString.StartsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(1);
        //        parsedString = "%" + parsedString;
        //    }

        //    if (parsedString.EndsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(0, parsedString.Length - 1);
        //        parsedString = parsedString + "%";
        //    }

        //    return parsedString;
        //}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Gates.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

                Gate gate = new Gate();
                gate.GTO.Name = tbName.Text.Trim();
                gate.GTO.Description = tbDesc.Text.Trim();
				List<GateTO> gateList = gate.Search();

				if (gateList.Count > 0)
				{
					populateListView(gateList);
				}
				else
				{
					MessageBox.Show(rm.GetString("noGatesFound", culture));
				}

				currentGate = new GateTO();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.btnSearch_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void Gates_Load(object sender, System.EventArgs e)
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

                List<GateTO> gateList = new Gate().Search();
                populateListView(gateList);

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Gates.Gates_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
				log.writeLog(DateTime.Now + " Gates.lvGates_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvGates_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
                
                if(lvGates.SelectedItems.Count == 1)
				{
					currentGate = (GateTO)lvGates.SelectedItems[0].Tag;

					tbName.Text = currentGate.Name.ToString();
					tbDesc.Text = currentGate.Description.ToString();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.lvGates_SelectedIndexChanged(): " + ex.Message + "\n");
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
				GatesAdd addForm = new GatesAdd();
				addForm.ShowDialog(this);

				List<GateTO> gateList = new Gate().Search();
				populateListView(gateList);
				tbName.Text = "";
				tbDesc.Text = "";
				currentGate = new GateTO();
				this.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.btnAdd_Click(): " + ex.Message + "\n");
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
					currentGate = (GateTO)this.lvGates.SelectedItems[0].Tag;

					// Open Update Form
					GatesAdd addForm = new GatesAdd(currentGate);
					addForm.ShowDialog(this);

					List<GateTO> gateList = new Gate().Search();
					populateListView(gateList);
					tbName.Text = "";
					tbDesc.Text = "";
					currentGate = new GateTO();
					this.Invalidate();
				}

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.btnUpdate_Click(): " + ex.Message + "\n");
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

				if (lvGates.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("noSelGateDel", culture));
				}
				else
				{
					DialogResult result = MessageBox.Show(rm.GetString("deleteGates", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;
						int selected = lvGates.SelectedItems.Count;
						foreach(ListViewItem item in lvGates.SelectedItems)
						{
							if (((GateTO)item.Tag).GateID == 0)
							{
								MessageBox.Show(rm.GetString("defaultGate", culture));
								selected--;
							}
							else
							{
								// TODO: Check if there are Readers on this Gate!!!!! 
								List<ReaderTO> readerList = new Reader().Search(((GateTO)item.Tag).GateID.ToString().Trim());

								if (readerList.Count > 0)
								{
									MessageBox.Show(rm.GetString("readersOnGate", culture));
									selected--;
								}
								else
								{
									// Find if exists Gate <-> Access group xref record
									ArrayList xrefArray = (new AccessGroupXGate()).Search("", ((GateTO)item.Tag).GateID.ToString());
									if (xrefArray.Count > 0)
									{
										MessageBox.Show(item.Text + ": " + rm.GetString("gateHasAccessGroupXref", culture));
										selected--;
									}
									else
									{
										isDeleted = new Gate().Delete(((GateTO)item.Tag).GateID) && isDeleted;
									}									
								}
							}
						}

						if ((selected > 0) && isDeleted)
						{
							MessageBox.Show(rm.GetString("gatesDel", culture));
						}
						else if (!isDeleted)
						{
							MessageBox.Show(rm.GetString("noGateDel", culture));
						}

						List<GateTO> gateList = new Gate().Search();
						populateListView(gateList);
						tbName.Text = "";
						tbDesc.Text = "";
						currentGate = new GateTO();
						this.Invalidate();
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.btnDelete_Click(): " + ex.Message + "\n");
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
					permission = (((int[]) menuItemsPermissions[menuItemID])[role.ApplRoleID]);
					readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
					addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
					updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
					deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
				}

				btnSearch.Enabled = readPermission;
				btnAdd.Enabled = addPermission;
				btnUpdate.Enabled = updatePermission;
				btnDelete.Enabled = deletePermission;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void Gates_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Gates.Gates_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnMaps_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                MapObjects mapObjects = new MapObjects(Constants.gateObjectType);
                mapObjects.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Gates.btnMaps_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Gates.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Gates.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
