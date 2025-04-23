<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ADAMLogin.aspx.cs" Inherits="ACTAWebUI.ADAMLogin" %>

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
        <asp:Table ID="headerLogInTable" runat="server" Width="600px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="loginRow" runat="server" Width="600px">
                <asp:TableCell ID="logInCol" runat="server" Width="600px" CssClass="hdrCell">
                    <asp:Label ID="lblLogin" runat="server" Width="590px" CssClass="hdrTitle"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="logInTable" runat="server" Width="600px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow4" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="userNameRow" runat="server" Width="600px">
                <asp:TableCell ID="userNameCol" runat="server" Width="150px" CssClass="contentCell">
                    <asp:Label ID="lblUserName" runat="server" Width="145px" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="userNameValueCol" runat="server" Width="300px" CssClass="contentCell">
                    <asp:TextBox ID="tbUserName" runat="server" Width="290px" CssClass="contentTb"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell3" runat="server" Width="150px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="passwordRow" runat="server" Width="600px">
                <asp:TableCell ID="passwordCol" runat="server" Width="150px" CssClass="contentCell">
                    <asp:Label ID="lblPassword" runat="server" Width="145px" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="passwordValueCol" runat="server" Width="300px" CssClass="contentCell">
                    <asp:TextBox ID="tbPassword" runat="server" Width="290px" TextMode="Password" CssClass="contentTb"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="150px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow ID="TableRow1" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="buttonRow" runat="server" Width="600px">
                <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="contentCell"></asp:TableCell>
                <asp:TableCell ID="okCol" runat="server" Width="300px" HorizontalAlign="Center" CssClass="contentCell">
                    <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
                <asp:TableCell ID="TableCell7" runat="server" Width="150px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow2" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table" runat="server" Width="600px" HorizontalAlign="Center" CssClass="footerTable">
            <asp:TableRow ID="errorRow" runat="server" Width="600px">
                <asp:TableCell ID="errorCol" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblError" runat="server" Width="590px" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
    </div>
    </form>
</body>
</html>
