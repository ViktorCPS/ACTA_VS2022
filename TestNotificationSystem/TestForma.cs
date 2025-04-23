using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

using Common;
using TransferObjects;
using System.Net;
using System.Threading;



namespace TestNotificationSystem
{
    public partial class TestForma : Form
    {
       
        public TestForma()
        {
            InitializeComponent();
            txtEmail.Text = "djuricmilosava@gmail.com";
            txtPassword.Text = "";
        }

        private void TestForma_Load(object sender, EventArgs e)
        {
            Thread t=new Thread(run);
            t.Start();
            t.IsBackground = true;

            //sets date format in dateTimePicker
            dtPFromDate.Format = DateTimePickerFormat.Custom;
            dtPFromDate.CustomFormat = "yyyy-MM-dd";
            dtPToDate.Format = DateTimePickerFormat.Custom;
            dtPToDate.CustomFormat = "yyyy-MM-dd";
        }

        public void run()
        { 
            //thread sleeps for 10 seconds, time for input date and another data on GUI
            Thread.Sleep(TimeSpan.FromSeconds(10));
            startMethod();
        }

        /// <summary>
        /// Finds all employees, and then for each one finds non entered IOPairs 
        /// and if is there any IOPairs finds eMail address of that employee 
        /// and calls method sendMail(...) which sends message to that eMail address
        /// </summary>
        public void startMethod()
        {
            try
            {
                while (true)
                {
                    if (!txtEmail.Text.Trim().Equals("") || !txtPassword.Text.Trim().Equals(""))
                    {
                        Employee empl = new Employee();
                        IOPair ioPair = new IOPair();
                        List<EmployeeTO> listAllEmployees = empl.getAllEmployees();

                        DateTime dt1 = dtPFromDate.Value;
                        DateTime dt2 = dtPToDate.Value;

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
                        MessageBox.Show("Please, enter mail and password");

                    }
                    Thread.Sleep(TimeSpan.FromSeconds(20));

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
    }
}
