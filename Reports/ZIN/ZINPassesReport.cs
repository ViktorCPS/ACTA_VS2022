using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Collections;

using Util;
using Common;
using TransferObjects;

namespace Reports.ZIN
{
    public partial class ZINPassesReport : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;

        List<PassTO> currentPassesList;
        private int sortOrder;
        private int sortField;
        private int startIndex;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        Filter filter;
        List<EmployeeTO> currentEmplArray;

        private int passesListCount = 0;

        private PassTO currentPass = null;

        //asco info for each employee id
        Dictionary<int, EmployeeAsco4TO> emplNoTable = new Dictionary<int, EmployeeAsco4TO>();

        // List View indexes
        const int EvIndex = 0;
        const int EmployeeNameIndex = 1;
        const int WUIndex = 2;
        const int DateIndex = 3;
        const int EnterIndex = 4;
        const int ExitIndex = 5;
        const int LocationIndex = 6;

        Dictionary<int, GateTO> gates = new Dictionary<int, GateTO>();
        Dictionary<int, string> categoryEmpl = new Dictionary<int, string>();

        public ZINPassesReport()
        {
            InitializeComponent();
            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINPassesReport).Assembly);
            setLanguage();
            logInUser = NotificationController.GetLogInUser();  
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
                this.Text = rm.GetString("ZINPassesReport", culture);

                // group box text
                this.gbSearch.Text = rm.GetString("gbSearch", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                gbEmplGroup.Text = rm.GetString("gbEmplGroup", culture);
               
                // check box text
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);

                // label's text
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);

                //radio button's text
                rbWorkingUnit.Text = rm.GetString("lblWU", culture);
                rbCategory.Text = rm.GetString("rbCategory", culture);
                
                // list view initialization
                lvPasses.BeginUpdate();
                lvPasses.Columns.Add(rm.GetString("hdrEvidAccount", culture), (lvPasses.Width - 10) / 7 - 20, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrEmployeeName", culture), (lvPasses.Width - 10) / 7 + 50, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvPasses.Width - 10) / 7 +50, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrDate", culture), (lvPasses.Width - 10) / 7 - 25, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrTimeEnter", culture), (lvPasses.Width - 10) / 7 - 40, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrTimeExit", culture), (lvPasses.Width - 10) / 7 - 40, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrLocation", culture), (lvPasses.Width - 10) / 7 + 25, HorizontalAlignment.Left);
                lvPasses.EndUpdate();
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPassesReport.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void ZINPassesReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                sortOrder = Constants.sortAsc;
                sortField = ZINPassesReport.DateIndex;
                startIndex = 0;
                this.lblTotal.Visible = false;

                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                dtpTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                List<GateTO> list = new Gate().Search();

                foreach(GateTO g in list)
                {
                    if(!gates.ContainsKey(g.GateID))
                    {
                        gates.Add(g.GateID,g);
                    }
                }

                currentPass = new PassTO();
                currentPassesList = new List<PassTO>();

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
                populateCategoryCombo();
               

                clearListView();
                
                if (!wuString.Equals(""))
                {
                    /*currentPassesList = currentPass.SearchInterval(-1, "", new DateTime(0), new DateTime(0), -1, -1, wuString, dtFrom.Value, dtTo.Value);
                    currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentPassesList, startIndex);
                    passesListCount = currentPassesList.Count;*/
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplPassPrivilege", culture));
                }                                                
                

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

              
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPassesReport.ZINPassesReport_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }

        /// <summary>
        /// Populate Working Unit Combo Box
        /// </summary>
        private void populateWorkingUnitCombo()
        {
            try
            {
                ArrayList wuArray = new ArrayList();

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
                log.writeLog(DateTime.Now + " ZINPassesReport.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " ZINPassesReport.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
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
                if (wuID < 0)
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
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPassesReport.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void clearListView()
        {
            lvPasses.BeginUpdate();
            lvPasses.Items.Clear();
            lvPasses.EndUpdate();

            lvPasses.Invalidate();

            btnPrev.Visible = false;
            btnNext.Visible = false;

            passesListCount = 0;
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWU.SelectedValue is int)
                {
                    if ((int)cbWU.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPassesReport.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (wuString.Equals(""))
                {
                    MessageBox.Show(rm.GetString("noEmplPassPrivilege", culture));
                    return;
                }

                if (currentEmplArray.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noPassesFound", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                

                string selectedWU = wuString;
                if (rbWorkingUnit.Checked)
                {
                    if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                    {
                        selectedWU = cbWU.SelectedValue.ToString();

                        WorkingUnit wu = new WorkingUnit();
                        if ((int)this.cbWU.SelectedValue != -1 && chbHierarhicly.Checked)
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
                                wuList = wu.FindAllChildren(wuList);
                            selectedWU = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                selectedWU += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (selectedWU.Length > 0)
                            {
                                selectedWU = selectedWU.Substring(0, selectedWU.Length - 1);
                            }
                        }
                    }
                }
                int count = 0;
                if (rbWorkingUnit.Checked)
                {
                    count = new Pass().SearchIntervalCount(dtpFrom.Value.Date, dtpTo.Value.Date, selectedWU, new DateTime(), new DateTime(1, 1, 1, 23, 59, 0));

                }
                else
                {
                    if(!categoryEmpl[cbCategory.SelectedIndex].Equals(""))
                    count = new Pass().SearchIntervalCountForZINReport(dtpFrom.Value.Date, dtpTo.Value.Date, categoryEmpl[cbCategory.SelectedIndex]);
                }
                if (count > Constants.maxRecords)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("passesGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        if (rbWorkingUnit.Checked)
                        currentPassesList = new Pass().SearchInterval(dtpFrom.Value.Date, dtpTo.Value.Date, selectedWU, new DateTime(), new DateTime(1, 1, 1, 23, 59, 0));
                        else
                        currentPassesList = new Pass().SearchIntervalForZINReport(dtpFrom.Value.Date, dtpTo.Value.Date, categoryEmpl[cbCategory.SelectedIndex]);
                    }
                    else
                    {
                        currentPassesList.Clear();
                        clearListView();
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        if (rbWorkingUnit.Checked)
                        currentPassesList = new Pass().SearchInterval(dtpFrom.Value.Date, dtpTo.Value.Date, selectedWU, new DateTime(), new DateTime(1, 1, 1, 23, 59, 0));
                    else
                        currentPassesList = new Pass().SearchIntervalForZINReport(dtpFrom.Value.Date, dtpTo.Value.Date, categoryEmpl[cbCategory.SelectedIndex]);
                    }
                    else
                    {
                        currentPassesList.Clear();
                        clearListView();
                    }
                }

                List<PassTO> temp = new List<PassTO>();
                foreach(PassTO p in currentPassesList)
                {
                    if (p.EventTime >= dtpFrom.Value && p.EventTime <= dtpTo.Value)
                        temp.Add(p);
                }
                currentPassesList = temp;

                //currentPass.Clear();
                passesListCount = currentPassesList.Count;
                if (currentPassesList.Count > 0)
                {
                    List<EmployeeAsco4TO> ascoList = new EmployeeAsco4().Search();
                    emplNoTable = new Dictionary<int, EmployeeAsco4TO>();
                    foreach (EmployeeAsco4TO emplInfo in ascoList)
                    {
                        if (!emplNoTable.ContainsKey(emplInfo.EmployeeID))
                        {
                            emplNoTable.Add(emplInfo.EmployeeID, emplInfo);
                        }
                    }

                    startIndex = 0;
                    currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentPassesList, startIndex);
                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentPassesList.Count.ToString().Trim();
                }
                else //else if (count == 0)
                {
                    //MessageBox.Show(rm.GetString("noPassesFound", culture));
                    clearListView();
                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                }

                //clearListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPassesReport.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Populate Direction Combo Box
        /// </summary>
        private void populateCategoryCombo()
        {
            try
            {
                cbCategory.Items.Add(Constants.SiemensCategories[Constants.SiemensCategory1].ToString());
                cbCategory.Items.Add(Constants.SiemensCategories[Constants.SiemensCategory2].ToString());
                cbCategory.Items.Add(Constants.SiemensCategories[Constants.SiemensCategory3].ToString());
                cbCategory.Items.Add(Constants.SiemensCategories[Constants.SiemensCategory4].ToString());
                cbCategory.Items.Add(Constants.SiemensCategories[Constants.SiemensCategory5].ToString());

                cbCategory.SelectedIndex = 0;
                categoryEmpl.Add(Constants.SiemensCategory1, getCategoryString(Constants.SiemensCategory1));
                categoryEmpl.Add(Constants.SiemensCategory2, getCategoryString(Constants.SiemensCategory2));
                categoryEmpl.Add(Constants.SiemensCategory3, getCategoryString(Constants.SiemensCategory3));
                categoryEmpl.Add(Constants.SiemensCategory4, getCategoryString(Constants.SiemensCategory4));
                categoryEmpl.Add(Constants.SiemensCategory5, getCategoryString(Constants.SiemensCategory5));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPassesReport.populateDirectionCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private string getCategoryString(int category)
        {
            string employees = "";
            try
            {
                EmployeeAsco4 empl = new EmployeeAsco4();
                empl.EmplAsco4TO.IntegerValue2 = category;
                List<EmployeeAsco4TO> list = empl.Search();
                foreach (EmployeeAsco4TO e in list)
                {
                    employees += e.EmployeeID + ", ";
                }
                if (employees.Length > 0)
                {
                   employees = employees.Substring(0, employees.Length - 2);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ZINPassesReport.getCategoryString(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return employees;
        }

        private void populateListView(List<PassTO> passesList, int startIndex)
        {
            try
            {
                if (passesList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvPasses.BeginUpdate();
                lvPasses.Items.Clear();

                if (passesList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < passesList.Count))
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
                        if (lastIndex >= passesList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = passesList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            PassTO pass = passesList[i];                            
                            ListViewItem item = new ListViewItem();
                            if (emplNoTable.ContainsKey(pass.EmployeeID))
                            {
                                //place no somewhere for sortting
                                pass.LocationID = emplNoTable[pass.EmployeeID].IntegerValue1;
                                item.Text = pass.LocationID.ToString();
                            }
                            else
                                item.Text = "N/A";
                            item.SubItems.Add(pass.EmployeeName.Trim());
                            item.SubItems.Add(pass.WUName.Trim());
                            if (!pass.EventTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                item.SubItems.Add(pass.EventTime.ToString("dd/MM/yyyy"));
                                if (pass.Direction.Equals(Constants.DirectionIn))
                                {
                                    item.SubItems.Add(pass.EventTime.ToString("HH:mm"));
                                    item.SubItems.Add("");
                                }
                                else
                                {
                                    item.SubItems.Add("");
                                    item.SubItems.Add(pass.EventTime.ToString("HH:mm"));                                    
                                }
                            }
                            else
                            {
                                item.SubItems.Add("N/A");
                                item.SubItems.Add("N/A");
                                item.SubItems.Add("N/A");
                            }
                            if(gates.ContainsKey(pass.GateID))
                            item.SubItems.Add( gates[pass.GateID].Name);
                            else
                            item.SubItems.Add("N/A");
                           
                            item.Tag = pass;

                            lvPasses.Items.Add(item);
                        }
                    }
                }

                lvPasses.EndUpdate();
                lvPasses.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
            	#region Inner Class for sorting Array List of Passes

		/*
		 *  Class used for sorting Array List of Passes
		*/

		private class ArrayListSort:IComparer<PassTO>    
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}        
			
			public int Compare(PassTO x, PassTO y)        
			{
				PassTO pass1 = null;
				PassTO pass2 = null;

				if (compOrder == Constants.sortAsc)
				{
					pass1 = x;
					pass2 = y;
				}
				else
				{
					pass1 = y;
					pass2 = x;
				}

				switch(compField)            
				{                
					case ZINPassesReport.EvIndex:
                            return pass1.LocationID.CompareTo(pass2.LocationID);
					case ZINPassesReport.EmployeeNameIndex:                       
                            return pass1.EmployeeName.CompareTo(pass2.EmployeeName);
					case ZINPassesReport.WUIndex:
                            return pass1.WUName.CompareTo(pass2.WUName);
					case ZINPassesReport.DateIndex:						
					case ZINPassesReport.EnterIndex:
					case ZINPassesReport.ExitIndex:
                        return pass1.EventTime.CompareTo(pass2.EventTime);
					case ZINPassesReport.LocationIndex:
                            return pass1.LocationName.CompareTo(pass2.LocationName);                   
					default:                    
						return pass1.EventTime.CompareTo(pass2.EventTime);
				}        
			}    
		}

		#endregion

        private void lvPasses_ColumnClick(object sender, ColumnClickEventArgs e)
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

                currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentPassesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.lvPasses_ColumnClick(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                populateListView(currentPassesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populateListView(currentPassesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnNext_Click(): " + ex.Message + "\n");
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

                   
                        if (currentPassesList.Count > 0)
                        {
                            // Table Definition for Crystal Reports
                            DataSet dataSetCR = new DataSet();
                            DataTable tableCR = new DataTable("employee_passes");
                            DataTable tableI = new DataTable("images");

                            tableCR.Columns.Add("ev_account", typeof(System.String));
                            tableCR.Columns.Add("empl_name", typeof(System.String));
                            tableCR.Columns.Add("wu_name", typeof(System.String));
                            tableCR.Columns.Add("date", typeof(System.String));
                            tableCR.Columns.Add("imageID", typeof(byte));
                            tableCR.Columns.Add("enter", typeof(System.String));
                            tableCR.Columns.Add("exit", typeof(System.String));
                            tableCR.Columns.Add("date_time", typeof(System.DateTime));
                            tableCR.Columns.Add("location", typeof(System.String));
                            
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

                            foreach (PassTO pass in currentPassesList)
                            {
                                DataRow row = tableCR.NewRow();
                                if (emplNoTable.ContainsKey(pass.EmployeeID))
                                {
                                    //place no somewhere for sortting
                                    row["ev_account"] = emplNoTable[pass.EmployeeID].IntegerValue1.ToString();
                                }
                                else
                                    row["ev_account"] = "N/A";
                                row["empl_name"] = pass.EmployeeName;
                                row["wu_name"] = pass.WUName;
                                row["date"] = pass.EventTime.ToString("dd.MM.yyyy");
                                if (pass.Direction.Equals(Constants.DirectionIn))
                                {
                                    row["enter"] = pass.EventTime.ToString("HH:mm"); ;
                                    row["exit"] = "";
                                }
                                else
                                {
                                    row["enter"] = "";
                                    row["exit"] = pass.EventTime.ToString("HH:mm");                                     
                                }

                                row["date_time"] = pass.EventTime;
                                if (gates.ContainsKey(pass.GateID))
                                    row["location"] = gates[pass.GateID].Name;
                                else
                                    row["location"] = "N/A";                              

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
                            string selEmplGroup = rbWorkingUnit.Text;
                            if (rbCategory.Checked)
                            {
                                selEmplGroup = rbCategory.Text;
                                selWorkingUnit = cbCategory.Text;
                            }
                            else
                            {
                                if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                                    selWorkingUnit = cbWU.Text;
                            }
                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                Reports.ZIN.ZIN_sr.ZINEmployeePassesCRView view = new Reports.ZIN.ZIN_sr.ZINEmployeePassesCRView(dataSetCR,
                                    dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmplGroup);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                Reports.ZIN.ZIN_en.ZINEmployeePassesCRView_en view = new Reports.ZIN.ZIN_en.ZINEmployeePassesCRView_en(dataSetCR,
                                    dtpFrom.Value, dtpTo.Value, selWorkingUnit);
                                view.ShowDialog(this);
                            }
                           
                        } //if (currentPassesList.Count > 0)
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

        private void rbCategory_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbCategory.Enabled = rbCategory.Checked;
                cbWU.Enabled = chbHierarhicly.Enabled = !rbCategory.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbWorkingUnit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbCategory.Enabled = !rbWorkingUnit.Checked;
                cbWU.Enabled = chbHierarhicly.Enabled = rbWorkingUnit.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

      
        
    }
}