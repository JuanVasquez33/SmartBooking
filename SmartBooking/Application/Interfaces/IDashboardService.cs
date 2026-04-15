using SmartBooking.Application.DTOs;

namespace SmartBooking.Application.Interfaces
{
    public interface IDashboardService
    {
        DashboardProfesionalDto ObtenerDashboardProfesional(int usuarioId);
        DashboardClienteDto ObtenerDashboardCliente(int usuarioId);
    }
}
