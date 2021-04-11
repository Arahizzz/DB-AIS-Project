using System;
using System.Collections.Generic;
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
    public class UserInfoModel : PageModel
    {

        public EmployeeUser UserInfo { get; set; }

        private readonly UserManager<EmployeeUser> _userManager;
        private readonly SignInManager<EmployeeUser> _signInManager;
        public UserInfoModel(UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UserInfo = await _userManager.FindByNameAsync(User.Identity.Name);
            return Page();
        }
    }
}
