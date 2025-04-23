using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Common;
using TransferObjects;
using Util;

namespace CommonWeb.Pickers
{
    public partial class OrgUnitsPicker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

                if (!Page.IsPostBack)
                {
                    populateOUTree();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in OrgUnitsPicker.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateOUTree()
        {
            try
            {
                OrganizationalUnit oUnit = new OrganizationalUnit(Session[Constants.sessionConnection]);
                string oUnits = "";

                if (Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>)
                {
                    foreach (int ouID in (List<int>)Session[Constants.sessionLoginCategoryOUnits])
                    {
                        oUnits += ouID.ToString().Trim() + ",";
                    }

                    if (oUnits.Length > 0)
                        oUnits = oUnits.Substring(0, oUnits.Length - 1);

                    List<OrganizationalUnitTO> ouList = oUnit.Search(oUnits);
                    Dictionary<int, List<OrganizationalUnitTO>> ouDic = new Dictionary<int, List<OrganizationalUnitTO>>();

                    foreach (OrganizationalUnitTO ou in ouList)
                    {
                        ouDic.Add(ou.OrgUnitID, new List<OrganizationalUnitTO>());
                    }

                    foreach (OrganizationalUnitTO ou in ouList)
                    {
                        if (ou.ParentOrgUnitID != ou.OrgUnitID && ouDic.ContainsKey(ou.ParentOrgUnitID))
                            ouDic[ou.ParentOrgUnitID].Add(ou);
                    }

                    foreach (OrganizationalUnitTO ou in ouList)
                    {
                        if (ou.OrgUnitID == ou.ParentOrgUnitID || !ouDic.ContainsKey(ou.ParentOrgUnitID))
                        {
                            TreeNode node = new TreeNode(ou.Name, ou.OrgUnitID.ToString());
                            node.ToolTip = ou.Desc.Trim();
                            node.SelectAction = TreeNodeSelectAction.Select;
                            treeOU.Nodes.Add(node);
                            if (ouDic.ContainsKey(ou.OrgUnitID) && ouDic[ou.OrgUnitID].Count > 0)
                                populateChildNodes(node, ouDic);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateChildNodes(TreeNode node, Dictionary<int, List<OrganizationalUnitTO>> ouDic)
        {
            try
            {
                if (ouDic.ContainsKey(int.Parse(node.Value)))
                {
                    foreach (OrganizationalUnitTO ou in ouDic[int.Parse(node.Value)])
                    {
                        TreeNode childNode = new TreeNode(ou.Name, ou.OrgUnitID.ToString());
                        childNode.ToolTip = ou.Desc.Trim();
                        childNode.SelectAction = TreeNodeSelectAction.Select;
                        node.ChildNodes.Add(childNode);
                        if (ouDic.ContainsKey(ou.OrgUnitID) && ouDic[ou.OrgUnitID].Count > 0)
                            populateChildNodes(childNode, ouDic);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void treeOU_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                int ouID = -1;
                if (!int.TryParse(treeOU.SelectedNode.Value, out ouID))
                    ouID = -1;

                if (ouID != -1)
                {
                    // put selected wuID in session so parent page could take it
                    Session[Constants.sessionSelectedOUID] = ouID;
                    Session[Constants.sessionOU] = ouID;
                    Session[Constants.sessionWU] = null;

                    if (Request.QueryString["postBackCtrl"] != null)
                    {
                       // string jsString = "window.opener.pagePostBack('" + Request.QueryString["postBackCtrl"] + "'); window.close();";
                        string jsString = "var openerWindow = window.dialogArguments; openerWindow.pagePostBack('" + Request.QueryString["postBackCtrl"] + "'); window.close();";
                        // initiate parent page post back and close pop up window
                        
                        ClientScript.RegisterStartupScript(GetType(), "close", "window.opener.location=window.opener.location; window.close();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in OrgUnitsPicker.treeWU_SelectedNodeChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/CommonWeb/Pickers/OrgUnitsPicker.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
