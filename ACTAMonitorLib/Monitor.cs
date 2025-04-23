using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;
using ACTAConfigManipulation;

namespace ACTAMonitorLib
{
	/// <summary>
	/// Class Monitor displays a card owner information when the card owner enters/exits a gate. 
	/// This information is displayed in the appropriate reader antenna control placed on the UI form.
    /// It is also responsible for launching of employees and tags reloading from the database 
    /// (including employees photos).
	/// </summary>
	public class Monitor : System.Windows.Forms.Form
	{
        private static Monitor _instance = null;
        private IContainer components;

        private Dictionary<string, CardReaderMonitor> gateCardReaderMonitors = new Dictionary<string, CardReaderMonitor>();

		private NotificationController notificationController;
		private NotificationObserverClient notificationObserverClient;

        private string[] gates;
        private List<EmployeeTO> employees = new List<EmployeeTO>();
        private List<TagTO> tags = new List<TagTO>();

		private List<ReaderTO> allReaders = new List<ReaderTO>();
        private Dictionary<string, List<ReaderTO>> gateReaders = new Dictionary<string, List<ReaderTO>>();
        private Dictionary<int, string> readerCameras = new Dictionary<int, string>();
        private ArrayList tagsForSnapshots = new ArrayList();

		private ArrayList enterControlsList = new ArrayList();
		private int enterContInUse = 0;
		
		private ArrayList exitControlsList = new ArrayList();
		private int exitContInUse = 0;

		private ArrayList controlsList = new ArrayList();

		private System.Windows.Forms.GroupBox gbEnter;
		private System.Windows.Forms.GroupBox gbExit;
		private System.Windows.Forms.StatusBar statusBar;

		private DebugLog log = null;

        private Dictionary<string, DateTime> gateDownloadStartTimes = new Dictionary<string, DateTime>();

		// Monitor form antennas layout constants (in % of appropriate antenna size)
		private const int antennasLeftMargin = 5;
		private const int antennasRightMargin = 5;
		private const int antennasTopMargin = 10;
		private const int antennasBottomMargin = 5;
		private const int antennasXDistance = 5;
		private const int antennasYDistance = 5;

		// Antenna control layout constants (in % of appropriate employee image size)
		private const int imageLeftMargin = 5;
		private const int imageRightMargin = 5;
		private const int imageTopMargin = 5;
		private const int imageBottomMargin = 60;	// in pixels, must be greater than panelEmployee height

        private const double imageProportionTolerance = 5.0; // in % of original employee image proportion
		private const double imageWidthIncrement = 5.0; // % step increment of image width 

        private const int readerLabelHeight = 23; // in pixels
        private const int readerLabelWidth = 100;

        private System.Windows.Forms.Timer timer1;
        private Queue cardOwners = new Queue();
        private object locker = new object();
        private TimeSpan maxTimeSpanToShowData = new TimeSpan(0, 0, 60);

        private delegate void InvokeDelegate(string s);
        private DateTime startOfShowingStatusBar = new DateTime(0);
        private BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer2;

        //public ACTASplashScreen splash = null;

        // reloading data
        private int reloadDay = -1;
        private bool dataReloaded = false;
        private bool photosReloaded = false;
        private string gateString = "";

        private Monitor()
		{
			InitializeComponent();

            if (NotificationController.GetApplicationName().Equals(""))
            {
                NotificationController.SetApplicationName(this.Text.Replace(" ", ""));
            }
            if (NotificationController.GetLogInUser() == null || NotificationController.GetLogInUser().UserID.Equals(""))
            {
                NotificationController.SetLogInUser(new ApplUserTO("MON", "", "", "", 0, "", 0, ""));
            }
		}

        public Monitor(string gateID)
        {
            InitializeComponent();

            if (NotificationController.GetApplicationName().Equals(""))
            {
                NotificationController.SetApplicationName(this.Text.Replace(" ", ""));
            }
            if (NotificationController.GetLogInUser() == null || NotificationController.GetLogInUser().UserID.Equals(""))
            {
                NotificationController.SetLogInUser(new ApplUserTO("MON", "", "", "", 0, "", 0, ""));
            }

            this.CenterToScreen();
            gateString = gateID;
        }

		public static Monitor Instance 
		{
			get 
			{
				if (_instance == null) 
				{
					_instance = new Monitor();
				}
				return _instance;
			}
		}

