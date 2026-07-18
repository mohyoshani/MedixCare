namespace MedixCare.ViewModels
{
    public class CreateAppointmentVM
    {
        public IEnumerable<Doctor> Doctors { get; set; } = [];
        public IEnumerable<Patient> Patients { get; set; } = [];

        [Required(ErrorMessage = "Please select a patient.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select a doctor.")]
        public int DoctorId { get; set; }

        public int? ParentAppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; } = DateTime.UtcNow;

        public BookingChannel BookingChannel { get; set; } = BookingChannel.Phone;

        public VisitType VisitType { get; set; } = VisitType.Examination;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    }
}
