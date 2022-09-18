using App.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Service.Infrastructure
{
    public class ServiceBase<T> : IService<T> where T : class
    {
        public IUnitOfWork UnitOfWork { get; private set; }
        readonly IRepository<T> repository;
        public ServiceBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            repository = unitOfWork.GetRepository<T>();
        }
        public void Add(T entity)
        {
            repository.Add(entity);
        }
        public void Delete(T entity)
        {
            repository.Delete(entity);
        }
        public void Delete(Expression<Func<T, bool>> where)
        {
            repository.Delete(where);
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return repository.Get(where);
        }
        public IEnumerable<T> GetAll()
        {
            return repository.GetAll();
        }
        public T GetById(object id)
        {
            return repository.GetById(id);
        }
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return repository.GetMany(where);
        }
        public void Update(T entity)
        {
            repository.Update(entity);
        }
    }
}
