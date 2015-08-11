using System;

namespace Architecture2.Common
{
    public static class DisposableExtension
    {
        public static void StandardDisposeWithAction<T>(ref T obj, Action additionalAction) where T : class, IDisposable
        {
            if (obj != null)
            {
                additionalAction?.Invoke();
                obj.Dispose();
                obj = null;
            }
        }

        public static void StandardDispose<T>(ref T obj) where T : class, IDisposable
        {
            StandardDisposeWithAction(ref obj, null);
        }

        public static void ProtectedDispose(ref bool disposed, bool disposing, Action disposingAction)
        {
            if (disposed)
                return;
            if (disposing)
            {
                disposingAction?.Invoke();
                disposed = true;
            }
        }

        public static void PublicDispose(Action disposeAction, object obj)
        {
            disposeAction();
            GC.SuppressFinalize(obj);
        }

        public static void EnsureNotDisposed<T>(bool disposed) where T : class
        {
            EnsureNotDisposed(disposed, typeof(T));
        }

        public static void EnsureNotDisposed(bool disposed, Type type)
        {
            if (disposed)
                throw new ObjectDisposedException(type.FullName);
        }

    }
}