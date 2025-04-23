<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ACTAWeb.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
</head>
<body>
    <script language="javascript" type="text/javascript">
        function mouseDown(e) {
            try { if (event.button == 2 || event.button == 3) { return false; } }
            catch (e) { if (e.which == 3) { return false; } }
        }
        
        document.oncontextmenu = function() { return false; }
        document.onmousedown = mouseDown;
        window.onload = closeWindows();
        
        function closeWindows()
        {
            try
            {            
                openerWindow = window.dialogArguments;
                if (openerWindow != null)
                {
                    //alert('found pop up opener');
                    window.close();
                    //alert('closed pop up');                
                    openerWindow.location.href = '/ACTAWeb/Login.aspx?sessionExpired=true';
                    //alert('reload popup opener');
                }            
                else
                {
                    var childWin = window;                
                    parentPage = childWin.parent;
                    var i = 0;
                    var rootParent = 0;
                    
                    if (parentPage != null && parentPage.location.href.indexOf('Login.aspx') < 0 && parentPage.location.href.indexOf('ACTAWebPage.aspx') < 0)
                    {
                        while (parentPage != null && childWin != null && rootParent == 0)
                        {                
                            //alert("current window: " + childWin.location.href);
                            //alert("parent:" + parentPage.location.href);
                            if (parentPage.location.href.indexOf('ACTAWebPage.aspx') < 0 && i < 20)
                            {
                                //alert(i + 'no root parent');
                                childWin = parentPage;
                                parentPage = childWin.parent;
                                i++;
                            }
                            else
                            {
                                //alert(i + ': found root parent');
                                rootParent = 1;
                            }
                        }
                       
                        childWin.location.href = '/ACTAWeb/Login.aspx?sessionExpired=true';
                        //alert('reload parent');
                    }
                }
                
                if (parentPage != null)
                {
                    var login = parentPage.document.getElementById('loginScreen');
                
                    if (login != null)
                    {
                        login.value = 'false';
                    }
                }
            } 
            catch(e) { alert(e); }
        }
    </script>
    <form id="form1" runat="server" method="post">
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
            <asp:TableRow ID="TableRow7" runat="server" Width="600px">
                <asp:TableCell ID="TableCell11" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblFiatLoginSR" runat="server" Width="590px" CssClass="contentInfoLbl"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow8" runat="server" Width="600px">
                <asp:TableCell ID="TableCell12" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblFiatLoginEN" runat="server" Width="590px" CssClass="contentInfoLbl"></asp:Label>
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
            <asp:TableRow ID="changePasswordRow" runat="server" Width="600px">
                <asp:TableCell ID="TableCell1" runat="server" Width="150px" CssClass="contentCell"></asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" Width="300px" CssClass="contentCell">
                    <asp:CheckBox ID="chbChangePassword" runat="server" Width="290px" CssClass="contentChb"></asp:CheckBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="150px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow9" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow30" runat="server" Width="600px">
                <asp:TableCell ID="TableCell30" runat="server" Width="150px" CssClass="tabMidAlignCell">
                    <asp:RadioButton ID="rbFiat" runat="server" AutoPostBack="true" OnCheckedChanged="rbFiat_CheckedChanged" CssClass="contentRb" />
                </asp:TableCell>                                                     
            </asp:TableRow>
            <asp:TableRow ID="TableRow6" runat="server" Width="600px">
                <asp:TableCell ID="TableCell10" runat="server" Width="150px" CssClass="tabMidAlignCell">
                    <asp:RadioButton ID="rbTM" runat="server" AutoPostBack="true" OnCheckedChanged="rbTM_CheckedChanged" CssClass="contentRb" />
                </asp:TableCell>                                                     
            </asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="buttonRow" runat="server" Width="600px">
                <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="contentCell"></asp:TableCell>
                <asp:TableCell ID="okCol" runat="server" Width="300px" HorizontalAlign="Center" CssClass="contentCell">
                    <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" CssClass="contentBtn"></asp:Button>
                    <asp:Button ID="btnReadTag" runat="server" OnClick="btnReadTag_Click" CssClass="contentBtn"></asp:Button>
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
            <asp:TableRow ID="TableRow3" runat="server" Width="600px">
                <asp:TableCell ID="TableCell8" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblInfoSR" runat="server" Width="590px" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow5" runat="server" Width="600px">
                <asp:TableCell ID="TableCell9" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblInfoENG" runat="server" Width="590px" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
