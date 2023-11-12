namespace LibraryAPI_R53_A.Core.Domain
{
    public class BookReview
    {
        public int BookReviewId { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? UserInfo { get; set; }
        public string? Comments { get; set; }
        public bool IsActive { get; set; }
    }
}
