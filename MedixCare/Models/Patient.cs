namespace MedixCare.Models
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male,

        [Display(Name = "Female")]
        Female
    }
    public class Patient
    {
        public int Id { get; set; }

        [MaxLength(11)]
        public string MobileNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Gender Gender { get; set; }
        public bool IsActive { get; set; }
        public ICollection<PatientHistory>? PatientHistories { get; set; } 
        public ICollection<Appointment>? Appointments { get; set; } 
        public ICollection<LabTest>? LabTests { get; set; } 
    }

}

