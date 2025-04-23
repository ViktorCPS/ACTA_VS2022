using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Data.OleDb;

using SyncDataAccess;
using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class SynchronizationPreview : Form
    {
        // FS
        public const int FSUnitIDIndex = 0;
        public const int FSCompanyCodeIndex = 1;
        public const int FSStringoneIndex = 2;
        public const int FSDescIndex = 3;
        public const int FSStatusIndex = 4;
        public const int FSValidFromIndex = 5;
        public const int FSSyncCreatedByIndex = 6;
        public const int FSSyncCreatedTimeIndex = 7;
        public const int FSSyncCreatedTimeHistIndex = 8;
        public const int FSResultIndex = 9;
        public const int FSRemarkIndex = 10;

        // OU
        public const int OUUnitIDIndex = 0;
        public const int OUParentUnitIDIndex = 1;
        public const int OUDescIndex = 2;
        public const int OUCodeIndex = 3;
        public const int OUCCCodeIndex = 4;
        public const int OUCCCompanyCodeIndex = 5;
        public const int OUStatusIndex = 6;
        public const int OUValidFromIndex = 7;
        public const int OUSyncCreatedByIndex = 8;
        public const int OUSyncCreatedTimeIndex = 9;
        public const int OUSyncCreatedTimeHistIndex = 10;
        public const int OUResultIndex = 11;
        public const int OURemarkIndex = 12;

        // CC
        public const int CCCCCodeIndex = 0;
        public const int CCCCCompanyCodeIndex = 1;
        public const int CCDescIndex = 2;
        public const int CCValidFromIndex = 3;
        public const int CCSyncCreatedByIndex = 4;
        public const int CCSyncCreatedTimeIndex = 5;
        public const int CCSyncCreatedTimeHistIndex = 6;
        public const int CCResultIndex = 7;
        public const int CCRemarkIndex = 8;

        // Position
        public const int PosCompanyCodeIndex = 0;
        public const int PosStatusIndex = 1;
        public const int PosPositionIDIndex = 2;
        public const int PosCodeIndex = 3;
        public const int PosTitleSRIndex = 4;
        public const int PosTitleENIndex = 5;
        public const int PosDescSRIndex = 6;
        public const int PosDescENIndex = 7;
        public const int PosValidFromIndex = 8;
        public const int PosSyncCreatedByIndex = 9;
        public const int PosSyncCreatedTimeIndex = 10;
        public const int PosSyncCreatedTimeHistIndex = 11;
        public const int PosResultIndex = 12;
        public const int PosRemarkIndex = 13;

        // Employee
        public const int EmplEmplIDIndex = 0;
        public const int EmplEplIDOldIndex = 1;
        public const int EmplFirstNameIndex = 2;
        public const int EmplLastNameIndex = 3;
        public const int EmplTagIndex = 4;
        public const int EmplWUIndex = 5;
        public const int EmplOUIndex = 6;
        public const int EmplPictureIndex = 7;
        public const int EmplTypeIndex = 8;
        public const int EmplPersonalHolidayCategoryIndex = 9;
        public const int EmplPersonalHolidayDateIndex = 10;
        public const int EmplEmailIndex = 11;
        public const int EmplJMBGIndex = 12;
        public const int EmplUsernameIndex = 13;
        public const int EmplStatusIndex = 14;
        public const int EmplBranchIndex = 15;
        public const int EmplALDateStartIndex = 16;
        public const int EmplALCurrYearIndex = 17;
        public const int EmplALPrevYearIndex = 18;
        public const int EmplALCurrYearLeftIndex = 19;
        public const int EmplALPrevYearLeftIndex = 20;
        public const int EmplLangIndex = 21;
        public const int EmplWorkLocationIDIndex = 22;
        public const int EmplWorkLocationCodeIndex = 23;
        public const int EmplPositionIDIndex = 24;
        public const int EmplAddressIndex = 25;
        public const int EmplDateOfBirthIndex = 26;
        public const int EmplPhone1Index = 27;
        public const int EmplPhone2Index = 28;
        public const int EmplValidFromIndex = 29;
        public const int EmplSyncCreatedByIndex = 30;
        public const int EmplSyncCreatedTimeIndex = 31;
        public const int EmplSyncCreatedTimeHistIndex = 32;
        public const int EmplResultIndex = 33;
        public const int EmplRemarkIndex = 34;

        // Responsibility
        public const int ResEmplIDIndex = 0;
        public const int ResUnitIDIndex = 1;
        public const int ResTypeIndex = 2;
        public const int ResValidFromIndex = 3;
        public const int ResValidToIndex = 4;
        public const int ResSyncCreatedByIndex = 5;
        public const int ResSyncCreatedTimeIndex = 6;
        public const int ResSyncCreatedTimeHistIndex = 7;
        public const int ResResultIndex = 8;
        public const int ResRemarkIndex = 9;

        // Annaul leave recalculation
        public const int ALEmplIDIndex = 0;
        public const int ALYearIndex = 1;
        public const int ALNumOfDaysIndex = 2;
        public const int ALSyncCreatedByIndex = 3;
        public const int ALSyncCreatedTimeIndex = 4;
        public const int ALSyncCreatedTimeHistIndex = 5;
        public const int ALResultIndex = 6;
        public const int ALRemarkIndex = 7;

        private ListViewItemComparer _compFS;
        private ListViewItemComparer _compOU;
        private ListViewItemComparer _compCC;
        private ListViewItemComparer _compPos;
        private ListViewItemComparer _compEmpl;
        private ListViewItemComparer _compRes;
        private ListViewItemComparer _compAL;

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();
        
        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();

        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();

        Object SyncConnection;

        SynchronizationDAO syncDAO;

        public SynchronizationPreview()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SynchronizationPreview).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                dtpFrom.Value = DateTime.Now.Date;
                dtpTo.Value = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer : IComparer
        {
            private ListView _listView;
            private string _type;

            public ListViewItemComparer(ListView lv, string listType)
            {
                _listView = lv;
                _type = listType;
            }

            public ListView ListView
            {
                get { return _listView; }
            }

            public string Type
            {
                get { return _type; }
            }

            private int _sortColumn = 0;

            public int SortColumn
            {
                get { return _sortColumn; }
                set { _sortColumn = value; }
            }

            public int Compare(object a, object b)
            {
                object item1 = ((ListViewItem)a).Tag;
                object item2 = ((ListViewItem)b).Tag;

                if (ListView.Sorting == SortOrder.Descending)
                {
                    object temp = item1;
                    item1 = item2;
                    item2 = temp;
                }
                // Handle non Detail Cases
                return CompareItems(item1, item2);
            }

            public int CompareItems(object item1, object item2)
            {
                // Return value based on sort column and type of list view sorting
                if (Type == Constants.SyncPreviewType.FS.ToString())
                {
                    SyncFinancialStructureTO fs1 = (SyncFinancialStructureTO)item1;
                    SyncFinancialStructureTO fs2 = (SyncFinancialStructureTO)item2;
                    switch (SortColumn)
                    {
                        case SynchronizationPreview.FSCompanyCodeIndex:
                            return fs1.CompanyCode.CompareTo(fs2.CompanyCode);
                        case SynchronizationPreview.FSDescIndex:
                            return fs1.Description.CompareTo(fs2.Description);
                        case SynchronizationPreview.FSRemarkIndex:
                            return fs1.Remark.CompareTo(fs2.Remark);
                        case SynchronizationPreview.FSResultIndex:
                            return fs1.Result.CompareTo(fs2.Result);
                        case SynchronizationPreview.FSStatusIndex:
                            return fs1.Status.CompareTo(fs2.Status);
                        case SynchronizationPreview.FSStringoneIndex:
                            return fs1.UnitStringone.CompareTo(fs2.UnitStringone);
                        case SynchronizationPreview.FSSyncCreatedByIndex:
                            return fs1.CreatedBy.CompareTo(fs2.CreatedBy);
                        case SynchronizationPreview.FSSyncCreatedTimeHistIndex:
                            return fs1.CreatedTimeHist.CompareTo(fs2.CreatedTimeHist);
                        case SynchronizationPreview.FSSyncCreatedTimeIndex:
                            return fs1.CreatedTime.CompareTo(fs2.CreatedTime);
                        case SynchronizationPreview.FSUnitIDIndex:
                            return fs1.UnitID.CompareTo(fs2.UnitID);
                        case SynchronizationPreview.FSValidFromIndex:
                            return fs1.ValidFrom.CompareTo(fs2.ValidFrom);
                        default:
                            return fs1.CreatedTimeHist.CompareTo(fs2.CreatedTimeHist);
                    }
                }
                else if (Type == Constants.SyncPreviewType.OU.ToString())
                {
                    SyncOrganizationalStructureTO ou1 = (SyncOrganizationalStructureTO)item1;
                    SyncOrganizationalStructureTO ou2 = (SyncOrganizationalStructureTO)item2;
                    switch (SortColumn)
                    {
                        case SynchronizationPreview.OUCCCodeIndex:
                            return ou1.CostCenterStringone.CompareTo(ou2.CostCenterStringone);
                        case SynchronizationPreview.OUCCCompanyCodeIndex:
                            return ou1.CompanyCode.CompareTo(ou2.CompanyCode);
                        case SynchronizationPreview.OUCodeIndex:
                            return ou1.Code.CompareTo(ou2.Code);
                        case SynchronizationPreview.OUDescIndex:
                            return ou1.Description.CompareTo(ou2.Description);
                        case SynchronizationPreview.OUParentUnitIDIndex:
                            return ou1.ParentUnitID.CompareTo(ou2.ParentUnitID);
                        case SynchronizationPreview.OURemarkIndex:
                            return ou1.Remark.CompareTo(ou2.Remark);
                        case SynchronizationPreview.OUResultIndex:
                            return ou1.Result.CompareTo(ou2.Result);
                        case SynchronizationPreview.OUStatusIndex:
                            return ou1.Status.CompareTo(ou2.Status);
                        case SynchronizationPreview.OUSyncCreatedByIndex:
                            return ou1.CreatedBy.CompareTo(ou2.CreatedBy);
                        case SynchronizationPreview.OUSyncCreatedTimeHistIndex:
                            return ou1.CreatedTimeHist.CompareTo(ou2.CreatedTimeHist);
                        case SynchronizationPreview.OUSyncCreatedTimeIndex:
                            return ou1.CreatedTime.CompareTo(ou2.CreatedTime);
                        case SynchronizationPreview.OUUnitIDIndex:
                            return ou1.UnitID.CompareTo(ou2.UnitID);
                        case SynchronizationPreview.OUValidFromIndex:
                            return ou1.ValidFrom.CompareTo(ou2.ValidFrom);
                        default:
                            return ou1.CreatedTimeHist.CompareTo(ou2.CreatedTimeHist);
                    }
                }
                else if (Type == Constants.SyncPreviewType.CC.ToString())
                {
                    SyncCostCenterTO cc1 = (SyncCostCenterTO)item1;
                    SyncCostCenterTO cc2 = (SyncCostCenterTO)item2;
                    switch (SortColumn)
                    {
                        case SynchronizationPreview.CCCCCodeIndex:
                            return cc1.Code.CompareTo(cc2.Code);
                        case SynchronizationPreview.CCCCCompanyCodeIndex:
                            return cc1.CompanyCode.CompareTo(cc2.CompanyCode);
                        case SynchronizationPreview.CCDescIndex:
                            return cc1.Desc.CompareTo(cc2.Desc);
                        case SynchronizationPreview.CCRemarkIndex:
                            return cc1.Remark.CompareTo(cc2.Remark);
                        case SynchronizationPreview.CCResultIndex:
                            return cc1.Result.CompareTo(cc2.Result);
                        case SynchronizationPreview.CCSyncCreatedByIndex:
                            return cc1.CreatedBy.CompareTo(cc2.CreatedBy);
                        case SynchronizationPreview.CCSyncCreatedTimeHistIndex:
                            return cc1.CreatedTimeHist.CompareTo(cc2.CreatedTimeHist);
                        case SynchronizationPreview.CCSyncCreatedTimeIndex:
                            return cc1.CreatedTime.CompareTo(cc2.CreatedTime);
                        case SynchronizationPreview.CCValidFromIndex:
                            return cc1.ValidFrom.CompareTo(cc2.ValidFrom);
                        default:
                            return cc1.CreatedTimeHist.CompareTo(cc2.CreatedTimeHist);
                    }
                }
                else if (Type == Constants.SyncPreviewType.POS.ToString())
                {
                    SyncEmployeePositionTO pos1 = (SyncEmployeePositionTO)item1;
                    SyncEmployeePositionTO pos2 = (SyncEmployeePositionTO)item2;
                    switch (SortColumn)
                    {
                        case SynchronizationPreview.PosCodeIndex:
                            return pos1.PositionCode.CompareTo(pos2.PositionCode);
                        case SynchronizationPreview.PosCompanyCodeIndex:
                            return pos1.CompanyCode.CompareTo(pos2.CompanyCode);
                        case SynchronizationPreview.PosDescSRIndex:
                            return pos1.DescSR.CompareTo(pos2.DescSR);
                        case SynchronizationPreview.PosDescENIndex:
                            return pos1.DescEN.CompareTo(pos2.DescEN);
                        case SynchronizationPreview.PosPositionIDIndex:
                            return pos1.PositionID.CompareTo(pos2.PositionID);
                        case SynchronizationPreview.PosRemarkIndex:
                            return pos1.Remark.CompareTo(pos2.Remark);
                        case SynchronizationPreview.PosResultIndex:
                            return pos1.Result.CompareTo(pos2.Result);
                        case SynchronizationPreview.PosStatusIndex:
                            return pos1.Status.CompareTo(pos2.Status);
                        case SynchronizationPreview.PosSyncCreatedByIndex:
                            return pos1.CreatedBy.CompareTo(pos2.CreatedBy);
                        case SynchronizationPreview.PosSyncCreatedTimeHistIndex:
                            return pos1.CreatedTimeHist.CompareTo(pos2.CreatedTimeHist);
                        case SynchronizationPreview.PosSyncCreatedTimeIndex:
                            return pos1.CreatedTime.CompareTo(pos2.CreatedTime);
                        case SynchronizationPreview.PosTitleSRIndex:
                            return pos1.PositionTitleSR.CompareTo(pos2.PositionTitleSR);
                        case SynchronizationPreview.PosTitleENIndex:
                            return pos1.PositionTitleEN.CompareTo(pos2.PositionTitleEN);
                        case SynchronizationPreview.PosValidFromIndex:
                            return pos1.ValidFrom.CompareTo(pos2.ValidFrom);
                        default:
                            return pos1.CreatedTimeHist.CompareTo(pos2.CreatedTimeHist);
                    }
                }
                else if (Type == Constants.SyncPreviewType.EMPL.ToString())
                {
                    SyncEmployeeTO empl1 = (SyncEmployeeTO)item1;
                    SyncEmployeeTO empl2 = (SyncEmployeeTO)item2;
                    switch (SortColumn)
                    {
                        case SynchronizationPreview.EmplAddressIndex:
                            return empl1.Address.CompareTo(empl1.Address);
                        case SynchronizationPreview.EmplALCurrYearIndex:
                            return empl1.AnnualLeaveCurrentYear.CompareTo(empl1.AnnualLeaveCurrentYear);
                        case SynchronizationPreview.EmplALCurrYearLeftIndex:
                            return empl1.AnnualLeaveCurrentYearLeft.CompareTo(empl1.AnnualLeaveCurrentYearLeft);
                        case SynchronizationPreview.EmplALDateStartIndex:
                            return empl1.AnnualLeaveStartDate.CompareTo(empl1.AnnualLeaveStartDate);
                        case SynchronizationPreview.EmplALPrevYearIndex:
                            return empl1.AnnualLeavePreviousYear.CompareTo(empl1.AnnualLeavePreviousYear);
                        case SynchronizationPreview.EmplALPrevYearLeftIndex:
                            return empl1.AnnualLeavePreviousYearLeft.CompareTo(empl1.AnnualLeavePreviousYearLeft);
                        case SynchronizationPreview.EmplBranchIndex:
                            return empl1.EmployeeBranch.CompareTo(empl1.EmployeeBranch);
                        case SynchronizationPreview.EmplDateOfBirthIndex:
                            return empl1.DateOfBirth.CompareTo(empl1.DateOfBirth);
                        case SynchronizationPreview.EmplEmailIndex:
                            return empl1.EmailAddress.CompareTo(empl1.EmailAddress);
                        case SynchronizationPreview.EmplEmplIDIndex:
                            return empl1.EmployeeID.CompareTo(empl1.EmployeeID);
                        case SynchronizationPreview.EmplEplIDOldIndex:
                            return empl1.EmployeeIDOld.CompareTo(empl1.EmployeeIDOld);
                        case SynchronizationPreview.EmplFirstNameIndex:
                            return empl1.FirstName.CompareTo(empl1.FirstName);
                        case SynchronizationPreview.EmplJMBGIndex:
                            return empl1.JMBG.CompareTo(empl1.JMBG);
                        case SynchronizationPreview.EmplLangIndex:
                            return empl1.Language.CompareTo(empl1.Language);
                        case SynchronizationPreview.EmplLastNameIndex:
                            return empl1.LastName.CompareTo(empl1.LastName);
                        case SynchronizationPreview.EmplOUIndex:
                            return empl1.OrganizationalUnitID.CompareTo(empl1.OrganizationalUnitID);
                        case SynchronizationPreview.EmplPersonalHolidayCategoryIndex:
                            return empl1.PersonalHolidayCategory.CompareTo(empl1.PersonalHolidayCategory);
                        case SynchronizationPreview.EmplPersonalHolidayDateIndex:
                            return empl1.PersonalHolidayDate.CompareTo(empl1.PersonalHolidayDate);
                        case SynchronizationPreview.EmplPhone1Index:
                            return empl1.PhoneNumber1.CompareTo(empl1.PhoneNumber1);
                        case SynchronizationPreview.EmplPhone2Index:
                            return empl1.PhoneNumber2.CompareTo(empl1.PhoneNumber2);
                        case SynchronizationPreview.EmplPictureIndex:
                            string pict1 = "N/A";
                            string pict2 = "N/A";
                            if (empl1.Picture != null)
                                pict1 = "defined";
                            if (empl2.Picture != null)
                                pict2 = "defined";
                            return pict1.CompareTo(pict2);
                        case SynchronizationPreview.EmplPositionIDIndex:
                            return empl1.PositionID.CompareTo(empl1.PositionID);
                        case SynchronizationPreview.EmplRemarkIndex:
                            return empl1.Remark.CompareTo(empl1.Remark);
                        case SynchronizationPreview.EmplResultIndex:
                            return empl1.Result.CompareTo(empl1.Result);
                        case SynchronizationPreview.EmplStatusIndex:
                            return empl1.Status.CompareTo(empl1.Status);
                        case SynchronizationPreview.EmplSyncCreatedByIndex:
                            return empl1.CreatedBy.CompareTo(empl1.CreatedBy);
                        case SynchronizationPreview.EmplSyncCreatedTimeHistIndex:
                            return empl1.CreatedTimeHist.CompareTo(empl1.CreatedTimeHist);
                        case SynchronizationPreview.EmplSyncCreatedTimeIndex:
                            return empl1.CreatedTime.CompareTo(empl1.CreatedTime);
                        case SynchronizationPreview.EmplTagIndex:
                            return empl1.TagID.CompareTo(empl1.TagID);
                        case SynchronizationPreview.EmplTypeIndex:
                            return empl1.EmployeeTypeID.CompareTo(empl1.EmployeeTypeID);
                        case SynchronizationPreview.EmplUsernameIndex:
                            return empl1.Username.CompareTo(empl1.Username);
                        case SynchronizationPreview.EmplValidFromIndex:
                            return empl1.ValidFrom.CompareTo(empl1.ValidFrom);
                        case SynchronizationPreview.EmplWorkLocationCodeIndex:
                            return empl1.WorkLocationCode.CompareTo(empl1.WorkLocationCode);
                        case SynchronizationPreview.EmplWorkLocationIDIndex:
                            return empl1.WorkLocationID.CompareTo(empl1.WorkLocationID);
                        case SynchronizationPreview.EmplWUIndex:
                            return empl1.FsUnitID.CompareTo(empl1.FsUnitID);

                        default:
                            return empl1.CreatedTimeHist.CompareTo(empl2.CreatedTimeHist);
                    }
                }
                else if (Type == Constants.SyncPreviewType.RES.ToString())
                {
                    SyncResponsibilityTO res1 = (SyncResponsibilityTO)item1;
                    SyncResponsibilityTO res2 = (SyncResponsibilityTO)item2;
                    switch (SortColumn)
                    {
                        case SynchronizationPreview.ResEmplIDIndex:
                            return res1.ResponsiblePersonID.CompareTo(res2.ResponsiblePersonID);
                        case SynchronizationPreview.ResRemarkIndex:
                            return res1.Remark.CompareTo(res2.Remark);
                        case SynchronizationPreview.ResResultIndex:
                            return res1.Result.CompareTo(res2.Result);
                        case SynchronizationPreview.ResSyncCreatedByIndex:
                            return res1.CreatedBy.CompareTo(res2.CreatedBy);
                        case SynchronizationPreview.ResSyncCreatedTimeHistIndex:
                            return res1.CreatedTimeHist.CompareTo(res2.CreatedTimeHist);
                        case SynchronizationPreview.ResSyncCreatedTimeIndex:
                            return res1.CreatedTime.CompareTo(res2.CreatedTime);
                        case SynchronizationPreview.ResTypeIndex:
                            return res1.StructureType.CompareTo(res2.StructureType);
                        case SynchronizationPreview.ResUnitIDIndex:
                            return res1.UnitID.CompareTo(res2.UnitID);
                        case SynchronizationPreview.ResValidFromIndex:
                            return res1.ValidFrom.CompareTo(res2.ValidFrom);
                        case SynchronizationPreview.ResValidToIndex:
                            return res1.ValidTo.CompareTo(res2.ValidTo);
                        default:
                            return res1.CreatedTimeHist.CompareTo(res2.CreatedTimeHist);
                    }
                }
                else if (Type == Constants.SyncPreviewType.AL.ToString())
                {
                    SyncAnnualLeaveRecalcTO al1 = (SyncAnnualLeaveRecalcTO)item1;
                    SyncAnnualLeaveRecalcTO al2 = (SyncAnnualLeaveRecalcTO)item2;
                    switch (SortColumn)
                    {
                        case SynchronizationPreview.ALEmplIDIndex:
                            return al1.EmployeeID.CompareTo(al2.EmployeeID);
                        case SynchronizationPreview.ALNumOfDaysIndex:
                            return al1.NumOfDays.CompareTo(al2.NumOfDays);
                        case SynchronizationPreview.ALRemarkIndex:
                            return al1.Remark.CompareTo(al2.Remark);
                        case SynchronizationPreview.ALResultIndex:
                            return al1.Result.CompareTo(al2.Result);
                        case SynchronizationPreview.ALSyncCreatedByIndex:
                            return al1.CreatedBy.CompareTo(al2.CreatedBy);
                        case SynchronizationPreview.ALSyncCreatedTimeHistIndex:
                            return al1.CreatedTimeHist.CompareTo(al2.CreatedTimeHist);
                        case SynchronizationPreview.ALSyncCreatedTimeIndex:
                            return al1.CreatedTime.CompareTo(al2.CreatedTime);
                        case SynchronizationPreview.ALYearIndex:
                            return al1.Year.CompareTo(al2.Year);
                        default:
                            return al1.CreatedTimeHist.CompareTo(al2.CreatedTimeHist);
                    }
                }
                else
                    return 0;
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("SynchronizationPreview", culture);

                // tab text
                tabPositions.Text = rm.GetString("tabPositions", culture);
                tabEmployees.Text = rm.GetString("tabEmployees", culture);
                tabResponsibility.Text = rm.GetString("tabResponsibility", culture);
                tabALRecalc.Text = rm.GetString("tabALRecalc", culture);
                tabALApproval.Text = rm.GetString("tabALRecalcApproval", culture);

                //label's text
                this.lblALEmployee.Text = rm.GetString("lblEmployee", culture);
                this.lblALAppEmployee.Text = rm.GetString("lblEmployee", culture);
                this.lblCCCode.Text = rm.GetString("lblCC", culture);
                this.lblEmplEmployee.Text = rm.GetString("lblEmployee", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblOU.Text = rm.GetString("lblOrgUnit", culture);
                this.lblPosition.Text = rm.GetString("lblPosition", culture);
                this.lblResEmployee.Text = rm.GetString("lblEmployee", culture);
                this.lblResult.Text = rm.GetString("lblResult", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblWU.Text = rm.GetString("lblWU", culture);
                
                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.btnSearchALRecalc.Text = rm.GetString("btnSearch", culture);
                this.btnApprove.Text = rm.GetString("btnApprove", culture);
                this.btnNotApprove.Text = rm.GetString("btnNotApprove", culture);
                
                //group box text
                this.gbALUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                this.gbALAppUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                this.gbCriteria.Text = rm.GetString("gbSearchCriteria", culture);
                this.gbEmplUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                this.gbResUnitFilter.Text = rm.GetString("gbUnitFilter", culture);                
                                
                // list view
                lvWU.BeginUpdate();
                lvWU.Columns.Add(rm.GetString("hdrUnitID", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrCompanyCode", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrStringone", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrDesc", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrStatus", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrValidFrom", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrSyncCreatedBy", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrSyncCreatedTime", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrSyncCreatedTimeHist", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrResult", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrRemark", culture), (lvWU.Width - 22) / 10, HorizontalAlignment.Left);
                lvWU.EndUpdate();

                lvOU.BeginUpdate();
                lvOU.Columns.Add(rm.GetString("hdrUnitID", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrParentUnitID", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrDesc", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrCode", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrCCCode", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrCCCompanyCode", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrStatus", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrValidFrom", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrSyncCreatedBy", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrSyncCreatedTime", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrSyncCreatedTimeHist", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrResult", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrRemark", culture), (lvOU.Width - 22) / 10, HorizontalAlignment.Left);
                lvOU.EndUpdate();

                lvCC.BeginUpdate();
                lvCC.Columns.Add(rm.GetString("hdrCCCode", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.Columns.Add(rm.GetString("hdrCCCompanyCode", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.Columns.Add(rm.GetString("hdrDesc", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.Columns.Add(rm.GetString("hdrValidFrom", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.Columns.Add(rm.GetString("hdrSyncCreatedBy", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.Columns.Add(rm.GetString("hdrSyncCreatedTime", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.Columns.Add(rm.GetString("hdrSyncCreatedTimeHist", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.Columns.Add(rm.GetString("hdrResult", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.Columns.Add(rm.GetString("hdrRemark", culture), (lvCC.Width - 22) / 9, HorizontalAlignment.Left);
                lvCC.EndUpdate();
                
                lvPosition.BeginUpdate();
                lvPosition.Columns.Add(rm.GetString("hdrCompanyCode", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrStatus", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrPositionID", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrCode", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrTitle", culture) + " SR", (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrTitle", culture) + " EN", (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrDesc", culture) + " SR", (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrDesc", culture) + " EN", (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrValidFrom", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrSyncCreatedBy", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrSyncCreatedTime", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrSyncCreatedTimeHist", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrResult", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.Columns.Add(rm.GetString("hdrRemark", culture), (lvPosition.Width - 22) / 10, HorizontalAlignment.Left);
                lvPosition.EndUpdate();
                
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrEmplID", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrEplIDOld", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrTag", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrWU", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrOU", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrPicture", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrType", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrPersonalHolidayCategory", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrPersonalHolidayDate", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrEmail", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrJMBG", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrUsername", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrStatus", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrBranch", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrALDateStart", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrALCurrYear", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrALPrevYear", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrALCurrYearLeft", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrALPrevYearLeft", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLang", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrWorkLocationID", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrWorkLocationCode", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrPositionID", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrAddress", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrDateOfBirth", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrPhone", culture) + " 1", (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrPhone", culture) + " 2", (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrValidFrom", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrSyncCreatedBy", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrSyncCreatedTime", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrSyncCreatedTimeHist", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrResult", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrRemark", culture), (lvEmployees.Width - 22) / 10, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvResponsibilities.BeginUpdate();
                lvResponsibilities.Columns.Add(rm.GetString("hdrEmplID", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrUnitID", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrType", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrValidFrom", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrValidTo", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrSyncCreatedBy", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrSyncCreatedTime", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrSyncCreatedTimeHist", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrResult", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrRemark", culture), (lvResponsibilities.Width - 22) / 10, HorizontalAlignment.Left);
                lvResponsibilities.EndUpdate();

                lvALRecalculation.BeginUpdate();
                lvALRecalculation.Columns.Add(rm.GetString("hdrEmplID", culture), (lvALRecalculation.Width - 22) / 8, HorizontalAlignment.Left);
                lvALRecalculation.Columns.Add(rm.GetString("hdrYear", culture), (lvALRecalculation.Width - 22) / 8, HorizontalAlignment.Left);
                lvALRecalculation.Columns.Add(rm.GetString("hdrNumOfDays", culture), (lvALRecalculation.Width - 22) / 8, HorizontalAlignment.Left);
                lvALRecalculation.Columns.Add(rm.GetString("hdrSyncCreatedBy", culture), (lvALRecalculation.Width - 22) / 8, HorizontalAlignment.Left);
                lvALRecalculation.Columns.Add(rm.GetString("hdrSyncCreatedTime", culture), (lvALRecalculation.Width - 22) / 8, HorizontalAlignment.Left);
                lvALRecalculation.Columns.Add(rm.GetString("hdrSyncCreatedTimeHist", culture), (lvALRecalculation.Width - 22) / 8, HorizontalAlignment.Left);
                lvALRecalculation.Columns.Add(rm.GetString("hdrResult", culture), (lvALRecalculation.Width - 22) / 8, HorizontalAlignment.Left);
                lvALRecalculation.Columns.Add(rm.GetString("hdrRemark", culture), (lvALRecalculation.Width - 22) / 8, HorizontalAlignment.Left);
                lvALRecalculation.EndUpdate();

                lvALRecalc.BeginUpdate();
                lvALRecalc.Columns.Add(rm.GetString("hdrEmplID", culture), (lvALRecalc.Width - 22) / 6, HorizontalAlignment.Left);
                lvALRecalc.Columns.Add(rm.GetString("hdrEmployee", culture), (lvALRecalc.Width - 22) / 6, HorizontalAlignment.Left);
                lvALRecalc.Columns.Add(rm.GetString("hdrYear", culture), (lvALRecalc.Width - 22) / 6, HorizontalAlignment.Left);
                lvALRecalc.Columns.Add(rm.GetString("hdrNumOfDays", culture), (lvALRecalc.Width - 22) / 6, HorizontalAlignment.Left);
                lvALRecalc.Columns.Add(rm.GetString("hdrSyncCreatedBy", culture), (lvALRecalc.Width - 22) / 6, HorizontalAlignment.Left);
                lvALRecalc.Columns.Add(rm.GetString("hdrSyncCreatedTime", culture), (lvALRecalc.Width - 22) / 6, HorizontalAlignment.Left);
                lvALRecalc.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SynchronizationPreview.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void SynchronizationPreview_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer objects
                _compFS = new ListViewItemComparer(lvWU, Constants.SyncPreviewType.FS.ToString());
                lvWU.ListViewItemSorter = _compFS;
                lvWU.Sorting = SortOrder.Ascending;

                _compOU = new ListViewItemComparer(lvOU, Constants.SyncPreviewType.OU.ToString());
                lvOU.ListViewItemSorter = _compOU;
                lvOU.Sorting = SortOrder.Ascending;

                _compCC = new ListViewItemComparer(lvCC, Constants.SyncPreviewType.CC.ToString());
                lvCC.ListViewItemSorter = _compCC;
                lvCC.Sorting = SortOrder.Ascending;

                _compPos = new ListViewItemComparer(lvPosition, Constants.SyncPreviewType.POS.ToString());
                lvPosition.ListViewItemSorter = _compPos;
                lvPosition.Sorting = SortOrder.Ascending;

                _compEmpl = new ListViewItemComparer(lvEmployees, Constants.SyncPreviewType.EMPL.ToString());
                lvEmployees.ListViewItemSorter = _compEmpl;
                lvEmployees.Sorting = SortOrder.Ascending;

                _compRes = new ListViewItemComparer(lvResponsibilities, Constants.SyncPreviewType.RES.ToString());
                lvResponsibilities.ListViewItemSorter = _compRes;
                lvResponsibilities.Sorting = SortOrder.Ascending;

                _compAL = new ListViewItemComparer(lvALRecalculation, Constants.SyncPreviewType.AL.ToString());
                lvALRecalculation.ListViewItemSorter = _compAL;
                lvALRecalculation.Sorting = SortOrder.Ascending;

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                    wuList.Add(wUnit.WorkingUnitID);
                }

                if (wuString.Length > 0)
                    wuString = wuString.Substring(0, wuString.Length - 1);

                foreach (int id in oUnits.Keys)
                {
                    ouString += id.ToString().Trim() + ",";
                    ouList.Add(id);
                }

                if (ouString.Length > 0)
                    ouString = ouString.Substring(0, ouString.Length - 1);

                wuDict = new WorkingUnit().getWUDictionary();

                emplDict = new Employee().SearchDictionary();

                populateResult();

                populateWU(cbWU);
                populateWU(cbEmplWU);
                populateWU(cbResWU);
                populateWU(cbALWU);
                populateWU(cbALAppWU);
                populateOU(cbOU);
                populateOU(cbEmplOU);
                populateOU(cbResOU);
                populateOU(cbALOU);
                populateOU(cbALAppOU);

                rbEmplFS.Checked = rbResFS.Checked = rbALFS.Checked = rbALAppFS.Checked = true;

                populateEmployees(rbEmplFS, rbEmplOU, cbEmplWU, cbEmplOU, cbEmplEmployee);
                populateEmployees(rbResFS, rbResOU, cbResWU, cbResOU, cbResEmployee);
                populateEmployees(rbALFS, rbALOU, cbALWU, cbALOU, cbALEmployee);
                populateEmployees(rbALAppFS, rbALAppOU, cbALAppWU, cbALAppOU, cbALAppEmployee);

                populatePositions();
                populateCC();

                if (!ConnectToSyncDB())                
                    btnSearchALRecalc.Enabled = btnApprove.Enabled = btnNotApprove.Enabled = false;                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SynchronizationPreview.SynchronizationPreview_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateResult()
        {
            try
            {
                List<string> resList = new List<string>();                
                resList.Add(rm.GetString("all", culture));                
                resList.Add(rm.GetString("fail", culture));                
                resList.Add(rm.GetString("succ", culture));

                cbResult.DataSource = resList;                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SynchronizationPreview.populateResult(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populatePositions()
        {
            try
            {
                List<EmployeePositionTO> posArray = new EmployeePosition().SearchEmployeePositions();

                EmployeePositionTO posTO = new EmployeePositionTO();
                posTO.PositionCode = rm.GetString("all", culture);
                posArray.Insert(0, posTO);

                cbPosition.DataSource = posArray;
                cbPosition.DisplayMember = "PositionCode";
                cbPosition.ValueMember = "PositionID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SynchronizationPreview.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateWU(ComboBox cb)
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cb.DataSource = wuArray;
                cb.DisplayMember = "Name";
                cb.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SynchronizationPreview.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateCC()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    // skip company
                    if (wuTO.WorkingUnitID == wuTO.ParentWorkingUID)
                        continue;

                    // check if it is cost center
                    if (wuDict.ContainsKey(wuTO.ParentWorkingUID) && wuDict[wuTO.ParentWorkingUID].WorkingUnitID == wuDict[wuTO.ParentWorkingUID].ParentWorkingUID)
                        continue;

                    if (wuDict.ContainsKey(wuDict[wuTO.ParentWorkingUID].ParentWorkingUID)
                        && wuDict[wuDict[wuTO.ParentWorkingUID].ParentWorkingUID].WorkingUnitID == wuDict[wuDict[wuTO.ParentWorkingUID].ParentWorkingUID].ParentWorkingUID)
                        wuArray.Add(wuTO);
                    else
                        continue;
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbCCCode.DataSource = wuArray;
                cbCCCode.DisplayMember = "Name";
                cbCCCode.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SynchronizationPreview.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOU(ComboBox cb)
        {
            try
            {
                List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

                foreach (int id in oUnits.Keys)
                {
                    ouArray.Add(oUnits[id]);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cb.DataSource = ouArray;
                cb.DisplayMember = "Name";
                cb.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SynchronizationPreview.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployees(RadioButton rbWU, RadioButton rbOU, ComboBox cbW, ComboBox cbO, ComboBox cbEmpl)
        {
            try
            {                
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employeeList = new List<EmployeeTO>();

                bool isWU = rbWU.Checked;

                int ID = -1;
                if (isWU && cbW.SelectedIndex > 0)
                    ID = (int)cbW.SelectedValue;
                else if (!isWU && cbO.SelectedIndex > 0)
                    ID = (int)cbO.SelectedValue;

                DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime from = currMonth.AddMonths(-1);
                DateTime to = currMonth.AddMonths(2).AddDays(-1);
                
                if (isWU)
                {
                    string wunits = wuString;

                    if (ID != -1)
                        wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);
                    
                    // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                    employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                }
                else
                {
                    string ounits = ouString;

                    if (ID != -1)
                        ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);
                    
                    employeeList = new Employee().SearchByOU(ounits, -1, null, from.Date, to.Date);
                }                

                EmployeeTO emplAll = new EmployeeTO();
                emplAll.LastName = rm.GetString("all", culture);
                employeeList.Insert(0, emplAll);

                cbEmpl.DataSource = employeeList;
                cbEmpl.DisplayMember = "FirstAndLastName";
                cbEmpl.ValueMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SynchronizationPreview.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbEmplFS_CheckedChanged(object sender, EventArgs e)
        {
            rbCheckedChanged(rbEmplFS, rbEmplOU, cbEmplWU, cbEmplOU, cbEmplEmployee, btnEmplWUTree, btnEmplOUTree);
        }

        private void rbEmplOU_CheckedChanged(object sender, EventArgs e)
        {
            rbCheckedChanged(rbEmplFS, rbEmplOU, cbEmplWU, cbEmplOU, cbEmplEmployee, btnEmplWUTree, btnEmplOUTree);
        }

        private void cbEmplWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(rbEmplFS, rbEmplOU, cbEmplWU, cbEmplOU, cbEmplEmployee);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbEmplWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbEmplOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(rbEmplFS, rbEmplOU, cbEmplWU, cbEmplOU, cbEmplEmployee);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbEmplOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEmplWUTree_Click(object sender, EventArgs e)
        {
            showWUTree(cbEmplWU);
        }

        private void btnEmplOUTree_Click(object sender, EventArgs e)
        {
            showOUTree(cbEmplOU);
        }

        private void rbResFS_CheckedChanged(object sender, EventArgs e)
        {
            rbCheckedChanged(rbResFS, rbResOU, cbResWU, cbResOU, cbResEmployee, btnResWUTree, btnResOUTree);
        }

        private void rbResOU_CheckedChanged(object sender, EventArgs e)
        {
            rbCheckedChanged(rbResFS, rbResOU, cbResWU, cbResOU, cbResEmployee, btnResWUTree, btnResOUTree);
        }

        private void cbResWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(rbResFS, rbResOU, cbResWU, cbResOU, cbResEmployee);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbResWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbResOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(rbResFS, rbResOU, cbResWU, cbResOU, cbResEmployee);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbResOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnResWUTree_Click(object sender, EventArgs e)
        {
            showWUTree(cbResWU);
        }

        private void btnResOUTree_Click(object sender, EventArgs e)
        {
            showOUTree(cbResOU);
        }

        private void rbALFS_CheckedChanged(object sender, EventArgs e)
        {
            rbCheckedChanged(rbALFS, rbALOU, cbALWU, cbALOU, cbALEmployee, btnALWUTree, btnALOUTree);
        }

        private void rbALOU_CheckedChanged(object sender, EventArgs e)
        {
            rbCheckedChanged(rbALFS, rbALOU, cbALWU, cbALOU, cbALEmployee, btnALWUTree, btnALOUTree);
        }

        private void cbALWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(rbALFS, rbALOU, cbALWU, cbALOU, cbALEmployee);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbALWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbALOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(rbALFS, rbALOU, cbALWU, cbALOU, cbALEmployee);
            }            
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbALOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnALWUTree_Click(object sender, EventArgs e)
        {
            showWUTree(cbALWU);
        }

        private void btnALOUTree_Click(object sender, EventArgs e)
        {
            showOUTree(cbALOU);
        }

        private void rbCheckedChanged(RadioButton rbWU, RadioButton rbOU, ComboBox cbW, ComboBox cbO, ComboBox cbEmpl, Button btnWU, Button btnOU)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cbW.Enabled = btnWU.Enabled = rbWU.Checked;

                cbO.Enabled = btnOU.Enabled = !rbWU.Checked;

                populateEmployees(rbWU, rbOU, cbW, cbO, cbEmpl);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.rbCheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void showWUTree(ComboBox cb)
        {            
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cb.SelectedIndex = cb.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.showWUTree(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void showOUTree(ComboBox cb)
        {            
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits(ouString);
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cb.SelectedIndex = cb.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.showOUTree(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            showWUTree(cbWU);
        }

        private void btnOUTree_Click(object sender, EventArgs e)
        {
            showOUTree(cbOU);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                populateFSSync();
                populateOUSync();
                populateCCSync();
                populatePositionsSync();
                populateEmployeesSync();
                populateResponsibilitySync();
                populateALRecalculationSync();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateFSSync()
        {
            try
            {
                SyncFinancialStructureHist fsHist = new SyncFinancialStructureHist();
                List<SyncFinancialStructureTO> fsList = fsHist.Search(dtpFrom.Value, dtpTo.Value, (int)cbWU.SelectedValue, cbResult.SelectedIndex - 1);

                lvWU.BeginUpdate();
                lvWU.Items.Clear();

                foreach (SyncFinancialStructureTO fsTO in fsList)
                {
                    ListViewItem item = new ListViewItem();
                    if (fsTO.UnitID == -1)
                        item.Text = "";
                    else
                        item.Text = fsTO.UnitID.ToString().Trim();
                    item.SubItems.Add(fsTO.CompanyCode.Trim());
                    item.SubItems.Add(fsTO.UnitStringone.Trim());
                    item.SubItems.Add(fsTO.Description.Trim());
                    item.SubItems.Add(fsTO.Status.Trim());
                    if (fsTO.ValidFrom.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(fsTO.ValidFrom.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    item.SubItems.Add(fsTO.CreatedBy.Trim());
                    if (fsTO.CreatedTime.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(fsTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (fsTO.CreatedTimeHist.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(fsTO.CreatedTimeHist.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (fsTO.Result == Constants.yesInt)
                        item.SubItems.Add(rm.GetString("succ", culture));
                    else
                        item.SubItems.Add(rm.GetString("fail", culture));
                    item.SubItems.Add(fsTO.Remark.Trim());
                    item.ToolTipText = fsTO.Remark;
                    item.Tag = fsTO;
                    
                    lvWU.Items.Add(item);
                }

                lvWU.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.populateFSSync(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOUSync()
        {
            try
            {                
                SyncOrganizationalStructureHist ouHist = new SyncOrganizationalStructureHist();
                List<SyncOrganizationalStructureTO> ouList = ouHist.Search(dtpFrom.Value, dtpTo.Value, (int)cbOU.SelectedValue, cbResult.SelectedIndex - 1);

                lvOU.BeginUpdate();
                lvOU.Items.Clear();

                foreach (SyncOrganizationalStructureTO ouTO in ouList)
                {
                    ListViewItem item = new ListViewItem();
                    if (ouTO.UnitID == -1)
                        item.Text = "";
                    else
                        item.Text = ouTO.UnitID.ToString().Trim();
                    if (ouTO.ParentUnitID == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(ouTO.ParentUnitID.ToString().Trim());
                    item.SubItems.Add(ouTO.Description.Trim());
                    item.SubItems.Add(ouTO.Code.Trim());
                    item.SubItems.Add(ouTO.CostCenterStringone.Trim());
                    item.SubItems.Add(ouTO.CompanyCode.Trim());
                    item.SubItems.Add(ouTO.Status.Trim());
                    if (ouTO.ValidFrom.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(ouTO.ValidFrom.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    item.SubItems.Add(ouTO.CreatedBy.Trim());
                    if (ouTO.CreatedTime.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(ouTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (ouTO.CreatedTimeHist.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(ouTO.CreatedTimeHist.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (ouTO.Result == Constants.yesInt)
                        item.SubItems.Add(rm.GetString("succ", culture));
                    else
                        item.SubItems.Add(rm.GetString("fail", culture));
                    item.SubItems.Add(ouTO.Remark.Trim());
                    item.ToolTipText = ouTO.Remark;
                    item.Tag = ouTO;

                    lvOU.Items.Add(item);
                }

                lvOU.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.populateOUSync(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateCCSync()
        {
            try
            {
                SyncCostCenterHist ccHist = new SyncCostCenterHist();
                string code = "";
                string companyCode = "";
                int ccID = (int)cbCCCode.SelectedValue;

                if (ccID != -1 && wuDict.ContainsKey(ccID))
                {
                    code = wuDict[ccID].Name.Replace(".", "");
                    int company = Common.Misc.getRootWorkingUnit(ccID, wuDict);

                    if (wuDict.ContainsKey(company))
                        companyCode = wuDict[company].Name;
                }

                List<SyncCostCenterTO> ccList = ccHist.Search(dtpFrom.Value, dtpTo.Value, code, companyCode, cbResult.SelectedIndex - 1);

                lvCC.BeginUpdate();
                lvCC.Items.Clear();

                foreach (SyncCostCenterTO ccTO in ccList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = ccTO.Code.Trim();
                    item.SubItems.Add(ccTO.CompanyCode.Trim());                    
                    item.SubItems.Add(ccTO.Desc.Trim());                    
                    if (ccTO.ValidFrom.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(ccTO.ValidFrom.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    item.SubItems.Add(ccTO.CreatedBy.Trim());
                    if (ccTO.CreatedTime.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(ccTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (ccTO.CreatedTimeHist.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(ccTO.CreatedTimeHist.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (ccTO.Result == Constants.yesInt)
                        item.SubItems.Add(rm.GetString("succ", culture));
                    else
                        item.SubItems.Add(rm.GetString("fail", culture));
                    item.SubItems.Add(ccTO.Remark.Trim());
                    item.ToolTipText = ccTO.Remark;
                    item.Tag = ccTO;

                    lvCC.Items.Add(item);
                }

                lvCC.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.populateccSync(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populatePositionsSync()
        {
            try
            {
                SyncEmployeePositionHist posHist = new SyncEmployeePositionHist();
                List<SyncEmployeePositionTO> posList = posHist.Search(dtpFrom.Value, dtpTo.Value, (int)cbPosition.SelectedValue, cbResult.SelectedIndex - 1);

                lvPosition.BeginUpdate();
                lvPosition.Items.Clear();

                foreach (SyncEmployeePositionTO posTO in posList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = posTO.CompanyCode.Trim();
                    item.SubItems.Add(posTO.Status.Trim());
                    if (posTO.PositionID == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(posTO.PositionID.ToString().Trim());
                    item.SubItems.Add(posTO.PositionCode.Trim());
                    item.SubItems.Add(posTO.PositionTitleSR.Trim());
                    item.SubItems.Add(posTO.PositionTitleEN.Trim());
                    item.SubItems.Add(posTO.DescSR.Trim());
                    item.SubItems.Add(posTO.DescEN.Trim());                    
                    if (posTO.ValidFrom.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(posTO.ValidFrom.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    item.SubItems.Add(posTO.CreatedBy.Trim());
                    if (posTO.CreatedTime.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(posTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (posTO.CreatedTimeHist.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(posTO.CreatedTimeHist.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (posTO.Result == Constants.yesInt)
                        item.SubItems.Add(rm.GetString("succ", culture));
                    else
                        item.SubItems.Add(rm.GetString("fail", culture));
                    item.SubItems.Add(posTO.Remark.Trim());
                    item.ToolTipText = posTO.Remark;
                    item.Tag = posTO;

                    lvPosition.Items.Add(item);
                }

                lvPosition.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.populatePositionsSync(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateResponsibilitySync()
        {
            try
            {
                SyncResponsibilityHist resHist = new SyncResponsibilityHist();
                List<SyncResponsibilityTO> resList = resHist.Search(dtpFrom.Value, dtpTo.Value, (int)cbResEmployee.SelectedValue, cbResult.SelectedIndex - 1);

                lvResponsibilities.BeginUpdate();
                lvResponsibilities.Items.Clear();

                foreach (SyncResponsibilityTO resTO in resList)
                {
                    ListViewItem item = new ListViewItem();
                    if (resTO.ResponsiblePersonID == -1)
                        item.Text = "";
                    else
                        item.Text = resTO.ResponsiblePersonID.ToString().Trim();
                    if (resTO.UnitID == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(resTO.UnitID.ToString().Trim());
                    item.SubItems.Add(resTO.StructureType.Trim());
                    if (resTO.ValidFrom.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(resTO.ValidFrom.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (resTO.ValidTo.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(resTO.ValidTo.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    item.SubItems.Add(resTO.CreatedBy.Trim());
                    if (resTO.CreatedTime.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(resTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (resTO.CreatedTimeHist.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(resTO.CreatedTimeHist.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (resTO.Result == Constants.yesInt)
                        item.SubItems.Add(rm.GetString("succ", culture));
                    else
                        item.SubItems.Add(rm.GetString("fail", culture));
                    item.SubItems.Add(resTO.Remark.Trim());
                    item.ToolTipText = resTO.Remark;
                    item.Tag = resTO;

                    lvResponsibilities.Items.Add(item);
                }

                lvResponsibilities.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.populateResponsibilitySync(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateALRecalculationSync()
        {
            try
            {
                SyncAnnualLeaveRecalcHist alHist = new SyncAnnualLeaveRecalcHist();
                List<SyncAnnualLeaveRecalcTO> alList = alHist.Search(dtpFrom.Value, dtpTo.Value, (int)cbALEmployee.SelectedValue, cbResult.SelectedIndex - 1);

                lvALRecalculation.BeginUpdate();                
                lvALRecalculation.Items.Clear();

                foreach (SyncAnnualLeaveRecalcTO alTO in alList)
                {
                    ListViewItem item = new ListViewItem();
                    if (alTO.EmployeeID == -1)
                        item.Text = "";
                    else
                        item.Text = alTO.EmployeeID.ToString().Trim();
                    if (alTO.Year.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(alTO.Year.ToString("yyyy"));
                    if (alTO.NumOfDays == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(alTO.NumOfDays.ToString().Trim());
                    item.SubItems.Add(alTO.CreatedBy.Trim());
                    if (alTO.CreatedTime.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(alTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (alTO.CreatedTimeHist.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(alTO.CreatedTimeHist.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (alTO.Result == Constants.yesInt)
                        item.SubItems.Add(rm.GetString("succ", culture));
                    else
                        item.SubItems.Add(rm.GetString("fail", culture));
                    item.SubItems.Add(alTO.Remark.Trim());
                    item.ToolTipText = alTO.Remark;
                    item.Tag = alTO;

                    lvALRecalculation.Items.Add(item);
                }

                lvALRecalculation.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.populateALRecalculationSync(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployeesSync()
        {
            try
            {
                SyncEmployeeHist emplHist = new SyncEmployeeHist();
                List<SyncEmployeeTO> emplList = emplHist.Search(dtpFrom.Value, dtpTo.Value, (int)cbEmplEmployee.SelectedValue, cbResult.SelectedIndex - 1);

                lvEmployees.BeginUpdate();                
                lvEmployees.Items.Clear();                
                
                foreach (SyncEmployeeTO emplTO in emplList)
                {
                    ListViewItem item = new ListViewItem();
                    if (emplTO.EmployeeID == -1)
                        item.Text = "";
                    else
                        item.Text = emplTO.EmployeeID.ToString().Trim();
                    if (emplTO.EmployeeIDOld == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.EmployeeIDOld.ToString().Trim());
                    item.SubItems.Add(emplTO.FirstName.Trim());
                    item.SubItems.Add(emplTO.LastName.Trim());
                    if (emplTO.TagID == 0)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.TagID.ToString().Trim());
                    if (emplTO.FsUnitID == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.FsUnitID.ToString().Trim());
                    if (emplTO.OrganizationalUnitID == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.OrganizationalUnitID.ToString().Trim());
                    if (emplTO.Picture == null)
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(rm.GetString("defined", culture));
                    if (emplTO.EmployeeTypeID == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.EmployeeTypeID.ToString().Trim());
                    if (emplTO.PersonalHolidayCategory == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.PersonalHolidayCategory.ToString().Trim());
                    if (emplTO.PersonalHolidayDate.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(emplTO.PersonalHolidayDate.ToString(Constants.dateFormat));
                    item.SubItems.Add(emplTO.EmailAddress.Trim());
                    item.SubItems.Add(emplTO.JMBG.Trim());
                    item.SubItems.Add(emplTO.Username.Trim());
                    item.SubItems.Add(emplTO.Status.Trim());
                    item.SubItems.Add(emplTO.EmployeeBranch.Trim());
                    if (emplTO.AnnualLeaveStartDate.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(emplTO.AnnualLeaveStartDate.ToString(Constants.dateFormat));
                    if (emplTO.AnnualLeaveCurrentYear == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.AnnualLeaveCurrentYear.ToString().Trim());
                    if (emplTO.AnnualLeavePreviousYear == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.AnnualLeavePreviousYear.ToString().Trim());
                    if (emplTO.AnnualLeaveCurrentYearLeft == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.AnnualLeaveCurrentYearLeft.ToString().Trim());
                    if (emplTO.AnnualLeavePreviousYearLeft == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.AnnualLeavePreviousYearLeft.ToString().Trim());
                    item.SubItems.Add(emplTO.Language.Trim());
                    if (emplTO.WorkLocationID == -1)
                        item.SubItems.Add("");                        
                    else
                        item.SubItems.Add(emplTO.WorkLocationID.ToString().Trim());
                    item.SubItems.Add(emplTO.WorkLocationCode.Trim());
                    if (emplTO.PositionID == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(emplTO.PositionID.ToString().Trim());
                    item.SubItems.Add(emplTO.Address.Trim());
                    if (emplTO.DateOfBirth.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(emplTO.DateOfBirth.ToString(Constants.dateFormat));
                    item.SubItems.Add(emplTO.PhoneNumber1.Trim());
                    item.SubItems.Add(emplTO.PhoneNumber2.Trim());
                    if (emplTO.ValidFrom.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(emplTO.ValidFrom.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    item.SubItems.Add(emplTO.CreatedBy.Trim());
                    if (emplTO.CreatedTime.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(emplTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (emplTO.CreatedTimeHist.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(emplTO.CreatedTimeHist.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    if (emplTO.Result == Constants.yesInt)
                        item.SubItems.Add(rm.GetString("succ", culture));
                    else
                        item.SubItems.Add(rm.GetString("fail", culture));
                    item.SubItems.Add(emplTO.Remark.Trim());
                    item.ToolTipText = emplTO.Remark;
                    item.Tag = emplTO;

                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.populateEmployeesSync(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbWU.SelectedValue != null && cbWU.SelectedValue is int)
                    populateFSSync();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbOU.SelectedValue != null && cbOU.SelectedValue is int)
                    populateOUSync();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbCCCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbCCCode.SelectedValue != null && cbCCCode.SelectedValue is int)
                    populateCCSync();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbCCCode_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbPosition.SelectedValue != null && cbPosition.SelectedValue is int)
                    populatePositionsSync();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbPosition_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbEmplEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbEmplEmployee.SelectedValue != null && cbEmplEmployee.SelectedValue is int)
                    populateEmployeesSync();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbEmplEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbResEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbResEmployee.SelectedValue != null && cbResEmployee.SelectedValue is int)
                    populateResponsibilitySync();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbResEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbALEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbALEmployee.SelectedValue != null && cbALEmployee.SelectedValue is int)
                    populateALRecalculationSync();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbALEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvWU_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvWU.Sorting;

                if (e.Column == _compFS.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvWU.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvWU.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _compFS.SortColumn = e.Column;
                    lvWU.Sorting = SortOrder.Ascending;
                }

                lvWU.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.lvWU_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvOU_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvOU.Sorting;

                if (e.Column == _compOU.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvOU.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvOU.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _compOU.SortColumn = e.Column;
                    lvOU.Sorting = SortOrder.Ascending;
                }

                lvOU.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.lvOU_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvCC_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvCC.Sorting;

                if (e.Column == _compCC.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvCC.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvCC.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _compCC.SortColumn = e.Column;
                    lvCC.Sorting = SortOrder.Ascending;
                }

                lvCC.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.lvCC_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvPosition_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvPosition.Sorting;

                if (e.Column == _compPos.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvPosition.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvPosition.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _compPos.SortColumn = e.Column;
                    lvPosition.Sorting = SortOrder.Ascending;
                }

                lvPosition.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.lvPosition_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployees_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvEmployees.Sorting;

                if (e.Column == _compEmpl.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvEmployees.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvEmployees.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _compEmpl.SortColumn = e.Column;
                    lvEmployees.Sorting = SortOrder.Ascending;
                }

                lvEmployees.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvResponsibilities_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvResponsibilities.Sorting;

                if (e.Column == _compRes.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvResponsibilities.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvResponsibilities.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _compRes.SortColumn = e.Column;
                    lvResponsibilities.Sorting = SortOrder.Ascending;
                }

                lvResponsibilities.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.lvResponsibilities_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvALRecalculation_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvALRecalculation.Sorting;

                if (e.Column == _compAL.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvALRecalculation.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvALRecalculation.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _compAL.SortColumn = e.Column;
                    lvALRecalculation.Sorting = SortOrder.Ascending;
                }

                lvALRecalculation.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.lvALRecalculation_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbALAppFS_CheckedChanged(object sender, EventArgs e)
        {
            rbCheckedChanged(rbALAppFS, rbALAppOU, cbALAppWU, cbALAppOU, cbALAppEmployee, btnALAppWUTree, btnALAppOUTree);
        }

        private void rbALAppOU_CheckedChanged(object sender, EventArgs e)
        {
            rbCheckedChanged(rbALAppFS, rbALAppOU, cbALAppWU, cbALAppOU, cbALAppEmployee, btnALAppWUTree, btnALAppOUTree);
        }

        private void cbALAppWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(rbALAppFS, rbALAppOU, cbALAppWU, cbALAppOU, cbALAppEmployee);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbALAppWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbALAppOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees(rbALAppFS, rbALAppOU, cbALAppWU, cbALAppOU, cbALAppEmployee);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.cbALAppOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnALAppWUTree_Click(object sender, EventArgs e)
        {
            showWUTree(cbALAppWU); ;
        }

        private void btnALAppOUTree_Click(object sender, EventArgs e)
        {
            showOUTree(cbALAppOU);
        }

        private void CloseSyncConnection()
        {
            try
            {
                if (SyncConnection != null)
                    syncDAO.CloseConnection(SyncConnection);
            }
            catch
            { }
        }

        private bool ConnectToSyncDB()
        {
            bool succ = false;
            try
            {
                if (syncDAO == null)
                    syncDAO = SynchronizationDAO.getDAO();

                SyncConnection = syncDAO.MakeNewDBConnection();
                
                if (SyncConnection == null)                
                    return false;
                
                syncDAO.setDBConnection(SyncConnection);

                succ = true;
            }
            catch
            {
                succ = false;
            }

            return succ;
        }

        private void SynchronizationPreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseSyncConnection();
        }

        private void btnSearchALRecalc_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                List<SyncAnnualLeaveRecalcTO> alList = syncDAO.getDifAnnualLeaveRecalc((int)cbALAppEmployee.SelectedValue);

                lvALRecalc.BeginUpdate();
                lvALRecalc.Items.Clear();

                foreach (SyncAnnualLeaveRecalcTO alTO in alList)
                {
                    ListViewItem item = new ListViewItem();
                    if (alTO.EmployeeID == -1)
                        item.Text = "";
                    else
                        item.Text = alTO.EmployeeID.ToString().Trim();
                    if (emplDict.ContainsKey(alTO.EmployeeID))
                        item.SubItems.Add(emplDict[alTO.EmployeeID].FirstAndLastName.Trim());
                    else
                        item.SubItems.Add("N/A");
                    if (alTO.Year.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(alTO.Year.ToString("yyyy"));
                    if (alTO.NumOfDays == -1)
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(alTO.NumOfDays.ToString().Trim());
                    item.SubItems.Add(alTO.CreatedBy.Trim());
                    if (alTO.CreatedTime.Equals(new DateTime()))
                        item.SubItems.Add("N/A");
                    else
                        item.SubItems.Add(alTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    
                    item.Tag = alTO;

                    lvALRecalc.Items.Add(item);
                }

                lvALRecalc.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.btnSearchALRecalc_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (new SyncAnnualLeaveRecalcFlag().Update(Constants.yesInt, false, true))
                    MessageBox.Show(rm.GetString("recalculationApproved", culture));
                else
                    MessageBox.Show(rm.GetString("recalculationApprovedFailed", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.btnApprove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNotApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (new SyncAnnualLeaveRecalcFlag().Update(Constants.noInt, false, true))
                    MessageBox.Show(rm.GetString("recalculationNotApproved", culture));
                else
                    MessageBox.Show(rm.GetString("recalculationNotApprovedFailed", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SynchronizationPreview.btnNotApprove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}



