<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeSchedulesPage.aspx.cs" Inherits="ACTAWebUI.EmployeeSchedulesPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>    
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
    <base target="_self" />
</head>
<body onload="document.body.style.cursor = 'default'">
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:Table ID="Table5" runat="server" Width="990px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow4" runat="server" Width="990px">
                <asp:TableCell ID="TableCell3" runat="server" Width="950px" HorizontalAlign="Center" CssClass="hdrCell">
                    <asp:Label ID="lblTitle" runat="server" CssClass="loginUserTextLeft"></asp:Label>                    
                </asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabNoBorderTable">                        
            <asp:TableRow ID="TableRow14" runat="server" Width="990px">
                <asp:TableCell ID="TableCell26" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                     <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                    marginwidth="0px" height="480px" class="pageIframe"></iframe>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow3" runat="server" Width="990px">
                <asp:TableCell ID="TableCell2" runat="server" Width="990px" HorizontalAlign="Right" CssClass="tabMidAlignCell">
                    <asp:Button ID="btnClose" runat="server" CssClass="contentBtn"></asp:Button>                    
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
