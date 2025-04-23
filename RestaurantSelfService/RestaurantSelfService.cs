using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Collections;

using ReaderInterface;
using Common;
using Util;
using TransferObjects;

namespace RestaurantSelfService
{
    public partial class RestaurantSelfService : Form
    {
        IReaderInterface readerInterface;
        static int desktopReaderCOMPort = 0;

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        bool stopReading = false;

        public RestaurantSelfService()
        {
            InitializeComponent();

            // Init Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            ReaderFactory.TechnologyType = Constants.DefaultTechType;
            readerInterface = ReaderFactory.GetReader;
        }

        private void restaurantSelfService_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(UI.Employees).Assembly);

                ApplUserTO user = new ApplUserTO();
                user.Name = Constants.selfServUser;
                user.UserID = Constants.selfServUser;
                user.LangCode = Constants.Lang_sr;

                NotificationController.SetLogInUser(user);

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
                    List<EmployeeTO> employees = new Employee().SearchByTags(tagID.ToString());

                    if (employees.Count > 0)
                    {                        
                        stopReading = true;
                        EmployeeSchedules ch = new EmployeeSchedules(employees[0]);
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

        private void RestaurantSelfService_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerReadingTag.Dispose();
        }

      
    }
}