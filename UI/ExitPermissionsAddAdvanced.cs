using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;
using ReaderInterface;

namespace UI
{
    public partial class ExitPermissionsAddAdvanced : Form
    {
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;
        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        // List View indexes
        const int FirstNameIndex = 0;
        const int LastNameIndex = 1;
        const int WorkingUnitIDIndex = 2;

        List<EmployeeTO> currentEmployeesList;
        private int sortOrder;
        private int sortField;
        private int startIndex;

        ExitPermission currentExitPerm = null;

        List<EmployeeTO> selectedEmployees;

        //there is two way's of sorting: by employees or by days
        private int Sorting;
        const int employeesSorting = 0;
        const int daysSorting = 1;

        List<PassTypeTO> ptArray;

        public ExitPermissionsAddAdvanced()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            this.CenterToScreen();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            rm = new ResourceManager("UI.Resource", typeof(ExitPermissionsAddAdvanced).Assembly);
            setLanguage();
            currentExitPerm = new ExitPermission();
            selectedEmployees = new List<EmployeeTO>();
            Sorting = employeesSorting;
            ptArray = new List<PassTypeTO>();
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
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvancedMulti.btnClose_Click(): " + ex.Message + "\n");
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
                this.Text = rm.GetString("ExitPermAddMultiForm", culture);

                // button's text
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnPreview.Text = rm.GetString("btnPrev", culture);
                btnAdd.Text = rm.GetString("btnAdd", culture);

                //label's text
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblRemoveDay.Text = rm.GetString("lblRemoveDay", culture);
                lblWU.Text = rm.GetString("lblWU", culture);

                //radio buttun's text
                rbCertainDays.Text = rm.GetString("rbCertainDays", culture);
                rbDaySort.Text = rm.GetString("rbDaySort", culture);
                rbEmplSort.Text = rm.GetString("rbEmplSort", culture);
                rbPeriod.Text = rm.GetString("rbPeriod", culture);

                //group box's
                gbChooseEmpl.Text = rm.GetString("gbChooseEmpl", culture);
                gbDays.Text = rm.GetString("gbDays", culture);
                gbPassType.Text = rm.GetString("gbPassType", culture);
                gbSortOrder.Text = rm.GetString("gbSortOrder", culture);
                gbDescription.Text = rm.GetString("gbDescription", culture);

                //check box text
                this.chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

