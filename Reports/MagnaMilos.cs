using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;
using Util;
using TransferObjects;
using System.Globalization;
using System.Resources;
using Common;
using System.Collections;

namespace Reports.Magna
{
    public partial class MagnaMilos : Form
    {
        List<EmployeeTO> employeeList;
        Dictionary<int, List<IOPairTO>> emplPairs;
        List<IOPairTO> pairs;
        List<OrganizationalUnitTO> ouArray;
        List<WorkingUnitTO> wuArray;

        // konstante Magna
        //private const int redovanRad = 0; // dok se ne promeni u passTypes na 1
        private const int redovanRad = 1;
        private const int prekovremeniRad = 4;
        private const int nocniRad = 6;
        private const int radNaDrzavniPraznik = 5;
        private const int bolovanjeDo30Dana = 101;
        private const int bolovanjePreko30Dana = 102;
        private const int povredaNaRadu = 104;
        private const int porodiljskoBolovanje = 103;
        private const int godisnjiOdmor = 8;
        private const int sluzbeniPut = 2;
        private const int slava = 10;

        DebugLog log;
        ApplUserTO logInUser;
        private CultureInfo culture;
        private ResourceManager rm;

        // Excel
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;

        private ListViewItemComparer _comp;
        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        // zaposleni
        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();

        Dictionary<string, DateTime> dateOfTheDay;
        DateTime mondayDate = new DateTime();
        DateTime from = new DateTime();
        DateTime to = new DateTime();

        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;

        private int emplSortField;

        //oUnits are organizational units
        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();
        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, OrganizationalUnitTO> ouDict = new Dictionary<int, OrganizationalUnitTO>();
        Dictionary<int, PassTypeTO> ptDict = new Dictionary<int, PassTypeTO>();
        Dictionary<int, Dictionary<int, string>> emplTypesDict = new Dictionary<int, Dictionary<int, string>>();
        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();
        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();


        #region Inner Class for sorting List of Employees
        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer : System.Collections.IComparer
        {
            private ListView _listView;

            public ListViewItemComparer(ListView lv)
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
                    case MagnaMilos.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case MagnaMilos.emplNameIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion
        



        public MagnaMilos()
        {
            try
            {
                InitializeComponent();

                // Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                // Set Language
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(MagnaMilos).Assembly);
                rbWU.Checked = true;

                setLanguage();
                dateOfTheDay = new Dictionary<string, DateTime>();
                setDates();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void MagnaMilos_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                emplSortField = emplNameIndex;

                // Initialize comparer objects
                _comp = new ListViewItemComparer(lvEmployees);
                _comp.SortColumn = emplSortField;
                lvEmployees.ListViewItemSorter = _comp;
                lvEmployees.Sorting = SortOrder.Ascending;

                wuDict = new WorkingUnit().getWUDictionary();
                ouDict = new OrganizationalUnit().SearchDictionary();
                ptDict = new PassType().SearchDictionaryCodeSorted();
                schemas = new TimeSchema().getDictionary();
                rules = new Common.Rule().SearchWUEmplTypeDictionary();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.ReportPurpose);
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

                populateWU();
                populateOU();

                rbWU.Checked = true;
                rbWU_CheckedChanged(this, new EventArgs());

                // get all paid leave
                Dictionary<int, List<int>> confirmationPTDict = new PassTypesConfirmation().SearchDictionary();
                List<int> sickLeaveNCF = new Common.Rule().SearchRulesExact(Constants.RuleCompanySickLeaveNCF);

                //populateDateListView();

                dtpFor.Value = DateTime.Today;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MagnaMilos.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }


