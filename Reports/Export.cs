using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Threading;

namespace Reports
{
	# region Summary

	/// <summary>
	/// Exports datatable to CSV or Excel format.
	/// This uses DataSet's XML features and XSLT for exporting.
	/// 
	/// C#.Net Example to be used in WebForms
	/// ------------------------------------- 
	/// using MyLib.ExportData;
	/// 
	/// private void btnExport_Click(object sender, System.EventArgs e)
	/// {
	///   try
	///   {
	///     // Declarations
	///     DataSet dsUsers =  ((DataSet) Session["dsUsers"]).Copy( );
	///     MyLib.ExportData.Export oExport = new MyLib.ExportData.Export("Web"); 
	///     string FileName = "UserList.csv";
	///     int[] ColList = {2, 3, 4, 5, 6};
	///     oExport.ExportDetails(dsUsers.Tables[0], ColList, Export.ExportFormat.CSV, FileName);
	///   }
	///   catch(Exception Ex)
	///   {
	///     lblError.Text = Ex.Message;
	///   }
	/// }	
	///  
	/// VB.Net Example to be used in WindowsForms
	/// ----------------------------------------- 
	/// Imports MyLib.ExportData
	/// 
	/// Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	/// 
	///	  Try	
	///	  
	///     'Declarations
	/// 	Dim dsUsers As DataSet = (CType(Session("dsUsers"), DataSet)).Copy()
	/// 	Dim oExport As New MyLib.ExportData.Export("Win")
	/// 	Dim FileName As String = "C:\\UserList.xls"
	/// 	Dim ColList() As Integer = New Integer() {2, 3, 4, 5, 6}			
	///     oExport.ExportDetails(dsUsers.Tables(0), ColList, Export.ExportFormat.CSV, FileName)	 
	///     
	///   Catch Ex As Exception
	/// 	lblError.Text = Ex.Message
	///   End Try
	///   
	/// End Sub
	///     
	/// </summary>

	# endregion // Summary

	public class Export
	{		
		public enum ExportFormat : int {CSV = 1, Excel = 2}; // Export format enumeration			
		
		private string appType;	

		private string[] headersNames = null; //19.10.2006, Bilja, da prosledim i imena kolona		 		 		 		 
			
		public Export()
		{
			appType = "Web";
			//response = System.Web..Current.Response;
		}

		//19.10.2006, Bilja, da prosledim i imena kolona
		public Export(string[] headersNames)
		{
			appType = "Web";
			//response = System.Web.HttpContext.Current.Response;
			this.headersNames = headersNames;
		}

		public Export(string ApplicationType)
		{
			appType = ApplicationType;
			if(appType != "Web" && appType != "Win") throw new Exception("Provide valid application format (Web/Win)");
			//if (appType == "Web") response = System.Web.HttpContext.Current.Response;
		}

		//19.10.2006, Bilja, da prosledim i imena kolona
		public Export(string ApplicationType, string[] headersNames)
		{
			appType = ApplicationType;
			if(appType != "Web" && appType != "Win") throw new Exception("Provide valid application format (Web/Win)");
			//if (appType == "Web") response = System.Web.HttpContext.Current.Response;
			this.headersNames = headersNames;
		}
		
		#region ExportDetails OverLoad : Type#1
		
		// Function  : ExportDetails 
		// Arguments : DetailsTable, FormatType, FileName
		// Purpose	 : To get all the column headers in the datatable and 
		//			   exorts in CSV / Excel format with all columns

