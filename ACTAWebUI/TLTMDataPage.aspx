<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TLTMDataPage.aspx.cs" Inherits="ACTAWebUI.TLTMDataPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
                    __doPostBack(btn, '');
                }
            }
            catch(e) { alert(e); }
        }
        
        function pageRefresh(ctrlID, arg)
        {
            try
            {
                var ctrl = document.getElementById(ctrlID);
                
                if (ctrl != null)
                {
                    __doPostBack(ctrl, arg);
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
                                                <asp:TableCell ID="TableCell7" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbWorkshop" runat="server" Width="130px" ReadOnly="true"
                                                        CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow5" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell8" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbUte" runat="server" Width="130px" ReadOnly="true"
                                                        CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell9" runat="server" Width="150px" HorizontalAlign="Center"
                                                    CssClass="tabCell">
                                                    <asp:ImageButton ID="btnWUTree" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/treeButton.png"
                                                        CssClass="contentImgBtn"></asp:ImageButton>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>
                                    <asp:View ID="tabOU" runat="server">
                                        <asp:Table ID="Table5" runat="server" Width="150px" CssClass="tabTable">
                                            <asp:TableRow ID="TableRow8" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell10" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbOrg" runat="server" Width="130px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow10" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell11" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbOrgUte" runat="server" Width="130px" ReadOnly="true"
                                                        CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow11" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell12" runat="server" Width="150px" HorizontalAlign="Center"
                                                    CssClass="tabCell">
                                                    <asp:ImageButton ID="btnOrgTree" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/treeButton.png"
                                                        CssClass="contentImgBtn"></asp:ImageButton>
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
                                    <asp:TableRow ID="TableRow31" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell24" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                            <asp:RadioButton ID="rbHierarchically" runat="server" CssClass="contentRb" />
                                        </asp:TableCell>                                                    
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow30" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell26" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                            <asp:RadioButton ID="rbSelected" runat="server" CssClass="contentRb" />
                                        </asp:TableCell>                                                     
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow32" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell27" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                            <asp:RadioButton ID="rbResponsiblePerson" runat="server" CssClass="contentRb" />
                                        </asp:TableCell>                                                     
                                    </asp:TableRow>
                                    <asp:TableRow ID="rowEmplTypeLbl" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell43" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblEmplType" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="rowEmplType" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell39" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:DropDownList ID="cbEmplType" runat="server" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="cbEmplType_SelectedIndexChanged" CssClass="contentDDList"></asp:DropDownList>
                                        </asp:TableCell>
                                    </asp:TableRow>
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
                                            <asp:ListBox ID="lboxEmployees" runat="server" Width="140px" SelectionMode="Multiple" OnPreRender="lboxEmployees_PreRender" Height="180px" CssClass="contentLblLeft">
                                            </asp:ListBox>
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
                                    <asp:TableRow ID="TableRow17" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell19" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table18" runat="server" Width="140px" CssClass="tabNoBorderTable">                                                
                                                <asp:TableRow ID="TableRow29" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell32" runat="server" Width="110px" CssClass="tabCell">
                                                        <asp:TextBox ID="tbDate" runat="server" Width="100px" AutoPostBack = "true" OnTextChanged="Date_Changed" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell44" runat="server" Width="30px" CssClass="tabCell">
                                                        <asp:ImageButton ID="btnDate" runat="server" Visible="false" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"
                                                            CssClass="contentImgBtn"></asp:ImageButton>
                                                            <asp:Calendar ID="calendar" runat="server" OnSelectionChanged="DataChange" FirstDayOfWeek="Monday" CssClass="calendar" ShowGridLines="true" Visible="false">
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
                                                            <asp:ImageButton ID="btn" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"  runat="server" OnClick="ShowCalendar" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow21" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell20" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:CheckBox ID="chbTwoDaysView" runat="server" CssClass="contentChb"/>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table10" runat="server" Width="1000px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow20" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell49" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Table ID="Table19" runat="server" Width="1000px"  HorizontalAlign="Left" CssClass="tabNoBorderTable">
                                    <asp:TableRow ID="TableRow38" runat="server" HorizontalAlign="Left" Width="1000px">                                                                                
                                        <asp:TableCell ID="TableCell28" runat="server" Width="140px" CssClass="tabCell">
                                            <asp:Table ID="Table13" runat="server" Width="140px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow24" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell29" runat="server" Width="70px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblPresentCounter" runat="server" Width="65px" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell36" runat="server" Width="70px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblPresent" runat="server" CssClass="counterLbl"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>                                                
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell30" runat="server" Width="140px" CssClass="tabCell">
                                            <asp:Table ID="Table9" runat="server" Width="140px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow26" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell31" runat="server" Width="70px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblAbsentCounter" runat="server" Width="65px" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell37" runat="server" Width="70px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblAbsent" runat="server" CssClass="counterLbl"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>                                                
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell33" runat="server" Width="140px" CssClass="tabCell">
                                            <asp:Table ID="Table16" runat="server" Width="140px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow27" runat="server" Width="140px">
                                                    <asp:TableCell ID="TableCell34" runat="server" Width="70px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblTotalCounter" runat="server" Width="65px" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell38" runat="server" Width="70px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblTotal" runat="server" CssClass="counterLbl"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell35" runat="server" Width="500px" CssClass="tabCell">
                                            <asp:Table ID="Table11" runat="server" Width="500px" Visible="true" CssClass="tabTable">                                                
                                                <asp:TableRow ID="TableRow18" runat="server" Width="500px">
                                                    <asp:TableCell ID="TableCell48" runat="server" Width="140px" CssClass="tabCell">
                                                        <asp:CheckBox ID="chbRegistration" runat="server" Width="130px" Visible="true" AutoPostBack="true" OnCheckedChanged="chbRegistration_OnCheckChanged" CssClass="contentChbSmall"></asp:CheckBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell45" runat="server" Width="140px" CssClass="tabCell">
                                                        <asp:CheckBox ID="chbNoRegistration" runat="server" Width="130px" Visible="true" AutoPostBack="true" OnCheckedChanged="chbNoRegistration_OnCheckChanged" CssClass="contentChbSmall"></asp:CheckBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell40" runat="server" Width="60px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblOffset" runat="server" Width="55px" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell46" runat="server" Width="40px" CssClass="tabMidAlignCell">
                                                        <asp:TextBox ID="tbOffset" runat="server" Width="30px" CssClass="contentTbSmall"></asp:TextBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell47" runat="server" Width="40px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblMin" runat="server" Width="35px" Text="min" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell52" runat="server" Width="140px" CssClass="tabCell">
                                                        <asp:CheckBox ID="chbUnjustifiedAbs" runat="server" Width="130px" Visible="true" AutoPostBack="true" OnCheckedChanged="chbUnjustifiedAbs_OnCheckChanged" CssClass="contentChbSmall"></asp:CheckBox>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell51" runat="server" Width="80px" HorizontalAlign="Right" CssClass="tabMidAlignCell" >
                                                        <asp:Button ID="btnExportReport" runat="server" OnClick="btnExportReport_Click" CssClass="contentBtnSmall"></asp:Button>
                                                    </asp:TableCell>
                                                </asp:TableRow>                                               
                                            </asp:Table>
                                        </asp:TableCell>                                        
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow23" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell150" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Table ID="Table2" runat="server" Width="1000px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow22" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell23" runat="server" Width="1000px" CssClass="tabCell">
                                            <asp:Panel ID="legendPanel" runat="server" Width="990px" Height="20px" ScrollBars="Auto" CssClass="resultPanel">
                                                <asp:PlaceHolder ID="legendCtrlHolder" runat="server"></asp:PlaceHolder>
                                            </asp:Panel>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow35" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell25" runat="server" Width="1000px" CssClass="tabCell">
                                            <asp:Panel ID="hourPanel" runat="server" Width="950px" Height="20px" ScrollBars="Auto" CssClass="resultPanel">
                                                <asp:PlaceHolder ID="hourLineCtrlHolder" runat="server"></asp:PlaceHolder>
                                            </asp:Panel>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow36" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell50" runat="server" Width="1000px" CssClass="tabCell">
                                            <asp:Panel ID="resultPanel" runat="server" Width="990px" Height="440px" ScrollBars="Auto" CssClass="resultPanel">
                                                <asp:PlaceHolder ID="ctrlHolder" runat="server"></asp:PlaceHolder>
                                            </asp:Panel>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow19" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell3" runat="server" Width="1000px" CssClass="tabCell">
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
                    <asp:Table ID="Table3" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="150px">                        
                            <asp:TableCell ID="TableCell21" runat="server" Width="75px" HorizontalAlign="Left" CssClass="tabCell">
                                <asp:Button ID="btnAssignTimeSchema" runat="server" OnClick="btnAssignTS_Click" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>                        
                            <asp:TableCell ID="TableCell5" runat="server" Width="75px" HorizontalAlign="Right" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" CssClass="contentBtn"></asp:Button>
                                <asp:TextBox ID="tbSelectedDate" runat="server" Visible="false" CssClass="contentTb"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table14" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow25" runat="server" Width="1000px">
                            <asp:TableCell ID="pairVisibilityTypeCell" runat="server" Width="230px" CssClass="tabMidAlignCell">                                
                                <asp:RadioButton ID="rbPassType" runat="server" AutoPostBack="true" OnCheckedChanged="rbPassType_CheckedChanged" CssClass="contentRb" />
                                <asp:RadioButton ID="rbLocation" runat="server" AutoPostBack="true" OnCheckedChanged="rbLocation_CheckedChanged" CssClass="contentRb" />                                
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell41" runat="server" Width="100px" HorizontalAlign="Right" CssClass="tabMidAlignCell">
                                <%--<asp:Button ID="btnPrev" runat="server" OnClick="btnPrev_Click" CssClass="contentBtn"></asp:Button>--%>
                                <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/backArrow.png" OnClick="btnPrev_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell22" runat="server" Width="340px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblGraphDay" runat="server" Width="330px" CssClass="contentGraphPeriodLbl"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell42" runat="server" Width="330px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
                                <%--<asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="contentBtn"></asp:Button>--%>
                                <asp:ImageButton ID="btnNext" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/forwardArrow.png" OnClick="btnNext_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
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
