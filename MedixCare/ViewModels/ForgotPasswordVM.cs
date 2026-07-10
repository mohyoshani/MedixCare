namespace MedixCare.ViewModels
{
    public interface ForgotPasswordVM
    {
        [Required]
        public string UserNameOrEmail { get; set; }
    }
}
