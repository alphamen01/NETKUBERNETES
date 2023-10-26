using Microsoft.AspNetCore.Identity;
using NETKUBERNETES.Models;
using NETKUBERNETES.Token;

namespace NETKUBERNETES.Data.Inmuebles
{
    public class InmuebleRepository : IInmuebleRepository
    {
        private readonly AppDbContext _contexto;

        private readonly IUsuarioSesion _usuarioSesion;

        private readonly UserManager<Usuario> _userManager;

        public InmuebleRepository(AppDbContext contexto, IUsuarioSesion usuarioSesion, UserManager<Usuario> userManager)
        {
            _contexto = contexto;
            _usuarioSesion = usuarioSesion;
            _userManager = userManager;
        }
        public async Task CreateInmueble(Inmueble inmueble)
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            inmueble.FechaCreacion = DateTime.Now;
            inmueble.UsuarioId = Guid.Parse(usuario!.Id);

            _contexto.Inmuebles!.Add(inmueble);
        }

        public void DeleteInmueble(int id)
        {
            var inmueble = _contexto.Inmuebles!.FirstOrDefault(x => x.Id == id);

            _contexto.Inmuebles!.Remove(inmueble!);
        }

        public IEnumerable<Inmueble> GetAllInmuebles()
        {
            var listInmuebles = _contexto.Inmuebles!.ToList();

            return listInmuebles;
        }

        public Inmueble GetInmuebleById(int id)
        {
            var inmueble = _contexto.Inmuebles!.FirstOrDefault(x => x.Id == id);

            return inmueble!;
        }

        public bool SaveChanges()
        {
            return (_contexto.SaveChanges() >= 0);
        }
    }
}
