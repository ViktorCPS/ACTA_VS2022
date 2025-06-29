using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ReaderInterface;
using System.Globalization;
using System.Resources;
using Util;
using Common;
using System.Collections;
using System.Threading;
using TransferObjects;
using UI;

namespace ACTASelftService
{
    public partial class SelfServMain : Form
    {
        IReaderInterface readerInterface;
        static int desktopReaderCOMPort = 0;

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        bool stopReading = false;

        public SelfServMain()
        {
            InitializeComponent();

            this.CenterToScreen();

            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            ReaderFactory.TechnologyType = Constants.DefaultTechType;
            readerInterface = ReaderFactory.GetReader;
			
        }

        private void timerReadingTag_Tick(object sender, EventArgs e)
        {
            try
            {
                if (stopReading)
                    return;
                stopReading = true;
                if (desktopReaderCOMPort == 0)
                {
                    desktopReaderCOMPort = readerInterface.FindDesktopReader();


                    //set note text
                    this.lblNote.Text = rm.GetString("showCardNote", culture);
                }
                                 
              
                if (desktopReaderCOMPort == 0)
                {
                    lblNote.Text = rm.GetString("readerNotFound", culture);
                     return;
                }

                uint tagID = UInt32.Parse(readerInterface.GetTagID(desktopReaderCOMPort));

                if (tagID != 0)
                {
                    EmployeeTO employee = new EmployeeTO();

                    List<EmployeeTO> employees = new Employee().SearchByTags(tagID.ToString());

                    if (employees.Count > 0)
                    {
                        employee = employees[0];
                        stopReading = true;
                        Choice ch = new Choice(employee);
                        ch.FormClosing += new FormClosingEventHandler(this.StartReading);
                        ch.ShowDialog();
                    }
                    else
                    {
                        employees = new Employee().SearchByBlockedTags(tagID.ToString());
                        if (employees.Count > 0)
                        {
                            lblNote.Text = rm.GetString("blockedCard", culture);
                        }
                        else
                        {
                            lblNote.Text = rm.GetString("unknownCard", culture);
                        }
                    }
                }
                else
                {
                    this.lblNote.Text = rm.GetString("showCardNote", culture);
                }

                Thread.Sleep(500);
                stopReading = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SelfSerfMain.timerDB_Elapsed(): " + ex.Message + "\n");
            }
        }

        private void StartReading(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                stopReading = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SelfSerfMain.StartReading(): " + ex.Message + "\n");
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void CheckInForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);

                setLanguage();

                if (desktopReaderCOMPort == 0)
                {
                    desktopReaderCOMPort = readerInterface.FindDesktopReader();

                    this.Cursor = Cursors.Arrow;
                    //set note text
                    this.lblNote.Text = rm.GetString("showCardNote", culture);
                }

                if (desktopReaderCOMPort == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    lblNote.Text = rm.GetString("readerNotFound", culture);
                    return;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SelfSerfMain.CheckInForm_Load(): " + ex.Message + "\n");
            }
            finally 
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setLanguage()
        {
            try
            {
                //set form text
                this.Text = rm.GetString("selfServ", culture);
                this.lblNote.Text = "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SelfSerfMain.setLanguage(): " + ex.Message + "\n");
            }
        }

        private void SelfServMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerReadingTag.Dispose();
        }
    }
}