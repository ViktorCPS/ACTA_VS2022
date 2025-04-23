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

namespace ACTAMealsOrder
{
    public partial class MealsOrderMain : Form
    {
        IReaderInterface readerInterface;
        static int desktopReaderCOMPort = 0;

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        bool stopReading = false;

        public MealsOrderMain()
        {
            try
            {
                InitializeComponent();

                this.CenterToScreen();

                // Init Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                ReaderFactory.TechnologyType = Constants.DefaultTechType;
                readerInterface = ReaderFactory.GetReader;
                setBackgroundPicture();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrderMain.MealsOrderMain(): " + ex.Message + "\n");
            }
        }

        private void setBackgroundPicture()
        {
            try
            {
                Image image = Image.FromFile(Constants.KeteringPricaFirstImagePath);
                if (image != null)
                {
                    this.BackgroundImage = image;
                }
            }
            catch
            {

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
                log.writeLog(DateTime.Now + " MealsOrderMain.StartReading(): " + ex.Message + "\n");
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
                this.Text = rm.GetString("mealsOrder", culture);
                btnExit.Text = rm.GetString("btnExit", culture);
                this.lblNote.Text = "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrderMain.setLanguage(): " + ex.Message + "\n");
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
                    EmployeeTO employee = new EmployeeTO();

                    List<EmployeeTO> employees = new Employee().SearchByTags(tagID.ToString());

                    if (employees.Count > 0)
                    {
                        employee = employees[0];
                        stopReading = true;
                        UI.MealsOrder order = new UI.MealsOrder(employee);
                        order.FormClosing += new FormClosingEventHandler(this.StartReading);
                        order.ShowDialog();
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
                log.writeLog(DateTime.Now + " MealsOrderMain.timerDB_Elapsed(): " + ex.Message + "\n");
            }
        }

        private void MealsOrderMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                timerReadingTag.Dispose();
                Application.Exit();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrderMain.MealsOrderMain_FormClosing(): " + ex.Message + "\n");
            }
        }

        private void MealsOrderMain_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(UI.Employees).Assembly);

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
                log.writeLog(DateTime.Now + " MealsOrderMain.CheckInForm_Load(): " + ex.Message + "\n");
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();                              
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrderMain.btnExit_Click(): " + ex.Message + "\n");
            }

        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrderMain.btnExit_Click(): " + ex.Message + "\n");
            }
        }

        private void label2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                stopReading = true;
                UI.MealsOrder order = new UI.MealsOrder();
                order.FormClosing += new FormClosingEventHandler(this.StartReading);
                order.ShowDialog();
                Thread.Sleep(500);
                stopReading = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrderMain.label2_DoubleClick(): " + ex.Message + "\n");
            }
        }

    }
}
