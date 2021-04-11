using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.Auth
{
     [Authorize(Roles = "cashier, manager")]
    public class EditPasswordModel : PageModel
    {

        public EmployeeUser UserInfo { get; set; }


        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        private readonly UserManager<EmployeeUser> _userManager;
        private readonly SignInManager<EmployeeUser> _signInManager;
        private readonly EmployeeUserRepository _employeeUserRepository;

        public EditPasswordModel(UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager, EmployeeUserRepository employeeUserRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeUserRepository = employeeUserRepository;
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
                var result = await _userManager.ChangePasswordAsync(UserInfo, CurrentPassword, NewPassword);
                if (result.Succeeded)
                {
                    return Redirect("/userinfo");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong current password");
                    return Page();
                }
            }
            
        }
    }
}
