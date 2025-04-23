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
	/// Summary description for EmployeeAccessGroupAdd.
	/// </summary>
	public class EmployeeAccessGroupAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		EmployeeGroupAccessControl currentEmployeeGroupAccessControl = null;

		ApplUserTO logInUser;		
		ResourceManager rm;
		private CultureInfo culture;				
		DebugLog log;

		public EmployeeAccessGroupAdd()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentEmployeeGroupAccessControl = new EmployeeGroupAccessControl();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(EmployeeAccessGroupAdd).Assembly);
			setLanguage();

			btnUpdate.Visible = false;
		}

		public EmployeeAccessGroupAdd(EmployeeGroupAccessControl employeeGroupAccessControl)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentEmployeeGroupAccessControl = new EmployeeGroupAccessControl(employeeGroupAccessControl.AccessGroupId, employeeGroupAccessControl.Name, employeeGroupAccessControl.Description);
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(EmployeeAccessGroupAdd).Assembly);
			setLanguage();

			tbName.Text = employeeGroupAccessControl.Name.Trim();
			tbDesc.Text = employeeGroupAccessControl.Description.Trim();

			btnSave.Visible = false;
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
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(8, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(96, 16);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(168, 20);
            this.tbName.TabIndex = 2;
            this.tbName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbName_KeyDown);
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(8, 48);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(80, 23);
            this.lblDesc.TabIndex = 3;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(96, 48);
            this.tbDesc.MaxLength = 150;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(168, 20);
            this.tbDesc.TabIndex = 4;
            this.tbDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDesc_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(24, 88);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(192, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(272, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "*";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(24, 88);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // EmployeeAccessGroupAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 118);
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(304, 152);
            this.MinimumSize = new System.Drawing.Size(304, 152);
            this.Name = "EmployeeAccessGroupAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "EmployeeAccessGroupAdd";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeeAccessGroupAdd_KeyUp);
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
				if (!currentEmployeeGroupAccessControl.Name.Equals(""))
				{
					this.Text = rm.GetString("updateAccessGroup", culture);
				}
				else
				{
					this.Text = rm.GetString("addAccessGroup", culture);
				}
				
				// button's text
				btnSave.Text   = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblName.Text = rm.GetString("lblName", culture);
				lblDesc.Text = rm.GetString("lblDescription", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroupAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				if (tbName.Text.Trim().Equals(""))
				{
					MessageBox.Show(rm.GetString("addAccessGroupNameEmpty", culture));
				}
				else
				{
					ArrayList accessGroups = currentEmployeeGroupAccessControl.Search(tbName.Text.Trim());

					if (accessGroups.Count == 0)
					{				
						int inserted = currentEmployeeGroupAccessControl.Save(tbName.Text.Trim(), tbDesc.Text.Trim());
						
						if (inserted > 0)
						{					
							currentEmployeeGroupAccessControl.Name        = tbName.Text.Trim();
							currentEmployeeGroupAccessControl.Description = tbDesc.Text.Trim();

							MessageBox.Show(rm.GetString("accessGroupSaved", culture));
							this.Close();
						}
						else
						{
							MessageBox.Show(rm.GetString("accessGroupNotSaved", culture));								
						}
					}
					else
					{
						MessageBox.Show(rm.GetString("accessGroupExists", culture));
					}
				}
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroupAdd.btnSave_Click(): " + ex.Message + "\n");
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
			
				bool isUpdated = false;

				if (tbName.Text.Trim().Equals(""))
				{
					MessageBox.Show(rm.GetString("addAccessGroupNameEmpty", culture));
				}
				else
				{
					if (!currentEmployeeGroupAccessControl.Name.Equals(tbName.Text.Trim()))
					{
						ArrayList accessGroups = currentEmployeeGroupAccessControl.Search(tbName.Text.Trim());
						if (accessGroups.Count != 0)
						{	
							MessageBox.Show(rm.GetString("accessGroupExists", culture));
							return;
						}
					}
									
					if (isUpdated = currentEmployeeGroupAccessControl.Update(currentEmployeeGroupAccessControl.AccessGroupId.ToString(), tbName.Text.Trim(), tbDesc.Text.Trim()))
					{		
						currentEmployeeGroupAccessControl.Name        = tbName.Text.Trim();
						currentEmployeeGroupAccessControl.Description = tbDesc.Text.Trim();

						MessageBox.Show(rm.GetString("accessGroupUpdated", culture));
						this.Close();
					}
					else
					{
						MessageBox.Show(rm.GetString("accessGroupNotUpdated", culture));								
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroupAdd.btnUpdate_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeAccessGroupAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void tbName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				if (e.KeyCode == Keys.Enter)
				{
					tbDesc.Focus();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroupAdd.tbName_KeyDown(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void tbDesc_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				if (e.KeyCode == Keys.Enter)
				{
					if (btnUpdate.Visible)
					{
						btnUpdate.Focus();
						btnUpdate.PerformClick();
					}
					else
					{
						btnSave.Focus();
						btnSave.PerformClick();
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeAccessGroupAdd.tbDesc_KeyDown(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void EmployeeAccessGroupAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " EmployeeAccessGroupAdd.EmployeeAccessGroupAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

	}
}
