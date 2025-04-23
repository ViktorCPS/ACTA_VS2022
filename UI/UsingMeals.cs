using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Configuration;
using System.IO.Ports;

using ReaderInterface;
using Util;
using Common;
using TransferObjects;

namespace UI
{
    public partial class UsingMeals : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        MealAssigned mealAssigned;
        EmployeeTO empl;
        MealsEmployeeSchedule mealSchedule; 

        ApplUserTO logInUser;
        IReaderInterface readerInterface;

        public Thread reader;
        delegate void updateTextBoxTextDelegate(TextBox tb, string newText);
        delegate void updateTextBoxVisibleDelegate(TextBox tb, bool isVisible);
        delegate void updateLabelTextDelegate(Label lbl, string newText);
        delegate void updateLabelVisibleDelegate(Label lbl, bool isVisible);
        delegate void updateButtonEnabledDelegate(Button btn, bool isEnabled);
        delegate void updateDateTimePickerValueDelegate(DateTimePicker dtp, DateTime value);
        delegate void updateDateTimePickerVisibleDelegate(DateTimePicker dtp, bool isVisible);

        private uint oldTagID = 0;
        
        private bool stopReading;
        int desktopReaderPort;

        List<int> modulesList;

        public UsingMeals()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(UsingMeals).Assembly);

                setLanguage();

                logInUser = NotificationController.GetLogInUser();
                mealAssigned = new MealAssigned();
                empl = new EmployeeTO();

                ReaderFactory.TechnologyType = new Reader().GetDefaultTechnology();
                readerInterface = ReaderFactory.GetReader;

                modulesList = Common.Misc.getLicenceModuls(null);

                if (modulesList.Contains((int)Constants.Moduls.RestaurantI))
                {
                    gbMealsII.Visible = false;
                }

                if (modulesList.Contains((int)Constants.Moduls.RestaurantII))
                {
                    lblType.Visible = tbType.Visible =gbMeals.Visible = false;
                    gbMealsII.Visible = true;
                }

                desktopReaderPort = 0;
                StartThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                // form text
                this.Text = rm.GetString("UsingMealsForm", culture);

                // group box's text
                this.gbEmpl.Text = rm.GetString("gbEmpl", culture);
                this.gbMeals.Text = this.gbMealsII.Text = rm.GetString("gbMeals", culture);
                this.gbStatus.Text = rm.GetString("gbStatus", culture);

                // label's text
                this.lblAvailable.Text = rm.GetString("lblAvailable", culture);
                this.lblEmpl.Text = rm.GetString("lblEmpl", culture);
                this.lblFrom.Text = lblFromII.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text =lblToII.Text= rm.GetString("lblTo", culture);
                this.lblInterval.Text =lblIntervalII.Text = rm.GetString("lblInterval", culture);
                this.lblType.Text = rm.GetString("lblEmplType", culture);
                this.lblUsed.Text = rm.GetString("lblUsedMeal", culture);
                this.lblMealTypeII.Text = rm.GetString("lblMealType", culture);

                // button's text
                this.btnNext.Text = rm.GetString("btnNext", culture);
                this.btnUse.Text = rm.GetString("btnUse", culture);
                this.btnClose.Text = rm.GetString("btnClose", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
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
                log.writeLog(DateTime.Now + " UsingMeals.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void UsingMeals_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                clearForm();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.UsingMeals_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void clearForm()
        {
            try
            {
                pbEmplPhoto.Image = null;
                tbEmpl.Text = "";
                tbType.Text = "";
                
                tbAvailable.Text = "";
                lblMealCount.Text = "";
                tbUsed.Text = "";
                lblMealUsedCount.Text = "";
                dtpFrom.Value = DateTime.Now.Date;
                dtpTo.Value = DateTime.Now.Date;
                dtpFrom.Visible = true;
                dtpTo.Visible = true;
                lblUnlimitedFrom.Text = "";
                lblUnlimitedTo.Text = "";
                lblPrice.Text = "";
                lblMealPrice.Text = "";
                tbPrice.Visible = false;
                tbPrice.Text = "";
                tbMealTypeII.Text = "";
                dtpFromII.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                dtpToII.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                
                clearStatusPanel();

                btnUse.Enabled = false;
                btnNext.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.clearForm(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void clearStatusPanel()
        {
            try
            {
                panel1.BackColor = this.BackColor;
                tbMealType.Visible = false;
                tbQty.Visible = false;
                lblFailed.Text = "";
                lblStatus.Text = "";
                lblMealType.Text = "";
                lblQty.Text = "";
                lblMeals.Text = "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.clearStatusPanel(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void clearFormThread()
        {
            try
            {
                pbEmplPhoto.Image = null;
                updateTextBoxText(tbEmpl, "");
                updateTextBoxText(tbType, "");

                updateTextBoxText(tbAvailable, "");
                updateLabelText(lblMealCount, "");
                updateTextBoxText(tbUsed, "");
                updateLabelText(lblMealUsedCount, "");
                updateDateTimePickerValue(dtpFrom, DateTime.Now.Date);
                updateDateTimePickerValue(dtpTo, DateTime.Now.Date);
                updateDateTimePickerVisible(dtpFrom, true);
                updateDateTimePickerVisible(dtpTo, true);
                updateLabelText(lblUnlimitedFrom, "");
                updateLabelText(lblUnlimitedTo, "");
                updateLabelText(lblPrice, "");
                updateLabelText(lblMealPrice, "");
                updateTextBoxVisible(tbPrice, false);
                updateTextBoxText(tbPrice, "");
                
                panel1.BackColor = this.BackColor;
                updateTextBoxVisible(tbMealType, false);
                updateTextBoxVisible(tbQty, false);
                updateLabelText(lblFailed, "");
                updateLabelText(lblStatus, "");
                updateLabelText(lblMealType, "");
                updateLabelText(lblQty, "");
                updateLabelText(lblMeals, "");

                updateButtonEnabled(btnNext, false);
                updateButtonEnabled(btnUse, false);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.clearForm(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void StartThread()
        {
            try
            {
                stopReading = false;
                reader = new Thread(new ThreadStart(Read));
                reader.Name = "TagController";
                reader.Start();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.StartThread(): " + ex.Message + "\n");
                throw ex;
            }
        }

        #region Thread Update Methods
        private void updateTextBoxText(TextBox tb, string newText)
        {
            try
            {
                if (tb.InvokeRequired)
                {
                    updateTextBoxTextDelegate del = new updateTextBoxTextDelegate(updateTextBox);
                    tb.BeginInvoke(del, new object[] { tb, newText });
                }
                else
                    tb.Text = newText;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.updateTextBoxText(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void updateTextBox(TextBox tb, string newText)
        {
            tb.Text = newText;
        }

        private void updateTextBoxVisible(TextBox tb, bool isVisible)
        {
            try
            {
                if (tb.InvokeRequired)
                {
                    updateTextBoxVisibleDelegate del = new updateTextBoxVisibleDelegate(updateTextBoxV);
                    tb.BeginInvoke(del, new object[] { tb, isVisible });
                }
                else
                    tb.Visible = isVisible;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.updateTextBoxVisible(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void updateTextBoxV(TextBox tb, bool isVisible)
        {
            tb.Visible = isVisible;
        }

        private void updateDateTimePickerValue(DateTimePicker dtp, DateTime value)
        {
            try
            {
                if (dtp.InvokeRequired)
                {
                    updateDateTimePickerValueDelegate del = new updateDateTimePickerValueDelegate(updateDateTimePicker);
                    dtp.BeginInvoke(del, new object[] { dtp, value });
                }
                else
                    dtp.Value = value;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.updateDateTimePickerValue(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void updateDateTimePicker(DateTimePicker dtp, DateTime value)
        {
            dtp.Value = value;
        }

        private void updateDateTimePickerVisible(DateTimePicker dtp, bool isVisible)
        {
            try
            {
                if (dtp.InvokeRequired)
                {
                    updateDateTimePickerVisibleDelegate del = new updateDateTimePickerVisibleDelegate(updateDateTimePickerV);
                    dtp.BeginInvoke(del, new object[] { dtp, isVisible });
                }
                else
                    dtp.Visible = isVisible;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.updateDateTimePickerVisible(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void updateDateTimePickerV(DateTimePicker dtp, bool isVisible)
        {
            dtp.Visible = isVisible;
        }

        private void updateButtonEnabled(Button btn, bool isEnabled)
        {
            try
            {
                if (btn.InvokeRequired)
                {
                    updateButtonEnabledDelegate del = new updateButtonEnabledDelegate(updateButtonE);
                    btn.BeginInvoke(del, new object[] { btn, isEnabled });
                }
                else
                    btn.Enabled = isEnabled;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.updateButtonEnabled(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void updateButtonE(Button btn, bool isEnabled)
        {
            btn.Enabled = isEnabled;
        }

        private void updateLabelText(Label lbl, string newText)
        {
            try
            {
                if (lbl.InvokeRequired)
                {
                    updateLabelTextDelegate del = new updateLabelTextDelegate(updateLabel);
                    lbl.BeginInvoke(del, new object[] { lbl, newText });
                }
                else
                    lbl.Text = newText;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.updateLabelText(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void updateLabel(Label lbl, string newText)
        {
            lbl.Text = newText;
        }

        private void updateLabelVisible(Label lbl, bool isVisible)
        {
            try
            {
                if (lbl.InvokeRequired)
                {
                    updateLabelVisibleDelegate del = new updateLabelVisibleDelegate(updateLabelV);
                    lbl.BeginInvoke(del, new object[] { lbl, isVisible });
                }
                else
                    lbl.Visible = isVisible;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.updateLabelVisible(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void updateLabelV(Label lbl, bool isVisible)
        {
            lbl.Visible = isVisible;
        }
        #endregion

        private void Read()
        {
            try
            {
                while (true)
                {
                    if (!stopReading)
                    {
                        // find terminal (provera negde na pocetku)
                        string terminalID = GetDesktopReaderProductInfoPort();
                        if (terminalID.Trim().Length == 0)
                        {
                            MessageBox.Show(rm.GetString("noTerminalFound", culture));
                        }

                        // detect RFID card (kad god je potrebno)
                        uint tagID = GetTagSerialNumber();

                        if (tagID > 0)
                        {
                            if (tagID != oldTagID)
                            {
                                oldTagID = tagID;
                                if (modulesList.Contains((int)Constants.Moduls.RestaurantI))
                                {
                                    empl = new Employee().FindEmplMealType(tagID);
                                    if (empl.EmployeeID != -1)
                                    {
                                        populateEmployeeBox();
                                        populateMealsBox();
                                    }
                                    else
                                    {
                                        serviceFailed(rm.GetString("notValidTag", culture));
                                    }
                                }
                                if (modulesList.Contains((int)Constants.Moduls.RestaurantII))
                                {
                                    List<EmployeeTO> employees = new Employee().SearchByTags(oldTagID.ToString().Trim());
                                    if (employees.Count == 1)
                                    {
                                        empl = employees[0];
                                    }

                                    if (empl.EmployeeID != -1)
                                    {
                                        ArrayList scheduleList = new MealsEmployeeSchedule().SearchForEmpl(DateTime.Now.Date, DateTime.Now.Date, empl.EmployeeID);
                                        if (scheduleList.Count == 1)
                                        {
                                            populateEmployeeBox();
                                            populateMealsBox();
                                        }
                                        else
                                        {
                                            serviceFailed(rm.GetString("notMealForToday", culture));
                                        }
                                    }
                                    else
                                    {
                                        serviceFailed(rm.GetString("notEmplFound", culture));
                                    }
                                }
                            }
                        }
                        else
                        {
                            clearFormThread();
                            oldTagID = tagID;
                        }

                        Thread.Sleep(500);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.Read(): " + ex.Message + "\n");
                serviceFailed(ex.Message);
            }
        }

        private void UsingMeals_Closed(object sender, EventArgs e)
        {
            StopThread();
        }

        public void StopThread()
        {
            try
            {
                this.stopReading = true;
                if (reader != null)
                {
                    reader.Join(); // Wait for the worker's thread to finish.
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.StopThread(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public string GetDesktopReaderProductInfo()
        {
            try
            {
                ReaderFactory.TechnologyType = "MIFARE";

                IReaderInterface ri = ReaderFactory.GetReader;

                if (desktopReaderPort == 0)
                {
                    try
                    {
                        desktopReaderPort = readerInterface.FindDesktopReader();
                    }

                    catch
                    {
                        desktopReaderPort = ri.FindDesktopReader();
                    }
                }

                Thread.Sleep(200);

                string desktopReaderProductInfo = ri.GetProductInfo(desktopReaderPort);

                return desktopReaderProductInfo;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.GetDesktopReaderProductInfo(): " + ex.Message + "\n");
                return "";
            }
        }

        public string GetDesktopReaderProductInfoPort()
        {
            try
            {
                ReaderFactory.TechnologyType = "MIFARE";
                IReaderInterface ri = ReaderFactory.GetReader;
                string desktopReaderProductInfo = "";
                if (desktopReaderPort == 0)
                {
                    try
                    {
                        desktopReaderPort = readerInterface.FindDesktopReader();
                    }
                    catch
                    {
                        desktopReaderPort = ri.FindDesktopReader();
                    }
                }
                Thread.Sleep(200);

                bool desktopReaderPortExists = false;
                string[] serialPorts = SerialPort.GetPortNames();
                foreach (string serialPortName in serialPorts)
                {
                    int serialPortNumber = Int32.Parse(serialPortName.Substring(3));
                    if (serialPortNumber == desktopReaderPort)
                    {
                        desktopReaderPortExists = true;
                        break;
                    }
                }
                if (!desktopReaderPortExists) return String.Empty;

                desktopReaderProductInfo = ri.GetProductInfo(desktopReaderPort);
                return desktopReaderProductInfo;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.GetDesktopReaderProductInfoPort(): " + ex.Message + "\n");
                return "";
            }
        }

        public uint GetTagSerialNumber()
        {
            try
            {
                ReaderFactory.TechnologyType = "MIFARE";
                IReaderInterface ri = ReaderFactory.GetReader;
                if (desktopReaderPort == 0)
                {
                    try
                    {
                        desktopReaderPort = Int32.Parse(ConfigurationManager.AppSettings["RFIDDeviceComPort"]);
                    }
                    catch
                    {
                        desktopReaderPort = ri.FindDesktopReader();
                    }
                }
                Thread.Sleep(200);
                uint tagID = UInt32.Parse(ri.GetTagID(desktopReaderPort));
                return tagID;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.GetTagSerialNumber(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void serviceFailed(string failed)
        {
            try
            {
                panel1.BackColor = Color.Red;
                updateLabelText(lblStatus, rm.GetString("lblStatusFailed", culture));
                updateLabelVisible(lblFailed, true);
                updateLabelText(lblFailed, failed);

                updateButtonEnabled(btnNext, true);

                //StopThread();
                this.stopReading = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.serviceFailed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void useFailed(string failed)
        {
            try
            {
                panel1.BackColor = Color.Red;
                lblStatus.Text = rm.GetString("lblStatusFailed", culture);
                lblFailed.Text = failed;
                lblFailed.Visible = true;

                btnNext.Enabled = true;
                //btnUse.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.useFailed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void usePassed()
        {
            try
            {
                panel1.BackColor = Color.MediumSeaGreen;
                lblStatus.Text = rm.GetString("lblStatusPassed", culture);
                lblMealType.Text = rm.GetString("lblMealType", culture);
                tbMealType.Visible = true;
                tbQty.Visible = true;
                lblFailed.Visible = false;
                if (gbMeals.Visible)
                {
                    tbMealType.Text = mealAssigned.MealTypeName;
                }
                else
                {
                    tbMealType.Text = tbMealTypeII.Text.ToString() ;
                    lblQty.Visible = tbQty.Visible =lblMeals.Visible = false;
                }
                if (mealAssigned.Quantity != Constants.undefined)
                {
                    lblQty.Text = rm.GetString("lblQty", culture);
                    tbQty.Text = "1";
                    lblMeals.Text = rm.GetString("lblMeal", culture);
                }
                else if (mealAssigned.MoneyAmount != Constants.undefined)
                {
                    lblQty.Text = rm.GetString("lblSum", culture);
                    tbQty.Text = tbPrice.Text;
                    lblMeals.Text = rm.GetString("lblMoney", culture);
                }

                btnNext.Enabled = true;
                btnUse.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.usePassed(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                clearForm();
                StartThread();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateEmployeeBox()
        {
            try
            {
                // update employee box
                updateTextBoxText(tbEmpl, empl.LastName + " " + empl.FirstName);
                updateTextBoxText(tbType, empl.MealTypeName);

                EmployeeImageFile eif = new EmployeeImageFile();
                bool useDatabaseFiles = false;
                int databaseCount = eif.SearchCount(-1);
                if (databaseCount >= 0)
                    useDatabaseFiles = true;

                if (!empl.Picture.Equals(""))
                {
                    if (!useDatabaseFiles)
                    {
                        try
                        {
                            pbEmplPhoto.Image = new Bitmap(Constants.EmployeePhotoDirectory + "\\" + empl.Picture);
                        }
                        catch (Exception ex)
                        {
                            log.writeLog(DateTime.Now + " UsingMeals.Read(): " + ex.Message + "\n");
                            MessageBox.Show(rm.GetString("emplPhotoOpen", culture));
                        }
                    }
                    else
                    {
                        ArrayList al = eif.Search(empl.EmployeeID);
                        if (al.Count > 0)
                        {
                            byte[] emplPhoto = ((EmployeeImageFile)al[0]).Picture;

                            MemoryStream memStream = new MemoryStream(emplPhoto);

                            // Set the position to the beginning of the stream.
                            memStream.Seek(0, SeekOrigin.Begin);

                            pbEmplPhoto.Image = new Bitmap(memStream);

                            memStream.Close();
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("emplPhotoOpen", culture));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.populateEmployeeBox(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateMealsBox()
        {
            try
            {
                if (gbMeals.Visible)
                {
                    // update meal box
                    ArrayList mealsAssigned = new MealAssigned().Search(empl.EmployeeID.ToString().Trim());

                    if (mealsAssigned.Count == 1)
                    {
                        mealAssigned = (MealAssigned)mealsAssigned[0];

                        if (mealAssigned.Quantity != Constants.undefined && mealAssigned.MoneyAmount != Constants.undefined)
                        {
                            serviceFailed(rm.GetString("incorrectMealAssigned", culture));
                        }
                        else
                        {
                            ArrayList mealsUsed = new MealUsed().Search(empl.EmployeeID,
                                !mealAssigned.ValidFrom.Equals(new DateTime(1900, 1, 1)) ? mealAssigned.ValidFrom.Date : new DateTime(),
                                !mealAssigned.ValidTo.Equals(new DateTime(2099, 1, 1)) ? mealAssigned.ValidTo.Date : new DateTime());

                            int usedQty = 0;
                            int usedMoney = 0;
                            foreach (MealUsed meal in mealsUsed)
                            {
                                usedQty += meal.Quantity;
                                usedMoney += meal.MoneyAmount;
                            }

                            if (mealAssigned.Quantity != Constants.undefined)
                            {
                                int available = mealAssigned.Quantity - usedQty;

                                updateTextBoxText(tbUsed, usedQty.ToString());
                                updateLabelText(lblMealUsedCount, rm.GetString("lblMeal", culture));
                                if (mealAssigned.Quantity != Constants.unlimited)
                                {
                                    updateTextBoxText(tbAvailable, available.ToString());
                                }
                                else
                                {
                                    updateTextBoxText(tbAvailable, rm.GetString("lvUnlimited", culture));
                                }
                                updateLabelText(lblMealCount, rm.GetString("lblMeal", culture));

                                if (mealAssigned.ValidFrom.Date.Equals(new DateTime(1900, 1, 1).Date)
                                    && mealAssigned.ValidTo.Date.Equals(new DateTime(2099, 1, 1).Date))
                                {
                                    updateDateTimePickerVisible(dtpFrom, false);
                                    updateDateTimePickerVisible(dtpTo, false);

                                    updateLabelText(lblUnlimitedFrom, rm.GetString("lvUnlimited", culture));
                                    updateLabelText(lblUnlimitedTo, rm.GetString("lvUnlimited", culture));
                                }
                                else
                                {
                                    updateDateTimePickerValue(dtpFrom, mealAssigned.ValidFrom.Date);
                                    updateDateTimePickerValue(dtpTo, mealAssigned.ValidTo.Date);
                                }

                                // check if daily limit is already reached
                                ArrayList dailyMeals = new MealUsed().Search(empl.EmployeeID, DateTime.Now.Date, DateTime.Now.Date);

                                int qtyDaily = 0;
                                foreach (MealUsed daily in dailyMeals)
                                {
                                    qtyDaily += daily.Quantity;
                                }

                                if (mealAssigned.QuantityDaily != Constants.unlimited && qtyDaily >= mealAssigned.QuantityDaily)
                                {
                                    serviceFailed(rm.GetString("qtyDailyLimitReached", culture));
                                }
                                else if (available > 0 || mealAssigned.Quantity == Constants.unlimited)
                                {
                                    updateButtonEnabled(btnUse, true);
                                    updateButtonEnabled(btnNext, true);
                                    //StopThread();
                                    this.stopReading = true;
                                }
                                else
                                {
                                    serviceFailed(rm.GetString("noMealsAvailable", culture));
                                }
                            }
                            else if (mealAssigned.MoneyAmount != Constants.undefined)
                            {
                                int available = mealAssigned.MoneyAmount - usedMoney;

                                updateTextBoxText(tbUsed, usedMoney.ToString());
                                updateLabelText(lblMealUsedCount, rm.GetString("lblMoney", culture));
                                if (mealAssigned.MoneyAmount != Constants.unlimited)
                                {
                                    updateTextBoxText(tbAvailable, available.ToString());
                                }
                                else
                                {
                                    updateTextBoxText(tbAvailable, rm.GetString("lvUnlimited", culture));
                                }
                                updateLabelText(lblMealCount, rm.GetString("lblMoney", culture));

                                if (mealAssigned.ValidFrom.Date.Equals(new DateTime(1900, 1, 1).Date)
                                    && mealAssigned.ValidTo.Date.Equals(new DateTime(2099, 1, 1).Date))
                                {
                                    updateDateTimePickerVisible(dtpFrom, false);
                                    updateDateTimePickerVisible(dtpTo, false);

                                    updateLabelText(lblUnlimitedFrom, rm.GetString("lvUnlimited", culture));
                                    updateLabelText(lblUnlimitedTo, rm.GetString("lvUnlimited", culture));
                                }
                                else
                                {
                                    updateDateTimePickerValue(dtpFrom, mealAssigned.ValidFrom.Date);
                                    updateDateTimePickerValue(dtpTo, mealAssigned.ValidTo.Date);
                                }

                                if (available > 0 || mealAssigned.MoneyAmount == Constants.unlimited)
                                {
                                    updateLabelText(lblPrice, rm.GetString("lblMoneyAmt", culture));
                                    updateLabelText(lblMealPrice, rm.GetString("lblMoney", culture));
                                    updateTextBoxVisible(tbPrice, true);
                                    updateButtonEnabled(btnUse, true);
                                    updateButtonEnabled(btnNext, true);
                                    //StopThread();
                                    this.stopReading = true;
                                }
                                else
                                {
                                    serviceFailed(rm.GetString("noMealsAvailable", culture));
                                }
                            }
                        }
                    }
                    else if (mealsAssigned.Count > 1)
                    {
                        serviceFailed(rm.GetString("moreThenOneMealAssigned", culture));
                    }
                    else
                    {
                        serviceFailed(rm.GetString("noMealAssigned", culture));
                    }
                }
                else
                {
                    mealSchedule = new MealsEmployeeSchedule();
                    ArrayList list = mealSchedule.SearchForEmpl(DateTime.Now.Date, DateTime.Now.Date, empl.EmployeeID);
                    if (list.Count == 1)
                    {
                        mealSchedule = (MealsEmployeeSchedule)list[0];
                        updateTextBoxText(tbMealTypeII, mealSchedule.MealsType);

                        MealType mealType = new MealType();
                        ArrayList mealTypeList = mealType.Search(mealSchedule.MealTypeID, "", "", new DateTime(), new DateTime());
                        if (mealTypeList.Count > 0)
                        {
                            mealType = (MealType)mealTypeList[0];
                            ArrayList schedules = new MealsTypeSchedule().Search(mealType.MealTypeID, DateTime.Now.Date, DateTime.Now.Date);
                            updateTextBoxText(tbEmpl,empl.FirstName+" "+empl.LastName);
                            if (schedules.Count > 0)
                            {
                                MealsTypeSchedule schedule = (MealsTypeSchedule)schedules[0];
                                updateDateTimePickerValue(dtpFromII, schedule.HoursFrom);
                                updateDateTimePickerValue(dtpToII,schedule.HoursTo);
                            }
                            updateTextBoxVisible(tbPrice, true);
                            updateButtonEnabled(btnUse, true);
                            updateButtonEnabled(btnNext, true);

                            this.stopReading = true;
                        }
                    }
                    else if (list.Count > 1)
                    {
                        serviceFailed(rm.GetString("moreThenOneMealAssigned", culture));
                    }
                    else
                    {
                        serviceFailed(rm.GetString("noMealAssigned", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.populateMealsBox(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                clearStatusPanel();

                // find point id
                string terminalID = GetDesktopReaderProductInfo();

                ArrayList points = new MealPoint().Search(-1, terminalID, "", "");

                if (points.Count > 0)
                {
                    if (gbMeals.Visible)
                    {
                        if (mealAssigned.Quantity != Constants.undefined)
                        {
                            // change trans_id and money
                            if (new MealUsed().Save(DateTime.Now.Ticks.ToString(), empl.EmployeeID, DateTime.Now, ((MealPoint)points[0]).MealPointID, mealAssigned.MealTypeID, 1, Constants.undefined) > 0)
                            {
                                usePassed();
                            }
                            else
                            {
                                useFailed(rm.GetString("useFailed", culture));
                            }
                        }
                        else if (mealAssigned.MoneyAmount != Constants.undefined)
                        {
                            int price = 0;

                            if (Int32.TryParse(tbPrice.Text.Trim(), out price))
                            {
                                if (new MealUsed().Save(DateTime.Now.Ticks.ToString(), empl.EmployeeID, DateTime.Now, ((MealPoint)points[0]).MealPointID, mealAssigned.MealTypeID, Constants.undefined, price) > 0)
                                {
                                    usePassed();
                                }
                                else
                                {
                                    useFailed(rm.GetString("useFailed", culture));
                                }
                            }
                            else
                            {
                                useFailed(rm.GetString("incorrectMealPrice", culture));
                            }
                        }
                    }
                    else
                    {
                        // change trans_id and money
                        if (new MealUsed().Save(DateTime.Now.Ticks.ToString(), empl.EmployeeID, DateTime.Now, ((MealPoint)points[0]).MealPointID, mealSchedule.MealTypeID, 1, Constants.undefined) > 0)
                        {
                            usePassed();
                        }
                        else
                        {
                            useFailed(rm.GetString("useFailed", culture));
                        }
                    }
                }
                else
                {
                    useFailed(rm.GetString("useFailed", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UsingMeals.btnUse_Click(): " + ex.Message + "\n");
                useFailed(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}