<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TMDataPage.aspx.cs" Inherits="ACTAWebUI.TMDataPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>TM Data</title>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell1" runat="server" Width="150px" CssClass="tabCell">
                    <asp:Table ID="Table6" runat="server" Width="150px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow6" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="tabCell">
                                <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" CssClass="menuTab" OnMenuItemClick="Menu1_MenuItemClick">
                                    <StaticMenuItemStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px" CssClass="nonactiveTab" />
                                    <StaticSelectedStyle HorizontalPadding="0px" VerticalPadding="0px" ItemSpacing="0px" CssClass="activeTab" />            
                                    <Items>
                                        <asp:MenuItem Text="WU" Value="0"></asp:MenuItem>
                                        <asp:MenuItem Text="OU" Value="1"></asp:MenuItem>
                                    </Items>
                                </asp:Menu>
                                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="tabWU" runat="server">
                                        <asp:Table ID="Table4" runat="server" Width="150px" CssClass="tabTable">
                                            <asp:TableRow ID="TableRow4" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell7" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbWorkshop" runat="server" Text="WU3" Width="140px" Enabled="false" CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>                                                
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow5" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell8" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbUte" runat="server" Text="UTE2" Width="140px" Enabled="false" CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>                                                
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow7" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell9" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabCell">
                                                    <asp:ImageButton ID="btnWUTree" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/wuTree.gif" CssClass="contentImgBtn"></asp:ImageButton>
                                                </asp:TableCell>                                                
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>
                                    <asp:View ID="tabOU" runat="server">
                                        <asp:Table ID="Table5" runat="server" Width="150px" CssClass="tabTable">
                                            <asp:TableRow ID="TableRow8" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell10" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbOrg" runat="server" Text="OU2" Width="140px" Enabled="false" CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>                                                
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow10" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell11" runat="server" Width="150px" CssClass="tabCell">
                                                    <asp:TextBox ID="tbOrgUte" runat="server" Text="UTE1" Width="140px" Enabled="false" CssClass="contentTbDisabled"></asp:TextBox>
                                                </asp:TableCell>                                                
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow11" runat="server" Width="150px">
                                                <asp:TableCell ID="TableCell12" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabCell">
                                                    <asp:ImageButton ID="btnOrgTree" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/wuTree.gif" CssClass="contentImgBtn"></asp:ImageButton>
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
                                            <asp:Label ID="lblEmployee" runat="server" Width="140px" Text="Employee:" CssClass="contentLblLeft"></asp:Label>                                            
                                        </asp:TableCell>                                                
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow13" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell15" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:TextBox ID="tbEmployee" runat="server" Width="140px" CssClass="contentTb"></asp:TextBox>
                                        </asp:TableCell>                                                
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow14" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell16" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabCell">
                                            <asp:ListBox ID="lboxEmployees" runat="server" Width="140px" Height="250px" CssClass="contentLb"></asp:ListBox>
                                        </asp:TableCell>                                                
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow15" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell17" runat="server" Width="150px" CssClass="tabCell">                                
                                <asp:Table ID="Table8" runat="server" Width="150px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow16" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell18" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblPeriod" runat="server" Width="145px" Text="Period:" CssClass="contentLblLeft"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell32" runat="server" Width="45px" CssClass="tabCell"></asp:TableCell>               
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow17" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell19" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:TextBox ID="tbPeriod" runat="server" Width="140px" CssClass="contentTb"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell20" runat="server" Width="45px" CssClass="tabCell"></asp:TableCell>                                                
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow18" runat="server" Width="150px">
                                        <asp:TableCell ID="TableCell21" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Label ID="lblMonthFormat" runat="server" Width="145px" Text="*Month format" CssClass="contentHelpLbl"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell22" runat="server" Width="45px" CssClass="tabCell"></asp:TableCell>                                                
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table10" runat="server" Width="1000px" CssClass="tabNoBorderTable">
                        <asp:TableRow ID="TableRow20" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell49" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Table ID="Table19" runat="server" Width="1000px" CssClass="tabNoBorderTable">
                                    <asp:TableRow ID="TableRow38" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell24" runat="server" Width="300px" CssClass="tabCell">
                                            <asp:Table ID="Table11" runat="server" Width="300px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow22" runat="server" Width="300px">
                                                    <asp:TableCell ID="TableCell25" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label13" runat="server" Width="70px" Text="Annual leave" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell55" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label5" runat="server" Text="31" CssClass="counterLbl"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell39" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                                        <asp:Table ID="Table17" runat="server" Width="150px" CssClass="tabNoBorderTable">
                                                            <asp:TableRow ID="TableRow19" runat="server" Width="150px">
                                                                <asp:TableCell ID="TableCell51" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="Label10" runat="server" Text="2011:" Width="70px" CssClass="contentSmallLbl"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell52" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="Label40" runat="server" Text="21" CssClass="counterSmallLbl"></asp:Label>
                                                                </asp:TableCell>                                                    
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow28" runat="server" Width="150px">
                                                                <asp:TableCell ID="TableCell53" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="Label42" runat="server" Text="2012:" Width="70px" CssClass="contentSmallLbl"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell54" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="Label43" runat="server" Text="10" CssClass="counterSmallLbl"></asp:Label>
                                                                </asp:TableCell>                                                    
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </asp:TableCell>
                                                </asp:TableRow>                                                
                                                <%--<asp:TableRow ID="TableRow31" runat="server" Width="300px">
                                                    <asp:TableCell ID="TableCell39" runat="server" Width="300px" CssClass="tabCell">
                                                        <asp:Table ID="Table17" runat="server" Width="300px" CssClass="tabNoBorderTable">
                                                            <asp:TableRow ID="TableRow32" runat="server" Width="300px" CssClass="tabRow">
                                                                <asp:TableCell ID="TableCell40" runat="server" Width="150px" CssClass="tabMidAlignCell">
                                                                    <asp:Label ID="Label41" runat="server" Text="31" CssClass="counterLbl"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell44" runat="server" Width="150px" CssClass="tabCell">
                                                                    <asp:Table ID="Table18" runat="server" Width="150px" CssClass="tabNoBorderTable">
                                                                        <asp:TableRow ID="TableRow33" runat="server" Width="150px" CssClass="tabRow">
                                                                            <asp:TableCell ID="TableCell45" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                                                <asp:Label ID="Label11" runat="server" Text="2011:" Width="65px" CssClass="contentSmallLbl"></asp:Label>
                                                                            </asp:TableCell>
                                                                            <asp:TableCell ID="TableCell46" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                                                <asp:Label ID="Label37" runat="server" Text="21" CssClass="counterSmallLbl"></asp:Label>
                                                                            </asp:TableCell>
                                                                        </asp:TableRow>
                                                                        <asp:TableRow ID="TableRow34" runat="server" Width="150px" CssClass="tabRow">
                                                                            <asp:TableCell ID="TableCell47" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                                                <asp:Label ID="Label38" runat="server" Text="2012:" Width="65px" CssClass="contentSmallLbl"></asp:Label>
                                                                            </asp:TableCell>
                                                                            <asp:TableCell ID="TableCell48" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                                                <asp:Label ID="Label39" runat="server" Text="10" CssClass="counterSmallLbl"></asp:Label>
                                                                            </asp:TableCell>
                                                                        </asp:TableRow>
                                                                    </asp:Table>        
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </asp:TableCell>
                                                </asp:TableRow>--%>
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell26" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table12" runat="server" Width="150px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow23" runat="server" Width="150px">
                                                    <asp:TableCell ID="TableCell27" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label4" runat="server" Width="70px" Text="Paid leave" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell35" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label9" runat="server" Text="7" CssClass="counterLbl"></asp:Label>
                                                    </asp:TableCell>                                                
                                                </asp:TableRow>                                                
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell28" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table13" runat="server" Width="150px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow24" runat="server" Width="150px">
                                                    <asp:TableCell ID="TableCell29" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label6" runat="server" Width="70px" Text="Bank hours" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell36" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label1" runat="server" Text="120" CssClass="counterLbl"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>                                                
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell30" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table9" runat="server" Width="150px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow26" runat="server" Width="150px">
                                                    <asp:TableCell ID="TableCell31" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label7" runat="server" Width="70px" Text="Overtime" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell37" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label2" runat="server" Text="12" CssClass="counterLbl"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>                                                
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell ID="TableCell33" runat="server" Width="150px" CssClass="tabCell">
                                            <asp:Table ID="Table16" runat="server" Width="150px" CssClass="tabTable">
                                                <asp:TableRow ID="TableRow27" runat="server" Width="150px">
                                                    <asp:TableCell ID="TableCell34" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label8" runat="server" Width="70px" Text="Stop working" CssClass="contentSmallLblLeft"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCell38" runat="server" Width="75px" CssClass="tabMidAlignCell">
                                                        <asp:Label ID="Label3" runat="server" Text="16" CssClass="counterLbl"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow21" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell150" runat="server" Width="1000px" CssClass="tabCell">
                                <asp:Table ID="Table2" runat="server" Width="1000px" CssClass="tabTable">
                                    <asp:TableRow ID="TableRow39" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell23" runat="server" Width="1000px" HorizontalAlign="Center" CssClass="tabCell">
                                            <asp:Label ID="lblGraphPeriod" runat="server" Text="Januar 2011" CssClass="counterLbl"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow35" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell3" runat="server" Width="1000px" CssClass="tabCell">
                                            <asp:Label ID="lblDate" runat="server" Width="135px" Text="Date" CssClass="contentLblLeft"></asp:Label>
                                            <asp:Label ID="lblTotal" runat="server" Width="70px" Text="Total" CssClass="contentLblLeft"></asp:Label>
                                            <asp:Label ID="Label12" runat="server" Width="30px" Text="00" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label14" runat="server" Width="30px" Text="01" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label15" runat="server" Width="30px" Text="02" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label16" runat="server" Width="30px" Text="03" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label17" runat="server" Width="30px" Text="04" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label18" runat="server" Width="30px" Text="05" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label19" runat="server" Width="30px" Text="06" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label20" runat="server" Width="30px" Text="07" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label21" runat="server" Width="30px" Text="08" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label22" runat="server" Width="30px" Text="09" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label23" runat="server" Width="30px" Text="10" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label24" runat="server" Width="30px" Text="11" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label25" runat="server" Width="30px" Text="12" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label26" runat="server" Width="30px" Text="13" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label27" runat="server" Width="30px" Text="14" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label28" runat="server" Width="30px" Text="15" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label29" runat="server" Width="30px" Text="16" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label30" runat="server" Width="30px" Text="17" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label31" runat="server" Width="30px" Text="18" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label32" runat="server" Width="30px" Text="19" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label33" runat="server" Width="30px" Text="20" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label34" runat="server" Width="30px" Text="21" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label35" runat="server" Width="30px" Text="22" CssClass="contentHdrLbl"></asp:Label>
                                            <asp:Label ID="Label36" runat="server" Width="30px" Text="23" CssClass="contentHdrLbl"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="TableRow36" runat="server" Width="1000px">
                                        <asp:TableCell ID="TableCell50" runat="server" Width="1000px" CssClass="tabCell">
                                            <asp:Panel ID="resultPanel" runat="server" Width="990px" Height="440px" ScrollBars="Auto" CssClass="resultPanel">
                                                <asp:PlaceHolder ID="ctrlHolder" runat="server"></asp:PlaceHolder>
                                            </asp:Panel>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table15" runat="server" Width="1180px" CssClass="tabNoBorderTable">
            <asp:TableRow ID="TableRow2" runat="server" Width="1180px">
                <asp:TableCell ID="TableCell151" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                    <asp:Table ID="Table3" runat="server" Width="150px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow3" runat="server" Width="150px">
                            <asp:TableCell ID="TableCell5" runat="server" Width="150px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>                            
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell ID="TableCell4" runat="server" Width="1000px" CssClass="tabCell">
                    <asp:Table ID="Table14" runat="server" Width="1000px" CssClass="tabTable">
                        <asp:TableRow ID="TableRow25" runat="server" Width="1000px">
                            <asp:TableCell ID="TableCell41" runat="server" Width="310px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnPrev" runat="server" Text="<<" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell42" runat="server" Width="310px" HorizontalAlign="Center" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnNext" runat="server" Text=">>" CssClass="contentBtn"></asp:Button>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell43" runat="server" Width="310px" HorizontalAlign="Right" CssClass="tabMidAlignCell">
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="contentBtn"></asp:Button>
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
