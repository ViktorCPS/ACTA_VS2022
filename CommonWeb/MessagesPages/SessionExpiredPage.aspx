<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionExpiredPage.aspx.cs" Inherits="CommonWeb.MessagesPages.SessionExpiredPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        <br />
        <br />   
        <br />
        <asp:Table ID="Table" runat="server" Width="600px" HorizontalAlign="Center" CssClass="tabTable">
            <asp:TableRow ID="errorRow" runat="server" Width="600px">
                <asp:TableCell ID="errorCol" runat="server" Width="600px" CssClass="tabCell">
                    <asp:Label ID="lblError" runat="server" Width="590px" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
