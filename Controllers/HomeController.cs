using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsCategories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ProductsCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext { get; set; }

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("products")]
        public IActionResult Products()
        {
            ViewBag.AllProducts = dbContext.Products.OrderBy(p => p.Name).ToList();
            return View();
        }

        [HttpPost("products/create")]
        public IActionResult ProductsCreate(Product newP)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newP);
                dbContext.SaveChanges();
                return RedirectToAction("Products");
            }
            else
            {
                ViewBag.AllProducts = dbContext.Products.OrderBy(p => p.Name).ToList();
                return View("Products");
            }
        }

        [HttpGet("product/{ProductId}")]
        public IActionResult ProductShow(int ProductId)
        {
            Product product = dbContext.Products
                                        .Include(p => p.CategoriesOfProduct)
                                        .ThenInclude(a => a.NavCategory)
                                        .FirstOrDefault(p => p.ProductId == ProductId);
            List<Category> AllCategories = dbContext.Categories
                                        .OrderBy(c => c.Name)
                                        .ToList();
            List<Category> CurrentCategories = product.CategoriesOfProduct.Select(a => a.NavCategory).ToList();
            ViewBag.UnassignedCategories = AllCategories.Except(CurrentCategories);
            return View(product);
        }

        [HttpPost("product/{pID}/AddCategory")]
        public IActionResult CategoryAdd(int pID, int CategoryId)
        {
            Association a = new Association();
            a.ProductId = pID;
            a.CategoryId = CategoryId;
            dbContext.Associations.Add(a);
            dbContext.SaveChanges();
            return Redirect($"/product/{pID}");
        }

        [HttpGet("categories")]
        public IActionResult Categories()
        {
            ViewBag.AllCategories = dbContext.Categories.OrderBy(p => p.Name).ToList();
            return View();
        }

        [HttpPost("categories/create")]
        public IActionResult CategoriesCreate(Category newC)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newC);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            }
            else
            {
                ViewBag.AllCategories = dbContext.Categories.OrderBy(C => C.Name).ToList();
                return View("Categories");
            }
        }

        [HttpGet("category/{CategoryId}")]
        public IActionResult CategoryShow(int CategoryId)
        {
            Category category = dbContext.Categories
                                        .Include(c => c.ProductsInCategory)
                                        .ThenInclude(a => a.NavProduct)
                                        .FirstOrDefault(c => c.CategoryId == CategoryId);
            List<Product> AllProducts = dbContext.Products
                                        .OrderBy(p => p.Name)
                                        .ToList();
            List<Product> CurrentProducts = category.ProductsInCategory
                                        .Select(a => a.NavProduct)
                                        .OrderBy(p => p.Name)
                                        .ToList();
            ViewBag.UnassignedProducts = AllProducts.Except(CurrentProducts);
            return View(category);
        }

        [HttpPost("category/{cID}/AddProduct")]
        public IActionResult ProductAdd(int cID, int ProductId)
        {
            Association a = new Association();
            a.CategoryId = cID;
            a.ProductId = ProductId;
            dbContext.Associations.Add(a);
            dbContext.SaveChanges();
            return Redirect($"/category/{cID}");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
