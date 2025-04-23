using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;
using ReaderInterface;

namespace UI
{
	/// <summary>
	/// Summary description for ExitPermissionsAdd.
	/// </summary>
	public class ExitPermissionsAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblWU;
		private System.Windows.Forms.ComboBox cbWU;
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.Label lblPassType;
		private System.Windows.Forms.ComboBox cbPassType;
		private System.Windows.Forms.DateTimePicker dtpDateTime;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblDateAndTime;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ResourceManager rm;
		private CultureInfo culture;
		DebugLog log;
		ApplUserTO logInUser;
		ExitPermissionTO currentExitPerm = null;
		PassTO selectedPassTO = null;

		// List View indexes
		const int FirstNameIndex = 0;
		const int LastNameIndex = 1;
		const int WorkingUnitIDIndex = 2;

		List<EmployeeTO> currentEmployeesList;
		private int sortOrder;
		private int sortField;
		private int startIndex;

		private List<WorkingUnitTO> wUnits;
		private string wuString = "";

		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Label lblOffset;
		private System.Windows.Forms.TextBox tbOffset;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListView lvEmployees;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrev;
		private System.Windows.Forms.CheckBox cbWorkDayStart;
		private System.Windows.Forms.Label label4;

		private DateTime startTime = new DateTime(0);
		private DateTime firstIN = new DateTime(0);
        private Button btnWUTree;
        private CheckBox chbHierarhicly;

		//Indicate if calling form needs to be reload
		public bool doReloadOnBack = true;
		
		public ExitPermissionsAdd()
		{
			InitializeComponent();
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentExitPerm = new ExitPermissionTO();
			currentEmployeesList = new List<EmployeeTO>();
			selectedPassTO = new PassTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(ExitPermissionsAdd).Assembly);
			setLanguage();
			
			populatePassTypeCombo();

			tbOffset.Text = ((int) Constants.offset).ToString();

			btnUpdate.Visible = false;
			lvEmployees.MultiSelect = true;
            
