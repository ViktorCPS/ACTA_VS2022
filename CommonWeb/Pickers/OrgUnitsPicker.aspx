<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgUnitsPicker.aspx.cs" Inherits="CommonWeb.Pickers.OrgUnitsPicker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ACTA Web</title>
    <script language="JavaScript" type="text/javascript" src="/ACTAWeb/CommonWeb/js/Functions.js"></script>
    <link href="/ACTAWeb/CommonWeb/css/ACTAWebStylesheet.css" type="text/css" rel="stylesheet"/>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TreeView ID="treeOU" runat="server" CssClass="treeView" ShowCheckBoxes="None" ShowExpandCollapse="true" OnSelectedNodeChanged="treeOU_SelectedNodeChanged">
            <NodeStyle CssClass="treeViewNode" />                                
        </asp:TreeView>
    </div>
    </form>
</body>
</html>
