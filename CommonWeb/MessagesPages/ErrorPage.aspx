<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="CommonWeb.MessagesPages.ErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Logout.js"></script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" method="post" runat="server">
    <div>
        <br />
        <br />
        <br />
        <br />   
        <br />
        <asp:Table ID="hdrTable" runat="server" Width="600px" HorizontalAlign="Center" CssClass="navigationTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="600px">                 
                <asp:TableCell ID="TableCell5" runat="server" HorizontalAlign="Left" Width="300px" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnBack" runat="server" OnClick="lbtnBack_Click" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="Right" Width="300px" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnExit" runat="server" OnClick="LogOut" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="ErrorTitleTable" runat="server" Width="600px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="errorTitleRow" runat="server" Width="600px">
                <asp:TableCell ID="errorTitleCol" runat="server" Width="600px" CssClass="hdrCell">
                    <asp:Label ID="lblErrorTitle" runat="server" Width="590px" CssClass="hdrTitle"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table1" runat="server" Width="600px" HorizontalAlign="Center" CssClass="contentErrorTable">
            <asp:TableRow ID="TableRow3" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server" Width="600px">
                <asp:TableCell ID="TableCell1" runat="server" Width="600px" CssClass="contentCell">
                    <asp:Label ID="lblError" runat="server" Width="590px" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow5" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