            // Sanja 21.9.2011.
            dtpDateTime.Value = dtpDateTime.Value.AddMinutes(5);
		}

		public ExitPermissionsAdd(ExitPermissionTO exitPerm)
		{
			InitializeComponent();
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentExitPerm = new ExitPermissionTO(exitPerm.PermissionID, exitPerm.EmployeeID, exitPerm.PassTypeID,
				exitPerm.StartTime, exitPerm.Offset, exitPerm.Used, exitPerm.Description, exitPerm.IssuedBy, exitPerm.IssuedTime);
			currentExitPerm.VerifiedBy = exitPerm.VerifiedBy;
			currentEmployeesList = new List<EmployeeTO>();

			selectedPassTO = new PassTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(ExitPermissionsAdd).Assembly);
			setLanguage();

			populatePassTypeCombo();

			btnSave.Visible = false;
			lvEmployees.MultiSelect = false;
            if (exitPerm.Used == (int)Constants.Used.Yes)
            {
                cbWU.Enabled = btnWUTree.Enabled = chbHierarhicly.Enabled = lvEmployees.Enabled =
                    cbPassType.Enabled = dtpDateTime.Enabled = tbOffset.Enabled = cbWorkDayStart.Enabled = false;
            }
			populateUpdateForm(currentExitPerm);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExitPermissionsAdd));
            this.lblWU = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblPassType = new System.Windows.Forms.Label();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblDateAndTime = new System.Windows.Forms.Label();
            this.dtpDateTime = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblOffset = new System.Windows.Forms.Label();
            this.tbOffset = new System.Windows.Forms.TextBox();
            this.lblMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.cbWorkDayStart = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(8, 16);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(100, 23);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(120, 16);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(188, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(16, 66);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(104, 23);
            this.lblEmployee.TabIndex = 2;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(8, 316);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(100, 23);
            this.lblPassType.TabIndex = 5;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(120, 318);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(216, 21);
            this.cbPassType.TabIndex = 6;
            // 
            // lblDateAndTime
            // 
            this.lblDateAndTime.Location = new System.Drawing.Point(4, 353);
            this.lblDateAndTime.Name = "lblDateAndTime";
            this.lblDateAndTime.Size = new System.Drawing.Size(104, 23);
            this.lblDateAndTime.TabIndex = 8;
            this.lblDateAndTime.Text = "Date and Time:";
            this.lblDateAndTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDateTime
            // 
            this.dtpDateTime.CustomFormat = "dd.MM.yyy HH:mm";
            this.dtpDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTime.Location = new System.Drawing.Point(120, 354);
            this.dtpDateTime.Name = "dtpDateTime";
            this.dtpDateTime.Size = new System.Drawing.Size(216, 20);
            this.dtpDateTime.TabIndex = 9;
            this.dtpDateTime.ValueChanged += new System.EventHandler(this.dtpDateTime_ValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 533);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 533);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 18;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(261, 533);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(8, 423);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(100, 23);
            this.lblDesc.TabIndex = 15;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(120, 425);
            this.tbDesc.MaxLength = 500;
            this.tbDesc.Multiline = true;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbDesc.Size = new System.Drawing.Size(216, 56);
            this.tbDesc.TabIndex = 16;
            // 
            // lblOffset
            // 
            this.lblOffset.Location = new System.Drawing.Point(8, 387);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(100, 23);
            this.lblOffset.TabIndex = 11;
            this.lblOffset.Text = "Offset:";
            this.lblOffset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbOffset
            // 
            this.tbOffset.Location = new System.Drawing.Point(120, 389);
            this.tbOffset.Name = "tbOffset";
            this.tbOffset.Size = new System.Drawing.Size(64, 20);
            this.tbOffset.TabIndex = 12;
            // 
            // lblMin
            // 
            this.lblMin.Location = new System.Drawing.Point(190, 387);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(24, 23);
            this.lblMin.TabIndex = 13;
            this.lblMin.Text = "min";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(344, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(344, 318);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(220, 389);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(344, 354);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "*";
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(19, 95);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(320, 208);
            this.lvEmployees.TabIndex = 3;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(304, 66);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 21;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(266, 66);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 20;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // cbWorkDayStart
            // 
            this.cbWorkDayStart.Location = new System.Drawing.Point(120, 487);
            this.cbWorkDayStart.Name = "cbWorkDayStart";
            this.cbWorkDayStart.Size = new System.Drawing.Size(216, 40);
            this.cbWorkDayStart.TabIndex = 17;
            this.cbWorkDayStart.Text = "Working day beginning";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(314, 14);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 41;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(120, 43);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 45;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // ExitPermissionsAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(368, 571);
            this.ControlBox = false;
            this.Controls.Add(this.chbHierarhicly);
            this.Controls.Add(this.btnWUTree);
            this.Controls.Add(this.cbWorkDayStart);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lvEmployees);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.tbOffset);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.lblOffset);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dtpDateTime);
            this.Controls.Add(this.lblDateAndTime);
            this.Controls.Add(this.cbPassType);
            this.Controls.Add(this.lblPassType);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.cbWU);
            this.Controls.Add(this.lblWU);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ExitPermissionsAdd";
            this.ShowInTaskbar = false;
            this.Text = "ExitPermissions";
            this.Load += new System.EventHandler(this.ExitPermissionsAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExitPermissionsAdd_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

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
					case ExitPermissionsAdd.FirstNameIndex:
						return empl1.FirstName.CompareTo(empl2.FirstName);
					case ExitPermissionsAdd.LastNameIndex:
						return empl1.LastName.CompareTo(empl2.LastName);
					case ExitPermissionsAdd.WorkingUnitIDIndex:
						return empl1.WorkingUnitName.CompareTo(empl2.WorkingUnitName);
					default:                    
						return empl1.LastName.CompareTo(empl2.LastName);
				}
            }    
		}

		#endregion

		/// <summary>
		/// Set proper language
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				if (currentExitPerm.PermissionID >= 0)
				{
					this.Text = rm.GetString("ExitPermUpdForm", culture);
				}
				else
				{
					this.Text = rm.GetString("ExitPermAddForm", culture);
				}
				
				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblWU.Text = rm.GetString("lblWU", culture);
				lblEmployee.Text = rm.GetString("lblEmployee", culture);
				lblPassType.Text = rm.GetString("lblPassType", culture);
				lblDateAndTime.Text = rm.GetString("lblDateAndTime", culture);
				lblOffset.Text = rm.GetString("lblOffset", culture);
				lblDesc.Text = rm.GetString("lblDescription", culture);

				// check box's text
				cbWorkDayStart.Text = rm.GetString("cbWorkDayStart", culture);
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

				// list view
				lvEmployees.BeginUpdate();
				lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);
				lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);
				lvEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);

				lvEmployees.EndUpdate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Working Unit Combo Box
		/// </summary>
		private void populateWorkingUnitCombo()
		{
			try
			{
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWU.DataSource = wuArray;
				cbWU.DisplayMember = "Name";
				cbWU.ValueMember = "WorkingUnitID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void populatePassTypeCombo()
		{
			try
			{
                PassType pType = new PassType();
                pType.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> ptArray = pType.Search();
                List<PassTypeTO> passTypes = new List<PassTypeTO>();

                foreach (PassTypeTO pt in ptArray)
                {
                    if (pt.PassTypeID != 0)
                    {
                        passTypes.Add(pt);
                    }
                }

                passTypes.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

				cbPassType.DataSource = passTypes;
				cbPassType.DisplayMember = "Description";
				cbPassType.ValueMember = "PassTypeID";
				cbPassType.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.populatePassTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		public void populateListView(List<EmployeeTO> employeeList, int startIndex)
		{
			try
			{
				if (employeeList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}

				lvEmployees.BeginUpdate();
				lvEmployees.Items.Clear();

				if (employeeList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < employeeList.Count))
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
						if (lastIndex >= employeeList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = employeeList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

						for (int i = startIndex; i < lastIndex; i++)
						{
							EmployeeTO employee = employeeList[i];
							ListViewItem item = new ListViewItem();

							item.Text = employee.FirstName.Trim();
							item.SubItems.Add(employee.LastName.Trim());

							// Get Working Unit name for the particular user
                            //WorkingUnit wu = new WorkingUnit();
                            //if (wu.Find(employee.WorkingUnitID))
                            //{
                            //    item.SubItems.Add(wu.Name.Trim());
                            //}

                            item.SubItems.Add(employee.WorkingUnitName.Trim());
							item.Tag = employee.EmployeeID;

							lvEmployees.Items.Add(item);
						}
					}
				}

				lvEmployees.EndUpdate();
				lvEmployees.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void populateUpdateForm(ExitPermissionTO exitPerm)
		{
			try
			{
				cbPassType.SelectedValue = exitPerm.PassTypeID;
				dtpDateTime.Value = exitPerm.StartTime;
				tbOffset.Text = exitPerm.Offset.ToString().Trim();
				tbDesc.Text = exitPerm.Description.ToString().Trim();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.populateUpdateForm(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private bool validate()
		{
			bool isValid = true;

			try
			{
				if (lvEmployees.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("exitPermEmployeeNotNull", culture));
					return false;
				}
				if (cbPassType.SelectedIndex <= 0)
				{
					MessageBox.Show(rm.GetString("exitPermPassTypeNotNull", culture));
					return false;
				}
				if (dtpDateTime.Value.Equals(new DateTime(0)))
				{
					MessageBox.Show(rm.GetString("exitPermStartTimeNotNull", culture));
					return false;
				}				
				try
				{
					if (Int32.Parse(tbOffset.Text.Trim()) <= 0)
					{
						MessageBox.Show(rm.GetString("ExitPermOffsetNotNumber", culture));
						return false;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("ExitPermOffsetNotNumber", culture));
					return false;
				}

                if (cbWorkDayStart.Checked && dtpDateTime.Value > DateTime.Now)
                {
                    MessageBox.Show(rm.GetString("workDayStartFutureDate", culture));
                    return false;
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.validate(): " + ex.Message + "\n");
				isValid = false;
				throw new Exception(ex.Message);
			}

			return isValid;
		}

		public void setSelectedPass(PassTO passTO)
		{
			selectedPassTO = passTO;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Do not reload calling form on cancel
                doReloadOnBack = false;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
			
		}

		private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWU.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            if (!chbHierarhicly.Checked)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }
                if (!check)
                {
                    List<string> statuses = new List<string>();
                    statuses.Add(Constants.statusActive);
                    statuses.Add(Constants.statusBlocked);
                    
                    if (cbWU.SelectedIndex <= 0)
                    {
                        currentEmployeesList = new Employee().SearchByWUWithStatuses(wuString, statuses);
                    }
                    else
                    {
                        string selectedWU = cbWU.SelectedValue.ToString();

                        //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                        WorkingUnit wu = new WorkingUnit();
                        if ((int)this.cbWU.SelectedValue != -1 && chbHierarhicly.Checked)
                        {
                            List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                            WorkingUnit workUnit = new WorkingUnit();
                            foreach (WorkingUnitTO workingUnit in wUnits)
                            {
                                if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
                                {
                                    wuList.Add(workingUnit);
                                    workUnit.WUTO = workingUnit;
                                }
                            }
                            if (workUnit.WUTO.ChildWUNumber > 0)
                                wuList = wu.FindAllChildren(wuList);
                            selectedWU = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                selectedWU += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (selectedWU.Length > 0)
                            {
                                selectedWU = selectedWU.Substring(0, selectedWU.Length - 1);
                            }
                        }
                        currentEmployeesList = new Employee().SearchWithStatuses(statuses, selectedWU);
                    }

                    currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                    startIndex = 0;
                    populateListView(currentEmployeesList, startIndex);
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				bool isValid = validate();

				if (isValid)
				{
					if (dtpDateTime.Value >= DateTime.Now)
					{
						int inserted = 0;

						currentExitPerm.PassTypeID = (int) cbPassType.SelectedValue;
						currentExitPerm.StartTime = dtpDateTime.Value;
						currentExitPerm.Offset = Int32.Parse(tbOffset.Text.Trim());
						currentExitPerm.Description = tbDesc.Text.Trim();
						//currentExitPerm.Used = (int) Constants.Used.No;
						if (logInUser.ExitPermVerification == (int)Constants.PermVerification.Yes)
						{
							currentExitPerm.Used = (int) Constants.Used.No;
							currentExitPerm.VerifiedBy = logInUser.UserID;
						}
						else
						{
							currentExitPerm.Used = (int) Constants.Used.Unverified;
							currentExitPerm.VerifiedBy = "";
						}

						currentExitPerm.IssuedBy = logInUser.UserID;

						foreach (ListViewItem item in lvEmployees.SelectedItems)
						{
							currentExitPerm.EmployeeID = (int)item.Tag;

							inserted = new ExitPermission().Save(currentExitPerm.EmployeeID, 
								currentExitPerm.PassTypeID, currentExitPerm.StartTime, currentExitPerm.Offset,
								currentExitPerm.Used, currentExitPerm.Description, currentExitPerm.VerifiedBy);
						}

						if (inserted > 0)
						{
							MessageBox.Show(rm.GetString("exitPermSaved", culture));
							this.Close();
						}

					}
					else
					{
						if (logInUser.ExitPermVerification == (int)Constants.PermVerification.No)
						{
							MessageBox.Show(rm.GetString("noRetroExitPermAdd", culture));
							return;
						}

                        if (lvEmployees.SelectedItems.Count > 1)
                        {
                            MessageBox.Show(rm.GetString("noRetroExitPermAddMulty", culture));
                            return;
                        }

						currentExitPerm.PassTypeID = (int) cbPassType.SelectedValue;
						currentExitPerm.StartTime = dtpDateTime.Value;
						currentExitPerm.Offset = Int32.Parse(tbOffset.Text.Trim());
						currentExitPerm.Description = tbDesc.Text.Trim();
						currentExitPerm.IssuedBy = logInUser.UserID;
						currentExitPerm.Used = (int) Constants.Used.Yes;						
						currentExitPerm.VerifiedBy = logInUser.UserID;

						if (lvEmployees.SelectedItems.Count == 1)
						{
							// one Employee is selected
							if (cbWorkDayStart.Checked)
							{
								currentExitPerm.EmployeeID = (int) lvEmployees.SelectedItems[0].Tag;
								int inserted = 0;
								DateTime datePerm = new DateTime(dtpDateTime.Value.Year, dtpDateTime.Value.Month, dtpDateTime.Value.Day, 0, 0, 0);
								ExitPermissionsWorkingDayBeginning permBegin = new ExitPermissionsWorkingDayBeginning((int) lvEmployees.SelectedItems[0].Tag, datePerm);
								permBegin.ShowDialog(this);

								if (!startTime.Equals(new DateTime(0)) && !firstIN.Equals(new DateTime(0)))
								{
									PassTO passIN = new PassTO(-1, currentExitPerm.EmployeeID, Constants.DirectionIn, 
										startTime, (int) Constants.PassType.Work, 
										(int) Constants.PairGenUsed.Unused, Constants.defaultLocID,
										(int) Constants.recordCreated.Automaticaly, 
										(int) Constants.IsWrkCount.IsCounter);

									DateTime eventTime = startTime.AddSeconds(1);
									PassTO passOUT = new PassTO(-1, currentExitPerm.EmployeeID, 
										Constants.DirectionOut, eventTime, currentExitPerm.PassTypeID, 
										(int) Constants.PairGenUsed.Unused, Constants.defaultLocID,
										(int) Constants.recordCreated.Automaticaly, 
										(int) Constants.IsWrkCount.IsCounter);
								
									List<PassTO> passTOList = new List<PassTO>();
									passTOList.Add(passIN);
									passTOList.Add(passOUT);
									currentExitPerm.StartTime = startTime;
									currentExitPerm.Used = (int) Constants.Used.Yes;
									currentExitPerm.VerifiedBy = logInUser.UserID;
									inserted = new Pass().SavePassesPermission(passTOList, currentExitPerm, logInUser.UserID.Trim());
								}

								if (inserted > 0)
								{
									MessageBox.Show(rm.GetString("exitPermSaved", culture));
									this.Close();
								}
								else
								{
									MessageBox.Show(rm.GetString("exitPermBeginNotSaved", culture));
								}
							}
							else
							{
								// select all passes for selected Employee and Date
								List<PassTO> passes = new Pass().SearchPassesForExitPerm((int) lvEmployees.SelectedItems[0].Tag, dtpDateTime.Value);

								if (passes.Count > 0)
								{
									ExitPermissionPasses passesForm = new ExitPermissionPasses(passes);
									passesForm.ShowDialog(this);
							
									// save Exit Permission and update selected Pass with selected Pass Type
									if (selectedPassTO.PassID >= 0)
									{
										selectedPassTO.PassTypeID = (int) cbPassType.SelectedValue;
										selectedPassTO.PairGenUsed = (int) Constants.PairGenUsed.Unused;

										currentExitPerm.EmployeeID = (int) lvEmployees.SelectedItems[0].Tag;
										currentExitPerm.StartTime = selectedPassTO.EventTime;

										bool savedRetroactive = new ExitPermission().SaveRetroactive(currentExitPerm.EmployeeID, currentExitPerm.PassTypeID, currentExitPerm.StartTime, 
											currentExitPerm.Offset, currentExitPerm.Used, currentExitPerm.Description,
											selectedPassTO, currentExitPerm.VerifiedBy);

										if (savedRetroactive)
										{
											MessageBox.Show(rm.GetString("exitPermSaved", culture));
											this.Close();
										}
									}
								}
								else
								{
									MessageBox.Show(rm.GetString("noPassesForExitPermFound", culture));
								}
							}
						}
						/*else
						{
							// more Employees are selected
							bool saved = false;

							int counter = 0;
							foreach (ListViewItem item in lvEmployees.SelectedItems)
							{
								// Find first Pass with direction 'OUT' and event time between start time and start time + offset
								ArrayList passes = new Pass().SearchPassesForExitPerm((int) item.Tag,
									currentExitPerm.StartTime, currentExitPerm.Offset);
								
								// Update found Pass
								if (passes.Count > 0)
								{
									if (counter == 0)
									{
										saved = true;
									}

									selectedPassTO = ((Pass) passes[0]).sendTransferObject();

									selectedPassTO.PassTypeID = currentExitPerm.PasTypeID;
									selectedPassTO.PairGenUsed = (int) Constants.PairGenUsed.Unused;

									currentExitPerm.EmployeeID = (int) item.Tag;

									saved = currentExitPerm.SaveRetroactive(currentExitPerm.EmployeeID, currentExitPerm.PasTypeID, currentExitPerm.StartTime, 
										currentExitPerm.Offset, currentExitPerm.Used, currentExitPerm.Description,
										selectedPassTO, currentExitPerm.VerifiedBy) && saved;
								}
								else
								{
									saved = false;
								}

								counter++;
							}

							if (saved)
							{
								MessageBox.Show(rm.GetString("exitPermSaved", culture));
								this.Close();
							}
							else
							{
								MessageBox.Show(rm.GetString("noPassesForExitPermissionsFound", culture));
							}
						}*/
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.btnSave_Click(): " + ex.Message + "\n");
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
			
				bool isValid = validate();

                if (currentExitPerm.Used == (int)Constants.Used.Yes)
                {
                    currentExitPerm.Description = tbDesc.Text.Trim();
                    bool updated = new ExitPermission().Update(currentExitPerm.PermissionID,
                            currentExitPerm.EmployeeID, currentExitPerm.PassTypeID,
                            currentExitPerm.StartTime, currentExitPerm.Offset, currentExitPerm.Used,
                            currentExitPerm.Description, currentExitPerm.VerifiedBy);

                    if (updated)
                    {
                        MessageBox.Show(rm.GetString("exitPermUpdated", culture));
                        this.Close();
                    }
                }

				else if (isValid)
				{                   
					if ((logInUser.ExitPermVerification == (int)Constants.PermVerification.No)
						&& (currentExitPerm.Used != (int) Constants.Used.Unverified))
					{
						MessageBox.Show(rm.GetString("noVerifiedUpd", culture));
						return;
					}

					if (dtpDateTime.Value >= DateTime.Now)
					{
						currentExitPerm.EmployeeID = (int) lvEmployees.SelectedItems[0].Tag;
						currentExitPerm.PassTypeID = (int) cbPassType.SelectedValue;
						currentExitPerm.StartTime = dtpDateTime.Value;
						currentExitPerm.Offset = Int32.Parse(tbOffset.Text.Trim());
						currentExitPerm.Description = tbDesc.Text.Trim();
						//currentExitPerm.Used = (int) Constants.Used.No;
						if (logInUser.ExitPermVerification == (int)Constants.PermVerification.Yes)
						{
							if (currentExitPerm.Used == (int) Constants.Used.Unverified)
								currentExitPerm.VerifiedBy = logInUser.UserID;

							currentExitPerm.Used = (int) Constants.Used.No;							
						}
						currentExitPerm.IssuedBy = logInUser.UserID;

						bool updated = new ExitPermission().Update(currentExitPerm.PermissionID, 
							currentExitPerm.EmployeeID, currentExitPerm.PassTypeID,
							currentExitPerm.StartTime, currentExitPerm.Offset, currentExitPerm.Used,
							currentExitPerm.Description, currentExitPerm.VerifiedBy);

						if (updated)
						{
							MessageBox.Show(rm.GetString("exitPermUpdated", culture));
							this.Close();
						}
					}
					else
					{
						if (logInUser.ExitPermVerification == (int)Constants.PermVerification.No)
						{
							MessageBox.Show(rm.GetString("noRetroExitPermUpd", culture));
							return;
						}

						if (cbWorkDayStart.Checked)
						{
							currentExitPerm.EmployeeID = (int) lvEmployees.SelectedItems[0].Tag;
							currentExitPerm.PassTypeID = (int) cbPassType.SelectedValue;
							currentExitPerm.Offset = Int32.Parse(tbOffset.Text.Trim());
							currentExitPerm.Description = tbDesc.Text.Trim();							
							currentExitPerm.IssuedBy = logInUser.UserID;
							if (currentExitPerm.Used == (int) Constants.Used.Unverified)
								currentExitPerm.VerifiedBy = logInUser.UserID;
							currentExitPerm.Used = (int) Constants.Used.Yes;

							int inserted = 0;
							DateTime datePerm = new DateTime(dtpDateTime.Value.Year, dtpDateTime.Value.Month, dtpDateTime.Value.Day, 0, 0, 0);
							ExitPermissionsWorkingDayBeginning permBegin = new ExitPermissionsWorkingDayBeginning((int) lvEmployees.SelectedItems[0].Tag, datePerm);
							permBegin.ShowDialog(this);

							if (!startTime.Equals(new DateTime(0)) && !firstIN.Equals(new DateTime(0)))
							{
								PassTO passIN = new PassTO(-1, currentExitPerm.EmployeeID, Constants.DirectionIn, 
									startTime, (int) Constants.PassType.Work, 
									(int) Constants.PairGenUsed.Unused, Constants.defaultLocID,
									(int) Constants.recordCreated.Automaticaly, 
									(int) Constants.IsWrkCount.IsCounter);

								DateTime eventTime = startTime.AddSeconds(1);
								PassTO passOUT = new PassTO(-1, currentExitPerm.EmployeeID, 
									Constants.DirectionOut, eventTime, currentExitPerm.PassTypeID, 
									(int) Constants.PairGenUsed.Unused, Constants.defaultLocID,
									(int) Constants.recordCreated.Automaticaly, 
									(int) Constants.IsWrkCount.IsCounter);
								
								List<PassTO> passTOList = new List<PassTO>();
								passTOList.Add(passIN);
								passTOList.Add(passOUT);
								currentExitPerm.StartTime = startTime;
								inserted = new Pass().SavePassesPermission(passTOList, currentExitPerm, logInUser.UserID.Trim());
							}

							if (inserted > 0)
							{
								MessageBox.Show(rm.GetString("exitPermSaved", culture));
								this.Close();
							}
							else
							{
								MessageBox.Show(rm.GetString("exitPermBeginNotSaved", culture));
							}
						}
						else
						{
							// select all passes for selected Employee and Date
							List<PassTO> passes = new Pass().SearchPassesForExitPerm((int) lvEmployees.SelectedItems[0].Tag, dtpDateTime.Value);

							if (passes.Count > 0)
							{
								ExitPermissionPasses passesForm = new ExitPermissionPasses(passes);
								passesForm.ShowDialog(this);
							
								// update Exit Permission and update selected Pass with selected Pass Type
								if (selectedPassTO.PassID >= 0)
								{
									selectedPassTO.PassTypeID = (int) cbPassType.SelectedValue;
									selectedPassTO.PairGenUsed = (int) Constants.PairGenUsed.Unused;

									currentExitPerm.EmployeeID = (int) lvEmployees.SelectedItems[0].Tag;
									currentExitPerm.PassTypeID = (int) cbPassType.SelectedValue;
									currentExitPerm.StartTime = selectedPassTO.EventTime;
									currentExitPerm.Offset = Int32.Parse(tbOffset.Text.Trim());
									currentExitPerm.Description = tbDesc.Text.Trim();									
									currentExitPerm.IssuedBy = logInUser.UserID;
									if (currentExitPerm.Used == (int) Constants.Used.Unverified)
										currentExitPerm.VerifiedBy = logInUser.UserID;
									currentExitPerm.Used = (int) Constants.Used.Yes;

									bool updatedRetroactive = new ExitPermission().UpdateRetroactive(currentExitPerm.PermissionID, currentExitPerm.EmployeeID, 
                                        currentExitPerm.PassTypeID, currentExitPerm.StartTime, 
										currentExitPerm.Offset, currentExitPerm.Used, currentExitPerm.Description,
										selectedPassTO, currentExitPerm.VerifiedBy);

									if (updatedRetroactive)
									{
										MessageBox.Show(rm.GetString("exitPermUpdated", culture));
										this.Close();
									}
								}
							}
							else
							{
								MessageBox.Show(rm.GetString("noPassesForExitPermFound", culture));
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void ExitPermissionsAdd_Load(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				// Initialize comparer object
				sortOrder = Constants.sortAsc;
				sortField = ExitPermissionsAdd.LastNameIndex;
				startIndex = 0;						

				wUnits = new List<WorkingUnitTO>();
				
				if (logInUser != null)
				{
					wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PermissionPurpose);
				}

				foreach (WorkingUnitTO wUnit in wUnits)
				{
					wuString += wUnit.WorkingUnitID.ToString().Trim() + ","; 
				}
				
				if (wuString.Length > 0)
				{
					wuString = wuString.Substring(0, wuString.Length - 1);
				}

				populateWorkingUnitCombo();

				/*string[] statuses = {Constants.statusActive, Constants.statusBlocked}; 
			
				ArrayList employeeList = new Employee().SearchByWUWithStatuses(wuString, statuses);

				if (employeeList.Count > 0)
				{
					populateListView(employeeList);
				}*/

				if (currentExitPerm.PermissionID >= 0)
				{
					EmployeeTO eTO = new Employee().Find(currentExitPerm.EmployeeID.ToString().Trim());
					cbWU.SelectedValue = eTO.WorkingUnitID;
					foreach(ListViewItem item in lvEmployees.Items)
					{
						if (((int) item.Tag) == currentExitPerm.EmployeeID)
						{
							item.Selected = true;
							lvEmployees.Select();
							break;
						}
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.Load(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void lvEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
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
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.lvEmployees_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
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
				log.writeLog(DateTime.Now + " ExitPermissions.btnPrev_Click(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " ExitPermissions.btnNext_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		public void setStartTime(DateTime startTime)
		{
			try
			{
				this.startTime = startTime;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.setStartTime(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		public void setFirstIN(DateTime firstIN)
		{
			try
			{
				this.firstIN = firstIN;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.setFirstIN(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void ExitPermissionsAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ExitPermissionsAdd.ExitPermissionsAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);

                if (cbWU.SelectedIndex <= 0)
                {
                    currentEmployeesList = new Employee().SearchByWUWithStatuses(wuString, statuses);
                }
                else
                {
                    string selectedWU = cbWU.SelectedValue.ToString();

                    //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                    WorkingUnit wu = new WorkingUnit();
                    if ((int)this.cbWU.SelectedValue != -1 && chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = wu.FindAllChildren(wuList);
                        selectedWU = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            selectedWU += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (selectedWU.Length > 0)
                        {
                            selectedWU = selectedWU.Substring(0, selectedWU.Length - 1);
                        }
                    }
                    currentEmployeesList = new Employee().SearchWithStatuses(statuses, selectedWU);
                }

                currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAdd.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpDateTime_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (dtpDateTime.Value.Date < DateTime.Now.Date)
                {
                    dtpDateTime.CustomFormat = "dd.MM.yyyy";
                    tbOffset.Visible = false;
                    lblOffset.Visible = false;
                    lblMin.Visible = false;
                    label3.Visible = false;
                }
                else
                {
                    dtpDateTime.CustomFormat = "dd.MM.yyyy HH:mm";
                    tbOffset.Visible = true;
                    lblOffset.Visible = true;
                    lblMin.Visible = true;
                    label3.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAdd.dtpDateTime_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        
	}
}
