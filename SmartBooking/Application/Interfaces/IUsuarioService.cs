using SmartBooking.Application.DTOs;

namespace SmartBooking.Application.Interfaces
{
    public interface IUsuarioService
    {
        /// <summary>Busca un usuario por su Auth0 sub</summary>
        UsuarioDto? ObtenerPorSub(string sub);

        /// <summary>Crea el usuario en la BD local tras el registro en Auth0</summary>
        Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto);

        /// <summary>Verifica si un usuario ya completó su perfil</summary>
        bool ExistePorSub(string sub);
    }


}
