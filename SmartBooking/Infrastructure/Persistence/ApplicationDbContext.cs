using Microsoft.EntityFrameworkCore;
using SmartBooking.Core.Entities;

namespace SmartBooking.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("usuario");
                e.HasKey(u => u.Id);
                e.Property(u => u.Id).HasColumnName("id");
                e.Property(u => u.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                e.Property(u => u.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
                e.Property(u => u.Contrasena).HasColumnName("contrasena").HasMaxLength(255);
                e.Property(u => u.Telefono).HasColumnName("telefono").HasMaxLength(20);
                e.Property(u => u.Tipo).HasColumnName("tipo").HasConversion<string>();
                e.Property(u => u.Activo).HasColumnName("activo");
                e.Property(u => u.FechaRegistro).HasColumnName("fechaRegistro");
                e.Property(u => u.Auth0Sub).HasColumnName("auth0_sub").HasMaxLength(100);
            });

            // Cliente
            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("cliente");
                e.HasKey(c => c.Id);
                e.Property(c => c.Id).HasColumnName("id");
                e.Property(c => c.FechaNacimiento).HasColumnName("fechaNacimiento");
                e.Property(c => c.Direccion).HasColumnName("direccion").HasMaxLength(255);
                e.Property(c => c.Notas).HasColumnName("notas");
                e.Property(c => c.TipoDocumento).HasColumnName("tipoDocumento").HasMaxLength(20);
                e.Property(c => c.NumeroDocumento).HasColumnName("numeroDocumento").HasMaxLength(30);
                e.HasOne(c => c.Usuario)
                    .WithOne()
                    .HasForeignKey<Cliente>(c => c.Id);
            });

            // Profesional
            modelBuilder.Entity<Profesional>(e =>
            {
                e.ToTable("profesional");
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).HasColumnName("id");
                e.Property(p => p.Especialidad).HasColumnName("especialidad").HasMaxLength(150);
                e.Property(p => p.Descripcion).HasColumnName("descripcion");
                e.Property(p => p.FotoPerfil).HasColumnName("fotoPerfil").HasMaxLength(500);
                e.Property(p => p.Direccion).HasColumnName("direccion").HasMaxLength(255);
                e.HasOne(p => p.Usuario)
                    .WithOne()
                    .HasForeignKey<Profesional>(p => p.Id);
            });

            // Servicio
            modelBuilder.Entity<Servicio>(e =>
            {
                e.ToTable("servicio");
                e.HasKey(s => s.Id);
                e.Property(s => s.Id).HasColumnName("id");
                e.Property(s => s.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
                e.Property(s => s.Descripcion).HasColumnName("descripcion");
                e.Property(s => s.Precio).HasColumnName("precio");
                e.Property(s => s.Duracion).HasColumnName("duracion");
                e.Property(s => s.ProfesionalId).HasColumnName("profesional_id");
                e.Property(s => s.Activo).HasColumnName("activo");
                e.HasOne(s => s.Profesional)
                    .WithMany(p => p.Servicios)
                    .HasForeignKey(s => s.ProfesionalId);
            });

            // Horario
            modelBuilder.Entity<Horario>(e =>
            {
                e.ToTable("horario");
                e.HasKey(h => h.Id);
                e.Property(h => h.Id).HasColumnName("id");
                e.Property(h => h.FechaDisponible).HasColumnName("fechaDisponible");
                e.Property(h => h.HoraInicio).HasColumnName("horaInicio");
                e.Property(h => h.HoraFin).HasColumnName("horaFin");
                e.Property(h => h.Disponible).HasColumnName("disponible");
                e.Property(h => h.ProfesionalId).HasColumnName("profesional_id");
                e.HasOne(h => h.Profesional)
                    .WithMany(p => p.Horarios)
                    .HasForeignKey(h => h.ProfesionalId);
            });

            // Reserva
            modelBuilder.Entity<Reserva>(e =>
            {
                e.ToTable("reserva");
                e.HasKey(r => r.Id);
                e.Property(r => r.Id).HasColumnName("id");
                e.Property(r => r.Fecha).HasColumnName("fecha");
                e.Property(r => r.Estado).HasColumnName("estado").HasConversion<string>();
                e.Property(r => r.NotasCliente).HasColumnName("notasCliente");
                e.Property(r => r.FechaCreacion).HasColumnName("fechaCreacion");
                e.Property(r => r.ClienteId).HasColumnName("cliente_id");
                e.Property(r => r.ProfesionalId).HasColumnName("profesional_id");
                e.Property(r => r.ServicioId).HasColumnName("servicio_id");
                e.Property(r => r.HorarioId).HasColumnName("horario_id");
                e.HasOne(r => r.Cliente).WithMany().HasForeignKey(r => r.ClienteId);
                e.HasOne(r => r.Profesional).WithMany().HasForeignKey(r => r.ProfesionalId);
                e.HasOne(r => r.Servicio).WithMany().HasForeignKey(r => r.ServicioId);
                e.HasOne(r => r.Horario).WithMany().HasForeignKey(r => r.HorarioId);
            });

            // Notificacion
            modelBuilder.Entity<Notificacion>(e =>
            {
                e.ToTable("notificacion");
                e.HasKey(n => n.Id);
                e.Property(n => n.Id).HasColumnName("id");
                e.Property(n => n.Mensaje).HasColumnName("mensaje");
                e.Property(n => n.Tipo).HasColumnName("tipo").HasConversion<string>();
                e.Property(n => n.Fecha).HasColumnName("fecha");
                e.Property(n => n.Leida).HasColumnName("leida");
                e.Property(n => n.UsuarioId).HasColumnName("usuario_id");
                e.Property(n => n.ReservaId).HasColumnName("reserva_id");
                e.HasOne(n => n.Usuario).WithMany().HasForeignKey(n => n.UsuarioId);
                e.HasOne(n => n.Reserva).WithMany().HasForeignKey(n => n.ReservaId);
            });
        }
    }
}
