using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;

namespace ReCap.Hub.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public void RASIC<TRet>(ref TRet backingField, TRet newValue, [CallerMemberName] string propertyName = null)
            => this.RaiseAndSetIfChanged(ref backingField, newValue, propertyName);
        
        
        public virtual Type GetViewTypeName()
            => Type.GetType(GetType().FullName.Replace("ViewModel", "View"));
    }
}
