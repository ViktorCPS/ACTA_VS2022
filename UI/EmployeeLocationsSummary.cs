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
using System.Drawing.Printing;

using Util;
using Common;
using TransferObjects;

namespace UI
{
    public partial class EmployeeLocationsSummary : Form
    {
        private CultureInfo culture;
        ResourceManager rm;

        DebugLog log;
        ApplUserTO logInUser;

        Dictionary<int, LocationTO> locations = new Dictionary<int, LocationTO>();
        //Dictionary<int, EmployeeLocation> emplLocs = new Dictionary<int, EmployeeLocation>();
        //List<int> levels = new List<int>(); // Leveles of locations
        Dictionary<int, int> locNums = new Dictionary<int, int>(); // <location_id, number of present employees on that specific location>
        Dictionary<int, int> locNumsFinal = new Dictionary<int, int>(); // <location_id, number of present employees on that location including child locations>

        Dictionary<int, WorkingUnitTO> wunits = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, int> wuNums = new Dictionary<int, int>(); // <working_unit_id, number of employees from that specific working unit>
        Dictionary<int, int> wuNumsFinal = new Dictionary<int, int>(); // <working_unit_id, number of employees of that working unit including child locations>
        Dictionary<int, int> wuNumsPresent = new Dictionary<int, int>(); // <working_unit_id, number of present employees from that specific working unit>
        Dictionary<int, int> wuNumsPresentFinal = new Dictionary<int, int>(); // <working_unit_id, number of present employees of that working unit including child locations>
        Dictionary<int, int> wuNumsAbsentFinal = new Dictionary<int, int>(); // <working_unit_id, number of absent employees of that working unit including child locations>

        //properties for printing
        private PageSettings pgSettings = new PageSettings();
        PrintDocument printDocument1 = new PrintDocument();
        private Bitmap memoryImage;


