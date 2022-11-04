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
    public class BasketController : Controller
    {
        private AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            List<BasketDetailVM> basketDetails = new List<BasketDetailVM>();

            foreach (var item in basketItems)
            {
                Product product = await _context.Products
                    .Where(m => m.Id == item.Id && m.IsDeleted == false).FirstOrDefaultAsync();

                BasketDetailVM newBasketDetails = new BasketDetailVM
                {
                    Name = product.Name,
                    Image = product.ProductImage,
                    Price = product.Price,
                    Count = item.Count,
                    Total = item.Count * product.Price
                };

                basketDetails.Add(newBasketDetails);

            }
            return View(basketDetails);
        }
    }
}
