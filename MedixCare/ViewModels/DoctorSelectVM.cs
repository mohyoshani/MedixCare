namespace MedixCare.ViewModels
{
    public class DoctorSelectVM
    {
            public string? query { get; set; }
            public int CurrentPage { get; set; } = 1;
            public int TotalPages {  get; set; }
            public IEnumerable<Doctor> Doctors { get; set; } = new List<Doctor>();
            public int doctorId { get; set; }
        }
    }

