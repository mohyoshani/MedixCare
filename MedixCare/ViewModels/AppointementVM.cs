namespace MedixCare.ViewModels
{
    public class AppointementVM
    {
        public string query { get; set; } = string.Empty;
        public double TotalPages { get; set; } 
        public int CurrentPage { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; } = [];
        public IEnumerable<Appointment> Appointments { get; set; } = [];
    }
}
