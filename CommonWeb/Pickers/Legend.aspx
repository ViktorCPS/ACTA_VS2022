<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Legend.aspx.cs" Inherits="CommonWeb.Pickers.Legend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:Table ID="Table5" runat="server" Width="800px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow9" runat="server" Width="800px">
                <asp:TableCell ID="TableCell3" runat="server" Width="800px" CssClass="hdrCell">
                    <asp:Label ID="lblLegend" runat="server" Width="790px" CssClass="loginUserTextLeft"></asp:Label>
                </asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table4" runat="server" Width="800px" HorizontalAlign="Center" CssClass="tabTable">            
            <asp:TableRow ID="TableRow11" runat="server" Width="800px">
                <asp:TableCell ID="TableCell14" runat="server" Width="800px" CssClass="tabCell">
                    <asp:Panel ID="legendPanel" runat="server" Width="790px" Height="500px" ScrollBars="Auto" CssClass="resultPanel">
                        <asp:PlaceHolder ID="legendCtrlHolder" runat="server"></asp:PlaceHolder>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow8" runat="server" Width="800px">                
                <asp:TableCell ID="TableCell9" runat="server" Width="800px" HorizontalAlign="Right" CssClass="tabCell">
                    <asp:Button ID="btnClose" runat="server" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
            </asp:TableRow> 
        </asp:Table>        
    </div>
    </form>
</body>
</html>
