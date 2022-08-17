using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class SupplierController : Controller
    {
        private readonly AppDbContext context;

        public SupplierController(AppDbContext context)
            => this.context = context;

        public async Task<IActionResult> Index()
        {
            var suppliers = await context.Suppliers.ToListAsync();
            return View(suppliers);
        }
    }
}
