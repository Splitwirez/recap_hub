using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;

namespace ReCap.Hub
{
    public abstract class RxObjectBase : ReactiveObject
    {
        public T RASIC<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
            => this.RaiseAndSetIfChanged(ref backingField, newValue, propertyName);
        
        
        public abstract Type GetViewType();
    }
}
