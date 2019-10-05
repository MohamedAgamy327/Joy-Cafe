namespace BLL.RepositoryService
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);

        TEntity Edit(TEntity entity);

        TEntity Remove(TEntity entity);
    }
}
