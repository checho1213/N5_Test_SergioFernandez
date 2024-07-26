namespace N5.Infraestructure.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly N5Context context;
    public GenericRepository(N5Context context)
    {
        this.context = context;
    }

    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
        context.SaveChanges();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return context.Set<T>().Where(expression);
    }

    public IEnumerable<T> GetAll()
    {
        return context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        return context.Set<T>().Find(id);
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        context.Attach(entity);
        context.SaveChanges();
    }
}
