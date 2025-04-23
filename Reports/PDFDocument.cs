using System;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
//using WebSupergoo.ABCpdf4;

using Common;
using Util;

namespace Reports
{
	/// <summary>
	/// Implements custom methods for PDF reports genetration, 
	/// using ABC Pdf tool.
	/// </summary>
	public class PDFDocument // : Doc
	{
		/*
		private int _leftMargine;
		private int _rightMargine;
		private int _topMargine;
		private int _bottomMargine;

		private int _headerHeight;

		private string _title;
		private int _titleFontSize;

		private string _leftBoxText;
		private int _leftBoxTextSize;

		private string _rightBoxText;
		private int _rightBoxTextSize;

		private string _filePath;
		private bool _isLandscape;

		public int StandardWidth = Constants.PDFStandardPageWidth;
		public int StandardHeight = Constants.PDFStandardPageHeight;

		DebugLog debug;

		public int LeftMargine
		{
			get { return _leftMargine; }
			set { _leftMargine = value; }
		}

		public int RightMargine
		{ 
			get { return _rightMargine; }
			set { _rightMargine = value; }
		}

		public int TopMargine
		{
			get { return _topMargine; }
			set { _topMargine = value; }
		}

		public int BottomMargine
		{
			get { return _bottomMargine; }
			set { _bottomMargine = value; }
		}

		public int HeaderHeight
		{
			get { return _headerHeight; }
			set {_headerHeight = value; }
		}
		
		public string Title
		{
			set { _title = value; }
			get { return _title; }
		}

		public int TitleFontSize
		{
			set { _titleFontSize = value; }
			get { return _titleFontSize; }
		}

		public string LeftBoxText
		{
			get { return _leftBoxText; }
			set { _leftBoxText = value; }
		}

		public int LeftBoxTextSize
		{
			get { return _leftBoxTextSize; }
			set { _leftBoxTextSize = value; }
		}

		public string RightBoxText 
		{ 
			get { return _rightBoxText; }
			set { _rightBoxText = value; }
		}

		public int RightBoxTextSize
		{
			get { return _rightBoxTextSize; }
			set { _rightBoxTextSize = value; }
		}

		public string FilePath
		{
			get { return _filePath; }
			set {_filePath = value; }
		}

		public bool IsLandscape
		{
			get { return _isLandscape; }
			set { _isLandscape = value; }
		}

		public PDFDocument()
		{
			// Init debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			this.TopMargine = Constants.TopMargine;
			this.BottomMargine = Constants.BottomMargine;
			this.LeftMargine = Constants.LeftMargine;
			this.RightMargine = Constants.RightMargine;

			this.AddPage();
			// Set Footer
		}

		

		public void Save()
		{
			try
			{
				if (IsLandscape)
				{
					int theID = this.GetInfoInt(this.Root, "Pages");
					this.SetInfo(theID, "/Rotate", "90");
				}

				//InsertFooter();

				this.Save(this.FilePath);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PDFDocument.Save(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void Open()
		{
			try
			{
				Process.Start(this.FilePath);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PDFDocument.Open(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}


		public void InsertTable(string[] colNames, int[] colWidths, double[] colPositions, ArrayList tableData)
		{
			try
			{
				int leftMargine;
				int rightMargine;
				int topMargine;
				int bottomMargine;
				int width;
				int height;

				if (!IsLandscape)
				{
					leftMargine = this.LeftMargine;
					rightMargine = this.RightMargine;
					topMargine = this.TopMargine;
					bottomMargine = this.BottomMargine;
					width = this.StandardWidth;
					height = this.StandardHeight;
				}
				else
				{
					leftMargine = this.BottomMargine;
					rightMargine = this.TopMargine;
					topMargine = this.LeftMargine;
					bottomMargine = this.RightMargine;
					width = this.StandardHeight;
					height = this.StandardWidth;
				}

				this.Rect.Position(leftMargine, bottomMargine);
				this.Rect.Width = width - (leftMargine + rightMargine);
				this.Rect.Height = height - this.HeaderHeight - topMargine - bottomMargine;

				PDFTable theTable = new PDFTable(this, colNames.Length);

				theTable.TableSetup(colNames, colWidths);

							
				theTable.NextRow();
				theTable.InsertTableHeader(colNames);

				theTable.populateTable(colPositions, tableData);

				this.Flatten();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PDFDocument.InsertTable(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void InsertTable(string[] colNames, int[] colWidths, double[] colPositions, 
			ArrayList tableData, bool haveTotals, ArrayList totalsData, bool haveTotalsByPT, ArrayList ptTotalsRowList)
		{
			try
			{
				int leftMargine;
				int rightMargine;
				int topMargine;
				int bottomMargine;
				int width;
				int height;

				if (!IsLandscape)
				{
					leftMargine = this.LeftMargine;
					rightMargine = this.RightMargine;
					topMargine = this.TopMargine;
					bottomMargine = this.BottomMargine;
					width = this.StandardWidth;
					height = this.StandardHeight;
				}
				else
				{
					leftMargine = this.BottomMargine;
					rightMargine = this.TopMargine;
					topMargine = this.LeftMargine;
					bottomMargine = this.RightMargine;
					width = this.StandardHeight;
					height = this.StandardWidth;
				}

				this.Rect.Position(leftMargine, bottomMargine);
				this.Rect.Width = width - (leftMargine + rightMargine);
				this.Rect.Height = height - this.HeaderHeight - topMargine - bottomMargine;

				PDFTable theTable = new PDFTable(this, colNames.Length);

				theTable.TableSetup(colNames, colWidths);

							
				theTable.NextRow();
				theTable.InsertTableHeader(colNames);

				theTable.populateTable(colPositions, tableData, haveTotals, totalsData, haveTotalsByPT, ptTotalsRowList);

				this.Flatten();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PDFDocument.InsertTable(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		public void InsertFooter(int fontSize)
		{
			if (!IsLandscape)
			{
				this.Rect.Position(this.LeftMargine, this.BottomMargine / 2);
				this.Rect.Width = StandardWidth - this.LeftMargine - this.RightMargine;
				this.Rect.Height = this.BottomMargine / 2;
				this.HPos = 1.0;
				this.VPos = 0.5;
			}
			else
			{
				this.Rect.Position(this.LeftMargine, this.BottomMargine / 2);
				this.Rect.Width = StandardHeight - this.BottomMargine - this.TopMargine;
				this.Rect.Height = (this.BottomMargine/2) + 20;
				this.HPos = 1.0;
				this.VPos = 0.5;
			}

			this.FontSize = fontSize;

			for(int i=1; i<=this.PageCount; i++)
			{
				this.PageNumber = i;
				this.AddText("Strana " + i);
			}
		}

		public void SetLandscape()
		{
			// apply a rotation transform
			double w = this.MediaBox.Width;
			double h = this.MediaBox.Height;
			double l = this.MediaBox.Left;
			double b = this.MediaBox.Bottom; 
			this.Transform.Rotate(90, l, b);
			this.Transform.Translate(w, 0); 

			// rotate our rectangle
			this.Rect.Width = h;
			this.Rect.Height = w;

			this.IsLandscape = true;
		}

		public void ResetRect()
		{
			this.Rect.Position(this.LeftMargine, this.TopMargine);
		}
		*/
	}
}

