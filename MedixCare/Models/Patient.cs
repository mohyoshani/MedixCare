namespace MedixCare.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public class Patient
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Gender Gender { get; set; }
        public ICollection<PatientHistory> PatientHistories { get; set; } = new List<PatientHistory>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

}

