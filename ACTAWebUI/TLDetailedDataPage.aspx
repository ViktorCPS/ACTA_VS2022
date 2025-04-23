<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TLDetailedDataPage.aspx.cs"
    Inherits="ACTAWebUI.TLDetailedDataPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>

    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>

    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body onload="document.body.style.cursor = 'default'">

    <script language="javascript" type="text/javascript">

        function pagePostBack(btnTree) {
            try {
                var btn = document.getElementById(btnTree);

                if (btn != null) {
                    __doPostBack(btn, '');
                }
            }
            catch (e) { alert(e); }
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
                                <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" CssClass="menuTab" OnMenuItemClick="Menu1_MenuItemClick">
                                    <StaticMenuItemStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px"
                                        CssClass="nonactiveTab" />
                                    <StaticSelectedStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px"
                                        CssClass="activeTab" />
                                    <Items>
                                        <asp:MenuItem Text="FS" Value="0"></asp:MenuItem>
                                        <asp:MenuItem Text="OU" Value="1"></asp:MenuItem>
                                    </Items>
                                </asp:Menu>
                                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="tabWU" runat="server">
                                        <asp:Table ID="Table4" runat="server" Width="150px" CssClass="tabTable">
                                            <asp:TableRow ID="TableRow4" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell3" runat="server" Width="150px" HorizontalAlign="Left"
                                                    CssClass="tabCell">
                                                    <asp:ListBox ID="lBoxWU" runat="server" Width="140px" Height="60px" SelectionMode="Multiple"
                                                        CssClass="contentLblLeft" OnSelectedIndexChanged="lBoxWU_SelectedIndexChanged"
                                                        AutoPostBack="True"></asp:ListBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow5" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell7" runat="server" Width="150px" HorizontalAlign="Left"
                                                    CssClass="tabCell">
                                                    <asp:CheckBox ID="cbSelectAllWU" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectAllWU_CheckedChanged"
                                                        CssClass="contentLblLeft" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>
                                    <asp:View ID="tabOU" runat="server">
                                        <asp:Table ID="Table5" runat="server" Width="150px" CssClass="tabTable">
                                            <asp:TableRow ID="TableRow8" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell10" runat="server" Width="150px" HorizontalAlign="Left"
                                                    CssClass="tabCell">
                                                    <asp:ListBox ID="lBoxOU" runat="server" Width="140px" Height="60px" SelectionMode="Multiple"
                                                        CssClass="contentLblLeft" OnSelectedIndexChanged="lBoxOU_SelectedIndexChanged"
                                                        AutoPostBack="True"></asp:ListBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow10" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell11" runat="server" Width="150px" HorizontalAlign="Left"
                                                    CssClass="tabCell">
                                                    <asp:CheckBox ID="cbSelectAllOU" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectAllOU_CheckedChanged"
                                                        CssClass="contentLblLeft" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>
                                </asp:MultiView>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow9" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell13" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table7" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow12" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell14" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblEmployee" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow13" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell15" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:TextBox ID="tbEmployee" runat="server" Width="130px" CssClass="contentTb"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow14" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell16" runat="server" Width="150px" HorizontalAlign="Left"
                                            CssClass="tabCell">
                                            <asp:ListBox ID="lboxEmployees" runat="server" Width="140px" Height="115px" SelectionMode="Multiple"
                                                OnPreRender="lboxEmployees_PreRender" CssClass="contentLblLeft"></asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell8" runat="server" Width="150px" HorizontalAlign="Left"
                                            CssClass="tabCell">
                                            <asp:CheckBox ID="cbSelectAllEmpolyees" runat="server" CssClass="contentLblLeft" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow32" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell58" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table20" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow33" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell59" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblPassTypes" runat="server" Width="110px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow34" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell60" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:ListBox runat="server" Width="140px" Height="80px" SelectionMode="Multiple"
                                                OnPreRender="lbPassTypes_PreRender" CssClass="contentLb" ID="lbPassTypes"></asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow21" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell9" runat="server" Width="150px" HorizontalAlign="Left"
                                            CssClass="tabCell">
                                            <asp:CheckBox ID="cbSelectAllPassTypes" runat="server" CssClass="contentLblLeft" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow15" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell17" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table8" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow16" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell18" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblPeriod" runat="server" Width="110px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow26" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell31" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table9" runat="server" Width="140px" CssClass="tabNoBorderTable">
                                                <asp:TableRow ID="TableRow27" runat="server" Width="140px">
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
                                                    <asp:TableCell ID="TableCell20" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:Label ID="lblFrom" runat="server" Width="65px" CssClass="contentLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell21" runat="server" Width="40px" CssClass="tabCell"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell22" runat="server" Width="30px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow29" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell32" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbFromDate" runat="server" Width="65px" AutoPostBack="true" OnTextChanged="Date_Changed"
                                                            CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell40" runat="server" Width="40px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbFromTime" runat="server" Width="33px" CssClass="contentTb"></asp:TextBox>
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
                                                <asp:TableRow ID="TableRow30" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell45" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:Label ID="lblTo" runat="server" Width="65px" CssClass="contentLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell46" runat="server" Width="40px" CssClass="tabCell"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell47" runat="server" Width="30px" CssClass="tabCell"></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow31" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell48" runat="server" Width="70px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbToDate" runat="server" Width="65px" AutoPostBack="true" OnTextChanged="Date_Changed"
                                                            CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell23" runat="server" Width="40px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbToTime" runat="server" Width="33px" CssClass="contentTb"></asp:TextBox>
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
                                <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                    marginwidth="0px" height="510px" class="pageIframe"></iframe>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow19" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell29" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Label ID="lblError"  runat="server" Width="980px" CssClass="errorText"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell151" runat="server" Width="155px" HorizontalAlign="Center"
                    CssClass="tabLeftAlignCell">
                    <asp:Table ID="Table3" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="150px" HorizontalAlign="Center"
                                CssClass="tabMidAlignCell">
                                <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" CssClass="contentBtn">
                                </asp:Button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table14" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow25" runat="server" Width="1000px">
                            <asp:TableCell ID="pairVisibilityTypeCell" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                <asp:RadioButton ID="rbPassType" runat="server" AutoPostBack="true" OnCheckedChanged="rbPassType_CheckedChanged" CssClass="contentRb" />                                
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell25" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                <asp:RadioButton ID="rbLocation" runat="server" AutoPostBack="true" OnCheckedChanged="rbLocation_CheckedChanged" CssClass="contentRb" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell41" runat="server" Width="200px" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnReport" runat="server" OnClick="btnReport_Click" CssClass="contentBtn">
                                </asp:Button>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell412" runat="server" Width="200px" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnReportNew" runat="server" OnClick="btnReportNew_Click" CssClass="contentBtn">
                                </asp:Button>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell12" runat="server" Width="150px" CssClass="tabLeftAlignCell">
                                <asp:CheckBox ID="cbMontlyTotals" runat="server" CssClass="contentLblLeft" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell24" runat="server" Width="150px" CssClass="tabLeftAlignCell">
                                <asp:CheckBox ID="cbEmplTotals" runat="server" CssClass="contentLblLeft" />
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
