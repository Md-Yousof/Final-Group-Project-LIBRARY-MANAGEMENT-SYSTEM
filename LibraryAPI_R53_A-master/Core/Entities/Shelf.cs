using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Domain
{
    public class Shelf
    {
        public int ShelfId { get; set; }
        public bool IsActive { get; set; }
        public string? ShelfName { get; set; }
        public int BookFloorId { get; set; }
        public BookFloor? BookFloor { get; set; }
        public virtual List<BookRack> BookRacks { get; set; } = new List<BookRack>();
        [JsonIgnore]
        public List<BookCopy> Copies { get; set; } = new List<BookCopy>();
    }
}
