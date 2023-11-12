using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI_R53_A.DTOs.Account
{
    // Step 09
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        //[RegularExpression("",ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be at leaset {2}, and maximum {1} characters")]
        public string Password { get; set; }
    }
}
