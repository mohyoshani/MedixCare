namespace MedixCare.ViewModels
{
    public class CreatePatientFlattendVM
    {
        public string MobileNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public Gender Gender { get; set; }

    }
}
