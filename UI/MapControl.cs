using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using UI;
using Common;
using Util;
namespace UI
{
    public partial class MapControl : UserControl
    {
        DebugLog log;

        private double ZOOMFACTOR = 1.25;	// = 25% smaller or larger
        private int MINMAX = 3;
        Size size;
        int activeObjectType = -1;
        const int cameraObjectType = 1;
        const int readerObjectType = 2;
        const int gateObjectType = 3;
        const int locationObjectType = 4;

        //location parametar's
        private System.Drawing.Drawing2D.GraphicsPath _pathData;
        private int _activeIndex = -1;
        private List<LocationTO> _pathsArray;
        private ArrayList _polygonPointsArray;
        private Graphics _graphics;
        private ArrayList _polygonPathsArray;

        //gate parametar's
        private System.Drawing.Drawing2D.GraphicsPath _gatePathData;
        private List<GateTO> _gatePathsArray;
        private ArrayList _gatePointsArray;
        private Image _gateImage;
        private  Size gateImageSize = new Size(20, 20);

        //reader parametar's
        private System.Drawing.Drawing2D.GraphicsPath _readerPathData;
        private List<ReaderTO> _readerPathsArray;
        private ArrayList _readerPointsArray;
        private Image _readerImage;
        private Size readerImageSize = new Size(20, 20);

        //camera parametar's
        private System.Drawing.Drawing2D.GraphicsPath _cameraPathData;
        private ArrayList _cameraPathsArray;
        private ArrayList _cameraPointsArray;
        private Image _cameraImage;
        private Size cameraImageSize = new Size(20, 20);

        private ToolTip _toolTip;

        //points defining
        private System.Drawing.Drawing2D.GraphicsPath _pointPathData;
        private ArrayList _pointPathsArray;
        
        private int prevPictureBoxWidth;
        private int prevPictureBoxHeight;
        private Point currentPoint;
        
        public int PrevPictureBoxHeight
        {
            get { return prevPictureBoxHeight; }
            set { prevPictureBoxHeight = value; }
        }
        public int PrevPictureBoxWidth
        {
            get { return prevPictureBoxWidth; }
            set { prevPictureBoxWidth = value; }
        }
        //public List<LocationTO> PathsArray
        //{
        //    get { return _pathsArray; }
        //    set { _pathsArray = value; }
        //}
        public delegate void RegionClickDelegate(LocationTO loc, Point p);
        [Category("Action")]
        public event RegionClickDelegate RegionClick;

        public delegate void PictureClickDelegate(Point imagePoint);
        [Category("Action")]
        public event PictureClickDelegate PictureClick;

        public delegate void RightClickDelegate(int objectID, string type);
        [Category("Action")]
        public event RightClickDelegate RightClick;

        const int readerModeIndex = 1;
        const int gateModeIndex = 2;
        const int cameraModeIndex = 3;
        const int locationModeIndex = 4;
        public int ModeIndex;

        private CultureInfo culture;
        private ResourceManager rm;

        public MapControl()
        {
            InitializeComponent();


            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(SecurityRoutesPointsAdd).Assembly);

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this._pathsArray = new List<LocationTO>();
            this._polygonPathsArray = new ArrayList();
            this._pathData = new System.Drawing.Drawing2D.GraphicsPath();
            this._pathData.FillMode = System.Drawing.Drawing2D.FillMode.Winding;
            this._graphics = Graphics.FromHwnd(this.pictureBox1.Handle);

            if (Directory.Exists(Constants.ObjectImagePath))
            {
                if (File.Exists(Constants.ObjectImagePath + "Gate.ico"))
                {
                    this._gateImage = Image.FromFile(Constants.ObjectImagePath + "Gate.ico");
                }
                if (File.Exists(Constants.ObjectImagePath + "citac.jpg"))
                {
                    this._readerImage = Image.FromFile(Constants.ObjectImagePath + "citac.jpg");
                }
                if (File.Exists(Constants.ObjectImagePath + "kamera.jpg"))
                {
                    this._cameraImage = Image.FromFile(Constants.ObjectImagePath + "kamera.jpg");
                }

            }

