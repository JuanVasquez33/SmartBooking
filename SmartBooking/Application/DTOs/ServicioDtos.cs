using System.ComponentModel.DataAnnotations;

namespace SmartBooking.Application.DTOs
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public double Precio { get; set; }
        public int Duracion { get; set; }
        public bool Activo { get; set; }
        public int ProfesionalId { get; set; }
    }

    public class CrearServicioDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = "";

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 480, ErrorMessage = "La duración debe estar entre 1 y 480 minutos")]
        public int Duracion { get; set; }
    }

    public class EditarServicioDto : CrearServicioDto
    {
        public int Id { get; set; }
    }

    public class MisServiciosViewModel
    {
        public List<ServicioDto> Servicios { get; set; } = new();
        public string? Icono { get; set; }
        public string? CategoriaNombre { get; set; }
        public string[] Sugerencias { get; set; } = Array.Empty<string>();
        public string? Especialidad { get; set; }
    }
}
