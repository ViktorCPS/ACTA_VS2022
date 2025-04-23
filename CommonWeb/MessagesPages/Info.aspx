<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Info.aspx.cs" Inherits="CommonWeb.MessagesPages.Info" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ACTA Web</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />        
        <asp:Table ID="Table5" runat="server" Width="700px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow9" runat="server" Width="700px">
                <asp:TableCell ID="TableCell3" runat="server" Width="700px" HorizontalAlign="Center" CssClass="hdrCell">
                    <asp:Label ID="lblTitle" runat="server" CssClass="loginUserTextLeft"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table4" runat="server" Width="700px" HorizontalAlign="Center" CssClass="tabTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="700px" CssClass="contentSeparatorRow"></asp:TableRow>            
            <asp:TableRow ID="TableRow21" runat="server" Width="700px">
                <asp:TableCell ID="TableCell150" runat="server" Width="700px" CssClass="tabCell">
                    <asp:Panel ID="infoPanel" runat="server" Width="690px" Height="150px" ScrollBars="Auto" CssClass="resultPanel">
                        <asp:Label ID="lblInfoMessage" runat="server" Width="680px" CssClass="infoText"></asp:Label>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow ID="TableRow7" runat="server" Width="700px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow3" runat="server" Width="700px">
                <asp:TableCell ID="TableCell4" runat="server" Width="700px" HorizontalAlign="Center" CssClass="tabCell">                    
                    <asp:Button ID="btnClose" runat="server" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow ID="TableRow4" runat="server" Width="700px" CssClass="contentSeparatorRow"></asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>