namespace MedixCare.ViewModels
{
    public class ResendEmailConfirmationVM
    {
        [Required]
        public string EmailOrUserName { get; set; }  = string.Empty;
    }
}
