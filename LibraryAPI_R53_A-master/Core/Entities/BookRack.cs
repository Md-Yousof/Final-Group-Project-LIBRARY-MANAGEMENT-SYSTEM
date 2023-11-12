using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Domain
{
    public class BookRack
    {
        public int BookRackId { get; set; }
        public string? BookRackName { get; set; }
     
        public int ShelfId { get; set; }
        [JsonIgnore]
        public Shelf? Shelf { get; set; }
    }

}
