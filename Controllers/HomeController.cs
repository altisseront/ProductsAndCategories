using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;


namespace ProductsAndCategories.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult Index()
    {
        ViewBag.AllProducts = _context.Products.ToList();
        return View();
    }
    [HttpPost("product/add")]
    public IActionResult AddProduct(Product newProduct)
    {
        if(ModelState.IsValid)
        {
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            return View("Index");
        }
    }
    [HttpGet("categories")]
    public IActionResult AllCategories()
    {
        ViewBag.AllCategories = _context.Categories.ToList();
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
    [HttpPost("category/add")]
    public IActionResult AddCategory(Category newCategory)
    {
        if(ModelState.IsValid)
        {
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("AllCategories");
        }
        else
        {
            return View("Index");
        }
    }
    [HttpGet("products/{productId}")]
    public IActionResult ShowProduct(int productId)
    {
        ViewBag.ThisProduct = _context.Products.FirstOrDefault(a => a.ProductId == productId);
        ViewBag.AllCategories = _context.Categories.ToList();
        ViewBag.ThisProductsCategories = _context.Products.Include(s => s.CategoriesOfProduct).ThenInclude(d => d.Category).ToList();
        return View();
    }
    [HttpGet("categories/{categoryId}")]
    public IActionResult ShowCategory(int categoryId)
    {
        ViewBag.ThisCategory = _context.Categories.FirstOrDefault(a => a.CategoryId == categoryId);
        ViewBag.AllProducts = _context.Products.ToList();
        ViewBag.ThisCategoriesProducts = _context.Categories.Include(s => s.ProductsInCategory).ThenInclude(d => d.Product).ToList();
        return View();
    }
    [HttpPost("{productId}/addcategory")]
    public IActionResult AppendCategory(int productId, Association newAssociation)
    {
        newAssociation.ProductId = productId;
        if(ModelState.IsValid)
        {
            _context.Add(newAssociation);
            _context.SaveChanges();
            return RedirectToAction("Index");
        } else {
            return View("AppendCategory");
        }
    }
    [HttpPost("{categoryId}/addproduct")]
    public IActionResult AppendProduct(int categoryId, Association newAssociation)
    {
        newAssociation.CategoryId = categoryId;
        if(ModelState.IsValid)
        {
            _context.Add(newAssociation);
            _context.SaveChanges();
            return RedirectToAction("Index");
        } else {
            return View("AppendProduct");
        }
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
