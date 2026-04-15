namespace SmartBooking.Core.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? Notas { get; set; }
        public Usuario Usuario { get; set; }
    }
}
