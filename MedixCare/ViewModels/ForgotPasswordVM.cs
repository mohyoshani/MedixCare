namespace MedixCare.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        public string UserNameOrEmail { get; set; } = string.Empty;
    }
}
