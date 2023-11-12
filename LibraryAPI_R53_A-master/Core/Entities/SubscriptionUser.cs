using LibraryAPI_R53_A.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Entities
{
    public class SubscriptionUser
    {
        [Key]
        public int SubscriptonUserId { get; set; }

        public string? ApplicationUserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? ApplicationUser { get; set; }


        public int SubscriptionPlanId { get; set; }
        [JsonIgnore]
        public SubscriptionPlan? SubscriptionPlan { get; set; }
        public string? TransactionId { get; set; }
        public bool? Accepted { get; set; }
    }
}
