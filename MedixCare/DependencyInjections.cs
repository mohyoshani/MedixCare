using MedixCare.Utility.DbInitialiZer;
using MedixCare.Utility.EmailService;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Runtime.CompilerServices;

namespace MedixCare
{
    public static class DependencyInjections
    {
        public static void InjectAll(this IServiceCollection services)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddTransient<EmailSender>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IRepository<ApplicationUserOTP>, Repository<ApplicationUserOTP>>();
            services.AddScoped<IRepository<Clinic>, Repository<Clinic>>();
            services.AddScoped<IRepository<Doctor>, Repository<Doctor>>();
            services.AddScoped<IRepository<DoctorSchedule>, Repository<DoctorSchedule>>();
            services.AddScoped<IRepository<DoctorLeave>, Repository<DoctorLeave>>();
            services.AddScoped<IRepository<PatientHistory>, Repository<PatientHistory>>();
            services.AddScoped<IRepository<Patient>, Repository<Patient>>();
            services.AddScoped<IRepository<Appointment>, Repository<Appointment>>();
            services.AddScoped<IRepository<Patient> , Repository<Patient>>();
            services.AddScoped<IRepository<LabTest>, Repository<LabTest>>();
            services.AddScoped<IRepository<Prescription>, Repository<Prescription>>();
            services.AddScoped<IRepository<PrescriptionItem>, Repository<PrescriptionItem>>();
            services.AddTransient<IFileHandler, FileHandler>();
            
        }
    }
}
