using System;
using System.Diagnostics.CodeAnalysis;

namespace Architecture2.Common
{
    public abstract class Disposable : IDisposable
    {
        private bool _disposed;

        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            DisposableExtension.PublicDispose(() => Dispose(true), this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ProtectedDispose(ref _disposed, disposing, null);
        }

        protected void EnsureNotDisposed(bool disposed)
        {
            DisposableExtension.EnsureNotDisposed(disposed, GetType());
        }

        protected void ProtectedDispose(ref bool disposed, bool disposing, Action disposingAction)
        {
            DisposableExtension.ProtectedDispose(ref disposed, disposing, disposingAction);
        }

        protected void StandardDisposeWithAction<T>(ref T obj, Action additionalAction) where T : class, IDisposable
        {
            DisposableExtension.StandardDisposeWithAction(ref obj, additionalAction);
        }

        protected void StandardDispose<T>(ref T obj) where T : class, IDisposable
        {
            DisposableExtension.StandardDispose(ref obj);
        }

    }
}
