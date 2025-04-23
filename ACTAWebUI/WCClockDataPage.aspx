<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WCClockDataPage.aspx.cs" Inherits="ACTAWebUI.WCClockDataPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
</head>
<body onload="document.body.style.cursor = 'default'">
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
    <form id="form1" runat="server" defaultbutton="btnShow">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell1" runat="server" Width="150px" CssClass="tabCell">
                    <asp:Table ID="Table6" runat="server" Width="150px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow6" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table4" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow4" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell7" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblEmplData" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>                                            
                                        </asp:TableCell>                                        
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow5" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell8" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Panel ID="emplDataPanel" runat="server" Width="140px" Height="230px" ScrollBars="Auto" CssClass="resultPanel">
                                                <asp:Table ID="Table3" runat="server" Width="120px" CssClass="tabNoBorderTable">                                            
                                                    <asp:TableRow ID="TableRow48" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell5" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblFirstName" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow3" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell10" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbFirstName" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow14" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell16" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblLastName" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow29" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell57" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbLastName" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                     <asp:TableRow ID="TableRow216" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell176" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblPayroll" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow226" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell1376" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbPayroll" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow30" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell58" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblStringone" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow31" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell59" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbStringone" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowPlantlbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell60" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblPlant" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowPlant" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell61" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbPlant" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowCClbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell62" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblCostCentar" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowCC" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell63" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbCostCentar" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWGlbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell64" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblWorkgroup" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWG" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell65" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbWorkgroup" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowUTElbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell66" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblUTE" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowUTE" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell67" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbUTE" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowBranchtlbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell68" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblBranch" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowBranch" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell69" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbBranch" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWUlbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell500" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblWUnit" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>                                                        
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWU" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell100" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbWUnit" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow46" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell70" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblOUnit" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow47" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell71" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbOUnit" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow300" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell160" runat="server" Width="120px" CssClass="tabCell">
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
                        <asp:TableRow ID="TableRow9" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell13" runat="server" Width="150px" CssClass="tabCell">                                
                                <asp:Table ID="Table7" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell9" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblGate" runat="server" Width="110px" CssClass="contentLblLeft"></asp:Label>                                            
                                        </asp:TableCell>                                                
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow12" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell14" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:ListBox ID="lbGates" runat="server" Width="140px" Height="90px" SelectionMode="Multiple" CssClass="contentLb"></asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow21" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell30" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:CheckBox ID="chbGate" runat="server" Width="110px" CssClass="contentChb"></asp:CheckBox>                                            
                                        </asp:TableCell>                                                
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow8" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell3" runat="server" Width="150px" CssClass="tabCell">                                
                                <asp:Table ID="Table2" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow10" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell11" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblPeriod" runat="server" Width="110px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>                                                
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow26" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell31" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table9" runat="server" Width="140px" CssClass="tabNoBorderTable">
                                                <asp:TableRow ID="TableRow27" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell33" runat="server" HorizontalAlign="Center" Width="25px" CssClass="tabCell">
                                                        <asp:ImageButton ID="btnPrevDayPeriod" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/backArrow.png" OnClick="btnPrevDayPeriod_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell34" runat="server" HorizontalAlign="Center" Width="25px" CssClass="tabCell">
                                                        <asp:ImageButton ID="btnNextDayPeriod" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/forwardArrow.png" OnClick="btnNextDayPeriod_Click" CssClass="contentImgBtnWide"></asp:ImageButton>            
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell35" runat="server" Width="90px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow11" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell12" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table5" runat="server" Width="140px" CssClass="tabNoBorderTable">
                                                <asp:TableRow ID="TableRow13" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell15" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:Label ID="lblFrom" runat="server" Width="65px" CssClass="contentLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell17" runat="server" Width="40px" CssClass="tabCell"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell18" runat="server" Width="30px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow15" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell19" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbFromDate" runat="server" Width="65px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell20" runat="server" Width="40px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbFromTime" runat="server" Width="33px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell21" runat="server" Width="30px" CssClass="tabCell">
                                                        <asp:ImageButton ID="btnFromDate" runat="server" Visible="false" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif" CssClass="contentImgBtn"></asp:ImageButton>
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
                                                    
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow16" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell22" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:Label ID="lblTo" runat="server" Width="65px" CssClass="contentLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell23" runat="server" Width="40px" CssClass="tabCell"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell24" runat="server" Width="30px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow17" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell25" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbToDate" runat="server" Width="65px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell26" runat="server" Width="40px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbToTime" runat="server" Width="33px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell27" runat="server" Width="30px" CssClass="tabCell">
                                                        <asp:ImageButton ID="btnToDate" runat="server" Visible="false" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif" CssClass="contentImgBtn"></asp:ImageButton>
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
                                                    
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>                                                
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>                        
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table10" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow20" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell49" runat="server" Width="1000px" CssClass="tabCell">
                                <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px" marginwidth="0px" height="515px" class="pageIframe" ></iframe>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow19" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell29" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Label ID="lblError" runat="server" Width="980px" CssClass="errorText"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>                                        
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell151" runat="server" Width="155px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                    <asp:Table ID="Table8" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow18" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell28" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table14" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow25" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell41" runat="server" Width="500px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnReport" runat="server" OnClick="btnReport_Click" CssClass="contentBtn"></asp:Button>
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
