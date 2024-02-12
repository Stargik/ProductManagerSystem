using System;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVCWebApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IProductService productService;


        public CategoriesController(IProductService productService)
        {
            this.productService = productService;
        }
        // GET: Manufacturers
        public async Task<ActionResult> Index()
        {
            var categories = await productService.GetAllCategoriesAsync();
            return View(categories);
        }

        // GET: Manufacturers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var category = await productService.GetCategoryByIdAsync((int)id);

            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}
