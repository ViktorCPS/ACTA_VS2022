using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Data.OleDb;
using System.IO;

using Util;
using Common;
using TransferObjects;

namespace UI
{
    public partial class GroupChangedMassiveInput : Form
    {
        private const int xlsIndex = 1;
        private const int xlsxIndex = 2;

        private const char delimiter = '\t';

        private CultureInfo culture;
        ResourceManager rm;

        DebugLog log;
        ApplUserTO logInUser;

        List<FileItem> fileItemsList = new List<FileItem>();
        List<int> emplIDs = new List<int>();
        List<int> grpIDs = new List<int>();

        private Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        private Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
        private Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();

        string logFilePath = "";

        public GroupChangedMassiveInput()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                this.CenterToScreen();

                logInUser = NotificationController.GetLogInUser();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(GroupChangedMassiveInput).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GroupChangedMassiveInput(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("GroupChangedMassiveInput", culture);

                // group box text
                gbGroups.Text = rm.GetString("gbGroups", culture);
                gbFileContent.Text = rm.GetString("gbFileContent", culture);

                // button's text
                btnBrowse.Text = rm.GetString("btnBrowse", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnGenerateFile.Text = rm.GetString("btnGenerateFile", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                
                // label's text
                lblFilePath.Text = rm.GetString("lblFilePath", culture);

                // list view
                lvGroups.BeginUpdate();
                lvGroups.Columns.Add(rm.GetString("hdrGroupID", culture), lvGroups.Width / 2 - 11, HorizontalAlignment.Left);
                lvGroups.Columns.Add(rm.GetString("hdrName", culture), lvGroups.Width / 2 - 11, HorizontalAlignment.Left);                
                lvGroups.EndUpdate();

                lvFileContent.BeginUpdate();
                lvFileContent.Columns.Add(rm.GetString("hdrEmplID", culture), lvFileContent.Width / 3 - 7, HorizontalAlignment.Left);
                lvFileContent.Columns.Add(rm.GetString("hdrGroupID", culture), lvFileContent.Width / 3 - 7, HorizontalAlignment.Left);
                lvFileContent.Columns.Add(rm.GetString("hdrDate", culture), lvFileContent.Width / 3 - 7, HorizontalAlignment.Left);                
                lvFileContent.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GroupChangedMassiveInput.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateWorkingGroup()
        {
            try
            {
                List<WorkingGroupTO> wgArray = new WorkingGroup().SearchIDSort();

                lvGroups.BeginUpdate();
                lvGroups.Items.Clear();
                for (int i = 0; i < wgArray.Count; i++)
                {
                    if (!grpIDs.Contains(wgArray[i].EmployeeGroupID))
                        grpIDs.Add(wgArray[i].EmployeeGroupID);

                    ListViewItem item = new ListViewItem();

                    item.Text = wgArray[i].EmployeeGroupID.ToString().Trim();
                    item.SubItems.Add(wgArray[i].GroupName.Trim());
                    lvGroups.Items.Add(item);
                }
                lvGroups.EndUpdate();
                lvGroups.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GroupChangedMassiveInput.populateWorkingGroup(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateFileContentList()
        {
            try
            {
                lvFileContent.BeginUpdate();
                lvFileContent.Items.Clear();
                for (int i = 0; i < fileItemsList.Count; i++)
                {
                    ListViewItem item = new ListViewItem();

                    item.Text = fileItemsList[i].EmplID.ToString().Trim();
                    item.SubItems.Add(fileItemsList[i].GrpID.ToString().Trim());
                    item.SubItems.Add(fileItemsList[i].StartDate.ToString(Constants.dateFormat).Trim());
                    item.Tag = fileItemsList[i];
                    lvFileContent.Items.Add(item);
                }
                lvFileContent.EndUpdate();
                lvFileContent.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GroupChangedMassiveInput.populateFileContentList(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnBrowse_ClickExcel(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                OpenFileDialog fbDialog = new OpenFileDialog();
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fbDialog.Filter = "XLS (*.xls)|*.xls|" +
                "XLSX (*.xlsx)|*.xlsx";
                fbDialog.Title = "Open file";

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    fileItemsList.Clear();
                    
                    // make connection to selected XLS file
                    string path = fbDialog.FileName;
                    tbFilePath.Text = fbDialog.FileName;
                    logFilePath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path).Trim() + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(path);

                    string excelConnectionString = "";
                    if (fbDialog.FilterIndex == xlsIndex)
                        excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=YES;'";
                    else if (fbDialog.FilterIndex == xlsxIndex)
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
                        da.Fill(ds, "EmployeesGroups");
                        DataTable table = ds.Tables["EmployeesGroups"];
                        if (table.Rows.Count > 0)
                        {
                            List<DataRow> invalidRows = new List<DataRow>();
                            // make list of items from XLS rows                            
                            foreach (DataRow row in table.Rows)
                            {
                                if (row.ItemArray.Length >= 3)
                                {
                                    FileItem item = new FileItem();

                                    int emplID = -1;
                                    int grpID = -1;
                                    DateTime startDate = new DateTime();

                                    if (!int.TryParse(row.ItemArray[0].ToString().Trim(), out emplID))
                                        emplID = -1;
                                    if (!int.TryParse(row.ItemArray[1].ToString().Trim(), out grpID))
                                        grpID = -1;
                                    if (!DateTime.TryParseExact(row.ItemArray[2].ToString().Trim(), Constants.dateFormat, null, DateTimeStyles.None, out startDate))
                                        startDate = new DateTime();

                                    if (!emplIDs.Contains(emplID))
                                        emplID = -1;

                                    if (!grpIDs.Contains(grpID))
                                        grpID = -1;

                                    item.EmplID = emplID;
                                    item.GrpID = grpID;
                                    item.StartDate = startDate;

                                    if (item.validItem())
                                        fileItemsList.Add(item);
                                    else
                                        invalidRows.Add(row);
                                }
                            }

                            populateFileContentList();

                            if (invalidRows.Count > 0)
                            {
                                addRowsLogFileExcel(invalidRows, null, rm.GetString("notValidData", culture));

                                MessageBox.Show(rm.GetString("notValidFileData", culture) + " " + logFilePath.Trim());
                            }
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noDataInXLS", culture));
                        }
                    }

                    conn.Close();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "GroupChangedMassiveInput.btnBrowse_Click(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;

                OpenFileDialog fbDialog = new OpenFileDialog();
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fbDialog.Filter = "Text files (*.txt)|*.txt";                
                fbDialog.Title = "Open file";

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    fileItemsList.Clear();

                    // make connection to selected XLS file
                    string path = fbDialog.FileName;
                    tbFilePath.Text = fbDialog.FileName;
                    logFilePath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path).Trim() + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(path);

                    // get data from file
                    if (System.IO.File.Exists(path))
                    {
                        List<string> invalidRows = new List<string>();
                        // make list of items from file rows                       
                        System.IO.FileStream str = System.IO.File.Open(path, System.IO.FileMode.Open);
                        System.IO.StreamReader reader = new System.IO.StreamReader(str);

                        // read file lines, skip header
                        string line = reader.ReadLine(); // header
                        line = reader.ReadLine(); // first row
                        while (line != null)
                        {                            
                            if (!line.Trim().Equals(""))
                            {
                                string[] rowItems = line.Split(delimiter);
                                if (rowItems.Length >= 3)
                                {
                                    FileItem item = new FileItem();

                                    int emplID = -1;
                                    int grpID = -1;
                                    DateTime startDate = new DateTime();

                                    if (!int.TryParse(rowItems[0].ToString().Trim(), out emplID))
                                        emplID = -1;
                                    if (!int.TryParse(rowItems[1].ToString().Trim(), out grpID))
                                        grpID = -1;
                                    if (!DateTime.TryParseExact(rowItems[2].ToString().Trim(), Constants.dateFormat, null, DateTimeStyles.None, out startDate))
                                        startDate = new DateTime();

                                    if (!emplIDs.Contains(emplID))
                                        emplID = -1;

                                    if (!grpIDs.Contains(grpID))
                                        grpID = -1;

                                    item.EmplID = emplID;
                                    item.GrpID = grpID;
                                    item.StartDate = startDate;

                                    if (item.validItem())
                                        fileItemsList.Add(item);
                                    else
                                        invalidRows.Add(line);
                                }
                                else
                                    invalidRows.Add(line);
                            }

                            line = reader.ReadLine();
                        }

                        reader.Close();
                        str.Dispose();
                        str.Close();

                        populateFileContentList();

                        if (invalidRows.Count > 0)
                        {
                            addRowsLogFile(invalidRows, null, rm.GetString("notValidData", culture));

                            MessageBox.Show(rm.GetString("notValidFileData", culture) + " " + logFilePath.Trim());
                        }
                    }
                    else
                        return;
                }
                else
                    return;                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "GroupChangedMassiveInput.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerateFile_ClickExcel(object sender, EventArgs e)
        {
            SaveFileDialog fbDialog = new SaveFileDialog();

            try            
            {
                this.Cursor = Cursors.WaitCursor;
              
                fbDialog.DefaultExt = ".xls";
                fbDialog.Filter = "XLS (*.xls)|*.xls|" +
                "XLSX (*.xlsx)|*.xlsx";
                fbDialog.Title = "Save file";
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string filePath = fbDialog.FileName;
                    fbDialog.Dispose();

                    CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                    Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                    Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
                    
                    object misValue = System.Reflection.Missing.Value;
                                        
                    // insert header                    
                    ws.Cells[1, 1] = rm.GetString("hdrEmplID", culture);
                    ws.Cells[1, 2] = rm.GetString("hdrGroupID", culture);
                    ws.Cells[1, 3] = rm.GetString("hdrDate", culture);

                    if (fbDialog.FilterIndex == xlsIndex)
                    {
                        wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                            Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                            Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);
                    }
                    else if (fbDialog.FilterIndex == xlsxIndex)
                    {
                        wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue,
                            Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                            Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);
                    }

                    wb.Close(true, null, null);
                    xla.Workbooks.Close();
                    xla.Quit();

                    releaseObject(ws);
                    releaseObject(wb);
                    releaseObject(xla);

                    System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;

                    MessageBox.Show(rm.GetString("FileGenerated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GroupChangedMassiveInput.btnGenerateFile_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerateFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog fbDialog = new SaveFileDialog();

            try
            {
                this.Cursor = Cursors.WaitCursor;

                fbDialog.DefaultExt = ".txt";
                fbDialog.Filter = "Text files (*.txt)|*.txt";
                fbDialog.Title = "Save file";
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string filePath = fbDialog.FileName;
                    fbDialog.Dispose();

                    FileStream stream = new FileStream(filePath, FileMode.Append);
                    stream.Close();

                    // insert header                    
                    string message = rm.GetString("hdrEmplID", culture) + delimiter + rm.GetString("hdrGroupID", culture) + delimiter + rm.GetString("hdrDate", culture);

                    StreamWriter writer = File.AppendText(filePath);
                    writer.WriteLine(message);
                    writer.Close();                    

                    MessageBox.Show(rm.GetString("FileGenerated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GroupChangedMassiveInput.btnGenerateFile_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GroupChangedMassiveInput.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        public struct FileItem
        {
            private int _emplID;
            private int _grpID;
            private DateTime _startDate;
            private string _remark;
            
            public int EmplID
            {
                get { return _emplID; }
                set { _emplID = value; }
            }

            public int GrpID
            {
                get { return _grpID; }
                set { _grpID = value; }
            }

            public DateTime StartDate
            {
                get { return _startDate; }
                set { _startDate = value; }
            }

            public string Remark
            {
                get { return _remark; }
                set { _remark = value; }
            }

            public bool validItem()
            {
                if (this.EmplID != -1 && this.GrpID != -1 && this.StartDate != null && !this.StartDate.Equals(new DateTime()))
                    return true;
                else
                    return false;
            }
        }

        private void GroupChangedMassiveInput_Load(object sender, EventArgs e)
        {
            try
            {
                populateWorkingGroup();

                emplIDs = new Employee().SearchIDs();

                string ids = "";

                foreach (int id in emplIDs)
                {
                    ids += id.ToString().Trim() + ",";
                }

                if (ids.Length > 0)
                {
                    ids = ids.Substring(0, ids.Length - 1);

                    emplDict = new Employee().SearchDictionary(ids);
                    ascoDict = new EmployeeAsco4().SearchDictionary(ids);
                }

                rulesDict = new Common.Rule().SearchWUEmplTypeDictionary();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GroupChangedMassiveInput.GroupChangedMassiveInput_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (fileItemsList.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noDataToSave", culture));
                    return;
                }
                else
                {
                    // get logged in employee
                    EmployeeTO Empl = new EmployeeTO();                    
                    EmployeeAsco4 asco = new EmployeeAsco4();
                    asco.EmplAsco4TO.NVarcharValue5 = NotificationController.GetLogInUser().UserID;
                    List<EmployeeAsco4TO> ascoList = asco.Search();

                    if (ascoList.Count > 0)                    
                        Empl = new Employee().Find(ascoList[0].EmployeeID.ToString());                    

                    // get dictionary of all rules, key is company and value are rules by employee type id
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule().SearchWUEmplTypeDictionary();
                    Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit().getWUDictionary();

                    DateTime minChangingDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).Date;

                    int cutOffDate = -1;
                    int emplCompany = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, wUnits);

                    if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(Empl.EmployeeTypeID) && emplRules[emplCompany][Empl.EmployeeTypeID].ContainsKey(Constants.RuleHRSSCCutOffDate))
                        cutOffDate = emplRules[emplCompany][Empl.EmployeeTypeID][Constants.RuleHRSSCCutOffDate].RuleValue;

                    if (cutOffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, null) > cutOffDate)
                        minChangingDate = minChangingDate.AddMonths(1).Date;
                    
                    string grpIDs = "";
                    string emplIDs = "";
                    DateTime minDate = new DateTime();
                    foreach (FileItem item in fileItemsList)
                    {
                        if (item.StartDate.Date < minChangingDate.Date)
                            continue;

                        grpIDs += item.GrpID.ToString().Trim() + ",";

                        emplIDs += item.EmplID.ToString().Trim() + ",";

                        if (minDate.Equals(new DateTime()) || minDate.Date > item.StartDate.Date)
                            minDate = item.StartDate;
                    }

                    if (grpIDs.Length > 0)
                        grpIDs = grpIDs.Substring(0, grpIDs.Length - 1);

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    List<EmployeeGroupsTimeScheduleTO> grpSchedules = new List<EmployeeGroupsTimeScheduleTO>();

                    if (grpIDs.Trim().Length > 0 && !minDate.Equals(new DateTime()))
                        grpSchedules = new EmployeeGroupsTimeSchedule().SearchGroupsSchedules(grpIDs, minDate.Date, new DateTime(), null);

                    Dictionary<int, List<EmployeeGroupsTimeScheduleTO>> groupSchedulesDict = new Dictionary<int, List<EmployeeGroupsTimeScheduleTO>>();

                    foreach (EmployeeGroupsTimeScheduleTO egts in grpSchedules)
                    {
                        if (!groupSchedulesDict.ContainsKey(egts.EmployeeGroupID))
                            groupSchedulesDict.Add(egts.EmployeeGroupID, new List<EmployeeGroupsTimeScheduleTO>());

                        groupSchedulesDict[egts.EmployeeGroupID].Add(egts);
                    }

                    Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();

                    if (emplIDs.Trim().Length > 0)
                        employees = new Employee().SearchDictionary(emplIDs);

                    Dictionary<int, WorkTimeSchemaTO> timeSchemas = new TimeSchema().getDictionary();

                    EmployeeTO employeeTO = new EmployeeTO();                    
                    List<FileItem> invalidItems = new List<FileItem>();                    

                    foreach (FileItem item in fileItemsList)
                    {
                        if (item.StartDate.Date < minChangingDate.Date)
                        {
                            invalidItems.Add(createItem(item, "CHANGING GROUPS FOR PERIOD THAT IS LOCKED FOR CHANGINGS"));
                            continue;
                        }

                        bool emplSuccesful = true;

                        //ako se transakcija otvara na nivou jednog zaposlenog, uraditi to ovde
                        Employee empl = new Employee();

                        List<EmployeeGroupsTimeScheduleTO> groupSchedules = new List<EmployeeGroupsTimeScheduleTO>();
                        if (groupSchedulesDict.ContainsKey(item.GrpID))
                            groupSchedules = groupSchedulesDict[item.GrpID];

                        if (empl.BeginTransaction())
                        {
                            try
                            {
                                if (item.StartDate != null && !item.StartDate.Equals(new DateTime()))
                                {
                                    if (employees.ContainsKey(item.EmplID))
                                        employeeTO = employees[item.EmplID];
                                    else
                                        employeeTO = new EmployeeTO();

                                    emplSuccesful = empl.Update(employeeTO.EmployeeID.ToString(), employeeTO.FirstName, employeeTO.LastName,
                                        employeeTO.WorkingUnitID.ToString(), employeeTO.Status, employeeTO.Password,
                                        employeeTO.AddressID.ToString(), employeeTO.Picture, item.GrpID.ToString().Trim(),
                                        employeeTO.Type, employeeTO.AccessGroupID.ToString().Trim(), false) && emplSuccesful;

                                    if (emplSuccesful)
                                    {
                                        EmployeesTimeSchedule ets = new EmployeesTimeSchedule();
                                        ets.SetTransaction(empl.GetTransaction());
                                        emplSuccesful = ets.DeleteFromToSchedule(employeeTO.EmployeeID, item.StartDate.Date, new DateTime(), "", false) && emplSuccesful;

                                        if (emplSuccesful)
                                        {
                                            int timeScheduleIndex = -1;
                                            for (int scheduleIndex = 0; scheduleIndex < groupSchedules.Count; scheduleIndex++)
                                            {
                                                if (item.StartDate.Date >= groupSchedules[scheduleIndex].Date)
                                                {
                                                    timeScheduleIndex = scheduleIndex;
                                                }
                                            }
                                            if (timeScheduleIndex >= 0)
                                            {
                                                EmployeeGroupsTimeScheduleTO egts = groupSchedules[timeScheduleIndex];
                                                int startDay = egts.StartCycleDay;
                                                int schemaID = egts.TimeSchemaID;

                                                WorkTimeSchemaTO actualTimeSchema = null;
                                                if (timeSchemas.ContainsKey(schemaID))
                                                    actualTimeSchema = timeSchemas[schemaID];

                                                if (actualTimeSchema != null)
                                                {
                                                    int cycleDuration = actualTimeSchema.CycleDuration;

                                                    TimeSpan ts = new TimeSpan(item.StartDate.Date.Ticks - egts.Date.Date.Ticks);
                                                    int dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;

                                                    int insert = ets.Save(employeeTO.EmployeeID, item.StartDate.Date,
                                                        schemaID, dayNum, "", false);
                                                    emplSuccesful = (insert > 0) && emplSuccesful;

                                                    if (emplSuccesful)
                                                    {
                                                        for (int scheduleIndex = timeScheduleIndex + 1; scheduleIndex < groupSchedules.Count; scheduleIndex++)
                                                        {
                                                            egts = groupSchedules[scheduleIndex];

                                                            insert = ets.Save(employeeTO.EmployeeID, egts.Date,
                                                                egts.TimeSchemaID, egts.StartCycleDay, "", false);
                                                            emplSuccesful = (insert > 0) && emplSuccesful;

                                                            if (!emplSuccesful)
                                                                break;
                                                        }
                                                    }
                                                }
                                            }

                                            if (emplSuccesful)
                                            {
                                                // delete absences pairs and update absences to unused
                                                deleteIOPUpdateEA(employeeTO.EmployeeID, item.StartDate.Date, empl.GetTransaction());

                                                //recalculate pauses
                                                if (item.StartDate.Date <= DateTime.Now.Date)
                                                {
                                                    IOPair ioPair = new IOPair();
                                                    ioPair.recalculatePause(employeeTO.EmployeeID.ToString(), item.StartDate.Date, DateTime.Now.Date, empl.GetTransaction());
                                                }
                                            } //if (emplSuccesful)
                                            else
                                            {
                                                invalidItems.Add(createItem(item, "INSERTING GROUP SCHEDULE FAILED"));

                                                if (empl.GetTransaction() != null)
                                                    empl.RollbackTransaction();

                                                continue;
                                            }
                                        } //if (emplSuccesful)
                                        else
                                        {
                                            invalidItems.Add(createItem(item, "DELETING EMPLOYEE SCHEDULE FAILED"));

                                            if (empl.GetTransaction() != null)
                                                empl.RollbackTransaction();

                                            continue;
                                        }
                                    } //if (emplSuccesful)
                                    else
                                    {
                                        invalidItems.Add(createItem(item, "UPDATE EMPLOYEE FAILED"));

                                        if (empl.GetTransaction() != null)
                                            empl.RollbackTransaction();

                                        continue;
                                    }
                                }
                                else
                                {
                                    invalidItems.Add(createItem(item, "INVALID DATE"));

                                    if (empl.GetTransaction() != null)
                                        empl.RollbackTransaction();

                                    continue;
                                }

                                if (emplSuccesful)
                                {
                                    // validate new employee schedule
                                    bool validFundHrs = true;
                                    DateTime scheduleInvalidDate = Common.Misc.isValidTimeSchedule(emplDict, ascoDict, rulesDict, employeeTO.EmployeeID.ToString().Trim(), item.StartDate.Date.Date, item.StartDate.Date.AddMonths(1).Date, empl.GetTransaction(), null, false, ref validFundHrs, true);
                                    if (scheduleInvalidDate.Equals(new DateTime()))
                                    {
                                        if (empl.GetTransaction() != null)
                                            empl.CommitTransaction();
                                    }
                                    else
                                    {
                                        if (empl.GetTransaction() != null)
                                            empl.RollbackTransaction();

                                        if (validFundHrs)
                                            invalidItems.Add(createItem(item, rm.GetString("notValidScheduleAssigned", culture)
                                                 + " " + scheduleInvalidDate.Date.AddDays(-1).ToString(Constants.dateFormat) + "/" + scheduleInvalidDate.Date.ToString(Constants.dateFormat)));

                                        else
                                            invalidItems.Add(createItem(item, rm.GetString("notValidFundHrs", culture) + " " + scheduleInvalidDate.Date.ToString(Constants.dateFormat) + "-" + scheduleInvalidDate.AddDays(6).Date.ToString(Constants.dateFormat)));

                                        continue;
                                    }
                                }
                                else
                                {
                                    invalidItems.Add(createItem(item, "CHANGING GROUP FAILED"));

                                    if (empl.GetTransaction() != null)
                                        empl.RollbackTransaction();

                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (empl.GetTransaction() != null)
                                    empl.RollbackTransaction();

                                invalidItems.Add(createItem(item, ex.Message));

                                continue;
                            }
                        }
                        else
                        {
                            invalidItems.Add(createItem(item, "BEGIN TRANSACTION FAILED"));

                            continue;
                        }
                    }

                    if (invalidItems.Count > 0)
                    {
                        addRowsLogFile(null, invalidItems, "");

                        MessageBox.Show(rm.GetString("groupChangedMassiveInputFailedForSomeItems", culture) + " " + logFilePath.Trim());
                    }
                    else
                        MessageBox.Show(rm.GetString("emplGroupChanged", culture));

                    fileItemsList.Clear();
                    populateFileContentList();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GroupChangedMassiveInput.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void addRowsLogFileExcel(List<DataRow> rows, List<FileItem> items, string remark)
        {
            try
            {
                if (!logFilePath.Trim().Equals(""))
                {
                    if (!File.Exists(logFilePath))
                        createLog();

                    // make connection to selected Excel file                    
                    string excelConnectionString = "";
                    if (Path.GetExtension(logFilePath).Trim().ToUpper().Equals(Constants.extXLS.Trim().ToUpper()))
                        excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + logFilePath + ";Extended Properties='Excel 8.0; HDR=YES;'";
                    else if (Path.GetExtension(logFilePath).Trim().ToUpper().Equals(Constants.extXLSX.Trim().ToUpper()))
                        excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + logFilePath + ";Extended Properties='Excel 12.0 xml; HDR=YES;'";
                    
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
                        if (rows != null && rows.Count > 0)
                        {
                            // add invalid rows to log file
                            foreach (DataRow row in rows)
                            {
                                if (row.ItemArray.Length >= 3)
                                {
                                    string emplIDValue = "";
                                    string groupIDValue = "";
                                    string dateValue = "";

                                    if (row.ItemArray[0] != DBNull.Value)
                                        emplIDValue = row.ItemArray[0].ToString();

                                    if (row.ItemArray[1] != DBNull.Value)
                                        groupIDValue = row.ItemArray[1].ToString();

                                    if (row.ItemArray[2] != DBNull.Value)
                                        dateValue = row.ItemArray[2].ToString();

                                    OleDbCommand sqlcom = new OleDbCommand("INSERT INTO [" + sheetNames[0].Trim() + "] VALUES ('" + emplIDValue + "', '" + 
                                        groupIDValue + "', '" + dateValue + "', '" + remark.Trim() + "')", conn);
                                    sqlcom.ExecuteNonQuery();
                                }
                            }
                        }

                        if (items != null && items.Count > 0)
                        {
                            // add invalid items to log file
                            foreach (FileItem item in items)
                            {
                                string dateValue = new DateTime().ToString(Constants.dateFormat);

                                if (item.StartDate != null)
                                    dateValue = item.StartDate.ToString(Constants.dateFormat);

                                OleDbCommand sqlcom = new OleDbCommand("INSERT INTO [" + sheetNames[0].Trim() + "] VALUES ('" + item.EmplID.ToString().Trim() + "', '" +
                                    item.GrpID.ToString().Trim() + "', '" + dateValue + "', '" + item.Remark.Trim() + "')", conn);
                                sqlcom.ExecuteNonQuery();
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GroupChangedMassiveInput.addRowsLogFile(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void createLog()
        {
            try
            {
                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                object misValue = System.Reflection.Missing.Value;

                // insert header                    
                ws.Cells[1, 1] = rm.GetString("hdrEmplID", culture);
                ws.Cells[1, 2] = rm.GetString("hdrGroupID", culture);
                ws.Cells[1, 3] = rm.GetString("hdrDate", culture);
                ws.Cells[1, 4] = rm.GetString("hdrRemark", culture);

                if (Path.GetExtension(logFilePath).Trim().ToUpper().Equals(Constants.extXLS.Trim().ToUpper()))
                {
                    wb.SaveAs(logFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                        Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);
                }
                else if (Path.GetExtension(logFilePath).Trim().ToUpper().Equals(Constants.extXLSX.Trim().ToUpper()))
                {
                    wb.SaveAs(logFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue,
                        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                        Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);
                }

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GroupChangedMassiveInput.createLog(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void addRowsLogFile(List<string> rows, List<FileItem> items, string remark)
        {
            try
            {
                if (!logFilePath.Trim().Equals(""))
                {
                    if (!File.Exists(logFilePath))
                    {
                        // create log file
                        FileStream stream = new FileStream(logFilePath, FileMode.Append);
                        stream.Close();
                    }

                    if (rows != null && rows.Count > 0)
                    {
                        StreamWriter writer = File.AppendText(logFilePath);
                                                
                        // add invalid rows to log file
                        foreach (string row in rows)
                        {
                            writer.WriteLine(row);
                        }

                        writer.Close();
                    }

                    if (items != null && items.Count > 0)
                    {
                        StreamWriter writer = File.AppendText(logFilePath);

                        // add invalid items to log file
                        foreach (FileItem item in items)
                        {                            
                            string dateValue = new DateTime().ToString(Constants.dateFormat);

                            if (item.StartDate != null)
                                dateValue = item.StartDate.ToString(Constants.dateFormat);

                            string row = item.EmplID.ToString().Trim() + delimiter + item.GrpID.ToString().Trim() + delimiter + dateValue + delimiter + item.Remark.Trim();
                                
                            writer.WriteLine(row);
                        }

                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GroupChangedMassiveInput.addRowsLogFile(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void deleteIOPUpdateEA(int employeeID, DateTime startingDate, IDbTransaction trans)
        {
            try
            {
                #region Reprocess Dates
                List<DateTime> datesList = new List<DateTime>();

                DateTime endDate = new IOPairProcessed().getMaxDateOfPair(employeeID.ToString(), trans);

                if (endDate.Date < DateTime.Now.Date)
                    endDate = DateTime.Now.Date;

                for (DateTime dt = startingDate.Date; dt <= endDate; dt = dt.AddDays(1))
                {
                    datesList.Add(dt);
                }

                //list od datetime for each employee
                Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                emplDateWholeDayList.Add(employeeID, datesList);
                if (datesList.Count > 0)
                {
                    Common.Misc.ReprocessPairsAndRecalculateCounters(employeeID.ToString(), startingDate.Date, DateTime.Now.Date, trans, emplDateWholeDayList, null, "");
                }

                #endregion

                DateTime end = new DateTime(0);
                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO.EmployeeID = employeeID;
                ea.EmplAbsTO.DateStart = startingDate.Date;
                ea.EmplAbsTO.DateEnd = end;
                List<EmployeeAbsenceTO> emplAbsences = ea.Search("", trans);

                foreach (EmployeeAbsenceTO abs in emplAbsences)
                {
                    new EmployeeAbsence().UpdateEADeleteIOP(abs.RecID, abs.EmployeeID, abs.PassTypeID,
                        abs.PassTypeID, abs.DateStart, abs.DateEnd, abs.DateStart, abs.DateEnd, (int)Constants.Used.No, trans);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GroupChangedMassiveInput.deleteIOPUpdateEA(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private FileItem createItem(FileItem invalidItem, string remark)
        {
            try
            {
                FileItem item = new FileItem();
                item.EmplID = invalidItem.EmplID;
                item.GrpID = invalidItem.GrpID;
                item.StartDate = invalidItem.StartDate;
                item.Remark = remark;

                return item;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GroupChangedMassiveInput.createItem(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