		private void Monitor_Load(object sender, System.EventArgs e)
		{
			//log = new DebugLog(Constants.logFilePath + System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id).ProcessName + "\\Log.txt");
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
            string ipAdresses = "";
            string vistorsCode = "";
            CultureInfo culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            ResourceManager rm = new ResourceManager("ACTAMonitorLib.GateResource", typeof(Monitor).Assembly);


            //ShowActivityInSplashScreen("Reading gates from DB ...");
			try
			{
                if (gateString.Equals(""))
                {
                    string strGates = ConfigurationManager.AppSettings["Gates"];
                    ipAdresses = ConfigurationManager.AppSettings["takeSnapshotFromCamera"];
                    vistorsCode = ConfigurationManager.AppSettings["VisitorsCode"];

                    if (strGates == null)
                    {
                        
                        MessageBox.Show(rm.GetString("noGates", culture));

                        ConfigAdd conf = new ConfigAdd(rm.GetString("Gates", culture));

                        conf.ShowDialog();

                        strGates = ConfigurationManager.AppSettings["Gates"];
                    }

                    if (strGates == null)
                    {
                        this.Close();
                    }
                    else
                    {
                        gates = strGates.Split(',');
                        for (int i = 0; i < gates.Length; i++)
                        {
                            gates[i] = gates[i].Trim();
                            string gate = gates[i];
                            gateReaders.Add(gate, (new Reader()).Search(gate));
                            gateDownloadStartTimes.Add(gate, (new Gate()).Find(Int32.Parse(gate)).DownloadStartTime);
                        }

                        allReaders = (new Reader()).Search(strGates);
                        
                    }
                }
                else
                {
                    gateReaders.Add(gateString, (new Reader()).Search(gateString));
                    gateDownloadStartTimes.Add(gateString, (new Gate()).Find(Int32.Parse(gateString)).DownloadStartTime);
                    allReaders = (new Reader()).Search(gateString);
                    gates = new string[1];
                    gates[0] = gateString;
                }

                //09.07.2009 Natasa 
                //If we have camera IPs in config file make pair (reader_id, camera_ip)
                if (ipAdresses != null&&ipAdresses.Trim().Length>0&&allReaders.Count>0)
                {
                    log.writeLog(DateTime.Now + " ACTAMonitor: Monitor will take pictures with the camera! ");
                    List<int> readersIDs = new List<int>();
                    foreach (ReaderTO r in allReaders)
                    {
                        readersIDs.Add(r.ReaderID);
                    }

                    //sort readers id's                  
                    readersIDs.Sort();

                    //make array of adresses from config split by comma
                    string[] adresses = ipAdresses.Split(',');

                    //if number of ip adresses in config doesn't match with number of readers show message
                    if (adresses.GetLength(0) != readersIDs.Count)
                    {
                        log.writeLog(DateTime.Now + " ACTAMonitor: Unmatching the number of terminals with a number of IP addresses in the config file. ");
                        MessageBox.Show(rm.GetString("adressesNotMatch", culture));
                    }
                    for(int i = 0; i<readersIDs.Count;i++)
                    {
                        readerCameras.Add(readersIDs[i], adresses[i]);
                    }
                }
			}
			catch(Exception ex)
			{
				log.writeLog(ex);
				MessageBox.Show("ACTAMonitor: Error getting gates from the database!");
				this.Close();
                return;
			}

            //ShowActivityInSplashScreen("Reading employees and tags from DB ...");
            try
            {
                employees = new Employee().Search();
                tags = new Tag().Search();
                int i = 0;
                if (!vistorsCode.Equals("") && int.TryParse(vistorsCode, out i))
                {
                    WorkingUnitTO wUnit = new WorkingUnitTO();
                    wUnit.WorkingUnitID = i;
                    List<WorkingUnitTO> wunits = new List<WorkingUnitTO>();
                    wunits.Add(wUnit);
                    wunits = new WorkingUnit().FindAllChildren(wunits);

                    foreach (WorkingUnitTO wu in wunits)
                    {
                        foreach (EmployeeTO empl in employees)
                        {
                            if (wu.WorkingUnitID == empl.WorkingUnitID)
                            {
                                foreach (TagTO t in tags)
                                {
                                    if(t.OwnerID == empl.EmployeeID)
                                    tagsForSnapshots.Add(t.TagID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
                MessageBox.Show("ACTAMonitor: Error getting employees and tags from the database!");
                this.Close();
                return;
            }

            if ((gates != null)||(!gateString.Equals("")))
            {
                //ShowActivityInSplashScreen("Uploading employees photos ...");
                try
                {
                    UploadEmployeesPhotos(null);
                }
                catch (Exception ex)
                {
                    log.writeLog(ex);
                    MessageBox.Show("ACTAMonitor: Error uploading employees photos!");
                }
                //ShowActivityInSplashScreen("Initializing antennas controls ...");
                try
                {
                    InitAntennasControls();
                    Screen[] allScreens = Screen.AllScreens;
                    this.Location = new Point((allScreens[0].Bounds.Width - this.Width) / 2,
                        (allScreens[0].Bounds.Height - this.Height) / 2);

                    foreach (ReaderTO reader in allReaders)
                    {
                        ShowAntennaControls(reader);
                    }
                }
                catch (Exception ex)
                {
                    log.writeLog(ex);
                    MessageBox.Show("ACTAMonitor: Error initializing antenna controls!");
                    this.Close();
                    return;
                }
                InitializeObserverClient();

                if (InitializeCardReaderMonitors() == false)
                {
                    this.Close();
                    return;
                }

                this.timer2.Enabled = true;
            }
		}
        //private void ShowActivityInSplashScreen(string activity)
        //{
        //    if (this.splash != null)
        //    {
        //        this.splash.ActivityText = activity;
        //    }
        //}
		private Size CalculateMonitorSizeFromAntenna(Size aSize) 
		{
			double width = (antennasLeftMargin + (allReaders.Count - 1) * antennasXDistance + 
				antennasRightMargin) * aSize.Width / 100.0 + allReaders.Count * aSize.Width;
			double height = (antennasTopMargin + antennasYDistance + 
				antennasBottomMargin) * aSize.Height / 100.0 + 2 * aSize.Height;
			return new Size((int)width, (int)height);
		}
		private Size CalculateAntennaSizeFromMonitor(Size mSize) 
		{
			double width = mSize.Width / ((antennasLeftMargin + (allReaders.Count - 1) * antennasXDistance + 
				antennasRightMargin) / 100.0 + allReaders.Count);
			double height = mSize.Height / ((antennasTopMargin + antennasYDistance + 
				antennasBottomMargin) / 100.0 + 2);
			return new Size((int)width, (int)height);
		}
		private Size CalculateAntennaSizeFromImage(Size iSize) 
		{
			double width = (imageLeftMargin + imageRightMargin) * iSize.Width / 100.0 + iSize.Width;
			double height = imageTopMargin * iSize.Height / 100.0 + iSize.Height + imageBottomMargin;
			return new Size((int)width, (int)height);
		}
		private Size CalculateImageSizeFromAntenna(Size aSize) 
		{
			double width = aSize.Width / ((imageLeftMargin + imageRightMargin) / 100.0 + 1);
			double height = (aSize.Height - imageBottomMargin) / (imageTopMargin / 100.0 + 1);
			return new Size((int)width, (int)height);
		}
		private bool ClipSize(ref Size size, Size maxSize) 
		{
			bool clipped = false;
			if (size.Width > maxSize.Width) 
			{
				size.Width = maxSize.Width;
				clipped = true;
			}
			if (size.Height > maxSize.Height) 
			{
				size.Height = maxSize.Height;
				clipped = true;
			}
			return clipped;
		}
		/// <summary>
		/// Performs layout according to employee image size.
		/// </summary>
		private void InitAntennasControls()
		{
            EmployeeImageFile eif = new EmployeeImageFile();
            bool useDatabaseFiles = false;

            int databaseCount = eif.SearchCount(-1);
            if (databaseCount >= 0)
                useDatabaseFiles = true;

			// determine original image size
			Size originalImageSize = new Size(200, 300);
            if (!useDatabaseFiles)
            {
                string path = Constants.EmployeePhotoDirectory;
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (FileInfo file in dir.GetFiles())
                {
                    //if (file.Name.EndsWith(".jpg") && !file.Name.StartsWith("whitehead"))
                    if ((file.Name.EndsWith(".jpg") || file.Name.EndsWith(".JPG")
                        || file.Name.EndsWith(".bmp") || file.Name.EndsWith(".BMP")
                        || file.Name.EndsWith(".gif") || file.Name.EndsWith(".GIF")
                        || file.Name.EndsWith(".jpeg") || file.Name.EndsWith(".JPEG"))
                        && !file.Name.StartsWith("whitehead"))
                    {
                        Image image = Image.FromFile(path + file.Name);
                        originalImageSize.Width = image.Width;
                        originalImageSize.Height = image.Height;
                        break;
                    }
                }
            }
            else
            {
                string[] statuses = { Constants.statusActive, Constants.statusBlocked };
                ArrayList emploeeImageInfo = eif.SearchImageInfo(statuses);

                if (emploeeImageInfo.Count > 0)
                {
                    ArrayList currentEmpl = eif.Search(((EmployeeImageFile)emploeeImageInfo[0]).EmployeeID);
                    if (currentEmpl.Count > 0)
                    {
                        byte[] emplPhoto = ((EmployeeImageFile)currentEmpl[0]).Picture;

                        MemoryStream memStream = new MemoryStream(emplPhoto);

                        // Set the position to the beginning of the stream.
                        memStream.Seek(0, SeekOrigin.Begin);

                        Image image = new Bitmap(memStream);
                        originalImageSize.Width = image.Width;
                        originalImageSize.Height = image.Height;

                        memStream.Close();
                    } //if (currentEmpl.Count > 0)
                }//if (emploeeImageInfo.Count > 0)
            }

			double originalImageProportion = (double)originalImageSize.Width / originalImageSize.Height;

			int uselessWidth = this.Size.Width - this.ClientSize.Width;
            int uselessHeight = this.Size.Height - this.ClientSize.Height + this.statusBar.Height + readerLabelHeight;
			Size monitorMinimumSize = new Size(this.MinimumSize.Width - uselessWidth, 
				this.MinimumSize.Height - uselessHeight);
			Size monitorMaximumSize = new Size(this.MaximumSize.Width - uselessWidth, 
				this.MaximumSize.Height - uselessHeight);

			Size monitorSize = new Size(monitorMinimumSize.Width, monitorMinimumSize.Height);

			Size antennaSize = CalculateAntennaSizeFromMonitor(monitorSize);
			Size imageSize = CalculateImageSizeFromAntenna(antennaSize);
			double imageProportion = (double)imageSize.Width / imageSize.Height;
			double devImageProportion = imageProportion / originalImageProportion;

			bool imageClipped = false;
			if (devImageProportion < (1 - imageProportionTolerance / 100) || 
				devImageProportion > (1 + imageProportionTolerance / 100)) 
			{
				// image size not in original proportion
				if ((devImageProportion > 1) || (devImageProportion < 0)) 
				{
					imageSize.Height = (int)((double)imageSize.Width / originalImageProportion);
				}
				else 
				{
					imageSize.Width = (int)((double)imageSize.Height * originalImageProportion);
				}
				imageClipped = ClipSize(ref imageSize, originalImageSize);
			}

			bool monitorClipped = false;
			while (!imageClipped) 
			{
				antennaSize = CalculateAntennaSizeFromImage(imageSize);
				monitorSize = CalculateMonitorSizeFromAntenna(antennaSize);
				
				if (ClipSize(ref monitorSize, monitorMaximumSize)) 
				{
					monitorClipped = true;
					break;
				}

				// increment employee image size
                int incW = (int)((double)imageSize.Width * imageWidthIncrement / 100);
                if (incW == 0) incW = 1;
                imageSize.Width += incW;
				imageSize.Height = (int)((double)imageSize.Width / originalImageProportion);
				imageClipped = ClipSize(ref imageSize, originalImageSize);
			}

			if (imageClipped) 
			{
				antennaSize = CalculateAntennaSizeFromImage(imageSize);
				monitorSize = CalculateMonitorSizeFromAntenna(antennaSize);

				if (ClipSize(ref monitorSize, monitorMaximumSize)) 
				{
					monitorClipped = true;
				}
			}
			if (monitorClipped) 
			{
				antennaSize = CalculateAntennaSizeFromMonitor(monitorSize);
				imageSize = CalculateImageSizeFromAntenna(antennaSize);
			}
			this.Size = new Size(monitorSize.Width + uselessWidth, 
				monitorSize.Height + uselessHeight);

			double x = ((double)antennasLeftMargin / 100.0) * antennaSize.Width / 2.0;
			double y = ((double)antennasTopMargin / 100.0) * antennaSize.Height / 2.0;
			this.gbEnter.Location = new Point((int)x, (int)y);
			y = antennaSize.Height + 
				((double)(antennasTopMargin + (double)antennasYDistance / 3.0 * 2.0)) / 100.0 * antennaSize.Height;
            this.gbExit.Location = new Point((int)x, (int)y + readerLabelHeight);
			double w = monitorSize.Width - 
				(double)(antennasLeftMargin + antennasRightMargin) / 100.0 * antennaSize.Width / 2.0;
			double h = antennaSize.Height + 
				(double)(antennasTopMargin / 2.0 + antennasYDistance / 3.0) / 100.0 * antennaSize.Height;
			this.gbEnter.Size = new Size((int)w, (int)h);
			this.gbExit.Size = this.gbEnter.Size;

			for(int i=0; i<allReaders.Count; i++)
			{
				int xPos = (int)((double)antennasLeftMargin / 100.0 * antennaSize.Width / 2.0 + 
					i * (antennaSize.Width + (double)antennasXDistance / 100.0 * antennaSize.Width));
				int yPos = (int)((double)antennasTopMargin / 100.0 * antennaSize.Height / 2.0) + 2;
				Antenna ant = new Antenna(xPos, yPos, new Size(antennaSize.Width, antennaSize.Height - 2), imageSize, imageLeftMargin, imageTopMargin);  
				ant.Visible = false;
				this.gbEnter.Controls.Add(ant);
				this.gbEnter.Invalidate();

				enterControlsList.Add(ant); 
			}
			for(int i=0; i<allReaders.Count; i++)
			{
				int xPos = (int)((double)antennasLeftMargin / 100.0 * antennaSize.Width / 2.0 + 
					i * (antennaSize.Width + (double)antennasXDistance / 100.0 * antennaSize.Width));
				int yPos = (int)((double)antennasTopMargin / 100.0 * antennaSize.Height / 2.0) + 2;
                Antenna ant = new Antenna(xPos, yPos, new Size(antennaSize.Width, antennaSize.Height - 2), imageSize, imageLeftMargin, imageTopMargin);  
				ant.Visible = false;
				this.gbExit.Controls.Add(ant);
				this.gbExit.Invalidate();

				exitControlsList.Add(ant); 
			}

			controlsList.AddRange(enterControlsList);
			controlsList.AddRange(exitControlsList);

            InitializeReadersLabels();

			this.Invalidate();
		}

        private void InitializeReadersLabels()
        {
            int i = 0;
            foreach (ReaderTO reader in allReaders)
            {
                Label label = new Label();
                label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Antenna ant = (Antenna) exitControlsList[i];
                int x = ant.Location.X;
                int y = this.gbExit.Location.Y - readerLabelHeight - 2;
                label.Location = new System.Drawing.Point(x, y);
                label.Name = "lbl" + reader.Description;
                label.Size = new System.Drawing.Size(ant.Size.Width, readerLabelHeight);
                label.Text = reader.Description;
                label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                this.Controls.Add(label);
                i++;
            }
        }

		private void ShowAntennaControls(ReaderTO reader)
		{
			// Antenna 0
			if ((reader.A0Direction.Equals(Constants.DirectionIn))
				|| (reader.A0Direction.Equals(Constants.DirectionInOut)))
			{
				Antenna antIn0 = (Antenna) enterControlsList[enterContInUse];
				antIn0.SetData(reader, 0);
				antIn0.Visible = true;
				enterContInUse ++;
			}
			else if (reader.A0Direction.Equals(Constants.DirectionOut))
			{
				Antenna antIn0 = (Antenna) exitControlsList[exitContInUse];
				antIn0.SetData(reader, 0);
				antIn0.Visible = true;
				exitContInUse ++;
			}
				
			// Antenna 1
			if ((reader.A1Direction.Equals(Constants.DirectionIn))
				|| (reader.A1Direction.Equals(Constants.DirectionInOut)))
			{
				Antenna antIn1 = (Antenna) enterControlsList[enterContInUse];
				antIn1.SetData(reader, 1);
				antIn1.Visible = true;
				enterContInUse ++;
			}
			else if (reader.A1Direction.Equals(Constants.DirectionOut))
			{
				Antenna antIn1 = (Antenna) exitControlsList[exitContInUse];
				antIn1.SetData(reader, 1);
				antIn1.Visible = true;
				exitContInUse ++;
			}

			exitContInUse = Math.Max(enterContInUse, exitContInUse);
			enterContInUse = Math.Max(enterContInUse, exitContInUse);
		}

		private void InitializeObserverClient()
		{
			notificationObserverClient = new NotificationObserverClient(this.ToString());
			notificationController = NotificationController.GetInstance();	
			notificationController.AttachToNotifier(notificationObserverClient);
			this.notificationObserverClient.OnCardOwnerObserved += new NotificationEventHandler(this.CardOwnerObservedHandler);
			this.notificationObserverClient.OnDataProcessingStateChanged += new NotificationEventHandler(this.DataProcessingStateChangedHandler);
		}

		private bool InitializeCardReaderMonitors() 
		{
            bool success = false;
            string currGate = "";
			try 
			{
                foreach (string gate in gates)
                {
                    currGate = gate;
                    //ShowActivityInSplashScreen("Initializing monitoring on gate" + gate + " ...");
                    CardReaderMonitor crm = new CardReaderMonitor(gateReaders[gate], gate,readerCameras, tagsForSnapshots);
                    crm.SetEmployeesAndTags(employees, tags);
                    gateCardReaderMonitors.Add(gate, crm);
                }
                success = true;
			}
			catch 
			{
                MessageBox.Show("ACTAMonitor: Error initializing monitoring on gate" + currGate + "!");
                this.StopMonitoring();
            }
            return success;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor));
            this.gbEnter = new System.Windows.Forms.GroupBox();
            this.gbExit = new System.Windows.Forms.GroupBox();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // gbEnter
            // 
            this.gbEnter.Location = new System.Drawing.Point(16, 0);
            this.gbEnter.Name = "gbEnter";
            this.gbEnter.Size = new System.Drawing.Size(562, 248);
            this.gbEnter.TabIndex = 2;
            this.gbEnter.TabStop = false;
            this.gbEnter.Text = "ENTER";
            // 
            // gbExit
            // 
            this.gbExit.Location = new System.Drawing.Point(16, 256);
            this.gbExit.Name = "gbExit";
            this.gbExit.Size = new System.Drawing.Size(562, 248);
            this.gbExit.TabIndex = 3;
            this.gbExit.TabStop = false;
            this.gbExit.Text = "EXIT";
            this.gbExit.Enter += new System.EventHandler(this.gbExit_Enter);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 544);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(590, 20);
            this.statusBar.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Monitor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(590, 564);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.gbEnter);
            this.Controls.Add(this.gbExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 720);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "Monitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ACTAMonitor";
            this.Load += new System.EventHandler(this.Monitor_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Monitor_Closing);
            this.ResumeLayout(false);

		}
		#endregion

		private void DataProcessingStateChangedHandler(object sender, NotificationEventArgs args)
		{
            this.statusBar.BeginInvoke(new InvokeDelegate(SetStatusBarText), args.message);
		}

        private void SetStatusBarText(string text)
        {
            this.statusBar.Text = text;
            startOfShowingStatusBar = DateTime.Now;
        }

		private void PopulateAntenna(ReaderTO reader, int antennaNum, string picturePath, string empoloyeeName, 
			string employeeID, bool entrancePermitted)
		{
			foreach(Antenna antenna in controlsList)
			{
				if (antenna.AntennaReader.ReaderID.Equals(reader.ReaderID) && (antenna.AntNum.Equals(antennaNum)))
				{
					antenna.ShowData(picturePath, empoloyeeName, employeeID, entrancePermitted);
				}
			}
		}

		private void Monitor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.StopMonitoring();
		}

		public void StopMonitoring() 
		{
			try 
			{
				this.Cursor = Cursors.WaitCursor;

                foreach (string gate in gates)
                {
                    try
                    {
                        if (gateCardReaderMonitors.ContainsKey(gate) && gateCardReaderMonitors[gate] != null) gateCardReaderMonitors[gate].Dispose();
                    }
                    catch (Exception ex)
                    {
                        log.writeLog(ex);
                    }
                }

				notificationController.DettachFromNotifier(notificationObserverClient);
			}
			catch 
			{
			}
			_instance = null;
		}

        /// <summary>
        /// Enqueues a card onwer to be shown (in timer1_Tick()).
        /// </summary>
        private void CardOwnerObservedHandler(object sender, NotificationEventArgs e)
        {
            lock (locker)
            {
                cardOwners.Enqueue(e.cardOwner);
            }
        }

        /// <summary>
        /// Shows a card owner.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Stop();
            bool exOccured = false;

            try
            {
                // clear antenna controls which has been showing image for more than predefined time interval
                foreach (Antenna antenna in controlsList)
                {
                    if (antenna.isShowingData)
                    {
                        if (DateTime.Now.Subtract(maxTimeSpanToShowData) > antenna.startOfShowingData)
                        {
                            antenna.ClearAntennaCtrl();
                        }
                    }
                }

                // clear status bar
                if (DateTime.Now.Subtract(maxTimeSpanToShowData) > startOfShowingStatusBar)
                {
                    this.statusBar.Text = "";
                }

                // update antenna controls with newly registered card owners
                CardOwner cardOwner = null;
                lock (locker)
                {
                    if (cardOwners.Count > 0)
                    {
                        cardOwner = (CardOwner)cardOwners.Dequeue();
                    }
                }

                if (cardOwner != null)
                {
                    Console.WriteLine("Main > notified card owner Id: " + cardOwner.employee.EmployeeID);

                    string path = Constants.LocalEmployeePhotoDirectory;
                    if (cardOwner.employee.Picture.Trim().Equals(""))
                    {
                        //path += ConfigurationManager.AppSettings["whiteheadPic"];
                        path += Constants.whiteheadImage;
                    }
                    else
                    {
                        path += cardOwner.employee.Picture.Trim();
                    }

                    PopulateAntenna(cardOwner.reader, cardOwner.antennaNum, path,
                        cardOwner.employee.FirstName + " " + cardOwner.employee.LastName,
                        cardOwner.employee.EmployeeID.ToString(), cardOwner.entrancePermitted);
                }
            }
            catch (Exception ex)
            {
                exOccured = true;
                log.writeLog(ex);
                this.statusBar.Text = "Monitor has been stopped for an unknown exception!";
            }

            if (!exOccured)
            {
                this.timer1.Enabled = true;
            }
        }

