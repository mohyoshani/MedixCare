namespace MedixCare.ViewModels
{
    public class PatientVM
    {
        public int currentPage { get; set; }
        public double totalPages { get; set; }
        public IEnumerable<Patient>? Patients { get; set; }  
        public string query { get; set; } = string.Empty;
    }
}
