using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using Util;
using Common;
using TransferObjects;

using System.Resources;
using System.Globalization;

namespace UI
{
	/// <summary>
	/// Summary description for ProlongTS.
	/// </summary>
	public class ProlongTS : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblWorkGroup;
		private System.Windows.Forms.ComboBox cbWorkGroup;
		private System.Windows.Forms.Label lblMonth;
		private System.Windows.Forms.DateTimePicker dtpMonth;
		private System.Windows.Forms.Button btnProlongTS;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CultureInfo culture;
		ResourceManager rm;

		DebugLog log;
        private Button btnAutoClosePairs;
        private GroupBox gbProlongTS;
        private CheckBox chbAutoClose;

		ApplUser currentUser = null;

		public ProlongTS()
		{
			InitializeComponent();
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentUser = new ApplUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource", typeof(ProlongTS).Assembly);
			setLanguage();

			dtpMonth.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			populateWGCombo();

            btnAutoClosePairs.Enabled = false;
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
            this.lblWorkGroup = new System.Windows.Forms.Label();
            this.cbWorkGroup = new System.Windows.Forms.ComboBox();
            this.lblMonth = new System.Windows.Forms.Label();
            this.dtpMonth = new System.Windows.Forms.DateTimePicker();
            this.btnProlongTS = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAutoClosePairs = new System.Windows.Forms.Button();
            this.gbProlongTS = new System.Windows.Forms.GroupBox();
            this.chbAutoClose = new System.Windows.Forms.CheckBox();
            this.gbProlongTS.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWorkGroup
            // 
            this.lblWorkGroup.Location = new System.Drawing.Point(6, 24);
            this.lblWorkGroup.Name = "lblWorkGroup";
            this.lblWorkGroup.Size = new System.Drawing.Size(100, 23);
            this.lblWorkGroup.TabIndex = 0;
            this.lblWorkGroup.Text = "Work group:";
            this.lblWorkGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWorkGroup
            // 
            this.cbWorkGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkGroup.Location = new System.Drawing.Point(118, 24);
            this.cbWorkGroup.Name = "cbWorkGroup";
            this.cbWorkGroup.Size = new System.Drawing.Size(192, 21);
            this.cbWorkGroup.TabIndex = 1;
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(6, 56);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(100, 23);
            this.lblMonth.TabIndex = 2;
            this.lblMonth.Text = "Month:";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpMonth
            // 
            this.dtpMonth.CustomFormat = "MMM, yyy";
            this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonth.Location = new System.Drawing.Point(118, 56);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.ShowUpDown = true;
            this.dtpMonth.Size = new System.Drawing.Size(80, 20);
            this.dtpMonth.TabIndex = 3;
            // 
            // btnProlongTS
            // 
            this.btnProlongTS.Location = new System.Drawing.Point(6, 96);
            this.btnProlongTS.Name = "btnProlongTS";
            this.btnProlongTS.Size = new System.Drawing.Size(200, 23);
            this.btnProlongTS.TabIndex = 4;
            this.btnProlongTS.Text = "Prolong time schedule";
            this.btnProlongTS.Click += new System.EventHandler(this.btnProlongTS_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(267, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAutoClosePairs
            // 
            this.btnAutoClosePairs.Location = new System.Drawing.Point(142, 170);
            this.btnAutoClosePairs.Name = "btnAutoClosePairs";
            this.btnAutoClosePairs.Size = new System.Drawing.Size(200, 23);
            this.btnAutoClosePairs.TabIndex = 2;
            this.btnAutoClosePairs.Text = "Auto close pairs";
            this.btnAutoClosePairs.Click += new System.EventHandler(this.btnAutoClosePairs_Click);
            // 
            // gbProlongTS
            // 
            this.gbProlongTS.Controls.Add(this.lblWorkGroup);
            this.gbProlongTS.Controls.Add(this.cbWorkGroup);
            this.gbProlongTS.Controls.Add(this.lblMonth);
            this.gbProlongTS.Controls.Add(this.btnProlongTS);
            this.gbProlongTS.Controls.Add(this.dtpMonth);
            this.gbProlongTS.Location = new System.Drawing.Point(12, 12);
            this.gbProlongTS.Name = "gbProlongTS";
            this.gbProlongTS.Size = new System.Drawing.Size(330, 141);
            this.gbProlongTS.TabIndex = 0;
            this.gbProlongTS.TabStop = false;
            this.gbProlongTS.Text = "Prolong TS";
            // 
            // chbAutoClose
            // 
            this.chbAutoClose.AutoSize = true;
            this.chbAutoClose.Location = new System.Drawing.Point(12, 174);
            this.chbAutoClose.Name = "chbAutoClose";
            this.chbAutoClose.Size = new System.Drawing.Size(101, 17);
            this.chbAutoClose.TabIndex = 1;
            this.chbAutoClose.Text = "Auto close pairs";
            this.chbAutoClose.UseVisualStyleBackColor = true;
            this.chbAutoClose.CheckedChanged += new System.EventHandler(this.chbAutoClose_CheckedChanged);
            // 
            // ProlongTS
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(357, 258);
            this.ControlBox = false;
            this.Controls.Add(this.chbAutoClose);
            this.Controls.Add(this.gbProlongTS);
            this.Controls.Add(this.btnAutoClosePairs);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "ProlongTS";
            this.ShowInTaskbar = false;
            this.Text = "Prolonging time schedule";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ProlongTS_KeyUp);
            this.gbProlongTS.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("ProlongTSForm", culture);

				// button's text
				btnProlongTS.Text = rm.GetString("btnProlongTS", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
                btnAutoClosePairs.Text = rm.GetString("btnAutoClosePairs", culture);

				// label's text
				lblWorkGroup.Text = rm.GetString("lblGroup", culture);
				lblMonth.Text = rm.GetString("lblMonth", culture);

                // group box's text
                gbProlongTS.Text = rm.GetString("gbProlongTS", culture);

                // check box's text
                chbAutoClose.Text = rm.GetString("chbAutoClose", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ProlongTS.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}

		}

		private void populateWGCombo()
		{
			try
			{				
				List<WorkingGroupTO> wgArray = new WorkingGroup().Search();
				wgArray.Insert(0, new WorkingGroupTO(-1, rm.GetString("all", culture), rm.GetString("all", culture)));

				cbWorkGroup.DataSource = wgArray;
				cbWorkGroup.DisplayMember = "GroupName";
				cbWorkGroup.ValueMember = "EmployeeGroupID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ProlongTS.populateWGCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " ProlongTS.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnProlongTS_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				DateTime month = dtpMonth.Value;

				// find all employees from selected working group
                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);

				List<EmployeeTO> employees = new List<EmployeeTO>();
				if (cbWorkGroup.SelectedIndex <= 0)
				{				
					employees = new Employee().SearchWithStatuses(statuses, "");
				}
				else
				{
                    Employee empl = new Employee();
                    empl.EmplTO.WorkingGroupID = (int)cbWorkGroup.SelectedValue;
					employees = empl.SearchWithStatuses(statuses, "");
				}
				
				// find time schedules for selected month
				EmployeesTimeSchedule ets = new EmployeesTimeSchedule();
                List<EmployeeTimeScheduleTO> monthTimeSchedule = ets.SearchEmployeeMonthTimeSchedules(month);

                List<EmployeeTimeScheduleTO> prevMonthTimeSchedule = ets.SearchEmployeeMonthTimeSchedules(month.AddMonths(-1));

                string pauseEmployees = "";

				// set time schedule to each employee
				foreach (EmployeeTO empl in employees)
				{
					EmployeeTimeScheduleTO ts = new EmployeeTimeScheduleTO();

					for (int i = 0; i < monthTimeSchedule.Count; i++)
					{
						if (monthTimeSchedule[i].EmployeeID == empl.EmployeeID)
						{
							ts = monthTimeSchedule[i];
							break;
						}
					}

					// set new time schedule only if old one does not exist
					if (ts.EmployeeID < 0)
					{
						// find time schedule defined for last month
						for (int i = 0; i < prevMonthTimeSchedule.Count; i++)
						{
							if (prevMonthTimeSchedule[i].EmployeeID == empl.EmployeeID)
							{
								ts = prevMonthTimeSchedule[i];
								break;
							}
						}
					
						// set time schedule to selected month as it was in last month
						if (ts.EmployeeID >= 0)
						{
                            TimeSchema sch = new TimeSchema();
                            sch.TimeSchemaTO.TimeSchemaID = ts.TimeSchemaID;
							List<WorkTimeSchemaTO> schemas = sch.Search();
							DateTime firstDay = new DateTime(month.Year, month.Month, 1);
							if (schemas.Count > 0)
							{
								WorkTimeSchemaTO schema = schemas[0];

								int startDay = (ts.StartCycleDay + ((int) ((TimeSpan) firstDay.Subtract(ts.Date)).TotalDays)) % schema.CycleDuration;
                                                                
								ets.Save(empl.EmployeeID, firstDay, ts.TimeSchemaID, startDay);
							}

							// delete absences pairs for that month and update absences to unused
							deleteIOPUpdateEA(empl.EmployeeID, firstDay);

                            pauseEmployees += empl.EmployeeID.ToString().Trim() + ",";
						}
					}
				}

                if (pauseEmployees.Length > 0)
                {
                    pauseEmployees = pauseEmployees.Substring(0, pauseEmployees.Length - 1);
                }
                if (!pauseEmployees.Equals(""))
                {
                    IOPair ioPair = new IOPair();
                    if ((month.Year < DateTime.Now.Year) ||
                        ((month.Year == DateTime.Now.Year) && (month.Month <= DateTime.Now.Month)))
                        ioPair.recalculatePause(pauseEmployees, new DateTime(month.Year, month.Month, 1), (new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1)).AddDays(-1), null);
                }
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ProlongTS.btnProlongTS_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		// delete absences pairs for that month and update absences to unused
		private void deleteIOPUpdateEA(int employeeID, DateTime date)
		{
			try
			{
				DateTime start = new DateTime(date.Year, date.Month, 1);
				DateTime end = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).AddDays(-1);

                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO.EmployeeID = employeeID;
                ea.EmplAbsTO.DateStart = start;
                ea.EmplAbsTO.DateEnd = end;
				List<EmployeeAbsenceTO> emplAbsences = ea.Search("");

				foreach (EmployeeAbsenceTO abs in emplAbsences)
				{
					new EmployeeAbsence().UpdateEADeleteIOP(abs.RecID, abs.EmployeeID, abs.PassTypeID,
						abs.PassTypeID, abs.DateStart, abs.DateEnd, abs.DateStart, abs.DateEnd, (int) Constants.Used.No, null);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ProlongTS.deleteIOPUpdateEA(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void ProlongTS_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ProlongTS.ProlongTS_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAutoClosePairs_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                new IOPair().autoCloseTodayPairs();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ProlongTS.btnAutoClosePairs_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbAutoClose_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (chbAutoClose.Checked)
                {
                    gbProlongTS.Enabled = false;
                    btnAutoClosePairs.Enabled = true;
                }
                else
                {
                    gbProlongTS.Enabled = true;
                    btnAutoClosePairs.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ProlongTS.chbAutoClose_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
