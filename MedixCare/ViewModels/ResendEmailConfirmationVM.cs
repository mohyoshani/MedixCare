namespace MedixCare.ViewModels
{
    public interface ResendEmailConfirmationVM
    {
        [Required]
        public string EmailOrUserName { get; set; } 
    }
}