		public void ExportDetails(DataTable DetailsTable, ExportFormat FormatType, string FileName)
		{
			//DetailsTable = formatTableDateFormat(DetailsTable);
			try
			{				
				if(DetailsTable.Rows.Count == 0) 
					throw new Exception("There are no details to export.");				
					//Page.RegisterStartupScript("nodetails", "<script language=javascript>"
						//+ "alert(\"There are no details to export. \"); "
						//+ "</script>");
				// Create Dataset

				DataSet dsExport = new DataSet("Export");
				int [] ColumnList = new int[DetailsTable.Columns.Count];
				for(int i = 0; i < ColumnList.Length ;i++)
				{
					ColumnList[i] = i;
				}
				DataTable dtExport = DetailsTable.Copy();//makeFormattedTable(DetailsTable, ColumnList) ; //
				dtExport.TableName = "Values"; 
				dsExport.Tables.Add(dtExport);					
				
				// Getting Field Names
				string[] sHeaders;				
				string[] sFileds = new string[dtExport.Columns.Count];	
			
				//19.10.2006, Bilja, da kreiram i imena kolona ako nisu prosledjena u konstruktoru
				bool createNewHeaders = (headersNames == null)? true : false;
				if (!createNewHeaders)
					sHeaders = headersNames;
				else
					sHeaders = new string[dtExport.Columns.Count];
				
				for (int i=0; i < dtExport.Columns.Count; i++)
				{
					//19.10.2006, Bilja, da kreiram i imena kolona SAMO ako nisu prosledjena u konstruktoru
					if (createNewHeaders)
						sHeaders[i] = dtExport.Columns[i].ColumnName;
					sFileds[i] = dtExport.Columns[i].ColumnName;					
				}

				if(appType == "Web")
					Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
				else if(appType == "Win")
					Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
			}			
			catch(Exception Ex)
			{
				throw Ex;
			}			
		}

        //has additional argument delimiter
        public void ExportDetails(DataTable DetailsTable, ExportFormat FormatType, string FileName, string delimiter)
        {
            //DetailsTable = formatTableDateFormat(DetailsTable);
            try
            {
                if (DetailsTable.Rows.Count == 0)
                    throw new Exception("There are no details to export.");
                //Page.RegisterStartupScript("nodetails", "<script language=javascript>"
                //+ "alert(\"There are no details to export. \"); "
                //+ "</script>");
                // Create Dataset

                DataSet dsExport = new DataSet("Export");
                int[] ColumnList = new int[DetailsTable.Columns.Count];
                for (int i = 0; i < ColumnList.Length; i++)
                {
                    ColumnList[i] = i;
                }
                DataTable dtExport = DetailsTable.Copy();//makeFormattedTable(DetailsTable, ColumnList) ; //
                dtExport.TableName = "Values";
                dsExport.Tables.Add(dtExport);

                // Getting Field Names
                string[] sHeaders;
                string[] sFileds = new string[dtExport.Columns.Count];

                //19.10.2006, Bilja, da kreiram i imena kolona ako nisu prosledjena u konstruktoru
                bool createNewHeaders = (headersNames == null) ? true : false;
                if (!createNewHeaders)
                    sHeaders = headersNames;
                else
                    sHeaders = new string[dtExport.Columns.Count];

                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    //19.10.2006, Bilja, da kreiram i imena kolona SAMO ako nisu prosledjena u konstruktoru
                    if (createNewHeaders)
                        sHeaders[i] = dtExport.Columns[i].ColumnName;
                    sFileds[i] = dtExport.Columns[i].ColumnName;
                }

                if (appType == "Web")
                    Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
                else if (appType == "Win")
                    Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName, delimiter);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

		public void ExportDetails(DataView DetailsView, string d, ExportFormat FormatType, string FileName)
		{
			//DetailsTable = formatTableDateFormat(DetailsTable);
			try
			{				
				if(DetailsView.Count == 0) 
					throw new Exception("There are no details to export.");				
				//Page.RegisterStartupScript("nodetails", "<script language=javascript>"
				//+ "alert(\"There are no details to export. \"); "
				//+ "</script>");
				// Create Dataset

				DataSet dsExport = new DataSet("Export");
				DataTable dtExport = DetailsView.Table.Copy();
				dtExport.TableName = "Values"; 
				dsExport.Tables.Add(dtExport);
				//dsExport.Tables["Values"].DefaultView.RowFilter = d;	
				//int u = dtExport.Rows.Count;

				// Getting Field Names
				string[] sHeaders = new string[dtExport.Columns.Count];
				string[] sFileds = new string[dtExport.Columns.Count];
				
				for (int i=0; i < dtExport.Columns.Count; i++)
				{
					sHeaders[i] = dtExport.Columns[i].ColumnName;
					sFileds[i] = dtExport.Columns[i].ColumnName;					
				}

				if(appType == "Web")
					Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
				else if(appType == "Win")
					Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
			}			
			catch(Exception Ex)
			{
				throw Ex;
			}			
		}


		#endregion // ExportDetails OverLoad : Type#1

		#region ExportDetails OverLoad : Type#2

