using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Data.OleDb;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class AbsenceMassiveInput : Form
    {
        private const int emplIDIndex = 0;        
        private const int emplStringoneIndex = 1;
        private const int emplNameIndex = 2;        
        private const int emplCCIndex = 3;
        private const int emplCCDescIndex = 4;
        private const int emplBranchIndex = 5;

        private const int previewEmplIDIndex = 0;        
        private const int previewEmplNameIndex = 1;
        private const int previewDateIndex = 2;
        private const int previewStartIndex = 3;
        private const int previewEndIndex = 4;
        private const int previewPtBeforeIndex = 5;
        private const int previewPtAfterIndex = 6;
        private const int previewEmplStringoneIndex = 7;
        private const int previewEmplCCIndex = 8;
        private const int previewEmplCCDescIndex = 9;
        private const int previewEmplBranchIndex = 10;

        private const string xlsExt = ".XLS";
        private const string xlsxExt = ".XLSX";

        private int sortOrder = 0;
        private int sortField = 0;
        private int startIndex = 0;
        
        private ListViewEmployeeItemComparer _comp;
                
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();
                
        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, WorkTimeSchemaTO> schemaDict = new Dictionary<int, WorkTimeSchemaTO>();
        Dictionary<int, PassTypeTO> ptDict = new Dictionary<int, PassTypeTO>();
        Dictionary<int, PassTypeLimitTO> ptLimitsDict = new Dictionary<int, PassTypeLimitTO>();

        List<EmployeeTO> employeeList = new List<EmployeeTO>();
        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int,EmployeeAsco4TO>();
        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        string emplIDs = "";

        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplOldPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();        
        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplNewPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
        Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
        Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplNewCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
        Dictionary<int, Dictionary<DateTime, List<string>>> emplDateMessages = new Dictionary<int, Dictionary<DateTime, List<string>>>();
        List<IOPairProcessedTO> newPairsAll = new List<IOPairProcessedTO>();
        string emplIDsChanged = "";

        Dictionary<int, int> newTypesIndexValues = new Dictionary<int, int>();

        Dictionary<string, string> TypeCodes = new Dictionary<string, string>();
        Dictionary<string, string> TypeAdditionalCodes = new Dictionary<string, string>();
        Dictionary<int, string> TypesAdditionalDict = new Dictionary<int, string>();

        Dictionary<int, int> TypesListDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesListClosureDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesListLayoffDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesListStoppageDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesListHolidayDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesNewListDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesNewListClosureDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesNewListLayoffDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesNewListStoppageDict = new Dictionary<int, int>();
        Dictionary<int, int> TypesNewListHolidayDict = new Dictionary<int, int>();
        
        public AbsenceMassiveInput()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(AbsenceMassiveInput).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;
                chbHierarhiclyWU.Checked = true;
                chbHierachyOU.Checked = true;
                
                dtpDay.Value = DateTime.Now;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;

                btnPrev.Visible = btnNext.Visible = false;

                tbLogPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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

        #region Inner Class for sorting List of Employees
        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewEmployeeItemComparer : IComparer
        {
            private ListView _listView;

            public ListViewEmployeeItemComparer(ListView lv)
            {
                _listView = lv;
            }
            public ListView ListView
            {
                get { return _listView; }
            }

            private int _sortColumn = 0;

            public int SortColumn
            {
                get { return _sortColumn; }
                set { _sortColumn = value; }
            }

            public int Compare(object a, object b)
            {
                ListViewItem item1 = (ListViewItem)a;
                ListViewItem item2 = (ListViewItem)b;

                if (ListView.Sorting == SortOrder.Descending)
                {
                    ListViewItem temp = item1;
                    item1 = item2;
                    item2 = temp;
                }
                // Handle non Detail Cases
                return CompareItems(item1, item2);
            }

            public int CompareItems(ListViewItem item1, ListViewItem item2)
            {
                // Subitem instances
                ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
                ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

                // Return value based on sort column	
                switch (SortColumn)
                {
                    case AbsenceMassiveInput.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return Comparer.Default.Compare(id1, id2);
                        }
                    case AbsenceMassiveInput.emplStringoneIndex:
                    case AbsenceMassiveInput.emplNameIndex:
                    case AbsenceMassiveInput.emplCCIndex:
                    case AbsenceMassiveInput.emplCCDescIndex:
                    case AbsenceMassiveInput.emplBranchIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);

                }
            }
        }

        private class ArrayListSort : IComparer<IOPairProcessedTO>
        {
            private int compOrder;
            private int compField;
            private Dictionary<int, PassTypeTO> passTypes = new Dictionary<int,PassTypeTO>();
            private Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int,WorkingUnitTO>();
            private Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();
            private Dictionary<int, EmployeeAsco4TO> asco = new Dictionary<int, EmployeeAsco4TO>();

            public ArrayListSort(int sortOrder, int sortField, Dictionary<int, PassTypeTO> passTypes, Dictionary<int, WorkingUnitTO> wUnits, Dictionary<int, EmployeeTO> employees, Dictionary<int, EmployeeAsco4TO> asco)
            {
                compOrder = sortOrder;
                compField = sortField;

                this.passTypes = passTypes;
                this.wUnits = wUnits;
                this.employees = employees;
                this.asco = asco;
            }

            public int Compare(IOPairProcessedTO x, IOPairProcessedTO y)
            {
                IOPairProcessedTO p1 = null;
                IOPairProcessedTO p2 = null;

                PassTypeTO ptOld1 = new PassTypeTO();
                PassTypeTO ptOld2 = new PassTypeTO();
                PassTypeTO ptNew1 = new PassTypeTO();
                PassTypeTO ptNew2 = new PassTypeTO();

                EmployeeTO empl1 = new EmployeeTO();
                EmployeeTO empl2 = new EmployeeTO();

                EmployeeAsco4TO asco1 = new EmployeeAsco4TO();
                EmployeeAsco4TO asco2 = new EmployeeAsco4TO();

                WorkingUnitTO cc1 = new WorkingUnitTO();
                WorkingUnitTO cc2 = new WorkingUnitTO();

                if (compOrder == Constants.sortAsc)
                {
                    p1 = x;
                    p2 = y;
                }
                else
                {
                    p1 = y;
                    p2 = x;
                }

                if (passTypes.ContainsKey(p1.OldPassTypeID))
                    ptOld1 = passTypes[p1.OldPassTypeID];
                if (passTypes.ContainsKey(p2.OldPassTypeID))
                    ptOld2 = passTypes[p2.OldPassTypeID];
                if (passTypes.ContainsKey(p1.PassTypeID))
                    ptNew1 = passTypes[p1.PassTypeID];
                if (passTypes.ContainsKey(p2.PassTypeID))
                    ptNew2 = passTypes[p2.PassTypeID];

                if (employees.ContainsKey(p1.EmployeeID))
                    empl1 = employees[p1.EmployeeID];
                if (employees.ContainsKey(p2.EmployeeID))
                    empl2 = employees[p2.EmployeeID];

                if (asco.ContainsKey(p1.EmployeeID))
                    asco1 = asco[p1.EmployeeID];
                if (asco.ContainsKey(p2.EmployeeID))
                    asco2 = asco[p2.EmployeeID];

                cc1 = Common.Misc.getEmplCostCenter(empl1, wUnits, null);
                cc2 = Common.Misc.getEmplCostCenter(empl2, wUnits, null);

                switch (compField)
                {
                    case AbsenceMassiveInput.previewEmplIDIndex:
                        return p1.EmployeeID.CompareTo(p2.EmployeeID);
                    case AbsenceMassiveInput.previewEmplNameIndex:
                        return empl1.FirstAndLastName.CompareTo(empl2.FirstAndLastName);
                    case AbsenceMassiveInput.previewDateIndex:
                        return p1.IOPairDate.CompareTo(p2.IOPairDate);
                    case AbsenceMassiveInput.previewStartIndex:
                        return p1.StartTime.CompareTo(p2.StartTime);
                    case AbsenceMassiveInput.previewEndIndex:
                        return p1.EndTime.CompareTo(p2.EndTime);
                    case AbsenceMassiveInput.previewPtBeforeIndex:
                        {
                            if (NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_en.Trim().ToUpper()))
                                return ptOld1.DescriptionAltAndID.CompareTo(ptOld2.DescriptionAltAndID);
                            else
                                return ptOld1.DescriptionAndID.CompareTo(ptOld2.DescriptionAndID);
                        }
                    case AbsenceMassiveInput.previewPtAfterIndex:
                        {
                        if (NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_en.Trim().ToUpper()))
                                return ptNew1.DescriptionAltAndID.CompareTo(ptNew2.DescriptionAltAndID);
                            else
                                return ptNew1.DescriptionAndID.CompareTo(ptNew2.DescriptionAndID);
                        }
                    case AbsenceMassiveInput.previewEmplStringoneIndex:
                        return asco1.NVarcharValue2.CompareTo(asco2.NVarcharValue2);
                    case AbsenceMassiveInput.previewEmplCCIndex:
                        return cc1.Code.CompareTo(cc2.Code);
                    case AbsenceMassiveInput.previewEmplCCDescIndex:
                        return cc1.Description.CompareTo(cc2.Description);
                    case AbsenceMassiveInput.previewEmplBranchIndex:
                        return asco1.NVarcharValue7.CompareTo(asco2.NVarcharValue7);
                    default:
                        return empl1.FirstAndLastName.CompareTo(empl2.FirstAndLastName);
                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("AbsenceMassiveInput", culture);

                //label's text
                this.lblAction.Text = rm.GetString("lblAction", culture);
                this.lblCycleDay.Text = rm.GetString("lblCycleDay", culture);
                this.lblDay.Text = rm.GetString("lblDay", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblPassType.Text = rm.GetString("lblWageType", culture);
                this.lblPassTypeNew.Text = rm.GetString("lblWageTypeNew", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblWS.Text = rm.GetString("lblWS", culture);
                this.lblEmplInfo.Text = rm.GetString("lblEmplInfo", culture);
                this.lblPreviewInfo.Text = rm.GetString("lblPreviewInfo", culture);
                this.lblLogPath.Text = rm.GetString("lblLogPath", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnBrowse.Text = rm.GetString("btnBrowse", culture);
                this.btnExportPreview.Text = rm.GetString("btnExportPreview", culture);
                this.btnGeneratePreview.Text = rm.GetString("btnGeneratePreview", culture);
                this.btnLogBrowse.Text = rm.GetString("btnBrowse", culture);

                //group box text
                this.gbAction.Text = rm.GetString("gbAction", culture);
                this.gbEmplFilter.Text = rm.GetString("gbEmplFilter", culture);
                this.gbEmployees.Text = rm.GetString("gbEmployee", culture);
                this.gbPeriod.Text = rm.GetString("gbPeriod", culture);
                this.gbPreview.Text = rm.GetString("gbPreview", culture);
                this.gbSettings.Text = rm.GetString("gbSettings", culture);
                //this.gbWS.Text = rm.GetString("gbWS", culture);

                // radio button text                
                this.rbFile.Text = rm.GetString("rbFile", culture);
                this.rbWSDay.Text = rm.GetString("rbWSDay", culture);
                this.rbWSGroup.Text = rm.GetString("rbWSGroup", culture);
                
                // check box text
                this.chbHierarhiclyWU.Text = rm.GetString("chbHierarhicly", culture);
                this.chbHierachyOU.Text = rm.GetString("chbHierarhicly", culture);
                this.chbHours.Text = rm.GetString("chbHours", culture);
                this.chbIncludeAdditional.Text = rm.GetString("chbIncludeAdditional", culture);

                // list view
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 50, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrStringone", culture), 100, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 170, HorizontalAlignment.Left);                
                lvEmployees.Columns.Add(rm.GetString("hdrCostCenter", culture), 100, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrCostCenterDesc", culture), 100, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrBranch", culture), 50, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvPreview.BeginUpdate();
                lvPreview.Columns.Add(rm.GetString("hdrID", culture), 50, HorizontalAlignment.Left);                
                lvPreview.Columns.Add(rm.GetString("hdrName", culture), 170, HorizontalAlignment.Left);                
                lvPreview.Columns.Add(rm.GetString("hdrDate", culture), 80, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrStart", culture), 50, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrEnd", culture), 50, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrPTBefore", culture), 150, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrPTAfter", culture), 150, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrStringone", culture), 100, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrCostCenter", culture), 100, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrCostCenterDesc", culture), 100, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrBranch", culture), 50, HorizontalAlignment.Left);
                lvPreview.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void AbsenceMassiveInput_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                // Initialize comparer objects
                _comp = new ListViewEmployeeItemComparer(lvEmployees);
                _comp.SortColumn = emplNameIndex;
                lvEmployees.ListViewItemSorter = _comp;
                lvEmployees.Sorting = SortOrder.Ascending;

                sortOrder = Constants.sortAsc;
                sortField = AbsenceMassiveInput.previewEmplNameIndex;
                startIndex = 0;
                                
                rules = new Common.Rule().SearchWUEmplTypeDictionary();
                wuDict = new WorkingUnit().getWUDictionary();
                schemaDict = new TimeSchema().getDictionary();
                ptDict = new PassType().SearchDictionary();
                ptLimitsDict = new PassTypeLimit().SearchDictionary();
                
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

                TypeCodes = Constants.MassiveInputTypeCodes();
                TypeAdditionalCodes = Constants.MassiveInputTypeAdditionalCodes();
                TypesAdditionalDict = Constants.ChangeRegularTypesAdditionalDict();

                populateWU();
                populateOU();
                populateSchGroups();
                populateSchemas();
                populateAction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.AbsenceMassiveInput_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setFilterVisibility()
        {
            try
            {
                cbWU.Enabled = btnWUTree.Enabled = chbHierarhiclyWU.Enabled = rbWU.Checked;
                cbOU.Enabled = btnOUTree.Enabled = chbHierachyOU.Enabled = rbOU.Checked;
                cbWSGroup.Enabled = rbWSGroup.Checked;
                gbWS.Enabled = rbWSDay.Checked;
                btnBrowse.Enabled = rbFile.Checked;
                                
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.setFilterVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                setFilterVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbWSGroup_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                setFilterVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.rbWSGroup_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbWSDay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                setFilterVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.rbWSDay_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                setFilterVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.rbOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbFile_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                setFilterVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.rbFile_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateSchGroups()
        {
            try
            {
                List<WorkingGroupTO> groupArray = new WorkingGroup().Search();

                groupArray.Insert(0, new WorkingGroupTO(-1, rm.GetString("all", culture), ""));
                
                cbWSGroup.DataSource = groupArray;
                cbWSGroup.DisplayMember = "GroupName";
                cbWSGroup.ValueMember = "EmployeeGroupID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.populateSchGroups(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateSchemas()
        {
            try
            {
                List<WorkTimeSchemaTO> schArray = new List<WorkTimeSchemaTO>();

                WorkTimeSchemaTO schAll = new WorkTimeSchemaTO();
                schAll.Name = rm.GetString("all", culture);
                schArray.Add(schAll);

                foreach (int id in schemaDict.Keys)
                {
                    if (schemaDict[id].Status.Trim().ToUpper() != Constants.statusRetired.Trim().ToUpper())
                        schArray.Add(schemaDict[id]);
                }                

                cbWS.DataSource = schArray;
                cbWS.DisplayMember = "Name";
                cbWS.ValueMember = "TimeSchemaID";

                cbWS_SelectedIndexChanged(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.populateSchemas(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateAction()
        {
            try
            {
                List<string> actionArray = new List<string>();

                actionArray.Add(rm.GetString(Constants.MassiveInputActions.CHANGE_REGULAR.ToString(), culture));
                actionArray.Add(rm.GetString(Constants.MassiveInputActions.CHANGE_OVERTIME.ToString(), culture));
                actionArray.Add(rm.GetString(Constants.MassiveInputActions.CHANGE_UNCONFIRMED.ToString(), culture));
                actionArray.Add(rm.GetString(Constants.MassiveInputActions.ENTERING_REGULAR.ToString(), culture));
                actionArray.Add(rm.GetString(Constants.MassiveInputActions.ENTERING_OVERTIME.ToString(), culture));

                cbAction.DataSource = actionArray;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.populateAction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                employeeList = new List<EmployeeTO>();
                emplIDs = "";

                if (rbWU.Checked || rbOU.Checked)
                {
                    bool isWU = rbWU.Checked;

                    int ID = -1;
                    if (isWU && cbWU.SelectedIndex > 0)
                        ID = (int)cbWU.SelectedValue;
                    else if (!isWU && cbOU.SelectedIndex > 0)
                        ID = (int)cbOU.SelectedValue;

                    //int onlyEmplTypeID = -1;
                    //int exceptEmplTypeID = -1;

                    DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime from = currMonth.AddMonths(-1);
                    DateTime to = currMonth.AddMonths(2).AddDays(-1);

                    if (ID != -1)
                    {
                        if (isWU)
                        {
                            string wunits = "";
                            if (chbHierarhiclyWU.Checked)
                                wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);
                            else
                                wunits = ID.ToString().Trim();

                            // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                            employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                        }
                        else
                        {
                            string ounits = "";
                            if (chbHierachyOU.Checked)
                                ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);
                            else
                                ounits = ID.ToString().Trim();

                            employeeList = new Employee().SearchByOU(ounits, -1, null, from.Date, to.Date);
                        }
                    }
                }

                if (rbWSGroup.Checked)
                {
                    if (cbWSGroup.SelectedIndex > 0)
                    {
                        Employee empl = new Employee();
                        empl.EmplTO.WorkingGroupID = (int)cbWSGroup.SelectedValue;
                        employeeList = empl.Search();
                    }
                }

                if (rbWSDay.Checked)
                {
                    if (cbWS.SelectedIndex > 0 && cbCycleDay.Items.Count > 0 && cbCycleDay.SelectedIndex >= 0)
                    {
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate("", dtpDay.Value.Date, dtpDay.Value.Date, null);

                        foreach (int emplID in emplSchedules.Keys)
                        {
                            List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(dtpDay.Value.Date, emplSchedules[emplID], schemaDict);

                            if (intervals.Count <= 0)
                                continue;

                            if (intervals[0].TimeSchemaID == (int)cbWS.SelectedValue && intervals[0].DayNum == cbCycleDay.SelectedIndex)
                                emplIDs += emplID.ToString().Trim() + ",";
                        }

                        if (emplIDs.Length > 0)
                        {
                            emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                            employeeList = new Employee().Search(emplIDs);
                        }
                    }
                }

                if (rbFile.Checked)
                {
                    browseEmployees();
                }

                if (emplIDs.Length == 0)
                {
                    foreach (EmployeeTO empl in employeeList)
                    {
                        emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }

                if (emplIDs.Length > 0)
                    ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);
                else
                    ascoDict = new Dictionary<int, EmployeeAsco4TO>();
                
                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                foreach (EmployeeTO empl in employeeList)
                {
                    if (!wuList.Contains(empl.WorkingUnitID) && !ouList.Contains(empl.OrgUnitID))
                        continue;

                    ListViewItem item = new ListViewItem();
                    item.Text = empl.EmployeeID.ToString().Trim();
                    if (ascoDict.ContainsKey(empl.EmployeeID))
                        item.SubItems.Add(ascoDict[empl.EmployeeID].NVarcharValue2.Trim());
                    else
                        item.SubItems.Add("");
                    item.SubItems.Add(empl.FirstAndLastName);                    
                    WorkingUnitTO ccWU = Common.Misc.getEmplCostCenter(empl, wuDict, null);
                    item.SubItems.Add(ccWU.Code.Trim());
                    item.SubItems.Add(ccWU.Description.Trim());
                    if (ascoDict.ContainsKey(empl.EmployeeID))
                        item.SubItems.Add(ascoDict[empl.EmployeeID].NVarcharValue6.Trim());
                    else
                        item.SubItems.Add("");
                    item.ToolTipText = empl.FirstAndLastName;

                    item.Tag = empl;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhiclyWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.chbHierarhiclyWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbHierachyOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.chbHierachyOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                    this.cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbEmployee_TextChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string text = tbEmployee.Text.Trim();

                int id = -1;

                lvEmployees.SelectedItems.Clear();

                if (int.TryParse(text, out id))
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (((EmployeeTO)item.Tag).EmployeeID.ToString().Trim().StartsWith(id.ToString().Trim()))
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                            break;
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (((EmployeeTO)item.Tag).FirstAndLastName.ToString().Trim().ToUpper().StartsWith(text.ToString().Trim().ToUpper()))
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                            break;
                        }
                    }
                }

                tbEmployee.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.tbEmployee_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<string> cycleDays = new List<string>();
                
                if (cbWS.SelectedIndex > 0)
                {
                    int id = (int)cbWS.SelectedValue;

                    if (schemaDict.ContainsKey(id))
                    {
                        foreach (int day in schemaDict[id].Days.Keys)
                        {
                            string cycleDay = (day + 1).ToString().Trim();

                            foreach (int num in schemaDict[id].Days[day].Keys)
                            {
                                cycleDay += " " + schemaDict[id].Days[day][num].StartTime.ToString(Constants.timeFormat) + ":"
                                    + schemaDict[id].Days[day][num].StartTime.ToString(Constants.timeFormat) + "(" + schemaDict[id].Days[day][num].Description.Trim() + ")";
                            }

                            cycleDays.Add(cycleDay);
                        }
                    }
                }

                cbCycleDay.DataSource = cycleDays;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.cbWS_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbAction.SelectedIndex >= 0)
                {
                    setDatePickerCustomFormat();

                    chbIncludeAdditional.Checked = chbClosure.Checked = chbLayOff.Checked = chbStoppage.Checked = chbPublicHoliday.Checked = false;

                    chbIncludeAdditional.Visible = (cbAction.SelectedIndex == (int)Constants.MassiveInputActions.CHANGE_REGULAR);

                    chbClosure.Visible = chbLayOff.Visible = chbStoppage.Visible = chbPublicHoliday.Visible = false;

                    populateTypes(cbAction.SelectedIndex);

                    rbCheckLimit.Checked = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.cbAction_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbPassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbPassType.SelectedIndex >= 0)
                {
                    setHoursVisibility();

                    populateNewTypes(cbPassType.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.cbPassType_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbPassTypeNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbPassTypeNew.SelectedIndex >= 0)
                    setHoursVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.cbPassTypeNew_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setHoursVisibility()
        {
            try
            {
                if (cbAction.SelectedIndex == (int)Constants.MassiveInputActions.CHANGE_REGULAR || cbAction.SelectedIndex == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                {
                    List<int> wholeDayAbsenceIndexes = new List<int>();
                    wholeDayAbsenceIndexes.Add((int)Constants.ChangeRegularTypes.ANNUAL_LEAVE);
                    wholeDayAbsenceIndexes.Add((int)Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE);
                    //wholeDayAbsenceIndexes.Add((int)Constants.ChangeRegularTypes.PAID_LEAVE);
                    wholeDayAbsenceIndexes.Add((int)Constants.ChangeRegularTypes.PAID_LEAVE_65);

                    if (wholeDayAbsenceIndexes.Contains(cbPassType.SelectedIndex) || TypesListDict.ContainsKey(cbPassType.SelectedIndex)
                        || TypesListClosureDict.ContainsKey(cbPassType.SelectedIndex) || TypesListLayoffDict.ContainsKey(cbPassType.SelectedIndex)
                        || TypesListStoppageDict.ContainsKey(cbPassType.SelectedIndex) || TypesListHolidayDict.ContainsKey(cbPassType.SelectedIndex)
                        || (newTypesIndexValues.ContainsKey(cbPassTypeNew.SelectedIndex) && wholeDayAbsenceIndexes.Contains(newTypesIndexValues[cbPassTypeNew.SelectedIndex]))
                        || TypesNewListDict.ContainsKey(cbPassTypeNew.SelectedIndex)
                        || TypesNewListClosureDict.ContainsKey(cbPassTypeNew.SelectedIndex) || TypesNewListLayoffDict.ContainsKey(cbPassTypeNew.SelectedIndex)
                        || TypesNewListStoppageDict.ContainsKey(cbPassTypeNew.SelectedIndex) || TypesNewListHolidayDict.ContainsKey(cbPassTypeNew.SelectedIndex))
                    {
                        chbHours.Checked = false;
                        chbHours.Enabled = false;
                    }
                    else
                        chbHours.Enabled = true;

                    chbHours_CheckedChanged(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.setHoursVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setDatePickerCustomFormat()
        {
            try
            {
                string format = Constants.dateFormat;
                if (cbAction.SelectedIndex == (int)Constants.MassiveInputActions.ENTERING_OVERTIME)
                {
                    format += " " + Constants.timeFormat;
                }

                dtpFrom.CustomFormat = format;
                dtpTo.CustomFormat = format;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.setHoursVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getTypeCode(string type)
        {
            try
            {
                if (TypeCodes.ContainsKey(type))
                    return TypeCodes[type] + rm.GetString(type, culture);
                else
                    return rm.GetString(type, culture);                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.getTypeCode(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getTypeAdditionalCode(string typeDesc, int addType)
        {
            try
            {
                string typeCode = "";

                string type = "";

                if (typeDesc.StartsWith(Constants.notConfirmedPrefix))
                    type = typeDesc.Substring(Constants.notConfirmedPrefix.Length);
                else
                    type = typeDesc;

                if (TypeAdditionalCodes.ContainsKey(typeDesc))
                    typeCode = TypeAdditionalCodes[typeDesc];

                if (addType == (int)Constants.AdditionalTypes.CLOSURE)
                    typeCode += " " + Constants.FiatClosure.Trim() + " - " + rm.GetString(type, culture);
                else if (addType == (int)Constants.AdditionalTypes.LAYOFF)
                    typeCode += " " + Constants.FiatLayOff.Trim() + " - " + rm.GetString(type, culture);
                else if (addType == (int)Constants.AdditionalTypes.STOPPAGE)
                    typeCode += " " + Constants.FiatStoppage.Trim() + " - " + rm.GetString(type, culture);
                else if (addType == (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY)
                    typeCode += " " + Constants.FiatPublicHolliday.Trim() + " - " + rm.GetString(type, culture);
                else
                    typeCode += " " + rm.GetString(type, culture);

                return typeCode;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.getTypeAdditionalCode(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateTypes(int action)
        {
            try
            {
                TypesListDict = new Dictionary<int, int>();
                TypesListClosureDict = new Dictionary<int, int>();
                TypesListLayoffDict = new Dictionary<int, int>();
                TypesListStoppageDict = new Dictionary<int, int>();
                TypesListHolidayDict = new Dictionary<int, int>();

                List<string> typeArray = new List<string>();
                newTypesIndexValues = new Dictionary<int, int>(); // empty here becouse of setting hours visibility

                TypesNewListDict = new Dictionary<int, int>();
                TypesNewListClosureDict = new Dictionary<int, int>();
                TypesNewListLayoffDict = new Dictionary<int, int>();
                TypesNewListStoppageDict = new Dictionary<int, int>();
                TypesNewListHolidayDict = new Dictionary<int, int>();

                chbHours.Enabled = true;
                chbHours.Checked = false;
                chbHours_CheckedChanged(this, new EventArgs());

                if (action == (int)Constants.MassiveInputActions.CHANGE_REGULAR)
                {
                    lblPassType.Visible = cbPassType.Visible = lblPassTypeNew.Visible = cbPassTypeNew.Visible = chbHours.Visible = numHours.Visible = true;

                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.ABSENCE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.ANNUAL_LEAVE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.BANK_HOURS_USED.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.STOP_WORKING.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.TRAINING.ToString()));                    
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.REGULAR_WORK.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.OVERTIME_NOTJUSTIFIED_USED.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.STRIKE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.JUSTIFIED_ABSENCE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.PAID_LEAVE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.PAID_LEAVE_65.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.MEDICAL_CHECK.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.DELAY.ToString()));

                    this.rbDoNotCheckLimit.Text = rm.GetString("rbCounterAllowNegative", culture);
                    this.rbCheckLimit.Text = rm.GetString("rbCounterNotAllowNegative", culture);
                }
                else if (action == (int)Constants.MassiveInputActions.CHANGE_OVERTIME)
                {
                    lblPassType.Visible = cbPassType.Visible = lblPassTypeNew.Visible = cbPassTypeNew.Visible = chbHours.Visible = numHours.Visible = true;                    

                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_PAID.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.BANK_HOURS.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.STOP_WORKING_DONE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_REJECTED.ToString()));                    
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY_NIGHT.ToString()));

                    this.rbDoNotCheckLimit.Text = rm.GetString("checkLimitNot", culture);
                    this.rbCheckLimit.Text = rm.GetString("checkLimit", culture);
                }
                else if (action == (int)Constants.MassiveInputActions.CHANGE_UNCONFIRMED)
                {
                    lblPassType.Visible = cbPassType.Visible = lblPassTypeNew.Visible = cbPassTypeNew.Visible = chbHours.Visible = numHours.Visible = false;
                }
                else if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                {
                    lblPassType.Visible = cbPassType.Visible = chbHours.Visible = numHours.Visible = true;
                    lblPassTypeNew.Visible = cbPassTypeNew.Visible = false;

                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.ABSENCE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.ANNUAL_LEAVE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.BANK_HOURS_USED.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.STOP_WORKING.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.TRAINING.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.REGULAR_WORK.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.OVERTIME_NOTJUSTIFIED_USED.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.STRIKE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.JUSTIFIED_ABSENCE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.PAID_LEAVE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.PAID_LEAVE_65.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.MEDICAL_CHECK.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeRegularTypes.DELAY.ToString()));

                    this.rbDoNotCheckLimit.Text = rm.GetString("rbCounterAllowNegative", culture);
                    this.rbCheckLimit.Text = rm.GetString("rbCounterNotAllowNegative", culture);
                }
                else if (action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME)
                {
                    lblPassType.Visible = cbPassType.Visible = true;
                    lblPassTypeNew.Visible = cbPassTypeNew.Visible = chbHours.Visible = numHours.Visible = false;

                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_PAID.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.BANK_HOURS.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.STOP_WORKING_DONE.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_REJECTED.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY.ToString()));
                    typeArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY_NIGHT.ToString()));

                    this.rbDoNotCheckLimit.Text = rm.GetString("checkLimitNot", culture);
                    this.rbCheckLimit.Text = rm.GetString("checkLimit", culture);
                }

                // add additional types
                if (chbIncludeAdditional.Checked)
                {
                    foreach (int key in TypesAdditionalDict.Keys)
                    {
                        TypesListDict.Add(typeArray.Count, key);
                        typeArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.NONE));

                        if (chbClosure.Checked)
                        {
                            TypesListClosureDict.Add(typeArray.Count, key);
                            typeArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.CLOSURE));
                        }

                        if (chbLayOff.Checked)
                        {
                            TypesListLayoffDict.Add(typeArray.Count, key);
                            typeArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.LAYOFF));
                        }

                        if (chbStoppage.Checked)
                        {
                            TypesListStoppageDict.Add(typeArray.Count, key);
                            typeArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.STOPPAGE));
                        }

                        if (chbPublicHoliday.Checked)
                        {
                            TypesListHolidayDict.Add(typeArray.Count, key);
                            typeArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY));
                        }
                    }
                }

                cbPassType.DataSource = typeArray;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.populateTypes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateNewTypes(int pt)
        {
            try
            {
                TypesNewListDict = new Dictionary<int, int>();
                TypesNewListClosureDict = new Dictionary<int, int>();
                TypesNewListLayoffDict = new Dictionary<int, int>();
                TypesNewListStoppageDict = new Dictionary<int, int>();
                TypesNewListHolidayDict = new Dictionary<int, int>();

                List<string> typeNewArray = new List<string>();
                newTypesIndexValues = new Dictionary<int, int>();

                int index = 0;

                if (cbAction.SelectedIndex == (int)Constants.MassiveInputActions.CHANGE_OVERTIME)
                {
                    if (pt != (int)Constants.ChangeOvertimeTypes.OVER_TIME_PAID)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_PAID.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeOvertimeTypes.OVER_TIME_PAID);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeOvertimeTypes.BANK_HOURS)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.BANK_HOURS.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeOvertimeTypes.BANK_HOURS);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeOvertimeTypes.STOP_WORKING_DONE)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.STOP_WORKING_DONE.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeOvertimeTypes.STOP_WORKING_DONE);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeOvertimeTypes.OVER_TIME_REJECTED)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_REJECTED.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeOvertimeTypes.OVER_TIME_REJECTED);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY_NIGHT)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY_NIGHT.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY_NIGHT);
                        index++;
                    }
                }
                else if (cbAction.SelectedIndex == (int)Constants.MassiveInputActions.CHANGE_REGULAR)
                {
                    if (pt != (int)Constants.ChangeRegularTypes.ABSENCE)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.ABSENCE.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.ABSENCE);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.ANNUAL_LEAVE)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.ANNUAL_LEAVE.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.ANNUAL_LEAVE);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.BANK_HOURS_USED)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.BANK_HOURS_USED.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.BANK_HOURS_USED);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.STOP_WORKING)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.STOP_WORKING.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.STOP_WORKING);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.TRAINING)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.TRAINING.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.TRAINING);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.REGULAR_WORK)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.REGULAR_WORK.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.REGULAR_WORK);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.OVERTIME_NOTJUSTIFIED_USED)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.OVERTIME_NOTJUSTIFIED_USED.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.OVERTIME_NOTJUSTIFIED_USED);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.STRIKE)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.STRIKE.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.STRIKE);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.JUSTIFIED_ABSENCE)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.JUSTIFIED_ABSENCE.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.JUSTIFIED_ABSENCE);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.PAID_LEAVE)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.PAID_LEAVE.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.PAID_LEAVE);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.PAID_LEAVE_65)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.PAID_LEAVE_65.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.PAID_LEAVE_65);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.MEDICAL_CHECK)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.MEDICAL_CHECK.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.MEDICAL_CHECK);
                        index++;
                    }
                    if (pt != (int)Constants.ChangeRegularTypes.DELAY)
                    {
                        typeNewArray.Add(getTypeCode(Constants.ChangeRegularTypes.DELAY.ToString()));
                        newTypesIndexValues.Add(index, (int)Constants.ChangeRegularTypes.DELAY);
                        index++;
                    }

                    // add additional types
                    if (chbIncludeAdditional.Checked)
                    {
                        foreach (int key in TypesAdditionalDict.Keys)
                        {
                            if (!TypesListDict.ContainsKey(pt) || key != TypesListDict[pt])
                            {
                                TypesNewListDict.Add(typeNewArray.Count, key);
                                typeNewArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.NONE));
                            }

                            if (chbClosure.Checked)
                            {
                                if (!TypesListClosureDict.ContainsKey(pt) || key != TypesListClosureDict[pt])
                                {
                                    TypesNewListClosureDict.Add(typeNewArray.Count, key);
                                    typeNewArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.CLOSURE));
                                }
                            }

                            if (chbLayOff.Checked)
                            {
                                if (!TypesListLayoffDict.ContainsKey(pt) || key != TypesListLayoffDict[pt])
                                {
                                    TypesNewListLayoffDict.Add(typeNewArray.Count, key);
                                    typeNewArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.LAYOFF));
                                }
                            }

                            if (chbStoppage.Checked)
                            {
                                if (!TypesListStoppageDict.ContainsKey(pt) || key != TypesListStoppageDict[pt])
                                {
                                    TypesNewListStoppageDict.Add(typeNewArray.Count, key);
                                    typeNewArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.STOPPAGE));
                                }
                            }

                            if (chbPublicHoliday.Checked)
                            {
                                if (!TypesListHolidayDict.ContainsKey(pt) || key != TypesListHolidayDict[pt])
                                {
                                    TypesNewListHolidayDict.Add(typeNewArray.Count, key);
                                    typeNewArray.Add(getTypeAdditionalCode(TypesAdditionalDict[key], (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY));
                                }
                            }
                        }
                    }

                }

                cbPassTypeNew.DataSource = typeNewArray;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.populateSchemas(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbWSGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.cbWSGroup_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvEmployees_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvEmployees.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvEmployees.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvEmployees.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvEmployees.Sorting = SortOrder.Ascending;
                }

                lvEmployees.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fbDialog = new OpenFileDialog();
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //fbDialog.Filter = "XLS (*.xls)|*.xls|" +
                //"XLSX (*.xlsx)|*.xlsx";
                fbDialog.Filter = "Text files (*.txt)|*.txt";
                fbDialog.Title = "Open file";

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    tbPath.Text = fbDialog.FileName;

                    populateEmployees();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void browseEmployeesExcel()
        {
            try
            {
                if (!tbPath.Text.Trim().Equals(""))
                {
                    // make connection to selected XLS file
                    string path = tbPath.Text;

                    string excelConnectionString = "";
                    if (System.IO.Path.GetExtension(path).Trim().ToUpper().Equals(xlsExt.Trim().ToUpper()))
                        excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=YES;'";
                    else if (System.IO.Path.GetExtension(path).Trim().ToUpper().Equals(xlsxExt.Trim().ToUpper()))
                        excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 xml; HDR=YES;'";

                    OleDbConnection conn = new OleDbConnection(excelConnectionString);
                    conn.Open();

                    // Get all sheetnames from an excel file into data table
                    DataTable dbSchema = new System.Data.DataTable();
                    dbSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    List<string> sheetNames = new List<string>();
                    if (dbSchema != null && dbSchema.Rows.Count > 0)
                    {
                        // Loop through all worksheets
                        for (int i = 0; i < dbSchema.Rows.Count; i++)
                        {
                            sheetNames.Add(dbSchema.Rows[i]["TABLE_NAME"].ToString());
                        }
                    }

                    if (sheetNames.Count > 0)
                    {
                        // get data from first sheet
                        OleDbCommand sqlcom = new OleDbCommand("SELECT * FROM [" + sheetNames[0].Trim() + "]", conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlcom);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Employees");
                        DataTable table = ds.Tables["Employees"];
                        if (table.Rows.Count > 0)
                        {
                            // make list of items from XLS rows                            
                            foreach (DataRow row in table.Rows)
                            {
                                if (row.ItemArray.Length > 0)
                                {
                                    int emplID = -1;
                                    if (!int.TryParse(row.ItemArray[0].ToString().Trim(), out emplID))
                                        emplID = -1;

                                    if (emplID != -1)
                                        emplIDs += emplID.ToString().Trim() + ",";
                                }
                            }

                            if (emplIDs.Length > 0)
                            {
                                emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                                employeeList = new Employee().Search(emplIDs);
                            }
                        }                        
                    }

                    conn.Close();
                }                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.browseEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void browseEmployees()
        {
            try
            {
                if (!tbPath.Text.Trim().Equals(""))
                {
                    string path = tbPath.Text;
                    string emplIDs = "";
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.FileStream str = System.IO.File.Open(path, System.IO.FileMode.Open);
                        System.IO.StreamReader reader = new System.IO.StreamReader(str);

                        // read file lines                        
                        string line = "";

                        while (line != null)
                        {
                            if (!line.Trim().Equals(""))
                            {
                                int id = -1;

                                if (!int.TryParse(line.Replace("\r", "").Replace("\n", "").Trim(), out id))
                                    id = -1;                                

                                if (id != -1)
                                    emplIDs += id.ToString().Trim() + ",";
                            }

                            line = reader.ReadLine();
                        }

                        reader.Close();
                        str.Dispose();
                        str.Close();
                    }

                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                        employeeList = new Employee().Search(emplIDs);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.browseEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnGeneratePreview_Click(object sender, EventArgs e)
        {
            try
            {                
                if (lvEmployees.Items.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noEmployeesSelected", culture));
                    return;
                }

                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("invalidPeriod", culture));
                    return;
                }

                // get selected time (if entering overtime is selected) without seconds
                DateTime fromTime = dtpFrom.Value.AddSeconds(-dtpFrom.Value.Second);
                DateTime toTime = dtpTo.Value.AddSeconds(-dtpTo.Value.Second);

                // if entering overtime is selected, start time and end time must be in one day
                if (cbAction.SelectedIndex == (int)Constants.MassiveInputActions.ENTERING_OVERTIME)
                {
                    if (!fromTime.Date.Equals(toTime.Date))
                    {
                        MessageBox.Show(rm.GetString("overtimeSameDay", culture));
                        return;
                    }

                    if (fromTime > toTime)
                    {
                        MessageBox.Show(rm.GetString("invalidPeriod", culture));
                        return;
                    }
                }

                string periodError = validatePeriod();

                if (!periodError.Trim().Equals(""))
                {
                    MessageBox.Show(periodError.Trim());
                    return;
                }
                                
                changeType(cbAction.SelectedIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnGeneratePreview_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }            
        }

        private void changeType(int action)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // get selected date interval - values are already validated
                DateTime fromDate = dtpFrom.Value.Date;
                DateTime toDate = dtpTo.Value.Date;

                // get selected time (if entering overtime is selected) without seconds - values are already validated
                DateTime fromTime = dtpFrom.Value.AddSeconds(-dtpFrom.Value.Second);
                DateTime toTime = dtpTo.Value.AddSeconds(-dtpTo.Value.Second);

                // get employees
                string IDs = "";
                emplDict = new Dictionary<int, EmployeeTO>();
                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        IDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";
                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        IDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";
                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                    }
                }

                if (IDs.Length > 0)
                    IDs = IDs.Substring(0, IDs.Length - 1);

                bool checkHours = false;
                // check if selected number of hours is correctly rounded if action is change overtime or change regular hours, and number of hours is selected
                if (chbHours.Checked)
                {
                    if (numHours.Value <= 0)
                    {
                        MessageBox.Show(rm.GetString("notValidNumHours", culture));
                        numHours.Focus();
                        return;
                    }

                    foreach (int emplID in emplDict.Keys)
                    {
                        int company = Common.Misc.getRootWorkingUnit(emplDict[emplID].WorkingUnitID, wuDict);

                        bool roundingOvertime = true;
                        bool roundingOvertimeOutWS = true;
                        bool roundingRegular = true;
                        if (rules.ContainsKey(company) && rules[company].ContainsKey(emplDict[emplID].EmployeeTypeID))
                        {
                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleOvertimeRounding)
                                && (numHours.Value * 60 < rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleOvertimeRounding].RuleValue
                                || (numHours.Value * 60) % rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleOvertimeRounding].RuleValue != 0))
                                roundingOvertime = false;

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleOvertimeRoundingOutWS)
                                && (numHours.Value * 60 < rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleOvertimeRoundingOutWS].RuleValue
                                || (numHours.Value * 60) % rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleOvertimeRoundingOutWS].RuleValue != 0))
                                roundingOvertimeOutWS = false;

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RulePresenceRounding)
                                && (numHours.Value * 60 < rules[company][emplDict[emplID].EmployeeTypeID][Constants.RulePresenceRounding].RuleValue
                                || (numHours.Value * 60) % rules[company][emplDict[emplID].EmployeeTypeID][Constants.RulePresenceRounding].RuleValue != 0))
                                roundingRegular = false;
                        }

                        if (action == (int)Constants.MassiveInputActions.CHANGE_OVERTIME)
                        {
                            if (!roundingOvertime && !roundingOvertimeOutWS)
                            {
                                MessageBox.Show(rm.GetString("numHoursNotRounded", culture));
                                numHours.Focus();
                                return;
                            }

                            if (!roundingOvertime)
                            {
                                DialogResult result = MessageBox.Show(rm.GetString("numHoursNotRoundedWS", culture), "", MessageBoxButtons.YesNo);

                                if (result == DialogResult.No)
                                {
                                    numHours.Focus();
                                    return;
                                }
                            }

                            if (!roundingOvertimeOutWS)
                            {
                                DialogResult result = MessageBox.Show(rm.GetString("numHoursNotRoundedOutWS", culture), "", MessageBoxButtons.YesNo);

                                if (result == DialogResult.No)
                                {
                                    numHours.Focus();
                                    return;
                                }
                            }
                        }

                        if (action == (int)Constants.MassiveInputActions.CHANGE_REGULAR || action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                        {
                            if (!roundingRegular)
                            {
                                MessageBox.Show(rm.GetString("numHoursNotRounded", culture));
                                numHours.Focus();
                                return;
                            }
                        }
                    }

                    checkHours = true;
                }

                // get time schedules and time schemas for selected employees for selected days, add one day for third shifts                
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(IDs, fromDate.Date, toDate.AddDays(1).Date, null);

                List<DateTime> datesList = new List<DateTime>();
                DateTime currDate = fromDate.Date;
                while (currDate.Date <= toDate.AddDays(1).Date)
                {
                    datesList.Add(currDate.Date);
                    currDate = currDate.AddDays(1).Date;
                }

                // get old pairs and copy for entering regular hours, for employees for selected days, and for next day becouse of third shifts
                List<IOPairProcessedTO> employeesOldPairs = new IOPairProcessed().SearchAllPairsForEmpl(IDs, datesList, "");
                                
                // create dictionary of old pairs list for each employee by date
                emplOldPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();                
                foreach (IOPairProcessedTO oldP in employeesOldPairs)
                {
                    IOPairProcessedTO oldPair = new IOPairProcessedTO(oldP);
                    oldPair.OldPassTypeID = oldPair.PassTypeID;
                    if (!emplOldPairs.ContainsKey(oldPair.EmployeeID))
                        emplOldPairs.Add(oldPair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                    if (!emplOldPairs[oldPair.EmployeeID].ContainsKey(oldPair.IOPairDate.Date))
                        emplOldPairs[oldPair.EmployeeID].Add(oldPair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplOldPairs[oldPair.EmployeeID][oldPair.IOPairDate.Date].Add(oldPair);
                }

                // get old counters
                emplOldCounters = new EmployeeCounterValue().SearchValues(IDs);

                Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();
                Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> emplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();

                emplNewPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                emplNewCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                newPairsAll = new List<IOPairProcessedTO>();
                emplIDsChanged = "";
                emplDateMessages = new Dictionary<int, Dictionary<DateTime, List<string>>>();
                                
                bool checkAnnualLeave = ((action == (int)Constants.MassiveInputActions.CHANGE_REGULAR || action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                    && (cbPassType.SelectedIndex == (int)Constants.ChangeRegularTypes.ANNUAL_LEAVE || cbPassType.SelectedIndex == (int)Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE
                    || (newTypesIndexValues.ContainsKey(cbPassTypeNew.SelectedIndex)
                    && (newTypesIndexValues[cbPassTypeNew.SelectedIndex] == (int)Constants.ChangeRegularTypes.ANNUAL_LEAVE || newTypesIndexValues[cbPassTypeNew.SelectedIndex] == (int)Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE))));

                // get employee holidays                
                Dictionary<int, List<DateTime>> personalHolidays = new Dictionary<int, List<DateTime>>();
                Dictionary<int, List<DateTime>> emplHolidays = employeeHolidays(ref personalHolidays, fromDate.Date, toDate.Date, IDs, emplSchedules, schemaDict, emplDict, rules);
                
                foreach (int emplID in emplDict.Keys)
                {
                    if (changingExisting(action) && !emplOldPairs.ContainsKey(emplID))
                        continue;

                    EmployeeTO empl = new EmployeeTO();
                    EmployeeAsco4TO asco = new EmployeeAsco4TO();
                    decimal numOfMinutesConverted = 0;

                    if (emplDict.ContainsKey(emplID))
                        empl = emplDict[emplID];

                    if (ascoDict.ContainsKey(emplID))
                        asco = ascoDict[emplID];

                    if (emplOldCounters.ContainsKey(emplID))
                    {
                        if (!emplNewCounters.ContainsKey(emplID))
                            emplNewCounters.Add(emplID, new Dictionary<int, EmployeeCounterValueTO>());

                        foreach (int type in emplOldCounters[emplID].Keys)
                        {
                            if (!emplNewCounters[emplID].ContainsKey(type))
                                emplNewCounters[emplID].Add(type, new EmployeeCounterValueTO(emplOldCounters[emplID][type]));
                        }
                    }

                    //Dictionary<int, EmployeeCounterValueTO> emplCounters = new Dictionary<int, EmployeeCounterValueTO>();
                    //if (emplNewCounters.ContainsKey(emplID))
                    //    emplCounters = emplNewCounters[emplID];

                    int company = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wuDict);

                    Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                    // get employee rules
                    if (rules.ContainsKey(company) && rules[company].ContainsKey(empl.EmployeeTypeID))
                        emplRules = rules[company][empl.EmployeeTypeID];

                    // get annual leave types
                    List<int> alList = Common.Misc.getAnnualLeaveTypes(emplRules);

                    // get old pass type
                    int ptOld = -1;
                    if (action == (int)Constants.MassiveInputActions.CHANGE_REGULAR || action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                    {
                        string ruleOldType = "";
                        switch (cbPassType.SelectedIndex)
                        {
                            case (int)Constants.ChangeRegularTypes.ABSENCE:
                                ptOld = Constants.absence;
                                break;
                            case (int)Constants.ChangeRegularTypes.ANNUAL_LEAVE:
                                ruleOldType = Constants.RuleCompanyAnnualLeave;
                                break;
                            case (int)Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE:
                                ruleOldType = Constants.RuleCompanyCollectiveAnnualLeave;
                                break;
                            case (int)Constants.ChangeRegularTypes.BANK_HOURS_USED:
                                ruleOldType = Constants.RuleCompanyBankHourUsed;
                                break;
                            case (int)Constants.ChangeRegularTypes.STOP_WORKING:
                                ruleOldType = Constants.RuleCompanyStopWorking;
                                break;
                            case (int)Constants.ChangeRegularTypes.TRAINING:
                                ruleOldType = Constants.RuleCompanyTrening;
                                break;
                            case (int)Constants.ChangeRegularTypes.REGULAR_WORK:
                                ruleOldType = Constants.RuleCompanyRegularWork;
                                break;
                            case (int)Constants.ChangeRegularTypes.OVERTIME_NOTJUSTIFIED_USED:
                                ruleOldType = Constants.RuleCompanyInitialOvertimeUsed;
                                break;
                            case (int)Constants.ChangeRegularTypes.STRIKE:
                                ruleOldType = Constants.RuleCompanyStrike;
                                break;
                            case (int)Constants.ChangeRegularTypes.JUSTIFIED_ABSENCE:
                                ruleOldType = Constants.RuleCompanyJustifiedAbsence;
                                break;
                            case (int)Constants.ChangeRegularTypes.PAID_LEAVE:
                                ruleOldType = Constants.RuleCompanyPaidLeave;
                                break;
                            case (int)Constants.ChangeRegularTypes.PAID_LEAVE_65:
                                ruleOldType = Constants.RuleCompanyPaidLeave65Percent;
                                break;
                            case (int)Constants.ChangeRegularTypes.MEDICAL_CHECK:
                                ruleOldType = Constants.RuleCompanyPeriodicalMedicalCheckUp;
                                break;
                            case (int)Constants.ChangeRegularTypes.DELAY:
                                ruleOldType = Constants.RuleCompanyDelay;
                                break;
                        }

                        if (ptOld == -1 && emplRules.ContainsKey(ruleOldType))
                            ptOld = emplRules[ruleOldType].RuleValue;

                        if (ptOld == -1)
                        {
                            if (TypesListDict.ContainsKey(cbPassType.SelectedIndex))
                                ptOld = getType(TypesListDict[cbPassType.SelectedIndex], company, (int)Constants.AdditionalTypes.NONE);
                            else if (TypesListClosureDict.ContainsKey(cbPassType.SelectedIndex))
                                ptOld = getType(TypesListClosureDict[cbPassType.SelectedIndex], company, (int)Constants.AdditionalTypes.CLOSURE);
                            else if (TypesListLayoffDict.ContainsKey(cbPassType.SelectedIndex))
                                ptOld = getType(TypesListLayoffDict[cbPassType.SelectedIndex], company, (int)Constants.AdditionalTypes.LAYOFF);
                            else if (TypesListStoppageDict.ContainsKey(cbPassType.SelectedIndex))
                                ptOld = getType(TypesListStoppageDict[cbPassType.SelectedIndex], company, (int)Constants.AdditionalTypes.STOPPAGE);
                            else if (TypesListHolidayDict.ContainsKey(cbPassType.SelectedIndex))
                                ptOld = getType(TypesListHolidayDict[cbPassType.SelectedIndex], company, (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY);
                        }
                    }
                    else if (action == (int)Constants.MassiveInputActions.CHANGE_OVERTIME || action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME)
                    {
                        string ruleOvertimeType = "";
                        switch (cbPassType.SelectedIndex)
                        {
                            case (int)Constants.ChangeOvertimeTypes.OVER_TIME_PAID:
                                ruleOvertimeType = Constants.RuleCompanyOvertimePaid;
                                break;
                            case (int)Constants.ChangeOvertimeTypes.BANK_HOURS:
                                ruleOvertimeType = Constants.RuleCompanyBankHour;
                                break;
                            case (int)Constants.ChangeOvertimeTypes.STOP_WORKING_DONE:
                                ruleOvertimeType = Constants.RuleCompanyStopWorkingDone;
                                break;
                            case (int)Constants.ChangeOvertimeTypes.OVER_TIME_REJECTED:
                                ruleOvertimeType = Constants.RuleCompanyOvertimeRejected;
                                break;
                            case (int)Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY:
                                ptOld = Constants.overtimeUnjustified;
                                break;
                            case (int)Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY_NIGHT:
                                ruleOvertimeType = Constants.RuleCompanyInitialNightOvertime;
                                break;
                        }

                        if (ptOld == -1 && emplRules.ContainsKey(ruleOvertimeType))
                            ptOld = emplRules[ruleOvertimeType].RuleValue;
                    }
                    
                    if (action != (int)Constants.MassiveInputActions.CHANGE_UNCONFIRMED && (ptOld == -1 || !ptDict.ContainsKey(ptOld)))
                        continue;

                    // get new pass type
                    int ptNew = -1;
                    if (action == (int)Constants.MassiveInputActions.CHANGE_REGULAR)
                    {
                        string ruleNewType = "";

                        if (newTypesIndexValues.ContainsKey(cbPassTypeNew.SelectedIndex) || TypesNewListDict.ContainsKey(cbPassTypeNew.SelectedIndex)
                             || TypesNewListClosureDict.ContainsKey(cbPassTypeNew.SelectedIndex) || TypesNewListLayoffDict.ContainsKey(cbPassTypeNew.SelectedIndex)
                             || TypesNewListStoppageDict.ContainsKey(cbPassTypeNew.SelectedIndex) || TypesNewListHolidayDict.ContainsKey(cbPassTypeNew.SelectedIndex))
                        {
                            if (newTypesIndexValues.ContainsKey(cbPassTypeNew.SelectedIndex))
                            {
                                switch (newTypesIndexValues[cbPassTypeNew.SelectedIndex])
                                {
                                    case (int)Constants.ChangeRegularTypes.ABSENCE:
                                        ptNew = Constants.absence;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.ANNUAL_LEAVE:
                                        ruleNewType = Constants.RuleCompanyAnnualLeave;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.COLLECTIVE_ANNUAL_LEAVE:
                                        ruleNewType = Constants.RuleCompanyCollectiveAnnualLeave;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.BANK_HOURS_USED:
                                        ruleNewType = Constants.RuleCompanyBankHourUsed;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.STOP_WORKING:
                                        ruleNewType = Constants.RuleCompanyStopWorking;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.TRAINING:
                                        ruleNewType = Constants.RuleCompanyTrening;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.REGULAR_WORK:
                                        ruleNewType = Constants.RuleCompanyRegularWork;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.OVERTIME_NOTJUSTIFIED_USED:
                                        ruleNewType = Constants.RuleCompanyInitialOvertimeUsed;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.STRIKE:
                                        ruleNewType = Constants.RuleCompanyStrike;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.JUSTIFIED_ABSENCE:
                                        ruleNewType = Constants.RuleCompanyJustifiedAbsence;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.PAID_LEAVE:
                                        ruleNewType = Constants.RuleCompanyPaidLeave;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.PAID_LEAVE_65:
                                        ruleNewType = Constants.RuleCompanyPaidLeave65Percent;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.MEDICAL_CHECK:
                                        ruleNewType = Constants.RuleCompanyPeriodicalMedicalCheckUp;
                                        break;
                                    case (int)Constants.ChangeRegularTypes.DELAY:
                                        ruleNewType = Constants.RuleCompanyDelay;
                                        break;
                                }

                                if (ptNew == -1 && emplRules.ContainsKey(ruleNewType))
                                    ptNew = emplRules[ruleNewType].RuleValue;
                            }

                            if (ptNew == -1)
                            {
                                if (TypesNewListDict.ContainsKey(cbPassTypeNew.SelectedIndex))
                                    ptNew = getType(TypesNewListDict[cbPassTypeNew.SelectedIndex], company, (int)Constants.AdditionalTypes.NONE);
                                else if (TypesNewListClosureDict.ContainsKey(cbPassTypeNew.SelectedIndex))
                                    ptNew = getType(TypesNewListClosureDict[cbPassTypeNew.SelectedIndex], company, (int)Constants.AdditionalTypes.CLOSURE);
                                else if (TypesNewListLayoffDict.ContainsKey(cbPassTypeNew.SelectedIndex))
                                    ptNew = getType(TypesNewListLayoffDict[cbPassTypeNew.SelectedIndex], company, (int)Constants.AdditionalTypes.LAYOFF);
                                else if (TypesNewListStoppageDict.ContainsKey(cbPassTypeNew.SelectedIndex))
                                    ptNew = getType(TypesNewListStoppageDict[cbPassTypeNew.SelectedIndex], company, (int)Constants.AdditionalTypes.STOPPAGE);
                                else if (TypesNewListHolidayDict.ContainsKey(cbPassTypeNew.SelectedIndex))
                                    ptNew = getType(TypesNewListHolidayDict[cbPassTypeNew.SelectedIndex], company, (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY);
                            }
                        }
                    }
                    else if (action == (int)Constants.MassiveInputActions.CHANGE_OVERTIME)
                    {
                        string ruleOvertimeType = "";

                        if (newTypesIndexValues.ContainsKey(cbPassTypeNew.SelectedIndex))
                        {
                            switch (newTypesIndexValues[cbPassTypeNew.SelectedIndex])
                            {
                                case (int)Constants.ChangeOvertimeTypes.OVER_TIME_PAID:
                                    ruleOvertimeType = Constants.RuleCompanyOvertimePaid;
                                    break;
                                case (int)Constants.ChangeOvertimeTypes.BANK_HOURS:
                                    ruleOvertimeType = Constants.RuleCompanyBankHour;
                                    break;
                                case (int)Constants.ChangeOvertimeTypes.STOP_WORKING_DONE:
                                    ruleOvertimeType = Constants.RuleCompanyStopWorkingDone;
                                    break;
                                case (int)Constants.ChangeOvertimeTypes.OVER_TIME_REJECTED:
                                    ruleOvertimeType = Constants.RuleCompanyOvertimeRejected;
                                    break;
                                case (int)Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY:
                                    ptNew = Constants.overtimeUnjustified;
                                    break;
                                case (int)Constants.ChangeOvertimeTypes.OVER_TIME_TO_JUSTIFY_NIGHT:
                                    ruleOvertimeType = Constants.RuleCompanyInitialNightOvertime;
                                    break;
                            }

                            if (ptNew == -1 && emplRules.ContainsKey(ruleOvertimeType))
                                ptNew = emplRules[ruleOvertimeType].RuleValue;
                        }
                    }
                    else if (action == (int)Constants.MassiveInputActions.CHANGE_UNCONFIRMED)
                        ptNew = Constants.absence;
                    else if (action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME)
                    {
                        ptNew = ptOld;
                        if (emplRules.ContainsKey(Constants.RuleCompanyInitialOvertime))
                            ptOld = emplRules[Constants.RuleCompanyInitialOvertime].RuleValue;
                        else
                            ptOld = Constants.overtimeUnjustified;
                    }
                    else if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                    {
                        ptNew = ptOld;
                        ptOld = Constants.absence;
                    }

                    if (ptNew == -1 || !ptDict.ContainsKey(ptNew))
                        continue;

                    bool wholeDayAbsenceType = (ptDict.ContainsKey(ptNew) && ptDict[ptNew].IsPass == Constants.wholeDayAbsence 
                        && !(emplRules.ContainsKey(Constants.RuleCompanyPaidLeave) && ptNew == emplRules[Constants.RuleCompanyPaidLeave].RuleValue));

                    List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();

                    if (emplSchedules.ContainsKey(emplID))
                        schedules = emplSchedules[emplID];

                    if (!emplDayIntervals.ContainsKey(emplID))
                        emplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                    if (!emplDaySchemas.ContainsKey(emplID))
                        emplDaySchemas.Add(emplID, new Dictionary<DateTime, WorkTimeSchemaTO>());

                    // get annual leaves from from and to week
                    List<IOPairProcessedTO> fromWeekAnnualPairs = null;
                    List<IOPairProcessedTO> toWeekAnnualPairs = null;

                    if (checkAnnualLeave)
                    {
                        IOPairProcessed annualPair = new IOPairProcessed();

                        fromWeekAnnualPairs = annualPair.SearchWeekPairs(emplID, fromDate.Date, false, Common.Misc.getAnnualLeaveTypesString(emplRules), null);
                        toWeekAnnualPairs = annualPair.SearchWeekPairs(emplID, toDate.Date, false, Common.Misc.getAnnualLeaveTypesString(emplRules), null);

                        // remove pairs that are in selected period
                        IEnumerator<IOPairProcessedTO> pairEnumerator = fromWeekAnnualPairs.GetEnumerator();
                        while (pairEnumerator.MoveNext())
                        {
                            if (pairEnumerator.Current.IOPairDate.Date >= fromDate.Date)
                            {
                                fromWeekAnnualPairs.Remove(pairEnumerator.Current);
                                pairEnumerator = fromWeekAnnualPairs.GetEnumerator();
                            }
                        }

                        pairEnumerator = toWeekAnnualPairs.GetEnumerator();
                        while (pairEnumerator.MoveNext())
                        {
                            if (pairEnumerator.Current.IOPairDate.Date <= toDate.Date)
                            {
                                toWeekAnnualPairs.Remove(pairEnumerator.Current);
                                pairEnumerator = toWeekAnnualPairs.GetEnumerator();
                            }
                        }
                    }

                    // check if annual leave or paid leave is selected as new type, and employee is not expat out
                    bool notHolidayTypes = ((emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && ptNew == emplRules[Constants.RuleCompanyAnnualLeave].RuleValue)
                        || (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && ptNew == emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)
                        || (ptDict.ContainsKey(ptNew) && (ptDict[ptNew].LimitCompositeID != -1 || ptDict[ptNew].LimitElementaryID != -1 || ptDict[ptNew].LimitOccasionID != -1)))
                        && !Common.Misc.isExpatOut(rules, empl);

                    bool changeAllowed = true;
                    List<IOPairProcessedTO> weekPairs = new List<IOPairProcessedTO>();
                    Dictionary<int, int> paidLeavesElementaryPairsDict = new Dictionary<int, int>();
                    //foreach (DateTime date in emplOldPairs[emplID].Keys)

                    for (DateTime date = fromDate.Date; date.Date <= toDate.Date.AddDays(1); date = date.AddDays(1).Date)
                    {
                        if (changingExisting(action) && !emplOldPairs[emplID].ContainsKey(date))
                            continue;

                        if (action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME && date.Date != fromDate.Date)
                            continue;

                        if (checkAnnualLeave)
                        {
                            if (date.DayOfWeek == DayOfWeek.Monday)
                                weekPairs = new List<IOPairProcessedTO>();

                            if (date.Date == fromDate.Date)
                                weekPairs.AddRange(fromWeekAnnualPairs);
                        }

                        // get previous, current and next day intervals and schemas
                        List<WorkTimeIntervalTO> prevIntervals = new List<WorkTimeIntervalTO>();
                        List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                        List<WorkTimeIntervalTO> nextIntervals = new List<WorkTimeIntervalTO>();
                        WorkTimeSchemaTO prevSch = new WorkTimeSchemaTO();
                        WorkTimeSchemaTO daySch = new WorkTimeSchemaTO();
                        WorkTimeSchemaTO nextSch = new WorkTimeSchemaTO();

                        if (!emplDayIntervals[emplID].ContainsKey(date.Date))
                            emplDayIntervals[emplID].Add(date.Date, Common.Misc.getTimeSchemaInterval(date.Date, schedules, schemaDict));

                        if (!emplDayIntervals[emplID].ContainsKey(date.AddDays(1).Date))
                            emplDayIntervals[emplID].Add(date.AddDays(1).Date, Common.Misc.getTimeSchemaInterval(date.AddDays(1).Date, schedules, schemaDict));

                        if (emplDayIntervals[emplID].ContainsKey(date.AddDays(-1).Date))
                            prevIntervals = emplDayIntervals[emplID][date.AddDays(-1).Date];

                        dayIntervals = emplDayIntervals[emplID][date.Date];
                        nextIntervals = emplDayIntervals[emplID][date.AddDays(1).Date];

                        if (!emplDaySchemas[emplID].ContainsKey(date.Date))
                        {
                            if (dayIntervals.Count > 0 && schemaDict.ContainsKey(dayIntervals[0].TimeSchemaID))
                                daySch = schemaDict[dayIntervals[0].TimeSchemaID];

                            emplDaySchemas[emplID].Add(date.Date, daySch);
                        }

                        if (!emplDaySchemas[emplID].ContainsKey(date.AddDays(1).Date))
                        {
                            if (nextIntervals.Count > 0 && schemaDict.ContainsKey(nextIntervals[0].TimeSchemaID))
                                nextSch = schemaDict[nextIntervals[0].TimeSchemaID];

                            emplDaySchemas[emplID].Add(date.AddDays(1).Date, nextSch);
                        }

                        if (emplDaySchemas[emplID].ContainsKey(date.AddDays(-1).Date))
                            prevSch = emplDaySchemas[emplID][date.AddDays(-1).Date];
                        daySch = emplDaySchemas[emplID][date];
                        nextSch = emplDaySchemas[emplID][date.AddDays(1).Date];

                        List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                        List<IOPairProcessedTO> annualLeavePairs = new List<IOPairProcessedTO>();
                        if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                        {
                            foreach (WorkTimeIntervalTO interval in dayIntervals)
                            {
                                if ((date.Date.Equals(fromDate.Date) && interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                    || (date.Date.Equals(toDate.Date.AddDays(1)) && interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                    || (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes == 0)
                                    continue;

                                DateTime start = new DateTime(date.Year, date.Month, date.Day, interval.StartTime.Hour, interval.StartTime.Minute, 0);
                                DateTime end = new DateTime(date.Year, date.Month, date.Day, interval.EndTime.Hour, interval.EndTime.Minute, 0);

                                IOPairProcessedTO dayPair = createPair(start, end, emplID, ptOld, emplRules, dayIntervals);

                                if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date) && overlapPair(dayPair, emplOldPairs[emplID][date]))
                                    continue;

                                if (notHolidayTypes)
                                {
                                    DateTime pairDate = date;
                                    
                                    if (interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                        pairDate = pairDate.AddDays(-1).Date;

                                    if ((emplHolidays.ContainsKey(emplID) && emplHolidays[emplID].Contains(pairDate.Date))
                                        || (personalHolidays.ContainsKey(emplID) && personalHolidays[emplID].Contains(pairDate.Date)))
                                        continue;
                                }

                                dayPairs.Add(dayPair);
                            }

                            if (checkAnnualLeave && emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date))
                            {
                                foreach (IOPairProcessedTO pair in emplOldPairs[emplID][date])
                                {
                                    if ((emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && pair.PassTypeID == emplRules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                        || (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && pair.PassTypeID == emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                                        annualLeavePairs.Add(pair);
                                }
                            }
                        }
                        else
                        {
                            if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date))
                            {
                                foreach (IOPairProcessedTO pair in emplOldPairs[emplID][date])
                                {
                                    dayPairs.Add(new IOPairProcessedTO(pair));
                                }
                            }
                        }

                        if (action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME)
                        {
                            IOPairProcessedTO overtimePair = createPair(fromTime, toTime, emplID, ptOld, emplRules, dayIntervals);

                            if (overtimePair.EmployeeID != -1)
                                dayPairs.Add(overtimePair);
                            else
                                continue;
                        }

                        bool dayChanged = false;                        
                        int nextIndex = 0;
                        List<IOPairProcessedTO> excludedDayPairs = new List<IOPairProcessedTO>();
                        for (int i = 0; i < dayPairs.Count; i++)
                        {
                            nextIndex++;

                            IOPairProcessedTO pair = dayPairs[i];

                            bool pairAddedToAnnualLeaves = false;
                            if (checkAnnualLeave
                                && ((emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && pair.PassTypeID == emplRules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                || (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && pair.PassTypeID == emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)))
                            {
                                annualLeavePairs.Add(pair);
                                pairAddedToAnnualLeaves = true;
                            }

                            // previous day pairs from first day do not belong to selected period                            
                            if (action != (int)Constants.MassiveInputActions.ENTERING_OVERTIME)
                            {
                                // previous day pairs day after last day belong to selected period
                                if (pair.IOPairDate.Date.Equals(fromDate.Date) || pair.IOPairDate.Date.Equals(toDate.AddDays(1).Date))
                                {
                                    bool previousDayPair = Common.Misc.isPreviousDayPair(pair, ptDict, dayPairs, daySch, dayIntervals);

                                    if ((pair.IOPairDate.Date.Equals(fromDate.Date) && previousDayPair)
                                        || (pair.IOPairDate.Date.Equals(toDate.AddDays(1).Date) && !previousDayPair))
                                        continue;
                                }
                            }

                            // if not holidays type is selected, check if pair is on personal or national holiday
                            if (notHolidayTypes)
                            {
                                DateTime pairDate = pair.IOPairDate.Date;

                                bool previousDayPair = Common.Misc.isPreviousDayPair(pair, ptDict, dayPairs, daySch, dayIntervals);

                                if (previousDayPair)
                                    pairDate = pairDate.AddDays(-1).Date;

                                if ((emplHolidays.ContainsKey(emplID) && emplHolidays[emplID].Contains(pairDate.Date))
                                    || (personalHolidays.ContainsKey(emplID) && personalHolidays[emplID].Contains(pairDate.Date)))
                                    continue;
                            }

                            if (action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME)
                            {
                                // continue if existing pair is processing
                                bool existingPair = false;
                                if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date))
                                {
                                    foreach (IOPairProcessedTO dayPair in emplOldPairs[emplID][date])
                                    {
                                        if (pair.compare(dayPair))
                                        {
                                            existingPair = true;
                                            break;
                                        }
                                    }
                                }

                                if (existingPair)
                                    continue;
                            }
                            else if ((action != (int)Constants.MassiveInputActions.CHANGE_UNCONFIRMED && pair.PassTypeID != ptOld)
                                || (action == (int)Constants.MassiveInputActions.CHANGE_UNCONFIRMED && pair.ConfirmationFlag != (int)Constants.Confirmation.NotConfirmed))
                                continue;

                            bool wholeDayAbsencePair = false;
                            // if whole day absence type is selected, check if pair is whole interval pair and exist whole interval pair from another day if pair is from third shift
                            if (wholeDayAbsenceType)
                            {
                                // for entering regular action all 'old' pairs are created from whole intervals
                                if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                    wholeDayAbsencePair = true;
                                else
                                {
                                    foreach (WorkTimeIntervalTO interval in dayIntervals)
                                    {
                                        if (Common.Misc.isWholeIntervalPair(pair, interval, daySch))
                                        {
                                            wholeDayAbsencePair = true;
                                            break;
                                        }
                                    }
                                }

                                if (pair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                {
                                    if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                        wholeDayAbsencePair = true;
                                    else
                                        wholeDayAbsencePair = false;

                                    bool existThirdShiftPair = false;
                                    // check pairs from previous day
                                    if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date.AddDays(-1).Date))
                                    {
                                        foreach (IOPairProcessedTO prevPair in emplOldPairs[emplID][date.AddDays(-1).Date])
                                        {
                                            if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                            {
                                                // check if there are pairs in previous day in start third shift interval
                                                if (Common.Misc.isThirdShiftBeginningInterval(Common.Misc.getPairInterval(prevPair, emplOldPairs[emplID][date.AddDays(-1).Date], prevSch, prevIntervals, ptDict)))
                                                {
                                                    existThirdShiftPair = true;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (prevPair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0) && prevPair.PassTypeID == ptOld)
                                                {
                                                    foreach (WorkTimeIntervalTO prevInterval in prevIntervals)
                                                    {
                                                        if (Common.Misc.isWholeIntervalPair(prevPair, prevInterval, prevSch))
                                                        {
                                                            wholeDayAbsencePair = true;
                                                            break;
                                                        }
                                                    }

                                                    if (wholeDayAbsencePair)
                                                        break;
                                                }
                                            }
                                        }

                                        if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR && existThirdShiftPair)
                                        {
                                            excludedDayPairs.Add(pair);
                                            continue;
                                        }
                                    }
                                }

                                if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                {
                                    if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                        wholeDayAbsencePair = true;
                                    else
                                        wholeDayAbsencePair = false;

                                    bool existThirdShiftPair = false;
                                    // check pairs from next day
                                    if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date.AddDays(1).Date))
                                    {
                                        foreach (IOPairProcessedTO nextPair in emplOldPairs[emplID][date.AddDays(1).Date])
                                        {
                                            if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                            {
                                                // check if there are pairs in previous day in start third shift interval
                                                if (Common.Misc.isThirdShiftEndInterval(Common.Misc.getPairInterval(nextPair, emplOldPairs[emplID][date.AddDays(1).Date], nextSch, nextIntervals, ptDict)))
                                                {
                                                    existThirdShiftPair = true;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (nextPair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && nextPair.PassTypeID == ptOld)
                                                {
                                                    foreach (WorkTimeIntervalTO nextInterval in nextIntervals)
                                                    {
                                                        if (Common.Misc.isWholeIntervalPair(nextPair, nextInterval, nextSch))
                                                        {
                                                            wholeDayAbsencePair = true;
                                                            break;
                                                        }
                                                    }

                                                    if (wholeDayAbsencePair)
                                                        break;
                                                }
                                            }
                                        }

                                        if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR && existThirdShiftPair)
                                        {
                                            excludedDayPairs.Add(pair);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if ((action != (int)Constants.MassiveInputActions.CHANGE_REGULAR && action != (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                || !wholeDayAbsenceType || wholeDayAbsencePair)
                            {
                                string error = "";

                                // validate changing pair type
                                List<IOPairProcessedTO> oldPairs = new List<IOPairProcessedTO>();                                
                                oldPairs.Add(new IOPairProcessedTO(pair));
                                List<IOPairProcessedTO> newPairs = new List<IOPairProcessedTO>();

                                // create new pair - if number of hours is checked, split last pair if there is not enough hours
                                IOPairProcessedTO newPair = new IOPairProcessedTO(pair);
                                IOPairProcessedTO newPairSplit = new IOPairProcessedTO(pair);

                                if (checkHours)
                                {
                                    if (numOfMinutesConverted >= numHours.Value * 60)
                                    {
                                        if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                        {
                                            // if pair is not whole intervals, it is split so do not exclude it
                                            if (Common.Misc.isWholeIntervalPair(pair, Common.Misc.getPairInterval(pair, dayPairs, daySch, dayIntervals, ptDict), daySch))
                                                excludedDayPairs.Add(pair);
                                        }
                                        continue;
                                    }

                                    int pairDuration = (int)newPair.EndTime.Subtract(newPair.StartTime).TotalMinutes;

                                    if (newPair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        pairDuration++;

                                    if (numOfMinutesConverted + pairDuration > numHours.Value * 60)
                                    {
                                        // split pair
                                        int numOfMinutes = (int)(numHours.Value * 60 - numOfMinutesConverted);
                                        newPair.EndTime = newPair.StartTime.AddMinutes(numOfMinutes);
                                        newPairSplit.StartTime = newPair.EndTime;
                                    }
                                }

                                newPair.PassTypeID = ptNew;
                                newPairs.Add(newPair);

                                if (!newPair.StartTime.Equals(newPairSplit.StartTime))
                                {
                                    List<IOPairProcessedTO> newDayPairs = new List<IOPairProcessedTO>();

                                    foreach (IOPairProcessedTO dayPair in dayPairs)
                                    {
                                        if (!pair.StartTime.Equals(dayPair.StartTime) && !pair.EndTime.Equals(dayPair.EndTime))
                                            newDayPairs.Add(pair);
                                        else
                                        {
                                            newDayPairs.Add(newPair);
                                            newDayPairs.Add(newPairSplit);
                                        }
                                    }
                                    // if pairs are splited, validate duration rounding, shift rules
                                    // if action is entering new regular hours, check if new pairs overlap some already existing pair
                                    if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR && emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date)
                                        && overlapPair(newPair, emplOldPairs[emplID][date]))
                                    {
                                        excludedDayPairs.Add(pair);
                                        continue;
                                    }

                                    if (action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME && (overlapInterval(newPair, dayIntervals)
                                        || (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date) && overlapPair(newPair, emplOldPairs[emplID][date]))))
                                        continue;

                                    if (action == (int)Constants.MassiveInputActions.CHANGE_OVERTIME)
                                        error = validateOvertimePair(newPair, newPair.StartTime, newPair.EndTime, emplRules, dayIntervals, newDayPairs, daySch);
                                    else if (action == (int)Constants.MassiveInputActions.CHANGE_REGULAR
                                        || action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                        error = validatePair(newPair, newPair.StartTime, newPair.EndTime, emplRules);
                                    if (!error.Trim().Equals(""))
                                    {
                                        changeAllowed = false;

                                        if (!emplDateMessages.ContainsKey(emplID))
                                            emplDateMessages.Add(emplID, new Dictionary<DateTime, List<string>>());
                                        if (!emplDateMessages[emplID].ContainsKey(date.Date))
                                            emplDateMessages[emplID].Add(date.Date, new List<string>());
                                        emplDateMessages[emplID][date.Date].Add(error);

                                        if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                            excludedDayPairs.Add(pair);

                                        break;
                                    }

                                    if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR && emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date)
                                        && overlapPair(newPairSplit, emplOldPairs[emplID][date]))
                                    {
                                        excludedDayPairs.Add(pair);
                                        continue;
                                    }

                                    if (action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME && (overlapInterval(newPairSplit, dayIntervals)
                                        || (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date) && overlapPair(newPairSplit, emplOldPairs[emplID][date]))))
                                        continue;

                                    if (action == (int)Constants.MassiveInputActions.CHANGE_OVERTIME)
                                        error = validateOvertimePair(newPairSplit, newPairSplit.StartTime, newPairSplit.EndTime, emplRules, dayIntervals, newDayPairs, daySch);
                                    else if (action == (int)Constants.MassiveInputActions.CHANGE_REGULAR
                                        || action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                        error = validatePair(newPairSplit, newPairSplit.StartTime, newPairSplit.EndTime, emplRules);
                                    if (!error.Trim().Equals(""))
                                    {
                                        changeAllowed = false;

                                        if (!emplDateMessages.ContainsKey(emplID))
                                            emplDateMessages.Add(emplID, new Dictionary<DateTime, List<string>>());
                                        if (!emplDateMessages[emplID].ContainsKey(date.Date))
                                            emplDateMessages[emplID].Add(date.Date, new List<string>());
                                        emplDateMessages[emplID][date.Date].Add(error);

                                        if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                            excludedDayPairs.Add(pair);

                                        break;
                                    }

                                    newPairs.Add(newPairSplit);
                                }
                                else if (ptDict.ContainsKey(newPair.PassTypeID) && ptDict[newPair.PassTypeID].IsPass == Constants.overtimePassType)
                                {
                                    if (action == (int)Constants.MassiveInputActions.ENTERING_OVERTIME && (overlapInterval(newPair, dayIntervals)
                                        || (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date) && overlapPair(newPair, emplOldPairs[emplID][date]))))
                                        continue;

                                    // if new pair is overtime, validate duration, rounding, shift rules
                                    error = validateOvertimePair(newPair, newPair.StartTime, newPair.EndTime, emplRules, dayIntervals, dayPairs, daySch);
                                    if (!error.Trim().Equals(""))
                                    {
                                        changeAllowed = false;

                                        if (!emplDateMessages.ContainsKey(emplID))
                                            emplDateMessages.Add(emplID, new Dictionary<DateTime, List<string>>());
                                        if (!emplDateMessages[emplID].ContainsKey(date.Date))
                                            emplDateMessages[emplID].Add(date.Date, new List<string>());
                                        emplDateMessages[emplID][date.Date].Add(error);

                                        break;
                                    }
                                }
                                else
                                {
                                    if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR && emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date)
                                        && overlapPair(newPair, emplOldPairs[emplID][date]))
                                    {
                                        excludedDayPairs.Add(pair);
                                        continue;
                                    }

                                    // validate duration
                                    error = validatePair(newPair, newPair.StartTime, newPair.EndTime, emplRules);
                                    if (!error.Trim().Equals(""))
                                    {
                                        changeAllowed = false;

                                        if (!emplDateMessages.ContainsKey(emplID))
                                            emplDateMessages.Add(emplID, new Dictionary<DateTime, List<string>>());
                                        if (!emplDateMessages[emplID].ContainsKey(date.Date))
                                            emplDateMessages[emplID].Add(date.Date, new List<string>());
                                        emplDateMessages[emplID][date.Date].Add(error);

                                        if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                            excludedDayPairs.Add(pair);

                                        break;
                                    }
                                }

                                Dictionary<DateTime, WorkTimeSchemaTO> daySchemas = new Dictionary<DateTime, WorkTimeSchemaTO>();
                                daySchemas.Add(pair.IOPairDate.Date, daySch);
                                Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervalsList = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                                // add previous dat and next day intervals becouse of third shifts vacation using before bank hours are used check
                                dayIntervalsList.Add(pair.IOPairDate.Date.AddDays(-1), prevIntervals);
                                dayIntervalsList.Add(pair.IOPairDate.Date, dayIntervals);
                                dayIntervalsList.Add(pair.IOPairDate.Date.AddDays(1), nextIntervals);

                                List<IOPairProcessedTO> fromPairsList = null;
                                List<IOPairProcessedTO> toPairsList = null;

                                if (checkAnnualLeave)
                                {
                                    fromPairsList = weekPairs;
                                    toPairsList = new List<IOPairProcessedTO>();
                                    DateTime weekEnd = Common.Misc.getWeekEnding(date.Date);
                                    DateTime weekDate = date.AddDays(1).Date;
                                    while (weekDate.Date <= weekEnd.Date && weekDate <= toDate.Date)
                                    {
                                        if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(weekDate.Date))
                                        {
                                            foreach (IOPairProcessedTO oldP in emplOldPairs[emplID][weekDate.Date])
                                            {
                                                if (alList.Contains(oldP.PassTypeID))
                                                    toPairsList.Add(oldP);
                                            }
                                        }

                                        weekDate = weekDate.AddDays(1).Date;
                                    }

                                    if (Common.Misc.getWeekBeggining(toDate.Date).Date.Equals(Common.Misc.getWeekBeggining(date.Date).Date))
                                        toPairsList.AddRange(toWeekAnnualPairs);

                                    oldPairs = new List<IOPairProcessedTO>();
                                    newPairs = new List<IOPairProcessedTO>();

                                    foreach (IOPairProcessedTO newP in dayPairs)
                                    {                                        
                                        oldPairs.Add(new IOPairProcessedTO(newP));

                                        if (pair.RecID != newP.RecID)
                                            newPairs.Add(new IOPairProcessedTO(newP));
                                        else if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                        {
                                            if (pair.StartTime != newP.StartTime)
                                                newPairs.Add(new IOPairProcessedTO(newP));
                                            else
                                                newPairs.Add(newPair);
                                        }
                                        else
                                            newPairs.Add(newPair);
                                    }
                                }

                                List<DateTime> exceptDates = new List<DateTime>();
                                List<IOPairProcessedTO> newCollectivePairs = new List<IOPairProcessedTO>();
                                DateTime currDay = fromDate.Date;
                                while (currDay.Date <= pair.IOPairDate.Date)
                                {
                                    // if actions is entering regular do not exclude days
                                    if (!exceptDates.Contains(currDay.Date) && action != (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                        exceptDates.Add(currDay.Date);

                                    if (currDay.Date < pair.IOPairDate.Date)
                                    {
                                        if (emplNewPairs.ContainsKey(emplID) && emplNewPairs[emplID].ContainsKey(currDay.Date))
                                            newCollectivePairs.AddRange(emplNewPairs[emplID][currDay.Date]);
                                        else if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(currDay.Date))
                                            newCollectivePairs.AddRange(emplOldPairs[emplID][currDay.Date]);
                                    }
                                    else
                                        newCollectivePairs.AddRange(dayPairs);

                                    currDay = currDay.Date.AddDays(1);
                                    continue;
                                }

                                // get previous day pairs
                                List<IOPairProcessedTO> prevDayPairs = null;
                                if (pair.IOPairDate.Date > fromDate.Date)
                                {
                                    prevDayPairs = new List<IOPairProcessedTO>();

                                    if (emplNewPairs.ContainsKey(emplID) && emplNewPairs[emplID].ContainsKey(pair.IOPairDate.Date.AddDays(-1)))
                                        prevDayPairs = emplNewPairs[emplID][pair.IOPairDate.Date.AddDays(-1)];
                                    else if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(pair.IOPairDate.Date.AddDays(-1)))
                                        prevDayPairs = emplOldPairs[emplID][pair.IOPairDate.Date.AddDays(-1)];
                                }

                                // create counters copy and set it to real counters only if validation is OK
                                Dictionary<int, EmployeeCounterValueTO> emplCountersCopy = new Dictionary<int, EmployeeCounterValueTO>();
                                if (emplNewCounters.ContainsKey(emplID))
                                {
                                    foreach (int type in emplNewCounters[emplID].Keys)
                                    {
                                        emplCountersCopy.Add(type, new EmployeeCounterValueTO(emplNewCounters[emplID][type]));
                                    }
                                }

                                error = Common.Misc.validatePairsPassType(pair.EmployeeID, asco, pair.IOPairDate, pair.IOPairDate, newPairs, oldPairs, dayPairs, ref emplCountersCopy, emplRules,
                                    ptDict, ptLimitsDict, schemaDict, daySchemas, dayIntervalsList, fromPairsList, toPairsList, paidLeavesElementaryPairsDict, newCollectivePairs, exceptDates,
                                    prevDayPairs, null, rbCheckLimit.Checked, false, true, true);

                                if (!error.Trim().Equals(""))
                                {
                                    if (!emplDateMessages.ContainsKey(emplID))
                                        emplDateMessages.Add(emplID, new Dictionary<DateTime, List<string>>());
                                    if (!emplDateMessages[emplID].ContainsKey(date.Date))
                                        emplDateMessages[emplID].Add(date.Date, new List<string>());
                                    emplDateMessages[emplID][date.Date].Add(error);

                                    if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                                        excludedDayPairs.Add(pair);

                                    // if pair can not be changed becouse labor law rules, continue, else stop employee processing
                                    if (error.Trim().Equals("notEnoughFreeHoursBetweenWorkingHours"))
                                        continue;
                                    else
                                    {
                                        changeAllowed = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (emplNewCounters.ContainsKey(emplID))
                                        emplNewCounters[emplID] = emplCountersCopy;

                                    pair.PassTypeID = ptNew;
                                    pair.ManualCreated = (int)Constants.recordCreated.Manualy;
                                    pair.VerificationFlag = (int)Constants.Verification.Verified;
                                    pair.CreatedBy = Constants.massiveAdminUser;

                                    if (pair.PassTypeID == Constants.absence
                                        || (emplRules.ContainsKey(Constants.RuleCompanyInitialOvertime) && pair.PassTypeID == emplRules[Constants.RuleCompanyInitialOvertime].RuleValue)
                                        || (emplRules.ContainsKey(Constants.RuleCompanyInitialNightOvertime) && pair.PassTypeID == emplRules[Constants.RuleCompanyInitialNightOvertime].RuleValue))
                                    {
                                        pair.VerifiedBy = "";
                                        pair.VerifiedTime = new DateTime();
                                        pair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                                    }
                                    else
                                    {
                                        pair.VerifiedBy = NotificationController.GetLogInUser().UserID;
                                        pair.VerifiedTime = DateTime.Now;
                                    }

                                    if (checkHours)
                                    {
                                        pair.EndTime = newPair.EndTime;

                                        int pairDuration = (int)newPair.EndTime.Subtract(newPair.StartTime).TotalMinutes;

                                        if (newPair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                            pairDuration++;

                                        numOfMinutesConverted += pairDuration;

                                        if (!newPair.StartTime.Equals(newPairSplit.StartTime))
                                        {
                                            newPairSplit.ManualCreated = (int)Constants.recordCreated.Manualy;
                                            newPairSplit.VerificationFlag = (int)Constants.Verification.Verified;
                                            pair.CreatedBy = Constants.massiveAdminUser;

                                            if (newPairSplit.PassTypeID == Constants.absence
                                                || (emplRules.ContainsKey(Constants.RuleCompanyInitialOvertime) && newPairSplit.PassTypeID == emplRules[Constants.RuleCompanyInitialOvertime].RuleValue)
                                                || (emplRules.ContainsKey(Constants.RuleCompanyInitialNightOvertime) && newPairSplit.PassTypeID == emplRules[Constants.RuleCompanyInitialNightOvertime].RuleValue))
                                            {
                                                newPairSplit.VerifiedBy = "";
                                                newPairSplit.VerifiedTime = new DateTime();
                                                newPairSplit.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                                            }
                                            else
                                            {
                                                newPairSplit.VerifiedBy = NotificationController.GetLogInUser().UserID;
                                                newPairSplit.VerifiedTime = DateTime.Now;
                                            }

                                            dayPairs.Insert(nextIndex, newPairSplit);
                                        }
                                    }

                                    dayChanged = true;

                                    // remove old pair from annual leaves if it was added to list, it is last added in list
                                    if (pairAddedToAnnualLeaves && annualLeavePairs.Count > 0)
                                        annualLeavePairs.RemoveAt(annualLeavePairs.Count - 1);

                                    if (checkAnnualLeave
                                        && ((emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && pair.PassTypeID == emplRules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                        || (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && pair.PassTypeID == emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)))
                                        annualLeavePairs.Add(pair);
                                }
                            }
                        }

                        weekPairs.AddRange(annualLeavePairs);

                        if (dayChanged)
                        {
                            if (!emplNewPairs.ContainsKey(emplID))
                            {
                                emplNewPairs.Add(emplID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                                emplIDsChanged += emplID.ToString().Trim() + ",";
                            }

                            if (!emplNewPairs[emplID].ContainsKey(date.Date))
                                emplNewPairs[emplID].Add(date.Date, new List<IOPairProcessedTO>());

                            if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                            {
                                emplNewPairs[emplID][date.Date] = new List<IOPairProcessedTO>();

                                foreach (IOPairProcessedTO dayPair in dayPairs)
                                {
                                    bool addPair = true;

                                    foreach (IOPairProcessedTO excludePair in excludedDayPairs)
                                    {
                                        if (dayPair.compare(excludePair))
                                        {
                                            addPair = false;
                                            break;
                                        }
                                    }

                                    if (addPair)
                                        emplNewPairs[emplID][date.Date].Add(dayPair);
                                }
                            }
                            else
                                emplNewPairs[emplID][date.Date] = dayPairs;

                            newPairsAll.AddRange(emplNewPairs[emplID][date.Date]);
                        }

                        if (!changeAllowed)
                            break;
                    }
                }

                if (emplIDsChanged.Length > 0)
                    emplIDsChanged = emplIDsChanged.Substring(0, emplIDsChanged.Length - 1);

                startIndex = 0;
                string logName = generateLog(true);

                // if action is entering regular pairs, empty old pairs dictioanry becouse none of old pairs should not be deleted
                if (action == (int)Constants.MassiveInputActions.ENTERING_REGULAR)
                    emplOldPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                populatePreview();
                                
                if (logName.Trim().Equals(""))
                    MessageBox.Show(rm.GetString("previewLogNotGenerated", culture));
                else
                    MessageBox.Show(rm.GetString("previewLogGenerated", culture) + " " + logName);                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.fillAbsence(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        
        public void populatePreview()
        {
            try
            {                
                if (newPairsAll.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvPreview.BeginUpdate();
                lvPreview.Items.Clear();

                if ((startIndex >= 0) && (startIndex < newPairsAll.Count))
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
                    if (lastIndex >= newPairsAll.Count)
                    {
                        btnNext.Enabled = false;
                        lastIndex = newPairsAll.Count;
                    }
                    else
                    {
                        btnNext.Enabled = true;
                    }

                    for (int i = startIndex; i < lastIndex; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        EmployeeTO empl = new EmployeeTO();
                        if (emplDict.ContainsKey(newPairsAll[i].EmployeeID))
                            empl = emplDict[newPairsAll[i].EmployeeID];
                        
                        item.Text = newPairsAll[i].EmployeeID.ToString().Trim();
                        item.SubItems.Add(empl.FirstAndLastName.Trim());                        
                        item.SubItems.Add(newPairsAll[i].IOPairDate.Date.ToString(Constants.dateFormat));
                        item.SubItems.Add(newPairsAll[i].StartTime.ToString(Constants.timeFormat));
                        item.SubItems.Add(newPairsAll[i].EndTime.ToString(Constants.timeFormat));
                        if (ptDict.ContainsKey(newPairsAll[i].OldPassTypeID))
                        {
                            if (NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_en.Trim().ToUpper()))
                                item.SubItems.Add(ptDict[newPairsAll[i].OldPassTypeID].DescriptionAltAndID.Trim());
                            else
                                item.SubItems.Add(ptDict[newPairsAll[i].OldPassTypeID].DescriptionAndID.Trim());
                        }
                        else
                            item.SubItems.Add("");
                        if (ptDict.ContainsKey(newPairsAll[i].PassTypeID))
                        {
                            if (NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_en.Trim().ToUpper()))
                                item.SubItems.Add(ptDict[newPairsAll[i].PassTypeID].DescriptionAltAndID.Trim());
                            else
                                item.SubItems.Add(ptDict[newPairsAll[i].PassTypeID].DescriptionAndID.Trim());
                        }
                        else
                            item.SubItems.Add("");
                        if (ascoDict.ContainsKey(newPairsAll[i].EmployeeID))
                            item.SubItems.Add(ascoDict[newPairsAll[i].EmployeeID].NVarcharValue2.Trim());
                        else
                            item.SubItems.Add("");   

                        item.Tag = newPairsAll[i];
                        WorkingUnitTO ccWU = Common.Misc.getEmplCostCenter(empl, wuDict, null);
                        item.SubItems.Add(ccWU.Code.Trim());
                        item.SubItems.Add(ccWU.Description.Trim());
                        if (ascoDict.ContainsKey(newPairsAll[i].EmployeeID))
                            item.SubItems.Add(ascoDict[newPairsAll[i].EmployeeID].NVarcharValue6.Trim());
                        else
                            item.SubItems.Add("");

                        lvPreview.Items.Add(item);
                    }
                }

                lvPreview.EndUpdate();
                lvPreview.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.populatePreview(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnExportPreview_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvPreview.Items.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                // insert header                
                ws.Cells[1, 1] = rm.GetString("hdrID", culture);                
                ws.Cells[1, 2] = rm.GetString("hdrName", culture);                
                ws.Cells[1, 3] = rm.GetString("hdrDate", culture);
                ws.Cells[1, 4] = rm.GetString("hdrStart", culture);
                ws.Cells[1, 5] = rm.GetString("hdrEnd", culture);
                ws.Cells[1, 6] = rm.GetString("hdrPTBefore", culture);
                ws.Cells[1, 7] = rm.GetString("hdrPTAfter", culture);
                ws.Cells[1, 8] = rm.GetString("hdrStringone", culture);
                ws.Cells[1, 9] = rm.GetString("hdrCostCenter", culture);
                ws.Cells[1, 10] = rm.GetString("hdrCostCenterDesc", culture);
                ws.Cells[1, 11] = rm.GetString("hdrBranch", culture);

                int colNum = 11;

                setRowFontWeight(ws, 1, colNum, true);

                int i = 2;

                for (int index = 0; index < newPairsAll.Count; index++)
                {                    
                    EmployeeTO empl = new EmployeeTO();
                    if (emplDict.ContainsKey(newPairsAll[index].EmployeeID))
                        empl = emplDict[newPairsAll[index].EmployeeID];

                    ws.Cells[i, 1] = newPairsAll[index].EmployeeID.ToString().Trim();
                    ws.Cells[i, 2] = empl.FirstAndLastName.Trim();
                    ws.Cells[i, 3] = newPairsAll[index].IOPairDate.Date.ToString(Constants.dateFormat);
                    ws.Cells[i, 4] = newPairsAll[index].StartTime.ToString(Constants.timeFormat);
                    ws.Cells[i, 5] = newPairsAll[index].EndTime.ToString(Constants.timeFormat);
                    if (ptDict.ContainsKey(newPairsAll[index].OldPassTypeID))
                    {
                        if (NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_en.Trim().ToUpper()))
                            ws.Cells[i, 6] = ptDict[newPairsAll[index].OldPassTypeID].DescriptionAltAndID.Trim();
                        else
                            ws.Cells[i, 6] = ptDict[newPairsAll[index].OldPassTypeID].DescriptionAndID.Trim();
                    }
                    else
                        ws.Cells[i, 6] = "";
                    if (ptDict.ContainsKey(newPairsAll[index].PassTypeID))
                    {
                        if (NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_en.Trim().ToUpper()))
                            ws.Cells[i, 7] = ptDict[newPairsAll[index].PassTypeID].DescriptionAltAndID.Trim();
                        else
                            ws.Cells[i, 7] = ptDict[newPairsAll[index].PassTypeID].DescriptionAndID.Trim();
                    }
                    else
                        ws.Cells[i, 7] = "";
                    if (ascoDict.ContainsKey(newPairsAll[index].EmployeeID))
                        ws.Cells[i, 8] = ascoDict[newPairsAll[index].EmployeeID].NVarcharValue2.Trim();
                    else
                        ws.Cells[i, 8] = "";
                    
                    WorkingUnitTO ccWU = Common.Misc.getEmplCostCenter(empl, wuDict, null);
                    ws.Cells[i, 9] = ccWU.Code.Trim();
                    ws.Cells[i, 10] = ccWU.Description.Trim();
                    if (ascoDict.ContainsKey(newPairsAll[index].EmployeeID))
                        ws.Cells[i, 11] = ascoDict[newPairsAll[index].EmployeeID].NVarcharValue6.Trim();
                    else
                        ws.Cells[i, 11] = "";

                    i++;
                }
                
                ws.Columns.AutoFit();
                ws.Rows.AutoFit();
                                
                string reportName = "Preview_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "XLS (*.xls)|*.xls";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;

                wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

                wb.Close(true, null, null);
                xla.Workbooks.Close();
                xla.Quit();

                releaseObject(ws);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnExportPreview_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string generateLog(bool isPreview)
        {
            try
            {
                string filePath = tbLogPath.Text;

                if (!System.IO.Directory.Exists(filePath))
                    return "";

                this.Cursor = Cursors.WaitCursor;

                Dictionary<string, string> messages = Constants.MassiveInputFailedMessages();
                
                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                // insert header                
                ws.Cells[1, 1] = rm.GetString("hdrID", culture);
                ws.Cells[1, 2] = rm.GetString("hdrName", culture);
                ws.Cells[1, 3] = rm.GetString("hdrDate", culture);
                ws.Cells[1, 4] = rm.GetString("hdrStatus", culture);
                ws.Cells[1, 5] = rm.GetString("hdrRemark", culture);
                
                int colNum = 5;

                setRowFontWeight(ws, 1, colNum, true);

                int i = 2;
                foreach (int emplID in emplDict.Keys)
                {
                    DateTime date = dtpFrom.Value.Date;
                    while (date.Date <= dtpTo.Value.Date.AddDays(1))
                    {
                        ws.Cells[i, 1] = emplID.ToString().Trim();
                        ws.Cells[i, 2] = emplDict[emplID].FirstAndLastName.Trim();
                        ws.Cells[i, 3] = date.ToString(Constants.dateFormat);

                        if (emplDateMessages.ContainsKey(emplID) && (!isPreview || emplDateMessages[emplID].ContainsKey(date.Date)))
                        {                           
                            ws.Cells[i, 4] = Constants.MassiveInputStatus.FAILED.ToString().Trim();

                            string remark = "";

                            if (emplDateMessages[emplID].ContainsKey(date.Date))
                            {
                                foreach (string msg in emplDateMessages[emplID][date.Date])
                                {
                                    if (messages.ContainsKey(msg))
                                        remark += messages[msg].Trim() + " ";
                                    else
                                        remark = msg + " ";
                                }
                            }
                            ws.Cells[i, 5] = remark;
                        }
                        else
                        {
                            if (emplNewPairs.ContainsKey(emplID) && emplNewPairs[emplID].ContainsKey(date.Date))
                            {
                                ws.Cells[i, 4] = Constants.MassiveInputStatus.DONE.ToString().Trim();
                                ws.Cells[i, 5] = "";
                            }
                            else
                            {
                                ws.Cells[i, 4] = Constants.MassiveInputStatus.NO_CHANGES.ToString().Trim();
                                ws.Cells[i, 5] = "";
                            }
                        }

                        date = date.AddDays(1);
                        i++;
                    }
                }

                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                string reportName = "";

                if (isPreview)
                    reportName += "LogPreview_";
                else
                    reportName += "Log_";

                reportName += DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                //SaveFileDialog sfd = new SaveFileDialog();
                //sfd.FileName = reportName;
                //sfd.InitialDirectory = Constants.csvDocPath;
                //sfd.Filter = "XLS (*.xls)|*.xls";

                //if (sfd.ShowDialog() != DialogResult.OK)
                //    return;

                //string filePath = sfd.FileName;

                filePath += "\\" + reportName;

                wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

                wb.Close(true, null, null);
                xla.Workbooks.Close();
                xla.Quit();

                releaseObject(ws);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;

                return filePath;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.generateLog(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setRowFontWeight(Microsoft.Office.Interop.Excel.Worksheet ws, int row, int colNum, bool isBold)
        {
            try
            {
                for (int i = 1; i <= colNum; i++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)ws.Cells[row, i]).Font.Bold = isBold;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.setRowFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " AbsenceMassiveInput.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private string validatePeriod()
        {
            try
            {
                int hrsscCutOffDate = -1;

                // get login employee
                EmployeeTO empl = new Employee().FindUserEmployee(NotificationController.GetLogInUser().UserID);

                // get hrssc cut off date
                int company = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wuDict);

                if (rules.ContainsKey(company) && rules[company].ContainsKey(empl.EmployeeTypeID) && rules[company][empl.EmployeeTypeID].ContainsKey(Constants.RuleHRSSCCutOffDate))
                    hrsscCutOffDate = rules[company][empl.EmployeeTypeID][Constants.RuleHRSSCCutOffDate].RuleValue;
                                
                DateTime currentMonth = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;

                if (dtpFrom.Value.Date < currentMonth.AddMonths(-1).Date)
                    return rm.GetString("changingLessPreviousMonth", culture);
                else if (Common.Misc.countWorkingDays(DateTime.Now.Date, null) > hrsscCutOffDate && dtpFrom.Value.Date < currentMonth.Date)
                    return rm.GetString("changingPreviousMonthCuttOffDatePassed", culture);
                else
                    return "";                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.validateAutoCheckDateInterval(): " + ex.Message + "\n");
                throw ex;
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
                populatePreview();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populatePreview();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (emplNewPairs.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noChangesToSave", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                // get already locked days
                Dictionary<int, List<DateTime>> emplLockedDays = new EmployeeLockedDay().SearchLockedDays(emplIDsChanged, Constants.LockedDayType.MASSIVE_INPUT.ToString().Trim(), dtpFrom.Value, dtpTo.Value);

                IOPairProcessed pair = new IOPairProcessed();
                EmployeeCounterValue counter = new EmployeeCounterValue();
                IOPairsProcessedHist pairHist = new IOPairsProcessedHist();
                EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                EmployeeLockedDay lockedDay = new EmployeeLockedDay();

                emplDateMessages = new Dictionary<int, Dictionary<DateTime, List<string>>>();

                bool savedAll = true;
                foreach (int emplID in emplNewPairs.Keys)
                {                    
                    pair = new IOPairProcessed();
                    if (pair.BeginTransaction())
                    {
                        try
                        {
                            counter = new EmployeeCounterValue();
                            pairHist = new IOPairsProcessedHist();
                            counterHist = new EmployeeCounterValueHist();
                            bool saved = true;

                            // get counters for this employee
                            Dictionary<int, EmployeeCounterValueTO> counters = new Dictionary<int, EmployeeCounterValueTO>();
                            if (emplNewCounters.ContainsKey(emplID))
                                counters = emplNewCounters[emplID];

                            // get compare counters
                            Dictionary<int, EmployeeCounterValueTO> oldCounters = new Dictionary<int, EmployeeCounterValueTO>();
                            if (emplOldCounters.ContainsKey(emplID))
                                oldCounters = emplOldCounters[emplID];

                            // get old and new pairs for this employee
                            // get old pairs for this employee and changed days
                            List<IOPairProcessedTO> newPairs = new List<IOPairProcessedTO>();
                            List<IOPairProcessedTO> oldPairs = new List<IOPairProcessedTO>();
                            lockedDay.SetTransaction(pair.GetTransaction());
                            foreach (DateTime date in emplNewPairs[emplID].Keys)
                            {
                                // check if day should be locked
                                bool isLockedDay = false;
                                foreach (IOPairProcessedTO nPair in emplNewPairs[emplID][date])
                                {
                                    if (nPair.CreatedBy.Equals(Constants.massiveAdminUser) && !nPair.VerifiedBy.Trim().Equals(""))
                                    {
                                        isLockedDay = true;
                                        break;
                                    }
                                }

                                newPairs.AddRange(emplNewPairs[emplID][date]);

                                if (emplOldPairs.ContainsKey(emplID) && emplOldPairs[emplID].ContainsKey(date))
                                    oldPairs.AddRange(emplOldPairs[emplID][date]);

                                // insert date locked if does not exist
                                if (isLockedDay)
                                {
                                    if (emplLockedDays.ContainsKey(emplID) && emplLockedDays[emplID].Contains(date.Date))
                                        continue;

                                    lockedDay.EmplLockedDayTO = new EmployeeLockedDayTO();
                                    lockedDay.EmplLockedDayTO.EmployeeID = emplID;
                                    lockedDay.EmplLockedDayTO.LockedDate = date.Date;
                                    lockedDay.EmplLockedDayTO.Type = Constants.LockedDayType.MASSIVE_INPUT.ToString();

                                    if (lockedDay.Save(false) <= 0)
                                    {
                                        saved = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    // if day is locked, unlock it
                                    if (emplLockedDays.ContainsKey(emplID) && emplLockedDays[emplID].Contains(date.Date))
                                    {
                                        lockedDay.EmplLockedDayTO = new EmployeeLockedDayTO();
                                        lockedDay.EmplLockedDayTO.EmployeeID = emplID;
                                        lockedDay.EmplLockedDayTO.LockedDate = date.Date;
                                        lockedDay.EmplLockedDayTO.Type = Constants.LockedDayType.MASSIVE_INPUT.ToString();

                                        if (!lockedDay.Delete(false))
                                        {
                                            saved = false;
                                            break;
                                        }
                                    }
                                }
                            }

                            DateTime modifiedTime = DateTime.Now;
                            if (saved)
                            {                                
                                // move old pairs to hist table
                                string recIDs = "";
                                if (oldPairs.Count > 0)
                                {
                                    foreach (IOPairProcessedTO oldPair in oldPairs)
                                    {
                                        recIDs += oldPair.RecID.ToString().Trim() + ",";
                                    }

                                    if (recIDs.Length > 0)
                                        recIDs = recIDs.Substring(0, recIDs.Length - 1);
                                }

                                if (recIDs.Length > 0)
                                {
                                    pairHist.SetTransaction(pair.GetTransaction());
                                    saved = saved && (pairHist.Save(recIDs, Constants.massiveAdminUser, modifiedTime, Constants.alertStatus, false) >= 0);

                                    if (saved)
                                        saved = saved && pair.Delete(recIDs, false);
                                }
                            }
                            
                            if (saved)
                            {
                                foreach (IOPairProcessedTO pairTO in newPairs)
                                {
                                    if ((int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes > 0)
                                    {
                                        pair.IOPairProcessedTO = pairTO;
                                        pair.IOPairProcessedTO.CreatedBy = Constants.massiveAdminUser;
                                        pair.IOPairProcessedTO.CreatedTime = modifiedTime;
                                        saved = saved && (pair.Save(false) >= 0);

                                        if (!saved)
                                            break;
                                    }
                                }
                            }

                            if (saved)
                            {
                                // update counters, updated counters insert to hist table
                                counterHist.SetTransaction(pair.GetTransaction());
                                counter.SetTransaction(pair.GetTransaction());
                                // update counters and move old counter values to hist table if updated
                                foreach (int type in counters.Keys)
                                {
                                    if (oldCounters.ContainsKey(type) && oldCounters[type].Value != counters[type].Value)
                                    {
                                        // move to hist table
                                        counterHist.ValueTO = new EmployeeCounterValueHistTO(oldCounters[type]);
                                        counterHist.ValueTO.ModifiedBy = Constants.massiveAdminUser;
                                        saved = saved && (counterHist.Save(false) > 0);

                                        if (!saved)
                                            break;

                                        counter.ValueTO = new EmployeeCounterValueTO(counters[type]);                                        
                                        counter.ValueTO.ModifiedBy = Constants.massiveAdminUser;

                                        saved = saved && counter.Update(false);

                                        if (!saved)
                                            break;
                                    }
                                }
                            }

                            if (saved)                            
                                pair.CommitTransaction();
                            else
                            {
                                if (pair.GetTransaction() != null)
                                    pair.RollbackTransaction();
                                savedAll = false;

                                if (!emplDateMessages.ContainsKey(emplID))
                                    emplDateMessages.Add(emplID, new Dictionary<DateTime, List<string>>());
                            }
                        }
                        catch (Exception ex)
                        {
                            if (pair.GetTransaction() != null)
                                pair.RollbackTransaction();
                            savedAll = false;
                            log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnSave_Click(): " + ex.Message + "\n");

                            if (!emplDateMessages.ContainsKey(emplID))
                                emplDateMessages.Add(emplID, new Dictionary<DateTime, List<string>>());
                        }
                    }
                    else
                        savedAll = false;
                }

                string logName = generateLog(false);
                string logMsg = "";
                if (logName.Trim().Equals(""))
                    logMsg = rm.GetString("previewLogNotGenerated", culture);
                else
                    logMsg = rm.GetString("previewLogGenerated", culture) + " " + logName;

                if (savedAll)
                    MessageBox.Show(rm.GetString("massiveInputAllSaved", culture) + " " + logMsg);
                else
                    MessageBox.Show(rm.GetString("massiveInputAllFailed", culture) + " " + logMsg);

                emplOldPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                emplNewPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                emplOldCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                emplNewCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                newPairsAll = new List<IOPairProcessedTO>();
                emplDateMessages = new Dictionary<int, Dictionary<DateTime, List<string>>>();
                emplIDsChanged = "";

                populatePreview();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHours_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!chbHours.Checked)
                    numHours.Value = 0;

                numHours.Enabled = chbHours.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.chbHours_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string validatePair(IOPairProcessedTO pair, DateTime start, DateTime end, Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                // validate duration due to minimal presence
                int pairDuration = ((int)end.TimeOfDay.TotalMinutes - (int)start.TimeOfDay.TotalMinutes);

                // if it is last pair from first night shift interval, add one minute
                if (end.Hour == 23 && end.Minute == 59)
                    pairDuration++;

                int minimalPresence = 1;
                if (emplRules.ContainsKey(Constants.RuleMinPresence))
                    minimalPresence = emplRules[Constants.RuleMinPresence].RuleValue;

                if (emplRules.ContainsKey(Constants.RuleCompanyRegularWork) && pair.PassTypeID == emplRules[Constants.RuleCompanyRegularWork].RuleValue)
                {
                    if (end > start && pairDuration < minimalPresence)                    
                        return "pairLessThenMinimum";
                }

                if (emplRules.ContainsKey(Constants.RuleCompanyDelay) && pair.PassTypeID == emplRules[Constants.RuleCompanyDelay].RuleValue)
                {
                    int delayMax = pairDuration;
                    if (emplRules.ContainsKey(Constants.RuleDelayMax))
                        delayMax = emplRules[Constants.RuleDelayMax].RuleValue;

                    if (pairDuration > delayMax)
                        return "delayMaxDurationExceeded";
                }

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string validateOvertimePair(IOPairProcessedTO pair, DateTime start, DateTime end, Dictionary<string, RuleTO> emplRules, 
            List<WorkTimeIntervalTO> dayIntervals, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO sch)
        {
            try
            {
                // validate duration due to minimal presence
                int pairDuration = ((int)end.TimeOfDay.TotalMinutes - (int)start.TimeOfDay.TotalMinutes);

                // if it is last pair from first night shift interval, add one minute
                if (end.Hour == 23 && end.Minute == 59)
                    pairDuration++;

                int minimalPresence = 1;
                if (emplRules.ContainsKey(Constants.RuleOvertimeMinimum))
                    minimalPresence = emplRules[Constants.RuleOvertimeMinimum].RuleValue;

                if (end > start && pair.PassTypeID != Constants.overtimeUnjustified && pairDuration < minimalPresence)
                    return "overtimeLessThenMinimum";

                bool isWorkAbsenceDay = false;

                if (dayIntervals.Count <= 0 || (dayIntervals.Count == 1 && dayIntervals[0].StartTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0))))
                    isWorkAbsenceDay = true;                

                int minPresenceRounding = 1;
                if (!isWorkAbsenceDay)
                {
                    if (emplRules.ContainsKey(Constants.RuleOvertimeRounding))
                        minPresenceRounding = emplRules[Constants.RuleOvertimeRounding].RuleValue;                    
                }
                else
                {
                    if (emplRules.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                        minPresenceRounding = emplRules[Constants.RuleOvertimeRoundingOutWS].RuleValue;
                }

                // validate duration due to overtime rounding rule
                if (pairDuration % minPresenceRounding != 0)                
                    return "overtimeNotValidRoundingRule";

                // validate if shift start/end overtime Rules are satisfied
                int shiftStart = 1;
                int shiftEnd = 1;
                if (emplRules.ContainsKey(Constants.RuleOvertimeShiftStart))
                    shiftStart = emplRules[Constants.RuleOvertimeShiftStart].RuleValue;
                if (emplRules.ContainsKey(Constants.RuleOvertimeShiftEnd))
                    shiftEnd = emplRules[Constants.RuleOvertimeShiftEnd].RuleValue;
                IOPairProcessedTO pairToValidate = new IOPairProcessedTO(pair);
                pairToValidate.StartTime = start;
                pairToValidate.EndTime = end;
                WorkTimeIntervalTO intervalBefore = Common.Misc.getIntervalBeforePair(pairToValidate, dayPairs, dayIntervals, sch, ptDict);
                
                double intervalBeforeDuration = intervalBefore.EndTime.TimeOfDay.Subtract(intervalBefore.StartTime.TimeOfDay).TotalMinutes;

                WorkTimeIntervalTO intervalAfter = Common.Misc.getIntervalAfterPair(pairToValidate, dayPairs, dayIntervals, sch, ptDict);
                
                double intervalAfterDuration = intervalAfter.EndTime.TimeOfDay.Subtract(intervalAfter.StartTime.TimeOfDay).TotalMinutes;

                if (!intervalBefore.EndTime.Equals(new DateTime()) && intervalBeforeDuration > 0 && pairToValidate.EndTime.TimeOfDay.Subtract(intervalBefore.EndTime.TimeOfDay).TotalMinutes < shiftEnd)
                    return "overtimeBeforeShiftEndRule";
                
                if (!intervalAfter.StartTime.Equals(new DateTime()) && intervalAfterDuration > 0 && intervalAfter.StartTime.TimeOfDay.Subtract(pairToValidate.StartTime.TimeOfDay).TotalMinutes < shiftStart)                
                    return "overtimeBeforeShiftStartRule";

                return "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.validateOvertimePair(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnLogBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbDialog = new FolderBrowserDialog();

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    tbLogPath.Text = fbDialog.SelectedPath;                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }    
        }

        private Dictionary<int, List<DateTime>> employeeHolidays(ref Dictionary<int, List<DateTime>> personalHolidays, DateTime startTime, DateTime endTime, string emplIDs, Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules,
            Dictionary<int, WorkTimeSchemaTO> schemas, Dictionary<int, EmployeeTO> employees, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict)
        {
            try
            {
                Dictionary<int, List<DateTime>> emplHolidays = new Dictionary<int, List<DateTime>>();

                string holidayType = "";

                // check if there are personal holidays for selected period and selected employees, no transfering holidays for personal holidays                
                List<EmployeeAsco4TO> ascoList = new EmployeeAsco4().Search(emplIDs);
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(startTime, endTime, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, null, nationalTransferableHolidays);

                // get personal holidays in selected period
                foreach (EmployeeAsco4TO asco in ascoList)
                {
                    // expat out does not have holidays
                    if (employees.ContainsKey(asco.EmployeeID) && Common.Misc.isExpatOut(rulesDict, employees[asco.EmployeeID]))
                        continue;

                    holidayType = asco.NVarcharValue1.Trim();

                    if (!holidayType.Trim().Equals(""))
                    {
                        // if category is IV, find holiday date
                        if (holidayType.Trim().Equals(Constants.holidayTypeIV.Trim()))
                        {
                            if (!emplHolidays.ContainsKey(asco.EmployeeID))
                                emplHolidays.Add(asco.EmployeeID, new List<DateTime>());

                            if (!personalHolidays.ContainsKey(asco.EmployeeID))
                                personalHolidays.Add(asco.EmployeeID, new List<DateTime>());

                            DateTime personalHolidayStartDate = new DateTime(startTime.Year, asco.DatetimeValue1.Month, asco.DatetimeValue1.Day, 0, 0, 0);
                            DateTime personalHolidayEndDate = new DateTime(endTime.Year, asco.DatetimeValue1.Month, asco.DatetimeValue1.Day, 0, 0, 0);
                            if (startTime.Date <= personalHolidayStartDate.Date && endTime.Date >= personalHolidayStartDate.Date)
                            {
                                emplHolidays[asco.EmployeeID].Add(personalHolidayStartDate.Date);
                                personalHolidays[asco.EmployeeID].Add(personalHolidayStartDate.Date);
                            }
                            else if (startTime.Date <= personalHolidayEndDate.Date && endTime.Date >= personalHolidayEndDate.Date)
                            {
                                emplHolidays[asco.EmployeeID].Add(personalHolidayEndDate);
                                personalHolidays[asco.EmployeeID].Add(personalHolidayEndDate);
                            }
                        }
                        else
                        {
                            // if category is I, II or III, check if pair date is personal holiday
                            if (personalHolidayDays.ContainsKey(holidayType.Trim()))
                            {
                                if (!emplHolidays.ContainsKey(asco.EmployeeID))
                                    emplHolidays.Add(asco.EmployeeID, new List<DateTime>());

                                if (!personalHolidays.ContainsKey(asco.EmployeeID))
                                    personalHolidays.Add(asco.EmployeeID, new List<DateTime>());

                                emplHolidays[asco.EmployeeID].AddRange(personalHolidayDays[holidayType.Trim()]);
                                personalHolidays[asco.EmployeeID].AddRange(personalHolidayDays[holidayType.Trim()]);
                            }
                        }
                    }
                }

                // add national holidays to all selected employees, industrial time schemas do not transfer holidays
                string[] IDs = emplIDs.Split(',');
                foreach (string emplID in IDs)
                {
                    int id = -1;
                    if (int.TryParse(emplID.Trim(), out id) && id != -1)
                    {
                        // expat out does not have holidays
                        if (employees.ContainsKey(id) && Common.Misc.isExpatOut(rulesDict, employees[id]))
                            continue;

                        foreach (DateTime natHoliday in nationalHolidaysDays)
                        {
                            if (!emplHolidays.ContainsKey(id))
                                emplHolidays.Add(id, new List<DateTime>());

                            emplHolidays[id].Add(natHoliday.Date);
                        }

                        // add transfered holidays to employees who does not have industrial schema for that day
                        foreach (DateTime natHolidaySunday in nationalHolidaysSundays)
                        {
                            bool isIndustrial = false;

                            if (emplSchedules.ContainsKey(id) && Common.Misc.getTimeSchema(natHolidaySunday.Date, emplSchedules[id], schemas).Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                                isIndustrial = true;

                            if (!isIndustrial)
                            {
                                if (!emplHolidays.ContainsKey(id))
                                    emplHolidays.Add(id, new List<DateTime>());

                                emplHolidays[id].Add(natHolidaySunday.Date);
                            }
                        }
                    }
                }

                return emplHolidays;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.employeeHolidays(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private IOPairProcessedTO createPair(DateTime start, DateTime end, int emplID, int ptID, Dictionary<string, RuleTO> emplRules, List<WorkTimeIntervalTO> dayIntervals)
        {
            try
            {
                DateTime startTime = start;
                DateTime endTime = end;

                if (ptDict.ContainsKey(ptID) && ptDict[ptID].IsPass == Constants.overtimePassType)
                {
                    DateTime dayEnd = start.Date.AddDays(1).AddMinutes(-1);
                    DateTime dayBegining = start.Date;

                    // is it working day
                    bool isWorkAbsenceDay = false;
                    if (dayIntervals.Count == 0 ||
                        (dayIntervals.Count == 1 && dayIntervals[0].StartTime.Hour == 0 && dayIntervals[0].StartTime.Minute == 0))
                        isWorkAbsenceDay = true;

                    // for overtime pass types, do overtime rounding of start and end time                    
                    int minPresenceRounding = 1;
                    if (emplRules.ContainsKey(Constants.RulePresenceRounding))
                    {
                        minPresenceRounding = emplRules[Constants.RulePresenceRounding].RuleValue;

                        if (startTime.Minute % minPresenceRounding != 0)
                        {
                            startTime = startTime.AddMinutes(minPresenceRounding - (startTime.Minute % minPresenceRounding));
                            if (startTime > dayEnd)
                                startTime = dayEnd;

                            if (endTime.TimeOfDay != new TimeSpan(23, 59, 0) && endTime.Minute % minPresenceRounding != 0)
                            {
                                endTime = endTime.AddMinutes(-(endTime.Minute % minPresenceRounding));
                                if (endTime < dayBegining)
                                    endTime = dayBegining;
                            }
                        }

                        if (startTime < endTime)
                        {
                            if (!isWorkAbsenceDay)
                            {
                                if (emplRules.ContainsKey(Constants.RuleOvertimeRounding))
                                    minPresenceRounding = emplRules[Constants.RuleOvertimeRounding].RuleValue;
                            }
                            else
                            {
                                if (emplRules.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                    minPresenceRounding = emplRules[Constants.RuleOvertimeRoundingOutWS].RuleValue;
                            }

                            int pairDuration = (int)endTime.Subtract(startTime).TotalMinutes;

                            if (endTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                pairDuration++;

                            if (pairDuration % minPresenceRounding != 0)
                            {
                                endTime = endTime.AddMinutes(-(endTime.Subtract(startTime).TotalMinutes % minPresenceRounding));
                                if (endTime < dayBegining)
                                    endTime = dayBegining;
                            }
                        }
                    }
                }
                
                IOPairProcessedTO pair = new IOPairProcessedTO();

                if (startTime < endTime)
                {
                    pair.EmployeeID = emplID;
                    pair.IOPairDate = start.Date;
                    pair.StartTime = startTime;
                    pair.EndTime = endTime;
                    pair.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                    pair.LocationID = Constants.locationOut;
                    pair.Alert = Constants.alertStatusNoAlert.ToString();
                    pair.ManualCreated = (int)Constants.recordCreated.Manualy;
                    pair.PassTypeID = ptID;
                    pair.IOPairID = Constants.unjustifiedIOPairID;
                    pair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                    pair.VerificationFlag = (int)Constants.Verification.Verified;
                }

                return pair;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.createPair(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool overlapPair(IOPairProcessedTO pair, List<IOPairProcessedTO> pairs)
        {
            try
            {
                bool ovarlap = false;
                foreach (IOPairProcessedTO pairTO in pairs)
                {
                    if (pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes > 0)
                    {
                        if ((pair.StartTime <= pairTO.StartTime && pair.EndTime > pairTO.StartTime) || (pair.StartTime > pairTO.StartTime && pair.StartTime < pairTO.EndTime))
                        {
                            ovarlap = true;
                            break;
                        }
                    }
                }

                return ovarlap;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.overlapPair(): " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool overlapInterval(IOPairProcessedTO pair, List<WorkTimeIntervalTO> intervals)
        {
            try
            {
                bool ovarlap = false;
                foreach (WorkTimeIntervalTO interval in intervals)
                {
                    if (interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes > 0)
                    {
                        if ((pair.StartTime.TimeOfDay <= interval.StartTime.TimeOfDay && pair.EndTime.TimeOfDay > interval.StartTime.TimeOfDay)
                            || (pair.StartTime.TimeOfDay > interval.StartTime.TimeOfDay && pair.StartTime.TimeOfDay < interval.EndTime.TimeOfDay))
                        {
                            ovarlap = true;
                            break;
                        }
                    }
                }

                return ovarlap;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.overlapInterval(): " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool changingExisting(int action)
        {
            try
            {
                return action == (int)Constants.MassiveInputActions.CHANGE_OVERTIME || action == (int)Constants.MassiveInputActions.CHANGE_REGULAR
                    || action == (int)Constants.MassiveInputActions.CHANGE_UNCONFIRMED;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.changingExisting(): " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private void chbIncludeAdditional_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chbClosure.Checked = chbLayOff.Checked = chbStoppage.Checked = chbPublicHoliday.Checked = false;

                chbClosure.Visible = chbLayOff.Visible = chbStoppage.Visible = chbPublicHoliday.Visible = chbIncludeAdditional.Checked;

                populateTypes(cbAction.SelectedIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.chbIncludeAdditional_CheckedChanged(): " + ex.StackTrace + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbClosure_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateTypes(cbAction.SelectedIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.chbClosure_CheckedChanged(): " + ex.StackTrace + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbLayOff_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateTypes(cbAction.SelectedIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.chbLayOff_CheckedChanged(): " + ex.StackTrace + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbStoppage_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateTypes(cbAction.SelectedIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.chbStoppage_CheckedChanged(): " + ex.StackTrace + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbPublicHoliday_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateTypes(cbAction.SelectedIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.chbPublicHoliday_CheckedChanged(): " + ex.StackTrace + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private int getType(int key, int company, int additionalType)
        {
            try
            {
                int ptID = -1;
                int val = -1;
                switch (key)
                {
                    case (int)Constants.ChangeRegularTypesAdditional.BLOOD_DONATION:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 11;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 94;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 95;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 97;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 96;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.DEATH_CLOSE_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 17;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 98;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 99;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 101;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 100;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.DEATH_MARITIAL_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 18;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 102;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 103;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 105;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 104;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.CHILD_BEARING_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 19;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 106;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 107;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 109;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 108;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.CHILD_BEARING:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 20;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 110;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 111;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 113;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 112;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.WEDDING:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 21;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 114;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 115;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 117;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 116;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.SUSPENSION_3:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 23;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 118;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 119;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 121;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 120;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.DRILL_SUMMONS:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 24;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 122;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 123;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 125;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 124;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_START:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 25;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 126;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 127;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 129;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 128;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.INDUSTRIAL_INJURY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 26;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 130;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 131;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 133;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 132;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_EXC:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 27;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 134;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 135;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 137;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 136;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.MATERNITY_LEAVE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 28;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 138;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 139;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 141;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 140;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.JUSTIFIED_ABSENCE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 13;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 142;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 143;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 145;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 144;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.UNJUSTIFIED_ABSENCE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 14;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 146;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 147;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 149;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 148;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.REPAIR_OF_DEMAGE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 33;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 158;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 159;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 161;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 160;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.EXAMINATION:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 34;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 162;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 163;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 165;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 164;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.DISEASE_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 35;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 166;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 167;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 169;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 168;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.SUSPENSION_4:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 38;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 170;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 171;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 173;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 172;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.INDUSTRIAL_INJURY_CONT:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 40;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 178;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 179;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 181;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 180;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_CONT:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 39;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 174;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 175;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 177;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 176;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.CARE_OF_CHILD:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 62;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 182;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 183;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 185;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 184;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.UNPAID_LEAVE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 80;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 186;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 187;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 189;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 188;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.PREGNANCY_LEAVE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 43;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 190;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 191;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 193;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 192;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.TISSUE_DONATION:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 92;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 295;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 296;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 298;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 297;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.PERSONAL_LEAVE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 54;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 194;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 195;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 197;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 196;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.DEATH_GRANDPARENTS:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 307;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 311;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 312;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 314;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 313;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.MOVING_EMPLOYEE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 309;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 319;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 320;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 322;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 321;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.DEATH_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 55;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 198;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 199;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 201;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 200;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.ILLNESS_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 81;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 202;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 203;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 205;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 204;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.WEDDING_CHILDREN:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 79;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 206;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 207;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 209;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 208;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.RECREATIONAL_HOLIDAY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 84;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 210;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 211;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 213;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 212;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.DISCIPLINARY_SUSPENSION:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 53;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 214;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 215;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 217;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 216;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE_NO_PAYMENT:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 91;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 299;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 300;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 302;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 301;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.CARE_OF_CHILD_3:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 63;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 218;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 219;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 221;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 220;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.PREGNENCY_LEAVE_30:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 90;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 303;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 304;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 306;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 305;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.STRIKE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 65;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 222;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 223;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 225;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 224;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 73;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 226;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 227;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 229;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 228;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_CHILD_BEARING_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 69;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 230;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 231;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 233;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 232;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_CHILD_BEARING:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 70;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 234;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 235;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 237;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 236;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_DEATH_CLOSE_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 67;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 238;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 239;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 241;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 240;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_DEATH_MARITIAL_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 68;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 242;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 243;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 245;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 244;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_DRILL_SUMMONS:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 72;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 246;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 247;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 249;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 248;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_ILLNESS_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 82;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 250;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 251;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 253;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 252;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_UNPAID_LEAVE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 41;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 254;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 255;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 257;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 256;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_MOVING_EMPLOYEE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 310;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 323;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 324;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 326;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 325;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_RECREATIONAL_HOLIDAY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 57;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 266;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 267;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 269;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 268;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_REPAIR_OF_DEMAGE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 76;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 270;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 271;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 273;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 272;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_DISEASE_FAMILY:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 78;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 274;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 275;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 277;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 276;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_EXAMINATION:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 77;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 278;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 279;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 281;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 280;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_WEDDING_CHILDREN:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 56;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 282;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 283;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 285;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 284;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_WEDDING:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 71;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 286;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 287;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 289;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 288;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_BLOOD_DONATION:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 66;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 290;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 291;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 293;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 292;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.NCF_DEATH_GRANDPARENTS:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = 308;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 315;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 316;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 318;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 317;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)Constants.ChangeRegularTypesAdditional.ABSENCE:
                        {
                            switch (company)
                            {
                                case (int)Constants.FiatCompanies.FAS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = -100;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = 328;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = 329;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = 331;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = 330;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.FS:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMauto:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                                case (int)Constants.FiatCompanies.MMdoo:
                                    {
                                        switch (additionalType)
                                        {
                                            case (int)Constants.AdditionalTypes.NONE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.CLOSURE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.LAYOFF:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.STOPPAGE:
                                                ptID = val;
                                                break;
                                            case (int)Constants.AdditionalTypes.PUBLIC_HOLIDAY:
                                                ptID = val;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }

                return ptID;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AbsenceMassiveInput.getType(): " + ex.StackTrace + "\n");
                throw ex;
            }
        }
    }
}
