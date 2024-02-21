using System;
using System.Data;
using System.Net.Mail;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVCWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
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

        // GET: Characteristics/Create
        public async Task<IActionResult> Create(int? productid)
        {
            if (productid is null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = productid;
            return View();
        }

        // POST: Characteristics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Name,ValueNumber,UnitType")] Characteristic characteristic)
        {
            if (ModelState.IsValid)
            {
                await productService.AddCharacteristicAsync(characteristic);
                return RedirectToAction(nameof(Edit), "Products", new { Id = characteristic.ProductId });
            }
            return View(characteristic);
        }

        // GET: Characteristics/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Characteristics/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Name,ValueNumber,UnitType")] Characteristic characteristic)
        {
            if (id != characteristic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await productService.UpdateCharacteristicAsync(characteristic);
                return RedirectToAction(nameof(Edit), "Products", new { Id = characteristic.ProductId });
            }
            return View(characteristic);
        }

        // GET: Characteristics/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Characteristics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var characteristic = await productService.GetCharacteristicByIdAsync((int)id);

            if (characteristic is not null)
            {
                await productService.RemoveCharacteristicByIdAsync((int)id);
            }
            return RedirectToAction(nameof(Edit), "Products", new { Id = characteristic?.ProductId });
        }
    }
}
