<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplOnDemandAppointmentsPage.aspx.cs" Inherits="ACTAWebUI.EmplOnDemandAppointmentsPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body onload="document.body.style.cursor = 'default'">    
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table15" runat="server" Width="990px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="990px">
                <asp:TableCell ID="TableCell151" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">                        
                    <asp:Table ID="Table3" runat="server" Width="990px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblEmplData" runat="server" Width="900px" CssClass="hdrTitle"></asp:Label>
                            </asp:TableCell>                            
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow20" runat="server" Width="990px">
                <asp:TableCell ID="TableCell28" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">                        
                    <asp:Table ID="Table7" runat="server" Width="990px" CssClass="tabTable">                                                
                        <asp:TableRow ID="TableRow9" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell29" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Table ID="Table8" runat="server" Width="980px" CssClass="tabNoBorderTable">
                                    <asp:TableRow ID="TableRow4" runat="server" Width="980px">
                                        <asp:TableCell ID="TableCell3" runat="server" Width="450px" CssClass="tabMidAlignCell">
                                            <asp:Label ID="lblPosition" runat="server" Width="350px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell4" runat="server" Width="80px" CssClass="tabMidAlignCell"></asp:TableCell>
                                        <asp:TableCell ID="TableCell7" runat="server" Width="450px" CssClass="tabMidAlignCell"></asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow6" runat="server" Width="980px">
                                        <asp:TableCell ID="TableCell8" runat="server" Width="450px" CssClass="tabMidAlignCell">
                                            <asp:DropDownList ID="cbPosition" runat="server" Width="350px" AutoPostBack="true" OnSelectedIndexChanged="cbPosition_SelectedIndexChanged" CssClass="contentDDList"></asp:DropDownList>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell9" runat="server" Width="80px" CssClass="tabMidAlignCell"></asp:TableCell>
                                        <asp:TableCell ID="TableCell10" runat="server" Width="450px" CssClass="tabMidAlignCell"></asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow1" runat="server" Width="980px">
                                        <asp:TableCell ID="TableCell1" runat="server" Width="450px" CssClass="tabMidAlignCell">
                                            <asp:Label ID="lblRisks" runat="server" Width="350px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell2" runat="server" Width="80px" CssClass="tabMidAlignCell">                                            
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell6" runat="server" Width="450px" CssClass="tabMidAlignCell">
                                            <asp:Label ID="lblSelectedRisks" runat="server" Width="350px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow5" runat="server" Width="980px">
                                        <asp:TableCell ID="TableCell18" runat="server" Width="450px" CssClass="tabMidAlignCell">
                                            <asp:ListBox ID="lbRisks" runat="server" Width="350px" SelectionMode="Multiple" Height="350px" OnPreRender="lbRisks_PreRender" CssClass="contentLblLeft"></asp:ListBox>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell33" runat="server" Width="80px" CssClass="tabMidAlignCell">
                                            <asp:Table ID="Table9" runat="server" Width="80px" CssClass="tabNoBorderTable">
                                                <asp:TableRow ID="TableRow23" runat="server" Width="80px">
                                                    <asp:TableCell ID="TableCell35" runat="server" Width="80px" CssClass="tabBottomAlignCell">
                                                        <asp:Button ID="btnAdd" runat="server" Text=">" OnClick="btnAdd_Click" CssClass="contentBtn"></asp:Button>
                                                    </asp:TableCell>                                                    
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow7" runat="server" Width="80px" CssClass="contentSeparatorRow"></asp:TableRow>
                                                <asp:TableRow ID="TableRow24" runat="server" Width="80px">
                                                    <asp:TableCell ID="TableCell36" runat="server" Width="80px" CssClass="tabCell">
                                                        <asp:Button ID="btnRemove" runat="server" Text="<" OnClick="btnRemove_Click" CssClass="contentBtn"></asp:Button>
                                                    </asp:TableCell>                                                    
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell34" runat="server" Width="450px" CssClass="tabMidAlignCell">
                                            <asp:ListBox ID="lbSelectedRisks" runat="server" Width="350px" SelectionMode="Multiple" Height="350px" OnPreRender="lbSelectedRisks_PreRender" CssClass="contentLblLeft">
                                                        </asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>                
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow8" runat="server" Width="990px" CssClass="contentSeparatorRow"></asp:TableRow>
                        <asp:TableRow ID="TableRow10" runat="server" Width="990px" CssClass="contentSeparatorRow"></asp:TableRow>
                        <asp:TableRow ID="TableRow21" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell30" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="contentBtn"></asp:Button>                                
                            </asp:TableCell>                
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow22" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell31" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblError" runat="server" Width="970px" CssClass="errorText"></asp:Label>
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