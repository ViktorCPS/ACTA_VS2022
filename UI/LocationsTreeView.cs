using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using Common;
using TransferObjects;
using System.Resources;
using System.Globalization;
using Util;


namespace UI
{
    public partial class LocationsTreeView : Form
    {
        private System.Data.DataSet dsLocations;
        public System.Data.DataView locationsDataView;
        public string selectedLocation;

        private DebugLog log;
        private ResourceManager rm;
        private CultureInfo culture;

        ToolTip toolTip1;
        bool loading = true;
        public LocationsTreeView()
        {
            InitializeComponent();
            dsLocations = new System.Data.DataSet();
            locationsDataView = new System.Data.DataView();
        }
        public LocationsTreeView(List<LocationTO> locationList)
        {
            InitializeComponent();
            string locString = "";
            foreach (LocationTO loc in locationList)
            {
                locString += loc.LocationID.ToString().Trim() + ",";
            }

            if (locString.Length > 0)
            {
                locString = locString.Substring(0, locString.Length - 1);
            }
            dsLocations = new Location().getLocations(locString);
            locationsDataView = new System.Data.DataView();
        }

        private void LocationsTreeView_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(WorkingUnits).Assembly);
                setLanguage();

                selectedLocation = "";

                toolTip1 = new ToolTip();

                this.CenterToScreen();
                populateDataTreeView();
                loading = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LocationsTreeView.locationsTreeView_AfterSelect(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            
            }
        }

        private void populateDataTreeView()
        {
            try
            {
                if (dsLocations != null)
                {
                    this.locationsDataView = dsLocations.Tables["Locations"].DefaultView;
                    BindingSource bs = new BindingSource();
                    bs.DataSource = this.locationsDataView;
                    this.dataTreeView1.ShowNodeToolTips = true;
                    this.dataTreeView1.IDColumn = "";
                    this.dataTreeView1.ParentIDColumn = "";
                    this.dataTreeView1.ToolTipTextColumn = "";
                    this.dataTreeView1.NameColumn = "";
                    this.dataTreeView1.DataSource = bs;
                    this.dataTreeView1.IDColumn = "location_id";
                    this.dataTreeView1.ParentIDColumn = "parent_location_id";
                    this.dataTreeView1.ToolTipTextColumn = "description";
                    this.dataTreeView1.NameColumn = "name";

                    this.dataTreeView1.Refresh();
                    dataTreeView1.ExpandAll();
                    dataTreeView1.SelectedNode = dataTreeView1.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "LocationsTreeView.populateDataTreeView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void setLanguage()
        {
            this.Text = rm.GetString("locationSelecting", culture);
        }

        private void dataTreeView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (!loading)
                {
                    TreeNode node = this.dataTreeView1.GetNodeAt(e.X, e.Y);
                    selectedLocation = node.Text;
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LocationsTreeView.locationsTreeView_AfterSelect(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dataTreeView1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Get the node at the current mouse pointer location.
                TreeNode theNode = this.dataTreeView1.GetNodeAt(e.X, e.Y);
                if ((theNode != null))
                {
                    // Verify that the tag property is not "null".
                    if (theNode.ToolTipText != "")
                    {
                        // Change the ToolTip only if the pointer moved to a new node.
                        if (theNode.ToolTipText != this.toolTip1.GetToolTip(this.dataTreeView1))
                        {
                            this.toolTip1.Show(theNode.ToolTipText, this, e.X + 30, e.Y + 50);
                        }
                    }
                    else
                    {
                        this.toolTip1.SetToolTip(this, "");
                    }
                }
                else     // Pointer is not over a node so clear the ToolTip.
                {
                    this.toolTip1.SetToolTip(this, "");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LocationsTreeView.dataTreeView1_MouseMove(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            finally { 
                this.Cursor = Cursors.Arrow; 
            }
        }

        private void LocationsTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this, "");
        }

        private void dataTreeView1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor; 
                if (!loading)
                {
                    if (e.KeyCode.Equals(Keys.Enter))
                    {
                        selectedLocation = dataTreeView1.SelectedNode.Text;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LocationsTreeView.dataTreeView1_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}