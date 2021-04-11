using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.Auth
{
    [Authorize(Roles = "cashier, manager")]
    public class EditPhoneNumberModel : PageModel
    {

        public EmployeeUser UserInfo { get; set; }

        [BindProperty]
        [MaxLength(13)]
        public string NewPhoneNumber { get; set; }

        private readonly UserManager<EmployeeUser> _userManager;
        private readonly SignInManager<EmployeeUser> _signInManager;

        public EditPhoneNumberModel(UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UserInfo = await _userManager.FindByNameAsync(User.Identity.Name);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                UserInfo = await _userManager.FindByNameAsync(User.Identity.Name);
                return Page();
            }
            else
            {
                UserInfo = await _userManager.FindByNameAsync(User.Identity.Name);
                var result = await _userManager.SetUserNameAsync(UserInfo, NewPhoneNumber);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    return Redirect("/Account/Login");
                }
                else
                {
                    ModelState.AddModelError("", "Can not change phone number");
                    return Page();
                }
            }

        }
    }
}
