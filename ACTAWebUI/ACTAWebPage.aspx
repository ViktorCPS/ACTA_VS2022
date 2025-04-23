<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ACTAWebPage.aspx.cs" Inherits="ACTAWebUI.ACTAWebPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <script language="javascript" type="text/javascript">
        window.onbeforeunload = endApp;
        
        function endApp()
        {
            try
            {
                var login = document.getElementById('loginScreen');
                
                if (login != null)
                {
                    if (login.value == 'true')
                    {                        
                        var wOpen;
                        var wOptions;

                        wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
                        // window should be in maximized mode
                        wOptions = wOptions + ',width=' + screen.availWidth;
                        wOptions = wOptions + ',height=' + screen.availHeight;
                        wOptions = wOptions + ',left=0,top=0';

                        wOpen = window.open('/ACTAWeb/ACTAWebUI/LogOutPage.aspx', 'ACTAWeb', wOptions);
                                        
                        if (wOpen != null)
                        {                     
                            wOpen.focus();
                        }
                    }
                }            
            }
            catch(e) { alert(e); }            
        }
    </script>    
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table4" runat="server" Width="1250px" HorizontalAlign="Center">
            <asp:TableRow ID="TableRow10" runat="server" Width="1250px">
                <asp:TableCell ID="TableCell33" runat="server" Width="1250px" HorizontalAlign="Center">
                    <iframe id="resultIframe" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                        marginwidth="0px" height="750px" src="/ACTAWeb/ACTAWebUI/DefaultPage.aspx" class="pageIframe">
                    </iframe>
                    <input id="loginScreen" type="hidden" runat="server" value="true" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
