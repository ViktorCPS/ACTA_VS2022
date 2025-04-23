using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

using Common;
using TransferObjects;
using Util;

namespace Reports
{
    public partial class TrespassReports : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        DebugLog log;

        List<LogTO> currentLogsList;
        int startIndex;

        //cbEvent indexes
        const int unknownCard = 1;
        const int cardDisapproved = 2;

        private int sortOrder;
        private int sortField;

        // List View indexes
		const int LocationIndex = 0;
		const int GateIndex = 1;
		const int ReaderIndex = 2;
		const int DirectionIndex = 3;
		const int EventIndex = 4;
		const int EmployeeIndex = 5;
        const int TagIndex = 6;
        const int EventTimeIndex = 7;

        Filter filter;

        public TrespassReports()
        {
            InitializeComponent();
            this.CenterToScreen();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeesReports).Assembly);
           
        }
        private void populateLocationCb()
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.populateLocationCb(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void setLanguage()
        {
            this.Text = rm.GetString("TrespassReport", culture);
            this.gbSearch.Text = rm.GetString("gbSearch", culture);
            this.gbFilter.Text = rm.GetString("gbFilter", culture);

            //label's text
            this.lblEmployee.Text = rm.GetString("lblEmployee", culture);
            this.lblEvent.Text = rm.GetString("lblEvent", culture);
            this.lblDirection.Text = rm.GetString("lblFrom", culture);
            this.lblGate.Text = rm.GetString("lblGate", culture);
            this.lblLocation.Text = rm.GetString("lblLocation", culture);
            this.lblReader.Text = rm.GetString("lblReader", culture);
            this.lblTo.Text = rm.GetString("lblTo", culture);
            this.lblDirection.Text = rm.GetString("lblDirection", culture);
            this.lblFrom.Text = rm.GetString("lblFrom", culture);

            //button's text
            this.btnClose.Text = rm.GetString("btnClose", culture);
            this.btnReport.Text = rm.GetString("btnReport", culture);
            this.btnSearch.Text = rm.GetString("btnSearch", culture);

            // List View Header
            this.lvTrespasses.BeginUpdate();

            lvTrespasses.Columns.Add(rm.GetString("hdrLocation", culture), (lvTrespasses.Width - 6) / 8+10, HorizontalAlignment.Left);
            lvTrespasses.Columns.Add(rm.GetString("hdrGate", culture), (lvTrespasses.Width - 6) / 8+10, HorizontalAlignment.Left);
            lvTrespasses.Columns.Add(rm.GetString("hdrReader", culture), (lvTrespasses.Width - 6) / 8+20, HorizontalAlignment.Left);
            lvTrespasses.Columns.Add(rm.GetString("hdrDirection", culture), (lvTrespasses.Width - 6) / 8-40, HorizontalAlignment.Left);
            lvTrespasses.Columns.Add(rm.GetString("hdrEvent", culture), (lvTrespasses.Width - 6) / 8, HorizontalAlignment.Left);
            lvTrespasses.Columns.Add(rm.GetString("hdrEmployee", culture), (lvTrespasses.Width - 6) / 8, HorizontalAlignment.Left);
            lvTrespasses.Columns.Add(rm.GetString("hdrTag", culture), (lvTrespasses.Width - 6) / 8-20, HorizontalAlignment.Left);
            lvTrespasses.Columns.Add(rm.GetString("hdrEventTime", culture), (lvTrespasses.Width - 6) / 8+10, HorizontalAlignment.Left);

            lvTrespasses.EndUpdate();
        }

        private void populateEmployeeCombo()
        {
            try
            {                            
                List<EmployeeTO> emplArray = new Employee().Search();
              
                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbEmployee.DataSource = emplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateGateCombo(int locID)
        {
            try
            {
                List<GateTO> gatesArray = new List<GateTO>();
                if (locID < 0)
                {
                    gatesArray = new Gate().Search();
                }
                else
                {
                    gatesArray = new Gate().SerchForLocation(locID);
                }
              
                GateTO gate = new GateTO();
                gate.Name= rm.GetString("all", culture);
                gatesArray.Insert(0, gate);
               
               this.cbGate.DataSource = gatesArray;
               this.cbGate.DisplayMember = "Name";
               this.cbGate.ValueMember = "GateID";
               this.cbGate.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.populateGateCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void populateReaderCombo(int locID, int gateID)
        {
            try
            {
                List<ReaderTO> readerArray = new List<ReaderTO>();
                if ((locID < 0)&&(gateID <0))
                {
                    readerArray = new Reader().SearchAll();
                }
                else
                {
                    readerArray = new Reader().getReaders(locID, gateID);
                }
                
                ReaderTO reader = new ReaderTO();
                reader.Description = rm.GetString("all", culture);
                readerArray.Insert(0, reader);

                this.cbReader.DataSource = readerArray;
                this.cbReader.DisplayMember = "Description";
                this.cbReader.ValueMember = "ReaderID";
                this.cbReader.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.populateGateCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Populate Direction Combo Box
        /// </summary>
        private void populateDirectionCombo()
        {
            try
            {
                cbDirection.Items.Add(rm.GetString("all", culture));
                cbDirection.Items.Add(Constants.DirectionIn);
                cbDirection.Items.Add(Constants.DirectionOut);
                
                cbDirection.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.populateDirectionCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateEventCombo()
        {
            try
            {
                this.cbEvent.Items.Add(rm.GetString("all", culture));
                this.cbEvent.Items.Add(rm.GetString("unknownCard", culture));
                this.cbEvent.Items.Add(rm.GetString("cardDisapproved", culture));

                this.cbEvent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.populateEventCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbLocation.SelectedIndex == 0)
                {
                    populateGateCombo(-1);
                    populateReaderCombo(-1, -1);
                }
                else
                {
                    populateGateCombo((int)cbLocation.SelectedValue);
                    populateReaderCombo((int)cbLocation.SelectedValue, -1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.cbLocation_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbGate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbGate.SelectedIndex == 0)
                {
                    if (cbLocation.SelectedIndex == 0)
                    {
                        populateReaderCombo(-1, -1);
                    }
                    else
                    {                        
                        populateReaderCombo((int)cbLocation.SelectedValue, -1);
                    }
                }
                else
                {                    
                    populateReaderCombo(-1, (int)cbGate.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.cbLocation_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                startIndex = 0;
                int locationID = -1;
                int gateID = -1;
                int readerID = -1;
                int employeeID = -1;
                int eventID = -1;
                string direction = "";

                if (cbLocation.SelectedIndex != 0)
                {
                    locationID = (int)cbLocation.SelectedValue;
                }
                if (cbGate.SelectedIndex != 0)
                {
                    gateID = (int)cbGate.SelectedValue;
                }
                if (cbReader.SelectedIndex != 0)
                {                    
                    readerID = (int)cbReader.SelectedValue;
                }
                if (cbEmployee.SelectedIndex != 0)
                {                    
                    employeeID = (int)cbEmployee.SelectedValue;
                }
                if (cbEvent.SelectedIndex != 0)
                {
                    if (cbEvent.SelectedIndex == unknownCard)
                    {
                        eventID = (int)Constants.EventTag.eventTagUnrecognized;
                    }
                    else if (cbEvent.SelectedIndex == cardDisapproved)
                    {
                        eventID = (int)Constants.EventTag.eventTagDenied;
                    }
                }
                if (cbDirection.SelectedIndex != 0)
                {
                    direction = cbDirection.SelectedItem.ToString();
                }

                currentLogsList = new Log().getTrespassLogs(locationID, gateID, readerID, direction, employeeID, eventID, dtpFromDate.Value.Date, dtpToDate.Value.Date);
                if (currentLogsList.Count > 0)
                {                  
                    sortField = TrespassReports.LocationIndex;
                    currentLogsList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentLogsList, startIndex);
                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentLogsList.Count.ToString().Trim();
                }
                else //else if (count == 0)
                {
                     clearListView();
                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void TrespassReports_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " TrespassReports.TrespassReports_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void clearListView()
        {
            lvTrespasses.BeginUpdate();
            lvTrespasses.Items.Clear();
            lvTrespasses.EndUpdate();

            lvTrespasses.Invalidate();

            btnPrev.Visible = false;
            btnNext.Visible = false;
        }
            private void populateListView(List<LogTO> logsList, int startIndex)
		{
			try
			{
				if (logsList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}

				lvTrespasses.BeginUpdate();
				lvTrespasses.Items.Clear();

				if (logsList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < logsList.Count))
					{
						if (startIndex == 0)
						{
							btnPrev.Enabled = false;
						}
						else
						{
							btnPrev.Enabled = true;
						}

						int lastIndex = startIndex + Constants.recordsPerPage;
						if (lastIndex >= logsList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = logsList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

						for (int i = startIndex; i < lastIndex; i++)
						{
							LogTO log = logsList[i];
							ListViewItem item = new ListViewItem();

							item.Text = log.Location;
							item.SubItems.Add(log.Gate);
							item.SubItems.Add(log.ReaderDescription);
							item.SubItems.Add(log.Direction);
                            if (log.EventHappened == 1)
                            {
                                item.SubItems.Add(rm.GetString("unknownCard", culture));
                            }
                            else
                            {
                                item.SubItems.Add(rm.GetString("cardDisapproved", culture));
                            }
                            if (log.EmployeeName.Trim().Length > 0)
                            {
                                item.SubItems.Add(log.EmployeeName);
                            }
                            else
                                item.SubItems.Add("N/A");
                            item.SubItems.Add(log.TagID.ToString());
                            if (!log.EventTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                item.SubItems.Add(log.EventTime.ToString("dd.MM.yyyy   HH:mm"));
                            }
                            item.ToolTipText = "Log_id: " + log.LogID.ToString();                            
                            
							item.Tag = log;
							lvTrespasses.Items.Add(item);
						}
					}
				}

				lvTrespasses.EndUpdate();
				lvTrespasses.Invalidate();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " TrespassReports.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}		
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                populateListView(currentLogsList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populateListView(currentLogsList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void TrespassReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex = 0;
                populateDirectionCombo();
                populateEmployeeCombo();
                populateLocationCb();
                populateGateCombo(-1);
                populateReaderCombo(-1, -1);
                populateEventCombo();
                currentLogsList = new List<LogTO>();
                setLanguage();
                this.btnPrev.Visible = false;
                this.btnNext.Visible = false;
                this.lblTotal.Visible = false;

                filter = new Filter();
                filter.SerachButton = btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.TrespassReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbEmployee.SelectedIndex != 0)
                {
                    cbEvent.SelectedIndex = cardDisapproved;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.cbEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbEvent.SelectedIndex == unknownCard)
                {
                    cbEmployee.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.cbEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvTrespasses_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrder;

                if (e.Column == sortField)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrder = Constants.sortAsc;
                }

                sortField = e.Column;
                currentLogsList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentLogsList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.lvTreaspasses_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }        	

		#region Inner Class for sorting Array List of Employees

		/*
		 *  Class used for sorting Array List of Employees
		*/

		private class ArrayListSort:IComparer<LogTO>
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}

            public int Compare(LogTO x, LogTO y)        
			{
                LogTO log1 = null;
                LogTO log2 = null;

				if (compOrder == Constants.sortAsc)
				{
					log1 = x;
					log2 = y;
				}
				else
				{
					log1 = y;
					log2 = x;
				}

				switch(compField)            
				{                
					case TrespassReports.LocationIndex: 
						return log1.Location.CompareTo(log2.Location); 
					case TrespassReports.GateIndex:
						return log1.Gate.CompareTo(log2.Gate);                
					case TrespassReports.ReaderIndex:
						return log1.ReaderDescription.CompareTo(log2.ReaderDescription);                
					case TrespassReports.DirectionIndex:
						return log1.Direction.CompareTo(log2.Direction);                
					case TrespassReports.EventIndex:
						return log1.EventHappened.CompareTo(log2.EventHappened);                
					case TrespassReports.EmployeeIndex:
						return log1.EmployeeName.CompareTo(log2.EmployeeName);                
					case TrespassReports.TagIndex:                   
						return log1.TagID.CompareTo(log2.TagID); 
                    case TrespassReports.EventTimeIndex:
                        return log1.EventTime.CompareTo(log2.EventTime);
                    default:
                        return log1.Location.CompareTo(log2.Location);
				}        
			}    
		}

		#endregion

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (currentLogsList.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("log");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("location", typeof(System.String));
                    tableCR.Columns.Add("gate", typeof(System.String));
                    tableCR.Columns.Add("reader", typeof(System.String));
                    tableCR.Columns.Add("direction", typeof(System.String));
                    tableCR.Columns.Add("event", typeof(System.String));
                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("tag", typeof(System.String));
                    tableCR.Columns.Add("event_time", typeof(System.String));
                    tableCR.Columns.Add("imageID", typeof(byte));
                    
                    tableI.Columns.Add("image", typeof(System.Byte[]));
                    tableI.Columns.Add("imageID", typeof(byte));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);

                    foreach (LogTO log in currentLogsList)
                    {
                        DataRow row = tableCR.NewRow();

                        row["location"] = log.Location;
                        row["gate"] = log.Gate;
                        row["reader"] = log.ReaderDescription;
                        row["direction"] = log.Direction;
                        if (log.EventHappened == Constants.unknownCard)
                        {
                            row["event"] = rm.GetString("unknownCard", culture);
                        }
                        if (log.EventHappened == Constants.cardDisapproved)
                        {
                            row["event"] = rm.GetString("cardDisapproved", culture);
                        }
                        row["employee"] = log.EmployeeName;
                        row["tag"] = log.TagID;
                        if (log.EventTime == new DateTime(0))
                        {
                            row["event_time"] = "";
                        }
                        else
                        {
                            row["event_time"] = log.EventTime.ToString("dd.MM.yyyy HH:mm");
                        }
                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selLocation = "*";
                    string selGate = "*";
                    string selReader = "*";
                    string selDirection = "*";
                    string selEmployee = "*";
                    string selEvent = "*";

                    if (cbLocation.SelectedIndex >= 0 && (int)cbLocation.SelectedValue >= 0)
                        selLocation = cbLocation.Text;
                    if (cbGate.SelectedIndex >= 0 && (int)cbGate.SelectedValue >= 0)
                        selGate = cbGate.Text;
                    if (cbReader.SelectedIndex >= 0)
                        selReader = cbReader.Text;
                    if (cbDirection.SelectedIndex >= 0 )
                        selDirection = cbDirection.Text;
                    if (cbEmployee.SelectedIndex >= 0)
                        selEmployee = cbEmployee.Text;
                    if (cbEvent.SelectedIndex >= 0 )
                        selEvent = cbEvent.Text;
                    
                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.TrespassLogCRView view = new Reports.Reports_sr.TrespassLogCRView(dataSetCR, selLocation,
                             selGate, selReader, selDirection, selEmployee, selEvent, this.dtpFromDate.Value, this.dtpToDate.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.TrespassLogCRView_en view = new Reports.Reports_en.TrespassLogCRView_en(dataSetCR, selLocation,
                             selGate, selReader, selDirection, selEmployee, selEvent, this.dtpFromDate.Value, this.dtpToDate.Value);
                        view.ShowDialog(this);
                    }/*
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.IOPairsCRView_fi view = new Reports.Reports_fi.IOPairsCRView_fi(dataSetCR,
                             selGate, selEmplyee, selLocation, selLocation, dtFrom.Value, dtTo.Value);
                        view.ShowDialog(this);
                    }*/
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TrespassReports.btnReport_Click(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;
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
            finally
            {
                this.Cursor = Cursors.Arrow;
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
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}