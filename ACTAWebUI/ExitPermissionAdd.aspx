<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExitPermissionAdd.aspx.cs" Inherits="ACTAWebUI.ExitPermissionAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Logout.js"></script>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table3" runat="server" Width="600px" HorizontalAlign="Center" CssClass="navigationTable">
            <asp:TableRow ID="TableRow3" runat="server" Width="600px">
                <asp:TableCell ID="TableCell1" runat="server" Width="100px" HorizontalAlign="Left" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnMenu" runat="server" Text="Menu" OnClick="lbtnMenu_Click" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell42" runat="server" Width="50px" HorizontalAlign="Left" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnBack" runat="server" Text="Back" OnClick="lbtnBack_Click" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" HorizontalAlign="Right" Width="450px" CssClass="navigationCell">
                    <asp:LinkButton ID="lbtnExit" runat="server" Text="Exit" CssClass="navigationLBtn"></asp:LinkButton>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table1" runat="server" Width="600px" HorizontalAlign="Center" CssClass="hdrTable">
            <asp:TableRow ID="TableRow1" runat="server" Width="600px">
                <asp:TableCell ID="TableCell3" runat="server" Width="600px" CssClass="hdrCell">
                    <asp:Label ID="lblExitPermissionAddUpd" runat="server" Width="590px" Text="Exit permission add/update" CssClass="hdrTitle"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table2" runat="server" Width="600px" HorizontalAlign="Center" CssClass="contentTable">
            <asp:TableRow ID="TableRow8" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>
            <asp:TableRow ID="TableRow6" runat="server" Width="600px">
                <asp:TableCell ID="TableCell12" runat="server" Width="150px" CssClass="contentCell">
                    <asp:Label ID="lblType" runat="server" Width="145px" Text="Type:" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell15" runat="server" Width="250px" CssClass="contentCell">
                    <asp:DropDownList ID="cbType" runat="server" Width="240px" AutoPostBack="false" CssClass="contentDDList"></asp:DropDownList>                    
                </asp:TableCell>
                <asp:TableCell ID="TableCell14" runat="server" Width="35px" CssClass="contentCell"></asp:TableCell>
                <asp:TableCell ID="TableCell27" runat="server" Width="165px" CssClass="contentCell"></asp:TableCell>
            </asp:TableRow> 
            <asp:TableRow ID="TableRow2" runat="server" Width="600px">
                <asp:TableCell ID="TableCell4" runat="server" Width="150px" CssClass="contentCell">
                    <asp:Label ID="lblDate" runat="server" Width="145px" Text="Date:" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell5" runat="server" Width="250px" CssClass="contentCell">
                    <asp:TextBox ID="tbDate" runat="server" Width="240px" CssClass="contentTb"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell8" runat="server" Width="35px" HorizontalAlign="Left" CssClass="contentCell">
                    <asp:ImageButton ID="btnDate" runat="server" ImageUrl="/ACTAWeb/CommonWeb/images/calendar.gif" CssClass="contentImgBtn"></asp:ImageButton>
                </asp:TableCell>
                <asp:TableCell ID="TableCell26" runat="server" Width="165px" CssClass="contentCell">
                    <asp:Label ID="lblDateFormat" runat="server" Width="150px" Text="*Date format" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow5" runat="server" Width="600px">
                <asp:TableCell ID="TableCell9" runat="server" Width="150px" CssClass="contentCell">
                    <asp:Label ID="lblTime" runat="server" Width="145px" Text="Time:" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell10" runat="server" Width="250px" HorizontalAlign="Right" CssClass="contentCell">
                    <asp:TextBox ID="tbTime" runat="server" Width="100px" CssClass="contentTb"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="35px" HorizontalAlign="Left" CssClass="contentCell"></asp:TableCell>
                <asp:TableCell ID="TableCell25" runat="server" Width="165px" CssClass="contentCell">
                    <asp:Label ID="lblDateExample" runat="server" Width="150px" Text="*Date example" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow10" runat="server" Width="600px">
                <asp:TableCell ID="TableCell16" runat="server" Width="150px" CssClass="contentCell">
                    <asp:Label ID="lblOffset" runat="server" Width="145px" Text="Offset:" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell17" runat="server" Width="250px" HorizontalAlign="Right" CssClass="contentCell">
                    <asp:TextBox ID="tbOffset" runat="server" Width="100px" CssClass="contentTb"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell18" runat="server" Width="35px" HorizontalAlign="Left" CssClass="contentCell">
                    <asp:Label ID="lblMin" runat="server" Width="30px" Text="min" CssClass="contentHelpLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell19" runat="server" Width="165px" CssClass="contentCell"></asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow11" runat="server" Width="600px">
                <asp:TableCell ID="TableCell20" runat="server" Width="150px" CssClass="contentCell">
                    <asp:Label ID="lblDesc" runat="server" Width="145px" Text="Description:" CssClass="contentLbl"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell21" runat="server" Width="250px" CssClass="contentCell">
                    <asp:TextBox ID="tbDescription" runat="server" Width="240px" CssClass="contentTb"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell22" runat="server" Width="35px" HorizontalAlign="Left" CssClass="contentCell"></asp:TableCell>
                <asp:TableCell ID="TableCell23" runat="server" Width="165px" CssClass="contentCell"></asp:TableCell>                
            </asp:TableRow>            
            <asp:TableRow ID="TableRow9" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>       
            <asp:TableRow ID="TableRow4" runat="server" Width="600px">
                <asp:TableCell ID="TableCell6" runat="server" Width="150px" CssClass="contentCell"></asp:TableCell>
                <asp:TableCell ID="TableCell13" runat="server" Width="250px" HorizontalAlign="Left" CssClass="contentCell">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="contentBtn"></asp:Button>
                </asp:TableCell>
                <asp:TableCell ID="TableCell31" runat="server" Width="35px" CssClass="contentCell"></asp:TableCell>
                <asp:TableCell ID="TableCell7" runat="server" Width="165px" CssClass="contentCell"></asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow ID="TableRow12" runat="server" Width="600px" CssClass="contentSeparatorRow"></asp:TableRow>        		
        </asp:Table>
        <asp:Table ID="Table5" runat="server" Width="600px" HorizontalAlign="Center" CssClass="footerTable">
            <asp:TableRow ID="TableRow7" runat="server" Width="600px">
                <asp:TableCell ID="TableCell39" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblError" runat="server" Width="590px" Text="Data validation error" CssClass="errorText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow" runat="server" Width="600px">
                <asp:TableCell ID="TableCell32" runat="server" Width="600px" CssClass="footerCell">
                    <asp:Label ID="lblLoggedInUser" runat="server" Width="590px" Text="Logged in:" CssClass="loginUserText"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
