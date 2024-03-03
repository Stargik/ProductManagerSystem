using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
	public enum PortType
	{
        [Display(Name = "За замовчуванням")]
        Default,
        [Display(Name = "Для Rozetka")]
        RozetkaXml
    }
}

