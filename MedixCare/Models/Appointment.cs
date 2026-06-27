namespace MedixCare.Models
{
    public enum BookingChannel
    {
        Phone,
        Online,
        WalkIn,
        Referral
    }
    public enum VisitType
    {
        FollowUp,
        Examination
    }
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }
    public class Appointment
    {
        public int Id { get; set; }
        public int? ParentAppointmentId { get; set; }
        public BookingChannel BookingChannel { get; set; } = BookingChannel.Phone;
        public VisitType VisitType { get; set; } = VisitType.Examination;
        public DateTime AppointmentDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public Doctor? Doctor { get; set; } 
        public Patient? Patient { get; set; }
        public Appointment? ParentAppointment { get; set; } 
        public ICollection<LabTest> LabTests { get; set; } = new List<LabTest>();
        public Prescription? Prescription { get; set; }
    }
}
