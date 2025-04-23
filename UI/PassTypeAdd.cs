using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using System.Data.SqlClient;
using TransferObjects;
using Common;
using Util;

using System.Resources;
using System.Globalization;

namespace UI
{
	/// <summary>
	/// Add new and Update Pass Type
	/// </summary>
	public class PassTypeAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox tbButton;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.TextBox tbPassTypeID;
		private System.Windows.Forms.Label lblButton;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblPassTypeID;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		PassTypeTO currentPassType = null;
		private CultureInfo culture;
		ResourceManager rm;
		DebugLog log;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbPassType;
		private System.Windows.Forms.Label lblPassType;
		private System.Windows.Forms.Label label2;

		ApplUserTO logInUser;
		private System.Windows.Forms.TextBox tbPayCode;
		private System.Windows.Forms.Label lblPayCode;

		// Controller instance
		public NotificationController Controller;

		// Possible values of Pass Types
		public struct PassTypeValue
		{
			public string typeName;
			public int typeValue;

			public string TypeName
			{
				get{ return typeName; }
				set{ typeName = value; }
			}

			public int TypeValue
			{
				get{ return typeValue; }
				set{ typeValue = value; }
			}

			public PassTypeValue(string typeName, int typeValue)
			{
				this.typeName = typeName;
				this.typeValue = typeValue;
				this.TypeName = typeName;
				this.TypeValue = typeValue;
			}
		}

		public PassTypeAdd(int passTypeID)
		{
			InitializeComponent();

			this.CenterToScreen();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(PassTypes).Assembly);
			logInUser = NotificationController.GetLogInUser();
			
			InitializeController();
			
			currentPassType = new PassTypeTO();

			populatePassTypeCombo();

			if (passTypeID == -1)
			{
				// Init for for Adding new Pass Type
				
				int ptID = new PassType().FindMAXPassTypeID();
				ptID++;
				tbPassTypeID.Text = ptID.ToString();
				btnUpdate.Visible = false;
				btnSave.Visible = true;
				label3.Visible = true;
			}
			else
			{
				// Fill form with given Pass Type
                currentPassType = new PassType().Find(passTypeID);
				btnSave.Visible = false;
				tbPassTypeID.Enabled = false;
				label3.Visible = false;

				// Fill Data
				tbPassTypeID.Text = currentPassType.PassTypeID.ToString();
				tbDescription.Text = currentPassType.Description;
				tbButton.Text = currentPassType.Button == -1 ? "" : currentPassType.Button.ToString();
				tbPayCode.Text = currentPassType.PaymentCode.Trim();

				cbPassType.SelectedValue = currentPassType.IsPass;

				btnUpdate.Visible = true;
				btnSave.Visible = false;
			}
			setLanguage();
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbButton = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbPassTypeID = new System.Windows.Forms.TextBox();
            this.lblButton = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblPassTypeID = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPayCode = new System.Windows.Forms.TextBox();
            this.lblPayCode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(40, 184);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(40, 184);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbButton
            // 
            this.tbButton.Location = new System.Drawing.Point(120, 80);
            this.tbButton.Name = "tbButton";
            this.tbButton.Size = new System.Drawing.Size(40, 20);
            this.tbButton.TabIndex = 5;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(120, 48);
            this.tbDescription.MaxLength = 50;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(304, 20);
            this.tbDescription.TabIndex = 3;
            // 
            // tbPassTypeID
            // 
            this.tbPassTypeID.Location = new System.Drawing.Point(120, 16);
            this.tbPassTypeID.Name = "tbPassTypeID";
            this.tbPassTypeID.Size = new System.Drawing.Size(160, 20);
            this.tbPassTypeID.TabIndex = 1;
            // 
            // lblButton
            // 
            this.lblButton.Location = new System.Drawing.Point(16, 80);
            this.lblButton.Name = "lblButton";
            this.lblButton.Size = new System.Drawing.Size(88, 23);
            this.lblButton.TabIndex = 4;
            this.lblButton.Text = "Button:";
            this.lblButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(16, 48);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(88, 23);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassTypeID
            // 
            this.lblPassTypeID.Location = new System.Drawing.Point(16, 16);
            this.lblPassTypeID.Name = "lblPassTypeID";
            this.lblPassTypeID.Size = new System.Drawing.Size(88, 23);
            this.lblPassTypeID.TabIndex = 0;
            this.lblPassTypeID.Text = "Pass Type ID:";
            this.lblPassTypeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(336, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(288, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "*";
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(120, 112);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(168, 21);
            this.cbPassType.TabIndex = 7;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(24, 112);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(80, 23);
            this.lblPassType.TabIndex = 6;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(296, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 32;
            this.label2.Text = "*";
            // 
            // tbPayCode
            // 
            this.tbPayCode.Location = new System.Drawing.Point(120, 144);
            this.tbPayCode.MaxLength = 4;
            this.tbPayCode.Name = "tbPayCode";
            this.tbPayCode.Size = new System.Drawing.Size(168, 20);
            this.tbPayCode.TabIndex = 9;
            // 
            // lblPayCode
            // 
            this.lblPayCode.Location = new System.Drawing.Point(16, 144);
            this.lblPayCode.Name = "lblPayCode";
            this.lblPayCode.Size = new System.Drawing.Size(88, 23);
            this.lblPayCode.TabIndex = 8;
            this.lblPayCode.Text = "Payment Code:";
            this.lblPayCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PassTypeAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(456, 214);
            this.ControlBox = false;
            this.Controls.Add(this.tbPayCode);
            this.Controls.Add(this.lblPayCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbPassType);
            this.Controls.Add(this.lblPassType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbButton);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbPassTypeID);
            this.Controls.Add(this.lblButton);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblPassTypeID);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(464, 248);
            this.MinimumSize = new System.Drawing.Size(464, 248);
            this.Name = "PassTypeAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "New Pass Type";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PassTypeAdd_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Pass Type ID
                if (this.tbPassTypeID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("messagePassTypeIDNotSet", culture));
                    return;
                }

