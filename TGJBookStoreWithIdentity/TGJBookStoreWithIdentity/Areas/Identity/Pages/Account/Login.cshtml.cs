// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TGJBookStoreWithIdentity.Models;
using System.Net.Mail;

namespace TGJBookStoreWithIdentity.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager; //add user manager to class to manage AppUser custom class
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<AppUser> signInManager, ILogger<LoginModel> logger, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager; //added to ctor
        }

        /// <summary>
        ///     
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///    
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///    
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     
            /// </summary>
            [Required]
            [Display(Name = "Email / Username")] //make login accept email or username //reference: https://codewithmukesh.com/blog/user-management-in-aspnet-core-mvc/
            public string Email { get; set; }

            /// <summary>
            ///     
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///    
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var userName = Input.Email;
                if (IsValidEmail(Input.Email)) //if the email is valid then get the username of that email 
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email); //get the User from the email
                    if (user != null)
                    {
                        userName = user.UserName; //set username variable
                    }
                }

                //pass userName as parameter here to signinmanager TClass
                var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

        //this function checks if user login input matches to an email
        public bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }



    }
}
