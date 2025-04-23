using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Configuration;

using TransferObjects;
using Common;
using Util;

using System.Resources;
using System.Globalization;
using System.IO;

namespace UI
{
    public partial class Maps : Form
    {
        DebugLog log;
        private CultureInfo culture;
        private ResourceManager rm;
        private ArrayList currentMapList;
        private Map currentMap;
        //sort index's
        public const int mapIDIndex = 0;
        public const int nameIndex = 1;
        public const int descriptionIndex = 2;
        public const int parentMapIndex = 3;

        private int sortField;
        private int sortOrder;

        ApplUserTO logInUser;
        List<ApplRoleTO> userRoles;

        Filter filter;

        public Maps()
        {
            logInUser = NotificationController.GetLogInUser();
            userRoles = NotificationController.GetCurrentRoles();


            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            currentMapList = new Map().Search(-1, -1, "", "");
            currentMap = new Map();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeeAbsences).Assembly);
            setLanguage();
            this.CenterToScreen();
            populateParentMapCombo();
            populateListView(currentMapList);

            this.lblParentMap.Visible = false;
            cbParentMap.Visible = false;
        }

        private void setLanguage()
		{
			try
			{
				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
				
				// Form name
				this.Text = rm.GetString("mapsMaintenance", culture);
				
				// group box text
                this.gbSearch.Text = rm.GetString("gbSearch", culture);
                this.gbFilter.Text = rm.GetString("gbFilter", culture);

				// label's text
				lblName.Text = rm.GetString("lblName", culture);
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblParentMap.Text = rm.GetString("lblParentMap", culture);
								
				// list view initialization
				this.lvMaps.BeginUpdate();
				lvMaps.Columns.Add(rm.GetString("hdrMapID", culture), (lvMaps.Width - 4) / 4, HorizontalAlignment.Left);
				lvMaps.Columns.Add(rm.GetString("hdrName", culture), (lvMaps.Width - 4) / 4, HorizontalAlignment.Left);
				lvMaps.Columns.Add(rm.GetString("hdrDescription", culture), (lvMaps.Width - 4) / 4, HorizontalAlignment.Left);
				lvMaps.Columns.Add(rm.GetString("hdrParentMap", culture), (lvMaps.Width - 4) / 4, HorizontalAlignment.Left);
				lvMaps.EndUpdate();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " Maps.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}        

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }         
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ClearListView();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int parentMapID = -1;
                if ((int)this.cbParentMap.SelectedValue != -1)
                {
                    parentMapID = (int)this.cbParentMap.SelectedValue;
                }
                currentMapList = new Map().Search(-1, parentMapID, tbName.Text.ToString(), tbDescription.Text.ToString());
                if (currentMapList.Count > 0)
                {
                    populateListView(currentMapList);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ClearListView()
        {
            try
            {
                lvMaps.Items.Clear();
                this.pictureBox1.Image = null;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.ClearListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void populateParentMapCombo()
        {
            try
            {
                ArrayList mapArray = new ArrayList();
                               
                 mapArray = new Map().Search(-1, -1, "", "");

                 Map map = new Map();
                 map.Name = rm.GetString("all", culture);
                 mapArray.Insert(0, map);

                 cbParentMap.DataSource = mapArray;
                 cbParentMap.DisplayMember = "Name";
                 cbParentMap.ValueMember = "MapID";
                 cbParentMap.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.populateParentMapCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populateListView(ArrayList mapsList)
        {
            try
            {
                lvMaps.BeginUpdate();
                lvMaps.Items.Clear();

                if (mapsList.Count > 0)
                {
                   
                        for (int i = 0; i < mapsList.Count; i++)
                        {
                            Map map = (Map)mapsList[i];
                            ListViewItem item = new ListViewItem();
                            item.Text = map.MapID.ToString().Trim();                           
                            item.SubItems.Add(map.Name.Trim());
                            item.SubItems.Add(map.Description.Trim());
                            item.SubItems.Add(map.getParentMap().Name);
                            item.Tag = map.MapID;
                           
                            lvMaps.Items.Add(item);
                            
                        }
                    
                }

                lvMaps.EndUpdate();
                lvMaps.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvMaps.SelectedItems.Count > 0)
                {

                    foreach (Map map in currentMapList)
                    {
                        if (map.MapID == int.Parse(lvMaps.SelectedItems[0].Tag.ToString()))
                            currentMap = map;
                    }
                    byte[] mapPhoto = currentMap.Content;
                    if (currentMap.Content.Length > 0)
                    {
                        MemoryStream memStream = new MemoryStream(mapPhoto);
                        Image img = new Bitmap(memStream);
                        this.pictureBox1.Image = img;
                        memStream.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                MapAdd mapAdd = new MapAdd();
                mapAdd.ShowDialog();
                btnSearch.PerformClick();
                populateParentMapCombo();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally{
            this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool isDeleted = true;
                MapsObjectHdr mapObj = new MapsObjectHdr();

                if (lvMaps.SelectedItems.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteMap", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int id = -1;
                        foreach (ListViewItem item in lvMaps.SelectedItems)
                        {
                            mapObj = new MapsObjectHdr();
                            if (Int32.TryParse(item.Text.Trim(), out id))
                            {
                                int countObjects = mapObj.SearchCount(id);
                                if (countObjects == 0)
                                {
                                    isDeleted = currentMap.Delete(id, true) && isDeleted;
                                    if (!isDeleted)
                                        break;
                                }
                                else
                                {
                                    bool objectDeleted = false;
                                    bool trans = mapObj.BeginTransaction();
                                    if (trans)
                                    {
                                        objectDeleted = mapObj.Remove(id, false);

                                        if (objectDeleted)
                                        {
                                            currentMap.SetTransaction(mapObj.GetTransaction());
                                            isDeleted = currentMap.Delete(id, false);
                                        }
                                    }
                                    if (isDeleted)
                                    {
                                        mapObj.CommitTransaction();
                                    }
                                    else
                                    {
                                        mapObj.RollbackTransaction();
                                    }
                                }

                            }
                            else
                            {
                                isDeleted = false;
                                break;
                            }
                        }

                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("mapsDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("mapsNotDeleted", culture));
                        }

                        btnSearch.PerformClick();
                        populateParentMapCombo();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noSelMapsDel", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvMaps.SelectedItems.Count > 0)
                {
                    MapAdd ma = new MapAdd(currentMap);
                    ma.ShowDialog(this);
                    if (ma.reloadOnReturn)
                    {
                        currentMapList = currentMap.Search(-1, -1, "", "");
                        populateListView(currentMapList);
                        populateParentMapCombo();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealTypeUpd", culture));
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvMaps_ColumnClick(object sender, ColumnClickEventArgs e)
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

                currentMapList.Sort(new ArrayListSort(sortOrder, sortField));
                
                populateListView(currentMapList);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.lvMaps_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvMaps_ColumnClick():" + ex.Message);
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

        private class ArrayListSort : IComparer
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(object x, object y)
            {
                Map map1 = null;
                Map map2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    map1 = (Map)x;
                    map2 = (Map)y;
                }
                else
                {
                    map1 = (Map)y;
                    map2 = (Map)x;
                }

                switch (compField)
                {
                    case Maps.mapIDIndex:
                        return map1.MapID.CompareTo(map2.MapID);
                    case Maps.nameIndex:
                        return map1.Name.CompareTo(map2.Name);
                    case Maps.descriptionIndex:
                        return map1.Description.CompareTo(map2.Description);
                    case Maps.parentMapIndex:
                        return map1.Name.CompareTo(map2.Name);                   
                    default:
                        return map1.Name.CompareTo(map2.Name);
                }
            }
        }

        #endregion

        private void Maps_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool adminRole = false;
                foreach (ApplRoleTO applRole in userRoles)
                {
                    if (applRole.ApplRoleID == Constants.ADMINRoleID)
                    {
                        adminRole = true;
                        break;
                    }

                }
                if (!adminRole)
                {
                    MessageBox.Show(rm.GetString("roleNotADMIN", culture));
                    this.Close();
                }

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Maps.Maps_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Maps_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + "Maps.Maps_KeyUp(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Maps.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " Maps.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
       
    }
}