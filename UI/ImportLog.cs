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
using System.IO;
using System.Xml.Serialization;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    public partial class ImportLog : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        private static string SUFFIXFORMAT = "yyyyMMddHHmmss";
        private static string FILEEXT = ".xml";

        int readerID = -1;

        public ImportLog()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(ImportLog).Assembly);

                setLanguage();
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
                this.Text = "Log -> XML";

                lblSrcFile.Text = rm.GetString("lblSrcFile", culture);
                lblReader.Text = rm.GetString("lblReader", culture);

                btnBrowse.Text = rm.GetString("btnBrowse", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnToXml.Text = rm.GetString("btnToXML", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ImportLog.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ImportLog.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ImportLog_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                populateReaderCombo();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ImportLog.ImportLog_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateReaderCombo()
        {
            try
            {
                List<ReaderTO> readers = new Reader().SearchAll();
                ReaderTO rTO = new ReaderTO();
                rTO.Description = rm.GetString("all", culture);
                readers.Insert(0, rTO);

                cbReader.DataSource = readers;
                cbReader.DisplayMember = "Description";
                cbReader.ValueMember = "ReaderID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ImportLog.populateReaderCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                OpenFileDialog dialog = new OpenFileDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tbSrcFilePath.Text = dialog.FileName;
                }

                dialog.Dispose();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ImportLog.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnToXml_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (!tbSrcFilePath.Text.Equals(""))
                {
                    if ((cbReader.SelectedIndex < 0 && !Int32.TryParse(cbReader.Text.Trim(), out readerID)) 
                        || (cbReader.SelectedIndex == 0 && (cbReader.Text.Equals(rm.GetString("all", culture)))))
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("readerNotSelected", culture));
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    string srcFilePath = tbSrcFilePath.Text.Trim();
                    if (cbReader.SelectedIndex > 0)
                    {
                        readerID = (int)cbReader.SelectedValue;
                    }
                    else
                    {
                        readerID = Int32.Parse(cbReader.Text.Trim());
                    }
                    LogTO logTO = new LogTO();
                    ArrayList LogTOList = new ArrayList();

                    string[] lines = File.ReadAllLines(srcFilePath);

                    foreach (string line in lines)
                    {
                        logTO = createLogFromLine(line);
                        if (logTO.TagID != 0 || logTO.Antenna != -1 || logTO.EventHappened != -1 || logTO.ActionCommited != -1 
                             || logTO.ReaderID != -1 || logTO.PassGenUsed != -1 || !logTO.EventTime.Equals(new DateTime()))
                        {
                            logTO.Button = logTO.Antenna;
                            LogTOList.Add(logTO);
                        }
                    }

                    if (LogTOList.Count > 0)
                    {
                        string filePath = Constants.unprocessed
                                + Constants.ReaderXMLLogFile
                                + "_" + readerID.ToString().Trim()
                                + "_" + DateTime.Now.ToString(SUFFIXFORMAT)
                                + FILEEXT;
                        Stream stream = File.Open(filePath, FileMode.Create);
                        LogTO[] logTOArray = (LogTO[])LogTOList.ToArray(typeof(LogTO));

                        XmlSerializer bformatter = new XmlSerializer(typeof(LogTO[]));
                        bformatter.Serialize(stream, logTOArray);
                        stream.Close();
                    }
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show("XML fajl uspešno kreiran i nalazi se u L\\unprocessed\\","INFO",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(rm.GetString("srcFileNotSelected", culture));
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ImportLog.btnTxt2Xml_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private LogTO createLogFromLine(string line)
        {
            try
            {
                LogTO logTO = new LogTO();

                logTO.ReaderID = readerID;
                logTO.PassGenUsed = (int)Constants.PassGenUsed.Unused;

                uint tagID = 0;
                int antenna = -1;
                int eventHappened = -1;
                int actionCommited = -1;
                DateTime eventTime = new DateTime();

                int index = line.IndexOf("\t");
                if (index >= 0)
                {
                    uint.TryParse(line.Substring(0, index), out tagID);
                    line = line.Substring(index + 1);

                    index = line.IndexOf("\t");
                    if (index >= 0)
                    {
                        if (!int.TryParse(line.Substring(0, index), out antenna))
                        {
                            antenna = -1;
                        }
                        line = line.Substring(index + 1);

                        index = line.IndexOf("\t");
                        if (index >= 0)
                        {
                            if (!int.TryParse(line.Substring(0, index), out eventHappened))
                            {
                                eventHappened = -1;
                            }
                            line = line.Substring(index + 1);

                            index = line.IndexOf("\t");
                            if (index >= 0)
                            {
                                if (!int.TryParse(line.Substring(0, index), out actionCommited))
                                {
                                    actionCommited = -1;
                                }

                                line = line.Substring(index + 1);

                                if (!DateTime.TryParse(line, out eventTime))
                                {
                                    eventTime = new DateTime();
                                }
                            }
                        }
                    }
                }

                if (tagID != 0)
                    logTO.TagID = tagID;
                if (tagID != 0 && (antenna < 0 || antenna > 1))
                {
                    return new LogTO();
                }
                else
                {
                    logTO.Antenna = antenna;
                }
                if (tagID != 0 && (eventHappened < 1 || eventHappened > 4))
                {
                    return new LogTO();
                }
                else
                {
                    logTO.EventHappened = eventHappened;
                }
                if (actionCommited != -1)
                {
                    logTO.ActionCommited = actionCommited;
                }
                if (eventTime.Equals(new DateTime()) || eventTime.Year > DateTime.Now.Year)
                {
                    return new LogTO();
                }
                else
                {
                    logTO.EventTime = eventTime;
                }

                return logTO;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ImportLog.createLogFromLine(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}