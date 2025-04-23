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

using Util;
using Common;
using TransferObjects;

namespace Reports.Grundfos
{
    public partial class GrundfosPYReportView : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        int startIndex = 0;

        uint calcID = 0;
        Dictionary<int, Dictionary<string, EmployeePYDataSumTO>> emplSumDict = new Dictionary<int, Dictionary<string, EmployeePYDataSumTO>>();
        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        Dictionary<int, Dictionary<string, RuleTO>> emplRuleDict = new Dictionary<int, Dictionary<string, RuleTO>>();

        List<int> paidLeaves = new List<int>();
        List<int> unpaidLeaves = new List<int>();

        public GrundfosPYReportView(uint calcID)
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(GrundfosPYReportView).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                this.calcID = calcID;
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
                this.Text = rm.GetString("GrundfosPYReportView", culture);
                
                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnGenerate.Text = rm.GetString("btnGenerate", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReportView.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrundfosPYReportView_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                EmployeePYDataSum sum = new EmployeePYDataSum();
                sum.EmplSum.PYCalcID = calcID;
                List<EmployeePYDataSumTO> sumList = sum.getEmployeesSum();

                foreach (EmployeePYDataSumTO sumTO in sumList)
                {
                    if (!emplSumDict.ContainsKey(sumTO.EmployeeID))
                        emplSumDict.Add(sumTO.EmployeeID, new Dictionary<string, EmployeePYDataSumTO>());

                    if (!emplSumDict[sumTO.EmployeeID].ContainsKey(sumTO.PaymentCode.Trim()))
                        emplSumDict[sumTO.EmployeeID].Add(sumTO.PaymentCode.Trim(), sumTO);
                }

                emplDict = new Employee().SearchDictionary();

                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> ruleDict = new Common.Rule().SearchWUEmplTypeDictionary();
                
                Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit().getWUDictionary();

                foreach (int id in emplDict.Keys)
                {
                    if (!emplRuleDict.ContainsKey(id))
                        emplRuleDict.Add(id, new Dictionary<string, RuleTO>());

                    int company = Common.Misc.getRootWorkingUnit(emplDict[id].WorkingUnitID, wuDict);

                    if (ruleDict.ContainsKey(company) && ruleDict[company].ContainsKey(emplDict[id].EmployeeTypeID))
                        emplRuleDict[id] = ruleDict[company][emplDict[id].EmployeeTypeID];
                }

                // get all paid leave
                Dictionary<int, List<int>> confirmationPTDict = new PassTypesConfirmation().SearchDictionary();
                List<int> sickLeaveNCF = new Common.Rule().SearchRulesExact(Constants.RuleCompanySickLeaveNCF);
                //**** search unpaid leaves                

                foreach (int ptID in confirmationPTDict.Keys)
                {
                    bool isUnpaid = false;
                    foreach (int unpaidID in unpaidLeaves)
                    {
                        if (confirmationPTDict[ptID].Contains(unpaidID))
                        {
                            isUnpaid = true;
                            break;
                        }
                    }

                    if (!isUnpaid && !sickLeaveNCF.Contains(ptID))
                        paidLeaves.AddRange(confirmationPTDict[ptID]);
                }

                populatePYData(false);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GrundfosPYReportView.GrundfosPYReportView_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void populatePYData(bool saveValues)
        {
            int i = 0;
            try
            {
                if (emplSumDict.Keys.Count > Constants.recPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                foreach (Control ctrl in dataPanel.Controls)
                {
                    if (ctrl is EmployeePYSumData)
                    {
                        if (saveValues)
                        {
                            EmployeeTO empl = ((EmployeePYSumData)ctrl).GetEmployee();
                            if (empl.EmployeeID != -1)
                            {
                                Dictionary<string, EmployeePYDataSumTO> sumDict = ((EmployeePYSumData)ctrl).GetDataValues();

                                if (!emplSumDict.ContainsKey(empl.EmployeeID))
                                    emplSumDict.Add(empl.EmployeeID, sumDict);
                                else
                                    emplSumDict[empl.EmployeeID] = sumDict;
                            }
                        }
                    }
                }

                dataPanel.Controls.Clear();

                GC.Collect();

                int xPos = 0;
                int yPos = 0;

                // insert header line (report header)
                EmployeePYSumData hdr = new EmployeePYSumData(new EmployeeTO(), new Dictionary<string, EmployeePYDataSumTO>(), new Dictionary<string, RuleTO>(), paidLeaves, unpaidLeaves, -1, xPos, yPos, true);
                dataPanel.Controls.Add(hdr);

                yPos += hdr.Height;

                if (emplSumDict.Keys.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < emplSumDict.Keys.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recPerPage;
                        if (lastIndex >= emplSumDict.Keys.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = emplSumDict.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        IEnumerator dataEnumerator = emplSumDict.Keys.GetEnumerator();
                        while (dataEnumerator.MoveNext() && i < lastIndex)
                        {
                            if (i < startIndex)
                            {
                                i++;
                                continue;
                            }

                            int id = (int)dataEnumerator.Current;

                            EmployeeTO emplTO = new EmployeeTO();
                            Dictionary<string, EmployeePYDataSumTO> sumDict = new Dictionary<string, EmployeePYDataSumTO>();
                            Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();

                            if (emplSumDict.ContainsKey(id))
                                sumDict = emplSumDict[id];

                            if (emplDict.ContainsKey(id))
                                emplTO = emplDict[id];

                            if (emplRuleDict.ContainsKey(id))
                                emplRules = emplRuleDict[id];
                            
                            EmployeePYSumData ctrl = new EmployeePYSumData(emplTO, sumDict, emplRules, paidLeaves, unpaidLeaves, i + 1, xPos, yPos, false);
                            dataPanel.Controls.Add(ctrl);

                            yPos += ctrl.Height;

                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GrundfosPYReportView.populatePYData(): " + i.ToString().Trim() + " - " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                startIndex -= Constants.recPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }

                populatePYData(true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GrundfosPYReportView.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                startIndex += Constants.recPerPage;
                
                populatePYData(true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GrundfosPYReportView.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void GrundfosPYReportView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (Control ctrl in dataPanel.Controls)
                {
                    if (ctrl is EmployeePYSumData)
                        ctrl.Dispose();
                }

                dataPanel.Controls.Clear();

                GC.Collect();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GrundfosPYReportView.GrundfosPYReportView_FormClosing(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
