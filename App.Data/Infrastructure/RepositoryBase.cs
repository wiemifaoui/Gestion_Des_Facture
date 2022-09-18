using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Data.Infrastructure
{
    class RepositoryBase<T> : IRepository<T> where T : class
    {
        readonly IDatabase database;
        readonly DbSet<T> dbSet;
        public RepositoryBase(IDatabase database)
        {
            this.database = database;
            dbSet = database.GetDbSet<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public void Delete(Expression<Func<T, bool>> where)
        {
            dbSet.RemoveRange(dbSet.Where(where));
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }
        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }
        public T GetById(object id)
        {
            return dbSet.Find(id);
        }
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }
        public void Update(T entity)
        {
            dbSet.Attach(entity);
            database.MarkAsChanged(entity);
        }
    }
}
