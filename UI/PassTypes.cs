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

namespace UI
{
	/// <summary>
	/// Summary description for PassTypes.
	/// </summary>
	public class PassTypes : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.Label lblButton;
		private System.Windows.Forms.TextBox tbButton;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnSearch;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		PassTypeTO currentPassType = null;

		// List View indexes
		//const int PassTypeIDIndex = 0;
		const int DescriptionIndex = 0;
		const int ButtonIndex = 1;
		const int IsPassIndex = 2;
		const int PayCodeIndex = 3;

		private ListViewItemComparer _comp;
		private System.Windows.Forms.ListView lvPassTypes;

		private CultureInfo culture;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnAdd;

		DebugLog log;

		// Controller instance
		public NotificationController Controller;
		private System.Windows.Forms.Button btnCancel;
		// Observer client instance
		public NotificationObserverClient observerClient;
		private System.Windows.Forms.ComboBox cbPassType;
		private System.Windows.Forms.Label lblPassType;
		ResourceManager rm;
		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		private System.Windows.Forms.Label lblPayCode;
		private System.Windows.Forms.TextBox tbPayCode;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
		bool deletePermission = false;

        Filter filter;

		// Possible values of Pass Types
		public struct PassTypeValue
		{
			public string typeName;
			public int typeValue;

			public string TypeName
			{
				get{ return typeName; }
				set{ typeName = value; }
			}

			public int TypeValue
			{
				get{ return typeValue; }
				set{ typeValue = value; }
			}

			public PassTypeValue(string typeName, int typeValue)
			{
				this.typeName = typeName;
				this.typeValue = typeValue;
				this.TypeName = typeName;
				this.TypeValue = typeValue;
			}
		}

