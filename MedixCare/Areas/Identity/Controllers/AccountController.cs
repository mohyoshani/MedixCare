using MedixCare.Utility.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Cryptography;

namespace MedixCare.area.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;
        private readonly IRepository<ApplicationUserOTP> _userOTP;

        public AccountController(
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            EmailSender emailSender, ILogger<AccountController> logger, IRepository<ApplicationUserOTP> userOTP)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _userOTP = userOTP;
        }

        //----------------------Aceesss Denied-----------------------

        public IActionResult AccessDenied()
        {
            return View();
        }



        //----------------Register-----------------------

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Address = model.Address,
                FullName = model.FullName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["error"] = "Failed To Register";
                return View(model);
            }
            bool isEmailSent = await SendEmailConfirmation(user);
            if (!isEmailSent)
            {
                TempData["error"] = "Try Registering again later, Failed to send confirmation email";
                return RedirectToAction(nameof(ResendConfirmationEmail));
            }
            else
            {
                TempData["success"] = "Account Registered Successfully. Please check your email.";
            }

           
            return RedirectToAction("Login", "Account", new { area = SD.IDENTITY_AREA });
        }

        //-----------------------Confirmation Email-----------------------------
       
        public async Task<IActionResult> Confirmation(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["error"] = string.Join(",", result.Errors.Select(a => a.Description));
                return RedirectToAction(nameof(Login));
            }
            TempData["success"] = "Account Confirmed Successfully";
            await _userManager.AddToRoleAsync(user, SD.Customer_Role);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ResendConfirmationEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendConfirmationEmail(ResendEmailConfirmationVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.EmailOrUserName)
                                 ?? await _userManager.FindByNameAsync(model.EmailOrUserName);
            if (user is null)
            {
                ModelState.AddModelError("EmailOrUserName", "Email or User Name is Invalid");
                return View(model);
            }
            bool isEmailSent = await SendEmailConfirmation(user);
            if (!isEmailSent)
            {
                ModelState.AddModelError(string.Empty, "Failed to send confirmation email, Try again later");
                return View(model);
            }

            else
            {
                TempData["success"] = "Confirmation Email Sent Successfully , Please check your email.";
            }
            return RedirectToAction(nameof(Login));
        }

        //----------------Login-----------------------

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail) 
                ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            var signInName = user?.UserName ?? model.UserNameOrEmail;

        
            var result = await _signInManager.PasswordSignInAsync(signInName, model.Password, model.RememberMe, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("UserNameOrEmail", "User Name or Email is Invalid");
                ModelState.AddModelError("Password", "Password is Invalid");
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Too Many Attemps, Try again after 5 minutes");
                }

                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Your email is not confirmed yet. Please check your inbox or resend confirmation email.");
                }
                return View(model);
            }
            TempData["success"] = $"Welcome {user!.FullName} , Login Successful";
            return RedirectToAction("Index", "Home", new { area = SD.CUSTOMER_AREA });

        }

        //--------------Logout-----------------------
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Clear();
            return RedirectToAction("Index", "Home", new { area = SD.CUSTOMER_AREA});
        }


        //-----------------Forgot Password-----------------------
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail)
                ?? await _userManager.FindByNameAsync(model.UserNameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError("UserNameOrEmail", "User Name or Email is Invalid");
                return View(model);
            }

            
                bool isOtpSent = await SendOTP(user);
                if (!isOtpSent)
                {
                    ModelState.AddModelError(string.Empty, "Failed to send OTP , Try again later");
                    return View(model);
                }
                TempData["success"] = "OTP Sent Successfully";

                return RedirectToAction(nameof(ValidateOTP), new { userId = user.Id! });
         
        }


        //-----------------Validate OTP-----------------------

        [HttpGet]
        public IActionResult ValidateOTP(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var model = new ValidateOTPVM
            {
                userId = userId
            };
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ValidateOTP(ValidateOTPVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var OTPInDb = await _userOTP.GetOneAsync(a => a.ApplicationUserId == model.userId && !a.IsUsed, cancellationToken: default , Tracked: true);

            if (OTPInDb is null)
            {
                TempData["error"] = "OTP is not Found";
                return View(model);
            }
            if (OTPInDb.OTP != model.OTP)
            {
                TempData["error"] = "Invalid OTP";
                return View(model);
            }

            OTPInDb.IsUsed = true;
            _userOTP.Update(OTPInDb);
            await _userOTP.CommitChangesAsync(cancellationToken: default);

            var user = await _userManager.FindByIdAsync(model.userId);
            if (user is null)
            {
                ModelState.AddModelError("", "User account no longer exists.");
                return View(model);
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return RedirectToAction(nameof(ResetPassword), new { userId = user!.Id, token = resetToken });
        }


        //-----------------Reset Password-----------------------

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return NotFound();

            var model = new ResetPasswordVM
            {
                userId = userId,
                Token = token
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = model.userId;
            var user = await _userManager.FindByIdAsync(userId!);

            if (user is null)
            {
                return NotFound();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {

                TempData["error"] = String.Join(", ", result.Errors.Select(e => e.Description));

                return View(model);

            }
            TempData["success"] = "Password Reset Successfully";
            return RedirectToAction(nameof(Login), "Account", new { area = SD.IDENTITY_AREA });
        }



        //Private Method to send email confirmation

        private async Task<bool> SendEmailConfirmation(ApplicationUser user)
        {
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action("Confirmation", "Account", new { area = SD.IDENTITY_AREA, userId = user.Id, token = token }, Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email!, $"Confirm your email {user.FullName}", $"Please confirm your account by clicking this link: <a href='{link}'>link</a>");
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email confirmation: {ex.Message}");
                return false;
            }

        }


        private async Task<bool> SendOTP(ApplicationUser user)
        {
            try
            {
                var oldOtps = await _userOTP.GetAllAsync(a => a.ApplicationUserId == user.Id && !a.IsUsed && a.ExpireIn > DateTime.UtcNow);

                if (oldOtps is not null && oldOtps.Any())
                {
                    foreach (var oldOtp in oldOtps)
                    {
                        oldOtp!.IsUsed = true;
                        _userOTP.Update(oldOtp);
                    }
                }
                var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
                await _emailSender.SendEmailAsync(user.Email!, $"Your OTP Code", $"Your OTP code is: {otp}");
                var userOtp = new ApplicationUserOTP
                {
                    ApplicationUserId = user.Id,
                    OTP = otp,
                };
                await _userOTP.CreateAsync(userOtp, cancellationToken: default);
                await _userOTP.CommitChangesAsync(cancellationToken: default);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending OTP: {ex.Message}");
                return false;
            }
        }
    }
}
