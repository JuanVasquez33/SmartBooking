using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace SmartBooking.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login — muestra la vista personalizada
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login — redirige a Auth0 con email/password
        [HttpPost]
        public async Task<IActionResult> Login(string email, string returnUrl = "/")
        {
            var props = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .WithParameter("login_hint", email) // pre-llena el email en Auth0
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, props);
            return new EmptyResult();
        }

        // GET: /Account/LoginWithGoogle — redirige directo a Google via Auth0
        public async Task<IActionResult> LoginWithGoogle(string returnUrl = "/")
        {
            var props = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .WithParameter("connection", "google-oauth2") // fuerza conexión Google
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, props);
            return new EmptyResult();
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            var props = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, props);
            await HttpContext.SignOutAsync("Cookies");

            return new EmptyResult();
       }

        public async Task<IActionResult> Token()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(accessToken);

            // ⚡ Buscar los claims usando el namespace
            var roles = jwt.Claims
                .Where(c => c.Type == "https://miapp.com/roles")
                .Select(c => c.Value)
                .ToList();

            var permissions = jwt.Claims
                .Where(c => c.Type == "https://miapp.com/permissions")
                .Select(c => c.Value)
                .ToList();

            return Ok(new { accessToken, roles, permissions });
        }
    }
}
