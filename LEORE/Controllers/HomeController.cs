using System.Diagnostics;
using System.Threading.Tasks;
using LEORE.Data;
using LEORE.Models;
using LEORE.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LEORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly LEOREContext _context;

        public HomeController(LEOREContext context )
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories =  await _context.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
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
