using System;
using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Data;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;

using Common;
using Util;
using ReaderInterface;
using TransferObjects;
using WIA;
using ACTAConfigManipulation;
using CelikWrapper;

namespace UI
{
	/// <summary>
	/// Summary description for Visitors.
	/// </summary>
	public class Visitors : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbFirstName;
		private System.Windows.Forms.RadioButton rbIdentificationCard;
		private System.Windows.Forms.RadioButton rbJMBG;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.ComboBox cbPerson;
        private System.Windows.Forms.ComboBox cbVisitor;
        private System.Windows.Forms.RichTextBox rtbRemarks;
        private System.Windows.Forms.TextBox tbJMBG;
        private System.Windows.Forms.TextBox tbIdentificationCard;
        private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.Label l6;
        private System.Windows.Forms.Label l2;
        private System.Windows.Forms.Label l3;
        private System.Windows.Forms.Label l4;
        private System.Windows.Forms.Label l5;
        private System.Windows.Forms.Label l7;
        private System.Windows.Forms.Label l1;
        private System.Windows.Forms.Label l8;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCardNum;
        private System.Windows.Forms.Label lblIdentification;
        private System.Windows.Forms.Label lblJMBG;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblVisitDescr;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.Label lblIdentificationCard;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.Button btnFromReader;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private Visit currentVisit = null;

        private CultureInfo culture;
        private ResourceManager rm;
        private DebugLog log;

		private string type = ""; 
        private DateTime startDate;	
        private List<ReaderTO> readers = new List<ReaderTO>();
		private List<LocationTO> locations = new List<LocationTO>();
		private List<WorkingUnitTO> workingunits = new List<WorkingUnitTO>();
		protected Visit slectedVisit = new Visit();
		
        private string visitorCode = "";
        private string gates = "";
        private string wuString = "";
        private ComboBox cbVisitDescr;
        private string wuStringVisitor = "";
        private List<string> statuses = new List<string>();
        IReaderInterface readerInterface;
        private GroupBox gbVisitor;
        private GroupBox gbScanDoc;
        private Button btnScan;
        private PictureBox pbScanDoc;
        private Button btnClear;

        static int desktopReaderCOMPort = 0;
        private Button btnWUTree;
        private Button btnLocationTree;
        private DateTimePicker dtpTo;
        private Label lblTo;
        private DateTimePicker dtpFrom;
        private Label lblFrom;
        byte[] scanImageBytes = new byte[0];

        string offline = "";

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;

        bool readPermission = false;
        bool addPermission = false;
        bool updatePermission = false;
        private Button btnReadCredentail;
        bool deletePermission = false;

        //credentail reading properties
        //CelikWrapper.CelikWrapper celik;
        //int cardReaderResponse = 0;
        CelikWrapper.EID_DOCUMENT_DATA dokumentData;
        CelikWrapper.EID_FIXED_PERSONAL_DATA fixedPersonalData;
        CelikWrapper.EID_PORTRAIT portrait;
        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        private PictureBox pbNoImage;
        string reader = "";

        //for USB smart card reader detection
        [DllImport("WinScard.dll")]
        public static extern int SCardEstablishContext(uint dwScope, int nNotUsed1, int nNotUsed2, ref int phContext);
        [DllImport("WinScard.dll")]
        public static extern int SCardListReaderGroups(int hContext, ref string cGroups, ref int nStringSize);
        [DllImport("WinScard.dll")]
        public static extern int SCardListReaders(int hContext, string cGroups, ref string cReaderLists, ref int nReaderCount);
        private int nContext; 

