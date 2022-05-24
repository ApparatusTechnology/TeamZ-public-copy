using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamZ.Helpers
{
    public static class DisposableExtensions
    {
        public static void Dispose(this IEnumerable<IDisposable> disposables)
        {
            foreach (var disposable in disposables.Where(o => o != null))
            {
                disposable.Dispose();
            }
        }

        public static TValue DisposeWith<TValue>(this TValue value, IDisposableContainer container)
            where TValue : IDisposable
        {
            container.Disposables.Add(value);
            return value;
        }
    }
}
