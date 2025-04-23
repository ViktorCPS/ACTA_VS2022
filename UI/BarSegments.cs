using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;

namespace UI
{
    //BarSegments class draw lines based on MinValue (firs line), 
    //MaxValue (last line) and StepValue (space between two lines).
    //BarSegments class displays bar segments which interpretes array list of IOPairs,
    //Location of bar is relate on StartTime and EndTime propery.
   
    public partial class BarSegments : UserControl
    {
        private int _minValue;
        private int _maxValue;
        private int _stepValue;
        private List<IOPairTO> _ioPairsList;
        private Color _backgroundColor;
        private int _gapToTheFirsLine;
        public Dictionary<int, PassTypeTO> PassTypes;
        public List<WorkTimeIntervalTO> intervalList;
        private DateTime dateOfSegments;
        ResourceManager rm;
        private CultureInfo culture;
        // Controller instance
        public NotificationController Controller;
        // Observer client instance
        public NotificationObserverClient observerClient;

        public BarSegments()
        {
            MinValue = 0;
            MaxValue = 24;
            StepValue = 60;
            IOPairsList = new List<IOPairTO>();
            BackgroudColor = Color.White;
            GapToTheFirsLine = 10;
            PassTypes = new Dictionary<int,PassTypeTO>();
            intervalList = new List<WorkTimeIntervalTO>();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeeAccessGroups).Assembly);
            