            this._readerPathsArray = new List<ReaderTO>();
            this._readerPathData = new System.Drawing.Drawing2D.GraphicsPath();
            this._readerPointsArray = new ArrayList();

            this._gatePathsArray = new List<GateTO>();
            this._gatePathData = new System.Drawing.Drawing2D.GraphicsPath();
            this._gatePointsArray = new ArrayList();

            this._cameraPathsArray = new ArrayList();
            this._cameraPathData = new System.Drawing.Drawing2D.GraphicsPath();
            this._cameraPointsArray = new ArrayList();

            this._pointPathsArray = new ArrayList();
            this._pointPathData = new System.Drawing.Drawing2D.GraphicsPath();

            this.components = new Container();
            this._toolTip = new ToolTip(this.components);
            this._toolTip.AutoPopDelay = 5000;
            this._toolTip.InitialDelay = 1000;
            this._toolTip.ReshowDelay = 500;

            currentPoint = new Point();
            
        }

        public Image Image
        {
            get
            {
                return this.pictureBox1.Image;
            }
            set
            {
                this.pictureBox1.Image = value;
                if (pictureBox1.Image != null)
                {
                    prevPictureBoxHeight = this.pictureBox1.Image.Height;
                    prevPictureBoxWidth = this.pictureBox1.Image.Width;
                }
            }
        }

        public int AddLocation(LocationTO l, Point[] points)
        {
            try
            {
                if (this._pathsArray.Count > 0)
                    this._pathData.SetMarkers();
               
                int parentIndex = getLocIndex(points[0]);
                bool hasParent = false;
                if (parentIndex != -1)
                {
                    hasParent = true;
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddPolygon((Point[]) _polygonPathsArray[parentIndex]);
                    for (int i = 1; i < points.Length; i++)
                    {
                        hasParent = hasParent && path.IsVisible(points[i]);
                    }
                }
                if (!hasParent)
                {
                    this._pathData.AddPolygon(points);
                    this._pathsArray.Add(l);
                    return this._polygonPathsArray.Add(points);
                }
                else
                {
                    List<LocationTO> tempList = new List<LocationTO>();
                    addLocPathOnPosition(parentIndex, points);
                    for (int i = 0; i < parentIndex; i++)
                    {
                        tempList.Add(_pathsArray[i]);
                    }
                    tempList.Add(l);
                    for (int i = parentIndex; i < _pathsArray.Count; i++)
                    {
                        tempList.Add(_pathsArray[i]);
                    }
                    _pathsArray = tempList;
                }
                return 1;                
            }
            catch
            {
                throw new Exception();
            }
        }

        public int AddPoint(Point[] points, Point point)
        {
            try
            {
                if (this._pointPathsArray.Count > 0)
                    this._pointPathData.SetMarkers();

                this._pointPathData.AddPolygon(points);
                if (_pointPathsArray.Count > 0)
                {
                    this._pointPathData.AddLine((Point)_pointPathsArray[_pointPathsArray.Count - 1], point);
                }
                return this._pointPathsArray.Add(point);
            }
            catch
            {
                throw new Exception();
            }
        }

