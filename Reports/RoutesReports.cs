using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Common;
using TransferObjects;
using Util;

namespace Reports
{
    public partial class RoutesReports : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;
        ArrayList routesSchedules;

        // report data
        string selWorkingUnit = "*";
        string selEmployee = "*";
        string selRoute = "*";

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        private string routeType;

        Filter filter;

        public RoutesReports(string routeType)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(RoutesReports).Assembly);

                setLanguage();

                logInUser = NotificationController.GetLogInUser();
                routesSchedules = new ArrayList();

                DateTime now = DateTime.Now.Date;
                dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
                dtpToDate.Value = now;

                rbAnalytic.Checked = true;

                this.routeType = routeType;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                // form text
                this.Text = rm.GetString("routesSchReports", culture);

                // group box's text
                this.gbCriteria.Text = rm.GetString("gbCriteria", culture);
                this.gbReport.Text = rm.GetString("gbReportType", culture);
                this.gbFilter.Text = rm.GetString("gbFilter", culture);

                // label's text
                this.lblWU.Text = rm.GetString("lblWU", culture);
                this.lblEmpl.Text = rm.GetString("lblEmployee", culture);
                this.lblRouteName.Text = rm.GetString("lblRoute", culture);
                this.lblFromDate.Text = rm.GetString("lblFrom", culture);
                this.lblToDate.Text = rm.GetString("lblTo", culture);
                this.lblRepType.Text = rm.GetString("lblRepType", culture);
                this.lblStatus.Text = rm.GetString("lblStatus", culture);

                // button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnGenerate.Text = rm.GetString("btnGenerateReport", culture);

                // radio button's text
                this.rbAnalytic.Text = rm.GetString("rbAnalytic", culture);
                this.rbSyntetic.Text = rm.GetString("rbSyntetic", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbWU.SelectedValue is int)
                {
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " RoutesReports.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (wuString.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("noEmplReportPrivilege", culture));
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    searchRoutes();
                    generateReport();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " RoutesReports.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void RoutesReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                wUnits = new List<WorkingUnitTO>();
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

                if (routeType.Equals(Constants.routeTag))
                {
                    populateWorkingUnits();
                    populateEmployeeCombo(-1);
                }
                else
                {
                    lblWU.Visible = cbWU.Visible = false;
                    populateRouteEmployeeCombo();
                }

                populateRoutesCombo();
                populateStatusCombo();
                

                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " RoutesReports.RoutesReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateRoutesCombo()
        {
            try
            {
                ArrayList routes = new SecurityRouteHdr().Search("", "");
                SecurityRouteHdr route = new SecurityRouteHdr();
                route.Name = rm.GetString("all", culture);
                routes.Insert(0, route);

                cbRouteName.DataSource = routes;
                cbRouteName.DisplayMember = "Name";
                cbRouteName.ValueMember = "SecurityRouteID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.populateRoutes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateStatusCombo()
        {
            try
            {
                ArrayList statuses = new ArrayList();
                statuses.Add(rm.GetString("all", culture));
                statuses.Add(Constants.statusCompleted);
                statuses.Add(Constants.statusNotCompleted);
                statuses.Add(Constants.statusPartially);
                statuses.Add(Constants.statusScheduled);

                cbStatus.DataSource = statuses;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.populateRoutes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateWorkingUnits()
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
                log.writeLog(DateTime.Now + " RoutesReports.populateWorkingUnits(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                List<EmployeeTO> emplArray = new List<EmployeeTO>();

                Employee empl = new Employee();
                if (wuID < 0)
                {
                    emplArray = empl.SearchByWU(wuString);
                }
                else
                {
                    empl.EmplTO.WorkingUnitID = wuID;
                    emplArray = empl.Search();
                }

                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO emplTO = new EmployeeTO();
                emplTO.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, emplTO);

                cbEmpl.DataSource = emplArray;
                cbEmpl.DisplayMember = "LastName";
                cbEmpl.ValueMember = "EmployeeID";
                cbEmpl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.populateEmployeeCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateRouteEmployeeCombo()
        {
            try
            {
                ArrayList routeEmplArray = new ArrayList();

                routeEmplArray = new SecurityRoutesEmployee().SearchByWU(wuString);

                SecurityRoutesEmployee routeEmpl = new SecurityRoutesEmployee();
                routeEmpl.EmployeeName = rm.GetString("all", culture);
                routeEmplArray.Insert(0, routeEmpl);

                cbEmpl.DataSource = routeEmplArray;
                cbEmpl.DisplayMember = "EmployeeName";
                cbEmpl.ValueMember = "EmployeeID";
                cbEmpl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.populateRouteEmployeeCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }
                
        private void searchRoutes()
        {
            try
            {
                routesSchedules = new SecurityRouteSchedule().Search((int)cbEmpl.SelectedValue,
                    (int)cbRouteName.SelectedValue, dtpFromDate.Value.Date, dtpToDate.Value.Date);

                if (routesSchedules.Count > 0)
                {
                    // set report parameters
                    if (cbWU.SelectedIndex >= 0)
                        selWorkingUnit = cbWU.Text;
                    if (routeType.Equals(Constants.routeTerminal))
                        selWorkingUnit = "";
                    if (cbEmpl.SelectedIndex >= 0)
                        selEmployee = cbEmpl.Text;
                    if (cbRouteName.SelectedIndex >= 0)
                        selRoute = cbRouteName.Text;

                    Hashtable routeDetails = new Hashtable();

                    ArrayList routeDtl = new ArrayList();

                    if (routeType.Equals(Constants.routeTag))
                    {
                        routeDtl = new SecurityRouteHdr().SearchDetailsTag((int)cbRouteName.SelectedValue);
                    }
                    else
                    {
                        routeDtl = new SecurityRouteHdr().SearchDetailsTerminal((int)cbRouteName.SelectedValue);
                    }

                    foreach (SecurityRouteDtl dtl in routeDtl)
                    {
                        if (!routeDetails.ContainsKey(dtl.SecurityRouteID))
                        {
                            routeDetails.Add(dtl.SecurityRouteID, new ArrayList());
                        }

                        ((ArrayList)routeDetails[dtl.SecurityRouteID]).Add(dtl);
                    }

                    if (routeType.Equals(Constants.routeTag))
                    {
                        setStatusTag(routeDetails);
                    }
                    else
                    {
                        setStatusTerminal(routeDetails);
                    }

                    if (cbStatus.SelectedIndex > 0)
                    {
                        ArrayList routes = (ArrayList)routesSchedules.Clone();
                        routesSchedules.Clear();
                        foreach (SecurityRouteSchedule routeSch in routes)
                        {
                            if (routeSch.Status.Trim().Equals(cbStatus.Text.Trim()))
                            {
                                routesSchedules.Add(routeSch);
                            }
                        }
                    }
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.searchRoutes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateReport()
        {
            try
            {
                if (routesSchedules.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("security_routes_schedule");
                    DataTable tableDtl = new DataTable("security_routes_sch_dtl");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("route_sch_id", typeof(int));
                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("date", typeof(System.String));
                    tableCR.Columns.Add("route", typeof(System.String));
                    tableCR.Columns.Add("status", typeof(System.String));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    tableDtl.Columns.Add("route_sch_id", typeof(int));
                    tableDtl.Columns.Add("gate", typeof(System.String));
                    tableDtl.Columns.Add("time_scheduled", typeof(System.String));
                    tableDtl.Columns.Add("time_realized", typeof(System.String));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableDtl);
                    dataSetCR.Tables.Add(tableI);

                    int schCounter = 0;
                    foreach (SecurityRouteSchedule sch in routesSchedules)
                    {
                        DataRow row = tableCR.NewRow();

                        row["route_sch_id"] = schCounter;
                        row["employee"] = sch.EmployeeName.Trim();
                        row["date"] = sch.Date.ToString("dd.MM.yyyy").Trim();
                        row["route"] = sch.RouteName.Trim(); ;
                        row["status"] = sch.Status.Trim();
                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();

                        if (rbAnalytic.Checked)
                        {
                            ArrayList routeDtl = new ArrayList();
                            List<PassTO> passes = new List<PassTO>();
                            ArrayList routes = new ArrayList();

                            if (routeType.Equals(Constants.routeTag))
                            {
                                routeDtl = new SecurityRouteHdr().SearchDetailsTag(sch.SecurityRouteID);
                                Pass pass = new Pass();
                                pass.PssTO.EmployeeID = sch.EmployeeID;
                                passes = pass.SearchInterval(sch.Date, sch.Date, "", new DateTime(1, 1, 1, 0, 0, 0), new DateTime(1, 1, 1, 23, 59, 0));
                            }
                            else
                            {
                                routeDtl = new SecurityRouteHdr().SearchDetailsTerminal(sch.SecurityRouteID);
                                routes = new SecurityRoutesLog().SearchInterval(sch.EmployeeID, -1, "", sch.Date, sch.Date, wuString);
                            }

                            foreach (SecurityRouteDtl dtl in routeDtl)
                            {
                                List<PassTO> dtlPasses = new List<PassTO>();
                                ArrayList dtlRoutes = new ArrayList();

                                if (routeType.Equals(Constants.routeTag))
                                {
                                    dtlPasses = findDtlPasses(dtl, passes);
                                }
                                else
                                {
                                    dtlRoutes = findDtlLogs(dtl, routes);
                                }

                                if (dtlPasses.Count == 0 && dtlRoutes.Count == 0)
                                {
                                    DataRow rowDtl = tableDtl.NewRow();
                                    rowDtl["route_sch_id"] = schCounter;
                                    rowDtl["gate"] = dtl.GateName.Trim();
                                    rowDtl["time_scheduled"] = dtl.TimeFrom.ToString("HH:mm") + "-" + dtl.TimeTo.ToString("HH:mm");
                                    rowDtl["time_realized"] = "";

                                    tableDtl.Rows.Add(rowDtl);
                                    tableDtl.AcceptChanges();
                                }
                                else
                                {
                                    if (routeType.Equals(Constants.routeTag))
                                    {
                                        foreach (PassTO pass in dtlPasses)
                                        {
                                            DataRow rowDtlPass = tableDtl.NewRow();
                                            rowDtlPass["route_sch_id"] = schCounter;
                                            rowDtlPass["gate"] = dtl.GateName.Trim();
                                            rowDtlPass["time_scheduled"] = dtl.TimeFrom.ToString("HH:mm") + "-" + dtl.TimeTo.ToString("HH:mm");
                                            rowDtlPass["time_realized"] = pass.EventTime.ToString("dd.MM.yyyy HH:mm");

                                            tableDtl.Rows.Add(rowDtlPass);
                                            tableDtl.AcceptChanges();
                                        }
                                    }
                                    else
                                    {
                                        foreach (SecurityRoutesLog log in dtlRoutes)
                                        {
                                            DataRow rowDtlPass = tableDtl.NewRow();
                                            rowDtlPass["route_sch_id"] = schCounter;
                                            rowDtlPass["gate"] = dtl.GateName.Trim();
                                            rowDtlPass["time_scheduled"] = dtl.TimeFrom.ToString("HH:mm") + "-" + dtl.TimeTo.ToString("HH:mm");
                                            rowDtlPass["time_realized"] = log.EventTime.ToString("dd.MM.yyyy HH:mm");

                                            tableDtl.Rows.Add(rowDtlPass);
                                            tableDtl.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }

                        schCounter++;
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    if (rbAnalytic.Checked)
                    {
                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.SecurityRoutesSchAnalyticCRView_sr view = new Reports.Reports_sr.SecurityRoutesSchAnalyticCRView_sr(dataSetCR,
                                dtpFromDate.Value.Date, dtpToDate.Value.Date, selWorkingUnit, selEmployee, selRoute);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.SecurityRoutesSchAnalyticCRView_en view = new Reports.Reports_en.SecurityRoutesSchAnalyticCRView_en(dataSetCR,
                                dtpFromDate.Value.Date, dtpToDate.Value.Date, selWorkingUnit, selEmployee, selRoute);
                            view.ShowDialog(this);
                        }
                    }
                    else if (rbSyntetic.Checked)
                    {
                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.SecurityRoutesSchSynteticCRView_sr view = new Reports.Reports_sr.SecurityRoutesSchSynteticCRView_sr(dataSetCR,
                                dtpFromDate.Value.Date, dtpToDate.Value.Date, selWorkingUnit, selEmployee, selRoute);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.SecurityRoutesSchSynteticCRView_en view = new Reports.Reports_en.SecurityRoutesSchSynteticCRView_en(dataSetCR,
                                dtpFromDate.Value.Date, dtpToDate.Value.Date, selWorkingUnit, selEmployee, selRoute);
                            view.ShowDialog(this);
                        }
                    }
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.generateReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private List<PassTO> findDtlPasses(SecurityRouteDtl dtl, List<PassTO> passes)
        {
            List<PassTO> dtlPasses = new List<PassTO>();

            try
            {
                foreach (PassTO pass in passes)
                {
                    if (pass.GateID == dtl.GateID && pass.EventTime.TimeOfDay >= dtl.TimeFrom.TimeOfDay
                        && pass.EventTime.TimeOfDay <= dtl.TimeTo.TimeOfDay)
                    {
                        dtlPasses.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.findDtlPasses(): " + ex.Message + "\n");
                throw ex;
            }

            return dtlPasses;
        }

        private ArrayList findDtlLogs(SecurityRouteDtl dtl, ArrayList passes)
        {
            ArrayList dtlPasses = new ArrayList();

            try
            {
                foreach (SecurityRoutesLog log in passes)
                {
                    if (log.PointID == dtl.GateID && log.EventTime.TimeOfDay >= dtl.TimeFrom.TimeOfDay
                        && log.EventTime.TimeOfDay <= dtl.TimeTo.TimeOfDay)
                    {
                        dtlPasses.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.findDtlLogs(): " + ex.Message + "\n");
                throw ex;
            }

            return dtlPasses;
        }

        private void setStatusTag(Hashtable routeDetails)
        {
            ArrayList dtlPasses = new ArrayList();
            Dictionary<int, List<PassTO>> passesTable = new Dictionary<int, List<PassTO>>();

            try
            {
                Pass pass1 = new Pass();
                pass1.PssTO.EmployeeID = (int)cbEmpl.SelectedValue;
                List<PassTO> passes = pass1.SearchInterval(dtpFromDate.Value.Date, dtpToDate.Value.Date, "", new DateTime(1, 1, 1, 0, 0, 0), new DateTime(1, 1, 1, 23, 59, 0));

                foreach (PassTO pass in passes)
                {
                    if (!passesTable.ContainsKey(pass.EmployeeID))
                    {
                        passesTable.Add(pass.EmployeeID, new List<PassTO>());
                    }

                    passesTable[pass.EmployeeID].Add(pass);
                }

                foreach (SecurityRouteSchedule sch in routesSchedules)
                {
                    int counter = 0;

                    if (routeDetails.ContainsKey(sch.SecurityRouteID))
                    {
                        foreach (SecurityRouteDtl detail in (ArrayList)routeDetails[sch.SecurityRouteID])
                        {
                            if (passesTable.ContainsKey(sch.EmployeeID))
                            {
                                foreach (PassTO pass in passesTable[sch.EmployeeID])
                                {
                                    if (pass.GateID == detail.GateID &&
                                        pass.EventTime.Date == sch.Date.Date
                                        && pass.EventTime.TimeOfDay >= detail.TimeFrom.TimeOfDay
                                        && pass.EventTime.TimeOfDay <= detail.TimeTo.TimeOfDay)
                                    {
                                        counter++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (sch.Date > DateTime.Now)
                    {
                        sch.Status = Constants.statusScheduled;
                    }
                    else if (counter == 0)
                    {
                        sch.Status = Constants.statusNotCompleted;
                    }
                    else if (counter == ((ArrayList)routeDetails[sch.SecurityRouteID]).Count)
                    {
                        sch.Status = Constants.statusCompleted;
                    }
                    else if (counter < ((ArrayList)routeDetails[sch.SecurityRouteID]).Count)
                    {
                        sch.Status = Constants.statusPartially;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.setStatusTag(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setStatusTerminal(Hashtable routeDetails)
        {
            ArrayList dtlLogs = new ArrayList();
            Hashtable logsTable = new Hashtable();

            try
            {
                ArrayList logs = new SecurityRoutesLog().SearchInterval((int)cbEmpl.SelectedValue, -1, "", 
                    dtpFromDate.Value.Date, dtpToDate.Value.Date, wuString);

                foreach (SecurityRoutesLog log in logs)
                {
                    if (!logsTable.ContainsKey(log.EmployeeID))
                    {
                        logsTable.Add(log.EmployeeID, new ArrayList());
                    }

                    ((ArrayList)logsTable[log.EmployeeID]).Add(log);
                }

                foreach (SecurityRouteSchedule sch in routesSchedules)
                {
                    int counter = 0;

                    if (routeDetails.ContainsKey(sch.SecurityRouteID))
                    {
                        foreach (SecurityRouteDtl detail in (ArrayList)routeDetails[sch.SecurityRouteID])
                        {
                            if (logsTable.ContainsKey(sch.EmployeeID))
                            {
                                foreach (SecurityRoutesLog log in ((ArrayList)logsTable[sch.EmployeeID]))
                                {
                                    if (log.PointID == detail.GateID &&
                                        log.EventTime.Date == sch.Date.Date
                                        && log.EventTime.TimeOfDay >= detail.TimeFrom.TimeOfDay
                                        && log.EventTime.TimeOfDay <= detail.TimeTo.TimeOfDay)
                                    {
                                        counter++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (sch.Date > DateTime.Now)
                    {
                        sch.Status = Constants.statusScheduled;
                    }
                    else if (counter == 0)
                    {
                        sch.Status = Constants.statusNotCompleted;
                    }
                    else if (counter == ((ArrayList)routeDetails[sch.SecurityRouteID]).Count)
                    {
                        sch.Status = Constants.statusCompleted;
                    }
                    else if (counter < ((ArrayList)routeDetails[sch.SecurityRouteID]).Count)
                    {
                        sch.Status = Constants.statusPartially;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RoutesReports.setStatusTerminal(): " + ex.Message + "\n");
                throw ex;
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
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}