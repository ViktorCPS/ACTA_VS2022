using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Configuration;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for RolePermissionCell.
	/// </summary>
	public class RolePermissionCell : Panel
	{
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.CheckBox cbReadExe;
		private System.Windows.Forms.CheckBox cbAdd;
		private System.Windows.Forms.CheckBox cbUpdate;
		private System.Windows.Forms.CheckBox cbDelete;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        // Controller instance
        public NotificationController Controller;

		DebugLog log;

		private int _roleID = -1;
		private string _menuItemID = "";
        private string _position = "";
		private int _readExePerm = -1;
		private int _addPerm = -1;
		private int _updatePerm = -1;
		private int _deletePerm = -1;

		public int RoleID
		{
			get {return _roleID;}
			set 
            {
                _roleID = value;
                if (_roleID == 0)
                {
                    desableControl();
                }
                else
                {
                    enableControl();
                }
            }
		}

		public string MenuItemID
		{
			get {return _menuItemID;}
			set {_menuItemID = value;}
		}

        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }

		public int ReadExePerm
		{
			get {return _readExePerm;}
			set {_readExePerm = value;}
		}

		public int AddPerm
		{
			get {return _addPerm;}
			set {_addPerm = value;}
		}

		public int UpdatePerm
		{
			get {return _updatePerm;}
			set {_updatePerm = value;}
		}

		public int DeletePerm
		{
			get {return _deletePerm;}
			set {_deletePerm = value;}
		}

		public RolePermissionCell(int x, int y)
		{
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                Controller = NotificationController.GetInstance();

                this.Location = new Point(x, y);

                this.panel.BackColor = Color.White;

                this.panel.Controls.Add(cbReadExe);
                this.panel.Controls.Add(cbAdd);
                this.panel.Controls.Add(cbUpdate);
                this.panel.Controls.Add(cbDelete);

                this.cbReadExe.Location = new Point(4, 2);
                this.cbAdd.Location = new Point(28, 2);
                this.cbUpdate.Location = new Point(52, 2);
                this.cbDelete.Location = new Point(76, 2);

                this.Controls.Add(panel);

                this.panel.Location = new Point(1, 1);

                this.BackColor = Color.Black;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.panel = new System.Windows.Forms.Panel();
            this.cbReadExe = new System.Windows.Forms.CheckBox();
            this.cbAdd = new System.Windows.Forms.CheckBox();
            this.cbUpdate = new System.Windows.Forms.CheckBox();
            this.cbDelete = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.Location = new System.Drawing.Point(17, 17);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(98, 24);
            this.panel.TabIndex = 0;
            // 
            // cbReadExe
            // 
            this.cbReadExe.Location = new System.Drawing.Point(95, 17);
            this.cbReadExe.Name = "cbReadExe";
            this.cbReadExe.Size = new System.Drawing.Size(20, 20);
            this.cbReadExe.TabIndex = 0;
            this.cbReadExe.CheckedChanged += new System.EventHandler(this.cbReadExe_CheckedChanged);
            // 
            // cbAdd
            // 
            this.cbAdd.Location = new System.Drawing.Point(201, 17);
            this.cbAdd.Name = "cbAdd";
            this.cbAdd.Size = new System.Drawing.Size(20, 20);
            this.cbAdd.TabIndex = 0;
            this.cbAdd.CheckedChanged += new System.EventHandler(this.cbAdd_CheckedChanged);
            // 
            // cbUpdate
            // 
            this.cbUpdate.Location = new System.Drawing.Point(283, 17);
            this.cbUpdate.Name = "cbUpdate";
            this.cbUpdate.Size = new System.Drawing.Size(20, 20);
            this.cbUpdate.TabIndex = 0;
            this.cbUpdate.CheckedChanged += new System.EventHandler(this.cbUpdate_CheckedChanged);
            // 
            // cbDelete
            // 
            this.cbDelete.Location = new System.Drawing.Point(381, 17);
            this.cbDelete.Name = "cbDelete";
            this.cbDelete.Size = new System.Drawing.Size(20, 20);
            this.cbDelete.TabIndex = 0;
            this.cbDelete.CheckedChanged += new System.EventHandler(this.cbDelete_CheckedChanged);
            // 
            // RolePermissionCell
            // 
            this.Size = new System.Drawing.Size(100, 26);
            this.ResumeLayout(false);

		}
		#endregion

		public void setPermissions(int permission)
		{		
			this.DeletePerm = permission % 2;
			permission = permission / 2;
			this.UpdatePerm = permission % 2;
			permission = permission / 2;
			this.AddPerm = permission % 2;
			permission = permission / 2;
			this.ReadExePerm = permission % 2;
			permission = permission / 2;
		}

		public int getPermission()
		{
			int permission = this.ReadExePerm * 8 + this.AddPerm * 4 + this.UpdatePerm * 2 + this.DeletePerm;

			return permission;
		}

		public void setCheckBoxes()
		{
			this.cbDelete.Checked = (this.DeletePerm == 1 ? true : false);
			this.cbUpdate.Checked = (this.UpdatePerm == 1 ? true : false);
			this.cbAdd.Checked = (this.AddPerm == 1 ? true : false);
			this.cbReadExe.Checked = (this.ReadExePerm == 1 ? true : false);
		}

		public void getCheckBoxes()
		{
			this.ReadExePerm = (this.cbReadExe.Checked ? 1 : 0);
			this.AddPerm = (this.cbAdd.Checked ? 1 : 0);
			this.UpdatePerm = (this.cbUpdate.Checked ? 1 : 0);
			this.DeletePerm = (this.cbDelete.Checked ? 1 : 0);
		}

        private void cbReadExe_CheckedChanged(object sender, EventArgs e)
        {
            if (cbReadExe.Checked)
            {
                Controller.RolePermissionCellChecked(this.RoleID, this.Position, Constants.read);
            }
        }

		private void cbAdd_CheckedChanged(object sender, System.EventArgs e)
		{
			if (cbAdd.Checked)
			{
				cbReadExe.Checked = true;
			}
		}

		private void cbUpdate_CheckedChanged(object sender, System.EventArgs e)
		{
			if (cbUpdate.Checked)
			{
				cbAdd.Checked = true;
			}
		}

		private void cbDelete_CheckedChanged(object sender, System.EventArgs e)
		{
			if (cbDelete.Checked)
			{
				cbUpdate.Checked = true;
			}
		}

        private void desableControl()
        {
            cbAdd.Enabled = false;
            cbReadExe.Enabled = false;
            cbUpdate.Enabled = false;
            cbDelete.Enabled = false;
        }

        private void enableControl()
        {
            cbAdd.Enabled = true;
            cbReadExe.Enabled = true;
            cbUpdate.Enabled = true;
            cbDelete.Enabled = true;
        }
	}
}
