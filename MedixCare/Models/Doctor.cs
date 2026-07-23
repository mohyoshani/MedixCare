namespace MedixCare.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public decimal ExaminationFee { get; set; }
        public decimal FollowUpFee { get; set; }
        public int ClinicId { get; set; }
        public bool IsActive { get; set; } 
        public int FollowUpDays { get; set; }
        public Clinic? Clinic { get; set; }

        //public string? ApplicationUserId { get; set; }
        //public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();
        public ICollection<DoctorLeave> DoctorLeaves { get; set; } = new List<DoctorLeave>();
    }
}