		public Visitors(string type)
		{
            try
            {
                InitializeComponent();

                this.type = type;
                //this.employees1 = (ArrayList)deepCopy(employees, true);

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                this.CenterToScreen();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.GateResource", typeof(Visitors).Assembly);

                setLanguage();

                currentVisit = new Visit();

                ReaderFactory.TechnologyType = new Reader().GetDefaultTechnology();
                readerInterface = ReaderFactory.GetReader;

                visitorCode = ConfigurationManager.AppSettings["VisitorsCode"];

                if (visitorCode == null)
                {
                    MessageBox.Show(rm.GetString("noVisitorsParameters", culture));

                    ConfigAdd conf = new ConfigAdd(rm.GetString("Visitors", culture));

                    conf.ShowDialog();

                    visitorCode = ConfigurationManager.AppSettings["VisitorsCode"];
                }
                
                gates = ConfigurationManager.AppSettings["Gates"];

                if (gates == null)
                {
                    MessageBox.Show(rm.GetString("noGates", culture));

                    ConfigAdd conf = new ConfigAdd(rm.GetString("Gates", culture));

                    conf.ShowDialog();

                    gates = ConfigurationManager.AppSettings["Gates"];
                }

                if (gates == null || visitorCode == null)
                {
                    return;
                }
                else
                {                    
                    readers = new Reader().Search(gates);
                    Location location = new Location();
                    location.LocTO.Status = Constants.DefaultStateActive.Trim();
                    List<LocationTO> loc = location.Search();

                    foreach (LocationTO currentLocation in loc)
                    {
                        foreach (ReaderTO currentReader in readers)
                        {
                            if (currentReader.A0LocID.ToString().Trim().Equals(currentLocation.LocationID.ToString().Trim())
                                || currentReader.A1LocID.ToString().Trim().Equals(currentLocation.LocationID.ToString().Trim()))
                            {
                                if (!locations.Contains(currentLocation))
                                {
                                    locations.Add(currentLocation);
                                    break;
                                }
                            }
                        }
                    }

                    WorkingUnit workingunit = new WorkingUnit();
                    workingunit.WUTO.Status = Constants.DefaultStateActive;
                    List<WorkingUnitTO> wuActive = workingunit.Search();

                    workingunit = new WorkingUnit();
                    workingunit.WUTO.WorkingUnitID = int.Parse(visitorCode.Trim());
                    List<WorkingUnitTO> wUnitsVisitors = workingunit.Search();
                    wUnitsVisitors = workingunit.FindAllChildren(wUnitsVisitors);

                    WorkingUnit workingunit1 = new WorkingUnit();
                    workingunit1.WUTO.WorkingUnitID = Constants.basicVisitorCode;
                    List<WorkingUnitTO> wUnitsVisitorsAll = workingunit1.Search();
                    wUnitsVisitorsAll = workingunit1.FindAllChildren(wUnitsVisitorsAll);

                    foreach (WorkingUnitTO wu in wuActive)
                    {
                        bool visitWU = false;
                        foreach (WorkingUnitTO wuV in wUnitsVisitorsAll)
                        {
                            if (wuV.WorkingUnitID == wu.WorkingUnitID)
                            {
                                visitWU = true;
                                break;
                            }
                        }
                        if (!visitWU)
                        {
                            workingunits.Add(wu);
                            wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                        }
                    }
                    if (wuString.Length > 0)
                    {
                        wuString = wuString.Substring(0, wuString.Length - 1);
                    }

                    foreach (WorkingUnitTO wuV in wUnitsVisitors)
                    {
                        wuStringVisitor += wuV.WorkingUnitID.ToString().Trim() + ",";
                    }
                    if (wuStringVisitor.Length > 0)
                    {
                        wuStringVisitor = wuStringVisitor.Substring(0, wuStringVisitor.Length - 1);
                    }

                    string message = ValidateStrings();
                    if (!message.Equals(""))
                    {
                        MessageBox.Show(message);
                        this.Close();
                    }

                    pbScanDoc.Width = (int)((Constants.scanDocWidth / Constants.mm2inch) * Constants.DpiX);
                    pbScanDoc.Height = (int)((Constants.scanDocHeight / Constants.mm2inch) * Constants.DpiY);

                    dtpFrom.Value = DateTime.Now;
                    dtpTo.Value = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.Visitors(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

        private string ValidateStrings()
        {
            StringBuilder sb = new StringBuilder();

            if (gates == "")
            {
                sb.Append("\n" + rm.GetString("noGatesInConfig", culture) + ", ");
            }

            if (visitorCode == "")
            {
                sb.Append("\n" + rm.GetString("noVisitorInConfig", culture) + ", ");
            }
            else
            {
                if (wuStringVisitor == "")
                {
                    sb.Append("\n" + rm.GetString("noVisitors", culture) + ", ");
                }
                if (wuString == "")
                {
                    sb.Append("\n" + rm.GetString("noEmployees", culture) + ", ");
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }

            return sb.ToString();
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Visitors));
            this.lblCardNum = new System.Windows.Forms.Label();
            this.tbJMBG = new System.Windows.Forms.TextBox();
            this.lblIdentification = new System.Windows.Forms.Label();
            this.lblJMBG = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.cbVisitor = new System.Windows.Forms.ComboBox();
            this.rbIdentificationCard = new System.Windows.Forms.RadioButton();
            this.rbJMBG = new System.Windows.Forms.RadioButton();
            this.lblLastName = new System.Windows.Forms.Label();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.cbPerson = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblVisitDescr = new System.Windows.Forms.Label();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rtbRemarks = new System.Windows.Forms.RichTextBox();
            this.lblIdentificationCard = new System.Windows.Forms.Label();
            this.tbIdentificationCard = new System.Windows.Forms.TextBox();
            this.l6 = new System.Windows.Forms.Label();
            this.l2 = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.l3 = new System.Windows.Forms.Label();
            this.l4 = new System.Windows.Forms.Label();
            this.l5 = new System.Windows.Forms.Label();
            this.l7 = new System.Windows.Forms.Label();
            this.l1 = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.l8 = new System.Windows.Forms.Label();
            this.btnFromReader = new System.Windows.Forms.Button();
            this.cbVisitDescr = new System.Windows.Forms.ComboBox();
            this.gbVisitor = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.gbScanDoc = new System.Windows.Forms.GroupBox();
            this.pbNoImage = new System.Windows.Forms.PictureBox();
            this.btnReadCredentail = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pbScanDoc = new System.Windows.Forms.PictureBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.gbVisitor.SuspendLayout();
            this.gbScanDoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNoImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScanDoc)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCardNum
            // 
            this.lblCardNum.Location = new System.Drawing.Point(35, 22);
            this.lblCardNum.Name = "lblCardNum";
            this.lblCardNum.Size = new System.Drawing.Size(100, 23);
            this.lblCardNum.TabIndex = 4;
            this.lblCardNum.Text = "Card number:";
            this.lblCardNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbJMBG
            // 
            this.tbJMBG.Location = new System.Drawing.Point(141, 88);
            this.tbJMBG.MaxLength = 13;
            this.tbJMBG.Name = "tbJMBG";
            this.tbJMBG.Size = new System.Drawing.Size(176, 20);
            this.tbJMBG.TabIndex = 11;
            this.tbJMBG.TextChanged += new System.EventHandler(this.tbJMBG_TextChanged);
            this.tbJMBG.Leave += new System.EventHandler(this.tbJMBG_Leave);
            // 
            // lblIdentification
            // 
            this.lblIdentification.Location = new System.Drawing.Point(23, 54);
            this.lblIdentification.Name = "lblIdentification";
            this.lblIdentification.Size = new System.Drawing.Size(112, 23);
            this.lblIdentification.TabIndex = 7;
            this.lblIdentification.Text = "Identification:";
            this.lblIdentification.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblJMBG
            // 
            this.lblJMBG.Location = new System.Drawing.Point(35, 86);
            this.lblJMBG.Name = "lblJMBG";
            this.lblJMBG.Size = new System.Drawing.Size(100, 23);
            this.lblJMBG.TabIndex = 10;
            this.lblJMBG.Text = "PIN:";
            this.lblJMBG.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFirstName
            // 
            this.tbFirstName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbFirstName.Location = new System.Drawing.Point(141, 153);
            this.tbFirstName.MaxLength = 64;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(176, 20);
            this.tbFirstName.TabIndex = 17;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(35, 148);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(100, 23);
            this.lblFirstName.TabIndex = 16;
            this.lblFirstName.Text = "First name:";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbVisitor
            // 
            this.cbVisitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVisitor.Location = new System.Drawing.Point(141, 24);
            this.cbVisitor.Name = "cbVisitor";
            this.cbVisitor.Size = new System.Drawing.Size(176, 21);
            this.cbVisitor.TabIndex = 5;
            this.cbVisitor.SelectedIndexChanged += new System.EventHandler(this.cbVisitor_SelectedIndexChanged);
            // 
            // rbIdentificationCard
            // 
            this.rbIdentificationCard.Location = new System.Drawing.Point(215, 54);
            this.rbIdentificationCard.Name = "rbIdentificationCard";
            this.rbIdentificationCard.Size = new System.Drawing.Size(121, 24);
            this.rbIdentificationCard.TabIndex = 9;
            this.rbIdentificationCard.Text = "Identification card";
            this.rbIdentificationCard.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbJMBG
            // 
            this.rbJMBG.Checked = true;
            this.rbJMBG.Location = new System.Drawing.Point(141, 54);
            this.rbJMBG.Name = "rbJMBG";
            this.rbJMBG.Size = new System.Drawing.Size(68, 24);
            this.rbJMBG.TabIndex = 8;
            this.rbJMBG.TabStop = true;
            this.rbJMBG.Text = "PIN";
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(35, 182);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(100, 23);
            this.lblLastName.TabIndex = 19;
            this.lblLastName.Text = "Last name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(141, 214);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(176, 21);
            this.cbWorkingUnit.TabIndex = 23;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // cbPerson
            // 
            this.cbPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPerson.Location = new System.Drawing.Point(141, 246);
            this.cbPerson.Name = "cbPerson";
            this.cbPerson.Size = new System.Drawing.Size(176, 21);
            this.cbPerson.TabIndex = 26;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(26, 212);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(109, 23);
            this.lblWU.TabIndex = 22;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(7, 244);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(128, 23);
            this.lblEmployee.TabIndex = 25;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(35, 278);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(100, 23);
            this.lblLocation.TabIndex = 27;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVisitDescr
            // 
            this.lblVisitDescr.Location = new System.Drawing.Point(35, 308);
            this.lblVisitDescr.Name = "lblVisitDescr";
            this.lblVisitDescr.Size = new System.Drawing.Size(100, 23);
            this.lblVisitDescr.TabIndex = 30;
            this.lblVisitDescr.Text = "Visit description:";
            this.lblVisitDescr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRemarks
            // 
            this.lblRemarks.Location = new System.Drawing.Point(35, 342);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(100, 23);
            this.lblRemarks.TabIndex = 33;
            this.lblRemarks.Text = "Remarks:";
            this.lblRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(202, 507);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 40;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(283, 507);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 41;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // rtbRemarks
            // 
            this.rtbRemarks.Location = new System.Drawing.Point(141, 344);
            this.rtbRemarks.MaxLength = 132;
            this.rtbRemarks.Name = "rtbRemarks";
            this.rtbRemarks.Size = new System.Drawing.Size(176, 88);
            this.rtbRemarks.TabIndex = 34;
            this.rtbRemarks.WordWrap = false;
            // 
            // lblIdentificationCard
            // 
            this.lblIdentificationCard.Location = new System.Drawing.Point(35, 118);
            this.lblIdentificationCard.Name = "lblIdentificationCard";
            this.lblIdentificationCard.Size = new System.Drawing.Size(100, 23);
            this.lblIdentificationCard.TabIndex = 13;
            this.lblIdentificationCard.Text = "Identification card:";
            this.lblIdentificationCard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbIdentificationCard
            // 
            this.tbIdentificationCard.Enabled = false;
            this.tbIdentificationCard.Location = new System.Drawing.Point(141, 120);
            this.tbIdentificationCard.Name = "tbIdentificationCard";
            this.tbIdentificationCard.Size = new System.Drawing.Size(176, 20);
            this.tbIdentificationCard.TabIndex = 14;
            this.tbIdentificationCard.Leave += new System.EventHandler(this.tbIdentificationCard_Leave);
            // 
            // l6
            // 
            this.l6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l6.ForeColor = System.Drawing.Color.Red;
            this.l6.Location = new System.Drawing.Point(323, 214);
            this.l6.Name = "l6";
            this.l6.Size = new System.Drawing.Size(15, 23);
            this.l6.TabIndex = 24;
            this.l6.Text = "*";
            this.l6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // l2
            // 
            this.l2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l2.ForeColor = System.Drawing.Color.Red;
            this.l2.Location = new System.Drawing.Point(323, 88);
            this.l2.Name = "l2";
            this.l2.Size = new System.Drawing.Size(14, 23);
            this.l2.TabIndex = 12;
            this.l2.Text = "*";
            this.l2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(141, 184);
            this.tbLastName.MaxLength = 64;
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(176, 20);
            this.tbLastName.TabIndex = 20;
            // 
            // l3
            // 
            this.l3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l3.ForeColor = System.Drawing.Color.Red;
            this.l3.Location = new System.Drawing.Point(323, 120);
            this.l3.Name = "l3";
            this.l3.Size = new System.Drawing.Size(15, 23);
            this.l3.TabIndex = 15;
            this.l3.Text = "*";
            this.l3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.l3.Visible = false;
            // 
            // l4
            // 
            this.l4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l4.ForeColor = System.Drawing.Color.Red;
            this.l4.Location = new System.Drawing.Point(323, 153);
            this.l4.Name = "l4";
            this.l4.Size = new System.Drawing.Size(15, 23);
            this.l4.TabIndex = 18;
            this.l4.Text = "*";
            this.l4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // l5
            // 
            this.l5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l5.ForeColor = System.Drawing.Color.Red;
            this.l5.Location = new System.Drawing.Point(323, 184);
            this.l5.Name = "l5";
            this.l5.Size = new System.Drawing.Size(15, 23);
            this.l5.TabIndex = 21;
            this.l5.Text = "*";
            this.l5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // l7
            // 
            this.l7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l7.ForeColor = System.Drawing.Color.Red;
            this.l7.Location = new System.Drawing.Point(323, 310);
            this.l7.Name = "l7";
            this.l7.Size = new System.Drawing.Size(15, 23);
            this.l7.TabIndex = 32;
            this.l7.Text = "*";
            this.l7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // l1
            // 
            this.l1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l1.ForeColor = System.Drawing.Color.Red;
            this.l1.Location = new System.Drawing.Point(323, 24);
            this.l1.Name = "l1";
            this.l1.Size = new System.Drawing.Size(18, 23);
            this.l1.TabIndex = 6;
            this.l1.Text = "*";
            this.l1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(141, 280);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(176, 21);
            this.cbLocation.TabIndex = 28;
            // 
            // l8
            // 
            this.l8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l8.ForeColor = System.Drawing.Color.Red;
            this.l8.Location = new System.Drawing.Point(323, 280);
            this.l8.Name = "l8";
            this.l8.Size = new System.Drawing.Size(15, 23);
            this.l8.TabIndex = 29;
            this.l8.Text = "*";
            this.l8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFromReader
            // 
            this.btnFromReader.Location = new System.Drawing.Point(26, 507);
            this.btnFromReader.Name = "btnFromReader";
            this.btnFromReader.Size = new System.Drawing.Size(160, 23);
            this.btnFromReader.TabIndex = 39;
            this.btnFromReader.Text = "From reader";
            this.btnFromReader.Click += new System.EventHandler(this.btnFromReader_Click);
            // 
            // cbVisitDescr
            // 
            this.cbVisitDescr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVisitDescr.Location = new System.Drawing.Point(141, 310);
            this.cbVisitDescr.Name = "cbVisitDescr";
            this.cbVisitDescr.Size = new System.Drawing.Size(176, 21);
            this.cbVisitDescr.TabIndex = 31;
            // 
            // gbVisitor
            // 
            this.gbVisitor.Controls.Add(this.dtpTo);
            this.gbVisitor.Controls.Add(this.lblTo);
            this.gbVisitor.Controls.Add(this.dtpFrom);
            this.gbVisitor.Controls.Add(this.lblFrom);
            this.gbVisitor.Controls.Add(this.btnLocationTree);
            this.gbVisitor.Controls.Add(this.btnWUTree);
            this.gbVisitor.Controls.Add(this.cbVisitor);
            this.gbVisitor.Controls.Add(this.cbVisitDescr);
            this.gbVisitor.Controls.Add(this.rbIdentificationCard);
            this.gbVisitor.Controls.Add(this.btnFromReader);
            this.gbVisitor.Controls.Add(this.rbJMBG);
            this.gbVisitor.Controls.Add(this.l8);
            this.gbVisitor.Controls.Add(this.lblCardNum);
            this.gbVisitor.Controls.Add(this.cbLocation);
            this.gbVisitor.Controls.Add(this.lblIdentification);
            this.gbVisitor.Controls.Add(this.l1);
            this.gbVisitor.Controls.Add(this.lblJMBG);
            this.gbVisitor.Controls.Add(this.l7);
            this.gbVisitor.Controls.Add(this.lblFirstName);
            this.gbVisitor.Controls.Add(this.l5);
            this.gbVisitor.Controls.Add(this.lblLastName);
            this.gbVisitor.Controls.Add(this.l4);
            this.gbVisitor.Controls.Add(this.cbWorkingUnit);
            this.gbVisitor.Controls.Add(this.l3);
            this.gbVisitor.Controls.Add(this.cbPerson);
            this.gbVisitor.Controls.Add(this.l2);
            this.gbVisitor.Controls.Add(this.lblWU);
            this.gbVisitor.Controls.Add(this.l6);
            this.gbVisitor.Controls.Add(this.lblEmployee);
            this.gbVisitor.Controls.Add(this.tbIdentificationCard);
            this.gbVisitor.Controls.Add(this.lblLocation);
            this.gbVisitor.Controls.Add(this.tbLastName);
            this.gbVisitor.Controls.Add(this.lblVisitDescr);
            this.gbVisitor.Controls.Add(this.tbFirstName);
            this.gbVisitor.Controls.Add(this.lblRemarks);
            this.gbVisitor.Controls.Add(this.tbJMBG);
            this.gbVisitor.Controls.Add(this.btnSave);
            this.gbVisitor.Controls.Add(this.lblIdentificationCard);
            this.gbVisitor.Controls.Add(this.btnCancel);
            this.gbVisitor.Controls.Add(this.rtbRemarks);
            this.gbVisitor.Location = new System.Drawing.Point(566, 12);
            this.gbVisitor.Name = "gbVisitor";
            this.gbVisitor.Size = new System.Drawing.Size(367, 546);
            this.gbVisitor.TabIndex = 3;
            this.gbVisitor.TabStop = false;
            this.gbVisitor.Text = "Visitor data";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(141, 469);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(176, 20);
            this.dtpTo.TabIndex = 38;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(38, 468);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(97, 23);
            this.lblTo.TabIndex = 37;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(141, 443);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(176, 20);
            this.dtpFrom.TabIndex = 36;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(41, 442);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(94, 23);
            this.lblFrom.TabIndex = 35;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(336, 277);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 42;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(336, 214);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 41;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // gbScanDoc
            // 
            this.gbScanDoc.Controls.Add(this.pbNoImage);
            this.gbScanDoc.Controls.Add(this.btnReadCredentail);
            this.gbScanDoc.Controls.Add(this.btnClear);
            this.gbScanDoc.Controls.Add(this.pbScanDoc);
            this.gbScanDoc.Controls.Add(this.btnScan);
            this.gbScanDoc.Location = new System.Drawing.Point(12, 12);
            this.gbScanDoc.Name = "gbScanDoc";
            this.gbScanDoc.Size = new System.Drawing.Size(548, 546);
            this.gbScanDoc.TabIndex = 0;
            this.gbScanDoc.TabStop = false;
            this.gbScanDoc.Text = "Scaned document";
            // 
            // pbNoImage
            // 
            this.pbNoImage.Image = global::UI.GateResource._600px_No_Symbol_svg;
            this.pbNoImage.Location = new System.Drawing.Point(9, 150);
            this.pbNoImage.Name = "pbNoImage";
            this.pbNoImage.Size = new System.Drawing.Size(530, 215);
            this.pbNoImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbNoImage.TabIndex = 4;
            this.pbNoImage.TabStop = false;
            this.pbNoImage.Visible = false;
            // 
            // btnReadCredentail
            // 
            this.btnReadCredentail.Location = new System.Drawing.Point(298, 507);
            this.btnReadCredentail.Name = "btnReadCredentail";
            this.btnReadCredentail.Size = new System.Drawing.Size(160, 23);
            this.btnReadCredentail.TabIndex = 3;
            this.btnReadCredentail.Text = "Read credentail";
            this.btnReadCredentail.UseVisualStyleBackColor = true;
            this.btnReadCredentail.Click += new System.EventHandler(this.btnReadCredentail_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(9, 507);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pbScanDoc
            // 
            this.pbScanDoc.Location = new System.Drawing.Point(9, 22);
            this.pbScanDoc.Name = "pbScanDoc";
            this.pbScanDoc.Size = new System.Drawing.Size(530, 340);
            this.pbScanDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbScanDoc.TabIndex = 1;
            this.pbScanDoc.TabStop = false;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(464, 507);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // Visitors
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(945, 570);
            this.ControlBox = false;
            this.Controls.Add(this.gbScanDoc);
            this.Controls.Add(this.gbVisitor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Visitors";
            this.ShowInTaskbar = false;
            this.Text = "Visitors";
            this.Load += new System.EventHandler(this.Visitors_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Visitors_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Visitors_FormClosing);
            this.gbVisitor.ResumeLayout(false);
            this.gbVisitor.PerformLayout();
            this.gbScanDoc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbNoImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScanDoc)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void setLanguage()
		{
			try
			{
                // Form name
                if (type == "Enter")
                {
                    this.Text = rm.GetString("titleVisitorsEnter", culture);
                }
                else
                {
                    this.Text = rm.GetString("titleVisitorsExit", culture);
                }

                gbVisitor.Text = rm.GetString("gbVisitor", culture);
                gbScanDoc.Text = rm.GetString("gbScanDoc", culture);

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
                btnFromReader.Text = rm.GetString("btnFromReader", culture);
                btnScan.Text = rm.GetString("btnScan", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
                btnReadCredentail.Text = rm.GetString("btnReadCredentail", culture);
				
				// label's text
				lblCardNum.Text = rm.GetString("lblCardNum", culture);
				lblIdentification.Text = rm.GetString("lblIdentification", culture);
				rbJMBG.Text = rm.GetString("lblJMBG", culture);
				rbIdentificationCard.Text = rm.GetString("lblIdentificationCard", culture);
				lblJMBG.Text = rm.GetString("lblJMBG", culture) + ":";
				lblIdentificationCard.Text = rm.GetString("lblIdentificationCard", culture) + ":";
				lblFirstName.Text = rm.GetString("lblFirstName", culture);
				lblLastName.Text = rm.GetString("lblLastName", culture);
				lblWU.Text = rm.GetString("lblWU", culture);
				lblEmployee.Text = rm.GetString("lblEmployee", culture);
				lblLocation.Text = rm.GetString("lblLocation", culture);
				lblVisitDescr.Text = rm.GetString("lblVisitDescr", culture);
				lblRemarks.Text = rm.GetString("lblRemarks", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Visitors.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
		
		private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (((RadioButton)sender).Checked == true)
                {
                    tbJMBG.Enabled = false;
                    tbIdentificationCard.Enabled = true;
                    tbJMBG.Text = "";
                    tbIdentificationCard.Text = "";
                    l2.Visible = false;
                    l3.Visible = true;
                }
                else
                {
                    tbJMBG.Enabled = true;
                    tbIdentificationCard.Enabled = false;
                    tbJMBG.Text = "";
                    tbIdentificationCard.Text = "";
                    l2.Visible = true;
                    l3.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.radioButton2_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void Visitors_Load(object sender, System.EventArgs e)
		{
            try
            {
                

                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);
                    
                if (gates == null || visitorCode == null)
                {
                    this.Close();
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    populateVisitorCombo();
                    populateWorkingUnitCombo();
                    populateLocationCombo();
                    populateVisitDescrCombo();

                    if (type.Equals("Exit"))
                    {
                        this.lblIdentification.Visible = false;
                        this.rbJMBG.Visible = false;
                        this.rbIdentificationCard.Visible = false;
                        this.lblJMBG.Enabled = false;
                        this.tbJMBG.Enabled = false;
                        this.lblIdentificationCard.Enabled = false;
                        this.tbIdentificationCard.Enabled = false;
                        this.lblFirstName.Enabled = false;
                        this.tbFirstName.Enabled = false;
                        this.lblLastName.Enabled = false;
                        this.tbLastName.Enabled = false;
                        this.lblWU.Enabled = false;
                        this.cbWorkingUnit.Enabled = false;
                        this.lblEmployee.Enabled = false;
                        this.cbPerson.Enabled = false;
                        this.lblVisitDescr.Enabled = false;
                        this.cbVisitDescr.Enabled = false;
                        this.lblLocation.Enabled = false;
                        this.cbLocation.Enabled = false;
                        this.l2.Visible = false;
                        this.l3.Visible = false;
                        this.l4.Visible = false;
                        this.l5.Visible = false;
                        this.l6.Visible = false;
                        this.l7.Visible = false;
                        this.l8.Visible = false;
                        this.btnScan.Enabled = false;
                        this.btnReadCredentail.Enabled = false;
                        this.btnClear.Enabled = false;
                        this.btnWUTree.Visible = false;
                        this.btnLocationTree.Visible = false;
                        this.lblFrom.Visible = false;
                        this.lblTo.Visible = false;
                        this.dtpFrom.Visible = false;
                        this.dtpTo.Visible = false;
                    }
                    else
                    {
                        pbNoImage.Visible = true;
                        offline = ConfigurationManager.AppSettings["OfflineVisits"];
                        if (offline == null || offline.Equals(""))
                        {
                            offline = Constants.noL;
                        }

                        if (offline.Trim().ToUpper().Equals(Constants.noL.ToUpper()))
                        {
                            this.lblFrom.Visible = false;
                            this.lblTo.Visible = false;
                            this.dtpFrom.Visible = false;
                            this.dtpTo.Visible = false;
                        }
                    }

                    menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                    currentRoles = NotificationController.GetCurrentRoles();
                    menuItemID = NotificationController.GetCurrentMenuItemID();

                    setVisibility();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " Visitors.Visitors_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populateEmployeeCombo(int wuID)
		{
			try
			{
                Employee empl = new Employee();
				List<EmployeeTO> emplArrayCombo = new List<EmployeeTO>();

                if (wuID == -1)
                {
                    emplArrayCombo = empl.SearchByWUWithStatuses(wuString, statuses);
                }
                else
                {
                    emplArrayCombo = empl.SearchWithStatuses(statuses, wuID.ToString().Trim());
                }		
                
				foreach(EmployeeTO employee in emplArrayCombo)
				{
					employee.LastName += " " + employee.FirstName;
				}
                
				EmployeeTO empl1 = new EmployeeTO();
				empl1.LastName = rm.GetString("all", culture);
				emplArrayCombo.Insert(0, empl1);
				
				cbPerson.DataSource = emplArrayCombo;
				cbPerson.DisplayMember = "LastName";
				cbPerson.ValueMember = "EmployeeID";

                cbPerson.SelectedIndex = 0;

				cbPerson.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Visitors.populateEmployeeCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateWorkingUnitCombo()
		{
			try
			{
                workingunits.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = workingunits;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";

                cbWorkingUnit.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Visitors.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

        private void populateVisitorCombo()
		{
			try
			{
                Employee empl = new Employee();
                List<EmployeeTO> emplArrayCombo = new List<EmployeeTO>();

                if (type.Equals("Enter"))
                {
                    emplArrayCombo = empl.SearchVisitors(wuStringVisitor, statuses, "AVAILABLE");
                }
                else if (type == "Exit")
                {
                    emplArrayCombo = empl.SearchVisitors(wuStringVisitor, statuses, "IN_USE");
                }

                foreach (EmployeeTO employee in emplArrayCombo)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl1 = new EmployeeTO();
                empl1.LastName = rm.GetString("all", culture);
                emplArrayCombo.Insert(0, empl1);

                cbVisitor.DataSource = emplArrayCombo;
				cbVisitor.DisplayMember = "LastName";
				cbVisitor.ValueMember = "EmployeeID";

                cbVisitor.SelectedIndex = 0;

				cbVisitor.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Visitors.populateVisitorCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateLocationCombo()
		{
			try
			{
                LocationTO loc1 = new LocationTO();
                loc1.Name = rm.GetString("all", culture);
                locations.Insert(0, loc1);

                cbLocation.DataSource = locations;
				cbLocation.DisplayMember = "Name";
				cbLocation.ValueMember = "LocationID";
				cbLocation.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " Visitors.populateLocationCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

        private void populateVisitDescrCombo()
        {
            try
            {
                ArrayList purposeList = new ArrayList();
                purposeList.Add(rm.GetString("all", culture));
                purposeList.Add(rm.GetString("privatePurpose", culture));
                purposeList.Add(rm.GetString("officialPurpose", culture));
                purposeList.Add(rm.GetString("otherPurpose", culture));

                cbVisitDescr.DataSource = purposeList;
                cbVisitDescr.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.populateVisitDescrCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void buttonSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbVisitor.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("visitNoCard", culture));
                    return;
                }

                if (type == "Enter")
                {
                    Visit visit = new Visit();
                    visit.receiveTransferObject(visit.Find(cbVisitor.SelectedValue.ToString()));
                    if (visit.VisitID != -1)
                    {
                        MessageBox.Show(rm.GetString("visitExist", culture));
                        return;
                    }

                    currentVisit.EmployeeID = (int)cbVisitor.SelectedValue;

                    if (rbJMBG.Checked == true)
                    {
                        if (tbJMBG.Text.ToString().Trim() == "")
                        {
                            MessageBox.Show(rm.GetString("visitNoJMBG", culture));
                            return;
                        }
                        if (tbJMBG.Text.Length != 13)
                        {
                            MessageBox.Show(rm.GetString("jmbgLength", culture));
                            return;
                        }
                        currentVisit.VisitorJMBG = tbJMBG.Text.ToString();
                    }
                    if (rbIdentificationCard.Checked == true)
                    {
                        if (tbIdentificationCard.Text.ToString().Trim() == "")
                        {
                            MessageBox.Show(rm.GetString("visitNoIdentification", culture));
                            return;
                        }
                        currentVisit.VisitorID = tbIdentificationCard.Text.ToString();
                    }
                    if (tbFirstName.Text.ToString().Trim() == "")
                    {
                        MessageBox.Show(rm.GetString("visitNoFirstName", culture));
                        return;
                    }
                    currentVisit.FirstName = tbFirstName.Text.ToString();
                    if (tbLastName.Text.ToString().Trim() == "")
                    {
                        MessageBox.Show(rm.GetString("visitNoLastName", culture));
                        return;
                    }
                    currentVisit.LastName = tbLastName.Text.ToString();
                    if (cbWorkingUnit.SelectedIndex <= 0)
                    {
                        MessageBox.Show(rm.GetString("visitNoWorkingUnit", culture));
                        return;
                    }
                    currentVisit.VisitedWorkingUnit = (int)cbWorkingUnit.SelectedValue;
                    currentVisit.VisitedPerson = (int)cbPerson.SelectedValue;
                    if (cbLocation.SelectedIndex <= 0)
                    {
                        MessageBox.Show(rm.GetString("visitNoLocation", culture));
                        return;
                    }
                    currentVisit.LocationID = Int32.Parse(cbLocation.SelectedValue.ToString());
                    if (cbVisitDescr.SelectedIndex <= 0)
                    {
                        MessageBox.Show(rm.GetString("visitNoDescr", culture));
                        return;
                    }
                    if (cbVisitDescr.SelectedItem.ToString().Equals(rm.GetString("privatePurpose", culture)))
                    {
                        currentVisit.VisitDescr = Constants.visitorPrivate;
                    }
                    else if (cbVisitDescr.SelectedItem.ToString().Equals(rm.GetString("officialPurpose", culture)))
                    {
                        currentVisit.VisitDescr = Constants.visitorOfficial;
                    }
                    else if (cbVisitDescr.SelectedItem.ToString().Equals(rm.GetString("otherPurpose", culture)))
                    {
                        currentVisit.VisitDescr = Constants.visitorOther;
                    }

                    currentVisit.Remarks = rtbRemarks.Text.ToString();

                    if (offline.Trim().ToUpper().Equals(Constants.yesL.ToUpper()))
                    {
                        currentVisit.DateStart = dtpFrom.Value;
                        currentVisit.DateEnd = dtpTo.Value;
                    }
                    else
                    {
                        currentVisit.DateStart = DateTime.Now;
                    }

                    int visitID = currentVisit.Save();

                    if (visitID > 0)
                    {
                        int inserted = 1;

                        if (scanImageBytes.Length > 1043576)
                        {
                            MessageBox.Show(rm.GetString("imgLarge", culture));
                        }
                        else if (scanImageBytes.Length > 0)
                        {
                            inserted = new VisitorDocFile().Save(visitID, Constants.defaultDocType, scanImageBytes, true);
                        }

                        if (inserted == 1)
                        {
                            DialogResult result;
                            if (offline.Trim().ToUpper().Equals(Constants.yesL.ToUpper()))
                            {
                                result = MessageBox.Show(this, rm.GetString("visitOfflineInserted", culture), "Visitors", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            }
                            else
                            {
                                result = MessageBox.Show(this, rm.GetString("visitInserted", culture), "Visitors", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            }
                            if (result == DialogResult.Yes)
                            {
                                int loc = cbLocation.SelectedIndex;
                                currentVisit.Clear();
                                clearControls(this.Controls);
                                populateVisitorCombo();
                                cbLocation.SelectedIndex = loc;
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("visitNotInserted", culture));
                    }
                }
                else if (type == "Exit")
                {
                    Visit visit = new Visit();
                    visit.receiveTransferObject(visit.Find(cbVisitor.SelectedValue.ToString()));
                    if (visit.VisitID == -1)
                    {
                        MessageBox.Show(rm.GetString("visitNotExist", culture));
                        return;
                    }

                    currentVisit = slectedVisit;

                    currentVisit.EmployeeID = (int)cbVisitor.SelectedValue;
                    currentVisit.DateEnd = DateTime.Now;

                    /*currentVisit.VisitorJMBG = tbJMBG.Text.ToString();
                    currentVisit.FirstName = tbFirstName.Text.ToString();
                    currentVisit.LastName = tbLastName.Text.ToString();
                    currentVisit.VisitorID = tbIdentificationCard.Text.ToString();
                    currentVisit.DateStart = startDate;
                    currentVisit.VisitedWorkingUnit = (int)cbWorkingUnit.SelectedValue;
                    currentVisit.VisitedPerson = (int)cbPerson.SelectedValue;
                    currentVisit.LocationID = (int)cbLocation.SelectedValue;
                    if (cbVisitDescr.SelectedItem.ToString().Equals(rm.GetString("privatePurpose", culture)))
                    {
                        currentVisit.VisitDescr = Constants.visitorPrivate;
                    }
                    else if (cbVisitDescr.SelectedItem.ToString().Equals(rm.GetString("officialPurpose", culture)))
                    {
                        currentVisit.VisitDescr = Constants.visitorOfficial;
                    }
                    else if (cbVisitDescr.SelectedItem.ToString().Equals(rm.GetString("otherPurpose", culture)))
                    {
                        currentVisit.VisitDescr = Constants.visitorOther;
                    }*/
                    currentVisit.Remarks = rtbRemarks.Text.ToString();

                    bool isUpdated = currentVisit.Update(currentVisit.sendTransferObject());
                    if (isUpdated)
                    {
                        DialogResult result = MessageBox.Show(this, rm.GetString("visitUpdated", culture), "Visitors", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            int loc = cbLocation.SelectedIndex;
                            currentVisit.Clear();
                            clearControls(this.Controls);
                            populateVisitorCombo();
                            cbLocation.SelectedIndex = loc;
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("visitNotUpdated", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.WaitCursor;
            }
		}

		private void cbVisitor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                if (type.Equals("Exit"))
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (((ComboBox)sender).SelectedIndex != 0)
                    {
                        Visit visit = new Visit();
                        visit.receiveTransferObject(visit.Find(((ComboBox)sender).SelectedValue.ToString()));
                        slectedVisit = visit;
                        tbJMBG.Text = visit.VisitorJMBG.ToString();
                        tbIdentificationCard.Text = visit.VisitorID.ToString();
                        tbFirstName.Text = visit.FirstName.ToString();
                        tbLastName.Text = visit.LastName.ToString();
                        startDate = visit.DateStart;
                        cbWorkingUnit.SelectedValue = visit.VisitedWorkingUnit;
                        cbPerson.SelectedValue = visit.VisitedPerson;
                        cbLocation.SelectedValue = visit.LocationID;
                        if (visit.VisitDescr.Equals(Constants.visitorPrivate))
                        {
                            cbVisitDescr.SelectedItem = rm.GetString("privatePurpose", culture);
                        }
                        else if (visit.VisitDescr.Equals(Constants.visitorOfficial))
                        {
                            cbVisitDescr.SelectedItem = rm.GetString("officialPurpose", culture);
                        }
                        else if (visit.VisitDescr.Equals(Constants.visitorOther))
                        {
                            cbVisitDescr.SelectedItem = rm.GetString("otherPurpose", culture);
                        }
                        else
                            cbVisitDescr.SelectedItem = rm.GetString("all", culture);

                        rtbRemarks.Text = visit.Remarks.ToString();

                        VisitorDocFileTO visitorDocFile = new VisitorDocFile().FindVisitorDocFileByVisitID(visit.VisitID.ToString());

                        if (visitorDocFile.Content != null && visitorDocFile.Content.Length > 0)
                        {
                            scanImageBytes = visitorDocFile.Content;

                            populatePictureBox();
                        }
                        else
                        {
                            pbScanDoc.Image = null;
                            scanImageBytes = new byte[0];
                            pbNoImage.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.cbVisitor_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void clearControls(Control.ControlCollection controls)
		{
			try
			{
				foreach(Control c in controls)
				{									
					if ( c is TextBox)
					{
						TextBox tb = (TextBox)c;	
						tb.Text = "";
					}
					if ( c is RichTextBox)
					{
						RichTextBox rtb = (RichTextBox)c;	
						rtb.Text = "";
					}
					if ( c is ComboBox)
					{
						ComboBox cb = (ComboBox)c;	
						cb.SelectedIndex = 0;
					}
                    if (c is PictureBox)
                    {
                        PictureBox pb = (PictureBox)c;
                        pb.Image = null;
                        scanImageBytes = new byte[0];
                    }
                    if (c is DateTimePicker)
                    {
                        DateTimePicker dtp = (DateTimePicker)c;
                        dtp.Value = DateTime.Now;
                    }
					if ( c.HasChildren )
					{
						clearControls(c.Controls);
					}
				}
			}
			catch(Exception e)
			{
				throw e;
			}			
		}

		private void cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbWorkingUnit.SelectedValue is int)
                {
                    populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
                else
                {
                    populateEmployeeCombo(-1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void btnFromReader_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //int COMPort = Int32.Parse(ConfigurationManager.AppSettings["DesktopReaderAddress"]);
                uint tagID = 0;
                this.Cursor = Cursors.WaitCursor;
                if (desktopReaderCOMPort == 0) desktopReaderCOMPort = readerInterface.FindDesktopReader();
                this.Cursor = Cursors.Arrow;
                if (desktopReaderCOMPort == 0)
                {
                    MessageBox.Show(rm.GetString("noDesktopReader", culture));
                    return;
                }
                else
                {
                    tagID = UInt32.Parse(readerInterface.GetTagID(desktopReaderCOMPort));
                }

                if (tagID == 0)
                {
                    MessageBox.Show(rm.GetString("noTagOnReader", culture));
                }
                else
                {
                    Employee empl = new Employee();
                    List<EmployeeTO> emplArrayCombo = new List<EmployeeTO>();

                    emplArrayCombo = empl.SearchByTags(tagID.ToString().Trim());

                    foreach (EmployeeTO employee in emplArrayCombo)
                    {
                        employee.LastName += " " + employee.FirstName;
                    }

                    EmployeeTO empl1 = new EmployeeTO();
                    empl1.LastName = rm.GetString("all", culture);
                    emplArrayCombo.Insert(0, empl1);

                    cbVisitor.DataSource = emplArrayCombo;
                    cbVisitor.DisplayMember = "LastName";
                    cbVisitor.ValueMember = "EmployeeID";

                    if (emplArrayCombo.Count > 1)
                        cbVisitor.SelectedIndex = 1;

                    cbVisitor.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.btnFromReader_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbJMBG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if ((rbJMBG.Checked) && (tbJMBG.Text.ToString().Trim() != ""))
                {
                    for (int i = tbJMBG.Text.Length - 1; i >= 0; i--)
                    {
                        try
                        {
                            int digit = Int32.Parse(tbJMBG.Text.Substring(i, 1));
                        }
                        catch
                        {
                            MessageBox.Show(rm.GetString("jmbgNotNum", culture));
                            tbJMBG.Text = tbJMBG.Text.Remove(i, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.tbJMBG_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void Visitors_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Visitors.Visitors_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbJMBG_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!tbJMBG.Text.Trim().Equals(""))
                {
                    this.Cursor = Cursors.WaitCursor;
                    VisitTO visitor = new Visit().FindMAXVisitPIN(tbJMBG.Text.Trim());

                    if (visitor.VisitID != -1)
                    {
                        tbFirstName.Text = visitor.FirstName;
                        tbLastName.Text = visitor.LastName;
                        cbWorkingUnit.SelectedValue = visitor.VisitedWorkingUnit;
                        if (cbWorkingUnit.SelectedValue == null)
                            cbWorkingUnit.SelectedIndex = 0;
                        cbPerson.SelectedValue = visitor.VisitedPerson;
                        if (cbPerson.SelectedValue == null)
                            cbPerson.SelectedIndex = 0;
                        cbLocation.SelectedValue = visitor.LocationID;
                        if (cbLocation.SelectedValue == null)
                            cbLocation.SelectedIndex = 0;
                        string visitDesc = "";
                        switch (visitor.VisitDescr)
                        {
                            case Constants.visitorPrivate:
                                visitDesc = rm.GetString("privatePurpose", culture);
                                break;
                            case Constants.visitorOfficial:
                                visitDesc = rm.GetString("officialPurpose", culture);
                                break;
                            case Constants.visitorOther:
                                visitDesc = rm.GetString("otherPurpose", culture);
                                break;
                        }

                        cbVisitDescr.SelectedIndex = cbVisitDescr.FindStringExact(visitDesc);
                        if (cbVisitDescr.SelectedIndex < 0)
                            cbVisitDescr.SelectedIndex = 0;

                        VisitorDocFileTO visitorDocFile = new VisitorDocFile().FindVisitorDocFileByJMBG(tbJMBG.Text.Trim());

                        if (visitorDocFile.Content != null && visitorDocFile.Content.Length > 0)
                        {
                            scanImageBytes = visitorDocFile.Content;

                            populatePictureBox();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.tbJMBG_Leave(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbIdentificationCard_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!tbIdentificationCard.Text.Trim().Equals(""))
                {
                    this.Cursor = Cursors.WaitCursor;

                    VisitTO visitor = new Visit().FindMAXVisitIdCard(tbIdentificationCard.Text.Trim());

                    if (visitor.VisitID != -1)
                    {
                        tbFirstName.Text = visitor.FirstName;
                        tbLastName.Text = visitor.LastName;
                        cbWorkingUnit.SelectedValue = visitor.VisitedWorkingUnit;
                        if (cbWorkingUnit.SelectedValue == null)
                            cbWorkingUnit.SelectedIndex = 0;
                        cbPerson.SelectedValue = visitor.VisitedPerson;
                        if (cbPerson.SelectedValue == null)
                            cbPerson.SelectedIndex = 0;
                        cbLocation.SelectedValue = visitor.LocationID;
                        if (cbLocation.SelectedValue == null)
                            cbLocation.SelectedIndex = 0;
                        string visitDesc = "";
                        switch (visitor.VisitDescr)
                        {
                            case Constants.visitorPrivate:
                                visitDesc = rm.GetString("privatePurpose", culture);
                                break;
                            case Constants.visitorOfficial:
                                visitDesc = rm.GetString("officialPurpose", culture);
                                break;
                            case Constants.visitorOther:
                                visitDesc = rm.GetString("otherPurpose", culture);
                                break;
                        }

                        cbVisitDescr.SelectedIndex = cbVisitDescr.FindStringExact(visitDesc);
                        if (cbVisitDescr.SelectedIndex < 0)
                            cbVisitDescr.SelectedIndex = 0;

                        VisitorDocFileTO visitorDocFile = new VisitorDocFile().FindVisitorDocFileByID(tbIdentificationCard.Text.Trim());

                        if (visitorDocFile.Content != null && visitorDocFile.Content.Length > 0)
                        {
                            scanImageBytes = visitorDocFile.Content;

                            populatePictureBox();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.tbIdentificationCard_Leave(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                WIA.CommonDialog WIACommonDialog = new CommonDialogClass();
                Device d = WIACommonDialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);

                Items items = WIACommonDialog.ShowSelectItems(d, WiaImageIntent.ColorIntent,
                    WiaImageBias.MaximizeQuality, true, true, false);

                foreach (Item item in items)
                {
                    ImageProcess ImageProcess = new ImageProcess();
                    Object Object1 = (Object)"Convert";
                    ImageProcess.Filters.Add(ImageProcess.FilterInfos.get_Item(ref Object1).FilterID, 0);
                    Object1 = (Object)"FormatID";
                    Object Object2 = (Object)FormatID.wiaFormatJPEG;
                    ImageProcess.Filters[1].Properties.get_Item(ref Object1).set_Value(ref Object2);
                    Object1 = (Object)"Compression";
                    Object2 = (Object)"CCITT4";
                    ImageProcess.Filters[1].Properties.get_Item(ref Object1).set_Value(ref Object2);

                    ImageFile imageFile = (ImageFile)WIACommonDialog.ShowTransfer(item, FormatID.wiaFormatJPEG, false);
                    imageFile = ImageProcess.Apply(imageFile);
                    
                    Vector vector = imageFile.FileData;
                    scanImageBytes = (byte[])vector.get_BinaryData();

                    populatePictureBox();
                }

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " Visitors.btnScan_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
             
                pbScanDoc.Image = null;
                scanImageBytes = new byte[0];
                pbNoImage.Visible = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnLocationTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LocationsTreeView locationsTreeView = new LocationsTreeView(locations);
                locationsTreeView.ShowDialog();
                if (!locationsTreeView.selectedLocation.Equals(""))
                {
                    this.cbLocation.SelectedIndex = cbLocation.FindStringExact(locationsTreeView.selectedLocation);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setVisibility()
        {
            try
            {
                int permission;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
                    addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
                    updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
                    deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
                }

                btnSave.Enabled = addPermission;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

		/*public object deepCopy(ArrayList array, bool doDeepCopy)
		{
			if (doDeepCopy)
			{
				BinaryFormatter BF = new BinaryFormatter();
				MemoryStream memStream = new MemoryStream();
				BF.Serialize(memStream, array);
				memStream.Flush();
				memStream.Position = 0;
				return (BF.Deserialize(memStream));
			}
			else
			{
				return (this.MemberwiseClone());
			}
		}*/

        private void populatePictureBox()
        {
            try
            {
                pbNoImage.Visible = false;
                pbScanDoc.SizeMode = PictureBoxSizeMode.StretchImage;
                MemoryStream memStream = new MemoryStream(scanImageBytes);
                // Set the position to the beginning of the stream.
                memStream.Seek(0, SeekOrigin.Begin);
                Bitmap b = new Bitmap(memStream);
                if (b.Height < pbScanDoc.Height && b.Width < pbScanDoc.Width)
                {
                    pbScanDoc.SizeMode = PictureBoxSizeMode.CenterImage;
                }
                else
                {
                    if (b.Height > b.Width)
                    {
                        b.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    pbScanDoc.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                pbScanDoc.Image = b;
                memStream.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.populatePictureBox(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void populatePictureBoxCredential()
        {
            try
            {
                pbNoImage.Visible = false;
                MemoryStream memStream = new MemoryStream(scanImageBytes);
                // Set the position to the beginning of the stream.
                memStream.Seek(0, SeekOrigin.Begin);
                Bitmap b = new Bitmap(memStream);
                pbScanDoc.SizeMode = PictureBoxSizeMode.CenterImage;
                pbScanDoc.Image = b;

                memStream.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.populatePictureBox(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnReadCredentail_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                scanImageBytes = new byte[0];
                if (pbScanDoc.Image != null)
                {
                    pbScanDoc.Image.Dispose();
                    pbScanDoc.Image = null;
                    pbNoImage.Visible = true;
                }
                tbFirstName.Text = "";
                tbLastName.Text = "";
                tbJMBG.Text = "";
                tbIdentificationCard.Text = "";

                CelikWrapper.CelikWrapper celik = new CelikWrapper.CelikWrapper();
                int cardReaderResponse = celik.EidStartupW(Constants.CelikV);
                reader = getFirstReaderName();
                if (reader.Length == 0)
                {
                    MessageBox.Show(rm.GetString("noSmartCardReader", culture));
                    return;
                }
                int beginReading = celik.EidBeginReadW(reader);

                beginReading = celik.EidReadFixedPersonalDataW(ref fixedPersonalData);
                beginReading = celik.EidReadDocumentDataW(ref dokumentData);
                beginReading = celik.EidReadPortraitW(ref portrait);

                if (beginReading != Constants.EID_OK)
                {
                    MessageBox.Show(rm.GetString((string)Constants.CelikReturnValues[beginReading], culture));
                    log.writeLog(DateTime.Now + " Visitors.Visitors_Load(): CREDENTIAL_READING_ERROR - " + (string)Constants.CelikReturnValues[cardReaderResponse] + "\n");
                    return;
                }
                scanImageBytes = portrait.portrait;
                populatePictureBoxCredential();
                string firstName = Constants.ConvertCyrillicToLatin(enc.GetString(fixedPersonalData.givenName).Substring(0, fixedPersonalData.givenNameSize));
                firstName = firstName.Substring(0, 1) + firstName.Substring(1).ToLower();
                string lastName = Constants.ConvertCyrillicToLatin(enc.GetString(fixedPersonalData.surname).Substring(0, fixedPersonalData.surnameSize));
                lastName = lastName.Substring(0, 1) + lastName.Substring(1).ToLower();
                tbFirstName.Text = firstName;
                tbLastName.Text = lastName;
                tbJMBG.Text = enc.GetString(fixedPersonalData.personalNumber).Substring(0, fixedPersonalData.personalNumberSize);
                tbIdentificationCard.Text = enc.GetString(dokumentData.docRegNo).Substring(0, dokumentData.docRegNoSize);

                int readingEnded = celik.EidEndReadW();
                int ended = celik.EidCleanupW();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.btnReadCredentail_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string getFirstReaderName()
        {
            string reader = "";
            try
            {
                //First step in using smart cards is CSardEstablishContext()
                uint nContext = 2; //system
                int nNotUsed1 = 0;
                int nNotUsed2 = 0;
                this.nContext = 0; //handle to context - SCARDCONTEXT

                int nRetVal1 = SCardEstablishContext(nContext, nNotUsed1, nNotUsed2, ref this.nContext);
                if (nRetVal1 != 0)
                {
                    log.writeLog("Error returned by SCardEstablishContext()");
                    return reader;
                }

                //next we split up the null delimited strings into a string array
                char[] delimiter = new char[1];
                delimiter[0] = Convert.ToChar(0);

                //Next we need to call the SCardListReaderGroups() to find reader groups to use
                string cGroupList = "" + Convert.ToChar(0);
                int nStringSize = -1; //SCARD_AUTOALLOCATE
                int nRetVal2 = SCardListReaderGroups(this.nContext, ref cGroupList, ref nStringSize);
                if (nRetVal2 != 0)
                {
                    log.writeLog("Error returned by SCardListReaderGroups()");
                    return reader;
                }


                string[] cGroups = cGroupList.Split(delimiter);

                string cReaderList = "" + Convert.ToChar(0);
                int nReaderCount = -1;

                // Get the reader list
                int nRetVal4 = SCardListReaders(this.nContext, cGroups[0], ref cReaderList, ref nReaderCount);
                if (nRetVal4 != 0)
                {
                    log.writeLog(" Error returned by SCardListReaders()");
                    return reader;
                }

                string[] cReaders = cReaderList.Split(delimiter);
                if (cReaders.Length > 0)
                {
                    reader = cReaders[0];
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.getFirstReaderName(): " + ex.Message + "\n");
                throw ex;
            }
            return reader;
        }

        private void Visitors_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
               //int ended = celik.EidEndReadW();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Visitors.Visitors_FormClosing(): " + ex.Message + "\n");
                throw ex;
            }
        }
	}
}
