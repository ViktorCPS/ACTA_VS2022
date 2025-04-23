<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CountersPage.aspx.cs" Inherits="ACTAWebUI.CountersPage" %>

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
        <asp:Table ID="Table2" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="990px">
                <asp:TableCell ID="TableCell4" runat="server" Width="990px" CssClass="tabCell">
                    <asp:Panel ID="resultPanel" runat="server" Width="950px" Height="505px" ScrollBars="Auto"
                        CssClass="resultPanel">
                        <asp:PlaceHolder ID="ctrlHolder" runat="server"></asp:PlaceHolder>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
