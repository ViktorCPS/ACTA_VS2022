<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultPage.aspx.cs" Inherits="ACTAWebUI.DefaultPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
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
                    <%--<asp:LinkButton ID="lbtnMenu" runat="server" OnClick="lbtnMenu_Click" CssClass="navigationLBtn"></asp:LinkButton>--%>
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" HorizontalAlign="Right" Width="600px" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnExit" runat="server" OnClick="LogOut" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table1" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell3" runat="server" Width="200px" CssClass="hdrCell">
                    <asp:DropDownList ID="cbCategory" runat="server" Width="195px" AutoPostBack="true" OnSelectedIndexChanged="cbCategory_SelectedIndexChanged" CssClass="contentDDList"></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="300px" CssClass="hdrCell">
                    <asp:CheckBox ID="chbDefault" runat="server" Width="295px" AutoPostBack="true" OnCheckedChanged="chbDefault_OnCheckChanged" CssClass="contentChb"></asp:CheckBox>
                </asp:TableCell>                
                <asp:TableCell ID="TableCell1" runat="server" Width="700px" CssClass="hdrCell">
                    <asp:Label ID="lblLoggedInUser" runat="server" Width="695px" CssClass="loginUserText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table4" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow10" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell33" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="contentCell">
                    <iframe id="tabAreaIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px" marginwidth="0px" height="630px" class="pageIframe" ></iframe>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table5" runat="server" Width="1200px" HorizontalAlign="Center" CssClass="footerTable">
            <asp:TableRow ID="TableRow7" runat="server" Width="1200px">
                <asp:TableCell ID="TableCell39" runat="server" Width="1000px" CssClass="footerCell">
                    <asp:Label ID="lblInformation" runat="server" Width="1095px" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" Width="180px" HorizontalAlign="Right" CssClass="footerCell">
                    <asp:Image ID="logo" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/WebLogo.png"></asp:Image>
                </asp:TableCell>
                <asp:TableCell ID="TableCell6" runat="server" Width="20px" CssClass="footerCell"></asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
    </div>
    </form>    
</body>
</html>
