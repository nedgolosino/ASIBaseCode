using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        { 
            _productService = productService; 
            _logger = logger;
        }

        private string GetLoggedInUserId()
        {
            return HttpContext.User.Identity.Name;
        }

        public IActionResult Index()
        {
            try
            {
                string userID = GetLoggedInUserId();
                var products = _productService.GetAllProducts().Where(e => e.UserName == userID).ToList();

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load products.");
                return View("Error", new ErrorViewModel { ErrorMessage = "Failed to load products." });
            }
        }

        public IActionResult ProductTable(string searchProductId = "")
        {
            try
            {
                string userId = GetLoggedInUserId();
                var products = _productService.GetAllProducts()
                                              .Where(e => e.UserName == userId)
                                              .ToList();


                if (!string.IsNullOrEmpty(searchProductId) && int.TryParse(searchProductId, out int productId))
                {
                    products = products.Where(e => e.ProductId == productId).ToList();
                }

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load product table.");
                return View("Error", new ErrorViewModel { ErrorMessage = "Failed to load product table." });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    model.UserName = GetLoggedInUserId();
                    _productService.AddProduct(model);
                    TempData["SuccessMessage"] = "Product added successfully!";
                    return RedirectToAction(nameof(ProductTable));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add new product.");
                    TempData["ErrorMessage"] = "Failed to add new product. Please try again.";
                }
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (!HasAccessToProduct(product))
            {
                TempData["ErrorMessage"] = "Product not found or access denied.";
                return RedirectToAction(nameof(ProductTable));
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!HasAccessToProduct(model))
                    {
                        TempData["ErrorMessage"] = "Access denied.";
                        return RedirectToAction(nameof(ProductTable));
                    }

                    _productService.UpdateProduct(model);
                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction(nameof(ProductTable));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update product.");
                    TempData["ErrorMessage"] = "Failed to update product. Please try again.";
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                if (!HasAccessToProduct(product))
                {
                    TempData["ErrorMessage"] = "Product not found or access denied.";
                    return RedirectToAction(nameof(ProductTable));
                }

                _productService.DeleteProduct(id);
                TempData["SuccessMessage"] = "Product deleted successfully!";
                return RedirectToAction(nameof(ProductTable));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product.");
                TempData["ErrorMessage"] = "Failed to delete product. Please try again.";
                return RedirectToAction(nameof(ProductTable));
            }
        }


        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (!HasAccessToProduct(product))
            {
                TempData["ErrorMessage"] = "Product not found or access denied.";
                return RedirectToAction(nameof(ProductTable));
            }
            return View(product);
        }

        private bool HasAccessToProduct(Product product)
        {
            return product != null && product.UserName == GetLoggedInUserId();
        }
    }
}
