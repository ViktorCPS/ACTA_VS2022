using System;
using System.Collections;
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
using System.ComponentModel;
using System.Resources;
using System.Globalization;
using System.Drawing;

using TransferObjects;
using Util;
using ReportsWeb;

namespace ACTAWebUI
{
    public partial class TabPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    setLanguage();

                    Session["FirstVisibleTab"] = null;
                    SetVisibleTabs(0);

                    if (MainMenu.Items.Count > 0)
                        MainMenu.Items[0].Selected = true;

                    // get tab count
                    int tabCount = MainMenu.Items.Count;

                    if (MainMenu.Items.Count > 0 && (MainMenu.Items[MainMenu.Items.Count - 1].Value == "Prev" || MainMenu.Items[MainMenu.Items.Count - 1].Value == "Next"))
                        tabCount--;

                    if (MainMenu.Items.Count > 1 && (MainMenu.Items[MainMenu.Items.Count - 2].Value == "Prev" || MainMenu.Items[MainMenu.Items.Count - 2].Value == "Next"))
                        tabCount--;

                    for (int i = 0; i < MainMenu.Items.Count; i++)
                    {
                        if (i == 0)
                        {
                            MainMenu.Items[i].Selected = true;
                            for (int j = 0; j < MultiView1.Views.Count; j++)
                            {
                                if (MultiView1.Views[j].ID == MainMenu.Items[i].Value)
                                {
                                    MultiView1.SetActiveView(MultiView1.Views[j]);                                    
                                    break;
                                }
                            }
                        }

                        CommonWeb.Misc.setMainMenuImage(MainMenu, i, MainMenu.Items[i].Selected, tabCount);
                        CommonWeb.Misc.setMainMenuSeparator(MainMenu, i, MainMenu.Items[i].Selected, tabCount);
                    }

                    int emplID = -1;
                    if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                        emplID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                    // TL tabs
                    TLTMDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx?reloadState=false";
                    TLReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLReportsPage.aspx?reloadState=false";
                    TLMassiveInputIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLMassiveInputPage.aspx?reloadState=false";
                    TLClockDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx?reloadState=false";
                    TLLoansIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLLoansPage.aspx?reloadState=false";
                    TLAnnualLeaveIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx?reloadState=false";
                    TLDetailsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx?reloadState=false";
                    TLWUStatisticalReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx?reloadState=false";

                    // WC tabs
                    WCTMDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx?reloadState=false&emplID=" + emplID;
                    WCReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?reloadState=false&emplID=" + emplID;
                    WCFormsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCFormsPage.aspx?reloadState=false&emplID=" + emplID;
                    WCClockDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCClockDataPage.aspx?reloadState=false&emplID=" + emplID;
                    WCDetailsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCDetailsPage.aspx?reloadState=false&emplID=" + emplID;
                    WCAnnualLeaveIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx?reloadState=false&emplID=" + emplID;
                    WCPayslipsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCPayslips.aspx?reloadState=false&emplID=" + emplID;

                    // WC Manager tabs
                    WCManagerTMDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx?reloadState=false";
                    WCManagerVerificationIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx?reloadState=false";
                    WCManagerClockDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx?reloadState=false";
                    WCManagerWUStatisticalReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx?reloadState=false";

                    // HRSCC tabs
                    HRSSCTMDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx?reloadState=false";
                    HRSSCMassiveInputIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLMassiveInputPage.aspx?reloadState=false";
                    HRSSCCountersIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/HRSSCCountersPage.aspx?reloadState=false";
                    HRSSCConfirmationAbsencesIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/HRSSCConfirmationPage.aspx?reloadState=false";
                    HRSSCVerificationIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCManagerVerificationPage.aspx?reloadState=false";
                    HRSSCOutstandingDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx?reloadState=false";
                    HRSSCClockDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx?reloadState=false";
                    HRSSCAnnualLeaveIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx?reloadState=false";
                    HRSSCDetailsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx?reloadState=false";
                    //HRSSCWUStatisticalReportsIframe.Attributes["src"] = "";

