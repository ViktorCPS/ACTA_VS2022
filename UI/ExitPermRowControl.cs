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
    public partial class ExitPermRowControl : UserControl
    {
        DebugLog log;
        private DateTime from;
        private DateTime to;
        public  DateTime earliestArrivalTime;
        public  DateTime latestArrivalTime;

        private DateTime Date;

        //permission that row represent
        public ExitPermissionTO currentPerm;
        //show if row is checked
        public bool insert;
        //show if row represent's begining of the day permission
        public string PermissionType;

        public int passTypeId;

       private int listNum;

        public WorkTimeSchemaTO timeSchema;

        private DateTime start;

        public DateTime Start
        {
            get { return start; }
            set
            { 
                start = value;
                dtpExitPermStartTime.Value = value;
            }
        }

        ExitPermControl ParentControl;

        public ExitPermRowControl()
        {
            currentPerm = new ExitPermissionTO();
            PermissionType = "";
            listNum = -1;
            passTypeId =-1;
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);           
            
            setVisibility(false);
            insert = false;
            timeSchema = new WorkTimeSchemaTO();
            earliestArrivalTime = new DateTime();
            latestArrivalTime =new DateTime();
            timeSchema = new WorkTimeSchemaTO();
            this.from = new DateTime();
            this.to = new DateTime();

        }

        public ExitPermRowControl(List<PassTypeTO> passTypes, int passType, DateTime from, DateTime to, bool disable, EmployeeTO employee, DateTime date, string IOPairPassType,string permType, ExitPermControl parent,int numInList, string desc, WorkTimeSchemaTO ts, DateTime earlArrive, DateTime latArrive)
        {
            currentPerm = new ExitPermissionTO();
            PermissionType = permType;
            ParentControl = parent;
            listNum = numInList;
            passTypeId = passType;
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            Date = date;

            earliestArrivalTime = earlArrive;
            latestArrivalTime = latArrive;
            timeSchema = ts;
            this.from = from;
            this.to = to;

            if (disable)//&&passType ==-1)
            {
                cbPassType.Items.Add(IOPairPassType);
                cbPassType.SelectedIndex = 0;
                insert = false;
            }
            else
            {
                chbInsertPerm.Checked = true;
                cbPassType.DataSource = passTypes;
                cbPassType.DisplayMember = "Description";
                cbPassType.ValueMember = "PassTypeID";
                cbPassType.SelectedValue = passType;
                insert = true;
                tbDescription.Text = desc;                
            }
            tbFrom.Text = from.ToString("HH:mm");
            tbTo.Text = to.ToString("HH:mm");
            if(cbPassType.SelectedValue!=null)
                currentPerm.PassTypeID = (int)cbPassType.SelectedValue;
            currentPerm.StartTime = new DateTime(date.Year, date.Month, date.Day, from.Hour, from.Minute, from.Second);
            currentPerm.Offset = 0;
            currentPerm.Description = tbDescription.Text.Trim();
            currentPerm.Used = (int)Constants.Used.Yes;
            currentPerm.EmployeeID = employee.EmployeeID;

            setVisibility(disable);
        }

        private void setVisibility(bool disable)
        {
            try
            {
                tbFrom.Enabled = false;
                tbTo.Enabled = false;
                if (disable)
                {
                    cbPassType.Enabled = false;
                    tbDescription.Enabled = false;
                    chbInsertPerm.Visible = false;
                }
                if (timeSchema!=null && timeSchema.Type.Equals(Constants.schemaTypeFlexi) && (PermissionType.Equals(ExitPermControl.isShiftStart)||PermissionType.Equals(ExitPermControl.isStart)))
                {
                    dtpExitPermStartTime.Visible = true;
                }
                else
                    dtpExitPermStartTime.Visible = false;
            }
            catch(Exception ex)
            {
                log.writeLog(DateTime.Now + " exitPermRowControl.setVisibility(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void chbInsertPerm_CheckedChanged(object sender, EventArgs e)
        {
             insert = chbInsertPerm.Checked;
             if (!chbInsertPerm.Checked)
             {
                 tbDescription.BackColor = SystemColors.InactiveCaptionText;
                 cbPassType.BackColor = SystemColors.InactiveCaptionText;
             }
             else
             {
                 tbDescription.BackColor = SystemColors.Window;
                cbPassType.BackColor = SystemColors.Window;
             }
            if( !(PermissionType.Equals("") || PermissionType.Equals(ExitPermControl.isStart)))
                ParentControl.checkingControl(listNum);
            
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                currentPerm.Description = tbDescription.Text.Trim();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " exitPermRowControl.tbDescription_TextChanged() " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbPassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

               if (cbPassType.SelectedValue != null)
                    currentPerm.PassTypeID = (int)cbPassType.SelectedValue;
            }
            catch
            {
                currentPerm.PassTypeID = ((PassTypeTO)cbPassType.SelectedValue).PassTypeID;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        public void changeCheck(bool check)
        {
            chbInsertPerm.Checked = check;            
        }

        private void dtpExitPermStartTime_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!earliestArrivalTime.Equals(new DateTime()) && !latestArrivalTime.Equals(new DateTime()))
                {
                    if (dtpExitPermStartTime.Value.TimeOfDay > latestArrivalTime.TimeOfDay)
                        dtpExitPermStartTime.Value = latestArrivalTime;
                    if (dtpExitPermStartTime.Value.TimeOfDay < earliestArrivalTime.TimeOfDay)
                        dtpExitPermStartTime.Value = earliestArrivalTime;
                    if (dtpExitPermStartTime.Value.TimeOfDay > this.to.TimeOfDay)
                        dtpExitPermStartTime.Value = to;
                }
                start = dtpExitPermStartTime.Value;
                currentPerm.StartTime = new DateTime(Date.Year, Date.Month, Date.Day, start.Hour, start.Minute, start.Second);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " exitPermRowControl.dtpExitPermStartTime_ValueChanged() " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }        
    }
}

