using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface IReciboRepository
    {
        ICollection<Recibo> GetRecibo();
        Recibo GetRecibo(int id);
        bool save();
        bool CreateRecibo(Recibo recibo);
    }
}
