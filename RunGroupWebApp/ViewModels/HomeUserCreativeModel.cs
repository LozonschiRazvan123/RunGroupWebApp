using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModels
{
    public class HomeUserCreativeModel
    {
        public string UserName { get; set; }
        public string Email { get;set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter the password!")]
        public string Password { get; set; }
        [Required]
        public int? ZipCode { get; set; }
    }
}
