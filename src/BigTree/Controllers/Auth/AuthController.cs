using BigTree.Models;
using BigTree.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigTree.Controllers.Auth
{
    public class AuthController : Controller
    {

        private SignInManager<WorldUser> _signInManager;

        public AuthController(SignInManager<WorldUser> signInManager)
        {
            _signInManager = signInManager;
        }


       
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App"); //already logged in so redirect to trips
            }
            return View();

        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm, string returnURL)
        {
            if(ModelState.IsValid)
            {

                var signInResult = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, true, false);

                if(signInResult.Succeeded)
                {
                    if(string.IsNullOrWhiteSpace(returnURL))
                    {
                        return RedirectToAction("Trips", "App");

                    }
                    else
                    {
                        return Redirect(returnURL);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or password incorrect");
                }
                

            }
            return View();
        }


        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }
    }
}
