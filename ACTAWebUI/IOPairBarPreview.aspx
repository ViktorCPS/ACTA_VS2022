<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IOPairBarPreview.aspx.cs" Inherits="ACTAWebUI.IOPairBarPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
     <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:Table ID="Table5" runat="server" Width="800px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow9" runat="server" Width="800px">
                <asp:TableCell ID="TableCell3" runat="server" Width="800px" CssClass="hdrCell">
                    <asp:Label ID="lblEmployee" runat="server" Width="790px" CssClass="loginUserTextLeft"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table4" runat="server" Width="800px" HorizontalAlign="Center" CssClass="tabTable">            
            <asp:TableRow ID="TableRow21" runat="server" Width="800px">
                <asp:TableCell ID="TableCell150" runat="server" Width="800px" CssClass="tabCell">
                    <asp:Table ID="Table2" runat="server" Width="790px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow11" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell14" runat="server" Width="790px" CssClass="tabCell">
                                <asp:Panel ID="legendPanel" runat="server" Width="780px" Height="20px" ScrollBars="Auto" CssClass="resultPanel">
                                    <asp:PlaceHolder ID="legendCtrlHolder" runat="server"></asp:PlaceHolder>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow36" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell50" runat="server" Width="790px" CssClass="tabCell">                            
                                <asp:Panel ID="resultPanel" runat="server" Width="780px" Height="480px" ScrollBars="Auto" CssClass="resultPanel">
                                    <asp:PlaceHolder ID="ctrlHolder" runat="server"></asp:PlaceHolder>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow3" runat="server" Width="800px">
                <asp:TableCell ID="TableCell4" runat="server" Width="800px" CssClass="tabCell">                    
                    <asp:Table ID="Table3" runat="server" Width="790px" CssClass="tabTable">                        
                        <asp:TableRow ID="TableRow8" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell11" runat="server" Width="640px" HorizontalAlign="Center" CssClass="hdrCell">
                                <asp:Label ID="lblDate" runat="server" Width="100px" CssClass="contentGraphPeriodLbl"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell9" runat="server" Width="150px" HorizontalAlign="Right" CssClass="tabCell">
                                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
    </div>
    </form>
</body>
</html>
