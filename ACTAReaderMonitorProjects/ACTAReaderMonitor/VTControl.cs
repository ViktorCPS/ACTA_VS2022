using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TransferObjects;

namespace ACTAReaderMonitor
{
    public partial class VTControl : UserControl
    {
        private GateTO validationTerminal = new GateTO();

        public GateTO ValidationTerminal
        {
            get { return validationTerminal; }
            set 
            {                
                validationTerminal = value;
                lblDescription.Text = validationTerminal.Description+"-"+validationTerminal.GateID.ToString();
                
            }
        }
        public VTControl()
        {
            InitializeComponent();
        }

        public void setStatus0(string status)
        {
            lblStatus0.Text = status;
        }
        public void setStatus1(string status)
        {
            lblStatus1.Text = status;
        }
    }
}
