using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SiemensDataAccess;
using System.Globalization;
using System.Resources;

using TransferObjects;
using Util;

namespace SiemensPasses
{
    public partial class SiemensPasses : Form
    {
        BrezaDAO brezaDAO = null;
        Object brezaConn = null;
        SiemensDAO siemensDAO = null;
        Object siemensConn = null;
        Dictionary<string, SiemensEmployeeTO> emplDict = new Dictionary<string, SiemensEmployeeTO>();
        
        List<SiemensLogTO> currentPassList = new List<SiemensLogTO>();
        int startIndex = 0;

        CultureInfo culture = null;
        ResourceManager rm = null;

        Dictionary<string, string> directionDict = new Dictionary<string, string>();
                        
        // List View indexes
        const int IDIndex = 0;
        const int LastNameIndex = 1;
        const int NameIndex = 2;
        const int TimeIndex = 3;
        const int DirectionIndex = 4;
        const int LocationIndex = 5;
        const int TypeIndex = 6;
        
        private int sortOrder;
        private int sortField;

        public SiemensPasses()
        {
            try
            {
                InitializeComponent();

                culture = CultureInfo.CreateSpecificCulture(Constants.Lang_sr);
                rm = new ResourceManager("SiemensPasses.Resource", typeof(SiemensPasses).Assembly);

                setLanguage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Inner Class for sorting Array List of Passes

        /*
		 *  Class used for sorting Array List of Employees
		*/

        private class ArrayListSort : IComparer<SiemensLogTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(SiemensLogTO x, SiemensLogTO y)
            {
                SiemensLogTO pass1 = null;
                SiemensLogTO pass2 = null;

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
                
                switch (compField)
                {
                    case SiemensPasses.IDIndex:
                        return pass1.Id.CompareTo(pass2.Id);
                    case SiemensPasses.NameIndex:
                        return pass1.Name.CompareTo(pass2.Name);
                    case SiemensPasses.LastNameIndex:
                        return pass1.LastName.CompareTo(pass2.LastName);
                    case SiemensPasses.TimeIndex:
                        return pass1.RegTime.CompareTo(pass2.RegTime);
                    case SiemensPasses.DirectionIndex:
                        return pass1.Direction.CompareTo(pass2.Direction);
                    case SiemensPasses.LocationIndex:
                        return pass1.RegLoc.CompareTo(pass2.RegLoc);
                    case SiemensPasses.TypeIndex:
                        return pass1.TypeID.CompareTo(pass2.TypeID);
                    default:
                        return pass1.LastName.CompareTo(pass2.LastName);
                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                // form text
                this.Text = rm.GetString("SiemensPasses", culture);

                // tab page text
                tabAdd.Text = rm.GetString("tabAdd", culture);
                tabSearch.Text = rm.GetString("tabSearch", culture);

                // label text
                lblDirection.Text = rm.GetString("lblDirection", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblID.Text = rm.GetString("lblID", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblCreated.Text = rm.GetString("lblCreated", culture);
                lblRemark.Text = rm.GetString("lblRemark", culture);
                
                // btn text
                btnAddIN.Text = rm.GetString("btnAddIN", culture);
                btnAddOut.Text = rm.GetString("btnAddOUT", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnExport.Text = rm.GetString("btnExport", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);

                // group box text
                gbEmployee.Text = rm.GetString("gbEmployee", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                gbTime.Text = rm.GetString("gbTime", culture);
                gbRemark.Text = rm.GetString("gbRemark", culture);

                lblTotal.Text = "";

                lvPasses.BeginUpdate();
                lvPasses.Columns.Add(rm.GetString("hdrID", culture), 50, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrLastName", culture), 110, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrFirstName", culture), 90, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrTime", culture), 120, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrDirection", culture), 80, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrCreated", culture), 100, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrType", culture), 50, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrRemark", culture), 70, HorizontalAlignment.Left);
                lvPasses.EndUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SiemensPasses_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ConnectBreza();
                ConnectSiemens();

                if (brezaDAO == null || siemensDAO == null)
                {
                    MessageBox.Show(rm.GetString("noDBConnection", culture));
                    btnSearch.Enabled = btnAddIN.Enabled = btnAddOut.Enabled = false;
                    return;
                }
                else
                {
                    btnSearch.Enabled = btnAddIN.Enabled = btnAddOut.Enabled = true;

                    directionDict.Add(rm.GetString("IN", culture), Constants.SiemensRegDirectionIn.Trim());
                    directionDict.Add(rm.GetString("OUT", culture), Constants.SiemensRegDirectionOut.Trim());

                    populateEmployees();
                    populateDirection();
                    populateCreated();

                    dtpFrom.Value = DateTime.Now.Date;
                    dtpTo.Value = DateTime.Now.Date;
                }

                sortOrder = Constants.sortAsc;
                sortField = LastNameIndex;

                btnNext.Visible = btnPrev.Visible = false;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void populateCreated()
        {
            try
            {
                List<string> createdList = new List<string>();
                createdList.Add("*");
                createdList.Add(rm.GetString("manualy", culture));
                createdList.Add(rm.GetString("automaticaly", culture));
                cbCreated.DataSource = createdList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateDirection()
        {
            try
            {
                List<string> directionList = new List<string>();
                directionList.Add("*");
                foreach (string direction in directionDict.Keys)
                {
                    directionList.Add(direction.Trim());
                }

                cbDirection.DataSource = directionList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateEmployees()
        {
            try
            {
                emplDict = new Dictionary<string, SiemensEmployeeTO>();//siemensDAO.getEmployeesNonVisitors();

                SiemensEmployeeTO emplAll = new SiemensEmployeeTO();
                emplAll.LastName = "*";

                List<SiemensEmployeeTO> emplList = new List<SiemensEmployeeTO>();
                List<SiemensEmployeeTO> idList = new List<SiemensEmployeeTO>();
                List<SiemensEmployeeTO> firstNameList = new List<SiemensEmployeeTO>();
                List<SiemensEmployeeTO> lastNameList = new List<SiemensEmployeeTO>();

                emplList.Add(new SiemensEmployeeTO(emplAll));
                idList.Add(new SiemensEmployeeTO(emplAll));
                firstNameList.Add(new SiemensEmployeeTO(emplAll));
                lastNameList.Add(new SiemensEmployeeTO(emplAll));
                
                foreach (string id in emplDict.Keys)
                {
                    emplList.Add(new SiemensEmployeeTO(emplDict[id]));
                    idList.Add(new SiemensEmployeeTO(emplDict[id]));
                    firstNameList.Add(new SiemensEmployeeTO(emplDict[id]));
                    lastNameList.Add(new SiemensEmployeeTO(emplDict[id]));
                }

                cbEmployee.DataSource = emplList;
                cbEmployee.DisplayMember = "FirstAndLastName";
                cbEmployee.ValueMember = "ID";

                cbID.DataSource = idList;
                cbID.DisplayMember = "IDLastNameFirstName";
                cbID.ValueMember = "ID";

                cbFirstName.DataSource = firstNameList;
                cbFirstName.DisplayMember = "FirstNameLastNameID";
                cbFirstName.ValueMember = "ID";

                cbLastName.DataSource = lastNameList;
                cbLastName.DisplayMember = "LastNameFirstNameID";
                cbLastName.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setEmployeeData(ComboBox cb)
        {
            try
            {
                SiemensEmployeeTO selEmpl = new SiemensEmployeeTO();
                if (cb.SelectedValue != null && cb.SelectedValue is string && cb.SelectedValue.ToString() != "*")
                {
                    if (emplDict.ContainsKey(cb.SelectedValue.ToString().Trim()))
                        selEmpl = emplDict[cb.SelectedValue.ToString()];
                }

                if (selEmpl.ID.Trim() != "")
                {
                    tbID.Text = selEmpl.ID.Trim().PadLeft(5, '0');
                    tbFirstName.Text = selEmpl.FirstName.Trim();
                    tbLastName.Text = selEmpl.LastName.Trim();
                }
                else
                    tbID.Text = tbFirstName.Text = tbLastName.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddIN_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SavePass(Constants.SiemensRegDirectionIn);
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

        private void btnAddOut_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SavePass(Constants.SiemensRegDirectionOut);
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

        private void SavePass(string direction)
        {
            try
            {
                if (tbID.Text.Trim() == "" || tbFirstName.Text.Trim() == "" || tbLastName.Text.Trim() == "")
                {
                    MessageBox.Show(rm.GetString("enterEmplData", culture));
                    return;
                }

                SiemensLogTO passTO = new SiemensLogTO();
                passTO.Direction = direction.Trim();
                passTO.Id = tbID.Text.Trim();
                passTO.LastName = tbLastName.Text.Trim();
                passTO.Name = tbFirstName.Text.Trim();
                passTO.RegLoc = Constants.SiemensManualCreatedLoc;
                passTO.TypeID = Constants.SiemensEmployeeType.Trim();
                passTO.ReadStatus = Constants.SiemensDefaultReadStatus;
                passTO.RegTime = dtpTime.Value;
                passTO.Col1 = tbRemark.Text.Trim();

                if (brezaDAO.insert(passTO, true) >0)
                    MessageBox.Show(rm.GetString("passSaved", culture));
                else
                    MessageBox.Show(rm.GetString("passNotSaved", culture));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ConnectBreza()
        {
            try
            {
                if (brezaDAO == null)
                    brezaDAO = BrezaDAO.getDAO();

                brezaConn = brezaDAO.MakeNewDBConnection();
                
                if (brezaConn == null)
                    return false;
                
                brezaDAO.SetDBConnection(brezaConn);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisconnectBreza()
        {
            try
            {
                if (brezaDAO != null)
                    brezaDAO.CloseConnection(brezaConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ConnectSiemens()
        {
            try
            {
                if (siemensDAO == null)
                    siemensDAO = SiemensDAO.getDAO();

                siemensConn = siemensDAO.MakeNewDBConnection();

                if (siemensDAO == null)
                    return false;

                siemensDAO.SetDBConnection(siemensConn);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisconnectSiemens()
        {
            try
            {
                if (siemensDAO != null)
                    siemensDAO.CloseConnection(siemensConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SiemensPasses_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DisconnectBreza();
                DisconnectSiemens();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbID_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEmployeeData(cbID);
        }

        private void cbFirstName_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEmployeeData(cbFirstName);
        }

        private void cbLastName_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEmployeeData(cbLastName);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                    return;
                }

                string emplID = "";
                if (cbEmployee.SelectedIndex > 0)
                    emplID = cbEmployee.SelectedValue.ToString();

                string direction = "";
                if (cbDirection.SelectedIndex > 0 && directionDict.ContainsKey(cbDirection.Text.Trim()))
                    direction = directionDict[cbDirection.Text.Trim()];

                string created = "";
                if (cbCreated.SelectedIndex > 0)
                {
                    if (cbCreated.Text == rm.GetString("manualy", culture))
                        created = "= '" + Constants.SiemensManualCreatedLoc.ToString().Trim() + "'";
                    else
                        created = "<> '" + Constants.SiemensManualCreatedLoc.ToString().Trim() + "'";
                }

                currentPassList = brezaDAO.getPasses(emplID, direction, created, tbRemarkSearch.Text.Trim(), dtpFrom.Value.Date, dtpTo.Value.Date);
                currentPassList.Sort(new ArrayListSort(sortOrder, sortField));

                startIndex = 0;
                populatePasses();
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

        private void populatePasses()
        {
            try
            {
                if (currentPassList.Count > Constants.recordsPerPage)
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
                lblTotal.Text = "";
                if (currentPassList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < currentPassList.Count))
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
                        if (lastIndex >= currentPassList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = currentPassList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            SiemensLogTO pass = currentPassList[i];
                            ListViewItem item = new ListViewItem();
                            item.Text = pass.Id.ToString().Trim();
                            item.SubItems.Add(pass.LastName.Trim());
                            item.SubItems.Add(pass.Name.Trim());                            
                            item.SubItems.Add(pass.RegTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                            item.SubItems.Add(getDirection(pass.Direction.Trim()));
                            item.SubItems.Add(getCreated(pass.RegLoc));
                            item.SubItems.Add(pass.TypeID.Trim());
                            item.SubItems.Add(pass.Col1.Trim());
                            item.Tag = pass;

                            lvPasses.Items.Add(item);
                        }
                    }

                    lblTotal.Text = rm.GetString("hdrTotal", culture) + " " + currentPassList.Count.ToString().Trim();
                }

                lvPasses.EndUpdate();
                lvPasses.Invalidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void bntPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)                
                    startIndex = 0;
                
                populatePasses();
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                startIndex += Constants.recordsPerPage;
                
                populatePasses();
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPassList.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                string delimiter = ",";

                // create header                
                string header = rm.GetString("hdrID", culture) + delimiter + rm.GetString("hdrLastName", culture) + delimiter + rm.GetString("hdrFirstName", culture) + delimiter 
                    + rm.GetString("hdrTime", culture) + delimiter + rm.GetString("hdrDirection", culture) + delimiter + rm.GetString("hdrCreated", culture)
                    + delimiter + rm.GetString("hdrType", culture) + delimiter + rm.GetString("hdrRemark", culture);

                List<string> lines = new List<string>();
                
                // create file lines
                foreach (SiemensLogTO pass in currentPassList)
                {
                    string line = pass.Id.Trim().Replace(delimiter, " ") + delimiter + pass.LastName.Trim().Replace(delimiter, " ") + delimiter + pass.Name.Trim().Replace(delimiter, " ") + delimiter 
                        + pass.RegTime.ToString(Constants.dateFormat + " " + Constants.timeFormat) + delimiter + getDirection(pass.Direction.Trim()).Replace(delimiter, " ")
                        + delimiter + getCreated(pass.RegLoc).Trim().Replace(delimiter, " ") + delimiter + pass.TypeID.Trim().Replace(delimiter, " ") + delimiter + pass.Col1.Trim().Replace(delimiter, " ");

                    lines.Add(line);
                }

                string reportName = "PassesReport_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");
                
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                sfd.Filter = "CSV (*.csv)|*.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string filePath = sfd.FileName;
                    sfd.Dispose();

                    FileStream stream = new FileStream(filePath, FileMode.Append);
                    stream.Close();

                    StreamWriter writer = new StreamWriter(filePath, true, Encoding.Unicode);
                    // insert header
                    writer.WriteLine(header);

                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    writer.Close();
                }
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

                currentPassList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populatePasses();
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

        private string getDirection(string dir)
        {
            try
            {
                string dirDisplayed = "";

                foreach (string direction in directionDict.Keys)
                {
                    if (directionDict[direction] == dir)
                    {
                        dirDisplayed = direction;
                        break;
                    }
                }

                return dirDisplayed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getCreated(int loc)
        {
            try
            {
                if (loc == Constants.SiemensManualCreatedLoc)
                    return rm.GetString("manualy", culture);
                else
                    return rm.GetString("automaticaly", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
