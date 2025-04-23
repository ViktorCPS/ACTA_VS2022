using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;


namespace UI
{
	/// <summary>
	/// Summary description for Readers.
	/// </summary>
	public class Readers : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblTech;
		private System.Windows.Forms.ComboBox cbTech;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.GroupBox gbAntenna0;
		private System.Windows.Forms.Label lbA0lPrimaryLoc;
		private System.Windows.Forms.Label lblA0PrimDirect;
		private System.Windows.Forms.ComboBox cbA0PrimDirect;
		private System.Windows.Forms.Label lblA1PrimLoc;
		private System.Windows.Forms.Label lblA1PrimDir;
		private System.Windows.Forms.ComboBox cbA1PrimDirect;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ListView lvReaders;

		public ReaderTO currentReader;
		private System.Windows.Forms.ComboBox cbA0PrimLoc;
		private System.Windows.Forms.ComboBox cbA1PrimLoc;
		private System.Windows.Forms.Button btnDelete;

		// Controller instance
		public NotificationController Controller;
		// Observer client instance
		public NotificationObserverClient observerClient;

		private CultureInfo culture;
		DebugLog log;
		ResourceManager rm;
		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		private System.Windows.Forms.GroupBox gbAntenna1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnClose;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ListViewItemComparer _comp;

		const int ReaderIDIndex = 0;
		const int DescIndex = 1;
		const int ConnTypeIndex = 2;
		const int AddressIndex = 3;
		const int Ant0LocIndex = 4;
		const int Ant1LocIndex = 5;
        private Button btnMaps;
		const int StatusIndex = 6;

		public Readers()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentReader = new ReaderTO();
			logInUser = NotificationController.GetLogInUser();
			
