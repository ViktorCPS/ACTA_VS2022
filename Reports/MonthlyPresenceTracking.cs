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

namespace Reports
{
    public partial class MonthlyPresenceTracking : Form
    {
        // Language settings
        private CultureInfo culture;
        private ResourceManager rm;
        string Izvestaj;
        DebugLog debug;

        public MonthlyPresenceTracking()
        {
            InitializeComponent();
            this.CenterToScreen();


            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
            setLanguage();

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
                debug.writeLog(DateTime.Now + " Reports.MonthlyPresenceTracking.setLanguage() " + ex.Message);
            }
        }

        private void MonthlyPresenceTracking_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["Izvestaj"] != null)
            {
                Izvestaj = (string)ConfigurationManager.AppSettings["Izvestaj"];
            }
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Provera da li postoje neopredeljeni prolasci

                IOPairProcessed ioPair = new IOPairProcessed();
                IOPairProcessedTO parTO = new IOPairProcessedTO();
                DateTime selected = dtpMesec.Value;

                DateTime from = new DateTime(selected.Year, selected.Month, 1, 0, 0, 0);

                DateTime to = from.AddMonths(1);
                List<IOPairProcessedTO> ioPairsList = new List<IOPairProcessedTO>();

                ioPairsList = ioPair.ProlasciPoTipuProlaska(from, to, "-1000,88");

                if (ioPairsList.Count > 0)
                {

                    string s = "", spisak = "";
                    int a = 1;
                    EmployeeTO zapTO = new EmployeeTO();
                    bool viseOd30 = false;
                    foreach (IOPairProcessedTO prolazak in ioPairsList)
                    {

                        if (zapTO.EmployeeID != prolazak.EmployeeID)
                        {
                            Employee zaposleni = new Employee();
                            zapTO = zaposleni.Find(prolazak.EmployeeID.ToString());
                        }

                        s = zapTO.LastName + " " + zapTO.FirstName + " - " + prolazak.IOPairDate.ToString("dd.MM.yyyy");
                        if (s.Length < 30)
                        {
                            s += "\t";
                        }
                        if (a++ % 2 == 0)
                        {
                            s += "\n";
                        }
                        else
                        {
                            s += "\t";
                        }
                        if (a < 30)
                        {
                            spisak += s;
                        }
                        else
                        {
                            viseOd30 = true;
                            break;
                        }
                    }
                    if (viseOd30)
                    {
                        spisak += "\nI još " + (ioPairsList.Count - 30) + " drugih...";
                    }
                    if (ioPairsList.Count > 1)
                    {
                        MessageBox.Show("U parovima za ovaj mesec postoje Neopredeljeni parovi:\n\n" + spisak);
                    }
                    else
                    {
                        MessageBox.Show("U parovima za ovaj mesec postoji Neopredeljeni par!!!\n\n" + spisak);
                    }
                }


                ioPairsList = ioPair.SearchForMonthlyTypesOfPasses(from, to);
                /*                
                 *      //pp.IOPairDate                            pair.IOPairDate 
                        //e.employee_id                            pair.EmployeeID 
                        //e.first_name                            pair.ConfirmedBy 
                        //e.last_name                            pair.VerifiedBy 
                        //working unit                            pair.LocationID                         
                        //Organization unit                        pair.IsWrkHrsCounter 
                        //Pass type                            pair.PassTypeID 
                        //Description                            pair.Desc 
                        //Start time                            pair.StartTime     
                        //End time                            pair.EndTime
                */

                string path = Izvestaj + "Spisak prolazaka za " + from.ToString("MMMM yyyy") + ".csv";
                double sati = 0;
                string csv = string.Empty;
                csv = "Br. reda;Datum;ID broj kartice;ID radnika;Zaposleni;GD1;GD2;Šifra prisustva;Opis prisustva;Br. sati";

                int i = 1;

                using (StreamWriter stream = File.CreateText(path))
                {
                    stream.WriteLine(csv, Encoding.UTF8);

                    List<IOPairProcessedTO> paroviUDanu = new List<IOPairProcessedTO>();
                    bool prvi = true;

                    foreach (IOPairProcessedTO par in ioPairsList)
                    {
                        if (par.StartTime.ToString("HH:mm") == "00:00")
                        {
                            par.IOPairDate = par.IOPairDate.AddDays(-1);
                        }

                        if (par.IOPairDate >= from)
                        {
                            if (prvi)
                            {
                                paroviUDanu.Add(par);
                                prvi = false;
                            }
                            else if (par.IOPairDate == paroviUDanu[0].IOPairDate)//proveri
                            {
                                paroviUDanu.Add(par);
                            }
                            else
                            {
                                i = Ispisi(stream, paroviUDanu, i);
                                paroviUDanu.Clear();
                                paroviUDanu.Add(par);
                            }
                        }

                        if (ioPairsList[ioPairsList.Count - 1] == par)
                        {
                            i = Ispisi(stream, paroviUDanu, i);
                        }
                    }
                }
                MessageBox.Show("Izveštaj je uspešno generisan i napravljen na:\n\t" + path);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se greška:\n" + ex.Message);
                debug.writeLog(DateTime.Now + " Reports.MonthlyPresenceTracking.btnGenerateReport_Click() " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private double ZaokruziSate(double sati)
        {
            try
            {
                double ostatak = sati % 1;
                if (ostatak < 0.125)
                {
                    sati -= ostatak;
                }
                else if (ostatak >= 0.125 && ostatak < 0.375)
                {
                    sati += 0.25 - ostatak;
                }
                else if (ostatak >= 0.375 && ostatak < 0.625)
                {
                    sati += 0.5 - ostatak;
                }
                else if (ostatak >= 0.625 && ostatak < 0.875)
                {
                    sati += 0.75 - ostatak;
                }
                else
                {
                    sati += 1 - ostatak;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog("Reports.MonthlyPresenceTracking.ZaokruziSate()" + DateTime.Now + " " + ex.Message);
            }
            return sati;
        }


        private int Ispisi(StreamWriter stream, List<IOPairProcessedTO> paroviUDanu, int i)
        {
            try
            {
                int pt = -5; //neka nerealna vrednost
                bool upis = false;
                bool pp = true;//pp-prvi prolazak
                string csv = "";
                TimeSpan vreme = new TimeSpan();
                double sati = 0;
                foreach (IOPairProcessedTO p in paroviUDanu)
                {
                    if (p.PassTypeID == pt)
                    {
                        vreme = p.EndTime.Subtract(p.StartTime);
                        sati += vreme.Hours + Convert.ToDouble(vreme.Minutes) / 60;
                        sati = ZaokruziSate(sati);
                        /* string sVreme = p.IOPairDate.ToString("dd\\/MM\\/yyyy");
                        csv = ";" + sVreme + ";;" + p.EmployeeID + ";" +
                    p.ConfirmedBy + " " + p.VerifiedBy + ";" + p.LocationID + ";" +
                    p.IsWrkHrsCounter + ";" + p.PassTypeID + ";" + p.Desc + ";" + sati; */
                        csv = ";" + p.IOPairDate.ToString("dd\\/MM\\/yyyy") + ";;" + p.EmployeeID + ";" +
                     p.ConfirmedBy + " " + p.VerifiedBy + ";" + p.LocationID + ";" +
                     p.IsWrkHrsCounter + ";" + p.PassTypeID + ";" + p.Desc + ";" + sati;
                    }
                    else
                    {
                        if (!pp)
                        {
                            if (sati != 0)
                            {
                                csv = (i++) + csv;
                                stream.WriteLine(csv, Encoding.UTF8);
                            }
                            upis = false;
                        }
                        else
                        {
                            pp = false;
                        }
                        vreme = p.EndTime.Subtract(p.StartTime);
                        sati = vreme.Hours + Convert.ToDouble(vreme.Minutes) / 60;
                        sati = ZaokruziSate(sati);
                        /*
                        string sVreme = p.IOPairDate.ToString("dd\\/MM\\/yyyy");
                        csv = ";" + sVreme + ";;" + p.EmployeeID + ";" +
                    p.ConfirmedBy + " " + p.VerifiedBy + ";" + p.LocationID + ";" +
                    p.IsWrkHrsCounter + ";" + p.PassTypeID + ";" + p.Desc + ";" + sati; */
                        csv = ";" + p.IOPairDate.ToString("dd\\/MM\\/yyyy") + ";;" + p.EmployeeID + ";" +
                     p.ConfirmedBy + " " + p.VerifiedBy + ";" + p.LocationID + ";" +
                     p.IsWrkHrsCounter + ";" + p.PassTypeID + ";" + p.Desc + ";" + sati;
                        pt = p.PassTypeID;
                    }

                }
                if (sati != 0)
                {
                    csv = (i++) + csv;
                    stream.WriteLine(csv, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog("Reports.MonthlyPresenceTracking.Ispisi() " + DateTime.Now + " " + ex.Message);

            }
            return i;
        }

    }
}
