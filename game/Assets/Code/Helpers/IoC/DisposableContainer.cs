using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.Helpers
{
    public interface IDisposableContainer
    {
        List<IDisposable> Disposables { get; }
    }

    public class DisposableContainer : IDisposable, IDisposableContainer
    {
        List<IDisposable> disposables = new List<IDisposable>();
        List<IDisposable> IDisposableContainer.Disposables => this.disposables;

        public bool Disposed
        {
            get;
            private set;
        }


        public void Dispose()
        {
            this.Disposed = true;
            this.disposables.Dispose();
            this.disposables.Clear();
        }
    }
}
