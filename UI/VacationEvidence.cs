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
using Reports;

namespace UI
{
    public partial class VacationEvidence : Form
    {
        private List<WorkingUnitTO> wUnits;
        private string wuString = "";
        private CultureInfo culture;
		private ResourceManager rm;
		DebugLog log;
        ApplUserTO logInUser;
        private int startIndex;
        List<EmployeeTO> currnetEmplArray;
        List<EmployeeVacEvidTO> currentVacations;

        //allUsers, Key is userID, value is UserName
        Dictionary<string, string> users = new Dictionary<string, string>();

        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();
        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

        // List View indexes
		const int WUIndex = 0;
		const int EmplNameIndex = 1;
		const int YearIndex = 2;
		const int NumOFDaysIndex = 3;
		const int FromLastYearIndex = 4;
		const int TotalIndex = 5;
		const int UsedIndex = 6;
		const int LeftIndex = 7;

        //sorting properties
        private int sortOrder;
        private int sortField;
        
        //datetime format
        const string dateTimeFormat = "dd.MM.yyyy HH:mm:ss";

        Filter filter;

        public VacationEvidence()
        {
            InitializeComponent();
            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(VacationEvidence).Assembly);
            logInUser = NotificationController.GetLogInUser();
            currnetEmplArray = new List<EmployeeTO>();
            currentVacations = new List<EmployeeVacEvidTO>();

            ApplUser au = new ApplUser();
            List<ApplUserTO> auArray = au.Search();
            foreach (ApplUserTO user in auArray)
            {
                if (!users.ContainsKey(user.UserID))
                    users.Add(user.UserID, user.Name);
            }

            setLanguage();
        }
        #region MDI child method's        
        public void MDIchangeSelectedEmployee(int selectedWU, int selectedEmployeeID, DateTime from, DateTime to, bool check)
        {
            try
            {
                chbHierarhicly.Checked = check;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (wu.WorkingUnitID == selectedWU)
                        cbWU.SelectedValue = selectedWU;
                }

                foreach (EmployeeTO empl in currnetEmplArray)
                {
                    if (empl.EmployeeID == selectedEmployeeID)
                        cbEmployee.SelectedValue = selectedEmployeeID;
                }
                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "  VacationEvidence.changeSelectedEmployee(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
       
        #endregion
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
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
                btnClose.Text = rm.GetString("btnClose", culture);
                btnDetailes.Text = rm.GetString("btnDetailes", culture);
                btnGenerate.Text = rm.GetString("btnGenerate", culture);
                btnUsing.Text = rm.GetString("btnUsing", culture);

                // Form name
                this.Text = rm.GetString("vacationEvidence", culture);

                // group box text
                this.gbApprovedDays.Text = rm.GetString("gbApprovedDays", culture);
                gbDaysLeft.Text = rm.GetString("gbDaysLeft", culture);
                gbSearch.Text = rm.GetString("gbSearch", culture);
                gbUsedDays.Text = rm.GetString("gbUsedDays", culture);
                gbReport.Text = rm.GetString("btnReport", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                
                // check box text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                chbIncludePreviously.Text = rm.GetString("chbIncludePreviously", culture);
                chbIncludeDetails.Text = rm.GetString("chbIncludeDetails", culture);
                               
                // label's text
                lblWU.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblFromApproved.Text = rm.GetString("lblFrom", culture);
                lblFromLeft.Text = rm.GetString("lblFrom", culture);
                lblFromUsed.Text = rm.GetString("lblFrom", culture);
                lblToApproved.Text = rm.GetString("lblTo", culture);
                lblToLeft.Text = rm.GetString("lblTo", culture);
                lblToUsed.Text = rm.GetString("lblTo", culture);
                lblYear.Text = rm.GetString("lblFrom", culture);
                lblTotal.Text = rm.GetString("lblTotal", culture);
                lblForCurrentYear.Text = rm.GetString("lblForCurrentYear", culture);
                lblEnter.Text = rm.GetString("lblEnter", culture);
                lblLastModification.Text = rm.GetString("lblLastModification", culture);

                // list view initialization
                lvVacations.BeginUpdate();
                lvVacations.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvVacations.Width - 10) / 8+30, HorizontalAlignment.Left);
                lvVacations.Columns.Add(rm.GetString("hdrEmployee", culture), (lvVacations.Width - 10) / 8 +30, HorizontalAlignment.Left);
                lvVacations.Columns.Add(rm.GetString("hdrYear", culture), (lvVacations.Width - 10) / 8-20, HorizontalAlignment.Left);
                lvVacations.Columns.Add(rm.GetString("hdrApproved", culture), (lvVacations.Width - 10) / 8-15 , HorizontalAlignment.Left);
                lvVacations.Columns.Add(rm.GetString("hdrTransposed", culture), (lvVacations.Width - 10) / 8 -15, HorizontalAlignment.Left);
                lvVacations.Columns.Add(rm.GetString("hdrTotalForUsing", culture), (lvVacations.Width - 10) / 8 , HorizontalAlignment.Left);
                lvVacations.Columns.Add(rm.GetString("hdrUsedDays", culture), (lvVacations.Width - 10) / 8-10, HorizontalAlignment.Left);
                lvVacations.Columns.Add(rm.GetString("hdrLeft", culture), (lvVacations.Width - 10) / 8-10, HorizontalAlignment.Left);
                lvVacations.EndUpdate();
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.setLanguage(): " + ex.Message + "\n");
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

