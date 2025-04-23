using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Collections;

using TransferObjects;
using Common;
using Util;

namespace Reports.Mittal
{
    public partial class MittalCountPassesOnReader : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        DebugLog log;
        Filter filter;
        List<LocationTO> locArray;

        private PassTO currentPass = null;
        List<GateTO> gateArray;

        public MittalCountPassesOnReader()
        {
            InitializeComponent();
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeesReports).Assembly);

            setLanguage();

            currentPass = new PassTO();
        }

        private void setLanguage()
        {
            try
            {
                gbSearchCriteria.Text = rm.GetString("gbSearchCriteria", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                lblDirection.Text = rm.GetString("lblDirection", culture);
                lblLocation.Text = rm.GetString("lblLocation", culture);
                this.Text = rm.GetString("MittalCountPassesOnReader", culture);

                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Populate Location Combo Box
        /// </summary>
        private void populateLocationCombo()
        {
            try
            {
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                locArray = loc.Search();
                locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), 0, 0, ""));

                cbLocation.DataSource = locArray;
                cbLocation.DisplayMember = "Name";
                cbLocation.ValueMember = "LocationID";
                cbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.populateLocationCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Populate Gate Combo Box
        /// </summary>
        private void populateGateArray(int locID)
        {
            try
            {
                
                //if (locID < 0)
                //{
                //    gateArray = new Gate().Search("", "", "", "", "", "");
                //}
                //else
                //{
                    gateArray = new Gate().SerchForLocationEnabled(locID);
                //}                
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.populateGateArray(): " + ex.Message + "\n");
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
                //cbDirection.Items.Add(Constants.DirectionInOut);

                cbDirection.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.populateDirectionCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void MittalCountPassesOnReader_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            
                populateDirectionCombo();
                populateLocationCombo();
                populateGateArray(-1);
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.MittalCountPassesOnReader_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string direction = "";
                if (cbDirection.SelectedIndex > 0)
                {
                    direction = cbDirection.SelectedItem.ToString();
                }
                Dictionary<int, List<int>> gatesNumber = new Dictionary<int, List<int>>();
                Dictionary<int, GateTO> gatesDict = new Dictionary<int, GateTO>();
                foreach (GateTO gate in gateArray)
                {
                    if (!gatesNumber.ContainsKey(gate.GateID))
                    {
                        gatesDict.Add(gate.GateID, gate);
                        List<int> list = new List<int>();
                        Pass pass = new Pass();
                        pass.PssTO.Direction = direction;
                        pass.PssTO.LocationID = (int)cbLocation.SelectedValue;
                        pass.PssTO.GateID = gate.GateID;
                        int count1 = pass.SearchIntervalCount(DateTime.Now.Date.AddMonths(-1), DateTime.Now.Date, "", new DateTime(1, 1, 1, 0, 0, 0), new DateTime(1, 1, 1, 23, 59, 0));
                        list.Add(count1);
                        int count3 = pass.SearchIntervalCount(DateTime.Now.Date.AddMonths(-3), DateTime.Now.Date, "", new DateTime(1, 1, 1, 0, 0, 0), new DateTime(1, 1, 1, 23, 59, 0));
                        list.Add(count3);
                        int count6 = pass.SearchIntervalCount(DateTime.Now.Date.AddMonths(-6), DateTime.Now.Date, "", new DateTime(1,1,1,0,0,0), new DateTime(1,1,1,23,59,0));
                        list.Add(count6);
                        gatesNumber.Add(gate.GateID, list);
                    }
                }
                DataSet dataSet = new DataSet();
                DataTable table = new DataTable("gate");
                table.Columns.Add("gate_name", typeof(System.String));
                table.Columns.Add("month_1", typeof(System.Int32));
                table.Columns.Add("month_3", typeof(System.Int32));
                table.Columns.Add("month_6", typeof(System.Int32));               
                table.Columns.Add("imageID", typeof(byte));

                DataTable tableI = new DataTable("images");
                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));

                try
                {
                    foreach (int gateID in gatesNumber.Keys)
                    {
                        if (gateID != 0)
                        {
                            DataRow row = table.NewRow();

                            row["gate_name"] = gatesDict[gateID].Name;
                            row["month_1"] = gatesNumber[gateID][0];
                            row["month_3"] = gatesNumber[gateID][1];
                            row["month_6"] = gatesNumber[gateID][2];

                            row["imageID"] = 1;
                            
                            table.Rows.Add(row);
                        }                       
                    }
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();
                    table.AcceptChanges();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " MittalCountPassesOnReader.generateCSVReport(): " + ex.Message + "\n");
                    throw new Exception(ex.Message);
                }

                dataSet.Tables.Add(table);
                dataSet.Tables.Add(tableI);

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports.Mittal.Mittal_sr.MittalCountPassesonReaderCRView_sr view = new Reports.Mittal.Mittal_sr.MittalCountPassesonReaderCRView_sr(
                        dataSet, cbLocation.Text.ToString(),cbDirection.Text.ToString());
                    view.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.btnGenerate_Click(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbLocation.SelectedIndex == 0)
                {
                    populateGateArray(-1);
                }
                else
                {
                    LocationTO loc = (LocationTO)cbLocation.SelectedItem;
                    populateGateArray(loc.LocationID);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalCountPassesOnReader.cbLocation_SelectedIndexChanged(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void cbFilter_SelectedIndexChanged_1(object sender, EventArgs e)
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
    }
}