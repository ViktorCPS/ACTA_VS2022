using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Data;
using System.IO;

using Common;
using TransferObjects;
using Util;
using DataAccess;

namespace Reports
{
	/// <summary>
	/// Summary description for LocationsReports.
	/// </summary>
	public class TagsReports : System.Windows.Forms.Form
	{
		private System.Windows.Forms.CheckBox chbCSV;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		DebugLog debug;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblDocFormat;

		// Working Units that logInUser is granted to
		List<WorkingUnitTO> wUnits;
		private System.Windows.Forms.CheckBox cbTXT;
		private System.Windows.Forms.CheckBox cbCR;

		public TagsReports()
		{
			InitializeComponent();
			this.CenterToScreen();

			// Init debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			// Language tool
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
			setLanguage();

			// Get User that is login and Working Units that User is granted to
			logInUser = NotificationController.GetLogInUser();
			if (logInUser != null)
			{
				wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
			}

			DateTime date = DateTime.Now;
			dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
			dtpToDate.Value = date;
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
            this.lblDocFormat = new System.Windows.Forms.Label();
            this.chbCSV = new System.Windows.Forms.CheckBox();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbTXT = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.gbTimeInterval.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(16, 128);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(152, 23);
            this.lblDocFormat.TabIndex = 9;
            this.lblDocFormat.Text = "Document format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chbCSV
            // 
            this.chbCSV.Enabled = false;
            this.chbCSV.Location = new System.Drawing.Point(184, 128);
            this.chbCSV.Name = "chbCSV";
            this.chbCSV.Size = new System.Drawing.Size(48, 24);
            this.chbCSV.TabIndex = 10;
            this.chbCSV.Text = "CSV";
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Location = new System.Drawing.Point(8, 16);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(352, 100);
            this.gbTimeInterval.TabIndex = 4;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(96, 64);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(200, 20);
            this.dtpToDate.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(24, 64);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(80, 23);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(32, 32);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(56, 23);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(96, 32);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(200, 20);
            this.dtpFromDate.TabIndex = 6;
            this.dtpFromDate.Value = new System.DateTime(2007, 2, 1, 0, 0, 0, 0);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(40, 176);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 23);
            this.btnGenerate.TabIndex = 13;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(232, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbTXT
            // 
            this.cbTXT.Location = new System.Drawing.Point(248, 128);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(48, 24);
            this.cbTXT.TabIndex = 11;
            this.cbTXT.Text = "TXT";
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Location = new System.Drawing.Point(312, 128);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(48, 24);
            this.cbCR.TabIndex = 12;
            this.cbCR.Text = "CR";
            // 
            // TagsReports
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(368, 221);
            this.ControlBox = false;
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.chbCSV);
            this.Controls.Add(this.gbTimeInterval);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(376, 248);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(376, 248);
            this.Name = "TagsReports";
            this.ShowInTaskbar = false;
            this.Text = "Tags Reports";
            this.Load += new System.EventHandler(this.TagsReports_Load);
            this.gbTimeInterval.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void btnGenerate_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (wUnits.Count == 0)
				{
					MessageBox.Show(rm.GetString("noWUGranted", culture));
				}
				else
				{
					string wuString = "";
					foreach (WorkingUnitTO wu in wUnits)
					{
						wuString += wu.WorkingUnitID.ToString().Trim() + ","; 
					}
				
					if (wuString.Length > 0)
					{
						wuString = wuString.Substring(0, wuString.Length - 1);
					}

					List<TagTO> tagsRep = new Tag().SearchInactiveTags(wuString, dtpFromDate.Value.Date, dtpToDate.Value.Date);
					if (tagsRep.Count == 0)
					{
						this.Cursor = Cursors.Arrow;
						MessageBox.Show(rm.GetString("dataNotFound", culture));
						return;
					}

					if (this.chbCSV.Checked)
					{
						this.generateCSVReport(tagsRep);
					}

					if (this.cbTXT.Checked)
					{
						this.generateTXTReport(tagsRep);
					}

					if (this.cbCR.Checked)
					{
						if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
						{
							Reports.Reports_sr.RetiredTagsCRView rep  = new Reports.Reports_sr.RetiredTagsCRView(this.PopulateCRData(tagsRep));
							rep.ShowDialog(this);
						}
						else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
						{
							Reports.Reports_en.RetiredTagsCRView_en rep  = new Reports.Reports_en.RetiredTagsCRView_en(this.PopulateCRData(tagsRep));
							rep.ShowDialog(this);
						}
					}

					//this.Close();
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " TagsReports.btnGenerate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void generateCSVReport(List<TagTO> tagsRep)
		{
			try
			{
				ArrayList tableData = new ArrayList();
				int index = 0;

				for(int i = 0; i < tagsRep.Count; i++)
				{
					TagTO tag = tagsRep[i];
					index++;
					string[] rowData = {index.ToString(), tag.ModifiedTime.ToString("dd.MM.yyy"), tag.TagID.ToString(), 
										   tag.EmployeeName, tag.Status, tag.ModifiedBy};

					tableData.Add(rowData);
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("datum",typeof(System.String));
				table.Columns.Add("kartica",typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("status", typeof(System.String));
				table.Columns.Add("promenio", typeof(System.String));
				
				for(int i = 0; i < tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j = 0; j < al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5};
				string[] cHeaders = {"rbr", "Datum", "Kartica", "Ime", "Status", "Promenio"};

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
				{
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					Export objExport = new Export("Win", cHeaders);

					string fileName = Constants.csvDocPath + "\\Izvestaj o nevazecim karticama-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".csv";
				
					objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " TagsReports.generateCSVReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateTXTReport(List<TagTO> tagsRep)
		{
			try
			{
				ArrayList tableData = new ArrayList();
				int index = 0;

				for(int i = 0; i < tagsRep.Count; i++)
				{
					TagTO tag = tagsRep[i];
					index++;
					string[] rowData = {index.ToString(), tag.ModifiedTime.ToString("dd.MM.yyy"), tag.TagID.ToString(), 
										   tag.EmployeeName, tag.Status, tag.ModifiedBy};

					tableData.Add(rowData);
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("datum",typeof(System.String));
				table.Columns.Add("kartica",typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("status", typeof(System.String));
				table.Columns.Add("promenio", typeof(System.String));
				
				for(int i = 0; i < tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j = 0; j < al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

                // Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5};
				string[] cHeaders = {"rbr", "Datum", "Kartica", "Ime", "Status", "Promenio"};

				// Export the details of specified columns to Excel
				if (table.Rows.Count == 0)
				{
					MessageBox.Show(rm.GetString("noTXTExport", culture));
				}
				else
				{
					Export objExport = new Export("Win", cHeaders);

					string fileName = Constants.txtDocPath + "\\Izvestaj o nevazecim karticama--" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";
				
					objExport.ExportDetails(table, Export.ExportFormat.Excel, fileName);					
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " LocationsReports.generateTXTReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("tagsReports", culture);

				lblFrom.Text = rm.GetString("lblFrom", culture);
				gbTimeInterval.Text = rm.GetString("timeInterval", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
				lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
				btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " TagsReports.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}			
		}

		/// <summary>
		/// Populate LocReportDS DataSet to create LocationCR
		/// CrystalReports report
		/// </summary>
		/// <param name="dataList">Data list</param>
		/// <returns>Instance of LocReportDS</returns>
		private DataSet PopulateCRData(List<TagTO> dataList)
		{
			DataSet dataSet = new DataSet();
            DataTable table = new DataTable("tag");
			table.Columns.Add("first_name", typeof(System.String));
			table.Columns.Add("last_name", typeof(System.String));
			table.Columns.Add("owner_id", typeof(int));
			table.Columns.Add("issued", typeof(System.DateTime));
			table.Columns.Add("valid_to", typeof(System.DateTime));
			table.Columns.Add("status", typeof(System.String));
			table.Columns.Add("description", typeof(System.String));
			table.Columns.Add("created_by", typeof(System.String));
			table.Columns.Add("created_time", typeof(System.DateTime));
			table.Columns.Add("modified_by", typeof(System.String));
			table.Columns.Add("modified_time", typeof(System.DateTime));
			table.Columns.Add("tag_id", typeof(System.String));
            table.Columns.Add("imageID", typeof(byte));

            DataTable tableI = new DataTable("images");
            tableI.Columns.Add("image", typeof(System.Byte[]));
            tableI.Columns.Add("imageID", typeof(byte));

			try
			{
                int i = 0;
				foreach(TagTO tag in dataList)
				{
					DataRow row = table.NewRow();
					
					row["first_name"] = tag.EmployeeName;
					row["owner_id"] = tag.OwnerID;
					row["issued"] = tag.Issued;
					row["valid_to"] = tag.ValidTO;
					row["status"] = tag.Status;
					row["description"] = tag.Description;
					row["modified_by"] = tag.ModifiedBy;
					row["modified_time"] = tag.ModifiedTime;
					row["tag_id"] = tag.TagID.ToString();

                    row["imageID"] = 1;
                    if (i == 0)
                    {
                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();
                    }
		
					table.Rows.Add(row);
                    i++;
				}

				table.AcceptChanges();
			}	
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " LocationsReports.generateCSVReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			
			dataSet.Tables.Add(table);
            dataSet.Tables.Add(tableI);
			return dataSet;
		}

		private void TagsReports_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
