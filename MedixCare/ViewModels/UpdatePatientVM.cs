namespace MedixCare.ViewModels
{
    public class UpdatePatientVM
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public bool isActive { get; set; }
    }
}
