using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for ApplUsersAdd.
	/// </summary>
	public class ChangePassword : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbConfirmPassword;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lblConfirmPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ApplUserTO logInUser;
		ResourceManager rm;

		private CultureInfo culture;
		
		DebugLog log;

		public ChangePassword()
		{
			InitializeComponent();
			
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(ApplUsersAdd).Assembly);

			setLanguage();

			tbPassword.Text = logInUser.Password.Trim();
			tbConfirmPassword.Enabled = false;
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
            this.tbConfirmPassword = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbConfirmPassword
            // 
            this.tbConfirmPassword.Location = new System.Drawing.Point(152, 48);
            this.tbConfirmPassword.MaxLength = 10;
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.PasswordChar = '*';
            this.tbConfirmPassword.Size = new System.Drawing.Size(192, 20);
            this.tbConfirmPassword.TabIndex = 6;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(152, 16);
            this.tbPassword.MaxLength = 10;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(192, 20);
            this.tbPassword.TabIndex = 4;
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.Location = new System.Drawing.Point(16, 48);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(128, 23);
            this.lblConfirmPassword.TabIndex = 5;
            this.lblConfirmPassword.Text = "Confirm Password:";
            this.lblConfirmPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(16, 16);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(128, 23);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(32, 88);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(272, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ChangePassword
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(376, 121);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.tbConfirmPassword);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.lblPassword);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(384, 155);
            this.MinimumSize = new System.Drawing.Size(384, 155);
            this.Name = "ChangePassword";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "ApplUsersAdd";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChangePassword_KeyUp);
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
				this.Text = rm.GetString("changePassword", culture);
				
				// button's text
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblPassword.Text = rm.GetString("lblPassword", culture);
				lblConfirmPassword.Text = rm.GetString("lblConfirmPassword", culture);
			}
			catch (Exception ex)
			{
                log.writeLog(DateTime.Now + " ChangePassword.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ChangePassword.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

                if (this.tbPassword.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("updUserPasswordNotEmpty", culture));
                    this.tbPassword.Text = "";
                    this.tbConfirmPassword.Text = "";
                    this.tbPassword.Focus();
                    return;
                }

				if (this.tbConfirmPassword.Enabled && (!this.tbPassword.Text.Trim().Equals(this.tbConfirmPassword.Text.Trim())))
				{
					MessageBox.Show(rm.GetString("updUserPasswordNotMatch", culture));
					this.tbPassword.Text = "";
					this.tbConfirmPassword.Text = "";
					this.tbPassword.Focus();
					return;
				}
				
				logInUser.Password = tbPassword.Text.Trim();

                ApplUser user = new ApplUser();
                user.UserTO = logInUser;

				bool updated = user.Update();

				if (updated)
				{
					MessageBox.Show(rm.GetString("userUpdated", culture));
					this.Close();				
				}
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " ChangePassword.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void tbPassword_TextChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                tbConfirmPassword.Enabled = true;
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " ChangePassword.tbPassword_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void ChangePassword_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ChangePassword.ChangePassword_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
