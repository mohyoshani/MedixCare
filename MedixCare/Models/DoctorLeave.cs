namespace MedixCare.Models
{
    public class DoctorLeave
    {
        public int Id { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime LeaveFrom { get; set; }
        public DateTime LeaveTo { get; set; }
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; } 
    }
}
