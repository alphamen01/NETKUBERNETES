using NETKUBERNETES.Dtos.UsuariosDtos;

namespace NETKUBERNETES.Data.Usuarios
{
    public interface IUsuarioRepository
    {
        Task<UsuarioResponseDto> GetUsuario();

        Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto);

        Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto usuarioRegistroRequestDto);
    }
}
