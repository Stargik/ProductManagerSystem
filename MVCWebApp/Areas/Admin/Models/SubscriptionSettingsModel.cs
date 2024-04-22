using System;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Areas.Admin.Models
{
	public class SubscriptionSettingsModel
	{
        [Display(Name = "Дата початку")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Інтервал надсилання")]
        public PeriodType PeriodType { get; set; }
    }
}

