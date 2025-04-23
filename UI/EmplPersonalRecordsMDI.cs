using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;

using TransferObjects;
using Common;
using Util;
using Reports;

namespace UI
{
    public partial class EmplPersonalRecordsMDI : Form
    {        
        // Debug log
        DebugLog log;

        // Language
        private CultureInfo culture;
        private ResourceManager rm;

        //permissions
        ApplUserTO logInUser;
        string language;
        Hashtable itemTable = new Hashtable();
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        //mdiChild forms
        ArrayList childForms;        

        //tabControl indexes
        private const string PASSES_INDEX = "Passes";
        private const string IOPAIRS_INDEX = "IOPairs";
        private const string ABSENCES_INDEX = "EmployeeAbsences";
        private const string EXIT_PERM_INDEX = "ExitPermissions";
        private const string EXTRA_HRS_INDEX = "ExtraHours";
        private const string VACATIONS_INDEX = "VacationEvidence";
        private const string EMPL_REP_INDEX = "EmployeesReports";
        private const string EMPL_REP_ANALITIC_INDEX = "EmployeeAnaliticReport";

        //selected employee ID
        private List<WorkingUnitTO> wUnits;
        private string wuString = "";   
   
        //list of indexes of open codes 
        ArrayList activeTabs = new ArrayList();

        //tabPage freshness
        const int fresh = 0;
        const int unFresh = 1;
        
        bool loading;

        public EmplPersonalRecordsMDI()
        {
            loading = true;

            InitializeComponent();
            this.CenterToScreen();

            childForms = new ArrayList();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            // Set Language
            language = NotificationController.GetLanguage();
            switch (language)
            {
                case Constants.Lang_sr:
                    itemTable = Constants.ItemIDsSerbian;
                    break;
                case Constants.Lang_en:
                    itemTable = Constants.ItemIDsEnglish;
                    break;
                case Constants.Lang_fi:
                    itemTable = Constants.ItemIDsFi;
                    break;
            }
            culture = CultureInfo.CreateSpecificCulture(language);
            rm = new ResourceManager("UI.Resource", typeof(Locations).Assembly);
            setLanguage();
           
        }

