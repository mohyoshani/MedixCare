namespace MedixCare.Models
{
    public class LabTest
    {
        public int Id { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string TestType { get; set; } = string.Empty;
        public DateTime TestDate { get; set; } = DateTime.UtcNow;
        public string LabName { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string TestFileName { get; set; } = string.Empty;
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
