using System.ComponentModel.DataAnnotations;

namespace ChatChit.Models.RequestModel
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
