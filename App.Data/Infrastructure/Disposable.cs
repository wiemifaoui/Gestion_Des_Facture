using System;

namespace App.Data.Infrastructure
{
    public class Disposable : IDisposable
    {
        bool disposed;
        ~Disposable()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                DisposeCore();
            }

            DisposeUnmanagedResources();
            disposed = true;
        }
        protected virtual void DisposeCore()
        { }
        protected virtual void DisposeUnmanagedResources()
        { }
    }
}


