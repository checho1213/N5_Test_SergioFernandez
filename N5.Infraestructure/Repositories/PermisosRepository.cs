namespace N5.Infraestructure.Repositories;
public class PermisosRepository : GenericRepository<Permission>, IPermisosRepository
{
    public PermisosRepository(N5Context context) : base(context)
    {

    }
}