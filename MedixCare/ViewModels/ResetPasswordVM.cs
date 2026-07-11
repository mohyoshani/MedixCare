namespace MedixCare.ViewModels
{
    public class ResetPasswordVM
    {

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string userId { get; set; } = null!;
        public string Token { get; set; } = null!;

    }
}