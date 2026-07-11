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
            
        }
    }
}
