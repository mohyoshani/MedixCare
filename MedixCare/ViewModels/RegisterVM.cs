
namespace MedixCare.ViewModels
{
    public class RegisterVM
    {

        [Required]
        public int Id { get; set; }

        [Required] 
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringValidation]
        public string UserName { get; set; } = string.Empty;

        
        [Required , DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        
        
        [Required , DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

       
        
        [Required(ErrorMessage = "Please confirm your password") , DataType(DataType.Password) , Compare(nameof(Password))]
        public string PasswordConfirmation { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;



    }
}
