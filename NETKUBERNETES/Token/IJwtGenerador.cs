using NETKUBERNETES.Models;

namespace NETKUBERNETES.Token
{
    public interface IJwtGenerador
    {
        string CrearToken(Usuario usuario);
    }
}
