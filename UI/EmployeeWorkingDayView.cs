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

using TransferObjects;
using Common;
using Util;

namespace UI
{
    //EmployeeWorkingDayView display Name in one cell 
    //Count summ of time for all IOPairs in list and display it in second cell
    //This class draw and rectangles around those two cells
    //EmployeeWorkingDayView contains BarSegments object

    public partial class EmployeeWorkingDayView : UserControl
    {
        private List<IOPairTO> IOPairsList;
        private BarSegments barSegments;
        private int _minValue;
        private int _maxValue;
        private int _stepValue;
        private System.Windows.Forms.ToolTip toolTip1;
        private Color _backgroudColor;
        private bool _isLast;
        private string _displayString;
        public Brush displayStringColor;
        private string _tooltipString;
        private List<IOPairTO> _ioPairsListForNextDay;
        private List<WorkTimeIntervalTO> _intervalListForNextDay;
        public Dictionary<int, PassTypeTO> PassTypes;
        public List<WorkTimeIntervalTO> intervalList;
        protected ResourceManager rm;
        protected CultureInfo culture;
        protected ApplUserTO logInUser;
                
        public EmployeeWorkingDayView()
        {
            IOPairsList = new List<IOPairTO>();
            MinValue = 0;
            MaxValue = 24;
            StepValue = 60;
            BackgroundColor = Color.FromArgb(255, 255, 235);
            IsLast = false;
            DisplayString = "";
            ToolTipString = "";
            PassTypes = new Dictionary<int,PassTypeTO>();
            intervalList = new List<WorkTimeIntervalTO>();
            logInUser = NotificationController.GetLogInUser();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeePresenceGraphicReports).Assembly);
            displayStringColor = Brushes.Black;

