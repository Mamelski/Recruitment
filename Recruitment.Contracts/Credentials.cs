using System.ComponentModel.DataAnnotations;

namespace Recruitment.Contracts
{
    public class Credentials
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}