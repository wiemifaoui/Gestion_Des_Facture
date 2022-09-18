using Microsoft.EntityFrameworkCore;
using System;

namespace App.Data.Infrastructure
{
    public interface IDatabase : IDisposable
    {
        DbSet<T> GetDbSet<T>() where T : class;
        void MarkAsChanged<T>(T entity) where T : class;
        void SaveChanges();
    }
}
