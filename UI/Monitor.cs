using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Threading;

using TransferObjects;
using Common;
using ReaderManagement;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Monitor : System.Windows.Forms.Form
	{
		private List<EmployeeTO> employees = new List<EmployeeTO>();
		private List<TagTO> tags = new List<TagTO>();		
		private List<ReaderTO> readers = new List<ReaderTO>();

		private CultureInfo culture;
		private ResourceManager rm;
		private System.Windows.Forms.GroupBox gbEnter;
		private System.Windows.Forms.GroupBox gbExit;
		
		private ReaderEventController monitorController;

		ArrayList EnterControlsList = new ArrayList();
		int enterContInUse = 0;
		
		ArrayList ExitContolsList = new ArrayList();
		int exitContInUse = 0;

		ArrayList ControlsList = new ArrayList();
		
		// Controller instance
		public NotificationController Controller;
		
		// Observer client instance
		public NotificationObserverClient observerClient;

		// Debug
		DebugLog debug;

		public Monitor(List<EmployeeTO> employees, List<TagTO> tags, List<ReaderTO> readers, List<LocationTO> locations)
		{
			InitializeObserverClient();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			this.employees = employees;
			this.tags = tags;
			this.readers = readers;
				
			InitializeComponent();
			InitAntennasControls();

			foreach(ReaderTO reader in readers)
			{
				visibleControls(reader);
			}
			
			monitorController = ReaderEventController.GetInstance();
			monitorController.SetData(employees, tags, readers, locations);
			monitorController.SetReaders();
			monitorController.StartMonitor();

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.GateResource", typeof(Monitor).Assembly);

			//setLanguage();
		}

		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor));
            this.gbEnter = new System.Windows.Forms.GroupBox();
            this.gbExit = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // gbEnter
            // 
            this.gbEnter.Location = new System.Drawing.Point(16, 8);
            this.gbEnter.Name = "gbEnter";
            this.gbEnter.Size = new System.Drawing.Size(888, 248);
            this.gbEnter.TabIndex = 0;
            this.gbEnter.TabStop = false;
            this.gbEnter.Text = "ENTER";
            // 
            // gbExit
            // 
            this.gbExit.Location = new System.Drawing.Point(16, 264);
            this.gbExit.Name = "gbExit";
            this.gbExit.Size = new System.Drawing.Size(888, 248);
            this.gbExit.TabIndex = 1;
            this.gbExit.TabStop = false;
            this.gbExit.Text = "EXIT";
            // 
            // Monitor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(912, 511);
            this.Controls.Add(this.gbEnter);
            this.Controls.Add(this.gbExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(920, 545);
            this.MinimumSize = new System.Drawing.Size(920, 545);
            this.Name = "Monitor";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Monitor";
            this.Closed += new System.EventHandler(this.Form1_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Monitor_KeyUp);
            this.ResumeLayout(false);

		}
		#endregion
		

		private void InitAntennasControls()
		{
			
			for(int i = 0; i < Constants.MAXNUMANTENNAS; i++)
			{
				Antenna ant = new Antenna(15 + (i * 145), 15, new Size(0,0), new Size(0,0), 0, 0);  
				ant.Visible = false;
				this.gbEnter.Controls.Add(ant);
				this.gbEnter.Invalidate();

				EnterControlsList.Add(ant); 
			}

            for (int i = 0; i < Constants.MAXNUMANTENNAS; i++)
			{
				Antenna ant = new Antenna(15 + (i * 145), 15, new Size(0,0), new Size(0,0), 0, 0);  
				ant.Visible = false;
				this.gbExit.Controls.Add(ant);
				this.gbExit.Invalidate();

				ExitContolsList.Add(ant); 
			}

			ControlsList.AddRange(EnterControlsList);
			ControlsList.AddRange(ExitContolsList);

			this.Invalidate();
		}


		private void Form1_Closed(object sender, System.EventArgs e)
		{
			try
			{
				monitorController.StopMonitor();
				Controller.DettachFromNotifier(this.observerClient);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Exception: " + ex.Message);
			}
		}

		private void setLanguage()
		{
			try
			{
				// Form name				
				this.Text = rm.GetString("menuMonitor", culture);
				
				//groupBox name
				gbEnter.Text = rm.GetString("gbEnter", culture);
				gbExit.Text = rm.GetString("gbExit", culture);
				
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Monitor.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void visibleControls(ReaderTO reader)
		{
			try
			{
				// Antenna 0
				if ((reader.A0Direction.Equals(Constants.DirectionIn))
					|| (reader.A0Direction.Equals(Constants.DirectionInOut)))
				{
					Antenna antIn0 = (Antenna) EnterControlsList[enterContInUse];
					antIn0.SetData(reader, 0);
					antIn0.Visible = true;
					enterContInUse ++;
				}
				else if (reader.A0Direction.Equals(Constants.DirectionOut))
				{
					Antenna antIn0 = (Antenna) ExitContolsList[exitContInUse];
					antIn0.SetData(reader, 0);
					antIn0.Visible = true;
					exitContInUse ++;
				}
					
				// Antenna 1
				if ((reader.A1Direction.Equals(Constants.DirectionIn))
					|| (reader.A0Direction.Equals(Constants.DirectionInOut)))
				{
					Antenna antIn1 = (Antenna) EnterControlsList[enterContInUse];
					antIn1.SetData(reader, 1);
					antIn1.Visible = true;
					enterContInUse ++;
				}
				else if (reader.A1Direction.Equals(Constants.DirectionOut))
				{
					Antenna antIn1 = (Antenna) ExitContolsList[exitContInUse];
					antIn1.SetData(reader, 1);
					antIn1.Visible = true;
					exitContInUse ++;
				}

				exitContInUse = Math.Max(enterContInUse, exitContInUse);
				enterContInUse = Math.Max(enterContInUse, exitContInUse);
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		internal void PopulateAntenna(ReaderTO reader, int antenaNum, string picturePath, string empoloyeeName, 
			string employeeID, bool entrancePermitted)
		{
			try
			{
				foreach(Antenna antenna in ControlsList)
				{
					if (antenna.AntennaReader.ReaderID.Equals(reader.ReaderID) && (antenna.AntNum.Equals(antenaNum)))
					{
						antenna.ShowData(picturePath, empoloyeeName, employeeID, entrancePermitted);
						writeToDatabase(int.Parse(employeeID), reader.ReaderID, antenaNum);
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void writeToDatabase(int employeeID, int readerID, int antennaNum)
		{
			EmployeeLocation emplLocation = new EmployeeLocation();
			try
			{				
				bool updated = emplLocation.Update(employeeID, readerID, antennaNum, 0, DateTime.Now, 0);
			}
			catch(Exception ex)
			{
				//log.writeLog(DateTime.Now + this.ToString() + ".populateControl() : " + ex.Message + "\n");
				if (ex.Message.Trim().Equals("2627"))
				{
					MessageBox.Show(rm.GetString("uniqueEmployeeExists", culture));
				}
				else
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void InitializeObserverClient()
		{
			observerClient = new NotificationObserverClient(this.ToString());
			Controller = NotificationController.GetInstance();	
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.MonitorEvent);
		}


		private void MonitorEvent(object sender, NotificationEventArgs args)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if ((args.reader != null) && (args.antennaNum != -1) && (args.tagID != 0))
                {
                    FillData(args.reader, args.antennaNum, args.tagID);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Monitor.MonitorEvent(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void FillData(ReaderTO reader, int antNum, UInt64 tagID)
		{
			try
			{
				int emplID = -1;
				EmployeeTO curreantEmployee = new EmployeeTO();

				foreach(TagTO tag in tags)
				{
					if(tag.TagID.Equals(tagID))
					{
						emplID = tag.OwnerID;
						break;
					}
				}
				
				if (!emplID.Equals(-1))
				{
					foreach(EmployeeTO empl in employees)
					{
						if (empl.EmployeeID.Equals(emplID))
						{
							curreantEmployee = empl;
							break;
						}
					}
					
					string path = Constants.EmployeePhotoDirectory;
					
					if (curreantEmployee.Picture.Trim().Equals("0"))
					{
						//path += ConfigurationManager.AppSettings["whiteheadPic"];
                        path += Constants.whiteheadImage;
					}
					else
					{
						path += curreantEmployee.Picture.Trim();
					}

					// Display 
					this.PopulateAntenna(reader, antNum, path, 
						curreantEmployee.FirstName + " " + curreantEmployee.LastName, curreantEmployee.EmployeeID.ToString(), true);
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".populateControl() : " + ex.Message);  
				throw ex;
			}
		}

        private void Monitor_KeyUp(object sender, KeyEventArgs e)
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
