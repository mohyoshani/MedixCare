

namespace MedixCare.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; } = string.Empty;
        public DateOnly DateOfbirth { get; set; }

        public string FullName { get; set; } = string.Empty;

        public Patient? Patient { get; set; }

    }
}
