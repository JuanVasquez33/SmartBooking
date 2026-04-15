using Microsoft.EntityFrameworkCore;
using SmartBooking.Application.DTOs;
using SmartBooking.Application.Interfaces;
using SmartBooking.Core.Entities;
using SmartBooking.Infrastructure.Auth;
using SmartBooking.Infrastructure.Persistence;

namespace SmartBooking.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        private readonly Auth0Service _authconfig;

        public UsuarioService(ApplicationDbContext context, Auth0Service authconfig)
        {
            _context = context;
            _authconfig = authconfig;
        }

        public bool ExistePorSub(string sub)
        {
            return _context.Usuarios.Any(u => u.Auth0Sub == sub);
        }

        public UsuarioDto? ObtenerPorSub(string sub)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Auth0Sub == sub);

            if (usuario == null) return null;

            var dto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Tipo = usuario.Tipo.ToString(),
                Activo = usuario.Activo,
                FechaRegistro = usuario.FechaRegistro,
                Auth0Sub = usuario.Auth0Sub,
                TipoDocumento = usuario.TipoDocumento,
                Direccion = usuario.Direccion,
                NumeroDocumento = usuario.NumeroDocumento
            };

            if (usuario.Tipo == TipoUsuario.profesional)
            {
                var profesional = _context.Profesionales.FirstOrDefault(p => p.Id == usuario.Id);
                dto.Especialidad = profesional?.Especialidad;
                dto.Descripcion = profesional?.Descripcion;
            }

            return dto;
        }

        public async Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Tipo = Enum.Parse<TipoUsuario>(dto.Tipo),
                Auth0Sub = dto.Auth0Sub,
                Activo = true,
                FechaRegistro = DateTime.Now,
                TipoDocumento = dto.TipoDocumento,
                NumeroDocumento = dto.NumeroDocumento,
                Direccion = dto.Direccion
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            if (dto.Tipo == "cliente")
            {
                _context.Clientes.Add(new Cliente
                {
                    Id = usuario.Id,
                });
            }
            else if (dto.Tipo == "profesional")
            {
                _context.Profesionales.Add(new Profesional
                {
                    Id = usuario.Id,
                    Especialidad = dto.Especialidad,
                    Descripcion = dto.Descripcion
                });
            }

            _context.SaveChanges();
            
            await _authconfig.GuardarMetadata(dto.Auth0Sub, new
            {
                tipo = dto.Tipo,
                tipo_documento = dto.TipoDocumento,
                numero_documento = dto.NumeroDocumento,
                especialidad = dto.Especialidad
            });

            return ObtenerPorSub(dto.Auth0Sub)!;
        }
    }

}
