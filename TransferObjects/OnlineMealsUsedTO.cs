using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class OnlineMealsUsedTO
    {
        int employeeID = -1;
        long transactionID = -1;
        int pointID = -1;
        int mealTypeID = -1;
        DateTime eventTime = new DateTime();
        int qty = -1;
        int onlineValidation = -1;
        int manualCreated = -1;
        string approved = "";
        string approvedBy = "";
        DateTime approvedTime = new DateTime();
        string approvedDesc = "";
        string reasonRefused = "";
        int autoCheck = -1;
        DateTime autoCheckTime = new DateTime();
        string auto_check_failure_reason = "";
        string createdBy = "";
        DateTime createdTime = new DateTime();
        string modifiedBy = "";
        DateTime modifiedTime = new DateTime();        
        string pointName = "";
        string employeeName = "";
        string restaurantName = "";
        string mealType = "";
        string emplStringone = "";
        string emplLocation = "";

        public string RestaurantName
        {
            get { return restaurantName; }
            set { restaurantName = value; }
        }

        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        public string EmployeeStringone
        {
            get { return emplStringone; }
            set { emplStringone = value; }
        }

        public string EmployeeLocation
        {
            get { return emplLocation; }
            set { emplLocation = value; }
        }

        public string PointName
        {
            get { return pointName; }
            set { pointName = value; }
        }
        
        public string MealType
        {
            get { return mealType; }
            set { mealType = value; }
        }
        public DateTime ModifiedTime
        {
            get { return modifiedTime; }
            set { modifiedTime = value; }
        }

        public string ModifiedBy
        {
            get { return modifiedBy; }
            set { modifiedBy = value; }
        }

        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public string AutoCheckFailureReason
        {
            get { return auto_check_failure_reason; }
            set { auto_check_failure_reason = value; }
        }

        public DateTime AutoCheckTime
        {
            get { return autoCheckTime; }
            set { autoCheckTime = value; }
        }

        public int AutoCheck
        {
            get { return autoCheck; }
            set { autoCheck = value; }
        }

        public string ReasonRefused
        {
            get { return reasonRefused; }
            set { reasonRefused = value; }
        }

        public string ApprovedDesc
        {
            get { return approvedDesc; }
            set { approvedDesc = value; }
        }

        public DateTime ApprovedTime
        {
            get { return approvedTime; }
            set { approvedTime = value; }
        }

        public string ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        public string Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        public int ManualCreated
        {
            get { return manualCreated; }
            set { manualCreated = value; }
        }

        public int OnlineValidation
        {
            get { return onlineValidation; }
            set { onlineValidation = value; }
        }


        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        public DateTime EventTime
        {
            get { return eventTime; }
            set { eventTime = value; }
        }

        public int MealTypeID
        {
            get { return mealTypeID; }
            set { mealTypeID = value; }
        }

        public int PointID
        {
            get { return pointID; }
            set { pointID = value; }
        }

        public long TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; }
        }


        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

    }
}
