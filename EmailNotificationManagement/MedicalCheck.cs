using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Globalization;
using Common;
using TransferObjects;
using Util;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace EmailNotificationManagement
{
    public class MedicalCheck
    {
        Dictionary<int, Dictionary<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>>> dictionaryBC = new Dictionary<int, Dictionary<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>>>();

        const int newCoupon = 1;
        const int changedCoupon = 2;
        const int deletedCoupon = 3;
        Dictionary<int, WorkingUnitTO> WUnits = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, MedicalCheckPointTO> mcPointDict = new Dictionary<int, MedicalCheckPointTO>();
        Dictionary<int, RiskTO> riskDict = new Dictionary<int, RiskTO>();
        Dictionary<int, VaccineTO> vaccineDict = new Dictionary<int, VaccineTO>();
        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();
        DebugLog log;

        public void MedicalCheckSender(System.Net.Mail.SmtpClient smtp, string emailAddress)
        {
            try
            {
                log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");

                MedicalCheckVisitHdrTO medicalCheckVisitHdr = new MedicalCheckVisitHdrTO();
                medicalCheckVisitHdr.FlagEmail = (int)Constants.EmailFlag.NOK;
                MedicalCheckVisitHdr mcvh = new MedicalCheckVisitHdr();
                mcvh.VisitHdrTO = medicalCheckVisitHdr;
                List<MedicalCheckVisitHdrTO> listMedical = mcvh.SearchMedicalCheckVisitHeaders();

                if (listMedical.Count > 0)
                {
                    WriteLog("Found new MedicalCheckVisitHdr to send: " + listMedical.Count);
                    // get all working units
                    WUnits = getWUnits();

                    // get all medical check points
                    mcPointDict = new MedicalCheckPoint().SearchMedicalCheckPointsDictionary();

                    // get all risks
                    riskDict = new Risk().SearchRisksDictionary();

                    // get all vaccines
                    vaccineDict = new Vaccine().SearchVaccinesDictionary();
                    // get all time schemas
                    schemas = new TimeSchema().getDictionary();

                }

                foreach (MedicalCheckVisitHdrTO medicalCheck in listMedical)
                {
                    if (medicalCheck.FlagChange == (int)Constants.EmailFlag.NOK)
                    {
                        if (medicalCheck.Status.Equals(Constants.MedicalCheckVisitStatus.RND.ToString()))
                        {
                            SendNewCoupon(smtp, emailAddress, medicalCheck);
                        }
                        else if (medicalCheck.Status.Equals(Constants.MedicalCheckVisitStatus.DELETED.ToString()))
                        {
                            SendDeletedCoupon(smtp, emailAddress, medicalCheck);
                        }
                    }
                    else if (medicalCheck.FlagChange == (int)Constants.EmailFlag.OK)
                    {
                        if (medicalCheck.Status.Equals(Constants.MedicalCheckVisitStatus.RND.ToString()))
                        {
                            SendChangedCoupon(smtp, emailAddress, medicalCheck);
                        }
                        else if (medicalCheck.Status.Equals(Constants.MedicalCheckVisitStatus.DELETED.ToString()))
                        {
                            SendDeletedCoupon(smtp, emailAddress, medicalCheck);
                        }
                    }

                }
                foreach (KeyValuePair<int, Dictionary<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>>> pair in dictionaryBC)
                {
                    WriteLog("Started sending for BC's supervisors");
                    if (pair.Key == newCoupon)
                    {
                        WriteLog("BC new visit found");
                        foreach (KeyValuePair<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>> pairSupervisor in pair.Value)
                        {

                            int supervisor_id = pairSupervisor.Key;
                            EmployeeTO Supervisor = new Employee().Find(supervisor_id.ToString());
                            EmployeeAsco4 supervisorAsco = new EmployeeAsco4();
                            supervisorAsco.EmplAsco4TO.EmployeeID = supervisor_id;
                            List<EmployeeAsco4TO> supervisorListAsco = supervisorAsco.Search();
                            EmployeeAsco4TO supervisorAscoTO = new EmployeeAsco4TO();
                            if (supervisorListAsco.Count == 1)
                            {

                                supervisorAscoTO = supervisorListAsco[0];
                            }
                            string mail = supervisorAscoTO.NVarcharValue3;

                            if (!mail.Equals(""))
                            {
                                ApplUserTO user = new ApplUser().Find(supervisorAscoTO.NVarcharValue5);

                                ResourceManager rm = new ResourceManager("EmailNotificationManagement.Resource", typeof(NotificationManager).Assembly);
                                CultureInfo culture = CultureInfo.CreateSpecificCulture(getLanguage(user));

                                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

                                mailMessage.To.Add(mail);
                                mailMessage.From = new System.Net.Mail.MailAddress(emailAddress);
                                mailMessage.Subject = rm.GetString("titleMedical", culture);

                                string message = "";
                                message += "<html><body>" + rm.GetString("Employee", culture) + "  <b>" + Supervisor.FirstAndLastName + ", </b><br /><br />";

                                message += rm.GetString("newCouponBC1", culture) + "<br /><br />";

                                int num = 0;
                                string visit_id = "";
                                foreach (KeyValuePair<EmployeeTO, List<MedicalCheckVisitHdrTO>> pairEmpl in pairSupervisor.Value)
                                {
                                    num++;
                                    message += num + ". " + pairEmpl.Key.FirstAndLastName + "<br />";
                                    foreach (MedicalCheckVisitHdrTO medicalCheck in pairEmpl.Value)
                                    {
                                        WriteLog(num + ". " + medicalCheck.VisitID);
                                        visit_id += medicalCheck.VisitID + ",";
                                    }

                                }
                                if (visit_id.Length > 0)
                                    visit_id = visit_id.Substring(0, visit_id.Length - 1);

                                message += "<br />" + rm.GetString("newCouponBC2", culture) + "<br />";
                                message += rm.GetString("newCouponBC3", culture) + "<br /><br />";

                                message += "<table><tr><td> </td></tr><tr><td> </td></tr><tr><td><font size=2 face=\"Helvetica\" >" + rm.GetString("pleaseDoNot", culture) + "</font></td></tr>";

                                message += "<tr><td><font size=1 face=\"Helvetica\"color=black> This e-mail is automatically generated by ActA System (powered by SDDITG,  www.sdditg.com). </font></td></tr>";
                                message += "<tr><td><font size=1 face=\"Helvetica\"color=black>Generated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " </font></td></tr>";
                                bool fiatLogoFound = File.Exists(Constants.FiatServicesLogoPath);
                                if (fiatLogoFound)
                                {
                                    message += "<tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td><img src='cid:logo_bottom' align=\"left\"></td></tr><tr><td><font size=1 face=\"Helvetica\"color=black>hr-services-serbia@fiatservices.com.</font></td></tr></table></body></html>";
                                    System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html"));
                                    System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(Constants.FiatServicesLogoPath, "image/jpeg"); // + sHeaderImg);     
                                    logo.ContentId = "logo_bottom";
                                    htmlView.LinkedResources.Add(logo);
                                    mailMessage.AlternateViews.Add(htmlView);

                                }
                                mailMessage.IsBodyHtml = true;
                                mailMessage.Body = message;

                                try
                                {
                                    smtp.Send(mailMessage);
                                    WriteLog("Mail sent. Employee: " + pairSupervisor.Key + ", mail: " + mail);
                                    foreach (string visit in visit_id.Split(','))
                                    {
                                        MedicalCheckVisitHdrTO medHDR = new MedicalCheckVisitHdrTO();
                                        medHDR.VisitID = uint.Parse(visit);
                                        medHDR.FlagEmail = (int)Constants.EmailFlag.OK;
                                        medHDR.FlagEmailCratedTime = DateTime.Now;
                                        MedicalCheckVisitHdr medical = new MedicalCheckVisitHdr();
                                        medical.VisitHdrTO = medHDR;
                                        medical.Update(true);

                                    }
                                    WriteLog("Visits updated. ");
                                }
                                catch (Exception ex)
                                {

                                    WriteLog("Mail not sent. Employee: " + pairSupervisor.Key + " " + ex.Message);
                                }
                            }
                            else
                            {

                                WriteLog("Mail Null. Employee: " + pairSupervisor.Key);
                            }
                        }
                    }
                    else if (pair.Key == changedCoupon)
                    {
                        WriteLog("BC changed visit found");
                        foreach (KeyValuePair<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>> pairSupervisor in pair.Value)
                        {

                            int supervisor_id = pairSupervisor.Key;
                            EmployeeTO Supervisor = new Employee().Find(supervisor_id.ToString());
                            EmployeeAsco4 supervisorAsco = new EmployeeAsco4();
                            supervisorAsco.EmplAsco4TO.EmployeeID = supervisor_id;
                            List<EmployeeAsco4TO> supervisorListAsco = supervisorAsco.Search();
                            EmployeeAsco4TO supervisorAscoTO = new EmployeeAsco4TO();
                            if (supervisorListAsco.Count == 1)
                            {

                                supervisorAscoTO = supervisorListAsco[0];
                            }
                            string mail = supervisorAscoTO.NVarcharValue3;
                            if (!mail.Equals(""))
                            {
                                ApplUserTO user = new ApplUser().Find(supervisorAscoTO.NVarcharValue5);

                                ResourceManager rm = new ResourceManager("EmailNotificationManagement.Resource", typeof(NotificationManager).Assembly);
                                CultureInfo culture = CultureInfo.CreateSpecificCulture(getLanguage(user));


                                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

                                mailMessage.To.Add(mail);
                                mailMessage.From = new System.Net.Mail.MailAddress(emailAddress);
                                mailMessage.Subject = rm.GetString("titleMedical", culture);

                                string message = "";
                                message += "<html><body>" + rm.GetString("Employee", culture) + "  <b>" + Supervisor.FirstAndLastName + ", </b><br /><br />";

                                message += rm.GetString("changedCouponBC1", culture) + "<b>" + rm.GetString("changedCouponBC2", culture) + "</b><b>" + rm.GetString("changedCouponBC3", culture) + "</b>" + rm.GetString("changedCouponBC4", culture) + "<br /><br />";

                                int num = 0;
                                string visit_id = "";
                                foreach (KeyValuePair<EmployeeTO, List<MedicalCheckVisitHdrTO>> pairEmpl in pairSupervisor.Value)
                                {
                                    foreach (MedicalCheckVisitHdrTO medicalCheck in pairEmpl.Value)
                                    {
                                        MedicalCheckVisitHdrHist medicalHistory = new MedicalCheckVisitHdrHist();
                                        medicalHistory.VisitHdrHistTO.VisitID = medicalCheck.VisitID;
                                        medicalHistory.VisitHdrHistTO.FlagEmail = (int)Constants.EmailFlag.OK;
                                        List<MedicalCheckVisitHdrHistTO> listMed = medicalHistory.SearchMedicalCheckVisitHeadersHistory();
                                        if (listMed.Count > 0)
                                        {
                                            MedicalCheckVisitHdrHistTO medicalHistoryTO = listMed[listMed.Count - 1];
                                            num++;
                                            message += num + ". " + pairEmpl.Key.FirstAndLastName + " (ID: " + medicalHistory.VisitHdrHistTO.VisitID + rm.GetString("changedCoupon2", culture) + " " + medicalHistory.VisitHdrHistTO.ScheduleDate.ToString(Constants.dateFormat + " " + Constants.timeFormat) + "). <br />";
                                            WriteLog(num + ". " + medicalCheck.VisitID);
                                        }
                                        visit_id += medicalCheck.VisitID + ",";
                                    }
                                }

                                if (visit_id.Length > 0)
                                    visit_id = visit_id.Substring(0, visit_id.Length - 1);

                                message += "<br />" + rm.GetString("newCouponBC2", culture) + "<br /><br />";
                                message += rm.GetString("newCouponBC3", culture) + "<br /><br />";

                                message += "<table><tr><td> </td></tr><tr><td> </td></tr><tr><td><font size=2 face=\"Helvetica\" >" + rm.GetString("pleaseDoNot", culture) + "</font></td></tr>";

                                message += "<tr><td><font size=1 face=\"Helvetica\"color=black> This e-mail is automatically generated by ActA System (powered by SDDITG,  www.sdditg.com). </font></td></tr>";
                                message += "<tr><td><font size=1 face=\"Helvetica\"color=black>Generated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " </font></td></tr>";
                                bool fiatLogoFound = File.Exists(Constants.FiatServicesLogoPath);
                                if (fiatLogoFound)
                                {
                                    message += "<tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td><img src='cid:logo_bottom' align=\"left\"></td></tr><tr><td><font size=1 face=\"Helvetica\"color=black>hr-services-serbia@fiatservices.com.</font></td></tr></table></body></html>";
                                    System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html"));
                                    System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(Constants.FiatServicesLogoPath, "image/jpeg"); // + sHeaderImg);     
                                    logo.ContentId = "logo_bottom";
                                    htmlView.LinkedResources.Add(logo);
                                    mailMessage.AlternateViews.Add(htmlView);

                                }
                                mailMessage.IsBodyHtml = true;
                                mailMessage.Body = message;


                                try
                                {
                                    smtp.Send(mailMessage);
                                    WriteLog("Mail sent. Employee: " + pairSupervisor.Key + ", mail: " + mail);
                                    foreach (string visit in visit_id.Split(','))
                                    {
                                        MedicalCheckVisitHdrTO medHDR = new MedicalCheckVisitHdrTO();
                                        medHDR.VisitID = uint.Parse(visit);
                                        medHDR.FlagEmail = (int)Constants.EmailFlag.OK;
                                        medHDR.FlagEmailCratedTime = DateTime.Now;
                                        MedicalCheckVisitHdr medical = new MedicalCheckVisitHdr();
                                        medical.VisitHdrTO = medHDR;
                                        medical.Update(true);


                                    } WriteLog("Visits updated.");
                                }
                                catch (Exception ex)
                                {

                                    WriteLog("Mail not sent. Employee: " + pairSupervisor.Key + " " + ex.Message);
                                }

                            }
                            else
                            {

                                WriteLog("Mail Null. Employee: " + pairSupervisor.Key);
                            }
                        }
                    }
                    else if (pair.Key == deletedCoupon)
                    {
                        WriteLog("BC deleted visit found");
                        foreach (KeyValuePair<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>> pairSupervisor in pair.Value)
                        {

                            int supervisor_id = pairSupervisor.Key;
                            EmployeeTO Supervisor = new Employee().Find(supervisor_id.ToString());
                            EmployeeAsco4 supervisorAsco = new EmployeeAsco4();
                            supervisorAsco.EmplAsco4TO.EmployeeID = supervisor_id;
                            List<EmployeeAsco4TO> supervisorListAsco = supervisorAsco.Search();
                            EmployeeAsco4TO supervisorAscoTO = new EmployeeAsco4TO();
                            if (supervisorListAsco.Count == 1)
                            {

                                supervisorAscoTO = supervisorListAsco[0];
                            }
                            string mail = supervisorAscoTO.NVarcharValue3;
                            if (!mail.Equals(""))
                            {
                                ApplUserTO user = new ApplUser().Find(supervisorAscoTO.NVarcharValue5);

                                ResourceManager rm = new ResourceManager("EmailNotificationManagement.Resource", typeof(NotificationManager).Assembly);
                                CultureInfo culture = CultureInfo.CreateSpecificCulture(getLanguage(user));


                                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                                mailMessage.To.Add(mail);
                                mailMessage.From = new System.Net.Mail.MailAddress(emailAddress);
                                mailMessage.Subject = rm.GetString("titleMedical", culture);

                                string message = "";
                                message += "<html><body>" + rm.GetString("Employee", culture) + "  <b>" + Supervisor.FirstAndLastName + ", </b><br /><br />";

                                message += rm.GetString("deletedCouponBC1", culture) + "<b>" + rm.GetString("deletedCouponBC2", culture) + "</b><b>" + rm.GetString("deletedCouponBC3", culture) + "</b>" + rm.GetString("deletedCouponBC4", culture) + "<br /><br />";

                                int num = 0;
                                string visit_id = "";
                                foreach (KeyValuePair<EmployeeTO, List<MedicalCheckVisitHdrTO>> pairEmpl in pairSupervisor.Value)
                                {
                                    foreach (MedicalCheckVisitHdrTO medicalCheck in pairEmpl.Value)
                                    {
                                        num++;
                                        message += num + ". " + pairEmpl.Key.FirstAndLastName + " (ID: " + medicalCheck.VisitID + rm.GetString("changedCoupon2", culture) + " " + medicalCheck.ScheduleDate.ToString(Constants.dateFormat + " " + Constants.timeFormat) + "). <br />";
                                        WriteLog(num + ". " + medicalCheck.VisitID);

                                        visit_id += medicalCheck.VisitID + ",";
                                    }
                                }
                                if (visit_id.Length > 0)
                                    visit_id = visit_id.Substring(0, visit_id.Length - 1);

                                message += "<br />" + rm.GetString("newCouponBC3", culture) + "<br /><br />";

                                message += "<table><tr><td> </td></tr><tr><td> </td></tr><tr><td><font size=2 face=\"Helvetica\" >" + rm.GetString("pleaseDoNot", culture) + "</font></td></tr>";

                                message += "<tr><td><font size=1 face=\"Helvetica\"color=black> This e-mail is automatically generated by ActA System (powered by SDDITG,  www.sdditg.com). </font></td></tr>";
                                message += "<tr><td><font size=1 face=\"Helvetica\"color=black>Generated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " </font></td></tr>";
                                bool fiatLogoFound = File.Exists(Constants.FiatServicesLogoPath);
                                if (fiatLogoFound)
                                {
                                    message += "<tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td><img src='cid:logo_bottom' align=\"left\"></td></tr><tr><td><font size=1 face=\"Helvetica\"color=black>hr-services-serbia@fiatservices.com.</font></td></tr></table></body></html>";
                                    System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html"));
                                    System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(Constants.FiatServicesLogoPath, "image/jpeg"); // + sHeaderImg);     
                                    logo.ContentId = "logo_bottom";
                                    htmlView.LinkedResources.Add(logo);
                                    mailMessage.AlternateViews.Add(htmlView);

                                }

                                mailMessage.IsBodyHtml = true;
                                mailMessage.Body = message;


                                try
                                {
                                    smtp.Send(mailMessage);
                                    WriteLog("Mail sent. Employee: " + pairSupervisor.Key + ", mail: " + mail);
                                    foreach (string visit in visit_id.Split(','))
                                    {
                                        MedicalCheckVisitHdrTO medHDR = new MedicalCheckVisitHdrTO();
                                        medHDR.VisitID = uint.Parse(visit);
                                        medHDR.FlagEmail = (int)Constants.EmailFlag.OK;
                                        medHDR.FlagEmailCratedTime = DateTime.Now;
                                        MedicalCheckVisitHdr medical = new MedicalCheckVisitHdr();
                                        medical.VisitHdrTO = medHDR;
                                        medical.Update(true);

                                    }
                                    WriteLog("Visits updated.");

                                }
                                catch (Exception ex)
                                {

                                    WriteLog("Mail not sent. Employee: " + pairSupervisor.Key + " " + ex.Message);
                                }
                            }
                            else
                            {

                                WriteLog("Mail Null. Employee: " + pairSupervisor.Key);
                            }
                        }
                    }
                    WriteLog("Finished sending for BC's supervisors");
                }
            }
            catch (Exception ex)
            {

                WriteLog("Exception in MedicalCheckSender: " + ex.Message);
            }

        }
        // get all working units
        public static Dictionary<int, WorkingUnitTO> getWUnits()
        {
            try
            {
                Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int, WorkingUnitTO>();
                List<WorkingUnitTO> wuList = new WorkingUnit().Search();

                foreach (WorkingUnitTO wu in wuList)
                {
                    if (!wUnits.ContainsKey(wu.WorkingUnitID))
                        wUnits.Add(wu.WorkingUnitID, wu);
                    else
                        wUnits[wu.WorkingUnitID] = wu;
                }

                return wUnits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string emplTypes(EmployeeTO employee, int company)
        {
            string empl = "";
            //// get selected company
            Dictionary<string, string> dictEmplTypes = new Dictionary<string, string>();
            EmployeeType emplType = new EmployeeType();

            List<EmployeeTypeTO> listEmplTypes = new List<EmployeeTypeTO>();
            listEmplTypes = emplType.Search();

            foreach (EmployeeTypeTO emplTypeTO in listEmplTypes)
            {
                if (emplTypeTO.EmployeeTypeID == employee.EmployeeTypeID && emplTypeTO.WorkingUnitID == company)
                {
                    empl = emplTypeTO.EmployeeTypeName;
                    break;
                }
            }


            return empl;
        }
        public static string getLanguage(object user)
        {
            try
            {
                string lang = Constants.Lang_sr;

                if (user != null && user is ApplUserTO)
                    lang = ((ApplUserTO)user).LangCode;

                return lang;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SendNewCoupon(System.Net.Mail.SmtpClient smtp, string emailAddress, MedicalCheckVisitHdrTO medicalCheck)
        {
            try
            {
                WriteLog("New visit found: Visit ID: " + medicalCheck.VisitID);

                EmployeeTO Employee = new Employee().Find(medicalCheck.EmployeeID.ToString());
                EmployeeAsco4 asco4 = new EmployeeAsco4();
                asco4.EmplAsco4TO.EmployeeID = medicalCheck.EmployeeID;
                EmployeeAsco4TO asco4TO = new EmployeeAsco4TO();
                List<EmployeeAsco4TO> listAsco4 = asco4.Search();
                if (listAsco4.Count == 1)
                    asco4TO = listAsco4[0];

                int company = asco4TO.IntegerValue4;
                string stringone = asco4TO.NVarcharValue2;
                string type = emplTypes(Employee, company);

                ApplUserTO user = new ApplUser().Find(asco4TO.NVarcharValue5);

                string mail = asco4TO.NVarcharValue3;

                ResourceManager rm = new ResourceManager("EmailNotificationManagement.Resource", typeof(NotificationManager).Assembly);
                CultureInfo culture = CultureInfo.CreateSpecificCulture(getLanguage(user));

                if (!type.Equals("BC"))
                {
                    if (!mail.Trim().Equals(""))
                    {
                        WriteLog("Coupon creation started: Visit ID: " + medicalCheck.VisitID);
                        // get schedules
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(medicalCheck.EmployeeID.ToString(), medicalCheck.ScheduleDate, medicalCheck.ScheduleDate, null);

                        string schemaID = "";
                        List<EmployeeTimeScheduleTO> schList = new List<EmployeeTimeScheduleTO>();
                        if (emplSchedules.ContainsKey(medicalCheck.EmployeeID))
                            schList = emplSchedules[medicalCheck.EmployeeID];
                        int schID = Common.Misc.getTimeSchema(medicalCheck.ScheduleDate.Date, schList, schemas).TimeSchemaID;
                        if (schID != -1)
                            schemaID = schID.ToString();
                        else
                            schemaID = "0";
                        List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(medicalCheck.ScheduleDate.Date, schList, schemas);

                        string intervalsString = "";
                        foreach (WorkTimeIntervalTO interval in intervals)
                        {
                            intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat) + Environment.NewLine;
                        }
                        if (intervalsString.Length > 0)
                            intervalsString = intervalsString.Substring(0, intervalsString.Length - 2);

                        string risk = "";
                        foreach (MedicalCheckVisitDtlTO dtlTO in medicalCheck.VisitDetails)
                        {
                            if (dtlTO.Type.Trim().ToUpper().Equals(Constants.VisitType.R.ToString().Trim().ToUpper()))
                            {
                                if (riskDict.ContainsKey(dtlTO.CheckID))
                                    risk += riskDict[dtlTO.CheckID].RiskCode.Trim() + Environment.NewLine;
                            }
                            else if (dtlTO.Type.Trim().ToUpper().Equals(Constants.VisitType.V.ToString().Trim().ToUpper()))
                            {
                                if (vaccineDict.ContainsKey(dtlTO.CheckID))
                                    risk += vaccineDict[dtlTO.CheckID].VaccineType.Trim() + Environment.NewLine;
                            }
                        }
                        if (risk.Length > 0)
                            risk = risk.Substring(0, risk.Length - 2);

                        string stringresult = "";
                        stringresult += rm.GetString("couponAppointment", culture) + "\t\n\t\n";
                        stringresult += rm.GetString("hdrCompany", culture) + ":        " + WUnits[company].Description + " \t\n";
                        stringresult += rm.GetString("couponPrevention", culture) + "\t\n\t\n";
                        stringresult += "_________________________________________________________________________________________________\t\n\t\n";
                        stringresult += rm.GetString("hdrVisitID", culture) + medicalCheck.VisitID + " \t\n";
                        stringresult += rm.GetString("hdrStringone", culture) + stringone + " \t\n";
                        stringresult += rm.GetString("hdrEmplType", culture) + type + " \t\n";
                        stringresult += rm.GetString("hdrID", culture) + medicalCheck.EmployeeID + " \t\n";
                        stringresult += rm.GetString("hdrEmployee", culture) + Employee.FirstAndLastName + " \t\n";
                        stringresult += rm.GetString("hdrEmplRisk", culture) + risk + " \t\n";
                        stringresult += rm.GetString("hdrShift", culture) + schemaID + " \t\n";
                        stringresult += rm.GetString("hdrInterval", culture) + intervalsString + " \t\n";
                        stringresult += rm.GetString("visitMonth", culture) + medicalCheck.ScheduleDate.ToString("yyyyMM") + " \t\n\t\n\t\n\t\n";
                        stringresult += rm.GetString("visitAppointmentDate", culture) + medicalCheck.ScheduleDate.ToString("dd.MM.yyyy") + " " + rm.GetString("visitAppointmentTime", culture) + medicalCheck.ScheduleDate.ToString("HH:mm") + " " + rm.GetString("visitAppointmentAmbulance", culture) + mcPointDict[medicalCheck.PointID].Desc + " \t\n\t\n\t\n";
                        stringresult += "_________________________________________________________________________________________________\t\n\t\n";

                        stringresult += "_________________________________________________________________________________________________\t\n\t\n\t\n\t\n\t\n";

                        stringresult += rm.GetString("couponResponsible", culture) + "\t\n\t\n";

                        stringresult += "                     ________________ __________ _______________\t\n\t\n\t\n\t\n\t\n";

                        stringresult += "__________________________________________________________________________________________________\t\n\t\n\t\n\t\n\t\n\t\n";
                        stringresult += rm.GetString("couponEmployeeBeen", culture) + "\t\n\t\n\t\n\t\n\n";
                        stringresult += rm.GetString("couponSuccVisit", culture) + "\t\n\t\n\t\n\t\n";


                        for (int i = 0; i < 14; i++)
                        {
                            stringresult += "\t\n";

                        }
                        stringresult += rm.GetString("couponDoctor", culture) + " ________________________________________________________________________\t\n";

                        for (int i = 0; i < 10; i++)
                        {
                            stringresult += "\t\n";

                        }

                        //create PDF file
                        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_Coupon" + DateTime.Now.ToString("dd-MM-yyyy HHmmss") + "\\Coupon.pdf";
                        string parentFolder = Directory.GetParent(filePath).FullName;
                        if (!Directory.Exists(parentFolder))
                        {
                            Directory.CreateDirectory(parentFolder);
                        }
                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        //DELETE OLD FOLDERS AND FILES FROM TEMP
                        string tempFolder = Constants.logFilePath + "Temp";
                        string[] dirs = Directory.GetDirectories(tempFolder);
                        foreach (string direct in dirs)
                        {
                            DateTime creation = Directory.GetCreationTime(direct);
                            TimeSpan create = creation.AddHours(2).TimeOfDay;

                            if (creation.Date < DateTime.Now.Date || create < DateTime.Now.TimeOfDay)
                                Directory.Delete(direct, true);
                        }


                        iTextSharp.text.Document pdfDocCreatePDF = new iTextSharp.text.Document();

                        //Because of UNICODE char.
                        string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
                        iTextSharp.text.pdf.BaseFont sylfaen = iTextSharp.text.pdf.BaseFont.CreateFont(sylfaenpath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                        iTextSharp.text.Font head = new iTextSharp.text.Font(sylfaen, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);
                        iTextSharp.text.Font normal = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        iTextSharp.text.Font underline = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);

                        Stream stream = Stream.Null;
                        iTextSharp.text.pdf.PdfWriter writerPdf = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocCreatePDF, new FileStream(filePath, FileMode.Create));
                        iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph(10, stringresult, normal);

                        pdfDocCreatePDF.Open();
                        pdfDocCreatePDF.Add(para);
                        pdfDocCreatePDF.Close();

                        WriteLog("Coupon creation finished: Visit ID: " + medicalCheck.VisitID);

                        WriteLog("Mail creation started: Employee : " + Employee.EmployeeID);
                        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                       
                        mailMessage.To.Add(mail);
                        mailMessage.From = new System.Net.Mail.MailAddress(emailAddress);
                        mailMessage.Subject = rm.GetString("titleMedical", culture);

                        // get OU responsible persons
                        List<EmployeeTO> emplOUResList = new Employee().SearchEmployeesOUResponsible(Employee.OrgUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);

                        foreach (EmployeeTO empl in emplOUResList)
                        {
                            EmployeeAsco4 asco = new EmployeeAsco4();
                            asco.EmplAsco4TO.EmployeeID = medicalCheck.EmployeeID;
                            EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();
                            List<EmployeeAsco4TO> listAsco = asco.Search();
                            if (listAsco.Count == 1)
                                ascoTO = listAsco[0];
                            if (ascoTO.NVarcharValue3 != "")
                            {
                                mailMessage.CC.Add(ascoTO.NVarcharValue3);
                            }
                        }

                        Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
                        mailMessage.Attachments.Add(data);

                        string message = "";
                        message += "<html><body>" + rm.GetString("Employee", culture) + "  <b>" + Employee.FirstAndLastName + ", </b><br /><br />";

                        message += rm.GetString("newCouponMedical", culture) + "<br />" + rm.GetString("newCouponMedical1", culture) + "<br /><br />";
                        message += rm.GetString("newCouponMedical2", culture) + "<br /><br />";
                        message += "<table><tr><td> </td></tr><tr><td> </td></tr><tr><td><font size=2 face=\"Helvetica\" >" + rm.GetString("pleaseDoNot", culture) + "</font></td></tr>";

                        message += "<tr><td><font size=1 face=\"Helvetica\"color=black> This e-mail is automatically generated by ActA System (powered by SDDITG,  www.sdditg.com). </font></td></tr>";
                        message += "<tr><td><font size=1 face=\"Helvetica\"color=black>Generated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " </font></td></tr>";
                        bool fiatLogoFound = File.Exists(Constants.FiatServicesLogoPath);
                        if (fiatLogoFound)
                        {
                            message += "<tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td><img src='cid:logo_bottom' align=\"left\"></td></tr><tr><td><font size=1 face=\"Helvetica\"color=black>hr-services-serbia@fiatservices.com.</font></td></tr></table></body></html>";
                            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html"));
                            System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(Constants.FiatServicesLogoPath, "image/jpeg"); // + sHeaderImg);     
                            logo.ContentId = "logo_bottom";
                            htmlView.LinkedResources.Add(logo);
                            mailMessage.AlternateViews.Add(htmlView);

                        }
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Body = message;

                        try
                        {
                            smtp.Send(mailMessage);
                            WriteLog("Mail sent. Employee: " + medicalCheck.VisitID + ", mail: " + mail);
                            MedicalCheckVisitHdrTO medHDR = medicalCheck;
                            medHDR.FlagEmail = (int)Constants.EmailFlag.OK;
                            medHDR.FlagEmailCratedTime = DateTime.Now;
                            MedicalCheckVisitHdr medical = new MedicalCheckVisitHdr();
                            medical.VisitHdrTO = medHDR;
                            medical.Update(true);
                        }
                        catch (Exception ex)
                        {

                            WriteLog("Mail not sent: Employee : " + Employee.EmployeeID + " -- " + ex.Message);
                        }
                    }
                    else
                    {

                        WriteLog("Mail NULL. Employee: " + medicalCheck.EmployeeID);
                    }
                }
                else
                {
                    // UKOLIKO JE BC, ZA SVE BC-OVE JEDNOG SUPERVISORA POSLATI MEJL SUPERVISORU
                    List<EmployeeTO> emplWUResList = new Employee().SearchEmployeesWUResponsible(Employee.WorkingUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);

                    if (!dictionaryBC.ContainsKey(newCoupon))
                        dictionaryBC.Add(newCoupon, new Dictionary<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>>());

                    foreach (EmployeeTO empl in emplWUResList)
                    {
                        if (!dictionaryBC[newCoupon].ContainsKey(empl.EmployeeID))
                            dictionaryBC[newCoupon].Add(empl.EmployeeID, new Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>());

                        if (!dictionaryBC[newCoupon][empl.EmployeeID].ContainsKey(Employee))
                            dictionaryBC[newCoupon][empl.EmployeeID].Add(Employee, new List<MedicalCheckVisitHdrTO>());

                        dictionaryBC[newCoupon][empl.EmployeeID][Employee].Add(medicalCheck);


                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog("Exception in SendNewCoupon: " + ex.Message);
                throw ex;
            }
        }

        private void SendChangedCoupon(System.Net.Mail.SmtpClient smtp, string emailAddress, MedicalCheckVisitHdrTO medicalCheck)
        {
            try
            {
                WriteLog("Changed visit found: Visit ID: " + medicalCheck.VisitID);

                EmployeeTO Employee = new Employee().Find(medicalCheck.EmployeeID.ToString());
                EmployeeAsco4 asco4 = new EmployeeAsco4();
                asco4.EmplAsco4TO.EmployeeID = medicalCheck.EmployeeID;
                EmployeeAsco4TO asco4TO = new EmployeeAsco4TO();
                List<EmployeeAsco4TO> listAsco4 = asco4.Search();
                if (listAsco4.Count == 1)
                    asco4TO = listAsco4[0];

                int company = asco4TO.IntegerValue4;
                string stringone = asco4TO.NVarcharValue2;
                string type = emplTypes(Employee, company);

                ApplUserTO user = new ApplUser().Find(asco4TO.NVarcharValue5);
                string mail = asco4TO.NVarcharValue3;

                ResourceManager rm = new ResourceManager("EmailNotificationManagement.Resource", typeof(NotificationManager).Assembly);
                CultureInfo culture = CultureInfo.CreateSpecificCulture(getLanguage(user));


                if (!type.Equals("BC"))
                {
                    if (!mail.Trim().Equals(""))
                    {

                        WriteLog("Coupon creation started: Visit ID: " + medicalCheck.VisitID);
                        // get schedules
                        Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(medicalCheck.EmployeeID.ToString(), medicalCheck.ScheduleDate, medicalCheck.ScheduleDate, null);

                        string schemaID = "";
                        List<EmployeeTimeScheduleTO> schList = new List<EmployeeTimeScheduleTO>();
                        if (emplSchedules.ContainsKey(medicalCheck.EmployeeID))
                            schList = emplSchedules[medicalCheck.EmployeeID];
                        int schID = Common.Misc.getTimeSchema(medicalCheck.ScheduleDate.Date, schList, schemas).TimeSchemaID;
                        if (schID != -1)
                            schemaID = schID.ToString();
                        else
                            schemaID = "0";
                        List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(medicalCheck.ScheduleDate.Date, schList, schemas);
                        string intervalsString = "";
                        foreach (WorkTimeIntervalTO interval in intervals)
                        {
                            intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat) + Environment.NewLine;
                        }
                        if (intervalsString.Length > 0)
                            intervalsString = intervalsString.Substring(0, intervalsString.Length - 2);
                        string risk = "";
                        foreach (MedicalCheckVisitDtlTO dtlTO in medicalCheck.VisitDetails)
                        {
                            if (dtlTO.Type.Trim().ToUpper().Equals(Constants.VisitType.R.ToString().Trim().ToUpper()))
                            {
                                if (riskDict.ContainsKey(dtlTO.CheckID))
                                    risk += riskDict[dtlTO.CheckID].RiskCode.Trim() + Environment.NewLine;
                            }
                            else if (dtlTO.Type.Trim().ToUpper().Equals(Constants.VisitType.V.ToString().Trim().ToUpper()))
                            {
                                if (vaccineDict.ContainsKey(dtlTO.CheckID))
                                    risk += vaccineDict[dtlTO.CheckID].VaccineType.Trim() + Environment.NewLine;
                            }
                        }
                        if (risk.Length > 0)
                            risk = risk.Substring(0, risk.Length - 2);

                        string stringresult = "";
                        stringresult += rm.GetString("couponAppointment", culture) + "\t\n\t\n";
                        stringresult += rm.GetString("hdrCompany", culture) + ":        " + WUnits[company].Description + " \t\n";
                        stringresult += rm.GetString("couponPrevention", culture) + "\t\n\t\n";
                        stringresult += "_________________________________________________________________________________________________\t\n\t\n";
                        stringresult += rm.GetString("hdrVisitID", culture) + "       " + medicalCheck.VisitID + " \t\n";
                        stringresult += rm.GetString("hdrStringone", culture) + "      " + stringone + " \t\n";
                        stringresult += rm.GetString("hdrEmplType", culture) + "            " + type + " \t\n";
                        stringresult += rm.GetString("hdrID", culture) + "        " + medicalCheck.EmployeeID + " \t\n";
                        stringresult += rm.GetString("hdrEmployee", culture) + "       " + Employee.FirstAndLastName + " \t\n";
                        stringresult += rm.GetString("hdrEmplRisk", culture) + "            " + risk + " \t\n";
                        stringresult += rm.GetString("hdrShift", culture) + "           " + schemaID + " \t\n";
                        stringresult += rm.GetString("hdrInterval", culture) + "         " + intervalsString + " \t\n";
                        stringresult += rm.GetString("visitMonth", culture) + "         " + medicalCheck.ScheduleDate.ToString("yyyyMM") + " \t\n\t\n\t\n\t\n";
                        stringresult += rm.GetString("visitAppointmentDate", culture) + medicalCheck.ScheduleDate.ToString("dd.MM.yyyy") + " " + rm.GetString("visitAppointmentTime", culture) + medicalCheck.ScheduleDate.ToString("HH:mm") + " " + rm.GetString("visitAppointmentAmbulance", culture) + mcPointDict[medicalCheck.PointID].Desc + " \t\n\t\n\t\n";
                        stringresult += "_________________________________________________________________________________________________\t\n\t\n";

                        stringresult += "_________________________________________________________________________________________________\t\n\t\n\t\n\t\n\t\n";

                        stringresult += rm.GetString("couponResponsible", culture) + "\t\n\t\n";

                        stringresult += "                     ________________ __________ _______________\t\n\t\n\t\n\t\n\t\n";

                        stringresult += "__________________________________________________________________________________________________\t\n\t\n\t\n\t\n\t\n\t\n";
                        stringresult += rm.GetString("couponEmployeeBeen", culture) + "\t\n\t\n\t\n\t\n\n";
                        stringresult += rm.GetString("couponSuccVisit", culture) + "\t\n\t\n\t\n\t\n";


                        for (int i = 0; i < 14; i++)
                        {
                            stringresult += "\t\n";

                        }
                        stringresult += rm.GetString("couponDoctor", culture) + " ________________________________________________________________________\t\n";

                        for (int i = 0; i < 10; i++)
                        {
                            stringresult += "\t\n";

                        }
                        string filePath = Constants.logFilePath + "Temp\\" + user.UserID + "_Coupon" + DateTime.Now.ToString("dd-MM-yyyy HHmmss") + "\\Coupon.pdf";

                        string parentFolder = Directory.GetParent(filePath).FullName;
                        if (!Directory.Exists(parentFolder))
                        {
                            Directory.CreateDirectory(parentFolder);
                        }
                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        //DELETE OLD FOLDERS AND FILES FROM TEMP
                        string tempFolder = Constants.logFilePath + "Temp";
                        string[] dirs = Directory.GetDirectories(tempFolder);
                        foreach (string direct in dirs)
                        {
                            DateTime creation = Directory.GetCreationTime(direct);
                            TimeSpan create = creation.AddHours(2).TimeOfDay;

                            if (creation.Date < DateTime.Now.Date || create < DateTime.Now.TimeOfDay)
                                Directory.Delete(direct, true);
                        }

                        iTextSharp.text.Document pdfDocCreatePDF = new iTextSharp.text.Document();

                        //Because of UNICODE char.
                        string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
                        iTextSharp.text.pdf.BaseFont sylfaen = iTextSharp.text.pdf.BaseFont.CreateFont(sylfaenpath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                        iTextSharp.text.Font head = new iTextSharp.text.Font(sylfaen, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);
                        iTextSharp.text.Font normal = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        iTextSharp.text.Font underline = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);

                        iTextSharp.text.pdf.PdfWriter writerPdf = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocCreatePDF, new FileStream(filePath, FileMode.Create));
                        iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph(10, stringresult, normal);

                        pdfDocCreatePDF.Open();
                        pdfDocCreatePDF.Add(para);
                        pdfDocCreatePDF.Close();

                        WriteLog("Coupon creation finished: Visit ID: " + medicalCheck.VisitID);

                        WriteLog("Mail creation started: Employee: " + medicalCheck.EmployeeID);

                        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

                        mailMessage.To.Add(mail);
                        mailMessage.From = new System.Net.Mail.MailAddress(emailAddress);
                        mailMessage.Subject = rm.GetString("titleMedical", culture);

                        // get OU responsible persons
                        List<EmployeeTO> emplOUResList = new Employee().SearchEmployeesOUResponsible(Employee.OrgUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);

                        foreach (EmployeeTO empl in emplOUResList)
                        {
                            EmployeeAsco4 asco = new EmployeeAsco4();
                            asco.EmplAsco4TO.EmployeeID = medicalCheck.EmployeeID;
                            EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();
                            List<EmployeeAsco4TO> listAsco = asco.Search();
                            if (listAsco.Count == 1)
                                ascoTO = listAsco[0];
                            if (ascoTO.NVarcharValue3 != "")
                            {
                                mailMessage.CC.Add(ascoTO.NVarcharValue3);
                            }

                        }

                        Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
                        mailMessage.Attachments.Add(data);

                        MedicalCheckVisitHdrHist medicalHistory = new MedicalCheckVisitHdrHist();
                        medicalHistory.VisitHdrHistTO.VisitID = medicalCheck.VisitID;
                        medicalHistory.VisitHdrHistTO.FlagEmail = (int)Constants.EmailFlag.OK;
                        List<MedicalCheckVisitHdrHistTO> listMed = medicalHistory.SearchMedicalCheckVisitHeadersHistory();

                        if (listMed.Count > 0)
                        {
                            MedicalCheckVisitHdrHistTO medicalHistoryTO = listMed[listMed.Count - 1];

                            string message = "";
                            message += "<html><body>" + rm.GetString("Employee", culture) + "  <b>" + Employee.FirstAndLastName + ", </b><br /><br />";
                            message += rm.GetString("changedCouponSerb1", culture) + " " + "<b>" + rm.GetString("changedCouponSerb2", culture) + " </b>" + rm.GetString("changedCouponSerb3", culture);
                            message += medicalCheck.VisitID + rm.GetString("changedCoupon2", culture) + medicalHistoryTO.ScheduleDate.ToString(Constants.dateFormat + " " + Constants.timeFormat) + ") <b>" + rm.GetString("changedCoupon5", culture) + "</b><br />";
                            message += rm.GetString("changedCoupon3", culture) + "<br /><br />" + rm.GetString("changedCoupon4", culture) + "<br /><br />";
                            message += "<table><tr><td> </td></tr><tr><td> </td></tr><tr><td><font size=2 face=\"Helvetica\" >" + rm.GetString("pleaseDoNot", culture) + "</font></td></tr>";

                            message += "<tr><td><font size=1 face=\"Helvetica\"color=black> This e-mail is automatically generated by ActA System (powered by SDDITG,  www.sdditg.com). </font></td></tr>";
                            message += "<tr><td><font size=1 face=\"Helvetica\"color=black>Generated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " </font></td></tr>";
                            bool fiatLogoFound = File.Exists(Constants.FiatServicesLogoPath);
                            if (fiatLogoFound)
                            {
                                message += "<tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td><img src='cid:logo_bottom' align=\"left\"></td></tr><tr><td><font size=1 face=\"Helvetica\"color=black>hr-services-serbia@fiatservices.com.</font></td></tr></table></body></html>";
                                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html"));
                                System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(Constants.FiatServicesLogoPath, "image/jpeg"); // + sHeaderImg);     
                                logo.ContentId = "logo_bottom";
                                htmlView.LinkedResources.Add(logo);
                                mailMessage.AlternateViews.Add(htmlView);

                            }
                            mailMessage.IsBodyHtml = true;
                            mailMessage.Body = message;
                            WriteLog("Mail creation finished: Employee" + medicalCheck.EmployeeID);

                            try
                            {
                                smtp.Send(mailMessage);
                                WriteLog("Mail sent. Employee: " + medicalCheck.VisitID + ", mail: " + mail);

                                MedicalCheckVisitHdrTO medHDR = medicalCheck;
                                medHDR.FlagEmail = (int)Constants.EmailFlag.OK;
                                medHDR.FlagEmailCratedTime = DateTime.Now;
                                MedicalCheckVisitHdr medical = new MedicalCheckVisitHdr();
                                medical.VisitHdrTO = medHDR;
                                medical.Update(true);
                            }
                            catch (Exception ex)
                            {

                                WriteLog("Mail not sent. Employee: " + medicalCheck.EmployeeID + " -- " + ex.Message);
                            }
                        }
                    }
                    else
                        WriteLog("Mail NULL. Employee: " + medicalCheck.EmployeeID);
                }
                else
                {
                    // UKOLIKO JE BC, ZA SVE BC-OVE JEDNOG SUPERVISORA POSLATI MEJL SUPERVISORU
                    List<EmployeeTO> emplWUResList = new Employee().SearchEmployeesWUResponsible(Employee.WorkingUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);
                    if (!dictionaryBC.ContainsKey(changedCoupon))
                        dictionaryBC.Add(changedCoupon, new Dictionary<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>>());
                    foreach (EmployeeTO empl in emplWUResList)
                    {
                        if (!dictionaryBC[changedCoupon].ContainsKey(empl.EmployeeID))
                            dictionaryBC[changedCoupon].Add(empl.EmployeeID, new Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>());

                        if (!dictionaryBC[changedCoupon][empl.EmployeeID].ContainsKey(Employee))
                            dictionaryBC[changedCoupon][empl.EmployeeID].Add(Employee, new List<MedicalCheckVisitHdrTO>());

                        dictionaryBC[changedCoupon][empl.EmployeeID][Employee].Add(medicalCheck);


                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog("Exception in SendChangedCoupon: " + ex.Message);
                throw ex;

            }
        }

        private void SendDeletedCoupon(System.Net.Mail.SmtpClient smtp, string emailAddress, MedicalCheckVisitHdrTO medicalCheck)
        {
            try
            {
                WriteLog("Deleted visit found: Visit ID: " + medicalCheck.VisitID);

                EmployeeTO Employee = new Employee().Find(medicalCheck.EmployeeID.ToString());
                EmployeeAsco4 asco4 = new EmployeeAsco4();
                asco4.EmplAsco4TO.EmployeeID = medicalCheck.EmployeeID;
                EmployeeAsco4TO asco4TO = new EmployeeAsco4TO();
                List<EmployeeAsco4TO> listAsco4 = asco4.Search();
                if (listAsco4.Count == 1)
                    asco4TO = listAsco4[0];

                int company = asco4TO.IntegerValue4;

                string type = emplTypes(Employee, company);

                ApplUserTO user = new ApplUser().Find(asco4TO.NVarcharValue5);
                string mail = asco4TO.NVarcharValue3;

                ResourceManager rm = new ResourceManager("EmailNotificationManagement.Resource", typeof(NotificationManager).Assembly);
                CultureInfo culture = CultureInfo.CreateSpecificCulture(getLanguage(user));



                if (!type.Equals("BC"))
                {
                    if (!mail.Trim().Equals(""))
                    {
                        WriteLog("Mail creation started: Employee : " + Employee.EmployeeID);

                        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                        mailMessage.To.Add(mail);
                        mailMessage.From = new System.Net.Mail.MailAddress(emailAddress);
                        mailMessage.Subject = rm.GetString("titleMedical", culture);

                        // get OU responsible persons
                        List<EmployeeTO> emplOUResList = new Employee().SearchEmployeesOUResponsible(Employee.OrgUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);

                        foreach (EmployeeTO empl in emplOUResList)
                        {
                            EmployeeAsco4 asco = new EmployeeAsco4();
                            asco.EmplAsco4TO.EmployeeID = medicalCheck.EmployeeID;
                            EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();
                            List<EmployeeAsco4TO> listAsco = asco.Search();
                            if (listAsco.Count == 1)
                                ascoTO = listAsco[0];
                            if (ascoTO.NVarcharValue3 != "")
                            {
                                mailMessage.CC.Add(ascoTO.NVarcharValue3);
                            }
                        }
                   
                        string message = "";
                        message += "<html><body>" + rm.GetString("Employee", culture) + "  <b>" + Employee.FirstAndLastName + ", </b><br /><br />";
                        message += rm.GetString("deletedCoupon1", culture);
                        message += medicalCheck.VisitID + rm.GetString("changedCoupon2", culture) + medicalCheck.ScheduleDate.ToString(Constants.dateFormat + " " + Constants.timeFormat) + rm.GetString("deletedCoupon2", culture) + " <b>" + rm.GetString("deletedCoupon3", culture) + "</b><br /><br />";
                        message += rm.GetString("newCouponBC3", culture) + "<br /><br />";
                        message += "<table><tr><td> </td></tr><tr><td> </td></tr><tr><td><font size=2 face=\"Helvetica\" >" + rm.GetString("pleaseDoNot", culture) + "</font></td></tr>";

                        message += "<tr><td><font size=1 face=\"Helvetica\"color=black> This e-mail is automatically generated by ActA System (powered by SDDITG,  www.sdditg.com). </font></td></tr>";
                        message += "<tr><td><font size=1 face=\"Helvetica\"color=black>Generated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " </font></td></tr>";
                        bool fiatLogoFound = File.Exists(Constants.FiatServicesLogoPath);
                        if (fiatLogoFound)
                        {
                            message += "<tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td><img src='cid:logo_bottom' align=\"left\"></td></tr><tr><td><font size=1 face=\"Helvetica\"color=black>hr-services-serbia@fiatservices.com.</font></td></tr></table></body></html>";
                            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html"));
                            System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(Constants.FiatServicesLogoPath, "image/jpeg"); // + sHeaderImg);     
                            logo.ContentId = "logo_bottom";
                            htmlView.LinkedResources.Add(logo);
                            mailMessage.AlternateViews.Add(htmlView);

                        }
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Body = message;
                        WriteLog("Mail creation finished: Employee : " + Employee.EmployeeID);
                        try
                        {
                            smtp.Send(mailMessage);
                            WriteLog("Mail sent: Employee : " + Employee.EmployeeID + ", mail: " + mail);
                            MedicalCheckVisitHdrTO medHDR = medicalCheck;
                            medHDR.FlagEmail = 1;
                            medHDR.FlagEmailCratedTime = DateTime.Now;
                            MedicalCheckVisitHdr medical = new MedicalCheckVisitHdr();
                            medical.VisitHdrTO = medHDR;
                            medical.Update(true);
                        }
                        catch (Exception ex)
                        {

                            WriteLog("Mail not sent: Employee : " + Employee.EmployeeID + " -- " + ex.Message);
                        }

                    }
                    else
                    {
                        WriteLog("Mail NULL: Employee : " + Employee.EmployeeID);
                    }
                }
                else
                {
                    // UKOLIKO JE BC, ZA SVE BC-OVE JEDNOG SUPERVISORA POSLATI MEJL SUPERVISORU
                    List<EmployeeTO> emplWUResList = new Employee().SearchEmployeesWUResponsible(Employee.WorkingUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);
                    if (!dictionaryBC.ContainsKey(deletedCoupon))
                        dictionaryBC.Add(deletedCoupon, new Dictionary<int, Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>>());
                    foreach (EmployeeTO empl in emplWUResList)
                    {
                        if (!dictionaryBC[deletedCoupon].ContainsKey(empl.EmployeeID))
                            dictionaryBC[deletedCoupon].Add(empl.EmployeeID, new Dictionary<EmployeeTO, List<MedicalCheckVisitHdrTO>>());

                        if (!dictionaryBC[deletedCoupon][empl.EmployeeID].ContainsKey(Employee))
                            dictionaryBC[deletedCoupon][empl.EmployeeID].Add(Employee, new List<MedicalCheckVisitHdrTO>());

                        dictionaryBC[deletedCoupon][empl.EmployeeID][Employee].Add(medicalCheck);


                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog("Exception in SendDeletedCoupon: " + ex.Message);
                throw ex;
            }
        }

        private void WriteLog(string message)
        {
            log.writeLog(DateTime.Now.ToString(Constants.dateFormat + " " + Constants.timeFormat) + ": " + message);
        }

    }
}
