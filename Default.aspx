<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ACTAWeb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Logout.js"></script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <script language="javascript" type="text/javascript">
        window.onload = startApp; // open in new window
        
        function startApp()
        {
            try
            {
                //window.location.href = '/ACTAWeb/ACTAWebUI/ACTAWebPage.aspx'; // for testing without opening application in pop up
                
                var wOpen;
                var wOptions;

                wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
                // window should be in maximized mode
                wOptions = wOptions + ',width=' + screen.availWidth;
                wOptions = wOptions + ',height=' + screen.availHeight;
                wOptions = wOptions + ',left=0,top=0';

                wOpen = window.open('/ACTAWeb/ACTAWebUI/ACTAWebPage.aspx', 'ACTAWeb', wOptions);
                                
                if (wOpen != null)
                {
                    wOpen.focus();
                }
                
                // close this window, this is the way to close without confirmation message            
                window.opener='X';
                window.open('','_parent','');
                window.close();
            }
            catch(e) { alert(e); }            
        }
    </script>
    
    <form id="form1" method="post" runat="server">
    <div>
        <%--<asp:Table ID="Table2" runat="server" Width="795px" HorizontalAlign="Center" CssClass="navigationTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="795px">
                <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="Right" Width="795px" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnExit" runat="server" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table3" runat="server" Width="795px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow3" runat="server" Width="795px">
                <asp:TableCell ID="TableCell3" runat="server" Width="795px" CssClass="hdrCell">
                    <asp:Label ID="lblACTAWeb" runat="server" Width="785px" CssClass="hdrTitle"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table1" runat="server" Width="795px" Height="600px" HorizontalAlign="Center" CssClass="contentTable">            
            <asp:TableRow ID="TableRow1" runat="server" Width="795px">
                <asp:TableCell ID="TableCell1" runat="server" Width="795px" CssClass="contentMenuCell">                    
                    <asp:Menu ID="Menu" runat="server" Orientation="Horizontal" CssClass="menu">
                              <DynamicMenuItemStyle CssClass="menuItem" />
                              <StaticMenuItemStyle CssClass="menuItem" />
                              <StaticHoverStyle CssClass="menuItemHover" />
                              <DynamicHoverStyle CssClass="menuItemHover" />                              
                        <Items>
                            <asp:MenuItem  Text="Passes" NavigateUrl="/ACTAWeb/ACTAWebUI/PassesSearch.aspx?Back=/ACTAWeb/Default.aspx" Value="Passes"></asp:MenuItem>
                            <asp:MenuItem Text="Permissions" NavigateUrl="/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx?Back=/ACTAWeb/Default.aspx" Value="Permissions"></asp:MenuItem>
                            <asp:MenuItem Text="Absences" NavigateUrl="/ACTAWeb/ACTAWebUI/WholeDayAbsencesSearch.aspx?Back=/ACTAWeb/Default.aspx" Value="Absences"></asp:MenuItem>
                            <asp:MenuItem Text="Presence Report" NavigateUrl="/ACTAWeb/ReportsWeb/EmployeePresenceReport.aspx?Back=/ACTAWeb/Default.aspx" Value="PresenceReport"></asp:MenuItem>
                            <asp:MenuItem Text="Design test" NavigateUrl="/ACTAWeb/ACTAWebUI/DesignTestPage.aspx?Back=/ACTAWeb/Default.aspx" Value="DesignTest"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow5" runat="server" Width="795px">
                <asp:TableCell ID="TableCell5" runat="server" Width="795px" CssClass="contentCell">                
                    <asp:Image ID="img" runat="server" ImageAlign="AbsMiddle" Width="785px" ImageUrl="/ACTAWeb/CommonWeb/images/ACTABackgroundImg.jpg" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>        
        <asp:Table ID="Table4" runat="server" Width="795px" HorizontalAlign="Center" CssClass="footerTable">
            <asp:TableRow ID="TableRow4" runat="server" Width="795px">
                <asp:TableCell ID="TableCell4" runat="server" Width="795px" CssClass="footerCell">
                    <asp:Label ID="lblLoggedIn" runat="server" Width="785px" CssClass="loginUserText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>--%>
    </div>
    </form>
</body>
</html>