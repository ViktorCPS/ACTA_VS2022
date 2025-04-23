using System;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for ExportLibraries.
	/// </summary>
	public class LibrariesExportImport : System.Windows.Forms.Form
	{
		private System.Windows.Forms.CheckBox cbLibEmployee;
		private System.Windows.Forms.CheckBox cbLibLocation;
		private System.Windows.Forms.CheckBox cbLibPassTypes;
		private System.Windows.Forms.CheckBox cbLibWorkingUnit;
		private System.Windows.Forms.CheckBox cbLibTags;
		private System.Windows.Forms.Button btnSave;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox cbReaders;

		// Debug log
		DebugLog log;
		ApplUserTO logInUser;

		private const int EXPORT = 0;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.Button btnClose;
		private const int IMPORT = 1;


		public LibrariesExportImport(int form)
		{
			InitializeComponent();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();
			// Open Export Form
			if (form == 0)
			{
				btnSave.Visible = true;
				btnImport.Visible = false;
			}

			// Open Import form
			if (form == 1)
			{
				cbLibEmployee.Enabled = false;
				cbLibTags.Enabled= false;
				cbLibWorkingUnit.Enabled = false;
				cbLibLocation.Enabled = false;
				cbLibPassTypes.Enabled = false;
				cbReaders.Enabled = false;

				btnSave.Visible = false;
				btnImport.Visible = true;

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
            this.cbLibEmployee = new System.Windows.Forms.CheckBox();
            this.cbLibTags = new System.Windows.Forms.CheckBox();
            this.cbLibWorkingUnit = new System.Windows.Forms.CheckBox();
            this.cbLibLocation = new System.Windows.Forms.CheckBox();
            this.cbLibPassTypes = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cbReaders = new System.Windows.Forms.CheckBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbLibEmployee
            // 
            this.cbLibEmployee.Location = new System.Drawing.Point(80, 32);
            this.cbLibEmployee.Name = "cbLibEmployee";
            this.cbLibEmployee.Size = new System.Drawing.Size(104, 24);
            this.cbLibEmployee.TabIndex = 0;
            this.cbLibEmployee.Text = "Employees";
            // 
            // cbLibTags
            // 
            this.cbLibTags.Location = new System.Drawing.Point(80, 64);
            this.cbLibTags.Name = "cbLibTags";
            this.cbLibTags.Size = new System.Drawing.Size(104, 24);
            this.cbLibTags.TabIndex = 1;
            this.cbLibTags.Text = "Tags";
            // 
            // cbLibWorkingUnit
            // 
            this.cbLibWorkingUnit.Location = new System.Drawing.Point(80, 96);
            this.cbLibWorkingUnit.Name = "cbLibWorkingUnit";
            this.cbLibWorkingUnit.Size = new System.Drawing.Size(104, 24);
            this.cbLibWorkingUnit.TabIndex = 2;
            this.cbLibWorkingUnit.Text = "Working Unit";
            // 
            // cbLibLocation
            // 
            this.cbLibLocation.Location = new System.Drawing.Point(80, 128);
            this.cbLibLocation.Name = "cbLibLocation";
            this.cbLibLocation.Size = new System.Drawing.Size(104, 24);
            this.cbLibLocation.TabIndex = 3;
            this.cbLibLocation.Text = "Locations";
            // 
            // cbLibPassTypes
            // 
            this.cbLibPassTypes.Location = new System.Drawing.Point(80, 160);
            this.cbLibPassTypes.Name = "cbLibPassTypes";
            this.cbLibPassTypes.Size = new System.Drawing.Size(104, 24);
            this.cbLibPassTypes.TabIndex = 4;
            this.cbLibPassTypes.Text = "Pass Types";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(64, 248);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Export";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(160, 248);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cbReaders
            // 
            this.cbReaders.Location = new System.Drawing.Point(80, 192);
            this.cbReaders.Name = "cbReaders";
            this.cbReaders.Size = new System.Drawing.Size(104, 24);
            this.cbReaders.TabIndex = 9;
            this.cbReaders.Text = "Readers";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(64, 248);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // LibrariesExportImport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 293);
            this.ControlBox = false;
            this.Controls.Add(this.cbReaders);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbLibPassTypes);
            this.Controls.Add(this.cbLibLocation);
            this.Controls.Add(this.cbLibWorkingUnit);
            this.Controls.Add(this.cbLibTags);
            this.Controls.Add(this.cbLibEmployee);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnSave);
            this.MaximumSize = new System.Drawing.Size(304, 320);
            this.MinimumSize = new System.Drawing.Size(304, 320);
            this.Name = "LibrariesExportImport";
            this.ShowInTaskbar = false;
            this.Text = "Export Libraries";
            this.ResumeLayout(false);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbLibEmployee.Checked)
                {
                    // Export Data from Database table to XML 
                    // files using serialization
                    Employee employee = new Employee();
                    employee.CacheAllData();
                }

                if (cbLibTags.Checked)
                {
                    Tag tag = new Tag();
                    tag.CacheAllData();
                }

                if (cbLibWorkingUnit.Checked)
                {
                    WorkingUnit wu = new WorkingUnit();
                    wu.CacheAllData();
                }

                if (cbLibLocation.Checked)
                {
                    Location location = new Location();
                    location.CacheAllData();
                }

                if (cbLibPassTypes.Checked)
                {
                    PassType passType = new PassType();
                    passType.CacheAllData();
                }

                if (cbReaders.Checked)
                {
                    Reader reader = new Reader();
                    reader.CacheAllData();
                }

                MessageBox.Show("Data Export succeeded!");
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LibrariesExportImport.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " LibrariesExportImport.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnImport_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				DataManager dataController = new DataManager();
				dataController.PushToDatabase();
				MessageBox.Show("Data Import succeeded!");
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " LibrariesExportImport.btnImport_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}
	}
}
