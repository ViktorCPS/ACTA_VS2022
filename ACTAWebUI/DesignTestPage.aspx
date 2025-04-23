<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesignTestPage.aspx.cs" Inherits="ACTAWebUI.DesignTestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Design Test</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Logout.js"></script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
</head>
<body>    
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table3" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="navigationTable">
            <asp:TableRow ID="TableRow3" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell2" runat="server" Width="600px" HorizontalAlign="Left" CssClass="navigationCell">
                    <%--<asp:LinkButton ID="lbtnMenu" runat="server" Text="Menu" OnClick="lbtnMenu_Click" CssClass="navigationLBtn"></asp:LinkButton>--%>
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" HorizontalAlign="Right" Width="600px" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnExit" runat="server" Text="Exit" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table1" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell1" runat="server" Width="1200px" CssClass="hdrCell">
                    <asp:Label ID="lblLoggedInUser" runat="server" Width="1195px" Text="Logged in:" CssClass="loginUserText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table4" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow10" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell33" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="contentCell">
                    <iframe id="resultIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px" marginwidth="0px" height="630px" class="pageIframe" ></iframe>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table5" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="footerTable">
            <asp:TableRow ID="TableRow7" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell39" runat="server" Width="1200px" CssClass="footerCell">
                    <asp:Label ID="lblError" runat="server" Width="1195px" Text="Data validation error" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
    </div>
    </form>    
</body>
</html>
