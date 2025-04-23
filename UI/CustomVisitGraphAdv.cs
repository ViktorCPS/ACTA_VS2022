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
using System.IO;
using MySql.Data.MySqlClient;
using System.Drawing.PieChart;
using System.Drawing.Printing;

using Common;
using Util;
using TransferObjects;
using System.Drawing.Imaging;
using System.Threading;

namespace UI
{
    public partial class CustomVisitGraphAdv : Form
    {
        //GRAPH TYPE INDEX'S
        private const int indexWeekly = 0;
        private const int indexDayInterval = 1;
        private const int indexDayOfWeek = 2;

        //Graph sort index
        private const int barGraph = 0;
        private const int pieGraph = 1;

        const int MarginGap = 20;
        private int graphType;
        //GRAPH VALUES
        Dictionary<string, List<int>> valueTable = new Dictionary<string, List<int>>();
        //make list of strings for xAxis of graph
        ArrayList xStrings = new ArrayList();
        bool ticBetweenLabels = false;
        Color color;
        int total = 0;
        int totalPie = 0;
        string graphName = "";
        //array list for legend
        ArrayList legendList = new ArrayList();
        string xString = "";

        //set list of values for pie chart, order is very important
        decimal[] ValuseForPieChart;
        Color[] colors;
        string[] toolTips;

        private PageSettings pgSettings = new PageSettings();
        PrintDocument printDocument1 = new PrintDocument();
        private Bitmap memoryImage;

        private SaveFileDialog _saveFileDialog = new SaveFileDialog();

        private DebugLog debug;
        private ResourceManager rm;
        private CultureInfo culture;

        private const string dayMonth = "dd.MM.";

        private const int period = 7;

        ArrayList legendValues;
        List<LocationTO> locArray;
        private Log log;
        private const int gap = 5;
        private const string dayMoth = "dd.MM.";

        public CustomVisitGraphAdv()
        {
            InitializeComponent();

            legendValues = new ArrayList();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ApplUsersAdd).Assembly);
            log = new Log();

            setLanguage();
            this.CenterToScreen();
            this.cbGraphType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbLocation.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("menuAdvancedReport", culture);

                // group box's
                gbTimeInterval.Text = rm.GetString("gbTimeInterval", culture);
                gbDayInterval.Text = rm.GetString("gbTimeInterval", culture);
                gbDayOfWeek.Text = rm.GetString("gbTimeInterval", culture);

                // button's text
                btnShow.Text = rm.GetString("btnShow", culture);

