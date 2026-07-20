namespace MedixCare.Models
{
    public enum TestType
    {
        Scan , 
        Analysis
    }
    public class LabTest
    {
        public int Id { get; set; }
        public string TestName { get; set; } = string.Empty;
        public TestType testType { get; set; } 
        public DateTime TestDate { get; set; } = DateTime.UtcNow;
        public string LabName { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public Appointment? Appointment { get; set; }

        public ICollection<LabTestAttachment> Attachments { get; set; } = new List<LabTestAttachment>();
    }
}
