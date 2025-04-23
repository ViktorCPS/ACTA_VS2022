using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using TransferObjects;
using Common;
using Util;

using System.Resources;
using System.Globalization;

namespace UI
{
	/// <summary>
	/// Summary description for ReaderAdvanceSettings.
	/// </summary>
	public class ReaderAdvanceSettings : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbAntenna0;
		private System.Windows.Forms.GroupBox gbAntenna1;
		private System.Windows.Forms.Label lblA0PrimaryLoc;
		private System.Windows.Forms.Label lblA0SecondaryLoc;
		private System.Windows.Forms.Label lblA0SecDirection;
		private System.Windows.Forms.Label lblA0PrimDirection;
		private System.Windows.Forms.Label lblA1SecDirection;
		private System.Windows.Forms.Label lblA1SecLoc;
		private System.Windows.Forms.Label lblA1PrimDirection;
		private System.Windows.Forms.Label lblA1PrimLoc;
		private System.Windows.Forms.Button btnReaderSettings;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ComboBox cbA0SecDir;
		private System.Windows.Forms.ComboBox cbA0SecLoc;
		private System.Windows.Forms.ComboBox cbA0PrimDir;
		private System.Windows.Forms.ComboBox cbA0PrimLoc;
		private System.Windows.Forms.ComboBox cbA1SecDir;
		private System.Windows.Forms.ComboBox cbA1SecLoc;
		private System.Windows.Forms.ComboBox cbA1PrimDir;
		private System.Windows.Forms.ComboBox cbA1PrimLoc;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public ReaderTO currentReader;

		// Controller instance
		public NotificationController Controller;
		// Observer client instance
		public NotificationObserverClient observerClient;


		private CultureInfo culture;
		private ResourceManager rm;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Label lblGate1;
		private System.Windows.Forms.ComboBox cbGate1;
		private System.Windows.Forms.ComboBox cbGate0;
		private System.Windows.Forms.Label lblGate0;

		DebugLog log;
		ApplUserTO logInUser;

