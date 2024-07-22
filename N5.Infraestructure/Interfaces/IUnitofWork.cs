namespace N5.Infraestructure.Interfaces;
public interface IUnitofWork : IDisposable
{
    IPermisosRepository PermisosRepository { get; }
}
