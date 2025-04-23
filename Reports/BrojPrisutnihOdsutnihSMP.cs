using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using TransferObjects;

namespace Reports
{
    public partial class BrojPrisutnihOdsutnihSMP : Form
    {
        public BrojPrisutnihOdsutnihSMP()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                lblInfo.Visible = true;
                lblInfo.Text = "Izrada izvestaja u toku...";
                string wuName = "";
                lblError.Text = "";
                lblError.Visible = false;
                //tbMsg.Text = "";
                //tbMsg.Visible = false;
                if(lbWU.SelectedItems.Count<=0 && !cbAll.Checked)
                {
                    MessageBox.Show("Morate izabrati barem jednu radnu jedinicu!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (var item in lbWU.SelectedItems)
                {
                    wuName += "'"+item.ToString() + "',";
                }
                if (cbAll.Checked)
                {
                    foreach (var item in lbWU.Items)
                    {
                        wuName += "'"+item.ToString() + "',";
                    }
                }
                if (wuName.Length > 0)
                    wuName = wuName.Substring(0, wuName.Length - 1);
                List<TransferObjects.EmployeeTO> listaRadnikaZaWU = listaRadnikaIzWU(wuName);
                if (listaRadnikaZaWU.Count <= 0 && lbWU.SelectedItems.Count==1)
                {
                    MessageBox.Show("Nema radnika za izabranu radnu jedinicu!","INFO",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
                else if (listaRadnikaZaWU.Count <= 0 && lbWU.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Nema radnika za izabrane radne jedinice!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<TransferObjects.IOPairProcessedTO> pairs = listaProlazakaZaRadnike(listaRadnikaZaWU, dtpFrom.Value.Date, dtpTo.Value.Date);
                if (pairs.Count <= 0)
                {
                    MessageBox.Show("Nema podataka za zadate kriterijume!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int brojOcekivanihSati = brSatiZaInterval(dtpFrom.Value.Date, dtpTo.Value.Date, listaRadnikaZaWU);
                Dictionary<int, TimeSpan> passType_duration = new Dictionary<int, TimeSpan>();
                foreach (TransferObjects.IOPairProcessedTO pair in pairs)
                {
                    if (pair.EndTime.Minute == 59)
                        pair.EndTime=pair.EndTime.AddMinutes(1);
                    TimeSpan duration = pair.EndTime - pair.StartTime;
                    if (!passType_duration.ContainsKey(pair.PassTypeID))
                        passType_duration.Add(pair.PassTypeID, new TimeSpan());
                    passType_duration[pair.PassTypeID] += duration;
                }
                Dictionary<int, int> passType_numEmpl = new Dictionary<int, int>();
                Dictionary<int,List<int>> passType_empls= new Dictionary<int,List<int>>();
                foreach (TransferObjects.IOPairProcessedTO pair in pairs)
                {
                    if (!passType_empls.ContainsKey(pair.PassTypeID))
                        passType_empls.Add(pair.PassTypeID, new List<int>());
                    if (!passType_empls[pair.PassTypeID].Contains(pair.EmployeeID))
                        passType_empls[pair.PassTypeID].Add(pair.EmployeeID);
                    else
                        continue;
                    if (!passType_numEmpl.ContainsKey(pair.PassTypeID))
                        passType_numEmpl.Add(pair.PassTypeID, 0);
                    passType_numEmpl[pair.PassTypeID] += 1;
                }
                PopuniExcelSaPassTypes(passType_duration, passType_numEmpl, passType_empls, brojOcekivanihSati,listaRadnikaZaWU.Count,wuName, pairs);
                lblInfo.Text = "Izvestaj je gotov!";
                Thread.Sleep(1000);
                progressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Text = "GRESKA! " + DateTime.Now.ToString("hh:mm") + " --- " + ex.ToString();
                lblError.Visible = true;
                //tbMsg.Visible = true;
                //tbMsg.Text = "GRESKA! " + DateTime.Now.ToString("hh:mm") + " --- " + ex.ToString();
            }
        }

        private void PopuniExcelSaPassTypes(Dictionary<int, TimeSpan> passType_duration, Dictionary<int, int> passType_numEmpl, Dictionary<int, List<int>> passType_empls, int brOcekivanihSati, int brRadnika, string wuName, List<TransferObjects.IOPairProcessedTO> listPairs)
        {
            progressBar1.Step = 2;
            progressBar1.Value = 0;
            progressBar1.Maximum=(passType_duration.Count+passType_numEmpl.Count+passType_empls.Count)*2;
            if (progressBar1.Maximum <= 40)
            {
                progressBar1.Maximum *= 3;
                progressBar1.Step *= 3;
            }
            else if (progressBar1.Maximum <= 60)
            {
                progressBar1.Maximum *= 2;
                progressBar1.Step *= 2;
            }
            progressBar1.Width = progressBar1.Maximum;
            progressBar1.Visible = true;
            TimeSpan wholeHours = new TimeSpan();
            foreach (KeyValuePair<int, TimeSpan> pst in passType_duration)
            {
                wholeHours += pst.Value;
            }
            string logFilePath = Util.Constants.LPath + "\\TreciIzvestaj_template.xlsx";
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook wb =
                excel.Workbooks.Open(logFilePath, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, true,
                Type.Missing, Type.Missing, true, false, Type.Missing, false, Type.Missing, Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet oSheets;
            oSheets = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets[1];
            if(cbAll.Checked)
                oSheets.Cells[3, 2] = "Jedinica: sve jedinice izabrane";
            else
                oSheets.Cells[3, 2] = "Jedinica: "+wuName;
            oSheets.Cells[4, 2] = "Ukupno radnika: " + brRadnika;
            oSheets.Cells[5, 2] = "Ukupno planiranih sati: " + brOcekivanihSati + "h";
            oSheets.Cells[4, 3] = "Od: " + dtpFrom.Value.ToString("dd.MM.yyyy.");
            oSheets.Cells[5, 3] = "Do: " + dtpTo.Value.ToString("dd.MM.yyyy.");
            oSheets.Cells[4, 5] = "Ukupan broj sati";
            oSheets.Cells[5, 5] = (wholeHours.TotalHours * 1.0 / 24);
            oSheets.get_Range(oSheets.Cells[5, 5], oSheets.Cells[5, 5]).NumberFormat = "[h]:mm";
            int red = 7, kol = 2, i = 0;
            Common.PassType pt = new Common.PassType();
            Common.Employee emp = new Common.Employee();
            List<TransferObjects.PassTypeTO> listPT = new List<TransferObjects.PassTypeTO>();
            //popuni tipove prolaska koji idu za FTE nacin racunanja
            foreach (KeyValuePair<int, TimeSpan> pass_type in passType_duration)
            {
                TransferObjects.PassTypeTO ptTO = new TransferObjects.PassTypeTO();
                ptTO = pt.Find(pass_type.Key);
                //-1000,4,48,50,60,88,108
                if (pass_type.Key != -1000 && pass_type.Key != 4 && pass_type.Key != 48 && pass_type.Key != 50 && pass_type.Key != 60 && pass_type.Key!=88 && pass_type.Key!=108)
                {
                    oSheets.Cells[red + i, kol] = "(" + ptTO.PaymentCode + ") " + ptTO.Description;
                    oSheets.Cells[red + i, kol + 1] = pass_type.Value.TotalHours / brOcekivanihSati;
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 1], oSheets.Cells[red + i, kol + 1]).NumberFormat = "00.00";
                    oSheets.Cells[red + i, kol + 2] = pass_type.Value.TotalHours / brOcekivanihSati;
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 2], oSheets.Cells[red + i, kol + 2]).NumberFormat = "00.00%";
                    double dur = pass_type.Value.TotalHours / 24;
                    oSheets.Cells[red + i, kol + 3] = dur;
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 3], oSheets.Cells[red + i, kol + 3]).NumberFormat = "[hh]:mm";
                    oSheets.get_Range("D" + (red + i), "E" + (red + i)).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    if (passType_numEmpl.ContainsKey(pass_type.Key))
                    {
                        oSheets.Cells[red + i, kol + 4] = passType_numEmpl[pass_type.Key];
                        oSheets.get_Range(oSheets.Cells[red + i, kol + 4], oSheets.Cells[red + i, kol + 4]).NumberFormat = "0";
                    }
                    i++;
                }
                progressBar1.PerformStep();
            }
            i--;
            Microsoft.Office.Interop.Excel.ChartObjects xlCharts = (Microsoft.Office.Interop.Excel.ChartObjects)oSheets.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(0, 0, 550.08, 324);
            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
            chartPage.SetSourceData(oSheets.get_Range("B7", "C" + (red + i)), Type.Missing);
            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlPie;
            i++;
            oSheets.get_Range(oSheets.Cells[red + i, kol], oSheets.Cells[red + i, kol + 4]).Merge(true);
            oSheets.Cells[red + i, kol] = "Tipovi prolaska dole ne ulaze u FTE format racunanja!";
            oSheets.get_Range(oSheets.Cells[red + i, kol], oSheets.Cells[red + i, kol]).Font.Bold = true;
            oSheets.get_Range(oSheets.Cells[red + i, kol], oSheets.Cells[red + i, kol]).RowHeight = 30;
            i++;
            //tipovi prolazaka koji ne idu u FTE -- prekovremeni radovi
            foreach (KeyValuePair<int, TimeSpan> pass_type in passType_duration)
            {
                TransferObjects.PassTypeTO ptTO = new TransferObjects.PassTypeTO();
                ptTO = pt.Find(pass_type.Key);
                //-1000,4,48,50,60,88,108
                if (pass_type.Key == -1000 || pass_type.Key == 4 || pass_type.Key == 48 || pass_type.Key == 50 || pass_type.Key == 60 || pass_type.Key==88 || pass_type.Key==108)
                {
                    oSheets.Cells[red + i, kol] = "(" + ptTO.PaymentCode + ") " + ptTO.Description;
                    oSheets.Cells[red + i, kol + 1] = "Neocekivani sati";
                    oSheets.Cells[red + i, kol + 2] = "Neocekivani sati";
                    double dur = pass_type.Value.TotalHours / 24;
                    oSheets.Cells[red + i, kol + 3] = dur;
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 3], oSheets.Cells[red + i, kol + 3]).NumberFormat = "[hh]:mm";
                    oSheets.get_Range(oSheets.Cells[red + i, kol + 3], oSheets.Cells[red + i, kol + 3]).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    if (passType_numEmpl.ContainsKey(pass_type.Key))
                    {
                        oSheets.Cells[red + i, kol + 4] = passType_numEmpl[pass_type.Key];
                        oSheets.get_Range(oSheets.Cells[red + i, kol + 4], oSheets.Cells[red + i, kol + 4]).NumberFormat = "0";
                    }
                    i++;
                }
                progressBar1.PerformStep();
            }
            i--;
            Microsoft.Office.Interop.Excel.Range r = oSheets.get_Range("B3", "F" + (red + i));
            r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            r.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;


