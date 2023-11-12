namespace LibraryAPI_R53_A.DTOs.Admin
{
    public class MemberViewDto
    {
        public string id { get; set; }
        public string UserName { get; set; }
        public bool IsLocked { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
