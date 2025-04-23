using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Util;
using TransferObjects;
using Common;

namespace UI
{
    public partial class RecordsOfBreaks : Form
    {

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";
        List<EmployeeTO> currentEmplArray;
        List<GateTO> gateArray;
        List<IOPairTO> ioPairs = null;

        public RecordsOfBreaks()
        {
            InitializeComponent();

            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            logInUser = NotificationController.GetLogInUser();
            setLanguage();
        }

        private void setLanguage()
        {
            try
            {
                // button's text
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnReport.Text = rm.GetString("btnReport", culture);

                // Form name
                this.Text = rm.GetString("menuRecordsOfBreaks", culture);

                // group box text
                this.gbRecordsOfBreaks.Text = rm.GetString("gbRecordsOfBreaks", culture);

                // check box text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

                // label's text
                lblWU.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblGate.Text = rm.GetString("lblGate", culture);
                //lblPassType.Text = rm.GetString("lblPassType", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);

                // list view initialization
                lvRecords.BeginUpdate();
                lvRecords.Columns.Add(rm.GetString("hdrEmplID", culture), (lvRecords.Width - 7) / 7, HorizontalAlignment.Left);
                lvRecords.Columns.Add(rm.GetString("hdrEmployee", culture), (lvRecords.Width - 7) / 7, HorizontalAlignment.Left);
                lvRecords.Columns.Add(rm.GetString("hdrDate", culture), (lvRecords.Width - 7) / 7, HorizontalAlignment.Left);
                lvRecords.Columns.Add(rm.GetString("hdrStart", culture), (lvRecords.Width - 7) / 7, HorizontalAlignment.Left);
                lvRecords.Columns.Add(rm.GetString("hdrEnd", culture), (lvRecords.Width - 7) / 7, HorizontalAlignment.Left);
                lvRecords.Columns.Add(rm.GetString("hdrPassType", culture), (lvRecords.Width - 7) / 7, HorizontalAlignment.Left);
                lvRecords.Columns.Add(rm.GetString("hdrTime", culture), (lvRecords.Width - 7) / 7, HorizontalAlignment.Left);
                lvRecords.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RecordsOfBreaks.setLanguage(): " + ex.Message + "\n");
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate Employee Combo Box
        /// </summary>
        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                currentEmplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    currentEmplArray = new Employee().SearchByWU(wuString);
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
                    currentEmplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in currentEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmplArray.Insert(0, empl);

                cbEmployee.DataSource = currentEmplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate Pass Type Combo Box
        /// </summary>
        //private void populatePassTypeCombo() {
        //    try {
        //        PassType pt = new PassType();
        //        pt.PTypeTO.IsPass = Constants.passOnReader;
        //        pt.PTypeTO.IsBreak = 1;//   23.01.2020. BOJAN da vrati samo pauzu za restoran i pusenje
        //        ptArray = pt.Search();
        //        ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

        //        cbPassType.DataSource = ptArray;
        //        cbPassType.DisplayMember = "Description";
        //        cbPassType.ValueMember = "PassTypeID";
        //        cbPassType.SelectedIndex = 0;
        //    }
        //    catch (Exception ex) {
        //        log.writeLog(DateTime.Now + " Passes.populatePassTypeCombo(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        /// <summary>
        /// Populate Gate Combo Box
        /// </summary>
        private void populateGateCombo()
        {
            try
            {
                Gate g = new Gate();
                g.GTO.GateType = "NOT DEFAULT";
                gateArray = g.Search();
                gateArray.Insert(0, new GateTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), new DateTime(), -1, -1));

                cbGate.DataSource = gateArray;
                cbGate.DisplayMember = "Name";
                cbGate.ValueMember = "GateID";
                cbGate.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateGateCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void RecordsOfBreaks_Load(object sender, EventArgs e)
        {
            wUnits = new List<WorkingUnitTO>();

            if (logInUser != null)
            {
                wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PassPurpose);
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
            //populatePassTypeCombo();
            populateGateCombo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string emplIDs = "";
            if (!cbEmployee.Text.Trim().Equals("*"))
            {
                EmployeeTO empl = cbEmployee.SelectedItem as EmployeeTO;
                emplIDs = empl.EmployeeID.ToString();
            }
            else
            {

                for (int i = 0; i < currentEmplArray.Count; i++)
                {
                    if (currentEmplArray[i].EmployeeID != -1)
                    {
                        if (i != currentEmplArray.Count - 1)
                        {
                            emplIDs += currentEmplArray[i].EmployeeID.ToString() + ", ";
                        }
                        else
                        {
                            emplIDs += currentEmplArray[i].EmployeeID.ToString();
                        }
                    }
                }
            }

            string gateIDs = "";
            if (!cbGate.Text.Trim().Equals("*"))
            {
                GateTO gate = cbGate.SelectedItem as GateTO;
                gateIDs = gate.GateID.ToString();
            }
            else
            {
                for (int i = 0; i < gateArray.Count; i++)
                {
                    if (gateArray[i].GateID != -1)
                    {
                        if (i != gateArray.Count - 1)
                        {
                            gateIDs += gateArray[i].GateID.ToString() + ", ";
                        }
                        else
                        {
                            gateIDs += gateArray[i].GateID.ToString();
                        }
                    }
                }
            }

            ioPairs = new List<IOPairTO>();
            ioPairs = new IOPair().getIOPairsForBreaks(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1), emplIDs, gateIDs);
            populateListView(ioPairs);
        }

        private void populateListView(List<IOPairTO> ioPairs)
        {
            try
            {
                lvRecords.BeginUpdate();
                lvRecords.Items.Clear();

                if (ioPairs.Count > 0)
                {
                    foreach (IOPairTO ioPair in ioPairs)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = ioPair.EmployeeID.ToString();
                        item.SubItems.Add(ioPair.EmployeeLastName + " " + ioPair.EmployeeName);
                        item.SubItems.Add(ioPair.IOPairDate.ToString("dd.MM.yyyy"));
                        item.SubItems.Add(ioPair.StartTime.ToString("HH:mm:ss"));
                        item.SubItems.Add(ioPair.EndTime.ToString("HH:mm:ss"));
                        item.SubItems.Add(ioPair.PassType);
                        item.SubItems.Add(ioPair.Time);
                        item.Tag = ioPair;
                        lvRecords.Items.Add(item);
                    }
                }

                lvRecords.EndUpdate();
                lvRecords.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RecordsOfBreaks.populateListView(): " + ex.Message + "\n");
                throw new Exception("Exception: " + ex.Message);
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
                log.writeLog(DateTime.Now + " RecordsOfBreaks.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvRecords.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("records_of_breaks");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("employee_id", typeof(System.String));
                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("date", typeof(System.String));
                    tableCR.Columns.Add("start_time", typeof(String));
                    tableCR.Columns.Add("end_time", typeof(System.String));
                    tableCR.Columns.Add("pass_type", typeof(System.String));
                    tableCR.Columns.Add("time", typeof(System.String));
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

                    foreach (IOPairTO ioPair in ioPairs)
                    {
                        DataRow row = tableCR.NewRow();

                        row["employee_id"] = ioPair.EmployeeID.ToString();
                        row["employee"] = ioPair.EmployeeLastName + " " + ioPair.EmployeeName;
                        row["date"] = ioPair.IOPairDate.ToString("dd.MM.yyyy");
                        row["start_time"] = ioPair.StartTime.ToString("HH:mm:ss");
                        row["end_time"] = ioPair.EndTime.ToString("HH:mm:ss");
                        row["pass_type"] = ioPair.PassType;
                        row["time"] = ioPair.Time;

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
                    string selEmployee = "*";
                    string selGate = "*";
                    string fromTime = "";
                    string toTime = "";

                    if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                        selWorkingUnit = cbWU.Text;
                    if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                        selEmployee = cbEmployee.Text;
                    if (cbGate.SelectedIndex >= 0 && (int)cbGate.SelectedValue >= 0)
                        selGate = cbGate.Text;

                    fromTime = dtpFrom.Value.ToString("HH:mm");
                    toTime = dtpTo.Value.ToString("HH:mm");

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.RecordsOfBreaksCRView view = new Reports.Reports_sr.RecordsOfBreaksCRView(dataSetCR,
                            dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selGate);
                        view.ShowDialog(this);
                    }
                    //else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    //{
                    //    Reports.Reports_en.EmployeePassesCRView_en view = new Reports.Reports_en.EmployeePassesCRView_en(dataSetCR,
                    //        dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selGate, selDirection, selLocation,
                    //        fromTime, toTime, firstIn, lastOut, advanced);
                    //    view.ShowDialog(this);
                    //}
                    //else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    //{
                    //    Reports.Reports_fi.EmployeePassesCRView_fi view = new Reports.Reports_fi.EmployeePassesCRView_fi(dataSetCR,
                    //        dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selGate, selDirection, selLocation,
                    //        fromTime, toTime, firstIn, lastOut, advanced);
                    //    view.ShowDialog(this);
                    //}
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
