namespace App.Data.Infrastructure
{
    public class UnitOfWork : Disposable, IUnitOfWork
    {
        readonly IDatabase database;
        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            database = databaseFactory.Create();
        }
        public void Commit()
        {
            database.SaveChanges();
        }
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new RepositoryBase<T>(database);
        }
        protected override void DisposeCore()
        {
            base.DisposeCore();
            database?.Dispose();
        }
    }
}