		/// <summary>
		/// Consturctor - receive Reader object to set some extra parameters
		/// and turn object back to parent form to be saved.
		/// </summary>
		/// <param name="currentReader">Current Reader object</param>
		public ReaderAdvanceSettings(ReaderTO currentParentReader, int a0GateID, int a0PrimLoc, string a0PrimDir,
			int a1GateID, int a1PrimLoc, string a1PrimDir)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(ReaderAdvanceSettings).Assembly);
			setLanguage();
			logInUser = NotificationController.GetLogInUser();

			currentReader = currentParentReader;
			currentReader.A0GateID = a0GateID;
			currentReader.A0LocID = a0PrimLoc;
			currentReader.A0Direction = a0PrimDir;

			currentReader.A1GateID = a1GateID;
			currentReader.A1LocID = a1PrimLoc;
			currentReader.A1Direction = a1PrimDir;

			this.cbA0PrimDir.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbA0PrimLoc.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbA1PrimDir.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbA1PrimLoc.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbA0SecDir.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbA0SecLoc.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbA1SecLoc.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbA1SecDir.DropDownStyle = ComboBoxStyle.DropDownList;

			this.CenterToScreen();
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
            this.gbAntenna0 = new System.Windows.Forms.GroupBox();
            this.cbGate0 = new System.Windows.Forms.ComboBox();
            this.lblGate0 = new System.Windows.Forms.Label();
            this.cbA0SecDir = new System.Windows.Forms.ComboBox();
            this.lblA0SecDirection = new System.Windows.Forms.Label();
            this.cbA0SecLoc = new System.Windows.Forms.ComboBox();
            this.lblA0SecondaryLoc = new System.Windows.Forms.Label();
            this.cbA0PrimDir = new System.Windows.Forms.ComboBox();
            this.lblA0PrimDirection = new System.Windows.Forms.Label();
            this.cbA0PrimLoc = new System.Windows.Forms.ComboBox();
            this.lblA0PrimaryLoc = new System.Windows.Forms.Label();
            this.gbAntenna1 = new System.Windows.Forms.GroupBox();
            this.cbGate1 = new System.Windows.Forms.ComboBox();
            this.lblGate1 = new System.Windows.Forms.Label();
            this.cbA1SecDir = new System.Windows.Forms.ComboBox();
            this.lblA1SecDirection = new System.Windows.Forms.Label();
            this.cbA1SecLoc = new System.Windows.Forms.ComboBox();
            this.lblA1SecLoc = new System.Windows.Forms.Label();
            this.cbA1PrimDir = new System.Windows.Forms.ComboBox();
            this.lblA1PrimDirection = new System.Windows.Forms.Label();
            this.cbA1PrimLoc = new System.Windows.Forms.ComboBox();
            this.lblA1PrimLoc = new System.Windows.Forms.Label();
            this.btnReaderSettings = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.gbAntenna0.SuspendLayout();
            this.gbAntenna1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAntenna0
            // 
            this.gbAntenna0.Controls.Add(this.cbGate0);
            this.gbAntenna0.Controls.Add(this.lblGate0);
            this.gbAntenna0.Controls.Add(this.cbA0SecDir);
            this.gbAntenna0.Controls.Add(this.lblA0SecDirection);
            this.gbAntenna0.Controls.Add(this.cbA0SecLoc);
            this.gbAntenna0.Controls.Add(this.lblA0SecondaryLoc);
            this.gbAntenna0.Controls.Add(this.cbA0PrimDir);
            this.gbAntenna0.Controls.Add(this.lblA0PrimDirection);
            this.gbAntenna0.Controls.Add(this.cbA0PrimLoc);
            this.gbAntenna0.Controls.Add(this.lblA0PrimaryLoc);
            this.gbAntenna0.Location = new System.Drawing.Point(16, 16);
            this.gbAntenna0.Name = "gbAntenna0";
            this.gbAntenna0.Size = new System.Drawing.Size(384, 192);
            this.gbAntenna0.TabIndex = 0;
            this.gbAntenna0.TabStop = false;
            this.gbAntenna0.Text = "Antenna 0";
            // 
            // cbGate0
            // 
            this.cbGate0.Enabled = false;
            this.cbGate0.Location = new System.Drawing.Point(144, 16);
            this.cbGate0.Name = "cbGate0";
            this.cbGate0.Size = new System.Drawing.Size(224, 21);
            this.cbGate0.TabIndex = 3;
            // 
            // lblGate0
            // 
            this.lblGate0.Location = new System.Drawing.Point(32, 16);
            this.lblGate0.Name = "lblGate0";
            this.lblGate0.Size = new System.Drawing.Size(100, 23);
            this.lblGate0.TabIndex = 2;
            this.lblGate0.Text = "Gate:";
            this.lblGate0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA0SecDir
            // 
            this.cbA0SecDir.Location = new System.Drawing.Point(144, 160);
            this.cbA0SecDir.Name = "cbA0SecDir";
            this.cbA0SecDir.Size = new System.Drawing.Size(224, 21);
            this.cbA0SecDir.TabIndex = 11;
            // 
            // lblA0SecDirection
            // 
            this.lblA0SecDirection.Location = new System.Drawing.Point(16, 160);
            this.lblA0SecDirection.Name = "lblA0SecDirection";
            this.lblA0SecDirection.Size = new System.Drawing.Size(120, 23);
            this.lblA0SecDirection.TabIndex = 10;
            this.lblA0SecDirection.Text = "Direction:";
            this.lblA0SecDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA0SecLoc
            // 
            this.cbA0SecLoc.Location = new System.Drawing.Point(144, 128);
            this.cbA0SecLoc.Name = "cbA0SecLoc";
            this.cbA0SecLoc.Size = new System.Drawing.Size(224, 21);
            this.cbA0SecLoc.TabIndex = 9;
            // 
            // lblA0SecondaryLoc
            // 
            this.lblA0SecondaryLoc.Location = new System.Drawing.Point(16, 128);
            this.lblA0SecondaryLoc.Name = "lblA0SecondaryLoc";
            this.lblA0SecondaryLoc.Size = new System.Drawing.Size(120, 23);
            this.lblA0SecondaryLoc.TabIndex = 8;
            this.lblA0SecondaryLoc.Text = "Secondary Location:";
            this.lblA0SecondaryLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA0PrimDir
            // 
            this.cbA0PrimDir.Enabled = false;
            this.cbA0PrimDir.Location = new System.Drawing.Point(144, 88);
            this.cbA0PrimDir.Name = "cbA0PrimDir";
            this.cbA0PrimDir.Size = new System.Drawing.Size(224, 21);
            this.cbA0PrimDir.TabIndex = 7;
            // 
            // lblA0PrimDirection
            // 
            this.lblA0PrimDirection.Location = new System.Drawing.Point(24, 88);
            this.lblA0PrimDirection.Name = "lblA0PrimDirection";
            this.lblA0PrimDirection.Size = new System.Drawing.Size(112, 23);
            this.lblA0PrimDirection.TabIndex = 6;
            this.lblA0PrimDirection.Text = "Direction:";
            this.lblA0PrimDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA0PrimLoc
            // 
            this.cbA0PrimLoc.Enabled = false;
            this.cbA0PrimLoc.Location = new System.Drawing.Point(144, 56);
            this.cbA0PrimLoc.Name = "cbA0PrimLoc";
            this.cbA0PrimLoc.Size = new System.Drawing.Size(224, 21);
            this.cbA0PrimLoc.TabIndex = 5;
            // 
            // lblA0PrimaryLoc
            // 
            this.lblA0PrimaryLoc.Location = new System.Drawing.Point(24, 56);
            this.lblA0PrimaryLoc.Name = "lblA0PrimaryLoc";
            this.lblA0PrimaryLoc.Size = new System.Drawing.Size(112, 23);
            this.lblA0PrimaryLoc.TabIndex = 4;
            this.lblA0PrimaryLoc.Text = "Primary Location:";
            this.lblA0PrimaryLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbAntenna1
            // 
            this.gbAntenna1.Controls.Add(this.cbGate1);
            this.gbAntenna1.Controls.Add(this.lblGate1);
            this.gbAntenna1.Controls.Add(this.cbA1SecDir);
            this.gbAntenna1.Controls.Add(this.lblA1SecDirection);
            this.gbAntenna1.Controls.Add(this.cbA1SecLoc);
            this.gbAntenna1.Controls.Add(this.lblA1SecLoc);
            this.gbAntenna1.Controls.Add(this.cbA1PrimDir);
            this.gbAntenna1.Controls.Add(this.lblA1PrimDirection);
            this.gbAntenna1.Controls.Add(this.cbA1PrimLoc);
            this.gbAntenna1.Controls.Add(this.lblA1PrimLoc);
            this.gbAntenna1.Location = new System.Drawing.Point(16, 224);
            this.gbAntenna1.Name = "gbAntenna1";
            this.gbAntenna1.Size = new System.Drawing.Size(384, 192);
            this.gbAntenna1.TabIndex = 12;
            this.gbAntenna1.TabStop = false;
            this.gbAntenna1.Text = "Antenna 1";
            // 
            // cbGate1
            // 
            this.cbGate1.Enabled = false;
            this.cbGate1.Location = new System.Drawing.Point(144, 16);
            this.cbGate1.Name = "cbGate1";
            this.cbGate1.Size = new System.Drawing.Size(224, 21);
            this.cbGate1.TabIndex = 15;
            // 
            // lblGate1
            // 
            this.lblGate1.Location = new System.Drawing.Point(32, 16);
            this.lblGate1.Name = "lblGate1";
            this.lblGate1.Size = new System.Drawing.Size(100, 23);
            this.lblGate1.TabIndex = 14;
            this.lblGate1.Text = "Gate:";
            this.lblGate1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA1SecDir
            // 
            this.cbA1SecDir.Location = new System.Drawing.Point(144, 160);
            this.cbA1SecDir.Name = "cbA1SecDir";
            this.cbA1SecDir.Size = new System.Drawing.Size(224, 21);
            this.cbA1SecDir.TabIndex = 23;
            // 
            // lblA1SecDirection
            // 
            this.lblA1SecDirection.Location = new System.Drawing.Point(16, 160);
            this.lblA1SecDirection.Name = "lblA1SecDirection";
            this.lblA1SecDirection.Size = new System.Drawing.Size(120, 23);
            this.lblA1SecDirection.TabIndex = 22;
            this.lblA1SecDirection.Text = "Direction:";
            this.lblA1SecDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA1SecLoc
            // 
            this.cbA1SecLoc.Location = new System.Drawing.Point(144, 128);
            this.cbA1SecLoc.Name = "cbA1SecLoc";
            this.cbA1SecLoc.Size = new System.Drawing.Size(224, 21);
            this.cbA1SecLoc.TabIndex = 21;
            // 
            // lblA1SecLoc
            // 
            this.lblA1SecLoc.Location = new System.Drawing.Point(16, 128);
            this.lblA1SecLoc.Name = "lblA1SecLoc";
            this.lblA1SecLoc.Size = new System.Drawing.Size(120, 23);
            this.lblA1SecLoc.TabIndex = 20;
            this.lblA1SecLoc.Text = "Secondary Location:";
            this.lblA1SecLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA1PrimDir
            // 
            this.cbA1PrimDir.Enabled = false;
            this.cbA1PrimDir.Location = new System.Drawing.Point(144, 88);
            this.cbA1PrimDir.Name = "cbA1PrimDir";
            this.cbA1PrimDir.Size = new System.Drawing.Size(224, 21);
            this.cbA1PrimDir.TabIndex = 19;
            // 
            // lblA1PrimDirection
            // 
            this.lblA1PrimDirection.Location = new System.Drawing.Point(16, 88);
            this.lblA1PrimDirection.Name = "lblA1PrimDirection";
            this.lblA1PrimDirection.Size = new System.Drawing.Size(120, 23);
            this.lblA1PrimDirection.TabIndex = 18;
            this.lblA1PrimDirection.Text = "Direction:";
            this.lblA1PrimDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbA1PrimLoc
            // 
            this.cbA1PrimLoc.Enabled = false;
            this.cbA1PrimLoc.Location = new System.Drawing.Point(144, 56);
            this.cbA1PrimLoc.Name = "cbA1PrimLoc";
            this.cbA1PrimLoc.Size = new System.Drawing.Size(224, 21);
            this.cbA1PrimLoc.TabIndex = 17;
            // 
            // lblA1PrimLoc
            // 
            this.lblA1PrimLoc.Location = new System.Drawing.Point(16, 56);
            this.lblA1PrimLoc.Name = "lblA1PrimLoc";
            this.lblA1PrimLoc.Size = new System.Drawing.Size(120, 23);
            this.lblA1PrimLoc.TabIndex = 16;
            this.lblA1PrimLoc.Text = "Primary Location:";
            this.lblA1PrimLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnReaderSettings
            // 
            this.btnReaderSettings.Location = new System.Drawing.Point(16, 432);
            this.btnReaderSettings.Name = "btnReaderSettings";
            this.btnReaderSettings.Size = new System.Drawing.Size(176, 23);
            this.btnReaderSettings.TabIndex = 24;
            this.btnReaderSettings.Text = "Show Reader Settings >>>";
            this.btnReaderSettings.Click += new System.EventHandler(this.btnReaderSettings_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(232, 432);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 25;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(328, 432);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(416, 432);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(96, 23);
            this.btnClear.TabIndex = 27;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // ReaderAdvanceSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(520, 462);
            this.ControlBox = false;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnReaderSettings);
            this.Controls.Add(this.gbAntenna1);
            this.Controls.Add(this.gbAntenna0);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(528, 496);
            this.MinimumSize = new System.Drawing.Size(528, 496);
            this.Name = "ReaderAdvanceSettings";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Reader Advance Settings";
            this.Load += new System.EventHandler(this.ReaderAdvanceSettings_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ReaderAdvanceSettings_KeyUp);
            this.gbAntenna0.ResumeLayout(false);
            this.gbAntenna1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void setLanguage()
		{
			try
			{
				
				this.Text = rm.GetString("readerAdvSettingsForm", culture);

				gbAntenna0.Text = rm.GetString("gbAntenna0", culture);
				gbAntenna1.Text = rm.GetString("gbAntenna1", culture);

                lblGate0.Text = rm.GetString("lblGate", culture);
				lblA0PrimaryLoc.Text = rm.GetString("lblPrimaryLoc", culture);
				lblA0PrimDirection.Text = rm.GetString("lblPrimDirect", culture);
				lblA0SecondaryLoc.Text = rm.GetString("secLocation", culture);
				lblA0SecDirection.Text = rm.GetString("lblPrimDirect", culture);

                lblGate1.Text = rm.GetString("lblGate", culture);
				lblA1PrimLoc.Text = rm.GetString("lblPrimaryLoc", culture);
				lblA1PrimDirection.Text = rm.GetString("lblPrimDirect", culture);
				lblA1SecLoc.Text = rm.GetString("secLocation", culture);
				lblA1SecDirection.Text = rm.GetString("lblPrimDirect", culture);

				btnReaderSettings.Text = rm.GetString("btnReaderSettings", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnOK.Text = rm.GetString("btnOK", culture);
				btnClear.Text = rm.GetString("btnFormClear", culture);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderAdvanceSettings.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populateLocationCb(ComboBox cb)
		{
			try
			{
				Location location = new Location();
                location.LocTO.Status = Constants.DefaultStateActive.Trim();
				List<LocationTO> locations = location.Search();

				locations.Insert(0, new LocationTO(-1, rm.GetString("all", culture), "", -1, -1, Constants.DefaultStateActive.ToString()));

				cb.DataSource = locations;
				cb.DisplayMember = "Name";
				cb.ValueMember = "LocationID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + "ReaderAdvanceSettings.populateLocationCb(...): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}

		private void populateGateCb(ComboBox cb)
		{
			try
			{				
				List<GateTO> gates = new Gate().Search();

				gates.Insert(0, new GateTO(-1, rm.GetString("all", culture), "", new DateTime(0), -1, -1));

				cb.DataSource = gates;
				cb.DisplayMember = "Name";
				cb.ValueMember = "GateID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReaderAdd.populateGateCb(...): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}

		private void ReaderAdvanceSettings_Load(object sender, System.EventArgs e)
		{
			InitialiseObserverClient();

			// Gate Combo Boxes
			populateGateCb(cbGate0);
			populateGateCb(cbGate1);

			// Locationa Combo Boxes
			populateLocationCb(cbA0PrimLoc);
			populateLocationCb(cbA0SecLoc);
			populateLocationCb(cbA1PrimLoc);
			populateLocationCb(cbA1SecLoc);

			// Direction Combo Boxes
			populateDirectionCb(cbA0PrimDir);
			populateDirectionCb(cbA0SecDir);
			populateDirectionCb(cbA1PrimDir);
			populateDirectionCb(cbA1SecDir);

			InitializeForm();
		}

		private void populateDirectionCb(ComboBox cb)
		{
			try
			{
				cb.Items.Add(rm.GetString("all", culture));
				cb.Items.Add(Constants.DirectionIn);
				cb.Items.Add(Constants.DirectionOut);
				//cb.Items.Add(Constants.DirectionInOut);

				cb.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReaderAdvanceSettings.populateDirectionCb(ComboBox cb): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}
		
		/// <summary>
		/// Populate Current Reader object with form's data and pass object 
		/// it to parent form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if ((cbA0SecLoc.SelectedIndex > 0 && cbA0SecDir.SelectedIndex <= 0)
                    || (cbA0SecLoc.SelectedIndex <= 0 && cbA0SecDir.SelectedIndex > 0))
                {
                    MessageBox.Show(rm.GetString("messageReaderA0SecLocDir", culture));
                    return;
                }

                if ((cbA1SecLoc.SelectedIndex > 0 && cbA1SecDir.SelectedIndex <= 0)
                    || (cbA1SecLoc.SelectedIndex <= 0 && cbA1SecDir.SelectedIndex > 0))
                {
                    MessageBox.Show(rm.GetString("messageReaderA1SecLocDir", culture));
                    return;
                }

                // Antenna 0

                if (cbGate0.SelectedIndex > 0)
                {
                    currentReader.A0GateID = (int)cbGate0.SelectedValue;
                }

                if (cbA0PrimLoc.SelectedIndex > 0)
                {
                    currentReader.A0LocID = (int)cbA0PrimLoc.SelectedValue;
                }

                if (cbA0PrimDir.SelectedIndex > 0)
                {
                    currentReader.A0Direction = cbA0PrimDir.SelectedItem.ToString();
                }

                if (cbA0SecLoc.SelectedIndex > 0)
                {
                    currentReader.A0SecLocID = (int)cbA0SecLoc.SelectedValue;
                }
                else
                    currentReader.A0SecLocID = -1;

                if (cbA0SecDir.SelectedIndex > 0)
                {
                    currentReader.A0SecDirection = cbA0SecDir.SelectedItem.ToString();
                }
                else
                    currentReader.A0SecDirection = "";

                // Antenna 1

                if (cbGate1.SelectedIndex > 0)
                {
                    currentReader.A1GateID = (int)cbGate1.SelectedValue;
                }

                if (cbA1PrimLoc.SelectedIndex != 0)
                {
                    currentReader.A1LocID = (int)cbA1PrimLoc.SelectedValue;
                }

                if (cbA1PrimDir.SelectedIndex != 0)
                {
                    currentReader.A1Direction = cbA1PrimDir.SelectedItem.ToString();
                }

                if (cbA1SecLoc.SelectedIndex != 0)
                {
                    currentReader.A1SecLocID = (int)cbA1SecLoc.SelectedValue;
                }
                else
                    currentReader.A1SecLocID = -1;

                if (cbA1SecDir.SelectedIndex != 0)
                {
                    currentReader.A1SecDirection = cbA1SecDir.SelectedItem.ToString();
                }
                else
                    currentReader.A1SecDirection = "";

                Controller.SettingsChanged(currentReader);
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ReaderAdvanceSettings.btnOK_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
			
		}

		public void InitialiseObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
		}

		private void InitializeForm()
		{
			try
			{
				cbGate0.SelectedValue = currentReader.A0GateID;
				cbA0PrimLoc.SelectedValue = currentReader.A0LocID;
				cbA0SecLoc.SelectedValue = currentReader.A0SecLocID;
				if (cbA0PrimDir.FindStringExact(currentReader.A0Direction) > 0)
				{
					cbA0PrimDir.SelectedIndex = cbA0PrimDir.FindStringExact(currentReader.A0Direction);
				}
				else
				{
					cbA0PrimDir.SelectedIndex = 0;
				}

				if (cbA0SecDir.FindStringExact(currentReader.A0SecDirection) > 0)
				{
					cbA0SecDir.SelectedIndex = cbA0SecDir.FindStringExact(currentReader.A0SecDirection);
				}
				else
				{
					cbA0SecDir.SelectedIndex = 0;
				}

				cbGate1.SelectedValue = currentReader.A1GateID;
				cbA1PrimLoc.SelectedValue = currentReader.A1LocID;
				cbA1SecLoc.SelectedValue = currentReader.A1SecLocID;

				if(cbA1PrimDir.FindStringExact(currentReader.A1Direction) > 0)
				{
					cbA1PrimDir.SelectedIndex = cbA1PrimDir.FindStringExact(currentReader.A1Direction);
				}
				else
				{
					cbA1PrimDir.SelectedIndex = 0;
				}

				if (cbA1SecDir.FindStringExact(currentReader.A1SecDirection) > 0)
				{
					cbA1SecDir.SelectedIndex = cbA1SecDir.FindStringExact(currentReader.A1SecDirection);
				}
				else
				{
					cbA1SecDir.SelectedIndex = 0;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ReaderAdvanceSettings.InitializeForm(): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			InitializeForm();
		}

		private void btnReaderSettings_Click(object sender, System.EventArgs e)
		{
				Process.Start(Constants.RegistryDataApplication, currentReader.ConnectionAddress + " " + currentReader.TechType.ToUpper());			
		}

        private void ReaderAdvanceSettings_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + "ReaderAdvanceSettings.ReaderAdvanceSettings_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
