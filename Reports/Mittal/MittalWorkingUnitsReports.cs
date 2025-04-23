using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Text;

using Common;
using TransferObjects;
using Util;

namespace Reports.Mittal
{
	/// <summary>
	/// Summary description for MittalWorkingUnitsReports.
	/// </summary>
	public class MittalWorkingUnitsReports : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbWorkingUnit;
		private System.Windows.Forms.Label lblWorkingUnitName;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.CheckBox checkbCSV;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblDocFormat;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.CheckBox chbHierarhicly;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;

		DebugLog debug;
		private System.Windows.Forms.CheckBox cbTXT;
		private System.Windows.Forms.CheckBox cbCR;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

		// Working Units that logInUser is granted to
		List<WorkingUnitTO> wUnits;

        Filter filter;

		public MittalWorkingUnitsReports()
		{
			InitializeComponent();

			// Init debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			// Language tool
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource", typeof(MittalWorkingUnitsReports).Assembly);
			setLanguage();
			logInUser = NotificationController.GetLogInUser();
			populateWorkigUnitCombo();
			
			DateTime date = DateTime.Now.Date;
			this.CenterToScreen();

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
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitName = new System.Windows.Forms.Label();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblDocFormat = new System.Windows.Forms.Label();
            this.checkbCSV = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbTXT = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbWorkingUnit.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnitName);
            this.gbWorkingUnit.Location = new System.Drawing.Point(13, 8);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(353, 91);
            this.gbWorkingUnit.TabIndex = 0;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(84, 56);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(84, 26);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(248, 21);
            this.cbWorkingUnit.TabIndex = 2;
            // 
            // lblWorkingUnitName
            // 
            this.lblWorkingUnitName.Location = new System.Drawing.Point(24, 25);
            this.lblWorkingUnitName.Name = "lblWorkingUnitName";
            this.lblWorkingUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnitName.TabIndex = 1;
            this.lblWorkingUnitName.Text = "Name:";
            this.lblWorkingUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(13, 120);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(496, 64);
            this.gbTimeInterval.TabIndex = 4;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Tag = "FILTERABLE";
            this.gbTimeInterval.Text = "Date Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(331, 24);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(299, 23);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(84, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(32, 23);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(93, 200);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(104, 23);
            this.lblDocFormat.TabIndex = 9;
            this.lblDocFormat.Tag = "FILTERABLE";
            this.lblDocFormat.Text = "Document Format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkbCSV
            // 
            this.checkbCSV.Enabled = false;
            this.checkbCSV.Location = new System.Drawing.Point(210, 200);
            this.checkbCSV.Name = "checkbCSV";
            this.checkbCSV.Size = new System.Drawing.Size(48, 24);
            this.checkbCSV.TabIndex = 11;
            this.checkbCSV.Tag = "FILTERABLE";
            this.checkbCSV.Text = "CSV";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(94, 248);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 23);
            this.btnGenerate.TabIndex = 14;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(294, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbTXT
            // 
            this.cbTXT.Location = new System.Drawing.Point(271, 200);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(48, 24);
            this.cbTXT.TabIndex = 12;
            this.cbTXT.Tag = "FILTERABLE";
            this.cbTXT.Text = "TXT";
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Location = new System.Drawing.Point(332, 200);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(56, 24);
            this.cbCR.TabIndex = 13;
            this.cbCR.Tag = "FILTERABLE";
            this.cbCR.Text = "CR";
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(372, 8);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 91);
            this.gbFilter.TabIndex = 35;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // MittalWorkingUnitsReports
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(524, 288);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.checkbCSV);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbWorkingUnit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MittalWorkingUnitsReports";
            this.ShowInTaskbar = false;
            this.Text = "MittalWorkingUnitsReports";
            this.Load += new System.EventHandler(this.MittalWorkingUnitsReports_Load);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void btnGenerateReport_Click(object sender, System.EventArgs e)
		{
			try
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.btnGenerateReport_Click() \n");
				this.Cursor = Cursors.WaitCursor;
				Hashtable passTypeSummary = new Hashtable();

				
				if (cbWorkingUnit.SelectedIndex == 0)
				{
					MessageBox.Show(rm.GetString("noWUSelected", culture));
					return;
				}					
				
				if (this.checkbCSV.Checked)
				{
					debug.writeLog(DateTime.Now + " WorkingUnitsSummaryReports csv: STARTED!\n");
					this.generateSummaryCSVReport(passTypeSummary);
					debug.writeLog(DateTime.Now + " WorkingUnitsSummaryReports csv: FNISHED!\n");
				}
				if (this.cbTXT.Checked)
				{
					this.generateSummaryTXTReport(passTypeSummary);
				}
				if (this.cbCR.Checked)
				{
					this.generateSummaryCRReport();
				}
				//this.Close();
				
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.btnGenerateReport_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void populateWorkigUnitCombo()
		{
			try
			{
				List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

				if (logInUser != null)
				{
					wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
				}

				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.populateWorkigUnitCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/*
		private void pdfAnalyticalSetup(PDFDocument doc)
		{
			try
			{
				string wu = "";
				if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
				{
					wu = rm.GetString("repAll", culture);
				}
				else
				{
					wu = cbWorkingUnit.Text.Trim();
				}

				// Set header
				doc.Title = "Izveštaj o ukupnom borvaku radnika po tipovima boravka";
				doc.LeftBoxText = "Radna jedinica: " + wu + "\n\nOd: " + dtpFromDate.Value.ToString("dd.MM.yyyy") + "\nDo: " +  dtpToDate.Value.ToString("dd.MM.yyyy") + "\n\n\n";
				doc.RightBoxText = "Datum: " + DateTime.Now.ToString("dd.MM.yyyy") + "\n\n\n"; 
				doc.FilePath = Constants.pdfDocPath + "\\Izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".pdf";
				doc.Font = doc.AddFont(Constants.pdfFont);
				doc.FontSize = Constants.pdfFontSize;
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.pdfSetup(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
		*/

		/*
		private void pdfSummarySetup(PDFDocument doc)
		{
			try
			{
				string wu = "";
				if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
				{
					wu = rm.GetString("repAll", culture);
				}
				else
				{
					wu = cbWorkingUnit.Text.Trim();
				}

				// Set header
				doc.Title = "Izveštaj o tipu prolazaka \n za radnu jedinicu  za  vremenski period";
				doc.LeftBoxText = "Radna jedinica: " + wu + "\n\nOd: " + dtpFromDate.Value.ToString("dd.MM.yyyy") + "\nDo: " +  dtpToDate.Value.ToString("dd.MM.yyyy") + "\n\n\n";
				doc.RightBoxText = "Datum: " + DateTime.Now.ToString("dd.MM.yyyy") + "\n\n\n"; 
				doc.FilePath = Constants.pdfDocPath + "\\Sumarni izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".pdf";
				doc.Font = doc.AddFont(Constants.pdfFont);
				doc.FontSize = Constants.pdfFontSize;
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.pdfSetup(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
		*/

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("workingUnitReports", culture);

				gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
				lblWorkingUnitName.Text = rm.GetString("lblName", culture);
				chbHierarhicly.Text = rm.GetString("hierarchically", culture);
				lblFrom.Text = rm.GetString("lblFrom", culture);
				gbTimeInterval.Text = rm.GetString("timeInterval", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
				lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
				btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}


		
		/// <summary>
		/// PDF report header
		/// </summary>
		/// <param name="titleFontSize"></param>
		/// <param name="doc"></param>
		/*
		public void InsertTitle(int titleFontSize, PDFDocument doc)
		{
			try
			{
				doc.TextStyle.Bold = true;
				doc.HeaderHeight = Constants.HeaderHeight;
				doc.Rect.String = "main";
				double totalWidth = doc.StandardWidth - (doc.LeftMargine + doc.RightMargine);
				doc.Rect.Height = doc.HeaderHeight;
				
				doc.Color.String = "0 0 0";

				doc.PageNumber = 1;
				doc.Rect.Position(doc.LeftMargine, doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
				doc.Rect.Width = totalWidth / 4;
				doc.FontSize = Constants.pdfFontSize;
				doc.HPos = 0.0;
				doc.VPos = 1.0;
				doc.AddText(doc.LeftBoxText);
								
				doc.Rect.Position(doc.LeftMargine + totalWidth / 4, doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
				doc.Rect.Width = 2 * (totalWidth / 4);
				doc.FontSize = titleFontSize;
				doc.HPos = 0.5;
				doc.VPos = 0.0;
				doc.AddText(doc.Title);
				
				doc.Rect.Position(doc.LeftMargine + 3 * (totalWidth / 4), doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
				doc.FontSize = Constants.pdfFontSize;
				doc.Rect.Width = totalWidth / 4;
				doc.HPos = 1.0;
				doc.VPos = 1.0;
				doc.AddText(doc.RightBoxText);
				doc.TextStyle.Bold = false;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " WorkigUnitsReports.InsertTitle(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

		/*
		private void generateAnalyticalPDFReport(ArrayList ioPairList)
		{
			try
			{
				PDFDocument doc = new PDFDocument();
				this.pdfAnalyticalSetup(doc);
				this.InsertTitle(14, doc);

				string[] colNames = {"rbr", "Datum", "Prezime", "Ime", "Lokacija", "Ulazak", "Izlazak", "Tip", "Trajanje"};
				int[] colWidths = {3, 3, 4, 3, 3, 3, 3, 4, 3};
				double[] colPositions = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
				ArrayList tableData = new ArrayList();

				int index = 0;

				TimeSpan timeSpan = new TimeSpan();

				string wuName = "";

				foreach(IOPairTO pairTO in ioPairList)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"), pairTO.EmployeeLastName, 
										   pairTO.EmployeeName, pairTO.LocationName, 
										   pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
										   pairTO.PassType.Trim(), timeSpan.Hours.ToString() + "h " + timeSpan.Minutes + "min"};

					if ( wuName != pairTO.WUName )
					{
						string[] rowEmpty =  {"", "", "", "", "", "", "", "", ""};
						tableData.Add(rowEmpty);
						
						string[] rowTitle =  {"", pairTO.WUName, "", "", "", "", "", "", ""};
						tableData.Add(rowTitle);
						wuName = pairTO.WUName;
					}

					tableData.Add(rowData);
				}

				doc.InsertTable(colNames, colWidths, colPositions, tableData);
				
				doc.InsertFooter(doc.FontSize);
				doc.Save();

				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports OPEN Document: Started! \n");
				doc.Open();
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports OPEN Document: Finished! \n");
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.generateAnalyticalPDFReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

		private void generateAnalyticalCSVReport(ArrayList ioPairList)
		{
			try
			{
				ArrayList tableData = new ArrayList();
				int index = 0;
				TimeSpan timeSpan = new TimeSpan();

				foreach(IOPairTO pairTO in ioPairList)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"), pairTO.EmployeeLastName, 
										   pairTO.EmployeeName, pairTO.LocationName, 
										   pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
										   pairTO.PassType.Trim(), timeSpan.Hours.ToString() + "h " + timeSpan.Minutes + "min"};

					tableData.Add(rowData);
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("datum",typeof(System.String));
				table.Columns.Add("prezime", typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("lokacija", typeof(System.String));
				table.Columns.Add("ulazak", typeof(System.String));
				table.Columns.Add("izlazak", typeof(System.String));
				table.Columns.Add("tip", typeof(System.String));
				table.Columns.Add("trajanje", typeof(System.String));
				
				for(int i=0; i< tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5,6,7,8};
				string[] cHeaders = {"rbr", "Datum", "Prezime", "Ime", "Lokacija", "Ulazak", "Izlazak", "Tip", "Trajanje"};

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
				{
					//MessageBox.Show("There are no data to export to CSV!");
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					Export objExport = new Export("Win", cHeaders);
					string wu = "";
					if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
					{
						wu = rm.GetString("repAll", culture);
					}
					else
					{
						wu = cbWorkingUnit.Text.Trim();
					}
					string fileName = Constants.csvDocPath + "\\Izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".csv";
					objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.generateAnalyticalCSVReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateAnalyticalTXTReport(ArrayList ioPairList)
		{
			try
			{
				ArrayList tableData = new ArrayList();
				int index = 0;
				TimeSpan timeSpan = new TimeSpan();

				foreach(IOPairTO pairTO in ioPairList)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"), pairTO.EmployeeLastName, 
										   pairTO.EmployeeName, pairTO.LocationName, 
										   pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
										   pairTO.PassType.Trim(), timeSpan.Hours.ToString() + "h " + timeSpan.Minutes + "min"};

					tableData.Add(rowData);
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("datum",typeof(System.String));
				table.Columns.Add("prezime", typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("lokacija", typeof(System.String));
				table.Columns.Add("ulazak", typeof(System.String));
				table.Columns.Add("izlazak", typeof(System.String));
				table.Columns.Add("tip", typeof(System.String));
				table.Columns.Add("trajanje", typeof(System.String));
				
				for(int i=0; i< tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();
				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5,6,7,8};
				string[] cHeaders = {"rbr", "Datum", "Prezime", "Ime", "Lokacija", "Ulazak", "Izlazak", "Tip", "Trajanje"};

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
				{
					//MessageBox.Show("There are no data to export to CSV!");
					MessageBox.Show(rm.GetString("noTXTExport", culture));
				}
				else
				{
					//19.10.2006, Bilja, da prosledim i imena kolona, bilo bez toga
					Export objExport = new Export("Win", cHeaders);
								 
					//RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
					string wu = "";
					if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
					{
						wu = rm.GetString("repAll", culture);
					}
					else
					{
						wu = cbWorkingUnit.Text.Trim();
					}
					string fileName = Constants.txtDocPath + "\\Izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";
					objExport.ExportDetails(table, Export.ExportFormat.Excel, fileName);
					System.Diagnostics.Process.Start(fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.generateAnalyticalTXTReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}


		private DataSet SummaryReportDataForEmployee(EmployeeTO currentEmployee)
		{
			DataSet dataSetTotalCR = null;
			DataTable totalTable = new DataTable("EmployeeTotalsCR");

			try
			{
				this.Cursor = Cursors.WaitCursor;
				IOPair ioPair = new IOPair();

				// list of pairs for report
                List<IOPairTO> ioPairList = new List<IOPairTO>();

				// list of Time Schemas for selected Employee and selected Time Interval
                List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

				// list of Time Schedules for one month
                List<EmployeeTimeScheduleTO> timeSchedule = new List<EmployeeTimeScheduleTO>();
					
				// get all valid IO Pairs for selected employee and time interval
				List<int> selectedEmployees = new List<int>();
				selectedEmployees.Add(currentEmployee.EmployeeID);

				ioPairList = ioPair.SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, selectedEmployees, -1);

				// get all dates in selected interval which has open pairs
				List<DateTime> datesList = new IOPair().SearchDatesWithOpenPairs(dtpFromDate.Value, dtpToDate.Value,
					currentEmployee.EmployeeID);
					
				// get Time Schemas for selected Employee and selected Time Interval
				DateTime date = dtpFromDate.Value.Date;

				//int totalPrivateOut = 0;
				//int totalOfficialOut = 0;

				while ((date <= dtpToDate.Value) || (date.Month == dtpToDate.Value.Month))
				{
					timeSchedule = new EmployeesTimeSchedule().SearchMonthSchedule(currentEmployee.EmployeeID, date);

					foreach (EmployeeTimeScheduleTO ets in timeSchedule)
					{
						timeScheduleList.Add(ets);
					}

					date = date.AddMonths(1);
				}

				// Kay is Date, Value is Time schema for that Date
				Dictionary<DateTime, WorkTimeSchemaTO> schemaForDay = new Dictionary<DateTime,WorkTimeSchemaTO>();
				
				// Set Time Schema for every selected day
				date = dtpFromDate.Value.Date;
				while (date <= dtpToDate.Value.Date)
				{
					int timeScheduleIndex = -1;

					for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
					{
                        /* 2008-03-14
                         * From now one, take the last existing time schedule, don't expect that every month has 
                         * time schedule*/
						if (date >= timeScheduleList[scheduleIndex].Date)
							//&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
						{
							timeScheduleIndex = scheduleIndex;
						}
					}

					if (timeScheduleIndex >= 0)
					{
                        TimeSchema sch = new TimeSchema();
                        sch.TimeSchemaTO.TimeSchemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
						List<WorkTimeSchemaTO> schemas = sch.Search();
						if (schemas.Count > 0)
						{
							schemaForDay.Add(date, schemas[0]);
						}
					}
					
					date = date.AddDays(1);
				}

				// Key is Pass Type Id, Value is Pass Type Description
				Dictionary<int, string> passTypes = new Dictionary<int,string>();

				List<PassTypeTO> passTypesAll = new PassType().Search();
				foreach (PassTypeTO pt in passTypesAll)
				{
					passTypes.Add(pt.PassTypeID, pt.Description);
				}

				// Key is PassTypeID, Value is total time
				Hashtable passTypesTotalTime = new Hashtable();
			
				// Key is Date, Value is hours spent on job for that date
				Hashtable job = new Hashtable();
				
				// Key is Date, Value is late for that date
				Hashtable late = new Hashtable();

				// Key is Date, Value is earlier going away for that date
				Hashtable early = new Hashtable();

				// Key is Date, Value is overtime work for that date
				Hashtable overtime = new Hashtable();

				// Table Definition for Crystal Reports
				DataSet dataSetCR = new DataSet();
				DataTable tableCR = new DataTable("EmployeeAnalyticalCR");

				tableCR.Columns.Add("date", typeof(System.DateTime));
				tableCR.Columns.Add("time_schema", typeof(System.String));
				tableCR.Columns.Add("last_name", typeof(System.String));
				tableCR.Columns.Add("first_name", typeof(System.String));
				tableCR.Columns.Add("late", typeof(int));
				tableCR.Columns.Add("early", typeof(int));
				tableCR.Columns.Add("total_time", typeof(int));
				tableCR.Columns.Add("over_time", typeof(int));			
				tableCR.Columns.Add("need_validation", typeof(System.String));
				tableCR.Columns.Add("working_unit", typeof(System.String));

				tableCR.Columns.Add("private_out", typeof(int));
				tableCR.Columns.Add("official_out", typeof(int));
				tableCR.Columns.Add("extra_hours", typeof(int));
				tableCR.Columns.Add("employee_id", typeof(int));

				TimeSpan totalTime = new TimeSpan(0);

				// Job
				foreach (IOPairTO iopairTO in ioPairList)
				{
					totalTime = iopairTO.EndTime.Subtract(iopairTO.StartTime); 

					if (job.ContainsKey(iopairTO.IOPairDate.Date))
					{
						job[iopairTO.IOPairDate.Date] = ((TimeSpan) job[iopairTO.IOPairDate.Date]).Add(totalTime);	
					}
					else
					{
						job.Add(iopairTO.IOPairDate.Date, totalTime);
					}

					if (passTypesTotalTime.ContainsKey(iopairTO.PassTypeID))
					{
						passTypesTotalTime[iopairTO.PassTypeID] = ((TimeSpan) passTypesTotalTime[iopairTO.PassTypeID]).Add(totalTime);	
					}
					else
					{
						passTypesTotalTime.Add(iopairTO.PassTypeID, totalTime);
					}
				}
				
				// Late
				date = dtpFromDate.Value.Date;
				Dictionary<DateTime, IOPairTO> earlyestArrivedPair = new Dictionary<DateTime,IOPairTO>();

				while (date <= dtpToDate.Value.Date)
				{
					// Find IOPair with earliest arrival for particular date
					foreach(IOPairTO pair in ioPairList)
					{
						if (date.Equals(pair.IOPairDate))
						{
							if (earlyestArrivedPair.ContainsKey(date))
							{
								if (earlyestArrivedPair[date].StartTime.TimeOfDay > pair.StartTime.TimeOfDay)
								{
									earlyestArrivedPair[date] = pair;
								}
							}
							else
							{
								earlyestArrivedPair.Add(date, pair);
							}
						}
					}
					
					// Calculate Late
					if (earlyestArrivedPair.ContainsKey(date))
					{
						if (!late.ContainsKey(date) && schemaForDay.ContainsKey(date))
						{
							WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(selectedEmployees[0], date, timeScheduleList);
							if (((TimeSpan) currentInterval.EndTime.Subtract(currentInterval.StartTime)).TotalMinutes != 0)
							{
								IOPairTO earlyestReal = earlyestArrivedPair[date];

								if (earlyestReal.StartTime.TimeOfDay > currentInterval.LatestArrivaed.TimeOfDay)
								{
									late.Add(date, earlyestReal.StartTime.TimeOfDay - currentInterval.LatestArrivaed.TimeOfDay);
								}
							}
						}
					}
				
					date = date.AddDays(1);
				}

				// Earlier going away for that date
				date = dtpFromDate.Value.Date;
				Dictionary<DateTime, IOPairTO> latestLeftPairs = new Dictionary<DateTime,IOPairTO>();

				while (date <= dtpToDate.Value)
				{
					foreach(IOPairTO pair in ioPairList)
					{
						if (date.Equals(pair.IOPairDate))
						{
							if (latestLeftPairs.ContainsKey(date))
							{
								if (latestLeftPairs[date].EndTime.TimeOfDay < pair.EndTime.TimeOfDay)
								{
									latestLeftPairs[date] = pair;
								}
							}
							else
							{
								latestLeftPairs.Add(date, pair);
							}
						}
					}

					// Calculate earlier going away from job 
					if (latestLeftPairs.ContainsKey(date))
					{
						if (!early.ContainsKey(date) && schemaForDay.ContainsKey(date))
						{
                            WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(selectedEmployees[0], date, timeScheduleList);
							IOPairTO latestReal = latestLeftPairs[date];

							if (latestReal.EndTime.TimeOfDay < currentInterval.EarliestLeft.TimeOfDay)
							{
								early.Add(date, currentInterval.EarliestLeft.TimeOfDay - latestReal.EndTime.TimeOfDay);
							}
						}
					}

					date = date.AddDays(1);
				}

				// Overtime work for that date
				date = dtpFromDate.Value.Date;
				TimeSpan expectedDuaration = new TimeSpan(0);
				TimeSpan realTime = new TimeSpan(0);
				TimeSpan dif = new TimeSpan(0);
				
				while (date <= dtpToDate.Value)
				{					
					if (job.ContainsKey(date))
					{
                        WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(selectedEmployees[0], date, timeScheduleList);
						expectedDuaration = currentInterval.EndTime.TimeOfDay - currentInterval.StartTime.TimeOfDay;
						realTime = (TimeSpan) job[date];
						dif = realTime - expectedDuaration;

						if (dif.TotalMinutes > 0)
						{
							if (overtime.ContainsKey(date))
							{
								overtime[date] = dif;
							}
							else
							{
								overtime.Add(date, dif); 
							}
						}
					}

					date = date.AddDays(1);
				}

				// Summary for Private out & Official out
				TimeSpan tsTemp = new TimeSpan(0);

				double totalPrivateOut = 0;
				double totalOfficialOut = 0;

				TimeSpan tpo = new TimeSpan();
				TimeSpan too = new TimeSpan();

                List<IOPairTO> ioPairListOut = ioPair.SearchForEmployees(dtpFromDate.Value, dtpToDate.Value, selectedEmployees, -1);
                				
				foreach(IOPairTO pair in ioPairListOut)
				{
					if ((pair.PassTypeID.Equals(Constants.privateOut))
						|| (pair.PassTypeID.Equals(Constants.privateOutExtra)))	
					{
						tsTemp = pair.EndTime.TimeOfDay - pair.StartTime.TimeOfDay;
						tpo = tpo.Add(tsTemp);
					}
					if (pair.PassTypeID.Equals(Constants.officialOut))
					{
						tsTemp = pair.EndTime.TimeOfDay - pair.StartTime.TimeOfDay;
						too = too.Add(tsTemp);
					}
				}			

				totalPrivateOut = tpo.TotalMinutes;
				totalOfficialOut = too.TotalMinutes;

				// ******** REPORT ********
				// all records for report
				date = dtpFromDate.Value.Date;
				ArrayList rowList = new ArrayList();
				WorkTimeSchemaTO tsc = new WorkTimeSchemaTO();

				// TOTALS
				TimeSpan totalLate = new TimeSpan(0);
				TimeSpan totalEarly = new TimeSpan(0);
				TimeSpan totalWorkTime = new TimeSpan(0);
				TimeSpan totalOverTime = new TimeSpan(0);

				while (date <= dtpToDate.Value.Date)
				{
					if (schemaForDay.ContainsKey(date) || late.ContainsKey(date) || early.ContainsKey(date) ||
						job.ContainsKey(date) || overtime.ContainsKey(date) || datesList.Contains(date))
					{
						DataRow rowCR = tableCR.NewRow();
						// One record in table

						ArrayList row = new ArrayList();

						rowCR["last_name"] = currentEmployee.LastName;
						rowCR["first_name"] = currentEmployee.FirstName;
						rowCR["working_unit"] = currentEmployee.WorkingUnitName;
						

						// Date
						row.Add(date.ToString("dd.MM.yyy"));
						rowCR["date"] = date;


						// Work Time Shema Description
						if (schemaForDay.ContainsKey(date))
						{
							tsc = schemaForDay[date];
						}
						else
						{
							tsc = new WorkTimeSchemaTO();
							tsc.Description = rm.GetString("noTimeSchema", culture);
						}
						
						row.Add(tsc.Description.ToString());
						rowCR["time_schema"] = tsc.Description.ToString();

						// Late
						if (late.ContainsKey(date))
						{
							totalTime = ((TimeSpan) late[date]);
							row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
							totalLate = totalLate.Add(totalTime);
							rowCR["late"] = totalTime.TotalMinutes;
						}
						else
						{
							row.Add("0h 0min");
							rowCR["late"] = 0;
						}
						
						// Early
						if (early.ContainsKey(date))
						{
							totalTime = ((TimeSpan) early[date]);
							row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
							totalEarly = totalEarly.Add(totalTime);
							rowCR["early"] = totalTime.TotalMinutes;
						}
						else
						{
							row.Add("0h 0min");
							rowCR["early"] = 0;
						}

						// Total working hours for a day
						if (job.ContainsKey(date))
						{
							totalTime = ((TimeSpan) job[date]);
							row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
							totalWorkTime = totalWorkTime.Add(totalTime);
							rowCR["total_time"] = totalTime.TotalMinutes;
						}
						else
						{
							row.Add("0h 0min");
							rowCR["total_time"] = 0;
						}
					
						// Overtime work for that date
						if (overtime.ContainsKey(date))
						{
							totalTime = ((TimeSpan) overtime[date]);
							row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
							totalOverTime = totalOverTime.Add(totalTime);
							rowCR["over_time"] = totalTime.TotalMinutes;
							rowCR["extra_hours"] = totalTime.TotalMinutes;
						}
						else
						{
							row.Add("0h 0min");
							rowCR["over_time"] = 0;
						}

						// Note
						if (datesList.Contains(date))
						{
							row.Add("X");
							rowCR["need_validation"] = "X";
						}
						else
						{
							row.Add("");
							rowCR["need_validation"] = "";
						}

						rowCR["private_out"] = totalPrivateOut;
						rowCR["official_out"] = totalPrivateOut;
						
						rowList.Add(row);
						tableCR.Rows.Add(rowCR);
					}
					
					date = date.AddDays(1);
				}

				dataSetCR = new DataSet();
				dataSetCR.Tables.Add(tableCR);

				// Claculate Totals
				ArrayList totalsRowList = new ArrayList();

				// One record in Total's table
				ArrayList rowTotal = new ArrayList();

				dataSetTotalCR = new DataSet();
				//DataTable totalTable = new DataTable("EmployeeTotalsCR");
				totalTable = tableCR.Clone();

				DataRow totalTableRow = totalTable.NewRow();

				totalTableRow["first_name"] = currentEmployee.FirstName;
				totalTableRow["last_name"] = currentEmployee.LastName;
				totalTableRow["working_unit"] = currentEmployee.WorkingUnitName;
				totalTableRow["employee_id"] = currentEmployee.EmployeeID;

				totalTableRow["late"] = totalLate.TotalMinutes;
				totalTableRow["early"] = totalEarly.TotalMinutes;
				totalTableRow["over_time"] = totalOverTime.TotalMinutes;

				if (passTypesTotalTime.ContainsKey(Constants.regularWork))
				{
					totalTableRow["total_time"] = ((TimeSpan)passTypesTotalTime[0]).TotalMinutes;
				}
				else
				{
					totalTableRow["total_time"] = 0;
				}
				
				totalTableRow["private_out"] = 0;

				if (passTypesTotalTime.ContainsKey(Constants.privateOut))
				{
					totalTableRow["private_out"] = ((TimeSpan) passTypesTotalTime[Constants.privateOut]).TotalMinutes;
				}
				if (passTypesTotalTime.ContainsKey(Constants.privateOutExtra))
				{
					totalTableRow["private_out"] = (Convert.ToDouble(totalTableRow["private_out"])) + 
						((TimeSpan) passTypesTotalTime[Constants.privateOutExtra]).TotalMinutes;
				}

				totalTableRow["official_out"] = 0;
				if (passTypesTotalTime.ContainsKey(Constants.officialOut))
				{
					totalTableRow["official_out"] = ((TimeSpan) passTypesTotalTime[Constants.officialOut]).TotalMinutes;
				}

				//totalTableRow["official_out"] = totalOfficialOut;
				totalTableRow["extra_hours"] = totalOverTime.TotalMinutes;
				totalTable.Rows.Add(totalTableRow);
				
				rowTotal.Add((totalLate.Hours + (totalLate.Days * 24)).ToString() + "h " + totalLate.Minutes + "min");
				rowTotal.Add((totalEarly.Hours + (totalEarly.Days * 24)).ToString() + "h " + totalEarly.Minutes + "min");
				rowTotal.Add((totalWorkTime.Hours + (totalWorkTime.Days * 24)).ToString() + "h " + totalWorkTime.Minutes + "min");
				rowTotal.Add((totalOverTime.Hours + (totalOverTime.Days * 24)).ToString() + "h " + totalOverTime.Minutes + "min");
				rowTotal.Add("");

				totalsRowList.Add(rowTotal);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.SummaryReportData(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			
			dataSetTotalCR.Tables.Add(totalTable);
			return dataSetTotalCR;
		}


		private void generateSummaryCSVReport(Hashtable employeeTotals)
		{
			List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();
			try
			{
				// Get Employees for selected Working Unit			
				Employee empl = new Employee();
				selectedEmployees = empl.SearchByWU(cbWorkingUnit.SelectedValue.ToString());

				// Key is Pass Type Id, Value is Pass Type Description
				List<PassTypeTO> passTypesAll = new PassType().Search();
				int index = 0;
				Hashtable currentHT = new Hashtable();

				ArrayList tableData = new ArrayList();

				foreach(EmployeeTO currentEmployee in selectedEmployees)
				{
					if (employeeTotals.ContainsKey(currentEmployee.EmployeeID))
					{
						currentHT = (Hashtable) employeeTotals[currentEmployee.EmployeeID];
						foreach(PassTypeTO currentPassType in passTypesAll)
						{
							if (currentHT.ContainsKey(currentPassType.PassTypeID))
							{
								index++;
								TimeSpan totalSpan = (TimeSpan) currentHT[currentPassType.PassTypeID];
								string[] rowData = {index.ToString(), currentEmployee.LastName,  
													   currentEmployee.FirstName, currentPassType.Description,
													   (totalSpan.Days * 24 + totalSpan.Hours).ToString() + "h " + totalSpan.Minutes + "min"};
								tableData.Add(rowData);
							}
						}
					}
				}

				DataTable table = new DataTable();
				table.Columns.Add("rbr", typeof(System.String));
				table.Columns.Add("prezime", typeof(System.String));
				table.Columns.Add("ime", typeof(System.String));
				table.Columns.Add("tip", typeof(System.String));
				table.Columns.Add("trajanje", typeof(System.String));
				
				for(int i=0; i< tableData.Count; i++) 
				{	
					string[] al = (string[])tableData[i];
					DataRow dr = table.NewRow();
					for(int j=0; j<al.Length; j++)
					{
						dr[j] = al[j]; 
					}
					table.Rows.Add(dr);
				}
				table.AcceptChanges();

				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4};
				string[] cHeaders = {"rbr","Prezime", "Ime", "Tip", "Trajanje"};

				// Export the details of specified columns to Excel
				if ( table.Rows.Count == 0 )
				{
					//MessageBox.Show("There are no data to export to CSV!");
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					Export objExport = new Export("Win", cHeaders);
					string wu = "";
					if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
					{
						wu = rm.GetString("repAll", culture);
					}
					else
					{
						wu = cbWorkingUnit.Text.Trim();
					}
					string fileName = Constants.csvDocPath + "\\Sumarni izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".csv";
					objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.generateSummaryCSVReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateSummaryTXTReport(Hashtable employeeTotals)
		{
			ArrayList selectedEmployees = new ArrayList();
			try
			{
				// Get Employees for selected Working Unit			
				Employee temp = new Employee();
				List<EmployeeTO> employees = temp.SearchByWU(cbWorkingUnit.SelectedValue.ToString());
				DataSet dsCurrentEmpl = null;
				DataTable tblCurrentEmpl = null;
				
				// Table Definition for Crystal Reports
				DataSet finalDS = new DataSet();
				DataTable finalTable = new DataTable("EmployeeAnalyticalCR");

				finalTable.Columns.Add("date", typeof(System.DateTime));
				finalTable.Columns.Add("time_schema", typeof(System.String));
				finalTable.Columns.Add("last_name", typeof(System.String));
				finalTable.Columns.Add("first_name", typeof(System.String));
				finalTable.Columns.Add("late", typeof(int));
				finalTable.Columns.Add("early", typeof(int));
				finalTable.Columns.Add("total_time", typeof(int));
				finalTable.Columns.Add("over_time", typeof(int));			
				finalTable.Columns.Add("need_validation", typeof(System.String));
				finalTable.Columns.Add("working_unit", typeof(System.String));

				finalTable.Columns.Add("private_out", typeof(int));
				finalTable.Columns.Add("official_out", typeof(int));
				finalTable.Columns.Add("extra_hours", typeof(int));
				finalTable.Columns.Add("employee_id", typeof(int));

				DataTable textTable = new DataTable("textTable");

				textTable.Columns.Add("num", typeof(System.String));
				textTable.Columns.Add("employee_id", typeof(System.String));
				textTable.Columns.Add("last_name", typeof(System.String));
				textTable.Columns.Add("first_name", typeof(System.String));
				textTable.Columns.Add("total_work", typeof(System.String));
				textTable.Columns.Add("official_out", typeof(System.String));
				textTable.Columns.Add("extra_hours", typeof(System.String));
				textTable.Columns.Add("private_out", typeof(System.String));
				textTable.Columns.Add("late", typeof(System.String));

				int num = 0;

				foreach(EmployeeTO employee in employees)
				{
					dsCurrentEmpl = this.SummaryReportDataForEmployee(employee);
					tblCurrentEmpl = (DataTable) dsCurrentEmpl.Tables[0];

					DataRow row = tblCurrentEmpl.Rows[0];
					DataRow textRow = textTable.NewRow();

					textRow[0] = Convert.ToString(++num);
					textRow[1] = Convert.ToString(row["employee_id"]);
					textRow[2] = Convert.ToString(row["last_name"]);
					textRow[3] = Convert.ToString(row["first_name"]);
					textRow[4] = ((int) row["total_time"] / 60) 
						+ " h " + ((int) row["total_time"] % 60) + " min";
					textRow[5] = ((int) row["official_out"] / 60) 
						+ " h " + ((int) row["official_out"] % 60) + " min";
					textRow[6] = ((int) row["extra_hours"] / 60) 
						+ " h " + ((int) row["extra_hours"] % 60) + " min";
					textRow[7] = ((int) row["private_out"] / 60) 
						+ " h " + ((int) row["private_out"] % 60) + " min";
					textRow[8] = ((int) row["late"] / 60) 
						+ " h " + ((int) row["late"] % 60) + " min";

					textTable.Rows.Add(textRow);
				}
				
				// Specify the column list to export
				int[] iColumns = {0,1,2,3,4,5,6,7,8};
				string[] cHeaders = {"rbr","Maticni broj", "Prezime", 
									"Ime", "Redovan rad", "Sluzbeni izlasci", 
									"Preraspodela", "Privatni izlasci", "Kašnjenje"};

				// Export the details of specified columns to Excel
				if ( textTable.Rows.Count == 0 )
				{
					//MessageBox.Show("There are no data to export to CSV!");
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					Export objExport = new Export("Win", cHeaders);
					
					string wu = "";
					if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
					{
						wu = rm.GetString("repAll", culture);
					}
					else
					{
						wu = cbWorkingUnit.Text.Trim();
					}
					string fileName = Constants.txtDocPath + "\\Sumarni izvestaj po radnoj jedinici " + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";
					objExport.ExportDetails(textTable, Export.ExportFormat.Excel, fileName);
					System.Diagnostics.Process.Start(fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.generateSummaryTXTReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateAnalyticalCRReport(List<IOPairTO> dataList)
		{
			try
			{
				DataSet dataSet = new DataSet();
				DataTable table = new DataTable("Test");

				table.Columns.Add("date", typeof(System.String));
				table.Columns.Add("last_name", typeof(System.String));
				table.Columns.Add("first_name", typeof(System.String));
				table.Columns.Add("location", typeof(System.String));
				table.Columns.Add("start", typeof(System.String));
				table.Columns.Add("end", typeof(System.String));
				table.Columns.Add("type", typeof(System.String));
				table.Columns.Add("total_time", typeof(System.String));			
				table.Columns.Add("working_unit", typeof(System.String));			

				TimeSpan timeSpan = new TimeSpan();

				foreach(IOPairTO pairTO in dataList)
				{
					DataRow row = table.NewRow();
					
					row["date"] = pairTO.IOPairDate.ToString("dd.MM.yyyy");
					row["first_name"] = pairTO.EmployeeName;
					row["last_name"] = pairTO.EmployeeLastName;
					row["location"] = pairTO.LocationName;
					row["start"] = pairTO.StartTime.ToString("HH:mm:ss");
					row["end"] = pairTO.EndTime.ToString("HH:mm:ss");
					row["type"] = pairTO.PassType;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					row["total_time"] = timeSpan.Hours.ToString() + "h " + timeSpan.Minutes.ToString() + "min ";
					row["working_unit"] = pairTO.WUName;
		
					table.Rows.Add(row);
				}
				table.AcceptChanges();
				dataSet.Tables.Add(table);

				if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
				{					
					Reports.Mittal.Mittal_sr.MittalWUSummaryCRView_sr view = new Reports.Mittal.Mittal_sr.MittalWUSummaryCRView_sr(
						dataSet, dtpFromDate.Value, dtpToDate.Value);
					view.ShowDialog(this);					
				}
				else if(NotificationController.GetLanguage().Equals(Constants.Lang_en))
				{					
					Reports.Mittal.Mittal_en.MittalWUSummaryCRView_en view = new Reports.Mittal.Mittal_en.MittalWUSummaryCRView_en(
						dataSet, dtpFromDate.Value, dtpToDate.Value);
					view.ShowDialog(this);					
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.generateAnalyticalCRReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}					
		}
		
		private void generateSummaryCRReport()
		{
			List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();

			try
			{
				Employee temp = new Employee();
				List<EmployeeTO> employees = temp.SearchByWU(cbWorkingUnit.SelectedValue.ToString());
				DataSet dsCurrentEmpl = null;
				DataTable tblCurrentEmpl = null;
				
				// Table Definition for Crystal Reports
				DataSet finalDS = new DataSet();
                DataTable finalTable = new DataTable("employee_analytical");
				
				finalTable.Columns.Add("date", typeof(System.DateTime));
				finalTable.Columns.Add("time_schema", typeof(System.String));
				finalTable.Columns.Add("last_name", typeof(System.String));
				finalTable.Columns.Add("first_name", typeof(System.String));
				finalTable.Columns.Add("late", typeof(int));
				finalTable.Columns.Add("early", typeof(int));
				finalTable.Columns.Add("total_time", typeof(int));
				finalTable.Columns.Add("over_time", typeof(int));			
				finalTable.Columns.Add("need_validation", typeof(System.String));
				finalTable.Columns.Add("working_unit", typeof(System.String));

				finalTable.Columns.Add("private_out", typeof(int));
				finalTable.Columns.Add("official_out", typeof(int));
				finalTable.Columns.Add("extra_hours", typeof(int));
				finalTable.Columns.Add("employee_id", typeof(int));
                finalTable.Columns.Add("imageID", typeof(byte));

                DataTable tableI = new DataTable("images");
                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));

                int i = 0;
				foreach(EmployeeTO employee in employees)
				{
					dsCurrentEmpl = this.SummaryReportDataForEmployee(employee);
					tblCurrentEmpl = (DataTable) dsCurrentEmpl.Tables[0];

					DataRow row = tblCurrentEmpl.Rows[0];
					DataRow finalRow = finalTable.NewRow();
					finalRow[0] = row[0];
					finalRow[1] = row[1];
					finalRow[2] = row[2];
					finalRow[3] = row[3];
					finalRow[4] = row[4];
					finalRow[5] = row[5];
					finalRow[6] = row[6];
					finalRow[7] = row[7];
					finalRow[8] = row[8];
					finalRow[9] = row[9];
					finalRow[10] = row[10];
					finalRow[11] = row[11];
					finalRow[12] = row[12];
					finalRow[13] = row[13];

                    finalRow["imageID"] = 1;
                    if (i == 0)
                    {
                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();
                    }

					finalTable.Rows.Add(finalRow);
                    i++;
				}

				finalTable.AcceptChanges();
				finalDS.Tables.Add(finalTable);
                finalDS.Tables.Add(tableI);

				if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
				{
					Reports.Mittal.Mittal_sr.MittalWUSummaryCRView_sr view = 
						new Reports.Mittal.Mittal_sr.MittalWUSummaryCRView_sr(finalDS, dtpFromDate.Value, dtpToDate.Value);

					view.ShowDialog(this);
				}
				else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
				{
					Reports.Mittal.Mittal_en.MittalWUSummaryCRView_en view = 
						new Reports.Mittal.Mittal_en.MittalWUSummaryCRView_en(
						finalDS, dtpFromDate.Value, dtpToDate.Value);

					view.ShowDialog(this);
				}
				
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.generateSummaryPDFReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public WorkTimeIntervalTO getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList)
		{
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();

			int timeScheduleIndex = -1;

			for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
			{
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
				if ((date >= timeScheduleList[scheduleIndex].Date))
					//&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
				{
					timeScheduleIndex = scheduleIndex;
				}
			}

			if (timeScheduleIndex >= 0)
			{
				int cycleDuration = 0;
				int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
				int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
                TimeSchema sch = new TimeSchema();
                sch.TimeSchemaTO.TimeSchemaID = schemaID;
				List<WorkTimeSchemaTO> timeSchema = sch.Search();
				WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
				if(timeSchema.Count > 0)
				{
					schema = timeSchema[0];
					cycleDuration = schema.CycleDuration;
				}

                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
				//TimeSpan days = date - ((EmployeesTimeSchedule) (timeScheduleList[timeScheduleIndex])).Date;
				//interval = (TimeSchemaInterval) ((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);
                interval = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration][0];
			}

			return interval;
		}

        private void MittalWorkingUnitsReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " MittalWorkingUnitsReports.MittalWorkingUnitsReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}

