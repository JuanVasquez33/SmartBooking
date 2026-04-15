namespace SmartBooking.Application.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string? Telefono { get; set; }
        public string Tipo { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Auth0Sub { get; set; }

        // Solo para profesional
        public string? Especialidad { get; set; }
        public string? Descripcion { get; set; }

        // Solo para cliente
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
    }

    public class DashboardProfesionalDto
    {
        public int CitasHoy { get; set; }
        public int CitasSemana { get; set; }
        public int TotalClientes { get; set; }
        public string IngresosMes { get; set; } = "$0";
        public List<CitaResumenDto> CitasDelDia { get; set; } = new();
    }

    public class DashboardClienteDto
    {
        public int ProximasCitas { get; set; }
        public int TotalReservas { get; set; }
        public int ReservasCompletadas { get; set; }
        public int ProfesionalesFavoritos { get; set; }
        public List<ReservaResumenDto> ProximasReservas { get; set; } = new();
    }

    public class CitaResumenDto
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public string HoraInicio { get; set; }
        public string NombreCliente { get; set; }
        public string InicialCliente { get; set; }
        public string NombreServicio { get; set; }
        public string Estado { get; set; }
    }

    public class ReservaResumenDto
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public string FechaFormateada { get; set; }
        public string HoraInicio { get; set; }
        public string NombreProfesional { get; set; }
        public string InicialProfesional { get; set; }
        public string NombreServicio { get; set; }
        public string Estado { get; set; }
    }

    public class CrearUsuarioDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string Direccion { get; set; }
        public string? Telefono { get; set; }
        public string Auth0Sub { get; set; }

        // Cliente
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }

        // Profesional
        public string? Especialidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
