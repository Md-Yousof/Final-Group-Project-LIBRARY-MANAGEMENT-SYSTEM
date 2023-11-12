using System.ComponentModel.DataAnnotations;

namespace LibraryAPI_R53_A.DTOs.Account
{
    // ----- Step 08
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
