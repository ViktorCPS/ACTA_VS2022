<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplRiskManagementDataPage.aspx.cs" Inherits="ACTAWebUI.EmplRiskManagementDataPage" %>

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
        <asp:Table ID="Table15" runat="server" Width="990px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="990px">
                <asp:TableCell ID="TableCell151" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">                        
                    <asp:Table ID="Table3" runat="server" Width="990px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblEmplData" runat="server" Width="900px" CssClass="hdrTitle"></asp:Label>
                                <asp:Button ID="btnPostBack" runat="server" OnClick="btnPostBack_Click" CssClass="contentInvisibleBtn"></asp:Button>
                            </asp:TableCell>                            
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow14" runat="server" Width="990px">
                <asp:TableCell ID="TableCell26" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                    <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                    marginwidth="0px" height="380px" class="pageIframe"></iframe>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow9" runat="server" Width="990px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow8" runat="server" Width="990px">
                <asp:TableCell ID="TableCell19" runat="server" Width="990px" CssClass="tabMidAlignCell">
                    <asp:Label ID="lblNewEntry" runat="server" Width="980px" CssClass="contentLblLeft"></asp:Label>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server" Width="990px">
                <asp:TableCell ID="TableCell1" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">                        
                    <asp:Table ID="tableSaveUpdate" runat="server" Width="990px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow4" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell2" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Table ID="Table2" runat="server" Width="970px" CssClass="tabNoBorderTable">
                                    <asp:TableRow ID="TableRow7" runat="server" Width="970px">
                                        <asp:TableCell ID="TableCell6" runat="server" Width="85px" CssClass="tabMidAlignCell">
                                            <asp:Label ID="lblFrom" runat="server" Width="70px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>                                                    
                                        <asp:TableCell ID="TableCell10" runat="server" Width="30px" CssClass="tabMidAlignCell"></asp:TableCell>
                                        <asp:TableCell ID="TableCell11" runat="server" Width="85px" CssClass="tabMidAlignCell">
                                            <asp:Label ID="lblTo" runat="server" Width="70px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>                                                    
                                        <asp:TableCell ID="TableCell14" runat="server" Width="30px" CssClass="tabMidAlignCell"></asp:TableCell>
                                        <asp:TableCell ID="TableCell15" runat="server" Width="320px" CssClass="tabMidAlignCell">
                                            <asp:Label ID="lblRisk" runat="server" Width="300px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell16" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                            <asp:Label ID="lblRotation" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell3" runat="server" Width="130px" CssClass="tabMidAlignCell"></asp:TableCell>
                                        <asp:TableCell ID="TableCell17" runat="server" Width="130px" CssClass="tabMidAlignCell"></asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow6" runat="server" Width="990px">
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
                                        <asp:TableCell ID="TableCell4" runat="server" Width="85px" CssClass="tabMidAlignCell">
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
                                        <asp:TableCell ID="TableCell12" runat="server" Width="320px" CssClass="tabMidAlignCell">
                                            <asp:DropDownList ID="cbRisk" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="cbRisk_SelectedIndexChanged" CssClass="contentDDList"></asp:DropDownList>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell13" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                            <asp:DropDownList ID="cbRotation" runat="server" Width="140px" CssClass="contentDDList"></asp:DropDownList>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell7" runat="server" Width="130px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="contentBtn"></asp:Button>                                            
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell8" runat="server" Width="130px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="contentBtn"></asp:Button>                                            
                                            <input ID="SelBox" type="hidden" runat="server" />
                                            <input ID="SelRecord" type="hidden" runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>                                    
                                </asp:Table>                                
                            </asp:TableCell>                            
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow5" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell18" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblError" runat="server" Width="970px" CssClass="errorText"></asp:Label>
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