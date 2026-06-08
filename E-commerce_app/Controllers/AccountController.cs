using E_commerce_app.Models;
using E_commerce_entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce_app.Controllers
{
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> _usermanager, SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _usermanager;
            signInManager = _signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            ApplicationUser user = await userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                //IdentityUserClaim<ApplicationUser> c
                //var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                bool isPasswordValid = await userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (isPasswordValid)
                {
                    ClaimsPrincipal cp = new ClaimsPrincipal();
                    ClaimsIdentity cd = new ClaimsIdentity("Cookies");
                    Claim idClaim = new Claim(ClaimTypes.NameIdentifier, user.Id);
                    Claim emailClaim = new Claim(ClaimTypes.Email, user.Email);
                    Claim nameClaim = new Claim(ClaimTypes.Name, user.UserName);
                    cd.AddClaim(emailClaim);
                    cd.AddClaim(nameClaim);
                    cd.AddClaim(idClaim);
                    var roles = await userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        Console.WriteLine(role);
                        cd.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                    cp.AddIdentity(cd);
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, cp);
                    return RedirectToAction("Index", "Product", new {pg = 1});
                }
                else
                {
                    ModelState.AddModelError("", "Wrong email or password");

                }

            }
            ModelState.AddModelError("", "Wrong email or password");

            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            ApplicationUser user = new ApplicationUser();
            user.FullName = registerViewModel.FullName;
            user.UserName = registerViewModel.Email;
            user.Email = registerViewModel.Email;
            user.PhoneNumber = registerViewModel.Phone;

            Address address = new Address();
            address.Country = registerViewModel.Country;
            address.City = registerViewModel.City;
            address.Street = registerViewModel.Street;
            address.Zip = registerViewModel.ZIP;
            address.IsDefault = true;

            user.Addresses.Add(address);

            var result = await userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "user");
                return RedirectToAction("Index", "Product", new {pg = 1});
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(registerViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "product", new { pg = 1 });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
