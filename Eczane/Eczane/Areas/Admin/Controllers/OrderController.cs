using Eczane.Data;
using Eczane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Eczane.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] //sadece adminin erişebilmesi için Authorize işlemi 
    public class OrdersController : Controller //sipariş yönetiminin gerçekleştirilmesi
    {
     
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrdersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> complete(string id) 
        {
            Order order = await _context.Orders.FindAsync(id);
            if (order != null && order.isCompleted == false)
            {
                order.isCompleted = true;
            }
            else if (order != null)
            {
                order.isCompleted = false;
            }

            _context.Update(order);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Orders.Count() / pageSize);

            return View(await _context.Orders.ToListAsync());


         
        }
        public async Task<IActionResult> SortCompleted(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Orders.Count() / pageSize);

            return View("Index", await _context.Orders.Where(o => o.isCompleted == true).ToListAsync());
        }

        public async Task<IActionResult> SortIncomplete(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Orders.Count() / pageSize);

            return View("Index", await _context.Orders.Where(o => o.isCompleted == false).ToListAsync());
        }
    }
}