        public int AddReader(ReaderTO reader, Point point)
        {
            try
            {
                if (this._readerPathsArray.Count > 0)
                    this._readerPathData.SetMarkers();

                Point p = new Point();
                if (point != new Point())
                {
                    p.X = point.X - readerImageSize.Width / 2;
                    p.Y = point.Y - readerImageSize.Height / 2;
                }
                else
                {
                    p.X = currentPoint.X - readerImageSize.Width / 2;
                    p.Y = currentPoint.Y - readerImageSize.Height / 2;
                }
                this._readerPathData.AddRectangle(new Rectangle(p,readerImageSize));
                this._readerPointsArray.Add(p);
                this._readerPathsArray.Add(reader);
                return _readerPathsArray.Count - 1; // index of added element (same ass ArrayList Add method return value)
            }
            catch
            {
                throw new Exception();
            }
        }
        public int AddCamera(Camera camera, Point point)
        {
            try
            {
                if (this._cameraPathsArray.Count > 0)
                    this._cameraPathData.SetMarkers();

                Point p = new Point();
                if (point != new Point())
                {
                    p.X = point.X - cameraImageSize.Width / 2;
                    p.Y = point.Y - cameraImageSize.Height / 2;
                }
                else
                {
                    p.X = currentPoint.X - cameraImageSize.Width / 2;
                    p.Y = currentPoint.Y - cameraImageSize.Height / 2;
                }

                this._cameraPathData.AddRectangle(new Rectangle(p, cameraImageSize));
                
                this._cameraPointsArray.Add(p);
                return this._cameraPathsArray.Add(camera);
            }
            catch
            {
                throw new Exception();
            }
        }
        public int AddGate(GateTO gate, Point point)
        {
            try
            {
                if (this._gatePathsArray.Count > 0)
                    this._gatePathData.SetMarkers();

                Point p = new Point();
                if (point != new Point())
                {
                    p.X = point.X - gateImageSize.Width / 2;
                    p.Y = point.Y - gateImageSize.Height / 2;
                }
                else
                {
                    p.X = currentPoint.X - gateImageSize.Width / 2;
                    p.Y = currentPoint.Y - gateImageSize.Height / 2;
                }

                this._gatePathData.AddRectangle(new Rectangle(p, gateImageSize));

                this._gatePointsArray.Add(p);
                this._gatePathsArray.Add(gate);
                return _gatePathsArray.Count - 1;
            }
            catch
            {
                throw new Exception();
            }
        }

