using System;
using System.Drawing;
using System.Collections;
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
    public partial class WorkingUnitsTreeView : Form
    {
        private System.Data.DataSet dataSetWU;
        public System.Data.DataView wuDataView;
        public string selectedWorkingUnit;

        private DebugLog log;
        private ResourceManager rm;
        private CultureInfo culture;

        ToolTip toolTip1;
        bool loading = true;


        public WorkingUnitsTreeView()
        {
            InitializeComponent();

            dataSetWU = new System.Data.DataSet();
            wuDataView = new System.Data.DataView();
        }

        public WorkingUnitsTreeView(System.Data.DataSet dsWorkingUnits)
        {
            dataSetWU = dsWorkingUnits;
            wuDataView = new System.Data.DataView();
            InitializeComponent();
        }
       
        private void WorkingUnitsTreeView_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(WorkingUnits).Assembly);
                setLanguage();

                selectedWorkingUnit = "";

                toolTip1 = new ToolTip();


                this.CenterToScreen();
                populateDataTreeView();
                loading = false;


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnitsTreeView.locationsTreeView_AfterSelect(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { this.Cursor = Cursors.Arrow; }
        }
        private void workingUnitsTreeView_Click(object sender,MouseEventArgs e)
        {
            try
            {
                if (!loading)
                {
                        TreeNode node = this.dataTreeView1.GetNodeAt(e.X, e.Y);
                        selectedWorkingUnit = node.Text;
                        this.Close();                   
                }
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnitsTreeView.locationsTreeView_AfterSelect(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dataTreeView1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
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
                log.writeLog(DateTime.Now + " WorkingUnitsTreeView.dataTreeView1_MouseMove(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void populateDataTreeView()
        {
            try
            {
                if (dataSetWU != null)
                {
                    if (dataSetWU.Tables.Contains("Working units"))
                    {
                        this.wuDataView = dataSetWU.Tables["Working units"].DefaultView;
                        BindingSource bs = new BindingSource();
                        bs.DataSource = this.wuDataView;

                        this.dataTreeView1.IDColumn = "";
                        this.dataTreeView1.ParentIDColumn = "";
                        this.dataTreeView1.ToolTipTextColumn = "";
                        this.dataTreeView1.NameColumn = "";
                        this.dataTreeView1.DataSource = bs;
                        this.dataTreeView1.IDColumn = "working_unit_id";
                        this.dataTreeView1.ParentIDColumn = "parent_working_unit_id";
                        this.dataTreeView1.ToolTipTextColumn = "description";
                        this.dataTreeView1.NameColumn = "name";
                        this.dataTreeView1.Refresh();
                        dataTreeView1.ExpandAll();
                        dataTreeView1.SelectedNode = dataTreeView1.Nodes[0];
                    }
                    else if (dataSetWU.Tables.Contains("Organization units"))
                    {
                        this.wuDataView = dataSetWU.Tables["Organization units"].DefaultView;
                        BindingSource bs = new BindingSource();
                        bs.DataSource = this.wuDataView;

                        this.dataTreeView1.IDColumn = "";
                        this.dataTreeView1.ParentIDColumn = "";
                        this.dataTreeView1.ToolTipTextColumn = "";
                        this.dataTreeView1.NameColumn = "";
                        this.dataTreeView1.DataSource = bs;
                        this.dataTreeView1.IDColumn = "organizational_unit_id";
                        this.dataTreeView1.ParentIDColumn = "parent_organizational_unit_id";
                        this.dataTreeView1.ToolTipTextColumn = "description";
                        this.dataTreeView1.NameColumn = "name";
                        this.dataTreeView1.Refresh();
                        dataTreeView1.ExpandAll();
                        dataTreeView1.SelectedNode = dataTreeView1.Nodes[0];
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnitsTreeView.populateDataTreeView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void setLanguage()
        {
            this.Text = rm.GetString("workingUnitsSelecting", culture);
        }
        private void WorkingUnits_MouseMove(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this, "");
        }

        private void dataTreeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!loading)
            {
                if (e.KeyCode.Equals(Keys.Enter))
                {
                    selectedWorkingUnit = dataTreeView1.SelectedNode.Text;
                    this.Close();
                }
            }
        }
    }
}