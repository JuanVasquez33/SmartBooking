using Microsoft.EntityFrameworkCore;
using SmartBooking.Application.DTOs;
using SmartBooking.Core.Entities;
using SmartBooking.Infrastructure.Persistence;

namespace SmartBooking.Application.Services
{
    public interface IServicioService
    {
        MisServiciosViewModel ObtenerMisServicios(int profesionalId, string? especialidad);
        ServicioDto? ObtenerPorId(int id, int profesionalId);
        ServicioDto Crear(int profesionalId, CrearServicioDto dto);
        ServicioDto Editar(int id, int profesionalId, EditarServicioDto dto);
        void Eliminar(int id, int profesionalId);
        void ToggleActivo(int id, int profesionalId);
    }

    public class ServicioService : IServicioService
    {
        private readonly ApplicationDbContext _context;

        public ServicioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public MisServiciosViewModel ObtenerMisServicios(int profesionalId, string? especialidad)
        {
            var servicios = _context.Servicios
                .Where(s => s.ProfesionalId == profesionalId)
                .OrderByDescending(s => s.Activo)
                .ThenBy(s => s.Nombre)
                .Select(s => new ServicioDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Descripcion = s.Descripcion,
                    Precio = s.Precio,
                    Duracion = s.Duracion,
                    Activo = s.Activo,
                    ProfesionalId = s.ProfesionalId
                })
                .ToList();

            var categoria = CatalogoServicios.ObtenerPorEspecialidad(especialidad)
                         ?? CatalogoServicios.Default;

            // Filtrar sugerencias que ya existen
            var nombresExistentes = servicios.Select(s => s.Nombre.ToLower()).ToHashSet();
            var sugerenciasFiltradas = categoria.Sugerencias
                .Where(s => !nombresExistentes.Contains(s.ToLower()))
                .ToArray();

            return new MisServiciosViewModel
            {
                Servicios = servicios,
                Icono = categoria.Icono,
                CategoriaNombre = categoria.Nombre,
                Sugerencias = sugerenciasFiltradas,
                Especialidad = especialidad
            };
        }

        public ServicioDto? ObtenerPorId(int id, int profesionalId)
        {
            var s = _context.Servicios
                .FirstOrDefault(s => s.Id == id && s.ProfesionalId == profesionalId);

            if (s == null) return null;

            return new ServicioDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                Precio = s.Precio,
                Duracion = s.Duracion,
                Activo = s.Activo,
                ProfesionalId = s.ProfesionalId
            };
        }

        public ServicioDto Crear(int profesionalId, CrearServicioDto dto)
        {
            var servicio = new Servicio
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion?.Trim(),
                Precio = dto.Precio,
                Duracion = dto.Duracion,
                ProfesionalId = profesionalId,
                Activo = true
            };

            _context.Servicios.Add(servicio);
            _context.SaveChanges();

            return ObtenerPorId(servicio.Id, profesionalId)!;
        }

        public ServicioDto Editar(int id, int profesionalId, EditarServicioDto dto)
        {
            var servicio = _context.Servicios
                .FirstOrDefault(s => s.Id == id && s.ProfesionalId == profesionalId)
                ?? throw new InvalidOperationException("Servicio no encontrado");

            servicio.Nombre = dto.Nombre.Trim();
            servicio.Descripcion = dto.Descripcion?.Trim();
            servicio.Precio = dto.Precio;
            servicio.Duracion = dto.Duracion;

            _context.SaveChanges();

            return ObtenerPorId(id, profesionalId)!;
        }

        public void Eliminar(int id, int profesionalId)
        {
            var servicio = _context.Servicios
                .FirstOrDefault(s => s.Id == id && s.ProfesionalId == profesionalId)
                ?? throw new InvalidOperationException("Servicio no encontrado");

            _context.Servicios.Remove(servicio);
            _context.SaveChanges();
        }

        public void ToggleActivo(int id, int profesionalId)
        {
            var servicio = _context.Servicios
                .FirstOrDefault(s => s.Id == id && s.ProfesionalId == profesionalId)
                ?? throw new InvalidOperationException("Servicio no encontrado");

            servicio.Activo = !servicio.Activo;
            _context.SaveChanges();
        }
    }
}
