<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HRSSCCountersPage.aspx.cs" Inherits="ACTAWebUI.HRSSCCountersPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body onload="document.body.style.cursor = 'default'">
    <script language="javascript" type="text/javascript">
        function pagePostBack(btnTree)
        {
            try
            {
                var btn = document.getElementById(btnTree);
                
                if (btn != null)
                {        
                    __doPostBack(btn,'');
                }
            }
            catch(e) { alert(e); }
        }
    </script>
    <form id="form1" runat="server" defaultbutton="btnShow">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell1" runat="server" Width="150px" CssClass="tabCell">
                    <asp:Table ID="Table6" runat="server" Width="150px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow6" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" CssClass="menuTab" OnMenuItemClick="Menu1_MenuItemClick">
                                    <StaticMenuItemStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px"
                                        CssClass="nonactiveTab" />
                                    <StaticSelectedStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px"
                                        CssClass="activeTab" />
                                    <Items>
                                        <asp:MenuItem Text="FS" Value="0"></asp:MenuItem>
                                        <asp:MenuItem Text="OU" Value="1"></asp:MenuItem>
                                    </Items>
                                </asp:Menu>
                                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="tabWU" runat="server">
                                        <asp:Table ID="Table4" runat="server" Width="150px" CssClass="tabTable">
                                            <asp:TableRow ID="TableRow4" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell7" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbWorkshop" runat="server" Width="130px" ReadOnly="true"
                                                        CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow5" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell8" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbUte" runat="server" Width="130px" ReadOnly="true"
                                                        CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell9" runat="server" Width="150px" HorizontalAlign="Center"
                                                    CssClass="tabCell">
                                                    <asp:ImageButton ID="btnWUTree" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/treeButton.png"
                                                        CssClass="contentImgBtn"></asp:ImageButton>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>
                                    <asp:View ID="tabOU" runat="server">
                                        <asp:Table ID="Table5" runat="server" Width="150px" CssClass="tabTable">
                                            <asp:TableRow ID="TableRow8" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell10" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbOrg" runat="server" Width="130px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow10" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell11" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbOrgUte" runat="server" Width="130px" ReadOnly="true"
                                                        CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow11" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell12" runat="server" Width="150px" HorizontalAlign="Center"
                                                    CssClass="tabCell">
                                                    <asp:ImageButton ID="btnOrgTree" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/treeButton.png"
                                                        CssClass="contentImgBtn"></asp:ImageButton>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>
                                </asp:MultiView>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow9" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell13" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table7" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="rowEmplTypeLbl" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell43" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblEmplType" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="rowEmplType" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell39" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:DropDownList ID="cbEmplType" runat="server" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="cbEmplType_SelectedIndexChanged" CssClass="contentDDList"></asp:DropDownList>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow12" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell14" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblEmployee" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow13" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell15" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:TextBox ID="tbEmployee" runat="server" Width="130px" CssClass="contentTb"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow14" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell16" runat="server" Width="150px" HorizontalAlign="Left"
                                            CssClass="tabCell">
                                            <asp:ListBox ID="lboxEmployees" runat="server" Width="140px" SelectionMode="Multiple" OnPreRender="lboxEmployees_PreRender" Height="300px" CssClass="contentLblLeft">
                                            </asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>                                                                      
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table10" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow20" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell49" runat="server" Width="1000px" CssClass="tabMidAlignCell">
                                <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                    marginwidth="0px" height="515px" class="pageIframe"></iframe>
                            </asp:TableCell>
                        </asp:TableRow>                        
                        <asp:TableRow ID="TableRow19" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell29" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Label ID="lblError" runat="server" Width="980px" CssClass="errorText"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell151" runat="server" Width="155px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                    <asp:Table ID="Table3" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" CssClass="contentBtn"></asp:Button>                                
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table14" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow25" runat="server" Width="1000px">
                        <asp:TableCell runat="server" Width="300px" CssClass="tabMidAlignCell"></asp:TableCell>
                            <asp:TableCell ID="TableCell41" runat="server" Width="400px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Width="300px" HorizontalAlign="Right" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnGO_Report" runat="server" OnClick="btnGO_Report_Click" CssClass="contentBtn2"></asp:Button>
                                <asp:Button ID="btnBH_Report" runat="server" OnClick="btnBH_Report_Click" CssClass="contentBtn2"></asp:Button>
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
