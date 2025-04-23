using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;
using ACTAConfigManipulation;

namespace UI
{
	/// <summary>
	/// Summary description for Info.
	/// </summary>
	public class Info : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox gbInfo;
		private System.Windows.Forms.Label lblDatabaseInfo;
		private System.Windows.Forms.Label lblLPath;
		private System.Windows.Forms.Label lblFilesPath;
		private System.Windows.Forms.Label lblRootPath;
		private System.Windows.Forms.Label lblDatabaseInfoVal;
		private System.Windows.Forms.Label lblLPathVal;
		private System.Windows.Forms.Label lblFilesPathVal;
		private System.Windows.Forms.Label lblRootPathVal;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public NotificationController Controller;
        // Observer client instance
        public NotificationObserverClient observerClient;

        EmployeeImageFile eif = new EmployeeImageFile();

		DebugLog log;	
		ApplUserTO logInUser;
		ResourceManager rm;
        private Button btnDBSetup;				
		private CultureInfo culture;
        private Button btnGateSetup;
        private Button btnGenerateLicReq;
        private bool dbChanged = true;
        private Button btnCloseSessions;

		public Info()
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				logInUser = NotificationController.GetLogInUser();

                InitialiseObserverClient();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("UI.Resource",typeof(Info).Assembly);
				setLanguage();

				lblDatabaseInfoVal.Text = Constants.DatabaseInfoString;
				lblLPathVal.Text = Constants.LPath;
                if (eif.SearchCount(-1) < 0)
                {
                    lblFilesPathVal.Text = Constants.EmployeePhotoDirectory;
                }
                else
                {
                    lblFilesPathVal.Text = Constants.GetDatabaseString + "_files"; ;
                }

