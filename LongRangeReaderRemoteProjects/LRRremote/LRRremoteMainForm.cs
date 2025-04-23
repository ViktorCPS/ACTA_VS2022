using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Configuration;
using System.Windows.Forms;

using Common;
using TransferObjects;

namespace LRRremote
{
    public partial class LRRremoteMainForm : Form
    {
        byte[] InventoryCmd = { 0x01, 0x01, 0x02, 0x00, 0x07, 0x00 };
        byte[] ConfigureLanesCmd = { 0x01, 0x41, 0x08, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
        byte[] SwitchLane1Cmd = { 0x01, 0x40, 0x08, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
        byte[] SwitchLane2Cmd = { 0x01, 0x40, 0x08, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
        byte[] StopRF = { 0x01, 0x96, 0x02, 0x00, 0x00, 0x00 };
        byte[] SetRelay = { 0x01, 0xA5, 0x01, 0x00, 0x04 };
        byte[] SetOutput1 = { 0x01, 0xA5, 0x01, 0x00, 0x01 };
        byte[] SetOutput2 = { 0x01, 0xA5, 0x01, 0x00, 0x02 };
        byte[] ResetOutputs = { 0x01, 0xA5, 0x01, 0x00, 0x00 };

        LRRinterface sp = null; int noPasses = 0; int archiveCounter = 0; int lastArchived = 0; const int ARCHIVE_PASSES = 1500;
        int maintenanceCounter = 0; const int MAINTENANCE_PASSES = 6000; int lastPairs = 0;
        TagsTimes DebouncedTagsLane1 = new TagsTimes(); TagsTimes DebouncedTagsLane2 = new TagsTimes();
        int lane1TimeMs = 0; int lane2TimeMs = 0; int laneTimeIncrementMs = 0; int betweenLanesTimeIncrementMs = 0;
        int debounceInterval = 0; int pairInterval = 0; string pairTime = String.Empty; int previousDay = 0;
        Dictionary<string, EmployeeTO> ICodeData = null;
        string answer = String.Empty;

        private Thread readingThread = null;
        private bool doRead = false;
        private bool resetReading = false;

        public LRRremoteMainForm()
        {
            InitializeComponent();
        }

        private void LRRremoteMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                resetTimer.Enabled = true;
                CreateConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error establishing connection: " + ex.Message, "Info");
            }
        }

        private void LRRremoteMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                doRead = false; Thread.Sleep(1000);
                if (readingThread != null) readingThread = null;
                if (sp != null) sp.Close();
            }
            catch { }
        }

        void CreateConnection()
        {
            try
            {
                string LRRaddress = ConfigurationManager.AppSettings["LRRaddress"]; this.Invoke(new utbT(update_Title), new object[] { LRRaddress });
                if (!(LRRaddress.StartsWith("COM"))) sp = new IPInterface(LRRaddress); else sp = new SerialInterface(LRRaddress, 115200); sp.Open();
            }
            catch (Exception ex) { throw ex; }
        }

