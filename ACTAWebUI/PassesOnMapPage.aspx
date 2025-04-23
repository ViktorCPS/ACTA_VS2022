<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassesOnMapPage.aspx.cs" Inherits="ACTAWebUI.PassesOnMapPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Passes on Map</title>
    <script type="text/javascript"
     src="https://maps.googleapis.com/maps/api/js?key=AIzaSyByNWkS3aLbDFYxiRHq6GVW0jySPFVB4l8&sensor=false">
          </script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body onload="initialize()">
    <script language="javascript" type="text/javascript">
        function pagePostBack(btnTree) {
            try {
                var btn = document.getElementById(btnTree);

                if (btn != null) {
                    __doPostBack(btn, '');
                }
            }
            catch (e) { alert(e); }
        }
    </script>
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
                    <div id="mapArea" style="width: 990px; height: 480px;" HorizontalAlign="Center">            
                    </div>
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
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
