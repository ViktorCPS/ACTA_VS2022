using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using ZedGraph;

namespace UI
{
    public partial class BarChartControl : UserControl
    {
        //value is number for graph, key is string that 
        Dictionary<string, List<int>> _graphValues;

        //string's will be writen on x axis.
        ArrayList _xAxisStrings;  
     
        //string's for legend
        ArrayList _legendKeys;

        private bool _showLegend;
        private Color _firstColor;
        private bool ticBetweenLabels;
        private bool _countEverage;
        private string _everageText;

        public string EverageText
        {
            get { return _everageText; }
            set { _everageText = value; }
        }

        public bool CountEverage
        {
            get { return _countEverage; }
            set { _countEverage = value; }
        }

        public Color FirstColor
        {
            get { return _firstColor; }
            set { _firstColor = value; }
        }

        public bool ShowLegend
        {
            get { return _showLegend; }
            set { _showLegend = value; }
        }
            
        //titles properties
        private string _graphTitle;
        private string _XAxisTitle;
        private string _YAxisTitle;

        public ArrayList LegendKeys
        {
            get { return _legendKeys; }
            set { _legendKeys = value; }
        }

        public string YAxisTitle
        {
            get { return _YAxisTitle; }
            set { _YAxisTitle = value; }
        }

        public string XAxisTitle
        {
            get { return _XAxisTitle; }
            set { _XAxisTitle = value; }
        }

        public string GraphTitle
        {
            get { return _graphTitle; }
            set { _graphTitle = value; }
        }

        public Dictionary<string, List<int>> GraphValues
        {
            get { return _graphValues; }
            set { _graphValues = value; }
        }

        public ArrayList XAxisStrings
        {
            get { return _xAxisStrings; }
            set { _xAxisStrings = value; }
        }

        //control is drawing chart, to create one, pass array list of strings you want to write on x Axis
        //array list of legend strings, hashtable where legend string is key and value is y value on graph
        //pass strings for Graph name, x Axis name and y Axis name or pass blank for not showing them
        public BarChartControl(ArrayList xAxisStrings, ArrayList legendKeys, Dictionary<string, List<int>> graphValues, string graphTitle, string xAxisTitle, string yAxisTitle, Color color, bool majorTicBetweenLabels)
        {
            InitializeComponent();
            this.GraphValues = graphValues;
            this.GraphTitle = graphTitle;
            this.XAxisTitle = xAxisTitle;
            this.YAxisTitle = yAxisTitle;
            this.XAxisStrings = xAxisStrings;
            this.LegendKeys = legendKeys;
            this.FirstColor = color;
            this.ticBetweenLabels = majorTicBetweenLabels;
            CountEverage = false;
            EverageText = "";
        }

        public BarChartControl()
        {
            InitializeComponent();
            this.GraphValues = new Dictionary<string,List<int>>();            
        }

        private void SetSize()
        {
            zedGraphControl1.Location = new Point(0, 0);            
            zedGraphControl1.Size = new Size(this.Width, this.Height);
        }

        private void LineChartControl_Load(object sender, EventArgs e)
        {
            CreateChart(zedGraphControl1);
            SetSize();
        }

