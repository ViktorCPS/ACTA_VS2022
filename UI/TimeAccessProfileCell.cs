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

namespace UI
{
	/// <summary>
	/// Summary description for TimeAccessProfileCell.
	/// </summary>
	public class TimeAccessProfileCell : Panel
	{
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.CheckBox cb0;
		private System.Windows.Forms.CheckBox cb1;
		private System.Windows.Forms.CheckBox cb2;
		private System.Windows.Forms.CheckBox cb3;
		private System.Windows.Forms.CheckBox cb4;
		private System.Windows.Forms.CheckBox cb5;
		private System.Windows.Forms.CheckBox cb6;
		private System.Windows.Forms.CheckBox cb7;
		private System.Windows.Forms.CheckBox cb8;
		private System.Windows.Forms.CheckBox cb9;
		private System.Windows.Forms.CheckBox cb10;
		private System.Windows.Forms.CheckBox cb11;
		private System.Windows.Forms.CheckBox cb12;
		private System.Windows.Forms.CheckBox cb13;		
		private System.Windows.Forms.CheckBox cb14;
		private System.Windows.Forms.CheckBox cb15;
		private System.Windows.Forms.CheckBox cb16;
		private System.Windows.Forms.CheckBox cb17;
		private System.Windows.Forms.CheckBox cb18;
		private System.Windows.Forms.CheckBox cb19;
		private System.Windows.Forms.CheckBox cb20;
		private System.Windows.Forms.CheckBox cb21;
		private System.Windows.Forms.CheckBox cb22;
		private System.Windows.Forms.CheckBox cb23;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		DebugLog log;

		public TimeAccessProfileCell(int x, int y)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.Location = new Point(x, y);
			this.Size = new Size(580, 24);

			this.panel.BackColor = Color.White;
			
			this.panel.Controls.Add(cb0);
			this.panel.Controls.Add(cb1);
			this.panel.Controls.Add(cb2);
			this.panel.Controls.Add(cb3);
			this.panel.Controls.Add(cb4);
			this.panel.Controls.Add(cb5);
			this.panel.Controls.Add(cb6);
			this.panel.Controls.Add(cb7);
			this.panel.Controls.Add(cb8);
			this.panel.Controls.Add(cb9);
			this.panel.Controls.Add(cb10);
			this.panel.Controls.Add(cb11);
			this.panel.Controls.Add(cb12);
			this.panel.Controls.Add(cb13);
			this.panel.Controls.Add(cb14);
			this.panel.Controls.Add(cb15);
			this.panel.Controls.Add(cb16);
			this.panel.Controls.Add(cb17);
			this.panel.Controls.Add(cb18);
			this.panel.Controls.Add(cb19);
			this.panel.Controls.Add(cb20);
			this.panel.Controls.Add(cb21);
			this.panel.Controls.Add(cb22);
			this.panel.Controls.Add(cb23);

			cb0.Location = new Point(4, 2);
			cb1.Location = new Point(28, 2);
			cb2.Location = new Point(52, 2);
			cb3.Location = new Point(76, 2);
			cb4.Location = new Point(100, 2);
			cb5.Location = new Point(124, 2);
			cb6.Location = new Point(148, 2);
			cb7.Location = new Point(172, 2);
			cb8.Location = new Point(196, 2);
			cb9.Location = new Point(220, 2);
			cb10.Location = new Point(244, 2);
			cb11.Location = new Point(268, 2);
			cb12.Location = new Point(292, 2);
			cb13.Location = new Point(316, 2);
			cb14.Location = new Point(340, 2);
			cb15.Location = new Point(364, 2);
			cb16.Location = new Point(388, 2);
			cb17.Location = new Point(412, 2);
			cb18.Location = new Point(436, 2);
			cb19.Location = new Point(460, 2);
			cb20.Location = new Point(484, 2);
			cb21.Location = new Point(508, 2);
			cb22.Location = new Point(532, 2);
			cb23.Location = new Point(556, 2);

			this.Controls.Add(panel);

