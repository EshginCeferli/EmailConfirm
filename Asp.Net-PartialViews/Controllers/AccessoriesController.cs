using Asp.Net_PartialViews.Data;
using Asp.Net_PartialViews.Models;
using Asp.Net_PartialViews.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.Net_PartialViews.Controllers
{
    public class AccessoriesController : Controller
    {
        private AppDbContext _context;

        public AccessoriesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<Product> products = await _context.Products.Where(m => !m.IsDeleted).Take(3).ToListAsync();
            ViewBag.count = await _context.Products.Where(m => !m.IsDeleted).CountAsync();
            AccessoriesVM accessoriesVM = new AccessoriesVM
            {
                Products = products,
            };

            return View(accessoriesVM);
        }
        public async Task<IActionResult> LoadMore(int skip)
        {
            IEnumerable<Product> products = await _context.Products.Where(m => !m.IsDeleted).Skip(skip).Take(3).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            //var dbProduct = _context.Products.FirstOrDefaultAsync();
            var dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null)
            {
                return BadRequest();
            }

            List<BasketVM> baskets = GetBasket();

            Updatebasket(baskets, dbProduct.Id);


            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));
            return Json(new { succes = true, responseText = "Your messagesfuly sent!" });

        }

        private void Updatebasket(List<BasketVM> baskets, int id)
        {
            int basketCount = baskets.Count;
            basketCount = 1;

            BasketVM existProduct = baskets.Find(m => m.Id == id);

            if (existProduct != null)
            {
                existProduct.Count++;
            }
            else
            {
                baskets.Add(new BasketVM
                {
                    Id = id,
                    Count = basketCount
                });
            }
        }
        private List<BasketVM> GetBasket()
        {
            List<BasketVM> baskets;

            if (Request.Cookies["basket"] != null)
            {
                baskets = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                baskets = new List<BasketVM>();
            }

            return baskets;
        }
    }
}
