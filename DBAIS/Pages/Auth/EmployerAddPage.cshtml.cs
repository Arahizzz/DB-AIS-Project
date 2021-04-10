using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.Auth
{
    public class EmployerAddModel : PageModel
    {
        //
        [BindProperty]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [BindProperty]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [BindProperty]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [BindProperty]
        [Display(Name = "Patronym")]
        public string Patronym { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [BindProperty]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }

        [BindProperty]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [BindProperty]
        [Display(Name = "Salary")]
        public int Salary { get; set; }

        [BindProperty]
        [Display(Name = "Telephone")]
        public string Telephone { get; set; }

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
        public DateTime DateOfBirth { get; set; }

        [BindProperty]
        public DateTime DateOfStart { get; set; }
        //

        private readonly UserManager<EmployeeUser> _userManager;
        private readonly SignInManager<EmployeeUser> _signInManager;
        public EmployerAddModel(UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            /*var newEmployer = new Models.EmployeeUser
            {
                Id = "id100",
                Surname = "Test",
                Patronymic = "Test",
                Name = "Test",
                PhoneNumber = "2134123",
                City = "Test",
                Street = "Test",
                Zip = "Test",
                DateOfBirth = DateTime.Now,
                DateOfStart = DateTime.Now,
                Role = "cashier",
                Salary = 1000
            };

            var result = await _userManager.CreateAsync(newEmployer, "password");*/
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var newEmployer = new Models.EmployeeUser
                {
                    Id = Id,
                    Surname = Surname,
                    Patronymic = Patronym,
                    Name = Name,
                    PhoneNumber = Telephone,
                    City = City,
                    Street = Street,
                    Zip = ZipCode,
                    DateOfBirth = DateOfBirth,
                    DateOfStart = DateOfStart,
                    Role = Role,
                    Salary = Salary
                };

                var result = await _userManager.CreateAsync(newEmployer, Password);
                if (result.Succeeded)
                {
                    // set cookies
                    //await _signInManager.SignInAsync(user, false);
                    return Redirect("/");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return Page();
            }
        }
    }
}
