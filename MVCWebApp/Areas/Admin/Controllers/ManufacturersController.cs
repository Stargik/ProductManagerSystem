using System;
using System.Data;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVCWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
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

        // GET: Manufacturers/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                await manufacturerService.AddAsync(manufacturer);
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }

        // GET: ManufacturersController/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: ManufacturersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Manufacturer manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await manufacturerService.UpdateAsync(manufacturer);
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }

        // GET: ManufacturersController/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ManufacturersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var manufacturer = await manufacturerService.GetByIdAsync((int)id);

            var productsByManufacturer = (await productService.GetAllAsync()).Where(p => p.ManufacturerId == id);

            if (productsByManufacturer.Any())
            {
                TempData["ErrorMessage"] = "Помилка. Неможливо видалити виробника, товари якого існують.";
                return RedirectToAction(nameof(Index));
            }
            if (manufacturer is not null)
            {
                await manufacturerService.DeleteByIdAsync((int)id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
