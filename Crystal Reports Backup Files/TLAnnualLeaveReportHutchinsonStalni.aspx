<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TLAnnualLeaveReportHutchinsonStalni.aspx.cs" Inherits="ReportsWeb.TLAnnualLeaveReportHutchinsonStalni" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" Height="498px" Width="837px">
        <LocalReport ReportPath="ReportsWeb\TLAnnualLeaveReportHutchinsonStalni.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    </form>
</body>
</html>
