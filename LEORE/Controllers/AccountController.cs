using LEORE.Data;
using LEORE.Helpers;
using LEORE.Models;
using LEORE.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LEORE.Controllers
{
    public class AccountController : Controller
    {
        private readonly LEOREContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(LEOREContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // تحقق إذا كان البريد الإلكتروني مستخدم من قبل
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم من قبل");
                    return View(model);
                }

                // إنشاء مستخدم جديد
                var user = new User
                {
                    Email = model.Email,
                    Password = PasswordHasher.HashPassword(model.Password), // تشفير كلمة المرور
                    FirstName = model.FirstName,
                    lastName = model.LastName,
                    Phone = model.Phone,
                    Address = model.Address,
                    Role = "Customer", // دور افتراضي
                    CreatedAt = DateTime.Now
                };

                // حفظ في قاعدة البيانات
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // تسجيل الدخول تلقائياً بعد التسجيل
                await LoginUser(user);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // البحث عن المستخدم بالبريد الإلكتروني
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null || !PasswordHasher.VerifyPassword(model.Password, user.Password))
                {
                    ModelState.AddModelError(string.Empty, "البريد الإلكتروني أو كلمة المرور غير صحيحة");
                    return View(model);
                }

                // تسجيل الدخول
                await LoginUser(user);

                // إذا كان هناك returnUrl، ارجع إليه
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // مسح بيانات الجلسة
            HttpContext.Session.Clear();

            // إنشاء رد فعل خروج جديد
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: /Account/EditProfile
        //[HttpGet]
        //public async Task<IActionResult> EditProfile()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var user = await _context.Users.FindAsync(userId);

        //    if (user == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var model = new EditProfileViewModel
        //    {
        //        FirstName = user.FirstName,
        //        LastName = user.lastName,
        //        Phone = user.Phone,
        //        Address = user.Address
        //    };

        //    return View(model);
        //}

        //// POST: /Account/EditProfile
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userId = HttpContext.Session.GetInt32("UserId");

        //        if (userId == null)
        //        {
        //            return RedirectToAction("Login");
        //        }

        //        var user = await _context.Users.FindAsync(userId);

        //        if (user == null)
        //        {
        //            return RedirectToAction("Login");
        //        }

        //        // تحديث البيانات
        //        user.FirstName = model.FirstName;
        //        user.lastName = model.LastName;
        //        user.Phone = model.Phone;
        //        user.Address = model.Address;

        //        _context.Users.Update(user);
        //        await _context.SaveChangesAsync();

        //        // تحديث بيانات الجلسة
        //        HttpContext.Session.SetString("UserFullName", $"{user.FirstName} {user.lastName}");

        //        TempData["SuccessMessage"] = "تم تحديث الملف الشخصي بنجاح";
        //        return RedirectToAction("Profile");
        //    }

        //    return View(model);
        //}

        //// GET: /Account/ChangePassword
        //[HttpGet]
        //public IActionResult ChangePassword()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    return View();
        //}

        //// POST: /Account/ChangePassword
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userId = HttpContext.Session.GetInt32("UserId");

        //        if (userId == null)
        //        {
        //            return RedirectToAction("Login");
        //        }

        //        var user = await _context.Users.FindAsync(userId);

        //        if (user == null)
        //        {
        //            return RedirectToAction("Login");
        //        }

        //        // التحقق من كلمة المرور القديمة
        //        if (!PasswordHasher.VerifyPassword(model.OldPassword, user.Password))
        //        {
        //            ModelState.AddModelError("OldPassword", "كلمة المرور القديمة غير صحيحة");
        //            return View(model);
        //        }

        //        // تحديث كلمة المرور
        //        user.Password = PasswordHasher.HashPassword(model.NewPassword);

        //        _context.Users.Update(user);
        //        await _context.SaveChangesAsync();

        //        TempData["SuccessMessage"] = "تم تغيير كلمة المرور بنجاح";
        //        return RedirectToAction("Profile");
        //    }

        //    return View(model);
        //}

        // دالة مساعدة لتسجيل الدخول
        private async Task LoginUser(User user)
        {
            // حفظ بيانات المستخدم في الجلسة
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserFullName", $"{user.FirstName} {user.lastName}");
            HttpContext.Session.SetString("UserRole", user.Role);

            // إنشاء Claims Identity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.lastName}"),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CustomCookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("Cookies", claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // تذكرني لمدة 7 أيام
            });
        }
    }
}