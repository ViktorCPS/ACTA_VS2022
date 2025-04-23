using System;
using System.Drawing;
using System.Collections;
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
	/// Summary description for GatesXGateTimeProfileUpd.
	/// </summary>
	public class GatesXGateTimeProfileUpd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblSelectProfileToAsign;
		private System.Windows.Forms.ComboBox cbGateProfiles;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		GateTO currentGate = null;

		ApplUserTO logInUser;		
		ResourceManager rm;
		private CultureInfo culture;				
		DebugLog log;

		public GatesXGateTimeProfileUpd(GateTO gate)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentGate = new GateTO(gate.GateID, gate.Name, gate.Description, gate.DownloadStartTime,
				gate.DownloadInterval, gate.DownloadEraseCounter); 
			currentGate.GateTimeaccessProfileID = gate.GateTimeaccessProfileID;
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(GatesXGateTimeProfileUpd).Assembly);
			setLanguage();

			tbName.Text = gate.Name.Trim();
			tbDesc.Text = gate.Description.Trim();

			populateComboBoxe();
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblSelectProfileToAsign = new System.Windows.Forms.Label();
            this.cbGateProfiles = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(192, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(8, 184);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbDesc
            // 
            this.tbDesc.Enabled = false;
            this.tbDesc.Location = new System.Drawing.Point(96, 48);
            this.tbDesc.MaxLength = 150;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(168, 20);
            this.tbDesc.TabIndex = 12;
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Location = new System.Drawing.Point(96, 16);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(168, 20);
            this.tbName.TabIndex = 10;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(8, 48);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(80, 23);
            this.lblDesc.TabIndex = 11;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(8, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 23);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "Gate name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSelectProfileToAsign
            // 
            this.lblSelectProfileToAsign.Location = new System.Drawing.Point(8, 96);
            this.lblSelectProfileToAsign.Name = "lblSelectProfileToAsign";
            this.lblSelectProfileToAsign.Size = new System.Drawing.Size(256, 23);
            this.lblSelectProfileToAsign.TabIndex = 13;
            this.lblSelectProfileToAsign.Text = "Select profile to be assigned to gate:";
            this.lblSelectProfileToAsign.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbGateProfiles
            // 
            this.cbGateProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGateProfiles.Location = new System.Drawing.Point(8, 128);
            this.cbGateProfiles.Name = "cbGateProfiles";
            this.cbGateProfiles.Size = new System.Drawing.Size(256, 21);
            this.cbGateProfiles.TabIndex = 14;
            this.cbGateProfiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbGateProfiles_KeyDown);
            // 
            // GatesXGateTimeProfileUpd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(282, 216);
            this.ControlBox = false;
            this.Controls.Add(this.cbGateProfiles);
            this.Controls.Add(this.lblSelectProfileToAsign);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(290, 250);
            this.MinimumSize = new System.Drawing.Size(290, 250);
            this.Name = "GatesXGateTimeProfileUpd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Gates <-> Gate access profiles - Update";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GatesXGateTimeProfileUpd_KeyUp);
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
				this.Text = rm.GetString("updateGateProfileForm", culture);
				
				// button's text
				btnSave.Text   = rm.GetString("btnSave", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblName.Text                 = rm.GetString("lblGateName", culture);
				lblDesc.Text                 = rm.GetString("lblDescription", culture);
				lblSelectProfileToAsign.Text = rm.GetString("lblSelectProfileToAsign", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesXGateTimeProfileUpd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool isUpdated = false;

                if (currentGate.GateTimeaccessProfileID.ToString() != cbGateProfiles.SelectedValue.ToString())
                {
                    ArrayList gates = (new AccessGroupXGate()).Search("", currentGate.GateID.ToString());
                    if (gates.Count > 0)
                    {
                        DialogResult proc = MessageBox.Show(rm.GetString("deleteAccessGroupXGate", culture), "", MessageBoxButtons.YesNo);
                        if (proc == DialogResult.Yes)
                        {
                            AccessGroupXGate accessGroupXGate = new AccessGroupXGate();
                            bool trans = accessGroupXGate.BeginTransaction();

                            if (trans)
                            {
                                isUpdated = accessGroupXGate.DeleteGates(currentGate.GateID.ToString(), false);

                                if (isUpdated)
                                {
                                    Gate g = new Gate();
                                    g.SetTransaction(accessGroupXGate.GetTransaction());

                                    isUpdated = g.UpdateGateTAProfile(currentGate.GateID.ToString(), cbGateProfiles.SelectedValue.ToString(), false) && isUpdated;
                                }

                                if (isUpdated)
                                {
                                    accessGroupXGate.CommitTransaction();

                                    currentGate.Name = tbName.Text.Trim();
                                    currentGate.Description = tbDesc.Text.Trim();
                                    currentGate.GateTimeaccessProfileID = Int32.Parse(cbGateProfiles.SelectedValue.ToString().Trim());

                                    MessageBox.Show(rm.GetString("gateProfileUpdated", culture));
                                    this.Close();
                                }
                                else
                                {
                                    accessGroupXGate.RollbackTransaction();

                                    MessageBox.Show(rm.GetString("gateProfileNotUpdated", culture));
                                }
                            } //if (trans)
                            else
                            {
                                MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                return;
                            }
                        } // yes
                    }
                    else //no records in xref table
                    {
                        if (isUpdated = new Gate().UpdateGateTAProfile(currentGate.GateID.ToString(), cbGateProfiles.SelectedValue.ToString(), true))
                        {
                            currentGate.Name = tbName.Text.Trim();
                            currentGate.Description = tbDesc.Text.Trim();
                            currentGate.GateTimeaccessProfileID = Int32.Parse(cbGateProfiles.SelectedValue.ToString().Trim());

                            MessageBox.Show(rm.GetString("gateProfileUpdated", culture));
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("gateProfileNotUpdated", culture));
                        }
                    }
                }
                else //no change, do not do anything
                {
                    MessageBox.Show(rm.GetString("gateProfileUpdated", culture));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GatesXGateTimeProfileUpd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                
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
                log.writeLog(DateTime.Now + " GatesXGateTimeProfileUpd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populateComboBoxe()
		{
			try
			{
				ArrayList gateTAProfileArray = new GateTimeAccessProfile().Search("");				

				cbGateProfiles.DataSource    = gateTAProfileArray;
				cbGateProfiles.DisplayMember = "Name";
				cbGateProfiles.ValueMember   = "GateTAProfileId";

				if (cbGateProfiles.Items.Count > 0)
					cbGateProfiles.SelectedValue = currentGate.GateTimeaccessProfileID;

				//cbGateProfiles.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesXGateTimeProfileUpd.populateComboBoxe(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void cbGateProfiles_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (e.KeyCode == Keys.Enter)
				{
					btnSave.Focus();
					btnSave.PerformClick();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " GatesXGateTimeProfileUpd.cbGateProfiles_KeyDown(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void GatesXGateTimeProfileUpd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " GatesXGateTimeProfileUpd.GatesXGateTimeProfileUpd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
