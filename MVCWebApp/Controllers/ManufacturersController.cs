using System;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVCWebApp.Controllers
{
    [Authorize]
    public class ManufacturersController : Controller
    {
        private readonly IManufacturerService manufacturerService;
        private readonly IProductService productService;


        public ManufacturersController(IManufacturerService manufacturerService, IProductService productService)
        {
            this.manufacturerService = manufacturerService;
            this.productService = productService;
        }
        // GET: Manufacturers
        public async Task<ActionResult> Index()
        {
            var manufacturers = await manufacturerService.GetAllAsync();
            return View(manufacturers);
        }

        // GET: Manufacturers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var manufacturer = await manufacturerService.GetByIdAsync((int)id);

            if (manufacturer is null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }
    }
}
