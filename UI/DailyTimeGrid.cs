using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    //DailyTimeGrid write numbers start from the MinValue to the MaxValue

    public partial class DailyTimeGrid : UserControl
    {
        private int _minValue;
        private int _maxValue;
        private int _stepValue;
        

        # region Constructors

        public DailyTimeGrid()
        {
            InitializeComponent();
            // set initial values
            MinValue = 0;
            MaxValue = 24;
            StepValue = 60;

        }

        public DailyTimeGrid(int minValue, int maxValue, int stepValue)
        {

            InitializeComponent();

            MinValue = minValue;
            MaxValue = maxValue;
            StepValue = stepValue;


        }

        # endregion

        private void DailyTimeGrid_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;

            Pen pen = new Pen(Color.LightBlue, 1);
            try
            {
                Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
                LinearGradientBrush brush = new LinearGradientBrush(rec, Color.Gray, Color.White, 90);
                brush.SetSigmaBellShape(0.2f, 1.0f);
                gr.FillRectangle(brush, rec);//background color

                float numOfLinesInHour = 60 / StepValue;

                float hourGap = (this.Width - 10) / (float)(MaxValue - MinValue);

                float lineGap = hourGap / numOfLinesInHour;
                string text = "";
                Font font = new Font("Arial", 9, FontStyle.Bold);
                float hourStep = 10;
                float lineStep = 0;
                if (MaxValue <= 24)
                {
                    //write numbers
                    for (int i = MinValue; i <= MaxValue; i++)
                    {
                        if (i > MinValue)//first gap is locked
                        {
                            hourStep = hourStep + hourGap;
                        }
                        //gr.DrawLine(pen, hourStep, 20, hourStep, this.Height - 5);
                        if (numOfLinesInHour >= 1)
                        {
                            for (int j = 1; j < numOfLinesInHour; j++)
                            {
                                lineStep = hourStep + lineGap * j;
                                //  gr.DrawLine(pen, lineStep, 20, lineStep, this.Height);
                            }
                        }

                        if (i < 10)
                        {
                            text = "0" + i.ToString();
                        }
                        else
                        {
                            text = i.ToString();
                        }
                        if (i < MaxValue)
                        {
                            gr.DrawString(text, font, Brushes.Black, hourStep - 7, this.Height / 20);
                        }
                    }
                }
                else
                {
                    int numOfDays = 1;
                    hourGap = (this.Width - 10) / (float)(MaxValue-MinValue);
                    while (numOfDays <= MaxValue / 24)
                    {
                       
                        //write numbers
                        for (int i = MinValue; i < 24; i+=2)
                        {
                            if ((i > MinValue)|| (numOfDays!=1))//first gap is locked
                            {
                                hourStep = hourStep + hourGap * 2;
                            }
                            //gr.DrawLine(pen, hourStep, 20, hourStep, this.Height - 5);
                            if (numOfLinesInHour >= 1)
                            {
                                for (int j = 1; j < numOfLinesInHour; j++)
                                {
                                    lineStep = hourStep + lineGap * j;
                                    //  gr.DrawLine(pen, lineStep, 20, lineStep, this.Height);
                                }
                            }

                            if (i < 10)
                            {
                                text = "0" + i.ToString();
                            }
                            else
                            {
                                text = i.ToString();
                            }
                            if (i < 24)
                            {
                                gr.DrawString(text, font, Brushes.Black, hourStep - 7, this.Height / 20);
                            }
                        }
                        numOfDays++;
                    }
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
        # region Properties

        // min value shown on DailyTimeGrid
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
        // max value shown on DailyTimeGrid
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
      
        # endregion
    }
}
