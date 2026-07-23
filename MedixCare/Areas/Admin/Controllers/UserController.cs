using Microsoft.AspNetCore.Mvc;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int page = 1, string? query = null , CancellationToken cancellationToken = default)
        {
            var users = _userManager.Users.AsQueryable();
            
            //Pagination
            var userCount = await users.CountAsync(cancellationToken);
            var userList  = await users.Skip((page - 1) * 5).Take(5).ToListAsync(cancellationToken);
            var totalPages = Math.Ceiling(userCount / 5.0);
            var currentPage = page;

            //filter by query

            if (query is not null)
            {
                users = users.Where(u => u.NormalizedUserName!.Contains(query) || u.NormalizedEmail!.Contains(query));
            }
            var userRoles = new Dictionary<ApplicationUser, string>();

            foreach (var item in userList)
            {
                userRoles.Add(item, (await _userManager.GetRolesAsync(item)).FirstOrDefault()!);
            }
            var model = new ApplicationUserFilterVM()
            {
                UserRoles = userRoles.ToDictionary(),
                query = query,
                totalPages = totalPages,
                currentPage = currentPage
            };
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {


            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            //if (await _userManager.IsInRoleAsync(user, SD.SuperAdmin_Role)) return NotFound();
            var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault()!;
            return View(new UserWithRoleVM()
            {

                ApplicationUser = user,
                RoleName = userRole,
                IdentityRoles = _roleManager.Roles.AsEnumerable()

            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UserWithRoleVM userRolesVM)
        {


            var user = await _userManager.FindByIdAsync(userRolesVM.Id);
            if (user is null) return NotFound();

            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (currentRole is not null)
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
            }

            await _userManager.AddToRoleAsync(user, userRolesVM.RoleName);

            TempData["success"] = "User role updated successfully";
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> LockUnlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            //يعكس الحالة لوك او انلوك حسب الحالة الحالية اللي جاية من الفيو
            user.LockoutEnabled = !user.LockoutEnabled;
            if (!user.LockoutEnabled)
            {
                user.LockoutEnd = DateTime.Now.AddDays(14); ;
                TempData["warning"] = $"User {user.UserName} locked successfully";
            }
            else
            {
                TempData["warning"] = $"User {user.UserName} unlocked successfully";
                user.LockoutEnd = null;
            }


            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