        void DestroyConnection()
        {
            try { if (sp != null) sp.Close(); } catch { }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!doRead) { btnStart.Text = "Stop"; startReading(); }
                else { btnStart.Text = "Start"; stopReading(); }
            }
            catch (Exception ex) { MessageBox.Show("Error starting reading thread: " + ex.Message, "Info"); }
        }

        void startReading()
        {
            ICodeData = Misc.GetICodeData();
            lane1TimeMs = lane2TimeMs = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            previousDay = DateTime.Now.Day;
            debounceInterval = (int)nudDebounceInterval.Value;
            pairInterval = (int)nudPairInterval.Value;
            answer = RequestAnswer(ConfigureLanesCmd, sp); if (answer.StartsWith("ERR")) update_tbPairs(answer + Environment.NewLine);
            doRead = true;
            ThreadStart starter = new ThreadStart(Reading);
            readingThread = new Thread(starter);
            readingThread.Start();
        }

        void stopReading()
        {
            doRead = false; Thread.Sleep(1000);
            if (readingThread != null) readingThread = null;
        }

        private void rLRR()
        {
            try
            {
                lvReadResults.Invoke(new alvW(archive_Windows));
                tbPairs.Invoke(new utbD(update_tbPairs), new object[] { "LRR reset " + DateTime.Now.ToLongTimeString() });
                resetReading = true;
            }
            catch { }
        }

        private void resetTimer_Tick(object sender, EventArgs e)
        {
            if (resetReading) { resetReading = false; stopReading(); Thread.Sleep(1000); startReading();  }
        }

        private void update_Title(string text) { this.Text = "LRRremote " + text; }
        public delegate void utbT(string text);

        private void update_tbPairs(string line) { tbPairs.AppendText(line + Environment.NewLine); }
        public delegate void utbD(string line);

        private void scroll_tbPairs() { tbPairs.SelectionStart = tbPairs.Text.Length; tbPairs.ScrollToCaret(); }
        public delegate void stbD();

        private void update_lvReadResults(ListViewItem lvi) {lvReadResults.Items.Add(lvi); lvReadResults.Items[lvReadResults.Items.Count - 1].EnsureVisible(); }
        public delegate void ulvD(ListViewItem lvi);

        private void archive_Windows()
        {
            try
            {
                File.AppendAllText("lvReadResults.txt", DateTime.Now.ToString() + Environment.NewLine);
                for (int i = lastArchived; i < lvReadResults.Items.Count; i++)
                {
                    foreach (ListViewItem.ListViewSubItem subItem in lvReadResults.Items[i].SubItems) File.AppendAllText("lvReadResults.txt", subItem.Text + "\t");
                    File.AppendAllText("lvReadResults.txt", Environment.NewLine);
                }
                lastArchived = lvReadResults.Items.Count;

                File.AppendAllText("tbPairs.txt", DateTime.Now.ToString() + Environment.NewLine);
                File.AppendAllText("tbPairs.txt", tbPairs.Text.Substring(lastPairs)); lastPairs = tbPairs.Text.Length;

                if (DateTime.Now.Day != previousDay)
                {
                    while (lvReadResults.Items.Count > 0) lvReadResults.Items.RemoveAt(0); lastArchived = 0;
                    tbPairs.Clear(); lastPairs = 0;
                    previousDay = DateTime.Now.Day;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public delegate void alvW();

        private void update_lblNoPasses(int noPasses) { lblNoPasses.Text = (noPasses + 1).ToString("D06"); }
        public delegate void ulblD(int noPasses);

        private void Reading()
        {
            while (doRead)
            {
                try
                {
                    archiveCounter++; if (archiveCounter >= ARCHIVE_PASSES) { archiveCounter = 0; lvReadResults.Invoke(new alvW(archive_Windows)); }

                    int lane = 0;

                    if ((noPasses % 2) == 0) { answer = RequestAnswer(SwitchLane1Cmd, sp); lane = 1; } else { answer = RequestAnswer(SwitchLane2Cmd, sp); lane = 2; }
                    if (answer.StartsWith("ERR")) { tbPairs.Invoke(new utbD(update_tbPairs), new object[] { answer }); rLRR(); break; }

                    answer = RequestAnswer(ResetOutputs, sp); if (answer.StartsWith("ERR")) { tbPairs.Invoke(new utbD(update_tbPairs), new object[] { answer }); rLRR(); break; }

                    string answerString = RequestAnswer(InventoryCmd, sp); if (answerString.StartsWith("ERR")) { tbPairs.Invoke(new utbD(update_tbPairs), new object[] { answer }); rLRR(); break; }

                    DateTime dateTimeNow = DateTime.Now; int ms = (int)(dateTimeNow.Ticks / TimeSpan.TicksPerMillisecond);
                    if (lane == 1) { laneTimeIncrementMs = ms - lane1TimeMs; betweenLanesTimeIncrementMs = ms - lane2TimeMs; lane1TimeMs = ms; }
                    else if (lane == 2) { laneTimeIncrementMs = ms - lane2TimeMs; betweenLanesTimeIncrementMs = ms - lane1TimeMs; lane2TimeMs = ms; }
                    if (noPasses < 2) laneTimeIncrementMs = betweenLanesTimeIncrementMs = 0;

                    int noTags = 1; string[] tagIDs = null;
                    if (answerString == String.Empty) { tagIDs = new string[1]; tagIDs[0] = "NO TAG          "; }
                    else
                    {
                        noTags = answerString.Length / 20; tagIDs = new string[noTags];
                        for (int j = 0; j < noTags; j++) tagIDs[j] = answerString.Substring(j * 20, 16);
                    }

                    for (int j = 0; j < noTags; j++)
                    {
                        if (j > 0) laneTimeIncrementMs = 0;
                        bool tagFirstAppearance = false; string tagIDlane1 = "-"; string tagIDlane2 = "-"; pairTime = String.Empty; string pairLine = String.Empty;
                        string detectionTime = dateTimeNow.Hour.ToString("D02") + ":" + dateTimeNow.Minute.ToString("D02") + ":" + dateTimeNow.Second.ToString("D02");
                        string detectionTimeWithMs = detectionTime + ":" + dateTimeNow.Millisecond.ToString("D03"); ;
                        if (lane == 1)
                        {
                            tagFirstAppearance = (!tagIDs[j].StartsWith("NO TAG")) && (DebouncedTagsLane1.AddEntry(detectionTime, tagIDs[j], debounceInterval));
                            if (tagFirstAppearance)
                            {
                                pairTime = DebouncedTagsLane2.GetPair(tagIDs[j], detectionTime, pairInterval);
                                if (pairTime != String.Empty) DebouncedTagsLane1.paired[DebouncedTagsLane1.noEntries - 1] = true;

                                if (pairTime != String.Empty) pairLine = "L2 -> L1    " + tagIDs[j] + "    " + pairTime + "   " + detectionTime;
                            }
                            tagIDlane1 = tagIDs[j];
                        }
                        else if (lane == 2)
                        {
                            tagFirstAppearance = (!tagIDs[j].StartsWith("NO TAG")) && (DebouncedTagsLane2.AddEntry(detectionTime, tagIDs[j], debounceInterval));
                            if (tagFirstAppearance)
                            {
                                pairTime = DebouncedTagsLane1.GetPair(tagIDs[j], detectionTime, pairInterval);
                                if (pairTime != String.Empty) DebouncedTagsLane2.paired[DebouncedTagsLane2.noEntries - 1] = true;

                                if (pairTime != String.Empty) pairLine = "L1 -> L2    " + tagIDs[j] + "    " + pairTime + "   " + detectionTime;
                            }
                            tagIDlane2 = tagIDs[j];
                        }

                        lblNoPasses.Invoke(new ulblD(update_lblNoPasses), new object[] { noPasses });

                        if (!tagIDs[j].StartsWith("NO TAG"))
                        {
                            if (lane == 1) answer = RequestAnswer(SetOutput1, sp); if (answer.StartsWith("ERR")) { tbPairs.Invoke(new utbD(update_tbPairs), new object[] { answer }); rLRR(); break; }
                            else if (lane == 2) answer = RequestAnswer(SetOutput2, sp); if (answer.StartsWith("ERR")) { tbPairs.Invoke(new utbD(update_tbPairs), new object[] { answer }); rLRR(); break; }

                            uint actaTagID = 0; string fullName = "-";
                            if (ICodeData.Count > 0)
                            {
                                try
                                {
                                    actaTagID = (uint)(ICodeData[tagIDs[j]].EmployeeID);
                                    fullName = ICodeData[tagIDs[j]].FirstAndLastName;
                                }
                                catch
                                {
                                    actaTagID = UInt32.Parse(tagIDs[j].Substring(12),System.Globalization.NumberStyles.HexNumber);
                                    fullName = "Ime " + actaTagID.ToString();
                                }
                            }
                            if (pairLine != String.Empty) pairLine += "   " + fullName;

                            ListViewItem lvi = new ListViewItem();
                            lvi.SubItems[0].Text = (noPasses + 1).ToString("D06"); lvi.SubItems.Add(detectionTimeWithMs);
                            lvi.SubItems.Add(tagIDlane1); lvi.SubItems.Add(tagIDlane2); lvi.SubItems.Add(pairTime);
                            lvi.SubItems.Add(laneTimeIncrementMs.ToString("D4")); lvi.SubItems.Add(betweenLanesTimeIncrementMs.ToString("D4"));
                            lvi.SubItems.Add(fullName);
                            if (tagFirstAppearance) lvi.ForeColor = Color.Black; else lvi.ForeColor = Color.DarkGray;
                            if (pairTime != String.Empty) lvi.SubItems[4].ForeColor = Color.Green;
                            lvReadResults.Invoke(new ulvD(update_lvReadResults), new object[] { lvi });

                            if (pairLine != String.Empty)
                            {
                                tbPairs.Invoke(new utbD(update_tbPairs), new object[] { pairLine }); tbPairs.Invoke(new stbD(scroll_tbPairs));

                                string dtStamp = dateTimeNow.Year.ToString("D04") + dateTimeNow.Month.ToString("D02") + dateTimeNow.Day.ToString("D02") +
                                                 dateTimeNow.Hour.ToString("D02") + dateTimeNow.Minute.ToString("D02") + dateTimeNow.Second.ToString("D02");
                                int antenna = -1; if (lane == 1) antenna = 1; else antenna = 0;
                                List<LogTO> logTOList = new List<LogTO>();
                                LogTO logTO = new LogTO(0, 1, actaTagID, antenna, 4, 20, dateTimeNow, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,-1);
                                logTOList.Add(logTO);
                                serialize(logTOList, "LRR_1_" + logTO.TagID + "_" + dtStamp + ".xml");
                            }
                        }

                        noPasses++; if (noPasses == 1000000) noPasses = 0;

                        maintenanceCounter++; if (maintenanceCounter >= MAINTENANCE_PASSES) { maintenanceCounter = 0; lvReadResults.Invoke(new alvW(archive_Windows)); ICodeData = Misc.GetICodeData(); break; }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        tbPairs.Invoke(new utbD(update_tbPairs), new object[] { "Error in pass " + noPasses.ToString() + ": " + ex.Message });
                    }
                    catch { }
                }

                Thread.Sleep(10);
            }
        }

        string RequestAnswer(byte[] command, LRRinterface sp)
        {
            const byte STX = 0x02; const byte ETX = 0x03; const byte DLE = 0x10; const byte NAK = 0x15; const int mT = 3;
            byte[] Answer = new byte[1024]; string answerString = String.Empty;
            byte[] sb1 = new byte[1]; byte[] rb1 = new byte[1];

            try
            {
                int nT = 0;
                while (nT < mT)
                {
                    nT++; sp.Flush();

                    // STX phase
                    sb1[0] = STX; sp.Send(sb1);
                    if (sp.Receive(rb1, 1) == 0) { answerString = "ERR: " + "waiting DLE after STX timed out"; if (nT < mT) continue; else break; };
                    if (rb1[0] != DLE) { answerString = "ERR: " + rb1[0].ToString("X02") + " received instead of DLE after STX"; if (nT < mT) continue; else break; }

                    // Request phase
                    byte[] Request = new byte[command.Length + 2]; for (int i = 0; i < command.Length; i++) Request[i] = command[i];
                    byte[] crc = CRC13239(command, command.Length); Request[Request.Length - 2] = crc[0]; Request[Request.Length - 1] = crc[1];
                    sp.Send(Request);
                    sb1[0] = DLE; sp.Send(sb1);
                    sb1[0] = ETX; sp.Send(sb1);
                    if (sp.Receive(rb1, 1) == 0) { answerString = "ERR: " + "waiting DLE after Request timed out"; if (nT < mT) continue; else break; };
                    if (rb1[0] != DLE) { answerString = "ERR: " + rb1[0].ToString("X02") + " received instead of DLE after Request"; if (nT < mT) continue; else break; }
                    if (sp.Receive(rb1, 1) == 0) { answerString = "ERR: " + "waiting STX after DLE timed out"; if (nT < mT) continue; else break; };
                    if (rb1[0] != STX) { answerString = "ERR: " + rb1[0].ToString("X02") + " received instead of STX after DLE"; if (nT < mT) continue; else break; }

                    // Answer phase
                    sb1[0] = DLE; sp.Send(sb1);
                    if (sp.Receive(Answer) == 0) { answerString = "ERR: " + "waiting Answer after DLE timed out"; if (nT < mT) continue; else break; }
                    sb1[0] = DLE; sp.Send(sb1);

                    answerString = String.Empty;
                    for (int i = Answer[2] + (Answer[3] << 8) - 1; i >= 0; i--) answerString += Answer[i + 4].ToString("X02"); //Console.WriteLine(answerString);
                    break;
                }
                return answerString;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error in RequestAnswer: " + ex.Message));
            }
        }

        byte[] CRC13239(byte[] buffer, int length)
        {
            try
            {
                const ushort CRC_POLYNOM = 0x8408;
                const ushort CRC_PRESET = 0xFFFF;
                ushort wCRC = CRC_PRESET;

                for (int i = 0; i < length; i++)
                {
                    wCRC ^= buffer[i];
                    for (int j = 0; j < 8; j++)
                    {
                        if ((wCRC & 0x0001) > 0)
                            wCRC = (ushort)((wCRC >> 1) ^ CRC_POLYNOM);
                        else
                            wCRC = (ushort)(wCRC >> 1);
                    }
                }
                byte[] crcBytes = new byte[2]; crcBytes[0] = (byte)(wCRC & 0x00FF); crcBytes[1] = (byte)((wCRC & 0xFF00) >> 8);

                return crcBytes;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error in CRC13239: " + ex.Message));
            }
        }

        private bool serialize(List<LogTO> LogTOList, string filePath)
        {
            bool isSerialized = false;

            try
            {
                Stream stream = File.Open(filePath, FileMode.Create);
                LogTO[] logTOArray = (LogTO[])LogTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(LogTO[]));
                bformatter.Serialize(stream, logTOArray);
                stream.Close();
                isSerialized = true;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error in serialize: " + ex.Message));
            }

            return isSerialized;
        }
    }
}