                //label's text 
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblLocation.Text = rm.GetString("lblLocation", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblGraphType.Text = rm.GetString("lblGraphType", culture);
                lblFromDate.Text = rm.GetString("lblFrom", culture);
                lblHourFrom.Text = rm.GetString("lblFrom", culture);
                lblDateTo.Text = rm.GetString("lblTo", culture);
                lblHourTo.Text = rm.GetString("lblTo", culture);
                lblDateTo.Text = rm.GetString("lblTo", culture);
                lblDateToDayOfWeek.Text = rm.GetString("lblTo", culture);
                lblDateFromDayOfWeek.Text = rm.GetString("lblFrom", culture);
                lblDayOfWeek.Text = rm.GetString("lblDay", culture);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void CustomVisitGraphAdv_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            
                populateTypeCombo();
                populateLocationCombo();
                dtpDayOfWeek.Value = DateTime.Now;
                dtpDateFromDayOfWeek.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                dtpDateToDayOfWeek.Value = DateTime.Now;
                dtpHoursFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 8, 0, 0);
                dtpHoursTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 20, 0, 0);
                dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                dtpToDate.Value = DateTime.Now;
                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                dtpTo.Value = DateTime.Now;

                // set client logo
                if (File.Exists(Constants.LogoPath))
                {
                    pbClientLogo.Visible = true;
                    pbClientLogo.Image = new Bitmap(Constants.LogoPath);
                    int maxWeight = this.Width - this.btnShow.Location.X - btnShow.Width - gap * 2;
                    int maxHeight = this.Height - (graphPanel.Location.Y + graphPanel.Height + gap * 2);
                    if (pbClientLogo.Image.Height < maxHeight)
                        pbClientLogo.Height = pbClientLogo.Image.Height;
                    else pbClientLogo.Height = maxHeight;
                    if (pbClientLogo.Image.Width < maxWeight)
                        pbClientLogo.Width = pbClientLogo.Image.Width;
                    else pbClientLogo.Width = maxWeight;
                    int x = graphPanel.Location.X + graphPanel.Width - pbClientLogo.Width;
                    int y = btnShow.Location.Y + btnShow.Height - pbClientLogo.Height;
                    pbClientLogo.Location = new Point(x, y);
                }
                else
                {
                    pbClientLogo.Visible = false;
                }
                //arrayList of legendValues
                legendValues.Add(rm.GetString("Mon", culture));
                legendValues.Add(rm.GetString("Tue", culture));
                legendValues.Add(rm.GetString("Wed", culture));
                legendValues.Add(rm.GetString("Thu", culture));
                legendValues.Add(rm.GetString("Fri", culture));
                legendValues.Add(rm.GetString("Sat", culture));
                legendValues.Add(rm.GetString("Sun", culture));

                //dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 1, 1, 1);

                btnShow_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.CustomVisitGraphAdv_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateLocationCombo()
        {
            try
            {
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                locArray = loc.Search();
                locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), 0, 0, ""));

                cbLocation.DataSource = locArray;
                cbLocation.DisplayMember = "Name";
                cbLocation.ValueMember = "LocationID";
                cbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.populateLocationCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateTypeCombo()
        {
            try
            {
                cbGraphType.Items.Add("Nedeljni izvestaj");
                cbGraphType.Items.Add("Interval u danu");
                cbGraphType.Items.Add("Za dan u nedelji");

                cbGraphType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.populateTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                 //check if everyting is selected valid
                bool valid = Validation();
                if (valid)
                {
                    //get all correct logs from DB for selected interval
                    List<LogTO> customerLogs = new List<LogTO>();
                    //make list of strings for xAxis of graph
                    xStrings = new ArrayList();
                    List<ReaderTO> readers = new List<ReaderTO>();
                    color = Color.Violet;
                    xString = "";

                    int selectedLocation = (int)cbLocation.SelectedValue;

                    if (selectedLocation != -1)
                    {
                        readers = new Reader().getReaders((int)cbLocation.SelectedValue, -1);
                        if (readers.Count == 0)
                        {
                            MessageBox.Show(rm.GetString("noReadersForLocation", culture));
                            return;
                        }
                    }
                    else
                    {
                        readers = new Reader().getReaders(-1, -1);
                    }

                    string readerIDs = getStringFromArray(readers);

                    graphName = "";

                    if (cbGraphType.SelectedIndex == indexWeekly)
                    {
                        btnSwitch.Visible = false;
                        DateTime day = dtpFrom.Value.Date;
                        while (!day.DayOfWeek.ToString().Substring(0, 3).Equals(Constants.Monday))
                        {
                            day = day.AddDays(-1);
                        }
                        dtpFrom.Value = day;
                        DateTime dayTo = dtpTo.Value.Date;
                        while (!dayTo.DayOfWeek.ToString().Substring(0, 3).Equals(Constants.Sunday))
                        {
                            dayTo = dayTo.AddDays(1);
                        }
                        dtpTo.Value = dayTo;
                        customerLogs = log.searchLogsForGraph(dtpFrom.Value.Date, dtpTo.Value.Date, new DateTime(), new DateTime(), readerIDs);
                        xStrings = getXStringsForInterval();
                        graphName += dtpFrom.Value.ToString("dd.MM.yyyy") + " - " + dtpTo.Value.ToString("dd.MM.yyyy");
                        color = Color.BlueViolet;
                        int count = 0;
                        Dictionary<string, List<int>> graphValues = new Dictionary<string, List<int>>();
                        foreach (ReaderTO reader in readers)
                        {
                            List<LogTO> logsForReader = new List<LogTO>();
                            foreach (LogTO l in customerLogs)
                            {
                                if (l.ReaderID == reader.ReaderID)
                                    logsForReader.Add(l);
                            }
                            //from log list make list of passes
                            List<LogTO> passesAntenna1 = new List<LogTO>();
                            List<LogTO> passesAntenna0 = Common.Misc.getPassesForLogs(logsForReader, ref passesAntenna1, new DateTime(), new DateTime());

                            if (selectedLocation != -1)
                            {
                                if (reader.A0LocID != selectedLocation)
                                    passesAntenna0 = new List<LogTO>();

                                if (reader.A1LocID != selectedLocation)
                                    passesAntenna1 = new List<LogTO>();
                            }
                            //get hashtable of values for graph created from passes list's
                            Dictionary<string, List<int>> readerValues = getGraphValues(passesAntenna0, passesAntenna1, xStrings);

                            if (graphValues.Count == 0)
                            {
                                graphValues = readerValues;
                            }
                            else
                            {                               
                                foreach (string s in legendValues)
                                {
                                    for (int i = 0; i < xStrings.Count; i++)
                                    {
                                        graphValues[s][i] = graphValues[s][i] + readerValues[s][i];
                                        
                                    }
                                }
                            }
                        }
                        count = 0;
                        foreach (string s in legendValues)
                        {
                            for (int i = 0; i < xStrings.Count; i++)
                            {
                                count += graphValues[s][i];
                            }
                        }
                        graphName += "\n" + rm.GetString("totalVisitNum", culture) + " " + count.ToString();

                        //add bar chart control to graphPanel
                        BarChartControl barChartControl = new BarChartControl(xStrings, legendValues, graphValues, graphName, xString, "[#]", color, true);
                        barChartControl.Location = new Point(0, 0);
                        barChartControl.CountEverage = true;
                        barChartControl.EverageText = rm.GetString("everage", culture);
                        barChartControl.Height = graphPanel.Height;
                        barChartControl.Width = graphPanel.Width;
                        barChartControl.ShowLegend = true;
                        graphPanel.Controls.Clear();
                        graphPanel.Controls.Add(barChartControl);
                    }
                    else
                    {
                        btnSwitch.Visible = true;
                        DateTime fromHrs = new DateTime();
                        DateTime toHrs = new DateTime();
                        switch (cbGraphType.SelectedIndex)
                        {
                            case indexDayInterval:
                                xStrings = getXStringsForDayInterval();
                                customerLogs = log.searchLogsForGraph(dtpFromDate.Value.Date, dtpToDate.Value.Date, new DateTime(), new DateTime(), readerIDs);
                                graphName += dtpFromDate.Value.ToString("dd.MM.yyyy") + " - " + dtpToDate.Value.ToString("dd.MM.yyyy");
                                color = Color.LimeGreen;
                                fromHrs = dtpHoursFrom.Value;
                                toHrs = dtpHoursTo.Value;
                                break;
                            case indexDayOfWeek:
                                xStrings = getXStringsForDayOfWeek();
                                customerLogs = log.searchLogsForGraph(dtpDateFromDayOfWeek.Value.Date, dtpDateToDayOfWeek.Value.Date, new DateTime(), new DateTime(), readerIDs);
                                graphName += dtpDateFromDayOfWeek.Value.ToString("dd.MM.yyyy") + " - " + dtpDateToDayOfWeek.Value.ToString("dd.MM.yyyy") + ", (" + tbDayOfWeek.Text + ")";
                                color = Color.OrangeRed;
                                break;
                        }
                        valueTable = new Dictionary<string,List<int>>();
                        List<int> graphValuesList = new List<int>();
                        foreach (ReaderTO reader in readers)
                        {
                            List<LogTO> logsForReader = new List<LogTO>();
                            foreach (LogTO l in customerLogs)
                            {
                                if (l.ReaderID == reader.ReaderID)
                                    logsForReader.Add(l);
                            }
                            //from log list make list of passes
                            List<LogTO> passesAntenna1 = new List<LogTO>();
                            List<LogTO> passesAntenna0 = Common.Misc.getPassesForLogs(logsForReader, ref passesAntenna1, fromHrs, toHrs);

                            if (selectedLocation != -1)
                            {
                                if (reader.A0LocID != selectedLocation)
                                    passesAntenna0 = new List<LogTO>();

                                if (reader.A1LocID != selectedLocation)
                                    passesAntenna1 = new List<LogTO>();
                            }
                            //get hashtable of values for graph created from passes list's
                            List<int> readerValues = getGraphValuesRegular(passesAntenna0, passesAntenna1, xStrings);
                            int demo = 1;
                            if (ConfigurationManager.AppSettings[Constants.GraphDEMO] != null)
                            {
                                demo = int.Parse(ConfigurationManager.AppSettings[Constants.GraphDEMO]);
                            }
                            if (graphValuesList.Count <= 0)
                            {
                                foreach (int i in readerValues)
                                {
                                    graphValuesList.Add(i * demo);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < readerValues.Count; i++)
                                {
                                    int num = graphValuesList[i];
                                    graphValuesList[i] = num + readerValues[i] * demo;
                                }
                            }
                        }

                        //array list for legend
                        legendList = new ArrayList();
                        legendList.Add("");
                        total = 0;
                        foreach (int num in graphValuesList)
                        {
                            total += num;
                        }
                        ValuseForPieChart = new decimal[graphValuesList.Count];
                        toolTips = new string[graphValuesList.Count];
                        totalPie = 0;
                        for (int i = 0; i < graphValuesList.Count; i++)
                        {
                            ValuseForPieChart[i] = (decimal)((int)graphValuesList[i]);
                            toolTips[i] = (string)xStrings[i];
                            if (total > 0)
                                toolTips[i] += "\n " + ((int)graphValuesList[i]).ToString() + " (" + Math.Round((double)(int)graphValuesList[i] * (double)100 / (double)total) + "%)";
                        }
                        colors = getColorsForPieChart();
                        valueTable.Add("", graphValuesList);

                        graphName += "\n" + rm.GetString("totalVisitNum", culture) + " " + total.ToString();

                        showBarChart();
                        //BarChartControl barChartControl1 = new BarChartControl(xStrings, legendList, valueTable, graphName, xString, "[#]", color, false);
                        //barChartControl1.Location = new Point(0, 0);
                        //barChartControl1.Height = graphPanel.Height;
                        //barChartControl1.Width = graphPanel.Width;
                        //barChartControl1.ShowLegend = false;
                        //graphPanel.Controls.Clear();
                        //graphPanel.Controls.Add(barChartControl1);
                    }                    
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private Color[] getColorsForPieChart()
        {
            Random rand = new Random();
            Color[] colors = new Color[xStrings.Count];
            if (totalPie != 0)
                colors = new Color[xStrings.Count + 1];
            try
            {
                int i = 0;
                foreach (string s in xStrings)
                {
                    Color color = Color.FromArgb(80, rand.Next(256),
                                               rand.Next(256),
                                               rand.Next(256));
                    colors[i] = color;
                    i++;
                }
                if (totalPie != 0)
                    colors[xStrings.Count] = Color.FromArgb(80, rand.Next(256),
                                               rand.Next(256),
                                               rand.Next(256));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraphAdv.getColorsForPieChart(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Arrow;
            }
            return colors;
        }

        private ArrayList getXStringsForDayOfWeek()
        {
            ArrayList strings = new ArrayList();
            try
            {
                DateTime date = dtpDateFromDayOfWeek.Value.Date;
                while (!date.DayOfWeek.Equals(dtpDayOfWeek.Value.DayOfWeek))
                {
                    date = date.AddDays(1);
                }
                dtpDateFromDayOfWeek.Value = date;
                while (date < dtpDateToDayOfWeek.Value.Date.AddDays(1))
                {
                    string element = date.ToString(dayMoth);
                    strings.Add(element);
                    date = date.AddDays(7);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraphAdv.getXStringsForDayInterval(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return strings;
        }

        private List<int> getGraphValuesRegular(List<LogTO> passesAntennaOne, List<LogTO> passesAntennaTwo, ArrayList keys)
        {
            //key is string showen on xAxis
            List<int> graphValues = new List<int>();
            try
            {
                //get hashtable with x Axis string as key and array of passes as value
                Dictionary<string, List<LogTO>> elementsPassesGateOne = getElementsPasses(passesAntennaOne);
                Dictionary<string, List<LogTO>> elementsPassesGateTwo = getElementsPasses(passesAntennaTwo);

                int count = 1;
                foreach (string key in keys)
                {
                    if (!elementsPassesGateOne.ContainsKey(key) && !elementsPassesGateTwo.ContainsKey(key))
                    {
                        graphValues.Add(0);
                    }
                    else
                    {
                        if (elementsPassesGateOne.ContainsKey(key))
                        {
                            List<LogTO> passes = elementsPassesGateOne[key];
                            int visits = Common.Misc.getNumOfVisits(passes, ref count, ConfigurationManager.AppSettings[Constants.GraphDevide]);

                            graphValues.Add(visits);
                        }
                        if (elementsPassesGateTwo.ContainsKey(key))
                        {
                            List<LogTO> passes = elementsPassesGateTwo[key];
                            int visits = Common.Misc.getNumOfVisits(passes, ref count, ConfigurationManager.AppSettings[Constants.GraphDevide]);
                            if (!elementsPassesGateOne.ContainsKey(key))
                                graphValues.Add(visits);
                            else
                            {
                                int br = (int)graphValues[graphValues.Count - 1];
                                br += visits;
                                graphValues[graphValues.Count - 1] = br;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraphAdv.getGraphValues(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return graphValues;
        }

        private Dictionary<string, List<LogTO>> getElementsPasses(List<LogTO> passesGate)
        {
            //from list of passes make hashtable where kay is element string 
            Dictionary<string, List<LogTO>> finalPasses = new Dictionary<string,List<LogTO>>();
            try
            {
                Dictionary<string, List<LogTO>> elementsPasses = new Dictionary<string, List<LogTO>>();
                switch (cbGraphType.SelectedIndex)
                {
                    //if selected graph is interval graph add passes in to hashtable
                    case indexDayInterval:

                        foreach (LogTO pass in passesGate)
                        {
                            
                            if (elementsPasses.ContainsKey(pass.EventTime.Date.ToString(dayMoth)))
                            {
                                elementsPasses[pass.EventTime.Date.ToString(dayMoth)].Add(pass);
                            }
                            else
                            {
                                List<LogTO> passes = new List<LogTO>();
                                passes.Add(pass);
                                elementsPasses.Add(pass.EventTime.Date.ToString(dayMoth), passes);
                            }
                        }
                        finalPasses = elementsPasses;
                        break;
                    case indexDayOfWeek:
                        foreach (LogTO pass in passesGate)
                        {
                            if (pass.EventTime.DayOfWeek.Equals(dtpDayOfWeek.Value.DayOfWeek))
                            {
                                if (elementsPasses.ContainsKey(pass.EventTime.Date.ToString(dayMoth)))
                                {
                                    elementsPasses[pass.EventTime.Date.ToString(dayMoth)].Add(pass);
                                }
                                else
                                {
                                    List<LogTO> passes = new List<LogTO>();
                                    passes.Add(pass);
                                    elementsPasses.Add(pass.EventTime.Date.ToString(dayMoth), passes);
                                }
                            }
                        }
                        finalPasses = elementsPasses;
                        break;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.getGraphValues(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return finalPasses;
        }

        private ArrayList getXStringsForDayInterval()
        {
            ArrayList strings = new ArrayList();
            try
            {
                DateTime date = dtpFromDate.Value.Date;
                while (date < dtpToDate.Value.Date.AddDays(1))
                {
                    string element = date.ToString(dayMoth);
                    strings.Add(element);
                    date = date.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraphAdv.getXStringsForDayInterval(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return strings;
        }

        private ArrayList getXStringsForInterval()
        {
            ArrayList strings = new ArrayList();
            try
            {
                DateTime day = dtpFrom.Value.Date;
                while (!day.DayOfWeek.ToString().Substring(0,3).Equals(Constants.Monday))
                {
                    day = day.AddDays(-1);
                }
                if (day.DayOfWeek.ToString().Substring(0, 3).Equals(Constants.Monday))
                {
                    while (day.Date <= dtpTo.Value.Date)
                    {
                        string element = "";
                        element += (day.ToString(dayMonth) + "-" + day.AddDays(period-1).ToString(dayMonth));
                        strings.Add(element);
                        day = day.AddDays(period);
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.getXStringsForInterval(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return strings;
        }

        //private ArrayList getPassesForLogs(ArrayList customerLogs, ref ArrayList passesAntenna1)
        //{
        //    ArrayList passesAntenna0 = new ArrayList();
        //    try
        //    {
        //        for (int i = 0; i < customerLogs.Count - 1; i++)
        //        {
        //            LogTO currentLog = (LogTO)customerLogs[i];
        //            if ((currentLog.EventTime >= new DateTime(currentLog.EventTime.Year, currentLog.EventTime.Month, currentLog.EventTime.Day, dtpHoursFrom.Value.Hour, 0, 0) &&
        //                currentLog.EventTime <= new DateTime(currentLog.EventTime.Year, currentLog.EventTime.Month, currentLog.EventTime.Day, dtpHoursTo.Value.Hour, 0, 0)) ||
        //                cbGraphType.SelectedIndex != indexDayInterval)
        //            {                        
        //                switch (currentLog.ActionCommited)
        //                {
        //                    case Constants.passAntenna0:
        //                        passesAntenna0.Add(currentLog);
        //                        break;
        //                    case Constants.passAntenna1:
        //                        passesAntenna1.Add(currentLog);
        //                        break;
        //                    default:
        //                        continue;

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.getPassesForLogs(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //    return passesAntenna0;
        //}

        private Dictionary<string, List<int>> getGraphValues(List<LogTO> passesAntennaOne, List<LogTO> passesAntennaTwo, ArrayList keys)
        {
            //key is string showen in Legend
            Dictionary<string, List<int>> graphValues = new Dictionary<string,List<int>>();
            try
            {
                graphValues = getTableWithLegendStrings();

                foreach (string legendValue in legendValues)
                {
                    List<int> graphElementValues = new List<int>();
                    //get hashtable with x Axis string as key and array of passes as value
                    Dictionary<string, List<LogTO>> elementsPassesGateOne = getElementsPasses(passesAntennaOne, legendValue);
                    Dictionary<string, List<LogTO>> elementsPassesGateTwo = getElementsPasses(passesAntennaTwo, legendValue);

                    int count = 1;
                    foreach (string key in keys)
                    {
                        if (!elementsPassesGateOne.ContainsKey(key) && !elementsPassesGateTwo.ContainsKey(key))
                        {
                            graphElementValues.Add(0);
                        }
                        else
                        {
                            if (elementsPassesGateOne.ContainsKey(key))
                            {
                                List<LogTO> passes = elementsPassesGateOne[key];
                                int visits = Common.Misc.getNumOfVisits(passes, ref count, ConfigurationManager.AppSettings[Constants.GraphDevide]);

                                graphElementValues.Add(visits);
                            }
                            if (elementsPassesGateTwo.ContainsKey(key))
                            {
                                List<LogTO> passes = elementsPassesGateTwo[key];
                                int visits = Common.Misc.getNumOfVisits(passes, ref count, ConfigurationManager.AppSettings[Constants.GraphDevide]);
                                if (!elementsPassesGateOne.ContainsKey(key))
                                    graphElementValues.Add(visits);
                                else
                                {
                                    int br = (int)graphElementValues[graphElementValues.Count - 1];
                                    br += visits;
                                    graphElementValues[graphElementValues.Count - 1] = br;
                                }
                            }
                        }
                    }

                    graphValues[legendValue] = graphElementValues;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.getGraphValues(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return graphValues;
        }

        //private int getNumOfVisits(int passes, ref int counter)
        //{
        //    int visits = 0;
        //    try
        //    {

        //        if (ConfigurationManager.AppSettings[Constants.GraphDevide] != null && ConfigurationManager.AppSettings[Constants.GraphDevide].Equals(Constants.yes))
        //        {
        //            switch (passes % 2)
        //            {
        //                case indexWeekly:
        //                    visits = passes / 2;
        //                    break;

        //                case indexDayInterval:
        //                    visits = passes / 2;
        //                    if (counter % 2 == 1)
        //                    {
        //                        visits++;
        //                    }
        //                    counter++;
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            visits = passes;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.getNumOfVisits(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //    return visits;
        //}

        private Dictionary<string, List<int>> getTableWithLegendStrings()
        {
            //Make initial hashtable key is day and it will be in leged and value is array list of values 
            Dictionary<string, List<int>> initialTable = new Dictionary<string, List<int>>();
            try
            {
                //for now we are put blank list for values
                initialTable.Add(rm.GetString("Mon", culture), new List<int>());
                initialTable.Add(rm.GetString("Tue", culture), new List<int>());
                initialTable.Add(rm.GetString("Wed", culture), new List<int>());
                initialTable.Add(rm.GetString("Thu", culture), new List<int>());
                initialTable.Add(rm.GetString("Fri", culture), new List<int>());
                initialTable.Add(rm.GetString("Sat", culture), new List<int>());
                initialTable.Add(rm.GetString("Sun", culture), new List<int>());                

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.getTableWithLegendStrings(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return initialTable;
        }

        private Dictionary<string, List<LogTO>> getElementsPasses(List<LogTO> passesAntenna,string dayOfWeek)
        {
            //from list of passes make hashtable where kay is element string 
            Dictionary<string, List<LogTO>> elementsPasses = new Dictionary<string, List<LogTO>>();
            try
            {
                switch (cbGraphType.SelectedIndex)
                {
                    //if selected graph is interval graph add passes in to hashtable
                    case indexWeekly:
                        DateTime day = dtpFrom.Value.Date;

                        while (day.Date <= dtpTo.Value.Date && day.DayOfWeek.ToString().Substring(0, 3).Equals(Constants.Monday))
                        {
                            foreach (LogTO pass in passesAntenna)
                            {
                                if (rm.GetString(pass.EventTime.DayOfWeek.ToString().Substring(0, 3), culture).Equals(dayOfWeek))
                                {
                                    if (pass.EventTime.Date >= day && pass.EventTime.Date <= day.AddDays(period - 1))
                                    {
                                        if (elementsPasses.ContainsKey((day.ToString(dayMonth) + "-" + day.AddDays(period - 1).ToString(dayMonth))))
                                        {
                                            elementsPasses[(day.ToString(dayMonth) + "-" + day.AddDays(period - 1).ToString(dayMonth))].Add(pass);
                                            continue;
                                        }
                                        else
                                        {
                                            List<LogTO> passes = new List<LogTO>();
                                            passes.Add(pass);
                                            elementsPasses.Add((day.ToString(dayMonth) + "-" + day.AddDays(period - 1).ToString(dayMonth)), passes);
                                            continue;
                                        }
                                    }
                                }
                            }

                            day = day.AddDays(period);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.getElementsPasses(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return elementsPasses;
        }

        private string getStringFromArray(List<ReaderTO> readers)
        {
            string newString = "";
            try
            {
                foreach (ReaderTO reader in readers)
                {
                    newString += reader.ReaderID.ToString() + ", ";
                }
                if (newString.Length > 0)
                    newString = newString.Substring(0, newString.Length - 2);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.getStringFromArray(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return newString;
        }

        private bool Validation()
        {
            bool valid = true;
            try
            {
                switch (cbGraphType.SelectedIndex)
                { 
                    case indexWeekly:
                        if (dtpFrom.Value > dtpTo.Value)
                        {
                            MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                            valid = false;
                        }
                        if (dtpFrom.Value.AddMonths(1) < dtpTo.Value)
                        {
                            MessageBox.Show(rm.GetString("msgLongTimeInterval", culture));
                            valid = false;
                        }
                        break;
                    case indexDayInterval:
                        if (dtpFromDate.Value > dtpToDate.Value ||(dtpHoursFrom.Value.Hour >= dtpHoursTo.Value.Hour))
                        {
                            MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                            valid = false;
                        }
                        if (dtpFromDate.Value.AddMonths(1) < dtpToDate.Value)
                        {
                            MessageBox.Show(rm.GetString("msgLongTimeInterval", culture));
                            valid = false;
                        }
                        break;
                    case indexDayOfWeek:
                        if (dtpDateFromDayOfWeek.Value > dtpDateToDayOfWeek.Value )
                        {
                            MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                            valid = false;
                        }
                        if (dtpDateFromDayOfWeek.Value.AddMonths(3) < dtpDateToDayOfWeek.Value)
                        {
                            MessageBox.Show(rm.GetString("msgLongerThreeMonths", culture));
                            valid = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.Validation(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return valid;
        }

        private void btnLocationTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LocationsTreeView locationsTreeView = new LocationsTreeView(locArray);
                locationsTreeView.ShowDialog();
                if (!locationsTreeView.selectedLocation.Equals(""))
                {
                    this.cbLocation.SelectedIndex = cbLocation.FindStringExact(locationsTreeView.selectedLocation);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomAttendanceGraphAdv.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbGraphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gbTimeInterval.Visible = false;
                gbDayInterval.Visible = false;
                gbDayOfWeek.Visible = false;
                switch (cbGraphType.SelectedIndex)
                {
                    case indexWeekly:
                        gbTimeInterval.Visible = true;
                        break;
                    case indexDayInterval:
                        gbDayInterval.Visible = true;
                        break;       
                    case indexDayOfWeek:
                        gbDayOfWeek.Visible = true;
                        break;
                    default:
                        gbTimeInterval.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.cbGraphType_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           
            try
            {
                this.Cursor = Cursors.WaitCursor;

                tbDayOfWeek.Text = rm.GetString(dtpDayOfWeek.Value.DayOfWeek.ToString(), culture);
            }
            catch (Exception ex) {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.dateTimePicker1_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbDayOfWeek_TextChanged(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;

                tbDayOfWeek.Text = rm.GetString(dtpDayOfWeek.Value.DayOfWeek.ToString(), culture);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.tbDayOfWeek_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
           
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (graphType == barGraph)
                {
                    showPieChart();
                }
                else if (graphType == pieGraph)
                {
                    showBarChart();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.CustomersVisitGraph_FormClosed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void showBarChart()
        {
            try
            {
                graphType = barGraph;
                //add bar chart control to graphPanel
                BarChartControl barChartControl = new BarChartControl(xStrings, legendList, valueTable, graphName, xString, "[#]", color, ticBetweenLabels);
                barChartControl.Location = new Point(0, 0);
                barChartControl.Height = graphPanel.Height;
                barChartControl.Width = graphPanel.Width;
                barChartControl.ShowLegend = false;
                graphPanel.Controls.Clear();
                graphPanel.Controls.Add(barChartControl);
                btnSwitch.Text = "Pie";

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.showBarChar(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Arrow;
            }
        }
        private void pie_MouseClick(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.Button.Equals(MouseButtons.Right))
                {
                    CaptureScreen();
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
                if (e.Button.Equals(MouseButtons.Left))
                {
                    this.contextMenuStrip1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.pie_MouseClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        private void CaptureScreen()
        {
            try
            {

                Graphics mygraphics = this.graphPanel.CreateGraphics();
                Size s = this.graphPanel.Size;
                memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
                Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                IntPtr dc1 = mygraphics.GetHdc();
                IntPtr dc2 = memoryGraphics.GetHdc();
                BitBlt(dc2, 0, 0, this.graphPanel.Width, this.graphPanel.Height, dc1, 0, 0, 13369376);
                mygraphics.ReleaseHdc(dc1);
                memoryGraphics.ReleaseHdc(dc2);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph. CaptureScreen(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void showPieChart()
        {
            try
            {
                graphType = pieGraph;

                System.Drawing.PieChart.PieChartControl pccStatisticsView = new System.Drawing.PieChart.PieChartControl();
                setStatiticsViewProperties(ref pccStatisticsView);
                pccStatisticsView.ToolTips = this.toolTips;
                pccStatisticsView.Values = ValuseForPieChart;
                pccStatisticsView.Colors = colors;
                pccStatisticsView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pie_MouseClick);
                float[] displ = { (float)0.05, (float)0.05, (float)0.05, (float)0.05, (float)0.05 };
                pccStatisticsView.SliceRelativeDisplacements = displ;

                //set slice string's to default value
                string[] PieSliceTexts = new string[xStrings.Count];
                if (total > 0)
                {
                    for (int i = 0; i < xStrings.Count; i++)
                    {
                        //if slice is larger than 5% take value from toolTip and set it to slice string
                        if ((ValuseForPieChart[i] * 100 / total) > 3)
                        {
                            PieSliceTexts[i] = toolTips[i];
                        }
                    }
                }

                pccStatisticsView.Texts = PieSliceTexts;
                pccStatisticsView.SetBounds(5, 5, graphPanel.Width - 10, graphPanel.Height - 10);
                graphPanel.Controls.Clear();
                this.lblGraphName.Text = graphName;
                this.lblGraphName.SetBounds(5, 10, graphPanel.Width - 10, 45);
                graphPanel.Controls.Add(lblGraphName);
                graphPanel.Controls.Add(pccStatisticsView);
                btnSwitch.Text = "Bar";
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.showPieChart(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setStatiticsViewProperties(ref PieChartControl pccStatisticsView)
        {
            try
            {
                pccStatisticsView.BackColor = Color.White;
                pccStatisticsView.LeftMargin = MarginGap;
                pccStatisticsView.RightMargin = MarginGap;
                pccStatisticsView.TopMargin = MarginGap * 3;
                pccStatisticsView.BottomMargin = MarginGap;
                pccStatisticsView.FitChart = true;
                pccStatisticsView.SliceRelativeHeight = (float)0.20;
                pccStatisticsView.InitialAngle = 180;
                pccStatisticsView.EdgeLineWidth = (float)1.0;
                pccStatisticsView.EdgeColorType = System.Drawing.PieChart.EdgeColorType.DarkerThanSurface;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.setStatiticsViewProperties(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.contextMenuStrip1.Visible = false;
            try
            {
                //CaptureScreen();

                Thread ct = new Thread(new ThreadStart(this.ClipboardCopyThread));
                //ct.ApartmentState = ApartmentState.STA;
                ct.SetApartmentState(ApartmentState.STA);
                ct.Start();
                ct.Join();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.copyToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow; 
            }
        }

        private void ClipboardCopyThread()
        {
            Clipboard.SetDataObject(memoryImage, true);
        }

        private void saveImageAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.contextMenuStrip1.Visible = false;

                _saveFileDialog.Filter =
                    "Jpeg Format (*.jpg)|*.jpg|" +
                    "Emf Format (*.emf)|*.emf|" +
                    "PNG Format (*.png)|*.png|" +
                    "Gif Format (*.gif)|*.gif|" +
                    "Tiff Format (*.tif)|*.tif|" +
                    "Bmp Format (*.bmp)|*.bmp";

                if (_saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // CaptureScreen();
                    Stream myStream = _saveFileDialog.OpenFile();
                    if (myStream != null)
                    {

                        myStream.Close();

                        ImageFormat format = ImageFormat.Png;
                        if (_saveFileDialog.FilterIndex == 3)
                            format = ImageFormat.Gif;
                        else if (_saveFileDialog.FilterIndex == 4)
                            format = ImageFormat.Jpeg;
                        else if (_saveFileDialog.FilterIndex == 5)
                            format = ImageFormat.Tiff;
                        else if (_saveFileDialog.FilterIndex == 6)
                            format = ImageFormat.Bmp;

                        memoryImage.Save(_saveFileDialog.FileName, format);
                        myStream.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.saveImageAsToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void printSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.contextMenuStrip1.Visible = false;
            PrintDocument pd = new PrintDocument();

            // Add a try/catch pair since the users of the control can't catch this one
            try
            {
                if (pd != null)
                {
                    PageSetupDialog setupDlg = new PageSetupDialog();
                    setupDlg.Document = pd;

                    if (setupDlg.ShowDialog() == DialogResult.OK)
                    {
                        pd.PrinterSettings = setupDlg.PrinterSettings;
                        pgSettings = setupDlg.PageSettings;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.printSetupToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void printDocument1_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                e.Graphics.DrawImage(memoryImage, 0, 0);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.printDocument1_PrintPage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.contextMenuStrip1.Visible = false;
                
                this.printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

                //CaptureScreen();
                printDocument1.DefaultPageSettings = pgSettings;
                //printDocument1.DefaultPageSettings.Landscape = true;
                PrintDialog dlg = new PrintDialog();
                dlg.Document = printDocument1;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.printToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }      
        
    }
}