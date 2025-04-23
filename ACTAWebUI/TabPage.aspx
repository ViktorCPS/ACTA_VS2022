<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TabPage.aspx.cs" Inherits="ACTAWebUI.TabPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--// EVERY TAB MUST HAVE CORRESPONDING VIEW!!!
        // TAB (MENU ITEM) VALUE AND VIEW ID MUST BE THE SAME!!!--%>
        <asp:Menu ID="MainMenu" runat="server" Orientation="Horizontal" CssClass="menuTab" OnMenuItemClick="MainMenu_MenuItemClick">
            <StaticMenuItemStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px" CssClass="nonactiveTab" />
            <StaticSelectedStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px" CssClass="activeTab" />
            <Items></Items>
        </asp:Menu>
        <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Visible = "false" CssClass="menuTab">
            <StaticMenuItemStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px" CssClass="nonactiveTab" />
            <StaticSelectedStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px" CssClass="activeTab" />
            <Items>
                <asp:MenuItem Text="TLTMData" Value="TLTMData"></asp:MenuItem>
                <asp:MenuItem Text="TLLoans" Value="TLLoans"></asp:MenuItem>
                <asp:MenuItem Text="TLAnnualLeave" Value="TLAnnualLeave"></asp:MenuItem>
                <asp:MenuItem Text="TLMassiveInput" Value="TLMassiveInput"></asp:MenuItem>
                <asp:MenuItem Text="TLClockData" Value="TLClockData"></asp:MenuItem>
                <asp:MenuItem Text="TLDetails" Value="TLDetails"></asp:MenuItem>
                <asp:MenuItem Text="TLWUStatisticalReports" Value="TLWUStatisticalReports"></asp:MenuItem>
                <asp:MenuItem Text="TLReports" Value="TLReports"></asp:MenuItem>
                <asp:MenuItem Text="WCTMData" Value="WCTMData"></asp:MenuItem>
                <asp:MenuItem Text="WCAnnualLeave" Value="WCAnnualLeave"></asp:MenuItem>
                <asp:MenuItem Text="WCClockData" Value="WCClockData"></asp:MenuItem>
                <asp:MenuItem Text="WCDetails" Value="WCDetails"></asp:MenuItem>
                <asp:MenuItem Text="WCForms" Value="WCForms"></asp:MenuItem>
                <asp:MenuItem Text="WCReports" Value="WCReports"></asp:MenuItem>
                <asp:MenuItem Text="WCPayslips" Value="WCPayslips"></asp:MenuItem>
                <asp:MenuItem Text="WCManagerTMData" Value="WCManagerTMData"></asp:MenuItem>
                <asp:MenuItem Text="WCManagerVerification" Value="WCManagerVerification"></asp:MenuItem>
                <asp:MenuItem Text="WCManagerClockData" Value="WCManagerClockData"></asp:MenuItem>
                <asp:MenuItem Text="WCManagerWUStatisticalReports" Value="WCManagerWUStatisticalReports"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCTMData" Value="HRSSCTMData"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCCounters" Value="HRSSCCounters"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCVerification" Value="HRSSCVerification"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCConfirmationAbsences" Value="HRSSCConfirmationAbsences"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCAnnualLeave" Value="HRSSCAnnualLeave"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCMassiveInput" Value="HRSSCMassiveInput"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCClockData" Value="HRSSCClockData"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCOutstandingData" Value="HRSSCOutstandingData"></asp:MenuItem>
                <asp:MenuItem Text="HRSSCDetails" Value="HRSSCDetails"></asp:MenuItem>                
                <asp:MenuItem Text="HRLegalEntityTMData" Value="HRLegalEntityTMData"></asp:MenuItem>
                <asp:MenuItem Text="HRLegalEntityLoans" Value="HRLegalEntityLoans"></asp:MenuItem>
                <asp:MenuItem Text="HRLegalEntityClockData" Value="HRLegalEntityClockData"></asp:MenuItem>
                <asp:MenuItem Text="HRLegalEntityDetails" Value="HRLegalEntityDetails"></asp:MenuItem>
                <asp:MenuItem Text="HRLegalEntityWUStatisticalReports" Value="HRLegalEntityWUStatisticalReports"></asp:MenuItem>
                <asp:MenuItem Text="HRLegalEntityReports" Value="HRLegalEntityReports"></asp:MenuItem>
                <asp:MenuItem Text="HRLegalEntityWorkAnalyzeReports" Value="HRLegalEntityWorkAnalyzeReports"></asp:MenuItem>
                <asp:MenuItem Text="HRLegalEntityBufferReport" Value="HRLegalEntityBufferReport"></asp:MenuItem>
                <asp:MenuItem Text="HRLegalEntityOutstandingData" Value="HRLegalEntityOutstandingData"></asp:MenuItem>
                <asp:MenuItem Text="MCScheduling" Value="MCScheduling"></asp:MenuItem>
                <asp:MenuItem Text="MCVisitsSearch" Value="MCVisitsSearch"></asp:MenuItem>
                <asp:MenuItem Text="MCEmployeeData" Value="MCEmployeeData"></asp:MenuItem>
                <asp:MenuItem Text="MCReports" Value="MCReports"></asp:MenuItem>
                <asp:MenuItem Text="BCTMData" Value="BCTMData"></asp:MenuItem>
                <asp:MenuItem Text="BCAnnualLeave" Value="BCAnnualLeave"></asp:MenuItem>
                <asp:MenuItem Text="BCClockData" Value="BCClockData"></asp:MenuItem>
                <asp:MenuItem Text="BCDetails" Value="BCDetails"></asp:MenuItem>
                <asp:MenuItem Text="BCForms" Value="BCForms"></asp:MenuItem>
                <asp:MenuItem Text="BCReports" Value="BCReports"></asp:MenuItem>
                <asp:MenuItem Text="<" Value="Prev"></asp:MenuItem>
                <asp:MenuItem Text=">" Value="Next"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="TLTMData" runat="server">
                <asp:Table ID="Table6" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow6" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell5" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="TLTMDataIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="TLLoans" runat="server">
                <asp:Table ID="Table3" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow4" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell4" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="TLLoansIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="TLAnnualLeave" runat="server">
                <asp:Table ID="Table22" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow22" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell22" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="TLAnnualLeaveIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="TLMassiveInput" runat="server">
                <asp:Table ID="Table1" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow2" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell2" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="TLMassiveInputIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="TLClockData" runat="server">
                <asp:Table ID="Table2" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow3" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell3" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="TLClockDataIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="TLDetails" runat="server">
                <asp:Table ID="Table4" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow5" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell6" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="TLDetailsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="TLWUStatisticalReports" runat="server">
                <asp:Table ID="Table5" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow7" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell7" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="TLWUStatisticalReportsIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="TLReports" runat="server">
                <asp:Table ID="Table7" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow1" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell1" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="TLReportsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCTMData" runat="server">
                <asp:Table ID="Table8" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow8" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell8" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCTMDataIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCAnnualLeave" runat="server">
                <asp:Table ID="Table25" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow25" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell25" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCAnnualLeaveIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCClockData" runat="server">
                <asp:Table ID="Table11" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow11" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell11" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCClockDataIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCDetails" runat="server">
                <asp:Table ID="Table12" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow12" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell12" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCDetailsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCForms" runat="server">
                <asp:Table ID="Table10" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow10" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell10" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCFormsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCReports" runat="server">
                <asp:Table ID="Table9" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow9" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell9" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCReportsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
             <asp:View ID="WCPayslips" runat="server">
                <asp:Table ID="Table36" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow36" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell36" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCPayslipsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCManagerTMData" runat="server">
                <asp:Table ID="Table13" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow13" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell13" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCManagerTMDataIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCManagerVerification" runat="server">
                <asp:Table ID="Table14" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow14" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell14" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCManagerVerificationIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCManagerClockData" runat="server">
                <asp:Table ID="Table32" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow32" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell32" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCManagerClockDataIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="WCManagerWUStatisticalReports" runat="server">
                <asp:Table ID="Table15" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow15" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell15" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="WCManagerWUStatisticalReportsIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCTMData" runat="server">
                <asp:Table ID="Table16" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow16" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell16" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCTMDataIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCCounters" runat="server">
                <asp:Table ID="Table18" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow18" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell18" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCCountersIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCVerification" runat="server">
                <asp:Table ID="Table20" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow20" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell20" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCVerificationIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCConfirmationAbsences" runat="server">
                <asp:Table ID="Table19" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow19" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell19" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCConfirmationAbsencesIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCAnnualLeave" runat="server">
                <asp:Table ID="Table30" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow30" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell30" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCAnnualLeaveIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCMassiveInput" runat="server">
                <asp:Table ID="Table17" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow17" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell17" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCMassiveInputIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCClockData" runat="server">
                <asp:Table ID="Table31" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow31" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell31" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCClockDataIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCOutstandingData" runat="server">
                <asp:Table ID="Table21" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow21" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell21" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCOutstandingDataIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRSSCDetails" runat="server">
                <asp:Table ID="Table33" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow33" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell33" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRSSCDetailsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>            
            <asp:View ID="HRLegalEntityTMData" runat="server">
                <asp:Table ID="Table23" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow23" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell23" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityTMDataIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRLegalEntityLoans" runat="server">
                <asp:Table ID="Table27" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow27" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell27" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityLoansIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRLegalEntityClockData" runat="server">
                <asp:Table ID="Table26" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow26" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell26" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityClockDataIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRLegalEntityDetails" runat="server">
                <asp:Table ID="Table28" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow28" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell28" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityDetailsIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRLegalEntityWUStatisticalReports" runat="server">
                <asp:Table ID="Table29" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow29" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell29" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityWUStatisticalReportsIframe" runat="server" frameborder="0"
                                scrolling="auto" marginheight="0px" marginwidth="0px" height="595px" class="pageIframe">
                            </iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRLegalEntityReports" runat="server">
                <asp:Table ID="Table24" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow24" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell24" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityReportsIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRLegalEntityWorkAnalyzeReports" runat="server">
                <asp:Table ID="Table34" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow34" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell34" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityWorkAnalyzeReportsIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRLegalEntityBufferReport" runat="server">
                <asp:Table ID="Table35" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow35" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell35" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityBufferReportIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="HRLegalEntityOutstandingData" runat="server">
                <asp:Table ID="Table41" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow41" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell41" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="HRLegalEntityOutstandingDataIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="MCScheduling" runat="server">
                <asp:Table ID="Table37" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow37" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell37" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="MCSchedulingIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="MCVisitsSearch" runat="server">
                <asp:Table ID="Table38" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow38" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell38" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="MCVisitsSearchIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="MCEmployeeData" runat="server">
                <asp:Table ID="Table39" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow39" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell39" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="MCEmployeeDataIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="MCReports" runat="server">
                <asp:Table ID="Table40" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow40" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell40" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="MCReportsIframe" runat="server" frameborder="0" scrolling="auto"
                                marginheight="0px" marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="BCTMData" runat="server">
                <asp:Table ID="Table42" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow42" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell42" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="BCTMDataIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="BCAnnualLeave" runat="server">
                <asp:Table ID="Table43" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow43" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell43" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="BCAnnualLeaveIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="BCClockData" runat="server">
                <asp:Table ID="Table44" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow44" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell44" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="BCClockDataIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="BCDetails" runat="server">
                <asp:Table ID="Table45" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow45" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell45" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="BCDetailsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="BCForms" runat="server">
                <asp:Table ID="Table46" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow46" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell46" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="BCFormsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="BCReports" runat="server">
                <asp:Table ID="Table47" runat="server" Width="1190px" CssClass="tabTable">
                    <asp:TableRow ID="TableRow47" runat="server" Width="1190px">
                        <asp:TableCell ID="TableCell47" runat="server" Width="1190px" CssClass="tabCell">
                            <iframe id="BCReportsIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                marginwidth="0px" height="595px" class="pageIframe"></iframe>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
        </asp:MultiView>
    </div>
    </form>
</body>
</html>
