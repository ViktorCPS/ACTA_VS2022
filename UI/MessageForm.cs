using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class MessageForm : Form
    {
        public MessageForm(string msg)
        {         
            InitializeComponent();

            this.Text = "";
            lblMessage.Text = msg.Trim();
        }
    }
}