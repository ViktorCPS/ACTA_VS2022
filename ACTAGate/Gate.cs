using System;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Data;
using System.Text;
using System.Threading;

using Common;
using ReaderManagement;
using UI;
using Util;
using TransferObjects;

namespace ACTAGate
{
	/// <summary>
	/// Summary description for ACTAGate.
	/// </summary>
	public class Gate : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuLibraries;
		private System.Windows.Forms.MenuItem menuDisplay;
		private System.Windows.Forms.MenuItem menuRegister;
		private System.Windows.Forms.MenuItem menuVisitors;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuHelp;
		private System.Windows.Forms.MenuItem menuAbout;
		private System.Windows.Forms.MenuItem menuVisitorsEnter;
		private System.Windows.Forms.MenuItem menuVisitorsExit;
		private System.Windows.Forms.MenuItem menuVisitorsReport;
        private System.Windows.Forms.MenuItem menuCheck;
        private System.Windows.Forms.MenuItem menuManual;
        private System.Windows.Forms.MenuItem menuItemMonitor;

        private IContainer components = null;

        private CultureInfo culture;
        private ResourceManager rm;
        // Debug
        DebugLog debug;

		public Gate()
		{
			try
			{
				InitializeComponent();

				// Debug
				NotificationController.SetApplicationName(this.Text.Replace(" ", ""));
				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				debug = new DebugLog(logFilePath);

				this.CenterToScreen();

				// Set Language
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.GateResource", typeof(Employees).Assembly);
				setLanguage(); 
			}
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.Gate(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gate));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuLibraries = new System.Windows.Forms.MenuItem();
            this.menuDisplay = new System.Windows.Forms.MenuItem();
            this.menuRegister = new System.Windows.Forms.MenuItem();
            this.menuItemMonitor = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuCheck = new System.Windows.Forms.MenuItem();
            this.menuVisitors = new System.Windows.Forms.MenuItem();
            this.menuVisitorsEnter = new System.Windows.Forms.MenuItem();
            this.menuVisitorsExit = new System.Windows.Forms.MenuItem();
            this.menuVisitorsReport = new System.Windows.Forms.MenuItem();
            this.menuHelp = new System.Windows.Forms.MenuItem();
            this.menuManual = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuLibraries,
            this.menuCheck,
            this.menuVisitors,
            this.menuHelp});
            // 
            // menuLibraries
            // 
            this.menuLibraries.Index = 0;
            this.menuLibraries.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuDisplay,
            this.menuRegister,
            this.menuItemMonitor,
            this.menuExit});
            this.menuLibraries.Text = "Libraries";
            // 
            // menuDisplay
            // 
            this.menuDisplay.Enabled = false;
            this.menuDisplay.Index = 0;
            this.menuDisplay.Text = "Display Passenger";
            this.menuDisplay.Visible = false;
            this.menuDisplay.Click += new System.EventHandler(this.menuDisplay_Click);
            // 
            // menuRegister
            // 
            this.menuRegister.Enabled = false;
            this.menuRegister.Index = 1;
            this.menuRegister.Text = "Register";
            this.menuRegister.Visible = false;
            this.menuRegister.Click += new System.EventHandler(this.menuRegister_Click);
            // 
            // menuItemMonitor
            // 
            this.menuItemMonitor.Index = 2;
            this.menuItemMonitor.Text = "Monitor";
            this.menuItemMonitor.Click += new System.EventHandler(this.menuItemMonitor_Click);
            // 
            // menuExit
            // 
            this.menuExit.Index = 3;
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuCheck
            // 
            this.menuCheck.Index = 1;
            this.menuCheck.Text = "Check";
            this.menuCheck.Visible = false;
            this.menuCheck.Click += new System.EventHandler(this.menuCheck_Click);
            // 
            // menuVisitors
            // 
            this.menuVisitors.Index = 2;
            this.menuVisitors.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuVisitorsEnter,
            this.menuVisitorsExit,
            this.menuVisitorsReport});
            this.menuVisitors.Text = "Visitors";
            // 
            // menuVisitorsEnter
            // 
            this.menuVisitorsEnter.Index = 0;
            this.menuVisitorsEnter.Text = "Enter";
            this.menuVisitorsEnter.Click += new System.EventHandler(this.menuVisitorsEnter_Click);
            // 
            // menuVisitorsExit
            // 
            this.menuVisitorsExit.Index = 1;
            this.menuVisitorsExit.Text = "Exit";
            this.menuVisitorsExit.Click += new System.EventHandler(this.menuVisitorsExit_Click);
            // 
            // menuVisitorsReport
            // 
            this.menuVisitorsReport.Index = 2;
            this.menuVisitorsReport.Text = "Visitors Report";
            this.menuVisitorsReport.Click += new System.EventHandler(this.menuVisitorsReport_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.Index = 3;
            this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuManual,
            this.menuAbout});
            this.menuHelp.Text = "Help";
            // 
            // menuManual
            // 
            this.menuManual.Index = 0;
            this.menuManual.Text = "Manual";
            this.menuManual.Click += new System.EventHandler(this.menuManual_Click);
            // 
            // menuAbout
            // 
            this.menuAbout.Index = 1;
            this.menuAbout.Text = "About Project";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // Gate
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(432, 318);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(440, 352);
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(440, 352);
            this.Name = "Gate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ACTAGate";
            this.Closed += new System.EventHandler(this.ACTAGate_Closed);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Gate_Closing);
            this.Load += new System.EventHandler(this.ACTAGate_Load);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			bool newMutexCreated = false;
			
			try
			{
				string mutexName = "Local\\" + 
					System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
				Mutex mutex = null;
				try
				{
					// Create a new mutex object with a unique name
					mutex = new Mutex(false, mutexName, out newMutexCreated);
				}
				catch(Exception ex)
				{
					MessageBox.Show (ex.Message+"\n\n"+ex.StackTrace+ 
						"\n\n"+"Application Exiting...","Exception thrown");
					Application.Exit ();
				}

				if(newMutexCreated)
				{
					Gate gate = new Gate();
					Application.Run(gate);
				}
				else
				{
					ResourceManager rm;
					CultureInfo culture;
					culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("UI.GateResource", typeof(Employees).Assembly);
					MessageBox.Show(rm.GetString("applRunning", culture));
				}
			}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
		}

		private void ACTAGate_Load(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				DataManager dataController = new DataManager();

				if (!dataController.TestDataBaseConnection())
                {
                    MessageBox.Show(rm.GetString("applCantStart", culture));
                    this.Close();
                    return;
				}
			}
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.ACTAGate_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Close();
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
				this.Text = rm.GetString("ACTAGateForm", culture);
				
				menuLibraries.Text = rm.GetString("menuLibraries", culture);
                menuDisplay.Text = rm.GetString("menuDisplay", culture);
				menuRegister.Text = rm.GetString("menuRegister", culture);
                menuItemMonitor.Text = rm.GetString("menuMonitor", culture);
                menuExit.Text = rm.GetString("menuExit", culture);

                menuCheck.Text = rm.GetString("menuCheck", culture);

				menuVisitors.Text = rm.GetString("menuVisitors", culture);
				menuVisitorsEnter.Text = rm.GetString("menuVisitorsEnter", culture);
				menuVisitorsExit.Text = rm.GetString("menuVisitorsExit", culture);
				menuVisitorsReport.Text = rm.GetString("menuVisitorsReport", culture);

				menuHelp.Text = rm.GetString("menuHelp", culture);
                menuManual.Text = rm.GetString("menuManual", culture);
				menuAbout.Text = rm.GetString("menuAbout", culture);			
			}
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

        private void ACTAGate_Closed(object sender, System.EventArgs e)
        {
            /*
			try
			{
				ReaderEventController controller = ReaderEventController.GetInstance();
				controller.
				controller.StopMonitor();
			}
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " GACTAGate_Closed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
             * */
        }

        private void menuDisplay_Click(object sender, System.EventArgs e)
        {
            try
            {
                List<EmployeeTO> employees = new List<EmployeeTO>();
                List<TagTO> tags = new List<TagTO>();
                List<ReaderTO> readers = new List<ReaderTO>();
                List<LocationTO> locations = new List<LocationTO>();
                                
                employees = new Employee().Search();                
                tags = new Tag().Search();

                string gates = ConfigurationManager.AppSettings["Gates"].ToString();

                Reader reader = new Reader();
                readers = reader.Search(gates);
                Location location = new Location();
                location.LocTO.Status = Constants.DefaultStateActive;
                List<LocationTO> loc = location.Search();

                foreach (LocationTO currentLocation in loc)
                {
                    foreach (ReaderTO currentReader in readers)
                    {
                        if (currentReader.A0LocID.ToString().Trim().Equals(currentLocation.LocationID.ToString().Trim())
                            || currentReader.A1LocID.ToString().Trim().Equals(currentLocation.LocationID.ToString().Trim()))
                        {
                            if (!locations.Contains(currentLocation))
                            {
                                locations.Add(currentLocation);
                            }
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                string message = "";

                if (employees.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("employees", culture) + ", ");
                }
                if (tags.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("tags", culture) + ", ");
                }
                if (readers.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("readers", culture) + ", ");
                }
                if (locations.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("locations", culture) + ", ");
                }

                if (sb.Length > 0)
                {
                    sb.Insert(0, rm.GetString("dataMissing1", culture) + " \n");
                    sb.Remove(sb.Length - 2, 2);
                    sb.Append("\n\n" + rm.GetString("dataMissing2", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing3", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing4", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing5", culture) + " ");
                }
                message = sb.ToString();

                if (!message.Equals(""))
                {
                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        this.Close();
                    }
                }

                UI.Monitor monitor = new UI.Monitor(employees, tags, readers, locations);
                monitor.Show();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuDisplay_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRegister_Click(object sender, System.EventArgs e)
        {
            try
            {
                List<EmployeeTO> employees = new List<EmployeeTO>();
                List<ReaderTO> readers = new List<ReaderTO>();
                List<LocationTO> locations = new List<LocationTO>();
                List<PassTypeTO> passtypes = new List<PassTypeTO>();
                List<WorkingUnitTO> workingunits = new List<WorkingUnitTO>();
                                
                employees = new Employee().Search();

                string gates = ConfigurationManager.AppSettings["Gates"].ToString();

                Reader reader = new Reader();
                readers = reader.Search(gates);
                Location location = new Location();
                location.LocTO.Status = Constants.DefaultStateActive;
                List<LocationTO> loc = location.Search();

                foreach (LocationTO currentLocation in loc)
                {
                    foreach (ReaderTO currentReader in readers)
                    {
                        if (currentReader.A0LocID.ToString().Trim().Equals(currentLocation.LocationID.ToString().Trim())
                            || currentReader.A1LocID.ToString().Trim().Equals(currentLocation.LocationID.ToString().Trim()))
                        {
                            if (!locations.Contains(currentLocation))
                            {
                                locations.Add(currentLocation);
                            }
                        }
                    }
                }
                                
                passtypes = new PassType().Search();

                WorkingUnit workingunit = new WorkingUnit();
                workingunit.WUTO.Status = Constants.DefaultStateActive.Trim();
                workingunits = workingunit.Search();

                StringBuilder sb = new StringBuilder();
                string message = "";

                if (employees.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("employees", culture) + ", ");
                }
                if (workingunits.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("workingunits", culture) + ", ");
                }
                if (locations.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("locations", culture) + ", ");
                }
                if (passtypes.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("passtypes", culture) + ", ");
                }

                if (sb.Length > 0)
                {
                    sb.Insert(0, rm.GetString("dataMissing1", culture) + " \n");
                    sb.Remove(sb.Length - 2, 2);
                    sb.Append("\n\n" + rm.GetString("dataMissing2", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing3", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing4", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing5", culture) + " ");
                }
                message = sb.ToString();

                if (!message.Equals(""))
                {
                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        this.Close();
                    }
                }

                PassesAdd pa = new PassesAdd("ACTAGate", employees, workingunits, locations, passtypes);
                pa.ShowDialog();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuRegister_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        // Monitor part
        private void menuItemMonitor_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor cursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                ACTAMonitorLib.Monitor.Instance.Show();
                this.Cursor = cursor;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuItemMonitor_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void Gate_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ACTAMonitorLib.Monitor.Instance.StopMonitoring();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.Gate_Closing(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        // End Monitor part

        private void menuExit_Click(object sender, System.EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuExit_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void menuCheck_Click(object sender, System.EventArgs e)
		{
            try
            {
                List<EmployeeTO> employees = new List<EmployeeTO>();
                List<TagTO> tags = new List<TagTO>();
                List<WorkingUnitTO> workingunits = new List<WorkingUnitTO>();
                                
                employees = new Employee().Search();                
                tags = new Tag().Search();
                WorkingUnit workingunit = new WorkingUnit();
                workingunit.WUTO.Status = Constants.DefaultStateActive.Trim();
                workingunits = workingunit.Search();

                StringBuilder sb = new StringBuilder();
                string message = "";

                if (employees.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("employees", culture) + ", ");
                }
                if (tags.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("tags", culture) + ", ");
                }
                if (workingunits.Count.Equals(0))
                {
                    sb.Append("\n" + rm.GetString("workingunits", culture) + ", ");
                }

                if (sb.Length > 0)
                {
                    sb.Insert(0, rm.GetString("dataMissing1", culture) + " \n");
                    sb.Remove(sb.Length - 2, 2);
                    sb.Append("\n\n" + rm.GetString("dataMissing2", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing3", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing4", culture) + " ");
                    sb.Append("\n" + rm.GetString("dataMissing5", culture) + " ");
                }
                message = sb.ToString();

                if (!message.Equals(""))
                {
                    DialogResult result = MessageBox.Show(message, "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        this.Close();
                    }
                }

                CheckCard chkCard = new CheckCard(employees, workingunits, tags);
                chkCard.ShowDialog();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuCheck_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

        private void menuVisitorsEnter_Click(object sender, System.EventArgs e)
        {
            try
            {
                Visitors visitorsEnter = new Visitors("Enter");
                visitorsEnter.ShowDialog();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuVisitorsEnter_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuVisitorsExit_Click(object sender, System.EventArgs e)
        {
            try
            {
                Visitors visitorsExit = new Visitors("Exit");
                visitorsExit.ShowDialog();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuVisitorsExit_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuVisitorsReport_Click(object sender, System.EventArgs e)
        {
            try
            {
                VisitorsView visitorsView = new VisitorsView();
                visitorsView.ShowDialog();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuVisitorsReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuManual_Click(object sender, System.EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Constants.HelpACTAGateManualPath);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuManual_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("manualNotOpened", culture));
            }
        }

        private void menuAbout_Click(object sender, System.EventArgs e)
        {
            try
            {
                MessageBox.Show("ACTAGate v.1\nCopyright @ 2007 \nSDD ITG, Belgrade", "ACTAGate");
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Gate.menuAbout_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
	}
}

