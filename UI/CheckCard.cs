using System;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Util;
using Common;
using ReaderInterface;
using TransferObjects;
using ACTAConfigManipulation;

namespace UI
{
	/// <summary>
	/// Summary description for Check.
	/// </summary>
	public class CheckCard : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbCriteria;
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.Label lblWU;
		private System.Windows.Forms.ComboBox cbEmployee;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.Label lblIdentification;
		private System.Windows.Forms.RadioButton rbPersonal;
		private System.Windows.Forms.RadioButton rbIdentificationCard;
		private System.Windows.Forms.Button btnReadCard;
		private System.Windows.Forms.GroupBox gbEmployee;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.TextBox tbFirstName;
		private System.Windows.Forms.TextBox tbLastName;
		private System.Windows.Forms.TextBox tbWU;
		private System.Windows.Forms.PictureBox pbEmplPicture;
		private System.Windows.Forms.Button btnClear;
		private DebugLog log;
		private List<WorkingUnitTO> workingunits = new List<WorkingUnitTO>();
		private List<EmployeeTO> employees1 = new List<EmployeeTO>();	
		private List<EmployeeTO> employees = new List<EmployeeTO>();	
		private List<TagTO> tags = new List<TagTO>();	
		private CultureInfo culture;
		private ResourceManager rm;
		IReaderInterface readerInterface;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox gbPasses;
		private System.Windows.Forms.ListView lvPasses;
		// List View indexes
		const int PassTypeIndex = 0;
		const int IssuedByIndex = 1;
		const int IssuedTimeIndex = 2;
		const int StartTimeIndex = 3;
		private ListViewItemComparer _comp;
		private System.Windows.Forms.Label lblEmployeeID;
		private System.Windows.Forms.TextBox tbEmployee;
		private System.Windows.Forms.Label lblType;
		private System.Windows.Forms.TextBox tbType;
		private System.Windows.Forms.TextBox tbWorkingHours;
		private System.Windows.Forms.Label lblWorkingHours;
		private System.Windows.Forms.TextBox tbSchema;
		private System.Windows.Forms.Label lblSchema;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        EmployeeImageFile eif = new EmployeeImageFile();
        public bool useDatabaseFiles = false;

