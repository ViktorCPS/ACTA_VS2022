using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    /// <summary>
    /// Summary description for ApplUsers.
    /// </summary>
    public class ApplUsers : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox gbOperators;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblNumOfTries;
        private System.Windows.Forms.Label lblPrivilegeLvl;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUserRole;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cbUserID;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.ComboBox cbPrivilegeLvl;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        ApplUserTO currentUser = null;

        ApplUserTO logInUser;
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;

        bool readPermission = false;
        bool addPermission = false;
        bool updatePermission = false;
        bool deletePermission = false;

        DebugLog log;

        // List View indexes
        const int UserIDIndex = 0;
        const int NameIndex = 1;
        const int DescriptionIndex = 2;
        const int StatusIndex = 3;
        const int NumOfTriesIndex = 4;
        const int LanguageIndex = 5;

        private ListViewItemComparer _comp;

        private CultureInfo culture;
        private System.Windows.Forms.TextBox tbNumOfTries;
        private System.Windows.Forms.Button btnLogView;
        ResourceManager rm;

        //private bool populateFilter = true;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
        //private bool textChanged = true;
        private GroupBox gbUserCategory;
        private ListView lvUserCategory;

        Dictionary<int, ApplUserCategoryTO> categoryDict = new Dictionary<int, ApplUserCategoryTO>();
        List<int> selectedApplUserCategories = new List<int>();
        Dictionary<string, List<int>> categoryXUserDict = new Dictionary<string, List<int>>();
        private ListView lvCategoryDetails;

        private Filter filter;

        public ApplUsers()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentUser = new ApplUserTO();

            logInUser = NotificationController.GetLogInUser();

            this.CenterToScreen();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            rm = new ResourceManager("UI.Resource", typeof(ApplUsers).Assembly);
            setLanguage();

            populateUserIDCombo();
            populateStatusCombo();
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
            this.gbOperators = new System.Windows.Forms.GroupBox();
            this.gbUserCategory = new System.Windows.Forms.GroupBox();
            this.lvUserCategory = new System.Windows.Forms.ListView();
            this.cbPrivilegeLvl = new System.Windows.Forms.ComboBox();
            this.tbNumOfTries = new System.Windows.Forms.TextBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cbUserID = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblPrivilegeLvl = new System.Windows.Forms.Label();
            this.lblNumOfTries = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.lvUsers = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUserRole = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnLogView = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.lvCategoryDetails = new System.Windows.Forms.ListView();
            this.gbOperators.SuspendLayout();
            this.gbUserCategory.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOperators
            // 
            this.gbOperators.Controls.Add(this.gbUserCategory);
            this.gbOperators.Controls.Add(this.cbPrivilegeLvl);
            this.gbOperators.Controls.Add(this.tbNumOfTries);
            this.gbOperators.Controls.Add(this.cbStatus);
            this.gbOperators.Controls.Add(this.tbDesc);
            this.gbOperators.Controls.Add(this.tbName);
            this.gbOperators.Controls.Add(this.cbUserID);
            this.gbOperators.Controls.Add(this.btnSearch);
            this.gbOperators.Controls.Add(this.lblPrivilegeLvl);
            this.gbOperators.Controls.Add(this.lblNumOfTries);
            this.gbOperators.Controls.Add(this.lblStatus);
            this.gbOperators.Controls.Add(this.lblDescription);
            this.gbOperators.Controls.Add(this.lblName);
            this.gbOperators.Controls.Add(this.lblUserID);
            this.gbOperators.Location = new System.Drawing.Point(16, 16);
            this.gbOperators.Name = "gbOperators";
            this.gbOperators.Size = new System.Drawing.Size(610, 256);
            this.gbOperators.TabIndex = 0;
            this.gbOperators.TabStop = false;
            this.gbOperators.Tag = "FILTERABLE";
            this.gbOperators.Text = "Operators";
            this.gbOperators.Enter += new System.EventHandler(this.gbOperators_Enter);
            // 
            // gbUserCategory
            // 
            this.gbUserCategory.Controls.Add(this.lvUserCategory);
            this.gbUserCategory.Location = new System.Drawing.Point(350, 19);
            this.gbUserCategory.Name = "gbUserCategory";
            this.gbUserCategory.Size = new System.Drawing.Size(242, 188);
            this.gbUserCategory.TabIndex = 13;
            this.gbUserCategory.TabStop = false;
            this.gbUserCategory.Text = "User category";
            // 
            // lvUserCategory
            // 
            this.lvUserCategory.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvUserCategory.FullRowSelect = true;
            this.lvUserCategory.GridLines = true;
            this.lvUserCategory.HideSelection = false;
            this.lvUserCategory.Location = new System.Drawing.Point(6, 19);
            this.lvUserCategory.Name = "lvUserCategory";
            this.lvUserCategory.Size = new System.Drawing.Size(230, 163);
            this.lvUserCategory.TabIndex = 40;
            this.lvUserCategory.UseCompatibleStateImageBehavior = false;
            this.lvUserCategory.View = System.Windows.Forms.View.Details;
            // 
            // cbPrivilegeLvl
            // 
            this.cbPrivilegeLvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrivilegeLvl.Location = new System.Drawing.Point(152, 184);
            this.cbPrivilegeLvl.Name = "cbPrivilegeLvl";
            this.cbPrivilegeLvl.Size = new System.Drawing.Size(192, 21);
            this.cbPrivilegeLvl.TabIndex = 11;
            this.cbPrivilegeLvl.Visible = false;
            // 
            // tbNumOfTries
            // 
            this.tbNumOfTries.Location = new System.Drawing.Point(152, 152);
            this.tbNumOfTries.Name = "tbNumOfTries";
            this.tbNumOfTries.Size = new System.Drawing.Size(192, 20);
            this.tbNumOfTries.TabIndex = 9;
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.Location = new System.Drawing.Point(152, 120);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(192, 21);
            this.cbStatus.TabIndex = 7;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(152, 88);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(192, 20);
            this.tbDesc.TabIndex = 5;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(152, 56);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(192, 20);
            this.tbName.TabIndex = 3;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // cbUserID
            // 
            this.cbUserID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserID.Location = new System.Drawing.Point(152, 24);
            this.cbUserID.Name = "cbUserID";
            this.cbUserID.Size = new System.Drawing.Size(192, 21);
            this.cbUserID.TabIndex = 1;
            this.cbUserID.SelectedIndexChanged += new System.EventHandler(this.cbUserID_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(517, 221);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblPrivilegeLvl
            // 
            this.lblPrivilegeLvl.Location = new System.Drawing.Point(16, 184);
            this.lblPrivilegeLvl.Name = "lblPrivilegeLvl";
            this.lblPrivilegeLvl.Size = new System.Drawing.Size(128, 23);
            this.lblPrivilegeLvl.TabIndex = 10;
            this.lblPrivilegeLvl.Text = "Privilege level:";
            this.lblPrivilegeLvl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPrivilegeLvl.Visible = false;
            // 
            // lblNumOfTries
            // 
            this.lblNumOfTries.Location = new System.Drawing.Point(16, 152);
            this.lblNumOfTries.Name = "lblNumOfTries";
            this.lblNumOfTries.Size = new System.Drawing.Size(128, 24);
            this.lblNumOfTries.TabIndex = 8;
            this.lblNumOfTries.Text = "Number of login tries:";
            this.lblNumOfTries.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(16, 120);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(128, 23);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(16, 88);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(128, 23);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(16, 56);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(128, 23);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUserID
            // 
            this.lblUserID.Location = new System.Drawing.Point(16, 24);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(128, 23);
            this.lblUserID.TabIndex = 0;
            this.lblUserID.Text = "UserID:";
            this.lblUserID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvUsers
            // 
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.GridLines = true;
            this.lvUsers.HideSelection = false;
            this.lvUsers.Location = new System.Drawing.Point(16, 282);
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(753, 224);
            this.lvUsers.TabIndex = 13;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            this.lvUsers.SelectedIndexChanged += new System.EventHandler(this.lvUsers_SelectedIndexChanged);
            this.lvUsers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUsers_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 596);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 23);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(104, 596);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(64, 23);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(187, 596);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(64, 23);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUserRole
            // 
            this.btnUserRole.Location = new System.Drawing.Point(341, 595);
            this.btnUserRole.Name = "btnUserRole";
            this.btnUserRole.Size = new System.Drawing.Size(112, 23);
            this.btnUserRole.TabIndex = 17;
            this.btnUserRole.Text = "User <-> Role";
            this.btnUserRole.Click += new System.EventHandler(this.btnUserRole_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(694, 596);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnLogView
            // 
            this.btnLogView.Location = new System.Drawing.Point(459, 594);
            this.btnLogView.Name = "btnLogView";
            this.btnLogView.Size = new System.Drawing.Size(136, 24);
            this.btnLogView.TabIndex = 18;
            this.btnLogView.Text = "Login View";
            this.btnLogView.Click += new System.EventHandler(this.btnLogView_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(632, 16);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 100);
            this.gbFilter.TabIndex = 25;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // lvCategoryDetails
            // 
            this.lvCategoryDetails.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvCategoryDetails.FullRowSelect = true;
            this.lvCategoryDetails.GridLines = true;
            this.lvCategoryDetails.HideSelection = false;
            this.lvCategoryDetails.Location = new System.Drawing.Point(17, 512);
            this.lvCategoryDetails.Name = "lvCategoryDetails";
            this.lvCategoryDetails.Size = new System.Drawing.Size(752, 81);
            this.lvCategoryDetails.TabIndex = 41;
            this.lvCategoryDetails.UseCompatibleStateImageBehavior = false;
            this.lvCategoryDetails.View = System.Windows.Forms.View.Details;
            // 
            // ApplUsers
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(778, 631);
            this.ControlBox = false;
            this.Controls.Add(this.lvCategoryDetails);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnLogView);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUserRole);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvUsers);
            this.Controls.Add(this.gbOperators);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ApplUsers";
            this.ShowInTaskbar = false;
            this.Text = "Operators";
            this.Load += new System.EventHandler(this.ApplUsers_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ApplUsers_KeyUp);
            this.gbOperators.ResumeLayout(false);
            this.gbOperators.PerformLayout();
            this.gbUserCategory.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer(ListView lv)
            {
                _listView = lv;
            }
            public ListView ListView
            {
                get { return _listView; }
            }

            private int _sortColumn = 0;

            public int SortColumn
            {
                get { return _sortColumn; }
                set { _sortColumn = value; }
            }

            public int Compare(object a, object b)
            {
                ListViewItem item1 = (ListViewItem)a;
                ListViewItem item2 = (ListViewItem)b;

                if (ListView.Sorting == SortOrder.Descending)
                {
                    ListViewItem temp = item1;
                    item1 = item2;
                    item2 = temp;
                }
                // Handle non Detail Cases
                return CompareItems(item1, item2);
            }

            public int CompareItems(ListViewItem item1, ListViewItem item2)
            {
                // Subitem instances
                ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
                ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

                // Return value based on sort column	
                switch (SortColumn)
                {
                    case ApplUsers.NumOfTriesIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()), Int32.Parse(sub2.Text.Trim()));
                        }
                    case ApplUsers.UserIDIndex:
                    case ApplUsers.NameIndex:
                    case ApplUsers.DescriptionIndex:
                    case ApplUsers.StatusIndex:
                    case ApplUsers.LanguageIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }
        }

        #endregion

        /// <summary>
        /// Set proper language and initialize List View
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("userForm", culture);

                // group box text
                gbOperators.Text = rm.GetString("gbUser", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                this.gbUserCategory.Text = rm.GetString("gbUserCategory", culture);

                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnUserRole.Text = rm.GetString("btnUserRole", culture);
                btnLogView.Text = rm.GetString("btnLogView", culture);

                // label's text
                lblUserID.Text = rm.GetString("lblUserID", culture);
                lblName.Text = rm.GetString("lblName", culture);
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);
                lblNumOfTries.Text = rm.GetString("lblNumOfTries", culture);
                lblPrivilegeLvl.Text = rm.GetString("lblPrivilegeLvl", culture);

                // list view
                lvUsers.BeginUpdate();
                lvUsers.Columns.Add(rm.GetString("lblUserID", culture), (lvUsers.Width - 6) / 6, HorizontalAlignment.Left);
                lvUsers.Columns.Add(rm.GetString("lblName", culture), (lvUsers.Width - 6) / 6, HorizontalAlignment.Left);
                lvUsers.Columns.Add(rm.GetString("lblDescription", culture), (lvUsers.Width - 6) / 6, HorizontalAlignment.Left);
                lvUsers.Columns.Add(rm.GetString("lblStatus", culture), (lvUsers.Width - 6) / 6, HorizontalAlignment.Left);
                lvUsers.Columns.Add(rm.GetString("lblNumOfTries", culture), (lvUsers.Width - 6) / 6, HorizontalAlignment.Left);
                lvUsers.Columns.Add(rm.GetString("lblLanguage", culture), (lvUsers.Width - 6) / 6, HorizontalAlignment.Left);
                lvUsers.EndUpdate();

                lvUserCategory.BeginUpdate();
                lvUserCategory.Columns.Add(rm.GetString("hdrName", culture), lvUserCategory.Width / 2 - 5, HorizontalAlignment.Left);
                lvUserCategory.Columns.Add(rm.GetString("hdrDescription", culture), lvUserCategory.Width / 2 - 5, HorizontalAlignment.Left);
                lvUserCategory.EndUpdate();

                lvCategoryDetails.BeginUpdate();
                lvCategoryDetails.Columns.Add(rm.GetString("hdrCategory", culture), lvCategoryDetails.Width / 3 - 5, HorizontalAlignment.Left);
                lvCategoryDetails.Columns.Add(rm.GetString("hdrDescription", culture), lvCategoryDetails.Width / 3 - 5, HorizontalAlignment.Left);
                lvCategoryDetails.Columns.Add(rm.GetString("hdrDefaultCategory", culture), lvCategoryDetails.Width / 3 - 5, HorizontalAlignment.Left);
                lvCategoryDetails.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void populateUserIDCombo()
        {
            try
            {             
                List<ApplUserTO> userArray = new ApplUser().Search();
                userArray.Insert(0, new ApplUserTO(rm.GetString("all", culture), "", "", "", -1, "", -1, ""));

                cbUserID.DataSource = userArray;
                cbUserID.DisplayMember = "UserID";
                cbUserID.ValueMember = "UserID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.populateUserIDCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateStatusCombo()
        {
            try
            {
                ArrayList statusArray = new ArrayList();
                //string[] statuses = ConfigurationManager.AppSettings["UserStatus"].Split(',');

                statusArray.Add(rm.GetString("all", culture));

                /*foreach (string status in statuses)
                {
                    statusArray.Add(status.Trim());
                }*/
                statusArray.Add(Constants.statusActive);
                statusArray.Add(Constants.statusDisabled);

                if (Common.Misc.isADMINRole(logInUser.UserID))
                {
                    statusArray.Add(Constants.statusRetired);
                }

                cbStatus.DataSource = statusArray;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.populateStatusCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateUserCategoryListView()
        {
            try
            {
                lvUserCategory.BeginUpdate();
                List<ApplUserCategoryTO> categories = new ApplUserCategory().Search(null);
                foreach (ApplUserCategoryTO category in categories)
                {
                    if (!categoryDict.ContainsKey(category.CategoryID))
                        categoryDict.Add(category.CategoryID, category);
                    ListViewItem item = new ListViewItem();
                    item.Text = category.Name;
                    item.SubItems.Add(category.Desc);
                    item.Tag = category;
                    lvUserCategory.Items.Add(item);
                }
                lvUserCategory.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.populateUserCategoryListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with Users found
        /// </summary>
        /// <param name="usersList"></param>
        public void populateListView(List<ApplUserTO> usersList)
        {
            try
            {
                lvUsers.BeginUpdate();
                lvUsers.Items.Clear();

                if (usersList.Count > 0)
                {
                    foreach (ApplUserTO user in usersList)
                    {
                        if (selectedApplUserCategories.Count <= 0 || categoryXUserDict.ContainsKey(user.UserID))
                        {
                            bool found = false;
                            foreach (int category in selectedApplUserCategories)
                            {
                                if (categoryXUserDict[user.UserID].Contains(category))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found || selectedApplUserCategories.Count <= 0)
                            {
                                ListViewItem item = new ListViewItem();
                                item.Text = user.UserID.Trim();
                                item.SubItems.Add(user.Name.Trim());
                                item.SubItems.Add(user.Description.Trim());
                                item.SubItems.Add(user.Status.Trim());
                                item.SubItems.Add(user.NumOfTries.ToString().Trim());
                                item.SubItems.Add(user.LangCode.Trim());
                                item.Tag = user;

                                lvUsers.Items.Add(item);
                            }
                        }
                    }
                }

                lvUsers.EndUpdate();
                lvUsers.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        //private string parse(string forParsing)
        //{
        //    string parsedString = forParsing.Trim();
        //    if (parsedString.StartsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(1);
        //        parsedString = "%" + parsedString;
        //    }

        //    if (parsedString.EndsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(0, parsedString.Length - 1);
        //        parsedString = parsedString + "%";
        //    }

        //    return parsedString;
        //}

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                selectedApplUserCategories = new List<int>();
                foreach (ListViewItem item in lvUserCategory.SelectedItems)
                {
                    int categoryID = ((ApplUserCategoryTO)item.Tag).CategoryID;
                    if (!selectedApplUserCategories.Contains(categoryID))
                        selectedApplUserCategories.Add(categoryID);
                }
                categoryXUserDict = new ApplUserXApplUserCategory().SearchUserCategoryDictionary();

                string userID = "";
                string status = "";
                if (cbUserID.SelectedIndex > 0)
                {
                    userID = cbUserID.SelectedValue.ToString().Trim();
                }

                if (cbStatus.SelectedIndex > 0)
                {
                    status = cbStatus.SelectedItem.ToString().Trim();
                }              

                List<ApplUserTO> userList = new List<ApplUserTO>();

                ApplUser user = new ApplUser();
                user.UserTO.UserID = userID;
                user.UserTO.Name = tbName.Text.Trim();
                user.UserTO.Description = tbDesc.Text.Trim();
                user.UserTO.Status = status;
                int numOfEntries = -1;
                if (int.TryParse(tbNumOfTries.Text.Trim(), out numOfEntries))
                    user.UserTO.NumOfTries = numOfEntries;

                if (status.Trim().Equals(""))
                {
                    if (Common.Misc.isADMINRole(logInUser.UserID))
                    {
                        userList = user.Search();
                    }
                    else
                    {
                        List<string> statuses = new List<string>();
                        statuses.Add(Constants.statusActive);
                        statuses.Add(Constants.statusDisabled);
                        userList = user.SearchWithStatus(statuses);
                    }
                }
                else
                {
                    userList = user.Search();
                }
                if (userList.Count > 0)
                {
                    populateListView(userList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noUsersFound", culture));
                }

                currentUser = new ApplUserTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ApplUsers_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvUsers);
                lvUsers.ListViewItemSorter = _comp;
                lvUsers.Sorting = SortOrder.Ascending;

                List<ApplUserTO> userList = new List<ApplUserTO>();
                if (Common.Misc.isADMINRole(logInUser.UserID))
                {
                    userList = new ApplUser().Search();
                }
                else
                {
                    List<string> statuses = new List<string>();
                    statuses.Add(Constants.statusActive);
                    statuses.Add(Constants.statusDisabled);
                    userList = new ApplUser().SearchWithStatus(statuses);
                }
                populateUserCategoryListView();
                btnSearch_Click(this,new EventArgs());

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " ApplUsers.ApplUsers_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvUsers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;

                //if (lvUsers.SelectedItems.Count == 1 && populateFilter)
                //{
                //    textChanged = false;
                //    currentUser = (ApplUserTO)lvUsers.SelectedItems[0].Tag;

                //    cbUserID.SelectedValue = currentUser.UserID;
                //    tbName.Text = currentUser.Name.ToString();
                //    tbDesc.Text = currentUser.Description.ToString();
                //    cbStatus.SelectedIndex = cbStatus.FindStringExact(currentUser.Status);
                //    tbNumOfTries.Text = currentUser.NumOfTries.ToString().Trim();
                //    textChanged = true;
                //}
                populateApplUsersCategoryDetails();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.lvUsers_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
       
        private void populateApplUsersCategoryDetails()
        {
            try
            {
                lvCategoryDetails.BeginUpdate();
                lvCategoryDetails.Items.Clear();

                if (lvUsers.SelectedItems.Count == 1)
                {
                    ApplUserTO selectedUser = (ApplUserTO)lvUsers.SelectedItems[0].Tag;
                    if (categoryXUserDict.ContainsKey(selectedUser.UserID))
                    {
                        ApplUserXApplUserCategory category = new ApplUserXApplUserCategory();
                        category.UserXCategoryTO.UserID = selectedUser.UserID;
                        List<ApplUserXApplUserCategoryTO> list = category.Search();
                        foreach (ApplUserXApplUserCategoryTO categoryTO in list)
                        {
                            ListViewItem item = new ListViewItem();
                            if (categoryDict.ContainsKey(categoryTO.CategoryID))
                            {
                                item.Text = categoryDict[categoryTO.CategoryID].Name;
                                item.SubItems.Add(categoryDict[categoryTO.CategoryID].Desc);
                            }
                            else
                            {
                                item.Text = "N/A";
                                item.SubItems.Add("N/A");
                            }
                            if (categoryTO.DefaultCategory == 0)
                                item.SubItems.Add(rm.GetString("no", culture));
                            else
                                item.SubItems.Add(rm.GetString("yes", culture));
                            item.Tag = categoryTO;
                            lvCategoryDetails.Items.Add(item);
                        }
                    }
                }

                lvCategoryDetails.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TMTypes.populateUserCategoryListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvUsers_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvUsers.Sorting;
                lvUsers.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvUsers.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvUsers.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvUsers.Sorting = SortOrder.Ascending;
                }

                lvUsers.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.lvUsers_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvUsers.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelUserDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteUsers", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool isDeleted = true;
                        int selected = lvUsers.SelectedItems.Count;

                        foreach (ListViewItem item in lvUsers.SelectedItems)
                        {
                            if (item.Text.Trim().Equals("SYS"))
                            {
                                MessageBox.Show(rm.GetString("defaultUser", culture));
                                selected--;
                            }
                            else
                            {
                                // Find if exists WU that this User is granted
                                /*ArrayList wuArray = new ApplUsersXWU().Search(item.Text.Trim(), "", "");

                                if (wuArray.Count > 0)
                                {
                                    MessageBox.Show(item.Text + ": " + rm.GetString("userHasWU", culture));
                                    selected--;
                                }
                                else
                                {
                                    // Find if exists Role that is granted to this User
                                    ArrayList roleArray = new ApplUsersXRole().Search(item.Text.Trim(), "");

                                    if (roleArray.Count > 0)
                                    {
                                        MessageBox.Show(item.Text + ": " + rm.GetString("userHasRoles", culture));
                                        selected--;
                                    }
                                    else
                                    {
                                        //isDeleted = currentUser.Delete(item.Text.Trim()) && isDeleted;
                                        currentUser.ReceiveTransferObject(currentUser.Find(item.Text.Trim()));
                                        isDeleted = currentUser.Update(currentUser.UserID, currentUser.Password,
                                            currentUser.Name, currentUser.Description, currentUser.PrivilegeLvl,
                                            Constants.statusRetired, currentUser.NumOfTries, currentUser.LangCode) && isDeleted;
                                    }
                                }*/

                                currentUser = (ApplUserTO)lvUsers.SelectedItems[0].Tag;                                
                                currentUser.Status = Constants.statusRetired;
                                ApplUser appUser = new ApplUser();
                                appUser.UserTO = currentUser;
                                isDeleted = appUser.Update() && isDeleted;
                            }
                        }

                        if ((selected > 0) && isDeleted)
                        {
                            MessageBox.Show(rm.GetString("usersDel", culture));
                        }
                        else if (!isDeleted)
                        {
                            MessageBox.Show(rm.GetString("noUserDel", culture));
                        }

                        List<ApplUserTO> userList = new List<ApplUserTO>();
                        if (Common.Misc.isADMINRole(logInUser.UserID))
                        {
                            userList = new ApplUser().Search();
                        }
                        else
                        {
                            List<string> statuses = new List<string>();
                            statuses.Add(Constants.statusActive);
                            statuses.Add(Constants.statusDisabled);
                            userList = new ApplUser().SearchWithStatus(statuses);
                        } 
                        populateListView(userList);
                        cbUserID.SelectedIndex = 0;
                        tbName.Text = "";
                        tbDesc.Text = "";
                        cbStatus.SelectedIndex = 0;
                        tbNumOfTries.Text = "";
                        currentUser = new ApplUserTO();
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Open Add Form
                ApplUsersAdd addForm = new ApplUsersAdd();
                addForm.ShowDialog(this);

                List<ApplUserTO> userList = new List<ApplUserTO>();
                if (Common.Misc.isADMINRole(logInUser.UserID))
                {
                    userList = new ApplUser().Search();
                }
                else
                {
                    List<string> statuses = new List<string>();
                    statuses.Add(Constants.statusActive);
                    statuses.Add(Constants.statusDisabled);
                    userList = new ApplUser().SearchWithStatus(statuses);
                }
                populateListView(userList);
                populateUserIDCombo(); //to add new user
                cbUserID.SelectedIndex = 0;
                tbName.Text = "";
                tbDesc.Text = "";
                cbStatus.SelectedIndex = 0;
                tbNumOfTries.Text = "";
                currentUser = new ApplUserTO();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.lvUsers.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneUser", culture));
                }
                else
                {
                    //currentUser.ReceiveTransferObject(currentUser.Find(this.lvUsers.SelectedItems[0].Text));
                    currentUser = (ApplUserTO)lvUsers.SelectedItems[0].Tag;
                    List<int> categories = new List<int>();
                    if (categoryXUserDict.ContainsKey(currentUser.UserID))
                        categories = categoryXUserDict[currentUser.UserID];
                    // Open Update Form
                    ApplUsersAdd addForm = new ApplUsersAdd(currentUser,categories);
                    addForm.ShowDialog(this);

                    List<ApplUserTO> userList = new List<ApplUserTO>();
                    if (Common.Misc.isADMINRole(logInUser.UserID))
                    {
                        userList = new ApplUser().Search();
                    }
                    else
                    {
                        List<string> statuses = new List<string>();
                        statuses.Add(Constants.statusActive);
                        statuses.Add(Constants.statusDisabled);
                        userList = new ApplUser().SearchWithStatus(statuses);
                    }
                    populateListView(userList);
                    cbUserID.SelectedIndex = 0;
                    tbName.Text = "";
                    tbDesc.Text = "";
                    cbStatus.SelectedIndex = 0;
                    tbNumOfTries.Text = "";
                    currentUser = new ApplUserTO();
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUserRole_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ApplUsersXRoles userXRoleForm = new ApplUsersXRoles();
                userXRoleForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnUserRole_Click(): " + ex.Message + "\n");
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
                int rolesPermission;                
                bool rolesAdd = false;
                bool rolesUpdate = false;
                bool rolesDelete = false;

                string usersRolesMenuItem = rm.GetString("menuConfiguration", culture) + "_" + rm.GetString("menuUserRoles", culture) + "_" + rm.GetString("menuUsersRoles", culture);
                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
                    addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
                    updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
                    deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);

                    rolesPermission = (((int[])menuItemsPermissions[usersRolesMenuItem])[role.ApplRoleID]);
                    rolesAdd = rolesAdd || (((rolesPermission / 4) % 2) == 0 ? false : true);
                    rolesUpdate = rolesUpdate || (((rolesPermission / 2) % 2) == 0 ? false : true);
                    rolesDelete = rolesDelete || ((rolesPermission % 2) == 0 ? false : true);
                }

                btnSearch.Enabled = readPermission;
                btnAdd.Enabled = addPermission;
                btnUpdate.Enabled = updatePermission;
                btnDelete.Enabled = deletePermission;
                btnUserRole.Visible = rolesAdd && rolesUpdate && rolesDelete;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogView_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ApplUserTO userTO = new ApplUserTO();
                if (lvUsers.SelectedItems.Count > 0)
                {
                    userTO = (ApplUserTO)lvUsers.SelectedItems[0].Tag;
                }
                ApplUsersLog userLogForm = new ApplUsersLog(userTO);
                userLogForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnLogView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbUserID_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void gbOperators_Enter(object sender, System.EventArgs e)
        {

        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //if (textChanged)
                //{
                    // do not invoke list view selected items changed event
                    //populateFilter = false;

                    lvUsers.SelectedItems.Clear();
                    lvUsers.Select();
                    lvUsers.Invalidate();

                    foreach (ListViewItem item in lvUsers.Items)
                    {
                        if (item.SubItems[1].Text.Trim().ToUpper().StartsWith(tbName.Text.Trim().ToUpper()))
                        {
                            item.Selected = true;
                            lvUsers.Select();
                            lvUsers.EnsureVisible(lvUsers.Items.IndexOf(lvUsers.SelectedItems[0]));

                            break;
                        }
                    }

                    tbName.Focus();
                    //populateFilter = true;
                    tbName.SelectionLength = 0;
                //}
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.tbFirstName_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ApplUsers_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ApplUsers.ApplUsers_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}

