using System;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Models;

namespace MVCWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IManufacturerService manufacturerService;
        private readonly IImageService imageService;

        public ProductsController(IProductService productService, IManufacturerService manufacturerService, IImageService imageService)
        {
            this.productService = productService;
            this.manufacturerService = manufacturerService;
            this.imageService = imageService;
        }
        // GET: Products
        public async Task<ActionResult> Index(int? searchCategoryId, int? searchManufacturerId, string? searchTitle)
        {
            var categories = (await productService.GetAllCategoriesAsync()).ToList();
            var manufacturers = (await manufacturerService.GetAllAsync()).ToList();

            categories.Insert(0, new Category { Id = -1, Title = "Всі категорії" });
            manufacturers.Insert(0, new Manufacturer { Id = -1, Name = "Всі бренди" });

            ViewData["ImageStoragePath"] = await imageService.GetStoragePath();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Title");
            ViewData["ManufacturerId"] = new SelectList(manufacturers, "Id", "Name");

            FilterSearchModel filterSearchModel = new FilterSearchModel
            {
                CategoryId = searchCategoryId == -1 ? null : searchCategoryId,
                ManufacturerId = searchManufacturerId == -1 ? null : searchManufacturerId,
                SearchTitle = searchTitle
            };

            var products = await productService.GetByFilterAsync(filterSearchModel);

            var productsView = new ProductsViewModel
            {
                Products = products,
                SearchCategoryId = searchCategoryId,
                SearchManufacturerId = searchManufacturerId,
                SearchTitle = searchTitle
            };

            return View(productsView);
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var product = await productService.GetByIdAsync((int)id);

            if (product is null)
            {
                return NotFound();
            }

            ViewData["ImageStoragePath"] = await imageService.GetStoragePath();
            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await productService.GetAllCategoriesAsync();
            var manufacturers = await manufacturerService.GetAllAsync();
            var currencyTypes = await productService.GetAllCurrencyTypesAsync();
            var stockStatuses = await productService.GetAllStockStatusesAsync();

            ViewData["CategoryId"] = new SelectList(categories, "Id", "Title");
            ViewData["ManufacturerId"] = new SelectList(manufacturers, "Id", "Name");
            ViewData["CurrencyTypeId"] = new SelectList(currencyTypes, "Id", "Name");
            ViewData["StockStatusId"] = new SelectList(stockStatuses, "Id", "Name");

            if (!categories.Any())
            {
                TempData["ErrorMessage"] = "Необхідна попередня наявність хоча б однієї категорії.";
                return RedirectToAction(nameof(Index));
            }

            if (!manufacturers.Any())
            {
                TempData["ErrorMessage"] = "Необхідна попередня наявність хоча б одного виробника.";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,ManufacturerCode,StockStatusId,CurrencyTypeId,CategoryId,ManufacturerId,MainImage")] ProductDTO productDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await productService.AddAsync(productDTO);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                TempData["ErrorMessage"] = "Помилка. Зображення з такою назвою вже існує.";
            }

            var categories = await productService.GetAllCategoriesAsync();
            var manufacturers = await manufacturerService.GetAllAsync();
            var currencyTypes = await productService.GetAllCurrencyTypesAsync();
            var stockStatuses = await productService.GetAllStockStatusesAsync();

            ViewData["CategoryId"] = new SelectList(categories, "Id", "Title");
            ViewData["ManufacturerId"] = new SelectList(manufacturers, "Id", "Name");
            ViewData["CurrencyTypeId"] = new SelectList(currencyTypes, "Id", "Name");
            ViewData["StockStatusId"] = new SelectList(stockStatuses, "Id", "Name");

            return View(productDTO);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var categories = await productService.GetAllCategoriesAsync();
            var manufacturers = await manufacturerService.GetAllAsync();
            var currencyTypes = await productService.GetAllCurrencyTypesAsync();
            var stockStatuses = await productService.GetAllStockStatusesAsync();
            var characteristics = await productService.GetAllCharacteristicByProductIdAsync((int)id);

            ViewData["CategoryId"] = new SelectList(categories, "Id", "Title");
            ViewData["ManufacturerId"] = new SelectList(manufacturers, "Id", "Name");
            ViewData["CurrencyTypeId"] = new SelectList(currencyTypes, "Id", "Name");
            ViewData["StockStatusId"] = new SelectList(stockStatuses, "Id", "Name");
            ViewData["Characteristics"] = characteristics;

            var productDTO = await productService.GetProductDTOByIdAsync((int)id);

            if (productDTO is null)
            {
                return NotFound();
            }
            return View(productDTO);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,ManufacturerCode,StockStatusId,CurrencyTypeId,CategoryId,ManufacturerId,MainImage")] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    await productService.UpdateAsync(productDTO);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                TempData["ErrorMessage"] = "Помилка. Зображення з такою назвою вже існує.";
            }

            return View(productDTO);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var product = await productService.GetByIdAsync((int)id);

            if (product is null)
            {
                return NotFound();
            }

            ViewData["ImageStoragePath"] = await imageService.GetStoragePath();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var product = await productService.GetByIdAsync((int)id);

            if (product is not null)
            {
                await productService.DeleteByIdAsync((int)id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}