namespace LibraryAPI_R53_A.Core.Domain
{
    public class UserPreference
    {
        public int UserPreferenceId { get; set; }
        public bool IsActive { get; set; }
        public string? UserInfoId { get; set; }
        public ApplicationUser? UserInfo { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
