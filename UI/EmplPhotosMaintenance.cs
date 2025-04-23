using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;
using Reports;

namespace UI
{
    public partial class EmplPhotosMaintenance : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;
        List<WorkingUnitTO> WUnits;
        List<EmployeeTO> emplArray;
        List<WorkingUnitTO> wuList;

        public EmplPhotosMaintenance()
        {
            InitializeComponent();
            this.CenterToScreen();
            WUnits = new List<WorkingUnitTO>();
            emplArray = new List<EmployeeTO>();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            setLanguage();

            populateWorkingUnitCombo();
            populateEmployeeCombo(new WorkingUnitTO());
            this.tbDestination.Enabled = false;
            this.tbSource.Enabled = false;
        }

        private void populateEmployeeCombo(WorkingUnitTO wu)
        {
            try
            {
                string wuString = "";
                List<WorkingUnitTO> wUnitsList = new List<WorkingUnitTO>();
                if ((wu.WorkingUnitID < 0) && (logInUser != null))
                {
                    wUnitsList = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }
                else
                {
                    wUnitsList = new List<WorkingUnitTO>();
                    wUnitsList.Add(wu);
                    wUnitsList = new WorkingUnit().FindAllChildren(wUnitsList);
                }

                foreach (WorkingUnitTO wUnit in wUnitsList)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                emplArray = new Employee().SearchByWU(wuString);

                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl1 = new EmployeeTO();
                empl1.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, empl1);

                cbEmployee.DataSource = emplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.Tag = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbEmployee_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                clearProgressBar(this.progressBarDump, this.lblProgressStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow; 
            }
        }
        private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //MessageBox.Show("Proba " + cbWU.SelectedValue.ToString() + " " + cbWU.SelectedIndex.ToString());
                if (cbWU.SelectedValue is WorkingUnitTO)
                {
                    populateEmployeeCombo((WorkingUnitTO)cbWU.SelectedValue);
                }
                clearProgressBar(this.progressBarDump, this.lblProgressStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateWorkingUnitCombo()
        {
            try
            {
                wuList = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    WUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                WUnits.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWU.DataSource = WUnits;
                cbWU.DisplayMember = "Name";
                //cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("menuEmplPhotosMaintenance", culture);

                // group box text
                this.gbDestination.Text = rm.GetString("gbDestination", culture);
                this.gbProgress.Text = rm.GetString("gbProgress", culture);
                this.gbSource.Text = rm.GetString("gbSource", culture);
                this.gbProgressLoad.Text = rm.GetString("gbProgress", culture);

                // button's text
                this.btnBrowse.Text = rm.GetString("btnBrowse", culture);
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnStart.Text = rm.GetString("btnStart", culture);
                this.btnBrowseLoad.Text = rm.GetString("btnBrowse", culture);
                this.btnStartLoad.Text = rm.GetString("btnStart", culture);

                // label's text
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblWU.Text = rm.GetString("lblWU", culture);
                lblEmplPhotosNote.Text = rm.GetString("lblEmplPhotosNote", culture);

                // tab pages text
                this.tpDump.Text = rm.GetString("tpDump", culture);
                this.tpLoad.Text = rm.GetString("tpLoad", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.setLanguage(): " + ex.Message + "\n");
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
                clearProgressBar(this.progressBarDump, this.lblProgressStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            
            EmployeeImageFile eif = new EmployeeImageFile();
            this.Cursor = Cursors.WaitCursor;
            this.progressBarDump.Value = 0;
            try
            {
                if (this.tbDestination.Text.Equals(""))
                    MessageBox.Show(rm.GetString("selFolder", culture));
                else
                {
                   
                    string emplString = "";
                    EmployeeTO empl = (EmployeeTO)cbEmployee.SelectedValue;
                    if (empl.EmployeeID == -1)
                    {
                        foreach (EmployeeTO employee in emplArray)
                        {
                            emplString += employee.EmployeeID + ", ";
                        }
                        if (emplString.Length > 0)
                        {
                            emplString = emplString.Substring(0, emplString.Length - 2);
                        }
                    }
                    else
                    {
                        emplString = empl.EmployeeID.ToString();
                    }


                    ArrayList al = eif.SearchImageForSnapshots(emplString);

                    if (al.Count > 0)
                    {
                        this.progressBarDump.Maximum = al.Count;

                        foreach (EmployeeImageFile emplImageFile in al)
                        {
                            byte[] emplPhoto = emplImageFile.Picture;

                            MemoryStream memStream = new MemoryStream(emplPhoto);

                            // Set the position to the beginning of the stream.
                            memStream.Seek(0, SeekOrigin.Begin);

                            Image image = new Bitmap(memStream);

                            image.Save(tbDestination.Text + "\\" + emplImageFile.EmployeeID + ".jpg");
                            memStream.Close();
                            this.progressBarDump.Value++;
                            this.lblProgressStatus.Text = this.progressBarDump.Value + "/" + this.progressBarDump.Maximum;
                            this.lblProgressStatus.Refresh();
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
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.btnStart_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;                
            }

        }

        private void btnBrowseLoad_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FolderBrowserDialog fbDialog = new FolderBrowserDialog();

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.tbSource.Text = fbDialog.SelectedPath.ToString();
                }
                clearProgressBar(this.progressBarLoad, this.lblProgressLoadStatus);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.btnBrowseLoad_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnStartLoad_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.progressBarLoad.Value = 0;
            try
            {
                if (this.tbSource.Text.Equals(""))
                    MessageBox.Show(rm.GetString("selFolder", culture));
                else
                {

                    this.progressBarLoad.Maximum = Directory.GetFiles(this.tbSource.Text).Length;
                    
                    this.progressBarLoad.Value = 0;
                    foreach (string Filename in Directory.GetFiles(this.tbSource.Text))
                    {
                        
                        FileInfo fiPicture = new FileInfo(Filename);
                        if (fiPicture.Length > 1043576)
                        {
                            MessageBox.Show(rm.GetString("imgLarge", culture));
                        }
                        else
                        {
                            // If it's a JPG file
                            if (fiPicture.Extension.ToLower() == ".jpg")
                            {
                                int employeeID = -1;
                                try
                                {
                                    employeeID = int.Parse(fiPicture.Name.Substring(0, fiPicture.Name.Length - 4));

                                }
                                catch (FormatException)
                                {
                                    continue;
                                }
                                Employee empl = new Employee();
                                //check if employee exist
                                empl.EmplTO.EmployeeID = employeeID;
                                List<EmployeeTO> al = empl.Search();
                                if (al.Count > 0)
                                {
                                    bool imageInserted = false;
                                    FileStream FilStr = new FileStream(fiPicture.FullName, FileMode.Open);
                                    if (FilStr != null)
                                    {
                                        BinaryReader BinRed = new BinaryReader(FilStr);

                                        byte[] imgbyte = new byte[FilStr.Length + 1];

                                        // Here you use ReadBytes method to add a byte array of the image stream.
                                        //so the image column will hold a byte array.
                                        imgbyte = BinRed.ReadBytes(Convert.ToInt32(BinRed.BaseStream.Length));

                                        BinRed.Close();
                                        FilStr.Close();
                                        EmployeeImageFile employeeImageFile = new EmployeeImageFile();
                                        Employee employee = new Employee();
                                        ArrayList arrList = employeeImageFile.Search(employeeID);
                                        if (arrList.Count > 0)
                                        {
                                            imageInserted = employeeImageFile.Update(employeeID, imgbyte, true);
                                        }
                                        else
                                        {
                                             int saved = 0;
                                             bool updated = false;
                                             bool trans = employeeImageFile.BeginTransaction();

                                             if (trans)
                                             {
                                                 saved = employeeImageFile.Save(employeeID, imgbyte, false);
                                                 if (saved > 0)
                                                 {
                                                     employee.SetTransaction(employeeImageFile.GetTransaction());
                                                     string picture = employeeID.ToString()+".jpg";
                                                     updated = employee.UpdatePicture(employeeID, picture, false);
                                                 }
                                                 if (updated)
                                                 {
                                                     employeeImageFile.CommitTransaction();
                                                     imageInserted = true;
                                                 }
                                                 else
                                                 {
                                                     employeeImageFile.RollbackTransaction();
                                                 }
                                             }
                                             else
                                             {
                                                 MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                 return;
                                             }
                                        }
                                    } //if (FilStr != null)

                                    if (imageInserted)
                                    {
                                        this.progressBarLoad.Value++;
                                        this.lblProgressLoadStatus.Text = this.progressBarLoad.Value + "/" + this.progressBarLoad.Maximum;
                                        this.lblProgressLoadStatus.Refresh();
                                    }
                                }                               
                            }
                        }
                    }
                    if (this.progressBarLoad.Value < this.progressBarLoad.Maximum)
                    {
                        MessageBox.Show(rm.GetString("NumOfEmplNotFound", culture) + " " + (this.progressBarLoad.Maximum-this.progressBarLoad.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.btnStartLoad_Click(): " + ex.Message + "\n");
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

        private void EmplPhotosMaintenance_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.EmplPhotosMaintenance_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string wuString = "";
                foreach (WorkingUnitTO wUnit in wuList)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplPhotosMaintenance.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

    }
}