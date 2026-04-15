using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBooking.Application.Interfaces;
using SmartBooking.Core.Entities;
using SmartBooking.Infrastructure.Persistence;
using System.Security.Claims;
using System.Text.Json;

namespace SmartBooking.Controllers
{
    [Authorize]
    public class ProfesionalController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IDashboardService _dashboardService;

        public ProfesionalController(
            IUsuarioService usuarioService,
            IDashboardService dashboardService)
        {
            _usuarioService = usuarioService;
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst("sub")?.Value;

            // OBTENER METADATA
            var userMetadataJson = User.FindFirst("https://smartbooking.com/user_metadata")?.Value;

            if (!string.IsNullOrEmpty(userMetadataJson))
            {
                var userMetadata = JsonSerializer.Deserialize<Dictionary<string, object>>(userMetadataJson);

                foreach (var item in userMetadata)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
            }

            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var usuario = _usuarioService.ObtenerPorSub(sub!);
            if (usuario == null) return RedirectToAction("Completar", "Account");

            var dashboard = _dashboardService.ObtenerDashboardProfesional(usuario.Id);
            return View(dashboard);
        }
    }

}
