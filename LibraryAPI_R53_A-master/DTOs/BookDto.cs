using LibraryAPI_R53_A.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.DTOs
{
    public class BookDto
    {
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public int PublisherId { get; set; }

        public DateTime PublishedYear { get; set; }
        public string? Edition { get; set; }
        public int? TotalCopies { get; set; }
        public string? Language { get; set; }
        public string? Description { get; set; }
        public decimal? BookPrice { get; set; }

        public string? DDCCode { get; set; }
        public bool IsActive { get; set; } = true;

        public IFormFile? Cover { get; set; }
        public bool IsDigital { get; set; } = true;
        public IFormFile? EBook { get; set; }
        public bool PublisherAgreement { get; set; } = true;
        public IFormFile? AgreementBookCopy { get; set; }
        public int CategoryId { get; set; }
        //[JsonIgnore]
        public ICollection<BookAuthor>? BookAuthor { get; set; }
        public ICollection<int>? AuthorIds { get; set; }
        public ICollection<int>? AuthorIdsToRemove { get; set; }


    }
}
