using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for ReaderActivity.
	/// </summary>
	public class ReaderActivity : Panel
	{
		private System.Windows.Forms.Button lanIndic;
		private System.Windows.Forms.Button btnData;
		private System.Windows.Forms.CheckBox cbReader;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel1;
		DebugLog log;
		private System.Windows.Forms.Label lblOccupation;
		private System.Windows.Forms.Label lblInProgress;
		private ReaderTO _reader;

		public ReaderTO Reader
		{
			get { return _reader; }
				 
		}

        private delegate void InvokeDelegate(string s);

        public ReaderActivity()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public ReaderActivity(ReaderTO reader, string lblBtn1Val, string lblBtn2Val)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
			
			this._reader = reader;
			this.cbReader.Text = reader.Description;
			this.lanIndic.BackColor = Color.DarkSeaGreen;
			this.btnData.BackColor = Color.DarkSeaGreen;
			this.panel1.Controls.Add(cbReader);
			this.panel1.Controls.Add(btnData);
			this.panel1.Controls.Add(lanIndic);
			this.panel1.Controls.Add(lblOccupation);
			this.panel1.Controls.Add(lblInProgress);

			this.Controls.Add(this.panel1);
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
            this.cbReader = new System.Windows.Forms.CheckBox();
            this.lanIndic = new System.Windows.Forms.Button();
            this.btnData = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblOccupation = new System.Windows.Forms.Label();
            this.lblInProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbReader
            // 
            this.cbReader.Checked = true;
            this.cbReader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReader.Location = new System.Drawing.Point(7, 10);
            this.cbReader.Name = "cbReader";
            this.cbReader.Size = new System.Drawing.Size(150, 34);
            this.cbReader.TabIndex = 0;
            this.cbReader.Text = "readerDesc";
            // 
            // lanIndic
            // 
            this.lanIndic.BackColor = System.Drawing.SystemColors.Control;
            this.lanIndic.Enabled = false;
            this.lanIndic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lanIndic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lanIndic.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lanIndic.Location = new System.Drawing.Point(175, 13);
            this.lanIndic.Name = "lanIndic";
            this.lanIndic.Size = new System.Drawing.Size(20, 20);
            this.lanIndic.TabIndex = 1;
            this.lanIndic.Text = "C";
            this.lanIndic.UseVisualStyleBackColor = false;
            // 
            // btnData
            // 
            this.btnData.Enabled = false;
            this.btnData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnData.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnData.Location = new System.Drawing.Point(216, 13);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(20, 20);
            this.btnData.TabIndex = 2;
            this.btnData.Text = "D";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 45);
            this.panel1.TabIndex = 0;
            // 
            // lblOccupation
            // 
            this.lblOccupation.Location = new System.Drawing.Point(254, 18);
            this.lblOccupation.Name = "lblOccupation";
            this.lblOccupation.Size = new System.Drawing.Size(40, 34);
            this.lblOccupation.TabIndex = 0;
            this.lblOccupation.Text = "100%";
            // 
            // lblInProgress
            // 
            this.lblInProgress.Location = new System.Drawing.Point(292, 18);
            this.lblInProgress.Name = "lblInProgress";
            this.lblInProgress.Size = new System.Drawing.Size(200, 23);
            this.lblInProgress.TabIndex = 0;
            this.lblInProgress.Text = "In Progress";
            // 
            // ReaderActivity
            // 
            this.Size = new System.Drawing.Size(400, 45);
            this.ResumeLayout(false);

		}
		#endregion

		public void Desable()
		{
			cbReader.Enabled = false;
		}

		public void Enable()
		{
			cbReader.Enabled = true;
		}

		public bool IsSelected()
		{
			return cbReader.Checked;
		}

		public void SetDataExists(bool exists)
		{
			if (exists)
			{
				btnData.BackColor = Color.DarkSeaGreen;
			}
			else
			{
				btnData.BackColor = Color.Tomato;
			}
		}

		public void SetLanExists(bool exists)
		{
			if (exists)
			{
				lanIndic.BackColor = Color.DarkSeaGreen;
			}
			else
			{
				lanIndic.BackColor = Color.Tomato;
				btnData.BackColor = Color.Empty;
			}
		}

        public void SetPingAlert(bool exists)
        {
            if (exists)
            {
                lanIndic.BackColor = Color.DarkSeaGreen;
            }
            else
            {
                lanIndic.BackColor = Color.Tomato;
            }
        }

		public void SetInProgress(string text)
		{
            this.lblInProgress.BeginInvoke(new InvokeDelegate(SetLblInProgressText), text);
		}

		public void SetOccupation(string text)
		{
            this.lblOccupation.BeginInvoke(new InvokeDelegate(SetLblOccupationText), text);
		}

		public void Init()
		{
			btnData.BackColor = Color.DarkSeaGreen;
			lanIndic.BackColor = Color.DarkSeaGreen;
			lblInProgress.Text = "";
			lblOccupation.Text = "";
		}

        private void SetLblInProgressText(string text)
        {
            this.lblInProgress.Text = text;
        }

        private void SetLblOccupationText(string text)
        {
            this.lblOccupation.Text = text;
        }
	}
}
