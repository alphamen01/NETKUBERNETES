using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETKUBERNETES.Dtos.UsuariosDtos;
using NETKUBERNETES.Middleware;
using NETKUBERNETES.Models;
using NETKUBERNETES.Token;
using System.Net;

namespace NETKUBERNETES.Data.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IJwtGenerador _jwtGenerador;
        private readonly AppDbContext _appDbContext;
        private readonly IUsuarioSesion _usuarioSesion;

        public UsuarioRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador, AppDbContext appDbContext, IUsuarioSesion usuarioSesion)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerador = jwtGenerador;
            _appDbContext = appDbContext;
            _usuarioSesion = usuarioSesion;
            
        }

        private UsuarioResponseDto TransformerUserToUserDto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Token = _jwtGenerador.CrearToken(usuario)
            };
        }

        public async Task<UsuarioResponseDto> GetUsuario()
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            if (usuario is null)
            {
                throw new MiddlewareException(HttpStatusCode.Unauthorized, new {mensaje = "El usuario del token no existe en la base de datos"});
            }

            var usuarioDto = TransformerUserToUserDto(usuario!);

            return usuarioDto;
        }

        public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto  usuarioLoginRequestDto)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioLoginRequestDto.Email!);

            if (usuario is null)
            {
                throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El email del usuario no existe en la base de datos" });
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario!, usuarioLoginRequestDto.Password!, false);

            if (resultado.Succeeded)
            {
                return TransformerUserToUserDto(usuario);
            }

            throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "Las credenciales son incorrectas" });
        }

        public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto usuarioRegistroRequestDto)
        {
            var existeEmail = await _appDbContext.Users.Where(x => x.Email == usuarioRegistroRequestDto.Email).AnyAsync();

            if (existeEmail)
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El Email del usuario ya existe en la base de datos" });
            }

            var existeUserName = await _appDbContext.Users.Where(x => x.UserName == usuarioRegistroRequestDto.UserName).AnyAsync();

            if (existeUserName)
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El UserName del usuario ya existe en la base de datos" });
            }

            var usuario = new Usuario
            {
                Nombre = usuarioRegistroRequestDto.Nombre,
                Apellido = usuarioRegistroRequestDto.Apellido,
                Telefono = usuarioRegistroRequestDto.Telefono,
                Email = usuarioRegistroRequestDto.Email,
                UserName = usuarioRegistroRequestDto.UserName,

            };

            var resultado = await _userManager.CreateAsync(usuario!, usuarioRegistroRequestDto.Password!);
            if (resultado.Succeeded)
            {
                return TransformerUserToUserDto(usuario);
            }

            throw new Exception("No se pudo registrar el usuario");
        }
    }
}
