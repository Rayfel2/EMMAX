using Microsoft.EntityFrameworkCore;
using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface IMetodoRepository
    {
        bool save();
        ICollection<MetodoPago> GetMetodo();
        MetodoPago GetMetodo(int id);
    }
}