		public CheckCard(List<EmployeeTO> employees, List<WorkingUnitTO> workingunits, List<TagTO> tags)
		{	
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			//this.employees1 = (ArrayList)deepCopy(employees, true);	
			this.employees = employees;
			this.workingunits = workingunits;
			this.tags = tags;
			InitializeComponent();
			this.CenterToScreen();

			ReaderFactory.TechnologyType = new Reader().GetDefaultTechnology();
			readerInterface = ReaderFactory.GetReader;

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.GateResource",typeof(CheckCard).Assembly);

			setLanguage();

			try
			{
                int databaseCount = eif.SearchCount(-1);
                if (databaseCount >= 0)
                    useDatabaseFiles = true;

                if (!useDatabaseFiles)
                {
                    if (Directory.Exists(Constants.EmployeePhotoDirectory))
                    {
                        if (File.Exists(Constants.EmployeePhotoDirectory + "whitehead.jpg"))
                        {
                            pbEmplPicture.Image = Image.FromFile(Constants.EmployeePhotoDirectory + "whitehead.jpg");
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noPictureDir", culture));
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noPictureDir", culture));
                    }
                }
                else
                {
                    try
                    {
                        //whitehead
                        pbEmplPicture.Image = Util.ResImages.whitehead;
                    }
                    catch
                    {
                        pbEmplPicture.Image = null;
                    }
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " CheckCard.CheckCard(): " + ex.Message + "\n");
				pbEmplPicture.Image = null;
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

		private void setLanguage()
		{
			try
			{
				// button's text
				btnClear.Text = rm.GetString("btnClear", culture);				
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnReadCard.Text = rm.GetString("btnReadCard", culture);
				
				// Form name				
				this.Text = rm.GetString("titleCheckCard", culture);
				
				// label's text				
				lblIdentification.Text = rm.GetString("lblIdentification", culture);
				rbPersonal.Text = rm.GetString("lblManual", culture);
				rbIdentificationCard.Text = rm.GetString("lblCard", culture);
				
				lblFirstName.Text = rm.GetString("lblFirstName", culture);
				lblLastName.Text = rm.GetString("lblLastName", culture);
				lblWU.Text = rm.GetString("lblWU", culture);
				lblWorkingUnit.Text = rm.GetString("lblWU", culture);
				lblEmployee.Text = rm.GetString("lblEmployee", culture);
				lblEmployeeID.Text = rm.GetString("lblEmployeeID", culture);
				lblType.Text = rm.GetString("lblType", culture);
				lblSchema.Text = rm.GetString("lblSchema", culture);
				lblWorkingHours.Text = rm.GetString("lblWorkingHours", culture);

				// group box's text
				gbCriteria.Text = rm.GetString("gbCriteria", culture);			
				gbEmployee.Text = rm.GetString("gbEmployee", culture);
				gbPasses.Text = rm.GetString("gbPasses", culture);
		
				// list view initialization
				lvPasses.BeginUpdate();
				lvPasses.Columns.Add(rm.GetString("hdrpassType", culture), (lvPasses.Width - 4) / 4, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrCreatedBy", culture), (lvPasses.Width - 4) / 4, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrCreatedTime", culture), (lvPasses.Width - 4) / 4, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrValid", culture), (lvPasses.Width - 4) / 4, HorizontalAlignment.Left);
				lvPasses.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " CheckCard.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbCriteria = new System.Windows.Forms.GroupBox();
            this.btnReadCard = new System.Windows.Forms.Button();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblIdentification = new System.Windows.Forms.Label();
            this.rbPersonal = new System.Windows.Forms.RadioButton();
            this.rbIdentificationCard = new System.Windows.Forms.RadioButton();
            this.gbEmployee = new System.Windows.Forms.GroupBox();
            this.tbWorkingHours = new System.Windows.Forms.TextBox();
            this.lblWorkingHours = new System.Windows.Forms.Label();
            this.tbSchema = new System.Windows.Forms.TextBox();
            this.lblSchema = new System.Windows.Forms.Label();
            this.tbType = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.lblEmployeeID = new System.Windows.Forms.Label();
            this.gbPasses = new System.Windows.Forms.GroupBox();
            this.lvPasses = new System.Windows.Forms.ListView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pbEmplPicture = new System.Windows.Forms.PictureBox();
            this.tbWU = new System.Windows.Forms.TextBox();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.gbCriteria.SuspendLayout();
            this.gbEmployee.SuspendLayout();
            this.gbPasses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmplPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCriteria
            // 
            this.gbCriteria.Controls.Add(this.btnReadCard);
            this.gbCriteria.Controls.Add(this.lblEmployee);
            this.gbCriteria.Controls.Add(this.lblWU);
            this.gbCriteria.Controls.Add(this.cbEmployee);
            this.gbCriteria.Controls.Add(this.cbWorkingUnit);
            this.gbCriteria.Controls.Add(this.lblIdentification);
            this.gbCriteria.Controls.Add(this.rbPersonal);
            this.gbCriteria.Controls.Add(this.rbIdentificationCard);
            this.gbCriteria.Location = new System.Drawing.Point(136, 24);
            this.gbCriteria.Name = "gbCriteria";
            this.gbCriteria.Size = new System.Drawing.Size(400, 128);
            this.gbCriteria.TabIndex = 0;
            this.gbCriteria.TabStop = false;
            this.gbCriteria.Text = "Search Criteria:";
            // 
            // btnReadCard
            // 
            this.btnReadCard.Location = new System.Drawing.Point(160, 56);
            this.btnReadCard.Name = "btnReadCard";
            this.btnReadCard.Size = new System.Drawing.Size(112, 23);
            this.btnReadCard.TabIndex = 4;
            this.btnReadCard.Text = "Read Card";
            this.btnReadCard.Click += new System.EventHandler(this.btnReadCard_Click);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(24, 88);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(128, 23);
            this.lblEmployee.TabIndex = 7;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEmployee.Visible = false;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(16, 56);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(136, 23);
            this.lblWU.TabIndex = 5;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblWU.Visible = false;
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(160, 88);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(176, 21);
            this.cbEmployee.TabIndex = 8;
            this.cbEmployee.Visible = false;
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmployee_SelectedIndexChanged);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(160, 56);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(176, 21);
            this.cbWorkingUnit.TabIndex = 6;
            this.cbWorkingUnit.Visible = false;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblIdentification
            // 
            this.lblIdentification.Location = new System.Drawing.Point(40, 24);
            this.lblIdentification.Name = "lblIdentification";
            this.lblIdentification.Size = new System.Drawing.Size(112, 23);
            this.lblIdentification.TabIndex = 1;
            this.lblIdentification.Text = "Identification:";
            this.lblIdentification.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbPersonal
            // 
            this.rbPersonal.Location = new System.Drawing.Point(272, 24);
            this.rbPersonal.Name = "rbPersonal";
            this.rbPersonal.Size = new System.Drawing.Size(88, 24);
            this.rbPersonal.TabIndex = 3;
            this.rbPersonal.Text = "Personal";
            this.rbPersonal.CheckedChanged += new System.EventHandler(this.rbPersonal_CheckedChanged);
            // 
            // rbIdentificationCard
            // 
            this.rbIdentificationCard.Checked = true;
            this.rbIdentificationCard.Location = new System.Drawing.Point(160, 24);
            this.rbIdentificationCard.Name = "rbIdentificationCard";
            this.rbIdentificationCard.Size = new System.Drawing.Size(112, 24);
            this.rbIdentificationCard.TabIndex = 2;
            this.rbIdentificationCard.TabStop = true;
            this.rbIdentificationCard.Text = "Identification card";
            // 
            // gbEmployee
            // 
            this.gbEmployee.Controls.Add(this.tbWorkingHours);
            this.gbEmployee.Controls.Add(this.lblWorkingHours);
            this.gbEmployee.Controls.Add(this.tbSchema);
            this.gbEmployee.Controls.Add(this.lblSchema);
            this.gbEmployee.Controls.Add(this.tbType);
            this.gbEmployee.Controls.Add(this.lblType);
            this.gbEmployee.Controls.Add(this.tbEmployee);
            this.gbEmployee.Controls.Add(this.lblEmployeeID);
            this.gbEmployee.Controls.Add(this.gbPasses);
            this.gbEmployee.Controls.Add(this.btnCancel);
            this.gbEmployee.Controls.Add(this.btnClear);
            this.gbEmployee.Controls.Add(this.pbEmplPicture);
            this.gbEmployee.Controls.Add(this.tbWU);
            this.gbEmployee.Controls.Add(this.tbLastName);
            this.gbEmployee.Controls.Add(this.tbFirstName);
            this.gbEmployee.Controls.Add(this.lblWorkingUnit);
            this.gbEmployee.Controls.Add(this.lblLastName);
            this.gbEmployee.Controls.Add(this.lblFirstName);
            this.gbEmployee.Location = new System.Drawing.Point(24, 168);
            this.gbEmployee.Name = "gbEmployee";
            this.gbEmployee.Size = new System.Drawing.Size(640, 408);
            this.gbEmployee.TabIndex = 9;
            this.gbEmployee.TabStop = false;
            this.gbEmployee.Text = "Employee";
            // 
            // tbWorkingHours
            // 
            this.tbWorkingHours.Enabled = false;
            this.tbWorkingHours.Location = new System.Drawing.Point(184, 208);
            this.tbWorkingHours.Name = "tbWorkingHours";
            this.tbWorkingHours.Size = new System.Drawing.Size(288, 20);
            this.tbWorkingHours.TabIndex = 23;
            // 
            // lblWorkingHours
            // 
            this.lblWorkingHours.Location = new System.Drawing.Point(72, 208);
            this.lblWorkingHours.Name = "lblWorkingHours";
            this.lblWorkingHours.Size = new System.Drawing.Size(100, 23);
            this.lblWorkingHours.TabIndex = 22;
            this.lblWorkingHours.Text = "Working Hours:";
            this.lblWorkingHours.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSchema
            // 
            this.tbSchema.Enabled = false;
            this.tbSchema.Location = new System.Drawing.Point(184, 176);
            this.tbSchema.Name = "tbSchema";
            this.tbSchema.Size = new System.Drawing.Size(288, 20);
            this.tbSchema.TabIndex = 21;
            // 
            // lblSchema
            // 
            this.lblSchema.Location = new System.Drawing.Point(72, 176);
            this.lblSchema.Name = "lblSchema";
            this.lblSchema.Size = new System.Drawing.Size(100, 23);
            this.lblSchema.TabIndex = 20;
            this.lblSchema.Text = "Schema:";
            this.lblSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbType
            // 
            this.tbType.Enabled = false;
            this.tbType.Location = new System.Drawing.Point(184, 144);
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(152, 20);
            this.tbType.TabIndex = 19;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(72, 144);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(100, 23);
            this.lblType.TabIndex = 18;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbEmployee
            // 
            this.tbEmployee.Enabled = false;
            this.tbEmployee.Location = new System.Drawing.Point(184, 16);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(152, 20);
            this.tbEmployee.TabIndex = 11;
            // 
            // lblEmployeeID
            // 
            this.lblEmployeeID.Location = new System.Drawing.Point(72, 16);
            this.lblEmployeeID.Name = "lblEmployeeID";
            this.lblEmployeeID.Size = new System.Drawing.Size(100, 23);
            this.lblEmployeeID.TabIndex = 10;
            this.lblEmployeeID.Text = "Employee:";
            this.lblEmployeeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbPasses
            // 
            this.gbPasses.Controls.Add(this.lvPasses);
            this.gbPasses.Location = new System.Drawing.Point(16, 248);
            this.gbPasses.Name = "gbPasses";
            this.gbPasses.Size = new System.Drawing.Size(616, 120);
            this.gbPasses.TabIndex = 24;
            this.gbPasses.TabStop = false;
            this.gbPasses.Text = "Passes";
            // 
            // lvPasses
            // 
            this.lvPasses.FullRowSelect = true;
            this.lvPasses.GridLines = true;
            this.lvPasses.HideSelection = false;
            this.lvPasses.Location = new System.Drawing.Point(8, 24);
            this.lvPasses.Name = "lvPasses";
            this.lvPasses.Size = new System.Drawing.Size(600, 80);
            this.lvPasses.TabIndex = 25;
            this.lvPasses.UseCompatibleStateImageBehavior = false;
            this.lvPasses.View = System.Windows.Forms.View.Details;
            this.lvPasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPasses_ColumnClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(488, 376);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(400, 376);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 26;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pbEmplPicture
            // 
            this.pbEmplPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbEmplPicture.Location = new System.Drawing.Point(384, 16);
            this.pbEmplPicture.Name = "pbEmplPicture";
            this.pbEmplPicture.Size = new System.Drawing.Size(90, 135);
            this.pbEmplPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbEmplPicture.TabIndex = 6;
            this.pbEmplPicture.TabStop = false;
            // 
            // tbWU
            // 
            this.tbWU.Enabled = false;
            this.tbWU.Location = new System.Drawing.Point(184, 112);
            this.tbWU.Name = "tbWU";
            this.tbWU.Size = new System.Drawing.Size(152, 20);
            this.tbWU.TabIndex = 17;
            // 
            // tbLastName
            // 
            this.tbLastName.Enabled = false;
            this.tbLastName.Location = new System.Drawing.Point(184, 80);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(152, 20);
            this.tbLastName.TabIndex = 15;
            // 
            // tbFirstName
            // 
            this.tbFirstName.Enabled = false;
            this.tbFirstName.Location = new System.Drawing.Point(184, 48);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(152, 20);
            this.tbFirstName.TabIndex = 13;
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(72, 112);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(100, 23);
            this.lblWorkingUnit.TabIndex = 16;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(72, 80);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(100, 23);
            this.lblLastName.TabIndex = 14;
            this.lblLastName.Text = "Last Name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(72, 48);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(100, 23);
            this.lblFirstName.TabIndex = 12;
            this.lblFirstName.Text = "First Name:";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CheckCard
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(672, 593);
            this.ControlBox = false;
            this.Controls.Add(this.gbEmployee);
            this.Controls.Add(this.gbCriteria);
            this.MaximumSize = new System.Drawing.Size(680, 620);
            this.MinimumSize = new System.Drawing.Size(680, 620);
            this.Name = "CheckCard";
            this.ShowInTaskbar = false;
            this.Text = "Check Card";
            this.Load += new System.EventHandler(this.CheckCard_Load);
            this.gbCriteria.ResumeLayout(false);
            this.gbEmployee.ResumeLayout(false);
            this.gbEmployee.PerformLayout();
            this.gbPasses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbEmplPicture)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        public void populateListView(List<ExitPermissionTO> exitPermList)
		{
			try
			{
				lvPasses.BeginUpdate();
				lvPasses.Items.Clear();

				if (exitPermList.Count > 0)
				{
					foreach(ExitPermissionTO exitPerm in exitPermList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = exitPerm.PassTypeDesc;
						
						item.SubItems.Add(exitPerm.UserName.Trim());
						if (!exitPerm.IssuedTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
						{
							item.SubItems.Add(exitPerm.IssuedTime.ToString("dd.MM.yyyy   HH:mm"));
						}
						else
						{								
							item.SubItems.Add("");
						}
						item.SubItems.Add(exitPerm.StartTime.ToString("dd.MM.yyyy   HH:mm").Trim() + " + " + exitPerm.Offset.ToString() + "min");
						
						lvPasses.Items.Add(item);
					}
				}

				lvPasses.EndUpdate();
				lvPasses.Invalidate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " CheckCard.populateListView(): " + ex.Message + "\n");
				throw new Exception("Exception: " + ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Dispose();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CheckCard.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
			
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                clearControls(this.Controls, true);
                lvPasses.Items.Clear();
                rbIdentificationCard.Checked = true;
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " CheckCard.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (cbWorkingUnit.SelectedValue is int)
				{
					populateEmployeeCombo((int) cbWorkingUnit.SelectedValue);
				}
				else
				{
					populateEmployeeCombo(-1);
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populateEmployeeCombo(int wuID)
		{
            try
            {
                this.employees1 = (List<EmployeeTO>)deepCopy(employees, true);
                string name = "";
                foreach (EmployeeTO employee in employees1)
                {
                    name = employee.LastName + " " + employee.FirstName;
                    employee.LastName = name;
                }

                List<EmployeeTO> emplArrayCombo = new List<EmployeeTO>();

                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);
                
                string vistorsCode = ConfigurationManager.AppSettings["VisitorsCode"];

                if (vistorsCode == null)
                {
                    MessageBox.Show(rm.GetString("noVisitorsParameters", culture));

                    ConfigAdd conf = new ConfigAdd(rm.GetString("Visitors", culture));

                    conf.ShowDialog();

                    vistorsCode = ConfigurationManager.AppSettings["VisitorsCode"];
                }

                if (wuID == -1)
                {
                    foreach (EmployeeTO currentEmployee in employees1)
                    {
                        foreach (string status in statuses)
                        {
                            if (currentEmployee.Status.ToString().Trim().Equals(status.Trim()) && !currentEmployee.WorkingUnitID.ToString().Trim().Equals(vistorsCode.Trim()))
                            {
                                emplArrayCombo.Add(currentEmployee);
                            }
                        }
                    }
                }
                else
                {
                    foreach (EmployeeTO currentEmployee in employees1)
                    {
                        foreach (string status in statuses)
                        {
                            if (currentEmployee.Status.ToString().Trim().Equals(status.Trim()) && currentEmployee.WorkingUnitID.ToString().Trim().Equals(wuID.ToString().Trim()))
                            {
                                emplArrayCombo.Add(currentEmployee);
                            }
                        }
                    }
                }

                EmployeeTO empl1 = new EmployeeTO();
                empl1.LastName = rm.GetString("all", culture);
                emplArrayCombo.Insert(0, empl1);

                cbEmployee.DataSource = emplArrayCombo;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CheckCard.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

		private void populateWorkingUnitCombo()
		{
			try
			{
				ArrayList wuArrayCombo = new ArrayList();

                string visitorsCode = ConfigurationManager.AppSettings["VisitorsCode"];
                if (visitorsCode == null)
                {
                    MessageBox.Show(rm.GetString("noVisitorsParameters", culture));

                    ConfigAdd conf = new ConfigAdd(rm.GetString("Visitors", culture));

                    conf.ShowDialog();

                    visitorsCode = ConfigurationManager.AppSettings["VisitorsCode"];
                }

                foreach (WorkingUnitTO currentWU in workingunits)
                {
                    if (!currentWU.WorkingUnitID.ToString().Trim().Equals(visitorsCode.Trim()))
                    {
                        wuArrayCombo.Add(currentWU);
                    }
                }
								
				wuArrayCombo.Insert(0, new WorkingUnit(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArrayCombo;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";
				//cbWU.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " CheckCard.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		public object deepCopy(object array, bool doDeepCopy)
		{
			if (doDeepCopy)
			{
				BinaryFormatter BF = new BinaryFormatter();
				MemoryStream memStream = new MemoryStream();
				BF.Serialize(memStream, array);
				memStream.Flush();
				memStream.Position = 0;
				return (BF.Deserialize(memStream));
			}
			else
			{
				return (this.MemberwiseClone());
			}
		}

		private void rbPersonal_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( rbPersonal.Checked == true)
			{
				btnReadCard.Visible = false;
				populateWorkingUnitCombo();
				cbWorkingUnit.Visible = true;
				cbEmployee.Visible = true;
				lblWU.Visible = true;
				lblEmployee.Visible = true;
			}
			else
			{
				btnReadCard.Visible = true;
				cbWorkingUnit.Visible = false;
				cbEmployee.Visible = false;
				lblWU.Visible = false;
				lblEmployee.Visible = false;
			}
			clearControls(this.Controls, false);
			lvPasses.Items.Clear();
		}

		private void btnReadCard_Click(object sender, System.EventArgs e)
		{
			try
			{
                

				//int COMPort = Int32.Parse(ConfigurationManager.AppSettings["DesktopReaderAddress"]);
                uint tagID = 0;
                this.Cursor = Cursors.WaitCursor;
                int COMPort = readerInterface.FindDesktopReader();
                this.Cursor = Cursors.Arrow;
                if (COMPort == 0)
                {
                    MessageBox.Show(rm.GetString("noDesktopReader", culture));
                    return;
                }
                else
                {
                    tagID = UInt32.Parse(readerInterface.GetTagID(COMPort));
                }
			
				if (tagID == 0)
				{
					MessageBox.Show(rm.GetString("noTagOnReader", culture));
					clearControls(this.Controls, true);	
					lvPasses.Items.Clear();
					rbIdentificationCard.Checked = true;
				}
				else
				{
					List<EmployeeTO> tmpEmployee = new List<EmployeeTO>();
					foreach(TagTO currentTag in tags)
					{
						if( currentTag.TagID.ToString().Trim().Equals(tagID.ToString().Trim()))
						{
							foreach(EmployeeTO currentEmployee in employees)
							{
								if(currentEmployee.EmployeeID.ToString().Trim().Equals(currentTag.OwnerID.ToString().Trim()))
								{
									tmpEmployee.Add(currentEmployee);
									break;
								}
							}
						}
					}
					//cbWorkingUnit.SelectedValue = int.Parse(((Employee)tmpEmployee[0]).WorkingUnitID.ToString());
					//cbEmployee.SelectedValue = int.Parse(((Employee)tmpEmployee[0]).EmployeeID.ToString());
					
					if ( tmpEmployee.Count > 0 )
					{					
						tbEmployee.Text = tmpEmployee[0].EmployeeID.ToString();
						tbFirstName.Text = tmpEmployee[0].FirstName.ToString();
						string lastName = tmpEmployee[0].LastName.ToString();
						//tbLastName.Text = lastName.Substring(0, lastName.IndexOf(" "));
						tbLastName.Text = lastName;

						string wuName = "";
						foreach(WorkingUnitTO currentWU in workingunits)
						{						
							if(currentWU.WorkingUnitID == tmpEmployee[0].WorkingUnitID)
							{
								wuName = currentWU.Name.ToString();
							}												
						}
						tbWU.Text = wuName;

						string emplType = "";
						if (tmpEmployee[0].Type.Trim().Equals(Constants.emplOrdinary))
						{
							emplType = rm.GetString("emplOrdinary", culture);
						}
						else if (tmpEmployee[0].Type.Trim().Equals(Constants.emplExtraOrdinary))
						{
							emplType = rm.GetString("emplExtraOrdinary", culture);
						}
						else if (tmpEmployee[0].Type.Trim().Equals(Constants.emplSpecial))
						{
							emplType = rm.GetString("emplSpecial", culture);
						}
						tbType.Text = emplType;

                        Employee empl = new Employee();
                        empl.EmplTO = tmpEmployee[0];
						ArrayList emplSchema = empl.findTimeSchema(DateTime.Now);
						if (emplSchema.Count <= 0)
						{
							tbSchema.Text = "";
							tbWorkingHours.Text = "";
						}
						else
						{
							tbSchema.Text = ((WorkTimeSchemaTO)emplSchema[0]).Name.Trim();
							if (emplSchema.Count > 1)
							{
								string workingHours = "";
								Dictionary<int, WorkTimeIntervalTO> intervals = ((WorkTimeSchemaTO)emplSchema[0]).Days[(int)emplSchema[1]];
								foreach (int intNum in intervals.Keys)
								{
									workingHours += intervals[intNum].StartTime.ToString("HH:mm");
									workingHours += "-";
									workingHours += intervals[intNum].EndTime.ToString("HH:mm");
									workingHours += ", ";
								}

								if (workingHours.Length > 0)
								{
									workingHours = workingHours.Substring(0, workingHours.Length - 2);
								}

								tbWorkingHours.Text = workingHours.Trim();
							}
							else
							{
								tbWorkingHours.Text = "";
							}
						}

						string imageName = tmpEmployee[0].Picture.ToString();
                        if (!useDatabaseFiles)
                        {
                            if (imageName != "")
                            {
                                try
                                {
                                    pbEmplPicture.Image = Image.FromFile(Constants.EmployeePhotoDirectory + imageName);
                                }
                                catch
                                {
                                    MessageBox.Show(rm.GetString("noPictureFile", culture));
                                    try
                                    {
                                        pbEmplPicture.Image = Image.FromFile(Constants.EmployeePhotoDirectory + "whitehead.jpg");
                                    }
                                    catch
                                    {
                                        pbEmplPicture.Image = null;
                                    }
                                }
                            } //if (imageName != "")
                            else
                            {
                                try
                                {
                                    pbEmplPicture.Image = Image.FromFile(Constants.EmployeePhotoDirectory + "whitehead.jpg");
                                }
                                catch
                                {
                                    pbEmplPicture.Image = null;
                                }
                            } 
                        }
                        else
                        {
                            if (imageName != "")
                            {
                                ArrayList al = eif.Search(tmpEmployee[0].EmployeeID);
                                if (al.Count > 0)
                                {
                                    byte[] emplPhoto = ((EmployeeImageFile)al[0]).Picture;

                                    MemoryStream memStream = new MemoryStream(emplPhoto);

                                    // Set the position to the beginning of the stream.
                                    memStream.Seek(0, SeekOrigin.Begin);

                                    pbEmplPicture.Image = new Bitmap(memStream);

                                    memStream.Close();
                                }
                                else
                                {
                                    MessageBox.Show(rm.GetString("noPictureFile", culture));

                                    try
                                    {
                                        //whitehead
                                        pbEmplPicture.Image = Util.ResImages.whitehead;
                                    }
                                    catch
                                    {
                                        pbEmplPicture.Image = null;
                                    }
                                }
                            } //if (imageName != "")
                            else
                            {
                                try
                                {
                                    //whitehead
                                    pbEmplPicture.Image = Util.ResImages.whitehead;
                                }
                                catch
                                {
                                    pbEmplPicture.Image = null;
                                }
                            }
                        }
					
						ExitPermission exitPerm = new ExitPermission();
                        List<ExitPermissionTO> exitPermList = exitPerm.SearchValid(tmpEmployee[0].EmployeeID.ToString());
						populateListView(exitPermList);
					}
					else
					{
						MessageBox.Show(rm.GetString("UnknownTag", culture));
						clearControls(this.Controls, true);	
						lvPasses.Items.Clear();
						rbIdentificationCard.Checked = true;
					}
				}				
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Tags.btnReadCard_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void clearControls(Control.ControlCollection controls, bool radio)
		{
			try
			{
				foreach(Control c in controls)
				{									
					if ( c is TextBox)
					{
						TextBox tb = (TextBox)c;	
						tb.Text = "";
					}
					if ( c is RichTextBox)
					{
						RichTextBox rtb = (RichTextBox)c;	
						rtb.Text = "";
					}
					if ( c is ComboBox)
					{
						ComboBox cb = (ComboBox)c;	
						if ( cb.Items.Count > 0 )
						{
							cb.SelectedIndex = 0;
						}
						else
						{
							cb.SelectedIndex = -1;
						}
					}
					if ( c is RadioButton)
					{
						if ( radio )
						{
							RadioButton rb = (RadioButton)c;	
							rb.Checked = false;
						}
					}					
					if ( c is PictureBox)
					{
						PictureBox pb = (PictureBox)c;
                        if (!useDatabaseFiles)
                        {
                            try
                            {
                                pb.Image = Image.FromFile(Constants.EmployeePhotoDirectory + "whitehead.jpg");
                            }
                            catch
                            {
                                pb.Image = null;
                            }
                        }
                        else
                        {
                            try
                            {
                                //whitehead
                                pb.Image = Util.ResImages.whitehead;
                            }
                            catch
                            {
                                pb.Image = null;
                            }
                        }
					}
					if ( c.HasChildren )
					{
						clearControls(c.Controls, radio);
					}
				}
			}
			catch(Exception e)
			{
				throw e;
			}			
		}

        public void emptyTextBoxesControls(Control.ControlCollection controls)
        {
            try
            {
                foreach (Control c in controls)
                {
                    if (c is TextBox)
                    {
                        TextBox tb = (TextBox)c;
                        tb.Text = "";
                    }
                    if (c is RichTextBox)
                    {
                        RichTextBox rtb = (RichTextBox)c;
                        rtb.Text = "";
                    }
                    if (c is PictureBox)
                    {
                        PictureBox pb = (PictureBox)c;
                        if (!useDatabaseFiles)
                        {
                            try
                            {
                                pb.Image = Image.FromFile(Constants.EmployeePhotoDirectory + "whitehead.jpg");
                            }
                            catch
                            {
                                pb.Image = null;
                            }
                        }
                        else
                        {
                            try
                            {
                                //whitehead
                                pb.Image = Util.ResImages.whitehead;
                            }
                            catch
                            {
                                pb.Image = null;
                            }
                        }
                    }
                    if (c.HasChildren)
                    {
                        emptyTextBoxesControls(c.Controls);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }		
        }
        
		private void cbEmployee_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                if (cbEmployee.SelectedIndex != 0)
                {
                    List<EmployeeTO> tmpEmployee = new List<EmployeeTO>();
                    foreach (EmployeeTO currentEmployee in employees)
                    {
                        if (currentEmployee.EmployeeID.ToString().Trim().Equals(cbEmployee.SelectedValue.ToString().Trim()))
                        {
                            tmpEmployee.Add(currentEmployee);
                        }
                    }
                    if (tmpEmployee.Count > 0)
                    {
                        tbEmployee.Text = tmpEmployee[0].EmployeeID.ToString();
                        tbFirstName.Text = tmpEmployee[0].FirstName.ToString();
                        string lastName = tmpEmployee[0].LastName.ToString();
                        tbLastName.Text = tmpEmployee[0].LastName.ToString();
                        string wuName = "";
                        foreach (WorkingUnitTO currentWU in workingunits)
                        {
                            if (currentWU.WorkingUnitID == tmpEmployee[0].WorkingUnitID)
                            {
                                wuName = currentWU.Name.ToString();
                            }
                        }
                        tbWU.Text = wuName;
                        string emplType = "";
                        if (tmpEmployee[0].Type.Trim().Equals(Constants.emplOrdinary))
                        {
                            emplType = rm.GetString("emplOrdinary", culture);
                        }
                        else if (tmpEmployee[0].Type.Trim().Equals(Constants.emplExtraOrdinary))
                        {
                            emplType = rm.GetString("emplExtraOrdinary", culture);
                        }
                        else if (tmpEmployee[0].Type.Trim().Equals(Constants.emplSpecial))
                        {
                            emplType = rm.GetString("emplSpecial", culture);
                        }
                        tbType.Text = emplType;

                        Employee empl = new Employee();
                        empl.EmplTO = tmpEmployee[0];
                        ArrayList emplSchema = empl.findTimeSchema(DateTime.Now);
                        if (emplSchema.Count <= 0)
                        {
                            tbSchema.Text = "";
                            tbWorkingHours.Text = "";
                        }
                        else
                        {
                            tbSchema.Text = ((WorkTimeSchemaTO)emplSchema[0]).Name.Trim();
                            if (emplSchema.Count > 1)
                            {
                                string workingHours = "";
                                Dictionary<int, WorkTimeIntervalTO> intervals = ((WorkTimeSchemaTO)emplSchema[0]).Days[(int)emplSchema[1]];
                                foreach (int intNum in intervals.Keys)
                                {
                                    workingHours += intervals[intNum].StartTime.ToString("HH:mm");
                                    workingHours += "-";
                                    workingHours += intervals[intNum].EndTime.ToString("HH:mm");
                                    workingHours += ", ";
                                }

                                if (workingHours.Length > 0)
                                {
                                    workingHours = workingHours.Substring(0, workingHours.Length - 2);
                                }

                                tbWorkingHours.Text = workingHours.Trim();
                            }
                            else
                            {
                                tbWorkingHours.Text = "";
                            }
                        }

                        string imageName = tmpEmployee[0].Picture.ToString();
                        if (!useDatabaseFiles)
                        {
                            if (imageName != "")
                            {
                                try
                                {
                                    pbEmplPicture.Image = Image.FromFile(Constants.EmployeePhotoDirectory + imageName);
                                }
                                catch
                                {
                                    MessageBox.Show(rm.GetString("noPictureFile", culture));
                                    try
                                    {
                                        pbEmplPicture.Image = Image.FromFile(Constants.EmployeePhotoDirectory + "whitehead.jpg");
                                    }
                                    catch
                                    {
                                        pbEmplPicture.Image = null;
                                    }
                                }
                            } //if (imageName != "")
                            else
                            {
                                try
                                {
                                    pbEmplPicture.Image = Image.FromFile(Constants.EmployeePhotoDirectory + "whitehead.jpg");
                                }
                                catch
                                {
                                    pbEmplPicture.Image = null;
                                }
                            }
                        }
                        else
                        {
                            if (imageName != "")
                            {
                                ArrayList al = eif.Search(tmpEmployee[0].EmployeeID);
                                if (al.Count > 0)
                                {
                                    byte[] emplPhoto = ((EmployeeImageFile)al[0]).Picture;

                                    MemoryStream memStream = new MemoryStream(emplPhoto);

                                    // Set the position to the beginning of the stream.
                                    memStream.Seek(0, SeekOrigin.Begin);

                                    pbEmplPicture.Image = new Bitmap(memStream);

                                    memStream.Close();
                                }
                                else
                                {
                                    MessageBox.Show(rm.GetString("noPictureFile", culture));

                                    try
                                    {
                                        //whitehead
                                        pbEmplPicture.Image = Util.ResImages.whitehead;
                                    }
                                    catch
                                    {
                                        pbEmplPicture.Image = null;
                                    }
                                }
                            } //if (imageName != "")
                            else
                            {
                                try
                                {
                                    //whitehead
                                    pbEmplPicture.Image = Util.ResImages.whitehead;
                                }
                                catch
                                {
                                    pbEmplPicture.Image = null;
                                }
                            }
                        }

                        ExitPermission exitPerm = new ExitPermission();
                        List<ExitPermissionTO> exitPermList = exitPerm.SearchValid(tmpEmployee[0].EmployeeID.ToString());
                        populateListView(exitPermList);
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("UnknownTag", culture));
                        clearControls(this.Controls, true);
                        lvPasses.Items.Clear();
                        rbIdentificationCard.Checked = true;
                    }
                }
                else
                {
                    emptyTextBoxesControls(this.Controls);
                    lvPasses.Items.Clear();
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " CheckCard.cbEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}				
		}

		private void lvPasses_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
				SortOrder prevOrder = lvPasses.Sorting;

				if (e.Column == _comp.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvPasses.Sorting = SortOrder.Descending;
					}
					else
					{
						lvPasses.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp.SortColumn = e.Column;
					lvPasses.Sorting = SortOrder.Ascending;
				}
                lvPasses.Sort();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " CheckCard.lvPasses_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show("Exception in lvPasses_ColumnClick():" + ex.Message);
			}
		}

		#region Inner Class for sorting items in View List

		/*
		 *  Class used for sorting items in the List View 
		*/
		private class ListViewItemComparer : IComparer
		{
			private ListView _listView;

			public ListViewItemComparer(ListView lv)
			{
				_listView = lv;
			}
			public ListView ListView
			{
				get{return _listView;}
			}

			private int _sortColumn = 0;

			public int SortColumn
			{
				get { return _sortColumn; }
				set { _sortColumn = value; }
			}

			public int Compare(object a, object b)
			{
				ListViewItem item1 = (ListViewItem) a;
				ListViewItem item2 = (ListViewItem) b;

				if (ListView.Sorting == SortOrder.Descending)
				{
					ListViewItem temp = item1;
					item1 = item2;
					item2 = temp;
				}
				// Handle non Detail Cases
				return CompareItems(item1, item2);
			}

			public int CompareItems(ListViewItem item1, ListViewItem item2)
			{
				
				// Subitem instances
				ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
				ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

				// Return value based on sort column	
				switch (SortColumn)
				{
                    case CheckCard.PassTypeIndex:
                    case CheckCard.IssuedByIndex:
                    case CheckCard.StartTimeIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case CheckCard.IssuedTimeIndex:					
					{
                        
                        DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                        DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                        if (!sub1.Text.Trim().Equals(""))
                        {
                            dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy   HH:mm",null);
                        }

                        if (!sub2.Text.Trim().Equals(""))
                        {
                            dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy   HH:mm",null);
                        }

                        return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
					}
					/*case CheckCard.StartTimeIndex:
					{
						IFormatProvider culture = new CultureInfo("fr-FR", true);
						DateTime dt1 = new DateTime(1,1,1,0,0,0);
						DateTime dt2 = new DateTime(1,1,1,0,0,0);

                        int index = -1;
                        string text1 = "";
                        index = sub1.Text.Trim().LastIndexOf("+");
                        if (index > 0)
                            text1 = sub1.Text.Trim().Substring(0, index).Trim();

                        string text2 = "";
                        index = sub2.Text.Trim().LastIndexOf("+");
                        if (index > 0)
                            text2 = sub2.Text.Trim().Substring(0, index).Trim();


                        if (!text1.Equals("")) 
						{
                            dt1 = DateTime.Parse(text1, culture, DateTimeStyles.NoCurrentDateDefault);
						}

                        if (!text2.Equals(""))
						{
                            dt2 = DateTime.Parse(text2, culture, DateTimeStyles.NoCurrentDateDefault);
						}
						
						return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
					}*/
					default:
						throw new IndexOutOfRangeException("Unrecognized column name extension");
				}
			}
		}

		#endregion

        private void CheckCard_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvPasses);
                lvPasses.ListViewItemSorter = _comp;
                lvPasses.Sorting = SortOrder.Ascending;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CheckCard.CheckCard_Load(): " + ex.Message + "\n");
                MessageBox.Show("Exception in CheckCard_Load():" + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