		public PassTypes()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			InitializeObserverClient();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new 
				ResourceManager("UI.Resource",typeof(PassTypes).Assembly);
			setLanguage();
			currentPassType = new PassTypeTO();
			logInUser = NotificationController.GetLogInUser();
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblButton = new System.Windows.Forms.Label();
            this.tbButton = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lvPassTypes = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPayCode = new System.Windows.Forms.TextBox();
            this.lblPayCode = new System.Windows.Forms.Label();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(40, 24);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(64, 23);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(112, 24);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(288, 20);
            this.tbDescription.TabIndex = 1;
            // 
            // lblButton
            // 
            this.lblButton.Location = new System.Drawing.Point(48, 56);
            this.lblButton.Name = "lblButton";
            this.lblButton.Size = new System.Drawing.Size(56, 23);
            this.lblButton.TabIndex = 2;
            this.lblButton.Text = "Button:";
            this.lblButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbButton
            // 
            this.tbButton.Location = new System.Drawing.Point(112, 56);
            this.tbButton.Name = "tbButton";
            this.tbButton.Size = new System.Drawing.Size(40, 20);
            this.tbButton.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 448);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(112, 448);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(208, 448);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(328, 120);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lvPassTypes
            // 
            this.lvPassTypes.FullRowSelect = true;
            this.lvPassTypes.GridLines = true;
            this.lvPassTypes.HideSelection = false;
            this.lvPassTypes.Location = new System.Drawing.Point(16, 184);
            this.lvPassTypes.Name = "lvPassTypes";
            this.lvPassTypes.Size = new System.Drawing.Size(564, 248);
            this.lvPassTypes.TabIndex = 9;
            this.lvPassTypes.UseCompatibleStateImageBehavior = false;
            this.lvPassTypes.View = System.Windows.Forms.View.Details;
            this.lvPassTypes.SelectedIndexChanged += new System.EventHandler(this.lvPassTypes_SelectedIndexChanged);
            this.lvPassTypes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPassTypes_ColumnClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbPayCode);
            this.groupBox1.Controls.Add(this.lblPayCode);
            this.groupBox1.Controls.Add(this.cbPassType);
            this.groupBox1.Controls.Add(this.lblPassType);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.tbDescription);
            this.groupBox1.Controls.Add(this.lblDescription);
            this.groupBox1.Controls.Add(this.tbButton);
            this.groupBox1.Controls.Add(this.lblButton);
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 160);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // tbPayCode
            // 
            this.tbPayCode.Location = new System.Drawing.Point(112, 120);
            this.tbPayCode.Name = "tbPayCode";
            this.tbPayCode.Size = new System.Drawing.Size(168, 20);
            this.tbPayCode.TabIndex = 7;
            // 
            // lblPayCode
            // 
            this.lblPayCode.Location = new System.Drawing.Point(8, 120);
            this.lblPayCode.Name = "lblPayCode";
            this.lblPayCode.Size = new System.Drawing.Size(100, 23);
            this.lblPayCode.TabIndex = 6;
            this.lblPayCode.Text = "Payment Code:";
            this.lblPayCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(112, 88);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(168, 21);
            this.cbPassType.TabIndex = 5;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(24, 88);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(80, 23);
            this.lblPassType.TabIndex = 4;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(505, 448);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(446, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(134, 100);
            this.gbFilter.TabIndex = 29;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(30, 56);
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
            this.cbFilter.Location = new System.Drawing.Point(4, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // PassTypes
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(592, 478);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvPassTypes);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "PassTypes";
            this.ShowInTaskbar = false;
            this.Text = "PassTypes";
            this.Load += new System.EventHandler(this.PassTypes_Load);
            this.Closed += new System.EventHandler(this.PassTypes_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PassTypes_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
					case PassTypes.ButtonIndex:
					case PassTypes.DescriptionIndex:
					case PassTypes.IsPassIndex:
					case PassTypes.PayCodeIndex:
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
		/// Set proper language.
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnCancel.Text = rm.GetString("btnClose", culture);
				
				// Form name
				this.Text = rm.GetString("menuPassTypes", culture);
				
				// group box text
				this.groupBox1.Text = rm.GetString("gbPassTypes", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);

				// label's text
				lblDescription.Text = rm.GetString("lblDescription", culture);
				lblButton.Text = rm.GetString("lblButton", culture);
				lblPassType.Text = rm.GetString("lblPassType", culture);
				lblPayCode.Text = rm.GetString("lblPayCode", culture);

				// list view initialization
				lvPassTypes.BeginUpdate();
				lvPassTypes.Columns.Add(rm.GetString("hdrDescripton", culture), (lvPassTypes.Width - 4) / 4, HorizontalAlignment.Left);
				lvPassTypes.Columns.Add(rm.GetString("hdrButton", culture), (lvPassTypes.Width - 4) / 4, HorizontalAlignment.Left);
				lvPassTypes.Columns.Add(rm.GetString("hdrPassType", culture), (lvPassTypes.Width - 4) / 4, HorizontalAlignment.Left);
				lvPassTypes.Columns.Add(rm.GetString("hdrPayCode", culture), (lvPassTypes.Width - 4) / 4, HorizontalAlignment.Left);
				lvPassTypes.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassType.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		public void populateListView(List<PassTypeTO> passTypesList)
		{
			try
			{
				lvPassTypes.BeginUpdate();
				lvPassTypes.Items.Clear();

				if (passTypesList.Count > 0)
				{
					foreach(PassTypeTO passType in passTypesList)
					{
						ListViewItem item = new ListViewItem();
						
						item.Text = passType.Description.ToString().Trim();
						if (passType.Button < 0)
						{
							item.SubItems.Add("");
						}
						else
						{
							item.SubItems.Add(passType.Button.ToString().Trim());
						}
						if (passType.IsPass == 0)
						{
							//item.SubItems.Add("");
							item.SubItems.Add(rm.GetString("isPassOther", culture));
						}
						else if (passType.IsPass == 1)
						{
							//item.SubItems.Add(passType.IsPass.ToString().Trim());
							item.SubItems.Add(rm.GetString("isPassReader", culture));
						}
						else if (passType.IsPass == 2)
						{
							//item.SubItems.Add(passType.IsPass.ToString().Trim());
							item.SubItems.Add(rm.GetString("isPassWholeDayAbsence", culture));
						}
						item.SubItems.Add(passType.PaymentCode.Trim());

						item.Tag = passType;

						lvPassTypes.Items.Add(item);
					}
				}

				lvPassTypes.EndUpdate();
				lvPassTypes.Invalidate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " PassTypes.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void PassTypes_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvPassTypes);
                lvPassTypes.ListViewItemSorter = _comp;
                lvPassTypes.Sorting = SortOrder.Ascending;

                populatePassTypeCombo();

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                List<PassTypeTO> passTypeList = new PassType().Search();

                populateListView(passTypeList);

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypes.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvPassTypes_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvPassTypes.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvPassTypes.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvPassTypes.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvPassTypes.Sorting = SortOrder.Ascending;
                }

                lvPassTypes.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.lvPassTypes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvPassTypes_ColumnClick():" + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvPassTypes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvPassTypes.SelectedItems.Count > 0)
                {
                    // populate Pass Type's search form
                    currentPassType = (PassTypeTO)lvPassTypes.SelectedItems[0].Tag;

                    tbDescription.Text = currentPassType.Description;

                    tbButton.Text = currentPassType.Button == -1 ? "" : currentPassType.Button.ToString();

                    cbPassType.SelectedValue = currentPassType.IsPass;

                    tbPayCode.Text = currentPassType.PaymentCode.Trim();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.lvPassTypes_SelectedIndexChanged(): " + ex.Message + "\n");
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
                PassTypeAdd ptAdd = new PassTypeAdd(-1);
                ptAdd.ShowDialog(this);

                tbDescription.Text = "";
                tbButton.Text = "";
                tbPayCode.Text = "";
                cbPassType.SelectedIndex = 0;

                List<PassTypeTO> passTypeList = new PassType().Search();
                populateListView(passTypeList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.btnAdd_Click(): " + ex.Message + "\n");
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

                if (lvPassTypes.SelectedItems.Count == 1)
                {
                    PassTypeAdd ptAdd = new PassTypeAdd(currentPassType.PassTypeID);
                    ptAdd.ShowDialog(this);

                    tbDescription.Text = "";
                    tbButton.Text = "";
                    tbPayCode.Text = "";
                    cbPassType.SelectedIndex = 0;

                    List<PassTypeTO> passTypeList = new PassType().Search();
                    populateListView(passTypeList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("msgPleaseSelectPassType", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
			int selected = lvPassTypes.SelectedItems.Count;

			if (!lvPassTypes.SelectedItems.Count.Equals(0))
			{
				try
				{
					DialogResult result = MessageBox.Show(rm.GetString("msgPassTypeDel", culture), "", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;

						foreach(ListViewItem item in lvPassTypes.SelectedItems)
						{
                            if (((PassTypeTO)item.Tag).PassTypeID <= 0 || ((PassTypeTO)item.Tag).PassTypeID == 5)
							{
								MessageBox.Show(rm.GetString("defaultPassTypeDel", culture));
								selected--;
							}
							else
							{		
								// Find if exists Passes for this Pass Type
                                Pass pass = new Pass();
                                pass.PssTO.PassTypeID = ((PassTypeTO)item.Tag).PassTypeID;
								List<PassTO> passArray = pass.Search();
						
								if (passArray.Count > 0)
								{
									MessageBox.Show(item.Text + ": " + rm.GetString("ptHasPasses", culture));
									selected--;
								}
								else
								{
									// Find if exists IO Pairs for this Pass Type
                                    IOPair iop = new IOPair();
                                    iop.PairTO.PassTypeID = ((PassTypeTO)item.Tag).PassTypeID;
									List<IOPairTO> ioPairArray = iop.Search();
						
									if (ioPairArray.Count > 0)
									{
										MessageBox.Show(item.Text + ": " + rm.GetString("ptHasIOPairs", culture));
										selected--;
									}
									else
									{
										// Find if exists Employee Absence for this Pass Type
										//ArrayList emplAbsenceArray = new EmployeeAbsence().Search("", item.Tag.ToString().Trim(),
										//	"", "");
                                        EmployeeAbsence ea = new EmployeeAbsence();
                                        ea.EmplAbsTO.PassTypeID = ((PassTypeTO)item.Tag).PassTypeID;
										List<EmployeeAbsenceTO> emplAbsenceArray = ea.Search("");
						
										if (emplAbsenceArray.Count > 0)
										{
											MessageBox.Show(item.Text + ": " + rm.GetString("ptHasEmplAbsences", culture));
											selected--;
										}
										else
										{
                                            isDeleted = new PassType().Delete(((PassTypeTO)item.Tag).PassTypeID) && isDeleted;
										}
									}
								}
							}
						}

						if ((selected > 0) && isDeleted)
						{
							MessageBox.Show(rm.GetString("passTypeDeleted", culture));
						}
						else if (!isDeleted)
						{
							MessageBox.Show(rm.GetString("passTypeNotDeleted", culture));
						}

						tbDescription.Text = "";
						tbButton.Text = "";
						tbPayCode.Text = "";
						cbPassType.SelectedIndex = 0;

                        List<PassTypeTO> passTypeList = new PassType().Search();
						populateListView(passTypeList);
						currentPassType = new PassTypeTO();
					}
				}
				catch(Exception ex)
				{
					log.writeLog(DateTime.Now + " PassType.btnDelete_Click(): " + ex.Message + "\n");
					MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                }
			}
			else
			{
				MessageBox.Show(rm.GetString("msgPleaseSelectPassType", culture));
			}
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                PassType pt = new PassType();
                pt.PTypeTO.Description = tbDescription.Text.Trim();
                int button = -1;
                if (int.TryParse(tbButton.Text.Trim(), out button))
                    pt.PTypeTO.Button = button;
                pt.PTypeTO.IsPass = (int)cbPassType.SelectedValue;
                pt.PTypeTO.PaymentCode = tbPayCode.Text.Trim();

                List<PassTypeTO> passTypeList = pt.Search();

                if (passTypeList.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("passTypeNotFound", culture));
                }

                populateListView(passTypeList);

                currentPassType = new PassTypeTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypes.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                tbButton.Clear();
                tbDescription.Clear();
                cbPassType.SelectedIndex = 0;
                currentPassType = new PassTypeTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypes.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
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

		private void PassTypes_Closed(object sender, System.EventArgs e)
		{
			Controller.DettachFromNotifier(this.observerClient);
			currentPassType = new PassTypeTO();
		}

		private void InitializeObserverClient()
		{
			observerClient = new NotificationObserverClient(this.ToString());
			Controller = NotificationController.GetInstance();	
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.PassTypesChangedEvent);
		}


		public void PassTypesChangedEvent(object sender, NotificationEventArgs args)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (args.isPassTypeChanged)
                {
                    List<PassTypeTO> passTypeList = new PassType().Search();
                    populateListView(passTypeList);
                    currentPassType = new PassTypeTO();
                }

                btnClear_Click(this, args);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypes.PassTypesChangedEvent(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PassTypes.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populatePassTypeCombo()
		{
			try
			{
				ArrayList ptMemebers = new ArrayList();
								
				PassTypeValue ptValue = new PassTypeValue(rm.GetString("all", culture), -1);
				ptMemebers.Add(ptValue);
				ptValue = new PassTypeValue(rm.GetString("isPassReader", culture), 1);
				ptMemebers.Add(ptValue);
				ptValue = new PassTypeValue(rm.GetString("isPassWholeDayAbsence", culture), 2);
				ptMemebers.Add(ptValue);
				ptValue = new PassTypeValue(rm.GetString("isPassOther", culture), 0);
				ptMemebers.Add(ptValue);
				
				cbPassType.DataSource = ptMemebers;
				cbPassType.DisplayMember = "TypeName";
				cbPassType.ValueMember = "TypeValue";
				cbPassType.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassTypes.populatePassTypeCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
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

        private void PassTypes_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " PassTypes.PassTypes_KeyUp(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PassTypes.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " PassTypes.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
