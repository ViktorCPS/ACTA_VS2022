<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultPage.aspx.cs" Inherits="ACTAWebUI.ResultPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <script language="javascript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Logout.js"></script>
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

        //parent document must have hidden element SelBox!!!!
        function highlightRow(chb)
        {
            try
            {
                var xState = chb.checked;
                if (parent.document != null)
                {
                    if (parent.document.getElementById('SelBox') != null)
                    {
                        if (xState)
                        {
                            chb.parentElement.parentElement.style.color = chb.parentElement.parentElement.style.backgroundColor; // save color of unselected item
                            chb.parentElement.parentElement.style.backgroundColor = '#FFFFD1'; // hardcoded from selected item color from constants
                            document.getElementById('selectedKeys').value += chb.value + '|';
                            //alert(document.getElementById('selectedKeys').value);  // set value which should be added to selected keys
                            parent.document.getElementById('SelBox').value += chb.value + '|';
                        }
                        else
                        {
                            chb.parentElement.parentElement.style.backgroundColor = chb.parentElement.parentElement.style.color; // save color of unselected item
                            chb.parentElement.parentElement.style.color = 'black';
                            // if selected keys contains selected value, remove it from selected keys
                            //alert(document.getElementById('selectedKeys').value);
                            var index = parent.document.getElementById('SelBox').value.indexOf(chb.value + '|')
                            //alert('index ' + index);
                            if (index >= 0)
                            {
                                var firstSelPart = parent.document.getElementById('SelBox').value.substr(0, index);
                                //alert('first part ' + firstSelPart + 'first length ' + firstSelPart.length);
                                var length = parent.document.getElementById('SelBox').value.length - firstSelPart.length - chb.value.length - 1;
                                //alert('length ' + length);
                                var lastSelPart = parent.document.getElementById('SelBox').value.substr(index + chb.value.length + 1, length);
                                //alert ('last part ' + lastSelPart);
                                document.getElementById('selectedKeys').value = firstSelPart + lastSelPart;
                                parent.document.getElementById('SelBox').value = firstSelPart + lastSelPart;
                            }
                            //alert(document.getElementById('selectedKeys').value);
                        }
                        
                        // if selection should reflect on parent page, paren page must have hidden field SelRecord and hidden button btnPostBack
                        if (parent.document.getElementById('SelRecord') != null && parent.document.getElementById('btnPostBack') != null)
                        {
                            // set last selected record to parent hidden field and force parent postback
                            var selVal = parent.document.getElementById('SelBox').value;
                            selVal = selVal.substring(0, selVal.length - 1);
                            selVal = selVal.substring(selVal.lastIndexOf('|') + 1);                            
                            if (selVal.length > 0)
                            {                                
                                parent.document.getElementById('SelRecord').value = selVal;
                            }
                            else
                            {                             
                                parent.document.getElementById('SelRecord').value = '-1';
                            }
                                                 
                            parent.document.getElementById('btnPostBack').click();
                        }
                    }                    
                }
            }
            catch(e) { alert(e); }
        }
        
        //parent document must have hidden element ChangedBox!!!!
        // id is unique id from row, employee_id for example
        // col is index of column in which is elemnet that is changed
        // element is changed element
        function changedRow(id, col, element)
        {
            try
            {
                if (parent.document != null)
                {                                    
                    if (parent.document.getElementById('ChangedBox') != null)
                    {
                        var index = parent.document.getElementById('ChangedBox').value.indexOf(id + '|' + col + '|');
                        //alert('index ' + index);
                        if (index >= 0)
                        {
                            var firstSelPart = parent.document.getElementById('ChangedBox').value.substr(0, index);
                            //alert('first part ' + firstSelPart + 'first length ' + firstSelPart.length);
                            var lastIndex = parent.document.getElementById('ChangedBox').value.indexOf('~', index);
                            var lastSelPart = parent.document.getElementById('ChangedBox').value.substr(lastIndex + 1);
                            //alert ('last index ' + lastIndex + 'last part ' + lastSelPart);
                            document.getElementById('changedKeys').value = firstSelPart + id + "|" + col + '|' + element.value + '~' + lastSelPart;
                            parent.document.getElementById('ChangedBox').value = firstSelPart + id + "|" + col + '|' + element.value + '~' + lastSelPart;
                        }
                        else
                        {
                            document.getElementById('changedKeys').value += id + "|" + col + '|' + element.value + '~';
                            parent.document.getElementById('ChangedBox').value += id + "|" + col + '|' + element.value + '~';
                        }
                        //alert('ChangedBoxNew: ' + parent.document.getElementById('ChangedBox').value);
                    }
                }
            }
            catch(e) { alert(e); }
        }        
        
        function setSelectedKeys()
        {
            try
            {                
                if (parent.document != null)
                {
                    if (parent.document.getElementById('SelBox') != null && document.getElementById('selectedKeys') != null)
                    {
                        document.getElementById('selectedKeys').value = parent.document.getElementById('SelBox').value;
                    }
                }
            }
            catch(e) { alert(e); }
        }
        
        function setChangedKeys()
        {
            try
            {
                if (parent.document != null)
                {
                    if (parent.document.getElementById('ChangedBox') != null && document.getElementById('changedKeys') != null)
                    {
                        document.getElementById('changedKeys').value = parent.document.getElementById('ChangedBox').value;
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
        <asp:Table ID="hdrTable" runat="server" Width="990px" HorizontalAlign="Center" CssClass="hdrListTable">
            <asp:TableRow ID="hdrRow" runat="server" Width="990px" CssClass="hdrListRow">
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table2" runat="server" Width="990px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="990px" CssClass="contentRow">
                <asp:TableCell ID="TableCell4" runat="server" Width="990px" CssClass="contentCell">
                    <asp:Panel ID="resultPanel" runat="server" Width="980px" Height="460px" ScrollBars="Auto"
                        CssClass="resultPanel">
                        <asp:DataGrid ID="resultGrid" runat="server" Width="960px" AutoGenerateColumns="false"
                            ItemStyle-CssClass="resultItem" AlternatingItemStyle-CssClass="resultAltItem" OnUnload="resultGrid_Unload"
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
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="footerTable" runat="server" Width="990px" HorizontalAlign="Center" CssClass="footerTable">
            <asp:TableRow ID="footerRow" runat="server" Width="990px" HorizontalAlign="Left" CssClass="footerRow">
                <asp:TableCell ID="TableCell7" runat="server" Width="25px" CssClass="footerCell">
                    <asp:LinkButton ID="lbtnPrev" runat="server" Text="<" OnClick="lbtnPrev_Click" CssClass="contentLBtn"></asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" Width="25px" CssClass="footerCell">
                    <asp:LinkButton ID="lbtnNext" runat="server" Text=">" OnClick="lbtnNext_Click" CssClass="contentLBtn"></asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell6" runat="server" Width="130px" CssClass="footerCell">
                    <asp:Label ID="lblGoToPage" runat="server" Width="125px" CssClass="contentBoldLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell8" runat="server" Width="160px" CssClass="footerCell">
                    <asp:DropDownList ID="cbPage" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="cbPage_SelectedIndexChanged"
                        CssClass="contentDDList">
                    </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ID="TableCell1" runat="server" Width="350px" CssClass="footerCell">                    
                    <input id="selectedKeys" type="hidden" runat="server" />
                    <input id="changedKeys" type="hidden" runat="server" />
                </asp:TableCell>
                <asp:TableCell ID="TableCell9" runat="server" Width="200px" CssClass="footerCell">
                    <asp:Label ID="lblTotal" runat="server" Width="195px" CssClass="contentBoldLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell10" runat="server" Width="100px" CssClass="footerCell">
                    <asp:Label ID="lblTotalCount" runat="server" Width="95px" Text="0" CssClass="contentBoldLbl"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
