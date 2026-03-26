namespace SmartBooking.Core.Entities
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public double Precio { get; set; }
        public int Duracion { get; set; } // minutos
        public int ProfesionalId { get; set; }
        public bool Activo { get; set; } = true;

        public Profesional Profesional { get; set; }
    }
}
