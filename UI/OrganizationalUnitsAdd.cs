using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using Util;
using System.Globalization;
using System.Resources;
using Common;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace UI
{
    public partial class OrganizationalUnitsAdd : Form
    {
        OrganizationalUnitTO currentOU = new OrganizationalUnitTO();
        ApplUserXOrgUnitTO currentApplUsersXOU = new ApplUserXOrgUnitTO();
        int wuID = -1;

        DebugLog log;

        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;

        List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();
        
        string messageWUSave1 = "";
        string messageWUSave2 = "";
        string messageWUSave3 = "";
        string messageWUSave4 = "";
        string messageWUUpd3 = "";
        string messageWUUpd4 = "";
        
        public OrganizationalUnitsAdd(OrganizationalUnitTO ou)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentOU = ou;

			logInUser = NotificationController.GetLogInUser();
            			
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(OrganizationalUnitsAdd).Assembly);
			
			setLanguage();
		}

        private void OrganizationalUnitsAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                populateParentUnit();
                populateWU();

                if (currentOU.OrgUnitID != -1)
                {
                    this.tbOUID.Text = currentOU.OrgUnitID.ToString();
                    this.tbName.Text = currentOU.Name.ToString();
                    this.tbDescription.Text = currentOU.Desc.ToString();
                    this.tbCode.Text = currentOU.Code.Trim();
                    this.cbParentUnitID.SelectedValue = currentOU.ParentOrgUnitID;

                    WorkingUnitXOrganizationalUnit wu = new WorkingUnitXOrganizationalUnit();
                    wu.WUXouTO.OrgUnitID = currentOU.OrgUnitID;
                    List<WorkingUnitXOrganizationalUnitTO> list = wu.Search();
                    if (list.Count > 0)
                    {
                        this.cbWU.SelectedValue = list[0].WorkingUnitID;
                        wuID = list[0].WorkingUnitID;
                    }

                    tbOUID.Enabled = false;
                    //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                    /*
                    btnSave.Visible = false;
                    lblStar1.Visible = false;
                    tbOUID.Enabled = false;
                    tbCode.Enabled = false;
                    tbDescription.Enabled = false;
                    tbName.Enabled = false;
                    cbParentUnitID.Enabled = false;
                    cbWU.Enabled = false;
                    btnOUTree.Enabled = false;
                    bntWUTree.Enabled = false;
                    btnUpdate.Enabled = false;
                     * */
                }
                else
                {
                    tbOUID.Text = (new OrganizationalUnit().FindMAXOUID() + 1).ToString().Trim();
                    btnUpdate.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.OrganizationalUnitsAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void populateWU()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new WorkingUnit().Search();

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OrganizationalUnitsAdd.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateParentUnit()
        {
            try
            {
                OrganizationalUnit oUnit = new OrganizationalUnit();
                oUnit.OrgUnitTO.Status = Constants.DefaultStateActive;
                List<OrganizationalUnitTO> orgUnits = oUnit.Search();
                ouArray = new List<OrganizationalUnitTO>();
                List<int> ouIDs = new List<int>(); // children of selected unit
                if (currentOU.OrgUnitID != -1)
                {
                    List<OrganizationalUnitTO> oUnits = new List<OrganizationalUnitTO>();
                    oUnits.Add(currentOU);
                    oUnits = new OrganizationalUnit().FindAllChildren(oUnits);
                    if (oUnits.Count > 0)
                    {
                        foreach (OrganizationalUnitTO ou in oUnits)
                        {
                            if (ou.OrgUnitID != currentOU.OrgUnitID)
                                ouIDs.Add(ou.OrgUnitID);
                        }
                    }
                }
                OrganizationalUnitTO unit = new OrganizationalUnitTO();
                unit.OrgUnitID = -1;
                unit.Name = rm.GetString("all", culture);
                // Add All as a first member of combo
                ouArray.Insert(0, unit);

                foreach (OrganizationalUnitTO ouMember in orgUnits)
                {
                    if (!ouIDs.Contains(ouMember.OrgUnitID))
                    {
                        if (ouMember.OrgUnitID == 0)
                        {
                            ouArray.Insert(1, ouMember);
                        }
                        else
                        {
                            if (currentOU.OrgUnitID != 0)
                            {
                                if ((ouMember.ParentOrgUnitID != currentOU.OrgUnitID) || (ouMember.OrgUnitID == 0))
                                {
                                    ouArray.Add(ouMember);
                                }
                                else
                                {
                                    if (ouMember.OrgUnitID == currentOU.OrgUnitID)
                                    {
                                        ouArray.Add(ouMember);
                                    }
                                }
                            }
                            else
                            {
                                ouArray.Add(ouMember);
                            }
                        }
                    }
                }

                cbParentUnitID.DataSource = ouArray;
                cbParentUnitID.DisplayMember = "Name";
                cbParentUnitID.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.populateParentUnit(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                if (currentOU.OrgUnitID != -1)
                {
                    this.Text = rm.GetString("updateOU", culture);
                }
                else
                {
                    this.Text = rm.GetString("addOU", culture);
                }

                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // label's text
                lblOrgUnitID.Text = rm.GetString("lblID", culture);
                lblParentOUID.Text = rm.GetString("lblParentOUID", culture);
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblName.Text = rm.GetString("lblName", culture);
                lblCode.Text = rm.GetString("lblCode", culture);
                lblWUID.Text = rm.GetString("lblWU", culture);

                // message's text
                messageWUSave1 = rm.GetString("messageOUSave1", culture);
                messageWUSave2 = rm.GetString("messageOUSave2", culture);
                messageWUSave3 = rm.GetString("messageOUSave3", culture);
                messageWUSave4 = rm.GetString("messageOUSave4", culture);
                messageWUUpd3 = rm.GetString("messageOUUpd3", culture);
                messageWUUpd4 = rm.GetString("messageOUUpd4", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tbOUID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(messageWUSave1);
                    return;
                }
                else
                {
                    try
                    {
                        if (!tbOUID.Text.Trim().Equals(""))
                        {
                            Int32.Parse(tbOUID.Text.Trim());
                        }
                    }
                    catch
                    {
                        MessageBox.Show(messageWUSave2);
                        return;
                    }
                }

                this.Cursor = Cursors.WaitCursor;

                OrganizationalUnit oUnit = new OrganizationalUnit();
                oUnit.OrgUnitTO.OrgUnitID = Int32.Parse(this.tbOUID.Text.Trim());
                if (oUnit.Search().Count > 0)
                {
                    MessageBox.Show(rm.GetString("ouIDExists", culture));
                    return;
                }

                oUnit.OrgUnitTO = new OrganizationalUnitTO();
                oUnit.OrgUnitTO.Name = tbName.Text.Trim();
                List<OrganizationalUnitTO> ounits = oUnit.Search();

                if (ounits.Count == 0)
                {
                    currentOU = new OrganizationalUnitTO();
                    currentApplUsersXOU = new ApplUserXOrgUnitTO();
                    ApplUsersXRole admin = new ApplUsersXRole();
                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit();

                    //Set currentOU properties if all data validations were passed
                    currentOU.OrgUnitID = Int32.Parse(this.tbOUID.Text.Trim());
                    if (cbWU.SelectedIndex >= 0)
                    {
                        wuXou.WUXouTO.OrgUnitID = currentOU.OrgUnitID;
                        wuXou.WUXouTO.WorkingUnitID = (int)cbWU.SelectedValue;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("ouNoWUSelected", culture));
                        return;
                    }
                    int parentOU = currentOU.OrgUnitID;
                    if (cbParentUnitID.SelectedIndex > 0)
                    {
                        parentOU = Int32.Parse(this.cbParentUnitID.SelectedValue.ToString().Trim());
                    }
                    currentOU.ParentOrgUnitID = parentOU;
                    currentOU.Name = tbName.Text.Trim();
                    currentOU.Desc = tbDescription.Text.Trim();
                    if (tbCode.Text.Trim() == "")
                        currentOU.Code = "0000";
                    else
                        currentOU.Code = tbCode.Text.Trim();
                    currentOU.Status = Constants.DefaultStateActive.ToString();

                    List<ApplUserXOrgUnitTO> applUsersXOU = new List<ApplUserXOrgUnitTO>();
                    ApplUserXOrgUnit auXou = new ApplUserXOrgUnit();

                    if (cbParentUnitID.SelectedIndex > 0)
                    {
                        auXou.AuXOUnitTO.OrgUnitID = parentOU;
                        applUsersXOU = auXou.Search();
                    }

                    List<ApplUserTO> AdminUserList = new List<ApplUserTO>();
                    AdminUserList = admin.FindUsersForRoleID(0);

                    // Save currentOU
                    OrganizationalUnit ou = new OrganizationalUnit();
                    if (ou.BeginTransaction())
                    {
                        try
                        {
                            ou.OrgUnitTO = currentOU;
                            bool saved = ou.Save(false) > 0;
                            if (saved)
                            {
                                // save connection with working unit
                                wuXou.SetTransaction(ou.GetTransaction());
                                saved = saved && wuXou.Save(wuXou.WUXouTO, false) > 0;

                                if (saved)
                                {
                                    auXou.SetTransaction(ou.GetTransaction());
                                    //if user have the right to see parent ou for some purpose, give him
                                    //the right to see this ou for the same purpose
                                    if (cbParentUnitID.SelectedIndex > 0)
                                    {
                                        foreach (ApplUserXOrgUnitTO userOU in applUsersXOU)
                                        {
                                            int insertedXOU = auXou.Save(userOU.UserID, currentOU.OrgUnitID, userOU.Purpose, false);
                                            if (insertedXOU == 0)
                                            {
                                                MessageBox.Show(rm.GetString("roleInsertingProblem", culture) + " " + userOU.Purpose +
                                                        " " + rm.GetString("roleInsertingProblem1", culture) + " " + userOU.UserID);
                                                saved = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Give everybody from Admin Role rights to see this OU for all purposes
                                        //Do it only if OU don't have parent ou, otherwise, it is already done in if part
                                        List<string> purposeValues = new List<string>();
                                        purposeValues.Add(Constants.ReportPurpose);
                                        purposeValues.Add(Constants.PassPurpose);
                                        purposeValues.Add(Constants.IOPairPurpose);
                                        purposeValues.Add(Constants.LocationPurpose);
                                        purposeValues.Add(Constants.PermissionPurpose);
                                        purposeValues.Add(Constants.AbsencesPurpose);
                                        purposeValues.Add(Constants.EmployeesPurpose);
                                        purposeValues.Add(Constants.ExtraHoursPurpose);
                                        purposeValues.Add(Constants.PermVerificationPurpose);
                                        purposeValues.Add(Constants.RestaurantPurpose);
                                        purposeValues.Add(Constants.VacationPurpose);

                                        foreach (ApplUserTO user in AdminUserList)
                                        {
                                            foreach (string purpose in purposeValues)
                                            {
                                                int insertedXOU = auXou.Save(user.UserID, currentOU.OrgUnitID, purpose, false);
                                                if (insertedXOU == 0)
                                                {
                                                    MessageBox.Show(rm.GetString("roleInsertingProblem", culture) + " " + purpose +
                                                        " " + rm.GetString("roleInsertingProblem1", culture) + " " + user.UserID);
                                                    saved = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            } // if (saved)

                            if (saved)
                            {
                                ou.CommitTransaction();
                                MessageBox.Show(messageWUSave4);
                                this.Close();
                            }
                            else
                            {
                                if (ou.GetTransaction() != null)
                                    ou.RollbackTransaction();

                                MessageBox.Show(rm.GetString("ouNotSaved", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ou.GetTransaction() != null)
                                ou.RollbackTransaction();

                            throw ex;
                        }
                    }
                    else
                        MessageBox.Show(rm.GetString("ouNotSaved", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("ouNameExists", culture));
                }
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    MessageBox.Show(rm.GetString("messageOUIDExists", culture));
                }
                else
                {
                    MessageBox.Show(sqlex.Message);
                    log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.btnSave_Click(): " + sqlex.Message + "\n");
                }
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    MessageBox.Show(rm.GetString("messageOUIDExists", culture));
                }
                else
                {
                    MessageBox.Show(mysqlex.Message);
                    log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.btnSave_Click(): " + mysqlex.Message + "\n");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits("");
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits("");
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbParentUnitID.SelectedIndex = cbParentUnitID.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string oldOUName = currentOU.Name;
                int oldParentOUID = currentOU.ParentOrgUnitID;
                
                this.Cursor = Cursors.WaitCursor;
                
                if (!oldOUName.Equals(tbName.Text.Trim()))
                {
                    OrganizationalUnit oUnit = new OrganizationalUnit();
                    oUnit.OrgUnitTO.Name = tbName.Text.Trim();
                    List<OrganizationalUnitTO> ounits = oUnit.Search();

                    if (ounits.Count > 0)
                    {
                        MessageBox.Show(rm.GetString("ouNameExists", culture));
                        return;
                    }
                }

                bool isUpdated = false;
                //currentOU = new WorkingUnit();

                WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit();

                //Set currentOU properties if all data validations were passed
                currentOU.OrgUnitID = Int32.Parse(this.tbOUID.Text.Trim());

                if (cbParentUnitID.SelectedIndex > 0)
                {
                    currentOU.ParentOrgUnitID = Int32.Parse(this.cbParentUnitID.SelectedValue.ToString().Trim());
                }

                List<WorkingUnitXOrganizationalUnitTO> wuList = new List<WorkingUnitXOrganizationalUnitTO>();
                if (cbWU.SelectedIndex >= 0)
                {
                    if ((int)cbWU.SelectedValue != wuID)
                    {
                        wuXou.WUXouTO.OrgUnitID = currentOU.OrgUnitID;
                        wuList = wuXou.Search();
                        wuXou.WUXouTO.WorkingUnitID = (int)cbWU.SelectedValue;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("ouNoWUSelected", culture));
                    return;
                }               
                
                currentOU.Name = tbName.Text.Trim();
                currentOU.Desc = tbDescription.Text.Trim();
                if (tbCode.Text.Trim() == "")
                    currentOU.Code = "0000";
                else
                    currentOU.Code = tbCode.Text.Trim();                
                currentOU.Status = Constants.DefaultStateActive;

                ApplUsersXRole admin = new ApplUsersXRole();
                List<ApplUserTO> AdminUserList = new List<ApplUserTO>();
                AdminUserList = admin.FindUsersForRoleID(0);

                ApplUserXOrgUnit auXou = new ApplUserXOrgUnit();
                List<ApplUserXOrgUnitTO> applUsersXOUParentOld = new List<ApplUserXOrgUnitTO>();
                List<ApplUserXOrgUnitTO> applUsersXOUParent = new List<ApplUserXOrgUnitTO>();
                List<ApplUserXOrgUnitTO> applUsersXOUChild = new List<ApplUserXOrgUnitTO>();
                if (oldParentOUID != currentOU.ParentOrgUnitID)
                {
                    //delete all right from previous parent
                    //only if WU was not a parent to itself
                    if (oldParentOUID != currentOU.OrgUnitID)
                    {
                        auXou.AuXOUnitTO.OrgUnitID = oldParentOUID;
                        applUsersXOUParentOld = auXou.Search();                        
                    }

                    auXou.AuXOUnitTO.OrgUnitID = currentOU.OrgUnitID;
                    applUsersXOUChild = auXou.Search();

                    auXou.AuXOUnitTO.OrgUnitID = currentOU.ParentOrgUnitID;
                    applUsersXOUParent = auXou.Search();
                }

                OrganizationalUnit ou = new OrganizationalUnit();
                ou.OrgUnitTO = currentOU;
                if (ou.BeginTransaction())
                {
                    try
                    {
                        if (isUpdated = ou.Update(false))
                        {
                            wuXou.SetTransaction(ou.GetTransaction());
                            if (wuList.Count > 0)
                            {
                                foreach (WorkingUnitXOrganizationalUnitTO wu in wuList)
                                {
                                    isUpdated = isUpdated && wuXou.Delete(wu.OrgUnitID, wu.WorkingUnitID, false);

                                    if (!isUpdated)
                                        break;
                                }
                            }

                            if (wuXou.WUXouTO.OrgUnitID != -1)
                                isUpdated = isUpdated && wuXou.Save(wuXou.WUXouTO, false) > 0;

                            if (isUpdated)
                            {
                                List<ApplUserXOrgUnitTO> deletedRights = new List<ApplUserXOrgUnitTO>();
                                //if new parent is different than the old parrent
                                if (oldParentOUID != currentOU.ParentOrgUnitID)
                                {
                                    //delete all right from previous parent
                                    //only if WU was not a parent to itself
                                    auXou.AuXOUnitTO = new ApplUserXOrgUnitTO();
                                    auXou.SetTransaction(ou.GetTransaction());
                                    if (oldParentOUID != currentOU.OrgUnitID)
                                    {
                                        foreach (ApplUserXOrgUnitTO userOUP in applUsersXOUParentOld)
                                        {
                                            foreach (ApplUserXOrgUnitTO userOUC in applUsersXOUChild)
                                            {
                                                if ((userOUP.UserID == userOUC.UserID) && (userOUP.Purpose == userOUC.Purpose))
                                                {
                                                    bool delete = auXou.Delete(userOUP.UserID, currentOU.OrgUnitID, userOUP.Purpose, false);

                                                    ApplUserXOrgUnitTO deletedPrivilege = new ApplUserXOrgUnitTO();
                                                    deletedPrivilege.OrgUnitID = currentOU.OrgUnitID;
                                                    deletedPrivilege.Purpose = userOUP.Purpose;
                                                    deletedPrivilege.UserID = userOUP.UserID;
                                                    deletedRights.Add(deletedPrivilege);

                                                    if (!delete)
                                                    {
                                                        MessageBox.Show(rm.GetString("ouRightsDeletingProblem", culture) + " " + userOUP.Purpose +
                                                                " " + rm.GetString("ouRightsDeletingProblem1", culture) + " " + userOUP.UserID);
                                                        isUpdated = false;
                                                        break;
                                                    }

                                                }
                                            }

                                            if (!isUpdated)
                                                break;
                                        }
                                    }

                                    if (isUpdated)
                                    {
                                        //give all rights from new parent
                                        if (currentOU.ParentOrgUnitID != currentOU.OrgUnitID)
                                        {
                                            foreach (ApplUserXOrgUnitTO userOUP in applUsersXOUParent)
                                            {
                                                bool notExists = true;
                                                foreach (ApplUserXOrgUnitTO userOUC in applUsersXOUChild)
                                                {
                                                    if ((userOUP.UserID == userOUC.UserID) && (userOUP.Purpose == userOUC.Purpose))
                                                    {
                                                        // check if it is deleted
                                                        bool deleted = false;
                                                        foreach (ApplUserXOrgUnitTO deletedOU in deletedRights)
                                                        {
                                                            if (deletedOU.UserID == userOUC.UserID && deletedOU.Purpose == userOUC.Purpose)
                                                            {
                                                                deleted = true;
                                                                break;
                                                            }
                                                        }

                                                        if (!deleted)
                                                        {
                                                            notExists = false;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (notExists)
                                                {
                                                    int insertedXOU = auXou.Save(userOUP.UserID, currentOU.OrgUnitID, userOUP.Purpose, false);
                                                    if (insertedXOU == 0)
                                                    {
                                                        MessageBox.Show(rm.GetString("roleInsertingProblem", culture) + " " + userOUP.Purpose +
                                                                " " + rm.GetString("roleInsertingProblem1", culture) + " " + userOUP.UserID);
                                                        isUpdated = false;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //return rights for admin role users
                                            List<string> purposeValues = new List<string>();
                                            purposeValues.Add(Constants.ReportPurpose);
                                            purposeValues.Add(Constants.PassPurpose);
                                            purposeValues.Add(Constants.IOPairPurpose);
                                            purposeValues.Add(Constants.LocationPurpose);
                                            purposeValues.Add(Constants.PermissionPurpose);
                                            purposeValues.Add(Constants.AbsencesPurpose);
                                            purposeValues.Add(Constants.EmployeesPurpose);
                                            purposeValues.Add(Constants.ExtraHoursPurpose);
                                            purposeValues.Add(Constants.PermVerificationPurpose);
                                            purposeValues.Add(Constants.RestaurantPurpose);
                                            purposeValues.Add(Constants.VacationPurpose);
                                            foreach (ApplUserTO user in AdminUserList)
                                            {
                                                foreach (string purpose in purposeValues)
                                                {
                                                    bool notExists = true;
                                                    foreach (ApplUserXOrgUnitTO userOUC in applUsersXOUChild)
                                                    {
                                                        if ((user.UserID == userOUC.UserID) && (purpose == userOUC.Purpose))
                                                        {
                                                            // check if it is deleted
                                                            bool deleted = false;
                                                            foreach (ApplUserXOrgUnitTO deletedOU in deletedRights)
                                                            {
                                                                if (deletedOU.UserID == userOUC.UserID && deletedOU.Purpose == userOUC.Purpose)
                                                                {
                                                                    deleted = true;
                                                                    break;
                                                                }
                                                            }

                                                            if (!deleted)
                                                            {
                                                                notExists = false;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (notExists)
                                                    {
                                                        int insertedXOU = auXou.Save(user.UserID, currentOU.OrgUnitID, purpose, false);
                                                        if (insertedXOU == 0)
                                                        {
                                                            MessageBox.Show(rm.GetString("roleInsertingProblem", culture) + " " + purpose +
                                                                " " + rm.GetString("roleInsertingProblem1", culture) + " " + user.UserID);
                                                            isUpdated = false;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (!isUpdated)
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (isUpdated)
                        {
                            ou.CommitTransaction();
                            MessageBox.Show(messageWUUpd3);
                            this.Close();
                        }
                        else
                        {
                            if (ou.GetTransaction() != null)
                                ou.RollbackTransaction();

                            MessageBox.Show(rm.GetString("ouNotUpdated", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ou.GetTransaction() != null)
                            ou.RollbackTransaction();

                        throw ex;
                    }
                }
                else
                    MessageBox.Show(rm.GetString("ouNotUpdated", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnitsAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
