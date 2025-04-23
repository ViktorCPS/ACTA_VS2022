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

namespace UI
{
    public partial class MapObjects : Form
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
        NormalizedPoint currentPoint;
        Point[] locationPoints;

        ArrayList pointList = new ArrayList();

        const int readerModeIndex = 1;
        const int gateModeIndex = 2;
        const int cameraModeIndex = 3;
        const int locationModeIndex = 4;
        int ModeIndex;

        MapsObjectDtl mapsObjectDtl;
        MapsObjectHdr mapsObjectHdr;

        private int removeObjectID;
        private string removeObjectTtpe;

        private int zoomPrevValue;
        private int mapPrevSelction;

        List<ApplRoleTO> userRoles;
        int mode = -1;

        public MapObjects()
        {
            InitializeComponent();           
        }

        public MapObjects(string objType)
        {
            InitializeComponent();
            switch (objType)
            { 
                case Constants.locationObjectType:
                    mode = locationModeIndex;
                    break;
                case Constants.readerObjectType:
                    mode = readerModeIndex;
                    break;
                case Constants.gateObjectType:
                    mode = gateModeIndex;
                    break;
                case Constants.cameraObjectType:
                    mode = cameraModeIndex;
                    break;                    
            }            
        }

        private void setLanguage()
        {
            try
            { 
                //form text
                this.Text = rm.GetString("mapObjectsMaintenance", culture);

                //label's text
                this.lblMap.Text = rm.GetString("lblMap", culture);
                this.lblMode.Text = rm.GetString("lblMode", culture);
                this.lblObjectType.Text = rm.GetString("lblObjectType", culture);
                this.lblPosition.Text = rm.GetString("lblPosition", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnClearLast.Text = rm.GetString("btnClearLast", culture);

                //menu items text
                this.cmsRemove.Items[0].Text = rm.GetString("menuRemove", culture);

                // list view initialization
                this.lvObjects.BeginUpdate();
                lvObjects.Columns.Add(rm.GetString("hdrName", culture), (lvObjects.Width - 20) / 2, HorizontalAlignment.Left);
                lvObjects.Columns.Add(rm.GetString("hdrDescription", culture), (lvObjects.Width - 20) / 2, HorizontalAlignment.Left);
                lvObjects.EndUpdate();

                this.lvPoints.BeginUpdate();
                lvPoints.Columns.Add("X", (lvObjects.Width - 4) / 2, HorizontalAlignment.Left);
                lvPoints.Columns.Add("Y", (lvObjects.Width - 4) / 2, HorizontalAlignment.Left);
                lvPoints.EndUpdate();                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateMapCombo()
        {
            try
            {
                mapArray = new ArrayList();

                mapArray = new Map().Search(-1, -1, "", "");

                Map map = new Map();
                map.Name = rm.GetString("noImage",culture);
                mapArray.Insert(0, map);

                cbMap.DataSource = mapArray;
                cbMap.DisplayMember = "Name";
                cbMap.ValueMember = "MapID";
                cbMap.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.populateMapCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateModeCombo()
        {
            try
            {
                cbMode.Items.Add(rm.GetString("addReader", culture));
                cbMode.Items.Add(rm.GetString("addGateMode", culture));
                cbMode.Items.Add(rm.GetString("addCameraMode", culture));
                cbMode.Items.Add(rm.GetString("addLocationMode", culture));

                cbMode.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.populateModeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if ((int)this.cbMap.SelectedIndex != 0)
                {
                    if (mode == -1)
                    {
                        cbMode.Enabled = true;
                    }
                    zoomPrevValue = 0;
                    trackBar1.Enabled = true;
                    foreach (Map map in mapArray)
                    {
                        if (map.MapID == (int)this.cbMap.SelectedValue)
                            currentMap = map;
                    }
                    mapControl1.clearControl();
                    byte[] mapPhoto = currentMap.Content;
                    if (currentMap.Content.Length > 0)
                    {
                        this.trackBar1.Value = 0;
                        MemoryStream memStream = new MemoryStream(mapPhoto);
                        Image img = new Bitmap(memStream);
                        this.mapControl1.Image = img;
                        this.mapControl1.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

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
                            NormalizedPoint point = new NormalizedPoint(objectPoint.X, objectPoint.Y);
                            addObject(objectPoint.Type, point.getPhysicalPoint(mapControl1.pictureBox1.Width, mapControl1.pictureBox1.Height));
                        }
                        else
                        {
                            locationPoints = new Point[pointsList.Count];
                            for (int i = 0; i < pointsList.Count; i++)
                            {
                                MapsObjectDtl objectPoint = (MapsObjectDtl)pointsList[i];
                                NormalizedPoint point = new NormalizedPoint(objectPoint.X, objectPoint.Y);
                                locationPoints[i] = point.getPhysicalPoint(mapControl1.pictureBox1.Width, mapControl1.pictureBox1.Height);
                            }
                            mapControl1.AddLocation(currnetLocation, locationPoints);
                        }

                    }
                    mapPrevSelction = (int)this.cbMap.SelectedIndex;
                    populateObjectsList();
                    setLblObjectType();
                }
                else
                {
                    cbMap.SelectedIndex = mapPrevSelction;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.cbMap_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " MapObjects.LoadObject(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MapObjects.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (currentPoint == null)
                {
                    if (cbMap.SelectedIndex != 0)
                    {
                        ModeIndex = cbMode.SelectedIndex + 1;
                        mapControl1.ModeIndex = ModeIndex;
                        setLblObjectType();
                        populateObjectsList();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("savePreviusObject", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.cbMode_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setLblObjectType()
        {
            try
            {
                switch (ModeIndex)
                {
                    case readerModeIndex:
                        tbObjectType.Text = Constants.readerObjectType;
                        break;
                    case gateModeIndex:
                        tbObjectType.Text = Constants.gateObjectType;
                        break;
                    case cameraModeIndex:
                        tbObjectType.Text = Constants.cameraObjectType;
                        break;
                    case locationModeIndex:
                        tbObjectType.Text = Constants.locationObjectType;
                         break;
                    default:
                        lvObjects.Items.Clear();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.setLblObjectType(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateObjectsList()
        {
            try
            {
                switch (ModeIndex)
                {
                    case readerModeIndex:
                        populateReaderList();
                        break;
                    case gateModeIndex:
                        populateGateList();
                        break;
                    case cameraModeIndex:
                        populateCameraList();
                        break;
                    case locationModeIndex:
                        populateLocationList();
                        break;
                    default:
                        lvObjects.Items.Clear();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.populateObjectsList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateLocationList()
        {
            try
            {
                lvObjects.Visible = true;
                List<LocationTO> locationList = new Location().getLocationsForMap(-1);
                lvObjects.BeginUpdate();

                lvObjects.Items.Clear();

                if (locationList.Count > 0)
                {

                    for (int i = 0; i < locationList.Count; i++)
                    {
                        LocationTO location = locationList[i];
                        ListViewItem item = new ListViewItem();
                        item.Text = location.Name;
                        item.SubItems.Add(location.Description.Trim());
                        item.Tag = location.LocationID;

                        lvObjects.Items.Add(item);
                    }
                }

                lvObjects.EndUpdate();
                lvObjects.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.populateCameraList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateCameraList()
        {
            try
            {
                lvObjects.Visible = true;
                ArrayList cameraList = new Camera().getCamerasForMap(-1);
                lvObjects.BeginUpdate();

                lvObjects.Items.Clear();

                if (cameraList.Count > 0)
                {
                    for (int i = 0; i < cameraList.Count; i++)
                    {
                        Camera camera = (Camera)cameraList[i];
                        ListViewItem item = new ListViewItem();
                        item.Text = camera.Description;
                        item.Tag = camera.CameraID;

                        lvObjects.Items.Add(item);
                    }
                }

                lvObjects.EndUpdate();
                lvObjects.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.populateCameraList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshPointsList(NormalizedPoint point)
        {
            try
            {
                lvPoints.BeginUpdate();
                lvPoints.Items.Clear();
                ListViewItem item = new ListViewItem();
                item.Text = point.X.ToString("0.000");
                item.SubItems.Add(point.Y.ToString("0.000"));
                lvPoints.Items.Add(item);
                lvPoints.EndUpdate();
                lvPoints.Invalidate();
                pointList.Add(point);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.refreshPointsList(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void addPointToList(NormalizedPoint point)
        {
            try
            {
                lvPoints.BeginUpdate();
                ListViewItem item = new ListViewItem();
                item.Text = point.X.ToString("0.000");
                item.SubItems.Add(point.Y.ToString("0.000"));
                lvPoints.Items.Add(item);
                lvPoints.EndUpdate();
                lvPoints.Invalidate();
                pointList.Add(point);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.refreshPointsList(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void clearPointsList()
        {
            try
            {
                lvPoints.BeginUpdate();
                lvPoints.Items.Clear();
                lvPoints.EndUpdate();
                pointList = new ArrayList();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.refreshPointsList(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void rightClickOnObject(int objectID, string type)
        {
            try
            {
                if (mode == -1||type.Equals(tbObjectType.Text.ToString()))
                {
                    removeObjectID = objectID;
                    removeObjectTtpe = type;
                    
                        cmsRemove.Items[0].Visible = true;
                        cmsRemove.Show(MousePosition.X,MousePosition.Y);
                    
                }               
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.rightClickOnObject(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void AddingObject(Point imagePoint)
        {
            try
            {
                if (cbMap.SelectedIndex != 0)
                {
                    if (ModeIndex.Equals(locationModeIndex))
                    {
                        addPointToList(new NormalizedPoint(imagePoint.X, imagePoint.Y, mapControl1.pictureBox1.Image.Width, mapControl1.pictureBox1.Image.Height));
                    }
                    else if (currentPoint == null)
                    {

                        currentPoint = new NormalizedPoint(imagePoint.X, imagePoint.Y, mapControl1.pictureBox1.Image.Width, mapControl1.pictureBox1.Image.Height);
                        refreshPointsList(currentPoint);

                        addObject(ModeIndex, imagePoint);

                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("savePreviusObject", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.populateCameraList(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void addObject(int index,Point imagePoint)
        {
            try
            {
                
                switch (index)
                {
                    case readerModeIndex:
                        mapControl1.AddReader(new ReaderTO(), new Point());
                        break;
                    case gateModeIndex:
                        mapControl1.AddGate(new GateTO(), new Point());
                        break;
                    case cameraModeIndex:
                        mapControl1.AddCamera(new Camera(), new Point());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.addObject(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " MapObjects.addObject(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateGateList()
        {
            try
            {
                lvObjects.Visible = true;
                List<GateTO> gateList = new Gate().getGatesForMap(-1);
                lvObjects.BeginUpdate();

                lvObjects.Items.Clear();

                if (gateList.Count > 0)
                {
                    for (int i = 0; i < gateList.Count; i++)
                    {
                        GateTO gate = gateList[i];
                        ListViewItem item = new ListViewItem();
                        item.Text = gate.Name;
                        item.SubItems.Add(gate.Description.Trim());
                        item.Tag = gate.GateID;

                        lvObjects.Items.Add(item);
                    }
                }

                lvObjects.EndUpdate();
                lvObjects.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.populateGateList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateReaderList()
        {
            try
            {
                lvObjects.Visible = true;
                List<ReaderTO> readerList = new Reader().getReadersForMap(-1);
                lvObjects.BeginUpdate();

                lvObjects.Items.Clear();

                if (readerList.Count > 0)
                {

                    for (int i = 0; i < readerList.Count; i++)
                    {
                        ReaderTO reader = readerList[i];
                        ListViewItem item = new ListViewItem();
                        item.Text = reader.Description;
                        item.Tag = reader.ReaderID;

                        lvObjects.Items.Add(item);
                    }
                }

                lvObjects.EndUpdate();
                lvObjects.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.populateReaderList(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        #region inner class
        /// <summary>
    /// Seat coordinates on map are stored as values between 0 and 1,
    /// normalized to total width/height.
    /// </summary>
        public class NormalizedPoint
        {
            double x;
            double y;

            public NormalizedPoint(double x, double y)
            {
                this.x = x;
                this.y = y;
            }

            public NormalizedPoint(int physicalX, int physicalY, int width, int height)
            {
                x = physicalX / (double)width;
                y = physicalY / (double)height;
            }

            public Point getPhysicalPoint(int physicalWidth, int physicalHeight)
            {
                return new Point(getPhysicalX(physicalWidth),
                    getPhysicalY(physicalHeight));
            }

            public int getPhysicalX(int physicalWidth)
            {
                return (int)(physicalWidth * x);
            }

            public int getPhysicalY(int physicalHeight)
            {
                return (int)(physicalHeight * y);
            }

            public double X
            {
                get { return x; }
                set { x = value; }
            }

            public double Y
            {
                get { return y; }
                set { y = value; }
            }
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            
                if (lvObjects.SelectedItems.Count > 0)
                {
                    if (((lvPoints.Items.Count > 0)&&(ModeIndex!=locationModeIndex))||((ModeIndex == locationModeIndex)&&(lvPoints.Items.Count>2)))
                    {
                        mapsObjectHdr = new MapsObjectHdr();
                        mapsObjectHdr.MapID = int.Parse(cbMap.SelectedValue.ToString());
                        mapsObjectHdr.ObjectID = int.Parse(lvObjects.SelectedItems[0].Tag.ToString());
                        mapsObjectHdr.Type = tbObjectType.Text.ToString();
                        int pointOrder = 0;
                        foreach (NormalizedPoint p in pointList)
                        {
                            mapsObjectDtl = new MapsObjectDtl();

                            mapsObjectDtl.X = p.X;
                            mapsObjectDtl.Y = p.Y;
                            mapsObjectDtl.ObjectID = int.Parse(lvObjects.SelectedItems[0].Tag.ToString());
                            mapsObjectDtl.Type = tbObjectType.Text.ToString();
                            mapsObjectDtl.PointOrder = pointOrder;

                            mapsObjectHdr.Points.Add(pointOrder, mapsObjectDtl);

                            pointOrder++;
                        }

                        int saved = mapsObjectHdr.Save();
                        if (saved > 0)
                        {
                            MessageBox.Show(rm.GetString("objectSaved", culture));
                            clearPointsList();
                            populateObjectsList();
                            if (ModeIndex.Equals(locationModeIndex))
                            {
                                mapControl1.setLocation(currnetLocation);
                            }
                            currentPoint = null;
                            this.cbMap_SelectedIndexChanged(this, new EventArgs());
                       
                    }
                    else
                    {

                        MessageBox.Show(rm.GetString("objectNotSaved", culture));
                        currentPoint = null;
                        this.cbMap_SelectedIndexChanged(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("selectPoint", culture));
                }
                }//if (lvObjects.SelectedItems.Count == 0)
                else
                {
                    MessageBox.Show(rm.GetString("selectObjectinList", culture));
                }
             
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvObjects.SelectedItems.Count > 0)
                {
                    LoadObject(tbObjectType.Text.ToString(), int.Parse(lvObjects.SelectedItems[0].Tag.ToString()));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.lvObjects_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool removed = mapsObjectHdr.Remove(removeObjectID, removeObjectTtpe);
                this.cbMap_SelectedIndexChanged(this, new EventArgs());
                this.cbMode_SelectedIndexChanged(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.lvObjects_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " MapObjects.trackBar1_Scroll(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClearLast_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvPoints.Items.Count == 1)
                {
                    clearPointsList();
                    this.cbMap_SelectedIndexChanged(this, new EventArgs());
                    currentPoint = null;
                }
                if (lvPoints.Items.Count > 1)
                {
                    mapControl1.deleteLastPoint();

                    lvPoints.Items.RemoveAt(lvPoints.Items.Count - 1);
                    pointList.RemoveAt(pointList.Count - 1);
                    currentPoint = new NormalizedPoint(mapControl1.pictureBox1.Image.Width, mapControl1.pictureBox1.Image.Height);
                    currentPoint.X = double.Parse(lvPoints.Items[lvPoints.Items.Count - 1].Text);
                    currentPoint.Y = double.Parse(lvPoints.Items[lvPoints.Items.Count - 1].SubItems[1].Text);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapObjects.btnClearLast_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
            }
        }

        private void cmsRemove_MouseLeave(object sender, EventArgs e)
        {
            cmsRemove.Visible = false;
        }

        private void MapObjects_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                userRoles = NotificationController.GetCurrentRoles();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                currentMap = new Map();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(EmployeeAbsences).Assembly);
                setLanguage();
                this.CenterToScreen();
                populateMapCombo();
                populateModeCombo();
                if (mode != -1)
                {
                    cbMode.SelectedIndex = mode - 1;
                    ModeIndex = mode;
                }
                else
                {
                    ModeIndex = 1;
                }
                tbObjectType.Enabled = false;

                currentCamera = new Camera();
                currentGate = new GateTO();
                currentReader = new ReaderTO();
                currnetLocation = new LocationTO();
                locationPoints = new Point[10];
                mapControl1.ModeIndex = 1;

                mapsObjectHdr = new MapsObjectHdr();
                mapsObjectDtl = new MapsObjectDtl();

                removeObjectID = -1;
                removeObjectTtpe = "";

                zoomPrevValue = trackBar1.Value;
                mapPrevSelction = 0;

                mapControl1.Image = Image.FromFile(Constants.ObjectImagePath + "SelectMap.PNG");
                mapControl1.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                mapControl1.pictureBox1.Size = mapControl1.Size;

                cbMode.Enabled = false;
                trackBar1.Enabled = false;

                bool adminRole = false;
                foreach (ApplRoleTO applRole in userRoles)
                {
                    if (applRole.ApplRoleID == Constants.ADMINRoleID)
                    {
                        adminRole = true;
                        break;
                    }

                }
                if (!adminRole)
                {
                    MessageBox.Show(rm.GetString("roleNotADMIN", culture));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "MapObjects.MapObjects_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void MapObjects_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + "MapObjects.MapObjects_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}