        private void UploadEmployeesPhotos(Object dbConnection)
        {
            EmployeeImageFile eif;
            if (dbConnection != null)
            {
                eif = new EmployeeImageFile(dbConnection);
            }
            else
            {
                eif = new EmployeeImageFile();
            }

            bool useDatabaseFiles = false;

            int databaseCount = eif.SearchCount(-1);
            if (databaseCount >= 0)
                useDatabaseFiles = true;

            if (!useDatabaseFiles)
            {
                string path = Constants.EmployeePhotoDirectory;
                DirectoryInfo dir = new DirectoryInfo(path);

                string localPath = Constants.LocalEmployeePhotoDirectory;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                DirectoryInfo localDir = new DirectoryInfo(localPath);

                System.Collections.Hashtable localPhotos = new System.Collections.Hashtable();
                FileInfo[] localPhotosDirFileList = localDir.GetFiles();
                foreach (FileInfo file in localPhotosDirFileList)
                {
                    localPhotos.Add(file.Name, file);
                }

                System.Collections.Hashtable photos = new System.Collections.Hashtable();
                FileInfo[] photosDirFileList = dir.GetFiles();
                foreach (FileInfo file in photosDirFileList)
                {
                    photos.Add(file.Name, file);

                    if (file.Name.EndsWith(".jpg") || file.Name.EndsWith(".JPG")
                        || file.Name.EndsWith(".bmp") || file.Name.EndsWith(".BMP")
                        || file.Name.EndsWith(".gif") || file.Name.EndsWith(".GIF")
                        || file.Name.EndsWith(".jpeg") || file.Name.EndsWith(".JPEG"))
                    {
                        if (!localPhotos.Contains(file.Name))
                        {
                            file.CopyTo(localPath + file.Name);
                            log.writeLog("Photo file does not exist: " + file.Name + " copied");
                        }
                        else
                        {
                            if (file.LastWriteTime.ToShortDateString() != ((FileInfo)(localPhotos[file.Name])).LastWriteTime.ToShortDateString())
                            {
                                file.CopyTo(localPath + file.Name, true);
                                log.writeLog("Photo file was modified: " + file.Name + " copied " +
                                             ", file time = " + file.LastWriteTime.ToString() +
                                             ", local file time = " + ((FileInfo)(localPhotos[file.Name])).LastWriteTime.ToString());
                            }
                        }
                    }
                }

                // local photos folder clean-up
                foreach (FileInfo file in localDir.GetFiles())
                {
                    if (!photos.Contains(file.Name))
                    {
                        file.Delete();
                        log.writeLog("Photo file does not exist in database: " + file.Name + " deleted");
                    }
                }
            } //if (!useDatabaseFiles)
            else
            {
                string[] statuses = { Constants.statusActive, Constants.statusBlocked };
                ArrayList emploeeImageInfo = eif.SearchImageInfo(statuses);

                string localPath = Constants.LocalEmployeePhotoDirectory;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                DirectoryInfo localDir = new DirectoryInfo(localPath);

                System.Collections.Hashtable localPhotos = new System.Collections.Hashtable();
                FileInfo[] localPhotosDirFileList = localDir.GetFiles();
                foreach (FileInfo file in localPhotosDirFileList)
                {
                    localPhotos.Add(file.Name.ToUpper(), file);
                }

                System.Collections.Hashtable photos = new System.Collections.Hashtable();
                foreach (EmployeeImageFile emplIF in emploeeImageInfo)
                {
                    photos.Add(emplIF.PictureName.ToUpper(), "");

                    if (!localPhotos.Contains(emplIF.PictureName.ToUpper()))
                    {
                        ArrayList al = eif.Search(emplIF.EmployeeID);
                        if (al.Count > 0)
                        {
                            byte[] emplPhoto = ((EmployeeImageFile)al[0]).Picture;

                            Stream stream = File.Open(localPath + emplIF.PictureName, FileMode.Create);
                            stream.Write(emplPhoto, 0, emplPhoto.Length);
                            stream.Close();
                            File.SetCreationTime(localPath + emplIF.PictureName, emplIF.ModifiedTime);
                            File.SetLastWriteTime(localPath + emplIF.PictureName, emplIF.ModifiedTime);
                            log.writeLog("Photo file does not exist: " + emplIF.PictureName + " copied");
                        }
                        else
                            log.writeLog("Error: " + emplIF.PictureName + " can not be copied");
                    }
                    else
                    {
                        if (emplIF.ModifiedTime.Ticks > ((FileInfo)(localPhotos[emplIF.PictureName.ToUpper()])).LastWriteTime.Ticks)
                        {
                            ArrayList al = eif.Search(emplIF.EmployeeID);
                            if (al.Count > 0)
                            {
                                //delete is needed, because, without it, if old picture was 2.jpg, and new is 2.JPG,
                                //create will not change the name, but, in next foreach, the file 2.jpg will not be recognized
                                //and it will be deleted
                                ((FileInfo)(localPhotos[emplIF.PictureName.ToUpper()])).Delete();

                                byte[] emplPhoto = ((EmployeeImageFile)al[0]).Picture;

                                Stream stream = File.Open(localPath + emplIF.PictureName, FileMode.Create);
                                stream.Write(emplPhoto, 0, emplPhoto.Length);
                                stream.Close();
                                File.SetCreationTime(localPath + emplIF.PictureName, emplIF.ModifiedTime);
                                File.SetLastWriteTime(localPath + emplIF.PictureName, emplIF.ModifiedTime);
                                log.writeLog("Photo file was modified: " + emplIF.PictureName + " copied " +
                                         ", file time = " + emplIF.ModifiedTime.ToString() +
                                         ", local file time = " + ((FileInfo)(localPhotos[emplIF.PictureName.ToUpper()])).LastWriteTime.ToString());
                            }
                            else
                                log.writeLog("Error: " + emplIF.PictureName + " can not be modified");
                        }
                    }
                }

                //add whitehead
                if (!localPhotos.Contains(Constants.whiteheadImage.ToUpper()))
                {
                    Image image = Util.ResImages.whitehead;

                    image.Save(localPath + Constants.whiteheadImage);
                    log.writeLog("Photo file does not exist: " + Constants.whiteheadImage + " copied");
                }

                // local photos folder clean-up
                foreach (FileInfo file in localDir.GetFiles())
                {
                    if ((!photos.Contains(file.Name.ToUpper()))
                        && (file.Name.ToUpper() != Constants.whiteheadImage.ToUpper()))
                    {
                        file.Delete();
                        log.writeLog("Photo file does not exist in database: " + file.Name + " deleted");
                    }
                }
            }
        }

