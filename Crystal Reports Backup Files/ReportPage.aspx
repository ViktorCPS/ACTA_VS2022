<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPage.aspx.cs" Inherits="ReportsWeb.ReportPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Report</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Logout.js"></script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<asp:Table ID="Table3" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="navigationTable">
            <asp:TableRow ID="TableRow3" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell1" runat="server" Width="100px" HorizontalAlign="Left" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnMenu" runat="server" OnClick="lbtnMenu_Click" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell42" runat="server" Width="50px" HorizontalAlign="Left" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnBack" runat="server" OnClick="lbtnBack_Click" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" HorizontalAlign="Right" Width="1050px" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnExit" runat="server" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table1" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell3" runat="server" Width="1200px" CssClass="hdrCell">
                    <asp:Label ID="lblReport" runat="server" Width="1195px" CssClass="hdrTitle"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>--%>
        <asp:Table ID="Table4" runat="server" Width="1150px" HorizontalAlign="Center" 
            CssClass="tabTable">
            <asp:TableRow ID="TableRow10" runat="server" Width="1150px">
                <asp:TableCell ID="TableCell33" runat="server" Width="1150px" CssClass="tabCell">
                    <iframe id="reportIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px" marginwidth="0px" height="500px" class="pageIframe"></iframe>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table5" runat="server" Width="1150px" HorizontalAlign="Center" CssClass="tabTable">
            <asp:TableRow ID="TableRow7" runat="server" Width="1150px">
                <asp:TableCell ID="TableCell1" runat="server" Width="1050px" HorizontalAlign="Center" CssClass="tabCell"></asp:TableCell>
                <asp:TableCell ID="TableCell39" runat="server" Width="100px" HorizontalAlign="Center" CssClass="tabCell">
                    <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
    </div>
    </form>
</body>
</html>
