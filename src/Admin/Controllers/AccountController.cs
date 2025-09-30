using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orion.Admin.Models.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Orion.Admin.Controllers
{
    // [Authorize]
    public class AccountController(
        UserManager<MasterUser> userManager,
        SignInManager<MasterUser> signInManager,
        IPasswordHasher<MasterUser> passwordHasher)
        : Controller
    {
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Register(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                MasterUser appUser = new MasterUser { UserName = user.UserName, Email = user.EmailAddress };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(user);
        }

        [AllowAnonymous]
        public IActionResult LogIn(string returnUrl)
        {
            LoginViewModel login = new LoginViewModel { ReturnUrl = returnUrl };

            return View(login);
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var appUser = await userManager.FindByNameAsync(login.UserName);

                if (appUser != null)
                {
                    SignInResult signInResult = await signInManager.PasswordSignInAsync(appUser.UserName, login.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        //return Redirect("login.ReturnUrl" ?? "/");
                        return Redirect("login.ReturnUrl" ?? "/Home");
                    }

                    ModelState.AddModelError("", "The login failed because of wrong credential information..!");
                }
            }
            return View(login);
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("LogIn");
        }

        public async Task<IActionResult> Edit()
        {
            MasterUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            MasterUserEditViewModel userEdit = new MasterUserEditViewModel(appUser);

            return View(userEdit);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MasterUserEditViewModel userEdit)
        {
            if (ModelState.IsValid)
            {
                MasterUser appUser = await userManager.FindByNameAsync(User.Identity?.Name);

                appUser.UserName = userEdit.UserName;
                if (appUser != null) appUser.PasswordHash = passwordHasher.HashPassword(appUser, userEdit.Password);

                if (appUser != null)
                {
                    IdentityResult ıdentityResult = await userManager.UpdateAsync(appUser);
                    if (ıdentityResult.Succeeded)
                    {
                        TempData["Success"] = "Your information has been edited..!";
                    }
                    else
                    {
                        TempData["Warning"] = "Your information has been wrong..!";
                    }
                }
            }

            return View(userEdit);


        }
    }

    public class MasterUser
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
