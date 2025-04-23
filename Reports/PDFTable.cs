using System;
using System.Drawing;
using System.Collections;
using System.Configuration;
//using WebSupergoo.ABCpdf4;

using Common;
using Util;


namespace Reports
{
	/// <summary>
	/// Summary description for PDFTable.
	/// </summary>
	public class PDFTable 
	{
		/*
		private PDFDocument mDoc = null;
		private string mRect = "";
		private Point mPos = new Point();
		private Rectangle mBounds = new Rectangle();
		private int mRowTop = 0;
		private int mRowBottom = 0;
		private int[] mHeights = new int [0];
		private int[] mWidths = new int [0];
		private ArrayList mRowIDs = new ArrayList();
		private bool mTruncated = false;
		public int RowHeightMin = 0;
		public int RowHeightMax = 0;
		public int Padding = 0;

		DebugLog debug;

		// Column's names
		public string[] colNames;

		public string[] ColNames
		{
			get {return colNames;}
			set {colNames = value;}
		}

		// Focus on the document and assign the relevant number of columns
		public PDFTable(PDFDocument doc, int columns) 
		{
			// Debug Log
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			mDoc = doc;
			SetRect(mDoc.Rect.String);
			SetColumns(columns);
		}

		// Add a new page, reset the table rect and move to the first row
		public void NewPage() 
		{
			mDoc.Page = mDoc.AddPage(-1);
			SetRect(mRect);
			NextRow();
		}

		// Assign a new table rectangle and reset the current table position
		public void SetRect(string rect) 
		{
			mDoc.Rect.String = rect;
			mRect = mDoc.Rect.String;
			mBounds.Y = (int)mDoc.Rect.Top;
			mBounds.X = (int)mDoc.Rect.Left;
			mBounds.Width = (int)mDoc.Rect.Width;
			mBounds.Height = (int)mDoc.Rect.Height;
			mRowTop = (int)mDoc.Rect.Top;
			mRowBottom = (int)mDoc.Rect.Top;
			mHeights = new int [0];
			mPos.Y = -1;
			mPos.X = -1;
		}

		// Change the number of columns in the table
		public void SetColumns(int inNum) 
		{
			if (inNum > 0) 
			{
				int[] theWidths = new int [ inNum ];
				for (int i = 0; i < theWidths.Length; i++)
					theWidths[i] = 1;
				mWidths.CopyTo(theWidths, 0);
				mWidths = theWidths;
			}
		}

		// Get the current row - a zero based index
		public int Row { get { return mPos.Y; } }
  
		// Get the current column - a zero based index
		public int Column { get { return mPos.X; } }

		// Find out if (the last row we added was truncated
		public bool RowTruncated { get { return mTruncated; } }

		// Change a column width
		public void SetColumnWidth(int i, int inNum) 
		{
			if ((i >= 0) && (i < mWidths.Length)) mWidths[i] = inNum;
		}

		// Move to the next column in the current row
		public void NextCell() 
		{
			mPos.X = mPos.X + 1;
			if (mPos.X >= mWidths.Length) mPos.X = mWidths.Length - 1;
			if (mPos.X < 0) mPos.X = 0;
			SelectCurrentCell(mPos.X);
		}

		// Move to the Next row - return false if the Next row would not fit on the page
		public bool NextRow() 
		{
			int theBottom = 0;
			mRowTop = mRowTop - GetRowHeight(mPos.Y);
			mDoc.Rect.String = mRect;
			mRowBottom = mRowTop;
			mDoc.Rect.Top = mRowTop;
			if (RowHeightMax > 0) 
			{
				theBottom = mRowTop - RowHeightMax;
				if (mDoc.Rect.Bottom < theBottom) mDoc.Rect.Bottom = theBottom;
			}
			bool isNextRow = (mDoc.Rect.Height > (RowHeightMin + (2 * Padding)));
			if (isNextRow) 
			{
				mPos.Y = mPos.Y + 1;
				mPos.X = -1;
				mRowIDs.Clear();
				mTruncated = false;
			}
			return isNextRow;
		}

		// Add text to the currently selected area
		public string AddText(string inText) 
		{
			string theRect = mDoc.Rect.String;
			int thePos = 0;
			mDoc.Rect.Inset (Padding, Padding);
			int theID = mDoc.AddText(inText);
			AddToRow(theID);
			if (!mTruncated) 
			{
				int theDrawn = 0;
				if (theID > 0)
					theDrawn = mDoc.GetInfoInt(theID, "Characters");
				if (theDrawn < inText.Length)
					mTruncated = true;
			}
			thePos = (int)(mDoc.Pos.Y - mDoc.FontSize);
			if (thePos < mRowBottom) mRowBottom = thePos;
			mDoc.Rect.String = theRect;
			return theID.ToString();
		}

		// Select the entire table area
		public void SelectTable() 
		{
			mDoc.Rect.String = mRect;
		}
  
		// Select a cell in the current row using a zero based index
		public void SelectCell(int inIndex) 
		{
			GetRowHeight(mPos.Y); // fix the current row height
			SelectCells(inIndex, mPos.Y, inIndex, mPos.Y);
		}

		// Select a row on the current page using a zero based index
		public void SelectRow(int inIndex) 
		{
			GetRowHeight(mPos.Y); // fix the current row height
			SelectCells(0, inIndex, mWidths.Length - 1, inIndex);
		}

		// Select a column on the current page using a zero based index
		public void SelectColumn(int inIndex) 
		{
			GetRowHeight(mPos.Y); // fix the current row height
			SelectCells(inIndex, 0, inIndex, mHeights.Length - 1);
		}
  
		// Select a rectangular area of cells on the current page
		public void SelectCells(int inX1, int inY1, int inX2, int inY2) 
		{
			int theTemp, i;
			double theTop, theLeft;
			// check inputs
			if (inX1 > inX2) 
			{
				theTemp = inX1;
				inX1 = inX2;
				inX2 = theTemp;
			}
			if (inY1 > inY2) 
			{
				theTemp = inX1;
				inY1 = inY2;
				inY2 = theTemp;
			}
			GetRowHeight(mPos.Y); // fix the current row height
			if (inY1 >= mHeights.Length) inY1 = mHeights.Length - 1;
			if (inY2 >= mHeights.Length) inY2 = mHeights.Length - 1;
			if (inY1 < 0) return;
			// select the cells
			mDoc.Rect.String = mRect;
			theTop = mDoc.Rect.Top;
			SelectCurrentCell(inX1);
			theLeft = mDoc.Rect.Left;
			SelectCurrentCell(inX2);
			mDoc.Rect.Top = theTop;
			mDoc.Rect.Bottom = theTop;
			mDoc.Rect.Left = theLeft;
			for (i = 0; i <= inY2; i++) 
			{
				mDoc.Rect.Bottom = mDoc.Rect.Bottom - mHeights[i];
				if (inY1 > i) mDoc.Rect.Top = mDoc.Rect.Top - mHeights[i];
			}
		}

		// Draw borders round the current selection
		public void Frame(bool inTop, bool inBott, bool inLeft, bool inRight) 
		{
			double t = mDoc.Rect.Top;
			double l = mDoc.Rect.Left;
			double b = mDoc.Rect.Bottom;
			double r = mDoc.Rect.Right;
			if (inTop) AddToRow(mDoc.AddLine(l, t, r, t));
			if (inBott) AddToRow(mDoc.AddLine(l, b, r, b));
			if (inLeft) AddToRow(mDoc.AddLine(l, t, l, b));
			if (inRight) AddToRow(mDoc.AddLine(r, t, r, b));
		}

		// Color the background of the current selection
		public void Fill(string inColor) 
		{
			int theLayer = mDoc.Layer;
			string theColor = mDoc.Color.String;
			mDoc.Layer = mDoc.LayerCount + 1;
			mDoc.Color.String = inColor;
			AddToRow(mDoc.FillRect());
			mDoc.Color.String = theColor;
			mDoc.Layer = theLayer;
		}

		// Get the current row height based on the cell contents drawn so far
		private int GetRowHeight(int inRow) 
		{
			if ((inRow >= 0) && ((mHeights.Length - 1) < inRow)) 
			{
				// establish and store current row height
				int theHeight;
				int[] theCopy = new int [ inRow + 1 ];
				mHeights.CopyTo(theCopy, 0);
				mHeights = theCopy;
				mRowBottom = mRowBottom - Padding;
				if (mRowBottom < mBounds.Y - mBounds.Height) 
					mRowBottom = mBounds.Y - mBounds.Height;
				theHeight = mRowTop - mRowBottom;
				if ((inRow > 0) && (theHeight < RowHeightMin)) 
					theHeight = RowHeightMin;
				if ((RowHeightMax > 0) && (theHeight < RowHeightMax)) 
					theHeight = RowHeightMax;
				mHeights[inRow] = theHeight;
			}
			if (inRow >= 0) return mHeights[inRow];
			return 0;
		}

		// Select the current cell
		private void SelectCurrentCell(int inIndex) 
		{
			int theTotal = 0, thePos = 0;
			if ((inIndex >= 0) && (inIndex < mWidths.Length)) 
			{
				// get the x offset and width of the cell
				for (int i = 0; i < mWidths.Length; i++) 
				{
					theTotal = theTotal + mWidths[i];
					if (i < inIndex) thePos = thePos + mWidths[i];
				}
				thePos = thePos * (mBounds.Width / theTotal);
				int theWidth = mWidths[inIndex] * (mBounds.Width / theTotal);
				// position the cell
				mDoc.Rect.Top = mRowTop;
				mDoc.Rect.Left = mBounds.X + thePos;
				mDoc.Rect.Width = theWidth;
			}
		}

		// Add to our list of objects drawn part of the current row
		private string AddToRow(int inID) 
		{
			mRowIDs.Add(inID);
			return inID.ToString();
		}

		// Delete all the objects drawn part of the current row
		public void DeleteLastRow() 
		{
			for (int i = 0; i < mRowIDs.Count; i++)
				mDoc.Delete((int)mRowIDs[i]);
			mRowIDs.Clear();
		}

		public void TableSetup(string[] colNames, int[] columnWidth)
		{
			// Row
			this.RowHeightMax = Constants.RowHeightMax;
			this.RowHeightMin = Constants.RowHeightMin;

			// Set column's names
			this.ColNames = colNames;

			// Set column's width
			for (int i = 0; i < columnWidth.Length; i++)
			{
				SetColumnWidth(i, columnWidth[i]);
			}
		}

		public void InsertTableHeader(string[] headerFileds)
		{
			try
			{
				mDoc.FontSize = Constants.pdfFontSize + 1;
				mDoc.TextStyle.Bold = true;
				
				for (int k = 0 ; k < headerFileds.Length; k++) 
				{
					mDoc.HPos = 0.5;

					// add the content to the cell
					this.NextCell();
					this.AddText(headerFileds[k]);

				}
				this.SelectRow(this.Row);
				this.Frame(false, true, false, false);
				
				mDoc.FontSize = Constants.pdfFontSize;
				mDoc.TextStyle.Bold = false;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PDFTable.InsertTableHeader(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void populateTable(double[] colPositions, ArrayList tableData)
		{

			foreach (string[] theCols in tableData) 
			{
				// add in new row
				bool feet = this.NextRow();

				if (!feet)
				{
					this.mDoc.Rect.SetRect(this.mDoc.LeftMargine, this.mDoc.BottomMargine, 
						(this.mDoc.StandardWidth - (this.mDoc.LeftMargine + this.mDoc.RightMargine)),
						(this.mDoc.StandardHeight - (this.mDoc.TopMargine + this.mDoc.BottomMargine)));

					this.mDoc.Page = mDoc.AddPage(-1);
					SetRect(this.mDoc.Rect.String);
					this.NextRow();
					this.InsertTableHeader(colNames);
					this.NextRow();
				}
				
				// add in columns
				for (int j = 0 ; j < theCols.Length; j++) 
				{
					// all but first column right aligned
					mDoc.HPos = colPositions[j];
					// add the content to the cell
					this.NextCell();
					this.AddText(theCols[j]);
				}
			}
		}


		public void populateTable(double[] colPositions, ArrayList tableData, bool haveTotals, ArrayList totalsData, bool haveTotalsByPT, ArrayList ptTotalsRowList)
		{

			int maxNumOfRows = tableData.Count;
			//string testString;
			foreach (string[] theCols in tableData) 
			{
				// add in new row
				bool feet = this.NextRow();
				if (!feet)
				{
					this.mDoc.Rect.SetRect(this.mDoc.LeftMargine, this.mDoc.BottomMargine, 
						(this.mDoc.StandardHeight - (this.mDoc.TopMargine + this.mDoc.BottomMargine)),
						(this.mDoc.StandardWidth - (this.mDoc.LeftMargine + this.mDoc.RightMargine)));

					this.mDoc.Page = mDoc.AddPage(-1);
					SetRect(this.mDoc.Rect.String);
					this.NextRow();

					this.InsertTableHeader(colNames);
					this.NextRow();
				}

				// add in columns
				for (int j = 0 ; j < theCols.Length; j++) 
				{
					// all but first column right aligned
					mDoc.HPos = colPositions[j];
					// add the content to the cell
					this.NextCell();
					this.AddText(theCols[j]);
				}
			}
			
			if (haveTotalsByPT)
			{
				this.NextRow();
				// add in new row
				bool feet = this.NextRow();
				if (!feet)
				{
					this.mDoc.Rect.SetRect(this.mDoc.LeftMargine, this.mDoc.BottomMargine, 
						(this.mDoc.StandardHeight - (this.mDoc.TopMargine + this.mDoc.BottomMargine)),
						(this.mDoc.StandardWidth - (this.mDoc.LeftMargine + this.mDoc.RightMargine)));

					this.mDoc.Page = mDoc.AddPage(-1);
					SetRect(this.mDoc.Rect.String);
					this.NextRow();
				}
				this.Frame(true, false, false, false);

				// add in columns
				foreach (string row in ptTotalsRowList) 
				{	
					// add in new row
					feet = this.NextRow();
					if (!feet)
					{
						this.mDoc.Rect.SetRect(this.mDoc.LeftMargine, this.mDoc.BottomMargine, 
							(this.mDoc.StandardHeight - (this.mDoc.TopMargine + this.mDoc.BottomMargine)),
							(this.mDoc.StandardWidth - (this.mDoc.LeftMargine + this.mDoc.RightMargine)));

						this.mDoc.Page = mDoc.AddPage(-1);
						SetRect(this.mDoc.Rect.String);
						this.NextRow();
					}

					mDoc.HPos = 0.0;
					this.AddText(row.Trim());
				}
			}

			if (haveTotals)
			{
				this.NextRow();
				// add in new row
				bool feet = this.NextRow();
				if (!feet)
				{
					this.mDoc.Rect.SetRect(this.mDoc.LeftMargine, this.mDoc.BottomMargine, 
						(this.mDoc.StandardHeight - (this.mDoc.TopMargine + this.mDoc.BottomMargine)),
						(this.mDoc.StandardWidth - (this.mDoc.LeftMargine + this.mDoc.RightMargine)));

					this.mDoc.Page = mDoc.AddPage(-1);
					SetRect(this.mDoc.Rect.String);
					this.NextRow();
				}

				this.Frame(true, false, false, false);
			
				// add in columns
				foreach (ArrayList theCols in totalsData) 
				{
					// add in columns
					for (int j = 0 ; j < theCols.Count; j++) 
					{
						// all but first column right aligned
						mDoc.HPos = colPositions[j];
						// add the content to the cell
						this.NextCell();
						this.AddText(theCols[j].ToString());
					}
				}
			}
		}
		*/
	}
}
