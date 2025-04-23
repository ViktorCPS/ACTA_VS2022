using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using TransferObjects;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for LogInForm.
	/// </summary>
	public class LogInForm : System.Windows.Forms.Form
	{		
		private System.Windows.Forms.TextBox tbUserName;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lblUser;
		private System.Windows.Forms.Button btnLogIn;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CultureInfo culture;
		ResourceManager rm;

		DebugLog log;

		ApplUser currentUser = null;

		private System.Windows.Forms.CheckBox cbChangePass;

		// Controller instance
		public NotificationController Controller;
		
		public LogInForm()
		{
			InitializeComponent();
			InitializeController();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentUser = new ApplUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(ApplUsers).Assembly);
			//setLanguage();
            ApplUserLogTO userlog = new ApplUserLogTO();
            Controller = NotificationController.GetInstance();
            NotificationController.SetLog(userlog);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogInForm));
            this.lblUser = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbChangePass = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(8, 8);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(80, 23);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "Username:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(96, 8);
            this.tbUserName.MaxLength = 20;
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(184, 20);
            this.tbUserName.TabIndex = 1;
            this.tbUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUserName_KeyDown);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(8, 40);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(80, 23);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(96, 40);
            this.tbPassword.MaxLength = 10;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(184, 20);
            this.tbPassword.TabIndex = 3;
            this.tbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPassword_KeyDown);
            // 
            // btnLogIn
            // 
            this.btnLogIn.Location = new System.Drawing.Point(16, 104);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(75, 23);
            this.btnLogIn.TabIndex = 5;
            this.btnLogIn.Text = "OK";
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(208, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbChangePass
            // 
            this.cbChangePass.Location = new System.Drawing.Point(96, 72);
            this.cbChangePass.Name = "cbChangePass";
            this.cbChangePass.Size = new System.Drawing.Size(184, 24);
            this.cbChangePass.TabIndex = 4;
            this.cbChangePass.Text = "Change password";
            // 
            // LogInForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(284, 127);
            this.Controls.Add(this.cbChangePass);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogIn);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 165);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 165);
            this.Name = "LogInForm";
            this.Text = "ACTAAdmin";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LogInForm_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

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
				this.Text = rm.GetString("logInForm", culture);

				// button's text
				btnLogIn.Text = rm.GetString("btnLogIn", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblUser.Text = rm.GetString("lblUser", culture);
				lblPassword.Text = rm.GetString("lblPassword", culture);

				// check box text
				cbChangePass.Text = rm.GetString("cbChangePass", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " LogInForm.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}

		}

		private void InitializeController()
		{
			Controller = NotificationController.GetInstance();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.Close();
                currentUser = new ApplUser();
                Controller.LogIn(currentUser.UserTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogInForm.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnLogIn_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string userID = this.tbUserName.Text;
                string password = this.tbPassword.Text;

                ApplUserTO userTO = currentUser.FindUserPassword(userID, "");

                if (userTO.UserID.Trim().Equals("") ||
                    (!userTO.UserID.Trim().Equals("") && !userTO.UserID.Equals(userID)))
                {
                    MessageBox.Show(Constants.LoginFailed);
                    this.tbUserName.SelectAll();
                    this.tbUserName.Focus();
                }
                else
                {
                    if (!userTO.Status.Equals("ACTIVE"))
                    {
                        MessageBox.Show(rm.GetString("noActiveUser", culture));
                        this.tbUserName.SelectAll();
                        this.tbUserName.Focus();
                    }
                    else if (userTO.NumOfTries == 0)
                    {
                        MessageBox.Show(rm.GetString("noMoreLogIn", culture));
                        this.btnCancel.PerformClick();
                        return;
                    }
                    else
                    {

                        ApplUser user = new ApplUser();
                        user.UserTO = userTO;

                        ApplUserLog userlog = new ApplUserLog();

                        //userTO = currentUser.FindUserPassword(userID, password);
                        if (userTO.UserID.Trim().Equals("") ||
                            (!userTO.Password.Trim().Equals("") && !userTO.Password.Equals(password)))
                        {
                            // If user is 'SYS' and password is licence password, open licence generate form
                            if (userTO.UserID.Trim().Equals(Constants.sysUser) &&
                                password.Equals(Constants.licencePassword + DateTime.Now.Date.ToString("MMdd")))
                            {
                                LicenceGenerate licGenerate = new LicenceGenerate(userTO.LangCode);
                                licGenerate.ShowDialog();
                            }
                            else
                            {
                                // Existing User try to log in with incorrect password and it's number og login tries will be decremented
                                user.UserTO.NumOfTries--;
                                user.Update();

                                if (user.UserTO.NumOfTries == 0)
                                {
                                    MessageBox.Show("Password does not exist. No more login tries!");
                                    this.btnCancel.PerformClick();
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Password does not exist.");
                                }

                                this.tbPassword.SelectAll();
                                this.tbPassword.Focus();
                            }
                        }
                        else
                        {
                            int numOfTries = Constants.numOfTries;
                            if (user.UserTO.NumOfTries != numOfTries)
                            {
                                user.UserTO.NumOfTries = numOfTries;
                                user.Update();
                            }

                            userTO = currentUser.FindExitPermVerification(userID);

                            currentUser.UserTO = userTO;
                            Controller.LogIn(currentUser.UserTO);
                            NotificationController.SetChangePassword(cbChangePass.Checked);
                            /*NotificationController.SetLogInUser(currentUser);
                            userlog = userlog.Insert();
                            NotificationController.SetLog(userlog);*/
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogInForm.btnLogIn_Click(): " + ex.Message + "\n");

                if (ex.Message.Equals("No Database connection."))
                {
                    MessageBox.Show(rm.GetString("noDBConn", culture));
                    this.Close();
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void tbUserName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (e.KeyCode == Keys.Enter)
                {
                    this.tbPassword.SelectAll();
                    this.tbPassword.Focus();
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " LogInForm.tbUserName_KeyDown(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void tbPassword_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            try{
            this.Cursor = Cursors.WaitCursor;

			if (e.KeyCode == Keys.Enter)
			{
				this.btnLogIn.Focus();
				this.btnLogIn.PerformClick();
			}
        }catch (Exception ex) {
            log.writeLog(DateTime.Now + " LogInForm.tbPassword_KeyDown(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		
        private void LogInForm_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " LogInForm.LogInForm_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        public void btnOKClick(string userID, string password)
        {
            tbUserName.Text = userID;
            tbPassword.Text = password;
            btnLogIn_Click(this, new EventArgs());
        }		
	}
}