            InitializeComponent();
        }

        public EmployeeWorkingDayView(int minValue, int maxValue, int stepValue, List<IOPairTO> ioPairsList, string displayString, string tooltipString, Dictionary<int, PassTypeTO> passTypes, List<WorkTimeIntervalTO> timeSchemaIntervalList)
        {
            IOPairsList = ioPairsList;
            MinValue = minValue;
            MaxValue = maxValue;
            StepValue = stepValue;
            BackgroundColor = Color.White;
            IsLast = false;
            DisplayString = displayString;
            ToolTipString = tooltipString;
            PassTypes = passTypes;
            intervalList = timeSchemaIntervalList;
            logInUser = NotificationController.GetLogInUser();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeePresenceGraphicReports).Assembly);
            displayStringColor = Brushes.Black;

            InitializeComponent();
        }

        private void RaiseEvent(object sender, EventArgs e)
        {
            this.barSegments.SetBounds(this.Width / 4, 0, this.Width / 4 * 3, this.Height);
        }

        private void EmployeeWorkingDayView_Paint(object sender, PaintEventArgs e)
        {
            this.barSegments.BackgroudColor = this.BackgroundColor;
            this.barSegments.IOPairsList = this.IOPairsList;
            this.barSegments.intervalList= this.intervalList;
            if (IOPairListForNextDay != null)
            {
                this.barSegments.SetBounds(this.Width / 4, 0,( this.Width / 4*3-10)/2+10, this.Height);
                this.barSegments.StepValue = 120;
                this.barSegments.PassTypes = this.PassTypes;
                BarSegments barSegments1 = new BarSegments(0,24,120,IOPairListForNextDay,PassTypes);
                barSegments1.SetBounds(this.Width / 4 + ((this.Width / 4 * 3 - 10) / 2 + 10), 0, (this.Width / 4 * 3 - 10) / 2, this.Height);
                barSegments1.BackgroudColor = this.BackgroundColor;
                barSegments1.intervalList = this.ItervalListForNextDay;
                barSegments1.GapToTheFirsLine = 0;
                this.Controls.Add(barSegments1);                
            }
            else
            {
                this.barSegments.SetBounds(this.Width / 4, 0, this.Width / 4 * 3, this.Height);                
            }
            Graphics gr = e.Graphics;
            RectangleF rect = new RectangleF(0, 0, this.Width, this.Height);

            LinearGradientBrush brush = new LinearGradientBrush(rect, BackgroundColor, Color.White, 90);
            brush.SetSigmaBellShape(0.2f, 1.0f);
            gr.FillRectangle(brush, rect);//Draw backgroud

            Pen pen = new Pen(Color.LightBlue, 1);
            try
            {
                int hours = 0;
                int min = 0;
                List<List<IOPairTO>> arrList = new List<List<IOPairTO>>();
                arrList.Add(IOPairsList);
                arrList.Add(IOPairListForNextDay);
                foreach (List<IOPairTO> list in arrList)
                {
                    if (list != null)
                    {
                        foreach (IOPairTO ioPair in list)// count summ time of all IOPairs in list
                        {
                            if (!ioPair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && !ioPair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                if (int.Parse(ioPair.StartTime.ToString("mm")) == 0)
                                {
                                    hours += int.Parse(ioPair.EndTime.ToString("HH")) - int.Parse(ioPair.StartTime.ToString("HH"));
                                }
                                else
                                {
                                    hours += int.Parse(ioPair.EndTime.ToString("HH")) - int.Parse(ioPair.StartTime.ToString("HH")) - 1;
                                    min += 60 - int.Parse(ioPair.StartTime.ToString("mm"));
                                }

                                min += int.Parse(ioPair.EndTime.ToString("mm"));
                            }

                            while (min >= 60)
                            {
                                hours++;
                                min -= 60;
                            }
                        }
                    }
                }
                this.toolTip1.SetToolTip(this, ToolTipString);
                string timeString = "";//text for summ time cell
                if (hours == 0) { timeString += "00h"; }
                else
                {
                    if (hours < 10) { timeString += "0"; }
                    timeString += hours + "h";
                }
                if (min == 0) { timeString += "00min"; }
                else
                {
                    if (min < 10) { timeString += "0"; }
                    timeString += min + "min";
                }
                
                Font font = new Font(FontFamily.GenericMonospace, 8);
                if (this.DisplayString.Length > this.Width / 41) { this.DisplayString = this.DisplayString.Substring(0, this.Width / 41); }// if name length is to large substring it

                gr.DrawString(this.DisplayString, font, displayStringColor, 2, this.Height / 12);//write Employee name
                gr.DrawString(timeString, font, Brushes.Black, this.Width / 6 + 1, this.Height / 12);//write summ time

                pen.Color = Color.Gray;
                gr.DrawRectangle(pen, 0, 0, this.Width / 4 - 1, this.Height);
                gr.DrawLine(pen, this.Width / 6, 0, this.Width / 6, this.Height);

                if (IsLast)//If it is last draw last line of table
                {
                    gr.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                gr.Dispose();
                pen.Dispose();
            }
        }

        # region Propeties

        public bool IsLast
        {
            get
            {
                return _isLast;
            }
            set
            {
                _isLast = value;
            }
        }
        public List<IOPairTO> IOPairListForNextDay
        {
            get 
            {
                return _ioPairsListForNextDay;
            }
            set
            {
                _ioPairsListForNextDay = value;
            }
        }
        public List<WorkTimeIntervalTO> ItervalListForNextDay
        {
            get
            {
                return _intervalListForNextDay;
            }
            set
            {
               _intervalListForNextDay = value;
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return _backgroudColor;
            }
            set
            {
                _backgroudColor = value;

            }
        }
        // min value shown 
        public int MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }
        // max value shown 
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }
        // gap between two lines in minutes
        public int StepValue
        {
            get
            {
                return _stepValue;
            }
            set
            {
                _stepValue = value;
                if (_stepValue < 15)
                {
                    _stepValue = 15;
                }
            }
        }
        public string DisplayString
        {
            get
            {
                return _displayString;
            }
            set
            {
                _displayString = value;
            }
        }
        public string ToolTipString
        {
            get
            {
                return _tooltipString;
            }
            set
            {
                _tooltipString = value;
            }
        }
        # endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.barSegments = new BarSegments(this.MinValue, this.MaxValue, this.StepValue, this.IOPairsList, this.PassTypes);
            this.Controls.Add(this.barSegments);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.EmployeeWorkingDayView_Paint);
            this.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
            this.ClientSizeChanged += new System.EventHandler(this.RaiseEvent);
            this.toolTip1 = new System.Windows.Forms.ToolTip(new System.ComponentModel.Container());
            this.barSegments.BackgroudColor = this.BackgroundColor;
            this.ResumeLayout(false);
        }
        #endregion
    }
}
