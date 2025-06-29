﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Microsoft.Office.Interop.Excel;

namespace Reports.Magna
{
    public class ExcelMilosSanja
    {
        private Application app = null;
        private Workbook workbook = null;
        private Worksheet worksheet = null;
        private Range workSheet_range = null;

        public ExcelMilosSanja()
        {
            createDoc(); 
        }
        private void createDoc()
        {
            try
            {
                Application app = new Application();
                app.Visible = true;
                workbook = app.Workbooks.Add(1);
                worksheet = (Worksheet)workbook.Sheets[1];
            }
            catch (Exception e)
            {
                Console.Write("Error");
            }
            finally
            {
            }
        }

        public void createHeaders(int row, int col, string htext, string cell1,string cell2, int mergeColumns,string b, bool font,int size,string fcolor)
        {
            worksheet.Cells[row, col] = htext;
            workSheet_range = worksheet.get_Range(cell1, cell2);
            workSheet_range.Merge(mergeColumns);
            switch(b)
            {
                case "YELLOW":
                workSheet_range.Interior.Color = Color.Yellow.ToArgb();
                break;
                case "GRAY":
                    workSheet_range.Interior.Color = Color.Gray.ToArgb();
                break;
                case "GAINSBORO":
                    workSheet_range.Interior.Color = Color.Gainsboro.ToArgb();
                    break;
                case "Turquoise":
                    workSheet_range.Interior.Color = Color.Turquoise.ToArgb();
                    break;
                case "PeachPuff":
                    workSheet_range.Interior.Color = Color.PeachPuff.ToArgb();
                    break;
                default:
                    //workSheet_range.Interior.Color = Color.ToArgb();
                    break;
            }
         
            workSheet_range.Borders.Color = Color.Black.ToArgb();
            workSheet_range.Font.Bold = font;
            workSheet_range.ColumnWidth = size;

            if (fcolor.Equals(""))
            {
                workSheet_range.Font.Color = Color.White.ToArgb();
            }
            else {
                workSheet_range.Font.Color = Color.Black.ToArgb();
            }
        }

        public void addData(int row, int col, string data, string cell1, string cell2, string format)
        {
            worksheet.Cells[row, col] = data;
            workSheet_range = worksheet.get_Range(cell1, cell2);
            workSheet_range.Borders.Color = Color.Black.ToArgb();
            workSheet_range.NumberFormat = format;
        }    
    }
}