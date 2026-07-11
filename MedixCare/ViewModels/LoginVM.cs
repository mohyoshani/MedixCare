namespace MedixCare.ViewModels
{
    public class LoginVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Username or Email")]
        public string UserNameOrEmail { get; set; }  = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
