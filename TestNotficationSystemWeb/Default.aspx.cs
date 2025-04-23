using System;
using System.Linq;
using System.Net.Mail;

using Common;
using TransferObjects;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using Util;

namespace TestNotficationSystemWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        static Thread t;
        static volatile bool interupt = true;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                t = new Thread(run);
                t.Start();
                t.IsBackground = true;
                Thread.Sleep(10);
                txtEmail.Text = "mimica.fon@gmail.com";
                txtPassword.Text = "";
            }
            dtPFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;

        }
        public void run()
        {
            startMethod();
        }

        public void startMethod()
        {
            try
            {
                while (interupt)
                {
                    if (!txtEmail.Text.Trim().Equals("") || !txtPassword.Text.Trim().Equals(""))
                    {
                        Employee empl = new Employee(Session[Constants.sessionConnection]);
                        IOPair ioPair = new IOPair(Session[Constants.sessionConnection]);
                        List<EmployeeTO> listAllEmployees = empl.getAllEmployees();

                        DateTime dt1 = new DateTime(2008, 11, 1);
                        DateTime dt2 = new DateTime(2011, 11, 28);

                        string str1 = dtPFromDate.SelectedDate.ToString("yyyy-MM-dd");
                        string str2 = dtpToDate.SelectedDate.ToString("yyyy-MM-dd");

                         dt1 = DateTime.ParseExact(str1, "yyyy-MM-dd", null);

                         dt2 = DateTime.ParseExact(str2, "yyyy-MM-dd", null);
                        

                        foreach (EmployeeTO empl1 in listAllEmployees)
                        {
                            List<IOPairTO> listIOPairs = ioPair.SearchNonEnteredIOPairs(empl1.EmployeeID, dt1, dt2);

                            if (listIOPairs.Count() > 0)
                            {
                                string address = empl.getAddressLine3(empl1.EmployeeID);
                                sendMail(listIOPairs, address, empl1.FirstName);
                            }
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Please, enter email address and password')</script>");

                    }
                    Thread.Sleep(TimeSpan.FromSeconds(20));

                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// method that takes all non entered IOPairs and sends it to email address of employee
        /// Message is sent like html page.
        /// </summary>
        /// <param name="listIOPairs"></param>
        /// <param name="emailAddress"></param>
        /// <param name="name"></param>
        public void sendMail(List<IOPairTO> listIOPairs, string emailAddress, string name)
        {
            List<IOPairTO> listStartTime = new List<IOPairTO>();
            List<IOPairTO> listEndTime = new List<IOPairTO>();
            string htmlBody;
            foreach (IOPairTO iopair in listIOPairs)
            {
                if (iopair.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    listStartTime.Add(iopair);
                }
                else
                {
                    listEndTime.Add(iopair);
                }
            }

            if (!emailAddress.Equals(""))
            {
                htmlBody = "Hello, " + name + "<br/><br/><table border=2>";
                if (listStartTime.Count() > 0)
                {
                    htmlBody = htmlBody + "<tr><td><font size=4>Not specified arrival time for the following pairs: </font></td></tr>";
                    foreach (IOPairTO iopairStart in listStartTime)
                    {
                        htmlBody = htmlBody + "<tr>" + iopairStart.CustomToStringForNoStartTime() + "</tr>";
                    }
                } if (listEndTime.Count() > 0)
                {
                    htmlBody = htmlBody + "<tr><td><font size=4>Not specified exit time for the following pairs: </font></td></tr>";

                    foreach (IOPairTO iopairEnd in listEndTime)
                    {
                        htmlBody = htmlBody + "<tr>" + iopairEnd.CustomToStringForNoEndTime() + "</tr>";

                    }
                }
                htmlBody = htmlBody + "</table>";
                MailMessage mess = new MailMessage();
                MailAddress from = new MailAddress(txtEmail.Text, "Milosava Djuric");
                MailAddress to = new MailAddress(emailAddress);
                mess.From = from;
                mess.To.Add(to);
                mess.Subject = "Non-entered pairs";
                mess.IsBodyHtml = true;

                mess.Body = htmlBody;

                SmtpClient mailClient = new SmtpClient("smtp.gmail.com");
                mailClient.EnableSsl = true;
                mailClient.Port = 587;
                mailClient.Credentials = new NetworkCredential(txtEmail.Text, txtPassword.Text);

                mailClient.Send(mess);



            }

        }
        public void RequestStop()
        {

            interupt = false;

        }
        protected void btExit_Click(object sender, EventArgs e)
        {
            try
            {

                RequestStop();
                Thread.Sleep(1);
                t.Join();

                String scriptString = "<script type=\"text/javascript\">" + "window.close();</" + "script>";

                if (!this.IsClientScriptBlockRegistered("clientScript"))
                {

                    this.RegisterClientScriptBlock("clientScript", scriptString);
                }
            }
            catch (ThreadAbortException ex)
            {

            }

        }



    }
}
