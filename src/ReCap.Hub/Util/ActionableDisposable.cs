using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReCap.Hub.Data;

namespace ReCap.Hub
{
    public abstract class EventDisposableBase : IDisposable
    {
        public virtual void Dispose()
        {
            Disposed?.Invoke(this, new EventArgs());
        }
        public event EventHandler Disposed;
    }
    public class ActionableDisposable : EventDisposableBase
    {
        Action _onDisposed = null;
        public ActionableDisposable(Action onDisposed)
        {
            _onDisposed = onDisposed;
        }


        public override void Dispose()
        {
            _onDisposed();
            base.Dispose();
        }
    }
}
