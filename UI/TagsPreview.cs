using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Util;
using Common;
using ReaderInterface;

namespace UI
{
    public partial class TagsPreview : Form
    {
        DebugLog log;

        ApplUserTO logInUser;
        private CultureInfo culture;
        ResourceManager rm;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        List<TagTO> currentTagsList;
        List<EmplStatistic> currentSumList;
        private int sortOrder;
        private int sortField;
        private int startIndex;

        private int sortOrderSUM;
        private int sortFieldSUM;
        private int startIndexSUM;

        static int desktopReaderCOMPort = 0;
        IReaderInterface readerInterface;

        // List View indexes
        const int TagIDIndex = 0;
        const int WUIndex = 1;
        const int OwnerIndex = 2;
        const int StatusIndex = 3;
        const int CreatedByIndex = 4;
        const int CreatedTimeIndex = 5;
        const int ModifiedByIndex = 6;
        const int ModifiedTimeIndex = 7;
        const int DescriptionIndex = 8;

        // List View indexes
        const int EmplIndexSUM = 0;
        const int LostIndexSUM = 1;
        const int DamagedIndexSUM = 2;
        const int ReturndIndexSUM = 3;
        const int TotalIndexSUM = 4;

        Filter filter;

        public TagsPreview()
        {
            InitializeComponent();

            this.CenterToScreen();

            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            ReaderFactory.TechnologyType = new Reader().GetDefaultTechnology();
            readerInterface = ReaderFactory.GetReader;

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(TagsPreview).Assembly);
            setLanguage();
            currentSumList = new List<EmplStatistic>();
            currentTagsList = new List<TagTO>();
        }

        private void gbSearch_Enter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("menuTagsPreview", culture);

                //tabPages text
                tbSumary.Text = rm.GetString("tbSumary", culture);
                tbTagSearch.Text = rm.GetString("tbTagSearch", culture);

                // group box text
                this.gbSearch.Text = rm.GetString("gbSearch", culture);
                this.gbSearch1.Text = rm.GetString("gbSearch", culture);
                this.gbFilter.Text = rm.GetString("gbFilter", culture);
                this.gbStatus.Text = rm.GetString("gbStatusTags", culture);
                this.gbValid.Text = rm.GetString("gbValid", culture);
                this.gbInvalid.Text = rm.GetString("gbInvalid", culture);
                gbTimeInterval.Text = rm.GetString("gbTimeInterval", culture);
                gbNumOFTags.Text = rm.GetString("gbNumOFTags", culture);

                //check box text
                this.chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                this.chbHierarhicly1.Text = rm.GetString("chbHierarhicly", culture);
                this.chbLessThan.Text = rm.GetString("chbLessThan", culture);
                this.cbMoreThan.Text = rm.GetString("cbMoreThan", culture);

                // button's text               
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnSearchSum.Text = rm.GetString("btnSearch", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
                btnClose.Text = rm.GetString("btnClose", culture);

                // label's text
                lblEmpl.Text = rm.GetString("lblEmployee", culture);
                lblWorkingUnitID.Text = rm.GetString("lblWorkingUnit", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                
                // list view initialization
                lvTags.BeginUpdate();
                lvTags.Columns.Add(rm.GetString("hdrTagID", culture), (lvTags.Width - 9) / 9-10, HorizontalAlignment.Left);
                lvTags.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvTags.Width - 9) / 9+10, HorizontalAlignment.Left);
                lvTags.Columns.Add(rm.GetString("hdrEmployee", culture), (lvTags.Width - 9) / 9+20, HorizontalAlignment.Left);
                lvTags.Columns.Add(rm.GetString("hdrStatus", culture), (lvTags.Width - 9) / 9-20, HorizontalAlignment.Left);
                lvTags.Columns.Add(rm.GetString("hdrCreatedBy", culture), (lvTags.Width - 9) / 9, HorizontalAlignment.Left);
                lvTags.Columns.Add(rm.GetString("hdrCreatedTime", culture), (lvTags.Width - 9) / 9, HorizontalAlignment.Left);
                lvTags.Columns.Add(rm.GetString("hdrModifiedBy", culture), (lvTags.Width - 9) / 9, HorizontalAlignment.Left);
                lvTags.Columns.Add(rm.GetString("hdrModifiedTime", culture), (lvTags.Width - 9) / 9, HorizontalAlignment.Left);
                lvTags.Columns.Add(rm.GetString("hdrDescription", culture), (lvTags.Width - 9) / 9, HorizontalAlignment.Left);
                lvTags.EndUpdate();

