namespace MedixCare.ViewModels
{
    public class AdminDashboardVM
    {
        public int TodayAppointmentsCount { get; set; }
        public int PendingTestsCount { get; set; }
        public int TotalPatientsCount { get; set; }
        public int ActiveDoctorsCount { get; set; }
        public IEnumerable<Appointment> LiveAppointments { get; set; } = new List<Appointment>();
        public IEnumerable<LabTest> RecentLabTests { get; set; } = new List<LabTest>();
        public int CompletedPercentage { get; set; }
        public int PendingPercentage { get; set; }
        public int CanceledPercentage { get; set; }
    }
}
