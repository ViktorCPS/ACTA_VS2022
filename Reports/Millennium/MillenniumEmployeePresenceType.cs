using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace Reports.Millennium
{
	/// <summary>
	/// Summary description for WorkingUnitsReports.
	/// </summary>
	public class MillenniumEmployeePresenceType : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbWorkingUnit;
		private System.Windows.Forms.Label lblWorkingUnitName;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.CheckBox checkbPDF;
		private System.Windows.Forms.CheckBox checkbCSV;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblDocFormat;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.CheckBox chbHierarhicly;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;
		ArrayList TypesCells = new ArrayList();
		int[] types;
		string[] values;
		string[] valuesdays;
		int[] total;
		Dictionary<int, string> Types;
		List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();

		DebugLog debug;
		private System.Windows.Forms.CheckBox cbTXT;
		private System.Windows.Forms.CheckBox cbCR;
		private System.Windows.Forms.Panel panel;

		// Working Units that logInUser is granted to
		List<WorkingUnitTO> wUnits;
		string wuString;

		private System.Windows.Forms.Button btnGenerate;
        private GroupBox gbPassTypes;
        private CheckBox chbShowRetired;

		// Key is Type ID, value is its passtype
		Hashtable TypeRow =null;

		public MillenniumEmployeePresenceType()
		{
			InitializeComponent();
			
			// Init debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			Hashtable TypeRow = new Hashtable();

			// Language tool
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePresenceType).Assembly);
			setLanguage();
			logInUser = NotificationController.GetLogInUser();
			populateWorkigUnitCombo();
			
			DateTime date = DateTime.Now.Date;
			this.CenterToScreen();

			dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
			dtpToDate.Value = date;
			List<PassTypeTO> TypeListAll= new PassType().Search();
			List<PassTypeTO> TypeList = new List<PassTypeTO>();
			foreach(PassTypeTO ptMember in TypeListAll)
			{
				if ((ptMember.PassTypeID >= 0) && (ptMember.IsPass != 0))
				{
                    TypeList.Add(ptMember);
				}
			}
			
			populatePresenceTypeScreen(TypeList);
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
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitName = new System.Windows.Forms.Label();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblDocFormat = new System.Windows.Forms.Label();
            this.checkbPDF = new System.Windows.Forms.CheckBox();
            this.checkbCSV = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbTXT = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.panel = new System.Windows.Forms.Panel();
            this.gbPassTypes = new System.Windows.Forms.GroupBox();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbWorkingUnit.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbPassTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnitName);
            this.gbWorkingUnit.Location = new System.Drawing.Point(16, 24);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(456, 64);
            this.gbWorkingUnit.TabIndex = 0;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(344, 24);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(80, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(248, 21);
            this.cbWorkingUnit.TabIndex = 2;
            // 
            // lblWorkingUnitName
            // 
            this.lblWorkingUnitName.Location = new System.Drawing.Point(16, 24);
            this.lblWorkingUnitName.Name = "lblWorkingUnitName";
            this.lblWorkingUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnitName.TabIndex = 1;
            this.lblWorkingUnitName.Text = "Name:";
            this.lblWorkingUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(16, 96);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(456, 64);
            this.gbTimeInterval.TabIndex = 4;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Date Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(320, 24);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(280, 24);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(80, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(16, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(16, 176);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(104, 23);
            this.lblDocFormat.TabIndex = 9;
            this.lblDocFormat.Text = "Document Format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkbPDF
            // 
            this.checkbPDF.Enabled = false;
            this.checkbPDF.Location = new System.Drawing.Point(144, 176);
            this.checkbPDF.Name = "checkbPDF";
            this.checkbPDF.Size = new System.Drawing.Size(48, 24);
            this.checkbPDF.TabIndex = 10;
            this.checkbPDF.Text = "PDF";
            this.checkbPDF.Visible = false;
            // 
            // checkbCSV
            // 
            this.checkbCSV.Enabled = false;
            this.checkbCSV.Location = new System.Drawing.Point(208, 176);
            this.checkbCSV.Name = "checkbCSV";
            this.checkbCSV.Size = new System.Drawing.Size(48, 24);
            this.checkbCSV.TabIndex = 11;
            this.checkbCSV.Text = "CSV";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(16, 580);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 23);
            this.btnGenerate.TabIndex = 15;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(368, 580);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbTXT
            // 
            this.cbTXT.Enabled = false;
            this.cbTXT.Location = new System.Drawing.Point(280, 176);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(48, 24);
            this.cbTXT.TabIndex = 12;
            this.cbTXT.Text = "TXT";
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Enabled = false;
            this.cbCR.Location = new System.Drawing.Point(344, 176);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(56, 24);
            this.cbCR.TabIndex = 13;
            this.cbCR.Text = "CR";
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Location = new System.Drawing.Point(9, 16);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(456, 320);
            this.panel.TabIndex = 14;
            // 
            // gbPassTypes
            // 
            this.gbPassTypes.Controls.Add(this.panel);
            this.gbPassTypes.Location = new System.Drawing.Point(7, 232);
            this.gbPassTypes.Name = "gbPassTypes";
            this.gbPassTypes.Size = new System.Drawing.Size(473, 342);
            this.gbPassTypes.TabIndex = 17;
            this.gbPassTypes.TabStop = false;
            this.gbPassTypes.Text = "gbPassTypes";
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbShowRetired.Location = new System.Drawing.Point(208, 209);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 18;
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            // 
            // EmployeePresenceType
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 615);
            this.ControlBox = false;
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.gbPassTypes);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.checkbCSV);
            this.Controls.Add(this.checkbPDF);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbWorkingUnit);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 649);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "EmployeePresenceType";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Employee Presence by Type ";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeePresenceType_KeyUp);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbPassTypes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
			
		private void populatePresenceTypeScreen(List<PassTypeTO> TypeList)
		{
			try
			{	
				PassType pt = new PassType();
				Label TypeLabel = new Label();
				
				int i = 0;
				int j = 0;
				
				foreach(PassTypeTO ptype in TypeList)
				{
					TypeRow = new Hashtable();
					// Draw Types label
					
					TypeLabel = new Label();
					this.panel.Controls.Add(TypeLabel);
					TypeLabel.Text = ptype.Description;
					TypeLabel.Size = new Size(100, 26);
					TypeLabel.TextAlign = ContentAlignment.MiddleCenter;
					TypeLabel.Location = new Point(0+j, 26 * i + 50);

					// Draw Types cell
				
					TypesCell cell = new TypesCell(100+j, 26 * i + 50);
					
					cell.TypeID = ptype.PassTypeID.ToString();
						
					cell.setCheckBoxes();
					TypesCells.Add(cell);
					this.panel.Controls.Add(cell);
					

					TypeRow.Add(ptype.PassTypeID,ptype.Description);	
					i++;
					if (i >= 10) 
					{
						j=j+150;
						i=0;
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeePresenceType.populatePresenceTypeScreen(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		private void btnGenerate_Click(object sender, System.EventArgs e)
		{
			try
			{
				debug.writeLog(DateTime.Now + " EmployeePresenceType.btnGenerateReport_Click() \n");
				this.Cursor = Cursors.WaitCursor;
				Hashtable passTypeSummary = new Hashtable();

				if (wUnits.Count == 0)
				{
					MessageBox.Show(rm.GetString("noWUGranted", culture));
				}
				else
				{
					int selectedWorkingUnit = (int) cbWorkingUnit.SelectedValue;

					if (this.chbHierarhicly.Checked)
					{
						WorkingUnit wu = new WorkingUnit();
						if ( selectedWorkingUnit != -1 )
						{
                            wu.WUTO.WorkingUnitID = selectedWorkingUnit;
							wUnits = wu.Search();
						}
						else
						{
							if ( selectedWorkingUnit == -1 )
							{
								for ( int i=wUnits.Count-1; i>=0; i-- )
								{
									if (wUnits[i].WorkingUnitID == wUnits[i].ParentWorkingUID)
									{
										wUnits.RemoveAt(i);
									}
								}
							}
						}
						wUnits = wu.FindAllChildren(wUnits);
						selectedWorkingUnit = -1;
					}

					wuString = "";
					foreach (WorkingUnitTO wu in wUnits)
					{
						wuString += wu.WorkingUnitID.ToString().Trim() + ","; 
					}
				
					if (wuString.Length > 0)
					{
						wuString = wuString.Substring(0, wuString.Length - 1);
					}

					IOPair ioPair = new IOPair();
					int count = ioPair.SearchForWUCount(selectedWorkingUnit, wuString, dtpFromDate.Value, dtpToDate.Value);
					if (count > Constants.maxWUReportRecords)
					{
						this.Cursor = Cursors.Arrow;
						MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
						return;
					}
					else if (count > Constants.warningWUReportRecords)
					{
						this.Cursor = Cursors.Arrow;
						DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
						if (result.Equals(DialogResult.No))
						{
							return;
						}
					}

					this.Cursor = Cursors.WaitCursor;


                    List<IOPairTO> ioPairList = ioPair.SearchForWU(selectedWorkingUnit, wuString,
						dtpFromDate.Value, dtpToDate.Value);
					
					// Get Employees for selected Working Unit	
					Employee empl = new Employee();
					if (this.chbHierarhicly.Checked)
					{
						selectedEmployees = empl.SearchByWU(wuString);
					}
					else
					{
						selectedEmployees = empl.SearchByWU(cbWorkingUnit.SelectedValue.ToString());		
					}
                    if (!chbShowRetired.Checked)
                    {
                        List<EmployeeTO> emplList = selectedEmployees;
                        selectedEmployees = new List<EmployeeTO>();
                        foreach (EmployeeTO emp in emplList)
                        { 
                            if(!emp.Status.Equals(Constants.statusRetired))
                            {
                                selectedEmployees.Add(emp); 
                            }
                        }
                    }
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedule = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

					foreach(EmployeeTO currentEmployee in selectedEmployees)
					{
                        List<EmployeeTimeScheduleTO> emplTS = new EmployeesTimeSchedule().SearchEmployeesSchedules(currentEmployee.EmployeeID.ToString(), dtpFromDate.Value, dtpToDate.Value);

						if (!emplTimeSchedule.ContainsKey(currentEmployee.EmployeeID))
						{
                            emplTimeSchedule.Add(currentEmployee.EmployeeID, new List<EmployeeTimeScheduleTO>());
						}
					
						emplTimeSchedule[currentEmployee.EmployeeID] = emplTS;
					}

					Dictionary<int, List<IOPairTO>> classifiedPairs = new Dictionary<int,List<IOPairTO>>();
                    List<IOPairTO> clasifiedPairsList = new List<IOPairTO>();
					for (int i = 0; i < ioPairList.Count; i++)
					{
						int currentEmployeeID = ioPairList[i].EmployeeID;
						if (!classifiedPairs.ContainsKey(currentEmployeeID))
						{
                            classifiedPairs.Add(currentEmployeeID, new List<IOPairTO>());
						}

                        classifiedPairs[currentEmployeeID].Add(ioPairList[i]);
					}
					
					/*
						if (this.checkbPDF.Checked)
						{
							this.generateAnalyticalPDFReport(ioPairList);
						}
						*/
				
					// Summary report
					
					Hashtable PassTypeSummary = new Hashtable();
					Hashtable passTypesTotalTime = new Hashtable();

					Hashtable NumDays = new Hashtable();
					Hashtable emplNumDays = new Hashtable();

					// list of Time Schema for selected Employee and selected Time Interval
					ArrayList timeScheduleList = new ArrayList();

					// list of Time Schedule for one month
					ArrayList timeSchedule = new ArrayList();

					List<int> employeesId = new List<int>();
					foreach(EmployeeTO employ in selectedEmployees)
					{
						employeesId.Add(employ.EmployeeID);
					}
                    // TODO: go trough working unit
					//int count = ioPair.SearchForEmployeesCount(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);
																		
					// get Time Schemas for selected Employee and selected Time Interval
					DateTime date = dtpFromDate.Value.Date;

					// Key is Pass Type Id, Value is Pass Type Description
					Types = new Dictionary<int,string>();

					List<PassTypeTO> passTypesAll = new PassType().Search();
					foreach (PassTypeTO pt in passTypesAll)
					{
						Types.Add(pt.PassTypeID, pt.Description);
					}

					// Key is PassTypeID, Value is total time
					TimeSpan totalTime = new TimeSpan(0);	
				
					// Totals by PassType
					foreach(EmployeeTO currentEmployee in selectedEmployees)
					{						
						for (DateTime day = dtpFromDate.Value; day <= dtpToDate.Value; day = day.AddDays(1))
						{
							if (classifiedPairs.ContainsKey(currentEmployee.EmployeeID))
							{
								Hashtable daylyPassTypeSummary = new Hashtable();
								Hashtable daylyPassTypesTotalTime = new Hashtable();
								foreach(IOPairTO ioTO in classifiedPairs[currentEmployee.EmployeeID])
								{
									if (day == ioTO.IOPairDate)
									{
										totalTime = ioTO.EndTime.Subtract(ioTO.StartTime);				
							
										if (!daylyPassTypeSummary.ContainsKey(currentEmployee.EmployeeID))
										{
											daylyPassTypeSummary.Add(currentEmployee.EmployeeID, new Hashtable());
										}
							
										daylyPassTypesTotalTime = (Hashtable) daylyPassTypeSummary[currentEmployee.EmployeeID];

										if (daylyPassTypesTotalTime.ContainsKey(ioTO.PassTypeID))
										{
											daylyPassTypesTotalTime[ioTO.PassTypeID] = ((TimeSpan) daylyPassTypesTotalTime[ioTO.PassTypeID]).Add(totalTime);	
										}
										else
										{
											daylyPassTypesTotalTime.Add(ioTO.PassTypeID, totalTime);
										}

										daylyPassTypeSummary[currentEmployee.EmployeeID] = daylyPassTypesTotalTime;


										if (!PassTypeSummary.ContainsKey(currentEmployee.EmployeeID))
										{
											PassTypeSummary.Add(currentEmployee.EmployeeID, new Hashtable());
										}
							
										passTypesTotalTime = (Hashtable) PassTypeSummary[currentEmployee.EmployeeID];

										if (passTypesTotalTime.ContainsKey(ioTO.PassTypeID))
										{
											passTypesTotalTime[ioTO.PassTypeID] = ((TimeSpan) passTypesTotalTime[ioTO.PassTypeID]).Add(totalTime);	
										}
										else
										{
											passTypesTotalTime.Add(ioTO.PassTypeID, totalTime);
										}

										PassTypeSummary[currentEmployee.EmployeeID] = passTypesTotalTime;

									}
								}
								
								if (emplTimeSchedule.ContainsKey(currentEmployee.EmployeeID))
								{
									foreach (PassTypeTO pt in passTypesAll)
									{
										if (daylyPassTypesTotalTime.ContainsKey(pt.PassTypeID))
										{

											if (!NumDays.ContainsKey(currentEmployee.EmployeeID))
											{
												NumDays.Add(currentEmployee.EmployeeID, new Hashtable());
											}
							
											emplNumDays = (Hashtable) NumDays[currentEmployee.EmployeeID];

											if (emplNumDays.ContainsKey(pt.PassTypeID))
											{
												emplNumDays[pt.PassTypeID] = (int) (emplNumDays[pt.PassTypeID])+1;	
											}
											else
											{
												emplNumDays.Add(pt.PassTypeID, 1);
											}

											NumDays[currentEmployee.EmployeeID] = emplNumDays;
										}
									}
								}
							}
						}
					}
					if (PassTypeSummary.Count == 0)
					{
						MessageBox.Show(rm.GetString("dataNotFound", culture));
						return;
					}

					if (passTypeSummary.Count > Constants.maxWUReportRecords)
					{
						this.Cursor = Cursors.Arrow;
						MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
						return;
					}
					else if (passTypeSummary.Count > Constants.warningWUReportRecords)
					{
						this.Cursor = Cursors.Arrow;
						DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
						if (result.Equals(DialogResult.No))
						{
							return;
						}
					}
						
					if (this.cbCR.Checked)
					{
						DataSet dataSet = new DataSet();
                        DataTable table = new DataTable("loc_rep");

						table.Columns.Add("date", typeof(System.String));
						table.Columns.Add("last_name", typeof(System.String));
						table.Columns.Add("first_name", typeof(System.String));
						table.Columns.Add("location", typeof(System.String));
						table.Columns.Add("start", typeof(System.String));
						table.Columns.Add("end", typeof(System.String));
						table.Columns.Add("type", typeof(System.String));
						table.Columns.Add("total_time", typeof(System.String));			
						table.Columns.Add("working_unit", typeof(System.String));			
						table.Columns.Add("employee_id", typeof(int));
                        table.Columns.Add("imageID", typeof(byte));

                        DataTable tableI = new DataTable("images");
                        tableI.Columns.Add("image", typeof(System.Byte[]));
                        tableI.Columns.Add("imageID", typeof(byte));

                        int counter = 0;

						DataRow row = table.NewRow();

						// Key is Pass Type Id, Value is Pass Type Description
						List<PassTypeTO> passTypes = new List<PassTypeTO>();
						
						int countpt = passTypesAll.Count;
						types = new int[countpt];
						values = new string[countpt];
						valuesdays = new string[countpt];
						total = new int[countpt];

						foreach(TypesCell cell in TypesCells)
						{
							cell.getCheckBoxes();
				
							int i = 0;
							foreach(PassTypeTO ptmember in passTypesAll)
							{	
						
								if ((cell.HasType == 1) && (cell.TypeID == ptmember.PassTypeID.ToString()))
								{
									passTypes.Add(ptmember);
								}
						
								types[i]=ptmember.PassTypeID;
								i++;
							}
						}
						if (passTypes.Count>7)
						{
							MessageBox.Show(rm.GetString("numberPassTypes", culture));
							return;
						}
                        if (passTypes.Count == 0)
                        { 
                            MessageBox.Show(rm.GetString("selectAtLeastOnePassType", culture));
                            return;
                        }

						Hashtable currentHT = new Hashtable();
						Hashtable currentHTDays = new Hashtable();
			
						string minutes = "";
						string h ="";
						string[] typesheader;
						typesheader=new string[countpt];

						for(int i=0;i<countpt;i++)
						{
							foreach(PassTypeTO ptmember in passTypes)
							{	
								if (types[i] == ptmember.PassTypeID) 
								{	
									if (Types.ContainsKey(types[i]))
									{
										string buf = (string) Types[types[i]];
										typesheader[i]=buf;
								
									}
									if (h=="")
									{
										h = typesheader[i];
									}
									else
									{
										h = h +","+ typesheader[i];
									}	
								}
							}		
						}

						string emplString = "";
						foreach (EmployeeTO employee in selectedEmployees)
						{
							emplString += employee.EmployeeID.ToString().Trim() + ","; 
						}
				
						if (emplString.Length > 0)
						{
							emplString = emplString.Substring(0, emplString.Length - 1);
						}

                        List<EmployeeTimeScheduleTO> emplTS = new EmployeesTimeSchedule().SearchEmployeesSchedules(emplString, dtpFromDate.Value, dtpToDate.Value);
			
						foreach(EmployeeTO currentEmployee in selectedEmployees)
						{
							string s="";
							string d="";
					
							for(int i=0;i<countpt;i++)
							{
								values[i] = null;
							}	
					
							for(int i=0;i<countpt;i++)
							{
								valuesdays[i] = null;
							}	
							
							IOPair iopair = new IOPair();
							
							foreach(EmployeeTO employ in selectedEmployees)
							{
                               	employeesId.Add(employ.EmployeeID);
							}

                            List<IOPairTO> ioPairListAll = iopair.SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, employeesId, -1);
							
							if (PassTypeSummary.ContainsKey(currentEmployee.EmployeeID))
							{
								currentHT = (Hashtable) PassTypeSummary[currentEmployee.EmployeeID];
								currentHTDays = (Hashtable) NumDays[currentEmployee.EmployeeID];
						
								row = table.NewRow();
								row["employee_id"] = currentEmployee.EmployeeID;
								row["last_name"] = currentEmployee.LastName;
								row["first_name"] = currentEmployee.FirstName;
						
								for(int i=0;i<countpt;i++)
								{
									if ((currentHT.ContainsKey(types[i]))&& (currentHTDays.ContainsKey(types[i])))
									{
										TimeSpan totalSpan = (TimeSpan) currentHT[types[i]];
                                        WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(currentEmployee.EmployeeID, date, emplTS);
                                        TimeSpan  expectedDuaration = currentInterval.EndTime.TimeOfDay - currentInterval.StartTime.TimeOfDay;
                                        if (expectedDuaration.TotalMinutes == 0)
                                        {
                                            expectedDuaration = new TimeSpan(8, 0, 0);
                                        }
                                        //do not show more than expected hours for regular work
                                        if((types[i] == 0)&&(totalSpan > expectedDuaration ))
                                        {
                                            totalSpan = expectedDuaration;
                                        }
										int numDays = (int)currentHTDays[types[i]];
										if (totalSpan.Minutes < 10) 
										{
											minutes = "0" + totalSpan.Minutes.ToString();
										}
										else
										{
											minutes = totalSpan.Minutes.ToString();						
										}
										valuesdays[i] = numDays.ToString();
										values[i] = (totalSpan.Days * 24 + totalSpan.Hours).ToString() + ":" + minutes;	
									}							
								}
								for(int i=0;i<countpt;i++)
								{
									foreach(PassTypeTO ptmember in passTypes)
									{	
										if (types[i] == ptmember.PassTypeID) 
										{
											if (s=="")
											{
												if (values[i]==null)
												{
													string buf = "00:00";
													s = s + buf;
												}
												else
												{
													string buf1 = values[i];
													s = s + buf1;
												}
												//break;
											}
											else
											{
												if (values[i]==null)
												{
													string buf = "00:00";
													s = s + "," + buf;
												}
												else
												{
													string buf1 = values[i];
													s = s + "," + buf1;
												}
												//break;
											}
											
											if (d=="")
											{
												if (valuesdays[i]==null)
												{
													string buf = "0";
													d = d + buf;
												}
												else
												{
													string buf1 = valuesdays[i];
													d = d + buf1;
												}
												//break;
											}
											else
											{
												if (valuesdays[i]==null)
												{
													string buf = "0";
													d = d + "," + buf;
												}
												else
												{
													string buf1 = valuesdays[i];
													d = d + "," + buf1;
												}
												//break;
											}
										}
									}		
								}
					
								row["location"]=s;
								row["total_time"]=d;
								row["type"]=h; 
								
								row["working_unit"] = currentEmployee.WorkingUnitName;

                                row["imageID"] = 1;
                                if (counter == 0)
                                {
                                    //add logo image just once
                                    DataRow rowI = tableI.NewRow();
                                    rowI["image"] = Constants.LogoForReport;
                                    rowI["imageID"] = 1;
                                    tableI.Rows.Add(rowI);
                                    tableI.AcceptChanges();
                                }

								table.Rows.Add(row);

                                counter++;
							}
						}
						table.AcceptChanges();
						dataSet.Tables.Add(table);
                        dataSet.Tables.Add(tableI);

						if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
						{
                            Millennium_sr.MillenniumEmployeePresenceTypeCRView_sr view = new Millennium_sr.MillenniumEmployeePresenceTypeCRView_sr(
								dataSet, dtpFromDate.Value, dtpToDate.Value);

							view.ShowDialog(this);
					
						}
						else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
						{
                            Millennium_en.MillenniumEmployeePresenceTypeCRView_en view = new Millennium_en.MillenniumEmployeePresenceTypeCRView_en(
								dataSet, dtpFromDate.Value, dtpToDate.Value);

							view.ShowDialog(this);
						}
					}

					//this.Close();
				}
				
				/*else
				{
					if (cbWorkingUnit.SelectedIndex == 0)
					{
						MessageBox.Show(rm.GetString("noWUSelected", culture));
						return;
					}
					

					if (passTypeSummary.Count > Constants.maxWUReportRecords)
					{
						this.Cursor = Cursors.Arrow;
						MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
						return;
					}
					else if (passTypeSummary.Count > Constants.warningWUReportRecords)
					{
						this.Cursor = Cursors.Arrow;
						DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
						if (result.Equals(DialogResult.No))
						{
							return;
						}
					}

					/*
					if (this.checkbPDF.Checked)
					{
						this.generateSummaryPDFReport(passTypeSummary);
					}
					*/
					
				/*	if (this.cbCR.Checked)
					{
						this.generateSummaryCRReport(passTypeSummary);
					}

					this.Close();
				}*/
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeePresenceType.btnGenerateReport_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}
        public WorkTimeIntervalTO getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList)
        {
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();

            int timeScheduleIndex = -1;

            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }

            if (timeScheduleIndex >= 0)
            {
                int cycleDuration = 0;
                int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
                int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
                TimeSchema sch = new TimeSchema();
                sch.TimeSchemaTO.TimeSchemaID = schemaID;
                List<WorkTimeSchemaTO> timeSchema = sch.Search();

                WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                if (timeSchema.Count > 0)
                {
                    schema = timeSchema[0];
                    cycleDuration = schema.CycleDuration;
                }

                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                //TimeSpan days = date - ((EmployeesTimeSchedule) (timeScheduleList[timeScheduleIndex])).Date;
                //interval = (TimeSchemaInterval) ((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);
                interval = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration][0];
            }

            return interval;
        }

		private void populateWorkigUnitCombo()
		{
			try
			{
				List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

				if (logInUser != null)
				{
					wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
				}

				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeePresenceType.populateWorkigUnitCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}	

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("employeePresenceTypeReport", culture);
				gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
				lblWorkingUnitName.Text = rm.GetString("lblName", culture);
				chbHierarhicly.Text = rm.GetString("hierarchically", culture);
				lblFrom.Text = rm.GetString("lblFrom", culture);
				gbTimeInterval.Text = rm.GetString("timeInterval", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
				lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
				btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
                gbPassTypes.Text = rm.GetString("gbPassTypes", culture);
                chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
				
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeePresenceType.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void chbHierarhicly_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

        private void EmployeePresenceType_KeyUp(object sender, KeyEventArgs e)
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }		
	}
}

