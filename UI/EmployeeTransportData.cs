using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class EmployeeTransportData : UserControl
    {
        private const int idLblWidth = 56;
        private const int idColWidth = 60;
        private const int emplLblWidth = 126;
        private const int emplColWidth = 130;
        private const int monthCbWidth = 96;
        private const int monthColWidth = 100;
        private const int colHeight = 20;
        
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        EmployeeTO empl = new EmployeeTO();
        Dictionary<DateTime, EmployeePYTransportDataTO> data = new Dictionary<DateTime, EmployeePYTransportDataTO>();
        DateTime month = new DateTime();
        Dictionary<int, EmployeePYTransportTypeTO> typeDict = new Dictionary<int, EmployeePYTransportTypeTO>();
        List<EmployeePYTransportTypeTO> typeList = new List<EmployeePYTransportTypeTO>();
        Dictionary<DateTime, List<WorkTimeIntervalTO>> intervals = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();

        bool initialiazing = false;
 
        public EmployeeTransportData(EmployeeTO empl, Dictionary<DateTime, EmployeePYTransportDataTO> data, DateTime month, Dictionary<int, EmployeePYTransportTypeTO> typeDict, 
            List<EmployeePYTransportTypeTO> typeList, Dictionary<DateTime, List<WorkTimeIntervalTO>> intervals, int xPos, int yPos, bool header)
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(EmployeeTransportData).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                this.empl = empl;
                this.data = data;
                this.month = month;
                this.typeDict = typeDict;
                this.typeList = typeList;
                this.intervals = intervals;
                
                DateTime firstDay = new DateTime(month.Year, month.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                this.Location = new Point(xPos, yPos);
                this.Width = idColWidth + emplColWidth + lastDay.Day * monthColWidth;

                if (header)
                    InitializeHeader(firstDay, lastDay);
                else
                    InitializeControl(firstDay, lastDay);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeHeader(DateTime firstDay, DateTime lastDay)
        {
            try
            {
                int xLbl = idColWidth - idLblWidth;
                int xPos = xLbl / 2;
                int yPos = (this.Height - colHeight) / 2;
                int index = 0;

                // create employee id label
                this.Controls.Add(CreateLabel(rm.GetString("hdrID", culture), true, xPos, yPos, idLblWidth, index++));
                xPos += idColWidth;

                // create employee label
                this.Controls.Add(CreateLabel(rm.GetString("hdrEmployee", culture), true, xPos, yPos, emplLblWidth, index++));
                xPos += emplColWidth;
                for (DateTime day = firstDay; day <= lastDay; day = day.AddDays(1))
                {
                    this.Controls.Add(CreateLabel(day.Day.ToString().Trim(), true, xPos, yPos, monthCbWidth, index++));
                    xPos += monthColWidth;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeTransportData.InitializeHeader(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void InitializeControl(DateTime firstDay, DateTime lastDay)
        {
            try
            {
                int xLbl = idColWidth - idLblWidth;
                int xPos = xLbl / 2;
                int yPos = (this.Height - colHeight) / 2;
                int index = 0;

                // create employee id label
                this.Controls.Add(CreateLabel(empl.EmployeeID.ToString().Trim(), false, xPos, yPos, idLblWidth, index++));
                xPos += idColWidth;

                // create employee label
                this.Controls.Add(CreateLabel(empl.FirstAndLastName, false, xPos, yPos, emplLblWidth, index++));
                xPos += emplColWidth;
                for (DateTime day = firstDay; day <= lastDay; day = day.AddDays(1))
                {
                    ComboBox cb = CreateComboBox(day, xPos, yPos, monthCbWidth);
                    this.Controls.Add(cb);

                    xPos += monthColWidth;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeTransportData.InitializeHeader(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private Label CreateLabel(string text, bool isHeader, int xPos, int yPos, int width, int index)
        {
            try
            {
                Label lbl = new Label();
                lbl.Name = empl.EmployeeID.ToString() + index.ToString().Trim();
                lbl.AutoSize = false;
                lbl.Width = width;
                lbl.Height = colHeight;
                lbl.Text = text.Trim();
                if (isHeader)
                {
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.BackColor = Color.Lavender;
                }
                else
                {
                    lbl.TextAlign = ContentAlignment.MiddleLeft;
                    lbl.BackColor = Color.White;
                }
                lbl.Location = new Point(xPos, yPos);

                return lbl;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeTransportData.CreateLabel(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private ComboBox CreateComboBox(DateTime day, int xPos, int yPos, int width)
        {
            try
            {
                ComboBox cb = new ComboBox();
                cb.Name = empl.EmployeeID.ToString().Trim() + day.Day.ToString().Trim();
                cb.Width = width;
                cb.Height = colHeight;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Location = new Point(xPos, yPos);
                cb.SelectedValueChanged += new EventHandler(cb_SelectedValueChanged);
                cb.Tag = day;

                List<EmployeePYTransportTypeTO> list = new List<EmployeePYTransportTypeTO>();
                foreach (EmployeePYTransportTypeTO typeTO in typeList)
                {
                    list.Add(new EmployeePYTransportTypeTO(typeTO));
                }

                cb.DataSource = list;
                cb.DisplayMember = "Name";
                cb.ValueMember = "TransportTypeID";

                return cb;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeTransportData.CreateComboBox(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetDataValues()
        {
            try
            {
                initialiazing = true;
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is ComboBox && ((ComboBox)ctrl).Tag != null && ((ComboBox)ctrl).Tag is DateTime)
                    {
                        int typeID = -1;
                        if (data.ContainsKey(((DateTime)(((ComboBox)ctrl).Tag)).Date))
                        {
                            ((ComboBox)ctrl).SelectedValue = data[((DateTime)(((ComboBox)ctrl).Tag)).Date].TransportTypeID;
                            typeID = data[((DateTime)(((ComboBox)ctrl).Tag)).Date].TransportTypeID;
                        }

                        // set back color
                        List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                        if (intervals.ContainsKey(((DateTime)(((ComboBox)ctrl).Tag)).Date))
                            dayIntervals = intervals[((DateTime)(((ComboBox)ctrl).Tag)).Date];

                        bool workingDay = false;
                        foreach (WorkTimeIntervalTO interval in dayIntervals)
                        {
                            if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            {
                                workingDay = true;
                                break;
                            }
                        }

                        if ((workingDay && typeID == -1) || (!workingDay && typeID != -1))
                            ((ComboBox)ctrl).BackColor = Color.LightCoral;
                        else
                            ((ComboBox)ctrl).BackColor = Color.White;
                    }
                }
                initialiazing = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeTransportData.SetDataValues(): " + ex.Message + "\n");
            }
        }

        public Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> GetDataValues()
        {
            Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> emplData = new Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>>();

            try
            {
                emplData.Add(empl.EmployeeID, data);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeTransportData.GetDataValues(): " + ex.Message + "\n");
                emplData = new Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>>();
            }

            return emplData;
        }

        private void cb_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!initialiazing && sender is ComboBox && ((ComboBox)sender).Tag != null && ((ComboBox)sender).Tag is DateTime && ((ComboBox)sender).SelectedValue != null && ((ComboBox)sender).SelectedValue is int)
                {
                    EmployeePYTransportDataTO dataTO = new EmployeePYTransportDataTO();
                    dataTO.EmployeeID = empl.EmployeeID;
                    dataTO.Date = (DateTime)(((ComboBox)sender).Tag);
                    dataTO.TransportTypeID = (int)(((ComboBox)sender).SelectedValue);

                    // set back color
                    List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                    if (intervals.ContainsKey(dataTO.Date.Date))
                        dayIntervals = intervals[dataTO.Date.Date];

                    bool workingDay = false;
                    foreach (WorkTimeIntervalTO interval in dayIntervals)
                    {
                        if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                        {
                            workingDay = true;
                            break;
                        }
                    }

                    if ((workingDay && dataTO.TransportTypeID == -1) || (!workingDay && dataTO.TransportTypeID != -1))
                        ((ComboBox)sender).BackColor = Color.LightCoral;
                    else
                        ((ComboBox)sender).BackColor = Color.White;

                    if (!data.ContainsKey(dataTO.Date.Date))
                        data.Add(dataTO.Date.Date, dataTO);
                    else
                        data[dataTO.Date.Date] = dataTO;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeTransportData.cb_SelectedValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
