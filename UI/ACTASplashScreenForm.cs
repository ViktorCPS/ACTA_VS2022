using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    /// <summary>
    /// Start window for ACTA application. It contains an image and a text box to show current activity during start up of
    /// ACTA application.
    /// </summary>
    internal partial class ACTASplashScreenForm : Form
    {
        public ACTASplashScreenForm()
        {
            InitializeComponent();
        }

        override public Image BackgroundImage
        {
            get
            {
                return _splashPicBox.Image;
            }
            set
            {
                _splashPicBox.Image = value;
            }
        }

        public string ActivityText
        {
            get
            {
                return _activityLabel.Text;
            }
            set
            {
                _activityLabel.Text = value;
                _activityLabel.Refresh();
            }
        }
    }
}