            //postavljanje info o radnicima za tipove prolaska u dictionary
            red = 1; kol = 8; i = 0; // postavljanje parametara za celije u excel

            foreach (KeyValuePair<int, List<int>> emplFromList in passType_empls)
            {
                TransferObjects.PassTypeTO ptTO = new TransferObjects.PassTypeTO();
                TransferObjects.EmployeeTO emplTO = new TransferObjects.EmployeeTO();
                ptTO = pt.Find(emplFromList.Key);
                oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red, kol + 2 + i]).Merge(true);
                oSheets.get_Range(oSheets.Cells[red + 1, kol + i], oSheets.Cells[red + 1, kol + 2 + i]).Merge(true);
                oSheets.Cells[red, kol + i] = "(" + ptTO.PaymentCode + ") " + ptTO.Description;
                oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red, kol + i]).WrapText = true;
                Microsoft.Office.Interop.Excel.Range rng = oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red, kol + 2 + i]);
                rng.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                rng.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red, kol + 2 + i]).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                red = 3;
                foreach (int emplID in emplFromList.Value)
                {
                    emplTO = emp.Find(emplID.ToString().Trim());
                    TimeSpan duration = new TimeSpan();
                    foreach (TransferObjects.IOPairProcessedTO pair in listPairs)
                    {
                        if (pair.PassTypeID == emplFromList.Key && emplID == pair.EmployeeID)
                        {
                            if (pair.EndTime.Minute == 59)
                                pair.EndTime=pair.EndTime.AddMinutes(1);
                            duration += (pair.EndTime - pair.StartTime);
                        }
                    }
                    oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red + 1, kol + i + 2]).Merge(false);
                    oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red + 1, kol + i + 2]).WrapText = true;
                    oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red + 1, kol + i + 2]).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    oSheets.get_Range(oSheets.Cells[red, kol + i], oSheets.Cells[red + 1, kol + i + 2]).Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
                    string hours = "", minutes = "";
                    int hoursInt = duration.Hours; int minutesInt = duration.Minutes; int daysInt = duration.Days;
                    if (daysInt > 0)
                        hoursInt += daysInt * 24;
                    if (hoursInt.ToString().Length < 2)
                    {
                        hours = "0" + hoursInt.ToString();
                    }
                    else
                        hours = hoursInt.ToString();
                    if (minutesInt.ToString().Length < 2)
                        minutes = "0" + minutesInt.ToString();
                    else
                        minutes = minutesInt.ToString();
                    oSheets.Cells[red, kol + i] = emplTO.FirstName + " " + emplTO.LastName + "(" + emplTO.EmployeeID + "), ukupno " + hours + ":" + minutes + "h";
                    red += 2;
                }
                red--;
                Microsoft.Office.Interop.Excel.Range rng1 = oSheets.get_Range(oSheets.Cells[3, kol + i], oSheets.Cells[red, kol + i+2]);
                rng1.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                rng1.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                i += 4;
                red = 1;
                progressBar1.PerformStep();
            }


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File|*.xls";
            saveFileDialog.Title = "Save an Excel File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK) //ukoliko je izabrana validna putanja
            {
                if (saveFileDialog.FileName != "") //i dato ime fajla
                {
                    wb.SaveAs(saveFileDialog.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                                            Type.Missing, Type.Missing,
                                            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                            Type.Missing, Type.Missing, Type.Missing,
                                            Type.Missing, Type.Missing);
                    Thread.Sleep(500);
                    excel.Visible = true;
                    //this.labelMessage.Text = this.labelMessage.Text = rm.GetString("MozetePogledatiIzvestaj", culture);
                }
            }
            else
            {
                //this.labelMessage.Text = this.labelMessage.Text = rm.GetString("NisiSacuvao", culture);
                wb.Close(false, "", "");
            }
            excel.Visible = false;
            excel.Quit();
            Marshal.ReleaseComObject(oSheets);
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(excel);

            oSheets = null;
            wb = null;
            excel = null;
            GC.GetTotalMemory(false);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.GetTotalMemory(true);
        }

        private int brSatiZaInterval(DateTime from, DateTime to, List<TransferObjects.EmployeeTO> listaRadnikaZaWU)
        {
            int brSati = 0;
            Dictionary<int, TransferObjects.WorkTimeSchemaTO> dictTimeSchema = new Common.TimeSchema().getDictionary();
            //Dictionary<int, List<TransferObjects.EmployeeTimeScheduleTO>> EmplTimeSchemas = new Common.EmployeesTimeSchedule().SearchEmployeesSchedulesDS;
            foreach (TransferObjects.EmployeeTO emplTO in listaRadnikaZaWU)
            {
                if (emplTO.EmployeeID == 64)
                {
                }
                Dictionary<int, List<TransferObjects.EmployeeTimeScheduleTO>> EmplTimeSchemas = new Common.EmployeesTimeSchedule().SearchEmployeesSchedulesDS(emplTO.EmployeeID.ToString().Trim(), from.Date, to.AddDays(1).Date);
                List <TransferObjects.EmployeeTimeScheduleTO> emplTS = EmplTimeSchemas[emplTO.EmployeeID];
                DateTime currDate = from;
                while (currDate.Date <= to.Date)
                {
                    bool is2DayShift = false;
                    bool is2DayShiftPrevious = false;
                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                    WorkTimeSchemaTO workTimeSchema = null;
                    Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, currDate, ref is2DayShift,
                                ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, dictTimeSchema);
                    List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                    TimeSpan intervalDuration = new TimeSpan();
                    if (edi == null)
                    {
                        currDate = currDate.AddDays(1);
                        continue;
                    }
                    intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);
                    foreach (WorkTimeIntervalTO interval in intervals)
                    {
                        intervalDuration += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                        if (interval.EndTime.Minute == 59)
                            intervalDuration = intervalDuration.Add(new TimeSpan(0, 1, 0));
                    }
                    brSati += intervalDuration.Hours;
                    currDate = currDate.AddDays(1);

                }
            }
            return brSati;
        }

        //private int brSatiZaInterval(DateTime from, DateTime to, List<TransferObjects.EmployeeTO> listaRadnikaZaWU)
        //{
        //    Common.EmployeeAsco4 ea= new Common.EmployeeAsco4();
        //    int brSati = 0;
        //    DateTime currDate = from;
        //    foreach (TransferObjects.EmployeeTO empl in listaRadnikaZaWU)
        //    {
        //        List<TransferObjects.EmployeeAsco4TO> eaEmpl = ea.Search(empl.EmployeeID.ToString());
        //        DateTime lastHiringDate = new DateTime();
        //        foreach (TransferObjects.EmployeeAsco4TO eaEmpl4 in eaEmpl)
        //        {
        //            lastHiringDate = eaEmpl4.DatetimeValue2.Date;
        //        }
        //        while (currDate <= to)
        //        {
        //            if (currDate.DayOfWeek != DayOfWeek.Saturday && currDate.DayOfWeek != DayOfWeek.Sunday && lastHiringDate<=currDate)
        //            {
        //                brSati+=8;
        //            }
        //            currDate = currDate.AddDays(1);
        //        }
        //        currDate = from;
        //    }
        //    return brSati;
        //}

        private List<TransferObjects.EmployeeTO> listaRadnikaIzWU(string name)
        {
            Common.Employee empl = new Common.Employee();
            return empl.listaRadnikaZaWU(name);
        }
        private List<TransferObjects.IOPairProcessedTO> listaProlazakaZaRadnike(List<TransferObjects.EmployeeTO> listaRadnikaIzWU, DateTime dtpFrom, DateTime dtpTo)
        {
            List<TransferObjects.IOPairProcessedTO> listaParova = new List<TransferObjects.IOPairProcessedTO>();
            if (dtpFrom > dtpTo)
            {
                lblError.Text = "Neispravan interval datuma!";
                lblError.Visible = true;
                return new List<TransferObjects.IOPairProcessedTO>();
            }
            string emplIDs = "";
            foreach (TransferObjects.EmployeeTO item in listaRadnikaIzWU)
            {
                emplIDs += item.EmployeeID + ",";
            }
            if (emplIDs.Length > 0)
            {
                emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
            }
            return listaParova = new Common.IOPairProcessed().pairsForPeriod(emplIDs, dtpFrom, dtpTo);
        }

        private void BrojPrisutnihOdsutnihSMP_Load(object sender, EventArgs e)
        {
            try
            {
                Common.WorkingUnit wu = new Common.WorkingUnit();
                List<TransferObjects.WorkingUnitTO> listaWU = wu.getAllWU();
                foreach (TransferObjects.WorkingUnitTO wuItem in listaWU)
                {
                    lbWU.Items.Add(wuItem.Name);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "GRESKA! "+DateTime.Now.ToString("hh:mm")+" --- " +ex.ToString();
                lblError.Visible = true;
                //tbMsg.Visible = true;
                //tbMsg.Text = "GRESKA! " + DateTime.Now.ToString("HH:mm") + " --- " + ex.ToString();
            }
        }

        private void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAll.Checked)
                lbWU.Enabled = false;
            else
                lbWU.Enabled = true;
        }
    }
}
