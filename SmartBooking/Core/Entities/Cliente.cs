namespace SmartBooking.Core.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Notas { get; set; }
        public string? TipoDocumento { get; set; }   // CC, CE, Pasaporte
        public string? NumeroDocumento { get; set; }

        public Usuario Usuario { get; set; }
    }
}
