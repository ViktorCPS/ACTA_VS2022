<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkAnalyzeReports.aspx.cs"
    Inherits="ACTAWebUI.WorkAnalyzeReports" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ACTA Web</title>

    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>

    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
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
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1180px" CssClass="tabNoBorderTable"
            Height="560px">
            <asp:TableRow ID="TableRow1" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell1" runat="server" Width="160px" CssClass="tabCell">
                    <asp:Table ID="Table6" runat="server" Width="160px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow12" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell14" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Label ID="lblReportType" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow9" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                <asp:RadioButton ID="rbDaily" runat="server" OnCheckedChanged="rbDaily_OnCheckedChanged"
                                    CssClass="contentRb" AutoPostBack="True" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow31" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell24" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                <asp:RadioButton ID="rb400" runat="server" Text="400" AutoPostBack="true" OnCheckedChanged="rb400_OnCheckedChanged"
                                    CssClass="contentRb" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow30" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell26" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                <asp:RadioButton ID="rb500" runat="server" Text="500" AutoPostBack="true" OnCheckedChanged="rb500_OnCheckedChanged"
                                    CssClass="contentRb" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow8" runat="server" Width="150px" CssClass="contentSeparatorRow" />
                        <asp:TableRow ID="TableRow4" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell3" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Label ID="lblPeriod" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow5" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                <asp:RadioButton ID="rbPrevMonth" runat="server" CssClass="contentRb" OnCheckedChanged="rbPreviousMonth_OnCheckedChanged"
                                    AutoPostBack="True" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow6" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell7" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                <asp:RadioButton ID="rbPrevWeek" runat="server" CssClass="contentRb" OnCheckedChanged="rbPreviousWeek_OnCheckedChanged"
                                    AutoPostBack="True" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow67" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell8" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                <asp:RadioButton ID="rbLast2Weeks" runat="server" CssClass="contentRb" OnCheckedChanged="rbPrevious2Weeks_OnCheckedChanged"
                                    AutoPostBack="True" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow33" runat="server" Width="150px">
                        <asp:TableCell ID="TableCell39" runat="server" Width="150px" CssClass="tabCell">
                            <asp:Table ID="Table39" runat="server" Height="20px" Width="150px">
                            </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                        <asp:TableRow ID="TableRow34" runat="server" Width="150px">
                        <asp:TableCell ID="TableCell41" runat="server" Width="150px" CssClass="tabCell">
                            <asp:Table ID="Table64" runat="server" Width="150px" CssClass="tabTable">
                            <asp:TableRow ID="TableRow115" runat="server" Width="150px">
                                <asp:TableCell ID="TableCell95" runat="server" Width="150px" CssClass="tabCell">
                                    <asp:Label ID="lblCompany" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow114" runat="server" Width="150px">
                                <asp:TableCell ID="TableCell94" runat="server" Width="150px" CssClass="tabCell">
                                    <asp:ListBox ID="lBoxCompany" runat="server" Width="140px" Height="100px" SelectionMode="Multiple"
                                        CssClass="contentLblLeft"></asp:ListBox>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                        <asp:TableRow ID="TableRow7" runat="server" Width="150px"> 
                    <asp:TableCell ID="TableCell43" runat="server" Width="150px" CssClass="tabCell">
                       <asp:Table ID="Table245" runat="server" Height="20px" Width="150px"></asp:Table>
                       </asp:TableCell>
                    </asp:TableRow>
                        <asp:TableRow ID="TableRow35" runat="server" Width="150px">
                        <asp:TableCell ID="TableCell49" runat="server" Width="150px" CssClass="tabCell">
                        <asp:Table ID="Table8" runat="server" Width="150px" CssClass="tabTable">
                            <asp:TableRow ID="TableRow16" runat="server" Width="150px">
                                <asp:TableCell ID="TableCell18" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Label ID="lblTime" runat="server" Width="110px" CssClass="contentLblLeft"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                            <asp:TableRow ID="TableRow29" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell22" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table12" runat="server" Width="140px" CssClass="tabNoBorderTable">
                                    <asp:TableRow ID="TableRow11" runat="server" Width="140px">
                                        <asp:TableCell ID="TableCell33" runat="server" HorizontalAlign="Center" Width="25px"
                                            CssClass="tabCell">
                                            <asp:ImageButton ID="btnPrevDayPeriod" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/backArrow.png"
                                                OnClick="btnPrevDayPeriod_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell34" runat="server" HorizontalAlign="Center" Width="25px"
                                            CssClass="tabCell">
                                            <asp:ImageButton ID="btnNextDayPeriod" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/forwardArrow.png"
                                                OnClick="btnNextDayPeriod_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell35" runat="server" Width="90px" CssClass="tabCell"></asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                             <asp:TableRow ID="TableRow17" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell19" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table18" runat="server" Width="140px" CssClass="tabNoBorderTable">
                                    <asp:TableRow ID="TableRow18" runat="server" Width="140px">
                                        <asp:TableCell ID="TableCell20" runat="server" Width="35px" CssClass="tabCell">
                                            <asp:Label ID="lblFrom" runat="server" Width="30px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell32" runat="server" Width="75px" CssClass="tabCell">
                                            <asp:TextBox ID="tbFromDate" runat="server" Width="70px" CssClass="contentTb"></asp:TextBox>
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
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow13" runat="server" Width="140px">
                                        <asp:TableCell ID="TableCell45" runat="server" Width="35px" CssClass="tabCell">
                                            <asp:Label ID="lblTo" runat="server" Width="30px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell48" runat="server" Width="75px" CssClass="tabCell">
                                            <asp:TextBox ID="tbToDate" runat="server" Width="70px" CssClass="contentTb"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell57" runat="server" Width="30px" CssClass="tabCell">
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
                                                            <asp:ImageButton ID="btnTo" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif" Visible="false" runat="server" OnClick="ShowCalendarTo" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                         </asp:Table>
                       </asp:TableCell>
                    </asp:TableRow>
                       <asp:TableRow ID="TableRow36" runat="server" Width="150px" CssClass="contentSeparatorRow" />
                       <asp:TableRow ID="TableRow3" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell153" runat="server" Width="150px" HorizontalAlign="Center"
                                CssClass="tabCell">
                                <asp:Button ID="btnReport" runat="server" OnClick="btnReport_Click" CssClass="contentBtn">
                                </asp:Button>
                            </asp:TableCell>
                        </asp:TableRow>
                      </asp:Table>
                </asp:TableCell>
                
                 <asp:TableCell ID="TableCell2" runat="server" Width="150px" CssClass="tabCell">
                 <asp:Table ID="Table4" runat="server" Width="150px" CssClass="tabTable">
                  <asp:TableRow ID="TableRow39" runat="server" Width="150px">
                        <asp:TableCell ID="TableCell50" runat="server" Width="140px" CssClass="tabCell">
                        <asp:Label ID="lblReportSickName" runat="server" Width="140px" HorizontalAlign="Center" CssClass="contentLblLeft"></asp:Label>
                        </asp:TableCell>
                     </asp:TableRow>
                    <asp:TableRow ID="TableRow38" runat="server" Width="150px" CssClass="contentSeparatorRow" />
                    <asp:TableRow ID="TableRow14" runat="server" Width="150px">
                        <asp:TableCell ID="TableCell11" runat="server" Width="150px" HorizontalAlign="Left" CssClass="tabCell">
                            <asp:ListBox ID="lBoxWU" runat="server" Width="140px" Height="160px" SelectionMode="Multiple"
                                CssClass="contentLblLeft" OnSelectedIndexChanged="lBoxWU_SelectedIndexChanged"
                                AutoPostBack="False"></asp:ListBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow15" runat="server" Width="150px">
                        <asp:TableCell ID="TableCell12" runat="server" Width="150px" HorizontalAlign="Left"
                            CssClass="tabCell">
                            <asp:CheckBox ID="cbSelectAllWU" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectAllWU_CheckedChanged"
                                CssClass="contentLblLeft" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow19" runat="server" Width="150px" CssClass="contentSeparatorRow" />
                    <asp:TableRow ID="TableRow20" runat="server" Width="150px">

                            <asp:TableCell ID="TableCell21" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table2" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow21" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell13" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblPeriod1" runat="server" Width="110px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow26" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell42" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table9" runat="server" Width="140px" CssClass="tabNoBorderTable">
                                                <asp:TableRow ID="TableRow27" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell15" runat="server" HorizontalAlign="Center" Width="25px"
                                                        CssClass="tabCell">
                                                        <asp:ImageButton ID="btnPrevDayPeriod1" runat="server" Visible="false" ImageUrl="/ACTAWeb/CommonWeb/images/backArrow.png"
                                                            OnClick="btnPrevDayPeriod1_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
                                                            <%--<asp:Calendar ID="calendarPrev" runat="server" OnSelectionChanged="DataPrevChange" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
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
                                                            <asp:ImageButton ID="btnPrev" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"  runat="server" OnClick="ShowCalendarPrev" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell16" runat="server" HorizontalAlign="Center" Width="25px"
                                                        CssClass="tabCell">
                                                        <asp:ImageButton ID="btnNextDayPeriod1" runat="server" Visible="false" ImageUrl="/ACTAWeb/CommonWeb/images/forwardArrow.png"
                                                            OnClick="btnNextDayPeriod1_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
                                                            <%--<asp:Calendar ID="calendarNext" runat="server" OnSelectionChanged="DataNextChange" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
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
                                                            <asp:ImageButton ID="btnNext" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"  runat="server" OnClick="ShowCalendarNext" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell17" runat="server" Width="90px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow22" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell23" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table7" runat="server" Width="140px" CssClass="tabNoBorderTable">
                                                <asp:TableRow ID="TableRow23" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell25" runat="server" Width="110px" CssClass="tabCell">
                                                        <asp:Label ID="lblFrom1" runat="server" Width="100px" CssClass="contentLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell28" runat="server" Width="30px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow24" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell29" runat="server" Width="110px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbFromDate1" runat="server" Width="100px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>                                                    
                                                    <asp:TableCell ID="TableCell30" runat="server" Width="30px" CssClass="tabCell">
                                                        <asp:ImageButton ID="btnFromDate1" Visible="false" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"
                                                            CssClass="contentImgBtn"></asp:ImageButton>
                                                            <asp:Calendar ID="calendarFrom1" runat="server" OnSelectionChanged="DataFrom1Change" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
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
                                                            <asp:ImageButton ID="btnFrom1" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"  runat="server" OnClick="ShowCalendarFrom1" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow25" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell31" runat="server" Width="110px" CssClass="tabCell">
                                                        <asp:Label ID="lblTo1" runat="server" Width="100px" CssClass="contentLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell47" runat="server" Width="30px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow37" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell36" runat="server" Width="110px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbToDate1" runat="server" Width="100px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell38" runat="server" Width="30px" CssClass="tabCell">
                                                        <asp:ImageButton ID="btnToDate1" runat="server" Visible="false" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"
                                                            CssClass="contentImgBtn"></asp:ImageButton>
                                                            <asp:Calendar ID="calendarTo1" runat="server" OnSelectionChanged="DateTo1Change" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
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
                                                            <asp:ImageButton ID="btnTo1" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"  runat="server" OnClick="ShowCalendarTo1" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    <asp:TableRow ID="TableRow28" runat="server" Width="150px" CssClass="contentSeparatorRow" />
                    <asp:TableRow ID="TableRow32" runat="server" Width="150px" CssClass="contentSeparatorRow">
                    <asp:TableCell ID="TableCell155" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabCell">
                         <asp:Button ID="btnReport1" runat="server" OnClick="btnReport1_Click" CssClass="contentBtn">
                           </asp:Button>
                    </asp:TableCell>                                   
                    </asp:TableRow>
                </asp:Table>               
                </asp:TableCell>
                <asp:TableCell ID="TableCell1_TreciIzvestaj" runat="server" Width="200px" CssClass="tabCell">
                    <asp:Table ID="Table1_TreciIzvestaj" runat="server" Width="200px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow1_TreciIzvestaj" runat="server" Width="200px">
                            <asp:TableCell ID="TableCell2_TreciIzvestaj" runat="server" Width="190px" CssClass="tabCell">
                                <asp:Label ID="Label1_TreciIzvestaj" runat="server" Width="190px" HorizontalAlign="Center" CssClass="contentLblLeft" Text="Broj prisutnih i odsutnih za period"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow40" runat="server" Width="200px" CssClass="contentSeparatorRow" />
                        <asp:TableRow ID="TableRow2_TreciIzvestaj" runat="server" Width="200px">
                            <asp:TableCell ID="TableCell3_TreciIzvestaj" runat="server" Width="190px" CssClass="tabCell">
                                <asp:ListBox ID="lbWU_TreciIzvestaj" CssClass="contentLblLeft" runat="server" Width="190px" Height="180px" SelectionMode="Multiple" OnSelectedIndexChanged="lbWU_IndexChanged" AutoPostBack="False"></asp:ListBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow3_TreciIzvestaj" runat="server" Width="200px">
                            <asp:TableCell ID="TableCell4_TreciIzvestaj" runat="server" Width="190px" HorizontalAlign="Left" CssClass="tabCell">
                                <asp:CheckBox ID="CheckBoxAllWU_TreciIzvestaj" runat="server" Text="Označi sve" AutoPostBack="true" OnCheckedChanged="cbSelectAllWU_CheckedChanged_TreciIzvestaj" CssClass="contentLblLeft" />
                                <%-- <asp:Label runat="server" Width="100px" HorizontalAlign="Center" Text="Označi sve" CssClass="contentLblLeft"></asp:Label>--%>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow41" runat="server" Width="200px" CssClass="contentSeparatorRow" />
                        <asp:TableRow ID="TableRow4_TreciIzvestaj" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell5_TreciIzvestaj" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabCell">
                                <asp:Table ID="Table2_TreciIzvestaj" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow5_TreciIzvestaj" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell6_TreciIzvestaj" runat="server" Width="140px" CssClass="tabCell">
                                            <asp:Label ID="Label2_TreciIzvestaj" runat="server" Width="140px" CssClass="contentLblLeft" Text="Period ili dan:"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow6_TreciIzvestaj" runat="server" Width="150px" CssClass="contentSeparatorRow"/>
                                    <asp:TableRow ID="TableRow7_TreciIzvestaj" runat="server" Width="150px">
                                        <asp:TableCell runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table runat="server" Width="150px" CssClass="tabNoBorderTable">
                                                <asp:TableRow runat="server" Width="140px">
                                                    <asp:TableCell runat="server" Width="100px" CssClass="tabCell">
                                                        <asp:Label runat="server" Width="100px" CssClass="contentLblLeft" Text="Od:"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell runat="server" Width="40px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow runat="server" Width="140px">
                                                    <asp:TableCell runat="server" Width="100px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbFromDate_TreciIzvestaj" runat="server" Width="100px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell runat="server" Width="40px" CssClass="tabCell">
                                                        <asp:Calendar ID="calendarFromDate_TreciIzvestaj" runat="server" OnSelectionChanged="fromDate_TreciIzvestaj_Changed" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
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
                                                        <asp:ImageButton ID="btnFromDate_TreciIzvestaj" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif" OnClick="btnFrom_TreciIzvestaj_Click" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow runat="server" Width="140px">
                                                    <asp:TableCell runat="server" Width="100px" CssClass="tabCell">
                                                        <asp:Label runat="server" Width="100px" CssClass="contentLblLeft" Text="Do:"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell runat="server" Width="40px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow runat="server" Width="140px">
                                                    <asp:TableCell runat="server" Width="100px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbToDate_TreciIzvestaj" runat="server" Width="100px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell runat="server" Width="40px" CssClass="tabCell">
                                                        <asp:Calendar ID="calendarToDate_TreciIzvestaj" runat="server" OnSelectionChanged="toDate_TreciIzvestaj_Changed" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
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
                                                        <asp:ImageButton ID="btnToDate_TreciIzvestaj" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif" OnClick="btnTo_TreciIzvestaj_Click" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" Width="200px" CssClass="contentSeparatorRow" />
                        <asp:TableRow runat="server" Width="200px">
                            <asp:TableCell runat="server" Width="190px" CssClass="tabCell" HorizontalAlign="Center">
                                <asp:Button ID="btnTreciIzvestaj" runat="server" Width="170px" CssClass="contentBtn" OnClick="btnTreciIzvestaj_Click" Text="Izvezi izveštaj u Excel fajl" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell9" runat="server" Width="650px" CssClass="tabCell"></asp:TableCell>   
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="1180px">
               <%-- <asp:TableCell ID="TableCell151" runat="server" Width="155px" HorizontalAlign="Center"
                    CssClass="tabMidAlignCell">
                    <asp:Table ID="Table3" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell153" runat="server" Width="150px" HorizontalAlign="Center"
                                CssClass="tabCell">
                                <asp:Button ID="btnReport" runat="server" OnClick="btnReport_Click" CssClass="contentBtn">
                                </asp:Button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>--%>
                <asp:TableCell ID="TableCell4" runat="server" Width="1000px" CssClass="tabMidAlignCell">
                    <asp:Table ID="Table5" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow10" runat="server" Width="1000px">                            
                            <asp:TableCell ID="TableCell10" runat="server" Width="1000px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblError" runat="server" Width="980px" Text="" CssClass="errorText"></asp:Label>
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
