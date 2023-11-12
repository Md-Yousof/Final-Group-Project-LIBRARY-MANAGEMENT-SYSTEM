using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Domain
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public int? PublisherId { get; set; }
        [JsonIgnore]
        public Publisher? Publisher { get; set; }
        public DateTime PublishedYear { get; set; }
        public string? Edition { get; set; }
        public int? TotalCopies { get; set; }
        public string? Language { get; set; }
        public string? Description { get; set; }
        public decimal? BookPrice { get; set; }
        //[JsonIgnore]
        [NotMapped]
        public decimal? RefundPrice
        {
            get
            {
                if (BookPrice.HasValue)
                {
                    return BookPrice * 0.7m; //70% of book price
                }
                else
                {
                    return null;
                }
            }
        }
        public string? DDCCode { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public IFormFile? Cover { get; set; }
        public bool IsDigital { get; set; }
        [NotMapped]
        public IFormFile? EBook { get; set; }
        public bool PublisherAgreement { get; set; }
        [NotMapped]
        public IFormFile? AgreementBookCopy { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }

        //public int SubcategoryId { get; set; }
        //[JsonIgnore]
        //public Subcategory? Subcategory { get; set; }

        public ICollection<BookAuthor>? BookAuthor { get; set; }
        [JsonIgnore]

        public List<BookCopy> Copies { get; set; } = new List<BookCopy>();
        [JsonIgnore]
        public ICollection<BookReview>? BookReviews { get; set; }
        [JsonIgnore]
        public ICollection<BookWishlist>? BookWishlists { get; set; }
        [JsonIgnore]
        public ICollection<BorrowedBook>? BorrowedBooks { get; set; }

        public string? CoverFileName { get; set; }
        public string? EBookFileName { get; set; }
        public string? AgreementFileName { get; set; }
    }
}
