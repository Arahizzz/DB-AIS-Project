using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.Auth
{
    public class SignUpPageModel : PageModel
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public int Surname { get; set; }

        [Required]
        [Display(Name = "Name")]
        public int Name { get; set; }

        [Required]
        [Display(Name = "Patronym")]
        public int Patronym { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "Salary")]
        public int Salary { get; set; }

        [Required]
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


        public void OnGet()
        {
        }
    }
}
