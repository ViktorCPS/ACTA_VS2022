using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Configuration;

using TransferObjects;
using Common;
using Util;

using System.Resources;
using System.Globalization;
using System.IO;
using ACTAMonitorLib;


namespace UI
{
    public partial class MapView : Form
    {
        DebugLog log;
        private CultureInfo culture;
        private ResourceManager rm;
        private Map currentMap;
        ArrayList mapArray;

        GateTO currentGate;
        ReaderTO currentReader;
        Camera currentCamera;
        LocationTO currnetLocation;
        Point[] locationPoints;

        MapsObjectDtl mapsObjectDtl;
        MapsObjectHdr mapsObjectHdr;

        private int zoomPrevValue;
        private int selectedObject;

        public MapView()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            currentMap = new Map();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeeAbsences).Assembly);
            setLanguage();
            this.CenterToScreen();
            populateMapCombo();

            currentCamera = new Camera();
            currentGate = new GateTO();
            currentReader = new ReaderTO();
            currnetLocation = new LocationTO();
            locationPoints = new Point[10];

            mapsObjectHdr = new MapsObjectHdr();
            mapsObjectDtl = new MapsObjectDtl();
            zoomPrevValue = trackBar1.Value;
            selectedObject = 0;

            mapControl1.Image = Image.FromFile(Constants.ObjectImagePath + "SelectMap.PNG");
            mapControl1.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            mapControl1.pictureBox1.Size = mapControl1.Size;
        }

        private void populateMapCombo()
        {
            try
            {
                mapArray = new ArrayList();

                mapArray = new Map().Search(-1, -1, "", "");

                Map map = new Map();
                map.Name = rm.GetString("noImage", culture);
                mapArray.Insert(0, map);

                cbMap.DataSource = mapArray;
                cbMap.DisplayMember = "Name";
                cbMap.ValueMember = "MapID";
                cbMap.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.populateMapCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rightClickOnObject(int objectID, string type)
        {
            try
            {   contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;

                switch (type)
                {
                    case Constants.gateObjectType:
                        contextMenuStrip1.Items[0].Visible = true;
                        break;
                    case Constants.locationObjectType:
                        contextMenuStrip1.Items[1].Visible = true;
                        break;
                    case Constants.cameraObjectType:
                        contextMenuStrip1.Items[2].Visible = true;
                        break;
                }
                selectedObject = objectID;
                contextMenuStrip1.Show(new Point(MousePosition.X , MousePosition.Y ));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.rightClickOnObject(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void cbMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if ((int)this.cbMap.SelectedIndex != 0)
                {
                    foreach (Map map in mapArray)
                    {
                        if (map.MapID == (int)this.cbMap.SelectedValue)
                            currentMap = map;
                    }
                    zoomPrevValue = 0;
                    trackBar1.Enabled = true;
                    mapControl1.clearControl();
                    byte[] mapPhoto = currentMap.Content;
                    if (currentMap.Content.Length > 0)
                    {
                        this.trackBar1.Value = 0;
                        MemoryStream memStream = new MemoryStream(mapPhoto);
                        Image img = new Bitmap(memStream);
                        this.mapControl1.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                        this.mapControl1.Image = img;
                        memStream.Close();
                    }

                    ArrayList objectList = new ArrayList();
                    objectList = mapsObjectHdr.Search(currentMap.MapID);
                    foreach (MapsObjectHdr objectHdr in objectList)
                    {
                        ArrayList pointsList = mapsObjectHdr.SearchDetails(objectHdr.ObjectID, objectHdr.Type);
                        LoadObject(objectHdr.Type, objectHdr.ObjectID);
                        if (pointsList.Count == 1)
                        {
                            MapsObjectDtl objectPoint = (MapsObjectDtl)pointsList[0];
                            MapObjects.NormalizedPoint point = new MapObjects.NormalizedPoint(objectPoint.X, objectPoint.Y);
                            addObject(objectPoint.Type, point.getPhysicalPoint(mapControl1.pictureBox1.Width, mapControl1.pictureBox1.Height));
                        }
                        else
                        {
                            locationPoints = new Point[pointsList.Count];
                            for (int i = 0; i < pointsList.Count; i++)
                            {
                                MapsObjectDtl objectPoint = (MapsObjectDtl)pointsList[i];
                                MapObjects.NormalizedPoint point = new MapObjects.NormalizedPoint(objectPoint.X, objectPoint.Y);
                                locationPoints[i] = point.getPhysicalPoint(mapControl1.pictureBox1.Width, mapControl1.pictureBox1.Height);
                            }
                            mapControl1.AddLocation(currnetLocation, locationPoints);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.cbMap_SelectedIndexChanged(): " + ex.Message + "\n");
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
                //form text
                this.Text = rm.GetString("mapReview", culture);

                //label's text
                this.lblMap.Text = rm.GetString("lblMap", culture);
                this.lblCamera.Text = rm.GetString("labelCamera", culture);
                this.lblGate.Text = rm.GetString("labelGate", culture);
                this.lblReader.Text = rm.GetString("labelReader", culture);
                
                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);               
                
                //menu item's text
                this.contextMenuStrip1.Items[0].Text = rm.GetString("startMonitor",culture);
                this.contextMenuStrip1.Items[1].Text = rm.GetString("currentPresence",culture);
                this.contextMenuStrip1.Items[2].Text = rm.GetString("liveStream",culture);
               
                //groups box text
                this.gbLegend.Text = rm.GetString("gbLegend", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadObject(string type, int objectID)
        {
            try
            {
                switch (type)
                {
                    case Constants.readerObjectType:                        
                        currentReader = new Reader().find(objectID);
                        break;
                    case Constants.cameraObjectType:
                        currentCamera = new Camera();
                        currentCamera = (Camera)currentCamera.Search(objectID, "", "", "")[0];
                        break;
                    case Constants.gateObjectType:                        
                        currentGate = new Gate().Find(objectID);
                        break;
                    case Constants.locationObjectType:                        
                        currnetLocation = new Location().Find(objectID);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.LoadObject(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MapView.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void addObject(string index, Point imagePoint)
        {
            try
            {

                switch (index)
                {
                    case Constants.readerObjectType:
                        mapControl1.AddReader(currentReader, imagePoint);
                        break;
                    case Constants.gateObjectType:
                        mapControl1.AddGate(currentGate, imagePoint);
                        break;
                    case Constants.cameraObjectType:
                        mapControl1.AddCamera(currentCamera, imagePoint);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.addObject(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (trackBar1.Value > zoomPrevValue)
                {
                    trackBar1.Value = zoomPrevValue + 1;
                    mapControl1.ZoomIn();
                }
                else
                {
                    trackBar1.Value = zoomPrevValue - 1;
                    mapControl1.ZoomOut();
                }
                zoomPrevValue = trackBar1.Value;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.trackBar1_Scroll(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void gateStartMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ACTAMonitorLib.Monitor monitor = new ACTAMonitorLib.Monitor(selectedObject.ToString());
                monitor.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.gateStartMonitorToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cameraLiveStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Reports.Video_surveillance videoSurv = new Reports.Video_surveillance(selectedObject);
                videoSurv.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.locationCurrentPresenceToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void locationCurrentPresenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                EmployeeLocations employeeLoc = new EmployeeLocations(selectedObject);
               employeeLoc.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapView.locationCurrentPresenceToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void contextMenuStrip1_MouseLeave(object sender, EventArgs e)
        {
            contextMenuStrip1.Visible = false;
        }

        private void MapView_Load(object sender, EventArgs e)
        {
            trackBar1.Enabled = false;
        }

        private void MapView_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " MapView.MapView_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        

        
        
    }
}