using Exam.Business.Helpers.Account;
using Exam.Core.Models;
using Exam.DTOs.AccountDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    public class AccountController : Controller
    {
       private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
               await _roleManager.CreateAsync( new IdentityRole() { 
                    Name = item.ToString()
               });
            }
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await  _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);
                if(user == null)
                {
                    ModelState.AddModelError("", "UserNameOrEmail ve ya Password duzgun deyil");
                    return View();
                }
            }
            var res = await  _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            await _signInManager.SignInAsync(user, loginDto.IsRemembered);
            if (res.IsLockedOut)
            {
                ModelState.AddModelError("", "birazdan cehd edin");
                return View();
            }
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", "UserNameOrEmail ve ya Password duzgun deyil");
                return View();
            }
            return RedirectToAction("Index", "Member", new { Area = "Admin" });
            
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User user = new User()
            {
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };
            var res = await _userManager.CreateAsync(user, registerDto.Password);
            await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            if (!res.Succeeded)
            {
                foreach(var item in res.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            return RedirectToAction(nameof(Login));
        }
    }
}
