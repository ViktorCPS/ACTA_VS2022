using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Globalization;
using System.Resources;

using Common;
using Util;
using TransferObjects;
using System.IO;

using System.Configuration;
using Microsoft.Office.Interop;
using System.Runtime.InteropServices;


using System.Threading;
using Microsoft.Office.Interop.Excel;

namespace Reports
{
    public partial class ReportForSalaryForecast : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog debug;
        bool isForecast;
        public ReportForSalaryForecast()
        {
            InitializeComponent();
            this.CenterToScreen();

            // Debug
            string logFilePath = Util.Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
            setLanguage();

            this.labelMessage.Text = rm.GetString("msgHistoryPassesForecast", culture);
            isForecast = false;
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("menuMonthlyTypeReport", culture);
                this.btnGenerateReport.Text = rm.GetString("btnGenerateReport", culture);
                this.gbChooseMonth.Text = rm.GetString("gbChooseMonth", culture);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se greška:\n" + ex.Message);
                debug.writeLog(DateTime.Now + " Reports.ReportForSalaryForecast.setLanguage() " + ex.Message);
            }
        }

        private void MonthlyPresenceTracking_Load(object sender, EventArgs e)
        {



        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                //this.labelMessage.Text = rm.GetString("msgHistoryPassesForecast", culture);
                this.labelMessage.Text = this.labelMessage.Text = rm.GetString("PrikupljanjeProlaska", culture);
                Employee zaPozivanjeFja = new Employee();
                List<EmployeeTO> spisakProlazaka = new List<EmployeeTO>();
                DateTime mesec = new DateTime(dtpMesec.Value.Year, dtpMesec.Value.Month, 1, 0, 0, 0);
                if (mesec == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                {
                    isForecast = true;
                }
                else
                {
                    isForecast = false;
                }
                List<EmployeeTO> spisakZaposlenih = new List<EmployeeTO>();

                spisakProlazaka = zaPozivanjeFja.ProlasciZaForecast(mesec);
                List<EmployeeTO> spisakPredvidjenihProlazaka = new List<EmployeeTO>();
                if (isForecast)
                {
                    this.labelMessage.Text = this.labelMessage.Text = rm.GetString("ProlasciZaForecast", culture);
                    spisakPredvidjenihProlazaka = zaPozivanjeFja.ForecastProlasci(mesec.AddMonths(1));
                }
                List<EmployeeTO> listaa = new List<EmployeeTO>();
                /* foreach(EmployeeTO empl in spisakPredvidjenihProlazaka)
                 {
                     if (empl.EmployeeID == 10)
                     {
                       
                         listaa.Add(empl);
                     }
                     else { continue; }
                 }*/
                this.labelMessage.Text = this.labelMessage.Text = rm.GetString("ZavrsenoPrikupljanjePodataka", culture);
                this.labelMessage.Text = this.labelMessage.Text = rm.GetString("PocetakObradeProlazaka", culture);


                Dictionary<int, int[]> zapNizMinuta = new Dictionary<int, int[]>();

                int lastEmplID = 0;


                int brZap = zaPozivanjeFja.BrZaposlenihDoOvogMeseca(mesec);


                int[][] niz = new int[brZap][];
                for (int i = 0; i < brZap; i++)
                    niz[i] = new int[29];
                int[] poslednjaVrednostNiza = new int[28];

                int bf = 0;
                int a = 0;
                for (int i = 0; i < spisakProlazaka.Count; i++)
                {
                    if (spisakProlazaka[i].EmployeeID != lastEmplID)
                    {
                        if (lastEmplID != 0)
                        {
                            bool uneo1 = false;
                            int pbf = bf;
                            if (isForecast)
                            {
                                while (bf < spisakPredvidjenihProlazaka.Count && spisakPredvidjenihProlazaka[bf].EmployeeID <= lastEmplID) //Nenad 27. XI 2018. bf was bigger than list count in some situations
                                // if (spisakProlazaka[i].EmployeeID == spisakPredvidjenihProlazaka[bf].EmployeeID)
                                {
                                    if (spisakPredvidjenihProlazaka[bf].EmployeeID == lastEmplID)
                                    {
                                        poslednjaVrednostNiza = CreateArray(spisakPredvidjenihProlazaka[bf].WorkingUnitID, spisakPredvidjenihProlazaka[bf].OrgUnitID, poslednjaVrednostNiza);
                                        uneo1 = true;
                                    }
                                    bf++;
                                }
                            }
                            if (uneo1 || pbf == bf)
                            {
                                zapNizMinuta.Add(lastEmplID, poslednjaVrednostNiza); //unos prethodnog zaposlenog i njegov niz minuta 
                                a++;
                            }

                        }
                        if (a == 990)
                        {
                        }

                        //prvi unos 
                        Array.Clear(niz[a], 0, niz[a].Length); // svaki element je 0, samo za prvi unos                        
                        lastEmplID = spisakProlazaka[i].EmployeeID;
                        poslednjaVrednostNiza = CreateArray(spisakProlazaka[i].WorkingUnitID, spisakProlazaka[i].OrgUnitID, niz[a]); // prvi unos minuta odredjenog tipa prolaska od datog zaposlenog

                        EmployeeTO empl = new EmployeeTO();
                        empl.EmployeeID = spisakProlazaka[i].EmployeeID;
                        empl.FirstName = spisakProlazaka[i].FirstName;
                        empl.WorkingUnitName = spisakProlazaka[i].WorkingUnitName;
                        empl.LastName = spisakProlazaka[i].LastName;
                        empl.CreatedBy = spisakProlazaka[i].CreatedBy;
                        empl.OrgUnitName = spisakProlazaka[i].OrgUnitName;
                        spisakZaposlenih.Add(empl);
                    }
                    else
                    {
                        niz[a] = poslednjaVrednostNiza;
                        poslednjaVrednostNiza = CreateArray(spisakProlazaka[i].WorkingUnitID, spisakProlazaka[i].OrgUnitID, niz[a]); // svaki sledeci unos minuta odredjenog tipa prolaska od datog zaposleno
                    }

                }

                //unos poslednjeg saposlenog
                //mm bool uneoPosl = false;
                if (isForecast)
                {
                    while (bf < spisakPredvidjenihProlazaka.Count && spisakPredvidjenihProlazaka[bf].EmployeeID <= lastEmplID) //Nenad 27. XI 2018. bf was bigger than list count in some situations
                    {
                        if (spisakPredvidjenihProlazaka[bf].EmployeeID == lastEmplID)
                        {
                            poslednjaVrednostNiza = CreateArray(spisakPredvidjenihProlazaka[bf].WorkingUnitID, spisakPredvidjenihProlazaka[bf].OrgUnitID, poslednjaVrednostNiza);//  Forecast predvidjanja, unos minuta odredjenog tipa prolaska od datog zaposleno
                            //mm uneoPosl = true;
                        }
                        if (bf + 1 < spisakPredvidjenihProlazaka.Count)
                            bf++;
                        else
                        {
                            break;
                        }

                    }
                }
                //if (uneoPosl) Miodrag, neophodno je uneti niz prolazaka u svakom slucaju jer za poslednjeg zaposlenoh svakako nije unet. 
                //{
                zapNizMinuta.Add(lastEmplID, poslednjaVrednostNiza); // unos poslednjeg zaposlenog i  njegov niz minuta
                //}


                string logFilePath = Util.Constants.LPath + "\\" + "template.xls";
                this.progressBar1.Value = 0;
                this.progressBar1.Step = 1;
                this.progressBar1.Maximum = spisakZaposlenih.Count;
                this.progressBar1.Visible = true;
                this.labelMessage.Text = this.labelMessage.Text = rm.GetString("NeDiraj", culture);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook wb =
                    excel.Workbooks.Open(logFilePath, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, true,
                    Type.Missing, Type.Missing, true, false, Type.Missing, false, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet oSheets;
                oSheets = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets[1];
                int rb = 0, red = 8;
                bool desilaSeGreska = false;
                //ApplUser applUser = new ApplUser();
                //List<ApplUserTO> userList = applUser.Search();
                EmployeeAsco4 emplAsco4 = new EmployeeAsco4();
                Employee emplBase = new Employee();
                IOPairProcessed ioPairProc = new IOPairProcessed();
                foreach (EmployeeTO zap in spisakZaposlenih)
                {
                    try
                    {
                        if (zap.EmployeeID == 285)
                        {
                        }
                        if (zap.CreatedBy == "EKSTERNI")
                            continue;
                        int k = 0;
                        rb++;
                        //Informacije o zaposlenom
                        oSheets.Cells[rb + 7, 1] = zap.EmployeeID;//redni broj
                        oSheets.Cells[rb + 7, 2] = zap.EmployeeID;
                        if (zap.CreatedBy == "INDIRECT")
                        {
                            oSheets.Cells[rb + 7, 3] = "INDIRECT";
                            oSheets.Cells[rb + 7, 4] = "White Collar";
                        }
                        else if (zap.CreatedBy=="DIRECT")
                        {
                            oSheets.Cells[rb + 7, 3] = "DIRECT";
                            oSheets.Cells[rb + 7, 4] = "Blue Collar";
                        }
                        else
                        {
                            oSheets.Cells[rb + 7, 3] = "/";
                            oSheets.Cells[rb + 7, 4] = "/";
                        }
                        oSheets.Cells[rb + 7, 5] = zap.WorkingUnitName;
                        oSheets.Cells[rb + 7, 6] = zap.WorkingUnitName;
                        oSheets.Cells[rb + 7, 7] = zap.OrgUnitName;
                        oSheets.Cells[rb + 7, 8] = zap.FirstName;
                        oSheets.Cells[rb + 7, 9] = zap.LastName;
                        List<EmployeeAsco4TO> listEmplAsco4 = emplAsco4.Search(zap.EmployeeID.ToString());
                        foreach (EmployeeAsco4TO item in listEmplAsco4)
                        {
                            if (item.DatetimeValue2.Month == mesec.Month && item.DatetimeValue2.Year==mesec.Year)
                            {
                                oSheets.Cells[rb + 7, 10] = item.DatetimeValue2.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                oSheets.Cells[rb + 7, 10] = mesec.Month.ToString()+"/1/"+mesec.Year.ToString();
                            }
                            if (item.DatetimeValue3.Month == mesec.Month && item.DatetimeValue3.Year==mesec.Year)
                            {
                                oSheets.Cells[rb + 7, 11] = item.DatetimeValue3.AddDays(-1).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                switch (mesec.Month)
                                {
                                    case 1:
                                    case 3:
                                    case 5:
                                    case 7:
                                    case 8:
                                    case 10:
                                    case 12:
                                        oSheets.Cells[rb + 7, 11] = mesec.Month.ToString()+"/31/"+mesec.Year.ToString();
                                        break;
                                    case 4:
                                    case 6:
                                    case 9:
                                    case 11:
                                        oSheets.Cells[rb + 7, 11] = mesec.Month.ToString() + "/30/" + mesec.Year.ToString();
                                        break;
                                    case 2:
                                        if (DateTime.Now.Year % 4 == 0)
                                        {
                                            oSheets.Cells[rb + 7, 11] = mesec.Month.ToString() + "/29/" + mesec.Year.ToString();
                                        }
                                        else
                                        {
                                            oSheets.Cells[rb + 7, 11] = mesec.Month.ToString() + "/28/" + mesec.Year.ToString();
                                        }
                                        break;
                                    default:
                                        oSheets.Cells[rb + 7, 11] = "GRESKA";
                                        break;
                                }
                            }
                        }
                        for (int j = 0; j < 29; j++)
                        {
                            if (zapNizMinuta.ContainsKey(zap.EmployeeID))
                            {
                                oSheets.Cells[rb + 7, 14 + j + k] = Math.Round(zapNizMinuta[zap.EmployeeID][j] / (float)60);
                            }
                        }
                        oSheets.Cells[red, 12] = "=(NETWORKDAYS(J"+red+",K"+red+")*8)";
                        oSheets.Cells[red, 13] = "=ROUND(N"+red+"+O"+red+"+P"+red+"+Q"+red+"+R"+red+"+W"+red+"+X"+red+"+Y"+red+"+Z"+red+"+AA"+red+"+AD"+red+"+AG"+red+"+AH"+red+"+AI"+red+"+AO"+red+",0)";
                        oSheets.Cells[red, 43] = "=(N"+red+"-AP"+red+")/8";
                        oSheets.Cells[red, 44] = "=(N"+red+"+AG"+red+"-AP"+red+")/8";
                        oSheets.Cells[red, 45] = "=L"+red;
                        int min = ioPairProc.BankHoursMonthly(zap.EmployeeID, mesec);
                        int hrs = 0;
                        if (min > 0)
                        {
                            hrs = min / 60;
                            if ((min % 60) >= 30)
                                hrs++;
                        }
                        oSheets.get_Range(oSheets.Cells[red, 37], oSheets.Cells[red, 38]).NumberFormat = "0";
                        oSheets.Cells[red, 37] = hrs.ToString();
                        if (mesec.Month >= 6)
                        {
                            min = ioPairProc.BankHours6Months(zap.EmployeeID, mesec);
                            if (min > 0)
                            {
                                hrs = min / 60;
                                if ((min % 60) >= 30)
                                    hrs++;
                            }
                            else
                                hrs = 0;
                            EmployeeCounterValueTO bankHrs = new EmployeeCounterValue().Find(5, zap.EmployeeID);
                            if (bankHrs.Value - hrs >= 0)
                                oSheets.Cells[red, 38] = hrs.ToString();
                            else
                                oSheets.Cells[red, 38] = 0;
                        }
                        else
                        {
                            oSheets.Cells[red, 38] = "u ovom trenutku ne postoji kumulativ";
                        }
                        oSheets.Cells[red, 32] = "";
                        oSheets.Cells[red, 36] = "";
                        oSheets.Cells[red, 39] = "";
                        oSheets.Cells[red, 40] = "";
                        int radneSubote = ioPairProc.radneSubote(zap.EmployeeID, mesec);
                        string komentar = "";
                        if (radneSubote > 0)
                        {
                            if (radneSubote == 1)
                            {
                                komentar = "1 radna subota";
                            }
                            else if (radneSubote > 1 && radneSubote < 5)
                            {
                                komentar = radneSubote.ToString() + " radne subote";
                            }
                            else if (radneSubote > 4)
                            {
                                komentar = radneSubote.ToString() + " radnih subota";
                            }

                        }
                        //Microsoft.Office.Interop.Excel.Range rangeComm= oSheets.get_Range("AQ"+red,"AR"+red);
                        //Microsoft.Office.Interop.Excel.Comment comment;
                        //comment.te
                        
                        oSheets.get_Range("L" + red, "AS" + red).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        red++;
                        this.progressBar1.PerformStep();
                    }
                    catch (Exception exc)
                    {
                        desilaSeGreska = true;
                        debug.writeLog(DateTime.Now + " Greska prilikom generisanja izvestaja : " + exc.Message);
                    }
                    //}
                    //else
                    //{
                    //    continue;
                    //}
                }
                //oSheets.get_Range("L8", "AS" + spisakZaposlenih.Count + 7).Columns.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                if (mesec.Month < 6)
                {
                    oSheets.get_Range("AL8", "AL" + spisakZaposlenih.Count + 7).Columns.AutoFit();
                    oSheets.get_Range("AK8", "AK" + spisakZaposlenih.Count + 7).Columns.ColumnWidth=14;
                }
                else
                {
                    oSheets.get_Range("AK8", "AL" + spisakZaposlenih.Count + 7).Columns.ColumnWidth = 14;
                }
                if (desilaSeGreska)
                    MessageBox.Show("Izveštaj nece biti potpun!!!");
                Microsoft.Office.Interop.Excel.Range r = oSheets.get_Range(oSheets.Cells[1, 1], oSheets.Cells[--red, 45]);
                r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                r.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                r = oSheets.get_Range(oSheets.Cells[7, 1], oSheets.Cells[7, 45]);
                r.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);

                this.labelMessage.Text = this.labelMessage.Text = rm.GetString("GotovaObrada", culture);
                this.progressBar1.Visible = false;
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
                        this.labelMessage.Text = this.labelMessage.Text = rm.GetString("MozetePogledatiIzvestaj", culture);
                    }
                }
                else
                {
                    this.labelMessage.Text = this.labelMessage.Text = rm.GetString("NisiSacuvao", culture);
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
            catch (Exception ex1)
            {
                this.progressBar1.Visible = false;
                this.labelMessage.Text = this.labelMessage.Text = rm.GetString("ProbajteOpet", culture);
                debug.writeLog(DateTime.Now + " Greska prilikom generisanja izvestaja : " + ex1.Message);
                MessageBox.Show(ex1.ToString(),"Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        // proveri da li se niz menja i vraca izmenjen
        private int[] CreateArray(int passType, int min, int[] sniz)
        {
            int[] niz = sniz;
            EmployeeTO empl = new EmployeeTO();
            // empl.WorkingUnitID tip
            //   empl.ogr        minuti

            switch (passType)
            {

                // N  - Redovan rad danju u Srbiji
                case 1:
                case 42:
                    niz[0] += min;
                    break;
                // O - Povreda na radu
                case 26: case 40:
                    niz[1] += min;
                    break;
                // P - Bolovanje do 30 dana 65%
                case 25: case 39:
                    niz[2] += min;
                    break;
                // Q - Odrzavanje trudnoce
                case 99: case 90: case 43:
                    niz[3] += min;
                    break;
                //R - Nega deteta preko 3 godine
                case 105:
                    niz[4] += min;
                    break;
                //S - Trudnicko preko 30 dana
                case 100:
                    niz[5] += min;
                    break;
                //T - Porodiljsko
                case 101:
                    niz[6] += min;
                    break;
                //U - Ostalo bolovanje preko 30 dana
                case 102: case 27:
                    niz[7] += min;
                    break;
                //V - Nega deteta do 3 godine
                case 28: case 63:
                    niz[8] += min;
                    break;
                //W - Godisnji odmor
                case 8:
                    niz[9] += min;
                    break;
                //X - Naknada za praznik
                case 10:
                    niz[10] += min;
                    break;
                //Y - Placeno odsustvo --- 79,81,55,95,94,35,34,32,31,13,24,21,20,19,18,17,11,33,30,22,12
                case 79:case 81:case 55:case 95: case 94: case 35:case 34: case 32: case 31: case 13: case 24: case 21: case 20: case 19: case 18: case 17:
                case 11:case 33: case 30:case 22:case 12:
                    niz[11] += min;
                    break;
                //Z - Vojna vezba
                case 72:
                    niz[12] += min;
                    break;
                //AA - COVID bolovanje 100%
                case 104:
                    niz[13] += min;
                    break;
                //AB - Prekovremeni rad 126%
                case 4:
                    niz[14] += min;
                    break;
                //AC - Dodatak za noćni rad 26%
                case 6:
                    niz[15] += min;
                    niz[0] += min;
                    break;
                //AD - Rad na praznik + nocni rad
                case 106:
                    niz[0] += min;
                    niz[15] += min;
                    niz[17] += min;
                    break;
                //AE - Dodatak za Rad na praznik 110%
                case 5:
                    niz[17] += min;
                    niz[0] += min;
                    break;
                //AG - Rad od kuce
                case 98:
                    niz[19] += min;
                    break;
                //AH - Neopravdani izostanak
                case -100:case 14:case 59:
                    niz[20] += min;
                    break;
                //AI - Neplaceno odsustvo
                case 41:
                    niz[21] += min;
                    //AJ -Kontrola casova
                    niz[22] = 0;
                    //AK - Sati za preraspodelu - TEKUCI MESEC- samo za isplatni listic
                    //niz[23] = 0;
                    //AL - Sati za preraspodelu - KUMULATIV  za isplatu 
                    niz[24] = 0;
                    break;
                //AM - Bolovanje po članu 85.zakona
                //case 91:
                //    niz[25] += min;
                //    break;
                //AO - Sluzbeni put
                case 2: case 15:
                    //AN - Sporni sati ( proveriti koji tipovi idu ovde)
                    niz[25] = 0;
                    niz[26] = 0;
                    niz[27] += min;
                    break;
                //AP - Rad u inostranstvu
                case 107:
                    niz[28] += min;
                    break;
                //case 48:
                //    niz[23] += min;
                //    break;
                // DODAJE NOCNI I PREKOVREMENI
                case 108:
                    niz[14] += min;
                    niz[15] += min;
                    break;
                case 109: // RAD NA PRAZNIK + PREKOVREMENI RAD
                    niz[17] += min;
                    niz[14] += min;
                    break;
                case 110: // RAD NA PRAZNIK + PREKOVREMENI RAD + NOCNI RAD
                    niz[17] += min;
                    niz[14] += min;
                    niz[15] += min;
                    break;
            }


            return niz;
        }



        private void dtpMesec_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedM = dtpMesec.Value;

            DateTime currentM = DateTime.Now.Date;

            if (selectedM.Month == currentM.Month)
            {
                this.btnGenerateReport.Enabled = true;
                this.labelMessage.Text = rm.GetString("msgHistoryPassesForecast", culture); //"Izvestaj na osnovu prolazaka do danasnjeg datuma, a od danasnjeg datuma je predvidjanje!";
            }
            else if (selectedM < currentM)
            {
                this.btnGenerateReport.Enabled = true;
                this.labelMessage.Text = rm.GetString("msgHistoryPasses", culture); //"Izvestaj na osnovu prolazaka!";
            }

            else if (selectedM > currentM)
            {
                this.btnGenerateReport.Enabled = false;
                this.labelMessage.Text = rm.GetString("msgForecast", culture);// " Forecast izvestaj! Izvestaj na osnovu  predvidjanja!";
            }
        }

        private void btnSmanjiBankuSati_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Da li ste sigurni da zelite da smanjite banku sati svim radnicima za kumulativ za mesec "+dtpMesec.Value.AddMonths(-6).ToString("MMMM yyyy.")+"?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    labelMessage.Text = labelMessage.Text = rm.GetString("BankaSatiUpdate",culture);
                    DateTime mesec = new DateTime(dtpMesec.Value.Year, dtpMesec.Value.Month, 1, 0, 0, 0);
                    Employee empl = new Employee();
                    IOPairProcessed ioproc = new IOPairProcessed();
                    List<int> listaID = empl.getEmplIDsForBankHours(mesec);
                    this.progressBar1.Value = 0;
                    this.progressBar1.Step = 1;
                    this.progressBar1.Maximum = listaID.Count;
                    this.progressBar1.Visible = true;
                    foreach (int ID in listaID)
                    {
                        ioproc.BankHours6MonthsPay(ID, mesec);
                        progressBar1.PerformStep();
                    }
                    progressBar1.Visible = false;
                    MessageBox.Show("Banka sati uspesno umanjena za kumulativ!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    labelMessage.Text = labelMessage.Text = rm.GetString("BankaSatiUpdateFinished",culture);
                }
                catch (Exception ex)
                {
                    labelMessage.Text = "GRESKA!!!";
                    this.progressBar1.Visible = false;
                    debug.writeLog(DateTime.Now + " Greska prilikom generisanja izvestaja : " + ex.Message);
                    MessageBox.Show("GRESKA!!! " + ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPeriodicalReport_Click(object sender, EventArgs e)
        {
            try
            {
                Employee zaPozivanjeFja = new Employee();
                List<EmployeeTO> spisakProlazaka = new List<EmployeeTO>();
                List<EmployeeTO> spisakZaposlenih = new List<EmployeeTO>();

                spisakProlazaka = zaPozivanjeFja.ProlasciZaForecast(dtpFrom.Value, dtpTo.Value);
                List<EmployeeTO> listaa = new List<EmployeeTO>();


                Dictionary<int, int[]> zapNizMinuta = new Dictionary<int, int[]>();

                int lastEmplID = 0;


                int brZap = zaPozivanjeFja.BrZaposlenihPeriodicno(dtpFrom.Value, dtpTo.Value);


                int[][] niz = new int[brZap][];
                for (int i = 0; i < brZap; i++)
                    niz[i] = new int[29];
                int[] poslednjaVrednostNiza = new int[28];

                int bf = 0;
                int a = 0;
                for (int i = 0; i < spisakProlazaka.Count; i++)
                {
                    if (spisakProlazaka[i].EmployeeID != lastEmplID)
                    {
                        if (lastEmplID != 0)
                        {
                            bool uneo1 = false;
                            int pbf = bf;
                            if (uneo1 || pbf == bf)
                            {
                                zapNizMinuta.Add(lastEmplID, poslednjaVrednostNiza); //unos prethodnog zaposlenog i njegov niz minuta 
                                a++;
                            }

                        }

                        //prvi unos 
                        Array.Clear(niz[a], 0, niz[a].Length); // svaki element je 0, samo za prvi unos                        
                        lastEmplID = spisakProlazaka[i].EmployeeID;
                        poslednjaVrednostNiza = CreateArray(spisakProlazaka[i].WorkingUnitID, spisakProlazaka[i].OrgUnitID, niz[a]); // prvi unos minuta odredjenog tipa prolaska od datog zaposlenog

                        EmployeeTO empl = new EmployeeTO();
                        empl.EmployeeID = spisakProlazaka[i].EmployeeID;
                        empl.FirstName = spisakProlazaka[i].FirstName;
                        empl.WorkingUnitName = spisakProlazaka[i].WorkingUnitName;
                        empl.LastName = spisakProlazaka[i].LastName;
                        empl.CreatedBy = spisakProlazaka[i].CreatedBy;
                        empl.OrgUnitName = spisakProlazaka[i].OrgUnitName;
                        spisakZaposlenih.Add(empl);
                    }
                    else
                    {
                        niz[a] = poslednjaVrednostNiza;
                        poslednjaVrednostNiza = CreateArray(spisakProlazaka[i].WorkingUnitID, spisakProlazaka[i].OrgUnitID, niz[a]); // svaki sledeci unos minuta odredjenog tipa prolaska od datog zaposleno
                    }

                }
                zapNizMinuta.Add(lastEmplID, poslednjaVrednostNiza);


                string logFilePath = Util.Constants.LPath + "\\" + "template.xls";
                this.progressBar2.Value = 0;
                this.progressBar2.Step = 1;
                this.progressBar2.Maximum = spisakZaposlenih.Count;
                this.progressBar2.Visible = true;
                //this.labelMessage.Text = this.labelMessage.Text = rm.GetString("NeDiraj", culture);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook wb =
                    excel.Workbooks.Open(logFilePath, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, true,
                    Type.Missing, Type.Missing, true, false, Type.Missing, false, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet oSheets;
                oSheets = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets[1];
                int rb = 0, red = 8;
                bool desilaSeGreska = false;
                EmployeeAsco4 emplAsco4 = new EmployeeAsco4();
                Employee emplBase = new Employee();
                IOPairProcessed ioPairProc = new IOPairProcessed();
                foreach (EmployeeTO zap in spisakZaposlenih)
                {
                    try
                    {
                        int k = 0;
                        rb++;
                        //Informacije o zaposlenom
                        oSheets.Cells[rb + 7, 1] = zap.EmployeeID;//redni broj
                        oSheets.Cells[rb + 7, 2] = zap.EmployeeID;
                        if (zap.CreatedBy == "INDIRECT")
                        {
                            oSheets.Cells[rb + 7, 3] = "INDIRECT";
                            oSheets.Cells[rb + 7, 4] = "White Collar";
                        }
                        else if (zap.CreatedBy == "DIRECT")
                        {
                            oSheets.Cells[rb + 7, 3] = "DIRECT";
                            oSheets.Cells[rb + 7, 4] = "Blue Collar";
                        }
                        else
                        {
                            oSheets.Cells[rb + 7, 3] = "/";
                            oSheets.Cells[rb + 7, 4] = "/";
                        }
                        oSheets.Cells[rb + 7, 5] = zap.WorkingUnitName;
                        oSheets.Cells[rb + 7, 6] = zap.WorkingUnitName;
                        oSheets.Cells[rb + 7, 7] = zap.OrgUnitName;
                        oSheets.Cells[rb + 7, 8] = zap.FirstName;
                        oSheets.Cells[rb + 7, 9] = zap.LastName;
                        List<EmployeeAsco4TO> listEmplAsco4 = emplAsco4.Search(zap.EmployeeID.ToString());
                        foreach (EmployeeAsco4TO item in listEmplAsco4)
                        {
                            if (item.DatetimeValue2.Month == dtpFrom.Value.Month && item.DatetimeValue2.Year == dtpFrom.Value.Year && item.DatetimeValue2.Day>=dtpFrom.Value.Day)
                            {
                                oSheets.Cells[rb + 7, 10] = item.DatetimeValue2.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                oSheets.Cells[rb + 7, 10] = dtpFrom.Value.ToString("MM/dd/yyyy");
                            }
                            if (item.DatetimeValue3.Month == dtpTo.Value.Month && item.DatetimeValue3.Year == dtpTo.Value.Year && item.DatetimeValue3.Day<=dtpTo.Value.Day)
                            {
                                oSheets.Cells[rb + 7, 11] = item.DatetimeValue3.AddDays(-1).ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                oSheets.Cells[rb + 7, 11] = dtpTo.Value.ToString("MM/dd/yyyy");
                            }
                        }
                        for (int j = 0; j < 29; j++)
                        {

                            if (zapNizMinuta.ContainsKey(zap.EmployeeID))
                            {
                                oSheets.Cells[rb + 7, 14 + j + k] = Math.Round(zapNizMinuta[zap.EmployeeID][j] / (float)60);
                            }
                        }
                        oSheets.Cells[red, 12] = "=(NETWORKDAYS(J" + red + ",K" + red + ")*8)";
                        oSheets.Cells[red, 13] = "=ROUND(N" + red + "+O" + red + "+P" + red + "+Q" + red + "+R" + red + "+W" + red + "+X" + red + "+Y" + red + "+Z" + red + "+AA" + red + "+AD" + red + "+AG" + red + "+AH" + red + "+AI" + red + ",0)";
                        oSheets.Cells[red, 43] = "=(N" + red + "-AP" + red + ")/8";
                        oSheets.Cells[red, 44] = "=(N" + red + "+AG" + red + "-AP" + red + ")/8";
                        oSheets.Cells[red, 45] = "=L" + red;
                        int min = ioPairProc.BankHoursPeriodical(zap.EmployeeID, dtpFrom.Value,dtpTo.Value);
                        int hrs = 0;
                        if (min > 0)
                        {
                            hrs = min / 60;
                            if ((min % 60) >= 30)
                                hrs++;
                        }
                        oSheets.get_Range(oSheets.Cells[red, 37], oSheets.Cells[red, 38]).NumberFormat = "0";
                        oSheets.Cells[red, 37] = hrs.ToString();
                        oSheets.Cells[red, 38] = "U ovom izvestaju ne postoji kumulativ banka sati!";
                        oSheets.Cells[red, 32] = "";
                        oSheets.Cells[red, 36] = "";
                        oSheets.Cells[red, 39] = "";
                        oSheets.Cells[red, 40] = "";
                        //int radneSubote = ioPairProc.radneSubote(zap.EmployeeID, mesec);
                        //string komentar = "";
                        //if (radneSubote > 0)
                        //{
                        //    if (radneSubote == 1)
                        //    {
                        //        komentar = "1 radna subota";
                        //    }
                        //    else if (radneSubote > 1 && radneSubote < 5)
                        //    {
                        //        komentar = radneSubote.ToString() + " radne subote";
                        //    }
                        //    else if (radneSubote > 4)
                        //    {
                        //        komentar = radneSubote.ToString() + " radnih subota";
                        //    }

                        //}
                        oSheets.get_Range("L" + red, "AS" + red).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        red++;
                        this.progressBar2.PerformStep();
                    }
                    catch (Exception exc)
                    {
                        desilaSeGreska = true;
                        debug.writeLog(DateTime.Now + " Greska prilikom generisanja izvestaja : " + exc.Message);
                    }
                    //}
                    //else
                    //{
                    //    continue;
                    //}
                }
                //oSheets.get_Range("L8", "AS" + spisakZaposlenih.Count + 7).Columns.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                oSheets.get_Range("AL8", "AL" + spisakZaposlenih.Count + 7).Columns.AutoFit();
                oSheets.get_Range("AK8", "AK" + spisakZaposlenih.Count + 7).Columns.ColumnWidth = 14;
                if (desilaSeGreska)
                    MessageBox.Show("Izveštaj nece biti potpun!!!");
                Microsoft.Office.Interop.Excel.Range r = oSheets.get_Range(oSheets.Cells[1, 1], oSheets.Cells[--red, 45]);
                r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                r.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                r = oSheets.get_Range(oSheets.Cells[7, 1], oSheets.Cells[7, 45]);
                r.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);

                //this.labelMessage.Text = this.labelMessage.Text = rm.GetString("GotovaObrada", culture);
                this.progressBar1.Visible = false;
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
                progressBar2.Visible = false;
            }
            catch (Exception ex1)
            {
                this.progressBar1.Visible = false;
                this.labelMessage.Text = this.labelMessage.Text = rm.GetString("ProbajteOpet", culture);
                debug.writeLog(DateTime.Now + " Greska prilikom generisanja izvestaja : " + ex1.Message);
                MessageBox.Show(ex1.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