        private void setDates()
        {
            try
            {
                DateTime now = DateTime.Now;

                switch (now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        mondayDate = now.AddDays(7);
                        break;
                    case DayOfWeek.Tuesday:
                        mondayDate = now.AddDays(6);
                        break;
                    case DayOfWeek.Wednesday:
                        mondayDate = now.AddDays(5);
                        break;
                    case DayOfWeek.Thursday:
                        mondayDate = now.AddDays(4);
                        break;
                    case DayOfWeek.Friday:
                        mondayDate = now.AddDays(3);
                        break;
                    case DayOfWeek.Saturday:
                        mondayDate = now.AddDays(2);
                        break;
                    case DayOfWeek.Sunday:
                        mondayDate = now.AddDays(1);
                        break;
                }

                DateTime date = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day, 0, 0, 0);
                dateOfTheDay.Add(DayOfWeek.Monday.ToString(), date);
                dateOfTheDay.Add(DayOfWeek.Tuesday.ToString(), date.AddDays(1));
                dateOfTheDay.Add(DayOfWeek.Wednesday.ToString(), date.AddDays(2));
                dateOfTheDay.Add(DayOfWeek.Thursday.ToString(), date.AddDays(3));
                dateOfTheDay.Add(DayOfWeek.Friday.ToString(), date.AddDays(4));
                dateOfTheDay.Add(DayOfWeek.Saturday.ToString(), date.AddDays(5));
                dateOfTheDay.Add(DayOfWeek.Sunday.ToString(), date.AddDays(6));

                from = date;
                to = date.AddDays(6);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.setDates(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void setLanguage()
        {
            //this.Text = rm.GetString("MagnaMonthReport", culture);

            //label's text
            //this.lblChoseMonth.Text = rm.GetString("lblChoseMonth", culture);
            this.lblEmployee.Text = rm.GetString("lblEmployee", culture);

            this.gbFor.Text = rm.GetString("date", culture);

            //button's text
            this.btnClose.Text = rm.GetString("btnClose", culture);
            this.btnGenerate.Text = rm.GetString("btnGenerate", culture);

            /// list view                
            lvEmployees.BeginUpdate();
            lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 80, HorizontalAlignment.Left);
            lvEmployees.Columns.Add(rm.GetString("hdrGUID", culture), 80, HorizontalAlignment.Left);
            lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 180, HorizontalAlignment.Left);
            lvEmployees.EndUpdate();
        }

        private void populateWU()
        {
            try
            {
                wuArray = new List<WorkingUnitTO>();

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOU()
        {
            try
            {
                ouArray = new List<OrganizationalUnitTO>();

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }


        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = rbWU.Checked;

                cbOU.Enabled = !rbWU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MagnaMilos.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = !rbOU.Checked;

                cbOU.Enabled = rbOU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MagnaMilos.rbOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                employeeList = new List<EmployeeTO>();

                bool isWU = rbWU.Checked;

                int ID = -1;
                if (isWU && cbWU.SelectedIndex > 0)
                    ID = (int)cbWU.SelectedValue;
                else if (!isWU && cbOU.SelectedIndex > 0)
                    ID = (int)cbOU.SelectedValue;

                DateTime a = dtpFor.Value.Date;

                if (from > to)
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                else if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                    }
                    else
                    {
                        string ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);

                        employeeList = new Employee().SearchByOU(ounits, -1, null, from.Date, to.Date);
                    }
                }

