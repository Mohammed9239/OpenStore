using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenStore.Data;
using OpenStore.Models;
using Type = OpenStore.Models.Type;

namespace OpenStore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;


        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "{0} يجب ان تكون اكثر من {2} رموز.", MinimumLength = 5)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            [Required]
            public int ProvinceID { get; set; }

            [Required]
            public string Streat { get; set; }

            [Required]
            public string Phone { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            await All();

            // To Get City 
            ViewData["City"] = new SelectList(_context.Cities, "ID", "Name");

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            await All();

            ViewData["City"] = new SelectList(_context.Cities, "ID", "Name",Input.ProvinceID);

            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FullName = Input.FullName,
                    UserName = Input.UserName,
                    Email = Input.Email,
                    ProvinceID = Input.ProvinceID,
                    Street = Input.Streat,
                    PhoneNumber = Input.Phone,
                    Date = DateTime.Now,
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        var AddToRole_result = await _userManager.AddToRoleAsync(user,"مشتري");
                        if (!AddToRole_result.Succeeded)
                        {
                            ModelState.AddModelError("", "حصل خطاء غير متوقع اثناء اضافة الصلاحية للمستخدم");
                            return Page();
                        }
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task All()
        {
            ViewData["CategoryLev1"] = await _context.CategoryLev1s.ToListAsync();
            ViewData["CartCount"] = "";
            ViewData["NotificationCount"] = "";
            if (_signInManager.IsSignedIn(User))
            {
                User user = await _userManager.GetUserAsync(User);
                ViewData["CartCount"] = _context.OrderProducts.Where(i => i.Order.UserID == user.Id && i.Order.State == State.incart).ToListAsync().Result.Count;
                ViewData["NotificationCount"] = _context.Notifications.Where(i => i.Type == Type.اشعار).ToListAsync().Result.Count;
            }
        }

    }
}
