using LibraryAPI_R53_A.Core.Domain;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int? BorrowId { get; set; }
        [JsonIgnore]
        public BorrowedBook? BorrowedBook { get; set; }
        public string? UserId { get; set; } //for monthly fees
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        //public int? SubId {get; set; } //if subscribed user
        //public SubscriptionPlan? Plan { get; set; }
        public decimal? Payment { get; set; } //all kinds of payment (monthly fees and/or book price)
        public decimal? Refund { get; set; }//non subscribed pay per borrow 70% and minus other fines
        public DateTime? TransactionDate { get; set; } //added at acception also modified at returned
        public decimal? Fine { get; set; }
        public decimal? MiscellaneousFines { get; set; }
        public string? Remarks { get; set; }
        public string TransactionId { get; set; }

        public Invoice()
        {
            TransactionId = GenerateTransactionId();
        }

        private string GenerateTransactionId()
        {
            string datePart = DateTime.Now.ToString("yyyyMMddHHmmss");
            string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6); 

            return datePart + randomPart;
        }
    }
}
