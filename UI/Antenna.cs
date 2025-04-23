using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Configuration;

using Common;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for Antenna.
	/// </summary>
	public class Antenna : UserControl
	{
		private System.Windows.Forms.PictureBox image;
		private System.Windows.Forms.Label name;
		private System.Windows.Forms.Label emplID;
		private System.Windows.Forms.Label lblEmplID;

		private ReaderTO reader;
		private int antNum = -1;
		private string direction;
		private System.Windows.Forms.Panel panelEmployee;

        public bool isShowingData = false;
        public DateTime startOfShowingData = new DateTime(0);
		
		public ReaderTO AntennaReader
		{
			get { return reader; }
		}

		public int AntNum
		{
			get { return antNum; }
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Antenna(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		public Antenna(int XPos, int YPos, ReaderTO currentReader, int currentAntNum, string antDirection)
		{
			InitializeComponent();
			this.reader = currentReader;
			this.antNum = currentAntNum;
			if (this.antNum.Equals(0))
			{
				this.direction = this.reader.A0Direction;
			}
			else
			{
				this.direction = this.reader.A0Direction;
			}

			this.Location = new Point(XPos, YPos);
		}

		public Antenna(int XPos, int YPos, Size aSize, Size iSize, int imageXPos, int imageYPos)
		{
			InitializeComponent();

			this.Location = new Point(XPos,YPos);
			this.Size = aSize;
			this.image.Size = iSize;
			this.image.Location = new Point(imageXPos, imageYPos);

			this.reader = new ReaderTO();
			this.antNum = -1;

			this.Invalidate();
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

		public void SetData(ReaderTO currentReader, int currentAntNum)
		{
			this.reader = currentReader;
			this.antNum = currentAntNum;
			if (this.antNum.Equals(0))
			{
				this.direction = this.reader.A0Direction;
			}
			else
			{
				this.direction = this.reader.A0Direction;
			}
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.image = new System.Windows.Forms.PictureBox();
            this.name = new System.Windows.Forms.Label();
            this.emplID = new System.Windows.Forms.Label();
            this.lblEmplID = new System.Windows.Forms.Label();
            this.panelEmployee = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.image)).BeginInit();
            this.panelEmployee.SuspendLayout();
            this.SuspendLayout();
            // 
            // image
            // 
            this.image.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.image.Location = new System.Drawing.Point(9, 10);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(163, 142);
            this.image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.image.TabIndex = 0;
            this.image.TabStop = false;
            // 
            // name
            // 
            this.name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.Location = new System.Drawing.Point(8, 5);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(173, 33);
            this.name.TabIndex = 0;
            // 
            // emplID
            // 
            this.emplID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.emplID.Location = new System.Drawing.Point(32, 37);
            this.emplID.Name = "emplID";
            this.emplID.Size = new System.Drawing.Size(116, 16);
            this.emplID.TabIndex = 0;
            this.emplID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblEmplID
            // 
            this.lblEmplID.Location = new System.Drawing.Point(8, 39);
            this.lblEmplID.Name = "lblEmplID";
            this.lblEmplID.Size = new System.Drawing.Size(23, 11);
            this.lblEmplID.TabIndex = 0;
            this.lblEmplID.Text = "ID:";
            // 
            // panelEmployee
            // 
            this.panelEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEmployee.BackColor = System.Drawing.SystemColors.Control;
            this.panelEmployee.Controls.Add(this.lblEmplID);
            this.panelEmployee.Controls.Add(this.emplID);
            this.panelEmployee.Controls.Add(this.name);
            this.panelEmployee.Location = new System.Drawing.Point(0, 176);
            this.panelEmployee.Name = "panelEmployee";
            this.panelEmployee.Size = new System.Drawing.Size(184, 56);
            this.panelEmployee.TabIndex = 1;
            // 
            // Antenna
            // 
            this.Controls.Add(this.panelEmployee);
            this.Controls.Add(this.image);
            this.Location = new System.Drawing.Point(30, 30);
            this.Name = "Antenna";
            this.Size = new System.Drawing.Size(184, 232);
            ((System.ComponentModel.ISupportInitialize)(this.image)).EndInit();
            this.panelEmployee.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		public void ShowData(string imagePath, string emplName, string id, bool entrancePermitted)
		{
			try
			{
				if (entrancePermitted) 
				{
					this.panelEmployee.BackColor = SystemColors.Control;
				}
				else 
				{
					this.panelEmployee.BackColor = Color.Red;
				}

				this.name.Text = emplName;
				this.emplID.Text = id;
				this.image.Image = Image.FromFile(imagePath);

                isShowingData = true;
                startOfShowingData = DateTime.Now;

				this.Invalidate();
			}
			catch
			{
				this.image.Image = null;
			}
		}

        public void ClearAntennaCtrl()
        {
            try
            {
                this.panelEmployee.BackColor = SystemColors.Control;
				this.name.Text = "";
				this.emplID.Text = "";
				this.image.Image = null;

                isShowingData = false;

				this.Invalidate();
			}
			catch
			{
			}
        }
	}
}
