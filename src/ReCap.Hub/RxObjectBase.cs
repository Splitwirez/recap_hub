using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;

namespace ReCap.Hub
{
    public abstract class RxObjectBase : ReactiveObject
    {
        public void RASIC<TRet>(ref TRet backingField, TRet newValue, [CallerMemberName] string propertyName = null)
            => this.RaiseAndSetIfChanged(ref backingField, newValue, propertyName);
        
        
        public abstract Type GetViewType();
    }
}
