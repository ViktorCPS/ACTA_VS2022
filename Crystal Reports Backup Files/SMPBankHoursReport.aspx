<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMPBankHoursReport.aspx.cs" Inherits="ReportsWeb.sr.SMPBankHoursReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 1098px; height: 500px;">
    
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="450px" Width="812px">
            <LocalReport ReportPath="ReportsWeb\sr\SMPBankHoursReport.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
