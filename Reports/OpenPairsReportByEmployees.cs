using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using TransferObjects;
using Util;
using Common;

namespace Reports {
    public partial class OpenPairsReportByEmployees : Form {

        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        DebugLog log;

        List<IOPairTO> ioPairList = new List<IOPairTO>();
        List<WorkingUnitTO> wUnits;
        private List<OrganizationalUnitTO> oUnits;
        List<EmployeeTO> currentEmployeesList;
        private string wuString = "";
        private string ouString = "";

        List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

        public OpenPairsReportByEmployees() {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(OpenPairsReportByEmployees).Assembly);

            //DateTime date = DateTime.Now;
            //dtpFromDate.Value = date.AddDays(-1);

            setLanguage();
            populateEmployeesCombo();
        }

        private void setLanguage() {
            //gbEmpolyee.Text = rm.GetString("employee", culture);
            //gbLocation.Text = rm.GetString("location", culture);
            //chBoxEmployeeAll.Text = rm.GetString("allEmpl", culture);
            //cbIncludeSubLoc.Text = rm.GetString("hierarchically", culture);
            //chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
            //gbTimeInterval.Text = rm.GetString("timeInterval", culture);
            //lblTo.Text = rm.GetString("lblTo", culture);
            //lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
            chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
            gbFilter.Text = rm.GetString("gbFilter", culture);
            lblMonth.Text = rm.GetString("lblMonth", culture);
            lblEmployee.Text = rm.GetString("lblEmployee", culture);
            lblOrgUnit.Text = rm.GetString("lblOrgUnit", culture);
            lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
            btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
            btnCancel.Text = rm.GetString("btnCancel", culture);

            this.Text = rm.GetString("OpenPairsReportByEmployees", culture);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void populateEmployeesCombo() {
            try {
                if (logInUser != null) {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                string wuString = "";
                foreach (WorkingUnitTO wUnit in wUnits) {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0) {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                currentEmployeesList = new Employee().SearchByWU(wuString);

                foreach (EmployeeTO employee in currentEmployeesList) {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmployeesList.Insert(0, empl);

                cbEmployee.DataSource = currentEmployeesList;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";


            }
            catch (Exception ex) {

                log.writeLog(DateTime.Now + " OpenPairsReportByEmployees.populateEmployeesCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                string employeeID = "";
                string orgUnitID = "";
                string workingUnitID = "";

                EmployeeTO employee = new EmployeeTO();
                if (cbEmployee.Text != "*") {
                    employee = cbEmployee.SelectedItem as EmployeeTO;
                    employeeID = employee.EmployeeID.ToString();
                }
                else {
                    if (currentEmployeesList.Count > 1) {
                        foreach (EmployeeTO emplTO in currentEmployeesList) {
                            if (!emplTO.EmployeeID.ToString().Equals("-1")) {
                                employeeID += emplTO.EmployeeID.ToString().Trim() + ",";
                            }
                        }

                        if (employeeID.Length > 0) {
                            employeeID = employeeID.Substring(0, employeeID.Length - 1);
                        }

                    }
                    else {
                        employeeID = "-1";
                    }
                }

                OrganizationalUnitTO ouTO = cbOrgUnits.SelectedItem as OrganizationalUnitTO;

                if (cbOrgUnits.Text != "*") {
                    ouTO = cbOrgUnits.SelectedItem as OrganizationalUnitTO;
                    orgUnitID = ouTO.OrgUnitID.ToString();
                }
                else {
                    if (oUnits.Count > 1) {
                        foreach (OrganizationalUnitTO emplTO in oUnits) {
                            if (!emplTO.OrgUnitID.ToString().Equals("-1")) {
                                orgUnitID += emplTO.OrgUnitID.ToString().Trim() + ",";
                            }
                        }

                        if (orgUnitID.Length > 0) {
                            orgUnitID = orgUnitID.Substring(0, orgUnitID.Length - 1);
                        }

                    }
                    else {
                        orgUnitID = "-1";
                    }
                }

                WorkingUnitTO wtTO = cbWorkingUnits.SelectedItem as WorkingUnitTO;

                if (cbWorkingUnits.Text != "*") {
                    wtTO = cbWorkingUnits.SelectedItem as WorkingUnitTO;
                    workingUnitID = wtTO.WorkingUnitID.ToString();
                }
                else {
                    if (wUnits.Count > 1) {
                        foreach (WorkingUnitTO emplTO in wUnits) {
                            if (!emplTO.WorkingUnitID.ToString().Equals("-1")) {
                                workingUnitID += emplTO.WorkingUnitID.ToString().Trim() + ",";
                            }
                        }

                        if (workingUnitID.Length > 0) {
                            workingUnitID = workingUnitID.Substring(0, workingUnitID.Length - 1);
                        }

                    }
                    else {
                        workingUnitID = "-1";
                    }
                }

                DateTime selected = dtpMonth.Value;
                DateTime from = new DateTime(selected.Year, selected.Month, 1, 0, 0, 0);
                DateTime to = new DateTime();
                if (selected.Month == DateTime.Now.Month) {
                    to = DateTime.Now.Date;
                }
                else {
                    to = from.AddMonths(1).AddDays(-1);
                }

                ioPairList = new IOPair().GetOpenPairsForReport(employeeID, from, to, orgUnitID, workingUnitID);
                //ioPairList = new IOPair().GetOpenPairsForReport(employee.EmployeeID.ToString(), dtpFromDate.Value.Date);

                if (ioPairList.Count == 0) {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("dataNotFound", culture));
                    return;
                }

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr)) {
                    Reports_sr.EmployeesCRView view = new Reports_sr.EmployeesCRView(
                        this.PopulateCRData(ioPairList),
                        dtpMonth.Value, cbWorkingUnits.Text, cbOrgUnits.Text);
                    view.ShowDialog(this);
                }
                //else if (NotificationController.GetLanguage().Equals(Constants.Lang_en)) {
                //    //I ZA ENGLESKU VERZIJU SE POZIVA SRPSKA DOK SE NE URADI ENGLESKA VERZIJA
                //    Reports_sr.EmployeesCRView view = new Reports_sr.EmployeesCRView(
                //        this.PopulateCRData(ioPairList),
                //        dtpMonth.Value);
                //    view.ShowDialog(this);


                //Reports_en.EmployeesCRView_en view = new Reports_en.EmployeesCRView_en(
                //    this.PopulateCRData(ioPairList),
                //    dtpFromDate.Value, dtpToDate.Value);
                //view.ShowDialog(this);
                //}

            }
            catch (Exception ex) {

                log.writeLog(DateTime.Now + " OpenPairsReportByEmployees.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private DataSet PopulateCRData(List<IOPairTO> dataList) {
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable("loc_rep");
            table.Columns.Add("date", typeof(System.String));
            table.Columns.Add("last_name", typeof(System.String));
            table.Columns.Add("first_name", typeof(System.String));
            table.Columns.Add("full_name", typeof(System.String));
            table.Columns.Add("location", typeof(System.String));
            table.Columns.Add("pass_type", typeof(System.String));
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

            try {
                //TimeSpan timeSpan = new TimeSpan();

                //string minutes;

                int i = 0;
                foreach (IOPairTO pairTO in dataList) {
                    DataRow row = table.NewRow();

                    row["date"] = pairTO.IOPairDate.ToString("dd.MM.yyyy");
                    //row["first_name"] = lvEmployees.SelectedItems[0].Text;
                    row["last_name"] = pairTO.EmployeeLastName;
                    //row["working_unit"] = lvEmployees.SelectedItems[0].SubItems[2].Text;
                    row["location"] = pairTO.LocationName;
                    row["pass_type"] = pairTO.PassType;

                    if (cbEmployee.Text == "*") {
                        row["full_name"] = "*";
                    }
                    else {
                        row["full_name"] = pairTO.EmployeeLastName;
                    }

                    if (pairTO.StartTime != new DateTime()) {
                        row["start"] = pairTO.StartTime.ToString("HH:mm:ss");
                    }
                    else {
                        row["start"] = "NEMA EVIDENCIJE";
                    }
                    if (pairTO.EndTime != new DateTime()) {
                        row["end"] = pairTO.EndTime.ToString("HH:mm:ss");
                    }
                    else {
                        row["end"] = "NEMA EVIDENCIJE";
                    }
                    //row["type"] = pairTO.PassType;
                    //timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);

                    //if (timeSpan.Minutes < 10) {
                    //    minutes = "0" + timeSpan.Minutes.ToString();
                    //}
                    //else {
                    //    minutes = timeSpan.Minutes.ToString();

                    //}
                    //row["total_time"] = timeSpan.Hours.ToString() + "h " + minutes + "min ";
                    //row["employee_id"] = pairTO.EmployeeID;

                    row["imageID"] = 1;
                    if (i == 0) {
                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();
                    }

                    table.Rows.Add(row);
                    i++;
                }

                table.AcceptChanges();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " OpenPairsReportByEmployees.PopulateCRData(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            dataSet.Tables.Add(table);
            dataSet.Tables.Add(tableI);
            return dataSet;
        }

        private void cbWorkingUnits_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits) {
                    if (cbWorkingUnits.SelectedIndex != 0) {
                        if (wu.WorkingUnitID == (int)cbWorkingUnits.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0) {
                            if (!chbHierarhicly.Checked) {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                    //else {
                    //    if (cbOrgUnits.Text != "*") {
                    //        populateEmployeeComboOrg((int)cbOrgUnits.SelectedValue);
                    //    }
                    //}
                }

                if (cbWorkingUnits.SelectedValue is int && !check) {
                    populateEmployeeCombo((int)cbWorkingUnits.SelectedValue);
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " OpenPairsReportByEmployees_cbWorkingUnits_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Populate Working Unit Combo Box
        /// </summary>
        private void populateWorkingUnitCombo() {
            try {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits) {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnits.DataSource = wuArray;
                cbWorkingUnits.DisplayMember = "Name";
                cbWorkingUnits.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " OpenPairsReportByEmployees.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate Employee Combo Box
        /// </summary>
        private void populateEmployeeCombo(int wuID) {
            try {
                currentEmployeesList = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1) {
                    currentEmployeesList = new Employee().SearchByWU(wuString);
                }
                else {
                    if (chbHierarhicly.Checked) {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits) {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnits.SelectedValue) {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wuList) {
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0) {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }
                    }
                    if (cbOrgUnits.Text == "*") {
                        currentEmployeesList = new Employee().SearchByWU(workUnitID);
                    }
                    else {
                        currentEmployeesList = new Employee().SearchByOUandWU((int)cbOrgUnits.SelectedValue, Convert.ToInt32(workUnitID));
                    }
                }

                foreach (EmployeeTO employee in currentEmployeesList) {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmployeesList.Insert(0, empl);

                cbEmployee.DataSource = currentEmployeesList;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " OpenPairsReportByEmployees.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        //   18.06.2019.  BOJAN
        private void populateEmployeeComboOrg(int ouID) {
            try {
                currentEmployeesList = new List<EmployeeTO>();
                string organizationUnitID = ouID.ToString();
                if (ouID == -1) {
                    currentEmployeesList = new Employee().SearchByOU(ouString);
                }
                else {
                    //if (chbHierarhicly.Checked) {
                    //    List<OrganizationalUnitTO> ouList = new List<OrganizationalUnitTO>();
                    //    OrganizationalUnit orgUnit = new OrganizationalUnit();
                    //    foreach (OrganizationalUnitTO orgUnit in oUnits) {
                    //        if (orgUnit.OrgUnitID == (int)this.cbOrgUnits.SelectedValue) {
                    //            ouList.Add(orgUnit);
                    //            orgUnit.orgunit = orgUnit;
                    //        }
                    //    }
                    //    if (workUnit.WUTO.ChildWUNumber > 0)
                    //        ouList = orgUnit.FindAllChildren(ouList);
                    //    organizationUnitID = "";
                    //    foreach (OrganizationalUnitTO ounit in ouList) {
                    //        organizationUnitID += ounit.WorkingUnitID.ToString().Trim() + ",";
                    //    }

                    //    if (organizationUnitID.Length > 0) {
                    //        organizationUnitID = organizationUnitID.Substring(0, organizationUnitID.Length - 1);
                    //    }
                    //}
                    if (cbWorkingUnits.Text == "*") {
                        currentEmployeesList = new Employee().SearchByOU(organizationUnitID);
                    }
                    else {
                        currentEmployeesList = new Employee().SearchByOUandWU(Convert.ToInt32(organizationUnitID), (int)cbWorkingUnits.SelectedValue);
                    }
                }

                foreach (EmployeeTO employee in currentEmployeesList) {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmployeesList.Insert(0, empl);

                cbEmployee.DataSource = currentEmployeesList;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " OpenPairsReportByEmployees.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateOrganizationalUnitCombo() {
            try {

                foreach (OrganizationalUnitTO ouTO in oUnits) {
                    ouArray.Add(ouTO);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbOrgUnits.DataSource = ouArray;
                cbOrgUnits.DisplayMember = "Name";
                cbOrgUnits.ValueMember = "OrgUnitID";

            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employees.populateOrganizationUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void OpenPairsReportByEmployees_Load(object sender, EventArgs e) {
            wUnits = new List<WorkingUnitTO>();
            oUnits = new List<OrganizationalUnitTO>();

            if (logInUser != null) {
                wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PassPurpose);
                oUnits = new OrganizationalUnit().Search();
            }

            foreach (WorkingUnitTO wUnit in wUnits) {
                wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
            }

            if (wuString.Length > 0) {
                wuString = wuString.Substring(0, wuString.Length - 1);
            }

            foreach (OrganizationalUnitTO oUnit in oUnits) {
                ouString += oUnit.OrgUnitID.ToString().Trim() + ",";
            }

            if (ouString.Length > 0) {
                ouString = ouString.Substring(0, ouString.Length - 1);
            }

            populateWorkingUnitCombo();
            populateEmployeeCombo(-1);
            populateOrganizationalUnitCombo();

            // odkomentarisati ako bude bilo potrebno da se koristi
            //lblOrgUnit.Visible = false;
            //cbOrgUnits.Visible = false;
        }

        private void cbOrgUnits_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                foreach (OrganizationalUnitTO ou in oUnits) {
                    if (cbOrgUnits.SelectedIndex != 0) {
                        if (ou.OrgUnitID == (int)cbOrgUnits.SelectedValue) {// && ou.EmplNumber == 0 && ou.ChildWUNumber > 0
                            //if (!chbHierarhicly.Checked) {
                            //    chbHierarhicly.Checked = true;
                            //    check = true;
                            //}
                        }
                    }
                }

                if (cbOrgUnits.SelectedValue is int && !check) {
                    populateEmployeeComboOrg((int)cbOrgUnits.SelectedValue);
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " OpenPairsReportByEmployees_cbOrgUnits_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
