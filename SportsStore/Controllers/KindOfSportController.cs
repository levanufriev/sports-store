using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class KindOfSportController : Controller
    {
        private readonly AppDbContext context;

        public KindOfSportController(AppDbContext context)
            => this.context = context;

        public async Task<IActionResult> Index()
        {
            var sports = await context.Sports.ToListAsync();
            return View(sports);
        }
    }
}