        public void drawPoints()
        {
            try
            {
                if (_pointPathData != null)
                {
                    Graphics g = pictureBox1.CreateGraphics();
                    g.DrawPath(Pens.Red, _pointPathData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void resetPoints()
        {
            try
            {
                if (_pointPathsArray.Count > 0)
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_pointPathData);
                    iterator.Rewind();
                    ArrayList points = new ArrayList();
                    for (int current = 0; current < _pointPathsArray.Count; current++)
                    {
                        Point point = (Point)_pointPathsArray[current];

                        point.X = Convert.ToInt32(pictureBox1.Width / (double)this.prevPictureBoxWidth * point.X);
                        point.Y = Convert.ToInt32(pictureBox1.Height / (double)this.prevPictureBoxHeight * point.Y);

                        Point[] poi = { new Point(point.X - 2, point.Y - 2), new Point(point.X + 2, point.Y - 2), new Point(point.X + 2, point.Y + 2), new Point(point.X - 2, point.Y + 2) };
                        path.AddPolygon(poi);
                        if (points.Count > 0)
                        {
                            path.AddLine((Point)points[points.Count - 1], point);
                        }
                        points.Add(point);

                    }
                    _pointPathData = path;
                    _pointPathsArray = points;
                    drawPoints();
                    pictureBox1.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void resetPolygons()
        {
            try
            {                
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_pathData);
                iterator.Rewind();
                ArrayList point = new ArrayList();
                for (int current = 0; current < iterator.SubpathCount; current++)
                {
                    Point[] points = (Point[])_polygonPathsArray[current];

                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i].X = Convert.ToInt32(pictureBox1.Width / (double)this.prevPictureBoxWidth * points[i].X);
                        points[i].Y = Convert.ToInt32(pictureBox1.Height / (double)this.prevPictureBoxHeight * points[i].Y);
                    }
                    if (point.Count > 0)
                        path.SetMarkers();
                    path.AddPolygon(points);
                    point.Add(points);
                }
                _pathData = path;
                _polygonPathsArray = point;
                size = new Size(this.pictureBox1.Width / 15, this.pictureBox1.Height / 15);
                resetCameras();
                resetGates();
                resetReders(); pictureBox1.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void addLocPathOnPosition(int pathPosition,Point[] pointLoc)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_pathData);
                iterator.Rewind();
                ArrayList point = new ArrayList();
                for (int current = 0; current < pathPosition; current++)
                {
                    Point[] points = (Point[])_polygonPathsArray[current];

                    if (point.Count > 0)
                        path.SetMarkers();
                    path.AddPolygon(points);
                    point.Add(points);
                }
                if (point.Count > 0)
                    path.SetMarkers();
                path.AddPolygon(pointLoc);
                point.Add(pointLoc);
                for (int current = pathPosition; current < iterator.SubpathCount; current++)
                {
                    Point[] points = (Point[])_polygonPathsArray[current];

                    if (point.Count > 0)
                        path.SetMarkers();
                    path.AddPolygon(points);
                    point.Add(points);
                }
                _pathData = path;
                _polygonPathsArray = point;
                //size = new Size(this.pictureBox1.Width / 15, this.pictureBox1.Height / 15);
               // resetCameras();
               // resetGates();
               // resetReders();
                //pictureBox1.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void resetGates()
        {
            try
            {
                if (prevPictureBoxWidth != 0 || prevPictureBoxHeight != 0)
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_gatePathData);
                    iterator.Rewind();
                    ArrayList point = new ArrayList();
                    for (int current = 0; current < iterator.SubpathCount; current++)
                    {
                        Point p = (Point)_gatePointsArray[current];

                        p.X = Convert.ToInt32(pictureBox1.Width / (double)this.prevPictureBoxWidth * p.X);
                        p.Y = Convert.ToInt32(pictureBox1.Height / (double)this.prevPictureBoxHeight * p.Y);

                        if (point.Count > 0)
                            path.SetMarkers();
                        path.AddRectangle(new Rectangle(p, gateImageSize));
                        point.Add(p);
                    }
                    _gatePathData = path;
                    _gatePointsArray = point;
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void resetReders()
        {
            try
            {
                if (prevPictureBoxWidth != 0 || prevPictureBoxHeight != 0)
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_readerPathData);
                    iterator.Rewind();
                    ArrayList point = new ArrayList();
                    for (int current = 0; current < iterator.SubpathCount; current++)
                    {
                        Point p = (Point)_readerPointsArray[current];


                        p.X = Convert.ToInt32(pictureBox1.Width / (double)this.prevPictureBoxWidth * p.X);
                        p.Y = Convert.ToInt32(pictureBox1.Height / (double)this.prevPictureBoxHeight * p.Y);

                        if (point.Count > 0)
                            path.SetMarkers();
                        path.AddRectangle(new Rectangle(p, readerImageSize));
                        point.Add(p);
                    }
                    _readerPathData = path;
                    _readerPointsArray = point;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void resetCameras()
        {
            try
            {
                if (prevPictureBoxWidth != 0 || prevPictureBoxHeight != 0)
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_cameraPathData);
                    iterator.Rewind();
                    ArrayList point = new ArrayList();
                    for (int current = 0; current < iterator.SubpathCount; current++)
                    {
                        Point p = (Point)_cameraPointsArray[current];

                        p.X = Convert.ToInt32(pictureBox1.Width / (double)this.prevPictureBoxWidth * p.X);
                        p.Y = Convert.ToInt32(pictureBox1.Height / (double)this.prevPictureBoxHeight * p.Y);


                        if (point.Count > 0)
                            path.SetMarkers();
                        path.AddRectangle(new Rectangle(p, cameraImageSize));
                        point.Add(p);
                    }
                    _cameraPathData = path;
                    _cameraPointsArray = point;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void deleteAllPolygons()
        {
            try
            {
                _pathData = new System.Drawing.Drawing2D.GraphicsPath();
                _pathsArray = new List<LocationTO>();
                _polygonPathsArray = new ArrayList();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void deleteAllRedaers()
        {
            try
            {
                _readerPathData = new System.Drawing.Drawing2D.GraphicsPath();
                 _readerPathsArray = new List<ReaderTO>();
                _readerPointsArray = new ArrayList();                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void deleteAllCameras()
        {
            try
            {
                _cameraPathData = new System.Drawing.Drawing2D.GraphicsPath();
                _cameraPathsArray = new ArrayList();
                _cameraPointsArray = new ArrayList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void deleteAllGates()
        {
            try
            {
                _gatePathData = new System.Drawing.Drawing2D.GraphicsPath();
                _gatePathsArray = new List<GateTO>();
                _gatePointsArray = new ArrayList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void clearControl()
        {
            try
            {
                deleteAllPoints();
                deleteAllPolygons();
                deleteAllRedaers();
                deleteAllGates();
                deleteAllCameras();
                _activeIndex = -1;
                activeObjectType = -1;
                pictureBox1.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void deleteLastPolygon()
        {
            try
            {
                if (_pointPathsArray.Count > 0)
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_pathData);
                    iterator.Rewind();
                    ArrayList polygons = new ArrayList();
                    ArrayList pointsList = new ArrayList();
                    for (int current = 0; current < iterator.SubpathCount - 1; current++)
                    {
                        polygons.Add(_pointPathsArray[current]);

                        Point[] points = (Point[])_polygonPointsArray[current];
                        path.AddPolygon(points);
                        pointsList.Add(points);
                    }
                    _pathData = path;
                    _polygonPointsArray = polygons;
                    _polygonPathsArray = pointsList;
                    pictureBox1.Refresh();
                    pictureBox1.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void deleteAllPoints()
        {
            try
            {
                _pointPathData = new System.Drawing.Drawing2D.GraphicsPath();
                _pointPathsArray = new ArrayList();
                pictureBox1.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void deleteLastPoint()
        {
            try
            {
                if (_pointPathsArray.Count > 0)
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_pointPathData);
                    iterator.Rewind();
                    ArrayList points = new ArrayList();
                    for (int current = 0; current < _pointPathsArray.Count - 1; current++)
                    {
                        Point point = (Point)_pointPathsArray[current];

                        Point[] poi = { new Point(point.X - 2, point.Y - 2), new Point(point.X + 2, point.Y - 2), new Point(point.X + 2, point.Y + 2), new Point(point.X - 2, point.Y + 2) };

                        if (points.Count > 0)
                        {
                            path.AddLine((Point)points[points.Count - 1], point);
                        }
                        path.AddPolygon(poi);
                        points.Add(point);
                    }
                    _pointPathData = path;
                    _pointPathsArray = points;
                    drawPoints();
                    pictureBox1.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public void setLocation(LocationTO loc)
        {
            try
            {
                if (_pointPathsArray.Count > 2)
                {
                    Point[] points = new Point[_pointPathsArray.Count];
                    for (int i = 0; i < _pointPathsArray.Count; i++)
                    {
                        points[i] = (Point)_pointPathsArray[i];
                    }
                    this.AddLocation(loc, points);
                    this.deleteAllPoints();
                    pictureBox1.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public void drawPolygons()
        //{
        //    try
        //    {
        //        Graphics g = pictureBox1.CreateGraphics();
        //        SolidBrush brush = new SolidBrush(Color.FromArgb(100, Color.Red));

        //        g.DrawPath(Pens.Red, _pathData);
        //        this._pathData.FillMode = System.Drawing.Drawing2D.FillMode.Winding;
        //        g.FillPath(brush, _pathData);
        //        Brush gateBrush = new TextureBrush(_gateImage);
        //        g.FillPath(gateBrush, _gatePathData);
        //        Brush readerBrush = new TextureBrush(_readerImage);
        //        g.FillPath(readerBrush, _readerPathData);
        //        Brush cameraBrush = new TextureBrush(_cameraImage);
        //        g.FillPath(cameraBrush, _cameraPathData);
        //        pictureBox1.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        private int getActiveIndexAtPoint(Point point)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_cameraPathData);
                iterator.Rewind();
                for (int current = 0; current < iterator.SubpathCount; current++)
                {
                    iterator.NextMarker(path);
                    if (path.IsVisible(point, _graphics))
                    {
                        activeObjectType = cameraObjectType;
                        return current;
                    }
                }
                path = new System.Drawing.Drawing2D.GraphicsPath();
                iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_readerPathData);
                iterator.Rewind();
                for (int current = 0; current < iterator.SubpathCount; current++)
                {
                    iterator.NextMarker(path);
                    if (path.IsVisible(point, _graphics))
                    {
                        activeObjectType = readerObjectType;
                        return current;
                    }
                }
                path = new System.Drawing.Drawing2D.GraphicsPath();
                iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_gatePathData);
                iterator.Rewind();
                for (int current = 0; current < iterator.SubpathCount; current++)
                {
                    iterator.NextMarker(path);
                    if (path.IsVisible(point, _graphics))
                    {
                        activeObjectType = gateObjectType;
                        return current;
                    }
                }
                 path = new System.Drawing.Drawing2D.GraphicsPath();
                iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_pathData);
                iterator.Rewind();
                for (int current = 0; current < iterator.SubpathCount; current++)
                {
                    iterator.NextMarker(path);
                    if (path.IsVisible(point, _graphics))
                    {
                        activeObjectType = locationObjectType;
                        return current;
                    }
                }
                return -1;
            }
            catch
            {
                throw new Exception();
            }
        }

        private int getLocIndex(Point p)
        {
            try
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_pathData);
                iterator.Rewind();
                for (int current = 0; current < iterator.SubpathCount; current++)
                {
                    iterator.NextMarker(path);
                    if (path.IsVisible(p, _graphics))
                        return current;
                }
                return -1;
            }
            catch
            {
                throw new Exception();
            }
        }

        
        #region Zooming Methods
        //private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    if (e.Delta < 0)
        //    {
        //        ZoomIn();
        //    }
        //    else
        //    {
        //        ZoomOut();
        //    }
        //    drawPoints();
        //}


        /// <summary>
        /// Make the PictureBox dimensions smaller to effect the Zoom.
        /// </summary>
        /// <remarks>Maximum 5 times bigger</remarks>
        public void ZoomIn()
        {
            if ((pictureBox1.Width < (MINMAX * pictureBox1.Image.Width)) &&
                (pictureBox1.Height < (MINMAX * pictureBox1.Image.Height)))
            {
                this.prevPictureBoxHeight = pictureBox1.Height;
                this.prevPictureBoxWidth = pictureBox1.Width;
                pictureBox1.Width = Convert.ToInt32(prevPictureBoxWidth * ZOOMFACTOR);
                pictureBox1.Height = Convert.ToInt32(prevPictureBoxHeight * ZOOMFACTOR);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                resetPoints();
                resetPolygons();

            }
        }

        /// Make the PictureBox dimensions larger to effect the Zoom.
        /// </summary>
        /// <remarks>Maximum 5 times bigger</remarks>
        public void ZoomOut()
        {
            if ((pictureBox1.Width > (pictureBox1.Image.Width / MINMAX)) &&
                (pictureBox1.Height > (pictureBox1.Image.Height / MINMAX)))
            {
                this.prevPictureBoxHeight = pictureBox1.Height;
                this.prevPictureBoxWidth = pictureBox1.Width;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Width = Convert.ToInt32(prevPictureBoxWidth / ZOOMFACTOR);
                pictureBox1.Height = Convert.ToInt32(prevPictureBoxHeight / ZOOMFACTOR);
                resetPoints();
                resetPolygons();
            }
        }
        #endregion

        
         

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Point p = new Point(e.X, e.Y);
                if (e.Button == MouseButtons.Left)
                {
                    switch (ModeIndex)
                    { 
                        case readerModeIndex:
                        case gateModeIndex:
                        case cameraModeIndex:
                            currentPoint = p;
                            this.PictureClick(this.toImagePoint(p));                            
                            pictureBox1.Refresh();
                            break;
                        case locationModeIndex:
                            Point[] poi = { new Point(e.X - 2, e.Y - 2), new Point(e.X + 2, e.Y - 2), new Point(e.X + 2, e.Y + 2), new Point(e.X - 2, e.Y + 2) };
                            this.AddPoint(poi, p);
                            this.PictureClick(this.toImagePoint(p));
                            drawPoints();
                            break;
                        default:
                            if (this._activeIndex == -1)
                                _activeIndex = this.getActiveIndexAtPoint(p);
                            if (this._activeIndex > -1 && this.RegionClick != null)
                            {
                                LocationTO l = _pathsArray[this._activeIndex];

                                this.RegionClick(l, p);
                            }
                            break;

                    }
                   
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public void resetSize()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }
        public Point toImagePoint(Point p)
        {
            Point point = new Point();
            try
            {
                int x = Convert.ToInt32((double)pictureBox1.Image.Width / pictureBox1.Width * p.X);
                int y = Convert.ToInt32((double)pictureBox1.Image.Height / pictureBox1.Height * p.Y);
                point = new Point(x, y);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return point;
        }
        public Point toPhysicalPoint(Point p)
        {
            Point point = new Point();
            try
            {
                int x = Convert.ToInt32(pictureBox1.Width / (double)pictureBox1.Image.Width * p.X);
                int y = Convert.ToInt32(pictureBox1.Height / (double)pictureBox1.Image.Height * p.Y);
                point = new Point(x, y);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return point;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    Graphics g = e.Graphics;
                    g.DrawPath(Pens.Red, _pathData);
                    g.DrawPath(Pens.Red, _pointPathData);
                    SolidBrush brush = new SolidBrush(Color.FromArgb(50, Color.Red));
                    //this._pathData.FillMode = System.Drawing.Drawing2D.FillMode.Winding;
                    //g.FillPath(brush, _pathData);
                    foreach (Point[] p in _polygonPathsArray)
                    {
                         g.FillPolygon(brush,p);
                    }
                    foreach (Point p in _readerPointsArray)
                    {
                        Rectangle r = new Rectangle(p,readerImageSize);
                        g.DrawImage(_readerImage, r);
                    }
                    foreach (Point p in _cameraPointsArray)
                    {
                        Rectangle r = new Rectangle(p, cameraImageSize);
                        g.DrawImage(_cameraImage, r);
                    }
                    foreach (Point p in _gatePointsArray)
                    {
                        Rectangle r = new Rectangle(p, gateImageSize);
                        g.DrawImage(_gateImage, r);
                    }
                   
                    brush = new SolidBrush(Color.FromArgb(70, Color.Red));
                    if (activeObjectType == locationObjectType && _activeIndex != -1)
                    {
                        e.Graphics.FillPolygon(brush, (Point[])_polygonPathsArray[_activeIndex]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                int newIndex = this.getActiveIndexAtPoint(new Point(e.X, e.Y));
                if (newIndex > -1)
                {
                    pictureBox1.Cursor = Cursors.Hand;
                    //if (this._activeIndex != newIndex)
                    //{
                        switch(activeObjectType)
                        {
                            case cameraObjectType:
                                Camera c = (Camera)this._cameraPathsArray[newIndex];
                                this._toolTip.SetToolTip(this.pictureBox1, c.Description);
                                break;
                            case readerObjectType:
                                ReaderTO r = this._readerPathsArray[newIndex];
                                this._toolTip.SetToolTip(this.pictureBox1, r.Description);
                                break;
                            case gateObjectType:
                                GateTO g = this._gatePathsArray[newIndex];
                                this._toolTip.SetToolTip(this.pictureBox1, g.Name);
                                break;
                            case locationObjectType:
                                LocationTO l = _pathsArray[newIndex];
                                this._toolTip.SetToolTip(this.pictureBox1, l.Name);                                
                            break;
                        }
                    //}
                }
                else
                {
                    pictureBox1.Cursor = Cursors.Default;
                    this._toolTip.RemoveAll();
                }
                //if (this._activeIndex != newIndex)
               // {
                    this._activeIndex = newIndex;
                    this.Refresh();
               // }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Focused == false)
            {
                pictureBox1.Focus();
            }
            drawPoints();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Point p = new Point(e.X, e.Y);
                if (e.Button == MouseButtons.Right)
                {
                    if (this._activeIndex != -1)
                    {
                        switch (activeObjectType)
                        {
                            case cameraObjectType:
                                Camera c = (Camera)this._cameraPathsArray[_activeIndex];
                                this.RightClick(c.CameraID, Constants.cameraObjectType);
                                break;
                            case readerObjectType:
                                ReaderTO r = this._readerPathsArray[_activeIndex];
                                this.RightClick(r.ReaderID, Constants.readerObjectType);
                                break;
                            case gateObjectType:
                                GateTO g = this._gatePathsArray[_activeIndex];
                                this.RightClick(g.GateID, Constants.gateObjectType);
                                break;
                            case locationObjectType:
                                LocationTO l = _pathsArray[_activeIndex];
                                this.RightClick(l.LocationID, Constants.locationObjectType);
                                break;
                        }
                    }
                    else
                    {
                        this.RightClick(-1, "");
                                
                    }                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
      
       
    }
}
