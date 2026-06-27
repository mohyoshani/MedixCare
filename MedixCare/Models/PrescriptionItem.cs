namespace MedixCare.Models
{
   
    public class PrescriptionItem
    {
     public int Id { get; set; }
        public string Dosage { get; set; } = string.Empty;
        public string MedicineName { get; set; } = string.Empty;
        public int MedicineFrequency { get; set; }
        public int DurationInDays { get; set; }
        public int PrescriptionId { get; set; }
        public Prescription? Prescription { get; set; } 
    }
}
