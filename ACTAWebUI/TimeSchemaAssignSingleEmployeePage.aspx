<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeSchemaAssignSingleEmployeePage.aspx.cs" Inherits="ACTAWebUI.TimeSchemaAssignSingleEmployeePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <script language="javascript" type="text/javascript">
        function pagePostBack(btnTree)
        {
            try
            {
                var btn = document.getElementById(btnTree);
                
                if (btn != null)
                {        
                    __doPostBack(btn,'');
                }
            }
            catch(e) { alert(e); }
        }        
    </script>
    <form id="form1" runat="server" defaultbutton="btnSelect">
    <div>
        <asp:Table ID="TableSelection" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell1" runat="server" Width="350px" CssClass="tabCell">
                   <asp:Table ID="Table6" runat="server" Width="350px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow6" runat="server" Width="350px">
                            <asp:TableCell ID="TableCellEmployeePanel" runat="server" Width="350px" CssClass="tabCell">
                                <asp:Table ID="Table4" runat="server" Width="350px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow4" runat="server" Width="350px">
                                        <asp:TableCell ID="TableCell7" runat="server" Width="350px" CssClass="tabCell">
                                            <asp:Label ID="lblEmplData" runat="server" Width="330px" CssClass="contentLblLeft"></asp:Label>                                            
                                        </asp:TableCell>                                        
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow5" runat="server" Width="350px">
                                        <asp:TableCell ID="TableCell8" runat="server" Width="350px" CssClass="tabCell">
                                            <asp:Panel ID="emplDataPanel" runat="server" Width="320px" Height="195px" ScrollBars="Auto" CssClass="resultPanel">
                                                <asp:Table ID="Table3" runat="server" Width="300px" CssClass="tabNoBorderTable">                                            
                                                    <asp:TableRow ID="TableRow48" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell5" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblFirstName" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow13" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell16" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbFirstName" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow140" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell17" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblLastName" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow29" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell30" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbLastName" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                     <asp:TableRow ID="TableRow216" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell176" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblPayroll" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow226" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell1376" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbPayroll" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow30" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell58" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblStringone" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow14" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell59" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbStringone" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowPlantlbl" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell60" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblPlant" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowPlant" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell61" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbPlant" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowCClbl" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell62" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblCostCentar" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowCC" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell63" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbCostCentar" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWGlbl" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell64" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblWorkgroup" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWG" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell65" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbWorkgroup" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowUTElbl" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell66" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblUTE" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowUTE" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell67" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbUTE" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowBranchtlbl" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell68" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblBranch" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowBranch" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell69" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbBranch" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWUlbl" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell31" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblWUnit" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>                                                        
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWU" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell100" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbWUnit" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow46" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell70" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:Label ID="lblOUnit" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow47" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell71" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbOUnit" runat="server" Width="280px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow300" runat="server" Width="300px">
                                                        <asp:TableCell ID="TableCell33" runat="server" Width="300px" CssClass="tabCell">
                                                            <asp:PlaceHolder ID="ascoCtrlHolder" runat="server"></asp:PlaceHolder>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </asp:Panel>
                                        </asp:TableCell>                                        
                                    </asp:TableRow>                                                      
                                </asp:Table> 
                             </asp:TableCell>
                           </asp:TableRow>
                        <asp:TableRow ID="TableRow3" runat="server" Width="350px">
                            <asp:TableCell ID="TableCell2" runat="server" Width="350px" CssClass="hdrCell">
                                <asp:Label ID="lblTimeSchema" runat="server" Width="110px" CssClass="contentLblLeft"></asp:Label>   
                                <asp:DropDownList ID="cbTimeSchema" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="cbTimeSchema_SelectedIndexChanged" CssClass="contentDDList"></asp:DropDownList> 
                            </asp:TableCell> 
                             </asp:TableRow>                           
                         <asp:TableRow ID="TableRow2" runat="server" Width="350px">
                            <asp:TableCell ID="TableCell3" runat="server" Width="350px" CssClass="tabCell">
                                <asp:Table ID="Table10" runat="server" Width="350px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow20" runat="server" Width="350px">
                                        <asp:TableCell ID="TableCell49" runat="server" Width="350px" CssClass="tabCell">
                                            <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px" marginwidth="0px" height="250px" class="pageIframe"></iframe>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow19" runat="server" Width="350px">
                                        <asp:TableCell ID="TableCell29" runat="server" Width="350px" CssClass="tabCell">
                                            <asp:Label ID="lblError" runat="server" Width="330px" CssClass="errorText"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                 </asp:Table>
                            </asp:TableCell>                                                  
                        </asp:TableRow>
                   </asp:Table>
                </asp:TableCell>                 
                <asp:TableCell ID="TableCell6" runat="server" Width="800px" CssClass="tabCell">
                    <asp:Table ID="Table2" runat="server" Width="800px" CssClass="tabTable">     
                        <asp:TableRow ID="TableRow9" runat="server" Width="800px">
                            <asp:TableCell ID="TableCell9" runat="server" Width="800px" CssClass="tabCell">
                                <asp:Table ID="Table5" runat="server" Width="790px" CssClass="tabNoBorderTable">
                                    <asp:TableRow ID="TableRow7" runat="server" Width="790px">  
                                        <asp:TableCell ID="TableCell13" runat="server" Width="190px" CssClass="tabCell">
                                            <asp:Table ID="Table8" runat="server" Width="190px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow8" runat="server" Width="190px">
                                                    <asp:TableCell ID="TableCell45" runat="server" Width="120px" CssClass="tabCell">
                                                        <asp:Label ID="lblPeriod" runat="server" Width="110px" CssClass="contentLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell46" runat="server" Width="70px" CssClass="tabCell"></asp:TableCell>                                                    
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow31" runat="server" Width="190px">
                                                    <asp:TableCell ID="TableCell48" runat="server" Width="120px" CssClass="tabCell">
                                                        <asp:DropDownList ID="cbMonths" runat="server" Width="110px" AutoPostBack="true" OnTextChanged ="dateSelection_TextChanged" CssClass="contentLblLeft">
                                                        </asp:DropDownList>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell57" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbYear" runat="server" Width="55px" AutoPostBack="true" OnTextChanged ="dateSelection_TextChanged" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>                          
                                        <asp:TableCell ID="TableCell4" runat="server" Width="350px" CssClass="tabCell">
                                            <asp:Table ID="selectionTable" runat="server" Width="350px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow11" runat="server" Width="350px">                            
                                                    <asp:TableCell ID="TableCell11" runat="server" Width="350px" CssClass="tabCell">
                                                         <asp:Label ID="lblSelection" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>  
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                 <asp:TableRow ID="TableRow10" runat="server" Width="360px">
                                                      <asp:TableCell ID="TableCell10" runat="server" Width="360px" CssClass="tabCell">
                                                          <asp:Table ID="Table9" runat="server" Width="350px" CssClass="tabNoBorderTable">
                                                                <asp:TableRow ID="TableRow16" runat="server" Width="350px">                            
                                                                    <asp:TableCell ID="TableCell20" runat="server" Width="35px" CssClass="tabCell">
                                                                        <asp:Label ID="lblFrom" runat="server" Width="30px" CssClass="contentLblLeft"></asp:Label>                                                                                 
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell32" runat="server" Width="75px" CssClass="tabCell">
                                                                        <asp:TextBox ID="tbFrom" runat="server" Width="70px" CssClass="contentTb"></asp:TextBox>
                                                                    </asp:TableCell>                                                    
                                                                    <asp:TableCell ID="TableCell44" runat="server" Width="30px" CssClass="tabCell">
                                                                        <asp:ImageButton ID="btnFromDate" runat="server" Visible="false" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"
                                                                            CssClass="contentImgBtn"></asp:ImageButton>
                                                                            <asp:Calendar ID="calendarFrom" runat="server" OnSelectionChanged="DataFromChange" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
                                                                <TodayDayStyle CssClass="calendarToday" />
			                                                    <SelectedDayStyle CssClass="calendarSelector" />
			                                                    <SelectorStyle CssClass="calendarSelector" />
			                                                    <DayHeaderStyle CssClass="calendarDayHdr" />
			                                                    <DayStyle CssClass="calendarDay" />			
			                                                    <TitleStyle CssClass="calendarTitle" />
			                                                    <OtherMonthDayStyle CssClass="calendarOtherMonth" />
			                                                    <NextPrevStyle CssClass="calendarNextPrev" />
			                                                    <WeekendDayStyle CssClass="calendarWeekend" />
                                                            </asp:Calendar>
                                                            <asp:ImageButton ID="btnFrom" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"  runat="server" OnClick="ShowCalendarFrom" />
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell12" runat="server" Width="35px" CssClass="tabCell">
                                                                        <asp:Label ID="lblTo" runat="server" Width="30px"  CssClass="contentLblLeft"></asp:Label>  
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell22" runat="server" Width="75px" CssClass="tabCell">
                                                                        <asp:TextBox ID="tbTo" runat="server" Width="70px" CssClass="contentTb"></asp:TextBox>
                                                                    </asp:TableCell>                                                    
                                                                    <asp:TableCell ID="TableCell23" runat="server" Width="30px" CssClass="tabCell">
                                                                        <asp:ImageButton ID="btnToDate" runat="server" Visible="false" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"
                                                                            CssClass="contentImgBtn"></asp:ImageButton>
                                                                            <asp:Calendar ID="calendarTo" runat="server" OnSelectionChanged="DataToChange" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
                                                                <TodayDayStyle CssClass="calendarToday" />
			                                                    <SelectedDayStyle CssClass="calendarSelector" />
			                                                    <SelectorStyle CssClass="calendarSelector" />
			                                                    <DayHeaderStyle CssClass="calendarDayHdr" />
			                                                    <DayStyle CssClass="calendarDay" />			
			                                                    <TitleStyle CssClass="calendarTitle" />
			                                                    <OtherMonthDayStyle CssClass="calendarOtherMonth" />
			                                                    <NextPrevStyle CssClass="calendarNextPrev" />
			                                                    <WeekendDayStyle CssClass="calendarWeekend" />
                                                            </asp:Calendar>
                                                            <asp:ImageButton ID="btnTo" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"  runat="server" OnClick="ShowCalendarTo" />
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell24" runat="server" Width="70px" CssClass="tabCell">
                                                                        <asp:Button ID="btnSelect" runat="server" OnClick="btnSelect_Click" CssClass="contentBtn"></asp:Button>                                    
                                                                    </asp:TableCell>
                                                              </asp:TableRow> 
                                                         </asp:Table>                                       
                                                      </asp:TableCell>
                                                  </asp:TableRow> 
                                             </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell15" runat="server" Width="240px" CssClass="tabCell"></asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>                        
                        </asp:TableRow>                               
                        <asp:TableRow ID="TableRow35" runat="server" Width="800px">
                           <asp:TableCell ID="TableCell25" runat="server" Width="800px" CssClass="tabCell">
                               <asp:Label ID="Label1" runat="server" Width="10px" CssClass="contentLblLeft"></asp:Label> 
                               <asp:Label ID="Mon" runat="server" Width="100px" CssClass="contentLblCenterBackBlue"></asp:Label> 
                               <asp:Label ID="Label2" runat="server" Width="10px" CssClass="contentLblLeft"></asp:Label>  
                               <asp:Label ID="Tue" runat="server" Width="100px" CssClass="contentLblCenterBackBlue"></asp:Label> 
                               <asp:Label ID="Label3" runat="server" Width="10px" CssClass="contentLblLeft"></asp:Label> 
                               <asp:Label ID="Wed" runat="server" Width="100px" CssClass="contentLblCenterBackBlue"></asp:Label>
                               <asp:Label ID="Label4" runat="server" Width="10px" CssClass="contentLblLeft"></asp:Label>    
                               <asp:Label ID="Thu" runat="server" Width="100px" CssClass="contentLblCenterBackBlue"></asp:Label>
                               <asp:Label ID="Label5" runat="server" Width="10px" CssClass="contentLblLeft"></asp:Label> 
                               <asp:Label ID="Fri" runat="server" Width="100px" CssClass="contentLblCenterBackBlue"></asp:Label>
                               <asp:Label ID="Label6" runat="server" Width="10px" CssClass="contentLblLeft"></asp:Label> 
                               <asp:Label ID="Sat" runat="server" Width="100px" CssClass="contentLblCenterBackBlue"></asp:Label>
                               <asp:Label ID="Label7" runat="server" Width="10px" CssClass="contentLblLeft"></asp:Label> 
                               <asp:Label ID="Sun" runat="server" Width="100px" CssClass="contentLblCenterBackBlue"></asp:Label>
                            </asp:TableCell>
                         </asp:TableRow>
                         <asp:TableRow ID="TableRow36" runat="server" Width="800px">
                               <asp:TableCell ID="TableCell50" runat="server" Width="800px" CssClass="tabCell">
                                   <asp:Panel ID="resultPanel" runat="server" Width="790px" Height="400px" ScrollBars="Auto" CssClass="resultPanel">
                                       <asp:PlaceHolder ID="ctrlHolder" runat="server"></asp:PlaceHolder>
                                   </asp:Panel>
                               </asp:TableCell>
                          </asp:TableRow>
                         <asp:TableRow ID="TableRow15" runat="server" Width="800px">
                            <asp:TableCell ID="TableCell36" runat="server" Width="800px" CssClass="tabCell">
                                <asp:Label ID="lblMessage" runat="server" Width="790px" CssClass="errorText"></asp:Label>
                            </asp:TableCell>                            
                        </asp:TableRow>
                          <asp:TableRow ID="TableRow12" runat="server" Width="800px">
                               <asp:TableCell ID="TableCell14" runat="server" Width="800px" CssClass="tabCell">
                                   <asp:Label ID="errorLabel" runat="server" Width="790px" CssClass="errorText"></asp:Label>                                    
                               </asp:TableCell>
                          </asp:TableRow> 
                          <asp:TableRow ID="TableRow17" runat="server" Width="800px">
                               <asp:TableCell ID="TableCell21" runat="server" Width="800px" CssClass="tabCell">
                                   <asp:Label ID="lblWorkingDay" runat="server" Width="150px" CssClass="contentLblRight"></asp:Label>
                                   <asp:Label ID="lblWorkingDayColor" runat="server" Width="30px" Height="20px" CssClass="contentCalendarWorkingDayLbl"></asp:Label>
                                   <asp:Label ID="lblWeekend" runat="server" Width="150px" CssClass="contentLblRight"></asp:Label>
                                   <asp:Label ID="lblWeekendColor" runat="server" Width="30px" Height="20px" CssClass="contentCalendarWeekendLbl"></asp:Label> 
                                   <asp:Label ID="lblNationalHoliday" runat="server" Width="150px" CssClass="contentLblRight"></asp:Label>
                                   <asp:Label ID="lblNationalHolidayColor" runat="server" Width="30px" Height="20px" CssClass="contentCalendarNationalHolidayLbl"></asp:Label>
                                   <asp:Label ID="lblPersonalHoliday" runat="server" Width="150px" CssClass="contentLblRight"></asp:Label>
                                   <asp:Label ID="lblPersonalHolidayColor" runat="server" Width="30px" Height="20px" CssClass="contentCalendarPersonalHolidayLbl"></asp:Label>                                   
                               </asp:TableCell>
                          </asp:TableRow>                           
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>         
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow18" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell151" runat="server" Width="350px" HorizontalAlign="Center" CssClass="tabLeftAlignCell">
                    <asp:Table ID="Table1" runat="server" Width="350px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow21" runat="server" Width="350px">
                            <asp:TableCell ID="TableCell27" runat="server" Width="350px" HorizontalAlign="Center"
                                CssClass="tabMidAlignCell">
                                <asp:Button ID="btnAssign" runat="server" OnClick="btnAssign_Click" CssClass="contentBtn"></asp:Button>
                                <input ID="SelBox" type="hidden" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow> 
                        
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell28" runat="server" Width="800px" CssClass="tabCell">
                    <asp:Table ID="Table14" runat="server" Width="800px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow25" runat="server" Width="800px">                           
                           <asp:TableCell ID="TableCell26" runat="server" Width="250px" CssClass="tabCell">                               
                               <asp:CheckBox ID="chbCheckLaborLaw" runat="server" Width="240px" Checked="true" CssClass="contentChb" ></asp:CheckBox>
                           </asp:TableCell>
                           <asp:TableCell ID="TableCell18" runat="server" Width="350px" HorizontalAlign="Left" CssClass="tabCell">
                               <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="contentBtn"></asp:Button>                               
                           </asp:TableCell>
                           <asp:TableCell ID="TableCell34" runat="server" Width="250px" CssClass="tabCell">                               
                               <asp:CheckBox ID="CheckBox1" runat="server" Width="240px" Checked="false" CssClass="contentChb" ></asp:CheckBox>
                           </asp:TableCell>
                           <asp:TableCell ID="TableCell35" runat="server" Width="350px" HorizontalAlign="Left" CssClass="tabCell">
                               <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" CssClass="contentBtn"></asp:Button>                               
                           </asp:TableCell>
                           <asp:TableCell ID="TableCell19" runat="server" Width="200px" HorizontalAlign="Right" CssClass="tabCell">
                               <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="contentBtn"></asp:Button>                                    
                           </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