			this.panel.Location = new Point(2, 2);

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
			this.cb2 = new System.Windows.Forms.CheckBox();
			this.cb3 = new System.Windows.Forms.CheckBox();
			this.cb0 = new System.Windows.Forms.CheckBox();
			this.panel = new System.Windows.Forms.Panel();
			this.cb1 = new System.Windows.Forms.CheckBox();
			this.cb4 = new System.Windows.Forms.CheckBox();
			this.cb6 = new System.Windows.Forms.CheckBox();
			this.cb5 = new System.Windows.Forms.CheckBox();
			this.cb9 = new System.Windows.Forms.CheckBox();
			this.cb10 = new System.Windows.Forms.CheckBox();
			this.cb11 = new System.Windows.Forms.CheckBox();
			this.cb12 = new System.Windows.Forms.CheckBox();
			this.cb13 = new System.Windows.Forms.CheckBox();
			this.cb14 = new System.Windows.Forms.CheckBox();
			this.cb15 = new System.Windows.Forms.CheckBox();
			this.cb16 = new System.Windows.Forms.CheckBox();
			this.cb17 = new System.Windows.Forms.CheckBox();
			this.cb18 = new System.Windows.Forms.CheckBox();
			this.cb19 = new System.Windows.Forms.CheckBox();
			this.cb20 = new System.Windows.Forms.CheckBox();
			this.cb7 = new System.Windows.Forms.CheckBox();
			this.cb8 = new System.Windows.Forms.CheckBox();
			this.cb21 = new System.Windows.Forms.CheckBox();
			this.cb22 = new System.Windows.Forms.CheckBox();
			this.cb23 = new System.Windows.Forms.CheckBox();
			// 
			// cb2
			// 
			this.cb2.Location = new System.Drawing.Point(17, 155);
			this.cb2.Name = "cb2";
			this.cb2.Size = new System.Drawing.Size(20, 20);
			this.cb2.TabIndex = 0;
			// 
			// cb3
			// 
			this.cb3.Location = new System.Drawing.Point(157, 81);
			this.cb3.Name = "cb3";
			this.cb3.Size = new System.Drawing.Size(20, 20);
			this.cb3.TabIndex = 0;
			// 
			// cb0
			// 
			this.cb0.Location = new System.Drawing.Point(593, 118);
			this.cb0.Name = "cb0";
			this.cb0.Size = new System.Drawing.Size(20, 20);
			this.cb0.TabIndex = 0;
			// 
			// panel
			// 
			this.panel.BackColor = System.Drawing.Color.White;
			this.panel.Location = new System.Drawing.Point(226, 81);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(576, 20);
			this.panel.TabIndex = 0;
			// 
			// cb1
			// 
			this.cb1.Location = new System.Drawing.Point(524, 118);
			this.cb1.Name = "cb1";
			this.cb1.Size = new System.Drawing.Size(20, 20);
			this.cb1.TabIndex = 0;
			// 
			// cb4
			// 
			this.cb4.Location = new System.Drawing.Point(88, 81);
			this.cb4.Name = "cb4";
			this.cb4.Size = new System.Drawing.Size(20, 20);
			this.cb4.TabIndex = 0;
			// 
			// cb6
			// 
			this.cb6.Location = new System.Drawing.Point(167, 118);
			this.cb6.Name = "cb6";
			this.cb6.Size = new System.Drawing.Size(20, 20);
			this.cb6.TabIndex = 0;
			// 
			// cb5
			// 
			this.cb5.Location = new System.Drawing.Point(17, 17);
			this.cb5.Name = "cb5";
			this.cb5.Size = new System.Drawing.Size(20, 20);
			this.cb5.TabIndex = 0;
			// 
			// cb9
			// 
			this.cb9.Location = new System.Drawing.Point(455, 118);
			this.cb9.Name = "cb9";
			this.cb9.Size = new System.Drawing.Size(20, 20);
			this.cb9.TabIndex = 0;
			// 
			// cb10
			// 
			this.cb10.Location = new System.Drawing.Point(17, 81);
			this.cb10.Name = "cb10";
			this.cb10.Size = new System.Drawing.Size(20, 20);
			this.cb10.TabIndex = 0;
			// 
			// cb11
			// 
			this.cb11.Location = new System.Drawing.Point(236, 118);
			this.cb11.Name = "cb11";
			this.cb11.Size = new System.Drawing.Size(20, 20);
			this.cb11.TabIndex = 0;
			// 
			// cb12
			// 
			this.cb12.Location = new System.Drawing.Point(304, 81);
			this.cb12.Name = "cb12";
			this.cb12.Size = new System.Drawing.Size(20, 20);
			this.cb12.TabIndex = 0;
			// 
			// cb13
			// 
			this.cb13.Location = new System.Drawing.Point(633, 44);
			this.cb13.Name = "cb13";
			this.cb13.Size = new System.Drawing.Size(20, 20);
			this.cb13.TabIndex = 0;
			// 
			// cb14
			// 
			this.cb14.Location = new System.Drawing.Point(13, 81);
			this.cb14.Name = "cb14";
			this.cb14.Size = new System.Drawing.Size(20, 20);
			this.cb14.TabIndex = 0;
			// 
			// cb15
			// 
			this.cb15.Location = new System.Drawing.Point(92, 118);
			this.cb15.Name = "cb15";
			this.cb15.Size = new System.Drawing.Size(20, 20);
			this.cb15.TabIndex = 0;
			// 
			// cb16
			// 
			this.cb16.Location = new System.Drawing.Point(598, 81);
			this.cb16.Name = "cb16";
			this.cb16.Size = new System.Drawing.Size(20, 20);
			this.cb16.TabIndex = 0;
			// 
			// cb17
			// 
			this.cb17.Location = new System.Drawing.Point(17, 118);
			this.cb17.Name = "cb17";
			this.cb17.Size = new System.Drawing.Size(20, 20);
			this.cb17.TabIndex = 0;
			// 
			// cb18
			// 
			this.cb18.Location = new System.Drawing.Point(673, 81);
			this.cb18.Name = "cb18";
			this.cb18.Size = new System.Drawing.Size(20, 20);
			this.cb18.TabIndex = 0;
			// 
			// cb19
			// 
			this.cb19.Location = new System.Drawing.Point(311, 118);
			this.cb19.Name = "cb19";
			this.cb19.Size = new System.Drawing.Size(20, 20);
			this.cb19.TabIndex = 0;
			// 
			// cb20
			// 
			this.cb20.Location = new System.Drawing.Point(523, 81);
			this.cb20.Name = "cb20";
			this.cb20.Size = new System.Drawing.Size(20, 20);
			this.cb20.TabIndex = 0;
			// 
			// cb7
			// 
			this.cb7.Location = new System.Drawing.Point(386, 118);
			this.cb7.Name = "cb7";
			this.cb7.Size = new System.Drawing.Size(20, 20);
			this.cb7.TabIndex = 0;
			// 
			// cb8
			// 
			this.cb8.Location = new System.Drawing.Point(379, 81);
			this.cb8.Name = "cb8";
			this.cb8.Size = new System.Drawing.Size(20, 20);
			this.cb8.TabIndex = 0;
			// 
			// cb21
			// 
			this.cb21.Location = new System.Drawing.Point(662, 118);
			this.cb21.Name = "cb21";
			this.cb21.Size = new System.Drawing.Size(20, 20);
			this.cb21.TabIndex = 0;
			// 
			// cb22
			// 
			this.cb22.Location = new System.Drawing.Point(448, 81);
			this.cb22.Name = "cb22";
			this.cb22.Size = new System.Drawing.Size(20, 20);
			this.cb22.TabIndex = 0;
			// 
			// cb23
			// 
			this.cb23.Location = new System.Drawing.Point(86, 155);
			this.cb23.Name = "cb23";
			this.cb23.Size = new System.Drawing.Size(20, 20);
			this.cb23.TabIndex = 0;

		}
		#endregion

