namespace UI
{
    partial class OrganizationalUnits
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrganizationalUnits));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpTreeView = new System.Windows.Forms.TabPage();
            this.lblRefreshToolTip = new System.Windows.Forms.Label();
            this.dataTreeView1 = new UI.DataTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnEmplXOUTree = new System.Windows.Forms.Button();
            this.btnCloseTree = new System.Windows.Forms.Button();
            this.btnDeleteTree = new System.Windows.Forms.Button();
            this.btnUpdateTree = new System.Windows.Forms.Button();
            this.btnAddTree = new System.Windows.Forms.Button();
            this.tpListView = new System.Windows.Forms.TabPage();
            this.gbOrganizationalUnits = new System.Windows.Forms.GroupBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblParentOUID = new System.Windows.Forms.Label();
            this.cbParentUnitID = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnEmplXOU = new System.Windows.Forms.Button();
            this.lvWorkingUnits = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tpTreeView.SuspendLayout();
            this.tpListView.SuspendLayout();
            this.gbOrganizationalUnits.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpTreeView);
            this.tabControl1.Controls.Add(this.tpListView);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(647, 474);
            this.tabControl1.TabIndex = 15;
            // 
            // tpTreeView
            // 
            this.tpTreeView.Controls.Add(this.lblRefreshToolTip);
            this.tpTreeView.Controls.Add(this.dataTreeView1);
            this.tpTreeView.Controls.Add(this.btnEmplXOUTree);
            this.tpTreeView.Controls.Add(this.btnCloseTree);
            this.tpTreeView.Controls.Add(this.btnDeleteTree);
            this.tpTreeView.Controls.Add(this.btnUpdateTree);
            this.tpTreeView.Controls.Add(this.btnAddTree);
            this.tpTreeView.Location = new System.Drawing.Point(4, 22);
            this.tpTreeView.Name = "tpTreeView";
            this.tpTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tpTreeView.Size = new System.Drawing.Size(639, 448);
            this.tpTreeView.TabIndex = 1;
            this.tpTreeView.Text = "Tree";
            this.tpTreeView.UseVisualStyleBackColor = true;
            // 
            // lblRefreshToolTip
            // 
            this.lblRefreshToolTip.Location = new System.Drawing.Point(7, 392);
            this.lblRefreshToolTip.Name = "lblRefreshToolTip";
            this.lblRefreshToolTip.Size = new System.Drawing.Size(626, 23);
            this.lblRefreshToolTip.TabIndex = 25;
            // 
            // dataTreeView1
            // 
            this.dataTreeView1.DataMember = null;
            this.dataTreeView1.FullRowSelect = true;
            this.dataTreeView1.HideSelection = false;
            this.dataTreeView1.HotTracking = true;
            this.dataTreeView1.ImageIndex = 0;
            this.dataTreeView1.ImageList = this.imageList1;
            this.dataTreeView1.Location = new System.Drawing.Point(6, 12);
            this.dataTreeView1.Name = "dataTreeView1";
            this.dataTreeView1.SelectedImageIndex = 0;
            this.dataTreeView1.SelectedNodes = ((System.Collections.ArrayList)(resources.GetObject("dataTreeView1.SelectedNodes")));
            this.dataTreeView1.ShowNodeToolTips = true;
            this.dataTreeView1.Size = new System.Drawing.Size(627, 377);
            this.dataTreeView1.TabIndex = 1;
            this.dataTreeView1.ToolTipTextColumn = "";
            this.dataTreeView1.ValueColumn = null;
            this.dataTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.dataTreeView1_AfterSelect);
            this.dataTreeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataTreeView1_MouseMove);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Network.ico");
            this.imageList1.Images.SetKeyName(1, "Msn-Buddy-mobile.ico");
            this.imageList1.Images.SetKeyName(2, "Msn-Buddy.ico");
            this.imageList1.Images.SetKeyName(3, "Msn-Messenger.ico");
            // 
            // btnEmplXOUTree
            // 
            this.btnEmplXOUTree.Location = new System.Drawing.Point(304, 419);
            this.btnEmplXOUTree.Name = "btnEmplXOUTree";
            this.btnEmplXOUTree.Size = new System.Drawing.Size(213, 23);
            this.btnEmplXOUTree.TabIndex = 24;
            this.btnEmplXOUTree.Text = "Employees <-> Organizational Units";
            this.btnEmplXOUTree.UseVisualStyleBackColor = true;
            this.btnEmplXOUTree.Click += new System.EventHandler(this.btnEmplXOU_Click);
            // 
            // btnCloseTree
            // 
            this.btnCloseTree.Location = new System.Drawing.Point(537, 419);
            this.btnCloseTree.Name = "btnCloseTree";
            this.btnCloseTree.Size = new System.Drawing.Size(96, 23);
            this.btnCloseTree.TabIndex = 23;
            this.btnCloseTree.Text = "Close";
            this.btnCloseTree.UseVisualStyleBackColor = true;
            this.btnCloseTree.Click += new System.EventHandler(this.btnCloseTree_Click);
            // 
            // btnDeleteTree
            // 
            this.btnDeleteTree.Location = new System.Drawing.Point(168, 419);
            this.btnDeleteTree.Name = "btnDeleteTree";
            this.btnDeleteTree.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteTree.TabIndex = 22;
            this.btnDeleteTree.Text = "Delete";
            this.btnDeleteTree.UseVisualStyleBackColor = true;
            this.btnDeleteTree.Click += new System.EventHandler(this.btnDeleteTree_Click);
            // 
            // btnUpdateTree
            // 
            this.btnUpdateTree.Location = new System.Drawing.Point(87, 419);
            this.btnUpdateTree.Name = "btnUpdateTree";
            this.btnUpdateTree.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateTree.TabIndex = 21;
            this.btnUpdateTree.Text = "Update";
            this.btnUpdateTree.UseVisualStyleBackColor = true;
            this.btnUpdateTree.Click += new System.EventHandler(this.btnUpdateTree_Click);
            // 
            // btnAddTree
            // 
            this.btnAddTree.Location = new System.Drawing.Point(6, 419);
            this.btnAddTree.Name = "btnAddTree";
            this.btnAddTree.Size = new System.Drawing.Size(75, 23);
            this.btnAddTree.TabIndex = 20;
            this.btnAddTree.Text = "Add";
            this.btnAddTree.UseVisualStyleBackColor = true;
            this.btnAddTree.Click += new System.EventHandler(this.btnAddTree_Click);
            // 
            // tpListView
            // 
            this.tpListView.Controls.Add(this.gbOrganizationalUnits);
            this.tpListView.Controls.Add(this.btnClose);
            this.tpListView.Controls.Add(this.btnEmplXOU);
            this.tpListView.Controls.Add(this.lvWorkingUnits);
            this.tpListView.Controls.Add(this.btnAdd);
            this.tpListView.Controls.Add(this.btnDelete);
            this.tpListView.Controls.Add(this.btnUpdate);
            this.tpListView.Location = new System.Drawing.Point(4, 22);
            this.tpListView.Name = "tpListView";
            this.tpListView.Padding = new System.Windows.Forms.Padding(3);
            this.tpListView.Size = new System.Drawing.Size(639, 448);
            this.tpListView.TabIndex = 0;
            this.tpListView.Text = "List";
            this.tpListView.UseVisualStyleBackColor = true;
            // 
            // gbOrganizationalUnits
            // 
            this.gbOrganizationalUnits.Controls.Add(this.btnWUTree);
            this.gbOrganizationalUnits.Controls.Add(this.tbName);
            this.gbOrganizationalUnits.Controls.Add(this.lblName);
            this.gbOrganizationalUnits.Controls.Add(this.lblDescription);
            this.gbOrganizationalUnits.Controls.Add(this.tbDescription);
            this.gbOrganizationalUnits.Controls.Add(this.lblParentOUID);
            this.gbOrganizationalUnits.Controls.Add(this.cbParentUnitID);
            this.gbOrganizationalUnits.Controls.Add(this.btnSearch);
            this.gbOrganizationalUnits.Location = new System.Drawing.Point(6, 6);
            this.gbOrganizationalUnits.Name = "gbOrganizationalUnits";
            this.gbOrganizationalUnits.Size = new System.Drawing.Size(482, 168);
            this.gbOrganizationalUnits.TabIndex = 0;
            this.gbOrganizationalUnits.TabStop = false;
            this.gbOrganizationalUnits.Text = "Organizational Units";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(446, 86);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 20;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(168, 24);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(272, 20);
            this.tbName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(24, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(136, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(24, 56);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(136, 23);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(168, 56);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(272, 20);
            this.tbDescription.TabIndex = 4;
            // 
            // lblParentOUID
            // 
            this.lblParentOUID.Location = new System.Drawing.Point(6, 88);
            this.lblParentOUID.Name = "lblParentOUID";
            this.lblParentOUID.Size = new System.Drawing.Size(154, 23);
            this.lblParentOUID.TabIndex = 5;
            this.lblParentOUID.Text = "Parent Organizational Unit ID:";
            this.lblParentOUID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbParentUnitID
            // 
            this.cbParentUnitID.Location = new System.Drawing.Point(168, 88);
            this.cbParentUnitID.Name = "cbParentUnitID";
            this.cbParentUnitID.Size = new System.Drawing.Size(272, 21);
            this.cbParentUnitID.TabIndex = 6;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(365, 128);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(537, 413);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEmplXOU
            // 
            this.btnEmplXOU.Location = new System.Drawing.Point(304, 413);
            this.btnEmplXOU.Name = "btnEmplXOU";
            this.btnEmplXOU.Size = new System.Drawing.Size(218, 23);
            this.btnEmplXOU.TabIndex = 12;
            this.btnEmplXOU.Text = "Employees <-> Organizational Units";
            this.btnEmplXOU.UseVisualStyleBackColor = true;
            this.btnEmplXOU.Click += new System.EventHandler(this.btnEmplXOU_Click);
            // 
            // lvWorkingUnits
            // 
            this.lvWorkingUnits.FullRowSelect = true;
            this.lvWorkingUnits.GridLines = true;
            this.lvWorkingUnits.Location = new System.Drawing.Point(6, 180);
            this.lvWorkingUnits.Name = "lvWorkingUnits";
            this.lvWorkingUnits.Size = new System.Drawing.Size(627, 216);
            this.lvWorkingUnits.TabIndex = 8;
            this.lvWorkingUnits.UseCompatibleStateImageBehavior = false;
            this.lvWorkingUnits.View = System.Windows.Forms.View.Details;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(6, 413);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(168, 413);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(87, 413);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // OrganizationalUnits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 486);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "OrganizationalUnits";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "OrganizationalUnits";
            this.Load += new System.EventHandler(this.OrganizationalUnits_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpTreeView.ResumeLayout(false);
            this.tpListView.ResumeLayout(false);
            this.gbOrganizationalUnits.ResumeLayout(false);
            this.gbOrganizationalUnits.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpTreeView;
        private System.Windows.Forms.Label lblRefreshToolTip;
        private DataTreeView dataTreeView1;
        private System.Windows.Forms.Button btnEmplXOUTree;
        private System.Windows.Forms.Button btnCloseTree;
        private System.Windows.Forms.Button btnDeleteTree;
        private System.Windows.Forms.Button btnUpdateTree;
        private System.Windows.Forms.Button btnAddTree;
        private System.Windows.Forms.TabPage tpListView;
        private System.Windows.Forms.GroupBox gbOrganizationalUnits;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblParentOUID;
        private System.Windows.Forms.ComboBox cbParentUnitID;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEmplXOU;
        private System.Windows.Forms.ListView lvWorkingUnits;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        public System.Windows.Forms.ImageList imageList1;
    }
}