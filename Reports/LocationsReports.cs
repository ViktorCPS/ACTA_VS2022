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

using Common;
using TransferObjects;
using Util;
using DataAccess;

namespace Reports
{
	/// <summary>
	/// Summary description for LocationsReports.
	/// </summary>
	public class LocationsReports : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox cbLocation;
		private System.Windows.Forms.CheckBox chbHierarhicly;
		private System.Windows.Forms.CheckBox chbPdf;
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

		private System.Windows.Forms.GroupBox gbLocation;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblDocFormat;

		// Working Units that logInUser is granted to
		List<WorkingUnitTO> wUnits;
		private System.Windows.Forms.CheckBox cbTXT;
		private System.Windows.Forms.CheckBox cbCR;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
		List<LocationTO> locations;

        Filter filter;

		public LocationsReports()
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

			populateParentLocationCb();

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
            this.gbLocation = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblDocFormat = new System.Windows.Forms.Label();
            this.chbPdf = new System.Windows.Forms.CheckBox();
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
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbLocation.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLocation
            // 
            this.gbLocation.Controls.Add(this.chbHierarhicly);
            this.gbLocation.Controls.Add(this.cbLocation);
            this.gbLocation.Controls.Add(this.lblLocation);
            this.gbLocation.Location = new System.Drawing.Point(8, 16);
            this.gbLocation.Name = "gbLocation";
            this.gbLocation.Size = new System.Drawing.Size(218, 91);
            this.gbLocation.TabIndex = 0;
            this.gbLocation.TabStop = false;
            this.gbLocation.Text = "Location";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(88, 51);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(96, 24);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchically";
            // 
            // cbLocation
            // 
            this.cbLocation.Location = new System.Drawing.Point(88, 24);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(121, 21);
            this.cbLocation.TabIndex = 2;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(16, 24);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(100, 23);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "Location:";
            // 
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(8, 245);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(134, 23);
            this.lblDocFormat.TabIndex = 9;
            this.lblDocFormat.Text = "Document format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbPdf
            // 
            this.chbPdf.Enabled = false;
            this.chbPdf.Location = new System.Drawing.Point(148, 245);
            this.chbPdf.Name = "chbPdf";
            this.chbPdf.Size = new System.Drawing.Size(48, 24);
            this.chbPdf.TabIndex = 10;
            this.chbPdf.Text = "PDF";
            this.chbPdf.Visible = false;
            // 
            // chbCSV
            // 
            this.chbCSV.Location = new System.Drawing.Point(202, 245);
            this.chbCSV.Name = "chbCSV";
            this.chbCSV.Size = new System.Drawing.Size(48, 24);
            this.chbCSV.TabIndex = 11;
            this.chbCSV.Text = "CSV";
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Location = new System.Drawing.Point(8, 133);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(361, 100);
            this.gbTimeInterval.TabIndex = 4;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(88, 64);
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
            this.dtpFromDate.Location = new System.Drawing.Point(88, 32);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(200, 20);
            this.dtpFromDate.TabIndex = 6;
            this.dtpFromDate.Value = new System.DateTime(2006, 8, 3, 0, 0, 0, 0);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(40, 293);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 23);
            this.btnGenerate.TabIndex = 14;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(232, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbTXT
            // 
            this.cbTXT.Location = new System.Drawing.Point(256, 245);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(48, 24);
            this.cbTXT.TabIndex = 12;
            this.cbTXT.Text = "TXT";
            this.cbTXT.CheckedChanged += new System.EventHandler(this.cbTXT_CheckedChanged);
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Location = new System.Drawing.Point(310, 245);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(48, 24);
            this.cbCR.TabIndex = 13;
            this.cbCR.Text = "CR";
            this.cbCR.CheckedChanged += new System.EventHandler(this.cbCR_CheckedChanged);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(232, 16);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 91);
            this.gbFilter.TabIndex = 32;
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
            // LocationsReports
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(376, 328);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.chbPdf);
            this.Controls.Add(this.chbCSV);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbLocation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocationsReports";
            this.ShowInTaskbar = false;
            this.Text = "LocationsReports";
            this.Load += new System.EventHandler(this.LocationsReports_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LocationsReports_KeyUp);
            this.gbLocation.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void populateParentLocationCb()
		{
			try
			{
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				List<LocationTO> locations = loc.Search();
				locations.Insert(0, new LocationTO(-1, rm.GetString("all", culture), "", -1, -1, Constants.DefaultStateActive.ToString()));
			
				cbLocation.DataSource = locations;
				cbLocation.DisplayMember = "Name";
				cbLocation.ValueMember = "LocationID";
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " LocationsReports.populateParentLocationCb(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void btnGenerate_Click(object sender, System.EventArgs e)
		{
			try
			{
				debug.writeLog(DateTime.Now + " LocationsReports.btnGenerate_Click()\n");
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

					int selectedLocation = (int) cbLocation.SelectedValue;
					Location loc = new Location();
                    loc.LocTO.LocationID = selectedLocation;					
					locations = loc.Search();

					if (this.chbHierarhicly.Checked)
					{
						if ( selectedLocation == -1 )
						{
							for ( int i = locations.Count-1; i >= 0; i-- )
							{
								if ( locations[i].LocationID != locations[i].ParentLocationID)
								{
									locations.RemoveAt(i);
								}
							}
						}
						locations = loc.FindAllChildren(locations);
						selectedLocation = -1;
					}

					string locString = "";
					foreach (LocationTO loc1 in locations)
					{
						locString += loc1.LocationID.ToString().Trim() + ","; 
					}
				
					if (locString.Length > 0)
					{
						locString = locString.Substring(0, locString.Length - 1);
					}

					IOPair ioPair = new IOPair();
					int count = ioPair.SearchLocationCount(selectedLocation, locString, wuString, dtpFromDate.Value, dtpToDate.Value);
					if (count > Constants.maxLocReportRecords)
					{
						this.Cursor = Cursors.Arrow;
						MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
						return;
					}
					if (count == 0)
					{
						this.Cursor = Cursors.Arrow;
						MessageBox.Show(rm.GetString("dataNotFound", culture));
						return;
					}
					else if (count > Constants.warningLocReportRecords)
					{
						this.Cursor = Cursors.Arrow;
						DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
						if (result.Equals(DialogResult.No))
						{
							return;
						}
					}

					this.Cursor = Cursors.WaitCursor;
					List<IOPairTO> locRep = ioPair.SearchLocation(selectedLocation, locString, wuString, dtpFromDate.Value, dtpToDate.Value);
					/*
					if (this.chbPdf.Checked)
					{
						debug.writeLog(DateTime.Now + " LocationsReports pdf: STARTED!\n");
						this.generatePDFReport(locRep);
						debug.writeLog(DateTime.Now + " LocationsReports pdf: FNISHED!\n");
					}
					*/

					if (this.chbCSV.Checked)
					{
						debug.writeLog(DateTime.Now + " LocationsReports csv: STARTED!\n");
						this.generateCSVReport(locRep);
						debug.writeLog(DateTime.Now + " LocationsReports csv: FNISHED!\n");
					}

					if (this.cbTXT.Checked)
					{
						this.generateTXTReport(locRep);
					}

					string selLocation = "";
					if (this.cbCR.Checked)
					{
						if (cbLocation.SelectedIndex.Equals(0))
						{
							selLocation = rm.GetString("allLocations", culture);
						}
						else
						{
							selLocation = cbLocation.Text;
						}
						
						if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
						{
							Reports_sr.LocationsReportCRView locView = new Reports_sr.LocationsReportCRView(this.PopulateCRData(locRep),dtpFromDate.Value, dtpToDate.Value);
							debug.writeLog("Before locView.ShowDialog(this)");
							locView.ShowDialog(this);
							debug.writeLog("After locView.ShowDialog(this)");
						}
						else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
						{
							Reports_en.LocationsReportCRView_en locView = new Reports_en.LocationsReportCRView_en(this.PopulateCRData(locRep),dtpFromDate.Value, dtpToDate.Value);
							debug.writeLog("Before locView.ShowDialog(this)");
							locView.ShowDialog(this);
							debug.writeLog("After locView.ShowDialog(this)");
						}
					}
				
					//this.Close();
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " LocationsReports.btnGenerate_Click(): " + ex.Message + "\n");
				//MessageBox.Show(ex.Message);
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

		/*
		private void pdfSetup(PDFDocument doc)
		{
			try
			{
				string location = "";
				if (cbLocation.Text.Trim().Equals(rm.GetString("all", culture)))
				{
					location = rm.GetString("repAll", culture);
				}
				else
				{
					location = cbLocation.Text.Trim();
				}
				doc.Title = "Izveštaj o prisustvu zaposlenih po lokacijama za vremenski period";
				doc.LeftBoxText = "Lokacija: " + location + "\n\nOd: " + dtpFromDate.Value.ToString("dd.MM.yyyy") + "\nDo: " +  dtpToDate.Value.ToString("dd.MM.yyyy") + "\n\n\n";
				doc.RightBoxText = "Datum: " + DateTime.Now.ToString("dd.MM.yyyy") + "\n\n\n"; 
				doc.FilePath = Constants.pdfDocPath + "\\Izvestaj po lokaciji " + location + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".pdf";
				doc.Font = doc.AddFont(Constants.pdfFont);
				doc.FontSize = Constants.pdfFontSize;
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " LocationsReports.pdfSetup(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

		/*
		private void generatePDFReport(ArrayList locRep)
		{
			try
			{
				PDFDocument doc = new PDFDocument();
				this.pdfSetup(doc);
				this.InsertTitle(14, doc);
			
				string[] colNames = {"rbr", "Datum", "Prezime", "Ime", "Lokacija", "Ulazak", "Izlazak", "Tip", "Trajanje"};
				int[] colWidths = {3, 3, 4, 3, 3, 3, 3, 4, 3};
				double[] colPositions = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
			
				ArrayList tableData = new ArrayList();
				int index = 0;
				string locName = "";
				TimeSpan timeSpan = new TimeSpan();

				foreach(IOPairTO pairTO in locRep)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"),
										   pairTO.EmployeeLastName, pairTO.EmployeeName,  
										   pairTO.LocationName, pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
										   pairTO.PassType.Trim(), timeSpan.Hours.ToString() + "h " + timeSpan.Minutes + "min"};

					if ( locName != pairTO.LocationName )
					{
						string[] rowEmpty =  {"", "", "", "", "", "", "", "", ""};
						tableData.Add(rowEmpty);
						
						string[] rowTitle =  {"", pairTO.LocationName, "", "", "", "", "", "", ""};
						tableData.Add(rowTitle);
						locName = pairTO.LocationName;
					}
					tableData.Add(rowData);
				}

				doc.InsertTable(colNames, colWidths, colPositions, tableData);
				
				doc.InsertFooter(doc.FontSize);
				doc.Save();
				doc.Open();
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " LocationsReports.generatePDFReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/

		private void generateCSVReport(List<IOPairTO> locRep)
		{
			try
			{
				ArrayList tableData = new ArrayList();
				int index = 0;

				TimeSpan timeSpan = new TimeSpan();

				foreach(IOPairTO pairTO in locRep)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"),
										   pairTO.EmployeeLastName, pairTO.EmployeeName,  
										   pairTO.LocationName, pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
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
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					Export objExport = new Export("Win", cHeaders);
					string location = "";
					if (cbLocation.Text.Trim().Equals(rm.GetString("all", culture)))
					{
						location = rm.GetString("repAll", culture);
					}
					else
					{
						location = cbLocation.Text.Trim();
					}
					string fileName = Constants.csvDocPath + "\\Izvestaj po lokaciji " + location + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".csv";
				
			    	objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName);
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " LocationsReports.generateCSVReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void generateTXTReport(List<IOPairTO> locRep)
		{
			try
			{
				ArrayList tableData = new ArrayList();
				int index = 0;

				TimeSpan timeSpan = new TimeSpan();

				foreach(IOPairTO pairTO in locRep)
				{
					index++;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					string[] rowData = {index.ToString(), pairTO.IOPairDate.ToString("dd.MM.yyyy"),
										   pairTO.EmployeeLastName, pairTO.EmployeeName,  
										   pairTO.LocationName, pairTO.StartTime.ToString("HH:mm:ss"), pairTO.EndTime.ToString("HH:mm:ss"), 
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
					MessageBox.Show(rm.GetString("noCSVExport", culture));
				}
				else
				{
					Export objExport = new Export("Win", cHeaders);
					string location = "";
					if (cbLocation.Text.Trim().Equals(rm.GetString("all", culture)))
					{
						location = rm.GetString("repAll", culture);
					}
					else
					{
						location = cbLocation.Text.Trim();
					}
					string fileName = Constants.txtDocPath + "\\Izvestaj po lokaciji " + location + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";
				
					objExport.ExportDetails(table, Export.ExportFormat.Excel, fileName);

					System.Diagnostics.Process.Start(fileName);
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
				this.Text = rm.GetString("locationReports", culture);

				gbLocation.Text = rm.GetString("location", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
				lblLocation.Text = rm.GetString("lblLocation", culture);
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
				debug.writeLog(DateTime.Now + " LocationsReports.setLanguage(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}			
		}

		/// <summary>
		/// Format and insert header for PDF document
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
				debug.writeLog(DateTime.Now + " LocationsReports.InsertTitle(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		*/


		/// <summary>
		/// Populate LocReportDS DataSet to create LocationCR
		/// CrystalReports report
		/// </summary>
		/// <param name="dataList">Data list</param>
		/// <returns>Instance of LocReportDS</returns>
		private DataSet PopulateCRData(List<IOPairTO> dataList)
		{
			DataSet dataSet = new DataSet();
            DataTable table = new DataTable("loc_rep");
			table.Columns.Add("date", typeof(System.String));
			table.Columns.Add("last_name", typeof(System.String));
			table.Columns.Add("first_name", typeof(System.String));
			table.Columns.Add("location", typeof(System.String));
			table.Columns.Add("start", typeof(System.String));
			table.Columns.Add("end", typeof(System.String));
			table.Columns.Add("type", typeof(System.String));
			table.Columns.Add("total_time", typeof(System.String));
			table.Columns.Add("employee_id", typeof(int));
            table.Columns.Add("imageID", typeof(byte));

            DataTable tableI = new DataTable("images");
            tableI.Columns.Add("image", typeof(System.Byte[]));
            tableI.Columns.Add("imageID", typeof(byte));

			try
			{
				TimeSpan timeSpan = new TimeSpan();
			
				string minutes;
                int i = 0;
				foreach(IOPairTO pairTO in dataList)
				{
					DataRow row = table.NewRow();
					
					row["date"] = pairTO.IOPairDate.ToString("dd.MM.yyyy");
					row["first_name"] = pairTO.EmployeeName;
					row["last_name"] = pairTO.EmployeeLastName;
					row["location"] = pairTO.LocationName;
					row["start"] = pairTO.StartTime.ToString("HH:mm");
					row["end"] = pairTO.EndTime.ToString("HH:mm");
					row["type"] = pairTO.PassType;
					timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);
					
					if (timeSpan.Minutes < 10) 
					{
						minutes = "0" + timeSpan.Minutes.ToString();
					}
					else
					{
						minutes = timeSpan.Minutes.ToString();
						
					}
					row["total_time"] = timeSpan.Hours.ToString() + "h " + minutes + "min ";
					row["employee_id"] = pairTO.EmployeeID;

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

        private void cbTXT_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbTXT.Checked)
            {
                this.cbCR.Checked = false;
            }
            else
            {
                this.cbCR.Checked = true;
            }
        }

        private void cbCR_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCR.Checked)
            {
                this.cbTXT.Checked = false;
            }
            else
            {
                this.cbTXT.Checked = true;
            }
        }

        private void LocationsReports_KeyUp(object sender, KeyEventArgs e)
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
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

        private void LocationsReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                filter = new Filter();
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
