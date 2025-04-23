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
    public partial class CustomersVisitGraph : Form
    {
        private DebugLog debug;
        private ResourceManager rm;
        private CultureInfo culture;

        private const string dayMoth = "dd.MM.";

        private LogTO log;

        List<LocationTO> locArray;

        //GRAPH TYPE INDEX'S
        private const int indexForDay = 0;
        private const int indexForInterval = 1;
        private const int indexForMonthsInterval = 2;

        //Graph sort index
        private const int barGraph = 0;
        private const int pieGraph = 1;

        const int MarginGap = 20;
        private const int gap = 5;
        int period = 0;

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
        string pieName = "";
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

        public CustomersVisitGraph()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ApplUsersAdd).Assembly);

            setLanguage();
            this.CenterToScreen();

            this.cbGraphType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbLocation.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CustomersAttendanceGraph_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.contextMenuStrip1.Visible = false;

                log = new LogTO();
                populateTypeCombo();
                populateLocationCombo();

                //set form values, take it from config
                int hourFrom = Constants.GraphfromHour;
                int hourTo = Constants.GraphtoHour;
                if (ConfigurationManager.AppSettings[Constants.GraphType] != null)
                    cbGraphType.SelectedIndex = int.Parse(ConfigurationManager.AppSettings[Constants.GraphType]);
                if (ConfigurationManager.AppSettings[Constants.GraphType] != null)
                    hourFrom = int.Parse(ConfigurationManager.AppSettings[Constants.GraphTimeFrom]);
                if (ConfigurationManager.AppSettings[Constants.GraphType] != null)
                    hourTo = int.Parse(ConfigurationManager.AppSettings[Constants.GraphTimeTo]);
                dtpTimeFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourFrom, 0, 0);
                dtpTimeTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourTo, 0, 0);
                if (ConfigurationManager.AppSettings[Constants.GraphType] != null)
                    periodNum.Value = int.Parse(ConfigurationManager.AppSettings[Constants.GraphPeriod]);
                if (ConfigurationManager.AppSettings[Constants.GraphDevide] == null)
                    Util.Misc.configAdd(Constants.GraphDevide, Constants.yes);
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

                groupBox1.Visible = false;
                btnGenerate.Visible = false;
                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 1, 1, 1);

                btnShow_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.CustomersAttendanceGraph_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateTypeCombo()
        {
            try
            {
                cbGraphType.Items.Add(rm.GetString("forDay", culture));
                cbGraphType.Items.Add(rm.GetString("forInterval", culture));
                cbGraphType.Items.Add(rm.GetString("forMonthInterval", culture));

                cbGraphType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.populateTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Set proper language
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("menuBasicReport", culture);

                // group box's
                gbTimeInterval.Text = rm.GetString("gbTimeInterval", culture);
                gbDayTimeInterval.Text = rm.GetString("gbTimeInterval", culture);
                gbMonthTimeInterval.Text = rm.GetString("gbTimeInterval", culture);

                // button's text
                btnShow.Text = rm.GetString("btnShow", culture);

                //label's text 
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblLocation.Text = rm.GetString("lblLocation", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblGraphType.Text = rm.GetString("lblGraphType", culture);
                lblTimeFrom.Text = rm.GetString("lblFrom", culture);
                lblTimeTo.Text = rm.GetString("lblTo", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblDate.Text = rm.GetString("lblDate", culture);
                lblMonthFrom.Text = rm.GetString("lblFrom", culture);
                lblMonthTo.Text = rm.GetString("lblTo", culture);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                totalPie = 0;
                pieName = "";
                period = (int)periodNum.Value;
                //check if everyting is selected valid
                bool valid = Validation();
                if (valid)
                {
                    //get all correct logs from DB for selected interval
                    List<LogTO> customerLogs = new List<LogTO>();
                    List<LogTO> customerLogs1 = new List<LogTO>();
                    valueTable = new Dictionary<string,List<int>>();
                    //make list of strings for xAxis of graph
                    xStrings = new ArrayList();
                    List<ReaderTO> readers = new List<ReaderTO>();
                    color = Color.Violet;
                    xString = "";
                    ticBetweenLabels = false;

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
                    switch (cbGraphType.SelectedIndex)
                    {
                        case indexForInterval:
                            xStrings = getXStringsForInterval();
                            customerLogs = new Log().searchLogsForGraph(dtpFrom.Value.Date, dtpTo.Value.Date, new DateTime(), new DateTime(), readerIDs);
                            graphName += dtpFrom.Value.ToString("dd.MM.yyyy") + " - " + dtpTo.Value.ToString("dd.MM.yyyy");
                            color = Color.Blue;
                            break;
                        case indexForDay:
                            xStrings = getXStringsForDay();
                            DateTime dateFrom = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day, dtpTimeFrom.Value.Hour, 1, 0);
                            DateTime dateTo = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day, dtpTimeTo.Value.Hour,1, 0);
                            dateTo = dateTo.AddHours((int)periodNum.Value);
                            if (dateTo.Date != dateFrom.Date)
                                dateTo = dateTo.Date;
                            customerLogs = new Log().searchLogsForGraph(new DateTime(), new DateTime(), dateFrom, dateTo.AddHours((int)periodNum.Value), readerIDs);
                            customerLogs1 = new Log().searchLogsForGraph(dateFrom.Date, dateFrom.Date, new DateTime(), new DateTime(), readerIDs);
                            graphName += dtpDate.Value.ToString("dd.MM.yyyy") + ", " + dtpTimeFrom.Value.ToString("HH") + " - " + ((string)xStrings[xStrings.Count-1]).Substring(3,2) + " h";
                            pieName += dtpDate.Value.ToString("dd.MM.yyyy");
                            color = Color.Violet;
                            xString = "[h]";
                            ticBetweenLabels = true;                            
                            break;
                        case indexForMonthsInterval:
                            xStrings = getXStringsForMoths();
                            DateTime fromDate = new DateTime(dtpMonthFrom.Value.Year, dtpMonthFrom.Value.Month, 1, 0, 0, 1);
                            DateTime toDate = new DateTime(dtpMonthTo.Value.AddMonths(1).Year, dtpMonthTo.Value.AddMonths(1).Month, 1, 0, 0, 1);
                            customerLogs = new Log().searchLogsForGraph(fromDate, toDate, new DateTime(), new DateTime(), readerIDs);
                            graphName += dtpMonthFrom.Value.ToString("MMM yyyy") + " - " + dtpMonthTo.Value.ToString("MMM yyyy");
                            color = Color.BlueViolet;
                            break;
                    }
                     
                    List<int> graphValues = new List<int>();
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
                        List<int> readerValues = getGraphValues(passesAntenna0, passesAntenna1, xStrings);
                        int demo = 1;
                        if (ConfigurationManager.AppSettings[Constants.GraphDEMO] != null)
                        {
                            demo = int.Parse(ConfigurationManager.AppSettings[Constants.GraphDEMO]);
                        }
                        if (graphValues.Count <= 0)
                        {

                            foreach (int i in readerValues)
                            {
                                graphValues.Add(i * demo);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < readerValues.Count; i++)
                            {
                                int num = (int)graphValues[i];
                                graphValues[i] = num + (readerValues[i] * demo);
                            }
                        }
                    }
                    if (customerLogs1.Count > 0)
                    { 
                       
                        foreach (ReaderTO reader in readers)
                        {
                            List<LogTO> logsForReader = new List<LogTO>();
                            foreach (LogTO l in customerLogs1)
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
                            totalPie += (passesAntenna0.Count + passesAntenna1.Count) / 2 + (passesAntenna0.Count + passesAntenna1.Count) % 2;
                        }
                    }
                    total = 0;
                    foreach (int num in graphValues)
                    {
                        total += num;
                    }
                    if (totalPie == 0||total>=totalPie)
                    {
                        ValuseForPieChart = new decimal[graphValues.Count];
                        toolTips = new string[graphValues.Count];
                        totalPie = 0;
                        for (int i = 0; i < graphValues.Count; i++)
                        {
                            ValuseForPieChart[i] = (decimal)((int)graphValues[i]);
                            toolTips[i] = (string)xStrings[i];
                            if (total > 0)
                                toolTips[i] += "\n " + ((int)graphValues[i]).ToString() + " (" + Math.Round((double)(int)graphValues[i] * 100 / (double)total,0) + "%)";
                        }
                    }
                    else
                    {
                        ValuseForPieChart = new decimal[graphValues.Count+1];
                        toolTips = new string[graphValues.Count+1];
                        for (int i = 0; i < graphValues.Count; i++)
                        {
                            ValuseForPieChart[i] = (decimal)((int)graphValues[i]);
                            toolTips[i] = (string)xStrings[i];
                            if (totalPie > 0)
                                toolTips[i] += "\n " + ((int)graphValues[i]).ToString() + " (" + Math.Round((double)(int)graphValues[i] * 100 / (double)totalPie,0) + "%)";
                        }
                    }

                    if (totalPie != 0)
                    {
                        ValuseForPieChart[graphValues.Count] = (decimal)(totalPie - total);
                        toolTips[graphValues.Count] = (totalPie - total + " (" + (totalPie - total) * 100 / totalPie) + "%)"; ;
                    }
                    
                    colors = getColorsForPieChart();
                    

                    //array list for legend
                    legendList = new ArrayList();
                    legendList.Add("");
                    
                    valueTable.Add("", graphValues);

                    graphName += "\n" + rm.GetString("totalVisitNum", culture) + " " + total.ToString();
                    if (totalPie <= 0)
                        pieName = graphName;
                    else
                        pieName += "\n" + rm.GetString("totalVisitNum", culture) + " " + totalPie.ToString();
                    showBarChart();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.btnShow_Click(): " + ex.Message + "\n");
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

                    if (totalPie != 0)
                    {
                        //set slice string's to default value
                         PieSliceTexts = new string[xStrings.Count+1];
                        
                            for (int i = 0; i < xStrings.Count; i++)
                            {
                                //if slice is larger than 5% take value from toolTip and set it to slice string
                                if ((ValuseForPieChart[i] * 100 / totalPie) > 3)
                                {
                                    PieSliceTexts[i] = toolTips[i];
                                }
                           }
                           PieSliceTexts[xStrings.Count] = toolTips[xStrings.Count];
                    }
                pccStatisticsView.Texts = PieSliceTexts;
                pccStatisticsView.SetBounds(5, 5, graphPanel.Width-10, graphPanel.Height-10);
                graphPanel.Controls.Clear();                
                this.lblGraphName.Text = pieName;
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

        private Color[] getColorsForPieChart()
        {
            Random rand = new Random();
            Color[] colors = new Color[xStrings.Count];
            if(totalPie!=0)
                colors = new Color[xStrings.Count+1];
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
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.getColorsForPieChart(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Arrow;
            }
            return colors;            
        }

        protected void setStatiticsViewProperties(ref System.Drawing.PieChart.PieChartControl pccStatisticsView)
        {
            try
            {
                pccStatisticsView.BackColor= Color.White;
                pccStatisticsView.LeftMargin = MarginGap;
                pccStatisticsView.RightMargin = MarginGap;
                pccStatisticsView.TopMargin = MarginGap*3;
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
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.getStringFromArray(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return newString;
        }

        private List<int> getGraphValues(List<LogTO> passesAntennaOne, List<LogTO> passesAntennaTwo, ArrayList keys)
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
                            int visits = Common.Misc.getNumOfVisits(passes, ref count,ConfigurationManager.AppSettings[Constants.GraphDevide]);

                            graphValues.Add(visits);
                        }
                        if (elementsPassesGateTwo.ContainsKey(key))
                        {
                            List<LogTO> passes = elementsPassesGateTwo[key];
                            int visits = Common.Misc.getNumOfVisits(passes, ref count,ConfigurationManager.AppSettings[Constants.GraphDevide]);
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
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.getGraphValues(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return graphValues;
        }

        private Dictionary<string, List<LogTO>> getElementsPasses(List<LogTO> passesGate)
        {
            //from list of passes make hashtable where kay is element string 
            Dictionary<string, List<LogTO>> finalPasses = new Dictionary<string, List<LogTO>>();
            try
            {
                Dictionary<string, List<LogTO>> elementsPasses = new Dictionary<string,List<LogTO>>();
                switch (cbGraphType.SelectedIndex)
                {
                    //if selected graph is interval graph add passes in to hashtable
                    case indexForInterval:

                        foreach (LogTO pass in passesGate)
                        {
                            if (ConfigurationManager.AppSettings[Constants.GraphDEMO] != null)
                            {
                                if (int.Parse(ConfigurationManager.AppSettings[Constants.GraphDEMO]) > 1)
                                {
                                    int interval = dtpMonthTo.Value.DayOfYear - dtpFrom.Value.DayOfYear;
                                    pass.EventTime = dtpFrom.Value.AddDays(pass.EventTime.Minute % interval);
                                }
                            }
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

                    // if selected graph type is day graph fill hashtable vith passes 
                    // and if selected period is more than 1 refill hashtable                         
                    case indexForDay:

                        foreach (LogTO pass in passesGate)
                        {
                            if (ConfigurationManager.AppSettings[Constants.GraphDEMO] != null)
                            {
                                if (int.Parse(ConfigurationManager.AppSettings[Constants.GraphDEMO]) > 1)
                                {
                                    pass.EventTime = new DateTime(pass.EventTime.Year, pass.EventTime.Month, pass.EventTime.Day, pass.EventTime.Minute % 12 + 9, pass.EventTime.Minute, pass.EventTime.Second);
                                }
                            }
                            
                            if (elementsPasses.ContainsKey(pass.EventTime.ToString("HH") + "-" + pass.EventTime.AddHours(period).ToString("HH")))
                            {
                                elementsPasses[(pass.EventTime.ToString("HH") + "-" + pass.EventTime.AddHours(period).ToString("HH"))].Add(pass);
                            }
                            else
                            {
                                List<LogTO> passes = new List<LogTO>();
                                passes.Add(pass);
                                elementsPasses.Add((pass.EventTime.ToString("HH") + "-" + pass.EventTime.AddHours(period).ToString("HH")), passes);
                            }
                        }
                        
                        if (period == 1)
                            finalPasses = elementsPasses;

                        else if (period > 1)
                        {
                            DateTime hour = dtpTimeFrom.Value;
                            while (hour <= dtpTimeTo.Value)
                            {
                                List<LogTO> passArray = new List<LogTO>();

                                for (DateTime h = hour; h < hour.AddHours(period); h = h.AddHours(1))
                                {
                                    if (elementsPasses.ContainsKey((h.ToString("HH") + "-" + h.AddHours(period).ToString("HH"))))
                                    {
                                        foreach (LogTO l in elementsPasses[h.ToString("HH") + "-" + h.AddHours(period).ToString("HH")])
                                        {
                                            passArray.Add(l);
                                        }
                                    }
                                }

                                finalPasses.Add((hour.ToString("HH") + "-" + hour.AddHours(period).ToString("HH")), passArray);
                                hour = hour.AddHours(period);
                            }
                        }

                        break;
                    case indexForMonthsInterval:
                        foreach (LogTO pass in passesGate)
                        {
                            if (elementsPasses.ContainsKey(pass.EventTime.Date.ToString("MMM yyyy")))
                            {
                                elementsPasses[pass.EventTime.Date.ToString("MMM yyyy")].Add(pass);
                            }
                            else
                            {
                                List<LogTO> passes = new List<LogTO>();
                                passes.Add(pass);
                                elementsPasses.Add(pass.EventTime.Date.ToString("MMM yyyy"), passes);
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

        private ArrayList getXStringsForInterval()
        {
            ArrayList strings = new ArrayList();
            try
            {
                DateTime date = dtpFrom.Value.Date;
                while (date < dtpTo.Value.Date.AddDays(1))
                {
                    string element = date.ToString(dayMoth);
                    strings.Add(element);
                    date = date.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return strings;
        }

        private ArrayList getXStringsForDay()
        {
            ArrayList strings = new ArrayList();
            try
            {
                DateTime hour = dtpTimeFrom.Value;
                while (hour <= dtpTimeTo.Value)
                {
                    string element = "";
                    element += hour.ToString("HH") + "-"+hour.AddHours(period).ToString("HH");
                    strings.Add(element);
                    hour = hour.AddHours((int)periodNum.Value);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return strings;
        }

        private ArrayList getXStringsForMoths()
        {
            ArrayList strings = new ArrayList();
            try
            {
                DateTime month = dtpMonthFrom.Value;
                while (month <= dtpMonthTo.Value)
                {
                    string element = "";
                    element += month.ToString("MMM yyyy");
                    strings.Add(element);
                    month = month.AddMonths(1);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return strings;
        }

        //private ArrayList getPassesForLogs(ArrayList customerLogs, ref ArrayList passesAntenna1)
        //{
        //    ArrayList passesAntenna0 = new ArrayList();
        //    try
        //    {
        //        for (int i = 0; i < customerLogs.Count ; i++)
        //        {
        //            LogTO currentLog = (LogTO)customerLogs[i];


        //            switch (currentLog.ActionCommited)
        //            {
        //                case Constants.passAntenna0:
        //                    passesAntenna0.Add(currentLog);
        //                    break;
        //                case Constants.passAntenna1:
        //                    passesAntenna1.Add(currentLog);
        //                    break;
        //                default:
        //                    continue;
        //            }


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.getPassesForLogs(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //    return passesAntenna0;
        //}

        private bool Validation()
        {
            bool valid = true;
            try
            {
                switch (cbGraphType.SelectedIndex)
                {
                    case indexForInterval:

                        if (dtpFrom.Value.Date > dtpTo.Value.Date)
                        {
                            MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                            valid = false;
                        }
                        if (dtpFrom.Value.Date.AddMonths(1) < dtpTo.Value.Date)
                        {
                            MessageBox.Show(rm.GetString("msgLongTimeInterval", culture));
                            valid = false;
                        }
                        break;
                    case indexForDay:
                        if (dtpTimeFrom.Value.Hour >= dtpTimeTo.Value.Hour)
                        {
                            MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                            valid = false;
                        }
                        break;
                    case indexForMonthsInterval:
                        if (dtpMonthFrom.Value.Date > dtpMonthTo.Value.Date)
                        {
                            MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                            valid = false;
                        }
                        if (dtpMonthFrom.Value.AddYears(1).Date < dtpMonthTo.Value.Date)
                        {
                            MessageBox.Show(rm.GetString("msgLongMonthInterval", culture));
                            valid = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.getPassesForLogs(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return valid;

        }

        private void cbGraphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gbTimeInterval.Visible = false;
                gbDayTimeInterval.Visible = false;
                gbMonthTimeInterval.Visible = false;
                switch (cbGraphType.SelectedIndex)
                {
                    case indexForDay:
                        gbDayTimeInterval.Visible = true;
                        break;
                    case indexForInterval:
                        gbTimeInterval.Visible = true;
                        break;
                    case indexForMonthsInterval:
                        gbMonthTimeInterval.Visible = true;
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
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dtpTimeFrom.Value = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day, dtpTimeFrom.Value.Hour, 0, 0);
                dtpTimeTo.Value = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day, dtpTimeTo.Value.Hour, 0, 0);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.dtpDate_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow; 
            }
        }
        /// <summary>
        /// Populate Location Combo Box
        /// </summary>
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
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.populateLocationCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
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
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                List<ReaderTO> readers = new List<ReaderTO>();
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
                int readerID = 1;
                if (readers.Count > 0)
                    readerID = readers[0].ReaderID;

                bool delete = new Log().deleteLogs(dtpDateFrom.Value.Date, dtpDateTo.Value.Date, new DateTime(), new DateTime(), readerID.ToString());

                DateTime date = dtpDateFrom.Value.Date;
                while (date <= dtpDateTo.Value.Date)
                {
                    int numOfInsertedPassesForDay = 0;
                    int hour = dtpHoursFrom.Value.Hour;
                    while (hour <= dtpHoursTo.Value.Hour)
                    {
                        Random rand = new Random();
                        int randNumber = rand.Next((int)numInHour.Value);
                        numOfInsertedPassesForDay = numOfInsertedPassesForDay + insertPass(date, hour, randNumber, readerID);
                        hour++;
                    }
                    debug.writeLog(DateTime.Now + " Number of passes inserted for " + date.ToString("dd.MM.yyyy") + " is: " + numOfInsertedPassesForDay.ToString() + "\n");
                    date = date.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private int insertPass(DateTime date, int hour, int randNumber, int readerID)
        {
            int numOfIsertedPasses = 0;
            try
            {

                if (randNumber > 0)
                {
                    for (int i = 0; i < randNumber; i++)
                    {
                        DateTime eventTime = new DateTime(date.Year, date.Month, date.Day, hour, i, 5);
                        //TransferObjects.LogTO logTO = new TransferObjects.LogTO(-1, readerID, 0, 255, 255, 3, eventTime, 0, "", "", "", "", "");
                        //ArrayList list = new Log().Search(logTO);
                        //if (list.Count <= 0)
                        //{
                            //int inserted = new Log().Save(logTO);
                            //if (inserted > 0)
                            //{
                                TransferObjects.LogTO logTOSecund = new TransferObjects.LogTO(0, readerID, 0, 255, 255, 0, eventTime.AddSeconds(DateTime.Now.Millisecond%2), 0, "", "", "", "", "",-1);
                                int insertedSecund = new Log().Save(logTOSecund);
                                if (insertedSecund > 0)
                                    numOfIsertedPasses++;
                            //}
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.insertPass(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return numOfIsertedPasses;
        }

        private void pbClientLogo_DoubleClick(object sender, EventArgs e)
        {
            groupBox1.Visible = !groupBox1.Visible;            
        }
                
        private void CustomersVisitGraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Util.Misc.configAdd(Constants.GraphType, cbGraphType.SelectedIndex.ToString());
                Util.Misc.configAdd(Constants.GraphTimeFrom, dtpTimeFrom.Value.Hour.ToString());
                Util.Misc.configAdd(Constants.GraphTimeTo, dtpTimeTo.Value.Hour.ToString());
                Util.Misc.configAdd(Constants.GraphPeriod, periodNum.Value.ToString());

                this.Close();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CustomersAttendanceGraph.CustomersVisitGraph_FormClosed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

        private void printDocument1_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
        
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
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
        }        

        private void pie_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

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
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void saveImageAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

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
            PrintDocument pd =new PrintDocument();
            
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
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void ClipboardCopyThread()
        {
            Clipboard.SetDataObject(memoryImage, true);
        }
    }
}