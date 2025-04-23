using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;

namespace UI
{
    public partial class MealsOrderPIN : Form
    {
        public MealsOrderPIN()
        {
            InitializeComponent();
            tbPIN.Text = "";
        }

        private void button0_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tbPIN.Text += "9";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (tbPIN.Text.Length > 0)
            {
                tbPIN.Text = tbPIN.Text.Remove(tbPIN.Text.Length - 1);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string pinFromConfig = Constants.PINforMealOrder;
                if (pinFromConfig != null && !pinFromConfig.Equals(""))
                {
                    if (tbPIN.Text.Equals(pinFromConfig))
                    {
                        MealsOrder.canSeeAndEditCurrentWeek(true);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("GREŠKA!");
                        tbPIN.Text = "";
                        //tbPIN.Text = "!";
                    }
                }
            }
            catch
            {
            }
        }

        private void btnOnlyView_Click(object sender, EventArgs e)
        {
            try
            {
                MealsOrder.canOnlySeeCurrentWeek(true);
                this.Close();
            }
            catch
            {

            }
        }


    }
}
