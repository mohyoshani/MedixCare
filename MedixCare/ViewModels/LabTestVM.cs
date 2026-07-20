namespace MedixCare.ViewModels
{
    public class LabTestVM
    {
        public string query { get; set; } = string.Empty;
        public double TotalPages { get; set; }
        public int currentPage { get; set; }
        public IEnumerable<LabTest>? labTests { get; set; }
    }
}