                cbWU.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " VacationEvidence.populateWorkingUnitCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void VacationEvidence_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //geting working units for user
                wUnits = new List<WorkingUnitTO>();
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.VacationPurpose);
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
                populateEmployeeCombo(-1);

                // get all holidays
                List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

                foreach (HolidayTO holiday in holidayList)
                {
                    holidays.Add(holiday.HolidayDate, holiday);
                }

                // get all time schemas
                timeSchemas = new TimeSchema().Search();

                sortOrder = Constants.sortAsc;
                sortField = VacationEvidence.EmplNameIndex;
                startIndex = 0;

                this.btnPrev.Visible = false;
                this.btnNext.Visible = false;
                lblTotal.Visible = false;


                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.VacationEvidence_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWithoutPrev_Click(object sender, EventArgs e)
        {             
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnWithoutPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }        
        }

        private void btnWithoutNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnWithoutNext_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " VacationEvidence.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
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

                if (cbWU.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                currnetEmplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    currnetEmplArray = new Employee().SearchByWU(wuString);
                }
                else
                {
                    if (chbHierarhicly.Checked)
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
                            wuList = workUnit.FindAllChildren(wuList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }
                    }
                    currnetEmplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in currnetEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currnetEmplArray.Insert(0, empl);

                cbEmployee.DataSource = currnetEmplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string employeeID = "";
                currentVacations = new List<EmployeeVacEvidTO>();
                //period for selected year
                DateTime yearFrom = new DateTime(dtpYearFrom.Value.AddYears(-1).Year, 1, 1, 0, 0, 0);
                DateTime yearTo = new DateTime(dtpYearFrom.Value.Year, 1, 1, 0, 0, 0);
                
                int numdaysMin = (int)nudFromApproved.Value;
                int numDaysMax = (int)nudToApproved.Value;

                if (cbEmployee.SelectedIndex != 0)
                {
                    employeeID = cbEmployee.SelectedValue.ToString();
                }
                else
                {
                    foreach (EmployeeTO employee in currnetEmplArray)
                    {
                        employeeID += " '" + employee.EmployeeID.ToString() + "' , ";
                    }
                    if (employeeID.Length > 0)
                    {
                        employeeID = employeeID.Substring(0, employeeID.Length - 2);
                    }
                }

                //list of vacationEvidences for selected employees and selected year and one year before
                List<EmployeeVacEvidTO> vactionlist = new List<EmployeeVacEvidTO>();
                
                int count = new EmployeeVacEvid().getVacationsCount(employeeID, yearFrom, yearTo, numdaysMin, numDaysMax);
                if (count > Constants.maxRecords)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("passesGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        vactionlist = new EmployeeVacEvid().getVacations(employeeID, yearFrom, yearTo, numdaysMin, numDaysMax);
                    }
                    else
                    {
                        clearListView();
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        vactionlist = new EmployeeVacEvid().getVacations(employeeID, yearFrom, yearTo, numdaysMin, numDaysMax);

                       //geting employeeAbsences with passType = vacation to calculate used vacations for each employee
                        EmployeeAbsence ea = new EmployeeAbsence();
                        ea.EmplAbsTO.PassTypeID = Constants.vacation;
                        List<EmployeeAbsenceTO> absencesList = ea.SearchForVacEvid(employeeID, "", yearFrom, yearTo);

                        Dictionary<int, List<EmployeeAbsenceTO>> emplAbsences = getAbsenceTable(absencesList);

                        foreach (EmployeeVacEvidTO vacation in vactionlist)
                        {
                            vacation.LastName += " " + vacation.FirstName;
                            vacation.FromLastYear = 0;

                            List<EmployeeAbsenceTO> abcsecnesForEmpl = new List<EmployeeAbsenceTO>();
                            if (emplAbsences.ContainsKey(vacation.EmployeeID))
                            {
                                abcsecnesForEmpl = emplAbsences[vacation.EmployeeID];
                            }
                            //from year vacation plan and absences count real used days and total time to use
                            EmployeeVacEvidTO vacTemp = Common.Misc.getVacationWithAbsences(vacation, abcsecnesForEmpl, holidays, timeSchemas);
                            vacation.DaysLeft = vacTemp.DaysLeft;
                            vacation.UsedDays = vacTemp.UsedDays;

                            //count for year before 
                            foreach (EmployeeVacEvidTO vacEvid in vactionlist)
                            {
                                if (vacEvid.EmployeeID == vacation.EmployeeID && vacEvid.VacYear.Year == vacation.VacYear.AddYears(-1).Year && vacEvid.ValidTo >= DateTime.Now)
                                {
                                    //from year vacation plan and absences conunt real used days and total time to use
                                    EmployeeVacEvidTO vacTemp1 = Common.Misc.getVacationWithAbsences(vacEvid, abcsecnesForEmpl, holidays, timeSchemas);
                                    vacEvid.DaysLeft = vacTemp1.DaysLeft;
                                    vacEvid.UsedDays = vacTemp1.UsedDays;

                                    TimeSpan ts = vacEvid.ValidTo.Date.Subtract(DateTime.Now);
                                    vacation.FromLastYear = Math.Min(vacEvid.DaysLeft, ts.Days);
                                }
                            }

                            if (vacation.DaysLeft >= 0 && vacation.FromLastYear >= 0)
                            {
                                vacation.TotalForUse = vacation.DaysLeft + vacation.FromLastYear;
                            }

                            if (!chbIncludePreviously.Checked && vacation.VacYear.Year != dtpYearFrom.Value.Year)
                                continue;
                            if (vacation.UsedDays >= (int)nudFromUsed.Value && vacation.UsedDays <= (int)nudToUsed.Value && vacation.DaysLeft >= (int)nudFromLeft.Value && vacation.DaysLeft <= (int)nudToLeft.Value)
                            {
                                currentVacations.Add(vacation);
                            }
                        }
                    }
                    else
                    {
                        clearListView();
                    }
                }
                clearListView();
                
                currentVacations.Sort(new ArrayListSort(Constants.sortAsc, EmplNameIndex));
                populateListView(currentVacations, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnSearch_Click(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }       

        //geting hashtable with employee ID as key and value is array list of absences
        private Dictionary<int, List<EmployeeAbsenceTO>> getAbsenceTable(List<EmployeeAbsenceTO> absencesList)
        {
            Dictionary<int, List<EmployeeAbsenceTO>> table = new Dictionary<int,List<EmployeeAbsenceTO>>();
            try
            {
                foreach (EmployeeAbsenceTO emplAbsence in absencesList)
                {
                    if (!table.ContainsKey(emplAbsence.EmployeeID))
                    {
                        List<EmployeeAbsenceTO> list = new List<EmployeeAbsenceTO>();
                        list.Add(emplAbsence);
                        table.Add(emplAbsence.EmployeeID, list);
                    }
                    else
                    {
                        table[emplAbsence.EmployeeID].Add(emplAbsence);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.getAbsenceTabel(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return table;
        }

        private void clearListView()
        {
            lvVacations.BeginUpdate();
            lvVacations.Items.Clear();
            lvVacations.EndUpdate();

            lvVacations.Invalidate();

            btnPrev.Visible = false;
            btnNext.Visible = false;

            this.richTextBox1.Text = "";
            this.tbEnter.Text = "";
            this.tbLastModification.Text = "";

            //passesListCount = 0;
        }

        bool isHoliday(DateTime day)
        {
            return (holidays.ContainsKey(day));
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
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

                if (cbWU.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        
        /// <summary>
        /// Populate List View with Passes found
        /// </summary>
        /// <param name="locationsList"></param>
        private void populateListView(List<EmployeeVacEvidTO> vacationsList, int startIndex)
        {
            try
            {
                if (vacationsList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvVacations.BeginUpdate();
                lvVacations.Items.Clear();

                if (vacationsList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < vacationsList.Count))
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
                        if (lastIndex >= vacationsList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = vacationsList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            EmployeeVacEvidTO  vacation = vacationsList[i];
                            ListViewItem item = new ListViewItem();
                            item.Text = vacation.WorkingUnit.Trim();
                            item.SubItems.Add(vacation.LastName.Trim());
                            item.SubItems.Add(vacation.VacYear.Year.ToString().Trim());
                            item.SubItems.Add(vacation.NumOfDays.ToString().Trim());
                            if (vacation.FromLastYear != -1 && vacation.VacYear.Year == DateTime.Now.Year)
                            {
                                item.SubItems.Add(vacation.FromLastYear.ToString());
                            }
                            else
                            {
                                item.SubItems.Add("N/A");
                            }
                            if (vacation.TotalForUse != -1&&vacation.VacYear.Year == DateTime.Now.Year)
                            {
                                item.SubItems.Add(vacation.TotalForUse.ToString());
                            }
                            else
                            {
                                item.SubItems.Add("N/A");
                            }                     

                            item.SubItems.Add(vacation.UsedDays.ToString());
                            //if (vacation.VacYear.Year == DateTime.Now.Year)
                            //{
                                item.SubItems.Add(vacation.DaysLeft.ToString());
                            //}
                            //else
                            //{
                            //    item.SubItems.Add("N/A");
                            //}
                            item.Tag = vacation;
                            lvVacations.Items.Add(item);

                        }
                    }
                }

                lvVacations.EndUpdate();
                lvVacations.Invalidate();
                lblTotal.Text = rm.GetString("lblTotal", culture) + " " + vacationsList.Count.ToString();
                lblTotal.Visible = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }       

        private void lvVacations_ColumnClick(object sender, ColumnClickEventArgs e)
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

				currentVacations.Sort(new ArrayListSort(sortOrder, sortField));
				startIndex = 0;
				populateListView(currentVacations, startIndex);

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " VacationEvidence.lvVacationEvidence_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show("Exception in lvVacationEvidence_ColumnClick():" + ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		
        }

        #region Inner Class for sorting Array List of Vacations evidence

        /*
		 *  Class used for sorting Array List of VacationEvidence
		*/

        private class ArrayListSort : IComparer<EmployeeVacEvidTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(EmployeeVacEvidTO x, EmployeeVacEvidTO y)
            {
                EmployeeVacEvidTO empl1 = null;
                EmployeeVacEvidTO empl2 = null;

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

                switch (compField)
                {
                    case VacationEvidence.WUIndex:
                        return empl1.WorkingUnit.CompareTo(empl2.WorkingUnit);
                    case VacationEvidence.EmplNameIndex:
                        return empl1.LastName.CompareTo(empl2.LastName);
                    case VacationEvidence.YearIndex:
                        return empl1.VacYear.CompareTo(empl2.VacYear);
                    case VacationEvidence.NumOFDaysIndex:
                        return empl1.NumOfDays.CompareTo(empl2.NumOfDays);
                    case VacationEvidence.FromLastYearIndex:
                        return empl1.FromLastYear.CompareTo(empl2.FromLastYear);
                    case VacationEvidence.TotalIndex:
                        return empl1.TotalForUse.CompareTo(empl2.TotalForUse);
                    case VacationEvidence.UsedIndex:
                        return empl1.UsedDays.CompareTo(empl2.UsedDays);
                    case VacationEvidence.LeftIndex:
                        return empl1.DaysLeft.CompareTo(empl2.DaysLeft);
                    default:
                        return empl1.FirstName.CompareTo(empl2.FirstName);
                }
            }
        }

        #endregion

        private void lvVacations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvVacations.SelectedItems.Count > 0 && (EmployeeVacEvidTO)lvVacations.SelectedItems[0].Tag != null)
                {
                    EmployeeVacEvidTO eve = (EmployeeVacEvidTO)lvVacations.SelectedItems[0].Tag;
                    if (eve.ModifiedBy.Equals(""))
                    {
                        this.tbLastModification.Text = "";
                    }
                    else
                    {
                        if (users.ContainsKey(eve.ModifiedBy))
                            this.tbLastModification.Text = users[eve.ModifiedBy] + " " + eve.ModifiedTime.ToString(dateTimeFormat);
                    }
                    if (users.ContainsKey(eve.CreatedBy))
                        this.tbEnter.Text = users[eve.CreatedBy] + " " + eve.CreatedTime.ToString(dateTimeFormat);
                    this.richTextBox1.Text = eve.Note;
                }
                else
                {
                    this.tbLastModification.Text = "";
                    this.tbEnter.Text = "";
                    this.richTextBox1.Text = "";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.lvVacations_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpYearFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (dtpYearFrom.Value.Year == DateTime.Now.Year)
                    chbIncludePreviously.Enabled = true;
                else
                    chbIncludePreviously.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.dtpYearFrom_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvVacations.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectVacations", culture));
                }
                else
                { 
                    DialogResult result = MessageBox.Show(rm.GetString("deleteVacations", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool deleted = true;
                        bool someAreUsed = false;
                        bool someInPast = false;
                        foreach (ListViewItem item in lvVacations.SelectedItems)
                        {
                            EmployeeVacEvidTO vac = (EmployeeVacEvidTO)item.Tag;
                            if (vac.VacYear.Year < DateTime.Now.Year)
                                someInPast = true;
                            else if (vac.UsedDays == 0)
                            {
                                deleted = deleted && new EmployeeVacEvid().Remove(vac.EmployeeID, vac.VacYear);
                            }
                            else
                            {
                                someAreUsed = true;
                            }
                        }
                        if (someInPast)
                            MessageBox.Show(rm.GetString("someVacInPast", culture));
                        if (someAreUsed)
                            MessageBox.Show(rm.GetString("someVacAreUsed", culture));
                        else if (deleted)
                        {
                            MessageBox.Show(rm.GetString("vacationsDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("vacationsNOTDeleted", culture));
                        }
                        btnSearch_Click(this, new EventArgs());
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);

            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                VacationAdd vacAdd = new VacationAdd();
                vacAdd.ShowDialog();
                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbIncludePreviously_EnabledChanged(object sender, EventArgs e)
        {
            if (!chbIncludePreviously.Enabled)
                chbIncludePreviously.Checked = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvVacations.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selectOneVacation", culture));
                    return;
                }

                EmployeeVacEvidTO vacation = (EmployeeVacEvidTO)lvVacations.SelectedItems[0].Tag;
                if (vacation.UsedDays > 0)
                {
                    MessageBox.Show(rm.GetString("canNotUpdateUsed", culture));
                    return;
                }
                VacationAdd vacAdd = new VacationAdd(vacation);
                vacAdd.ShowDialog();
                this.btnSearch_Click(this, new EventArgs());

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.WaitCursor; 
            }
        }

        private void btnDetailes_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvVacations.SelectedItems.Count !=1)
                {
                    MessageBox.Show(rm.GetString("selectVacationsDetails", culture));
                }
                else
                {
                    EmployeeVacEvidTO vacation = (EmployeeVacEvidTO)lvVacations.SelectedItems[0].Tag;
                    VacationDetails details = new VacationDetails(vacation);
                    details.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnDetailes_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUsing_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvVacations.SelectedItems.Count != 1)
                {
                    EmployeeAbsencesAdd eaa = new EmployeeAbsencesAdd(new EmployeeVacEvidTO());
                    eaa.ShowDialog(this);
                }
                else
                {
                    EmployeeAbsencesAdd eaa = new EmployeeAbsencesAdd((EmployeeVacEvidTO)lvVacations.SelectedItems[0].Tag);
                    eaa.ShowDialog(this);                   
                }
                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnUsing_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //prepare variables for selecting details
                string employeesString = "";
                DateTime yearFrom = new DateTime();
                DateTime yearTo = new DateTime();

                if (currentVacations.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("employee_vacations");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("working_unit", typeof(System.String));
                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("employee_id", typeof(System.String));
                    tableCR.Columns.Add("year", typeof(System.String));
                    tableCR.Columns.Add("approved", typeof(System.Int64));
                    tableCR.Columns.Add("transposed", typeof(System.String));
                    tableCR.Columns.Add("total", typeof(System.String));
                    tableCR.Columns.Add("used", typeof(System.Int64));
                    tableCR.Columns.Add("left", typeof(System.Int64));
                    tableCR.Columns.Add("operator", typeof(System.String));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);

                    foreach (EmployeeVacEvidTO employeeVacation in currentVacations)
                    {
                        if (chbIncludeDetails.Checked)
                        {
                            employeesString += employeeVacation.EmployeeID.ToString() + ", ";
                            if (yearFrom == new DateTime() || yearFrom > employeeVacation.VacYear)
                                yearFrom = employeeVacation.VacYear;
                            if (yearTo == new DateTime() || yearTo < employeeVacation.VacYear)
                                yearTo = employeeVacation.VacYear;
                        }

                        DataRow row = tableCR.NewRow();
                        row["working_unit"] = employeeVacation.WorkingUnit;
                        row["employee"] = employeeVacation.LastName;
                        row["employee_id"] = employeeVacation.EmployeeID.ToString();
                        row["year"] = employeeVacation.VacYear.ToString("yyyy");
                        row["approved"] = employeeVacation.NumOfDays;
                        if (employeeVacation.VacYear.Year == DateTime.Now.Year)
                        {
                            row["transposed"] = employeeVacation.FromLastYear.ToString();
                            row["total"] = employeeVacation.TotalForUse.ToString();
                        }
                        else
                        {
                            row["transposed"] = "N/A";
                            row["total"] = "N/A";
                        }
                        row["used"] = employeeVacation.UsedDays;
                        row["left"] = employeeVacation.DaysLeft;
                        row["operator"] = (string)users[employeeVacation.CreatedBy];
                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selWorkingUnit = "*";
                    string selEmplName = "*";
                    string selYear = dtpYearFrom.Value.Year.ToString();
                    string lblIncludeLast = "";
                    string selApprovedFrom = nudFromApproved.Value.ToString();
                    string selApprovedTo = nudToApproved.Value.ToString();
                    string selUsedFrom = nudFromUsed.Value.ToString();
                    string selUsedTo = nudToUsed.Value.ToString();
                    string selLeftFrom = nudFromLeft.Value.ToString();
                    string selLeftTo = nudToLeft.Value.ToString();

                    if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                        selWorkingUnit = cbWU.Text;
                    if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                        selEmplName = cbEmployee.Text;
                    if (chbIncludePreviously.Checked)
                        lblIncludeLast = rm.GetString("lblIncludingLast", culture);

                    if (!chbIncludeDetails.Checked)
                    {
                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.EmplVacationsCRView_sr view = new Reports.Reports_sr.EmplVacationsCRView_sr(dataSetCR,
                                 selWorkingUnit, selEmplName, selYear, lblIncludeLast, selApprovedFrom, selApprovedTo, selUsedFrom, selUsedTo, selLeftFrom, selLeftTo);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.EmplVacationsCRView_en view = new Reports.Reports_en.EmplVacationsCRView_en(dataSetCR,
                                 selWorkingUnit, selEmplName, selYear, lblIncludeLast, selApprovedFrom, selApprovedTo, selUsedFrom, selUsedTo, selLeftFrom, selLeftTo);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                        {
                            Reports.Reports_fi.EmplVacationsCRView_fi view = new Reports.Reports_fi.EmplVacationsCRView_fi(dataSetCR,
                                 selWorkingUnit, selEmplName, selYear, lblIncludeLast, selApprovedFrom, selApprovedTo, selUsedFrom, selUsedTo, selLeftFrom, selLeftTo);
                            view.ShowDialog(this);
                        }
                    }
                    else
                    {
                        if (employeesString.Length > 0)
                        {
                            employeesString = employeesString.Substring(0,employeesString.Length-2);
                            List<EmployeeVacEvidScheduleTO> emplVacationPlans = new List<EmployeeVacEvidScheduleTO>();
                            emplVacationPlans = new EmployeeVacEvid().getVacationDetails(employeesString, yearFrom, yearTo);
                            List<EmployeeAbsenceTO> emplAbsences = new List<EmployeeAbsenceTO>();
                            EmployeeAbsence ea = new EmployeeAbsence();
                            ea.EmplAbsTO.PassTypeID = Constants.vacation;
                            ea.EmplAbsTO.DateStart = yearFrom;
                            ea.EmplAbsTO.DateEnd = yearTo.AddYears(1);
                            emplAbsences = ea.SearchForVacEvid(employeesString, "", new DateTime(), new DateTime());
                            
                            DataTable tablePl = new DataTable("employee_vac_plans");
                            DataTable tableAbs = new DataTable("empl_abs");

                            tablePl.Columns.Add("employee_id", typeof(System.String));
                            tablePl.Columns.Add("year", typeof(System.String));
                            tablePl.Columns.Add("from", typeof(System.DateTime));
                            tablePl.Columns.Add("to", typeof(System.DateTime));
                            tablePl.Columns.Add("status", typeof(System.String));

                            tableAbs.Columns.Add("employee_id", typeof(System.String));
                            tableAbs.Columns.Add("year", typeof(System.String));
                            tableAbs.Columns.Add("from", typeof(System.DateTime));
                            tableAbs.Columns.Add("to", typeof(System.DateTime));
                            tableAbs.Columns.Add("num_of_days", typeof(System.Int64));
                            tableAbs.Columns.Add("operator", typeof(System.String));

                            //add logo image just once
                            foreach (EmployeeAbsenceTO employeeAbsence in emplAbsences)
                            {
                                DataRow row = tableAbs.NewRow();
                                row["from"] = employeeAbsence.DateStart;
                                row["year"] = employeeAbsence.VacationYear.ToString("yyyy");
                                row["employee_id"] = employeeAbsence.EmployeeID.ToString();
                                row["to"] = employeeAbsence.DateEnd;
                                List<EmployeeTimeScheduleTO> EmplScheduleList = Common.Misc.GetEmployeeTimeSchedules(employeeAbsence.EmployeeID, employeeAbsence.DateStart, employeeAbsence.DateEnd);
                                int usedDays = Common.Misc.getNumOfWorkingDays(EmplScheduleList, employeeAbsence.DateStart, employeeAbsence.DateEnd, holidays,timeSchemas);
                                row["num_of_days"] = usedDays;                            
                                row["operator"] = (string)users[employeeAbsence.CreatedBy];

                                tableAbs.Rows.Add(row);
                                tableAbs.AcceptChanges();
                            }

                            dataSetCR.Tables.Add(tablePl);
                            dataSetCR.Tables.Add(tableAbs);
                            foreach (EmployeeVacEvidScheduleTO employeeVacation in emplVacationPlans)
                            {
                                DataRow row = tablePl.NewRow();
                                row["employee_id"] = employeeVacation.EmployeeID.ToString();
                                row["year"] = employeeVacation.VacYear.ToString("yyyy");
                                row["from"] = employeeVacation.StartDate;                                
                                row["to"] = employeeVacation.EndDate;
                                row["status"] = employeeVacation.Status;

                                tablePl.Rows.Add(row);
                                tablePl.AcceptChanges();
                            }
                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                Reports.Reports_sr.EmplVacDetailsCRView_sr view = new Reports.Reports_sr.EmplVacDetailsCRView_sr(dataSetCR,
                                     selWorkingUnit, selEmplName, selYear, lblIncludeLast, selApprovedFrom, selApprovedTo, selUsedFrom, selUsedTo, selLeftFrom, selLeftTo);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                Reports.Reports_en.EmplVacDetailsCRView_en view = new Reports.Reports_en.EmplVacDetailsCRView_en(dataSetCR,
                                    selWorkingUnit, selEmplName, selYear, lblIncludeLast, selApprovedFrom, selApprovedTo, selUsedFrom, selUsedTo, selLeftFrom, selLeftTo);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                            {
                                Reports.Reports_fi.EmplVacDetailsCRView_fi view = new Reports.Reports_fi.EmplVacDetailsCRView_fi(dataSetCR,
                                     selWorkingUnit, selEmplName, selYear, lblIncludeLast, selApprovedFrom, selApprovedTo, selUsedFrom, selUsedTo, selLeftFrom, selLeftTo);
                                view.ShowDialog(this);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void VacationEvidence_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + "  VacationEvidence.VacationEvidence_KeyUp(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " VacationEvidence.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " VacationEvidence.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}