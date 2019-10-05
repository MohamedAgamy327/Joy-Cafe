using System.Data.Entity;

namespace BLL.RepositoryService
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> GeneralDBContext;

        public GenericRepository(DbContext context)
        {
            Context = context;
            GeneralDBContext = context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            return GeneralDBContext.Add(entity);
        }

        public TEntity Edit(TEntity entity)
        {
            GeneralDBContext.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public TEntity Remove(TEntity entity)
        {
            GeneralDBContext.Attach(entity);
            GeneralDBContext.Remove(entity);
            return entity;
        }

    }
}
