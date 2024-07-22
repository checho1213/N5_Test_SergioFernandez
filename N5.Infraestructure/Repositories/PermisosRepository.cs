using N5.Domain.Entities;
using N5.Infraestructure.context;
using N5.Infraestructure.Interfaces;

namespace N5.Infraestructure.Repositories;
public class PermisosRepository : GenericRepository<Permisos>, IPermisosRepository
{
    public PermisosRepository(N5Context context) : base(context)
    {

    }
}