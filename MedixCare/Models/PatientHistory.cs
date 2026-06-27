namespace MedixCare.Models
{
    public class PatientHistory
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;

        //Like allergies, past surgeries, chronic conditions, medications, etc.
        public string Notes { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; } 
        public Patient? Patient { get; set; }

    }
}
