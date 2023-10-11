using ProductList.DataBase;
using ProductList.Extensions;
using ProductList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace ProductList.Controllers
{
    public class ItemController : Controller
    {
        private ListContext _context;

        public ItemController(ListContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            var lista = _context.Item.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();

            if (lista.Any(x => x.Price > 0))
                TempData["value"] = $"Total value of items: {lista.Sum(x => x.Price)}";

            return View(lista);
        }

        [HttpPost]
        public IActionResult Index(string searchString)
        {
            var items = from i in _context.Item.ToList()
                        select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString) && s.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            if (items.Any(x => x.Price > 0))
            {
                TempData["value"] = $"Total value of items: {items.Sum(x => x.Price)}";
            }

            this.ShowMessage("Filtered list!");

            return View(items.ToList());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _context.Item.Find(id);

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            item.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Item.Update(item);
            _context.SaveChanges();

            this.ShowMessage("Item Updated!");

            if (_context.Item.Any(x => x.Price > 0))
                TempData["value"] = $"Total value of items: {_context.Item.Sum(x => x.Price)}";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Item item) 
        {
            item.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Item.Add(item);
            _context.SaveChanges();

            this.ShowMessage("Item registered!");

            return RedirectToAction("Register");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var item = _context.Item.Find(id);

            _context.Item.Remove(item);
            _context.SaveChanges();

            this.ShowMessage("Item removed!");

            return RedirectToAction("Index");
        }
    }
}