			this.CenterToScreen();
			
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Readers));
            this.lblTech = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.cbTech = new System.Windows.Forms.ComboBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.gbAntenna0 = new System.Windows.Forms.GroupBox();
            this.cbA0PrimLoc = new System.Windows.Forms.ComboBox();
            this.lblA0PrimDirect = new System.Windows.Forms.Label();
            this.lbA0lPrimaryLoc = new System.Windows.Forms.Label();
            this.cbA0PrimDirect = new System.Windows.Forms.ComboBox();
            this.gbAntenna1 = new System.Windows.Forms.GroupBox();
            this.cbA1PrimDirect = new System.Windows.Forms.ComboBox();
            this.cbA1PrimLoc = new System.Windows.Forms.ComboBox();
            this.lblA1PrimDir = new System.Windows.Forms.Label();
            this.lblA1PrimLoc = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lvReaders = new System.Windows.Forms.ListView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMaps = new System.Windows.Forms.Button();
            this.gbAntenna0.SuspendLayout();
            this.gbAntenna1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTech
            // 
            this.lblTech.Location = new System.Drawing.Point(32, 32);
            this.lblTech.Name = "lblTech";
            this.lblTech.Size = new System.Drawing.Size(100, 23);
            this.lblTech.TabIndex = 1;
            this.lblTech.Text = "Technology:";
            this.lblTech.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(152, 64);
            this.tbDesc.MaxLength = 45;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(152, 20);
            this.tbDesc.TabIndex = 4;
            // 
            // cbTech
            // 
            this.cbTech.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTech.Location = new System.Drawing.Point(152, 32);
            this.cbTech.Name = "cbTech";
            this.cbTech.Size = new System.Drawing.Size(152, 21);
            this.cbTech.TabIndex = 2;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(32, 64);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(100, 23);
            this.lblDesc.TabIndex = 3;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbAntenna0
            // 
            this.gbAntenna0.Controls.Add(this.cbA0PrimLoc);
            this.gbAntenna0.Controls.Add(this.lblA0PrimDirect);
            this.gbAntenna0.Controls.Add(this.lbA0lPrimaryLoc);
            this.gbAntenna0.Controls.Add(this.cbA0PrimDirect);
            this.gbAntenna0.Location = new System.Drawing.Point(16, 104);
            this.gbAntenna0.Name = "gbAntenna0";
            this.gbAntenna0.Size = new System.Drawing.Size(304, 96);
            this.gbAntenna0.TabIndex = 5;
            this.gbAntenna0.TabStop = false;
            this.gbAntenna0.Text = "Antenna 0";
            // 
            // cbA0PrimLoc
            // 
            this.cbA0PrimLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbA0PrimLoc.Location = new System.Drawing.Point(128, 24);
            this.cbA0PrimLoc.Name = "cbA0PrimLoc";
            this.cbA0PrimLoc.Size = new System.Drawing.Size(160, 21);
            this.cbA0PrimLoc.TabIndex = 7;
            // 
            // lblA0PrimDirect
            // 
            this.lblA0PrimDirect.Location = new System.Drawing.Point(12, 56);
            this.lblA0PrimDirect.Name = "lblA0PrimDirect";
            this.lblA0PrimDirect.Size = new System.Drawing.Size(100, 23);
            this.lblA0PrimDirect.TabIndex = 8;
            this.lblA0PrimDirect.Text = "Direction:";
            this.lblA0PrimDirect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbA0lPrimaryLoc
            // 
            this.lbA0lPrimaryLoc.Location = new System.Drawing.Point(16, 24);
            this.lbA0lPrimaryLoc.Name = "lbA0lPrimaryLoc";
            this.lbA0lPrimaryLoc.Size = new System.Drawing.Size(96, 23);
            this.lbA0lPrimaryLoc.TabIndex = 6;
            this.lbA0lPrimaryLoc.Text = "Primary Location:";
            this.lbA0lPrimaryLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA0PrimDirect
            // 
            this.cbA0PrimDirect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbA0PrimDirect.Location = new System.Drawing.Point(128, 56);
            this.cbA0PrimDirect.Name = "cbA0PrimDirect";
            this.cbA0PrimDirect.Size = new System.Drawing.Size(160, 21);
            this.cbA0PrimDirect.TabIndex = 9;
            this.cbA0PrimDirect.SelectedIndexChanged += new System.EventHandler(this.cbA0PrimDirect_SelectedIndexChanged);
            // 
            // gbAntenna1
            // 
            this.gbAntenna1.Controls.Add(this.cbA1PrimDirect);
            this.gbAntenna1.Controls.Add(this.cbA1PrimLoc);
            this.gbAntenna1.Controls.Add(this.lblA1PrimDir);
            this.gbAntenna1.Controls.Add(this.lblA1PrimLoc);
            this.gbAntenna1.Location = new System.Drawing.Point(328, 104);
            this.gbAntenna1.Name = "gbAntenna1";
            this.gbAntenna1.Size = new System.Drawing.Size(304, 96);
            this.gbAntenna1.TabIndex = 10;
            this.gbAntenna1.TabStop = false;
            this.gbAntenna1.Text = "Antenna 1";
            // 
            // cbA1PrimDirect
            // 
            this.cbA1PrimDirect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbA1PrimDirect.Location = new System.Drawing.Point(128, 56);
            this.cbA1PrimDirect.Name = "cbA1PrimDirect";
            this.cbA1PrimDirect.Size = new System.Drawing.Size(160, 21);
            this.cbA1PrimDirect.TabIndex = 14;
            this.cbA1PrimDirect.SelectedIndexChanged += new System.EventHandler(this.cbA1PrimDirect_SelectedIndexChanged);
            // 
            // cbA1PrimLoc
            // 
            this.cbA1PrimLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbA1PrimLoc.Location = new System.Drawing.Point(128, 24);
            this.cbA1PrimLoc.Name = "cbA1PrimLoc";
            this.cbA1PrimLoc.Size = new System.Drawing.Size(160, 21);
            this.cbA1PrimLoc.TabIndex = 12;
            // 
            // lblA1PrimDir
            // 
            this.lblA1PrimDir.Location = new System.Drawing.Point(12, 56);
            this.lblA1PrimDir.Name = "lblA1PrimDir";
            this.lblA1PrimDir.Size = new System.Drawing.Size(100, 23);
            this.lblA1PrimDir.TabIndex = 13;
            this.lblA1PrimDir.Text = "Direction:";
            this.lblA1PrimDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblA1PrimLoc
            // 
            this.lblA1PrimLoc.Location = new System.Drawing.Point(16, 24);
            this.lblA1PrimLoc.Name = "lblA1PrimLoc";
            this.lblA1PrimLoc.Size = new System.Drawing.Size(96, 23);
            this.lblA1PrimLoc.TabIndex = 11;
            this.lblA1PrimLoc.Text = "Primary Location:";
            this.lblA1PrimLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(557, 208);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lvReaders
            // 
            this.lvReaders.FullRowSelect = true;
            this.lvReaders.GridLines = true;
            this.lvReaders.HideSelection = false;
            this.lvReaders.Location = new System.Drawing.Point(16, 280);
            this.lvReaders.Name = "lvReaders";
            this.lvReaders.Size = new System.Drawing.Size(646, 224);
            this.lvReaders.TabIndex = 16;
            this.lvReaders.UseCompatibleStateImageBehavior = false;
            this.lvReaders.View = System.Windows.Forms.View.Details;
            this.lvReaders.SelectedIndexChanged += new System.EventHandler(this.lvReaders_SelectedIndexChanged);
            this.lvReaders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvReaders_ColumnClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(208, 520);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.gbAntenna0);
            this.groupBox1.Controls.Add(this.gbAntenna1);
            this.groupBox1.Controls.Add(this.cbTech);
            this.groupBox1.Controls.Add(this.lblDesc);
            this.groupBox1.Controls.Add(this.tbDesc);
            this.groupBox1.Controls.Add(this.lblTech);
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(646, 248);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Readers";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 520);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 17;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(112, 520);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 18;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click_1);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(592, 520);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMaps
            // 
            this.btnMaps.Image = ((System.Drawing.Image)(resources.GetObject("btnMaps.Image")));
            this.btnMaps.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMaps.Location = new System.Drawing.Point(400, 520);
            this.btnMaps.Name = "btnMaps";
            this.btnMaps.Size = new System.Drawing.Size(75, 23);
            this.btnMaps.TabIndex = 21;
            this.btnMaps.Text = "Maps";
            this.btnMaps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMaps.Click += new System.EventHandler(this.btnMaps_Click);
            // 
            // Readers
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(672, 558);
            this.ControlBox = false;
            this.Controls.Add(this.btnMaps);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lvReaders);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(680, 592);
            this.MinimumSize = new System.Drawing.Size(680, 592);
            this.Name = "Readers";
            this.ShowInTaskbar = false;
            this.Text = "Readers";
            this.Load += new System.EventHandler(this.Readers_Load);
            this.Closed += new System.EventHandler(this.Readers_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Readers_KeyUp);
            this.gbAntenna0.ResumeLayout(false);
            this.gbAntenna1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
					case Readers.ReaderIDIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()), Int32.Parse(sub2.Text.Trim()));
					}
					case Readers.DescIndex:
					case Readers.ConnTypeIndex:
					case Readers.AddressIndex:
					case Readers.Ant0LocIndex:
					case Readers.Ant1LocIndex:
					case Readers.StatusIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					default:
						throw new IndexOutOfRangeException("Unrecognized column name extension");

				}
			}
		}

		#endregion

		private void populateLocationCb(ComboBox cb)
		{
			try
			{
				Location location = new Location();
                location.LocTO.Status = Constants.DefaultStateActive.Trim();
				List<LocationTO> locations = location.Search();

				locations.Insert(0, new LocationTO(-1, rm.GetString("all", culture), "", -1, -1, Constants.DefaultStateActive.ToString()));

				cb.DataSource = locations;
				cb.DisplayMember = "Name";
				cb.ValueMember = "LocationID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.populateLocationCb(...): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}

		private void Readers_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvReaders);
                lvReaders.ListViewItemSorter = _comp;
                lvReaders.Sorting = SortOrder.Ascending;

                InitialiseObserverClient();
                rm = new ResourceManager("UI.Resource", typeof(Readers).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                populateLocationCb(cbA0PrimLoc);
                populateLocationCb(cbA1PrimLoc);
                populateTechCb();
                populateDirectionCb(cbA0PrimDirect);
                populateDirectionCb(cbA1PrimDirect);
                setLanguage();

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                List<ReaderTO> readerList = new Reader().Search();
                populateReaderListView(readerList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.Readers_Load(...): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
			
		}

		private void populateTechCb()
		{
			try
			{
				this.cbTech.Items.AddRange(new object[] {
															Constants.TechTypeHitag1,
															Constants.TechTypeHitag2,
															Constants.TechTypeMifare,
															Constants.TechTypeICode
														});

				this.cbTech.Items.Insert(0, rm.GetString("all", culture));
				this.cbTech.SelectedIndex = 0;			
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.populateTechCb(): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}

		private void populateDirectionCb(ComboBox cb)
		{
			try
			{
				cb.Items.Add(rm.GetString("all", culture));
				cb.Items.Add(Constants.DirectionIn);
				cb.Items.Add(Constants.DirectionOut);
				//cb.Items.Add(Constants.DirectionInOut);

				cb.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.populateDirectionCb(ComboBox cb): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			//string description = "";
			//string connType = "";
			//string a0LocID = "";
			//string a0Direction = "";
			//string a0SecLocID = "";
			//string a0SecDir = "";
			//string a0IsCounter = "";
			//string a1LocID = "";
			//string a1Direction = "";
			//string a1SecLocID = "";
			//string a1SecDir = "";
			//string a1IsCounter = "";
			//string techType = "";
            this.Cursor = Cursors.WaitCursor;

			currentReader = new ReaderTO();

            try
            {
                currentReader.Description = tbDesc.Text.Trim();
                if ((int)cbA0PrimLoc.SelectedValue != -1)
                {
                    currentReader.A0LocID = (int)cbA0PrimLoc.SelectedValue;
                }

                if (!cbA0PrimDirect.SelectedItem.ToString().Trim().Equals(rm.GetString("all", culture)))
                {
                    currentReader.A0Direction = cbA0PrimDirect.SelectedItem.ToString().Trim();
                }

                if ((int)cbA1PrimLoc.SelectedValue != -1)
                {
                    currentReader.A1LocID = (int)cbA1PrimLoc.SelectedValue;
                }

                if (!cbA1PrimDirect.SelectedItem.ToString().Trim().Equals(rm.GetString("all", culture)))
                {
                    currentReader.A1Direction = cbA1PrimDirect.SelectedItem.ToString();
                }

                if (!cbTech.SelectedItem.ToString().Trim().Equals(rm.GetString("all", culture)))
                {
                    currentReader.TechType = cbTech.SelectedItem.ToString();
                }

                Reader r = new Reader();
                r.RdrTO = currentReader;
                List<ReaderTO> readerList = r.Search();

                if (readerList.Count > 0)
                {
                    populateReaderListView(readerList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noReadersFound", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.btnSearch_Click(...): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}
		
		private void populateReaderListView(List<ReaderTO> readerList)
		{
			try
			{
				lvReaders.BeginUpdate();
				lvReaders.Items.Clear();

				lvReaders.Columns[1].TextAlign = HorizontalAlignment.Left;
				lvReaders.Columns[2].TextAlign = HorizontalAlignment.Left;
				lvReaders.Columns[3].TextAlign = HorizontalAlignment.Left;
				lvReaders.Columns[4].TextAlign = HorizontalAlignment.Left;
				lvReaders.Columns[5].TextAlign = HorizontalAlignment.Left;
				lvReaders.Columns[6].TextAlign = HorizontalAlignment.Left;

				if(readerList.Count > 0)
				{
					foreach(ReaderTO reader in readerList)
					{
						ListViewItem item = new ListViewItem();

						item.Text = reader.ReaderID.ToString();

						item.SubItems.Add(reader.Description);
						item.SubItems.Add(reader.ConnectionType);
						item.SubItems.Add(reader.ConnectionAddress);
					
						item.SubItems.Add(new Reader().getLocation(reader.A0LocID).Name.ToString());
						item.SubItems.Add(new Reader().getLocation(reader.A1LocID).Name.ToString());

						item.SubItems.Add(reader.Status);
                        item.Tag = reader;

						lvReaders.Items.Add(item);
					}
				}

				lvReaders.EndUpdate();
				lvReaders.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.populateReaderListView(ArrayList readerList): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}

		// TODO: set language from Resx eng and srt
		private void setLanguage()
		{
			try
			{
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				
				this.Text = rm.GetString("menuReaders", culture);

				groupBox1.Text = rm.GetString("gbReaders", culture);
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
                btnMaps.Text = rm.GetString("btnMaps", culture);
                lblTech.Text = rm.GetString("lblTech", culture);
				lblDesc.Text = rm.GetString("lblDesc", culture);
				lbA0lPrimaryLoc.Text = rm.GetString("lblPrimaryLoc", culture);
				lblA0PrimDirect.Text = rm.GetString("lblPrimDirect", culture);
				lblA1PrimLoc.Text = rm.GetString("lblPrimaryLoc", culture);
				lblA1PrimDir.Text = rm.GetString("lblPrimDirect", culture);
				gbAntenna0.Text = rm.GetString("gbAntenna0", culture);
				gbAntenna1.Text = rm.GetString("gbAntenna1", culture);

				lvReaders.BeginUpdate();

				lvReaders.Columns.Add(rm.GetString("hdrReaderID", culture), 2 * (lvReaders.Width - 6) / 17, HorizontalAlignment.Center);
				lvReaders.Columns.Add(rm.GetString("hdrDescripton", culture), 3 * (lvReaders.Width - 6) / 17, HorizontalAlignment.Center);
				lvReaders.Columns.Add(rm.GetString("hdrConnType", culture), 2 * (lvReaders.Width - 6) / 17, HorizontalAlignment.Center);
				lvReaders.Columns.Add(rm.GetString("hdrConnAddress", culture), 2 * (lvReaders.Width - 6) / 17, HorizontalAlignment.Center);
				lvReaders.Columns.Add(rm.GetString("hdrAntenna0Loc", culture), 3 * (lvReaders.Width - 6) / 17, HorizontalAlignment.Center);
				lvReaders.Columns.Add(rm.GetString("hdrAntenna1Loc", culture), 3 * (lvReaders.Width - 6) / 17, HorizontalAlignment.Center);
				lvReaders.Columns.Add(rm.GetString("hdrStatus", culture), 2 * (lvReaders.Width - 6) / 17, HorizontalAlignment.Center);

				lvReaders.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Reader.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void lvReaders_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvReaders.SelectedItems.Count > 0)
                {
                    currentReader = (ReaderTO)lvReaders.SelectedItems[0].Tag;

                    // Set reader's data
                    tbDesc.Text = currentReader.Description;

                    cbA0PrimLoc.SelectedIndex = cbA0PrimLoc.FindStringExact(lvReaders.SelectedItems[0].SubItems[4].Text);
                    cbA1PrimLoc.SelectedIndex = cbA1PrimLoc.FindStringExact(lvReaders.SelectedItems[0].SubItems[5].Text);

                    cbA0PrimDirect.SelectedIndex = cbA0PrimDirect.FindStringExact(currentReader.A0Direction);
                    cbA1PrimDirect.SelectedIndex = cbA1PrimDirect.FindStringExact(currentReader.A1Direction);

                    cbTech.SelectedIndex = cbTech.FindStringExact(currentReader.TechType);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Reader.lvReaders_SelectedIndexChanged(...): " + ex.Message + "\n");
                MessageBox.Show("Exception: " + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (lvReaders.SelectedItems.Count > 0)
			{
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    DialogResult result = MessageBox.Show(rm.GetString("deleteReaders", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool deleted = true;
                        foreach (ListViewItem item in lvReaders.SelectedItems)
                        {
                            deleted = new Reader().Delete(((ReaderTO)item.Tag).ReaderID) && deleted;
                        }

                        if (deleted)
                        {
                            MessageBox.Show(rm.GetString("readerDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("readerNotDeleted", culture));
                        }

                        clearForm();
                        List<ReaderTO> readerList = new Reader().Search();
                        populateReaderListView(readerList);
                    }
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " Reader.btnDelete_Click(...): " + ex.Message + "\n");
                    MessageBox.Show("Exception: " + ex.Message);
                }
                finally { 
                    this.Cursor = Cursors.Arrow; 
                }
			}
			else
			{
				MessageBox.Show(rm.GetString("messageSelectReader", culture));
			}
		}

		private void cbA0PrimDirect_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cbA0PrimDirect.SelectedIndex == 0)
			{
				currentReader.A0Direction = null;
			}
		}

		private void cbA1PrimDirect_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cbA1PrimDirect.SelectedIndex == 0)
			{
				currentReader.A1Direction = null;
			}
		}

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

		/*private void btnAddvance_Click(object sender, System.EventArgs e)
		{
			ReaderAdvanceSettings ras = new ReaderAdvanceSettings(currentReader);
			ras.ShowDialog();
		}*/

		public void InitialiseObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.SettingsChangeEvent);
		}

		public void SettingsChangeEvent(object sender, NotificationEventArgs e)
        {
            try
            {


                if (e.reader != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                    cbA0PrimLoc.SelectedValue = e.reader.A0LocID;
                    cbA0PrimDirect.SelectedIndex = cbA0PrimDirect.FindStringExact(e.reader.A0Direction);

                    cbA1PrimLoc.SelectedValue = e.reader.A1LocID;
                    cbA1PrimDirect.SelectedIndex = cbA0PrimDirect.FindStringExact(e.reader.A1Direction);

                    clearForm();
                    List<ReaderTO> readerList = new Reader().Search();
                    populateReaderListView(readerList);

                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Readers.SettingsChangeEvent(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void Readers_Closed(object sender, System.EventArgs e)
		{
			Controller.DettachFromNotifier(this.observerClient);
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ReadersAdd addReader = new ReadersAdd(new ReaderTO());
                addReader.ShowDialog(this);

                clearForm();
                List<ReaderTO> readerList = new Reader().Search();
                populateReaderListView(readerList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Readers.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdate_Click_1(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvReaders.SelectedItems.Count == 1)
                {
                    ReadersAdd addReader = new ReadersAdd((ReaderTO)lvReaders.SelectedItems[0].Tag);
                    addReader.ShowDialog(this);

                    clearForm();
                    List<ReaderTO> readerList = new Reader().Search();
                    populateReaderListView(readerList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("messageSelectReader", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Readers.btnUpdate_Click_1(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Readers.btnClose_Click(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " Readers.setVisibility(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void lvReaders_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvReaders.Sorting;
                lvReaders.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvReaders.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvReaders.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvReaders.Sorting = SortOrder.Ascending;
                }
                lvReaders.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Readers.lvReaders_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void clearForm()
		{
			cbTech.SelectedIndex = 0;
			tbDesc.Text = "";

			cbA0PrimDirect.SelectedIndex = 0;
			cbA0PrimLoc.SelectedIndex = 0;

			cbA1PrimDirect.SelectedIndex = 0;
			cbA1PrimLoc.SelectedIndex = 0;
		}

        private void Readers_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Readers.Readers_KeyUp(): " + ex.Message + "\n");
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
                MapObjects mapObjects = new MapObjects(Constants.readerObjectType);
                mapObjects.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Readers.btnMaps_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}

