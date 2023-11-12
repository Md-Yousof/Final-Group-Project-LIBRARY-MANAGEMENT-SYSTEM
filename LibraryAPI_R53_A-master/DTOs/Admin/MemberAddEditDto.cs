namespace LibraryAPI_R53_A.DTOs.Admin
{
    public class MemberAddEditDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        //public string Email { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
    }
}
