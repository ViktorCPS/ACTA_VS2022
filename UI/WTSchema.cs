using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Working Time Schema search and view form
	/// </summary>
	public class WTSchema : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblLength;
		private System.Windows.Forms.NumericUpDown numDays;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Label lblDays;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox gbSchema;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CultureInfo culture;
		private ResourceManager rm;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbName;

		private WorkTimeSchemaTO currentTimeSchema;

		// Observer
		private NotificationController Controller;
		private NotificationObserverClient observerClient;

		// Debug log
		DebugLog log;
		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		// List View Indexes
		const int SchemaName = 0;
		const int Desc = 1;
		const int Type = 2;
		const int Duration = 3;

		private ListViewItemComparer _comp;

		public WTSchema()
		{
			InitializeComponent();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			InitialiseObserverClient();
			this.CenterToScreen();

			currentTimeSchema = new WorkTimeSchemaTO();
			logInUser = NotificationController.GetLogInUser();

			// Set Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();

			//tbName.Text = "*";
			//tbDesc.Text = "*";

			// Initialize comparer object
			_comp = new ListViewItemComparer(listView);
			listView.ListViewItemSorter = _comp;
			listView.Sorting = SortOrder.Ascending; 
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
            this.lblLength = new System.Windows.Forms.Label();
            this.numDays = new System.Windows.Forms.NumericUpDown();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblDays = new System.Windows.Forms.Label();
            this.gbSchema = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).BeginInit();
            this.gbSchema.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLength
            // 
            this.lblLength.Location = new System.Drawing.Point(40, 104);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(48, 23);
            this.lblLength.TabIndex = 11;
            this.lblLength.Text = "Length:";
            this.lblLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numDays
            // 
            this.numDays.Location = new System.Drawing.Point(96, 104);
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(48, 20);
            this.numDays.TabIndex = 10;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(96, 72);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(216, 20);
            this.tbDesc.TabIndex = 9;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(24, 72);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(64, 23);
            this.lblDesc.TabIndex = 8;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDays
            // 
            this.lblDays.Location = new System.Drawing.Point(160, 104);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(32, 23);
            this.lblDays.TabIndex = 12;
            this.lblDays.Text = "Days";
            this.lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbSchema
            // 
            this.gbSchema.Controls.Add(this.btnSearch);
            this.gbSchema.Location = new System.Drawing.Point(16, 16);
            this.gbSchema.Name = "gbSchema";
            this.gbSchema.Size = new System.Drawing.Size(472, 128);
            this.gbSchema.TabIndex = 13;
            this.gbSchema.TabStop = false;
            this.gbSchema.Text = "Work Time Schema";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(384, 88);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // listView
            // 
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(16, 168);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(472, 184);
            this.listView.TabIndex = 14;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 368);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(112, 368);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 16;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(208, 368);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(416, 368);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(48, 40);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(40, 23);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(96, 40);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(216, 20);
            this.tbName.TabIndex = 7;
            // 
            // WTSchema
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(504, 421);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.lblDays);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.numDays);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.gbSchema);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(512, 429);
            this.MinimumSize = new System.Drawing.Size(512, 429);
            this.Name = "WTSchema";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.WTSchema_Load);
            this.Closed += new System.EventHandler(this.WTSchema_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WTSchema_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).EndInit();
            this.gbSchema.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
					case WTSchema.Duration:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()), Int32.Parse(sub2.Text.Trim()));
					}
					case WTSchema.Desc:
					case WTSchema.Type:
					case WTSchema.SchemaName:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					default:
					{
						return(0);
					}

				}
			}

		}

		#endregion

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				WTSchemaAdd wtschAdd = new WTSchemaAdd();
				wtschAdd.ShowDialog(this);

				tbName.Text = "";
				tbDesc.Text = "";
				numDays.Value = 0;
				List<WorkTimeSchemaTO> schemas = new TimeSchema().Search();
                populateListView(getSchemas(schemas));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchema.btnAdd_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " WTSchema.btnClose_Click(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " WTSchemaAdd: Enter UpdateClick\n");
				
				if (listView.SelectedItems.Count == 1)
				{
					log.writeLog(DateTime.Now + " WTSchemaAdd: Item selected\n");
			
					if (currentTimeSchema != null)
					{
						log.writeLog(DateTime.Now + " WTSchemaAdd: Time Schema selected\n");
						WTSchemaAdd wtschAdd = new WTSchemaAdd(currentTimeSchema);
						log.writeLog(DateTime.Now + " WTSchemaAdd: Form initialised\n");
						wtschAdd.ShowDialog(this);
						log.writeLog(DateTime.Now + " WTSchemaAdd: Form closed\n");

						tbName.Text = "";
						tbDesc.Text = "";
						numDays.Value = 0;
						List<WorkTimeSchemaTO> schemas = new TimeSchema().Search();
                        populateListView(getSchemas(schemas));
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("msgSelectSchema", culture));
					return;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchema.btnUpdate_Click(): " + ex.Message + "\n");
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
				int selected = listView.SelectedItems.Count;
				if (listView.SelectedItems.Count > 0)
				{
					DialogResult result1 = MessageBox.Show(rm.GetString("msgRemoveSchema", culture), "", MessageBoxButtons.YesNo);
					if (result1 == DialogResult.Yes)
					{
						bool isDeleted = true;
						foreach (ListViewItem item in listView.SelectedItems)
						{
							if (((WorkTimeSchemaTO)item.Tag).TimeSchemaID == 0)
							{
								MessageBox.Show(rm.GetString("msgDefaultSchemaDel", culture));
								selected--;
							}
                            else if (new TimeSchema().SchemaIsUsed(((WorkTimeSchemaTO)item.Tag).TimeSchemaID))
							{
								MessageBox.Show(rm.GetString("msgSchemaIsUsed", culture));
								selected--;
							}
							else
							{
                                TimeSchema sch = new TimeSchema();
                                sch.TimeSchemaTO = (WorkTimeSchemaTO)item.Tag;
                                sch.TimeSchemaTO.Status = Constants.statusRetired;
                                isDeleted = sch.Update() && isDeleted;
							}
						}

						if (selected > 0 && isDeleted)
						{
							MessageBox.Show(rm.GetString("schemaDeleted", culture));
						}
						else if (!isDeleted)
						{
							MessageBox.Show(rm.GetString("schemaNotDeleted", culture));
						}

						tbName.Text = "";
						tbDesc.Text = "";
						numDays.Value = 0;
						List<WorkTimeSchemaTO> schemas = new TimeSchema().Search();
                        populateListView(getSchemas(schemas));
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("msgSelectSchema", culture));
					return;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchema.btnDelete_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("WTSchemaTitle", culture);

				gbSchema.Text = rm.GetString("WTSchemaGroup", culture);
				lblName.Text = rm.GetString("lblName1", culture);
				lblDesc.Text = rm.GetString("lblDesc", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
				lblLength.Text = rm.GetString("lblLength", culture);
				lblDays.Text = rm.GetString("lblDays", culture);

				listView.BeginUpdate();

				listView.Columns.Add(rm.GetString("lblSchema", culture), (listView.Width / 4)-1, HorizontalAlignment.Center);
				listView.Columns.Add(rm.GetString("hdrDescripton", culture), (listView.Width / 4)-1, HorizontalAlignment.Center);
				listView.Columns.Add(rm.GetString("lblType", culture), (listView.Width / 4)-1, HorizontalAlignment.Center);
				listView.Columns.Add(rm.GetString("lblDuration", culture), (listView.Width / 4)-1, HorizontalAlignment.Center);

				listView.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchema.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
			currentTimeSchema = new WorkTimeSchemaTO();
			List<WorkTimeSchemaTO> schemas = new List<WorkTimeSchemaTO>(); 
			
			int cycleDay = Int32.Parse(numDays.Value.ToString());

            try
            {
                TimeSchema sch = new TimeSchema();
                sch.TimeSchemaTO.Name = tbName.Text.Trim();
                sch.TimeSchemaTO.Description = tbDesc.Text.Trim();
                if (cycleDay > 0)
                    sch.TimeSchemaTO.CycleDuration = cycleDay;
                schemas = sch.Search();
                if (schemas.Count > 0)
                {
                    populateListView(getSchemas(schemas));
                }
                else
                {
                    MessageBox.Show(rm.GetString("noSchemaFound", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchema.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
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

		private void populateListView(List<WorkTimeSchemaTO> schemaArray)
		{
			try
			{
				listView.BeginUpdate();
				listView.Items.Clear();

				foreach(WorkTimeSchemaTO schema in schemaArray)
				{
                    if (schema.Status != Constants.statusRetired)
                    {
                        ListViewItem item = new ListViewItem();

                        item.Text = schema.Name;
                        item.SubItems.Add(schema.Description.ToString());
                        item.SubItems.Add(schema.Type.ToString());
                        item.SubItems.Add(schema.CycleDuration.ToString());

                        item.Tag = schema;

                        listView.Items.Add(item);
                    }
				}

				listView.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchema.populateListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                log.writeLog(DateTime.Now + " Selected items: " + listView.SelectedItems.Count.ToString().Trim() + "\n");
                if (this.listView.SelectedItems.Count > 0)
                {
                    this.tbName.Text = this.listView.SelectedItems[0].Text;
                    this.tbDesc.Text = this.listView.SelectedItems[0].SubItems[1].Text;
                    this.numDays.Value = Int32.Parse(this.listView.SelectedItems[0].SubItems[3].Text);

                    // Fill current TimeSchema object with selected data
                    currentTimeSchema = new WorkTimeSchemaTO();
                    currentTimeSchema = (WorkTimeSchemaTO)listView.SelectedItems[0].Tag;
                    log.writeLog(DateTime.Now + " SchemaID: " + currentTimeSchema.TimeSchemaID.ToString().Trim() + "\n");
                    log.writeLog(DateTime.Now + " SchemaName: " + currentTimeSchema.Name.Trim() + "\n");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchema.listView_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void InitialiseObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.SchemaChangedEvent);
		}

		public void SchemaChangedEvent(object sender, NotificationEventArgs args)
		{
			try
			{
				if (args.schema != null)
				{
					//List<WorkTimeSchemaTO> schemas = args.schema.Search("", "", "", "", "");
                    List<WorkTimeSchemaTO> schemas = new TimeSchema().Search();
                    populateListView(getSchemas(schemas));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchema.SchemaChangedEvent(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void WTSchema_Closed(object sender, System.EventArgs e)
		{
			Controller.DettachFromNotifier(this.observerClient);
		}

		private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = listView.Sorting;
                listView.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        listView.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        listView.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    listView.Sorting = SortOrder.Ascending;
                }
                listView.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchema.listView_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void WTSchema_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                List<WorkTimeSchemaTO> schemas = new TimeSchema().Search();
                populateListView(getSchemas(schemas));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchema.WTSchema_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
				log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private List<WorkTimeSchemaTO> getSchemas(List<WorkTimeSchemaTO> schemas)
        {
            try
            {
                List<WorkTimeSchemaTO> schemasList = new List<WorkTimeSchemaTO>();
                int permission;

                string timeTableMenuItemID = rm.GetString("menuConfiguration", culture) + "_"
                    + rm.GetString("menuItemTimeTable", culture);

                bool readPermission = false;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[timeTableMenuItemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
                }

                if (readPermission)
                {
                    foreach (WorkTimeSchemaTO schTO in schemas)
                    {
                        schemasList.Add(schTO);
                    }
                }
                else
                {
                    foreach (WorkTimeSchemaTO schema in schemas)
                    {
                        if (!schema.Type.Equals(Constants.schemaTypeIndustrial))
                        {
                            schemasList.Add(schema);
                        }
                    }
                }

                return schemasList;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.getSchemas(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void WTSchema_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " WTSchema.WTSchema_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