                ascoDict = new EmployeeAsco4().SearchDictionary("");

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();
                foreach (EmployeeTO empl in employeeList)
                {

                    ListViewItem item = new ListViewItem();
                    item.Text = empl.EmployeeID.ToString().Trim();
                    if (ascoDict.ContainsKey(empl.EmployeeID) && ascoDict[empl.EmployeeID].IntegerValue1 != -1)
                    {
                        EmployeeAsco4TO eAsco = ascoDict[empl.EmployeeID];
                        item.SubItems.Add(eAsco.IntegerValue1.ToString().Trim());
                    }
                    else
                    {
                        item.SubItems.Add(" ");
                    }
                    item.SubItems.Add(empl.FirstAndLastName);
                    item.ToolTipText = empl.FirstAndLastName;

                    item.Tag = empl;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.populateEmployees(): " + ex.Message + "\n");
                throw ex;
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
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MagnaMIlos.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MagnaMilos.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void tbEmployee_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string emplName = tbEmployee.Text.Trim().ToUpper();
                int emplID = -1;
                if (!int.TryParse(emplName, out emplID))
                    emplID = -1;

                lvEmployees.SelectedItems.Clear();

                if (emplName.Trim().Equals(""))
                {
                    tbEmployee.Focus();
                    return;
                }

                foreach (ListViewItem item in lvEmployees.Items)
                {
                    if ((emplID != -1 && ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim().ToUpper().StartsWith(emplID.ToString().Trim().ToUpper()))
                        || (emplID == -1 && ((EmployeeTO)item.Tag).FirstAndLastName.Trim().ToUpper().StartsWith(emplName.Trim().ToUpper())))
                    {
                        item.Selected = true;
                        lvEmployees.Select();
                        lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));
                        lvEmployees.Invalidate();
                        tbEmployee.Focus();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatYTimeAndAtendance.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }



        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvEmployees.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Niste odabrali ni jednog zaposlenog!", "Greska!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult dg = MessageBox.Show("Ovo moze potrajati, zelite li da nastavite?", "Izvestaj Magna", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                switch (dg)
                {
                    case DialogResult.Yes:

                        xlApp = new Excel.Application();
                        if (xlApp == null)
                        {
                            MessageBox.Show("Excel is not properly installed!!");
                            return;
                        }
                        object misValue = System.Reflection.Missing.Value;
                        xlWorkBook = xlApp.Workbooks.Add(misValue);


                        // ----------------------------------------------------------------------------
                        #region  get selected employees, and create dictionary (EmployeeID, EmployeeTO
                        if (lvEmployees.SelectedItems.Count > 0)
                        {
                            // ----------------------------------------------------------------------------
                            #region mesec
                            DateTime date = dtpFor.Value.Date;
                            string month = "";
                            switch (date.Month)
                            {
                                case 1:
                                    month = "Januar";
                                    break;
                                case 2:
                                    month = "Februar";
                                    break;
                                case 3:
                                    month = "Mart";
                                    break;
                                case 4:
                                    month = "April";
                                    break;
                                case 5:
                                    month = "Maj";
                                    break;
                                case 6:
                                    month = "Jun";
                                    break;
                                case 7:
                                    month = "Jul";
                                    break;
                                case 8:
                                    month = "Avgust";
                                    break;
                                case 9:
                                    month = "Septembar";
                                    break;
                                case 10:
                                    month = "Oktobar";
                                    break;
                                case 11:
                                    month = "Novembar";
                                    break;
                                case 12:
                                    month = "Decembar";
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                            // ebd region mesec

                            string emplIDs = "";
                            int emplID = 0;
                            Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();

                            EmployeeTO emplTO = new EmployeeTO();
                            EmployeeAsco4TO eAsco = new EmployeeAsco4TO();
                            int brojSelektovanih = 0;

                            #region nepotrebno
                            //// populate pass_types
                            //List<PassTypeTO> passTypesAll = new PassType().Search();
                            //List<PassTypeTO> passTypes = new List<PassTypeTO>();

                            //foreach (PassTypeTO pt in passTypesAll)
                            //{
                            //    //if (pt.PassTypeID == Constants.regularWork || pt.IsPass != Constants.passOnReader)
                            //    if (pt.PassTypeID == redovanRad || pt.IsPass != Constants.passOnReader)
                            //    {
                            //        passTypes.Add(pt);
                            //    }
                            //} 
                            #endregion

                            // selected empolyees
                            foreach (ListViewItem item in lvEmployees.SelectedItems)
                            {
                                emplIDs = ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim();

                                if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                                    emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);

                                brojSelektovanih++;

                                // nadji maticni broj
                                emplID = ((EmployeeTO)item.Tag).EmployeeID;
                                if (ascoDict.ContainsKey(emplID))
                                {
                                    eAsco = ascoDict[emplID];
                                    //eAsco.NVarcharValue3 - maticni broj
                                }

                                // odabrani zaposleni
                                foreach (EmployeeTO emTO in employeeList)
                                {
                                    if (emTO.EmployeeID == Convert.ToInt32(emplIDs))
                                    {
                                        emplTO = emTO;
                                        break;
                                    }
                                }

                                int wuID = (int)cbWU.SelectedValue;
                                string wUnitsString = wuID.ToString().Trim();

                                // naziv radne jedinice zaposlenog
                                foreach (WorkingUnitTO wuTO in wuArray)
                                {
                                    if (wuTO.WorkingUnitID == wuID)
                                    {
                                        emplTO.WorkingUnitName = wuTO.Name;
                                        break;
                                    }
                                }

                                // get IOPairs za radnu jedinicu
                                List<IOPairTO> ioPairs = prepareData(wuID, wUnitsString);

                                //emplPairs = new Dictionary<int, List<IOPairTO>>(); // Key: Day, Value:IOPair List 
                                pairs = new List<IOPairTO>();

                                foreach (IOPairTO pair in ioPairs)
                                {
                                    //if (pair.EmployeeID.Equals(emplTO.EmployeeID))
                                        if (pair.EmployeeID.Equals(emplTO.EmployeeID) && (pair.PassTypeID == redovanRad || pair.PassTypeID == nocniRad || pair.PassTypeID == prekovremeniRad ||
                                        pair.PassTypeID == radNaDrzavniPraznik || pair.PassTypeID == bolovanjeDo30Dana || pair.PassTypeID == bolovanjePreko30Dana || pair.PassTypeID == povredaNaRadu ||
                                        pair.PassTypeID == porodiljskoBolovanje || pair.PassTypeID == godisnjiOdmor || pair.PassTypeID == sluzbeniPut || pair.PassTypeID == slava))
                                    {
                                        #region nepotrebno
                                        //if (emplPairs.ContainsKey(pair.IOPairDate.Day))
                                        //{
                                        //    List<IOPairTO> existing = emplPairs[pair.IOPairDate.Day];
                                        //    existing.Add(pair);
                                        //}
                                        //else
                                        //{
                                        //List<IOPairTO> newDay = new List<IOPairTO>();
                                        //newDay.Add(pair);
                                        //emplPairs.Add(pair.IOPairDate.Day, newDay);
                                        //} 
                                        #endregion
                                        // lista sa prolascima za odabranog zaposlenog za odabrani mesec
                                        pairs.Add(pair);
                                    }
                                }






                                // ----------------------------------------------------------------------------
                                #region formatiranje i pozicioniranje u excel-u

                                if (brojSelektovanih > 3) // ukoliko su selektovana vise od 3 radnika, potrebno je da se doda novi sheet u excel-u
                                {
                                    xlWorkBook.Sheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                                }
                                //izabrani Sheet
                                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets["Sheet" + brojSelektovanih];
                                xlWorkSheet.Name = emplTO.FirstAndLastName;

                                // Setup Sheet
                                var _pageSetup = xlWorkSheet.PageSetup;
                                _pageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4; // stranica A4 formata
                                _pageSetup.Orientation = Excel.XlPageOrientation.xlLandscape; // landscape orientation
                                // Fit Sheet on One Page 
                                _pageSetup.FitToPagesWide = 1;
                                _pageSetup.FitToPagesTall = 1;

                                // nece da radi metoda InchesToPoint
                                //// Set Margins
                                //_pageSetup.LeftMargin = Excel.InchesToPoints(0.5);
                                //_pageSetup.RightMargin = Excel.InchesToPoints(0.5);
                                //_pageSetup.TopMargin = Excel.InchesToPoints(0.75);
                                //_pageSetup.BottomMargin = Excel.InchesToPoints(0.75);
                                //_pageSetup.HeaderMargin = Excel.InchesToPoints(0.3);
                                //_pageSetup.FooterMargin = Excel.InchesToPoints(0.3);

                                // Set Margins
                                _pageSetup.LeftMargin = 21.6;
                                _pageSetup.RightMargin = 21.6;
                                _pageSetup.TopMargin = 54;
                                _pageSetup.BottomMargin = 54;
                                _pageSetup.HeaderMargin = 21.6;
                                _pageSetup.FooterMargin = 21.6;






                                // ----------------------------------------------------------------------------
                                #region Header

                                xlWorkSheet.Cells[1, 7] = "RADNA LISTA";
                                Excel.Range range = xlWorkSheet.get_Range(xlWorkSheet.Cells[1, 7], xlWorkSheet.Cells[1, 31]);
                                range.Merge(true);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.Font.Bold = true;

                                xlWorkSheet.Cells[2, 7] = "za mesec " + month + " " + date.Year + "";
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[2, 7], xlWorkSheet.Cells[2, 31]);
                                range.Merge(true);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                                xlWorkSheet.Cells[3, 1] = "LOGO";
                                xlWorkSheet.Cells[4, 1] = "IME I PREZIME:";
                                //xlWorkSheet.Cells[4, 2] = "_______________________________________________________";
                                xlWorkSheet.Cells[4, 2] = emplTO.FirstAndLastName;
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[4, 2], xlWorkSheet.Cells[4, 16]);
                                range.Merge(true);

                                xlWorkSheet.Cells[4, 23] = "Mat. broj:";
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[4, 23], xlWorkSheet.Cells[4, 25]);
                                range.Merge(true);
                                //xlWorkSheet.Cells[4, 26] = "_____________";
                                xlWorkSheet.Cells[4, 26] = eAsco.NVarcharValue3; // maticni broj zaposlenog
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[4, 26], xlWorkSheet.Cells[4, 29]);
                                range.Merge(true);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                range.NumberFormat = "0";

