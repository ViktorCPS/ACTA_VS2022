<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TLReportsPage.aspx.cs"
    Inherits="ACTAWebUI.TLReports" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ACTA Web</title>

    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>

    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />
</head>
<body onload="document.body.style.cursor = 'default'">
    <form id="form1" runat="server" defaultbutton="btnShow">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1181px" CssClass="tabNoBorderTable">
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
                                                <asp:TableCell ID="TableCell3" runat="server" Width="150px" HorizontalAlign="Left"
                                                    CssClass="tabCell">
                                                    <asp:ListBox ID="lBoxWU" runat="server" Width="140px" Height="100px" SelectionMode="Multiple"
                                                        CssClass="contentLblLeft" OnSelectedIndexChanged="lBoxWU_SelectedIndexChanged"
                                                        AutoPostBack="True"></asp:ListBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow5" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell7" runat="server" Width="150px" HorizontalAlign="Left"
                                                    CssClass="tabCell">
                                                    <asp:CheckBox ID="cbSelectAllWU" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectAllWU_CheckedChanged"
                                                        CssClass="contentLblLeft" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>
                                    <asp:View ID="tabOU" runat="server">
                                        <asp:Table ID="Table5" runat="server" Width="150px" CssClass="tabTable">
                                            <asp:TableRow ID="TableRow8" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell10" runat="server" Width="150px" HorizontalAlign="Left"
                                                    CssClass="tabCell">
                                                    <asp:ListBox ID="lBoxOU" runat="server" Width="140px" Height="100px" SelectionMode="Multiple"
                                                        CssClass="contentLblLeft" OnSelectedIndexChanged="lBoxOU_SelectedIndexChanged"
                                                        AutoPostBack="True"></asp:ListBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow10" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell11" runat="server" Width="150px" HorizontalAlign="Left"
                                                    CssClass="tabCell">
                                                    <asp:CheckBox ID="cbSelectAllOU" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectAllOU_CheckedChanged"
                                                        CssClass="contentLblLeft" />
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
                                            <asp:ListBox ID="lboxEmployees" runat="server" Width="140px" Height="150px" SelectionMode="Multiple" OnPreRender="lboxEmployees_PreRender"
                                                CssClass="contentLblLeft"></asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell8" runat="server" Width="150px" HorizontalAlign="Left"
                                            CssClass="tabCell">
                                            <asp:CheckBox ID="cbSelectAllEmpolyees" runat="server" CssClass="contentLblLeft" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow15" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell17" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table8" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow30" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell45" runat="server" Width="70px" CssClass="tabCell">
                                            <asp:Label ID="lblPeriod" runat="server" Width="65px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell46" runat="server" Width="40px" CssClass="tabCell"></asp:TableCell>
                                        <asp:TableCell ID="TableCell47" runat="server" Width="30px" CssClass="tabCell"></asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow31" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell48" runat="server" Width="100px" CssClass="tabCell">
                                            <asp:DropDownList ID="lbMonths" runat="server" Width="90px" AutoPostBack="true" OnSelectedIndexChanged="Date_Changed" CssClass="contentLblLeft">
                                            </asp:DropDownList>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell57" runat="server" Width="50px" CssClass="tabCell">
                                            <asp:TextBox ID="tbYear" runat="server" AutoPostBack="true" OnTextChanged="Date_Changed" Width="40px" CssClass="contentTb"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" Width="150px">
                            <asp:TableCell ID="TableCell9" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table2" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow11" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell18" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:RadioButton ID="rbSummary" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                Checked="True" AutoPostBack="true" CssClass="contentRb" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow166" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell19" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:RadioButton ID="rbAnalitical" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="true" CssClass="contentRb" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow16" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell20" runat="server" CssClass="tabCell">
                                            <asp:RadioButton ID="rbCounter" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="true" CssClass="contentRb" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table910" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow820" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell49" runat="server" Width="1000px" CssClass="tabCell">
                                <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                    marginwidth="0px" height="515px" class="pageIframe"></iframe>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow811" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell12" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Label ID="lblGraphPeriod" runat="server" CssClass="contentLblLeftBold" Width="400px">
                                </asp:Label>
                                <asp:Label ID="lblError" runat="server" Width="550px" CssClass="errorText"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1182px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell151" runat="server" Width="155px" HorizontalAlign="Center"
                    CssClass="tabLeftAlignCell">
                    <asp:Table ID="Table3" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="150px" HorizontalAlign="Center"
                                CssClass="tabMidAlignCell">
                                <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" CssClass="contentBtn">
                                </asp:Button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table14" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell42" runat="server" Width="450px" CssClass="tabMidAlignCell"
                                HorizontalAlign="Left" VerticalAlign="Middle">
                                <asp:CheckBox ID="chbSinglePage" runat="server" CssClass="contentLblLeft" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell41" runat="server" Width="550px" CssClass="tabMidAlignCell"
                                HorizontalAlign="Left" VerticalAlign="Middle">
                                <asp:Button ID="btnReport" runat="server" OnClick="btnReport_Click" CssClass="contentBtn">
                                </asp:Button>
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
