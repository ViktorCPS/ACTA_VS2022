using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for GatesAdd.
	/// </summary>
	public class GatesAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblGateID;
		private System.Windows.Forms.TextBox tbGateID;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		GateTO currentGate = null;
		ResourceManager rm;

		private CultureInfo culture;
		
		DebugLog log;
		private System.Windows.Forms.Label lblDownloadStart;
		private System.Windows.Forms.DateTimePicker dtpStartTime;
		private System.Windows.Forms.Label lblDownloadInterval;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblDownloadEraseCounter;
		private System.Windows.Forms.TextBox tbEraseCounter;
		private System.Windows.Forms.TextBox tbInterval;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label label3;

		ApplUserTO logInUser;

		public GatesAdd()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentGate = new GateTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(GatesAdd).Assembly);
			setLanguage();

			btnUpdate.Visible = false;
			lblGateID.Visible = false;
			tbGateID.Visible = false;
		}

		public GatesAdd(GateTO gate)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentGate = new GateTO(gate.GateID, gate.Name, gate.Description, 
				gate.DownloadStartTime, gate.DownloadInterval, gate.DownloadEraseCounter);
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(GatesAdd).Assembly);

			setLanguage();
			
			tbGateID.Text = gate.GateID.ToString().Trim();
			tbName.Text = gate.Name.Trim();
			tbDesc.Text = gate.Description.Trim();
			btnSave.Visible = false;
			tbGateID.Enabled = false;
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
            this.lblGateID = new System.Windows.Forms.Label();
            this.tbGateID = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDownloadStart = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblDownloadInterval = new System.Windows.Forms.Label();
            this.lblDownloadEraseCounter = new System.Windows.Forms.Label();
            this.tbEraseCounter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbInterval = new System.Windows.Forms.TextBox();
            this.lblMin = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblGateID
            // 
            this.lblGateID.Location = new System.Drawing.Point(48, 16);
            this.lblGateID.Name = "lblGateID";
            this.lblGateID.Size = new System.Drawing.Size(64, 23);
            this.lblGateID.TabIndex = 0;
            this.lblGateID.Text = "Gate ID:";
            this.lblGateID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbGateID
            // 
            this.tbGateID.Location = new System.Drawing.Point(120, 16);
            this.tbGateID.Name = "tbGateID";
            this.tbGateID.Size = new System.Drawing.Size(176, 20);
            this.tbGateID.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(40, 48);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(64, 23);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(120, 48);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(176, 20);
            this.tbName.TabIndex = 3;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(40, 80);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(64, 23);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Text = "Description";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(120, 80);
            this.tbDesc.MaxLength = 50;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(176, 20);
            this.tbDesc.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(40, 224);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(40, 224);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(72, 24);
            this.btnUpdate.TabIndex = 17;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(224, 224);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDownloadStart
            // 
            this.lblDownloadStart.Location = new System.Drawing.Point(8, 112);
            this.lblDownloadStart.Name = "lblDownloadStart";
            this.lblDownloadStart.Size = new System.Drawing.Size(192, 23);
            this.lblDownloadStart.TabIndex = 6;
            this.lblDownloadStart.Text = "Download start time:";
            this.lblDownloadStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "HH:mm";
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(216, 112);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(80, 20);
            this.dtpStartTime.TabIndex = 7;
            // 
            // lblDownloadInterval
            // 
            this.lblDownloadInterval.Location = new System.Drawing.Point(8, 144);
            this.lblDownloadInterval.Name = "lblDownloadInterval";
            this.lblDownloadInterval.Size = new System.Drawing.Size(192, 23);
            this.lblDownloadInterval.TabIndex = 9;
            this.lblDownloadInterval.Text = "Download interval:";
            this.lblDownloadInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDownloadEraseCounter
            // 
            this.lblDownloadEraseCounter.Location = new System.Drawing.Point(8, 176);
            this.lblDownloadEraseCounter.Name = "lblDownloadEraseCounter";
            this.lblDownloadEraseCounter.Size = new System.Drawing.Size(232, 23);
            this.lblDownloadEraseCounter.TabIndex = 13;
            this.lblDownloadEraseCounter.Text = "Download erase counter:";
            this.lblDownloadEraseCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbEraseCounter
            // 
            this.tbEraseCounter.Location = new System.Drawing.Point(248, 176);
            this.tbEraseCounter.Name = "tbEraseCounter";
            this.tbEraseCounter.Size = new System.Drawing.Size(48, 20);
            this.tbEraseCounter.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(312, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "*";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(320, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "*";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(304, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "*";
            // 
            // tbInterval
            // 
            this.tbInterval.Location = new System.Drawing.Point(216, 144);
            this.tbInterval.Name = "tbInterval";
            this.tbInterval.Size = new System.Drawing.Size(80, 20);
            this.tbInterval.TabIndex = 10;
            // 
            // lblMin
            // 
            this.lblMin.Location = new System.Drawing.Point(296, 144);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(24, 23);
            this.lblMin.TabIndex = 11;
            this.lblMin.Text = "min";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(296, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 23);
            this.label3.TabIndex = 19;
            this.label3.Text = "%";
            // 
            // GatesAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(344, 254);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.tbInterval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbEraseCounter);
            this.Controls.Add(this.lblDownloadEraseCounter);
            this.Controls.Add(this.lblDownloadInterval);
            this.Controls.Add(this.dtpStartTime);
            this.Controls.Add(this.lblDownloadStart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbGateID);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblGateID);
            this.MaximumSize = new System.Drawing.Size(352, 288);
            this.MinimumSize = new System.Drawing.Size(352, 288);
            this.Name = "GatesAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "GatesAdd";
            this.Load += new System.EventHandler(this.GatesAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GatesAdd_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Set proper language
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				if (currentGate.GateID >= 0)
				{
					this.Text = rm.GetString("updateGate", culture);
				}
				else
				{
					this.Text = rm.GetString("addGate", culture);
				}

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblGateID.Text = rm.GetString("lblGateID", culture);
				lblName.Text = rm.GetString("lblName", culture);
				lblDesc.Text = rm.GetString("lblDescription", culture);
				lblDownloadStart.Text = rm.GetString("lblDownloadStart", culture);
				lblDownloadInterval.Text = rm.GetString("lblDownloadInterval", culture);
				lblDownloadEraseCounter.Text = rm.GetString("lblDownloadEraseCounter", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (tbInterval.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("GateIntervalNotNull", culture));
                    return;
                }
                try
                {
                    if (Int32.Parse(tbInterval.Text.Trim()) <= 0)
                    {
                        MessageBox.Show(rm.GetString("GateIntervalNotNumber", culture));
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("GateIntervalNotNumber", culture));
                    return;
                }

                if (tbEraseCounter.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("GateEraseCounterNotNull", culture));
                    return;
                }
                try
                {
                    if (Int32.Parse(tbEraseCounter.Text.Trim()) <= 0)
                    {
                        MessageBox.Show(rm.GetString("GateEraseCounterNotNumber", culture));
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("GateEraseCounterNotNumber", culture));
                    return;
                }

                Gate g = new Gate();
                g.GTO.Name = tbName.Text.Trim();
                List<GateTO> gates = g.Search();

                if (gates.Count == 0)
                {
                    currentGate.Name = this.tbName.Text.Trim();
                    currentGate.Description = this.tbDesc.Text.Trim();
                    DateTime date = dtpStartTime.Value;
                    currentGate.DownloadStartTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                    currentGate.DownloadInterval = Int32.Parse(tbInterval.Text.Trim());
                    currentGate.DownloadEraseCounter = Int32.Parse(tbEraseCounter.Text.Trim());

                    int inserted = new Gate().Save(currentGate.Name, currentGate.Description,
                        currentGate.DownloadStartTime, currentGate.DownloadInterval, currentGate.DownloadEraseCounter);
                    if (inserted > 0)
                    {
                        MessageBox.Show(rm.GetString("gateSaved", culture));
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("gateNameExists", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GatesAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (tbInterval.Text.Equals(""))
				{
					MessageBox.Show(rm.GetString("GateIntervalNotNull", culture));
					return;
				}
				try
				{
					if (Int32.Parse(tbInterval.Text.Trim()) <= 0)
					{
						MessageBox.Show(rm.GetString("GateIntervalNotNumber", culture));
						return;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("GateIntervalNotNumber", culture));
					return;
				}
				
				if (tbEraseCounter.Text.Equals(""))
				{
					MessageBox.Show(rm.GetString("GateEraseCounterNotNull", culture));
					return;
				}
				try
				{
					if (Int32.Parse(tbEraseCounter.Text.Trim()) <= 0)
					{
						MessageBox.Show(rm.GetString("GateEraseCounterNotNumber", culture));
						return;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("GateEraseCounterNotNumber", culture));
					return;
				}

				if (!currentGate.Name.Trim().Equals(tbName.Text.Trim()))
				{
                    Gate g = new Gate();
                    g.GTO.Name = tbName.Text.Trim();
					List<GateTO> gates = g.Search();

					if (gates.Count > 0)
					{
						MessageBox.Show(rm.GetString("gateNameExists", culture));
						return;
					}
				}
				
				bool isUpdated = false;
				DateTime date = dtpStartTime.Value;
				DateTime startTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);

				if (isUpdated = new Gate().Update(Int32.Parse(this.tbGateID.Text.Trim()), this.tbName.Text.Trim(), 
					this.tbDesc.Text.Trim(), startTime, Int32.Parse(tbInterval.Text.Trim()), Int32.Parse(tbEraseCounter.Text.Trim())))
				{
					currentGate.GateID = Int32.Parse(this.tbGateID.Text.Trim());
					currentGate.Name = this.tbName.Text.Trim();
					currentGate.Description = this.tbDesc.Text.Trim();
					currentGate.DownloadStartTime = startTime;
					currentGate.DownloadInterval = Int32.Parse(tbInterval.Text.Trim());
					currentGate.DownloadEraseCounter = Int32.Parse(tbEraseCounter.Text.Trim());
					
					MessageBox.Show(rm.GetString("gateUpdated", culture));
					this.Close();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesAdd.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " GatesAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void GatesAdd_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (currentGate.GateID < 0)
                {
                    dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                        (int)Constants.downloadStartHour, 0, 0);
                    tbInterval.Text = Constants.downloadInterval.ToString().Trim();
                    tbEraseCounter.Text = Constants.downloadEraseCounter.ToString().Trim();
                }
                else
                {
                    dtpStartTime.Value = currentGate.DownloadStartTime;
                    tbInterval.Text = currentGate.DownloadInterval.ToString().Trim();
                    tbEraseCounter.Text = currentGate.DownloadEraseCounter.ToString();
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " GatesAdd.GatesAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void GatesAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " GatesAdd.GatesAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
