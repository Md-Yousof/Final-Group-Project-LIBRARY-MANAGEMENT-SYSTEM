using LibraryAPI_R53_A.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public string? ProfileImage { get; set; }
        [NotMapped]
        public IFormFile? UserImage { get; set; }

        [JsonIgnore]
        public ICollection<SubscriptionUser>? SubscriptionUsers { get; set; }

        public bool IsSubscribed { get; set; }
      
        public virtual List<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();
        [JsonIgnore]
        public ICollection<BookReview>? BookReviews { get; set; }
        [JsonIgnore]
        public ICollection<BorrowedBook>? BorrowedBooks { get; set;}
        [JsonIgnore]
        public ICollection<BookWishlist>? BookWishlists { get; set; }
        [JsonIgnore]
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
