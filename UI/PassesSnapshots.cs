using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;
using System.IO;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    public partial class PassesSnapshots : Form
    {
        private CameraSnapshotFile currentCameraSnapshotFile = null;

        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        string passID = "";
        private ArrayList photosList = null;
        private ArrayList cameraSFList = null;
        int currentPhotoIndex = 0;
        int photoCount = 0;
        private List<PassTO> passesList = null;
        int currentPassIndex = 0;
        int passCount = 0;
        private ArrayList employeeImageList = null;
        decimal usedOffset = 30;
        ArrayList photosForDisplay = null;
        //All time schemas
        List<WorkTimeSchemaTO> timeSchemas = null;
        // all employee time schedules for selected Time Interval, key is employee ID
        Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();
        string employeeIDString = "";
        PassTO currentPass;

        public PassesSnapshots(string passID)
        {
            try
            {
                InitializeComponent();

                this.passID = passID;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentCameraSnapshotFile = new CameraSnapshotFile();
                photosList = new ArrayList();
                cameraSFList = new ArrayList();
                passesList = new List<PassTO>();
                employeeImageList = new ArrayList();
                photosForDisplay = new ArrayList();
                timeSchemas = new List<WorkTimeSchemaTO>();
                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(PassesSnapshots).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.PassesSnapshots(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Set proper language and initialize List View
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("passesSnapshotsForm", culture);

                // group box text
                gbCurrentPass.Text = rm.GetString("gbCurrentPass", culture);
                gbEmployee.Text = rm.GetString("gbEmployee", culture);
                gbPass.Text = rm.GetString("gbPass", culture);
                gbPhotos.Text = rm.GetString("gbPhotos", culture);
                gbPhotoNavigation.Text = rm.GetString("gbPhotoNavigation", culture);
                gbPassNavigation.Text = rm.GetString("gbPassNavigation", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnRefresh.Text = rm.GetString("btnRefresh", culture);

                // label's text
                lblEmplName.Text = rm.GetString("lblLFName", culture);
                lblEmplWU.Text = rm.GetString("lblWorkingUnit", culture);
                lblEmplWT.Text = rm.GetString("lblWorkTime1", culture);
                lblPassTime.Text = rm.GetString("lblTime", culture);
                lblPassDirection.Text = rm.GetString("lblPrimDirect", culture);
                lblPassLocation.Text = rm.GetString("lblLocation", culture);
                lblPassType.Text = rm.GetString("lblPassType", culture);
                lblPassIsWrkHrs.Text = rm.GetString("lblIsWrkHrs", culture);
                lblFileCTime.Text = rm.GetString("lblFileCrTime", culture);
                lblOffset.Text = rm.GetString("lblOffset", culture);
                lblSec.Text = rm.GetString("lblSec", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " PassesSnapshots.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void PassesSnapshots_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                currentPass = new PassTO();
                fillPassesList();

                // all time shemas
                timeSchemas = new TimeSchema().Search();

                DateTime fromDay = new DateTime(0);
                DateTime toDay = new DateTime(0);
                //string employeeIDString = "";
                for (int i = 0; i < passesList.Count; i++)
                {
                    PassTO pass = passesList[i];
                    int currentEmployeeID = pass.EmployeeID;
                    if (!emplTimeSchedules.ContainsKey(currentEmployeeID))
                    {
                        employeeIDString += currentEmployeeID.ToString().Trim() + ",";

                        emplTimeSchedules.Add(currentEmployeeID, new List<EmployeeTimeScheduleTO>());
                    }

                    if ((fromDay == new DateTime(0)) || (fromDay.Date > pass.EventTime.Date))
                        fromDay = pass.EventTime;

                    if ((toDay == new DateTime(0)) || (toDay.Date < pass.EventTime.Date))
                        toDay = pass.EventTime;
                }
                if (employeeIDString.Length > 0)
                {
                    employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);
                }

                //get time schemas for selected Employees, for selected Time Interval
                List<EmployeeTimeScheduleTO> timeSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedules(employeeIDString, fromDay, toDay);
                for (int i = 0; i < timeSchedules.Count; i++)
                {
                    emplTimeSchedules[timeSchedules[i].EmployeeID].Add(timeSchedules[i]);
                }

                fillEmployeeImageList();
                currentPassIndex = 0;
                displayPass();

                fillPhotosList();
                displayPhotos();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.PassesSnapshots_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void fillPassesList()
        {
            try
            {
                passesList = new Pass().SearchForSnapshots(passID);

                passCount = passesList.Count;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.fillPassesList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void fillEmployeeImageList()
        {
            try
            {
                if (employeeIDString != "")
                {
                    employeeImageList = (new EmployeeImageFile()).SearchImageForSnapshots(employeeIDString);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.fillEmployeeImageList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void fillPhotosList()
        {
            try
            {
                usedOffset = nudOffset.Value;
                photosList.Clear();
                photosForDisplay.Clear();
                photoCount = 0;

                if (passCount > 0)
                {
                    PassTO pass = passesList[currentPassIndex];
                    currentPass = pass;

                    DateTime fromDate = pass.EventTime.Subtract(Constants.minutesOffsetForFileTime);
                    DateTime toDate = pass.EventTime.Add(Constants.minutesOffsetForFileTime);
                    string direction = "";
                    if (pass.Direction.Equals(Constants.DirectionIn))
                        direction = "'" + Constants.DirectionIn + "', '" + Constants.DirectionInOut + "'";
                    else if (pass.Direction.Equals(Constants.DirectionOut))
                        direction = "'" + Constants.DirectionOut + "', '" + Constants.DirectionInOut + "'";
                    else if (pass.Direction.Equals(Constants.DirectionInOut))
                        direction = "'" + Constants.DirectionInOut + "'";
                    ArrayList photosForPass = currentCameraSnapshotFile.SearchForPass(pass.PassID, fromDate, toDate, direction);

                    if (photosForPass.Count > 0)
                    {
                        TimeSpan averrageError = calculateAverrageError(photosForPass);

                        int selOffset = (int)nudOffset.Value;
                        TimeSpan offset;
                        if (selOffset > 59)
                            offset = new TimeSpan(0, (selOffset / 60), (selOffset % 60));
                        else
                            offset = new TimeSpan(0, 0, selOffset);

                        DateTime fromOffsetDate = pass.EventTime.Subtract(offset);
                        DateTime toOffsetDate = pass.EventTime.Add(offset);

                        string recordID = "";
                        int index = -1;
                        TimeSpan minDiff = new TimeSpan();
                        TimeSpan currDiff = new TimeSpan();
                        currentPhotoIndex = 0;
                        foreach (CameraSnapshotFile csf in photosForPass)
                        {
                            DateTime effectiveTime = new DateTime(0);

                            TimeSpan diff = new TimeSpan();
                            if (csf.FileCreatedTime > csf.CameraCreatedTime)
                                diff = csf.FileCreatedTime.Subtract(csf.CameraCreatedTime);
                            else
                                diff = csf.CameraCreatedTime.Subtract(csf.FileCreatedTime);

                            if (diff <= Constants.diffFileCameraCTforEfc)
                            {
                                effectiveTime = csf.CameraCreatedTime;
                            }
                            else 
                            {
                                effectiveTime = csf.FileCreatedTime.Subtract(averrageError);
                            }

                            if ((effectiveTime >= fromOffsetDate)
                                && (effectiveTime <= toOffsetDate))
                            {
                                recordID += csf.RecordID.ToString() + ",";
                                index++;

                                if (effectiveTime > pass.EventTime)
                                    currDiff = effectiveTime.Subtract(pass.EventTime);
                                else
                                    currDiff = pass.EventTime.Subtract(effectiveTime);

                                if ((currDiff < minDiff) || (index == 0))
                                {
                                    minDiff = currDiff;
                                    currentPhotoIndex = index;
                                }
                            }
                        }
                        if (recordID != "")
                            recordID = recordID.Substring(0, recordID.Length - 1);

                        if (recordID != "")
                        {
                            photosForDisplay = currentCameraSnapshotFile.SearchForPassDisplay(recordID);

                            foreach (CameraSnapshotFile cameraSF in photosForDisplay)
                            {
                                byte[] passPhoto = cameraSF.Content;
                                MemoryStream memStream = new MemoryStream(passPhoto);
                                Image img = new Bitmap(memStream);
                                photosList.Add(img);
                                cameraSFList.Add(cameraSF);

                                memStream.Close();
                            }

                            photoCount = photosList.Count;
                        } //if (recordID != "")
                    } //if (photosForPass.Count > 0)

                    if (photoCount == 0)
                    {
                        MessageBox.Show(rm.GetString("noPhotosForPass", culture));
                    }
                } //if (passCount > 0)
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.fillPhotosList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void displayPass()
        {
            try
            {
                if (passCount > 0)
                {
                    PassTO pass = passesList[currentPassIndex];

                    lblEmplNameVal.Text = pass.EmployeeName;
                    lblEmplWUVal.Text = pass.WUName;

                    lblPassTimeVal.Text = pass.EventTime.ToString("dd.MM.yyyy  HH:mm:ss");
                    lblPassDirectionVal.Text = pass.Direction;
                    lblPassLocationVal.Text = pass.LocationName;
                    lblPassTypeVal.Text = pass.PassType;

                    if (pass.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                    {
                        lblPassIsWrkHrsVal.Text = rm.GetString("yes", culture);
                    }
                    else
                    {
                        lblPassIsWrkHrsVal.Text = rm.GetString("no", culture);
                    }

                    bool photoExist = false;
                    foreach (EmployeeImageFile eif in employeeImageList)
                    {
                        if (eif.EmployeeID == pass.EmployeeID)
                        {
                            byte[] emplPhoto = eif.Picture;
                            MemoryStream memStream = new MemoryStream(emplPhoto);
                            Image img = new Bitmap(memStream);
                            pbEmployee.Image = img;
                            memStream.Close();

                            lblOfficialEmplPhoto.Text = rm.GetString("lblOfficialEmplPhoto", culture);
                            photoExist = true;

                            break;
                        }
                    }
                    if (!photoExist)
                    {
                        pbEmployee.Image = null;
                        lblOfficialEmplPhoto.Text = "";
                    }

                    lblPassNum.Text = (currentPassIndex + 1).ToString() + " / " + passCount.ToString();

                    // list of Time Schemas for employee and date used
                    List<EmployeeTimeScheduleTO> timeScheduleList = emplTimeSchedules[pass.EmployeeID];
                    cbEmplWTVal.Items.Clear();
                    if (timeScheduleList.Count > 0)
                    {
                        int dayNum = -1;
                        WorkTimeSchemaTO actualTimeSchema = Common.Misc.getTimeSchemaForDayAndDayNum(timeScheduleList,
                            pass.EventTime, timeSchemas, ref dayNum);
                        if ((actualTimeSchema != null) && (dayNum >= 0))
                        {
                            Dictionary<int, WorkTimeIntervalTO> dayIntervals = actualTimeSchema.Days[dayNum];
                            if (dayIntervals != null)
                            {
                                for (int i = 0; i < dayIntervals.Count; i++)
                                {
                                    WorkTimeIntervalTO tsInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                    cbEmplWTVal.Items.Add(tsInterval.StartTime.ToString("HH:mm") + " - " + tsInterval.EndTime.ToString("HH:mm"));
                                }

                                if (cbEmplWTVal.Items.Count > 0)
                                    cbEmplWTVal.SelectedIndex = 0;
                            }
                        }
                    }
                }
                else
                {
                    lblPassNum.Text = "";
                    lblEmplNameVal.Text = "";
                    lblEmplWUVal.Text = "";
                    cbEmplWTVal.Items.Clear();
                    lblPassTimeVal.Text = "";
                    lblPassDirectionVal.Text = "";
                    lblPassLocationVal.Text = "";
                    lblPassTypeVal.Text = "";
                    lblPassIsWrkHrsVal.Text = "";
                    pbEmployee.Image = null;
                    lblOfficialEmplPhoto.Text = "";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.displayPass(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void displayPhotos()
        {
            try
            {
                if (photoCount > 0)
                {
                    if (currentPhotoIndex >= photoCount)
                        currentPhotoIndex = 0;

                    if (((currentPhotoIndex - 2) >= 0) && ((currentPhotoIndex - 2) < photoCount))
                    {
                        pb1.Image = (Image)photosList[currentPhotoIndex - 2];
                    }
                    else
                    {
                        pb1.Image = null;
                    }

                    if (((currentPhotoIndex - 1) >= 0) && ((currentPhotoIndex - 1) < photoCount))
                    {
                        pb2.Image = (Image)photosList[currentPhotoIndex - 1];
                    }
                    else
                    {
                        pb2.Image = null;
                    }

                    if ((currentPhotoIndex >= 0) && (currentPhotoIndex < photoCount))
                    {
                        pb3.Image = (Image)photosList[currentPhotoIndex];
                    }
                    else
                    {
                        pb3.Image = null;
                    }

                    if (((currentPhotoIndex + 1) >= 0) && ((currentPhotoIndex + 1) < photoCount))
                    {
                        pb4.Image = (Image)photosList[currentPhotoIndex + 1];
                    }
                    else
                    {
                        pb4.Image = null;
                    }

                    if (((currentPhotoIndex + 2) >= 0) && ((currentPhotoIndex + 2) < photoCount))
                    {
                        pb5.Image = (Image)photosList[currentPhotoIndex + 2];
                    }
                    else
                    {
                        pb5.Image = null;
                    }

                    lblPhotoNum.Text = (currentPhotoIndex + 1).ToString() + " / " + photoCount.ToString();
                    lblFileCTime.Text = rm.GetString("lblFileCrTime", culture) + " "
                        + ((CameraSnapshotFile)photosForDisplay[currentPhotoIndex]).FileCreatedTime.ToString("dd.MM.yyyy  HH:mm:ss");
                }
                else
                {
                    lblPhotoNum.Text = "";
                    lblFileCTime.Text = "";
                    pb1.Image = null;
                    pb2.Image = null;
                    pb3.Image = null;
                    pb4.Image = null;
                    pb5.Image = null;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.displayPhotos(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPhotoPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if ((currentPhotoIndex - 1) >= 0)
                {
                    currentPhotoIndex--;
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnPhotoPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPhotoNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if ((currentPhotoIndex + 1) < photoCount)
                {
                    currentPhotoIndex++;
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnPhotoNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPhotoFirst_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (currentPhotoIndex != 0)
                {
                    currentPhotoIndex = 0;
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnPhotoFirst_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPhotoLast_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (currentPhotoIndex != (photoCount - 1))
                {
                    currentPhotoIndex = photoCount - 1;
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnPhotoLast_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPassPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if ((currentPassIndex - 1) >= 0)
                {
                    currentPassIndex--;
                    displayPass();

                    fillPhotosList();
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnPassPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPassNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if ((currentPassIndex + 1) < passCount)
                {
                    currentPassIndex++;
                    displayPass();

                    fillPhotosList();
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnPassNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPassFirst_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (currentPassIndex != 0)
                {
                    currentPassIndex = 0;
                    displayPass();

                    fillPhotosList();
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnPassFirst_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPassLast_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Arrow;

                if (currentPassIndex != (passCount - 1))
                {
                    currentPassIndex = passCount - 1;
                    displayPass();

                    fillPhotosList();
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnPassLast_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (usedOffset != nudOffset.Value)
                {
                    fillPhotosList();
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.btnRefresh_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private TimeSpan calculateAverrageError(ArrayList photosForPass)
        {
            TimeSpan averrageError = Constants.difoltAverrageError;
            int count = 0;

            try
            {
                TimeSpan sum = new TimeSpan();
                foreach (CameraSnapshotFile cameraSF in photosForPass)
                {
                    TimeSpan diff = new TimeSpan();
                    if (cameraSF.FileCreatedTime > cameraSF.CameraCreatedTime)
                        diff = cameraSF.FileCreatedTime.Subtract(cameraSF.CameraCreatedTime);
                    else
                        diff = cameraSF.CameraCreatedTime.Subtract(cameraSF.FileCreatedTime);

                    if (diff <= Constants.diffFileCameraCTforAvg)
                    {
                        sum = sum.Add(diff);
                        count++;
                    }
                }

                if (count > 0)
                {
                    long averageTicks = (sum.Ticks / count);
                    averrageError = new TimeSpan(averageTicks);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.calculateAverrageError(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return averrageError;
        }

        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            bool blnProcess = false;
            try
            {
                if (keyData == Keys.Right)
                {
                    // Process the keystroke
                    blnProcess = true;

                    if ((currentPhotoIndex + 1) < photoCount)
                    {
                        currentPhotoIndex++;
                        displayPhotos();
                    }
                }
                else if (keyData == Keys.Left)
                {
                    // Process the keystroke
                    blnProcess = true;

                    if ((currentPhotoIndex - 1) >= 0)
                    {
                        currentPhotoIndex--;
                        displayPhotos();
                    }
                }
                else if (keyData == Keys.Up)
                {
                    // Process the keystroke
                    blnProcess = true;

                    if ((currentPassIndex + 1) < passCount)
                    {
                        currentPassIndex++;
                        displayPass();

                        fillPhotosList();
                        displayPhotos();
                    }
                }
                else if (keyData == Keys.Down)
                {
                    // Process the keystroke
                    blnProcess = true;

                    if ((currentPassIndex - 1) >= 0)
                    {
                        currentPassIndex--;
                        displayPass();

                        fillPhotosList();
                        displayPhotos();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.ProcessCmdKey(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            if (blnProcess == true)
                return true;
            else
                return base.ProcessCmdKey(ref m, keyData);
        }

        private void PassesSnapshots_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.PassesSnapshots_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void pb3_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if ((currentPhotoIndex >= 0) && (currentPhotoIndex < photoCount))
                {
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex, currentPass, cameraSFList);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.pb3_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void pb1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (((currentPhotoIndex - 2) >= 0) && ((currentPhotoIndex - 2) < photoCount))
                {
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex - 2, currentPass, cameraSFList);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.pb1_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void pb2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (((currentPhotoIndex - 1) >= 0) && ((currentPhotoIndex - 1) < photoCount))
                {
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex - 1, currentPass, cameraSFList);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.pb2_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.Arrow; 
            }
        }

        private void pb4_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (((currentPhotoIndex + 1) >= 0) && ((currentPhotoIndex + 1) < photoCount))
                {
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex + 1, currentPass, cameraSFList);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.pb4_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void pb5_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (((currentPhotoIndex + 2) >= 0) && ((currentPhotoIndex + 2) < photoCount))
                {
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex + 2, currentPass, cameraSFList);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesSnapshots.pb5_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}