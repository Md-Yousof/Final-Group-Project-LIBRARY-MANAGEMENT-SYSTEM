using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Domain
{
    public class BookAuthor
    {


        public int BookId { get; set; }
        [JsonIgnore]
        public Book? Book { get; set; }
        public int AuthorId { get; set; }
        [JsonIgnore]
        public Author? Author { get; set; }
    }
}
