namespace LibraryAPI_R53_A.Core.Domain
{
    public class Subcategory
    {
        public int SubcategoryId { get; set; }
        public string? DDCCode { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
