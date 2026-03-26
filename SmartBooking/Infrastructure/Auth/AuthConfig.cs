using Auth0.AspNetCore.Authentication;

namespace SmartBooking.Infrastructure.Auth
{
    public static class AuthConfig
    {
        public static void AddAuth0(WebApplicationBuilder builder)
        {
            builder.Services
                .AddAuth0WebAppAuthentication(options =>
                {
                    options.Domain = builder.Configuration["Auth0:Domain"];
                    options.ClientId = builder.Configuration["Auth0:ClientId"];
                    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
                })
                .WithAccessToken(options =>
                {
                    options.Audience = "https://servicebooking.api"; // TU API
                    options.Scope = "openid profile"; // permisos se agregan vía Action
                });
        }
    }
}