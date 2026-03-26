namespace SmartBooking.Core.Entities
{
    public class Profesional
    {
        public int Id { get; set; }
        public string? Especialidad { get; set; }
        public string? Descripcion { get; set; }
        public string? FotoPerfil { get; set; }
        public string? Direccion { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
        public ICollection<Horario> Horarios { get; set; } = new List<Horario>();
    }
}
