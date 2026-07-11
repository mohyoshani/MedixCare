namespace MedixCare.Utility.DbInitialiZer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DbInitializer> _logger;


        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, ILogger<DbInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }
        public async Task Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                    _context.Database.Migrate();
                if(!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.SuperAdmin_Role));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Admin_Role));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Customer_Role));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Employee_Role));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Doctor_Role));

                   await _userManager.CreateAsync(new ApplicationUser
                   {
                       FullName = "SuperAdmin",
                       Email = "SuperAdmin@Domain.com",
                       EmailConfirmed = true,
                       UserName = "SuperAdmin"

                   } , "SuperAdmin@2026#");
                }
                var user = await _userManager.FindByEmailAsync("SuperAdmin@Domain.com");
                await _userManager.AddToRoleAsync(user!, SD.SuperAdmin_Role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
               
            }
        }
    }
}
