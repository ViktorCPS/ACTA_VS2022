using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace TransferObjects
{
    public class VacationReportTO
    {
        [DisplayName("Employee ID")]
        [Description("Employee ID")]
        public int emp_id { get; set; }

        [DisplayName("Name")]
        public string fullName { get; set; }

        [DisplayName("Organization Unit")]
        public string orgUnit_name { get; set; }

        [DisplayName("Working Unit")]
        public string workingUnit_name { get; set; }

        [DisplayName("Division")]
        public string division_name { get; set; }

        [DisplayName("Vacation Days (This Year)")]
        public int vacation_thisYear { get; set; }

        [DisplayName("Vacation Days (Last Year)")]
        public int vacation_lastYear { get; set; }

        [DisplayName("Vacation Days Used")]
        public int vacation_used { get; set; }

        [DisplayName("Days Left (Previous Year)")]
        public int days_left_previous_year { get; set; }

        [DisplayName("Days Left (Current Year)")]
        public int days_left_current_year { get; set; }

        [DisplayName("Spent YTD")]
        public int spent_ytd { get; set; }

        [DisplayName("Theoretical Maximum")]
        public int t_max { get; set; }

        [DisplayName("Accrual In Days YTD")]
        public int accrual_in_days_ytd { get; set; }
    }
}
