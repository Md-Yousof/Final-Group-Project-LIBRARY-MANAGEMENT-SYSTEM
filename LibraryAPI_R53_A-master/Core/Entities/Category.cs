using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryAPI_R53_A.Core.Domain
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? DDCCode { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public virtual List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        [JsonIgnore]
        public ICollection<Book>? Books { get; set; }
        [JsonIgnore]
        public ICollection<UserPreference>? UserPreferences { get; set; }

    }
}
