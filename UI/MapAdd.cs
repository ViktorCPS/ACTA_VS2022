using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;


namespace UI
{
    public partial class MapAdd : Form
    {
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        DebugLog log;

        Map currentMap;
        byte[] imgbyte;
        public bool reloadOnReturn;

        public MapAdd()
        {
            InitializeComponent();           
            currentMap = new Map();           
            this.btnUpdate.Visible = false;
            int mapID = currentMap.FindMAXMapID();
            mapID++;
            tbMapID.Text = mapID.ToString();
			
        }
        public MapAdd(Map map)
        {
            InitializeComponent();            
            currentMap = map;           
            this.btnSave.Visible = false;
            tbMapID.Text = map.MapID.ToString();
            tbName.Text = map.Name;
            tbDescription.Text = map.Description;
            byte[] mapPhoto = currentMap.Content;
            if (currentMap.Content.Length > 0)
            {
                MemoryStream memStream = new MemoryStream(mapPhoto);
                Image img = new Bitmap(memStream);
                this.pbPicture.Image = img;
                memStream.Close();
            }
            lblStar1.Visible = false;
            
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                if (currentMap.MapID >= 0)
                {
                    this.Text = rm.GetString("updateMap", culture);
                }
                else
                {
                    this.Text = rm.GetString("addMap", culture);
                }
                
                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnBrowse.Text = rm.GetString("btnBrowse", culture);
                
                // label's text
                lblName.Text = rm.GetString("lblName", culture);
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblPisture.Text = rm.GetString("lblPictureMap", culture);
                lblMapID.Text = rm.GetString("lblMapID", culture);
                
                //group box's text
                gbMap.Text = rm.GetString("gbMap", culture);
                               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        
      
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = rm.GetString("browsePhoto", culture);
                dlg.Filter = "bmp files (*.bmp)|*.bmp"
                    + "|gif files (*.gif)|*.gif"
                    + "|jpg files (*.jpg)|*.jpg"
                    + "|jpeg files (*.jpeg)|*.jpeg";
                dlg.FilterIndex = 3;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fiPicture = new FileInfo(dlg.FileName);
                    FileStream FilStr = new FileStream(fiPicture.FullName, FileMode.Open);
                    if (fiPicture.Length > 1043576)
                    {
                        MessageBox.Show(rm.GetString("imgLarge", culture));
                    }
                    else
                    {
                        this.tbPicture.Text = dlg.FileName;

                        BinaryReader BinRed = new BinaryReader(FilStr);

                        imgbyte = new byte[FilStr.Length + 1];

                        imgbyte = BinRed.ReadBytes(Convert.ToInt32(BinRed.BaseStream.Length));

                        BinRed.Close();
                        FilStr.Close();
                        MemoryStream memStream = new MemoryStream(imgbyte);
                        Image img = new Bitmap(memStream);



                        if (pbPicture.Image != null)
                            pbPicture.Image.Dispose();

                        pbPicture.Image = img;

                        dlg.Dispose();

                    }
                }

                dlg.Dispose();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapAdd.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.tbMapID.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageMapIDNotSet", culture));
                }
                if (this.tbName.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageMapNameNotSet", culture));
                }
                if (this.tbPicture.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageNoPictureSelected", culture));
                }

                try
                {
                    if (!tbMapID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbMapID.Text.Trim());
                    }
                }
                catch
                {
                    throw new Exception(rm.GetString("messageMapIDMustBeNum", culture));
                }
                currentMap.MapID = Int32.Parse(tbMapID.Text.Trim());
                currentMap.Name = tbName.Text.Trim();
                currentMap.Description = tbDescription.Text.Trim();

                bool saved = currentMap.Save(int.Parse(tbMapID.Text.ToString()), tbName.Text.ToString(), tbDescription.Text.ToString(), imgbyte);
                if (saved)
                {
                    reloadOnReturn = true;
                    DialogResult result = MessageBox.Show(rm.GetString("messageMapInserted", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        this.tbMapID.Text = "";
                        this.tbName.Text = "";
                        this.tbDescription.Text = "";
                        this.tbPicture.Text = "";
                        this.pbPicture.Image = null;
                        this.tbMapID.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }

            }

            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    MessageBox.Show(rm.GetString("messageMapIDExists", culture));
                }
                else
                {
                    MessageBox.Show(sqlex.Message);
                    log.writeLog(DateTime.Now + " MapAdd.btnSave_Click(): " + sqlex.Message + "\n");
                }
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    MessageBox.Show(rm.GetString("messageMapIDExists", culture));
                }
                else
                {
                    MessageBox.Show(mysqlex.Message);
                    log.writeLog(DateTime.Now + " MapAdd.btnSave_Click(): " + mysqlex.Message + "\n");
                }
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapAdd.btnBrowse_Click(): " + ex.Message + "\n");
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

                bool updated = false;

                // validate
                if (this.tbMapID.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageMapIDNotSet", culture));
                }
                if (this.tbName.Text.Trim().Equals(""))
                {
                    throw new Exception(rm.GetString("messageMapNameNotSet", culture));
                }

                try
                {
                    if (!tbMapID.Text.Trim().Equals(""))
                    {
                        Int32.Parse(tbMapID.Text.Trim());
                    }
                }
                catch
                {
                    throw new Exception(rm.GetString("messageMapIDMustBeNum", culture));
                }
                currentMap.MapID = Int32.Parse(tbMapID.Text.Trim());
                currentMap.Name = tbName.Text.Trim();
                currentMap.Description = tbDescription.Text.Trim();
                if (imgbyte != null)
                {
                    currentMap.Content = imgbyte;
                }

                updated = currentMap.Update(currentMap.MapID, currentMap.MapID, currentMap.Name, currentMap.Description, currentMap.Content);

                if (updated)
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("mapUpdated", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("mapNotUpdated", culture));
                }

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapsAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void MapAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(LocationsAdd).Assembly);

                setLanguage();
                reloadOnReturn = false;
                this.pbPicture.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapsAdd.MapAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void MapAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + "MapAdd.MapAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}