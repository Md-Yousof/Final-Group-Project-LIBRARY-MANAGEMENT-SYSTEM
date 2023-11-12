namespace LibraryAPI_R53_A.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? DDCCode { get; set; }
        public bool IsActive { get; set; }
    }
}
