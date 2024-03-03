using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace MVCWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CurrencyTypesController : Controller
    {
        private readonly ICurrencyTypeService currencyTypeService;
        private readonly IProductService productService;


        public CurrencyTypesController(ICurrencyTypeService currencyTypeService, IProductService productService)
        {
            this.currencyTypeService = currencyTypeService;
            this.productService = productService;
        }
        // GET: CurrencyTypes
        public async Task<ActionResult> Index()
        {
            var currencyTypes = await currencyTypeService.GetAllAsync();
            return View(currencyTypes);
        }

        // GET: CurrencyTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var currencyType = await currencyTypeService.GetByIdAsync((int)id);

            if (currencyType is null)
            {
                return NotFound();
            }

            return View(currencyType);
        }

        // GET: CurrencyTypes/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: CurrencyTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Rate")] CurrencyType currencyType)
        {
            if (ModelState.IsValid)
            {
                await currencyTypeService.AddAsync(currencyType);
                return RedirectToAction(nameof(Index));
            }
            return View(currencyType);
        }

        // GET: CurrencyTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var currencyType = await currencyTypeService.GetByIdAsync((int)id);
            if (currencyType is null)
            {
                return NotFound();
            }
            return View(currencyType);
        }

        // POST: CurrencyTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Rate")] CurrencyType currencyType)
        {
            if (id != currencyType.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await currencyTypeService.UpdateAsync(currencyType);
                return RedirectToAction(nameof(Index));
            }
            return View(currencyType);
        }

        // GET: CurrencyTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var currencyType = await currencyTypeService.GetByIdAsync((int)id);

            if (currencyType is null)
            {
                return NotFound();
            }

            return View(currencyType);
        }

        // POST: CurrencyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var currencyType = await currencyTypeService.GetByIdAsync((int)id);

            var productsByCurrencyType = (await productService.GetAllAsync()).Where(p => p.CurrencyTypeId == id);

            if (productsByCurrencyType.Any())
            {
                TempData["ErrorMessage"] = "Помилка. Неможливо видалити валюту, товари якої існують.";
                return RedirectToAction(nameof(Index));
            }
            if (currencyType is not null)
            {
                await currencyTypeService.DeleteByIdAsync((int)id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
