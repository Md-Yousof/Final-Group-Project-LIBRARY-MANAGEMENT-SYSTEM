using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Domain
{
    public enum BookCondition
    {
        Good,
        ToRepair,
        Damaged,
    }
    public class BookCopy
    {
        public int BookCopyId { get; set; }
        public string? CallNumber { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookCondition? condition { get; set; }
        public int BookId { get; set; }
        [JsonIgnore]
        public Book? Book { get; set; }
        public string? DDC { get; set; }
        public int ShelfId { get; set; }
        [JsonIgnore]
        public Shelf? Shelf { get; set; }
        [JsonIgnore]
        public ICollection<BorrowedBook>? BorrowBook { get; set; }
        //[JsonIgnore]
        //public ICollection<Inspection>? Inspections { get; set; }
    }
}
