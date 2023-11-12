namespace LibraryAPI_R53_A.DTOs
{
    public class SubscriptionPlanDto
    {
        public int SubscriptionPlanId { get; set; }
        public string? PlanName { get; set; }
        public string? PlanDescription { get; set; }
        public decimal PlanPrice { get; set; }
        public bool IsActive { get; set; }
        public decimal? MonthlyFee { get; set; }//new added

    }
}
