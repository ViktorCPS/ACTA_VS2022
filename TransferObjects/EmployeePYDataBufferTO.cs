using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class EmployeePYDataBufferTO
    {
       private uint _pYCalcID = 0;
       private string _companyCode = "";
       private string type = "";
       private int _employeeID = -1;
       private string _employeeType = "";
       private string _ccName = "";
       private string _ccDesc = "";
       private string _firstName = "";
       private string _lastName = "";
       private DateTime _dateStart = new DateTime();
       private DateTime _dateEnd = new DateTime();
       private int _fundHrs = -1;
       private int _fundDay = -1;
       private int _fundHrsEst = -1;
       private int _fundDayEst = -1;
       private int _mealCounter = -1;
       private int _transportCounter = -1;
       private int _vacationLeftCurrYear = -1;
       private int _vacationLeftPrevYear = -1;
       private int _vacationUsedCurrYear = -1;
       private int _valactionUsedPrevYear = -1;
       private decimal _bankHrsBalans = -1;
       private decimal _stopWorkingHrsBalans = -1;
       private int _paidLeaveBalans = -1;
       private int _paidLeaveUsed = -1;
       private int _notJustifiedOvertimeBalans = -1;
       private int _notJustifiedOvertime = -1;
       //private int _BGMealsApproved = -1;
       //private int _BGMealsNotApproved = -1;
       //private int _KGMealsApproved = -1;
       //private int _KGMealsNotApproved = -1;
       private Dictionary<int, int> _approvedMeals = new Dictionary<int, int>();
       private Dictionary<int, int> _notApprovedMeals = new Dictionary<int, int>();

       public int PaidLeaveUsed
       {
           get { return _paidLeaveUsed; }
           set { _paidLeaveUsed = value; }
       }

       public int PaidLeaveBalans
       {
           get { return _paidLeaveBalans; }
           set { _paidLeaveBalans = value; }
       }

       public decimal StopWorkingHrsBalans
       {
           get { return _stopWorkingHrsBalans; }
           set { _stopWorkingHrsBalans = value; }
       }

       public decimal BankHrsBalans
       {
           get { return _bankHrsBalans; }
           set { _bankHrsBalans = value; }
       }

       public int ValactionUsedPrevYear
       {
           get { return _valactionUsedPrevYear; }
           set { _valactionUsedPrevYear = value; }
       }

       public int VacationUsedCurrYear
       {
           get { return _vacationUsedCurrYear; }
           set { _vacationUsedCurrYear = value; }
       }

       public int VacationLeftPrevYear
       {
           get { return _vacationLeftPrevYear; }
           set { _vacationLeftPrevYear = value; }
       }

       public int VacationLeftCurrYear
       {
           get { return _vacationLeftCurrYear; }
           set { _vacationLeftCurrYear = value; }
       }

       public int TransportCounter
       {
           get { return _transportCounter; }
           set { _transportCounter = value; }
       }

       public int MealCounter
       {
           get { return _mealCounter; }
           set { _mealCounter = value; }
       }

       public int FundDay
       {
           get { return _fundDay; }
           set { _fundDay = value; }
       }

       public int FundHrs
       {
           get { return _fundHrs; }
           set { _fundHrs = value; }
       }

       public int FundDayEst
       {
           get { return _fundDayEst; }
           set { _fundDayEst = value; }
       }

       public int FundHrsEst
       {
           get { return _fundHrsEst; }
           set { _fundHrsEst = value; }
       }

       public DateTime DateEnd
       {
           get { return _dateEnd; }
           set { _dateEnd = value; }
       }

       public DateTime DateStart
       {
           get { return _dateStart; }
           set { _dateStart = value; }
       }         


       public string LastName
       {
           get { return _lastName; }
           set { _lastName = value; }
       }

       public string FirstName
       {
           get { return _firstName; }
           set { _firstName = value; }
       }

       public int EmployeeID
       {
           get { return _employeeID; }
           set { _employeeID = value; }
       }

       public int NotJustifiedOvertime
       {
           get { return _notJustifiedOvertime; }
           set { _notJustifiedOvertime = value; }
       }

       public int NotJustifiedOvertimeBalans
       {
           get { return _notJustifiedOvertimeBalans; }
           set { _notJustifiedOvertimeBalans = value; }
       }

       public string EmployeeType
       {
           get { return _employeeType; }
           set { _employeeType = value; }
       }

       public string CCName
       {
           get { return _ccName; }
           set { _ccName = value; }
       }

       public string CCDesc
       {
           get { return _ccDesc; }
           set { _ccDesc = value; }
       }

       public string Type
       {
           get { return type; }
           set { type = value; }
       }

       public string CompanyCode
       {
           get { return _companyCode; }
           set { _companyCode = value; }
       }

       public uint PYCalcID
       {
           get { return _pYCalcID; }
           set { _pYCalcID = value; }
       }

       //public int BGMealsApproved
       //{
       //    get { return _BGMealsApproved; }
       //    set { _BGMealsApproved = value; }
       //}

       //public int BGMealsNotApproved
       //{
       //    get { return _BGMealsNotApproved; }
       //    set { _BGMealsNotApproved = value; }
       //}

       //public int KGMealsApproved
       //{
       //    get { return _KGMealsApproved; }
       //    set { _KGMealsApproved = value; }
       //}

       //public int KGMealsNotApproved
       //{
       //    get { return _KGMealsNotApproved; }
       //    set { _KGMealsNotApproved = value; }
       //}

       public Dictionary<int, int> ApprovedMeals
       {
           get { return _approvedMeals; }
           set { _approvedMeals = value; }
       }

       public Dictionary<int, int> NotApprovedMeals
       {
           get { return _notApprovedMeals; }
           set { _notApprovedMeals = value; }
       }
    }
}
