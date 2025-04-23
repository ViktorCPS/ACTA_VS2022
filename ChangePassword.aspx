<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="ACTAWeb.ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
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
        <asp:Table ID="Table1" runat="server" Width="600px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="600px">
                <asp:TableCell ID="TableCell18" runat="server" Width="600px" CssClass="hdrCell">
                    <asp:Label ID="lblChangePassword" runat="server" Width="590px" CssClass="hdrTitle"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table2" runat="server" Width="600px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="passwordRow" runat="server" Width="600px">
                <asp:TableCell ID="passwordCol" runat="server" Width="200px" CssClass="contentCell">
                    <asp:Label ID="lblPassword" runat="server" Width="195px" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="passwordValueCol" runat="server" Width="300px" CssClass="contentCell">
                    <%--<asp:TextBox ID="tbPassword" runat="server" Width="290px" TextMode="Password" CssClass="contentTb"></asp:TextBox>--%>
                    <opp:PasswordTextBox id="tbPassword" runat="server" Width="290" MaxLength=10 CssClass="contentTb" />
                </asp:TableCell>
                <asp:TableCell ID="TableCell1" runat="server" Width="100px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="confirmPasswordRow" runat="server" Width="600px">
                <asp:TableCell ID="confirmPasswordCol" runat="server" Width="200px" CssClass="contentCell">
                    <asp:Label ID="lblConfirmPassword" runat="server" Width="195px" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="confirmPasswordValueCol" runat="server" Width="300px" CssClass="contentCell">
                    <%--<asp:TextBox ID="tbConfirmPassword" runat="server" Width="290px" TextMode="Password" CssClass="contentTb"></asp:TextBox>--%>
                    <opp:PasswordTextBox id="tbConfirmPassword" runat="server" Width="290" MaxLength=10 CssClass="contentTb" />
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="100px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow5" runat="server" Width="600px">
                <asp:TableCell ID="TableCell3" runat="server" Width="200px" CssClass="contentCell">
                    <asp:Label ID="lblLangCode" runat="server" Width="195px" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="300px" CssClass="contentCell">
                    <asp:Table ID="Table3" runat="server" Width="300px" HorizontalAlign="Center" CssClass="tabNoBorder">
                        <asp:TableRow ID="TableRow6" runat="server" Width="300px">
                            <asp:TableCell ID="TableCell8" runat="server" Width="150px" CssClass="contentCell">
                                <asp:RadioButton ID="rbSr" runat="server" CssClass="contentRb" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell9" runat="server" Width="150px" CssClass="contentCell">
                                <asp:RadioButton ID="rbEn" runat="server" CssClass="contentRb" />
                            </asp:TableCell>
                        </asp:TableRow>                        
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" Width="100px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow4" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="buttonRow" runat="server" Width="600px">
                <asp:TableCell ID="TableCell6" runat="server" Width="200px" HorizontalAlign="Right" CssClass="contentCell">
                    <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
                <asp:TableCell ID="okCol" runat="server" Width="300px" HorizontalAlign="Right" CssClass="contentCell">
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
                <asp:TableCell ID="TableCell7" runat="server" Width="100px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow3" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>        		
        </asp:Table>
        <asp:Table ID="Table5" runat="server" Width="600px" HorizontalAlign="Center" CssClass="footerTable">
            <asp:TableRow ID="TableRow7" runat="server" Width="600px">
                <asp:TableCell ID="TableCell39" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblError" runat="server" Width="590px" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow" runat="server" Width="600px">
                <asp:TableCell ID="TableCell32" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblLoggedInUser" runat="server" Width="590px" CssClass="loginUserText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
