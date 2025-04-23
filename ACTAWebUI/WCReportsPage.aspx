<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WCReportsPage.aspx.cs"
    Inherits="ACTAWebUI.WCReportsPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />

    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>

</head>
<body onload="CheckPDF(),document.body.style.cursor = 'default'">
    <form id="form1" runat="server" defaultbutton="btnShow">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell1" runat="server" Width="150px" CssClass="tabCell">
                    <asp:Table ID="Table6" runat="server" Width="150px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow6" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table4" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow4" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell7" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblEmplData" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
                                            <input type='hidden' id='test' runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow5" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell8" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Panel ID="emplDataPanel" runat="server" Width="140px" Height="300px" ScrollBars="Auto" CssClass="resultPanel">
                                                <asp:Table ID="Table3" runat="server" Width="120px" CssClass="tabNoBorderTable">                                            
                                                    <asp:TableRow ID="TableRow48" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell5" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblFirstName" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow3" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell10" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbFirstName" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow14" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell16" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblLastName" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow29" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell57" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbLastName" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                     <asp:TableRow ID="TableRow216" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell176" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblPayroll" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow226" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell1376" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbPayroll" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow30" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell58" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblStringone" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow31" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell59" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbStringone" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowPlantlbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell60" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblPlant" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowPlant" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell61" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbPlant" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowCClbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell62" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblCostCentar" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowCC" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell63" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbCostCentar" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWGlbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell64" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblWorkgroup" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWG" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell65" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbWorkgroup" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowUTElbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell66" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblUTE" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowUTE" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell67" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbUTE" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowBranchtlbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell68" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblBranch" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowBranch" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell69" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbBranch" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWUlbl" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell20" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblWUnit" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>                                                        
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="rowWU" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell21" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbWUnit" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow46" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell70" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:Label ID="lblOUnit" runat="server" Width="105px" CssClass="contentLblLeft"></asp:Label>                                            
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow47" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell71" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:TextBox ID="tbOUnit" runat="server" Width="105px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="TableRow11" runat="server" Width="120px">
                                                        <asp:TableCell ID="TableCell19" runat="server" Width="120px" CssClass="tabCell">
                                                            <asp:PlaceHolder ID="ascoCtrlHolder" runat="server"></asp:PlaceHolder>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </asp:Panel>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow15" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell17" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table8" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell45" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblPeriod" runat="server" Width="65px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow8" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell48" runat="server" Width="100px" CssClass="tabCell">
                                            <asp:DropDownList ID="lbMonths" runat="server" Width="80px" CssClass="contentLblLeft">
                                            </asp:DropDownList>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell4" runat="server" Width="50px" CssClass="tabCell">
                                            <asp:TextBox ID="tbYear" runat="server" Width="35px" CssClass="contentTb"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" Width="150px">
                            <asp:TableCell runat="server" Width="150px">
                                <asp:Table ID="Table7" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell12" runat="server" Width="150px" HorizontalAlign="Left"
                                            VerticalAlign="Middle">
                                            <asp:RadioButton ID="rbSummary" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="true" CssClass="contentRb" Checked="True" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow28" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell38" runat="server" Width="150px" HorizontalAlign="Left"
                                            VerticalAlign="Middle">
                                            <asp:RadioButton ID="rbCounter" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="true" CssClass="contentRb" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow24" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell34" runat="server" Width="150px" HorizontalAlign="Left"
                                            VerticalAlign="Middle">
                                            <asp:RadioButton ID="rbPayS" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="true" CssClass="contentRb" Enabled="False" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table10" runat="server" Width="1000px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow21" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell150" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Table ID="Table2" runat="server" Width="1000px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow36" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell13" runat="server" Width="1000px" CssClass="tabCell">
                                            <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                                marginwidth="0px" height="523px" class="pageIframe"></iframe>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:TableRow ID="TableRow19" runat="server" Width="1000px">
                                    <asp:TableCell runat="server" Width="1000px" CssClass="tabCell">
                                        <asp:Label ID="lblGraphPeriod" runat="server" CssClass="contentLblLeftBold" Width="430px"></asp:Label>
                                        <asp:Label ID="lblError" runat="server" Width="530px" CssClass="errorText"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow212" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell151" runat="server" Width="155px" HorizontalAlign="Center"
                    CssClass="tabLeftAlignCell">
                    <asp:Table ID="Table5" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow23" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell9" runat="server" Width="150px" HorizontalAlign="Center"
                                CssClass="tabMidAlignCell">
                                <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" CssClass="contentBtn">
                                </asp:Button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table14" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow9" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell41" runat="server" Width="1000px" CssClass="tabMidAlignCell"
                                HorizontalAlign="Center" VerticalAlign="Middle">
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
