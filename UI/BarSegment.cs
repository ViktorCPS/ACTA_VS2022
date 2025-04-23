using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    //Class BarSegment draw single bar segment
    //color of a single bar dependes of PassTypeID, property of list element.
    //If IOPair is open (there  is no StartTime or EndTime), this class will display an arrow.
    public partial class BarSegment : UserControl
    {
        private IOPairTO ioPair;
        private Color _backgroundColor;
        public Dictionary<int, PassTypeTO> PassTypes;
        private BarSegments parentBarSegments;
        ResourceManager rm;
        private CultureInfo culture;
        // Controller instance
        public NotificationController Controller;
        // Observer client instance
        public NotificationObserverClient observerClient;

        ApplUserTO logInUser;
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;
        bool updatePermission;
        
        public BarSegment()
        {
            PassTypes = new Dictionary<int,PassTypeTO>();
            InitializeComponent();
        }
        public BarSegment(IOPairTO iopair, Dictionary<int, PassTypeTO> passTypes, BarSegments bar)
        {
            this.ioPair = iopair;
            this.PassTypes = passTypes;
            this.parentBarSegments = bar;
            InitializeComponent();
        }
        private void Bar_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            Color IOPairColor = new Color();
            Color IOPairColor1 = new Color();
            ToolTip toolTip = new ToolTip();
            logInUser = NotificationController.GetLogInUser();
            string toolTipString = "";
            menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
            currentRoles = NotificationController.GetCurrentRoles();
            menuItemID = NotificationController.GetCurrentMenuItemID();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(EmployeePresenceGraphicReports).Assembly);
            updatePermission = false;

            SolidBrush brush = new SolidBrush(Color.White);

            try
            {               
                RectangleF rect3 = new RectangleF(0, 0, this.Width, this.Height);
                LinearGradientBrush brush3 = new LinearGradientBrush(rect3, BackgroudColor, Color.White, 90);
                brush3.SetSigmaBellShape(0.2f, 1.0f);
                gr.FillRectangle(brush3, rect3);
                PassTypeTO passType = new PassTypeTO();
                if (PassTypes.ContainsKey(ioPair.PassTypeID))
                    passType = PassTypes[ioPair.PassTypeID];

                //Define color of bar
                switch (ioPair.PassTypeID)
                {
                    case Constants.regularWork:
                    case Constants.extraHours:
                        IOPairColor = Color.FromArgb(51, 153, 102);
                        IOPairColor1 = Color.FromArgb(150, 0, 204, 255);
                        break;
                    case Constants.pause:
                    case Constants.automaticPause:
                    case Constants.automaticShortBreakPassType:
                        IOPairColor = Color.FromArgb(255, 255, 0);
                        IOPairColor1 = Color.FromArgb(150, 255, 255, 0);
                        break;
                    case Constants.officialOut:
                        IOPairColor = Color.FromArgb(2, 255, 2);
                        IOPairColor1 = Color.FromArgb(150, 2, 255, 2);
                        break;
                    case Constants.privateOut:
                        IOPairColor = Color.FromArgb(255, 204, 153);
                        IOPairColor1 = Color.FromArgb(150, 255, 204, 153);
                        break;
                    case Constants.vacation:
                        IOPairColor = Color.FromArgb(0, 204, 255);
                        IOPairColor1 = Color.FromArgb(150, 0, 204, 255);
                        break;
                    case Constants.sickLeave:
                        IOPairColor = Color.FromArgb(0, 249, 249);
                        IOPairColor1 = Color.FromArgb(150, 0, 249, 249);
                        break;
                    default:
                        
                        if (passType.IsPass == Constants.passOnReader)
                        {
                            IOPairColor = Color.FromArgb(255, 153, 0);
                            IOPairColor1 = Color.FromArgb(150, 255, 153, 0);
                        }
                        else if (passType.IsPass == Constants.wholeDayAbsence)
                        {
                            IOPairColor = Color.FromArgb(132, 193, 255);
                            IOPairColor1 = Color.FromArgb(150, 132, 193, 255);
                        }                        
                        break;
                }

                brush = new SolidBrush(IOPairColor);
                if (ioPair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))//If there is no StartTime drow an arrow
                {
                    // Create points for polygon
                    PointF p1 = new PointF(this.Width, 0);
                    PointF p2 = new PointF(this.Width, this.Height);
                    PointF p3 = new PointF(0, this.Height / 2);
                    PointF[] ptsArray =
                        {
                            p1, p2,p3
                        };
                    LinearGradientBrush brush5 = new LinearGradientBrush(p1, p3, IOPairColor, Color.White);
                    brush5.SetSigmaBellShape(0.5f, 0.6f);
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    // Draw open pair
                    e.Graphics.FillPolygon(brush5, ptsArray);
                    toolTipString = " - " + ioPair.EndTime.ToString("HH:mm:ss");
                }
                else if (ioPair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))//If there is no EndTime draw an arrow
                {
                    // Create points for polygon
                    PointF p1 = new PointF(0, 0);
                    PointF p2 = new PointF(0, this.Height);
                    PointF p3 = new PointF(this.Width, this.Height / 2);

                    PointF[] ptsArray =
                        {
                            p1, p2,p3
                        };

                    LinearGradientBrush brush5 = new LinearGradientBrush(p1, p3, IOPairColor, Color.White);
                    brush5.SetSigmaBellShape(0.5f, 0.6f);
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    // Draw open pair
                    e.Graphics.FillPolygon(brush5, ptsArray);
                    toolTipString = ioPair.StartTime.ToString("HH:mm:ss") + " - ";
                }
                else
                {
                    if (this.Width > 8)//If bar isn't to short draw 3D effect
                    {
                        RectangleF rect = new RectangleF(2, 0, this.Width - 4, this.Height);
                        LinearGradientBrush brush1 = new LinearGradientBrush(rect, IOPairColor, Color.White, 90);
                        brush1.SetSigmaBellShape(0.8f, 0.8f);

                        RectangleF rectElipse = new RectangleF(this.Width - 4, 0, 4, this.Height);
                        LinearGradientBrush brush2 = new LinearGradientBrush(rectElipse, IOPairColor, Color.White, 90);
                        brush2.SetSigmaBellShape(0.2f, 0.8f);
                        gr.SmoothingMode = SmoothingMode.HighQuality;

                        gr.FillRectangle(brush1, rect);

                        gr.FillEllipse(brush2, rectElipse);
                        gr.DrawEllipse(new Pen(IOPairColor1), new RectangleF(this.Width - 4, 0, 4, this.Height));
                        gr.FillEllipse(new SolidBrush(IOPairColor), new RectangleF(0, 0, 4, this.Height));

                    }
                    else //If bar is to short draw just rectangle
                    {
                        RectangleF rect1 = new RectangleF(0, 0, this.Width, this.Height);
                        LinearGradientBrush brush1 = new LinearGradientBrush(rect1, IOPairColor, Color.White, 90);
                        brush1.SetSigmaBellShape(0.8f, 0.8f);
                        gr.FillRectangle(brush1, rect1);

                    }
                    toolTipString = ioPair.StartTime.ToString("HH:mm:ss") + " - " + ioPair.EndTime.ToString("HH:mm:ss");
                }
                toolTip.SetToolTip(this, passType.Description + "\n" + ioPair.LocationName + "\n" + toolTipString);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // Release resources
                // Dispose of objects
                gr.Dispose();
                brush.Dispose();
            }
        }
        private void BarSegment_DoubleClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int permission;
                bool wuIOPairPurpose = false;
                List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
                string wuString = "";

                if (logInUser != null)
				{
					wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.IOPairPurpose);
				}

				foreach (WorkingUnitTO wUnit in wUnits)
				{
					wuString += wUnit.WorkingUnitID.ToString().Trim() + ","; 
				}
				
				if (wuString.Length > 0)
				{
					wuString = wuString.Substring(0, wuString.Length - 1);
				}
                Employee empl = new Employee();
                List<EmployeeTO> emplList = new List<EmployeeTO>();

                emplList = empl.SearchByWU(wuString);
                foreach (EmployeeTO employee in emplList)
                {
                    if (employee.EmployeeID == ioPair.EmployeeID)
                    {
                        wuIOPairPurpose = true;
                    }
                }
                if (wuIOPairPurpose)
                {
                    foreach (ApplRoleTO role in currentRoles)
                    {
                        permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                        updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
                    }
                    if (updatePermission)
                    {
                        IOPairsAdd ioPairsAdd = new IOPairsAdd(this.ioPair);
                        ioPairsAdd.ShowDialog();

                        this.parentBarSegments.Controls.Clear();
                        this.parentBarSegments.Refresh();
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("NoUpdatePermision", culture));
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("NoUpdatePermision", culture));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally 
            {
                this.Cursor = Cursors.Arrow;
            }
           
        }
        public Color BackgroudColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
            }
        }
    }
}
