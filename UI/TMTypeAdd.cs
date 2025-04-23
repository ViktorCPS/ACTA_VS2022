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

namespace UI
{
    public partial class TMTypeAdd : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        PassTypeTO currentPassType = new PassTypeTO();
        private List<ApplUserCategoryTO> allCategories = new List<ApplUserCategoryTO>();
        private List<ApplUserCategoryXPassTypeTO> categoryXTypeList = new List<ApplUserCategoryXPassTypeTO>();
        List<int> belongCategoriesID = new List<int>();

        private List<PassTypeTO> allTypes = new List<PassTypeTO>();
        List<int> belongConfirmTypeIDs = new List<int>();

        public TMTypeAdd()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
        }
        public TMTypeAdd(PassTypeTO passType, List<int> belongList, List<int> confirmList)
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);

            currentPassType = passType;
            belongCategoriesID = belongList;
            belongConfirmTypeIDs = confirmList;
        }

        /// <summary>
        /// Set proper language
        /// </summary>
        private void setLanguage()
        {
            try
            {

                if (currentPassType.PassTypeID.Equals(-1))
                {
                    this.Text = rm.GetString("addPassType", culture);
                }
                else
                {
                    this.Text = rm.GetString("updatePassType", culture);
                }

                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // group box text
                this.gbAbsenceConfirmation.Text = rm.GetString("gbAbsenceConformation", culture);
                gbLimits.Text = rm.GetString("gbLimits", culture);
                this.gbMassiveInput.Text = rm.GetString("gbMassiveInput", culture);
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
                lblSegmentColor.Text = rm.GetString("lblSegmentColor", culture); 

                //radio button's text
                rbMassiveInputYes.Text = rm.GetString("yes", culture);
                rbMassiveInuptNo.Text = rm.GetString("no", culture);
                rbVerificationYes.Text = rm.GetString("yes", culture);
                rbVerificationNo.Text = rm.GetString("no", culture);
                rbManualInputYes.Text = rm.GetString("yes", culture);
                rbManualInputNo.Text = rm.GetString("no", culture);
                rbAbsenceConfirmationYes.Text = rm.GetString("yes", culture);
                rbAbsenceConfirmationNo.Text = rm.GetString("no", culture);
                                

                lvUserCategory.BeginUpdate();
                lvUserCategory.Columns.Add(rm.GetString("hdrName", culture), lvUserCategory.Width / 2 - 5, HorizontalAlignment.Left);
                lvUserCategory.Columns.Add(rm.GetString("hdrDescription", culture), lvUserCategory.Width / 2 - 5, HorizontalAlignment.Left);
                lvUserCategory.EndUpdate();

                lvTypeCategories.BeginUpdate();
                lvTypeCategories.Columns.Add(rm.GetString("hdrName", culture), lvTypeCategories.Width / 3 - 5, HorizontalAlignment.Left);
                lvTypeCategories.Columns.Add(rm.GetString("hdrDescription", culture), lvTypeCategories.Width / 3 - 5, HorizontalAlignment.Left);
                lvTypeCategories.EndUpdate();

                lvTypes.BeginUpdate();
                lvTypes.Columns.Add(rm.GetString("hdrName", culture), lvUserCategory.Width - 10, HorizontalAlignment.Left);
                lvTypes.EndUpdate();

                lvTypesConfirmation.BeginUpdate();
                lvTypesConfirmation.Columns.Add(rm.GetString("hdrName", culture), lvTypeCategories.Width  - 10, HorizontalAlignment.Left);
                lvTypesConfirmation.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

                //WorkingUnitTO wuTO = new WorkingUnitTO();
                //wuTO.Name = rm.GetString("all", culture);
                //companies.Insert(0, wuTO);

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
                lvUserCategory.Items.Clear();
                allCategories = new ApplUserCategory().Search(null);
                foreach (ApplUserCategoryTO category in allCategories)
                {
                    if (!belongCategoriesID.Contains(category.CategoryID))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = category.Name;
                        item.SubItems.Add(category.Desc);
                        item.Tag = category;
                        lvUserCategory.Items.Add(item);
                    }
                }
                lvUserCategory.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateUserCategoryListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populatePassTypeListView()
        {
            try
            {
                lvTypes.BeginUpdate();
                lvTypes.Items.Clear();
                
                foreach (PassTypeTO type in allTypes)
                {
                    if (!belongConfirmTypeIDs.Contains(type.PassTypeID))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = type.Description;
                        item.Tag = type;
                        lvTypes.Items.Add(item);
                    }
                }
                lvTypes.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populatePassTypeListView(): " + ex.Message + "\n");
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
     

        private void TMTypeAdd_Load(object sender, EventArgs e)
        {
            try
            {
                btnUpdate.Visible = currentPassType.PassTypeID != -1;
                
                setLanguage();
                populateCompanyCombo();
                populateTypeCombo();
                populateLimitCombo(cbLimitComposite, Constants.limitComposite);
                populateLimitCombo(cbLimitOccasional, Constants.limitOccassionaly);
                populateLimitCombo(cbLimitPerType, Constants.limitElementary);
                populateUserCategoryListView();
                populateTypeBelongCategorieListView();

                PassType pt = new PassType();
                pt.PTypeTO.WUID = (int)cbCompany.SelectedValue;
                allTypes = pt.Search();
                populatePassTypeListView();
                populateTypeBelongConfirmListView();
                if (currentPassType.PassTypeID != -1)
                {
                    tbPassTypeID.Enabled = false;
                    tbDesc.Text = currentPassType.Description;
                    tbDescAlternative.Text = currentPassType.DescAlt;
                    tbSegmentColor.Text = currentPassType.SegmentColor;
                    tbPayCode.Text = currentPassType.PaymentCode;
                    tbPassTypeID.Text = currentPassType.PassTypeID.ToString();
                    rbAbsenceConfirmationYes.Checked = currentPassType.ConfirmFlag == 1;
                    rbManualInputYes.Checked = currentPassType.ManualInputFlag == 1;
                    rbMassiveInputYes.Checked = currentPassType.MassiveInput == 1;
                    rbVerificationYes.Checked = currentPassType.VerificationFlag == 1;
                    cbCompany.SelectedValue = currentPassType.WUID;
                    cbLimitComposite.SelectedValue = currentPassType.LimitCompositeID;
                    cbLimitOccasional.SelectedValue = currentPassType.LimitOccasionID;
                    cbLimitPerType.SelectedValue = currentPassType.LimitElementaryID;
                    cbType.SelectedValue = currentPassType.IsPass;
                }
                rbAbsenceConfirmationYes_CheckedChanged(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateComboBox(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbAbsenceConfirmationYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lvTypesConfirmation.Enabled = lvTypes.Enabled =btnAddConfirmType.Enabled = btnRemoveConfirmType.Enabled = rbAbsenceConfirmationYes.Checked;
                if (rbAbsenceConfirmationYes.Checked == false)
                {
                    belongConfirmTypeIDs = new List<int>();
                    populatePassTypeListView();
                    populateTypeBelongConfirmListView();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateComboBox(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvUserCategory.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvUserCategory.SelectedItems)
                    {
                        ApplUserCategoryTO tt = (ApplUserCategoryTO)item.Tag;
                        if (!belongCategoriesID.Contains(tt.CategoryID))
                            belongCategoriesID.Add(tt.CategoryID);
                    }
                    populateUserCategoryListView();
                    populateTypeBelongCategorieListView();
                }
                else
                {
                    MessageBox.Show(rm.GetString("selType", culture), "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TMTypes.btnAdd_Click(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateTypeBelongCategorieListView()
        {
            try
            {
                if (allCategories.Count > 0)
                {
                    lvTypeCategories.BeginUpdate();
                    lvTypeCategories.Items.Clear();
                    foreach (int id in belongCategoriesID)
                    {
                        foreach (ApplUserCategoryTO type in allCategories)
                        {
                            if (id == type.CategoryID)
                            {
                                ListViewItem item = new ListViewItem();
                                item.Text = type.Name;
                                item.SubItems.Add(type.Desc);
                                item.Tag = type;
                                item.ToolTipText = type.Desc;
                                lvTypeCategories.Items.Add(item);
                            }
                        }
                    }
                    lvTypeCategories.EndUpdate();
                    lvTypeCategories.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TMTypes.populateTypeBelongCategorieListView(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void populateTypeBelongConfirmListView()
        {
            try
            {
                if (allCategories.Count > 0)
                {
                    lvTypesConfirmation.BeginUpdate();
                    lvTypesConfirmation.Items.Clear();
                    foreach (int id in belongConfirmTypeIDs)
                    {
                        foreach (PassTypeTO type in allTypes)
                        {
                            if (id == type.PassTypeID)
                            {
                                ListViewItem item = new ListViewItem();
                                item.Text = type.Description;
                                item.Tag = type;
                                lvTypesConfirmation.Items.Add(item);
                            }
                        }
                    }
                    lvTypesConfirmation.EndUpdate();
                    lvTypesConfirmation.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TMTypes.populateTypeBelongConfirmListView(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Pass Type ID
                if (this.tbPassTypeID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("messagePassTypeIDNotSet", culture));
                    return;
                }

                try
                {
                    if (!tbPassTypeID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbPassTypeID.Text.Trim());
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypeIDMustBeNum", culture));
                    return;
                }

                // Pass Type
                if (cbType.SelectedIndex < 0)
                {
                    MessageBox.Show(rm.GetString("msgPassType", culture));
                    return;
                }
                /*if (((int) cbPassType.SelectedValue) < 2 && tbButton.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("passTypeButtonNotNull", culture));
                    return;
                }*/

                // Button
                //try
                //{
                //    if (!tbButton.Text.Trim().Equals(""))
                //    {
                //        Int32.Parse(tbButton.Text.Trim());
                //    }
                //}
                //catch
                //{
                //    MessageBox.Show(rm.GetString("messagePassTypeButton", culture));
                //    return;
                //}

                // Payment Code
                try
                {
                    if (!tbPayCode.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbPayCode.Text.Trim());
                    }
                    if ((!tbPayCode.Text.Trim().Equals("")) && (tbPayCode.Text.Trim().Length < Constants.PaymentCodeLength))
                    {
                        MessageBox.Show(rm.GetString("messagePassTypePayCode", culture));
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypePayCode", culture));
                    return;
                }

                if (tbDesc.Text.ToString().Trim() == "")
                {
                    MessageBox.Show(rm.GetString("enterTypeDesc", culture));
                    return;
                }
                if (tbDescAlternative.Text.ToString().Trim() == "")
                {
                    MessageBox.Show(rm.GetString("enterTypeDescAlternative", culture));
                    return;
                }
                //if (cbCompany.SelectedIndex <=0)
                //{
                //    MessageBox.Show(rm.GetString("enterTypeCompany", culture));
                //    return;
                //}

                // Pass Type
                PassType pt = new PassType();
                int ptID = -1;
                if (int.TryParse(tbPassTypeID.Text.Trim(), out ptID))
                    pt.PTypeTO.PassTypeID = ptID;
                pt.PTypeTO.Description = tbDesc.Text.Trim();                
                pt.PTypeTO.IsPass = (int)cbType.SelectedValue;
                pt.PTypeTO.PaymentCode = tbPayCode.Text.Trim();
                pt.PTypeTO.DescAlt = tbDescAlternative.Text.ToString().Trim();
                pt.PTypeTO.SegmentColor = tbSegmentColor.Text.ToString().Trim();

                pt.PTypeTO.WUID = (int)cbCompany.SelectedValue;
                if (rbManualInputNo.Checked)
                    pt.PTypeTO.ManualInputFlag = Constants.noInt;
                else
                    pt.PTypeTO.ManualInputFlag = Constants.yesInt;
                if (rbVerificationNo.Checked)
                    pt.PTypeTO.VerificationFlag = Constants.noInt;
                else
                    pt.PTypeTO.VerificationFlag = Constants.yesInt;
                if (rbMassiveInuptNo.Checked)
                    pt.PTypeTO.MassiveInput = Constants.noInt;
                else
                    pt.PTypeTO.MassiveInput = Constants.yesInt;
                if (rbAbsenceConfirmationNo.Checked)
                    pt.PTypeTO.ConfirmFlag = Constants.noInt;
                else
                    pt.PTypeTO.ConfirmFlag = Constants.yesInt;
                if (cbLimitComposite.SelectedIndex > 0)
                    pt.PTypeTO.LimitCompositeID = (int)cbLimitComposite.SelectedValue;
                if (cbLimitOccasional.SelectedIndex > 0)
                    pt.PTypeTO.LimitOccasionID = (int)cbLimitOccasional.SelectedValue;
                if (cbLimitPerType.SelectedIndex > 0)
                    pt.PTypeTO.LimitElementaryID = (int)cbLimitPerType.SelectedValue;                
               bool trans = pt.BeginTransaction();
               if (trans)
               {
                   try
                   {
                       int inserted = pt.Save(false);
                       bool succ = (inserted == 1);
                       ApplUserCategoryXPassType type = new ApplUserCategoryXPassType();
                       type.SetTransaction(pt.GetTransaction());
                       foreach (int category in belongCategoriesID)
                       {
                           type.UserCategoryXPassTypeTO.PassTypeID = pt.PTypeTO.PassTypeID;
                           type.UserCategoryXPassTypeTO.CategoryID = category;
                           succ = succ && type.Save(false) == 1;
                       }
                       if (succ)
                       {
                           PassTypesConfirmation confirm = new PassTypesConfirmation();
                           confirm.SetTransaction(pt.GetTransaction());
                           foreach (int typeID in belongConfirmTypeIDs)
                           {
                               confirm.PTConfirmTO.PassTypeID = pt.PTypeTO.PassTypeID;
                               confirm.PTConfirmTO.ConfirmationPassTypeID = typeID;
                               succ = succ && confirm.Save(confirm.PTConfirmTO,false) == 1;
                           }
                       }
                       if (succ)
                       {
                           pt.CommitTransaction();
                           MessageBox.Show(rm.GetString("messagePassTypeInserted", culture));
                           currentPassType = new PassTypeTO();
                           this.Close();
                       }
                       else
                       {
                           pt.RollbackTransaction();
                           MessageBox.Show(rm.GetString("messagePassTypeNOTInserted", culture));
                       }
                   }
                   catch (Exception ex)
                   {
                       if (pt.GetTransaction() != null)
                           pt.RollbackTransaction();
                       log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TMTypes.btnSave_Click(); " + ex.Message);
                       MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                   }
               }

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TMTypes.btnSave_Click(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvTypeCategories.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvTypeCategories.SelectedItems)
                    {
                        ApplUserCategoryTO tt = (ApplUserCategoryTO)item.Tag;

                        if (belongCategoriesID.Contains(tt.CategoryID))
                            belongCategoriesID.Remove(tt.CategoryID);

                    }
                    populateTypeBelongCategorieListView();
                    populateUserCategoryListView();
                }
                else
                {
                    MessageBox.Show(rm.GetString("selTypeBelong", culture), "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EventAdd.btnRemove_Click(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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
                if (this.tbPassTypeID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("messagePassTypeIDNotSet", culture));
                    return;
                }

                try
                {
                    if (!tbPassTypeID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbPassTypeID.Text.Trim());
                    }
                }
                //catch(Exception ex)
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypeIDMustBeNum", culture));
                    return;
                }

                if (cbType.SelectedIndex < 0)
                {
                    MessageBox.Show(rm.GetString("msgPassType", culture));
                    return;
                }

                /*if (((int) cbPassType.SelectedValue) < 2 && tbButton.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("passTypeButtonNotNull", culture));
                    return;
                }*/

                //try
                //{
                //    if (!tbButton.Text.Trim().Equals(""))
                //    {
                //        Int32.Parse(tbButton.Text.Trim());
                //    }
                //}
                ////catch(Exception ex)
                //catch
                //{
                //    MessageBox.Show(rm.GetString("messagePassTypeButton", culture));
                //    return;
                //}

                // Payment Code
                try
                {
                    if (!tbPayCode.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbPayCode.Text.Trim());
                    }
                    if ((!tbPayCode.Text.Trim().Equals("")) && (tbPayCode.Text.Trim().Length < Constants.PaymentCodeLength))
                    {
                        MessageBox.Show(rm.GetString("messagePassTypePayCode", culture));
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypePayCode", culture));
                    return;
                }

                // Pass Type
                int isPass = (int)cbType.SelectedValue;

                bool isUpdated = false;
                if (tbDesc.Text.ToString().Trim() == "")
                {
                    MessageBox.Show(rm.GetString("enterTypeDesc", culture));
                    return;
                }
                if (tbDescAlternative.Text.ToString().Trim() == "")
                {
                    MessageBox.Show(rm.GetString("enterTypeDescAlternative", culture));
                    return;
                }
                //if (cbCompany.SelectedIndex <= 0)
                //{
                //    MessageBox.Show(rm.GetString("enterTypeCompany", culture));
                //    return;
                //}

                // currentPassType still contains old values of Pass Type
                PassType pt = new PassType();
                int ptID = -1;
                if (int.TryParse(tbPassTypeID.Text.Trim(), out ptID))
                    pt.PTypeTO.PassTypeID = ptID;
                pt.PTypeTO.Description = tbDesc.Text.Trim();
                pt.PTypeTO.IsPass = (int)cbType.SelectedValue;
                pt.PTypeTO.PaymentCode = tbPayCode.Text.Trim();
                pt.PTypeTO.DescAlt = tbDescAlternative.Text.ToString().Trim();
                pt.PTypeTO.SegmentColor = tbSegmentColor.Text.ToString().Trim();
                
                    pt.PTypeTO.WUID = (int)cbCompany.SelectedValue;
                if (rbManualInputNo.Checked)
                    pt.PTypeTO.ManualInputFlag = Constants.noInt;
                else
                    pt.PTypeTO.ManualInputFlag = Constants.yesInt;
                if (rbVerificationNo.Checked)
                    pt.PTypeTO.VerificationFlag = Constants.noInt;
                else
                    pt.PTypeTO.VerificationFlag = Constants.yesInt;
                if (rbMassiveInuptNo.Checked)
                    pt.PTypeTO.MassiveInput = Constants.noInt;
                else
                    pt.PTypeTO.MassiveInput = Constants.yesInt;
                if (rbAbsenceConfirmationNo.Checked)
                    pt.PTypeTO.ConfirmFlag = Constants.noInt;
                else
                    pt.PTypeTO.ConfirmFlag = Constants.yesInt;
                if (cbLimitComposite.SelectedIndex > 0)
                    pt.PTypeTO.LimitCompositeID = (int)cbLimitComposite.SelectedValue;
                if (cbLimitOccasional.SelectedIndex > 0)
                    pt.PTypeTO.LimitOccasionID = (int)cbLimitOccasional.SelectedValue;
                if (cbLimitPerType.SelectedIndex > 0)
                    pt.PTypeTO.LimitElementaryID = (int)cbLimitPerType.SelectedValue;
                
               isUpdated = pt.BeginTransaction();
               if (isUpdated)
               {
                   try
                   {
                       isUpdated = isUpdated && pt.Update(-1, false);

                       if (isUpdated)
                       {
                           ApplUserCategoryXPassType typeCategory = new ApplUserCategoryXPassType();
                           typeCategory.SetTransaction(pt.GetTransaction());
                           isUpdated = isUpdated && typeCategory.Delete(pt.PTypeTO.PassTypeID, false);
                           foreach (int category in belongCategoriesID)
                           {
                               typeCategory.UserCategoryXPassTypeTO.PassTypeID = pt.PTypeTO.PassTypeID;
                               typeCategory.UserCategoryXPassTypeTO.CategoryID = category;
                               isUpdated = isUpdated && typeCategory.Save(false) == 1;
                           }
                           if (isUpdated)
                           {
                               PassTypesConfirmation confirm = new PassTypesConfirmation();
                               confirm.SetTransaction(pt.GetTransaction());
                               isUpdated = isUpdated && confirm.Delete(pt.PTypeTO.PassTypeID, false);
                               foreach (int typeID in belongConfirmTypeIDs)
                               {
                                   confirm.PTConfirmTO.PassTypeID = pt.PTypeTO.PassTypeID;
                                   confirm.PTConfirmTO.ConfirmationPassTypeID = typeID;
                                   isUpdated = isUpdated && confirm.Save(confirm.PTConfirmTO,false) == 1;
                               }
                           }
                           if (isUpdated)
                           {
                               pt.CommitTransaction();
                               MessageBox.Show(rm.GetString("messagePassTypeUpdated", culture));
                               currentPassType = new PassTypeTO();
                               this.Close();
                           }
                           else
                           {
                               pt.RollbackTransaction();
                               MessageBox.Show(rm.GetString("messagePassTypeNOTUpdated", culture));
                           }

                       }
                   }
                   catch (Exception ex)
                   {
                       pt.RollbackTransaction();
                       log.writeLog(DateTime.Now + " PassTypeAdd.btnUpdate_Click(): " + ex.Message + "\n");
                       
                           MessageBox.Show(ex.Message);
                   }
               }
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeAdd.btnUpdate_Click(): " + ex.Message + "\n");
                if (ex.Message.Equals("Button already exists."))
                {
                    MessageBox.Show(rm.GetString("passTypeButtonExists", culture));
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAddConfirmType_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvTypes.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvTypes.SelectedItems)
                    {
                        PassTypeTO tt = (PassTypeTO)item.Tag;
                        if (!belongConfirmTypeIDs.Contains(tt.PassTypeID))
                            belongConfirmTypeIDs.Add(tt.PassTypeID);
                    }
                    populatePassTypeListView();
                    populateTypeBelongConfirmListView();
                }
                else
                {
                    MessageBox.Show(rm.GetString("selType", culture), "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TMTypes.btnAddConfirmType_Click(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRemoveConfirmType_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvTypesConfirmation.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvTypesConfirmation.SelectedItems)
                    {
                        PassTypeTO tt = (PassTypeTO)item.Tag;

                        if (belongConfirmTypeIDs.Contains(tt.PassTypeID))
                            belongConfirmTypeIDs.Remove(tt.PassTypeID);

                    }
                    populatePassTypeListView();
                    populateTypeBelongConfirmListView();
                }
                else
                {
                    MessageBox.Show(rm.GetString("selTypeBelong", culture), "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TMTypes.btnRemoveConfirmType_Click(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PassType pt = new PassType();
                try
                {
                    pt.PTypeTO.WUID = (int)cbCompany.SelectedValue;
                    allTypes = pt.Search();
                    populatePassTypeListView();
                    populateTypeBelongConfirmListView();
                }
                catch { }
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TMTypes.cbCompany_SelectedIndexChanged(); " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
    }
}