                    // HR Legal entity tabs
                    HRLegalEntityTMDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLTMDataPage.aspx?reloadState=false";
                    HRLegalEntityReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLReportsPage.aspx?reloadState=false";
                    HRLegalEntityClockDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx?reloadState=false";
                    HRLegalEntityLoansIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLLoansPage.aspx?reloadState=false";
                    HRLegalEntityDetailsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLDetailedDataPage.aspx?reloadState=false";
                    HRLegalEntityWUStatisticalReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx?reloadState=false";
                    HRLegalEntityWorkAnalyzeReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WorkAnalyzeReports.aspx?reloadState=false&emplID=" + emplID;
                    HRLegalEntityBufferReportIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/BufferReport.aspx?reloadState=false";
                    HRLegalEntityOutstandingDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/HRSSCOutstandingDataPage.aspx?reloadState=false";

                    // Medical Check Up tabs
                    MCSchedulingIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/MCSchedulingPage.aspx?reloadState=false";
                    MCVisitsSearchIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/MCVisitsSearchPage.aspx?reloadState=false";                    
                    MCEmployeeDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/MCEmployeeDataPage.aspx?reloadState=false";
                    MCReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/MCReportsPage.aspx?reloadState=false";

                    // BC Self tabs
                    BCTMDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx?reloadState=false&emplID=" + emplID;                    
                    BCReportsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?reloadState=false&emplID=" + emplID;
                    BCFormsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCFormsPage.aspx?reloadState=false&emplID=" + emplID;
                    BCClockDataIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCClockDataPage.aspx?reloadState=false&emplID=" + emplID;
                    BCDetailsIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCDetailsPage.aspx?reloadState=false&emplID=" + emplID;
                    BCAnnualLeaveIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx?reloadState=false&emplID=" + emplID;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TabPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TabPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void MainMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                if (e.Item.Value == "Prev")                
                    SetVisibleTabs(-1);
                else if (e.Item.Value == "Next")
                    SetVisibleTabs(1);
                else
                {
                    for (int i = 0; i < MainMenu.Items.Count; i++)
                    {
                        if (MainMenu.Items[i].Value.Equals(e.Item.Value))
                        {
                            MainMenu.Items[i].Selected = true;
                            for (int j = 0; j < MultiView1.Views.Count; j++)
                            {
                                if (MultiView1.Views[j].ID == MainMenu.Items[i].Value)
                                {
                                    MultiView1.SetActiveView(MultiView1.Views[j]);
                                    break;
                                }
                            }
                        }
                    }
                }

                // get tab count
                int tabCount = MainMenu.Items.Count;

                if (MainMenu.Items.Count > 0 && (MainMenu.Items[MainMenu.Items.Count - 1].Value == "Prev" || MainMenu.Items[MainMenu.Items.Count - 1].Value == "Next"))
                    tabCount--;

                if (MainMenu.Items.Count > 1 && (MainMenu.Items[MainMenu.Items.Count - 2].Value == "Prev" || MainMenu.Items[MainMenu.Items.Count - 2].Value == "Next"))
                    tabCount--;

