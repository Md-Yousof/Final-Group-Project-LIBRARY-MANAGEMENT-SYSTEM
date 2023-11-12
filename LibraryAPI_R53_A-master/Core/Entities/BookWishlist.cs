namespace LibraryAPI_R53_A.Core.Domain
{
    public class BookWishlist
    {
        public int BookWishlistId { get; set; }
        public bool IsActive { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? UserInfo { get; set; }
        public int? BookId { get; set; }
        public Book? Book { get; set; }
    }
}