                                xlWorkSheet.Cells[5, 1] = "Radno mesto";
                                if (rbWU.Checked)
                                {
                                    //WorkingUnitTO selectedWU = cbWU.SelectedItem as WorkingUnitTO;
                                    //xlWorkSheet.Cells[5, 2] = selectedWU.Name;
                                    xlWorkSheet.Cells[5, 2] = emplTO.WorkingUnitName;
                                }
                                else // rbOU.Checked
                                {
                                    //OrganizationalUnitTO selectedOU = cbOU.SelectedItem as OrganizationalUnitTO;
                                    //xlWorkSheet.Cells[5, 2] = selectedOU.Name;
                                    foreach (OrganizationalUnitTO ouTO in ouArray)
                                    {
                                        if (ouTO.OrgUnitID == emplTO.OrgUnitID)
                                        {
                                            xlWorkSheet.Cells[5, 2] = "Sifra organizacione jedinice" + ouTO.Name;
                                            break;
                                        }
                                    }
                                }
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[5, 2], xlWorkSheet.Cells[5, 16]);
                                range.Merge(true);

                                #endregion
                                // end region header






                                // ----------------------------------------------------------------------------
                                #region table
                                xlWorkSheet.Cells[7, 1] = "OPIS";

                                xlWorkSheet.Cells[8, 1] = "Redovan rad";
                                xlWorkSheet.Cells[9, 1] = "Prekovremeni rad";
                                xlWorkSheet.Cells[10, 1] = "Nocni rad";
                                xlWorkSheet.Cells[11, 1] = "Rad na drzavni praznik";
                                xlWorkSheet.Cells[12, 1] = "Bolovanje do 30 dana";
                                xlWorkSheet.Cells[13, 1] = "Bolovanje preko 30 dana";
                                xlWorkSheet.Cells[14, 1] = "Povreda na radu";
                                xlWorkSheet.Cells[15, 1] = "Porodiljsko bolovanje";
                                xlWorkSheet.Cells[16, 1] = "Godisnji odmor";
                                xlWorkSheet.Cells[17, 1] = "Sluzbeni put";
                                xlWorkSheet.Cells[18, 1] = "Ostalo";