		public void checkAll()
		{
			foreach(Control control in this.panel.Controls)
			{
				if (control is CheckBox)
				{
					((CheckBox)control).Checked = true;
				}
			}			
		}

		public void uncheckAll()
		{
			foreach(Control control in this.panel.Controls)
			{
				if (control is CheckBox)
				{
					((CheckBox)control).Checked = false;
				}
			}			
		}

		public void disableAll()
		{
			foreach(Control control in this.panel.Controls)
			{
				if (control is CheckBox)
				{
					((CheckBox)control).Enabled = false;
				}
			}
		}

		public void setCheckBoxes(int ch0, int ch1, int ch2, int ch3, int ch4, int ch5,
			int ch6, int ch7, int ch8, int ch9, int ch10, int ch11, int ch12, int ch13, 
			int ch14, int ch15, int ch16, int ch17, int ch18, int ch19, int ch20, 
			int ch21, int ch22, int ch23)
		{
			cb0.Checked = (ch0 == 1 ? true : false);
			cb1.Checked = (ch1 == 1 ? true : false);
			cb2.Checked = (ch2 == 1 ? true : false);
			cb3.Checked = (ch3 == 1 ? true : false);
			cb4.Checked = (ch4 == 1 ? true : false);
			cb5.Checked = (ch5 == 1 ? true : false);
			cb6.Checked = (ch6 == 1 ? true : false);
			cb7.Checked = (ch7 == 1 ? true : false);
			cb8.Checked = (ch8 == 1 ? true : false);
			cb9.Checked = (ch9 == 1 ? true : false);
			cb10.Checked = (ch10 == 1 ? true : false);
			cb11.Checked = (ch11 == 1 ? true : false);
			cb12.Checked = (ch12 == 1 ? true : false);
			cb13.Checked = (ch13 == 1 ? true : false);
			cb14.Checked = (ch14 == 1 ? true : false);
			cb15.Checked = (ch15 == 1 ? true : false);
			cb16.Checked = (ch16 == 1 ? true : false);
			cb17.Checked = (ch17 == 1 ? true : false);
			cb18.Checked = (ch18 == 1 ? true : false);
			cb19.Checked = (ch19 == 1 ? true : false);
			cb20.Checked = (ch20 == 1 ? true : false);
			cb21.Checked = (ch21 == 1 ? true : false);
			cb22.Checked = (ch22 == 1 ? true : false);
			cb23.Checked = (ch23 == 1 ? true : false);
		}

