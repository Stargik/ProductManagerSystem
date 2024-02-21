using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVCWebApp.Areas.Admin.Models
{
	public class RegisterUserViewModel
	{
        [Display(Name = "Логін")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public string UserName { get; set; }

        [Display(Name = "Імʼя")]
        public string? Name { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Підтвердження паролю")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Ролі")]
        [BindProperty(Name = "roles")]
        public IEnumerable<string> UserRoles { get; set; } = new List<string>();
    }
}

