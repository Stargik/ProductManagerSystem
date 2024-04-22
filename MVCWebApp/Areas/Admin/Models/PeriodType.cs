using System;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Areas.Admin.Models
{
	public enum PeriodType
	{
        [Display(Name = "Щоденно")]
        Daily,
        [Display(Name = "Щотижнево")]
        Weekly,
        [Display(Name = "Щомісячно")]
        Monthly,
        [Display(Name = "Щорічно")]
        Yearly
    }
}

