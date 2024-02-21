using System;
using System.Net.Mail;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVCWebApp.Controllers
{
    [Authorize]
    public class CharacteristicsController : Controller
    {
        private readonly IProductService productService;


        public CharacteristicsController(IProductService productService)
        {
            this.productService = productService;
        }
        // GET: Characteristics
        public async Task<ActionResult> Index(int? productId)
        {
            if (productId is null)
            {
                return NotFound();
            }
            ViewBag.ProductId = productId;
            ViewBag.ProductTitle = (await productService.GetByIdAsync((int)productId)).Title;
            var characteristics = productService.GetAllCharacteristicByProductIdAsync((int)productId);
            return View(await characteristics);
        }

        // GET: Characteristics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var characteristic = await productService.GetCharacteristicByIdAsync((int)id);
            if (characteristic is null)
            {
                return NotFound();
            }
            return View(characteristic);
        }
    }
}