                lvSum.BeginUpdate();
                lvSum.Columns.Add(rm.GetString("hdrEmployee", culture), (lvSum.Width - 9) / 5 - 10, HorizontalAlignment.Left);
                lvSum.Columns.Add(Constants.statusLost, (lvSum.Width - 9) / 5, HorizontalAlignment.Left);
                lvSum.Columns.Add(Constants.statusDamaged, (lvSum.Width - 9) / 5, HorizontalAlignment.Left);
                lvSum.Columns.Add(Constants.statusReturned, (lvSum.Width - 9) / 5, HorizontalAlignment.Left);
                lvSum.Columns.Add(rm.GetString("hdrTotal", culture), (lvSum.Width - 9) / 5, HorizontalAlignment.Left);
                lvSum.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateWorkigUnitComboSum()
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
                log.writeLog(DateTime.Now + " TagsPreview.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void TagsPreview_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                sortOrder = Constants.sortAsc;
                sortField = 0;
                startIndex = 0;

                sortOrderSUM = Constants.sortAsc;
                sortFieldSUM = 0;
                startIndexSUM = 0;

                btnPrev.Visible = false;
                btnNext.Visible = false;


                btnPrevSum.Visible = false;
                btnNextSum.Visible = false;

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
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
                populateWorkigUnitComboSum();
                populateEmployeeCombo(-1);
                if (wuString.Equals(""))
                {
                    MessageBox.Show(rm.GetString("noEmplPrivilege", culture));
                }
                filter = new Filter();
                //filter.SerachButton = btnSearch;
                filter.TabID = this.tcPreview.SelectedTab.Text;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.TagsPreview_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " TagsPreview.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                    cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWorkingUnitID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                if (!chbHierarhicly.Checked)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (cbWU.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
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
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " TagsPreview.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }       
        }
        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                List<EmployeeTO> emplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    emplArray = new Employee().Search();
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
                    emplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbEmpl.DataSource = emplArray;
                cbEmpl.DisplayMember = "LastName";
                cbEmpl.ValueMember = "EmployeeID";
                cbEmpl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private List<EmployeeTO> getEmployeesList(int wuID)
        {
            List<EmployeeTO> emplArray = new List<EmployeeTO>();
            try
            {                
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    emplArray = new Employee().Search();
                }
                else
                {
                    if (chbHierarhicly1.Checked)
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
                    emplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return emplArray;
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
                log.writeLog(DateTime.Now + " TagsPreview.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReadTagID_Click(object sender, EventArgs e)
        {
            try
            {
              
                //int COMPort = Int32.Parse(ConfigurationManager.AppSettings["DesktopReaderAddress"]);
                uint tagID = 0;
                this.Cursor = Cursors.WaitCursor;
                if (desktopReaderCOMPort == 0) desktopReaderCOMPort = readerInterface.FindDesktopReader();
                this.Cursor = Cursors.Arrow;
                if (desktopReaderCOMPort == 0)
                {
                    MessageBox.Show(rm.GetString("noDesktopReader", culture));
                    return;
                }
                else
                {
                    tagID = UInt32.Parse(readerInterface.GetTagID(desktopReaderCOMPort));
                }

                if (tagID == 0)
                {
                    MessageBox.Show(rm.GetString("noTagOnReader", culture));
                }
                else
                {
                    this.tbTagID.Text = tagID.ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tags.btnFromReader_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("noTagOnReader", culture));
            }
            finally
            {
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
                populateListView(currentTagsList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.btnPrev_Click(): " + ex.Message + "\n");
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
                populateListView(currentTagsList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }


        /// <summary>
        /// Populate ListView
        /// </summary>
        /// <param name="employeesList">Array of tag object</param>
        public void populateListView(List<TagTO> tagsList, int startIndex)
        {
            try
            {
                if (tagsList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvTags.BeginUpdate();
                lvTags.Items.Clear();

                if (tagsList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < tagsList.Count))
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
                        if (lastIndex >= tagsList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = tagsList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        //ListViewItem[] lvItems = new ListViewItem[lastIndex - startIndex];

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            TagTO tag = tagsList[i];
                            ListViewItem item = new ListViewItem();

                            item.Text = tag.TagID.ToString().Trim();
                            item.SubItems.Add(tag.WorkingUnit.Trim());
                            item.SubItems.Add(tag.EmployeeName.Trim());
                            item.SubItems.Add(tag.Status.Trim());
                            item.SubItems.Add(tag.CreatedBy.Trim());
                            item.SubItems.Add(tag.Issued.ToString("dd.MM.yyyy"));
                            if (tag.ModifiedBy.Equals(""))
                            {
                                item.SubItems.Add("N/A");
                            }
                            else
                            {
                                item.SubItems.Add(tag.ModifiedBy);
                            }
                            if (tag.ModifiedTime == new DateTime())
                            {
                                item.SubItems.Add("N/A");
                            }
                            else
                            {
                                item.SubItems.Add(tag.ModifiedTime.ToString("dd.MM.yyyy"));
                            }

                            item.SubItems.Add(tag.Description.ToString());
                            lvTags.Items.Add(item);
                            //lvItems[i - startIndex] = item;
                        }
                        //lvTags.Items.AddRange(lvItems);
                    }
                }

                lvTags.EndUpdate();
                lvTags.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate ListView
        /// </summary>
        /// <param name="employeesList">Array of tag object</param>
        private void populateListViewSum(List<EmplStatistic> tagsSumList, int startIndex)
        {
            try
            {
                if (tagsSumList.Count > Constants.recordsPerPage)
                {
                    btnPrevSum.Visible = true;
                    btnNextSum.Visible = true;
                }
                else
                {
                    btnPrevSum.Visible = false;
                    btnNextSum.Visible = false;
                }

                lvSum.BeginUpdate();
                lvSum.Items.Clear();

                if (tagsSumList.Count > 0)
                {
                    if ((startIndexSUM >= 0) && (startIndexSUM < tagsSumList.Count))
                    {
                        if (startIndexSUM == 0)
                        {
                            btnPrevSum.Enabled = false;
                        }
                        else
                        {
                            btnPrevSum.Enabled = true;
                        }

                        int lastIndex = startIndexSUM + Constants.recordsPerPage;
                        if (lastIndex >= tagsSumList.Count)
                        {
                            btnNextSum.Enabled = false;
                            lastIndex = tagsSumList.Count;
                        }
                        else
                        {
                            btnNextSum.Enabled = true;
                        }

                        //ListViewItem[] lvItems = new ListViewItem[lastIndex - startIndex];

                        for (int i = startIndexSUM; i < lastIndex; i++)
                        {
                            EmplStatistic eStat = (EmplStatistic)tagsSumList[i];
                            ListViewItem item = new ListViewItem();

                            item.Text = eStat.emplName.ToString().Trim();
                            item.SubItems.Add(eStat.lostTags.ToString());
                            item.SubItems.Add(eStat.damagedTags.ToString());
                            item.SubItems.Add(eStat.returndTags.ToString());
                            item.SubItems.Add(eStat.total.ToString());
                            item.Tag = eStat;
                            lvSum.Items.Add(item);
                            //lvItems[i - startIndex] = item;
                        }
                        //lvSum.Items.AddRange(lvItems);
                    }
                }

                lvSum.EndUpdate();
                lvSum.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.tbTagID.Text = "";

                this.cbWU.SelectedIndex = 0;
                this.cbEmpl.SelectedIndex = 0;

                this.cbIncludeTimeSearch.Checked = false;
                this.cbLost.Checked = true;
                this.cbReturned.Checked = true;
                this.cbDamaged.Checked = true;
                this.cbBlocked.Checked = true;
                this.cbActive.Checked = true;
                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpTo.Value = DateTime.Now;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
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
                log.writeLog(DateTime.Now + " TagsPreview.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Tag tag = new Tag();

                if (wuString.Equals(""))
                {
                    MessageBox.Show(rm.GetString("noEmplTagsPrivilege", culture));
                    return;
                }

                if (cbEmpl.Items.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noTagsFound", culture));
                    return;
                }

                int emplID = -1;
                if (cbEmpl.SelectedIndex >= 0 && (int)cbEmpl.SelectedValue >= 0)
                {
                    emplID = (int)cbEmpl.SelectedValue;
                }
                string selectedWU = wuString;
                if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                {
                    selectedWU = cbWU.SelectedValue.ToString();

                    //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
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

                string status = "";
                if (cbActive.Checked)
                    status += "'"+Constants.statusActive+"' , ";
                if (cbBlocked.Checked)
                    status += "'" + Constants.statusBlocked + "' , ";
                if (cbLost.Checked)
                    status += "'" + Constants.statusLost + "' , ";
                if (cbReturned.Checked)
                    status += "'" + Constants.statusReturned + "' , ";
                if (cbDamaged.Checked)
                    status += "'" + Constants.statusDamaged + "' , ";

                if (status.Equals(""))
                {
                    MessageBox.Show(rm.GetString("noTagsFound", culture));
                    return;
                }
                else
                {
                    status = status.Substring(0, status.Length - 2);
                }
                DateTime from = new DateTime();
                DateTime to = new DateTime();

                if (cbIncludeTimeSearch.Checked)
                {
                    from = dtpFrom.Value.Date;
                    to = dtpTo.Value.Date;
                }

                int count = tag.SearchTagsCount(emplID, status, selectedWU,
                     from, to, tbTagID.Text.ToString().Trim());

                if (count > Constants.maxRecords)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("tagsGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        currentTagsList = tag.SearchTags(emplID, status, selectedWU,
                     from, to, tbTagID.Text.ToString().Trim());
                    }
                    else
                    {
                        currentTagsList.Clear();
                        clearListView();
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        currentTagsList = tag.SearchTags(emplID, status, selectedWU,
                     from, to, tbTagID.Text.ToString().Trim());
                    }
                    else
                    {
                        currentTagsList.Clear();
                        clearListView();
                    }
                }
                if (currentTagsList.Count > 0)
                {
                    currentTagsList.Sort(new ArrayListSort(sortOrder, sortField));
                    lblTotal.Text = rm.GetString("lblTotal", culture) + currentTagsList.Count.ToString().Trim();
                    startIndex = 0;
                    populateListView(currentTagsList, startIndex);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noTagsFound", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void clearListView()
        {
            try
            {
                lvTags.Items.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.clearListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }
        

		#region Inner Class for sorting Array List of TagsPreview

		/*
		 *  Class used for sorting Array List of TagsPreview
		*/

		private class ArrayListSort:IComparer <TagTO>  
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}

            public int Compare(TagTO x, TagTO y)        
			{
                TagTO tag1 = null;
                TagTO tag2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    tag1 = x;
                    tag2 = y;
                }
                else
                {
                    tag1 = y;
                    tag2 = x;
                }

				switch(compField)            
				{                
					case TagsPreview.TagIDIndex: 
						return tag1.TagID.CompareTo(tag2.TagID);
					case TagsPreview.WUIndex:
						return tag1.WorkingUnit.CompareTo(tag2.WorkingUnit);
					case TagsPreview.OwnerIndex:
						return tag1.EmployeeName.CompareTo(tag2.EmployeeName);
					case TagsPreview.StatusIndex:
						return tag1.Status.CompareTo(tag2.Status);
					case TagsPreview.CreatedByIndex:
						return tag1.CreatedBy.CompareTo(tag2.CreatedBy);
					case TagsPreview.CreatedTimeIndex:
						return tag1.Issued.CompareTo(tag2.Issued);
					case TagsPreview.ModifiedByIndex:
						return tag1.ModifiedBy.CompareTo(tag2.ModifiedBy);
					case TagsPreview.ModifiedTimeIndex:
						return tag1.ModifiedTime.CompareTo(tag2.ModifiedTime);
					case TagsPreview.DescriptionIndex:
						return tag1.Description.CompareTo(tag2.Description);				
					default:                    
						return tag1.EmployeeName.CompareTo(tag2.EmployeeName);
				}        
			}    
		}

		#endregion

        private void cbIncludeTimeSearch_CheckedChanged(object sender, EventArgs e)
        {
            gbTimeInterval.Enabled = cbIncludeTimeSearch.Checked;
        }

        private void tcPreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                filter.TabID = this.tcPreview.SelectedTab.Text;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + "SecurityRoutes.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter, this.tcPreview.SelectedTab);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree1_Click(object sender, EventArgs e)
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
                log.writeLog(DateTime.Now + " TagsPreview.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbMoreThan_CheckedChanged(object sender, EventArgs e)
        {
            tbMoreThan.Enabled = cbMoreThan.Checked;

        }

        private void chbLessThan_CheckedChanged(object sender, EventArgs e)
        {
            tbLessThan.Enabled = chbLessThan.Checked;
        }

        private void btnSearchSum_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                startIndexSUM = 0;
                                
                List<TagTO> tags = new List<TagTO>();

                if (wuString.Equals(""))
                {
                    MessageBox.Show(rm.GetString("noEmplTagsPrivilege", culture));
                    return;
                }
                List<EmployeeTO> emplList = getEmployeesList((int)cbWorkingUnit.SelectedValue);
                if (emplList.Count <= 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noDataFound", culture));
                    lvSum.Items.Clear();
                    return;
                }
                string selectedWU = wuString;
                if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                {
                    selectedWU = cbWorkingUnit.SelectedValue.ToString();

                    //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                    WorkingUnit wu = new WorkingUnit();
                    if ((int)this.cbWorkingUnit.SelectedValue != -1 && chbHierarhicly1.Checked)
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

                int count = new Tag().SearchTagsCount(-1, "", selectedWU, new DateTime(), new DateTime(), "");

                if (count > 0)
                {
                    tags = new Tag().SearchTags(-1, "", selectedWU, new DateTime(), new DateTime(), "");
                }

                currentSumList = new List<EmplStatistic>();
                foreach (EmployeeTO empl in emplList)
                {
                    EmplStatistic emplStat = new EmplStatistic();
                    emplStat.emplID = empl.EmployeeID;
                    emplStat.emplName = empl.LastName;
                    emplStat.wuID = empl.WorkingUnitID;

                    foreach (TagTO t in tags)
                    {
                        if (t.OwnerID == empl.EmployeeID)
                        {
                            switch (t.Status)
                            {
                                case Constants.statusLost:
                                    emplStat.lostTags++;
                                    emplStat.total++;
                                    break;
                                case Constants.statusDamaged:
                                    emplStat.damagedTags++;
                                    emplStat.total++;
                                    break;
                                case Constants.statusReturned:
                                    emplStat.returndTags++;
                                    emplStat.total++;
                                    break;
                            }
                        }
                    }
                    if ((!cbMoreThan.Checked || emplStat.total > (int)tbMoreThan.Value)
                        && (!chbLessThan.Checked || emplStat.total < (int)tbLessThan.Value))
                    {
                        currentSumList.Add(emplStat);
                    }
                }
                if (currentSumList.Count > 0)
                {
                    currentSumList.Sort(new ArrayListSortSUM(sortOrderSUM, sortFieldSUM));
                    lblTotalSum.Text = rm.GetString("lblTotal", culture) + currentSumList.Count.ToString().Trim();
                    startIndexSUM = 0;
                    populateListViewSum(currentSumList, startIndexSUM);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noTagsFound", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.btnSearchSum_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private class EmplStatistic
        {
            public int emplID = -1;
            public int wuID = -1;
            public string emplName = "";
            public int lostTags = 0;
            public int damagedTags = 0;
            public int returndTags = 0;
            public int total = 0;

            public EmplStatistic()
            { 
                
            }
        }

        private void lvTags_ColumnClick(object sender, ColumnClickEventArgs e)
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

                currentTagsList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentTagsList, startIndex);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvEmployees_ColumnClick():" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        #region Inner Class for sorting Array List of TagsPreview

        /*
		 *  Class used for sorting Array List of TagsPreview
		*/

        private class ArrayListSortSUM : IComparer <EmplStatistic>
        {
            private int compOrder;
            private int compField;
            public ArrayListSortSUM(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(EmplStatistic x, EmplStatistic y)
            {
                EmplStatistic empl1 = null;
                EmplStatistic empl2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    empl1 = x;
                    empl2 = y;
                }
                else
                {
                    empl1 = y;
                    empl2 = x;
                }

                switch (compField)
                {
                    case TagsPreview.EmplIndexSUM:
                        return empl1.emplName.CompareTo(empl2.emplName);
                    case TagsPreview.LostIndexSUM:
                        return empl1.lostTags.CompareTo(empl2.lostTags);
                    case TagsPreview.DamagedIndexSUM:
                        return empl1.damagedTags.CompareTo(empl2.damagedTags);
                    case TagsPreview.ReturndIndexSUM:
                        return empl1.returndTags.CompareTo(empl2.returndTags);
                    case TagsPreview.TotalIndexSUM:
                        return empl1.total.CompareTo(empl2.total);                  
                    default:
                        return empl1.emplName.CompareTo(empl2.emplName);
                }
            }
        }

        #endregion

        private void lvSum_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrderSUM;

                if (e.Column == sortFieldSUM)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrderSUM = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrderSUM = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrderSUM = Constants.sortAsc;
                }

                sortFieldSUM = e.Column;

                currentSumList.Sort(new ArrayListSortSUM(sortOrderSUM, sortFieldSUM));
                startIndexSUM = 0;
                populateListViewSum(currentSumList, startIndexSUM);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.lvSum_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvSum_ColumnClick():" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvSum_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvSum.SelectedItems.Count > 0)
                {
                    EmplStatistic es = (EmplStatistic)lvSum.SelectedItems[0].Tag;
                    cbWU.SelectedValue = es.wuID;
                    cbEmpl.SelectedValue = es.emplID;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.lvSum_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvSum_ColumnClick():" + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TagsPreview_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TagsPreview.TagsPreview_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }      
    }
}