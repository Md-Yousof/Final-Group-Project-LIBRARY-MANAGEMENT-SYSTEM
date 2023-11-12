namespace LibraryAPI_R53_A.DTOs
{
    public class PublisherDto
    {
        public int PublisherId { get; set; }
        public string? PublisherName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
