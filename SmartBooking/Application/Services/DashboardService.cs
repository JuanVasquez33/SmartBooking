using Microsoft.EntityFrameworkCore;
using SmartBooking.Application.DTOs;
using SmartBooking.Application.Interfaces;
using SmartBooking.Core.Entities;
using SmartBooking.Infrastructure.Persistence;

namespace SmartBooking.Application.Services
{

    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public DashboardProfesionalDto ObtenerDashboardProfesional(int usuarioId)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var citasHoy = _context.Reservas
                .Count(r => r.ProfesionalId == usuarioId && r.Fecha == hoy);

            var citasSemana = _context.Reservas
                .Count(r => r.ProfesionalId == usuarioId &&
                            r.Fecha >= hoy &&
                            r.Fecha <= hoy.AddDays(7));

            var totalClientes = _context.Reservas
                .Where(r => r.ProfesionalId == usuarioId)
                .Select(r => r.ClienteId)
                .Distinct()
                .Count();

            var citasDelDia = _context.Reservas
                .Include(r => r.Cliente).ThenInclude(c => c.Usuario)
                .Include(r => r.Servicio)
                .Include(r => r.Horario)
                .Where(r => r.ProfesionalId == usuarioId && r.Fecha == hoy)
                .OrderBy(r => r.Horario.HoraInicio)
                .Take(5)
                .Select(r => new CitaResumenDto
                {
                    Id = r.Id,
                    Fecha = r.Fecha,
                    HoraInicio = r.Horario.HoraInicio.ToString("hh:mm tt"),
                    NombreCliente = r.Cliente.Usuario.Nombre,
                    InicialCliente = r.Cliente.Usuario.Nombre.Substring(0, 1).ToUpper(),
                    NombreServicio = r.Servicio.Nombre,
                    Estado = r.Estado.ToString()
                })
                .ToList();

            return new DashboardProfesionalDto
            {
                CitasHoy = citasHoy,
                CitasSemana = citasSemana,
                TotalClientes = totalClientes,
                IngresosMes = "$0",
                CitasDelDia = citasDelDia
            };
        }

        public DashboardClienteDto ObtenerDashboardCliente(int usuarioId)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var proximasCitas = _context.Reservas
                .Count(r => r.ClienteId == usuarioId &&
                            r.Fecha >= hoy &&
                            r.Estado != EstadoReserva.cancelada);

            var totalReservas = _context.Reservas
                .Count(r => r.ClienteId == usuarioId);

            var completadas = _context.Reservas
                .Count(r => r.ClienteId == usuarioId &&
                            r.Estado == EstadoReserva.confirmada &&
                            r.Fecha < hoy);

            var profesionalesFavoritos = _context.Reservas
                .Where(r => r.ClienteId == usuarioId)
                .Select(r => r.ProfesionalId)
                .Distinct()
                .Count();

            var proximasReservas = _context.Reservas
                .Include(r => r.Profesional).ThenInclude(p => p.Usuario)
                .Include(r => r.Servicio)
                .Include(r => r.Horario)
                .Where(r => r.ClienteId == usuarioId &&
                            r.Fecha >= hoy &&
                            r.Estado != EstadoReserva.cancelada)
                .OrderBy(r => r.Fecha)
                .Take(5)
                .Select(r => new ReservaResumenDto
                {
                    Id = r.Id,
                    Fecha = r.Fecha,
                    FechaFormateada = r.Fecha.ToString("dd MMM"),
                    HoraInicio = r.Horario.HoraInicio.ToString("hh:mm tt"),
                    NombreProfesional = r.Profesional.Usuario.Nombre,
                    InicialProfesional = r.Profesional.Usuario.Nombre.Substring(0, 1).ToUpper(),
                    NombreServicio = r.Servicio.Nombre,
                    Estado = r.Estado.ToString()
                })
                .ToList();

            return new DashboardClienteDto
            {
                ProximasCitas = proximasCitas,
                TotalReservas = totalReservas,
                ReservasCompletadas = completadas,
                ProfesionalesFavoritos = profesionalesFavoritos,
                ProximasReservas = proximasReservas
            };
        }
    }
}
