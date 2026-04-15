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
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IDashboardService _dashboardService;

        public UsuarioController(
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

            // 👇 OBTENER METADATA
            var userMetadataJson = User.FindFirst("https://smartbooking.com/user_metadata")?.Value;
            var appMetadataJson = User.FindFirst("https://smartbooking.com/app_metadata")?.Value;

            // 👇 DEBUG EN CONSOLA
            Console.WriteLine("=== USER METADATA ===");
            Console.WriteLine(userMetadataJson);

            Console.WriteLine("=== APP METADATA ===");
            Console.WriteLine(appMetadataJson);

            // 👇 OPCIONAL: convertir a objeto
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

            var dashboard = _dashboardService.ObtenerDashboardCliente(usuario.Id);
            return View(dashboard);
        }
    }
}
