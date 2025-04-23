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
	/// Summary description for WTGroups.
	/// </summary>
	public class WTGroups : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbGroups;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.ComboBox cbGroups;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ListView lvGroups;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private WorkingGroupTO currentGroup = null;

		private CultureInfo culture;
		ResourceManager rm;

		DebugLog log;
		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		// List View indexes
		const int EmployeeIDIndex = 0;
		const int FirstNameIndex = 1;
		const int LastNameIndex = 2;
		const int WorkingUnitIDIndex = 3;

		private int sortField;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrev;
		private int startIndex;
		private int sortOrder;
        private Button btnMassiveInput;
		List<EmployeeTO> currentEmployeesList;
		
		public WTGroups()
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				this.CenterToScreen();

				currentGroup = new WorkingGroupTO();
				currentEmployeesList = new List<EmployeeTO>();
				logInUser = NotificationController.GetLogInUser();

				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
				rm = new ResourceManager("UI.Resource",typeof(WTGroups).Assembly);
				setLanguage();

				populateWorkingGroupCombo();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
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
            this.gbGroups = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbGroups = new System.Windows.Forms.ComboBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lvGroups = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnMassiveInput = new System.Windows.Forms.Button();
            this.gbGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbGroups
            // 
            this.gbGroups.Controls.Add(this.btnSearch);
            this.gbGroups.Controls.Add(this.cbGroups);
            this.gbGroups.Controls.Add(this.lblDesc);
            this.gbGroups.Location = new System.Drawing.Point(8, 16);
            this.gbGroups.Name = "gbGroups";
            this.gbGroups.Size = new System.Drawing.Size(432, 80);
            this.gbGroups.TabIndex = 0;
            this.gbGroups.TabStop = false;
            this.gbGroups.Text = "Groups";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(336, 32);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbGroups
            // 
            this.cbGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroups.Location = new System.Drawing.Point(96, 32);
            this.cbGroups.Name = "cbGroups";
            this.cbGroups.Size = new System.Drawing.Size(216, 21);
            this.cbGroups.TabIndex = 1;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(8, 32);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(72, 23);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvGroups
            // 
            this.lvGroups.FullRowSelect = true;
            this.lvGroups.GridLines = true;
            this.lvGroups.Location = new System.Drawing.Point(8, 136);
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.Size = new System.Drawing.Size(432, 216);
            this.lvGroups.TabIndex = 3;
            this.lvGroups.UseCompatibleStateImageBehavior = false;
            this.lvGroups.View = System.Windows.Forms.View.Details;
            this.lvGroups.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGroups_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(8, 378);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(96, 378);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(184, 378);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(368, 378);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(408, 104);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(368, 104);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnMassiveInput
            // 
            this.btnMassiveInput.Location = new System.Drawing.Point(275, 358);
            this.btnMassiveInput.Name = "btnMassiveInput";
            this.btnMassiveInput.Size = new System.Drawing.Size(75, 43);
            this.btnMassiveInput.TabIndex = 7;
            this.btnMassiveInput.Text = "Massive input";
            this.btnMassiveInput.Click += new System.EventHandler(this.btnMassiveInput_Click);
            // 
            // WTGroups
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(448, 426);
            this.ControlBox = false;
            this.Controls.Add(this.btnMassiveInput);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvGroups);
            this.Controls.Add(this.gbGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "WTGroups";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WTGroups";
            this.Load += new System.EventHandler(this.WTGroups_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WTGroups_KeyUp);
            this.gbGroups.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Inner Class for sorting Array List of Employees

		/*
		 *  Class used for sorting Array List of Employees
		*/

		private class ArrayListSort:IComparer<EmployeeTO>  
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}        
			
			public int Compare(EmployeeTO x, EmployeeTO y)        
			{
				EmployeeTO empl1 = null;
				EmployeeTO empl2 = null;

				if (compOrder == Constants.sortAsc)
				{
					empl1 = x;
					empl2 = y;
				}
				else
				{
					empl1 = y;
					empl2 = x;
				}

				switch(compField)            
				{                
					case WTGroups.EmployeeIDIndex: 
						return empl1.EmployeeID.CompareTo(empl2.EmployeeID);
					case WTGroups.FirstNameIndex:
						return empl1.FirstName.CompareTo(empl2.FirstName);
					case WTGroups.LastNameIndex:
						return empl1.LastName.CompareTo(empl2.LastName);
					case WTGroups.WorkingUnitIDIndex:
						return empl1.WorkingUnitName.CompareTo(empl2.WorkingUnitName);
					default:                    
						return empl1.LastName.CompareTo(empl2.LastName);
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
				this.Text = rm.GetString("menuWTGroups", culture);

				// group box text
				gbGroups.Text = rm.GetString("gbGroups", culture);
				
				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
                btnMassiveInput.Text = rm.GetString("btnMassiveInput", culture);

				// label's text
				lblDesc.Text = rm.GetString("lblDescription", culture);
				
				// list view
				lvGroups.BeginUpdate();
				lvGroups.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvGroups.Width - 4) / 4, HorizontalAlignment.Left);
				lvGroups.Columns.Add(rm.GetString("hdrFirstName", culture), (lvGroups.Width - 4) / 4, HorizontalAlignment.Left);
				lvGroups.Columns.Add(rm.GetString("hdrLastName", culture), (lvGroups.Width - 4) / 4, HorizontalAlignment.Left);
				lvGroups.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvGroups.Width - 4) / 4, HorizontalAlignment.Left);
				lvGroups.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Working Groups Combo Box
		/// </summary>
		private void populateWorkingGroupCombo()
		{
			try
			{				
				List<WorkingGroupTO> wgArray = new WorkingGroup().Search();
				wgArray.Insert(0, new WorkingGroupTO(-1, rm.GetString("all", culture), rm.GetString("all", culture)));

				cbGroups.DataSource = wgArray;
				cbGroups.DisplayMember = "GroupName";
				cbGroups.ValueMember = "EmployeeGroupID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.populateWorkingGroupCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " WTGroups.btnClose_Click(): " + ex.Message + "\n");
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
				WTGroupsAdd groupAdd = new WTGroupsAdd();
				groupAdd.ShowDialog(this);

				populateWorkingGroupCombo();
				listViewClear();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.btnAdd_Click(): " + ex.Message + "\n");
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
				if (this.cbGroups.SelectedIndex <= 0)
				{
					MessageBox.Show(rm.GetString("noSelGrp", culture));
					return;
				}

				currentGroup = new WorkingGroup().Find((int) this.cbGroups.SelectedValue);
				WTGroupsAdd groupAdd = new WTGroupsAdd(currentGroup);
				groupAdd.ShowDialog(this);

				populateWorkingGroupCombo();
				listViewClear();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.btnUpdate_Click(): " + ex.Message + "\n");
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
				int wgID = -1;
								
				if (this.cbGroups.SelectedValue.ToString() == "-1")
				{
					MessageBox.Show(rm.GetString("selWTGroup", culture));
					return;
				}
				else
				{
					wgID = (int)this.cbGroups.SelectedValue;
				}

                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);

                Employee empl = new Employee();
                empl.EmplTO.WorkingGroupID = wgID;
                currentEmployeesList = empl.SearchWithStatuses(statuses, "");

				if (currentEmployeesList.Count > 0)
				{
					populateListView(currentEmployeesList, startIndex);
				}
				else
				{
					MessageBox.Show(rm.GetString("emplNotFound", culture));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.btnSearch_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}
	
		/// <summary>
		/// Populate ListView
		/// </summary>
		/// <param name="employeesList">Array of Employee object</param>
		public void populateListView(List<EmployeeTO> employeesList, int startIndex)
		{
			try
			{
				if (employeesList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}
                				
				lvGroups.BeginUpdate();
				lvGroups.Items.Clear();

				if (employeesList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < employeesList.Count))
					{
						if (startIndex == 0)
						{
							btnPrev.Enabled = false;
						}
						else
						{
							btnPrev.Enabled = true;
						}

						int lastIndex = startIndex + Constants.recordsPerPage;
						if (lastIndex >= employeesList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = employeesList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

						for (int i = startIndex; i < lastIndex; i++)
						{
							EmployeeTO employee = employeesList[i];
							ListViewItem item = new ListViewItem();

							item.Text = employee.EmployeeID.ToString().Trim();
							item.SubItems.Add(employee.FirstName.Trim());
							item.SubItems.Add(employee.LastName.Trim());

							// Get Working Unit name for the particular user
                            //wu = new WorkingUnit();
                            //if (wu.Find(employee.WorkingUnitID))
                            //{
                            //    item.SubItems.Add(wu.Name.Trim());
                            //}

                            item.SubItems.Add(employee.WorkingUnitName.Trim());
						
							lvGroups.Items.Add(item);
						}
					}
				}

				lvGroups.EndUpdate();
				lvGroups.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.populateListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbGroups.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelGrpDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteWorkingGroups", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        if (this.cbGroups.SelectedValue.ToString().Trim().Equals("0"))
                        {
                            MessageBox.Show(rm.GetString("defaultWorkingGroup", culture));
                        }
                        else
                        {
                            List<string> statuses = new List<string>();
                            statuses.Add(Constants.statusActive);
                            statuses.Add(Constants.statusBlocked);

                            Employee empl = new Employee();
                            empl.EmplTO.WorkingGroupID = (int)cbGroups.SelectedValue;
                            List<EmployeeTO> employees = empl.SearchWithStatuses(statuses, "");

                            if (employees.Count > 0)
                            {
                                MessageBox.Show(rm.GetString("groupHasEmpl", culture));
                            }
                            else
                            {
                                WorkingGroup wg = new WorkingGroup();
                                bool trans = wg.BeginTransaction();
                                if (trans)
                                {
                                    try
                                    {
                                        bool isDeleted = true;
                                        List<EmployeeGroupsTimeScheduleTO> groupSchedules = new EmployeeGroupsTimeSchedule().SearchFromSchedules(
                                   currentGroup.EmployeeGroupID, new DateTime(), wg.GetTransaction());
                                        if (groupSchedules.Count > 0)
                                        {
                                            EmployeeGroupsTimeSchedule egTimeSch = new EmployeeGroupsTimeSchedule();
                                            egTimeSch.SetTransaction(wg.GetTransaction());
                                            isDeleted = egTimeSch.DeleteSchedule((int)cbGroups.SelectedValue, false);
                                        }
                                        isDeleted = isDeleted && wg.Delete((int)cbGroups.SelectedValue, false);
                                        if (isDeleted == true)
                                        {
                                            wg.CommitTransaction();
                                            MessageBox.Show(rm.GetString("groupDeleted", culture));
                                            populateWorkingGroupCombo();
                                            listViewClear();
                                        }
                                        else
                                        {
                                            wg.RollbackTransaction();
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        if (wg.GetTransaction() != null)
                                            wg.RollbackTransaction();
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroups.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void listViewClear()
		{
			try
			{
				lvGroups.BeginUpdate();
				lvGroups.Items.Clear();
				lvGroups.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.listViewClear(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void lvGroups_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				int prevOrder = sortOrder;

				if (e.Column == sortField)
				{
					if (prevOrder == Constants.sortAsc)
					{
						sortOrder = Constants.sortDesc;
					}
					else
					{
						sortOrder = Constants.sortAsc;
					}
				}
				else
				{
					// New Sort Order
					sortOrder = Constants.sortAsc;
				}

				sortField = e.Column;

				currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
				startIndex = 0;
				populateListView(currentEmployeesList, startIndex);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.lvGroups_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void WTGroups_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                sortOrder = Constants.sortAsc;
                sortField = WTGroups.LastNameIndex;
                startIndex = 0;

                btnPrev.Visible = false;
                btnNext.Visible = false;
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " WTGroups.WTGroups_Load(): " + ex.Message + "\n");
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
                btnMassiveInput.Enabled = updatePermission;
				btnDelete.Enabled = deletePermission;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnPrev_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				startIndex -= Constants.recordsPerPage;
				if (startIndex < 0)
				{
					startIndex = 0;
				}
				populateListView(currentEmployeesList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.btnPrev_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				startIndex += Constants.recordsPerPage;
				populateListView(currentEmployeesList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " WTGroups.btnNext_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        private void WTGroups_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " WTGroups.WTGroups_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
                
        private void btnMassiveInput_Click(object sender, EventArgs e)
        {
            try
            {
                // massive input is allowed only if system is closed and DP finshed his work
                SystemClosingEventTO eventTO = new SystemClosingEventTO();
                List<SystemClosingEventTO> closingList = new SystemClosingEvent().Search(DateTime.Now, DateTime.Now);

                foreach (SystemClosingEventTO evtTO in closingList)
                {
                    if (evtTO.Type.Trim().ToUpper() == Constants.closingEventTypeDemanded.Trim().ToUpper())
                    {
                        eventTO = evtTO;                        
                    }

                    if (evtTO.Type.Trim().ToUpper().Equals(Constants.closingEventTypeRegularPeriodical))
                    {
                        if ((evtTO.StartTime.TimeOfDay < evtTO.EndTime.TimeOfDay && DateTime.Now.TimeOfDay >= evtTO.StartTime.TimeOfDay && DateTime.Now.TimeOfDay < evtTO.EndTime.TimeOfDay)
                            || (evtTO.StartTime.TimeOfDay >= evtTO.EndTime.TimeOfDay && (DateTime.Now.TimeOfDay < evtTO.EndTime.TimeOfDay || DateTime.Now.TimeOfDay >= evtTO.StartTime.TimeOfDay)))
                        {
                            bool altLang = !NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                            if (altLang)
                                MessageBox.Show(evtTO.MessageEN.Trim());
                            else
                                MessageBox.Show(evtTO.MessageSR.Trim());

                            return;
                        }
                    }
                }

                if (eventTO.EventID == -1 || eventTO.DPEngineState.Trim().ToUpper() != Constants.DPEngineState.FINISHED.ToString().Trim().ToUpper())
                {
                    MessageBox.Show(rm.GetString("systemNotClosed", culture));
                }
                else
                {
                    GroupChangedMassiveInput grpChanged = new GroupChangedMassiveInput();
                    grpChanged.ShowDialog(this);

                    populateWorkingGroupCombo();
                    listViewClear();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTGroups.btnMassiveInput_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        
	}
}