            InitializeComponent();
            InitializeDate();
            InitObserverClient();
        }

        public BarSegments(int minValue, int maxValue, int stepValue, List<IOPairTO> ioPairsList, Dictionary<int, PassTypeTO> passTypes)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            StepValue = stepValue;
            IOPairsList = ioPairsList;
            BackgroudColor = Color.White; 
            GapToTheFirsLine = 10;
            PassTypes = passTypes;
            intervalList = new List<WorkTimeIntervalTO>();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeeAccessGroups).Assembly);            
                        
            InitializeComponent();
            InitializeDate();
            InitObserverClient();
        }

        private void InitializeDate()
        {
            try 
            {
                if (IOPairsList.Count > 0)
                {
                    IOPairTO pair = IOPairsList[0];
                    if (!pair.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                    {
                        this.dateOfSegments = pair.StartTime.Date;
                    }
                    if (!pair.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                    {
                        this.dateOfSegments = pair.EndTime.Date;
                    }
                }
                else
                {
                    dateOfSegments = new DateTime();
                }
            }           
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void InitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
        }

        private void BarSegments_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            
            Pen penForGrids = new Pen(Color.FromArgb(30, 0, 51, 204), 1);

            RectangleF rectBarSegments = new RectangleF(0, 0, this.Width, this.Height);
            LinearGradientBrush BackgroudBrush = new LinearGradientBrush(rectBarSegments, BackgroudColor, Color.White, 90);
            BackgroudBrush.SetSigmaBellShape(0.2f, 1.0f);
            gr.FillRectangle(BackgroudBrush, rectBarSegments);

            try
            {
                int startHour = 0;
                int startMinute = 0;
                int endHour = 0;
                int endMinute = 0;

                float numOfLinesInHour = 60 / StepValue;
                float hourGap = (this.Width - GapToTheFirsLine) / (float)(MaxValue - MinValue);
                float minuteGap = hourGap / 60;
                //gap between two lines
                float lineGap = hourGap / numOfLinesInHour;
                float hourStep = GapToTheFirsLine;
                float lineStep = 0;
               
                //draw lines 
                for (int i = MinValue; i <= MaxValue; i++)
                {
                    if (i > MinValue)//first gap is locked and it is 10
                    {
                        hourStep = hourStep + hourGap;
                    }
                    gr.DrawLine(penForGrids, hourStep, 0, hourStep, this.Height);
                    if (numOfLinesInHour >= 1)
                    {
                        for (int j = 1; j < numOfLinesInHour; j++)
                        {
                            lineStep = hourStep + lineGap * j;
                            gr.DrawLine(penForGrids, lineStep, 0, lineStep, this.Height);
                        }
                    }
                   
                    if (StepValue >= 120)//If step is two hours
                    {
                        i++;
                        hourStep += hourGap;
                    }
                    if (GapToTheFirsLine == 0)
                    {
                        Pen pen1 = new Pen(Color.FromArgb(0, 51, 204));
                        gr.DrawLine(pen1, lineStep, 0, lineStep, this.Height);
                    }
                }

                SolidBrush brushTimeShemaIntervals = new SolidBrush(Color.FromArgb(60, Color.LightPink));
                Pen penTimeShemaIntervals = new Pen(Color. Pink);
                foreach (WorkTimeIntervalTO interval in intervalList)
                {
                    float x = 0;
                    float y = 0;
                    float rectWidth = 0;
                    float rectHeight = 0;
                    if (!interval.StartTime.Equals(null) && !interval.EndTime.Equals(null))
                    {
                        int startHourInterval = int.Parse(interval.StartTime.ToString("HH"));
                        int startMinuteInterval = int.Parse(interval.StartTime.ToString("mm"));
                        int endHourInterval = int.Parse(interval.EndTime.ToString("HH"));
                        int endMinuteInterval = int.Parse(interval.EndTime.ToString("mm"));

                        x = GapToTheFirsLine + hourGap * (startHourInterval - MinValue) + minuteGap * startMinuteInterval;
                        float rectEndi = GapToTheFirsLine + hourGap * (endHourInterval - MinValue) + minuteGap * endMinuteInterval;
                        rectWidth = rectEndi - x;
                        rectHeight = this.Height;
                        Rectangle recTimeSchema = new Rectangle(Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(rectWidth), Convert.ToInt32(rectHeight));
                        gr.FillRectangle(brushTimeShemaIntervals, recTimeSchema);
                        gr.DrawRectangle(penTimeShemaIntervals,recTimeSchema);
                    }
                }
                brushTimeShemaIntervals.Dispose();

                foreach (IOPairTO ioPair in IOPairsList)
                {
                    if (ioPair.StartTime.Date.Equals(dateOfSegments) || ioPair.EndTime.Date.Equals(dateOfSegments))
                    {
                        float x = 0;
                        float y = 0;
                        float rectWidth = 0;
                        float rectHeight = 0;
                        startHour = int.Parse(ioPair.StartTime.ToString("HH"));
                        startMinute = int.Parse(ioPair.StartTime.ToString("mm"));
                        endHour = int.Parse(ioPair.EndTime.ToString("HH"));
                        endMinute = int.Parse(ioPair.EndTime.ToString("mm"));

                        if (ioPair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))//If there is no StartTime 
                        {
                            x = GapToTheFirsLine + hourGap * (endHour - MinValue) + minuteGap * endMinute;
                            rectWidth = this.Height / 2;
                            x -= rectWidth;
                            rectHeight = this.Height;
                            y = 0;
                        }
                        else if (ioPair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))//If there is no EndTime 
                        {
                            x = GapToTheFirsLine + hourGap * (startHour - MinValue) + minuteGap * startMinute;
                            rectWidth = this.Height / 2;
                            rectHeight = this.Height;
                            y = 0;
                        }
                        else
                        {

                            x = GapToTheFirsLine + hourGap * (startHour - MinValue) + minuteGap * startMinute;
                            float rectEnd = GapToTheFirsLine + hourGap * (endHour - MinValue) + minuteGap * endMinute;
                            rectWidth = rectEnd - x;
                            y = this.Height / 5;
                            rectHeight = this.Height /5*3;
                        }
                        if (rectWidth > 0)
                        {
                            //Draw bar segment where start point is (x,y) 
                            BarSegment bar = new BarSegment(ioPair, PassTypes, this);
                            bar.SetBounds(Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(rectWidth), Convert.ToInt32(rectHeight));
                            bar.BackgroudColor = this.BackgroudColor;
                            bar.PassTypes = this.PassTypes;
                            this.Controls.Add(bar);

                        }
                    }
                    else
                    {
                        Controller.IOPairDateChanged(true);
                    }                                      
                }               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // Release resources
                // Dispose of objects
                gr.Dispose();
                BackgroudBrush.Dispose();
                penForGrids.Dispose();
            }
        }        

        # region Properties

        public int GapToTheFirsLine
        {
            get 
            {
                return _gapToTheFirsLine;
            }
            set
            {
                _gapToTheFirsLine = value;
            }
        }

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

        public Color BackgroudColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
            }
        }

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

        public List<IOPairTO> IOPairsList
        {
            get
            {
                return _ioPairsList;
            }
            set
            {
                _ioPairsList = value;
            }

        }
        # endregion
    }   
}
