<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WCFormsPage.aspx.cs" Inherits="ACTAWebUI.WCFormsPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet" />

    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>

</head>
<body oncontextmenu="return false" onload="document.body.style.cursor = 'default'">
    <form id="form1" runat="server" defaultbutton="btnShow">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow167" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell189" runat="server" Width="150px" CssClass="tabCell">
                    <asp:Table ID="Table6" runat="server" Width="150px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow6" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table4" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow4" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell7" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblEmplData" runat="server" Width="140px" CssClass="contentLblLeft"></asp:Label>
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
                        <asp:TableRow ID="TableRow1" runat="server" Width="150px">
                            <asp:TableCell runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow17309" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell790" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblForms" runat="server" Width="130px" CssClass="contentLblLeft">
                                            </asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell12" runat="server" Width="150px" CssClass="tabCell" HorizontalAlign="Left">
                                            <asp:RadioButton ID="rbFillIn" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="true" CssClass="contentRb" Checked="True" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow2" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell3" runat="server" Width="150px" CssClass="tabCell" HorizontalAlign="Left">
                                            <asp:RadioButton ID="rbEmail" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="true" CssClass="contentRb" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell4" runat="server" Width="150px" CssClass="tabCell" HorizontalAlign="Left">
                                           <asp:RadioButton ID="rbFuel" runat="server" OnCheckedChanged="rb_CheckedChanged"
                                                AutoPostBack="true" CssClass="contentRb" />
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
                                            <rsweb:ReportViewer ID="ReportViewer4" runat="server" Height="520px" Width="900px"
                                                ZoomMode="PageWidth">
                                            </rsweb:ReportViewer>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:TableRow ID="TableRow19" runat="server" Width="1000px">
                                    <asp:TableCell ID="TableCell1" runat="server" Width="1000px" CssClass="tabCell">
                                        <asp:Label ID="lblError" runat="server" Width="980px" CssClass="errorText"></asp:Label>
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
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
