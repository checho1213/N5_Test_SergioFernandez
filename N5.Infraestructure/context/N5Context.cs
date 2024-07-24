namespace N5.Infraestructure.context;
public class N5Context : DbContext
{
    public N5Context(DbContextOptions<N5Context> options):base(options)
    {
        
    }
    public DbSet<Permisos>Permisos { get; set; }
    public DbSet<TiposPermisos> TiposPermisos { get; set; }
}
