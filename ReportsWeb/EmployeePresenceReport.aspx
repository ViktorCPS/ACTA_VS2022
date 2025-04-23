<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeePresenceReport.aspx.cs" Inherits="ReportsWeb.EmployeePresenceReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Logout.js"></script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table3" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="navigationTable">
            <asp:TableRow ID="TableRow3" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell1" runat="server" Width="600px" HorizontalAlign="Left" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnMenu" runat="server" Text="Menu" OnClick="lbtnMenu_Click" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" HorizontalAlign="Right" Width="600px" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnExit" runat="server" Text="Exit" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table1" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell3" runat="server" Width="1200px" CssClass="hdrCell">
                    <asp:Label ID="lblPresenceReport" runat="server" Width="1190px" Text="Presence report" CssClass="hdrTitle"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table2" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow8" runat="server" Width="1200px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow4" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell2" runat="server" Width="140px" CssClass="contentCell">
                    <asp:Label ID="lblMonth" runat="server" Width="135px" Text="Month:" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" Width="240px" CssClass="contentCell">
                    <asp:TextBox ID="tbMonth" runat="server" Width="230px" CssClass="contentTb"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell26" runat="server" Width="165px" CssClass="contentCell">
                    <asp:Label ID="lblMonthFormat" runat="server" Width="160px" Text="*Month format" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell16" runat="server" Width="160px" CssClass="contentCell">
                    <asp:Label ID="lblMonthExample" runat="server" Width="155px" Text="*Month example" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell17" runat="server" Width="165px" CssClass="contentCell">
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
                <asp:TableCell ID="TableCell18" runat="server" Width="165px" CssClass="contentCell">
                    <asp:Button ID="btnStatistics" runat="server" Text="Statistics" OnClick="btnStatistics_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
                <asp:TableCell ID="TableCell28" runat="server" Width="165px" CssClass="contentCell">
                    <asp:Button ID="btnShowGraph" runat="server" Text="Show graph" OnClick="btnShowGraph_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow5" runat="server" Width="1200px" CssClass="contentSeparatorRow"></asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table6" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow6" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell6" runat="server" Width="1200px" CssClass="contentCell">
                    <asp:Label ID="Label7" runat="server" Width="10px" Text="" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="lblDate" runat="server" Width="200px" Text="Month" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="lblTotal" runat="server" Width="100px" Text="Total" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label1" runat="server" Width="30px" Text="00" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label3" runat="server" Width="30px" Text="01" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label2" runat="server" Width="30px" Text="02" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label4" runat="server" Width="30px" Text="03" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label5" runat="server" Width="30px" Text="04" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label6" runat="server" Width="30px" Text="05" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label9" runat="server" Width="30px" Text="06" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label10" runat="server" Width="30px" Text="07" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label11" runat="server" Width="30px" Text="08" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label12" runat="server" Width="30px" Text="09" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label13" runat="server" Width="30px" Text="10" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label14" runat="server" Width="30px" Text="11" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label15" runat="server" Width="30px" Text="12" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label17" runat="server" Width="30px" Text="13" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label18" runat="server" Width="30px" Text="14" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label19" runat="server" Width="30px" Text="15" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label20" runat="server" Width="30px" Text="16" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label21" runat="server" Width="30px" Text="17" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label22" runat="server" Width="30px" Text="18" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label23" runat="server" Width="30px" Text="19" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label24" runat="server" Width="30px" Text="20" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label25" runat="server" Width="30px" Text="21" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label16" runat="server" Width="30px" Text="22" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                    <asp:Label ID="Label8" runat="server" Width="30px" Text="23" Font-Bold="true" CssClass="contentHdrLbl"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
        <asp:Table ID="Table4" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell4" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="contentCell">
                    <asp:Panel ID="resultPanel" runat="server" Width="1190px" Height="500px" ScrollBars="Auto" CssClass="resultPanel">
                        <asp:PlaceHolder ID="ctrlHolder" runat="server"></asp:PlaceHolder>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
        <asp:Table ID="Table5" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="footerTable">
            <asp:TableRow ID="TableRow7" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell39" runat="server" Width="1200px" CssClass="footerCell">
                    <asp:Label ID="lblError" runat="server" Width="1190px" Text="Data validation error" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell32" runat="server" Width="1200px" CssClass="footerCell">
                    <asp:Label ID="lblLoggedInUser" runat="server" Width="1190px" Text="Logged in:" CssClass="loginUserText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
