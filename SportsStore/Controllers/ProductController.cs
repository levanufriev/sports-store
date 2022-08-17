using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsStore.Data;
using SportsStore.Data.Services;
using SportsStore.Data.ViewModels;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var allProducts = await productService.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allProducts.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                //var filteredResultNew = allProducts.Where(n => string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Description, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allProducts);
        }

        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
            var products = await productService.GetAllAsync();
            var result = products.Where(c => categoryId == c.CategoryId).ToList();

            return result is null ? View("NotFound") : View("Index", result);
        }

        public async Task<IActionResult> FilterByBrand(int brandId)
        {
            var products = await productService.GetAllAsync();
            var result = products.Where(c => brandId == c.SupplierId).ToList();

            return result is null ? View("NotFound") : View("Index", result);
        }

        public async Task<IActionResult> Create()
        {
            var productDropdownsData = await productService.GetNewProductDropdownsValues();

            ViewBag.Suppliers = new SelectList(productDropdownsData.Suppliers, "Id", "Name");
            ViewBag.Categories = new SelectList(productDropdownsData.Categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewProductVM product)
        {
            if (!ModelState.IsValid)
            {
                var productDropdownsData = await productService.GetNewProductDropdownsValues();

                ViewBag.Suppliers = new SelectList(productDropdownsData.Suppliers, "Id", "Name");
                ViewBag.Categories = new SelectList(productDropdownsData.Categories, "Id", "Name");

                return View(product);
            }

            await productService.AddNewProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var productDetail = await productService.GetProductByIdAsync(id);
            ViewBag.Image = productDetail.ImageUrl;
            return View(productDetail);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productDetails = await productService.GetProductByIdAsync(id);
            if (productDetails == null) return View("NotFound");

            var response = new NewProductVM()
            {
                Id = productDetails.Id,
                Name = productDetails.Name,
                Description = productDetails.Description,
                Price = productDetails.Price,
                ImageUrl = productDetails.ImageUrl,
                CategoryId = productDetails.CategoryId,
                SupplierId = productDetails.SupplierId,
            };

            var productDropdownsData = await productService.GetNewProductDropdownsValues();
            ViewBag.Categories = new SelectList(productDropdownsData.Categories, "Id", "Name");
            ViewBag.Suppliers = new SelectList(productDropdownsData.Suppliers, "Id", "Name");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewProductVM product)
        {
            if (id != product.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var productDropdownsData = await productService.GetNewProductDropdownsValues();

                ViewBag.Categories = new SelectList(productDropdownsData.Categories, "Id", "Name");
                ViewBag.Suppliers = new SelectList(productDropdownsData.Suppliers, "Id", "Name");

                return View(product);
            }

            await productService.UpdateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productDetails = await productService.GetByIdAsync(id);
            if (productDetails == null) return View("NotFound");
            return View(productDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productDetails = await productService.GetByIdAsync(id);
            if (productDetails == null) return View("NotFound");

            await productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

