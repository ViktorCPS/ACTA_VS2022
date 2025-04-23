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
    public partial class HolidaysExtendedAdd : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        HolidaysExtendedTO currentHoliday = new HolidaysExtendedTO();

        public HolidaysExtendedAdd()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            setLanguage();
        }
        public HolidaysExtendedAdd(HolidaysExtendedTO holiday)
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);

            currentHoliday = holiday;
            btnSave.Visible = false;
            btnUpdate.Visible = true;
            setLanguage();
        }

        private void btnCancel_Click(object sender, EventArgs e)
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
                // Form name
                if (!currentHoliday.DateStart.Equals(new DateTime()))
                {
                    this.Text = rm.GetString("updateHoliday", culture);
                }
                else
                {
                    this.Text = rm.GetString("addHoliday", culture);
                }

                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // label's text
                lblDesc.Text = rm.GetString("lblDescription", culture);
                lblCategory.Text = rm.GetString("lblCategory", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblYear.Text = rm.GetString("lblYear", culture);
                lblSundayTransferable.Text = rm.GetString("lblSundayTransferable", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dtpTo.Value =  new DateTime(dtpYear.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
                dtpFrom.Value = new DateTime(dtpYear.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
                if (dtpFrom.Value > dtpTo.Value)
                {
                    MessageBox.Show(rm.GetString("endDateBeforeStart", culture));
                    return;                    
                }
                if (tbDesc.Text.ToString().Trim() == "")
                {
                    MessageBox.Show(rm.GetString("enterDesc", culture));
                    return;
                }
                HolidaysExtended hTO = new HolidaysExtended();
                hTO.HolTO.Category = cbCategory.SelectedItem.ToString();
                hTO.HolTO.Year = new DateTime(dtpYear.Value.Year, 1, 1);
                List<HolidaysExtendedTO> list = hTO.Search(dtpFrom.Value.Date, dtpTo.Value.Date);

                if (list.Count <= 0)
                {
                    currentHoliday.Description = this.tbDesc.Text.Trim();
                    currentHoliday.Category = cbCategory.SelectedItem.ToString();
                    if (cbType.SelectedIndex ==0)
                        currentHoliday.Type = Constants.nationalHoliday;
                    else
                        currentHoliday.Type = Constants.personalHoliday;
                    currentHoliday.SundayTransferable = cbSundayTransferable.SelectedIndex;
                    currentHoliday.Year = new DateTime(dtpYear.Value.Year, 1, 1);
                    currentHoliday.DateStart = new DateTime(dtpYear.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
                    currentHoliday.DateEnd = new DateTime(dtpYear.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);

                    hTO.HolTO = currentHoliday;
                    int inserted = hTO.Save();
                    if (inserted > 0)
                    {
                        MessageBox.Show(rm.GetString("holidaySaved", culture));
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("holidayExists", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void HolidaysExtendedAdd_Load(object sender, EventArgs e)
        {
            try
            {
                populateCategoryCombo();
                populateSundayTrasCombo();
                populateTypeCombo();
                dtpYear.Value = dtpFrom.Value = dtpTo.Value = DateTime.Now;

                if (currentHoliday.RecID >= 0)
                {
                    tbDesc.Text = currentHoliday.Description;
                    cbCategory.SelectedItem = currentHoliday.Category;
                    cbSundayTransferable.SelectedIndex = currentHoliday.SundayTransferable;
                    cbType.SelectedItem = rm.GetString(currentHoliday.Type,culture);
                    dtpFrom.Value = currentHoliday.DateStart;
                    dtpTo.Value = currentHoliday.DateEnd;
                    dtpYear.Value = currentHoliday.Year;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysAdd.HolidaysExtendedAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate Category Combo Box
        /// </summary>
        private void populateCategoryCombo()
        {
            try
            {
                cbType.Items.Add(rm.GetString(Constants.nationalHoliday, culture));
                cbType.Items.Add(rm.GetString(Constants.personalHoliday, culture));

                cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysAdd.populateCategoryCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate Category Combo Box
        /// </summary>
        private void populateSundayTrasCombo()
        {
            try
            {
                cbSundayTransferable.Items.Add(rm.GetString("no", culture));
                cbSundayTransferable.Items.Add(rm.GetString("yes", culture));

                cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysAdd.populateCategoryCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate Type Combo Box
        /// </summary>
        private void populateTypeCombo()
        {
            try
            {

                cbCategory.Items.Add(Constants.holidayTypeDefault);
                cbCategory.Items.Add(Constants.holidayTypeI);
                cbCategory.Items.Add(Constants.holidayTypeII);
                cbCategory.Items.Add(Constants.holidayTypeIII);

                cbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysAdd.populateCategoryCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dtpTo.Value = new DateTime(dtpYear.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);
                dtpFrom.Value = new DateTime(dtpYear.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
                if (dtpFrom.Value > dtpTo.Value)
                {
                    MessageBox.Show(rm.GetString("endDateBeforeStart", culture));
                    return;
                }
                if (tbDesc.Text.ToString().Trim() == "")
                {
                    MessageBox.Show(rm.GetString("enterDesc", culture));
                    return;
                }
                HolidaysExtended hTO = new HolidaysExtended();
                hTO.HolTO.Category = cbCategory.SelectedItem.ToString();
                hTO.HolTO.Year = new DateTime(dtpYear.Value.Year, 1, 1);
                List<HolidaysExtendedTO> list = hTO.Search(dtpFrom.Value.Date, dtpTo.Value.Date);

                if (list.Count <= 0 || (list.Count == 1 && list[0].RecID == currentHoliday.RecID))
                {
                    currentHoliday.Description = this.tbDesc.Text.Trim();
                    currentHoliday.Category = cbCategory.SelectedItem.ToString();
                    if (cbType.SelectedIndex == 0)
                        currentHoliday.Type = Constants.nationalHoliday;
                    else
                        currentHoliday.Type = Constants.personalHoliday;
                    currentHoliday.SundayTransferable = cbSundayTransferable.SelectedIndex;
                    currentHoliday.Year = new DateTime(dtpYear.Value.Year, 1, 1);
                    currentHoliday.DateStart = new DateTime(dtpYear.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);
                    currentHoliday.DateEnd = new DateTime(dtpYear.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);

                    hTO.HolTO = currentHoliday;
                    bool updated = hTO.Update();
                    if (updated)
                    {
                        MessageBox.Show(rm.GetString("holidayUpdated", culture));
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("holidayExists", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
