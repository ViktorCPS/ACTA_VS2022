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
using System.Reflection;
using System.Web.Compilation;
using Common;
using TransferObjects;
using Util;

namespace CommonWeb.Pickers
{
    public partial class WUnitsPicker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

                if (!Page.IsPostBack)
                {
                    populateWUTree();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WUnitsPicker.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateWUTree()
        {
            try
            {
                WorkingUnit wUnit = new WorkingUnit(Session[Constants.sessionConnection]);
                string wUnits = "";

                if (Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                {
                    foreach (int wuID in (List<int>)Session[Constants.sessionLoginCategoryWUnits])
                    {
                        wUnits += wuID.ToString().Trim() + ",";
                    }

                    if (wUnits.Length > 0)
                        wUnits = wUnits.Substring(0, wUnits.Length - 1);

                    List<WorkingUnitTO> wuList = wUnit.Search(wUnits);
                    Dictionary<int, List<WorkingUnitTO>> wuDic = new Dictionary<int, List<WorkingUnitTO>>();

                    foreach (WorkingUnitTO wu in wuList)
                    {
                        wuDic.Add(wu.WorkingUnitID, new List<WorkingUnitTO>());
                    }

                    foreach (WorkingUnitTO wu in wuList)
                    {
                        if (wu.ParentWorkingUID != wu.WorkingUnitID && wuDic.ContainsKey(wu.ParentWorkingUID))
                            wuDic[wu.ParentWorkingUID].Add(wu);
                    }

                    foreach (WorkingUnitTO wu in wuList)
                    {
                        if (wu.WorkingUnitID == wu.ParentWorkingUID || !wuDic.ContainsKey(wu.ParentWorkingUID))
                        {
                            TreeNode node = new TreeNode(wu.Name, wu.WorkingUnitID.ToString());
                            node.ToolTip = wu.Description.Trim();
                            if (wuDic.ContainsKey(wu.WorkingUnitID) && wuDic[wu.WorkingUnitID].Count > 0 && Request.QueryString["isTmp"] != null
                                && Request.QueryString["isTmp"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                                node.SelectAction = TreeNodeSelectAction.None;
                            else
                                node.SelectAction = TreeNodeSelectAction.Select;
                            treeWU.Nodes.Add(node);
                            if (wuDic.ContainsKey(wu.WorkingUnitID) && wuDic[wu.WorkingUnitID].Count > 0)
                                populateChildNodes(node, wuDic);                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateChildNodes(TreeNode node, Dictionary<int, List<WorkingUnitTO>> wuDic)
        {
            try
            {
                if (wuDic.ContainsKey(int.Parse(node.Value)))
                {
                    foreach (WorkingUnitTO wu in wuDic[int.Parse(node.Value)])
                    {
                        TreeNode childNode = new TreeNode(wu.Name, wu.WorkingUnitID.ToString());
                        childNode.ToolTip = wu.Description.Trim();
                        if (wuDic.ContainsKey(wu.WorkingUnitID) && wuDic[wu.WorkingUnitID].Count > 0 && Request.QueryString["isTmp"] != null
                                && Request.QueryString["isTmp"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                            childNode.SelectAction = TreeNodeSelectAction.None;
                        else
                            childNode.SelectAction = TreeNodeSelectAction.Select;
                        node.ChildNodes.Add(childNode);
                        if (wuDic.ContainsKey(wu.WorkingUnitID) && wuDic[wu.WorkingUnitID].Count > 0)
                            populateChildNodes(childNode, wuDic);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void treeWU_SelectedNodeChanged(object sender, EventArgs e)        
        {
            try
            {
                // if wu is selectd for loans, only UTE (lief node) can be selected
                if (Request.QueryString["isTmp"] != null && Request.QueryString["isTmp"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper())
                    && treeWU.SelectedNode.ChildNodes.Count > 0)
                    return;

                int wuID = -1;
                if (!int.TryParse(treeWU.SelectedNode.Value, out wuID))
                    wuID = -1;

                if (wuID != -1)
                {
                    // put selected wuID in session so parent page could take it
                    if (Request.QueryString["isTmp"] != null && Request.QueryString["isTmp"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                        Session[Constants.sessionSelectedTempWUID] = wuID;
                    else
                    {
                        Session[Constants.sessionSelectedWUID] = wuID;
                        Session[Constants.sessionWU] = wuID;
                        Session[Constants.sessionOU] = null;
                    }

                    if (Request.QueryString["postBackCtrl"] != null)
                    {
                        string jsString = "var openerWindow = window.dialogArguments; openerWindow.pagePostBack('" + Request.QueryString["postBackCtrl"] + "'); closeWindow();";
                        //window.opener.SetDataFromPopup(data);
                        // initiate parent page post back and close pop up window
                        //ClientScript.RegisterStartupScript(GetType(), "close", jsString, true);

                        //VIKTOR 15.11.2023 - probao ove fje da vidim hoce li odraditi callback kako treba
                        //GetType(), "refresh", "window.opener.location=window.opener.location;", true
                        
                        ScriptManager.RegisterStartupScript(this, GetType(), "close", "window.opener.location=window.opener.location; window.close();", true);
                        //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClosePopup", jsString, true);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WUnitsPicker.treeWU_SelectedNodeChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/CommonWeb/Pickers/WUnitsPicker.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