				lblRootPathVal.Text = Constants.RootDirectory;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Info.Info(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        public void InitialiseObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
            Controller.AttachToNotifier(observerClient);
            this.observerClient.Notification += new NotificationEventHandler(this.DBChangedEvent);
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
            this.lblDatabaseInfo = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.lblRootPathVal = new System.Windows.Forms.Label();
            this.lblFilesPathVal = new System.Windows.Forms.Label();
            this.lblLPathVal = new System.Windows.Forms.Label();
            this.lblRootPath = new System.Windows.Forms.Label();
            this.lblFilesPath = new System.Windows.Forms.Label();
            this.lblLPath = new System.Windows.Forms.Label();
            this.lblDatabaseInfoVal = new System.Windows.Forms.Label();
            this.btnDBSetup = new System.Windows.Forms.Button();
            this.btnGateSetup = new System.Windows.Forms.Button();
            this.btnGenerateLicReq = new System.Windows.Forms.Button();
            this.btnCloseSessions = new System.Windows.Forms.Button();
            this.gbInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDatabaseInfo
            // 
            this.lblDatabaseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabaseInfo.Location = new System.Drawing.Point(9, 32);
            this.lblDatabaseInfo.Name = "lblDatabaseInfo";
            this.lblDatabaseInfo.Size = new System.Drawing.Size(119, 23);
            this.lblDatabaseInfo.TabIndex = 0;
            this.lblDatabaseInfo.Text = "Database info:";
            this.lblDatabaseInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(507, 301);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.lblRootPathVal);
            this.gbInfo.Controls.Add(this.lblFilesPathVal);
            this.gbInfo.Controls.Add(this.lblLPathVal);
            this.gbInfo.Controls.Add(this.lblRootPath);
            this.gbInfo.Controls.Add(this.lblFilesPath);
            this.gbInfo.Controls.Add(this.lblLPath);
            this.gbInfo.Controls.Add(this.lblDatabaseInfoVal);
            this.gbInfo.Controls.Add(this.lblDatabaseInfo);
            this.gbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInfo.Location = new System.Drawing.Point(10, 16);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(572, 175);
            this.gbInfo.TabIndex = 0;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "Info";
            // 
            // lblRootPathVal
            // 
            this.lblRootPathVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRootPathVal.Location = new System.Drawing.Point(142, 128);
            this.lblRootPathVal.Name = "lblRootPathVal";
            this.lblRootPathVal.Size = new System.Drawing.Size(424, 23);
            this.lblRootPathVal.TabIndex = 7;
            this.lblRootPathVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFilesPathVal
            // 
            this.lblFilesPathVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilesPathVal.Location = new System.Drawing.Point(145, 96);
            this.lblFilesPathVal.Name = "lblFilesPathVal";
            this.lblFilesPathVal.Size = new System.Drawing.Size(421, 23);
            this.lblFilesPathVal.TabIndex = 5;
            this.lblFilesPathVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLPathVal
            // 
            this.lblLPathVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLPathVal.Location = new System.Drawing.Point(142, 64);
            this.lblLPathVal.Name = "lblLPathVal";
            this.lblLPathVal.Size = new System.Drawing.Size(424, 23);
            this.lblLPathVal.TabIndex = 3;
            this.lblLPathVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRootPath
            // 
            this.lblRootPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRootPath.Location = new System.Drawing.Point(6, 128);
            this.lblRootPath.Name = "lblRootPath";
            this.lblRootPath.Size = new System.Drawing.Size(122, 23);
            this.lblRootPath.TabIndex = 6;
            this.lblRootPath.Text = "Root path:";
            this.lblRootPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFilesPath
            // 
            this.lblFilesPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilesPath.Location = new System.Drawing.Point(6, 96);
            this.lblFilesPath.Name = "lblFilesPath";
            this.lblFilesPath.Size = new System.Drawing.Size(122, 23);
            this.lblFilesPath.TabIndex = 4;
            this.lblFilesPath.Text = "Photos path:";
            this.lblFilesPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLPath
            // 
            this.lblLPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLPath.Location = new System.Drawing.Point(6, 64);
            this.lblLPath.Name = "lblLPath";
            this.lblLPath.Size = new System.Drawing.Size(122, 23);
            this.lblLPath.TabIndex = 2;
            this.lblLPath.Text = "L path:";
            this.lblLPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDatabaseInfoVal
            // 
            this.lblDatabaseInfoVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabaseInfoVal.Location = new System.Drawing.Point(142, 32);
            this.lblDatabaseInfoVal.Name = "lblDatabaseInfoVal";
            this.lblDatabaseInfoVal.Size = new System.Drawing.Size(424, 23);
            this.lblDatabaseInfoVal.TabIndex = 1;
            this.lblDatabaseInfoVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDBSetup
            // 
            this.btnDBSetup.Location = new System.Drawing.Point(10, 258);
            this.btnDBSetup.Name = "btnDBSetup";
            this.btnDBSetup.Size = new System.Drawing.Size(227, 23);
            this.btnDBSetup.TabIndex = 3;
            this.btnDBSetup.Text = "DBSetup";
            this.btnDBSetup.UseVisualStyleBackColor = true;
            this.btnDBSetup.Click += new System.EventHandler(this.btnDBSetup_Click);
            // 
            // btnGateSetup
            // 
            this.btnGateSetup.Location = new System.Drawing.Point(276, 258);
            this.btnGateSetup.Name = "btnGateSetup";
            this.btnGateSetup.Size = new System.Drawing.Size(196, 23);
            this.btnGateSetup.TabIndex = 4;
            this.btnGateSetup.Text = "Gate parameters setup";
            this.btnGateSetup.UseVisualStyleBackColor = true;
            this.btnGateSetup.Click += new System.EventHandler(this.btnGateSetup_Click);
            // 
            // btnGenerateLicReq
            // 
            this.btnGenerateLicReq.Location = new System.Drawing.Point(10, 212);
            this.btnGenerateLicReq.Name = "btnGenerateLicReq";
            this.btnGenerateLicReq.Size = new System.Drawing.Size(227, 23);
            this.btnGenerateLicReq.TabIndex = 1;
            this.btnGenerateLicReq.Text = "Generate licence request";
            this.btnGenerateLicReq.UseVisualStyleBackColor = true;
            this.btnGenerateLicReq.Click += new System.EventHandler(this.btnGenerateLicReq_Click);
            // 
            // btnCloseSessions
            // 
            this.btnCloseSessions.Location = new System.Drawing.Point(276, 212);
            this.btnCloseSessions.Name = "btnCloseSessions";
            this.btnCloseSessions.Size = new System.Drawing.Size(196, 23);
            this.btnCloseSessions.TabIndex = 2;
            this.btnCloseSessions.Text = "Close opened sessions";
            this.btnCloseSessions.UseVisualStyleBackColor = true;
            this.btnCloseSessions.Click += new System.EventHandler(this.btnCloseSessions_Click);
            // 
            // Info
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(592, 336);
            this.ControlBox = false;
            this.Controls.Add(this.btnCloseSessions);
            this.Controls.Add(this.btnGenerateLicReq);
            this.Controls.Add(this.btnGateSetup);
            this.Controls.Add(this.btnDBSetup);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.btnClose);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 363);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 363);
            this.Name = "Info";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Info";
            this.gbInfo.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Set proper language and initialize List View
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("infoForm", culture);

				// group box text
				gbInfo.Text = rm.GetString("gbInfo", culture);

				// button's text
                btnGenerateLicReq.Text = rm.GetString("btnGenerateLicReq", culture);
                btnCloseSessions.Text = rm.GetString("btnCloseSessions", culture);
                btnDBSetup.Text = rm.GetString("btnDBSetup", culture);
                btnGateSetup.Text = rm.GetString("btnGateSetup", culture);
				btnClose.Text  = rm.GetString("btnClose", culture);

				// label's text				
				lblDatabaseInfo.Text = rm.GetString("lblDatabaseInfo", culture);
				lblLPath.Text        = rm.GetString("lblLPath", culture);
				lblFilesPath.Text   = rm.GetString("lblFilesPath", culture);
				lblRootPath.Text     = rm.GetString("lblRootPath", culture);		
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Info.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Info.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void btnDBSetup_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string dataBaseInfo = Constants.DatabaseInfoString;

                string dataProvider = "";
                string server = "";
                string port = "";
                string database = "";

                int startIndex = -1;
                int endIndex = -1;

                //calculate data provider
                startIndex = dataBaseInfo.ToLower().IndexOf("data provider=");
                if (startIndex >= 0)
                {
                    endIndex = dataBaseInfo.IndexOf(";", startIndex);
                    if (endIndex >= startIndex)
                        dataProvider = dataBaseInfo.Substring(startIndex + 14, endIndex - startIndex - 13);
                }

                //calculate server
                startIndex = dataBaseInfo.ToLower().IndexOf("server=");
                if (startIndex >= 0)
                {
                    endIndex = dataBaseInfo.IndexOf(";", startIndex);
                    if (endIndex >= startIndex)
                        server = dataBaseInfo.Substring(startIndex + 7, endIndex - startIndex - 7);
                }

                //calculate port
                startIndex = dataBaseInfo.ToLower().IndexOf("port=");
                if (startIndex >= 0)
                {
                    endIndex = dataBaseInfo.IndexOf(";", startIndex);
                    if (endIndex >= startIndex)
                        port = dataBaseInfo.Substring(startIndex + 5, endIndex - startIndex - 5);
                }

                //calculatedatabase
                startIndex = dataBaseInfo.ToLower().IndexOf("database=");
                if (startIndex >= 0)
                {
                    endIndex = dataBaseInfo.IndexOf(";", startIndex);
                    if (endIndex >= startIndex)
                        database = dataBaseInfo.Substring(startIndex + 9, endIndex - startIndex - 9);
                }

                DBSetup dbSetup = new DBSetup(dataProvider, server, port, database, Constants.dbSetupInfo);
                dbSetup.ShowDialog(this);

                if (dbChanged)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Info.btnDBSetup_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void DBChangedEvent(object sender, NotificationEventArgs args)
        {
            this.dbChanged = args.dbChanged;
        }

        private void btnGateSetup_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ConfigAdd confAdd = new ConfigAdd("");
                confAdd.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Info.btnGateSetup_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerateLicReq_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string server = Controller.getDBServerName();
                string port = Controller.getDBServerPort();
                bool licReqCreated = Common.Misc.generateLicenceRequest(server, port);

                this.Cursor = Cursors.Arrow;
                if (licReqCreated)
                {
                    MessageBox.Show(rm.GetString("licreqCreated", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("licreqNotCreated", culture));
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " Info.btnGenerateLicReq_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCloseSessions_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<ApplUserLogTO> usersLog = new ApplUserLog().SearchOpenSessions("", "", Constants.UserLoginChanel.DESKTOP.ToString());

                bool isUpdated = true;
                ApplUserLog applUserLog = new ApplUserLog();
                foreach (ApplUserLogTO userLog in usersLog)
                {
                    applUserLog.UserLogTO = userLog;
                    isUpdated = (applUserLog.Update(Constants.autoUser, "") >= 0 ? true : false) && isUpdated;

                    if (!isUpdated)
                        break;
                }

                this.Cursor = Cursors.Arrow;

                if (isUpdated)
                {
                    MessageBox.Show(rm.GetString("sessionsClosed", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("sessionsNotClosed", culture));
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " Info.btnCloseSessions_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
	}
}
