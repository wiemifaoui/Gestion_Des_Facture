using App.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Service.Infrastructure
{
    public interface IService<T> where T : class
    {
        IUnitOfWork UnitOfWork { get; }
        T GetById(object id);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
    }
}
