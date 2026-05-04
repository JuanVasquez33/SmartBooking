using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooking.Application.DTOs;
using SmartBooking.Application.Interfaces;
using SmartBooking.Application.Services;
using System.Security.Claims;

namespace SmartBooking.Controllers
{
    [Authorize]
    public class ServicioController : Controller
    {
        private readonly IServicioService _servicioService;
        private readonly IUsuarioService _usuarioService; 
        private readonly KafkaProducerService _kafka;

        public ServicioController(IServicioService servicioService, IUsuarioService usuarioService, KafkaProducerService kafka)
        {
            _servicioService = servicioService;
            _usuarioService = usuarioService;
            _kafka = kafka;
        }

        private UsuarioDto? GetUsuarioActual()
        {
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst("sub")?.Value;
            return _usuarioService.ObtenerPorSub(sub!);
        }

        // GET: /Servicio
        public IActionResult Index()
        {
            var usuario = GetUsuarioActual();
            if (usuario == null) return RedirectToAction("Completar", "Account");

            var vm = _servicioService.ObtenerMisServicios(usuario.Id, usuario.Especialidad);
            return View(vm);
        }

        // POST: /Servicio/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(CrearServicioDto dto)
        {
            var usuario = GetUsuarioActual();
            if (usuario == null) return RedirectToAction("Completar", "Account");

            if (!ModelState.IsValid)
            {
                var vm = _servicioService.ObtenerMisServicios(usuario.Id, usuario.Especialidad);
                TempData["Error"] = "Por favor completa todos los campos requeridos.";
                return View("Index", vm);
            }

            _servicioService.Crear(usuario.Id, dto);
            TempData["Exito"] = $"Servicio '{dto.Nombre}' creado exitosamente.";
            return RedirectToAction("Index");
        }

        // POST: /Servicio/CrearDesdesugerencia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearDesdeSugerencia(string nombre)
        {
            var usuario = GetUsuarioActual();
            if (usuario == null) return RedirectToAction("Completar", "Account");

            // Retorna un partial con el formulario prellenado
            TempData["NombreSugerido"] = nombre;
            return RedirectToAction("Index");
        }

        // POST: /Servicio/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarServicioDto dto)
        {
            var usuario = GetUsuarioActual();
            if (usuario == null) return RedirectToAction("Completar", "Account");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor completa todos los campos requeridos.";
                return RedirectToAction("Index");
            }

            _servicioService.Editar(dto.Id, usuario.Id, dto);
      
            await _kafka.PublicarAsync(
                nivel: "INFO",
                proceso: "ServicioController.Editar",
                mensaje: $"Servicio '{dto.Nombre}' (Id: {dto.Id}) editado",
                usuarioId: usuario.Id
            );
            
            TempData["Exito"] = "Servicio actualizado exitosamente.";
            return RedirectToAction("Index");
        }

        // POST: /Servicio/Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            var usuario = GetUsuarioActual();
            if (usuario == null) return RedirectToAction("Completar", "Account");

            _servicioService.Eliminar(id, usuario.Id);
            TempData["Exito"] = "Servicio eliminado.";
            return RedirectToAction("Index");
        }

        // POST: /Servicio/ToggleActivo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActivo(int id)
        {
            var usuario = GetUsuarioActual();
            if (usuario == null) return RedirectToAction("Completar", "Account");

            _servicioService.ToggleActivo(id, usuario.Id);
            return RedirectToAction("Index");
        }
    }
}
