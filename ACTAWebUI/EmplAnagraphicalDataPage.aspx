<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplAnagraphicalDataPage.aspx.cs" Inherits="ACTAWebUI.EmplAnagraphicalDataPage" %>

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
            <asp:TableRow ID="TableRow14" runat="server" Width="990px">
                <asp:TableCell ID="TableCell26" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                    <asp:Panel ID="emplDataPanel" runat="server" Width="990px" Height="470px" ScrollBars="Auto" CssClass="resultPanel">
                        <asp:Table ID="Table2" runat="server" Width="990px" CssClass="tabTable">
                            <asp:TableRow ID="TableRow1" runat="server" Width="990px">
                                <asp:TableCell ID="TableCell1" runat="server" Width="440px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                    <asp:Table ID="Table1" runat="server" Width="440px" CssClass="tabNoBorderTable">
                                        <asp:TableRow ID="TableRow36" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell14" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblCompany" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell15" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbCompany" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow37" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow15" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell27" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblUTE" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell28" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbUTE" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow9" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow16" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell29" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblOUnit" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell30" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbOUnit" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow6" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow17" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell31" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblFirstName" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell32" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbFirstName" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow7" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow18" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell33" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblLastName" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell34" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbLastName" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow8" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow19" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell35" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblEmplID" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell36" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbEmplID" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow10" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow20" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell37" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblStringone" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell38" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbStringone" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow11" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow21" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell39" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblShiftGroup" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell40" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbShiftGroup" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow25" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow22" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell41" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblBirthDate" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell42" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbBirthDate" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow26" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow23" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell43" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblCity" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell44" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbCity" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow27" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow24" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell45" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblAddress" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell21" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbAddress" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow28" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow12" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell22" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblPhone1" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell23" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbPhone1" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow29" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow5" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell6" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblPhone2" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell7" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbPhone2" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow30" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow4" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell3" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblHiringDate" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>                                        
                                            <asp:TableCell ID="TableCell4" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbHiringDate" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow31" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow13" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell24" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblTerminationDate" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>                                        
                                            <asp:TableCell ID="TableCell25" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbTerminationDate" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>                        
                                    </asp:Table>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell2" runat="server" Width="440px" HorizontalAlign="Center" CssClass="tabCell">
                                    <asp:Table ID="Table4" runat="server" Width="440px" CssClass="tabNoBorderTable">
                                        <asp:TableRow ID="TableRow32" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell8" runat="server" Width="140px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblPosition" runat="server" Width="130px" CssClass="contentLbl"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell9" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:TextBox ID="tbPosition" runat="server" Width="290px" ReadOnly="true" CssClass="contentTbDisabled"></asp:TextBox>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow ID="TableRow33" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
                                        <asp:TableRow ID="TableRow35" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell12" runat="server" Width="140px" CssClass="tabMidAlignCell"></asp:TableCell>
                                            <asp:TableCell ID="TableCell10" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:Label ID="lblPositionRisks" runat="server" Width="290px" CssClass="contentLblLeft"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>                                        
                                        <asp:TableRow ID="TableRow34" runat="server" Width="440px">
                                            <asp:TableCell ID="TableCell13" runat="server" Width="140px" CssClass="tabMidAlignCell"></asp:TableCell>
                                            <asp:TableCell ID="TableCell11" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                <asp:ListBox ID="lbPosRisks" runat="server" Width="290px" Height="330px" OnPreRender="lbPosRisks_PreRender" CssClass="contentLblLeft"></asp:ListBox>
                                            </asp:TableCell>
                                        </asp:TableRow>                                                                              
                                    </asp:Table>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                </asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
