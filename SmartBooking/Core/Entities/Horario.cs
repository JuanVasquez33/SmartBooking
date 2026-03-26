namespace SmartBooking.Core.Entities
{
    public class Horario
    {
        public int Id { get; set; }
        public DateOnly FechaDisponible { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public bool Disponible { get; set; } = true;
        public int ProfesionalId { get; set; }

        public Profesional Profesional { get; set; }
    }
}
