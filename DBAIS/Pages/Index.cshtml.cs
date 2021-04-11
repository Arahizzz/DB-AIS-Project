using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CheckRepository _customerRepository;
        private readonly SignInManager<EmployeeUser> _signInManager;
        private readonly UserManager<EmployeeUser> _userManager;

        public IndexModel(CheckRepository customerRepository, UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager)
        {
            _customerRepository = customerRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public List<PurchaseInfo> Purchases { get; set; } = null!;

        public async Task OnGetAsync()
        {
            Purchases = await _customerRepository.GetCustomerChecks("7121520341739");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
