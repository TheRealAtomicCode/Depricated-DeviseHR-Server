namespace DeviseHR_Server.DTOs.RequestDTOs
{
       // Regular 3,
       // Variable 2,
       // Agency 1
 

    public class CreateContractRequest
    {
        public int user_id { get; set; }
        public int? pattern_id { get; set; }
        public int contract_type { get; set; }
        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }
        public int contracted_working_hours_per_week_in_minutes { get; set; }
        public int full_time_working_hours_per_week_in_minutes { get; set; }
        public int contracted_working_days_per_week { get; set; }
        public int avrage_working_day { get; set; }
        public bool is_leave_in_days { get; set; }
        public int companies_full_time_annual_leave_entitlement { get; set; }
        public int contracted_annual_leave_entitlement { get; set; }
        public int this_years_annual_leave_allowence { get; set; }
        public int? term_time_id { get; set; }
    }
