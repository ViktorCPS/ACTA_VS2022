using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Util;
using Common;
using TransferObjects;

namespace Reports.ZIN
{
    public partial class ZINPresencePreview : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;
        private List<WorkingUnitTO> wUnits;
        private string wuString = "";
        List<EmployeeTO> currentEmplArray;

        Filter filter;
        bool control = false;

        public ZINPresencePreview()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINPassesReport).Assembly);
            setLanguage();
            logInUser = NotificationController.GetLogInUser();  
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void setLanguage()
        {
            try
            {
                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnReport.Text = rm.GetString("btnReport", culture);

                // Form name
                this.Text = rm.GetString("ZINPresencePreview", culture);

                // group box's text
                this.gbFilter.Text = rm.GetString("gbFilter", culture);
                this.gbDate.Text = rm.GetString("gbDate", culture);
                gbTime.Text = rm.GetString("gbTimeInterval", culture);
                gbPresenceInterval.Text = rm.GetString("gbPresenceInterval", culture);

                // label's text
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblHrs.Text = rm.GetString("lblHrs", culture);
                lblDate.Text = rm.GetString("lblDate", culture);
                lblMinPresence.Text = rm.GetString("lblMoreThan", culture);
                label3.Text = rm.GetString("lblHrs", culture);

                lvWU.BeginUpdate();
                lvWU.Columns.Add(rm.GetString("hdrWorkingUnit", culture),lvWU.Width-25);
                lvWU.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPresencePreview.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<int> selWorkingUnits = new List<int>();
                if (lvWU.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selWU", culture));
                    return;
                }
                else
                {
                    foreach (ListViewItem item in lvWU.SelectedItems)
                    {
                        selWorkingUnits.Add((int)item.Tag);
                    }
                }

                string selectedWU = wuString;

                List<IOPairTO> currentIOPairsList = new IOPair().SearchWithType(dtpDateFrom.Value.Date, dtpDateFrom.Value.Date, selectedWU, -1);

                Dictionary<int, int> presentEmplInWU = new Dictionary<int, int>();
                //key is working unit id value is list of employee id's
                Dictionary<int, List<int>> WUEmployees = new Dictionary<int, List<int>>();
                //key is employeeID value is employee
                Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();

                foreach (int wuID in selWorkingUnits)
                {
                    if (!WUEmployees.ContainsKey(wuID))
                    {
                        WUEmployees.Add(wuID, new List<int>());
                    }
                }
                foreach (EmployeeTO empl in currentEmplArray)
                {
                    if (!selWorkingUnits.Contains(empl.WorkingUnitID))
                    {
                        continue;
                    }
                    else
                    {
                        if (!WUEmployees.ContainsKey(empl.WorkingUnitID))
                        {
                            WUEmployees.Add(empl.WorkingUnitID, new List<int>());
                        }
                        if (!presentEmplInWU.ContainsKey(empl.WorkingUnitID))
                        {
                            presentEmplInWU.Add(empl.WorkingUnitID, 0);
                        }
                        if (!employees.ContainsKey(empl.EmployeeID))
                        {
                            employees.Add(empl.EmployeeID, empl);
                        }
                        WUEmployees[empl.WorkingUnitID].Add(empl.EmployeeID);
                    }
                }

                List<int> counted = new List<int>();
                Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int, List<IOPairTO>>();
                foreach (IOPairTO pair in currentIOPairsList)
                {
                    if (pair.EndTime != new DateTime() && pair.StartTime != new DateTime() &&
                        pair.StartTime.TimeOfDay < dtpHoursTo.Value.TimeOfDay && pair.EndTime.TimeOfDay > dtpHoursFrom.Value.TimeOfDay)
                    {
                        if (employees.ContainsKey(pair.EmployeeID))
                        {
                            if (pair.StartTime.TimeOfDay < dtpHoursFrom.Value.TimeOfDay)
                            {
                                pair.StartTime = dtpHoursFrom.Value;
                            }

                            if (pair.EndTime.TimeOfDay > dtpHoursTo.Value.TimeOfDay)
                            {
                                pair.EndTime = dtpHoursTo.Value;
                            }

                            if (!emplPairs.ContainsKey(pair.EmployeeID))
                            {
                                emplPairs.Add(pair.EmployeeID, new List<IOPairTO>());
                            }

                            emplPairs[pair.EmployeeID].Add(pair);
                        }
                    }
                }

                int referentValue = (int)nudPresenceInterval.Value;
                foreach (int emplID in emplPairs.Keys)
                {
                    TimeSpan totalPairsDuration = new TimeSpan();
                    List<IOPairTO> pairs = emplPairs[emplID];

                    foreach (IOPairTO pair in pairs)
                    {
                        totalPairsDuration += pair.EndTime - pair.StartTime;
                    }

                    if (totalPairsDuration.Hours >= referentValue)
                    {
                        int workingUnit = employees[emplID].WorkingUnitID;
                        if (presentEmplInWU.ContainsKey(workingUnit))
                        {
                            presentEmplInWU[workingUnit] = presentEmplInWU[workingUnit] + 1;
                        }
                    }
                }

                //foreach (IOPair pair in currentIOPairsList)
                //{

                //    if ((pair.StartTime.TimeOfDay >= dtpHoursFrom.Value.TimeOfDay && pair.StartTime.TimeOfDay < dtpHoursTo.Value.TimeOfDay)
                //        || (pair.EndTime.TimeOfDay >= dtpHoursFrom.Value.TimeOfDay && pair.EndTime.TimeOfDay < dtpHoursTo.Value.TimeOfDay))
                //    {
                //        if (employees.ContainsKey(pair.EmployeeID)&&!counted.Contains(pair.EmployeeID))
                //        {
                //            int workingUnit = employees[pair.EmployeeID].WorkingUnitID;
                //            if (presentEmplInWU.ContainsKey(workingUnit))
                //            {
                //                presentEmplInWU[workingUnit] = presentEmplInWU[workingUnit] + 1;
                //                counted.Add(pair.EmployeeID);
                //            }
                //        }
                //    }
                //}           


                if (wUnits.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("wUnits");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("working_unit", typeof(System.String));
                    tableCR.Columns.Add("present", typeof(System.Int32));
                    tableCR.Columns.Add("total", typeof(System.Int32));
                    tableCR.Columns.Add("absent", typeof(System.Int32));
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

                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        //if (tableCR.Rows.Count == 10)
                        //    break;
                        DataRow row = tableCR.NewRow();
                        row["working_unit"] = wu.Name;
                        if (WUEmployees.ContainsKey(wu.WorkingUnitID))
                        {
                            int absent = 0;
                            if (presentEmplInWU.ContainsKey(wu.WorkingUnitID))
                            {
                                row["present"] = presentEmplInWU[wu.WorkingUnitID];
                                absent = WUEmployees[wu.WorkingUnitID].Count - presentEmplInWU[wu.WorkingUnitID];
                            }
                            else
                            {
                                row["present"] = 0;
                            }
                            row["total"] = WUEmployees[wu.WorkingUnitID].Count;
                            row["absent"] = absent;

                        }
                        else
                        {
                            continue;
                        }
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

                    ZIN_sr.ZINPresencePreviewView preview = new Reports.ZIN.ZIN_sr.ZINPresencePreviewView(dataSetCR, dtpDateFrom.Value, dtpHoursFrom.Value, dtpHoursTo.Value);
                    preview.ShowDialog();

                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPresencePreview.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { this.Cursor = Cursors.Arrow; } 
        }

        /// <summary>
        /// Populate WorkingUnit Combo Box
        /// </summary>
        private void populateWorkingUnitListView()
        {
            try
            {
                Dictionary<WorkingUnitTO, int> allWU = new WorkingUnit().SearchAllWUHierarchicly();

                // populate list view
                lvWU.BeginUpdate();
                foreach (WorkingUnitTO wu in allWU.Keys)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = wu.Name;
                    item.Tag = wu.WorkingUnitID;
                    lvWU.Items.Add(item);
                }
                lvWU.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPresencePreview.populateWorkingUnitListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void ZINPresencePreview_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.CenterToScreen();
                dtpHoursFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                dtpHoursTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0);
                wUnits = new List<WorkingUnitTO>();
                populateWorkingUnitListView();
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                currentEmplArray = new List<EmployeeTO>();
                currentEmplArray = new Employee().SearchByWU(wuString);
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPresencePreview.ZINPresencePreview_Load(): " + ex.Message + "\n");
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
                MessageBox.Show(ex.Message);
            }
        }

        private void lvWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (control)
                    return;
                if (lvWU.SelectedItems.Count > 12)
                {
                    control = true;

                    for (int i = 11; i < lvWU.SelectedItems.Count; i++)
                    {
                        lvWU.SelectedItems[i].Selected = false;
                    }
                    control = false;
                }
                    
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPresencePreview.lvWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}