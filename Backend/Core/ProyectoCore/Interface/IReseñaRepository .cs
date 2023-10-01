using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface IReseñaRepository
    {
        ICollection<Reseña> GetReseñas();
        Reseña GetReseñas(int id);
        bool save();
        
    }
}