        public EmployeeLocationsSummary()
        {
            try
            {
                InitializeComponent();

                this.CenterToScreen();

                // Init Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(EmployeeLocationsSummary).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.btnClose_Click(): " + ex.Message + "\n");
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
                // Form name
                this.Text = rm.GetString("emplLocSummaryForm", culture);

                // Tab names
                this.tbLocations.Text = rm.GetString("locTab", culture);
                this.tbWU.Text = rm.GetString("WUTab", culture);

                // button text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnPrint.Text = rm.GetString("btnPrint", culture);
                this.btnReport.Text = rm.GetString("btnReport", culture);

                //label's text
                this.lblPrint.Text = "";
                
                // list view initialization
                lvLocations.BeginUpdate();
                lvLocations.Columns.Add(rm.GetString("hdrLocation", culture), lvLocations.Width / 2 + 100, HorizontalAlignment.Left);
                lvLocations.Columns.Add(rm.GetString("hdrPresent", culture), lvLocations.Width / 2 - 105, HorizontalAlignment.Left);
                lvLocations.EndUpdate();

                lvWUByLoc.BeginUpdate();
                lvWUByLoc.Columns.Add(rm.GetString("hdrLocation", culture), lvWUByLoc.Width / 2 + 100, HorizontalAlignment.Left);
                lvWUByLoc.Columns.Add(rm.GetString("hdrPresent", culture), lvWUByLoc.Width / 2 - 105, HorizontalAlignment.Left);
                lvWUByLoc.EndUpdate();

                lvWU.BeginUpdate();
                lvWU.Columns.Add(rm.GetString("hdrWorkingUnit", culture), lvWU.Width / 3 + 135, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrPresent", culture), lvWU.Width / 3 - 70, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrNotPresent", culture), lvWU.Width / 3 - 70, HorizontalAlignment.Left);
                lvWU.EndUpdate();

                lvLocByWU.BeginUpdate();
                lvLocByWU.Columns.Add(rm.GetString("hdrWorkingUnit", culture), lvLocByWU.Width / 3 + 135, HorizontalAlignment.Left);
                lvLocByWU.Columns.Add(rm.GetString("hdrPresent", culture), lvLocByWU.Width / 3 - 70, HorizontalAlignment.Left);
                lvLocByWU.Columns.Add(rm.GetString("hdrNotPresent", culture), lvLocByWU.Width / 3 - 70, HorizontalAlignment.Left);
                lvLocByWU.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void getChildWU(List<WorkingUnitTO> wuList, int level, ref Dictionary<WorkingUnitTO, int> allWU)
        {
            try
            {
                foreach (WorkingUnitTO wu in wuList)
                {
                    if (wu.Status.Equals(Constants.statusActive))
                    {
                        if (!allWU.ContainsKey(wu))
                        {
                            allWU.Add(wu, level);
                            List<WorkingUnitTO> wuChildren = new WorkingUnit().SearchChildWU(wu.WorkingUnitID.ToString().Trim());

                            if (wuChildren.Count > 0)
                            {
                                getChildWU(wuChildren, level + 1, ref allWU);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getChildWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void EmployeeLocationsSummary_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                lblPrint.Text = NotificationController.GetLogInUser().Name.Trim() + "   " + DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo);
                // find all locations and initialize locations counters
                Dictionary<LocationTO, int> allLocations = new Location().SearchAllLocationsHierarchicly();
                
                foreach (LocationTO loc in allLocations.Keys)
                {
                    locations.Add(loc.LocationID, loc);
                    locNums.Add(loc.LocationID, 0);
                    locNumsFinal.Add(loc.LocationID, 0);
                }
                // find all working units and initialize working units counters
                Dictionary<WorkingUnitTO, int> allWU = new Dictionary<WorkingUnitTO, int>();

                List<WorkingUnitTO> rootWU = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                getChildWU(rootWU, 0, ref allWU);

                // find all working units and initialize working units counters
                //Dictionary<WorkingUnitTO, int> allWU = new WorkingUnit().SearchAllWUHierarchicly();           
                foreach (WorkingUnitTO wu in allWU.Keys)
                {
                    if(!wunits.ContainsKey(wu.WorkingUnitID))
                       wunits.Add(wu.WorkingUnitID, wu);
                    if (!wuNums.ContainsKey(wu.WorkingUnitID))
                    wuNums.Add(wu.WorkingUnitID, 0);
                    if (!wuNumsFinal.ContainsKey(wu.WorkingUnitID))
                    wuNumsFinal.Add(wu.WorkingUnitID, 0);
                    if (!wuNumsPresent.ContainsKey(wu.WorkingUnitID))
                    wuNumsPresent.Add(wu.WorkingUnitID, 0);
                    if (!wuNumsPresentFinal.ContainsKey(wu.WorkingUnitID))
                    wuNumsPresentFinal.Add(wu.WorkingUnitID, 0);
                    if (!wuNumsAbsentFinal.ContainsKey(wu.WorkingUnitID))
                    wuNumsAbsentFinal.Add(wu.WorkingUnitID, 0);
                }

                List<EmployeeTO> emplNums = new Employee().SearchEmplNumByWUnits();
                foreach (EmployeeTO empl in emplNums)
                {
                    if (wuNums.ContainsKey(empl.WorkingUnitID))
                    {
                        wuNums[empl.WorkingUnitID] = empl.AddressID; // AddressId contain number of employees in that working unit
                    }
                }

                List<EmployeeLocationTO> emplLocations = new EmployeeLocation().SearchEmployeeLocationsIn("");

                foreach (EmployeeLocationTO emplLoc in emplLocations)
                {
                    if (locations.ContainsKey(emplLoc.LocationID))
                    {
                        //emplLoc.LocName = locations[emplLoc.LocationID].Name;
                        locNums[emplLoc.LocationID] = emplLoc.ReaderID;
                    }
                }

                foreach (int id in locations.Keys)
                {
                    List<LocationTO> locs = new List<LocationTO>();
                    locs.Add(locations[id]);
                    locs = new Location().FindAllChildren(locs);

                    foreach (LocationTO loc in locs)
                    {
                        if (locNums.ContainsKey(loc.LocationID))
                        {
                            locNumsFinal[id] += locNums[loc.LocationID];
                        }
                    }
                }

                populateLocationListView(lvLocations);

                emplLocations = new EmployeeLocation().SearchEmployeeLocationsInByWU("");

                foreach (EmployeeLocationTO emplLoc in emplLocations)
                {
                    if (wunits.ContainsKey(emplLoc.WUID))
                    {
                        //emplLoc.WUName = wunits[emplLoc.WUID].Name;
                        wuNumsPresent[emplLoc.WUID] = emplLoc.ReaderID;
                    }
                }

                foreach (int id in wunits.Keys)
                {
                    List<WorkingUnitTO> wus = new List<WorkingUnitTO>();
                    wus.Add(wunits[id]);
                    wus = new WorkingUnit().FindAllChildren(wus);

                    foreach (WorkingUnitTO wu in wus)
                    {
                        if (wuNums.ContainsKey(wu.WorkingUnitID))
                        {
                            wuNumsFinal[id] += wuNums[wu.WorkingUnitID];
                        }

                        if (wuNumsPresent.ContainsKey(wu.WorkingUnitID))
                        {
                            wuNumsPresentFinal[id] += wuNumsPresent[wu.WorkingUnitID];
                        }
                    }

                    wuNumsAbsentFinal[id] = wuNumsFinal[id] - wuNumsPresentFinal[id];
                }

                populateWUListView(lvWU);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.EmployeeLocationsSummary_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateLocationListView(ListView lv)
        {
            try
            {                
                // populate list view
                lv.Items.Clear();
                lv.BeginUpdate();
                foreach (int id in locNumsFinal.Keys)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = locations[id].Name;
                    item.SubItems.Add(locNumsFinal[id].ToString().Trim()); // this is number of employees present on this location
                    item.Tag = locations[id];
                    lv.Items.Add(item);
                }
                lv.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.populateLocationListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateWUListView(ListView lv)
        {
            try
            {
                // populate list view
                lv.Items.Clear();
                lv.BeginUpdate();
                foreach (int id in wuNumsPresentFinal.Keys)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = wunits[id].Name;
                    item.SubItems.Add(wuNumsPresentFinal[id].ToString().Trim()); // this is number of employees present from this working unit
                    item.SubItems.Add(wuNumsAbsentFinal[id].ToString().Trim()); // this is number of employees absent from this working unit
                    item.Tag = wunits[id];
                    lv.Items.Add(item);
                }
                lv.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.populateLocByWUListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (tabControl1.SelectedTab.Equals(tbWU))
                {
                    if (wuNumsPresentFinal.Keys.Count > 0)
                    {

                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("wu_presence");
                        DataTable tableCRLoc = new DataTable("wu_loc_presence");


                        tableCR.Columns.Add("wu", typeof(System.String));
                        tableCR.Columns.Add("present", typeof(System.String));
                        tableCR.Columns.Add("absent", typeof(System.String));


                        tableCRLoc.Columns.Add("present", typeof(System.String));
                        tableCRLoc.Columns.Add("location", typeof(System.String));
                        tableCRLoc.Columns.Add("wu", typeof(System.String));

                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableCRLoc);

                        foreach (ListViewItem item in lvWU.Items)
                        {
                            DataRow row = tableCR.NewRow();

                            row["wu"] = item.Text;
                            row["present"] = item.SubItems[1].Text;
                            row["absent"] = item.SubItems[2].Text;


                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }
                        if (lvWU.SelectedIndices.Count > 0)
                        {
                            foreach (ListViewItem item in lvWUByLoc.Items)
                            {
                                DataRow row = tableCRLoc.NewRow();

                                row["location"] = item.Text;
                                row["wu"] = lvWU.SelectedItems[0].Text;
                                row["present"] = item.SubItems[1].Text;

                                tableCRLoc.Rows.Add(row);
                                tableCRLoc.AcceptChanges();

                            }
                        }
                        if (tableCR.Rows.Count == 0 && tableCRLoc.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                            return;
                        }


                        string selUser = logInUser.Name;


                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.SummaryEmployeeLocationCRView_sr view = new Reports.Reports_sr.SummaryEmployeeLocationCRView_sr(dataSetCR, selUser, 1);
                            view.ShowDialog(this);

                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.SummaryEmployeeLocationCRView_en view = new Reports.Reports_en.SummaryEmployeeLocationCRView_en(dataSetCR, selUser, 1);
                            view.ShowDialog(this);
                        }

                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                }
                else
                {
                    if (locNumsFinal.Keys.Count > 0)
                    {

                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("loc_wu_presence");
                        DataTable tableCRLoc = new DataTable("loc_presence");


                        tableCR.Columns.Add("wu", typeof(System.String));
                        tableCR.Columns.Add("present", typeof(System.String));
                        tableCR.Columns.Add("absent", typeof(System.String));
                        tableCR.Columns.Add("location", typeof(System.String));

                        tableCRLoc.Columns.Add("location", typeof(System.String));
                        tableCRLoc.Columns.Add("present", typeof(System.String));

                        tableCRLoc.Columns.Add("wu", typeof(System.String));


                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableCRLoc);
                        if (lvLocations.SelectedIndices.Count > 0)
                        {
                            foreach (ListViewItem item in lvLocByWU.Items)
                            {
                                DataRow row = tableCR.NewRow();

                                row["wu"] = item.Text;
                                row["present"] = item.SubItems[1].Text;
                                row["absent"] = item.SubItems[2].Text;
                                row["location"] = lvLocations.SelectedItems[0].Text;

                                tableCR.Rows.Add(row);
                                tableCR.AcceptChanges();
                            }
                        }
                        foreach (ListViewItem item in lvLocations.Items)
                        {
                            DataRow row = tableCRLoc.NewRow();

                            row["location"] = item.Text;
                            row["present"] = item.SubItems[1].Text;

                            tableCRLoc.Rows.Add(row);
                            tableCRLoc.AcceptChanges();

                        }
                        if (tableCR.Rows.Count == 0 && tableCRLoc.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                            return;
                        }

                        string selUser = logInUser.Name;


                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.SummaryEmployeeLocationCRView_sr view = new Reports.Reports_sr.SummaryEmployeeLocationCRView_sr(dataSetCR, selUser, 2);
                            view.ShowDialog(this);

                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.SummaryEmployeeLocationCRView_en view = new Reports.Reports_en.SummaryEmployeeLocationCRView_en(dataSetCR, selUser, 2);
                            view.ShowDialog(this);
                        }

                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

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

        private void lvLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvLocations.SelectedItems.Count > 0)
                {
                    foreach (int id in wunits.Keys)
                    {
                        wuNumsPresent[id] = 0;
                        wuNumsPresentFinal[id] = 0;
                        wuNumsAbsentFinal[id] = 0;
                    }

                    List<LocationTO> locs = new List<LocationTO>();
                    locs.Add((LocationTO)lvLocations.SelectedItems[0].Tag);
                    locs = new Location().FindAllChildren(locs);
                    string locString = "";
                    foreach (LocationTO loc in locs)
                    {
                        locString += loc.LocationID.ToString().Trim() + ",";
                    }
                    if (!locString.Trim().Equals(""))
                    {
                        locString = locString.Substring(0, locString.Length - 1);
                    }

                    List<EmployeeLocationTO> emplLocations = new EmployeeLocation().SearchEmployeeLocationsInByWU(locString);

                    foreach (EmployeeLocationTO emplLoc in emplLocations)
                    {
                        if (wunits.ContainsKey(emplLoc.WUID))
                        {
                            //emplLoc.WUName = wunits[emplLoc.WUID].Name;
                            wuNumsPresent[emplLoc.WUID] = emplLoc.ReaderID;
                        }
                    }

                    foreach (int id in wunits.Keys)
                    {
                        List<WorkingUnitTO> wus = new List<WorkingUnitTO>();
                        wus.Add(wunits[id]);
                        wus = new WorkingUnit().FindAllChildren(wus);

                        foreach (WorkingUnitTO wu in wus)
                        {
                            if (wuNumsPresent.ContainsKey(wu.WorkingUnitID))
                            {
                                wuNumsPresentFinal[id] += wuNumsPresent[wu.WorkingUnitID];
                            }
                        }

                        wuNumsAbsentFinal[id] = wuNumsFinal[id] - wuNumsPresentFinal[id];
                    }

                    populateWUListView(lvLocByWU);
                }
                else
                {
                    lvLocByWU.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.lvLocations_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvWU.SelectedItems.Count > 0)
                {

                    foreach (int id in locations.Keys)
                    {
                        locNums[id] = 0;
                        locNumsFinal[id] = 0;
                    }

                    List<WorkingUnitTO> wus = new List<WorkingUnitTO>();
                    wus.Add((WorkingUnitTO)lvWU.SelectedItems[0].Tag);
                    wus = new WorkingUnit().FindAllChildren(wus);
                    string wuString = "";
                    foreach (WorkingUnitTO wu in wus)
                    {
                        wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                    }
                    if (!wuString.Trim().Equals(""))
                    {
                        wuString = wuString.Substring(0, wuString.Length - 1);
                    }

                    List<EmployeeLocationTO> emplLocations = new EmployeeLocation().SearchEmployeeLocationsIn(wuString);

                    foreach (EmployeeLocationTO emplLoc in emplLocations)
                    {
                        if (locations.ContainsKey(emplLoc.LocationID))
                        {
                            //emplLoc.LocName = locations[emplLoc.LocationID].Name;
                            locNums[emplLoc.LocationID] = emplLoc.ReaderID;
                        }
                    }

                    foreach (int id in locations.Keys)
                    {
                        List<LocationTO> locs = new List<LocationTO>();
                        locs.Add(locations[id]);
                        locs = new Location().FindAllChildren(locs);

                        foreach (LocationTO loc in locs)
                        {
                            if (locNums.ContainsKey(loc.LocationID))
                            {
                                locNumsFinal[id] += locNums[loc.LocationID];
                            }
                        }
                    }

                    populateLocationListView(lvWUByLoc);
                }
                else
                {
                    lvWUByLoc.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.lvWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //lblPrint.Text = NotificationController.GetLogInUser().Name.Trim() + "   " + DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo);
                this.printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

                CaptureScreen();
                printDocument1.DefaultPageSettings = pgSettings;
                printDocument1.DefaultPageSettings.Landscape = true;
                PrintDialog dlg = new PrintDialog();
                dlg.Document = printDocument1;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
                //lblPrint.Text = "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.btnPrint_Click(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void printDocument1_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.Graphics.DrawImage(memoryImage, 0, 0);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.printDocument1_PrintPage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        private void CaptureScreen()
        {
            try
            {
                Graphics mygraphics = this.CreateGraphics();
                Size s = this.Size;
                memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
                Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                IntPtr dc1 = mygraphics.GetHdc();
                IntPtr dc2 = memoryGraphics.GetHdc();
                BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
                mygraphics.ReleaseHdc(dc1);
                memoryGraphics.ReleaseHdc(dc2);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocationsSummary.CaptureScreen(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}