using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;
using Reports;

namespace UI
{
    public partial class VisitorsMaintenance : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        private ArrayList visitorsList;
        private ArrayList visitorsListDump;

        public VisitorsMaintenance()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                this.tbDestination.Enabled = false;
                
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(VisitorsMaintenance).Assembly);
                
                DateTime firstDayLastMonth = DateTime.Today.AddMonths(-1);
                this.dtpUntilDate.Value = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month, 1);
                this.dtpDateFrom.Value = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month, 1);
                
                visitorsList = new ArrayList();
                visitorsListDump = new ArrayList();
                
                setLanguage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("menuVisitors", culture);

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
                log.writeLog(DateTime.Now + " VisitorsMaintenance.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " VisitorsMaintenance.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbSaveFiles_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
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
                log.writeLog(DateTime.Now + " VisitorsMaintenance.cbSaveFiles_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " VisitorsMaintenance.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpUntilDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                VisitorDocFile visitorDocFile = new VisitorDocFile();
                visitorsList = visitorDocFile.SearchFileCreatedTime(new DateTime(0), this.dtpUntilDate.Value.Date);
                this.lblNumberOfFiles.Text = visitorsList.Count.ToString();
                double size = 0;
                foreach (VisitorDocFile vistirDoc in visitorsList)
                {
                    size += vistirDoc.Content.Length;
                }
                size = size / Constants.megabytes;
                this.lblTotalSize.Text = size.ToString("F") + " MB";
                clearProgressBar(this.progressBar, lblProgressStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorsMaintenance.dtpUntilDate_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " VisitorsMaintenance.clearProgressBar(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            VisitorDocFile vdf = null;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.progressBar.Value = 0;
                if (this.tbDestination.Text.Equals("") && (this.cbSaveFiles.Checked))
                    MessageBox.Show(rm.GetString("selFolder", culture));
                else
                {
                    bool isSaved = false;
                    bool isDeleted = false;

                    vdf = new VisitorDocFile();

                    if (visitorsList.Count > 0)
                    {
                        this.progressBar.Maximum = visitorsList.Count;
                        vdf.BeginTransaction();
                        isDeleted = vdf.DeleteUntilDate(this.dtpUntilDate.Value, false);
                        if (isDeleted)
                        {
                            if (this.cbSaveFiles.Checked)
                            {
                                try
                                {
                                    foreach (VisitorDocFile visitorDocFile in visitorsList)
                                    {
                                        byte[] visitorDoc = visitorDocFile.Content;

                                        MemoryStream memStream = new MemoryStream(visitorDoc);

                                        // Set the position to the beginning of the stream.
                                        memStream.Seek(0, SeekOrigin.Begin);

                                        Image image = new Bitmap(memStream);
                                        string fileName = visitorDocFile.VisitID.ToString().Trim() + "_"
                                            + visitorDocFile.DocType.ToString().Trim() + "_"
                                            + visitorDocFile.CreatedTime.ToString("yyyyMMdd") + ".jpg";
                                        image.Save(tbDestination.Text + "\\" + fileName);
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
                            if ((isSaved) || (!this.cbSaveFiles.Checked))
                            {
                                this.progressBar.Value = this.progressBar.Maximum;
                                this.lblProgressStatus.Text = this.progressBar.Maximum + "/" + this.progressBar.Maximum;
                                vdf.CommitTransaction();
                            }
                            else
                            {
                                vdf.RollbackTransaction();
                            }
                        }
                        else
                        {
                            vdf.RollbackTransaction();
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
                if (vdf.GetTransaction() != null)
                {
                    vdf.RollbackTransaction();
                }
                log.writeLog(DateTime.Now + " VisitorsMaintenance.btnStart_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " VisitorsMaintenance.btnBrowseDump_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                VisitorDocFile visitorDocFile = new VisitorDocFile();
                visitorsListDump = visitorDocFile.SearchFileCreatedTime(this.dtpDateFrom.Value.Date, this.dtpDateTo.Value.Date);
                this.lblNumOfFilesDump.Text = visitorsListDump.Count.ToString();
                double size = 0;
                foreach (VisitorDocFile visitorDoc in visitorsListDump)
                {
                    size += visitorDoc.Content.Length;
                }
                size = size / Constants.megabytes;
                this.lblSizeOfFilesDump.Text = size.ToString("F") + " MB";
                clearProgressBar(this.pbDump, lblPBDumpStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorsMaintenance.btnBrowseDump_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnStartDump_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                pbDump.Value = 0;
                if (tbDestinationDump.Text.Equals(""))
                    MessageBox.Show(rm.GetString("selFolder", culture));
                else
                {
                    VisitorDocFile vdf = new VisitorDocFile();

                    if (this.visitorsListDump.Count > 0)
                    {
                        this.pbDump.Maximum = this.visitorsListDump.Count;

                        foreach (VisitorDocFile visitorDoc in visitorsListDump)
                        {
                            byte[] doc = visitorDoc.Content;

                            MemoryStream memStream = new MemoryStream(doc);

                            // Set the position to the beginning of the stream.
                            memStream.Seek(0, SeekOrigin.Begin);

                            Image image = new Bitmap(memStream);

                            string fileName = visitorDoc.VisitID.ToString().Trim() + "_"
                                + visitorDoc.DocType.ToString().Trim() + "_"
                                + visitorDoc.CreatedTime.ToString("yyyyMMdd") + ".jpg";
                            image.Save(this.tbDestinationDump.Text + "\\" + fileName);
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
                log.writeLog(DateTime.Now + " VisitorsMaintenance.btnBrowseDump_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void VisitorsMaintenance_Load(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void VisitorsMaintenance_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " VisitorsMaintenance.VisitorsMaintenance_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}