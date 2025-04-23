<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestNotficationSystemWeb._Default"  %>

<%@ Register assembly="BasicFrame.WebControls.BasicDatePicker" namespace="BasicFrame.WebControls" tagprefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>  
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
</head>
<body id="bod" runat="server">
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblEmail" runat="server" Text="E mail address: "></asp:Label>
        <asp:TextBox ID="txtEmail" runat="server" style="margin-left: 21px" 
            Height="22px" Width="133px"></asp:TextBox>
        <asp:Button ID="btExit" runat="server" onclick="btExit_Click" Text="Exit" 
            style="margin-left: 455px" />
        <br />
        <br />
        <asp:Label ID="lblPass" runat="server" Text="Password: "></asp:Label>
 <asp:TextBox ID="txtPassword" runat="server" style="margin-left: 49px" Width="132px" 
            TextMode="Password"></asp:TextBox><br /><br/>
        <asp:Label ID="lblfrom" runat="server" Text="From date: "></asp:Label>
        <asp:Calendar ID="dtPFromDate" runat="server" BackColor="White" 
            BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt" 
            ForeColor="Black" Height="180px" 
            Width="200px" DayNameFormat="Shortest" 
            VisibleDate="2011-11-29" CellPadding="4" 
            SelectedDate="12/01/2011 16:21:33">
            <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
            <SelectorStyle BackColor="#CCCCCC" />
            <WeekendDayStyle BackColor="#FFFFCC" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <OtherMonthDayStyle ForeColor="Gray" />
            <NextPrevStyle 
                VerticalAlign="Bottom" />
            <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="#CCCCCC" />
            <TitleStyle BackColor="#999999" BorderColor="Black" 
                Font-Bold="True" />
        </asp:Calendar>
        <br/>
        <asp:Label ID="lblTo" runat="server" Text="To date: "></asp:Label>
        <asp:Calendar ID="dtpToDate" runat="server" BackColor="White" 
            BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt" 
            ForeColor="Black" Height="180px" 
             Width="200px" DayNameFormat="Shortest" CellPadding="4" 
            SelectedDate="12/01/2011 15:55:18">
            <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
            <SelectorStyle BackColor="#CCCCCC" />
            <WeekendDayStyle BackColor="#FFFFCC" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <NextPrevStyle 
                VerticalAlign="Bottom" />
            <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="#CCCCCC" />
            <TitleStyle BackColor="#999999" BorderColor="Black" 
                Font-Bold="True" />
        </asp:Calendar>
    </div>
    </form>
</body>
</html>
