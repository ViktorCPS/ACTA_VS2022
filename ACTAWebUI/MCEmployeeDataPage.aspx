<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MCEmployeeDataPage.aspx.cs" Inherits="ACTAWebUI.MCEmployeeDataPage" %>

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
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell1" runat="server" Width="150px" CssClass="tabCell">
                    <asp:Table ID="Table6" runat="server" Width="150px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="rowEmplFilter" runat="server" Width="150px">
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
                        <asp:TableRow ID="rowEmpl" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell13" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table7" runat="server" Width="150px" CssClass="tabTable">
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
                                            <asp:ListBox ID="lboxEmployees" runat="server" Width="140px" SelectionMode="Single" OnPreRender="lboxEmployees_PreRender" Height="180px" CssClass="contentLblLeft">
                                            </asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>                                    
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow15" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell3" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table2" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow16" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell17" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Menu ID="MenuData" runat="server" Orientation="Vertical" CssClass="menuTab" OnMenuItemClick="MenuData_MenuItemClick">
                                                <StaticMenuItemStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px"
                                                    CssClass="nonactiveInnerTab" />
                                                <StaticSelectedStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px"
                                                    CssClass="activeInnerTab" />
                                                <Items>
                                                    <asp:MenuItem Text="AnagraphicalData" Value="AnagraphicalData"></asp:MenuItem>
                                                    <asp:MenuItem Text="RiskManagement" Value="RiskManagement"></asp:MenuItem>
                                                    <asp:MenuItem Text="VisitsManagement" Value="VisitsManagement"></asp:MenuItem>
                                                    <asp:MenuItem Text="Disabilities" Value="Disabilities"></asp:MenuItem>
                                                    <asp:MenuItem Text="WeightHeight" Value="WeightHeight"></asp:MenuItem>
                                                    <asp:MenuItem Text="Vaccines" Value="Vaccines"></asp:MenuItem>
                                                    <asp:MenuItem Text="OnDemandAppointments" Value="OnDemandAppointments"></asp:MenuItem>
                                                    <asp:MenuItem Text="Schedules" Value="Schedules"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </asp:TableCell>
                                    </asp:TableRow>                                    
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="1010px" CssClass="tabCell">
                    <asp:Table ID="Table10" runat="server" Width="1010px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow20" runat="server" Width="1010px">
                            <asp:TableCell ID="TableCell49" runat="server" Width="1010px" CssClass="tabCell">
                                <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                    marginwidth="0px" height="515px" class="pageIframe"></iframe>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell151" runat="server" Width="155px" HorizontalAlign="Center"
                    CssClass="tabMidAlignCell">                        
                    <asp:Table ID="backTable" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="150px" HorizontalAlign="Center"
                                CssClass="tabMidAlignCell">
                                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell> <%--btnBack_Click--%>
                        </asp:TableRow>                        
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="1010px" CssClass="tabCell">
                    <asp:Label ID="lblError" runat="server" Width="980px" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
