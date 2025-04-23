using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using TransferObjects;
using Common;
using Util;
using Reports;
using System.IO;

using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class CamSnapshotMaintenance : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        private ArrayList cameraSnapshotsList;
        private ArrayList cameraSnapshotsListDump;

        public CamSnapshotMaintenance()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.tbDestination.Enabled = false;
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            DateTime firstDayLastMonth = DateTime.Today.AddMonths(-1);
            this.dtpUntilDate.Value = new DateTime(firstDayLastMonth.Year,firstDayLastMonth.Month,1);
            this.dtpDateFrom.Value = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month, 1);
            cameraSnapshotsList = new ArrayList();
            cameraSnapshotsListDump = new ArrayList();
            setLanguage();

        }

        private void cbSaveFiles_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
               
                this.Cursor = Cursors.WaitCursor;
            
                if (this.cbSaveFiles.Checked)
                {
                    this.gbDestination.Enabled = true;
                }
                else
                {
                    this.gbDestination.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotMaintenance.cbSaveFiles_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                clearProgressBar(this.progressBar, lblProgressStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotMaintenance.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpUntilDate_ValueChanged(object sender, EventArgs e)
        {
           
        }
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("menuCameraSnapshotMaintenance", culture);

                // group box text
                this.gbDestination.Text = rm.GetString("gbDestination", culture);
                this.gbProgress.Text = rm.GetString("gbProgress", culture);
                this.gbSelectEndDate.Text = rm.GetString("gbSelectEndDate", culture);
                this.gbDestinationDump.Text = rm.GetString("gbDestination", culture);
                this.gbFound.Text = rm.GetString("gbFound", culture);
                this.gbProgressDump.Text = rm.GetString("gbProgress", culture);
                this.gbSearchCriteria.Text = rm.GetString("gbSearchCriteria", culture);
                
                // button's text
                this.btnBrowse.Text = rm.GetString("btnBrowse", culture);
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnStart.Text = rm.GetString("btnStart", culture);
                this.btnBrowseDump.Text = rm.GetString("btnBrowse", culture);
                this.btnStartDump.Text = rm.GetString("btnStart", culture);
                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.btnPhotoPreview.Text = rm.GetString("btnPhotoPreview", culture);
                this.btnSearch1.Text = rm.GetString("btnSearch", culture);
                                
                // label's text
                this.lblNumber.Text = rm.GetString("lblNumber", culture);
                this.lblSize.Text = rm.GetString("lblSize", culture);
                this.lblNumberDump.Text = rm.GetString("lblNumber", culture);
                this.lblSizeDump.Text = rm.GetString("lblSize", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblUntil.Text = rm.GetString("lblUntil", culture);
               
                // check box text
                this.cbSaveFiles.Text = rm.GetString("cbSaveFiles", culture);

                // tab pages text
                this.tpDump.Text = rm.GetString("tpDump", culture);
                this.tpClearHistory.Text = rm.GetString("tpClearHistory", culture);
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void clearProgressBar(ProgressBar pb, Label lbl)
        {
            try
            {
                pb.Value = 0;
                lbl.Text = "";
                lbl.Refresh();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.clearProgressBar(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.progressBar.Value = 0;
                if (this.tbDestination.Text.Equals("")&&(this.cbSaveFiles.Checked))
                    MessageBox.Show(rm.GetString("selFolder", culture));
                else
                {
                    bool isSaved = false;
                    bool isDeleted = false;
                    
                    CameraSnapshotFile csf = new CameraSnapshotFile();
                    
                    if (cameraSnapshotsList.Count > 0)
                    {
                        this.progressBar.Maximum = cameraSnapshotsList.Count;
                        csf.BeginTransaction();
                        isDeleted = csf.DeleteUntilDate(this.dtpUntilDate.Value, false);
                        if (isDeleted)
                        {
                            if (this.cbSaveFiles.Checked)
                            {
                                try
                                {

                                    foreach (CameraSnapshotFile cameraSnapshotFile in cameraSnapshotsList)
                                    {

                                        byte[] camSnapshots = cameraSnapshotFile.Content;

                                        MemoryStream memStream = new MemoryStream(camSnapshots);

                                        // Set the position to the beginning of the stream.
                                        memStream.Seek(0, SeekOrigin.Begin);

                                        Image image = new Bitmap(memStream);

                                        image.Save(tbDestination.Text + "\\" + cameraSnapshotFile.FileName);
                                        memStream.Close();
                                        this.progressBar.Value++;
                                        this.lblProgressStatus.Text = this.progressBar.Value + "/" + this.progressBar.Maximum;
                                        this.lblProgressStatus.Refresh();
                                    }
                                    isSaved = true;
                                }
                                catch
                                {
                                    isSaved = false;
                                }
                            }
                            if ((isSaved)||(!this.cbSaveFiles.Checked))
                            {
                                this.progressBar.Value = this.progressBar.Maximum;
                                this.lblProgressStatus.Text = this.progressBar.Maximum + "/" + this.progressBar.Maximum;
                                csf.CommitTransaction();
                            }
                            else
                            {
                                csf.RollbackTransaction();
                            }
                        }
                        else
                        {
                            csf.RollbackTransaction();
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noPhotos", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " btnStart_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnBrowseDump_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FolderBrowserDialog fbDialog = new FolderBrowserDialog();

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.tbDestinationDump.Text = fbDialog.SelectedPath.ToString();
                }
                clearProgressBar(this.pbDump, lblPBDumpStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotMaintenance.btnBrowseDump_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                CameraSnapshotFile cameraSnapshotFile = new CameraSnapshotFile();
                this.cameraSnapshotsListDump = cameraSnapshotFile.SearchFileCreatedTime(this.dtpDateFrom.Value.Date, this.dtpDateTo.Value.Date);
                this.lblNumOfFilesDump.Text = this.cameraSnapshotsListDump.Count.ToString();
                double size = 0;
                foreach (CameraSnapshotFile camSnapshot in this.cameraSnapshotsListDump)
                {
                    size += camSnapshot.Content.Length;
                }
                size = size / Constants.megabytes;
                this.lblSizeOfFilesDump.Text = size.ToString("F") + " MB";
                clearProgressBar(this.pbDump, lblPBDumpStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotMaintenance.btnBrowseDump_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnStartDump_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.pbDump.Value = 0;
                if (this.tbDestinationDump.Text.Equals("") )
                    MessageBox.Show(rm.GetString("selFolder", culture));
                else
                {
                    CameraSnapshotFile csf = new CameraSnapshotFile();

                    if (this.cameraSnapshotsListDump.Count > 0)
                    {
                        this.pbDump.Maximum = this.cameraSnapshotsListDump.Count;
                                                 
                                    foreach (CameraSnapshotFile cameraSnapshotFile in cameraSnapshotsListDump)
                                    {

                                        byte[] camSnapshots = cameraSnapshotFile.Content;

                                        MemoryStream memStream = new MemoryStream(camSnapshots);

                                        // Set the position to the beginning of the stream.
                                        memStream.Seek(0, SeekOrigin.Begin);

                                        Image image = new Bitmap(memStream);

                                        image.Save(this.tbDestinationDump.Text + "\\" + cameraSnapshotFile.FileName);
                                        memStream.Close();
                                        this.pbDump.Value++;
                                        this.lblPBDumpStatus.Text = this.pbDump.Value + "/" + this.pbDump.Maximum;
                                        this.lblPBDumpStatus.Refresh();
                                    }
                                                            
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noPhotos", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotMaintenance.btnBrowseDump_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void CamSnapshotMaintenance_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " CamSnapshotMaintenance.CamSnapshotMaintenance_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void CamSnapshotMaintenance_Load(object sender, EventArgs e)
        {
            //btnSearch_Click(sender, e);
        }

        private void btnPhotoPreview_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                CameraSnapshots cs = new CameraSnapshots();
                cs.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamSnapshotMaintenance.Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                CameraSnapshotFile cameraSnapshotFile = new CameraSnapshotFile();
                cameraSnapshotsList = cameraSnapshotFile.SearchFileCreatedTime(new DateTime(0), this.dtpUntilDate.Value.Date);
                this.lblNumberOfFiles.Text = cameraSnapshotsList.Count.ToString();
                double size = 0;
                foreach (CameraSnapshotFile camSnapshot in cameraSnapshotsList)
                {
                    size += camSnapshot.Content.Length;
                }
                size = size / Constants.megabytes;
                this.lblTotalSize.Text = size.ToString("F") + " MB";
                clearProgressBar(this.progressBar, lblProgressStatus);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshotMaintenance.btnSearch1_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}