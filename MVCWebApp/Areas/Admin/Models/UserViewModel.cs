using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Areas.Identity.Models;

namespace MVCWebApp.Areas.Admin.Models
{
	public class UserViewModel
	{
        public User User { get; set; }
        [Display(Name = "Ролі")]
        [BindProperty(Name = "roles")]
        public IEnumerable<string> UserRoles { get; set; } = new List<string>();
        [Display(Name = "Всі ролі")]
        public IEnumerable<IdentityRole> AllRoles { get; set; } = new List<IdentityRole>();
    }
}

