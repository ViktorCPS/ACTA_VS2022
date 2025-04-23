using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;
using TransferObjects;
using Common;
using Util;
using System.Drawing.Imaging;

namespace UI
{
    public partial class CostumerVisitReport : Form
    {
        //jpg document properties
        private const int FontSize = 12;
        private const int height = 1000;
        private const int width = 900;        
        private const string FontName = "Times New Roman";
        private const float rowHeight = 15f;
        private const float columnWidth = 150f;
        private const int columnElements = 62;

        private CultureInfo culture;
        private ResourceManager rm;
        private DebugLog debug;
        private ArrayList xStrings;
        private LogTO log;
        private const string dayMoth = "dd.MM.yyyy.";

        private int total = 0;

        public CostumerVisitReport()
        {
            InitializeComponent();
            this.CenterToScreen();

            xStrings = new ArrayList();
            log = new LogTO();
            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);
            setLanguage();
            this.tbName.Text = "PeopleCounter" + DateTime.Now.ToString("yyyy_MM_dd");
            this.dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.dtpTo.Value = DateTime.Now.AddDays(-1);
        }
        private void setLanguage()
        {
            try
            { 
                //form text
                this.Text = rm.GetString("basicCounter", culture);

                //button's text
                this.btnBrowse.Text = rm.GetString("btnBrowse", culture);
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnGenReport.Text = rm.GetString("btnGenerateReport", culture);

                //label's text
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblName.Text = rm.GetString("lblName1", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);

                //group box's text
                this.gbDestination.Text = rm.GetString("gbDestination", culture);
                this.gbTimeInterval.Text = rm.GetString("gbTimeInterval", culture);

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + "CostumerVisitReport.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("notUpdateTagsNow", culture));
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                FolderBrowserDialog fbDialog = new FolderBrowserDialog();

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.tbDestination.Text = fbDialog.SelectedPath.ToString();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + "CostumerVisitReport.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("notUpdateTagsNow", culture));
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool valid = Validation();
                if (valid)
                {
                    //get all correct logs from DB for selected interval
                    List<LogTO> customerLogs = new List<LogTO>();
                    xStrings = getXStringsForInterval();
                    customerLogs = new Log().searchLogsForGraph(dtpFrom.Value.Date, dtpTo.Value.Date, new DateTime(), new DateTime(), "");
                    //from log list make list of passes
                    List<LogTO> passesAntenna1 = new List<LogTO>();
                    List<LogTO> passesAntenna0 = Common.Misc.getPassesForLogs(customerLogs, ref passesAntenna1, new DateTime(),new DateTime());
                    //get hashtable of values for graph created from passes list's
                    List<int> graphValues = getGraphValues(passesAntenna0, passesAntenna1, xStrings);

                    total = 0;
                    foreach (int num in graphValues)
                    {
                        total += num;
                    }

                    if (this.tbDestination.Text.Equals(""))
                        MessageBox.Show(rm.GetString("selFolder", culture));
                    else if (this.tbName.Text.Equals(""))
                    {
                        MessageBox.Show(rm.GetString("enterName", culture));
                    }
                    else
                    {
                        //StreamWriter SW;

                        if (total > 0)
                        {
                            string fileName = tbDestination.Text.ToString() + "\\" + tbName.Text.ToString() + ".jpg";

                            float xValue = 15f;
                            float yValue = 15f;
                             Color FontColor = Color.Black;
                             Color BackColor = Color.White;
                             Bitmap objBitmap = new Bitmap(width, height);
                             Graphics objGraphics = Graphics.FromImage(objBitmap);
                             Font objFont = new Font(FontName, FontSize);
                             SolidBrush objBrushForeColor = new SolidBrush(FontColor);
                             SolidBrush objBrushBackColor = new SolidBrush(BackColor);
                             objGraphics.FillRectangle(objBrushBackColor, 0, 0, width, height);
                             PointF objPoint = new PointF(xValue, rowHeight );
                             objGraphics.DrawString(rm.GetString("lblDate", culture) + "         - " + rm.GetString("lblNumber", culture), objFont, objBrushForeColor, objPoint);
                            for (int i = 0; i < xStrings.Count; i++)
                            {
                                string Text = (string)xStrings[i] + " - " + ((int)graphValues[i]).ToString();
                                if (i%columnElements == 0&&i!=0)
                                {
                                    xValue = xValue + columnWidth;
                                    yValue = rowHeight;
                                    objPoint = new PointF(xValue, yValue); 
                                    objGraphics.DrawString(rm.GetString("lblDate", culture) + "         - " + rm.GetString("lblNumber", culture), objFont, objBrushForeColor, objPoint);                            
                                }
                                
                                yValue = rowHeight * (i%columnElements + 2);                                
                                objPoint = new PointF(xValue, yValue); 
                                objGraphics.DrawString(Text, objFont, objBrushForeColor, objPoint);
                                
                            }
                            objBitmap.Save(fileName, ImageFormat.Jpeg);                            
                           
                            //saving data to text file
                            //string fileName = tbDestination.Text.ToString() + "\\" + tbName.Text.ToString() + ".txt";
                            //if (File.Exists(fileName))
                            //{
                            //    File.Delete(fileName);
                            //}
                            //SW = File.CreateText(fileName);
                            //SW.WriteLine(rm.GetString("lblDate", culture) + "      - " + rm.GetString("lblNumber", culture));

                            //for (int i = 0; i < xStrings.Count; i++)
                            //{
                            //    SW.WriteLine((string)xStrings[i] + " - " + ((int)graphValues[i]).ToString());
                            //}
                            //SW.Close();

                            MessageBox.Show(rm.GetString("fileCreated", culture));
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noPassesFound", culture));
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + "CostumerVisitReport.btnGenReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("fileNotCreated", culture));
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            bool valid = true;
            try
            {
                if (dtpFrom.Value > dtpTo.Value)
                {
                    MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                    valid = false;
                }
                if (dtpFrom.Value.Date.AddYears(1) < dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("msgLongMonthInterval", culture));
                    valid = false;
                }
                if(!System.IO.Directory.Exists(tbDestination.Text.ToString()))
                {
                    MessageBox.Show(rm.GetString("noDirectory",culture));
                    valid = false;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + "CostumerVisitReport.Validation(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("fileNotCreated", culture));
            }
            return valid;
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
                debug.writeLog(DateTime.Now + " CostumerVisitReport.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return strings;
        }

        //private ArrayList getPassesForLogs(ArrayList customerLogs, ref ArrayList passesAntenna1)
        //{
        //    ArrayList passesAntenna0 = new ArrayList();
        //    try
        //    {
        //        for (int i = 0; i < customerLogs.Count; i++)
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
        //        debug.writeLog(DateTime.Now + " CostumerVisitReport.getPassesForLogs(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //    return passesAntenna0;
        //}

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
                debug.writeLog(DateTime.Now + " CostumerVisitReport.getGraphValues(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return graphValues;
        }

        private Dictionary<string, List<LogTO>> getElementsPasses(List<LogTO> passesAntennaOne)
        {
            //from list of passes make hashtable where kay is element string 
            Dictionary<string, List<LogTO>> finalPasses = new Dictionary<string,List<LogTO>>();
            try
            {
                Dictionary<string, List<LogTO>> elementsPasses = new Dictionary<string, List<LogTO>>();
                foreach (LogTO pass in passesAntennaOne)
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
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " CostumerVisitReport.getGraphValues(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return finalPasses;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}