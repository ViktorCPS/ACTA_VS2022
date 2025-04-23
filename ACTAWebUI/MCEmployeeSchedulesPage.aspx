<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MCEmployeeSchedulesPage.aspx.cs" Inherits="ACTAWebUI.MCEmployeeSchedulesPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>    
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />    
</head>
<body onload="document.body.style.cursor = 'default'">    
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table15" runat="server" Width="980px" HorizontalAlign="Left" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow5" runat="server" Width="980px">
                <asp:TableCell ID="TableCell4" runat="server" Width="980px" CssClass="tabMidAlignCell">
                    <asp:Table ID="hdrTable" runat="server" Width="970px" HorizontalAlign="Left" CssClass="hdrListTable">
                        <asp:TableRow ID="hdrRow" runat="server" Width="970px" CssClass="hdrListRow">
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>  
            <asp:TableRow ID="TableRow1" runat="server" Width="980px">
                <asp:TableCell ID="TableCell1" runat="server" Width="980px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
                    <asp:Table ID="Table1" runat="server" Width="970px" HorizontalAlign="Left" CssClass="tabTable">
                        <asp:TableRow ID="TableRow6" runat="server" Width="970px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="970px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Panel ID="schPanel" runat="server" Width="950px" Height="370px" ScrollBars="Auto" CssClass="resultPanel">
                                    <asp:DataGrid ID="schGrid" runat="server" Width="930px" AutoGenerateColumns="false"
                                        ItemStyle-CssClass="resultItem" AlternatingItemStyle-CssClass="resultAltItem" CssClass="resultGrid">
                                    </asp:DataGrid>
                                </asp:Panel>                    
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>                    
                </asp:TableCell>                
            </asp:TableRow>            
            <asp:TableRow ID="TableRow3" runat="server" Width="980px">
                <asp:TableCell ID="TableCell2" runat="server" Width="980px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
                    <asp:Table ID="searchTable" runat="server" Width="970px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow7" runat="server" Width="970px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="85px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblFrom" runat="server" Width="70px" CssClass="contentLblLeft"></asp:Label>
                            </asp:TableCell>                                                    
                            <asp:TableCell ID="TableCell10" runat="server" Width="30px" CssClass="tabMidAlignCell"></asp:TableCell>
                            <asp:TableCell ID="TableCell11" runat="server" Width="85px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblTo" runat="server" Width="70px" CssClass="contentLblLeft"></asp:Label>
                            </asp:TableCell>                                                    
                            <asp:TableCell ID="TableCell14" runat="server" Width="30px" CssClass="tabMidAlignCell"></asp:TableCell>                            
                            <asp:TableCell ID="TableCell17" runat="server" Width="200px" CssClass="tabMidAlignCell"></asp:TableCell>
                            <asp:TableCell ID="TableCell15" runat="server" Width="540px" CssClass="tabMidAlignCell"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow2" runat="server" Width="970px">
                            <asp:TableCell ID="TableCell32" runat="server" Width="85px" CssClass="tabMidAlignCell">
                                <asp:TextBox ID="tbFromDate" runat="server" Width="70px" CssClass="contentTb"></asp:TextBox>
                            </asp:TableCell>                                                    
                            <asp:TableCell ID="TableCell44" runat="server" Width="30px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
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
                            <asp:TableCell ID="TableCell3" runat="server" Width="85px" CssClass="tabMidAlignCell">
                                <asp:TextBox ID="tbToDate" runat="server" Width="70px" CssClass="contentTb"></asp:TextBox>
                            </asp:TableCell>                                                    
                            <asp:TableCell ID="TableCell9" runat="server" Width="30px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
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
                            <asp:TableCell ID="TableCell7" runat="server" Width="200px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="contentBtn"></asp:Button>                                            
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell8" runat="server" Width="540px" HorizontalAlign="Center" CssClass="tabMidAlignCell"></asp:TableCell>
                        </asp:TableRow>                                    
                    </asp:Table> 
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow4" runat="server" Width="980px">
                <asp:TableCell ID="TableCell18" runat="server" Width="980px" CssClass="tabMidAlignCell">
                    <asp:Label ID="lblError" runat="server" Width="970px" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>  
        </asp:Table>
    </div>
    </form>
</body>
</html>
