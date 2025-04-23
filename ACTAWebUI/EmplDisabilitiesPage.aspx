<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplDisabilitiesPage.aspx.cs" Inherits="ACTAWebUI.EmplDisabilitiesPage" %>

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
            <asp:TableRow ID="emplRow" runat="server" Width="990px">
                <asp:TableCell ID="TableCell151" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">                        
                    <asp:Table ID="Table3" runat="server" Width="990px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblEmplData" runat="server" Width="900px" CssClass="hdrTitle"></asp:Label>
                                <asp:Button ID="btnPostBack" runat="server" OnClick="btnPostBack_Click" CssClass="contentInvisibleBtn"></asp:Button>
                            </asp:TableCell>                            
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow14" runat="server" Width="990px">
                <asp:TableCell ID="TableCell26" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                    <iframe id="resultIFrame" runat="server" frameborder="0" scrolling="auto" marginheight="0px"
                                    marginwidth="0px" height="260px" class="pageIframe"></iframe>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow9" runat="server" Width="990px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow8" runat="server" Width="990px">
                <asp:TableCell ID="TableCell19" runat="server" Width="990px" CssClass="tabMidAlignCell">
                    <asp:Label ID="lblNewEntry" runat="server" Width="980px" CssClass="contentLblLeft"></asp:Label>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server" Width="990px">
                <asp:TableCell ID="TableCell1" runat="server" Width="990px" HorizontalAlign="Center" CssClass="tabMidAlignCell">                        
                    <asp:Table ID="tableSaveUpdate" runat="server" Width="990px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow4" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell2" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Table ID="Table2" runat="server" Width="970px" CssClass="tabNoBorderTable">
                                    <asp:TableRow ID="TableRow7" runat="server" Width="970px">
                                        <asp:TableCell ID="TableCell6" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                            <asp:Table ID="Table4" runat="server" Width="300px" CssClass="tabNoBorderTable">
                                                <asp:TableRow ID="TableRow13" runat="server" Width="300px">
                                                    <asp:TableCell ID="TableCell22" runat="server" Width="110px" CssClass="tabMidAlignCell">
                                                        <asp:Table ID="Table1" runat="server" Width="300px" CssClass="tabNoBorderTable">
                                                            <asp:TableRow ID="TableRow12" runat="server" Width="300px">
                                                                <asp:TableCell ID="TableCell16" runat="server" Width="110px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="lblType" runat="server" Width="100px" CssClass="contentLbl"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell17" runat="server" Width="85px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
                                                                    <asp:RadioButton ID="rbPermanent" runat="server" AutoPostBack="true" OnCheckedChanged="rbPermanent_CheckedChanged" CssClass="contentRb" />
                                                                </asp:TableCell>                                                    
                                                                <asp:TableCell ID="TableCell20" runat="server" Width="85px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
                                                                    <asp:RadioButton ID="rbTemporary" runat="server" AutoPostBack="true" OnCheckedChanged="rbTemporary_CheckedChanged" CssClass="contentRb" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell21" runat="server" Width="20px" CssClass="tabMidAlignCell"></asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow11" runat="server" Width="300px">
                                                                <asp:TableCell ID="TableCell10" runat="server" Width="110px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="lblStartDate" runat="server" Width="100px" CssClass="contentLbl"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell32" runat="server" Width="85px" CssClass="tabMidAlignCell">
                                                                    <asp:TextBox ID="tbStartDate" runat="server" Width="70px" CssClass="contentTb"></asp:TextBox>
                                                                </asp:TableCell>                                                    
                                                                <asp:TableCell ID="TableCell44" runat="server" Width="85px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
                                                                    <asp:ImageButton ID="btnStartDate" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"
                                                                        CssClass="contentImgBtn"></asp:ImageButton>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell11" runat="server" Width="20px" CssClass="tabMidAlignCell"></asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow10" runat="server" Width="300px">
                                                                <asp:TableCell ID="TableCell3" runat="server" Width="110px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="lblEndDate" runat="server" Width="100px" CssClass="contentLbl"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell13" runat="server" Width="85px" CssClass="tabMidAlignCell">
                                                                    <asp:TextBox ID="tbEndDate" runat="server" Width="70px" CssClass="contentTb"></asp:TextBox>
                                                                </asp:TableCell>                                                    
                                                                <asp:TableCell ID="TableCell14" runat="server" Width="85px" HorizontalAlign="Left" CssClass="tabMidAlignCell">
                                                                    <asp:ImageButton ID="btnEndDate" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif"
                                                                        CssClass="contentImgBtn"></asp:ImageButton>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell15" runat="server" Width="20px" CssClass="tabMidAlignCell"></asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow15" runat="server" Width="300px">
                                                    <asp:TableCell ID="TableCell23" runat="server" Width="110px" CssClass="tabMidAlignCell">
                                                        <asp:Table ID="Table5" runat="server" Width="300px" CssClass="tabNoBorderTable">
                                                            <asp:TableRow ID="TableRow16" runat="server" Width="300px">
                                                                <asp:TableCell ID="TableCell24" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="lblNote" runat="server" Width="280px" CssClass="contentLblLeft"></asp:Label>                                                                    
                                                                </asp:TableCell>
                                                            </asp:TableRow>    
                                                            <asp:TableRow ID="TableRow6" runat="server" Width="300px">
                                                                <asp:TableCell ID="TableCell25" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                                    <asp:TextBox ID="tbNote" runat="server" Width="275px" Height="40px" TextMode="MultiLine" CssClass="contentTb"></asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </asp:TableCell>
                                                </asp:TableRow>                                                
                                             </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell9" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                            <asp:Table ID="Table6" runat="server" Width="300px" CssClass="tabNoBorderTable">
                                                <asp:TableRow ID="TableRow17" runat="server" Width="300px">
                                                    <asp:TableCell ID="TableCell4" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="lblDisability" runat="server" Width="270px" CssClass="contentLblLeft"></asp:Label>                                                                    
                                                    </asp:TableCell>
                                                </asp:TableRow>    
                                                <asp:TableRow ID="TableRow18" runat="server" Width="300px">
                                                    <asp:TableCell ID="TableCell12" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                        <asp:TextBox ID="tbDisability" runat="server" Width="265px" CssClass="contentTb"></asp:TextBox>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow19" runat="server" Width="300px">
                                                    <asp:TableCell ID="TableCell27" runat="server" Width="300px" CssClass="tabMidAlignCell">
                                                        <asp:ListBox ID="lbDisability" runat="server" Width="275px" SelectionMode="Single" Height="95px" OnPreRender="lbDisability_PreRender" CssClass="contentLblLeft">
                                                        </asp:ListBox>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell7" runat="server" Width="330px" HorizontalAlign="Center" CssClass="tabBottomAlignCell">
                                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="contentBtn"></asp:Button>                                            
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell8" runat="server" Width="330px" HorizontalAlign="Center" CssClass="tabBottomAlignCell">
                                            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="contentBtn"></asp:Button>                                            
                                            <input ID="SelBox" type="hidden" runat="server" />
                                            <input ID="SelRecord" type="hidden" runat="server" />
                                        </asp:TableCell>                                        
                                    </asp:TableRow>                                                                     
                                </asp:Table>                                
                            </asp:TableCell>                            
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow5" runat="server" Width="990px">
                            <asp:TableCell ID="TableCell18" runat="server" Width="990px" CssClass="tabMidAlignCell">
                                <asp:Label ID="lblError" runat="server" Width="970px" CssClass="errorText"></asp:Label>
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