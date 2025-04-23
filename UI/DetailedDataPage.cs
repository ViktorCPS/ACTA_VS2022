using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common;
using TransferObjects;
using System.Globalization;
using System.Resources;
using Util;
using ACTAWorkAnalysisReports;
using System.IO;
using System.Diagnostics;

namespace UI
{
    public partial class DetailedDataPage : Form
    {
        private List<int> wuList = new List<int>();
        private List<int> ouList = new List<int>();

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string wuString = "";
        private string ouString = "";

        Dictionary<int, WorkingUnitTO> dictWuList = new Dictionary<int, WorkingUnitTO>();
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;
       
        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();
        Dictionary<int, EmployeeTypeTO> dictEmplTypes = new Dictionary<int, EmployeeTypeTO>();

        DateTime fromDate = new DateTime();
        DateTime toDate = new DateTime();
        Dictionary<int, PassTypeTO> dictionaryPassTypes = new Dictionary<int, PassTypeTO>();
        string[] columns = new string[18];
            
        public DetailedDataPage()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(MealsUsed).Assembly);

                setLanguage();


                logInUser = NotificationController.GetLogInUser();

                DateTime now = DateTime.Now;

                dtpFrom.Value = now.Date.AddDays(-now.Day + 1);
                dtpTo.Value = now.AddSeconds(-now.Second);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    this.cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void populateWU()
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public void populateListViewEmployees(int wuID)
        {
            try
            {
                List<EmployeeTO> emplArray = new List<EmployeeTO>();
                if (rbWU.Checked)
                {
                    string workUnitID = wuID.ToString();
                    if (wuID == -1)
                    {
                        //emplArray = new Employee().SearchByWU(wuString);
                    }
                    else
                    {
                        List<WorkingUnitTO> wList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
                            {
                                wList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wList = workUnit.FindAllChildren(wList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wList)
                        {
                            if (wuList.Contains(wunit.WorkingUnitID))
                                workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }

                        emplArray = new Employee().SearchByWU(workUnitID);
                    }

                }
                else
                {

                    string orgUnitID = wuID.ToString();
                    if (wuID == -1)
                    {
                        //emplArray = new Employee().SearchByOU(ouString, -1, -1, -1, dtpFrom.Value, dtpTo.Value);
                    }
                    else
                    {
                        List<OrganizationalUnitTO> oList = new List<OrganizationalUnitTO>();
                        OrganizationalUnit orgUnit = new OrganizationalUnit();
                        foreach (KeyValuePair<int, OrganizationalUnitTO> organizationalUnitPair in oUnits)
                        {
                            OrganizationalUnitTO organizationalUnit = organizationalUnitPair.Value;
                            if (organizationalUnit.OrgUnitID == (int)this.cbOU.SelectedValue)
                            {
                                oList.Add(organizationalUnit);
                                orgUnit.OrgUnitTO = organizationalUnit;
                            }
                        }

                        oList = orgUnit.FindAllChildren(oList);
                        orgUnitID = "";
                        foreach (OrganizationalUnitTO ounit in oList)
                        {
                            if (ouList.Contains(ounit.OrgUnitID))
                                orgUnitID += ounit.OrgUnitID.ToString().Trim() + ",";
                        }

                        if (orgUnitID.Length > 0)
                        {
                            orgUnitID = orgUnitID.Substring(0, orgUnitID.Length - 1);
                        }

                        emplArray = new Employee().SearchByOU(orgUnitID, -1, null, dtpFrom.Value, dtpTo.Value);
                    }
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                if (emplArray.Count > 0)
                {
                    for (int i = 0; i < emplArray.Count; i++)
                    {
                        EmployeeTO emplTO = (EmployeeTO)emplArray[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = emplTO.FirstName.Trim();
                        item.ToolTipText = emplTO.EmployeeID.ToString();
                        item.SubItems.Add(emplTO.LastName.Trim());
                        item.Tag = emplTO;
                        lvEmployees.Items.Add(item);
                    }
                }

                lvEmployees.EndUpdate();
                lvEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            int wuID = -1;

            if (rbWU.Checked)
            {
                cbWU.Enabled = btnWUTree.Enabled = true;
                cbOU.Enabled = btnOUTree.Enabled = false;

                if (cbWU.SelectedIndex > 0)
                    wuID = (int)cbWU.SelectedValue;
            }
            else
            {
                cbOU.Enabled = btnOUTree.Enabled = true;
                cbWU.Enabled = btnWUTree.Enabled = false;

                if (cbOU.SelectedIndex > 0)
                    wuID = (int)cbOU.SelectedValue;
            }

            populateListViewEmployees(wuID);
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            int wuID = -1;

            if (!rbOU.Checked)
            {
                cbWU.Enabled = btnWUTree.Enabled = true;
                cbOU.Enabled = btnOUTree.Enabled = false;

                if (cbWU.SelectedIndex > 0)
                    wuID = (int)cbWU.SelectedValue;
            }
            else
            {
                cbOU.Enabled = btnOUTree.Enabled = true;
                cbWU.Enabled = btnWUTree.Enabled = false;

                if (cbOU.SelectedIndex > 0)
                    wuID = (int)cbOU.SelectedValue;
            }

            populateListViewEmployees(wuID);
        }
        private void populateOU()
        {
            try
            {
                List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

                foreach (int id in oUnits.Keys)
                {
                    ouArray.Add(oUnits[id]);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbOU.DataSource = ouArray;
                cbOU.DisplayMember = "Name";
                cbOU.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void btnOUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits(ouString);
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbOU.SelectedIndex = cbOU.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void DetailedDataPage_Load(object sender, EventArgs e)
        {
            schemas = new TimeSchema().getDictionary();

            List<EmployeeTypeTO> listTypes = new EmployeeType().Search();

            foreach (EmployeeTypeTO emplType in listTypes)
            {
                if (!dictEmplTypes.ContainsKey(emplType.EmployeeTypeID))
                    dictEmplTypes.Add(emplType.EmployeeTypeID, emplType);
            }
            //table.Columns.Add("empl_id", typeof(System.String));
            //table.Columns.Add("first_name", typeof(System.String));
            //table.Columns.Add("cost_centre_code", typeof(System.String));
            //table.Columns.Add("cost_centre", typeof(System.String));
            //table.Columns.Add("branch", typeof(System.String));
            //table.Columns.Add("empl_type", typeof(System.String));
            //table.Columns.Add("date_week", typeof(System.String));
            //table.Columns.Add("date_year", typeof(System.String));
            //table.Columns.Add("date_month", typeof(System.String));
            //table.Columns.Add("date_day", typeof(System.String));
            //table.Columns.Add("start_time", typeof(System.TimeSpan));
            //table.Columns.Add("end_time", typeof(System.TimeSpan));
            //table.Columns.Add("pass_type", typeof(System.String));
            //table.Columns.Add("total", typeof(System.Double));
            //table.Columns.Add("description", typeof(System.String));
            //table.Columns.Add("shift", typeof(System.String));
            //table.Columns.Add("cycle_day", typeof(System.String));
            //table.Columns.Add("intervals", typeof(System.String));
            //table.AcceptChanges();
            //set.Tables.Add(table);
            //set.AcceptChanges();
            columns[0] = rm.GetString("hdrEmplID", culture);
            columns[1] = rm.GetString("hdrEmployee", culture);
            columns[2] = rm.GetString("hdrCostCenter", culture);
            columns[3] = rm.GetString("hdrCostCenterDesc", culture);
            columns[4] = rm.GetString("hdrBranch", culture);
            columns[5] = rm.GetString("hdrEmplType", culture);
            columns[6] = rm.GetString("hdrWeek", culture);
            columns[7] = rm.GetString("hdrYear", culture);
            columns[8] = rm.GetString("hdrMonth", culture);
            columns[9] = rm.GetString("hdrDay", culture);
            columns[10] = rm.GetString("hdrSartTime", culture);
            columns[11] = rm.GetString("hdrEndTime", culture);
            columns[12] = rm.GetString("hdrWageType", culture);
            columns[13] = rm.GetString("total", culture);
            columns[14] = rm.GetString("hdrDescription", culture);
            columns[15] = rm.GetString("rbWSDay", culture);
            columns[16] = rm.GetString("hdrCycleDay", culture);
            columns[17] = rm.GetString("hdrIntervals", culture);
            tbTimeFrom.Text = "00:00";
            tbTimeTo.Text = "23:59";
            if (logInUser != null)
            {
                wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
            }

            foreach (WorkingUnitTO wUnit in wUnits)
            {
                wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                wuList.Add(wUnit.WorkingUnitID);
            }

            if (wuString.Length > 0)
                wuString = wuString.Substring(0, wuString.Length - 1);

            foreach (int id in oUnits.Keys)
            {
                ouString += id.ToString().Trim() + ",";
                ouList.Add(id);
            }

            if (ouString.Length > 0)
                ouString = ouString.Substring(0, ouString.Length - 1);

            List<WorkingUnitTO> wuListAll = wUnits;
            dictWuList = getWUnits(wuListAll);

            populateWU();
            populateOU();
        }
        private void cbOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int ouID = -1;
                if (cbOU.SelectedIndex > 0)
                    ouID = (int)cbOU.SelectedValue;

                populateListViewEmployees(ouID);
                populatePassTypes(false);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void setLanguage()
        {
            try
            {
                //group box text
                //gbFilter.Text = rm.GetString("gbFilter", culture);
                gbEmployee.Text = rm.GetString("gbEmployees", culture);

                string nesto = rm.GetString("gbWageType", culture);
                gbWageType.Text = nesto;
                gbFilter.Text = rm.GetString("gbUnitFilter", culture);

                //lblText
                lblFrom.Text = rm.GetString("lblFrom", culture);

                lblTo.Text = rm.GetString("lblTo", culture);


                cbSelectAllEmpl.Text = rm.GetString("chbSelectAll", culture);


                //RB text
                rbOU.Text = rm.GetString("gbOrganizationalUnits", culture);
                rbWU.Text = rm.GetString("WUForm", culture);




                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 7) / 2, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 7) / 2, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();


                lvWageTypes.BeginUpdate();
                lvWageTypes.Columns.Add(rm.GetString("hdrWageTypeID", culture), (lvWageTypes.Width - 7) / 2, HorizontalAlignment.Left);
                lvWageTypes.Columns.Add(rm.GetString("hdrWageType", culture), (lvWageTypes.Width - 7) / 2, HorizontalAlignment.Left);
                lvWageTypes.EndUpdate();


                btnClose.Text = rm.GetString("btnClose", culture);

                btnSearch.Text = rm.GetString("btnExport", culture);



            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;

                int wuID = -1;
                if (cbWU.SelectedIndex > 0)
                    wuID = (int)cbWU.SelectedValue;

                populateListViewEmployees(wuID);
                populatePassTypes(true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void cbSelectAllEmpl_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbSelectAllEmpl.Checked)
                {

                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        item.Selected = false;
                    }

                }
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " OnlineMealsUsed.cbSelectAllEmpl_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populatePassTypes(bool wu)
        {
            try
            {

                bool isAltLang = !NotificationController.GetLanguage().Equals(Constants.Lang_sr);

                List<PassTypeTO> passTypeList = new List<PassTypeTO>();
                if (wu)
                {
                    if (cbWU.SelectedIndex != -1)
                    {
                        passTypeList = new PassType().SearchForCompany(Common.Misc.getRootWorkingUnit(((WorkingUnitTO)cbWU.SelectedItem).WorkingUnitID, dictWuList), isAltLang);
                    }
                }
                else
                {
                    if (cbOU.SelectedIndex != -1)
                    {
                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit();
                        wuXou.WUXouTO.OrgUnitID = ((OrganizationalUnitTO)cbOU.SelectedItem).OrgUnitID;
                        List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                        if (list.Count > 0)
                        {
                            WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                            passTypeList = new PassType().SearchForCompany(Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, dictWuList), isAltLang);
                        }
                    }
                }
                populateWageTypes(passTypeList, isAltLang);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void populateWageTypes(List<PassTypeTO> list, bool isAltLang)
        {
            lvWageTypes.BeginUpdate();
            lvWageTypes.Items.Clear();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    PassTypeTO ptTO = (PassTypeTO)list[i];
                    if (!dictionaryPassTypes.ContainsKey(ptTO.PassTypeID))
                        dictionaryPassTypes.Add(ptTO.PassTypeID, ptTO);
                    ListViewItem item = new ListViewItem();
                    item.Text = ptTO.PassTypeID.ToString();

                    if (isAltLang)
                        item.SubItems.Add(ptTO.DescriptionAltAndID);
                    else
                        item.SubItems.Add(ptTO.DescriptionAndID);


                    item.Tag = ptTO;
                    lvWageTypes.Items.Add(item);
                }
            }

            lvWageTypes.EndUpdate();
            lvWageTypes.Invalidate();
        }

        public Dictionary<int, WorkingUnitTO> getWUnits(List<WorkingUnitTO> wuListAll)
        {
            try
            {
                Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int, WorkingUnitTO>();

                foreach (WorkingUnitTO wu in wuListAll)
                {
                    if (!wUnits.ContainsKey(wu.WorkingUnitID))
                        wUnits.Add(wu.WorkingUnitID, wu);
                    else
                        wUnits[wu.WorkingUnitID] = wu;
                }

                return wUnits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string pass_types = "";
            Stopwatch stop = new Stopwatch();
            stop.Start();

            if (lvWageTypes.SelectedIndices.Count > 0)
            {

                foreach (int index in lvWageTypes.SelectedIndices)
                {
                    pass_types += lvWageTypes.Items[index].Text.Trim() + ",";
                }

                if (pass_types.Length > 0)
                    pass_types = pass_types.Substring(0, pass_types.Length - 1);
            }

            Dictionary<int, EmployeeTO> dictionaryEmpl = new Dictionary<int, EmployeeTO>();

            //filter for empolyees
            string emplIDs = "";
            if (lvEmployees.SelectedItems.Count > 0)
            {
                foreach (int index in lvEmployees.SelectedIndices)
                {
                    EmployeeTO empl = ((EmployeeTO)lvEmployees.Items[index].Tag);
                    emplIDs += empl.EmployeeID + ",";
                    if (!dictionaryEmpl.ContainsKey(empl.EmployeeID))
                        dictionaryEmpl.Add(empl.EmployeeID, empl);

                }
                if (emplIDs.Length > 0)
                {
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }
            }
            else
            {
                foreach (ListViewItem item in lvEmployees.Items)
                {
                    EmployeeTO empl = ((EmployeeTO)item.Tag);
                    emplIDs += empl.EmployeeID + ",";
                    if (!dictionaryEmpl.ContainsKey(empl.EmployeeID))
                        dictionaryEmpl.Add(empl.EmployeeID, empl);
                }
                if (emplIDs.Length > 0)
                {
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }
            }


            DateTime dateFrom = new DateTime();
            DateTime dateTO = new DateTime();

            string hourFrom = "";
            string hourTo = "";
            string minFrom = "";
            string minTo = "";

            string timeFrom = tbTimeFrom.Text;
            string timeTo = tbTimeTo.Text;

            if (timeFrom.Trim() == "" || timeTo.Trim() == "" || !timeFrom.Contains(":") || !timeTo.Contains(":"))
            {
                MessageBox.Show(rm.GetString("invalidTimeFormat", culture));
                return;
            }

            hourFrom = timeFrom.Remove(timeFrom.IndexOf(':'));
            hourTo = timeTo.Remove(timeTo.IndexOf(':'));

            minFrom = timeFrom.Substring(timeFrom.IndexOf(':') + 1);
            minTo = timeTo.Substring(timeTo.IndexOf(':') + 1);

            int hourF = -1;
            if (!int.TryParse(hourFrom, out hourF))
                hourF = -1;

            int hourT = -1;
            if (!int.TryParse(hourTo, out hourT))
                hourT = -1;

            int minT = -1;
            if (!int.TryParse(minTo, out minT))
                minT = -1;

            int minF = -1;
            if (!int.TryParse(minFrom, out minF))
                minF = -1;

            if (hourF == -1 || hourT == -1 || minF == -1 || minT == -1 || hourF < 0 || hourF > 23 || hourT < 0 || hourT > 23 || minF < 0 || minF > 59 || minT < 0 || minT > 59)
            {
                MessageBox.Show(rm.GetString("invalidTimeFormat", culture));
                return;
            }

            dateTO = dtpTo.Value;
            dateFrom = dtpFrom.Value;
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, hourF, minF, 0);
            dateTO = new DateTime(dateTO.Year, dateTO.Month, dateTO.Day, hourT, minT, 0);

            if (dateFrom > dateTO)
            {
                MessageBox.Show(rm.GetString("dateFromBigerThanTO", culture));
                return;
            }

            fromDate = dateFrom;
            toDate = dateTO;

            Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, dateFrom, dateTO, null);
            Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);

            List<IOPairProcessedTO> IOPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateFrom, dateTO, pass_types);
            stop.Stop();
            log.writeLog("First" + stop.ElapsedMilliseconds);
            lblTotal.Text = "" + IOPairs.Count;
            
            stop.Start();
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DetailedData" + dtpFrom.Value.Date.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                
            StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);
            string column = "";
            foreach (string col in columns) {
                column += col + "\t";
            }
            writer.WriteLine(column);
            foreach (IOPairProcessedTO iopair in IOPairs)
            {
                string result = "";
                if (dictionaryEmpl.ContainsKey(iopair.EmployeeID))
                {

                  
                    result += iopair.EmployeeID + "\t";
                    result += dictionaryEmpl[iopair.EmployeeID].FirstAndLastName + "\t";
                    WorkingUnitTO costCentre = new WorkingUnitTO();
                    if (dictWuList.ContainsKey(dictionaryEmpl[iopair.EmployeeID].WorkingUnitID)
                        && dictWuList.ContainsKey(dictWuList[dictionaryEmpl[iopair.EmployeeID].WorkingUnitID].ParentWorkingUID)
                        && dictWuList.ContainsKey(dictWuList[dictWuList[dictionaryEmpl[iopair.EmployeeID].WorkingUnitID].ParentWorkingUID].ParentWorkingUID))
                        costCentre = dictWuList[dictWuList[dictWuList[dictionaryEmpl[iopair.EmployeeID].WorkingUnitID].ParentWorkingUID].ParentWorkingUID];

                    List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                    DateTime ioPairDate = new DateTime();

                    if (iopair.IOPairDate.Year != -1 && iopair.IOPairDate.Month != -1 && iopair.IOPairDate.Day != -1) { ioPairDate = new DateTime(iopair.IOPairDate.Year, iopair.IOPairDate.Month, iopair.IOPairDate.Day); }
                    if (iopair.EmployeeID != -1)
                    {
                        if (emplSchedules.ContainsKey(iopair.EmployeeID))
                            schedules = emplSchedules[iopair.EmployeeID];

                        WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(ioPairDate, schedules, schemas);

                        if (sch.TimeSchemaID != -1)
                        {
                            result += sch.Description + "\t";
                        }
                        else
                        {
                            result += "N/A" + "\t";
                        }


                        List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(ioPairDate, schedules, schemas);

                        int cycleDay = 0;
                        string intervalsString = "";
                        foreach (WorkTimeIntervalTO interval in intervals)
                        {
                            cycleDay = interval.DayNum + 1;

                            intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat);

                            if (!interval.Description.Trim().Equals(""))
                                intervalsString += "(" + interval.Description.Trim() + ")";

                            intervalsString += "; ";
                        }

                        result += cycleDay.ToString().Trim() + "\t";
                        result += intervalsString.Trim() + "\t";
                        
                    }
                    Calendar cal = culture.Calendar;
                    Int32 week = cal.GetWeekOfYear(DateTime.Today, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
                    result += week + "\t";
                    result += iopair.IOPairDate.Year + "\t";
                    result += iopair.IOPairDate.Month + "\t";
                    result += iopair.IOPairDate.Day + "\t";
                    result += costCentre.Name + "\t";
                    result += costCentre.Description + "\t";
                   
                    if (ascoDict.ContainsKey(iopair.EmployeeID))
                    {
                        result += (ascoDict[iopair.EmployeeID]).NVarcharValue6 + "\t";
                       
                    }
                    else
                        result += " " + "\t";
                      

                    if (dictEmplTypes.ContainsKey(dictionaryEmpl[iopair.EmployeeID].EmployeeTypeID))
                         result += dictEmplTypes[dictionaryEmpl[iopair.EmployeeID].EmployeeTypeID].EmployeeTypeName + "\t";
                    else
                        result += " " + "\t";
                       
                    result += iopair.StartTime.ToString("HH:mm:ss") + "\t";
                    result += iopair.EndTime.ToString("HH:mm:ss") + "\t";
                   
                    bool isAltLang = !NotificationController.GetLanguage().Equals(Constants.Lang_sr);

                    if (dictionaryPassTypes.ContainsKey(iopair.PassTypeID))
                    {
                        if (isAltLang)
                            result += dictionaryPassTypes[iopair.PassTypeID].DescAlt + "\t";
                            
                        else
                            result += dictionaryPassTypes[iopair.PassTypeID].Description + "\t";
                         
                    }
                    result += Math.Round((iopair.EndTime - iopair.StartTime).TotalHours, 2) + "\t";
                    
                    result += iopair.Desc + "\t";

                   
                    writer.WriteLine(result);
                    
                }
            }
            writer.Close();
            stop.Stop();
            log.writeLog("Second" + stop.ElapsedMilliseconds);
            lblTotal.Text = "TOTAL: " + IOPairs.Count;
           
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Stopwatch stop = new Stopwatch();
            stop.Start();
           
           
            string filePath = Constants.logFilePath + "Temp\\" + "DetaiedDataReport" + "_" + DateTime.Now.ToString("mm.ss.ff") + "\\" + "";

            filePath += "DetailedDataReport" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
            string Pathh = Directory.GetParent(filePath).FullName;
            if (!Directory.Exists(Pathh))
            {
                Directory.CreateDirectory(Pathh);
            }
            if (File.Exists(filePath))
                File.Delete(filePath);

            string[] columns = new string[18];
            columns[0] = rm.GetString("hdrEmplID", culture);
            columns[1] = rm.GetString("hdrEmployee", culture);
            columns[2] = rm.GetString("hdrCostCenter", culture);
            columns[3] = rm.GetString("hdrCostCenterDesc", culture);
            columns[4] = rm.GetString("hdrBranch", culture);
            columns[5] = rm.GetString("hdrEmplType", culture);
            columns[6] = rm.GetString("hdrWeek", culture);
            columns[7] = rm.GetString("hdrYear", culture);
            columns[8] = rm.GetString("hdrMonth", culture);
            columns[9] = rm.GetString("hdrDay", culture);
            columns[10] = rm.GetString("hdrSartTime", culture);
            columns[11] = rm.GetString("hdrEndTime", culture);
            columns[12] = rm.GetString("hdrWageType", culture);
            columns[13] = rm.GetString("total", culture);
            columns[14] = rm.GetString("hdrDescription", culture);
            columns[15] = rm.GetString("rbWSDay", culture);
            columns[16] = rm.GetString("hdrCycleDay", culture);
            columns[17] = rm.GetString("hdrIntervals", culture);

            string date =rm.GetString("lblPeriod",culture)+" "+ fromDate.ToString("dd.MM.yyyy HH:mm") + " - " + toDate.ToString("dd.MM.yyyy HH:mm");
            string wu = "";
            if (rbWU.Checked)
                wu = rm.GetString("hdrWorkingUnit", culture) + ": " + ((WorkingUnitTO)cbWU.SelectedItem).Name;
            else
                wu = rm.GetString("hdrOrgUnit", culture) + ": " + ((OrganizationalUnitTO)cbOU.SelectedItem).Name;

            //ExportToExcel.CreateExcelDocument(set, filePath, columns, wu, date);
            stop.Stop();
            log.writeLog("Third" + stop.ElapsedMilliseconds);

            MessageBox.Show(rm.GetString("generateFinish", culture));
        }

    }
}
