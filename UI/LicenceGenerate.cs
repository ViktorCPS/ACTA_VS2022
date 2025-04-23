using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.IO;
using System.Threading;

using Common;
using Util;

namespace UI
{
    public partial class LicenceGenerate : Form
    {
        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        public struct Customer
        {
            public string _name;
            public int _menuItemPos;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public int MenuItemPos
            {
                get { return _menuItemPos; }
                set { _menuItemPos = value; }
            }
        }

        public LicenceGenerate(string langCode)
        {
            try
            {
                InitializeComponent();

                this.CenterToScreen();

                // Init Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(langCode);
                rm = new ResourceManager("UI.Resource", typeof(LicenceGenerate).Assembly);
                setLanguage();

                this.lblServerValue.Text = "";
                this.lblPortValue.Text = "";
                this.numSessions.Value = 0;

                cbCustomizedReports.Checked = false;
                cbCustomer.Enabled = false;

                rbTag.Checked = false;
                rbTerminal.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("LicenceGenerateForm", culture);

                // group box text
                gbSessions.Text = rm.GetString("gbSessions", culture);
                gbFunctionality.Text = rm.GetString("gbFunctionality", culture);
                gbCustomizedReports.Text = rm.GetString("gbCustomizedReports", culture);
                gbDataProcessing.Text = rm.GetString("gbDataProcessing", culture);
                                
                // button's text
                btnGenerate.Text = rm.GetString("btnGenerateLicence", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnSelectAll.Text = rm.GetString("btnSelectAll", culture);
                btnClear.Text = rm.GetString("btnClear", culture);

                // label's text
                lblServer.Text = rm.GetString("lblServer", culture);
                lblPort.Text = rm.GetString("lblPort", culture);
                lblNoSessionsCurr.Text = rm.GetString("lblNoSessionsCurr", culture);
                lblSessionsNum.Text = rm.GetString("lblNoSessions", culture);
                lblCustomer.Text = rm.GetString("lblCustomer", culture);

                // check box
                cbAbsences.Text = rm.GetString("cbAbsences", culture);
                cbAccessControl.Text = rm.GetString("cbAccessControl", culture);
                cbExitPermits.Text = rm.GetString("cbExitPermits", culture);
                cbExtraHours.Text = rm.GetString("cbExtraHours", culture);
                cbMonitoring.Text = rm.GetString("cbMonitoring", culture);
                cbSnapshots.Text = rm.GetString("cbSnapshots", culture);
                cbTimeTable1.Text = rm.GetString("cbTimeTable1", culture);
                cbTimeTable2.Text = rm.GetString("cbTimeTable2", culture);
                cbVisitors.Text = rm.GetString("cbVisitors", culture);
                cbGraficalRep.Text = rm.GetString("menuGraficalRep", culture);
                cbRestaurant.Text = rm.GetString("menuRestaurant", culture);
                cbRoutes.Text = rm.GetString("menuRoutes", culture);
                cbPeopleCounter.Text = rm.GetString("menuPeopleCounter", culture);
                cbBasic.Text = rm.GetString("rbBasic", culture);
                cbStandard.Text = rm.GetString("rbStandard", culture);
                cbAdvance.Text = rm.GetString("rbAdvance", culture);
                cbVacation.Text = rm.GetString("cbVacation", culture);
                cbCustomizedReports.Text = rm.GetString("gbCustomizedReports", culture);
                cbProcessAllTags.Text = rm.GetString("cbProcessAllTags", culture);
                cbRecordsToProcess.Text = rm.GetString("cbRecordsToProcess", culture);
                chbMedicalCheck.Text = rm.GetString("chbMedicalCheck", culture);

                // radio button's text
                rbTag.Text = rm.GetString("rbRouteTag", culture);
                rbTerminal.Text = rm.GetString("rbTerminal", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
                Application.Exit();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            
        }

        private void LicenceGenerate_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                populateCustomerCombo();
                string filePath = Application.StartupPath + "\\licreq.txt";
                if (File.Exists(filePath))
                {
                    // get licence request from licreq.txt
                    // licence request structure:
                    // database_server_port						6 characters
                    // database_server_name						63 characters
                    // sessions_num     						6 characters
                    // 18 moduls    						    6 characters each
                    // customer                                 6 characters
                    FileStream stream = new FileStream(filePath, FileMode.Open);
                    stream.Close();

                    StreamReader reader = new StreamReader(filePath);

                    string licReqText = reader.ReadLine();
                    reader.Close();

                    if (licReqText == null) 
                    {
                        MessageBox.Show(rm.GetString("invalidLicenceReq", culture));
                        this.Close();
                        //Application.Exit();
                    }
                    else
                    {
                        // decrypt licence request
                        byte[] buffer = Convert.FromBase64String(licReqText);
                        licReqText = Util.Misc.decrypt(buffer);

                        if (licReqText.Length != Constants.LicenceReqLength)
                        {
                            MessageBox.Show(rm.GetString("invalidLicenceReq", culture));
                            this.Close();
                        }
                        else
                        {
                            string port = licReqText.Substring(0, Constants.serverPortLength).Trim();
                            string server = licReqText.Substring(Constants.serverPortLength, Constants.serverNameLength).Trim();
                            string noSessions = licReqText.Substring(Constants.serverPortLength + Constants.serverNameLength, Constants.noSessionsLength).Trim();
                            string moduls = licReqText.Substring(Constants.serverPortLength + Constants.serverNameLength + Constants.noSessionsLength, 
                                Constants.modulLength * Constants.modulNum);
                            string customer = licReqText.Substring(Constants.serverPortLength + Constants.serverNameLength + Constants.noSessionsLength
                                + Constants.modulLength * Constants.modulNum, Constants.customerLength);

                            this.lblPortValue.Text = port;
                            this.lblServerValue.Text = server;
                            this.lblNoSessionsCurVal.Text = noSessions;
                            this.numSessions.Value = Int32.Parse(noSessions);

                            if (port.Equals(""))
                            {
                                this.lblPort.Visible = false;
                                this.lblPortValue.Visible = false;
                            }

                            setCBChecked(moduls);
                            setVisibleCustomer(customer);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noLicenceReq", culture));
                    this.Close();
                    //Application.Exit();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int noSessions = (int)numSessions.Value;
                string server = lblServerValue.Text.Trim();
                string port = this.lblPortValue.Text.Trim();

                // validation
                if (noSessions <= 0)
                {
                    MessageBox.Show(rm.GetString("numSessionsNotPositive", culture));
                    numSessions.Select();
                    numSessions.Focus();
                    return;
                }

                if ((cbAbsences.Checked || cbTimeTable2.Checked || cbExitPermits.Checked || cbExtraHours.Checked)
                    && !cbTimeTable1.Checked)
                {
                    MessageBox.Show(rm.GetString("notValidFunctionality", culture));
                    return;
                }

                string moduls = "";

                string customer = "0";

                if (cbCustomizedReports.Checked)
                {
                    customer = ((int)cbCustomer.SelectedValue).ToString();
                    if (customer.Equals(((int)Constants.Customers.UNIPROM).ToString()))
                    {
                        moduls += (int)Constants.Moduls.UNIPROM + ",";
                    }
                }

                if (cbTimeTable1.Checked)
                {
                    moduls += (int)Constants.Moduls.TimeTable1 + ",";
                }
                
                if (cbTimeTable2.Checked)
                {
                    moduls += (int)Constants.Moduls.TimeTable2 + ",";
                }

                if (cbAbsences.Checked)
                {
                    moduls += (int)Constants.Moduls.Absences + ",";
                }

                if (cbExitPermits.Checked)
                {
                    moduls += (int)Constants.Moduls.ExitPermits + ",";
                }

                if (cbExtraHours.Checked)
                {
                    moduls += (int)Constants.Moduls.ExtraHours + ",";
                }

                if (cbAccessControl.Checked)
                {
                    moduls += (int)Constants.Moduls.AccessControl + ",";
                }

                if (cbSnapshots.Checked)
                {
                    moduls += (int)Constants.Moduls.Snapshots + ",";
                }

                if (cbMonitoring.Checked)
                {
                    moduls += (int)Constants.Moduls.Monitoring + ",";
                }

                if (cbVisitors.Checked)
                {
                    moduls += (int)Constants.Moduls.Visitors + ",";
                }

                if (cbGraficalRep.Checked)
                {
                    moduls += (int)Constants.Moduls.GraficalReports + ",";
                }

                if (cbRestaurant.Checked)
                {
                    moduls += (int)Constants.Moduls.Restaurant + ",";
                    if (rbRestaurant1.Checked)
                    {
                        moduls += (int)Constants.Moduls.RestaurantI + ",";
                    }
                    else if (rbRestaurant2.Checked)
                    {
                        moduls += (int)Constants.Moduls.RestaurantII + ",";
                    }
                }

                if (cbRoutes.Checked)
                {
                    if (rbTag.Checked)
                    {
                        moduls += (int)Constants.Moduls.RoutesTag + ",";
                    }
                    else if (rbTerminal.Checked)
                    {
                        moduls += (int)Constants.Moduls.RoutesTerminal + ",";
                    }
                }

                if (cbPeopleCounter.Checked)
                {
                    if (cbAdvance.Checked)
                    {
                        moduls += (int)Constants.Moduls.PeopleCounterAdvance + ",";
                    }
                    else if (cbStandard.Checked)
                    {
                        moduls += (int)Constants.Moduls.PeopleCounterStandard + ",";
                    }
                    else if (cbBasic.Checked)
                    {
                        moduls += (int)Constants.Moduls.PeopleCounterBasic + ",";
                    }
                }

                if (cbVacation.Checked)
                {
                    moduls += (int)Constants.Moduls.Vacation + ",";
                }

                if (cbProcessAllTags.Checked)
                {
                    moduls += (int)Constants.Moduls.ProcessAllTags + ",";
                }

                if (cbSiemensCompatibility.Checked)
                {
                    moduls += (int)Constants.Moduls.SiemensCompatibility + ",";
                }

                if (cbRecordsToProcess.Checked)
                {
                    moduls += (int)Constants.Moduls.RecordsToProcess+ ",";
                }
                if (cbEnterDataByEmpl.Checked)
                {
                    moduls += (int)Constants.Moduls.SelfService + ",";
                }
                if (chbMedicalCheck.Checked)
                {
                    moduls += (int)Constants.Moduls.MedicalCheck + ",";
                }

                if (moduls.Length > 0)
                {
                    moduls = moduls.Substring(0, moduls.Length - 1);
                }

                bool created = Common.Misc.generateLicence(server, port, noSessions.ToString().Trim(), moduls, customer);

                if (created)
                {
                    MessageBox.Show(rm.GetString("licenceMade", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("licenceNotCreated", culture));
                }

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setCBChecked(string moduls)
        {
            try
            {
                ArrayList modList = new ArrayList();

                if (!moduls.Trim().Equals("") && moduls.Length == Constants.modulNum * Constants.modulLength)
                {
                    for (int i = 0; i < Constants.modulNum; i++)
                    {
                        modList.Add(moduls.Substring(i * Constants.modulLength, Constants.modulLength).Trim());
                    }

                    if (modList.Contains(((int)Constants.Moduls.TimeTable1).ToString()))
                    {
                        cbTimeTable1.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.TimeTable2).ToString()))
                    {
                        cbTimeTable2.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.Absences).ToString()))
                    {
                        cbAbsences.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.ExitPermits).ToString()))
                    {
                        cbExitPermits.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.ExtraHours).ToString()))
                    {
                        cbExtraHours.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.AccessControl).ToString()))
                    {
                        cbAccessControl.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.Snapshots).ToString()))
                    {
                        cbSnapshots.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.Monitoring).ToString()))
                    {
                        cbMonitoring.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.Visitors).ToString()))
                    {
                        cbVisitors.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.GraficalReports).ToString()))
                    {
                        cbGraficalRep.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.RestaurantI).ToString()))
                    {
                        cbRestaurant.Checked = true;
                        rbRestaurant1.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.RestaurantII).ToString()))
                    {
                        cbRestaurant.Checked = true;
                        rbRestaurant2.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.RoutesTag).ToString()))
                    {
                        cbRoutes.Checked = true;
                        rbTag.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.RoutesTerminal).ToString()))
                    {
                        cbRoutes.Checked = true;
                        rbTerminal.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.PeopleCounterBasic).ToString()))
                    {
                        cbPeopleCounter.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.PeopleCounterStandard).ToString()))
                    {
                        cbPeopleCounter.Checked = true;
                        cbStandard.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.PeopleCounterAdvance).ToString()))
                    {
                        cbPeopleCounter.Checked = true;
                        cbAdvance.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.Vacation).ToString()))
                    {
                        cbVacation.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.ProcessAllTags).ToString()))
                    {
                        cbProcessAllTags.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.SiemensCompatibility).ToString()))
                    {
                        cbSiemensCompatibility.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.SelfService).ToString()))
                    {
                        cbEnterDataByEmpl.Checked = true;
                    }
                    if (modList.Contains(((int)Constants.Moduls.MedicalCheck).ToString()))
                    {
                        chbMedicalCheck.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.setCBChecked(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbCustomizedReports_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbCustomizedReports.Checked)
                {
                    cbCustomer.Enabled = true;
                }
                else
                {
                    cbCustomer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.cbCustomizedReports_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateCustomerCombo()
        {
            try
            {
                ArrayList customers = new ArrayList();

                Customer cust = new Customer();
                cust.Name = Constants.Customers.Mittal.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Mittal;
                customers.Add(cust);

                cust.Name = Constants.Customers.AAC.ToString();
                cust.MenuItemPos = (int)Constants.Customers.AAC;
                customers.Add(cust);

                cust.Name = Constants.Customers.PIO.ToString();
                cust.MenuItemPos = (int)Constants.Customers.PIO;
                customers.Add(cust);

                cust.Name = Constants.Customers.Geox.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Geox;
                customers.Add(cust);

                cust.Name = Constants.Customers.ATB.ToString();
                cust.MenuItemPos = (int)Constants.Customers.ATB;
                customers.Add(cust);

                cust.Name = Constants.Customers.Millennium.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Millennium;
                customers.Add(cust);

                cust = new Customer();
                cust.Name = Constants.Customers.Sinvoz.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Sinvoz;
                customers.Add(cust);

                cust.Name = Constants.Customers.FOD.ToString();
                cust.MenuItemPos = (int)Constants.Customers.FOD;
                customers.Add(cust);

                cust.Name = Constants.Customers.Vlatacom.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Vlatacom;
                customers.Add(cust);

                cust.Name = Constants.Customers.JUBMES.ToString();
                cust.MenuItemPos = (int)Constants.Customers.JUBMES;
                customers.Add(cust);

                cust.Name = Constants.Customers.JEEP.ToString();
                cust.MenuItemPos = (int)Constants.Customers.JEEP;
                customers.Add(cust);

                cust.Name = Constants.Customers.UNIPROM.ToString();
                cust.MenuItemPos = (int)Constants.Customers.UNIPROM;
                customers.Add(cust);

                cust.Name = Constants.Customers.DSF.ToString();
                cust.MenuItemPos = (int)Constants.Customers.DSF;
                customers.Add(cust);

                cust.Name = Constants.Customers.ZIN.ToString();
                cust.MenuItemPos = (int)Constants.Customers.ZIN;
                customers.Add(cust);

                cust.Name = Constants.Customers.EUNET.ToString();
                cust.MenuItemPos = (int)Constants.Customers.EUNET;
                customers.Add(cust);

                cust.Name = Constants.Customers.Ministarstvo.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Ministarstvo;
                customers.Add(cust);

                cust.Name = Constants.Customers.GSK.ToString();
                cust.MenuItemPos = (int)Constants.Customers.GSK;
                customers.Add(cust);

                cust.Name = Constants.Customers.Niksic.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Niksic;
                customers.Add(cust);

                cust.Name = Constants.Customers.FIAT.ToString();
                cust.MenuItemPos = (int)Constants.Customers.FIAT;
                customers.Add(cust);

                cust.Name = Constants.Customers.Lames.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Lames;
                customers.Add(cust);

                cust.Name = Constants.Customers.WAKERNEUSON.ToString();
                cust.MenuItemPos = (int)Constants.Customers.WAKERNEUSON;
                customers.Add(cust);
                                
                cust.Name = Constants.Customers.ConfezioniAndrea.ToString();
                cust.MenuItemPos = (int)Constants.Customers.ConfezioniAndrea;
                customers.Add(cust);

                cust.Name = Constants.Customers.PMC.ToString();
                cust.MenuItemPos = (int)Constants.Customers.PMC;
                customers.Add(cust);

                cust.Name = Constants.Customers.NikolaTesla.ToString();
                cust.MenuItemPos = (int)Constants.Customers.NikolaTesla;
                customers.Add(cust);

                cust.Name = Constants.Customers.Grundfos.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Grundfos;
                customers.Add(cust);

                cust.Name = Constants.Customers.Hyatt.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Hyatt;
                customers.Add(cust);

                cust.Name = Constants.Customers.Magna.ToString();
                cust.MenuItemPos = (int)Constants.Customers.Magna;
                customers.Add(cust);

                cbCustomer.DataSource = customers;
                cbCustomer.DisplayMember = "Name";
                cbCustomer.ValueMember = "MenuItemPos";

                if (customers.Count > 0)
                {
                    cbCustomer.SelectedValue = ((Customer)customers[0]).MenuItemPos;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.populateCustomerCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setVisibleCustomer(string customer)
        {
            try
            {
                if (!customer.Trim().Equals("") && !customer.Trim().Equals(Constants.noCustomer))
                {
                    cbCustomizedReports.Checked = true;
                    cbCustomer.SelectedValue = Int32.Parse(customer);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.setVisibleCustomer(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cbAbsences.Checked = cbAccessControl.Checked = cbExitPermits.Checked
                    = cbExtraHours.Checked = cbGraficalRep.Checked = cbMonitoring.Checked
                        = cbRestaurant.Checked = cbRoutes.Checked = cbSnapshots.Checked
                            = cbTimeTable1.Checked = cbTimeTable2.Checked = cbVisitors.Checked
                                = cbVacation.Checked = chbMedicalCheck.Checked = true;

                cbPeopleCounter.Checked = cbEnterDataByEmpl.Checked = false;

                rbTag.Checked = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.btnSelectAll_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cbAbsences.Checked = cbAccessControl.Checked = cbExitPermits.Checked
                    = cbExtraHours.Checked = cbGraficalRep.Checked = cbMonitoring.Checked
                        = cbRestaurant.Checked = cbRoutes.Checked = cbSnapshots.Checked
                            = cbTimeTable1.Checked = cbTimeTable2.Checked = cbVisitors.Checked
                                = cbPeopleCounter.Checked = cbVacation.Checked = cbSiemensCompatibility.Checked = cbEnterDataByEmpl.Checked
                                    = chbMedicalCheck.Checked = false;

                rbTag.Checked = rbTerminal.Checked = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbRoutes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbRoutes.Checked)
                {
                    if (!rbTerminal.Checked)
                    {
                        rbTag.Checked = true;
                    }
                }
                else
                {
                    rbTag.Checked = rbTerminal.Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.cbRoutes_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbTag_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbTag.Checked && !cbRoutes.Checked)
                {
                    cbRoutes.Checked = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.rbTag_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
            }

        }

        private void rbTerminal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbTerminal.Checked && !cbRoutes.Checked)
                {
                    cbRoutes.Checked = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.rbTerminal_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow; 
            }
        }

        private void cbPeopleCounter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cbAbsences.Enabled = cbAccessControl.Enabled = cbExitPermits.Enabled = cbRestaurant.Enabled = cbRoutes.Enabled = cbSnapshots.Enabled = cbTimeTable1.Enabled = cbTimeTable2.Enabled = cbGraficalRep.Enabled = chbMedicalCheck.Enabled
                    = cbMonitoring.Enabled = cbProcessAllTags.Enabled = cbVisitors.Enabled = cbCustomizedReports.Enabled = cbVacation.Enabled = cbExtraHours.Enabled = cbSiemensCompatibility.Enabled = cbRecordsToProcess.Enabled = !cbPeopleCounter.Checked;

                cbProcessAllTags.Checked = cbPeopleCounter.Checked;
                if (cbPeopleCounter.Checked)
                {
                    cbAbsences.Checked = cbAccessControl.Checked = cbExitPermits.Checked = cbExtraHours.Checked = cbGraficalRep.Checked =
                         cbRestaurant.Checked = cbMonitoring.Checked = cbRoutes.Checked = cbSnapshots.Checked = cbTimeTable1.Checked = cbTimeTable2.Checked = cbVisitors.Checked =
                                cbCustomizedReports.Checked = cbVacation.Checked = cbSiemensCompatibility.Checked
                                    = cbRecordsToProcess.Checked = cbEnterDataByEmpl.Checked = chbMedicalCheck.Checked = false;

                    rbTag.Checked = rbTag.Enabled = rbTerminal.Checked = rbTerminal.Enabled = false;

                    cbBasic.Checked = true;

                }
                else
                {
                    cbBasic.Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.cbPeopleCounter_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbBasic_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbBasic.Checked)
                {
                    cbPeopleCounter.Checked = true;
                }
                else
                {
                    cbPeopleCounter.Checked = cbStandard.Checked = cbAdvance.Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.cbBasic_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbStandard_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbStandard.Checked)
                {
                    cbBasic.Checked = true;
                }
                else
                {
                    cbAdvance.Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.cbStandard_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbAdvance_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbAdvance.Checked)
                {
                    cbBasic.Checked = cbStandard.Checked = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.cbAdvance_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbSiemensCompatibility_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbSiemensCompatibility.Checked)
                {
                    cbAccessControl.Checked = cbAccessControl.Enabled = cbSnapshots.Checked = cbSnapshots.Enabled = cbMonitoring.Checked =
                    cbMonitoring.Enabled = cbVisitors.Checked = cbPeopleCounter.Checked = cbPeopleCounter.Enabled =
                    cbRestaurant.Checked = cbRestaurant.Enabled = cbRestaurant.Checked = cbRestaurant.Enabled = cbBasic.Checked = cbBasic.Enabled =
                    cbAdvance.Checked = cbAdvance.Enabled = cbStandard.Checked = cbStandard.Enabled = false;
                }
                else
                {
                    cbAccessControl.Enabled = true;
                    cbSnapshots.Enabled = true;
                    cbMonitoring.Enabled = true;
                    cbPeopleCounter.Enabled = true;
                    cbRestaurant.Enabled = true;
                    cbAdvance.Enabled = true;
                    cbStandard.Enabled = true;
                    cbBasic.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.chbSiemensCompatibility_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.Arrow; 
            }
        }

        private void cbRestaurant_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbRestaurant.Checked)
                {
                    if (!rbRestaurant1.Checked)
                    {
                        rbRestaurant1.Checked = true;
                    }
                }
                else
                {
                    rbRestaurant1.Checked = rbRestaurant2.Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LicenceGenerate.cbRestaurant_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}