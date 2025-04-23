<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MCVisitHistoryPage.aspx.cs" Inherits="ACTAWebUI.MCVisitHistoryPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>    
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
    <base target="_self" />
</head>
<body onload="document.body.style.cursor = 'default'">
    <script language="javascript" type="text/javascript">        
        //-------------------------------------------------------------
        //----Select highlish rows when the checkboxes are selected
        //
        // Note: The colors are hardcoded, however you can use 
        //       RegisterClientScript blocks methods to use Grid's
        //       ItemTemplates and SelectTemplates colors.
        //		 for ex: grdEmployees.ItemStyle.BackColor OR
        //				 grdEmployees.SelectedItemStyle.BackColor
        //-------------------------------------------------------------

        //parent document must have hidden element SelBox!!!!
        function highlightRow(chb)
        {
            try
            {
                var xState = chb.checked;                
                if (xState)
                {
                    chb.parentElement.parentElement.style.color = chb.parentElement.parentElement.style.backgroundColor; // save color of unselected item
                    chb.parentElement.parentElement.style.backgroundColor = '#FFFFD1'; // hardcoded from selected item color from constants
                    document.getElementById('selectedKeys').value = chb.value + '|';                            
                }
                else
                {
                    chb.parentElement.parentElement.style.backgroundColor = chb.parentElement.parentElement.style.color; // save color of unselected item
                    chb.parentElement.parentElement.style.color = 'black';
                    // if selected keys contains selected value, remove it from selected keys                            
                    var index = document.getElementById('selectedKeys').value.indexOf(chb.value + '|')                            
                    if (index >= 0)
                    {
                        var firstSelPart = document.getElementById('selectedKeys').value.substr(0, index);                                
                        var length = document.getElementById('selectedKeys').value.length - firstSelPart.length - chb.value.length - 1;                                
                        var lastSelPart = document.getElementById('selectedKeys').value.substr(index + chb.value.length + 1, length);                                
                        document.getElementById('selectedKeys').value = firstSelPart + lastSelPart;                                
                    }                            
                }
                
                document.getElementById('btnPostBack').click();
            }
            catch(e) { alert(e); }
        }        
    </script>
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:Table ID="Table5" runat="server" Width="1000px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow4" runat="server" Width="1000px">
                <asp:TableCell ID="TableCell3" runat="server" Width="950px" HorizontalAlign="Center" CssClass="hdrCell">
                    <asp:Label ID="lblTitle" runat="server" CssClass="loginUserTextLeft"></asp:Label>                    
                </asp:TableCell>
                <asp:TableCell ID="TableCell6" runat="server" Width="50px" CssClass="hdrCell">                    
                    <asp:Button ID="btnPostBack" runat="server" CssClass="contentInvisibleBtn"></asp:Button>                    
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1000px" HorizontalAlign="Center" CssClass="tabTable">            
            <asp:TableRow ID="TableRow2" runat="server" Width="1000px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow14" runat="server" Width="1000px">
                <asp:TableCell ID="TableCell26" runat="server" Width="1000px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                     <asp:Table ID="hdrTable" runat="server" Width="990px" HorizontalAlign="Center" CssClass="hdrListTable">
                        <asp:TableRow ID="hdrRow" runat="server" Width="990px" CssClass="hdrListRow">
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow7" runat="server" Width="1000px">
                <asp:TableCell ID="TableCell7" runat="server" Width="1000px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                    <asp:Table ID="Table2" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabTable">
                        <asp:TableRow ID="TableRow10" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell8" runat="server" Width="1000px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Panel ID="hdrPanel" runat="server" Width="980px" Height="300px" ScrollBars="Auto" CssClass="resultPanel">
                                    <asp:DataGrid ID="hdrGrid" runat="server" Width="960px" AutoGenerateColumns="false"
                                        ItemStyle-CssClass="resultItem" AlternatingItemStyle-CssClass="resultAltItem" 
                                        OnItemDataBound="hdrGrid_ItemDataBound" CssClass="resultGrid">
                                        <Columns>
                                            <asp:TemplateColumn ItemStyle-CssClass="resultSelCol">
                                                <ItemTemplate>
                                                    <input id="chbSel" type="checkbox" runat="server" onclick="highlightRow(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </asp:Panel>                    
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>                    
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow9" runat="server" Width="1000px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow8" runat="server" Width="1000px">
                <asp:TableCell ID="TableCell19" runat="server" Width="1000px" CssClass="tabMidAlignCell">
                    <asp:Label ID="lblVisitDtls" runat="server" Width="980px" CssClass="contentLblLeft"></asp:Label>
                </asp:TableCell>                
            </asp:TableRow>            
            <asp:TableRow ID="TableRow5" runat="server" Width="1000px">
                <asp:TableCell ID="TableCell4" runat="server" Width="1000px" CssClass="tabMidAlignCell">
                    <asp:Table ID="dtlTable" runat="server" Width="990px" HorizontalAlign="Center" CssClass="hdrListTable">
                        <asp:TableRow ID="dtlRow" runat="server" Width="990px" CssClass="hdrListRow">
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>  
            <asp:TableRow ID="TableRow1" runat="server" Width="1000px">
                <asp:TableCell ID="TableCell1" runat="server" Width="1000px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                    <asp:Table ID="Table1" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabTable">
                        <asp:TableRow ID="TableRow6" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="1000px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Panel ID="dtlPanel" runat="server" Width="980px" Height="150px" ScrollBars="Auto" CssClass="resultPanel">
                                    <asp:DataGrid ID="dtlGrid" runat="server" Width="960px" AutoGenerateColumns="false"
                                        ItemStyle-CssClass="resultItem" AlternatingItemStyle-CssClass="resultAltItem" CssClass="resultGrid">
                                    </asp:DataGrid>
                                </asp:Panel>                    
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>                    
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow3" runat="server" Width="1000px">
                <asp:TableCell ID="TableCell2" runat="server" Width="1000px" HorizontalAlign="Right" CssClass="tabMidAlignCell">
                    <asp:Button ID="btnClose" runat="server" CssClass="contentBtn"></asp:Button>
                    <input id="selectedKeys" type="hidden" runat="server" />                    
                </asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>