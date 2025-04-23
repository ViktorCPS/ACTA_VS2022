using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACTAReaderMonitorManagement;
using System.Configuration;
using Common;
using TransferObjects;
using Util;
using DataAccess;

namespace ACTAReaderMonitor
{
    public partial class ACTAMonitor : Form
    {

        public ACTAMonitor()
        {
            InitializeComponent();
        }
        Object dbconnection = null;
        List<VTControl> ctrlsList = new List<VTControl>();
        List<ACTAReaderMonitorManager> controlList = new List<ACTAReaderMonitorManager>();
        int reader_num = -1;


        private Dictionary<int, ACTAReaderMonitorManager> ticketControlManagerList;

        // Debug
        private DebugLog log;

        private string serviceStatus = "";

        private object locker = new object();
        private static object instanceLocker = new object();

        public System.Timers.Timer timerDB;
        public System.Timers.Timer timerDropArms;
        bool DBconnected = false;

        DAOFactory daoFactory = null;
        string noDBConnectionString = "Cannot connect to the database!";


        private void Start_Click(object sender, EventArgs e)
        {
            NotificationController.SetApplicationName("ACTAReaderMonitorService");
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            ticketControlManagerList = new Dictionary<int, ACTAReaderMonitorManager>();

            // init ticket controller
            try
            {
                try
                {
                    daoFactory = DAOFactory.getDAOFactory();
                    DBconnected = true;

                    InitializeTicketControlManager();
                    StartTicketProcessing();

                }
                catch (Exception ex)
                {
                    DBconnected = false;
                }
                if (!DBconnected)
                {
                    // timer for checking database connection
                    timerDB = new System.Timers.Timer(Constants.dbRefreshTime);
                    timerDB.Elapsed += new System.Timers.ElapsedEventHandler(timerDB_Elapsed);
                    timerDB.Start();
                }
                Stop.Visible = true;
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }

        }

        private void tryToConnect()
        {
            try
            {
                try
                {
                    if (!DBconnected)
                    {
                        try
                        {
                            daoFactory = DAOFactory.getDAOFactory();
                            DBconnected = true;

                            try
                            {
                                InitializeTicketControlManager();
                            }
                            catch (Exception ex)
                            {
                                DBconnected = false;
                                throw;
                            }
                            StartTicketProcessing();
                            timerDB.Enabled = false;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        DBconnected = daoFactory.TestDataSourceConnection();
                    }
                }
                catch
                {
                    DBconnected = false;
                }

            }
            catch (Exception ex)
            {
                DBconnected = false;
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTAReaderMonitorService.tryToConnect(): " + ex.Message + "\n");
            }
        }
        public void StartTicketProcessing()
        {
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ACTAReaderMonitorManager manager in ticketControlManagerList.Values)
                    {
                        manager.Start();
                    }
                }
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void timerDB_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                try
                {
                    if (!DBconnected)
                    {
                        tryToConnect();
                    }
                    else if (serviceStatus.Equals(noDBConnectionString))
                    {

                    }
                    else timerDB.Enabled = false;
                }
                catch
                {
                    DBconnected = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTAReaderMonitorService.timerDB_Elapsed(): " + ex.Message + "\n");
            }
        }
        private bool InitializeTicketControlManager()
        {
            bool success = false;
            try
            {
                string reader_num = "";
                if (ConfigurationManager.AppSettings["gates"] != null)
                {
                    reader_num = ConfigurationManager.AppSettings["gates"];
                }
                string[] gates = reader_num.Split(',');
                List<GateTO> list = new List<GateTO>();
                foreach (string gate in gates)
                {
                    GateTO reader = new Gate().Find(int.Parse(gate));
                    list.Add(reader);
                }

                int x = 5;
                int y = 5;
                //list = new Gate().
                for (int i = 0; i < list.Count; i++)
                {
                    GateTO readerInList = list[i];
                    Reader reader = new Reader();
                    List<ReaderTO> readers = reader.getReaders(-1, readerInList.GateID);
                    foreach (ReaderTO readerTO in readers)
                    {
                        
                            VTControl ctrl = new VTControl();
                            //ValidationTerminalTO valTerminal = new ValidationTerminalTO();
                            //valTerminal.IpAddress = readerInList.ConnectionAddress;
                            //valTerminal.Name = readerInList.Description;
                            //valTerminal.ValidationTerminalID = readerInList.ReaderID;
                            //readerTO.ConnectionAddress = "10.20.8.200";
                            ctrl.ValidationTerminal = readerInList;
                            ctrl.Location = new Point(x, y);
                            panelControls.Controls.Add(ctrl);
                            ctrlsList.Add(ctrl);
                            if ((x + ctrl.Width * 2 + 5) > panelControls.Width)
                            {
                                y += ctrl.Height + 5;
                            }
                            else
                            {
                                x += ctrl.Width + 5;
                            }
                            ACTAReaderMonitorManager control;

                            control = new ACTAReaderMonitorManager(readerTO);
                            ticketControlManagerList.Add(readerTO.ReaderID, control);
                            controlList.Add(control);
                        }
                    
                }

                success = true;

            }
            catch (Exception ex)
            {
                //log.writeLog(ex.Message);
                if (ticketControlManagerList != null)
                {
                    foreach (ACTAReaderMonitorManager manager in ticketControlManagerList.Values)
                    {
                        manager.Stop();
                    }
                }
                throw ex;
            }
            return success;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < controlList.Count; i++)
            {
                ctrlsList[i].setStatus0(controlList[i].workStatus0);
                ctrlsList[i].setStatus1(controlList[i].workStatus1);
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach (ACTAReaderMonitorManager control in controlList)
            {
                control.Stop();
            }
            //foreach (AACManagerCommands control in controlCommandsList)
            //{
            //    control.Stop();
            //}g
            foreach (VTControl control in ctrlsList)
            {
                control.Dispose();
            }
            timer1.Enabled = false;
            panelControls.Controls.Clear();
            Stop.Visible = false;
            this.Cursor = Cursors.Arrow;
            if (dbconnection != null)
                DBConnectionManager.Instance.CloseDBConnection(dbconnection);
        }

        private void ACTAMonitor_Load(object sender, EventArgs e)
        {

        }


    }
}
