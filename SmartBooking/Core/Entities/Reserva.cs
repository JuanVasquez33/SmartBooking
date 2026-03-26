namespace SmartBooking.Core.Entities
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public EstadoReserva Estado { get; set; } = EstadoReserva.pendiente;
        public string? NotasCliente { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int ClienteId { get; set; }
        public int ProfesionalId { get; set; }
        public int ServicioId { get; set; }
        public int HorarioId { get; set; }

        public Cliente Cliente { get; set; }
        public Profesional Profesional { get; set; }
        public Servicio Servicio { get; set; }
        public Horario Horario { get; set; }
    }
}
