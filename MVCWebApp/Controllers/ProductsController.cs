using System;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVCWebApp.Configuration;
using MVCWebApp.Models;
using X.PagedList;

namespace MVCWebApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IManufacturerService manufacturerService;
        private readonly IImageService imageService;
        private readonly PaginationSettings paginationSettings;
        private readonly IDataPortServiceFactory<Product> productDataServiceFactory;

        public ProductsController(IProductService productService, IManufacturerService manufacturerService, IImageService imageService, IOptions<PaginationSettings> paginationSettings, IDataPortServiceFactory<Product> productDataServiceFactory)
        {
            this.productService = productService;
            this.manufacturerService = manufacturerService;
            this.imageService = imageService;
            this.paginationSettings = paginationSettings.Value;
            this.productDataServiceFactory = productDataServiceFactory;
        }
        // GET: Products
        public async Task<ActionResult> Index(int? categoryId, string? name, int? searchCategoryId, int? searchManufacturerId, string? searchTitle, int? pageNumber, int? pageSize, ProductSortState sortOrder = ProductSortState.Default)
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

            var products = await productService.GetByFilterAsync(filterSearchModel, sortOrder);

            ViewBag.CategoryName = "Всі товари";

            if (categoryId is not null)
            {
                ViewBag.CategoryId = categoryId;
                ViewBag.CategoryName = name;
                products = await productService.GetByFilterAsync(new FilterSearchModel { CategoryId = categoryId });
            }

            pageSize = (pageSize ?? paginationSettings.PageSize);
            pageNumber = (pageNumber ?? paginationSettings.PageNumber);
            var productsPaged = products.ToPagedList((int)pageNumber, (int)pageSize);

            var productsView = new ProductsViewModel
            {
                Products = productsPaged,
                SearchCategoryId = searchCategoryId,
                SearchManufacturerId = searchManufacturerId,
                SearchTitle = searchTitle,
                SortProductsViewModel = new SortProductsViewModel(sortOrder)
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

        public async Task<IActionResult> Export()
        {
            var categories = (await productService.GetAllCategoriesAsync()).ToList();
            var manufacturers = (await manufacturerService.GetAllAsync()).ToList();

            categories.Insert(0, new Category { Id = -1, Title = "Всі категорії" });
            manufacturers.Insert(0, new Manufacturer { Id = -1, Name = "Всі бренди" });

            ViewData["CategoryId"] = new SelectList(categories, "Id", "Title");
            ViewData["ManufacturerId"] = new SelectList(manufacturers, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ExportXml(List<int> categoryIds, List<int> manufacturerIds, string contentType = "application/xml", PortType portType = PortType.Default)
        {
            var exportService = productDataServiceFactory.GetExportService(contentType);

            categoryIds.Remove(-1);
            manufacturerIds.Remove(-1);

            using (var stream = new MemoryStream())
            {
                var products = (await productService.GetAllAsync()).Where(p => categoryIds.Contains(p.CategoryId) && manufacturerIds.Contains(p.ManufacturerId));

                await exportService.WriteToAsync(stream, products, portType);

                await stream.FlushAsync();

                return new FileContentResult(stream.ToArray(), contentType)
                {
                    FileDownloadName = $"Catalog_{DateTime.UtcNow.ToString("MMddyyyy")}.xml"
                };
            }
        }
    }
}