		// Function  : ExportDetails 
		// Arguments : DetailsTable, ColumnList, FormatType, FileName		
		// Purpose	 : To get the specified column headers in the datatable and
		//			   exorts in CSV / Excel format with specified columns

		public void ExportDetails(DataTable DetailsTable, int[] ColumnList, ExportFormat FormatType, string FileName)
		{
			try
			{
				if(DetailsTable.Rows.Count == 0)
					throw new Exception("There are no details to export");
				
				// Create Dataset
				DataSet dsExport = new DataSet("Export");
				DataTable dtExport = DetailsTable.Copy();
				dtExport.TableName = "Values"; 
				dsExport.Tables.Add(dtExport);

				if(ColumnList.Length > dtExport.Columns.Count)
					throw new Exception("ExportColumn List should not exceed Total Columns");
				
				// Getting Field Names
				string[] sHeaders = new string[ColumnList.Length];
				string[] sFileds = new string[ColumnList.Length];
				
				for (int i=0; i < ColumnList.Length; i++)
				{
					if((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
						throw new Exception("ExportColumn Number should not exceed Total Columns Range");
					
					sHeaders[i] = dtExport.Columns[ColumnList[i]].ColumnName;
					sFileds[i] = dtExport.Columns[ColumnList[i]].ColumnName;					
				}

				if(appType == "Web")
					Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
				else if(appType == "Win")
					Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
			}			
			catch(Exception Ex)
			{
				throw Ex;
			}			
		}

		public void ExportDetails(DataView DetailsDataView, int[] ColumnList, ExportFormat FormatType, string FileName)
		{
//			this.ColumnList = ColumnList;
//			oldListColumns = getDataColumns(ColumnList,DetailsTable);
//			DetailsTable = formatTableDateFormat(DetailsTable);		
			

			try
			{
				if(DetailsDataView.Count == 0)
					throw new Exception("There are no details to export");
				
				// Create Dataset
				DataSet dsExport = new DataSet("Export");
				DataTable dtExport = makeFormattedTable(DetailsDataView, ColumnList);     ;//DetailsTable.Copy();
				//DataTable dtExport = DetailsDataView.Table;
				dtExport.TableName = "Values"; 
				dsExport.Tables.Add(dtExport);

				if(ColumnList.Length > dtExport.Columns.Count)
					throw new Exception("ExportColumn List should not exceed Total Columns");
				
				// Getting Field Names
				string[] sHeaders = new string[ColumnList.Length];
				string[] sFileds = new string[ColumnList.Length];
				
				for (int i=0; i < ColumnList.Length; i++)
				{
					if((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
						throw new Exception("ExportColumn Number should not exceed Total Columns Range");
					
					sHeaders[i] = dtExport.Columns[ColumnList[i]].ColumnName;
					sFileds[i] = dtExport.Columns[ColumnList[i]].ColumnName;					
				}

				if(appType == "Web")
					Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
				else if(appType == "Win")
					Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
			}			
			catch(Exception Ex)
			{
				throw Ex;
			}			
		}
		
		#endregion // ExportDetails OverLoad : Type#2

		#region ExportDetails OverLoad : Type#3

		// Function  : ExportDetails 
		// Arguments : DetailsTable, ColumnList, Headers, FormatType, FileName	
		// Purpose	 : To get the specified column headers in the datatable and	
		//			   exorts in CSV / Excel format with specified columns and 
		//			   with specified headers

		public void ExportDetails(DataTable DetailsTable, int[] ColumnList, string[] Headers, ExportFormat FormatType, 
			string FileName)
		{
			try
			{
				if(DetailsTable.Rows.Count == 0)
					throw new Exception("There are no details to export");
				
				// Create Dataset
				DataSet dsExport = new DataSet("Export");
				DataTable dtExport = DetailsTable.Copy();
				dtExport.TableName = "Values"; 
				dsExport.Tables.Add(dtExport);

				if(ColumnList.Length != Headers.Length)
					throw new Exception("ExportColumn List and Headers List should be of same length");
				else if(ColumnList.Length > dtExport.Columns.Count || Headers.Length > dtExport.Columns.Count)
					throw new Exception("ExportColumn List should not exceed Total Columns");
				
				// Getting Field Names
				string[] sFileds = new string[ColumnList.Length];
				
				for (int i=0; i < ColumnList.Length; i++)
				{
					if((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
						throw new Exception("ExportColumn Number should not exceed Total Columns Range");
					
					sFileds[i] = dtExport.Columns[ColumnList[i]].ColumnName;					
				}
                
				if(appType == "Web")
					Export_with_XSLT_Web(dsExport, Headers, sFileds, FormatType, FileName);
				else if(appType == "Win")
					Export_with_XSLT_Windows(dsExport, Headers, sFileds, FormatType, FileName);
			}			
			catch(Exception Ex)
			{
				throw Ex;
			}			
		}

		public void ExportDetails(DataView DetailsView, int[] ColumnList, string[] Headers, ExportFormat FormatType, 
			string FileName)
		{
//			this.ColumnList = ColumnList;
//			oldListColumns = getDataColumns(ColumnList,DetailsTable);
//			DetailsTable = formatTableDateFormat(DetailsTable);
			try
			{
				
				if(DetailsView.Count == 0)
					throw new Exception("There are no details to export");
				
				// Create Dataset
				DataSet dsExport = new DataSet("Export");
				 DataTable dtExport = makeFormattedTable(DetailsView,ColumnList) ;//DetailsTable.Copy();
				
				dtExport.TableName = "Values"; 
				dsExport.Tables.Add(dtExport);
				

				if(ColumnList.Length != Headers.Length)
					throw new Exception("ExportColumn List and Headers List should be of same length");
				else if(ColumnList.Length > dtExport.Columns.Count || Headers.Length > dtExport.Columns.Count)
					throw new Exception("ExportColumn List should not exceed Total Columns");
				
				// Getting Field Names
				string[] sFileds = new string[ColumnList.Length];
				
				for (int i=0; i < ColumnList.Length; i++)
				{
					if((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
						throw new Exception("ExportColumn Number should not exceed Total Columns Range");
					
					sFileds[i] = dtExport.Columns[ColumnList[i]].ColumnName;					
				}

				if(appType == "Web")
					Export_with_XSLT_Web(dsExport, Headers, sFileds, FormatType, FileName);
				else if(appType == "Win")
					Export_with_XSLT_Windows(dsExport, Headers, sFileds, FormatType, FileName);
			}			
			catch(Exception Ex)
			{
				throw Ex;
			}			
		}
		
		#endregion // ExportDetails OverLoad : Type#3

		#region Export_with_XSLT_Web

		// Function  : Export_with_XSLT_Web 
		// Arguments : dsExport, sHeaders, sFileds, FormatType, FileName
		// Purpose   : Exports dataset into CSV / Excel format

		private void Export_with_XSLT_Web(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
		{
			try
			{				
				// Appending Headers
				//response.Clear();
				//response.Buffer= true;
				
				if(FormatType == ExportFormat.CSV)
				{
					//response.ContentType = "text/csv";
					//response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
				}		
				else
				{
					//response.ContentType = "application/vnd.ms-excel";
					//response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
				}

				// XSLT to use for transforming this dataset.						
				MemoryStream stream = new MemoryStream( );
				XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Unicode);
				
				CreateStylesheet(writer, sHeaders, sFileds, FormatType);
				writer.Flush( ); 
				stream.Seek( 0, SeekOrigin.Begin); 

				XmlDataDocument xmlDoc = new XmlDataDocument(dsExport);
				//XslTransform xslTran = new XslTransform();
                XslCompiledTransform xslTran = new XslCompiledTransform();				
                xslTran.Load(new XmlTextReader(stream), null, null);
								
				System.IO.StringWriter  sw = new System.IO.StringWriter();			
				//xslTran.Transform(xmlDoc, null, sw, null);
                xslTran.Transform(xmlDoc,null, sw);

									
				//Writeout the Content				
				//response.Write(sw.ToString());				
				sw.Close();	
				writer.Close();
				stream.Close();			
				//response.End();
			}
			catch(ThreadAbortException Ex)
			{
				string ErrMsg = Ex.Message;
			}
			catch(Exception Ex)
			{
				throw Ex;
			}
		}		
		
		#endregion // Export_with_XSLT 

		#region Export_with_XSLT_Windows 

		// Function  : Export_with_XSLT_Windows 
		// Arguments : dsExport, sHeaders, sFileds, FormatType, FileName
		// Purpose   : Exports dataset into CSV / Excel format

		private void Export_with_XSLT_Windows(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
		{
			try
			{				
				// XSLT to use for transforming this dataset.						
				MemoryStream stream = new MemoryStream();
				XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Unicode);
				
				CreateStylesheet(writer, sHeaders, sFileds, FormatType);
				writer.Flush( ); 
				stream.Seek( 0, SeekOrigin.Begin); 

				/*
				FileStream outStream = File.OpenWrite("C:\\output.txt");
				stream.WriteTo(outStream);
				outStream.Flush();
				outStream.Close();
                */ 

				XmlDataDocument xmlDoc = new XmlDataDocument(dsExport);
				//XslTransform xslTran = new XslTransform();	
                XslCompiledTransform xslTran = new XslCompiledTransform();	
				try
				{
					XmlTextReader xtReader = new XmlTextReader(stream);
					
					xslTran.Load(xtReader, null, null); //this.GetType().Assembly.Evidence
				}			
				catch(XsltCompileException ex)
				{
					throw ex;
				}

				
								
				System.IO.StringWriter  sw = new System.IO.StringWriter();			
				//xslTran.Transform(xmlDoc, null, sw, null);
                xslTran.Transform(xmlDoc, null, sw);
                		
				//Writeout the Content									
				StreamWriter strwriter =  new StreamWriter(FileName,false,Encoding.Unicode);
				strwriter.WriteLine(sw.ToString());
				strwriter.Close();
				
				sw.Close();	
				writer.Close();
				stream.Close();	
			}			
			catch(Exception Ex)
			{
				throw Ex;
			}
		}

        //has a additional argument delimiter
        private void Export_with_XSLT_Windows(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName, string delimiter)
        {
            try
            {
                // XSLT to use for transforming this dataset.						
                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Unicode);

                CreateStylesheet(writer, sHeaders, sFileds, FormatType, delimiter);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                /*
                FileStream outStream = File.OpenWrite("C:\\output.txt");
                stream.WriteTo(outStream);
                outStream.Flush();
                outStream.Close();
                */

                XmlDataDocument xmlDoc = new XmlDataDocument(dsExport);
                //XslTransform xslTran = new XslTransform();	
                XslCompiledTransform xslTran = new XslCompiledTransform();
                try
                {
                    XmlTextReader xtReader = new XmlTextReader(stream);

                    xslTran.Load(xtReader, null, null); //this.GetType().Assembly.Evidence
                }
                catch (XsltCompileException ex)
                {
                    throw ex;
                }



                System.IO.StringWriter sw = new System.IO.StringWriter();
                //xslTran.Transform(xmlDoc, null, sw, null);
                xslTran.Transform(xmlDoc, null, sw);

                //Writeout the Content									
                StreamWriter strwriter = new StreamWriter(FileName, false, Encoding.Unicode);
                strwriter.WriteLine(sw.ToString());
                strwriter.Close();

                sw.Close();
                writer.Close();
                stream.Close();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }		

		#endregion // Export_with_XSLT 

		#region CreateStylesheet 

		// Function  : WriteStylesheet 
		// Arguments : writer, sHeaders, sFileds, FormatType
		// Purpose   : Creates XSLT file to apply on dataset's XML file 

		private void CreateStylesheet(XmlTextWriter writer, string[] sHeaders, string[] sFileds, ExportFormat FormatType)
		{
			try
			{
				// xsl:stylesheet
				string ns = "http://www.w3.org/1999/XSL/Transform";	
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument();				
				writer.WriteStartElement("xsl","stylesheet",ns);
				writer.WriteAttributeString("version","1.0");
				writer.WriteStartElement("xsl:output");
				writer.WriteAttributeString("method","text");
				writer.WriteAttributeString("version","4.0");
				writer.WriteEndElement();
				
				// xsl-template
				writer.WriteStartElement("xsl:template");
				writer.WriteAttributeString("match","/");

				// xsl:value-of for headers
				for(int i=0; i< sHeaders.Length; i++)
				{
					writer.WriteString("\"");
					writer.WriteStartElement("xsl:value-of");
					writer.WriteAttributeString("select", "'" + sHeaders[i] + "'");
					writer.WriteEndElement(); // xsl:value-of
					writer.WriteString("\"");
					if (i != sFileds.Length - 1) writer.WriteString( (FormatType == ExportFormat.CSV ) ? "," : "	" );
				}
								
				// xsl:for-each
				writer.WriteStartElement("xsl:for-each");
				writer.WriteAttributeString("select","Export/Values");
				writer.WriteString("\r\n");				
				
				// xsl:value-of for data fields
				for(int i=0; i< sFileds.Length; i++)
				{					
					writer.WriteString("\"");
					writer.WriteStartElement("xsl:value-of");
					writer.WriteAttributeString("select", sFileds[i]);
					writer.WriteEndElement(); // xsl:value-of
					writer.WriteString("\"");
					if (i != sFileds.Length - 1) writer.WriteString( (FormatType == ExportFormat.CSV ) ? "," : "	" );
				}
								
				writer.WriteEndElement(); // xsl:for-each
				writer.WriteEndElement(); // xsl-template
				writer.WriteEndElement(); // xsl:stylesheet
				writer.WriteEndDocument();					
			}
			catch(Exception Ex)
			{
				throw Ex;
			}
		}

        //has a additional argument delimiter
        private void CreateStylesheet(XmlTextWriter writer, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string delimiter)
        {
            try
            {
                // xsl:stylesheet
                string ns = "http://www.w3.org/1999/XSL/Transform";
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("xsl", "stylesheet", ns);
                writer.WriteAttributeString("version", "1.0");
                writer.WriteStartElement("xsl:output");
                writer.WriteAttributeString("method", "text");
                writer.WriteAttributeString("version", "4.0");
                writer.WriteEndElement();

                // xsl-template
                writer.WriteStartElement("xsl:template");
                writer.WriteAttributeString("match", "/");

                // xsl:value-of for headers
                for (int i = 0; i < sHeaders.Length; i++)
                {
                    writer.WriteString("\"");
                    writer.WriteStartElement("xsl:value-of");
                    writer.WriteAttributeString("select", "'" + sHeaders[i] + "'");
                    writer.WriteEndElement(); // xsl:value-of
                    writer.WriteString("\"");
                    if (i != sFileds.Length - 1) writer.WriteString((FormatType == ExportFormat.CSV) ? delimiter : "	");
                }

                // xsl:for-each
                writer.WriteStartElement("xsl:for-each");
                writer.WriteAttributeString("select", "Export/Values");
                writer.WriteString("\r\n");

                // xsl:value-of for data fields
                for (int i = 0; i < sFileds.Length; i++)
                {
                    writer.WriteString("\"");
                    writer.WriteStartElement("xsl:value-of");
                    writer.WriteAttributeString("select", sFileds[i]);
                    writer.WriteEndElement(); // xsl:value-of
                    writer.WriteString("\"");
                    if (i != sFileds.Length - 1) writer.WriteString((FormatType == ExportFormat.CSV) ? delimiter : "	");
                }

                writer.WriteEndElement(); // xsl:for-each
                writer.WriteEndElement(); // xsl-template
                writer.WriteEndElement(); // xsl:stylesheet
                writer.WriteEndDocument();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

		#endregion // WriteStylesheet

//		private DataTable formatTableDateFormat(DataTable table)
//		{
//			try
//			{
//				string[] names = getDateColumnNames(table);
//				if(names == null) return table;
//				if(names.Length == 0 ) return table;
//				newDataColumns = new DataColumn[names.Length];
//				oldDataColumns = new DataColumn[names.Length];
//				
//			
//				for(int i = 0 ;i < names.Length ; i++ )
//				{
//					oldDataColumns[i] = table.Columns[names[i]];
//					newDataColumns[i] = new DataColumn(names[i]+"_EX",typeof(string));					 
//					table.Columns.Add(newDataColumns[i]);
//				}
//				table.AcceptChanges();
//		 
//				foreach(DataRow row in table.Rows)
//				{
//					row.BeginEdit();
//					for(int i = 0;i < newDataColumns.Length;i++)
//					{
//						if(row[oldDataColumns[i]] is DBNull ) row[newDataColumns[i]] = DBNull.Value;
//						else row[newDataColumns[i]] = ((DateTime)row[oldDataColumns[i]]).ToString("dd/MM/yyyy"); 
//					}				 
//				}
//				foreach(DataColumn column in oldDataColumns)
//				{
//					table.Columns.Remove(column);
//				}
//				table.AcceptChanges();
//				return table; 
//			}
//			catch(Exception ex)
//			{
//				string message = ex.Message;
//				return null;
//			}
//		}
//
//		private void removeFormattedColumns()
//		{
//			if(this.ColumnList != null)
//			{
//
//			}
//		}

		private DataColumn[] getDataColumns(int[] columnList,DataTable table)
		{
			DataColumn[] columns = new DataColumn[columnList.Length];
			for(int i =0;i< columnList.Length ;i++)
			{
				columns[i] = table.Columns[columnList[i]];
			}
			return columns;			 
		}

//		private void changeColumnList(DataColumn[] oldColumns,DataColumn[] newColumns,DataTable table)
//		{
//			for(int i= 0; i < oldColumns.Length ;i++)
//			{
//				int index = table.Columns..IndexOf(oldColumns); 
//				for(int j=0;j<this.ColumnList.Lengthl;j++)
//				{
//					if
//				}
//			}
//		}

		private string[] getDateColumnNames(DataTable table)
		{
			string names = null;
			foreach(DataColumn column in table.Columns)
			{
				if(column.DataType.FullName.ToString() =="System.DateTime")
					names += ","+column.ColumnName;
			}
			if(names != null)
			{
				names = names.Remove(0,1);
				return names.Split(new char[]{','});
			}
			else return null;			 
		}

		private DataTable makeFormattedTable(DataView dataView , int[] columnList)
		{
			try
			{
				DataColumn[] oldColumns = getDataColumns(columnList,dataView.Table);
				DataColumn[] newColumns = getNewColumns(oldColumns);
				DataTable newTable = new DataTable(dataView.Table.TableName);
				newTable.Columns.AddRange(newColumns);
				foreach(DataRowView oldRow in dataView)
				{	
					
					DataRow newRow = newTable.NewRow();
					newTable.Rows.Add(newRow);
					newRow.BeginEdit();
					for(int i=0;i< oldColumns.Length;i++)
					{
						if( oldRow.Row[oldColumns[i]].GetType() == typeof(System.DateTime))
						{
							if(!( oldRow.Row[oldColumns[i]] is DBNull))
								newRow[newColumns[i]] = ((DateTime) oldRow.Row[oldColumns[i]]).ToString("MM/dd/yyyy"); 
						}
						else
						{
							newRow[newColumns[i]] = oldRow.Row[oldColumns[i]];
						}
					}
					
					newRow.AcceptChanges();
				}
				newTable.AcceptChanges();
				return newTable;
			}
			catch(Exception ex)
			{
				string message = ex.Message;
				return null;
			}

		}

		private DataTable makeFormattedTable(DataTable dataTable, int[] columnList)
		{
			try
			{
				DataColumn[] oldColumns = getDataColumns(columnList, dataTable);
				DataColumn[] newColumns = getNewColumns(oldColumns);
				DataTable newTable = new DataTable(dataTable.TableName);

				newTable.Columns.AddRange(newColumns);

				for (int rowNum = 0; rowNum < dataTable.Rows.Count; rowNum++)
				{						
					DataRow newRow = newTable.NewRow();
					newTable.Rows.Add(newRow);
					newRow.BeginEdit();
					for(int i = 0; i < oldColumns.Length; i++)
					{
						if( dataTable.Columns[i].DataType == typeof(System.DateTime))
						{
							if (!(dataTable.Rows[rowNum][oldColumns[i]] is DBNull))
								newRow[newColumns[i]] = ((DateTime) dataTable.Rows[rowNum][oldColumns[i]]).ToString("yyyy/MM/dd"); 
						}
						else
						{
							newRow[newColumns[i]] = dataTable.Rows[rowNum][oldColumns[i]];
						}
					}					
					newRow.AcceptChanges();
				}
				newTable.AcceptChanges();
				return newTable;
			}
			catch(Exception ex)
			{
				string message = ex.Message;
				return null;
			}
		}

		private DataColumn[] getNewColumns(DataColumn[] oldColumns)
		{
			DataColumn[] newColumns = new DataColumn[oldColumns.Length];
			for(int i= 0;i<oldColumns.Length;i++)
			{
				switch(oldColumns[i].DataType.FullName.ToString())
				{
					case "System.DateTime":
						newColumns[i] = new DataColumn(oldColumns[i].ColumnName,typeof(string));
						break;
					default:
						newColumns[i] = new DataColumn(oldColumns[i].ColumnName,oldColumns[i].GetType());
						break;
				}
			}
			return newColumns;				
		}
	}
}
