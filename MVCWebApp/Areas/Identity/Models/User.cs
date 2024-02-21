using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MVCWebApp.Areas.Identity.Models
{
	public class User : IdentityUser
	{
		[Display(Name = "Імʼя")]
		public string? Name { get; set; }
	}
}

