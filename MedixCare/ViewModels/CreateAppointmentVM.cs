using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedixCare.ViewModels
{
    public class CreateAppointmentVM
    {

        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public VisitType VisitType { get; set; }

        public string DoctorName { get; set; } = string.Empty; 
        public IEnumerable<Patient>? PatientsList { get; set; }
        public int PatientId { get; set; }
        public IEnumerable<DoctorLeave>? doctorLeaves { get; set; }
        public IEnumerable<DoctorSchedule>? doctorSchedules { get; set; }
    }
}