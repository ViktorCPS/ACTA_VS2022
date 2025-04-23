using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class EmployeeAdditionalData : Form
    {
        private const int height = 25;
        private const int lblWidth = 100;        
        private const int tbWidth = 200;        
        private const int dtpWidth = 130;        
        private const int numWidth = 80;

        private int startX = 10;
        private int startY = 10;

        private const int x = 10;
        private const int y = 10;

        private DateTime dtNullValue = new DateTime(1900, 1, 1);

        EmployeeAsco4TO addDataTO = new EmployeeAsco4TO();
        EmployeeAsco4TO oldAddDataTO = new EmployeeAsco4TO();
        int emplID = -1;

        private CultureInfo culture;                
        DebugLog log;
        ResourceManager rm;
        

        public EmployeeAdditionalData(EmployeeAsco4TO addData, int emplID)
        {
            try
            {
                InitializeComponent();
                
                // Init Debug 
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                if (addData.EmployeeID != -1)
                    this.oldAddDataTO = new EmployeeAsco4TO(addData);
                this.addDataTO = addData;
                this.emplID = emplID;

                setLanguage();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("EmployeeAddData", culture);
                
                gbData.Text = rm.GetString("gbData", culture);

                if (this.addDataTO.EmployeeID == -1)
                    btnSave.Text = rm.GetString("btnSave", culture);
                else
                    btnSave.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                lblInfo.Text = rm.GetString("lblInfoAddData", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAdditionalData.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EmployeeAdditionalData_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeDataPanels();             
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeeAdditionalData.EmployeeAdditionalData_Load(): " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializeDataPanels()
        {
            try
            {
                Dictionary<string, string> metadata = new EmployeeAsco4Metadata().GetMetadataValues(NotificationController.GetLanguage());

                int xIntPos = startX;
                int yIntPos = startY;
                int xDTPos = startX;
                int yDTPos = startY;
                int xTextPos = startX;
                int yTextPos = startY;
                
                                
                foreach (string col in metadata.Keys)
                {
                    if (!metadata[col].Trim().Equals(""))
                    {
                        // create label
                        Label lbl = new Label();
                        lbl.Width = lblWidth;
                        lbl.Height = height;                        
                        lbl.Text = metadata[col].Trim() + ":";
                        lbl.TextAlign = ContentAlignment.MiddleRight;
                        lbl.Tag = col;

                        DateTime dtValue = new DateTime();
                        int intValue = -1;
                        string textValue = "";

                        switch (col)
                        {
                            case "datetime_value_1":
                                dtValue = this.addDataTO.DatetimeValue1;
                                break;
                            case "datetime_value_2":
                                dtValue = this.addDataTO.DatetimeValue2;
                                break;
                            case "datetime_value_3":
                                dtValue = this.addDataTO.DatetimeValue3;
                                break;
                            case "datetime_value_4":
                                dtValue = this.addDataTO.DatetimeValue4;
                                break;
                            case "datetime_value_5":
                                dtValue = this.addDataTO.DatetimeValue5;
                                break;
                            case "datetime_value_6":
                                dtValue = this.addDataTO.DatetimeValue6;
                                break;
                            case "datetime_value_7":
                                dtValue = this.addDataTO.DatetimeValue7;
                                break;
                            case "datetime_value_8":
                                dtValue = this.addDataTO.DatetimeValue8;
                                break;
                            case "datetime_value_9":
                                dtValue = this.addDataTO.DatetimeValue9;
                                break;
                            case "datetime_value_10":
                                dtValue = this.addDataTO.DatetimeValue10;
                                break;
                            case "integer_value_1":
                                intValue = this.addDataTO.IntegerValue1;
                                break;
                            case "integer_value_2":
                                intValue = this.addDataTO.IntegerValue2;
                                break;
                            case "integer_value_3":
                                intValue = this.addDataTO.IntegerValue3;
                                break;
                            case "integer_value_4":
                                intValue = this.addDataTO.IntegerValue4;
                                break;
                            case "integer_value_5":
                                intValue = this.addDataTO.IntegerValue5;
                                break;
                            case "integer_value_6":
                                intValue = this.addDataTO.IntegerValue6;
                                break;
                            case "integer_value_7":
                                intValue = this.addDataTO.IntegerValue7;
                                break;
                            case "integer_value_8":
                                intValue = this.addDataTO.IntegerValue8;
                                break;
                            case "integer_value_9":
                                intValue = this.addDataTO.IntegerValue9;
                                break;
                            case "integer_value_10":
                                intValue = this.addDataTO.IntegerValue10;
                                break;
                            case "nvarchar_value_1":
                                textValue = this.addDataTO.NVarcharValue1.Trim();
                                break;
                            case "nvarchar_value_2":
                                textValue = this.addDataTO.NVarcharValue2.Trim();
                                break;
                            case "nvarchar_value_3":
                                textValue = this.addDataTO.NVarcharValue3.Trim();
                                break;
                            case "nvarchar_value_4":
                                textValue = this.addDataTO.NVarcharValue4.Trim();
                                break;
                            case "nvarchar_value_5":
                                textValue = this.addDataTO.NVarcharValue5.Trim();                                
                                break;
                            case "nvarchar_value_6":
                                textValue = this.addDataTO.NVarcharValue6.Trim();
                                break;
                            case "nvarchar_value_7":
                                textValue = this.addDataTO.NVarcharValue7.Trim();
                                break;
                            case "nvarchar_value_8":
                                textValue = this.addDataTO.NVarcharValue8.Trim();
                                break;
                            case "nvarchar_value_9":
                                textValue = this.addDataTO.NVarcharValue9.Trim();
                                break;
                            case "nvarchar_value_10":
                                textValue = this.addDataTO.NVarcharValue10.Trim();
                                break;
                        }

                        if (col.StartsWith("datetime_value"))
                        {
                            lbl.Location = new Point(xDTPos, yDTPos);
                            xDTPos += lblWidth + x;

                            // create DateTimePicker
                            DateTimePicker dtp = new DateTimePicker();
                            dtp.Width = dtpWidth;
                            dtp.Height = height;
                            dtp.Format = DateTimePickerFormat.Custom;
                            dtp.CustomFormat = "dd.MM.yyyy. HH:mm";
                            dtp.Location = new Point(xDTPos, yDTPos);

                            if (!dtValue.Equals(new DateTime()))
                                dtp.Value = dtValue;
                            else
                                dtp.Value = dtNullValue;

                            dtp.Tag = col;

                            xDTPos = startX;
                            yDTPos += height + y;

                            panelDateTimeData.Controls.Add(lbl);
                            panelDateTimeData.Controls.Add(dtp);

                            //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                            /*
                            dtp.Enabled = false;
                            */
                        }
                        else if (col.StartsWith("integer_value"))
                        {
                            lbl.Location = new Point(xIntPos, yIntPos);
                            xIntPos += lblWidth + x;

                            // create NumericUpDown
                            NumericUpDown num = new NumericUpDown();
                            num.Width = numWidth;
                            num.Height = height;
                            num.Location = new Point(xIntPos, yIntPos);
                            num.Minimum = -10000;
                            num.Maximum = 1000000000;
                            num.TextAlign = HorizontalAlignment.Right;

                            num.Value = intValue;

                            num.Tag = col;

                            xIntPos = startX;
                            yIntPos += height + y;

                            panelIntData.Controls.Add(lbl);
                            panelIntData.Controls.Add(num);
                            
                                
                        }
                        else if (col.StartsWith("nvarchar_value"))
                        {
                            lbl.Location = new Point(xTextPos, yTextPos);
                            xTextPos += lblWidth + x;

                            // create NumericUpDown
                            TextBox tb = new TextBox();
                            tb.Width = tbWidth;
                            tb.Height = height;
                            tb.MaxLength = 256;
                            tb.Location = new Point(xTextPos, yTextPos);
                            tb.Text = textValue.Trim();
                            tb.Tag = col;

                            xTextPos = startX;
                            yTextPos += height + y;

                            panelTextData.Controls.Add(lbl);
                            panelTextData.Controls.Add(tb);
                            toolTip1.SetToolTip(tb, textValue.Trim());
                            //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                            /*
                            if (col.StartsWith("nvarchar_value_3") || col.StartsWith("nvarchar_value_4") || col.StartsWith("nvarchar_value_10"))
                            {
                                tb.Enabled = false;
                            }
                              */
                            
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeeAdditionalData.InitializeDataPanel(): " + ex.Message);
                throw ex;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (Control ctrl in panelIntData.Controls)
                {
                    if (ctrl is NumericUpDown)
                    {
                        switch (((NumericUpDown)ctrl).Tag.ToString())
                        {
                            case "integer_value_1":
                                this.addDataTO.IntegerValue1 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_2":
                                this.addDataTO.IntegerValue2 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_3":
                                this.addDataTO.IntegerValue3 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_4":
                                this.addDataTO.IntegerValue4 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_5":
                                this.addDataTO.IntegerValue5 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_6":
                                this.addDataTO.IntegerValue6 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_7":
                                this.addDataTO.IntegerValue7 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_8":
                                this.addDataTO.IntegerValue8 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_9":
                                this.addDataTO.IntegerValue9 = (int)((NumericUpDown)ctrl).Value;
                                break;
                            case "integer_value_10":
                                this.addDataTO.IntegerValue10 = (int)((NumericUpDown)ctrl).Value;
                                break;
                        }
                    }
                }

                foreach (Control ctrl in panelDateTimeData.Controls)
                {
                    if (ctrl is DateTimePicker)
                    {
                        DateTime dtValue = new DateTime();
                        if (!((DateTimePicker)ctrl).Value.Date.Equals(dtNullValue.Date))
                            dtValue = ((DateTimePicker)ctrl).Value;

                        switch (((DateTimePicker)ctrl).Tag.ToString())
                        {
                            case "datetime_value_1":
                                this.addDataTO.DatetimeValue1 = dtValue;
                                break;
                            case "datetime_value_2":
                                this.addDataTO.DatetimeValue2 = dtValue;
                                break;
                            case "datetime_value_3":
                                this.addDataTO.DatetimeValue3 = dtValue;
                                break;
                            case "datetime_value_4":
                                this.addDataTO.DatetimeValue4 = dtValue;
                                break;
                            case "datetime_value_5":
                                this.addDataTO.DatetimeValue5 = dtValue;
                                break;
                            case "datetime_value_6":
                                this.addDataTO.DatetimeValue6 = dtValue;
                                break;
                            case "datetime_value_7":
                                this.addDataTO.DatetimeValue7 = dtValue;
                                break;
                            case "datetime_value_8":
                                this.addDataTO.DatetimeValue8 = dtValue;
                                break;
                            case "datetime_value_9":
                                this.addDataTO.DatetimeValue9 = dtValue;
                                break;
                            case "datetime_value_10":
                                this.addDataTO.DatetimeValue10 = dtValue;
                                break;
                        }
                    }
                }

                foreach (Control ctrl in panelTextData.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        switch (((TextBox)ctrl).Tag.ToString())
                        {
                            case "nvarchar_value_1":
                                this.addDataTO.NVarcharValue1 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_2":
                                this.addDataTO.NVarcharValue2 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_3":
                                this.addDataTO.NVarcharValue3 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_4":
                                this.addDataTO.NVarcharValue4 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_5":
                                this.addDataTO.NVarcharValue5 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_6":
                                this.addDataTO.NVarcharValue6 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_7":
                                this.addDataTO.NVarcharValue7 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_8":
                                this.addDataTO.NVarcharValue8 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_9":
                                this.addDataTO.NVarcharValue9 = ((TextBox)ctrl).Text.Trim();
                                break;
                            case "nvarchar_value_10":
                                this.addDataTO.NVarcharValue10 = ((TextBox)ctrl).Text.Trim();
                                break;
                        }
                    }
                }

                EmployeeAsco4 emplAsco4 = new EmployeeAsco4();
                if (emplAsco4.BeginTransaction())
                {
                    try
                    {
                        bool updated = true;
                        string msg = "";
                        if (this.addDataTO.EmployeeID == -1)
                        {
                            // save additional data
                            this.addDataTO.EmployeeID = this.emplID;
                            emplAsco4.EmplAsco4TO = this.addDataTO;
                            updated = emplAsco4.save(false);
                            msg = "EmplAddDataSaved";
                        }
                        else
                        {
                            // save history
                            EmployeeAsco4Hist hist = new EmployeeAsco4Hist();
                            hist.SetTransaction(emplAsco4.GetTransaction());
                            hist.EmplAsco4TO = oldAddDataTO;
                            updated = hist.save(false);
                            if (updated)
                            {
                                // update existing additional data                    
                                emplAsco4.EmplAsco4TO = this.addDataTO;
                                updated = emplAsco4.update(false);
                                msg = "EmplAddDataUpdated";
                            }
                        }

                        if (updated)
                        {
                            emplAsco4.CommitTransaction();
                            MessageBox.Show(rm.GetString(msg, culture));
                            this.Close();
                        }
                        else
                        {
                            if (emplAsco4.GetTransaction() != null)
                                emplAsco4.RollbackTransaction();

                            if (this.addDataTO.EmployeeID == -1)
                                MessageBox.Show(rm.GetString("EmplAddDataNotSaved", culture));
                            else
                                MessageBox.Show(rm.GetString("EmplAddDataNotUpdated", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (emplAsco4.GetTransaction() != null)
                            emplAsco4.RollbackTransaction();
                        
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeeAdditionalData.btnSave_Click(): " + ex.Message);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void EmployeeAdditionalData_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeeAdditionalData.EmployeeAdditionalData_FormClosed(): " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
