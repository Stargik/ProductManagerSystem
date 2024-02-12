using System;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Models;

namespace MVCWebApp.Controllers
{
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
        public async Task<ActionResult> Index(int? categoryId, string? name, int? searchCategoryId, int? searchManufacturerId, string? searchTitle)
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

            ViewBag.CategoryName = "Всі товари";

            if (categoryId is not null)
            {
                ViewBag.CategoryId = categoryId;
                ViewBag.CategoryName = name;
                products = await productService.GetByFilterAsync(new FilterSearchModel { CategoryId = categoryId });
            }

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
    }
}