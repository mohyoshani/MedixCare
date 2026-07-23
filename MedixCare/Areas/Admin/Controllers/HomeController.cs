using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize]
    public class HomeController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;


         var todayAppointments = await _context.Appointments
        .Include(a => a.Patient)
        .Include(a => a.Doctor)
        .Where(a => a.AppointmentDate.Date == today)
        .ToListAsync();

            var totalToday = todayAppointments.Count;

            
            int completedCount = todayAppointments.Count(a => a.Status == AppointmentStatus.Completed);
            int canceledCount = todayAppointments.Count(a => a.Status == AppointmentStatus.Cancelled);
            int pendingCount = totalToday - (completedCount + canceledCount);

    
            var viewModel = new AdminDashboardVM
            {
                TodayAppointmentsCount = totalToday,
                PendingTestsCount = await _context.LabTests.CountAsync(t => string.IsNullOrEmpty(t.Summary)),
                TotalPatientsCount = await _context.Patients.CountAsync(),
                ActiveDoctorsCount = await _context.Doctors.CountAsync(d => d.IsActive),

             
                CompletedPercentage = totalToday > 0 ? (completedCount * 100) / totalToday : 0,
                CanceledPercentage = totalToday > 0 ? (canceledCount * 100) / totalToday : 0,
                PendingPercentage = totalToday > 0 ? (pendingCount * 100) / totalToday : 0,

                LiveAppointments = todayAppointments.OrderBy(a => a.AppointmentDate).Take(5).ToList(),
                RecentLabTests = await _context.LabTests
                    .Include(t => t.Patient)
                    .OrderByDescending(t => t.TestDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}
