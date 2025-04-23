using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;
using System.IO;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class CameraSnapshots : Form
    {
        private ResourceManager rm;
        private CultureInfo culture;
        private DebugLog log;

        private int currentPhotoIndex = 0;
        private int photoCount = 0;
        private ArrayList photosList = null;
        private ArrayList photosForDisplay = null;

        private CameraSnapshotFile currentCameraSnapshotFile = null;

        private List<ReaderTO> readerArray = new List<ReaderTO>();
        private ArrayList cameraArray = new ArrayList();
        private ArrayList cameraSFList = null;

        //list's for camera detail's showing
        private Dictionary<int, List<ReaderTO>> readers;
        private Hashtable cameras;
        private Dictionary<int, List<GateTO>> gates;
        private ArrayList camerasXreaders;
        private Dictionary<int, List<LocationTO>> locations;
        private Hashtable directions;

        private Filter filter;

        public CameraSnapshots()
        {
            InitializeComponent();

            cameraSFList = new ArrayList();
            photosList = new ArrayList();
            photosForDisplay = new ArrayList();

            currentCameraSnapshotFile = new CameraSnapshotFile();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.CenterToScreen();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Cameras).Assembly);
            setLanguage();
            setTabels();
        }

        private void setTabels()
        {
            try
            {
                readers = new Dictionary<int,List<ReaderTO>>();
                cameras = new Hashtable();
                gates = new Dictionary<int,List<GateTO>>();
                locations = new Dictionary<int,List<LocationTO>>();
                directions = new Hashtable();

                //cameras X readers list
                camerasXreaders = new CamerasXReaders().Search(-1, -1, "");

                //set readers list
                List<ReaderTO> readersList = new Reader().SearchAll();

                //set gates list
                List<GateTO> gatesList = new Gate().Search();

                //set locations list
                List<LocationTO> locationsList = new Location().Search();

                //set cameras hashtable's
                ArrayList camerasList = new Camera().Search(-1, "", "", "");

                //find list's foreach camera in list and set table's
                foreach (Camera camera in camerasList)
                {
                    //add camera
                    if(!cameras.ContainsKey(camera.CameraID))
                    cameras.Add(camera.CameraID, camera);
                    //initialize list's
                    List<ReaderTO> camReaders = new List<ReaderTO>();
                    List<GateTO> camGates = new List<GateTO>();
                    List<LocationTO> camLocations = new List<LocationTO>();
                    ArrayList camDirections = new ArrayList();

                    foreach (CamerasXReaders cxr in camerasXreaders)
                    {
                        //find relted reader's
                        if (cxr.CameraID == camera.CameraID)
                        {
                            if (!camDirections.Contains(cxr.DirectionCovered))
                                camDirections.Add(cxr.DirectionCovered);
                            foreach (ReaderTO read in readersList)
                            {
                                if (read.ReaderID == cxr.ReaderID)
                                {
                                    if (!camReaders.Contains(read))
                                    camReaders.Add(read);

                                    //find related gate's
                                    foreach (GateTO gate in gatesList)
                                    {
                                        if (((gate.GateID == read.A0GateID || gate.GateID == read.A1GateID) && cxr.DirectionCovered.Equals(Constants.DirectionInOut))
                                            || (gate.GateID == read.A0GateID && read.A0Direction.Equals(cxr.DirectionCovered))
                                            || (gate.GateID == read.A1GateID) && read.A1Direction.Equals(cxr.DirectionCovered))
                                        {
                                            if (!camGates.Contains(gate))
                                            camGates.Add(gate);
                                        }
                                    }
                                    //find related locations's
                                    foreach (LocationTO loc in locationsList)
                                    {
                                        if (((loc.LocationID == read.A0LocID || loc.LocationID == read.A1LocID) && cxr.DirectionCovered.Equals(Constants.DirectionInOut))
                                            || (loc.LocationID == read.A0LocID && read.A0Direction.Equals(cxr.DirectionCovered))
                                            || (loc.LocationID == read.A1LocID) && read.A1Direction.Equals(cxr.DirectionCovered))
                                        {
                                            if (!camLocations.Contains(loc))
                                            camLocations.Add(loc);
                                        }
                                    }
                                }
                            }//foreach (Reader read in readersList)
                        }//if (cxr.CameraID == camera.CameraID)
                    }// foreach (CamerasXReaders cxr in camerasXreaders)

                    readers.Add(camera.CameraID, camReaders);
                    gates.Add(camera.CameraID, camGates);
                    locations.Add(camera.CameraID, camLocations);
                    directions.Add(camera.CameraID, camDirections);
                }//foreach (Camera camera in camerasList)       

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.setTabels(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate Direction Combo Box
        /// </summary>
        private void populateDirectionCombo()
        {
            try
            {
                cbDirection.Items.Add(rm.GetString("all", culture));
                cbDirection.Items.Add(Constants.DirectionIn);
                cbDirection.Items.Add(Constants.DirectionOut);
                cbDirection.Items.Add(Constants.DirectionInOut);

                cbDirection.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateDirectionCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populateGateCombo(int locID)
        {
            try
            {
                List<GateTO> gatesArray = new List<GateTO>();
                if (locID < 0)
                {
                    gatesArray = new Gate().Search();
                }
                else
                {
                    gatesArray = new Gate().SerchForLocation(locID);
                }

                GateTO gate = new GateTO();
                gate.Name = rm.GetString("all", culture);
                gatesArray.Insert(0, gate);

                this.cbGate.DataSource = gatesArray;
                this.cbGate.DisplayMember = "Name";
                this.cbGate.ValueMember = "GateID";
                this.cbGate.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.populateGateCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populateReaderCombo(int locID, int gateID)
        {
            try
            {
                readerArray = new List<ReaderTO>();
                if ((locID < 0) && (gateID < 0))
                {
                    readerArray = new Reader().SearchAll();
                }
                else
                {
                    readerArray = new Reader().getReaders(locID, gateID);
                }

                ReaderTO reader = new ReaderTO();
                reader.Description = rm.GetString("all", culture);
                readerArray.Insert(0, reader);

                this.cbReader.DataSource = readerArray;
                this.cbReader.DisplayMember = "Description";
                this.cbReader.ValueMember = "ReaderID";
                this.cbReader.Invalidate();
                populateCameraCb();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.populateGateCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populateLocationCb()
        {
            try
            {
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                List<LocationTO> locations = loc.Search();
                locations.Insert(0, new LocationTO(-1, rm.GetString("all", culture), "", -1, -1, Constants.DefaultStateActive.ToString()));

                cbLocation.DataSource = locations;
                cbLocation.DisplayMember = "Name";
                cbLocation.ValueMember = "LocationID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.populateLocationCb(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void CameraSnapshots_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateDirectionCombo();
                populateLocationCb();
                populateGateCombo(-1);
                populateReaderCombo(-1, -1);
                this.lblPhotoNum.Visible = false;

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.CameraSnapshots_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
        /// <summary>
        /// Populate List View with Access Groups found
        /// </summary>
        /// <param name="accessGroupsList"></param>
        private void populateCameraCb()
        {
            try
            {
                string readersString = "";
                if (cbReader.SelectedIndex > 0)
                    readersString = cbReader.SelectedValue.ToString();
                else
                {
                    foreach (ReaderTO reader in readerArray)
                    {
                        if (reader.ReaderID != -1)
                            readersString += reader.ReaderID.ToString() + ", ";
                    }
                    if (readersString.Length > 0)
                        readersString = readersString.Substring(0, readersString.Length - 2);
                }

                 string direction = "";
                if (cbDirection.SelectedIndex > 0)
                {
                    direction = cbDirection.SelectedItem.ToString();
                }

                cameraArray = new Camera().SearchForReaders(readersString, direction);
                cameraArray.Insert(0, new Camera(-1, "", rm.GetString("all", culture), ""));

                cbCamera.DataSource = cameraArray;
                cbCamera.DisplayMember = "Description";
                cbCamera.ValueMember = "CameraID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbLocation.SelectedIndex == 0)
                {
                    populateGateCombo(-1);
                    populateReaderCombo(-1, -1);
                }
                else
                {                    
                    populateGateCombo((int)cbLocation.SelectedIndex);
                    populateReaderCombo((int)cbLocation.SelectedIndex, -1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.cbLocation_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbGate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbGate.SelectedIndex == 0)
                {
                    if (cbLocation.SelectedIndex == 0)
                    {
                        populateReaderCombo(-1, -1);
                        
                    }
                    else
                    {
                        populateReaderCombo((int)cbLocation.SelectedIndex, -1);

                    }
                }
                else
                {                    
                    populateReaderCombo(-1, (int)cbGate.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.cbLocation_SelectedIndexChanged(): " + ex.Message + "\n");
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
                //Form text
                this.Text = rm.GetString("cameraSnapshots", culture);

                //label's text
                this.lblCamera.Text = rm.GetString("lblCamera", culture);
                lblDirection.Text = rm.GetString("lblPrimDirect", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblGate.Text = rm.GetString("lblGate", culture);
                this.lblLocation.Text = rm.GetString("lblLocation", culture);
                this.lblReader.Text = rm.GetString("lblReader", culture);
                this.lblTimeForm.Text = rm.GetString("lblFrom", culture);
                this.lblTimeTo.Text = rm.GetString("lblTo", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);

                this.lblPhotoCamera.Text = rm.GetString("lblCamera", culture);
                this.lblPhotoDirection.Text = rm.GetString("lblPrimDirect", culture);
                this.lblPhotoGate.Text = rm.GetString("lblGate", culture);
                this.lblPhotoLocation.Text = rm.GetString("lblLocation", culture);
                this.lblPhotoTerminal.Text = rm.GetString("lblReader", culture);
               

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.gbPhotoInfo.Text = rm.GetString("gbPhotoInfo", culture);

                //group box's text
                this.gbDate.Text = rm.GetString("gbDate", culture);
                this.gbPhotoNavigation.Text = rm.GetString("gbPhotoNavigation", culture);
                this.gbPhotos.Text = rm.GetString("gbPhotos", culture);
                this.gbSearch.Text = rm.GetString("gbSearch", culture);
                this.gbTime.Text = rm.GetString("gbTime", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " CameraSnapshots.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
                log.writeLog(DateTime.Now + " CameraSnapshots.btnPhotoFirst_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " CameraSnapshots.btnPhotoPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
                log.writeLog(DateTime.Now + " CameraSnapshots.btnPhotoNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
                log.writeLog(DateTime.Now + " CameraSnapshots.btnPhotoLast_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                        currentCameraSnapshotFile = (CameraSnapshotFile)photosForDisplay[currentPhotoIndex];
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

                    setPhotoInfoValues();
                }
                else
                {
                    lblPhotoNum.Text = "";
                    pb1.Image = null;
                    pb2.Image = null;
                    pb3.Image = null;
                    pb4.Image = null;
                    pb5.Image = null;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.displayPhotos(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setPhotoInfoValues()
        {
            try
            {
                int currentCamID = currentCameraSnapshotFile.CameraID;

                if (cameras.ContainsKey(currentCamID))
                    tbPhotoCamera.Text = ((Camera)cameras[currentCamID]).Description;

                //set location's for current picture
                if (locations.ContainsKey(currentCamID))
                {
                    List<LocationTO> locationsList = locations[currentCamID];
                    cbPhotoLocations.DataSource = locationsList;
                    cbPhotoLocations.DisplayMember = "Name";
                    cbPhotoLocations.Invalidate();
                }

                //set gate's for current picture
                if (gates.ContainsKey(currentCamID))
                {
                    List<GateTO> gatesList = gates[currentCamID];
                    cbPhotoGates.DataSource = gatesList;
                    cbPhotoGates.DisplayMember = "Name";
                    cbPhotoGates.Invalidate();
                }

                //set reader's for current picture
                if (readers.ContainsKey(currentCamID))
                {
                    List<ReaderTO> readersList = readers[currentCamID];
                    cbPhotoReaders.DataSource = readersList;
                    cbPhotoReaders.DisplayMember = "Description";
                    cbPhotoReaders.Invalidate();
                }

                //set direstion for currnet picture
                if (directions.ContainsKey(currentCamID))
                {
                    cbPhotoDirections.Items.Clear();
                    ArrayList direstionList = (ArrayList)directions[currentCamID];
                    foreach (string s in direstionList)
                    {
                        cbPhotoDirections.Items.Add(s);
                    }
                    cbPhotoDirections.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.setPhotoInfoValues(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                photosList.Clear();
                photosForDisplay.Clear();
                photoCount = 0;

                string cameraID = "";
                if (cameraArray.Count <= 1)
                {
                    MessageBox.Show(rm.GetString("noPhotosForCriteria", culture));
                    this.lblPhotoNum.Visible = false;
                    return;
                }
                if (cbCamera.SelectedIndex <= 0)
                {
                    foreach (Camera cam in cameraArray)
                    {
                        if (cam.CameraID != -1)
                            cameraID += cam.CameraID.ToString() + ", ";
                    }
                    if (!cameraID.Equals(""))
                        cameraID = cameraID.Substring(0, cameraID.Length - 2);
                }
                else
                {
                    cameraID = cbCamera.SelectedValue.ToString();
                }

                ArrayList photosArray = currentCameraSnapshotFile.SearchSnapshots(cameraID, dtpFrom.Value.Date, dtpTo.Value.Date, dtFrom.Value, dtTo.Value);
                if (photosArray.Count > 0)
                {
                    currentPhotoIndex = 0;

                    photosForDisplay = photosArray;

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

                } //if (photosForPass.Count > 0)

                if (photoCount == 0)
                {
                    MessageBox.Show(rm.GetString("noPhotosForCriteria", culture));
                    this.lblPhotoNum.Visible = false;
                }
                this.lblPhotoNum.Visible = true;
                displayPhotos();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
       
        private void cbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateCameraCb();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " CameraSnapshots.cbDirection_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbReader_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateCameraCb();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.cbReader_SelectedIndexChanged(): " + ex.Message + "\n");
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
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex,  cameraSFList, cameras);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.pb3_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex - 2,  cameraSFList, cameras);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.pb1_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex - 1,  cameraSFList, cameras);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.pb2_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex + 1, cameraSFList, cameras);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.pb4_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
                    Snapshots snapshots = new Snapshots(photosList, currentPhotoIndex + 2,  cameraSFList,cameras);
                    snapshots.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.pb5_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraSnapshots.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }        
    }
}