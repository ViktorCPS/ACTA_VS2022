using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Util;
using Common;
using TransferObjects;
using System.Collections;

namespace UI
{
    public partial class TMTypes : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        List<PassTypeTO> currentTypes = new List<PassTypeTO>();

        Dictionary<int, WorkingUnitTO> CompaniesDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, PassTypeTO> typesDict = new Dictionary<int, PassTypeTO>();
        Dictionary<int, PassTypeTO> allTypesDict = new Dictionary<int, PassTypeTO>();
        Dictionary<int, PassTypeLimitTO> limitDict = new Dictionary<int, PassTypeLimitTO>();
        Dictionary<int, ApplUserCategoryTO> categoryDict = new Dictionary<int, ApplUserCategoryTO>();
        Dictionary<int, List<int>> categoryXTypeDict = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> typeXConfirmDict = new Dictionary<int, List<int>>();
        List<int> selectedApplUserCategories = new List<int>();
        private ListViewItemComparer _comp;

        public TMTypes()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Set proper language
        /// </summary>
        private void setLanguage()
        {
            try
            {

                this.Text = rm.GetString("menuPassTypes", culture);

                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnClear.Text = rm.GetString("btnClear", culture);

                // group box text
                gbLimits.Text = rm.GetString("gbLimits", culture);
                this.gbMassiveInput.Text = rm.GetString("gbMassiveInput", culture);
                this.gbResults.Text = rm.GetString("gbResults", culture);
                this.gbSearch.Text = rm.GetString("gbSearch", culture);
                this.gbUserCategory.Text = rm.GetString("gbUserCategory", culture);
                this.gbVerification.Text = rm.GetString("gbVerification", culture);
                this.gbManualInput.Text = rm.GetString("gbManualInput", culture);

                // label's text
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblCompany.Text = rm.GetString("lblCompany", culture);
                lblDescAlternative.Text = rm.GetString("lblDescAlternative", culture);
                lblLimitComposite.Text = rm.GetString("lblLimitComposite", culture);
                lblLimitOccasional.Text = rm.GetString("lblLimitOccasional", culture);
                lblLimitPerType.Text = rm.GetString("lblLimitPerType", culture);
                lblPaymentCode.Text = rm.GetString("lblPayCode", culture);
                lblType.Text = rm.GetString("lblType", culture);

                //radio button's text
                rbMassiveInputYes.Text = rm.GetString("yes", culture);
                rbMassiveInputNo.Text = rm.GetString("no", culture);
                rbVerificationYes.Text = rm.GetString("yes", culture);
                rbVerificationNo.Text = rm.GetString("no", culture);
                rbManualInputYes.Text = rm.GetString("yes", culture);
                rbManualInputNo.Text = rm.GetString("no", culture);

                // list view initialization
                lvPassTypes.BeginUpdate();
                lvPassTypes.Columns.Add(rm.GetString("hdrDescripton", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrDescAlternative", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrType", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrPayCode", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrCompany", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrMassiveInput", culture), lvPassTypes.Width /12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrManualInput", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrConformaion", culture), lvPassTypes.Width / 12- 20, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrVerification", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrLimitComposite", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrLimitPerType", culture), lvPassTypes.Width / 12, HorizontalAlignment.Left);
                lvPassTypes.Columns.Add(rm.GetString("hdrLimitOccasional", culture), lvPassTypes.Width /12, HorizontalAlignment.Left);
                lvPassTypes.EndUpdate();

                lvUserCategory.BeginUpdate();
                lvUserCategory.Columns.Add(rm.GetString("hdrName", culture), lvUserCategory.Width / 2 - 5, HorizontalAlignment.Left);
                lvUserCategory.Columns.Add(rm.GetString("hdrDescription", culture), lvUserCategory.Width / 2 - 5, HorizontalAlignment.Left);
                lvUserCategory.EndUpdate();

                lvCategoryDetails.BeginUpdate();
                lvCategoryDetails.Columns.Add(rm.GetString("hdrCategory", culture), lvCategoryDetails.Width / 3 - 5, HorizontalAlignment.Left);
                lvCategoryDetails.Columns.Add(rm.GetString("hdrDescription", culture), lvCategoryDetails.Width / 3 - 5, HorizontalAlignment.Left);
                lvCategoryDetails.Columns.Add(rm.GetString("hdrPurpose", culture), lvCategoryDetails.Width / 3 - 5, HorizontalAlignment.Left);
                lvCategoryDetails.EndUpdate();
                
                lvConfirmationTypes.BeginUpdate();
                lvConfirmationTypes.Columns.Add(rm.GetString("gbAbsenceConformation", culture), lvConfirmationTypes.Width - 15, HorizontalAlignment.Left);
                lvConfirmationTypes.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateComboBox(ComboBox cb, string field)
        {
            try
            {
                List<string> list = new PassType().SearchDistinctField(field);
                cb.Items.Add("*");
                foreach (string s in list)
                {
                    cb.Items.Add(s);
                }
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateComboBox(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateCompanyCombo()
        {
            try
            {
                List<WorkingUnitTO> companies = new WorkingUnit().SearchRootWU("");

                foreach (WorkingUnitTO wu in companies)
                {
                    if (!CompaniesDict.ContainsKey(wu.WorkingUnitID))
                    {
                        CompaniesDict.Add(wu.WorkingUnitID, wu);
                    }
                }
                WorkingUnitTO wuTO = new WorkingUnitTO();
                wuTO.Name = rm.GetString("all", culture);
                companies.Insert(0, wuTO);

                cbCompany.DataSource = companies;
                cbCompany.DisplayMember = "Name";
                cbCompany.ValueMember = "WorkingUnitID";
                cbCompany.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateComboBox(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populateUserCategoryListView()
        {
            try
            {
                lvUserCategory.BeginUpdate();
                List<ApplUserCategoryTO> categories = new ApplUserCategory().Search(null);
                foreach (ApplUserCategoryTO category in categories)
                {
                    if (!categoryDict.ContainsKey(category.CategoryID))
                        categoryDict.Add(category.CategoryID, category);
                    ListViewItem item = new ListViewItem();
                    item.Text = category.Name;
                    item.SubItems.Add(category.Desc);
                    item.Tag = category;
                    lvUserCategory.Items.Add(item);
                }
                lvUserCategory.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateUserCategoryListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateTypeCategoryDetails()
        {
            try
            {
                lvCategoryDetails.BeginUpdate();
                lvCategoryDetails.Items.Clear();
                if (lvPassTypes.SelectedItems.Count == 1)
                {
                    PassTypeTO selectedType = (PassTypeTO)lvPassTypes.SelectedItems[0].Tag;
                    if (categoryXTypeDict.ContainsKey(selectedType.PassTypeID))
                    {
                        ApplUserCategoryXPassType category = new ApplUserCategoryXPassType();
                        category.UserCategoryXPassTypeTO.PassTypeID = selectedType.PassTypeID;
                        List<ApplUserCategoryXPassTypeTO> list = category.Search();
                        foreach (ApplUserCategoryXPassTypeTO categoryTO in list)
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = categoryDict[categoryTO.CategoryID].Name;
                            item.SubItems.Add(categoryDict[categoryTO.CategoryID].Desc);
                            item.SubItems.Add(categoryTO.Purpose);
                            item.Tag = categoryTO;
                            lvCategoryDetails.Items.Add(item);
                        }
                    }
                }
                lvCategoryDetails.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateUserCategoryListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateTypeConfirmDetails()
        {
            try
            {
                lvConfirmationTypes.BeginUpdate();
                lvConfirmationTypes.Items.Clear();
                if (lvPassTypes.SelectedItems.Count == 1)
                {
                    PassTypeTO selectedType = (PassTypeTO)lvPassTypes.SelectedItems[0].Tag;
                    if (typeXConfirmDict.ContainsKey(selectedType.PassTypeID))
                    {
                        PassTypesConfirmation category = new PassTypesConfirmation();
                        category.PTConfirmTO.PassTypeID = selectedType.PassTypeID;
                        List<PassTypesConfirmationTO> list = category.Search();
                        foreach (PassTypesConfirmationTO categoryTO in list)
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = allTypesDict[categoryTO.ConfirmationPassTypeID].Description;
                            item.Tag = categoryTO;
                            lvConfirmationTypes.Items.Add(item);
                        }
                    }
                }
                lvConfirmationTypes.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateUserCategoryListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateLimitCombo(ComboBox cb, string limitType)
        {
            try
            {
                PassTypeLimit limit = new PassTypeLimit();
                limit.PTLimitTO.Type = limitType;
                List<PassTypeLimitTO> limits = limit.Search();

                PassTypeLimitTO limitTO = new PassTypeLimitTO();
                limitTO.Name = rm.GetString("all", culture);
                limits.Insert(0, limitTO);

                cb.DataSource = limits;
                cb.DisplayMember = "Name";
                cb.ValueMember = "PtLimitID";
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateComboBox(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateConformationTypeCombo()
        {
            try
            {
                allTypesDict = new PassType().SearchDictionary();                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateComboBox(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateTypeCombo()
        {
            try
            {
                List<UI.PassTypeAdd.PassTypeValue> ptMemebers = new List<UI.PassTypeAdd.PassTypeValue>();

                UI.PassTypeAdd.PassTypeValue ptValue = new UI.PassTypeAdd.PassTypeValue(rm.GetString("all", culture), -1);
                ptMemebers.Add(ptValue);
                ptValue = new UI.PassTypeAdd.PassTypeValue(rm.GetString("isPassReader", culture), (int)Constants.PassTypeAll.PassOnReader);
                ptMemebers.Add(ptValue);
                ptValue = new UI.PassTypeAdd.PassTypeValue(rm.GetString("isPassWholeDayAbsence", culture), (int)Constants.PassTypeAll.WholeDayAbsences);
                ptMemebers.Add(ptValue);
                ptValue = new UI.PassTypeAdd.PassTypeValue(rm.GetString("isOverTime", culture), (int)Constants.PassTypeAll.OverTime);
                ptMemebers.Add(ptValue);
                ptValue = new UI.PassTypeAdd.PassTypeValue(rm.GetString("isPassOther", culture), (int)Constants.PassTypeAll.OtherPaymentCode);
                ptMemebers.Add(ptValue);

                cbType.DataSource = ptMemebers;
                cbType.DisplayMember = "TypeName";
                cbType.ValueMember = "TypeValue";
                cbType.SelectedIndex = 0;

                cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateComboBox(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void TMTypes_Load(object sender, EventArgs e)
        {
            try
            {
                _comp = new ListViewItemComparer(lvPassTypes);
                lvPassTypes.ListViewItemSorter = _comp;
                lvPassTypes.Sorting = SortOrder.Ascending;
                setLanguage();
                loadData();
                populateConformationTypeCombo();
                populateLimitCombo(cbLimitComposite, Constants.limitComposite);
                populateLimitCombo(cbLimitOccasional, Constants.limitOccassionaly);
                populateLimitCombo(cbLimitPerType, Constants.limitElementary);
                populateUserCategoryListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.TMTypes_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void loadData()
        {
            try
            {
                limitDict = new PassTypeLimit().SearchDictionary();
                categoryXTypeDict = new ApplUserCategoryXPassType().SearchTypeDictionary();
                typeXConfirmDict = new PassTypesConfirmation().SearchDictionary();
                populateComboBox(cbDescription, "description");
                populateComboBox(cbDescAltenative, "description_alternative");
                populateComboBox(cbPaymentCode, "payment_code");
                populateCompanyCombo();
                populateTypeCombo();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.loadData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void loadDataTemp()
        {
            try
            {
                categoryXTypeDict = new ApplUserCategoryXPassType().SearchTypeDictionary();
                typeXConfirmDict = new PassTypesConfirmation().SearchDictionary();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.loadData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                selectedApplUserCategories = new List<int>();
                foreach (ListViewItem item in lvUserCategory.SelectedItems)
                {
                    int categoryID = ((ApplUserCategoryTO)item.Tag).CategoryID;
                    if (!selectedApplUserCategories.Contains(categoryID))
                        selectedApplUserCategories.Add(categoryID);
                }
                this.Cursor = Cursors.WaitCursor;
                PassType type = new PassType();
                if (cbDescription.SelectedIndex > 0)
                    type.PTypeTO.Description = cbDescription.SelectedItem.ToString();
                if (cbDescAltenative.SelectedIndex > 0)
                    type.PTypeTO.DescAlt = cbDescAltenative.SelectedItem.ToString();
                if (cbPaymentCode.SelectedIndex > 0)
                    type.PTypeTO.PaymentCode = cbPaymentCode.SelectedItem.ToString();
                if (cbType.SelectedIndex > 0)
                {
                    type.PTypeTO.IsPass = (int)cbType.SelectedValue;
                }
                if (cbCompany.SelectedIndex > 0)
                    type.PTypeTO.WUID = (int)cbCompany.SelectedValue;
                if (!rbManualInputNo.Checked || !rbManualInputYes.Checked)
                {
                    if (rbManualInputNo.Checked)
                        type.PTypeTO.ManualInputFlag = Constants.noInt;
                    else
                        type.PTypeTO.ManualInputFlag = Constants.yesInt;
                }
                if (!rbVerificationNo.Checked || !rbVerificationYes.Checked)
                {
                    if (rbVerificationNo.Checked)
                        type.PTypeTO.VerificationFlag = Constants.noInt;
                    else
                        type.PTypeTO.VerificationFlag = Constants.yesInt;
                }
                if (!rbMassiveInputNo.Checked || !rbMassiveInputYes.Checked)
                {
                    if (rbMassiveInputNo.Checked)
                        type.PTypeTO.MassiveInput = Constants.noInt;
                    else
                        type.PTypeTO.MassiveInput = Constants.yesInt;
                }
                if (cbLimitComposite.SelectedIndex > 0)
                    type.PTypeTO.LimitCompositeID = (int)cbLimitComposite.SelectedValue;
                if (cbLimitOccasional.SelectedIndex > 0)
                    type.PTypeTO.LimitOccasionID = (int)cbLimitOccasional.SelectedValue;
                if (cbLimitPerType.SelectedIndex > 0)
                    type.PTypeTO.LimitElementaryID = (int)cbLimitPerType.SelectedValue;
                
                currentTypes = type.Search();
                populateListView(currentTypes);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void populateListView(List<PassTypeTO> passTypesList)
        {
            try
            {
                // clear categoiries and confirmation types
                lvConfirmationTypes.BeginUpdate();
                lvConfirmationTypes.Items.Clear();
                lvConfirmationTypes.EndUpdate();

                lvCategoryDetails.BeginUpdate();
                lvCategoryDetails.Items.Clear();
                lvCategoryDetails.EndUpdate();

                lvPassTypes.BeginUpdate();
                lvPassTypes.Items.Clear();

                if (passTypesList.Count > 0)
                {
                    foreach (PassTypeTO passType in passTypesList)
                    {
                        bool valid = true;
                        if (selectedApplUserCategories.Count > 0)
                        {
                            if (!categoryXTypeDict.ContainsKey(passType.PassTypeID))
                                continue;
                            valid = false;
                            foreach (int selected in selectedApplUserCategories)
                                if (categoryXTypeDict[passType.PassTypeID].Contains(selected))
                                {
                                    valid = true;
                                    break;
                                }
                        }

                        if (!valid)
                            continue;
                        ListViewItem item = new ListViewItem();

                        item.Text = passType.Description.ToString().Trim();

                        item.SubItems.Add(passType.DescAlt.ToString().Trim());
                        switch (passType.IsPass)
                        {
                            case (int)Constants.PassTypeAll.PassOnReader:
                                item.SubItems.Add(rm.GetString("isPassReader", culture));
                                break;
                            case (int)Constants.PassTypeAll.OverTime:
                                item.SubItems.Add(rm.GetString("isOverTime", culture));
                                break;
                            case (int)Constants.PassTypeAll.OtherPaymentCode:
                                item.SubItems.Add(rm.GetString("isPassOther", culture));
                                break;
                            case (int)Constants.PassTypeAll.WholeDayAbsences:
                                item.SubItems.Add(rm.GetString("isPassWholeDayAbsence", culture));
                                break;
                        }
                        item.SubItems.Add(passType.PaymentCode.ToString().Trim());
                        if (CompaniesDict.ContainsKey(passType.WUID))
                            item.SubItems.Add(CompaniesDict[passType.WUID].Name);
                        else
                            item.SubItems.Add("N/A");
                        if (passType.MassiveInput == Constants.yesInt)
                            item.SubItems.Add(rm.GetString("yes", culture));
                        else
                            item.SubItems.Add(rm.GetString("no", culture));
                        if (passType.ManualInputFlag == Constants.yesInt)
                            item.SubItems.Add(rm.GetString("yes", culture));
                        else
                            item.SubItems.Add(rm.GetString("no", culture));
                        if (passType.ConfirmFlag == Constants.yesInt)
                            item.SubItems.Add(rm.GetString("yes", culture));
                        else
                            item.SubItems.Add(rm.GetString("no", culture));
                        //if (typesDict.ContainsKey(passType.ConfirmPTID))
                        //    item.SubItems.Add(typesDict[passType.ConfirmPTID].Description);
                        //else
                        //    item.SubItems.Add("N/A");

                        if (passType.VerificationFlag == Constants.yesInt)
                            item.SubItems.Add(rm.GetString("yes", culture));
                        else
                            item.SubItems.Add(rm.GetString("no", culture));

                        if (limitDict.ContainsKey(passType.LimitCompositeID))
                            item.SubItems.Add(limitDict[passType.LimitCompositeID].Name);
                        else
                            item.SubItems.Add("N/A");

                        if (limitDict.ContainsKey(passType.LimitElementaryID))
                            item.SubItems.Add(limitDict[passType.LimitElementaryID].Name);
                        else
                            item.SubItems.Add("N/A");
                        if (limitDict.ContainsKey(passType.LimitOccasionID))
                            item.SubItems.Add(limitDict[passType.LimitOccasionID].Name);
                        else
                            item.SubItems.Add("N/A");

                        item.ToolTipText = passType.PassTypeID.ToString();

                        item.Tag = passType;

                        lvPassTypes.Items.Add(item);
                    }
                }

                lvPassTypes.EndUpdate();
                lvPassTypes.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypes.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvPassTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateTypeCategoryDetails();
                populateTypeConfirmDetails();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypes.lvPassTypes_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            int selected = lvPassTypes.SelectedItems.Count;
            if (!lvPassTypes.SelectedItems.Count.Equals(0))
            {
                try
                {
                    DialogResult result = MessageBox.Show(rm.GetString("msgPassTypeDel", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        bool isDeleted = true;

                        foreach (ListViewItem item in lvPassTypes.SelectedItems)
                        {
                            if (((PassTypeTO)item.Tag).PassTypeID <= 0 || ((PassTypeTO)item.Tag).PassTypeID == 5)
                            {
                                MessageBox.Show(rm.GetString("defaultPassTypeDel", culture));
                                selected--;
                            }
                            else
                            {
                                // Find if exists Passes for this Pass Type
                                Pass pass = new Pass();
                                pass.PssTO.PassTypeID = ((PassTypeTO)item.Tag).PassTypeID;
                                List<PassTO> passArray = pass.Search();

                                if (passArray.Count > 0)
                                {
                                    MessageBox.Show(item.Text + ": " + rm.GetString("ptHasPasses", culture));
                                    selected--;
                                }
                                else
                                {
                                    // Find if exists IO Pairs for this Pass Type
                                    IOPair iop = new IOPair();
                                    iop.PairTO.PassTypeID = ((PassTypeTO)item.Tag).PassTypeID;
                                    List<IOPairTO> ioPairArray = iop.Search();

                                    if (ioPairArray.Count > 0)
                                    {
                                        MessageBox.Show(item.Text + ": " + rm.GetString("ptHasIOPairs", culture));
                                        selected--;
                                    }
                                    else
                                    {
                                        // Find if exists Employee Absence for this Pass Type
                                        //ArrayList emplAbsenceArray = new EmployeeAbsence().Search("", item.Tag.ToString().Trim(),
                                        //	"", "");
                                        EmployeeAbsence ea = new EmployeeAbsence();
                                        ea.EmplAbsTO.PassTypeID = ((PassTypeTO)item.Tag).PassTypeID;
                                        List<EmployeeAbsenceTO> emplAbsenceArray = ea.Search("");

                                        if (emplAbsenceArray.Count > 0)
                                        {
                                            MessageBox.Show(item.Text + ": " + rm.GetString("ptHasEmplAbsences", culture));
                                            selected--;
                                        }
                                        else
                                        {
                                            ApplUserCategoryXPassType category = new ApplUserCategoryXPassType();
                                            category.UserCategoryXPassTypeTO.PassTypeID = ((PassTypeTO)item.Tag).PassTypeID;
                                            bool trans = category.BeginTransaction();
                                            if (trans)
                                            {
                                                try
                                                {
                                                    isDeleted = isDeleted && category.Delete(((PassTypeTO)item.Tag).PassTypeID, false);

                                                    PassTypesConfirmation confirm = new PassTypesConfirmation();
                                                    confirm.SetTransaction(category.GetTransaction());
                                                    isDeleted = isDeleted && confirm.Delete(((PassTypeTO)item.Tag).PassTypeID, false);

                                                    PassType type = new PassType();
                                                    type.SetTransaction(category.GetTransaction());
                                                    isDeleted = type.Delete(((PassTypeTO)item.Tag).PassTypeID, false) && isDeleted;

                                                    if (isDeleted)
                                                        category.CommitTransaction();
                                                    else
                                                        category.RollbackTransaction();
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (category.GetTransaction() != null)
                                                        category.RollbackTransaction();
                                                    log.writeLog(DateTime.Now + " PassTypes.btnDelete_Click(): " + ex.Message + "\n");
                                                    isDeleted = false;
                                                    MessageBox.Show(ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if ((selected > 0) && isDeleted)
                        {
                            MessageBox.Show(rm.GetString("passTypeDeleted", culture));
                        }
                        else if (!isDeleted)
                        {
                            MessageBox.Show(rm.GetString("passTypeNotDeleted", culture));
                        }

                        btnSearch_Click(this, new EventArgs());
                    }
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " PassTypes.btnDelete_Click(): " + ex.Message + "\n");
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                 this.Cursor = Cursors.WaitCursor;
                TMTypeAdd addForm = new TMTypeAdd();
                addForm.ShowDialog();

                loadDataTemp();
                btnSearch_Click(this, new EventArgs());

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypes.btnAdd_Click(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;

                if (lvPassTypes.SelectedItems.Count == 1)
                {
                    List<int>belongList = new List<int>();
                    if(categoryXTypeDict.ContainsKey(((PassTypeTO)lvPassTypes.SelectedItems[0].Tag).PassTypeID))
                        belongList = categoryXTypeDict[((PassTypeTO)lvPassTypes.SelectedItems[0].Tag).PassTypeID];

                    List<int> confirmList = new List<int>();
                    if (typeXConfirmDict.ContainsKey(((PassTypeTO)lvPassTypes.SelectedItems[0].Tag).PassTypeID))
                        confirmList = typeXConfirmDict[((PassTypeTO)lvPassTypes.SelectedItems[0].Tag).PassTypeID];

                    TMTypeAdd ptAdd = new TMTypeAdd((PassTypeTO)lvPassTypes.SelectedItems[0].Tag, belongList,confirmList);
                    ptAdd.ShowDialog(this);
                    loadDataTemp();
                    btnSearch_Click(this, new EventArgs());
                   
                }
                else
                {
                    MessageBox.Show(rm.GetString("msgPleaseSelectPassType", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbManualInputYes_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbManualInputYes.Checked)
                rbManualInputNo.Checked = true;
        }

        private void rbManualInputNo_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbManualInputNo.Checked)
                rbManualInputYes.Checked = true;
        }

        private void rbVerificationYes_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbVerificationYes.Checked)
                rbVerificationNo.Checked = true;
        }

        private void rbVerificationNo_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbVerificationNo.Checked)
                rbVerificationYes.Checked = true;
        }

        private void rbMassiveInputYes_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbMassiveInputYes.Checked)
                rbMassiveInputNo.Checked = true;
        }

        private void rbMassiveInputNo_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbVerificationYes.Checked)
                rbVerificationNo.Checked = true;
        }

        //private void rbAbsenceConfirmationYes_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!rbAbsenceConfirmationYes.Checked)
        //        rbAbsenceConfirmationNo.Checked = true;
        //}

        //private void rbAbsenceConfirmationNo_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!rbAbsenceConfirmationNo.Checked)
        //        rbAbsenceConfirmationYes.Checked = true;
        //}

        private void lvPassTypes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvPassTypes.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvPassTypes.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvPassTypes.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvPassTypes.Sorting = SortOrder.Ascending;
                }

                lvPassTypes.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.lvPassTypes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvPassTypes_ColumnClick():" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer : IComparer
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
                //switch (SortColumn)
                //{
                //    case PassTypes.ButtonIndex:
                //    case PassTypes.DescriptionIndex:
                //    case PassTypes.IsPassIndex:
                //    case PassTypes.PayCodeIndex:
                //        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                //        }
                //    default:
                //        throw new IndexOutOfRangeException("Unrecognized column name extension");
                //}
            }
        }

        #endregion

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                loadData();
                cbCompany.SelectedIndex = cbDescAltenative.SelectedIndex = cbDescription.SelectedIndex = cbLimitComposite.SelectedIndex =
                    cbLimitOccasional.SelectedIndex = cbLimitPerType.SelectedIndex = cbPaymentCode.SelectedIndex = cbType.SelectedIndex = 0;
                rbManualInputNo.Checked = rbMassiveInputNo.Checked = rbVerificationNo.Checked = true;
                 rbManualInputYes.Checked = rbMassiveInputYes.Checked = rbVerificationYes.Checked = true;
                foreach (ListViewItem item in lvUserCategory.Items)
                    item.Selected = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        }
    }
