using Microsoft.AspNetCore.Mvc;
using SmartBooking.Infrastructure.Persistence;

namespace SmartBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                ViewBag.DbStatus = canConnect ? "✅ Conexión exitosa" : "❌ No se pudo conectar";
            }
            catch (Exception ex)
            {
                ViewBag.DbStatus = $"❌ Error: {ex.Message}";
            }

            return View();
        }
    }
}