		public string checked0()
		{
			return (cb0.Checked ? "1" : "0");
		}

		public string checked1()
		{
			return (cb1.Checked ? "1" : "0");
		}

		public string checked2()
		{
			return (cb2.Checked ? "1" : "0");
		}

		public string checked3()
		{
			return (cb3.Checked ? "1" : "0");
		}

		public string checked4()
		{
			return (cb4.Checked ? "1" : "0");
		}

		public string checked5()
		{
			return (cb5.Checked ? "1" : "0");
		}

		public string checked6()
		{
			return (cb6.Checked ? "1" : "0");
		}

		public string checked7()
		{
			return (cb7.Checked ? "1" : "0");
		}

		public string checked8()
		{
			return (cb8.Checked ? "1" : "0");
		}

		public string checked9()
		{
			return (cb9.Checked ? "1" : "0");
		}

		public string checked10()
		{
			return (cb10.Checked ? "1" : "0");
		}

		public string checked11()
		{
			return (cb11.Checked ? "1" : "0");
		}

		public string checked12()
		{
			return (cb12.Checked ? "1" : "0");
		}

		public string checked13()
		{
			return (cb13.Checked ? "1" : "0");
		}

		public string checked14()
		{
			return (cb14.Checked ? "1" : "0");
		}

		public string checked15()
		{
			return (cb15.Checked ? "1" : "0");
		}

		public string checked16()
		{
			return (cb16.Checked ? "1" : "0");
		}

		public string checked17()
		{
			return (cb17.Checked ? "1" : "0");
		}

		public string checked18()
		{
			return (cb18.Checked ? "1" : "0");
		}

		public string checked19()
		{
			return (cb19.Checked ? "1" : "0");
		}

		public string checked20()
		{
			return (cb20.Checked ? "1" : "0");
		}

		public string checked21()
		{
			return (cb21.Checked ? "1" : "0");
		}

		public string checked22()
		{
			return (cb22.Checked ? "1" : "0");
		}

		public string checked23()
		{
			return (cb23.Checked ? "1" : "0");
		}

	}
}
