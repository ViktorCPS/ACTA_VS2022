using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Util;
using Common;
using TransferObjects;

namespace ACTAConfigManipulation
{
    public partial class ConfigAdd : Form
    {
        // Controller instance
        public NotificationController Controller;

        // Observer client instance
        public NotificationObserverClient observerClient;

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        int startPosY = 21;
        int startPosX = 5;

        private string tabName = "";

        public ConfigAdd(string tabName)
        {
            try
            {
                InitializeComponent();
                IntitObserverClient();

                this.CenterToScreen();

                // Init Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("ACTAConfigManipulation.Resource", typeof(ConfigAdd).Assembly);
                setLanguage();

                this.tabName = tabName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Init Controller and Observer Client
        /// </summary>
        private void IntitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
            Controller.AttachToNotifier(observerClient);
            this.observerClient.Notification += new NotificationEventHandler(this.AuxPortChanged);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show(rm.GetString("setupExit", culture));
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                // Form text
                this.Text = rm.GetString("ConfigAdd", culture);

                // Tab pages text
                this.tabPageGates.Text = rm.GetString("Gates", culture);
                this.tabPageVisitors.Text = rm.GetString("Visitors", culture);

                // groupBoxes
                this.gbVisitorsCode.Text = rm.GetString("VisitorsCode", culture);
                this.gbUpdateEmplLoc.Text = rm.GetString("UpdateEmplLoc", culture);
                this.gbOfflineVisits.Text = rm.GetString("gbOfflineVisits", culture);

                // buttons text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnGatesSave.Text = rm.GetString("btnSave", culture);
                this.btnVisitorsSave.Text = rm.GetString("btnSave", culture);

                // labels text
                this.lblGateID.Text = rm.GetString("lblID", culture);
                this.lblGateName.Text = rm.GetString("lblGateName", culture);
                this.lblGateDescription.Text = rm.GetString("lblDescription", culture);
                this.lblAuxPort.Text = rm.GetString("lblAuxPort", culture);
                this.lblReaders.Text = rm.GetString("lblReadersAuxPorts", culture);
                this.lblWU.Text = rm.GetString("lblWU", culture);
                this.lblDesc.Text = rm.GetString("lblDescription", culture);
                this.lblUpdateEmplLoc.Text = rm.GetString("lblUpdateEmplLoc", culture);
                this.lblOfflineVisits.Text = rm.GetString("lblOfflineVisits", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.setLanguage(): " + ex.Message + "\n");
                throw (ex);
            }
        }

        private void populateWUCombo()
        {
            try
            {
                WorkingUnit wu = new WorkingUnit();
                wu.WUTO.WorkingUnitID = Constants.basicVisitorCode;
                List<WorkingUnitTO> wUnitsVisitors = wu.Search();

                if (wUnitsVisitors.Count > 0)
                {
                    this.tbDesc.Text = wUnitsVisitors[0].Description;
                }

                wUnitsVisitors = wu.FindAllChildren(wUnitsVisitors);

                cbWU.DataSource = wUnitsVisitors;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
                
                if (wu.Find((int)cbWU.SelectedValue))
                {
                    this.tbDesc.Text = wu.WUTO.Description;
                }
                else
                {
                    this.tbDesc.Text = "";
                }                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.populateWUCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateUpdEmplLocCombo()
        {
            try
            {
                ArrayList updEmplLoc = new ArrayList();
                updEmplLoc.Add(Constants.yesL);
                updEmplLoc.Add(Constants.noL);

                this.cbUpdEmplLoc.DataSource = updEmplLoc;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.populateUpdEmplLocCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOfflineVisitsCombo()
        {
            try
            {
                ArrayList offlineVisists = new ArrayList();
                offlineVisists.Add(Constants.yesL);
                offlineVisists.Add(Constants.noL);

                this.cbOfflineViists.DataSource = offlineVisists;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.populateOfflineVisitsCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void ConfigAdd_Load(object sender, EventArgs e)
        {
            try
            {
                // Gates
                this.panel1.Controls.Clear();

                addHeader();

                List<GateTO> gates = new Gate().Search();

                int y = startPosY;
                foreach (GateTO gate in gates)
                {
                    GatesAuxPorts gatesAuxPorts = new GatesAuxPorts(startPosX, y, gate);
                    this.panel1.Controls.Add(gatesAuxPorts);
                    y += Constants.gateAuxPortHeight;
                }

                this.panel1.Invalidate();
                this.Invalidate();

                // Visitors
                populateWUCombo();
                populateUpdEmplLocCombo();
                populateOfflineVisitsCombo();

                if (!this.tabName.Equals(""))
                {
                    foreach (TabPage page in this.tabControl1.TabPages)
                    {
                        if (!page.Text.Equals(this.tabName))
                        {
                            this.tabControl1.TabPages.Remove(page);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.ConfigAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void addHeader()
        {
			this.panel1.Controls.Add(lblGateID);
			this.panel1.Controls.Add(lblGateName);
			this.panel1.Controls.Add(lblGateDescription);
			this.panel1.Controls.Add(lblAuxPort);
            this.panel1.Controls.Add(lblReaders);
        }

        /// <summary>
		/// Method that implements selecting of event which will be rised.
		/// </summary>
        public void AuxPortChanged(object sender, NotificationEventArgs e)
        {
            try
            {
                if (e.readersAuxPorts != null && e.readersAuxPorts.Count > 0)
                {
                    foreach (ReaderAuxPort readerAuxPort in e.readersAuxPorts)
                    {
                        readerAuxPort.PosX += this.panel1.AutoScrollPosition.X;
                        readerAuxPort.PosY += this.panel1.AutoScrollPosition.Y;
                        readerAuxPort.Location = new Point(readerAuxPort.PosX, readerAuxPort.PosY);

                        this.panel1.Controls.Add(readerAuxPort);
                    }

                    this.panel1.Invalidate();
                    this.Invalidate();
                }
                else if (e.gateID >= 0)
                {
                    int i = 0;

                    int controlsNum = this.panel1.Controls.Count;

                    while (i < controlsNum)
                    {
                        if ((this.panel1.Controls[i] is ReaderAuxPort) && (((ReaderAuxPort)this.panel1.Controls[i]).GateID == e.gateID))
                        {
                            this.panel1.Controls.RemoveAt(i);
                            controlsNum--;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    this.panel1.Invalidate();
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.AuxPortChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGatesSave_Click(object sender, EventArgs e)
        {
            try
            {
                // all selected Aux Ports must be different
                ArrayList auxPorts = new ArrayList();

                foreach (Control ctrl in this.panel1.Controls)
                {
                    if (ctrl is ReaderAuxPort)
                    {
                        auxPorts.Add(((ReaderAuxPort) ctrl).AuxPort.Trim());
                    }
                }

                foreach (string auxPort in auxPorts)
                {
                    if (auxPorts.IndexOf(auxPort) != auxPorts.LastIndexOf(auxPort))
                    {
                        MessageBox.Show(rm.GetString("sameAuxPort", culture));
                        return;
                    }
                }

                // form gates string

                ArrayList gatesString = new ArrayList();
                string gates = "";

                foreach (Control ctrl in this.panel1.Controls)
                {
                    if ((ctrl is GatesAuxPorts) && (((GatesAuxPorts)ctrl).Selected))
                    {
                        gatesString.Add(((GatesAuxPorts)ctrl).Gate.GateID.ToString().Trim());
                    }
                }

                gatesString.Sort();

                foreach (string gateID in gatesString)
                {
                    gates += gateID + ",";
                }

                if (!gates.Equals(""))
                {
                    gates = gates.Substring(0, gates.Length - 1);
                    Util.Misc.configAdd("Gates", gates);
                }

                // remove Aux Ports from config
                Util.Misc.configRemove("AuxPorts");

                // form Aux Ports
                string[] gatesSelected = gates.Split(',');
                
                // key is Aux Port + gateID, values is selected Aux Ports
                Hashtable auxPortsKeys = new Hashtable();

                if (gatesSelected.Length > 0)
                {
                    foreach (string gateID in gatesSelected)
                    {
                        auxPortsKeys.Add("AuxPorts" + gateID, new StringBuilder());
                        
                        foreach (Control ctrl in this.panel1.Controls)
                        {
                            if ((ctrl is ReaderAuxPort) && (((ReaderAuxPort)ctrl).GateID.ToString().Equals(gateID)))
                            {
                                ((StringBuilder)auxPortsKeys["AuxPorts" + gateID]).Append(((ReaderAuxPort)ctrl).AuxPort.Trim() + ",");
                            }
                        }
                    }

                    foreach (string key in auxPortsKeys.Keys)
                    {
                        string value = ((StringBuilder) auxPortsKeys[key]).ToString().Trim();

                        if (!value.Equals(""))
                        {
                            value = value.Substring(0, value.Length - 1);
                            Util.Misc.configAdd(key, value);
                        }
                    }
                }

                MessageBox.Show(rm.GetString("configGatesAdded", culture));

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.btnGatesSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                WorkingUnit wu = new WorkingUnit();
                if ((cbWU.SelectedValue is int) && (wu.Find((int)cbWU.SelectedValue)))
                {
                    this.tbDesc.Text = wu.WUTO.Description;
                }
                else
                {
                    this.tbDesc.Text = "";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnVisitorsSave_Click(object sender, EventArgs e)
        {
            try
            {
                // add visitors code
                string visitorsCode = cbWU.SelectedValue.ToString().Trim();
                Util.Misc.configAdd("VisitorsCode", visitorsCode);

                // add update employee location
                string updEmplLoc = cbUpdEmplLoc.Text.Trim();
                Util.Misc.configAdd("UpdateEmployeeLocation", updEmplLoc);

                // add update employee location
                string offlineVisits = cbOfflineViists.Text.Trim();
                Util.Misc.configAdd("OfflineVisits", offlineVisits);

                MessageBox.Show(rm.GetString("visitorsParametersSaved", culture));
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ConfigAdd.btnVisitorsSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}