using System;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVCWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        // GET: Manufacturers/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Category category)
        {
            if (ModelState.IsValid)
            {
                await productService.AddCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Manufacturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Manufacturers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await productService.UpdateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Manufacturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var category = await productService.GetCategoryByIdAsync((int)id);

            var productsByCategory = (await productService.GetAllAsync()).Where(p => p.CategoryId == id);

            if (productsByCategory.Any())
            {
                TempData["ErrorMessage"] = "Помилка. Неможливо видалити категорію, товари якої існують.";
                return RedirectToAction(nameof(Index));
            }
            if (category is not null)
            {
                await productService.RemoveCategoryByIdAsync((int)id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
