namespace MedixCare.ViewModels
{
    public class UpdateAppointmentVM
    {
        public int Id { get; set; } 

        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public VisitType VisitType { get; set; }
        public int PatientId { get; set; }
        public AppointmentStatus Status { get; set; }
        public IEnumerable<Patient>? PatientsList { get; set; }
        public IEnumerable<DoctorSchedule>? DoctorSchedules { get; set; }
        public IEnumerable<DoctorLeave>?DoctorLeaves { get; set; }
    }
}
