using ProductList.Extensions;
using ProductList.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ProductList.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var _DBUser = await _userManager.FindByIdAsync(id);
                if (_DBUser == null)
                {
                    this.ShowMessage("User not found.", true);
                    return RedirectToAction("Index", "Home");
                }
                var _VMUser = new RegisterUserViewModel
                {
                    Id = _DBUser.Id,
                    UserName = _DBUser.UserName,
                    Email = _DBUser.Email,
                    PhoneNumber = _DBUser.PhoneNumber
                };
                return View(_VMUser);
            }
            return View(new RegisterUserViewModel());
        }

        private bool EntityExists(string id)
        {
            return (_userManager.Users.AsNoTracking().Any(u => u.Id == id));
        }

        private static void MapRegisterUserViewModel(RegisterUserViewModel entityOrigin, IdentityUser entityDestination)
        {
            entityDestination.UserName = entityOrigin.UserName;
            entityDestination.NormalizedUserName = entityOrigin.UserName.ToUpper().Trim();
            entityDestination.Email = entityOrigin.Email;
            entityDestination.NormalizedEmail = entityOrigin.Email.ToUpper().Trim();
            entityDestination.PhoneNumber = entityOrigin.PhoneNumber;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserViewModel _VMUser)
        {
            ModelState.Remove("Id");

            if (!string.IsNullOrEmpty(_VMUser.Id))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfPassword");
            }

            if (ModelState.IsValid)
            {
                if (EntityExists(_VMUser.Id))
                {
                    var _DBUser = await _userManager.FindByIdAsync(_VMUser.Id);
                    if ((_VMUser.Email != _DBUser.Email) &&
                        (_userManager.Users.Any(u => u.NormalizedEmail == _VMUser.Email.ToUpper().Trim())))
                    {
                        ModelState.AddModelError("Email", "There is already a registered user with this email.");
                        return View(_VMUser);
                    }
                    MapRegisterUserViewModel(_VMUser, _DBUser);

                    var resultado = await _userManager.UpdateAsync(_DBUser);
                    if (resultado.Succeeded)
                    {
                        this.ShowMessage("User changed successfully.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        this.ShowMessage("Unable to change user.", true);
                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(_VMUser);
                    }
                }
                else
                {
                    var _DBUser = await _userManager.FindByEmailAsync(_VMUser.Email);
                    if (_DBUser != null)
                    {
                        ModelState.AddModelError("Email", "There is already a registered user with this email.");
                        return View(_DBUser);
                    }

                    _DBUser = new IdentityUser();
                    MapRegisterUserViewModel(_VMUser, _DBUser);

                    var resultado = await _userManager.CreateAsync(_DBUser, _VMUser.Password);
                    if (resultado.Succeeded)
                    {
                        this.ShowMessage("User registered successfully. Use your credentials to log in to the system.");
                        if (User.Identity.IsAuthenticated)
                        {
                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        this.ShowMessage("Error registering user.", true);
                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(_VMUser);
                    }
                }
            }
            else
            {
                return View(_VMUser);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel login)
        {
            ModelState.Remove("ReturnUrl");

            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(login.User, login.Password, login.Remember, false);
                if (resultado.Succeeded)
                {
                    login.ReturnUrl = login.ReturnUrl ?? "~/";
                    return LocalRedirect(login.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Review your access details and try again.");
                    return View(login);
                }
            }
            else
            {
                return View(login);
            }
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.AsNoTracking().ToListAsync();
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                this.ShowMessage("User not informed.", true);
                return RedirectToAction(nameof(Index));
            }

            if (!EntityExists(id))
            {
                this.ShowMessage("User not found.", true);
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var resultado = await _userManager.DeleteAsync(user);
                if (resultado.Succeeded)
                {
                    this.ShowMessage("User deleted successfully.");
                }
                else
                {
                    this.ShowMessage("Unable to delete user.", true);
                }
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.ShowMessage("User not found.", true);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
