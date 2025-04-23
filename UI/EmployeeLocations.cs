using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    /// <summary>
    /// Summary description for EmployeeLocation.
    /// </summary>
    public class EmployeeLocations : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.Button btnClose;

        // List View indexes		
        const int EmployeeIndex = 0;
        const int WorkingUnitIndex = 1;
        const int LocationIndex = 2;
        const int PassTypeIndex = 3;
        const int EventTimeIndex = 4;

        List<EmployeeLocationTO> currentEmployeesList;
        private int sortOrder;
        private int sortField;
        private int startIndex;

        private int locationOUT = -100;
        private CultureInfo culture;
        ResourceManager rm;

        DebugLog log;
        ApplUserTO logInUser;
        // Current EmployeeLocation
        private EmployeeLocationTO currentEmployeeLocation;
        // Controller instance
        public NotificationController Controller;
        private System.Windows.Forms.ComboBox cbPassType;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.Label lblPassType;
        private System.Windows.Forms.GroupBox gbEmployeeLocations;

        // Date Time format
        protected string dateTimeformat = "";
        private System.Windows.Forms.Button btnClear;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblTotal;

        private int locationID;
        private Button btnReport;
        private CheckBox chbHierarhicly;
        private ListView lvLocations;
        private CheckBox chbLocHierarhicly;
        private Button btnSummary;
        private CheckBox cbMarkViolation;
        private GroupBox gbMarkViolation;
        private CheckBox cbAbsent;
        private CheckBox cbOutWH;
        private CheckBox cbEmplInPresence;
        private CheckBox cbOutsidePause;
        private CheckBox cbDuringPause;
        private Panel panel3;
        private Panel panel2;
        private Panel panel1;
        private Panel panel4;

        List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
        List<EmployeeTO> emplArray;
        List<WorkTimeSchemaTO> timeSchema;
        Dictionary<int, TimeSchemaPauseTO> pausesTable = new Dictionary<int, TimeSchemaPauseTO>();
        private const int PRESENT = 0;
        private const int OUTWRKHRS = 1;
        private const int ABSENT = 2;
        private const int ABSENTPAUSE = 3;
        private const int WHOLEDAYABSENCE = 4;

        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();
        Dictionary<int, WorkTimeSchemaTO> timeSchemasTable = new Dictionary<int, WorkTimeSchemaTO>();
        Dictionary<int, int> emplViolation = new Dictionary<int, int>();
        Dictionary<int, PassTypeTO> passTypes = new Dictionary<int, PassTypeTO>();
        private Button button1;
        private Button button2;
        private CheckBox chbShowEvents;
        private GroupBox gbEvents;
        private DateTimePicker dtTo;
        private DateTimePicker dtFrom;
        private Label label1;
        private Label lblFrom;
        private GroupBox gbShow;
        private RadioButton rbJustData;
        private RadioButton rbColor;
        private Button button3;
        private Panel panel5;
        private CheckBox cbNoWholeDayAbsence;
        private CheckBox cbUnjustified;
        private Button button4;
        private Button button5;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public EmployeeLocations()
        {
            try
            {
                InitializeComponent();

                this.CenterToScreen();

                // Init Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentEmployeeLocation = new EmployeeLocationTO();
                logInUser = NotificationController.GetLogInUser();

                currentEmployeesList = new List<EmployeeLocationTO>();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(EmployeeLocations).Assembly);
                setLanguage();

                //this.cbWorkingUnitID.DropDownStyle = ComboBoxStyle.DropDownList;
                locationID = -1;
                // Set datetime format
                DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
                dateTimeformat = dateTimeFormat.SortableDateTimePattern;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public EmployeeLocations(int selectedLoc)
        {
            try
            {
                InitializeComponent();

                this.CenterToScreen();

                // Init Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentEmployeeLocation = new EmployeeLocationTO();
                logInUser = NotificationController.GetLogInUser();

                currentEmployeesList = new List<EmployeeLocationTO>();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(EmployeeLocations).Assembly);
                setLanguage();

                //this.cbWorkingUnitID.DropDownStyle = ComboBoxStyle.DropDownList;
                locationID = selectedLoc;
                // Set datetime format
                DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
                dateTimeformat = dateTimeFormat.SortableDateTimePattern;
                chbLocHierarhicly.Checked = true;
                gbEmployeeLocations.Enabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbEmployeeLocations = new System.Windows.Forms.GroupBox();
            this.cbUnjustified = new System.Windows.Forms.CheckBox();
            this.gbShow = new System.Windows.Forms.GroupBox();
            this.rbJustData = new System.Windows.Forms.RadioButton();
            this.rbColor = new System.Windows.Forms.RadioButton();
            this.chbShowEvents = new System.Windows.Forms.CheckBox();
            this.gbEvents = new System.Windows.Forms.GroupBox();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cbMarkViolation = new System.Windows.Forms.CheckBox();
            this.gbMarkViolation = new System.Windows.Forms.GroupBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbNoWholeDayAbsence = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbOutsidePause = new System.Windows.Forms.CheckBox();
            this.cbDuringPause = new System.Windows.Forms.CheckBox();
            this.cbAbsent = new System.Windows.Forms.CheckBox();
            this.cbOutWH = new System.Windows.Forms.CheckBox();
            this.cbEmplInPresence = new System.Windows.Forms.CheckBox();
            this.lvLocations = new System.Windows.Forms.ListView();
            this.chbLocHierarhicly = new System.Windows.Forms.CheckBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSummary = new System.Windows.Forms.Button();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.gbEmployeeLocations.SuspendLayout();
            this.gbShow.SuspendLayout();
            this.gbEvents.SuspendLayout();
            this.gbMarkViolation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmployeeLocations
            // 
            this.gbEmployeeLocations.Controls.Add(this.cbUnjustified);
            this.gbEmployeeLocations.Controls.Add(this.gbShow);
            this.gbEmployeeLocations.Controls.Add(this.chbShowEvents);
            this.gbEmployeeLocations.Controls.Add(this.gbEvents);
            this.gbEmployeeLocations.Controls.Add(this.cbMarkViolation);
            this.gbEmployeeLocations.Controls.Add(this.gbMarkViolation);
            this.gbEmployeeLocations.Controls.Add(this.lvLocations);
            this.gbEmployeeLocations.Controls.Add(this.chbLocHierarhicly);
            this.gbEmployeeLocations.Controls.Add(this.chbHierarhicly);
            this.gbEmployeeLocations.Controls.Add(this.cbPassType);
            this.gbEmployeeLocations.Controls.Add(this.lblPassType);
            this.gbEmployeeLocations.Controls.Add(this.cbEmployee);
            this.gbEmployeeLocations.Controls.Add(this.btnSearch);
            this.gbEmployeeLocations.Controls.Add(this.cbWorkingUnit);
            this.gbEmployeeLocations.Controls.Add(this.lblEmployee);
            this.gbEmployeeLocations.Controls.Add(this.lblWU);
            this.gbEmployeeLocations.Controls.Add(this.button3);
            this.gbEmployeeLocations.Controls.Add(this.btnClear);
            this.gbEmployeeLocations.Location = new System.Drawing.Point(16, 8);
            this.gbEmployeeLocations.Name = "gbEmployeeLocations";
            this.gbEmployeeLocations.Size = new System.Drawing.Size(902, 392);
            this.gbEmployeeLocations.TabIndex = 1;
            this.gbEmployeeLocations.TabStop = false;
            this.gbEmployeeLocations.Text = "Employee Location";
            // 
            // cbUnjustified
            // 
            this.cbUnjustified.AutoSize = true;
            this.cbUnjustified.Location = new System.Drawing.Point(15, 280);
            this.cbUnjustified.Name = "cbUnjustified";
            this.cbUnjustified.Size = new System.Drawing.Size(119, 17);
            this.cbUnjustified.TabIndex = 16;
            this.cbUnjustified.Text = "Unjustified absence";
            this.cbUnjustified.UseVisualStyleBackColor = true;
            this.cbUnjustified.CheckedChanged += new System.EventHandler(this.cbUnjustified_CheckedChanged);
            // 
            // gbShow
            // 
            this.gbShow.Controls.Add(this.rbJustData);
            this.gbShow.Controls.Add(this.rbColor);
            this.gbShow.Location = new System.Drawing.Point(454, 280);
            this.gbShow.Name = "gbShow";
            this.gbShow.Size = new System.Drawing.Size(394, 58);
            this.gbShow.TabIndex = 15;
            this.gbShow.TabStop = false;
            this.gbShow.Text = "Show";
            // 
            // rbJustData
            // 
            this.rbJustData.AutoSize = true;
            this.rbJustData.Location = new System.Drawing.Point(169, 19);
            this.rbJustData.Name = "rbJustData";
            this.rbJustData.Size = new System.Drawing.Size(68, 17);
            this.rbJustData.TabIndex = 1;
            this.rbJustData.TabStop = true;
            this.rbJustData.Text = "Just data";
            this.rbJustData.UseVisualStyleBackColor = true;
            // 
            // rbColor
            // 
            this.rbColor.AutoSize = true;
            this.rbColor.Checked = true;
            this.rbColor.Location = new System.Drawing.Point(29, 19);
            this.rbColor.Name = "rbColor";
            this.rbColor.Size = new System.Drawing.Size(49, 17);
            this.rbColor.TabIndex = 0;
            this.rbColor.TabStop = true;
            this.rbColor.Text = "Color";
            this.rbColor.UseVisualStyleBackColor = true;
            // 
            // chbShowEvents
            // 
            this.chbShowEvents.AutoSize = true;
            this.chbShowEvents.Location = new System.Drawing.Point(433, 192);
            this.chbShowEvents.Name = "chbShowEvents";
            this.chbShowEvents.Size = new System.Drawing.Size(15, 14);
            this.chbShowEvents.TabIndex = 14;
            this.chbShowEvents.UseVisualStyleBackColor = true;
            this.chbShowEvents.CheckedChanged += new System.EventHandler(this.chbShowEvents_CheckedChanged);
            // 
            // gbEvents
            // 
            this.gbEvents.Controls.Add(this.dtTo);
            this.gbEvents.Controls.Add(this.dtFrom);
            this.gbEvents.Controls.Add(this.label1);
            this.gbEvents.Controls.Add(this.lblFrom);
            this.gbEvents.Enabled = false;
            this.gbEvents.Location = new System.Drawing.Point(454, 192);
            this.gbEvents.Name = "gbEvents";
            this.gbEvents.Size = new System.Drawing.Size(394, 82);
            this.gbEvents.TabIndex = 13;
            this.gbEvents.TabStop = false;
            this.gbEvents.Text = "Show Events for period";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(229, 25);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(130, 20);
            this.dtTo.TabIndex = 3;
            this.dtTo.Value = new System.DateTime(2013, 7, 24, 0, 0, 0, 0);
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(51, 26);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(130, 20);
            this.dtFrom.TabIndex = 2;
            this.dtFrom.Value = new System.DateTime(2013, 7, 24, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "To:  ";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(15, 28);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(36, 13);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From: ";
            // 
            // cbMarkViolation
            // 
            this.cbMarkViolation.AutoSize = true;
            this.cbMarkViolation.Location = new System.Drawing.Point(433, 19);
            this.cbMarkViolation.Name = "cbMarkViolation";
            this.cbMarkViolation.Size = new System.Drawing.Size(15, 14);
            this.cbMarkViolation.TabIndex = 12;
            this.cbMarkViolation.UseVisualStyleBackColor = true;
            this.cbMarkViolation.CheckedChanged += new System.EventHandler(this.cbMarkViolation_CheckedChanged);
            // 
            // gbMarkViolation
            // 
            this.gbMarkViolation.Controls.Add(this.panel5);
            this.gbMarkViolation.Controls.Add(this.cbNoWholeDayAbsence);
            this.gbMarkViolation.Controls.Add(this.panel4);
            this.gbMarkViolation.Controls.Add(this.panel3);
            this.gbMarkViolation.Controls.Add(this.panel2);
            this.gbMarkViolation.Controls.Add(this.panel1);
            this.gbMarkViolation.Controls.Add(this.cbOutsidePause);
            this.gbMarkViolation.Controls.Add(this.cbDuringPause);
            this.gbMarkViolation.Controls.Add(this.cbAbsent);
            this.gbMarkViolation.Controls.Add(this.cbOutWH);
            this.gbMarkViolation.Controls.Add(this.cbEmplInPresence);
            this.gbMarkViolation.Location = new System.Drawing.Point(454, 19);
            this.gbMarkViolation.Name = "gbMarkViolation";
            this.gbMarkViolation.Size = new System.Drawing.Size(394, 167);
            this.gbMarkViolation.TabIndex = 11;
            this.gbMarkViolation.TabStop = false;
            this.gbMarkViolation.Text = "Mark violation";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.LightBlue;
            this.panel5.Location = new System.Drawing.Point(344, 134);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(35, 17);
            this.panel5.TabIndex = 9;
            // 
            // cbNoWholeDayAbsence
            // 
            this.cbNoWholeDayAbsence.AutoSize = true;
            this.cbNoWholeDayAbsence.Location = new System.Drawing.Point(42, 134);
            this.cbNoWholeDayAbsence.Name = "cbNoWholeDayAbsence";
            this.cbNoWholeDayAbsence.Size = new System.Drawing.Size(155, 17);
            this.cbNoWholeDayAbsence.TabIndex = 9;
            this.cbNoWholeDayAbsence.Text = "without whole day absence";
            this.cbNoWholeDayAbsence.UseVisualStyleBackColor = true;
            this.cbNoWholeDayAbsence.CheckedChanged += new System.EventHandler(this.cbDuringPause_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightPink;
            this.panel4.Location = new System.Drawing.Point(344, 111);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(35, 17);
            this.panel4.TabIndex = 8;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Yellow;
            this.panel3.Location = new System.Drawing.Point(344, 89);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(35, 17);
            this.panel3.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightGreen;
            this.panel2.Location = new System.Drawing.Point(344, 44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(35, 17);
            this.panel2.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Location = new System.Drawing.Point(344, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(35, 17);
            this.panel1.TabIndex = 5;
            // 
            // cbOutsidePause
            // 
            this.cbOutsidePause.AutoSize = true;
            this.cbOutsidePause.Location = new System.Drawing.Point(42, 111);
            this.cbOutsidePause.Name = "cbOutsidePause";
            this.cbOutsidePause.Size = new System.Drawing.Size(132, 17);
            this.cbOutsidePause.TabIndex = 4;
            this.cbOutsidePause.Text = "outside the pause time";
            this.cbOutsidePause.UseVisualStyleBackColor = true;
            this.cbOutsidePause.CheckedChanged += new System.EventHandler(this.cbOutsidePause_CheckedChanged);
            // 
            // cbDuringPause
            // 
            this.cbDuringPause.AutoSize = true;
            this.cbDuringPause.Location = new System.Drawing.Point(42, 89);
            this.cbDuringPause.Name = "cbDuringPause";
            this.cbDuringPause.Size = new System.Drawing.Size(87, 17);
            this.cbDuringPause.TabIndex = 3;
            this.cbDuringPause.Text = "during pause";
            this.cbDuringPause.UseVisualStyleBackColor = true;
            this.cbDuringPause.CheckedChanged += new System.EventHandler(this.cbDuringPause_CheckedChanged);
            // 
            // cbAbsent
            // 
            this.cbAbsent.AutoSize = true;
            this.cbAbsent.Location = new System.Drawing.Point(6, 69);
            this.cbAbsent.Name = "cbAbsent";
            this.cbAbsent.Size = new System.Drawing.Size(139, 17);
            this.cbAbsent.TabIndex = 2;
            this.cbAbsent.Text = "Absent in working hours";
            this.cbAbsent.UseVisualStyleBackColor = true;
            this.cbAbsent.CheckedChanged += new System.EventHandler(this.cbAbsent_CheckedChanged);
            // 
            // cbOutWH
            // 
            this.cbOutWH.AutoSize = true;
            this.cbOutWH.Location = new System.Drawing.Point(6, 44);
            this.cbOutWH.Name = "cbOutWH";
            this.cbOutWH.Size = new System.Drawing.Size(188, 17);
            this.cbOutWH.TabIndex = 1;
            this.cbOutWH.Text = "In presence outside working hours";
            this.cbOutWH.UseVisualStyleBackColor = true;
            // 
            // cbEmplInPresence
            // 
            this.cbEmplInPresence.AutoSize = true;
            this.cbEmplInPresence.Location = new System.Drawing.Point(6, 21);
            this.cbEmplInPresence.Name = "cbEmplInPresence";
            this.cbEmplInPresence.Size = new System.Drawing.Size(162, 17);
            this.cbEmplInPresence.TabIndex = 0;
            this.cbEmplInPresence.Text = "In presence in working hours";
            this.cbEmplInPresence.UseVisualStyleBackColor = true;
            // 
            // lvLocations
            // 
            this.lvLocations.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lvLocations.FullRowSelect = true;
            this.lvLocations.GridLines = true;
            this.lvLocations.HideSelection = false;
            this.lvLocations.Location = new System.Drawing.Point(15, 160);
            this.lvLocations.Name = "lvLocations";
            this.lvLocations.Size = new System.Drawing.Size(313, 94);
            this.lvLocations.TabIndex = 7;
            this.lvLocations.UseCompatibleStateImageBehavior = false;
            this.lvLocations.View = System.Windows.Forms.View.Details;
            // 
            // chbLocHierarhicly
            // 
            this.chbLocHierarhicly.Location = new System.Drawing.Point(334, 126);
            this.chbLocHierarhicly.Name = "chbLocHierarhicly";
            this.chbLocHierarhicly.Size = new System.Drawing.Size(93, 24);
            this.chbLocHierarhicly.TabIndex = 8;
            this.chbLocHierarhicly.Text = "Hierarchy ";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(334, 24);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(93, 24);
            this.chbHierarhicly.TabIndex = 2;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(128, 94);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(200, 21);
            this.cbPassType.TabIndex = 6;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(8, 94);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(104, 23);
            this.lblPassType.TabIndex = 5;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(128, 56);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(200, 21);
            this.cbEmployee.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(773, 344);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(128, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(200, 21);
            this.cbWorkingUnit.TabIndex = 2;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(8, 56);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(104, 23);
            this.lblEmployee.TabIndex = 3;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(8, 24);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(104, 23);
            this.lblWU.TabIndex = 1;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(524, 344);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Clear";
            this.button3.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(524, 344);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSummary
            // 
            this.btnSummary.Location = new System.Drawing.Point(124, 803);
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.Size = new System.Drawing.Size(118, 23);
            this.btnSummary.TabIndex = 15;
            this.btnSummary.Text = "Summary preview";
            this.btnSummary.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // lvEmployees
            // 
            this.lvEmployees.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(16, 460);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(902, 311);
            this.lvEmployees.TabIndex = 13;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(843, 804);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(886, 426);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(843, 426);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 11;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTotal.Location = new System.Drawing.Point(766, 774);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 16;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(16, 805);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 14;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 804);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Report";
            this.button1.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(124, 804);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Summary preview";
            this.button2.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(124, 804);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(118, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "Summary preview";
            this.button4.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(124, 804);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(118, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "Summary preview";
            this.button5.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // EmployeeLocations
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(947, 839);
            this.ControlBox = false;
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSummary);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvEmployees);
            this.Controls.Add(this.gbEmployeeLocations);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "EmployeeLocations";
            this.ShowInTaskbar = false;
            this.Text = "EmployeeLocation";
            this.Load += new System.EventHandler(this.EmployeeLocations_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeeLocations_KeyUp);
            this.gbEmployeeLocations.ResumeLayout(false);
            this.gbEmployeeLocations.PerformLayout();
            this.gbShow.ResumeLayout(false);
            this.gbShow.PerformLayout();
            this.gbEvents.ResumeLayout(false);
            this.gbEvents.PerformLayout();
            this.gbMarkViolation.ResumeLayout(false);
            this.gbMarkViolation.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region Inner Class for sorting Array List of Employees Locations

        /*
		 *  Class used for sorting Array List of Employees Locations
		*/

        private class ArrayListSort : IComparer<EmployeeLocationTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(EmployeeLocationTO x, EmployeeLocationTO y)
            {
                EmployeeLocationTO empl1 = null;
                EmployeeLocationTO empl2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    empl1 = x;
                    empl2 = y;
                }
                else
                {
                    empl1 = y;
                    empl2 = x;
                }

                switch (compField)
                {
                    case EmployeeLocations.EmployeeIndex:
                        return empl1.EmployeeName.CompareTo(empl2.EmployeeName);
                    case EmployeeLocations.WorkingUnitIndex:
                        return empl1.WUName.CompareTo(empl2.WUName);
                    case EmployeeLocations.LocationIndex:
                        return empl1.LocName.CompareTo(empl2.LocName);
                    case EmployeeLocations.PassTypeIndex:
                        return empl1.PassType.CompareTo(empl2.PassType);
                    case EmployeeLocations.EventTimeIndex:
                        return empl1.EventTime.CompareTo(empl2.EventTime);
                    default:
                        return empl1.EmployeeID.CompareTo(empl2.EmployeeID);
                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                // button's text
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnSummary.Text = rm.GetString("btnSummary", culture);

                // Form name
                this.Text = rm.GetString("menuEmployeeLocations", culture);

                //check box's
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                chbLocHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                cbAbsent.Text = rm.GetString("cbAbsent", culture);
                cbOutsidePause.Text = rm.GetString("cbOutsidePause", culture);
                cbDuringPause.Text = rm.GetString("cbDuringPause", culture);
                cbEmplInPresence.Text = rm.GetString("cbEmplInPresence", culture);
                cbOutWH.Text = rm.GetString("cbOutWH", culture);
                cbNoWholeDayAbsence.Text = rm.GetString("cbNoWholeDayAbsence", culture);
                cbUnjustified.Text = rm.GetString("ttUnapprovedAbsence", culture);

                // group box text
                this.gbEmployeeLocations.Text = rm.GetString("gbEmployeeLocations", culture);
                gbMarkViolation.Text = rm.GetString("gbMarkViolation", culture);
                gbEvents.Text = rm.GetString("gbEvents", culture);
                gbShow.Text = rm.GetString("gbShow", culture);

                //radiobutton's text
                rbColor.Text = rm.GetString("rbColor", culture);
                rbJustData.Text = rm.GetString("rbJustData", culture);

                // label's text
                lblWU.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblPassType.Text = rm.GetString("lblPassType", culture);

                // list view initialization
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployees.Width - 5) / 5, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployees.Width - 5) / 5, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLocation", culture), (lvEmployees.Width - 5) / 5, HorizontalAlignment.Left); 
                lvEmployees.Columns.Add(rm.GetString("hdrEventTime", culture), (lvEmployees.Width - 5) / 5, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrPassType", culture), (lvEmployees.Width - 5) / 5, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvLocations.BeginUpdate();
                lvLocations.Columns.Add(rm.GetString("hdrLocation", culture), (lvLocations.Width - 5), HorizontalAlignment.Left);
                lvLocations.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate Working Unit Combo Box
        /// </summary>
        private void populateWorkingUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate Employee Combo Box
        /// </summary>
        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                emplArray = new List<EmployeeTO>();

                List<EmployeeTO> tempEmployees = new List<EmployeeTO>();

                string workUnitID = wuID.ToString();
                if (wuID < 0)
                {
                    tempEmployees = new Employee().SearchByWU(wuString);
                }
                else
                {
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }
                    }
                    tempEmployees = new Employee().SearchByWU(workUnitID);
                }
                foreach (EmployeeTO employee in tempEmployees)
                {
                    if (employee.Status.Equals(Constants.statusActive) || employee.Status.Equals(Constants.statusBlocked))
                    {
                        employee.LastName += " " + employee.FirstName;
                        emplArray.Add(employee);
                    }
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbEmployee.DataSource = emplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate Pass Type Combo Box
        /// </summary>
        private void populatePassTypeCombo()
        {
            try
            {
                List<PassTypeTO> ptArray = new PassType().Search();

                List<PassTypeTO> ptList = new List<PassTypeTO>();

                foreach (PassTypeTO ptMember in ptArray)
                {
                    if (ptMember.IsPass != 2)
                    {
                        ptList.Add(ptMember);
                    }
                }

                ptList.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

                cbPassType.DataSource = ptList;
                cbPassType.DisplayMember = "Description";
                cbPassType.ValueMember = "PassTypeID";
                cbPassType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.populatePassTypeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate Location Combo Box
        /// </summary>
        private void populateLocationListView()
        {
            try
            {
                Dictionary<LocationTO, int> allLocations = new Location().SearchAllLocationsHierarchicly();

                allLocations.Add(new LocationTO(locationOUT, "OUT", "", locationOUT, 0, ""), 0);

                // populate list view
                lvLocations.BeginUpdate();
                foreach (LocationTO location in allLocations.Keys)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = location.Name;
                    item.Tag = location;
                    lvLocations.Items.Add(item);
                }
                lvLocations.EndUpdate();

                if (locationID != locationOUT)
                {
                    for (int i = 0; i < lvLocations.Items.Count; i++)
                    {
                        ListViewItem item = lvLocations.Items[i];
                        if (((LocationTO)item.Tag).LocationID == locationID)
                        {
                            item.Selected = true;
                            lvLocations.Select();
                            lvLocations.EnsureVisible(i);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.populateLocationListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                if (!chbHierarhicly.Checked)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (cbWorkingUnit.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }

                if (cbWorkingUnit.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public virtual List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList)
        {
            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
            List<EmployeeTimeScheduleTO> timeScheduleListForEmployee = new List<EmployeeTimeScheduleTO>();
            foreach (EmployeeTimeScheduleTO employeeSchedule in timeScheduleList)
            {
                if (employeeSchedule.EmployeeID == employeeID)
                {
                    timeScheduleListForEmployee.Add(employeeSchedule);
                }
            }
            int timeScheduleIndex = -1;

            for (int scheduleIndex = 0; scheduleIndex < timeScheduleListForEmployee.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= timeScheduleListForEmployee[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleListForEmployee[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }

            if (timeScheduleIndex >= 0)
            {
                int cycleDuration = 0;
                int startDay = timeScheduleListForEmployee[timeScheduleIndex].StartCycleDay;
                int schemaID = timeScheduleListForEmployee[timeScheduleIndex].TimeSchemaID;
                List<WorkTimeSchemaTO> timeSchemaEmployee = new List<WorkTimeSchemaTO>();
                foreach (WorkTimeSchemaTO timeSch in timeSchema)
                {
                    if (timeSch.TimeSchemaID == schemaID)
                    {
                        timeSchemaEmployee.Add(timeSch);
                    }
                }
                WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                if (timeSchemaEmployee.Count > 0)
                {
                    schema = timeSchemaEmployee[0];
                    cycleDuration = schema.CycleDuration;
                }

                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                //TimeSpan days = date - ((EmployeesTimeSchedule) (timeScheduleListForEmployee[timeScheduleIndex])).Date;
                //interval = (TimeSchemaInterval) ((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleListForEmployee[timeScheduleIndex].Date.Date.Ticks);

                Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                for (int i = 0; i < table.Count; i++)
                {
                    intervalList.Add(table[i]);
                }

                if (schema.Type.Equals(Constants.schemaTypeFlexi))
                {
                    intervalList = setStartEndTimesFlexi(employeeID, date, intervalList);
                    foreach (WorkTimeIntervalTO inter in intervalList)
                    {
                        inter.EarliestArrived = inter.LatestArrivaed = inter.StartTime;
                        inter.LatestLeft = inter.EarliestLeft = inter.EndTime;
                    }
                }
                //else
                //{
                //    foreach (TimeSchemaInterval inter in intervalList)
                //    {                        
                //        inter.StartTime = inter.EarliestArrived;
                //        inter.EndTime = inter.LatestLeft;
                //    }
                //}                
            }

            return intervalList;
        }

        private List<WorkTimeIntervalTO> setStartEndTimesFlexi(int employeeID, DateTime date, List<WorkTimeIntervalTO> intervalList)
        {
            List<WorkTimeIntervalTO> list = new List<WorkTimeIntervalTO>();
            try
            {
                List<PassTO> passes = new List<PassTO>();
                Pass pass1 = new Pass();
                pass1.PssTO.EmployeeID = employeeID;
                pass1.PssTO.Direction = Constants.DirectionIn;
                passes = pass1.SearchInterval(date.Date, date.Date, "", new DateTime(), new DateTime());
                DateTime fromTime = new DateTime();
                for (int i = 0; i < intervalList.Count; i++)
                {
                    PassTO firstPass = new PassTO();

                    WorkTimeIntervalTO interval = intervalList[i];

                    TimeSpan duration = new TimeSpan(interval.EndTime.TimeOfDay.Ticks - interval.StartTime.TimeOfDay.Ticks);

                    if (passes.Count > 0)
                    {
                        foreach (PassTO pass in passes)
                        {
                            if (pass.EventTime.TimeOfDay > fromTime.TimeOfDay && pass.EventTime.TimeOfDay < interval.LatestLeft.TimeOfDay
                                && (firstPass.PassID == -1 || firstPass.EventTime > pass.EventTime))
                                firstPass = pass;
                        }
                    }
                    if (firstPass.PassID == -1)
                        interval.StartTime = interval.EarliestArrived;
                    else if (firstPass.EventTime.TimeOfDay <= interval.EarliestArrived.TimeOfDay)
                        interval.StartTime = interval.EarliestArrived;
                    else if (firstPass.EventTime.TimeOfDay > interval.EarliestArrived.TimeOfDay &&
                        firstPass.EventTime.TimeOfDay < interval.LatestArrivaed.TimeOfDay)
                        interval.StartTime = firstPass.EventTime;
                    else if (firstPass.EventTime.TimeOfDay >= interval.LatestArrivaed.TimeOfDay)
                        interval.StartTime = interval.LatestArrivaed;

                    interval.EndTime = interval.StartTime.Add(duration);

                    fromTime = interval.LatestLeft;
                    list.Add(interval);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.setStartEndTimes(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Populate List View with Employee Locations found
        /// </summary>
        /// <param name="locationsList"></param>
        /// 
        private void populateListView(List<EmployeeLocationTO> emplLocList, int startIndex)
        {
            try
            {
                List<EmployeeLocationTO> list = new List<EmployeeLocationTO>();
                emplViolation = new Dictionary<int, int>();
                if (emplLocList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                if (emplLocList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < emplLocList.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= emplLocList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = emplLocList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        if (emplLocList.Count > 0)
                        {
                            for (int i = startIndex; i < lastIndex; i++)
                            {
                                ListViewItem item = new ListViewItem();
                                EmployeeLocationTO emplLoc = emplLocList[i];

                                //EmployeeTO emplTO = new Employee().Find(emplLoc.EmployeeID.ToString());
                                item.Text = emplLoc.EmployeeName.Trim();
                                item.SubItems.Add(emplLoc.WUName);
                                item.SubItems.Add(emplLoc.LocName);

                                if (!emplLoc.EventTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                                {
                                    item.SubItems.Add(emplLoc.EventTime.ToString("dd/MM/yyyy HH:mm"));
                                }
                                else
                                {
                                    item.SubItems.Add("");
                                }

                                item.Tag = emplLoc;

                                /////////////////////////////////////////////////////////////////////////
                                //10.09.2009 Natasa
                                //check for violation's

                                if (cbMarkViolation.Checked)
                                {

                                    switch (emplLoc.Type)
                                    {
                                        case ABSENT:
                                            if (rbColor.Checked) item.BackColor = Color.LightPink;
                                            if (cbOutsidePause.Checked)
                                            {
                                                lvEmployees.Items.Add(item);
                                            }
                                            break;
                                        case ABSENTPAUSE:
                                            if (rbColor.Checked) item.BackColor = Color.Yellow;
                                            if (cbDuringPause.Checked)
                                            {
                                                lvEmployees.Items.Add(item);

                                            }
                                            break;
                                        case OUTWRKHRS:
                                            if (rbColor.Checked) item.BackColor = Color.LightGreen;
                                            if (cbOutWH.Checked)
                                            {
                                                lvEmployees.Items.Add(item);

                                            }
                                            break;
                                        case PRESENT:
                                            if (rbColor.Checked) item.BackColor = Color.White;
                                            if (cbEmplInPresence.Checked)
                                            {
                                                lvEmployees.Items.Add(item);

                                            }
                                            break;
                                        case WHOLEDAYABSENCE:
                                            if (rbColor.Checked) item.BackColor = Color.LightBlue;
                                            if (!cbNoWholeDayAbsence.Checked)
                                            {
                                                lvEmployees.Items.Add(item);


                                            }
                                            break;

                                    }
                                    item.SubItems.Add(emplLoc.PassType);
                                }
                                else
                                {
                                    item.SubItems.Add(emplLoc.PassType);

                                    lvEmployees.Items.Add(item);

                                }
                            }
                        }

                    }
                }

                lvEmployees.EndUpdate();
                lvEmployees.Invalidate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        //private void populateListView(List<EmployeeLocationTO> emplLocList, int startIndex)
        //{
        //    try
        //    {
        //        List<EmployeeLocationTO> list = new List<EmployeeLocationTO>();
        //        emplViolation = new Dictionary<int, int>();
        //        if (emplLocList.Count > Constants.recordsPerPage)
        //        {
        //            btnPrev.Visible = true;
        //            btnNext.Visible = true;
        //        }
        //        else
        //        {
        //            btnPrev.Visible = false;
        //            btnNext.Visible = false;
        //        }

        //        lvEmployees.BeginUpdate();
        //        lvEmployees.Items.Clear();

        //        if (emplLocList.Count > 0)
        //        {
        //            if ((startIndex >= 0) && (startIndex < emplLocList.Count))
        //            {
        //                if (startIndex == 0)
        //                {
        //                    btnPrev.Enabled = false;
        //                }
        //                else
        //                {
        //                    btnPrev.Enabled = true;
        //                }

        //                int lastIndex = startIndex + Constants.recordsPerPage;
        //                if (lastIndex >= emplLocList.Count)
        //                {
        //                    btnNext.Enabled = false;
        //                    lastIndex = emplLocList.Count;
        //                }
        //                else
        //                {
        //                    btnNext.Enabled = true;
        //                }

        //                if (emplLocList.Count > 0)
        //                {
        //                    for (int i = startIndex; i < lastIndex; i++)
        //                    {
        //                        ListViewItem item = new ListViewItem();
        //                        EmployeeLocationTO emplLoc = emplLocList[i];

        //                        //EmployeeTO emplTO = new Employee().Find(emplLoc.EmployeeID.ToString());
        //                        item.Text = emplLoc.EmployeeName.Trim();
        //                        item.SubItems.Add(emplLoc.WUName);
        //                        item.SubItems.Add(emplLoc.LocName);
        //                        item.SubItems.Add(emplLoc.PassType);

        //                        if (!emplLoc.EventTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
        //                        {
        //                            item.SubItems.Add(emplLoc.EventTime.ToString("dd/MM/yyyy HH:mm"));
        //                        }
        //                        else
        //                        {
        //                            item.SubItems.Add("");
        //                        }

        //                        item.Tag = emplLoc;

        //                        /////////////////////////////////////////////////////////////////////////
        //                        //10.09.2009 Natasa
        //                        //check for violation's
        //                        if (cbMarkViolation.Checked)
        //                        {
        //                            int presence = -1;

        //                            PassTypeTO type = new PassTypeTO();
        //                            if (passTypes.ContainsKey(emplLoc.PassTypeID))
        //                                type = passTypes[emplLoc.PassTypeID];
        //                            if (type.PassTypeID != -1 && type.PassTypeID != Constants.regularWork
        //                                && type.IsPass == Constants.passOnReader)
        //                                presence = PRESENT;
        //                            else
        //                            {
        //                                List<WorkTimeIntervalTO> timeSchemaIntervalList = this.getTimeSchemaInterval(emplLoc.EmployeeID, DateTime.Now.Date, timeScheduleList);

        //                                foreach (WorkTimeIntervalTO interval in timeSchemaIntervalList)
        //                                {
        //                                    if (interval.EarliestArrived.TimeOfDay < DateTime.Now.TimeOfDay && interval.LatestLeft.TimeOfDay > DateTime.Now.TimeOfDay)
        //                                    {
        //                                        if (emplLoc.LocationID == -1)
        //                                        {
        //                                            presence = ABSENT;
        //                                            if (pausesTable.ContainsKey(interval.PauseID))
        //                                            {
        //                                                TimeSchemaPauseTO pause = pausesTable[interval.PauseID];
        //                                                if (interval.StartTime.AddMinutes(pause.EarliestUseTime).TimeOfDay <= DateTime.Now.TimeOfDay
        //                                                    && interval.EndTime.AddMinutes(-pause.LatestUseTime).TimeOfDay >= DateTime.Now.TimeOfDay
        //                                                    && emplLoc.EventTime.Date == DateTime.Now.Date
        //                                                    && emplLoc.EventTime.TimeOfDay <= DateTime.Now.TimeOfDay)
        //                                                {
        //                                                    if (emplLoc.EventTime.AddMinutes(pause.PauseDuration).AddMinutes(pause.PauseOffset).TimeOfDay >= DateTime.Now.TimeOfDay)
        //                                                        presence = ABSENTPAUSE;
        //                                                }
        //                                            }

        //                                        }
        //                                        else
        //                                            presence = PRESENT;
        //                                    }
        //                                    else if (emplLoc.LocationID != -1)
        //                                    {
        //                                        presence = OUTWRKHRS;
        //                                    }
        //                                    else
        //                                        presence = PRESENT;
        //                                }

        //                                if (presence == -1 && emplLoc.LocationID != -1)
        //                                {
        //                                    presence = OUTWRKHRS;
        //                                }

        //                                if (presence == -1 && emplLoc.LocationID == -1)
        //                                {
        //                                    presence = PRESENT;
        //                                }
        //                            }

        //                            if (!emplViolation.ContainsKey(emplLoc.EmployeeID))
        //                                emplViolation.Add(emplLoc.EmployeeID, presence);

        //                            switch (presence)
        //                            {
        //                                case ABSENT:
        //                                    item.BackColor = Color.LightPink;
        //                                    if (cbOutsidePause.Checked)
        //                                    {
        //                                        lvEmployees.Items.Add(item);
        //                                        list.Add(emplLoc);
        //                                    }
        //                                    break;
        //                                case ABSENTPAUSE:
        //                                    item.BackColor = Color.Yellow;
        //                                    if (cbDuringPause.Checked)
        //                                    {
        //                                        lvEmployees.Items.Add(item);
        //                                        list.Add(emplLoc);
        //                                    }
        //                                    break;
        //                                case OUTWRKHRS:
        //                                    item.BackColor = Color.LightGreen;
        //                                    if (cbOutWH.Checked)
        //                                    {
        //                                        lvEmployees.Items.Add(item);
        //                                        list.Add(emplLoc);
        //                                    }
        //                                    break;
        //                                case PRESENT:
        //                                    item.BackColor = Color.White;
        //                                    if (cbEmplInPresence.Checked)
        //                                    {
        //                                        lvEmployees.Items.Add(item);
        //                                        list.Add(emplLoc);
        //                                    }
        //                                    break;

        //                            }
        //                        }

        //                        ///////////////////////////////////////////////////////////////////////////////
        //                        else
        //                        {
        //                            lvEmployees.Items.Add(item);
        //                            list.Add(emplLoc);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        lvEmployees.EndUpdate();
        //        lvEmployees.Invalidate();
        //        currentEmployeesList = list;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " EmployeeLocations.populateListView(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        /// <summary>
        /// Clears Form
        /// </summary>
        private void formClear()
        {
            try
            {
                this.chbHierarhicly.Checked = this.chbLocHierarhicly.Checked = false;
                this.cbWorkingUnit.SelectedIndex = 0;
                this.cbEmployee.SelectedIndex = 0;
                this.cbPassType.SelectedIndex = 0;
                this.lvLocations.SelectedItems.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.formClear(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string workingUnit = "";
                int employee = -1;
                int passType = -1;
                string locString = "";
                Dictionary<int, WorkingUnitTO> wUnitsDict = new WorkingUnit().getWUDictionary();

                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    workingUnit = cbWorkingUnit.SelectedValue.ToString();

                    //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                    WorkingUnit wu = new WorkingUnit();
                    if ((int)this.cbWorkingUnit.SelectedValue != -1 && chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO wUnit in wUnits)
                        {
                            if (wUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
                            {
                                wuList.Add(wUnit);
                                workUnit.WUTO = wUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = wu.FindAllChildren(wuList);
                        workingUnit = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            workingUnit += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workingUnit.Length > 0)
                        {
                            workingUnit = workingUnit.Substring(0, workingUnit.Length - 1);
                        }
                    }
                }
                if (cbEmployee.SelectedIndex > 0)
                {
                    employee = (int)cbEmployee.SelectedValue;
                }

                if (cbPassType.SelectedIndex > 0)
                {
                    passType = (int)cbPassType.SelectedValue;
                }
                if (lvLocations.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvLocations.SelectedItems)
                    {
                        if (chbLocHierarhicly.Checked)
                        {
                            int selectedLocation = ((LocationTO)item.Tag).LocationID;
                            Location loc = new Location();
                            loc.LocTO.LocationID = selectedLocation;
                            List<LocationTO> locations = loc.Search();

                            if (selectedLocation == -1)
                            {
                                for (int i = locations.Count - 1; i >= 0; i--)
                                {
                                    if (locations[i].LocationID != locations[i].ParentLocationID)
                                    {
                                        locations.RemoveAt(i);
                                    }
                                }
                            }
                            locations = loc.FindAllChildren(locations);
                            selectedLocation = -1;

                            foreach (LocationTO loc1 in locations)
                            {
                                locString += loc1.LocationID.ToString().Trim() + ",";
                            }
                        }
                        else
                        {
                            locString += ((LocationTO)item.Tag).LocationID.ToString().Trim() + ",";
                        }
                    }

                    if (locString.Length > 0)
                    {
                        locString = locString.Substring(0, locString.Length - 1);
                    }
                }


                currentEmployeesList = new List<EmployeeLocationTO>();
                List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
                if (workingUnit != "")
                {
                    List<EmployeeTO> wuEmployees = new Employee().SearchByWU(workingUnit);
                   
                    Employee empl = new Employee();
                    List<EmployeeLocationTO> emplLocCurrentList = new List<EmployeeLocationTO>();

                    for (int i = 0; i < wuEmployees.Count; i++)
                    {
                        // skip retired employees
                        if (wuEmployees[i].Status == Constants.statusRetired)
                            continue;

                        if (employee == -1)
                        {
                            if (!wuString.Equals(""))
                            {
                                EmployeeLocation eLoc = new EmployeeLocation();
                                eLoc.EmplLocTO.EmployeeID = wuEmployees[i].EmployeeID;
                                eLoc.EmplLocTO.PassTypeID = passType;
                                if (!chbShowEvents.Checked) emplLocationList.AddRange(eLoc.Search(locString, wuString));
                                else emplLocationList.AddRange(eLoc.Search(locString, wuString, dtFrom.Value, dtTo.Value));
                            }
                            else
                            {
                                MessageBox.Show(rm.GetString("noEmplLocationPrivilege", culture));
                            }

                            for (int j = 0; j < emplLocCurrentList.Count; j++)
                            {
                                emplLocationList.Add(emplLocCurrentList[j]);
                            }
                        }
                        else if (employee == wuEmployees[i].EmployeeID)
                        {
                            if (!wuString.Equals(""))
                            {
                                EmployeeLocation eLoc = new EmployeeLocation();
                                eLoc.EmplLocTO.EmployeeID = employee;
                                eLoc.EmplLocTO.PassTypeID = passType;
                                if (!chbShowEvents.Checked) emplLocationList = eLoc.Search(locString, wuString);
                                else emplLocationList = eLoc.Search(locString, wuString, dtFrom.Value, dtTo.Value);
                            }
                            else
                            {
                                MessageBox.Show(rm.GetString("noEmplLocationPrivilege", culture));
                            }
                            break;
                        }
                    }
                }
                else
                {
                    if (!wuString.Equals(""))
                    {
                        List<EmployeeTO> employeesList = new Employee().SearchByWU(workingUnit);
                        Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();
                        foreach (EmployeeTO el in employeesList)
                        {
                            // skip retired employees
                            if (el.Status == Constants.statusRetired)
                                continue;

                            if (!employees.ContainsKey(el.EmployeeID))
                                employees.Add(el.EmployeeID, el);
                        }
                        EmployeeLocation eLoc = new EmployeeLocation();
                        eLoc.EmplLocTO.EmployeeID = employee;
                        eLoc.EmplLocTO.PassTypeID = passType;

                        if (employee != -1)
                        {

                            EmployeeTO employeeSel = new Employee().Find(employee.ToString());

                            if (!chbShowEvents.Checked) emplLocationList.AddRange(eLoc.Search(locString, employeeSel.WorkingUnitID.ToString()));
                            else emplLocationList.AddRange(eLoc.Search(locString, employeeSel.WorkingUnitID.ToString(), dtFrom.Value, dtTo.Value));

                        }
                        else
                        {

                            if (!chbShowEvents.Checked) emplLocationList.AddRange(eLoc.Search(locString, wuString));
                            else emplLocationList.AddRange(eLoc.Search(locString, wuString, dtFrom.Value, dtTo.Value));

                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noEmplLocationPrivilege", culture));
                    }
                }

                if (currentEmployeesList.Count > 0)
                {
                    startIndex = 0;
                    if (chbShowEvents.Checked)
                        timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(this.getSelectedEmployeesString(), dtFrom.Value.Date, dtTo.Value.Date);
                    else
                        timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(this.getSelectedEmployeesString(), DateTime.Now.Date, DateTime.Now.Date);
                    string schemaID = "";
                    foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                    {
                        schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                    }
                    if (!schemaID.Equals(""))
                    {
                        schemaID = schemaID.Substring(0, schemaID.Length - 2);
                    }

                    timeSchema = new TimeSchema().Search(schemaID);
                    foreach (EmployeeLocationTO emplLoc in emplLocationList)
                    {

                        int presence = -1;

                        PassTypeTO type = new PassTypeTO();
                        if (passTypes.ContainsKey(emplLoc.PassTypeID))
                            type = passTypes[emplLoc.PassTypeID];
                        if (type.PassTypeID != -1 && type.PassTypeID != Constants.regularWork
                            && type.IsPass == Constants.passOnReader)
                        { presence = PRESENT; }
                        else
                        {
                            List<WorkTimeIntervalTO> timeSchemaIntervalList = this.getTimeSchemaInterval(emplLoc.EmployeeID, DateTime.Now.Date, timeScheduleList);

                            foreach (WorkTimeIntervalTO interval in timeSchemaIntervalList)
                            {
                                if (interval.EarliestArrived.TimeOfDay < DateTime.Now.TimeOfDay && interval.LatestLeft.TimeOfDay > DateTime.Now.TimeOfDay)
                                {
                                    if (emplLoc.LocationID == locationOUT)
                                    {
                                        EmployeeAbsence abs = new EmployeeAbsence();
                                        abs.EmplAbsTO.EmployeeID = emplLoc.EmployeeID;
                                        List<EmployeeAbsenceTO> absences = abs.Search();
                                        bool absenceflag = false;
                                        foreach (EmployeeAbsenceTO absence in absences)
                                        {

                                            if (absence.DateStart.Date >= DateTime.Now.Date)
                                            {
                                                emplLoc.PassType = absence.PassType;
                                                emplLoc.PassTypeID = absence.PassTypeID;

                                                presence = WHOLEDAYABSENCE;
                                                absenceflag = true;
                                            }

                                        }
                                        if (!absenceflag)
                                        {
                                            presence = ABSENT;
                                            if (pausesTable.ContainsKey(interval.PauseID))
                                            {
                                                TimeSchemaPauseTO pause = pausesTable[interval.PauseID];
                                                if (interval.StartTime.AddMinutes(pause.EarliestUseTime).TimeOfDay <= DateTime.Now.TimeOfDay
                                                    && interval.EndTime.AddMinutes(-pause.LatestUseTime).TimeOfDay >= DateTime.Now.TimeOfDay
                                                    && emplLoc.EventTime.Date == DateTime.Now.Date
                                                    && emplLoc.EventTime.TimeOfDay <= DateTime.Now.TimeOfDay)
                                                {
                                                    if (emplLoc.EventTime.AddMinutes(pause.PauseDuration).AddMinutes(pause.PauseOffset).TimeOfDay >= DateTime.Now.TimeOfDay)
                                                        presence = ABSENTPAUSE;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        presence = PRESENT;
                                    }
                                }
                                else if (emplLoc.LocationID != locationOUT)
                                {
                                    presence = OUTWRKHRS;
                                }
                                else
                                {
                                    presence = PRESENT;
                                }
                            }

                            if (presence == -1 && emplLoc.LocationID != locationOUT)
                            {
                                presence = OUTWRKHRS;
                            }

                            if (presence == -1 && emplLoc.LocationID == locationOUT)
                            {
                                presence = PRESENT;
                            }


                        }
                        emplLoc.Type = presence;

                        if (cbMarkViolation.Checked)
                        {

                            switch (presence)
                            {
                                case ABSENT:

                                    if (cbOutsidePause.Checked)
                                    {
                                        currentEmployeesList.Add(emplLoc);
                                    }
                                    break;
                                case ABSENTPAUSE:

                                    if (cbDuringPause.Checked)
                                    {
                                        currentEmployeesList.Add(emplLoc);
                                    }
                                    break;
                                case OUTWRKHRS:

                                    if (cbOutWH.Checked)
                                    {
                                        currentEmployeesList.Add(emplLoc);
                                    }
                                    break;
                                case PRESENT:

                                    if (cbEmplInPresence.Checked)
                                    {
                                        currentEmployeesList.Add(emplLoc);
                                    }
                                    break;
                                case WHOLEDAYABSENCE:

                                    if (!cbNoWholeDayAbsence.Checked)
                                    {
                                        currentEmployeesList.Add(emplLoc);
                                    }
                                    break;

                            }
                        }
                        else
                        {
                            currentEmployeesList.Add(emplLoc);
                        }

                    }
                    currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentEmployeesList, startIndex);
                    lblTotal.Text = rm.GetString("lblTotal", culture) + lvEmployees.Items.Count.ToString().Trim();
                }
                else
                {
                    populateListView(currentEmployeesList, startIndex);
                    MessageBox.Show(rm.GetString("noEmplLocFound", culture));

                }

                currentEmployeeLocation = new EmployeeLocationTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        //private void btnSearch_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        string workingUnit = "";
        //        int employee = -1;
        //        int passType = -1;
        //        string locString = "";

        //        if (cbWorkingUnit.SelectedIndex > 0)
        //        {
        //            workingUnit = cbWorkingUnit.SelectedValue.ToString();

        //            //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
        //            WorkingUnit wu = new WorkingUnit();
        //            if ((int)this.cbWorkingUnit.SelectedValue != -1 && chbHierarhicly.Checked)
        //            {
        //                List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
        //                WorkingUnit workUnit = new WorkingUnit();
        //                foreach (WorkingUnitTO wUnit in wUnits)
        //                {
        //                    if (wUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
        //                    {
        //                        wuList.Add(wUnit);
        //                        workUnit.WUTO = wUnit;
        //                    }
        //                }
        //                if (workUnit.WUTO.ChildWUNumber > 0)
        //                    wuList = wu.FindAllChildren(wuList);
        //                workingUnit = "";
        //                foreach (WorkingUnitTO wunit in wuList)
        //                {
        //                    workingUnit += wunit.WorkingUnitID.ToString().Trim() + ",";
        //                }

        //                if (workingUnit.Length > 0)
        //                {
        //                    workingUnit = workingUnit.Substring(0, workingUnit.Length - 1);
        //                }
        //            }
        //        }
        //        if (cbEmployee.SelectedIndex > 0)
        //        {
        //            employee = (int)cbEmployee.SelectedValue;
        //        }

        //        if (cbPassType.SelectedIndex > 0)
        //        {
        //            passType = (int)cbPassType.SelectedValue;
        //        }
        //        if (lvLocations.SelectedItems.Count > 0)
        //        {
        //            foreach (ListViewItem item in lvLocations.SelectedItems)
        //            {
        //                if (chbLocHierarhicly.Checked)
        //                {
        //                    int selectedLocation = ((LocationTO)item.Tag).LocationID;
        //                    Location loc = new Location();
        //                    loc.LocTO.LocationID = selectedLocation;
        //                    List<LocationTO> locations = loc.Search();

        //                    if (selectedLocation == -1)
        //                    {
        //                        for (int i = locations.Count - 1; i >= 0; i--)
        //                        {
        //                            if (locations[i].LocationID != locations[i].ParentLocationID)
        //                            {
        //                                locations.RemoveAt(i);
        //                            }
        //                        }
        //                    }
        //                    locations = loc.FindAllChildren(locations);
        //                    selectedLocation = -1;

        //                    foreach (LocationTO loc1 in locations)
        //                    {
        //                        locString += loc1.LocationID.ToString().Trim() + ",";
        //                    }
        //                }
        //                else
        //                {
        //                    locString += ((LocationTO)item.Tag).LocationID.ToString().Trim() + ",";
        //                }
        //            }

        //            if (locString.Length > 0)
        //            {
        //                locString = locString.Substring(0, locString.Length - 1);
        //            }
        //        }

        //        currentEmployeesList = new List<EmployeeLocationTO>();
        //        if (workingUnit != "")
        //        {
        //            Employee empl = new Employee();
        //            List<EmployeeLocationTO> emplLocCurrentList = new List<EmployeeLocationTO>();

        //            List<EmployeeTO> wuEmployees = empl.SearchByWU(workingUnit);

        //            for (int i = 0; i < wuEmployees.Count; i++)
        //            {
        //                if (employee == -1)
        //                {
        //                    if (!wuString.Equals(""))
        //                    {
        //                        EmployeeLocation eLoc = new EmployeeLocation();
        //                        eLoc.EmplLocTO.EmployeeID = wuEmployees[i].EmployeeID;
        //                        eLoc.EmplLocTO.PassTypeID = passType;
        //                        if (!chbShowEvents.Checked) currentEmployeesList = eLoc.Search(locString, wuString);
        //                        else currentEmployeesList = eLoc.Search(locString, wuString, dtFrom.Value, dtTo.Value.AddDays(1));

        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(rm.GetString("noEmplLocationPrivilege", culture));
        //                    }

        //                    for (int j = 0; j < emplLocCurrentList.Count; j++)
        //                    {
        //                        currentEmployeesList.Add(emplLocCurrentList[j]);
        //                    }
        //                }
        //                else if (employee == wuEmployees[i].EmployeeID)
        //                {
        //                    if (!wuString.Equals(""))
        //                    {
        //                        EmployeeLocation eLoc = new EmployeeLocation();
        //                        eLoc.EmplLocTO.EmployeeID = employee;
        //                        eLoc.EmplLocTO.PassTypeID = passType;
        //                        if (!chbShowEvents.Checked) currentEmployeesList = eLoc.Search(locString, wuString);
        //                        else currentEmployeesList = eLoc.Search(locString, wuString, dtFrom.Value, dtTo.Value.AddDays(1));
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(rm.GetString("noEmplLocationPrivilege", culture));
        //                    }
        //                    break;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            if (!wuString.Equals(""))
        //            {
        //                EmployeeLocation eLoc = new EmployeeLocation();
        //                eLoc.EmplLocTO.EmployeeID = employee;
        //                eLoc.EmplLocTO.PassTypeID = passType;

        //                if (!chbShowEvents.Checked) currentEmployeesList = eLoc.Search(locString, wuString);
        //                else currentEmployeesList = eLoc.Search(locString, wuString, dtFrom.Value, dtTo.Value.AddDays(1));
        //            }
        //            else
        //            {
        //                MessageBox.Show(rm.GetString("noEmplLocationPrivilege", culture));
        //            }
        //        }

        //        if (currentEmployeesList.Count > 0)
        //        {
        //            startIndex = 0;
        //            if (chbShowEvents.Checked)
        //                timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(this.getSelectedEmployeesString(), dtFrom.Value, dtTo.Value);
        //            else
        //                timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(this.getSelectedEmployeesString(), DateTime.Now.Date, DateTime.Now.Date);
        //            string schemaID = "";
        //            foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
        //            {
        //                schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
        //            }
        //            if (!schemaID.Equals(""))
        //            {
        //                schemaID = schemaID.Substring(0, schemaID.Length - 2);
        //            }

        //            timeSchema = new TimeSchema().Search(schemaID);

        //            currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
        //            populateListView(currentEmployeesList, startIndex);
        //            lblTotal.Text = rm.GetString("lblTotal", culture) + lvEmployees.Items.Count.ToString().Trim();
        //        }
        //        else
        //        {  populateListView(currentEmployeesList, startIndex);
        //            MessageBox.Show(rm.GetString("noEmplLocFound", culture));
                  
        //        }

        //        currentEmployeeLocation = new EmployeeLocationTO();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " EmployeeLocations.btnSearch_Click(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Arrow;
        //    }
        //}

        private string getSelectedEmployeesString()
        {
            string emplString = "";
            try
            {
                if (cbEmployee.SelectedIndex > 0)
                {
                    emplString = cbEmployee.SelectedValue.ToString();
                }
                else
                {
                    foreach (EmployeeTO empl in emplArray)
                    {
                        emplString += empl.EmployeeID + ", ";
                    }
                    if (emplString.Length > 0)
                        emplString = emplString.Substring(0, emplString.Length - 2);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.getSelectedEmployeesString(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return emplString;
        }

        private void EmployeeLocations_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                sortOrder = Constants.sortAsc;
                sortField = EmployeeLocations.EmployeeIndex;
                startIndex = 0;

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.LocationPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    if(wUnit.Status.Equals(Constants.statusActive))
                       wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateWorkingUnitCombo();
                populateEmployeeCombo(-1);
                populateLocationListView();
                populatePassTypeCombo();

                if (!wuString.Equals(""))
                {
                    btnSearch_Click(this, new EventArgs());
                    /*if (lvLocations.SelectedItems.Count == 0)
                        currentEmployeesList = currentEmployeeLocation.Search("", "", "", "", "", "", wuString);
                    else
                        currentEmployeesList = currentEmployeeLocation.Search("", "", "", "", "", locationID.ToString(), wuString);
					populateListView(currentEmployeesList, startIndex);
					lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesList.Count.ToString().Trim();*/
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplLocationPrivilege", culture));
                }

                List<TimeSchemaPauseTO> pauses = new TimeSchemaPause().Search("");
                // get all time schemas
                timeSchemas = new TimeSchema().Search();
                foreach (TimeSchemaPauseTO pause in pauses)
                {
                    if (!pausesTable.ContainsKey(pause.PauseID))
                        pausesTable.Add(pause.PauseID, pause);
                }
                foreach (WorkTimeSchemaTO sch in timeSchemas)
                {
                    if (!timeSchemasTable.ContainsKey(sch.TimeSchemaID))
                        timeSchemasTable.Add(sch.TimeSchemaID, sch);
                }

                List<PassTypeTO> types = new PassType().Search();
                foreach (PassTypeTO pt in types)
                {
                    if (!passTypes.ContainsKey(pt.PassTypeID))
                        passTypes.Add(pt.PassTypeID, pt);
                }

                dtFrom.Value = DateTime.Now.Date;
                dtTo.Value = DateTime.Now.Date.AddDays(1).AddMinutes(-1);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.Load(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrder;

                if (e.Column == sortField)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrder = Constants.sortAsc;
                }

                sortField = e.Column;

                currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployees_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.chbHierarhicly.Checked = this.chbLocHierarhicly.Checked = false;
                if (lvEmployees.SelectedItems.Count > 0)
                {
                    EmployeeLocationTO emplLocTO = (EmployeeLocationTO)lvEmployees.SelectedItems[0].Tag;
                    if (emplLocTO.EmployeeID >= 0)
                    {
                        cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(lvEmployees.SelectedItems[0].SubItems[1].Text);
                        cbEmployee.SelectedIndex = cbEmployee.FindStringExact(lvEmployees.SelectedItems[0].Text.ToString());

                        foreach (ListViewItem item in lvLocations.SelectedItems)
                        {
                            item.Selected = false;
                        }
                        lvLocations.Select();

                        for (int i = 0; i < lvLocations.Items.Count; i++)
                        {
                            ListViewItem item = lvLocations.Items[i];
                            if (item.Text.Trim().Equals(lvEmployees.SelectedItems[0].SubItems[2].Text.Trim()))
                            {
                                item.Selected = true;
                                lvLocations.Select();
                                lvLocations.EnsureVisible(i);
                                break;
                            }
                        }
                        cbPassType.SelectedIndex = cbPassType.FindStringExact(lvEmployees.SelectedItems[0].SubItems[3].Text);
                        //dtpStartDate.Value = emplAbsTO.DateStart;
                        //dtpEndDate.Value = emplAbsTO.DateEnd;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.lvEmployees_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                formClear();
                if (!wuString.Equals(""))
                {
                    currentEmployeesList = new EmployeeLocation().Search("", wuString);
                    startIndex = 0;
                    currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentEmployeesList, startIndex);
                    lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesList.Count.ToString().Trim();
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplLocationPrivilege", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPrev_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populateListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void EmployeeLocations_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.EmployeeLocations_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (currentEmployeesList.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("empl_locations");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("wu_name", typeof(System.String));
                    tableCR.Columns.Add("location", typeof(System.String));
                    tableCR.Columns.Add("pass_type", typeof(System.String));
                    tableCR.Columns.Add("event_time", typeof(System.String));
                    tableCR.Columns.Add("violation", typeof(System.String));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);

                    foreach (EmployeeLocationTO employeeLocation in currentEmployeesList)
                    {
                        DataRow row = tableCR.NewRow();

                        row["employee"] = employeeLocation.EmployeeName;
                        row["wu_name"] = employeeLocation.WUName;
                        row["location"] = employeeLocation.LocName;
                        row["pass_type"] = employeeLocation.PassType;
                        string violation = "";
                        if (emplViolation.ContainsKey(employeeLocation.EmployeeID))
                        {
                            switch (emplViolation[employeeLocation.EmployeeID])
                            {
                                case PRESENT:
                                    violation = "-";
                                    break;
                                case OUTWRKHRS:
                                    violation = rm.GetString("OWH", culture);
                                    break;
                                case ABSENT:
                                    violation = rm.GetString("AOP", culture);
                                    break;
                                case ABSENTPAUSE:
                                    violation = rm.GetString("AP", culture);
                                    break;
                            }
                        }
                        row["violation"] = violation;
                        if (employeeLocation.EventTime == new DateTime(0))
                        {
                            row["event_time"] = "";
                        }
                        else
                        {
                            row["event_time"] = employeeLocation.EventTime.ToString("dd.MM.yyyy HH:mm:ss");
                        }
                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selWorkingUnit = "*";
                    string selEmplName = "*";
                    string selLocation = "*";
                    string selPassType = "*";
                    string selUser = logInUser.Name;

                    if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                        selWorkingUnit = cbWorkingUnit.Text;
                    if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                        selEmplName = cbEmployee.Text;
                    if (cbPassType.SelectedIndex >= 0)
                        selPassType = cbPassType.Text;
                    if (lvLocations.SelectedItems.Count == 0)
                    {
                        selLocation = rm.GetString("all", culture);
                    }
                    else if (lvLocations.SelectedItems.Count == 1)
                    {
                        selLocation = lvLocations.SelectedItems[0].Text.Trim();
                    }
                    else if (lvLocations.SelectedItems.Count < lvLocations.Items.Count)
                    {
                        selLocation = rm.GetString("multipleLocations", culture);
                    }
                    else
                    {
                        selLocation = rm.GetString("allLocations", culture);
                    }

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.EmployeeLocationCRView_sr view = new Reports.Reports_sr.EmployeeLocationCRView_sr(dataSetCR,
                             selWorkingUnit, selEmplName, selPassType, selLocation, selUser);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.EmployeeLocationCRView_en view = new Reports.Reports_en.EmployeeLocationCRView_en(dataSetCR,
                             selWorkingUnit, selEmplName, selPassType, selLocation, selUser);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.EmployeeLocationCRView_fi view = new Reports.Reports_fi.EmployeeLocationCRView_fi(dataSetCR,
                             selWorkingUnit, selEmplName, selPassType, selLocation, selUser);
                        view.ShowDialog(this);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWorkingUnit.SelectedValue is int)
                {
                    if ((int)cbWorkingUnit.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                EmployeeLocationsSummary emplLocSummary = new EmployeeLocationsSummary();
                emplLocSummary.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.btnSummary_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbMarkViolation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cbOutWH.Checked = cbMarkViolation.Checked;
                cbOutsidePause.Checked = cbMarkViolation.Checked;
                cbEmplInPresence.Checked = cbMarkViolation.Checked;
                cbDuringPause.Checked = cbMarkViolation.Checked;
                cbAbsent.Checked = cbMarkViolation.Checked;
                gbMarkViolation.Enabled = cbMarkViolation.Checked;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.cbMarkViolation_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbAbsent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cbDuringPause.Checked = cbAbsent.Checked;
                cbOutsidePause.Checked = cbAbsent.Checked;
                cbNoWholeDayAbsence.Checked = cbAbsent.Checked;
                cbNoWholeDayAbsence.Enabled = cbAbsent.Checked;
                cbDuringPause.Enabled = cbAbsent.Checked;
                cbOutsidePause.Enabled = cbAbsent.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.cbAbsent_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbOutsidePause_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!cbOutsidePause.Checked && !cbDuringPause.Checked && !cbNoWholeDayAbsence.Checked)
                    cbAbsent.Checked = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.cbOutsidePause_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbDuringPause_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!cbOutsidePause.Checked && !cbDuringPause.Checked && !cbNoWholeDayAbsence.Checked)
                    cbAbsent.Checked = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.cbDuringPause_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
       
        private void chbShowEvents_CheckedChanged(object sender, EventArgs e)
        {
            if (chbShowEvents.Checked) gbEvents.Enabled = true;
            else gbEvents.Enabled = false;
        }

        private void cbUnjustified_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbUnjustified.Checked)
                {
                    cbPassType.Enabled = false;
                    lvLocations.Enabled = false;
                    cbMarkViolation.Checked = true;
                    cbMarkViolation.Enabled = false;
                    gbMarkViolation.Enabled = false;
                    cbAbsent.Checked = true;
                    cbEmplInPresence.Checked = false;
                    cbOutsidePause.Checked = true;
                    cbOutWH.Checked = false;
                    cbNoWholeDayAbsence.Checked = true;
                    gbEvents.Enabled = false;
                    chbShowEvents.Enabled = false;
                    chbShowEvents.Checked = false;
                    gbShow.Enabled = false;
                    rbJustData.Checked = true;
                    rbColor.Checked = false;
                    cbDuringPause.Checked = true;

                    foreach (ListViewItem item in lvLocations.Items)
                    {
                        if (item.Text.Equals(Constants.DirectionOut))
                        {
                            item.Selected = true;
                            break;
                        }

                    }
                    for (int i = 0; i < cbPassType.Items.Count; i++)
                    {
                        if (((PassTypeTO)cbPassType.Items[i]).PassTypeID == Constants.regularWork)
                        {
                            cbPassType.SelectedIndex = i;
                            break;

                        }
                    }
                }
                else
                {
                    cbPassType.Enabled = true;
                    lvLocations.Enabled = true;
                    cbMarkViolation.Checked = false;
                    cbMarkViolation.Enabled = true;
                    gbMarkViolation.Enabled = false;
                    cbAbsent.Checked = false;
                    cbEmplInPresence.Checked = false;
                    cbOutsidePause.Checked = false;
                    cbOutWH.Checked = false;
                    cbEmplInPresence.Enabled = true;
                    cbOutsidePause.Enabled = true;
                    cbNoWholeDayAbsence.Enabled = true;
                    cbNoWholeDayAbsence.Checked = false;
                    gbEvents.Enabled = false;
                    chbShowEvents.Enabled = true;
                    chbShowEvents.Checked = false;
                    gbShow.Enabled = true;
                    rbJustData.Checked = true;
                    rbColor.Checked = false;
                    cbDuringPause.Checked = false;
                    cbDuringPause.Enabled = true;

                    cbPassType.SelectedIndex = 0;
                    foreach (ListViewItem item in lvLocations.Items)
                    {
                        if (item.Text.Equals(Constants.DirectionOut))
                        {
                            item.Selected = false;
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLocations.cbUnjustified_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
