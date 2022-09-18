using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System;
using App.Domain;
using App.DTO;

namespace Recon.Presentation.Controllers
{
    public class AccountController : Controller
    {
        //Identity provider for login manager
        //private readonly UserManager<ApplicationUser> _userManager;
        ////Identity provider for login manager
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(//UserManager<ApplicationUser> userManager,
        //                      SignInManager<ApplicationUser> signInManager,
                               RoleManager<IdentityRole> roleManager)
        {
            //_roleManager = roleManager;
            //_userManager = userManager;
            //_signInManager = signInManager;
        }

        /// <summary>This function is used to open theview for registering users
        /// </summary>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>This function is used to register the user
        /// <param><c>user</c> is the new user to be registered.</param>
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel user)
        {
            //if (ModelState.IsValid)
            //{
            //    //Set the user details of the user that is to be created
            //    var userToBeCreated = new ApplicationUser
            //    {
            //        Name=user.Name,
            //        UserName = user.Email,
            //        Email = user.Email,      
            //    };
            //    //Create the user
            //    var result = await _userManager.CreateAsync(userToBeCreated, user.Password);
            //    if (!result.Succeeded && result.Errors.Select(x => x.Code).Contains("DuplicateName"))
            //    {
            //        ViewBag.Message = "User aleady exists";
            //        return View();
            //    }
            //    if (result.Succeeded)
            //    {
            //        return RedirectToAction("Login", "Account");
            //    }
      
            //}
          
                return View(user); 
            

        }

        /// <summary>This function is used to return the view for login
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>This function is used to identify the user
        /// <param><c>userLogin</c> is the new user to be identified.</param>
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userLogin)
        {
            //if (ModelState.IsValid)
            //{
            //    var user = await _userManager.FindByEmailAsync(userLogin.Email);

            //    if (!await _userManager.CheckPasswordAsync(user, userLogin.Password))
            //    {
            //        ViewBag.InvalidLogin = "Login invalid";
            //        return View(userLogin);
            //    }

            //    var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, userLogin.RememberMe, true);
            //    if (result.Succeeded)
            //    {

            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        ViewBag.InvalidLogin = "Login invalid";
            //        return View(userLogin);
            //    }
            //}
            //else
            //{
                return View(userLogin);
            //}
        }
        /// <summary>
        /// Get the local ip adresse of the connected user
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
        /// <summary>This function is used to logout the current user
        /// </summary>
        public async Task<IActionResult> Logout()
        {
            //allow user to logout 
            //await _signInManager.SignOutAsync();
            return Redirect("Login");
        }

        /// <summary>This function is used to Tell the user he doesn't have access to page
        /// </summary>
        public async Task<IActionResult> AccessDenied()
        {
            if (User.Identity.IsAuthenticated)
            {
                //allow user to logout 
               // await _signInManager.SignOutAsync();
            }
            return Redirect("Login");
        }
    }
}
