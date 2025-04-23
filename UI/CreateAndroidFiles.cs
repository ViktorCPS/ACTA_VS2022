using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using TransferObjects;
using Util;
using Common;
using System.IO;
using Ionic.Zip;

namespace UI
{
    public partial class CreateAndroidFiles : Form
    {

        // List View indexes		
        const int EmployeeID = 0;
        const int FirstAndLastName = 1;
        
        string path = @"c:\Users\";
        string pathForPic = @"D:\RF_123_140508120216.jpg";
        private string txtName = "RT_";
        private string txt = ".txt";
        private string zipName = "R_";
        private string zip = ".zip";
        private string dateTimeForFile = "";
        private string seconds = "";

        int noOfFiles = 0;

        //for TXT
        private string headerMobile = "ActA mobile registration";
        private string IMEI = "";
        private string longi = "";
        private string lati = "";
        private string dateTime = "";
        private string UID = "";
        private string ID = "";
        private string Name = "";

        public CreateAndroidFiles()
        {
            InitializeComponent();
            ckbxInOut.Checked = true;
            setLanguage();
            populateListView();
        }

        private void setLanguage()
        {
            lvEmployee.BeginUpdate();
            lvEmployee.Columns.Add("ID", 60, HorizontalAlignment.Left);
            lvEmployee.Columns.Add("IME I PREZIME", 270, HorizontalAlignment.Left);
            lvEmployee.EndUpdate();
        }

        private void populateListView()
        {
            try
            {
                List<EmployeeTO> zaposleniList = new List<EmployeeTO>();
                Employee zaposleni = new Employee();
                zaposleni.EmplTO.FirstName = "A";
                zaposleniList = zaposleni.Search();

                lvEmployee.BeginUpdate();
                lvEmployee.Items.Clear();

                if (zaposleniList.Count > 0)
                {
                    foreach (EmployeeTO z in zaposleniList)
                    {
                        if (z.Status == Constants.statusRetired)
                            continue;

                        ListViewItem item = new ListViewItem();
                        item = lvEmployee.Items.Add(z.EmployeeID.ToString());
                        item.SubItems.Add(z.FirstAndLastName.Trim());
                        item.Tag = z;
                    }
                }
                lvEmployee.EndUpdate();
                lvEmployee.Invalidate();
            }
            catch
            {
            }
        }

        private string inOrOut()
        {
            if (ckbxInOut.Checked)
            {
                return "IN";
            }
            else
            {
                return "OUT";
            }
        }

        private void ckbxInOut_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbxInOut.Checked)
                ckbxInOut.Text = "IN";
            else
                ckbxInOut.Text = "OUT";
        }

        private void setValues(EmployeeTO em)
        {
            try
            {
                if (tbIMEI.Text != null && tbIMEI.Text != "")
                {
                    IMEI = tbIMEI.Text;
                }

                if (tbLongi.Text != null && tbLongi.Text != "")
                {
                    longi = tbLongi.Text;
                }

                if (tbLati.Text != null && tbLati.Text != "")
                {
                    lati = tbLati.Text;
                }

                dateTime = dtpDateTime.Text.Replace(' ', 'T');

                //if (tbUID.Text != null && tbUID.Text != "")
                //{
                //    UID = tbUID.Text;
                //}

                TagTO tag = new Tag().FindActive(em.EmployeeID);
                if (tag.TagID != 0)
                    UID = tag.TagID.ToString();
                else
                    UID = "";

                ID = em.EmployeeID.ToString();

                Name = em.FirstAndLastName;

                string s = dateTime.Substring(2);
                s = s.Replace("-", string.Empty);
                s = s.Replace("T", string.Empty);
                s = s.Replace(":", string.Empty);
                dateTimeForFile = s;
                seconds = s.Substring(dateTimeForFile.Length - 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCreateFiles_Click(object sender, EventArgs e)
        {
            try
            {
                int count = this.lvEmployee.SelectedItems.Count;
                if (count == 0)
                {
                    MessageBox.Show("Selektujte bar jednog zaposlenog.");
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        EmployeeTO em = (EmployeeTO)lvEmployee.SelectedItems[i].Tag;
                        setValues(em);

                        string textToWrite = headerMobile + "|"
                                            + IMEI + "|"
                                            + longi + "|"
                                            + lati + "|"
                                            + dateTime + "|"
                                            + UID + "|"
                                            + inOrOut() + "|"
                                            + ID + "|"
                                            + Name;

                        bool succ = false;
                        string pathForTxt = @"D:\" + txtName + ID + "_" + dateTimeForFile + txt;

                        if (!File.Exists(pathForTxt))
                        {
                            // Create a file to write to. 
                            using (StreamWriter sw = File.CreateText(pathForTxt))
                            {
                                sw.WriteLine(textToWrite);
                                succ = true;
                            }
                        }

                        string pathForZip = @"D:\" + zipName + ID + "_" + dateTimeForFile + zip;
                        if (succ)
                        {
                            succ = false;
                            using (ZipFile zipFile = new ZipFile())
                            {
                                zipFile.AddFile(pathForTxt);
                                zipFile.AddFile(pathForPic);
                                zipFile.Save(pathForZip);
                                succ = true;
                            }
                        }

                        string pathForZipWithPass = zipName + ID + "_" + dateTimeForFile + zip;

                        if (succ)
                        {
                            succ = false;
                            using (ZipFile zipFile = new ZipFile())
                            {
                                zipFile.Password = ID + seconds;
                                zipFile.AddFile(pathForZip);
                                zipFile.Save(Constants.MobileDownload + "\\" + pathForZipWithPass);
                                succ = true;
                                noOfFiles++;
                            }
                        }

                        if (succ)
                        {
                            if (File.Exists(pathForTxt))
                            {
                                File.Delete(pathForTxt);
                            }
                            if (File.Exists(pathForZip))
                            {
                                File.Delete(pathForZip);
                            }
                        }
                    }

                    MessageBox.Show("Kreiran/o je " + noOfFiles + " zip fajl/a.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
