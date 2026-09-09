using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        // GET: SignUp
        [HttpGet]
        public IActionResult SignUp() => View();

        // POST: SignUp
        [HttpPost]
        public async Task<IActionResult> SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                // ADD THIS BLOCK HERE
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Username and password are required.");
                }
                // END OF ADDED BLOCK

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    CollegeName = model.CollegeName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin"); // assign role
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // GET: SignIn
        [HttpGet]
        public IActionResult SignIn() => View();

        // POST: SignIn
        [HttpPost]
        public async Task<IActionResult> SignIn(UserModel model)
        {
            if (ModelState.IsValid)
            {
                // ADD THIS BLOCK HERE
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Username and password are required.");
                }
                // END OF ADDED BLOCK

                var result = await _signInManager.PasswordSignInAsync(
                    model.Username, model.Password, isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        // SignOut
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Temporary: create default roles (visit /User/CreateRole once)
        public async Task<IActionResult> CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));
            if (!await _roleManager.RoleExistsAsync("Staff"))
                await _roleManager.CreateAsync(new IdentityRole("Staff"));
            return Content("Roles created successfully.");
        }
    }
}