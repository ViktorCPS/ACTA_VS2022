﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TLPaidLeavesReportPage.aspx.cs" Inherits="ReportsWeb.TLPayedLeavesReportPage" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 782px; height: 800px;">
    
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="797px" 
            Width="785px">
            <LocalReport ReportPath="ReportsWeb\TLPaidLeavesReport.rdlc"></LocalReport>
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
