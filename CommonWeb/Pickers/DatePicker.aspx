<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatePicker.aspx.cs" Inherits="CommonWeb.Pickers.DatePicker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
    <base target="_self" />
</head>
<body onload="this.window.focus();">
    <form id="form1" method="post" runat="server">
    <div>
         <asp:Calendar ID="calendar" runat="server" FirstDayOfWeek="Monday" ShowGridLines="false" OnSelectionChanged="DateChange" CssClass="calendar">
            <TodayDayStyle CssClass="calendarToday" />
			<SelectedDayStyle CssClass="calendarSelector" />
			<SelectorStyle CssClass="calendarSelector" />
			<DayHeaderStyle CssClass="calendarDayHdr" />
			<DayStyle CssClass="calendarDay" />			
			<TitleStyle CssClass="calendarTitle" />
			<OtherMonthDayStyle CssClass="calendarOtherMonth" />
			<NextPrevStyle CssClass="calendarNextPrev" />
			<WeekendDayStyle CssClass="calendarWeekend" />
        </asp:Calendar>
    </div>
    </form>
</body>
</html>