        private void setLanguage()
        {
            try
            {
                //Form text
                this.Text = rm.GetString("EmplPersonalRecordsMDI", culture);

                //tab page's text
                this.IOPairs.Text = rm.GetString("IOPairsTitle", culture);
                this.Passes.Text = rm.GetString("menuPasses", culture);
                this.EmployeeAbsences.Text = rm.GetString("menuEmployeeAbsences", culture) ;
                this.ExitPermissions.Text = rm.GetString("ExitPermissionsForm", culture) ;
                this.ExtraHours.Text = rm.GetString("extraHoursForm", culture) ;
                this.VacationEvidence.Text = rm.GetString("vacationEvidence", culture);
                this.EmployeesReports.Text = rm.GetString("menuRepEmpl", culture) + " ";
                this.EmployeeAnaliticReport.Text = rm.GetString("menuEmplReport", culture) ;

                //add blanko to tab page text
                foreach (TabPage tp in tcChildForms.TabPages)
                {
                    tp.Text += " ";
                }
            
                //label's text
                this.lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                this.lblName.Text = rm.GetString("lblEmployee", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblCriteriaShange.Text = rm.GetString("lblCriteriaShange", culture);

                //menu item's text
                this.cascadeToolStripMenuItem.Text = rm.GetString("cascade", culture);
                this.tileHorizontalToolStripMenuItem.Text = rm.GetString("tileHorizontal", culture);
                this.tileVerticalToolStripMenuItem.Text = rm.GetString("tileVertical", culture);
                this.closeAllToolStripMenuItem.Text = rm.GetString("closeAll", culture);
                this.minimizeAllToolStripMenuItem.Text = rm.GetString("minimizeAll", culture);
                this.maximizeAllToolStripMenuItem.Text = rm.GetString("maximizeAll", culture);
                this.windowsMenu.Text = rm.GetString("windowsMenu", culture);
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

       
        private void populateEmployeeCombo(List<EmployeeTO> array)
        {
            try
            {
                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                array.Insert(0, empl);

                foreach (EmployeeTO employee in array)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                cbEmplName.DataSource = array;
                cbEmplName.DisplayMember = "LastName";
                cbEmplName.ValueMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }      
      
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }
             
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Form childForm in MdiChildren)
                {
                    childForm.Close();
                }
                childForms = new ArrayList();
                tcChildForms.Refresh();
                tcChildForms.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI. CloseAllToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }       
        private void EmplPersonalRecordsMDI_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpTo.Value = DateTime.Now;

                this.Cursor = Cursors.WaitCursor;

                getWorkingUnits();               
                populateWorkingUnitCombo();

                Employee empl = new Employee();
                List<EmployeeTO> emplList = empl.SearchByWU(wuString);
                populateEmployeeCombo(emplList);
                loading = false;

                setTabControl();                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.EmplPersonalRecordsMDI_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setTabControl()
        {
            try
           {
                //drawing active form's
               this.tcChildForms.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
               this.tcChildForms.DrawItem += new DrawItemEventHandler(this.tabControl1_DrawItem);

                //on form opening select first tab in order to open first form
                tcChildForms_SelectedIndexChanged(this, new TabControlEventArgs(tcChildForms.SelectedTab, tcChildForms.SelectedIndex, TabControlAction.Selected));

                for (int tabIndex = 0; tabIndex < tcChildForms.TabCount;tabIndex++)
                {
                    setTabVisibility(tabIndex);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.setTabControl(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setTabVisibility(int tabIndex)
        {
            try
            {
                List<WorkingUnitTO> wunits = new List<WorkingUnitTO>();
                
                string itemID = "";
                itemID  = (string)itemTable[((TabPage)tcChildForms.TabPages[tabIndex]).Name];
                NotificationController.SetCurrentMenuItemID(itemID);
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                int permission;
                bool readPermission = false;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[itemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
                }

                switch (((TabPage)tcChildForms.TabPages[tabIndex]).Name)
                {
                    case PASSES_INDEX:
                        wunits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PassPurpose);                   
                        break;
                    case IOPAIRS_INDEX:
                        wunits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.IOPairPurpose);
                        break;
                    case ABSENCES_INDEX:
                        wunits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.AbsencesPurpose);
                        break;
                    case EXIT_PERM_INDEX:
                        wunits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PermissionPurpose);
                        break;
                    case EXTRA_HRS_INDEX:
                        wunits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ExtraHoursPurpose);
                        break;
                    case VACATIONS_INDEX:
                        wunits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.VacationPurpose);
                        break;
                    case EMPL_REP_INDEX:
                        wunits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                        break;
                    case EMPL_REP_ANALITIC_INDEX:
                        wunits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                        break;                  
                }
                if (!readPermission || wunits.Count == 0)
                      tcChildForms.TabPages[tabIndex].Dispose();
                    
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.setTabVisibility(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void getWorkingUnits()
        {
            try
            {
                wUnits = new List<WorkingUnitTO>();
                List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                if (logInUser != null)
                {
                    wuList = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), "");
                }
                List<int> wuIDs = new List<int>();
                foreach (WorkingUnitTO wu in wuList)
                {
                    if (!wuIDs.Contains(wu.WorkingUnitID))
                    {
                        wUnits.Add(wu);
                        wuIDs.Add(wu.WorkingUnitID);
                    }
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }
                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.getWorkingUnits(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void tcChildForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //on CloseAll select -1 and no tab will be selected, but we need this not to get exception
                if (tcChildForms.SelectedIndex < 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                Form form = new Form();
                bool exist = false;

                //if there is any form check if form for selected tab is allready opened
                if (childForms.Count > 0)
                {
                    foreach (Form childForm in this.childForms)
                    {
                        //Check for its corresponding MDI child form
                        if (((int)childForm.Tag) == tcChildForms.SelectedIndex)
                        {
                            //Activate the MDI child form
                            childForm.Select();
                            form = childForm;
                            exist = true;
                        }
                    }

                }
                //if form is not open, open it
                //add new form and search with selected criteria
                if (!exist)
                {
                    switch (tcChildForms.SelectedTab.Name)
                    {
                        case PASSES_INDEX:
                            Passes pass = new Passes();                           
                            addNewChildForm(pass);
                            pass.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date,chbHierarhicly.Checked);
                            break;
                        case IOPAIRS_INDEX:
                            IOPairs iopairs = new IOPairs();
                            addNewChildForm(iopairs);
                            iopairs.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                            break;
                        case ABSENCES_INDEX:
                            EmployeeAbsences absence = new EmployeeAbsences();
                            addNewChildForm(absence);
                            absence.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                            break;
                        case EXIT_PERM_INDEX:
                            ExitPermissions exitPerm = new ExitPermissions();
                            addNewChildForm(exitPerm);
                            exitPerm.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                            break;
                        case EXTRA_HRS_INDEX:
                            ExtraHours extraHrs = new ExtraHours();
                            addNewChildForm(extraHrs);
                            extraHrs.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                            break;
                        case VACATIONS_INDEX:
                            VacationEvidence vacEvid = new VacationEvidence();
                            addNewChildForm(vacEvid);
                            vacEvid.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                            break;
                        case EMPL_REP_INDEX:
                            EmployeesReports emplRep = new EmployeesReports();
                            addNewChildForm(emplRep);
                            emplRep.MDIchangeSelectedEmployee((int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date);
                            break;
                        case EMPL_REP_ANALITIC_INDEX:
                            EmpoyeeAnaliticReport emplAnalitic = new EmpoyeeAnaliticReport();
                            addNewChildForm(emplAnalitic);
                            emplAnalitic.MDIchangeSelectedEmployee((int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date);
                            break;
                    }
                    //foreach tab in tabControl in tag is writen if it has form with fresh criteria
                    tcChildForms.SelectedTab.Tag = fresh;
                }
                //if form is open set criteria
                else
                {
                    if (tcChildForms.SelectedTab.Tag != null)
                    {
                        if ((int)tcChildForms.SelectedTab.Tag != fresh)
                        {
                            switch (tcChildForms.SelectedTab.Name)
                            {
                                case PASSES_INDEX:
                                    Passes passes = (Passes)form;
                                    passes.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                                    break;
                                case IOPAIRS_INDEX:
                                    IOPairs iopairs = (IOPairs)form;
                                    iopairs.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                                    break;
                                case ABSENCES_INDEX:
                                    EmployeeAbsences absence = (EmployeeAbsences)form;
                                    absence.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                                    break;
                                case EXIT_PERM_INDEX:
                                    ExitPermissions exitPerm = (ExitPermissions)form;
                                    exitPerm.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                                    break;
                                case EXTRA_HRS_INDEX:
                                    ExtraHours extraHrs = (ExtraHours)form;
                                    extraHrs.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                                    break;
                                case VACATIONS_INDEX:
                                    VacationEvidence vacEvid = (VacationEvidence)form;
                                    vacEvid.MDIchangeSelectedEmployee((int)cbWorkingUnit.SelectedValue, (int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date, chbHierarhicly.Checked);
                                    break;
                                case EMPL_REP_INDEX:
                                    EmployeesReports emplRep = (EmployeesReports)form;
                                    emplRep.MDIchangeSelectedEmployee((int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date);
                                    break;
                                case EMPL_REP_ANALITIC_INDEX:
                                    Reports.EmpoyeeAnaliticReport emplAnalitic = (EmpoyeeAnaliticReport)form;
                                    emplAnalitic.MDIchangeSelectedEmployee((int)cbEmplName.SelectedValue, dtpFrom.Value.Date, dtpTo.Value.Date);
                                    break;
                            }
                        }
                        tcChildForms.SelectedTab.Tag = fresh;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.tcChildForms_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        //seting freshness help us now if form is with changed criteria or not
        //in order to refresh form only if any criteria is changed, in some other case just open form,
        //so if we open form, change criteria on it deactivade it and again activated it would not change
        private void setFreshness()
        {
            try
            {
                foreach (Form childForm in this.childForms)
                {  
                    ((TabPage)tcChildForms.TabPages[(int)childForm.Tag]).Tag = unFresh;                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.setFreshness(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        //add new form and set event's for form closing and form acivated 
        private void addNewChildForm(Form childForm)
        {
            try
            {
                childForm.ControlBox = true;
                childForm.MdiParent = this;
                childForm.MaximizeBox = true;
                childForm.MinimizeBox = true;
                childForm.ShowIcon = true;
                childForm.FormBorderStyle = FormBorderStyle.Sizable;
                childForm.Activated += new EventHandler(this.formActivated);
                childForm.FormClosing += new FormClosingEventHandler(this.formClosing);
                string itemID = (string)itemTable[childForm.Name];               
                NotificationController.SetCurrentMenuItemID(itemID);
                childForm.AutoScroll = true;
                childForm.Tag = tcChildForms.SelectedIndex;
                childForm.Show();
                childForms.Add(childForm);
                childForm.Location = new Point(0, 0);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.addNewChildForm(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void formClosing(object obj, FormClosingEventArgs e)
        {
            try
            {
                Form form = this.ActiveMdiChild;
                if (childForms.Contains(form))
                    childForms.Remove(form);
                if (childForms.Count == 0)
                {
                    tcChildForms.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.formClosing(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        public void formActivated(object obj, EventArgs e)
        {
            try
            {
                Form form = this.ActiveMdiChild;
                if (childForms.Contains(form))
                {
                    int tabPageIndex = (int)form.Tag;
                    tcChildForms.SelectedIndex = tabPageIndex;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.formActivated(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                    cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    Employee empl = new Employee();
                    List<EmployeeTO> emplList = new List<EmployeeTO>();
                    string workUnitID = cbWorkingUnit.SelectedValue.ToString();

                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
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

                    emplList = empl.SearchByWU(workUnitID);

                    populateEmployeeCombo(emplList);
                }
                setFreshness();
                if (!loading)
                    tcChildForms_SelectedIndexChanged(this, new TabControlEventArgs(tcChildForms.SelectedTab, tcChildForms.SelectedIndex, TabControlAction.Selected));        
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
       
        private void minimizeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (Form child in childForms)
                {
                    child.WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.minimizeAllToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void maximizeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (Form child in childForms)
                {
                    child.WindowState = FormWindowState.Maximized;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.maximizeAllToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        //draw active and inactive tabs with diferent color
        private void tabControl1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Font fntTab;
                Brush bshBack;
                Brush bshFore;
                activeTabs = new ArrayList();
                foreach (Form f in childForms)
                {
                    activeTabs.Add((int)f.Tag);
                }
                if (activeTabs.Contains(e.Index))
                {
                    fntTab = this.tcChildForms.TabPages[e.Index].Font;
                    bshBack = new SolidBrush(SystemColors.ControlLight);
                    bshFore = new SolidBrush(Color.Black);
                }
                else
                {
                    fntTab = e.Font;
                    bshBack = new SolidBrush(SystemColors.ControlDark);
                    bshFore = new SolidBrush(Color.Black);
                }

                string tabName = this.tcChildForms.TabPages[e.Index].Text;
                StringFormat sftTab = new StringFormat();
                e.Graphics.FillRectangle(bshBack, e.Bounds);
                Rectangle recTab = e.Bounds;
                recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width + 5, recTab.Height - 4);
                e.Graphics.DrawString(tabName, fntTab, bshFore, recTab, sftTab);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.tabControl1_DrawItem(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    Employee empl = new Employee();
                    List<EmployeeTO> emplList = new List<EmployeeTO>();
                    string workUnitID = cbWorkingUnit.SelectedValue.ToString();

                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
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
                    emplList = empl.SearchByWU(workUnitID);

                    populateEmployeeCombo(emplList);                    
                }

                setFreshness();
                if(!loading)
                    tcChildForms_SelectedIndexChanged(this, new TabControlEventArgs(tcChildForms.SelectedTab, tcChildForms.SelectedIndex, TabControlAction.Selected));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbEmplName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                setFreshness();
                if (!loading)
                    tcChildForms_SelectedIndexChanged(this, new TabControlEventArgs(tcChildForms.SelectedTab, tcChildForms.SelectedIndex, TabControlAction.Selected));

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.cbEmplName_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                setFreshness();
                if (!loading)
                    tcChildForms_SelectedIndexChanged(this, new TabControlEventArgs(tcChildForms.SelectedTab, tcChildForms.SelectedIndex, TabControlAction.Selected));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.cbEmplName_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                setFreshness();
                if (!loading)
                    tcChildForms_SelectedIndexChanged(this, new TabControlEventArgs(tcChildForms.SelectedTab, tcChildForms.SelectedIndex, TabControlAction.Selected));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.cbEmplName_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            } 
        }

        private void EmplPersonalRecordsMDI_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (childForms.Count == 0)
                {
                    if (e.KeyCode.Equals(Keys.F1))
                    {
                        Util.Misc.helpManualHtml(this.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPersonalRecordsMDI.EmplPersonalRecordsMDI_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
       
    }
}
