namespace MedixCare.Models
{
    public class DoctorSchedule
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; } 
        public TimeOnly EndTime { get; set; } 
        public DayOfWeek DayOfWeek { get; set; }
        public int MaxPatients { get; set; } 
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

    }
}
