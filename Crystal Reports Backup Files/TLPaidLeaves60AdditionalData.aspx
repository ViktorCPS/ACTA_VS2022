<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TLPaidLeaves60AdditionalData.aspx.cs" Inherits="ReportsWeb.TLPaidLeaves60AdditionalData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:Table ID="Table5" runat="server" Width="400px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow9" runat="server" Width="400px">
                <asp:TableCell ID="TableCell3" runat="server" Width="400px" CssClass="hdrCell">
                    <asp:Label ID="lblAddData" runat="server" Width="390px" CssClass="loginUserTextLeft"></asp:Label>
                </asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table4" runat="server" Width="400px" HorizontalAlign="Center" CssClass="tabTable">            
            <asp:TableRow ID="TableRow10" runat="server" Width="400px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server" Width="400px">
                <asp:TableCell ID="TableCell1" runat="server" Width="400px" CssClass="tabCell">                    
                    <asp:Table ID="Table1" runat="server" Width="390px" CssClass="tabTable">                        
                        <asp:TableRow ID="TableRow2" runat="server" Width="390px">
                            <asp:TableCell ID="TableCell2" runat="server" Width="390px" CssClass="tabCell">
                                <asp:Label ID="lblPaidLeaveReason" runat="server" Width="300px" CssClass="contentLblLeft"></asp:Label>                                
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow16" runat="server" Width="390px" CssClass="contentSeparatorRow"></asp:TableRow>
                        <asp:TableRow ID="TableRow3" runat="server" Width="390px">
                            <asp:TableCell ID="TableCell4" runat="server" Width="390px" CssClass="tabCell">
                                <asp:RadioButton ID="rbStopWorking" runat="server" CssClass="contentRb" />                                
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow4" runat="server" Width="390px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="390px" CssClass="tabCell">
                                <asp:RadioButton ID="rbLessWorking" runat="server" CssClass="contentRb" />                                
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow5" runat="server" Width="390px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="390px" CssClass="tabCell">
                                <asp:RadioButton ID="rbOther" runat="server" CssClass="contentRb" />
                                <asp:TextBox ID="tbReason" runat="server" Width="350px" CssClass="contentTb"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow18" runat="server" Width="390px" CssClass="contentSeparatorRow"></asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow12" runat="server" Width="400px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow6" runat="server" Width="400px">
                <asp:TableCell ID="TableCell7" runat="server" Width="400px" CssClass="tabCell">                    
                    <asp:Table ID="Table2" runat="server" Width="390px" CssClass="tabTable">                        
                        <asp:TableRow ID="TableRow7" runat="server" Width="390px">
                            <asp:TableCell ID="TableCell8" runat="server" Width="390px" CssClass="tabCell">
                                <asp:Label ID="lblReasonDesc" runat="server" Width="300px" CssClass="contentLblLeft"></asp:Label>                                
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow15" runat="server" Width="390px" CssClass="contentSeparatorRow"></asp:TableRow>
                        <asp:TableRow ID="TableRow8" runat="server" Width="390px">
                            <asp:TableCell ID="TableCell9" runat="server" Width="390px" CssClass="tabCell">
                                <asp:RadioButton ID="rbFiatStopWorking" runat="server" CssClass="contentRb" />                                
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow11" runat="server" Width="390px">
                            <asp:TableCell ID="TableCell11" runat="server" Width="390px" CssClass="tabCell">
                                <asp:RadioButton ID="rbDescOther" runat="server" CssClass="contentRb" />
                                <asp:TextBox ID="tbDescOther" runat="server" Width="350px" CssClass="contentTb"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow17" runat="server" Width="390px" CssClass="contentSeparatorRow"></asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow13" runat="server" Width="400px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow14" runat="server" Width="400px">
                <asp:TableCell ID="TableCell10" runat="server" Width="400px" HorizontalAlign="Center" CssClass="tabCell">
                    <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
