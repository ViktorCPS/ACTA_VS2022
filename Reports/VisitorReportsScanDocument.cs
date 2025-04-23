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

using Util;
using Common;
using TransferObjects;

namespace Reports
{
    public partial class VisitorReportsScanDocument : Form
    {
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        public VisitorReportsScanDocument(int visitID, string card, string firstName, string lastName,
            string JMBG, string desc, string start, string end, string wu, string employee)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("Reports.ReportResource", typeof(VisitorReportsScanDocument).Assembly);
                setLanguage();

                tbCard.Text = card.Trim();
                tbFirstName.Text = firstName.Trim();
                tbLastName.Text = lastName.Trim();
                tbJMBG.Text = JMBG.Trim();
                tbDesc.Text = desc.Trim();
                tbStart.Text = start.Trim();
                tbEnd.Text = end.Trim();
                tbWU.Text = wu.Trim();
                tbEmpl.Text = employee.Trim();

                VisitorDocFileTO visitorDocFile = new VisitorDocFile().FindVisitorDocFileByVisitID(visitID.ToString().Trim());

                if (visitorDocFile.Content != null && visitorDocFile.Content.Length > 0)
                {
                    byte[] scanImageBytes = visitorDocFile.Content;

                    MemoryStream memStream = new MemoryStream(scanImageBytes);
                    // Set the position to the beginning of the stream.
                    memStream.Seek(0, SeekOrigin.Begin);
                    Image img = new Bitmap(memStream);
                    if (img.Height < pbScanDoc.Height && img.Width < pbScanDoc.Width)
                        pbScanDoc.SizeMode = PictureBoxSizeMode.CenterImage;
                    else
                        pbScanDoc.SizeMode = PictureBoxSizeMode.StretchImage;
                    pbScanDoc.Image = new Bitmap(memStream);
                    memStream.Close();
                }
                else
                {
                    pbNoImage.Visible = true;
                }
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
                // Form name
                this.Text = rm.GetString("titleVisitorsScanDoc", culture);
                
                // group box's text
                gbVisitor.Text = rm.GetString("gbVisitor", culture);
                gbScanDoc.Text = rm.GetString("gbScanDoc", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);

                // label's text
                lblCardNum.Text = rm.GetString("lblCardNum", culture);
                lblJMBG.Text = rm.GetString("lblIdentification", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblVisitStart.Text = rm.GetString("lblVisitStart", culture);
                lblVisitEnd.Text = rm.GetString("lblVisitEnd", culture);
                lblWU.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblVisitDescr.Text = rm.GetString("lblVisitDescr", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReportsScanDocument.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}