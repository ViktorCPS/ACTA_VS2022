using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using SiemensDataAccess;
using System.Collections;
using System.Resources;
using Common;
using System.Globalization;
using TransferObjects;
using UI;
using System.IO;


namespace SiemensUI
{
    public partial class SiemensMapping : Form
    {
        DebugLog log;
        ResourceManager rm;

        const int START_Y_POSITION = 5;
        const int X_POSITION = 15;
        const int SPACE_Y_POSITION = 115;
        
        private CultureInfo culture;
        SiemensDAO siemens;
        
        //ponts list types
        ArrayList oldMappedPoints = new ArrayList();
        ArrayList TAPoints = new ArrayList();
        ArrayList NotTAPoints = new ArrayList();
        ArrayList newBasePoints = new ArrayList();

        const int PointTypeDEL = 0;
        const int PointTypeTA = 1;
        const int PointTypeNOTTA = 2;
        const int PointTypeNEW = 3;

        List<SiemensDevicesControl> controls = new List<SiemensDevicesControl>();
        List<SiemensDevicesControl> comboControls = new List<SiemensDevicesControl>();

        public SiemensMapping()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);


            rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SiemensMapping_Load(object sender, EventArgs e)
        {
            try
            {
                this.CenterToScreen();

                if (!File.Exists(Constants.SiPassConnPath))
                {
                    MessageBox.Show(rm.GetString("noConnString", culture));
                    this.Close();
                    return;
                }
                siemens = SiemensDAO.getDAO();               
                                    
                if (siemens != null)
                {
                    ArrayList oldPoints = siemens.deserializeMapping(Constants.SiPassMappingPath);

                    //Natasa 02.09.2009
                    //from now return all points from dataBase

                    //string pointsString = "";

                    //foreach (PointTO point in oldPoints)
                    //{
                    //    if (pointsString.Equals(""))
                    //    {
                    //        pointsString += " '" + point.PointName + "' ";
                    //    }
                    //    else
                    //    {
                    //        pointsString += ", '" + point.PointName + "' ";                     
                    //    }
                    //}

                    ArrayList allPoints = siemens.getNewPoints();
                    Hashtable pointsTable = new Hashtable();
                    foreach (PointTO pt in allPoints)
                    {
                        if (!pointsTable.ContainsKey(pt.PointID))
                        {
                            pointsTable.Add(pt.PointID, pt);
                        }
                    }

                    setDevicesControls(oldPoints, pointsTable);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noConnection", culture));
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.SiemensMapping_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setDevicesControls(ArrayList oldPoints, Hashtable allBasePoints)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int position = START_Y_POSITION;
                SiemensDevicesControl deviceControl = new SiemensDevicesControl();
                foreach (PointTO point in oldPoints)
                {
                    if (allBasePoints.ContainsKey(point.PointID))
                    {
                        point.PointName = ((PointTO)allBasePoints[point.PointID]).PointName;

                        if (point.ReadPasses == Constants.SiemensPointPassReading)
                            TAPoints.Add(point);
                        else
                            NotTAPoints.Add(point);
                        allBasePoints.Remove(point.PointID);
                    }
                    else
                    {
                        oldMappedPoints.Add(point);
                    }

                }
                foreach (PointTO point in allBasePoints.Values)
                {
                    newBasePoints.Add(point);
                }

                oldMappedPoints.Sort(new ArrayListSort());
                TAPoints.Sort(new ArrayListSort());
                NotTAPoints.Sort(new ArrayListSort());
                newBasePoints.Sort(new ArrayListSort());

                lblDeletedPoint.Text = lblDeletedPoint.Text +" ("+ oldMappedPoints.Count.ToString()+")";
                lblTAPoints.Text += " (" +TAPoints.Count.ToString()+")";
                lblNotTAPoint.Text += " (" +NotTAPoints.Count.ToString()+")";
                lblNewPoint.Text += " ("+newBasePoints.Count.ToString()+")";

               
                foreach (PointTO point in TAPoints)
                {
                    deviceControl = new SiemensDevicesControl(point);
                    deviceControl.Tag = PointTypeTA;
                    deviceControl.BackColor = Color.FromArgb(100, 144, 238, 144);
                    deviceControl.Location = new Point(X_POSITION, position);
                    this.panelDevices.Controls.Add(deviceControl);
                    position += SPACE_Y_POSITION;
                    controls.Add(deviceControl);
                }
                foreach (PointTO point in NotTAPoints)
                {
                    deviceControl = new SiemensDevicesControl(point);
                    deviceControl.Tag = PointTypeNOTTA;
                    deviceControl.BackColor = Color.LightBlue;
                    deviceControl.Location = new Point(X_POSITION, position);
                    this.panelDevices.Controls.Add(deviceControl);
                    position += SPACE_Y_POSITION;
                    controls.Add(deviceControl);
                }
                foreach (PointTO point in oldMappedPoints)
                {
                    deviceControl = new SiemensDevicesControl(point);
                    deviceControl.Enabled = false;
                    deviceControl.Tag = PointTypeDEL;
                    deviceControl.Location = new Point(X_POSITION, position);
                    this.panelDevices.Controls.Add(deviceControl);
                    position += SPACE_Y_POSITION;
                    controls.Add(deviceControl);
                }
                foreach (PointTO point in newBasePoints)
                {
                    deviceControl = new SiemensDevicesControl(point);
                    deviceControl.Tag = PointTypeNEW;
                    deviceControl.BackColor = Color.LightPink;
                    deviceControl.Location = new Point(X_POSITION, position);
                    this.panelDevices.Controls.Add(deviceControl);
                    position += SPACE_Y_POSITION;
                    controls.Add(deviceControl);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.setDevicesControls(): " + ex.Message + "\n");
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

        private class ArrayListSort : IComparer
        {
            
            public ArrayListSort()
            {
               
            }

            public int Compare(object x, object y)
            {
                PointTO point1 = null;
                PointTO point2 = null;

                point1 = (PointTO)x;
                point2 = (PointTO)y;

                return point1.PointName.CompareTo(point2.PointName);
            }
        }

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (controls.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noControls", culture));
                }
                else
                {
                    ArrayList pointList = new ArrayList();
                    //Reader r = new Reader();
                    //List<ReaderTO> readers = r.SearchAll();

                    ////dictionary of old readers inserted into ACTA db
                    //Dictionary<int,ReaderTO> readersACTA = new Dictionary<int,ReaderTO>();
                    //foreach(ReaderTO reader in readers)
                    //{
                    //    if(!readersACTA.ContainsKey(reader.ReaderID))
                    //        readersACTA.Add(reader.ReaderID,reader);
                    //}

                    //get all points from Asco db 
                    foreach (SiemensDevicesControl control in controls)
                    {
                        if (control.Enabled)
                        {
                            pointList.Add(control.LocalPoint);                            
                        }
                    }

                    ////dictionary of all asco points of interes
                    //Dictionary<int, PointTO> readersAsco = new Dictionary<int, PointTO>();
                    //foreach (PointTO p in pointList)
                    //{
                    //    if (p.ReadPasses == Constants.SiemensPointPassReading &&
                    //        !readersAsco.ContainsKey(p.PointID))
                    //    {
                    //        readersAsco.Add(p.PointID, p);
                    //    }
                    //}
                    try
                    {
                        if (File.Exists(Constants.SiPassMappingPath))
                            File.Copy(Constants.SiPassMappingPath, Constants.SiPassMappingOLDPath, true);
                    }
                    catch (Exception ex)
                    {
                        log.writeLog(DateTime.Now + " SiemensMapping.btnSave_Click() mapping.xml file manipulation exception: " + ex.Message);
                        MessageBox.Show(rm.GetString("pointNOTSerialized", culture));
                        return;
                    }
                    //write points data to mapping.xml we need it to keep pass type for every reader
                    bool serialized = siemens.serialize(pointList, Constants.SiPassMappingPath);
                    //if (serialized)
                    //{
                    //    //begin transaction for reader update and insert
                    //    bool trans = r.BeginTransaction();
                    //    if (trans)
                    //    {
                    //        try
                    //        {
                    //            //set reader status to DISEABLE if there is no point in dictionary with the same id
                    //            foreach (int readerID in readersACTA.Keys)
                    //            {
                    //                if (!readersAsco.ContainsKey(readerID))
                    //                {
                    //                    ReaderTO read = readersACTA[readerID];
                    //                    read.Status = Constants.readerStatusDisabled;
                    //                    serialized = serialized && r.Update(read, false);

                    //                }
                    //                else
                    //                {
                    //                    ReaderTO ACTAreader = readersACTA[readerID];
                    //                    PointTO AscoReader = (PointTO)readersAsco[readerID];
                    //                    if (!ACTAreader.Description.Equals(AscoReader.PointName)
                    //                        || ACTAreader.A0IsCounter != AscoReader.TimeAttCounter
                    //                        || !ACTAreader.A0Direction.Equals(AscoReader.Direction)
                    //                        || ACTAreader.Status.Equals(Constants.readerStatusDisabled))
                    //                    {
                    //                        string direction = "";
                    //                        if (AscoReader.Direction == Constants.SiemensDirectionIn)
                    //                            direction = Constants.DirectionIn;
                    //                        else
                    //                            direction = Constants.DirectionOut;
                    //                        ACTAreader.Description = AscoReader.PointName;
                    //                        ACTAreader.A0IsCounter = AscoReader.TimeAttCounter;
                    //                        ACTAreader.A0Direction = direction;
                    //                        ACTAreader.A1IsCounter = AscoReader.TimeAttCounter;
                    //                        ACTAreader.A1Direction = direction;
                    //                        ACTAreader.A0LocID = Constants.defaultLocID;
                    //                        ACTAreader.A1LocID = Constants.defaultLocID;
                    //                        ACTAreader.Status = Constants.readerStatusEnabled;

                    //                        serialized = serialized && r.Update(ACTAreader, false);
                    //                    }
                    //                }
                    //            }
                    //            if (serialized)
                    //            {
                    //                Gate g = new Gate();
                    //                g.SetTransaction(r.GetTransaction());
                    //                foreach (int readerID in readersAsco.Keys)
                    //                {
                    //                    if (!readersACTA.ContainsKey(readerID))
                    //                    {
                    //                        PointTO AscoReader = (PointTO)readersAsco[readerID];
                    //                        ReaderTO ACTAreader = new ReaderTO();

                    //                        g.GTO.Description = AscoReader.PointName;
                    //                        g.GTO.Name = AscoReader.PointName;
                    //                        int gateID = g.Save(g.GTO.Name, g.GTO.Description, new DateTime(), -1, -1, false);
                    //                        serialized = serialized && (gateID > 0);

                    //                        string direction = "";
                    //                        if (AscoReader.Direction == Constants.SiemensDirectionIn)
                    //                            direction = Constants.DirectionIn;
                    //                        else
                    //                            direction = Constants.DirectionOut;

                    //                        ACTAreader.Description = AscoReader.PointName;
                    //                        ACTAreader.A0IsCounter = AscoReader.TimeAttCounter;
                    //                        ACTAreader.A0Direction = direction;
                    //                        ACTAreader.A1IsCounter = AscoReader.TimeAttCounter;
                    //                        ACTAreader.A1Direction = direction;
                    //                        ACTAreader.A0LocID = Constants.defaultLocID;
                    //                        ACTAreader.A1LocID = Constants.defaultLocID;
                    //                        ACTAreader.A0GateID = gateID;
                    //                        ACTAreader.A1GateID = gateID;
                    //                        ACTAreader.ReaderID = AscoReader.PointID;
                    //                        ACTAreader.Status = Constants.readerStatusEnabled;
                    //                        r.RdrTO = ACTAreader;
                    //                        serialized = serialized && (r.Save(false) == 1);
                    //                    }
                    //                }
                    //                if (serialized)
                    //                {
                    //                    r.CommitTransaction();
                    //                }
                    //                else
                    //                {
                    //                    r.RollbackTransaction();

                    //                }

                    //            }
                    //            else
                    //            {
                    //                r.RollbackTransaction();
                    //            }
                        //    }
                        //    catch(Exception ex)
                        //    {
                        //        r.RollbackTransaction();
                        //        serialized = false;
                        //        log.writeLog(DateTime.Now + " SiemensMapping.btnSave_Click() mapping.xml file manipulation exception: " + ex.Message);                            
                        //    }
                        //}
                        //else
                        //{
                        //    serialized = false;
                            
                        //}
                    //}
                    //set to diseable 
                    if (serialized)
                    {
                        MessageBox.Show(rm.GetString("pointSerialized", culture));
                        try
                        {
                            if (File.Exists(Constants.SiPassMappingOLDPath))
                            {
                                File.Delete(Constants.SiPassMappingOLDPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.writeLog(DateTime.Now + " SiemensMapping.btnSave_Click() mapping.xml file manipulation exception: " + ex.Message);
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("pointNOTSerialized", culture));
                        try
                        { 
                             if (File.Exists(Constants.SiPassMappingOLDPath))
                            {
                                File.Copy(Constants.SiPassMappingOLDPath, Constants.SiPassMappingPath, true);
                                File.Delete(Constants.SiPassMappingOLDPath);
                            }
                        }
                        catch(Exception ex)
                        {
                            log.writeLog(DateTime.Now + " SiemensMapping.btnSave_Click() mapping.xml file manipulation exception: " + ex.Message);                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTAPoints_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (SiemensDevicesControl control in controls)
                {
                    int pointType = (int)control.Tag;
                    if (pointType == PointTypeTA)
                    {
                        //control.Select();
                        panelDevices.AutoScrollPosition = new Point(0, Math.Abs(panelDevices.AutoScrollPosition.Y - control.Location.Y));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.btnTAPoints_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNotTAPoint_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (SiemensDevicesControl control in controls)
                {
                    int pointType = (int)control.Tag;
                    if (pointType == PointTypeNOTTA)
                    {
                        //control.Select();
                        panelDevices.AutoScrollPosition = new Point(0, Math.Abs(panelDevices.AutoScrollPosition.Y - control.Location.Y));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.btnNotTAPoint_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeletedPoint_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (SiemensDevicesControl control in controls)
                {
                    int pointType = (int)control.Tag;
                    if (pointType == PointTypeDEL)
                    {
                        //control.Select();
                        panelDevices.AutoScrollPosition = new Point(0, Math.Abs(panelDevices.AutoScrollPosition.Y - control.Location.Y));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.btnDeletedPoint_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNewPoint_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (SiemensDevicesControl control in controls)
                {
                    int pointType = (int)control.Tag;
                    if (pointType == PointTypeNEW)
                    {
                        //control.Select();
                        panelDevices.AutoScrollPosition = new Point(0, Math.Abs(panelDevices.AutoScrollPosition.Y - control.Location.Y));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.btnNewPoint_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                comboControls = new List<SiemensDevicesControl>();
                if (tbPointName.Text.ToString().Trim().Equals(""))
                {
                    MessageBox.Show("You must type at least one letter.");
                    return;
                }
                else
                {
                    ArrayList points = new ArrayList();
                    foreach (SiemensDevicesControl control in controls)
                    {
                        string searchString = tbPointName.Text.ToString().Trim().ToLower().Replace(" ","");
                        string name = control.LocalPoint.PointName.Trim().ToLower().Replace(" ", "");
                        if (name.Contains(searchString))
                        {
                            points.Add(control.LocalPoint);
                            comboControls.Add(control);
                        }
                    }
                    populatePointNameCombo(points);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void populatePointNameCombo(ArrayList array)
        {
            try
            {               
                cbPoints.DataSource = array;
                cbPoints.DisplayMember = "PointName";
                cbPoints.ValueMember = "PointID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.populatePointNameCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void cbPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
                if(cbPoints.SelectedIndex>=0)
                {
                    foreach (SiemensDevicesControl control in comboControls)
                    {
                        int pID = -100;
                        try
                        {
                            pID = (int)cbPoints.SelectedValue;
                        }
                        catch { }
                        if (control.LocalPoint.PointID==pID)
                        {
                            //control.Focus();
                            panelDevices.AutoScrollPosition =new Point(0,Math.Abs(panelDevices.AutoScrollPosition.Y-control.Location.Y));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensMapping.cbPoints_SelectedIndexChanged(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}