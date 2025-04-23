using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;

using Util;
using Common;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for ReaderSetup.
	/// </summary>
	public class ReaderSetup : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblReaders;
		private System.Windows.Forms.ComboBox cbReaders;
		private System.Windows.Forms.Button btnSetup;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		DebugLog log;
		List<ReaderTO> readers = new List<ReaderTO>();

		public ReaderSetup()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
			InitializeComponent();

			populateReaderList();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReaderSetup));
            this.btnSetup = new System.Windows.Forms.Button();
            this.cbReaders = new System.Windows.Forms.ComboBox();
            this.lblReaders = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(96, 208);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(75, 23);
            this.btnSetup.TabIndex = 2;
            this.btnSetup.Text = "Setup";
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // cbReaders
            // 
            this.cbReaders.Location = new System.Drawing.Point(64, 88);
            this.cbReaders.Name = "cbReaders";
            this.cbReaders.Size = new System.Drawing.Size(144, 21);
            this.cbReaders.TabIndex = 1;
            // 
            // lblReaders
            // 
            this.lblReaders.Location = new System.Drawing.Point(72, 56);
            this.lblReaders.Name = "lblReaders";
            this.lblReaders.Size = new System.Drawing.Size(100, 23);
            this.lblReaders.TabIndex = 2;
            this.lblReaders.Text = "Terminal:";
            this.lblReaders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReaderSetup
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.lblReaders);
            this.Controls.Add(this.cbReaders);
            this.Controls.Add(this.btnSetup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReaderSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Terminal setup";
            this.ResumeLayout(false);

		}
		#endregion

		private void populateReaderList()
		{
			try
			{				
				this.readers = new Reader().Search();

				cbReaders.DataSource = readers;
				cbReaders.DisplayMember = "Description";
				cbReaders.ValueMember = "ReaderID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Exception in: " + 
				this.ToString() + ".populateReaderList() : " + ex.Message + "\n");  
			}
		}

		private void btnSetup_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                CultureInfo culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                ReaderTO currentReader = (ReaderTO)cbReaders.SelectedItem;
                Process.Start(Constants.RegistryDataApplication, currentReader.ConnectionAddress + " " + currentReader.TechType.ToUpper() + " " + culture.TwoLetterISOLanguageName);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".btnSetup_Click() : " + ex.Message + "\n");
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}
	}
}