                // list view
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 3) / 3-7, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 3) / 3-7, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployees.Width - 3) / 3-7, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvDays.BeginUpdate();
                lvDays.Columns.Add(rm.GetString("choosenDays",culture),lvDays.Width-25);
                lvDays.EndUpdate();
                
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvancedMulti.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populatePassTypeCombo()
        {
            try
            {
                PassType pt = new PassType();
                pt.PTypeTO.IsPass = Constants.passOnReader;
                ptArray = pt.Search();
                
                List<PassTypeTO> passTypes = new List<PassTypeTO>();

                foreach (PassTypeTO pt1 in ptArray)
                {
                    if (pt1.PassTypeID != 0)
                    {
                        passTypes.Add(pt1);
                    }
                }

                passTypes.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

                cbPassType.DataSource = passTypes;
                cbPassType.DisplayMember = "Description";
                cbPassType.ValueMember = "PassTypeID";
                cbPassType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.populatePassTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void ExitPermissionsAddAdvanced_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                sortOrder = Constants.sortAsc;
                sortField = ExitPermissionsAddAdvanced.LastNameIndex;
                startIndex = 0;

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PermissionPurpose);
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
                
                if (currentExitPerm.PermTO.PermissionID >= 0)
                {
                    EmployeeTO eTO = new Employee().Find(currentExitPerm.PermTO.EmployeeID.ToString().Trim());
                    cbWU.SelectedValue = eTO.WorkingUnitID;
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (((EmployeeTO)item.Tag).EmployeeID == currentExitPerm.PermTO.EmployeeID)
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            break;
                        }
                    }
                }
                populatePassTypeCombo();

                lvDays.Enabled = false;
                lblRemoveDay.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.Load(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
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
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);
                if (cbWU.SelectedIndex <= 0)
                {
                    currentEmployeesList = new Employee().SearchByWUWithStatuses(wuString, statuses);
                }
                else
                {
                    string wunits = "";
                    WorkingUnit wu = new WorkingUnit();
                    List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();

                    wuList.Add((WorkingUnitTO)cbWU.SelectedItem);

                    if (chbHierarhicly.Checked)
                    {
                        wuList = wu.FindAllChildren(wuList);
                    }

                    foreach (WorkingUnitTO workingUnit in wuList)
                    {
                        wunits += workingUnit.WorkingUnitID + ",";
                    }

                    if (wunits.Length > 0)
                    {
                        wunits = wunits.Substring(0, wunits.Length - 1);
                    }
                    currentEmployeesList = new Employee().SearchByWU(wunits);
                }

                currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateListView(List<EmployeeTO> employeeList, int startIndex)
        {
            try
			{
				if (employeeList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}

				lvEmployees.BeginUpdate();
				lvEmployees.Items.Clear();

				if (employeeList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < employeeList.Count))
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
						if (lastIndex >= employeeList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = employeeList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

						for (int i = startIndex; i < lastIndex; i++)
						{
							EmployeeTO employee = employeeList[i];
							ListViewItem item = new ListViewItem();

							item.Text = employee.FirstName.Trim();
							item.SubItems.Add(employee.LastName.Trim());

							// Get Working Unit name for the particular user
                            //WorkingUnit wu = new WorkingUnit();
                            //if (wu.Find(employee.WorkingUnitID))
                            //{
                            //    item.SubItems.Add(wu.Name.Trim());
                            //}

                            item.SubItems.Add(employee.WorkingUnitName.Trim());
							item.Tag = employee;

							lvEmployees.Items.Add(item);
						}
					}
				}

				lvEmployees.EndUpdate();
				lvEmployees.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.btnPrev_Click(): " + ex.Message + "\n");
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
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployees_ColumnClick(object sender, ColumnClickEventArgs e)
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

                currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        #region Inner Class for sorting Array List of Employees

        /*
		 *  Class used for sorting Array List of Employees
		*/

        private class ArrayListSort : IComparer<EmployeeTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(EmployeeTO x, EmployeeTO y)
            {
                EmployeeTO empl1 = null;
                EmployeeTO empl2 = null;

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
                    case ExitPermissionsAddAdvanced.FirstNameIndex:
                        return empl1.FirstName.CompareTo(empl2.FirstName);
                    case ExitPermissionsAddAdvanced.LastNameIndex:
                        return empl1.LastName.CompareTo(empl2.LastName);
                    case ExitPermissionsAddAdvanced.WorkingUnitIDIndex:
                        return empl1.WorkingUnitName.CompareTo(empl2.WorkingUnitName);
                    default:
                        return empl1.LastName.CompareTo(empl2.LastName);
                }
            }
        }

        #endregion

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
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbPeriod_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbPeriod.Checked)
                {
                    lblFrom.Enabled = true;
                    dtpFrom.Enabled = true;
                    lblTo.Enabled = true;
                    dtpTo.Enabled = true;

                    lvDays.Enabled = false;
                    lblRemoveDay.Enabled = false;

                    lvDays.Items.Clear();
                }
                else
                {
                    lblFrom.Enabled = false;
                    dtpFrom.Enabled = false;
                    lblTo.Enabled = false;
                    dtpTo.Enabled = false;

                    lvDays.Enabled = true;
                    lblRemoveDay.Enabled = true;
                    ExitPermDaysSelection exitPermDaysSel = new ExitPermDaysSelection(this);
                    exitPermDaysSel.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.rbPeriod_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvDays_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.lvDays.BeginUpdate();
                this.lvDays.Items.Remove(this.lvDays.SelectedItems[0]);
                this.lvDays.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.lvDays_DoubleClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool isValid = validate();

                if (isValid)
                {
                    if (rbPeriod.Checked)
                    {
                        ExitPermissionsPreview preview = new ExitPermissionsPreview(selectedEmployees, dtpFrom.Value.Date, dtpTo.Value.Date, (int)cbPassType.SelectedValue, Sorting, ptArray, tbDescription.Text);
                        if (preview.currentPosition == ExitPermissionsPreview.startPosition)
                        {
                            MessageBox.Show(rm.GetString("noHolesFound", culture));
                        }
                        else
                        {
                            preview.ShowDialog();
                        }
                    }
                    else
                    {
                        if (lvDays.Items.Count == 0)
                        {
                            MessageBox.Show(rm.GetString("chooseDates", culture));
                        }
                        else
                        {
                            List<DateTime> dates = new List<DateTime>();
                            foreach (ListViewItem item in lvDays.Items)
                            {
                                dates.Add((DateTime)item.Tag);
                            }
                            ExitPermissionsPreview preview = new ExitPermissionsPreview(selectedEmployees, dates, (int)cbPassType.SelectedValue, Sorting, ptArray, tbDescription.Text);
                            if (preview.currentPosition == ExitPermissionsPreview.startPosition)
                            {
                                MessageBox.Show(rm.GetString("noHolesFound", culture));
                            }
                            else
                            {
                                preview.ShowDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.btnPreview_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool validate()
        {
            bool isValid = true;

            try
            {
                if ( dtpTo.Value.Date>DateTime.Now.Date&&rbPeriod.Checked)
                {
                    MessageBox.Show(rm.GetString("dateInFuture", culture));
                    return false;
                }
                if (dtpFrom.Value.Date>dtpTo.Value.Date&&rbPeriod.Checked)
                {
                    MessageBox.Show(rm.GetString("dateFromBigerThanTO", culture));
                    return false;
                }
                if (lvEmployees.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("exitPermEmployeeNotNull", culture));
                    return false;
                }
                if (cbPassType.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("exitPermPassTypeNotNull", culture));
                    return false;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsPreview.validate(): " + ex.Message + "\n");
                isValid = false;
                throw new Exception(ex.Message);
            }

            return isValid;
        }

        private void lvEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                selectedEmployees = new List<EmployeeTO>();
                foreach (ListViewItem item in lvEmployees.SelectedItems)
                {
                    selectedEmployees.Add((EmployeeTO)item.Tag);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.lvEmployees_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbEmplSort_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbEmplSort.Checked)
                {
                    Sorting = employeesSorting;
                }
                else
                {
                    Sorting = daysSorting;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.rbEmplSort_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ArrayList daysList = new ArrayList();
                foreach (ListViewItem item in lvDays.Items)
                {
                    daysList.Add((DateTime)item.Tag); 
                }
                ExitPermDaysSelection daysSel = new ExitPermDaysSelection(this, daysList);
                daysSel.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.rbtnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ExitPermissionsAddAdvanced_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.ExitPermissionsAddAdvanced_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);
                if (cbWU.SelectedIndex <= 0)
                {

                    currentEmployeesList = new Employee().SearchByWUWithStatuses(wuString, statuses);
                }
                else
                {
                    string wunits = "";
                    WorkingUnit wu = new WorkingUnit();
                    List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();

                    wuList.Add((WorkingUnitTO)cbWU.SelectedItem);

                    if (chbHierarhicly.Checked)
                    {
                        wuList = wu.FindAllChildren(wuList);
                    }

                    foreach (WorkingUnitTO workingUnit in wuList)
                    {
                        wunits += workingUnit.WorkingUnitID + ",";
                    }

                    if (wunits.Length > 0)
                    {
                        wunits = wunits.Substring(0, wunits.Length - 1);
                    }
                    currentEmployeesList = new Employee().SearchByWU(wunits);
                }

                currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsAddAdvanced.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            } 
        }

    }
}