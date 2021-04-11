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
    public class UserInfoUpdateModel : PageModel
    {

        public EmployeeUser UserInfo { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Surname { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Name { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Patronym { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string City { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Street { get; set; }

        [BindProperty]
        [MaxLength(9)]
        public string ZipCode { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateOfStart { get; set; }

        private readonly UserManager<EmployeeUser> _userManager;
        private readonly SignInManager<EmployeeUser> _signInManager;

        private readonly EmployeeRepository _employeeRepository;
        public UserInfoUpdateModel(UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager, EmployeeRepository employeeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeRepository = employeeRepository;
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
                var newEmployee = new Models.Employee
                {
                    Id = UserInfo.Id,
                    Surname = Surname,
                    Patronymic = Patronym,
                    Name = Name,
                    PhoneNumber = UserInfo.PhoneNumber,
                    City = City,
                    Street = Street,
                    Zip = ZipCode,
                    DateOfBirth = DateOfBirth,
                    DateOfStart = DateOfStart,
                    Role = UserInfo.Role,
                    Salary = UserInfo.Salary
                };
                await _employeeRepository.EditEmployee(newEmployee);
                return Redirect("/userinfo");
            }
        }
    }
}
