using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.IO;

using System.Security.Permissions;
using Util;

namespace ACTAWorkAnalysisReports
{
    public class ExportToExcel
    {
        bool PA = false;
        public static DebugLog debug;
        public static bool CreateExcelDocument<T>(List<T> list, string xlsxFilePath, bool ispa, bool isAnomalies)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ListToDataTable(list));

            return CreateExcelDocument(ds, xlsxFilePath, ispa, isAnomalies);
        }
        public static bool CreateExcelDocument<T>(List<T> list, string xlsxFilePath, string[] columns, string selectedWu, string date)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ListToDataTable(list));

            return CreateExcelDocument(ds, xlsxFilePath, columns, selectedWu, date);
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        public bool CreateExcelDocument(DataTable dt, string xlsxFilePath, string[] columns, string selectedWu, string date)
        {
            try
            {
               
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                return CreateExcelDocument(ds, xlsxFilePath, columns, selectedWu, date);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool CreateExcelDocument(DataTable dt, string xlsxFilePath, bool isPA, bool isAnomaies)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                return CreateExcelDocument(ds, xlsxFilePath, isPA, isAnomaies);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static bool CreateExcelDocument(DataSet ds, string excelFilename, string[] columns, string selectedWU, string date)
        {
            try
            {
                debug = new DebugLog(Constants.logFilePath + "OpenXML" + "Log.txt");
              
                Stream destination = File.Create(excelFilename);
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(destination, SpreadsheetDocumentType.Workbook))
                {
                    CreateParts(ds, document, columns, selectedWU, date);

                }
                Trace.WriteLine("Successfully created: " + excelFilename);
                destination.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public static bool CreateExcelDocument(DataSet ds, string excelFilename, bool isPA, bool isAnomalies)
        {
            try
            {

                Stream destination = File.Create(excelFilename);
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(destination, SpreadsheetDocumentType.Workbook))
                {
                    CreateParts(ds, document, isPA, isAnomalies);
                }
                Trace.WriteLine("Successfully created: " + excelFilename);
                destination.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private static void CreateParts(DataSet ds, SpreadsheetDocument document, string[] columns, string selectedWu, string date)
        {
            debug.writeLog("CreateParts");
            WorkbookPart workbookPart = document.AddWorkbookPart();
            Workbook workbook = new Workbook();
            workbookPart.Workbook = workbook;
            //workbook.AddNamespaceDeclaration("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");

            NumberingFormat nf2decimal = new NumberingFormat();
            nf2decimal.NumberFormatId = UInt32Value.FromUInt32(3453);
            nf2decimal.FormatCode = StringValue.FromString("0.00");

            Stylesheet stylesheet = GenerateStyleSheet(nf2decimal);
            //stylesheet.AddNamespaceDeclaration("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");


            workbookStylesPart.Stylesheet = stylesheet;
            workbookStylesPart.Stylesheet.NumberingFormats = new NumberingFormats();
            workbookStylesPart.Stylesheet.NumberingFormats.Append(nf2decimal);
            string worksheetName = ds.DataSetName; uint worksheetNumber = 1;
            Sheets sheets = new Sheets(); string workSheetID = "rId" + worksheetNumber.ToString();


            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>(workSheetID);
            Worksheet worksheet = new Worksheet();

            SheetViews sheetViews = new SheetViews();

            SheetView sheetView = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            sheetViews.Append(sheetView);

            SheetData sheetData1 = new SheetData(); int rowIndex = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                if (dt.Rows.Count > 0)
                {
                    rowIndex = WriteDataTableToExcelWorksheet(dt, worksheetPart, sheetData1, sheetViews, worksheetPart, worksheet, rowIndex, columns, selectedWu, date);
                }
                //worksheetNumber++;
            }
            worksheet.Append(sheetViews);
            worksheet.Append(sheetData1);

            worksheetPart.Worksheet = worksheet;
            Sheet sheet = new Sheet() { Name = worksheetName, SheetId = (UInt32Value)worksheetNumber, Id = workSheetID };
            //sheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            sheets.Append(sheet);
            workbook.Append(sheets);


        }
        private static void CreateParts(DataSet ds, SpreadsheetDocument document, bool isPA, bool isAnomalies)
        {

            WorkbookPart workbookPart = document.AddWorkbookPart();
            Workbook workbook = new Workbook();
            workbookPart.Workbook = workbook;
            //workbook.AddNamespaceDeclaration("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");

            NumberingFormat nf2decimal = new NumberingFormat();
            nf2decimal.NumberFormatId = UInt32Value.FromUInt32(3453);
            nf2decimal.FormatCode = StringValue.FromString("0.00");

            Stylesheet stylesheet = GenerateStyleSheet(nf2decimal);
            //stylesheet.AddNamespaceDeclaration("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");


            workbookStylesPart.Stylesheet = stylesheet;
            workbookStylesPart.Stylesheet.NumberingFormats = new NumberingFormats();
            workbookStylesPart.Stylesheet.NumberingFormats.Append(nf2decimal);
            string worksheetName = ds.DataSetName; uint worksheetNumber = 1;
            Sheets sheets = new Sheets(); string workSheetID = "rId" + worksheetNumber.ToString();


            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>(workSheetID);
            Worksheet worksheet = new Worksheet();

            SheetViews sheetViews = new SheetViews();

            SheetView sheetView = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            sheetViews.Append(sheetView);

            SheetData sheetData1 = new SheetData(); int rowIndex = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                if (dt.Rows.Count > 6)
                {
                    rowIndex = WriteDataTableToExcelWorksheet(dt, worksheetPart, sheetData1, sheetViews, worksheetPart, worksheet, rowIndex, isPA, isAnomalies);
                }
                //worksheetNumber++;
            }
            worksheet.Append(sheetViews);
            worksheet.Append(sheetData1);

            worksheetPart.Worksheet = worksheet;
            Sheet sheet = new Sheet() { Name = worksheetName, SheetId = (UInt32Value)worksheetNumber, Id = workSheetID };
            //sheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            sheets.Append(sheet);
            workbook.Append(sheets);
        }
        private static Stylesheet GenerateStyleSheet(NumberingFormat nf2decimal)
        {

            return new Stylesheet(
                new NumberingFormats(),
                new Fonts(
                    new Font(                                                               // Index 0 - The default font.
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 1 - The bold font.
                        new Bold(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 - The Italic font.
                        new Italic(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 - The Times Roman font. with 16 size
                        new FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(                                                           // Index 0 - The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(                                                           // Index 1 - The default fill of gray 125 (required)
                        new PatternFill() { PatternType = PatternValues.Gray125 }),
                    new Fill(                                                           // Index 2 - The yellow fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }
                        ) { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(                                                         // Index 0 - The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Hair },
                        new RightBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Hair },
                        new TopBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Hair },
                        new BottomBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Hair },
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 - The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyFont = true },// Index 1 - Bold 
                     new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyFont = true, NumberFormatId = nf2decimal.NumberFormatId, ApplyNumberFormat = true },    // Index 2 - Bold 
                    new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 - Italic
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 4 - Times Roman
                    new CellFormat() { FontId = 0, FillId = 1, BorderId = 0, ApplyFill = true },       // Index 5 - Yellow Fill
                    new CellFormat(                                                                   // Index 6 - Alignment
                        new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    ) { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 7 - Border
                )
            ); // return
        }

        private static int WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart1, SheetData sheetData1, SheetViews sheetViews, WorksheetPart worksheetPart, Worksheet worksheet, int rowIndex, bool isPA, bool isAnomalies)
        {

            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            ////
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;
            int rowNum = 0;
            if (isAnomalies)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ++rowIndex;
                    Row newExcelRow = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };

                    for (int colInx = 0; colInx < numberOfColumns; colInx++)
                    {
                        cellValue = dr.ItemArray[colInx].ToString();


                        if (IsNumericColumn[colInx])
                        {
                            //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                            cellNumericValue = 0;
                            double.TryParse(cellValue, out cellNumericValue);
                            cellValue = cellNumericValue.ToString();
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, isPA);
                        }
                        else
                        {
                            //  For text cells, just write the input data straight out to the Excel file.
                            AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, rowIndex);
                        }
                    }
                    sheetData1.Append(newExcelRow);
                }
            }
            else
            {

                foreach (DataRow dr in dt.Rows)
                {
                    rowNum++;
                    ++rowIndex;
                    Row newExcelRow = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };
                    if (rowNum > 6 && rowNum != dt.Rows.Count)
                    {

                        // ...create a new row, and append a set of this row's data to it.
                        for (int colInx = 0; colInx < numberOfColumns; colInx++)
                        {
                            cellValue = dr.ItemArray[colInx].ToString();


                            try
                            {
                                //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                                cellNumericValue = double.Parse(cellValue);

                                cellValue = cellNumericValue.ToString();

                                AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, isPA);
                            }
                            catch (Exception)
                            {
                                //  For text cells, just write the input data straight out to the Excel file.
                                AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, rowIndex);
                            }
                        }

                    }
                    else
                    {
                        for (int colInx = 0; colInx < numberOfColumns; colInx++)
                        {
                            cellValue = dr.ItemArray[colInx].ToString();

                            AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, rowIndex);
                        }
                    }
                    sheetData1.Append(newExcelRow);
                }
            }
            int rowI = 0;
            if (rowIndex % 52 != 0)
            {
                int indf = 52 - (rowIndex % 52);
                rowIndex += indf + 1;
            }
            while (rowIndex >= rowI)
            {
                rowI++;

            }
            for (int i = rowIndex + 1; i < rowI; i++)
            {
                ++rowIndex;
                Row newExcelRow = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = " ";

                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        cellNumericValue = 0;
                        double.TryParse(cellValue, out cellNumericValue);
                        cellValue = cellNumericValue.ToString();
                        AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, isPA);
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, rowIndex);
                    }
                }
            }

            return rowIndex;

        }

        private static int WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart1, SheetData sheetData1, SheetViews sheetViews, WorksheetPart worksheetPart, Worksheet worksheet, int rowIndex, string[] columns, string selectedWU, string date)
        {
            debug.writeLog("WriteDataTableToExcelWorksheet start" + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            ////
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;
            Row newExcelRowTitle = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };


            AppendTextCell(excelColumnNames[1] + rowIndex.ToString(), selectedWU, newExcelRowTitle, rowIndex);

            AppendTextCell(excelColumnNames[4] + rowIndex.ToString(), date, newExcelRowTitle, rowIndex);

            sheetData1.Append(newExcelRowTitle);
            ++rowIndex;
            ++rowIndex;

            int rowNum = 0;
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {

                Row newExcelRow = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };


                cellValue = columns[colInx].ToString();

                AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, rowIndex);


                sheetData1.Append(newExcelRow);
            }
            debug.writeLog("Columns add finish" + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            ++rowIndex;
            foreach (DataRow dr in dt.Rows)
            {
                ++rowIndex;
                debug.writeLog("Row" + rowIndex + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                Row newExcelRow = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();


                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        cellNumericValue = 0;
                        double.TryParse(cellValue, out cellNumericValue);
                        cellValue = cellNumericValue.ToString();
                        AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, false);
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, rowIndex);
                    }
                }
                sheetData1.Append(newExcelRow);
            }


            int rowI = 0;
            //if (rowIndex % 52 != 0)
            //{
            //    int indf = 52 - (rowIndex % 52);
            //    rowIndex += indf + 1;
            //}
            while (rowIndex >= rowI)
            {
                rowI++;

            }
            for (int i = rowIndex + 1; i < rowI; i++)
            {
                ++rowIndex;
                Row newExcelRow = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = " ";

                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        cellNumericValue = 0;
                        double.TryParse(cellValue, out cellNumericValue);
                        cellValue = cellNumericValue.ToString();
                        AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, false);
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, rowIndex);
                    }
                }
            }
            debug.writeLog("WriteDataTableToExcelWorksheet end" + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

            return rowIndex;

        }

        private static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow, int rowIndex)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell;
            //if (rowIndex != 12)
            cell = new Cell() { CellReference = cellReference, DataType = CellValues.String, StyleIndex = 1 };
            //else
            //    cell = new Cell() { CellReference = cellReference, DataType = CellValues.String, StyleIndex = 0 };

            CellValue cellValue = new CellValue();

            cellValue.Text = cellStringValue;
            cell.Append(cellValue);

            excelRow.Append(cell);

        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow, bool isPA)
        {
            Cell cell;
            //  Add a new Excel Cell to our Row 
            if (isPA)
            {
                cell = new Cell() { CellReference = cellReference, StyleIndex = 1 };
            }
            else
            {
                cell = new Cell() { CellReference = cellReference, StyleIndex = 2 };
            }

            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static string GetExcelColumnName(int columnIndex)
        {

            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }
    }
}
