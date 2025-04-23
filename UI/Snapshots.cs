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
    public partial class Snapshots : Form
    {
        private ArrayList photosList = null;
        private ArrayList photosListBytes = null;
        private ArrayList cameraArray = null;
        private int currentPhotoIndex = 0;
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        private Size pictureBoxSize;
        private string picutreName;

        //key is cameraID value is camera object
        Hashtable cameras = new Hashtable();

       
        
        public Snapshots()
        {
            InitializeComponent();

            photosList = new ArrayList();
            photosListBytes = new ArrayList();
            cameraArray = new ArrayList();
            currentPhotoIndex = 0;
            pictureBoxSize = new Size();
            picutreName = "";
        }
        public Snapshots(ArrayList photosArray, int selectedImage, PassTO pass, ArrayList cameraArray)
        {
            InitializeComponent();
            photosListBytes = new ArrayList();
            photosList = photosArray;
            foreach (CameraSnapshotFile cameraSF in cameraArray)
            {
                byte[] passPhoto = cameraSF.Content;
                photosListBytes.Add(passPhoto);
            }
            this.cameraArray = cameraArray;
            currentPhotoIndex = selectedImage;
            pictureBoxSize = new Size();
            picutreName = pass.EmployeeName + "_" + pass.EventTime.ToString("dd_MM_yyyy_HH_mm");
            centerToForm();
        }

        public Snapshots(ArrayList photosArray, int selectedImage, ArrayList cameraArray, Hashtable cameraTable)
        {
            InitializeComponent();
            photosListBytes = new ArrayList();
            photosList = photosArray;
            foreach (CameraSnapshotFile cameraSF in cameraArray)
            {
                byte[] passPhoto = cameraSF.Content;
                photosListBytes.Add(passPhoto);
            }
            this.cameraArray = cameraArray;
            currentPhotoIndex = selectedImage;
            pictureBoxSize = new Size();
            picutreName = "";
            cameras = cameraTable;
            centerToForm();
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
                log.writeLog(DateTime.Now + " Snapshots.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Snapshots_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.CenterToScreen();
                displayPhotos();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
                pictureBoxSize = pictureBox1.Size;
                centerToForm();
                setLanguage();

            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Snapshots.Snapshots_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void displayPhotos()
        {
            try
            {
                trackBar1.Value = 1;
                trackBar1.Invalidate();
                Image image = (Image)photosList[currentPhotoIndex];
                this.pictureBox1.Image = image;
                this.pictureBox1.Size = panelSnapshot.Size;
                this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                this.pictureBox1.Invalidate();
                centerToForm();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Snapshots.displayPhotos(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void centerToForm()
        {
            //try
            //{
            //    Point centerLocation = new Point((12 + ((panelSnapshot.Width - pictureBox1.Width) / 2)), (12 + ((panelSnapshot.Height - pictureBox1.Height) / 2)));
            //    pictureBox1.Location = centerLocation;
            //}
            //catch (Exception ex)
            //{
            //    log.writeLog(DateTime.Now + " Snapshots.centerToForm(): " + ex.Message + "\n");
            //    MessageBox.Show(ex.Message);
            //}
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
                log.writeLog(DateTime.Now + " Snapshots.btnPhotoPrev_Click(): " + ex.Message + "\n");
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

                if ((currentPhotoIndex + 1) < photosList.Count)
                {
                    currentPhotoIndex++;
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Snapshots.btnPhotoNext_Click(): " + ex.Message + "\n");
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

                if (currentPhotoIndex != (photosList.Count - 1))
                {
                    currentPhotoIndex = photosList.Count - 1;
                    displayPhotos();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Snapshots.btnPhotoLast_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Snapshots.btnPhotoFirst_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("snapshotsForm", culture);

                // group box text
                gbPhotoNavigation.Text = rm.GetString("gbPhotoNavigation", culture);
               

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
               
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Snapshots.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Size newSize = new Size(panelSnapshot.Width * trackBar1.Value, panelSnapshot.Height * trackBar1.Value);
                pictureBox1.Size = newSize;
                pictureBox1.Update();
                centerToForm();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Snapshots.trackBar1_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

       
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                byte[] image = (byte[])photosListBytes[currentPhotoIndex];
                MemoryStream memStream = new MemoryStream(image);
                Image img = new Bitmap(memStream);
                CameraSnapshotFile cameraSF = (CameraSnapshotFile)cameraArray[currentPhotoIndex];

                string path = "";
                if (picutreName.Equals(""))
                {
                    path += ((Camera)cameras[cameraSF.CameraID]).Description + "_" + cameraSF.FileCreatedTime.ToString("yyyy_MM_dd_HH_mm_ss");
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = path;
                    sfd.Filter = "JPG (*.jpg)|*.jpg|" +
                    "JPEG (*.jpeg)|*.jpeg|" +
                    "GIF (*.gif)|*.gif";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        sfd.AddExtension = true;
                        path = sfd.FileName;

                        try
                        {
                            img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                            MessageBox.Show(rm.GetString("cameraSnapShotSaved", culture));
                        }
                        catch (Exception ex)
                        {
                            log.writeLog(DateTime.Now + " Snapshots.btnSave_Click(): " + ex.Message + "\n");
                            MessageBox.Show(rm.GetString("cameraSnapShotNotSaved", culture));
                        }
                    }
                }

                if (!picutreName.Equals(""))
                {
                    FolderBrowserDialog fbDialog = new FolderBrowserDialog();

                    if (fbDialog.ShowDialog() == DialogResult.OK)
                    {

                        path = fbDialog.SelectedPath.ToString();


                        path += "\\" + picutreName + "_" + cameraSF.CameraID.ToString() + ".jpg";

                        try
                        {
                            img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                            MessageBox.Show(rm.GetString("cameraSnapShotSaved", culture));
                        }
                        catch (Exception ex)
                        {
                            log.writeLog(DateTime.Now + " Snapshots.btnSave_Click(): " + ex.Message + "\n");
                            MessageBox.Show(rm.GetString("cameraSnapShotNotSaved", culture));
                        }
                    }
                }
                memStream.Close();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Snapshots.btnSave_Click_1(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

       
        
    }
}