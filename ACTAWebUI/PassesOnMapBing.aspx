<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassesOnMapBing.aspx.cs" Inherits="ACTAWebUI.PassesOnMapBing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Passes on Map</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <script type="text/javascript"
               src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0">
          </script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body onload="GetMap();">
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
        <asp:Table ID="Table1" runat="server" Width="700px" CssClass="tabNoBorderTable"  HorizontalAlign="Center"
            Height="700px">
            <asp:TableRow ID="TableRow1" runat="server" Width="700px" HorizontalAlign="Center"> 
                <asp:TableCell ID="TableCell1" runat="server" Width="670px" CssClass="tabCell" HorizontalAlign="Center">
                             <div id="myMap"
                                   style="position:relative; width:600px; height:600px;">
                              </div>
                     
                              <asp:Literal ID="Literal1" runat="server">
                              </asp:Literal>
                
                </asp:TableCell>
            </asp:TableRow>
          </asp:Table>

    </form>
</body>
</html>