                try
                {
                    if (!tbPassTypeID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbPassTypeID.Text.Trim());
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypeIDMustBeNum", culture));
                    return;
                }

                // Pass Type
                if (cbPassType.SelectedIndex < 0)
                {
                    MessageBox.Show(rm.GetString("msgPassType", culture));
                    return;
                }
                /*if (((int) cbPassType.SelectedValue) < 2 && tbButton.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("passTypeButtonNotNull", culture));
                    return;
                }*/

                // Button
                try
                {
                    if (!tbButton.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbButton.Text.Trim());
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypeButton", culture));
                    return;
                }

                // Payment Code
                try
                {
                    if (!tbPayCode.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbPayCode.Text.Trim());
                    }
                    if ((!tbPayCode.Text.Trim().Equals("")) && (tbPayCode.Text.Trim().Length < Constants.PaymentCodeLength))
                    {
                        MessageBox.Show(rm.GetString("messagePassTypePayCode", culture));
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypePayCode", culture));
                    return;
                }

                // Pass Type
                PassType pt = new PassType();
                int ptID = -1;
                if (int.TryParse(tbPassTypeID.Text.Trim(), out ptID))
                    pt.PTypeTO.PassTypeID = ptID;
                pt.PTypeTO.Description = tbDescription.Text.Trim();
                int button = -1;
                if (int.TryParse(tbButton.Text.Trim(), out button))
                    pt.PTypeTO.Button = button;
                pt.PTypeTO.IsPass = (int)cbPassType.SelectedValue;
                pt.PTypeTO.PaymentCode = tbPayCode.Text.Trim();
                int inserted = pt.Save();
                if (inserted == 1)
                {
                    MessageBox.Show(rm.GetString("messagePassTypeInserted", culture));
                    currentPassType = new PassTypeTO();
                }

