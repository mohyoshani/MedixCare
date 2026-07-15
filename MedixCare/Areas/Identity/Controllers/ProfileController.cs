using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace MedixCare.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = user.Adapt<ApplicationUserVM>();
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> Update(ApplicationUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Address = model.Address;

            if (user.Email != model.Email)
            {
                await _userManager.SetEmailAsync(user, model.Email);
                await _userManager.SetUserNameAsync(user, model.Email);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            if (model.CurrentPassword is not null && model.NewPassword is not null)
            {
                    var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!passwordResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Join(", ", passwordResult.Errors.Select(e => e.Description)), "Password change failed.");
                        return View(model);
                    }
            }
            await _signInManager.RefreshSignInAsync(user);
            TempData["Success"] = "Profile and Password Updated successfully.";
            return RedirectToAction("Update");
        }
    }
}
