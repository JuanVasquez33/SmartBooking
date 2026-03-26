namespace SmartBooking.Core.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string? Contrasena { get; set; }
        public string? Telefono { get; set; }
        public TipoUsuario Tipo { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string? Auth0Sub { get; set; } // ID de Auth0 ej: "google-oauth2|123456"
    }
}