                                //First column width
                                Excel.Range kolona1 = xlWorkSheet.get_Range("A:A", Type.Missing);
                                kolona1.EntireColumn.ColumnWidth = 24;

                                // ----------------------------------------------------------------------------
                                #region dani
                                //oznaci vikend kolone - ofarbaj u sivo
                                DateTime from = new DateTime(dtpFor.Value.Year, dtpFor.Value.Month, 1);
                                DateTime to = from.AddMonths(1);
                                int column = 2;
                                int dan = 1;

                                #region prekopirano iz PresenceTracking
                                bool is2DayShift = false;
                                bool is2DaysShiftPrevious = false;
                                WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int, WorkTimeIntervalTO>();
                                #endregion

                                while (from < to)
                                {
                                    xlWorkSheet.Cells[7, column] = dan.ToString();
                                    if (from.DayOfWeek.ToString() == "Saturday" || from.DayOfWeek.ToString() == "Sunday")
                                    {
                                        range = xlWorkSheet.get_Range(xlWorkSheet.Cells[8, column], xlWorkSheet.Cells[18, column]);
                                        range.Interior.Color = ColorTranslator.ToOle(Color.Gray); // za subotu i nedelju oboj kolone
                                    }

                                    // prolasci za izabrani dan (from)
                                    List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(pairs, from, is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);

                                    //bool containRegularWork = false;
                                    TimeSpan tsRegularWork = new TimeSpan();
                                    TimeSpan tsOverTimeWork = new TimeSpan();
                                    TimeSpan tsNightShift = new TimeSpan();
                                    TimeSpan tsWorkOnHoliday = new TimeSpan();
                                    TimeSpan tsSickLeaveUntil30days = new TimeSpan();
                                    TimeSpan tsSickLeaveAfter30days = new TimeSpan();
                                    TimeSpan tsInjuryAtWork = new TimeSpan();
                                    TimeSpan tsNursery = new TimeSpan();
                                    TimeSpan tsAnnualLeave = new TimeSpan();
                                    TimeSpan tsOfficialTrip = new TimeSpan();
                                    TimeSpan tsRest = new TimeSpan();


                                    foreach (IOPairTO interval in dayIOPairList)
                                    {
                                        //if (this.isPass(interval.PassTypeID) && interval.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                        //{
                                        //    containRegularWork = true;
                                        //    tsRegularWork += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                        //}

                                        #region switch
                                        //u zavisnosti od tipa prolaska popuni odgovarajuce polje u tabeli
                                        switch (interval.PassTypeID)
                                        {
                                            case redovanRad:
                                                #region redovan rad
                                                if (this.isPass(interval.PassTypeID) && interval.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                                {
                                                    //containRegularWork = true;
                                                    tsRegularWork = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                                    string hours = tsRegularWork.Hours.ToString();
                                                    if (tsRegularWork.Hours < 10)
                                                        hours = "0" + hours;
                                                    string minutes = tsRegularWork.Minutes.ToString();
                                                    if (tsRegularWork.Minutes < 10)
                                                        minutes = "0" + minutes;
                                                    xlWorkSheet.Cells[8, column] = "'" + hours + ":" + minutes;
                                                }
                                                #endregion
                                                break;
                                            case prekovremeniRad:
                                                #region prekovremeni rad
                                                if (this.isPass(interval.PassTypeID) && interval.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                                {
                                                    tsOverTimeWork = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                                    string hours = tsOverTimeWork.Hours.ToString();
                                                    if (tsOverTimeWork.Hours < 10)
                                                        hours = "0" + hours;
                                                    string minutes = tsOverTimeWork.Minutes.ToString();
                                                    if (tsOverTimeWork.Minutes < 10)
                                                        minutes = "0" + minutes;
                                                    xlWorkSheet.Cells[9, column] = "'" + hours + ":" + minutes;
                                                }
                                                #endregion
                                                break;
                                            case nocniRad:
                                                // case nocniRad - prekopirano iz PresenceTracking
                                                #region nocni rad
                                                if (this.isPass(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                                {
                                                    TimeSpan EndHelp = new TimeSpan(07, 30, 0);
                                                    TimeSpan startOnly = new TimeSpan(22, 0, 0);
                                                    TimeSpan endOnly = new TimeSpan(23, 59, 59);
                                                    TimeSpan startOnly1 = new TimeSpan(0, 0, 0);
                                                    TimeSpan endOnly1 = new TimeSpan(6, 0, 0);

                                                    if (interval.EndTime.TimeOfDay >= startOnly)
                                                    {
                                                        TimeSpan now = new TimeSpan(22, 0, 0);
                                                        if (interval.StartTime.TimeOfDay >= now)
                                                        {
                                                            tsNightShift += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                                        }
                                                        else
                                                        {
                                                            tsNightShift += (interval.EndTime.TimeOfDay - now);
                                                        }
                                                    }

                                                    else if (interval.EndTime.TimeOfDay <= EndHelp)
                                                    {
                                                        tsNightShift += (interval.EndTime.TimeOfDay - (interval.EndTime.TimeOfDay - endOnly1));
                                                    }

                                                    else if (interval.StartTime.TimeOfDay <= endOnly1)
                                                    {
                                                        TimeSpan now = new TimeSpan(0, 0, 0);
                                                        TimeSpan after = new TimeSpan(6, 0, 0);
                                                        if (interval.EndTime.TimeOfDay <= after)
                                                        {
                                                            tsNightShift += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                                        }
                                                        else
                                                        {
                                                            tsNightShift += (interval.EndTime.TimeOfDay - (interval.EndTime.TimeOfDay - after));
                                                        }
                                                    }

                                                    else if (interval.StartTime.TimeOfDay >= startOnly)
                                                    {
                                                        TimeSpan after = new TimeSpan(23, 59, 59);
                                                        tsNightShift += (after - interval.StartTime.TimeOfDay);
                                                    }

                                                    string hours = tsNightShift.Hours.ToString();
                                                    if (tsNightShift.Hours < 10)
                                                        hours = "0" + hours;
                                                    string minutes = tsNightShift.Minutes.ToString();
                                                    if (tsNightShift.Minutes < 10)
                                                        minutes = "0" + minutes;
                                                    xlWorkSheet.Cells[10, column] = "'" + hours + ":" + minutes;
                                                }
                                                #endregion
                                                break;
                                            case radNaDrzavniPraznik:
                                                #region rad na drzavni praznik
                                                if (this.isPass(interval.PassTypeID) && interval.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                                {
                                                    tsWorkOnHoliday = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                                    string hours = tsWorkOnHoliday.Hours.ToString();
                                                    if (tsWorkOnHoliday.Hours < 10)
                                                        hours = "0" + hours;
                                                    string minutes = tsWorkOnHoliday.Minutes.ToString();
                                                    if (tsWorkOnHoliday.Minutes < 10)
                                                        minutes = "0" + minutes;
                                                    xlWorkSheet.Cells[11, column] = "'" + hours + ":" + minutes;
                                                }
                                                #endregion
                                                break;
                                            case bolovanjeDo30Dana:
                                           //     if (this.isPass(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                                //     { 

                                                tsSickLeaveUntil30days = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                                string hours1 = tsSickLeaveUntil30days.Hours.ToString();
                                                if (tsSickLeaveUntil30days.Hours < 10)
                                                    hours1 = "0" + hours1;
                                                string minutes1 = tsSickLeaveUntil30days.Minutes.ToString();
                                                if (tsSickLeaveUntil30days.Minutes < 10)
                                                    minutes1 = "0" + minutes1;
                                                xlWorkSheet.Cells[12, column] = "'" + hours1 + ":" + minutes1;
                                                    range = xlWorkSheet.get_Range(xlWorkSheet.Cells[12, column], xlWorkSheet.Cells[12, column]);
                                                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                                           //     }
                                                break;
                                            case bolovanjePreko30Dana:
                                              //  if (this.isPass(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                           //     {
                                                tsSickLeaveAfter30days = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                                string hours2 = tsSickLeaveAfter30days.Hours.ToString();
                                                if (tsSickLeaveAfter30days.Hours < 10)
                                                    hours2 = "0" + hours2;
                                                string minutes2 = tsSickLeaveAfter30days.Minutes.ToString();
                                                if (tsSickLeaveAfter30days.Minutes < 10)
                                                    minutes2 = "0" + minutes2;
                                                xlWorkSheet.Cells[13, column] = "'" + hours2 + ":" + minutes2;
                                                    range = xlWorkSheet.get_Range(xlWorkSheet.Cells[13, column], xlWorkSheet.Cells[13, column]);
                                                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                                          //      }
                                                break;
                                            case povredaNaRadu:
                                            //    if (this.isPass(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                            //    {

                                                tsInjuryAtWork = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                                string hours3 = tsInjuryAtWork.Hours.ToString();
                                                if (tsInjuryAtWork.Hours < 10)
                                                    hours3 = "0" + hours3;
                                                string minutes3 = tsInjuryAtWork.Minutes.ToString();
                                                if (tsInjuryAtWork.Minutes < 10)
                                                    minutes3 = "0" + minutes3;
                                                xlWorkSheet.Cells[14, column] = "'" + hours3 + ":" + minutes3;
                                                    range = xlWorkSheet.get_Range(xlWorkSheet.Cells[14, column], xlWorkSheet.Cells[14, column]);
                                                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                                            //    }
                                                break;
                                            case porodiljskoBolovanje:
                                           //     if (this.isPass(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                             //   {

                                                tsNursery = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                                string hours4 = tsNursery.Hours.ToString();
                                                if (tsNursery.Hours < 10)
                                                    hours4 = "0" + hours4;
                                                string minutes4 = tsNursery.Minutes.ToString();
                                                if (tsNursery.Minutes < 10)
                                                    minutes4 = "0" + minutes4;
                                                xlWorkSheet.Cells[15, column] = "'" + hours4 + ":" + minutes4;
                                                    range = xlWorkSheet.get_Range(xlWorkSheet.Cells[15, column], xlWorkSheet.Cells[15, column]);
                                                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                                            //    }
                                                break;
                                            case godisnjiOdmor:
                                              //  if (this.isPass(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                              //  {
                                               // if (this.isPass(interval.PassTypeID) && interval.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                            //    {
                                                    //containRegularWork = true;
                                                    tsAnnualLeave = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                                    string hours5 = tsAnnualLeave.Hours.ToString();
                                                    if (tsAnnualLeave.Hours < 10)
                                                        hours5 = "0" + hours5;
                                                    string minutes5 = tsAnnualLeave.Minutes.ToString();
                                                    if (tsAnnualLeave.Minutes < 10)
                                                        minutes5 = "0" + minutes5;
                                                    xlWorkSheet.Cells[16, column] = "'" + hours5 + ":" + minutes5;
                                            //    }
                                              
                                                    range = xlWorkSheet.get_Range(xlWorkSheet.Cells[16, column], xlWorkSheet.Cells[16, column]);
                                                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                                        //        }
                                                break;
                                            case sluzbeniPut:
                                         //       if (this.isPass(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                       //         {
                                                tsOfficialTrip = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                                string hours6 = tsOfficialTrip.Hours.ToString();
                                                if (tsOfficialTrip.Hours < 10)
                                                    hours6 = "0" + hours6;
                                                string minutes6 = tsOfficialTrip.Minutes.ToString();
                                                if (tsOfficialTrip.Minutes < 10)
                                                    minutes6 = "0" + minutes6;
                                                xlWorkSheet.Cells[17, column] = "'" + hours6 + ":" + minutes6;
                                                    range = xlWorkSheet.get_Range(xlWorkSheet.Cells[17, column], xlWorkSheet.Cells[17, column]);
                                                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                                        //        }
                                                break;
                                            case slava:
                                        //        if (this.isPass(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                            //  {
                                                tsRest = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                                string hours7 = tsRest.Hours.ToString();
                                                if (tsRest.Hours < 10)
                                                    hours7 = "0" + hours7;
                                                string minutes7 = tsRest.Minutes.ToString();
                                                if (tsRest.Minutes < 10)
                                                    minutes7 = "0" + minutes7;
                                                xlWorkSheet.Cells[18, column] = "'" + hours7 + ":" + minutes7;
                                                    range = xlWorkSheet.get_Range(xlWorkSheet.Cells[18, column], xlWorkSheet.Cells[18, column]);
                                                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                                              //  }
                                                break;

                                            default:
                                                break;
                                        }
                                        #endregion
                                    }


                                    //xlWorkSheet.Cells[7, column] = dan.ToString();
                                    //if (from.DayOfWeek.ToString() == "Saturday" || from.DayOfWeek.ToString() == "Sunday")
                                    //{
                                    //    range = xlWorkSheet.get_Range(xlWorkSheet.Cells[8, column], xlWorkSheet.Cells[18, column]);
                                    //    range.Interior.Color = ColorTranslator.ToOle(Color.Gray); // za subotu i nedelju oboj kolone
                                    //    //range.Interior.Color = Excel.XlRgbColor.rgbAliceBlue;
                                    //    //XlRgbColor.rgbBlack;
                                    //}
                                    from = from.AddDays(1); // predji na sledecci dan
                                    dan++;
                                    column++;

                                }
                                #endregion
                                // end region dani

                                //border
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[7, 1], xlWorkSheet.Cells[18, dan]);
                                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // kreiranje okvira
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[7, 2], xlWorkSheet.Cells[18, dan]);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; // centralno poravnje
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[8, 2], xlWorkSheet.Cells[18, dan]);
                                range.Font.Size = 8; // font 8 za sate u tabeli

                                //Columns days width
                                Excel.Range dani = xlWorkSheet.get_Range("B:AI", Type.Missing);
                                dani.EntireColumn.ColumnWidth = 3;

                                #endregion
                                // end region table




                                // ----------------------------------------------------------------------------
                                #region footer
                                xlWorkSheet.Cells[21, 1] = "Zaposleni";
                                xlWorkSheet.Cells[21, 7] = "Pretpostavljeni";
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[21, 7], xlWorkSheet.Cells[21, 11]);
                                range.Merge(true);

                                xlWorkSheet.Cells[21, 21] = "Direktor proizvodnje";
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[21, 21], xlWorkSheet.Cells[21, 25]);
                                range.Merge(true);

