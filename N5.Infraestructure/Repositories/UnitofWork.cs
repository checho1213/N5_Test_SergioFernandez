using N5.Infraestructure.context;
using N5.Infraestructure.Interfaces;

namespace N5.Infraestructure.Repositories;
public class UnitOfWork : IUnitofWork
{
    private N5Context context;
    public UnitOfWork(N5Context context)
    {
        this.context = context;
        PermisosRepository = new PermisosRepository(context);
    }
    public IPermisosRepository PermisosRepository
    {
        get;
        private set;
    }
    public void Dispose()
    {
        context.Dispose();
    }
    public int Save()
    {
        return context.SaveChanges();
    }
}
