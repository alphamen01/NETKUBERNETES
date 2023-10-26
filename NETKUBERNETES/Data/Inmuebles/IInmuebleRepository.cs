using NETKUBERNETES.Models;

namespace NETKUBERNETES.Data.Inmuebles
{
    public interface IInmuebleRepository
    {
        bool SaveChanges();

        IEnumerable<Inmueble> GetAllInmuebles();

        Inmueble GetInmuebleById(int id);

        Task CreateInmueble(Inmueble inmueble);

        void DeleteInmueble(int id);

    }
}