                                xlWorkSheet.Cells[23, 1] = "________________________";
                                xlWorkSheet.Cells[23, 7] = "_____________________________";
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[23, 7], xlWorkSheet.Cells[23, 14]);
                                range.Merge(true);

                                xlWorkSheet.Cells[23, 21] = "_________________________________";
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[23, 21], xlWorkSheet.Cells[23, 29]);
                                range.Merge(true);


                                xlWorkSheet.Cells[25, 21] = "Direktor ljudskih resursa";
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[25, 21], xlWorkSheet.Cells[25, 27]);
                                range.Merge(true);

                                xlWorkSheet.Cells[27, 21] = "_________________________________";
                                range = xlWorkSheet.get_Range(xlWorkSheet.Cells[27, 21], xlWorkSheet.Cells[27, 29]);
                                range.Merge(true);
                                xlWorkSheet.Cells[27, 1] = "Datum predaje " + DateTime.Now.ToString("dd.MM.yyyy") + "";
                                xlWorkSheet.Cells[30, 1] = "NAPOMENA: Crvenom bojom su oznaceni dani kada su zaposleni imali posebne tipove prolazaka, koji ne predstavljaju direktan rad.";
                                #endregion
                                // end region footer

                                #endregion
                                // end region formatiranje i pozicioniranje u excel-u

                            }



                            // ----------------------------------------------------------------------------
                            #region SAVE
                            // save
                            string reportName = "Magna izvestaj za " + month + " - " + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                            // putanja gde ce biti sacuvano
                            xlWorkBook.SaveAs("c:\\Magna izvestaji\\" + reportName + "", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                            xlWorkBook.Close(true, misValue, misValue);
                            xlApp.Quit();

                            releaseObject(xlWorkSheet);
                            releaseObject(xlWorkBook);
                            releaseObject(xlApp);

                            MessageBox.Show("Excel file created , you can find the file c:\\" + reportName + "", "Save as", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            #endregion
                            // end region save
                        }
                        else
                        {
                            MessageBox.Show("Niste odabrali ni jednog zaposlenog!", "Greska!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        #endregion
                        // end region get selected employees, and create dictionary (EmployeeID, EmployeeTO)

                        break;

                    default:
                        break;
                }


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        public List<IOPairTO> prepareData(int wuID, string wUnitsString)
        {
            List<IOPairTO> ioPairsList = new List<IOPairTO>();
            try
            {
                IOPair ioPair = new IOPair();
                DateTime selected = dtpFor.Value;

                this.from = new DateTime(selected.Year, selected.Month, 1, 0, 0, 0);
                this.to = this.from.AddMonths(1);
                //this.to = new DateTime(selectd.Year, selectd.Month + 1, 1, 0, 0 ,0);
                ioPairsList = ioPair.SearchForPresence(wuID, wUnitsString, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceTracking.prepareData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return ioPairsList;
        }

        private bool isPass(int currentPassTypeID)
        {
            bool isPass = false;

            try
            {
                PassType tempPass = new PassType();
                tempPass.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> allPass = tempPass.Search();

                foreach (PassTypeTO pass in allPass)
                {
                    if (pass.PassTypeID == currentPassTypeID)
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + this.Name + " PresenceTracking.isPass(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isPass;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }




    }
}
