using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.IO;

using TransferObjects;
using Util;
using Common;

namespace UI
{
    public partial class UnregularDataPreview : Form
    {
        private const string delimiter = "\t";

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        Dictionary<int, List<string>> unregularData = new Dictionary<int, List<string>>();
        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();

        public UnregularDataPreview(Dictionary<int, List<string>> unregularData, Dictionary<int, EmployeeTO> emplDict)
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(UnregularDataPreview).Assembly);
                
                setLanguage();

                this.unregularData = unregularData;
                this.emplDict = emplDict;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("UnregularDataPreview", culture);
                
                //button's text
                this.btnClose.Text = rm.GetString("btnOK", culture);
                this.btnExportPreview.Text = rm.GetString("btnExportPreview", culture);                
                
                // list view
                lvPreview.BeginUpdate();
                lvPreview.Columns.Add(rm.GetString("hdrEmplID", culture), 100, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrName", culture), 150, HorizontalAlignment.Left);
                lvPreview.Columns.Add(rm.GetString("hdrRemark", culture), 345, HorizontalAlignment.Left);
                lvPreview.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " UnregularDataPreview.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UnregularDataPreview_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                populatePreview();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " UnregularDataPreview.UnregularDataPreview_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void populatePreview()
        {
            try
            {                
                lvPreview.BeginUpdate();
                lvPreview.Items.Clear();

                foreach (int id in unregularData.Keys)
                {
                    ListViewItem item = new ListViewItem();
                    
                    item.Text = id.ToString().Trim();
                    if (emplDict.ContainsKey(id))
                        item.SubItems.Add(emplDict[id].FirstAndLastName.Trim());
                    else
                        item.SubItems.Add("");

                    string remark = "";
                    foreach (string msg in unregularData[id])
                    {
                        remark += msg.Trim() + " ";
                    }
                    item.SubItems.Add(remark.Trim());
                    
                    lvPreview.Items.Add(item);
                }

                lvPreview.EndUpdate();
                lvPreview.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " UnregularDataPreview.populatePreview(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnExportPreview_Click(object sender, EventArgs e)
        {
            SaveFileDialog fbDialog = new SaveFileDialog();

            try
            {
                this.Cursor = Cursors.WaitCursor;

                fbDialog.DefaultExt = ".txt";
                fbDialog.Filter = "Text files (*.txt)|*.txt";
                fbDialog.Title = "Save file";
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string filePath = fbDialog.FileName;
                    fbDialog.Dispose();

                    FileStream stream = new FileStream(filePath, FileMode.Append);
                    stream.Close();

                    // insert header                    
                    string header = rm.GetString("hdrEmplID", culture) + delimiter + rm.GetString("hdrName", culture) + delimiter + rm.GetString("hdrRemark", culture);

                    StreamWriter writer = File.AppendText(filePath);
                    writer.WriteLine(header);

                    foreach (int id in unregularData.Keys)
                    {
                        string line = id.ToString().Trim();
                        if (emplDict.ContainsKey(id))
                            line += delimiter + emplDict[id].FirstAndLastName.Trim();
                        else
                            line += delimiter + " ";

                        string remark = "";
                        foreach (string msg in unregularData[id])
                        {
                            remark += msg.Trim() + " ";
                        }
                        
                        line += delimiter + remark.Trim();

                        writer.WriteLine(line);
                    }

                    writer.Close();

                    MessageBox.Show(rm.GetString("FileGenerated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " UnregularDataPreview.btnGenerateFile_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
