using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using Common;
using ReaderRemoteManagement;

namespace ReaderRemoteControl
{
    public partial class Form2 : Form
    {
      
        public Form2()
        {
            InitializeComponent();
        }

        private void rbDiff_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFile.Checked)
            {
                rbDiff.Checked = false; rbFile.Checked = true;
            }
            else { rbFile.Checked = false; rbDiff.Checked = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK) {
                txtLocation.Text = open.FileName;
            }

        }
        Dictionary<int, List<string>> listLocations = new Dictionary<int, List<string>>();

        private void btnInsert_Click(object sender, EventArgs e)
        {

            string[] pointID = txtPoints.Text.Split(';');
            string p = "";
            Dictionary<string, string> dictionaryPoints = new Dictionary<string, string>();

            foreach (string po in pointID)
            {
                p += po.Remove(po.IndexOf(':')) + ",";

                dictionaryPoints.Add(po.Remove(po.IndexOf(':')), po.Substring(po.IndexOf(':') + 1));

            }
            if (p.Length > 0)
                p = p.Remove(p.LastIndexOf(','));

            List<OnlineMealsPointsTO> points = new OnlineMealsPoints().searchForIDs(p);

            List<ValidationTerminalTO> vtList = new List<ValidationTerminalTO>();

            Dictionary<string, ValidationTerminalTO> vtDict = new Dictionary<string, ValidationTerminalTO>();


            foreach (OnlineMealsPointsTO point in points)
            {
                //if (point.PointID == 1 || point.PointID == 2)
                //{
                if (!vtDict.ContainsKey(point.ReaderIPAddress.Trim()))
                {
                    //if (point.ReaderIPAddress.Equals("10.20.8.200"))
                    //{
                    ValidationTerminalTO terminal = new ValidationTerminalTO();
                    terminal.IpAddress = point.ReaderIPAddress;
                    terminal.Name = point.RestaurantName;
                    terminal.Status = "ENABLED";
                    terminal.Description = point.RestaurantName;
                    terminal.ValidationTerminalID = point.RestaurantID;
                    if (!terminal.Locations.ContainsKey(point.Reader_ant))
                        terminal.Locations.Add(point.Reader_ant, dictionaryPoints[point.PointID.ToString()]);

                    vtDict.Add(terminal.IpAddress, terminal);
                    //}
                }
                else
                {
                    vtDict[point.ReaderIPAddress].Locations.Add(point.Reader_ant, dictionaryPoints[point.PointID.ToString()]);
                }

                //}
            }
           
            if (rbDiff.Checked)
            {
                foreach (KeyValuePair<string, ValidationTerminalTO> pair in vtDict)
                {
                    DownloadLog log = new DownloadLog(pair.Value, pair.Value.Locations);
                    log.GetLog(true,"");
                }

            }
            else {
                foreach (KeyValuePair<string, ValidationTerminalTO> pair in vtDict)
                {
                    DownloadLog log = new DownloadLog(pair.Value, pair.Value.Locations);
                    log.GetLog(false,txtLocation.Text);
                }
            }
        }

   
    }
}