        /// <summary>
        /// Runs background worker for reloading data.
        /// </summary>
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (gateDownloadStartTimes.Count > 0)
            {
                if ((DateTime.Now.Hour == gateDownloadStartTimes[gates[0]].Hour)
                    && (DateTime.Now.Minute == gateDownloadStartTimes[gates[0]].Minute)
                    && (DateTime.Now.Day != reloadDay))
                {
                    if (backgroundWorker1.IsBusy == false)
                    {
                        backgroundWorker1.RunWorkerAsync();
                        reloadDay = DateTime.Now.Day;
                    }
                }
            }
        }

        /// <summary>
        /// Reloads data.
        /// </summary>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            dataReloaded = false;
            photosReloaded = false;

            Object dbConnection = null;
            try
            {
                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();

                employees = new Employee(dbConnection).Search();
                tags = new Tag(dbConnection).Search();
                dataReloaded = true;

                UploadEmployeesPhotos(dbConnection);
                photosReloaded = true;

                log.writeLog(DateTime.Now + " Data reloaded from DB.\n");
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
            finally
            {
                if (dbConnection != null)
                {
                    DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                }
            }
        }

        /// <summary>
        /// Transfers data to card owner resolvers after reloading.
        /// </summary>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!dataReloaded)
            {
                SetStatusBarText("ACTAMonitor: Error getting employees and tags from the database!");
            }

            if (!photosReloaded)
            {
                SetStatusBarText("ACTAMonitor: Error uploading employees photos!");
            }

            try
            {
                if (dataReloaded)
                {
                    foreach (string gate in gates)
                    {
                        if (gateCardReaderMonitors[gate] != null) gateCardReaderMonitors[gate].SetEmployeesAndTags(employees, tags);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
                SetStatusBarText("ACTAMonitor: Error setting employees and tags!");
            }
        }

        private void gbExit_Enter(object sender, EventArgs e)
        {

        }
	}
}
