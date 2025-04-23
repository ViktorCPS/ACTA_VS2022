<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmallResultPage.aspx.cs"
    Inherits="ACTAWebUI.SmallResultPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>
    <script language="javascript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Logout.js"></script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body>

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
        function highlightRow(chb)
        {
            try
            {
                var xState = chb.checked;
                if (parent.document != null) {
                    if (parent.document.getElementById('SelBox') != null) {
                        if (xState) {
                            elm = chb.form.elements;

                            for (i = 0; i < elm.length; i++) {
                                if (elm[i].type == "checkbox" && elm[i].id != chb.id) {
                                    if (elm[i].checked == xState)
                                        elm[i].click();
                                }
                            }
                            chb.parentElement.parentElement.style.color = chb.parentElement.parentElement.style.backgroundColor; // save color of unselected item
                            chb.parentElement.parentElement.style.backgroundColor = '#FFFFD1'; // hardcoded from selected item color from constants
                            //document.getElementById('selectedKeys').value += chb.value + '|';
                            //alert(document.getElementById('selectedKeys').value);  // set value which should be added to selected keys                                                      

                            parent.document.getElementById('SelBox').value = chb.value + '|';


                        }
                        else {
                            chb.parentElement.parentElement.style.backgroundColor = chb.parentElement.parentElement.style.color; // save color of unselected item
                            chb.parentElement.parentElement.style.color = 'black';
                            parent.document.getElementById('SelBox').value = "";
                            //                    // if selected keys contains selected value, remove it from sele                    
                            //                    //alert(document.getElementById('selectedKeys').value);
                            //                    var index = parent.document.getElementById('SelBox').value.indexOf(chb.value + '|')
                            //                    //alert('index ' + index);
                            //                    if (index >= 0) 
                            //                    {
                            //                        var firstSelPart = parent.document.getElementById('SelBox').value.substr(0, index);
                            //                        //alert('first part ' + firstSelPart + 'first length ' + firstSelPart.length);
                            //                        var length = parent.document.getElementById('SelBox').value.length - firstSelPart.length - chb.value.length - 1;
                            //                        //alert('length ' + length);
                            //                        var lastSelPart = parent.document.getElementById('SelBox').value.substr(index + chb.value.length + 1, length);
                            //                        //alert ('last part ' + lastSelPart);
                            //                        parent.document.getElementById('SelBox').value = firstSelPart + lastSelPart;

                            //                        parent.document.getElementById('SelBox').value = firstSelPart + lastSelPart;

                            //                    }
                            //alert(document.getElementById('selectedKeys').value);
                        }
                    }
                }
            }
            catch(e) { alert(e); }
        }

        //-------------------------------------------------------------
        // Select all the checkboxes (Hotmail style)	    
        //-------------------------------------------------------------
        function selectAll(spanChk)
        {
            try
            {
                // Added as ASPX uses SPAN for checkbox 
                var xState = spanChk.checked;
                var theBox = spanChk;

                elm = theBox.form.elements;
                for (i = 0; i < elm.length; i++) {
                    if (elm[i].type == "checkbox" && elm[i].id != theBox.id) {
                        if (elm[i].checked != xState)
                            elm[i].click();
                    }
                }
            }
            catch(e) { alert(e); }
        }         
    </script>

    <form id="form1" runat="server">
    <div>
        <asp:Table ID="hdrTable" runat="server" Width="340px" HorizontalAlign="Center" CssClass="hdrListTable">
            <asp:TableRow ID="hdrRow" runat="server" Width="340px" CssClass="hdrListRow">
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table2" runat="server" Width="340px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="340px" CssClass="contentRow">
                <asp:TableCell ID="TableCell4" runat="server" Width="340px" CssClass="contentCell">
                    <asp:Panel ID="resultPanel" runat="server" Width="325px" Height="210px" ScrollBars="Auto"
                        CssClass="resultPanel">
                        <asp:DataGrid ID="resultGrid" runat="server" Width="305px" AutoGenerateColumns="false"
                            ItemStyle-CssClass="resultItem" AlternatingItemStyle-CssClass="resultAltItem"
                            OnItemDataBound="resultGrid_ItemDataBound" CssClass="resultGrid">
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
                <asp:TableCell ID="TableCell1" runat="server" Width="350px" CssClass="footerCell">
                    <input id="selectedKeys" type="hidden" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