                Controller.PassTypeChanged(true);
                this.Close();
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    log.writeLog(DateTime.Now + " PassTypeAdd.btnSave_Click(): " + rm.GetString("messagePassTypeExist", culture) + "\n");
                    MessageBox.Show(rm.GetString("messagePassTypeExist", culture));
                }
                else
                {
                    log.writeLog(DateTime.Now + " PassType.btnSave_Click(): " + sqlex.Message + "\n");
                    MessageBox.Show(sqlex.Message);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeAdd.btnSave_Click(): " + ex.Message + "\n");
                if (ex.Message.Equals("Button already exists."))
                {
                    MessageBox.Show(rm.GetString("passTypeButtonExists", culture));
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

		private void setLanguage()
		{
			try
			{
				if (currentPassType.PassTypeID.Equals(-1))
				{
					this.Text = rm.GetString("addPassType", culture);
				}
				else
				{
					this.Text = rm.GetString("updatePassType", culture);
				}

				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				lblPassTypeID.Text = rm.GetString("lblPassTypeID", culture);
				lblDescription.Text = rm.GetString("lblDescription", culture);
				lblButton.Text = rm.GetString("lblButton", culture);
				lblPassType.Text = rm.GetString("lblPassType", culture);
				lblPayCode.Text = rm.GetString("lblPayCode", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassTypeAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void InitializeController()
		{
			Controller = NotificationController.GetInstance();
		}

		private void populatePassTypeCombo()
		{
			try
			{
				ArrayList ptMemebers = new ArrayList();
								
				//ptValue = new PassTypeValue(rm.GetString("all", culture), -1);
				//ptMemebers.Add(ptValue);
				PassTypeValue ptValue = new PassTypeValue(rm.GetString("isPassReader", culture), 1);
				ptMemebers.Add(ptValue);
				ptValue = new PassTypeValue(rm.GetString("isPassWholeDayAbsence", culture), 2);
				ptMemebers.Add(ptValue);
				ptValue = new PassTypeValue(rm.GetString("isPassOther", culture), 0);
				ptMemebers.Add(ptValue);
				
				cbPassType.DataSource = ptMemebers;
				cbPassType.DisplayMember = "TypeName";
				cbPassType.ValueMember = "TypeValue";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " PassTypeAdd.populatePassTypeCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/*private void cbPassType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cbPassType.SelectedIndex == 0)
			{
				this.label1.Visible = true;
				this.tbButton.Enabled = true;
			}
			else if (((int) cbPassType.SelectedValue) == 2)
			{
				this.label1.Visible = false;
				this.tbButton.Text = "";
				this.tbButton.Enabled = false;
			}
			else
			{
				this.label1.Visible = true;
				this.tbButton.Enabled = true;				
			}
		}*/

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.tbPassTypeID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("messagePassTypeIDNotSet", culture));
                    return;
                }

                try
                {
                    if (!tbPassTypeID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbPassTypeID.Text.Trim());
                    }
                }
                //catch(Exception ex)
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypeIDMustBeNum", culture));
                    return;
                }

                if (cbPassType.SelectedIndex < 0)
                {
                    MessageBox.Show(rm.GetString("msgPassType", culture));
                    return;
                }

                /*if (((int) cbPassType.SelectedValue) < 2 && tbButton.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("passTypeButtonNotNull", culture));
                    return;
                }*/

                try
                {
                    if (!tbButton.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbButton.Text.Trim());
                    }
                }
                //catch(Exception ex)
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypeButton", culture));
                    return;
                }

                // Payment Code
                try
                {
                    if (!tbPayCode.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbPayCode.Text.Trim());
                    }
                    if ((!tbPayCode.Text.Trim().Equals("")) && (tbPayCode.Text.Trim().Length < Constants.PaymentCodeLength))
                    {
                        MessageBox.Show(rm.GetString("messagePassTypePayCode", culture));
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(rm.GetString("messagePassTypePayCode", culture));
                    return;
                }

                // Pass Type
                int isPass = (int)cbPassType.SelectedValue;

                bool isUpdated = false;

                // currentPassType still contains old values of Pass Type
                PassType pt = new PassType();
                int ptID = -1;
                if (int.TryParse(tbPassTypeID.Text.Trim(), out ptID))
                    pt.PTypeTO.PassTypeID = ptID;
                pt.PTypeTO.Description = tbDescription.Text.Trim();
                int button = -1;
                if (int.TryParse(tbButton.Text.Trim(), out button))
                    pt.PTypeTO.Button = button;
                pt.PTypeTO.IsPass = (int)cbPassType.SelectedValue;
                pt.PTypeTO.PaymentCode = tbPayCode.Text.Trim();
                if (isUpdated = pt.Update(currentPassType.Button))
                {
                    MessageBox.Show(rm.GetString("messagePassTypeUpdated", culture));
                    currentPassType = new PassTypeTO();
                }

                Controller.PassTypeChanged(true);
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeAdd.btnUpdate_Click(): " + ex.Message + "\n");
                if (ex.Message.Equals("Button already exists."))
                {
                    MessageBox.Show(rm.GetString("passTypeButtonExists", culture));
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

        private void PassTypeAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " PassTypeAdd.PassTypeAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
