using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface ICategoriaRepository
    {
        ICollection<Categorium> GetCategorias();
        Categorium GetCategorias(int id);
        bool save();
        List<int> GetCategoriaIdsByPartialNames(List<string> partialNames);
    }
}
