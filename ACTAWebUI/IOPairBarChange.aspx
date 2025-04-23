<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IOPairBarChange.aspx.cs" Inherits="ACTAWebUI.IOPairBarChange" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

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
        <asp:Table ID="Table5" runat="server" Width="800px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow9" runat="server" Width="800px">
                <asp:TableCell ID="TableCell3" runat="server" Width="800px" CssClass="hdrCell">
                    <asp:Label ID="lblEmployee" runat="server" Width="790px" CssClass="loginUserTextLeft"></asp:Label>
                </asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table4" runat="server" Width="800px" HorizontalAlign="Center" CssClass="tabTable">
            <asp:TableRow ID="TableRow4" runat="server" Width="800px">
                <asp:TableCell ID="TableCell7" runat="server" Width="800px" CssClass="tabCell">
                    <asp:Table ID="Table19" runat="server" Width="790px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow38" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell24" runat="server" Width="790px" CssClass="tabCell">
                                <asp:Panel ID="counterPanel" runat="server" Width="780px" Height="40px" ScrollBars="Auto" CssClass="resultPanel">
                                    <asp:PlaceHolder ID="counterCtrlHolder" runat="server"></asp:PlaceHolder>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow11" runat="server" Width="800px">
                <asp:TableCell ID="TableCell14" runat="server" Width="800px" CssClass="tabCell">
                    <asp:Panel ID="legendPanel" runat="server" Width="790px" Height="20px" ScrollBars="Auto" CssClass="resultPanel">
                        <asp:PlaceHolder ID="legendCtrlHolder" runat="server"></asp:PlaceHolder>
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow21" runat="server" Width="800px">
                <asp:TableCell ID="TableCell150" runat="server" Width="800px" CssClass="tabCell">
                    <asp:Table ID="Table2" runat="server" Width="790px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow36" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell50" runat="server" Width="790px" CssClass="tabCell">                            
                                <asp:Panel ID="resultPanel" runat="server" Width="780px" Height="50px" ScrollBars="Auto" CssClass="resultPanel">
                                    <asp:PlaceHolder ID="ctrlHolder" runat="server"></asp:PlaceHolder>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server" Width="800px">
                <asp:TableCell ID="TableCell1" runat="server" Width="800px" CssClass="tabCell">                    
                    <asp:Table ID="Table1" runat="server" Width="790px" CssClass="tabTable">                        
                        <asp:TableRow ID="TableRow2" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell2" runat="server" Width="790px" CssClass="tabCell">
                                <asp:Panel ID="pairDataPanel" runat="server" Width="780px" Height="300px" ScrollBars="Auto" CssClass="resultPanel">
                                    <asp:PlaceHolder ID="pairDataCtrlHolder" runat="server"></asp:PlaceHolder>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow6" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell10" runat="server" Width="790px" CssClass="tabCell">
                                <asp:Label ID="lblError" runat="server" Width="780px" CssClass="errorText"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow10" runat="server" Width="800px">
                <asp:TableCell ID="TableCell12" runat="server" Width="800px" HorizontalAlign="Left" CssClass="tabCell">
                    <asp:Label ID="lblHelpMessage" runat="server" Width="790px" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>                            
            </asp:TableRow>
            <asp:TableRow ID="TableRow3" runat="server" Width="800px">
                <asp:TableCell ID="TableCell4" runat="server" Width="800px" CssClass="tabCell">                    
                    <asp:Table ID="Table3" runat="server" Width="790px" CssClass="tabTable">                        
                        <asp:TableRow ID="TableRow5" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell15" runat="server" Width="200px" HorizontalAlign="Left" CssClass="tabCell">                                
                                <asp:CheckBox ID="chbDateCompany" runat="server" Width="190px" AutoPostBack="true" OnCheckedChanged="chbDateCompany_CheckedChanged" CssClass="contentChb"></asp:CheckBox>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell5" runat="server" Width="50px" HorizontalAlign="Right" CssClass="tabCell">
                                <%--<asp:Button ID="btnPrev" runat="server" OnClick="btnPrev_Click" CssClass="contentBtn"></asp:Button>--%>
                                <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/backArrow.png" OnClick="btnPrev_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell11" runat="server" Width="290px" HorizontalAlign="Center" CssClass="tabCell">
                                <asp:Label ID="lblDate" runat="server" Width="280px" CssClass="contentGraphPeriodLbl"></asp:Label>
                                <asp:TextBox ID="tbDate" runat="server" Visible="false" CssClass="contentTb"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell6" runat="server" Width="250px" HorizontalAlign="Left" CssClass="tabCell">
                                <%--<asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="contentBtn"></asp:Button>--%>
                                <asp:ImageButton ID="btnNext" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/forwardArrow.png" OnClick="btnNext_Click" CssClass="contentImgBtnWide"></asp:ImageButton>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow7" runat="server" Width="790px" CssClass="contentSeparatorRow" />
                        <asp:TableRow ID="TableRow8" runat="server" Width="790px">
                            <asp:TableCell ID="TableCell16" runat="server" Width="200px" HorizontalAlign="Left" CssClass="tabCell">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell8" runat="server" Width="50px" HorizontalAlign="Left" CssClass="tabCell"></asp:TableCell>
                            <asp:TableCell ID="TableCell13" runat="server" Width="290px" HorizontalAlign="Center" CssClass="tabCell">
                                <asp:Button ID="btnUndo" runat="server" OnClick="btnUndo_Click" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell9" runat="server" Width="250px" HorizontalAlign="Right" CssClass="tabCell">
                                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="contentBtn"></asp:Button>
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
