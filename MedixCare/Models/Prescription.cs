namespace MedixCare.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; } 

        public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
    }
}
