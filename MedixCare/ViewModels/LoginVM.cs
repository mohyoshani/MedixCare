namespace MedixCare.ViewModels
{
    public interface LoginVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Username or Email")]
        public string UserNameOrEmail { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
