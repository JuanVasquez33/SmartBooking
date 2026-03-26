namespace SmartBooking.Core.Entities
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public TipoNotificacion Tipo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool Leida { get; set; } = false;
        public int UsuarioId { get; set; }
        public int ReservaId { get; set; }

        public Usuario Usuario { get; set; }
        public Reserva Reserva { get; set; }
    }
}
