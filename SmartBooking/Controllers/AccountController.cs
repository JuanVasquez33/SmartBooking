using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Auth0.AspNetCore.Authentication;
using SmartBooking.Application.DTOs;
using SmartBooking.Application.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace SmartBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _config;

        public AccountController(IUsuarioService usuarioService, IConfiguration config)
        {
            _usuarioService = usuarioService;
            _config = config;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string returnUrl = "/")
        {
            var props = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Callback", "Account"))
                .WithParameter("login_hint", email)
                .Build();
            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, props);
            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string nombre, string email, string password)
        {
            try
            {
                using var http = new HttpClient();
                var bodyDict = new Dictionary<string, string>
                {
                    { "client_id", _config["Auth0:ClientId"]! },
                    { "email", email },
                    { "password", password },
                    { "name", nombre },
                    { "connection", "Username-Password-Authentication" }
                };

                var response = await http.PostAsync(
                    $"https://{_config["Auth0:Domain"]}/dbconnections/signup",
                    new FormUrlEncodedContent(bodyDict)
                );

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    var errorObj = JsonSerializer.Deserialize<JsonElement>(error);
                    var mensaje = errorObj.TryGetProperty("description", out var desc)
                        ? desc.GetString()
                        : "Error al crear la cuenta. Intenta de nuevo.";
                    ViewBag.Error = mensaje;
                    return View();
                }

                var props = new LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(Url.Action("Callback", "Account"))
                    .WithParameter("login_hint", email)
                    .Build();
                await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, props);
                return new EmptyResult();
            }
            catch
            {
                ViewBag.Error = "Ocurrió un error inesperado. Intenta de nuevo.";
                return View();
            }
        }

        public async Task<IActionResult> LoginWithGoogle()
        {
            var props = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Callback", "Account"))
                .WithParameter("connection", "google-oauth2")
                .Build();
            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, props);
            return new EmptyResult();
        }

        [Authorize]
        public IActionResult Callback()
        {
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub))
                return RedirectToAction("Login");

            if (!_usuarioService.ExistePorSub(sub))
                return RedirectToAction("Completar");

            var usuario = _usuarioService.ObtenerPorSub(sub)!;
            return RedirectByRole(usuario.Tipo);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Completar() => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompletarAsync(
            string tipo, string tipoDocumento, string numeroDocumento,
            string? telefono, string? especialidad, string? descripcion, string direccion)
        {
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst("sub")?.Value;

            var nombre = User.FindFirst("name")?.Value
                      ?? User.FindFirst(ClaimTypes.Name)?.Value ?? "Usuario";

            var email = User.FindFirst(ClaimTypes.Email)?.Value
                     ?? User.FindFirst("email")?.Value ?? "";

            var dto = new CrearUsuarioDto
            {
                Nombre = nombre, Email = email, Telefono = telefono,
                Tipo = tipo, Auth0Sub = sub!,
                TipoDocumento = tipoDocumento, NumeroDocumento = numeroDocumento,
                Especialidad = especialidad, Descripcion = descripcion, Direccion = direccion
            };

            var usuario = await _usuarioService.CrearAsync(dto);
            return RedirectByRole(usuario.Tipo);
        }

        public async Task<IActionResult> Logout()
        {
            var props = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();
            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, props);
            await HttpContext.SignOutAsync("Cookies");
            return new EmptyResult();
        }

        private IActionResult RedirectByRole(string tipo) => tipo switch
        {
            "profesional" => RedirectToAction("Index", "Profesional"),
            "administrador" => RedirectToAction("Index", "Admin"),
            _ => RedirectToAction("Index", "Usuario")
        };
    }
}
