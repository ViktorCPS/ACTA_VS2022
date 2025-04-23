<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WCPayslips.aspx.cs"
    Inherits="ACTAWebUI.WCPayslips" %>


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
            <asp:TableRow ID="TableRow145" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell167" runat="server" Width="150px" CssClass="tabCell">
                    <asp:Table ID="Table6" runat="server" Width="150px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow124" runat="server" Width="150px" Height="10px">
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow6" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table8" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell45" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblPeriod" runat="server" Width="65px" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow8" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell48" runat="server" Width="105px" CssClass="tabCell">
                                            <asp:DropDownList ID="lbMonths" runat="server" CssClass="contentLblLeft">
                                            </asp:DropDownList>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell4" runat="server" Width="45px" CssClass="tabCell">
                                            <asp:TextBox ID="tbYear" runat="server" Width="35px" CssClass="contentTb"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow15" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell17" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Table ID="Table4" runat="server" Width="140px" CssClass="tabNoBorderTable" Height="350px">
                                    <asp:TableRow ID="TableRow4" runat="server" Width="140px">
                                        <asp:TableCell ID="TableCell7" runat="server" Width="140px" CssClass="tabCell">
                                            <input type='hidden' id='test' runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow5" runat="server" Width="140px">
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell221" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table10" runat="server" Width="1000px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow21" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell150" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Table ID="Table2" runat="server" Width="1000px" CssClass="tabNoBorderTable" Height="530px">
                                    <asp:TableRow ID="TableRow36" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell13" runat="server" Width="1000px" CssClass="tabCell">   
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:TableRow ID="TableRow19" runat="server" Width="1000px">
                                    <asp:TableCell ID="TableCell2" runat="server" Width="1000px" CssClass="tabCell">
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
                            <asp:TableCell ID="TableCell9" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnShow" runat="server"  OnClick="btnShow_Click" CssClass="contentBtn">
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
