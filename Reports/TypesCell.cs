using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Configuration;

using TransferObjects;
using Common;
using Util;

namespace Reports
{
	/// <summary>
	/// Summary description for TypesCell.
	/// </summary>
	public class TypesCell : Panel
	{
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.CheckBox cbHasType;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		DebugLog log;

		private string _typeID = "";
		private System.Windows.Forms.Label label;	
		private int _hasType = -1;

		public string TypeID
		{
			get {return _typeID;}
			set {_typeID = value;}
		}

		public int HasType
		{
			get {return _hasType;}
			set {_hasType = value;}
		}

		public TypesCell(int x,int y)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.Location = new Point(x, y);

			this.panel.BackColor = Color.White;
			
			this.panel.Controls.Add(cbHasType);

			this.cbHasType.Location = new Point(4, 2);

			this.Controls.Add(panel);

			this.panel.Location = new Point(1, 1);

			this.BackColor = Color.Black;

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
			this.cbHasType = new System.Windows.Forms.CheckBox();
			this.panel = new System.Windows.Forms.Panel();
			this.label = new System.Windows.Forms.Label();
			// 
			// cbHasType
			// 
			this.cbHasType.Location = new System.Drawing.Point(17, 17);
			this.cbHasType.Name = "cbHasType";
			this.cbHasType.Size = new System.Drawing.Size(100, 26);
			this.cbHasType.TabIndex = 0;
			// 
			// panel
			// 
			this.panel.Location = new System.Drawing.Point(123, 17);
			this.panel.Name = "panel";
			this.panel.TabIndex = 0;
			// 
			// label
			// 
			this.label.Location = new System.Drawing.Point(192, 17);
			this.label.Name = "label";
			this.label.TabIndex = 0;
			this.label.Text = "label";
			// 
			// TypesCell
			// 
			this.Size = new System.Drawing.Size(35, 26);

		}
		#endregion
		public void setCheckBoxes()
		{
			this.cbHasType.Checked = (this.HasType == 1 ? true : false);

		}
		public void getCheckBoxes()
		{
			this.HasType = (this.cbHasType.Checked ? 1 : 0);
		}
		
	}
}