                for (int i = 0; i < MainMenu.Items.Count; i++)
                {
                    CommonWeb.Misc.setMainMenuImage(MainMenu, i, MainMenu.Items[i].Selected, tabCount);
                    CommonWeb.Misc.setMainMenuSeparator(MainMenu, i, MainMenu.Items[i].Selected, tabCount);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TabPage.MainMenu_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TabPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TabPage).Assembly);

                Menu1.FindItem("TLClockData").Text = rm.GetString("tabClockData", culture);
                Menu1.FindItem("TLDetails").Text = rm.GetString("tabDetails", culture);
                Menu1.FindItem("TLLoans").Text = rm.GetString("tabLoans", culture);
                Menu1.FindItem("TLAnnualLeave").Text = rm.GetString("tabAnnualLeave", culture);
                Menu1.FindItem("TLMassiveInput").Text = rm.GetString("tabMassiveInput", culture);
                Menu1.FindItem("TLReports").Text = rm.GetString("tabReports", culture);
                Menu1.FindItem("TLTMData").Text = rm.GetString("tabTMData", culture);
                Menu1.FindItem("TLWUStatisticalReports").Text = rm.GetString("tabWUStatisticReports", culture);
                Menu1.FindItem("WCAnnualLeave").Text = rm.GetString("tabAbsences", culture);
                Menu1.FindItem("WCClockData").Text = rm.GetString("tabClockData", culture);
                Menu1.FindItem("WCDetails").Text = rm.GetString("tabDetails", culture);
                Menu1.FindItem("WCForms").Text = rm.GetString("tabForms", culture);
                Menu1.FindItem("WCReports").Text = rm.GetString("tabReports", culture);
                Menu1.FindItem("WCPayslips").Text = rm.GetString("tabPayslip", culture);
                Menu1.FindItem("WCTMData").Text = rm.GetString("tabTMData", culture);
                Menu1.FindItem("WCManagerTMData").Text = rm.GetString("tabTMData", culture);
                Menu1.FindItem("WCManagerVerification").Text = rm.GetString("tabVerification", culture);
                Menu1.FindItem("WCManagerClockData").Text = rm.GetString("tabClockData", culture);
                Menu1.FindItem("WCManagerWUStatisticalReports").Text = rm.GetString("tabWUStatisticReports", culture);
                Menu1.FindItem("HRSSCConfirmationAbsences").Text = rm.GetString("tabConfirmationAbsences", culture);
                Menu1.FindItem("HRSSCCounters").Text = rm.GetString("tabCounters", culture);
                Menu1.FindItem("HRSSCAnnualLeave").Text = rm.GetString("tabAnnualLeave", culture);
                Menu1.FindItem("HRSSCMassiveInput").Text = rm.GetString("tabMassiveInput", culture);
                Menu1.FindItem("HRSSCOutstandingData").Text = rm.GetString("tabOutstandingData", culture);
                Menu1.FindItem("HRSSCTMData").Text = rm.GetString("tabTMData", culture);
                Menu1.FindItem("HRSSCVerification").Text = rm.GetString("tabVerification", culture);
                Menu1.FindItem("HRSSCClockData").Text = rm.GetString("tabClockData", culture);
                Menu1.FindItem("HRSSCDetails").Text = rm.GetString("tabDetails", culture);                
                Menu1.FindItem("HRLegalEntityClockData").Text = rm.GetString("tabClockData", culture);
                Menu1.FindItem("HRLegalEntityDetails").Text = rm.GetString("tabDetails", culture);
                Menu1.FindItem("HRLegalEntityLoans").Text = rm.GetString("tabLoans", culture);
                Menu1.FindItem("HRLegalEntityReports").Text = rm.GetString("tabReports", culture);
                Menu1.FindItem("HRLegalEntityTMData").Text = rm.GetString("tabTMData", culture);
                Menu1.FindItem("HRLegalEntityWUStatisticalReports").Text = rm.GetString("tabWUStatisticReports", culture);
                //Menu1.FindItem("HRLegalEntityWorkAnalyzeReports").Text = rm.GetString("tabWorkAnalyzeReports", culture);
                Menu1.FindItem("HRLegalEntityWorkAnalyzeReports").Text = rm.GetString("tabAdditionalReports", culture);
                Menu1.FindItem("HRLegalEntityBufferReport").Text = rm.GetString("tabBufferReport", culture);
                Menu1.FindItem("HRLegalEntityOutstandingData").Text = rm.GetString("tabOutstandingData", culture);
                Menu1.FindItem("MCScheduling").Text = rm.GetString("tabScheduling", culture);
                Menu1.FindItem("MCVisitsSearch").Text = rm.GetString("tabSearch", culture);
                Menu1.FindItem("MCEmployeeData").Text = rm.GetString("tabEmplData", culture);
                Menu1.FindItem("MCReports").Text = rm.GetString("tabReports", culture);
                Menu1.FindItem("BCAnnualLeave").Text = rm.GetString("tabAbsences", culture);
                Menu1.FindItem("BCClockData").Text = rm.GetString("tabClockData", culture);
                Menu1.FindItem("BCDetails").Text = rm.GetString("tabDetails", culture);
                Menu1.FindItem("BCForms").Text = rm.GetString("tabForms", culture);
                Menu1.FindItem("BCReports").Text = rm.GetString("tabReports", culture);
                Menu1.FindItem("BCTMData").Text = rm.GetString("tabTMData", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetVisibleTabs(int movement)
        {
            try
            {
                // EVERY TAB MUST HAVE CORRESPONDING VIEW!!!
                // TAB (MENU ITEM) VALUE AND VIEW ID MUST BE THE SAME!!!
                List<string> tabs = new List<string>();

                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && Constants.CategoryTabs.ContainsKey(((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID))
                    tabs = Constants.CategoryTabs[((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID];

                // remove loans and forms for Grundfos
                string costumer = Common.Misc.getCustomer(null);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                if (cost == (int)Constants.Customers.Grundfos)
                {
                    List<string> GrundfosTabs = new List<string>();
                    foreach (string tab in tabs)
                    {
                        if (!Constants.GrundfosUnvisibleTabs.Contains(tab))
                            GrundfosTabs.Add(tab);
                    }
                    tabs = GrundfosTabs;
                }

                int firstVisible = 0;
                if (Session["FirstVisibleTab"] != null && Session["FirstVisibleTab"] is int)
                    firstVisible = (int)Session["FirstVisibleTab"];

                firstVisible += movement;
                Session["FirstVisibleTab"] = firstVisible;

                int lastVisible = firstVisible + Constants.maxVisibleTabs - 1;
                                
                bool showPrevNavigation = tabs.Count > Constants.maxVisibleTabs && firstVisible > 0;
                bool showNextNavigation = tabs.Count > Constants.maxVisibleTabs && lastVisible < tabs.Count - 1;

                int counter = -1;
                List<string> visibleTabs = new List<string>();
                MainMenu.Items.Clear();
                int activeTabView = MultiView1.ActiveViewIndex;
                string activeTab = "";
                if (activeTabView >= 0 && activeTabView < MultiView1.Views.Count)
                    activeTab = MultiView1.Views[activeTabView].ID;                

                for (int i = 0; i < Menu1.Items.Count; i++)
                {
                    if (!tabs.Contains(Menu1.Items[i].Value))
                    {
                        if ((showPrevNavigation && (Menu1.Items[i].Value == "Prev") || (showNextNavigation && Menu1.Items[i].Value == "Next")))
                        {
                            MenuItem tabItem = new MenuItem();
                            tabItem.Text = Menu1.Items[i].Text;
                            tabItem.Value = Menu1.Items[i].Value;
                            MainMenu.Items.Add(tabItem);
                        }
                    }
                    else
                    {
                        counter++;
                        if (counter >= firstVisible && counter <= lastVisible)
                        {
                            visibleTabs.Add(Menu1.Items[i].Value);
                            MenuItem tabItem = new MenuItem();
                            tabItem.Text = Menu1.Items[i].Text;
                            tabItem.Value = Menu1.Items[i].Value;
                            if (tabItem.Value == activeTab)
                                tabItem.Selected = true;
                            MainMenu.Items.Add(tabItem);
                        }
                    }
                }

                //IEnumerator viewEnumerator = MultiView1.Views.GetEnumerator();
                //while (viewEnumerator.MoveNext())
                //{
                //    if (!visibleTabs.Contains(((View)viewEnumerator.Current).ID))
                //    {
                //        MultiView1.Views.Remove((View)viewEnumerator.Current);
                //        viewEnumerator = MultiView1.Views.GetEnumerator();
                //    }
                //}

                //IEnumerator menuEnumerator = Menu1.Items.GetEnumerator();
                //while (menuEnumerator.MoveNext())
                //{
                //    if (!tabs.Contains(((MenuItem)menuEnumerator.Current).Value))
                //    {
                //        if ((showPrevNavigation && ((MenuItem)menuEnumerator.Current).Value == "Prev") || (showNextNavigation && ((MenuItem)menuEnumerator.Current).Value == "Next"))
                //            continue;

                //        Menu1.Items.Remove((MenuItem)menuEnumerator.Current);
                //        menuEnumerator = Menu1.Items.GetEnumerator();
                //    }
                //}

                //IEnumerator viewEnumerator = MultiView1.Views.GetEnumerator();
                //while (viewEnumerator.MoveNext())
                //{
                //    if (!tabs.Contains(((View)viewEnumerator.Current).ID))
                //    {
                //        MultiView1.Views.Remove((View)viewEnumerator.Current);
                //        viewEnumerator = MultiView1.Views.GetEnumerator();
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setMenuImage(int index, bool isActive)
        {
            try
            {
                if (index >= 0 && index < Menu1.Items.Count)
                {
                    if (isActive)
                    {
                        if (index == 0)
                            Menu1.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/activeFirst.jpg";
                        else
                            Menu1.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactive-activeSeparator.jpg";
                    }
                    else
                    {
                        if (index == 0)
                            Menu1.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveFirst.jpg";
                        else if (!Menu1.Items[index - 1].Selected)
                            Menu1.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveSeparator.jpg";
                        else
                            Menu1.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/active-nonactiveSeparator.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setMenuSeparator(int index, bool isActive)
        {
            try
            {
                if (index == Menu1.Items.Count - 1)
                {
                    if (isActive)
                    {
                        Menu1.Items[index].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/activeLast.jpg";
                    }
                    else
                    {
                        Menu1.Items[index].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveLast.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
