using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Domain
{
    public class BookFloor
    {
        public int BookFloorId { get; set; }
        public string? BookFloorName { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public virtual List<Shelf> Shelves { get; set; } = new List<Shelf>();
    }
}
