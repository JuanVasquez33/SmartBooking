namespace SmartBooking.Core.Entities
{
    public class AppLog
    {
        public int Id { get; set; }
        public string Nivel { get; set; } = string.Empty;
        public string Proceso { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