        private void LineChartControl_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        // Call this method from the Form_Load method, passing your ZedGraphControl
        public void CreateChart(ZedGraphControl zgc)
        {
            try
            {
                GraphPane myPane = zgc.GraphPane;

                //show legend is public property that define if legend is visible or not
                myPane.Legend.IsVisible = this.ShowLegend;

                // Set the titles and axis labels
                myPane.Title.Text = GraphTitle;
                myPane.YAxis.Title.Text = "";
                myPane.XAxis.Title.Text = "";

                //make array of labels and values
                //labels for x Axis are in XAxisStrings array 
                string[] labels = new string[XAxisStrings.Count];
                double[] xValues = new double[XAxisStrings.Count];
                double[] everigeValues = new double[XAxisStrings.Count];

                int i = 0;
                foreach (string label in XAxisStrings)
                {
                    labels[i] = label;
                    i++;                    
                    xValues[i - 1] = i;                    
                }

                int counter = 1;
                Color everageColor = Color.Purple;
                Color color = Color.Purple;
                if (CountEverage)
                {
                    foreach (string key in LegendKeys)
                    {
                        //yValues are in array list which is element of hashtable GraphValues
                        //make array from this values foreach key string in LegendKeys list
                        List<int> values = GraphValues[key];

                        i = 0;
                        foreach (int value in values)
                        {
                            everigeValues[i] = everigeValues[i] + value;
                            i++;                            
                        }
                    }
                    i = 0;
                    while (i < XAxisStrings.Count)
                    {
                        everigeValues[i] = everigeValues[i] / LegendKeys.Count;
                        i++;
                    }
                    LineItem line;
                    line = myPane.AddCurve("", xValues, everigeValues, everageColor, SymbolType.Circle);
                    line.Line.Width = 1.5F;
                    //// Make it a smooth line
                    //line.Line.IsSmooth = true;
                    line.Line.SmoothTension = 0.6F;

                    // Fill the symbols with white
                    line.Symbol.Fill = new Fill(everageColor);
                    line.Symbol.Size = 5;
                }
                int max = 0;
                foreach (string key in LegendKeys)
                {
                    //yValues are in array list which is element of hashtable GraphValues
                    //make array from this values foreach key string in LegendKeys list
                    List<int> values = GraphValues[key];
                    string name = key;

                    double[] yValues = new double[values.Count];

                    i = 0;
                    foreach (int value in values)
                    {
                        yValues[i] = value;                        
                        i++;
                        if (value > max)
                            max = value;
                    }

                    //get color for bar's
                    color = getColor(counter);
                        
                    //make instance of bar item
                    BarItem bar;
                    bar = myPane.AddBar(name, xValues, yValues, color);
                    // Fill the bar with a color gradient
                    bar.Bar.Fill = new Fill(Color.FromArgb(200, color), Color.White, Color.FromArgb(200, color));

                    myPane.XAxis.MajorTic.Size = 6f;
                    if (XAxisStrings.Count >= 1 && (XAxisStrings.Count * ((string)XAxisStrings[0]).Length) < 66)
                    {
                        myPane.XAxis.MajorGrid.IsVisible = this.ticBetweenLabels;
                        myPane.XAxis.MajorTic.IsBetweenLabels = this.ticBetweenLabels;
                    }
                    counter++;
                }
                
                if (CountEverage)
                {
                    LineItem line;
                    line = myPane.AddCurve(EverageText, xValues, everigeValues, everageColor, SymbolType.Circle);
                    ////// Fill the area under the curve with a white-green gradient
                    line.Line.Fill = new Fill(Color.FromArgb(50, everageColor), Color.White, Color.FromArgb(50, everageColor), 90F);

                    line.Line.Width = 1.5F;
                    //// Make it a smooth line
                    //line.Line.IsSmooth = true;
                    line.Line.SmoothTension = 0.6F;
                    
                    // Fill the symbols with white
                    line.Symbol.Fill = new Fill(everageColor);
                    line.Symbol.Size = 5;
                }
                // Fill the pane background with a gradient
                myPane.Fill = new Fill(Color.White, Color.LightBlue, 90F);
                // Fill the axis background 
                myPane.Chart.Fill = new Fill(Color.White);

                // Draw the X tics between the labels instead of at the labels
                myPane.XAxis.MajorTic.IsCrossInside = true;
                myPane.YAxis.MajorGrid.IsVisible = true;
                myPane.YAxis.MajorGrid.Color = Color.Black;
                myPane.YAxis.MajorGrid.PenWidth = 2;
                myPane.XAxis.MajorGrid.IsZeroLine = true;

                // Manually set the scale maximums according to user preference
                myPane.XAxis.Type = AxisType.Text;
                
                // Set the XAxis labels
                myPane.XAxis.Scale.TextLabels = labels;
                myPane.YAxis.Scale.Max = Math.Truncate(max * 1.15 + 1);
                if (myPane.YAxis.Scale.Max < 5)
                    myPane.YAxis.Scale.Max = 5;

                // Add a text item to decorate the graph
                TextObj text = new TextObj();
                text.Text = YAxisTitle;
                text.Location.X = 0.5;
                text.Location.Y = myPane.YAxis.Scale.Max;
                text.FontSpec.Fill = new Fill(Color.White, this.FirstColor, 45F);                
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.StringAlignment = StringAlignment.Near;
                myPane.GraphObjList.Add(text);

                if (!XAxisTitle.Equals(""))
                {
                    TextObj textx = new TextObj();
                    textx.Text = XAxisTitle;
                    textx.Location.X = XAxisStrings.Count + 0.5;
                    textx.Location.Y = 0;
                    textx.FontSpec.Fill = new Fill(Color.White,this.FirstColor, 45F);
                    textx.FontSpec.Border.IsVisible = false;
                    textx.FontSpec.IsBold = true;
                    textx.Location.AlignH = AlignH.Center;
                    textx.Location.AlignV = AlignV.Center;
                    textx.FontSpec.StringAlignment = StringAlignment.Near;
                    myPane.GraphObjList.Add(textx);
                }

                // Calculate the Axis Scale Ranges
                zgc.AxisChange();
                //Create TextObj's to provide labels for each bar                                
                BarItem.CreateBarLabels(myPane, false, "f0");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //geting color for bar, first 7 bars have hardcoded colors
        //after that it will be chosen random
        private Color getColor(int counter)
        {
            Color color = new Color();
            try
            {
                switch (counter)
                { 
                    case 1:
                        color = FirstColor;
                        break;
                    case 2:
                        color = Color.Green;
                        break;
                    case 3:
                        color = Color.RoyalBlue;
                        break;
                    case 4:
                        color = Color.DeepPink;
                        break;
                    case 5:
                        color = Color.DarkCyan;
                        break;
                    case 6:
                        color = Color.Brown;
                        break;
                    case 7:
                        color = Color.Red;
                        break;
                    default:
                        Random rand = new Random();
                        color = Color.FromArgb(rand.Next(255), rand.Next(DateTime.Now.Millisecond % 255), rand.Next(255));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return color;
        }
    }
}
